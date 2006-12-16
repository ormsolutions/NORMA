#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

// Turn this on to block hiding of shapes implied by an objectification pattern
//#define SHOW_IMPLIED_SHAPES
// Turn this on to show a fact shape instead of a subtype link for all SubtypeFacts
//#define SHOW_FACTSHAPE_FOR_SUBTYPE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling.Design;
namespace Neumont.Tools.ORM.ShapeModel
{
	#region IStickyObject interface
	/// <summary>
	/// Interface for implementing "Sticky" selections.  Presentation elements that are sticky
	/// will maintain their selected status when compatible objects are clicked.
	/// </summary>
	public interface IStickyObject
	{
		/// <summary>
		/// Call this on an object when you're setting it as a StickyObject.  This method
		/// will go through the object's associated elements to perform any actions needed
		/// such as calling Invalidate().
		/// </summary>
		void StickyInitialize();
		/// <summary>
		/// Returns whether the Presentation Element that was passed in is selectable in the
		/// context of this StickyObject.  For example, when an external constraint is the
		/// active StickyObject, roles are selectable and objects are not.
		/// </summary>
		/// <returns>Whether the PresentationElement passed in is selectable in this StickyObject's context</returns>
		bool StickySelectable(ModelElement mel);
		/// <summary>
		/// Needed to allow outside entities to tell the StickyObject to redraw itself and its children.
		/// </summary>
		void StickyRedraw();
	}
	#endregion // IStickyObject interface

	// NOTE: ORMDiagram must be the first class in this file or ORMDiagram.resx will end up with the wrong name in the assembly
	[ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramDefaultFilterString, ToolboxItemFilterType.Require)]
	public partial class ORMDiagram : IProxyDisplayProvider
	{
		#region Constructors
		/// <summary>Constructor.</summary>
		/// <param name="store"><see cref="Store"/> where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public ORMDiagram(Store store, params PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
			// This constructor calls our other constructor which takes a Partition.
			// All work should be done there rather than here.
		}
		/// <summary>Constructor.</summary>
		/// <param name="partition"><see cref="Partition"/> where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public ORMDiagram(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			//turned snap to grid off because we are aligning the facttypes based
			//on the center of the roles. Since the center of the roles is not necessarily
			//going to be located in alignment on the grid we had to turn this off so facttypes
			//would get properly aligned with other objects.
			base.SnapToGrid = false;
			base.Name = ResourceStrings.DiagramCommandNewPage.Replace("&", "");
		}
		#endregion
		#region DragDrop overrides
		/// <summary>
		/// Check to see if <see cref="DiagramDragEventArgs.Data">dragged object</see> is a type that can be dropped on the <see cref="Diagram"/>,
		/// if so change <see cref="DiagramDragEventArgs.Effect"/>.
		/// </summary>
		public override void OnDragOver(DiagramDragEventArgs e)
		{
			string[] dataFormats = e.Data.GetFormats();
			if (Array.IndexOf(dataFormats, typeof(ObjectType).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(FactType).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(SetComparisonConstraint).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(SetConstraint).FullName) >= 0 ||
				Array.IndexOf(dataFormats, typeof(ModelNote).FullName) >= 0)
			{
				e.Effect = DragDropEffects.All;
				e.Handled = true;
			}
			base.OnDragOver(e);
		}
		/// <summary>
		/// check to see if dragged object is a type that can be dropped on the diagram, if so allow it to be added to form by calling FixUpDiagram
		/// </summary>
		/// <param name="e"></param>
		public override void OnDragDrop(DiagramDragEventArgs e)
		{
			IDataObject dataObject = e.Data;
			if (dataObject == null)
			{
				return;
			}
			if (PlaceORMElementOnDiagram(dataObject, null, e.MousePosition, true))
			{
				e.Effect = DragDropEffects.All;
				e.Handled = true;
			}
			base.OnDragDrop(e);
		}
		/// <summary>
		/// Place a new shape for an existing element onto this diagram
		/// </summary>
		/// <param name="dataObject">The dataObject containing the element to place. If this is set, elementToPlace must be null.</param>
		/// <param name="elementToPlace">The the element to place. If this is set, dataObject must be null.</param>
		/// <param name="elementPosition">An initial position for the element</param>
		/// <param name="selectIfNotAdded">Select the object on the diagram if it is already there.</param>
		/// <returns>true if the element was placed</returns>
		public bool PlaceORMElementOnDiagram(IDataObject dataObject, ModelElement elementToPlace, PointD elementPosition, bool selectIfNotAdded)
		{
			Debug.Assert((dataObject == null) ^ (elementToPlace == null), "Pass in dataObject or elementToPlace");
			bool retVal = false;
			ObjectType objectType = null;
			FactType factType = null;
			SetComparisonConstraint multiCol = null;
			SetConstraint singleCol = null;
			ModelNote modelNote = null;
			ModelElement element = null;
			LinkedElementCollection<FactType> verifyFactTypeList = null;
			if (null != (objectType = (dataObject == null) ? elementToPlace as ObjectType : dataObject.GetData(typeof(ObjectType)) as ObjectType))
			{
				element = objectType;
			}
			else if (null != (factType = (dataObject == null) ? elementToPlace as FactType : dataObject.GetData(typeof(FactType)) as FactType))
			{
				element = factType;
			}
			else if (null != (multiCol = (dataObject == null) ? elementToPlace as SetComparisonConstraint : dataObject.GetData(typeof(SetComparisonConstraint)) as SetComparisonConstraint))
			{
				verifyFactTypeList = multiCol.FactTypeCollection;
				element = multiCol;
			}
			else if (null != (singleCol = (dataObject == null) ? elementToPlace as SetConstraint : dataObject.GetData(typeof(SetConstraint)) as SetConstraint))
			{
				verifyFactTypeList = singleCol.FactTypeCollection;
				element = singleCol;
			}
			else if (null != (modelNote = (dataObject == null) ? elementToPlace as ModelNote : dataObject.GetData(typeof(ModelNote)) as ModelNote))
			{
				element = modelNote;
			}
			if (verifyFactTypeList != null)
			{
				int factsRemaining = verifyFactTypeList.Count;
				if (factsRemaining != 0)
				{
					ModelElement testElement = element;
					element = null;
					bool[] factsContained = new bool[factsRemaining];
					FactType fact;
					foreach (ShapeElement shape in NestedChildShapes)
					{
						if (null != (fact = shape.ModelElement as FactType))
						{
							int index = verifyFactTypeList.IndexOf(fact);
							if (index != -1)
							{
								if (!factsContained[index])
								{
									factsContained[index] = true;
									--factsRemaining;
									if (factsRemaining == 0)
									{
										element = testElement;
										break;
									}
								}
							}
						}
					}
				}
			}
			if (element != null)
			{
				retVal = true;
				bool storeChange = false;

				using (Transaction transaction = Store.TransactionManager.BeginTransaction(ResourceStrings.DropShapeTransactionName))
				{
					bool clearContext;
					if (clearContext = !elementPosition.IsEmpty)
					{
						DropTargetContext.Set(transaction.TopLevelTransaction, Id, elementPosition, null);
					}
					FixUpLocalDiagram(null, element);
					if (clearContext)
					{
						DropTargetContext.Remove(transaction.TopLevelTransaction);
					}
					DropTargetContext.Remove(transaction.TopLevelTransaction);
					if (factType != null && factType.RoleCollection != null)
					{
						LinkedElementCollection<RoleBase> roleCollection = factType.RoleCollection;
						for (int i = 0; i < roleCollection.Count; i++)
						{
							//Role role = roleBase.Role;
							Role role = roleCollection[i].Role;
							// Pick up role players
							FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId));

							// Pick up attached constraints
							FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(role, FactSetConstraint.FactTypeDomainRoleId));
							FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(role, FactSetComparisonConstraint.FactTypeDomainRoleId));

							// Pick up the role shape
							FixUpLocalDiagram(factType, role);

							// Get the role value constraint and the link to it.
							RoleHasValueConstraint valueConstraintLink = RoleHasValueConstraint.GetLinkToValueConstraint(role);

							if (valueConstraintLink != null)
							{
								FixUpLocalDiagram(factType, valueConstraintLink.ValueConstraint);
							}

						}
						LinkedElementCollection<ReadingOrder> orders = factType.ReadingOrderCollection;
						if (orders.Count != 0)
						{
							FixUpLocalDiagram(factType, orders[0]);
						}
						FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(factType, ModelNoteReferencesFactType.ElementDomainRoleId));
						Objectification objectification = factType.Objectification;
						if (objectification != null && !objectification.IsImplied)
						{
							ObjectType nestingType = objectification.NestingType;
							FixUpLocalDiagram(factType, nestingType);
							FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(nestingType, ModelNoteReferencesObjectType.ElementDomainRoleId));
						}
					}
					else if (objectType != null)
					{
						ReadOnlyCollection<ObjectTypePlaysRole> rolePlayerLinks = DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(objectType, ObjectTypePlaysRole.RolePlayerDomainRoleId);
						int linksCount = rolePlayerLinks.Count;
						for (int i = 0; i < linksCount; ++i)
						{
							ObjectTypePlaysRole link = rolePlayerLinks[i];
							Role role = link.PlayedRole;
							SubtypeMetaRole subRole;
							SupertypeMetaRole superRole;
							FactType subtypeFact = null;
							if (null != (subRole = role as SubtypeMetaRole))
							{
								subtypeFact = role.FactType;
							}
							else if (null != (superRole = role as SupertypeMetaRole))
							{
								subtypeFact = role.FactType;
							}
							if (subtypeFact != null)
							{
								FixUpLocalDiagram(null, subtypeFact);
							}
							else
							{
								FixUpLocalDiagram(null, link);
							}
						}
						ValueConstraint valueConstraint = objectType.FindValueConstraint(false);
						if (valueConstraint != null)
						{
							FixUpLocalDiagram(objectType, valueConstraint);
						}
						FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(objectType, ModelNoteReferencesObjectType.ElementDomainRoleId));
					}
					else if (singleCol != null)
					{
						FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(singleCol, FactSetConstraint.SetConstraintDomainRoleId));
					}
					else if (multiCol != null)
					{
						FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(multiCol, FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId));
					}
					else if (modelNote != null)
					{
						FixupRelatedLinks(null, DomainRoleInfo.GetElementLinks<ElementLink>(modelNote, ModelNoteReferencesModelElement.NoteDomainRoleId));
					}
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
						storeChange = true;
					}
				}
				if (!storeChange && selectIfNotAdded)
				{
					DiagramView selectOnView;
					ShapeElement shape;
					if (null != (selectOnView = ActiveDiagramView) &&
						null != (shape = FindShapeForElement(element)))
					{
						selectOnView.Selection.Set(new DiagramItem(shape));
						selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
					}
				}
			} 
			return retVal;
		}
		/// <summary>
		/// Fixes up the local diagram for each of the links related to the specified ModelElement.
		/// </summary>
		/// <param name="droppedOnElement">The dropped on element.</param>
		/// <param name="links">The links.</param>
		private void FixupRelatedLinks(ModelElement droppedOnElement, ReadOnlyCollection<ElementLink> links)
		{
			int linksCount = links.Count;
			for (int i = 0; i < linksCount; ++i)
			{
				FixUpLocalDiagram(droppedOnElement, links[i]);
			}
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram.
		/// </summary>
		/// <param name="existingParent">An element with a shape on this diagram.
		/// Pass in null to use the model.</param>
		/// <param name="newChild">The new element to add.</param>
		public void FixUpLocalDiagram(ModelElement existingParent, ModelElement newChild)
		{
			ShapeElement parentShape = this;
			if (existingParent != null && existingParent != ModelElement)
			{
				parentShape = FindShapeForElement(existingParent);
				if (parentShape == null)
				{
					return;
				}
			}
			ShapeElement newChildShape = parentShape.FixUpChildShapes(newChild);
			if (newChildShape != null && newChildShape.Diagram == this)
			{
				FixUpDiagramSelection(newChildShape);
			}
		}
		# endregion // DragDrop overrides
		#region Toolbox filter strings
		// UNDONE: 2006-06 DSL Tools port: Some of these toolbox filter strings have been changed to point to the filter strings
		// in ToolboxHelper. Is this the correct thing to do, and does anything else need to be done? (The original versions of
		// the changed filter strings are below, commented out.)
		/// <summary>
		/// The filter string used for simple actions
		/// </summary>
		public const string ORMDiagramDefaultFilterString = ORMShapeToolboxHelper.ToolboxFilterString;
		//public const string ORMDiagramDefaultFilterString = "ORMDiagramDefaultFilterString";

		/// <summary>
		/// The filter string used to create an external constraint. Very similar to a
		/// normal action, except the external constraint connector is activated on completion
		/// of the action.
		/// </summary>
		public const string ORMDiagramExternalConstraintFilterString = "ORMDiagramExternalConstraintFilterString";
		/// <summary>
		/// The filter string used to connect role sequences to external constraints
		/// </summary>
		public const string ORMDiagramConnectExternalConstraintFilterString = ORMShapeToolboxHelper.ExternalConstraintConnectorFilterString;
		//public const string ORMDiagramConnectExternalConstraintFilterString = "ORMDiagramConnectExternalConstraintFilterString";
		/// <summary>
		/// The filter string used to create subtype relationships between object types
		/// </summary>
		public const string ORMDiagramCreateSubtypeFilterString = ORMShapeToolboxHelper.SubtypeConnectorFilterString;
		//public const string ORMDiagramCreateSubtypeFilterString = "ORMDiagramCreateSubtypeFilterString";
		/// <summary>
		/// The filter string used to create an internal constraint. Very similar to a
		/// normal action, except the internal constraint connector is activated on completion
		/// of the action.
		/// </summary>
		public const string ORMDiagramInternalUniquenessConstraintFilterString = "ORMDiagramInternalUniquenessConstraintFilterString";
		/// <summary>
		/// The filter string used to connect role sequences to internal uniqueness constraints
		/// </summary>
		public const string ORMDiagramConnectInternalUniquenessConstraintFilterString = "ORMDiagramConnectInternalUniquenessConstraintFilterString";
		/// <summary>
		/// The filter string used to connect a role to its role player object type
		/// </summary>
		public const string ORMDiagramConnectRoleFilterString = ORMShapeToolboxHelper.RoleConnectorFilterString;
		//public const string ORMDiagramConnectRoleFilterString = "ORMDiagramConnectRoleFilterString";
		/// <summary>
		/// The filter string used to create a model note. Very similar to a
		/// normal action, except the model note property editor is activated on
		/// completion of the action.
		/// </summary>
		public const string ORMDiagramModelNoteFilterString = "ORMDiagramModelNoteFilterString";
		/// <summary>
		/// The filter string used to associate a model note with other model element
		/// </summary>
		public const string ORMDiagramConnectModelNoteFilterString = ORMShapeToolboxHelper.ModelNoteConnectorFilterString;
		#endregion // Toolbox filter strings
		#region StickyEditObject
		/// <summary>
		/// The StickyObject associated with this diagram.  
		/// </summary>
		[NonSerialized]
		private IStickyObject mySticky;
		/// <summary>
		/// Get access to the diagram's StickyObject
		/// </summary>
		/// <value>StickyObject</value>
		public IStickyObject StickyObject
		{
			get
			{
				return mySticky;
			}
			set
			{
				// If the previous StickyObject was a ShapeElement, invalidate it so that it can redraw.
				// This is because a sticky ShapeElement should give a visual indicator that it's active.

				// Need to account for: going from null to ShapeElement, ShapeElement to null, ShapeElement to ShapeElement
				IStickyObject currentStickyShape;
				IStickyObject incomingStickyShape;

				currentStickyShape = mySticky;
				incomingStickyShape = value;
				if (currentStickyShape != null)
				{
					mySticky = null;
					currentStickyShape.StickyRedraw();
				}

				if (incomingStickyShape != null)
				{
					mySticky = value;
					mySticky.StickyInitialize();
				}
			}
		}
		#endregion //StickyEditObject
		#region View Fixup Methods
		/// <summary>
		/// Called as a result of the FixUpDiagram calls
		/// with the diagram as the first element.
		/// </summary>
		/// <param name="element">Added element</param>
		/// <returns>True for items displayed directly on the
		/// surface. Nesting object types are not displayed.</returns>
		protected override bool ShouldAddShapeForElement(ModelElement element)
		{
			ElementLink link = element as ElementLink;
			SubtypeFact subtypeFact = null;
			ModelElement element1 = null;
			ModelElement element2 = null;
			if (link != null)
			{
				element1 = DomainRoleInfo.GetSourceRolePlayer(link);
				Role role1 = element1 as Role;
				if (role1 != null)
				{
					element1 = role1.FactType;
				}
				element2 = DomainRoleInfo.GetTargetRolePlayer(link);
				Role role2 = element2 as Role;
				if (role2 != null)
				{
					element2 = role2.FactType;
				}
			}
			else if ((subtypeFact = element as SubtypeFact) != null)
			{
				element1 = subtypeFact.Subtype;
				element2 = subtypeFact.Supertype;
			}

			bool isLink = link != null || subtypeFact != null;
			if (isLink && (element1 == null || element2 == null || FindShapeForElement(element1) == null || FindShapeForElement(element2) == null))
			{
				return false;
			}
			else if (!isLink && !this.AutoPopulateShapes)
			{
				// Note that this used to be the following, but we can't rely on ActiveDiagramView
				// to be null when the diagram is visible in an inactive window.
				// MSBUG ActiveDiagramView should be null if the containing window is not active.
				//else if (!isLink && (!this.AutoPopulateShapes && this.ActiveDiagramView == null))
				DiagramView activeDiagramView = this.ActiveDiagramView;
				if (activeDiagramView == null)
				{
					return false;
				}
				else
				{
					// If the diagram has an active view on it, then we
					// need to make sure that the view belongs to the currently
					// active document. Otherwise, with multiple windows open
					// on the document and different diagrams open in each document,
					// dropping on one document will also add the shape to the
					// diagram in the non-active window.
					IServiceProvider serviceProvider;
					IMonitorSelectionService selectionService;
					DiagramDocView currentView;
					if (null == (serviceProvider = (Store as IORMToolServices).ServiceProvider) ||
						null == (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))) ||
						null == (currentView = selectionService.CurrentDocumentView as DiagramDocView) ||
						currentView.CurrentDesigner != activeDiagramView)
					{
						return false;
					}
				}
			}
			ObjectType objType;
			FactType factType;
			ObjectTypePlaysRole objectTypePlaysRole;
			SetConstraint setConstraint;
			ExclusionConstraint exclusionConstraint;
			if (null != (factType = element as FactType))
			{
				if (factType is SubtypeFact)
				{
					return true;
				}
#if !SHOW_IMPLIED_SHAPES
				else if (factType.ImpliedByObjectification != null)
				{
					return false;
				}
#endif // !SHOW_IMPLIED_SHAPES
				return ShouldDisplayPartOfReferenceMode(factType);
			}
			else if (null != (objectTypePlaysRole = element as ObjectTypePlaysRole))
			{
#if SHOW_IMPLIED_SHAPES
#if !SHOW_FACTSHAPE_FOR_SUBTYPE
				FactType fact = objectTypePlaysRole.PlayedRoleCollection.FactType;
				if (fact is SubtypeFact)
				{
					return false;
				}
#endif // !SHOW_FACTSHAPE_FOR_SUBTYPE
#elif SHOW_FACTSHAPE_FOR_SUBTYPE
				FactType fact = objectTypePlaysRole.PlayedRoleCollection.FactType;
				if (fact.ImpliedByObjectification != null)
				{
					return false;
				}
#else
				FactType fact = objectTypePlaysRole.PlayedRole.FactType;
				if (fact is SubtypeFact || fact.ImpliedByObjectification != null)
				{
					return false;
				}
#endif
				return ShouldDisplayPartOfReferenceMode(objectTypePlaysRole);
			}
			else if (null != (setConstraint = element as SetConstraint))
			{
				return !setConstraint.Constraint.ConstraintIsInternal;
			}
			else if (null != (exclusionConstraint = element as ExclusionConstraint))
			{
				return exclusionConstraint.ExclusiveOrMandatoryConstraint == null;
			}
			else if (element is SetComparisonConstraint ||
					 element is RoleHasValueConstraint ||
					 element is FactConstraint ||
					 element is ModelNote ||
					 element is ModelNoteReferencesModelElement)
			{
				return true;
			}
			else if (null != (objType = element as ObjectType))
			{
				return ShouldDisplayObjectType(objType);
			}
			return false;
		}
		/// <summary>
		/// Determine if an ObjectType element should be displayed on
		/// the diagram.
		/// </summary>
		/// <param name="typeElement">The element to test</param>
		/// <returns>true to display, false to not display</returns>
		public bool ShouldDisplayObjectType(ObjectType typeElement)
		{
			// We don't ever display a nesting ObjectType, even if the Objectification is not drawn.
			if (typeElement.NestedFactType == null)
			{
				return ShouldDisplayPartOfReferenceMode(typeElement);
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Function to determine if a fact type, which may be participating
		/// in a reference mode pattern, should be displayed.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(FactType factType)
		{
			foreach (UniquenessConstraint constraint in factType.GetInternalConstraints<UniquenessConstraint>())
			{
				ObjectType entity = constraint.PreferredIdentifierFor;
				// We only consider this to be a collapsible ref mode if its roleplayer is a value type
				LinkedElementCollection<Role> constraintRoles;
				ObjectType rolePlayer;
				Role role;
				RoleProxy proxy;
				FactType impliedFact;
				if (entity != null &&
					1 == (constraintRoles = constraint.RoleCollection).Count &&
					null != (rolePlayer = (role = constraintRoles[0]).RolePlayer) &&
					rolePlayer.IsValueType &&
					(null == (proxy = role.Proxy) ||
					!(null != (impliedFact = proxy.FactType) &&
					impliedFact.ImpliedByObjectification == entity.Objectification)))
				{
					return !ShouldCollapseReferenceMode(entity);
				}
			}
			return true;
		}
		/// <summary>
		/// Function to determine if a role player link, which may be participating
		/// in a reference mode pattern, should be displayed. Defers to test for
		/// the corresponding fact type.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(ObjectTypePlaysRole objectTypePlaysRole)
		{
			Role role = objectTypePlaysRole.PlayedRole;
			FactType factType = role.FactType;
			return (factType != null) ? ShouldDisplayPartOfReferenceMode(factType) : true;
		}
		/// <summary>
		/// Function to determine if an object type, which may be participating
		/// as the value type in the reference mode pattern, should be displayed. The
		/// object type needs to be displayed if any of the reference modes using the
		/// value type has a true ExpandRefMode property or if the object type is
		/// a role player for any other visible role.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(ObjectType objectType)
		{
			if (objectType.IsValueType)
			{
				LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				if (playedRoleCount > 0)
				{
					bool partOfCollapsedRefMode = false;
					for (int i = 0; i < playedRoleCount; ++i)
					{
						FactType factType = playedRoles[i].FactType;
						if (factType != null)
						{
							if (ShouldDisplayPartOfReferenceMode(factType))
							{
								partOfCollapsedRefMode = false;
								break;
							}
							else
							{
								partOfCollapsedRefMode = true;
								// Keep going. We may be part of a
								// non-collapsed relationship as well, in
								// which case we need to be visible.
							}
						}
					}
					if (partOfCollapsedRefMode)
					{
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// Test if the reference mode should be collapsed. Helper function for
		/// ShouldDisplayPartOfReferenceMode implementations.
		/// </summary>
		/// <param name="objectType"></param>
		/// <returns>True if the object type has a collapsed reference mode</returns>
		private bool ShouldCollapseReferenceMode(ObjectType objectType)
		{
			ObjectTypeShape objectTypeShape;
			ObjectifiedFactTypeNameShape objectifiedShape;
			if (null != (objectTypeShape = FindShapeForElement<ObjectTypeShape>(objectType)))
			{
				if (objectType.HasReferenceMode)
				{
					return !objectTypeShape.ExpandRefMode;
				}
			}
			else if (null != (objectifiedShape = FindShapeForElement<ObjectifiedFactTypeNameShape>(objectType)))
			{
				if (objectType.HasReferenceMode)
				{
					return !objectifiedShape.ExpandRefMode;
				}
			}
			return objectType.HasReferenceMode;
		}
#if SHOW_FACTSHAPE_FOR_SUBTYPE
		/// <summary>See <see cref="ORMDiagramBase.CreateChildShape"/>.</summary>
		protected override ShapeElement CreateChildShape(ModelElement element)
		{
			if (element is SubtypeFact)
			{
				return new FactTypeShape(this.Partition);
			}
			return base.CreateChildShape(element);
		}
#endif // SHOW_FACTSHAPE_FOR_SUBTYPE
		/// <summary>
		/// Defer to ConfiguringAsChildOf for ORMBaseShape and ORMBaseBinaryLinkShape children
		/// </summary>
		/// <param name="child">The child being configured</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		protected override void OnChildConfiguring(ShapeElement child, bool createdDuringViewFixup)
		{
			ORMBaseShape baseShape;
			ORMBaseBinaryLinkShape baseLinkShape;
			if (null != (baseShape = child as ORMBaseShape))
			{
				baseShape.ConfiguringAsChildOf(this, createdDuringViewFixup);
			}
			else if (null != (baseLinkShape = child as ORMBaseBinaryLinkShape))
			{
				// ORM lines cross, they don't jump. However, the RouteJumpType cannot
				// be set before the diagram is in place, so this property cannot be set
				// from initialization code in the shape itself.
				baseLinkShape.RouteJumpType = VGObjectLineJumpCode.VGObjectJumpCodeNever;
				baseLinkShape.ConfiguringAsChildOf(this, createdDuringViewFixup);
			}
		}
		/// <summary>
		/// Auto shape placement performance when AutoPopulateShapes is turned on (such
		/// as when importing a model with no shape information) is dreadful. Don't auto-place
		/// in this condition.
		/// </summary>
		public override bool ShouldAutoPlaceChildShapes
		{
			get
			{
				return false;	//!AutoPopulateShapes || (Partition != Store.DefaultPartition);
			}
		}
		/// <summary>
		/// Locate an existing shape on this diagram corresponding to this element
		/// </summary>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		public ShapeElement FindShapeForElement(ModelElement element)
		{
			return FindShapeForElement<ShapeElement>(element);
		}
		/// <summary>
		/// Locate an existing typed shape on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		public TShape FindShapeForElement<TShape>(ModelElement element) where TShape : ShapeElement
		{
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(element))
			{
				TShape shape = pel as TShape;
				if (shape != null && shape.Diagram == this)
				{
					return shape;
				}
			}
			return null;
		}
		/// <summary>
		/// Setup our routing style.
		/// </summary>
		public override void OnInitialize()
		{
			base.OnInitialize();
			this.RoutingStyle = VGRoutingStyle.VGRouteNone;
		}
		#endregion // View Fixup Methods
		#region Customize appearance
		/// <summary>
		/// The Brush to use when drawing the background of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyBackgroundResource = new StyleSetResourceId("Neumont", "StickyBackgroundResource");
		/// <summary>
		/// The Brush to use when drawing the foreground of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyForegroundResource = new StyleSetResourceId("Neumont", "StickyForegroundResource");
		/// <summary>
		/// The brush or pen used to draw a link decorator as sticky
		/// </summary>
		public static readonly StyleSetResourceId StickyConnectionLineDecoratorResource = new StyleSetResourceId("Neumont", "StickyConnectionLineDecorator");
		/// <summary>
		/// The brush or pen used to draw a link decorator as active. Generally corresponds to the role picker color
		/// </summary>
		public static readonly StyleSetResourceId ActiveConnectionLineDecoratorResource = new StyleSetResourceId("Neumont", "ActiveConnectionLineDecorator");
		/// <summary>
		/// The brush used to draw a link as active. Generally corresponds to the role picker color.
		/// </summary>
		public static readonly StyleSetResourceId ActiveBackgroundResource = new StyleSetResourceId("Neumont", "ActiveBackgroundResource");
		/// <summary>
		/// The brush used to draw the background for an item with errors.
		/// </summary>
		public static readonly StyleSetResourceId ErrorBackgroundResource = new StyleSetResourceId("Neumont", "ErrorBackgroundResource");
		/// <summary>
		/// The brush used to draw the background for an item with errors when the shape is highlighted.
		/// </summary>
		public static readonly StyleSetResourceId HighlightedErrorBackgroundResource = new StyleSetResourceId("Neumont", "HighlightedErrorBackgroundResource");
		/// <summary>
		/// A transparent brush.
		/// </summary>
		public static readonly StyleSetResourceId TransparentBrushResource = new StyleSetResourceId("Neumont", "TransparentBrushResource");

		/// <summary>
		/// Standard override to populate the style set for the shape type
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);

			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			Color stickyBackColor = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			Color stickyForeColor = colorService.GetForeColor(ORMDesignerColor.ActiveConstraint);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = stickyBackColor;
			classStyleSet.AddBrush(StickyBackgroundResource, DiagramBrushes.ShapeBackgroundSelected, brushSettings);

			brushSettings.Color = stickyForeColor;
			classStyleSet.AddBrush(StickyForegroundResource, DiagramBrushes.ShapeText, brushSettings);

			PenSettings penSettings = new PenSettings();
			penSettings.Color = stickyBackColor;
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.AddPen(StickyBackgroundResource, DiagramPens.ShapeHighlightOutline, penSettings);

			penSettings.Color = stickyForeColor;
			classStyleSet.AddPen(StickyForegroundResource, DiagramPens.ShapeHighlightOutline, penSettings);
		}
		/// <summary>
		/// Drop the grid size to make positioning easier.
		/// </summary>
		public override double DefaultGridSize
		{
			get
			{
				return .05;
			}
		}
		#endregion // Customize appearance
		#region Toolbox support
		/// <summary>
		/// Enable our toolbox actions. Additional filters recognized in this
		/// routine are added in ORMDesignerPackage.CreateToolboxItems.
		/// </summary>
		public override void OnViewMouseEnter(DiagramPointEventArgs pointArgs)
		{
			DiagramView activeView = ActiveDiagramView;
			MouseAction action = null;

			if (activeView != null)
			{
				if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectExternalConstraintFilterString))
				{
					action = ExternalConstraintConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramExternalConstraintFilterString))
				{
					action = ExternalConstraintAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectInternalUniquenessConstraintFilterString))
				{
					action = InternalUniquenessConstraintConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramInternalUniquenessConstraintFilterString))
				{
					action = InternalUniquenessConstraintAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectRoleFilterString))
				{
					action = RoleConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramCreateSubtypeFilterString))
				{
					action = SubtypeConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramModelNoteFilterString))
				{
					action = ModelNoteAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramConnectModelNoteFilterString))
				{
					action = ModelNoteConnectAction;
				}
				else if (activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramDefaultFilterString))
				{
					action = ToolboxAction;
				}
			}

			DiagramClientView clientView = pointArgs.DiagramClientView;
			if (clientView.ActiveMouseAction != action &&
				// UNDONE: We should not need the following line because the current mouse action
				// should correspond to the current toolbox action. However, Toolbox.SetSelectedToolboxItem
				// always crashes, so there is no way to reset the action when we explicitly chain.
				// The result of not doing this is that moving the mouse off and back on the diagram
				// during a chained mouse action cancels the action.
				// See corresponding code in ExternalConstraintConnectAction.ChainMouseAction and
				// InternalUniquenessConstraintConnectAction.ChainMouseAction.
				(action != null || activeView.Toolbox.GetSelectedToolboxItem() != null))
			{
				clientView.ActiveMouseAction = action;
			}
		}
		/// <summary>
		/// Select the given item on the default tab
		/// </summary>
		/// <param name="activeView">DiagramView</param>
		/// <param name="itemId">Name of the item id</param>
		public static void SelectToolboxItem(DiagramView activeView, string itemId)
		{
			SelectToolboxItem(activeView, itemId, ResourceStrings.ToolboxDefaultTabName);
		}
		/// <summary>
		/// Select the given item on the specified toolbox tab
		/// UNDONE: The critical point of this routine crashes VS, so
		/// it is currently a noop
		/// </summary>
		/// <param name="activeView">DiagramView</param>
		/// <param name="itemId">Name of the item id</param>
		/// <param name="tabName">The tab name to select</param>
		public static void SelectToolboxItem(DiagramView activeView, string itemId, string tabName)
		{
			IToolboxService toolbox = activeView.Toolbox;
			if (toolbox != null)
			{
				// Select the connector action on the toolbox
				Debug.Assert(toolbox.GetSelectedToolboxItem() == null); // Should be turned off during MouseActionDeactivated
				ToolboxItemCollection items = toolbox.GetToolboxItems(tabName);
				foreach (ToolboxItem item in items)
				{
					ModelingToolboxItem modelingItem = item as ModelingToolboxItem;
					if (modelingItem != null && modelingItem.Id == itemId)
					{
						// UNDONE: See comments on side effect in ORMDiagram.OnViewMouseEnter
						//toolbox.SetSelectedToolboxItem(item); // UNDONE: MSBUG Gives 'Value does not fall within expected range' error message, not sure why
						break;
					}
				}
			}
		}
		#region External constraint action
		[NonSerialized]
		private ExternalConstraintConnectAction myExternalConstraintConnectAction;
		[NonSerialized]
		private ExternalConstraintAction myExternalConstraintAction;
		/// <summary>
		/// The connect action used to connect an external constraint to its role sequences
		/// </summary>
		public ExternalConstraintConnectAction ExternalConstraintConnectAction
		{
			get
			{
				if (myExternalConstraintConnectAction == null)
				{
					myExternalConstraintConnectAction = new ExternalConstraintConnectAction(this);
				}
				return myExternalConstraintConnectAction;
			}
		}
		/// <summary>
		/// Create the action used to connect an external constraint to its role sequences
		/// </summary>
		/// <returns>ExternalConstraintConnectAction instance</returns>
		protected virtual ExternalConstraintConnectAction CreateExternalConstraintConnectAction()
		{
			return new ExternalConstraintConnectAction(this);
		}
		/// <summary>
		/// The action used to drop an external constraint from the toolbox
		/// </summary>
		public ExternalConstraintAction ExternalConstraintAction
		{
			get
			{
				if (myExternalConstraintAction == null)
				{
					myExternalConstraintAction = CreateExternalConstraintAction();
					myExternalConstraintAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
					{
						ExternalConstraintAction action = sender as ExternalConstraintAction;
						if (action.ActionCompleted)
						{
							ExternalConstraintShape addedShape = action.AddedConstraintShape;
							Debug.Assert(addedShape != null); // ActionCompleted should be false otherwise
							ExternalConstraintConnectAction.ChainMouseAction(addedShape, e.DiagramClientView);
						}
					};
				}
				return myExternalConstraintAction;
			}
		}
		/// <summary>
		/// Create the action used to add an external constraint from the toolbox
		/// </summary>
		/// <returns>ExternalConstraintAction instance</returns>
		protected virtual ExternalConstraintAction CreateExternalConstraintAction()
		{
			return new ExternalConstraintAction(this);
		}
		#endregion // External constraint action
		#region Internal uniqueness constraint action
		[NonSerialized]
		private InternalUniquenessConstraintAction myInternalUniquenessConstraintAction;
		[NonSerialized]
		private InternalUniquenessConstraintConnectAction myInternalUniquenessConstraintConnectAction;
		/// <summary>
		/// The connect action used to connect an internal uniqueness constraint
		/// to its roles.
		/// </summary>
		public InternalUniquenessConstraintConnectAction InternalUniquenessConstraintConnectAction
		{
			get
			{
				if (myInternalUniquenessConstraintConnectAction == null)
				{
					myInternalUniquenessConstraintConnectAction = CreateInternalUniquenessConstraintConnectAction();
				}
				return myInternalUniquenessConstraintConnectAction;
			}
		}
		/// <summary>
		/// Create the connect action used to add an internal uniqueness constraint from the toolbox
		/// </summary>
		/// <returns>InternalUniquenssConstraintAction instance</returns>
		protected virtual InternalUniquenessConstraintConnectAction CreateInternalUniquenessConstraintConnectAction()
		{
			return new InternalUniquenessConstraintConnectAction(this);
		}
		/// <summary>
		/// The action used to add an internal uniqueness constraint from the toolbox
		/// </summary>
		public InternalUniquenessConstraintAction InternalUniquenessConstraintAction
		{
			get
			{
				if (myInternalUniquenessConstraintAction == null)
				{
					myInternalUniquenessConstraintAction = CreateInternalUniquenessConstraintAction();
					myInternalUniquenessConstraintAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
					{
						InternalUniquenessConstraintAction action = sender as InternalUniquenessConstraintAction;
						if (action.ActionCompleted)
						{
							UniquenessConstraint constraint = action.AddedConstraint;
							FactTypeShape addedToShape = action.DropTargetShape;
							DiagramClientView view = e.DiagramClientView;
							Debug.Assert(constraint != null); // ActionCompleted should be false otherwise
							view.Selection.Set(addedToShape.GetDiagramItem(constraint));
							InternalUniquenessConstraintConnectAction.ChainMouseAction(addedToShape, constraint, view);
						}
					};
				}
				return myInternalUniquenessConstraintAction;
			}
		}
		/// <summary>
		/// Create the connect action used to connect internal uniqueness constrant roles
		/// </summary>
		/// <returns>InternalUniquenssConstraintAction instance</returns>
		protected virtual InternalUniquenessConstraintAction CreateInternalUniquenessConstraintAction()
		{
			return new InternalUniquenessConstraintAction(this);
		}
		#endregion Internal uniqueness constraint action
		#region Role drag action
		[NonSerialized]
		private RoleDragPendingAction myRoleDragPendingAction;
		[NonSerialized]
		private RoleConnectAction myRoleConnectAction;
		/// <summary>
		/// The drag action used by a role box to begin dragging.
		/// The default implementation chains to a RoleConnectAction
		/// when dragging begins.
		/// </summary>
		public RoleDragPendingAction RoleDragPendingAction
		{
			get
			{
				RoleDragPendingAction retVal = myRoleDragPendingAction;
				if (retVal == null)
				{
					myRoleDragPendingAction = retVal = CreateRoleDragPendingAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the drag action used for the RoleDragPendingAction property
		/// </summary>
		/// <returns>RoleDragPendingAction instance</returns>
		protected virtual RoleDragPendingAction CreateRoleDragPendingAction()
		{
			return new RoleDragPendingAction(this);
		}
		/// <summary>
		/// The connect action used to connect a role and
		/// its role player (an object type)
		/// </summary>
		public RoleConnectAction RoleConnectAction
		{
			get
			{
				RoleConnectAction retVal = myRoleConnectAction;
				if (retVal == null)
				{
					myRoleConnectAction = retVal = CreateRoleConnectAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the connect action used to connect roles to their role players
		/// </summary>
		/// <returns>RoleConnectAction instance</returns>
		protected virtual RoleConnectAction CreateRoleConnectAction()
		{
			return new RoleConnectAction(this);
		}
		#endregion // Role drag action
		#region Subtype create action
		[NonSerialized]
		private SubtypeConnectAction mySubtypeConnectAction;
		/// <summary>
		/// The connect action used to connect a base type to a derived type
		/// </summary>
		public SubtypeConnectAction SubtypeConnectAction
		{
			get
			{
				SubtypeConnectAction retVal = mySubtypeConnectAction;
				if (retVal == null)
				{
					mySubtypeConnectAction = retVal = CreateSubtypeConnectAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the connect action used to connect roles to their role players
		/// </summary>
		/// <returns>SubtypeConnectAction instance</returns>
		protected virtual SubtypeConnectAction CreateSubtypeConnectAction()
		{
			return new SubtypeConnectAction(this);
		}
		#endregion // Subtype create action
		#region ModelNote create action
		[NonSerialized]
		private ModelNoteAction myModelNoteAction;
		/// <summary>
		/// The action used to drop a model note from the toolbox
		/// </summary>
		public ModelNoteAction ModelNoteAction
		{
			get
			{
				if (myModelNoteAction == null)
				{
					myModelNoteAction = CreateModelNoteAction();
					myModelNoteAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
					{
						ModelNoteAction action = sender as ModelNoteAction;
						if (action.ActionCompleted)
						{
							ModelNoteShape addedShape = action.AddedNoteShape;
							Debug.Assert(addedShape != null); // ActionCompleted should be false otherwise
							Store store = Store;
							EditorUtility.ActivatePropertyEditor(
								(store as IORMToolServices).ServiceProvider,
								DomainTypeDescriptor.CreatePropertyDescriptor(addedShape.ModelElement, Note.TextDomainPropertyId),
								true);
						}
					};
				}
				return myModelNoteAction;
			}
		}
		/// <summary>
		/// Create the action used to add an external constraint from the toolbox
		/// </summary>
		/// <returns>ExternalConstraintAction instance</returns>
		protected virtual ModelNoteAction CreateModelNoteAction()
		{
			return new ModelNoteAction(this);
		}
		#endregion // ModelNote connect action
		#region ModelNote connect action
		[NonSerialized]
		private ModelNoteConnectAction myModelNoteConnectAction;
		/// <summary>
		/// The connect action used to connect a note to a referenced element
		/// </summary>
		public ModelNoteConnectAction ModelNoteConnectAction
		{
			get
			{
				ModelNoteConnectAction retVal = myModelNoteConnectAction;
				if (retVal == null)
				{
					myModelNoteConnectAction = retVal = CreateModelNoteConnectAction();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Create the connect action used to connect a note to a referenced element
		/// </summary>
		/// <returns>ModelNoteConnectAction instance</returns>
		protected virtual ModelNoteConnectAction CreateModelNoteConnectAction()
		{
			return new ModelNoteConnectAction(this);
		}
		#endregion // ModelNote connect action
		#endregion // Toolbox support
		#region Other base overrides
		/// <summary>
		/// Clean up disposable members (connection actions)
		/// </summary>
		/// <param name="disposing">Do stuff if true</param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					// Use a somewhat paranoid pattern here to protect against reentrancy
					IDisposable disposeMe;
					disposeMe = myExternalConstraintAction as IDisposable;
					myExternalConstraintAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myExternalConstraintConnectAction as IDisposable;
					myExternalConstraintConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myInternalUniquenessConstraintAction as IDisposable;
					myInternalUniquenessConstraintAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myInternalUniquenessConstraintConnectAction as IDisposable;
					myInternalUniquenessConstraintConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myRoleDragPendingAction as IDisposable;
					myRoleDragPendingAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = myRoleConnectAction as IDisposable;
					myRoleConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}

					disposeMe = mySubtypeConnectAction as IDisposable;
					mySubtypeConnectAction = null;
					if (disposeMe != null)
					{
						disposeMe.Dispose();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		/// <summary>
		/// Set the base font based on the font and color settings.
		/// UNDONE: This affects the size right now, but not the font name
		/// </summary>
		protected override Font BaseFontFromEnvironment
		{
			get
			{
				return (this.Store as ObjectModel.IORMToolServices).FontAndColorService.GetFont(ORMDesignerColorCategory.Editor);
			}
		}
		/// <summary>
		/// Stop all auto shape selection on transaction commit except when
		/// the item is being dropped.
		/// </summary>
		public override IList FixUpDiagramSelection(ShapeElement newChildShape)
		{
			if (DropTargetContext.HasDropTargetContext(Store.TransactionManager.CurrentTransaction.TopLevelTransaction))
			{
				return base.FixUpDiagramSelection(newChildShape);
			}
			return null;
		}
		#endregion // Other base overrides
		#region Accessibility Properties
		/// <summary>
		/// Return the class name as the accessible name
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return TypeDescriptor.GetClassName(this);
			}
		}
		/// <summary>
		/// Return the component name as the accessible value
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return TypeDescriptor.GetComponentName(this);
			}
		}
		#endregion // Accessibility Properties
		#region Utility Methods
		/// <summary>
		/// Modify the luminosity for a given color. This
		/// duplicates the algorithm in ShapeElement.GetShapeLuminosity,
		/// which is not available because it is only run for shape elements
		/// in the DiagramClientView.HighlightedShapes collection.
		/// </summary>
		/// <param name="startColor">The original color</param>
		/// <returns>The modified color</returns>
		public static Color ModifyLuminosity(Color startColor)
		{
			HslColor hslColor = HslColor.FromRgbColor(startColor);
			hslColor.Luminosity = ModifyLuminosity(hslColor.Luminosity);
			return hslColor.ToRgbColor();
		}
		/// <summary>
		/// Modify a specific luminosity. 
		/// </summary>
		/// <param name="startLuminosity">Beginning luminosity value</param>
		/// <returns>modified luminosity</returns>
		public static int ModifyLuminosity(int startLuminosity)
		{
			// Base framework algorithm for reference
			//const int luminosityCheck = 160;
			//const int luminosityDelta = 40;
			//const double luminosityFactor = 0.9;
			//return (startLuminosity >= luminosityCheck) ?
			//	(int)(startLuminosity * luminosityFactor) :
			//	(startLuminosity + luminosityDelta);

			// Use a sliding scale to brighten/darken colors
			const int maxLuminosity = 255;
			const int luminosityCheck = 160;
			const int luminosityFixedDelta = 60;
			const int luminosityIncrementalDelta = 30;
			const double luminosityFixedFactor = 0.93;
			const double luminosityIncrementalFactor = -0.06;
			return (startLuminosity >= luminosityCheck) ?
				(int)(startLuminosity * (luminosityFixedFactor + (luminosityIncrementalFactor * (double)(maxLuminosity - startLuminosity) / (maxLuminosity - luminosityCheck)))) :
				(startLuminosity + luminosityFixedDelta + (int)((double)(luminosityCheck - startLuminosity) / luminosityCheck * luminosityIncrementalDelta));
		}
		#endregion // Utility Methods
		#region IProxyDisplayProvider Implementation
		/// <summary>
		/// Implements IProxyDisplayProvider.ElementDisplayedAs
		/// </summary>
		protected ModelElement ElementDisplayedAs(ModelElement element)
		{
			ObjectType objectElement;
			ExclusionConstraint exclusionConstraint;
			if (null != (objectElement = element as ObjectType))
			{
				if (!ShouldDisplayObjectType(objectElement))
				{
					FactType nestedFact = objectElement.NestedFactType;
					if (nestedFact != null)
					{
						return nestedFact;
					}
					// Otherwise, every fact type we're a role player for is
					// part of a collapsed reference mode. Grab the first fact, and
					// find the corresponding object type.
					foreach (ConstraintRoleSequence constraintSequence in objectElement.PlayedRoleCollection[0].ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint iuc = constraintSequence as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ObjectType displayedType = iuc.PreferredIdentifierFor;
							if (displayedType != null)
							{
								return displayedType;
							}
						}
					}
				}
			}
			else if (null != (exclusionConstraint = element as ExclusionConstraint))
			{
				MandatoryConstraint mandatoryConstraint = exclusionConstraint.ExclusiveOrMandatoryConstraint;
				if (mandatoryConstraint != null)
				{
					return mandatoryConstraint;
				}
			}
			return null;
		}
		ModelElement IProxyDisplayProvider.ElementDisplayedAs(ModelElement element)
		{
			return ElementDisplayedAs(element);
		}
		#endregion // IProxyDisplayProvider Implementation
	}

	#region ORMDiagramBase class
	public partial class ORMDiagramBase
	{
		private NodeShape CreateShapeForObjectType(ObjectType newElement)
		{
			return FactTypeShape.ShouldDrawObjectification(newElement.NestedFactType) ? (NodeShape)new ObjectifiedFactTypeNameShape(this.Partition) : new ObjectTypeShape(this.Partition);
		}
	}
	#endregion // ORMDiagramBase class
}

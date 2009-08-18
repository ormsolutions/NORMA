#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	/// <summary>
	/// A callback delegate to use during shape placement. Used with <see cref="M:ORMDiagram.PlaceElementOnDiagram"/>
	/// and <see cref="M:ORMDiagram.FixupRelatedLinks"/>
	/// </summary>
	/// <param name="element">The placed element</param>
	/// <param name="newShape">The newly created shape element</param>
	public delegate void FixupNewShape(ModelElement element, ShapeElement newShape);
	/// <summary>
	/// Implement this interface on any shape (generally a link shape) that is auto-created
	/// and also selectable. If this is set, then the element is filtered out of any drag/drop
	/// or copy of shapes.
	/// </summary>
	public interface IAutoCreatedSelectableShape
	{
	}
	[DiagramMenuDisplay(DiagramMenuDisplayOptions.Required | DiagramMenuDisplayOptions.AllowMultiple, typeof(ORMDiagram), "UNDONE", "Diagram.TabImage", "Diagram.BrowserImage")]
	public partial class ORMDiagram : IProxyDisplayProvider, IMergeElements
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
		#endregion // Constructors
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
			if (!e.Handled)
			{
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && !e.Handled; ++i)
					{
						extenders[i].OnDragOver(this, e);
					}
				}
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
			if (PlaceORMElementOnDiagram(dataObject, null, e.MousePosition, ORMPlacementOption.AllowMultipleShapes, null, null))
			{
				e.Effect = DragDropEffects.All;
				e.Handled = true;
			}
			if (!e.Handled)
			{
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && !e.Handled; ++i)
					{
						extenders[i].OnDragDrop(this, e);
					}
				}
			}
			base.OnDragDrop(e);
		}
		/// <summary>
		/// Place a new shape for an existing element onto this diagram
		/// </summary>
		/// <param name="dataObject">The dataObject containing the element to place. If this is set, elementToPlace must be null.</param>
		/// <param name="elementToPlace">The the element to place. If this is set, dataObject must be null.</param>
		/// <param name="elementPosition">An initial position for the element</param>
		/// <param name="placementOptions">Controls the actions by this method</param>
		/// <param name="beforeStandardFixupCallback">A <see cref="FixupNewShape"/> callback used to configure the shape before standard processing is applied</param>
		/// <param name="afterStandardFixupCallback">A <see cref="FixupNewShape"/> callback used to configure the shape after standard processing is applied</param>
		/// <returns>true if the element was placed</returns>
		public bool PlaceORMElementOnDiagram(IDataObject dataObject, ModelElement elementToPlace, PointD elementPosition, ORMPlacementOption placementOptions, FixupNewShape beforeStandardFixupCallback, FixupNewShape afterStandardFixupCallback)
		{
			Debug.Assert((dataObject == null) ^ (elementToPlace == null), "Pass in dataObject or elementToPlace");
			bool retVal = false;
			ObjectType objectType = null;
			FactType factType = null;
			SetComparisonConstraint setComparisonConstraint = null;
			SetConstraint setConstraint = null;
			ModelNote modelNote = null;
			ModelElement element = null;
			LinkedElementCollection<FactType> verifyFactTypeList = null;
			if (null != (objectType = (dataObject == null) ? elementToPlace as ObjectType : dataObject.GetData(typeof(ObjectType)) as ObjectType))
			{
				element = objectType;
			}
			else if (null != (factType = (dataObject == null) ? elementToPlace as FactType : dataObject.GetData(typeof(FactType)) as FactType))
			{
				// SubtypeFacts are not placed, they appear as links between placed shapes and are fixed up at that point
				if (!(factType is SubtypeFact))
				{
					element = factType;
				}
			}
			else if (null != (setComparisonConstraint = (dataObject == null) ? elementToPlace as SetComparisonConstraint : dataObject.GetData(typeof(SetComparisonConstraint)) as SetComparisonConstraint))
			{
				verifyFactTypeList = setComparisonConstraint.FactTypeCollection;
				element = setComparisonConstraint;
			}
			else if (null != (setConstraint = (dataObject == null) ? elementToPlace as SetConstraint : dataObject.GetData(typeof(SetConstraint)) as SetConstraint))
			{
				verifyFactTypeList = setConstraint.FactTypeCollection;
				element = setConstraint;
			}
			else if (null != (modelNote = (dataObject == null) ? elementToPlace as ModelNote : dataObject.GetData(typeof(ModelNote)) as ModelNote))
			{
				element = modelNote;
			}
			if (verifyFactTypeList != null)
			{
				int factCount = verifyFactTypeList.Count;
				for (int i = 0; i < factCount; ++i)
				{
					if (!ElementHasShape(verifyFactTypeList[i]))
					{
						element = null;
						break;
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
					Dictionary<object, object> topLevelContextInfo = transaction.TopLevelTransaction.Context.ContextInfo;
					if (placementOptions == ORMPlacementOption.AllowMultipleShapes)
					{
						topLevelContextInfo.Add(MultiShapeUtility.AllowMultipleShapes, null);
					}
					ShapeElement shapeElement = FixUpLocalDiagram(element);
					if (clearContext)
					{
						DropTargetContext.Remove(transaction.TopLevelTransaction);
					}
					if (shapeElement != null)
					{
						// Perform preliminary fixup
						if (null != beforeStandardFixupCallback)
						{
							beforeStandardFixupCallback(element, shapeElement);
						}
						if (factType != null)
						{
							FixupFactType(factType, (FactTypeShape)shapeElement, false);
						}
						else if (objectType != null)
						{
							FixupObjectType(objectType, shapeElement as ObjectTypeShape, false);
						}
						else if (setConstraint != null)
						{
							FixupConstraint(setConstraint, (ExternalConstraintShape)shapeElement);
						}
						else if (setComparisonConstraint != null)
						{
							FixupConstraint(setComparisonConstraint, (ExternalConstraintShape)shapeElement);
						}
						else if (modelNote != null)
						{
							FixupModelNote(modelNote, (ModelNoteShape)shapeElement);
						}
						
						// Perform additional fixup
						if (null != afterStandardFixupCallback)
						{
							afterStandardFixupCallback(element, shapeElement);
						}
					}
					if (placementOptions == ORMPlacementOption.AllowMultipleShapes)
					{
						topLevelContextInfo.Remove(MultiShapeUtility.AllowMultipleShapes);
					}
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
						storeChange = true;
					}
				}
				if (!storeChange && placementOptions == ORMPlacementOption.SelectIfNotPlaced)
				{
					DiagramView selectOnView = ActiveDiagramView;
					ShapeElement shape = null;

					foreach (ShapeElement existingShape in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, element))
					{
						shape = existingShape;
						break;
					}

					if (selectOnView != null && shape != null)
					{
						selectOnView.Selection.Set(new DiagramItem(shape));
						selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Place a new shape for an existing element onto this diagram
		/// </summary>
		/// <param name="elementToPlace">The the element to place.</param>
		/// <param name="elementPosition">An initial position for the element</param>
		/// <param name="placementOptions">Controls the actions by this method</param>
		/// <param name="fixupShapeCallback">A <see cref="FixupNewShape"/> callback used to configure the shape</param>
		/// <returns>true if the element was placed</returns>
		public bool PlaceElementOnDiagram(ModelElement elementToPlace, PointD elementPosition, ORMPlacementOption placementOptions, FixupNewShape fixupShapeCallback)
		{
			bool retVal = false;
			if (elementToPlace != null)
			{
				using (Transaction transaction = Store.TransactionManager.BeginTransaction(ResourceStrings.DropShapeTransactionName))
				{
					bool clearContext;
					if (clearContext = !elementPosition.IsEmpty)
					{
						DropTargetContext.Set(transaction.TopLevelTransaction, Id, elementPosition, null);
					}
					Dictionary<object, object> topLevelContextInfo = transaction.TopLevelTransaction.Context.ContextInfo;
					if (placementOptions == ORMPlacementOption.AllowMultipleShapes)
					{
						topLevelContextInfo.Add(MultiShapeUtility.AllowMultipleShapes, null);
					}
					ShapeElement shapeElement = FixUpLocalDiagram(elementToPlace);
					if (clearContext)
					{
						DropTargetContext.Remove(transaction.TopLevelTransaction);
					}
					if (shapeElement != null && fixupShapeCallback != null)
					{
						fixupShapeCallback(elementToPlace, shapeElement);
					}
					if (placementOptions == ORMPlacementOption.AllowMultipleShapes)
					{
						topLevelContextInfo.Remove(MultiShapeUtility.AllowMultipleShapes);
					}
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
						retVal = true;
					}
				}
				if (!retVal && placementOptions == ORMPlacementOption.SelectIfNotPlaced)
				{
					DiagramView selectOnView = ActiveDiagramView;
					ShapeElement shape = null;

					foreach (ShapeElement existingShape in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, elementToPlace))
					{
						shape = existingShape;
						break;
					}

					if (selectOnView != null && shape != null)
					{
						selectOnView.Selection.Set(new DiagramItem(shape));
						selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
						retVal = true;
					}
				}
			}
			return retVal;
		}
		private void FixupFactType(FactType factType, FactTypeShape factTypeShape, bool childShapesMerged)
		{
			bool duplicateShape = false;
			Objectification objectification = factType.Objectification;
			ObjectType nestingType = (objectification != null) ? objectification.NestingType : null;
			bool lookForNonDisplayedRelatedTypes = false;
			bool haveNonDisplayedRelatedTypes = false;
			foreach (FactTypeShape testShape in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(factTypeShape.Diagram, factType))
			{
				if (testShape != factTypeShape)
				{
					duplicateShape = true;
					if (nestingType != null)
					{
						if (lookForNonDisplayedRelatedTypes)
						{
							if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
							{
								break;
							}
						}
						else if (nestingType.IsSubtypeOrSupertype &&
							factTypeShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
						{
							lookForNonDisplayedRelatedTypes = true;
							if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
							{
								break;
							}
						}
						else
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
			}
			LinkedElementCollection<RoleBase> roleCollection = factType.RoleCollection;
			int roleCount = roleCollection.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				Role role = roleCollection[i].Role;

				if (!duplicateShape)
				{
					// Pick up role players
					FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId));

					// Pick up attached constraints
					FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(role, FactSetConstraint.FactTypeDomainRoleId));
					FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(role, FactSetComparisonConstraint.FactTypeDomainRoleId));
				}

				if (!childShapesMerged)
				{
					// Pick up the role shape
					//check if we have a specific shape or need to use the model element
					if (factTypeShape == null)
					{
						FixUpLocalDiagram(factType, role);
					}
					else
					{
						FixUpLocalDiagram(factTypeShape as ShapeElement, role);
					}

					// Get the role value constraint and the link to it.
					RoleHasValueConstraint valueConstraintLink = RoleHasValueConstraint.GetLinkToValueConstraint(role);

					if (valueConstraintLink != null)
					{
						if (factTypeShape == null)
						{
							FixUpLocalDiagram(factType, valueConstraintLink.ValueConstraint);
						}
						else
						{
							FixUpLocalDiagram(factTypeShape as ShapeElement, valueConstraintLink.ValueConstraint);
						}
					}
				}
			}
			if (!childShapesMerged)
			{
				LinkedElementCollection<ReadingOrder> orders = factType.ReadingOrderCollection;
				if (orders.Count != 0)
				{
					if (factTypeShape == null)
					{
						FixUpLocalDiagram(factType, orders[0]);
					}
					else
					{
						FixUpLocalDiagram(factTypeShape as ShapeElement, orders[0]);
					}
				}
			}
			if (!duplicateShape)
			{
				FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(factType, ModelNoteReferencesFactType.ElementDomainRoleId));
			}
			if (objectification != null && !objectification.IsImplied)
			{
				if (!childShapesMerged)
				{
					if (factTypeShape == null)
					{
						FixUpLocalDiagram(factType, nestingType);
					}
					else
					{
						FixUpLocalDiagram(factTypeShape as ShapeElement, nestingType);
					}
				}
				if (!duplicateShape || haveNonDisplayedRelatedTypes)
				{
					FixupObjectTypeLinks(nestingType, false);
				}
			}
		}
		private void FixupObjectType(ObjectType objectType, ObjectTypeShape objectTypeShape, bool childShapesMerged)
		{
			bool duplicateShape = false;
			bool lookForNonDisplayedRelatedTypes = false;
			bool haveNonDisplayedRelatedTypes = false;
			foreach (ObjectTypeShape testShape in MultiShapeUtility.FindAllShapesForElement<ObjectTypeShape>(objectTypeShape.Diagram, objectType))
			{
				if (testShape != objectTypeShape)
				{
					duplicateShape = true;
					if (lookForNonDisplayedRelatedTypes)
					{
						if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
						{
							break;
						}
					}
					else if (objectType.IsSubtypeOrSupertype &&
						objectTypeShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
					{
						lookForNonDisplayedRelatedTypes = true;
						if (haveNonDisplayedRelatedTypes = testShape.DisplayRelatedTypes != RelatedTypesDisplay.AttachAllTypes)
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
			}
			if (!duplicateShape || haveNonDisplayedRelatedTypes)
			{
				FixupObjectTypeLinks(objectType, haveNonDisplayedRelatedTypes);
			}
			if (!childShapesMerged)
			{
				ValueConstraint valueConstraint = objectType.FindValueConstraint(false);
				if (valueConstraint != null)
				{
					//check if we have a specific shape or need to use the model element
					if (objectTypeShape == null)
					{
						FixUpLocalDiagram(objectType, valueConstraint);
					}
					else
					{
						FixUpLocalDiagram(objectTypeShape as ShapeElement, valueConstraint);
					}
				}
			}
		}
		/// <summary>
		/// Helper function for FixupFactType and FixupObjectType
		/// </summary>
		private void FixupObjectTypeLinks(ObjectType objectType, bool supertypeLinksOnly)
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
				else if (supertypeLinksOnly)
				{
					continue;
				}
				else if (null != (superRole = role as SupertypeMetaRole))
				{
					subtypeFact = role.FactType;
				}
				if (subtypeFact != null)
				{
					FixUpLocalDiagram(subtypeFact);
				}
				else
				{
					FixUpLocalDiagram(link);
				}
			}
			if (!supertypeLinksOnly)
			{
				FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(objectType, ModelNoteReferencesObjectType.ElementDomainRoleId));
			}
		}
		private void FixupConstraint(IConstraint constraint, ExternalConstraintShape constraintShape)
		{
			Debug.Assert(constraint is SetComparisonConstraint || constraint is SetConstraint,
				"Only use FixupConstraint for a SetConstraint or SetComparisonConstraint.");

			ModelElement constraintElement = constraint as ModelElement;
			bool duplicateShape = false;
			foreach (ExternalConstraintShape testShape in MultiShapeUtility.FindAllShapesForElement<ExternalConstraintShape>(constraintShape.Diagram, constraintElement))
			{
				if (testShape != constraintShape)
				{
					duplicateShape = true;
					break;
				}
			}

			if (!duplicateShape)
			{
				FixupRelatedLinks(
					DomainRoleInfo.GetElementLinks<ElementLink>(
						constraintElement,
						constraint is SetComparisonConstraint ?
						FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId :
						FactSetConstraint.SetConstraintDomainRoleId),
					delegate(ModelElement link, ShapeElement newShape)
					{
						ExternalConstraintLink linkShape = newShape as ExternalConstraintLink;
						if (linkShape != null)
						{
							FactTypeShape shape = linkShape.AssociatedFactTypeShape as FactTypeShape;
							if (shape != null)
							{
								shape.ConstraintShapeSetChanged(constraint);
							}
						}
					});
			}
		}
		private void FixupModelNote(ModelNote noteElement, ModelNoteShape noteShape)
		{
			bool duplicateShape = false;
			foreach (ModelNoteShape testShape in MultiShapeUtility.FindAllShapesForElement<ModelNoteShape>(noteShape.Diagram, noteElement))
			{
				if (testShape != noteShape)
				{
					duplicateShape = true;
					break;
				}
			}

			if (!duplicateShape)
			{
				FixupRelatedLinks(DomainRoleInfo.GetElementLinks<ElementLink>(noteElement, ModelNoteReferencesModelElement.NoteDomainRoleId));
			}
		}
		/// <summary>
		/// Fixes up the local diagram for each of the links
		/// </summary>
		/// <param name="links">The links</param>
		public void FixupRelatedLinks(ReadOnlyCollection<ElementLink> links)
		{
			FixupRelatedLinks(links, null);
		}
		/// <summary>
		/// Fixes up the local diagram for each of the links
		/// </summary>
		/// <param name="links">The links</param>
		/// <param name="afterFixup">A <see cref="FixupNewShape"/> callback that fires after link fixup is complete</param>
		public void FixupRelatedLinks(ReadOnlyCollection<ElementLink> links, FixupNewShape afterFixup)
		{
			int linksCount = links.Count;
			for (int i = 0; i < linksCount; ++i)
			{
				ElementLink link;
				ShapeElement newChildShape = FixUpLocalDiagram(link = links[i]);
				if (afterFixup != null)
				{
					afterFixup(link, newChildShape);
				}
			}
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram.  Uses this model as the parent.
		/// </summary>
		/// <param name="newChild">The new element to add</param>
		/// <returns>A newly created child shape</returns>
		public ShapeElement FixUpLocalDiagram(ModelElement newChild)
		{
			return FixUpLocalDiagram(this as ShapeElement, newChild);
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram
		/// </summary>
		/// <param name="existingParent">A model element with a shape on this diagram</param>
		/// <param name="newChild">The new element to add</param>
		/// <returns>All newly created child shapes for the element</returns>
		public IList<ShapeElement> FixUpLocalDiagram(ModelElement existingParent, ModelElement newChild)
		{
			List<ShapeElement> allChildShapes = new List<ShapeElement>();

			if (existingParent == null || existingParent == ModelElement)
			{
				//fix up using this diagram as the parent
				ShapeElement newChildShape;
				if ((newChildShape = FixUpLocalDiagram(newChild)) != null)
				{
					allChildShapes.Add(newChildShape);
				}
				return allChildShapes;
			}

			//fix up for each shape associated with the model element
			foreach (ShapeElement parentShape in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, existingParent))
			{
				ShapeElement newChildShape;
				if ((newChildShape = FixUpLocalDiagram(parentShape, newChild)) != null)
				{
					allChildShapes.Add(newChildShape);
				}
			}

			return allChildShapes;
		}
		/// <summary>
		/// Do the same work as <see cref="Diagram.FixUpDiagram"/> for just
		/// this diagram
		/// </summary>
		/// <param name="existingParent">A shape element on this diagram</param>
		/// <param name="newChild">The new element to add</param>
		/// <returns>A newly created child shape for the element</returns>
		public ShapeElement FixUpLocalDiagram(ShapeElement existingParent, ModelElement newChild)
		{
			if (existingParent == null || existingParent == ModelElement)
			{
				//use this diagram as the parent
				existingParent = this;
			}

			ShapeElement newChildShape;
			if ((newChildShape = existingParent.FixUpChildShapes(newChild)) != null &&
				newChildShape.Diagram == this)
			{
				FixUpDiagramSelection(newChildShape);
				return newChildShape;
			}

			return null;
		}
		#endregion // DragDrop overrides
		#region Toolbox filter strings
		/// <summary>
		/// The filter string used for simple actions
		/// </summary>
		public const string ORMDiagramDefaultFilterString = ORMShapeToolboxHelper.ToolboxFilterString;

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
		/// <summary>
		/// The filter string used to create subtype relationships between object types
		/// </summary>
		public const string ORMDiagramCreateSubtypeFilterString = ORMShapeToolboxHelper.SubtypeConnectorFilterString;
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
		/// with the diagram as the first element
		/// </summary>
		/// <param name="element">Added element</param>
		/// <returns>True for items displayed directly on the
		/// surface. Nesting object types are not displayed</returns>
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
			if (isLink && (element1 == null || element2 == null || !ElementHasShape(element1) || !ElementHasShape(element2)))
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
				Store store = Store;
				TransactionManager transactionManager = store.TransactionManager;
				if (activeDiagramView == null)
				{
					if (!(transactionManager.InTransaction && AutomatedElementDirective.NeverIgnore == ((IORMToolServices)store).GetAutomatedElementDirective(element)))
					{
						return false;
					}
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
					object selectionContainer;
					IORMDesignerView currentView;
					Guid diagramDropTargetId;
					IORMToolServices toolServices;
					AutomatedElementDirective directive;
					if (transactionManager.InTransaction &&
						(AutomatedElementDirective.Ignore == (directive = (toolServices = (IORMToolServices)store).GetAutomatedElementDirective(element)) ||
						(directive != AutomatedElementDirective.NeverIgnore &&
						((null == (serviceProvider = toolServices.ServiceProvider) ||
						null == (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))) ||
						(null == (currentView = (selectionContainer = selectionService.CurrentSelectionContainer) as IORMDesignerView) &&
						null == (currentView = selectionService.CurrentDocumentView as IORMDesignerView)) ||
						currentView.CurrentDesigner != activeDiagramView ||
						(selectionContainer != currentView && selectionContainer is IORMSelectionContainer)) ||
						((diagramDropTargetId = DropTargetContext.GetTargetDiagramId(transactionManager.CurrentTransaction.TopLevelTransaction)) != Guid.Empty &&
						diagramDropTargetId != this.Id)))))
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
			MandatoryConstraint mandatoryConstraint;
			ModelNoteReferencesModelElement noteReference;
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
			else if (null != (mandatoryConstraint = element as MandatoryConstraint))
			{
				return !mandatoryConstraint.IsSimple && !mandatoryConstraint.IsImplied;
			}
			else if (null != (setConstraint = element as SetConstraint))
			{
				return !setConstraint.Constraint.ConstraintIsInternal;
			}
			else if (null != (exclusionConstraint = element as ExclusionConstraint))
			{
				return exclusionConstraint.ExclusiveOrMandatoryConstraint == null;
			}
			else if (null != (noteReference = element as ModelNoteReferencesModelElement))
			{
				SetConstraint referencedSetConstraint = noteReference.Element as SetConstraint;
				// Note that note references to internal constraint cannot be added with the current UI, but
				// are valid in the object model. Don't try to link them.
				return referencedSetConstraint == null || !referencedSetConstraint.Constraint.ConstraintIsInternal;
			}
			else if (element is SetComparisonConstraint ||
					 element is RoleHasValueConstraint ||
					 element is FactConstraint ||
					 element is ModelNote)
			{
				return true;
			}
			else if (null != (objType = element as ObjectType))
			{
				return ShouldDisplayObjectType(objType);
			}
			IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
			if (extenders != null)
			{
				for (int i = 0; i < extenders.Length; ++i)
				{
					if (extenders[i].ShouldAddShapeForElement(this, element))
					{
						return true;
					}
				}
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
            // This also applies to Implicit Boolean ValueTypes (those that are part of a binarized unary).
			if (typeElement.NestedFactType == null && !typeElement.IsImplicitBooleanValue)
			{
				return ShouldDisplayPartOfReferenceMode(typeElement);
			}
            return false;
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
			return false; // If a shape can't be found, then do not collapse, regardless of objectType.HasReferenceMode
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
		/// Defer to <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/> on the child shape
		/// </summary>
		/// <param name="child">The child being configured</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		protected override void OnChildConfiguring(ShapeElement child, bool createdDuringViewFixup)
		{
			IConfigureAsChildShape baseShape;
			if (null != (baseShape = child as IConfigureAsChildShape))
			{
				baseShape.ConfiguringAsChildOf(this, createdDuringViewFixup);
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
			return FindShapeForElement<ShapeElement>(element, false);
		}
		/// <summary>
		/// Locate an existing shape on this diagram corresponding to this element
		/// </summary>
		/// <param name="element">The element to search</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>An existing shape, or null if not found</returns>
		public ShapeElement FindShapeForElement(ModelElement element, bool filterDeleting)
		{
			return FindShapeForElement<ShapeElement>(element, filterDeleting);
		}
		/// <summary>
		/// Locate an existing typed shape on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		public TShape FindShapeForElement<TShape>(ModelElement element) where TShape : ShapeElement
		{
			return FindShapeForElement<TShape>(element, false);
		}
		/// <summary>
		/// Locate an existing typed shape on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="element">The element to search</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>An existing shape, or null if not found</returns>
		public TShape FindShapeForElement<TShape>(ModelElement element, bool filterDeleting) where TShape : ShapeElement
		{
			if (element != null)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(element))
				{
					TShape shape = pel as TShape;
					if (shape != null && shape.Diagram == this && (!filterDeleting || !shape.IsDeleting))
					{
						return shape;
					}
				}
			}
			return null;
		}
		///// <summary>
		///// Locate all existing shapes on this diagram corresponding to this element
		///// </summary>
		///// <param name="element">The element to search</param>
		///// <returns>An IEnumerable for enumeration through all existing shapes</returns>
		//public IEnumerable<ShapeElement> FindAllShapesForElement(ModelElement element)
		//{
		//    return FindAllShapesForElement<ShapeElement>(element, false);
		//}
		///// <summary>
		///// Locate all existing shapes on this diagram corresponding to this element
		///// </summary>
		///// <param name="element">The element to search</param>
		///// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		///// <returns>An IEnumerable for enumeration through all existing shapes</returns>
		//public IEnumerable<ShapeElement> FindAllShapesForElement(ModelElement element, bool filterDeleting)
		//{
		//    return FindAllShapesForElement<ShapeElement>(element, filterDeleting);
		//}
		/// <summary>
		/// Determines if an element has a shape on this diagram.
		/// </summary>
		/// <param name="element">The element to check</param>
		/// <returns>true if a shape exists on this diagram for the element</returns>
		public bool ElementHasShape(ModelElement element)
		{
			return ElementHasShape(element, false);
		}
		/// <summary>
		/// Determines if an element has a shape on this diagram.
		/// </summary>
		/// <param name="element">The element to check</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>true if a shape exists on this diagram for the element</returns>
		public bool ElementHasShape(ModelElement element, bool filterDeleting)
		{
			foreach (ShapeElement shapeElement in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(this, element, filterDeleting))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Setup our routing style.
		/// </summary>
		public override void OnInitialize()
		{
			base.OnInitialize();
			this.RoutingStyle = VGRoutingStyle.VGRouteNone;
			if (this.Subject == null)
			{
				ReadOnlyCollection<ORMModel> modelElements = this.Store.DefaultPartition.ElementDirectory.FindElements<ORMModel>();
				if (modelElements.Count > 0)
				{
					this.Associate(modelElements[0]);
				}
			}
		}
		/// <summary>See <see cref="ShapeElement.FixUpChildShapes"/>.</summary>
		public override ShapeElement FixUpChildShapes(ModelElement childElement)
		{
			return MultiShapeUtility.FixUpChildShapes(this, childElement);
		}
		#endregion // View Fixup Methods
		#region Customize appearance
		/// <summary>
		/// The Brush to use when drawing the background of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyBackgroundResource = new StyleSetResourceId("ORMArchitect", "StickyBackgroundResource");
		/// <summary>
		/// The Brush to use when drawing the foreground of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyForegroundResource = new StyleSetResourceId("ORMArchitect", "StickyForegroundResource");
		/// <summary>
		/// The brush or pen used to draw a link decorator as sticky
		/// </summary>
		public static readonly StyleSetResourceId StickyConnectionLineDecoratorResource = new StyleSetResourceId("ORMArchitect", "StickyConnectionLineDecorator");
		/// <summary>
		/// The brush or pen used to draw a link decorator as active. Generally corresponds to the role picker color
		/// </summary>
		public static readonly StyleSetResourceId ActiveConnectionLineDecoratorResource = new StyleSetResourceId("ORMArchitect", "ActiveConnectionLineDecorator");
		/// <summary>
		/// The brush used to draw a link as active. Generally corresponds to the role picker color.
		/// </summary>
		public static readonly StyleSetResourceId ActiveBackgroundResource = new StyleSetResourceId("ORMArchitect", "ActiveBackgroundResource");
		/// <summary>
		/// The brush used to draw the background for an item with errors.
		/// </summary>
		public static readonly StyleSetResourceId ErrorBackgroundResource = new StyleSetResourceId("ORMArchitect", "ErrorBackgroundResource");
		/// <summary>
		/// The brush used to draw the background for an item with errors when the shape is highlighted.
		/// </summary>
		public static readonly StyleSetResourceId HighlightedErrorBackgroundResource = new StyleSetResourceId("ORMArchitect", "HighlightedErrorBackgroundResource");
		/// <summary>
		/// A transparent brush.
		/// </summary>
		public static readonly StyleSetResourceId TransparentBrushResource = new StyleSetResourceId("ORMArchitect", "TransparentBrushResource");

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
				else
				{
					IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
					if (extenders != null)
					{
						for (int i = 0; i < extenders.Length && action == null; ++i)
						{
							action = extenders[i].GetMouseAction(this, activeView);
						}
					}
					if (action == null &&
						activeView.SelectedToolboxItemSupportsFilterString(ORMDiagram.ORMDiagramDefaultFilterString))
					{
						action = ToolboxAction;
					}
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
				(action != null || (activeView != null && activeView.Toolbox.GetSelectedToolboxItem() != null)))
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
					Store store = Utility.ValidateStore(Store);
					if (store != null)
					{
						IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
						if (extenders != null)
						{
							for (int i = 0; i < extenders.Length; ++i)
							{
								extenders[i].ExtendedShapeDisposed(this);
							}
						}
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
				return ResourceStrings.ORMDiagramAccessibleName;
			}
		}
		/// <summary>
		/// Return the component name as the accessible value
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return this.Name;
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
		protected object ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			ObjectType objectElement;
			ExclusionConstraint exclusionConstraint;
			SetConstraint setConstraint;
			FactType factType;
			if (null != (objectElement = element as ObjectType))
			{
				if (!ShouldDisplayObjectType(objectElement) &&
					!(forError is ObjectTypeDuplicateNameError))
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
			else if (null != (factType = element as FactType))
			{
				// For an implied FactType, select the associated role on the
				// nesting FactType.
				Objectification objectification;
				if (null != (objectification = factType.ImpliedByObjectification))
				{
					foreach (RoleBase roleBase in factType.RoleCollection)
					{
						RoleProxy proxy;
						ObjectifiedUnaryRole objectifiedUnaryRole;
						if (null != (proxy = roleBase as RoleProxy))
						{
							return proxy.TargetRole;
						}
						else if (null != (objectifiedUnaryRole = roleBase as ObjectifiedUnaryRole))
						{
							return objectifiedUnaryRole.TargetRole;
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
			else if (null != (setConstraint = element as SetConstraint))
			{
				// Internal constraints are displayed with the FactType
				LinkedElementCollection<FactType> factTypes;
				if ((setConstraint as IConstraint).ConstraintIsInternal &&
					1 == (factTypes = setConstraint.FactTypeCollection).Count)
				{
					return factTypes[0];
				}
			}
			return null;
		}
		object IProxyDisplayProvider.ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			return ElementDisplayedAs(element, forError);
		}
		#endregion // IProxyDisplayProvider Implementation
		#region IMergeElements implementation
		/// <summary>
		/// Implements <see cref="IMergeElements.MergeRelate"/>. Allows
		/// duplication of shapes across diagrams in the same model.
		/// </summary>
		protected new void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			ShapeElement shape = sourceElement as ShapeElement;
			if (shape != null && shape.ParentShape == null)
			{
				NestedChildShapes.Add(shape);
				MergeRelateShape(shape);
			}
		}
		/// <summary>
		/// Complete the merge of a top-level shape element into the diagram. Called immediately
		/// after the <paramref name="shape"/> element is added to the <see cref="ShapeElement.NestedChildShapes"/> collection.
		/// </summary>
		/// <param name="shape">The newly merged <see cref="ShapeElement"/></param>
		protected virtual void MergeRelateShape(ShapeElement shape)
		{
			object AllowMultipleShapes;
			Dictionary<object, object> topLevelContextInfo;
			bool containedAllowMultipleShapes;
			if (!(containedAllowMultipleShapes = (topLevelContextInfo = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
			{
				topLevelContextInfo.Add(AllowMultipleShapes, null);
			}

			ModelElement element = shape.ModelElement;
			FactType factType;
			ObjectType objectType;
			SetConstraint setConstraint;
			SetComparisonConstraint setComparisonConstraint;
			ModelNote modelNote;
			if (null != (factType = element as FactType))
			{
				FixupFactType(factType, shape as FactTypeShape, true);
			}
			else if (null != (objectType = element as ObjectType))
			{
				FixupObjectType(objectType, shape as ObjectTypeShape, true);
			}
			else if (null != (setConstraint = element as SetConstraint))
			{
				FixupConstraint(setConstraint, (ExternalConstraintShape)shape);
			}
			else if (null != (setComparisonConstraint = element as SetComparisonConstraint))
			{
				FixupConstraint(setComparisonConstraint, (ExternalConstraintShape)shape);
			}
			else if (null != (modelNote = element as ModelNote))
			{
				FixupModelNote(modelNote, (ModelNoteShape)shape);
			}

			if (!containedAllowMultipleShapes)
			{
				topLevelContextInfo.Remove(AllowMultipleShapes);
			}
		}
		void IMergeElements.MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			MergeRelate(sourceElement, elementGroup);
		}
		/// <summary>
		/// Make sure that all elements in <paramref name="verifyFactTypes"/>
		/// have a corresponding shape on the diagram or a pending shape in
		/// the <paramref name="elementGroupPrototype"/>
		/// </summary>
		/// <param name="verifyFactTypes">An <see cref="IList{FactType}"/> to verify</param>
		/// <param name="elementGroupPrototype">The <see cref="ElementGroupPrototype"/> that is being merged</param>
		/// <returns><see langword="true"/> if all <see cref="FactType"/>s are accounted for.</returns>
		private bool VerifyCorrespondingFactTypes(IList<FactType> verifyFactTypes, ElementGroupPrototype elementGroupPrototype)
		{
			int factCount = verifyFactTypes.Count;
			bool searchedPrototypes = elementGroupPrototype == null;
			FactType[] verifyFactTypes_Editable = null;
			IElementDirectory elementDirectory = null;
			ReadOnlyCollection<ProtoElement> rootElements = null;
			for (int i = 0; i < factCount; ++i)
			{
				FactType verifyFact = verifyFactTypes[i];
				if (null != verifyFact &&
					null == FindShapeForElement(verifyFact))
				{
					SubtypeFact subtypeFact = verifyFact as SubtypeFact;
					if (subtypeFact != null)
					{
						// Subtypes links are not directly prototyped. If the shape does not yet it exist,
						// then it will be created automatically if both the subtype and supertype already
						// exist are or available in the prototypes.
						ObjectType supertype = subtypeFact.Supertype;
						ObjectType subtype = subtypeFact.Subtype;
						bool haveSupertypeRepresentation = FindShapeForElement(supertype) != null;
						bool haveSubtypeRepresentation = FindShapeForElement(subtype) != null;
						if (!haveSupertypeRepresentation || !haveSubtypeRepresentation)
						{
							if (rootElements == null)
							{
								elementDirectory = Store.ElementDirectory;
								rootElements = elementGroupPrototype.RootProtoElements;
							}
							foreach (ProtoElement protoElement in rootElements)
							{
								PresentationElement testPel;
								FactType testFactType;
								ObjectType testObjectType;
								if (null != (testPel = elementDirectory.FindElement(protoElement.ElementId) as PresentationElement))
								{
									ModelElement testElement = testPel.ModelElement;
									if (null != (testObjectType = testElement as ObjectType) ||
										(null != (testFactType = testElement as FactType) &&
										null != (testObjectType = testFactType.NestingType)))
									{
										if (!haveSupertypeRepresentation)
										{
											if (testObjectType == supertype)
											{
												haveSupertypeRepresentation = true;
												if (haveSubtypeRepresentation)
												{
													break;
												}
											}
										}
										if (!haveSubtypeRepresentation)
										{
											if (testObjectType == subtype)
											{
												haveSubtypeRepresentation = true;
												if (haveSupertypeRepresentation)
												{
													break;
												}
											}
										}
									}
								}
							}
						}
						if (haveSupertypeRepresentation && haveSubtypeRepresentation)
						{
							continue;
						}
						return false;
					}
					if (searchedPrototypes)
					{
						return false;
					}
					searchedPrototypes = true;
					// See which prototype facts are being added. Create an editable list so
					// we can walk this once only.
					if (rootElements == null)
					{
						elementDirectory = Store.ElementDirectory;
						rootElements = elementGroupPrototype.RootProtoElements;
					}
					foreach (ProtoElement protoElement in rootElements)
					{
						PresentationElement testPel;
						FactType testFactType;
						if (null != (testPel = elementDirectory.FindElement(protoElement.ElementId) as PresentationElement) &&
							null != (testFactType = testPel.ModelElement as FactType))
						{
							int verifyIndex = verifyFactTypes.IndexOf(testFactType);
							if (verifyIndex != -1)
							{
								if (verifyFactTypes_Editable == null)
								{
									verifyFactTypes_Editable = new FactType[factCount];
									verifyFactTypes.CopyTo(verifyFactTypes_Editable, 0);
									for (int k = 0; k < i; ++k)
									{
										verifyFactTypes_Editable[k] = null;
									}
									verifyFactTypes = verifyFactTypes_Editable;
								}
								verifyFactTypes_Editable[verifyIndex] = null;
							}
						}
					}
					if (verifyFactTypes[i] != null)
					{
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// Extend CanMerge to allow duplication of shapes across diagrams in the same model
		/// </summary>
		protected override bool CanMerge(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
		{
			Store store = Store;
			PresentationElement pel;
			ModelElement element;
			object storeId;
			if (Partition == store.DefaultPartition &&
				elementGroupPrototype.SourceContext.ContextInfo.TryGetValue("SourceStore", out storeId) &&
				storeId != null &&
				(Guid)storeId == store.Id &&
				null != (pel = store.ElementDirectory.FindElement(rootElement.ElementId) as PresentationElement) &&
				null != (element = pel.ModelElement) &&
				(ShouldAddShapeForElement(element)))
			{
				return CanMergeElement(element, elementGroupPrototype);
			}
			return base.CanMerge(rootElement, elementGroupPrototype);
		}
		/// <summary>
		/// Provide element-specific testing to determine if an element can be merged or not.
		/// This provides a stronger test than <see cref="ShouldAddShapeForElement"/>, which does
		/// not consider pending shapes representing by the <paramref name="elementGroupPrototype"/>
		/// </summary>
		/// <param name="element">A <see cref="ModelElement"/> that does not currently have a shape on the
		/// diagram and has already passed the <see cref="ShouldAddShapeForElement"/> method.</param>
		/// <param name="elementGroupPrototype">The <see cref="ElementGroupPrototype"/> that is being merged</param>
		/// <returns><see langword="true"/> to allow merge.</returns>
		protected virtual bool CanMergeElement(ModelElement element, ElementGroupPrototype elementGroupPrototype)
		{
			bool retVal = true;
			SetConstraint setConstraint;
			SetComparisonConstraint setComparisonConstraint;
			if (null != (setConstraint = element as SetConstraint))
			{
				retVal = VerifyCorrespondingFactTypes(setConstraint.FactTypeCollection, elementGroupPrototype);
			}
			else if (null != (setComparisonConstraint = element as SetComparisonConstraint))
			{
				retVal = VerifyCorrespondingFactTypes(setComparisonConstraint.FactTypeCollection, elementGroupPrototype);
			}
			return retVal;
		}
		[NonSerialized]
		private ORMDesignerElementOperations myElementOperations;
		/// <summary>
		/// Modified element operations to allow duplication of shapes across diagrams
		/// in the same model.
		/// </summary>
		public override DesignSurfaceElementOperations ElementOperations
		{
			get
			{
				ORMDesignerElementOperations elementOperations = myElementOperations;
				if (elementOperations == null)
				{
					myElementOperations = elementOperations = new ORMDesignerElementOperations(Store);
				}
				return elementOperations;
			}
		}
		/// <summary>
		/// Reconstitute parts of a merged shape that are not duplicated as
		/// part of the copy closure.
		/// </summary>
		/// <param name="mergedShape">The new shape being merged</param>
		/// <param name="prototypeShape">The shape the new shape is based on</param>
		protected virtual void ReconstituteMergedShape(PresentationElement mergedShape, PresentationElement prototypeShape)
		{
			FactTypeShape factTypeShape;
			if (null != (factTypeShape = mergedShape as FactTypeShape))
			{
				// Role order display is not included in the copy because
				// of the direct links to the object model. We need to reattach
				// to existing roles, not create new ones as part of the prototype.
				FactTypeShape protoFactTypeShape = (FactTypeShape)prototypeShape;
				LinkedElementCollection<RoleBase> protoDisplayRoles = protoFactTypeShape.RoleDisplayOrderCollection;
				int displayRoleCount = protoDisplayRoles.Count;
				if (displayRoleCount != 0)
				{
					LinkedElementCollection<RoleBase> displayRoles = factTypeShape.RoleDisplayOrderCollection;
					for (int j = 0; j < displayRoleCount; ++j)
					{
						displayRoles.Add(protoDisplayRoles[j]);
					}
				}
			}
		}
		#region ORMDesignerElementOperations class
		/// <summary>
		/// Support duplication of shapes across diagrams in the same model
		/// </summary>
		protected class ORMDesignerElementOperations : DesignSurfaceElementOperations
		{
			/// <summary>
			/// Create custom operations to allow copying of shapes between
			/// diagrams in the same model
			/// </summary>
			/// <param name="store">The context <see cref="Store"/></param>
			public ORMDesignerElementOperations(Store store)
				: base((IServiceProvider)store, store)
			{
			}
			/// <summary>
			/// Mark all non-parented shape elements as root elements before propagating element group
			/// </summary>
			protected override void PropagateElementGroupContextToTransaction(ModelElement targetElement, ElementGroup elementGroup, Transaction t)
			{
				ReadOnlyCollection<ModelElement> rootElements = elementGroup.RootElements;
				ReadOnlyCollection<ModelElement> elements = elementGroup.ModelElements;
				int elementCount = elements.Count;
				for (int i = 0; i < elementCount; ++i)
				{
					ShapeElement shape = elements[i] as ShapeElement;
					if (shape != null && shape.ParentShape == null && !rootElements.Contains(shape))
					{
						elementGroup.MarkAsRoot(shape);
					}
				}
				base.PropagateElementGroupContextToTransaction(targetElement, elementGroup, t);
			}
			/// <summary>
			/// Presort elements before copying. The element order is persistent through the
			/// copy operation. Presorting the elements makes merging much easier at the drop target.
			/// Sorting can be modified by overriding the <see cref="CopyOrderComparer"/> property,
			/// which uses the <see cref="CompareElementsForCopy"/> method as the default sort.
			/// </summary>
			public override void Copy(IDataObject data, ICollection<ModelElement> elements, ClosureType closureType, PointF sourcePosition)
			{
				int elementCount = elements.Count;
				if (elementCount > 1)
				{
					int remainingCount = elementCount;
					foreach (ModelElement mel in elements)
					{
						if (!FilterCopiedElement(mel))
						{
							--remainingCount;
						}
					}
					if (remainingCount != 0)
					{
						Comparison<ModelElement> comparer = CopyOrderComparer;
						if (comparer != null || remainingCount != elementCount)
						{
							ModelElement[] modifiedElements = new ModelElement[remainingCount];
							if (remainingCount == elementCount)
							{
								elements.CopyTo(modifiedElements, 0);
							}
							else
							{
								int i = -1;
								foreach (ModelElement mel in elements)
								{
									if (FilterCopiedElement(mel))
									{
										modifiedElements[++i] = mel;
									}
								}
							}
							if (comparer != null)
							{
								Array.Sort<ModelElement>(modifiedElements, comparer);
							}
							elements = modifiedElements;
						}
					}
				}
				base.Copy(data, elements, closureType, sourcePosition);
			}
			/// <summary>
			/// Use as a compare routine to presort copied elements. Override and
			/// return null for no sort. The default compare is available in the
			/// <see cref="CompareElementsForCopy"/> method.
			/// </summary>
			protected virtual Comparison<ModelElement> CopyOrderComparer
			{
				get
				{
					// UNDONE: Wire the CopyOrderComparer into the ShapeExtension mechanism
					return new Comparison<ModelElement>(CompareElementsForCopy);
				}
			}
			/// <summary>
			/// Remove elements from a set of copied elements
			/// </summary>
			/// <param name="element"><see cref="ModelElement">Element</see> to filter.</param>
			/// <returns>Return <see langword="true"/> to include the <paramref name="element"/>.</returns>
			protected virtual bool FilterCopiedElement(ModelElement element)
			{
				return !(element is IAutoCreatedSelectableShape);
			}
			/// <summary>
			/// Reorder elements so that <see cref="ExternalConstraintShape"/> elements
			/// are copied last. Used by the <see cref="CopyOrderComparer"/> and <see cref="Copy"/> methods.
			/// </summary>
			protected static int CompareElementsForCopy(ModelElement element1, ModelElement element2)
			{
				if (element1 == element2)
				{
					return 0;
				}
				int retVal = 0;
				ExternalConstraintShape constraintShape1 = element1 as ExternalConstraintShape;
				ExternalConstraintShape constraintShape2 = element2 as ExternalConstraintShape;
				if (constraintShape1 == null)
				{
					if (constraintShape2 != null)
					{
						retVal = -1;
					}
				}
				else if (constraintShape2 == null)
				{
					retVal = 1;
				}
				return retVal;
			}
			/// <summary>
			/// Support shape merging by reattaching model elements to the shapes
			/// before an attempt is made to merge the shapes into the diagram.
			/// </summary>
			protected override void OnElementsReconstituted(MergeElementGroupEventArgs e)
			{
				ORMDiagram diagram;
				Store store;
				if (null != (diagram = e.TargetElement as ORMDiagram) &&
					diagram.Partition == (store = diagram.Store).DefaultPartition)
				{
					ElementGroup group = e.ElementGroup;
					ReadOnlyCollection<ModelElement> elements = group.ModelElements;
					ReadOnlyCollection<ProtoElement> protoElements = e.ElementGroupPrototype.ProtoElements;
					DomainDataDirectory dataDirectory = store.DomainDataDirectory;
					IElementDirectory elementDirectory = store.ElementDirectory;
					DomainClassInfo baseShapeClassInfo = dataDirectory.FindDomainClass(ORMBaseShape.DomainClassId);
					int elementCount = elements.Count;
					Debug.Assert(elementCount == protoElements.Count);
					for (int i = 0; i < elementCount; ++i)
					{
						PresentationElement pel;
						PresentationElement protoShape;
						ModelElement backingElement;
						if (null != (pel = elements[i] as PresentationElement) &&
							null != (protoShape = elementDirectory.FindElement(protoElements[i].ElementId) as PresentationElement) &&
							null != (backingElement = protoShape.ModelElement))
						{
							pel.Associate(backingElement);
							// Defer to an overridable callback for additional shape-specific fixup
							diagram.ReconstituteMergedShape(pel, protoShape);
						}
					}
				}
				base.OnElementsReconstituted(e);
			}
		}
		#endregion // ORMDesignerElementOperations class
		#endregion // IMergeElements implementation
		#region ShapeExtension support
		/// <summary>
		/// Add loaded extension attributes to the standard toolbox items attributes
		/// </summary>
		public override ICollection TargetToolboxItemFilterAttributes
		{
			get
			{
				ICollection baseAttributes = base.TargetToolboxItemFilterAttributes;
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					ICollection[] extenderAttributeSets = null;
					int extenderCount = extenders.Length;
					int extenderAttributeCount = 0;
					for (int i = 0; i < extenderCount; ++i)
					{
						ICollection extenderAttributes = extenders[i].GetToolboxFilterAttributes() as ICollection;
						int attributeCount;
						if (extenderAttributes != null &&
							0 != (attributeCount = extenderAttributes.Count))
						{
							extenderAttributeCount += attributeCount;
							(extenderAttributeSets ?? (extenderAttributeSets = new ICollection[extenderCount]))[i] = extenderAttributes;
						}
					}
					if (extenderAttributeCount != 0)
					{
						int baseCount = (baseAttributes != null) ? baseAttributes.Count : 0;
						ToolboxItemFilterAttribute[] allAttributes = new ToolboxItemFilterAttribute[baseCount + extenderAttributeCount];
						baseAttributes.CopyTo(allAttributes, 0);
						int copyToIndex = baseCount;
						for (int i = 0; i < extenderCount; ++i)
						{
							ICollection extenderCollection = extenderAttributeSets[i];
							if (extenderCollection != null)
							{
								extenderCollection.CopyTo(allAttributes, copyToIndex);
								copyToIndex += extenderCollection.Count;
							}
						}
						return allAttributes;
					}
				}
				return baseAttributes;
			}
		}
		/// <summary>
		/// Defer child shape creation to <see cref="IShapeExtender{ORMDiagram}"/> implementations as needed
		/// </summary>
		protected override ShapeElement CreateChildShape(ModelElement element)
		{
			ShapeElement retVal = base.CreateChildShape(element);
			if (retVal == null)
			{
				IShapeExtender<ORMDiagram>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ORMDiagram>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && retVal == null; ++i)
					{
						retVal = extenders[i].CreateChildShape(this, element);
					}
				}
			}
			return retVal;
		}
		#endregion // ShapeExtension support
	}
	#region ORMShapeDomainModel toolbox initialization
	[ModelingToolboxItemProvider("ToolboxInitializer")]
	partial class ORMShapeDomainModel
	{
		private sealed class ToolboxInitializer : IModelingToolboxItemProvider
		{
			#region IModelingToolboxItemProvider Implementation
			IList<ModelingToolboxItem> IModelingToolboxItemProvider.CreateToolboxItems(IServiceProvider serviceProvider)
			{
				IList<ModelingToolboxItem> items;
				FrameworkDomainModel.InitializingToolboxItems = true;
				try
				{
					items = new ORMShapeToolboxHelper(serviceProvider).CreateToolboxItems();
				}
				finally
				{
					FrameworkDomainModel.InitializingToolboxItems = false;
				}

				// Add additional filter strings. These are not easily specified in the .dsl file, so we
				// do it here.
				IDictionary<string, int> itemIndexDictionary = ToolboxHelperUtility.CreateIdentifierToIndexMap(items);

				ToolboxItemFilterAttribute attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
				ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintItemId, attribute);

				attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
				ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintConnectorItemId, attribute);

				attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramModelNoteFilterString, ToolboxItemFilterType.Allow);
				ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxModelNoteItemId, attribute);

				attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramExternalConstraintFilterString, ToolboxItemFilterType.Allow);
				string[] itemIds = new string[] {
					ResourceStrings.ToolboxEqualityConstraintItemId,
					ResourceStrings.ToolboxExclusionConstraintItemId,
					ResourceStrings.ToolboxExclusiveOrConstraintItemId,
					ResourceStrings.ToolboxExternalUniquenessConstraintItemId,
					ResourceStrings.ToolboxInclusiveOrConstraintItemId,
					ResourceStrings.ToolboxRingConstraintItemId,
					ResourceStrings.ToolboxSubsetConstraintItemId,
					ResourceStrings.ToolboxFrequencyConstraintItemId};
				for (int i = 0; i < itemIds.Length; ++i)
				{
					ToolboxHelperUtility.AddFilterAttribute(items, itemIndexDictionary, itemIds[i], attribute);
				}
				return items;
			}
			int IModelingToolboxItemProvider.ToolboxItemPositionOffset
			{
				get
				{
					return 0;
				}
			}
			#endregion // IModelingToolboxItemProvider Implementation
		}
	}
	#endregion // ORMShapeDomainModel toolbox initialization
	#region ORMDiagramDynamicColor enum
	/// <summary>
	/// Specify the color role for a dynamic shape color
	/// </summary>
	[TypeConverter(typeof(EnumConverter<ORMDiagramDynamicColor, ORMDiagram>))]
	[ResourceAccessorCategory(typeof(ORMDiagram), "ORMDiagramDynamicColor.Category")]
	public enum ORMDiagramDynamicColor
	{
		/// <summary>
		/// Get the background color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.Background.Description")]
		Background,
		/// <summary>
		/// Get the foreground color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.ForegroundGraphics.Description")]
		ForegroundGraphics,
		/// <summary>
		/// Shape text color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.ForegroundText.Description")]
		ForegroundText,
		/// <summary>
		/// The outline color for a shape
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.Outline.Description")]
		Outline,
		/// <summary>
		/// Constraint color (alethic modality)
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.Constraint.Description")]
		Constraint,
		/// <summary>
		/// Deontic constraint color
		/// </summary>
		[ResourceAccessorDescription(typeof(ORMDiagram), "ORMDiagramDynamicColor.DeonticConstraint.Description")]
		DeonticConstraint,
	}
	#endregion // ORMDiagramDynamicColor enum
	#region IDynamicColorSetConsumer implementation
	partial class ORMShapeDomainModel : IDynamicColorSetConsumer<ORMDiagram>
	{
		#region IDynamicColorSetConsumer Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorSetConsumer{ORMDiagram}.DynamicColorSet"/>
		/// </summary>
		protected static Type DynamicColorSet
		{
			get
			{
				return typeof(ORMDiagramDynamicColor);
			}
		}
		Type IDynamicColorSetConsumer<ORMDiagram>.DynamicColorSet
		{
			get
			{
				return DynamicColorSet;
			}
		}
		#endregion // IDynamicColorSetConsumer Implementation
	}
	#endregion // IDynamicColorSetConsumer implementation
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
	#region ORMPlacementOption enum
	/// <summary>
	/// Controls the actions taken when placing a <see cref="ModelElement"/> on a <see cref="Diagram"/>.
	/// </summary>
	[Serializable]
	public enum ORMPlacementOption
	{
		/// <summary>
		/// No special placement actions should be taken.
		/// </summary>
		None = 0,
		/// <summary>
		/// Select a <see cref="ShapeElement"/> for the <see cref="ModelElement"/> on the <see cref="Diagram"/> if one already exists.
		/// </summary>
		SelectIfNotPlaced = 1,
		/// <summary>
		/// Always add a new <see cref="ShapeElement"/> for the <see cref="ModelElement"/>.
		/// </summary>
		AllowMultipleShapes = 2,
	}
	#endregion // ORMPlacementOption enum
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

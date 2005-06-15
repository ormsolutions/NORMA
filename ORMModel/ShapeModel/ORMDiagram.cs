using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;
namespace Northface.Tools.ORM.ShapeModel
{
	#region IStickyObject interface
	/// <summary>
	/// Interface for implementing "Sticky" selections.  Presentation elements that are sticky
	/// will maintain their selected status when compatible objects are clicked.
	/// </summary>
	[CLSCompliant(true)]
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
	public partial class ORMDiagram
	{
		#region Toolbox filter strings
		/// <summary>
		/// The filter string used for simple actions
		/// </summary>
		public const string ORMDiagramDefaultFilterString = "ORMDiagramDefaultFilterString";
		/// <summary>
		/// The filter string used to create an external constraint. Very similar to a
		/// normal action, except the external constraint connector is activated on completion
		/// of the action.
		/// </summary>
		public const string ORMDiagramExternalConstraintFilterString = "ORMDiagramExternalConstraintFilterString";
		/// <summary>
		/// The filter string used to connect role sequences to external constraints
		/// </summary>
		public const string ORMDiagramConnectExternalConstraintFilterString = "ORMDiagramConnectExternalConstraintFilterString";
		/// <summary>
		/// The filter string used to create subtype relationships between object types
		/// </summary>
		public const string ORMDiagramCreateSubtypeFilterString = "ORMDiagramCreateSubtypeFilterString";
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
		public const string ORMDiagramConnectRoleFilterString = "ORMDiagramConnectRoleFilterString";
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
			ObjectType objType;
			FactType factType;
			ObjectTypePlaysRole objectTypePlaysRole;
			if (null != (factType = element as FactType))
			{
				if (factType is SubtypeFact)
				{
					return true;
				}
				return ShouldDisplayPartOfReferenceMode(factType);
			}
			else if (null != (objectTypePlaysRole = element as ObjectTypePlaysRole))
			{
				if (objectTypePlaysRole.PlayedRoleCollection.FactType is SubtypeFact)
				{
					return false;
				}
				return ShouldDisplayPartOfReferenceMode(objectTypePlaysRole);
			}
			else if (element is ExternalFactConstraint ||
					 element is SingleColumnExternalConstraint ||
					 element is MultiColumnExternalConstraint ||
					 element is RoleHasValueRangeDefinition)
			{
				return true;
			}
			else if (null != (objType = element as ObjectType))
			{
				if (objType.NestedFactType == null)
				{
					return ShouldDisplayPartOfReferenceMode(objType);
				}
				else
				{
					return false;
				}
			}
			return base.ShouldAddShapeForElement(element);
		}
		/// <summary>
		/// Function to determine if a fact type, which may be participating
		/// in a reference mode pattern, should be displayed.
		/// </summary>
		private bool ShouldDisplayPartOfReferenceMode(FactType factType)
		{
			foreach (InternalConstraint constraint in factType.InternalConstraintCollection)
			{
				ObjectType entity = constraint.PreferredIdentifierFor;
				if (entity != null)
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
			Role role = objectTypePlaysRole.PlayedRoleCollection;
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
				RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
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
			ShapeElement shapeElement = FindShapeForElement(objectType);
			ObjectTypeShape objectTypeShape = shapeElement as ObjectTypeShape;
			if (objectTypeShape != null)
			{
				ObjectType obj = objectTypeShape.AssociatedObjectType;
				if (obj.HasReferenceMode)
				{
					return !objectTypeShape.ExpandRefMode;
				}
			}
			return false;
		}
		/// <summary>
		/// An object type is displayed as an ObjectTypeShape unless it is
		/// objectified, in which case we display it as an ObjectifiedFactTypeNameShape
		/// </summary>
		/// <param name="element">The element to test. Expecting an ObjectType.</param>
		/// <param name="shapeTypes">The choice of shape types</param>
		/// <returns></returns>
		protected override MetaClassInfo ChooseShape(ModelElement element, IList shapeTypes)
		{
			Guid classId = element.MetaClassId;
			if (classId == ObjectType.MetaClassGuid)
			{
				return ChooseShapeTypeForObjectType((ObjectType)element, shapeTypes);
			}
			Debug.Assert(false); // We're only expecting an ObjectType here
			return base.ChooseShape(element, shapeTypes);
		}
		/// <summary>
		/// Helper function to choose the appropriate shape for an ObjectType
		/// UNDONE: The original plan was to override ChooseParentShape here to switch the
		/// parent to a FactType. However, the childShape passed to ChooseParentShape is not
		/// yet attached to its backing ModelElement, so the override is essentially useless,
		/// and we need to duplicate the ChooseShape code on FactTypeShape itself.
		/// </summary>
		/// <param name="element">ObjectType</param>
		/// <param name="shapeTypes">IList of MetaClassInfo</param>
		/// <returns></returns>
		public static MetaClassInfo ChooseShapeTypeForObjectType(ObjectType element, IList shapeTypes)
		{
			Guid shapeClassId = (element.NestedFactType == null) ? ObjectTypeShape.MetaClassGuid : ObjectifiedFactTypeNameShape.MetaClassGuid;
			foreach (MetaClassInfo classInfo in shapeTypes)
			{
				if (classInfo.Id == shapeClassId)
				{
					return classInfo;
				}
			}
			Debug.Assert(false); // Missed a shape associated with ObjectType
			return null;
		}
		/// <summary>
		/// Defer to ConfiguringAsChildOf for ORMBaseShape children
		/// </summary>
		/// <param name="child">The child being configured</param>
		protected override void OnChildConfiguring(ShapeElement child)
		{
			ORMBaseShape baseShape;
			RolePlayerLink roleLink;
			ExternalConstraintLink constraintLink;
			ValueRangeLink valueRangeLink;
			SubtypeLink subtypeLink;
			if (null != (baseShape = child as ORMBaseShape))
			{
				baseShape.ConfiguringAsChildOf(this);
			}
			else if (null != (roleLink = child as RolePlayerLink))
			{
				roleLink.ConfiguringAsChildOf(this);
			}
			else if (null != (valueRangeLink = child as ValueRangeLink))
			{
				valueRangeLink.ConfiguringAsChildOf(this);
			}
			else if (null != (constraintLink = child as ExternalConstraintLink))
			{
				constraintLink.ConfiguringAsChildOf(this);
			}
			else if (null != (subtypeLink = child as SubtypeLink))
			{
				subtypeLink.ConfiguringAsChildOf(this);
			}
		}
		/// <summary>
		/// Locate an existing shape on this diagram corresponding to this element
		/// </summary>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		public ShapeElement FindShapeForElement(ModelElement element)
		{
			foreach (PresentationElement pel in element.AssociatedPresentationElements)
			{
				ShapeElement shape = pel as ShapeElement;
				if (shape != null && shape.Diagram == this)
				{
					return shape;
				}
			}
			return null;
		}
		/// <summary>
		/// Locate an existing typed shape on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="ShapeType">The type of the shape to return</typeparam>
		/// <param name="element">The element to search</param>
		/// <returns>An existing shape, or null if not found</returns>
		[CLSCompliant(false)]
		public ShapeType FindShapeForElement<ShapeType>(ModelElement element) where ShapeType : ShapeElement
		{
			foreach (PresentationElement pel in element.AssociatedPresentationElements)
			{
				ShapeType shape = pel as ShapeType;
				if (shape != null && shape.Diagram == this)
				{
					return shape;
				}
			}
			return null;
		}
		#endregion // View Fixup Methods
		#region Customize appearance
		/// <summary>
		/// The Brush to use when drawing the background of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyBackgroundResource = new StyleSetResourceId("Northface", "StickyBackgroundResource");
		/// <summary>
		/// The Brush to use when drawing the foreground of a sticky object.
		/// </summary>
		public static readonly StyleSetResourceId StickyForegroundResource = new StyleSetResourceId("Northface", "StickyForegroundResource");
		/// <summary>
		/// 
		/// </summary>
		/// <param name="classStyleSet"></param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);

			ORMDesignerFontsAndColors colorService = ORMDesignerPackage.FontAndColorService;
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
				return .0125;
			}
		}
		#endregion // Customize appearance
		#region Toolbox initialization
		/// <summary>
		/// Initialize toolbox items. All items are thrown on the diagram (it doesn't
		/// really matter what object we put them on).
		/// </summary>
		/// <param name="toolboxItem"></param>
		/// <returns></returns>
		public override ElementGroupPrototype InitializeToolboxItem(ModelingToolboxItem toolboxItem)
		{
			Store store = Store;
			string itemId = toolboxItem.Id;
			ElementGroup group = new ElementGroup(store);
			ElementGroupPrototype retVal = null;
			int roleArity = 0;
			switch (itemId)
			{
				case ResourceStrings.ToolboxEntityTypeItemId:
					ObjectType entityType = ObjectType.CreateObjectType(store);
					group.AddGraph(entityType);
					retVal = group.CreatePrototype(entityType);
					break;
				case ResourceStrings.ToolboxValueTypeItemId:
					ObjectType valueType = ObjectType.CreateObjectType(store);
					group.AddGraph(valueType);
					// Do not try to set the IsValueType property here. IsValueType picks
					// up the default data type for the model, which can only be done
					// when the model is known. Instead, flag the element so that it
					// can be set during MergeRelate on the model.
					group.UserData = "VALUETYPE";
					retVal = group.CreatePrototype(valueType);
					break;
				case ResourceStrings.ToolboxUnaryFactTypeItemId:
					roleArity = 1;
					break;
				case ResourceStrings.ToolboxBinaryFactTypeItemId:
					roleArity = 2;
					break;
				case ResourceStrings.ToolboxTernaryFactTypeItemId:
					roleArity = 3;
					break;
				case ResourceStrings.ToolboxInternalUniquenessConstraintItemId:
					InternalUniquenessConstraint iuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
					group.AddGraph(iuc);
					retVal = group.CreatePrototype(iuc);
					break;
				case ResourceStrings.ToolboxExternalUniquenessConstraintItemId:
					ExternalUniquenessConstraint euc = ExternalUniquenessConstraint.CreateExternalUniquenessConstraint(store);
					group.AddGraph(euc);
					retVal = group.CreatePrototype(euc);
					break;
				case ResourceStrings.ToolboxEqualityConstraintItemId:
					EqualityConstraint eqc = EqualityConstraint.CreateEqualityConstraint(store);
					group.AddGraph(eqc);
					retVal = group.CreatePrototype(eqc);
					break;
				case ResourceStrings.ToolboxExclusionConstraintItemId:
					ExclusionConstraint exc = ExclusionConstraint.CreateExclusionConstraint(store);
					group.AddGraph(exc);
					retVal = group.CreatePrototype(exc);
					break;
				case ResourceStrings.ToolboxInclusiveOrConstraintItemId:
					DisjunctiveMandatoryConstraint dmc = DisjunctiveMandatoryConstraint.CreateDisjunctiveMandatoryConstraint(store);
					group.AddGraph(dmc);
					retVal = group.CreatePrototype(dmc);
					break;
				case ResourceStrings.ToolboxExclusiveOrConstraintItemId:
					// Intentionally unprototyped item (for now)
					break;
				case ResourceStrings.ToolboxSubsetConstraintItemId:
					SubsetConstraint sc = SubsetConstraint.CreateSubsetConstraint(store);
					group.AddGraph(sc);
					retVal = group.CreatePrototype(sc);
					break;
				case ResourceStrings.ToolboxFrequencyConstraintItemId:
					FrequencyConstraint fc = FrequencyConstraint.CreateFrequencyConstraint(store);
					group.AddGraph(fc);
					retVal = group.CreatePrototype(fc);
					break;
				case ResourceStrings.ToolboxRoleConnectorItemId:
				case ResourceStrings.ToolboxExternalConstraintConnectorItemId:
				case ResourceStrings.ToolboxSubtypeConnectorItemId:
					// Intentionally unprototyped item
					break;
				default:
					Debug.Assert(false); // Unknown Id
					break;
			}
			if (retVal == null)
			{
				if (roleArity != 0)
				{
					FactType factType = FactType.CreateFactType(store);
					RoleMoveableCollection roles = factType.RoleCollection;
					for (int i = 0; i < roleArity; ++i)
					{
						Role role = Role.CreateRole(store);
						roles.Add(role);
						group.AddGraph(role);
					}
					group.AddGraph(factType);
					retVal = group.CreatePrototype(factType);
				}
			}
			return retVal;
		}
		#endregion // Toolbox initialization
		#region Toolbox support
		/// <summary>
		/// Enable our toolbox actions. Additional filters recognized in this
		/// routine are added in ORMEditorFactory.GetToolboxItems.
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
						//toolbox.SetSelectedToolboxItem(item); // UNDONE: Crashes, not sure why
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
		public  ExternalConstraintAction ExternalConstraintAction
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
							InternalUniquenessConstraint constraint = action.AddedConstraint;
							FactTypeShape addedToShape = action.DropTargetShape;
							Debug.Assert(constraint != null); // ActionCompleted should be false otherwise
							InternalUniquenessConstraintConnectAction.ChainMouseAction(addedToShape, constraint, e.DiagramClientView);
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
		#endregion // Toolbox support
		#region Other base overrides
		/// <summary>
		/// Clean up disposable members (connection actions)
		/// </summary>
		/// <param name="disposing">Do stuff if true</param>
		protected override void Dispose(bool disposing)
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
		/// <summary>
		/// Set the base font based on the font and color settings.
		/// UNDONE: This affects the size right now, but not the font name
		/// </summary>
		protected override Font BaseFontFromEnvironment
		{
			get
			{
				return ORMDesignerPackage.FontAndColorService.GetFont();
			}
		}
		#endregion // Other base overrides
		#region Display Properties
		/// <summary>
		/// Retrieve the component name for the property grid. The
		/// component name is displayed bolded in the property grid dropdown
		/// before the class name (retrieved from GetClassName)
		/// </summary>
		/// <returns></returns>
		public override string GetComponentName()
		{
			ModelElement element = ModelElement;
			return (element != null) ? element.GetComponentName() : base.GetComponentName();
		}
		/// <summary>
		/// Crash fix, the shell is calling back after the store is disposed. Catch the case.
		/// </summary>
		public override string GetClassName()
		{
			return Store.Disposed ? GetType().Name : base.GetClassName();
		}
		/// <summary>
		/// Block display of the diagram's name, which is displayed beside the
		/// Name for the underlying model if we let it through
		/// </summary>
		/// <param name="metaAttrInfo">MetaAttributeInfo</param>
		/// <returns>false for Diagram.Name, defers to base for all other attributes</returns>
		public override bool ShouldCreatePropertyDescriptor(MetaAttributeInfo metaAttrInfo)
		{
			Guid attributeGuid = metaAttrInfo.Id;
			if (attributeGuid == Diagram.NameMetaAttributeGuid)
			{
				return false;
			}
			return base.ShouldCreatePropertyDescriptor(metaAttrInfo);
		}
		#endregion // Display Properties
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
				(startLuminosity + luminosityFixedDelta + (int)((double)(luminosityCheck - startLuminosity)/luminosityCheck * luminosityIncrementalDelta));
		}
		#endregion // Utility Methods
		#region Deserialization Fixup
		/// <summary>
		/// Return all deserialization fixup listeners for the presentation model
		/// </summary>
		[CLSCompliant(false)]
		public static IEnumerable<IDeserializationFixupListener> DeserializationFixupListeners
		{
			get
			{
				yield return new DisplayRolePlayersFixupListener();
				yield return new DisplayExternalConstraintLinksFixupListener();
				yield return new DisplaySubtypeLinkFixupListener();
				yield return new DisplayRoleValueRangeDefinitionFixupListener();
				yield return new DisplayValueTypeValueRangeDefinitionFixupListener();
				yield return new EliminateOrphanedShapesFixupListener();
			}
		}
		#endregion // Deserialization Fixup
	}
}

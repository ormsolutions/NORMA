using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
namespace Northface.Tools.ORM.ShapeModel
{
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
			if (element is FactType ||
				element is ObjectTypePlaysRole ||
				element is ExternalFactConstraint ||
				element is SingleColumnExternalConstraint ||
				element is MultiColumnExternalConstraint)
			{
				return true;
			}
			else if (null != (objType = element as ObjectType))
			{
				return objType.NestedFactType == null;
			}
			return base.ShouldAddShapeForElement(element);
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
			if (null != (baseShape = child as ORMBaseShape))
			{
				baseShape.ConfiguringAsChildOf(this);
			}
			else if (null != (roleLink = child as RolePlayerLink))
			{
				// UNDONE: Move this chunk of code elsewhere more specific
				// to a roleplayer link

				// If we're already connected then walk away
				if (roleLink.FromShape == null && roleLink.ToShape == null)
				{
					ObjectTypePlaysRole modelLink = roleLink.ModelElement as ObjectTypePlaysRole;
					ObjectType rolePlayer = modelLink.RolePlayer;
					FactType nestedFact = rolePlayer.NestedFactType;
					NodeShape fromShape;
					NodeShape toShape;
					if (null != (fromShape = FindShapeForElement(modelLink.PlayedRoleCollection.FactType) as NodeShape) &&
						null != (toShape = FindShapeForElement((nestedFact == null) ? rolePlayer as ModelElement : nestedFact) as NodeShape))
					{
						roleLink.Connect(fromShape, toShape);
					}
				}
			}
			else if (null != (constraintLink = child as ExternalConstraintLink))
			{
				// UNDONE: Move this chunk of code elsewhere more specific
				// to a roleplayer link

				// If we're already connected then walk away
				if (constraintLink.FromShape == null && constraintLink.ToShape == null)
				{
					IFactConstraint modelLink = constraintLink.ModelElement as IFactConstraint;
					FactType attachedFact = modelLink.FactType;
					IConstraint constraint = modelLink.Constraint;
					NodeShape fromShape;
					NodeShape toShape;
					if (null != (fromShape = FindShapeForElement(constraint as ModelElement) as NodeShape) &&
						null != (toShape = FindShapeForElement(attachedFact) as NodeShape))
					{
						constraintLink.Connect(fromShape, toShape);
					}
				}
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
		#endregion // View Fixup Methods
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
					valueType.IsValueType = true;
					group.AddGraph(valueType);
					group.AddGraph(valueType.DataType);
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
				case ResourceStrings.ToolboxRoleConnectorItemId:
				case ResourceStrings.ToolboxExternalConstraintConnectorItemId:
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
				// (or over a warning tooltip) during a chained mouse action cancels the action.
				// See corresponding code in ExternalConstraintConnectAction.ChainMouseAction.
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
		private ExternalConstraintConnectAction myExternalConstraintConnectAction;
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
		private InternalUniquenssConstraintAction myInternalUniquenessConstraintAction;
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
		public InternalUniquenssConstraintAction InternalUniquenessConstraintAction
		{
			get
			{
				if (myInternalUniquenessConstraintAction == null)
				{
					myInternalUniquenessConstraintAction = CreateInternalUniquenessConstraintAction();
					myInternalUniquenessConstraintAction.AfterMouseActionDeactivated += delegate(object sender, DiagramEventArgs e)
					{
						InternalUniquenssConstraintAction action = sender as InternalUniquenssConstraintAction;
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
		protected virtual InternalUniquenssConstraintAction CreateInternalUniquenessConstraintAction()
		{
			return new InternalUniquenssConstraintAction(this);
		}
		#endregion Internal uniqueness constraint action
		#region Role drag action
		private RoleDragPendingAction myRoleDragPendingAction;
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
			
			// Use a sliding scale to brighten colors
			const int luminosityCheck = 160;
			const int luminosityFixedDelta = 60;
			const int luminosityIncrementalDelta = 30;
			const double luminosityFactor = 0.9;
			return (startLuminosity >= luminosityCheck) ?
				(int)(startLuminosity * luminosityFactor) :
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
				yield return new EliminateOrphanedShapesFixupListener();
			}
		}
		#endregion // Deserialization Fixup
	}
}

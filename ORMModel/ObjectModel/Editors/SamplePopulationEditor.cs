using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	/// <summary>
	/// Editor Control for changing sample instances
	/// </summary>
	public partial class SamplePopulationEditor : UserControl
	{
		#region Member Variables
		private SamplePopulationBaseBranch myBranch;
		private ObjectType mySelectedValueType;
		private ObjectType mySelectedEntityType;
		private FactType mySelectedFactType;
		#endregion // Member Variables
		#region Static Variables
		/// <summary>
		/// Provides a ref to the tree control from nested objects
		/// </summary>
		private static VirtualTreeControl TreeControl;
		#endregion // Static Variables
		#region Constructor
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SamplePopulationEditor()
		{
			InitializeComponent();
			SamplePopulationEditor.TreeControl = this.vtrSamplePopulation;
		}
		#endregion
		#region Properties
		/// <summary>
		/// The current value type being edited by the control
		/// </summary>
		public ObjectType SelectedValueType
		{
			get
			{
				return this.mySelectedValueType;
			}
			set
			{
				if (!object.ReferenceEquals(value, mySelectedValueType))
				{
					this.mySelectedValueType = value;
					bool visibility;
					if (visibility = (value != null))
					{
						PopulateControlForValueType();
						mySelectedEntityType = null;
						mySelectedFactType = null;
					}
					AdjustVisibility(visibility);
				}
			}
		}

		/// <summary>
		/// The current entity type being edited by the control
		/// </summary>
		public ObjectType SelectedEntityType
		{
			get
			{
				return this.mySelectedEntityType;
			}
			set
			{
				if (!object.ReferenceEquals(value, mySelectedEntityType))
				{
					this.mySelectedEntityType = value;
					bool visibility;
					if (visibility = (value != null))
					{
						PopulateControlForEntityType();
						mySelectedValueType = null;
						mySelectedFactType = null;
					}
					AdjustVisibility(visibility);
				}
			}
		}

		/// <summary>
		/// The current fact type being edited by the control
		/// </summary>
		public FactType SelectedFactType
		{
			get
			{
				return this.mySelectedFactType;
			}
			set
			{
				if (!object.ReferenceEquals(value, mySelectedFactType))
				{
					this.mySelectedFactType = value;
					bool visibility;
					if (visibility = (value != null))
					{
						PopulateControlForFactType();
						mySelectedValueType = null;
						mySelectedEntityType = null;
					}
					AdjustVisibility(visibility);
				}
			}
		}

		/// <summary>
		/// Returns true if a reading is currently in edit mode.
		/// </summary>
		public bool InLabelEdit
		{
			get
			{
				return vtrSamplePopulation.InLabelEdit;
			}
		}

		/// <summary>
		/// Returns the active label edit control, or null
		/// </summary>
		public Control LabelEditControl
		{
			get
			{
				return vtrSamplePopulation.LabelEditControl;
			}
		}

		/// <summary>
		/// Returns true if the pane is active
		/// </summary>
		public bool IsPaneActive
		{
			get
			{
				if (vtrSamplePopulation != null)
				{
					ContainerControl sc = this.ActiveControl as ContainerControl;
					if (sc != null)
					{
						return object.ReferenceEquals(vtrSamplePopulation, sc.ActiveControl);
					}
				}
				return false;
			}
		}
		#endregion
		#region PopulateControl and Helpers
		private void PopulateControlForValueType()
		{
			Debug.Assert(mySelectedValueType != null);
			DisconnectTree();
			int numColumns = 1; // ValueTypes will always be a single column tree
			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[numColumns+1];
			headers[0] = CreateRowNumberColumn();
			for (int i = 0; i < numColumns; ++i)
			{
				headers[i+1] = new VirtualTreeColumnHeader(mySelectedValueType.Name);
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new SamplePopulationValueTypeBranch(mySelectedValueType);
			ConnectTree();
		}

		private void PopulateControlForEntityType()
		{
			Debug.Assert(mySelectedEntityType != null);
			DisconnectTree();
			UniquenessConstraint preferredIdentifier = mySelectedEntityType.PreferredIdentifier;
			if (preferredIdentifier != null)
			{
				RoleMoveableCollection roleCollection = preferredIdentifier.RoleCollection;
				int numColumns = roleCollection.Count;
				VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[numColumns+1];
				headers[0] = CreateRowNumberColumn();
				for (int i = 0; i < numColumns; ++i)
				{
					headers[i+1] = new VirtualTreeColumnHeader(DeriveColumnName(roleCollection[i].Role));
				}
				vtrSamplePopulation.SetColumnHeaders(headers, true);
				myBranch = new SamplePopulationEntityTypeBranch(mySelectedEntityType, numColumns+1);
				ConnectTree();
			}
		}

		private void PopulateControlForFactType()
		{
			Debug.Assert(mySelectedFactType != null);
			DisconnectTree();
			RoleBaseMoveableCollection roleCollection = mySelectedFactType.RoleCollection;
			int numColumns = roleCollection.Count;
			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[numColumns+1];
			headers[0] = CreateRowNumberColumn();
			for (int i = 0; i < numColumns; ++i)
			{
				headers[i+1] = new VirtualTreeColumnHeader(DeriveColumnName(roleCollection[i].Role));
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new SamplePopulationFactTypeBranch(mySelectedFactType, numColumns+1);
			ConnectTree();
		}

		private VirtualTreeColumnHeader CreateRowNumberColumn()
		{
			return new VirtualTreeColumnHeader("", 35, true);
		}

		private void DisconnectTree()
		{
			this.vtrSamplePopulation.MultiColumnTree = null;
		}

		private void ConnectTree()
		{
			SamplePopulationVirtualTree spvt = new SamplePopulationVirtualTree(myBranch, (myBranch as IMultiColumnBranch).ColumnCount);
			this.vtrSamplePopulation.MultiColumnTree = spvt;
		}

		/// <summary>
		/// Nulls all member objects and refreshes visibility
		/// </summary>
		public void NullSelection()
		{
			this.mySelectedValueType = null;
			this.mySelectedEntityType = null;
			this.mySelectedFactType = null;
			DisconnectTree();
			AdjustVisibility(false);
		}

		private void AdjustVisibility(bool visibility)
		{
			this.vtrSamplePopulation.Visible = visibility;
			this.lblNoSelection.Visible = !visibility;
		}

		private string DeriveColumnName(Role selectedRole)
		{
			string name;
			ObjectType rolePlayer = selectedRole.RolePlayer;
			if (rolePlayer == null)
			{
				// Use the missing roleplayer localization from the Reading Editor
				name = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
			}
			else if ((name = selectedRole.Name).Length == 0)
			{
				name = rolePlayer.Name;
			}
			return name;
		}

		private void vtrSamplePopulation_SelectionChanged(object sender, EventArgs e)
		{
			VirtualTreeControl vtr = vtrSamplePopulation;
			ITree tree = vtr.Tree;
			bool multiColumnHighlight = false;
			if (tree != null)
			{
				int currentColumn = vtr.CurrentColumn;
				int currentRow = vtr.CurrentIndex;
				VirtualTreeItemInfo info = tree.GetItemInfo(currentRow, currentColumn, false);
				SamplePopulationBaseBranch baseBranch = info.Branch as SamplePopulationBaseBranch;
				if (null != baseBranch && baseBranch.IsFullRowSelectColumn(info.Column))
				{
					multiColumnHighlight = true;
				}
			}
			vtr.MultiColumnHighlight = multiColumnHighlight;
		}
		#endregion
		#region Model Events and Handler Methods
		#region Event Handler Attach/Detach Methods
		/// <summary>
		/// Attaches the event handlers to the store so that the tool window
		/// contents can be updated to reflect any model changes.
		/// </summary>
		public void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo;

			// Track FactTypeInstance changes
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(FactTypeHasRoleAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeHasRoleRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasFactTypeInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(FactTypeHasFactTypeInstanceAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeHasFactTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(FactTypeInstanceHasRoleInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(FactTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeInstanceHasRoleInstanceRemovedEvent));

			// Track EntityTypeInstance changes
			classInfo = dataDirectory.FindMetaRelationship(EntityTypeHasPreferredIdentifier.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(EntityTypeHasPreferredIdentifierAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(EntityTypeHasPreferredIdentifierRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(EntityTypeHasEntityTypeInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(EntityTypeHasEntityTypeInstanceAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(EntityTypeHasEntityTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(EntityTypeInstanceHasRoleInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(EntityTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(EntityTypeInstanceHasRoleInstanceRemovedEvent));

			// Track ValueTypeInstance changes
			classInfo = dataDirectory.FindMetaRelationship(ValueTypeHasValueTypeInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ValueTypeHasValueTypeInstanceAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ValueTypeHasValueTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(ValueTypeInstance.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ValueTypeInstanceValueChangedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasFactType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeRemovedEvent));

			// Track object type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasObjectType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ObjectTypeRemovedEvent));
		}

		/// <summary>
		/// Removes the event handlers from the store that were placed to allow
		/// the tool window to keep in sync with the mdoel
		/// </summary>
		public void DetachEventHandlers(Store store)
		{
			if (store == null || store.Disposed)
			{
				return;
			}
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo;

			// Track FactTypeInstance changes
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(FactTypeHasRoleAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeHasRoleRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasFactTypeInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(FactTypeHasFactTypeInstanceAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeHasFactTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(FactTypeInstanceHasRoleInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(FactTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeInstanceHasRoleInstanceRemovedEvent));

			// Track EntityTypeInstance changes
			classInfo = dataDirectory.FindMetaRelationship(EntityTypeHasPreferredIdentifier.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(EntityTypeHasPreferredIdentifierAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(EntityTypeHasPreferredIdentifierRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(EntityTypeHasEntityTypeInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(EntityTypeHasEntityTypeInstanceAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(EntityTypeHasEntityTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindMetaRelationship(EntityTypeInstanceHasRoleInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(EntityTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(EntityTypeInstanceHasRoleInstanceRemovedEvent));

			// Track ValueTypeInstance changes
			classInfo = dataDirectory.FindMetaRelationship(ValueTypeHasValueTypeInstance.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ValueTypeHasValueTypeInstanceAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ValueTypeHasValueTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(ValueTypeInstance.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ValueTypeInstanceValueChangedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasFactType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeRemovedEvent));

			// Track object type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasObjectType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ObjectTypeRemovedEvent));
		}
		#endregion
		#region Fact Type Instance Event Handlers
		private void FactTypeHasRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactTypeModelChangeHandler(link.FactType);
		}

		private void FactTypeHasRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactTypeModelChangeHandler(link.FactType);
		}

		private void FactTypeHasFactTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
			FactTypeModelChangeHandler(link.FactType);
		}

		private void FactTypeHasFactTypeInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
			FactTypeModelChangeHandler(link.FactType);
		}

		private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
			FactTypeModelChangeHandler(link.FactTypeInstance.FactType);
		}

		private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
			FactTypeModelChangeHandler(link.FactTypeInstance.FactType);
		}

		private void FactTypeModelChangeHandler(FactType factType)
		{
			FactType selectedFactType = mySelectedFactType;
			if (factType != null && selectedFactType != null && object.ReferenceEquals(selectedFactType, factType))
			{
				PopulateControlForFactType();
			}
		}
		#endregion
		#region Entity Type Instance Event Handlers
		private void EntityTypeHasPreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			ObjectType entityType = link.PreferredIdentifierFor;
			if(entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			{
				PopulateControlForEntityType();
			}
		}

		private void EntityTypeHasPreferredIdentifierRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			ObjectType entityType = link.PreferredIdentifierFor;
			if (entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			{
				PopulateControlForEntityType();
			}
		}

		private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
			ObjectType entityType = link.EntityType;
			if (entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			{
				SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
				Debug.Assert(entityBranch != null);
				entityBranch.AddEntityInstanceDisplay(link.EntityTypeInstanceCollection);
			}
		}

		private void EntityTypeHasEntityTypeInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
			EntityTypeModelChangeHandler(link.EntityType);
		}

		private void EntityTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			EntityTypeModelChangeHandler(link.EntityTypeInstance.EntityType);
		}

		private void EntityTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			EntityTypeModelChangeHandler(link.EntityTypeInstance.EntityType);
		}

		private void EntityTypeModelChangeHandler(ObjectType entityType)
		{
			Debug.Assert(!entityType.IsValueType);
			ObjectType selectedEntityType = mySelectedEntityType;
			if (entityType != null && selectedEntityType != null && object.ReferenceEquals(entityType, selectedEntityType))
			{
				PopulateControlForEntityType();
			}
		}
		#endregion
		#region Value Type Instance Event Handlers
		private void ValueTypeHasValueTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
			ObjectType valueType = link.ValueType;
			if (valueType != null && object.ReferenceEquals(valueType, mySelectedValueType))
			{
				SamplePopulationValueTypeBranch valueBranch = myBranch as SamplePopulationValueTypeBranch;
				Debug.Assert(valueBranch != null);
				valueBranch.AddValueInstanceDisplay(link.ValueTypeInstanceCollection);
			}
		}

		private void ValueTypeHasValueTypeInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
			ObjectType valueType = link.ValueType;
			if (valueType != null && object.ReferenceEquals(valueType, mySelectedValueType))
			{
				// UNDONE: Need to find the exact location of what is being removed, can't right now
				//PopulateControlForValueType();
				myBranch.RemoveInstanceDisplay();
			}
		}

		private void ValueTypeInstanceValueChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			ValueTypeInstance changedInstance = e.ModelElement as ValueTypeInstance;
			ObjectType valueType = changedInstance.ValueType;
			if (valueType != null && object.ReferenceEquals(valueType, mySelectedValueType))
			{
				SamplePopulationValueTypeBranch valueBranch = myBranch as SamplePopulationValueTypeBranch;
				Debug.Assert(valueBranch != null);
				valueBranch.EditValueInstanceDisplay(changedInstance);
			}
		}
		#endregion
		#region Misc Event Handlers
		private void FactTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			FactType factType = link.FactTypeCollection;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				NullSelection();
			}
		}

		private void ObjectTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
			ObjectType objectType = link.ObjectTypeCollection;
			if (objectType != null && 
				(object.ReferenceEquals(objectType, mySelectedEntityType) || object.ReferenceEquals(objectType, mySelectedValueType)))
			{
				NullSelection();
			}
		}
		#endregion
		#endregion
		#region Nested Branch Classes
		private abstract class SamplePopulationBaseBranch : IBranch, IMultiColumnBranch
		{
			/// <summary>
			/// An enum indicating special columns
			/// </summary>
			private enum SpecialColumnIndex
			{
				/// <summary>
				/// The special column showing the row number. Clicking in this
				/// column shows a full row select.
				/// </summary>
				FullRowSelectColumn = 0,
			}
			#region Member Variables
			private int myColumnCount;
			private Store myStore;
			private BranchModificationEventHandler myModificationEvents;
			#endregion // Member Variables
			#region Construction
			public SamplePopulationBaseBranch(int columnCount, Store store)
			{
				this.myColumnCount = columnCount;
				this.myStore = store;
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			protected VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				if (column == (int)SpecialColumnIndex.FullRowSelectColumn)
				{
					return VirtualTreeLabelEditData.Invalid;
				}
				return VirtualTreeLabelEditData.Default;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			protected LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}

			protected static BranchFeatures Features
			{
				get
				{
					return	BranchFeatures.ComplexColumns |
							BranchFeatures.DelayedLabelEdits |
							BranchFeatures.DefaultPositionTracking |
							BranchFeatures.ExplicitLabelEdits |
							BranchFeatures.Realigns |
							BranchFeatures.InsertsAndDeletes;
				}
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return Features;
				}
			}

			protected static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return GetAccessibilityData(row, column);
			}

			protected VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				if (column == (int)SpecialColumnIndex.FullRowSelectColumn)
				{
					retval.State = VirtualTreeDisplayStates.GrayText | VirtualTreeDisplayStates.TextAlignFar;
				}
				return retval;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return GetDisplayData(row, column, requiredData);
			}

			protected object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return null;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}

			protected string GetText(int row, int column)
			{
				if (column == (int)SpecialColumnIndex.FullRowSelectColumn)
				{
					// Returns row number
					return (row + 1).ToString();
				}
				else if (row == NewRowIndex)
				{
					return null;
				}
				else
				{
					return "";
				}
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}

			protected static string GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return GetTipText(row, column, tipType);
			}

			protected static bool IsExpandable(int row, int column)
			{
				return true;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}

			LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return new LocateObjectData();
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return LocateObject(obj, style, locateOptions);
			}

			protected event BranchModificationEventHandler OnBranchModification
			{
				add
				{
					myModificationEvents += value;
				}
				remove
				{
					myModificationEvents -= value;
				}
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add
				{
					OnBranchModification += value;
				}
				remove
				{
					OnBranchModification -= value;
				}
			}

			protected static void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
				OnDragEvent(sender, row, column, eventType, args);
			}

			protected static void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
				OnGiveFeedback(args, row, column);
			}

			protected static void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
				OnQueryContinueDrag(args, row, column);
			}

			protected static VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return OnStartDrag(sender, row, column, reason);
			}

			protected StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return ToggleState(row, column);
			}

			protected static StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
			}

			protected static int UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			int IBranch.UpdateCounter
			{
				get
				{
					return UpdateCounter;
				}
			}

			protected int VisibleItemCount
			{
				get
				{
					// Represents the new row
					return 1;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}

			/// <summary>
			/// Returns the row index of the "new" row
			/// </summary>
			protected int NewRowIndex
			{
				get
				{
					return (this as IBranch).VisibleItemCount - 1;
				}
			}
			#endregion
			#region IMultiColumnBranch Member Mirror/Implementation
			protected int ColumnCount
			{
				get
				{
					return this.myColumnCount;
				}
			}
			int IMultiColumnBranch.ColumnCount
			{
				get
				{
					return ColumnCount;
				}
			}

			protected static SubItemCellStyles ColumnStyles(int column)
			{
				return SubItemCellStyles.Simple;
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}

			protected int GetJaggedColumnCount(int row)
			{
				return ColumnCount;
			}
			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				return GetJaggedColumnCount(row);
			}
			#endregion
			#region Accessor Properties
			/// <summary>
			/// The store for the current active document
			/// </summary>
			protected Store Store
			{
				get
				{
					return myStore;
				}
			}
			#endregion // Accessor Properties
			#region Helper Methods
			/// <summary>
			/// Return true if a selection for the specified column in this
			/// branch should select the full row
			/// </summary>
			public virtual bool IsFullRowSelectColumn(int column)
			{
				return column == (int)SpecialColumnIndex.FullRowSelectColumn;
			}

			protected ValueTypeInstance RecurseIdentifyingValueTypeInstance(ObjectTypeInstance objectTypeInstance, ObjectType parentType, Role identifierRole)
			{
				ObjectTypeInstance result = RecurseAndCreateIdentifyingEntityTypeInstance(objectTypeInstance, parentType, identifierRole);
				ValueTypeInstance selectedValueTypeInstance = RecurseValueTypeInstance(result, identifierRole);
				return selectedValueTypeInstance;
			}

			private ValueTypeInstance RecurseValueTypeInstance(ObjectTypeInstance objectTypeInstance, Role identifierRole)
			{
				EntityTypeInstance eInstance;
				ValueTypeInstance vInstance;
				if (null != (vInstance = objectTypeInstance as ValueTypeInstance))
				{
					return vInstance;
				}
				else if(null != (eInstance = objectTypeInstance as EntityTypeInstance))
				{
					EntityTypeRoleInstanceMoveableCollection roleInstances = eInstance.RoleInstanceCollection;
					if (identifierRole == null)
					{
						return RecurseValueTypeInstance(roleInstances[0].ObjectTypeInstanceCollection, null);
					}
					else
					{
						foreach (EntityTypeRoleInstance roleInstance in roleInstances)
						{
							if (object.ReferenceEquals(roleInstance.RoleCollection, identifierRole))
							{
								return RecurseValueTypeInstance(roleInstance.ObjectTypeInstanceCollection, null);
							}
						}
					}
				}
				return null;
			}

			private ObjectTypeInstance RecurseAndCreateIdentifyingEntityTypeInstance(ObjectTypeInstance objectTypeInstance, ObjectType parentType, Role identifierRole)
			{
				if (parentType.IsValueType)
				{
					if (objectTypeInstance == null)
					{
						ValueTypeInstance editInstance = ValueTypeInstance.CreateValueTypeInstance(myStore);
						editInstance.ValueType = parentType;
						return editInstance;
					}
					return objectTypeInstance;
				}
				else
				{
					if (identifierRole == null)
					{
						identifierRole = parentType.PreferredIdentifier.RoleCollection[0];
					}
					EntityTypeInstance editInstance = objectTypeInstance as EntityTypeInstance;
					if (editInstance == null)
					{
						editInstance = EntityTypeInstance.CreateEntityTypeInstance(myStore);
						editInstance.EntityType = parentType;
					}
					EntityTypeRoleInstance editingRoleInstance = null;
					EntityTypeRoleInstanceMoveableCollection identifierInstances = editInstance.RoleInstanceCollection;
					foreach (EntityTypeRoleInstance roleInstance in editInstance.RoleInstanceCollection)
					{
						if(object.ReferenceEquals(roleInstance.RoleCollection, identifierRole))
						{
							editingRoleInstance = roleInstance;
							break;
						}
					}
					if(editingRoleInstance == null)
					{
						EntityTypeRoleInstance identifierInstance = EntityTypeRoleInstance.CreateEntityTypeRoleInstance(myStore,
							new RoleAssignment[] {
								new RoleAssignment(EntityTypeRoleInstance.RoleCollectionMetaRoleGuid, identifierRole),
								new RoleAssignment(EntityTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, RecurseAndCreateIdentifyingEntityTypeInstance(null, identifierRole.RolePlayer, null))
							});
						identifierInstance.EntityTypeInstance = editInstance;
						return editInstance;
					}
					else
					{
						return RecurseAndCreateIdentifyingEntityTypeInstance(editingRoleInstance.ObjectTypeInstanceCollection, identifierRole.RolePlayer, null);
					}
				}
			}

			protected string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, ObjectType parentType)
			{
				StringBuilder outputText = null;
				string retVal = (parentType == null) ? "" : RecurseObjectTypeInstanceValue(objectTypeInstance, parentType, null, ref outputText);
				return (outputText != null) ? outputText.ToString() : retVal;
			}

			private string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string listSeparator, ref StringBuilder outputText)
			{
				ValueTypeInstance valueInstance;
				EntityTypeInstance entityTypeInstance;
				if (parentType.IsValueType)
				{
					valueInstance = objectTypeInstance as ValueTypeInstance;
					string valueText = " ";
					if (valueInstance != null)
					{
						valueText = valueInstance.Value;
						if (outputText != null)
						{
							outputText.Append(valueText);
							return null;
						}
					}
					return valueText;
				}
				else
				{
					entityTypeInstance = objectTypeInstance as EntityTypeInstance;
					UniquenessConstraint identifier = parentType.PreferredIdentifier;
					RoleMoveableCollection identifierRoles = identifier.RoleCollection;
					int identifierCount = identifierRoles.Count;
					if (identifierCount == 1)
					{
						ObjectTypeInstance nestedInstance = null;
						if (entityTypeInstance != null)
						{
							EntityTypeRoleInstanceMoveableCollection roleInstances = entityTypeInstance.RoleInstanceCollection;
							if (roleInstances.Count > 0)
							{
								nestedInstance = roleInstances[0].ObjectTypeInstanceCollection;
							}
						}
						return RecurseObjectTypeInstanceValue(nestedInstance, identifierRoles[0].RolePlayer, listSeparator, ref outputText);
					}
					else
					{
						EntityTypeRoleInstanceMoveableCollection roleInstances = null;
						int roleInstanceCount = 0;
						if (entityTypeInstance != null)
						{
							roleInstances = entityTypeInstance.RoleInstanceCollection;
							roleInstanceCount = roleInstances.Count;
						}
						if (outputText == null)
						{
							outputText = new StringBuilder();
						}
						outputText.Append("(");
						if (listSeparator == null)
						{
							listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
						}
						for (int i = 0; i < identifierCount; ++i)
						{
							Role identifierRole = identifierRoles[i];
							if (i != 0)
							{
								outputText.Append(listSeparator);
							}
							if (roleInstanceCount != 0)
							{
								for (int j = 0; j < roleInstanceCount; ++j)
								{
									EntityTypeRoleInstance instance = roleInstances[j];
									if (object.ReferenceEquals(instance.RoleCollection, identifierRole))
									{
										RecurseObjectTypeInstanceValue(instance.ObjectTypeInstanceCollection, identifierRole.RolePlayer, listSeparator, ref outputText);
										break;
									}
									else if (j == roleInstanceCount - 1)
									{
										RecurseObjectTypeInstanceValue(null, identifierRole.RolePlayer, listSeparator, ref outputText);
									}
								}
							}
							else
							{
								if (i == 0)
								{
									outputText.Append(" ");
								}
								RecurseObjectTypeInstanceValue(null, identifierRole.RolePlayer, listSeparator, ref outputText);
							}
						}
						outputText.Append(")");
					}
					return null;
				}
			}
			#endregion
			#region Branch Update Methods
			public void AddInstanceDisplay(int location)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, location, 1));
				}
			}

			public void EditInstanceDisplay(int location)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, location, 1, 1)));
					// Note: If we start using DelayRedraw, then these need to go back on
					//myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
					//myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
				}
			}

			public void RemoveInstanceDisplay()
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.Realign(this));
				}
			}

			protected void AddAndInitializeValueTypeInstance(int row, int column, string newText, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ValueTypeInstance editInstance = ValueTypeInstance.CreateValueTypeInstance(Store);
				editInstance.ValueType = parentValueType;
				editInstance.Value = newText;
			}

			protected void EditValueTypeInstance(int row, int column, string newText, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ValueTypeInstance instance = parentValueType.ValueTypeInstanceCollection[row];
				Debug.Assert(instance != null);
				instance.Value = newText;
			}

			protected void RemoveValueTypeInstance(int row, int column, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ValueTypeInstance removeInstance = parentValueType.ValueTypeInstanceCollection[row];
				Debug.Assert(removeInstance != null);
				removeInstance.Remove();
			}

			protected void RemoveEntityTypeInstance(int row, int column, ObjectType parentEntityType)
			{
				Debug.Assert(!parentEntityType.IsValueType);
				EntityTypeInstance removeInstance = parentEntityType.EntityTypeInstanceCollection[row];
				Debug.Assert(removeInstance != null);
				removeInstance.Remove();
			}
			#endregion
		}
		private class SamplePopulationValueTypeBranch : SamplePopulationBaseBranch, IBranch
		{
			private ObjectType myValueType;
			#region Construction
			// Value Type Branches will always have 1 column, plus the full row select column
			public SamplePopulationValueTypeBranch(ObjectType selectedValueType) : base(2, selectedValueType.Store)
			{
				Debug.Assert(selectedValueType.IsValueType);
				myValueType = selectedValueType;
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			protected new VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retval = base.BeginLabelEdit(row, column, activationStyle);
				if(retval.IsValid)
				{
					if (!(row == NewRowIndex))
					{
						retval.AlternateText = myValueType.ValueTypeInstanceCollection[row].Value.ToString();
					}
				}
				return retval;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			protected new LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				bool isNewRow = (row == NewRowIndex);
				bool textIsEmpty = String.IsNullOrEmpty(newText);
				// If on the new row and nothing is entered, ignore it.
				if (isNewRow && textIsEmpty)
				{
					return LabelEditResult.CancelEdit;
				}
				// Is New Row && Text is Empty = Do Nothing
				// Is New Row && Text is not empty = Make a new one && set the value
				// Not New Row && Text is Empty = Delete the object
				// Not New Row && Text is not empty = set the value
				string columnName = TreeControl.GetColumnHeader(column).Text;
				if (isNewRow)
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, columnName)))
					{
						AddAndInitializeValueTypeInstance(row, column, newText, myValueType);
						t.Commit();
					}
				}
				else if (textIsEmpty)
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, columnName)))
					{
						RemoveValueTypeInstance(row, column, myValueType);
						t.Commit();
					}
				}
				else
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, columnName)))
					{
						EditValueTypeInstance(row, column, newText, myValueType);
						t.Commit();
					}
				}
				return LabelEditResult.AcceptEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}

			protected new object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (row == NewRowIndex)
				{
					return null;
				}
				return myValueType.ValueTypeInstanceCollection[row];
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}

			protected new string GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text != null && text.Length == 0)
				{
					text = myValueType.ValueTypeInstanceCollection[row].Value.ToString();
				}
				return text;
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}

			protected new int VisibleItemCount
			{
				get
				{
					return myValueType.ValueTypeInstanceCollection.Count + base.VisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion
			#region Branch Update Methods
			public void AddValueInstanceDisplay(ValueTypeInstance valueTypeInstance)
			{
				int location = myValueType.ValueTypeInstanceCollection.IndexOf(valueTypeInstance);
				if (location != -1)
				{
					base.AddInstanceDisplay(location);
				}
			}

			public void EditValueInstanceDisplay(ValueTypeInstance valueTypeInstance)
			{
				int location = myValueType.ValueTypeInstanceCollection.IndexOf(valueTypeInstance);
				if (location != -1)
				{
					base.EditInstanceDisplay(location);
				}
			}
			#endregion
		}
		private class SamplePopulationEntityTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			private ObjectType myEntityType;
			private RoleMoveableCollection myIdentifierRoles;
			private SamplePopulationEntityEditorBranch[,] myEditorBranches;
			#region Construction
			public SamplePopulationEntityTypeBranch(ObjectType selectedEntityType, int numColumns)
				: base(numColumns, selectedEntityType.Store)
			{
				Debug.Assert(!selectedEntityType.IsValueType);
				myEntityType = selectedEntityType;
				EntityTypeInstanceMoveableCollection instances = selectedEntityType.EntityTypeInstanceCollection;
				int numInstances = instances.Count;
				UniquenessConstraint identifier = selectedEntityType.PreferredIdentifier;
				Debug.Assert(identifier != null);
				RoleMoveableCollection identifierRoles = myIdentifierRoles = identifier.RoleCollection;
				int identifierRoleCount = numColumns - 1;
				myEditorBranches = new SamplePopulationEntityEditorBranch[numInstances+1, identifierRoleCount];
				if (identifierRoleCount > 1)
				{
					for (int row = 0; row < numInstances; ++row)
					{
						EntityTypeInstance entityInstance = instances[row];
						for (int col = 0; col < identifierRoleCount; ++col)
						{
							EntityTypeRoleInstanceMoveableCollection roleInstances = entityInstance.RoleInstanceCollection;
							int roleInstanceCount = roleInstances.Count;
							Role identifierRole = identifierRoles[col];
							EntityTypeRoleInstance entityRoleInstance = null;
							ObjectTypeInstance rolePlayerInstance = null;
							for (int i = 0; i < roleInstanceCount; ++i)
							{
								if (object.ReferenceEquals(roleInstances[i].RoleCollection, identifierRole))
								{
									entityRoleInstance = roleInstances[i];
									break;
								}
							}
							if(entityRoleInstance != null)
							{
								rolePlayerInstance = entityRoleInstance.ObjectTypeInstanceCollection;
							}
							ObjectType rolePlayer = identifierRole.RolePlayer;
							UniquenessConstraint roleInstanceIdentifier = rolePlayer.PreferredIdentifier;
							if (roleInstanceIdentifier != null && roleInstanceIdentifier.RoleCollection.Count > 1)
							{
								myEditorBranches[row, col] = new SamplePopulationEntityEditorBranch(rolePlayerInstance as EntityTypeInstance, entityInstance, selectedEntityType, identifierRole, rolePlayer);
								if (myEditorBranches[numInstances, col] == null)
								{
									myEditorBranches[numInstances, col] = new SamplePopulationEntityEditorBranch(null, null, selectedEntityType, identifierRole, rolePlayer);
								}
							}
						}
					}
				}
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			protected new VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				//UNDONE: Editing is currently unstable, so will be disabled for now
				return VirtualTreeLabelEditData.Invalid;
				//VirtualTreeLabelEditData retval = base.BeginLabelEdit(row, column, activationStyle);
				//if (retval.IsValid)
				//{
				//    if (myEditorBranches[row, column-1] != null)
				//    {
				//        retval = VirtualTreeLabelEditData.Invalid;
				//    }
				//}
				//return retval;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			protected new LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				if (row != NewRowIndex)
				{
					ObjectTypeInstance editInstance = myEntityType.EntityTypeInstanceCollection[row];
					EntityTypeInstance eEditInstance;
					ValueTypeInstance vEditInstance;
					bool delete = newText.Length == 0;
					if (editInstance == null && !delete)
					{
						//UNDONE: Localize the transaction name
						using (Transaction t = Store.TransactionManager.BeginTransaction("Create Recursive Instances"))
						{
							ValueTypeInstance instance = RecurseIdentifyingValueTypeInstance(editInstance, myEntityType, myEntityType.PreferredIdentifier.RoleCollection[column-1]);
							instance.Value = newText;
							t.Commit();
						}
						return LabelEditResult.AcceptEdit;
					}
					else if (null != (vEditInstance = editInstance as ValueTypeInstance))
					{
						ObjectType parentValueType = vEditInstance.ValueType;
						if (delete)
						{
							using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, parentValueType.Name)))
							{
								RemoveValueTypeInstance(row, column-1, parentValueType);
								t.Commit();
							}
						}
						else
						{
							using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, parentValueType.Name)))
							{
								EditValueTypeInstance(row, column-1, newText, parentValueType);
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					else if (null != (eEditInstance = editInstance as EntityTypeInstance))
					{
						if (delete)
						{
							ObjectType parentEntityType = eEditInstance.EntityType;
							using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, parentEntityType.Name)))
							{
								RemoveEntityTypeInstance(row, column-1, parentEntityType);
								t.Commit();
							}
						}
						else
						{
							//UNDONE: Localize the transaction name
							using (Transaction t = Store.TransactionManager.BeginTransaction("Create Recursive Instances"))
							{
								vEditInstance = RecurseIdentifyingValueTypeInstance(eEditInstance, eEditInstance.EntityType, null);
								vEditInstance.Value = newText;
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
				}
				return LabelEditResult.CancelEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}

			protected new object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.SubItemExpansion)
				{
					return myEditorBranches[row, column-1];
				}
				return null;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}

			protected new string GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null)
				{
					text = RecurseObjectTypeInstanceValue(null, myIdentifierRoles[column - 1].RolePlayer);
				}
				else if (text.Length == 0)
				{
					EntityTypeInstance selectedInstance = myEntityType.EntityTypeInstanceCollection[row];
					EntityTypeRoleInstanceMoveableCollection entityTypeRoleInstances = selectedInstance.RoleInstanceCollection;
					int roleInstanceCount = entityTypeRoleInstances.Count;
					EntityTypeRoleInstance roleInstance;
					Role identifierRole = myIdentifierRoles[column - 1];
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						if (object.ReferenceEquals(identifierRole, (roleInstance = entityTypeRoleInstances[i]).RoleCollection))
						{
							text = RecurseObjectTypeInstanceValue(roleInstance.ObjectTypeInstanceCollection, identifierRole.RolePlayer);
							return text;
						}
					}
					text = RecurseObjectTypeInstanceValue(null, identifierRole.RolePlayer);
				}
				return text;
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}

			protected new bool IsExpandable(int row, int column)
			{
				return myEditorBranches[row, column - 1] != null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}

			protected new int VisibleItemCount
			{
				get
				{
					return myEntityType.EntityTypeInstanceCollection.Count + base.VisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion
			#region IMultiColumnBranch Member Mirror/Implementation
			protected new static SubItemCellStyles ColumnStyles(int column)
			{
				switch (column)
				{
					case 0:
						return SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Expandable;
				}
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}
			#endregion
			#region Branch Update Methods
			public void AddEntityInstanceDisplay(EntityTypeInstance entityTypeInstance)
			{
				int location = myEntityType.EntityTypeInstanceCollection.IndexOf(entityTypeInstance);

				if (location != -1)
				{
					base.AddInstanceDisplay(location);
				}
			}

			public void EditEntityInstanceDisplay(EntityTypeInstance entityTypeInstance)
			{
				int location = myEntityType.EntityTypeInstanceCollection.IndexOf(entityTypeInstance);
				if (location != -1)
				{
					base.EditInstanceDisplay(location);
				}
			}
			#endregion
		}
		private class SamplePopulationFactTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			private FactType myFactType;
			private RoleBaseMoveableCollection myFactRoles;
			private SamplePopulationEntityEditorBranch[,] myEditorBranches;
			#region Construction
			public SamplePopulationFactTypeBranch(FactType selectedFactType, int numColumns)
				: base(numColumns, selectedFactType.Store)
			{
				myFactType = selectedFactType;
				FactTypeInstanceMoveableCollection instances = selectedFactType.FactTypeInstanceCollection;
				int numInstances = instances.Count;
				RoleBaseMoveableCollection factRoles = myFactRoles = selectedFactType.RoleCollection;
				int factRoleCount = numColumns - 1;
				myEditorBranches = new SamplePopulationEntityEditorBranch[numInstances + 1, factRoleCount];
				for (int row = 0; row < numInstances; ++row)
				{
					FactTypeInstance factInstance = instances[row];
					for (int col = 0; col < factRoleCount; ++col)
					{
						FactTypeRoleInstance factRoleInstance = factInstance.RoleInstanceCollection[col];
						ObjectTypeInstance rolePlayerInstance = factRoleInstance.ObjectTypeInstanceCollection;
						Role instanceRole = factRoleInstance.RoleCollection;
						ObjectType rolePlayer = instanceRole.RolePlayer;
						UniquenessConstraint roleInstanceIdentifier = rolePlayer.PreferredIdentifier;
						if (roleInstanceIdentifier != null && roleInstanceIdentifier.RoleCollection.Count > 1)
						{
							myEditorBranches[row, col] = null;
							//myEditorBranches[row, col] = new SamplePopulationEntityEditorBranch(rolePlayerInstance as EntityTypeInstance, factInstance, selectedFactType, instanceRole, rolePlayer);
							//if (myEditorBranches[numInstances, col] == null)
							//{
							//    myEditorBranches[numInstances, col] = new SamplePopulationEntityEditorBranch(null, null, selectedEntityType, instanceRole, rolePlayer);
							//}
						}
					}
				}
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			protected new VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			protected new object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				FactTypeInstanceMoveableCollection factTypeInstances = myFactType.FactTypeInstanceCollection;
				if (row < factTypeInstances.Count)
				{
					FactTypeRoleInstanceMoveableCollection factTypeRoleInstances = factTypeInstances[row].RoleInstanceCollection;
					if (column - 1 < factTypeRoleInstances.Count)
					{
						return factTypeRoleInstances[column - 1];
					}
				}
				return base.GetObject(row, column, style, ref options);
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}

			protected new string GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null)
				{
					text = RecurseObjectTypeInstanceValue(null, myFactType.RoleCollection[column - 1].Role.RolePlayer);
				}
				else if (text.Length == 0)
				{
					FactTypeInstance factTypeInstance = myFactType.FactTypeInstanceCollection[row];
					FactTypeRoleInstanceMoveableCollection factTypeRoleInstances = factTypeInstance.RoleInstanceCollection;
					int roleInstanceCount = factTypeRoleInstances.Count;
					FactTypeRoleInstance instance;
					Role factTypeRole = myFactType.RoleCollection[column - 1].Role;
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						if (object.ReferenceEquals(factTypeRole, (instance = factTypeRoleInstances[i]).RoleCollection))
						{
							return RecurseObjectTypeInstanceValue(instance.ObjectTypeInstanceCollection, factTypeRole.RolePlayer);
						}
					}
					text = RecurseObjectTypeInstanceValue(null, factTypeRole.RolePlayer);
				}
				return text;
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}

			protected new bool IsExpandable(int row, int column)
			{
				return myEditorBranches[row, column - 1] != null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}

			protected new int VisibleItemCount
			{
				get
				{
					return myFactType.FactTypeInstanceCollection.Count + base.VisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion
			#region IMultiColumnBranch Member Mirror/Implementation
			protected new static SubItemCellStyles ColumnStyles(int column)
			{
				switch (column)
				{
					case 0:
						return SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Expandable;
				}
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}
			#endregion
		}
		private class SamplePopulationEntityEditorBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private SamplePopulationEntityEditorBranch[] myEditBranches;
			private ObjectTypeInstance[] myObjectTypeInstances;
			private EntityTypeInstance myEditInstance, myParentInstance;
			private ObjectType myParentType, myInstanceType;
			private Role myEditRole;
			private int myVisibleItemCount;
			#endregion // Member Variables
			#region Construction
			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="editInstance">The EntityTypeInstance which will be edited</param>
			/// <param name="parentType">The Entity type which contains the given editInstance</param>
			/// <param name="instanceType">The Entity type of the given editInstance</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited</param>
			/// <param name="parentInstance">Instace of the parent Entity type which contains the given editInstance</param>
			public SamplePopulationEntityEditorBranch(EntityTypeInstance editInstance, EntityTypeInstance parentInstance, ObjectType parentType, Role editRole, ObjectType instanceType) : base(2, parentType.Store)
			{
				myParentType = parentType;
				myInstanceType = instanceType;
				myEditInstance = editInstance;
				myEditRole = editRole;
				myParentInstance = parentInstance;
				UniquenessConstraint primaryIdentifier = instanceType.PreferredIdentifier;
				Debug.Assert(primaryIdentifier != null); // Shouldn't be able to get into an edit branch if there isn't an identifier sequence
				RoleMoveableCollection primaryIdentifierRoles = primaryIdentifier.RoleCollection;
				int primaryIdentifierRoleCount = myVisibleItemCount = primaryIdentifierRoles.Count;
				myEditBranches = new SamplePopulationEntityEditorBranch[primaryIdentifierRoleCount];
				myObjectTypeInstances = new ObjectTypeInstance[primaryIdentifierRoleCount];
				// For each primary identifier role, set up a row
				for (int i = 0; i < primaryIdentifierRoleCount; ++i)
				{
					ObjectTypeInstance currentInstance = null;
					Role identifierRole = primaryIdentifierRoles[i];
					EntityTypeRoleInstanceMoveableCollection roleInstances = null;
					int roleInstanceCount = 0;
					if (editInstance != null)
					{
						roleInstances = editInstance.RoleInstanceCollection;
						roleInstanceCount = roleInstances.Count;
					}
					RoleMoveableCollection identifierRoles;
					for(int j = 0; j < roleInstanceCount; j++)
					{
						if (object.ReferenceEquals(identifierRole, roleInstances[j].RoleCollection))
						{
							myObjectTypeInstances[i] = currentInstance = roleInstances[j].ObjectTypeInstanceCollection;
							break;
						}
					}
					ObjectType rolePlayer = primaryIdentifierRoles[i].RolePlayer;
					if(!rolePlayer.IsValueType)
					{
						UniquenessConstraint identifier = rolePlayer.PreferredIdentifier;
						if (identifier != null && (identifierRoles = identifier.RoleCollection).Count > 1)
						{
							myEditBranches[i] = new SamplePopulationEntityEditorBranch(currentInstance as EntityTypeInstance, editInstance, instanceType, identifierRole, rolePlayer);
						}
					}
				}
			}
			#endregion
			#region IBranch Member Mirror/Implementations

			/// <summary>
			/// Make this an expandable branch
			/// </summary>
			protected new static BranchFeatures Features
			{
				get
				{
					return (SamplePopulationBaseBranch.Features & (~BranchFeatures.ComplexColumns)) | BranchFeatures.Expansions;
				}
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return Features;
				}
			}

			protected new VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				//UNDONE: Editing is currently unstable, so will be disabled for now
				return VirtualTreeLabelEditData.Invalid;
				//if (myEditBranches[row] == null)
				//{
				//    return VirtualTreeLabelEditData.Default;
				//}
				//return VirtualTreeLabelEditData.Invalid;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			protected new LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				ObjectTypeInstance editInstance = myObjectTypeInstances[row];
				EntityTypeInstance eEditInstance;
				ValueTypeInstance vEditInstance;
				bool delete = newText.Length == 0;
				if (editInstance == null && !delete)
				{
					//UNDONE: Localize the transaction name
					using (Transaction t = Store.TransactionManager.BeginTransaction("Create Recursive Instances"))
					{
						ValueTypeInstance instance = RecurseIdentifyingValueTypeInstance(myParentInstance, myParentType, myEditRole);
						instance.Value = newText;
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
				}
				else if (null != (vEditInstance = editInstance as ValueTypeInstance))
				{
					ObjectType parentValueType = vEditInstance.ValueType;
					if(delete)
					{
						using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, parentValueType.Name)))
						{
							RemoveValueTypeInstance(row, column, parentValueType);
							t.Commit();
						}
					}
					else
					{
						using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, parentValueType.Name)))
						{
							EditValueTypeInstance(row, column, newText, parentValueType);
							t.Commit();
						}
					}
					return LabelEditResult.AcceptEdit;
				}
				else if (null != (eEditInstance = editInstance as EntityTypeInstance))
				{
					if (delete)
					{
						ObjectType parentEntityType = eEditInstance.EntityType;
						using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, parentEntityType.Name)))
						{
							RemoveEntityTypeInstance(row, column, parentEntityType);
							t.Commit();
						}
					}
					else
					{
						//UNDONE: Localize the transaction name
						using (Transaction t = Store.TransactionManager.BeginTransaction("Create Recursive Instances"))
						{
							vEditInstance = RecurseIdentifyingValueTypeInstance(eEditInstance, eEditInstance.EntityType, null);
							vEditInstance.Value = newText;
							t.Commit();
						}
					}
					return LabelEditResult.AcceptEdit;
				}
				return LabelEditResult.CancelEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}

			protected new VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return VirtualTreeDisplayData.Empty;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return GetDisplayData(row, column, requiredData);
			}

			protected new object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return myEditBranches[row];
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}

			protected new string GetText(int row, int column)
			{
				ObjectTypeInstance selectedInstance = myObjectTypeInstances[row];
				ObjectType identifierType = myInstanceType.PreferredIdentifier.RoleCollection[row].RolePlayer;
				if(selectedInstance == null)
				{
					return RecurseObjectTypeInstanceValue(null, identifierType);
				}
				else
				{
					return RecurseObjectTypeInstanceValue(selectedInstance, identifierType);
				}
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}

			protected new bool IsExpandable(int row, int column)
			{
				return myEditBranches[row] != null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}

			protected new int VisibleItemCount
			{
				get
				{
					return myVisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion
			#region Helper Methods
			public override bool IsFullRowSelectColumn(int column)
			{
				return false;
			}
			#endregion
		}
		#endregion
		#region Nested Class SamplePopulationVirtualTree
		private class SamplePopulationVirtualTree : MultiColumnTree
		{
			public SamplePopulationVirtualTree(IBranch root, int columnCount)
				: base(columnCount)
			{
				Debug.Assert(root != null);
				this.Root = root;
			}
		}
		#endregion
	}
}

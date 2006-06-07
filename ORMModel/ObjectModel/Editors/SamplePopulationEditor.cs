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
			myBranch = new SamplePopulationValueTypeBranch(mySelectedValueType, numColumns+1);
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
			EntityTypeModelChangeHandler(link.PreferredIdentifierFor);
		}

		private void EntityTypeHasPreferredIdentifierRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			EntityTypeModelChangeHandler(link.PreferredIdentifierFor);
		}

		private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
			EntityTypeModelChangeHandler(link.EntityType);
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

			LabelEditResult CommitLabelEdit(int row, int column, string newText)
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
					using (Transaction t = myStore.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, columnName)))
					{
						AddAndInitializeInstance(row, column, newText);
						t.Commit();
					}					
				}
				else if (textIsEmpty)
				{
					using (Transaction t = myStore.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, columnName)))
					{
						RemoveInstance(row, column);
						t.Commit();
					}					
				}
				else
				{
					using (Transaction t = myStore.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, columnName)))
					{
						EditInstance(row, column, newText);
						t.Commit();
					}
				}
				return LabelEditResult.AcceptEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}

			static BranchFeatures Features
			{
				get
				{
					return	BranchFeatures.DelayedLabelEdits |
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

			static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
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
				return false;
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
			int ColumnCount
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

			static SubItemCellStyles ColumnStyles(int column)
			{
				return SubItemCellStyles.Simple;
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}

			int GetJaggedColumnCount(int row)
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
			protected string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance)
			{
				if (objectTypeInstance is ValueTypeInstance)
				{
					return (objectTypeInstance as ValueTypeInstance).Value.ToString();
				}
				else
				{
					Debug.Assert(objectTypeInstance is EntityTypeInstance);
					EntityTypeInstance entityTypeInstance = (objectTypeInstance as EntityTypeInstance);
					EntityTypeRoleInstanceMoveableCollection roleInstances = entityTypeInstance.RoleInstanceCollection;
					StringBuilder outputText = new StringBuilder();
					if (roleInstances.Count == 1)
					{
						outputText.Append(RecurseObjectTypeInstanceValue(roleInstances[0].ObjectTypeInstanceCollection));
					}
					else
					{
						outputText.Append("(");
						string listSeperator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
						foreach (EntityTypeRoleInstance entityTypeRoleInstance in roleInstances)
						{
							outputText.Append(RecurseObjectTypeInstanceValue(entityTypeRoleInstance.ObjectTypeInstanceCollection));
							outputText.Append(listSeperator + " ");
						}
						outputText.Remove(outputText.Length - 1, 1);
						outputText.Append(")");
					}
					return outputText.ToString();
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

			/// <summary>
			/// Provide a generic interface for creating and initializing an
			/// Instance, must be called from inside a transaction.
			/// </summary>
			/// <param name="row">Row to create the instance at</param>
			/// <param name="column">Column to create the instance at</param>
			/// <param name="newText">Default value of the instance</param>
			protected abstract void AddAndInitializeInstance(int row, int column, string newText);
			/// <summary>
			/// Provide a generic interface for editing an
			/// Instance, must be called from inside a transaction.
			/// </summary>
			/// <param name="row">Row to edit at</param>
			/// <param name="column">Column to edit at</param>
			/// <param name="newText">New value for the instance</param>
			protected abstract void EditInstance(int row, int column, string newText);
			/// <summary>
			/// Provide a generic interface for removing an
			/// Instance, must be called from inside a transaction.
			/// </summary>
			/// <param name="row">Row to remove at</param>
			/// <param name="column">Column to remove at</param>
			protected abstract void RemoveInstance(int row, int column);
			#endregion
		}
		private class SamplePopulationValueTypeBranch : SamplePopulationBaseBranch, IBranch
		{
			private ObjectType myValueType;
			#region Construction
			public SamplePopulationValueTypeBranch(ObjectType selectedValueType, int numColumns) : base(numColumns, selectedValueType.Store)
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
				if (text.Length == 0)
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
			protected override void AddAndInitializeInstance(int row, int column, string newText)
			{
				ValueTypeInstance editInstance = ValueTypeInstance.CreateValueTypeInstance(Store);
				editInstance.ValueType = myValueType;
				editInstance.Value = newText;
			}

			protected override void EditInstance(int row, int column, string newText)
			{
				Debug.Assert(row != NewRowIndex);
				ValueTypeInstance editInstance = myValueType.ValueTypeInstanceCollection[row];
				Debug.Assert(editInstance != null);
				editInstance.Value = newText;
			}

			protected override void RemoveInstance(int row, int column)
			{
				Debug.Assert(row != NewRowIndex);
				ValueTypeInstance removeInstance = myValueType.ValueTypeInstanceCollection[row];
				Debug.Assert(removeInstance != null);
				removeInstance.Remove();
			}

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
		private class SamplePopulationEntityTypeBranch : SamplePopulationBaseBranch, IBranch
		{
			private ObjectType myEntityType;
			#region Construction
			public SamplePopulationEntityTypeBranch(ObjectType selectedEntityType, int numColumns)
				: base(numColumns, selectedEntityType.Store)
			{
				Debug.Assert(!selectedEntityType.IsValueType);
				myEntityType = selectedEntityType;
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
				EntityTypeInstanceMoveableCollection entityTypeInstances = myEntityType.EntityTypeInstanceCollection;
				if (row < entityTypeInstances.Count)
				{
					EntityTypeRoleInstanceMoveableCollection entityTypeRoleInstances = entityTypeInstances[row].RoleInstanceCollection;
					if (column - 1 < entityTypeRoleInstances.Count)
					{
						return entityTypeRoleInstances[column - 1];
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
				if (text.Length == 0)
				{
					EntityTypeRoleInstanceMoveableCollection entityTypeRoleInstances = myEntityType.EntityTypeInstanceCollection[row].RoleInstanceCollection;
					if (entityTypeRoleInstances.Count >= column)
					{
						ObjectTypeInstance cellValue = entityTypeRoleInstances[column - 1].ObjectTypeInstanceCollection;
						text = RecurseObjectTypeInstanceValue(cellValue);
					}
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
			#region Branch Update Methods
			protected override void AddAndInitializeInstance(int row, int column, string newText)
			{
				// UNDONE: AddAndInitializeInstance
			}

			protected override void EditInstance(int row, int column, string newText)
			{
				// UNDONE: EditInstance
			}

			protected override void RemoveInstance(int row, int column)
			{
				// UNDONE: RemoveInstance
			}
			#endregion
		}
		private class SamplePopulationFactTypeBranch : SamplePopulationBaseBranch, IBranch
		{
			private FactType myFactType;
			#region Construction
			public SamplePopulationFactTypeBranch(FactType selectedFactType, int numColumns)
				: base(numColumns, selectedFactType.Store)
			{
				myFactType = selectedFactType;
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
				if (text.Length == 0)
				{
					FactTypeRoleInstanceMoveableCollection factTypeRoleInstances = myFactType.FactTypeInstanceCollection[row].RoleInstanceCollection;
					if (factTypeRoleInstances.Count >= column)
					{
						ObjectTypeInstance cellValue = factTypeRoleInstances[column - 1].ObjectTypeInstanceCollection;
						text = RecurseObjectTypeInstanceValue(cellValue);
					}
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
			#region Branch Update Methods
			protected override void AddAndInitializeInstance(int row, int column, string newText)
			{
				// UNDONE: AddAndInitializeInstance
			}

			protected override void EditInstance(int row, int column, string newText)
			{
				// UNDONE: EditInstance
			}

			protected override void RemoveInstance(int row, int column)
			{
				// UNDONE: RemoveInstance
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

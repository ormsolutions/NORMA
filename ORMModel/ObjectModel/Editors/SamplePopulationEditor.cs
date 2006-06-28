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
			FactTypeHasRole link = sender as FactTypeHasRole;
			FactType factType = link.FactType;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				PopulateControlForFactType();
			}
		}

		private void FactTypeHasRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeHasRole link = sender as FactTypeHasRole;
			FactType factType = link.FactType;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				PopulateControlForFactType();
			}
		}

		private void FactTypeHasFactTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
			FactType factType = link.FactType;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				SamplePopulationFactTypeBranch factBranch = myBranch as SamplePopulationFactTypeBranch;
				Debug.Assert(factBranch != null);
				factBranch.AddFactInstanceDisplay(link.FactTypeInstanceCollection);
			}
		}

		private void FactTypeHasFactTypeInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
			FactType factType = link.FactType;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				// UNDONE: Need to find the exact location of what is being removed, can't right now
				myBranch.RemoveInstanceDisplay();
			}
		}

		private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
			FactTypeInstance factTypeInstance = link.FactTypeInstance;
			FactType factType = factTypeInstance.FactType;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				SamplePopulationFactTypeBranch factBranch = myBranch as SamplePopulationFactTypeBranch;
				Debug.Assert(factBranch != null);
				factBranch.EditFactInstanceDisplay(factTypeInstance);
			}
		}

		private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
			FactTypeInstance factTypeInstance = link.FactTypeInstance;
			FactType factType = factTypeInstance.FactType;
			if (factType != null && object.ReferenceEquals(factType, mySelectedFactType))
			{
				SamplePopulationFactTypeBranch factBranch = myBranch as SamplePopulationFactTypeBranch;
				Debug.Assert(factBranch != null);
				factBranch.EditFactInstanceDisplay(factTypeInstance);
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
			//UNDONE: Need to handle these changes for the nested branches somehow, for now just reload
			if (mySelectedEntityType != null)
			{
				PopulateControlForEntityType();
			}
			else if (mySelectedFactType != null)
			{
				PopulateControlForFactType();
			}
			//EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
			//ObjectType entityType = link.EntityType;
			//if (entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			//{
			//    SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
			//    Debug.Assert(entityBranch != null);
			//    entityBranch.AddEntityInstanceDisplay(link.EntityTypeInstanceCollection);
			//}
		}

		private void EntityTypeHasEntityTypeInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			//UNDONE: Need to handle these changes for the nested branches somehow, for now just reload
			if (mySelectedEntityType != null)
			{
				PopulateControlForEntityType();
			}
			else if (mySelectedFactType != null)
			{
				PopulateControlForFactType();
			}
			//EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
			//ObjectType entityType = link.EntityType;
			//if (entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			//{
			//    // UNDONE: Need to find the exact location of what is being removed, can't right now
			//    myBranch.RemoveInstanceDisplay();
			//}
		}

		private void EntityTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			//UNDONE: Need to handle these changes for the nested branches somehow, for now just reload
			if (mySelectedEntityType != null)
			{
				PopulateControlForEntityType();
			}
			else if (mySelectedFactType != null)
			{
				PopulateControlForFactType();
			}
			//EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			//EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
			//ObjectType entityType = entityTypeInstance.EntityType;
			//if(entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			//{
			//    SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
			//    Debug.Assert(entityBranch != null);
			//    entityBranch.EditEntityInstanceDisplay(entityTypeInstance);
			//}
		}

		private void EntityTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			//UNDONE: Need to handle these changes for the nested branches somehow, for now just reload
			if (mySelectedEntityType != null)
			{
				PopulateControlForEntityType();
			}
			else if (mySelectedFactType != null)
			{
				PopulateControlForFactType();
			}
			//EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			//EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
			//ObjectType entityType = entityTypeInstance.EntityType;
			//if (entityType != null && object.ReferenceEquals(entityType, mySelectedEntityType))
			//{
			//    SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
			//    Debug.Assert(entityBranch != null);
			//    entityBranch.EditEntityInstanceDisplay(entityTypeInstance);
			//}
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
				myBranch.RemoveInstanceDisplay();
			}
		}

		private void ValueTypeInstanceValueChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			//UNDONE: Need to handle these changes for the nested branches somehow, for now just reload
			if (mySelectedEntityType != null)
			{
				PopulateControlForEntityType();
			}
			else if (mySelectedFactType != null)
			{
				PopulateControlForFactType();
			}
			else
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
				if (column == (int)SpecialColumnIndex.FullRowSelectColumn)
				{
					return false;
				}
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

			protected ObjectTypeInstance RecurseValueTypeInstance(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string newText, ref ValueTypeInstance rootInstance)
			{
				if (parentType.IsValueType)
				{
					DataType valueDataType = parentType.DataType;
					if(valueDataType.CanCompare)
					{
						ValueTypeInstanceMoveableCollection instances = parentType.ValueTypeInstanceCollection;
						int instanceCount = instances.Count;
						for (int i = 0; i < instanceCount; ++i)
						{
							int compare = valueDataType.Compare(instances[i].Value, newText);
							if (compare == 0)
							{
								rootInstance = instances[i];
								return rootInstance;
							}
						}
					}
					ValueTypeInstance editValueTypeInstance = objectTypeInstance as ValueTypeInstance;
					if (editValueTypeInstance == null)
					{
						editValueTypeInstance = ValueTypeInstance.CreateValueTypeInstance(myStore);
						editValueTypeInstance.ValueType = parentType;
					}
					rootInstance = editValueTypeInstance;
					return editValueTypeInstance;
				}
				else
				{
					RoleMoveableCollection identifierRoles = parentType.PreferredIdentifier.RoleCollection;
					Debug.Assert(identifierRoles.Count == 1);
					Role identifierRole = identifierRoles[0];
					EntityTypeInstance editEntityTypeInstance = objectTypeInstance as EntityTypeInstance;
					EntityTypeRoleInstance editingRoleInstance = null;
					if (editEntityTypeInstance == null)
					{
						editEntityTypeInstance = EntityTypeInstance.CreateEntityTypeInstance(myStore);
						editEntityTypeInstance.EntityType = parentType;
					}
					else
					{
						editingRoleInstance = editEntityTypeInstance.RoleInstanceCollection[0];
					}
					if (editingRoleInstance == null)
					{
						EntityTypeRoleInstance identifierInstance = EntityTypeRoleInstance.CreateEntityTypeRoleInstance(myStore,
							new RoleAssignment[] {
								new RoleAssignment(EntityTypeRoleInstance.RoleCollectionMetaRoleGuid, identifierRole),
								new RoleAssignment(EntityTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref rootInstance))
							});
						identifierInstance.EntityTypeInstance = editEntityTypeInstance;
						return editEntityTypeInstance;
					}
					else
					{
						return RecurseValueTypeInstance(editingRoleInstance.ObjectTypeInstanceCollection, identifierRole.RolePlayer, newText, ref rootInstance);
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
					}
					if (outputText != null)
					{
						outputText.Append(valueText);
						return null;
					}
					return valueText;
				}
				else
				{
					entityTypeInstance = objectTypeInstance as EntityTypeInstance;
					UniquenessConstraint identifier = parentType.PreferredIdentifier;
					if (identifier == null)
					{
						string valueText = " ";
						if (outputText != null)
						{
							outputText.Append(valueText);
							return null;
						}
						return valueText;
					}
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

			protected void AddAndInitializeValueTypeInstance(string newText, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ValueTypeInstance newInstance = ValueTypeInstance.CreateValueTypeInstance(Store);
				newInstance.ValueType = parentValueType;
				newInstance.Value = newText;
			}

			protected void EditValueTypeInstance(ValueTypeInstance editInstance, string newText)
			{
				Debug.Assert(editInstance != null);
				editInstance.Value = newText;
			}

			protected void RemoveValueTypeInstance(ValueTypeInstance removeInstance)
			{
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
						AddAndInitializeValueTypeInstance(newText, myValueType);
						t.Commit();
					}
				}
				else if (textIsEmpty)
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, columnName)))
					{
						RemoveValueTypeInstance(myValueType.ValueTypeInstanceCollection[row]);
						t.Commit();
					}
				}
				else
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, columnName)))
					{
						EditValueTypeInstance(myValueType.ValueTypeInstanceCollection[row], newText);
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
				VirtualTreeLabelEditData retval = base.BeginLabelEdit(row, column, activationStyle);
				if (retval.IsValid)
				{
					if (IsExpandable(row, column))
					{
						//UNDONE: Set up combo boxes for selection, code for pulling collection and committing selection is below
						// To pull a collection
						//Role identifierRole = myEntityType.PreferredIdentifier.RoleCollection[column - 1];
						//ObjectType rolePlayer = identifierRole.RolePlayer;
						//if (rolePlayer.IsValueType)
						//{
						//    // This is the collection to be selected from
						//    rolePlayer.ValueTypeInstanceCollection;
						//}
						//else
						//{
						//    // This is the collection to be selected from
						//    rolePlayer.EntityTypeInstanceCollection;
						//}
						//// For each of the ObjectTypeInstances, pass them in to 
						//// RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, Role identifierRole) to get the text output for each

						// Might be easier to deal with a single ObjectTypeInstanceMoveableCollection, rather than either of the stronger typed versions,
						// but not sure if there are any built in conversions, EntityTypeInstance and ValueTypeInstance are subtypes of ObjectTypeInstance,
						// so it should be easy to get to though.

						// To commit a selection
						// int selection = current selection
						// collection = variable containing the entire collection, be it any of the three types
						//ObjectTypeInstance selectedInstance = collection[selection];
						//ObjectType entityType = myEntityType;
						//EntityTypeInstanceMoveableCollection currentInstanceCollection = entityType.EntityTypeInstanceCollection;
						//ObjectTypeInstance selectedParentInstance = currentInstanceCollection.Count < row ? currentInstanceCollection[row] : null;
						//Role identifierRole = entityType.PreferredIdentifier.RoleCollection[column - 1];
						//ConnectInstance(selectedParentInstance, selectedInstance, identifierRole);
						retval = VirtualTreeLabelEditData.Invalid;
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
				bool delete = newText.Length == 0;
				Store store = Store;
				ObjectType selectedEntityType = myEntityType;
				// If editing an existing EntityTypeInstance
				if (row != NewRowIndex)
				{
					EntityTypeInstance editInstance = selectedEntityType.EntityTypeInstanceCollection[row];
					Role factRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					EntityTypeRoleInstance editRoleInstance = editInstance.FindRoleInstance(factRole);
					// If editing an existing EntityTypeRoleInstance
					if (editRoleInstance != null)
					{
						ValueTypeInstance instance = null;
						// RoleInstance exists, therefore the link down to a ValueTypeInstance should exist.  If the chain
						// is broken for some reason, this will throw because it will try to self repair and it isn't inside a transaction.
						ObjectTypeInstance result = RecurseValueTypeInstance(editRoleInstance.ObjectTypeInstanceCollection, editRoleInstance.RoleCollection.RolePlayer, newText, ref instance);
						if (delete)
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, instance.ValueType.Name)))
							{
								RemoveValueTypeInstance(instance);
								t.Commit();
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, instance.ValueType.Name)))
							{
								EditValueTypeInstance(instance, newText);
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					// If editing an existing EntityTypeInstance but creating a new EntityTypeRoleInstance
					else if (!delete)
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedEntityType.Name)))
						{
							ValueTypeInstance instance = null;
							ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance);
							EditValueTypeInstance(instance, newText);
							ConnectInstance(editInstance, result, factRole);
							t.Commit();
						}
						return LabelEditResult.AcceptEdit;
					}
				}
				// New Row Editing
				else if (!delete)
				{
					using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, myEntityType.Name)))
					{
						Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref instance);
						EditValueTypeInstance(instance, newText);
						ConnectInstance(null, result, identifierRole);
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
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
					ObjectType selectedEntityType = myEntityType;
					EntityTypeInstanceMoveableCollection instances = selectedEntityType.EntityTypeInstanceCollection;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					EntityTypeInstance parentInstance = (row < instances.Count) ? instances[row] : null;
					EntityTypeInstance editInstance = null;
					if(parentInstance != null)
					{
						EntityTypeRoleInstanceMoveableCollection roleInstances = parentInstance.RoleInstanceCollection;
						int roleInstanceCount = roleInstances.Count;
						EntityTypeRoleInstance roleInstance;
						for(int i = 0; i < roleInstanceCount; ++i)
						{
							if(object.ReferenceEquals((roleInstance = roleInstances[i]).RoleCollection, identifierRole))
							{
								editInstance = roleInstance.ObjectTypeInstanceCollection as EntityTypeInstance;
								break;
							}
						}
					}
					return new SamplePopulationEntityEditorBranch(parentInstance, selectedEntityType, editInstance, identifierRole, this);
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
					text = RecurseObjectTypeInstanceValue(null, myEntityType.PreferredIdentifier.RoleCollection[column - 1].RolePlayer);
				}
				else if (text.Length == 0)
				{
					ObjectType selectedEntityType = myEntityType;
					EntityTypeInstance selectedInstance = selectedEntityType.EntityTypeInstanceCollection[row];
					EntityTypeRoleInstanceMoveableCollection entityTypeRoleInstances = selectedInstance.RoleInstanceCollection;
					int roleInstanceCount = entityTypeRoleInstances.Count;
					EntityTypeRoleInstance roleInstance;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
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
				if (!base.IsFullRowSelectColumn(column))
				{
					Role identifierRole = myEntityType.PreferredIdentifier.RoleCollection[column - 1];
					ObjectType rolePlayer = identifierRole.RolePlayer;
					if (rolePlayer != null)
					{
						UniquenessConstraint roleIdentifier = rolePlayer.PreferredIdentifier;
						return (roleIdentifier != null) && (roleIdentifier.RoleCollection.Count > 1);
					}
				}
				return false;
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
			#region Helper Methods
			/// <summary>
			/// Connect a given instance to the branch's current objectType, for the given role
			/// </summary>
			/// <param name="parentInstance">Instance to connect to</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect to</param>
			public void ConnectInstance(EntityTypeInstance parentInstance, ObjectTypeInstance connectInstance, Role identifierRole)
			{
				Debug.Assert(connectInstance != null);
				Store store = Store;
				ObjectType selectedEntityType = myEntityType;

				if (parentInstance == null)
				{
					parentInstance = EntityTypeInstance.CreateEntityTypeInstance(store);
					parentInstance.EntityType = selectedEntityType;
				}
				EntityTypeRoleInstance roleInstance = EntityTypeRoleInstance.CreateEntityTypeRoleInstance(store,
					new RoleAssignment[] {
						new RoleAssignment(EntityTypeRoleInstance.RoleCollectionMetaRoleGuid, identifierRole),
						new RoleAssignment(EntityTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, connectInstance)
					});
				roleInstance.EntityTypeInstance = parentInstance;
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
				VirtualTreeLabelEditData retval = base.BeginLabelEdit(row, column, activationStyle);
				if (retval.IsValid)
				{
					if (IsExpandable(row, column))
					{
						////UNDONE: Set up combo boxes for selection, code for pulling collection and committing selection is below
						//// To pull a collection
						//Role identifierRole = myFactType.RoleCollection[column - 1].Role;
						//ObjectType rolePlayer = identifierRole.RolePlayer;
						//if (rolePlayer.IsValueType)
						//{
						//    // This is the collection to be selected from
						//    rolePlayer.ValueTypeInstanceCollection;
						//}
						//else
						//{
						//    // This is the collection to be selected from
						//    rolePlayer.EntityTypeInstanceCollection;
						//}
						//// For each of the ObjectTypeInstances, pass them in to 
						//// RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, Role identifierRole) to get the text output for each

						// Might be easier to deal with a single ObjectTypeInstanceMoveableCollection, rather than either of the stronger typed versions,
						// but not sure if there are any built in conversions, EntityTypeInstance and ValueTypeInstance are subtypes of ObjectTypeInstance,
						// so it should be easy to get to though.

						// To commit a selection
						// int selection = current selection
						// collection = variable containing the entire collection, be it any of the three types
						//ObjectTypeInstance selectedInstance = collection[selection];
						//FactType factType = myFactType;
						//FactTypeInstanceMoveableCollection currentInstanceCollection = factType.FactTypeInstanceCollection;
						//FactTypeInstance selectedParentInstance = currentInstanceCollection.Count < row ? currentInstanceCollection[row] : null;
						//Role factRole = factType.RoleCollection[column - 1].Role;
						//ConnectInstance(selectedParentInstance, selectedInstance, factRole);
						retval = VirtualTreeLabelEditData.Invalid;
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
				bool delete = newText.Length == 0;
				Store store = Store;
				// If editing an existing FactTypeInstance
				if (row != NewRowIndex)
				{
					FactType selectedFactType = myFactType;
					FactTypeInstance editInstance = selectedFactType.FactTypeInstanceCollection[row];
					FactTypeRoleInstance editRoleInstance = null;
					FactTypeRoleInstanceMoveableCollection roleInstances = editInstance.RoleInstanceCollection;
					int instanceCount = roleInstances.Count;
					Role factRole = selectedFactType.RoleCollection[column - 1].Role;
					for (int i = 0; i < instanceCount; ++i)
					{
						if (object.ReferenceEquals(factRole, roleInstances[i].RoleCollection))
						{
							editRoleInstance = roleInstances[i];
							break;
						}
					}
					// If editing an existing FactTypeRoleInstance
					if (editRoleInstance != null)
					{
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(editRoleInstance.ObjectTypeInstanceCollection, editRoleInstance.RoleCollection.RolePlayer, newText, ref instance);
						if (delete)
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, instance.ValueType.Name)))
							{
								RemoveValueTypeInstance(instance);
								t.Commit();
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, instance.ValueType.Name)))
							{
								EditValueTypeInstance(instance, newText);
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					// If editing an existing FactTypeInstance but creating a new FactTypeRoleInstance
					else if (!delete)
					{
						//UNDONE: Localize the transaction name
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedFactType.Name)))
						{
							ValueTypeInstance instance = null;
							ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance);
							EditValueTypeInstance(instance, newText);
							FactTypeRoleInstance roleInstance = FactTypeRoleInstance.CreateFactTypeRoleInstance(store,
							new RoleAssignment[] {
								new RoleAssignment(FactTypeRoleInstance.RoleCollectionMetaRoleGuid, factRole),
								new RoleAssignment(FactTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, result)
							});
							roleInstance.FactTypeInstance = editInstance;
							t.Commit();
						}
						return LabelEditResult.AcceptEdit;
					}
				}
				// New Row Editing
				else if (!delete)
				{
					using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, myFactType.Name)))
					{
						FactType selectedFactType = myFactType;
						FactTypeInstance newInstance = FactTypeInstance.CreateFactTypeInstance(Store);
						newInstance.FactType = selectedFactType;
						Role factRole = selectedFactType.RoleCollection[column - 1].Role;
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance);
						EditValueTypeInstance(instance, newText);
						FactTypeRoleInstance roleInstance = FactTypeRoleInstance.CreateFactTypeRoleInstance(store,
							new RoleAssignment[] {
								new RoleAssignment(FactTypeRoleInstance.RoleCollectionMetaRoleGuid, factRole),
								new RoleAssignment(FactTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, result)
						    });
						roleInstance.FactTypeInstance = newInstance;
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
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
					FactType selectedFactType = myFactType;
					FactTypeInstanceMoveableCollection instances = selectedFactType.FactTypeInstanceCollection;
					Role selectedRole = selectedFactType.RoleCollection[column - 1].Role;
					FactTypeInstance parentInstance = (row < instances.Count) ? instances[row] : null;
					EntityTypeInstance editInstance = null;
					if (parentInstance != null)
					{
						FactTypeRoleInstanceMoveableCollection roleInstances = parentInstance.RoleInstanceCollection;
						int roleInstanceCount = roleInstances.Count;
						FactTypeRoleInstance roleInstance;
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							if (object.ReferenceEquals((roleInstance = roleInstances[i]).RoleCollection, selectedRole))
							{
								editInstance = roleInstance.ObjectTypeInstanceCollection as EntityTypeInstance;
								break;
							}
						}
					}
					return new SamplePopulationEntityEditorBranch(parentInstance, selectedFactType, editInstance, selectedRole, this);
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
				if (!base.IsFullRowSelectColumn(column))
				{
					Role factRole = myFactType.RoleCollection[column - 1].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					if (rolePlayer != null)
					{
						UniquenessConstraint roleIdentifier = rolePlayer.PreferredIdentifier;
						return (roleIdentifier != null) && (roleIdentifier.RoleCollection.Count > 1);
					}
				}
				return false;
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
			#region Helper Methods
			/// <summary>
			/// Connect a given instance to the branch's current objectType, for the given role
			/// </summary>
			/// <param name="parentInstance">Instance to connect to</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect to</param>
			public void ConnectInstance(FactTypeInstance parentInstance, ObjectTypeInstance connectInstance, Role identifierRole)
			{
				Debug.Assert(connectInstance != null);
				Debug.Assert(myFactType.RoleCollection.Contains(identifierRole));
				Store store = Store;
				FactType selectedFactType = myFactType;
				if (parentInstance == null)
				{
					parentInstance = FactTypeInstance.CreateFactTypeInstance(store);
					parentInstance.FactType = selectedFactType;
				}
				FactTypeRoleInstance roleInstance = FactTypeRoleInstance.CreateFactTypeRoleInstance(store,
					new RoleAssignment[] {
				        new RoleAssignment(FactTypeRoleInstance.RoleCollectionMetaRoleGuid, identifierRole),
				        new RoleAssignment(FactTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, connectInstance)
				    });
				roleInstance.FactTypeInstance = parentInstance;
			}
			#endregion
			#region Branch Update Methods
			public void AddFactInstanceDisplay(FactTypeInstance factTypeInstance)
			{
				int location = myFactType.FactTypeInstanceCollection.IndexOf(factTypeInstance);
				if (location != -1)
				{
					base.AddInstanceDisplay(location);
				}
			}

			public void EditFactInstanceDisplay(FactTypeInstance factTypeInstance)
			{
				int location = myFactType.FactTypeInstanceCollection.IndexOf(factTypeInstance);
				if (location != -1)
				{
					base.EditInstanceDisplay(location);
				}
			}
			#endregion
		}
		private class SamplePopulationEntityEditorBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private SamplePopulationBaseBranch myParentBranch;
			private EntityTypeInstance myEditInstance, myParentEntityInstance;
			private ObjectType myParentEntityType;
			private FactType myParentFactType;
			private FactTypeInstance myParentFactInstance;
			private Role myEditRole;
			#endregion // Member Variables
			#region Construction
			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentEntityInstance">Instance of the parent Entity type which contains the given editInstance</param>
			/// <param name="parentEntityType">The Entity type which contains the given editInstance</param>
			/// <param name="editInstance">The EntityTypeInstance which will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public SamplePopulationEntityEditorBranch(EntityTypeInstance parentEntityInstance, ObjectType parentEntityType, EntityTypeInstance editInstance, Role editRole, SamplePopulationBaseBranch parentBranch)
				: this(editInstance, editRole, parentBranch)
			{
				myParentEntityType = parentEntityType;
				myParentEntityInstance = parentEntityInstance;
			}

			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentFactTypeInstance">Instance of the parent Fact type which contains the given editInstance</param>
			/// <param name="parentFactType">The Fact type which contains the given editInstance</param>
			/// <param name="editInstance">The EntityTypeInstance which will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public SamplePopulationEntityEditorBranch(FactTypeInstance parentFactTypeInstance, FactType parentFactType, EntityTypeInstance editInstance, Role editRole, SamplePopulationBaseBranch parentBranch)
				: this(editInstance, editRole, parentBranch)
			{
				myParentFactType = parentFactType;
				myParentFactInstance = parentFactTypeInstance;
			}

			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="editInstance">The EntityTypeInstance which will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public SamplePopulationEntityEditorBranch(EntityTypeInstance editInstance, Role editRole, SamplePopulationBaseBranch parentBranch) : base(2, editRole.Store)
			{
				myEditInstance = editInstance;
				myEditRole = editRole;
				myParentBranch = parentBranch;
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
				if (IsExpandable(row, column))
				{
					////UNDONE: Set up combo boxes for selection, code for pulling collection and committing selection is below
					//// To pull a collection
					//Role selectedRole = myEditRole.RolePlayer.PreferredIdentifier.RoleCollection[row];
					//ObjectType rolePlayer = selectedRole.RolePlayer;
					//if (rolePlayer.IsValueType)
					//{
					//    // This is the collection to be selected from
					//    rolePlayer.ValueTypeInstanceCollection;
					//}
					//else
					//{
					//    // This is the collection to be selected from
					//    rolePlayer.EntityTypeInstanceCollection;
					//}
					//// For each of the ObjectTypeInstances, pass them in to 
					//// RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, Role identifierRole) to get the text output for each

					// Might be easier to deal with a single ObjectTypeInstanceMoveableCollection, rather than either of the stronger typed versions,
					// but not sure if there are any built in conversions, EntityTypeInstance and ValueTypeInstance are subtypes of ObjectTypeInstance,
					// so it should be easy to get to though.

					// To commit a selection
					// int selection = current selection
					// collection = variable containing the entire collection, be it any of the three types
					//ObjectTypeInstance selectedInstance = collection[selection];
					//Role selectedRole = myEditRole.RolePlayer.PreferredIdentifier.RoleCollection[row];
					//ConnectInstance(selectedInstance, selectedRole);
					return VirtualTreeLabelEditData.Invalid;
				}
				return VirtualTreeLabelEditData.Default;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			protected new LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				ObjectType instanceType = myEditRole.RolePlayer;
				Role identifierRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType editType = identifierRole.RolePlayer;
				EntityTypeInstance editInstance = myEditInstance;
				ObjectTypeInstance roleEditInstance = null;
				if(editInstance != null)
				{
					EntityTypeRoleInstance foundRoleInstance = editInstance.FindRoleInstance(identifierRole);
					if (foundRoleInstance != null)
					{
						roleEditInstance = foundRoleInstance.ObjectTypeInstanceCollection;
					}
				}
				bool delete = newText.Length == 0;
				Store store = Store;
				ValueTypeInstance vEditInstance;
				EntityTypeInstance eEditInstance;
				if (roleEditInstance == null)
				{
					if (delete)
					{
						return LabelEditResult.CancelEdit;
					}
					using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, identifierRole.RolePlayer.Name)))
					{
						ValueTypeInstance instance = null;
						ObjectType rolePlayer = identifierRole.RolePlayer;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, editType, newText, ref instance);
						EditValueTypeInstance(instance, newText);
						ConnectInstance(result, identifierRole);
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
				}
				else if (null != (vEditInstance = roleEditInstance as ValueTypeInstance))
				{
					ObjectType parentValueType = vEditInstance.ValueType;
					if(delete)
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, parentValueType.Name)))
						{
							RemoveValueTypeInstance(vEditInstance);
							t.Commit();
						}
					}
					else
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, parentValueType.Name)))
						{
							EditValueTypeInstance(vEditInstance, newText);
							t.Commit();
						}
					}
					return LabelEditResult.AcceptEdit;
				}
				else if (null != (eEditInstance = roleEditInstance as EntityTypeInstance))
				{
					ObjectType parentEntityType = eEditInstance.EntityType;
					ValueTypeInstance instance = null;
					ObjectTypeInstance result = RecurseValueTypeInstance(eEditInstance, parentEntityType, newText, ref instance);
					if (delete)
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, parentEntityType.Name)))
						{
							RemoveValueTypeInstance(instance);
							t.Commit();
						}
					}
					else
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, parentEntityType.Name)))
						{
							EditValueTypeInstance(vEditInstance, newText);
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
				if (style == ObjectStyle.ExpandedBranch)
				{
					ObjectType selectedEntityType = myEditRole.RolePlayer;
					EntityTypeInstanceMoveableCollection instances = selectedEntityType.EntityTypeInstanceCollection;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[row];
					EntityTypeInstance parentInstance = myEditInstance;
					EntityTypeInstance editInstance = null;
					if (parentInstance != null)
					{
						EntityTypeRoleInstance foundRoleInstance = parentInstance.FindRoleInstance(identifierRole);
						if (foundRoleInstance != null)
						{
							editInstance = foundRoleInstance.ObjectTypeInstanceCollection as EntityTypeInstance;
							Debug.Assert(editInstance != null);
						}
					}
					return new SamplePopulationEntityEditorBranch(parentInstance, selectedEntityType, editInstance, identifierRole, this);
				}
				return null;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}

			protected new string GetText(int row, int column)
			{
				EntityTypeInstance editInstance = myEditInstance;
				ObjectType instanceType = myEditRole.RolePlayer;
				Role instanceRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType identifierType = instanceRole.RolePlayer;
				if (editInstance != null)
				{
					EntityTypeRoleInstance selectedRoleInstance = editInstance.FindRoleInstance(instanceRole);
					if (selectedRoleInstance != null)
					{
						return RecurseObjectTypeInstanceValue(selectedRoleInstance.ObjectTypeInstanceCollection, identifierType);
					}
				}
				return RecurseObjectTypeInstanceValue(null, identifierType);
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}

			protected new bool IsExpandable(int row, int column)
			{
				ObjectType instanceType = myEditRole.RolePlayer;
				Role identifierRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType rolePlayer = identifierRole.RolePlayer;
				UniquenessConstraint roleIdentifier = rolePlayer.PreferredIdentifier;
				return (roleIdentifier != null) && (roleIdentifier.RoleCollection.Count > 1);
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}

			protected new int VisibleItemCount
			{
				get
				{
					return myEditRole.RolePlayer.PreferredIdentifier.RoleCollection.Count;
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

			/// <summary>
			/// Hook up the given instance from a child branch to its parent instance on the current branch.
			/// If the parent instance doesn't exist, it will be created.  The method then  calls back up
			/// the chain to the current branch's parent branch, passing the newly
			/// created instance to be connected and the role to connect on.
			/// </summary>
			/// <param name="instance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect on</param>
			public void ConnectInstance(ObjectTypeInstance instance, Role identifierRole)
			{
				Debug.Assert(instance != null);
				Debug.Assert(myEditRole.RolePlayer.PreferredIdentifier.RoleCollection.Contains(identifierRole));
				Store store = Store;

				EntityTypeInstance editInstance = myEditInstance;
				EntityTypeRoleInstance connection = EntityTypeRoleInstance.CreateEntityTypeRoleInstance(store,
					new RoleAssignment[] {
					new RoleAssignment(EntityTypeRoleInstance.RoleCollectionMetaRoleGuid, identifierRole),
					new RoleAssignment(EntityTypeRoleInstance.ObjectTypeInstanceCollectionMetaRoleGuid, instance)});
				if (editInstance == null)
				{
					editInstance = EntityTypeInstance.CreateEntityTypeInstance(store);
					editInstance.EntityType = myEditRole.RolePlayer;
					connection.EntityTypeInstance = editInstance;
					SamplePopulationEntityTypeBranch entityBranch;
					SamplePopulationFactTypeBranch factBranch;
					SamplePopulationEntityEditorBranch editBranch;
					if (null != (entityBranch = myParentBranch as SamplePopulationEntityTypeBranch))
					{
						entityBranch.ConnectInstance(myParentEntityInstance, editInstance, myEditRole);
					}
					else if (null != (factBranch = myParentBranch as SamplePopulationFactTypeBranch))
					{
						factBranch.ConnectInstance(myParentFactInstance, editInstance, myEditRole);
					}
					else if (null != (editBranch = myParentBranch as SamplePopulationEntityEditorBranch))
					{
						editBranch.ConnectInstance(editInstance, myEditRole);
					}
					else
					{
						Debug.Fail("Branch is of an unknown type");
					}
				}
				else
				{
					connection.EntityTypeInstance = editInstance;
				}
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

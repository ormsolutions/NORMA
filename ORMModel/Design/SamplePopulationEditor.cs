using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Design
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
		private bool myInEvents;
		private bool myRepopulated;
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
				if (value != mySelectedValueType)
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
				if (value != mySelectedEntityType)
				{
					this.mySelectedEntityType = value;
					if (value != null)
					{
						// PopulateControlForEntityType takes care of visibility
						PopulateControlForEntityType();
						mySelectedValueType = null;
						mySelectedFactType = null;
					}
					else
					{
						AdjustVisibility(false);
					}
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
				if (value != mySelectedFactType)
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
						return vtrSamplePopulation == sc.ActiveControl;
					}
				}
				return false;
			}
		}
		#endregion
		#region PopulateControl and Helpers
		private void PopulateControlForValueType()
		{
			if (myInEvents && myRepopulated)
			{
				return;
			}
			myRepopulated = true;
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
			if (myInEvents && myRepopulated)
			{
				return;
			}
			myRepopulated = true;
			Debug.Assert(mySelectedEntityType != null);
			DisconnectTree();
			UniquenessConstraint preferredIdentifier = mySelectedEntityType.PreferredIdentifier;
			if (preferredIdentifier != null)
			{
				AdjustVisibility(true);
				LinkedElementCollection<Role> roleCollection = preferredIdentifier.RoleCollection;
				int numColumns = roleCollection.Count;
				VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[numColumns + 1];
				headers[0] = CreateRowNumberColumn();
				for (int i = 0; i < numColumns; ++i)
				{
					headers[i + 1] = new VirtualTreeColumnHeader(DeriveColumnName(roleCollection[i].Role));
				}
				vtrSamplePopulation.SetColumnHeaders(headers, true);
				myBranch = new SamplePopulationEntityTypeBranch(mySelectedEntityType, numColumns + 1);
				ConnectTree();
			}
			else
			{
				AdjustVisibility(false);
			}
		}

		private void PopulateControlForFactType()
		{
			if (myInEvents && myRepopulated)
			{
				return;
			}
			myRepopulated = true;
			Debug.Assert(mySelectedFactType != null);
			DisconnectTree();
			LinkedElementCollection<RoleBase> roleCollection = mySelectedFactType.RoleCollection;
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
			ITree tree = this.vtrSamplePopulation.Tree;
			if (tree != null)
			{
				// Null out the tree root to force event handlers to detach
				tree.Root = null;
				if (myInEvents)
				{
					myInEvents = false;
					tree.DelayRedraw = false;
				}
				this.vtrSamplePopulation.Tree = null;
			}
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
			this.myBranch = null;
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
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			DomainClassInfo classInfo;

			//Track Currently Executing Events
			eventDirectory.ElementEventsBegun.Add(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent));
			eventDirectory.ElementEventsEnded.Add(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent));

			// Track FactTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasFactTypeInstance.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasFactTypeInstanceAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasFactTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(FactTypeInstanceHasRoleInstance.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeInstanceHasRoleInstanceRemovedEvent));

			// Track EntityTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierRoleAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRoleRemovedEvent));
			
			// UNDONE: Also care about role reordering on preferred identifiers
			classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasEntityTypeInstance.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityTypeInstanceAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasEntityTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(EntityTypeInstanceHasRoleInstance.DomainClassId);
			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeInstanceHasRoleInstanceRemovedEvent));

			// Track ValueTypeInstance changes
			classInfo = dataDirectory.FindDomainClass(ValueTypeInstance.DomainClassId);
			eventDirectory.ElementPropertyChanged.Add(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValueTypeInstanceValueChangedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasFactType.DomainClassId);
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemovedEvent));

			// Track object type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasObjectType.DomainClassId);
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectTypeRemovedEvent));
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
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			DomainClassInfo classInfo;

			// Track Currently Executing Events
			eventDirectory.ElementEventsBegun.Remove(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent));
			eventDirectory.ElementEventsEnded.Remove(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent));

			// Track FactTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasFactTypeInstance.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasFactTypeInstanceAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasFactTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(FactTypeInstanceHasRoleInstance.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeInstanceHasRoleInstanceRemovedEvent));

			// Track EntityTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierRoleAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRoleRemovedEvent));

			// UNDONE: Also care about role reordering on preferred identifiers
			classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasEntityTypeInstance.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityTypeInstanceAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasEntityTypeInstanceRemovedEvent));

			classInfo = dataDirectory.FindDomainRelationship(EntityTypeInstanceHasRoleInstance.DomainClassId);
			eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeInstanceHasRoleInstanceAddedEvent));
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeInstanceHasRoleInstanceRemovedEvent));

			// Track ValueTypeInstance changes
			classInfo = dataDirectory.FindDomainClass(ValueTypeInstance.DomainClassId);
			eventDirectory.ElementPropertyChanged.Remove(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValueTypeInstanceValueChangedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasFactType.DomainClassId);
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemovedEvent));

			// Track object type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasObjectType.DomainClassId);
			eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectTypeRemovedEvent));
		}
		#endregion
		#region Fact Type Instance Event Handlers
		//handling model events Related to changes in Readings and their
		//connections so the reading editor can accurately reflect the model
		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			myInEvents = false; // Sanity, should not be needed
			myRepopulated = false;
			if (myBranch != null)
			{
				ITree tree = this.vtrSamplePopulation.Tree;
				if (tree != null)
				{
					myInEvents = true;
					this.vtrSamplePopulation.Tree.DelayRedraw = true;
				}
			}
		}
		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			if (myInEvents)
			{
				myInEvents = false;
				this.vtrSamplePopulation.Tree.DelayRedraw = false;
			}
		}
		private void FactTypeHasRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
			if (factType != null && factType == mySelectedFactType)
			{
				PopulateControlForFactType();
			}
		}
		private void FactTypeHasRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
			if (factType != null && !factType.IsDeleted && factType == mySelectedFactType)
			{
				PopulateControlForFactType();
			}
		}

		private void FactTypeHasFactTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
			if (link.FactType == mySelectedFactType)
			{
				SamplePopulationFactTypeBranch factBranch = myBranch as SamplePopulationFactTypeBranch;
				Debug.Assert(factBranch != null);
				factBranch.AddFactInstanceDisplay(link.FactTypeInstance);
			}
		}

		private void FactTypeHasFactTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType factType = (e.ModelElement as FactTypeHasFactTypeInstance).FactType;
			if (!factType.IsDeleted && factType == mySelectedFactType)
			{
				// UNDONE: Need to find the exact location of what is being removed, can't right now
				myBranch.RemoveInstanceDisplay();
			}
		}

		private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactTypeInstance factTypeInstance = (e.ModelElement as FactTypeInstanceHasRoleInstance).FactTypeInstance;
			FactType factType = factTypeInstance.FactType;
			if (factType != null && factType != mySelectedFactType)
			{
				SamplePopulationFactTypeBranch factBranch = myBranch as SamplePopulationFactTypeBranch;
				Debug.Assert(factBranch != null);
				factBranch.EditFactInstanceDisplay(factTypeInstance);
			}
		}

		private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactTypeInstance factTypeInstance = (e.ModelElement as FactTypeInstanceHasRoleInstance).FactTypeInstance;
			FactType factType;
			if (!factTypeInstance.IsDeleted &&
				null != (factType = factTypeInstance.FactType) &&
				factType == mySelectedFactType)
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
			if (myRepopulated)
			{
				return;
			}
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			ObjectType entityType = link.PreferredIdentifierFor;
			if(entityType != null && entityType == mySelectedEntityType)
			{
				PopulateControlForEntityType();
			}
		}
		private void EntityTypeHasPreferredIdentifierRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedEntityType != null)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor) &&
					mySelectedEntityType == preferredFor)
				{
					PopulateControlForEntityType();
				}
			}
		}
		private void EntityTypeHasPreferredIdentifierRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedEntityType != null)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					!uniqueness.IsDeleted &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor) &&
					mySelectedEntityType == preferredFor)
				{
					PopulateControlForEntityType();
				}
			}
		}

		private void EntityTypeHasPreferredIdentifierRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			ObjectType entityType = link.PreferredIdentifierFor;
			if (entityType != null && entityType == mySelectedEntityType)
			{
				PopulateControlForEntityType();
			}
		}

		private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
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
			//if (entityType != null && entityType == mySelectedEntityType)
			//{
			//    SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
			//    Debug.Assert(entityBranch != null);
			//    entityBranch.AddEntityInstanceDisplay(link.EntityTypeInstanceCollection);
			//}
		}

		private void EntityTypeHasEntityTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
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
			//if (entityType != null && entityType == mySelectedEntityType)
			//{
			//    // UNDONE: Need to find the exact location of what is being removed, can't right now
			//    myBranch.RemoveInstanceDisplay();
			//}
		}

		private void EntityTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
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
			//if(entityType != null && entityType == mySelectedEntityType)
			//{
			//    SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
			//    Debug.Assert(entityBranch != null);
			//    entityBranch.EditEntityInstanceDisplay(entityTypeInstance);
			//}
		}

		private void EntityTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
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
			//if (entityType != null && entityType == mySelectedEntityType)
			//{
			//    SamplePopulationEntityTypeBranch entityBranch = myBranch as SamplePopulationEntityTypeBranch;
			//    Debug.Assert(entityBranch != null);
			//    entityBranch.EditEntityInstanceDisplay(entityTypeInstance);
			//}
		}
		#endregion
		#region Value Type Instance Event Handlers
		private void ValueTypeInstanceValueChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			//UNDONE: Need to handle these changes for the nested branches somehow, for now just reload
			if (mySelectedEntityType != null)
			{
				PopulateControlForEntityType();
			}
			else if (mySelectedFactType != null)
			{
				PopulateControlForFactType();
			}
		}
		#endregion
		#region Misc Event Handlers
		private void FactTypeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			FactType factType = link.FactType;
			if (factType != null && factType == mySelectedFactType)
			{
				NullSelection();
			}
		}

		private void ObjectTypeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
			ObjectType objectType = link.ObjectType;
			if (objectType != null && 
				(objectType == mySelectedEntityType || objectType == mySelectedValueType))
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
				if (IsFullRowSelectColumn(column) || row < 0)
				{
					// Not sure why we need a < 0 check here, but I've had the VirtualTreeControl
					// request it.
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
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				if (IsFullRowSelectColumn(column))
				{
					retVal.State = VirtualTreeDisplayStates.GrayText | VirtualTreeDisplayStates.TextAlignFar;
				}
				return retVal;
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
				if (IsFullRowSelectColumn(column))
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

			protected bool IsExpandable(int row, int column)
			{
				return !IsFullRowSelectColumn(column);
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
			/// <summary>
			/// Attach event handlers needed to listen to changes on a specific branch type
			/// </summary>
			protected virtual void AttachEventHandlers(Store store)
			{
			}
			/// <summary>
			/// Detach event handlers needed to listen to changes on a specific branch type
			/// </summary>
			protected virtual void DetachEventHandlers(Store store)
			{
			}
			protected event BranchModificationEventHandler OnBranchModification
			{
				add
				{
					bool attachEvents = myModificationEvents == null;
					myModificationEvents += value;
					if (attachEvents)
					{
						AttachEventHandlers(myStore);
					}
				}
				remove
				{
					myModificationEvents -= value;
					if (myModificationEvents == null)
					{
						Store store = myStore;
						if (store != null && !store.Disposed)
						{
							DetachEventHandlers(store);
						}
					}
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
			protected static ObjectTypeInstance RecurseValueTypeInstance(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string newText, ref ValueTypeInstance rootInstance)
			{
				if (parentType.IsValueType)
				{
					DataType valueDataType = parentType.DataType;
					if(valueDataType.CanCompare)
					{
						LinkedElementCollection<ValueTypeInstance> instances = parentType.ValueTypeInstanceCollection;
						int instanceCount = instances.Count;
						for (int i = 0; i < instanceCount; ++i)
						{
							ValueTypeInstance currentValueInstance = instances[i];
							string value = currentValueInstance.Value;
							if (valueDataType.CanParseAnyValue ||
								(valueDataType.CanParse(value) && valueDataType.CanParse(newText)))
							{
								int compare = valueDataType.Compare(value, newText);
								if (compare == 0)
								{
									rootInstance = instances[i];
									return rootInstance;
								}
							}
						}
					}
					ValueTypeInstance editValueTypeInstance = objectTypeInstance as ValueTypeInstance;
					if (editValueTypeInstance == null)
					{
						editValueTypeInstance = new ValueTypeInstance(parentType.Store);
						editValueTypeInstance.ValueType = parentType;
					}
					rootInstance = editValueTypeInstance;
					return editValueTypeInstance;
				}
				else
				{
					LinkedElementCollection<Role> identifierRoles = parentType.PreferredIdentifier.RoleCollection;
					Debug.Assert(identifierRoles.Count == 1);
					Role identifierRole = identifierRoles[0];
					EntityTypeInstance editEntityTypeInstance = objectTypeInstance as EntityTypeInstance;
					EntityTypeRoleInstance editingRoleInstance = null;
					if (editEntityTypeInstance == null)
					{
						editEntityTypeInstance = new EntityTypeInstance(parentType.Store);
						editEntityTypeInstance.EntityType = parentType;
					}
					else
					{
						editingRoleInstance = editEntityTypeInstance.RoleInstanceCollection[0];
					}
					if (editingRoleInstance == null)
					{
						EntityTypeRoleInstance identifierInstance = new EntityTypeRoleInstance(identifierRole,
							RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref rootInstance));
						identifierInstance.EntityTypeInstance = editEntityTypeInstance;
						return editEntityTypeInstance;
					}
					else
					{
						return RecurseValueTypeInstance(editingRoleInstance.ObjectTypeInstance, identifierRole.RolePlayer, newText, ref rootInstance);
					}
				}
			}

			protected static string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, ObjectType parentType)
			{
				StringBuilder outputText = null;
				string retVal = (parentType == null) ? "" : RecurseObjectTypeInstanceValue(objectTypeInstance, parentType, null, ref outputText);
				return (outputText != null) ? outputText.ToString() : retVal;
			}

			private static string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string listSeparator, ref StringBuilder outputText)
			{
				ValueTypeInstance valueInstance;
				EntityTypeInstance entityTypeInstance;
				if (parentType == null)
				{
					if (outputText != null)
					{
						outputText.Append(" ");
					}
					return " ";
				}
				else if (parentType.IsValueType)
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
					LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
					int identifierCount = identifierRoles.Count;
					if (identifierCount == 1)
					{
						ObjectTypeInstance nestedInstance = null;
						if (entityTypeInstance != null)
						{
							LinkedElementCollection<EntityTypeRoleInstance> roleInstances = entityTypeInstance.RoleInstanceCollection;
							if (roleInstances.Count > 0)
							{
								nestedInstance = roleInstances[0].ObjectTypeInstance;
							}
						}
						return RecurseObjectTypeInstanceValue(nestedInstance, identifierRoles[0].RolePlayer, listSeparator, ref outputText);
					}
					else
					{
						LinkedElementCollection<EntityTypeRoleInstance> roleInstances = null;
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
									if (instance.Role == identifierRole)
									{
										RecurseObjectTypeInstanceValue(instance.ObjectTypeInstance, identifierRole.RolePlayer, listSeparator, ref outputText);
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
					myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, location - 1, 1));
					// UNDONE: If an add occurs due to an item being added in the UI, then in some cases we want
					// to go with the item (multi-column instances), and in others we want to stay on the last row (single column)
					//myModificationEvents(this, BranchModificationEventArgs.MoveItem(this, location - 1, location));
				}
			}

			public void EditInstanceDisplay(int location)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, location, 1, 1)));
					// Note: These are turned on to trigger the delayed redraw. DisplayDataChanged is not enough.
					myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
					myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
				}
			}

			public void RemoveInstanceDisplay()
			{
				if (myModificationEvents != null)
				{
					// Just delete at the beginning. We're never sure of
					// the location for a deleted item
					myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, 0, 1));
					// UNDONE: Turn off Realign support
					//myModificationEvents(this, BranchModificationEventArgs.Realign(this));
				}
			}
			public void RealignDisplay()
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.Realign(this));
				}
			}

			protected void AddAndInitializeValueTypeInstance(string newText, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ValueTypeInstance newInstance = new ValueTypeInstance(Store);
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
				removeInstance.Delete();
			}

			protected void RemoveEntityTypeInstance(int row, int column, ObjectType parentEntityType)
			{
				Debug.Assert(!parentEntityType.IsValueType);
				EntityTypeInstance removeInstance = parentEntityType.EntityTypeInstanceCollection[row];
				Debug.Assert(removeInstance != null);
				removeInstance.Delete();
			}
			#endregion
		}
		private sealed class SamplePopulationValueTypeBranch : SamplePopulationBaseBranch, IBranch
		{
			private readonly ObjectType myValueType;
			#region Construction
			// Value Type Branches will always have 1 column, plus the full row select column
			public SamplePopulationValueTypeBranch(ObjectType selectedValueType) : base(2, selectedValueType.Store)
			{
				Debug.Assert(selectedValueType.IsValueType);
				myValueType = selectedValueType;
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
				{
					if (row != NewRowIndex)
					{
						retVal.AlternateText = myValueType.ValueTypeInstanceCollection[row].Value;
					}
				}
				return retVal;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
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

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (row == NewRowIndex)
				{
					return null;
				}
				return myValueType.ValueTypeInstanceCollection[row];
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text != null && text.Length == 0)
				{
					text = myValueType.ValueTypeInstanceCollection[row].Value;
				}
				return text;
			}

			// Cache the item count so we can get accurate event notifications
			private int myItemCountCache = -1;
			int IBranch.VisibleItemCount
			{
				get
				{
					int cache = myItemCountCache;
					if (cache == -1)
					{
						myItemCountCache = cache = myValueType.ValueTypeInstanceCollection.Count + base.VisibleItemCount;
					}
					return cache;
				}
			}
			#endregion
			#region Branch Update Methods
			protected override void AttachEventHandlers(Store store)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				EventManagerDirectory eventDirectory = store.EventManagerDirectory;
				DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(ValueTypeHasValueTypeInstance.DomainClassId);
				eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasValueTypeInstanceAddedEvent));
				eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasValueTypeInstanceDeletedEvent));

				classInfo = dataDirectory.FindDomainClass(ValueTypeInstance.DomainClassId);
				eventDirectory.ElementPropertyChanged.Add(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValueTypeInstanceValueChangedEvent));
			}
			protected override void DetachEventHandlers(Store store)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				EventManagerDirectory eventDirectory = store.EventManagerDirectory;
				DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(ValueTypeHasValueTypeInstance.DomainClassId);
				eventDirectory.ElementAdded.Remove(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasValueTypeInstanceAddedEvent));
				eventDirectory.ElementDeleted.Remove(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasValueTypeInstanceDeletedEvent));

				classInfo = dataDirectory.FindDomainClass(ValueTypeInstance.DomainClassId);
				eventDirectory.ElementPropertyChanged.Remove(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValueTypeInstanceValueChangedEvent));
			}
			private void ValueTypeHasValueTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
				if (link.ValueType == myValueType)
				{
					if (myItemCountCache != -1)
					{
						++myItemCountCache;
					}
					base.AddInstanceDisplay(VisibleItemCount - 1);
				}
			}
			private void ValueTypeHasValueTypeInstanceDeletedEvent(object sender, ElementDeletedEventArgs e)
			{
				ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
				if (link.ValueType == myValueType)
				{
					if (myItemCountCache != -1)
					{
						--myItemCountCache;
					}
					base.RemoveInstanceDisplay();
				}
			}
			private void ValueTypeInstanceValueChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ValueTypeInstance instance = e.ModelElement as ValueTypeInstance;
				ObjectType modifiedValueType = instance.ValueType;
				if (modifiedValueType != null && !modifiedValueType.IsDeleted && modifiedValueType == myValueType)
				{
					int location = modifiedValueType.ValueTypeInstanceCollection.IndexOf(instance);
					if (location != -1)
					{
						base.EditInstanceDisplay(location);
					}
				}
			}
			#endregion
		}
		private sealed class SamplePopulationEntityTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			private readonly ObjectType myEntityType;
			#region Construction
			public SamplePopulationEntityTypeBranch(ObjectType entityType, int numColumns)
				: base(numColumns, entityType.Store)
			{
				Debug.Assert(!entityType.IsValueType);
				myEntityType = entityType;
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			#region CellEditContext class, used for label editing
			/// <summary>
			/// A helper class to provide a context for label editing
			/// </summary>
			private sealed class CellEditContext
			{
				#region EntityTypeInstanceColumnDescriptor class
				/// <summary>
				/// A property descriptor to host inside the TypeEditorHost.
				/// Handles all in-place editing.
				/// </summary>
				private sealed class EntityTypeInstanceColumnDescriptor : PropertyDescriptor
				{
					#region InstanceDropDown class
					/// <summary>
					/// An ElementPicker list used to display available instance values
					/// </summary>
					private sealed class InstanceDropDown : ElementPicker
					{
						private IList myInstances;
						private static Size myLastControlSize = Size.Empty;
						/// <summary>
						/// Manage control size independently
						/// </summary>
						protected sealed override Size LastControlSize
						{
							get { return myLastControlSize; }
							set { myLastControlSize = value; }
						}
						/// <summary>
						/// Translate the displayed text to the underlying instance
						/// </summary>
						protected sealed override object TranslateFromDisplayObject(int newIndex, object newObject)
						{
							return (newIndex >= 0) ? myInstances[newIndex] : null;
						}
						/// <summary>
						/// Translate the initial value into its corresponding text so it
						/// can be selected in the list
						/// </summary>
						protected sealed override object TranslateToDisplayObject(object initialObject, IList contentList)
						{
							int index = myInstances.IndexOf(initialObject);
							return (index >= 0) ? contentList[index] : null;
						}
						/// <summary>
						/// Provide text for a null item at the top of the list
						/// </summary>
						protected sealed override string NullItemText
						{
							get
							{
								// UNDONE: Localize null text
								return "<Unspecified>";
							}
						}
						/// <summary>
						/// Return the string values for the contents of the dropdown list
						/// </summary>
						protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
						{
							CellEditContext instance = (CellEditContext)context.Instance;
							Role role = instance.myRole;
							ObjectType rolePlayer = role.RolePlayer;
							if (rolePlayer != null)
							{
								IList instances = rolePlayer.IsValueType ? (IList)rolePlayer.ValueTypeInstanceCollection : rolePlayer.EntityTypeInstanceCollection;
								int instanceCount = instances.Count;
								string[] strings = new string[instanceCount];
								for (int i = 0; i < instanceCount; ++i)
								{
									strings[i] = RecurseObjectTypeInstanceValue((ObjectTypeInstance)instances[i], rolePlayer);
								}
								myInstances = instances;
								return strings;
							}
							return null;
						}
					}
					#endregion // InstanceDropDown class
					#region InstanceConverter class
					private sealed class InstanceConverter : StringConverter
					{
						public sealed override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
						{
							string stringValue = value as string;
							if (stringValue != null)
							{
								if (stringValue.Length == 0)
								{
									return null;
								}
								else
								{
									CellEditContext editContext = (CellEditContext)context.Instance;
									ObjectTypeInstance currentInstance = editContext.myColumnInstance;
									if (currentInstance != null && 0 == string.CompareOrdinal(stringValue, RecurseObjectTypeInstanceValue(currentInstance, editContext.myRole.RolePlayer)))
									{
										return currentInstance;
									}
									// UNDONE: We'll hit this if the user edits the text and opens the dropdown without committing
									// Should we add a new item at this point, or just try to get the best match from the list?
									return stringValue;
								}
							}
							return base.ConvertFrom(context, culture, value);
						}
						public sealed override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
						{
							CellEditContext editContext = (CellEditContext)context.Instance;
							ObjectType rolePlayer;
							UniquenessConstraint preferredIdentifier;
							if (sourceType == typeof(string) &&
								((null == (rolePlayer = ((CellEditContext)context.Instance).myRole.RolePlayer)) ||
								(!rolePlayer.IsValueType &&
								(null == (preferredIdentifier = rolePlayer.PreferredIdentifier) ||
								preferredIdentifier.RoleCollection.Count > 1))))
							{
								return false;
							}
							return base.CanConvertFrom(context, sourceType);
						}
						public sealed override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
						{
							ObjectTypeInstance typedValue = (ObjectTypeInstance)value;
							if (typedValue != null)
							{
								return RecurseObjectTypeInstanceValue(typedValue, ((CellEditContext)context.Instance).myRole.RolePlayer);
							}
							return string.Empty;
						}
					}
					#endregion // InstanceConverter class
					#region Constructor
					public EntityTypeInstanceColumnDescriptor()
						: base(" ", null)
					{
					}
					#endregion // Constructor
					#region Base Overrides
					private static readonly TypeConverter TypeConverter = new InstanceConverter();
					public sealed override TypeConverter Converter
					{
						get
						{
							return TypeConverter;
						}
					}
					public sealed override object GetEditor(Type editorBaseType)
					{
						Debug.Assert(editorBaseType == typeof(UITypeEditor));
						return new InstanceDropDown();
					}
					public sealed override object GetValue(object component)
					{
						return (component as CellEditContext).myColumnInstance;
					}
					public sealed override void SetValue(object component, object value)
					{
						// Ignore strings coming through here with an as cast. Opening the
						// dropdown with dirty text will attempt a set value
						ObjectTypeInstance typedValue = value as ObjectTypeInstance;
						CellEditContext context = (CellEditContext)component;
						ObjectTypeInstance columnInstance = context.myColumnInstance;
						Role role = context.myRole;
						if (typedValue == null)
						{
							if (columnInstance != null)
							{
								EntityTypeInstance entityTypeInstance = context.myEntityTypeInstance;
								// UNDONE: Localize transaction name
								using (Transaction t = role.Store.TransactionManager.BeginTransaction("Clear role instance"))
								{
									entityTypeInstance.FindRoleInstance(role).Delete();
									t.Commit();
								}
								// Removing the last role instance can remove the fact type instance, check
								if (entityTypeInstance.IsDeleted)
								{
									context.myEntityTypeInstance = null;
								}
								context.myColumnInstance = null;
							}
						}
						else if (columnInstance != typedValue)
						{
							// UNDONE: Localize transaction name
							using (Transaction t = role.Store.TransactionManager.BeginTransaction("Set role instance"))
							{
								ConnectInstance(context.myEntityType, ref context.myEntityTypeInstance, typedValue, role);
								t.Commit();
							}
							context.myColumnInstance = typedValue;
						}
					}
					public sealed override Type ComponentType
					{
						get { return typeof(ObjectTypeInstance); }
					}
					public sealed override Type PropertyType
					{
						get { return typeof(ObjectTypeInstance); }
					}
					public sealed override bool IsReadOnly
					{
						get { return false; }
					}
					public sealed override bool CanResetValue(object component)
					{
						return false;
					}
					public sealed override void ResetValue(object component)
					{
					}
					public sealed override bool ShouldSerializeValue(object component)
					{
						return true;
					}
					#endregion // Base Overrides
				}
				#endregion // EntityTypeInstanceColumnDescriptor class
				#region Member Variables
				private static readonly EntityTypeInstanceColumnDescriptor Descriptor = new EntityTypeInstanceColumnDescriptor();
				private readonly Role myRole;
				private readonly ObjectType myEntityType;
				private EntityTypeInstance myEntityTypeInstance;
				private ObjectTypeInstance myColumnInstance;
				#endregion // Member Variables
				#region Constructor
				/// <summary>
				/// Create an editing context for the given entityType role and entityTypeInstance
				/// </summary>
				/// <param name="entityType">The entityType to attach to instances to.</param>
				/// <param name="role">The role being edited</param>
				/// <param name="entityTypeInstance">The current entityTypeInstance. Can be null.</param>
				public CellEditContext(ObjectType entityType, Role role, EntityTypeInstance entityTypeInstance)
				{
					Debug.Assert(entityType != null);
					Debug.Assert(role != null);
					myEntityType = entityType;
					myRole = role;
					myEntityTypeInstance = entityTypeInstance;
					if (entityTypeInstance != null)
					{
						EntityTypeRoleInstance roleInstance = entityTypeInstance.FindRoleInstance(role);
						if (roleInstance != null)
						{
							myColumnInstance = roleInstance.ObjectTypeInstance;
						}
					}
				}
				#endregion // Constructor
				#region CreateInPlaceEditControl method
				/// <summary>
				/// Create an inplace edit control that works with this context
				/// </summary>
				/// <returns>IVirtualTreeInPlaceControl</returns>
				public IVirtualTreeInPlaceControl CreateInPlaceEditControl()
				{
					ObjectType rolePlayer;
					UniquenessConstraint preferredIdentifier;
					bool blockEdits =
						(null == (rolePlayer = myRole.RolePlayer)) ||
						(!rolePlayer.IsValueType &&
						(null == (preferredIdentifier = rolePlayer.PreferredIdentifier) ||
						preferredIdentifier.RoleCollection.Count > 1));
					TypeEditorHost host = TypeEditorHost.Create(
						Descriptor,
						this,
						blockEdits ? TypeEditorHostEditControlStyle.ReadOnlyEdit : TypeEditorHostEditControlStyle.Editable);
					if (host != null)
					{
						(host as IVirtualTreeInPlaceControl).Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.SizeToText;
					}
					return host;
				}
				#endregion // CreateInPlaceEditControl method
			}
			#endregion // CellEditContext class, used for label editing

			private VirtualTreeLabelEditData BeginLabelEdit_Start(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
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
						retVal = VirtualTreeLabelEditData.Invalid;
					}
				}
				return retVal;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
				{
					ObjectType entityType = myEntityType;
					LinkedElementCollection<EntityTypeInstance> instances = entityType.EntityTypeInstanceCollection;
					retVal.CustomInPlaceEdit = new CellEditContext(entityType, entityType.PreferredIdentifier.RoleCollection[column - 1], (row < instances.Count) ? instances[row] : null).CreateInPlaceEditControl();
					retVal.CustomCommit = delegate(VirtualTreeItemInfo itemInfo, Control editControl)
					{
						// Defer to the normal text edit if the control is not dirty
						return (editControl as IVirtualTreeInPlaceControl).Dirty ? itemInfo.Branch.CommitLabelEdit(itemInfo.Row, itemInfo.Column, editControl.Text) : LabelEditResult.CancelEdit;
					};
				}
				return retVal;
			}

			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
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
						ObjectTypeInstance result = RecurseValueTypeInstance(editRoleInstance.ObjectTypeInstance, editRoleInstance.Role.RolePlayer, newText, ref instance);
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
							ConnectInstance(myEntityType, ref editInstance, result, factRole);
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
						EntityTypeInstance parentInstance = null;
						ConnectInstance(myEntityType, ref parentInstance, result, identifierRole);
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
				}
				return LabelEditResult.CancelEdit;
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.SubItemExpansion)
				{
					ObjectType selectedEntityType = myEntityType;
					LinkedElementCollection<EntityTypeInstance> instances = selectedEntityType.EntityTypeInstanceCollection;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					EntityTypeInstance parentInstance = (row < instances.Count) ? instances[row] : null;
					EntityTypeInstance editInstance = null;
					if (parentInstance != null)
					{
						LinkedElementCollection<EntityTypeRoleInstance> roleInstances = parentInstance.RoleInstanceCollection;
						int roleInstanceCount = roleInstances.Count;
						EntityTypeRoleInstance roleInstance;
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							if ((roleInstance = roleInstances[i]).Role == identifierRole)
							{
								editInstance = roleInstance.ObjectTypeInstance as EntityTypeInstance;
								break;
							}
						}
					}
					return new SamplePopulationEntityEditorBranch(parentInstance, selectedEntityType, editInstance, identifierRole, this);
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
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
					LinkedElementCollection<EntityTypeRoleInstance> entityTypeRoleInstances = selectedInstance.RoleInstanceCollection;
					int roleInstanceCount = entityTypeRoleInstances.Count;
					EntityTypeRoleInstance roleInstance;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						if (identifierRole == (roleInstance = entityTypeRoleInstances[i]).Role)
						{
							text = RecurseObjectTypeInstanceValue(roleInstance.ObjectTypeInstance, identifierRole.RolePlayer);
							return text;
						}
					}
					text = RecurseObjectTypeInstanceValue(null, identifierRole.RolePlayer);
				}
				return text;
			}

			bool IBranch.IsExpandable(int row, int column)
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

			int IBranch.VisibleItemCount
			{
				get
				{
					return myEntityType.EntityTypeInstanceCollection.Count + base.VisibleItemCount;
				}
			}
			#endregion
			#region IMultiColumnBranch Member Mirror/Implementation
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				switch (column)
				{
					case 0:
						return SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Expandable;
				}
			}
			#endregion
			#region Helper Methods
			/// <summary>
			/// The entity type used to create this branch
			/// </summary>
			public ObjectType EntityType
			{
				get
				{
					return myEntityType;
				}
			}
			/// <summary>
			/// Connect a given instance to the specified entity type, for the given role
			/// </summary>
			/// <param name="entityType">The parent entity. Cannot be null.</param>
			/// <param name="parentInstance">Instance to connect to. Created if needed.</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect to</param>
			public static void ConnectInstance(ObjectType entityType, ref EntityTypeInstance parentInstance, ObjectTypeInstance connectInstance, Role identifierRole)
			{
				Debug.Assert(entityType != null);
				Debug.Assert(connectInstance != null);
				Store store = entityType.Store;

				if (parentInstance == null)
				{
					parentInstance = new EntityTypeInstance(store);
					parentInstance.EntityType = entityType;
				}
				EntityTypeRoleInstance roleInstance = new EntityTypeRoleInstance(identifierRole, connectInstance);
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
		private sealed class SamplePopulationFactTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			private readonly FactType myFactType;
			#region Construction
			public SamplePopulationFactTypeBranch(FactType selectedFactType, int numColumns)
				: base(numColumns, selectedFactType.Store)
			{
				myFactType = selectedFactType;
			}
			#endregion
			#region IBranch Member Mirror/Implementations
			#region CellEditContext class, used for label editing
			/// <summary>
			/// A helper class to provide a context for label editing
			/// </summary>
			private sealed class CellEditContext
			{
				#region FactTypeInstanceColumnDescriptor class
				/// <summary>
				/// A property descriptor to host inside the TypeEditorHost.
				/// Handles all in-place editing.
				/// </summary>
				private sealed class FactTypeInstanceColumnDescriptor : PropertyDescriptor
				{
					#region InstanceDropDown class
					/// <summary>
					/// An ElementPicker list used to display available instance values
					/// </summary>
					private sealed class InstanceDropDown : ElementPicker
					{
						private IList myInstances;
						private static Size myLastControlSize = Size.Empty;
						/// <summary>
						/// Manage control size independently
						/// </summary>
						protected sealed override Size LastControlSize
						{
							get { return myLastControlSize; }
							set { myLastControlSize = value; }
						}
						/// <summary>
						/// Translate the displayed text to the underlying instance
						/// </summary>
						protected sealed override object TranslateFromDisplayObject(int newIndex, object newObject)
						{
							return (newIndex >= 0) ? myInstances[newIndex] : null;
						}
						/// <summary>
						/// Translate the initial value into its corresponding text so it
						/// can be selected in the list
						/// </summary>
						protected sealed override object TranslateToDisplayObject(object initialObject, IList contentList)
						{
							int index = myInstances.IndexOf(initialObject);
							return (index >= 0) ? contentList[index] : null;
						}
						/// <summary>
						/// Provide text for a null item at the top of the list
						/// </summary>
						protected sealed override string NullItemText
						{
							get
							{
								// UNDONE: Localize null text
								return "<Unspecified>";
							}
						}
						/// <summary>
						/// Return the string values for the contents of the dropdown list
						/// </summary>
						protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
						{
							CellEditContext instance = (CellEditContext)context.Instance;
							Role role = instance.myRole;
							ObjectType rolePlayer = role.RolePlayer;
							if (rolePlayer != null)
							{
								IList instances = rolePlayer.IsValueType ? (IList)rolePlayer.ValueTypeInstanceCollection : rolePlayer.EntityTypeInstanceCollection;
								int instanceCount = instances.Count;
								string[] strings = new string[instanceCount];
								for (int i = 0; i < instanceCount; ++i)
								{
									strings[i] = RecurseObjectTypeInstanceValue((ObjectTypeInstance)instances[i], rolePlayer);
								}
								myInstances = instances;
								return strings;
							}
							return null;
						}
					}
					#endregion // InstanceDropDown class
					#region InstanceConverter class
					private sealed class InstanceConverter : StringConverter
					{
						public sealed override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
						{
							string stringValue = value as string;
							if (stringValue != null)
							{
								if (stringValue.Length == 0)
								{
									return null;
								}
								else
								{
									CellEditContext editContext = (CellEditContext)context.Instance;
									ObjectTypeInstance currentInstance = editContext.myColumnInstance;
									if (currentInstance != null && 0 == string.CompareOrdinal(stringValue, RecurseObjectTypeInstanceValue(currentInstance, editContext.myRole.RolePlayer)))
									{
										return currentInstance;
									}
									// UNDONE: We'll hit this if the user edits the text and opens the dropdown without committing
									// Should we add a new item at this point, or just try to get the best match from the list?
									return stringValue;
								}
							}
							return base.ConvertFrom(context, culture, value);
						}
						public sealed override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
						{
							CellEditContext editContext = (CellEditContext)context.Instance;
							ObjectType rolePlayer;
							UniquenessConstraint preferredIdentifier;
							if (sourceType == typeof(string) &&
								((null == (rolePlayer = ((CellEditContext)context.Instance).myRole.RolePlayer)) ||
								(!rolePlayer.IsValueType &&
								(null == (preferredIdentifier = rolePlayer.PreferredIdentifier) ||
								preferredIdentifier.RoleCollection.Count > 1))))
							{
								return false;
							}
							return base.CanConvertFrom(context, sourceType);
						}
						public sealed override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
						{
							ObjectTypeInstance typedValue = (ObjectTypeInstance)value;
							if (typedValue != null)
							{
								return RecurseObjectTypeInstanceValue(typedValue, ((CellEditContext)context.Instance).myRole.RolePlayer);
							}
							return string.Empty;
						}
					}
					#endregion // InstanceConverter class
					#region Constructor
					public FactTypeInstanceColumnDescriptor()
						: base(" ", null)
					{
					}
					#endregion // Constructor
					#region Base Overrides
					private static readonly TypeConverter TypeConverter = new InstanceConverter();
					public sealed override TypeConverter Converter
					{
						get
						{
							return TypeConverter;
						}
					}
					public sealed override object GetEditor(Type editorBaseType)
					{
						Debug.Assert(editorBaseType == typeof(UITypeEditor));
						return new InstanceDropDown();
					}
					public sealed override object GetValue(object component)
					{
						return (component as CellEditContext).myColumnInstance;
					}
					public sealed override void SetValue(object component, object value)
					{
						// Ignore strings coming through here with an as cast. Opening the
						// dropdown with dirty text will attempt a set value
						ObjectTypeInstance typedValue = value as ObjectTypeInstance;
						CellEditContext context = (CellEditContext)component;
						ObjectTypeInstance columnInstance = context.myColumnInstance;
						Role role = context.myRole;
						if (typedValue == null)
						{
							if (columnInstance != null)
							{
								// UNDONE: Localize transaction name
								FactTypeInstance factTypeInstance = context.myFactTypeInstance;
								using (Transaction t = role.Store.TransactionManager.BeginTransaction("Clear role instance"))
								{
									factTypeInstance.FindRoleInstance(role).Delete();
									t.Commit();
								}
								// Removing the last role instance can remove the fact type instance, check
								if (factTypeInstance.IsDeleted)
								{
									context.myFactTypeInstance = null;
								}
								context.myColumnInstance = null;
							}
						}
						else if (columnInstance != typedValue)
						{
							// UNDONE: Localize transaction name
							using (Transaction t = role.Store.TransactionManager.BeginTransaction("Set role instance"))
							{
								ConnectInstance(ref context.myFactTypeInstance, typedValue, role);
								t.Commit();
							}
							context.myColumnInstance = typedValue;
						}
					}
					public sealed override Type ComponentType
					{
						get { return typeof(ObjectTypeInstance); }
					}
					public sealed override Type PropertyType
					{
						get { return typeof(ObjectTypeInstance); }
					}
					public sealed override bool IsReadOnly
					{
						get { return false; }
					}
					public sealed override bool CanResetValue(object component)
					{
						return false;
					}
					public sealed override void ResetValue(object component)
					{
					}
					public sealed override bool ShouldSerializeValue(object component)
					{
						return false;
					}
					#endregion // Base Overrides
				}
				#endregion // FactTypeInstanceColumnDescriptor class
				#region Member Variables
				private static readonly FactTypeInstanceColumnDescriptor Descriptor = new FactTypeInstanceColumnDescriptor();
				private Role myRole;
				private FactTypeInstance myFactTypeInstance;
				private ObjectTypeInstance myColumnInstance;
				#endregion // Member Variables
				#region Constructor
				/// <summary>
				/// Create an editing context for the given role and factTypeInstance
				/// </summary>
				/// <param name="role">The role being edited</param>
				/// <param name="factTypeInstance">The current factTypeInstance. Can be null.</param>
				public CellEditContext(Role role, FactTypeInstance factTypeInstance)
				{
					Debug.Assert(role != null);
					myRole = role;
					myFactTypeInstance = factTypeInstance;
					if (factTypeInstance != null)
					{
						FactTypeRoleInstance roleInstance = factTypeInstance.FindRoleInstance(role);
						if (roleInstance != null)
						{
							myColumnInstance = roleInstance.ObjectTypeInstance;
						}
					}
				}
				#endregion // Constructor
				#region CreateInPlaceEditControl method
				/// <summary>
				/// Create an inplace edit control that works with this context
				/// </summary>
				/// <returns>IVirtualTreeInPlaceControl</returns>
				public IVirtualTreeInPlaceControl CreateInPlaceEditControl()
				{
					ObjectType rolePlayer;
					UniquenessConstraint preferredIdentifier;
					bool blockEdits =
						(null == (rolePlayer = myRole.RolePlayer)) ||
						(!rolePlayer.IsValueType &&
						(null == (preferredIdentifier = rolePlayer.PreferredIdentifier) ||
						preferredIdentifier.RoleCollection.Count > 1));
					TypeEditorHost host = TypeEditorHost.Create(
						Descriptor,
						this,
						blockEdits ? TypeEditorHostEditControlStyle.ReadOnlyEdit : TypeEditorHostEditControlStyle.Editable);
					if (host != null)
					{
						(host as IVirtualTreeInPlaceControl).Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.SizeToText;
					}
					return host;
				}
				#endregion // CreateInPlaceEditControl method
			}
			#endregion // CellEditContext class, used for label editing
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
				{
					LinkedElementCollection<FactTypeInstance> instances = myFactType.FactTypeInstanceCollection;
					retVal.CustomInPlaceEdit = new CellEditContext(myFactType.RoleCollection[column - 1].Role, (row < instances.Count) ? instances[row] : null).CreateInPlaceEditControl();
					retVal.CustomCommit = delegate(VirtualTreeItemInfo itemInfo, Control editControl)
					{
						// Defer to the normal text edit if the control is not dirty
						return (editControl as IVirtualTreeInPlaceControl).Dirty ? itemInfo.Branch.CommitLabelEdit(itemInfo.Row, itemInfo.Column, editControl.Text) : LabelEditResult.CancelEdit;
					};
				}
				return retVal;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				bool delete = newText.Length == 0;
				Store store = Store;
				// If editing an existing FactTypeInstance
				if (row != NewRowIndex)
				{
					FactType selectedFactType = myFactType;
					FactTypeInstance editInstance = selectedFactType.FactTypeInstanceCollection[row];
					FactTypeRoleInstance editRoleInstance = null;
					LinkedElementCollection<FactTypeRoleInstance> roleInstances = editInstance.RoleInstanceCollection;
					int instanceCount = roleInstances.Count;
					Role factRole = selectedFactType.RoleCollection[column - 1].Role;
					for (int i = 0; i < instanceCount; ++i)
					{
						if (factRole == roleInstances[i].Role)
						{
							editRoleInstance = roleInstances[i];
							break;
						}
					}
					// If editing an existing FactTypeRoleInstance
					if (editRoleInstance != null)
					{
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(editRoleInstance.ObjectTypeInstance, editRoleInstance.Role.RolePlayer, newText, ref instance);
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
							FactTypeRoleInstance roleInstance = new FactTypeRoleInstance(factRole, result);
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
						FactTypeInstance newInstance = new FactTypeInstance(store);
						newInstance.FactType = selectedFactType;
						Role factRole = selectedFactType.RoleCollection[column - 1].Role;
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance);
						EditValueTypeInstance(instance, newText);
						FactTypeRoleInstance roleInstance = new FactTypeRoleInstance(factRole, result);
						roleInstance.FactTypeInstance = newInstance;
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
				}
				return LabelEditResult.CancelEdit;
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.SubItemExpansion)
				{
					FactType selectedFactType = myFactType;
					LinkedElementCollection<FactTypeInstance> instances = selectedFactType.FactTypeInstanceCollection;
					Role selectedRole = selectedFactType.RoleCollection[column - 1].Role;
					FactTypeInstance parentInstance = (row < instances.Count) ? instances[row] : null;
					EntityTypeInstance editInstance = null;
					if (parentInstance != null)
					{
						LinkedElementCollection<FactTypeRoleInstance> roleInstances = parentInstance.RoleInstanceCollection;
						int roleInstanceCount = roleInstances.Count;
						FactTypeRoleInstance roleInstance;
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							if ((roleInstance = roleInstances[i]).Role == selectedRole)
							{
								editInstance = roleInstance.ObjectTypeInstance as EntityTypeInstance;
								break;
							}
						}
					}
					return new SamplePopulationEntityEditorBranch(parentInstance, selectedFactType, editInstance, selectedRole, this);
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null)
				{
					text = RecurseObjectTypeInstanceValue(null, myFactType.RoleCollection[column - 1].Role.RolePlayer);
				}
				else if (text.Length == 0)
				{
					FactTypeInstance factTypeInstance = myFactType.FactTypeInstanceCollection[row];
					LinkedElementCollection<FactTypeRoleInstance> factTypeRoleInstances = factTypeInstance.RoleInstanceCollection;
					int roleInstanceCount = factTypeRoleInstances.Count;
					FactTypeRoleInstance instance;
					Role factTypeRole = myFactType.RoleCollection[column - 1].Role;
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						if (factTypeRole == (instance = factTypeRoleInstances[i]).Role)
						{
							return RecurseObjectTypeInstanceValue(instance.ObjectTypeInstance, factTypeRole.RolePlayer);
						}
					}
					text = RecurseObjectTypeInstanceValue(null, factTypeRole.RolePlayer);
				}
				return text;
			}

			bool IBranch.IsExpandable(int row, int column)
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

			int IBranch.VisibleItemCount
			{
				get
				{
					return myFactType.FactTypeInstanceCollection.Count + base.VisibleItemCount;
				}
			}
			#endregion
			#region IMultiColumnBranch Member Mirror/Implementation
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				switch (column)
				{
					case 0:
						return SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Expandable;
				}
			}
			#endregion
			#region Helper Methods
			/// <summary>
			/// Connect a given instance to the branch's current objectType, for the given role
			/// </summary>
			/// <param name="parentInstance">Instance to connect to. Can be null.</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect to</param>
			public static void ConnectInstance(ref FactTypeInstance parentInstance, ObjectTypeInstance connectInstance, Role identifierRole)
			{
				Debug.Assert(connectInstance != null);
				Store store = identifierRole.Store;
				FactType factType = identifierRole.FactType;
				if (parentInstance == null)
				{
					parentInstance = new FactTypeInstance(store);
					parentInstance.FactType = factType;
				}
				FactTypeRoleInstance roleInstance = new FactTypeRoleInstance(identifierRole, connectInstance);
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
		private sealed class SamplePopulationEntityEditorBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly SamplePopulationBaseBranch myParentBranch;
			private readonly EntityTypeInstance myEditInstance;
			private EntityTypeInstance myParentEntityInstance;
			private readonly ObjectType myParentEntityType;
			private readonly FactType myParentFactType;
			private FactTypeInstance myParentFactInstance;
			private readonly Role myEditRole;
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
			BranchFeatures IBranch.Features
			{
				get
				{
					return (SamplePopulationBaseBranch.Features & (~BranchFeatures.ComplexColumns)) | BranchFeatures.Expansions;
				}
			}

			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
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

			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				ObjectType instanceType = myEditRole.RolePlayer;
				Role identifierRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType editType = identifierRole.RolePlayer;
				EntityTypeInstance editInstance = myEditInstance;
				ObjectTypeInstance roleEditInstance = null;
				if (editInstance != null)
				{
					EntityTypeRoleInstance foundRoleInstance = editInstance.FindRoleInstance(identifierRole);
					if (foundRoleInstance != null)
					{
						roleEditInstance = foundRoleInstance.ObjectTypeInstance;
					}
				}
				bool delete = newText.Length == 0;
				Store store = Store;
				ValueTypeInstance vEditInstance;
				//EntityTypeInstance eEditInstance;
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
					if (delete)
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
#if FALSE // UNDONE: Inline entity editing
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
							EditValueTypeInstance(eEditInstance, newText);
							t.Commit();
						}
					}
					return LabelEditResult.AcceptEdit;
				}
#endif // FALSE // UNDONE: Inline entity editing
				return LabelEditResult.CancelEdit;
			}

			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return VirtualTreeDisplayData.Empty;
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					ObjectType selectedEntityType = myEditRole.RolePlayer;
					LinkedElementCollection<EntityTypeInstance> instances = selectedEntityType.EntityTypeInstanceCollection;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[row];
					EntityTypeInstance parentInstance = myEditInstance;
					EntityTypeInstance editInstance = null;
					if (parentInstance != null)
					{
						EntityTypeRoleInstance foundRoleInstance = parentInstance.FindRoleInstance(identifierRole);
						if (foundRoleInstance != null)
						{
							editInstance = foundRoleInstance.ObjectTypeInstance as EntityTypeInstance;
							Debug.Assert(editInstance != null);
						}
					}
					return new SamplePopulationEntityEditorBranch(parentInstance, selectedEntityType, editInstance, identifierRole, this);
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
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
						return RecurseObjectTypeInstanceValue(selectedRoleInstance.ObjectTypeInstance, identifierType);
					}
				}
				return RecurseObjectTypeInstanceValue(null, identifierType);
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				ObjectType instanceType = myEditRole.RolePlayer;
				Role identifierRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType rolePlayer = identifierRole.RolePlayer;
				UniquenessConstraint roleIdentifier = rolePlayer.PreferredIdentifier;
				return (roleIdentifier != null) && (roleIdentifier.RoleCollection.Count > 1);
			}

			int IBranch.VisibleItemCount
			{
				get
				{
					return myEditRole.RolePlayer.PreferredIdentifier.RoleCollection.Count;
				}
			}
			#endregion
			#region Helper Methods
			public sealed override bool IsFullRowSelectColumn(int column)
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
				EntityTypeRoleInstance connection = new EntityTypeRoleInstance(identifierRole, instance);
				if (editInstance == null)
				{
					editInstance = new EntityTypeInstance(store);
					editInstance.EntityType = myEditRole.RolePlayer;
					connection.EntityTypeInstance = editInstance;
					SamplePopulationEntityTypeBranch entityBranch;
					SamplePopulationFactTypeBranch factBranch;
					SamplePopulationEntityEditorBranch editBranch;
					if (null != (entityBranch = myParentBranch as SamplePopulationEntityTypeBranch))
					{
						SamplePopulationEntityTypeBranch.ConnectInstance(entityBranch.EntityType, ref myParentEntityInstance, editInstance, myEditRole);
					}
					else if (null != (factBranch = myParentBranch as SamplePopulationFactTypeBranch))
					{
						SamplePopulationFactTypeBranch.ConnectInstance(ref myParentFactInstance, editInstance, myEditRole);
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
		private sealed class SamplePopulationVirtualTree : MultiColumnTree
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

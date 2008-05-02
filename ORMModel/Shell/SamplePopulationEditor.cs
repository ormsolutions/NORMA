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
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling.Shell;

#if VISUALSTUDIO_9_0
using VirtualTreeInPlaceControlFlags = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeInPlaceControls;
#endif //VISUALSTUDIO_9_0

namespace Neumont.Tools.ORM.Shell
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
			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[numColumns + 1];
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
					headers[i + 1] = new VirtualTreeColumnHeader(SamplePopulationBaseBranch.DeriveColumnName(roleCollection[i].Role));
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
			int? unaryRoleIndex = FactType.GetUnaryRoleIndex(roleCollection);
			int unaryRoleAdjust = 0;
			if (unaryRoleIndex.HasValue)
			{
				unaryRoleAdjust = unaryRoleIndex.Value;
				numColumns = 1;
			}
			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[numColumns + 1];
			headers[0] = CreateRowNumberColumn();
			for (int i = 0; i < numColumns; ++i)
			{
				headers[i + 1] = new VirtualTreeColumnHeader(SamplePopulationBaseBranch.DeriveColumnName(roleCollection[i + unaryRoleAdjust].Role));
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new SamplePopulationFactTypeBranch(mySelectedFactType, numColumns + 1, unaryRoleIndex);
			ConnectTree();
		}

		private VirtualTreeColumnHeader CreateRowNumberColumn()
		{
			return new VirtualTreeColumnHeader(string.Empty, 35, true);
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
					tree.DelayRedraw = false;
				}
				this.vtrSamplePopulation.Tree = null;
			}
		}

		private void ConnectTree()
		{
			if (myInEvents && myRepopulated)
			{
				return;
			}
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

		/// <summary>
		/// Deletes selected row of the branch
		/// </summary>
		public void DeleteSelectedSamplePopulationInstance()
		{
			VirtualTreeControl vtr = vtrSamplePopulation;
			ITree tree = vtr.Tree;
			if (tree != null)
			{
				int currentColumn = vtr.CurrentColumn;
				int currentRow = vtr.CurrentIndex;
				VirtualTreeItemInfo info = tree.GetItemInfo(currentRow, currentColumn, false);
				SamplePopulationBaseBranch baseBranch = info.Branch as SamplePopulationBaseBranch;
				if (null != baseBranch)
				{
					baseBranch.DeleteInstance(currentRow, currentColumn);
				}
			}
		}

		/// <summary>
		/// Begins an edit on the given cell, triggered by F2
		/// </summary>
		public void BeginEditSamplePopulationInstance()
		{
			Store store = null;
			string instanceTypeName = string.Empty;
			if(mySelectedEntityType != null)
			{
				store = mySelectedEntityType.Store;
				instanceTypeName = mySelectedEntityType.Name;
			}
			else if (mySelectedFactType != null)
			{
				store = mySelectedFactType.Store;
				instanceTypeName = mySelectedFactType.Name;
			}
			else if (mySelectedValueType != null)
			{
				store = mySelectedValueType.Store;
				instanceTypeName = mySelectedValueType.Name;
			}
			if (store != null)
			{
				using(Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, instanceTypeName)))
				{
					vtrSamplePopulation.BeginLabelEdit();
				}
			}
		}

		/// <summary>
		/// Returns a bool representing if the control is currently performing a full row select
		/// </summary>
		public bool FullRowSelect
		{
			get
			{
				return vtrSamplePopulation.MultiColumnHighlight;
			}
		}

		/// <summary>
		/// Attempts to fix a PopulationMandatoryError
		/// </summary>
		public void AutoCorrectMandatoryError(PopulationMandatoryError error)
		{
			ObjectTypeInstance instance = error.ObjectTypeInstance;
			MandatoryConstraint constraint = error.MandatoryConstraint as MandatoryConstraint;
			LinkedElementCollection<Role> roles = constraint.RoleCollection;
			// Should only handle simple mandatory constraints for now
			Debug.Assert(roles.Count == 1);
			FactTypeInstance nullInstance = null;
			using (Transaction t = instance.Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, roles[0].FactType.Name)))
			{
				SamplePopulationFactTypeBranch.ConnectInstance(ref nullInstance, instance, roles[0]);
				t.Commit();
			}
		}
		#endregion

		#region Model Events and Handler Methods
		#region Event Handler Attach/Detach Methods
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> so that the <see cref="ORMSamplePopulationToolWindow"/>
		/// contents can be updated to reflect any model changes.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			if (store == null || store.Disposed)
			{
				return;
			}
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo;

			// Track Currently Executing Events
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent), action);

			// Track FactTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleRemovedEvent), action);

			// Track EntityTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(EntityTypeHasPreferredIdentifierRolePlayerChangedEvent), action);

			classInfo = dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierRoleAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRoleRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(RolePlayerChangedEvent), action);

			// Track fact type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasFactType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemovedEvent), action);

			// Track object type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasObjectType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectTypeRemovedEvent), action);
		}
		#endregion
		#region Fact Type Instance Event Handlers
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
					tree.DelayRedraw = true;
				}
			}
		}

		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			if (myInEvents)
			{
				myInEvents = false;
				VirtualTreeControl treeControl = this.vtrSamplePopulation;
				ITree tree = treeControl.Tree;
				if (tree != null)
				{
					tree.DelayRedraw = false;
				}
				if (myRepopulated && treeControl.Visible)
				{
					ConnectTree();
				}
			}
		}

		private void FactTypeHasRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
			if (!factType.IsDeleted && factType == mySelectedFactType)
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
			if (!factType.IsDeleted && factType == mySelectedFactType)
			{
				PopulateControlForFactType();
			}
		}
		#endregion
		#region Entity Type Instance Event Handlers
		private void ProcessPreferredIdentifierEvent(EntityTypeHasPreferredIdentifier link, ObjectType entityType, UniquenessConstraint preferredIdentifier)
		{
			Debug.Assert(!myRepopulated);
			ObjectType selectedEntityType;
			FactType selectedFactType;
			if (entityType == null)
			{
				entityType = link.PreferredIdentifierFor;
			}
			if (null != (selectedEntityType = mySelectedEntityType))
			{
				if (!entityType.IsDeleted && entityType == selectedEntityType)
				{
					PopulateControlForEntityType();
				}
			}
			else if (null != (selectedFactType = mySelectedFactType))
			{
				if (preferredIdentifier == null)
				{
					preferredIdentifier = link.PreferredIdentifier;
				}
				Role identifierRole;
				Role identifiedRole;
				LinkedElementCollection<Role> roles;
				if (!preferredIdentifier.IsDeleted &&
					preferredIdentifier.IsInternal &&
					(roles = preferredIdentifier.RoleCollection).Count == 1 &&
					(identifierRole = roles[0]).FactType == selectedFactType &&
					null != (identifiedRole = identifierRole.OppositeRole as Role) &&
					entityType == identifiedRole.RolePlayer)
				{
					PopulateControlForFactType();
				}
			}
		}
		private void EntityTypeHasPreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			ProcessPreferredIdentifierEvent((EntityTypeHasPreferredIdentifier)e.ModelElement, null, null);
		}

		private void EntityTypeHasPreferredIdentifierRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			ProcessPreferredIdentifierEvent((EntityTypeHasPreferredIdentifier)e.ModelElement, null, null);
		}

		private void EntityTypeHasPreferredIdentifierRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			EntityTypeHasPreferredIdentifier link = (EntityTypeHasPreferredIdentifier)e.ElementLink;
			ObjectType entityType = null;
			UniquenessConstraint preferredIdentifier = null;
			if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				entityType = (ObjectType)e.OldRolePlayer;
			}
			else
			{
				preferredIdentifier = (UniquenessConstraint)e.OldRolePlayer;
			}
			ProcessPreferredIdentifierEvent(link, entityType, preferredIdentifier);
			if (!myRepopulated)
			{
				ProcessPreferredIdentifierEvent(link, null, null);
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

		private void RolePlayerChangedEvent(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			ConstraintRoleSequence sequence = e.SourceElement as ConstraintRoleSequence;
			ObjectType entityType = mySelectedEntityType;
			if (entityType != null && (entityType.PreferredIdentifier == sequence || RecurseIdentifierUpdate(entityType, sequence)))
			{
				PopulateControlForEntityType();
			}
		}
		private bool RecurseIdentifierUpdate(ObjectType objectType, ConstraintRoleSequence checkIdentifier)
		{
			if (!objectType.IsValueType)
			{
				UniquenessConstraint identifier = objectType.PreferredIdentifier;
				if (identifier != null)
				{
					if(identifier == checkIdentifier)
					{
						return true;
					}
					LinkedElementCollection<Role> roles = identifier.RoleCollection;
					int roleCount = roles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						if (RecurseIdentifierUpdate(roles[i].RolePlayer, checkIdentifier))
						{
							return true;
						}
					}
				}
			}
			return false;
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
			private bool myIsReadOnly;
			private bool myIgnoreEvents;
			private BranchModificationEventHandler myModificationEvents;
			#endregion // Member Variables
			#region Construction
			public SamplePopulationBaseBranch(int columnCount, Store store)
			{
				this.myColumnCount = columnCount;
				this.myStore = store;
			}
			#endregion // Construction
			#region CellEditContext class, used for label editing
			/// <summary>
			/// A helper class to provide a context for label editing
			/// </summary>
			protected sealed class CellEditContext
			{
				#region InstanceColumnDescriptor class
				/// <summary>
				/// A property descriptor to host inside the TypeEditorHost.
				/// Handles all in-place editing.
				/// </summary>
				private sealed class InstanceColumnDescriptor : PropertyDescriptor
				{
					#region InstanceDropDown class
					/// <summary>
					/// An ElementPicker list used to display available instance values
					/// </summary>
					private sealed class InstanceDropDown : ElementPicker<InstanceDropDown>
					{
						private IList myInstances;

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
								return ResourceStrings.ModelSamplePopulationEditorNullSelection;
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
								LinkedElementCollection<ObjectTypeInstance> instances = rolePlayer.ObjectTypeInstanceCollection;
								int instanceCount = instances.Count;
								string[] strings = new string[instanceCount];
								for (int i = 0; i < instanceCount; ++i)
								{
									strings[i] = instances[i].Name;
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
									if (currentInstance != null && stringValue == currentInstance.Name)
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
								return typedValue.Name;
							}
							return string.Empty;
						}
					}
					#endregion // InstanceConverter class
					#region Constructor
					public InstanceColumnDescriptor()
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
						return editorBaseType == typeof(UITypeEditor) ? new InstanceDropDown() : base.GetEditor(editorBaseType);
					}
					public sealed override object GetValue(object component)
					{
						return ((CellEditContext)component).myColumnInstance;
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
								if (context.myIsEntityTypeEditor)
								{
									EntityTypeInstance entityTypeInstance = context.myEntityTypeInstance;
									using (Transaction t = role.Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, role.Name)))
									{
										entityTypeInstance.FindRoleInstance(role).Delete();
										t.Commit();
									}
									// Removing the last role instance can remove the fact type instance, check
									if (entityTypeInstance.IsDeleted)
									{
										context.myEntityTypeInstance = null;
									}
								}
								else
								{
									FactTypeInstance factTypeInstance = context.myFactTypeInstance;
									using (Transaction t = role.Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, role.Name)))
									{
										factTypeInstance.FindRoleInstance(role).Delete();
										t.Commit();
									}
									// Removing the last role instance can remove the fact type instance, check
									if (factTypeInstance.IsDeleted)
									{
										context.myFactTypeInstance = null;
									}
								}
								context.myColumnInstance = null;
							}
						}
						else if (columnInstance != typedValue)
						{
							using (Transaction t = role.Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, role.Name)))
							{
								SamplePopulationEntityEditorBranch entityEditorBranch = context.myEditBranch;
								if(entityEditorBranch != null)
								{
									entityEditorBranch.ConnectInstance(typedValue, role);
								}
								else if(context.myIsEntityTypeEditor)
								{
									SamplePopulationEntityTypeBranch.ConnectInstance(context.myEntityType, ref context.myEntityTypeInstance, typedValue, role);
								}
								else
								{
									SamplePopulationFactTypeBranch.ConnectInstance(ref context.myFactTypeInstance, typedValue, role);
								}
								t.Commit();
							}
							context.myColumnInstance = typedValue;
						}
					}
					public sealed override Type ComponentType
					{
						get
						{
							return typeof(ObjectTypeInstance);
						}
					}
					public sealed override Type PropertyType
					{
						get
						{
							return typeof(ObjectTypeInstance);
						}
					}
					public sealed override bool IsReadOnly
					{
						get
						{
							return false;
						}
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
				#endregion // InstanceColumnDescriptor class
				#region Member Variables
				private static readonly InstanceColumnDescriptor Descriptor = new InstanceColumnDescriptor();
				private readonly SamplePopulationEntityEditorBranch myEditBranch;
				private readonly Role myRole;
				private readonly ObjectType myEntityType;
				private readonly bool myIsEntityTypeEditor;
				private EntityTypeInstance myEntityTypeInstance;
				private FactTypeInstance myFactTypeInstance;
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
					myIsEntityTypeEditor = true;
					if (entityTypeInstance != null)
					{
						EntityTypeRoleInstance roleInstance = entityTypeInstance.FindRoleInstance(role);
						if (roleInstance != null)
						{
							myColumnInstance = roleInstance.ObjectTypeInstance;
						}
					}
				}

				/// <summary>
				/// Create an editing context for the given entityType role and entityTypeInstance for a nested branch
				/// </summary>
				/// <param name="role">The role being edited</param>
				/// <param name="entityTypeInstance">The current entityTypeInstance. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				public CellEditContext(Role role, EntityTypeInstance entityTypeInstance, SamplePopulationEntityEditorBranch editBranch)
				{
					Debug.Assert(role != null);
					myRole = role;
					myEntityTypeInstance = entityTypeInstance;
					myEditBranch = editBranch;
					myIsEntityTypeEditor = true;
					if (entityTypeInstance != null)
					{
						EntityTypeRoleInstance roleInstance = entityTypeInstance.FindRoleInstance(role);
						if (roleInstance != null)
						{
							myColumnInstance = roleInstance.ObjectTypeInstance;
						}
					}
				}

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
					myIsEntityTypeEditor = false;
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
					TypeEditorHost host = OnScreenTypeEditorHost.Create(
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
			#region IBranch Interface Members
			/// <summary>
			/// Implements <see cref="IBranch.BeginLabelEdit"/>
			/// </summary>
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
			/// <summary>
			/// Implements <see cref="IBranch.CommitLabelEdit"/>
			/// </summary>
			protected static LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}
			/// <summary>
			/// Implements <see cref="IBranch.Features"/>
			/// </summary>
			protected BranchFeatures Features
			{
				get
				{
					const BranchFeatures features = BranchFeatures.ComplexColumns | BranchFeatures.Realigns | BranchFeatures.DefaultPositionTracking;
					return myIsReadOnly ? features : features | BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.InsertsAndDeletes;
				}
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return Features;
				}
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetAccessibilityData"/>
			/// </summary>
			protected static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return GetAccessibilityData(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetDisplayData"/>
			/// </summary>
			protected VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				if (myIsReadOnly)
				{
					retVal.State = VirtualTreeDisplayStates.GrayText;
				}
				else if (IsFullRowSelectColumn(column))
				{
					retVal.State = VirtualTreeDisplayStates.GrayText | VirtualTreeDisplayStates.TextAlignFar;
				}
				return retVal;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return GetDisplayData(row, column, requiredData);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetObject"/>
			/// </summary>
			protected static object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return null;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetText"/>
			/// </summary>
			protected string GetText(int row, int column)
			{
				if (IsFullRowSelectColumn(column))
				{
					// Returns row number
					return (row + 1).ToString();
				}
				else if (row == NewRowIndex && !IsReadOnly)
				{
					return null;
				}
				else
				{
					return string.Empty;
				}
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetTipText"/>
			/// </summary>
			protected static string GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return GetTipText(row, column, tipType);
			}
			/// <summary>
			/// Implements <see cref="IBranch.IsExpandable"/>
			/// </summary>
			protected bool IsExpandable(int row, int column)
			{
				return !IsFullRowSelectColumn(column);
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.LocateObject"/>
			/// </summary>
			protected static LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return new LocateObjectData();
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return LocateObject(obj, style, locateOptions);
			}
			/// <summary>
			/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> needed to listen on changes to a specific <see cref="IBranch"/> type.
			/// </summary>
			/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
			/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
			/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
			protected virtual void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
			{
			}

			private void ManageEventHandlers(Store store, EventHandlerAction action)
			{
				if (store == null || store.Disposed)
				{
					return; // bail out
				}
				this.ManageEventHandlers(store, ModelingEventManager.GetModelingEventManager(store), action);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnBranchModification"/>
			/// </summary>
			protected event BranchModificationEventHandler OnBranchModification
			{
				add
				{
					bool attachEvents = myModificationEvents == null;
					myModificationEvents += value;
					if (attachEvents)
					{
						this.ManageEventHandlers(myStore, EventHandlerAction.Add);
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
							this.ManageEventHandlers(store, EventHandlerAction.Remove);
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
			/// <summary>
			/// Implements <see cref="IBranch.OnDragEvent"/>
			/// </summary>
			protected static void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
				OnDragEvent(sender, row, column, eventType, args);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnGiveFeedback"/>
			/// </summary>
			protected static void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
				OnGiveFeedback(args, row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnQueryContinueDrag"/>
			/// </summary>
			protected static void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
				OnQueryContinueDrag(args, row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnStartDrag"/>
			/// </summary>
			protected static VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return OnStartDrag(sender, row, column, reason);
			}
			/// <summary>
			/// Implements <see cref="IBranch.ToggleState"/>
			/// </summary>
			protected static StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return ToggleState(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.SynchronizeState"/>
			/// </summary>
			protected static StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
			}
			/// <summary>
			/// Implements <see cref="IBranch.UpdateCounter"/>
			/// </summary>
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
			/// <summary>
			/// Implements <see cref="IBranch.VisibleItemCount"/>
			/// </summary>
			protected static int VisibleItemCount
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
			#endregion // IBranch Interface Members
			#region IMultiColumnBranch Interface Members
			/// <summary>
			/// Implements <see cref="IMultiColumnBranch.ColumnCount"/>
			/// </summary>
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
			/// <summary>
			/// Implements <see cref="IMultiColumnBranch.ColumnStyles"/>
			/// </summary>
			protected static SubItemCellStyles ColumnStyles(int column)
			{
				return SubItemCellStyles.Simple;
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}
			/// <summary>
			/// Impelements <see cref="IMultiColumnBranch.GetJaggedColumnCount"/>
			/// </summary>
			protected int GetJaggedColumnCount(int row)
			{
				return ColumnCount;
			}
			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				return GetJaggedColumnCount(row);
			}
			#endregion // IMultiColumnBranch Interface Members
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

			/// <summary>
			/// Whether or not the branch is read only
			/// </summary>
			protected bool IsReadOnly
			{
				get
				{
					return myIsReadOnly;
				}
				set
				{
					myIsReadOnly = value;
				}
			}

			/// <summary>
			/// Whether or not the branch is newly repopulated and should ignore events.
			/// Managed by the outer editor when it is repopulated.
			/// </summary>
			public bool IgnoreEvents
			{
				get
				{
					return myIgnoreEvents;
				}
				set
				{
					// Note that this could also be done by explicitly disconnecting
					// events, but this is a relatively expensive process. We may
					// reconsider this in the future, as well as the possibility of
					// simply not reconnecting the new branch until events are completed.
					myIgnoreEvents = value;
				}
			}
			#endregion // Accessor Properties
			#region Helper Methods
			public static string DeriveColumnName(Role role)
			{
				StringBuilder outputText = null;
				string retVal = (role == null || role.RolePlayer == null) ? ResourceStrings.ModelSamplePopulationEditorNullSelection : RecurseColumnIdentifier(role, null, ref outputText);
				return (outputText != null) ? outputText.ToString() : retVal;
			}

			// UNDONE: This whole method needs to be localized
			private static string RecurseColumnIdentifier(Role role, string listSeparator, ref StringBuilder outputText)
			{
				ObjectType rolePlayer = role.RolePlayer;
				if (rolePlayer == null)
				{
					if (outputText != null)
					{
						outputText.Append(" ");
					}
					return " ";
				}
				string derivedName = (role.Name.Length != 0) ? role.Name : rolePlayer.Name;
				if (rolePlayer.IsValueType)
				{
					if (outputText != null)
					{
						outputText.Append(derivedName);
						return null;
					}
					return derivedName;
				}
				else
				{
					UniquenessConstraint identifier = rolePlayer.PreferredIdentifier;
					if (identifier == null)
					{
						if (outputText != null)
						{
							outputText.Append(derivedName);
							return null;
						}
						return derivedName;
					}
					LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
					int identifierCount = identifierRoles.Count;
					if (outputText == null)
					{
						outputText = new StringBuilder();
					}
					outputText.Append(derivedName);
					outputText.Append(" (");
					if (listSeparator == null)
					{
						listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
					}
					string refModeString;
					if (identifier.IsInternal &&
						identifierCount == 1 &&
						!string.IsNullOrEmpty((refModeString = rolePlayer.ReferenceModeDecoratedString)))
					{
						outputText.Append(refModeString);
					}
					else
					{
						for (int i = 0; i < identifierCount; ++i)
						{
							Role identifierRole = identifierRoles[i];
							if (i != 0)
							{
								outputText.Append(listSeparator);
							}
							RecurseColumnIdentifier(identifierRole, listSeparator, ref outputText);
						}
					}
					outputText.Append(")");
					return null;
				}
			}

			/// <summary>
			/// Return true if a selection for the specified column in this
			/// branch should select the full row
			/// </summary>
			public virtual bool IsFullRowSelectColumn(int column)
			{
				return column == (int)SpecialColumnIndex.FullRowSelectColumn;
			}

			protected static ObjectTypeInstance RecurseValueTypeInstance(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string newText, ref ValueTypeInstance rootInstance, bool create)
			{
				if (parentType.IsValueType)
				{
					DataType valueDataType = parentType.DataType;
					if (create && valueDataType.CanCompare)
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
					else if (!create)
					{
						return objectTypeInstance;
					}
					Debug.Assert(parentType.Store.TransactionActive, "Transaction must be active to create new instances");
					ValueTypeInstance editValueTypeInstance = new ValueTypeInstance(parentType.Store);
					editValueTypeInstance.ValueType = parentType;
					editValueTypeInstance.Value = newText;
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
					if (editEntityTypeInstance != null)
					{
						editingRoleInstance = editEntityTypeInstance.RoleInstanceCollection[0];
					}
					ObjectTypeInstance objectInstance;
					if (editingRoleInstance == null)
					{
						objectInstance = RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref rootInstance, create);
					}
					else
					{
						objectInstance = RecurseValueTypeInstance(editingRoleInstance.ObjectTypeInstance, identifierRole.RolePlayer, newText, ref rootInstance, create);
					}
					LinkedElementCollection<EntityTypeInstance> instances = parentType.EntityTypeInstanceCollection;
					int instanceCount = instances.Count;
					for(int i = 0; i < instanceCount; ++i)
					{
						if(instances[i].RoleInstanceCollection[0].ObjectTypeInstance == objectInstance)
						{
							return instances[i];
						}
					}
					if (create)
					{
						editEntityTypeInstance = new EntityTypeInstance(parentType.Store);
						editEntityTypeInstance.EntityType = parentType;
						EntityTypeRoleInstance identifierInstance = new EntityTypeRoleInstance(identifierRole, objectInstance);
						identifierInstance.EntityTypeInstance = editEntityTypeInstance;
					}
					return editEntityTypeInstance;
				}
			}
			#endregion // Helper Methods
			#region Branch Update Methods
			protected void AddInstanceDisplay(int location)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, location - 1, 1));
					// UNDONE: If an add occurs due to an item being added in the UI, then in some cases we want
					// to go with the item (multi-column instances), and in others we want to stay on the last row (single column)
					//myModificationEvents(this, BranchModificationEventArgs.MoveItem(this, location - 1, location));
				}
			}

			protected void EditInstanceDisplay(int location)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, location, -1, 1)));
					// Note: These are turned on to trigger the delayed redraw. DisplayDataChanged is not enough.
					myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
					myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
				}
			}

			protected void RemoveInstanceDisplay(int location)
			{
				if (myModificationEvents != null)
				{
					int columnCount = myColumnCount;
					ITree parentTree = TreeControl.Tree;
					for (int i = 1; i < myColumnCount; ++i)
					{
						if((this as IMultiColumnBranch).ColumnStyles(i) == SubItemCellStyles.Mixed && (this as IBranch).IsExpandable(location, i))
						{
							myModificationEvents(this, BranchModificationEventArgs.UpdateCellStyle(this, location, i, false)); 
						}
					}
					myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, location, 1));
				}
			}

			protected void EditColumnHeader(int column, Role newRole)
			{
				TreeControl.UpdateColumnHeaderAppearance(column, DeriveColumnName(newRole), VirtualTreeColumnHeaderStyles.Default, -1);
			}

			protected void EditColumnDisplay(int column)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, 0, column, (this as IBranch).VisibleItemCount)));
					// Note: These are turned on to trigger the delayed redraw. DisplayDataChanged is not enough.
					myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
					myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
				}
			}

			/// <summary>
			/// Remove existing items from the branch and add new items based on the current VisibleItemCount.
			/// </summary>
			/// <param name="oldItemCount">The number of items previously in the branch</param>
			protected void Repopulate(int oldItemCount)
			{
				if (myModificationEvents != null)
				{
					if (oldItemCount != 0)
					{
						myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, 0, oldItemCount));
					}
					int newCount = (this as IBranch).VisibleItemCount;
					if (newCount != 0)
					{
						myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, -1, newCount));
					}
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
				if (editInstance.Value != newText)
				{
					editInstance.Value = newText;
				}
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

			/// <summary>
			/// Calculates and returns the new selection point in terms of the branch
			/// </summary>
			/// <param name="currentCol"></param>
			/// <param name="currentRow"></param>
			/// <returns></returns>
			public virtual Point MoveSelectionForward(int currentCol, int currentRow)
			{
				int numCol = (this as IMultiColumnBranch).ColumnCount;
				int numRow = (this as IBranch).VisibleItemCount;
				if(currentCol < numCol - 1)
				{
					++currentCol;
				}
				else if(currentRow < numRow - 1)
				{
					++currentRow;
					currentCol = 0;
				}
				return new Point(currentCol, currentRow);
			}

			// Remove the instance at the given row
			public abstract void DeleteInstance(int row, int column);
			#endregion // Branch Update Methods
		}
		private sealed class SamplePopulationValueTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly List<ValueTypeInstance> myCachedInstances;
			private readonly ObjectType myValueType;
			#endregion
			#region Construction
			// Value Type Branches will always have 1 column, plus the full row select column
			public SamplePopulationValueTypeBranch(ObjectType selectedValueType) : base(2, selectedValueType.Store)
			{
				Debug.Assert(selectedValueType.IsValueType);
				myValueType = selectedValueType;
				myCachedInstances = new List<ValueTypeInstance>();
				myCachedInstances.AddRange(selectedValueType.ValueTypeInstanceCollection);
			}
			#endregion
			#region IBranch Interface Members
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

			/// <summary>
			/// Set up immediate label edits
			/// </summary>
			BranchFeatures IBranch.Features
			{
				get
				{
					return (base.Features & (~BranchFeatures.ComplexColumns)) | BranchFeatures.ImmediateSelectionLabelEdits;
				}
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (row == NewRowIndex)
				{
					return null;
				}
				return myCachedInstances[row];
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text != null && text.Length == 0)
				{
					text = myCachedInstances[row].Value;
				}
				return text;
			}
			private new int VisibleItemCount
			{
				get
				{
					return myCachedInstances.Count + SamplePopulationBaseBranch.VisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion // IBranch Interface Members
			#region Event Handlers
			protected sealed override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				DomainClassInfo classInfo;
				
				classInfo = dataDirectory.FindDomainRelationship(ValueTypeHasValueTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasValueTypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasValueTypeInstanceDeletedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ValueTypeInstance.ValueDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValueTypeInstanceValueChangedEvent), action);
			}
			private void ValueTypeHasValueTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
				ObjectType valueType = link.ValueType;
				if (!valueType.IsDeleted && valueType == myValueType)
				{
					myCachedInstances.Add(link.ValueTypeInstance);
					base.AddInstanceDisplay(VisibleItemCount - 1);
				}
			}

			private void ValueTypeHasValueTypeInstanceDeletedEvent(object sender, ElementDeletedEventArgs e)
			{
				ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
				ObjectType valueType = link.ValueType;
				if (!valueType.IsDeleted && valueType == myValueType)
				{
					List<ValueTypeInstance> instances = myCachedInstances;
					int instanceLocation = instances.IndexOf(link.ValueTypeInstance);
					instances.RemoveAt(instanceLocation);
					base.RemoveInstanceDisplay(instanceLocation);
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
			#endregion // Event Handlers
			public sealed override void DeleteInstance(int row, int column)
			{
				if(base.IsFullRowSelectColumn(column) && row < myCachedInstances.Count)
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, myValueType.Name)))
					{
						myValueType.ValueTypeInstanceCollection[row].Delete();
						t.Commit();
					}
				}
			}
		}
		private sealed class SamplePopulationEntityTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly List<EntityTypeInstance> myCachedInstances;
			private readonly ObjectType myEntityType;
			#endregion
			#region Construction
			public SamplePopulationEntityTypeBranch(ObjectType entityType, int numColumns)
				: base(numColumns, entityType.Store)
			{
				Debug.Assert(!entityType.IsValueType);
				myEntityType = entityType;
				myCachedInstances = new List<EntityTypeInstance>();
				myCachedInstances.AddRange(entityType.EntityTypeInstanceCollection);
			}
			#endregion
			#region IBranch Interface Members
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
				{
					ObjectType entityType = myEntityType;
					ObjectType columnRolePlayer = entityType.PreferredIdentifier.RoleCollection[column - 1].RolePlayer;
					if (columnRolePlayer != null && (columnRolePlayer.IsValueType || columnRolePlayer.PreferredIdentifier != null))
					{
						LinkedElementCollection<EntityTypeInstance> instances = entityType.EntityTypeInstanceCollection;
						retVal.CustomInPlaceEdit = new CellEditContext(entityType, entityType.PreferredIdentifier.RoleCollection[column - 1], (row < instances.Count) ? instances[row] : null).CreateInPlaceEditControl();
						retVal.CustomCommit = delegate(VirtualTreeItemInfo itemInfo, Control editControl)
						{
							// Defer to the normal text edit if the control is not dirty
							return (editControl as IVirtualTreeInPlaceControl).Dirty ? itemInfo.Branch.CommitLabelEdit(itemInfo.Row, itemInfo.Column, editControl.Text) : LabelEditResult.CancelEdit;
						};
					}
					else
					{
						retVal = VirtualTreeLabelEditData.Invalid;
					}
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
					EntityTypeInstance editInstance = myCachedInstances[row];
					Role factRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					EntityTypeRoleInstance editRoleInstance = editInstance.FindRoleInstance(factRole);
					// If editing an existing EntityTypeRoleInstance
					if (editRoleInstance != null)
					{
						if (delete)
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, "")))
							{
								editRoleInstance.Delete();
								t.Commit();
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, "")))
							{
								ValueTypeInstance instance = null;
								ObjectTypeInstance objectInstance = editRoleInstance.ObjectTypeInstance;
								ValueTypeInstance valueInstance = objectInstance as ValueTypeInstance;
								if(valueInstance != null)
								{
									EditValueTypeInstance(valueInstance, newText);
								}
								else
								{
									ObjectTypeInstance result = RecurseValueTypeInstance(objectInstance, editRoleInstance.Role.RolePlayer, newText, ref instance, true);
									ConnectInstance(myEntityType, ref editInstance, result, factRole);
									editRoleInstance.Delete();
								}
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
							ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance, true);
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
						ObjectTypeInstance result = RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref instance, true);
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
					List<EntityTypeInstance> instances = myCachedInstances;
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
					text = ObjectTypeInstance.GetDisplayString(null, myEntityType.PreferredIdentifier.RoleCollection[column - 1].RolePlayer);
				}
				else if (text.Length == 0)
				{
					ObjectType selectedEntityType = myEntityType;
					EntityTypeInstance selectedInstance = myCachedInstances[row];
					LinkedElementCollection<EntityTypeRoleInstance> entityTypeRoleInstances = selectedInstance.RoleInstanceCollection;
					int roleInstanceCount = entityTypeRoleInstances.Count;
					EntityTypeRoleInstance roleInstance;
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						if (identifierRole == (roleInstance = entityTypeRoleInstances[i]).Role)
						{
							text = roleInstance.ObjectTypeInstance.Name;
							return text;
						}
					}
					text = ObjectTypeInstance.GetDisplayString(null, identifierRole.RolePlayer);
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
			private new int VisibleItemCount
			{
				get
				{
					return myCachedInstances.Count + SamplePopulationBaseBranch.VisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion // IBranch Interface Members
			#region IMultiColumnBranch Interface Members
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				switch (column)
				{
					case 0:
						return SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Mixed;
				}
			}
			#endregion // IMultiColumnBranch Interface Members
			#region Event Handlers
			protected sealed override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				DomainClassInfo classInfo;

				// Track EntityTypeInstance changes
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierRoleAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRoleRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasEntityTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityTypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasEntityTypeInstanceRemovedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeInstanceNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
				propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);
			}
			private void EntityTypeHasPreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType entityType = link.PreferredIdentifierFor;
				if (entityType != null && !entityType.IsDeleted)
				{
					UniquenessConstraint currentPreferredIdentifier = myEntityType.PreferredIdentifier;
					if (currentPreferredIdentifier != null)
					{
						LinkedElementCollection<Role> identifierRoles = currentPreferredIdentifier.RoleCollection;
						int identifierCount = identifierRoles.Count;
						for (int i = 0; i < identifierCount; ++i)
						{
							if (entityType == identifierRoles[i].RolePlayer)
							{
								EditColumnHeader(i + 1, identifierRoles[i]);
								EditColumnDisplay(i + 1);
							}
						}
					}
				}
			}

			private void EntityTypeHasPreferredIdentifierRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType entityType = link.PreferredIdentifierFor;
				if (entityType != null && !entityType.IsDeleted)
				{
					UniquenessConstraint identifier = myEntityType.PreferredIdentifier;
					if (identifier != null)
					{
						LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
						int identifierCount = identifierRoles.Count;
						for (int i = 0; i < identifierCount; ++i)
						{
							if (entityType == identifierRoles[i].RolePlayer)
							{
								base.EditColumnHeader(i + 1, identifierRoles[i]);
								base.EditColumnDisplay(i + 1);
							}
						}
					}
				}
			}

			private void EntityTypeHasPreferredIdentifierRoleAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					!uniqueness.IsDeleted &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor))
				{
					UniquenessConstraint identifier = myEntityType.PreferredIdentifier;
					if (identifier != null)
					{
						LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
						int identifierCount = identifierRoles.Count;
						for (int i = 0; i < identifierCount; ++i)
						{
							if (preferredFor == identifierRoles[i].RolePlayer)
							{
								base.EditColumnHeader(i + 1, identifierRoles[i]);
								base.EditColumnDisplay(i + 1);
							}
						}
					}
				}
			}

			private void EntityTypeHasPreferredIdentifierRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					!uniqueness.IsDeleted &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor))
				{
					UniquenessConstraint identifier = myEntityType.PreferredIdentifier;
					if (identifier != null)
					{
						LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
						int identifierCount = identifierRoles.Count;
						for (int i = 0; i < identifierCount; ++i)
						{
							if (preferredFor == identifierRoles[i].RolePlayer)
							{
								base.EditColumnHeader(i + 1, identifierRoles[i]);
								base.EditColumnDisplay(i + 1);
							}
						}
					}
				}
			}

			private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entityType = link.EntityType;
				if (entityType != null && !entityType.IsDeleted && entityType == myEntityType && entityType.PreferredIdentifier != null)
				{
					myCachedInstances.Add(link.EntityTypeInstance);
					AddEntityInstanceDisplay(link.EntityTypeInstance);
				}
			}

			private void EntityTypeHasEntityTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entityType = link.EntityType;
				if (entityType != null && !entityType.IsDeleted && entityType == myEntityType && entityType.PreferredIdentifier != null)
				{
					List<EntityTypeInstance> instances = myCachedInstances;
					int instanceLocation = instances.IndexOf(link.EntityTypeInstance);
					Debug.Assert(instanceLocation != -1);
					instances.RemoveAt(instanceLocation);
					base.RemoveInstanceDisplay(instanceLocation);
				}
			}

			private bool RecurseInstanceUpdate(ObjectTypeInstance checkInstance, ObjectTypeInstance compareInstance)
			{
				EntityTypeInstance entityInstance;
				ValueTypeInstance valueInstance;
				if (null != (entityInstance = checkInstance as EntityTypeInstance))
				{
					if (entityInstance == compareInstance)
					{
						return true;
					}
					else
					{
						LinkedElementCollection<EntityTypeRoleInstance> roleInstances = entityInstance.RoleInstanceCollection;
						int roleInstancesCount = roleInstances.Count;
						for (int i = 0; i < roleInstancesCount; ++i)
						{
							bool result = RecurseInstanceUpdate(roleInstances[i].ObjectTypeInstance, compareInstance);
							if (result)
							{
								return result;
							}
						}
					}
				}
				else if (null != (valueInstance = checkInstance as ValueTypeInstance))
				{
					if (valueInstance == compareInstance)
					{
						return true;
					}
				}
				return false;
			}

			private void ObjectTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectTypeInstance objectTypeInstance = e.ModelElement as ObjectTypeInstance;
				EntityTypeInstance entityInstance;
				if (null != (entityInstance = objectTypeInstance as EntityTypeInstance))
				{
					ObjectType entityType = entityInstance.EntityType;
					if (entityType == myEntityType)
					{
						EditEntityInstanceDisplay(entityInstance);
					}
				}
				else
				{
					List<EntityTypeInstance> instances = myCachedInstances;
					int instanceCount = instances.Count;
					for (int i = 0; i < instanceCount; ++i)
					{
						if (RecurseInstanceUpdate(instances[i], objectTypeInstance))
						{
							EditInstanceDisplay(i);
						}
					}
				}
			}

			private void RoleNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				Role role = e.ModelElement as Role;
				UniquenessConstraint identifier = myEntityType.PreferredIdentifier;
				if (identifier != null && !role.IsDeleted)
				{
					int location = identifier.RoleCollection.IndexOf(role);
					if (location != -1)
					{
						base.EditColumnHeader(location + 1, role);
					}
				}
			}

			private void ObjectTypeNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectType objectType = e.ModelElement as ObjectType;
				UniquenessConstraint identifier = myEntityType.PreferredIdentifier;
				if (identifier != null && !objectType.IsDeleted)
				{
					LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
					int collectionCount = identifierRoles.Count;
					for (int i = 0; i < collectionCount; ++i)
					{
						if (identifierRoles[i].RolePlayer == objectType)
						{
							base.EditColumnHeader(i + 1, identifierRoles[i]);
						}
					}
				}
			}
			#endregion // Event Handlers
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
			#endregion // Helper Methods
			#region Branch Update Methods
			private void AddEntityInstanceDisplay(EntityTypeInstance entityTypeInstance)
			{
				int location = myCachedInstances.IndexOf(entityTypeInstance);

				if (location != -1)
				{
					base.EditInstanceDisplay(location);
					base.AddInstanceDisplay(myCachedInstances.Count);
				}
			}

			private void EditEntityInstanceDisplay(EntityTypeInstance entityTypeInstance)
			{
				int location = myCachedInstances.IndexOf(entityTypeInstance);
				if (location != -1)
				{
					base.EditInstanceDisplay(location);
				}
			}
			#endregion // Branch Update Methods
			public sealed override void DeleteInstance(int row, int column)
			{
				if (base.IsFullRowSelectColumn(column) && row < myCachedInstances.Count)
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, myEntityType.Name)))
					{
						myEntityType.EntityTypeInstanceCollection[row].Delete();
						t.Commit();
					}
				}
			}
		}
		private sealed class SamplePopulationFactTypeBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly FactType myFactType;
			private List<FactTypeInstance> myCachedFactTypeInstances;
			private List<EntityTypeInstance> myCachedEntityTypeInstances;
			private ObjectType myProxyObjectType;
			private int myUnaryColumn;
			private bool myHasUnaryColumn;
			#endregion
			#region Construction
			public SamplePopulationFactTypeBranch(FactType selectedFactType, int numColumns, int? unaryColumnAdjustment)
				: base(numColumns, selectedFactType.Store)
			{
				myFactType = selectedFactType;
				ValidateReadOnlyProxyObject();
				ObjectType proxyObjectType = myProxyObjectType;
				if (proxyObjectType != null)
				{
					myCachedEntityTypeInstances = new List<EntityTypeInstance>(proxyObjectType.EntityTypeInstanceCollection);
				}
				else
				{
					myCachedFactTypeInstances = new List<FactTypeInstance>(selectedFactType.FactTypeInstanceCollection);
				}
				if (unaryColumnAdjustment.HasValue)
				{
					myHasUnaryColumn = true;
					myUnaryColumn = unaryColumnAdjustment.Value;
				}
			}
			#endregion
			#region IBranch Interface Members
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
				{
					Role role = myFactType.RoleCollection[column + myUnaryColumn - 1].Role;
					ObjectType rolePlayer = role.RolePlayer;
					if (rolePlayer == null || (!rolePlayer.IsValueType && rolePlayer.PreferredIdentifier == null))
					{
						retVal = VirtualTreeLabelEditData.Invalid;
					}
					else
					{
						List<FactTypeInstance> instances = myCachedFactTypeInstances;
						retVal.CustomInPlaceEdit = new CellEditContext(role, (row < instances.Count) ? instances[row] : null).CreateInPlaceEditControl();
						retVal.CustomCommit = delegate(VirtualTreeItemInfo itemInfo, Control editControl)
						{
							// Defer to the normal text edit if the control is not dirty
							return (editControl as IVirtualTreeInPlaceControl).Dirty ? itemInfo.Branch.CommitLabelEdit(itemInfo.Row, itemInfo.Column, editControl.Text) : LabelEditResult.CancelEdit;
						};
					}
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
					FactTypeInstance editInstance = myCachedFactTypeInstances[row];
					FactTypeRoleInstance editRoleInstance = null;
					LinkedElementCollection<FactTypeRoleInstance> roleInstances = editInstance.RoleInstanceCollection;
					int instanceCount = roleInstances.Count;
					Role factRole = selectedFactType.RoleCollection[column + myUnaryColumn - 1].Role;
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
						if (delete)
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, "")))
							{
								if (instance != null)
								{
									RemoveValueTypeInstance(instance);
								}
								else
								{
									ObjectTypeInstance result = RecurseValueTypeInstance(editRoleInstance.ObjectTypeInstance, editRoleInstance.Role.RolePlayer, newText, ref instance, false);
									result.Delete();
								}
								t.Commit();
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, "")))
							{
								editRoleInstance.Delete();
								ObjectTypeInstance result = RecurseValueTypeInstance(editRoleInstance.ObjectTypeInstance, editRoleInstance.Role.RolePlayer, newText, ref instance, true);
								ConnectInstance(ref editInstance, result, factRole);
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					// If editing an existing FactTypeInstance but creating a new FactTypeRoleInstance
					else if (!delete)
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedFactType.Name)))
						{
							ValueTypeInstance instance = null;
							ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance, true);
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
						Role factRole = selectedFactType.RoleCollection[column + myUnaryColumn - 1].Role;
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance, true);
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
					if (IsReadOnly)
					{
						ObjectType selectedEntityType = myProxyObjectType;
						List<EntityTypeInstance> instances = myCachedEntityTypeInstances;
						Role identifierRole = myFactType.RoleCollection[column + myUnaryColumn - 1].Role;
						EntityTypeInstance parentInstance = instances[row];
						EntityTypeInstance editInstance = null;
						if (identifierRole.RolePlayer == selectedEntityType)
						{
							editInstance = parentInstance;
						}
						else
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
					else
					{
						FactType selectedFactType = myFactType;
						List<FactTypeInstance> instances = myCachedFactTypeInstances;
						Role selectedRole = selectedFactType.RoleCollection[column + myUnaryColumn - 1].Role;
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
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null)
				{
					text = ObjectTypeInstance.GetDisplayString(null, myFactType.RoleCollection[column + myUnaryColumn - 1].Role.RolePlayer);
				}
				else if (text.Length == 0)
				{
					if (IsReadOnly)
					{
						Role selectedRole = myFactType.RoleCollection[column + myUnaryColumn - 1].Role;
						ObjectType rolePlayer = selectedRole.RolePlayer;
						ObjectTypeInstance instance = null;
						if (myProxyObjectType != null && rolePlayer == myProxyObjectType)
						{
							instance = myCachedEntityTypeInstances[row];
						}
						else
						{
							if (myProxyObjectType != null)
							{
								LinkedElementCollection<EntityTypeRoleInstance> roleInstances = myCachedEntityTypeInstances[row].RoleInstanceCollection;
								int roleInstanceCount = roleInstances.Count;
								for (int i = 0; i < roleInstanceCount; ++i)
								{
									if (roleInstances[i].Role == selectedRole)
									{
										instance = roleInstances[i].ObjectTypeInstance;
										break;
									}
								}
							}
						}
						text = (instance == null) ? "" : instance.Name;
					}
					else
					{
						FactTypeInstance factTypeInstance = myCachedFactTypeInstances[row];
						LinkedElementCollection<FactTypeRoleInstance> factTypeRoleInstances = factTypeInstance.RoleInstanceCollection;
						int roleInstanceCount = factTypeRoleInstances.Count;
						FactTypeRoleInstance instance;
						Role factTypeRole = myFactType.RoleCollection[column + myUnaryColumn - 1].Role;
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							if (factTypeRole == (instance = factTypeRoleInstances[i]).Role)
							{
								return instance.ObjectTypeInstance.Name;
							}
						}
						text = ObjectTypeInstance.GetDisplayString(null, factTypeRole.RolePlayer);
					}
				}
				return text;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				if (!IsReadOnly && !base.IsFullRowSelectColumn(column))
				{
					Role factRole = myFactType.RoleCollection[column + myUnaryColumn - 1].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					if (rolePlayer != null)
					{
						UniquenessConstraint roleIdentifier = rolePlayer.PreferredIdentifier;
						return (roleIdentifier != null) && (roleIdentifier.RoleCollection.Count > 1);
					}
				}
				return false;
			}
			private new int VisibleItemCount
			{
				get
				{
					if (IsReadOnly)
					{
						if (myProxyObjectType != null)
						{
							return myCachedEntityTypeInstances.Count;
						}
					}
					else
					{
						return myCachedFactTypeInstances.Count + SamplePopulationBaseBranch.VisibleItemCount;
					}
					return 0;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion // IBranch Interface Members
			#region IMultiColumnBranch Interface Members
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				switch (column)
				{
					case 0:
						return SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Mixed;
				}
			}
			#endregion // IMultiColumnBranch Interface Members
			#region Event Handlers
			protected sealed override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				DomainClassInfo classInfo;

				// Track EntityTypeInstance changes
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasEntityTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityTypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasEntityTypeInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierRoleAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRoleRemovedEvent), action);

				// Track FactTypeInstance changes
				classInfo = dataDirectory.FindDomainRelationship(FactTypeHasFactTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasFactTypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasFactTypeInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeInstanceHasRoleInstanceRemovedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ObjectTypePlaysRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectTypePlaysRoleAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectTypePlaysRoleRemovedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectTypeRolePlayerChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeInstanceNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
				propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);
			}

			private void ObjectTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectTypeInstance objectTypeInstance = e.ModelElement as ObjectTypeInstance;
				if (IsReadOnly)
				{
					List<EntityTypeInstance> instances = myCachedEntityTypeInstances;
					int instanceCount = instances.Count;
					for (int i = 0; i < instanceCount; ++i)
					{
						LinkedElementCollection<EntityTypeRoleInstance> entityTypeRoleInstances = instances[i].RoleInstanceCollection;
						int entityTypeRoleInstanceCount = entityTypeRoleInstances.Count;
						for (int j = 0; j < entityTypeRoleInstanceCount; ++j)
						{
							if (entityTypeRoleInstances[j].ObjectTypeInstance == objectTypeInstance)
							{
								EditInstanceDisplay(i);
								break;
							}
						}
					}
				}
				else
				{
					List<FactTypeInstance> instances = myCachedFactTypeInstances;
					int instanceCount = instances.Count;
					for (int i = 0; i < instanceCount; ++i)
					{
						LinkedElementCollection<FactTypeRoleInstance> factTypeRoleInstances = instances[i].RoleInstanceCollection;
						int factTypeRoleInstanceCount = factTypeRoleInstances.Count;
						for (int j = 0; j < factTypeRoleInstanceCount; ++j)
						{
							if (factTypeRoleInstances[j].ObjectTypeInstance == objectTypeInstance)
							{
								EditInstanceDisplay(i);
								break;
							}
						}
					}
				}
			}

			private void EntityTypeHasPreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType preferredFor = link.PreferredIdentifierFor;
				if (preferredFor != null && !preferredFor.IsDeleted)
				{
					UpdateColumnHeadersForObjectType(preferredFor);
				}
				if (!IsReadOnly)
				{
					ValidateReadOnlyProxyObject();
				}
			}

			private void EntityTypeHasPreferredIdentifierRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType preferredFor = link.PreferredIdentifierFor;
				if (preferredFor != null && !preferredFor.IsDeleted)
				{
					UpdateColumnHeadersForObjectType(preferredFor);
				}
				if (IsReadOnly)
				{
					ValidateReadOnlyProxyObject();
				}
			}

			private void EntityTypeHasPreferredIdentifierRoleAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					!uniqueness.IsDeleted &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor))
				{
					UpdateColumnHeadersForObjectType(preferredFor);
				}
				if (!IsReadOnly)
				{
					Role selectedRole = link.Role;
					if (selectedRole.FactType == myFactType)
					{
						ValidateReadOnlyProxyObject();
					}
				}
			}

			private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entityType = link.EntityType;
				if (entityType != null && !entityType.IsDeleted && entityType == myProxyObjectType && entityType.PreferredIdentifier != null)
				{
					List<EntityTypeInstance> instances = myCachedEntityTypeInstances;
					instances.Add(link.EntityTypeInstance);
					base.AddInstanceDisplay(instances.Count - 1);
				}
			}

			private void EntityTypeHasEntityTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entityType = link.EntityType;
				if (entityType != null && !entityType.IsDeleted && entityType == myProxyObjectType && entityType.PreferredIdentifier != null)
				{
					List<EntityTypeInstance> instances = myCachedEntityTypeInstances;
					int instanceLocation = instances.IndexOf(link.EntityTypeInstance);
					Debug.Assert(instanceLocation != -1);
					instances.RemoveAt(instanceLocation);
					base.RemoveInstanceDisplay(instanceLocation);
				}
			}

			private void EntityTypeHasPreferredIdentifierRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					!uniqueness.IsDeleted &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor))
				{
					UpdateColumnHeadersForObjectType(preferredFor);
				}
				if (IsReadOnly)
				{
					ValidateReadOnlyProxyObject();
				}
			}

			private void FactTypeHasFactTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
				FactType factType = link.FactType;
				List<FactTypeInstance> instances;
				if (!factType.IsDeleted &&
					factType == myFactType &&
					null != (instances = myCachedFactTypeInstances))
				{
					FactTypeInstance factTypeInstance = link.FactTypeInstance;
					instances.Add(factTypeInstance);
					int instanceCount = instances.Count;
					if (instanceCount > 1)
					{
						base.EditInstanceDisplay(instanceCount - 1);
					}
					base.AddInstanceDisplay(instanceCount);
				}
			}

			private void FactTypeHasFactTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
				FactType factType = link.FactType;
				List<FactTypeInstance> instances;
				if (!factType.IsDeleted &&
					factType == myFactType &&
					null != (instances = myCachedFactTypeInstances))
				{
					int instanceLocation = instances.IndexOf(link.FactTypeInstance);
					Debug.Assert(instanceLocation != -1);
					instances.RemoveAt(instanceLocation);
					base.RemoveInstanceDisplay(instanceLocation);
				}
			}

			private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeInstance factTypeInstance = (e.ModelElement as FactTypeInstanceHasRoleInstance).FactTypeInstance;
				FactType factType;
				if (!factTypeInstance.IsDeleted &&
					null != (factType = factTypeInstance.FactType) &&
					!factType.IsDeleted &&
					factType == myFactType)
				{
					EditFactInstanceDisplay(factTypeInstance);
				}
			}

			private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				FactTypeInstance factTypeInstance = (e.ModelElement as FactTypeInstanceHasRoleInstance).FactTypeInstance;
				FactType factType;
				if (!factTypeInstance.IsDeleted &&
					null != (factType = factTypeInstance.FactType) &&
					!factType.IsDeleted &&
					factType == myFactType)
				{
					EditFactInstanceDisplay(factTypeInstance);
				}
			}

			private void RoleNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				Role role = e.ModelElement as Role;
				if (!role.IsDeleted)
				{
					UpdateColumnHeadersForRole(role, false);
				}
			}

			private void ObjectTypePlaysRoleAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ProcessObjectTypePlaysRole(e.ModelElement as ObjectTypePlaysRole);
			}

			private void ObjectTypePlaysRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ProcessObjectTypePlaysRole(e.ModelElement as ObjectTypePlaysRole);
			}

			private void ObjectTypeRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				ProcessObjectTypePlaysRole(e.ElementLink as ObjectTypePlaysRole);
			}

			private void ProcessObjectTypePlaysRole(ObjectTypePlaysRole link)
			{
				ObjectType rolePlayer = link.RolePlayer;
				Role factRole = link.PlayedRole;
				if (!rolePlayer.IsDeleted && !factRole.IsDeleted)
				{
					UpdateColumnHeadersForRole(factRole, true);
				}
			}

			/// <summary>
			/// Checks if the branch should be ReadOnly, which happens when
			/// we have a 1-1 identifying FactType
			/// </summary>
			private void ValidateReadOnlyProxyObject()
			{
				ObjectType proxyObject = null;
				if (myFactType != null && myFactType.Objectification == null)
				{
					foreach (UniquenessConstraint iuc in myFactType.GetInternalConstraints<UniquenessConstraint>())
					{
						ObjectType preferredFor;
						LinkedElementCollection<Role> roles;
						if (null != (preferredFor = iuc.PreferredIdentifierFor) &&
							1 == (roles = iuc.RoleCollection).Count)
						{
							proxyObject = preferredFor;
							break;
						}
					}
				}
				int oldItemCount = -1;
				ObjectType oldProxyObject = myProxyObjectType;
				if (oldProxyObject != null)
				{
					if (oldProxyObject != proxyObject)
					{
						oldItemCount = VisibleItemCount;
						if (proxyObject == null)
						{
							myCachedEntityTypeInstances = null;
							myCachedFactTypeInstances = new List<FactTypeInstance>(myFactType.FactTypeInstanceCollection);
						}
						else
						{
							myCachedFactTypeInstances = null;
							myCachedEntityTypeInstances = new List<EntityTypeInstance>(proxyObject.EntityTypeInstanceCollection);
						}
					}
				}
				else if (myCachedFactTypeInstances != null && proxyObject != null)
				{
					oldItemCount = VisibleItemCount;
					myCachedFactTypeInstances = null;
					myCachedEntityTypeInstances = new List<EntityTypeInstance>(proxyObject.EntityTypeInstanceCollection);
				}
				myProxyObjectType = proxyObject;
				IsReadOnly = proxyObject != null;
				if (oldItemCount != -1)
				{
					base.Repopulate(oldItemCount);
				}
			}

			private void ObjectTypeNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectType objectType = e.ModelElement as ObjectType;
				if (!objectType.IsDeleted)
				{
					UpdateColumnHeadersForObjectType(objectType);
				}
			}
			#endregion
			#region Helper Methods
			/// <summary>
			/// Helper method to update column headers when <see cref="ObjectType"/> changes are made
			/// that affect the header display
			/// </summary>
			private void UpdateColumnHeadersForObjectType(ObjectType objectType)
			{
				LinkedElementCollection<RoleBase> roleCollection = myFactType.RoleCollection;
				int collectionCount = roleCollection.Count;
				if (myHasUnaryColumn)
				{
					int unaryColumn = myUnaryColumn;
					Role currentRole;
					if (unaryColumn < collectionCount && (currentRole = roleCollection[unaryColumn].Role).RolePlayer == objectType)
					{
						base.EditColumnHeader(1, currentRole);
					}
				}
				else
				{
					for (int i = 0; i < collectionCount; ++i)
					{
						Role currentRole = roleCollection[i].Role;
						if (currentRole.RolePlayer == objectType)
						{
							base.EditColumnHeader(i + 1, currentRole);
						}
					}
				}
			}
			/// <summary>
			/// Helper method to update column headers when <see cref="Role"/> changes are made
			/// that affect the header display
			/// </summary>
			private void UpdateColumnHeadersForRole(Role role, bool updateProxyHeader)
			{
				LinkedElementCollection<RoleBase> factRoles = myFactType.RoleCollection;
				int roleCount = factRoles.Count;
				if (myHasUnaryColumn)
				{
					int unaryColumn = myUnaryColumn;
					if (unaryColumn < roleCount && factRoles[unaryColumn].Role == role)
					{
						base.EditColumnHeader(1, role);
					}
				}
				else
				{
					for (int i = 0; i < roleCount; ++i)
					{
						if (role == factRoles[i].Role)
						{
							base.EditColumnHeader(i + 1, role);
							if (updateProxyHeader && myProxyObjectType != null)
							{
								Debug.Assert(roleCount == 2);
								int oppositeIndex = (i + 1) % 2;
								base.EditColumnHeader(oppositeIndex + 1, factRoles[oppositeIndex].Role);
							}
							break;
						}
					}
				}
			}
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
			#endregion // Helper Methods
			#region Branch Update Methods
			private void EditFactInstanceDisplay(FactTypeInstance factTypeInstance)
			{
				List<FactTypeInstance> instances;
				int location;
				if (null != (instances = myCachedFactTypeInstances) &&
					-1 != (location = instances.IndexOf(factTypeInstance)))
				{
					base.EditInstanceDisplay(location);
				}
			}
			#endregion // Branch Update Methods
			public sealed override void DeleteInstance(int row, int column)
			{
				if (base.IsFullRowSelectColumn(column))
				{
					IList elements = (myProxyObjectType != null) ? (IList)myCachedEntityTypeInstances : myCachedFactTypeInstances;
					if (elements != null && row < elements.Count)
					{
						using (Transaction t = Store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorRemoveInstanceTransactionText, myFactType.Name)))
						{
							((ModelElement)elements[row]).Delete();
							t.Commit();
						}
					}
				}
			}
		}
		private sealed class SamplePopulationEntityEditorBranch : SamplePopulationBaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly SamplePopulationBaseBranch myParentBranch;
			private EntityTypeInstance myEditInstance;
			private EntityTypeInstance myParentEntityInstance;
			private readonly ObjectType myParentEntityType;
			private readonly FactType myParentFactType;
			private FactTypeInstance myParentFactInstance;
			private readonly Role myEditRole;
			private int myItemCountCache;
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
				myItemCountCache = editRole.RolePlayer.PreferredIdentifier.RoleCollection.Count;
			}
			#endregion // Construction
			#region IBranch Interface Members
			/// <summary>
			/// Make this an expandable branch
			/// </summary>
			BranchFeatures IBranch.Features
			{
				get
				{
					return (base.Features & (~BranchFeatures.ComplexColumns)) | BranchFeatures.Expansions;
				}
			}

			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retVal = base.BeginLabelEdit(row, column, activationStyle);
				if (retVal.IsValid)
				{
					Role editRole = myEditRole.RolePlayer.PreferredIdentifier.RoleCollection[row];
					retVal.CustomInPlaceEdit = new CellEditContext(editRole, myEditInstance, this).CreateInPlaceEditControl();
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
				ObjectType instanceType = myEditRole.RolePlayer;
				Role identifierRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType editType = identifierRole.RolePlayer;
				EntityTypeInstance editInstance = myEditInstance;
				EntityTypeRoleInstance roleEditInstance = null;
				if (editInstance != null)
				{
					roleEditInstance = editInstance.FindRoleInstance(identifierRole);
				}
				if(newText.Length != 0)
				{
					Store store = Store;
					ObjectType rolePlayer = identifierRole.RolePlayer;
					using (Transaction t = store.TransactionManager.BeginTransaction(String.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, rolePlayer.Name)))
					{
						if (roleEditInstance != null)
						{
							roleEditInstance.Delete();
						}
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, editType, newText, ref instance, true);
						EditValueTypeInstance(instance, newText);
						ConnectInstance(result, identifierRole);
						t.Commit();
					}
					return LabelEditResult.AcceptEdit;
				}
				return LabelEditResult.CancelEdit;
			}

			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				retval.Image = 0;
				retval.SelectedImage = 0;  //you must set both .Image and .SelectedImage or an exception will be thrown
				return retval;
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
						return selectedRoleInstance.ObjectTypeInstance.Name;
					}
				}
				return ObjectTypeInstance.GetDisplayString(null, identifierType);
			}

			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.Icon)
				{
					return SamplePopulationBaseBranch.DeriveColumnName(myEditRole.RolePlayer.PreferredIdentifier.RoleCollection[row]);
				}
				return null;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				ObjectType instanceType = myEditRole.RolePlayer;
				Role identifierRole = instanceType.PreferredIdentifier.RoleCollection[row];
				ObjectType rolePlayer = identifierRole.RolePlayer;
				UniquenessConstraint roleIdentifier = rolePlayer.PreferredIdentifier;
				return (roleIdentifier != null) && (roleIdentifier.RoleCollection.Count > 1);
			}
			private new int VisibleItemCount
			{
				get
				{
					return myItemCountCache;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			#endregion // IBranch Interface Members
			#region Event Handlers
			protected sealed override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				DomainClassInfo classInfo;

				// Track EntityTypeInstance changes
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasEntityTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityTypeInstanceAddedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(EntityTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeInstanceHasRoleInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactTypeHasFactTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasFactTypeInstanceAddedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeInstanceHasRoleInstanceRemovedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeInstanceNameChangedEvent), action);
			}

			private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				if (link.EntityType == myParentEntityType && myParentEntityInstance == null)
				{
					myParentEntityInstance = link.EntityTypeInstance;
					EditColumnDisplay(0);
				}
			}

			private void EntityTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = RecurseInstanceUpdate(null, link.EntityTypeInstance, link.RoleInstance.Role, link.RoleInstance.ObjectTypeInstance);
				if (instance != null)
				{
					myEditInstance = instance;
					EditColumnDisplay(0);
				}
			}

			private void EntityTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = RecurseInstanceUpdate(null, link.EntityTypeInstance, link.RoleInstance.Role, link.RoleInstance.ObjectTypeInstance);
				if (instance != null && instance == myEditInstance)
				{
					myEditInstance = null;
					EditColumnDisplay(0);
				}
			}

			private void FactTypeHasFactTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
				if (link.FactType == myParentFactType && myParentFactInstance == null)
				{
					myParentFactInstance = link.FactTypeInstance;
					EditColumnDisplay(0);
				}
			}

			private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = RecurseInstanceUpdate(link.FactTypeInstance, null, link.RoleInstance.Role, link.RoleInstance.ObjectTypeInstance as EntityTypeInstance);
				if (instance != null)
				{
					myEditInstance = instance;
					EditColumnDisplay(0);
				}
			}

			private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = RecurseInstanceUpdate(link.FactTypeInstance, null, link.RoleInstance.Role, link.RoleInstance.ObjectTypeInstance as EntityTypeInstance);
				if (instance == myEditInstance)
				{
					myEditInstance = null;
					EditColumnDisplay(0);
				}
			}

			private void ObjectTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectTypeInstance objectTypeInstance = e.ModelElement as ObjectTypeInstance;
				if(myEditInstance == objectTypeInstance)
				{
					EditColumnDisplay(0);
				}
				else if(myEditInstance != null)
				{
					LinkedElementCollection<EntityTypeRoleInstance> instances = myEditInstance.RoleInstanceCollection;
					int instanceCount = instances.Count;
					for (int i = 0; i < instanceCount; ++i)
					{
						if (instances[i].ObjectTypeInstance == objectTypeInstance)
						{
							EditInstanceDisplay(i);
						}
					}
				}
			}

			private EntityTypeInstance RecurseInstanceUpdate(FactTypeInstance parentInstance, EntityTypeInstance entityTypeInstance, Role selectedRole, ObjectTypeInstance roleInstancePlayer)
			{
				if ((parentInstance != null && !parentInstance.IsDeleted && myParentFactInstance == parentInstance)
					|| (entityTypeInstance != null && !entityTypeInstance.IsDeleted && myParentEntityInstance == entityTypeInstance))
				{
					if (myEditRole == selectedRole)
					{
						return roleInstancePlayer as EntityTypeInstance;
					}
					return null;
				}
				else
				{
					if (myEditInstance == roleInstancePlayer)
					{
						base.EditColumnDisplay(0);
					}
					else
					{
						LinkedElementCollection<Role> identifierRoles = myEditRole.RolePlayer.PreferredIdentifier.RoleCollection;
						int identifierCount = identifierRoles.Count;
						for (int i = 0; i < identifierCount; ++i)
						{
							if (identifierRoles[i] == selectedRole)
							{
								EntityTypeRoleInstance roleInstance;
								if (myEditInstance != null
									&& null != (roleInstance = myEditInstance.FindRoleInstance(selectedRole))
									&& roleInstance.ObjectTypeInstance == roleInstancePlayer)
								{
									base.EditInstanceDisplay(i);
								}
								break;
							}
						}
					}
					SamplePopulationEntityEditorBranch parentBranch;
					if (null != (parentBranch = myParentBranch as SamplePopulationEntityEditorBranch))
					{
						EntityTypeInstance instance = parentBranch.RecurseInstanceUpdate(parentInstance, entityTypeInstance, selectedRole, roleInstancePlayer);
						if (instance != null)
						{
							if (myParentEntityInstance == null)
							{
								myParentEntityInstance = instance;
							}
							RoleInstance foundInstance = instance.FindRoleInstance(myEditRole);
							if (foundInstance != null)
							{
								return foundInstance.ObjectTypeInstance as EntityTypeInstance;
							}
							return null;
						}
						return instance;
					}
				}
				return null;
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
			#endregion // Helper Methods
			public sealed override void DeleteInstance(int row, int column)
			{
				// Empty implementation
			}
		}
		#endregion // Nested Branch Classes

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
		#endregion // Nested Class SamplePopulationVirtualTree

		private void vtrSamplePopulation_LabelEditControlChanged(object sender, EventArgs e)
		{
			if (LabelEditControl == null)
			{
				//NextSibling
				//Parent
				//Repeat Above Two until Parent is Invalid
				//RightColumn
				int row = vtrSamplePopulation.CurrentIndex;
				int col = vtrSamplePopulation.CurrentColumn;
				ColumnPermutation permutation = vtrSamplePopulation.ColumnPermutation;
				VirtualTreeCoordinate parentCoord = new VirtualTreeCoordinate(row, col);
				VirtualTreeCoordinate coord;
				do
				{
					VirtualTreeCoordinate siblingCoord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.NextSibling, parentCoord.Row, parentCoord.Column, permutation);
					if (siblingCoord.IsValid)
					{
						coord = siblingCoord;
						break;
					}
					coord = parentCoord;
					parentCoord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Parent, parentCoord.Row, parentCoord.Column, permutation);
				}
				while(parentCoord.IsValid);

				if (!parentCoord.IsValid)
				{
					VirtualTreeCoordinate rightColumnCoord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.RightColumn, coord.Row, coord.Column, permutation);
					if (!rightColumnCoord.IsValid)
					{
						VirtualTreeCoordinate lastChildCoord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.LastChild, coord.Row, coord.Column, permutation);
						if (lastChildCoord.IsValid)
						{
							coord = lastChildCoord;
						}
						coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Down, coord.Row, coord.Column, permutation);
						while (coord.IsValid && coord.Column != 1)
						{
							coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.LeftColumn, coord.Row, coord.Column, permutation);
						}
					}
					else
					{
						coord = rightColumnCoord;
					}
				}

				if (coord.IsValid)
				{
					vtrSamplePopulation.CurrentColumn = coord.Column;
					vtrSamplePopulation.CurrentIndex = coord.Row;
				}
				//StringBuilder navigationTest = new StringBuilder();
				//VirtualTreeCoordinate coord;
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.ComplexParent, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("ComplexParent:\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Down, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("Down:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.FirstChild, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("FirstChild:\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.LastChild, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("LastChild:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Left, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("Left:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.LeftColumn, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("LeftColumn:\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.NextSibling, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("NextSibling:\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.None, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("None:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Parent, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("Parent:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.PreviousSibling, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("PreviousSibling:\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Right, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("Right:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.RightColumn, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("RightColumn:\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//coord = vtrSamplePopulation.Tree.GetNavigationTarget(TreeNavigation.Up, vtrSamplePopulation.CurrentIndex, vtrSamplePopulation.CurrentColumn, vtrSamplePopulation.ColumnPermutation);
				//navigationTest.AppendLine("Up:\t\tValid: " + coord.IsValid + "\tCol: " + coord.Column + "\tRow: " + coord.Row);
				//MessageBox.Show(navigationTest.ToString(), "Navigation Targets");
				//if (nextCoordinate.IsValid)
				//{
				//    vtrSamplePopulation.CurrentColumn = nextCoordinate.Column;
				//    vtrSamplePopulation.CurrentIndex = nextCoordinate.Row;
				//}
				//int col = vtrSamplePopulation.CurrentColumn;
				//int row = vtrSamplePopulation.CurrentIndex;
				//bool lastCol = (col == vtrSamplePopulation.MultiColumnTree.ColumnCount - 1);
				//bool lastRow = (row == vtrSamplePopulation.Tree.VisibleItemCount - vtrSamplePopulation.Tree.GetSubItemCount(row, col) - 1);
				//VirtualTreeItemInfo info = vtrSamplePopulation.Tree.GetItemInfo(row, col, true);
				//while (info.Level > 0)
				//{
				//    if (info.LastBranchItem)
				//    {
				//        row = vtrSamplePopulation.Tree.GetParentIndex(row, col);
				//        info = vtrSamplePopulation.Tree.GetItemInfo(row, col, true);
				//    }
				//    else
				//    {
				//        vtrSamplePopulation.CurrentIndex = vtrSamplePopulation.Tree.GetDescendantItemCount(row, col, true, false) + row + 1;
				//        return;
				//    }
				//}
				//if (!lastCol || !lastRow)
				//{
				//    if (lastCol)
				//    {
				//        col = 1;
				//        row += vtrSamplePopulation.Tree.GetSubItemCount(row, col) + 1;
				//    }
				//    else
				//    {
				//        ++col;
				//    }
				//}
				//vtrSamplePopulation.CurrentColumn = col;
				//vtrSamplePopulation.CurrentIndex = row;
			}
		}
	}
}
 
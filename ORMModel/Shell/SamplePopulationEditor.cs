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

//#define ROLEINSTANCE_ROLEPLAYERCHANGE // Keep in sync with define in ObjectModel/SamplePopulation.cs. See comments in other location
//#define SAMPLEPOPULATIONEDITOR_DEBUGHELPER // Turn on to get some debug helper routines in place
// Turn this define on to treat an entity with a single-valued identifier that is
// identified by another entity as a single unit. This corresponds to the original
// design, but becomes ambiguous on a text edit because it is not clear if the user
// is editing the referenced entity instance or creating a new one. Note that the
// work was not done in BaseBranch.RecurseValueTypeInstance to extend this functionality to
// subtype instances, which have the same ambiguity issues, so this would need to be implemented
// if this is turned on.
// CONSIDER: Enable the functionality for a new instance where there is no ambiguity.
//#define SAMPLEPOPULATIONEDITOR_CHAINSINGLEVALUEDENTITYIDENTIFIERS
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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Shell;

#if VISUALSTUDIO_9_0
using VirtualTreeInPlaceControlFlags = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeInPlaceControls;
#endif //VISUALSTUDIO_9_0

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/// <summary>
	/// Editor Control for changing sample instances
	/// </summary>
	public partial class SamplePopulationEditor : UserControl
	{
		#region Member Variables
		private BaseBranch myBranch;
		private ObjectType mySelectedValueType;
		private ObjectType mySelectedEntityType;
		private FactType mySelectedFactType;
		private bool myInEvents;
		private bool myRepopulated;
		private bool myTransactionCommittedDuringLabelEdit;
		#endregion // Member Variables
		#region Static Variables
		/// <summary>
		/// Provides a ref to the tree control from nested objects
		/// </summary>
		private static VirtualTreeControl TreeControl;
		private static readonly ObjectStyle ErrorObject = (ObjectStyle)VirtualTreeConstant.FirstUserObjectStyle;
		#endregion // Static Variables
		#region Constructor
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SamplePopulationEditor()
		{
			InitializeComponent();
			lblNoSelection.Text = ResourceStrings.ModelSamplePopulationEditorEmptyDisplayText;
			VirtualTreeControl treeControl = vtrSamplePopulation;
			ImageList images = ResourceStrings.SamplePopulationEditorImageList;
			treeControl.ImageList = images;
			treeControl.HeaderImageList = images;
			Debug.Assert(SamplePopulationEditor.TreeControl == null, "The SamplePopulationEditor tool window should only be created once per Visual Studio session.");
			SamplePopulationEditor.TreeControl = treeControl;
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
						FactType objectifiedFactType = value.NestedFactType;
						if (objectifiedFactType != null)
						{
							if (mySelectedFactType != objectifiedFactType)
							{
								mySelectedFactType = objectifiedFactType;
								PopulateControlForFactType();
								AdjustVisibility(true);
							}
						}
						else
						{
							// PopulateControlForEntityType takes care of visibility
							PopulateControlForEntityType();
							mySelectedFactType = null;
						}
						mySelectedValueType = null;
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
				// Selecting an objectifying entity type sets both the
				// selected FactType and the selected EntityType. Check this
				// scenario to determine which object is actually selected.
				if (this.mySelectedEntityType != null)
				{
					return null;
				}
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
				else if (value != null && mySelectedEntityType != null)
				{
					// Switch to a FactType selection instead of an EntityType selection
					mySelectedEntityType = null;
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
		/// Return true if delete should be enabled for current contents.
		/// </summary>
		/// <remarks>The 'CanDelete' status should correspond to the main
		/// selection in the selection container, not the sub-selection
		/// within the control. Supporting different commands for different
		/// situations requires refreshing the command status on an internal
		/// selection change.</remarks>
		public bool CanDelete
		{
			get
			{
				BaseBranch branch;
				return null != (branch = myBranch) && !branch.IsReadOnly;
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
		private enum InstanceTypeImageIndex
		{
			/// <summary>
			/// A blank image
			/// </summary>
			None,
			/// <summary>
			/// A ValueType instance
			/// </summary>
			ValueType,
			/// <summary>
			/// An entity type instance
			/// </summary>
			EntityType,
			/// <summary>
			/// An entity type instance for an objectified FactType
			/// </summary>
			EntityTypeObjectification,
			/// <summary>
			/// An entity type subtype instance
			/// </summary>
			EntityTypeSubtype,
			/// <summary>
			/// An entity type subtype instance for an objectified FactType
			/// </summary>
			EntityTypeSubtypeObjectification,
			/// <summary>
			/// Overlay index
			/// </summary>
			ErrorOverlay,
		}
		private static InstanceTypeImageIndex GetImageIndex(ObjectType objectType)
		{
			return GetImageIndex(objectType, false);
		}
		private static InstanceTypeImageIndex GetImageIndex(ObjectType objectType, bool ignoreObjectification)
		{
			InstanceTypeImageIndex retVal = InstanceTypeImageIndex.None;
			if (objectType != null)
			{
				UniquenessConstraint pid;
				if (objectType.IsValueType)
				{
					retVal = InstanceTypeImageIndex.ValueType;
				}
				else if (null != (pid = objectType.ResolvedPreferredIdentifier))
				{
					ignoreObjectification = ignoreObjectification || objectType.NestedFactType == null;
					retVal = (pid.PreferredIdentifierFor == objectType) ?
						ignoreObjectification ? InstanceTypeImageIndex.EntityType : InstanceTypeImageIndex.EntityTypeObjectification :
						ignoreObjectification ? InstanceTypeImageIndex.EntityTypeSubtype : InstanceTypeImageIndex.EntityTypeSubtypeObjectification;
				}
			}
			return retVal;
		}
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
				headers[i+1] = new VirtualTreeColumnHeader(mySelectedValueType.Name, VirtualTreeColumnHeaderStyles.Default, (int)InstanceTypeImageIndex.ValueType);
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new ValueTypeBranch(mySelectedValueType);
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
			ObjectType entityType = mySelectedEntityType;
			UniquenessConstraint preferredIdentifier = entityType.ResolvedPreferredIdentifier;
			if (preferredIdentifier != null)
			{
				AdjustVisibility(true);
				int numColumns;
				VirtualTreeColumnHeader[] headers;
				ObjectType preferredFor = preferredIdentifier.PreferredIdentifierFor;
				if (entityType == preferredFor)
				{
					LinkedElementCollection<Role> roleCollection = preferredIdentifier.RoleCollection;
					numColumns = roleCollection.Count;
					headers = new VirtualTreeColumnHeader[numColumns + 1];
					for (int i = 0; i < numColumns; ++i)
					{
						Role role = roleCollection[i].Role;
						ObjectType columnRolePlayer = role.RolePlayer;
						headers[i + 1] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(role), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex((numColumns == 1 && columnRolePlayer.IsValueType) ? entityType : role.RolePlayer));
					}
				}
				else
				{
					numColumns = 1;
					headers = new VirtualTreeColumnHeader[2];
					headers[1] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(entityType), VirtualTreeColumnHeaderStyles.Default, (int)InstanceTypeImageIndex.EntityTypeSubtype);
				}
				headers[0] = CreateRowNumberColumn();
				vtrSamplePopulation.SetColumnHeaders(headers, true);
				myBranch = new EntityTypeBranch(mySelectedEntityType, numColumns + 1);
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
			FactType selectedFactType = mySelectedFactType;
			Debug.Assert(selectedFactType != null);
			DisconnectTree();

			// Figure out if the FactType is objectified and if the
			// preferred identifier of the objectification is not an
			// internal constraint of the objectified FactType.
			int factColumnOffset = 1;
			ObjectType objectifyingType;
			ObjectType preferredForEntityType = null;
			if (null != (objectifyingType = selectedFactType.NestingType))
			{
				UniquenessConstraint pid;
				if (null != (pid = objectifyingType.ResolvedPreferredIdentifier))
				{
					preferredForEntityType = pid.PreferredIdentifierFor;
					++factColumnOffset;
					if (objectifyingType == preferredForEntityType)
					{
						if (pid.IsObjectifiedPreferredIdentifier)
						{
							// Entity population is implied, do not show columns for it
							objectifyingType = null;
							--factColumnOffset;
						}
					}
				}
				else
				{
					objectifyingType = null;
				}
			}
			IList<RoleBase> factRoles = selectedFactType.OrderedRoleCollection;
			int factTypeColumnCount = factRoles.Count;
			int? unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
			int unaryRoleAdjust = 0;
			if (unaryRoleIndex.HasValue)
			{
				unaryRoleAdjust = unaryRoleIndex.Value;
				factTypeColumnCount = 1;
			}
			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[factTypeColumnCount + factColumnOffset];
			headers[0] = CreateRowNumberColumn();
			if (objectifyingType != null)
			{
				headers[1] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(objectifyingType, true), VirtualTreeColumnHeaderStyles.Default, (int)((objectifyingType == preferredForEntityType) ? InstanceTypeImageIndex.EntityType : InstanceTypeImageIndex.EntityTypeSubtype));
			}
			for (int i = 0; i < factTypeColumnCount; ++i)
			{
				Role role = factRoles[i + unaryRoleAdjust].Role;
				headers[i + factColumnOffset] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(role), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(role.RolePlayer));
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new FactTypeBranch(selectedFactType, factTypeColumnCount, unaryRoleIndex, objectifyingType);
			ConnectTree();
		}
		/// <summary>
		/// If the specified <see cref="ObjectType"/> plays a part in the
		/// identification structure for the current selection, then repopulate the control
		/// </summary>
		private void TestRepopulateForIdentifierPart(ObjectType identifierTypePart)
		{
			if ((myInEvents && myRepopulated) || identifierTypePart == null || identifierTypePart.IsDeleted)
			{
				return;
			}
			FactType selectedFactType;
			ObjectType selectedEntityType;
			if (null != (selectedFactType = mySelectedFactType))
			{
				if (IsPartOfDisplayedFactType(selectedFactType, identifierTypePart))
				{
					PopulateControlForFactType();
				}
			}
			else if (null != (selectedEntityType = mySelectedEntityType))
			{
				if (IsPartOfDisplayedIdentifier(selectedEntityType, identifierTypePart))
				{
					PopulateControlForEntityType();
				}
			}
		}
		/// <summary>
		/// If the specified <see cref="Role"/> plays a part in the
		/// identification structure for the current selection, then repopulate the control
		/// </summary>
		private void TestRepopulateForIdentifierPart(Role identifierRolePart)
		{
			if ((myInEvents && myRepopulated) || identifierRolePart == null || identifierRolePart.IsDeleted)
			{
				return;
			}
			FactType selectedFactType;
			ObjectType selectedEntityType;
			if (null != (selectedFactType = mySelectedFactType))
			{
				if (IsPartOfDisplayedFactType(selectedFactType, identifierRolePart))
				{
					PopulateControlForFactType();
				}
			}
			else if (null != (selectedEntityType = mySelectedEntityType))
			{
				if (IsPartOfDisplayedIdentifier(selectedEntityType, identifierRolePart))
				{
					PopulateControlForEntityType();
				}
			}
		}
		/// <summary>
		/// If the specified <see cref="FactType"/> is currently selected or plays a part in the
		/// identification structure for the current selection, then repopulate the control
		/// </summary>
		private void TestRepopulateForIdentifierPart(FactType identifierFactTypePart)
		{
			if ((myInEvents && myRepopulated) || identifierFactTypePart == null || identifierFactTypePart.IsDeleted)
			{
				return;
			}
			FactType selectedFactType;
			ObjectType selectedEntityType;
			ObjectType objectifyingType;
			if (null != (selectedFactType = mySelectedFactType))
			{
				if (selectedFactType == identifierFactTypePart)
				{
					PopulateControlForFactType();
				}
				else if (null != (objectifyingType = identifierFactTypePart.NestingType))
				{
					if (IsPartOfDisplayedFactType(selectedFactType, objectifyingType))
					{
						PopulateControlForFactType();
					}
				}
			}
			else if (null != (selectedEntityType = mySelectedEntityType) &&
				null != (objectifyingType = identifierFactTypePart.NestingType))
			{
				if (IsPartOfDisplayedIdentifier(selectedEntityType, objectifyingType))
				{
					PopulateControlForEntityType();
				}
			}
		}
		/// <summary>
		/// Recursively test if an <see cref="ObjectType"/> (<paramref name="identifierTypePart"/>) is part of the identification
		/// structure for any role players of a <paramref name="factType"/>.
		/// </summary>
		private static bool IsPartOfDisplayedFactType(FactType factType, ObjectType identifierTypePart)
		{
			ObjectType objectifyingType = factType.NestingType;
			if (objectifyingType != null)
			{
				return IsPartOfDisplayedIdentifier(objectifyingType, identifierTypePart);
			}
			else
			{
				foreach (RoleBase factRole in factType.RoleCollection)
				{
					if (IsPartOfDisplayedIdentifier(factRole.Role.RolePlayer, identifierTypePart))
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Recursively test if an <see cref="Role"/> (<paramref name="identifierRolePart"/>) is part of the identification
		/// structure for any role players of a <paramref name="factType"/>.
		/// </summary>
		private static bool IsPartOfDisplayedFactType(FactType factType, Role identifierRolePart)
		{
			ObjectType objectifyingType = factType.NestingType;
			if (objectifyingType != null)
			{
				return IsPartOfDisplayedIdentifier(objectifyingType, identifierRolePart);
			}
			else
			{
				foreach (RoleBase factRoleBase in factType.RoleCollection)
				{
					Role factRole = factRoleBase.Role;
					if (factRole == identifierRolePart ||
						IsPartOfDisplayedIdentifier(factRole.RolePlayer, identifierRolePart))
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Recursively test if an <see cref="ObjectType"/> (<paramref name="identifierTypePart"/>) is part of the identification
		/// structure of an <paramref name="identifiedType"/>.
		/// </summary>
		private static bool IsPartOfDisplayedIdentifier(ObjectType identifiedType, ObjectType identifierTypePart)
		{
			if (null == identifiedType || null == identifierTypePart)
			{
				return false;
			}
			if (identifiedType == identifierTypePart)
			{
				return true;
			}
			UniquenessConstraint pid = identifiedType.ResolvedPreferredIdentifier;
			if (pid == null)
			{
				return false;
			}
			ObjectType preferredFor = pid.PreferredIdentifierFor;
			if (preferredFor == identifiedType)
			{
				FactType nestedFactType = identifiedType.NestedFactType;
				if (nestedFactType == null || !pid.IsObjectifiedPreferredIdentifier)
				{
					// Check the identifier roles
					foreach (Role identifierRole in pid.RoleCollection)
					{
						if (IsPartOfDisplayedIdentifier(identifierRole.RolePlayer, identifierTypePart))
						{
							return true;
						}
					}
				}
				if (nestedFactType != null)
				{
					// Test all of the FactType roles, they are all displayed
					foreach (RoleBase factRole in nestedFactType.RoleCollection)
					{
						if (IsPartOfDisplayedIdentifier(factRole.Role.RolePlayer, identifierTypePart))
						{
							return true;
						}
					}
				}
			}
			else
			{
				// Subtype situation, ask the supertype
				return IsPartOfDisplayedIdentifier(preferredFor, identifierTypePart);
			}
			return false;
		}
		/// <summary>
		/// Recursively test if an <see cref="Role"/> (<paramref name="identifierRolePart"/>) is part of the identification
		/// structure of an <paramref name="identifiedType"/>.
		/// </summary>
		private static bool IsPartOfDisplayedIdentifier(ObjectType identifiedType, Role identifierRolePart)
		{
			ObjectType identifierTypePart;
			if (null == identifiedType ||
				null == identifierRolePart ||
				null == (identifierTypePart = identifierRolePart.RolePlayer))
			{
				return false;
			}

			if (identifiedType == identifierTypePart)
			{
				return true;
			}
			UniquenessConstraint pid = identifiedType.ResolvedPreferredIdentifier;
			if (pid == null)
			{
				return false;
			}
			ObjectType preferredFor = pid.PreferredIdentifierFor;
			if (preferredFor == identifiedType)
			{
				FactType nestedFactType = identifiedType.NestedFactType;
				if (nestedFactType == null || !pid.IsObjectifiedPreferredIdentifier)
				{
					// Check the identifier roles
					foreach (Role identifierRole in pid.RoleCollection)
					{
						if (identifierRolePart == identifierRole ||
							IsPartOfDisplayedIdentifier(identifierRole.RolePlayer, identifierRolePart))
						{
							return true;
						}
					}
				}
				if (nestedFactType != null)
				{
					// Test all of the FactType roles, they are all displayed
					foreach (RoleBase factRoleBase in nestedFactType.RoleCollection)
					{
						Role factRole = factRoleBase.Role;
						if (factRole == identifierRolePart ||
							IsPartOfDisplayedIdentifier(factRole.RolePlayer, identifierRolePart))
						{
							return true;
						}
					}
				}
			}
			else
			{
				// Subtype situation, ask the supertype
				return IsPartOfDisplayedIdentifier(preferredFor, identifierRolePart);
			}
			return false;
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
				BaseBranch baseBranch = info.Branch as BaseBranch;
				if (null != baseBranch && baseBranch.IsFullRowSelectColumn(info.Column))
				{
					multiColumnHighlight = true;
				}
			}
			vtr.MultiColumnHighlight = multiColumnHighlight;
		}
		/// <summary>
		/// Perform a delete action on the selected cell
		/// </summary>
		public void DeleteSelectedCell()
		{
			BaseBranch rootBranch;
			if (!CanDelete ||
				null == (rootBranch = myBranch))
			{
				return;
			}

			int currentColumn;
			int currentRow;
			VirtualTreeControl vtr;
			ITree tree;
			BaseBranch resolvedBranch;
			VirtualTreeItemInfo info;
			if (null != (vtr = vtrSamplePopulation) &&
				null != (tree = vtr.Tree) &&
				VirtualTreeConstant.NullIndex != (currentColumn = vtr.CurrentColumn) &&
				VirtualTreeConstant.NullIndex != (currentRow = vtr.CurrentIndex) &&
				!(info =  tree.GetItemInfo(currentRow, currentColumn, false)).Blank &&
				null != (resolvedBranch = info.Branch as BaseBranch))
			{
				int resolvedColumn = info.Column;
				int resolvedRow = info.Row;
				if (rootBranch == resolvedBranch && rootBranch.IsFullRowSelectColumn(resolvedColumn))
				{
					resolvedBranch.DeleteInstance(resolvedRow, resolvedColumn);
				}
				else
				{
					resolvedBranch.DeleteInstancePart(resolvedRow, resolvedColumn);
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
			if (mySelectedFactType != null)
			{
				store = mySelectedFactType.Store;
				instanceTypeName = mySelectedFactType.Name;
			}
			else if (mySelectedEntityType != null)
			{
				store = mySelectedEntityType.Store;
				instanceTypeName = mySelectedEntityType.Name;
			}
			else if (mySelectedValueType != null)
			{
				store = mySelectedValueType.Store;
				instanceTypeName = mySelectedValueType.Name;
			}
			if (store != null)
			{
				vtrSamplePopulation.BeginLabelEdit();
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
		/// <param name="error">The error being repaired</param>
		/// <param name="autoCorrectRole">The role to correct</param>
		/// <returns>true if the error was automatically corrected. This can return false
		/// if the <paramref name="autoCorrectRole"/> is a role in a <see cref="FactType"/>
		/// with an implied population.</returns>
		public bool AutoCorrectMandatoryError(PopulationMandatoryError error, Role autoCorrectRole)
		{
			if (autoCorrectRole != null)
			{
				// The represented elements for the error correspond to either
				// a role if the FactType is not automatically populated, or the
				// identified ObjectType if it is read only.
				ModelElement[] representedElements = ((IRepresentModelElements)error).GetRepresentedElements();
				FactType factType = autoCorrectRole.FactType;
				SubtypeFact subtypeFact = factType as SubtypeFact;
				ObjectType subtype = null;
				ObjectType impliedEntityType;
				ObjectType impliedSupertype;
				if ((Array.IndexOf<ModelElement>(representedElements, (ModelElement)subtypeFact ?? autoCorrectRole) != -1 ||
					subtypeFact != null && subtypeFact.ProvidesPreferredIdentifier && autoCorrectRole is SupertypeMetaRole && Array.IndexOf<ModelElement>(representedElements, subtype = subtypeFact.Subtype) != -1) ||
					(autoCorrectRole.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole && Array.IndexOf<ModelElement>(representedElements, autoCorrectRole.OppositeRole.Role.RolePlayer) != -1))
				{
					ObjectTypeInstance instance = error.ObjectTypeInstance;
					MandatoryConstraint constraint = error.MandatoryConstraint;
					EntityTypeSubtypeInstance subtypeInstance = null;
					FactTypeInstance factInstance = null;
					EntityTypeInstance entityInstance = null;
					FactTypeInstanceImplication implication;
					Store store = instance.Store;
					using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, (subtype != null) ? subtype.Name : factType.Name)))
					{
						if (subtype != null)
						{
							subtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance((EntityTypeInstance)instance, subtype, false, true);
						}
						else if ((implication = new FactTypeInstanceImplication(factType)).IsImplied && implication.ImpliedProxyRole == null && (impliedSupertype = implication.IdentifyingSupertype) != null)
						{
							FactType selectedFactType = mySelectedFactType;
							impliedEntityType = implication.ImpliedByEntityType;
							EntityTypeBranch.ConnectInstance(impliedSupertype, impliedSupertype != impliedEntityType ? impliedEntityType : null, ref entityInstance, ref subtypeInstance, instance, autoCorrectRole);
							if (selectedFactType != null && entityInstance != null)
							{
								factInstance = new FactTypeInstance(store);
								factInstance.FactType = selectedFactType;
								new ObjectificationInstance(factInstance, (ObjectTypeInstance)subtypeInstance ?? entityInstance);
							}
						}
						else
						{
							FactTypeBranch.ConnectInstance(ref factInstance, instance, autoCorrectRole, null);
						}
						if (t.HasPendingChanges)
						{
							t.Commit();
						}
					}
					ObjectifiedInstanceRequiredError objectifiedInstanceRequired;
					TooFewFactTypeRoleInstancesError partialFactInstanceError;
					TooFewEntityTypeRoleInstancesError partialEntityInstanceError;
					object selectErrorObject = null;
					object selectInstance = null;
					if (null != subtypeInstance)
					{
						if (null != (objectifiedInstanceRequired = subtypeInstance.ObjectifiedInstanceRequiredError))
						{
							// We need to create the subtypeinstance and attach it to a FactType, but we don't know
							// which one, so we defer to the branches to get a good selection.
							selectErrorObject = objectifiedInstanceRequired;
						}
						else
						{
							selectInstance = subtypeInstance;
						}
					}
					else if (null != entityInstance)
					{
						if (null != (partialEntityInstanceError = entityInstance.TooFewEntityTypeRoleInstancesError))
						{
							selectErrorObject = partialEntityInstanceError;
						}
						else
						{
							selectInstance = entityInstance;
						}
					}
					else if (null != factInstance)
					{
						if (null != (partialFactInstanceError = factInstance.TooFewFactTypeRoleInstancesError))
						{
							// The new fact instance is likely to have a partial
							// population. Activate the row if this is the case.
							selectErrorObject = partialFactInstanceError;
						}
						else
						{
							selectInstance = factInstance;
						}
					}
					if (selectErrorObject != null)
					{
						if (vtrSamplePopulation.SelectObject(null, selectErrorObject, (int)ErrorObject, 0))
						{
							vtrSamplePopulation.BeginLabelEdit();
						}
					}
					else if (selectInstance != null)
					{
						vtrSamplePopulation.SelectObject(null, selectInstance, (int)ObjectStyle.TrackingObject, 0);
					}
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Select the instance for the given error in the population window and activate in place editors as appropriate
		/// </summary>
		public bool ActivateModelError(ModelError error)
		{
			// Add special handling for incomplete identifier errors on objectified
			// EntityType instances with an external identifier. These instances are
			// not displayed (they are in the dropdown for the identifier column), so
			// there is no way to select and expand them without associating an empty FactTypeInstance.
			// Note that the identifier picker dropdown special cases empty identifier instances
			// so that they can be easily 'stolen' for other instances.
			TooFewEntityTypeRoleInstancesError partialIdentifierError;
			EntityTypeInstance entityInstance;
			ObjectType entityType;
			if (null != (partialIdentifierError = error as TooFewEntityTypeRoleInstancesError) &&
				null != (entityInstance = partialIdentifierError.EntityTypeInstance) &&
				null != entityInstance.ObjectifiedInstanceRequiredError &&
				null != (entityType = entityInstance.EntityType))
			{
				Store store = entityInstance.Store;
				FactType factType = entityType.NestedFactType;
				using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRelateObjectifiedInstanceIdentifierTransactionText, entityType.Name, entityInstance.IdentifierName, FactTypeInstance.GenerateEmptyInstanceName(factType))))
				{
					FactTypeInstance factInstance = new FactTypeInstance(store);
					factInstance.FactType = factType; // Must be set if the error is present
					entityInstance.ObjectifiedInstance = factInstance;
					t.Commit();
				}
			}

			bool retVal = false;
			if (retVal = vtrSamplePopulation.SelectObject(null, error, (int)ErrorObject, 0))
			{
				vtrSamplePopulation.BeginLabelEdit();
			}
			return retVal;
		}
		/// <summary>
		/// Determine if an <see cref="ObjectType"/> has a complex identifier, meaning
		/// that the immediate preferred identifier or the identifier of the immediate identifier
		/// (applied recursively) has multiple parts, or if the objectType is identified by the
		/// identifier for a supertype.
		/// </summary>
		private static bool HasComplexIdentifier(ObjectType objectType)
		{
			return HasMultiPartIdentifier(
				objectType,
#if SAMPLEPOPULATIONEDITOR_CHAINSINGLEVALUEDENTITYIDENTIFIERS
				false,
#else
				true,
#endif
				true);
		}
		/// <summary>
		/// Determine if an <see cref="ObjectType"/> has a complex identifier, meaning
		/// that the immediate preferred identifier or the identifier of the immediate identifier
		/// (applied recursively) has multiple parts, or if the objectType is identified by the
		/// identifier for a supertype.
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> to test</param>
		/// <param name="entityTypeIdentifierIsComplex">If set, then treat a single-role identifer
		/// with non-ValueType role player as complex.</param>
		/// <param name="supertypeIdentifierIsComplex">If set, and <paramref name="objectType"/>
		/// is identified by a supertype, then it is always considered to be complex even if
		/// the supertype identifier is not complex.</param>
		private static bool HasMultiPartIdentifier(ObjectType objectType, bool entityTypeIdentifierIsComplex, bool supertypeIdentifierIsComplex)
		{
			UniquenessConstraint pid;
			if (null != objectType &&
				null != (pid = objectType.ResolvedPreferredIdentifier))
			{
				if (supertypeIdentifierIsComplex && pid.PreferredIdentifierFor != objectType)
				{
					return true;
				}
				LinkedElementCollection<Role> roles = pid.RoleCollection;
				int roleCount = roles.Count;
				if (roleCount > 1)
				{
					return true;
				}
				for (int i = 0; i < roleCount; ++i)
				{
					ObjectType recurseObjectType = roles[i].RolePlayer;
					if (entityTypeIdentifierIsComplex && recurseObjectType != null && !recurseObjectType.IsValueType)
					{
						return true;
					}
					if (HasMultiPartIdentifier(recurseObjectType, entityTypeIdentifierIsComplex, supertypeIdentifierIsComplex))
					{
						return true;
					}
				}
			}
			return false;
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
			myTransactionCommittedDuringLabelEdit = false;
			if (Utility.ValidateStore(store) == null)
			{
				return;
			}
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo;
			DomainPropertyInfo propertyInfo;

			// Track Currently Executing Events
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent), action);

			// Track committing transactions
			eventManager.AddOrRemoveHandler(new EventHandler<TransactionCommitEventArgs>(TransactionCommittedEvent), action);

			// Track FactType changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleRemovedEvent), action);

			// Track reading changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasReadingOrder.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ReadingOrderAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingOrderRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(ReadingOrderReorderedEvent), action);

			// Track role player changes
			classInfo = dataDirectory.FindDomainRelationship(ObjectTypePlaysRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RolePlayerAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(RolePlayerRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(RolePlayerRolePlayerChangedEvent), action);

			// Track EntityTypeInstance changes
			classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(EntityTypeHasPreferredIdentifierRolePlayerChangedEvent), action);

			classInfo = dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasPreferredIdentifierRoleAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasPreferredIdentifierRoleRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(PreferredIdentifierRoleOrderChangedEvent), action);

			classInfo = dataDirectory.FindDomainRelationship(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeMissingReferenceSchemeAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeMissingReferenceSchemeRemovedEvent), action);

			// Track fact type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasFactType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemovedEvent), action);

			// Track object type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasObjectType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectTypeRemovedEvent), action);

			// Track objectification changes
			classInfo = dataDirectory.FindDomainRelationship(Objectification.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationRolePlayerChangedEvent), action);

			// Track SubtypeFact changes
			classInfo = dataDirectory.FindDomainClass(SubtypeFact.DomainClassId);
			propertyInfo = dataDirectory.FindDomainProperty(SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(SubtypeFactIdentificationPathChangedEvent), action);
		}
		#endregion // Event Handler Attach/Detach Methods
		#region Fact Type Instance Event Handlers
		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			myInEvents = false; // Sanity, should not be needed
			myRepopulated = false;
			if (myBranch != null)
			{
				VirtualTreeControl treeControl = vtrSamplePopulation;
				ITree tree = treeControl.Tree;
				if (tree != null)
				{
					myInEvents = true;
					treeControl.InLabelEdit = false;
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
			FactTypeHasRole link = (FactTypeHasRole)e.ModelElement;
			FactType factType = link.FactType;
			if (!factType.IsDeleted)
			{
				if (factType == mySelectedFactType)
				{
					PopulateControlForFactType();
				}
				else
				{
					TestRepopulateForIdentifierPart(link.Role.Role);
				}
			}
		}
		private void FactTypeHasRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactTypeHasRole link = (FactTypeHasRole)e.ModelElement;
			FactType factType = link.FactType;
			if (!factType.IsDeleted && factType == mySelectedFactType)
			{
				if (factType == mySelectedFactType)
				{
					PopulateControlForFactType();
				}
				else
				{
					TestRepopulateForIdentifierPart(link.Role.Role);
				}
			}
		}
		private void RolePlayerAddedEvent(object sender, ElementAddedEventArgs e)
		{
			// Adding an implicit boolean value type is treated the same as removing a role
			if (myRepopulated)
			{
				return;
			}
			TestRepopulateForIdentifierPart(((ObjectTypePlaysRole)e.ModelElement).PlayedRole);
		}
		private void RolePlayerRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			TestRepopulateForIdentifierPart(((ObjectTypePlaysRole)e.ModelElement).PlayedRole);
		}
		private void RolePlayerRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			TestRepopulateForIdentifierPart(((ObjectTypePlaysRole)e.ElementLink).PlayedRole);
		}
		private void ReadingOrderAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedValueType == null)
			{
				FactTypeHasReadingOrder link = (FactTypeHasReadingOrder)e.ModelElement;
				FactType factType;
				if (!link.IsDeleted &&
					!(factType = link.FactType).IsDeleted &&
					factType.ReadingOrderCollection[0] == link.ReadingOrder)
				{
					// The order is based on the first one. Any other added one will not
					// longer be first, or will have been moved into the first position,
					// or will have triggered a delete
					TestRepopulateForIdentifierPart(factType);
				}
			}
		}
		private void ReadingOrderRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedValueType == null)
			{
				FactTypeHasReadingOrder link = (FactTypeHasReadingOrder)e.ModelElement;
				FactType factType;
				if (!(factType = link.FactType).IsDeleted)
				{
					// We have no way of telling where this readingorder used to be position
					// in an event, so we are forced to repopulate every time.
					TestRepopulateForIdentifierPart(factType);
				}
			}
		}
		private void ReadingOrderReorderedEvent(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedValueType == null)
			{
				FactType factType = (FactType)e.SourceElement;
				if (!factType.IsDeleted &&
					(e.OldOrdinal == 0 || e.NewOrdinal == 0))
				{
					TestRepopulateForIdentifierPart(factType);
				}
			}
		}
		private void SubtypeFactIdentificationPathChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			SubtypeFact subtypeFact = (SubtypeFact)e.ModelElement;
			if (!subtypeFact.IsDeleted)
			{
				ObjectType subtype;
				if (subtypeFact == mySelectedFactType)
				{
					// Changing the objectification path changes whether this is implied or not
					PopulateControlForFactType();
				}
				else if (null != (subtype = subtypeFact.Subtype))
				{
					if (subtype == mySelectedEntityType)
					{
						if (null != (mySelectedFactType = subtype.NestedFactType))
						{
							PopulateControlForFactType();
						}
						else
						{
							PopulateControlForEntityType();
						}
					}
					else
					{
						TestRepopulateForIdentifierPart(subtype);
					}
				}
			}
		}
		#endregion // Fact Type Instance Event Handlers
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
					if (null != (mySelectedFactType = entityType.NestedFactType))
					{
						PopulateControlForFactType();
					}
					else
					{
						PopulateControlForEntityType();
					}
					return;
				}
			}
			else if (null != (selectedFactType = mySelectedFactType))
			{
				if (selectedFactType == entityType.NestedFactType)
				{
					PopulateControlForFactType();
					return;
				}
				if (preferredIdentifier == null)
				{
					preferredIdentifier = link.PreferredIdentifier;
				}
				// Check if the selected FactType's population is now implied
				if (!preferredIdentifier.IsDeleted)
				{
					foreach (Role identifierRole in preferredIdentifier.RoleCollection)
					{
						if (identifierRole.FactType == selectedFactType)
						{
							PopulateControlForFactType();
							return;
						}
					}
				}
			}
			TestRepopulateForIdentifierPart(entityType);
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
			if (mySelectedValueType == null)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor))
				{
					if (mySelectedEntityType == preferredFor)
					{
						FactType selectedFactType = mySelectedFactType;
						if (selectedFactType != null && !selectedFactType.IsDeleted)
						{
							if (!uniqueness.IsObjectifiedPreferredIdentifier)
							{
								PopulateControlForFactType();
							}
						}
						else
						{
							mySelectedFactType = null;
							PopulateControlForEntityType();
						}
					}
					else
					{
						TestRepopulateForIdentifierPart(preferredFor);
					}
				}
			}
		}

		private void EntityTypeHasPreferredIdentifierRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedValueType == null)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint uniqueness;
				ObjectType preferredFor;
				if (null != (uniqueness = link.ConstraintRoleSequence as UniquenessConstraint) &&
					!uniqueness.IsDeleted &&
					null != (preferredFor = uniqueness.PreferredIdentifierFor))
				{
					if (mySelectedEntityType == preferredFor)
					{
						FactType selectedFactType = mySelectedFactType;
						if (selectedFactType != null && !selectedFactType.IsDeleted)
						{
							if (!uniqueness.IsObjectifiedPreferredIdentifier)
							{
								PopulateControlForFactType();
							}
						}
						else
						{
							mySelectedFactType = null;
							PopulateControlForEntityType();
						}
					}
					else
					{
						TestRepopulateForIdentifierPart(preferredFor);
					}
				}
			}
		}

		private void EntityTypeMissingReferenceSchemeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedValueType == null)
			{
				ObjectType entityType = ((ObjectTypeHasEntityTypeRequiresReferenceSchemeError)e.ModelElement).ObjectType;
				if (!entityType.IsDeleted)
				{
					if (mySelectedEntityType == entityType)
					{
						if (null != (mySelectedFactType = entityType.NestedFactType))
						{
							PopulateControlForFactType();
						}
						else
						{
							PopulateControlForEntityType();
						}
					}
					else
					{
						TestRepopulateForIdentifierPart(entityType);
					}
				}
			}
		}

		private void EntityTypeMissingReferenceSchemeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			if (mySelectedValueType == null)
			{
				ObjectType entityType = ((ObjectTypeHasEntityTypeRequiresReferenceSchemeError)e.ModelElement).ObjectType;
				if (!entityType.IsDeleted)
				{
					if (mySelectedEntityType == entityType)
					{
						if (null != (mySelectedFactType = entityType.NestedFactType))
						{
							PopulateControlForFactType();
						}
						else
						{
							PopulateControlForEntityType();
						}
					}
					else
					{
						TestRepopulateForIdentifierPart(entityType);
					}
				}
			}
		}

		private void ObjectificationAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType selectedFactType;
			ObjectType selectedEntityType = null;
			if (null != (selectedFactType = mySelectedFactType) ||
				null != (selectedEntityType = mySelectedEntityType))
			{
				Objectification link = (Objectification)e.ModelElement;
				ObjectType objectifyingType = link.NestingType;
				FactType objectifiedType = link.NestedFactType;
				UniquenessConstraint pid;
				if (selectedFactType != null &&
					objectifiedType == selectedFactType)
				{
					if (null != (pid = objectifyingType.PreferredIdentifier) &&
						!pid.IsObjectifiedPreferredIdentifier)
					{
						// Repopulate with the identifier column
						PopulateControlForFactType();
					}
				}
				else if (selectedEntityType == objectifyingType)
				{
					// Populate as a FactType
					mySelectedFactType = link.NestedFactType;
					PopulateControlForFactType();
				}
				else
				{
					TestRepopulateForIdentifierPart(objectifyingType);
				}
			}
		}
		private void ObjectificationRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType selectedFactType;
			Objectification link = (Objectification)e.ModelElement;
			if (null != (selectedFactType = mySelectedFactType) &&
				selectedFactType == link.NestedFactType &&
				!selectedFactType.IsDeleted)
			{
				if (mySelectedEntityType != null)
				{
					mySelectedFactType = null;
					PopulateControlForEntityType();
				}
				else
				{
					// It is very hard to tell here if the preferred identifier
					// was objectified or not, so we repopulate all the time.
					PopulateControlForFactType();
				}
				return;
			}
			TestRepopulateForIdentifierPart(link.NestingType);
		}
		private void ObjectificationRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			FactType selectedFactType;
			if (null != (selectedFactType = mySelectedFactType))
			{
				ObjectType selectedEntityType;
				Objectification link = (Objectification)e.ElementLink;
				if (e.DomainRole.Id == Objectification.NestedFactTypeDomainRoleId)
				{
					if (selectedFactType == e.OldRolePlayer)
					{
						if (null != (selectedEntityType = mySelectedEntityType))
						{
							if (selectedEntityType == link.NestingType)
							{
								mySelectedFactType = link.NestedFactType;
								PopulateControlForEntityType();
							}
						}
						else
						{
							PopulateControlForFactType();
						}
						return;
					}
					else if (selectedFactType == link.NestedFactType)
					{
						if (null != (selectedEntityType = mySelectedEntityType))
						{
							if (null != (mySelectedFactType = selectedEntityType.NestedFactType))
							{
								PopulateControlForFactType();
							}
							else
							{
								mySelectedFactType = null;
								PopulateControlForEntityType();
							}
						}
						else
						{
							PopulateControlForFactType();
						}
						return;
					}
					TestRepopulateForIdentifierPart(link.NestingType);
				}
				else if (selectedFactType == link.NestedFactType)
				{
					if (null != (selectedEntityType = mySelectedEntityType))
					{
						if (selectedEntityType == e.OldRolePlayer)
						{
							if (null != (mySelectedFactType = selectedEntityType.NestedFactType))
							{
								PopulateControlForFactType();
							}
							else
							{
								PopulateControlForEntityType();
							}
						}
						else
						{
							mySelectedFactType = null;
							PopulateControlForEntityType();
						}
					}
					else
					{
						PopulateControlForFactType();
					}
				}
				else
				{
					TestRepopulateForIdentifierPart(link.NestingType);
					TestRepopulateForIdentifierPart((ObjectType)e.OldRolePlayer);
				}
			}
		}
		private void PreferredIdentifierRoleOrderChangedEvent(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			TestRepopulateForIdentifierPart(((UniquenessConstraint)e.SourceElement).PreferredIdentifierFor);
		}
		#endregion // Entity Type Instance Event Handlers
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
		#endregion // Misc Event Handlers
		#endregion // Model Events and Handler Methods
		#region Nested Branch Classes
		/// <summary>
		/// Implement this interface to indicate that a top-level branch
		/// supports expanding branches for a single 'new' row that
		/// is not attached to any instance. This interface is used along
		/// with <see cref="IUnattachedBranch"/> to smoothly coordinate
		/// attached these branches to the correct instances in response
		/// to events.
		/// </summary>
		private interface IUnattachedBranchOwner
		{
			/// <summary>
			/// Anchor an expansion branch that is not currently associated with an
			/// instance.
			/// </summary>
			/// <param name="objectInstance">The <see cref="ObjectTypeInstance"/> to attach. If this instance is already attached
			/// in another row in the parent then no modifications are made.</param>
			/// <param name="factInstance">The <see cref="FactTypeInstance"/> to attach. If this instance is already attached
			/// in another row in the parent then no modifications are made.</param>
			/// <returns>True if all unattached branches were successfully attached</returns>
			bool TryAnchorUnattachedBranches(ObjectTypeInstance objectInstance, FactTypeInstance factInstance);
		}
		/// <summary>
		/// Implemented on an expansion branch so that an implementation of
		/// of <see cref="IUnattachedBranchOwner"/> can smoothly attached
		/// instance to branch expansion with no instance information.
		/// </summary>
		private interface IUnattachedBranch
		{
			/// <summary>
			/// Attach instance information to an expanded branch
			/// </summary>
			/// <param name="objectInstance">The <see cref="ObjectTypeInstance"/> to attach</param>
			/// <param name="factInstance">The <see cref="FactTypeInstance"/> to attach</param>
			void AnchorUnattachedBranch(ObjectTypeInstance objectInstance, FactTypeInstance factInstance);
		}
		private abstract class BaseBranch : IBranch, IMultiColumnBranch
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
			public BaseBranch(int columnCount, Store store)
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
						private IList myInstanceList;
						/// <summary>
						/// Cater for alternative string representations to display instances
						/// using the IdentifierName instead of the default Name.
						/// </summary>
						protected override object TranslateFromDisplayObject(int newIndex, object newObject)
						{
							IList instanceList = myInstanceList;
							if (newIndex >= 0 &&
								null != (instanceList = myInstanceList))
							{
								return instanceList[newIndex];
							}
							return base.TranslateFromDisplayObject(newIndex, newObject);
						}
						/// <summary>
						/// Cater for alternative string representations to display instances
						/// using the IdentifierName instead of the default Name.
						/// </summary>
						protected override object TranslateToDisplayObject(object initialObject, IList contentList)
						{
							IList instanceList;
							if (initialObject != null &&
								null != (instanceList = myInstanceList))
							{
								int index = instanceList.IndexOf(initialObject);
								return (index != -1) ? contentList[index] : null;
							}
							return base.TranslateToDisplayObject(initialObject, contentList);
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
							CellEditContext contextInstance = (CellEditContext)context.Instance;
							ObjectType rolePlayer = contextInstance.ContextTargetObjectType;
							if (rolePlayer != null)
							{
								bool filterEmptyInstances = rolePlayer.NestedFactType == null && !rolePlayer.IsValueType;
								IList<ObjectTypeInstance> instances = rolePlayer.ObjectTypeInstanceCollection;
								int instanceCount = instances.Count;
								ObjectType subtype;
								bool displayIdentifierNames = false;
								ObjectTypeInstance currentInstance = (ObjectTypeInstance)contextInstance.myColumnInstance;
								UniquenessConstraint pid;
								ObjectType preferredFor;
								ObjectType contextEntityType = contextInstance.myEntityType;
								bool isEntityEditor = contextInstance.myIsEntityTypeEditor;
								if (contextEntityType != null && !isEntityEditor)
								{
									// FactType identifier

									// Show the current instance and any other instance that is not currently attached,
									// plus a null to detach the entity instance. We also show any instance that is
									// currently associated with an empty FactTypeInstance. These empty instances
									// are generated in response to a user action (selecting from the pick list or
									// activating a model error) and are expendable. However, we don't show them
									// if the instance we're currently choosing for is empty as this results in
									// the pointless exercise of moving an identifier from one empty FactTypeInstance
									// to another.
									ObjectTypeInstance[] unreferencedSupertypeInstances = null;
									int unreferenceSupertypeInstanceCount = 0;
									subtype = contextInstance.myEntityTypeSubtype;
									if (subtype != null)
									{
										unreferencedSupertypeInstances = GetUnreferencedSupertypeInstances(subtype.ObjectTypeInstanceCollection, contextEntityType.ObjectTypeInstanceCollection, null);
										if (unreferencedSupertypeInstances != null)
										{
											unreferenceSupertypeInstanceCount = unreferencedSupertypeInstances.Length;
										}
									}
									int unattachedCount = 0;
									bool showIdentifiersFromEmptyFactInstances = currentInstance != null || contextInstance.myFactInstance != null;
									for (int i = 0; i < instanceCount; ++i)
									{
										ObjectTypeInstance instance = instances[i];
										FactTypeInstance objectifiedInstance;
										if (instance != currentInstance &&
											(null == (objectifiedInstance = instance.ObjectifiedInstance) ||
											(showIdentifiersFromEmptyFactInstances && objectifiedInstance.RoleInstanceCollection.Count == 0)))
										{
											++unattachedCount;
										}
									}
									displayIdentifierNames = subtype == null; // Display the identifier name unless this is a subtype
									if (unattachedCount != 0)
									{
										int totalCount = unattachedCount + ((currentInstance != null) ? 1 : 0) + unreferenceSupertypeInstanceCount;
										ObjectTypeInstance[] filteredInstances = new ObjectTypeInstance[totalCount];
										unattachedCount = -1;
										for (int i = 0; i < instanceCount; ++i)
										{
											ObjectTypeInstance instance = instances[i];
											FactTypeInstance objectifiedInstance;
											if (instance == currentInstance ||
												null == (objectifiedInstance = instance.ObjectifiedInstance) ||
												(showIdentifiersFromEmptyFactInstances && objectifiedInstance.RoleInstanceCollection.Count == 0))
											{
												filteredInstances[++unattachedCount] = instance;
											}
										}
										if (unreferenceSupertypeInstanceCount != 0)
										{
											unreferencedSupertypeInstances.CopyTo(filteredInstances, unattachedCount + 1); 
										}
										instances = filteredInstances;
										instanceCount = totalCount;
									}
									else if (currentInstance == null)
									{
										if (unreferenceSupertypeInstanceCount != 0)
										{
											instances = unreferencedSupertypeInstances;
											instanceCount = instances.Count;
										}
										else
										{
											return null;
										}
									}
									else if (unreferenceSupertypeInstanceCount != 0)
									{
										ObjectTypeInstance[] newInstances = new ObjectTypeInstance[unreferenceSupertypeInstanceCount + 1];
										newInstances[0] = currentInstance;
										unreferencedSupertypeInstances.CopyTo(newInstances, 1);
										instances = newInstances;
										instanceCount = newInstances.Length;
									}
									else if (displayIdentifierNames)
									{
										myInstanceList = new ObjectTypeInstance[] { currentInstance };
										return new string[] { currentInstance.IdentifierName };
									}
									else
									{
										return new ObjectTypeInstance[] { currentInstance };
									}
								}
								else if (null != (subtype = contextInstance.myEntityTypeSubtype))
								{
									// Subtype editor, not a FactType identifier

									// Put all unused supertype instances in the list, plus the supertype
									// of the current subtype instance.
									ObjectTypeInstance[] filteredInstances = GetUnreferencedSupertypeInstances(subtype.ObjectTypeInstanceCollection, instances, contextInstance.mySubtypeInstance);
									if (filteredInstances != null)
									{
										instances = filteredInstances;
										instanceCount = instances.Count;
									}
									else
									{
										return null;
									}
								}
								else if (null != (pid = rolePlayer.ResolvedPreferredIdentifier) &&
									rolePlayer != (preferredFor = pid.PreferredIdentifierFor))
								{
									// Normal role player with a subtype, show the current instances and the unattached
									// supertype instances.
									ObjectTypeInstance[] unreferencedSupertypeInstances = GetUnreferencedSupertypeInstances(instances, preferredFor.ObjectTypeInstanceCollection, null);
									if (unreferencedSupertypeInstances != null)
									{
										if (instanceCount == 0)
										{
											instances = unreferencedSupertypeInstances;
											instanceCount = unreferencedSupertypeInstances.Length;
										}
										else
										{
											ObjectTypeInstance[] newInstances = new ObjectTypeInstance[instanceCount + unreferencedSupertypeInstances.Length];
											instances.CopyTo(newInstances, 0);
											unreferencedSupertypeInstances.CopyTo(newInstances, instanceCount);
											instances = newInstances;
											instanceCount = newInstances.Length;
										}
									}
								}
								else if (isEntityEditor && contextEntityType != null)
								{
									Role role = contextInstance.myRole;
									if (role != null)
									{
										switch (role.GetReferenceSchemePattern())
										{
											case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
												// There is a 1-1 correspondence between the instances full instances
												// and the role identifier part. Switching out creates duplicates in
												// implied EntityInstance collection, which is bad.
												return (currentInstance == null) ? null : new ObjectTypeInstance[] { currentInstance };
											case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
												List<ObjectTypeInstance> filteredInstances = null;
												foreach (ObjectTypeInstance instance in instances)
												{
													bool keepInstance = instance == currentInstance;
													if (!keepInstance)
													{
														keepInstance = true;
														foreach (EntityTypeRoleInstance testRoleInstance in EntityTypeRoleInstance.GetLinksToRoleCollection(instance))
														{
															if (testRoleInstance.EntityTypeInstance.EntityType == contextEntityType)
															{
																keepInstance = false;
																break;
															}
														}
													}
													if (keepInstance)
													{
														(filteredInstances ?? (filteredInstances = new List<ObjectTypeInstance>(instanceCount))).Add(instance);
													}
												}
												if (filteredInstances == null)
												{
													return null;
												}
												instances = filteredInstances;
												instanceCount = filteredInstances.Count;
												break;
										}
									}
								}
								if (instanceCount == 0)
								{
									return null;
								}
								if (filterEmptyInstances)
								{
									List<ObjectTypeInstance> filteredList = null;
									int lastFiltered = -1;
									for (int i = 0; i < instanceCount; ++i)
									{
										ObjectTypeInstance testInstance = instances[i];
										if (currentInstance != testInstance && IsEmptyInstance(testInstance))
										{
											if (filteredList == null)
											{
												if ((i - lastFiltered) == 1)
												{
													lastFiltered = i;
												}
												else
												{
													filteredList = new List<ObjectTypeInstance>(instanceCount - lastFiltered - 1);
													// Add up to this point
													for (int j = lastFiltered + 1; j < i; ++j)
													{
														filteredList.Add(instances[j]);
													}
												}
											}
										}
										else if (filteredList != null)
										{
											filteredList.Add(testInstance);
										}
									}
									if (filteredList != null)
									{
										instances = filteredList;
										instanceCount = instances.Count;
									}
									else if (lastFiltered != -1)
									{
										// The whole list was filtered
										return null;
									}
								}
								if (displayIdentifierNames)
								{
									string[] instanceIdentifierNames = new string[instanceCount];
									for (int i = 0; i < instanceCount; ++i)
									{
										instanceIdentifierNames[i] = instances[i].IdentifierName;
									}
									myInstanceList = (IList)instances;
									return instanceIdentifierNames;
								}
								return (IList)instances;
							}
							return null;
						}
					}
					/// <summary>
					/// Get all supertype instances not currently used by the given subtype.
					/// </summary>
					/// <param name="subtypeInstances">The full set of subtype instances</param>
					/// <param name="supertypeInstances">The full set of instances</param>
					/// <param name="filterInstance">A subtype instance to ignore</param>
					/// <returns><see cref="ObjectTypeInstance"/> array containing <see cref="EntityTypeInstance"/> elements</returns>
					private static ObjectTypeInstance[] GetUnreferencedSupertypeInstances(IList<ObjectTypeInstance> subtypeInstances, IList<ObjectTypeInstance> supertypeInstances, EntityTypeSubtypeInstance filterInstance)
					{
						int instanceCount = supertypeInstances.Count;
						int subtypeInstanceCount = subtypeInstances.Count;
						int currentInstanceCount = (filterInstance == null) ? 0 : 1;
						ObjectTypeInstance[] filteredInstances = null;
						if (subtypeInstanceCount >= currentInstanceCount)
						{
							instanceCount -= subtypeInstanceCount - currentInstanceCount;
							Debug.Assert(instanceCount >= 0);
							filteredInstances = new ObjectTypeInstance[instanceCount];
							if (instanceCount != 0)
							{
								ObjectTypeInstance[] referencedSupertypeInstances = new ObjectTypeInstance[subtypeInstanceCount - currentInstanceCount];
								int i = 0;
								foreach (EntityTypeSubtypeInstance subtypeInstance in subtypeInstances)
								{
									if (subtypeInstance != filterInstance)
									{
										referencedSupertypeInstances[i] = subtypeInstance.SupertypeInstance;
										++i;
									}
								}
								i = 0;
								foreach (ObjectTypeInstance instance in supertypeInstances)
								{
									if (-1 == Array.IndexOf<ObjectTypeInstance>(referencedSupertypeInstances, instance))
									{
										filteredInstances[i] = instance;
										++i;
									}
								}
							}
						}
						return filteredInstances;
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
							ObjectType rolePlayer;
							if (sourceType == typeof(string) &&
								(null == (rolePlayer = ((CellEditContext)context.Instance).ContextObjectType) ||
								HasMultiPartIdentifier(rolePlayer, false, false)))
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
									EntityTypeSubtypeInstance subtypeInstance = context.mySubtypeInstance;
									if (subtypeInstance != null)
									{
										EntityTypeInstance entityInstance = context.myEntityInstance;
										using (Transaction t = role.Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, context.myEntityTypeSubtype.Name, subtypeInstance.Name)))
										{
											subtypeInstance.Delete();
											t.Commit();
										}
										context.UpdateInstanceFields(null, null);
									}
									else
									{
										EntityTypeInstance entityInstance = context.myEntityInstance;
										IList<EntityTypeRoleInstance> entityRoleInstances = entityInstance.RoleInstanceCollection;
										EntityTypeRoleInstance roleInstance = EntityTypeInstance.FindRoleInstance(entityRoleInstances, role);
										ObjectTypeInstance deleteInstance = roleInstance.ObjectTypeInstance;
										ValueTypeInstance deleteValueInstance = null;
										if (role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
										{
											// See comments in EntityTypeBranch.CommitLabelEdit
											deleteValueInstance = deleteInstance as ValueTypeInstance;
											if (deleteValueInstance == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
											{
												throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
											}
										}
										using (Transaction t = role.Store.TransactionManager.BeginTransaction(
											(entityRoleInstances.Count == 1) ?
												string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, context.ContextObjectType.Name, entityInstance.Name) :
												string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(role, false), deleteInstance.Name)))
										{
											if (deleteValueInstance != null)
											{
												deleteValueInstance.Delete();
											}
											else
											{
												roleInstance.Delete();
											}
											t.Commit();
										}
										// Removing the last role instance can remove the entity type instance, check
										context.UpdateInstanceFields(entityInstance.IsDeleted ? null : entityInstance, null);
									}
								}
								else
								{
									FactTypeInstance factInstance = context.myFactInstance;
									if (role != null)
									{
										IList<FactTypeRoleInstance> factRoleInstances = factInstance.RoleInstanceCollection;
										FactTypeRoleInstance roleInstance = FactTypeInstance.FindRoleInstance(factRoleInstances, role);
										using (Transaction t = role.Store.TransactionManager.BeginTransaction(
											(factRoleInstances.Count == 1) ?
											string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, factInstance.Name) :
											string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(role, true), roleInstance.ObjectTypeInstance.Name)))
										{
											roleInstance.Delete();
											t.Commit();
										}
									}
									else
									{
										ObjectType contextObjectType = context.ContextObjectType;
										using (Transaction t = contextObjectType.Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorSeparateObjectifiedInstanceIdentifierTransactionText, contextObjectType.Name, factInstance.ObjectifyingInstance.IdentifierName, factInstance.Name)))
										{
											factInstance.ObjectifyingInstance = null;
											t.Commit();
										}
									}
									context.UpdateInstanceFields(factInstance.IsDeleted ? null : factInstance, null);
								}
							}
						}
						else if (columnInstance != typedValue)
						{
							ObjectType targetContextObjectType = context.ContextTargetObjectType;
							if (role != null)
							{
								using (Transaction t = role.Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, context.ContextObjectType.Name)))
								{
									if (targetContextObjectType != typedValue.ObjectType)
									{
										// This occurs if the list has been extended to show unreferenced supertype instances
										typedValue = EntityTypeSubtypeInstance.GetSubtypeInstance((EntityTypeInstance)typedValue, targetContextObjectType, false, true);
									}
									EntityEditorBranch entityEditorBranch = context.myEditBranch;
									if (entityEditorBranch != null)
									{
										entityEditorBranch.ConnectInstance(typedValue, role, null, context.myFactInstance);
										if (context.myIsEntityTypeEditor)
										{
											context.UpdateInstanceFields((ObjectTypeInstance)context.mySubtypeInstance ?? context.myEntityInstance);
										}
										else
										{
											context.UpdateInstanceFields(typedValue.ObjectifiedInstance, null);
										}
									}
									else if (context.myIsEntityTypeEditor)
									{
										EntityTypeInstance entityInstance = context.myEntityInstance;
										EntityTypeSubtypeInstance subtypeInstance = context.mySubtypeInstance;
										EntityTypeBranch.ConnectInstance(context.myEntityType, context.myEntityTypeSubtype, ref entityInstance, ref subtypeInstance, typedValue, role);
										context.UpdateInstanceFields(entityInstance, subtypeInstance);
									}
									else
									{
										FactTypeInstance factInstance = context.myFactInstance;
										FactTypeBranch.ConnectInstance(ref factInstance, typedValue, role, null);
										context.UpdateInstanceFields(factInstance, null);
									}
									if (t.HasPendingChanges)
									{
										t.Commit();
									}
								}
							}
							else
							{
								FactTypeInstance factInstance = context.myFactInstance;
								FactType factType;
								using (Transaction t = targetContextObjectType.Store.TransactionManager.BeginTransaction(string.Format(
									ResourceStrings.ModelSamplePopulationEditorRelateObjectifiedInstanceIdentifierTransactionText,
									context.ContextObjectType.Name,
									typedValue.IdentifierName,
									(factInstance != null) ?
										factInstance.Name :
										(null != (factType = targetContextObjectType.NestedFactType) ? FactTypeInstance.GenerateEmptyInstanceName(factType) : ""))))
								{
									if (targetContextObjectType != typedValue.ObjectType)
									{
										// This occurs if the list has been extended to show unreferenced supertype instances
										typedValue = EntityTypeSubtypeInstance.GetSubtypeInstance((EntityTypeInstance)typedValue, targetContextObjectType, false, true);
									}
									EntityEditorBranch entityEditorBranch = context.myEditBranch;
									if (entityEditorBranch != null)
									{
										entityEditorBranch.ConnectInstance(typedValue, null, targetContextObjectType, factInstance);
										context.UpdateInstanceFields(typedValue.ObjectifiedInstance, typedValue);
									}
									else
									{
										Debug.Assert(!context.myIsEntityTypeEditor);
										FactTypeBranch.ConnectInstance(ref factInstance, typedValue, null, targetContextObjectType);
										context.UpdateInstanceFields(factInstance, typedValue);
									}
									if (t.HasPendingChanges)
									{
										t.Commit();
									}
								}
							}
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
				public static readonly PropertyDescriptor Descriptor = new InstanceColumnDescriptor();
				private readonly EntityEditorBranch myEditBranch;
				private readonly Role myRole;
				private readonly ObjectType myEntityType;
				private readonly ObjectType myEntityTypeSubtype;
				private readonly bool myIsEntityTypeEditor;
				private EntityTypeInstance myEntityInstance;
				private EntityTypeSubtypeInstance mySubtypeInstance;
				private FactTypeInstance myFactInstance;
				private ObjectTypeInstance myColumnInstance;
				#endregion // Member Variables
				#region Constructor
				/// <summary>
				/// Create an editing context for the given entityType role and entityTypeInstance
				/// </summary>
				/// <param name="entityType">The entityType to attach to instances to.</param>
				/// <param name="entityTypeSubtype">A subtype of the entity to attach to.</param>
				/// <param name="role">The role being edited</param>
				/// <param name="entityInstance">The current entityTypeInstance. Can be null.</param>
				/// <param name="subtypeInstance">The current entityTypeSubtypeInstance. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				public CellEditContext(ObjectType entityType, ObjectType entityTypeSubtype, Role role, EntityTypeInstance entityInstance, EntityTypeSubtypeInstance subtypeInstance, EntityEditorBranch editBranch)
				{
					Debug.Assert(entityType != null);
					Debug.Assert(role != null);
					myEntityType = entityType;
					myEntityTypeSubtype = entityTypeSubtype;
					myRole = role;
					myIsEntityTypeEditor = true;
					myEditBranch = editBranch;
					UpdateInstanceFields(entityInstance, subtypeInstance);
				}
				/// <summary>
				/// Update instance fields for an entity type editor edit context
				/// </summary>
				private void UpdateInstanceFields(ObjectTypeInstance objectInstance)
				{
					EntityTypeSubtypeInstance subtypeInstance = objectInstance as EntityTypeSubtypeInstance;
					EntityTypeInstance entityInstance = (subtypeInstance != null) ? subtypeInstance.SupertypeInstance : (EntityTypeInstance)objectInstance;
					ObjectType entityTypeSubtype;
					if (subtypeInstance == null &&
						null != (entityTypeSubtype = myEntityTypeSubtype))
					{
						// Doing this simple sanity check means that we can call this
						// update function for all subtype cases.
						subtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, entityTypeSubtype, true, false);
					}
					UpdateInstanceFields(entityInstance, subtypeInstance);
				}
				/// <summary>
				/// Update instance fields for an entity type editor edit context
				/// </summary>
				private void UpdateInstanceFields(EntityTypeInstance entityInstance, EntityTypeSubtypeInstance subtypeInstance)
				{
					Debug.Assert(myIsEntityTypeEditor);
					myEntityInstance = entityInstance;
					mySubtypeInstance = subtypeInstance;
					ObjectTypeInstance newInstance = null;
					if (myEntityTypeSubtype != null)
					{
						newInstance = entityInstance;
					}
					else if (entityInstance != null)
					{
						EntityTypeRoleInstance roleInstance = entityInstance.FindRoleInstance(myRole);
						if (roleInstance != null)
						{
							newInstance = roleInstance.ObjectTypeInstance;
						}
					}
					myColumnInstance = newInstance;
				}
				/// <summary>
				/// Create an editing context for the given role and factTypeInstance
				/// </summary>
				/// <param name="role">The role being edited</param>
				/// <param name="factInstance">The current <see cref="FactTypeInstance"/>. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				public CellEditContext(Role role, FactTypeInstance factInstance, EntityEditorBranch editBranch)
				{
					Debug.Assert(role != null);
					myRole = role;
					myIsEntityTypeEditor = false;
					myEditBranch = editBranch;
					UpdateInstanceFields(factInstance, null);
				}
				/// <summary>
				/// Create an editing context for the given objectifying type and factTypeInstance
				/// </summary>
				/// <param name="objectifyingType">The objectifying type to modify.</param>
				/// <param name="factInstance">The current <see cref="FactTypeInstance"/>. Can be null.</param>
				/// <param name="objectifyingInstance">The current objectifying instance. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				public CellEditContext(ObjectType objectifyingType, FactTypeInstance factInstance, ObjectTypeInstance objectifyingInstance, EntityEditorBranch editBranch)
				{
					Debug.Assert(objectifyingType != null);
					UniquenessConstraint pid = objectifyingType.ResolvedPreferredIdentifier;
					Debug.Assert(pid != null);
					ObjectType preferredFor = pid.PreferredIdentifierFor;
					myEditBranch = editBranch;
					if (preferredFor != objectifyingType)
					{
						myEntityTypeSubtype = objectifyingType;
						myEntityType = preferredFor;
					}
					else
					{
						myEntityType = objectifyingType;
					}
					myIsEntityTypeEditor = false;
					UpdateInstanceFields(factInstance, objectifyingInstance);
				}
				/// <summary>
				/// Update instance fields for a FactType context
				/// </summary>
				private void UpdateInstanceFields(FactTypeInstance factInstance, ObjectTypeInstance objectifyingInstance)
				{
					Debug.Assert(!myIsEntityTypeEditor);
					Role role = myRole;
					if (role == null)
					{
						myFactInstance = factInstance;
						myColumnInstance = objectifyingInstance;
					}
					else
					{
						myFactInstance = factInstance;
						ObjectTypeInstance newInstance = null;
						if (factInstance != null)
						{
							FactTypeRoleInstance roleInstance = factInstance.FindRoleInstance(role);
							if (roleInstance != null)
							{
								newInstance = roleInstance.ObjectTypeInstance;
							}
						}
						myColumnInstance = newInstance;
					}
				}
				#endregion // Constructor
				#region Accessor Properties
				/// <summary>
				/// Determine the <see cref="ObjectType"/> of the instance we're editing
				/// </summary>
				private ObjectType ContextObjectType
				{
					get
					{
						Role role = myRole;
						return (role != null && (!(role is SupertypeMetaRole) || myEntityType == null)) ? role.RolePlayer : (myEntityTypeSubtype ?? myEntityType);
					}
				}
				/// <summary>
				/// Determine the target <see cref="ObjectType"/> of the instance we're editing.
				/// For subtype entity editor cases, the target object type is the supertype, whereas the
				/// <see cref="ContextObjectType"/> is the subtype.
				/// </summary>
				private ObjectType ContextTargetObjectType
				{
					get
					{
						Role role = myRole;
						return (role != null && (!(role is SubtypeMetaRole) || myEntityType == null)) ? role.RolePlayer : myIsEntityTypeEditor ? myEntityType : (myEntityTypeSubtype ?? myEntityType);
					}
				}
				#endregion // Accessor Properties
				#region CreateInPlaceEditControl method
				/// <summary>
				/// Create an inplace edit control that works with this context
				/// </summary>
				/// <returns>IVirtualTreeInPlaceControl</returns>
				public IVirtualTreeInPlaceControl CreateInPlaceEditControl()
				{
					ObjectType rolePlayer;
					Role role = myRole;
					bool blockEdits =
						(null == (rolePlayer = (role is SubtypeMetaRole) ? ContextTargetObjectType : ContextObjectType)) ||
						(myRole != null && rolePlayer.NestedFactType != null) ||
						HasComplexIdentifier(rolePlayer);
					TypeEditorHost host = EditContextTypeEditorHost.Create(
						Descriptor,
						this,
						blockEdits ? TypeEditorHostEditControlStyle.TransparentEditRegion : TypeEditorHostEditControlStyle.Editable);
					if (host != null)
					{
						(host as IVirtualTreeInPlaceControl).Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.SizeToText | (blockEdits ? VirtualTreeInPlaceControlFlags.ForwardKeyEvents | VirtualTreeInPlaceControlFlags.DrawItemText : 0);
					}
					return host;
				}
				#endregion // CreateInPlaceEditControl method
				#region EditContextTypeEditorHost
				/// <summary>
				/// A type editor class to set the test for a pass-through edit region the same as the parent text
				/// </summary>
				private class EditContextTypeEditorHost : OnScreenTypeEditorHost
				{
					/// <summary>
					/// Make sure the text always corresponds to the correct name. Without this,
					/// transparent edit region cases are returning incorrect text.
					/// </summary>
					public override string Text
					{
						get
						{
							if (this.EditControlStyle == TypeEditorHostEditControlStyle.TransparentEditRegion)
							{
								CellEditContext instance = (CellEditContext)CurrentInstance;
								ObjectTypeInstance columnInstance = instance.myColumnInstance;
								Role role = instance.myRole;
								if (columnInstance == null)
								{
									ObjectType objectifyingType = (role != null) ? ((role is SubtypeMetaRole) ? instance.myEntityType : role.RolePlayer) : (instance.myEntityTypeSubtype ?? instance.myEntityType);
									if (objectifyingType != null)
									{
										return ObjectTypeInstance.GetDisplayString(null, (role is SubtypeMetaRole) ? objectifyingType : (instance.myEntityTypeSubtype ?? objectifyingType), role == null);
									}
								}
								else if (role == null)
								{
									return columnInstance.IdentifierName;
								}
							}
							return base.Text;
						}
						set
						{
							base.Text = value;
						}
					}
					/// <summary>
					/// Creates a new TypeEditorHost to display the given UITypeEditor
					/// </summary>
					/// <param name="editor">The UITypeEditor instance to host</param>
					/// <param name="editControlStyle">The type of control to show in the edit area.</param>
					/// <param name="propertyDescriptor">Property descriptor used to get/set values in the drop-down.</param>
					/// <param name="instance">Instance object used to get/set values in the drop-down.</param>
					protected EditContextTypeEditorHost(UITypeEditor editor, PropertyDescriptor propertyDescriptor, object instance, TypeEditorHostEditControlStyle editControlStyle)
						: base(editor, propertyDescriptor, instance, editControlStyle)
					{
					}
					/// <summary>
					/// Factory method for creating the appropriate drop-down control based on the given property descriptor.
					/// If the property descriptor supports a UITypeEditor, a TypeEditorHost will be created with that editor.
					/// If not, and the TypeConverver attached to the PropertyDescriptor supports standard values, a
					/// TypeEditorHostListBox will be created with this TypeConverter.
					/// </summary>
					/// <param name="propertyDescriptor">A property descriptor describing the property being set</param>
					/// <param name="instance">The object instance being edited</param>
					/// <param name="editControlStyle">The type of control to show in the edit area.</param>
					/// <returns>A TypeEditorHost instance if the given property descriptor supports it, null otherwise.</returns>
					public static new TypeEditorHost Create(PropertyDescriptor propertyDescriptor, object instance, TypeEditorHostEditControlStyle editControlStyle)
					{
						TypeEditorHost host = null;
						if (propertyDescriptor != null)
						{
							UITypeEditor editor = propertyDescriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
							if (editor != null)
							{
								return new EditContextTypeEditorHost(editor, propertyDescriptor, instance, editControlStyle);
							}
							TypeConverter typeConverter = propertyDescriptor.Converter;
							if ((typeConverter != null) && typeConverter.GetStandardValuesSupported(null))
							{
								host = new OnScreenTypeEditorHostListBox(typeConverter, propertyDescriptor, instance, editControlStyle);
							}
						}
						return host;
					}
				}
				#endregion // EditContextTypeEditorHost
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

				VirtualTreeLabelEditData retVal = VirtualTreeLabelEditData.Default;
				if (UsesEditContext)
				{
					CellEditContext editContext = CreateEditContext(row, column);
					if (editContext != null)
					{
						retVal.CustomInPlaceEdit = editContext.CreateInPlaceEditControl();
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
				if (Utility.ValidateStore(store) == null)
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
			public bool IsReadOnly
			{
				get
				{
					return myIsReadOnly;
				}
				protected set
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
				return DeriveColumnName(role, false);
			}
			public static string DeriveColumnName(Role role, bool ignoreObjectification)
			{
				StringBuilder outputText = null;
				string retVal = (role == null || role.RolePlayer == null) ? ResourceStrings.ModelSamplePopulationEditorNullSelection : RecurseColumnIdentifier(role, null, ignoreObjectification, null, ref outputText);
				return (outputText != null) ? outputText.ToString() : retVal;
			}
			public static string DeriveColumnName(ObjectType objectType)
			{
				return DeriveColumnName(objectType, false);
			}
			public static string DeriveColumnName(ObjectType objectType, bool ignoreObjectification)
			{
				StringBuilder outputText = null;
				string retVal = (objectType == null) ? ResourceStrings.ModelSamplePopulationEditorNullSelection : RecurseColumnIdentifier(null, objectType, ignoreObjectification, null, ref outputText);
				return (outputText != null) ? outputText.ToString() : retVal;
			}
			protected static string GetRolePlayerTypeName(Role role, bool useRoleName)
			{
				string retVal = useRoleName ? role.Name : "";
				ObjectType rolePlayer;
				if (string.IsNullOrEmpty(retVal) &&
					null != (rolePlayer = role.RolePlayer))
				{
					retVal = rolePlayer.Name;
				}
				return retVal;
			}
			// UNDONE: This whole method needs to be localized
			private static string RecurseColumnIdentifier(Role role, ObjectType rolePlayer, bool ignoreObjectification, string listSeparator, ref StringBuilder outputText)
			{
				if (rolePlayer == null)
				{
					rolePlayer = role.RolePlayer;
				}
				if (rolePlayer == null)
				{
					if (outputText != null)
					{
						outputText.Append(" ");
					}
					return " ";
				}
				string roleName = (role != null) ? role.Name : "";
				string derivedName = (roleName.Length != 0) ? roleName : rolePlayer.Name;
				UniquenessConstraint identifier = null;
				ObjectType supertypeRolePlayer = null;
				bool isValueType = rolePlayer.IsValueType;
				FactType nestedFactType = (ignoreObjectification || isValueType) ? null : rolePlayer.NestedFactType;
				bool useIdentifiedReferenceMode = false;
				if (isValueType)
				{
					ObjectType identifiedType;
					switch (role.GetReferenceSchemePattern(out identifiedType))
					{
						case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
						case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
							rolePlayer = identifiedType;
							derivedName = rolePlayer.Name;
							useIdentifiedReferenceMode = true;
							break;
						default:
							if (outputText != null)
							{
								outputText.Append(derivedName);
								return null;
							}
							return derivedName;
					}
				}
				else if (null == (identifier = rolePlayer.ResolvedPreferredIdentifier))
				{
					if (nestedFactType == null)
					{
						if (outputText != null)
						{
							outputText.Append(derivedName);
							return null;
						}
						return derivedName;
					}
				}
				else
				{
					supertypeRolePlayer = identifier.PreferredIdentifierFor;
				}
				if (outputText == null)
				{
					outputText = new StringBuilder();
				}
				outputText.Append(derivedName);
				outputText.Append("(");
				bool identifierWritten = false;
				if (supertypeRolePlayer != null &&
					supertypeRolePlayer != rolePlayer)
				{
					RecurseColumnIdentifier(null, supertypeRolePlayer, false, listSeparator, ref outputText);
					identifierWritten = true;
				}
				else
				{
					if (listSeparator == null)
					{
						listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
					}
					if (identifier == null)
					{
						if (useIdentifiedReferenceMode)
						{
							outputText.Append(rolePlayer.ReferenceModeDecoratedString);
							identifierWritten = true;
						}
					}
					else if (nestedFactType == null || !identifier.IsObjectifiedPreferredIdentifier)
					{
						LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
						int identifierRoleCount = identifierRoles.Count;
						string refModeString;
						if (identifier.IsInternal &&
							identifierRoleCount == 1 &&
							!string.IsNullOrEmpty(refModeString = rolePlayer.ReferenceModeDecoratedString))
						{
							outputText.Append(refModeString);
						}
						else
						{
							for (int i = 0; i < identifierRoleCount; ++i)
							{
								Role identifierRole = identifierRoles[i];
								if (i != 0)
								{
									outputText.Append(listSeparator);
								}
								RecurseColumnIdentifier(identifierRole, null, false, listSeparator, ref outputText);
							}
						}
						identifierWritten = true;
					}
				}
				if (nestedFactType != null)
				{
					IList<RoleBase> factRoles = nestedFactType.OrderedRoleCollection;
					int factRoleCount = factRoles.Count;
					if (listSeparator == null && (identifierWritten || factRoleCount > 1))
					{
						listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
					}
					if (identifierWritten)
					{
						outputText.Append(listSeparator);
					}
					for (int i = 0; i < factRoleCount; ++i)
					{
						Role factRole = factRoles[i].Role;
						if (i != 0)
						{
							outputText.Append(listSeparator);
						}
						RecurseColumnIdentifier(factRole, null, false, listSeparator, ref outputText);
					}
				}
				outputText.Append(")");
				return null;
			}
			/// <summary>
			/// Determine if the specified instance is empty. A <see cref="ValueTypeInstance"/>
			/// is never empty, an <see cref="EntityTypeInstance"/> is empty if it has not
			/// attached instances, and a <see cref="EntityTypeSubtypeInstance"/> is empty if
			/// its <see cref="P:EntityTypeSubtypeInstance.SupertypeInstance"/> is empty
			/// </summary>
			public static bool IsEmptyInstance(ObjectTypeInstance objectInstance)
			{
				EntityTypeSubtypeInstance subtypeInstance = objectInstance as EntityTypeSubtypeInstance;
				EntityTypeInstance entityInstance = (subtypeInstance != null) ? subtypeInstance.SupertypeInstance : objectInstance as EntityTypeInstance;
				return entityInstance == null || entityInstance.RoleInstanceCollection.Count == 0;
			}
			/// <summary>
			/// Given an <see cref="ObjectType"/> that is a subtype without an explicit
			/// preferred identifier, find some <see cref="SupertypeMetaRole"/>
			/// attached to the identifying SuperType.
			/// </summary>
			public static Role GetIdentifyingSupertypeRole(ObjectType subtype)
			{
				Role retVal = null;
				ObjectType.WalkSupertypeRelationships(
					subtype,
					delegate(SubtypeFact subtypeFact, ObjectType type, int depth)
					{
						if (subtypeFact.ProvidesPreferredIdentifier)
						{
							if (type.PreferredIdentifier != null)
							{
								retVal = subtypeFact.SupertypeRole;
								return ObjectTypeVisitorResult.Stop;
							}
							// All we need is one identifying path, there is no reason
							// to find an additional one.
							return ObjectTypeVisitorResult.SkipFollowingSiblings;
						}
						else
						{
							return ObjectTypeVisitorResult.SkipChildren;
						}
					});
				return retVal;
			}
			/// <summary>
			/// Given an <see cref="ObjectType"/> that is a subtype without an explicit
			/// preferred identifier, find some <see cref="SubtypeMetaRole"/>
			/// attached to the Subtype.
			/// </summary>
			public static Role GetPreferredSubtypeRole(ObjectType subtype)
			{
				Role retVal = null;
				ObjectType.WalkSupertypeRelationships(
					subtype,
					delegate(SubtypeFact subtypeFact, ObjectType type, int depth)
					{
						if (subtypeFact.ProvidesPreferredIdentifier)
						{
							retVal = subtypeFact.SubtypeRole;
							return ObjectTypeVisitorResult.Stop;
						}
						return ObjectTypeVisitorResult.SkipChildren;
					});
				return retVal;
			}
			/// <summary>
			/// Return true if a selection for the specified column in this
			/// branch should select the full row
			/// </summary>
			public virtual bool IsFullRowSelectColumn(int column)
			{
				return column == (int)SpecialColumnIndex.FullRowSelectColumn;
			}
			/// <summary>
			/// A helper function to safely attach a <paramref name="factInstance"/> to the external <paramref name="identifierInstance"/>
			/// that identifies the objectified <see cref="FactType"/> population.
			/// </summary>
			/// <param name="factInstance">The <see cref="FactTypeInstance"/> associated with an objectified <see cref="FactType"/></param>
			/// <param name="identifierInstance">The <see cref="ObjectTypeInstance"/> associated with the objectifying type that identifies this <paramref name="factInstance"/></param>
			/// <remarks>An identifier can be moved from one FactTypeInstance to another if the original instance has no population. If
			/// the specified <paramref name="factInstance"/> has an empty identifier, then all references to the empty identifier are
			/// transferred to the new identifier. This facilititates the creation of empty placeholder identifiers that to provide
			/// a means of referencing an objectified instance.</remarks>
			/// <exception cref="InvalidOperationException">Is thrown if the identifier is already attached to another non-empty FactTypeInstance</exception>
			protected static void ConnectObjectifyingIdentifierInstance(FactTypeInstance factInstance, ObjectTypeInstance identifierInstance)
			{
				ObjectificationInstance existingInstanceLink;
				// Make sure that a differed objectified instance already attached to the
				// parent instance can be safely abandoned
				FactTypeInstance existingObjectifiedInstance;
				if (null != (existingObjectifiedInstance = identifierInstance.ObjectifiedInstance))
				{
					if (existingObjectifiedInstance == factInstance)
					{
						// The instances are already connected, there is nothing else to do
						return;
					}
					if (existingObjectifiedInstance.RoleInstanceCollection.Count == 0)
					{
						// The picker offers identifier instances attached to empty FactType instances
						// Make sure these are cleared so the 1-1 enforcement doesn't throw. We revalidate
						// this condition to allow the same enforcement from the text-editing facilities.
						identifierInstance.ObjectifiedInstance = null;
					}
					else
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelSamplePopulationEditorObjectifyingIdentifierAlreadyUsedExceptionText, identifierInstance.ToString()));
					}
				}
				if (null != (existingInstanceLink = ObjectificationInstance.GetLinkToObjectifyingInstance(factInstance)))
				{
					// If the current identifier identifier is empty, meaning that it has no attached role instances, then
					// it instance exists solely for the purpose of relating objectified FactType information to other instances.
					// In this case, we want to abandon the empty identifier to be deleted by the rules engine, but only after we
					// move all referencing links to the new instance.
					ObjectTypeInstance objectifyingInstance = existingInstanceLink.ObjectifyingInstance;
					EntityTypeSubtypeInstance objectifyingSubtypeInstance = objectifyingInstance as EntityTypeSubtypeInstance;
					EntityTypeInstance objectifyingEntityInstance = (null != objectifyingSubtypeInstance) ? objectifyingSubtypeInstance.SupertypeInstance : (EntityTypeInstance)objectifyingInstance;
					if (objectifyingEntityInstance.RoleInstanceCollection.Count == 0)
					{
						try
						{
							EntityTypeSubtypeInstance identifierSubtypeInstance = identifierInstance as EntityTypeSubtypeInstance;
							EntityTypeInstance identifierEntityInstance = (null == identifierSubtypeInstance) ? (EntityTypeInstance)identifierInstance : null; // We don't care about the supertype instance
							Debug.Assert((objectifyingSubtypeInstance == null) == (identifierSubtypeInstance == null)); // The old and new instance types should always match
							if (identifierEntityInstance != null)
							{
								foreach (EntityTypeSubtypeInstanceHasSupertypeInstance supertypeLink in EntityTypeSubtypeInstanceHasSupertypeInstance.GetLinksToEntityTypeSubtypeInstanceCollection(objectifyingEntityInstance))
								{
									supertypeLink.SupertypeInstance = identifierEntityInstance;
								}
							}
							foreach (RoleInstance roleInstance in RoleInstance.GetLinksToRoleCollection(objectifyingInstance))
							{

#if ROLEINSTANCE_ROLEPLAYERCHANGE
								link.ObjectTypeInstance = resultEntityInstance;
#else
								FactTypeRoleInstance factRoleInstance;
								if (null != (factRoleInstance = roleInstance as FactTypeRoleInstance))
								{
									FactTypeInstance reattachFactInstance = factRoleInstance.FactTypeInstance;
									roleInstance.Delete();
									new FactTypeRoleInstance(roleInstance.Role, identifierInstance).FactTypeInstance = reattachFactInstance;
								}
								else
								{
									EntityTypeInstance reattachEntityInstance = ((EntityTypeRoleInstance)roleInstance).EntityTypeInstance;
									roleInstance.Delete();
									new EntityTypeRoleInstance(roleInstance.Role, identifierInstance).EntityTypeInstance = reattachEntityInstance;
								}
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
							}
						}
						catch (InvalidOperationException)
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelSamplePopulationEditorObjectifyingIdentifierRelationshipConflictsExceptionText, identifierInstance.Name));
						}
					}
					existingInstanceLink.ObjectifyingInstance = identifierInstance;
				}
				else
				{
					factInstance.ObjectifyingInstance = identifierInstance;
				}
			}
			protected static ObjectTypeInstance RecurseValueTypeInstance(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string newText, ref ValueTypeInstance rootInstance, bool create, bool allowValueEdit)
			{
				if (parentType.IsValueType)
				{
					DataType valueDataType = parentType.DataType;
					if (create && valueDataType.CanCompare)
					{
						bool canParseAnyValue = valueDataType.CanParseAnyValue;
						LinkedElementCollection<ValueTypeInstance> instances = parentType.ValueTypeInstanceCollection;
						int instanceCount = instances.Count;
						for (int i = 0; i < instanceCount; ++i)
						{
							ValueTypeInstance currentValueInstance = instances[i];
							string value = currentValueInstance.Value;
							if (canParseAnyValue ||
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
					if (!create)
					{
						return objectTypeInstance;
					}
					Debug.Assert(parentType.Store.TransactionActive, "Transaction must be active to create new instances");
					ValueTypeInstance editValueTypeInstance = objectTypeInstance as ValueTypeInstance;
					if (editValueTypeInstance != null)
					{
						if (allowValueEdit && RoleInstance.GetLinksToRoleCollection(editValueTypeInstance).Count <= 1)
						{
							editValueTypeInstance.Value = newText;
						}
						else
						{
							editValueTypeInstance = AddAndInitializeValueTypeInstance(newText, parentType);
						}
					}
					else
					{
						editValueTypeInstance = AddAndInitializeValueTypeInstance(newText, parentType);
					}
					rootInstance = editValueTypeInstance;
					return editValueTypeInstance;
				}
				else
				{
					LinkedElementCollection<Role> identifierRoles = parentType.PreferredIdentifier.RoleCollection;
					Debug.Assert(identifierRoles.Count == 1);
					Role identifierRole = identifierRoles[0];
					EntityTypeInstance editEntityInstance = objectTypeInstance as EntityTypeInstance;
					// Note that we don't offer direct text editing of subtype instances, so is either a ValueTypeInstance
					// of an EntityTypeInstance.
					EntityTypeRoleInstance editingRoleInstance = null;
					bool recurseAllowValueEdit = create;
					if (editEntityInstance != null)
					{
						editingRoleInstance = editEntityInstance.FindRoleInstance(identifierRole);
						if (recurseAllowValueEdit)
						{
							// Before recursing and potentially allowing an edit to the underlying value instance,
							// we first need to check this parent instance is itself used multiple times.
							// The scenario we want to protect against is:
							// Person(.name) has Gender(.code)
							// 1) Enter F in two gender fields with the 'Person has Gender' FactType selected
							// 2) Editing the second F to be an M needs to create a new instance, not edit the single existing instance
							int maxLeft = editEntityInstance.ObjectifiedInstance == null ? 0 : 1;
							int setCount = RoleInstance.GetLinksToRoleCollection(editEntityInstance).Count;
							if (setCount > maxLeft)
							{
								recurseAllowValueEdit = false;
							}
							else
							{
								maxLeft -= setCount;
								recurseAllowValueEdit = editEntityInstance.EntityTypeSubtypeInstanceCollection.Count <= maxLeft;
							}
						}
					}
					ObjectTypeInstance objectInstance = RecurseValueTypeInstance(
						(editingRoleInstance != null) ? editingRoleInstance.ObjectTypeInstance : null,
						identifierRole.RolePlayer,
						newText,
						ref rootInstance,
						create,
						recurseAllowValueEdit);
					LinkedElementCollection<EntityTypeInstance> instances = parentType.EntityTypeInstanceCollection;
					int instanceCount = instances.Count;
					for(int i = 0; i < instanceCount; ++i)
					{
						EntityTypeInstance instance = instances[i];
						LinkedElementCollection<EntityTypeRoleInstance> roleInstances = instance.RoleInstanceCollection;
						if(roleInstances.Count == 1 && roleInstances[0].ObjectTypeInstance == objectInstance)
						{
							return instance;
						}
					}
					if (create)
					{
						if (editEntityInstance == null || editEntityInstance.RoleInstanceCollection.Count != 0)
						{
							editEntityInstance = new EntityTypeInstance(parentType.Store);
							editEntityInstance.EntityType = parentType;
						}
						new EntityTypeRoleInstance(identifierRole, objectInstance).EntityTypeInstance = editEntityInstance;
					}
					return editEntityInstance;
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
					for (int i = 1; i < columnCount; ++i)
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
				EditColumnHeader(column, false, newRole);
			}

			protected void EditColumnHeader(int column, bool ignoreObjectification, Role newRole)
			{
				TreeControl.UpdateColumnHeaderAppearance(column, DeriveColumnName(newRole, ignoreObjectification), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(newRole.RolePlayer, ignoreObjectification));
			}

			protected void EditColumnHeader(int column, bool ignoreObjectification, ObjectType objectType)
			{
				TreeControl.UpdateColumnHeaderAppearance(column, DeriveColumnName(objectType, ignoreObjectification), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(objectType, ignoreObjectification));
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

			protected static ValueTypeInstance AddAndInitializeValueTypeInstance(string newText, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ValueTypeInstance newInstance = new ValueTypeInstance(parentValueType.Store);
				newInstance.ValueType = parentValueType;
				newInstance.Value = newText;
				return newInstance;
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
			/// <summary>
			/// Provide an entry point for deleting part of an instance.
			/// </summary>
			/// <remarks>If <see cref="UsesEditContext"/> is false then the
			/// derived branch must explicitly override this method.</remarks>
			/// <param name="row">The row to delete</param>
			/// <param name="column">The column to delete</param>
			public virtual void DeleteInstancePart(int row, int column)
			{
				Debug.Assert(UsesEditContext);
				CellEditContext editContext = CreateEditContext(row, column);
				if (editContext != null)
				{
					CellEditContext.Descriptor.SetValue(editContext, null);
				}
			}
			/// <summary>
			/// Return true if the branch creates an edit context, false
			/// if it deletes directly.
			/// </summary>
			protected virtual bool UsesEditContext
			{
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Create an editing context for the specified entry. Provides shared
			/// code to allow for consistent behavior between selecting 'Unspecified'
			/// in the editing dropdown and executing the delete command without the editor.
			/// </summary>
			/// <remarks><see cref="UsesEditContext"/> must return false, or an override
			/// is required for this method.</remarks>
			/// <param name="row">The row to create a context for</param>
			/// <param name="column">The column to create a context for.</param>
			/// <returns><see cref="CellEditContext"/> or <see langword="null"/> if
			/// a context is not available.</returns>
			protected virtual CellEditContext CreateEditContext(int row, int column)
			{
				Debug.Assert(!UsesEditContext);
				return null;
			}
			#endregion // Branch Update Methods
		}
		private sealed class ValueTypeBranch : BaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly List<ValueTypeInstance> myCachedInstances;
			private readonly ObjectType myValueType;
			#endregion
			#region Construction
			// Value Type Branches will always have 1 column, plus the full row select column
			public ValueTypeBranch(ObjectType selectedValueType) : base(2, selectedValueType.Store)
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
				ObjectType valueType = myValueType;
				if (isNewRow)
				{
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, valueType.Name)))
					{
						AddAndInitializeValueTypeInstance(newText, valueType);
						t.Commit();
					}
				}
				else if (textIsEmpty)
				{
					ValueTypeInstance valueInstance = valueType.ValueTypeInstanceCollection[row];
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, valueType.Name, valueInstance.Name)))
					{
						valueInstance.Delete();
						t.Commit();
					}
				}
				else
				{
					ValueTypeInstance valueInstance = valueType.ValueTypeInstanceCollection[row];
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, valueType.Name, valueInstance.Name)))
					{
						valueInstance.Value = newText;
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
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = base.GetDisplayData(row, column, requiredData);
				if (!base.IsFullRowSelectColumn(column))
				{
					retVal.SelectedImage = retVal.Image = (short)InstanceTypeImageIndex.ValueType;
				}
				return retVal;
			}

			private new int VisibleItemCount
			{
				get
				{
					return myCachedInstances.Count + BaseBranch.VisibleItemCount;
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
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
					{
						instances.RemoveAt(instanceLocation);
						base.RemoveInstanceDisplay(instanceLocation);
					}
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
			#region Base overrides
			/// <summary>
			/// Provide custom code for part deletion and always
			/// allow a label edit for data cells
			/// </summary>
			protected override bool UsesEditContext
			{
				get
				{
					return false;
				}
			}
			/// <summary>
			/// Delete the instance part by deferring to CommitLabelEdit
			/// </summary>
			public override void DeleteInstancePart(int row, int column)
			{
				if (!IsFullRowSelectColumn(column))
				{
					CommitLabelEdit(row, column, "");
				}
			}
			public sealed override void DeleteInstance(int row, int column)
			{
				if(base.IsFullRowSelectColumn(column) && row < myCachedInstances.Count)
				{
					ValueTypeInstance valueInstance = myValueType.ValueTypeInstanceCollection[row];
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, myValueType.Name, valueInstance.Name)))
					{
						valueInstance.Delete();
						t.Commit();
					}
				}
			}
			#endregion // Base overrides
		}
		private sealed class EntityTypeBranch : BaseBranch, IBranch, IMultiColumnBranch, IUnattachedBranchOwner
		{
			#region Member Variables
			/// <summary>
			/// Cached instances, including empty and non-empty instances.
			/// Empty instances are at the end of the list and are not displayed.
			/// We need to track empty instances as well as displayed instances
			/// so that a new instance, which will correspond to the 'new' row
			/// in the list, can be distinguished from a modification of an
			/// empty instance.
			/// </summary>
			private readonly List<ObjectTypeInstance> myCachedInstances;
			/// <summary>
			/// The count of known instances in the cache that are not currently empty.
			/// </summary>
			private int myNonEmptyInstanceCount;
			private readonly ObjectType myEntityType;
			private readonly ObjectType myEntityTypeSubtype;
			/// <summary>
			/// Expansion branches from the 'new' row that need to
			/// be attached and notified when a new instance is added.
			/// The zero index in this array is the first item column,
			/// not the dummy row number column.
			/// </summary>
			private IUnattachedBranch[] myUnattachedBranches;
			#endregion
			#region Construction
			public EntityTypeBranch(ObjectType entityType, int numColumns)
				: base(numColumns, entityType.Store)
			{
				Debug.Assert(!entityType.IsValueType);
				UniquenessConstraint preferredIdentifier = entityType.ResolvedPreferredIdentifier;
				ObjectType preferredFor = preferredIdentifier.PreferredIdentifierFor;
				if (preferredFor == entityType)
				{
					myEntityType = entityType;
				}
				else
				{
					myEntityTypeSubtype = entityType;
					myEntityType = preferredFor;
				}
				LinkedElementCollection<ObjectTypeInstance> instances = entityType.ObjectTypeInstanceCollection;
				int instanceCount = instances.Count;
				List<ObjectTypeInstance> instanceCache;
				if (instanceCount == 0)
				{
					instanceCache = new List<ObjectTypeInstance>();
				}
				else
				{
					instanceCache = new List<ObjectTypeInstance>(instanceCount);
					int lastEmptyIndex = instanceCount;
					int nonEmptyCount = 0;
					for (int i = 0; i < instanceCount; ++i)
					{
						ObjectTypeInstance instance = instances[i];
						if (IsEmptyInstance(instance))
						{
							if (lastEmptyIndex == instanceCount)
							{
								// Fill the list from this point with null values so that we
								// can index the last item
								for (int j = nonEmptyCount; j < instanceCount; ++j)
								{
									instanceCache.Add(null);
								}
							}
							instanceCache[--lastEmptyIndex] = instance;
						}
						else
						{
							if (lastEmptyIndex == instanceCount)
							{
								// We haven't null filled, just add
								instanceCache.Add(instance);
							}
							else
							{
								instanceCache[nonEmptyCount] = instance;
							}
							++nonEmptyCount;
						}
					}
					myNonEmptyInstanceCount = nonEmptyCount;
				}
				myCachedInstances = instanceCache;
			}
			#endregion
			#region IBranch Interface Members
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				bool delete = newText.Length == 0;
				Store store = Store;
				ObjectType selectedEntityType = myEntityType;
				ObjectType selectedEntityTypeSubtype = myEntityTypeSubtype;
				// If editing an existing EntityTypeInstance
				if (row != NewRowIndex)
				{
					EntityTypeInstance editInstance;
					EntityTypeSubtypeInstance editSubtypeInstance;
					if (selectedEntityTypeSubtype != null)
					{
						editSubtypeInstance = (EntityTypeSubtypeInstance)myCachedInstances[row];
						editInstance = editSubtypeInstance.SupertypeInstance;
					}
					else
					{
						editInstance = (EntityTypeInstance)myCachedInstances[row];
						editSubtypeInstance = null;
					}
					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
					IList<EntityTypeRoleInstance> editRoleInstances = editInstance.RoleInstanceCollection;
					EntityTypeRoleInstance editRoleInstance = EntityTypeInstance.FindRoleInstance(editRoleInstances, identifierRole);
					// If editing an existing EntityTypeRoleInstance
					if (editRoleInstance != null)
					{
						if (delete)
						{
							ObjectTypeInstance deleteInstance = editRoleInstance.ObjectTypeInstance;
							ValueTypeInstance deleteValueInstance = null;
							if (identifierRole.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
							{
								// If we delete the role instance then a new instance will be created automatically because of
								// the implied population pattern. To avoid this situation, first check if we have a ValueType
								// and if the ValueType instance is playing no other roles. This is the standard popular reference
								// mode pattern. If this is the case, then we delete the opposite ValueType instance. If this is
								// not the case, then we should not proceed with the deletion.
								deleteValueInstance = deleteInstance as ValueTypeInstance;
								if (deleteValueInstance == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
								{
									throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
								}
							}
							using (Transaction t = store.TransactionManager.BeginTransaction(
								(editRoleInstances.Count == 1) ?
									string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, selectedEntityType.Name, deleteInstance.Name) :
									string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(identifierRole, false), deleteInstance.Name)))
							{
								if (deleteValueInstance != null)
								{
									deleteValueInstance.Delete();
								}
								else
								{
									editRoleInstance.Delete();
								}
								t.Commit();
							}
						}
						else
						{
							ObjectType editRolePlayer = editRoleInstance.Role.RolePlayer;
							ObjectTypeInstance objectInstance = editRoleInstance.ObjectTypeInstance;
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, editRolePlayer.Name, objectInstance.Name)))
							{
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(objectInstance, editRolePlayer, newText, ref instance, true, true);
								ConnectInstance(myEntityType, myEntityTypeSubtype, ref editInstance, ref editSubtypeInstance, result, identifierRole);
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					// If editing an existing EntityTypeInstance but creating a new EntityTypeRoleInstance
					else if (!delete)
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedEntityType.Name)))
						{
							ValueTypeInstance instance = null;
							ObjectTypeInstance result = RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref instance, true, true);
							ConnectInstance(myEntityType, myEntityTypeSubtype, ref editInstance, ref editSubtypeInstance, result, identifierRole);
							t.Commit();
						}
						return LabelEditResult.AcceptEdit;
					}
				}
				// New Row Editing
				else if (!delete)
				{
					using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, myEntityType.Name)))
					{
						Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
						ValueTypeInstance instance = null;
						ObjectTypeInstance result = RecurseValueTypeInstance(null, identifierRole.RolePlayer, newText, ref instance, true, true);
						EntityTypeInstance parentInstance = null;
						EntityTypeSubtypeInstance parentSubtypeInstance = null;
						ConnectInstance(selectedEntityType, selectedEntityTypeSubtype, ref parentInstance, ref parentSubtypeInstance, result, identifierRole);
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
					ObjectType selectedEntityTypeSubtype = myEntityTypeSubtype;
					EntityEditorBranch expandedBranch;
					ObjectTypeInstance parentInstance = null;
					if (selectedEntityTypeSubtype != null)
					{
						expandedBranch = new EntityEditorBranch(
							null,
							selectedEntityTypeSubtype,
							parentInstance = (row < myNonEmptyInstanceCount ? (EntityTypeSubtypeInstance)myCachedInstances[row] : null),
							GetPreferredSubtypeRole(selectedEntityTypeSubtype),
							this);
					}
					else
					{
						ObjectType selectedEntityType = myEntityType;
						ObjectTypeInstance editInstance = null;
						Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
						EntityTypeInstance parentEntityInstance = null;
						if (row < myNonEmptyInstanceCount)
						{
							parentEntityInstance = (EntityTypeInstance)myCachedInstances[row];
							EntityTypeRoleInstance foundRoleInstance = parentEntityInstance.FindRoleInstance(identifierRole);
							if (foundRoleInstance != null)
							{
								editInstance = foundRoleInstance.ObjectTypeInstance;
								Debug.Assert(editInstance != null);
							}
						}
						expandedBranch = new EntityEditorBranch(parentEntityInstance, selectedEntityType, editInstance, identifierRole, this);
					}
					if (parentInstance == null)
					{
						IUnattachedBranch[] branches = myUnattachedBranches ?? (myUnattachedBranches = new IUnattachedBranch[ColumnCount - 1]);
						branches[column - 1] = expandedBranch;
					}
					return expandedBranch;
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null)
				{
					text = ObjectTypeInstance.GetDisplayString(
						null,
						(myEntityTypeSubtype != null) ?
							myEntityType :
							myEntityType.ResolvedPreferredIdentifier.RoleCollection[column - 1].RolePlayer,
						false);
				}
				else if (text.Length == 0)
				{
					if (myEntityTypeSubtype != null)
					{
						EntityTypeSubtypeInstance subtypeInstance = (EntityTypeSubtypeInstance)myCachedInstances[row];
						text = (subtypeInstance != null) ? subtypeInstance.Name : ObjectTypeInstance.GetDisplayString(null, myEntityType, false);
					}
					else
					{
						ObjectType selectedEntityType = myEntityType;
						EntityTypeInstance selectedInstance = (EntityTypeInstance)myCachedInstances[row];
						Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
						EntityTypeRoleInstance roleInstance = selectedInstance.FindRoleInstance(identifierRole);
						text = (roleInstance != null) ? roleInstance.ObjectTypeInstance.Name : ObjectTypeInstance.GetDisplayString(null, identifierRole.RolePlayer, false);
					}
				}
				return text;
			}

			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.Icon)
				{
					return TreeControl.GetColumnHeader(column).Text;
				}
				return null;
			}

			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = base.GetDisplayData(row, column, requiredData);
				if (!base.IsFullRowSelectColumn(column))
				{
					retVal.SelectedImage = retVal.Image = (short)TreeControl.GetColumnHeader(column).ImageIndex;
				}
				return retVal;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				if (!base.IsFullRowSelectColumn(column))
				{
					return myEntityTypeSubtype != null || HasComplexIdentifier(myEntityType.ResolvedPreferredIdentifier.RoleCollection[column - 1].RolePlayer);
				}
				return false;
			}
			private new int VisibleItemCount
			{
				get
				{
					return myNonEmptyInstanceCount + BaseBranch.VisibleItemCount;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				if (style == ErrorObject)
				{
					TooFewEntityTypeRoleInstancesError partialPopulationError;
					EntityTypeInstance entityInstance;
					int row = -1;
					int column = 0;
					if (null != (partialPopulationError = obj as TooFewEntityTypeRoleInstancesError))
					{
						entityInstance = partialPopulationError.EntityTypeInstance;
						ObjectType entityType;
						if ((entityType = entityInstance.EntityType) == myEntityType &&
							-1 != (row = myCachedInstances.IndexOf(entityInstance)) &&
							row < myNonEmptyInstanceCount)
						{
							IList<Role> identifierRoles = entityType.PreferredIdentifier.RoleCollection;
							IList<EntityTypeRoleInstance> roleInstances = entityInstance.RoleInstanceCollection;
							int roleCount = identifierRoles.Count;
							for (int i = 0; i < roleCount; ++i)
							{
								if (null == EntityTypeInstance.FindRoleInstance(roleInstances, identifierRoles[i]))
								{
									column = i + 1;
									break;
								}
							}
						}
					}
					if (row != -1)
					{
						return new LocateObjectData(row, column, (int)TrackingObjectAction.ThisLevel);
					}
				}
				else if (style == ObjectStyle.TrackingObject)
				{
					int row;
					EntityTypeInstance entityInstance;
					ObjectType entityType;
					if (null != (entityInstance = obj as EntityTypeInstance) &&
						(entityType = entityInstance.EntityType) == myEntityType &&
						-1 != (row = myCachedInstances.IndexOf(entityInstance)) &&
						row < myNonEmptyInstanceCount)
					{
						return new LocateObjectData(row, 0, (int)TrackingObjectAction.ThisLevel);
					}
				}
				return new LocateObjectData();
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
				classInfo = dataDirectory.FindDomainRelationship(ObjectTypeHasObjectTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(InstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(InstanceRemovedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeInstanceNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
				propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);

				// Handlers to manage empty instances based on the role content
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeRoleInstanceRemovedEvent), action);

				eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), new EventHandler<RolePlayerChangedEventArgs>(SubtypeInstanceSupertypeRolePlayerChangedEvent), action);
			}

			private void InstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectTypeHasObjectTypeInstance link = (ObjectTypeHasObjectTypeInstance)e.ModelElement;
				if (!link.IsDeleted)
				{
					ObjectType linkEntityType = link.ObjectType;
					if (linkEntityType == (myEntityTypeSubtype ?? myEntityType) && myEntityType.PreferredIdentifier != null)
					{
						TestNotifyAddInstance(link.ObjectTypeInstance);
					}
				}
			}

			private void InstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectTypeHasObjectTypeInstance link = (ObjectTypeHasObjectTypeInstance)e.ModelElement;
				ObjectType entityType = link.ObjectType;
				if (!entityType.IsDeleted && entityType == (myEntityTypeSubtype ?? myEntityType) && myEntityType.PreferredIdentifier != null)
				{
					List<ObjectTypeInstance> instances = myCachedInstances;
					int instanceLocation = instances.IndexOf(link.ObjectTypeInstance);
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction, or with an empty instance
					{
						instances.RemoveAt(instanceLocation);
						if (instanceLocation < myNonEmptyInstanceCount)
						{
							--myNonEmptyInstanceCount;
							base.RemoveInstanceDisplay(instanceLocation);
						}
					}
				}
			}

			private void ObjectTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectTypeInstance objectInstance = (ObjectTypeInstance)e.ModelElement;
				if (!objectInstance.IsDeleted &&
					objectInstance.ObjectType == (myEntityTypeSubtype ?? myEntityType))
				{
					int instanceLocation = myCachedInstances.IndexOf(objectInstance);
					if (instanceLocation != -1 && instanceLocation < myNonEmptyInstanceCount)
					{
						base.EditInstanceDisplay(instanceLocation);
					}
				}
			}

			private void RoleNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				Role role = (Role)e.ModelElement;
				if (!role.IsDeleted && IsPartOfDisplayedIdentifier(myEntityTypeSubtype ?? myEntityType, role))
				{
					UpdateColumnHeaders();
				}
			}

			private void ObjectTypeNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectType objectType = (ObjectType)e.ModelElement;
				if (!objectType.IsDeleted && IsPartOfDisplayedIdentifier(myEntityTypeSubtype ?? myEntityType, objectType))
				{
					UpdateColumnHeaders();
				}
			}
			private void UpdateColumnHeaders()
			{
				UniquenessConstraint pid;
				ObjectType entityTypeSubtype = myEntityTypeSubtype;
				ObjectType entityType = myEntityType;
				if (entityTypeSubtype != null)
				{
					base.EditColumnHeader(1, false, entityTypeSubtype);
				}
				else if (null != (pid = entityType.PreferredIdentifier))
				{
					LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
					int pidRoleCount = pidRoles.Count;
					for (int i = 0; i < pidRoleCount; ++i)
					{
						base.EditColumnHeader(i + 1, pidRoles[i]);
					}
				}
			}
			private void EntityTypeRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = (EntityTypeInstanceHasRoleInstance)e.ModelElement;
				EntityTypeInstance entityInstance;
				if (!link.IsDeleted &&
					!(entityInstance = link.EntityTypeInstance).IsDeleted &&
					entityInstance.EntityType == myEntityType)
				{
					// Make sure we have the appropriate instance
					ObjectType testSubtype = myEntityTypeSubtype;
					ObjectTypeInstance verifyInstance = (testSubtype == null) ? (ObjectTypeInstance)entityInstance : EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, testSubtype, true, false);
					if (verifyInstance != null)
					{
						TestNotifyAddInstance(verifyInstance);
					}
				}
			}
			private void EntityTypeRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = (EntityTypeInstanceHasRoleInstance)e.ModelElement;
				EntityTypeInstance entityInstance;
				if (!(entityInstance = link.EntityTypeInstance).IsDeleted &&
					entityInstance.EntityType == myEntityType &&
					entityInstance.RoleInstanceCollection.Count == 0)
				{
					ObjectType testSubtype = myEntityTypeSubtype;
					ObjectTypeInstance verifyInstance = (testSubtype == null) ? (ObjectTypeInstance)entityInstance : EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, testSubtype, true, false);
					if (verifyInstance != null)
					{
						List<ObjectTypeInstance> instances = myCachedInstances;
						int instanceLocation = instances.IndexOf(verifyInstance);
						if (instanceLocation != -1 && instanceLocation < myNonEmptyInstanceCount)
						{
							instances.RemoveAt(instanceLocation);
							--myNonEmptyInstanceCount;
							base.RemoveInstanceDisplay(instanceLocation);
							instances.Add(verifyInstance); // The instance is still alive, just empty
						}
					}
				}
			}
			private void SubtypeInstanceSupertypeRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ElementLink;
				EntityTypeSubtypeInstance subtypeInstance;
				if (!link.IsDeleted &&
					!(subtypeInstance = link.EntityTypeSubtypeInstance).IsDeleted &&
					subtypeInstance.EntityTypeSubtype == myEntityTypeSubtype)
				{
					if (IsEmptyInstance(subtypeInstance))
					{
						List<ObjectTypeInstance> instances = myCachedInstances;
						int instanceLocation = instances.IndexOf(subtypeInstance);
						if (instanceLocation != -1 && instanceLocation < myNonEmptyInstanceCount)
						{
							instances.RemoveAt(instanceLocation);
							--myNonEmptyInstanceCount;
							base.RemoveInstanceDisplay(instanceLocation);
							instances.Add(subtypeInstance); // The instance is still alive, just empty
						}
					}
					else
					{
						TestNotifyAddInstance(subtypeInstance);
					}
				}
			}
			#endregion // Event Handlers
			#region IUnattachedBranchOwner Implementation
			bool IUnattachedBranchOwner.TryAnchorUnattachedBranches(ObjectTypeInstance objectInstance, FactTypeInstance factInstance)
			{
				if (objectInstance != null)
				{
					return TestNotifyAddInstance(objectInstance);
				}
				return false;
			}
			#endregion // IUnattachedBranchOwner Implementation
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
			/// The entity type subtype used to create this branch
			/// </summary>
			public ObjectType EntityTypeSubtype
			{
				get
				{
					return myEntityTypeSubtype;
				}
			}
			/// <summary>
			/// Connect a given instance to the specified entity type, for the given role
			/// </summary>
			/// <param name="entityType">The parent entity. Cannot be null.</param>
			/// <param name="entityTypeSubtype">A subtype of the current <paramref name="entityType"/>. Can be null.</param>
			/// <param name="parentInstance">Instance to connect to. Created if needed.</param>
			/// <param name="parentSubtypeInstance">Subtype instance to connect to. Created if needed.</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect to</param>
			public static void ConnectInstance(ObjectType entityType, ObjectType entityTypeSubtype, ref EntityTypeInstance parentInstance, ref EntityTypeSubtypeInstance parentSubtypeInstance, ObjectTypeInstance connectInstance, Role identifierRole)
			{
				Debug.Assert(entityType != null);
				Debug.Assert(connectInstance != null);
				Store store = entityType.Store;

				if (parentInstance == null)
				{
					if (identifierRole is SubtypeMetaRole)
					{
						// Signal to indicate a subtype instance-only connection, there is nothing to do
						return;
					}
					if (null != entityTypeSubtype)
					{
						EntityTypeInstance supertypeInstance;
						EntityTypeSubtypeInstance subtypeInstance;
						if (null != (subtypeInstance = connectInstance as EntityTypeSubtypeInstance))
						{
							parentSubtypeInstance = subtypeInstance;
							parentInstance = subtypeInstance.SupertypeInstance;
							return;
						}
						else if (null != (supertypeInstance = connectInstance as EntityTypeInstance))
						{
							parentInstance = supertypeInstance;
							parentSubtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(supertypeInstance, entityTypeSubtype, false, true);
							return;
						}
					}
					switch (identifierRole.GetReferenceSchemePattern())
					{
						case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
						case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
							// The FactType patterns are one-to-one and implied. We should never
							// create another instance if one is already available.
							foreach (EntityTypeRoleInstance testRoleInstance in EntityTypeRoleInstance.GetLinksToRoleCollection(connectInstance))
							{
								EntityTypeInstance testEntityInstance = testRoleInstance.EntityTypeInstance;
								if (testEntityInstance.EntityType == entityType)
								{
									parentInstance = testEntityInstance;
									if (entityTypeSubtype != null)
									{
										parentSubtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(testEntityInstance, entityTypeSubtype, true, false);
									}
									break;
								}
							}
							break;
					}
					if (parentInstance == null)
					{
						parentInstance = new EntityTypeInstance(store);
						parentInstance.EntityType = entityType;
						new EntityTypeRoleInstance(identifierRole, connectInstance).EntityTypeInstance = parentInstance;
					}
					if (entityTypeSubtype != null && parentSubtypeInstance == null)
					{
						parentSubtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(parentInstance, entityTypeSubtype, false, true);
					}
				}
				else if (parentSubtypeInstance != null)
				{
					parentSubtypeInstance.SupertypeInstance = (EntityTypeInstance)connectInstance;
				}
				else
				{
					parentInstance.EnsureRoleInstance(identifierRole, connectInstance);
				}
			}
			/// <summary>
			/// See if a <see cref="ObjectTypeInstance"/> needs to be added.
			/// Anchor unattached branches and notify as needed.
			/// </summary>
			/// <param name="objectInstance">The instance to attach</param>
			/// <returns>Returns true if the new instance was previously unknown and is not empty.</returns>
			private bool TestNotifyAddInstance(ObjectTypeInstance objectInstance)
			{
				List<ObjectTypeInstance> instances = myCachedInstances;
				int instanceCount = instances.Count;
				int instanceLocation = instances.IndexOf(objectInstance);
				bool isEmpty = IsEmptyInstance(objectInstance);
				int nonEmptyCount = myNonEmptyInstanceCount;
				if (instanceLocation == -1)
				{
					if (isEmpty)
					{
						// Tack it on the end, no notifications
						instances.Add(objectInstance);
					}
					else
					{
						if (nonEmptyCount == instanceCount)
						{
							instances.Add(objectInstance);
						}
						else
						{
							// Put the empty element from the current first
							// non empty location at the end, then insert this
							// item in its slot
							instances.Add(instances[nonEmptyCount]);
							instances[nonEmptyCount] = objectInstance;
						}
						++myNonEmptyInstanceCount;
						base.EditInstanceDisplay(nonEmptyCount);
						base.AddInstanceDisplay(nonEmptyCount + 1);
						IUnattachedBranch[] notifyBranches = myUnattachedBranches;
						if (notifyBranches != null)
						{
							for (int i = 0; i < notifyBranches.Length; ++i)
							{
								IUnattachedBranch notifyBranch = notifyBranches[i];
								if (notifyBranch != null)
								{
									notifyBranch.AnchorUnattachedBranch(objectInstance, null);
								}
							}
							Array.Clear(notifyBranches, 0, notifyBranches.Length);
						}
						return true;
					}
				}
				else if (isEmpty)
				{
					if (instanceLocation < nonEmptyCount)
					{
						// Unlikely, but handle it anyway
						instances.RemoveAt(instanceLocation);
						--myNonEmptyInstanceCount;
						base.RemoveInstanceDisplay(instanceLocation);
						instances.Add(objectInstance); // The instance is still alive, just empty
					}
				}
				else if (instanceLocation >= nonEmptyCount)
				{
					if (instanceLocation > nonEmptyCount)
					{
						// Swap the current empty into the non empty slot
						ObjectTypeInstance emptyInstance = instances[nonEmptyCount];
						instances[nonEmptyCount] = instances[instanceLocation];
						instances[instanceLocation] = emptyInstance;
					}
					++myNonEmptyInstanceCount;
					base.AddInstanceDisplay(nonEmptyCount);
				}
				return false;
			}
			#endregion // Helper Methods
			#region Base overrides
			protected sealed override CellEditContext CreateEditContext(int row, int column)
			{
				CellEditContext retVal = null;
				ObjectType entityType = myEntityType;
				ObjectType entityTypeSubtype = myEntityTypeSubtype;
				Role identifierRole = entityType.PreferredIdentifier.RoleCollection[column - 1];
				ObjectType columnRolePlayer = identifierRole.RolePlayer;
				if (columnRolePlayer != null && (columnRolePlayer.IsValueType || columnRolePlayer.ResolvedPreferredIdentifier != null))
				{
					List<ObjectTypeInstance> instances = myCachedInstances;
					EntityTypeInstance editInstance = null;
					EntityTypeSubtypeInstance editSubtypeInstance = null;
					if (row < myNonEmptyInstanceCount)
					{
						editInstance = (entityTypeSubtype != null) ?
							(editSubtypeInstance = (EntityTypeSubtypeInstance)myCachedInstances[row]).SupertypeInstance :
							(EntityTypeInstance)myCachedInstances[row];
					}
					retVal = new CellEditContext(
						entityType,
						entityTypeSubtype,
						entityTypeSubtype != null ? GetIdentifyingSupertypeRole(entityTypeSubtype) : identifierRole,
						editInstance,
						editSubtypeInstance,
						null);
				}
				return retVal;
			}
			public sealed override void DeleteInstance(int row, int column)
			{
				if (base.IsFullRowSelectColumn(column) && row < myNonEmptyInstanceCount)
				{
					ObjectType targetType = myEntityTypeSubtype ?? myEntityType;
					ObjectTypeInstance targetInstance = targetType.ObjectTypeInstanceCollection[row];
					if (myEntityTypeSubtype == null)
					{
						LinkedElementCollection<Role> roles = myEntityType.PreferredIdentifier.RoleCollection;
						Role identifierRole;
						ValueTypeInstance oppositeValueInstance;
						EntityTypeRoleInstance roleInstance;
						if (roles.Count == 1 &&
							(identifierRole = roles[0]).GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole &&
							null != (roleInstance = ((EntityTypeInstance)targetInstance).FindRoleInstance(identifierRole)) &&
							(null == (oppositeValueInstance = roleInstance.ObjectTypeInstance as ValueTypeInstance) ||
							RoleInstance.GetLinksToRoleCollection(oppositeValueInstance).Count > 1))
						{
							throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
						}
					}
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, targetType.Name, targetInstance.Name)))
					{
						// Note that the opposite value instance will automatically be deleted
						// by rules for cases not blocked in the implication check
						targetInstance.Delete();
						t.Commit();
					}
				}
			}
			#endregion // Base overrides
		}
		private sealed class FactTypeBranch : BaseBranch, IBranch, IMultiColumnBranch, IUnattachedBranchOwner
		{
			#region Member Variables
			private readonly FactType myFactType;
			private List<FactTypeInstance> myCachedFactInstances;
			private List<ObjectTypeInstance> myCachedObjectInstances;
			private FactTypeInstanceImplication myImplicationProxy;
			private ObjectType myObjectifyingType;
			/// <summary>
			/// Expansion branches from the 'new' row that need to
			/// be attached and notified when a new instance is added.
			/// The zero index in this array is the first item column,
			/// not the dummy row number column.
			/// </summary>
			private IUnattachedBranch[] myUnattachedBranches;
			private int myUnaryColumn;
			private bool myHasUnaryColumn;
			#endregion
			#region Construction
			public FactTypeBranch(FactType selectedFactType, int factTypeColumnCount, int? unaryColumnAdjustment, ObjectType objectifyingType)
				: base(factTypeColumnCount + ((objectifyingType == null) ? 1 : 2), selectedFactType.Store)
			{
				myFactType = selectedFactType;
				ValidateReadOnlyProxyObject();
				ObjectType proxyObjectType = myImplicationProxy.ImpliedByEntityType;
				if (proxyObjectType != null)
				{
					myCachedObjectInstances = GetNonEmptyObjectInstances(proxyObjectType);
				}
				else
				{
					myCachedFactInstances = new List<FactTypeInstance>(selectedFactType.FactTypeInstanceCollection);
				}
				myObjectifyingType = objectifyingType;
				if (unaryColumnAdjustment.HasValue)
				{
					myHasUnaryColumn = true;
					myUnaryColumn = unaryColumnAdjustment.Value;
				}
			}
			#endregion
			#region IBranch Interface Members
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				bool delete = newText.Length == 0;
				Store store = Store;
				// If editing an existing FactTypeInstance
				if (row != NewRowIndex)
				{
					FactType selectedFactType = myFactType;
					FactTypeInstance editInstance = myCachedFactInstances[row];
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							Role factRole = selectedFactType.OrderedRoleCollection[column + myUnaryColumn].Role;
							LinkedElementCollection<FactTypeRoleInstance> factRoleInstances = editInstance.RoleInstanceCollection;
							FactTypeRoleInstance editRoleInstance = FactTypeInstance.FindRoleInstance(factRoleInstances, factRole);
							// If editing an existing FactTypeRoleInstance
							if (editRoleInstance != null)
							{
								ObjectTypeInstance attachedInstance = editRoleInstance.ObjectTypeInstance;
								if (delete)
								{
									if (factRoleInstances.Count == 1)
									{
										using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, editInstance.Name)))
										{
											editRoleInstance.Delete();
											t.Commit();
										}
									}
									else
									{
										using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(editRoleInstance.Role, false), attachedInstance.Name)))
										{
											editRoleInstance.Delete();
											t.Commit();
										}
									}
								}
								else
								{
									ValueTypeInstance instance = null;
									ObjectType editRolePlayer = editRoleInstance.Role.RolePlayer;
									using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, editRolePlayer.Name, attachedInstance.Name)))
									{
										ObjectTypeInstance result = RecurseValueTypeInstance(attachedInstance, editRolePlayer, newText, ref instance, true, true);
										ConnectInstance(ref editInstance, result, factRole, null);
										if (t.HasPendingChanges)
										{
											t.Commit();
										}
									}
								}
								return LabelEditResult.AcceptEdit;
							}
							// If editing an existing FactTypeInstance but creating a new FactTypeRoleInstance
							else if (!delete)
							{
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedFactType.Name)))
								{
									ValueTypeInstance instance = null;
									ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance, true, true);
									editInstance.EnsureRoleInstance(factRole, result);
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
							break;
						case ColumnType.EntityType:
							ObjectTypeInstance objectifyingInstance = editInstance.ObjectifyingInstance;
							ObjectType objectifyingType = myObjectifyingType;
							if (objectifyingInstance != null)
							{
								if (delete)
								{
									// Check for an empty start instance, note assumption that we aren't text-editing a subtype instance
									EntityTypeInstance objectifyingEntityInstance;
									LinkedElementCollection<EntityTypeRoleInstance> entityRoleInstances;
									if (null == (objectifyingEntityInstance = objectifyingInstance as EntityTypeInstance) ||
										(entityRoleInstances = objectifyingEntityInstance.RoleInstanceCollection).Count == 0)
									{
										return LabelEditResult.CancelEdit;
									}
									// Delete entity instance. Note that the user can choose 'unspecified' to detach
									EntityTypeRoleInstance entityRoleInstance;
									ValueTypeInstance deleteValueInstance = null;
									if (entityRoleInstances.Count == 1 &&
										(entityRoleInstance = entityRoleInstances[0]).Role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
									{
										// See comments in EntityTypeBranch.CommitLabelEdit
										deleteValueInstance = entityRoleInstance.ObjectTypeInstance as ValueTypeInstance;
										if (deleteValueInstance == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
										{
											throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
										}
									}
									using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, objectifyingType.Name, objectifyingEntityInstance.IdentifierName)))
									{
										if (deleteValueInstance != null)
										{
											deleteValueInstance.Delete();
										}
										else
										{
											objectifyingEntityInstance.Delete();
										}
										t.Commit();
									}
									return LabelEditResult.AcceptEdit;
								}
								else
								{
									ValueTypeInstance instance = null;
									using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, objectifyingType.Name, objectifyingInstance.Name)))
									{
										ObjectTypeInstance result = RecurseValueTypeInstance(objectifyingInstance, objectifyingType, newText, ref instance, true, true);
										if (result != objectifyingInstance)
										{
											ConnectInstance(ref editInstance, result, null, objectifyingType);
										}
										if (t.HasPendingChanges)
										{
											t.Commit();
										}
									}
									return LabelEditResult.AcceptEdit;
								}
							}
							else if (!delete)
							{
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedFactType.Name)))
								{
									ValueTypeInstance instance = null;
									ObjectTypeInstance result = RecurseValueTypeInstance(null, objectifyingType, newText, ref instance, true, true);
									FactTypeInstance existingResultInstance;
									if (null != (existingResultInstance = result.ObjectifiedInstance) &&
										existingResultInstance.RoleInstanceCollection.Count != 0)
									{
										throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelSamplePopulationEditorObjectifyingIdentifierAlreadyUsedExceptionText, result.Name));
									}
									editInstance.ObjectifyingInstance = result;
									if (t.HasPendingChanges)
									{
										t.Commit();
									}
								}
								return LabelEditResult.AcceptEdit;
							}
							break;
					}
				}
				// New Row Editing
				else if (!delete)
				{
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							FactType selectedFactType = myFactType;
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedFactType.Name)))
							{
								FactTypeInstance newInstance = new FactTypeInstance(store);
								newInstance.FactType = selectedFactType;
								Role factRole = selectedFactType.OrderedRoleCollection[column + myUnaryColumn].Role;
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(null, factRole.RolePlayer, newText, ref instance, true, true);
								instance.Value = newText;
								new FactTypeRoleInstance(factRole, result).FactTypeInstance = newInstance;
								t.Commit();
							}
							break;
						case ColumnType.EntityType:
							ObjectType objectifyingType = myObjectifyingType;
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, objectifyingType.Name)))
							{
								FactTypeInstance newInstance = new FactTypeInstance(store);
								newInstance.FactType = myFactType;
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(null, objectifyingType, newText, ref instance, true, true);
								instance.Value = newText;
								newInstance.ObjectifyingInstance = result;
								t.Commit();
							}
							break;
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
					List<FactTypeInstance> instances = myCachedFactInstances;
					FactTypeInstance parentInstance = (row < instances.Count) ? instances[row] : null;
					EntityEditorBranch expandedBranch = null;
					int startColumn = column;
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							Role selectedRole = selectedFactType.OrderedRoleCollection[column + myUnaryColumn].Role;
							ObjectTypeInstance editInstance = null;
							if (parentInstance != null)
							{
								FactTypeRoleInstance foundRoleInstance = parentInstance.FindRoleInstance(selectedRole);
								if (foundRoleInstance != null)
								{
									editInstance = foundRoleInstance.ObjectTypeInstance;
									Debug.Assert(editInstance != null);
								}
							}
							expandedBranch = new EntityEditorBranch(parentInstance, selectedFactType, editInstance, selectedRole, null, this);
							break;
						case ColumnType.EntityType:
							expandedBranch = new EntityEditorBranch(parentInstance, selectedFactType, (parentInstance != null) ? parentInstance.ObjectifyingInstance : null, null, myObjectifyingType, this);
							break;
					}
					if (expandedBranch != null && parentInstance == null)
					{
						IUnattachedBranch[] branches = myUnattachedBranches ?? (myUnattachedBranches = new IUnattachedBranch[ColumnCount - 1]);
						branches[startColumn - 1] = expandedBranch;
					}
					return expandedBranch;
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null)
				{
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							text = ObjectTypeInstance.GetDisplayString(null, myFactType.OrderedRoleCollection[column + myUnaryColumn].Role.RolePlayer, false);
							break;
						case ColumnType.EntityType:
							text = ObjectTypeInstance.GetDisplayString(null, myObjectifyingType, true);
							break;
					}
				}
				else if (text.Length == 0)
				{
					if (IsReadOnly)
					{
						Debug.Assert(myObjectifyingType == null);
						ObjectTypeInstance instance = myCachedObjectInstances[row];
						if (instance != null)
						{
							RoleBase impliedRole = myImplicationProxy.ImpliedProxyRole;
							if (impliedRole != null &&
								myFactType.OrderedRoleCollection.IndexOf(impliedRole) == column - 1)
							{
								FactTypeInstance factInstance;
								if (null != (factInstance = instance.ObjectifiedInstance))
								{
									RoleProxy roleProxy = impliedRole as RoleProxy;
									Role targetRole = (roleProxy != null) ? roleProxy.TargetRole : ((ObjectifiedUnaryRole)impliedRole).TargetRole;
									FactTypeRoleInstance factRoleInstance;
									if (null != (factRoleInstance = factInstance.FindRoleInstance(targetRole)) &&
										null != (instance = factRoleInstance.ObjectTypeInstance))
									{
										text = instance.Name;
									}
								}
							}
							else
							{
								text = instance.Name;
							}
						}
					}
					else
					{
						FactTypeInstance factInstance = myCachedFactInstances[row];
						switch (ResolveColumn(ref column))
						{
							case ColumnType.FactType:
								LinkedElementCollection<FactTypeRoleInstance> factTypeRoleInstances = factInstance.RoleInstanceCollection;
								int roleInstanceCount = factTypeRoleInstances.Count;
								FactTypeRoleInstance instance;
								Role factTypeRole = myFactType.OrderedRoleCollection[column + myUnaryColumn].Role;
								for (int i = 0; i < roleInstanceCount; ++i)
								{
									if (factTypeRole == (instance = factTypeRoleInstances[i]).Role)
									{
										return instance.ObjectTypeInstance.Name;
									}
								}
								text = ObjectTypeInstance.GetDisplayString(null, factTypeRole.RolePlayer, false);
								break;
							case ColumnType.EntityType:
								ObjectTypeInstance objectifyingInstance = factInstance.ObjectifyingInstance;
								text = (objectifyingInstance != null) ? objectifyingInstance.IdentifierName : ObjectTypeInstance.GetDisplayString(null, myObjectifyingType, true);
								break;
						}
					}
				}
				return text;
			}

			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.Icon)
				{
					return TreeControl.GetColumnHeader(column).Text;
				}
				return null;
			}

			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = base.GetDisplayData(row, column, requiredData);
				if (!base.IsFullRowSelectColumn(column))
				{
					retVal.SelectedImage = retVal.Image = (short)TreeControl.GetColumnHeader(column).ImageIndex;
				}
				return retVal;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				if (!IsReadOnly && !base.IsFullRowSelectColumn(column))
				{
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							ObjectType rolePlayer = myFactType.OrderedRoleCollection[column + myUnaryColumn].Role.RolePlayer;
							return rolePlayer != null && (rolePlayer.NestedFactType != null || HasComplexIdentifier(rolePlayer));
						case ColumnType.EntityType:
							return HasComplexIdentifier(myObjectifyingType);
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
						if (myImplicationProxy.ImpliedByEntityType != null)
						{
							return myCachedObjectInstances.Count;
						}
					}
					else
					{
						return myCachedFactInstances.Count + BaseBranch.VisibleItemCount;
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
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				List<FactTypeInstance> instances;
				TrackingObjectAction trackingAction = TrackingObjectAction.ThisLevel;
				int row = -1;
				int column = 0;
				if (style == ErrorObject &&
					null != (instances = myCachedFactInstances))
				{
					TooFewFactTypeRoleInstancesError partialFactInstancePopulationError;
					TooFewEntityTypeRoleInstancesError partialEntityInstancePopulationError;
					ObjectifyingInstanceRequiredError objectifyingInstanceError;
					FactTypeInstance factInstance;
					if (null != (partialFactInstancePopulationError = obj as TooFewFactTypeRoleInstancesError))
					{
						factInstance = partialFactInstancePopulationError.FactTypeInstance;
						if (-1 != (row = instances.IndexOf(factInstance)))
						{
							IList<RoleBase> factRoles = myFactType.OrderedRoleCollection;
							IList<FactTypeRoleInstance> roleInstances = factInstance.RoleInstanceCollection;
							int roleCount = factRoles.Count;
							for (int i = 0; i < roleCount; ++i)
							{
								if (null == FactTypeInstance.FindRoleInstance(roleInstances, factRoles[i].Role))
								{
									int testColumn = 1;
									column = i + (ResolveColumn(ref testColumn) == ColumnType.EntityType ? 2 : 1);
									break;
								}
							}
						}
					}
					else if (null != (partialEntityInstancePopulationError = obj as TooFewEntityTypeRoleInstancesError))
					{
						EntityTypeInstance entityInstance;
						ObjectType entityType;
						if (null != (entityInstance = partialEntityInstancePopulationError.EntityTypeInstance) &&
							null != (entityType = entityInstance.ObjectType) &&
							null != (factInstance = entityInstance.ObjectifiedInstance) &&
							-1 != (row = instances.IndexOf(factInstance)))
						{
							UniquenessConstraint pid;
							if (null != (pid = entityType.PreferredIdentifier) &&
								pid.IsObjectifiedPreferredIdentifier)
							{
								IList<RoleBase> factRoles = myFactType.OrderedRoleCollection;
								IList<FactTypeRoleInstance> roleInstances = factInstance.RoleInstanceCollection;
								IList<Role> identifierRoles = null;
								int roleCount = factRoles.Count;
								for (int i = 0; i < roleCount; ++i)
								{
									Role role = factRoles[i].Role;
									if (null == FactTypeInstance.FindRoleInstance(roleInstances, factRoles[i].Role))
									{
										// The identifier roles are a subset of the FactType roles, but can be a proper
										// subset. Use the displayed order (based on the FactType), but verify that
										// the role we hit is in the identifier subset.
										if ((identifierRoles ?? (identifierRoles = pid.RoleCollection)).Contains(role))
										{
											column = i + 1;
											break;
										}
									}
								}
							}
							else
							{
								column = 1;
								trackingAction = TrackingObjectAction.NextLevel;
							}
						}
					}
					else if (null != (objectifyingInstanceError = obj as ObjectifyingInstanceRequiredError))
					{
						if (-1 != (row = instances.IndexOf(objectifyingInstanceError.FactTypeInstance)))
						{
							column = 1;
						}
					}
					else if (obj is ObjectifiedInstanceRequiredError)
					{
						// Grab the first FactTypeInstance without an objectifying instance, or the new row
						row = 0;
						int instanceCount = instances.Count;
						for (;row < instanceCount; ++row)
						{
							if (instances[row].ObjectifyingInstance == null)
							{
								break;
							}
						}
						column = 1;
					}
				}
				else if (style == ObjectStyle.TrackingObject)
				{
					FactTypeInstance factInstance;
					EntityTypeInstance entityInstance;
					if (null != (factInstance = obj as FactTypeInstance))
					{
						if (!IsReadOnly)
						{
							row = myCachedFactInstances.IndexOf(factInstance);
						}
					}
					else if (null != (entityInstance = obj as EntityTypeInstance))
					{
						if (IsReadOnly)
						{
							row = myCachedObjectInstances.IndexOf(entityInstance);
						}
						else if (null != (factInstance = entityInstance.ObjectifiedInstance))
						{
							row = myCachedFactInstances.IndexOf(factInstance);
							column = 1;
						}
					}
				}
				if (row != -1)
				{
					return new LocateObjectData(row, column, (int)trackingAction);
				}
				return new LocateObjectData();
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
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasEntityTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityTypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasEntityTypeInstanceRemovedEvent), action);

				// Track FactTypeInstance changes
				classInfo = dataDirectory.FindDomainRelationship(FactTypeHasFactTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasFactTypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasFactTypeInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeInstanceHasRoleInstanceRemovedEvent), action);

#if ROLEINSTANCE_ROLEPLAYERCHANGE
				classInfo = dataDirectory.FindDomainRelationship(FactTypeRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(FactTypeRoleInstanceRolePlayerChangedEvent), action);
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeInstanceNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(FactTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(FactTypeInstanceNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
				propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ObjectificationInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationInstanceRemovedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationInstanceRolePlayerChangedEvent), action);

				// Handlers to manage empty instances based on the role content
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeRoleInstanceRemovedEvent), action);

				eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), new EventHandler<RolePlayerChangedEventArgs>(SubtypeInstanceSupertypeRolePlayerChangedEvent), action);
			}

			private void ObjectTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectTypeInstance objectTypeInstance = (ObjectTypeInstance)e.ModelElement;
				FactTypeInstance factInstance;
				ObjectType objectifyingType;
				if (IsReadOnly)
				{
					int index;
					if (myImplicationProxy.ImpliedByEntityType == objectTypeInstance.ObjectType &&
						-1 != (index = myCachedObjectInstances.IndexOf(objectTypeInstance)))
					{
						EditInstanceDisplay(index);
					}
				}
				else if (null != (objectifyingType = myObjectifyingType) &&
					objectifyingType == objectTypeInstance.ObjectType &&
					null != (factInstance = objectTypeInstance.ObjectifiedInstance))
				{
					int index = myCachedFactInstances.IndexOf(factInstance);
					if (index != -1)
					{
						EditInstanceDisplay(index);
					}
				}
			}

			private void FactTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				if (!IsReadOnly)
				{
					int index = myCachedFactInstances.IndexOf((FactTypeInstance)e.ModelElement);
					if (index != -1)
					{
						EditInstanceDisplay(index);
					}
				}
			}


			private void EntityTypeHasEntityTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entityType = link.EntityType;
				if (entityType != null && !entityType.IsDeleted && entityType == myImplicationProxy.ImpliedByEntityType)
				{
					List<ObjectTypeInstance> instances = myCachedObjectInstances;
					ObjectTypeInstance addedInstance = link.EntityTypeInstance;
					if (!instances.Contains(addedInstance)) // Possible for implied entity instances when the preferred identifier changes
					{
						instances.Add(addedInstance);
						base.AddInstanceDisplay(instances.Count - 1);
					}
				}
			}

			private void EntityTypeHasEntityTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entityType = link.EntityType;
				if (entityType != null && !entityType.IsDeleted && entityType == myImplicationProxy.ImpliedByEntityType)
				{
					List<ObjectTypeInstance> instances = myCachedObjectInstances;
					int instanceLocation = instances.IndexOf(link.EntityTypeInstance);
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
					{
						instances.RemoveAt(instanceLocation);
						base.RemoveInstanceDisplay(instanceLocation);
					}
				}
			}
			private void FactTypeHasFactTypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeHasFactTypeInstance link;
				FactType factType;
				if (!IsReadOnly &&
					!(factType = (link = (FactTypeHasFactTypeInstance)e.ModelElement).FactType).IsDeleted &&
					factType == myFactType)
				{
					TestNotifyAddInstance(link.FactTypeInstance);
				}
			}
			private void FactTypeHasFactTypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
				FactType factType = link.FactType;
				List<FactTypeInstance> instances;
				if (!factType.IsDeleted &&
					factType == myFactType &&
					null != (instances = myCachedFactInstances))
				{
					int instanceLocation = instances.IndexOf(link.FactTypeInstance);
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
					{
						instances.RemoveAt(instanceLocation);
						base.RemoveInstanceDisplay(instanceLocation);
					}
				}
			}

			private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EditFactInstanceDisplay(((FactTypeInstanceHasRoleInstance)e.ModelElement).FactTypeInstance);
			}

#if !ROLEINSTANCE_ROLEPLAYERCHANGE
			private void FactTypeRoleInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				EditFactInstanceDisplay(((FactTypeRoleInstance)e.ElementLink).FactTypeInstance);
			}
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
			private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EditFactInstanceDisplay(((FactTypeInstanceHasRoleInstance)e.ModelElement).FactTypeInstance);
			}
			private void ObjectificationInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				if (myObjectifyingType != null)
				{
					EditFactInstanceDisplay(((ObjectificationInstance)e.ModelElement).ObjectifiedInstance);
				}
			}
			private void ObjectificationInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				if (myObjectifyingType != null)
				{
					EditFactInstanceDisplay(((ObjectificationInstance)e.ModelElement).ObjectifiedInstance);
				}
			}
			private void ObjectificationInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				if (myObjectifyingType != null)
				{
					EditFactInstanceDisplay(((ObjectificationInstance)e.ElementLink).ObjectifiedInstance);
					if (e.DomainRole.Id == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
					{
						EditFactInstanceDisplay((FactTypeInstance)e.OldRolePlayer);
					}
				}
			}
			private void RoleNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				Role role = e.ModelElement as Role;
				if (!role.IsDeleted && IsPartOfDisplayedFactType(myFactType, role))
				{
					UpdateColumnHeaders();
				}
			}

			/// <summary>
			/// Checks if the branch should be ReadOnly, which happens when
			/// we have a binary FactType that is part of an identifier.
			/// </summary>
			private void ValidateReadOnlyProxyObject()
			{
				FactType factType = myFactType;
				FactTypeInstanceImplication newProxy = new FactTypeInstanceImplication(factType);
				FactTypeInstanceImplication oldProxy = myImplicationProxy;
				int oldItemCount = -1;
				if (oldProxy.IsImplied)
				{
					if (oldProxy != newProxy)
					{
						oldItemCount = VisibleItemCount;
						if (!newProxy.IsImplied)
						{
							myCachedObjectInstances = null;
							myCachedFactInstances = new List<FactTypeInstance>(factType.FactTypeInstanceCollection);
						}
						else
						{
							myCachedFactInstances = null;
							myCachedObjectInstances = GetNonEmptyObjectInstances(newProxy.ImpliedByEntityType);
						}
					}
				}
				else if (myCachedFactInstances != null && newProxy.IsImplied)
				{
					oldItemCount = VisibleItemCount;
					myCachedFactInstances = null;
					myCachedObjectInstances = GetNonEmptyObjectInstances(newProxy.ImpliedByEntityType);
				}
				myImplicationProxy = newProxy;
				IsReadOnly = newProxy.IsImplied;
				if (oldItemCount != -1)
				{
					base.Repopulate(oldItemCount);
				}
			}

			private static List<ObjectTypeInstance> GetNonEmptyObjectInstances(ObjectType objectType)
			{
				LinkedElementCollection<ObjectTypeInstance> instances = objectType.ObjectTypeInstanceCollection;
				int instanceCount = 0;
				List<ObjectTypeInstance> retVal = new List<ObjectTypeInstance>(instanceCount);
				foreach (ObjectTypeInstance instance in instances)
				{
					if (!IsEmptyInstance(instance))
					{
						retVal.Add(instance);
					}
				}
				return retVal;
			}

			private void ObjectTypeNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectType objectType = e.ModelElement as ObjectType;
				if (!objectType.IsDeleted)
				{
					if (IsPartOfDisplayedFactType(myFactType, objectType))
					{
						UpdateColumnHeaders();
					}
				}
			}
			private void EntityTypeRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectType proxyObjectType = myImplicationProxy.IdentifyingSupertype;
				if (proxyObjectType != null)
				{
					EntityTypeInstance entityInstance;
					EntityTypeInstanceHasRoleInstance link = (EntityTypeInstanceHasRoleInstance)e.ModelElement;
					if (!link.IsDeleted &&
						!(entityInstance = link.EntityTypeInstance).IsDeleted &&
						entityInstance.EntityType == proxyObjectType)
					{
						ObjectType subtype = myImplicationProxy.ImpliedByEntityType;
						ObjectTypeInstance verifyInstance = (subtype == proxyObjectType) ? (ObjectTypeInstance)entityInstance : EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, subtype, true, false);
						if (verifyInstance != null)
						{
							// Make sure we have the appropriate instance
							List<ObjectTypeInstance> instances = myCachedObjectInstances;
							if (!instances.Contains(verifyInstance))
							{
								instances.Add(verifyInstance);
								base.AddInstanceDisplay(instances.Count - 1);
							}
						}
					}
				}
			}
			private void EntityTypeRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectType proxyObjectType = myImplicationProxy.IdentifyingSupertype;
				if (proxyObjectType != null)
				{
					EntityTypeInstance entityInstance;
					EntityTypeInstanceHasRoleInstance link = (EntityTypeInstanceHasRoleInstance)e.ModelElement;
					if (!(entityInstance = link.EntityTypeInstance).IsDeleted &&
						entityInstance.EntityType == proxyObjectType &&
						entityInstance.RoleInstanceCollection.Count == 0)
					{
						ObjectType subtype = myImplicationProxy.ImpliedByEntityType;
						ObjectTypeInstance verifyInstance = (subtype == proxyObjectType) ? (ObjectTypeInstance)entityInstance : EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, subtype, true, false);
						if (verifyInstance != null)
						{
							List<ObjectTypeInstance> instances = myCachedObjectInstances;
							int instanceLocation = instances.IndexOf(verifyInstance);
							if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
							{
								instances.RemoveAt(instanceLocation);
								base.RemoveInstanceDisplay(instanceLocation);
							}
						}
					}
				}
			}
			private void SubtypeInstanceSupertypeRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				ObjectType proxySubType = myImplicationProxy.ImpliedByEntityType;
				ObjectType proxySupertype;
				if (proxySubType != null &&
					null != (proxySupertype = myImplicationProxy.IdentifyingSupertype) &&
					proxySubType != proxySupertype)
				{
					EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ElementLink;
					EntityTypeSubtypeInstance subtypeInstance;
					if (!link.IsDeleted &&
						!(subtypeInstance = link.EntityTypeSubtypeInstance).IsDeleted &&
						subtypeInstance.EntityTypeSubtype == proxySubType)
					{
						List<ObjectTypeInstance> instances = myCachedObjectInstances;
						if (IsEmptyInstance(subtypeInstance))
						{
							int instanceLocation = instances.IndexOf(subtypeInstance);
							if (instanceLocation != -1)
							{
								instances.RemoveAt(instanceLocation);
								base.RemoveInstanceDisplay(instanceLocation);
							}
						}
						else
						{
							if (!instances.Contains(subtypeInstance))
							{
								instances.Add(subtypeInstance);
								base.AddInstanceDisplay(instances.Count - 1);
							}
						}
					}
				}
			}
			#endregion // Event Handlers
			#region IUnattachedBranchOwner Implementation
			bool IUnattachedBranchOwner.TryAnchorUnattachedBranches(ObjectTypeInstance objectInstance, FactTypeInstance factInstance)
			{
				if (!IsReadOnly &&
					(factInstance != null ||
					null != (factInstance = objectInstance.ObjectifiedInstance)))
				{
					return TestNotifyAddInstance(factInstance);
				}
				return false;
			}
			#endregion // IUnattachedBranchOwner Implementation
			#region Helper Methods
			private enum ColumnType
			{
				/// <summary>
				/// The full row select column
				/// </summary>
				FullRowSelect,
				/// <summary>
				/// A normal FactType column
				/// </summary>
				FactType,
				/// <summary>
				/// A column that defers to the wrapped EntityTypeBranch
				/// </summary>
				EntityType,
			}
			/// <summary>
			/// Get the <see cref="ColumnType"/> of the column and
			/// adjust the column values so that no further adjustment
			/// is needed to direct reference appropriate collection
			/// or item.
			/// </summary>
			private ColumnType ResolveColumn(ref int column)
			{
				if (base.IsFullRowSelectColumn(column))
				{
					return ColumnType.FullRowSelect;
				}
				--column;
				if (myObjectifyingType != null)
				{
					if (column == 0)
					{
						return ColumnType.EntityType;
					}
					--column;
				}
				return ColumnType.FactType;
			}
			/// <summary>
			/// Helper method to update column headers
			/// </summary>
			private void UpdateColumnHeaders()
			{
				ObjectType objectifyingType = myObjectifyingType;
				int columnOffset = 1;
				if (objectifyingType != null)
				{
					columnOffset = 2;
					base.EditColumnHeader(1, true, objectifyingType);
				}
				IList<RoleBase> factRoles = myFactType.OrderedRoleCollection;
				int roleCount = factRoles.Count;
				if (myHasUnaryColumn)
				{
					int unaryColumn = myUnaryColumn;
					if (unaryColumn < roleCount)
					{
						base.EditColumnHeader(columnOffset, factRoles[unaryColumn].Role);
					}
				}
				else
				{
					for (int i = 0; i < roleCount; ++i)
					{
						base.EditColumnHeader(i + columnOffset, factRoles[i].Role);
					}
				}
			}
			/// <summary>
			/// Connect a given instance to the branch's current objectType, for the specified role or objectifying type
			/// </summary>
			/// <param name="parentInstance">Instance to connect to. Can be null.</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="factTypeRole">Role to connect to</param>
			/// <param name="objectifyingType">Objectifying <see cref="ObjectType"/>. Specified in place of <paramref name="factTypeRole"/>
			/// to relate a <see cref="FactTypeInstance"/> to an existing objectifying <see cref="ObjectTypeInstance"/></param>
			public static void ConnectInstance(ref FactTypeInstance parentInstance, ObjectTypeInstance connectInstance, Role factTypeRole, ObjectType objectifyingType)
			{
				Debug.Assert(connectInstance != null);
				if (factTypeRole != null)
				{
					Store store = factTypeRole.Store;
					FactType factType = factTypeRole.FactType;
					if (parentInstance == null)
					{
						parentInstance = new FactTypeInstance(store);
						parentInstance.FactType = factType;
						new FactTypeRoleInstance(factTypeRole, connectInstance).FactTypeInstance = parentInstance;
					}
					else
					{
						parentInstance.EnsureRoleInstance(factTypeRole, connectInstance);
					}
				}
				else
				{
					Debug.Assert(objectifyingType != null);
					if (parentInstance == null)
					{
						parentInstance = new FactTypeInstance(objectifyingType.Store);
						parentInstance.FactType = objectifyingType.NestedFactType;
					}
					ConnectObjectifyingIdentifierInstance(parentInstance, connectInstance);
				}
			}
			/// <summary>
			/// See if a <see cref="FactTypeInstance"/> needs to be added.
			/// Anchor unattached branches and notify as needed.
			/// </summary>
			/// <param name="factInstance">The instance to attach</param>
			/// <returns>Returns true if the new instance is added.</returns>
			private bool TestNotifyAddInstance(FactTypeInstance factInstance)
			{
				List<FactTypeInstance> instances;
				if (null != (instances = myCachedFactInstances) &&
					!instances.Contains(factInstance))
				{
					instances.Add(factInstance);
					int instanceCount = instances.Count;
					base.EditInstanceDisplay(instanceCount - 1);
					base.AddInstanceDisplay(instanceCount);
					IUnattachedBranch[] notifyBranches = myUnattachedBranches;
					if (notifyBranches != null)
					{
						ObjectTypeInstance objectifyingInstance = factInstance.ObjectifyingInstance;
						for (int i = 0; i < notifyBranches.Length; ++i)
						{
							IUnattachedBranch notifyBranch = notifyBranches[i];
							if (notifyBranch != null)
							{
								notifyBranch.AnchorUnattachedBranch(objectifyingInstance, factInstance);
							}
						}
						Array.Clear(notifyBranches, 0, notifyBranches.Length);
					}
					return true;
				}
				return false;
			}
			#endregion // Helper Methods
			#region Branch Update Methods
			private void EditFactInstanceDisplay(FactTypeInstance factInstance)
			{
				FactType factType = myFactType;
				if (factInstance != null &&
					!factInstance.IsDeleted &&
					null != (factType = myFactType) &&
					factInstance.FactType == factType)
				{
					List<FactTypeInstance> instances;
					int location;
					if (null != (instances = myCachedFactInstances) &&
						-1 != (location = instances.IndexOf(factInstance)))
					{
						base.EditInstanceDisplay(location);
					}
				}
			}
			#endregion // Branch Update Methods
			#region Base overrides
			protected override CellEditContext CreateEditContext(int row, int column)
			{
				CellEditContext retVal = null;
				ObjectType rolePlayer;
				switch (ResolveColumn(ref column))
				{
					case ColumnType.FactType:
						Role role = myFactType.OrderedRoleCollection[column + myUnaryColumn].Role;
						rolePlayer = role.RolePlayer;
						if (rolePlayer != null && (rolePlayer.IsValueType || rolePlayer.ResolvedPreferredIdentifier != null))
						{
							List<FactTypeInstance> instances = myCachedFactInstances;
							retVal = new CellEditContext(
								role,
								(row < instances.Count) ? instances[row] : null,
								null);
						}
						break;
					case ColumnType.EntityType:
						rolePlayer = myObjectifyingType;
						if (rolePlayer.ResolvedPreferredIdentifier != null)
						{
							List<FactTypeInstance> instances = myCachedFactInstances;
							FactTypeInstance factInstance = (row < instances.Count) ? instances[row] : null;
							retVal = new CellEditContext(
								rolePlayer,
								factInstance,
								(factInstance != null) ? factInstance.ObjectifyingInstance : null,
								null);
						}
						break;
				}
				return retVal;
			}
			public sealed override void DeleteInstance(int row, int column)
			{
				if (base.IsFullRowSelectColumn(column) && myImplicationProxy.ImpliedByEntityType == null)
				{
					IList<FactTypeInstance> instances = myCachedFactInstances;
					if (instances != null && row < instances.Count)
					{
						FactTypeInstance factInstance = instances[row];
						using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, factInstance.Name)))
						{
							ObjectTypeInstance objectInstance = (myObjectifyingType != null) ? factInstance.ObjectifyingInstance : null;
							factInstance.Delete();
							if (objectInstance != null && !objectInstance.IsDeleted) // Might delete automatically if implied. Otherwise, remove it.
							{
								objectInstance.Delete();
							}
							t.Commit();
						}
					}
				}
			}
			#endregion // Base overrides
		}
		private sealed class EntityEditorBranch : BaseBranch, IBranch, IMultiColumnBranch, IUnattachedBranch
		{
			#region Member Variables
			private readonly BaseBranch myParentBranch;
			private EntityTypeInstance myEditInstance;
			private EntityTypeSubtypeInstance myEditSubtypeInstance;
			private EntityTypeInstance myParentEntityInstance;
			private EntityTypeSubtypeInstance myParentEntitySubtypeInstance;
			private readonly ObjectType myParentEntityType;
			private readonly FactType myParentFactType;
			private FactTypeInstance myParentFactInstance;
			private readonly Role myEditRole;
			private readonly ObjectType myObjectifyingType;
			private int myItemCountCache;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentObjectInstance">Instance of the parent Entity type which contains the given editInstance. Pass null for a top-level subtype situation.</param>
			/// <param name="parentEntityType">The Entity type which contains the given editInstance</param>
			/// <param name="editInstance">The <see cref="EntityTypeInstance"/> or <see cref="EntityTypeSubtypeInstance"/> which will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited. Pass in a <see cref="SubtypeMetaRole"/> to indicate a top-level subtype</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public EntityEditorBranch(ObjectTypeInstance parentObjectInstance, ObjectType parentEntityType, ObjectTypeInstance editInstance, Role editRole, BaseBranch parentBranch)
				: this(editInstance, editRole, null, parentBranch)
			{
				myParentEntityType = parentEntityType;
				if (parentObjectInstance != null)
				{
					EntityTypeSubtypeInstance subtypeInstance;
					if (null != (subtypeInstance = parentObjectInstance as EntityTypeSubtypeInstance))
					{
						myParentEntitySubtypeInstance = subtypeInstance;
						myParentEntityInstance = subtypeInstance.SupertypeInstance;
					}
					else
					{
						myParentEntityInstance = parentObjectInstance as EntityTypeInstance;
					}
				}
			}

			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentFactInstance">Instance of the parent Fact type which contains the given editInstance</param>
			/// <param name="parentFactType">The Fact type which contains the given editInstance</param>
			/// <param name="editInstance">The <see cref="EntityTypeInstance"/> or <see cref="EntityTypeSubtypeInstance"/> which will be edited</param>
			/// <param name="editRole">Role from the <paramref name="parentFactType"/> which is being edited</param>
			/// <param name="objectifyingType">The objectifying type associated with the <paramref name="parentFactType"/>. Used in place of <paramref name="editRole"/> to edit the objectification relationship.</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public EntityEditorBranch(FactTypeInstance parentFactInstance, FactType parentFactType, ObjectTypeInstance editInstance, Role editRole, ObjectType objectifyingType, BaseBranch parentBranch)
				: this(editInstance, editRole, objectifyingType, parentBranch)
			{
				myParentFactType = parentFactType;
				myParentFactInstance = parentFactInstance;
				if (objectifyingType != null)
				{
					myParentEntityType = objectifyingType;
					if (myEditRole == null) // The edit role is set by initial constructor for subtype identifier cases
					{
						ObjectTypeInstance objectInstance;
						if (null != parentFactInstance &&
							null != (objectInstance = parentFactInstance.ObjectifyingInstance))
						{
							ContextParentObjectInstance = objectInstance;
						}
					}
				}
			}

			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="editInstance">The <see cref="EntityTypeInstance"/> or <see cref="EntityTypeSubtypeInstance"/> which will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited</param>
			/// <param name="objectifyingType">The objectifying type associated with the <paramref name="parentFactType"/>. Used in place of <paramref name="editRole"/> to edit the objectification relationship.</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			private EntityEditorBranch(ObjectTypeInstance editInstance, Role editRole, ObjectType objectifyingType, BaseBranch parentBranch) : base(2, (editRole != null) ? editRole.Store : objectifyingType.Store)
			{
				if (editInstance != null)
				{
					ContextObjectInstance = editInstance;
				}
				bool testObjectification;
				ObjectType contextObjectType;
				if (objectifyingType != null)
				{
					testObjectification = false;
					contextObjectType = objectifyingType;
					editRole = GetPreferredSubtypeRole(objectifyingType); // Special case to handle a subtype instance
				}
				else
				{
					testObjectification = true;
					contextObjectType = editRole.RolePlayer;
				}
				myEditRole = editRole;
				myObjectifyingType = objectifyingType;
				myParentBranch = parentBranch;
				FactType objectifiedFactType;
				UniquenessConstraint pid = contextObjectType.ResolvedPreferredIdentifier;
				if (testObjectification &&
					null != (objectifiedFactType = contextObjectType.NestedFactType))
				{
					myItemCountCache = objectifiedFactType.OrderedRoleCollection.Count; // OrderedRoleCollection handles unary binarization automatically
					if (pid.PreferredIdentifierFor != contextObjectType || !pid.IsObjectifiedPreferredIdentifier)
					{
						++myItemCountCache;
					}
				}
				else
				{
					if (pid.PreferredIdentifierFor == contextObjectType)
					{
						myItemCountCache = pid.RoleCollection.Count;
					}
					else
					{
						myItemCountCache = 1;
					}
				}
			}
			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentEntitySubtypeInstance">Instance of the parent Entity type which contains the given editInstance</param>
			/// <param name="parentEntityType">The Entity type which contains the given editInstance</param>
			/// <param name="editInstance">The <see cref="EntityTypeInstance"/> or <see cref="EntityTypeSubtypeInstance"/> which will be edited</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			private EntityEditorBranch(EntityTypeSubtypeInstance parentEntitySubtypeInstance, ObjectType parentEntityType, ObjectTypeInstance editInstance, BaseBranch parentBranch)
				: this(editInstance, GetIdentifyingSupertypeRole(parentEntityType), null, parentBranch)
			{
				myParentEntityType = parentEntityType;
				if (parentEntitySubtypeInstance != null)
				{
					myParentEntitySubtypeInstance = parentEntitySubtypeInstance;
					myParentEntityInstance = parentEntitySubtypeInstance.SupertypeInstance;
				}
			}
			#endregion // Constructors
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
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				bool delete = newText.Length == 0;
				ObjectType rowType;
				Role rowRole;
				ObjectTypeInstance rowInstance;
				FactTypeInstance rowFactInstance;
				EntityTypeInstance entityInstance;
				IList<EntityTypeRoleInstance> entityRoleInstances;
				EntityTypeRoleInstance entityRoleInstance;
				Store store = Store;
				switch (ResolveRow(ref row, out rowType, out rowRole, out rowFactInstance, out rowInstance))
				{
					case RowType.IdentifierRole:
						entityInstance = myEditInstance;
						entityRoleInstances = null;
						entityRoleInstance = null;
						if (entityInstance != null)
						{
							entityRoleInstances = entityInstance.RoleInstanceCollection;
							entityRoleInstance = EntityTypeInstance.FindRoleInstance(entityRoleInstances, rowRole);
						}
						if (delete)
						{
							if (entityInstance != null && entityRoleInstance != null)
							{
								using (Transaction t = rowRole.Store.TransactionManager.BeginTransaction(
									(entityRoleInstances.Count == 1) ?
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, entityInstance.EntityType.Name, entityInstance.Name) :
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(rowRole, false), entityRoleInstance.ObjectTypeInstance.Name)))
								{
									entityRoleInstance.Delete();
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, rowType.Name)))
							{
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(
									(entityRoleInstance != null) ? entityRoleInstance.ObjectTypeInstance : null,
									rowType,
									newText,
									ref instance,
									true,
									true);
								instance.Value = newText;
								ConnectInstance(result, rowRole, null, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
					case RowType.FactRole:
						FactTypeRoleInstance factRoleInstance = null;
						IList<FactTypeRoleInstance> factRoleInstances = null;
						if (rowFactInstance != null)
						{
							factRoleInstances = rowFactInstance.RoleInstanceCollection;
							factRoleInstance = FactTypeInstance.FindRoleInstance(factRoleInstances, rowRole);
						}
						if (delete)
						{
							if (rowFactInstance != null &&
								factRoleInstance != null)
							{
								using (Transaction t = rowRole.Store.TransactionManager.BeginTransaction(
									(factRoleInstances.Count == 1) ?
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, rowFactInstance.Name) :
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(rowRole, true), factRoleInstance.ObjectTypeInstance.Name)))
								{
									factRoleInstance.Delete();
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, rowType.Name)))
							{
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(
									(factRoleInstance != null) ? factRoleInstance.ObjectTypeInstance : null,
									rowType,
									newText,
									ref instance,
									true,
									true);
								instance.Value = newText;
								ConnectInstance(result, rowRole, null, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
					case RowType.Supertype:
						if (delete)
						{
							entityInstance = myEditInstance;
							if (entityInstance != null &&
								(entityRoleInstances = entityInstance.RoleInstanceCollection).Count == 1)
							{
								entityRoleInstance = entityRoleInstances[0];
								ObjectTypeInstance deleteInstance = entityRoleInstance.ObjectTypeInstance;
								ValueTypeInstance deleteValueInstance = null;
								if (entityRoleInstance.Role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
								{
									// See comments in EntityTypeBranch.CommitLabelEdit
									deleteValueInstance = deleteInstance as ValueTypeInstance;
									if (deleteValueInstance == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
									{
										throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
									}
								}
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, entityInstance.ObjectType.Name, entityInstance)))
								{
									if (deleteValueInstance != null)
									{
										deleteValueInstance.Delete();
									}
									else
									{
										entityInstance.Delete();
									}
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, rowType.Name)))
							{
								Role supertypeRole = GetIdentifyingSupertypeRole(rowType);
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(
									myEditInstance,
									supertypeRole.RolePlayer,
									newText,
									ref instance,
									true,
									true);
								instance.Value = newText;
								ConnectInstance(result, supertypeRole, null, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
					case RowType.ObjectifyingIdentifier:
						if (delete)
						{
							entityInstance = rowInstance as EntityTypeInstance;
							if (entityInstance != null &&
								(entityRoleInstances = entityInstance.RoleInstanceCollection).Count == 1)
							{
								entityRoleInstance = entityRoleInstances[0];
								ObjectTypeInstance deleteInstance = entityRoleInstance.ObjectTypeInstance;
								ValueTypeInstance deleteValueInstance = null;
								if (entityRoleInstance.Role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
								{
									// See comments in EntityTypeBranch.CommitLabelEdit
									deleteValueInstance = deleteInstance as ValueTypeInstance;
									if (deleteValueInstance == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
									{
										throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
									}
								}
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, entityInstance.EntityType.Name, entityInstance.Name)))
								{
									if (deleteValueInstance != null)
									{
										deleteValueInstance.Delete();
									}
									else
									{
										entityInstance.Delete();
									}
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, rowType.Name)))
							{
								ValueTypeInstance instance = null;
								ObjectTypeInstance result = RecurseValueTypeInstance(
									myEditInstance,
									rowType,
									newText,
									ref instance,
									true,
									true);
								instance.Value = newText;
								ConnectInstance(result, null, rowType, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
				}
				return LabelEditResult.CancelEdit;
			}

			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				ObjectType instanceType;
				RowType rowType = ResolveRow(ref row, out instanceType);
				return new VirtualTreeDisplayData((short)GetImageIndex(instanceType, rowType == RowType.ObjectifyingIdentifier));
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					ObjectType rowType;
					Role rowRole;
					ObjectTypeInstance rowInstance;
					FactTypeInstance rowFactInstance;
					switch (ResolveRow(ref row, out rowType, out rowRole, out rowFactInstance, out rowInstance))
					{
						case RowType.IdentifierRole:
							return new EntityEditorBranch(myEditInstance, rowType, rowInstance, rowRole, this);
						case RowType.Supertype:
							return new EntityEditorBranch(myEditSubtypeInstance, rowType, myEditInstance, this);
						case RowType.FactRole:
							return new EntityEditorBranch(rowFactInstance, myEditRole.RolePlayer.NestedFactType, rowInstance, rowRole, null, this);
						case RowType.ObjectifyingIdentifier:
							return new EntityEditorBranch(rowFactInstance, rowType.NestedFactType, rowInstance, null, rowType, this);
					}
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				ObjectTypeInstance instance;
				ObjectType instanceType;
				switch (ResolveRow(ref row, out instanceType, out instance))
				{
					case RowType.ObjectifyingIdentifier:
						return (instance != null) ? instance.IdentifierName : ObjectTypeInstance.GetDisplayString(null, instanceType, true);
					case RowType.Supertype:
						return (instance != null) ? instance.Name : ObjectTypeInstance.GetDisplayString(null, GetIdentifyingSupertypeRole(instanceType).RolePlayer, false);
				}
				return (instance != null) ? instance.Name : ObjectTypeInstance.GetDisplayString(null, instanceType, false);
			}

			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.Icon)
				{
					ObjectType rowObjectType;
					Role rowRole;
					RowType rowType = ResolveRow(ref row, out rowObjectType, out rowRole);
					return (rowRole != null) ?
						DeriveColumnName(rowRole, rowType == RowType.ObjectifyingIdentifier) :
						DeriveColumnName(rowObjectType, rowType == RowType.ObjectifyingIdentifier);
				}
				return null;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				ObjectType instanceType;
				RowType rowType = ResolveRow(ref row, out instanceType);
				return instanceType != null && ((rowType != RowType.ObjectifyingIdentifier && instanceType.NestedFactType != null) || HasComplexIdentifier(instanceType));
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
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				TooFewEntityTypeRoleInstancesError partialEntityInstancePopulationError;
				EntityTypeInstance entityInstance;
				ObjectType entityType;
				if (style == ErrorObject &&
					null != (partialEntityInstancePopulationError = obj as TooFewEntityTypeRoleInstancesError) &&
					null != (entityInstance = partialEntityInstancePopulationError.EntityTypeInstance) &&
					null != (entityType = entityInstance.EntityType) &&
					myParentEntityType == entityType)
				{
					int row = -1;
					IList<Role> identifierRoles = entityType.PreferredIdentifier.RoleCollection;
					IList<EntityTypeRoleInstance> roleInstances = entityInstance.RoleInstanceCollection;
					int roleCount = identifierRoles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						if (null == EntityTypeInstance.FindRoleInstance(roleInstances, identifierRoles[i]))
						{
							row = i;
							break;
						}
					}
					if (row != -1)
					{
						return new LocateObjectData(row, 0, (int)TrackingObjectAction.ThisLevel);
					}
				}
				return new LocateObjectData();
			}
			#endregion // IBranch Interface Members
			#region Event Handlers
			protected sealed override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				DomainClassInfo classInfo;

				// Track EntityTypeInstance changes
				classInfo = dataDirectory.FindDomainRelationship(EntityTypeSubtypeInstanceHasSupertypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeSubtypeHasSupertypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeSubtypeHasSupertypeInstanceRemovedEvent), action);
				eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), new EventHandler<RolePlayerChangedEventArgs>(EntityTypeSubtypeInstanceHasSupertypeInstanceRolePlayerChangedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(EntityTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeInstanceHasRoleInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactTypeInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeInstanceHasRoleInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ObjectificationInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationInstanceRemovedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationInstanceRolePlayerChangedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeInstanceNameChangedEvent), action);
			}

			private void EntityTypeSubtypeInstanceHasSupertypeInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ElementLink;
				ObjectTypeInstance instance;
				EntityTypeSubtypeInstance subtypeInstance = link.EntityTypeSubtypeInstance;
				if (RecurseInstanceUpdate(null, link.SupertypeInstance, subtypeInstance, null, InstanceUpdateType.Subtype, out instance) ||
					RecurseInstanceUpdate(null, (EntityTypeInstance)e.OldRolePlayer, subtypeInstance, null, InstanceUpdateType.Subtype, out instance))
				{
					EditColumnDisplay(0);
				}
			}
			private void EntityTypeSubtypeHasSupertypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ModelElement;
				ObjectTypeInstance instance;
				if (RecurseInstanceUpdate(null, link.SupertypeInstance, link.EntityTypeSubtypeInstance, null, InstanceUpdateType.Subtype, out instance))
				{
					EditColumnDisplay(0);
				}
			}
			private void EntityTypeSubtypeHasSupertypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ModelElement;
				ObjectTypeInstance instance;
				if (RecurseInstanceUpdate(null, link.SupertypeInstance, link.EntityTypeSubtypeInstance, null, InstanceUpdateType.Subtype, out instance))
				{
					EditColumnDisplay(0);
				}
			}
			private void EntityTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = (EntityTypeInstanceHasRoleInstance)e.ModelElement;
				ObjectTypeInstance instance;
				if (RecurseInstanceUpdate(null, link.EntityTypeInstance, null, link.RoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void EntityTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				ObjectTypeInstance instance;
				if (RecurseInstanceUpdate(null, link.EntityTypeInstance, null, link.RoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void FactTypeInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = (FactTypeInstanceHasRoleInstance)e.ModelElement;
				ObjectTypeInstance instance;
				if (RecurseInstanceUpdate(link.FactTypeInstance, null, null, link.RoleInstance.Role, InstanceUpdateType.FactTypeRole, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void ObjectificationInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
				ObjectTypeInstance instance = link.ObjectifyingInstance;
				EntityTypeSubtypeInstance subtypeInstance = instance as EntityTypeSubtypeInstance;
				if (RecurseInstanceUpdate(link.ObjectifiedInstance, (subtypeInstance == null) ? (EntityTypeInstance)instance : null, subtypeInstance, null, InstanceUpdateType.ObjectificationIdentifier, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void ObjectificationInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
				ObjectTypeInstance instance = link.ObjectifyingInstance;
				EntityTypeSubtypeInstance subtypeInstance = instance as EntityTypeSubtypeInstance;
				if (RecurseInstanceUpdate(link.ObjectifiedInstance, (subtypeInstance == null) ? (EntityTypeInstance)instance : null, subtypeInstance, null, InstanceUpdateType.ObjectificationIdentifier, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void ObjectificationInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				ObjectificationInstance link = (ObjectificationInstance)e.ElementLink;
				ObjectTypeInstance instance = link.ObjectifyingInstance;
				EntityTypeSubtypeInstance subtypeInstance = instance as EntityTypeSubtypeInstance;
				EntityTypeInstance entityInstance = (subtypeInstance == null) ? (EntityTypeInstance)instance : null;
				FactTypeInstance factInstance = link.ObjectifiedInstance;
				bool success;
				if (!(success = RecurseInstanceUpdate(factInstance, entityInstance, subtypeInstance, null, InstanceUpdateType.ObjectificationIdentifier, out instance)))
				{
					if (e.DomainRole.Id == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
					{
						factInstance = (FactTypeInstance)e.OldRolePlayer;
					}
					else
					{
						instance = (ObjectTypeInstance)e.OldRolePlayer;
						subtypeInstance = instance as EntityTypeSubtypeInstance;
						entityInstance = (subtypeInstance == null) ? (EntityTypeInstance)instance : null;
					}
					success = RecurseInstanceUpdate(factInstance, entityInstance, subtypeInstance, null, InstanceUpdateType.ObjectificationIdentifier, out instance);
				}
				if (success)
				{
					EditColumnDisplay(0);
				}
			}

			private void FactTypeInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				if (myEditInstance != null)
				{
					FactTypeInstanceHasRoleInstance link = (FactTypeInstanceHasRoleInstance)e.ModelElement;
					ObjectTypeInstance instance;
					if (RecurseInstanceUpdate(link.FactTypeInstance, null, null, link.RoleInstance.Role, InstanceUpdateType.FactTypeRole, out instance))
					{
						EditColumnDisplay(0);
					}
				}
			}

			private void ObjectTypeInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectTypeInstance objectTypeInstance = e.ModelElement as ObjectTypeInstance;
				if (myEditInstance == objectTypeInstance)
				{
					EditColumnDisplay(0);
				}
				else if (myEditInstance != null)
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
#if SAMPLEPOPULATIONEDITOR_DEBUGHELPER
			/// <summary>
			/// The <see cref="FactTypeInstance"/> from the top-most EntityEditorBranch expansion
			/// </summary>
			public FactTypeInstance TopLevelParentFactInstance
			{
				get
				{
					EntityEditorBranch currentBranch = null;
					EntityEditorBranch parentBranch = this;
					while (parentBranch != null)
					{
						currentBranch = parentBranch;
						parentBranch = currentBranch.myParentBranch as EntityEditorBranch;
					}
					return currentBranch.myParentFactInstance;
				}
			}
			/// <summary>
			/// The <see cref="EntityTypeInstance"/> from the top-most EntityEditorBranch expansion
			/// </summary>
			public EntityTypeInstance TopLevelParentEntityInstance
			{
				get
				{
					EntityEditorBranch currentBranch = null;
					EntityEditorBranch parentBranch = this;
					while (parentBranch != null)
					{
						currentBranch = parentBranch;
						parentBranch = currentBranch.myParentBranch as EntityEditorBranch;
					}
					return currentBranch.myParentEntityInstance;
				}
			}
			/// <summary>
			/// The <see cref="EntityTypeSubtypeInstance"/> from the top-most EntityEditorBranch expansion
			/// </summary>
			public EntityTypeSubtypeInstance TopLevelParentSubtypeInstance
			{
				get
				{
					EntityEditorBranch currentBranch = null;
					EntityEditorBranch parentBranch = this;
					while (parentBranch != null)
					{
						currentBranch = parentBranch;
						parentBranch = currentBranch.myParentBranch as EntityEditorBranch;
					}
					return currentBranch.myParentEntitySubtypeInstance;
				}
			}
#endif // SAMPLEPOPULATIONEDITOR_DEBUGHELPER
			/// <summary>
			/// Specify the type of modification that needs to be
			/// applied to expanded branches by the <see cref="RecurseInstanceUpdate"/>
			/// method.
			/// </summary>
			private enum InstanceUpdateType
			{
				/// <summary>
				/// A role instance for an entity type instance
				/// identifier has been added, deleted, or redirected
				/// </summary>
				EntityTypeIdentifierRole,
				/// <summary>
				/// A role instance for a fact instance
				/// has been added, deleted, or redirected
				/// </summary>
				FactTypeRole,
				/// <summary>
				/// A subtype instance relationship has been
				/// modified. From a branch perspective, the
				/// subtype instance plays the parent role in
				/// this relationship.
				/// </summary>
				Subtype,
				/// <summary>
				/// A link between an objectified fact instance
				/// and its identifier has been added, deleted, or
				/// modified. From a branch perspective, the fact
				/// instance plays the parent role in this relationship.
				/// </summary>
				ObjectificationIdentifier,
			}
			/// <summary>
			/// Provide a quick check to see if this branch can
			/// successfully bind to the expected update type.
			/// </summary>
			/// <remarks>An objectification identifier branch may
			/// can also be a subtype.</remarks>
			private bool MatchesUpdateType(InstanceUpdateType updateType)
			{
				switch (updateType)
				{
					case InstanceUpdateType.FactTypeRole:
						return myParentFactType != null && myObjectifyingType == null;
					case InstanceUpdateType.EntityTypeIdentifierRole:
						return myParentFactType == null && myParentEntityType.PreferredIdentifier != null;
					case InstanceUpdateType.Subtype:
						Role editRole = myEditRole;
						return editRole != null && (editRole is SubtypeMetaRole || editRole is SupertypeMetaRole) && !(myParentFactType is SubtypeFact);
					case InstanceUpdateType.ObjectificationIdentifier:
						// This indicates a direct representation of this type of relationship
						// by this branch. We do not check if one of the other branch styles
						// might reference the objectifying instance because this would always
						// be true.
						return myParentFactType != null && myObjectifyingType != null;
				}
				return false;
			}
			/// <summary>
			/// Return the instance being edited by this branch, and update the display for the selected role.
			/// If this is a top-level branch and no instance is currently being edited, then determine the
			/// instance to edit using the <see cref="IUnattachedBranchOwner"/> interface.
			/// </summary>
			/// <param name="parentFactInstance">The fact instance to match</param>
			/// <param name="parentEntityInstance">The entity instance to match</param>
			/// <param name="parentSubtypeInstance">The subtype instance to match</param>
			/// <param name="selectedRole">The role from the parent entity or fact instance, or a supertype role</param>
			/// <param name="updateType">The type of update being performed</param>
			/// <param name="resolvedInstance">The resolved instance. Can be null if a parent branch
			/// recognizes the type of instance, but there is no current instance for this branch of the
			/// sample population.</param>
			/// <returns>true if the <paramref name="resolvedInstance"/> should be used.</returns>
			private bool RecurseInstanceUpdate(FactTypeInstance parentFactInstance, EntityTypeInstance parentEntityInstance, EntityTypeSubtypeInstance parentSubtypeInstance, Role selectedRole, InstanceUpdateType updateType, out ObjectTypeInstance resolvedInstance)
			{
				resolvedInstance = null;
				bool offerToParentBranch = false;
				ObjectTypeInstance parentObjectInstance = (ObjectTypeInstance)parentSubtypeInstance ?? parentEntityInstance;
				if (updateType == InstanceUpdateType.ObjectificationIdentifier &&
					MatchesUpdateType(updateType))
				{
					// Note that the 'MatchesUpdateType' check was designed to provide structure to
					// this spaghetti routine, but recoding this at this point is too sensitive.
					// Consider making better use of the update type when we have a test bed in
					// place to verify the current behavior. The ObjectificationIdentifier case
					// is special because instances always reference the ObjectTypeInstance side
					// of the relationship.
					IBranch parentBranch = myParentBranch;
					EntityEditorBranch parentEditorBranch = parentBranch as EntityEditorBranch;
					IUnattachedBranchOwner unattachedOwner;
					FactTypeInstance contextFactInstance = myParentFactInstance;
					if (parentFactInstance.IsDeleted)
					{
						// There is no way to get type information from a deleted instance, but if we
						// have a current match then we know there is a problem, so we clear the current
						// information unless we're the top-level branch.
						if (contextFactInstance == parentFactInstance)
						{
							if (parentEditorBranch != null)
							{
								if (!parentObjectInstance.IsDeleted && parentEditorBranch.IsInstanceMatch(parentObjectInstance))
								{
									ContextParentObjectInstance = parentObjectInstance;
									resolvedInstance = ContextObjectInstance;
									return true;
								}
								else
								{
									ContextParentFactInstance = null;
								}
							}
							else
							{
								// This branch is top-level and will be deleted with a later event, there
								// is nothing else to do.
								return false;
							}
						}
						offerToParentBranch = true;
					}
					else if (parentFactInstance.FactType == myParentFactType)
					{
						ObjectTypeInstance verifiedIdentifier = parentFactInstance.ObjectifyingInstance;
						if (verifiedIdentifier == null)
						{
							if (parentEditorBranch == null)
							{
								ContextParentObjectInstance = null;
								return true;
							}
							offerToParentBranch = true;
						}
						else if (parentEditorBranch != null)
						{
							if (parentEditorBranch.IsInstanceMatch(verifiedIdentifier))
							{
								ContextParentFactInstance = parentFactInstance;
								resolvedInstance = ContextObjectInstance;
								return true;
							}
							offerToParentBranch = true;
						}
						else if (contextFactInstance != null)
						{
							// Top level branch, matches if set
							if (contextFactInstance == parentFactInstance)
							{
								ContextParentObjectInstance = verifiedIdentifier;
								resolvedInstance = ContextObjectInstance;
								return true;
							}
							return false;
						}
						else if (null != (unattachedOwner = parentBranch as IUnattachedBranchOwner) &&
							unattachedOwner.TryAnchorUnattachedBranches(verifiedIdentifier, parentFactInstance))
						{
							resolvedInstance = ContextObjectInstance;
							return true;
						}
					}
				}

				// Check for a fact instance type match
				if (parentFactInstance != null)
				{
					FactType parentFactType;
					if (null == (parentFactType = myParentFactType) ||
						offerToParentBranch ||
						parentFactInstance.IsDeleted ||
						parentFactInstance.FactType != parentFactType)
					{
						// The type of change did not match here, but if it matches a parent branch, then
						// this branch will likely need to be updated as well. Recurse up the parent chain.
						IBranch parentBranch = myParentBranch;
						EntityEditorBranch parentEditorBranch;
						if (null != (parentEditorBranch = parentBranch as EntityEditorBranch))
						{
							ObjectTypeInstance resolvedParentInstance;
							if (parentEditorBranch.RecurseInstanceUpdate(parentFactInstance, parentEntityInstance, parentSubtypeInstance, selectedRole, updateType, out resolvedParentInstance))
							{
								ContextParentObjectInstance = resolvedParentInstance;
								resolvedInstance = ContextObjectInstance;
								return true;
							}
						}
						if (updateType != InstanceUpdateType.ObjectificationIdentifier) // Try matching the object instance part in addition to the fact instance
						{
							return false;
						}
					}
					if (updateType == InstanceUpdateType.ObjectificationIdentifier)
					{
						IBranch parentBranch = myParentBranch;
						EntityEditorBranch parentEditorBranch = parentBranch as EntityEditorBranch;
						if (null != parentEditorBranch && parentEditorBranch.IsInstanceMatch(parentObjectInstance))
						{
							ContextParentObjectInstance = parentObjectInstance;
							resolvedInstance = ContextObjectInstance;
							return true;
						}
						return false;
					}
				}
				else if (updateType != InstanceUpdateType.Subtype && myParentFactType != null && myObjectifyingType == null)
				{
					return false;
				}

				// Check for an object instance type match, after a quick sanity check on the provided role
				ObjectType parentEntityType;
				Role editRole = myEditRole;
				bool checkSubSuper = !(myParentFactType is SubtypeFact);
				SupertypeMetaRole supertypeEditRole = checkSubSuper ? editRole as SupertypeMetaRole : null;
				SubtypeMetaRole subtypeEditRole = (checkSubSuper && supertypeEditRole == null) ? editRole as SubtypeMetaRole : null;
				if (selectedRole != null &&
					selectedRole != editRole)
				{
					return false;
				}
				if (parentEntityInstance != null &&
					null != (parentEntityType = ContextObjectType) &&
					!parentObjectInstance.IsDeleted &&
					parentObjectInstance.ObjectType != parentEntityType)
				{
					// The type of change did not match here, but if it matches a parent branch, then
					// this branch will likely need to be updated as well. Recurse up the parent chain.
					IBranch parentBranch = myParentBranch;
					EntityEditorBranch parentEditorBranch;
					if (null != (parentEditorBranch = parentBranch as EntityEditorBranch))
					{
						// Note that we recurse even on a match to handle cases where the parent entity information
						// is out of date. The parent entity information is only stable for the top-level branch.
						ObjectTypeInstance resolvedParentInstance;
						if (parentEditorBranch.RecurseInstanceUpdate(parentFactInstance, parentEntityInstance, parentSubtypeInstance, selectedRole, updateType, out resolvedParentInstance))
						{
							ContextParentObjectInstance = resolvedParentInstance;
							resolvedInstance = ContextObjectInstance;
							return true;
						}
					}
					else
					{
						EntityTypeSubtypeInstance contextSubtypeInstance = ContextParentSubtypeInstance;
						if (contextSubtypeInstance != null)
						{
							if (parentSubtypeInstance == contextSubtypeInstance)
							{
								ContextParentObjectInstance = parentSubtypeInstance;
								resolvedInstance = ContextObjectInstance;
								return true;
							}
						}
						else if (parentEntityInstance == ContextParentEntityInstance)
						{
							resolvedInstance = ContextObjectInstance;
							return true;
						}
					}
					return false;
				}
				EntityTypeInstance editInstance = myEditInstance;
				if (editInstance != null)
				{
					// We have a match, so there is no need to go farther as long as
					// everything is still in sync. Do a quick verification and defer
					// to full processing if things are out of whack.
					bool instanceChanged = false;
					if (parentSubtypeInstance != null)
					{
						EntityTypeSubtypeInstance contextSubtypeInstance = ContextParentSubtypeInstance;
						if (contextSubtypeInstance != null)
						{
							if (parentSubtypeInstance == contextSubtypeInstance)
							{
								ContextParentObjectInstance = parentSubtypeInstance.IsDeleted ? null : parentSubtypeInstance;
								resolvedInstance = ContextObjectInstance;
								return true;
							}
						}
						else if (null != (contextSubtypeInstance = myEditSubtypeInstance) &&
							parentSubtypeInstance == contextSubtypeInstance)
						{
							ContextObjectInstance = parentSubtypeInstance.IsDeleted ? null : parentSubtypeInstance;
							resolvedInstance = ContextObjectInstance;
							return true;
						}
					}
					else if (parentEntityInstance != null)
					{
						if (parentEntityInstance == editInstance)
						{
							if (subtypeEditRole != null)
							{
								EntityTypeSubtypeInstance subtypeInstance = myEditSubtypeInstance;
								if (subtypeInstance != null)
								{
									if (subtypeInstance.IsDeleted)
									{
										ContextParentObjectInstance = null;
										return true;
									}
									else if (subtypeInstance.SupertypeInstance == editInstance)
									{
										resolvedInstance = subtypeInstance;
										return true;
									}
								}
								instanceChanged = true;
							}
							else if (supertypeEditRole != null)
							{
								EntityTypeSubtypeInstance subtypeInstance = myParentEntitySubtypeInstance;
								if (subtypeInstance != null)
								{
									if (subtypeInstance.IsDeleted)
									{
										ContextParentObjectInstance = null;
										return true;
									}
									else if (subtypeInstance.SupertypeInstance == editInstance)
									{
										resolvedInstance = editInstance;
										return true;
									}
								}
								instanceChanged = true;
							}
							else
							{
								ObjectTypeInstance verifyInstance = (ObjectTypeInstance)myEditSubtypeInstance ?? editInstance;
								if (verifyInstance.IsDeleted)
								{
									ObjectTypeInstance contextParent = ContextParentObjectInstance;
									if (contextParent != null && contextParent.IsDeleted)
									{
										ContextParentObjectInstance = null;
									}
									else
									{
										ContextObjectInstance = null;
									}
									return true;
								}
								EntityTypeRoleInstance roleInstance = editInstance.FindRoleInstance(editRole);
								if (((roleInstance != null) ? roleInstance.ObjectTypeInstance : null) == verifyInstance)
								{
									resolvedInstance = verifyInstance;
									return true;
								}
								instanceChanged = true;
							}
						}
					}
					else if (parentFactInstance != null)
					{
						if (parentFactInstance == myParentFactInstance)
						{
#if SAMPLEPOPULATIONEDITOR_DEBUGHELPER
							// The following cases may be possible, but I have not found any
							// way to hit this code path. Leave them under the debug helper
							// switch for future investigation
							if (subtypeEditRole != null || supertypeEditRole != null)
							{
								Debug.Fail("Subtype/supertype ParentFactInstance case");
							}
							else if (editRole != null)
							{
								Debug.Assert(myObjectifyingType == null, "ObjectifyingIdentifier case");
#else
							if (editRole != null && subtypeEditRole != null && supertypeEditRole != null && myObjectifyingType == null)
							{
#endif
								ObjectTypeInstance verifyInstance = (ObjectTypeInstance)myEditSubtypeInstance ?? editInstance;
								if (verifyInstance.IsDeleted)
								{
									ContextObjectInstance = null;
									return true;
								}
								FactTypeRoleInstance roleInstance = parentFactInstance.FindRoleInstance(editRole);
								if (((roleInstance != null) ? roleInstance.ObjectTypeInstance : null) == verifyInstance)
								{
									ContextObjectInstance = verifyInstance;
									resolvedInstance = ContextObjectInstance;
									return true;
								}
								instanceChanged = true;
							}
						}
					}
					if (!instanceChanged)
					{
						return false;
					}
				}
				if (supertypeEditRole != null || subtypeEditRole != null)
				{
					EntityTypeSubtypeInstance subtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(parentEntityInstance, myParentEntityType, true, false);
					if (subtypeInstance != null)
					{
						FactTypeInstance contextFactInstance;
						if (null == (contextFactInstance = myParentFactInstance) || contextFactInstance.ObjectifyingInstance == subtypeInstance)
						{
							IBranch parentBranch = myParentBranch;
							EntityEditorBranch parentEditorBranch;
							IUnattachedBranchOwner unattachedOwner;
							bool matchingSubtypeInstance = false;
							if (null != (parentEditorBranch = parentBranch as EntityEditorBranch))
							{
								matchingSubtypeInstance = parentEditorBranch.IsInstanceMatch(subtypeInstance);
							}
							else if (contextFactInstance != null ||
								null != ContextParentSubtypeInstance)
							{
								// The top-level branch is attached to a specific instance, reset the associated entity
								matchingSubtypeInstance = true;
							}
							else if (null != (unattachedOwner = parentBranch as IUnattachedBranchOwner) &&
								unattachedOwner.TryAnchorUnattachedBranches(subtypeInstance, null))
							{
								resolvedInstance = ContextObjectInstance;
								return true;
							}
							if (matchingSubtypeInstance)
							{
								ContextParentObjectInstance = subtypeInstance;
								resolvedInstance = ContextObjectInstance;
								return true;
							}
						}
					}
					return false;
				}
				else
				{
					ObjectType objectifyingType = myObjectifyingType;
					bool factMember = myParentFactType != null && objectifyingType == null;
					ObjectTypeInstance recurseParentObjectInstance = parentObjectInstance ?? (!factMember ? parentFactInstance.ObjectifyingInstance : null);
					if (recurseParentObjectInstance != null)
					{
						IBranch parentBranch = myParentBranch;
						EntityEditorBranch parentEditorBranch;
						IUnattachedBranchOwner unattachedOwner;
						if (null != (parentEditorBranch = parentBranch as EntityEditorBranch))
						{
							ObjectTypeInstance instance = null;
							foreach (RoleInstance recurseParentRoleInstance in RoleInstance.GetLinksToRoleCollection(recurseParentObjectInstance))
							{
								if (recurseParentRoleInstance.Role != editRole)
								{
									continue;
								}
								EntityTypeRoleInstance recurseEntityRoleInstance;
								FactTypeRoleInstance recurseFactRoleInstance;
								bool recurseMatches = false;
								if (null != (recurseEntityRoleInstance = recurseParentRoleInstance as EntityTypeRoleInstance))
								{
									recurseMatches = parentEditorBranch.RecurseInstanceUpdate(null, recurseEntityRoleInstance.EntityTypeInstance, null, recurseParentRoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out instance);
								}
								else if (null != (recurseFactRoleInstance = recurseParentRoleInstance as FactTypeRoleInstance))
								{
									recurseMatches = parentEditorBranch.RecurseInstanceUpdate(recurseFactRoleInstance.FactTypeInstance, null, null, recurseParentRoleInstance.Role, InstanceUpdateType.FactTypeRole, out instance);
								}
								if (recurseMatches)
								{
									ContextParentObjectInstance = instance;
									resolvedInstance = ContextObjectInstance;
									return true;
								}
							}
							EntityTypeInstance recurseEntityInstance = recurseParentObjectInstance as EntityTypeInstance;
							if (recurseEntityInstance != null)
							{
								foreach (EntityTypeSubtypeInstance subtypeInstance in recurseEntityInstance.EntityTypeSubtypeInstanceCollection)
								{
									if (parentEditorBranch.RecurseInstanceUpdate(null, recurseEntityInstance, subtypeInstance, null, InstanceUpdateType.Subtype, out instance))
									{
										ContextObjectInstance = instance;
										resolvedInstance = instance;
										return true;
									}
								}
							}
						}
						else if (null != (unattachedOwner = parentBranch as IUnattachedBranchOwner))
						{
							if (factMember)
							{
								FactTypeInstance contextFactInstance = myParentFactInstance;
								foreach (FactTypeRoleInstance recurseFactRoleInstance in FactTypeRoleInstance.GetLinksToRoleCollection(recurseParentObjectInstance))
								{
									if (recurseFactRoleInstance.Role != editRole)
									{
										continue;
									}
									if (null != contextFactInstance)
									{
										if (contextFactInstance == recurseFactRoleInstance.FactTypeInstance)
										{
											ContextObjectInstance = recurseParentObjectInstance;
											resolvedInstance = ContextObjectInstance;
											return true;
										}

									}
									else if (unattachedOwner.TryAnchorUnattachedBranches(null, recurseFactRoleInstance.FactTypeInstance))
									{
										resolvedInstance = ContextObjectInstance;
										return true;
									}
								}
							}
							else
							{
								ObjectTypeInstance contextInstance = ContextParentEntityInstance;
								if (null != contextInstance)
								{
									if (contextInstance == recurseParentObjectInstance)
									{
										ContextObjectInstance = recurseParentObjectInstance;
										resolvedInstance = ContextObjectInstance;
										return true;
									}
								}
								else if (unattachedOwner.TryAnchorUnattachedBranches(recurseParentObjectInstance, null))
								{
									resolvedInstance = ContextObjectInstance;
									return true;
								}
							}
							return false;
						}
					}
					else if (factMember && parentFactInstance != null)
					{
						EntityEditorBranch parentBranch;
						if (null == (parentBranch = myParentBranch as EntityEditorBranch) ||
							parentBranch.IsInstanceMatch(parentFactInstance.ObjectifyingInstance))
						{
							ContextParentFactInstance = parentFactInstance;
							resolvedInstance = ContextObjectInstance;
							return true;
						}
					}
				}
				return false;
			}
			/// <summary>
			/// Determine if the specified <paramref name="matchInstance"/> matches
			/// the instance for this branch.
			/// </summary>
			/// <remarks>Helper function for <see cref="RecurseInstanceUpdate"/></remarks>
			private bool IsInstanceMatch(ObjectTypeInstance matchInstance)
			{
				if (matchInstance == null)
				{
					return false;
				}
				else if (myEditRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact))
				{
					ObjectTypeInstance testInstance;
					if (null == (testInstance = ContextObjectInstance))
					{
						FactTypeInstance factInstance;
						IUnattachedBranchOwner unattachedOwner;
						if (null != (factInstance = myParentFactInstance))
						{
							if (factInstance.ObjectifyingInstance == matchInstance)
							{
								ContextObjectInstance = matchInstance;
								return true;
							}
						}
						else if (null != (unattachedOwner = myParentBranch as IUnattachedBranchOwner) &&
							unattachedOwner.TryAnchorUnattachedBranches(matchInstance, null))
						{
							testInstance = ContextObjectInstance;
						}
					}
					return matchInstance == testInstance;
				}
				else
				{
					foreach (RoleInstance testRoleInstance in RoleInstance.GetLinksToRoleCollection(matchInstance))
					{
						EntityTypeRoleInstance testEntityRoleInstance;
						FactTypeRoleInstance testFactRoleInstance;
						ObjectTypeInstance testInstance = null;
						bool recurseMatches = false;
						if (null != (testEntityRoleInstance = testRoleInstance as EntityTypeRoleInstance))
						{
							recurseMatches = RecurseInstanceUpdate(null, testEntityRoleInstance.EntityTypeInstance, null, testRoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out testInstance);
						}
						else if (null != (testFactRoleInstance = testRoleInstance as FactTypeRoleInstance))
						{
							recurseMatches = RecurseInstanceUpdate(testFactRoleInstance.FactTypeInstance, null, null, testRoleInstance.Role, InstanceUpdateType.FactTypeRole, out testInstance);
						}
						if (recurseMatches)
						{
							return matchInstance == testInstance;
						}
					}
					EntityTypeInstance recurseEntityInstance = matchInstance as EntityTypeInstance;
					if (recurseEntityInstance != null)
					{
						foreach (EntityTypeSubtypeInstance subtypeInstance in recurseEntityInstance.EntityTypeSubtypeInstanceCollection)
						{
							ObjectTypeInstance testInstance;
							if (RecurseInstanceUpdate(null, recurseEntityInstance, subtypeInstance, null, InstanceUpdateType.Subtype, out testInstance))
							{
								return matchInstance == testInstance;
							}
						}
					}
				}
				return false;
			}
			#endregion // Event Handlers
			#region IUnattachedBranch Implementation
			void IUnattachedBranch.AnchorUnattachedBranch(ObjectTypeInstance objectInstance, FactTypeInstance factInstance)
			{
				if (myParentFactType != null)
				{
					if (objectInstance == null)
					{
						if (factInstance != null)
						{
							objectInstance = factInstance.ObjectifyingInstance;
						}
					}
					else if (factInstance == null)
					{
						if (objectInstance != null)
						{
							factInstance = objectInstance.ObjectifiedInstance;
						}
					}
					ContextParentFactInstance = factInstance;
				}
				else
				{
					if (objectInstance == null && factInstance != null)
					{
						objectInstance = factInstance.ObjectifyingInstance;
					}
					ContextParentObjectInstance = objectInstance;
				}
			}
			#endregion // IUnattachedBranch Implementation
			#region Helper Methods
			public sealed override bool IsFullRowSelectColumn(int column)
			{
				return false;
			}
			private ObjectType ContextObjectType
			{
				get
				{
					return myObjectifyingType ?? myEditRole.RolePlayer;
				}
			}
			/// <summary>
			/// Read-only typed version of <see cref="ContextParentObjectInstance"/>
			/// </summary>
			private EntityTypeInstance ContextParentEntityInstance
			{
				get
				{
					return (myEditRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact)) ? myEditInstance : myParentEntityInstance;
				}
			}
			/// <summary>
			/// Return the parent <see cref="EntityTypeSubtypeInstance"/> if it is set
			/// </summary>
			private EntityTypeSubtypeInstance ContextParentSubtypeInstance
			{
				get
				{
					return (myEditRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact)) ? myEditSubtypeInstance : myParentEntitySubtypeInstance;
				}
			}
			/// <summary>
			/// Control setting the instance and subtypeinstance fields
			/// </summary>
			private ObjectTypeInstance ContextParentObjectInstance
			{
				get
				{
					return ContextParentEntityInstance;
				}
				set
				{
					Role editRole = myEditRole;
					ObjectType objectifyingType = myObjectifyingType;
					if (myParentFactType != null)
					{
						FactTypeInstance resolvedFactInstance = (value != null) ? value.ObjectifiedInstance : null;
						bool updateFactInstance = myParentBranch is EntityEditorBranch;
						if (!updateFactInstance)
						{
							// The parent fact instance can change for an expanded. However, if we're a
							// top-level branch, then the parent fact instance cannot change.
							FactTypeInstance parentFactInstance = myParentFactInstance;
							updateFactInstance =
								(parentFactInstance != null && parentFactInstance == resolvedFactInstance) || // Resets edit instance
								(parentFactInstance == null && resolvedFactInstance != null);
						}
						if (updateFactInstance)
						{
							if (objectifyingType != null)
							{
								myParentFactInstance = resolvedFactInstance;
							}
							else
							{
								ContextParentFactInstance = resolvedFactInstance;
								return;
							}
						}
						else if (objectifyingType == null)
						{
							return;
						}
					}
					if (editRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact))
					{
						// Parent fields are not set, just the instance
						ContextObjectInstance = value;
					}
					else
					{
						EntityTypeInstance entityInstance = null;
						EntityTypeSubtypeInstance subtypeInstance = null;
						if (null != value &&
							null == (entityInstance = value as EntityTypeInstance) &&
							null != (subtypeInstance = value as EntityTypeSubtypeInstance))
						{
							entityInstance = subtypeInstance.SupertypeInstance;
						}
						if (entityInstance != null)
						{
							myParentEntityInstance = entityInstance;
							myParentEntitySubtypeInstance = subtypeInstance;
							if (objectifyingType != null)
							{
								// Match the parent instances
								myEditInstance = entityInstance;
								myEditSubtypeInstance = subtypeInstance;
							}
							else if (editRole is SupertypeMetaRole && !(myParentFactType is SubtypeFact))
							{
								// Match the parent entity, but not the subtype
								ContextObjectInstance = entityInstance;
							}
							else
							{
								ObjectTypeInstance foundInstance = null;
								RoleInstance foundRoleInstance = entityInstance.FindRoleInstance(editRole);
								if (foundRoleInstance != null)
								{
									foundInstance = foundRoleInstance.ObjectTypeInstance;
								}
								ContextObjectInstance = foundInstance;
							}
						}
						else
						{
							myParentEntityInstance = null;
							myParentEntitySubtypeInstance = null;
							myEditInstance = null;
							myEditSubtypeInstance = null;
						}
					}
				}
			}
			/// <summary>
			/// Control setting the parent fact instance
			/// </summary>
			private FactTypeInstance ContextParentFactInstance
			{
				get
				{
					return myParentFactInstance;
				}
				set
				{
					ObjectType objectifyingType = myObjectifyingType;
					myParentFactInstance = value;
					if (objectifyingType != null)
					{
						ContextParentObjectInstance = (value != null) ? value.ObjectifyingInstance : null;
					}
					else
					{
						Debug.Assert(myParentEntityType == null); // Only the identifier role can have both a parent EntityType and a parent FactType
						ObjectTypeInstance localInstance = null;
						FactTypeRoleInstance roleInstance;
						if (value != null &&
							null != (roleInstance = value.FindRoleInstance(myEditRole)))
						{
							localInstance = roleInstance.ObjectTypeInstance;
						}
						ContextObjectInstance = localInstance;
					}
				}
			}
			/// <summary>
			/// Control setting the instance and subtypeinstance fields
			/// </summary>
			private ObjectTypeInstance ContextObjectInstance
			{
				get
				{
					return (ObjectTypeInstance)myEditSubtypeInstance ?? myEditInstance;
				}
				set
				{
					EntityTypeSubtypeInstance subtypeInstance;
					if (value == null)
					{
						myEditInstance = null;
						myEditSubtypeInstance = null;
					}
					else if (null != (subtypeInstance = value as EntityTypeSubtypeInstance))
					{
						myEditSubtypeInstance = subtypeInstance;
						myEditInstance = subtypeInstance.SupertypeInstance;
					}
					else
					{
						myEditInstance = (EntityTypeInstance)value;
						myEditSubtypeInstance = null;
					}
				}
			}
			/// <summary>
			/// An enum used to classify the type of row
			/// </summary>
			private enum RowType
			{
				/// <summary>
				/// The row is part of the context EntityType's preferred identifier
				/// </summary>
				IdentifierRole,
				/// <summary>
				/// The row is the supertype instance for a context subtype
				/// </summary>
				Supertype,
				/// <summary>
				/// The row for the objectified FactType
				/// </summary>
				FactRole,
				/// <summary>
				/// The row is an external identifier for an objectified FactType
				/// </summary>
				ObjectifyingIdentifier,
			}
			private RowType ResolveRow(ref int row, out ObjectType rowType, out ObjectTypeInstance rowInstance)
			{
				Role dummyRole;
				FactTypeInstance dummyFactInstance;
				return ResolveRow(ref row, out rowType, out dummyRole, out dummyFactInstance, out rowInstance);
			}
			private RowType ResolveRow(ref int row, out ObjectType rowType, out Role rowRole, out FactTypeInstance rowFactInstance, out ObjectTypeInstance rowInstance)
			{
				rowType = null;
				rowInstance = null;
				rowRole = null;
				rowFactInstance = null;
				ObjectType contextType = myObjectifyingType;
				bool testObjectification = false; // Testing the parent's objectifying type will give another instance of the same branch
				if (contextType == null)
				{
					contextType = myEditRole.RolePlayer;
					testObjectification = true;
				}
				FactType objectifiedFactType;
				UniquenessConstraint pid = contextType.ResolvedPreferredIdentifier;
				if (testObjectification &&
					null != (objectifiedFactType = contextType.NestedFactType))
				{
					ObjectTypeInstance contextInstance = (ObjectTypeInstance)myEditSubtypeInstance ?? myEditInstance;
					if (pid.PreferredIdentifierFor != contextType || !pid.IsObjectifiedPreferredIdentifier)
					{
						if (row == 0)
						{
							rowType = contextType;
							if (contextInstance != null)
							{
								rowInstance = contextInstance;
								rowFactInstance = contextInstance.ObjectifiedInstance;
							}
							return RowType.ObjectifyingIdentifier;
						}
						--row;
					}
					Role role = objectifiedFactType.OrderedRoleCollection[row].Role;
					rowRole = role;
					rowType = role.RolePlayer;
					if (contextInstance != null)
					{
						FactTypeInstance objectifiedInstance;
						FactTypeRoleInstance roleInstance;
						if (null != (objectifiedInstance = contextInstance.ObjectifiedInstance))
						{
							rowFactInstance = objectifiedInstance;
							if (null != (roleInstance = objectifiedInstance.FindRoleInstance(role)))
							{
								rowInstance = roleInstance.ObjectTypeInstance;
							}
						}
					}
					return RowType.FactRole;
				}
				ObjectType preferredFor = pid.PreferredIdentifierFor;
				if (preferredFor != contextType)
				{
					rowType = contextType;
					rowInstance = myEditInstance;
					return RowType.Supertype;
				}
				else
				{
					Role identifierRole = pid.RoleCollection[row];
					rowRole = identifierRole;
					rowType = identifierRole.RolePlayer;
					EntityTypeInstance instance;
					EntityTypeRoleInstance roleInstance;
					if (null != (instance = myEditInstance) &&
						null != (roleInstance = instance.FindRoleInstance(identifierRole)))
					{
						rowInstance = roleInstance.ObjectTypeInstance;
					}
					return RowType.IdentifierRole;
				}
			}
			private RowType ResolveRow(ref int row, out ObjectType rowType)
			{
				Role dummyRole;
				return ResolveRow(ref row, out rowType, out dummyRole);
			}
			private RowType ResolveRow(ref int row, out ObjectType rowType, out Role rowRole)
			{
				rowType = null;
				rowRole = null;
				ObjectType contextType = myObjectifyingType;
				bool testObjectification = false; // Testing the parent's objectifying type will give another instance of the same branch
				if (contextType == null)
				{
					contextType = myEditRole.RolePlayer;
					testObjectification = true;
				}
				FactType objectifiedFactType;
				UniquenessConstraint pid = contextType.ResolvedPreferredIdentifier;
				if (testObjectification &&
					null != (objectifiedFactType = contextType.NestedFactType))
				{
					if (pid.PreferredIdentifierFor != contextType || !pid.IsObjectifiedPreferredIdentifier)
					{
						if (row == 0)
						{
							rowType = contextType;
							return RowType.ObjectifyingIdentifier;
						}
						--row;
					}
					Role role = objectifiedFactType.OrderedRoleCollection[row].Role;
					rowRole = role;
					rowType = role.RolePlayer;
					return RowType.FactRole;
				}
				ObjectType preferredFor = pid.PreferredIdentifierFor;
				if (preferredFor != contextType)
				{
					rowType = preferredFor;
					return RowType.Supertype;
				}
				else
				{
					Role identifierRole = pid.RoleCollection[row];
					rowRole = identifierRole;
					rowType = identifierRole.RolePlayer;
					return RowType.IdentifierRole;
				}
			}
			/// <summary>
			/// Hook up the given instance from a child branch to its parent instance on the current branch.
			/// If the parent instance doesn't exist, it will be created.  The method then calls back up
			/// the chain to the current branch's parent branch, passing the newly
			/// created instance to be connected and the role to connect on.
			/// </summary>
			/// <param name="instance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect on</param>
			/// <param name="objectifyingType">The objectifying type. Used in place of <paramref name="identifierRole"/> to edit the objectification relationship.</param>
			/// <param name="factInstance">The fact instance from an objectified FactType</param>
			public void ConnectInstance(ObjectTypeInstance instance, Role identifierRole, ObjectType objectifyingType, FactTypeInstance factInstance)
			{
				Debug.Assert(instance != null);
				Store store = Store;

				EntityTypeInstance editInstance = myEditInstance;
				ObjectTypeInstance recurseConnectInstance = null;

				if ((identifierRole is SupertypeMetaRole || identifierRole is SubtypeMetaRole) && (factInstance == null || !(factInstance.FactType is SubtypeFact)))
				{
					ObjectType subtype = ContextObjectType;
					EntityTypeInstance entityInstance = (EntityTypeInstance)instance;
					EntityTypeSubtypeInstance editSubtypeInstance = myEditSubtypeInstance;
					if (editSubtypeInstance != null && editSubtypeInstance.SupertypeInstance == instance)
					{
						// No change, leave the instances bound
						recurseConnectInstance = editSubtypeInstance;
					}
					else
					{
						// If the instance is already bound to a subtype instance for this subtype,
						// then use the existing instance.
						EntityTypeSubtypeInstance existingSubtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, subtype, true, false);
						if (existingSubtypeInstance != null)
						{
							myEditSubtypeInstance = existingSubtypeInstance;
						}
					}
					if (recurseConnectInstance == null)
					{
						if (editSubtypeInstance == null)
						{
							editSubtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(entityInstance, subtype, true, true);
						}
						else
						{
							editSubtypeInstance.SupertypeInstance = entityInstance;
						}
						recurseConnectInstance = editSubtypeInstance;
					}
				}

				if (recurseConnectInstance == null)
				{
					if (identifierRole == null)
					{
						recurseConnectInstance = instance;
						ObjectTypeInstance startingInstance;
						FactTypeInstance attachedFactInstance;
						if (null != (startingInstance = ContextObjectInstance) &&
							null != (attachedFactInstance = startingInstance.ObjectifiedInstance))
						{
							ConnectObjectifyingIdentifierInstance(attachedFactInstance, instance);
						}
					}
					else
					{
						ObjectType editRolePlayer = myObjectifyingType;
						bool checkObjectification = false;
						if (editRolePlayer == null)
						{
							checkObjectification = true;
							editRolePlayer = myEditRole.RolePlayer;
						}
						UniquenessConstraint pid = editRolePlayer.ResolvedPreferredIdentifier;
						ObjectType identifierFor = pid.PreferredIdentifierFor;
						FactType objectifiedFactType = checkObjectification ? editRolePlayer.NestedFactType : null;

						if (objectifiedFactType != null &&
							identifierRole != null)
						{
							Debug.Assert(objectifiedFactType.RoleCollection.Contains(identifierRole));
							ObjectTypeInstance objectifyingInstance = (ObjectTypeInstance)myEditSubtypeInstance ?? myEditInstance;
							FactTypeInstance objectifiedInstance = (objectifyingInstance != null) ? objectifyingInstance.ObjectifiedInstance : factInstance;
							if (objectifiedInstance == null)
							{
								bool newInstance = objectifyingInstance == null;
								CreateImpliedObjectInstance(editRolePlayer, objectifiedFactType, ref objectifyingInstance, ref objectifiedInstance);
								if (newInstance)
								{
									ContextObjectInstance = objectifyingInstance;
								}
								new FactTypeRoleInstance(identifierRole, instance).FactTypeInstance = objectifiedInstance;
							}
							else
							{
								objectifiedInstance.EnsureRoleInstance(identifierRole, instance);
							}
							recurseConnectInstance = objectifyingInstance;
						}
						else
						{
							Debug.Assert(pid.RoleCollection.Contains(identifierRole));
							if (editInstance == null)
							{
								EntityTypeSubtypeInstance subtypeInstance = null;
								switch (identifierRole.GetReferenceSchemePattern())
								{
									case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
									case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
										// The FactType patterns are one-to-one and implied. We should never
										// create another instance if one is already available.
										foreach (EntityTypeRoleInstance testRoleInstance in EntityTypeRoleInstance.GetLinksToRoleCollection(instance))
										{
											EntityTypeInstance testEntityInstance = testRoleInstance.EntityTypeInstance;
											if (testEntityInstance.EntityType == identifierFor)
											{
												editInstance = testEntityInstance;
												if (editRolePlayer != identifierFor)
												{
													subtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(editInstance, editRolePlayer, true, false);
												}
												break;
											}
										}
										break;
								}
								if (editInstance == null)
								{
									LinkedElementCollection<FactType> pidFactTypes;
									FactType identifierForObjectifiedFactType;
									FactType identifierFactType;
									Role unaryRole;
									ObjectifiedUnaryRole objectifiedUnaryRole;
									if (pid.IsInternal &&
										null != (identifierForObjectifiedFactType = ((identifierFor != editRolePlayer) ? identifierFor.NestedFactType : objectifiedFactType)) &&
										1 == (pidFactTypes = pid.FactTypeCollection).Count &&
										((identifierFactType = pidFactTypes[0]) == identifierForObjectifiedFactType ||
										(null != (unaryRole = identifierForObjectifiedFactType.UnaryRole) &&
										null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
										identifierFactType == objectifiedUnaryRole.FactType)))
									{
										// Entity instances are implied, create indirectly through the objectified FactType
										factInstance = new FactTypeInstance(store);
										factInstance.FactType = identifierForObjectifiedFactType;
										new FactTypeRoleInstance(identifierRole, instance).FactTypeInstance = factInstance;
										editInstance = (EntityTypeInstance)factInstance.ObjectifyingInstance;
										Debug.Assert(editInstance != null); // The ObjectifyingInstance should be populated by inline rules
									}
									else
									{
										editInstance = new EntityTypeInstance(store);
										editInstance.EntityType = identifierFor;
										new EntityTypeRoleInstance(identifierRole, instance).EntityTypeInstance = editInstance;
									}
								}
								if (subtypeInstance == null && editRolePlayer != identifierFor)
								{
									subtypeInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(editInstance, editRolePlayer, false, true);
								}
								recurseConnectInstance = (ObjectTypeInstance)subtypeInstance ?? editInstance;
							}
							else if (pid.IsObjectifiedPreferredIdentifier)
							{
								// Control population through the FactType
								editInstance.ObjectifiedInstance.EnsureRoleInstance(identifierRole, instance);
							}
							else
							{
								editInstance.EnsureRoleInstance(identifierRole, instance);
							}
						}
					}
				}
				if (recurseConnectInstance != null)
				{
					EntityTypeBranch entityBranch;
					FactTypeBranch factBranch;
					EntityEditorBranch editBranch;
					if (null != (entityBranch = myParentBranch as EntityTypeBranch))
					{
						EntityTypeBranch.ConnectInstance(entityBranch.EntityType, entityBranch.EntityTypeSubtype, ref myParentEntityInstance, ref myParentEntitySubtypeInstance, recurseConnectInstance, myEditRole);
					}
					else if (null != (factBranch = myParentBranch as FactTypeBranch))
					{
						ObjectType contextObjectifyingType = myObjectifyingType;
						FactTypeBranch.ConnectInstance(ref myParentFactInstance, recurseConnectInstance, (contextObjectifyingType == null) ? myEditRole : null, contextObjectifyingType);
					}
					else if (null != (editBranch = myParentBranch as EntityEditorBranch))
					{
						ObjectType contextObjectifyingType = myObjectifyingType;
						editBranch.ConnectInstance(recurseConnectInstance, (contextObjectifyingType == null) ? myEditRole : null, contextObjectifyingType, myParentFactInstance);
					}
					else
					{
						Debug.Fail("Branch is of an unknown type");
					}
				}
			}
			/// <summary>
			/// Recursively create an <see cref="ObjectTypeInstance"/> required for editing an objectified <see cref="FactTypeInstance"/>
			/// </summary>
			/// <param name="objectifyingType">The objectifying <see cref="ObjectType"/>. Cannot be null.</param>
			/// <param name="objectifiedFactType">The objectified <see cref="FactType"/>. Cannot be null.</param>
			/// <param name="objectInstance">The current or created <see cref="ObjectTypeInstance"/></param>
			/// <param name="factInstance">The current or created <see cref="FactTypeInstance"/></param>
			private static void CreateImpliedObjectInstance(ObjectType objectifyingType, FactType objectifiedFactType, ref ObjectTypeInstance objectInstance, ref FactTypeInstance factInstance)
			{
				ObjectTypeInstance editObjectInstance = objectInstance;
				FactTypeInstance editFactInstance = factInstance;
				if (editObjectInstance != null && editFactInstance != null)
				{
					return;
				}
				Store store = objectifyingType.Store;
				bool impliedObjectInstance = false;
				if (editObjectInstance == null)
				{
					UniquenessConstraint pid = objectifyingType.ResolvedPreferredIdentifier;
					ObjectType identifierFor = pid.PreferredIdentifierFor;
					if (identifierFor == objectifyingType)
					{
						if (pid.IsObjectifiedPreferredIdentifier)
						{
							impliedObjectInstance = true;
						}
						else
						{
							EntityTypeInstance editEntityInstance = new EntityTypeInstance(store);
							editEntityInstance.EntityType = objectifyingType;
							editObjectInstance = editEntityInstance;
						}
					}
					else
					{
						FactType objectifiedByIdentifierFor = identifierFor.NestedFactType;
						EntityTypeInstance supertypeInstance = null;
						if (objectifiedByIdentifierFor != null)
						{
							ObjectTypeInstance recurseObjectInstance = null;
							FactTypeInstance recurseFactInstance = null;
							CreateImpliedObjectInstance(identifierFor, objectifiedByIdentifierFor, ref recurseObjectInstance, ref recurseFactInstance);
							supertypeInstance = (EntityTypeInstance)recurseObjectInstance;
						}
						else
						{
							supertypeInstance = new EntityTypeInstance(store);
							supertypeInstance.EntityType = identifierFor;
						}
						editObjectInstance = EntityTypeSubtypeInstance.GetSubtypeInstance(supertypeInstance, objectifyingType, false, true);
						if (editFactInstance != null)
						{
							editObjectInstance.ObjectifiedInstance = factInstance;
						}
					}
				}
				if (editFactInstance == null)
				{
					editFactInstance = new FactTypeInstance(store);
					editFactInstance.FactType = objectifiedFactType;
					if (impliedObjectInstance)
					{
						editObjectInstance = editFactInstance.ObjectifyingInstance;
					}
					else
					{
						editFactInstance.ObjectifyingInstance = editObjectInstance;
					}
				}
				objectInstance = editObjectInstance;
				factInstance = editFactInstance;
			}
			#endregion // Helper Methods
			#region Base overrides
			protected override CellEditContext CreateEditContext(int row, int column)
			{
				CellEditContext retVal = null;
				ObjectType rowType;
				ObjectTypeInstance rowInstance;
				Role rowRole;
				FactTypeInstance rowFactInstance;
				Role supertypeIdentifyingRole;
				switch (ResolveRow(ref row, out rowType, out rowRole, out rowFactInstance, out rowInstance))
				{
					case RowType.FactRole:
						retVal = new CellEditContext(rowRole, rowFactInstance, this);
						break;
					case RowType.IdentifierRole:
						supertypeIdentifyingRole = GetIdentifyingSupertypeRole(rowType);
						retVal = new CellEditContext(
							(supertypeIdentifyingRole != null) ? supertypeIdentifyingRole.RolePlayer : rowType,
							(supertypeIdentifyingRole != null) ? rowType : null,
							rowRole,
							myEditInstance,
							myEditSubtypeInstance,
							this);
						break;
					case RowType.ObjectifyingIdentifier:
						retVal = new CellEditContext(rowType, rowFactInstance, rowInstance, this);
						break;
					case RowType.Supertype:
						supertypeIdentifyingRole = GetIdentifyingSupertypeRole(rowType);
						retVal = new CellEditContext(
							(supertypeIdentifyingRole != null) ? supertypeIdentifyingRole.RolePlayer : rowType,
							(supertypeIdentifyingRole != null) ? rowType : null,
							GetPreferredSubtypeRole(rowType),
							myEditInstance,
							myEditSubtypeInstance, this);
						break;
				}
				return retVal;
			}
			public sealed override void DeleteInstance(int row, int column)
			{
				// Empty implementation
			}
			#endregion // Base overrides
		}
		#endregion // Nested Branch Classes
		#region Nested Class SamplePopulationVirtualTree
		private sealed class SamplePopulationVirtualTree : StandardMultiColumnTree
		{
			public SamplePopulationVirtualTree(IBranch root, int columnCount)
				: base(columnCount)
			{
				Debug.Assert(root != null);
				this.Root = root;
			}
		}
		#endregion // Nested Class SamplePopulationVirtualTree
		#region LabelEdit Events
		private void TransactionCommittedEvent(object sender, TransactionCommitEventArgs e)
		{
			myTransactionCommittedDuringLabelEdit = true;
		}
		private void vtrSamplePopulation_LabelEditControlChanged(object sender, EventArgs e)
		{
			if (LabelEditControl != null)
			{
				myTransactionCommittedDuringLabelEdit = false;
			}
			else if (myTransactionCommittedDuringLabelEdit)
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
				while (parentCoord.IsValid);

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
			}
		}
		#endregion // LabelEdit Events
	}
}
 
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
// work was not done in BaseBranch.RecurseValueInstance to extend this functionality to
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
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
using System.Diagnostics.Eventing.Reader;





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
			treeControl.StandardCheckBoxes = true;
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
			/// A unary role
			/// </summary>
			UnaryRoleInstance,
			/// <summary>
			/// Overlay index
			/// </summary>
			ErrorOverlay,
			/// <summary>
			/// IdentityTag image
			/// </summary>
			IdentityTag,
		}
		private static InstanceTypeImageIndex GetImageIndex(Role role, ObjectType objectType, bool ignoreObjectification, bool resolveUnaryRolePlayer)
		{
			if (role != null)
			{
				if (!resolveUnaryRolePlayer && role.FactType.UnaryPattern != UnaryValuePattern.NotUnary)
				{
					return InstanceTypeImageIndex.UnaryRoleInstance;
				}

				if (objectType == null)
				{
					objectType = role.RolePlayer;
				}
			}
			InstanceTypeImageIndex retVal = InstanceTypeImageIndex.None;
			if (objectType != null)
			{
				UniquenessConstraint pid;
				if (objectType.IsValueType)
				{
					retVal = InstanceTypeImageIndex.ValueType;
				}
				else
				{
					ignoreObjectification = ignoreObjectification || objectType.NestedFactType == null;
					if (null != (pid = objectType.ResolvedPreferredIdentifier))
					{
						retVal = (pid.PreferredIdentifierFor == objectType) ?
							ignoreObjectification ? InstanceTypeImageIndex.EntityType : InstanceTypeImageIndex.EntityTypeObjectification :
							ignoreObjectification ? InstanceTypeImageIndex.EntityTypeSubtype : InstanceTypeImageIndex.EntityTypeSubtypeObjectification;
					}
					else if (null != objectType.GetPreferredTerminalSupertype())
					{
						retVal = ignoreObjectification ? InstanceTypeImageIndex.EntityTypeSubtype : InstanceTypeImageIndex.EntityTypeSubtypeObjectification;
					}
					else
					{
						retVal = ignoreObjectification ? InstanceTypeImageIndex.IdentityTag : InstanceTypeImageIndex.EntityTypeObjectification;
					}
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
			ObjectType unidentifiedRootSupertype = null;
			Func<ObjectType, bool, ObjectTypeIdentityInfo> identityKindCache = CreateObjectTypeIdentityCache();
			bool showIdentityTag = 0 != (identityKindCache(entityType, false).Kind & ObjectTypeIdentityKind.IdentityTag);
			int roleStartOffset = showIdentityTag ? 2 : 1;
			AdjustVisibility(true);
			int idLength;
			VirtualTreeColumnHeader[] headers;
			if (preferredIdentifier != null)
			{
				ObjectType preferredFor = preferredIdentifier.PreferredIdentifierFor;
				if (entityType == preferredFor)
				{
					LinkedElementCollection<Role> roleCollection = preferredIdentifier.RoleCollection;
					idLength = roleCollection.Count;
					headers = new VirtualTreeColumnHeader[idLength + roleStartOffset];
					for (int i = 0; i < idLength; ++i)
					{
						Role role = roleCollection[i].Role;
						ObjectType columnRolePlayer = role.RolePlayer;
						headers[i + roleStartOffset] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(role, false, false), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(role, (idLength == 1 && columnRolePlayer.IsValueType) ? entityType : columnRolePlayer, false, false));
					}
				}
				else
				{
					idLength = 1;
					headers = new VirtualTreeColumnHeader[roleStartOffset + 1];
					// The obvious name here is the preferredFor name, but either name is correct due to the is-a relationship
					// between the instances. The current names provides the only distinction for the selection type, so it is
					// better to show subtype(supertype(...)) than just supertype(...) in this scenario.
					headers[roleStartOffset] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(entityType, false), VirtualTreeColumnHeaderStyles.Default, (int)InstanceTypeImageIndex.EntityTypeSubtype);
				}
			}
			else if (null != (unidentifiedRootSupertype = entityType.GetPreferredTerminalSupertype()))
			{
				idLength = 1;
				headers = new VirtualTreeColumnHeader[roleStartOffset + 1];
				// UNDONE: NOW Work on the naming here. It is highly unlikely DeriveColumnName will handle an unidentified type. Add type([]=(,,)) for IdentityTag.
				// The identity tag always comes from the supertype, so is never shown on this code path.
				headers[roleStartOffset] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(entityType, false), VirtualTreeColumnHeaderStyles.Default, (int)InstanceTypeImageIndex.EntityTypeSubtype);
			}
			else
			{
				idLength = 0;
				headers = new VirtualTreeColumnHeader[2];
			}
			headers[0] = CreateRowNumberColumn();
			if (showIdentityTag)
			{
				headers[1] = CreateIdentityTagColumn();
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new EntityTypeBranch(mySelectedEntityType, preferredIdentifier, unidentifiedRootSupertype, idLength, identityKindCache, showIdentityTag);
			ConnectTree();
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
			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[factTypeColumnCount + factColumnOffset];
			headers[0] = CreateRowNumberColumn();
			if (objectifyingType != null)
			{
				headers[1] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(objectifyingType, true), VirtualTreeColumnHeaderStyles.Default, (int)((objectifyingType == preferredForEntityType) ? InstanceTypeImageIndex.EntityType : InstanceTypeImageIndex.EntityTypeSubtype));
			}
			for (int i = 0; i < factTypeColumnCount; ++i)
			{
				Role role = factRoles[i].Role;
				headers[i + factColumnOffset] = new VirtualTreeColumnHeader(BaseBranch.DeriveColumnName(role, false, true), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(role, null, false, true));
			}
			vtrSamplePopulation.SetColumnHeaders(headers, true);
			myBranch = new FactTypeBranch(selectedFactType, factTypeColumnCount, objectifyingType);
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

		private VirtualTreeColumnHeader CreateIdentityTagColumn()
		{
			return new VirtualTreeColumnHeader(ResourceStrings.ModelSamplePopulationEditorIdentityTag, VirtualTreeColumnHeaderStyles.Default, (int)InstanceTypeImageIndex.IdentityTag);
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
					(autoCorrectRole.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole && Array.IndexOf<ModelElement>(representedElements, autoCorrectRole.OppositeOrUnaryRole.Role.RolePlayer) != -1))
				{
					ObjectInstance instance = error.ObjectInstance;
					MandatoryConstraint constraint = error.MandatoryConstraint;
					ObjectInstance subtypeInstance = null;
					FactInstance factInstance = null;
					ObjectInstance entityInstance = null;
					FactInstanceImplication implication;
					Store store = instance.Store;
					using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, (subtype != null) ? subtype.Name : factType.Name)))
					{
						if (subtype != null)
						{
							subtypeInstance = ObjectInstance.GetSubtypeInstance(instance, subtype, false, true);
						}
						else if ((implication = new FactInstanceImplication(factType)).IsImplied && implication.ImpliedProxyRole == null && (impliedSupertype = implication.IdentifyingSupertype) != null)
						{
							FactType selectedFactType = mySelectedFactType;
							impliedEntityType = implication.ImpliedByEntityType;
							EntityTypeBranch.ConnectInstance(impliedSupertype, impliedSupertype != impliedEntityType ? impliedEntityType : null, ref entityInstance, ref subtypeInstance, instance, autoCorrectRole);
							if (selectedFactType != null && entityInstance != null)
							{
								factInstance = new FactInstance(store);
								factInstance.FactType = selectedFactType;
								new ObjectificationInstance(factInstance, (ObjectInstance)subtypeInstance ?? entityInstance);
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
					TooFewFactRoleInstancesError partialFactInstanceError;
					TooFewEntityRoleInstancesError partialEntityInstanceError;
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
						if (null != (partialEntityInstanceError = entityInstance.TooFewEntityRoleInstancesError))
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
						if (null != (partialFactInstanceError = factInstance.TooFewFactRoleInstancesError))
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
			// there is no way to select and expand them without associating an empty FactInstance.
			// Note that the identifier picker dropdown special cases empty identifier instances
			// so that they can be easily 'stolen' for other instances.
			TooFewEntityRoleInstancesError partialIdentifierError;
			ObjectInstance entityInstance;
			ObjectType entityType;
			if (null != (partialIdentifierError = error as TooFewEntityRoleInstancesError) &&
				null != (entityInstance = partialIdentifierError.EntityInstance) &&
				null != entityInstance.ObjectifiedInstanceRequiredError &&
				null != (entityType = entityInstance.EntityType))
			{
				Store store = entityInstance.Store;
				FactType factType = entityType.NestedFactType;
				using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRelateObjectifiedInstanceIdentifierTransactionText, entityType.Name, entityInstance.IdentifierName, FactInstance.GenerateEmptyInstanceName(factType))))
				{
					FactInstance factInstance = new FactInstance(store);
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
		/// <summary>
		/// Determine if the IdentityTag column should be display for an entity type.
		/// 
		/// </summary>
		/// <param name="entityType">The type to test</param>
		/// <param name="resolvedPreferredIdentifier">The preferred identifier. This may be from a supertype.</param>
		/// <param name="unidentifiedRootSupertype">If there is no preferred identifier, assume the top-most single supertype
		/// will ultimately be the identifying supertype.</param>
		/// <returns>True if there is no preferred identifier and this is not a subtype, otherwise, return true if there are explicitly tagged elements on this type.</returns>
		private static bool TestShowEntityTypeIdentityTag(ObjectType entityType, UniquenessConstraint resolvedPreferredIdentifier, ObjectType unidentifiedRootSupertype)
		{
			if (resolvedPreferredIdentifier != null)
			{
				if (resolvedPreferredIdentifier.PreferredIdentifierFor == entityType)
				{
					// This is a self-identifying type. These are only tagged if the type has explicitly
					// typed elements.
					foreach (ObjectInstance instance in entityType.ObjectInstanceCollection)
					{
						if (!string.IsNullOrEmpty(instance.IdentityTag) && !instance.AutoIdentityTag)
						{
							// Any explicitly tagged instance forces the whole type to show the color.
							return true;
						}
					}
				}
				// We do not test population for the supertype. In this case, the population (if needed) is shown
				// as part of the expandable supertype identifiers.
			}
			else if (unidentifiedRootSupertype == null)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Determine how an object type is identified. This affects how sample population
		/// for an object instance is displayed (expanded or as a single value).
		/// </summary>
		[Flags]
		private enum ObjectTypeIdentityKind
		{
			/// <summary>
			/// No flags are set
			/// </summary>
			None = 0,
			/// <summary>
			/// The instance is for a value type and is identified by the value
			/// </summary>
			Value = 1,
			/// <summary>
			/// The entity type is directly identified
			/// </summary>
			DirectIdentifier = 2,
			/// <summary>
			/// The non-subtype identifier resolves to a single value (possibly across multiple identifiers)
			/// </summary>
			SimpleIdentifier = 4,
			/// <summary>
			/// The entity type is identified through a supertype that has an identifier
			/// </summary>
			IdentifiedSupertype = 8,
			/// <summary>
			/// The entity type is identifier through an unidentified terminal supertype
			/// </summary>
			UnidentifiedSupertype = 0x10,
			/// <summary>
			/// The entity type has a visible tag column. This can appear on its own or with
			/// a direct identifier. However, with a supertype identifier the tag is associated with
			/// the supertype, not the subtype.
			/// </summary>
			IdentityTag = 0x20,
		}

		/// <summary>
		/// Specify identifying information about an entity type display.
		/// </summary>
		private struct ObjectTypeIdentityInfo
		{
			/// <summary>
			/// The display kind.
			/// </summary>
			public readonly ObjectTypeIdentityKind Kind;

			/// <summary>
			/// The related supertype, if any.
			/// </summary>
			public readonly ObjectType Supertype;

			/// <summary>
			/// Create a new <see cref="ObjectTypeIdentityInfo"/>
			/// </summary>
			public ObjectTypeIdentityInfo(ObjectTypeIdentityKind kind, ObjectType supertype)
			{
				this.Kind = kind;
				this.Supertype = supertype;
			}

			/// <summary>
			/// Determine if the entity should be expanded as a single value.
			/// This will be true for a direct, simple identifier with no identity tag,
			/// or an identity tag without other elements.
			/// </summary>
			public bool DisplayAsInlineValue
			{
				get
				{
					ObjectTypeIdentityKind kind = this.Kind;
					if (0 == (kind & (ObjectTypeIdentityKind.IdentifiedSupertype | ObjectTypeIdentityKind.UnidentifiedSupertype)))
					{
						bool hasIdentityTag = 0 != (kind & ObjectTypeIdentityKind.IdentityTag);
						if (0 != (kind & (ObjectTypeIdentityKind.SimpleIdentifier | ObjectTypeIdentityKind.Value)))
						{
							return !hasIdentityTag;
						}
						else if (hasIdentityTag && 0 == (kind & ObjectTypeIdentityKind.DirectIdentifier))
						{
							return true;
						}
					}
					return false;
				}
			}
		}

		/// <summary>
		/// Cacheable structure using identifiers to indicate how instance of an object type are represented
		/// during an editable session (without resetting the topmost branch) in the population editor.
		/// </summary>
		private class ObjectTypeIdentityInfo_WithIds
		{
			public readonly ObjectTypeIdentityKind Kind;
			public readonly Nullable<Guid> SupertypeId;

			/// <summary>
			/// Create a new <see cref="ObjectTypeIdentityInfo_WithIds"/> instance
			/// </summary>
			/// <param name="kind">The display kind</param>
			/// <param name="supertypeId">The supertype identifier</param>
			public ObjectTypeIdentityInfo_WithIds(ObjectTypeIdentityKind kind, Nullable<Guid> supertypeId)
			{
				this.Kind = kind;
				this.SupertypeId = supertypeId;
			}

			/// <summary>
			/// Convert to the structure with a supertype instance instead of the id.
			/// </summary>
			/// <param name="store">The context <see cref="Store"/></param>
			/// <returns>Populated <see cref="ObjectTypeIdentityInfo"/></returns>
			public ObjectTypeIdentityInfo AsObjectTypeIdentityInfo(Store store)
			{
				ObjectTypeIdentityKind kind = this.Kind;
				return new ObjectTypeIdentityInfo(kind, 0 != (kind & (ObjectTypeIdentityKind.IdentifiedSupertype | ObjectTypeIdentityKind.UnidentifiedSupertype)) ? store.DefaultPartition.ElementDirectory.FindElement(this.SupertypeId.GetValueOrDefault()) as ObjectType : null);
			}
		}
		private static Func<Func<ObjectType, bool, ObjectTypeIdentityInfo>> CreateObjectTypeIdentityCache = () =>
		{
			Dictionary<Guid, ObjectTypeIdentityInfo_WithIds> cache = null;
			return (ObjectType objectType, bool isUsed) =>
			{
				bool calc = false;
				ObjectTypeIdentityInfo_WithIds cachedResult;
				if (cache == null)
				{
					if (!isUsed)
					{
						cache = new Dictionary<Guid, ObjectTypeIdentityInfo_WithIds>();
						calc = true;
					}
				}
				else if (cache.TryGetValue(objectType.Id, out cachedResult))
				{
					return cachedResult.AsObjectTypeIdentityInfo(objectType.Store);
				}
				else
				{
					calc = !isUsed;
				}

				if (calc)
				{
					UniquenessConstraint pid = null;
					ObjectType supertype = null;
					ObjectTypeIdentityKind kind = ObjectTypeIdentityKind.None;
					if (objectType.DataType != null)
					{
						kind = ObjectTypeIdentityKind.Value;
					}
					else if (null != (pid = objectType.ResolvedPreferredIdentifier))
					{
						supertype = pid.PreferredIdentifierFor;
						if (supertype == objectType)
						{
							kind = ObjectTypeIdentityKind.DirectIdentifier;
							supertype = null;
							if (!HasComplexIdentifier(objectType))
							{
								kind |= ObjectTypeIdentityKind.SimpleIdentifier;
							}
						}
						else
						{
							kind = ObjectTypeIdentityKind.IdentifiedSupertype;
						}
					}
					else if (null != (supertype = objectType.GetPreferredTerminalSupertype()))
					{
						kind |= ObjectTypeIdentityKind.UnidentifiedSupertype;
					}
					if (TestShowEntityTypeIdentityTag(objectType, pid, pid == null ? supertype : null))
					{
						kind |= ObjectTypeIdentityKind.IdentityTag;
					}
					cache[objectType.Id] = new ObjectTypeIdentityInfo_WithIds(kind, supertype?.Id);
					return new ObjectTypeIdentityInfo(kind, supertype);
				}
				return default(ObjectTypeIdentityInfo);
			};
		};
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

			// Track object type classification changes
			classInfo = dataDirectory.FindDomainRelationship(ValueTypeHasDataType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(DataTypeAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(DataTypeRemovedEvent), action);

			// Track EntityInstance changes
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
		#region Object Type Transition Event Handlers
		private void DataTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			ObjectType valueType = ((ValueTypeHasDataType)e.ModelElement).ValueType;
			if (valueType == mySelectedEntityType && !valueType.IsDeleted) // Outright deletion is handled separately
			{
				mySelectedFactType = null;
				mySelectedEntityType = null;
				mySelectedValueType = valueType;
				PopulateControlForValueType();
			}
		}
		private void DataTypeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myRepopulated)
			{
				return;
			}
			ObjectType objectType = ((ValueTypeHasDataType)e.ModelElement).ValueType;
			if (objectType == mySelectedValueType && !objectType.IsDeleted)
			{
				mySelectedValueType = null;
				mySelectedEntityType = objectType;
				PopulateControlForEntityType();
			}
		}
		#endregion // Object Type Transition Event Handlers
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
				if (!entityType.IsDeleted && myBranch.HasBranchesForEntityType(entityType))
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
				bool repopulate = false;
				if (selectedFactType == entityType.NestedFactType || myBranch?.HasBranchesForEntityType(entityType) == true)
				{
					repopulate = true;
				}
				else
				{
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
								repopulate = true;
							}
						}
					}
				}

				if (repopulate)
				{
					PopulateControlForFactType();
					return;
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
			UniquenessConstraint uniqueness;
			if (null != (uniqueness = e.SourceElement as UniquenessConstraint))
			{
				TestRepopulateForIdentifierPart(uniqueness.PreferredIdentifierFor);
			}
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
		/// attaching these branches to the correct instances in response
		/// to events.
		/// </summary>
		private interface IUnattachedBranchOwner
		{
			/// <summary>
			/// Anchor an expansion branch that is not currently associated with an
			/// instance.
			/// </summary>
			/// <param name="objectInstance">The <see cref="ObjectInstance"/> to attach. If this instance is already attached
			/// in another row in the parent then no modifications are made.</param>
			/// <param name="factInstance">The <see cref="FactInstance"/> to attach. If this instance is already attached
			/// in another row in the parent then no modifications are made.</param>
			/// <returns>True if all unattached branches were successfully attached</returns>
			bool TryAnchorUnattachedBranches(ObjectInstance objectInstance, FactInstance factInstance);
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
			/// <param name="objectInstance">The <see cref="ObjectInstance"/> to attach</param>
			/// <param name="factInstance">The <see cref="FactInstance"/> to attach</param>
			void AnchorUnattachedBranch(ObjectInstance objectInstance, FactInstance factInstance);
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
			public BaseBranch(int columnCount, Store store, Func<ObjectType, bool, ObjectTypeIdentityInfo> objectTypeIdentityCache)
			{
				this.myColumnCount = columnCount;
				this.myStore = store;
				this.myObjectTypeIdentityCache = objectTypeIdentityCache;
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
						private delegate ObjectInstance AutoInstanceGenerator();
						private IList myInstanceList;
						private AutoInstanceGenerator myAutoInstanceGenerator;
						private bool myNoNullItem;
						/// <summary>
						/// Cater for alternative string representations to display instances
						/// using the IdentifierName instead of the default Name.
						/// </summary>
						protected override object TranslateFromDisplayObject(int newIndex, object newObject)
						{
							if (newIndex >= 0)
							{
								if (newIndex == 0)
								{
									if (myAutoInstanceGenerator != null)
									{
										return myAutoInstanceGenerator();
									}
									else if (newObject is DataType)
									{
										return newObject;
									}
								}

								IList instanceList = myInstanceList;
								if (instanceList != null)
								{
									if (myAutoInstanceGenerator != null)
									{
										--newIndex;
									}
									return instanceList[newIndex];
								}

							}
							return base.TranslateFromDisplayObject(newIndex, newObject);
						}
						/// <summary>
						/// Cater for alternative string representations to display instances
						/// using the IdentifierName instead of the default Name.
						/// </summary>
						protected override object TranslateToDisplayObject(object initialObject, IList contentList)
						{
							// The initial object will never be a data type here, just an instance.
							// Offsets for auto generation are not needed here.
							IList instanceList;
							if (initialObject != null &&
								null != (instanceList = myInstanceList))
							{
								int index = instanceList.IndexOf(initialObject);
								return (index != -1) ? contentList[index] : initialObject;
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
								return myNoNullItem ? null : ResourceStrings.ModelSamplePopulationEditorNullSelection;
							}
						}
						/// <summary>
						/// Return the string values for the contents of the dropdown list
						/// </summary>
						protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
						{
							myInstanceList = null;
							myAutoInstanceGenerator = null;
							myNoNullItem = false;
							CellEditContext contextInstance = (CellEditContext)context.Instance;
							ObjectType rolePlayer = contextInstance.ContextTargetObjectType;
							DataType autoGenDataType = null;
							if (rolePlayer != null)
							{
								ValueTypeHasDataType dataTypeUse = ValueTypeHasDataType.GetLinkToDataType(rolePlayer);
								bool filterEmptyInstances = rolePlayer.NestedFactType == null && dataTypeUse == null;
								Role contextRole = contextInstance.myIsEntityTypeEditor ? contextInstance.myRole : null;
								UniquenessConstraint pid = null;

								// Recursively resolve data type use to see if we eventually read a single generatable data type.
								if (dataTypeUse == null)
								{
									pid = rolePlayer.ResolvedPreferredIdentifier;
									if (pid != null && value == null) // Only look further for an auto-generate data type for a new value, not an existing one (value == null).
									{
										UniquenessConstraint recursePid = pid;
										while (dataTypeUse == null && recursePid != null)
										{
											if (contextRole == null)
											{
												LinkedElementCollection<Role> roles = recursePid.RoleCollection;
												if (roles.Count == 1)
												{
													contextRole = roles[0];
												}
											}

											if (contextRole != null)
											{
												ObjectType identifyingObjectType = contextRole.RolePlayer;
												dataTypeUse = ValueTypeHasDataType.GetLinkToDataType(identifyingObjectType);
												if (dataTypeUse == null)
												{
													recursePid = identifyingObjectType.ResolvedPreferredIdentifier;
													// The role is provided the first time through, but not after that.
													contextRole = null;
												}
											}
											else
											{
												recursePid = null;
											}
										}
									}
								}

								if (dataTypeUse != null && dataTypeUse.AutoGenerated && value == null)
								{
									// Offer to create a new instance for auto generated identifiers
									autoGenDataType = dataTypeUse.DataType;
									myAutoInstanceGenerator = delegate ()
									{
										string newValue = dataTypeUse.DataType.AutoGenerate(GetInstanceValues(dataTypeUse));
										if (newValue != null)
										{
											contextInstance.myEmulateCommitLabelEdit(newValue);
										}
										return null;
									};
								}

								IList<ObjectInstance> instances = rolePlayer.ObjectInstanceCollection;
								int instanceCount = instances.Count;
								ObjectType subtype;
								bool displayIdentifierNames = false;
								ObjectInstance currentInstance = (ObjectInstance)contextInstance.myColumnInstance;
								ObjectType preferredFor;
								ObjectType contextEntityType = contextInstance.myEntityType;
								bool isEntityEditor = contextInstance.myIsEntityTypeEditor;
								if (contextEntityType != null && !isEntityEditor)
								{
									// FactType identifier

									// Show the current instance and any other instance that is not currently attached,
									// plus a null to detach the entity instance. We also show any instance that is
									// currently associated with an empty FactInstance. These empty instances
									// are generated in response to a user action (selecting from the pick list or
									// activating a model error) and are expendable. However, we don't show them
									// if the instance we're currently choosing for is empty as this results in
									// the pointless exercise of moving an identifier from one empty FactInstance
									// to another.
									ObjectInstance[] unreferencedSupertypeInstances = null;
									int unreferenceSupertypeInstanceCount = 0;
									subtype = contextInstance.myEntityTypeSubtype;
									if (subtype != null)
									{
										unreferencedSupertypeInstances = GetUnreferencedSupertypeInstances(subtype.ObjectInstanceCollection, contextEntityType.ObjectInstanceCollection, null);
										if (unreferencedSupertypeInstances != null)
										{
											unreferenceSupertypeInstanceCount = unreferencedSupertypeInstances.Length;
										}
									}
									int unattachedCount = 0;
									bool showIdentifiersFromEmptyFactInstances = currentInstance != null || contextInstance.myFactInstance != null;
									for (int i = 0; i < instanceCount; ++i)
									{
										ObjectInstance instance = instances[i];
										FactInstance objectifiedInstance;
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
										ObjectInstance[] filteredInstances = new ObjectInstance[totalCount];
										unattachedCount = -1;
										for (int i = 0; i < instanceCount; ++i)
										{
											ObjectInstance instance = instances[i];
											FactInstance objectifiedInstance;
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
										else if (autoGenDataType != null)
										{
											return new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance };
										}
										else
										{
											return null;
										}
									}
									else if (unreferenceSupertypeInstanceCount != 0)
									{
										ObjectInstance[] newInstances = new ObjectInstance[unreferenceSupertypeInstanceCount + 1];
										newInstances[0] = currentInstance;
										unreferencedSupertypeInstances.CopyTo(newInstances, 1);
										instances = newInstances;
										instanceCount = newInstances.Length;
									}
									else if (displayIdentifierNames)
									{
										myInstanceList = new ObjectInstance[] { currentInstance };
										if (autoGenDataType != null)
										{
											return new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance, currentInstance.IdentifierName };
										}
										else
										{
											return new string[] { currentInstance.IdentifierName };
										}
									}
									else if (autoGenDataType != null)
									{
										return new object[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance, currentInstance };
									}
									else
									{
										return new ObjectInstance[] { currentInstance };
									}
								}
								else if (null != (subtype = contextInstance.myEntityTypeSubtype))
								{
									// Subtype editor, not a FactType identifier

									// Put all unused supertype instances in the list, plus the supertype
									// of the current subtype instance.
									ObjectInstance[] filteredInstances = GetUnreferencedSupertypeInstances(subtype.ObjectInstanceCollection, instances, contextInstance.mySubtypeInstance);
									if (filteredInstances != null)
									{
										instances = filteredInstances;
										instanceCount = instances.Count;
									}
									else if (autoGenDataType != null)
									{
										return new object[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance };
									}
									else
									{
										return null;
									}
								}
								else if (pid != null &&
									rolePlayer != (preferredFor = pid.PreferredIdentifierFor))
								{
									// Normal role player with a subtype, show the current instances and the unattached
									// supertype instances.
									ObjectInstance[] unreferencedSupertypeInstances = GetUnreferencedSupertypeInstances(instances, preferredFor.ObjectInstanceCollection, null);
									if (unreferencedSupertypeInstances != null)
									{
										if (instanceCount == 0)
										{
											instances = unreferencedSupertypeInstances;
											instanceCount = unreferencedSupertypeInstances.Length;
										}
										else
										{
											ObjectInstance[] newInstances = new ObjectInstance[instanceCount + unreferencedSupertypeInstances.Length];
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
												if (autoGenDataType != null)
												{
													return currentInstance == null ? new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance } : new object[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance, currentInstance };
												}
												else
												{
													return currentInstance == null ? null : new ObjectInstance[] { currentInstance };
												}
											case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
												List<ObjectInstance> filteredInstances = null;
												foreach (ObjectInstance instance in instances)
												{
													bool keepInstance = instance == currentInstance;
													if (!keepInstance)
													{
														keepInstance = true;
														foreach (RoleInstance roleInstance in RoleInstance.GetLinksToRoleCollection(instance))
														{
															if (roleInstance.IdentifiedEntityInstance?.EntityType == contextEntityType)
															{
																keepInstance = false;
																break;
															}
														}
													}

													if (keepInstance)
													{
														(filteredInstances ?? (filteredInstances = new List<ObjectInstance>(instanceCount))).Add(instance);
													}
												}
												if (filteredInstances == null)
												{
													return autoGenDataType != null ? new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance } : null;
												}
												instances = filteredInstances;
												instanceCount = filteredInstances.Count;
												break;
										}
									}
								}
								if (instanceCount == 0)
								{
									return autoGenDataType != null ? new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance } : null;
								}
								if (filterEmptyInstances)
								{
									List<ObjectInstance> filteredList = null;
									int lastFiltered = -1;
									for (int i = 0; i < instanceCount; ++i)
									{
										ObjectInstance testInstance = instances[i];
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
													filteredList = new List<ObjectInstance>(instanceCount - lastFiltered - 1);
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
										return autoGenDataType != null ? new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance } : null;
									}
								}
								if (displayIdentifierNames)
								{
									int index = autoGenDataType != null ? 1 : 0;
									string[] instanceIdentifierNames = new string[instanceCount + index];
									if (index == 1)
									{
										instanceIdentifierNames[0] = ResourceStrings.ModelSamplePopulationEditorGenerateInstance;
									}
									for (int i = 0; i < instanceCount; ++i, ++index)
									{
										instanceIdentifierNames[index] = instances[index].IdentifierName;
									}
									myInstanceList = (IList)instances;
									return instanceIdentifierNames;
								}

								if (autoGenDataType != null)
								{
									object[] results = new object[instanceCount + 1];
									results[0] = ResourceStrings.ModelSamplePopulationEditorGenerateInstance;
									((IList)instances).CopyTo(results, 1);
									return results;
								}
								return (IList)instances;
							}
							else if (null != (rolePlayer = contextInstance.myAutoGenerateValueType))
							{
								myNoNullItem = true;
								ValueTypeHasDataType dataTypeUse = ValueTypeHasDataType.GetLinkToDataType(rolePlayer);
								autoGenDataType = dataTypeUse.DataType;
								myAutoInstanceGenerator = delegate ()
								{
									string newValue = dataTypeUse.DataType.AutoGenerate(GetInstanceValues(dataTypeUse));
									if (newValue != null)
									{
										contextInstance.myEmulateCommitLabelEdit(newValue);
									}
									return null;
								};
							}
							return autoGenDataType != null ? new string[] { ResourceStrings.ModelSamplePopulationEditorGenerateInstance } : null;
						}
					}
					private static IEnumerable<string> GetInstanceValues(ValueTypeHasDataType dataTypeUse)
					{
						DataType dataType = dataTypeUse.DataType;
						bool useInvariant = !dataType.CanParseAnyValue && dataType.IsCultureSensitive;
						foreach (ObjectInstance instance in dataTypeUse.ValueType.ObjectInstanceCollection)
						{
							ObjectInstance valueInstance = instance;
							if (valueInstance != null)
							{
								yield return useInvariant ? valueInstance.InvariantValue : valueInstance.Value;
							}
						}
					}
					/// <summary>
					/// Get all supertype instances not currently used by the given subtype.
					/// </summary>
					/// <param name="subtypeInstances">The full set of subtype instances</param>
					/// <param name="supertypeInstances">The full set of instances</param>
					/// <param name="filterInstance">A subtype instance to ignore</param>
					/// <returns><see cref="ObjectInstance"/> array containing <see cref="ObjectInstance"/> elements representing entity instances</returns>
					private static ObjectInstance[] GetUnreferencedSupertypeInstances(IList<ObjectInstance> subtypeInstances, IList<ObjectInstance> supertypeInstances, ObjectInstance filterInstance)
					{
						int instanceCount = supertypeInstances.Count;
						int subtypeInstanceCount = subtypeInstances.Count;
						int currentInstanceCount = (filterInstance == null) ? 0 : 1;
						ObjectInstance[] filteredInstances = null;
						if (subtypeInstanceCount >= currentInstanceCount)
						{
							instanceCount -= subtypeInstanceCount - currentInstanceCount;
							Debug.Assert(instanceCount >= 0);
							filteredInstances = new ObjectInstance[instanceCount];
							if (instanceCount != 0)
							{
								ObjectInstance[] referencedSupertypeInstances = new ObjectInstance[subtypeInstanceCount - currentInstanceCount];
								int i = 0;
								foreach (ObjectInstance subtypeInstance in subtypeInstances)
								{
									if (subtypeInstance != filterInstance)
									{
										referencedSupertypeInstances[i] = subtypeInstance.SupertypeInstance;
										++i;
									}
								}
								i = 0;
								foreach (ObjectInstance instance in supertypeInstances)
								{
									if (-1 == Array.IndexOf<ObjectInstance>(referencedSupertypeInstances, instance))
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
									ObjectInstance currentInstance = editContext.myColumnInstance;
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
							ObjectInstance typedValue = (ObjectInstance)value;
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
						ObjectInstance typedValue = value as ObjectInstance;
						CellEditContext context = (CellEditContext)component;
						ObjectInstance columnInstance = context.myColumnInstance;
						Role role = context.myRole;
						if (typedValue == null)
						{
							if (columnInstance != null)
							{
								if (context.myIsEntityTypeEditor)
								{
									ObjectInstance subtypeInstance = context.mySubtypeInstance;
									if (subtypeInstance != null)
									{
										ObjectInstance entityInstance = context.myEntityInstance;
										using (Transaction t = role.Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, context.myEntityTypeSubtype.Name, subtypeInstance.Name)))
										{
											subtypeInstance.Delete();
											t.Commit();
										}
										context.UpdateInstanceFields((ObjectInstance)null, null);
									}
									else
									{
										ObjectInstance entityInstance = context.myEntityInstance;
										RoleInstance roleInstance = entityInstance.FindIdentifyingRoleInstance(role);
										ObjectInstance deleteInstance = roleInstance.ObjectInstance;
										ObjectInstance deleteValueInstance = null;
										if (role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
										{
											// See comments in EntityTypeBranch.CommitLabelEdit
											deleteValueInstance = deleteInstance;
											if (deleteValueInstance == null || deleteValueInstance.ValueType == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
											{
												throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
											}
										}

										RoleInstance matchingSingleRoleInstance = null;
										entityInstance.IterateIdentifyingRoleInstances((testRole, testRoleleInstance) => {
											if (testRoleleInstance != null)
											{
												if (matchingSingleRoleInstance == null && testRoleleInstance.ObjectInstance == deleteInstance)
												{
													matchingSingleRoleInstance = roleInstance;
													return false;
												}

												matchingSingleRoleInstance = null;
												return true;
											}
											return false;
										});
										using (Transaction t = role.Store.TransactionManager.BeginTransaction(
											matchingSingleRoleInstance != null ?
												string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, context.ContextObjectType.Name, entityInstance.Name) :
												string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(role, false), deleteInstance.Name, entityInstance.Name)))
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
									FactInstance factInstance = context.myFactInstance;
									if (role != null)
									{
										IList<RoleInstance> factRoleInstances = factInstance.RoleInstanceCollection;
										RoleInstance roleInstance = FactInstance.FindRoleInstance(factRoleInstances, role);
										using (Transaction t = role.Store.TransactionManager.BeginTransaction(
											(factRoleInstances.Count == 1) ?
											string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, factInstance.Name) :
											string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(role, true), roleInstance.ObjectInstance.Name, factInstance.Name)))
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
								if (targetContextObjectType != typedValue.ObjectType)
								{
									// This occurs if the list has been extended to show unreferenced supertype instances
									// UNDONE: NOW Stopped here. This throws if forceCreate (the true arg) is respected. It needs to be inside the transaction.
									// In a preferred subtype situation, the typedValue.Name is the same (used below for transaction text), but I'm not sure
									// if this happens for secondary subtypes, which will populate a 1-1 relationship to get the correct name.
									// Is it possible to retroactively change the name of a transaction before commit? NO. Consider just doing two transactions.
									// Possibly just use typedValue.IdentifierName??
									typedValue = ObjectInstance.GetSubtypeInstance(typedValue, targetContextObjectType, false, true);
								}

								string instanceName = null;
								{
									FactInstance factInstance;
									ObjectInstance entityInstance;
									if (null != (factInstance = context.myFactInstance))
									{
										instanceName = factInstance.Name;
									}
									else if (null != (entityInstance = context.myEntityInstance))
									{
										instanceName = entityInstance.Name;
									}
								}

								using (Transaction t = role.Store.TransactionManager.BeginTransaction(
									instanceName == null ?
										string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, context.ContextObjectType.Name) :
										string.Format(ResourceStrings.ModelSamplePopulationEditorAddObjectInstanceReferenceTransactionText, typedValue.ObjectType.Name, typedValue.Name, instanceName)))
								{
									EntityEditorBranch entityEditorBranch = context.myEditBranch;
									if (entityEditorBranch != null)
									{
										entityEditorBranch.ConnectInstance(typedValue, role, null, context.myFactInstance);
										if (context.myIsEntityTypeEditor)
										{
											context.UpdateInstanceFields((ObjectInstance)context.mySubtypeInstance ?? context.myEntityInstance);
										}
										else
										{
											context.UpdateInstanceFields(typedValue.ObjectifiedInstance, null);
										}
									}
									else if (context.myIsEntityTypeEditor)
									{
										ObjectInstance entityInstance = context.myEntityInstance;
										ObjectInstance subtypeInstance = context.mySubtypeInstance;
										EntityTypeBranch.ConnectInstance(context.myEntityType, context.myEntityTypeSubtype, ref entityInstance, ref subtypeInstance, typedValue, role);
										context.UpdateInstanceFields(entityInstance, subtypeInstance);
									}
									else
									{
										FactInstance factInstance = context.myFactInstance;
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
								FactInstance factInstance = context.myFactInstance;
								FactType factType;
								using (Transaction t = targetContextObjectType.Store.TransactionManager.BeginTransaction(string.Format(
									ResourceStrings.ModelSamplePopulationEditorRelateObjectifiedInstanceIdentifierTransactionText,
									context.ContextObjectType.Name,
									typedValue.IdentifierName,
									(factInstance != null) ?
										factInstance.Name :
										(null != (factType = targetContextObjectType.NestedFactType) ? FactInstance.GenerateEmptyInstanceName(factType) : ""))))
								{
									if (targetContextObjectType != typedValue.ObjectType)
									{
										// This occurs if the list has been extended to show unreferenced supertype instances
										typedValue = ObjectInstance.GetSubtypeInstance(typedValue, targetContextObjectType, false, true);
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
							return typeof(ObjectInstance);
						}
					}
					public sealed override Type PropertyType
					{
						get
						{
							return typeof(ObjectInstance);
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
				private readonly Action<string> myEmulateCommitLabelEdit;
				private readonly ObjectType myAutoGenerateValueType;
				private ObjectInstance myEntityInstance;
				private ObjectInstance mySubtypeInstance;
				private FactInstance myFactInstance;
				private ObjectInstance myColumnInstance;
				#endregion // Member Variables
				#region Constructor
				/// <summary>
				/// Create an editing context for the given entityType role and entityInstance
				/// </summary>
				/// <param name="entityType">The entityType to attach to instances to.</param>
				/// <param name="entityTypeSubtype">A subtype of the entity to attach to.</param>
				/// <param name="role">The role being edited</param>
				/// <param name="entityInstance">The current entityInstance. Can be null.</param>
				/// <param name="subtypeInstance">The current EntitySubtypeInstance. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				/// <param name="emulateCommitLabelEdit">Callback to emulate the action of label edit on the calling branch.</param>
				public CellEditContext(ObjectType entityType, ObjectType entityTypeSubtype, Role role, ObjectInstance entityInstance, ObjectInstance subtypeInstance, EntityEditorBranch editBranch, Action<string> emulateCommitLabelEdit)
				{
					Debug.Assert(entityType != null);
					Debug.Assert(role != null);
					myEntityType = entityType;
					myEntityTypeSubtype = entityTypeSubtype;
					myRole = role;
					myIsEntityTypeEditor = true;
					myEditBranch = editBranch;
					myEmulateCommitLabelEdit = emulateCommitLabelEdit;
					UpdateInstanceFields(entityInstance, subtypeInstance);
				}
				/// <summary>
				/// Update instance fields for an entity type editor edit context
				/// </summary>
				private void UpdateInstanceFields(ObjectInstance objectInstance)
				{
					if (objectInstance == null)
					{
						return; // Ignore when creating a new instance
					}
					ObjectInstance supertypeInstance = objectInstance.SupertypeInstance;
					ObjectInstance entityInstance;
					ObjectInstance subtypeInstance;
					if (supertypeInstance != null)
					{
						subtypeInstance = objectInstance;
						entityInstance = supertypeInstance;
					}
					else
					{
						subtypeInstance = null;
						entityInstance = objectInstance;
					}
						ObjectType entityTypeSubtype;
					if (entityInstance == objectInstance &&
						null != (entityTypeSubtype = myEntityTypeSubtype))
					{
						// Doing this simple sanity check means that we can call this
						// update function for all subtype cases.
						subtypeInstance = ObjectInstance.GetSubtypeInstance(entityInstance, entityTypeSubtype, true, false);
					}
					UpdateInstanceFields(entityInstance, subtypeInstance);
				}
				/// <summary>
				/// Update instance fields for an entity type editor edit context
				/// </summary>
				private void UpdateInstanceFields(ObjectInstance entityInstance, ObjectInstance subtypeInstance)
				{
					Debug.Assert(myIsEntityTypeEditor);
					myEntityInstance = entityInstance;
					mySubtypeInstance = subtypeInstance;
					ObjectInstance newInstance = null;
					if (myEntityTypeSubtype != null)
					{
						newInstance = entityInstance;
					}
					else if (entityInstance != null)
					{
						RoleInstance roleInstance = entityInstance.FindIdentifyingRoleInstance(myRole);
						if (roleInstance != null)
						{
							newInstance = roleInstance.ObjectInstance;
						}
					}
					myColumnInstance = newInstance;
				}
				/// <summary>
				/// Create an editing context for the given role and factInstance
				/// </summary>
				/// <param name="role">The role being edited</param>
				/// <param name="factInstance">The current <see cref="FactInstance"/>. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				/// <param name="emulateCommitLabelEdit">Callback to emulate the action of label edit on the calling branch.</param>
				public CellEditContext(Role role, FactInstance factInstance, EntityEditorBranch editBranch, Action<string> emulateCommitLabelEdit)
				{
					Debug.Assert(role != null);
					myRole = role;
					myIsEntityTypeEditor = false;
					myEditBranch = editBranch;
					myEmulateCommitLabelEdit = emulateCommitLabelEdit;
					UpdateInstanceFields(factInstance, null);
				}
				/// <summary>
				/// Create an editing context for the given objectifying type and factInstance
				/// </summary>
				/// <param name="objectifyingType">The objectifying type to modify.</param>
				/// <param name="factInstance">The current <see cref="FactInstance"/>. Can be null.</param>
				/// <param name="objectifyingInstance">The current objectifying instance. Can be null.</param>
				/// <param name="editBranch">Branch the editor exists on</param>
				/// <param name="emulateCommitLabelEdit">Callback to emulate the action of label edit on the calling branch.</param>
				public CellEditContext(ObjectType objectifyingType, FactInstance factInstance, ObjectInstance objectifyingInstance, EntityEditorBranch editBranch, Action<string> emulateCommitLabelEdit)
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
					myEmulateCommitLabelEdit = emulateCommitLabelEdit;
					UpdateInstanceFields(factInstance, objectifyingInstance);
				}
				/// <summary>
				/// Update instance fields for a FactType context
				/// </summary>
				private void UpdateInstanceFields(FactInstance factInstance, ObjectInstance objectifyingInstance)
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
						ObjectInstance newInstance = null;
						if (factInstance != null)
						{
							RoleInstance roleInstance = factInstance.FindRoleInstance(role);
							if (roleInstance != null)
							{
								newInstance = roleInstance.ObjectInstance;
							}
						}
						myColumnInstance = newInstance;
					}
				}
				/// <summary>
				/// Create a minimal edit context to get a dropdown and an &lt;Auto Generate&gt; entry for a new value type instance
				/// </summary>
				/// <param name="autoGenerateValueType">The objectifying type to modify.</param>
				/// <param name="emulateCommitLabelEdit">Callback to emulate the action of label edit on the calling branch.</param>
				public CellEditContext(ObjectType autoGenerateValueType, Action<string> emulateCommitLabelEdit)
				{
					myAutoGenerateValueType = autoGenerateValueType;
					myEmulateCommitLabelEdit = emulateCommitLabelEdit;
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

				/// <summary>
				/// Get alternate editing text if the value displays differently
				/// </summary>
				public string ContextAlternateText
				{
					get
					{
						ObjectInstance instance = myColumnInstance;
						if (instance != null)
						{
							ObjectType type = instance.ObjectType;
							if (type.DataType == null && type.PreferredIdentifier == null) // Note that we do not use inline edits for supertype references, so we do not need to check the recursive ResolvedPreferredIdentifier
							{
								string tag = instance.IdentityTag;
								if (!string.IsNullOrEmpty(tag))
								{
									return tag;
								}
							}
						}
						return null;
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
						null == myAutoGenerateValueType &&
						((null == (rolePlayer = (role is SubtypeMetaRole) ? ContextTargetObjectType : ContextObjectType)) ||
						(myRole != null && rolePlayer.NestedFactType != null) ||
						HasComplexIdentifier(rolePlayer));
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
								ObjectInstance columnInstance = instance.myColumnInstance;
								Role role = instance.myRole;
								if (columnInstance == null)
								{
									ObjectType objectifyingType = (role != null) ? ((role is SubtypeMetaRole) ? instance.myEntityType : role.RolePlayer) : (instance.myEntityTypeSubtype ?? instance.myEntityType);
									if (objectifyingType != null)
									{
										return ObjectInstance.GetDisplayString(null, (role is SubtypeMetaRole) ? objectifyingType : (instance.myEntityTypeSubtype ?? objectifyingType), role == null);
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
					bool defaultEdit;
					CellEditContext editContext = CreateEditContext(row, column, out defaultEdit);
					if (editContext != null)
					{
						retVal.CustomInPlaceEdit = editContext.CreateInPlaceEditControl();
						retVal.CustomCommit = delegate(VirtualTreeItemInfo itemInfo, Control editControl)
						{
							// Defer to the normal text edit if the control is not dirty
							return (editControl as IVirtualTreeInPlaceControl).Dirty ? itemInfo.Branch.CommitLabelEdit(itemInfo.Row, itemInfo.Column, editControl.Text) : LabelEditResult.CancelEdit;
						};
						retVal.AlternateText = editContext.ContextAlternateText;
					}
					else if (!defaultEdit)
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
					const BranchFeatures features = BranchFeatures.ComplexColumns | BranchFeatures.Realigns | BranchFeatures.DefaultPositionTracking | BranchFeatures.StateChanges;
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
			public static string DeriveColumnName(Role role, bool ignoreObjectification, bool resolveUnaryRolePlayer)
			{
				StringBuilder outputText = null;
				string retVal = (role == null || role.RolePlayer == null) ? ResourceStrings.ModelSamplePopulationEditorNullSelection : RecurseColumnIdentifier(role, null, ignoreObjectification, resolveUnaryRolePlayer, null, null, ref outputText);
				return (outputText != null) ? outputText.ToString() : retVal;
			}
			public static string DeriveColumnName(ObjectType objectType, bool ignoreObjectification)
			{
				StringBuilder outputText = null;
				string retVal = (objectType == null) ? ResourceStrings.ModelSamplePopulationEditorNullSelection : RecurseColumnIdentifier(null, objectType, ignoreObjectification, false, null, null, ref outputText);
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
			private static string RecurseColumnIdentifier(Role role, ObjectType rolePlayer, bool ignoreObjectification, bool resolveUnaryRolePlayer, string listSeparator, Predicate<ObjectType> stackCheck, ref StringBuilder outputText)
			{
				FactType unaryFactType;
				if (!resolveUnaryRolePlayer &&
					role != null &&
					null != (unaryFactType = role.FactType) &&
					unaryFactType.UnaryPattern != UnaryValuePattern.NotUnary)
				{
					string unaryResult = role.Name;
					if (string.IsNullOrEmpty(unaryResult))
					{
						string reading = unaryFactType.GetDefaultReading().Text.Trim();
						unaryResult = reading.StartsWith("{0}") ? string.Format(reading, string.Empty).Trim() : string.Format(reading, ResourceStrings.ReadingShapeEllipsis);
					}
					if (outputText != null)
					{
						outputText.Append(unaryResult);
						return null;
					}
					return unaryResult;
				}

				if (rolePlayer == null)
				{
					rolePlayer = role.RolePlayer;
				}
				if (rolePlayer == null)
				{
					if (outputText != null)
					{
						outputText.Append(" ");
						return null;
					}
					return " ";
				}

				if (stackCheck != null && stackCheck(rolePlayer))
				{
					if (outputText != null)
					{
						outputText.Append(rolePlayer.Name);
						outputText.Append(ResourceStrings.ReadingShapeEllipsis);
						return null;
					}
					return rolePlayer.Name + ResourceStrings.ReadingShapeEllipsis;
				}
				string roleName = (role != null) ? role.Name : "";
				string derivedName = (roleName.Length != 0) ? roleName : rolePlayer.Name;
				UniquenessConstraint identifier = null;
				ObjectType supertypeRolePlayer = null;
				bool isValueType = rolePlayer.IsValueType;
				Predicate<ObjectType> nextStackCheck = isValueType ? stackCheck : null; // The value type won't recurse over its preferred identifier.
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
					RecurseColumnIdentifier(null, supertypeRolePlayer, false, false, listSeparator, nextStackCheck ?? (nextStackCheck = CreateColumnIdentifierStackCheck(rolePlayer, stackCheck)), ref outputText);
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
								RecurseColumnIdentifier(identifierRole, null, false, false, listSeparator, nextStackCheck ?? (nextStackCheck = CreateColumnIdentifierStackCheck(rolePlayer, stackCheck)), ref outputText);
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
						RecurseColumnIdentifier(factRole, null, false, true, listSeparator, nextStackCheck ?? (nextStackCheck = CreateColumnIdentifierStackCheck(rolePlayer, stackCheck)), ref outputText);
					}
				}
				outputText.Append(")");
				return null;
			}
			/// <summary>
			/// Helper to create a delegate to determine if a role player is recursively defined.
			/// </summary>
			/// <param name="rolePlayer">The current role player.</param>
			/// <param name="contextStackCheck">A context delegate from higher up in the object tree.</param>
			/// <returns>New stack check.</returns>
			private static Predicate<ObjectType> CreateColumnIdentifierStackCheck(ObjectType rolePlayer, Predicate<ObjectType> contextStackCheck)
			{
				return delegate(ObjectType recursiveRolePlayer)
				{
					return rolePlayer == recursiveRolePlayer || (contextStackCheck != null && contextStackCheck(recursiveRolePlayer));
				};
			}
			/// <summary>
			/// Determine if the specified instance is empty. A value instance
			/// is never empty, an entity instance is empty if it has not
			/// attached instances, and an entity subtype instance is empty if
			/// its <see cref="P:ObjectInstance.SupertypeInstance"/> is empty
			/// </summary>
			public static bool IsEmptyInstance(ObjectInstance objectInstance)
			{
				// UNDONE: NOW Kill EntityTypeRoleInstance Verify the identityTag check for all uses
				ObjectType entityType = objectInstance.EntityType;
				if (entityType != null && string.IsNullOrEmpty(objectInstance.IdentityTag))
				{
					if (entityType.PreferredIdentifier == null)
					{
						objectInstance = objectInstance.SupertypeInstance;
					}
					return objectInstance != null ? !objectInstance.IterateIdentifyingRoleInstances((role, roleInstance) => roleInstance != null) : true;
				}
				return false;
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
			/// <param name="factInstance">The <see cref="FactInstance"/> associated with an objectified <see cref="FactType"/></param>
			/// <param name="identifierInstance">The <see cref="ObjectInstance"/> associated with the objectifying type that identifies this <paramref name="factInstance"/></param>
			/// <remarks>An identifier can be moved from one FactInstance to another if the original instance has no population. If
			/// the specified <paramref name="factInstance"/> has an empty identifier, then all references to the empty identifier are
			/// transferred to the new identifier. This facilititates the creation of empty placeholder identifiers that to provide
			/// a means of referencing an objectified instance.</remarks>
			/// <exception cref="InvalidOperationException">Is thrown if the identifier is already attached to another non-empty FactInstance</exception>
			protected static void ConnectObjectifyingIdentifierInstance(FactInstance factInstance, ObjectInstance identifierInstance)
			{
				ObjectificationInstance existingInstanceLink;
				// Make sure that a differed objectified instance already attached to the
				// parent instance can be safely abandoned
				FactInstance existingObjectifiedInstance;
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
					// If the current instance is empty, meaning that it has no attached identifying role instances, then
					// its instance exists solely for the purpose of relating objectified FactType information to other instances.
					// In this case, we want to abandon the empty instance to be deleted by the rules engine, but only after we
					// move all referencing links to the new instance.
					ObjectInstance objectifyingInstance = existingInstanceLink.ObjectifyingInstance;
					ObjectInstance supertypeInstance = objectifyingInstance.SupertypeInstance;
					ObjectInstance objectifyingSubtypeInstance;
					ObjectInstance objectifyingEntityInstance;
					if (supertypeInstance != null)
					{
						objectifyingSubtypeInstance = objectifyingInstance;
						objectifyingEntityInstance = supertypeInstance;
					}
					else
					{
						objectifyingSubtypeInstance = null;
						objectifyingEntityInstance = objectifyingInstance;
					}
					if (!objectifyingEntityInstance.IterateIdentifyingRoleInstances((role, instance) => instance != null))
					{
						try
						{
							ObjectInstance identifierSupertypeInstance = identifierInstance.SupertypeInstance;
							ObjectInstance identifierEntityInstance = (null == identifierSupertypeInstance) ? identifierInstance : null; // We don't care about the supertype instance
							Debug.Assert((supertypeInstance == null) == (identifierSupertypeInstance == null)); // The old and new instance types should always match
							if (identifierEntityInstance != null)
							{
								foreach (EntitySubtypeInstanceHasSupertypeInstance supertypeLink in EntitySubtypeInstanceHasSupertypeInstance.GetLinksToEntitySubtypeInstanceCollection(objectifyingEntityInstance))
								{
									supertypeLink.SupertypeInstance = identifierEntityInstance;
								}
							}

							foreach (RoleInstance roleInstance in RoleInstance.GetLinksToRoleCollection(objectifyingInstance))
							{
#if ROLEINSTANCE_ROLEPLAYERCHANGE
								link.ObjectInstance = resultEntityInstance;
#else
								if (roleInstance.Role.FactType.UnaryPattern != UnaryValuePattern.NotUnary)
								{
									FactInstance reattachFactInstance = roleInstance.FactInstance;
									roleInstance.Delete();
									new RoleInstance(roleInstance.Role, identifierInstance).FactInstance = reattachFactInstance;
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
			protected static ObjectInstance RecurseValueInstance(ObjectInstance startObjectInstance, ObjectType parentType, string newText, ref ObjectInstance rootInstance, bool create, bool allowValueEdit)
			{
				UniquenessConstraint pid;
				if (parentType.IsValueType)
				{
					DataType valueDataType = parentType.DataType;
					if (create && valueDataType.CanCompare)
					{
						bool canParseAnyValue = valueDataType.CanParseAnyValue;
						LinkedElementCollection<ObjectInstance> instances = parentType.ObjectInstanceCollection;
						int instanceCount = instances.Count;
						if (instanceCount != 0)
						{
							string invariantNewText = newText;
							bool validNewValue = true;
							if (!canParseAnyValue)
							{
								validNewValue = valueDataType.TryConvertToInvariant(newText, out invariantNewText);
							}
							for (int i = 0; i < instanceCount; ++i)
							{
								ObjectInstance currentValueInstance = instances[i];
								string value = currentValueInstance.Value;
								if (validNewValue)
								{
									if ((canParseAnyValue || valueDataType.ParseNormalizeValue(value, currentValueInstance.InvariantValue, out value)) &&
										0 == valueDataType.Compare(value, invariantNewText))
									{
										return rootInstance = currentValueInstance;
									}
								}
								else if (value == newText) // Just go for a string match with unparseable new text
								{
									return rootInstance = currentValueInstance;
								}
							}
						}
					}
					if (!create)
					{
						return startObjectInstance;
					}
					Debug.Assert(parentType.Store.TransactionActive, "Transaction must be active to create new instances");
					ObjectInstance editValueInstance = startObjectInstance;
					if (editValueInstance != null)
					{
						if (allowValueEdit && RoleInstance.GetLinksToRoleCollection(editValueInstance).Count <= 1)
						{
							editValueInstance.Value = newText;
						}
						else
						{
							editValueInstance = AddAndInitializeValueInstance(newText, parentType);
						}
					}
					else
					{
						editValueInstance = AddAndInitializeValueInstance(newText, parentType);
					}
					rootInstance = editValueInstance;
					return editValueInstance;
				}
				else if (null != (pid = parentType.PreferredIdentifier))
				{
					LinkedElementCollection<Role> identifierRoles = pid.RoleCollection;
					Debug.Assert(identifierRoles.Count == 1);
					Role identifierRole = identifierRoles[0];
					ObjectInstance editEntityInstance = startObjectInstance;
					// Note that we don't offer direct text editing of subtype instances, so is either a ValueInstance
					// of an EntityInstance.
					RoleInstance editingRoleInstance = null;
					bool recurseAllowValueEdit = create;
					if (editEntityInstance != null)
					{
						editingRoleInstance = editEntityInstance.FindIdentifyingRoleInstance(identifierRole);
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
								recurseAllowValueEdit = editEntityInstance.EntitySubtypeInstanceCollection.Count <= maxLeft;
							}
						}
					}
					ObjectInstance objectInstance = RecurseValueInstance(
						(editingRoleInstance != null) ? editingRoleInstance.ObjectInstance : null,
						identifierRole.RolePlayer,
						newText,
						ref rootInstance,
						create,
						recurseAllowValueEdit);
					LinkedElementCollection<ObjectInstance> instances = parentType.ObjectInstanceCollection;
					int instanceCount = instances.Count;
					for (int i = 0; i < instanceCount; ++i)
					{
						ObjectInstance instance = instances[i];
						RoleInstance matchingSingleRoleInstance = null;
						instance.IterateIdentifyingRoleInstances((role, roleInstance) => {
							if (roleInstance != null)
							{
								if (matchingSingleRoleInstance == null && roleInstance.ObjectInstance == objectInstance)
								{
									matchingSingleRoleInstance = roleInstance;
									return false;
								}
								matchingSingleRoleInstance = null;
								return true;
							}
							return false;
						});

						if(matchingSingleRoleInstance != null)
						{
							return instance;
						}
					}

					if (create)
					{
						// Determine if the entity population is implied by a nested fact type population
						FactType objectifiedFactType;
						LinkedElementCollection<FactType> identifierFactTypes;
						if (null != (objectifiedFactType = parentType.NestedFactType) &&
							pid.IsInternal &&
							1 == (identifierFactTypes = pid.FactTypeCollection).Count &&
							objectifiedFactType == identifierFactTypes[0])
						{
							// UNDONE: NOW Kill EntityTypeRoleInstance I don't think we're treating this separately any more. See if this case even exists. It should be handled automatically by EnsureIdentifyingRoleInstance in the other branch.
							FactInstance editFactInstance;
							if (editEntityInstance == null || editEntityInstance.IterateIdentifyingRoleInstances((role, roleInstance) => roleInstance != null) || null == (editFactInstance = editEntityInstance.ObjectifiedInstance))
							{
								editFactInstance = new FactInstance(objectifiedFactType.Store);
								editFactInstance.FactType = objectifiedFactType;
								editEntityInstance = editFactInstance.ObjectifyingInstance;
							}
							new RoleInstance(identifierRole, objectInstance).FactInstance = editFactInstance;
						}
						else
						{
							if (editEntityInstance == null || editEntityInstance.IterateIdentifyingRoleInstances((role, instance) => instance != null))
							{
								editEntityInstance = new ObjectInstance(parentType.Store);
								editEntityInstance.ObjectType = parentType;
							}
							editEntityInstance.EnsureIdentifyingRoleInstance(identifierRole, objectInstance);
						}
					}
					return editEntityInstance;
				}
				else
				{
					// Set the identity tag if there is no preferred identifier

					// First see if we have an existing instance with this name in the identity tag
					if (!string.IsNullOrEmpty(newText))
					{
						foreach (ObjectInstance existingMatch in parentType.ObjectInstanceCollection)
						{
							if (existingMatch.IdentityTag == newText)
							{
								return existingMatch;
							}
						}
					}

					ObjectInstance editEntityInstance = startObjectInstance;
					if (create)
					{
						if (editEntityInstance == null || RoleInstance.GetLinksToRoleCollection(editEntityInstance).Count > 1)
						{
							editEntityInstance = new ObjectInstance(parentType.Store);
							editEntityInstance.ObjectType = parentType;
						}
						editEntityInstance.AutoIdentityTag = false;
						editEntityInstance.IdentityTag = newText;
					}
					return editEntityInstance;
				}
			}
			private Func<ObjectType, bool, ObjectTypeIdentityInfo> myObjectTypeIdentityCache;
			/// <summary>
			/// Return a cache to determine if an identity tag is visible for a given type.
			/// This is shared between a root branch and all expansions.
			/// If the second argument is true and the object type has not been cached, this returns
			/// an <see cref="ObjectTypeIdentityInfo"/> with a <see cref="ObjectTypeIdentityInfo.Kind"/> value
			/// of <see cref="ObjectTypeIdentityKind.None"/>
			/// </summary>
			public Func<ObjectType, bool, ObjectTypeIdentityInfo> ObjectTypeIdentityCache
			{
				get
				{
					Func<ObjectType, bool, ObjectTypeIdentityInfo> testVisible = myObjectTypeIdentityCache;
					if (testVisible == null)
					{
						myObjectTypeIdentityCache = testVisible = CreateObjectTypeIdentityCache();
					}
					return testVisible;
				}
			}
			/// <summary>
			/// Determine if entity identity info has been requested for a given
			/// object type in this branch.
			/// </summary>
			public bool HasBranchesForEntityType(ObjectType objectType)
			{
				Func<ObjectType, bool, ObjectTypeIdentityInfo> cache = myObjectTypeIdentityCache;
				return cache != null && cache(objectType, true).Kind != ObjectTypeIdentityKind.None;
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

			protected void EditColumnHeader(int column, bool ignoreObjectification, bool resolveUnaryRolePlayer, Role newRole)
			{
				TreeControl.UpdateColumnHeaderAppearance(column, DeriveColumnName(newRole, ignoreObjectification, resolveUnaryRolePlayer), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(newRole, null, ignoreObjectification, resolveUnaryRolePlayer));
			}

			protected void EditColumnHeader(int column, bool ignoreObjectification, ObjectType objectType)
			{
				TreeControl.UpdateColumnHeaderAppearance(column, DeriveColumnName(objectType, ignoreObjectification), VirtualTreeColumnHeaderStyles.Default, (int)GetImageIndex(null, objectType, ignoreObjectification, false));
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

			protected static ObjectInstance AddAndInitializeValueInstance(string newText, ObjectType parentValueType)
			{
				Debug.Assert(parentValueType.IsValueType);
				ObjectInstance newInstance = new ObjectInstance(parentValueType.Store);
				newInstance.ObjectType = parentValueType;
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
				bool dummy;
				CellEditContext editContext = CreateEditContext(row, column, out dummy);
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
			/// <param name="useDefaultEdit">If this is set and null is returned then default
			/// editing behavior continues (blocked otherwise)</param>
			/// <returns><see cref="CellEditContext"/> or <see langword="null"/> if
			/// a context is not available.</returns>
			protected virtual CellEditContext CreateEditContext(int row, int column, out bool useDefaultEdit)
			{
				Debug.Assert(!UsesEditContext);
				useDefaultEdit = true;
				return null;
			}
			#endregion // Branch Update Methods
		}
		private sealed class ValueTypeBranch : BaseBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly List<ObjectInstance> myCachedInstances;
			private readonly ObjectType myValueType;
			#endregion
			#region Construction
			// Value Type Branches will always have 1 column, plus the full row select column
			public ValueTypeBranch(ObjectType selectedValueType) : base(2, selectedValueType.Store, null)
			{
				Debug.Assert(selectedValueType.IsValueType);
				myValueType = selectedValueType;
				myCachedInstances = new List<ObjectInstance>();
				myCachedInstances.AddRange(selectedValueType.ObjectInstanceCollection);
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
						AddAndInitializeValueInstance(newText, valueType);
						t.Commit();
					}
				}
				else if (textIsEmpty)
				{
					ObjectInstance valueInstance = valueType.ObjectInstanceCollection[row];
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, valueType.Name, valueInstance.Name)))
					{
						valueInstance.Delete();
						t.Commit();
					}
				}
				else
				{
					ObjectInstance valueInstance = valueType.ObjectInstanceCollection[row];
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
					text = myCachedInstances[row].Name;
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
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				int column = 0;
				if (style == ErrorObject)
				{
					CompatibleValueInstanceError badValueError;
					if (null != (badValueError = obj as CompatibleValueInstanceError))
					{
						obj = badValueError.ValueInstance;
						style = ObjectStyle.TrackingObject;
						column = 1; // The first column is for full-row select, choose the second to edit.
					}
				}
				if (style == ObjectStyle.TrackingObject)
				{
					int row;
					ObjectInstance valueInstance;
					ObjectType valueType;
					if (null != (valueInstance = obj as ObjectInstance) &&
						(valueType = valueInstance.ValueType) == myValueType &&
						-1 != (row = myCachedInstances.IndexOf(valueInstance)))
					{
						return new LocateObjectData(row, column, (int)TrackingObjectAction.ThisLevel);
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
				
				classInfo = dataDirectory.FindDomainRelationship(ObjectTypeHasObjectInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasValueInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasValueInstanceDeletedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectInstance.ValueDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValueInstanceValueChangedEvent), action);
			}
			private void ValueTypeHasValueInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectTypeHasObjectInstance link = (ObjectTypeHasObjectInstance)e.ModelElement;
				ObjectType valueType = link.ObjectType;
				if (!valueType.IsDeleted && valueType == myValueType && valueType.DataType != null)
				{
					myCachedInstances.Add(link.ObjectInstance);
					base.AddInstanceDisplay(VisibleItemCount - 1);
				}
			}

			private void ValueTypeHasValueInstanceDeletedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectTypeHasObjectInstance link = (ObjectTypeHasObjectInstance)e.ModelElement;
				ObjectType valueType = link.ObjectType;
				if (!valueType.IsDeleted && valueType == myValueType && valueType.DataType != null)
				{
					List<ObjectInstance> instances = myCachedInstances;
					int instanceLocation = instances.IndexOf(link.ObjectInstance);
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
					{
						instances.RemoveAt(instanceLocation);
						base.RemoveInstanceDisplay(instanceLocation);
					}
				}
			}

			private void ValueInstanceValueChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectInstance instance = (ObjectInstance)e.ModelElement;
				ObjectType modifiedValueType = instance.ValueType;
				if (modifiedValueType != null && !modifiedValueType.IsDeleted && modifiedValueType == myValueType)
				{
					int location = modifiedValueType.ObjectInstanceCollection.IndexOf(instance);
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
					return ValueTypeHasDataType.GetLinkToDataType(myValueType).AutoGenerated;
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
					ObjectInstance valueInstance = myValueType.ObjectInstanceCollection[row];
					using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, myValueType.Name, valueInstance.Name)))
					{
						valueInstance.Delete();
						t.Commit();
					}
				}
			}
			/// <summary>
			/// Create an edit context to force an 'Auto Generate' into a dropdown instead of a simple editor
			/// </summary>
			protected override CellEditContext CreateEditContext(int row, int column, out bool useDefaultEdit)
			{
				useDefaultEdit = true;
				if (row == NewRowIndex)
				{
					return new CellEditContext(myValueType, delegate (string commitText) { (this as IBranch).CommitLabelEdit(row, column, commitText); });
				}
				return null;
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
			private readonly List<ObjectInstance> myCachedInstances;
			/// <summary>
			/// The count of known instances in the cache that are not currently empty.
			/// </summary>
			private int myNonEmptyInstanceCount;
			private bool myShowIdentityTag;
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
			public EntityTypeBranch(ObjectType entityType, UniquenessConstraint preferredIdentifier, ObjectType unidentifiedRootSupertype, int idLength, Func<ObjectType, bool, ObjectTypeIdentityInfo> objectTypeIdentityCache, bool showIdentityTag)
				: base(idLength + (showIdentityTag ? 2 : 1), entityType.Store, objectTypeIdentityCache) // +1 for row selection column, +2 to show identity tag column
			{
				Debug.Assert(!entityType.IsValueType);
				myShowIdentityTag = showIdentityTag;
				if (preferredIdentifier != null)
				{
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
				}
				else if (unidentifiedRootSupertype != null)
				{
					myEntityTypeSubtype = entityType;
					myEntityType = unidentifiedRootSupertype;
				}
				else
				{
					myEntityType = entityType;
				}
				LinkedElementCollection<ObjectInstance> instances = entityType.ObjectInstanceCollection;
				int instanceCount = instances.Count;
				List<ObjectInstance> instanceCache;
				if (instanceCount == 0)
				{
					instanceCache = new List<ObjectInstance>();
				}
				else
				{
					instanceCache = new List<ObjectInstance>(instanceCount);
					int lastEmptyIndex = instanceCount;
					int nonEmptyCount = 0;
					for (int i = 0; i < instanceCount; ++i)
					{
						ObjectInstance instance = instances[i];
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
				// If editing an existing EntityInstance
				if (row != NewRowIndex)
				{
					ObjectInstance editInstance;
					ObjectInstance editSubtypeInstance;
					if (selectedEntityTypeSubtype != null)
					{
						editSubtypeInstance = myCachedInstances[row];
						editInstance = editSubtypeInstance.SupertypeInstance;
					}
					else
					{
						editInstance = myCachedInstances[row];
						editSubtypeInstance = null;
					}

					int identifierOffset = 1;
					if (myShowIdentityTag)
					{
						if (column == 1)
						{
							// Editing the identity tag. We may not have a preferred identifier in this case
							if (delete && editInstance.RoleCollection.Count == 0 && editInstance.EntitySubtypeInstanceCollection.Count == 0)
							{
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, selectedEntityType.Name, editInstance.Name)))
								{
									editInstance.Delete();
									t.Commit();
								}
							}
							else
							{
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceIdentityTagTransactionText, selectedEntityType.Name, editInstance.Name, newText)))
								{
									editInstance.AutoIdentityTag = false;
									editInstance.IdentityTag = newText;
									t.Commit();
								}
							}
							return LabelEditResult.AcceptEdit;
						}
						identifierOffset = 2;
					}

					Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - identifierOffset]; // If we get this far (no identity tag) then we have a preferred identifier
					RoleInstance editRoleInstance = editInstance.FindIdentifyingRoleInstance(identifierRole);
					// If editing an existing RoleInstance
					if (editRoleInstance != null)
					{
						if (delete)
						{
							ObjectInstance deleteInstance = editRoleInstance.ObjectInstance;
							ObjectInstance deleteValueInstance = null;
							if (identifierRole.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
							{
								// If we delete the role instance then a new instance will be created automatically because of
								// the implied population pattern. To avoid this situation, first check if we have a ValueType
								// and if the ValueType instance is playing no other roles. This is the standard popular reference
								// mode pattern. If this is the case, then we delete the opposite ValueType instance. If this is
								// not the case, then we should not proceed with the deletion.
								deleteValueInstance = deleteInstance;
								if (deleteValueInstance == null || deleteValueInstance.ValueType == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
								{
									throw new InvalidOperationException(ResourceStrings.ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText);
								}
							}

							using (Transaction t = store.TransactionManager.BeginTransaction(
								editInstance.IterateIdentifyingRoleInstances((identifyingRole, testRoleInstance) => testRoleInstance != null && testRoleInstance != editRoleInstance) ?
									string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, selectedEntityType.Name, editInstance.Name) :
									string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(identifierRole, false), deleteInstance.Name, editInstance.Name)))
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
							ObjectInstance objectInstance = editRoleInstance.ObjectInstance;
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, editRolePlayer.Name, objectInstance.Name)))
							{
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(objectInstance, editRolePlayer, newText, ref instance, true, true);
								ConnectInstance(myEntityType, myEntityTypeSubtype, ref editInstance, ref editSubtypeInstance, result, identifierRole);
								t.Commit();
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					// If editing an existing EntityInstance but creating a new RoleInstance
					else if (!delete)
					{
						using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedEntityType.Name)))
						{
							ObjectInstance instance = null;
							ObjectInstance result = RecurseValueInstance(null, identifierRole.RolePlayer, newText, ref instance, true, true);
							ConnectInstance(myEntityType, myEntityTypeSubtype, ref editInstance, ref editSubtypeInstance, result, identifierRole);
							t.Commit();
						}
						return LabelEditResult.AcceptEdit;
					}
				}
				// New Row Editing
				else if (!delete)
				{
					using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedEntityType.Name)))
					{
						if (myShowIdentityTag && column == 1)
						{
							ObjectInstance instance = new ObjectInstance(selectedEntityType.Store);
							instance.ObjectType = selectedEntityType;
							instance.AutoIdentityTag = false;
							instance.IdentityTag = newText;
						}
						else
						{
							Role identifierRole = selectedEntityTypeSubtype != null ? GetIdentifyingSupertypeRole(selectedEntityTypeSubtype) : selectedEntityType.PreferredIdentifier.RoleCollection[column - (myShowIdentityTag ? 2 : 1)];
							ObjectInstance instance = null;
							ObjectInstance result = RecurseValueInstance(null, identifierRole.RolePlayer, newText, ref instance, true, true);
							ObjectInstance parentInstance = null;
							ObjectInstance parentSubtypeInstance = null;
							ConnectInstance(selectedEntityType, selectedEntityTypeSubtype, ref parentInstance, ref parentSubtypeInstance, result, identifierRole);
						}
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
					column -= myShowIdentityTag ? 2 : 1;
					ObjectType selectedEntityTypeSubtype = myEntityTypeSubtype;
					EntityEditorBranch expandedBranch;
					ObjectInstance parentInstance = null;
					if (selectedEntityTypeSubtype != null)
					{
						expandedBranch = new EntityEditorBranch(
							null,
							selectedEntityTypeSubtype,
							parentInstance = (row < myNonEmptyInstanceCount ? myCachedInstances[row] : null),
							GetPreferredSubtypeRole(selectedEntityTypeSubtype),
							this);
					}
					else
					{
						ObjectType selectedEntityType = myEntityType;
						ObjectInstance editInstance = null;
						UniquenessConstraint pid = selectedEntityType.PreferredIdentifier;
						Role identifierRole;
						if (pid == null || (identifierRole = pid.RoleCollection[column]).FactType.UnaryPattern != UnaryValuePattern.NotUnary)
						{
							return null;
						}
						ObjectInstance parentEntityInstance = null;
						if (row < myNonEmptyInstanceCount)
						{
							parentEntityInstance = myCachedInstances[row];
							RoleInstance foundRoleInstance = parentEntityInstance.FindIdentifyingRoleInstance(identifierRole);
							if (foundRoleInstance != null)
							{
								editInstance = foundRoleInstance.ObjectInstance;
							}
						}
						expandedBranch = new EntityEditorBranch(parentEntityInstance, selectedEntityType, editInstance, identifierRole, this);
					}
					if (parentInstance == null)
					{
						IUnattachedBranch[] branches = myUnattachedBranches ?? (myUnattachedBranches = new IUnattachedBranch[ColumnCount - 1]);
						branches[column] = expandedBranch;
					}
					return expandedBranch;
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				string text = base.GetText(row, column);
				if (text == null) // Signal that this is the new row
				{
					if (myShowIdentityTag)
					{
						if (column == 1)
						{
							return string.Empty;
						}
						--column;
					}
					ObjectType displayType = null;
					Role identifierRole;
					if (myEntityTypeSubtype != null)
					{
						displayType = myEntityType;
					}
					else if ((identifierRole = myEntityType.ResolvedPreferredIdentifier.RoleCollection[column - 1]).FactType.UnaryPattern == UnaryValuePattern.NotUnary)
					{
						displayType = identifierRole.RolePlayer;
					}
					text = displayType != null ? ObjectInstance.GetDisplayString(null, displayType, false) : string.Empty;
				}
				else if (text.Length == 0)
				{
					if (myShowIdentityTag)
					{
						if (column == 1)
						{
							return myCachedInstances[row].IdentityTag;
						}
						--column;
					}
					if (myEntityTypeSubtype != null)
					{
						ObjectInstance subtypeInstance = myCachedInstances[row];
						text = (subtypeInstance != null) ? subtypeInstance.Name : ObjectInstance.GetDisplayString(null, myEntityType, false);
					}
					else
					{
						ObjectType selectedEntityType = myEntityType;
						ObjectInstance selectedInstance = myCachedInstances[row];
						Role identifierRole = selectedEntityType.PreferredIdentifier.RoleCollection[column - 1];
						RoleInstance roleInstance = selectedInstance.FindIdentifyingRoleInstance(identifierRole);
						if (identifierRole.FactType.UnaryPattern == UnaryValuePattern.NotUnary)
						{
							text = (roleInstance != null) ? roleInstance.ObjectInstance.Name : ObjectInstance.GetDisplayString(null, identifierRole.RolePlayer, false);
						}
						else
						{
							text = roleInstance != null ? "\x2713" : string.Empty;
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
					Role identifierRole;
					// Checking the StateMask is a workaround to an tree control issue where an expanded sibling subitem
					// will cause a checkbox to draw in all of the empty rows inside the unary column cell.
					if (
						requiredData.StateMask != 0 &&
						!IsReadOnly && // The text is also a checkbox in the readonly case. There is no need to set the second check
						myEntityTypeSubtype == null
					)
					{
						if (myShowIdentityTag)
						{
							if (column == 1)
							{
								if (row != NewRowIndex)
								{
									ObjectInstance selectedInstance = myEntityTypeSubtype != null ? myCachedInstances[row].SupertypeInstance : myCachedInstances[row];
									if (selectedInstance != null && !string.IsNullOrEmpty(selectedInstance.IdentityTag) && !selectedInstance.AutoIdentityTag)
									{
										retVal.Bold = true;
									}
								}
								return retVal;
							}
							--column;
						}
						if ((identifierRole = myEntityType.PreferredIdentifier.RoleCollection[column - 1]).FactType.UnaryPattern != UnaryValuePattern.NotUnary)
						{
							bool isChecked = false;
							if (row < NewRowIndex)
							{
								ObjectInstance selectedInstance = myEntityTypeSubtype != null ? myCachedInstances[row].SupertypeInstance : myCachedInstances[row];
								isChecked = null != selectedInstance.FindIdentifyingRoleInstance(identifierRole);
							}
							retVal.StateImageIndex = (short)(isChecked ? StandardCheckBoxImage.CheckedFlat : StandardCheckBoxImage.UncheckedFlat);
						}
					}
				}
				return retVal;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				Role identifierRole;
				if (
					!IsReadOnly &&
					myEntityTypeSubtype == null
				)
				{
					if (myShowIdentityTag)
					{
						if (column == 1)
						{
							return StateRefreshChanges.None;
						}
						--column;
					}
					if ((identifierRole = myEntityType.PreferredIdentifier.RoleCollection[column - 1]).FactType.UnaryPattern != UnaryValuePattern.NotUnary)
					{
						ObjectInstance parentInstance = null;
						ObjectInstance parentSubtypeInstance = null;
						if (row < NewRowIndex)
						{
							if (myEntityTypeSubtype != null)
							{
								parentSubtypeInstance = myCachedInstances[row];
								parentInstance = parentSubtypeInstance.SupertypeInstance;
							}
							else
							{
								parentInstance = myCachedInstances[row];
							}

							RoleInstance roleInstance = parentInstance.FindIdentifyingRoleInstance(identifierRole);
							if (roleInstance != null && roleInstance.Role.FactType.UnaryPattern != UnaryValuePattern.NotUnary)
							{
								using (Transaction t = identifierRole.Store.TransactionManager.BeginTransaction(
									parentInstance.IterateIdentifyingRoleInstances((role, testInstance) => testInstance != null && testInstance != roleInstance) ?
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, parentInstance.EntityType.Name, parentInstance.Name) :
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveUnaryRoleInstanceTransactionText, string.Format(identifierRole.FactType.GetDefaultReading().Text, identifierRole.RolePlayer?.Name ?? string.Empty), parentInstance.Name)))
								{
									roleInstance.Delete();
									t.Commit();
								}
								return StateRefreshChanges.Current;
							}
						}

						using (Transaction t = identifierRole.Store.TransactionManager.BeginTransaction(
							parentInstance == null ?
								string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, myEntityType.Name) :
								string.Format(ResourceStrings.ModelSamplePopulationEditorAddUnaryRoleInstanceTransactionText, string.Format(identifierRole.FactType.GetDefaultReading().Text, identifierRole.RolePlayer?.Name ?? string.Empty), parentInstance.Name)))
						{
							ConnectInstance(myEntityType, myEntityTypeSubtype, ref parentInstance, ref parentSubtypeInstance, null, identifierRole);
							t.Commit();
						}
					}
				}
				return StateRefreshChanges.None;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				if (!base.IsFullRowSelectColumn(column))
				{
					if (myShowIdentityTag)
					{
						if (column == 1)
						{
							return false;
						}
						--column;
					}
					Role role;
					return myEntityTypeSubtype != null || ((role = myEntityType.ResolvedPreferredIdentifier.RoleCollection[column - 1]).FactType.UnaryPattern == UnaryValuePattern.NotUnary && HasComplexIdentifier(role.RolePlayer));
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
					TooFewEntityRoleInstancesError partialPopulationError;
					CompatibleValueInstanceError badValueError;
					int row = -1;
					int column = myShowIdentityTag ? 1 : 0;
					if (null != (partialPopulationError = obj as TooFewEntityRoleInstancesError))
					{
						ObjectInstance entityInstance = partialPopulationError.EntityInstance;
						UniquenessConstraint pid;
						int testRow;
						if (entityInstance.EntityType == myEntityType &&
							-1 != (testRow = myCachedInstances.IndexOf(entityInstance)) &&
							testRow < myNonEmptyInstanceCount &&
							null != (pid = myEntityType.PreferredIdentifier))
						{
							IList<Role> identifierRoles = pid.RoleCollection;
							int roleCount = identifierRoles.Count;
							for (int i = 0; i < roleCount; ++i)
							{
								if (null == entityInstance.FindIdentifyingRoleInstance(identifierRoles[i]))
								{
									row = testRow;
									column = i + 1;
									break;
								}
							}
						}
					}
					else if (null != (badValueError = obj as CompatibleValueInstanceError))
					{
						ObjectInstance valueInstance = badValueError.ValueInstance;
						foreach (RoleInstance roleInstance in RoleInstance.GetLinksToRoleCollection(valueInstance))
						{
							ObjectInstance entityInstance = roleInstance.IdentifiedEntityInstance;
							UniquenessConstraint pid;
							int testRow;
							if (entityInstance != null &&
								entityInstance.EntityType == myEntityType &&
								-1 != (testRow = myCachedInstances.IndexOf(entityInstance)) &&
								testRow < myNonEmptyInstanceCount &&
								null != (pid = myEntityType.PreferredIdentifier))
							{
								IList<Role> identifierRoles = pid.RoleCollection;
								row = testRow;
								column = identifierRoles.IndexOf(roleInstance.Role) + 1;
								break;
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
					ObjectInstance entityInstance;
					ObjectType entityType;
					if (null != (entityInstance = obj as ObjectInstance) &&
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
					case 1:
						return myShowIdentityTag ? SubItemCellStyles.Simple : SubItemCellStyles.Mixed;
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

				// Track EntityInstance changes
				classInfo = dataDirectory.FindDomainRelationship(ObjectTypeHasObjectInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(InstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(InstanceRemovedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectInstanceNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
				propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);

				// Handlers to manage empty instances based on the role content
				classInfo = dataDirectory.FindDomainRelationship(FactInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityRoleInstanceRemovedEvent), action);

				eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(EntitySubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), new EventHandler<RolePlayerChangedEventArgs>(SubtypeInstanceSupertypeRolePlayerChangedEvent), action);
			}

			private void InstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectTypeHasObjectInstance link = (ObjectTypeHasObjectInstance)e.ModelElement;
				if (!link.IsDeleted)
				{
					ObjectType linkEntityType = link.ObjectType;
					if (linkEntityType == (myEntityTypeSubtype ?? myEntityType))
					{
						ObjectInstance instance = link.ObjectInstance;
						if (myEntityType.PreferredIdentifier != null || !string.IsNullOrEmpty(instance.IdentityTag))
						{
							TestNotifyAddInstance(instance);
						}
					}
				}
			}

			private void InstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectTypeHasObjectInstance link = (ObjectTypeHasObjectInstance)e.ModelElement;
				ObjectType entityType = link.ObjectType;
				if (!entityType.IsDeleted && entityType == (myEntityTypeSubtype ?? myEntityType))
				{
					ObjectInstance instance = link.ObjectInstance;
					List<ObjectInstance> instances = myCachedInstances;
					int instanceLocation = instances.IndexOf(instance);
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

			private void ObjectInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectInstance objectInstance = (ObjectInstance)e.ModelElement;
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
					int headerOffset = myShowIdentityTag ? 2 : 1;
					if (headerOffset +  pidRoleCount == ColumnCount) // Sanity check, protect against transitioning identifier styles.
					{
						for (int i = 0; i < pidRoleCount; ++i)
						{
							base.EditColumnHeader(i + headerOffset, false, false, pidRoles[i]);
						}
					}
				}
			}
			/// <summary>
			/// Helper function for <see cref="EntityRoleInstanceAddedEvent"/>
			/// </summary>
			private void EntityInstancePartAddedEvent(ObjectInstance entityInstance)
			{
				if (
					!entityInstance.IsDeleted &&
					entityInstance.EntityType == myEntityType
				)
				{
					// Make sure we have the appropriate instance
					ObjectType testSubtype = myEntityTypeSubtype;
					ObjectInstance verifyInstance = (testSubtype == null) ? entityInstance : ObjectInstance.GetSubtypeInstance(entityInstance, testSubtype, true, false);
					if (verifyInstance != null)
					{
						TestNotifyAddInstance(verifyInstance);
					}
				}
			}
			/// <summary>
			/// Helper function for <see cref="EntityRoleInstanceRemovedEvent"/>
			/// </summary>
			private void EntityInstancePartRemovedEvent(ObjectInstance entityInstance)
			{
				if (!entityInstance.IsDeleted &&
					entityInstance.EntityType == myEntityType &&
					string.IsNullOrEmpty(entityInstance.IdentityTag) &&
					!entityInstance.HasIdentifyingRoleInstance)
				{
					ObjectType testSubtype = myEntityTypeSubtype;
					ObjectInstance verifyInstance = (testSubtype == null) ? entityInstance : ObjectInstance.GetSubtypeInstance(entityInstance, testSubtype, true, false);
					if (verifyInstance != null)
					{
						List<ObjectInstance> instances = myCachedInstances;
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
			private void EntityRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactInstanceHasRoleInstance link = (FactInstanceHasRoleInstance)e.ModelElement;
				if (!link.IsDeleted)
				{
					link.FindEntityWithIdentifyingRole(out ObjectInstance entityInstance, out Role identifyingRole);
					if (entityInstance != null)
					{
						EntityInstancePartAddedEvent(entityInstance);
					}
				}
			}
			private void EntityRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				((FactInstanceHasRoleInstance)e.ModelElement).FindEntityWithIdentifyingRole(out ObjectInstance entityInstance, out Role identifyingRole);
				if (entityInstance != null)
				{
					EntityInstancePartRemovedEvent(entityInstance);
				}
			}
			private void SubtypeInstanceSupertypeRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ElementLink;
				ObjectInstance subtypeInstance;
				if (!link.IsDeleted &&
					!(subtypeInstance = link.EntitySubtypeInstance).IsDeleted &&
					subtypeInstance.ObjectType == myEntityTypeSubtype)
				{
					if (IsEmptyInstance(subtypeInstance))
					{
						List<ObjectInstance> instances = myCachedInstances;
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
			bool IUnattachedBranchOwner.TryAnchorUnattachedBranches(ObjectInstance objectInstance, FactInstance factInstance)
			{
				if (
					objectInstance != null &&
					!objectInstance.IsDeleted &&
					objectInstance.ObjectType == myEntityType
				)
				{
					// Make sure we have the appropriate instance
					ObjectType testSubtype = myEntityTypeSubtype;
					if (objectInstance.ObjectType?.PreferredIdentifier != null || (testSubtype == null && !string.IsNullOrEmpty(objectInstance.IdentityTag)))
					{
						ObjectInstance verifyInstance = (testSubtype == null) ? objectInstance : ObjectInstance.GetSubtypeInstance(objectInstance, testSubtype, true, false);
						if (verifyInstance != null)
						{
							return TestNotifyAddInstance(verifyInstance);
						}
					}
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
			/// <param name="connectInstance">Instance to connect. Not set for a unary identifier role</param>
			/// <param name="identifierRole">Role to connect to</param>
			public static void ConnectInstance(ObjectType entityType, ObjectType entityTypeSubtype, ref ObjectInstance parentInstance, ref ObjectInstance parentSubtypeInstance, ObjectInstance connectInstance, Role identifierRole)
			{
				Debug.Assert(entityType != null);
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
						ObjectInstance supertypeInstance = connectInstance.SupertypeInstance;
						if (supertypeInstance != null)
						{
							parentSubtypeInstance = connectInstance;
							parentInstance = supertypeInstance;
							return;
						}
						else if (connectInstance.ObjectType?.PreferredIdentifier != null)
						{
							parentInstance = connectInstance;
							parentSubtypeInstance = ObjectInstance.GetSubtypeInstance(connectInstance, entityTypeSubtype, false, true);
							return;
						}
					}

					if (identifierRole.FactType.UnaryPattern == UnaryValuePattern.NotUnary)
					{
						switch (identifierRole.GetReferenceSchemePattern())
						{
							case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
							case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
								// The FactType patterns are one-to-one and implied. We should never
								// create another instance if one is already available.
								foreach (RoleInstance testRoleInstance in RoleInstance.GetLinksToRoleCollection(connectInstance))
								{
									ObjectInstance testEntityInstance = testRoleInstance.IdentifiedEntityInstance;
									if (testEntityInstance != null && testEntityInstance.EntityType == entityType)
									{
										parentInstance = testEntityInstance;
										if (entityTypeSubtype != null)
										{
											parentSubtypeInstance = ObjectInstance.GetSubtypeInstance(testEntityInstance, entityTypeSubtype, true, false);
										}
										break;
									}
								}
								break;
						}
						if (parentInstance == null)
						{
							parentInstance = new ObjectInstance(store);
							parentInstance.ObjectType = entityType;
							parentInstance.EnsureIdentifyingRoleInstance(identifierRole, connectInstance);
						}
					}
					else
					{
						parentInstance = new ObjectInstance(store);
						parentInstance.ObjectType = entityType;
						parentInstance.EnsureIdentifyingRoleInstance(identifierRole, null);
					}

					if (entityTypeSubtype != null && parentSubtypeInstance == null)
					{
						parentSubtypeInstance = ObjectInstance.GetSubtypeInstance(parentInstance, entityTypeSubtype, false, true);
					}
				}
				else if (parentSubtypeInstance != null)
				{
					parentSubtypeInstance.SupertypeInstance = connectInstance;
				}
				else
				{
					parentInstance.EnsureIdentifyingRoleInstance(identifierRole, connectInstance);
				}
			}
			/// <summary>
			/// See if a <see cref="ObjectInstance"/> needs to be added.
			/// Anchor unattached branches and notify as needed.
			/// </summary>
			/// <param name="objectInstance">The instance to attach</param>
			/// <returns>Returns true if the new instance was previously unknown and is not empty.</returns>
			private bool TestNotifyAddInstance(ObjectInstance objectInstance)
			{
				List<ObjectInstance> instances = myCachedInstances;
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
						ObjectInstance emptyInstance = instances[nonEmptyCount];
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
			protected sealed override CellEditContext CreateEditContext(int row, int column, out bool useDefaultEdit)
			{
				CellEditContext retVal = null;
				useDefaultEdit = false;
				int roleOffset = 1;
				if (myShowIdentityTag)
				{
					if (column == 1)
					{
						useDefaultEdit = true;
						return null;
					}
					roleOffset = 2;
				}
				ObjectType entityType = myEntityType;
				ObjectType entityTypeSubtype = myEntityTypeSubtype;
				Role identifierRole = entityType.PreferredIdentifier.RoleCollection[column - roleOffset];
				ObjectType columnRolePlayer;
				if (
					null != (columnRolePlayer = identifierRole.RolePlayer) &&
					(columnRolePlayer.IsValueType || columnRolePlayer.ResolvedPreferredIdentifier != null) &&
					(entityTypeSubtype != null || identifierRole.FactType.UnaryPattern == UnaryValuePattern.NotUnary)
				)
				{
					List<ObjectInstance> instances = myCachedInstances;
					ObjectInstance editInstance = null;
					ObjectInstance editSubtypeInstance = null;
					if (row < myNonEmptyInstanceCount)
					{
						editInstance = (entityTypeSubtype != null) ?
							(editSubtypeInstance = myCachedInstances[row]).SupertypeInstance :
							myCachedInstances[row];
					}
					retVal = new CellEditContext(
						entityType,
						entityTypeSubtype,
						entityTypeSubtype != null ? GetIdentifyingSupertypeRole(entityTypeSubtype) : identifierRole,
						editInstance,
						editSubtypeInstance,
						null,
						delegate(string commitText)
						{
							(this as IBranch).CommitLabelEdit(row, column, commitText);
						});
				}
				return retVal;
			}
			public sealed override void DeleteInstance(int row, int column)
			{
				if (base.IsFullRowSelectColumn(column) && row < myNonEmptyInstanceCount)
				{
					ObjectType targetType = myEntityTypeSubtype ?? myEntityType;
					ObjectInstance targetInstance = targetType.ObjectInstanceCollection[row];
					if (myEntityTypeSubtype == null)
					{
						UniquenessConstraint pid = myEntityType.PreferredIdentifier;
						LinkedElementCollection<Role> roles;
						Role identifierRole;
						ObjectInstance oppositeValueInstance;
						RoleInstance identifyingRoleInstance;
						if (pid != null &&
							(roles = pid.RoleCollection).Count == 1 &&
							(identifierRole = roles[0]).GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole &&
							null != (identifyingRoleInstance = targetInstance.FindIdentifyingRoleInstance(identifierRole)) &&
							(null == (oppositeValueInstance = identifyingRoleInstance.ObjectInstance) ||
							oppositeValueInstance.ValueType == null ||
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
			private List<FactInstance> myCachedFactInstances;
			private List<ObjectInstance> myCachedObjectInstances;
			private FactInstanceImplication myImplicationProxy;
			private ObjectType myObjectifyingType;
			/// <summary>
			/// Expansion branches from the 'new' row that need to
			/// be attached and notified when a new instance is added.
			/// The zero index in this array is the first item column,
			/// not the dummy row number column.
			/// </summary>
			private IUnattachedBranch[] myUnattachedBranches;
			#endregion
			#region Construction
			public FactTypeBranch(FactType selectedFactType, int factTypeColumnCount, ObjectType objectifyingType)
				: base(factTypeColumnCount + ((objectifyingType == null) ? 1 : 2), selectedFactType.Store, CreateObjectTypeIdentityCache())
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
					myCachedFactInstances = new List<FactInstance>(selectedFactType.FactInstanceCollection);
				}
				myObjectifyingType = objectifyingType;
			}
			#endregion
			#region IBranch Interface Members
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				bool delete = newText.Length == 0;
				Store store = Store;
				// If editing an existing FactInstance
				if (row != NewRowIndex)
				{
					FactType selectedFactType = myFactType;
					FactInstance editInstance = myCachedFactInstances[row];
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							Role factRole = selectedFactType.OrderedRoleCollection[column].Role;
							LinkedElementCollection<RoleInstance> factRoleInstances = editInstance.RoleInstanceCollection;
							RoleInstance editRoleInstance = FactInstance.FindRoleInstance(factRoleInstances, factRole);
							// If editing an existing RoleInstance
							if (editRoleInstance != null)
							{
								ObjectInstance attachedInstance = editRoleInstance.ObjectInstance;
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
										using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(editRoleInstance.Role, false), attachedInstance.Name, editInstance.Name)))
										{
											editRoleInstance.Delete();
											t.Commit();
										}
									}
								}
								else
								{
									ObjectInstance instance = null;
									ObjectType editRolePlayer = editRoleInstance.Role.RolePlayer;
									using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, editRolePlayer.Name, attachedInstance.Name)))
									{
										ObjectInstance result = RecurseValueInstance(attachedInstance, editRolePlayer, newText, ref instance, true, true);
										ConnectInstance(ref editInstance, result, factRole, null);
										if (t.HasPendingChanges)
										{
											t.Commit();
										}
									}
								}
								return LabelEditResult.AcceptEdit;
							}
							// If editing an existing FactInstance but creating a new RoleInstance
							else if (!delete)
							{
								using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, selectedFactType.Name)))
								{
									ObjectInstance instance = null;
									ObjectInstance result = RecurseValueInstance(null, factRole.RolePlayer, newText, ref instance, true, true);
									editInstance.EnsureRoleInstance(factRole, result);
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
							break;
						case ColumnType.EntityType:
							ObjectInstance objectifyingInstance = editInstance.ObjectifyingInstance;
							ObjectType objectifyingType = myObjectifyingType;
							if (objectifyingInstance != null)
							{
								if (delete)
								{
									// Check for an empty start instance, note assumption that we aren't text-editing a subtype instance
									ObjectInstance objectifyingEntityInstance;
									LinkedElementCollection<Role> pidRoles;
									int pidRoleCount;
									if (null == (objectifyingEntityInstance = objectifyingInstance) ||
										null == (pidRoles = objectifyingEntityInstance.EntityType?.PreferredIdentifier?.RoleCollection) ||
										0 == (pidRoleCount = pidRoles.Count))
									{
										return LabelEditResult.CancelEdit;
									}


									// Delete entity instance. Note that the user can choose 'unspecified' to detach
									RoleInstance identifyingRoleInstance;
									Role pidRole;
									ObjectInstance deleteValueInstance = null;
									if (pidRoleCount == 1 &&
										null != (identifyingRoleInstance = objectifyingEntityInstance.FindIdentifyingRoleInstance(pidRole = pidRoles[0])) &&
										pidRole.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
									{
										// See comments in EntityTypeBranch.CommitLabelEdit
										deleteValueInstance = identifyingRoleInstance.ObjectInstance;
										if (deleteValueInstance == null || deleteValueInstance.ValueType == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
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
									ObjectInstance instance = null;
									using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorEditInstanceTransactionText, objectifyingType.Name, objectifyingInstance.Name)))
									{
										ObjectInstance result = RecurseValueInstance(objectifyingInstance, objectifyingType, newText, ref instance, true, true);
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
									ObjectInstance instance = null;
									ObjectInstance result = RecurseValueInstance(null, objectifyingType, newText, ref instance, true, true);
									FactInstance existingResultInstance;
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
								FactInstance newInstance = new FactInstance(store);
								newInstance.FactType = selectedFactType;
								Role factRole = selectedFactType.OrderedRoleCollection[column].Role;
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(null, factRole.RolePlayer, newText, ref instance, true, true);
								// instance.Value = newText; // UNDONE: NOW Check set Value after RecurseValueInstance???
								new RoleInstance(factRole, result).FactInstance = newInstance;
								t.Commit();
							}
							break;
						case ColumnType.EntityType:
							ObjectType objectifyingType = myObjectifyingType;
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, objectifyingType.Name)))
							{
								FactInstance newInstance = new FactInstance(store);
								newInstance.FactType = myFactType;
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(null, objectifyingType, newText, ref instance, true, true);
								// instance.Value = newText; // UNDONE: NOW Check set Value after RecurseValueInstance???
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
					List<FactInstance> instances = myCachedFactInstances;
					FactInstance parentInstance = (row < instances.Count) ? instances[row] : null;
					EntityEditorBranch expandedBranch = null;
					int startColumn = column;
					switch (ResolveColumn(ref column))
					{
						case ColumnType.FactType:
							Role selectedRole = selectedFactType.OrderedRoleCollection[column].Role;
							ObjectInstance editInstance = null;
							if (parentInstance != null)
							{
								RoleInstance foundRoleInstance = parentInstance.FindRoleInstance(selectedRole);
								if (foundRoleInstance != null)
								{
									editInstance = foundRoleInstance.ObjectInstance;
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
							text = ObjectInstance.GetDisplayString(null, myFactType.OrderedRoleCollection[column].Role.RolePlayer, false);
							break;
						case ColumnType.EntityType:
							text = ObjectInstance.GetDisplayString(null, myObjectifyingType, true);
							break;
					}
				}
				else if (text.Length == 0)
				{
					if (IsReadOnly)
					{
						Debug.Assert(myObjectifyingType == null);
						ObjectInstance instance = myCachedObjectInstances[row];
						if (instance != null)
						{
							RoleProxy impliedRole = myImplicationProxy.ImpliedProxyRole;
							if (impliedRole != null &&
								myFactType.OrderedRoleCollection.IndexOf(impliedRole) == column - 1)
							{
								FactInstance factInstance;
								if (null != (factInstance = instance.ObjectifiedInstance))
								{
									Role targetRole = impliedRole.TargetRole;
									RoleInstance factRoleInstance;
									if (null != (factRoleInstance = factInstance.FindRoleInstance(targetRole)) &&
										null != (instance = factRoleInstance.ObjectInstance))
									{
										text = instance.Name;
									}
								}
							}
							else
							{
								// If this is a non-unary role in a compound identifier than get the instance from the correct role.
								// Otherwise, just use the name of the instance.
								Role identifyingRole;
								if (null != (identifyingRole = myImplicationProxy.ImpliedIdentifyingRole) &&
									ResolveColumn(ref column) == ColumnType.FactType &&
									identifyingRole == myFactType.OrderedRoleCollection[column] &&
									identifyingRole.FactType.UnaryPattern == UnaryValuePattern.NotUnary)
								{
									text = instance.FindIdentifyingRoleInstance(identifyingRole)?.ObjectInstance?.Name ?? string.Empty;
								}
								else
								{
									text = instance.Name;
								}
							}
						}
					}
					else
					{
						FactInstance factInstance = myCachedFactInstances[row];
						switch (ResolveColumn(ref column))
						{
							case ColumnType.FactType:
								LinkedElementCollection<RoleInstance> roleInstances = factInstance.RoleInstanceCollection;
								int roleInstanceCount = roleInstances.Count;
								RoleInstance instance;
								Role factTypeRole = myFactType.OrderedRoleCollection[column].Role;
								for (int i = 0; i < roleInstanceCount; ++i)
								{
									if (factTypeRole == (instance = roleInstances[i]).Role)
									{
										return instance.ObjectInstance.Name;
									}
								}
								text = ObjectInstance.GetDisplayString(null, factTypeRole.RolePlayer, false);
								break;
							case ColumnType.EntityType:
								ObjectInstance objectifyingInstance = factInstance.ObjectifyingInstance;
								text = (objectifyingInstance != null) ? objectifyingInstance.IdentifierName : ObjectInstance.GetDisplayString(null, myObjectifyingType, true);
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
							ObjectType rolePlayer = myFactType.OrderedRoleCollection[column].Role.RolePlayer;
							return rolePlayer != null && (rolePlayer.NestedFactType != null || !ObjectTypeIdentityCache(rolePlayer, false).DisplayAsInlineValue);
						case ColumnType.EntityType:
							return !ObjectTypeIdentityCache(myObjectifyingType, false).DisplayAsInlineValue;
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
				List<FactInstance> instances;
				TrackingObjectAction trackingAction = TrackingObjectAction.ThisLevel;
				int row = -1;
				int column = 0;
				if (style == ErrorObject &&
					null != (instances = myCachedFactInstances))
				{
					TooFewFactRoleInstancesError partialFactInstancePopulationError;
					TooFewEntityRoleInstancesError partialEntityInstancePopulationError;
					ObjectifyingInstanceRequiredError objectifyingInstanceError;
					FactInstance factInstance;
					if (null != (partialFactInstancePopulationError = obj as TooFewFactRoleInstancesError))
					{
						factInstance = partialFactInstancePopulationError.FactInstance;
						if (-1 != (row = instances.IndexOf(factInstance)))
						{
							IList<RoleBase> factRoles = myFactType.OrderedRoleCollection;
							IList<RoleInstance> roleInstances = factInstance.RoleInstanceCollection;
							int roleCount = factRoles.Count;
							for (int i = 0; i < roleCount; ++i)
							{
								if (null == FactInstance.FindRoleInstance(roleInstances, factRoles[i].Role))
								{
									int testColumn = 1;
									column = i + (ResolveColumn(ref testColumn) == ColumnType.EntityType ? 2 : 1);
									break;
								}
							}
						}
					}
					else if (null != (partialEntityInstancePopulationError = obj as TooFewEntityRoleInstancesError))
					{
						ObjectInstance entityInstance;
						ObjectType entityType;
						if (null != (entityInstance = partialEntityInstancePopulationError.EntityInstance) &&
							null != (entityType = entityInstance.ObjectType) &&
							null != (factInstance = entityInstance.ObjectifiedInstance) &&
							-1 != (row = instances.IndexOf(factInstance)))
						{
							UniquenessConstraint pid;
							if (null != (pid = entityType.PreferredIdentifier) &&
								pid.IsObjectifiedPreferredIdentifier)
							{
								IList<RoleBase> factRoles = myFactType.OrderedRoleCollection;
								IList<RoleInstance> roleInstances = factInstance.RoleInstanceCollection;
								IList<Role> identifierRoles = null;
								int roleCount = factRoles.Count;
								for (int i = 0; i < roleCount; ++i)
								{
									Role role = factRoles[i].Role;
									if (null == FactInstance.FindRoleInstance(roleInstances, factRoles[i].Role))
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
						if (-1 != (row = instances.IndexOf(objectifyingInstanceError.FactInstance)))
						{
							column = 1;
						}
					}
					else if (obj is ObjectifiedInstanceRequiredError)
					{
						// Grab the first FactInstance without an objectifying instance, or the new row
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
					FactInstance factInstance;
					ObjectInstance entityInstance;
					if (null != (factInstance = obj as FactInstance))
					{
						if (!IsReadOnly)
						{
							row = myCachedFactInstances.IndexOf(factInstance);
						}
					}
					else if (null != (entityInstance = obj as ObjectInstance) && entityInstance.ObjectType?.PreferredIdentifier != null) // Check for the entity instance pattern
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

				// Track EntityInstance changes
				classInfo = dataDirectory.FindDomainRelationship(ObjectTypeHasObjectInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeHasEntityInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntityTypeHasEntityInstanceRemovedEvent), action);

				// Track FactInstance changes
				classInfo = dataDirectory.FindDomainRelationship(FactTypeHasFactInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasFactInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasFactInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactInstanceHasRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactInstanceHasRoleInstanceRemovedEvent), action);

#if ROLEINSTANCE_ROLEPLAYERCHANGE
				classInfo = dataDirectory.FindDomainRelationship(RoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(RoleInstanceRolePlayerChangedEvent), action);
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(ObjectInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectInstanceNameChangedEvent), action);

				propertyInfo = dataDirectory.FindDomainProperty(FactInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(FactInstanceNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
				propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ObjectificationInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationInstanceRemovedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationInstanceRolePlayerChangedEvent), action);

				// Handlers to manage implied instances based on the role content
				classInfo = dataDirectory.FindDomainRelationship(FactInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(IdentifyingRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(IdentifyingRoleInstanceRemovedEvent), action);

				eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(EntitySubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), new EventHandler<RolePlayerChangedEventArgs>(SubtypeInstanceSupertypeRolePlayerChangedEvent), action);
			}

			private void ObjectInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectInstance objectInstance = (ObjectInstance)e.ModelElement;
				FactInstance factInstance;
				ObjectType objectifyingType;
				if (IsReadOnly)
				{
					int index;
					if (myImplicationProxy.ImpliedByEntityType == objectInstance.ObjectType &&
						-1 != (index = myCachedObjectInstances.IndexOf(objectInstance)))
					{
						EditInstanceDisplay(index);
					}
				}
				else if (null != (objectifyingType = myObjectifyingType) &&
					objectifyingType == objectInstance.ObjectType &&
					null != (factInstance = objectInstance.ObjectifiedInstance))
				{
					int index = myCachedFactInstances.IndexOf(factInstance);
					if (index != -1)
					{
						EditInstanceDisplay(index);
					}
				}
			}

			private void FactInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				if (!IsReadOnly)
				{
					int index = myCachedFactInstances.IndexOf((FactInstance)e.ModelElement);
					if (index != -1)
					{
						EditInstanceDisplay(index);
					}
				}
			}


			private void EntityTypeHasEntityInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectTypeHasObjectInstance link = e.ModelElement as ObjectTypeHasObjectInstance;
				ObjectType entityType = link.ObjectType;
				if (entityType != null && !entityType.IsDeleted && entityType == myImplicationProxy.ImpliedByEntityType && entityType.PreferredIdentifier != null)
				{
					List<ObjectInstance> instances = myCachedObjectInstances;
					ObjectInstance addedInstance = link.ObjectInstance;
					if (!instances.Contains(addedInstance)) // Possible for implied entity instances when the preferred identifier changes
					{
						instances.Add(addedInstance);
						base.AddInstanceDisplay(instances.Count - 1);
					}
				}
			}

			private void EntityTypeHasEntityInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectTypeHasObjectInstance link = e.ModelElement as ObjectTypeHasObjectInstance;
				ObjectType entityType = link.ObjectType;
				if (entityType != null && !entityType.IsDeleted && entityType == myImplicationProxy.ImpliedByEntityType && entityType.PreferredIdentifier != null)
				{
					List<ObjectInstance> instances = myCachedObjectInstances;
					int instanceLocation = instances.IndexOf(link.ObjectInstance);
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
					{
						instances.RemoveAt(instanceLocation);
						base.RemoveInstanceDisplay(instanceLocation);
					}
				}
			}
			private void FactTypeHasFactInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactTypeHasFactInstance link;
				FactType factType;
				if (!IsReadOnly &&
					!(factType = (link = (FactTypeHasFactInstance)e.ModelElement).FactType).IsDeleted &&
					factType == myFactType)
				{
					TestNotifyAddInstance(link.FactInstance);
				}
			}
			private void FactTypeHasFactInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				FactTypeHasFactInstance link = e.ModelElement as FactTypeHasFactInstance;
				FactType factType = link.FactType;
				List<FactInstance> instances;
				if (!factType.IsDeleted &&
					factType == myFactType &&
					null != (instances = myCachedFactInstances))
				{
					int instanceLocation = instances.IndexOf(link.FactInstance);
					if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
					{
						instances.RemoveAt(instanceLocation);
						base.RemoveInstanceDisplay(instanceLocation);
					}
				}
			}

			private void FactInstanceHasRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EditFactInstanceDisplay(((FactInstanceHasRoleInstance)e.ModelElement).FactInstance);
			}

#if !ROLEINSTANCE_ROLEPLAYERCHANGE
			private void RoleInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				EditFactInstanceDisplay(((RoleInstance)e.ElementLink).FactInstance);
			}
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
			private void FactInstanceHasRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EditFactInstanceDisplay(((FactInstanceHasRoleInstance)e.ModelElement).FactInstance);
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
						EditFactInstanceDisplay((FactInstance)e.OldRolePlayer);
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
				FactInstanceImplication newProxy = new FactInstanceImplication(factType);
				FactInstanceImplication oldProxy = myImplicationProxy;
				int oldItemCount = -1;
				if (oldProxy.IsImplied)
				{
					if (oldProxy != newProxy)
					{
						oldItemCount = VisibleItemCount;
						if (!newProxy.IsImplied)
						{
							myCachedObjectInstances = null;
							myCachedFactInstances = new List<FactInstance>(factType.FactInstanceCollection);
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

			private static List<ObjectInstance> GetNonEmptyObjectInstances(ObjectType objectType)
			{
				LinkedElementCollection<ObjectInstance> instances = objectType.ObjectInstanceCollection;
				int instanceCount = 0;
				List<ObjectInstance> retVal = new List<ObjectInstance>(instanceCount);
				foreach (ObjectInstance instance in instances)
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
			/// <summary>
			/// Helper function for <see cref="IdentifyingRoleInstanceAddedEvent"/>
			/// </summary>
			private void EntityInstancePartAddedEvent(ObjectInstance entityInstance, ObjectType proxyObjectType)
			{
				if (
					!entityInstance.IsDeleting &&
					entityInstance.EntityType == proxyObjectType
				)
				{
					ObjectType subtype = myImplicationProxy.ImpliedByEntityType;
					ObjectInstance verifyInstance = (subtype == proxyObjectType) ? (ObjectInstance)entityInstance : ObjectInstance.GetSubtypeInstance(entityInstance, subtype, true, false);
					if (verifyInstance != null)
					{
						// Make sure we have the appropriate instance
						List<ObjectInstance> instances = myCachedObjectInstances;
						if (!instances.Contains(verifyInstance))
						{
							instances.Add(verifyInstance);
							base.AddInstanceDisplay(instances.Count - 1);
						}
					}
				}
			}
			/// <summary>
			/// Helper function for <see cref="IdentifyingRoleInstanceRemovedEvent"/>
			/// </summary>
			private void EntityInstancePartRemovedEvent(ObjectInstance entityInstance, ObjectType proxyObjectType)
			{
				if (entityInstance.IsDeleted &&
					entityInstance.EntityType == proxyObjectType &&
					string.IsNullOrEmpty(entityInstance.IdentityTag) &&
					!entityInstance.HasIdentifyingRoleInstance)
				{
					ObjectType subtype = myImplicationProxy.ImpliedByEntityType;
					ObjectInstance verifyInstance = (subtype == proxyObjectType) ? (ObjectInstance)entityInstance : ObjectInstance.GetSubtypeInstance(entityInstance, subtype, true, false);
					if (verifyInstance != null)
					{
						List<ObjectInstance> instances = myCachedObjectInstances;
						int instanceLocation = instances.IndexOf(verifyInstance);
						if (instanceLocation != -1) // Possible on add followed by delete in the same transaction
						{
							instances.RemoveAt(instanceLocation);
							base.RemoveInstanceDisplay(instanceLocation);
						}
					}
				}
			}
			private void IdentifyingRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectType proxyObjectType;
				if (null != (proxyObjectType = myImplicationProxy.IdentifyingSupertype))
				{
					((FactInstanceHasRoleInstance)e.ModelElement).FindEntityWithIdentifyingRole(out ObjectInstance entityInstance, out Role identifyingRole);
					if (entityInstance != null)
					{
						EntityInstancePartAddedEvent(entityInstance, proxyObjectType);
					}
				}
			}
			private void IdentifyingRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectType proxyObjectType;
				if (null != (proxyObjectType = myImplicationProxy.IdentifyingSupertype))
				{
					((FactInstanceHasRoleInstance)e.ModelElement).FindEntityWithIdentifyingRole(out ObjectInstance entityInstance, out Role identifyingRole);
					if (entityInstance != null)
					{
						EntityInstancePartRemovedEvent(entityInstance, proxyObjectType);
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
					EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ElementLink;
					ObjectInstance subtypeInstance;
					if (!link.IsDeleted &&
						!(subtypeInstance = link.EntitySubtypeInstance).IsDeleted &&
						subtypeInstance.ObjectType == proxySubType)
					{
						List<ObjectInstance> instances = myCachedObjectInstances;
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
			bool IUnattachedBranchOwner.TryAnchorUnattachedBranches(ObjectInstance objectInstance, FactInstance factInstance)
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
				for (int i = 0; i < roleCount; ++i)
				{
					base.EditColumnHeader(i + columnOffset, false, true, factRoles[i].Role);
				}
			}
			/// <summary>
			/// Connect a given instance to the branch's current objectType, for the specified role or objectifying type
			/// </summary>
			/// <param name="parentInstance">Instance to connect to. Can be null.</param>
			/// <param name="connectInstance">Instance to connect</param>
			/// <param name="factTypeRole">Role to connect to</param>
			/// <param name="objectifyingType">Objectifying <see cref="ObjectType"/>. Specified in place of <paramref name="factTypeRole"/>
			/// to relate a <see cref="FactInstance"/> to an existing objectifying <see cref="ObjectInstance"/></param>
			public static void ConnectInstance(ref FactInstance parentInstance, ObjectInstance connectInstance, Role factTypeRole, ObjectType objectifyingType)
			{
				Debug.Assert(connectInstance != null);
				if (factTypeRole != null)
				{
					connectInstance = ObjectInstance.GetTypedSubtypeInstance(connectInstance, factTypeRole.RolePlayer);
					Store store = factTypeRole.Store;
					FactType factType = factTypeRole.FactType;
					if (parentInstance == null)
					{
						parentInstance = new FactInstance(store);
						parentInstance.FactType = factType;
						new RoleInstance(factTypeRole, connectInstance).FactInstance = parentInstance;
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
						parentInstance = new FactInstance(objectifyingType.Store);
						parentInstance.FactType = objectifyingType.NestedFactType;
					}
					ConnectObjectifyingIdentifierInstance(parentInstance, connectInstance);
				}
			}
			/// <summary>
			/// See if a <see cref="FactInstance"/> needs to be added.
			/// Anchor unattached branches and notify as needed.
			/// </summary>
			/// <param name="factInstance">The instance to attach</param>
			/// <returns>Returns true if the new instance is added.</returns>
			private bool TestNotifyAddInstance(FactInstance factInstance)
			{
				List<FactInstance> instances;
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
						ObjectInstance objectifyingInstance = factInstance.ObjectifyingInstance;
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
			private void EditFactInstanceDisplay(FactInstance factInstance)
			{
				FactType factType = myFactType;
				if (factInstance != null &&
					!factInstance.IsDeleted &&
					null != (factType = myFactType) &&
					factInstance.FactType == factType)
				{
					List<FactInstance> instances;
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
			protected override CellEditContext CreateEditContext(int row, int column, out bool useDefaultEdit)
			{
				CellEditContext retVal = null;
				useDefaultEdit = false;
				ObjectType rolePlayer;
				int startColumn = column;
				switch (ResolveColumn(ref column))
				{
					case ColumnType.FactType:
						Role role = myFactType.OrderedRoleCollection[column].Role;
						rolePlayer = role.RolePlayer;
						if (rolePlayer != null)
						{
							List<FactInstance> instances = myCachedFactInstances;
							retVal = new CellEditContext(
								role,
								(row < instances.Count) ? instances[row] : null,
								null,
								delegate (string commitText)
								{
									(this as IBranch).CommitLabelEdit(row, startColumn, commitText);
								});
						}
						break;
					case ColumnType.EntityType:
						rolePlayer = myObjectifyingType;
						if (rolePlayer.ResolvedPreferredIdentifier != null)
						{
							List<FactInstance> instances = myCachedFactInstances;
							FactInstance factInstance = (row < instances.Count) ? instances[row] : null;
							retVal = new CellEditContext(
								rolePlayer,
								factInstance,
								(factInstance != null) ? factInstance.ObjectifyingInstance : null,
								null,
								delegate (string commitText)
								{
									(this as IBranch).CommitLabelEdit(row, startColumn, commitText);
								});
						}
						break;
				}
				return retVal;
			}
			public sealed override void DeleteInstance(int row, int column)
			{
				if (base.IsFullRowSelectColumn(column) && myImplicationProxy.ImpliedByEntityType == null)
				{
					IList<FactInstance> instances = myCachedFactInstances;
					if (instances != null && row < instances.Count)
					{
						FactInstance factInstance = instances[row];
						using (Transaction t = Store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, factInstance.Name)))
						{
							ObjectInstance objectInstance = (myObjectifyingType != null) ? factInstance.ObjectifyingInstance : null;
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
			private ObjectInstance myEditInstance;
			private ObjectInstance myEditSubtypeInstance;
			private ObjectInstance myParentEntityInstance;
			private ObjectInstance myParentEntitySubtypeInstance;
			private List<EntityEditorBranch> myExpansions = null;
			private readonly ObjectType myParentEntityType;
			private readonly FactType myParentFactType;
			private FactInstance myParentFactInstance;
			private readonly Role myEditRole;
			private readonly ObjectType myObjectifyingType;
			private readonly bool myShowIdentityTag;
			private int myItemCountCache;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentObjectInstance">Instance of the parent Entity type which contains the given editInstance. Pass null for a top-level subtype situation.</param>
			/// <param name="parentEntityType">The Entity type which contains the given editInstance</param>
			/// <param name="editInstance">The <see cref="ObjectInstance"/> that is either an entity instance or an entity subtype instance which will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited. Pass in a <see cref="SubtypeMetaRole"/> to indicate a top-level subtype</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public EntityEditorBranch(ObjectInstance parentObjectInstance, ObjectType parentEntityType, ObjectInstance editInstance, Role editRole, BaseBranch parentBranch)
				: this(editInstance, editRole, null, parentBranch)
			{
				myParentEntityType = parentEntityType;
				if (parentObjectInstance != null)
				{
					ObjectInstance supertypeInstance = parentObjectInstance.SupertypeInstance;
					if (supertypeInstance != null)
					{
						myParentEntitySubtypeInstance = parentObjectInstance;
						myParentEntityInstance = supertypeInstance;
					}
					else
					{
						myParentEntityInstance = parentObjectInstance;
						myParentEntitySubtypeInstance = null;
					}
				}
			}

			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentFactInstance">Instance of the parent Fact type which contains the given editInstance</param>
			/// <param name="parentFactType">The Fact type which contains the given editInstance</param>
			/// <param name="editInstance">The <see cref="ObjectInstance"/> for an entity type or entity type subtype which will be edited</param>
			/// <param name="editRole">Role from the <paramref name="parentFactType"/> which is being edited</param>
			/// <param name="objectifyingType">The objectifying type associated with the <paramref name="parentFactType"/>. Used in place of <paramref name="editRole"/> to edit the objectification relationship.</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			public EntityEditorBranch(FactInstance parentFactInstance, FactType parentFactType, ObjectInstance editInstance, Role editRole, ObjectType objectifyingType, BaseBranch parentBranch)
				: this(editInstance, editRole, objectifyingType, parentBranch)
			{
				myParentFactType = parentFactType;
				myParentFactInstance = parentFactInstance;
				if (objectifyingType != null)
				{
					myParentEntityType = objectifyingType;
					if (myEditRole == null) // The edit role is set by initial constructor for subtype identifier cases
					{
						ObjectInstance objectInstance;
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
			/// <param name="editInstance">The <see cref="ObjectInstance"/> for an entity type or entity type subtypes. The object instance will be edited</param>
			/// <param name="editRole">Role from the parent Entity type which is being edited</param>
			/// <param name="objectifyingType">The objectifying type associated with the <paramref name="editInstance"/>. Used in place of <paramref name="editRole"/> to edit the objectification relationship.</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			private EntityEditorBranch(ObjectInstance editInstance, Role editRole, ObjectType objectifyingType, BaseBranch parentBranch) : base(1, (editRole != null) ? editRole.Store : objectifyingType.Store, parentBranch.ObjectTypeIdentityCache)
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

				if (0 != (this.ObjectTypeIdentityCache(contextObjectType, false).Kind & ObjectTypeIdentityKind.IdentityTag))
				{
					myShowIdentityTag = true;
					++myItemCountCache;
				}
			}
			/// <summary>
			/// Create a sub item editing branch
			/// </summary>
			/// <param name="parentEntitySubtypeInstance">Instance of the parent Entity type which contains the given editInstance</param>
			/// <param name="parentEntityType">The Entity type which contains the given editInstance</param>
			/// <param name="editInstance">The <see cref="ObjectInstance"/> representing an entity instance or entity subtype instance pattern which will be edited</param>
			/// <param name="parentBranch">Reference to the parent editing branch</param>
			private EntityEditorBranch(ObjectInstance parentEntitySubtypeInstance, ObjectType parentEntityType, ObjectInstance editInstance, BaseBranch parentBranch)
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
					return (base.Features & (~BranchFeatures.ComplexColumns)) | BranchFeatures.Expansions | BranchFeatures.StateChanges;
				}
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				bool delete = newText.Length == 0;
				ObjectType rowType;
				Role rowRole;
				ObjectInstance rowInstance;
				FactInstance rowFactInstance;
				ObjectInstance entityInstance;
				RoleInstance identifyingRoleInstance;
				RowType resolvedRowType;
				Store store = Store;
				switch (resolvedRowType = ResolveRow(ref row, out rowType, out rowRole, out rowFactInstance, out identifyingRoleInstance, out rowInstance))
				{
					case RowType.UnaryIdentifierRole: // This may be a dead path, but the code handles it correctly if it is ever hit
					case RowType.IdentifierRole:
						entityInstance = myEditInstance;
						if (entityInstance != null)
						{
							identifyingRoleInstance = entityInstance.FindIdentifyingRoleInstance(rowRole);
						}
						if (delete)
						{
							if (entityInstance != null && identifyingRoleInstance != null)
							{
								using (Transaction t = rowRole.Store.TransactionManager.BeginTransaction(
									(string.IsNullOrEmpty(entityInstance.IdentityTag) && !entityInstance.IterateIdentifyingRoleInstances((testRole, roleInstance) => roleInstance != null && testRole != rowRole)) ?
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, entityInstance.EntityType.Name, entityInstance.Name) :
										(resolvedRowType == RowType.IdentifierRole && identifyingRoleInstance != null ?
											string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(rowRole, false), identifyingRoleInstance.ObjectInstance.Name, entityInstance.Name) :
											string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveUnaryRoleInstanceTransactionText, string.Format(rowRole.FactType.GetDefaultReading().Text, rowRole.RolePlayer?.Name ?? string.Empty), entityInstance.Name))))
								{
									identifyingRoleInstance.Delete();
									t.Commit();
								}
								return LabelEditResult.AcceptEdit;
							}
						}
						else
						{
							using (Transaction t = store.TransactionManager.BeginTransaction(string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, rowType.Name)))
							{
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(
									(identifyingRoleInstance != null) ? identifyingRoleInstance.ObjectInstance : null,
									rowType,
									newText,
									ref instance,
									true,
									true);
								//instance.Value = newText; // UNDONE: NOW Check set Value after RecurseValueInstance???
								ConnectInstance(result, rowRole, null, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
					case RowType.FactRole:
						RoleInstance factRoleInstance = null;
						IList<RoleInstance> factRoleInstances = null;
						if (rowFactInstance != null)
						{
							factRoleInstances = rowFactInstance.RoleInstanceCollection;
							factRoleInstance = FactInstance.FindRoleInstance(factRoleInstances, rowRole);
						}
						if (delete)
						{
							if (rowFactInstance != null &&
								factRoleInstance != null)
							{
								using (Transaction t = rowRole.Store.TransactionManager.BeginTransaction(
									(factRoleInstances.Count == 1) ?
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveFactInstanceTransactionText, rowFactInstance.Name) :
										string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText, GetRolePlayerTypeName(rowRole, true), factRoleInstance.ObjectInstance.Name, rowFactInstance.Name)))
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
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(
									(factRoleInstance != null) ? factRoleInstance.ObjectInstance : null,
									rowType,
									newText,
									ref instance,
									true,
									true);
								// instance.Value = newText;  // UNDONE: NOW Check set Value after RecurseValueInstance???
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
								entityInstance.ObjectType?.PreferredIdentifier != null &&
								(string.IsNullOrEmpty(entityInstance.IdentityTag) && !entityInstance.IterateIdentifyingRoleInstances((testRole, roleInstance) => roleInstance != null && testRole != rowRole)))
							{
								ObjectInstance deleteInstance;
								identifyingRoleInstance = entityInstance.FindIdentifyingRoleInstance(rowRole);
								deleteInstance = identifyingRoleInstance?.ObjectInstance;
								ObjectInstance deleteValueInstance = null;
								if (identifyingRoleInstance != null && identifyingRoleInstance.Role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
								{
									// See comments in EntityTypeBranch.CommitLabelEdit
									deleteValueInstance = deleteInstance;
									if (deleteValueInstance == null || deleteValueInstance.ValueType == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
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
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(
									myEditInstance,
									supertypeRole.RolePlayer,
									newText,
									ref instance,
									true,
									true);
								// instance.Value = newText; // UNDONE: NOW Check set Value after RecurseValueInstance???
								ConnectInstance(result, supertypeRole, null, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
					case RowType.ObjectifyingIdentifier:
						if (delete)
						{
							entityInstance = rowInstance;
							if (entityInstance != null &&
								entityInstance.ObjectType?.PreferredIdentifier != null &&
								(string.IsNullOrEmpty(entityInstance.IdentityTag) && !entityInstance.IterateIdentifyingRoleInstances((testRole, roleInstance) => roleInstance != null && testRole != rowRole)))
							{
								ObjectInstance deleteValueInstance = null;
								identifyingRoleInstance = entityInstance.FindIdentifyingRoleInstance(rowRole);
								if (identifyingRoleInstance?.Role.GetReferenceSchemePattern() == ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole)
								{
									// See comments in EntityTypeBranch.CommitLabelEdit
									deleteValueInstance = identifyingRoleInstance.ObjectInstance;
									if (deleteValueInstance == null || deleteValueInstance.ValueType == null || RoleInstance.GetLinksToRoleCollection(deleteValueInstance).Count > 1)
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
								ObjectInstance instance = null;
								ObjectInstance result = RecurseValueInstance(
									myEditInstance,
									rowType,
									newText,
									ref instance,
									true,
									true);
								// instance.Value = newText; // UNDONE: NOW Check set Value after RecurseValueInstance???
								ConnectInstance(result, null, rowType, null);
								t.Commit();
							}
							return LabelEditResult.AcceptEdit;
						}
						break;
					case RowType.IdentityTag:
						// UNDONE: NOW Figure this out. Add IdentityTag support to RecurseValueInstance or simply connect here?
						break;
				}
				return LabelEditResult.CancelEdit;
			}

			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				ObjectType instanceType;
				Role role;
				int startRow = row;
				RowType rowType = ResolveRow(ref row, out instanceType, out role);
				VirtualTreeDisplayData retVal = new VirtualTreeDisplayData((short)(rowType == RowType.IdentityTag ?
					InstanceTypeImageIndex.IdentityTag :
					GetImageIndex(role, instanceType, rowType == RowType.ObjectifyingIdentifier, false)));

				if (rowType == RowType.UnaryIdentifierRole && !IsReadOnly && requiredData.StateMask != 0) // See comment on StateMask in EntityTypeBranch
				{
					ObjectType dummyRowType;
					ObjectInstance dummyRowInstance;
					FactInstance dummyFactInstance;
					RoleInstance identifyingRoleInstance;

					// Use the more involved routine to get the unary check state
					ResolveRow(ref startRow, out dummyRowType, out role, out dummyFactInstance, out identifyingRoleInstance, out dummyRowInstance);
					retVal.StateImageIndex = (short)(identifyingRoleInstance != null ? StandardCheckBoxImage.CheckedFlat : StandardCheckBoxImage.UncheckedFlat);
				}
				return retVal;
			}

			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				ObjectType rowType;
				ObjectInstance dummyRowInstance;
				FactInstance dummyFactInstance;
				RoleInstance identifyingRoleInstance;
				Role unaryRole;
				if (!IsReadOnly && RowType.UnaryIdentifierRole == ResolveRow(ref row, out rowType, out unaryRole, out dummyFactInstance, out identifyingRoleInstance, out dummyRowInstance))
				{
					if (identifyingRoleInstance != null)
					{
						ObjectInstance parentInstance = dummyRowInstance;
						bool instanceDeleting = string.IsNullOrEmpty(parentInstance.IdentityTag) && !parentInstance.IterateIdentifyingRoleInstances((testRole, roleInstance) => roleInstance != null && testRole != unaryRole);
						using (Transaction t = unaryRole.Store.TransactionManager.BeginTransaction(
							instanceDeleting ?
								string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveObjectInstanceTransactionText, parentInstance.EntityType.Name, parentInstance.Name) :
								string.Format(ResourceStrings.ModelSamplePopulationEditorRemoveUnaryRoleInstanceTransactionText, string.Format(unaryRole.FactType.GetDefaultReading().Text, unaryRole.RolePlayer?.Name ?? string.Empty), parentInstance.Name)))
						{
							identifyingRoleInstance.Delete();
							t.Commit();
						}

						if (instanceDeleting)
						{
							return StateRefreshChanges.None; // Don't tell the tree to refresh a cell that is already removed by other events
						}
					}
					else
					{
						using (Transaction t = unaryRole.Store.TransactionManager.BeginTransaction(
							myEditInstance == null ?
								string.Format(ResourceStrings.ModelSamplePopulationEditorNewInstanceTransactionText, unaryRole.RolePlayer.Name) :
								string.Format(ResourceStrings.ModelSamplePopulationEditorAddUnaryRoleInstanceTransactionText, string.Format(unaryRole.FactType.GetDefaultReading().Text, unaryRole.RolePlayer?.Name ?? string.Empty), myEditInstance.Name)))
						{
							ConnectInstance(null, unaryRole, null, null);
							t.Commit();
						}
					}
					return StateRefreshChanges.Current;
				}
				return StateRefreshChanges.None;
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					ObjectType rowType;
					Role rowRole;
					ObjectInstance rowInstance;
					FactInstance rowFactInstance;
					RoleInstance identifyingRoleInstance;
					bool trackExpansion = false;
					EntityEditorBranch expansion = null;
					switch (ResolveRow(ref row, out rowType, out rowRole, out rowFactInstance, out identifyingRoleInstance, out rowInstance))
					{
						case RowType.IdentifierRole:
							expansion = new EntityEditorBranch(myEditInstance, rowType, rowInstance, rowRole, this);
							trackExpansion = true;
							break;
						//case RowType.UnaryIdentifierRole: This is handled with a local checkbox, not an expansion
						case RowType.Supertype:
							expansion = new EntityEditorBranch(myEditSubtypeInstance, rowType, myEditInstance, this);
							trackExpansion = true;
							break;
						case RowType.FactRole:
							expansion = new EntityEditorBranch(rowFactInstance, myEditRole.RolePlayer.NestedFactType, rowInstance, rowRole, null, this);
							break;
						case RowType.ObjectifyingIdentifier:
							expansion = new EntityEditorBranch(rowFactInstance, rowType.NestedFactType, rowInstance, null, rowType, this);
							trackExpansion = true;
							break;
					}
					if (trackExpansion)
					{
						(myExpansions ?? (myExpansions = new List<EntityEditorBranch>())).Add(expansion);
					}
					return expansion;
				}
				return null;
			}

			string IBranch.GetText(int row, int column)
			{
				ObjectInstance instance;
				ObjectType instanceType;
				RoleInstance identifyingRoleInstance;
				Role dummyRole;
				FactInstance dummyFactInstance;
				switch (ResolveRow(ref row, out instanceType, out dummyRole, out dummyFactInstance, out identifyingRoleInstance, out instance))
				{
					case RowType.ObjectifyingIdentifier:
						return (instance != null) ? instance.IdentifierName : ObjectInstance.GetDisplayString(null, instanceType, true);
					case RowType.Supertype:
						return (instance != null) ? instance.Name : ObjectInstance.GetDisplayString(null, GetIdentifyingSupertypeRole(instanceType).RolePlayer, false);
					case RowType.UnaryIdentifierRole:
						return identifyingRoleInstance != null ? "\x2713" : string.Empty;
					case RowType.IdentityTag:
						return instance != null ? instance.IdentityTag : string.Empty;
				}
				return (instance != null) ? instance.Name : ObjectInstance.GetDisplayString(null, instanceType, false);
			}

			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.Icon)
				{
					ObjectType rowObjectType;
					Role rowRole;
					RowType rowType = ResolveRow(ref row, out rowObjectType, out rowRole);
					if (rowType == RowType.IdentityTag)
					{
						return ResourceStrings.ModelSamplePopulationEditorIdentityTag;
					}
					return (rowRole != null) ?
						DeriveColumnName(rowRole, rowType == RowType.ObjectifyingIdentifier, false) :
						DeriveColumnName(rowObjectType, rowType == RowType.ObjectifyingIdentifier);
				}
				return null;
			}

			bool IBranch.IsExpandable(int row, int column)
			{
				ObjectType instanceType;
				Role dummyRole;
				RowType rowType = ResolveRow(ref row, out instanceType, out dummyRole);
				return instanceType != null && rowType != RowType.IdentityTag && ((rowType != RowType.ObjectifyingIdentifier && instanceType.NestedFactType != null) || HasComplexIdentifier(instanceType));
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
				TooFewEntityRoleInstancesError partialEntityInstancePopulationError;
				ObjectInstance entityInstance;
				ObjectType entityType;
				UniquenessConstraint pid;
				if (style == ErrorObject &&
					null != (partialEntityInstancePopulationError = obj as TooFewEntityRoleInstancesError) &&
					null != (entityInstance = partialEntityInstancePopulationError.EntityInstance) &&
					null != (entityType = entityInstance.EntityType) &&
					myParentEntityType == entityType &&
					null != (pid = entityType.PreferredIdentifier))
				{
					int row = -1;
					IList<Role> identifierRoles = pid.RoleCollection;
					int roleCount = identifierRoles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						if (null == entityInstance.FindIdentifyingRoleInstance(identifierRoles[i]))
						{
							row = i;
							break;
						}
					}

					if (row != -1)
					{
						if (myShowIdentityTag)
						{
							++row;
						}
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

				// Track EntityInstance changes
				classInfo = dataDirectory.FindDomainRelationship(EntitySubtypeInstanceHasSupertypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntitySubtypeHasSupertypeInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(EntitySubtypeHasSupertypeInstanceRemovedEvent), action);
				eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(EntitySubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), new EventHandler<RolePlayerChangedEventArgs>(EntitySubtypeInstanceHasSupertypeInstanceRolePlayerChangedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(FactInstanceHasRoleInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactRoleInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactRoleInstanceRemovedEvent), action);

				classInfo = dataDirectory.FindDomainRelationship(ObjectificationInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationInstanceAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationInstanceRemovedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationInstanceRolePlayerChangedEvent), action);

				DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ObjectInstance.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectInstanceNameChangedEvent), action);
			}

			private void EntitySubtypeInstanceHasSupertypeInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ElementLink;
				ObjectInstance instance;
				ObjectInstance subtypeInstance = link.EntitySubtypeInstance;
				if (RecurseInstanceUpdate(null, link.SupertypeInstance, subtypeInstance, null, InstanceUpdateType.Subtype, out instance) ||
					RecurseInstanceUpdate(null, (ObjectInstance)e.OldRolePlayer, subtypeInstance, null, InstanceUpdateType.Subtype, out instance))
				{
					EditColumnDisplay(0);
				}
			}
			private void EntitySubtypeHasSupertypeInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ModelElement;
				ObjectInstance instance;
				if (RecurseInstanceUpdate(null, link.SupertypeInstance, link.EntitySubtypeInstance, null, InstanceUpdateType.Subtype, out instance))
				{
					EditColumnDisplay(0);
				}
			}
			private void EntitySubtypeHasSupertypeInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ModelElement;
				ObjectInstance instance;
				if (RecurseInstanceUpdate(null, link.SupertypeInstance, link.EntitySubtypeInstance, null, InstanceUpdateType.Subtype, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void FactRoleInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				FactInstanceHasRoleInstance link = (FactInstanceHasRoleInstance)e.ModelElement;
				RoleInstance roleInstance = link.RoleInstance;
				Role role = roleInstance.Role;
				ObjectInstance instance;
				if (RecurseInstanceUpdate(link.FactInstance, null, null, role, InstanceUpdateType.FactTypeRole, out instance))
				{
					EditColumnDisplay(0);
				}

				ObjectInstance entityInstance = roleInstance.IdentifiedEntityInstance;
				if (entityInstance != null &&
					(role.RolePlayer != entityInstance.EntityType || role.FactType.UnaryPattern != UnaryValuePattern.NotUnary))
				{
					if (RecurseInstanceUpdate(null, entityInstance, null, link.RoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out instance))
					{
						EditColumnDisplay(0);
					}
				}
			}

			private void ObjectificationInstanceAddedEvent(object sender, ElementAddedEventArgs e)
			{
				ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
				ObjectInstance instance = link.ObjectifyingInstance;
				bool isSubtypeInstance = instance.SupertypeInstance != null;
				if (RecurseInstanceUpdate(link.ObjectifiedInstance, isSubtypeInstance ? null : instance, isSubtypeInstance ? instance : null, null, InstanceUpdateType.ObjectificationIdentifier, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void ObjectificationInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
				ObjectInstance instance = link.ObjectifyingInstance;
				bool isSubtypeInstance = instance.SupertypeInstance != null;
				if (RecurseInstanceUpdate(link.ObjectifiedInstance, isSubtypeInstance ? null : instance,  isSubtypeInstance ? instance : null, null, InstanceUpdateType.ObjectificationIdentifier, out instance))
				{
					EditColumnDisplay(0);
				}
			}

			private void ObjectificationInstanceRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
			{
				ObjectificationInstance link = (ObjectificationInstance)e.ElementLink;
				ObjectInstance instance = link.ObjectifyingInstance;
				ObjectInstance subtypeInstance;
				ObjectInstance entityInstance;
				if (instance.SupertypeInstance != null)
				{
					subtypeInstance = instance;
					entityInstance = null;
				}
				else
				{
					entityInstance = instance;
					subtypeInstance = null;
				}

				FactInstance factInstance = link.ObjectifiedInstance;
				bool success;
				if (!(success = RecurseInstanceUpdate(factInstance, entityInstance, subtypeInstance, null, InstanceUpdateType.ObjectificationIdentifier, out instance)))
				{
					if (e.DomainRole.Id == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
					{
						factInstance = (FactInstance)e.OldRolePlayer;
					}
					else
					{
						instance = (ObjectInstance)e.OldRolePlayer;
						if (instance.SupertypeInstance != null)
						{
							subtypeInstance = instance;
							entityInstance = null;
						}
						else
						{
							entityInstance = instance;
							subtypeInstance = null;
						}
					}
					success = RecurseInstanceUpdate(factInstance, entityInstance, subtypeInstance, null, InstanceUpdateType.ObjectificationIdentifier, out instance);
				}
				if (success)
				{
					EditColumnDisplay(0);
				}
			}

			private void FactRoleInstanceRemovedEvent(object sender, ElementDeletedEventArgs e)
			{
				if (myEditInstance != null)
				{
					FactInstanceHasRoleInstance link = (FactInstanceHasRoleInstance)e.ModelElement;
					ObjectInstance instance;
					if (RecurseInstanceUpdate(link.FactInstance, null, null, link.RoleInstance.Role, InstanceUpdateType.FactTypeRole, out instance))
					{
						EditColumnDisplay(0);
					}

					link.FindEntityWithIdentifyingRole(out ObjectInstance entityInstance, out Role identifyingRole);
					if (entityInstance != null && RecurseInstanceUpdate(null, entityInstance, null, identifyingRole, InstanceUpdateType.EntityTypeIdentifierRole, out instance))
					{
						EditColumnDisplay(0);
					}
				}
			}

			private void ObjectInstanceNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
			{
				ObjectInstance objectInstance = e.ModelElement as ObjectInstance;
				ObjectInstance editEntityInstance;
				if (myEditInstance == objectInstance)
				{
					EditColumnDisplay(0);
				}
				else if (null != (editEntityInstance = myEditInstance))
				{
					LinkedElementCollection<Role> pidRoles = editEntityInstance.EntityType?.PreferredIdentifier?.RoleCollection;
					if (pidRoles != null)
					{
						int pidRoleCount = pidRoles.Count;
						int location = 0;
						for (int i = 0; i < pidRoleCount; ++i)
						{
							RoleInstance identifyingRoleInstance = myEditInstance.FindIdentifyingRoleInstance(pidRoles[i]);
							if (identifyingRoleInstance != null)
							{
								if (identifyingRoleInstance.ObjectInstance == objectInstance)
								{
									// UNDONE: NOW Kill EntityTypeRoleInstance. I'm not sure this code was right before. Relied on order of entity.RoleInstances, which should be unordered. This might just be the pid role position, which makes more sense.
									EditInstanceDisplay(location);
								}
								++location;
							}
						}
					}
				}
			}
#if SAMPLEPOPULATIONEDITOR_DEBUGHELPER
			/// <summary>
			/// The <see cref="FactInstance"/> from the top-most EntityEditorBranch expansion
			/// </summary>
			public FactInstance TopLevelParentFactInstance
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
			/// The <see cref="EntityInstance"/> from the top-most EntityEditorBranch expansion
			/// </summary>
			public EntityInstance TopLevelParentEntityInstance
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
			/// The <see cref="EntitySubtypeInstance"/> from the top-most EntityEditorBranch expansion
			/// </summary>
			public EntitySubtypeInstance TopLevelParentSubtypeInstance
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
			private bool RecurseInstanceUpdate(FactInstance parentFactInstance, ObjectInstance parentEntityInstance, ObjectInstance parentSubtypeInstance, Role selectedRole, InstanceUpdateType updateType, out ObjectInstance resolvedInstance)
			{
				resolvedInstance = null;
				bool offerToParentBranch = false;
				ObjectInstance parentObjectInstance = (ObjectInstance)parentSubtypeInstance ?? parentEntityInstance;
				if (updateType == InstanceUpdateType.ObjectificationIdentifier &&
					MatchesUpdateType(updateType))
				{
					// Note that the 'MatchesUpdateType' check was designed to provide structure to
					// this spaghetti routine, but recoding this at this point is too sensitive.
					// Consider making better use of the update type when we have a test bed in
					// place to verify the current behavior. The ObjectificationIdentifier case
					// is special because instances always reference the ObjectInstance side
					// of the relationship.
					IBranch parentBranch = myParentBranch;
					EntityEditorBranch parentEditorBranch = parentBranch as EntityEditorBranch;
					IUnattachedBranchOwner unattachedOwner;
					FactInstance contextFactInstance = myParentFactInstance;
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
						ObjectInstance verifiedIdentifier = parentFactInstance.ObjectifyingInstance;
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
							ObjectInstance resolvedParentInstance;
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

				if (updateType == InstanceUpdateType.EntityTypeIdentifierRole &&
					selectedRole != null &&
					parentEntityInstance != null &&
					parentEntityInstance == ContextParentEntityInstance)
				{
					ObjectInstance selectedInstance = parentEntityInstance.FindIdentifyingRoleInstance(selectedRole)?.ObjectInstance;
					if (selectedInstance != ContextObjectInstance)
					{
						ContextObjectInstance = selectedInstance;
						return true;
					}
				}
				else if (parentEntityInstance != null &&
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
						ObjectInstance resolvedParentInstance;
						if (parentEditorBranch.RecurseInstanceUpdate(parentFactInstance, parentEntityInstance, parentSubtypeInstance, selectedRole, updateType, out resolvedParentInstance))
						{
							ContextParentObjectInstance = resolvedParentInstance;
							resolvedInstance = ContextObjectInstance;
							return true;
						}
					}
					else
					{
						ObjectInstance contextSubtypeInstance = ContextParentSubtypeInstance;
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
				ObjectInstance editInstance = myEditInstance;
				if (editInstance != null)
				{
					// We have a match, so there is no need to go farther as long as
					// everything is still in sync. Do a quick verification and defer
					// to full processing if things are out of whack.
					bool instanceChanged = false;
					if (parentSubtypeInstance != null)
					{
						ObjectInstance contextSubtypeInstance = ContextParentSubtypeInstance;
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
								ObjectInstance subtypeInstance = myEditSubtypeInstance;
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
								ObjectInstance subtypeInstance = myParentEntitySubtypeInstance;
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
								ObjectInstance verifyInstance = (ObjectInstance)myEditSubtypeInstance ?? editInstance;
								if (verifyInstance.IsDeleted)
								{
									ObjectInstance contextParent = ContextParentObjectInstance;
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
								RoleInstance roleInstance = editInstance.FindIdentifyingRoleInstance(editRole);
								if (((roleInstance != null) ? roleInstance.ObjectInstance : null) == verifyInstance)
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
								ObjectInstance verifyInstance = (ObjectInstance)myEditSubtypeInstance ?? editInstance;
								if (verifyInstance.IsDeleted)
								{
									ContextObjectInstance = null;
									return true;
								}
								RoleInstance roleInstance = parentFactInstance.FindRoleInstance(editRole);
								if (((roleInstance != null) ? roleInstance.ObjectInstance : null) == verifyInstance)
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
					ObjectInstance subtypeInstance = ObjectInstance.GetSubtypeInstance(parentEntityInstance, myParentEntityType, true, false);
					if (subtypeInstance != null)
					{
						FactInstance contextFactInstance;
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
					ObjectInstance recurseParentObjectInstance = parentObjectInstance ?? (!factMember ? parentFactInstance.ObjectifyingInstance : null);
					if (recurseParentObjectInstance != null)
					{
						IBranch parentBranch = myParentBranch;
						EntityEditorBranch parentEditorBranch;
						IUnattachedBranchOwner unattachedOwner;
						if (null != (parentEditorBranch = parentBranch as EntityEditorBranch))
						{
							ObjectInstance instance = null;
							foreach (RoleInstance recurseParentRoleInstance in RoleInstance.GetLinksToRoleCollection(recurseParentObjectInstance))
							{
								if (recurseParentRoleInstance.Role != editRole)
								{
									continue;
								}
								RoleInstance recurseFactRoleInstance;
								ObjectInstance testRecurseEntityInstance;
								bool recurseMatches = false;
								if (null != (testRecurseEntityInstance = recurseParentRoleInstance.IdentifiedEntityInstance))
								{
									recurseMatches = parentEditorBranch.RecurseInstanceUpdate(null, testRecurseEntityInstance, null, recurseParentRoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out instance);
								}
								else if (null != (recurseFactRoleInstance = recurseParentRoleInstance as RoleInstance))
								{
									recurseMatches = parentEditorBranch.RecurseInstanceUpdate(recurseFactRoleInstance.FactInstance, null, null, recurseParentRoleInstance.Role, InstanceUpdateType.FactTypeRole, out instance);
								}
								if (recurseMatches)
								{
									ContextParentObjectInstance = instance;
									resolvedInstance = ContextObjectInstance;
									return true;
								}
							}
							ObjectInstance recurseEntityInstance = recurseParentObjectInstance;
							if (recurseEntityInstance != null && recurseEntityInstance.ObjectType?.PreferredIdentifier != null) // Ensure this is an entity instance
							{
								foreach (ObjectInstance subtypeInstance in recurseEntityInstance.EntitySubtypeInstanceCollection)
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
								FactInstance contextFactInstance = myParentFactInstance;
								foreach (RoleInstance recurseFactRoleInstance in RoleInstance.GetLinksToRoleCollection(recurseParentObjectInstance))
								{
									if (recurseFactRoleInstance.Role != editRole)
									{
										continue;
									}
									if (null != contextFactInstance)
									{
										if (contextFactInstance == recurseFactRoleInstance.FactInstance)
										{
											ContextObjectInstance = recurseParentObjectInstance;
											resolvedInstance = ContextObjectInstance;
											return true;
										}

									}
									else if (unattachedOwner.TryAnchorUnattachedBranches(null, recurseFactRoleInstance.FactInstance))
									{
										resolvedInstance = ContextObjectInstance;
										return true;
									}
								}
							}
							else
							{
								ObjectInstance contextInstance = ContextParentEntityInstance;
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
			private bool IsInstanceMatch(ObjectInstance matchInstance)
			{
				if (matchInstance == null)
				{
					return false;
				}
				else if (myEditRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact))
				{
					ObjectInstance testInstance;
					if (null == (testInstance = ContextObjectInstance))
					{
						FactInstance factInstance;
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
						RoleInstance testFactRoleInstance;
						ObjectInstance testInstance = null;
						ObjectInstance testEntityInstance;
						bool recurseMatches = false;
						if (null != (testEntityInstance = testRoleInstance.IdentifiedEntityInstance))
						{
							recurseMatches = RecurseInstanceUpdate(null, testEntityInstance, null, testRoleInstance.Role, InstanceUpdateType.EntityTypeIdentifierRole, out testInstance);
						}
						else if (null != (testFactRoleInstance = testRoleInstance as RoleInstance))
						{
							recurseMatches = RecurseInstanceUpdate(testFactRoleInstance.FactInstance, null, null, testRoleInstance.Role, InstanceUpdateType.FactTypeRole, out testInstance);
						}
						if (recurseMatches)
						{
							return matchInstance == testInstance;
						}
					}
					ObjectInstance recurseEntityInstance = matchInstance;
					if (recurseEntityInstance.ObjectType?.PreferredIdentifier != null)
					{
						foreach (ObjectInstance subtypeInstance in recurseEntityInstance.EntitySubtypeInstanceCollection)
						{
							ObjectInstance testInstance;
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
			void IUnattachedBranch.AnchorUnattachedBranch(ObjectInstance objectInstance, FactInstance factInstance)
			{
				if (myParentFactType != null)
				{
					if (factInstance == null && objectInstance != null)
					{
						factInstance = objectInstance.ObjectifiedInstance;
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
			private ObjectInstance ContextParentEntityInstance
			{
				get
				{
					return (myEditRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact)) ? myEditInstance : myParentEntityInstance;
				}
			}
			/// <summary>
			/// Return the parent <see cref="ObjectInstance"/> for an entity subtype if it is set
			/// </summary>
			private ObjectInstance ContextParentSubtypeInstance
			{
				get
				{
					return (myEditRole is SubtypeMetaRole && !(myParentFactType is SubtypeFact)) ? myEditSubtypeInstance : myParentEntitySubtypeInstance;
				}
			}
			/// <summary>
			/// Control setting the instance and subtypeinstance fields
			/// </summary>
			private ObjectInstance ContextParentObjectInstance
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
						FactInstance resolvedFactInstance = (value != null) ? value.ObjectifiedInstance : null;
						bool updateFactInstance = myParentBranch is EntityEditorBranch;
						if (!updateFactInstance)
						{
							// The parent fact instance can change for an expanded. However, if we're a
							// top-level branch, then the parent fact instance cannot change.
							FactInstance parentFactInstance = myParentFactInstance;
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
						ObjectInstance entityInstance = null;
						ObjectInstance subtypeInstance = null;

						if (value != null)
						{
							ObjectInstance supertypeInstance = value.SupertypeInstance;
							if (supertypeInstance != null)
							{
								entityInstance = supertypeInstance;
								subtypeInstance = value;
							}
							else if (value.ObjectType?.PreferredIdentifier != null)
							{
								entityInstance = value;
							}
						}

						if (entityInstance != null)
						{
							myParentEntityInstance = entityInstance;
							myParentEntitySubtypeInstance = subtypeInstance;
							if (objectifyingType != null)
							{
								// Match the parent instances
								ContextObjectInstance = subtypeInstance;
							}
							else if (editRole is SupertypeMetaRole && !(myParentFactType is SubtypeFact))
							{
								// Match the parent entity, but not the subtype
								ContextObjectInstance = entityInstance;
							}
							else
							{
								ObjectInstance foundInstance = null;
								RoleInstance foundRoleInstance = entityInstance.FindIdentifyingRoleInstance(editRole);
								if (foundRoleInstance != null)
								{
									foundInstance = foundRoleInstance.ObjectInstance;
								}
								ContextObjectInstance = foundInstance;
							}
						}
						else
						{
							myParentEntityInstance = null;
							myParentEntitySubtypeInstance = null;
							ContextObjectInstance = null;
						}
					}
				}
			}
			/// <summary>
			/// Control setting the parent fact instance
			/// </summary>
			private FactInstance ContextParentFactInstance
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
						ObjectInstance localInstance = null;
						RoleInstance roleInstance;
						if (value != null &&
							null != (roleInstance = value.FindRoleInstance(myEditRole)))
						{
							localInstance = roleInstance.ObjectInstance;
						}
						ContextObjectInstance = localInstance;
					}
				}
			}
			/// <summary>
			/// Control setting the instance and subtypeinstance fields
			/// </summary>
			private ObjectInstance ContextObjectInstance
			{
				get
				{
					return (ObjectInstance)myEditSubtypeInstance ?? myEditInstance;
				}
				set
				{
					ObjectInstance attachInstance = null;
					ObjectInstance supertypeInstance;
					if (value == null)
					{
						myEditInstance = null;
						myEditSubtypeInstance = null;
					}
					else if (null != (supertypeInstance = value.SupertypeInstance))
					{
						myEditSubtypeInstance = value;
						attachInstance = value;
						myEditInstance = supertypeInstance;
					}
					else
					{
						myEditInstance = value;
						attachInstance = value;
						myEditSubtypeInstance = null;
					}

					List<EntityEditorBranch> unattachedExpansions;
					int unattachedCount;
					if (attachInstance != null &&
						null != (unattachedExpansions = myExpansions) &&
						0 != (unattachedCount = unattachedExpansions.Count))
					{
						for (int i = unattachedCount - 1; i >= 0; --i)
						{
							EntityEditorBranch expansion = unattachedExpansions[i];
							(expansion as IUnattachedBranch).AnchorUnattachedBranch(attachInstance, null); // Do this even for a null instance
						}
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
				/// The row is a unary role that is part of the context EntityType's preferred identifier
				/// </summary>
				UnaryIdentifierRole,
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
				/// <summary>
				/// The row represents the identity tag for an non-subtype entity type.
				/// </summary>
				IdentityTag,
			}
			private RowType ResolveRow(ref int row, out ObjectType rowType, out Role rowRole, out FactInstance rowFactInstance, out RoleInstance rowIdentifyingRoleInstance, out ObjectInstance rowInstance)
			{
				rowType = null;
				rowInstance = null;
				rowRole = null;
				rowFactInstance = null;
				rowIdentifyingRoleInstance = null;
				if (myShowIdentityTag)
				{
					if (row == 0)
					{
						rowInstance = myEditInstance;
						return RowType.IdentityTag;
					}
					--row;
				}
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
					ObjectInstance contextInstance = (ObjectInstance)myEditSubtypeInstance ?? myEditInstance;
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
						FactInstance objectifiedInstance;
						RoleInstance roleInstance;
						if (null != (objectifiedInstance = contextInstance.ObjectifiedInstance))
						{
							rowFactInstance = objectifiedInstance;
							if (null != (roleInstance = objectifiedInstance.FindRoleInstance(role)))
							{
								rowInstance = roleInstance.ObjectInstance;
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
					ObjectInstance instance;
					rowRole = identifierRole;
					if (identifierRole.FactType.UnaryPattern == UnaryValuePattern.NotUnary)
					{
						rowType = identifierRole.RolePlayer;
						RoleInstance roleInstance;
						if (null != (instance = myEditInstance) &&
							null != (roleInstance = instance.FindIdentifyingRoleInstance(identifierRole)))
						{
							rowIdentifyingRoleInstance = roleInstance;
							rowInstance = roleInstance.ObjectInstance;
						}
						return RowType.IdentifierRole;
					}
					else
					{
						if (null != (instance = myEditInstance))
						{
							rowIdentifyingRoleInstance = instance.FindIdentifyingRoleInstance(identifierRole);
						}
						return RowType.UnaryIdentifierRole; // The rowInstance is not set
					}
				}
			}
			private RowType ResolveRow(ref int row, out ObjectType rowType, out Role rowRole)
			{
				rowType = null;
				rowRole = null;
				if (myShowIdentityTag)
				{
					if (row == 0)
					{
						return RowType.IdentityTag;
					}
					--row;
				}
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
					if (identifierRole.FactType.UnaryPattern == UnaryValuePattern.NotUnary)
					{
						rowType = identifierRole.RolePlayer;
						return RowType.IdentifierRole;
					}
					return RowType.UnaryIdentifierRole;
				}
			}
			/// <summary>
			/// Hook up the given instance from a child branch to its parent instance on the current branch.
			/// If the parent instance doesn't exist, it will be created. The method then calls back up
			/// the chain to the current branch's parent branch, passing the newly
			/// created instance to be connected and the role to connect on.
			/// </summary>
			/// <param name="instance">Instance to connect</param>
			/// <param name="identifierRole">Role to connect on</param>
			/// <param name="objectifyingType">The objectifying type. Used in place of <paramref name="identifierRole"/> to edit the objectification relationship.</param>
			/// <param name="factInstance">The fact instance from an objectified FactType</param>
			public void ConnectInstance(ObjectInstance instance, Role identifierRole, ObjectType objectifyingType, FactInstance factInstance)
			{
				Store store = Store;

				ObjectInstance editInstance = myEditInstance;
				ObjectInstance recurseConnectInstance = null;

				if ((identifierRole is SupertypeMetaRole || identifierRole is SubtypeMetaRole) && (factInstance == null || !(factInstance.FactType is SubtypeFact)))
				{
					ObjectType subtype = ContextObjectType;
					ObjectInstance entityInstance = instance;
					ObjectInstance editSubtypeInstance = myEditSubtypeInstance;
					if (editSubtypeInstance != null && editSubtypeInstance.SupertypeInstance == instance)
					{
						// No change, leave the instances bound
						recurseConnectInstance = editSubtypeInstance;
					}
					else
					{
						// If the instance is already bound to a subtype instance for this subtype,
						// then use the existing instance.
						ObjectInstance existingSubtypeInstance = ObjectInstance.GetSubtypeInstance(entityInstance, subtype, true, false);
						if (existingSubtypeInstance != null)
						{
							myEditSubtypeInstance = existingSubtypeInstance;
						}
					}
					if (recurseConnectInstance == null)
					{
						if (editSubtypeInstance == null)
						{
							editSubtypeInstance = ObjectInstance.GetSubtypeInstance(entityInstance, subtype, true, true);
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
						ObjectInstance startingInstance;
						FactInstance attachedFactInstance;
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
							ObjectInstance objectifyingInstance = (ObjectInstance)myEditSubtypeInstance ?? myEditInstance;
							FactInstance objectifiedInstance = (objectifyingInstance != null) ? objectifyingInstance.ObjectifiedInstance : factInstance;
							if (objectifiedInstance == null)
							{
								bool newInstance = objectifyingInstance == null;
								CreateImpliedObjectInstance(editRolePlayer, objectifiedFactType, ref objectifyingInstance, ref objectifiedInstance);
								if (newInstance)
								{
									ContextObjectInstance = objectifyingInstance;
								}
								new RoleInstance(identifierRole, instance).FactInstance = objectifiedInstance;
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
								ObjectInstance subtypeInstance = null;
								switch (identifierRole.GetReferenceSchemePattern())
								{
									case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
									case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
										// The FactType patterns are one-to-one and implied. We should never
										// create another instance if one is already available.
										foreach (RoleInstance testRoleInstance in RoleInstance.GetLinksToRoleCollection(instance))
										{
											ObjectInstance testEntityInstance = testRoleInstance.IdentifiedEntityInstance;
											if (testEntityInstance != null && testEntityInstance.EntityType == identifierFor)
											{
												editInstance = testEntityInstance;
												if (editRolePlayer != identifierFor)
												{
													subtypeInstance = ObjectInstance.GetSubtypeInstance(editInstance, editRolePlayer, true, false);
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
									if (pid.IsInternal &&
										null != (identifierForObjectifiedFactType = ((identifierFor != editRolePlayer) ? identifierFor.NestedFactType : objectifiedFactType)) &&
										1 == (pidFactTypes = pid.FactTypeCollection).Count &&
										(identifierFactType = pidFactTypes[0]) == identifierForObjectifiedFactType)
									{
										// Entity instances are implied, create indirectly through the objectified FactType
										factInstance = new FactInstance(store);
										factInstance.FactType = identifierForObjectifiedFactType;
										new RoleInstance(identifierRole, instance).FactInstance = factInstance;
										editInstance = factInstance.ObjectifyingInstance;
										Debug.Assert(editInstance != null); // The ObjectifyingInstance should be populated by inline rules
									}
									else
									{
										editInstance = new ObjectInstance(store);
										editInstance.ObjectType = identifierFor;
										editInstance.EnsureIdentifyingRoleInstance(identifierRole, instance); // null instance indicates a unary role
									}
								}
								if (subtypeInstance == null && editRolePlayer != identifierFor)
								{
									subtypeInstance = ObjectInstance.GetSubtypeInstance(editInstance, editRolePlayer, false, true);
								}
								recurseConnectInstance = (ObjectInstance)subtypeInstance ?? editInstance;
							}
							else if (pid.IsObjectifiedPreferredIdentifier)
							{
								// Control population through the FactType
								editInstance.ObjectifiedInstance.EnsureRoleInstance(identifierRole, instance);
							}
							else
							{
								editInstance.EnsureIdentifyingRoleInstance(identifierRole, instance); // null instance indicates a unary role
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
			/// Recursively create an <see cref="ObjectInstance"/> required for editing an objectified <see cref="FactInstance"/>
			/// </summary>
			/// <param name="objectifyingType">The objectifying <see cref="ObjectType"/>. Cannot be null.</param>
			/// <param name="objectifiedFactType">The objectified <see cref="FactType"/>. Cannot be null.</param>
			/// <param name="objectInstance">The current or created <see cref="ObjectInstance"/></param>
			/// <param name="factInstance">The current or created <see cref="FactInstance"/></param>
			private static void CreateImpliedObjectInstance(ObjectType objectifyingType, FactType objectifiedFactType, ref ObjectInstance objectInstance, ref FactInstance factInstance)
			{
				ObjectInstance editObjectInstance = objectInstance;
				FactInstance editFactInstance = factInstance;
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
							ObjectInstance editEntityInstance = new ObjectInstance(store);
							editEntityInstance.ObjectType = objectifyingType;
							editObjectInstance = editEntityInstance;
						}
					}
					else
					{
						FactType objectifiedByIdentifierFor = identifierFor.NestedFactType;
						ObjectInstance supertypeInstance = null;
						if (objectifiedByIdentifierFor != null)
						{
							ObjectInstance recurseObjectInstance = null;
							FactInstance recurseFactInstance = null;
							CreateImpliedObjectInstance(identifierFor, objectifiedByIdentifierFor, ref recurseObjectInstance, ref recurseFactInstance);
							supertypeInstance = recurseObjectInstance;
						}
						else
						{
							supertypeInstance = new ObjectInstance(store);
							supertypeInstance.ObjectType = identifierFor;
						}
						editObjectInstance = ObjectInstance.GetSubtypeInstance(supertypeInstance, objectifyingType, false, true);
						if (editFactInstance != null)
						{
							editObjectInstance.ObjectifiedInstance = factInstance;
						}
					}
				}
				if (editFactInstance == null)
				{
					editFactInstance = new FactInstance(store);
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
			protected override CellEditContext CreateEditContext(int row, int column, out bool useDefaultEdit)
			{
				CellEditContext retVal = null;
				useDefaultEdit = false;
				ObjectType rowType;
				ObjectInstance rowInstance;
				RoleInstance rowIdentifyingRoleInstance;
				Role rowRole;
				FactInstance rowFactInstance;
				Role supertypeIdentifyingRole;
				int startRow = row;
				switch (ResolveRow(ref row, out rowType, out rowRole, out rowFactInstance, out rowIdentifyingRoleInstance, out rowInstance))
				{
					case RowType.FactRole:
						retVal = new CellEditContext(rowRole, rowFactInstance, this, delegate (string commitText) { (this as IBranch).CommitLabelEdit(startRow, column, commitText); });
						break;
					case RowType.IdentifierRole:
						supertypeIdentifyingRole = GetIdentifyingSupertypeRole(rowType);
						retVal = new CellEditContext(
							(supertypeIdentifyingRole != null) ? supertypeIdentifyingRole.RolePlayer : rowType,
							(supertypeIdentifyingRole != null) ? rowType : null,
							rowRole,
							myEditInstance,
							myEditSubtypeInstance,
							this,
							delegate (string commitText)
							{
								(this as IBranch).CommitLabelEdit(startRow, column, commitText);
							});
						break;
					//case RowType.UnaryIdentifierRole: // Uses a checkbox, no edit context
					case RowType.ObjectifyingIdentifier:
						retVal = new CellEditContext(rowType, rowFactInstance, rowInstance, this, delegate (string commitText) { (this as IBranch).CommitLabelEdit(startRow, column, commitText); });
						break;
					case RowType.Supertype:
						supertypeIdentifyingRole = GetIdentifyingSupertypeRole(rowType);
						retVal = new CellEditContext(
							(supertypeIdentifyingRole != null) ? supertypeIdentifyingRole.RolePlayer : rowType,
							(supertypeIdentifyingRole != null) ? rowType : null,
							GetPreferredSubtypeRole(rowType),
							myEditInstance,
							myEditSubtypeInstance,
							this,
							delegate (string commitText)
							{
								(this as IBranch).CommitLabelEdit(startRow, column, commitText);
							});
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
 
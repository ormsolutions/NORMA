#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;

#endregion

namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	public partial class ReadingEditor : UserControl
	{
		#region ColumnIndex enum
		private enum ColumnIndex
		{
			ReadingText = 0,
			IsPrimary = 1,
		}
		#endregion // ColumnIndex enum
		#region Static Methods
		/// <summary>
		/// Tests if the ObjectType is the RolePlayer for any of Roles
		/// </summary>
		public static bool IsParticipant(ObjectType objectType, Role[] roleOrder)
		{
			int numRoles = roleOrder.Length;
			for (int i = 0; i < numRoles; ++i)
			{
				if (object.ReferenceEquals(objectType, roleOrder[i].RolePlayer))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tests if the ObjectType is the RolePlayer of the Role
		/// </summary>
		public static bool IsParticipant(ObjectType objectType, Role leadRole)
		{
			return object.ReferenceEquals(objectType, leadRole.RolePlayer);
		}

		/// <summary>
		/// Tests if the ObjectType is the RolePlayer of any Role
		/// contained in the ReadingOrder
		/// </summary>
		public static bool IsParticipant(ObjectType objectType, ReadingOrder readingOrder)
		{
			RoleMoveableCollection roles = readingOrder.RoleCollection;
			foreach (Role role in roles)
			{
				if (object.ReferenceEquals(role.RolePlayer, objectType))
				{
					return true;
				}
			}
			return false;
		}
		#endregion // Static Methods
		#region Member Variables
		private FactType myFact;
		private List<ReadingEntry> myReadingList;
		private ReadingBranch myBranch;
		//the role order of the tree item that is selected, will be null if
		//leading role sub group is selected or the "All" node is selected.
		private Role[] mySelectedRoleOrder;
		//the leading role when a leading role sub group is selected,
		//will be null if a specific reading order or the "All"
		//node are currently selected
		private Role mySelectedLeadRole;
		#endregion // Member Variables
		#region construction
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReadingEditor()
		{
			InitializeComponent();

			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[ReadingBranch.COLUMN_COUNT]
				{
					new VirtualTreeColumnHeader(ResourceStrings.ModelReadingEditorListColumnHeaderReadingText, VirtualTreeColumnHeaderStyles.Default), 
					new VirtualTreeColumnHeader(ResourceStrings.ModelReadingEditorListColumnHeaderIsPrimary, 50, true, VirtualTreeColumnHeaderStyles.Default)
				};
			vtrReadings.SetColumnHeaders(headers, true);
		}
		#endregion
		#region Properties
		/// <summary>
		/// The fact that is being edited in the control, or that needs to be edited.
		/// </summary>
		public FactType EditingFactType
		{
			get
			{
				return myFact;
			}
			set
			{
				myFact = value;
				if (myFact != null)
				{
					PopulateControl();
					tvwReadingOrder.SelectedNode = tvwReadingOrder.Nodes[0];
				}
			}
		}
		#endregion
		#region PopulateControl and helpers
		private void PopulateControl()
		{
			Debug.Assert(myFact != null);

			tvwReadingOrder.Nodes.Clear();
			lstReadings.Items.Clear();

			tvwReadingOrder.Nodes.AddRange(CreateAutoFilledRootNodes());
			RoleMoveableCollection roleSeq = myFact.RoleCollection;
			int numRoles = roleSeq.Count;

			List<Role> roleList = new List<Role>(numRoles);
			for (int i = 0; i < numRoles; ++i)
			{
				roleList.Add(roleSeq[i]);
			}

			List<List<Role>> rolePerms = BuildPermutations(roleList);
			Role lastStart = null;
			TreeNode lastStartNode = null;
			int permCount = rolePerms.Count;
			for (int i = 0; i < permCount; ++i)
			{
				List<Role> curSeq = rolePerms[i];
				if (curSeq[0].Equals(lastStart))
				{
					lastStartNode.Nodes.Add(new ReadingTreeNode(curSeq.ToArray()));
				}
				else
				{
					lastStart = curSeq[0];
					lastStartNode = new ReadingRootTreeNode(lastStart);
					tvwReadingOrder.Nodes.Add(lastStartNode);
					lastStartNode.Nodes.Add(new ReadingTreeNode(curSeq.ToArray()));
				}
			}
			tvwReadingOrder.ExpandAll();
		}

		private List<List<Role>> BuildPermutations(List<Role> roleList)
		{
			List<List<Role>> retval = new List<List<Role>>();
			List<Role> tmpList = null;
			int count = roleList.Count;
			if (count == 1)
			{
				retval.Add(roleList);
			}
			else
			{
				for (int i = 0; i < count; ++i)
				{
					Role currentRole = roleList[i];
					tmpList = new List<Role>(count - 1);
					for (int j = 0; j < count; ++j)
					{
						if (!roleList[j].Equals(currentRole))
						{
							tmpList.Add(roleList[j]);
						}
					}
					List<List<Role>> result = BuildPermutations(tmpList);
					int resCount = result.Count;
					for (int j = 0; j < resCount; ++j)
					{
						result[j].Insert(0, currentRole);
						retval.Add(result[j]);
					}
				}
			}
			return retval;
		}

		private static TreeNode[] CreateAutoFilledRootNodes()
		{
			TreeNode[] retval = new TreeNode[1];
			retval[0] = new TreeNode(ResourceStrings.ModelReadingEditorAllReadingsNodeName);
			return retval;
		}
		#endregion
		#region ReadingList selection change and refresh code
		private void tvwReadingOrder_AfterSelect(object sender, TreeViewEventArgs e)
		{
			RefreshReadingList();
		}

		/// <summary>
		/// Causes the control to reload the reading list and refresh the display.
		/// </summary>
		private void RefreshReadingList()
		{
			ReadingTreeNode readingNode = tvwReadingOrder.SelectedNode as ReadingTreeNode;
			List<ReadingEntry> readingList = new List<ReadingEntry>();
			if (readingNode != null)
			{
				//a specific reading order node is selected
				mySelectedRoleOrder = readingNode.RoleOrder;
				mySelectedLeadRole = null;
				ReadingOrderMoveableCollection readingOrders = myFact.ReadingOrderCollection;
				foreach (ReadingOrder readingOrd in readingOrders)
				{
					if (IsMatchingReadingOrder(readingNode.RoleOrder, readingOrd))
					{
						AddReadingEntries(readingList, readingOrd);
						break;
					}
				}
				readingList.Add(NewReadingEntry.Singleton);
			}
			else
			{
				ReadingRootTreeNode readingRootNode = tvwReadingOrder.SelectedNode as ReadingRootTreeNode;
				if (readingRootNode != null)
				{
					//start role root node is selected
					mySelectedRoleOrder = null;
					mySelectedLeadRole = readingRootNode.LeadRole;
					ReadingOrderMoveableCollection readingOrders = myFact.ReadingOrderCollection;
					foreach (ReadingOrder readingOrd in readingOrders)
					{
						if (IsMatchingLeadRole(readingRootNode.LeadRole, readingOrd))
						{
							AddReadingEntries(readingList, readingOrd);
						}
					}
				}
				else
				{
					mySelectedLeadRole = null;
					mySelectedRoleOrder = null;
					//assuming "All" node is only other possibility
					ReadingOrderMoveableCollection readingOrders = myFact.ReadingOrderCollection;
					foreach (ReadingOrder readingOrd in readingOrders)
					{
						AddReadingEntries(readingList, readingOrd);
					}
				}
			}
			myReadingList = readingList;

			ReadingBranch branch = new ReadingBranch(myReadingList, mySelectedRoleOrder, myFact);
			myBranch = branch;
			ReadingVirtualTree tree = new ReadingVirtualTree(branch);
			this.vtrReadings.MultiColumnTree = tree;
		}

		private static void AddReadingEntries(List<ReadingEntry> readingList, ReadingOrder readingOrder)
		{
			ReadingMoveableCollection readings = readingOrder.ReadingCollection;
			foreach (Reading read in readings)
			{
				readingList.Add(new ReadingEntry(read, readingOrder));
			}
		}

		private static bool IsMatchingReadingOrder(Role[] roleOrder, ReadingOrder readingOrder)
		{
			return IsMatchingReadingOrder(roleOrder, readingOrder.RoleCollection);
		}
		private static bool IsMatchingReadingOrder(Role[] roleOrder, RoleMoveableCollection readingOrderRoles)
		{
			Debug.Assert(roleOrder.Length == readingOrderRoles.Count);
			int numRoles = roleOrder.Length;
			if (numRoles != readingOrderRoles.Count)
			{
				return false;
			}

			for (int i = 0; i < numRoles; ++i)
			{
				if (!object.ReferenceEquals(roleOrder[i], readingOrderRoles[i])) return false;
			}
			return true;
		}

		private static bool IsMatchingLeadRole(Role leadRole, ReadingOrder readingOrder)
		{
			return IsMatchingLeadRole(leadRole, readingOrder.RoleCollection);
		}
		private static bool IsMatchingLeadRole(Role leadRole, RoleMoveableCollection readingOrderRoles)
		{
			return object.ReferenceEquals(leadRole, readingOrderRoles[0]);
		}
		#endregion
		#region Reading activation helper

		/// <summary>
		/// Looks for the tree node that has the same role sequence
		/// as the one passed in, if found it will then select it.
		/// </summary>
		/// <param name="readingOrderRoles">The role sequence to match against</param>
		/// <returns>true if a selection was made, false otherwise</returns>
		private bool SelectNode(RoleMoveableCollection readingOrderRoles)
		{
			bool retval = false;
			TreeNodeCollection nodes = tvwReadingOrder.Nodes;

			// Find the root node
			foreach (TreeNode node in nodes)
			{
				ReadingRootTreeNode rootNode = node as ReadingRootTreeNode;
				if (rootNode != null && IsMatchingLeadRole(rootNode.LeadRole, readingOrderRoles))
				{
					// Find the child node
					foreach (ReadingTreeNode childNode in rootNode.Nodes)
					{
						if (IsMatchingReadingOrder(childNode.RoleOrder, readingOrderRoles))
						{
							// Select to populate the grid
							tvwReadingOrder.SelectedNode = childNode;
							retval = true;
							break;
						}
					}
					break;
				}
			}
			return retval;
		}

		/// <summary>
		/// Select the current reading in the window. The
		/// reading must be the child of the current fact.
		/// </summary>
		/// <param name="reading">Reading</param>
		public void ActivateReading(Reading reading)
		{
			ReadingOrder order;
			FactType factType;
			if (null != (order = reading.ReadingOrder) &&
				null != (factType = order.FactType) &&
				object.ReferenceEquals(factType, myFact))
			{
				RoleMoveableCollection readingOrderRoles = order.RoleCollection;

				if (SelectNode(readingOrderRoles))
				{
					// Find the item in the grid
					if (vtrReadings.SelectObject(null, reading, (int)ObjectStyle.TrackingObject, 0))
					{
						vtrReadings.BeginLabelEdit();
					}
				}
			}
		}

		/// <summary>
		/// Select the primary reading of the reading order
		/// matching the fact's role display order if one exists,
		/// if not selects the new entry for the role
		/// sequence matching the facts role display order.
		/// </summary>
		/// <param name="fact">FactType</param>
		public void ActivateReading(FactType fact)
		{
			ReadingOrder order;
			Reading reading;
			if (null != (order = FactType.FindMatchingReadingOrder(fact))
				&& null != (reading = order.PrimaryReading)
				&& object.ReferenceEquals(fact, myFact))
			{
				RoleMoveableCollection readingOrderRoles = order.RoleCollection;
				if (SelectNode(readingOrderRoles))
				{
					if (vtrReadings.SelectObject(null, reading, (int)ObjectStyle.TrackingObject, 0))
					{
						vtrReadings.BeginLabelEdit();
					}
				}
			}
			else
			{
				RoleMoveableCollection factRoles = fact.RoleCollection;
				if (SelectNode(factRoles))
				{
					if (vtrReadings.SelectObject(null, NewReadingEntry.Singleton, (int)ObjectStyle.TrackingObject, 0))
					{
						vtrReadings.BeginLabelEdit();
					}
				}
			}
		}
		#endregion // Reading activation helper
		#region model events and handlers

		#region event handler attach/detach methods

		/// <summary>
		/// Attaches the event handlers to the store so that the tool window
		/// contents can be updated to reflect any model changes.
		/// </summary>
		public void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);

			// Track Reading changes
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ReadingLinkAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ReadingLinkRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(Reading.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ReadingAttributeChangedEvent));

			// Track ReadingOrder changes
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasReadingOrder.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ReadingOrderLinkRemovedEvent));

			// Track Role changes
			classInfo = dataDirectory.FindMetaClass(ObjectTypePlaysRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ObjectTypePlaysRoleAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ObjectTypePlaysRoleRemovedEvent));

			// Track ObjectType changes
			classInfo = dataDirectory.FindMetaClass(ObjectType.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ObjectTypeAttributeChangedEvent));

			// ReadingOrderHasRole changes
			classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ReadingOrderHasRoleAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ReadingOrderHasRoleRemovedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasFactType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeRemovedEvent));
		}

		/// <summary>
		/// removes the event handlers from the store that were placed to allow
		/// the tool window to keep in sync with the mdoel
		/// </summary>
		public void DetachEventHandlers(Store store)
		{
			if (store == null || store.Disposed)
			{
				return; // Bail out
			}
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);

			// Track Reading changes
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ReadingLinkAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ReadingLinkRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(Reading.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ReadingAttributeChangedEvent));

			// Track ReadingOrder changes
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasReadingOrder.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ReadingOrderLinkRemovedEvent));

			// Track Role changes
			classInfo = dataDirectory.FindMetaClass(ObjectTypePlaysRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ObjectTypePlaysRoleAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ObjectTypePlaysRoleRemovedEvent));

			// Track ObjectType changes
			classInfo = dataDirectory.FindMetaClass(ObjectType.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ObjectTypeAttributeChangedEvent));

			// ReadingOrderHasRole changes
			classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ReadingOrderHasRoleAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ReadingOrderHasRoleRemovedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasFactType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeRemovedEvent));
		}

		#endregion

		#region Reading Event Handlers
		//handling model events Related to changes in Readings and their
		//connections so the reading editor can accurately reflect the model

		private void ReadingLinkAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			Reading read = link.ReadingCollection;
			ReadingOrder ord = link.ReadingOrder;

			if (!object.ReferenceEquals(ord.FactType, myFact))
			{
				return;
			}

			int index = -1;
			//TODO:need to put more work into ordering Readings added to the model.
			//they end up getting put in a different order than they appear when
			//the list is constructed from scratch versus the order when the list
			//is cleared via undo and redo. Might want to look into making
			//the list a sorted list. Only appears when the Readings of more
			//than one ReadingOrder are being shown at the same time.

			//the all node is selected
			if (mySelectedLeadRole == null && mySelectedRoleOrder == null)
			{
				index = ord.ReadingCollection.IndexOf(read);
			}
			//leadrole branch selected
			else if (mySelectedLeadRole != null && IsMatchingLeadRole(mySelectedLeadRole, ord))
			{
				index = ord.ReadingCollection.IndexOf(read);
			}
			//specific order branch selected
			else if (mySelectedRoleOrder != null && IsMatchingReadingOrder(mySelectedRoleOrder, ord))
			{
				index = ord.ReadingCollection.IndexOf(read);
			}

			//test if it already exists so that if the list was built from scratch because
			//of roles being added we don't put the item in the list twice
			if(index > -1 && (IndexOfReadingEntry(read) < 0))
			{
				myReadingList.Insert(index, new ReadingEntry(read, ord));
				myBranch.ItemAdded(index);
			}
		}

		private void ReadingLinkRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			ReadingOrder ord = link.ReadingOrder;
			Reading read = link.ReadingCollection;

			// Handled all at once by ReadingOrderLinkRemovedEvent if all
			// are gone.
			if (!ord.IsRemoved)
			{
				FactType f = ord.FactType;
				if (object.ReferenceEquals(f, myFact))
				{
					RemoveReadingEntry(read);
				}
			}
		}

		private void ReadingAttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			Reading reading = e.ModelElement as Reading;
			ReadingOrder ord = reading.ReadingOrder;

			if (ord == null || !object.ReferenceEquals(ord.FactType, myFact))
			{
				return;
			}

			int index = IndexOfReadingEntry(reading);
			if (index > -1)
			{
				ReadingEntry re = myReadingList[index];
				re.InvalidateText();
				int column = -1;
				Guid attrId = e.MetaAttribute.Id;
				if (attrId.Equals(Reading.TextMetaAttributeGuid))
				{
					column = (int)ColumnIndex.ReadingText;
				}
				else if (attrId.Equals(Reading.IsPrimaryMetaAttributeGuid))
				{
					column = (int)ColumnIndex.IsPrimary;
				}
				myBranch.ItemUpdate(index, column);
			}
		}

		#endregion

		#region ReadingOrder Event Handlers
		//handle model events related to the ReadingOrder or its Roles being removed in order to
		//keep the editor window in sync with what is in the model.

		private void ReadingOrderLinkRemovedEvent(object sender, ElementRemovedEventArgs e)
		{

			FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
			ReadingOrder ord = link.ReadingOrderCollection;
			FactType fact = link.FactType;

			if (!object.ReferenceEquals(fact, myFact))
			{
				return;
			}

			if (!fact.IsRemoved)
			{
				RemoveReadingOrderRelatedEntries(ord);
			}
		}

		private void ReadingOrderHasRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			Role role = link.RoleCollection;
			FactType roleFact = role.FactType;
			if (myFact != null && object.ReferenceEquals(myFact, roleFact) && !myBranch.IsAdding)
			{
				ReloadData();
			}
		}

		private void ReadingOrderHasRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			ReadingOrder ord = link.ReadingOrder;
			if (myFact != null && !myFact.IsRemoved)
			{
				if (myFact.ReadingOrderCollection.Contains(ord))
				{
					ReloadData();
				}
			}
		}

		#endregion

		#region ObjectType Role Players Event Handlers
		//handle model events related to changes in what Roles are associated with
		//the reading editor and what the values of the RolePlayers text is
		//so the editor can stay in sync with the model

		//Currently checking everything, might be good to change it to only
		//test the reading list if an affected selection tree item or one
		//of its children were impacted by the change.

		private void ObjectTypePlaysRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			Role role = link.PlayedRoleCollection;
			ObjectTypeChangedHelper(role);
		}

		private void ObjectTypePlaysRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			Role role = link.PlayedRoleCollection;
			ObjectTypeChangedHelper(role);
		}

		private void ObjectTypeAttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			Guid attrGuid = e.MetaAttribute.Id;
			if (attrGuid.Equals(ObjectType.NameMetaAttributeGuid) && EditingFactType != null)
			{
				ObjectType objectType = e.ModelElement as ObjectType;
				Debug.Assert(objectType != null);
				ObjectTypeChangedHelper(objectType);
			}
		}

		private void ObjectTypeChangedHelper(Role changedRole)
		{
			bool wasImpacted = SetTextOnTreeNodes(changedRole, tvwReadingOrder.Nodes);
			if (wasImpacted)
			{
				int numEntries = myReadingList.Count;
				for (int i = 0; i < numEntries; ++i)
				{
					if (myReadingList[i].Contains(changedRole))
					{
						myBranch.ItemUpdate(i, (int)ColumnIndex.ReadingText);
					}
				}
			}
		}

		private void ObjectTypeChangedHelper(ObjectType changedObjectType)
		{
			bool wasImpacted = SetTextOnTreeNodes(changedObjectType, tvwReadingOrder.Nodes);
			if (wasImpacted)
			{
				int numEntries = myReadingList.Count;
				for (int i = 0; i < numEntries; ++i)
				{
					if (ReadingEditor.IsParticipant(changedObjectType, myReadingList[i].ReadingOrder))
					{
						myBranch.ItemUpdate(i, (int)ColumnIndex.ReadingText);
					}
				}
			}
		}
		#endregion

		#region FactType Event Handlers
		private void FactTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			if (object.ReferenceEquals(link.FactTypeCollection, myFact))
			{
				ORMDesignerPackage.ReadingEditorWindow.EditingFactType = null;
			}
		}
		#endregion // FactType Event Handlers

		#region Helper methods

		private void ReloadData()
		{
			// might be overkill but role removal changes permutations
			PopulateControl();
			tvwReadingOrder.SelectedNode = null;
			tvwReadingOrder.SelectedNode = tvwReadingOrder.Nodes[0];
		}

		/// <summary>
		/// Tests if any custom nodes that have values based on the changed ObjectType
		/// are in the tree and initiates a text change if they are. Uses recursion
		/// to handle child nodes. It returns true if the tree or one of its children
		/// had to update its text because it was dependent on the object for its value.
		/// </summary>
		private static bool SetTextOnTreeNodes(ObjectType changedObjectType, TreeNodeCollection nodes)
		{
			bool wasImpacted = false;
			BaseReadingTreeNode node;
			int numNodes = nodes.Count;
			for (int i = 0; i < numNodes; ++i)
			{
				node = nodes[i] as BaseReadingTreeNode;
				if (node != null)
				{
					if (node.IsImpactedBy(changedObjectType))
					{
						wasImpacted = true;
						node.SetText();
					}
				}

				wasImpacted = wasImpacted | SetTextOnTreeNodes(changedObjectType, nodes[i].Nodes);
			}

			return wasImpacted;
		}

		private static bool SetTextOnTreeNodes(Role changedRole, TreeNodeCollection nodes)
		{
			bool wasImpacted = false;
			BaseReadingTreeNode node;
			int numNodes = nodes.Count;
			for (int i = 0; i < numNodes; ++i)
			{
				node = nodes[i] as BaseReadingTreeNode;
				if (node != null)
				{
					if (node.IsImpactedBy(changedRole))
					{
						wasImpacted = true;
						node.SetText();
					}
				}

				wasImpacted = wasImpacted | SetTextOnTreeNodes(changedRole, nodes[i].Nodes);
			}

			return wasImpacted;
		}

		/// <summary>
		/// Removes any ReadingEntry items from the list that are related
		/// to the indicated ReadingOrder
		/// </summary>
		private void RemoveReadingOrderRelatedEntries(ReadingOrder readingOrder)
		{
			int initNrEntries = myReadingList.Count;
			if (initNrEntries > 0)
			{
				int i = initNrEntries - 1;
				while (i >= 0)
				{
					if (object.ReferenceEquals(myReadingList[i].ReadingOrder, readingOrder))
					{
						myReadingList.RemoveAt(i);
						myBranch.ItemRemoved(i);
					}
					--i;
				}
			}
		}

		/// <summary>
		/// Handles removing the ReadingEntry for the specified Reading
		/// object and handles the branch update as well.
		/// </summary>
		/// <returns>Returns the index of the reading that was removed, -1 if it wasn't found.</returns>
		private int RemoveReadingEntry(Reading reading)
		{
			int index = IndexOfReadingEntry(reading);
			//should be a reading that was part of the currently displayed fact
			if (index >= 0)
			{
				myReadingList.RemoveAt(index);
				myBranch.ItemRemoved(index);
			}
			return index;
		}

		/// <summary>
		/// Locate the index of the ReadingEntry that represents the specified Reading.
		/// </summary>
		private int IndexOfReadingEntry(Reading reading)
		{
			int numEntries = myReadingList.Count;
			int index = -1;
			for (int i = 0; i < numEntries; ++i)
			{
				if (object.ReferenceEquals(myReadingList[i].Reading, reading))
				{
					index = i;
					break;
				}
			}
			return index;
		}
		#endregion

		#endregion
		#region nested abstract class BaseReadingTreeNode
		private abstract class BaseReadingTreeNode : TreeNode
		{
			public abstract void SetText();
			public abstract bool IsImpactedBy(ObjectType objectType);
			public abstract bool IsImpactedBy(Role role);
		}
		#endregion
		#region nested class ReadingRootTreeNode
		private class ReadingRootTreeNode : BaseReadingTreeNode
		{
			Role myLeadRole;

			public ReadingRootTreeNode(Role leadRole)
			{
				Debug.Assert(leadRole != null);

				myLeadRole = leadRole;
				SetText();
			}

			public override void SetText()
			{
				ObjectType rolePlayer = myLeadRole.RolePlayer;
				if (rolePlayer == null)
				{
					this.Text = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
				}
				else
				{
					this.Text = rolePlayer.Name;
				}
			}

			public override bool IsImpactedBy(ObjectType objectType)
			{
				return ReadingEditor.IsParticipant(objectType, myLeadRole);
			}

			public override bool IsImpactedBy(Role role)
			{
				return object.ReferenceEquals(role, LeadRole);
			}

			public Role LeadRole
			{
				get
				{
					return myLeadRole;
				}
			}
		}
		#endregion
		#region nested class ReadingTreeNode
		private class ReadingTreeNode : BaseReadingTreeNode
		{
			Role[] myRoleOrder;

			public ReadingTreeNode(Role[] roleOrder)
			{
				Debug.Assert(roleOrder != null);
				Debug.Assert(roleOrder.Length > 0);

				myRoleOrder = roleOrder;
				SetText();
			}

			public override void SetText()
			{
				StringBuilder sb = new StringBuilder();
				int roleCount = (myRoleOrder == null ? 0 : myRoleOrder.Length);
				for (int i = 0; i < roleCount; ++i)
				{
					ObjectType rolePlayer = myRoleOrder[i].RolePlayer;
					if (rolePlayer == null)
					{
						sb.Append(ResourceStrings.ModelReadingEditorMissingRolePlayerText);
					}
					else
					{
						sb.Append(rolePlayer.Name);
					}
					sb.Append(", ");
				}
				this.Text = sb.ToString(0, sb.Length - 2);
			}

			public override bool IsImpactedBy(ObjectType objectType)
			{
				return ReadingEditor.IsParticipant(objectType, myRoleOrder);
			}

			public override bool IsImpactedBy(Role role)
			{
				int numRoles = myRoleOrder.Length;
				for (int i = 0; i < numRoles; ++i)
				{
					if (object.ReferenceEquals(role, myRoleOrder[i]))
					{
						return true;
					}
				}
				return false;
			}

			public Role[] RoleOrder
			{
				get
				{
					return myRoleOrder;
				}
			}
		}
		#endregion
		#region nested class ReadingEntry
		private class ReadingEntry
		{
			protected static Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}");

			Reading myReading;
			ReadingOrder myReadingOrder;
			String myText;

			#region construction
			protected ReadingEntry()
			{
			}

			public ReadingEntry(Reading reading, ReadingOrder readingOrder)
			{
				Debug.Assert(reading != null, "The associated Reading is required.");
				Debug.Assert(readingOrder != null, "The associated ReadingOrder is required.");
				Debug.Assert(readingOrder.ReadingCollection.Contains(reading), "The Reading must belong to the ReadingOrder");

				myReading = reading;
				myReadingOrder = readingOrder;
			}
			#endregion

			public virtual String Text
			{
				get
				{
//					if (myText == null)
//					{
						myText = GenerateDisplayText();
//					}
					return myText;
				}
			}

			private String GenerateDisplayText()
			{
				String retval = regCountPlaces.Replace(myReading.Text, new MatchEvaluator(ReplacePlaceholders));
				return retval;
			}

			private string ReplacePlaceholders(Match m)
			{
				string retval = null;
				RoleMoveableCollection roles = myReading.ReadingOrder.RoleCollection;
				string matchText = m.Value;
				int rolePosition = int.Parse(matchText.Substring(1, matchText.Length - 2), CultureInfo.InvariantCulture);
				if (roles.Count > rolePosition)
				{
					ObjectType player = roles[rolePosition].RolePlayer;
					if (player != null)
					{
						retval = player.Name;
					}
					else
					{
						retval = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
					}
				}
				else
				{
					retval = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
				}
				return retval;
			}

			public Reading Reading
			{
				get
				{
					return myReading;
				}
			}

			public ReadingOrder ReadingOrder
			{
				get
				{
					return myReadingOrder;
				}
			}

			/// <summary>
			/// Notifies the class that the text to display for the underlying reading
			/// needs to be regenerated.
			/// </summary>
			public void InvalidateText()
			{
				myText = null;
			}

			public bool Contains(Role role)
			{
				if (myReading != null)
				{
					return myReadingOrder.RoleCollection.Contains(role);
				}
				return false;
			}
		}
		#endregion
		#region nested class NewReadingEntry
		private class NewReadingEntry : ReadingEntry
		{
			private static NewReadingEntry mySingleton;

			private NewReadingEntry()
			{
			}

			public static NewReadingEntry Singleton
			{
				get
				{
					if (mySingleton == null)
					{
						//assuming not multi-threaded
						mySingleton = new NewReadingEntry();
					}
					return mySingleton;
				}
			}

			public override String Text
			{
				get
				{
					return ResourceStrings.ModelReadingEditorNewItemText;
				}
			}
		}
		#endregion
		#region nested class ReadingBranch
		private class ReadingBranch : IBranch, IMultiColumnBranch
		{
			public const int COLUMN_COUNT = 2;

			List<ReadingEntry> myReadingList;
			BranchModificationEventHandler myModificationEvents;
			Role[] myRoleOrder;
			FactType myFact;
			int myInsertedRow = -1;

			#region Construction
			public ReadingBranch(List<ReadingEntry> readingList, Role[] roleOrder, FactType fact)
			{
				Debug.Assert(readingList != null);
				Debug.Assert(fact != null);

				myRoleOrder = roleOrder;
				myReadingList = readingList;
				myFact = fact;
			}
			#endregion

			#region IBranch Member Mirror/Implementations

			VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				VirtualTreeLabelEditData retval;
				if (column == (int) ColumnIndex.ReadingText)
				{
					retval = VirtualTreeLabelEditData.Default;
					Reading reading = myReadingList[row].Reading;
					if (reading == null)
					{
						StringBuilder sb = new StringBuilder();
						int numRoles = myRoleOrder.Length;
						for (int i = 0; i < numRoles; ++i)
						{
							sb.Append("{");
							sb.Append(i);
							sb.Append("}");
						}
						retval.AlternateText = sb.ToString();
					}
					else
					{
						retval.AlternateText = reading.Text;
					}
				}
				else
				{
					retval = VirtualTreeLabelEditData.Invalid;
				}
				return retval;
			}

			LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				Reading theReading = myReadingList[row].Reading;
				if (theReading == null)
				{
					if (myRoleOrder == null)
					{
						//should only get here on "New" line and the role order
						//should have be specified if one was added.
						Debug.Assert(false, "Should not be possible to have no role order.");
						return LabelEditResult.BlockDeactivate;
					}
					else
					{
						//Code to handle adding a new reading.
						try
						{
							myInsertedRow = row;

							using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
							{
								ReadingOrder theOrder = FactType.GetReadingOrder(myFact, myRoleOrder);
								Debug.Assert(theOrder != null, "A ReadingOrder should have been found or created.");
								Reading theNewReading = Reading.CreateReading(theOrder.Store);
								ReadingMoveableCollection readings = theOrder.ReadingCollection;
								readings.Add(theNewReading);
								theNewReading.Text = newText;
								t.Commit();
							}
						}
						finally
						{
							myInsertedRow = -1;
						}
					}
				}
				else
				{
					if (column == (int) ColumnIndex.IsPrimary)
					{
						return LabelEditResult.BlockDeactivate;
					}
					else if (column == (int) ColumnIndex.ReadingText)
					{
						//The reading text is being changed
						using (Transaction t = theReading.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorChangeReadingText))
						{
							theReading.Text = newText;
							t.Commit();
						}
					}
				}
				return LabelEditResult.AcceptEdit;
			}

			static BranchFeatures Features
			{
				get
				{
					return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.StateChanges | BranchFeatures.InsertsAndDeletes | BranchFeatures.JaggedColumns;
				}
			}

			static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}

			VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				if (column == (int) ColumnIndex.IsPrimary)
				{
					Reading theReading = myReadingList[row].Reading;
					if (theReading != null)
					{
						if (theReading.IsPrimary)
						{
							retval.StateImageIndex = (int)StandardCheckBoxImage.Indeterminate;
						}
						else
						{
							retval.StateImageIndex = (int)StandardCheckBoxImage.Unchecked;
						}
					}
				}
				return retval;
			}

			object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return myReadingList[row];
			}

			string GetText(int row, int column)
			{
				String retval = null;
				if (column == (int) ColumnIndex.ReadingText)
				{
					retval = myReadingList[row].Text;
				}
				return retval;
			}

			static string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (column == (int) ColumnIndex.IsPrimary)
				{
					return ResourceStrings.ModelReadingEditorIsPrimaryToolTip;
				}
				return null;
			}

			static bool IsExpandable(int row, int column)
			{
				return false;
			}

			LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				switch (style)
				{
					case ObjectStyle.TrackingObject:
						//indicates desire to select the "New" item
						NewReadingEntry newEntry = NewReadingEntry.Singleton;
						if (object.ReferenceEquals(obj, newEntry))
						{
							List<ReadingEntry> list = myReadingList;
							int listLoc;
							if (-1 != (listLoc = list.IndexOf(newEntry)))
							{
								return new LocateObjectData(listLoc, 0, (int)TrackingObjectAction.ThisLevel);
							}
						}
						else
						{
							Reading reading = obj as Reading;
							if (reading != null)
							{
								List<ReadingEntry> list = myReadingList;
								int listCount = list.Count;
								for (int i = 0; i < listCount; ++i)
								{
									if (object.ReferenceEquals(reading, list[i].Reading))
									{
										return new LocateObjectData(i, 0, (int)TrackingObjectAction.ThisLevel);
									}
								}
							}
						}
						break;
				}
				return new LocateObjectData();
			}

			event BranchModificationEventHandler OnBranchModification
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

			static void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}

			static void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}

			static void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}

			static VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}

			StateRefreshChanges ToggleState(int row, int column)
			{
				StateRefreshChanges retval = StateRefreshChanges.None;
				Reading theReading = myReadingList[row].Reading;
				if (theReading != null)
				{
					if (!theReading.IsPrimary)
					{
						using (Transaction t = theReading.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorChangePrimaryReadingText))
						{
							theReading.IsPrimary = true; //only false ones should get this far
							t.Commit();
							retval = StateRefreshChanges.ParentsChildren;
						}
					}
				}
				return retval;
			}
			static StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			static int UpdateCounter
			{
				get
				{
					return 0;
				}
			}

			int VisibleItemCount
			{
				get
				{
					return myReadingList.Count;
				}
			}
			#endregion
			#region IBranch Members
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return Features;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return GetAccessibilityData(row, column);
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return GetDisplayData(row, column, requiredData);
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return GetTipText(row, column, tipType);
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return LocateObject(obj, style, locateOptions);
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
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
				OnDragEvent(sender, row, column, eventType, args);
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
				OnGiveFeedback(args, row, column);
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
				OnQueryContinueDrag(args, row, column);
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return OnStartDrag(sender, row, column, reason);
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return ToggleState(row, column);
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
			}
			int IBranch.UpdateCounter
			{
				get
				{
					return UpdateCounter;
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
			static int ColumnCount
			{
				get
				{
					return COLUMN_COUNT;
				}
			}

			static SubItemCellStyles ColumnStyles(int column)
			{
				return SubItemCellStyles.Simple;
			}

			int GetJaggedColumnCount(int row)
			{
				// For the 'New' row, let the edit box
				// and new selection go all the way across.
				return (myReadingList[row] is NewReadingEntry) ? 1 : COLUMN_COUNT;
			}
			#endregion
			#region IMultiColumnBranch Members
			int IMultiColumnBranch.ColumnCount
			{
				get
				{
					return ColumnCount;
				}
			}

			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}

			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				return GetJaggedColumnCount(row);
			}
			#endregion

			#region branch update methods
			/// <summary>
			/// Used to find out if the branch is in the process of adding a new entry from
			/// input into the branch.
			/// </summary>
			public bool IsAdding
			{
				get
				{
					return myInsertedRow != -1;
				}
			}

			/// <summary>
			/// Triggers the events notifying the tree that an item in the branch has been updated.
			/// </summary>
			public void ItemUpdate(int row, int column)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.VisibleElements, this, row, column, 1)));
				}
			}

			/// <summary>
			/// Tell the branch to update it contents because an item has been added.
			/// </summary>
			/// <param name="row">zero based index of where the new item was placed.</param>
			public void ItemAdded(int row)
			{
				if (myModificationEvents != null)
				{
					if (myInsertedRow > -1)
					{
						myModificationEvents(this, BranchModificationEventArgs.MoveItem(this, myInsertedRow, row));
						row = this.VisibleItemCount - 1; // Insert at the new row location
					}
					myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, row - 1, 1));
				}
			}

			/// <summary>
			/// Triggers notification that an item has been removed from the branch.
			/// </summary>
			public void ItemRemoved(int row)
			{
				if (myModificationEvents != null)
				{
					myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, row, 1));
				}
			}
			#endregion
		}
		#endregion
		#region nested class ReadingVirtualTree
		private class ReadingVirtualTree : MultiColumnTree
		{
			public ReadingVirtualTree(IBranch root) : base(ReadingBranch.COLUMN_COUNT)
			{
				Debug.Assert(root != null);
				this.Root = root;
			}
		}
		#endregion
	}
}

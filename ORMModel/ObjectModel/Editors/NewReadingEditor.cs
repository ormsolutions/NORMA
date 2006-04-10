/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using System.Drawing.Design;

namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	#region ActiveFactType structure
	/// <summary>
	/// A structure to represent the currently active fact. Allows for
	/// a custom display order, such as the custom order available from
	/// a FactTypeShape.
	/// </summary>
	public struct ActiveFactType
	{
		/// <summary>
		/// Represents an empty ActiveFactType
		/// </summary>
		public static readonly ActiveFactType Empty = new ActiveFactType();
		/// <summary>
		/// Is the structure equivalent to ActiveFactType.Empty?
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myFactType == null;
			}
		}
		private FactType myFactType;
		private RoleMoveableCollection myDisplayOrder;
		/// <summary>
		/// Set the active fact type to just use the native role order for its display order
		/// </summary>
		/// <param name="factType">A fact type. Can be null.</param>
		public ActiveFactType(FactType factType) : this(factType, null){}
		/// <summary>
		/// Set the active fact type to just use the native role order for its display order
		/// </summary>
		/// <param name="factType">A fact type. Can be null.</param>
		/// <param name="displayOrder">A custom order representing
		/// the display order for a graphical representation of the fact</param>
		public ActiveFactType(FactType factType, RoleMoveableCollection displayOrder)
		{
			if (factType != null)
			{
				myFactType = factType;
				myDisplayOrder = (displayOrder != null) ? displayOrder : factType.RoleCollection;
			}
			else
			{
				myFactType = null;
				myDisplayOrder = null;
			}
		}
		/// <summary>
		/// Get the current FactType
		/// </summary>
		public FactType FactType
		{
			get
			{
				return myFactType;
			}
		}
		/// <summary>
		/// Get the current display order
		/// </summary>
		public RoleMoveableCollection DisplayOrder
		{
			get
			{
				return myDisplayOrder;
			}
		}
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(ActiveFactType fact1, ActiveFactType fact2)
		{
			return fact1.Equals(fact2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(ActiveFactType fact1, ActiveFactType fact2)
		{
			return !(fact1.Equals(fact2));
		}
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is ActiveFactType) ? Equals( (ActiveFactType)obj) : false;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(ActiveFactType obj)
		{
			bool leftEmpty = IsEmpty;
			bool rightEmpty = obj.IsEmpty;
			if (leftEmpty && rightEmpty)
			{
				return true;
			}
			else if (!leftEmpty && !rightEmpty)
			{
				if (object.ReferenceEquals(myFactType, obj.myFactType))
				{
					if (AreDisplayOrdersEqual(myDisplayOrder, obj.myDisplayOrder))
					{
						return true;
					}
				}
			}
			return false;
		}
		private bool AreDisplayOrdersEqual(RoleMoveableCollection order1, RoleMoveableCollection order2)
		{
			if (object.ReferenceEquals(order1, order2))
			{
				return true;
			}
			bool retVal = false;
			int count = order1.Count;
			if (count != 0 && count == order2.Count)
			{
				retVal = true;
				for (int i = 0; i < count; ++i)
				{
					if (!object.ReferenceEquals(order1[i], order2[i]))
					{
						retVal = false;
						break;
					}
				}
			}
			return retVal;
		}
		private static int RotateRight(int value, int places)
		{
			places = places & 0x1F;
			if (places == 0)
			{
				return value;
			}
			int mask = ~0x7FFFFFF >> (places - 1);
			return ((value >> places) & ~mask) | ((value << (32 - places)) & mask);
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			FactType fact = myFactType;
			int hashCode = 0;
			if (fact != null)
			{
				hashCode = fact.GetHashCode();
				RoleMoveableCollection order = myDisplayOrder;
				int count = order.Count;
				for (int i = 0; i < count; ++i)
				{
					hashCode ^= RotateRight(order[i].GetHashCode(), i);
				}
			}
			return hashCode;
		}
	}
	#endregion // ActiveFactType structure

	public partial class NewReadingEditor : UserControl
	{
		#region ReadingOrder ColumnIndex enum
		private enum ColumnIndex
		{
			ReadingOrder = 0,
			ReadingBranch = 1,
		}
		#endregion // Reading Order ColumnIndex enum

		//Provides a ref to the tree control from nestec objects
		private static CustomVirtualTreeControl TreeControl;

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
		private RoleMoveableCollection myDisplayRoleOrder;
		//private List<ReadingEntry> myReadingList;
		private ImageList myImageList;
		private static ReadingOrderBranch myReadingOrderBranch;
		#endregion // Member Variables

		#region construction
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NewReadingEditor()
		{
			InitializeComponent();

			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[ReadingOrderBranch.COLUMN_COUNT]
			  {
			    new VirtualTreeColumnHeader(" ", 18,false, VirtualTreeColumnHeaderStyles.Default), 
			    new VirtualTreeColumnHeader(" ", 1f, 200, VirtualTreeColumnHeaderStyles.Default)
			  };
			vtrReadings.SetColumnHeaders(headers, false);
			//set permutation to ensure that the readingOrder column is first, then the readingBranch column
			this.vtrReadings.ColumnPermutation = new ColumnPermutation(ReadingOrderBranch.COLUMN_COUNT, new int[] { (int)ColumnIndex.ReadingOrder, (int)ColumnIndex.ReadingBranch }, vtrReadings.RightToLeft == RightToLeft.No);
			this.vtrReadings.Update();
		}

		#endregion

		#region Properties
		/// <summary>
		/// The fact that is being edited in the control, or that needs to be edited.
		/// </summary>
		public ActiveFactType EditingFactType
		{
			get
			{
				return new ActiveFactType(myFact, myDisplayRoleOrder);
			}
			set
			{
				myFact = value.FactType;
				myDisplayRoleOrder = value.DisplayOrder;
				if (myFact != null)
				{
					PopulateControl();
				}
			}
		}

		/// <summary>
		/// The role order we're using to display the current fact.
		/// </summary>
		public RoleMoveableCollection DisplayRoleOrder
		{
			get
			{
				return myDisplayRoleOrder;
			}
		}

		/// <summary>
		/// The reading that is being edited in the control, or that needs to be edited.
		/// </summary>
		public Reading CurrentReading
		{
			get
			{
				ITree tree = vtrReadings.Tree;
				int currentIndex = vtrReadings.CurrentIndex;
				//if (currentIndex >= 0)
				//{
				//    VirtualTreeItemInfo itemInfo = tree.GetItemInfo(currentIndex, vtrReadings.CurrentColumn, false);
				//    int options = 0;
				//    ReadingEntry entry = itemInfo.Branch.GetObject(itemInfo.Row, itemInfo.Column, ObjectStyle.TrackingObject, ref options) as ReadingEntry;
				//    return (entry == null) ? null : entry.Reading;
				//}
				return null;
			}
		}
		/// <summary>
		/// Returns true if a reading is currently in edit mode.
		/// </summary>
		public bool InLabelEdit
		{
			get
			{
				return vtrReadings.InLabelEdit;
			}
		}
		/// <summary>
		/// Returns the active label edit control, or null
		/// </summary>
		public Control LabelEditControl
		{
			get
			{
				return vtrReadings.LabelEditControl;
			}
		}
		/// <summary>
		/// Returns true if the reading pane is active
		/// </summary>
		public bool IsReadingPaneActive
		{
			get
			{
				if (vtrReadings != null)
				{
					ContainerControl sc = this.ActiveControl as ContainerControl;
					if (sc != null)
					{
						return object.ReferenceEquals(vtrReadings, sc.ActiveControl);
					}
				}
				return false;
			}
		}

		#endregion

		#region PopulateControl and helpers

		private void PopulateControl()
		{
			Debug.Assert(myFact != null);
			myReadingOrderBranch = new ReadingOrderBranch(myFact, myDisplayRoleOrder);
			//ITree existingTree = vtrReadings.Tree;
			//if (existingTree != null)
			//{
			//    IBranch branch = myReadingOrderBranch;
			//    existingTree.Root = branch;
			//    int count = branch.VisibleItemCount;
			//    IMultiColumnTree mcTree = (IMultiColumnTree)existingTree;
			//    for (int i = 0; i < count; ++i)
			//    {
			//        mcTree.UpdateCellStyle(branch, i, 1, true);
			//        //branch.OnBranchModification(BranchModificationEventArgs.UpdateCellStyle(branch, i, 1, true));
			//    }
			//}
			//else
			//{
			ReadingVirtualTree rvt = new ReadingVirtualTree(myReadingOrderBranch);
			this.vtrReadings.MultiColumnTree = rvt;
			//}
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
			//TreeNodeCollection nodes = tvwReadingOrder.Nodes;

			//// Find the root node
			//foreach (TreeNode node in nodes)
			//{
			//    ReadingRootTreeNode rootNode = node as ReadingRootTreeNode;
			//    if (rootNode != null && IsMatchingLeadRole(rootNode.LeadRole, readingOrderRoles))
			//    {
			//        // Find the child node
			//        foreach (ReadingTreeNode childNode in rootNode.Nodes)
			//        {
			//            if (IsMatchingReadingOrder(childNode.RoleOrder, readingOrderRoles))
			//            {
			//                // Select to populate the grid
			//                tvwReadingOrder.SelectedNode = childNode;
			//                retval = true;
			//                break;
			//            }
			//        }
			//        break;
			//    }
			//}
			return retval;
		}

		/// <summary>
		/// Select the current reading in the window. The
		/// reading must be the child of the current fact.
		/// </summary>
		/// <param name="reading">Reading</param>
		public void ActivateReading(Reading reading)
		{
			//ReadingOrder order;
			//FactType factType;
			//if (null != (order = reading.ReadingOrder) &&
			//    null != (factType = order.FactType) &&
			//    object.ReferenceEquals(factType, myFact))
			//{
			//    RoleMoveableCollection readingOrderRoles = order.RoleCollection;

			//    if (SelectNode(readingOrderRoles))
			//    {
			//        // Find the item in the grid
			//        if (vtrReadings.SelectObject(null, reading, (int)ObjectStyle.TrackingObject, 0))
			//        {
			//            vtrReadings.BeginLabelEdit();
			//        }
			//    }
			//}
		}

		/// <summary>
		/// Puts the reading that is currently selected in the reading order
		/// into edit mode.
		/// </summary>
		public void EditSelectedReading()
		{
			using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.CommandEditReadingText))
			{
				vtrReadings.BeginLabelEdit();
			}
		}

		/// <summary>
		/// Deletes the reading that is currently selected in the reading order
		/// if the virtual tree is not in edit mode.
		/// </summary>
		public void DeleteSelectedReading()
		{
			//using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.CommandDeleteReadingText))
			//{
			//    if (!vtrReadings.InLabelEdit)
			//    {
			//        Reading reading = myReadingList[vtrReadings.CurrentIndex].Reading;
			//        reading.Remove();
			//        if (t.HasPendingChanges) t.Commit();
			//    }
			//}
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
			//ReadingOrder order;
			//Reading reading;
			//if (null != (order = FactType.FindMatchingReadingOrder(fact))
			//    && null != (reading = order.PrimaryReading)
			//    && object.ReferenceEquals(fact, myFact))
			//{
			//    RoleMoveableCollection readingOrderRoles = order.RoleCollection;
			//    if (SelectNode(readingOrderRoles))
			//    {
			//        if (vtrReadings.SelectObject(null, reading, (int)ObjectStyle.TrackingObject, 0))
			//        {
			//            vtrReadings.BeginLabelEdit();
			//        }
			//    }
			//}
			//else
			//{
			//    RoleMoveableCollection factRoles = fact.RoleCollection;
			//    if (SelectNode(factRoles))
			//    {
			//        if (vtrReadings.SelectObject(null, NewReadingEntry.Singleton, (int)ObjectStyle.TrackingObject, 0))
			//        {
			//            vtrReadings.BeginLabelEdit();
			//        }
			//    }
			//}
		}
		#endregion // Reading activation helper

		#region model events and handlers

		#region Nested event handler attach/detach methods

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

			eventDirectory.ElementEventsBegun.Add(new ElementEventsBegunEventHandler(ElementEventsBegunEvent));
			eventDirectory.ElementEventsEnded.Add(new ElementEventsEndedEventHandler(ElementEventsEndedEvent));
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

			eventDirectory.ElementEventsBegun.Remove(new ElementEventsBegunEventHandler(ElementEventsBegunEvent));
			eventDirectory.ElementEventsEnded.Remove(new ElementEventsEndedEventHandler(ElementEventsEndedEvent));
		}

		#endregion

		#region Reading Event Handlers
		private bool myInEvents;
		private bool myUpdateRequired;
		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			myInEvents = false; // Sanity, should not be needed
			if (myFact != null)
			{
				myInEvents = true;
				myUpdateRequired = false;
				//vtrReadings.Tree.DelayRedraw = true;
			}
		}
		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			if (myInEvents)
			{
				myInEvents = false;
				ITree tree = vtrReadings.Tree;
				bool refresh = myUpdateRequired;
				myUpdateRequired = false;
				if (refresh)
				{
					//tree.Redraw = false;
					PopulateControl();
					//tree.Redraw = true;
				}
				//tree.DelayRedraw = false;
			}
		}

		//handling model events Related to changes in Readings and their
		//connections so the reading editor can accurately reflect the model

		private void ReadingLinkAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			ReadingOrder readingOrder = link.ReadingOrder;
			FactType fact = readingOrder.FactType;

			if (!object.ReferenceEquals(fact, myFact))
			{
				return;
			}
			myUpdateRequired = true;
			//(myReadingOrderBranch as IBranch).LocateObject
			//Reading reading = link.ReadingCollection;
			//ReadingOrderMoveableCollection readingOrderCollection = fact.ReadingOrderCollection;


			//RoleMoveableCollection readingOrderRoles = readingOrder.RoleCollection;
			//if (myCache.DicRoleOrder.ContainsKey(readingOrder.RoleCollection))
			//{
			//}

			//int index = -1;
			//TODO:need to put more work into ordering Readings added to the model.
			//they end up getting put in a different order than they appear when
			//the list is constructed from scratch versus the order when the list
			//is cleared via undo and redo. Might want to look into making
			//the list a sorted list. Only appears when the Readings of more
			//than one ReadingOrder are being shown at the same time.

			////////the all node is selected
			//////if (mySelectedLeadRole == null && mySelectedRoleOrder == null)
			//////{
			//////    index = ord.ReadingCollection.IndexOf(read);
			//////}
			////////leadrole branch selected
			//////else if (mySelectedLeadRole != null && IsMatchingLeadRole(mySelectedLeadRole, ord))
			//////{
			//////    index = ord.ReadingCollection.IndexOf(read);
			//////}

			//////specific order branch selected
			//////else
			//////if (mySelectedRoleOrder != null &&  IsMatchingReadingOrder(mySelectedRoleOrder, ord))
			//////{
			//////    index = ord.ReadingCollection.IndexOf(read);
			//////}

			//test if it already exists so that if the list was built from scratch because
			//of roles being added we don't put the item in the list twice
			//if (index > -1 && (IndexOfReadingEntry(read) < 0))
			//{
			//    myReadingList.Insert(index, new ReadingEntry(read, ord));
			//    myBranch.ItemAdded(index);
			//}
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
			//Reading reading = e.ModelElement as Reading;
			//ReadingOrder ord = reading.ReadingOrder;

			//if (ord == null || !object.ReferenceEquals(ord.FactType, myFact))
			//{
			//  return;
			//}

			//int index = IndexOfReadingEntry(reading);
			//if (index > -1)
			//{
			//  ReadingEntry re = myReadingList[index];
			//  re.InvalidateText();
			//  int column = -1;
			//  Guid attrId = e.MetaAttribute.Id;
			//  if (attrId.Equals(Reading.TextMetaAttributeGuid))
			//  {
			//    column = (int)ColumnIndex.FactReadings;
			//  }
			//  //else if (attrId.Equals(Reading.IsPrimaryMetaAttributeGuid))
			//  //{
			//  //  column = (int)ColumnIndex.IsPrimary;
			//  //}
			//  myBranch.ItemUpdate(index, column);
			//}
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
			//ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			//Role role = link.RoleCollection;
			//FactType roleFact = role.FactType;
			//if (myFact != null && object.ReferenceEquals(myFact, roleFact) && !myBranch.IsAdding)
			//{
			//  ReloadData();
			//}
		}

		private void ReadingOrderHasRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			//ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			//ReadingOrder ord = link.ReadingOrder;
			//if (myFact != null && !myFact.IsRemoved)
			//{
			//  if (myFact.ReadingOrderCollection.Contains(ord))
			//  {
			//    ReloadData();
			//  }
			//}
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
			//bool wasImpacted = SetTextOnTreeNodes(changedRole, tvwReadingOrder.Nodes);
			//if (wasImpacted)
			//{
			//  int numEntries = myReadingList.Count;
			//  for (int i = 0; i < numEntries; ++i)
			//  {
			//    if (myReadingList[i].Contains(changedRole))
			//    {
			//      myBranch.ItemUpdate(i, (int)ColumnIndex.FactReadings);
			//    }
			//  }
			//}
		}

		private void ObjectTypeChangedHelper(ObjectType changedObjectType)
		{
			//bool wasImpacted = SetTextOnTreeNodes(changedObjectType, tvwReadingOrder.Nodes);
			//if (wasImpacted)
			//{
			//  int numEntries = myReadingList.Count;
			//  for (int i = 0; i < numEntries; ++i)
			//  {
			//    if (ReadingEditor.IsParticipant(changedObjectType, myReadingList[i].ReadingOrder))
			//    {
			//      myBranch.ItemUpdate(i, (int)ColumnIndex.FactReadings);
			//    }
			//  }
			//}
		}
		#endregion

		#region FactType Event Handlers
		private void FactTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			if (object.ReferenceEquals(link.FactTypeCollection, myFact))
			{
				ORMDesignerPackage.NewReadingEditorWindow.EditingFactType = ActiveFactType.Empty;
			}
		}
		#endregion // FactType Event Handlers

		#endregion //Reading Event Handlers

		#region Helper methods

		/// <summary>
		/// Tests if any custom nodes that have values based on the changed ObjectType
		/// are in the tree and initiates a text change if they are. Uses recursion
		/// to handle child nodes. It returns true if the tree or one of its children
		/// had to update its text because it was dependent on the object for its value.
		/// </summary>
		private static bool SetTextOnTreeNodes(ObjectType changedObjectType, TreeNodeCollection nodes)
		{
			bool wasImpacted = false;
			//BaseReadingTreeNode node;
			//int numNodes = nodes.Count;
			//for (int i = 0; i < numNodes; ++i)
			//{
			//    node = nodes[i] as BaseReadingTreeNode;
			//    if (node != null)
			//    {
			//        if (node.IsImpactedBy(changedObjectType))
			//        {
			//            wasImpacted = true;
			//            node.SetText();
			//        }
			//    }
			//    wasImpacted = wasImpacted | SetTextOnTreeNodes(changedObjectType, nodes[i].Nodes);
			//}
			return wasImpacted;
		}

		private static bool SetTextOnTreeNodes(Role changedRole, TreeNodeCollection nodes)
		{
			bool wasImpacted = false;
			//BaseReadingTreeNode node;
			//int numNodes = nodes.Count;
			//for (int i = 0; i < numNodes; ++i)
			//{
			//    node = nodes[i] as BaseReadingTreeNode;
			//    if (node != null)
			//    {
			//        if (node.IsImpactedBy(changedRole))
			//        {
			//            wasImpacted = true;
			//            node.SetText();
			//        }
			//    }
			//    wasImpacted = wasImpacted | SetTextOnTreeNodes(changedRole, nodes[i].Nodes);
			//}
			return wasImpacted;
		}

		/// <summary>
		/// Removes any ReadingEntry items from the list that are related
		/// to the indicated ReadingOrder
		/// </summary>
		private void RemoveReadingOrderRelatedEntries(ReadingOrder readingOrder)
		{
			//int initNrEntries = myReadingList.Count;
			//if (initNrEntries > 0)
			//{
			//  int i = initNrEntries - 1;
			//  while (i >= 0)
			//  {
			//    if (object.ReferenceEquals(myReadingList[i].ReadingOrder, readingOrder))
			//    {
			//      myReadingList.RemoveAt(i);
			//      myBranch.ItemRemoved(i);
			//    }
			//    --i;
			//  }
			//}
		}

		/// <summary>
		/// Handles removing the ReadingEntry for the specified Reading
		/// object and handles the branch update as well.
		/// </summary>
		/// <returns>Returns the index of the reading that was removed, -1 if it wasn't found.</returns>
		private int RemoveReadingEntry(Reading reading)
		{
			int index = IndexOfReadingEntry(reading);
			////should be a reading that was part of the currently displayed fact
			//if (index >= 0)
			//{
			//  myReadingList.RemoveAt(index);
			//  myBranch.ItemRemoved(index);
			//}
			return index;
		}

		/// <summary>
		/// Locate the index of the ReadingEntry that represents the specified Reading.
		/// </summary>
		private int IndexOfReadingEntry(Reading reading)
		{
			//int numEntries = myReadingList.Count;
			int index = -1;
			//for (int i = 0; i < numEntries; ++i)
			//{
			//    if ( !myCache.DicReading.ContainsKey(reading) ) // object.ReferenceEquals(myReadingList[i].Reading, reading))
			//    {
			//        index = i;
			//        break;
			//    }
			//}
			return index;
		}
		#endregion

		#region nested class ReadingOrderBranch
		private class ReadingOrderBranch : IBranch, IMultiColumnBranch
		{
			public const int COLUMN_COUNT = 2;
			private BranchModificationEventHandler myModificationEvents;
			private FactType myFact;
			private ReadingOrderKeyedCollection myReadingOrderKeyedCollection;
			private ReadingOrderKeyedCollection myReadingOrderPermutations;
			private RoleMoveableCollection myRoleDisplayOrder;
			private string[] myRoleNames;
			private int myInsertedRow = -1;

			#region Construction
			public ReadingOrderBranch(FactType fact, RoleMoveableCollection roleDisplayOrder)
			{
				myFact = fact;
				ReadingOrderMoveableCollection readingOrders = fact.ReadingOrderCollection;
				int thisBranchCount = readingOrders.Count;
				myReadingOrderKeyedCollection = new ReadingOrderKeyedCollection();
				myRoleDisplayOrder = roleDisplayOrder;
				for (int i = 0; i < thisBranchCount; ++i)
				{
					myReadingOrderKeyedCollection.Add(new ReadingOrderInformation(this, readingOrders[i]));
				}
			}
			#endregion

			#region Branch Properties
			private RoleMoveableCollection DisplayOrder
			{
				get
				{
					return myRoleDisplayOrder;
				}
			}
			#endregion //Branch Properties

			#region IBranch Member Mirror/Implementations

			VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
				if (column == (int)ColumnIndex.ReadingBranch && rowType == RowType.TypeEditorHost)
				{
					if (myReadingOrderPermutations == null)
					{
						ReadingOrderInformation roi;
						int arity = myRoleDisplayOrder.Count;
						// UNDONE: This should all change to using Role[] when BuildPermutations is brought to O(sanity)
						List<Role> tempRoleList = new List<Role>(arity);
						myReadingOrderPermutations = new ReadingOrderKeyedCollection();
						for (int i = 0; i < arity; ++i)
						{
							tempRoleList.Add(myRoleDisplayOrder[i]);
						}
						List<List<Role>> myPerms;myPerms = this.BuildPermutations(tempRoleList);
						int numPerms = myPerms.Count;
						for (int i = 0; i < numPerms; ++i)
						{
							roi =  new ReadingOrderInformation(this, myPerms[i].ToArray());
							myReadingOrderPermutations.Add(roi);
						}
					}
					// UNDONE: Localize
					TypeEditorHost host = TypeEditorHost.Create(new ReadingOrderDescriptor( myFact, "New Reading..."), myReadingOrderPermutations, TypeEditorHostEditControlStyle.TransparentEditRegion);
					host.Flags = VirtualTreeInPlaceControlFlags.DrawItemText | VirtualTreeInPlaceControlFlags.ForwardKeyEvents | VirtualTreeInPlaceControlFlags.SizeToText;

					return new VirtualTreeLabelEditData(host, delegate(VirtualTreeItemInfo itemInfo, Control editControl)
																								{
																									return LabelEditResult.AcceptEdit;
																								});
				}
				else if (column == (int)ColumnIndex.ReadingBranch && rowType == RowType.UnCommitted)
				{
					VirtualTreeLabelEditData retval = VirtualTreeLabelEditData.Default;
					StringBuilder sb = new StringBuilder();
					int numRoles = myRoleNames.Length;
					for (int i = 0; i < numRoles; ++i)
					{
						sb.Append("{");
						sb.Append(i);
						sb.Append("}");
					}
					retval.AlternateText = sb.ToString();
					return retval;
				}
				else
				{
					return VirtualTreeLabelEditData.Invalid;
				}
			}

			LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				if (myReadingOrderKeyedCollection.GetRowType(row) == RowType.UnCommitted)
				{
					Reading theNewReading;
					try
					{
						myInsertedRow = row;
						using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
						{
							ReadingOrder theOrder = FactType.GetReadingOrder(myFact, myReadingOrderKeyedCollection[row].RoleOrder as IList<Role>);
							Debug.Assert(theOrder != null, "A ReadingOrder should have been found or created.");
							theNewReading = Reading.CreateReading(theOrder.Store);
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
				return LabelEditResult.AcceptEdit;
			}

			static BranchFeatures Features
			{
				get
				{
					return BranchFeatures.ExplicitLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.DelayedLabelEdits | BranchFeatures.InsertsAndDeletes | BranchFeatures.ComplexColumns;
				}
			}

			static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}

			VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				if (column == (int)ColumnIndex.ReadingBranch)
				{
					retval.ForeColor = (rowType == RowType.UnCommitted) ? Color.Gray : retval.ForeColor;
				}
				else
				{
					if (rowType != RowType.TypeEditorHost)
					{
						retval.Image = 0;
						retval.SelectedImage = 0;   //MUST set both .Image and .SelectedImage or exception will occur
					}
				}
				return retval;
			}

			object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.SubItemRootBranch:
						if (myReadingOrderKeyedCollection.GetRowType(row) == RowType.Committed)
						{
							return myReadingOrderKeyedCollection[row].Branch;
						}
						else
						{
							return null;
						}
					case ObjectStyle.TrackingObject:
						return null;
					default:
						return null;
				}
			}

			string GetText(int row, int column)
			{
				string retVal;
				if (column == (int)ColumnIndex.ReadingBranch)
				{
					RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
					switch (rowType)
					{
						case RowType.TypeEditorHost:
							retVal = "New Reading...";
							break;
						case RowType.UnCommitted:
							retVal = myReadingOrderKeyedCollection[row].Text;
							break;
						default:
							retVal = null;
							break;
					}
				}
				else
				{
					retVal = null;
				}
				return retVal;
			}

			string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (column == (int)ColumnIndex.ReadingOrder)
				{
					string retval;
					RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
					switch (rowType)
					{
						case RowType.Committed:
							retval = myReadingOrderKeyedCollection[row].ToString();
							break;
						case RowType.UnCommitted:
							retval = myReadingOrderKeyedCollection[row].Text;
							break;
						case RowType.TypeEditorHost:
							retval = "UNDONE: Localize Choose a New Reading Order from the Drop Down List";
							break;
						default:
							retval = null;
							break;
					}
					return retval;
				}
				else
				{
					return null;
				}
			}

			static bool IsExpandable(int row, int column)
			{
				return false;
			}

			LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				//RoleList roleListObj = obj as RoleList;				//UNDONE: use the RMC instead
				//string roleListSring = roleListObj.ToString();
				switch (style)
				{
					case ObjectStyle.TrackingObject:
						//int listLoc;
						//if (-1 != (listLoc = myNewReadingOrders.IndexOf(roleListObj))) //get list location for selected reading order 
						//{
						//    int branchLoc = myHelper.TotalCommitted + listLoc;
						//    return new LocateObjectData(branchLoc, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.ThisLevel);
						//}

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
					return myReadingOrderKeyedCollection.Count + 1;
				}
			}
			#endregion

			#region IBranch Interface Members
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
				return SubItemCellStyles.Complex;
			}
			int GetJaggedColumnCount(int row)
			{
				return COLUMN_COUNT;
			}
			#endregion

			#region IMultiColumnBranch Interface Members
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

			#region Order Branch Custom Methods
			private string[] GetRoleNames()
			{
				string[] retVal = myRoleNames;
				if (retVal == null)
				{
					RoleMoveableCollection factRoles = myFact.RoleCollection;
					ObjectType rolePlayer;
					int factArity = factRoles.Count;
					if (factArity == 1)
					{
						rolePlayer = factRoles[0].RolePlayer;
						myRoleNames = new string[] { (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText };
					}
					else
					{
						RoleMoveableCollection roleDisplayOrder = myRoleDisplayOrder;
						ObjectType[] rolePlayers = new ObjectType[factArity];
						for (int i = 0; i < factArity; ++i)
						{
							rolePlayers[i] = roleDisplayOrder[i].RolePlayer;
						}
						myRoleNames = new string[factArity];
						for (int i = 0; i < factArity; ++i)
						{
							rolePlayer = roleDisplayOrder[i].RolePlayer;

							int subscript = 0;
							bool useSubscript = false;
							int j = 0;
							for (; j < i; ++j)
							{
								if (object.ReferenceEquals(rolePlayer, rolePlayers[j]))
								{
									useSubscript = true;
									++subscript;
								}
							}
							for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
							{
								if (object.ReferenceEquals(rolePlayer, rolePlayers[j]))
								{
									useSubscript = true;
								}
							}
							if (useSubscript)
							{
								myRoleNames[i] = string.Format(CultureInfo.InvariantCulture, "{0}({1})", (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText, subscript + 1); //UNDONE: localize
							}
							else
							{
								myRoleNames[i] = (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText;
							}
						}
					}
					retVal = myRoleNames;
				}
				return retVal;
			}
			#endregion //Order Branch Custom Methods

			#region Branch update methods
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

			private int ShowNewOrder(IList order)
			{
				int retVal;
				ReadingOrderInformation info = new ReadingOrderInformation(this, order as Role[]);
				if (!myReadingOrderKeyedCollection.Contains(order))
				{
					myReadingOrderKeyedCollection.Add(info);
					myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, this.VisibleItemCount - 2, 1));
				}
				retVal = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[info.RoleOrder]);
				return retVal;
			}
			#endregion

			#region Nested KeyedCollection class

			private enum RowType
			{
				Committed = 0,
				UnCommitted = 1,
				TypeEditorHost = 3
			}

			private class ReadingOrderKeyedCollection : KeyedCollection<IList, ReadingOrderInformation>
			{
				public ReadingOrderKeyedCollection() : base(ListEqualityComparer.Comparer, 16) { }
				protected override IList GetKeyForItem(ReadingOrderInformation item)
				{
					return item.RoleOrder;
				}
				/// <summary>
				/// Returns a RowType for the row requested, defaults to RowType.Committed
				/// </summary>
				/// <param name="row">zero based Row index</param>
				/// <returns>RowType</returns>
				public RowType GetRowType(int row)
				{
					if (row >= Count)
					{
						return RowType.TypeEditorHost;
					}
					return (this[row].ReadingOrder != null) ? RowType.Committed : RowType.UnCommitted;
				}
				#region Nested Class to implement IEqualityComparer for an IList
				private class ListEqualityComparer : IEqualityComparer<IList>
				{
					private ListEqualityComparer() { }
					public static readonly ListEqualityComparer Comparer = new ListEqualityComparer();
					public bool Equals(IList x, IList y)
					{
						int xCount = x.Count;
						if (xCount != y.Count)
						{
							return false;
						}
						for (int i = 0; i < xCount; ++i)
						{
							if (!object.ReferenceEquals(x[i], y[i]))
							{
								return false;
							}
						}
						return true;
					}

					private static int RotateRight(int value, int places)
					{
						places = places & 0x1F;
						if (places == 0)
						{
							return value;
						}
						int mask = ~0x7FFFFFF >> (places - 1);
						return ((value >> places) & ~mask) | ((value << (32 - places)) & mask);
					}

					public int GetHashCode(IList obj)
					{
						int objCount = obj.Count;
						if (objCount == 0)
						{
							return 0;
						}

						int hashCode = obj[0].GetHashCode();
						for (int i = 1; i < objCount; ++i)
						{
							hashCode ^= RotateRight(obj[i].GetHashCode(), i);
						}
						return hashCode;
					}
				}
				#endregion // Nested Class to implement IEqualityComparer for an IList
			}
			#endregion nested KeyedCollection class

			#region Nested ReadingInformation class used with the KeyedCollection class
			private class ReadingOrderInformation
			{
				private ReadingOrderBranch myParentBranch;
				private IList myRoleOrder;
				private ReadingOrder myReadingOrder;
				private string[] myOrderedReplacementFields;
				private string myText;
				private ReadingBranch myReadingBranch;

				public ReadingOrderInformation(ReadingOrderBranch parentBranch, Role[] roleOrder)
				{
					myParentBranch = parentBranch;
					myRoleOrder = roleOrder;
				}

				public ReadingOrderInformation(ReadingOrderBranch parentBranch, ReadingOrder readingOrder)
				{
					myParentBranch = parentBranch;
					myReadingOrder = readingOrder;
					myRoleOrder = readingOrder.RoleCollection;
				}

				public ReadingOrder ReadingOrder
				{
					get
					{
						return myReadingOrder;
					}
				}

				public ReadingBranch Branch
				{
					get
					{
						ReadingBranch retval = myReadingBranch;
						if (retval == null)
						{
							myReadingBranch = new ReadingBranch(myReadingOrder, this);
							retval = myReadingBranch;
						}
						return retval;
					}

				}

				public IList RoleOrder
				{
					get
					{
						return myRoleOrder;
					}
				}

				public string Text
				{
					get
					{
						string retVal = myText;
						if (myText == null)
						{
							retVal = string.Join(", ", OrderedReplacementFields); //UNDONE: Localize separator
							myText = retVal;
						}
						return retVal;
					}
				}

				public string[] OrderedReplacementFields
				{
					get
					{
						string[] retVal = myOrderedReplacementFields;
						if (retVal == null)
						{
							string[] roleNames = myParentBranch.GetRoleNames();
							RoleMoveableCollection displayOrder = myParentBranch.DisplayOrder;
							int arity = roleNames.Length;
							Debug.Assert(arity == displayOrder.Count);
							IList roles = myRoleOrder;
							switch (arity)
							{
								case 1:
									retVal = new string[]{roleNames[0]};
									break;
								case 2:
									retVal = new string[]{
										roleNames[displayOrder.IndexOf((Role)roles[0])],
										roleNames[displayOrder.IndexOf((Role)roles[1])]};
									break;
								case 3:
									retVal = new string[]{
										roleNames[displayOrder.IndexOf((Role)roles[0])],
										roleNames[displayOrder.IndexOf((Role)roles[1])],
										roleNames[displayOrder.IndexOf((Role)roles[2])]};
									break;
								default:
									retVal = new string[arity];
									for (int i = 0; i < arity; ++i)
									{
										retVal[i] = roleNames[displayOrder.IndexOf((Role)roles[i])];
									}
									break;
							}
							myOrderedReplacementFields = retVal;
						}
						return retVal;
					}
				}
				public override string ToString()
				{
					return Text;
				}
			}
			#endregion //Nested ReadingInformation class used with the KeyedCollection class

			#region  Nested Class Branch Property Descriptor for New Reading Drop Down

			private class ReadingOrderDescriptor : PropertyDescriptor
			{
				FactType myFact;

				private class ReadingOrderEditor : ElementPicker
				{
					public ReadingOrderEditor(){}

					protected override object TranslateFromDisplayObject(int newIndex, object newObject)
					{
						return newObject as ReadingOrderInformation;
					}
					protected override object TranslateToDisplayObject(object initialObject, IList contentList)
					{
						ReadingOrderKeyedCollection key = (contentList as ReadingOrderKeyedCollection);
						return key.ToString();
					}
					protected override System.Collections.IList GetContentList(ITypeDescriptorContext context, object value)
					{
					    return context.Instance as IList;
					}
				}
				public ReadingOrderDescriptor(FactType fact, string name)
					: base(name, null)
				{
					myFact = fact;
				}
				public override object GetEditor(Type editorBaseType)
				{
					Debug.Assert(editorBaseType == typeof(UITypeEditor));
					return new ReadingOrderEditor();
				}
				public override bool CanResetValue(object component)
				{
					return false;
				}
				public override Type ComponentType
				{
					get { return typeof(ReadingOrderInformation); }
				}
				public override object GetValue(object component)
				{
					return this.Name;  
				}
				public override bool IsReadOnly
				{
					get { return true; }
				}
				public override Type PropertyType
				{
					get { return typeof(ReadingOrderInformation); }
				}
				public override void ResetValue(object component)
				{
				}
				public override void SetValue(object component, object value)
				{
					ReadingOrderKeyedCollection collection = myReadingOrderBranch.myReadingOrderKeyedCollection;
					ReadingOrderInformation info = value as ReadingOrderInformation;
					int branchLocation = -1;
					if (info != null && collection.Contains(info.RoleOrder))
					{
						branchLocation = collection.IndexOf(collection[info.RoleOrder]);
					}

					if (branchLocation >= 0 && collection[branchLocation].ReadingOrder != null)
					{
						collection[branchLocation].Branch.ShowNewRow();
						this.BeginNewLabelEdit(branchLocation, collection);
					}
					else
					{
						branchLocation = myReadingOrderBranch.ShowNewOrder(info.RoleOrder);
						this.BeginNewLabelEdit(branchLocation, collection);
					}
				}

				private void BeginNewLabelEdit(int editLocation, ReadingOrderKeyedCollection collection)
				{
					int offset = 0; 
					for (int i = 0; i <= editLocation; i++)
					{
						offset += (collection[i].ReadingOrder !=null) ? collection[i].Branch.RowCount : 1;
					}
					NewReadingEditor.TreeControl.CurrentIndex = offset - 1;
					NewReadingEditor.TreeControl.CurrentColumn = (int)ColumnIndex.ReadingBranch;
					NewReadingEditor.TreeControl.BeginLabelEdit();
				}

				public override bool ShouldSerializeValue(object component)
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}
			#endregion

			#region Nested method to generate role permutations
			public List<List<Role>> BuildPermutations(List<Role> roleList)
			{
				// UNDONE: This is absolutely nuts. It should return Role[][] and allocate the arrays once
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
			#endregion //Nested Static Class to generate role permutations

			#region Nested class ReadingBranch
			private class ReadingBranch : IBranch
			{
				/// <summary>
				/// Returns number of reading orders currently in the branch including new uncommitted readings
				/// </summary>
				public int RowCount
				{
					get
					{
						return this.VisibleItemCount;
					}
				}

				public const int COLUMN_COUNT = 1;
				private FactType myFact;
				private ReadingMoveableCollection myReadingMC;
				private BranchModificationEventHandler myModificationEvents;
				private IList<string> myReadingDisplayText;
				private ReadingOrder myReadingOrder;
				private ReadingOrderInformation myReadingInformation;
				private bool showNewRow = false; 
				int myInsertedRow = -1;

				#region Construction

				public ReadingBranch(ReadingOrder order, ReadingOrderInformation orderInformation)
				{
					myReadingOrder = order;
					myReadingMC = myReadingOrder.ReadingCollection;
					myFact = myReadingMC[0].ReadingOrder.FactType;
					myReadingInformation = orderInformation;
					int numReadings = myReadingMC.Count;
					RoleMoveableCollection tempRoleCollection = myReadingOrder.RoleCollection;
					myReadingDisplayText = new List<string>();
					for (int i = 0; i < numReadings; ++i)
					{
						myReadingDisplayText.Add(FactType.PopulatePredicateText(myReadingMC[i], tempRoleCollection, myReadingInformation.OrderedReplacementFields));
					}
				}

				#endregion

				#region IBranch Member Mirror/Implementations

				VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
				{
					VirtualTreeLabelEditData retval = VirtualTreeLabelEditData.Default;
					if (row >= myReadingMC.Count)
					{
						StringBuilder sb = new StringBuilder();
						int numRoles = myReadingInformation.OrderedReplacementFields.Length;
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
						retval.AlternateText = myReadingMC[row].Text;
					}
					return retval;
				}

				LabelEditResult CommitLabelEdit(int row, int column, string newText)
				{
					Reading theReading;
					if (row >= myReadingMC.Count)
					{
					    try
					    {
					        myInsertedRow = row;
					        using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
					        {
								Debug.Assert(myReadingOrder != null, "A ReadingOrder should have been found or created.");
								Reading theNewReading = Reading.CreateReading(myReadingOrder.Store);
								ReadingMoveableCollection readings = myReadingOrder.ReadingCollection;
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
					else
					{
						theReading = myReadingMC[row];
						using (Transaction t = theReading.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorChangePrimaryReadingText))
						{
							theReading.Text = newText;
							t.Commit();
						}
					}
					return LabelEditResult.AcceptEdit;
				}

				static BranchFeatures Features
				{
					get
					{
						return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.InsertsAndDeletes;
					}
				}

				static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
				{
					return VirtualTreeAccessibilityData.Empty;
				}

				VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
					retval.ForeColor = (row == this.VisibleItemCount -1 && this.showNewRow  ) ? Color.Gray : retval.ForeColor;
					return retval;
				}

				object GetObject(int row, int column, ObjectStyle style, ref int options)
				{
					return null;
				}

				string GetText(int row, int column)
				{
					if (row <= this.VisibleItemCount)
					{
						return myReadingDisplayText[row];
					}
					else
					{
						return null;
					}
				}

				static string GetTipText(int row, int column, ToolTipType tipType)
				{
					return null;
				}

				static bool IsExpandable(int row, int column)
				{
					return false;
				}

				LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
				{
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
					return StateRefreshChanges.None;
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
						return myReadingDisplayText.Count;
					}
				}

				#endregion

				#region IBranch Interface Members
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
				/// Displays the new row for adding a reading to the reading order
				/// </summary>
				public void ShowNewRow()
				{
					if (!this.showNewRow)
					{
						this.showNewRow = true;
						this.myReadingDisplayText.Add(myReadingInformation.Text);
						myModificationEvents(this, BranchModificationEventArgs.InsertItems(this,  -1 , 1));
					}
				}

				///// <summary>
				///// Triggers the events notifying the tree that an item in the branch has been updated.
				///// </summary>
				//public void ItemUpdate(int row, int column)
				//{
				//    if (myModificationEvents != null)
				//    {
				//        myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.VisibleElements, this, row, column, 1)));
				//    }
				//}

				///// <summary>
				///// Tell the branch to update it contents because an item has been added.
				///// </summary>
				///// <param name="row">zero based index of where the new item was placed.</param>
				//public void ItemAdded(int row)
				//{
				//    //if (myModificationEvents != null)
				//    //{
				//    //  if (myInsertedRow > -1)
				//    //  {
				//    //    myModificationEvents(this, BranchModificationEventArgs.MoveItem(this, myInsertedRow, row));
				//    //    row = this.VisibleItemCount - 1; // Insert at the new row location
				//    //  }
				//    //}
				//}

				///// <summary>
				///// Triggers notification that an item has been removed from the branch.
				///// </summary>
				//public void ItemRemoved(int row)
				//{
				//    if (myModificationEvents != null)
				//    {
				//        myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, row, 1));
				//    }
				//}
				#endregion
			}
			#endregion
		}
		#endregion Nested ReadingOrderBranch class

		#region nested class ReadingVirtualTree
		private class ReadingVirtualTree : MultiColumnTree, IMultiColumnTree
		{
			public ReadingVirtualTree(IBranch root)
				: base(ReadingOrderBranch.COLUMN_COUNT)
			{
				Debug.Assert(root != null);
				this.Root = root;

			}

		}
		#endregion

		#region nested class CustomVirtualTreeControl
		/// <summary>
		/// This custom class is needed to override the height of column headers to provide 
		/// the Appearance of NO Headers when column permutation is used. 
		/// If column headers are not used when column permutation is used an "index out of range exception" 
		/// is fired when the mouse travels over the region occupied by the scrollbar if no scrollbar exists.
		/// </summary>
		public class CustomVirtualTreeControl : VirtualTreeControl
		{
			/// <summary>
			/// Needed for creating a header to prevent the vertical scrollbar from thowing an exception
			/// </summary>
			/// <returns></returns>
			protected override VirtualTreeHeaderControl CreateHeaderControl()
			{
				return new ZeroHeightHeader(this);
			}
			private class ZeroHeightHeader : VirtualTreeHeaderControl
			{
				public ZeroHeightHeader(VirtualTreeControl associatedControl) : base(associatedControl) { }
				public override int HeaderHeight
				{
					get
					{
						return 0;
					}
				}
			}
		}
		#endregion
	}
}

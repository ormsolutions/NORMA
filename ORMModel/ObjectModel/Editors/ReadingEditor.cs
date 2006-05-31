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
using System.ComponentModel.Design;
using System.Drawing.Design;


namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	/// <summary>
	/// Valid Commands for context menu
	/// </summary>
	[Flags]
	public enum ReadingEditorCommands
	{
		/// <summary>
		/// Commands not set
		/// </summary>
		None = 0,
		/// <summary>
		///
		/// </summary>
		AddReading = 1,
		/// <summary>
		/// 
		/// </summary>
		AddReadingOrder = 2,
		/// <summary>
		/// 
		/// </summary>
		DeleteReading = 4,
		/// <summary>
		/// 
		/// </summary>
		PromoteReading = 8,
		/// <summary>
		///
		/// </summary>
		DemoteReading = 0x10,
		/// <summary>
		/// 
		/// </summary>
		PromoteReadingOrder = 0x20,
		/// <summary>
		/// 
		/// </summary>
		DemoteReadingOrder = 0x40
	}

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
		private RoleBaseMoveableCollection myDisplayOrder;
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
		public ActiveFactType(FactType factType, RoleBaseMoveableCollection displayOrder)
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
		public RoleBaseMoveableCollection DisplayOrder
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
		private bool AreDisplayOrdersEqual(RoleBaseMoveableCollection order1, RoleBaseMoveableCollection order2)
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
				RoleBaseMoveableCollection order = myDisplayOrder;
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

	public partial class ReadingEditor : UserControl
	{
		#region Enums
		private enum ColumnIndex
		{
			ReadingOrder = 0,
			ReadingBranch = 1,
		}
		private enum RowType
		{
			Committed = 0,
			UnCommitted = 1,
			TypeEditorHost = 3,
			None	=4
		}
		#endregion // Enums
		
		#region Static Methods
		/// <summary>
		/// Returns the latest instance of the editor
		/// </summary>
		public static ReadingEditor Instance
		{
			get
			{
				return ReadingEditor.instance;
			}
		}
		/// <summary>
		/// Tests if the ObjectType is the RolePlayer for any of Roles
		/// </summary>
		public static bool IsParticipant(ObjectType objectType, RoleBase[] roleOrder)
		{
			int numRoles = roleOrder.Length;
			for (int i = 0; i < numRoles; ++i)
			{
				if (object.ReferenceEquals(objectType, roleOrder[i].Role.RolePlayer))
				{
					return true;
				}			
			}
			return false;
		}
		/// <summary>
		/// Tests if the ObjectType is the RolePlayer of the Role
		/// </summary>
		public static bool IsParticipant(ObjectType objectType, RoleBase leadRole)
		{
			return object.ReferenceEquals(objectType, leadRole.Role.RolePlayer);
		}
		/// <summary>
		/// Tests if the ObjectType is the RolePlayer of any RoleBase
		/// contained in the ReadingOrder
		/// </summary>
		public static bool IsParticipant(ObjectType objectType, ReadingOrder readingOrder)
		{
			RoleBaseMoveableCollection roles = readingOrder.RoleCollection;
			foreach (RoleBase role in roles)
			{
				if (object.ReferenceEquals(role.Role.RolePlayer, objectType))
				{
					return true;
				}
			}
			return false;
		}
		#endregion // Static Methods

		#region Static Variables
		/// <summary>
		/// Provides a ref to the ReadingOrderBranch from nested objects
		/// </summary>
		private static ReadingOrderBranch myReadingOrderBranch;
		/// <summary>
		/// Provides a ref to the tree control from nested objects
		/// </summary>
		private static CustomVirtualTreeControl TreeControl;
		/// <summary>
		/// Provides a ref to the Reading Editorl from nested objects
		/// </summary>
		private static ReadingEditor instance;
		#endregion // Static Variables

		#region Member Variables
		private FactType myFact;
		private RoleBaseMoveableCollection myDisplayRoleOrder;
		private ImageList myImageList;
		private ReadingEditorCommands myVisibleCommands;
		private bool myInEvents;
		#endregion // Member Variables

		#region Construction
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReadingEditor()
		{
			InitializeComponent();

			ReadingEditor.instance = this;
			ReadingEditor.TreeControl = this.vtrReadings;

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
		#endregion //construction

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
		public RoleBaseMoveableCollection DisplayRoleOrder
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

		#endregion //Properties

		#region PopulateControl and helpers
		private void PopulateControl()
		{
			Debug.Assert(myFact != null);
			myReadingOrderBranch = new ReadingOrderBranch(myFact, myDisplayRoleOrder);
			ReadingVirtualTree rvt = new ReadingVirtualTree(myReadingOrderBranch);

			VirtualTreeControl control = this.vtrReadings;

			ITree oldTree = vtrReadings.MultiColumnTree as ITree;
			// Turn off all event handlers for old branches whenever we repopulate
			bool turnedDelayRedrawOff = false;
			if (oldTree != null)
			{
				oldTree.Root = null;
				if (myInEvents)
				{
					turnedDelayRedrawOff = true;
					this.vtrReadings.Tree.DelayRedraw = false;
				}
			}
			this.vtrReadings.MultiColumnTree = rvt;
			if (turnedDelayRedrawOff)
			{
				this.vtrReadings.Tree.DelayRedraw = true;
			}

		}
		#endregion //PopulateControl and helpers

		#region Reading activation helper
		/// <summary>
		/// Select the current reading in the window. The
		/// reading must be the child of the current fact.
		/// </summary>
		/// <param name="reading">Reading</param>
		public void ActivateReading(Reading reading) 
		{
			ReadingOrder order;
			FactType factType;
			if (null != (order = reading.ReadingOrder) && null != (factType = order.FactType) && object.ReferenceEquals(factType, myFact))
			{
				myReadingOrderBranch.EditReading(reading);
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
			if (object.ReferenceEquals(fact, myFact))
			{
				RoleBaseMoveableCollection collection = myFact.RoleCollection;
				myReadingOrderBranch.EditReadingOrder(collection);
			}
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
		/// </summary>
		public void DeleteSelectedReading()
		{
			myReadingOrderBranch.RemoveSelectedItem();
		}
		#endregion // Reading activation helper

		#region Tree Context Menu Methods
		private void OnContextMenuInvoked(object sender, ContextMenuEventArgs e)
		{
			ORMReadingEditorToolWindow.TheMenuService.ShowContextMenu(ORMDesignerDocView.ORMDesignerCommandIds.ReadingEditorContextMenu, e.X, e.Y);
		}
		/// <summary>
		/// Event for selection changed
		/// </summary>
		/// <param name="sender">The VirtualTreeControl</param>
		/// <param name="e">EventArgs</param>
		private void OnTreeControlSelectionChanged(object sender, EventArgs e)
		{		
			this.UpdateContextMenuItems();
		}
		/// <summary>
		/// Check the current status of the requested command. 
		/// </summary>
		/// <param name="sender">MenuCommand</param>
		/// <param name="commandFlag">The command to check for enabled</param>
		public static void OnStatusCommand(object sender, ReadingEditorCommands commandFlag)
		{
			MenuCommand command = sender as MenuCommand;
			Debug.Assert(command != null);
			command.Enabled = command.Visible = 0 != (commandFlag & ReadingEditor.instance.myVisibleCommands);
		}
		/// <summary>
		/// Call the Drop Down list of Reading Orders to Select a new reading entry
		/// </summary>
		public void AddReading()
		{
			myReadingOrderBranch.AddNewReading();
		}
		/// <summary>
		/// Call the Drop Down list of Reading Orders to Select a new readingOrder entry
		/// </summary>
		public void AddReadingOrder()
		{
			myReadingOrderBranch.AddNewReadingOrder();
		}
		/// <summary>
		/// Calls RemoveSelectedItem on the ReadingOrder Branch
		/// </summary>
		public void OnMenuDeleteReading()
		{
			myReadingOrderBranch.RemoveSelectedItem();
		}
		/// <summary>
		/// Moves the Reading up within the ReadingOrder Collection
		/// </summary>
		public void PromoteReading()
		{
			myReadingOrderBranch.PromoteSelectedReading();
		}
		/// <summary>
		/// Moves the Reading down within the ReadingOrder Collection
		/// </summary>
		public void DemoteReading()
		{
			myReadingOrderBranch.DemoteSelectedReading();
		}
		/// <summary>
		/// Moves the ReadingOrder up within the ReadingOrder Collection
		/// </summary>
		public void PromoteReadingOrder()
		{
			myReadingOrderBranch.PromoteSelectedReadingOrder();
		}
		/// <summary>
		/// Moves the ReadingOrder down within the ReadingOrder Collection
		/// </summary>
		public void DemoteReadingOrder()
		{
			myReadingOrderBranch.DemoteSelectedReadingOrder();
		}
		private void UpdateContextMenuItems()
		{
			myVisibleCommands = myReadingOrderBranch.SupportedSelectionCommands();
		}
		#endregion  //Tree Context Menu Methods

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

			//Track FactType RoleOrder changes
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(FactTypeHasRoleAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeHasRoleRemovedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasFactType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(FactTypeRemovedEvent));

			//Track Order Change
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasReadingOrder.MetaRelationshipGuid);
			eventDirectory.RolePlayerOrderChanged.Add(classInfo, new RolePlayerOrderChangedEventHandler(ReadingOrderPositionChangedHandler));
			classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);
			eventDirectory.RolePlayerOrderChanged.Add(classInfo, new RolePlayerOrderChangedEventHandler(ReadingPositionChangedHandler));
			
			//Track Currently Executing Events
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

			//Track FactType RoleOrder changes
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasRole.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(FactTypeHasRoleAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeHasRoleRemovedEvent));

			// Track fact type removal
			classInfo = dataDirectory.FindMetaRelationship(ModelHasFactType.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(FactTypeRemovedEvent));

			// Track Order Change	
			classInfo = dataDirectory.FindMetaRelationship(FactTypeHasReadingOrder.MetaRelationshipGuid);
			eventDirectory.RolePlayerOrderChanged.Remove(classInfo, new RolePlayerOrderChangedEventHandler(ReadingOrderPositionChangedHandler));
			classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);
			eventDirectory.RolePlayerOrderChanged.Remove(classInfo, new RolePlayerOrderChangedEventHandler(ReadingPositionChangedHandler));

			//Track Currently Executing Events
			eventDirectory.ElementEventsBegun.Remove(new ElementEventsBegunEventHandler(ElementEventsBegunEvent));
			eventDirectory.ElementEventsEnded.Remove(new ElementEventsEndedEventHandler(ElementEventsEndedEvent));
		}
		#endregion //Nested event handler attach/detach methods

		#region Reading Event Handlers
		//handling model events Related to changes in Readings and their
		//connections so the reading editor can accurately reflect the model
		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			myInEvents = false; // Sanity, should not be needed
			if (myFact != null)
			{
				myInEvents = true;
				this.vtrReadings.Tree.DelayRedraw = true;
			}
		}
		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			if (myInEvents)
			{
				myInEvents = false;
				this.vtrReadings.Tree.DelayRedraw = false;
			}
		}
		private void ReadingLinkAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			ReadingOrder readingOrder = link.ReadingOrder;
			FactType fact = readingOrder.FactType;
			if (object.ReferenceEquals(fact, myFact)) //maybe ignore if reading order Branch does not exist as readingorderlink will follow
			{
				myReadingOrderBranch.ReadingAdded(link.ReadingCollection);
			}
			this.UpdateContextMenuItems();
		}
		private void ReadingLinkRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			ReadingOrder order = link.ReadingOrder;
			// Handled all at once by ReadingOrderLinkRemovedEvent if all are gone.
			if (!order.IsRemoved)
			{
				if (object.ReferenceEquals(order.FactType, myFact))
				{
					ReadingEditor.myReadingOrderBranch.ReadingRemoved(link.ReadingCollection);
				}
			}
		}
		private void ReadingAttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			Reading reading = e.ModelElement as Reading;
			ReadingOrder order = reading.ReadingOrder;
			Guid attributeId = e.MetaAttribute.Id;
			if (attributeId == Reading.TextMetaAttributeGuid &&
				!reading.IsRemoved &&
				null != (order = reading.ReadingOrder) &&
				object.ReferenceEquals(order.FactType, myFact))
			{
				myReadingOrderBranch.UpdateReading(reading);
			}
		}
		private void ReadingOrderPositionChangedHandler(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			myReadingOrderBranch.ReadingOrderLocationUpdate(e.CounterpartRolePlayer as ReadingOrder);
			this.UpdateContextMenuItems();
		}
		private void ReadingPositionChangedHandler(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			myReadingOrderBranch.ReadingLocationUpdate(e.CounterpartRolePlayer as Reading);
			this.UpdateContextMenuItems();
		}
		#endregion //Reading Event Handlers

		#region ReadingOrder Event Handlers
		//handle model events related to the ReadingOrder or its Roles being removed in order to
		//keep the editor window in sync with what is in the model.
		private void ReadingOrderLinkRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
			FactType factType = link.FactType;

			if (object.ReferenceEquals(factType, myFact) && !factType.IsRemoved)
			{
				myReadingOrderBranch.ReadingOrderRemoved(link.ReadingOrderCollection);
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
			//UNDONE: Implement
			//ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			//RoleBase role = link.PlayedRoleCollection;
			//ObjectTypeChangedHelper(role);
		}
		private void ObjectTypePlaysRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			//UNDONE: Implement
			//ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			//RoleBase role = link.PlayedRoleCollection;
			//ObjectTypeChangedHelper(role);
		}
		private void ObjectTypeAttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			//UNDONE: Implement
			//Guid attrGuid = e.MetaAttribute.Id;
			//if (attrGuid.Equals(ObjectType.NameMetaAttributeGuid) && EditingFactType != null)
			//{
			//    ObjectType objectType = e.ModelElement as ObjectType;
			//    Debug.Assert(objectType != null);
			//    ObjectTypeChangedHelper(objectType);
			//}
		}
		private void ObjectTypeChangedHelper(RoleBase changedRole)
		{
			//UNDONE: Implement
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
		private void FactTypeHasRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myFact != null && object.ReferenceEquals(myFact, (e.ModelElement as FactTypeHasRole).FactType))
			{
				this.PopulateControl();
			}
		}
		private void FactTypeHasRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			if (myFact != null && object.ReferenceEquals(myFact, (e.ModelElement as FactTypeHasRole).FactType))
			{
				this.PopulateControl();
			}
		}
		private void FactTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			if (object.ReferenceEquals(link.FactTypeCollection, myFact))
			{
				ORMDesignerPackage.ReadingEditorWindow.EditingFactType = ActiveFactType.Empty;
			}
		}
		#endregion // FactType Event Handlers
		#endregion //model events and handlers

		#region nested class ReadingOrderBranch
		private class ReadingOrderBranch : IBranch, IMultiColumnBranch
		{
			public const int COLUMN_COUNT = 2;
			private BranchModificationEventHandler myModificationEvents;
			private FactType myFact;
			private ReadingOrderKeyedCollection myReadingOrderKeyedCollection;
			private ReadingOrderKeyedCollection myReadingOrderPermutations;
			private RoleBaseMoveableCollection myRoleDisplayOrder;
			private string[] myRoleNames;
			private int myInsertedRow = -1;

			#region Construction
			public ReadingOrderBranch(FactType fact, RoleBaseMoveableCollection roleDisplayOrder)
			{
				myFact = fact;
				myReadingOrderKeyedCollection = new ReadingOrderKeyedCollection();
				myRoleDisplayOrder = roleDisplayOrder;
				this.PopulateReadingOrderInfo(-1); //Populate for all readings
			}
			#endregion

			#region Branch Properties
			private RoleBaseMoveableCollection DisplayOrder
			{
				get
				{
					return myRoleDisplayOrder;
				}
			}
			private FactType Fact
			{
				get
				{
					return myFact;
				}
			}
			/// <summary>
			/// Returns the number of items visible
			/// </summary>
			public int VisibleItemsCount
			{
				get { return this.VisibleItemCount; }
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
						// UNDONE: This should all change to using RoleBase[] when BuildPermutations is brought to O(sanity)
						List<RoleBase> tempRoleList = new List<RoleBase>(arity);
						myReadingOrderPermutations = new ReadingOrderKeyedCollection();
						for (int i = 0; i < arity; ++i)
						{
							tempRoleList.Add(myRoleDisplayOrder[i].Role);
						}
						List<List<RoleBase>> myPerms;myPerms = this.BuildPermutations(tempRoleList);
						int numPerms = myPerms.Count;
						for (int i = 0; i < numPerms; ++i)
						{
							roi =  new ReadingOrderInformation(this, myPerms[i].ToArray());
							myReadingOrderPermutations.Add(roi);
						}
					}
					// UNDONE: Localize "New Reading... " text
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
					if (myRoleNames == null)
					{
						myRoleNames = this.GetRoleNames();
					}
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
							ReadingOrder theOrder = FactType.GetReadingOrder(myFact, myReadingOrderKeyedCollection[row].RoleOrder as IList<RoleBase>);
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
						retval.SelectedImage = 0;   //you must set both .Image and .SelectedImage or an exception will be thrown
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
							retval = "Choose a New Reading Order from the Drop Down List"; //UNDONE: Localize 
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
				switch (style)
				{
					case ObjectStyle.TrackingObject:
						ReadingOrderInformation info;
						if (null != (info = obj as ReadingOrderInformation))
						{
							int location = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[info.RoleOrder]);
							if (-1 != location && myReadingOrderKeyedCollection.GetRowType(location) == RowType.UnCommitted)
							{
								return new LocateObjectData(location, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.ThisLevel);
							}
							else if (-1 != location)
							{
								return new LocateObjectData(location, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.NextLevel);
							}
						}
						else if (RowType.TypeEditorHost.Equals(obj))
						{
							return new LocateObjectData(this.VisibleItemCount -1, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.ThisLevel);
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
			public int VisibleItemCount
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

			#region Branch update methods
			#region Reading Branch Methods
			/// <summary>
			/// Initiates a New Reading within the selected Reading Order
			/// </summary>
			public void AddNewReading()
			{
				VirtualTreeItemInfo orderItemInfo = this.CurrentSelectionInfoReadingOrderBranch();
				myReadingOrderKeyedCollection[orderItemInfo.Row].Branch.ShowNewRow(true);
			}
			/// <summary>
			/// Instruct the readingbranch that a reading has been added to the collection
			/// </summary>
			/// <param name="reading">the reading to add</param>
			public void ReadingAdded(Reading reading) //UNDONE: work thru with addition of reading to current readings
			{
				ReadingOrder order = reading.ReadingOrder;
				int location = this.LocateCollectionItem(order);

				if (location < 0)
				{
					this.PopulateReadingOrderInfo(order);
					if (myModificationEvents != null)
					{
						int newLoc = this.LocateCollectionItem(order);
						myModificationEvents.Invoke(this, BranchModificationEventArgs.InsertItems(this, newLoc - 1, 1));
						myModificationEvents.Invoke(this, BranchModificationEventArgs.UpdateCellStyle(this, newLoc, (int)ColumnIndex.ReadingBranch, true)); //may not be needed due to callback on update
						//redraw off and back on in the branch if it has no more than 1 reading
					}
				}

				if (location >= 0)
				{
					myReadingOrderKeyedCollection[location].Branch.AddReading(reading);
					if (myModificationEvents != null)
					{
						myModificationEvents(this, BranchModificationEventArgs.UpdateCellStyle(this, location, (int)ColumnIndex.ReadingBranch, true));

						int actualIndex = myFact.ReadingOrderCollection.IndexOf(order);
						if (actualIndex != location)
						{
							this.ReadingOrderLocationUpdate(order);
						}
						else
						{
							myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
							myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
						}
					}
				}
			}
			/// <summary>
			/// Initiate edit for a reading
			/// </summary>
			/// <param name="reading">The reading</param>
			public void EditReading(Reading reading)
			{
				Debug.Assert(reading != null, "reading should exist");

				ReadingOrder order = reading.ReadingOrder;

				int count = myReadingOrderKeyedCollection.Count;
				for (int i = 0; i < count; ++i)
				{
					if(object.ReferenceEquals(myReadingOrderKeyedCollection[i].ReadingOrder, order))
					{
						myReadingOrderKeyedCollection[i].Branch.EditReading(reading);
					}
				}
			}
			/// <summary>
			/// Triggers the events notifying the tree that a Reading in the Readingbranch has been updated. 
			/// </summary>
			/// <param name="reading">The Reading affected</param>
			public void UpdateReading(Reading reading)
			{
				ReadingOrderInformation info = new ReadingOrderInformation(this, reading.ReadingOrder);
				if (!myReadingOrderKeyedCollection.Contains(info.RoleOrder))
				{
					myReadingOrderKeyedCollection.Add(info);
				}
				int row = this.myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[info.RoleOrder]);
				Debug.Assert(row >= 0, "Reading Must exist before it can be updated");
				myReadingOrderKeyedCollection[row].Branch.UpdateReading(reading);
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="reading">The Reading moved</param>
			public void ReadingLocationUpdate(Reading reading)
			{
				if (myModificationEvents != null)
				{
					ReadingOrder order = reading.ReadingOrder;
					int currentLocation = this.LocateCollectionItem(order);
					Debug.Assert(currentLocation >= 0, "Cannot Locate Reading");
					if (currentLocation >= 0)
					{
						myReadingOrderKeyedCollection[currentLocation].Branch.ReadingItemOrderChanged(reading);
					}
				}
			}
			/// <summary>
			/// Triggers notification that a Reading has been removed from the branch.
			/// </summary>
			/// <param name="reading">The Reading which has been removed</param>
			public void ReadingRemoved(Reading reading)
			{
				ReadingOrder order = reading.ReadingOrder;
				if (myModificationEvents != null)
				{
					int location = this.LocateCollectionItem(order);
					myReadingOrderKeyedCollection[location].Branch.ItemRemoved(reading);
				}
			}	
			/// <summary>
			/// Moves the selected Reading Up
			/// </summary>
			public void PromoteSelectedReading()
			{
				VirtualTreeItemInfo orderItemInfo = this.CurrentSelectionInfoReadingOrderBranch();	
				VirtualTreeItemInfo readingItemInfo = this.CurrentSelectionInfoReadingBranch();
				myReadingOrderKeyedCollection[orderItemInfo.Row].Branch.PromoteItem(readingItemInfo.Row);		
			}
			/// <summary>
			/// Moves the selected Reading Down
			/// </summary>
			public void DemoteSelectedReading()
			{
				VirtualTreeItemInfo orderItemInfo = this.CurrentSelectionInfoReadingOrderBranch();
				VirtualTreeItemInfo readingItemInfo = this.CurrentSelectionInfoReadingBranch();
				myReadingOrderKeyedCollection[orderItemInfo.Row].Branch.DemoteItem(readingItemInfo.Row);
			}
			#endregion //Reading Branch Methods

			#region ReadingOrder Branch Methods
			/// <summary>
			/// Used to find out if the branch is in the process of adding a new entry from input into the branch.
			/// </summary>
			public bool IsAdding
			{
				get
				{
					return myInsertedRow != -1;
				}
			}
			/// <summary>
			/// Initiates the Drop Down to select a new reading order for the reading to add
			/// </summary>
			public void AddNewReadingOrder()
			{
				TreeControl.SelectObject(myReadingOrderBranch, RowType.TypeEditorHost, (int)ObjectStyle.TrackingObject, 0);
				TreeControl.BeginLabelEdit();
			}
			/// <summary>
			/// Initiate edit for first reading within the RoleBaseMoveableCollection (will create a new item if needed)
			/// </summary>
			/// <param name="collection">The RoleBaseMoveableCollection to use</param>
			public void EditReadingOrder(RoleBaseMoveableCollection collection)
			{
				Debug.Assert(collection != null, "RoleBaseMoveableCollection is null");

				int numRoles = collection.Count;
				RoleBase[] roles = new RoleBase[numRoles];
				for (int i = 0; i < numRoles; ++i)
				{
					roles[i] = collection[i];
				}

				ReadingOrderInformation orderInfo = new ReadingOrderInformation(this, roles);
				if (!myReadingOrderKeyedCollection.Contains(orderInfo))
				{
					int newOrder = this.ShowNewOrder(roles as IList);

					if (ReadingEditor.TreeControl.SelectObject(myReadingOrderBranch, orderInfo, (int)ObjectStyle.TrackingObject, 0))
					{
						ReadingEditor.TreeControl.BeginLabelEdit();
					}
				}
			}
			/// <summary>
			/// Moves the selected ReadingOrder up
			/// </summary>
			public void PromoteSelectedReadingOrder()
			{
				this.MoveItem(true);
			}
			/// <summary>
			/// Moves the selected ReadingOrder Down
			/// </summary>
			public void DemoteSelectedReadingOrder()
			{
				this.MoveItem(false);
			}
			/// <summary>
			/// Removes the item selected when called by the context menu
			/// </summary>
			public void RemoveSelectedItem()
			{
				VirtualTreeItemInfo orderItem = this.CurrentSelectionInfoReadingOrderBranch();
				VirtualTreeItemInfo readingItem = this.CurrentSelectionInfoReadingBranch();
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(orderItem.Row);

				if (rowType == RowType.Committed)
				{
					myReadingOrderKeyedCollection[orderItem.Row].Branch.RemoveItem(readingItem.Row); //Remove selected Row in the ReadingBranch
				}	
				else if (rowType == RowType.UnCommitted) //remove the row from the readingOrderBranch
				{
				    myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, orderItem.Row, 1));
				    myReadingOrderKeyedCollection.RemoveAt(orderItem.Row);
				}
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="order">The ReadingOrder affected</param>
			public void ReadingOrderLocationUpdate(ReadingOrder order)
			{
				if (myModificationEvents != null)
				{
					int currentLocation = this.LocateCollectionItem(order);
					if (currentLocation >= 0) //make sure it was found
					{
						ReadingOrderInformation readingOrderInfo = myReadingOrderKeyedCollection[currentLocation];
						int newLocation = myFact.ReadingOrderCollection.IndexOf(order);
						myReadingOrderKeyedCollection.RemoveAt(currentLocation);
						myReadingOrderKeyedCollection.Insert(newLocation, readingOrderInfo);
						myModificationEvents(this, BranchModificationEventArgs.MoveItem(this, currentLocation, newLocation));
					}					
				}
			}
			/// <summary>
			/// Triggers notification that a ReadingOrder has been removed from the branch.
			/// </summary>
			/// <param name="order">The ReadingOrder which has been removed</param>
			public void ReadingOrderRemoved(ReadingOrder order)
			{
				if (myModificationEvents != null)
				{
					int location = this.LocateCollectionItem(order);
					if (location >= 0) //make sure it was found
					{
						myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, location, 1));
						myReadingOrderKeyedCollection.RemoveAt(location);
						myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
						myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
					}
				}
			}
			/// <summary>
			///  Get context menu commands supported for current selection
			/// </summary>
			/// <returns>ReadingEditorCommands bit field</returns>
			public ReadingEditorCommands SupportedSelectionCommands()
			{
				ReadingEditorCommands retval = ReadingEditorCommands.AddReadingOrder;
				VirtualTreeItemInfo orderItemInfo = this.CurrentSelectionInfoReadingOrderBranch();
				if (orderItemInfo.Row < this.VisibleItemCount - 1)
				{
					//ReadingOrder Context Menu Options
					if (myReadingOrderKeyedCollection.Count > 1)
					{
						if (orderItemInfo.Row > 0 && myReadingOrderKeyedCollection[orderItemInfo.Row].ReadingOrder != null)
						{
							retval |= ReadingEditorCommands.AddReading | ReadingEditorCommands.PromoteReadingOrder;
						}

						if (orderItemInfo.Row < this.VisibleItemCount - 2 && myReadingOrderKeyedCollection[orderItemInfo.Row].ReadingOrder != null)
						{
							retval |= ReadingEditorCommands.AddReading | ReadingEditorCommands.DemoteReadingOrder;
						}
					}

					//Reading Context Menu Options
					if (myReadingOrderKeyedCollection[orderItemInfo.Row].ReadingOrder != null)
					{
						VirtualTreeItemInfo readingItemInfo = this.CurrentSelectionInfoReadingBranch();
						if (readingItemInfo.Row < myReadingOrderKeyedCollection[orderItemInfo.Row].ReadingOrder.ReadingCollection.Count) //Catch if readingbranch is displaying ONLY an uncomitted reading
						{
							retval |= ReadingEditorCommands.DeleteReading;
							ReadingBranch readingBranch = myReadingOrderKeyedCollection[orderItemInfo.Row].Branch;
							if (readingItemInfo.Row > 0 && readingBranch.RowCount > 1)
							{
								retval |= ReadingEditorCommands.PromoteReading;
							}

							if (readingItemInfo.Row < readingBranch.RowCount - ((readingBranch.HasNewRow) ? 2 : 1)) //catch if displaying an uncommitted reading and committed readings
							{
								retval |= ReadingEditorCommands.DemoteReading;
							}
						}
					}
				}
				return retval;
			}
			#endregion //ReadingOrder Branch Methods

			#region Branch Helper Functions
			private string[] GetRoleNames()
			{
				string[] retVal = myRoleNames;
				if (retVal == null)
				{
					RoleBaseMoveableCollection factRoles = myFact.RoleCollection;
					ObjectType rolePlayer;
					int factArity = factRoles.Count;
					if (factArity == 1)
					{
						rolePlayer = factRoles[0].Role.RolePlayer;
						myRoleNames = new string[] { (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText };
					}
					else
					{
						RoleBaseMoveableCollection roleDisplayOrder = myRoleDisplayOrder;
						ObjectType[] rolePlayers = new ObjectType[factArity];
						for (int i = 0; i < factArity; ++i)
						{
							rolePlayers[i] = roleDisplayOrder[i].Role.RolePlayer;
						}
						myRoleNames = new string[factArity];
						for (int i = 0; i < factArity; ++i)
						{
							rolePlayer = roleDisplayOrder[i].Role.RolePlayer;

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
			private int ShowNewOrder(IList order)
			{
				int retVal;
				ReadingOrderInformation info = new ReadingOrderInformation(this, order as RoleBase[]);
				if (!myReadingOrderKeyedCollection.Contains(order))
				{
					myReadingOrderKeyedCollection.Add(info);
					myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, this.VisibleItemCount - 2, 1));
				}
				retVal = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[info.RoleOrder]);
				return retVal;
			}
			/// <summary>
			/// Populates the tree
			/// </summary>
			/// <param name="readingOrderIndex">Use -1 for all, or the index of a specific ReadingOrder within the collection </param>
			private void PopulateReadingOrderInfo(int readingOrderIndex)
			{
				ReadingOrderMoveableCollection readingOrders = myFact.ReadingOrderCollection;
				if (readingOrderIndex == -1)
				{
					myReadingOrderKeyedCollection.Clear();
					int thisBranchCount = readingOrders.Count;
					for (int i = 0; i < thisBranchCount; ++i)
					{
						this.PopulateReadingOrderInfo(readingOrders[i]);
					}
				}
				else
				{
					this.PopulateReadingOrderInfo(readingOrders[readingOrderIndex]);
				}
			}
			private void PopulateReadingOrderInfo(ReadingOrder order)
			{
				myReadingOrderKeyedCollection.Add(new ReadingOrderInformation(this, order));
			}
			/// <summary>
			/// Initiates a transaction for Moving a ReadingOrder within the collection
			/// </summary>
			/// <param name="promote">True if Moving UP, False if Moving DOWN</param>
			private void MoveItem(bool promote)
			{
				int currentLocation = -1, newLocation = 0;
				VirtualTreeItemInfo orderItemInfo = this.CurrentSelectionInfoReadingOrderBranch();
				Debug.Assert(orderItemInfo.Row >= 0, "Why is this not in the KeyedCollection?");
				currentLocation = orderItemInfo.Row;
				if (promote)
				{
					newLocation = currentLocation - 1;
				}
				else
				{
					newLocation = currentLocation + 1;
				}
				using (Transaction t = myFact.Store.TransactionManager.BeginTransaction("Moving Reading Order"))  //UNDONE: Centralize & Localize
				{
					myFact.ReadingOrderCollection.Move(currentLocation, newLocation);
					t.Commit();
				}
			}		
			/// <summary>
			/// Locates the OrderInformation item in the collection which reference matches the order passed in
			/// </summary>
			/// <param name="order">The ReadingOrder to match</param>
			/// <returns>index of item in the collection, -1 if not found</returns>
			private int LocateCollectionItem(ReadingOrder order)
			{
				int retVal = -1;
				if (myReadingOrderKeyedCollection.Contains(order.RoleCollection)) //UNDONE: need to re-work "IndexOf" to return -1 if non-existent
				{
					retVal = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[order.RoleCollection]);
				}
				return retVal;
			}
			/// <summary>
			/// Information about the current selected cell in a tree for the ReadingOrderBranch
			/// </summary>
			/// <returns>VirtualTreeItemInfo</returns>
			private VirtualTreeItemInfo CurrentSelectionInfoReadingOrderBranch()
			{
				return TreeControl.Tree.GetItemInfo(TreeControl.CurrentIndex, (int)ColumnIndex.ReadingOrder, true);
			}
			/// <summary>
			/// Information about the current selected cell in a tree for the ReadingBranch
			/// </summary>
			/// <returns>VirtualTreeItemInfo</returns>
			private VirtualTreeItemInfo CurrentSelectionInfoReadingBranch()
			{
				return TreeControl.Tree.GetItemInfo(TreeControl.CurrentIndex, TreeControl.CurrentColumn, true);
			}
			#endregion //Branch Helper Functions
			#endregion //Branch update methods

			#region Nested KeyedCollection class
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

				/// <summary>
				/// Used for a new order that does not exist in the model (uncommitted)
				/// </summary>
				/// <param name="parentBranch">branch this exists in</param>
				/// <param name="roleOrder">RoleBase[]</param>
				public ReadingOrderInformation(ReadingOrderBranch parentBranch, RoleBase[] roleOrder)
				{
					myParentBranch = parentBranch;
					myRoleOrder = roleOrder;
				}

				/// <summary>
				/// Used for a an order that does exist in the model (committed)
				/// </summary>
				/// <param name="parentBranch">branch this exists in</param>
				/// <param name="readingOrder">ReadingOrder to add</param>
				public ReadingOrderInformation(ReadingOrderBranch parentBranch, ReadingOrder readingOrder)
				{
					myParentBranch = parentBranch;
					myReadingOrder = readingOrder;
					myRoleOrder = readingOrder.RoleCollection;
				}

				/// <summary>
				/// Compare ReadingBranch object reference
				/// </summary>
				/// <param name="branch">ReadingBranch to compare</param>
				/// <returns>true if reference is equal, false otherwise</returns>
				public bool ReadingBranchReferenceEquals(ReadingBranch branch)
				{
					if (!object.ReferenceEquals(this.myReadingBranch, branch))
					{
						return false;
					}
					return true;
				}
				/// <summary>
				/// Returns the ReadingOrder of this Reading Order Information Item
				/// </summary>
				public ReadingOrder ReadingOrder
				{
					get
					{
						return myReadingOrder;
					}
				}
				/// <summary>
				/// Returns a ReadingBranch for this Order, the ReadingOrder must already exist in the model
				/// </summary>
				public ReadingBranch Branch
				{
					get
					{
						ReadingBranch retval = myReadingBranch;
						if (retval == null) 
						{
							if (myReadingOrder == null) //obtain new readingOrder to commit a new reading (in readingOrder is non-existent)
							{
								myReadingOrder = FactType.GetReadingOrder(myParentBranch.Fact, this.myRoleOrder as IList<RoleBase>);
							}
							myReadingBranch = new ReadingBranch(myReadingOrder, this);
							retval = myReadingBranch;
						}
						return retval;
					}
				}
				/// <summary>
				/// Returns an IList of the RoleBase Order
				/// </summary>
				public IList RoleOrder
				{
					get
					{
						return myRoleOrder;
					}
				}
				/// <summary>
				/// Returns Display Text for the RoleBase Order
				/// </summary>
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
				/// <summary>
				/// Returns a string[] for the Role Order Replacement Fields
				/// </summary>
				public string[] OrderedReplacementFields
				{
					get
					{
						string[] retVal = myOrderedReplacementFields;
						if (retVal == null)
						{
							string[] roleNames = myParentBranch.GetRoleNames();
							RoleBaseMoveableCollection displayOrder = myParentBranch.DisplayOrder;
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
										roleNames[displayOrder.IndexOf((RoleBase)roles[0])],
										roleNames[displayOrder.IndexOf((RoleBase)roles[1])]};
									break;
								case 3:
									retVal = new string[]{
										roleNames[displayOrder.IndexOf((RoleBase)roles[0])],
										roleNames[displayOrder.IndexOf((RoleBase)roles[1])],
										roleNames[displayOrder.IndexOf((RoleBase)roles[2])]};
									break;
								default:
									retVal = new string[arity];
									for (int i = 0; i < arity; ++i)
									{
										retVal[i] = roleNames[displayOrder.IndexOf((RoleBase)roles[i])];
									}
									break;
							}
							myOrderedReplacementFields = retVal;
						}
						return retVal;
					}
				}
				/// <summary>
				/// Overridden, treturns Text
				/// </summary>
				/// <returns></returns>
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
						collection[branchLocation].Branch.ShowNewRow(true);
						this.DoLabelEdit(branchLocation, collection);
					}
					else
					{
						branchLocation = myReadingOrderBranch.ShowNewOrder(info.RoleOrder);
						this.DoLabelEdit(branchLocation, collection);
					}
				}
				private void DoLabelEdit(int editLocation, ReadingOrderKeyedCollection collection)
				{
					int offset = 0; 
					for (int i = 0; i <= editLocation; i++)
					{
						offset += (collection[i].ReadingOrder != null) ? collection[i].Branch.RowCount : 1;
					}
					ReadingEditor.TreeControl.CurrentIndex = offset -1;
					ReadingEditor.TreeControl.CurrentColumn = (int)ColumnIndex.ReadingBranch;
					ReadingEditor.TreeControl.BeginLabelEdit();
				}
				public override bool ShouldSerializeValue(object component)
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}
			#endregion

			#region Nested method to generate role permutations
			public List<List<RoleBase>> BuildPermutations(List<RoleBase> roleList)
			{
				// UNDONE: This is absolutely nuts. It should return RoleBase[][] and allocate the arrays once
				List<List<RoleBase>> retval = new List<List<RoleBase>>();
				List<RoleBase> tmpList = null;
				int count = roleList.Count;
				if (count == 1)
				{
					retval.Add(roleList);
				}
				else
				{
					for (int i = 0; i < count; ++i)
					{
						RoleBase currentRole = roleList[i];
						tmpList = new List<RoleBase>(count - 1);
						for (int j = 0; j < count; ++j)
						{
							if (!roleList[j].Equals(currentRole))
							{
								tmpList.Add(roleList[j]);
							}
						}
						List<List<RoleBase>> result = BuildPermutations(tmpList);
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
				public const int COLUMN_COUNT = 1;
				private FactType myFact;
				private ReadingMoveableCollection myReadingMC;
				private BranchModificationEventHandler myModificationEvents;
				private List<ReadingData> myReadings = new List<ReadingData>();
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
					this.PopulateBranchData();
				}
				#endregion

				#region Branch Properties
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
				/// <summary>
				/// Returns true of branch has a new uncomiitted reading, false otherwise
				/// </summary>
				public bool HasNewRow
				{
					get
					{
						return showNewRow;
					}
				}
				#endregion //Branch Properties

				#region IBranch Member Mirror/Implementations

				VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
				{
					VirtualTreeLabelEditData retval = VirtualTreeLabelEditData.Default;
					if (row == myReadings.Count -1 && this.showNewRow)
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
						retval.AlternateText = myReadings[row].Reading.Text;
					}
					return retval;
				}

				LabelEditResult CommitLabelEdit(int row, int column, string newText)
				{
					Reading theReading;
					if (this.showNewRow && row == myReadings.Count - 1)
					{
					    try
					    {
					        myInsertedRow = row;
					        using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
					        {
								Debug.Assert(myReadingOrder != null, "A ReadingOrder should have been found or created.");
								this.ShowNewRow(false);
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
						theReading = myReadings[row].Reading;
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
					if (row < this.VisibleItemCount)
					{
						return myReadings[row].Text;
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
						return myReadings.Count; //get actual count for number of readings as a reading may not exist which is in the myReadings List
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
				#endregion

				#region Branch update methods
				/// <summary>
				/// Addition of a New Reading into the ReadingBranch
				/// </summary>
				/// <param name="reading">The reading changed or added</param>
				public void AddReading(Reading reading)
				{
					if (!myReadings.Contains(new ReadingData(null, reading)))
					{
						int index = myReadingMC.IndexOf(reading);
						myReadings.Insert(index, new ReadingData(FactType.PopulatePredicateText(reading, myReadingOrder.RoleCollection, myReadingInformation.OrderedReplacementFields), reading));
						if (myModificationEvents != null)
						{
							myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, -1, 1));
						}
					}
					else
					{
						this.UpdateReading(reading);
					}
				}
				/// <summary>
				/// Iinitiates a Begin Label Edit in the specified row and first column of the branch
				/// </summary>
				/// <param name="row">The Row to Edit</param>
				public void EditRow(int row)
				{
					this.BeginLabelEdit(row, 0, VirtualTreeLabelEditActivationStyles.ImmediateSelection);
				}
				/// <summary>
				/// Initiates a Begin Label Edit for the specified Reading
				/// </summary>
				/// <param name="reading">the Reading to edit</param>
				public void EditReading(Reading reading)
				{
					int count = myReadingMC.Count;
					int location = -1;
					for (int i = 0; i < count; ++i)
					{
						if(object.ReferenceEquals(myReadingMC[i], reading))
						{
							location = i;
							break;
						}
					}
					this.EditRow(location);
				}
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
				public void ShowNewRow(bool show)
				{
					if (!this.showNewRow && show == true)
					{
						this.showNewRow = true;
						myReadings.Add(new ReadingData(myReadingInformation.Text, null));
						myModificationEvents(this, BranchModificationEventArgs.InsertItems(this, -1 , 1));	
					}
					else if (this.showNewRow && show == false)
					{
						this.showNewRow = false;
						myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, this.VisibleItemCount -1, 1));
						myReadings.Remove(new ReadingData(myReadingInformation.Text, null));
					
					}
					
					if (this.showNewRow && show == true)
					{
						int count = myReadingOrderBranch.myReadingOrderKeyedCollection.Count;
						int offset = 0;
						for (int i = 0; i <= count; i++)
						{
							offset += myReadingOrderBranch.myReadingOrderKeyedCollection[i].Branch.RowCount;

							if(object.ReferenceEquals(myReadingOrderBranch.myReadingOrderKeyedCollection[i].Branch, this))
							{
								break;
							}		
						}
						ReadingEditor.TreeControl.CurrentIndex = offset - 1;
						ReadingEditor.TreeControl.CurrentColumn = (int)ColumnIndex.ReadingBranch;
						ReadingEditor.TreeControl.BeginLabelEdit();
					}
				}
				/// <summary>
				/// Initiates an Update of an existing Reading
				/// </summary>
				/// <param name="reading">The reading changed</param>
				public void UpdateReading(Reading reading)
				{
					int location = myReadings.IndexOf(new ReadingData(null, reading));
		
					myReadings[location] = new ReadingData(FactType.PopulatePredicateText(reading, reading.ReadingOrder.RoleCollection, myReadingInformation.OrderedReplacementFields), reading);

					if (myModificationEvents != null)
					{
						myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, location, 0, 1)));
						myModificationEvents(this, BranchModificationEventArgs.Redraw(false));
						myModificationEvents(this, BranchModificationEventArgs.Redraw(true));
					}
				}
				/// <summary>
				/// Removes a reading from the brach based upon the reference of the reading object sent
				/// </summary>
				/// <param name="reading">The reading object which has been removed</param>
				public void ItemRemoved(Reading reading)
				{
					for (int i = myReadings.Count - 1; i >= 0; --i) // run counter backwards so we can modify the set
					{
						if( object.ReferenceEquals(myReadings[i].Reading, reading))
						{
							myReadings.RemoveAt(i);
							myModificationEvents(this, BranchModificationEventArgs.DeleteItems(this, i, 1));
						}
					}
				}
				/// <summary>
				/// Initiate the removal of the current item selected
				/// </summary>
				/// <param name="row">Row to remove</param>
				public void RemoveItem(int row)
				{
					if ( this.showNewRow == true && row == this.VisibleItemCount - 1)
					{
						this.ShowNewRow(false);
					}
					else
					{
						this.CommitLabelEdit(row, 0, "");
					}
				}
				/// <summary>
				/// Move a Reading Up
				/// </summary>
				/// <param name="row">Row to move up</param>
				public void PromoteItem(int row)
				{
					this.MoveItem(row, row - 1);
				}
				/// <summary>
				/// Move a Reading Down
				/// </summary>
				/// <param name="row">Row to move down</param>
				public void DemoteItem(int row)
				{
					this.MoveItem(row, row + 1);
				}
				/// <summary>
				/// Updates the Display Text and notifies the tree that the display data has changed
				/// </summary>
				public void ReadingItemOrderChanged(Reading reading)
				{
					int oldRow = myReadings.IndexOf(new ReadingData(null, reading));
					int currentRow = myReadingMC.IndexOf(reading);

					if (oldRow != currentRow)
					{
						ReadingData readingData = myReadings[oldRow];
						myReadings.RemoveAt(oldRow);
						myReadings.Insert(currentRow, readingData);

						if (myModificationEvents != null)
						{
							myModificationEvents(this, BranchModificationEventArgs.MoveItem(this, oldRow, currentRow));
						}
					}
				}
				#endregion

				#region Branch Helper Methods, Structs
				/// <summary>
				/// Iinitiates a transaction for moving a Reading within this Reading Order
				/// </summary>
				/// <param name="currentRow">Index of Reading To Move</param>
				/// <param name="newLocation">New Location in the collection</param>
				private void MoveItem(int currentRow, int newLocation)
				{
					using (Transaction t = myFact.Store.TransactionManager.BeginTransaction("Demote Reading"))  //UNDONE: Centralize & Localize
					{
						myReadingMC.Move(currentRow, newLocation);
						t.Commit();
					}
				}
				/// <summary>
				/// Structure to hold Cached Reading Display Text with Populated Role Names, and the Reading Object associated
				/// </summary>
				public struct ReadingData : IEquatable<ReadingData>
				{
					private Reading myReading;
					private string myText;

					/// <summary>
					/// Constructor
					/// </summary>
					/// <param name="text">Populated Reading Display Text</param>
					/// <param name="reading">Reading Object</param>
					public ReadingData(string text, Reading reading)
					{
						this.myText = text;
						this.myReading = reading;
					}

					/// <summary>
					/// Returns the Populated Reading Display Text
					/// </summary>
					public string Text
					{
						get { return this.myText; }
					}

					/// <summary>
					/// Returns the Reading Object
					/// </summary>
					public Reading Reading
					{
						get { return this.myReading; }
					}
					/// <summary>
					/// 
					/// </summary>
					/// <param name="other">Reading to compare</param>
					/// <returns>True if reading passed matches current</returns>
					public bool Equals(ReadingData other)
					{
						return (object)other.myReading == myReading;
					}
				}
				//UNDONE: PopulateBranchData could be integrated with the structure for better design (allowing late binding, better performance, and enhanced encapsulation)
				/// <summary>
				/// Populates the Readings List with ReadingData Structure Types 
				/// </summary>
				private void PopulateBranchData()
				{
					myReadings.Clear();
					RoleBaseMoveableCollection roleCollection = myReadingOrder.RoleCollection;
					int numReadings = myReadingMC.Count;
					for (int i = 0; i < numReadings; ++i)
					{
						myReadings.Add( new ReadingData(FactType.PopulatePredicateText(myReadingMC[i], roleCollection, myReadingInformation.OrderedReplacementFields), myReadingMC[i] ));
					}
				}
				#endregion //Branch Helper Methods
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

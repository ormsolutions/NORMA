#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;

#endregion

namespace Northface.Tools.ORM.ObjectModel.Editors
{
	public partial class ReadingEditor : UserControl
	{
		private enum ColumnIndex
		{
			ReadingText = 0,
			IsPrimary = 1,
		}

		private FactType myFact = null;
		private List<ReadingEntry> myReadingList = null;
		private Role[] mySelectedRoleOrder = null;
		private ReadingBranch myBranch = null;

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

		private TreeNode[] CreateAutoFilledRootNodes()
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
				ReadingOrderMoveableCollection readingOrders = myFact.ReadingOrderCollection;
				foreach (ReadingOrder readingOrd in readingOrders)
				{
					if (IsMatchingReadingOrder(readingNode.RoleOrder, readingOrd))
					{
						AddReadingEntries(readingList, readingOrd);
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
					//assuming "All" node is only other possibility
					ReadingOrderMoveableCollection readingOrders = myFact.ReadingOrderCollection;
					foreach (ReadingOrder readingOrd in readingOrders)
					{
						AddReadingEntries(readingList, readingOrd);
					}
				}
			}
			myReadingList = readingList;

			ReadingBranch branch = new ReadingBranch(myReadingList, mySelectedRoleOrder, myFact, this);
			myBranch = branch;
			ReadingVirtualTree tree = new ReadingVirtualTree(branch);
			this.vtrReadings.MultiColumnTree = tree;
		}

		private void AddReadingEntries(List<ReadingEntry> readingList, ReadingOrder readingOrder)
		{
			ReadingMoveableCollection readings = readingOrder.ReadingCollection;
			foreach (Reading read in readings)
			{
				readingList.Add(new ReadingEntry(read));
			}
		}

		private bool IsMatchingReadingOrder(Role[] roleOrder, ReadingOrder readingOrder)
		{
			Debug.Assert(roleOrder.Length == readingOrder.RoleCollection.Count);

			RoleMoveableCollection roles = readingOrder.RoleCollection;
			int numRoles = roleOrder.Length;
			for (int i = 0; i < numRoles; ++i)
			{
				if (!roleOrder[i].Equals(roles[i])) return false;
			}
			return true;
		}

		private bool IsMatchingLeadRole(Role leadRoad, ReadingOrder readingOrder)
		{
			return leadRoad.Equals(readingOrder.RoleCollection[0]);
		}
		#endregion

		#region model events and handlers
		/// <summary>
		/// Attaches the event handlers to the store so that the tool window
		/// contents can be updated to reflect any model changes.
		/// </summary>
		public void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ReadingOrderHasReading.MetaRelationshipGuid);

			// Track ElementLink changes
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ElementLinkAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ElementLinkRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(Reading.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ElementAttributeChangedEvent));
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

			// Track ElementLink changes
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ElementLinkAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ElementLinkRemovedEvent));

			classInfo = dataDirectory.FindMetaClass(Reading.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ElementAttributeChangedEvent));
		}

		private void ElementLinkAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			Reading read = link.ReadingCollection;
			ReadingOrder ord = link.ReadingOrder;
			int index = ord.ReadingCollection.IndexOf(read);
			Debug.Assert(index >= 0);
			myReadingList.Insert(index, new ReadingEntry(read));
			myBranch.ItemAdded(index);
		}

		private void ElementLinkRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			Reading read = link.ReadingCollection;
			int numEntries = myReadingList.Count;
			int index = -1;
			for (int i = 0; i < numEntries; ++i)
			{
				if (object.ReferenceEquals(myReadingList[i].Reading, read))
				{
					index = i;
					break;
				}
			}
			Debug.Assert(index >= 0);
			myReadingList.RemoveAt(index);
			myBranch.ItemRemoved(index);
		}

		private void ElementAttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			Reading reading = e.ModelElement as Reading;
			int numEntries = myReadingList.Count;
			for (int i = 0; i < numEntries; ++i)
			{
				ReadingEntry re = myReadingList[i];
				if (object.ReferenceEquals(reading, re.Reading))
				{
					re.InvalidateText();
					int column = -1;
					Guid attrId = e.MetaAttribute.Id;
					if (attrId.Equals(Reading.TextMetaAttributeGuid))
					{
						column = (int)ColumnIndex.ReadingText;
					}
					else if (attrId.Equals(Reading.IsPrimaryMetaAttributeGuid))
					{
						column = (int)ColumnIndex.ReadingText;
					}
					myBranch.ItemUpdate(i, column);
					break;
				}
			}
		}

		#endregion

		#region nested class ReadingRootTreeNode
		private class ReadingRootTreeNode : TreeNode
		{
			Role myLeadRole;

			public ReadingRootTreeNode(Role leadRole)
			{
				Debug.Assert(leadRole != null);

				myLeadRole = leadRole;
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
		private class ReadingTreeNode : TreeNode
		{
			Role[] myRoleOrder;

			public ReadingTreeNode(Role[] roleOrder)
			{
				Debug.Assert(roleOrder != null);
				Debug.Assert(roleOrder.Length > 0);

				myRoleOrder = roleOrder;

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
			protected const string ELLIPSIS = "\x2026";
			protected const char C_ELLIPSIS = '\x2026';

			Reading myReading;
			String myText;
			int myRolePosition;

			#region construction
			protected ReadingEntry()
			{
			}

			public ReadingEntry(Reading reading)
			{
				Debug.Assert(reading != null);
				myReading = reading;
			}
			#endregion

			public virtual String Text
			{
				get
				{
					if (myText == null)
					{
						myText = GenerateDisplayText();
					}
					return myText;
				}
			}

			private String GenerateDisplayText()
			{
				RoleMoveableCollection roleSeq = myReading.ReadingOrder.RoleCollection;
				myRolePosition = 0;
				String retval = regCountPlaces.Replace(myReading.Text, new MatchEvaluator(ReplacePlaceHolders));
				return retval;
			}

			private string ReplacePlaceHolders(Match m)
			{
				string retval = null;
				RoleMoveableCollection roles = myReading.ReadingOrder.RoleCollection;
				if (myReading.ReadingOrder.RoleCollection.Count > myRolePosition)
				{
					ObjectType player = myReading.ReadingOrder.RoleCollection[myRolePosition].RolePlayer;
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
				++myRolePosition;
				return retval;
			}

			public Reading Reading
			{
				get
				{
					return myReading;
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
		}
		#endregion
		#region nested class NewReadingEntry
		private class NewReadingEntry : ReadingEntry
		{
			private static NewReadingEntry mySingleton = null;

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
		#region nested class ReadingInfo
		private class ReadingInfo
		{
			private Role[] myRoleSequence = null;
			private String myReading = null;

			public ReadingInfo(Role[] roleSequence, String reading)
			{
				myRoleSequence = roleSequence;
				myReading = reading;
			}

			public Role[] RoleSequence
			{
				get
				{
					return myRoleSequence;
				}
				set
				{
					myRoleSequence = value;
				}
			}

			public String Reading
			{
				get
				{
					return myReading;
				}
				set
				{
					myReading = value;
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
			ReadingEditor myParent;
			int myInsertedRow = -1;

			#region Construction
			public ReadingBranch(List<ReadingEntry> readingList, Role[] roleOrder, FactType fact, ReadingEditor parent)
			{
				Debug.Assert(readingList != null);
				Debug.Assert(fact != null);

				myRoleOrder = roleOrder;
				myReadingList = readingList;
				myFact = fact;
				myParent = parent;
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

			BranchFeatures Features
			{
				get
				{
					return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.StateChanges | BranchFeatures.InsertsAndDeletes | BranchFeatures.JaggedColumns;
				}
			}

			VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
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

			string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (column == (int) ColumnIndex.IsPrimary)
				{
					return ResourceStrings.ModelReadingEditorIsPrimaryToolTip;
				}
				return null;
			}

			bool IsExpandable(int row, int column)
			{
				return false;
			}

			LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				ReadingEntry ent = obj as ReadingEntry;
				Debug.Assert(ent != null);

				int pos = myReadingList.IndexOf(ent);
				LocateObjectData retval = new LocateObjectData(pos, 0, locateOptions);
				return retval;
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

			void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}

			void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}

			void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}

			VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
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

			int UpdateCounter
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
			int ColumnCount
			{
				get
				{
					return COLUMN_COUNT;
				}
			}

			SubItemCellStyles ColumnStyles(int column)
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

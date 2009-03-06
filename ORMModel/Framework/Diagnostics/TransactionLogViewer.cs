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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Emit = System.Reflection.Emit;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Framework.Diagnostics
{
	#region PartitionChange structure
	/// <summary>
	/// A structure use to track event arguments for a single item
	/// </summary>
	public struct PartitionChange
	{
		private EventArgs myArgs;
		private Partition myPartition;
		private RoleAssignment[] myRoleAssignments;
		/// <summary>
		/// Create a new StoreChange structure
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> describing the change.</param>
		/// <param name="roleAssignments">The <see cref="RoleAssignment"/> array associated with this change.</param>
		/// <param name="partition">The <see cref="Partition"/> the change occurred in</param>
		private PartitionChange(EventArgs e, RoleAssignment[] roleAssignments, Partition partition)
		{
			myArgs = e;
			myRoleAssignments = (roleAssignments != null && roleAssignments.Length == 2) ? roleAssignments : null;
			myPartition = partition;
		}
		/// <summary>
		/// The <see cref="EventArgs"/> describing the change.
		/// </summary>
		public EventArgs ChangeArgs
		{
			get
			{
				return myArgs;
			}
		}
		/// <summary>
		/// The <see cref="Partition"/> the change occurred in.
		/// </summary>
		public Partition Partition
		{
			get
			{
				return myPartition;
			}
		}
		/// <summary>
		/// The <see cref="RoleAssignments"/> array the change occurred in
		/// </summary>
		public RoleAssignment[] RoleAssignments
		{
			get
			{
				return myRoleAssignments;
			}
		}
		/// <summary>
		/// Retrieve an array of <see cref="PartitionChange"/> structures
		/// based on the sepcified <see cref="TransactionItem"/>
		/// </summary>
		public static PartitionChange[] GetPartitionChanges(TransactionItem transactionItem)
		{
			return TransactionItemGetPartitionChanges(transactionItem);
		}
		#region Dynamic Microsoft.VisualStudio.Modeling.TransactionItem.GetPartitionChanges implementation
		private delegate PartitionChange[] TransactionItemGetPartitionChangesDelegate(TransactionItem @this);
		/// <summary>
		/// Microsoft.VisualStudio.Modeling.ModelCommand is internal, but the partition and EventArgs
		/// elements are not. Retrieve these with a dynamic method.
		/// </summary>
		private static readonly TransactionItemGetPartitionChangesDelegate TransactionItemGetPartitionChanges = CreateTransactionItemGetPartitionChanges();
		private static TransactionItemGetPartitionChangesDelegate CreateTransactionItemGetPartitionChanges()
		{
			Type transactionItemType = typeof(TransactionItem);
			Type partitionType = typeof(Partition);
			Type partitionChangeType = typeof(PartitionChange);
			Type partitionChangeArrayType = partitionChangeType.MakeArrayType();
			Type roleAssignmentArrayType = typeof(RoleAssignment).MakeArrayType();
			Assembly modelingAssembly = transactionItemType.Assembly;
			string privateTypeBaseName = transactionItemType.Namespace + Type.Delimiter;
			Type modelCommandType;
			Type modelCommandListType;
			Type elementCommandType;
			Type elementLinkCommandType;
			PropertyInfo commandsProperty;
			MethodInfo getCommandsMethod;
			PropertyInfo partitionProperty;
			MethodInfo getPartitionMethod;
			PropertyInfo eventArgsProperty;
			MethodInfo getEventArgsMethod;
			FieldInfo roleAssignmentsField;
			ConstructorInfo partitionChangeConstructor;
			if (null == (modelCommandType = modelingAssembly.GetType(privateTypeBaseName + "ModelCommand", false)) ||
				null == (elementCommandType = modelingAssembly.GetType(privateTypeBaseName + "ElementCommand", false)) ||
				null == (elementLinkCommandType = modelingAssembly.GetType(privateTypeBaseName + "ElementLinkCommand", false)) ||
				!modelCommandType.IsAssignableFrom(elementCommandType) ||
				!elementCommandType.IsAssignableFrom(elementLinkCommandType) ||
				null == (modelCommandListType = typeof(List<>).MakeGenericType(modelCommandType)) ||
				null == (commandsProperty = transactionItemType.GetProperty("Commands", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
				commandsProperty.PropertyType != modelCommandListType ||
				null == (getCommandsMethod = commandsProperty.GetGetMethod(true)) ||
				null == (partitionProperty = elementCommandType.GetProperty("Partition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
				partitionProperty.PropertyType != partitionType ||
				null == (getPartitionMethod = partitionProperty.GetGetMethod(true)) ||
				null == (eventArgsProperty = modelCommandType.GetProperty("EventArgs", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
				eventArgsProperty.PropertyType != typeof(EventArgs) ||
				null == (getEventArgsMethod = eventArgsProperty.GetGetMethod(true)) ||
				null == (partitionChangeConstructor = partitionChangeType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(EventArgs), roleAssignmentArrayType, typeof(Partition) }, null)) ||
				null == (roleAssignmentsField = elementLinkCommandType.GetField("roleAssignments", BindingFlags.NonPublic | BindingFlags.Instance)))
			{
				// The structure of the internal dll implementation has changed, il generation will fail
				return null;
			}

			// Approximate method being written (assuming TransactionItem context):
			// PartitionChange[] GetPartitionChanges()
			// {
			//     List<ModelCommand> commands = Commands;
			//     commandsCount = commands.Count;
			//     PartitionChange[] retVal = new PartitionChange[commandsCount];
			//     for (int i = 0; i < commandsCount; ++i)
			//     {
			//         ModelCommand currentCommand = commands[i];
			//         ElementCommand elementCommand = currentCommand as ElementCommand;
			//         ElementLinkCommand elementLinkCommand = currentCommand as ElementLinkCommand;
			//         retVal[i] = new PartitionChange(currentCommand.EventArgs, (elementLinkCommand != null) ? elementLinkCommand.roleAssignments : null, (elementCommand != null) ? elementCommand.Partition : null);
			//     }
			//     return retVal;
			// }
			DynamicMethod dynamicMethod = new DynamicMethod(
				"TransactionItemGetPartitionChanges",
				partitionChangeArrayType,
				new Type[] { transactionItemType },
				transactionItemType,
				true);
			// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 55 bytes
			// to avoid a resize to an even larger buffer.
			ILGenerator il = dynamicMethod.GetILGenerator(64);
			Emit.Label loopBodyLabel = il.DefineLabel();
			Emit.Label loopTestLabel = il.DefineLabel();
			Emit.Label notAnElementCommandLabel = il.DefineLabel();
			Emit.Label pushPartitionLabel = il.DefineLabel();
			Emit.Label notAnElementLinkCommandLabel = il.DefineLabel();
			Emit.Label createPartitionChangeLabel = il.DefineLabel();
			il.DeclareLocal(typeof(int)); // commandsCount
			il.DeclareLocal(typeof(int)); // i
			il.DeclareLocal(modelCommandType); // currentCommand
			il.DeclareLocal(modelCommandListType); // commands
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Call, getCommandsMethod);
			il.Emit(OpCodes.Dup); // Store the result
			il.Emit(OpCodes.Stloc_3);

			// Cache the loop count and create the returned array
			il.Emit(OpCodes.Call, modelCommandListType.GetProperty("Count").GetGetMethod());
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc_0);
			il.Emit(OpCodes.Newarr, partitionChangeType);

			// Initialize the loop
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc_1);
			il.Emit(OpCodes.Br_S, loopTestLabel);

			// Loop contents
			il.MarkLabel(loopBodyLabel);
			il.Emit(OpCodes.Dup); // Duplicate the return value
			il.Emit(OpCodes.Ldloc_1); // Push i for array element storage
			il.Emit(OpCodes.Ldelema, partitionChangeType);
			il.Emit(OpCodes.Ldloc_3); // Load commands
			il.Emit(OpCodes.Ldloc_1); // push i
			il.Emit(OpCodes.Call, modelCommandListType.GetProperty("Item").GetGetMethod());
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc_2);

			// Push the EventArgs argument
			il.Emit(OpCodes.Call, getEventArgsMethod);

			// Push the roleAssignments argument
			il.Emit(OpCodes.Ldloc_2);
			il.Emit(OpCodes.Isinst, elementLinkCommandType);
			il.Emit(OpCodes.Dup); // For test
			il.Emit(OpCodes.Brfalse_S, notAnElementLinkCommandLabel);
			il.Emit(OpCodes.Ldfld, roleAssignmentsField);
			il.Emit(OpCodes.Br_S, pushPartitionLabel);

			// Cast failed, pop extra typed item and load null
			il.MarkLabel(notAnElementLinkCommandLabel);
			il.Emit(OpCodes.Pop); // Pops modelCommand instance
			il.Emit(OpCodes.Ldnull);
			
			// Push the partition argument
			il.MarkLabel(pushPartitionLabel);
			il.Emit(OpCodes.Ldloc_2);
			il.Emit(OpCodes.Isinst, elementCommandType);
			il.Emit(OpCodes.Dup); // For test
			il.Emit(OpCodes.Brfalse_S, notAnElementCommandLabel);
			il.Emit(OpCodes.Call, getPartitionMethod);
			il.Emit(OpCodes.Br_S, createPartitionChangeLabel);

			// Cast failed, pop extra typed item and load null
			il.MarkLabel(notAnElementCommandLabel);
			il.Emit(OpCodes.Pop); // Pops modelCommand instance
			il.Emit(OpCodes.Ldnull);

			// Initialize the new element into the array
			il.MarkLabel(createPartitionChangeLabel);
			il.Emit(OpCodes.Call, partitionChangeConstructor);

			// Loop index increment
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc_1);

			// Loop test
			il.MarkLabel(loopTestLabel);
			il.Emit(OpCodes.Ldloc_1);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Blt_S, loopBodyLabel);

			// Return the array (already on the stack)
			il.Emit(OpCodes.Ret);
			return (TransactionItemGetPartitionChangesDelegate)dynamicMethod.CreateDelegate(typeof(TransactionItemGetPartitionChangesDelegate));
		}
		#endregion // Dynamic Microsoft.VisualStudio.Modeling.TransactionItem.GetPartitionChanges implementation
	}
	#endregion // PartitionChange structure
	/// <summary>
	/// Helper class to display a dialog showing steps in completed transactions
	/// from a <see cref="Store"/>
	/// </summary>
	public partial class TransactionLogViewer : Form
	{
		#region GlyphIndex enum
		/// <summary>
		/// Indices correspond to values in the ImageStream
		/// </summary>
		private enum GlyphIndex
		{
			DomainModel,
			DomainClass,
			DomainRelationship,
			SourceDomainRole,
			TargetDomainRole,
			DomainProperty,
			OldValue,
			NewValue,
			ElementId,
			OldElementId,
			NewElementId,
			Partition,
			Filter, // Used for FilterLabel
			Blank, // Blank image
		}
		#endregion // GlyphIndex enum
		#region Member Variables
		private static TransactionLogViewer mySingleton;
		private IList<TransactionItem> myUndoItems;
		private IList<TransactionItem> myRedoItems;
		private PartitionChangeFilter myFilter;
		private bool mySkipClose;
		#endregion // Member Variables
		#region Initialization
		/// <summary>
		/// Display a dialog detailing transactions in the current store
		/// </summary>
		/// <param name="store">The target <see cref="Store"/></param>
		/// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to display the dialog</param>
		public static void Show(Store store, IServiceProvider serviceProvider)
		{
			IUIService uiService;
			if (null != serviceProvider &&
				null != (uiService = (IUIService)serviceProvider.GetService(typeof(IUIService))))
			{
				TransactionLogViewer viewer = mySingleton;
				if (mySingleton == null)
				{
					viewer = new TransactionLogViewer();
					ChangeBranch.InitializeHeaders(viewer.TreeControl);
					mySingleton = viewer;
				}
				viewer.Attach(store.UndoManager);
				uiService.ShowDialog(viewer);
			}
		}
		/// <summary>
		/// Create a new TransactionLogViewer form
		/// </summary>
		private TransactionLogViewer()
		{
			InitializeComponent();
		}
		private void Attach(UndoManager undoManager)
		{
			// Items filled here should be cleared in the TransactionLogViewer_FormClosed event
			myUndoItems = undoManager.UndoableTransactions;
			myRedoItems = undoManager.RedoableTransactions;
			FillCombo(UndoItemsCombo, myUndoItems);
			FillCombo(RedoItemsCombo, myRedoItems);
			TreeControl.MultiColumnTree = new StandardMultiColumnTree((int)ChangeBranch.ColumnContent.Count);
			if (myUndoItems.Count != 0)
			{
				UndoItemsCombo.SelectedIndex = 0;
			}
			else if (myRedoItems.Count != 0)
			{
				RedoItemsCombo.SelectedIndex = 0;
			}
		}
		private static void FillCombo(ComboBox combo, ICollection<TransactionItem> transactionItems)
		{
			ComboBox.ObjectCollection items = combo.Items;
			items.Clear();
			foreach (TransactionItem item in transactionItems)
			{
				items.Add(item.Name);
			}
		}
		#endregion // Initialization
		#region Filter Classes
		#region ChangeFilter class
		private class PartitionChangeFilter
		{
			private bool myFilterChanged;
			private Partition myPartition;
			private DomainModelInfo myDomainModel;
			private DomainClassInfo myDomainClass;
			private DomainPropertyInfo myDomainProperty;
			private Type myChangeType;
			private Guid? myElementId;
			private ChangeSource? myChangeSource;
			/// <summary>
			/// Return an array of indices into the <paramref name=" changes"/>
			/// array that matches the current state of the filter.
			/// </summary>
			/// <param name="changes">An array of <see cref="PartitionChange"/>s</param>
			/// <param name="filter">On input, this is the previous filter returned
			/// from this method for these changes. If the current filter state has not changed
			/// since the previous call then the array will not be modified.</param>
			/// <returns><see langword="true"/> if the <paramref name="filter"/> array is modified.</returns>
			public bool ApplyFilter(PartitionChange[] changes, ref int[] filter)
			{
				if (!myFilterChanged)
				{
					myFilterChanged = false;
					return false;
				}
				myFilterChanged = false;
				Partition partition = myPartition;
				DomainModelInfo modelInfo = myDomainModel;
				DomainClassInfo classInfo = myDomainClass;
				DomainPropertyInfo propertyInfo = myDomainProperty;
				ChangeSource? source = myChangeSource;
				Guid? elementId = myElementId;
				Type changeType = myChangeType;
				if (partition == null &&
					modelInfo == null &&
					classInfo == null &&
					propertyInfo == null &&
					changeType == null &&
					!source.HasValue &&
					!elementId.HasValue)
				{
					filter = null;
					return true;
				}
				int changeCount = changes.Length;
				int[] newFilter = new int[changeCount];
				int filterCount = 0;
				for (int i = 0; i < changeCount; ++i)
				{
					PartitionChange change = changes[i];
					if (partition != null && change.Partition != partition)
					{
						continue;
					}
					EventArgs changeArgs = change.ChangeArgs;
					if (changeType != null && changeType != changeArgs.GetType())
					{
						continue;
					}
					GenericEventArgs genericArgs = changeArgs as GenericEventArgs;
					ElementPropertyChangedEventArgs propertyChangeArgs = genericArgs as ElementPropertyChangedEventArgs;
					if (modelInfo != null)
					{
						if (propertyChangeArgs != null)
						{
							if (propertyChangeArgs.DomainProperty.DomainModel != modelInfo)
							{
								continue;
							}
						}
						else if (genericArgs == null || modelInfo != genericArgs.DomainModel)
						{
							continue;
						}
					}
					if (classInfo != null &&
						(genericArgs == null || classInfo != genericArgs.DomainClass) &&
						(propertyChangeArgs == null || propertyChangeArgs.DomainProperty.DomainClass != classInfo))
					{
						continue;
					}
					if (propertyInfo != null && (null == propertyChangeArgs || propertyInfo != propertyChangeArgs.DomainProperty))
					{
						continue;
					}
					if (source.HasValue && (genericArgs == null || source.Value != genericArgs.ChangeSource))
					{
						continue;
					}
					if (elementId.HasValue)
					{
						if (genericArgs == null)
						{
							continue;
						}
						else
						{
							Guid id = elementId.Value;
							if (genericArgs.ElementId != id)
							{
								RoleAssignment[] assignments = change.RoleAssignments;
								int assignmentCount = (assignments != null) ? assignments.Length : 0;
								int j = 0;
								for (; j < assignmentCount; ++j)
								{
									if (assignments[j].RolePlayer.Id == id)
									{
										break;
									}
								}
								if (j == assignmentCount)
								{
									RolePlayerChangedEventArgs changedArgs;
									if (null == (changedArgs = genericArgs as RolePlayerChangedEventArgs) ||
										(id != changedArgs.NewRolePlayerId && id != changedArgs.OldRolePlayerId))
									{
										RolePlayerOrderChangedEventArgs orderChangedArgs;
										if (null == (orderChangedArgs = genericArgs as RolePlayerOrderChangedEventArgs) ||
											id != orderChangedArgs.CounterpartRolePlayerId)
										{
											continue;
										}
									}
								}
							}
						}
					}
					newFilter[filterCount] = i;
					++filterCount;
				}
				if (filterCount == changeCount)
				{
					newFilter = null;
				}
				else
				{
					Array.Resize<int>(ref newFilter, filterCount);
				}
				filter = newFilter;
				return true;
			}
			/// <summary>
			/// Clear all values from the current filter. Note that this can
			/// cause the <see cref="FilterChanged"/> property to change to <see langword="true"/>
			/// </summary>
			public void ClearFilter()
			{
				Partition = null;
				DomainClass = null;
				DomainModel = null;
				DomainProperty = null;
				ChangeType = null;
				ChangeSource = null;
				ElementId = null;
			}
			/// <summary>
			/// Return true if the current filter has no values turned on
			/// </summary>
			public bool IsClear
			{
				get
				{
					return myPartition == null &&
						myDomainModel == null &&
						myDomainClass == null &&
						myChangeType == null &&
						myDomainProperty == null &&
						!myChangeSource.HasValue &&
						!myElementId.HasValue;
				}
			}
			/// <summary>
			/// True if the filter has changed since the last call to ApplyFilter
			/// </summary>
			public bool FilterChanged
			{
				get
				{
					return myFilterChanged;
				}
			}
			/// <summary>
			/// Filter on this <see cref="Partition"/>
			/// </summary>
			public Partition Partition
			{
				get
				{
					return myPartition;
				}
				set
				{
					if (value != myPartition)
					{
						myFilterChanged = true;
						myPartition = value;
					}
				}
			}
			/// <summary>
			/// Filter on this <see cref="DomainModelInfo"/>
			/// </summary>
			public DomainModelInfo DomainModel
			{
				get
				{
					return myDomainModel;
				}
				set
				{
					if (value != myDomainModel)
					{
						myFilterChanged = true;
						myDomainModel = value;
					}
				}
			}
			/// <summary>
			/// Filter on this <see cref="DomainClassInfo"/>
			/// </summary>
			public DomainClassInfo DomainClass
			{
				get
				{
					return myDomainClass;
				}
				set
				{
					if (value != myDomainClass)
					{
						myFilterChanged = true;
						myDomainClass = value;
					}
				}
			}
			/// <summary>
			/// Filter on this <see cref="DomainPropertyInfo"/>
			/// </summary>
			public DomainPropertyInfo DomainProperty
			{
				get
				{
					return myDomainProperty;
				}
				set
				{
					if (value != myDomainProperty)
					{
						myFilterChanged = true;
						myDomainProperty = value;
					}
				}
			}
			/// <summary>
			/// Filter on the <see cref="Type"/> of the <see cref="EventArgs"/> returned by <see cref="PartitionChange.ChangeArgs"/>
			/// </summary>
			public Type ChangeType
			{
				get
				{
					return myChangeType;
				}
				set
				{
					if (value != myChangeType)
					{
						myFilterChanged = true;
						myChangeType = value;
					}
				}
			}
			/// <summary>
			/// Filter on this <see cref="ChangeSource"/>
			/// </summary>
			public ChangeSource? ChangeSource
			{
				get
				{
					return myChangeSource;
				}
				set
				{
					if (value != myChangeSource)
					{
						myFilterChanged = true;
						myChangeSource = value;
					}
				}
			}
			/// <summary>
			/// Filter on this ElementId
			/// </summary>
			public Guid? ElementId
			{
				get
				{
					return myElementId;
				}
				set
				{
					if (value != myElementId)
					{
						myFilterChanged = true;
						myElementId = value;
					}
				}
			}
		}
		#endregion // ChangeFilter class
		#region IItemFilter interface
		/// <summary>
		/// An interface to implement along with <see cref="IBranch"/>
		/// to enable the branch to add one of its items to a filter.
		/// </summary>
		private interface IItemFilter
		{
			/// <summary>
			/// Modify the filter to include this item
			/// </summary>
			/// <param name="filter">The <see cref="PartitionChangeFilter"/> to modify</param>
			/// <param name="row">The row in the branch.</param>
			/// <param name="column">The column in the branch.</param>
			void FilterItem(PartitionChangeFilter filter, int row, int column);
		}
		#endregion // IItemFilter interface
		#endregion // Filter Classes
		#region BaseBranch class
		/// <summary>
		/// A helper class to provide a default IBranch implementation
		/// </summary>
		private abstract class BaseBranch : IBranch
		{
			#region IBranch Implementation
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.None;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return VirtualTreeDisplayData.Empty;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				Debug.Fail("Should override.");
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				Debug.Fail("Should override.");
				return null;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return false;
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return default(LocateObjectData);
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add { }
				remove { }
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
			{
			}
			void IBranch.OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
			{
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
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
					Debug.Fail("Should override");
					return 0;
				}
			}
			#endregion // IBranch Implementation
		}
		#endregion // BaseBranch class
		#region ChangeBranch class
		/// <summary>
		/// A helper class to provide a default IBranch implementation
		/// </summary>
		private sealed class ChangeBranch : BaseBranch, IBranch, IMultiColumnBranch, IItemFilter
		{
			#region Header initialization
			public static void InitializeHeaders(VirtualTreeControl control)
			{
				control.SetColumnHeaders(new VirtualTreeColumnHeader[]
				{
					new VirtualTreeColumnHeader("Change"),
					new VirtualTreeColumnHeader("Details (1)"),
					new VirtualTreeColumnHeader("Details (2)"),
					new VirtualTreeColumnHeader("", 36, true),
					new VirtualTreeColumnHeader("Partition"),
					new VirtualTreeColumnHeader("ChangeSource"),
					new VirtualTreeColumnHeader("DomainModel"),
					new VirtualTreeColumnHeader("DomainClass"),
					new VirtualTreeColumnHeader("ElementId"),
				}, true);
				control.ColumnPermutation = new ColumnPermutation(
					(int)ColumnContent.Count,
					new int[]{
						(int)ColumnContent.Index,
						(int)ColumnContent.Partition,
						(int)ColumnContent.ChangeSource,
						(int)ColumnContent.Model,
						(int)ColumnContent.Class,
						(int)ColumnContent.Id,
						(int)ColumnContent.ChangeType,
						(int)ColumnContent.Detail1,
						(int)ColumnContent.Detail2,
					},
					false);
			}
			#endregion // Header initialization
			#region ColumnContent enum
			/// <summary>
			/// Enum listing the column contents
			/// </summary>
			public enum ColumnContent
			{
				/// <summary>
				/// The type of change (add, remove, change, etc)
				/// </summary>
				ChangeType,
				/// <summary>
				/// The first detail column
				/// </summary>
				Detail1,
				/// <summary>
				/// The second detail column
				/// </summary>
				Detail2,
				/// <summary>
				/// The index of the change number
				/// </summary>
				Index,
				/// <summary>
				/// The item's partition
				/// </summary>
				Partition,
				/// <summary>
				/// The <see cref="Microsoft.VisualStudio.Modeling.ChangeSource"/>
				/// </summary>
				ChangeSource,
				/// <summary>
				/// The containing model
				/// </summary>
				Model,
				/// <summary>
				/// The class for the change
				/// </summary>
				Class,
				/// <summary>
				/// The Id for the element
				/// </summary>
				Id,
				/// <summary>
				/// The number of normal columns
				/// </summary>
				Count,
			}
			#endregion // ColumnContent enum
			#region IDetailHandler interface
			/// <summary>
			/// An interface to add detailed handlers for different types of event args.
			/// A dictionary keyed off the type of change argument will be used to key
			/// each type directly to its handler.
			/// </summary>
			private interface IDetailHandler
			{
				/// <summary>
				/// Return the number of detail columns required to detail this object.
				/// The returned value should not be higher than 2
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				int GetDetailColumnCount(PartitionChange change);
				/// <summary>
				/// Return the column style for the detail column at the specified index
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <param name="detailColumn">The 0-based detail column</param>
				/// <returns><see cref="SubItemCellStyles"/></returns>
				SubItemCellStyles GetDetailColumnStyle(PartitionChange change, int detailColumn);
				/// <summary>
				/// Return the text for a detail column
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <param name="detailColumn">The 0-based detail column</param>
				/// <returns>Text for the detailed item</returns>
				string GetDetailText(PartitionChange change, int detailColumn);
				/// <summary>
				/// Return the tip text for a detail column
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <param name="detailColumn">The 0-based detail column</param>
				/// <param name="tipType">The tooltip target</param>
				/// <returns>TipText for the detailed item</returns>
				string GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType);
				/// <summary>
				/// Return the detail object (generally an IBranch implementation)
				/// for a detail expansion or subitem
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <param name="detailColumn">The 0-based detail column, or -1 for the primary expansion</param>
				/// <param name="style"><see cref="ObjectStyle"/></param>
				/// <returns>Will generally return an <see cref="IBranch"/>, but may be used for
				/// other <see cref="ObjectStyle"/> values in the future.</returns>
				object GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style);
				/// <summary>
				/// Decide if the detail item can be expanded
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <param name="detailColumn">The 0-based detail column, or -1 for the primary expansion</param>
				/// <returns><see langword="true"/> if the detail item can be expanded</returns>
				bool GetDetailIsExpandable(PartitionChange change, int detailColumn);
				/// <summary>
				/// Provide display information for the item details
				/// </summary>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <param name="detailColumn">The 0-based detail column, or -1 for the primary expansion</param>
				/// <param name="requiredData"><see cref="VirtualTreeDisplayDataMasks"/></param>
				/// <returns><see cref="VirtualTreeDisplayData"/></returns>
				VirtualTreeDisplayData GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData);
				/// <summary>
				/// Provide base class info for this element. Used to display a base ClassInfo/DomainModel
				/// when the change is bound to a base type but the instance is of a more derived type, such
				/// as a property change on a base class.
				/// </summary>
				/// <remarks>This has nothing to do with the two detail columns, but is not a sufficiently compelling reason to
				/// add a parallel type mechanism.</remarks>
				/// <param name="change">The <see cref="PartitionChange"/> to provide details for</param>
				/// <returns>A <see cref="DomainClassInfo"/> or <see langword="null"/></returns>
				DomainClassInfo GetBaseDomainClassInfo(PartitionChange change);
			}
			#endregion // IDetailHandler interface
			#region Helper methods
			/// <summary>
			/// Make sure we don't try to to string a ModelElement stored in a
			/// property value, get the identifier instead.
			/// </summary>
			private static string GetValueString(object value)
			{
				string retVal = null;
				if (value != null)
				{
					Type type = value.GetType();
					if (!type.IsEnum &&
						Type.GetTypeCode(type) == TypeCode.Object &&
						typeof(ModelElement).IsAssignableFrom(type))
					{
						retVal = FormatIdentifier(((ModelElement)value).Id);
					}
					else
					{
						retVal = value.ToString();
					}
				}
				return retVal;
			}
			/// <summary>
			/// Return true if the value is a model element
			/// </summary>
			private static bool IsElement(object value)
			{
				if (value != null)
				{
					Type type = value.GetType();
					if (!type.IsEnum &&
						Type.GetTypeCode(type) == TypeCode.Object &&
						typeof(ModelElement).IsAssignableFrom(type))
					{
						return true;
					}
				}
				return false;
			}
			private static string FormatIdentifier(Guid id)
			{
				return id.ToString("N").Substring(0, 8);
			}
			private static string FormatFullIdentifier(Guid id)
			{
				return id.ToString("D");
			}
			#endregion // Helper methods
			#region Filter implementation
			private PartitionChange GetChange(int branchRow)
			{
				int[] filter = myFilter;
				return (filter != null) ? myChanges[filter[branchRow]] : myChanges[branchRow];
			}
			/// <summary>
			/// Called to potentially modify the current filter
			/// </summary>
			/// <param name="filter">The <see cref="PartitionChangeFilter"/></param>
			public void FilterChanged(PartitionChangeFilter filter)
			{
				int[] newFilter = null;
				int[] oldFilter = myFilter;
				if (oldFilter != null)
				{
					// We need to copy the filter because of the MSBUG with VirtualTree.DeleteItems discussed
					int filterCount = oldFilter.Length;
					newFilter = new int[filterCount];
					oldFilter.CopyTo(newFilter, 0);
				}
				if (filter.ApplyFilter(myChanges, ref newFilter))
				{
					int itemCount = VisibleItemCount;
					BranchModificationEventHandler modify = myModify;
					if (modify != null)
					{
						modify(this, BranchModificationEventArgs.DelayRedraw(true));

						myFilter = newFilter; // This changes the VisibleItemCount
						int columnCount = (int)ColumnContent.Count;

						// MSBUG: VirtualTreeControl is not handling selection tracking
						// properly for some insert cases. So reducing the filter will work most
						// of the time, but anything with an insert (clearing the filter or changing
						// the filtered id) may end up with a random selection. There appear to be
						// two problems here.
						// 1) (Most common) VirtualTreeControl.ListBoxStateTracker.ApplyChange
						// is not processing absIndex == -1 for a positive count. This is the notification provided
						// when a new item is inserted before the current first item, so this is very bad.
						// 2) (Less common, but bad for us) Subitem changes are not being tracked correctly. I
						// have not tracked this one down fully, but it is likely in either the same routine
						// or VirtualTreeControl.OnToggleExpansion.
						
						// UNDONE: Work around selection tracking bug.
						// There should be two reliable workarounds:
						// 1) Support selection tracking so the control can snapshot the selection and
						// call VirtualTreeControl.SelectObject after the filter changes.
						// 2) Add our own OnItemCountChanged callback to calculate the correct selection
						// offsets and apply the selection change directly (probably more code, but also
						// a much more localized change. This might also help track down the problem
						// in the MS code)
						
						// Incrementally step through the change sets. Note that we must do this
						// carefully and in order because any callbacks for an insert/updatecellstyle must
						// be carefully coordinated to call back at the correct index in the filter.
						int fullItemCount = myChanges.Length;
						int oldItemNextIndex = 0;
						int oldFilterCount;
						int oldItem;
						if (oldFilter == null)
						{
							oldItem = 0;
							oldFilterCount = fullItemCount;
						}
						else
						{
							oldFilterCount = oldFilter.Length;
							oldItem = (oldFilterCount == 0) ? fullItemCount : oldFilter[oldItemNextIndex++];
						}
						int newItemNextIndex = 0;
						int newFilterCount;
						int newItem;
						if (newFilter == null)
						{
							newItem = 0;
							newFilterCount = fullItemCount;
						}
						else
						{
							newFilterCount = newFilter.Length;
							newItem = (newFilterCount == 0) ? fullItemCount : newFilter[newItemNextIndex++];
						}
						int nextIndex = 0;
						for (; ; )
						{
							if (newItem == oldItem)
							{
								if (newItem == fullItemCount)
								{
									break;
								}
								newItem = (newFilter == null) ?
									(newItem + 1) :
									((newItemNextIndex < newFilterCount) ? newFilter[newItemNextIndex++] : fullItemCount);
								oldItem = (oldFilter == null) ?
									(oldItem + 1) :
									((oldItemNextIndex < oldFilterCount) ? oldFilter[oldItemNextIndex++] : fullItemCount);
								++nextIndex;
							}
							else if (newItem < oldItem)
							{
								int startInsert = nextIndex;
								while (newItem < oldItem)
								{
									// Insert numbered item (newItem + 1)
									newItem = (newFilter == null) ?
										(newItem + 1) :
										((newItemNextIndex < newFilterCount) ? newFilter[newItemNextIndex++] : fullItemCount);
									++nextIndex;
								}
								// Insert indexed items: (startInsert + 1) through (nextIndex)

								// insert at startInsert with count nextIndex - startInsert
								modify(this, BranchModificationEventArgs.InsertItems(this, startInsert - 1, nextIndex - startInsert));
								for (int i = 0; i < columnCount; ++i)
								{
									if (0 != (ColumnStyles(i) & SubItemCellStyles.Complex))
									{
										for (int j = startInsert; j < nextIndex; ++j)
										{
											// MSBUG: Hack workaround a bug in VirtualTree.InsertItems. Inserting
											// a complex item should requery the branch for subitem expansions.
											// This isn't as severe as the other (it doesn't crash), but this should
											// still happen automatically.
											modify(this, BranchModificationEventArgs.UpdateCellStyle(this, j, i, true));
										}
									}
								}
							}
							else // oldItem < newItem
							{
								int deleteBound = nextIndex;
								while (oldItem < newItem)
								{
									//Debug.WriteLine("Delete numbered item " + (oldItem + 1).ToString());
									++deleteBound;
									oldItem = (oldFilter == null) ?
										(oldItem + 1) :
										((oldItemNextIndex < oldFilterCount) ? oldFilter[oldItemNextIndex++] : fullItemCount);
								}
								// Delete index items: (nextIndex) through (deleteBound - 1)
								// MSBUG: Hack workaround a bug in VirtualTree.DeleteItems. The
								// call to ChangeFullCountRecursive needs to pass a valid subItemIncr
								// instead of the constant value 0.
								for (int i = 0; i < columnCount; ++i)
								{
									if (0 != (ColumnStyles(i) & SubItemCellStyles.Complex))
									{
										for (int j = nextIndex; j < deleteBound; ++j)
										{
											modify(this, BranchModificationEventArgs.UpdateCellStyle(this, j, i, false));
										}
									}
								}
								modify(this, BranchModificationEventArgs.DeleteItems(this, nextIndex, deleteBound - nextIndex));
							}
						}

						modify(this, BranchModificationEventArgs.DelayRedraw(false));
					}
				}
			}
			#endregion // Filter implementation
			#region DetailHandler implementations
			#region PropertyChangedDetailHandler class
			private class PropertyChangedDetailHandler : IDetailHandler
			{
				private class ChangeToFromBranch : BaseBranch, IBranch
				{
					private ElementPropertyChangedEventArgs myArgs;
					public ChangeToFromBranch(ElementPropertyChangedEventArgs args)
					{
						myArgs = args;
					}
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.None;
						}
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						switch (row)
						{
							case 0:
								retVal.SelectedImage = retVal.Image = IsElement(myArgs.NewValue) ? (short)GlyphIndex.NewElementId : (short)GlyphIndex.NewValue;
								break;
							case 1:
								retVal.SelectedImage = retVal.Image = IsElement(myArgs.OldValue) ? (short)GlyphIndex.OldElementId : (short)GlyphIndex.OldValue;
								retVal.GrayText = true;
								break;
						}
						return retVal;
					}
					string IBranch.GetText(int row, int column)
					{
						return GetValueString((row == 0) ? myArgs.NewValue : myArgs.OldValue);
					}
					string IBranch.GetTipText(int row, int column, ToolTipType tipType)
					{
						if (tipType == ToolTipType.Icon)
						{
							object value = (row == 0) ? myArgs.NewValue : myArgs.OldValue;
							if (IsElement(value))
							{
								return FormatFullIdentifier(((ModelElement)value).Id);
							}
						}
						return null;
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							return 2;
						}
					}
				}
				int IDetailHandler.GetDetailColumnCount(PartitionChange change)
				{
					return 2;
				}
				SubItemCellStyles IDetailHandler.GetDetailColumnStyle(PartitionChange change, int detailColumn)
				{
					return (detailColumn == 0) ? SubItemCellStyles.Simple : SubItemCellStyles.Complex;
				}
				string IDetailHandler.GetDetailText(PartitionChange change, int detailColumn)
				{
					ElementPropertyChangedEventArgs args = (ElementPropertyChangedEventArgs)change.ChangeArgs;
					switch (detailColumn)
					{
						case 0:
							return args.DomainProperty.Name;
					}
					return null;
				}
				string IDetailHandler.GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType)
				{
					return null;
				}
				object IDetailHandler.GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style)
				{
					if (style == ObjectStyle.SubItemRootBranch && detailColumn == 1)
					{
						return new ChangeToFromBranch((ElementPropertyChangedEventArgs)change.ChangeArgs);
					}
					return null;
				}
				bool IDetailHandler.GetDetailIsExpandable(PartitionChange change, int detailColumn)
				{
					return false;
				}
				VirtualTreeDisplayData IDetailHandler.GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					if (detailColumn == 0)
					{
						retVal.SelectedImage = retVal.Image = (short)GlyphIndex.DomainProperty;
						retVal.Bold = true;
					}
					return retVal;
				}
				DomainClassInfo IDetailHandler.GetBaseDomainClassInfo(PartitionChange change)
				{
					ElementPropertyChangedEventArgs args = (ElementPropertyChangedEventArgs)change.ChangeArgs;
					DomainClassInfo propertyClass = args.DomainProperty.DomainClass;
					return (propertyClass != args.DomainClass) ? propertyClass : null;
				}
			}
			#endregion // PropertyChangedDetailHandler class
			#region RoleAssignmentDetailBranch class
			private class RoleAssignmentDetailBranch : BaseBranch, IBranch, IItemFilter
			{
				private DomainRoleInfo myRoleInfo;
				private Guid myElementId;
				public RoleAssignmentDetailBranch(Store store, RoleAssignment assignment)
				{
					myRoleInfo = store.DomainDataDirectory.FindDomainRole(assignment.DomainRoleId);
					myElementId = assignment.RolePlayer.Id;
				}
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.None;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					switch (row)
					{
						case 0:
							return myRoleInfo.Name;
						case 1:
							return FormatIdentifier(myElementId);
					}
					return null;
				}
				string IBranch.GetTipText(int row, int column, ToolTipType tipType)
				{
					if (row == 1 && tipType == ToolTipType.Icon)
					{
						return FormatFullIdentifier(myElementId);
					}
					return null;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					switch (row)
					{
						case 0:
							retVal.SelectedImage = retVal.Image = myRoleInfo.IsSource ? (short)GlyphIndex.SourceDomainRole : (short)GlyphIndex.TargetDomainRole;
							retVal.Bold = true;
							break;
						case 1:
							retVal.SelectedImage = retVal.Image = (short)GlyphIndex.ElementId;
							break;
					}
					return retVal;
				}
				int IBranch.VisibleItemCount
				{
					get
					{
						return 2;
					}
				}
				void IItemFilter.FilterItem(PartitionChangeFilter filter, int row, int column)
				{
					if (row == 1)
					{
						filter.ElementId = myElementId;
					}
				}
			}
			#endregion // RoleAssignmentDetailBranch class
			#region AddDetailHandler class
			private class AddDetailHandler : IDetailHandler
			{
				private class InitialPropertyExpansionBranch : BaseBranch, IBranch, IMultiColumnBranch
				{
					private PropertyAssignment[] myAssignments;
					private DomainDataDirectory myDirectory;
					public InitialPropertyExpansionBranch(Store store, PropertyAssignment[] assignments)
					{
						myDirectory = store.DomainDataDirectory;
						myAssignments = assignments;
					}
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.None;
						}
					}
					string IBranch.GetText(int row, int column)
					{
						PropertyAssignment assignment = myAssignments[row];
						switch (column)
						{
							case 0:
								return myDirectory.GetDomainProperty(assignment.PropertyId).Name;
							case 1:
								return GetValueString(assignment.Value);
						}
						return null;
					}
					string IBranch.GetTipText(int row, int column, ToolTipType tipType)
					{
						if (column == 1 && tipType == ToolTipType.Icon)
						{
							object value = myAssignments[row].Value;
							if (IsElement(value))
							{
								return FormatFullIdentifier(((ModelElement)value).Id);
							}
						}
						return null;
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						switch (column)
						{
							case 0:
								retVal.SelectedImage = retVal.Image = (short)GlyphIndex.DomainProperty;
								retVal.Bold = true;
								break;
							case 1:
								if (IsElement(myAssignments[row].Value))
								{
									retVal.SelectedImage = retVal.Image = (short)GlyphIndex.ElementId;
								}
								break;
						}
						if (column == 0)
						{
						}
						return retVal;
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							return myAssignments.Length;
						}
					}
					int IMultiColumnBranch.ColumnCount
					{
						get
						{
							return 2; 
						}
					}

					SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
					{
						return SubItemCellStyles.Simple;
					}

					int IMultiColumnBranch.GetJaggedColumnCount(int row)
					{
						return 2;
					}
				}
				int IDetailHandler.GetDetailColumnCount(PartitionChange change)
				{
					return (change.RoleAssignments != null) ? 2 : 0;
				}
				SubItemCellStyles IDetailHandler.GetDetailColumnStyle(PartitionChange change, int detailColumn)
				{
					return SubItemCellStyles.Complex;
				}
				string IDetailHandler.GetDetailText(PartitionChange change, int detailColumn)
				{
					return null;
				}
				string IDetailHandler.GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType)
				{
					return null;
				}
				object IDetailHandler.GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style)
				{
					if (style == ObjectStyle.ExpandedBranch && detailColumn == -1)
					{
						return new InitialPropertyExpansionBranch(change.Partition.Store, ((ElementAddedEventArgs)change.ChangeArgs).GetAssignments());
					}
					else if (style == ObjectStyle.SubItemRootBranch)
					{
						ElementAddedEventArgs args = (ElementAddedEventArgs)change.ChangeArgs;
						DomainRelationshipInfo relationshipInfo = (DomainRelationshipInfo)args.DomainClass;
						RoleAssignment[] assignments = change.RoleAssignments;
						RoleAssignment assignment = assignments[0];
						return new RoleAssignmentDetailBranch(change.Partition.Store, relationshipInfo.DomainRoles[detailColumn].Id == assignment.DomainRoleId ? assignment : assignments[1]);
					}
					return null;
				}
				bool IDetailHandler.GetDetailIsExpandable(PartitionChange change, int detailColumn)
				{
					return detailColumn == -1 && ((ElementAddedEventArgs)change.ChangeArgs).GetAssignments().Length != 0;
				}
				VirtualTreeDisplayData IDetailHandler.GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData)
				{
					return VirtualTreeDisplayData.Empty;
				}
				DomainClassInfo IDetailHandler.GetBaseDomainClassInfo(PartitionChange change)
				{
					return null;
				}
			}
			#endregion // AddDetailHandler class
			#region DeleteDetailHandler class
			private class DeleteDetailHandler : IDetailHandler
			{
				int IDetailHandler.GetDetailColumnCount(PartitionChange change)
				{
					return (change.RoleAssignments != null) ? 2 : 0;
				}
				SubItemCellStyles IDetailHandler.GetDetailColumnStyle(PartitionChange change, int detailColumn)
				{
					return SubItemCellStyles.Complex;
				}
				string IDetailHandler.GetDetailText(PartitionChange change, int detailColumn)
				{
					return null;
				}
				string IDetailHandler.GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType)
				{
					return null;
				}
				object IDetailHandler.GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style)
				{
					if (style == ObjectStyle.SubItemRootBranch)
					{
						ElementEventArgs args = (ElementEventArgs)change.ChangeArgs;
						DomainRelationshipInfo relationshipInfo = (DomainRelationshipInfo)args.DomainClass;
						RoleAssignment[] assignments = change.RoleAssignments;
						RoleAssignment assignment = assignments[0];
						return new RoleAssignmentDetailBranch(change.Partition.Store, relationshipInfo.DomainRoles[detailColumn].Id == assignment.DomainRoleId ? assignment : assignments[1]);
					}
					return null;
				}
				bool IDetailHandler.GetDetailIsExpandable(PartitionChange change, int detailColumn)
				{
					return false;
				}
				VirtualTreeDisplayData IDetailHandler.GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData)
				{
					return VirtualTreeDisplayData.Empty;
				}
				DomainClassInfo IDetailHandler.GetBaseDomainClassInfo(PartitionChange change)
				{
					return null;
				}
			}
			#endregion // AddDetailHandler class
			#region RolePlayerChangeDetailHandler class
			private class RolePlayerChangeDetailHandler : IDetailHandler
			{
				#region RolePlayerChangeDetailBranch class
				private class RolePlayerChangeDetailBranch : BaseBranch, IBranch, IItemFilter
				{
					private DomainRoleInfo myRoleInfo;
					private Guid myOldId;
					private Guid myNewId;
					public RolePlayerChangeDetailBranch(DomainRoleInfo role, Guid oldRolePlayerId, Guid newRolePlayerId)
					{
						myRoleInfo = role;
						myOldId = oldRolePlayerId;
						myNewId = newRolePlayerId;
					}
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.None;
						}
					}
					string IBranch.GetText(int row, int column)
					{
						switch (row)
						{
							case 0:
								return myRoleInfo.Name;
							case 1:
								return FormatIdentifier(myNewId);
							case 2:
								return FormatIdentifier(myOldId);
						}
						return null;
					}
					string IBranch.GetTipText(int row, int column, ToolTipType tipType)
					{
						if (tipType == ToolTipType.Icon)
						{
							switch (row)
							{
								case 1:
									return FormatFullIdentifier(myNewId);
								case 2:
									return FormatFullIdentifier(myOldId);
							}
						}
						return null;
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						switch (row)
						{
							case 0:
								retVal.SelectedImage = retVal.Image = myRoleInfo.IsSource ? (short)GlyphIndex.SourceDomainRole : (short)GlyphIndex.TargetDomainRole;
								retVal.Bold = true;
								break;
							case 1:
								retVal.SelectedImage = retVal.Image = (short)GlyphIndex.NewElementId;
								break;
							case 2:
								retVal.SelectedImage = retVal.Image = (short)GlyphIndex.OldElementId;
								retVal.GrayText = true;
								break;
						}
						return retVal;
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							return 3;
						}
					}
					void IItemFilter.FilterItem(PartitionChangeFilter filter, int row, int column)
					{
						switch (row)
						{
							case 1:
								filter.ElementId = myNewId;
								break;
							case 2:
								filter.ElementId = myOldId;
								break;
						}
					}
				}
				#endregion // RolePlayerChangeDetailBranch class
				int IDetailHandler.GetDetailColumnCount(PartitionChange change)
				{
					return 2;
				}
				SubItemCellStyles IDetailHandler.GetDetailColumnStyle(PartitionChange change, int detailColumn)
				{
					return (detailColumn == (((RolePlayerChangedEventArgs)change.ChangeArgs).DomainRole.IsSource ? 0 : 1)) ? SubItemCellStyles.Complex : SubItemCellStyles.Simple;
				}
				string IDetailHandler.GetDetailText(PartitionChange change, int detailColumn)
				{
					// Return the role player name of the opposite role. Unfortunately, we don't actually
					// have the information about the current opposite role player from the transaction log
					return ((RolePlayerChangedEventArgs)change.ChangeArgs).DomainRole.OppositeDomainRole.Name;
				}
				string IDetailHandler.GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType)
				{
					return null;
				}
				object IDetailHandler.GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style)
				{
					if (style == ObjectStyle.SubItemRootBranch)
					{
						RolePlayerChangedEventArgs args = (RolePlayerChangedEventArgs)change.ChangeArgs;
						return new RolePlayerChangeDetailBranch(args.DomainRole, args.OldRolePlayerId, args.NewRolePlayerId);
					}
					return null;
				}
				bool IDetailHandler.GetDetailIsExpandable(PartitionChange change, int detailColumn)
				{
					return false;
				}
				VirtualTreeDisplayData IDetailHandler.GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					retVal.SelectedImage = retVal.Image = ((RolePlayerChangedEventArgs)change.ChangeArgs).DomainRole.OppositeDomainRole.IsSource ? (short)GlyphIndex.SourceDomainRole : (short)GlyphIndex.TargetDomainRole;
					retVal.Bold = true;
					return retVal;
				}
				DomainClassInfo IDetailHandler.GetBaseDomainClassInfo(PartitionChange change)
				{
					return null;
				}
			}
			#endregion // RolePlayerChangeDetailHandler class
			#region RolePlayerOrderChangeDetailHandler class
			private class RolePlayerOrderChangeDetailHandler : IDetailHandler
			{
				#region CounterpartRolePlayerDetailBranch class
				private class CounterpartRolePlayerDetailBranch : BaseBranch, IBranch, IItemFilter
				{
					private DomainRoleInfo myRoleInfo;
					private Guid myId;
					public CounterpartRolePlayerDetailBranch(DomainRoleInfo counterpartRole, Guid counterpartElementId)
					{
						myRoleInfo = counterpartRole;
						myId = counterpartElementId;
					}
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.None;
						}
					}
					string IBranch.GetText(int row, int column)
					{
						switch (row)
						{
							case 0:
								return myRoleInfo.Name;
							case 1:
								return FormatIdentifier(myId);
						}
						return null;
					}
					string IBranch.GetTipText(int row, int column, ToolTipType tipType)
					{
						if (row == 1 && tipType == ToolTipType.Icon)
						{
							return FormatFullIdentifier(myId);
						}
						return null;
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						switch (row)
						{
							case 0:
								retVal.SelectedImage = retVal.Image = myRoleInfo.IsSource ? (short)GlyphIndex.SourceDomainRole : (short)GlyphIndex.TargetDomainRole;
								retVal.Bold = true;
								break;
							case 1:
								retVal.SelectedImage = retVal.Image = (short)GlyphIndex.ElementId;
								break;
						}
						return retVal;
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							return 2;
						}
					}
					void IItemFilter.FilterItem(PartitionChangeFilter filter, int row, int column)
					{
						if (row == 1)
						{
							filter.ElementId = myId;
						}
					}
				}
				#endregion // CounterpartRolePlayerDetailBranch class
				#region PositionDetailBranch class
				private class PositionDetailBranch : BaseBranch, IBranch
				{
					private string myNewPosition;
					private string myOldPosition;
					public PositionDetailBranch(int newPosition, int oldPosition)
					{
						myNewPosition = newPosition.ToString(CultureInfo.CurrentCulture);
						myOldPosition = oldPosition.ToString(CultureInfo.CurrentCulture);
					}
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.None;
						}
					}
					string IBranch.GetText(int row, int column)
					{
						return (row == 0) ? myNewPosition : myOldPosition;
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						switch (row)
						{
							case 0:
								retVal.SelectedImage = retVal.Image = (short)GlyphIndex.NewValue;
								break;
							case 1:
								retVal.SelectedImage = retVal.Image = (short)GlyphIndex.OldValue;
								retVal.GrayText = true;
								break;
						}
						return retVal;
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							return 2;
						}
					}
				}
				#endregion // PositionDetailBranch class
				int IDetailHandler.GetDetailColumnCount(PartitionChange change)
				{
					return 2;
				}
				SubItemCellStyles IDetailHandler.GetDetailColumnStyle(PartitionChange change, int detailColumn)
				{
					return SubItemCellStyles.Complex;
				}
				string IDetailHandler.GetDetailText(PartitionChange change, int detailColumn)
				{
					return null;
				}
				string IDetailHandler.GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType)
				{
					return null;
				}
				object IDetailHandler.GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style)
				{
					if (style == ObjectStyle.SubItemRootBranch)
					{
						RolePlayerOrderChangedEventArgs args = (RolePlayerOrderChangedEventArgs)change.ChangeArgs;
						return (detailColumn == 0) ?
							(IBranch)(new CounterpartRolePlayerDetailBranch(args.CounterpartDomainRole, args.CounterpartRolePlayerId)) :
							new PositionDetailBranch(args.NewOrdinal, args.OldOrdinal);
					}
					return null;
				}
				bool IDetailHandler.GetDetailIsExpandable(PartitionChange change, int detailColumn)
				{
					return false;
				}
				VirtualTreeDisplayData IDetailHandler.GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData)
				{
					return VirtualTreeDisplayData.Empty;
				}
				DomainClassInfo IDetailHandler.GetBaseDomainClassInfo(PartitionChange change)
				{
					return null;
				}
			}
			#endregion // RolePlayerOrderChangeDetailHandler class
			#region RedirectDetailHandler class
			private class RedirectDetailHandler : IDetailHandler
			{
				private Dictionary<Type, IDetailHandler> myDetailHandlers;
				private RedirectDetailHandler()
				{
					Dictionary<Type, IDetailHandler> handlers = new Dictionary<Type, IDetailHandler>();
					handlers[typeof(ElementPropertyChangedEventArgs)] = new PropertyChangedDetailHandler();
					handlers[typeof(ElementAddedEventArgs)] = new AddDetailHandler();
					DeleteDetailHandler deleteHandler = new DeleteDetailHandler();
					handlers[typeof(ElementDeletedEventArgs)] = deleteHandler;
					handlers[typeof(ElementDeletingEventArgs)] = deleteHandler;
					handlers[typeof(RolePlayerChangedEventArgs)] = new RolePlayerChangeDetailHandler();
					handlers[typeof(RolePlayerOrderChangedEventArgs)] = new RolePlayerOrderChangeDetailHandler();
					// Add additional typed handlers here
					myDetailHandlers = handlers;
				}
				public static IDetailHandler Instance = new RedirectDetailHandler();
				int IDetailHandler.GetDetailColumnCount(PartitionChange change)
				{
					IDetailHandler typedHandler;
					if (myDetailHandlers.TryGetValue(change.ChangeArgs.GetType(), out typedHandler))
					{
						return typedHandler.GetDetailColumnCount(change);
					}
					return 0;
				}
				SubItemCellStyles IDetailHandler.GetDetailColumnStyle(PartitionChange change, int detailColumn)
				{
					return myDetailHandlers[change.ChangeArgs.GetType()].GetDetailColumnStyle(change, detailColumn);
				}
				string IDetailHandler.GetDetailText(PartitionChange change, int detailColumn)
				{
					return myDetailHandlers[change.ChangeArgs.GetType()].GetDetailText(change, detailColumn);
				}
				string IDetailHandler.GetDetailTipText(PartitionChange change, int detailColumn, ToolTipType tipType)
				{
					return myDetailHandlers[change.ChangeArgs.GetType()].GetDetailTipText(change, detailColumn, tipType);
				}
				object IDetailHandler.GetDetailObject(PartitionChange change, int detailColumn, ObjectStyle style)
				{
					return myDetailHandlers[change.ChangeArgs.GetType()].GetDetailObject(change, detailColumn, style);
				}
				bool IDetailHandler.GetDetailIsExpandable(PartitionChange change, int detailColumn)
				{
					IDetailHandler typedHandler;
					if (detailColumn == -1)
					{
						if (myDetailHandlers.TryGetValue(change.ChangeArgs.GetType(), out typedHandler))
						{
							return typedHandler.GetDetailIsExpandable(change, detailColumn);
						}
					}
					else
					{
						return myDetailHandlers[change.ChangeArgs.GetType()].GetDetailIsExpandable(change, detailColumn);
					}
					return false;
				}
				VirtualTreeDisplayData IDetailHandler.GetDetailDisplayData(PartitionChange change, int detailColumn, VirtualTreeDisplayDataMasks requiredData)
				{
					return myDetailHandlers[change.ChangeArgs.GetType()].GetDetailDisplayData(change, detailColumn, requiredData);
				}
				DomainClassInfo IDetailHandler.GetBaseDomainClassInfo(PartitionChange change)
				{
					return myDetailHandlers[change.ChangeArgs.GetType()].GetBaseDomainClassInfo(change);
				}
			}
			#endregion // RedirectDetailHandler class
			#endregion // DetailHandler implementations
			#region Member variables
			private PartitionChange[] myChanges;
			private int[] myFilter;
			private BranchModificationEventHandler myModify;
			#endregion // Member variables
			#region Constructor
			public ChangeBranch(TransactionItem transactionItem)
			{
				myChanges = PartitionChange.GetPartitionChanges(transactionItem);
			}
			#endregion // Constructor
			#region IBranch Implementation
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.ComplexColumns | BranchFeatures.Expansions | BranchFeatures.InsertsAndDeletes;
				}
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.ExpandedBranch:
						if (column == (int)ColumnContent.ChangeType)
						{
							return RedirectDetailHandler.Instance.GetDetailObject(GetChange(row), -1, style);
						}
						break;
					case ObjectStyle.SubItemExpansion:
					case ObjectStyle.SubItemRootBranch:
						PartitionChange change = GetChange(row);
						switch ((ColumnContent)column)
						{
							case ColumnContent.Class:
								if (style == ObjectStyle.SubItemRootBranch)
								{
									GenericEventArgs genericArgs;
									DomainClassInfo classInfo;
									DomainClassInfo baseClassInfo;
									if (null != (baseClassInfo = RedirectDetailHandler.Instance.GetBaseDomainClassInfo(change)) &&
										null != (genericArgs = change.ChangeArgs as GenericEventArgs) &&
										null != (classInfo = genericArgs.DomainClass))
									{
										return new ClassDetailBranch(baseClassInfo, classInfo);
									}
								}
								break;
							default:
								column -= (int)ColumnContent.Detail1;
								if (RedirectDetailHandler.Instance.GetDetailColumnCount(change) > column &&
									((style == ObjectStyle.SubItemRootBranch) ? SubItemCellStyles.Complex : SubItemCellStyles.Expandable) == RedirectDetailHandler.Instance.GetDetailColumnStyle(change, column))
								{
									return RedirectDetailHandler.Instance.GetDetailObject(change, column, style);
								}
								break;
						}
						break;
				}
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				string retVal = null;
				PartitionChange change = GetChange(row);
				switch ((ColumnContent)column)
				{
					case ColumnContent.ChangeType:
						retVal = change.ChangeArgs.GetType().Name;
						retVal = retVal.Substring(0, retVal.Length - 9);
						if (retVal.StartsWith("Element"))
						{
							retVal = retVal.Substring(7);
						}
						break;
					case ColumnContent.Index:
						int[] filter = myFilter;
						retVal = (filter != null) ? (filter[row] + 1).ToString(CultureInfo.CurrentCulture) : (row + 1).ToString(CultureInfo.CurrentCulture);
						break;
					case ColumnContent.Partition:
						{
							Partition partition;
							if (null != (partition = change.Partition))
							{
								object alternateId;
								if (null != (alternateId = partition.AlternateId))
								{
									retVal = alternateId.ToString();
								}
								else if (partition == partition.Store.DefaultPartition)
								{
									retVal = "Default";
								}
							}
						}
						break;
					case ColumnContent.ChangeSource:
						{
							GenericEventArgs genericArgs;
							if (null != (genericArgs = change.ChangeArgs as GenericEventArgs))
							{
								retVal = Enum.GetName(typeof(ChangeSource), genericArgs.ChangeSource);
							}
						}
						break;
					case ColumnContent.Model:
						{
							GenericEventArgs genericArgs;
							DomainModelInfo modelInfo;
							if (null != (genericArgs = change.ChangeArgs as GenericEventArgs) &&
								null != (modelInfo = genericArgs.DomainModel))
							{
								DomainClassInfo baseDomainClass = RedirectDetailHandler.Instance.GetBaseDomainClassInfo(change);
								DomainModelInfo baseModelInfo;
								if (baseDomainClass != null && (baseModelInfo = baseDomainClass.DomainModel) != modelInfo)
								{
									modelInfo = baseModelInfo;
								}
								retVal = modelInfo.Name;
							}
						}
						break;
					case ColumnContent.Class:
						{
							GenericEventArgs genericArgs;
							DomainClassInfo classInfo;
							if (null != (genericArgs = change.ChangeArgs as GenericEventArgs) &&
								null != (classInfo = genericArgs.DomainClass))
							{
								retVal = classInfo.Name;
							}
						}
						break;
					case ColumnContent.Id:
						{
							GenericEventArgs genericArgs;
							Guid elementId;
							if (null != (genericArgs = change.ChangeArgs as GenericEventArgs) &&
								(elementId = genericArgs.ElementId) != Guid.Empty)
							{
								retVal = FormatIdentifier(elementId);
							}
						}
						break;
					case ColumnContent.Detail1:
					case ColumnContent.Detail2:
						column -= (int)ColumnContent.Detail1;
						if (RedirectDetailHandler.Instance.GetDetailColumnCount(change) > column)
						{
							retVal = RedirectDetailHandler.Instance.GetDetailText(change, column);
						}
						break;
				}
				return retVal;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				switch ((ColumnContent)column)
				{
					case ColumnContent.Id:
						GenericEventArgs genericArgs;
						Guid elementId;
						if (null != (genericArgs = GetChange(row).ChangeArgs as GenericEventArgs) &&
							(elementId = genericArgs.ElementId) != Guid.Empty)
						{
							return FormatFullIdentifier(elementId);
						}
						break;
					case ColumnContent.Detail1:
					case ColumnContent.Detail2:
						column -= (int)ColumnContent.Detail1;
						PartitionChange change = GetChange(row);
						if (RedirectDetailHandler.Instance.GetDetailColumnCount(change) > column)
						{
							return RedirectDetailHandler.Instance.GetDetailTipText(change, column, tipType);
						}
						break;
				}
				return null;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				switch ((ColumnContent)column)
				{
					case ColumnContent.Index:
						retVal.Bold = true;
						retVal.GrayText = true;
						break;
					case ColumnContent.Partition:
						retVal.SelectedImage = retVal.Image = (short)GlyphIndex.Partition;
						{
							Partition partition = GetChange(row).Partition;
							if (partition != null && partition == partition.Store.DefaultPartition)
							{
								retVal.GrayText = true;
							}
						}
						break;
					case ColumnContent.Class:
						{
							GenericEventArgs args = GetChange(row).ChangeArgs as GenericEventArgs;
							retVal.SelectedImage = retVal.Image = (args.DomainClass is DomainRelationshipInfo) ? (short)GlyphIndex.DomainRelationship : (short)GlyphIndex.DomainClass;
							retVal.Bold = true;
						}
						break;
					case ColumnContent.Model:
						retVal.SelectedImage = retVal.Image = (short)GlyphIndex.DomainModel;
						retVal.Bold = true;
						break;
					case ColumnContent.Id:
						retVal.SelectedImage = retVal.Image = (short)GlyphIndex.ElementId;
						break;
					case ColumnContent.Detail1:
					case ColumnContent.Detail2:
						column -= (int)ColumnContent.Detail1;
						PartitionChange change = GetChange(row);
						if (RedirectDetailHandler.Instance.GetDetailColumnCount(change) > column)
						{
							retVal = RedirectDetailHandler.Instance.GetDetailDisplayData(change, column, requiredData);
						}
						break;
				}
				return retVal;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				switch ((ColumnContent)column)
				{
					case ColumnContent.ChangeType:
						return RedirectDetailHandler.Instance.GetDetailIsExpandable(GetChange(row), -1);
					case ColumnContent.Detail1:
					case ColumnContent.Detail2:
						column -= (int)ColumnContent.Detail1;
						PartitionChange change = GetChange(row);
						if (RedirectDetailHandler.Instance.GetDetailColumnCount(change) > column)
						{
							return RedirectDetailHandler.Instance.GetDetailIsExpandable(change, column);
						}
						break;
				}
				return false;
			}
			private int VisibleItemCount
			{
				get
				{
					int[] filter = myFilter;
					return (filter != null) ? filter.Length : myChanges.Length;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return VisibleItemCount;
				}
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add { myModify += value; }
				remove { myModify -= value; }
			}
			#endregion // IBranch Implementation
			#region IMultiColumnBranch Implementation
			int IMultiColumnBranch.ColumnCount
			{
				get
				{
					return (int)ColumnContent.Count;
				}
			}
			private SubItemCellStyles ColumnStyles(int column)
			{
				switch ((ColumnContent)column)
				{
					case ColumnContent.Detail1:
					case ColumnContent.Detail2:
						return SubItemCellStyles.Mixed | SubItemCellStyles.Simple;
					case ColumnContent.Class:
						return SubItemCellStyles.Complex | SubItemCellStyles.Simple;
					default:
						return SubItemCellStyles.Simple;
				}
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return ColumnStyles(column);
			}
			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				return (int)ColumnContent.Count;
			}
			#endregion // IMultiColumnBranch Implementation
			#region IItemFilter Implementation
			void IItemFilter.FilterItem(PartitionChangeFilter filter, int row, int column)
			{
				GenericEventArgs args;
				PartitionChange change = GetChange(row);
				switch ((ColumnContent)column)
				{
					case ColumnContent.Partition:
						Partition partition = change.Partition;
						if (partition != null)
						{
							filter.Partition = partition;
						}
						break;
					case ColumnContent.Model:
						DomainModelInfo model;
						if (null != (args = change.ChangeArgs as GenericEventArgs) &&
							null != (model = args.DomainModel))
						{
							DomainClassInfo baseDomainClass = RedirectDetailHandler.Instance.GetBaseDomainClassInfo(change);
							DomainModelInfo baseModelInfo;
							if (baseDomainClass != null && (baseModelInfo = baseDomainClass.DomainModel) != model)
							{
								model = baseModelInfo;
							}
							filter.DomainModel = model;
						}
						break;
					case ColumnContent.Class:
						DomainClassInfo classInfo;
						if (null != (args = change.ChangeArgs as GenericEventArgs) &&
							null != (classInfo = args.DomainClass))
						{
							filter.DomainClass = classInfo;
						}
						break;
					case ColumnContent.ChangeSource:
						if (null != (args = change.ChangeArgs as GenericEventArgs))
						{
							filter.ChangeSource = args.ChangeSource;
						}
						break;
					case ColumnContent.Id:
						if (null != (args = change.ChangeArgs as GenericEventArgs))
						{
							Guid id = args.ElementId;
							if (id != Guid.Empty)
							{
								filter.ElementId = id;
							}
						}
						break;
					case ColumnContent.ChangeType:
						filter.ChangeType = change.ChangeArgs.GetType();
						break;
					case ColumnContent.Detail1:
						ElementPropertyChangedEventArgs propertyChangedArgs = change.ChangeArgs as ElementPropertyChangedEventArgs;
						if (null != propertyChangedArgs)
						{
							filter.DomainProperty = propertyChangedArgs.DomainProperty;
						}
						break;
				}
			}
			#endregion // IItemFilter Implementation
			#region ClassDetailBranch class
			private class ClassDetailBranch : BaseBranch, IBranch, IItemFilter
			{
				#region Member variables and constructor
				private DomainClassInfo[] myDomainClasses;
				public ClassDetailBranch(DomainClassInfo baseDomainClass, DomainClassInfo instanceDomainClass)
				{
					myDomainClasses = new DomainClassInfo[] { baseDomainClass, instanceDomainClass };
				}
				#endregion // Member variables and constructor
				#region IBranch Implementation
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.None;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					return myDomainClasses[row].Name;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					retVal.Bold = true;
					if (row == 0)
					{
						retVal.SelectedImage = retVal.Image = (myDomainClasses[row] is DomainRelationshipInfo) ? (short)GlyphIndex.DomainRelationship : (short)GlyphIndex.DomainClass;
					}
					else
					{
						retVal.SelectedImage = retVal.Image = (short)GlyphIndex.Blank;
						retVal.GrayText = true;
					}
					return retVal;
				}
				int IBranch.VisibleItemCount
				{
					get
					{
						return 2;
					}
				}
				#endregion // IBranch Implementation
				#region IItemFilter Implementation
				void IItemFilter.FilterItem(PartitionChangeFilter filter, int row, int column)
				{
					filter.DomainClass = myDomainClasses[row];
				}
				#endregion // IItemFilter Implementation
			}
			#endregion // ClassDetailBranch class
		}
		#endregion // BaseBranch class
		#region Event Handlers
		private void UndoItemsCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = UndoItemsCombo.SelectedIndex;
			if (index >= 0)
			{
				RedoItemsCombo.SelectedIndex = -1;
				ResetTree(myUndoItems[index]);
			}
		}
		private void RedoItemsCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = RedoItemsCombo.SelectedIndex;
			if (index >= 0)
			{
				UndoItemsCombo.SelectedIndex = -1;
				ResetTree(myRedoItems[index]);
			}
		}
		private void TransactionLogViewer_FormClosed(object sender, FormClosedEventArgs e)
		{
			TreeControl.Tree = null;
			UndoItemsCombo.Items.Clear();
			RedoItemsCombo.Items.Clear();
			myRedoItems = null;
			myUndoItems = null;
			myFilter = null;
		}
		private void ResetTree(TransactionItem transactionItem)
		{
			// MSBUG: We should be able to reset the Root branch with the
			// tree attached to the control, but it is losing subitem expansions
			// and causes all sorts of drawing problems, so detach and reattach
			// the tree.
			VirtualTreeControl control = TreeControl;
			ITree tree = control.Tree;
			control.Tree = null;
			myFilter = null;
			CancelButton = CloseButton;
			ClearFilterButton.Enabled = false;
			tree.Root = new ChangeBranch(transactionItem);
			control.MultiColumnTree = (IMultiColumnTree)tree;
		}
		private void TransactionLogViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (mySkipClose)
			{
				mySkipClose = false;
				e.Cancel = true;
			}
			else
			{
				// IUIService.Show display the window in normal state. All is lost
				// if its size settings are set at Maximized.
				WindowState = FormWindowState.Normal;
			}
		}
		private void TreeControl_DoubleClick(object sender, DoubleClickEventArgs e)
		{
			VirtualTreeItemInfo itemInfo = e.ItemInfo;
			IItemFilter itemFilter;
			if (!itemInfo.Blank &&
				null != (itemFilter = itemInfo.Branch as IItemFilter))
			{
				PartitionChangeFilter filter = myFilter;
				bool filterWasClear = true;
				if (filter == null)
				{
					filter = new PartitionChangeFilter();
					myFilter = filter;
				}
				else
				{
					filterWasClear = filter.IsClear;
				}
				itemFilter.FilterItem(filter, itemInfo.Row, itemInfo.Column);
				if (filter.FilterChanged)
				{
					(TreeControl.Tree.Root as ChangeBranch).FilterChanged(filter);
					if (filterWasClear != filter.IsClear)
					{
						if (filterWasClear)
						{
							ClearFilterButton.Enabled = true;
							CancelButton = ClearFilterButton;
						}
						else
						{
							ClearFilterButton.Enabled = false;
							CancelButton = CloseButton;
						}
					}
					e.Handled = true;
				}
			}
		}
		private void ClearFilterButton_Click(object sender, EventArgs e)
		{
			PartitionChangeFilter filter = myFilter;
			if (filter != null)
			{
				filter.ClearFilter();
				if (filter.FilterChanged)
				{
					(TreeControl.Tree.Root as ChangeBranch).FilterChanged(filter);
				}
				mySkipClose = CancelButton == ClearFilterButton;
				ClearFilterButton.Enabled = false;
				CancelButton = CloseButton;
			}
		}
		private void CloseButton_Click(object sender, EventArgs e)
		{
			if (CancelButton != CloseButton)
			{
				Close();
			}
		}
		#endregion // Event Handlers
	}
}
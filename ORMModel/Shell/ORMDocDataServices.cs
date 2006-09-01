#region Common Public License Copyright Notice
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
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using StoreUndoManager = Microsoft.VisualStudio.Modeling.UndoManager;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using ShellUndoManager = Microsoft.VisualStudio.Modeling.Shell.UndoManager;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ORMDesignerDocData : IORMToolServices
	{
		#region SurveyTreeSetup
		private ITree mySurveyTree = null;
		private MainList myRootBranch;
		/// <summary>
		/// property to return the survey tree associated with this DocData
		/// </summary>
		public ITree SurveyTree
		{
			get
			{
				if (mySurveyTree == null)
				{
					mySurveyTree = new VirtualTree();
					IList<ISurveyNodeProvider> nodeProviderList = new List<ISurveyNodeProvider>();
					IList<ISurveyQuestionProvider> questionProviderList = new List<ISurveyQuestionProvider>();
					ICollection<DomainModel> domainModels = this.Store.DomainModels;
					foreach (DomainModel domainModel in domainModels)
					{
						ISurveyNodeProvider nodeProvider = domainModel as ISurveyNodeProvider;
						if (nodeProvider != null)
						{
							nodeProviderList.Add(nodeProvider);
						}
						ISurveyQuestionProvider questionProvider = domainModel as ISurveyQuestionProvider;
						if (questionProvider != null)
						{
							questionProviderList.Add(questionProvider);
						}
						IORMModelEventSubscriber eventSubscriber = domainModel as IORMModelEventSubscriber;
						if (eventSubscriber != null)
						{
							eventSubscriber.SurveyQuestionLoad();
						}
					}
					myRootBranch = new MainList(nodeProviderList, questionProviderList);
					mySurveyTree.Root = myRootBranch.RootBranch;
				}
				return mySurveyTree;
			}
		}

		#endregion //SurveyTreeSetup
		#region Store services passthrough
		/// <summary>
		/// Create a store that implements IORMToolServices by deferring
		/// all methods to this document.
		/// </summary>
		protected override Store CreateStore()
		{
			return new ORMStore(this, ServiceProvider);
		}
		/// <summary>
		/// A store implementation that defers all services to the
		/// owning document.
		/// </summary>
		protected class ORMStore : Store, IORMToolServices
		{
			#region Member Variables
			private IORMToolServices myServices;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new store
			/// </summary>
			/// <param name="services">IORMToolServices to defer to</param>
			/// <param name="serviceProvider">Global service provider</param>
			public ORMStore(IORMToolServices services, IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				myServices = services;
			}
			#endregion // Constructors
			#region IORMToolServices Implementation
			/// <summary>
			/// Defer to TaskProvider on the document. Implements
			/// IORMToolServices.TaskProvider
			/// </summary>
			protected IORMToolTaskProvider TaskProvider
			{
				get
				{
					return myServices.TaskProvider;
				}
			}
			IORMToolTaskProvider IORMToolServices.TaskProvider
			{
				get
				{
					return TaskProvider;
				}
			}
			/// <summary>
			/// Defer to ColorService on the document. Implements
			/// IORMToolServices.ColorService
			/// </summary>
			protected IORMFontAndColorService FontAndColorService
			{
				get
				{
					return myServices.FontAndColorService;
				}
			}
			IORMFontAndColorService IORMToolServices.FontAndColorService
			{
				get
				{
					return FontAndColorService;
				}
			}
			/// <summary>
			/// Defer to ServiceProvider on the document. Implements
			/// IORMToolServices.ServiceProvider
			/// </summary>
			protected IServiceProvider ServiceProvider
			{
				get
				{
					return myServices.ServiceProvider;
				}
			}
			IServiceProvider IORMToolServices.ServiceProvider
			{
				get
				{
					return ServiceProvider;
				}
			}
			/// <summary>
			/// Defer to VerbalizationSnippetsDictionary on the document. Implements
			/// IORMToolServices.VerbalizationSnippetsDictionary
			/// </summary>
			protected IDictionary<Type, IVerbalizationSets> VerbalizationSnippetsDictionary
			{
				get
				{
					return myServices.VerbalizationSnippetsDictionary;
				}
			}
			IDictionary<Type, IVerbalizationSets> IORMToolServices.VerbalizationSnippetsDictionary
			{
				get
				{
					return VerbalizationSnippetsDictionary;
				}
			}
			/// <summary>
			/// currently unimplemented as this should only be returned from ORMDocDataServices
			/// </summary>
			protected INotifySurveyElementChanged NotifySurveyElementChanged
			{
				get
				{
					return myServices.NotifySurveyElementChanged;
				}
			}
			INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
			{
				get
				{
					return NotifySurveyElementChanged;
				}
			}
			/// <summary>
			/// Defer to CanAddTransaction on the document. Implements
			/// IORMToolServices.CanAddTransaction
			/// </summary>
			protected bool CanAddTransaction
			{
				get
				{
					return myServices.CanAddTransaction;
				}
				set
				{
					myServices.CanAddTransaction = value;
				}
			}
			bool IORMToolServices.CanAddTransaction
			{
				get
				{
					return CanAddTransaction;
				}
				set
				{
					CanAddTransaction = value;
				}
			}
			#endregion // IORMToolServices Implementation
		}
		/// <summary>See <see cref="ModelingDocData.CreateModelingDocStore"/>.</summary>
		protected override ModelingDocStore CreateModelingDocStore(Store store)
		{
			return new ORMModelingDocStore(ServiceProvider, store);
		}
		/// <summary>
		/// Override ModelingDocStore to provide our own UndoUnit implementation
		/// with better control over window reactivation during Undo
		/// </summary>
		protected class ORMModelingDocStore : ModelingDocStore
		{
			#region AddUndoUnit filtering
			#region Dynamic Microsoft.VisualStudio.Modeling.TransactionItem.ChangesPartition implementation
			private delegate bool TransactionItemChangesPartitionDelegate(TransactionItem @this, Partition partition);
			/// <summary>
			/// Microsoft.VisualStudio.Modeling.UndoManager has a TopmostUndableTransaction property,
			/// but not a TopmostRedoableTransaction property. Generate a dynamic method to emulate this functionality.
			/// </summary>
			private static readonly TransactionItemChangesPartitionDelegate TransactionItemChangesPartition = CreateTransactionItemChangesPartitionDelegate();
			private static TransactionItemChangesPartitionDelegate CreateTransactionItemChangesPartitionDelegate()
			{
				Type transactionItemType = typeof(TransactionItem);
				Type partitionType = typeof(Partition);
				Assembly modelingAssembly = transactionItemType.Assembly;
				string privateTypeBaseName = transactionItemType.Namespace + Type.Delimiter;
				Type modelCommandType;
				Type modelCommandListType;
				Type elementCommandType;
				PropertyInfo commandsProperty;
				MethodInfo getCommandsMethod;
				PropertyInfo partitionProperty;
				MethodInfo getPartitionMethod;
				if (null == (modelCommandType = modelingAssembly.GetType(privateTypeBaseName + "ModelCommand", false)) ||
					null == (elementCommandType = modelingAssembly.GetType(privateTypeBaseName + "ElementCommand", false)) ||
					!modelCommandType.IsAssignableFrom(elementCommandType) ||
					null == (modelCommandListType = typeof(List<>).MakeGenericType(modelCommandType)) ||
					null == (commandsProperty = transactionItemType.GetProperty("Commands", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
					commandsProperty.PropertyType != modelCommandListType ||
					null == (getCommandsMethod = commandsProperty.GetGetMethod(true)) ||
					null == (partitionProperty = elementCommandType.GetProperty("Partition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
					partitionProperty.PropertyType != partitionType ||
					null == (getPartitionMethod = partitionProperty.GetGetMethod(true)))
				{
					// The structure of the internal dll implementation has changed, il generation will fail
					return null;
				}

				// Approximate method being written (assuming TransactionItem context):
				// bool ChangesPartitionDelegate(Partition partition)
				// {
				//     List<ModelCommand> commands = Commands;
				//     commandsCount = commands.Count;
				//     for (int i = 0; i < commandsCount; ++i)
				//     {
				//         ElementCommand currentCommand = commands[i] as ElementCommand;
				//         if (currentCommand != null && currentCommand.Partition == partition)
				//         {
				//             return true;
				//         }
				//     }
				//     return false;
				// }
				DynamicMethod dynamicMethod = new DynamicMethod("TransactionItemChangesPartition", typeof(bool), new Type[] { transactionItemType, partitionType }, transactionItemType, true);
				// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 55 bytes
				// to avoid a resize to an even larger buffer.
				ILGenerator il = dynamicMethod.GetILGenerator(64);
				Label loopBodyLabel = il.DefineLabel();
				Label loopTestLabel = il.DefineLabel();
				Label notAnElementCommandLabel = il.DefineLabel();
				Label loopIncrementLabel = il.DefineLabel();
				il.DeclareLocal(typeof(int)); // commandsCount
				il.DeclareLocal(typeof(int)); // i
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Call, getCommandsMethod);
				il.Emit(OpCodes.Dup); // Save for the loop, repush each time before getting instance

				// Cache the loop count
				il.Emit(OpCodes.Call, modelCommandListType.GetProperty("Count").GetGetMethod());
				il.Emit(OpCodes.Stloc_0);

				// Initialize the loop
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Stloc_1);
				il.Emit(OpCodes.Br_S, loopTestLabel);

				// Loop contents
				il.MarkLabel(loopBodyLabel);
				il.Emit(OpCodes.Dup); // Get copy of commands
				il.Emit(OpCodes.Ldloc_1); // push i
				il.Emit(OpCodes.Call, modelCommandListType.GetProperty("Item").GetGetMethod());
				il.Emit(OpCodes.Isinst, elementCommandType);
				il.Emit(OpCodes.Dup); // For test
				il.Emit(OpCodes.Brfalse_S, notAnElementCommandLabel);
				il.Emit(OpCodes.Call, getPartitionMethod);
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Bne_Un_S, loopIncrementLabel);

				// Have a match, get out
				il.Emit(OpCodes.Pop); // Pop commands
				il.Emit(OpCodes.Ldc_I4_1);
				il.Emit(OpCodes.Ret);

				// Cast failed, pop extra item
				il.MarkLabel(notAnElementCommandLabel);
				il.Emit(OpCodes.Pop); // Pops elementCommand instance

				// Loop index increment
				il.MarkLabel(loopIncrementLabel);
				il.Emit(OpCodes.Ldloc_1);
				il.Emit(OpCodes.Ldc_I4_1);
				il.Emit(OpCodes.Add);
				il.Emit(OpCodes.Stloc_1);

				// Loop test
				il.MarkLabel(loopTestLabel);
				il.Emit(OpCodes.Ldloc_1);
				il.Emit(OpCodes.Ldloc_0);
				il.Emit(OpCodes.Blt_S, loopBodyLabel);

				// Return false
				il.Emit(OpCodes.Pop);
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Ret);
				return (TransactionItemChangesPartitionDelegate)dynamicMethod.CreateDelegate(typeof(TransactionItemChangesPartitionDelegate));
			}
			#endregion // Dynamic Microsoft.VisualStudio.Modeling.TransactionItem.ChangesPartition implementation
			private EventHandler<UndoItemEventArgs> myFilteredUndoItemAddedHandler;
			private EventHandler<UndoItemEventArgs> myFilteredUndoItemDiscardedHandler;
			/// <summary>
			/// Create a new ORMModelingDocStore.
			/// </summary>
			public ORMModelingDocStore(IServiceProvider serviceProvider, Store store)
				: base(serviceProvider, store)
			{
				FieldInfo undoItemAddedFieldInfo;
				FieldInfo undoItemDiscardedFieldInfo;
				StoreUndoManager undoManager;
				EventHandler<UndoItemEventArgs> undoItemAddedField;
				EventHandler<UndoItemEventArgs> undoItemDiscardedField;
				// Make sure everything is set up as we expected in the current assembly version
				if (ORMUndoUnit.SupportsUndoFiltering &&
					null != TransactionItemChangesPartition &&
					null != (undoManager = store.UndoManager) &&
					null != (undoItemAddedFieldInfo = typeof(StoreUndoManager).GetField("UndoItemAdded", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) &&
					null != (undoItemDiscardedFieldInfo = typeof(StoreUndoManager).GetField("UndoItemDiscarded", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) &&
					null != (undoItemAddedField = (EventHandler<UndoItemEventArgs>)undoItemAddedFieldInfo.GetValue(undoManager)) &&
					null != (undoItemDiscardedField = (EventHandler<UndoItemEventArgs>)undoItemDiscardedFieldInfo.GetValue(undoManager)))
				{
					// Grab the private base handlers by walking invocation lists
					EventHandler<UndoItemEventArgs> filteredUndoItemAddedHandler = null;
					EventHandler<UndoItemEventArgs> filteredUndoItemDiscardedHandler = null;
					Delegate[] delegates = ((MulticastDelegate)undoItemAddedField).GetInvocationList();
					int delegatesCount = delegates.Length;
					for (int i = 0; i < delegatesCount; ++i)
					{
						Delegate currentDelegate = delegates[i];
						if (currentDelegate.Target == this)
						{
							filteredUndoItemAddedHandler = (EventHandler<UndoItemEventArgs>)currentDelegate;
							break;
						}
					}
					delegates = ((MulticastDelegate)undoItemDiscardedField).GetInvocationList();
					delegatesCount = delegates.Length;
					for (int i = 0; i < delegatesCount; ++i)
					{
						Delegate currentDelegate = delegates[i];
						if (currentDelegate.Target == this)
						{
							filteredUndoItemDiscardedHandler = (EventHandler<UndoItemEventArgs>)currentDelegate;
							break;
						}
					}

					// If we go everthing we needed, then attach the handlers
					if (filteredUndoItemAddedHandler != null &&
						filteredUndoItemDiscardedHandler != null)
					{
						myFilteredUndoItemAddedHandler = filteredUndoItemAddedHandler;
						undoManager.UndoItemAdded -= filteredUndoItemAddedHandler;
						undoManager.UndoItemAdded += new EventHandler<UndoItemEventArgs>(UndoItemAddedFilter);
						myFilteredUndoItemDiscardedHandler = filteredUndoItemDiscardedHandler;
						undoManager.UndoItemDiscarded -= filteredUndoItemDiscardedHandler;
						undoManager.UndoItemDiscarded += new EventHandler<UndoItemEventArgs>(UndoItemDiscardedFilter);
					}
				}
			}
			/// <summary>
			/// Restore all event handlers in the base to the original state,
			/// then Dispose the base
			/// </summary>
			protected override void Dispose(bool disposing)
			{
				Store store = Store;
				if (store != null)
				{
					// Add back the original handlers so the base can dispose cleanly
					StoreUndoManager undoManager = store.UndoManager;
					if (null != myFilteredUndoItemAddedHandler)
					{
						undoManager.UndoItemAdded -= new EventHandler<UndoItemEventArgs>(UndoItemAddedFilter);
						undoManager.UndoItemAdded += myFilteredUndoItemAddedHandler;
					}
					if (null != myFilteredUndoItemDiscardedHandler)
					{
						undoManager.UndoItemDiscarded -= new EventHandler<UndoItemEventArgs>(UndoItemDiscardedFilter);
						undoManager.UndoItemDiscarded += myFilteredUndoItemDiscardedHandler;
					}
				}
				base.Dispose(true);
			}
			private void UndoItemAddedFilter(object sender, UndoItemEventArgs e)
			{
				TransactionItem transactionItem = e.TransactionItem;
				if (TransactionItemChangesPartition(transactionItem, transactionItem.Store.DefaultPartition))
				{
					myFilteredUndoItemAddedHandler(sender, e);
				}
			}
			private void UndoItemDiscardedFilter(object sender, UndoItemEventArgs e)
			{
				// If the item we're discarding is not the current item in the
				// undo manager, then we didn't create an undo unit for it and
				// there is nothing to do.
				ShellUndoManager shellUndoManager;
				MSOLE.IOleUndoManager oleUndoManager;
				if (null != (shellUndoManager = UndoManager) &&
					null != (oleUndoManager = shellUndoManager.VSUndoManager))
				{
					MSOLE.IEnumOleUndoUnits enumUnits;
					oleUndoManager.EnumUndoable(out enumUnits);
					enumUnits.Reset();
					MSOLE.IOleUndoUnit[] undoUnitArray = new MSOLE.IOleUndoUnit[1];
					uint unitsCount;
					enumUnits.Next(1, undoUnitArray, out unitsCount);
					ORMUndoUnit undoUnit;
					if (1 == unitsCount &&
						null != (undoUnit = undoUnitArray[0] as ORMUndoUnit) &&
						undoUnit.TransactionItem != e.TransactionItem)
					{
						return;
					}

				}
				myFilteredUndoItemDiscardedHandler(sender, e);
			}
			#endregion // AddUndoUnit filtering
			/// <summary>
			/// Create a custom undo unit
			/// </summary>
			/// <param name="undoableTransaction">New TransactionItem to attach to the undo unit</param>
			/// <returns>ORMUndoUnit</returns>
			protected override UndoUnit CreateUndoUnit(TransactionItem undoableTransaction)
			{
				return new ORMUndoUnit(ServiceProvider, Context, undoableTransaction);
			}
		}
		/// <summary>
		/// Replacement for Microsoft.VisualStudio.Modeling.Shell.UndoUnit. Changes
		/// window management and notifies the IORMToolServices implementation that
		/// new transactions are blocked.
		/// </summary>
		[CLSCompliant(false)]
		protected class ORMUndoUnit : UndoUnit, MSOLE.IOleUndoUnit
		{
			#region Member Variables
			private Context myContext;
			private bool myInUndoState;
			private IServiceProvider myServiceProvider;
			private TransactionItem myTransactionItem;
			//private ModelingWindowPane myWindow;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new ORMUndoUnit
			/// </summary>
			public ORMUndoUnit(IServiceProvider serviceProvider, Context context, TransactionItem transactionItem)
				: base(serviceProvider, context, transactionItem)
			{
				myInUndoState = true;
				myServiceProvider = serviceProvider;
				myContext = context;
				myTransactionItem = transactionItem;
				//myWindow = ActiveModelingWindow;
			}

			// Note that there are other constructors defined on Microsoft.VisualStudio.Modeling.Shell.UndoUnit,
			// but none that are actually called.
			#endregion  // Constructors
			#region Accessor Properties
			/// <summary>
			/// The TransactionItem passed to the constructor
			/// </summary>
			public TransactionItem TransactionItem
			{
				get
				{
					return myTransactionItem;
				}
			}
			#endregion // Accessor Properties
			#region Helper Properties
			// UNDONE: Consider restoring current docview/diagram/selection/toolwindow focus when
			// an undo/redo occurs.
			///// <summary>
			///// Get the active modeling window
			///// </summary>
			//protected virtual ModelingWindowPane ActiveModelingWindow
			//{
			//    [DebuggerStepThrough]
			//    get
			//    {
			//        if (myServiceProvider != null)
			//        {
			//            IMonitorSelectionService monitorService = myServiceProvider.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService;
			//            if (monitorService != null)
			//            {
			//                // Note that this was CurrentWindow in the original implementation, not CurrentDocumentView
			//                return (monitorService.CurrentDocumentView as ModelingWindowPane);
			//            }
			//        }
			//        return null;
			//    }
			//}
			#endregion // Helper Properties
			#region Dynamic Microsoft.VisualStudio.Modeling.UndoManager.TopmostRedoableTransaction implementation
			private delegate Guid GetTopmostRedoableTransactionDelegate(StoreUndoManager @this);
			/// <summary>
			/// Microsoft.VisualStudio.Modeling.UndoManager has a TopmostUndableTransaction property,
			/// but not a TopmostRedoableTransaction property. Generate a dynamic method to emulate this functionality.
			/// </summary>
			private static readonly GetTopmostRedoableTransactionDelegate GetTopmostRedoableTransaction = CreateGetTopmostRedoableTransactionDelegate();
			private static GetTopmostRedoableTransactionDelegate CreateGetTopmostRedoableTransactionDelegate()
			{
				Type undoManagerType = typeof(StoreUndoManager);
				Type listType = typeof(List<TransactionItem>);
				FieldInfo redoStackField = undoManagerType.GetField("redoStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (redoStackField == null || !listType.IsAssignableFrom(redoStackField.FieldType))
				{
					return null;
				}
				DynamicMethod dynamicMethod = new DynamicMethod("GetTopmostRedoableTransaction", typeof(Guid), new Type[] { undoManagerType }, undoManagerType, true);
				// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 36 bytes
				// to avoid a resize to an even larger buffer.
				ILGenerator il = dynamicMethod.GetILGenerator(48);
				Label emptyStackLabel = il.DefineLabel();
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, redoStackField); // Use to get the count
				il.Emit(OpCodes.Dup); // Used to call get_Item
				il.Emit(OpCodes.Call, listType.GetProperty("Count").GetGetMethod());
				il.Emit(OpCodes.Dup); // Used to call get_Item
				il.Emit(OpCodes.Brfalse_S, emptyStackLabel);
				il.Emit(OpCodes.Ldc_I4_1);
				il.Emit(OpCodes.Sub);
				il.Emit(OpCodes.Call, listType.GetProperty("Item").GetGetMethod());
				il.Emit(OpCodes.Call, typeof(TransactionItem).GetProperty("Id").GetGetMethod());
				il.Emit(OpCodes.Ret);
				il.MarkLabel(emptyStackLabel);
				il.Emit(OpCodes.Pop);
				il.Emit(OpCodes.Pop);
				il.Emit(OpCodes.Ldsfld, typeof(Guid).GetField("Empty"));
				il.Emit(OpCodes.Ret);

				return (GetTopmostRedoableTransactionDelegate)dynamicMethod.CreateDelegate(typeof(GetTopmostRedoableTransactionDelegate));
			}
			/// <summary>
			/// Returns true if the undo unit implementation allows items in the store's undo stacks that do
			/// not appear on the corresponding VS undo stacks.
			/// </summary>
			public static readonly bool SupportsUndoFiltering = GetTopmostRedoableTransaction != null;
			#endregion // Dynamic Microsoft.VisualStudio.Modeling.UndoManager.TopmostRedoableTransaction implementation
			#region IOleUndoUnit Implementation
			/// <summary>
			/// Implements IOleUndoUnit.Do
			/// </summary>
			protected new void Do(MSOLE.IOleUndoManager undoManager)
			{
				TransactionItem transactionItem = myTransactionItem;
				if (transactionItem != null)
				{
					Store store = transactionItem.Store;
					if (!store.Disposed)
					{
						IORMToolServices toolServices = store as IORMToolServices;
						if (toolServices != null)
						{
							toolServices.CanAddTransaction = false;
						}
						try
						{
							//if (myWindow != null && ActiveModelingWindow != null)
							//{
							//    // UNDONE: Note that ShowNoActivate will not bring a docview to the foreground
							//    // and Show will rip focus from the current window. Neither is optimal
							//    // for docview window activation.
							//    myWindow.Show();
							//}
							Context context = myContext;
							store.PushContext(context);
							StoreUndoManager modelingUndoManager = context.UndoManager;
							bool undoState = myInUndoState;
							Guid transactionItemId = transactionItem.Id;
							int successfulChangeCount = 0;

							// The ORMModelingDocStore class does not create undo units for
							// transactions with no changes in the default partition. This means
							// that IOleUndoManager may have fewer undo items than the store
							// has. The loops here catch the stores undo manager up to the current
							// item in the store proceeed safely.
							if (undoState)
							{
								while (modelingUndoManager.TopmostUndoableTransaction != transactionItemId)
								{
									modelingUndoManager.Undo();
									++successfulChangeCount;
								}
								modelingUndoManager.Undo(transactionItemId);
								++successfulChangeCount;
							}
							else
							{
								GetTopmostRedoableTransactionDelegate callGetTopmostRedoableTransaction = GetTopmostRedoableTransaction;
								while (callGetTopmostRedoableTransaction(modelingUndoManager) != transactionItemId)
								{
									modelingUndoManager.Redo();
									++successfulChangeCount;
								}
								modelingUndoManager.Redo(transactionItemId);
								++successfulChangeCount;
							}
							store.PopContext();
							try
							{
								if (undoManager != null)
								{
									undoManager.Add(this);
								}
								myInUndoState = !undoState;
							}
							catch (COMException)
							{
								store.PushContext(context);
								if (undoState)
								{
									while (successfulChangeCount != 0)
									{
										modelingUndoManager.Redo();
										--successfulChangeCount;
									}
								}
								else
								{
									while (successfulChangeCount != 0)
									{
										modelingUndoManager.Undo();
										--successfulChangeCount;
									}
								}
								store.PopContext();
							}
						}
						finally
						{
							if (toolServices != null)
							{
								toolServices.CanAddTransaction = true;
							}
						}
					}
				}
			}
			void MSOLE.IOleUndoUnit.Do(MSOLE.IOleUndoManager undoManager)
			{
				Do(undoManager);
			}
			/// <summary>
			/// Implements IOleUndoUnit.GetDescription
			/// </summary>
			protected new void GetDescription(out string description)
			{
				string retVal = myTransactionItem.Name;
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = " ";
				}
				description = retVal;
			}
			void MSOLE.IOleUndoUnit.GetDescription(out string description)
			{
				GetDescription(out description);
			}
			/// <summary>
			/// Implements IOleUndoUnit.GetUnitType
			/// </summary>
			protected new static void GetUnitType(out Guid unitGuid, out int unitId)
			{
				unitGuid = Guid.Empty;
				unitId = 0;
			}
			void MSOLE.IOleUndoUnit.GetUnitType(out Guid unitGuid, out int unitId)
			{
				GetUnitType(out unitGuid, out unitId);
			}
			/// <summary>
			/// Implements IOleUndoUnit.OnNextAdd
			/// </summary>
			protected new static void OnNextAdd()
			{
			}
			void MSOLE.IOleUndoUnit.OnNextAdd()
			{
				OnNextAdd();
			}
			#endregion // IOleUndoUnit Implementation
		}

		#endregion // Store services passthrough
		#region IORMToolServices Implementation
		private IORMToolTaskProvider myTaskProvider;
		private string myLastVerbalizationSnippetsOptions;
		private IDictionary<Type, IVerbalizationSets> myVerbalizationSnippets;
		private int myCustomBlockCanAddTransactionCount;
		/// <summary>
		/// Retrieve the task provider for this document. Created
		/// on demand using the CreateTaskProvider method. Implements
		/// IORMToolServices.TaskProvider.
		/// </summary>
		protected IORMToolTaskProvider TaskProvider
		{
			get
			{
				IORMToolTaskProvider provider = myTaskProvider;
				if (provider == null)
				{
					myTaskProvider = provider = CreateTaskProvider();
				}
				return provider;
			}
		}
		IORMToolTaskProvider IORMToolServices.TaskProvider
		{
			get
			{
				return TaskProvider;
			}
		}
		/// <summary>
		/// Get the color service for this document Defers to
		/// the packages color service. Implements
		/// IORMToolServices.ColorService
		/// </summary>
		protected static IORMFontAndColorService FontAndColorService
		{
			get
			{
				return ORMDesignerPackage.FontAndColorService;
			}
		}
		IORMFontAndColorService IORMToolServices.FontAndColorService
		{
			get
			{
				return FontAndColorService;
			}
		}
		IServiceProvider IORMToolServices.ServiceProvider
		{
			get
			{
				return ServiceProvider;
			}
		}
		/// <summary>
		/// Implements IORMToolServices.VerbalizationSnippetsDictionary
		/// </summary>
		protected IDictionary<Type, IVerbalizationSets> VerbalizationSnippetsDictionary
		{
			get
			{
				IDictionary<Type, IVerbalizationSets> retVal = myVerbalizationSnippets;
				string currentSnippetsOptions = myLastVerbalizationSnippetsOptions;
				string verbalizationOptions = OptionsPage.CurrentCustomVerbalizationSnippets;
				if (verbalizationOptions == null)
				{
					verbalizationOptions = "";
				}
				if (retVal == null || (currentSnippetsOptions == null || currentSnippetsOptions != verbalizationOptions))
				{
					currentSnippetsOptions = verbalizationOptions;
					// UNDONE: Directory should be configurable
					retVal = VerbalizationSnippetSetsManager.LoadSnippetsDictionary(
						Store,
						ORMDesignerPackage.VerbalizationDirectory,
						VerbalizationSnippetsIdentifier.ParseIdentifiers(verbalizationOptions));
					myVerbalizationSnippets = retVal;
					myLastVerbalizationSnippetsOptions = currentSnippetsOptions;
				}
				return retVal;
			}
		}
		IDictionary<Type, IVerbalizationSets> IORMToolServices.VerbalizationSnippetsDictionary
		{
			get
			{
				return VerbalizationSnippetsDictionary;
			}
		}
		/// <summary>
		/// Get the INotifySurveyElementChanged for the SurveyTree of this DocData
		/// may be null
		/// </summary>
		protected INotifySurveyElementChanged NotifySurveyElementChanged
		{
			get
			{
				return this.myRootBranch as INotifySurveyElementChanged;
			}
		}
		INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
		{
			get
			{
				return NotifySurveyElementChanged;
			}
		}
		/// <summary>
		/// Defer to CanAddTransaction on the document. Implements
		/// IORMToolServices.CanAddTransaction
		/// </summary>
		protected bool CanAddTransaction
		{
			get
			{
				Store store = Store;
				return !store.Disposed && !store.InUndoRedoOrRollback && myCustomBlockCanAddTransactionCount == 0;
			}
			set
			{
				int refCount = myCustomBlockCanAddTransactionCount;
				if (!value)
				{
					++refCount;
				}
				else if (refCount != 0)
				{
					--refCount;
				}
				myCustomBlockCanAddTransactionCount = refCount;
			}
		}
		bool IORMToolServices.CanAddTransaction
		{
			get
			{
				return CanAddTransaction;
			}
			set
			{
				CanAddTransaction = value;
			}
		}
		#endregion // IORMToolServices Implementation
		#region TaskProvider implementation
		/// <summary>
		/// Default implementation of a task provider
		/// </summary>
		[CLSCompliant(false)]
		protected class ORMTaskProvider : TaskProvider, IORMToolTaskProvider
		{
			#region Member Variables
			ORMDesignerDocData myDocument;
			#endregion //Member Variables
			#region Constructors
			/// <summary>
			/// Create a task provider for this document
			/// </summary>
			/// <param name="document"></param>
			public ORMTaskProvider(ORMDesignerDocData document)
				: base(document.ServiceProvider)
			{
				Debug.Assert(document.myTaskProvider == null); // Only need one
				myDocument = document;
			}
			#endregion // Constructors
			#region IORMToolTaskProvider Implementation
			/// <summary>
			/// Implements IORMToolTaskProvider.CreateTask
			/// </summary>
			/// <returns>IORMToolTaskItem</returns>
			protected IORMToolTaskItem CreateTask()
			{
				ORMTaskItem task = new ORMTaskItem(this);
				task.CanDelete = false;
				task.Checked = false;
				task.IsCheckedEditable = false;
				task.IsPriorityEditable = false;
				task.IsTextEditable = false;
				task.Line = -1;
				task.SubcategoryIndex = 0;
				// Give this a high priority so it shows up as an error.
				// Normal priority appears as a warning.
				task.Priority = TaskPriority.High;
				// Make this show in the Error list using this category
				task.Category = TaskCategory.BuildCompile;
				task.Document = myDocument.FileName;
				return task;
			}
			IORMToolTaskItem IORMToolTaskProvider.CreateTask()
			{
				return CreateTask();
			}

			/// <summary>
			/// Implements IORMToolTaskProvider.AddTask
			/// </summary>
			/// <param name="task">IORMToolTaskItem created by CreateTask</param>
			protected void AddTask(IORMToolTaskItem task)
			{
				Tasks.Add((Task)task);
			}
			void IORMToolTaskProvider.AddTask(IORMToolTaskItem task)
			{
				AddTask(task);
			}

			/// <summary>
			/// Implements IORMToolTaskProvider.RemoveTask
			/// </summary>
			/// <param name="task">IORMToolTaskItem previously added by AddTask</param>
			protected void RemoveTask(IORMToolTaskItem task)
			{
				Tasks.Remove((Task)task);
			}
			void IORMToolTaskProvider.RemoveTask(IORMToolTaskItem task)
			{
				RemoveTask(task);
			}

			/// <summary>
			/// Implements IORMToolTaskProvider.RemoveAllTasks
			/// </summary>
			protected void RemoveAllTasks()
			{
				Tasks.Clear();
			}
			void IORMToolTaskProvider.RemoveAllTasks()
			{
				RemoveAllTasks();
			}
			/// <summary>
			/// Implements IORMToolTaskProvider.NavigateTo;
			/// </summary>
			/// <param name="task"></param>
			/// <returns></returns>
			protected bool NavigateTo(IORMToolTaskItem task)
			{
				ModelElement element = null;
				IRepresentModelElements locator = task.ElementLocator;
				if (locator != null)
				{
					ModelElement[] elements = locator.GetRepresentedElements();
					if (elements != null && elements.Length != 0)
					{
						// UNDONE: Support selection of multiple elements
						element = elements[0];
					}
				}
				ModelElement startElement = element;
				IProxyDisplayProvider proxyProvider = null;
				bool useProxy = false;
				bool haveCurrentDesigner = false;
				DiagramDocView currentDocView = null;
				VSDiagramView currentDesigner = null;

				// The shape priority is:
				// 1) The first shape on a diagram which is the ActiveDiagramView for the current designer
				// 2) The first shape with an active diagram view
				// 3) The first shape.
				// We'll walk through the collection first to pick up the different elements
				const int ActiveViewOnActiveDocView = 0;
				const int ActiveView = 1;
				const int FirstShape = 2;
				const int SelectShapesSize = 3;
				ShapeElement[] selectShapes = new ShapeElement[SelectShapesSize];
				bool selectShapesInitialized = true;
				ORMDesignerDocData targetDocData = myDocument;
				// UNDONE: Move navigation code from here down into 
				// docdata and docview classes so we can use it elsewhere
				while (element != null)
				{
					ModelElement selectElement = element;
					if (useProxy)
					{
						// Second pass, we were unable to find a suitable shape for the first
						selectElement = proxyProvider.ElementDisplayedAs(element);
						if (selectElement != null && selectElement == element)
						{
							selectElement = null;
						}
					}

					// UNDONE: We should potentially be creating a shape
					// so we can jump to any error
					if (selectElement != null)
					{
						bool continueNow = false;

						// Grab the shapes in priority order
						if (!selectShapesInitialized)
						{
							selectShapes.Initialize();
							selectShapesInitialized = true;
						}
						foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(selectElement))
						{
							ShapeElement shape = pel as ShapeElement;
							if (shape != null)
							{
								// Get the current active designer
								if (!haveCurrentDesigner)
								{
									haveCurrentDesigner = true;
									IServiceProvider serviceProvider;
									IMonitorSelectionService selectionService;
									DiagramDocView currentView;
									if (null != (serviceProvider = targetDocData.ServiceProvider) &&
										null != (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))) &&
										null != (currentView = selectionService.CurrentDocumentView as DiagramDocView) &&
										currentView.DocData == targetDocData)
									{
										currentDocView = currentView;
										currentDesigner = currentDocView.CurrentDesigner;
									}
								}

								// Get the shapes in priority
								Diagram diagram = shape.Diagram;
								if (diagram != null)
								{
									selectShapesInitialized = false;
									if (selectShapes[FirstShape] == null)
									{
										selectShapes[FirstShape] = shape;
									}
									VSDiagramView diagramView = diagram.ActiveDiagramView as VSDiagramView;
									if (diagramView != null)
									{
										if (selectShapes[ActiveView] == null)
										{
											selectShapes[ActiveView] = shape;
										}
										if (diagramView == currentDesigner)
										{
											if (selectShapes[ActiveViewOnActiveDocView] == null)
											{
												selectShapes[ActiveViewOnActiveDocView] = shape;
											}
										}
									}
								}
							}
						}

						// Walk the shapes in priority order and try to select them
						if (!selectShapesInitialized)
						{
							for (int i = 0; i < SelectShapesSize; ++i)
							{
								ShapeElement shape = selectShapes[i];
								if (shape != null)
								{
									if (proxyProvider == null)
									{
										proxyProvider = shape as IProxyDisplayProvider;
									}
									if (!useProxy && element is ORMModel && element != startElement)
									{
										if (proxyProvider != null)
										{
											useProxy = true;
											element = startElement;
											continueNow = true;
											break;
										}
									}

									// Select the correct document, activate it, select the correct tab,
									// then select the shape.
									Diagram diagram = shape.Diagram;
									Debug.Assert(diagram != null); // Checked in first pass through shape elements
									VSDiagramView diagramView = diagram.ActiveDiagramView as VSDiagramView;
									MultiDiagramDocView docView;
									VSDiagramView selectOnView = null;
									if (diagramView != null)
									{
										if (currentDesigner != null && diagramView == currentDesigner)
										{
											selectOnView = diagramView;
										}
										else if (null != (docView = diagramView.DocView as MultiDiagramDocView))
										{
											selectOnView = diagramView;
											docView.Show();
										}
									}
									else if (null != (docView = currentDocView as MultiDiagramDocView))
									{
										if (docView.ActivateDiagram(diagram))
										{
											selectOnView = docView.CurrentDesigner;
										}
									}
									else
									{
										// Walk all the documents and invalidate ORM diagrams if the options have changed
										#region Walk RunningDocumentTable
										IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)targetDocData.ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
										IEnumRunningDocuments docIter;
										ErrorHandler.ThrowOnFailure(docTable.GetRunningDocumentsEnum(out docIter));
										int hrIter;
										uint[] currentDocs = new uint[1];
										uint fetched = 0;
										do
										{
											ErrorHandler.ThrowOnFailure(hrIter = docIter.Next(1, currentDocs, out fetched));
											if (hrIter == 0)
											{
												uint grfRDTFlags;
												uint dwReadLocks;
												uint dwEditLocks;
												string bstrMkDocument;
												IVsHierarchy pHier;
												uint itemId;
												IntPtr punkDocData = IntPtr.Zero;
												ErrorHandler.ThrowOnFailure(docTable.GetDocumentInfo(
													currentDocs[0],
													out grfRDTFlags,
													out dwReadLocks,
													out dwEditLocks,
													out bstrMkDocument,
													out pHier,
													out itemId,
													out punkDocData));
												try
												{
													ORMDesignerDocData docData = Marshal.GetObjectForIUnknown(punkDocData) as ORMDesignerDocData;
													if (targetDocData == docData)
													{
														IList<ModelingDocView> views = docData.DocViews;
														int viewCount = views.Count;
														for (int j = 0; j < viewCount; ++j)
														{
															docView = views[j] as MultiDiagramDocView;
															if (docView != null && docView.ActivateDiagram(diagram))
															{
																docView.Show();
																selectOnView = docView.CurrentDesigner;
																break;
															}
														}
													}
												}
												finally
												{
													if (punkDocData != IntPtr.Zero)
													{
														Marshal.Release(punkDocData);
													}
												}
											}
										} while (fetched != 0 && selectOnView == null);
										#endregion // Walk RunningDocumentTable
									}

									if (selectOnView != null)
									{
										selectOnView.Selection.Set(new DiagramItem(shape));
										selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
										ModelError error;
										IModelErrorActivation activator;
										if (null != (error = locator as ModelError) &
											null != (activator = shape as IModelErrorActivation))
										{
											activator.ActivateModelError(error);
										}
										return true;
									}
								}
							}
						}
						if (continueNow)
						{
							continue;
						}
					}
					ModelElement nextElement = element;
					element = null;

					// If we could not select the current element, then go up the aggregation chain
					foreach (DomainRoleInfo role in nextElement.GetDomainClass().AllDomainRolesPlayed)
					{
						DomainRoleInfo oppositeRole = role.OppositeDomainRole;
						if (oppositeRole.IsEmbedding)
						{
							LinkedElementCollection<ModelElement> parents = role.GetLinkedElements(nextElement);
							if (parents.Count > 0)
							{
								Debug.Assert(parents.Count == 1); // The aggregating side of a relationship should have multiplicity==1
								element = parents[0];
								break;
							}
						}
					}
				}
				return false;
			}
			bool IORMToolTaskProvider.NavigateTo(IORMToolTaskItem task)
			{
				return NavigateTo(task);
			}
			#endregion // IORMToolTaskProvider Implementation
		}
		/// <summary>
		/// Default implementation of a task item
		/// </summary>
		[CLSCompliant(false)]
		protected class ORMTaskItem : Task, IORMToolTaskItem
		{
			#region Member Variables
			IRepresentModelElements myElementLocator;
			IORMToolTaskProvider myOwner;
			#endregion //Member Variables
			#region Constructors
			private ORMTaskItem()
			{
			}
			/// <summary>
			/// Create a task item for the specified owning provider
			/// </summary>
			/// <param name="owner">IORMToolTaskProvider</param>
			public ORMTaskItem(IORMToolTaskProvider owner)
			{
				myOwner = owner;
			}
			#endregion // Constructors
			#region IORMToolTaskItem Implementation
			/// <summary>
			/// Implements IORMToolTaskItem.ElementLocator property
			/// </summary>
			protected IRepresentModelElements ElementLocator
			{
				get { return myElementLocator; }
				set { myElementLocator = value; }
			}
			IRepresentModelElements IORMToolTaskItem.ElementLocator
			{
				get { return ElementLocator; }
				set { ElementLocator = value; }
			}
			/// <summary>
			/// Implements IORMToolTaskItem.Text property
			/// </summary>
			protected new string Text
			{
				get { return base.Text; }
				set
				{
					// Don't trigger task list change unless needed
					string oldText = base.Text;
					if (oldText != value)
					{
						base.Text = value;
					}
				}
			}
			string IORMToolTaskItem.Text
			{
				get { return Text; }
				set { Text = value; }
			}
			#endregion // IORMToolTaskItem Implementation
			#region Base overrides
			/// <summary>
			/// Navigate to the item associate with this task
			/// </summary>
			/// <param name="e">EventArgs</param>
			protected override void OnNavigate(EventArgs e)
			{
				if (!myOwner.NavigateTo(this))
				{
					base.OnNavigate(e);
				}
			}
			#endregion // Base overrides
		}
		/// <summary>
		/// Create a new task provider. Called once the first time the TaskProvider
		/// property is accessed.
		/// </summary>
		protected virtual IORMToolTaskProvider CreateTaskProvider()
		{
			Debug.Assert(myTaskProvider == null);
			return new ORMTaskProvider(this);
		}
		#endregion // TaskProvider implementation
	}
}

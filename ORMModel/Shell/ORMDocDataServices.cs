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
// Uncomment the following line to make the TransactionItemChangesPartitionDelegate return an int instead of a boolean.
// This is useful for tracking which of the current transaction commands are modifying the primary partition. 
//#define DEBUG_MODIFIED_PARITION_COMMAND
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Diagrams;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using System.IO;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ORMDesignerDocData : IORMToolServices
	{
		#region SurveyTreeSetup
		private ITree myVirtualTree = null;
		private SurveyTree mySurveyTree;
		/// <summary>
		/// property to return the survey tree associated with this DocData
		/// </summary>
		public ITree SurveyTree
		{
			get
			{
				if (myVirtualTree == null)
				{
					myVirtualTree = new VirtualTree();
					ReloadSurveyTree(false);
				}
				return myVirtualTree;
			}
		}
		/// <summary>
		/// Called on a file unload to force the survey tree to recreate
		/// </summary>
		private void UnloadSurveyTree()
		{
			ITree tree = myVirtualTree;
			if (tree != null)
			{
				tree.Root = null;
				mySurveyTree = null;
			}
		}
		/// <summary>
		/// Force the survey tree to recreate its root branch if it has already been shown
		/// </summary>
		private void ReloadSurveyTree(bool isReload)
		{
			ITree tree = myVirtualTree;
			if (tree != null)
			{
				Store store = Store;
				List<ISurveyNodeProvider> nodeProviderList = new List<ISurveyNodeProvider>();
				List<ISurveyQuestionProvider> questionProviderList = new List<ISurveyQuestionProvider>();
				ICollection<DomainModel> domainModels = store.DomainModels;
				ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);
				SurveyTree rootBranch = new SurveyTree(
					Utility.EnumerateDomainModels<ISurveyNodeProvider>(domainModels),
					Utility.EnumerateDomainModels<ISurveyQuestionProvider>(domainModels));
				EventSubscriberReasons reasons = EventSubscriberReasons.SurveyQuestionEvents | EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.UserInterfaceEvents;
				if (isReload)
				{
					reasons |= EventSubscriberReasons.DocumentReloading;
				}
				foreach (IModelingEventSubscriber eventSubscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(domainModels))
				{
					eventSubscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Add);
				}
				SetFlag(PrivateFlags.AddedSurveyQuestionEvents, true);
				mySurveyTree = rootBranch;
				tree.Root = rootBranch.CreateRootBranch();
			}
		}
		#endregion //SurveyTreeSetup
		#region Store services passthrough
		/// <summary>
		/// Create a store that implements <see cref="IORMToolServices"/> by deferring
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
		protected class ORMStore : Store, IORMToolServices, IModelingEventManagerProvider, ISerializationContextHost
		{
			#region Member Variables
			private readonly IORMToolServices myServices;
			private readonly ModelingEventManager myModelingEventManager;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new store
			/// </summary>
			/// <param name="services">IORMToolServices to defer to</param>
			/// <param name="serviceProvider">Global service provider</param>
			public ORMStore(IORMToolServices services, IServiceProvider serviceProvider)
				: base(serviceProvider, null)
			{
				myServices = services;
				myModelingEventManager = new UIModelingEventManager(this, serviceProvider);
			}
			#endregion // Constructors
			#region IORMToolServices Implementation
			/// <summary>
			/// Defer to <see cref="IORMToolServices.PropertyProviderService"/> on the document.
			/// </summary>
			protected IORMPropertyProviderService PropertyProviderService
			{
				get
				{
					return myServices.PropertyProviderService;
				}
			}
			IORMPropertyProviderService IORMToolServices.PropertyProviderService
			{
				get
				{
					return PropertyProviderService;
				}
			}
			/// <summary>
			/// Defer to <see cref="IORMToolServices.ModelErrorActivationService"/> on the document.
			/// </summary>
			protected IORMModelErrorActivationService ModelErrorActivationService
			{
				get
				{
					return myServices.ModelErrorActivationService;
				}
			}
			IORMModelErrorActivationService IORMToolServices.ModelErrorActivationService
			{
				get
				{
					return ModelErrorActivationService;
				}
			}
			/// <summary>
			/// Defer to TaskProvider on the document. Implements
			/// <see cref="IORMToolServices.TaskProvider"/>
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
			/// <see cref="IORMToolServices.FontAndColorService"/>
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
			/// <see cref="IORMToolServices.ServiceProvider"/>
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
			/// Defer to GetVerbalizationSnippetsDictionary on the document. Implements
			/// <see cref="IORMToolServices.GetVerbalizationSnippetsDictionary"/>
			/// </summary>
			protected IDictionary<Type, IVerbalizationSets> GetVerbalizationSnippetsDictionary(string target)
			{
				return myServices.GetVerbalizationSnippetsDictionary(target);
			}
			IDictionary<Type, IVerbalizationSets> IORMToolServices.GetVerbalizationSnippetsDictionary(string target)
			{
				return GetVerbalizationSnippetsDictionary(target);
			}
			/// <summary>
			/// Defer to VerbalizationTargets on the document. Implements
			/// <see cref="IORMToolServices.VerbalizationTargets"/>
			/// </summary>
			protected IDictionary<string, VerbalizationTargetData> VerbalizationTargets
			{
				get
				{
					return myServices.VerbalizationTargets;
				}
			}
			IDictionary<string, VerbalizationTargetData> IORMToolServices.VerbalizationTargets
			{
				get
				{
					return VerbalizationTargets;
				}
			}
			/// <summary>
			/// Returns an instance of the specified layout engine type
			/// </summary>
			/// <param name="engineType"></param>
			/// <returns></returns>
			protected LayoutEngine GetLayoutEngine(Type engineType)
			{
				return myServices.GetLayoutEngine(engineType);
			}
			LayoutEngine IORMToolServices.GetLayoutEngine(Type engineType)
			{
				return GetLayoutEngine(engineType);
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
			/// <see cref="IORMToolServices.CanAddTransaction"/>
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
			/// <summary>
			/// Defer to ActivateShape on the document. Implements
			/// <see cref="IORMToolServices.ActivateShape"/>
			/// </summary>
			protected bool ActivateShape(ShapeElement shape)
			{
				return myServices.ActivateShape(shape);
			}
			bool IORMToolServices.ActivateShape(ShapeElement shape)
			{
				return ActivateShape(shape);
			}
			#endregion // IORMToolServices Implementation
			#region IModelingEventManagerProvider Implementation
			/// <summary>  
			/// Implements <see cref="IModelingEventManagerProvider.ModelingEventManager"/>
			/// </summary>  
			protected ModelingEventManager ModelingEventManager
			{
				get
				{
					return myModelingEventManager;
				}
			}
			ModelingEventManager IModelingEventManagerProvider.ModelingEventManager
			{
				get
				{
					return ModelingEventManager;
				}
			}
			#endregion // IModelingEventManagerProvider Implementation
			#region ISerializationContextHost Implementation
			private ISerializationContext mySerializationContext;
			/// <summary>
			/// Implements <see cref="ISerializationContextHost.SerializationContext"/>
			/// </summary>
			protected ISerializationContext SerializationContext
			{
				get
				{
					return mySerializationContext;
				}
				set
				{
					mySerializationContext = value;
				}
			}
			ISerializationContext ISerializationContextHost.SerializationContext
			{
				get
				{
					return SerializationContext;
				}
				set
				{
					SerializationContext = value;
				}
			}
			#endregion // ISerializationContextHost Implementation
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
#if	DEBUG_MODIFIED_PARITION_COMMAND
			private delegate int TransactionItemChangesPartitionDelegate(TransactionItem @this, Partition partition);
#else
			private delegate bool TransactionItemChangesPartitionDelegate(TransactionItem @this, Partition partition);
#endif
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
				DynamicMethod dynamicMethod = new DynamicMethod(
					"TransactionItemChangesPartition",
#if	DEBUG_MODIFIED_PARITION_COMMAND
					typeof(int),
#else
 typeof(bool),
#endif
 new Type[] { transactionItemType, partitionType },
					transactionItemType, true);
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
#if	DEBUG_MODIFIED_PARITION_COMMAND
				il.Emit(OpCodes.Ldloc_1); // push i
#else
				il.Emit(OpCodes.Ldc_I4_1);
#endif
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
#if	DEBUG_MODIFIED_PARITION_COMMAND
				il.Emit(OpCodes.Ldc_I4_M1);
#else
				il.Emit(OpCodes.Ldc_I4_0);
#endif
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
#if DEBUG_MODIFIED_PARITION_COMMAND
				int changedAt = TransactionItemChangesPartition(transactionItem, transactionItem.Store.DefaultPartition);
				if (changedAt != -1)
#else
				if (TransactionItemChangesPartition(transactionItem, transactionItem.Store.DefaultPartition))
#endif
				{
					myFilteredUndoItemAddedHandler(sender, e);
				}
				else
				{
					// If we didn't add, then we need to explicitly clear the redo stack. This is
					// normally a side effect of adding, but we're not adding.
					ShellUndoManager shellUndoManager;
					MSOLE.IOleUndoManager oleUndoManager;
					if (null != (shellUndoManager = UndoManager) &&
						null != (oleUndoManager = shellUndoManager.VSUndoManager))
					{
						MSOLE.IEnumOleUndoUnits enumUnits;
						oleUndoManager.EnumRedoable(out enumUnits);
						enumUnits.Reset();
						MSOLE.IOleUndoUnit[] undoUnitArray = new MSOLE.IOleUndoUnit[1];
						uint unitsCount = 0;
						MSOLE.IOleUndoUnit lastUndoUnit = null;
						for (; ; )
						{
							if (unitsCount == 1)
							{
								lastUndoUnit = undoUnitArray[0];
							}
							int hr;
							if (ErrorHandler.Failed(hr = enumUnits.Next(1, undoUnitArray, out unitsCount)))
							{
								lastUndoUnit = null;
								break;
							}
							if (hr == VSConstants.S_FALSE)
							{
								break;
							}
						}
						if (lastUndoUnit != null)
						{
							oleUndoManager.DiscardFrom(lastUndoUnit);
						}
					}
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
			/// Implements <see cref="Microsoft.VisualStudio.OLE.Interop.IOleUndoUnit.Do"/>
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
			/// Implements <see cref="Microsoft.VisualStudio.OLE.Interop.IOleUndoUnit.GetDescription"/>
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
			/// Implements <see cref="Microsoft.VisualStudio.OLE.Interop.IOleUndoUnit.GetUnitType"/>
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
			/// Implements <see cref="Microsoft.VisualStudio.OLE.Interop.IOleUndoUnit.OnNextAdd"/>
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
		#region ORMPropertyProviderService class
		private sealed class ORMPropertyProviderService : IORMPropertyProviderService, IDisposable
		{
			private readonly Store myStore;
			private readonly Dictionary<RuntimeTypeHandle, ORMPropertyProvisioning> myProvisioningDictionary;

			public ORMPropertyProviderService(Store store)
				: base()
			{
				Debug.Assert(store != null);
				this.myStore = store;
				this.myProvisioningDictionary = new Dictionary<RuntimeTypeHandle, ORMPropertyProvisioning>(RuntimeTypeHandleComparer.Instance);
			}

			public void Dispose()
			{
				this.myProvisioningDictionary.Clear();
			}

			public void AddOrRemovePropertyProvider<TExtendableElement>(ORMPropertyProvisioning propertyProvisioning, bool includeSubtypes, EventHandlerAction action)
				where TExtendableElement : ModelElement, IORMExtendableElement
			{
				if ((object)propertyProvisioning == null)
				{
					throw new ArgumentNullException("propertyProvisioning");
				}

				bool register = action == EventHandlerAction.Add;

				Type extendableElementType = typeof(TExtendableElement);

				if (register)
				{
					this.RegisterPropertyProvider(extendableElementType.TypeHandle, propertyProvisioning);
				}
				else
				{
					this.UnregisterPropertyProvider(extendableElementType.TypeHandle, propertyProvisioning);
				}
				if (includeSubtypes)
				{
					Store store = this.myStore;
					DomainClassInfo domainClassInfo = store.DomainDataDirectory.GetDomainClass(extendableElementType);
					foreach (DomainClassInfo subtypeInfo in domainClassInfo.AllDescendants)
					{
						if (register)
						{
							this.RegisterPropertyProvider(subtypeInfo.ImplementationClass.TypeHandle, propertyProvisioning);
						}
						else
						{
							this.UnregisterPropertyProvider(subtypeInfo.ImplementationClass.TypeHandle, propertyProvisioning);
						}
					}
				}
			}
			private void RegisterPropertyProvider(RuntimeTypeHandle extendableElementRuntimeTypeHandle, ORMPropertyProvisioning propertyProvisioning)
			{
				Dictionary<RuntimeTypeHandle, ORMPropertyProvisioning> provisioningDictionary = this.myProvisioningDictionary;
				ORMPropertyProvisioning existingPropertyProvider;
				provisioningDictionary.TryGetValue(extendableElementRuntimeTypeHandle, out existingPropertyProvider);
				provisioningDictionary[extendableElementRuntimeTypeHandle] = (ORMPropertyProvisioning)Delegate.Combine(existingPropertyProvider, propertyProvisioning);
			}

			private void UnregisterPropertyProvider(RuntimeTypeHandle extendableElementRuntimeTypeHandle, ORMPropertyProvisioning propertyProvisioning)
			{
				Dictionary<RuntimeTypeHandle, ORMPropertyProvisioning> provisioningDictionary = this.myProvisioningDictionary;
				ORMPropertyProvisioning existingPropertyProvisioning;
				provisioningDictionary.TryGetValue(extendableElementRuntimeTypeHandle, out existingPropertyProvisioning);
				existingPropertyProvisioning = (ORMPropertyProvisioning)Delegate.Remove(existingPropertyProvisioning, propertyProvisioning);
				if ((object)existingPropertyProvisioning == null)
				{
					provisioningDictionary.Remove(extendableElementRuntimeTypeHandle);
				}
				else
				{
					provisioningDictionary[extendableElementRuntimeTypeHandle] = existingPropertyProvisioning;
				}
			}

			public void GetProvidedProperties(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				if (extendableElement == null)
				{
					throw new ArgumentNullException("extendableElement");
				}
				if (properties == null)
				{
					throw new ArgumentNullException("properties");
				}

				ORMPropertyProvisioning propertyProvisioning;
				if (this.myProvisioningDictionary.TryGetValue(extendableElement.GetType().TypeHandle, out propertyProvisioning))
				{
					// We don't need to check propertyProvisioning for null, since UnregisterPropertyProvider would have removed it from the
					// dictionary if there were no provisionings left.
					propertyProvisioning(extendableElement, properties);
				}
			}
		}
		#endregion // ORMPropertyProvisioningService class
		#region ORMModelErrorActivationService class
		private sealed class ORMModelErrorActivationService : IORMModelErrorActivationService
		{
			#region Member variables and constructor
			private Store myStore;
			private Dictionary<Type, ORMModelErrorActivator> myActivators;
			public ORMModelErrorActivationService(Store store)
			{
				myStore = store;
				myActivators = new Dictionary<Type, ORMModelErrorActivator>();
			}
			#endregion // Member variables and constructor
			#region IORMModelErrorActivationService Implementation
			private bool ActivateError(ModelElement selectedElement, ModelError error, DomainClassInfo domainClass)
			{
				ORMModelErrorActivator activator;
				if (myActivators.TryGetValue(domainClass.ImplementationClass, out activator))
				{
					if (activator((IORMToolServices)myStore, selectedElement, error))
					{
						return true;
					}
				}
				// See if anything on a base type can handle it. This maximizes the chances of finding a handler.
				// UNDONE: Do we want both the 'registerDerivedTypes' parameter on RegisterErrorActivator and this recursion?
				domainClass = domainClass.BaseDomainClass;
				if (domainClass != null)
				{
					return ActivateError(selectedElement, error, domainClass);
				}
				return false;
			}
			bool IORMModelErrorActivationService.ActivateError(ModelElement selectedElement, ModelError error)
			{
				return ActivateError(selectedElement, error, selectedElement.GetDomainClass());
			}
			/// <summary>
			/// Recursively register the given <paramref name="activator"/> for the <paramref name="domainClass"/>
			/// </summary>
			/// <param name="domainClass">The <see cref="DomainClassInfo"/> for the type to register</param>
			/// <param name="activator">A delegate callback for when an element of this type is selected</param>
			private void RegisterErrorActivator(DomainClassInfo domainClass, ORMModelErrorActivator activator)
			{
				myActivators[domainClass.ImplementationClass] = activator;
				foreach (DomainClassInfo derivedClassInfo in domainClass.AllDescendants)
				{
					RegisterErrorActivator(derivedClassInfo, activator);
				}
			}
			void IORMModelErrorActivationService.RegisterErrorActivator(Type elementType, bool registerDerivedTypes, ORMModelErrorActivator activator)
			{
				if (registerDerivedTypes)
				{
					DomainDataDirectory dataDirectory = myStore.DomainDataDirectory;
					RegisterErrorActivator(elementType.IsSubclassOf(typeof(ElementLink)) ? dataDirectory.GetDomainRelationship(elementType) : dataDirectory.GetDomainClass(elementType), activator);
				}
				else
				{
					myActivators[elementType] = activator;
				}
			}
			#endregion // IORMModelErrorActivationService Implementation
		}
		#endregion // ORMModelErrorActivationService class
		#region IORMToolServices Implementation
		private IORMToolTaskProvider myTaskProvider;
		private string myLastVerbalizationSnippetsOptions;
		private IDictionary<string, IDictionary<Type, IVerbalizationSets>> myTargetedVerbalizationSnippets;
		private uint myInstanceVerbalizationChangeCookie;
		private static FileSystemWatcher myVerbalizationChangeWatcher;
		private static uint myVerbalizationChangeCookie;
		private IDictionary<string, VerbalizationTargetData> myVerbalizationTargets;
		private IDictionary<Type, LayoutEngineData> myLayoutEngines;
		private int myCustomBlockCanAddTransactionCount;
		private IORMPropertyProviderService myPropertyProviderService;
		private IORMModelErrorActivationService myModelErrorActivatorService;

		/// <summary>
		/// Retrieve the <see cref="IORMPropertyProviderService"/> for this document.
		/// Implements <see cref="IORMToolServices.PropertyProviderService"/>.
		/// </summary>
		protected IORMPropertyProviderService PropertyProviderService
		{
			get
			{
				return myPropertyProviderService ?? (myPropertyProviderService = new ORMPropertyProviderService(Store));
			}
		}
		IORMPropertyProviderService IORMToolServices.PropertyProviderService
		{
			get
			{
				return PropertyProviderService;
			}
		}

		/// <summary>
		/// Retrieve the <see cref="IORMModelErrorActivationService"/> for this document.
		/// Implements <see cref="IORMToolServices.ModelErrorActivationService"/>.
		/// </summary>
		protected IORMModelErrorActivationService ModelErrorActivationService
		{
			get
			{
				return myModelErrorActivatorService ?? (myModelErrorActivatorService = new ORMModelErrorActivationService(Store));
			}
		}
		IORMModelErrorActivationService IORMToolServices.ModelErrorActivationService
		{
			get
			{
				return ModelErrorActivationService;
			}
		}

		/// <summary>
		/// Retrieve the task provider for this document. Created
		/// on demand using the CreateTaskProvider method. Implements
		/// <see cref="IORMToolServices.TaskProvider"/>
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
		/// <see cref="IORMToolServices.FontAndColorService"/>
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
		/// Implements <see cref="IORMToolServices.GetVerbalizationSnippetsDictionary"/>
		/// </summary>
		protected IDictionary<Type, IVerbalizationSets> GetVerbalizationSnippetsDictionary(string target)
		{
			IDictionary<Type, IVerbalizationSets> retVal = null;
			IDictionary<string, IDictionary<Type, IVerbalizationSets>> targetedSnippets = myTargetedVerbalizationSnippets;
			string currentSnippetsOptions = myLastVerbalizationSnippetsOptions;
			string verbalizationOptions = OptionsPage.CurrentCustomVerbalizationSnippets;
			if (verbalizationOptions == null)
			{
				verbalizationOptions = "";
			}
			bool loadTarget = false;
			if (targetedSnippets == null || myInstanceVerbalizationChangeCookie != myVerbalizationChangeCookie || (currentSnippetsOptions == null || currentSnippetsOptions != verbalizationOptions))
			{
				// UNDONE: See comments in LoadSnippetsDictionary about loading
				// a dictionary with all type/target combinations then wrapping it
				// to provide a target-specific dictionary.
				currentSnippetsOptions = verbalizationOptions;
				myLastVerbalizationSnippetsOptions = currentSnippetsOptions;
				loadTarget = true;
				if (targetedSnippets != null)
				{
					targetedSnippets.Clear();
				}
				myInstanceVerbalizationChangeCookie = myVerbalizationChangeCookie;
			}
			else if (targetedSnippets != null)
			{
				loadTarget = !targetedSnippets.TryGetValue(target, out retVal);
			}
			if (loadTarget)
			{
				retVal = VerbalizationSnippetSetsManager.LoadSnippetsDictionary(
					Store,
					target,
					ORMDesignerPackage.VerbalizationDirectory,
					VerbalizationSnippetsIdentifier.ParseIdentifiers(verbalizationOptions));
				if (targetedSnippets == null)
				{
					myTargetedVerbalizationSnippets = targetedSnippets = new Dictionary<string, IDictionary<Type, IVerbalizationSets>>();
					if (myVerbalizationChangeWatcher == null)
					{
						FileSystemWatcher changeWatcher = new FileSystemWatcher(ORMDesignerPackage.VerbalizationDirectory, "*.xml");
						changeWatcher.IncludeSubdirectories = true;
						changeWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
						FileSystemEventHandler handler = new FileSystemEventHandler(VerbalizationCustomizationsChanged);
						changeWatcher.Created += handler;
						changeWatcher.Changed += handler;
						changeWatcher.Deleted += handler;
						changeWatcher.Renamed += new RenamedEventHandler(VerbalizationCustomizationsRenamed);
						changeWatcher.EnableRaisingEvents = true;
						FileSystemWatcher useWatcher = System.Threading.Interlocked.CompareExchange<FileSystemWatcher>(ref myVerbalizationChangeWatcher, changeWatcher, null);
						if (useWatcher != null)
						{
							changeWatcher.Dispose();
						}
					}
				}
				targetedSnippets[target] = retVal;
			}
			return retVal;
		}
		/// <summary>
		/// Track changes to the verbalization dictionary so that we can
		/// reload snippets files as they change.
		/// </summary>
		private static void VerbalizationCustomizationsChanged(object sender, FileSystemEventArgs e)
		{
			unchecked
			{
				++myVerbalizationChangeCookie;
			};
		}
		/// <summary>
		/// Track changes to the verbalization dictionary so that we can
		/// reload snippets files as they change.
		/// </summary>
		private static void VerbalizationCustomizationsRenamed(object sender, RenamedEventArgs e)
		{
			unchecked
			{
				++myVerbalizationChangeCookie;
			};
		}
		IDictionary<Type, IVerbalizationSets> IORMToolServices.GetVerbalizationSnippetsDictionary(string target)
		{
			return GetVerbalizationSnippetsDictionary(target);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.VerbalizationTargets"/>
		/// </summary>
		protected IDictionary<string, VerbalizationTargetData> VerbalizationTargets
		{
			get
			{
				IDictionary<string, VerbalizationTargetData> retVal = myVerbalizationTargets;
				if (null == retVal)
				{
					retVal = new Dictionary<string, VerbalizationTargetData>();
					foreach (DomainModel domainModel in Store.DomainModels)
					{
						Type domainModelType = domainModel.GetType();
						object[] providers = domainModelType.GetCustomAttributes(typeof(VerbalizationTargetProviderAttribute), false);
						if (providers.Length != 0) // Single use non-inheritable attribute, there will only be one
						{
							IVerbalizationTargetProvider provider = ((VerbalizationTargetProviderAttribute)providers[0]).CreateTargetProvider(domainModelType);
							if (provider != null)
							{
								VerbalizationTargetData[] targets = provider.ProvideVerbalizationTargets();
								if (targets != null)
								{
									for (int i = 0; i < targets.Length; ++i)
									{
										retVal[targets[i].KeyName] = targets[i];
									}
								}
							}
						}
					}
					myVerbalizationTargets = retVal;
				}
				return retVal;
			}
		}
		IDictionary<string, VerbalizationTargetData> IORMToolServices.VerbalizationTargets
		{
			get
			{
				return VerbalizationTargets;
			}
		}
		/// <summary>
		/// Get an instance of the specified <seealso cref="LayoutEngine"/> type.
		/// </summary>
		/// <param name="engineType"></param>
		/// <returns></returns>
		protected LayoutEngine GetLayoutEngine(Type engineType)
		{
			IDictionary<Type, LayoutEngineData> retVal = myLayoutEngines;
			if (retVal == null)
			{
				retVal = LayoutEngineData.CreateLayoutEngineDictionary(Store);
				myLayoutEngines = retVal;
			}
			return retVal[engineType].Instance;
		}
		LayoutEngine IORMToolServices.GetLayoutEngine(Type engineType)
		{
			return GetLayoutEngine(engineType);
		}
		/// <summary>
		/// Get the INotifySurveyElementChanged for the SurveyTree of this DocData
		/// may be null
		/// </summary>
		protected INotifySurveyElementChanged NotifySurveyElementChanged
		{
			get
			{
				return this.mySurveyTree as INotifySurveyElementChanged;
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
		/// Implements <see cref="IORMToolServices.CanAddTransaction"/>
		/// </summary>
		protected bool CanAddTransaction
		{
			get
			{
				Store store = Store;
				return !store.Disposed && !store.ShuttingDown && !store.InUndoRedoOrRollback && myCustomBlockCanAddTransactionCount == 0;
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
		/// <summary>
		/// Implements <see cref="IORMToolServices.ActivateShape"/>
		/// </summary>
		protected bool ActivateShape(ShapeElement shape)
		{
			DiagramDocView currentDocView = null;
			VSDiagramView currentDesigner = null;
			bool haveCurrentDesigner = false;

			return ActivateShapeHelper(shape, ref currentDocView, ref currentDesigner, ref haveCurrentDesigner);
		}
		private void GetCurrentDesigner(ref DiagramDocView currentDocView, ref VSDiagramView currentDesigner)
		{
			IServiceProvider serviceProvider;
			IMonitorSelectionService selectionService;
			DiagramDocView currentView;
			if (null != (serviceProvider = ServiceProvider) &&
				null != (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))) &&
				null != (currentView = selectionService.CurrentDocumentView as DiagramDocView) &&
				currentView.DocData == this)
			{
				currentDocView = currentView;
				currentDesigner = currentDocView.CurrentDesigner;
			}
		}
		private bool ActivateShapeHelper(ShapeElement shape, ref DiagramDocView currentDocView, ref VSDiagramView currentDesigner, ref bool haveCurrentDesigner)
		{
			bool retVal = false;
			if (!haveCurrentDesigner)
			{
				haveCurrentDesigner = true;
				GetCurrentDesigner(ref currentDocView, ref currentDesigner);
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
				if (docView.SelectDiagram(diagram))
				{
					selectOnView = docView.CurrentDesigner;
				}
			}
			else
			{
				// Find an appropriate view to activate
				docView = ActivateView(diagram);
				if (docView != null)
				{
					selectOnView = docView.CurrentDesigner;
				}
			}

			if (selectOnView != null)
			{
				selectOnView.Selection.Set(new DiagramItem(shape));
				selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[] { shape });
				retVal = true;
			}
			return retVal;
		}
		/// <summary>
		/// Find an appropriate view to activate for the specified
		/// <paramref name="diagram"/>
		/// </summary>
		/// <param name="diagram">The diagram to activate. Can be <see langword="null"/></param>
		/// <returns>DocView associated with the diagram, or any designer if a diagram is not specified.</returns>
		private MultiDiagramDocView ActivateView(Diagram diagram)
		{
			MultiDiagramDocView docView = null;
			#region Walk RunningDocumentTable
			IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
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
						if (this == docData)
						{
							IList<ModelingDocView> views = docData.DocViews;
							int viewCount = views.Count;
							for (int j = 0; j < viewCount; ++j)
							{
								docView = views[j] as MultiDiagramDocView;
								if (docView != null && (diagram == null || docView.SelectDiagram(diagram)))
								{
									docView.Show();
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
			} while (fetched != 0 && docView == null);
			#endregion // Walk RunningDocumentTable
			return docView;
		}
		bool IORMToolServices.ActivateShape(ShapeElement shape)
		{
			return ActivateShape(shape);
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
			/// Implements <see cref="IORMToolTaskProvider.CreateTask"/>
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
			/// Implements <see cref="IORMToolTaskProvider.AddTask"/>
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
			/// Implements <see cref="IORMToolTaskProvider.RemoveTask"/>
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
			/// Implements <see cref="IORMToolTaskProvider.RemoveAllTasks"/>
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
			/// Implements <see cref="IORMToolTaskProvider.NavigateTo"/>
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
							Array.Clear(selectShapes, 0, selectShapes.Length);
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
									targetDocData.GetCurrentDesigner(ref currentDocView, ref currentDesigner);
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

									if (targetDocData.ActivateShapeHelper(shape, ref currentDocView, ref currentDesigner, ref haveCurrentDesigner))
									{
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

				// We couldn't find this on the shapes, attempt to find the item in the model browser
				if (startElement != null)
				{
					element = startElement;
					VirtualTreeControl treeControl = null;
					while (element != null)
					{
						if (element is ISurveyNode)
						{
							// Assume if we're a SurveyNode that it is possible to select the item in the survey tree
							if (treeControl == null)
							{
								// Make sure a docview associated with the current model is
								// active. Otherwise, the model browser will not contain the
								// correct tree.
								if (!haveCurrentDesigner)
								{
									haveCurrentDesigner = true;
									targetDocData.GetCurrentDesigner(ref currentDocView, ref currentDesigner);
								}
								if (currentDocView == null || currentDocView.DocData != targetDocData)
								{
									if (null == targetDocData.ActivateView(null))
									{
										return false;
									}
								}
								// UNDONE: See if we can get the tree control without forcing the window to show
								// This is safe, but gives weird results if the initial item cannot be found.
								ORMModelBrowserToolWindow browserWindow;
								SurveyTreeContainer treeContainer;
								if (null != (browserWindow = ORMDesignerPackage.ORMModelBrowserWindow))
								{
									browserWindow.Show();
									if (null != (treeContainer = browserWindow.Window as SurveyTreeContainer))
									{
										treeControl = treeContainer.TreeControl;
									}
								}
								if (null == treeControl)
								{
									return false;
								}
							}
							if (treeControl.SelectObject(null, element, (int)ObjectStyle.TrackingObject, 0))
							{
								ModelError error;
								if (null != (error = locator as ModelError))
								{
									((IORMToolServices)myDocument).ModelErrorActivationService.ActivateError(element, error);
								}
								return true;
							}
						}
						ModelElement currentElement = element;
						element = null;
						// If we could not select the current element, then go up the aggregation chain
						foreach (DomainRoleInfo role in currentElement.GetDomainClass().AllDomainRolesPlayed)
						{
							DomainRoleInfo oppositeRole = role.OppositeDomainRole;
							if (oppositeRole.IsEmbedding)
							{
								LinkedElementCollection<ModelElement> parents = role.GetLinkedElements(currentElement);
								if (parents.Count > 0)
								{
									Debug.Assert(parents.Count == 1); // The aggregating side of a relationship should have multiplicity==1
									element = parents[0];
									break;
								}
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
			/// Implements <see cref="IORMToolTaskItem.ElementLocator"/> property
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
			/// Implements <see cref="IORMToolTaskItem.Text"/> property
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
		#region UIModelingEventManager class
		/// <summary>  
		/// A class to display an exception message without  
		/// breaking an event loop.  
		/// </summary>  
		private class UIModelingEventManager : ModelingEventManager
		{
			private IServiceProvider myServiceProvider;
			/// <summary>  
			/// Create a new UIModelingEventManager  
			/// </summary>  
			public UIModelingEventManager(Store store, IServiceProvider serviceProvider)
				: base(store)
			{
				myServiceProvider = serviceProvider;
			}
			/// <summary>  
			/// Use the standard <see cref="System.Windows.Forms.Design.IUIService"/> to display  
			/// the exception message.  
			/// </summary>  
			/// <param name="ex">The exception to display.</param>  
			protected override void DisplayException(Exception ex)
			{
				IServiceProvider provider;
				System.Windows.Forms.Design.IUIService uiService;
				if (null != (provider = myServiceProvider) &&
						null != (uiService = (System.Windows.Forms.Design.IUIService)provider.GetService(typeof(System.Windows.Forms.Design.IUIService))))
				{
					uiService.ShowDialog(new System.Windows.Forms.ThreadExceptionDialog(ex));
				}
			}
		}
		#endregion // UIModelingEventManager class
	}
}

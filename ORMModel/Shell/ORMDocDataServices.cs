#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using StoreUndoManager = Microsoft.VisualStudio.Modeling.UndoManager;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using ShellUndoManager = Microsoft.VisualStudio.Modeling.Shell.UndoManager;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	public partial class ORMDesignerDocData : IORMToolServices
	{
		#region SurveyTreeSetup
		private ITree myVirtualTree = null;
		private SurveyTree<Store> mySurveyTree;
		private int mySurveyTreeTopIndex = -1;
		private int mySurveyTreeSelectedRow = -1;
		private int mySurveyTreeWindowSessionCookie;
		/// <summary>
		/// Return the survey tree associated with this DocData
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
		/// Maintain the top displayed index for the survey tree to enable
		/// reactivation in the same location.
		/// </summary>
		public int SurveyTreeTopIndexCache
		{
			get
			{
				return mySurveyTreeTopIndex;
			}
			set
			{
				mySurveyTreeTopIndex = value;
			}
		}
		/// <summary>
		/// Maintain the currently selected roe for the survey tree
		/// to enable reactivation in the same location.
		/// </summary>
		public int SurveyTreeSelectedRowCache
		{
			get
			{
				return mySurveyTreeSelectedRow;
			}
			set
			{
				mySurveyTreeSelectedRow = value;
			}
		}
		/// <summary>
		/// A cookie set with the <see cref="SurveyTreeTopIndexCache"/> and
		/// <see cref="SurveyTreeSelectedRowCache"/> values used to determine
		/// if the cached values can be trusted when the survey display is
		/// reactivated for this document.
		/// </summary>
		public int SurveyTreeWindowSessionCookie
		{
			get
			{
				return mySurveyTreeWindowSessionCookie;
			}
			set
			{
				mySurveyTreeWindowSessionCookie = value;
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
				mySurveyTreeTopIndex = -1;
				mySurveyTreeSelectedRow = -1;
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
				ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);
				IFrameworkServices frameworkServices = (IFrameworkServices)store;
				SurveyTree<Store> rootBranch = new ORMSurveyTree(
					store,
					frameworkServices.GetTypedDomainModelProviders<ISurveyNodeProvider>(),
					frameworkServices.GetTypedDomainModelProviders<ISurveyQuestionProvider<Store>>());
				EventSubscriberReasons reasons = EventSubscriberReasons.SurveyQuestionEvents | EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.UserInterfaceEvents;
				if (isReload)
				{
					reasons |= EventSubscriberReasons.DocumentReloading;
				}
				foreach (IModelingEventSubscriber eventSubscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(store.DomainModels))
				{
					eventSubscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Add);
				}
				SetFlag(PrivateFlags.AddedSurveyQuestionEvents, true);
				mySurveyTree = rootBranch;
				tree.Root = rootBranch.CreateRootBranch();
				mySurveyTreeTopIndex = -1;
				mySurveyTreeSelectedRow = -1;
			}
		}
		private class ORMSurveyTree : SurveyTree<Store>
		{
			public ORMSurveyTree(Store surveyContext, ISurveyNodeProvider[] nodeProviders, ISurveyQuestionProvider<Store>[] questionProviders)
				: base(surveyContext, nodeProviders, questionProviders)
			{
			}
			/// <summary>
			/// Bind the delayed text editor setting to the ORM options page
			/// </summary>
			protected override bool DelayActivateTextEditors
			{
				get
				{
					return OptionsPage.CurrentDelayActivateModelBrowserLabelEdits;
				}
			}
			bool myCheckedColors;
			bool myColorsEnabled;
			/// <summary>
			/// Turn on dynamic coloring if any extensions are loaded that support it.
			/// </summary>
			protected override bool DynamicColorsEnabled
			{
				get
				{
					Store store;
					if (null == (store = Utility.ValidateStore(SurveyContext)))
					{
						return false;
					}
					if (!myCheckedColors)
					{
						myCheckedColors = true;
						myColorsEnabled = null != ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMModelBrowserDynamicColor, ShapeElement, ModelElement>>();
					}
					return myColorsEnabled;
				}
			}
			protected override Color GetDynamicColor(object element, SurveyDynamicColor colorRole)
			{
				ModelElement mel;
				Store store;
				if (null != (mel = element as ModelElement) &&
					null != (store = Utility.ValidateStore(this.SurveyContext)))
				{
					// We know the next one will succeed because it worked for DynamicColorsEnabled
					IDynamicShapeColorProvider<ORMModelBrowserDynamicColor, ShapeElement, ModelElement>[] providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMModelBrowserDynamicColor, ShapeElement, ModelElement>>();
					ORMModelBrowserDynamicColor checkColor = (colorRole == SurveyDynamicColor.ForeColor) ? ORMModelBrowserDynamicColor.Foreground : ORMModelBrowserDynamicColor.Background;
					for (int i = 0; i < providers.Length; ++i)
					{
						Color alternateColor = providers[i].GetDynamicColor(checkColor, null, mel);
						if (alternateColor != Color.Empty)
						{
							return alternateColor;
						}
					}
				}
				return Color.Empty;
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
				: base(serviceProvider, (Type[])null)
			{
				myServices = services;
				myModelingEventManager = new UIModelingEventManager(this, serviceProvider);
			}
			#endregion // Constructors
			#region IORMToolServices Implementation
			/// <summary>
			/// Defer to <see cref="IFrameworkServices.PropertyProviderService"/> on the document.
			/// </summary>
			protected IPropertyProviderService PropertyProviderService
			{
				get
				{
					return myServices.PropertyProviderService;
				}
			}
			IPropertyProviderService IFrameworkServices.PropertyProviderService
			{
				get
				{
					return PropertyProviderService;
				}
			}
			/// <summary>
			/// Defer to <see cref="IFrameworkServices.GetTypedDomainModelProviders"/> on the document.
			/// </summary>
			protected T[] GetTypedDomainModelProviders<T>() where T : class
			{
				return myServices.GetTypedDomainModelProviders<T>();
			}
			T[] IFrameworkServices.GetTypedDomainModelProviders<T>()
			{
				return GetTypedDomainModelProviders<T>();
			}
			/// <summary>
			/// Defer to <see cref="IFrameworkServices.CopyClosureManager"/> on the document.
			/// </summary>
			protected ICopyClosureManager CopyClosureManager
			{
				get
				{
					return myServices.CopyClosureManager;
				}
			}
			ICopyClosureManager IFrameworkServices.CopyClosureManager
			{
				get
				{
					return CopyClosureManager;
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
			/// Defer to ExtensionVerbalizerService on the document. Implements
			/// <see cref="IORMToolServices.ExtensionVerbalizerService"/>
			/// </summary>
			protected IExtensionVerbalizerService ExtensionVerbalizerService
			{
				get
				{
					return myServices.ExtensionVerbalizerService;
				}
			}
			IExtensionVerbalizerService IORMToolServices.ExtensionVerbalizerService
			{
				get
				{
					return ExtensionVerbalizerService;
				}
			}
			/// <summary>
			/// Implements <see cref="IORMToolServices.VerbalizationOptions"/>
			/// </summary>
			protected IDictionary<string, object> VerbalizationOptions
			{
				get
				{
					return myServices.VerbalizationOptions;
				}
			}
			IDictionary<string, object> IORMToolServices.VerbalizationOptions
			{
				get
				{
					return VerbalizationOptions;
				}
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
			INotifySurveyElementChanged IFrameworkServices.NotifySurveyElementChanged
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
			/// Defer to ProcessingVisibleTransactionItemEvents on the document. Implements
			/// <see cref="IORMToolServices.ProcessingVisibleTransactionItemEvents"/>
			/// </summary>
			protected bool ProcessingVisibleTransactionItemEvents
			{
				get
				{
					return myServices.ProcessingVisibleTransactionItemEvents;
				}
				set
				{
					myServices.ProcessingVisibleTransactionItemEvents = value;
				}
			}
			bool IORMToolServices.ProcessingVisibleTransactionItemEvents
			{
				get
				{
					return ProcessingVisibleTransactionItemEvents;
				}
				set
				{
					ProcessingVisibleTransactionItemEvents = value;
				}
			}
			/// <summary>
			/// Defer to GetAutomatedElementDirective on the document. Implements
			/// <see cref="IFrameworkServices.GetAutomatedElementDirective"/>
			/// </summary>
			protected AutomatedElementDirective GetAutomatedElementDirective(ModelElement element)
			{
				return myServices.GetAutomatedElementDirective(element);
			}
			AutomatedElementDirective IFrameworkServices.GetAutomatedElementDirective(ModelElement element)
			{
				return GetAutomatedElementDirective(element);
			}
			/// <summary>
			/// Defer to AutomatedElementFilter on the document. Implements
			/// <see cref="IFrameworkServices.AutomatedElementFilter"/>
			/// </summary>
			protected event AutomatedElementFilterCallback AutomatedElementFilter
			{
				add
				{
					myServices.AutomatedElementFilter += value;
				}
				remove
				{
					myServices.AutomatedElementFilter -= value;
				}
			}
			event AutomatedElementFilterCallback IFrameworkServices.AutomatedElementFilter
			{
				add
				{
					AutomatedElementFilter += value;
				}
				remove
				{
					AutomatedElementFilter -= value;
				}
			}
			/// <summary>
			/// Defer to ActivateShape on the document. Implements
			/// <see cref="IORMToolServices.ActivateShape"/>
			/// </summary>
			protected bool ActivateShape(ShapeElement shape, NavigateToWindow window)
			{
				return myServices.ActivateShape(shape, window);
			}
			bool IORMToolServices.ActivateShape(ShapeElement shape, NavigateToWindow window)
			{
				return ActivateShape(shape, window);
			}
			/// <summary>
			/// Defer to NavigateTo on the document. Implements
			/// <see cref="IORMToolServices.NavigateTo"/>
			/// </summary>
			protected bool NavigateTo(object target, NavigateToWindow window)
			{
				return myServices.NavigateTo(target, window);
			}
			bool IORMToolServices.NavigateTo(object target, NavigateToWindow window)
			{
				return NavigateTo(target, window);
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
			private delegate int TransactionItemChangesPartitionDelegate(TransactionItem @this, Partition partition, Predicate<EventArgs> isIgnored);
#else
			private delegate bool TransactionItemChangesPartitionDelegate(TransactionItem @this, Partition partition, Predicate<EventArgs> isIgnored);
#endif
			/// <summary>
			/// Microsoft.VisualStudio.Modeling.UndoManager has a TopmostUndoableTransaction property,
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
				Type notifyEventType = typeof(Predicate<EventArgs>);
				PropertyInfo commandsProperty;
				MethodInfo getCommandsMethod;
				PropertyInfo partitionProperty;
				MethodInfo getPartitionMethod;
				PropertyInfo eventArgsProperty;
				MethodInfo getEventArgsMethod;
				if (null == (modelCommandType = modelingAssembly.GetType(privateTypeBaseName + "ModelCommand", false)) ||
					null == (elementCommandType = modelingAssembly.GetType(privateTypeBaseName + "ElementCommand", false)) ||
					!modelCommandType.IsAssignableFrom(elementCommandType) ||
					null == (modelCommandListType = typeof(List<>).MakeGenericType(modelCommandType)) ||
					null == (commandsProperty = transactionItemType.GetProperty("Commands", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
					commandsProperty.PropertyType != modelCommandListType ||
					null == (getCommandsMethod = commandsProperty.GetGetMethod(true)) ||
					null == (partitionProperty = elementCommandType.GetProperty("Partition", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
					partitionProperty.PropertyType != partitionType ||
					null == (getPartitionMethod = partitionProperty.GetGetMethod(true)) ||
					null == (eventArgsProperty = modelCommandType.GetProperty("EventArgs", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) ||
					eventArgsProperty.PropertyType != typeof(EventArgs) ||
					null == (getEventArgsMethod = eventArgsProperty.GetGetMethod(true)))
				{
					// The structure of the internal dll implementation has changed, il generation will fail
					return null;
				}

				// Approximate method being written (assuming TransactionItem context):
				// bool ChangesPartitionDelegate(Partition partition, Predicate<EventArgs> isIgnored)
				// {
				//     List<ModelCommand> commands = Commands;
				//     commandsCount = commands.Count;
				//     for (int i = 0; i < commandsCount; ++i)
				//     {
				//         ElementCommand currentCommand = commands[i] as ElementCommand;
				//         if (currentCommand != null && currentCommand.Partition == partition && !ignored(currentCommand.EventArgs))
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
 new Type[] { transactionItemType, partitionType, notifyEventType },
					transactionItemType, true);
				// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 55 bytes
				// to avoid a resize to an even larger buffer.
				ILGenerator il = dynamicMethod.GetILGenerator(128); // Test this
				Label loopBodyLabel = il.DefineLabel();
				Label loopTestLabel = il.DefineLabel();
				Label notAnElementCommandOrForeignPartitionLabel = il.DefineLabel();
				Label loopIncrementLabel = il.DefineLabel();
				il.DeclareLocal(typeof(int)); // commandsCount
				il.DeclareLocal(typeof(int)); // i
				il.DeclareLocal(typeof(EventArgs)); // eventArgs
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
				il.Emit(OpCodes.Dup); // Keep copy of element command for getting partition
				il.Emit(OpCodes.Brfalse_S, notAnElementCommandOrForeignPartitionLabel);
				il.Emit(OpCodes.Dup); // Keep copy of element command to get event args
				il.Emit(OpCodes.Call, getPartitionMethod);
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Bne_Un_S, notAnElementCommandOrForeignPartitionLabel);
				il.Emit(OpCodes.Call, getEventArgsMethod);
				il.Emit(OpCodes.Stloc_2); // store eventArgs
				il.Emit(OpCodes.Ldarg_2); // push callback delegate
				il.Emit(OpCodes.Ldloc_2); // push eventArgs
				il.Emit(OpCodes.Callvirt, notifyEventType.GetMethod("Invoke"));
				il.Emit(OpCodes.Brtrue_S, loopIncrementLabel);

				// Have a match, get out
				il.Emit(OpCodes.Pop); // Pop commands
#if	DEBUG_MODIFIED_PARITION_COMMAND
				il.Emit(OpCodes.Ldloc_1); // push i
#else
				il.Emit(OpCodes.Ldc_I4_1);
#endif
				il.Emit(OpCodes.Ret);

				// Cast failed, pop extra item
				il.MarkLabel(notAnElementCommandOrForeignPartitionLabel);
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
			private bool myLatestUndoItemChangesDefaultPartition;
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
			/// <summary>
			/// Track whether the last added UndoItem changes
			/// the default partition. Undo items are fired
			/// before events are raised, so this property can
			/// be used to determine if a newly completing transaction
			/// modified the primary partition.
			/// </summary>
			public bool LatestUndoItemChangesDefaultPartition
			{
				get
				{
					return myLatestUndoItemChangesDefaultPartition;
				}
			}
			/// <summary>
			/// Cache property handlers to determine when a property change should be ignored.
			/// The DocStore is recreated on reload, so we do not need to worry about clearing
			/// this cache.
			/// </summary>
			private Dictionary<Guid, Predicate<ElementPropertyChangedEventArgs>> myIgnoredProperties;
			private void UndoItemAddedFilter(object sender, UndoItemEventArgs e)
			{
				TransactionItem transactionItem = e.TransactionItem;
#if DEBUG_MODIFIED_PARITION_COMMAND
				int changedAt =
#else
				if (
#endif
					TransactionItemChangesPartition(
						transactionItem,
						transactionItem.Store.DefaultPartition,
						delegate(EventArgs args)
						{
							// Determine if the current change is meaningful to the user.
							ElementPropertyChangedEventArgs propChangeArgs;
							if (null != (propChangeArgs = args as ElementPropertyChangedEventArgs))
							{
								Dictionary<Guid, Predicate<ElementPropertyChangedEventArgs>> ignoredProps = myIgnoredProperties;
								if (ignoredProps == null)
								{
									myIgnoredProperties = ignoredProps = new Dictionary<Guid, Predicate<ElementPropertyChangedEventArgs>>();
									foreach (IRegisterSignalChanges changes in ((IFrameworkServices)propChangeArgs.ModelElement.Store).GetTypedDomainModelProviders <IRegisterSignalChanges>())
									{
										foreach (KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>> changePair in changes.GetSignalPropertyChanges())
										{
											ignoredProps[changePair.Key] = changePair.Value;
										}
									}
								}
								Predicate<ElementPropertyChangedEventArgs> test;
								if (ignoredProps.TryGetValue(propChangeArgs.DomainProperty.Id, out test) &&
									(test == null || test(propChangeArgs)))
								{
									return true;
								}
							}
							return false;
						})
#if DEBUG_MODIFIED_PARITION_COMMAND
				if (changedAt != -1)
#else
				)
#endif
				{
					myLatestUndoItemChangesDefaultPartition = true;
					myFilteredUndoItemAddedHandler(sender, e);
				}
				else
				{
					myLatestUndoItemChangesDefaultPartition = false;
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
						bool inSecondaryCall = false;
						bool inPrimaryCall = false;
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
							bool firstSecondaryCall;

							// The ORMModelingDocStore class does not create undo units for
							// transactions with no changes in the default partition. This means
							// that IOleUndoManager may have fewer undo items than the store
							// has. The loops here catch the store's undo manager up to the current
							// item in the visible undo manager to proceeed in an orderly fashion.
							if (undoState)
							{
								firstSecondaryCall = toolServices != null;
								while (modelingUndoManager.TopmostUndoableTransaction != transactionItemId)
								{
									if (firstSecondaryCall)
									{
										firstSecondaryCall = false;
										toolServices.ProcessingVisibleTransactionItemEvents = false;
										inSecondaryCall = true;
									}
									modelingUndoManager.Undo();
									++successfulChangeCount;
								}
								if (inSecondaryCall)
								{
									inSecondaryCall = false;
									toolServices.ProcessingVisibleTransactionItemEvents = true;
								}
								if (toolServices != null)
								{
									toolServices.ProcessingVisibleTransactionItemEvents = true;
									inPrimaryCall = true;
								}
								modelingUndoManager.Undo(transactionItemId);
								if (inPrimaryCall)
								{
									inPrimaryCall = false;
									toolServices.ProcessingVisibleTransactionItemEvents = false;
								}
								++successfulChangeCount;
							}
							else
							{
								firstSecondaryCall = toolServices != null;
								GetTopmostRedoableTransactionDelegate callGetTopmostRedoableTransaction = GetTopmostRedoableTransaction;
								while (callGetTopmostRedoableTransaction(modelingUndoManager) != transactionItemId)
								{
									if (firstSecondaryCall)
									{
										firstSecondaryCall = false;
										toolServices.ProcessingVisibleTransactionItemEvents = false;
										inSecondaryCall = true;
									}
									modelingUndoManager.Redo();
									++successfulChangeCount;
								}
								if (inSecondaryCall)
								{
									inSecondaryCall = false;
									toolServices.ProcessingVisibleTransactionItemEvents = true;
								}
								if (toolServices != null)
								{
									toolServices.ProcessingVisibleTransactionItemEvents = true;
									inPrimaryCall = true;
								}
								modelingUndoManager.Redo(transactionItemId);
								if (inPrimaryCall)
								{
									inPrimaryCall = false;
									toolServices.ProcessingVisibleTransactionItemEvents = false;
								}
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
								if (successfulChangeCount > 1 && toolServices != null)
								{
									// The only way to get this far is to successfully complete
									// the requested transaction item, which means that the final
									// change count will always be primary.
									toolServices.ProcessingVisibleTransactionItemEvents = false;
									inSecondaryCall = true;
								}
								if (undoState)
								{
									while (successfulChangeCount != 0)
									{
										if (successfulChangeCount == 1 && toolServices != null)
										{
											if (inSecondaryCall)
											{
												inSecondaryCall = false;
												toolServices.ProcessingVisibleTransactionItemEvents = true;
											}
											toolServices.ProcessingVisibleTransactionItemEvents = true;
											inPrimaryCall = true;
										}
										modelingUndoManager.Redo();
										--successfulChangeCount;
									}
								}
								else
								{
									while (successfulChangeCount != 0)
									{
										if (successfulChangeCount == 1 && toolServices != null)
										{
											if (inSecondaryCall)
											{
												inSecondaryCall = false;
												toolServices.ProcessingVisibleTransactionItemEvents = true;
											}
											toolServices.ProcessingVisibleTransactionItemEvents = true;
											inPrimaryCall = true;
										}
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
								if (inSecondaryCall)
								{
									toolServices.ProcessingVisibleTransactionItemEvents = true;
								}
								if (inPrimaryCall)
								{
									toolServices.ProcessingVisibleTransactionItemEvents = false;
								}
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
					Delegate[] targets = activator.GetInvocationList();
					IORMToolServices services = (IORMToolServices)myStore;
					for (int i = 0; i < targets.Length; ++i)
					{
						if (((ORMModelErrorActivator)targets[i])(services, selectedElement, error))
						{
							return true;
						}
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
				AddErrorActivator(domainClass.ImplementationClass, activator);
				foreach (DomainClassInfo derivedClassInfo in domainClass.AllDescendants)
				{
					RegisterErrorActivator(derivedClassInfo, activator);
				}
			}
			private void AddErrorActivator(Type elementType, ORMModelErrorActivator activator)
			{
				Dictionary<Type, ORMModelErrorActivator> activators = myActivators;
				ORMModelErrorActivator existingActivator;
				if (activators.TryGetValue(elementType, out existingActivator))
				{
					existingActivator += activator;
					activators[elementType] = existingActivator;
				}
				else
				{
					activators[elementType] = activator;
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
					AddErrorActivator(elementType, activator);
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
#if VISUALSTUDIO_15_0
		private static FileSystemWatcher[] myVerbalizationChangeWatcher;
#else
		private static FileSystemWatcher myVerbalizationChangeWatcher;
#endif
		private static uint myVerbalizationChangeCookie;
		private IDictionary<string, VerbalizationTargetData> myVerbalizationTargets;
		private IExtensionVerbalizerService myExtensionVerbalizerService;
		private IDictionary<string, object> myDefaultVerbalizationOptions;
		private IDictionary<string, object> myVerbalizationOptions;
		private IDictionary<Type, LayoutEngineData> myLayoutEngines;
		private int myCustomBlockCanAddTransactionCount;
		private int myCustomProcessingVisibleTransactionItemEventsCount;
		private IPropertyProviderService myPropertyProviderService;
		private TypedDomainModelProviderCache myTypedDomainModelProviderCache;
		private IORMModelErrorActivationService myModelErrorActivatorService;
		private AutomatedElementFilterService myAutomatedElementFilterService;

		/// <summary>
		/// Retrieve the <see cref="IPropertyProviderService"/> for this document.
		/// Implements <see cref="IFrameworkServices.PropertyProviderService"/>.
		/// </summary>
		protected IPropertyProviderService PropertyProviderService
		{
			get
			{
				// Defensively verify store state
				PropertyProviderService providers = myPropertyProviderService as PropertyProviderService;
				Store store = Store;
				if (providers == null || providers.Store != store)
				{
					store = Utility.ValidateStore(store);
					if (store == null)
					{
						myPropertyProviderService = null;
						return null;
					}
					myPropertyProviderService = providers = new PropertyProviderService(store);
				}
				return providers;
			}
		}
		IPropertyProviderService IFrameworkServices.PropertyProviderService
		{
			get
			{
				return PropertyProviderService;
			}
		}
		/// <summary>
		/// Retrieve the domain models that implement a given interface for this model
		/// Implements <see cref="IFrameworkServices.GetTypedDomainModelProviders"/>.
		/// </summary>
		protected T[] GetTypedDomainModelProviders<T>() where T : class
		{
			// Defensively verify store state
			TypedDomainModelProviderCache cache = myTypedDomainModelProviderCache;
			Store store = Store;
			if (cache == null || cache.Store != store)
			{
				store = Utility.ValidateStore(store);
				if (store == null)
				{
					myTypedDomainModelProviderCache = null;
					return null;
				}
				myTypedDomainModelProviderCache = cache = new TypedDomainModelProviderCache(store);
			}
			return cache.GetTypedDomainModelProviders<T>();
		}
		T[] IFrameworkServices.GetTypedDomainModelProviders<T>()
		{
			return GetTypedDomainModelProviders<T>();
		}
		private CopyClosureManager myCopyClosureManager;
		/// <summary>
		/// Implements <see cref="IFrameworkServices.CopyClosureManager"/>
		/// </summary>
		protected ICopyClosureManager CopyClosureManager
		{
			get
			{
				CopyClosureManager retVal = myCopyClosureManager;
				if (retVal == null)
				{
					myCopyClosureManager = retVal = new CopyClosureManager(Store);
				}
				return retVal;
			}
		}
		ICopyClosureManager IFrameworkServices.CopyClosureManager
		{
			get
			{
				return CopyClosureManager;
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
#if VISUALSTUDIO_15_0
					ORMDesignerPackage.VerbalizationDirectories,
#else
					ORMDesignerPackage.VerbalizationDirectory,
#endif
					VerbalizationSnippetsIdentifier.ParseIdentifiers(verbalizationOptions));
				if (targetedSnippets == null)
				{
					myTargetedVerbalizationSnippets = targetedSnippets = new Dictionary<string, IDictionary<Type, IVerbalizationSets>>();
					if (myVerbalizationChangeWatcher == null
#if VISUALSTUDIO_15_0
						|| ((IList< FileSystemWatcher>)myVerbalizationChangeWatcher).IndexOf(null) != -1
#endif
					)
					{
						FileSystemEventHandler handler = new FileSystemEventHandler(VerbalizationCustomizationsChanged);
#if VISUALSTUDIO_15_0
						string[] directories = ORMDesignerPackage.VerbalizationDirectories;
						int directoryCount = directories.Length;
						FileSystemWatcher[] changeWatchers = new FileSystemWatcher[directoryCount];
						FileSystemWatcher[] useWatchers = System.Threading.Interlocked.CompareExchange<FileSystemWatcher[]>(ref myVerbalizationChangeWatcher, changeWatchers, null);
						if (useWatchers != null)
						{
							changeWatchers = useWatchers;
						}
						for (int i = 0; i < directoryCount; ++i)
						{
							if (changeWatchers[i] != null)
							{
								continue;
							}
							FileSystemWatcher changeWatcher = new FileSystemWatcher(directories[i], "*.xml");
#else
							FileSystemWatcher changeWatcher = new FileSystemWatcher(ORMDesignerPackage.VerbalizationDirectory, "*.xml");
#endif
							changeWatcher.IncludeSubdirectories = true;
							changeWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
							changeWatcher.Created += handler;
							changeWatcher.Changed += handler;
							changeWatcher.Deleted += handler;
							changeWatcher.Renamed += new RenamedEventHandler(VerbalizationCustomizationsRenamed);
							changeWatcher.EnableRaisingEvents = true;
#if VISUALSTUDIO_15_0
							FileSystemWatcher useWatcher = System.Threading.Interlocked.CompareExchange<FileSystemWatcher>(ref changeWatchers[i], changeWatcher, null);
#else
							FileSystemWatcher useWatcher = System.Threading.Interlocked.CompareExchange<FileSystemWatcher>(ref myVerbalizationChangeWatcher, changeWatcher, null);
#endif
							if (useWatcher != null)
							{
								changeWatcher.Dispose();
							}
#if VISUALSTUDIO_15_0
						}
#endif
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
		/// Implements <see cref="IORMToolServices.ExtensionVerbalizerService"/>
		/// </summary>
		protected IExtensionVerbalizerService ExtensionVerbalizerService
		{
			get
			{
				IExtensionVerbalizerService retVal = myExtensionVerbalizerService;
				return retVal ?? (myExtensionVerbalizerService = new ExtensionVerbalizerService(Store));
			}
		}
		IExtensionVerbalizerService IORMToolServices.ExtensionVerbalizerService
		{
			get
			{
				return ExtensionVerbalizerService;
			}
		}
		// UNDONE: Integrate dynamically loaded verbalization options into options page so we can
		// extend this beyond the core model.
		private bool myVerbOptionCombineMandatoryUnique = true;
		private bool myVerbOptionDefaultConstraint = true;
		private bool myVerbOptionFactTypesWithObjectType = true;
		private ObjectTypeNameVerbalizationStyle myVerbOptionObjectTypeNameDisplay = ObjectTypeNameVerbalizationStyle.AsIs;
		private string myVerbOptionRemoveObjectTypeNameCharactersOnSeparate = ".:_";
		/// <summary>
		/// Implements <see cref="IORMToolServices.VerbalizationOptions"/>
		/// </summary>
		protected IDictionary<string, object> VerbalizationOptions
		{
			get
			{
				IDictionary<string, object> defaultOptions = myDefaultVerbalizationOptions;
				if (defaultOptions == null)
				{
					myDefaultVerbalizationOptions = defaultOptions = new Dictionary<string, object>();
					foreach (DomainModel domainModel in Store.DomainModels)
					{
						Type domainModelType = domainModel.GetType();
						object[] providers = domainModelType.GetCustomAttributes(typeof(VerbalizationOptionProviderAttribute), false);
						if (providers.Length != 0) // Single use non-inheritable attribute, there will only be one
						{
							IVerbalizationOptionProvider provider = ((VerbalizationOptionProviderAttribute)providers[0]).CreateOptionProvider(domainModelType);
							if (provider != null)
							{
								VerbalizationOptionData[] data = provider.ProvideVerbalizationOptions();
								if (data != null)
								{
									for (int i = 0; i < data.Length; ++i)
									{
										VerbalizationOptionData item = data[i];
										defaultOptions[item.Name] = item.DefaultValue;
									}
								}
							}
						}
					}
				}
				bool combineMandatoryUnique = OptionsPage.CurrentCombineMandatoryAndUniqueVerbalization;
				bool defaultConstraint = OptionsPage.CurrentShowDefaultConstraintVerbalization;
				bool factTypesWithObjectType = OptionsPage.CurrentVerbalizeFactTypesWithObjectType;
				ObjectTypeNameVerbalizationStyle nameStyle = OptionsPage.CurrentVerbalizationObjectTypeNameDisplay;
				string removedCharacters = OptionsPage.CurrentVerbalizationRemoveObjectTypeNameCharactersOnSeparate;
				IDictionary<string, object> options = myVerbalizationOptions;
				if (options == null ||
					combineMandatoryUnique != myVerbOptionCombineMandatoryUnique ||
					defaultConstraint != myVerbOptionDefaultConstraint ||
					factTypesWithObjectType != myVerbOptionFactTypesWithObjectType ||
					nameStyle != myVerbOptionObjectTypeNameDisplay ||
					removedCharacters != myVerbOptionRemoveObjectTypeNameCharactersOnSeparate)
				{
					myVerbalizationOptions = options = new Dictionary<string, object>(defaultOptions);
					options[CoreVerbalizationOption.CombineSimpleMandatoryAndUniqueness] = combineMandatoryUnique;
					options[CoreVerbalizationOption.ShowDefaultConstraint] = defaultConstraint;
					options[CoreVerbalizationOption.FactTypesWithObjectType] = factTypesWithObjectType;
					options[CoreVerbalizationOption.ObjectTypeNameDisplay] = nameStyle;
					options[CoreVerbalizationOption.RemoveObjectTypeNameCharactersOnSeparate] = removedCharacters;
					myVerbOptionCombineMandatoryUnique = combineMandatoryUnique;
					myVerbOptionDefaultConstraint = defaultConstraint;
					myVerbOptionFactTypesWithObjectType = factTypesWithObjectType;
					myVerbOptionObjectTypeNameDisplay = nameStyle;
					myVerbOptionRemoveObjectTypeNameCharactersOnSeparate = removedCharacters;
				}
				return options;
			}
		}
		IDictionary<string, object> IORMToolServices.VerbalizationOptions
		{
			get
			{
				return VerbalizationOptions;
			}
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
		INotifySurveyElementChanged IFrameworkServices.NotifySurveyElementChanged
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
				Store store = Utility.ValidateStore(Store);
				return store != null && !store.InUndoRedoOrRollback && myCustomBlockCanAddTransactionCount == 0;
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
		/// Implements <see cref="IORMToolServices.ProcessingVisibleTransactionItemEvents"/>
		/// </summary>
		protected bool ProcessingVisibleTransactionItemEvents
		{
			get
			{
				int customCount = myCustomProcessingVisibleTransactionItemEventsCount;
				if (customCount == 0)
				{
					ORMModelingDocStore docStore = ModelingDocStore as ORMModelingDocStore;
					return docStore != null ? docStore.LatestUndoItemChangesDefaultPartition : true;
				}
				else
				{
					return customCount > 0;
				}
			}
			set
			{
				int refCount = myCustomProcessingVisibleTransactionItemEventsCount;
				if (value)
				{
					++refCount;
				}
				else
				{
					--refCount;
				}
				myCustomProcessingVisibleTransactionItemEventsCount = refCount;
			}
		}
		bool IORMToolServices.ProcessingVisibleTransactionItemEvents
		{
			get
			{
				return ProcessingVisibleTransactionItemEvents;
			}
			set
			{
				ProcessingVisibleTransactionItemEvents = value;
			}
		}
		/// <summary>
		/// Implements <see cref="IFrameworkServices.GetAutomatedElementDirective"/>
		/// </summary>
		protected AutomatedElementDirective GetAutomatedElementDirective(ModelElement element)
		{
			AutomatedElementFilterService impl = myAutomatedElementFilterService;
			if (null == impl)
			{
				myAutomatedElementFilterService = impl = new AutomatedElementFilterService(this);
			}
			return impl.GetAutomatedElementDirective(element);
		}
		AutomatedElementDirective IFrameworkServices.GetAutomatedElementDirective(ModelElement element)
		{
			return GetAutomatedElementDirective(element);
		}
		/// <summary>
		/// Implements <see cref="IFrameworkServices.AutomatedElementFilter"/>
		/// </summary>
		protected event AutomatedElementFilterCallback AutomatedElementFilter
		{
			add
			{
				AutomatedElementFilterService impl = myAutomatedElementFilterService;
				if (null == impl)
				{
					myAutomatedElementFilterService = impl = new AutomatedElementFilterService(this);
				}
				impl.AutomatedElementFilter += value;
			}
			remove
			{
				AutomatedElementFilterService impl = myAutomatedElementFilterService;
				if (null == impl)
				{
					myAutomatedElementFilterService = impl = new AutomatedElementFilterService(this);
				}
				impl.AutomatedElementFilter -= value;
			}
		}
		event AutomatedElementFilterCallback IFrameworkServices.AutomatedElementFilter
		{
			add
			{
				AutomatedElementFilter += value;
			}
			remove
			{
				AutomatedElementFilter -= value;
			}
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.ActivateShape"/>
		/// </summary>
		protected bool ActivateShape(ShapeElement shape, NavigateToWindow window)
		{
			if (shape != null)
			{
				switch (window)
				{
					case NavigateToWindow.Document:
						Diagram diagram = shape as Diagram;
						if (diagram != null)
						{
							return ActivateView(diagram) != null;
						}
						else
						{
							DiagramDocView currentDocView = null;
							VSDiagramView currentDesigner = null;
							bool haveCurrentDesigner = false;
							return ActivateShapeHelper(shape, null, ref currentDocView, ref currentDesigner, ref haveCurrentDesigner);
						}
					case NavigateToWindow.DiagramSpy:
						return ORMDesignerPackage.DiagramSpyWindow.ActivateShape(shape);
					case NavigateToWindow.ModelBrowser:
						return NavigateTo(shape.ModelElement, NavigateToWindow.ModelBrowser);
				}
			}
			return false;
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
		/// <summary>
		/// Currently focused ORM designer view
		/// </summary>
		private IORMDesignerView CurrentORMView
		{
			get
			{
				IServiceProvider serviceProvider;
				IMonitorSelectionService selectionService;
				if (null != (serviceProvider = ServiceProvider) &&
					null != (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))))
				{
					IORMDesignerView retVal = null;
					if (null != (retVal = selectionService.CurrentWindow as IORMDesignerView) &&
						null != retVal.CurrentDesigner &&
						retVal.DocData == this)
					{
						return retVal;
					}
					retVal = selectionService.CurrentDocumentView as IORMDesignerView;
					if (retVal != null &&
						retVal.DocData == this)
					{
						return retVal;
					}
				}
				return null;
			}
		}
		private bool ActivateShapeHelper(ShapeElement shape, DiagramItem diagramItem, ref DiagramDocView currentDocView, ref VSDiagramView currentDesigner, ref bool haveCurrentDesigner)
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
				docView = diagramView.DocView as MultiDiagramDocView;
				if (currentDesigner != null && diagramView == currentDesigner)
				{
					selectOnView = diagramView;
					if (docView != null)
					{
						// Show even if we're current or some selection changed events do not fire
						docView.Show();
					}
				}
				else if (docView != null)
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
					docView.Show();
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
				selectOnView.Selection.Set(diagramItem ?? new DiagramItem(shape));
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
		bool IORMToolServices.ActivateShape(ShapeElement shape, NavigateToWindow window)
		{
			return ActivateShape(shape, window);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.NavigateTo"/>
		/// </summary>
		protected bool NavigateTo(object target, NavigateToWindow window)
		{
			if (target == null)
			{
				return false;
			}
			ModelElement element = null;
			IRepresentModelElements elementRep = target as IRepresentModelElements;
			if (elementRep != null)
			{
				ModelElement[] elements = elementRep.GetRepresentedElements();
				if (elements != null)
				{
					int elementCount = elements.Length;
					if (elementCount != 0)
					{
						// UNDONE: Support selection of multiple elements
						if (elementCount != 1)
						{
							IORMDesignerView contextView;
							ModelElement primarySelection;
							if (null != (contextView = CurrentORMView) &&
								null != (primarySelection = EditorUtility.ResolveContextInstance(contextView.PrimarySelection, false) as ModelElement))
							{
								for (int i = 0; i < elementCount; ++i)
								{
									ModelElement testElement = elements[i];
									if (testElement == primarySelection)
									{
										element = testElement;
										break;
									}
								}
							}
							if (element == null)
							{
								element = elements[0];
							}
						}
						else
						{
							element = elements[0];
						}
					}
				}
			}
			if (element == null)
			{
				element = target as ModelElement;
				if (element == null)
				{
					// All elements on the design surface have corresponding model elements,
					// but this does not need to be true for the model browser. Attempt a model
					// browser activation directly against the supplied target.
					window = NavigateToWindow.ModelBrowser;
				}
			}
			ModelElement startElement = element;
			ModelError modelError = target as ModelError;
			bool haveCurrentDesigner = false;
			DiagramDocView currentDocView = null;
			VSDiagramView currentDesigner = null;
			if (window != NavigateToWindow.ModelBrowser)
			{
				IProxyDisplayProvider proxyProvider = null;
				bool useProxy = false;
				ORMDiagramSpyWindow diagramSpyWindow = null;
				bool activateInSpyWindow = window == NavigateToWindow.DiagramSpy;
				Diagram diagramSpyDiagram = activateInSpyWindow ? (diagramSpyWindow = ORMDesignerPackage.DiagramSpyWindow).ActiveDiagram : null;

				// The shape priority is:
				// 1) If we're selecting on the diagram spy, the first shape on the active DiagramSpy diagram
				// 2) The first shape on a diagram which is the ActiveDiagramView for the current designer
				// 3) The first shape with an active diagram view
				// 4) The first shape.
				// We'll walk through the collection first to pick up the different elements
				const int DiagramSpyView = 0;
				const int ActiveViewOnActiveDocView = 1;
				const int ActiveView = 2;
				const int FirstShape = 3;
				const int SelectShapesSize = 4;
				ShapeElement[] selectShapes = new ShapeElement[SelectShapesSize];
				bool selectShapesInitialized = false;
				bool usedProxy = false;
				while (element != null)
				{
					ModelElement selectElement = element;
					if (useProxy && !usedProxy)
					{
						// Second pass, we were unable to find a suitable shape for the first
						selectElement = proxyProvider.ElementDisplayedAs(element, modelError) as ModelElement;
						if (selectElement != null)
						{
							usedProxy = true;
							if (selectElement == element)
							{
								selectElement = null;
							}
							else
							{
								element = selectElement;
							}
						}
					}

					if (selectElement != null)
					{
						bool continueNow = false;

						// Grab the shapes in priority order
						if (selectShapesInitialized)
						{
							Array.Clear(selectShapes, 0, selectShapes.Length);
							selectShapesInitialized = false;
						}
						foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(selectElement))
						{
							ShapeElement shape = pel as ShapeElement;
							if (shape != null)
							{
								IProxyDisplayProvider localProxyProvider;
								if (null != (localProxyProvider = shape as IProxyDisplayProvider) &&
									localProxyProvider.ElementDisplayedAs(selectElement, modelError) == ProxyDisplayProviderDirective.IgnoreShape)
								{
									continue;
								}
								
								// Get the current active designer
								if (!haveCurrentDesigner)
								{
									haveCurrentDesigner = true;
									GetCurrentDesigner(ref currentDocView, ref currentDesigner);
								}

								// Get the shapes in priority
								Diagram diagram = shape.Diagram;
								if (diagram != null)
								{
									selectShapesInitialized = true;
									if (selectShapes[FirstShape] == null)
									{
										selectShapes[FirstShape] = shape;
									}
									if (diagramSpyDiagram == diagram && selectShapes[DiagramSpyView] == null)
									{
										selectShapes[DiagramSpyView] = shape;
										break; // No reason to continue, we'll end up using this shape
									}
									VSDiagramView diagramView = (currentDesigner != null && currentDesigner.Diagram == diagram) ? currentDesigner : diagram.ActiveDiagramView as VSDiagramView;
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
						if (selectShapesInitialized)
						{
							for (int i = 0; i < SelectShapesSize; ++i)
							{
								ShapeElement shape = selectShapes[i];
								if (shape != null)
								{
									IProxyDisplayProvider localProxyProvider = shape as IProxyDisplayProvider;
									if (element is ORMModel && element != startElement)
									{
										if (useProxy)
										{
											// There is no where else to go, move on to the model browser
											element = null;
											continueNow = true;
											break;
										}
										else
										{
											if (proxyProvider == null)
											{
												proxyProvider = localProxyProvider;
											}
											if (proxyProvider != null)
											{
												useProxy = true;
												element = startElement;
												continueNow = true;
												break;
											}
										}
									}
									object proxyElement;
									DiagramItem proxyDiagramItem = null;
									if (null != localProxyProvider &&
										null != (proxyElement = localProxyProvider.ElementDisplayedAs(startElement, modelError)))
									{
										ShapeElement alternateShape;
										if (null != (proxyDiagramItem = proxyElement as DiagramItem))
										{
											alternateShape = proxyDiagramItem.Shape;
										}
										else
										{
											alternateShape = proxyElement as ShapeElement;
										}
										if (alternateShape != null)
										{
											shape = alternateShape;
										}
										else if (proxyProvider == null)
										{
											proxyProvider = localProxyProvider;
										}
									}

									if (activateInSpyWindow)
									{
										if (proxyDiagramItem != null)
										{
											if (diagramSpyWindow.ActivateDiagramItem(proxyDiagramItem))
											{
												return true;
											}
										}
										else if (diagramSpyWindow.ActivateShape(shape))
										{
											return true;
										}
									}
									else if (ActivateShapeHelper(shape, proxyDiagramItem, ref currentDocView, ref currentDesigner, ref haveCurrentDesigner))
									{
										IModelErrorActivation activator;
										if (null != modelError &&
											null != (activator = shape as IModelErrorActivation))
										{
											if (!activator.ActivateModelError(modelError))
											{
												// The shape itself could not activate this error.
												// Attempt to activate using an activator that is tied to the
												// backing element for the current shape.
												ModelErrorActivationService.ActivateError(shape.ModelElement, modelError);
											}
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
					element = GetContainingElement(element);
				}
			}

			// We couldn't find this on the shapes, attempt to find the item in the model browser
			element = startElement;
			VirtualTreeControl treeControl = null;
			while (element != null || target != null)
			{
				if (treeControl == null)
				{
					// Make sure a docview associated with the current model is
					// active. Otherwise, the model browser will not contain the
					// correct tree.
					if (!haveCurrentDesigner)
					{
						haveCurrentDesigner = true;
						GetCurrentDesigner(ref currentDocView, ref currentDesigner);
					}
					if (currentDocView == null || currentDocView.DocData != this)
					{
						if (null == ActivateView(null))
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
				if (treeControl.SelectObject(null, element ?? target, (int)ObjectStyle.TrackingObject, 0))
				{
					if (modelError != null)
					{
						ModelErrorActivationService.ActivateError(element, modelError);
					}
					return true;
				}
				if (element != null)
				{
					element = GetContainingElement(element);
				}
				else
				{
					// Out of options, break the loop
					target = null;
				}
			}
			return false;
		}
		/// <summary>
		/// Get the containing embedding element
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		private static ModelElement GetContainingElement(ModelElement element)
		{
			DomainClassInfo classInfo = element.GetDomainClass();
			DomainRelationshipInfo relationshipInfo = classInfo as DomainRelationshipInfo;
			if (relationshipInfo != null)
			{
				if (!relationshipInfo.IsEmbedding)
				{
					// Jump straight to the source element
					return DomainRoleInfo.GetSourceRolePlayer((ElementLink)element);
				}
				else
				{
					// Find the role opposite a 'one' role, preferring the source role
					// if there are multiple 'one' roles.
					DomainRoleInfo singleRoleInfo = null;
					DomainRoleInfo sourceRoleInfo = null;
					foreach (DomainRoleInfo roleInfo in relationshipInfo.DomainRoles)
					{
						bool isSource = roleInfo.IsSource;
						if (isSource)
						{
							sourceRoleInfo = roleInfo;
						}
						if (roleInfo.IsOne)
						{
							if (singleRoleInfo == null || !isSource)
							{
								singleRoleInfo = roleInfo;
							}
						}
					}
					if (singleRoleInfo != null)
					{
						return singleRoleInfo.OppositeDomainRole.GetRolePlayer((ElementLink)element);
					}
					else if (sourceRoleInfo != null)
					{
						return sourceRoleInfo.GetRolePlayer((ElementLink)element);
					}
				}
			}
			else
			{
				// If we could not select the current element, then go up the aggregation chain
				foreach (DomainRoleInfo role in classInfo.AllDomainRolesPlayed)
				{
					DomainRoleInfo oppositeRole = role.OppositeDomainRole;
					if (oppositeRole.IsEmbedding)
					{
						LinkedElementCollection<ModelElement> parents = role.GetLinkedElements(element);
						if (parents.Count > 0)
						{
							Debug.Assert(parents.Count == 1); // The aggregating side of a relationship should have multiplicity==1
							return parents[0];
						}
					}
				}
			}
			return null;
		}
		bool IORMToolServices.NavigateTo(object target, NavigateToWindow window)
		{
			return NavigateTo(target, window);
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
			protected bool NavigateTo(IORMToolTaskItem task)
			{
				return myDocument.NavigateTo(task.ElementLocator, NavigateToWindow.Document);
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

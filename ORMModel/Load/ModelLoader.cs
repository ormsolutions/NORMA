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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.Load
{
	#region ModelLoader class
	/// <summary>
	/// Main class for automatically loading and saving an ORM model.
	/// </summary>
	public class ModelLoader
	{
		#region Member Variables
		private readonly ExtensionLoader myExtensionLoader;
		private readonly VerbalizationManager myVerbalizationManager;
		private readonly bool mySkipNonGenerativeExtensions;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a new model loader with support for extension models
		/// </summary>
		/// <param name="extensionLoader">The <see cref="ExtensionLoader"/> to use with this model load.</param>
		public ModelLoader(ExtensionLoader extensionLoader)
		{
			myExtensionLoader = extensionLoader;
		}
		/// <summary>
		/// Create a new model loader with support for extension models
		/// </summary>
		/// <param name="extensionLoader">The <see cref="ExtensionLoader"/> to use with this model load.</param>
		/// <param name="skipNonGenerativeExtensions">Do not load non-generative extensions.</param>
		public ModelLoader(ExtensionLoader extensionLoader, bool skipNonGenerativeExtensions)
		{
			myExtensionLoader = extensionLoader;
			mySkipNonGenerativeExtensions = skipNonGenerativeExtensions;
		}
		/// <summary>
		/// Create a new model loader with support for extension models and verbalization
		/// </summary>
		/// <param name="extensionLoader">The <see cref="ExtensionLoader"/> to use with this model load.</param>
		/// <param name="verbalizationManager">Create a loader with verbalization support.</param>
		public ModelLoader(ExtensionLoader extensionLoader, VerbalizationManager verbalizationManager)
		{
			myExtensionLoader = extensionLoader;
			myVerbalizationManager = verbalizationManager;
		}
		/// <summary>
		/// Create a new model loader with support for extension models and verbalization
		/// </summary>
		/// <param name="extensionLoader">The <see cref="ExtensionLoader"/> to use with this model load.</param>
		/// <param name="verbalizationManager">Create a loader with verbalization support.</param>
		/// <param name="skipNonGenerativeExtensions">Do not load non-generative extensions.</param>
		public ModelLoader(ExtensionLoader extensionLoader, VerbalizationManager verbalizationManager, bool skipNonGenerativeExtensions)
		{
			myExtensionLoader = extensionLoader;
			myVerbalizationManager = verbalizationManager;
			mySkipNonGenerativeExtensions = skipNonGenerativeExtensions;
		}
		#endregion // Constructor
		#region Virtual methods
		/// <summary>
		/// Create a new <see cref="Store"/> object. The returned Store should implement
		/// <see cref="IORMToolServices"/>, <see cref="IModelingEventManagerProvider"/> and
		/// <see cref="ISerializationContextHost"/>.
		/// </summary>
		/// <returns>A default <see cref="ORMStandaloneStore"/> instance.</returns>
		protected virtual Store CreateStore()
		{
			return new ORMStandaloneStore(null, myVerbalizationManager);
		}
		#endregion // Virtual methods
		#region Load methods
		/// <summary>
		/// Load a model from an input stream
		/// </summary>
		/// <param name="inputStream">The stream to input from.</param>
		/// <returns>A <see cref="Store"/> with the model loaded.</returns>
		public Store Load(Stream inputStream)
		{
			List<string> unrecognizedNamespaces = null;
			Stream namespaceStrippedStream = null;
			Store store = null;
			try
			{
				XmlReaderSettings readerSettings = new XmlReaderSettings();
				ExtensionLoader fullExtensionLoader = myExtensionLoader;
				ExtensionLoader extensionLoader = mySkipNonGenerativeExtensions ? fullExtensionLoader.NonGenerativeLoader : fullExtensionLoader;
				readerSettings.CloseInput = false;
				Dictionary<string, ExtensionModelBinding> documentExtensions = null;
				Dictionary<string, Type> skippedExtensions = null;
				using (XmlReader reader = XmlReader.Create(inputStream, readerSettings))
				{
					reader.MoveToContent();
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.MoveToFirstAttribute())
						{
							do
							{
								if (reader.Prefix == "xmlns")
								{
									string URI = reader.Value;
									bool isShapeNamespace = false;
									if (!string.Equals(URI, ORMCoreDomainModel.XmlNamespace, StringComparison.Ordinal) &&
										!(isShapeNamespace = string.Equals(URI, ORMShapeDomainModel.XmlNamespace, StringComparison.Ordinal)) &&
										!string.Equals(URI, ORMSerializationEngine.RootXmlNamespace, StringComparison.Ordinal))
									{
										ExtensionModelBinding? extensionType = extensionLoader.GetExtensionDomainModel(URI);
										if (extensionType.HasValue)
										{
											if (documentExtensions == null)
											{
												documentExtensions = new Dictionary<string, ExtensionModelBinding>();
											}
											documentExtensions[URI] = extensionType.Value;
										}
										else if (fullExtensionLoader != extensionLoader && (extensionType = fullExtensionLoader.GetExtensionDomainModel(URI)).HasValue)
										{
											if (skippedExtensions == null)
											{
												skippedExtensions = new Dictionary<string, Type>();
											}
											skippedExtensions[URI] = extensionType.Value.Type;
										}
										else
										{
											(unrecognizedNamespaces ?? (unrecognizedNamespaces = new List<string>())).Add(URI);
										}
									}
									else if (isShapeNamespace && fullExtensionLoader != extensionLoader)
									{
										if (skippedExtensions == null)
										{
											skippedExtensions = new Dictionary<string, Type>();
										}
										skippedExtensions[URI] = typeof(ORMShapeDomainModel);
									}
								}
							} while (reader.MoveToNextAttribute());
						}
					}
				}
				extensionLoader.VerifyRequiredExtensions(ref documentExtensions);

				if (unrecognizedNamespaces != null)
				{
					inputStream.Position = 0;
					namespaceStrippedStream = ExtensionLoader.CleanupStream(inputStream, extensionLoader.StandardDomainModels, documentExtensions != null ? documentExtensions.Values : null, unrecognizedNamespaces);
					if (namespaceStrippedStream != null)
					{
						inputStream = namespaceStrippedStream;
					}
					else
					{
						unrecognizedNamespaces = null;
					}
				}
				inputStream.Position = 0;
				store = CreateStore();
				store.UndoManager.UndoState = UndoState.Disabled;
				store.LoadDomainModels(extensionLoader.GetRequiredDomainModels(documentExtensions));
				if (skippedExtensions != null)
				{
					ISkipExtensions skippingStore = store as ISkipExtensions;
					if (skippingStore == null)
					{
						throw new NORMAExtensionLoadException(ResourceStrings.LoadExceptionExtensionSkippingNotSupportedByStore);
					}
					int skipCount = skippedExtensions.Count;
					Type[] skipped = new Type[skipCount];
					skippedExtensions.Values.CopyTo(skipped, 0);
					skippingStore.SkippedExtensionTypes = skipped;
				}

				try
				{
					ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);
					using (Transaction t = store.TransactionManager.BeginTransaction("File load and fixup"))
					{
						foreach (IModelingEventSubscriber subscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(store.DomainModels))
						{
							subscriber.ManageModelingEventHandlers(eventManager, EventSubscriberReasons.DocumentLoading | EventSubscriberReasons.ModelStateEvents, EventHandlerAction.Add);
						}
						if (inputStream.Length > 1)
						{
							(new ORMSerializationEngine(store)).Load(inputStream, mySkipNonGenerativeExtensions ? SerializationEngineLoadOptions.ResolveSkippedExtensions : SerializationEngineLoadOptions.None);
						}
						t.Commit();
					}
					foreach (IModelingEventSubscriber subscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(store.DomainModels))
					{
						subscriber.ManageModelingEventHandlers(eventManager, EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.ModelStateEvents, EventHandlerAction.Add);
					}
					store.UndoManager.UndoState = UndoState.Enabled;
				}
				catch (TypeInitializationException ex)
				{
					// If the type that failed to load is an extension, then remove it from
					// the list of available extensions and try again.
					if (documentExtensions != null)
					{
						string typeName = ex.TypeName;
						foreach (KeyValuePair<string, ExtensionModelBinding> pair in documentExtensions)
						{
							Type testType = pair.Value.Type;
							if (testType.FullName == typeName)
							{
								if (extensionLoader.CustomExtensionUnavailable(testType))
								{
									return Load(inputStream);
								}
								break;
							}
						}
					}
					throw;
				}
			}
			finally
			{
				if (namespaceStrippedStream != null)
				{
					namespaceStrippedStream.Dispose();
				}
			}
			return store;
		}
		/// <summary>
		/// Load directly from a file.
		/// </summary>
		/// <param name="fileName">The path of the file to open.</param>
		/// <returns>A <see cref="Store"/> containing the element.</returns>
		public Store Load(string fileName)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				return Load(fileStream);
			}
		}
		#endregion // Load methods
		#region Save methods
		/// <summary>
		/// Save an in-memory model to a <see cref="Stream"/>
		/// </summary>
		/// <param name="store">A <see cref="Store"/> returned by the <see cref="Load(String)"/> or <see cref="Load(Stream)"/> methods.</param>
		/// <param name="stream">A writable <see cref="Stream"/></param>
		public void Save(Store store, Stream stream)
		{
			(new ORMSerializationEngine(store)).Save(stream);
		}
		/// <summary>
		/// Save an in-memory model to a file.
		/// </summary>
		/// <param name="store">A <see cref="Store"/> returned by the <see cref="Load(String)"/> or <see cref="Load(Stream)"/> methods.</param>
		/// <param name="fileName">The file to save to.</param>
		public void Save(Store store, string fileName)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				Save(store, fileStream);
			}
		}
		#endregion // Save methods
	}
	#endregion // ModelLoader class
	#region ORMStandaloneStore class
	/// <summary>
	/// A <see cref="Store"/> to use for loading an ORM model without display services.
	/// </summary>
	public class ORMStandaloneStore : Store, IORMToolServices, IFrameworkServices, IORMFontAndColorService, IModelingEventManagerProvider, ISerializationContextHost, ISkipExtensions
	{
		#region Constructor
		/// <summary>
		/// Create a new <see cref="ORMStandaloneStore"/>
		/// </summary>
		/// <param name="serviceProvider">The <see cref="IServiceProvider"/>. Can be <see langword="null"/>.</param>
		/// <param name="verbalizationManager">The <see cref="VerbalizationManager"/> used to support verbalization
		/// methods on the <see cref="IORMToolServices"/> interface supported by this store.</param>
		public ORMStandaloneStore(IServiceProvider serviceProvider, VerbalizationManager verbalizationManager)
			: base(serviceProvider)
		{
			myVerbalizationManager = verbalizationManager;
		}
		#endregion // Constructor
		#region IORMToolServices Implementation
		private PropertyProviderService myPropertyProviderService;
		/// <summary>
		/// Implements <see cref="IFrameworkServices.PropertyProviderService"/>
		/// </summary>
		protected IPropertyProviderService PropertyProviderService
		{
			get
			{
				// Implemented on a per-store basis, do not defer to myServices
				PropertyProviderService providerService = myPropertyProviderService;
				if (providerService == null)
				{
					myPropertyProviderService = providerService = new PropertyProviderService(this);
				}
				return providerService;
			}
		}
		IPropertyProviderService IFrameworkServices.PropertyProviderService
		{
			get
			{
				return PropertyProviderService;
			}
		}
		private TypedDomainModelProviderCache myTypedDomainModelCache;
		/// <summary>
		/// Implements <see cref="IFrameworkServices.GetTypedDomainModelProviders(System.Boolean)"/>.
		/// </summary>
		protected T[] GetTypedDomainModelProviders<T>(bool dependencyOrder) where T : class
		{
			TypedDomainModelProviderCache cache = myTypedDomainModelCache;
			if (cache == null)
			{
				myTypedDomainModelCache = cache = new TypedDomainModelProviderCache(this);
			}
			return cache.GetTypedDomainModelProviders<T>(dependencyOrder);
		}
		T[] IFrameworkServices.GetTypedDomainModelProviders<T>()
		{
			return GetTypedDomainModelProviders<T>(false);
		}
		T[] IFrameworkServices.GetTypedDomainModelProviders<T>(bool dependencyOrder)
		{
			return GetTypedDomainModelProviders<T>(dependencyOrder);
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
					myCopyClosureManager = retVal = new CopyClosureManager(this);
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
		/// Implements <see cref="IORMToolServices.ModelErrorActivationService"/>
		/// </summary>
		protected static IORMModelErrorActivationService ModelErrorActivationService
		{
			get
			{
				return null;
			}
		}
		IORMModelErrorActivationService IORMToolServices.ModelErrorActivationService
		{
			get
			{
				return ModelErrorActivationService;
			}
		}
		private IORMExtendableElementService myExtendableElementService;
		/// <summary>
		/// Implements <see cref="IORMToolServices.ExtendableElementService"/>
		/// </summary>
		protected IORMExtendableElementService ExtendableElementService
		{
			get
			{
				return (myExtendableElementService ?? (myExtendableElementService = ExtendableElementUtility.CreateExtendableElementService(this)));
			}
		}
		IORMExtendableElementService IORMToolServices.ExtendableElementService
		{
			get
			{
				return ExtendableElementService;
			}
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.TaskProvider"/>
		/// </summary>
		protected static IORMToolTaskProvider TaskProvider
		{
			get
			{
				return null;
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
		/// Implements <see cref="IORMToolServices.CanAddTransaction"/>
		/// </summary>
		protected static bool CanAddTransaction
		{
			get
			{
				return true;
			}
			set
			{
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
		protected static bool ProcessingVisibleTransactionItemEvents
		{
			get
			{
				return true;
			}
			set
			{
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
		private Delegate myAutomedElementFilter;
		/// <summary>
		/// Implements <see cref="IFrameworkServices.GetAutomatedElementDirective"/>
		/// </summary>
		protected AutomatedElementDirective GetAutomatedElementDirective(ModelElement element)
		{
			AutomatedElementDirective retVal = AutomatedElementDirective.None;
			Delegate filterList = myAutomedElementFilter;
			if (filterList != null)
			{
				Delegate[] targets = filterList.GetInvocationList();
				for (int i = 0; i < targets.Length; ++i)
				{
					switch (((AutomatedElementFilterCallback)targets[i])(element))
					{
						case AutomatedElementDirective.NeverIgnore:
							// Strongest form, return immediately
							return AutomatedElementDirective.NeverIgnore;
						case AutomatedElementDirective.Ignore:
							retVal = AutomatedElementDirective.Ignore;
							break;
					}
				}
			}
			return retVal;
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
				myAutomedElementFilter = Delegate.Combine(myAutomedElementFilter, value);
			}
			remove
			{
				myAutomedElementFilter = Delegate.Remove(myAutomedElementFilter, value);
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
		/// Implements <see cref="IORMToolServices.FontAndColorService"/>
		/// </summary>
		protected IORMFontAndColorService FontAndColorService
		{
			get
			{
				return this;
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
		/// Implements <see cref="IORMToolServices.ServiceProvider"/>
		/// </summary>
		protected IServiceProvider ServiceProvider
		{
			get
			{
				return this;
			}
		}
		IServiceProvider IORMToolServices.ServiceProvider
		{
			get
			{
				return ServiceProvider;
			}
		}
		private VerbalizationManager myVerbalizationManager;
		private IDictionary<string, VerbalizationTargetData> myVerbalizationTargets;
		private IDictionary<string, object> myVerbalizationOptions;
		private IExtensionVerbalizerService myExtensionVerbalizerService;
		private IDictionary<string, IDictionary<Type, IVerbalizationSets>> myTargetedVerbalizationSnippets;
		/// <summary>
		/// Implements <see cref="IORMToolServices.VerbalizationTargets"/>
		/// </summary>
		protected IDictionary<string, VerbalizationTargetData> VerbalizationTargets
		{
			get
			{
				if (myVerbalizationManager == null)
				{
					return null;
				}
				IDictionary<string, VerbalizationTargetData> retVal = myVerbalizationTargets;
				if (null == retVal)
				{
					retVal = new Dictionary<string, VerbalizationTargetData>();
					foreach (DomainModel domainModel in this.DomainModels)
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
		/// Implements <see cref="IORMToolServices.GetVerbalizationSnippetsDictionary"/>
		/// </summary>
		protected IDictionary<Type, IVerbalizationSets> GetVerbalizationSnippetsDictionary(string target)
		{
			VerbalizationManager mgr = myVerbalizationManager;
			if (mgr == null)
			{
				return null;
			}
			IDictionary<Type, IVerbalizationSets> retVal = null;
			IDictionary<string, IDictionary<Type, IVerbalizationSets>> targetedSnippets = myTargetedVerbalizationSnippets;
			bool loadTarget = false;
			if (targetedSnippets == null)
			{
				loadTarget = true;
				myTargetedVerbalizationSnippets = targetedSnippets = new Dictionary<string, IDictionary<Type, IVerbalizationSets>>();
			}
			else if (targetedSnippets != null)
			{
				loadTarget = !targetedSnippets.TryGetValue(target, out retVal);
			}
			if (loadTarget)
			{
				IList<VerbalizationSnippetsIdentifier> identifiers = mgr.CustomSnippetsIdentifiers;
				targetedSnippets[target] = retVal = VerbalizationSnippetSetsManager.LoadSnippetsDictionary(
					this,
					target,
#if VISUALSTUDIO_15_0
					mgr.SnippetsDirectories,
#else
					mgr.SnippetsDirectory,
#endif
					(identifiers != null && identifiers.Count != 0) ? identifiers : null);
			}
			return retVal;
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
				if (myVerbalizationManager == null)
				{
					return null;
				}
				IExtensionVerbalizerService retVal = myExtensionVerbalizerService;
				return retVal ?? (myExtensionVerbalizerService = new ExtensionVerbalizerService(this));
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
				VerbalizationManager mgr = myVerbalizationManager;
				if (mgr == null)
				{
					return null;
				}
				IDictionary<string, object> options = myVerbalizationOptions;
				if (options == null)
				{
					myVerbalizationOptions = options = new Dictionary<string, object>();
					foreach (DomainModel domainModel in this.DomainModels)
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
										options[item.Name] = item.DefaultValue;
									}
								}
							}
						}
					}
					IDictionary<string, object> customOptions = mgr.CustomOptions;
					if (customOptions != null)
					{
						foreach (KeyValuePair<string, object> pair in customOptions)
						{
							options[pair.Key] = pair.Value;
						}
					}
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
		/// Implements <see cref="IFrameworkServices.NotifySurveyElementChanged"/>
		/// </summary>
		protected static INotifySurveyElementChanged NotifySurveyElementChanged
		{
			get
			{
				return null;
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
		/// Implements <see cref="IORMToolServices.GetLayoutEngine"/>
		/// </summary>
		protected static LayoutEngine GetLayoutEngine(Type engineType)
		{
			return null;
		}
		LayoutEngine IORMToolServices.GetLayoutEngine(Type engineType)
		{
			return GetLayoutEngine(engineType);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.ActivateShape"/>
		/// </summary>
		protected static bool ActivateShape(ShapeElement shape, NavigateToWindow window)
		{
			return false;
		}
		bool IORMToolServices.ActivateShape(ShapeElement shape, NavigateToWindow window)
		{
			return ActivateShape(shape, window);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.NavigateTo(object,NavigateToWindow)"/>
		/// </summary>
		protected static bool NavigateTo(object element, NavigateToWindow window)
		{
			return false;
		}
		bool IORMToolServices.NavigateTo(object element, NavigateToWindow window)
		{
			return NavigateTo(element, window);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.NavigateTo(object,NavigateToWindow,NavigateToOptions)"/>
		/// </summary>
		protected static bool NavigateTo(object element, NavigateToWindow window, NavigateToOptions options)
		{
			return false;
		}
		bool IORMToolServices.NavigateTo(object element, NavigateToWindow window, NavigateToOptions options)
		{
			return NavigateTo(element, window, options);
		}
		#endregion // IORMToolServices Implementation
		#region IModelingEventManagerProvider Implementation
		private ModelingEventManager myModelingEventManager;
		/// <summary>
		/// Implements <see cref="IModelingEventManagerProvider.ModelingEventManager"/>
		/// </summary>
		protected ModelingEventManager ModelingEventManager
		{
			get
			{
				ModelingEventManager retVal = myModelingEventManager;
				if (retVal == null)
				{
					myModelingEventManager = retVal = new ModelingEventManagerImpl(this);
				}
				return retVal;
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
		#region ModelingEventManagerImpl class
		/// <summary>  
		/// Display event exceptions by rethrowing
		/// </summary>
		private sealed class ModelingEventManagerImpl : ModelingEventManager
		{
			/// <summary>  
			/// Create a new <see cref="ModelingEventManager"/>  
			/// </summary>  
			public ModelingEventManagerImpl(Store store)
				: base(store)
			{
			}
			/// <summary>
			/// Record exception
			/// </summary>  
			/// <param name="ex">The exception to display.</param>  
			protected override void DisplayException(Exception ex)
			{
				throw ex;
			}
		}
		#endregion // ModelingEventManagerImpl class
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
		#region IORMFontAndColorService Implementation
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetBackColor"/>
		/// </summary>
		protected static Color GetBackColor(ORMDesignerColor colorIndex)
		{
			return Color.White;
		}
		Color IORMFontAndColorService.GetBackColor(ORMDesignerColor colorIndex)
		{
			return GetBackColor(colorIndex);
		}
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetFont"/>
		/// </summary>
		protected Font GetFont(ORMDesignerColorCategory fontCategory)
		{
			VerbalizationManager mgr;
			if (fontCategory == ORMDesignerColorCategory.Verbalizer &&
				null != (mgr = myVerbalizationManager))
			{
				return new Font(mgr.FontFamilyName, mgr.FontSize / 72, FontStyle.Regular);
			}
			return new Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
		}
		Font IORMFontAndColorService.GetFont(ORMDesignerColorCategory fontCategory)
		{
			return GetFont(fontCategory);
		}
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetFontStyle"/>
		/// </summary>
		protected FontStyle GetFontStyle(ORMDesignerColor colorIndex)
		{
			VerbalizationManager mgr;
			int index = (int)colorIndex;
			if (index >= (int)ORMDesignerColor.FirstVerbalizerColor &&
				index <= (int)ORMDesignerColor.LastVerbalizerColor &&
				null != (mgr = myVerbalizationManager))
			{
				return mgr.GetIsBold(colorIndex) ? FontStyle.Bold : FontStyle.Regular;
			}
			return FontStyle.Bold;
		}
		FontStyle IORMFontAndColorService.GetFontStyle(ORMDesignerColor colorIndex)
		{
			return GetFontStyle(colorIndex);
		}
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetForeColor"/>
		/// </summary>
		protected Color GetForeColor(ORMDesignerColor colorIndex)
		{
			VerbalizationManager mgr;
			int index = (int)colorIndex;
			if (index >= (int)ORMDesignerColor.FirstVerbalizerColor &&
				index <= (int)ORMDesignerColor.LastVerbalizerColor &&
				null != (mgr = myVerbalizationManager))
			{
				return mgr.GetColor(colorIndex);
			}
			return Color.Black;
		}
		Color IORMFontAndColorService.GetForeColor(ORMDesignerColor colorIndex)
		{
			return GetForeColor(colorIndex);
		}
		#endregion // IORMFontAndColorService Implementation
		#region ISkipExtensions implementation
		IList<Type> ISkipExtensions.SkippedExtensionTypes
		{
			get
			{
				return SkippedExtensionTypes;
			}
			set
			{
				SkippedExtensionTypes = value;
			}
		}
		private IList<Type> mySkippedExtensionTypes;
		/// <summary>
		/// Implements <see cref="ISkipExtensions.SkippedExtensionTypes"/>
		/// </summary>
		protected IList<Type> SkippedExtensionTypes
		{
			get
			{
				return mySkippedExtensionTypes;
			}
			set
			{
				mySkippedExtensionTypes = value;
			}
		}
		#endregion // ISkipExtensions implementation
	}
	#endregion // ORMStandaloneStore class
}

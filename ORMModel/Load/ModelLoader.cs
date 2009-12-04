#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
		private ExtensionLoader myExtensionLoader;
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
			return new ORMStandaloneStore(null);
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
				ExtensionLoader extensionLoader = myExtensionLoader	;
				readerSettings.CloseInput = false;
				Dictionary<string, ExtensionModelBinding> documentExtensions = null;
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
									if (!string.Equals(URI, ORMCoreDomainModel.XmlNamespace, StringComparison.Ordinal) &&
										!string.Equals(URI, ORMShapeDomainModel.XmlNamespace, StringComparison.Ordinal) &&
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
										else
										{
											(unrecognizedNamespaces ?? (unrecognizedNamespaces = new List<string>())).Add(URI);
										}
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
					namespaceStrippedStream = ExtensionLoader.CleanupStream(inputStream, extensionLoader.StandardDomainModels, documentExtensions.Values, unrecognizedNamespaces);
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
							(new ORMSerializationEngine(store)).Load(inputStream);
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
					// If the type that failed to load is an extensions, then remove it from
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
	public class ORMStandaloneStore : Store, IORMToolServices, IFrameworkServices, IORMFontAndColorService, IModelingEventManagerProvider, ISerializationContextHost
	{
		#region Constructor
		/// <summary>
		/// Create a new <see cref="ORMStandaloneStore"/>
		/// </summary>
		/// <param name="serviceProvider">The <see cref="IServiceProvider"/>. Can be <see langword="null"/>.</param>
		public ORMStandaloneStore(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
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
		/// Implements <see cref="IFrameworkServices.GetTypedDomainModelProviders"/>.
		/// </summary>
		protected T[] GetTypedDomainModelProviders<T>() where T : class
		{
			TypedDomainModelProviderCache cache = myTypedDomainModelCache;
			if (cache == null)
			{
				myTypedDomainModelCache = cache = new TypedDomainModelProviderCache(this);
			}
			return cache.GetTypedDomainModelProviders<T>();
		}
		T[] IFrameworkServices.GetTypedDomainModelProviders<T>()
		{
			return GetTypedDomainModelProviders<T>();
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
		/// Implements <see cref="IORMToolServices.GetAutomatedElementDirective"/>
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
		AutomatedElementDirective IORMToolServices.GetAutomatedElementDirective(ModelElement element)
		{
			return GetAutomatedElementDirective(element);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.AutomatedElementFilter"/>
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
		event AutomatedElementFilterCallback IORMToolServices.AutomatedElementFilter
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
		/// <summary>
		/// Implements <see cref="IORMToolServices.VerbalizationTargets"/>
		/// </summary>
		protected static IDictionary<string, VerbalizationTargetData> VerbalizationTargets
		{
			get
			{
				return null;
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
		protected static IDictionary<Type, IVerbalizationSets> GetVerbalizationSnippetsDictionary(string target)
		{
			return null;
		}
		IDictionary<Type, IVerbalizationSets> IORMToolServices.GetVerbalizationSnippetsDictionary(string target)
		{
			return GetVerbalizationSnippetsDictionary(target);
		}
		/// <summary>
		/// Implements <see cref="IORMToolServices.ExtensionVerbalizerService"/>
		/// </summary>
		protected static IExtensionVerbalizerService ExtensionVerbalizerService
		{
			get
			{
				return null;
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
		/// Implements <see cref="IORMToolServices.NavigateTo"/>
		/// </summary>
		protected static bool NavigateTo(object element, NavigateToWindow window)
		{
			return false;
		}
		bool IORMToolServices.NavigateTo(object element, NavigateToWindow window)
		{
			return NavigateTo(element, window);
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
				throw new InvalidOperationException("", ex);
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
			return Color.Black;
		}
		Color IORMFontAndColorService.GetBackColor(ORMDesignerColor colorIndex)
		{
			return GetBackColor(colorIndex);
		}
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetFont"/>
		/// </summary>
		protected static Font GetFont(ORMDesignerColorCategory fontCategory)
		{
			return new Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
		}
		Font IORMFontAndColorService.GetFont(ORMDesignerColorCategory fontCategory)
		{
			return GetFont(fontCategory);
		}
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetFontStyle"/>
		/// </summary>
		protected static FontStyle GetFontStyle(ORMDesignerColor colorIndex)
		{
			return FontStyle.Bold;
		}
		FontStyle IORMFontAndColorService.GetFontStyle(ORMDesignerColor colorIndex)
		{
			return GetFontStyle(colorIndex);
		}
		/// <summary>
		/// Implements <see cref="IORMFontAndColorService.GetForeColor"/>
		/// </summary>
		protected static Color GetForeColor(ORMDesignerColor colorIndex)
		{
			return Color.White;
		}
		Color IORMFontAndColorService.GetForeColor(ORMDesignerColor colorIndex)
		{
			return GetForeColor(colorIndex);
		}
		#endregion // IORMFontAndColorService Implementation
	}
	#endregion // ORMStandaloneStore class
}

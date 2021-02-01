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

// Turning this on will result in the ability to debug the extension stripper
// transform and related callback code. A message showing with debugging when
// the transform is first loading, giving you the opportunity to format the
// transform, insert breakpoints, etc. Obviously, this is for debug purposes
// only and should never be turned on.
//#define DEBUG_EXTENSIONSTRIPPER_TRANSFORM
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Win32;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.Load
{
	#region ExtensionModelOptions enum
	/// <summary>
	/// Available options for an extension model
	/// </summary>
	[Flags]
	public enum ExtensionModelOptions
	{
		/// <summary>
		/// No special options are applied
		/// </summary>
		None = 0,
		/// <summary>
		/// The extension model loads automatically as part
		/// of another model, should not be displayed to the user.
		/// </summary>
		Secondary = 1,
		/// <summary>
		/// The extension model should always be loaded.
		/// </summary>
		AutoLoad = 2,
		/// <summary>
		/// The extension model should not be loaded if the document is loaded
		/// solely for code generation. This overrides the <see cref="ExtensionModelOptions.AutoLoad"/>
		/// flag and is generally used for models associated with displaying diagrams or
		/// other displayed elements. If a model references a non-generative model then it should
		/// also be set accordingly.
		/// </summary>
		NonGenerative = 4,
	}
	#endregion // ExtensionModelOptions enum
	#region ExtensionModelData struct
	/// <summary>
	/// Information for loading an extension model. This provides the raw
	/// data, which is converted to type information for use with the
	/// <see cref="ExtensionModelBinding"/> class.
	/// </summary>
	public struct ExtensionModelData
	{
		/// <summary>
		/// The URI associated with this extension. For extension models
		/// with serialized elements, this URI is used as an XML namespace
		/// corresponding to the elements serialized with the model.
		/// </summary>
		public readonly string NamespaceUri;
		/// <summary>
		/// The location of the assembly containing this extension. If this
		/// is not set, then it is assumed that the DomainModelClass is contained
		/// in the core assembly.
		/// </summary>
		public readonly string CodeBase;
		/// <summary>
		/// The full .NET assembly name of the assembly containing this extension.
		/// If this is is not set, then it is assumed that the DomainModelClass is
		/// contained in the core assembly.
		/// </summary>
		public readonly string AssemblyName;
		/// <summary>
		/// The class name the extension's domain model.
		/// </summary>
		public readonly string DomainModelClass;
		/// <summary>
		/// <see cref="ExtensionModelOptions"/> specifying load and display characteristics
		/// of the extension model.
		/// </summary>
		public readonly ExtensionModelOptions Options;
		/// <summary>
		/// Create a new <see cref="ExtensionModelData"/>
		/// </summary>
		/// <param name="namespaceUri">The URI associated with this extension.
		/// For extension models with serialized elements, this URI is used as
		/// an XML namespace corresponding to the elements serialized with the model.</param>
		/// <param name="codeBase">The location of the assembly containing this
		/// extension. If this is not set, then it is assumed that the DomainModelClass
		/// is contained in the core assembly.</param>
		/// <param name="assemblyName">The full .NET assembly name of the assembly
		/// containing this extension. If this is is not set, then it is assumed that
		/// the DomainModelClass is contained in the core assembly.</param>
		/// <param name="domainModelClass">The class name the extension's domain model.</param>
		/// <param name="options"><see cref="ExtensionModelOptions"/> specifying load and
		/// display characteristics of the extension model.</param>
		public ExtensionModelData(string namespaceUri, string codeBase, string assemblyName, string domainModelClass, ExtensionModelOptions options)
		{
			NamespaceUri = namespaceUri;
			CodeBase = codeBase;
			AssemblyName = assemblyName;
			DomainModelClass = domainModelClass;
			Options = options;
		}
		/// <summary>
		/// Load extension model settings from the registry. The loader checks
		/// for information under this key in both the local machine (HKEY_LOCAL_MACHINE)
		/// and user (HKEY_CURRENT_USER) registry hives.
		/// </summary>
		/// <remarks>The registry key is assumed to contain keys with subkeys corresponding
		/// to the extension namespace names. In addition to the key names, the supported values
		/// retrieved are Assembly, Class, CodeBase, AutoLoadNamespace, SecondaryNamespace</remarks>
		/// <param name="keyName">The registry key name from either the application and user roots
		/// or the provided root keys.</param>
		/// <param name="localMachineRootKey">An alternate open key in the local machine hive. The provided
		/// <paramref name="keyName"/> is relative to this key.</param>
		/// <param name="userRootKey">An alternate open key in the current user hive. The provided
		/// <paramref name="keyName"/> is relative to this key.</param>
		/// <returns>List of unvalidated raw information. If the list contains duplicates,
		/// then the last entry for a given namespace should be given priority.</returns>
		public static IList<ExtensionModelData> LoadFromRegistry(string keyName, RegistryKey localMachineRootKey, RegistryKey userRootKey)
		{
			List<ExtensionModelData> retVal = null;
			bool currentUserPass = false;
			for (; ; )
			{
				RegistryKey rootKey = currentUserPass ? (userRootKey ?? Registry.CurrentUser) : (localMachineRootKey ?? Registry.LocalMachine);
				using (RegistryKey extensionsKey = rootKey.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (extensionsKey != null)
					{
						string[] extensionNamespaces = extensionsKey.GetSubKeyNames();
						foreach (string extensionNamespace in extensionNamespaces)
						{
							using (RegistryKey extensionKey = extensionsKey.OpenSubKey(extensionNamespace, RegistryKeyPermissionCheck.ReadSubTree))
							{
								ExtensionModelOptions options = ExtensionModelOptions.None;
								string domainModelClass = extensionKey.GetValue("Class") as string;

								object valueObject = extensionKey.GetValue("SecondaryNamespace");
								if (valueObject != null && ((int)valueObject) == 1)
								{
									options |= ExtensionModelOptions.Secondary;
								}
								valueObject = extensionKey.GetValue("AutoLoadNamespace");
								if (valueObject != null && ((int)valueObject) == 1)
								{
									options |= ExtensionModelOptions.AutoLoad;
								}
								valueObject = extensionKey.GetValue("NonGenerative");
								if (valueObject != null && ((int)valueObject) == 1)
								{
									options |= ExtensionModelOptions.NonGenerative;
								}
								(retVal ?? (retVal = new List<ExtensionModelData>())).Add(new ExtensionModelData(
									extensionNamespace,
									extensionKey.GetValue("CodeBase") as string,
									extensionKey.GetValue("Assembly") as string,
									extensionKey.GetValue("Class") as string,
									options));
							}
						}
					}
				}
				if (currentUserPass)
				{
					break;
				}
				currentUserPass = true;
			}
			return retVal;
		}
		/// <summary>
		/// Retrieve the <see cref="DomainModel"/>-derived type for this extension.
		/// </summary>
		/// <returns>A <see cref="Type"/> instance, or <see langword="null"/> if the
		/// specified information is not a <see cref="DomainModel"/>. Can also throw
		/// any exception associated with model loads.</returns>
		public Type ResolveExtensionDomainModel()
		{
			string extensionTypeString = DomainModelClass;
			if (string.IsNullOrEmpty(extensionTypeString))
			{
				// If we don't have an extension type name, just return null
				return null;
			}

			string assemblyValue = AssemblyName;
			string codeBaseValue = CodeBase;
			if (string.IsNullOrEmpty(assemblyValue) && string.IsNullOrEmpty(codeBaseValue))
			{
				// Extension is registered in this assembly
				return typeof(ExtensionModelData).Assembly.GetType(extensionTypeString, true, false);
			}
			else
			{
				AssemblyName extensionAssemblyName;
				string extensionAssemblyNameString = assemblyValue;
				if (!string.IsNullOrEmpty(extensionAssemblyNameString))
				{
					extensionAssemblyName = new AssemblyName(extensionAssemblyNameString);
				}
				else
				{
					extensionAssemblyName = new AssemblyName();
				}
				extensionAssemblyName.CodeBase = codeBaseValue;

				Assembly extensionAssembly = Assembly.Load(extensionAssemblyName);
				Type extensionType = extensionAssembly.GetType(extensionTypeString, true, false);

				if (extensionType.IsSubclassOf(typeof(DomainModel)))
				{
					NORMAExtensionLoadKeyAttribute.VerifyAssembly(extensionType);
					return extensionType;
				}
			}
			return null;
		}
	}
	#endregion // ExtensionModelData struct
	#region ExtensionModelBinding class
	/// <summary>
	/// Contains information about an ORM extension.
	/// </summary>
	public struct ExtensionModelBinding : IEquatable<ExtensionModelBinding>, IComparable<ExtensionModelBinding>
	{
		private readonly string myNamespaceUri;
		private readonly Type myType;
		private readonly ICollection<Guid> myExtendsIds;
		private readonly Guid myDomainModelId;
		private readonly ExtensionModelOptions myOptions;
		private ICollection<Guid> myAlsoLoadIds;
		/// <summary>
		/// Initializes a new instance of <see cref="ExtensionModelBinding"/>.
		/// </summary>
		/// <param name="namespaceUri">The XML namespace URI of the <see cref="ExtensionModelBinding"/>.</param>
		/// <param name="type">The <see cref="Type"/> of the <see cref="ExtensionModelBinding"/>.</param>
		/// <param name="options">The <see cref="ExtensionModelOptions"/> for this model.</param>
		public ExtensionModelBinding(string namespaceUri, Type type, ExtensionModelOptions options)
		{
			// Verify that if the domain serializes elements, then it serializes this one
			if (0 == (options & ExtensionModelOptions.AutoLoad))
			{
				object[] namespaceAttributes = type.GetCustomAttributes(typeof(CustomSerializedXmlSchemaAttribute), false);
				bool foundMatch = false;
				int count;
				if (namespaceAttributes != null && (count = namespaceAttributes.Length) != 0)
				{
					for (int i = 0; i < count; ++i)
					{
						if (((CustomSerializedXmlSchemaAttribute)namespaceAttributes[i]).XmlNamespace == namespaceUri)
						{
							foundMatch = true;
							break;
						}
					}
				}

#pragma warning disable 618
				// UNDONE: Remove CustomSerializedXmlNamespacesAttribute support
				if (!foundMatch)
				{
					namespaceAttributes = type.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
					if (namespaceAttributes != null && namespaceAttributes.Length != 0)
					{
						foreach (string testNamespace in (CustomSerializedXmlNamespacesAttribute)namespaceAttributes[0])
						{
							if (testNamespace == namespaceUri)
							{
								foundMatch = true;
								break;
							}
						}
					}
				}
#pragma warning restore 618

				if (!foundMatch)
				{
					// Bogus request, return and leave IsValidExtension false
					this = default(ExtensionModelBinding);
					return;
				}
			}
			this.myNamespaceUri = namespaceUri;
			this.myType = type;
#if VISUALSTUDIO_10_0
			object[] extendsAttributes = type.GetCustomAttributes(typeof(DependsOnDomainModelAttribute), false);
#else
			object[] extendsAttributes = type.GetCustomAttributes(typeof(ExtendsDomainModelAttribute), false);
#endif
			Guid[] extendsIds = new Guid[extendsAttributes.Length];
			for (int i = 0; i < extendsAttributes.Length; ++i)
			{
#if VISUALSTUDIO_10_0
				// UNDONE: VS2010 This maps from the type to the identifier so that
				// we can retrieve the type from the identifier later on. Consider
				// branching the loader for VS2010 so that it runs directly off types
				// instead of domain model identifiers.
				Type extendedModelType = ((DependsOnDomainModelAttribute)extendsAttributes[i]).ExtendedDomainModelType;
				object[] extensionIdAttributes = extendedModelType.GetCustomAttributes(typeof(DomainObjectIdAttribute), false);
				extendsIds[i] = (extensionIdAttributes.Length != 0) ? ((DomainObjectIdAttribute)extensionIdAttributes[0]).Id : Guid.Empty;
#else
				extendsIds[i] = ((ExtendsDomainModelAttribute)extendsAttributes[i]).ExtendedModelId;
#endif
			}
			myExtendsIds = Array.AsReadOnly(extendsIds);
			object[] domainObjectIdAttributes = type.GetCustomAttributes(typeof(DomainObjectIdAttribute), false);
			myDomainModelId = (domainObjectIdAttributes.Length != 0) ? ((DomainObjectIdAttribute)domainObjectIdAttributes[0]).Id : Guid.Empty;
			myAlsoLoadIds = null;
			myOptions = options;
		}
		/// <summary>
		/// The XML namespace URI of this <see cref="ExtensionModelBinding"/>.
		/// </summary>
		public string NamespaceUri
		{
			get
			{
				return this.myNamespaceUri;
			}
		}
		/// <summary>
		/// The <see cref="Type"/> of this <see cref="ExtensionModelBinding"/>.
		/// </summary>
		public Type Type
		{
			get
			{
				return this.myType;
			}
		}
		/// <summary>
		/// The identifiers for the <see cref="DomainModelInfo"/> models
		/// extended by this extension.
		/// </summary>
		public ICollection<Guid> ExtendsDomainModelIds
		{
			get
			{
				return myExtendsIds;
			}
		}
		/// <summary>
		/// The identifer for the <see cref="DomainClassInfo"/> associated
		/// with this extension model
		/// </summary>
		public Guid DomainModelId
		{
			get
			{
				return myDomainModelId;
			}
		}
		/// <summary>
		/// Return a list of ids for extensions that should also be loaded. Can be null.
		/// </summary>
		public ICollection<Guid> AlsoLoadIds
		{
			get
			{
				return myAlsoLoadIds;
			}
			set
			{
				myAlsoLoadIds = value;
			}
		}
		/// <summary>
		/// Returns <see langword="true"/> if the extension has complete information
		/// </summary>
		public bool IsValidExtension
		{
			get
			{
				return myType != null && myDomainModelId != Guid.Empty;
			}
		}
		/// <summary>
		/// Returns <see langword="true"/> if the  extension is secondary, meaning that it is not visible
		/// in the Extension Manager dialog and automatically turned off by the Extension Manager when all
		/// non-secondary extensions that use it are turned off.
		/// </summary>
		public bool IsSecondary
		{
			get
			{
				return 0 != (myOptions & ExtensionModelOptions.Secondary);
			}
		}
		/// <summary>
		/// True if the extension is always loaded. Generally used for extensions
		/// that contribute services but not elements.
		/// </summary>
		public bool IsAutoLoad
		{
			get
			{
				return 0 != (myOptions & ExtensionModelOptions.AutoLoad);
			}
		}
		/// <summary>
		/// True if the extension is not needed for code generation scenarios.
		/// Generally used for extensions that contribute display or editing elements.
		/// This takes precedence over <see cref="IsAutoLoad"/> for models loaded
		/// specifically for code generation.
		/// </summary>
		public bool IsNonGenerative
		{
			get
			{
				return 0 != (myOptions & ExtensionModelOptions.NonGenerative);
			}
		}
		/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
		public override bool Equals(object obj)
		{
			return (obj is ExtensionModelBinding) && this.Equals((ExtensionModelBinding)obj);
		}
		/// <summary>See <see cref="Object.GetHashCode"/>.</summary>
		public override int GetHashCode()
		{
			return this.myNamespaceUri.GetHashCode();
		}
		/// <summary>
		/// Checks if two ExtensionModelBinding's are equals.
		/// </summary>
		/// <param name="other">the namespace to compare.</param>
		/// <returns>true if they equal false if they don't.</returns>
		public bool Equals(ExtensionModelBinding other)
		{
			return this.myNamespaceUri.Equals(other.myNamespaceUri);
		}
		/// <summary>
		/// Compares the two namespaceURI's.
		/// </summary>
		/// <param name="other">The <see cref="ExtensionModelBinding"/> you would like to compare.</param>
		/// <returns>standard compare logic.</returns>
		public int CompareTo(ExtensionModelBinding other)
		{
			return this.myNamespaceUri.CompareTo(other.myNamespaceUri);
		}
	}
	#endregion // ExtensionModelBinding class
	#region ExtensionLoader class
	/// <summary>
	/// Class to manage loading of extension models
	/// </summary>
	public sealed class ExtensionLoader
	{
		#region Member Variables
		private IDictionary<string, ExtensionModelBinding> myAvailableExtensions;
		private string[] myAutoLoadExtensions;
		private IDictionary<Guid, string> myExtensionIdToExtensionNameMap;
		private readonly IDictionary<Guid, Type> myStandardDomainModelsMap;
		private IDictionary<string, Exception> myUnloadableExtensionErrors;
		private ExtensionLoader myNonGenerativeLoader;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a new <see cref="ExtensionLoader"/> to manage the requested extensions
		/// </summary>
		/// <param name="extensionModels">A list of <see cref="ExtensionModelData"/> to load.
		/// The list can be loaded from the registry using the <see cref="ExtensionModelData.LoadFromRegistry"/> method.</param>
		public ExtensionLoader(IList<ExtensionModelData> extensionModels)
		{
			Dictionary<string, ExtensionModelBinding> availableExtensions = new Dictionary<string, ExtensionModelBinding>();
			Dictionary<string, Exception> unloadableExtensions = null;
			string[] autoLoadExtensions = null;
			IDictionary<Guid, string> extensionIdToExtensionNameMap = null;
			if (extensionModels != null)
			{
				// Determine all supported assemblies
				int autoLoadCount = 0;
				foreach (ExtensionModelData extensionData in extensionModels)
				{
					Type domainModelType = null;
					ExtensionModelBinding extensionType;
					string extensionNamespace;
					ExtensionModelOptions options;
					try
					{
						domainModelType = extensionData.ResolveExtensionDomainModel();
					}
					catch (Exception ex)
					{
						if (ex is NORMAExtensionLoadException ||
							ex is TypeInitializationException ||
							ex is TypeLoadException)
						{
							(unloadableExtensions ?? (unloadableExtensions = new Dictionary<string, Exception>()))[extensionData.NamespaceUri] = ex;
						}
						else
						{
							throw;
						}
					}
					
					if (null != domainModelType &&
						(extensionType = new ExtensionModelBinding(
							extensionNamespace = extensionData.NamespaceUri,
							domainModelType,
							options = extensionData.Options)).IsValidExtension)
					{
						if (0 != (options & ExtensionModelOptions.AutoLoad))
						{
							++autoLoadCount;
						}

						availableExtensions[extensionNamespace] = default(ExtensionModelBinding);
						ResolveAlsoLoadedDomainModels(ref extensionType, availableExtensions);
						availableExtensions[extensionNamespace] = extensionType;
					}
				}

				// Track automatically loaded assemblies and create a mapping from
				// the domain model identifier to the extension namespace Uri.
				extensionIdToExtensionNameMap = new Dictionary<Guid, string>(availableExtensions.Count);
				if (autoLoadCount != 0)
				{
					autoLoadExtensions = new string[autoLoadCount];
					autoLoadCount = 0;
				}
				foreach (KeyValuePair<string, ExtensionModelBinding> pair in availableExtensions)
				{
					ExtensionModelBinding extensionType = pair.Value;
					extensionIdToExtensionNameMap.Add(extensionType.DomainModelId, pair.Key);
					if (extensionType.IsAutoLoad)
					{
						autoLoadExtensions[autoLoadCount] = extensionType.NamespaceUri;
						++autoLoadCount;
					}
				}
			}
			myAvailableExtensions = availableExtensions;
			myAutoLoadExtensions = autoLoadExtensions ?? new string[0];
			myExtensionIdToExtensionNameMap = extensionIdToExtensionNameMap ?? new Dictionary<Guid, string>();
			myUnloadableExtensionErrors = unloadableExtensions;

			// Add standard models
			// Any model change here that has toolbox information requires a corresponding change in ORMPackage.GetToolboxProviderInfoMap
			Dictionary<Guid, Type> standardModels = new Dictionary<Guid, Type>(6);
			standardModels.Add(ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.DomainModelId, typeof(ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel));
			standardModels.Add(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel.DomainModelId, typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel));
			standardModels.Add(Microsoft.VisualStudio.Modeling.Diagrams.CoreDesignSurfaceDomainModel.DomainModelId, typeof(Microsoft.VisualStudio.Modeling.Diagrams.CoreDesignSurfaceDomainModel));
			standardModels.Add(ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.DomainModelId, typeof(ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel));
			// UNDONE: Temporary until the report validation is moved into a separate dll. See orm2 ticket #315
			standardModels.Add(ObjectModel.Verbalization.HtmlReport.DomainModelId, typeof(ObjectModel.Verbalization.HtmlReport));
			standardModels.Add(ORMSolutions.ORMArchitect.Framework.Shell.DiagramSurvey.DomainModelId, typeof(ORMSolutions.ORMArchitect.Framework.Shell.DiagramSurvey));
			myStandardDomainModelsMap = standardModels;

			// Add assembly resolution callbacks to support extension assemblies
			// that are not in the global assembly cache.
			Dictionary<string, Assembly> knownAssemblies = new Dictionary<string, Assembly>(3 + availableExtensions.Count, StringComparer.Ordinal);
			Assembly knownAssembly = typeof(ExtensionLoader).Assembly;
			knownAssemblies[knownAssembly.FullName] = knownAssembly;

			// Converter resolution relies on Type.GetType, which is failing for modeling
			// SDK library dependencies when they are not loaded from the Visual Studio probing
			// path. Add these dependencies as well.
			knownAssembly = typeof(Microsoft.VisualStudio.Modeling.ModelElement).Assembly;
			knownAssemblies[knownAssembly.FullName] = knownAssembly;
			knownAssembly = typeof(Microsoft.VisualStudio.Modeling.Diagrams.PresentationElement).Assembly;
			knownAssemblies[knownAssembly.FullName] = knownAssembly;

			foreach (ExtensionModelBinding extensionType in availableExtensions.Values)
			{
				knownAssembly = extensionType.Type.Assembly;
				knownAssemblies[knownAssembly.FullName] = knownAssembly;
			}
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
			{
				// This supports retrieving types from our assembly and our extension assemblies,
				// even if they aren't in the normal assembly probing path.
				Assembly resolvedAssembly;
				knownAssemblies.TryGetValue(e.Name, out resolvedAssembly);
				return resolvedAssembly;
			};
		}
		/// <summary>
		/// Private entry point used to create the non-generative loader
		/// from a full loader.
		/// </summary>
		private ExtensionLoader(ExtensionLoader fullLoader)
		{
			// Return previously reduced set on deeper request
			myNonGenerativeLoader = this;
			myExtensionIdToExtensionNameMap = fullLoader.myExtensionIdToExtensionNameMap; // This may be too big, but is harmless

			// Make this recursive, so models that depend on non-generative models are
			// automatically removed as well as any 'also loaded' models. This reduces
			// the number of models we need to explicitly register as non-generative.
			// A false value here indicates that the model is currently being processed
			// and is meant to block uncontrolled recursion.
			Dictionary<Guid, bool> nonGenerativeIds = new Dictionary<Guid, bool>();

			// Standard models are very limited in this mode
			Dictionary<Guid, Type> standardModels = new Dictionary<Guid, Type>(2);
			standardModels.Add(ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.DomainModelId, typeof(ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel));
			standardModels.Add(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel.DomainModelId, typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel));

			// The other standard models are considered non-generative
			nonGenerativeIds.Add(Microsoft.VisualStudio.Modeling.Diagrams.CoreDesignSurfaceDomainModel.DomainModelId, true);
			nonGenerativeIds.Add(ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.DomainModelId, true);
			// UNDONE: Temporary. See comment in other constructor
			nonGenerativeIds.Add(ObjectModel.Verbalization.HtmlReport.DomainModelId, true);
			nonGenerativeIds.Add(ORMSolutions.ORMArchitect.Framework.Shell.DiagramSurvey.DomainModelId, true);
			myStandardDomainModelsMap = standardModels;

			// Recursively get non-generative binding information for all extensions
			IDictionary<string, ExtensionModelBinding> originalAvailableExtensions = fullLoader.myAvailableExtensions;
			foreach (ExtensionModelBinding binding in originalAvailableExtensions.Values)
			{
				fullLoader.TestIsNonGenerativeBinding(binding, nonGenerativeIds, false);
			}

			// Set available extensions
			Dictionary<string, ExtensionModelBinding> availableExtensions = new Dictionary<string, ExtensionModelBinding>();
			foreach (KeyValuePair<string, ExtensionModelBinding> kvp in originalAvailableExtensions)
			{
				bool isNonGenerative;
				if (!nonGenerativeIds.TryGetValue(kvp.Value.DomainModelId, out isNonGenerative) || !isNonGenerative)
				{
					availableExtensions[kvp.Key] = kvp.Value;
				}
			}
			myAvailableExtensions = availableExtensions;

			// Fill in autoload settings
			string[] originalAutoLoads = fullLoader.myAutoLoadExtensions;
			string[] newAutoLoads = null;
			if (originalAutoLoads != null)
			{
				int originalAutoLoadCount = originalAutoLoads.Length;
				if (originalAutoLoadCount != 0)
				{
					int newAutoLoadCount = originalAutoLoadCount;
					BitTracker nonGenerativeTracker = new BitTracker(originalAutoLoadCount);
					ExtensionModelBinding binding;
					bool isNonGenerative;
					for (int i = 0; i < originalAutoLoadCount; ++i)
					{
						if (originalAvailableExtensions.TryGetValue(originalAutoLoads[i], out binding) &&
							nonGenerativeIds.TryGetValue(binding.DomainModelId, out isNonGenerative) &&
							isNonGenerative)
						{
							if (--newAutoLoadCount == 0)
							{
								break;
							}
							nonGenerativeTracker[i] = true;
						}
					}

					if (newAutoLoadCount != 0)
					{
						if (newAutoLoadCount == originalAutoLoadCount)
						{
							newAutoLoads = originalAutoLoads;
						}
						else
						{
							newAutoLoads = new string[newAutoLoadCount];
							for (int i = 0, newIndex = 0; i < originalAutoLoadCount; ++i)
							{
								if (!nonGenerativeTracker[i])
								{
									newAutoLoads[newIndex] = originalAutoLoads[i];
									++newIndex;
								}
							}
						}
					}
				}
			}
			myAutoLoadExtensions = newAutoLoads ?? new string[0];
		}
		private bool TestIsNonGenerativeBinding(ExtensionModelBinding binding, Dictionary<Guid, bool> nonGenerativeIds, bool forceNonGenerative)
		{
			bool isNonGenerative;
			Guid modelId = binding.DomainModelId;
			if (nonGenerativeIds.TryGetValue(binding.DomainModelId, out isNonGenerative))
			{
				return isNonGenerative;
			}

			isNonGenerative = forceNonGenerative || binding.IsNonGenerative;
			nonGenerativeIds[modelId] = isNonGenerative; // Defensive, block recursion
			string extensionName;
			ExtensionModelBinding extensionBinding;
			if (!isNonGenerative)
			{
				ICollection<Guid> extends = binding.ExtendsDomainModelIds;
				if (extends != null && extends.Count != 0)
				{
					foreach (Guid extendsId in extends)
					{
						// Note that we enter all standard non-generative ids up front, so we only recurse
						// on non-standard extensions.
						bool knownNonGenerativeId;
						if (nonGenerativeIds.TryGetValue(extendsId, out knownNonGenerativeId))
						{
							if (knownNonGenerativeId)
							{
								nonGenerativeIds[modelId] = isNonGenerative = true;
								break;
							}
						}

						if (myExtensionIdToExtensionNameMap.TryGetValue(extendsId, out extensionName) &&
							myAvailableExtensions.TryGetValue(extensionName, out extensionBinding))
						{
							if (TestIsNonGenerativeBinding(extensionBinding, nonGenerativeIds, false))
							{
								nonGenerativeIds[modelId] = isNonGenerative = true;
								break;
							}
						}
					}
				}
			}

			if (isNonGenerative)
			{
				// Also remove 'also loaded' models.
				ICollection<Guid> alsoLoadIds = binding.AlsoLoadIds;
				if (alsoLoadIds != null && alsoLoadIds.Count != 0)
				{
					foreach (Guid alsoLoadId in alsoLoadIds)
					{
						bool alsoLoadNonGenerative;
						if (nonGenerativeIds.TryGetValue(alsoLoadId, out alsoLoadNonGenerative))
						{
							if (alsoLoadNonGenerative)
							{
								continue;
							}
							nonGenerativeIds.Remove(alsoLoadId);
						}

						if (myExtensionIdToExtensionNameMap.TryGetValue(alsoLoadId, out extensionName) &&
							myAvailableExtensions.TryGetValue(extensionName, out extensionBinding))
						{
							TestIsNonGenerativeBinding(extensionBinding, nonGenerativeIds, true);
						}
					}
				}
			}
			return isNonGenerative;
		}
		/// <summary>
		/// Helper for the constructor to resolve <see cref="AlsoLoadDomainModelAttribute"/>
		/// </summary>,
		/// <param name="expandExtension">Get additional domain models for this extension.</param>
		/// <param name="availableExtensions">The currently known domain models. A previously-bound extension
		/// will not be reprocessed.</param>
		private static void ResolveAlsoLoadedDomainModels(ref ExtensionModelBinding expandExtension, Dictionary<string, ExtensionModelBinding> availableExtensions)
		{
			// Add 'also load' extensions
			object[] alsoLoadedAttributes = expandExtension.Type.GetCustomAttributes(typeof(AlsoLoadDomainModelAttribute), false);
			if (alsoLoadedAttributes != null &&
				alsoLoadedAttributes.Length != 0)
			{
				List<Guid> alsoLoadedIds = null;
				for (int i = 0; i < alsoLoadedAttributes.Length; ++i)
				{
					AlsoLoadDomainModelAttribute alsoLoad = (AlsoLoadDomainModelAttribute)alsoLoadedAttributes[i];
					string testURI = alsoLoad.NamespaceURI;
					ExtensionModelBinding alsoBound;
					if (!string.IsNullOrEmpty(testURI) &&
						!availableExtensions.ContainsKey(testURI) &&
						(alsoBound = new ExtensionModelBinding(testURI, alsoLoad.AlsoLoadType, ExtensionModelOptions.Secondary | (alsoLoad.IsNonGenerative ? ExtensionModelOptions.NonGenerative : ExtensionModelOptions.None))).IsValidExtension)
					{
						// Record and recurse
						(alsoLoadedIds ?? (alsoLoadedIds = new List<Guid>())).Add(alsoBound.DomainModelId);
						availableExtensions[testURI] = default(ExtensionModelBinding);
						ResolveAlsoLoadedDomainModels(ref alsoBound, availableExtensions);
						availableExtensions[testURI] = alsoBound;
					}
				}
				if (alsoLoadedIds != null)
				{
					expandExtension.AlsoLoadIds = alsoLoadedIds;
				}
			}
		}
		#endregion // Constructor
		#region Accessors
		/// <summary>
		/// Retrieves the <see cref="DomainModel"/> for a specific extension namespace.
		/// </summary>
		/// <remarks>If a <see cref="DomainModel"/> cannot be found for a namespace, <see langword="null"/> is returned.</remarks>
		public ExtensionModelBinding? GetExtensionDomainModel(string extensionNamespace)
		{
			ExtensionModelBinding extensionType;
			return myAvailableExtensions.TryGetValue(extensionNamespace, out extensionType) ? new ExtensionModelBinding?(extensionType) : null;
		}
		/// <summary>
		/// Enumerate all models available to the ORM designer
		/// </summary>
		/// <returns>See <see cref="IEnumerable{Type}"/></returns>
		public IEnumerable<Type> AvailableDomainModels
		{
			get
			{
				foreach (Type standardType in myStandardDomainModelsMap.Values)
				{
					yield return standardType;
				}
				foreach (ExtensionModelBinding extension in myAvailableExtensions.Values)
				{
					yield return extension.Type;
				}
			}
		}
		/// <summary>
		/// Get the standard models that are always loaded with the tool
		/// </summary>
		public ICollection<Type> StandardDomainModels
		{
			get
			{
				return myStandardDomainModelsMap.Values;
			}
		}
		/// <summary>
		/// This method cycles through the registered Custom Extensions.
		/// It then returns an IList of ExtensionModelBinding. containing all the Types of the Custom Extensions.
		/// </summary>
		/// <returns>An IList of registered ExtensionModelBindings.</returns>
		public IDictionary<string, ExtensionModelBinding> AvailableCustomExtensions
		{
			get
			{
				return myAvailableExtensions;
			}
		}
		/// <summary>
		/// Retrieve a dictionary of errors, keyed by the extension namespace,
		/// for extensions that could not be found or did not pass initial load validation.
		/// </summary>
		public IDictionary<string, Exception> UnloadableExtensionErrors
		{
			get
			{
				return myUnloadableExtensionErrors;
			}
		}
		/// <summary>
		/// Load failures have been reported, clear them so they aren't reported again.
		/// </summary>
		public void ClearUnloadedExtensionErrors()
		{
			myUnloadableExtensionErrors = null;
		}
		#endregion // Accessors
		#region Methods
		/// <summary>
		/// Get the domain model name corresponding to an extension domain model identifier.
		/// Returns <see langword="null"/> if the domain model is not an available extension.
		/// </summary>
		public string MapExtensionDomainModelToName(Guid domainModelId)
		{
			string retVal = null;
			myExtensionIdToExtensionNameMap.TryGetValue(domainModelId, out retVal);
			return retVal;
		}
		/// <summary>
		/// Get an array of required types in normalized order for the given set
		/// of <paramref name="extensionModels"/>.
		/// </summary>
		/// <param name="extensionModels">Extension models to add. Should be preverified with
		/// <see cref="VerifyRequiredExtensions"/>.</param>
		/// <returns>An array of domain model types suitable for use with the <see cref="Store.LoadDomainModels"/> method.</returns>
		public Type[] GetRequiredDomainModels(IDictionary<string, ExtensionModelBinding> extensionModels)
		{
			Type[] retVal;
			ICollection<Type> standardModelTypes = myStandardDomainModelsMap.Values;
			int standardModelCount = standardModelTypes.Count;
			if (extensionModels != null)
			{
				retVal = new Type[standardModelCount + (extensionModels != null ? extensionModels.Count : 0)];
				standardModelTypes.CopyTo(retVal, 0);
				int extensionIndex = standardModelCount;
				foreach (ExtensionModelBinding extensionBinding in extensionModels.Values)
				{
					retVal[extensionIndex] = extensionBinding.Type;
					++extensionIndex;
				}
			}
			else
			{
				retVal = new Type[standardModelCount];
				standardModelTypes.CopyTo(retVal, 0);
			}
			// See comments wrt/ordering in ORMDesignerDocData.GetDomainModels
			Array.Sort<Type>(
				retVal,
				delegate(Type x, Type y)
				{
					return x.FullName.CompareTo(y.FullName);
				});
			return retVal;
		}
		/// <summary>
		/// Extend the set of required extensions to include any dependencies
		/// </summary>
		/// <param name="extensions">Currently loaded extensions. May be created if null to add auto-load extensions.</param>
		public void VerifyRequiredExtensions(ref Dictionary<string, ExtensionModelBinding> extensions)
		{
			IDictionary<string, ExtensionModelBinding> availableExtensions = myAvailableExtensions;
			string[] autoLoadExtensions = myAutoLoadExtensions;

			// First get all autoload extensions
			for (int i = 0; i < autoLoadExtensions.Length; ++i)
			{
				string extensionNamespace = autoLoadExtensions[i];
				if (extensions == null)
				{
					extensions = new Dictionary<string, ExtensionModelBinding>();
					extensions[extensionNamespace] = availableExtensions[extensionNamespace];
				}
				else if (!extensions.ContainsKey(extensionNamespace))
				{
					extensions[extensionNamespace] = availableExtensions[extensionNamespace];
				}
			}
			if (extensions == null)
			{
				return;
			}

			IDictionary<Guid, string> idToExtensionNameMap = myExtensionIdToExtensionNameMap;
			IDictionary<Guid, Type> standardModelsMap = myStandardDomainModelsMap;

			// Get a starting keyset we can iterate so the enumerators don't cry foul
			ICollection<string> startKeysCollection = extensions.Keys;
			int startKeysCount = startKeysCollection.Count;
			if (startKeysCount == 0)
			{
				return;
			}
			string[] startKeys = new string[startKeysCount];
			startKeysCollection.CopyTo(startKeys, 0);

			// Recursively verify dependencies for each starting element
			for (int i = 0; i < startKeys.Length; ++i)
			{
				VerifyExtensions(startKeys[i], extensions, availableExtensions, idToExtensionNameMap, standardModelsMap);
			}
		}
		/// <summary>
		/// Combine extension models already in a <see cref="Store"/> with additional
		/// extension model <see cref="ExtensionModelBinding">bindings</see>.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> to get existing extensions from.</param>
		/// <param name="bindings">A dictionary starting with additional extensions. This is extended
		/// to include additional extensions loaded in the store.</param>
		public void AddRequiredExtensions(Store store, ref Dictionary<string, ExtensionModelBinding> bindings)
		{
			VerifyRequiredExtensions(ref bindings);
			IDictionary<Guid, string> idToExtensionNameMap = myExtensionIdToExtensionNameMap;
			IDictionary<Guid, Type> standardModelsMap = myStandardDomainModelsMap;
			IDictionary<string, ExtensionModelBinding> availableExtensions = myAvailableExtensions;
			foreach (DomainModelInfo modelInfo in store.DomainDataDirectory.DomainModels)
			{
				Guid domainModelId = modelInfo.Id;
				string extensionNamespace;
				ExtensionModelBinding binding;
				if (!standardModelsMap.ContainsKey(domainModelId) &&
					idToExtensionNameMap.TryGetValue(domainModelId, out extensionNamespace) &&
					!bindings.ContainsKey(extensionNamespace) &&
					availableExtensions.TryGetValue(extensionNamespace, out binding))
				{
					bindings.Add(extensionNamespace, binding);
				}
			}
		}
		/// <summary>
		/// Recursively add additional extension models. Helper function for <see cref="VerifyRequiredExtensions"/>
		/// </summary>
		private static void VerifyExtensions(string extensionNamespace, IDictionary<string, ExtensionModelBinding> targetExtensions, IDictionary<string, ExtensionModelBinding> availableExtensions, IDictionary<Guid, string> extensionModelMap, IDictionary<Guid, Type> standardModelMap)
		{
			ExtensionModelBinding extension = availableExtensions[extensionNamespace];
			ICollection<Guid> recurseExtensions = extension.ExtendsDomainModelIds;
			bool firstPass = true;
			while (recurseExtensions != null)
			{
				if (recurseExtensions.Count != 0)
				{
					foreach (Guid recurseExtensionId in recurseExtensions)
					{
						string recurseExtensionNamespace;
						if (extensionModelMap.TryGetValue(recurseExtensionId, out recurseExtensionNamespace) &&
							!standardModelMap.ContainsKey(recurseExtensionId) &&
							!targetExtensions.ContainsKey(recurseExtensionNamespace))
						{
							targetExtensions.Add(recurseExtensionNamespace, availableExtensions[recurseExtensionNamespace]);
							VerifyExtensions(recurseExtensionNamespace, targetExtensions, availableExtensions, extensionModelMap, standardModelMap);
						}
					}
				}
				recurseExtensions = firstPass ? extension.AlsoLoadIds : null;
				firstPass = false;
			}
		}
		/// <summary>
		/// A custom extension has failed to load. Remove the extension from the list
		/// of available extensions.
		/// </summary>
		/// <param name="unvailableExtensionType">The extension <see cref="Type"/>The
		/// extension that has failed to load.</param>
		/// <returns><see langword="true"/> if the extension was successfully removed.</returns>
		public bool CustomExtensionUnavailable(Type unvailableExtensionType)
		{
			IDictionary<string, ExtensionModelBinding> customExtensions = myAvailableExtensions;
			if (customExtensions != null)
			{
				foreach (KeyValuePair<string, ExtensionModelBinding> pair in customExtensions)
				{
					ExtensionModelBinding extensionType = pair.Value;
					if (extensionType.Type == unvailableExtensionType)
					{
						customExtensions.Remove(pair.Key);
						IDictionary<Guid, string> extensionIdMap = myExtensionIdToExtensionNameMap;
						if (extensionIdMap.ContainsKey(extensionType.DomainModelId))
						{
							extensionIdMap.Remove(extensionType.DomainModelId);
						}
						string[] autoloadExtensions = myAutoLoadExtensions;
						int autoloadExtensionLength;
						int removeExtensionIndex;
						if (0 != (autoloadExtensionLength = autoloadExtensions.Length) &&
							-1 != (removeExtensionIndex = Array.IndexOf<string>(autoloadExtensions, extensionType.NamespaceUri)))
						{
							string[] newExtensions = new string[autoloadExtensionLength - 1];
							if (autoloadExtensionLength > 1)
							{
								for (int i = 0; i < removeExtensionIndex; ++i)
								{
									newExtensions[i] = autoloadExtensions[i];
								}
								for (int i = removeExtensionIndex + 1; i < autoloadExtensionLength; ++i)
								{
									newExtensions[i - 1] = autoloadExtensions[i];
								}
							}
							myAutoLoadExtensions = newExtensions;
						}
						return true;
					}
				}
			}
			return false;
		}
		#endregion // Methods
		#region NonGenerative Models
		/// <summary>
		/// Get the set of extensions that are used for code generation
		/// but not for display purposes.
		/// </summary>
		public ExtensionLoader NonGenerativeLoader
		{
			get
			{
				ExtensionLoader loader = myNonGenerativeLoader;
				if (loader == null)
				{
					myNonGenerativeLoader = loader = new ExtensionLoader(this);
				}
				return loader;
			}
		}
		#endregion // NonGenerative Models
		#region Extension Stripping
		#region ExtensionStripperUtility class
		/// <summary>
		/// This is a custom callback class for the XSLT file that is
		/// responsible for adding or removing the custom extension namespaces to the ORM document.
		/// </summary>
		private sealed class ExtensionStripperUtility
		{
			private readonly string[] myNamespaces;
			private Dictionary<string, string> myAddedNamespaces;
			private IEnumerator<string> myEnumerator;
			private int myLastIdRemovalPhase;
			private int myCurrentIdRemovalPhase;
			private Dictionary<string, string> myRemovedIds;
			private static readonly Random myRandom = new Random();
			/// <summary>
			/// Default Constructor for the <see cref="ExtensionStripperUtility"/>.
			/// </summary>
			/// <param name="sortedNamespaces">An array of available namespaces. The array should be sorted with the
			/// default string sort.</param>
			public ExtensionStripperUtility(string[] sortedNamespaces)
			{
				myNamespaces = sortedNamespaces;
				myLastIdRemovalPhase = -1;
			}
			/// <summary>
			/// This method checks to see if the namespace was added
			/// </summary>
			/// <param name="namespaceUri">The namespace to check if it was added,</param>
			/// <returns>true if the namespace was added false if it was not.</returns>
			public bool WasNamespaceAdded(string namespaceUri)
			{
				Dictionary<string, string> addedNamespaces = myAddedNamespaces;
				return addedNamespaces != null && myAddedNamespaces.ContainsKey(namespaceUri);
			}
			/// <summary>
			/// This method adds the namespace to the selected list. for future reference.
			/// </summary>
			/// <param name="namespaceUri">the namespace you want to add.</param>
			public void AddNamespace(string namespaceUri)
			{
				Dictionary<string, string> addedNamespaces = myAddedNamespaces;
				if (addedNamespaces == null)
				{
					myAddedNamespaces = addedNamespaces = new Dictionary<string, string>();
					addedNamespaces.Add(namespaceUri, namespaceUri);
				}
				else
				{
					addedNamespaces[namespaceUri] = namespaceUri;
				}
			}
			/// <summary>
			/// This is an Iterator helper class to move accross the available namespaces
			/// you wish to add.
			/// </summary>
			/// <returns>The current namespace position if there is a next one. an empty string if there is not.</returns>
			public string GetNextSelectedNamespace()
			{
				IEnumerator<string> enumerator = myEnumerator ?? (myEnumerator = ((IEnumerable<string>)myNamespaces).GetEnumerator());
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
				else
				{
					myEnumerator = null;
					return string.Empty;
				}
			}
			/// <summary>
			/// This method checks if the namespace is currently active.
			/// </summary>
			/// <param name="namespaceUri">The namespace you wish to check for active status.</param>
			/// <returns>true if the namespace was active false if it was not.</returns>
			public bool IsNamespaceActive(string namespaceUri)
			{
				return Array.BinarySearch<string>(myNamespaces, namespaceUri) >= 0;
			}
			/// <summary>
			/// This is a Randomizer to get around the fact that we do not have unique identifiers
			/// for the different namespaces.
			/// </summary>
			/// <returns>the a psuedo random namespace extension to be used as the prefix.</returns>
			public static string GetRandomPrefix()
			{
				return "ormExtension" + myRandom.Next();
			}
			/// <summary>
			/// Remember id values so we can look them up quickly on a later pass
			/// </summary>
			public bool RemoveId(string idValue)
			{
				Dictionary<string, string> removedIds = myRemovedIds;
				if (removedIds == null)
				{
					myRemovedIds = removedIds = new Dictionary<string, string>();
				}
				else if (myRemovedIds.ContainsKey(idValue))
				{
					return false;
				}
				myLastIdRemovalPhase = myCurrentIdRemovalPhase;
				removedIds.Add(idValue, idValue);
				return true;
			}
			/// <summary>
			/// Begin a new id removal phase. Returns true if any ids
			/// have been removed since the last time this method was called.
			/// </summary>
			public bool BeginIdRemovalPhase()
			{
				if (myLastIdRemovalPhase == myCurrentIdRemovalPhase)
				{
					++myCurrentIdRemovalPhase;
					return true;
				}
				return false;
			}
			/// <summary>
			/// See if the attribute value corresponds to a removed identifier.
			/// </summary>
			/// <param name="attributeValue">Verify if the attribute value is a reference to
			/// a removed identifier.</param>
			/// <returns>true value is a removed identifier</returns>
			public bool IsRemovedId(string attributeValue)
			{
				Dictionary<string, string> removedIds = myRemovedIds;
				return removedIds != null && removedIds.ContainsKey(attributeValue);
			}
		}
		#endregion // ExtensionStripperUtility class
		#region Transform Management
		private static XslCompiledTransform myExtensionStripperTransform;
#if DEBUG_EXTENSIONSTRIPPER_TRANSFORM
		private static System.CodeDom.Compiler.TempFileCollection myDebugExtensionStripperTempFile;
#endif // DEBUG_EXTENSIONSTRIPPER_TRANSFORM
		private static readonly object LockObject = new object();
		/// <summary>
		/// This method grabs and compiles the XSLT transform that strips or adds custom extensions to the ORM file.
		/// </summary>
		/// <returns>The compiled XSLT tranform.</returns>
		private static XslCompiledTransform GetExtensionStripperTransform()
		{
			XslCompiledTransform retVal = myExtensionStripperTransform;
			if (retVal == null)
			{
				lock (LockObject)
				{
					retVal = myExtensionStripperTransform;
					if (retVal == null)
					{
						Type resourceType = typeof(ExtensionLoader);
						using (Stream transformStream = resourceType.Assembly.GetManifestResourceStream(resourceType, "ExtensionStripper.xslt"))
						{
#if DEBUG_EXTENSIONSTRIPPER_TRANSFORM 
							retVal = new XslCompiledTransform(true);
							System.CodeDom.Compiler.TempFileCollection tempFiles = new System.CodeDom.Compiler.TempFileCollection();
							string fileName = tempFiles.AddExtension("xslt");
							myDebugExtensionStripperTempFile = tempFiles;
							using (FileStream tempFile = new FileStream(fileName, FileMode.Create))
							{
								byte[] buffer = new byte[1024];
								int totalRead;
								do
								{
									totalRead = transformStream.Read(buffer, 0, 1024);
									tempFile.Write(buffer, 0, totalRead);
								} while (totalRead == 1024);
								tempFile.Flush();
							}
							IWin32Window ownerWindow = Utility.GetDialogOwnerWindow(ORMDesignerPackage.Singleton);
							if (DialogResult.Yes == MessageBox.Show(ownerWindow, "Debug extension stripper transform saved to temporary file:\r\n\r\n" + fileName + "\r\n\r\nChoose 'Yes' to place this file name on the clipboard and wait while you prepare to debug it by\r\n   -Opening the file in the debugger\r\n   -Formatting the file with the Format Document command (on the Edit/Advanced menu)\r\n   -Resaving the temporary file\r\n   -Adding breakpoints to the transform", "Extension Stripper Debugger", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
							{
								Clipboard.Clear();
								Clipboard.SetText(fileName);
								MessageBox.Show(ownerWindow, "Press OK when you are ready to load the transform and continue debugging.", "Extension Stripper Debugger", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							retVal.Load(fileName, XsltSettings.TrustedXslt, new XmlUrlResolver());
#else
							retVal = new XslCompiledTransform();
							using (XmlReader reader = XmlReader.Create(transformStream))
							{
								retVal.Load(reader, XsltSettings.TrustedXslt, new XmlUrlResolver());
							}
#endif
						}
						myExtensionStripperTransform = retVal;
					}
				}
			}
			return retVal;
		}
		#endregion // Transform Management
		#region CleanupStream method
		/// <summary>
		/// This method is responsible for cleaning the streamed ORM file.
		/// </summary>
		/// <param name="stream">The file stream that contains the ORM file.</param>
		/// <param name="standardTypes">The standard models that are not loaded as extensions</param>
		/// <param name="extensionTypes">A collection of extension types.</param>
		/// <param name="unrecognizedNamespaces">An editable list of unrecognized namespaces. If this is set,
		/// the namespaces will be verified after secondary namespaces from the extension types are validated.
		/// If no remaining unrecognized namespaces are left after validation, then the method will return null.
		/// Recognized namespaces will be removed from the list.</param>
		/// <returns>The cleaned stream.</returns>
		public static Stream CleanupStream(Stream stream, ICollection<Type> standardTypes, ICollection<ExtensionModelBinding> extensionTypes, IList<string> unrecognizedNamespaces)
		{
			MemoryStream outputStream = new MemoryStream((int)stream.Length);
			XsltArgumentList argList = new XsltArgumentList();

			// Get all of the custom serialization attributes for the standard and
			// extension types. The serialization engine does not serialize elements
			// for types without a serialization attribute, so there is no need to
			// look at standard or extension types without this attribute.
			List<string> namespaceList = new List<string>();
			foreach (Type standardType in standardTypes)
			{
				object[] attributes = standardType.GetCustomAttributes(typeof(CustomSerializedXmlSchemaAttribute), false);
				if (attributes != null)
				{
					for (int i = 0; i < attributes.Length; ++i)
					{
						namespaceList.Add(((CustomSerializedXmlSchemaAttribute)attributes[i]).XmlNamespace);
					}
				}
			}

			foreach (ExtensionModelBinding extensionType in extensionTypes)
			{
				object[] attributes = extensionType.Type.GetCustomAttributes(typeof(CustomSerializedXmlSchemaAttribute), false);
				if (attributes != null)
				{
					for (int i = 0; i < attributes.Length; ++i)
					{
						namespaceList.Add(((CustomSerializedXmlSchemaAttribute)attributes[i]).XmlNamespace);
					}
				}
			}

#pragma warning disable 618
			// UNDONE: Remove CustomSerializedXmlNamespacesAttribute support
			List<CustomSerializedXmlNamespacesAttribute> namespaceAttributes = new List<CustomSerializedXmlNamespacesAttribute>();
			foreach (Type standardType in standardTypes)
			{
				object[] attributes = standardType.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				CustomSerializedXmlNamespacesAttribute currentAttribute;
				if (attributes != null &&
					attributes.Length != 0 &&
					0 != (currentAttribute = (CustomSerializedXmlNamespacesAttribute)attributes[0]).Count)
				{
					namespaceAttributes.Add(currentAttribute);
				}
			}
			foreach (ExtensionModelBinding extensionType in extensionTypes)
			{
				object[] attributes = extensionType.Type.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				CustomSerializedXmlNamespacesAttribute currentAttribute;
				if (attributes != null &&
					attributes.Length != 0 &&
					0 != (currentAttribute = (CustomSerializedXmlNamespacesAttribute)attributes[0]).Count)
				{
					namespaceAttributes.Add(currentAttribute);
				}
			}

			for (int i = 0, count = namespaceAttributes.Count; i < count; ++i)
			{
				CustomSerializedXmlNamespacesAttribute currentAttribute = namespaceAttributes[i];
				int attributeCount = currentAttribute.Count;
				for (int j = 0; j < attributeCount; ++j)
				{
					namespaceList.Add(currentAttribute[j]);
				}
			}
#pragma warning restore 618

			string[] namespaces = new string[namespaceList.Count];
			namespaceList.CopyTo(namespaces);
			Array.Sort<string>(namespaces);
			if (unrecognizedNamespaces != null)
			{
				for (int i = unrecognizedNamespaces.Count - 1; i >= 0; --i)
				{
					if (Array.BinarySearch<string>(namespaces, unrecognizedNamespaces[i]) >= 0)
					{
						unrecognizedNamespaces.RemoveAt(i);
					}
				}
				if (unrecognizedNamespaces.Count == 0)
				{
					return null;
				}
			}
			argList.AddExtensionObject("urn:schemas-neumont-edu:ORM:ExtensionStripperUtility", new ExtensionStripperUtility(namespaces));
			XslCompiledTransform transform = GetExtensionStripperTransform();

			stream.Position = 0;
			using (XmlReader reader = XmlReader.Create(stream))
			{
				transform.Transform(reader, argList, outputStream);
			}
			outputStream.Position = 0;
			return outputStream;
		}
		#endregion // CleanupStream method
		#endregion // Extension Stripping
	}
	#endregion // ExtensionLoader class
	#region NORMAExtensionLoadException class
	/// <summary>
	/// An exception representing the failure of an extension to load
	/// due to version incompatibility, licensing, or other issues.
	/// </summary>
	public class NORMAExtensionLoadException : Exception
	{
		/// <summary>
		/// Create a new extension load exception.
		/// </summary>
		/// <param name="message">The message to display.</param>
		public NORMAExtensionLoadException(string message)
			: base(message)
		{
		}
	}
	#endregion // NORMAExtensionLoadException class
	#region NORMAExtensionCompatibilityAttribute class
	/// <summary>
	/// Create an assembly attribute to place on NORMA-loaded extension assemblies
	/// to determine if they are compatible with the current NORMA version.
	/// Extensions should call the <see cref="NORMAExtensionCompatibilityAttribute.VerifyCompatibility"/>
	/// method from the class construct of each extension <see cref="DomainModel"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
	public sealed class NORMAExtensionCompatibilityAttribute : Attribute
	{
		#region Fields and Constructor
		private int myMinBuild;
		private int myMaxBuild;
		private int myMinRevision;
		private int myMaxRevision;
		private bool myMatchRevision;
		private bool myMatchBuild;
		/// <summary>
		/// Create a new extension attribute. The type of check
		/// to make is set through the named properties.
		/// </summary>
		public NORMAExtensionCompatibilityAttribute()
		{
			// Everything defaults to off
		}
		#endregion // Fields and Constructor
		#region Helper Methods
		/// <summary>
		/// Call from the class constructor of an extension <see cref="DomainModel"/> to
		/// verify that the extension will work with the current NORMA version.
		/// </summary>
		/// <param name="extensionAssembly">The extension assembly. Can be retrieved with
		/// <see cref="Assembly.GetExecutingAssembly"/> in the calling class constructor.</param>
		/// <exception cref="NORMAExtensionLoadException">A load exception is thrown if the compatibility attributes do not match.</exception>
		public static void VerifyCompatibility(Assembly extensionAssembly)
		{
			object[] attributes;
			if (null != (attributes = extensionAssembly.GetCustomAttributes(typeof(NORMAExtensionCompatibilityAttribute), false)) &&
				attributes.Length != 0)
			{
				bool compatible = true;
				NORMAExtensionCompatibilityAttribute compatAttr = (NORMAExtensionCompatibilityAttribute)attributes[0];
				Version NORMAVersion = new Version(((AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0]).Version);
				attributes = extensionAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
				Version extensionVersion = (attributes != null && attributes.Length != 0) ? new Version(((AssemblyFileVersionAttribute)attributes[0]).Version) : null;
				if (extensionVersion != null &&
					(NORMAVersion.Major != extensionVersion.Major ||
					NORMAVersion.Minor != extensionVersion.Minor))
				{
					compatible = false;
				}
				else
				{
					int NORMAVer = NORMAVersion.Build;
					int minVer = compatAttr.MinBuild;
					int maxVer = compatAttr.MaxBuild;
					if ((minVer != 0 || maxVer != 0) ?
							((minVer != 0 && NORMAVer < minVer) || (maxVer != 0 && NORMAVer > maxVer)) :
							(compatAttr.MatchBuild ? (extensionVersion == null || extensionVersion.Build != NORMAVer) : false))
					{
						compatible = false;
					}
					else
					{
						NORMAVer = NORMAVersion.Revision;
						minVer = compatAttr.MinRevision;
						maxVer = compatAttr.MaxRevision;
						if ((minVer != 0 || maxVer != 0) ?
								((minVer != 0 && NORMAVer < minVer) || (maxVer != 0 && NORMAVer > maxVer)) :
								(compatAttr.MatchRevision ? (extensionVersion == null || extensionVersion.Revision != NORMAVer) : false))
						{
							compatible = false;
						}
					}
				}
				if (!compatible)
				{
					string assemblyName = extensionAssembly.GetName().Name;
					string assemblyDescription = (null != (attributes = extensionAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)) && attributes.Length != 0) ?
						((AssemblyDescriptionAttribute)attributes[0]).Description :
						null;
					if (!string.IsNullOrEmpty(assemblyDescription))
					{
						assemblyName += "\r\n" + assemblyDescription;
					}
					throw new NORMAExtensionLoadException(string.Format(CultureInfo.CurrentCulture, ResourceStrings.LoadExceptionIncompatibleAssembly, assemblyName));
				}
			}
		}
		#endregion // Helper Methods
		#region Base Overrides
		/// <summary>
		/// Default extension compatibility checks for a matching
		/// build number and no other settings.
		/// </summary>
		public override bool IsDefaultAttribute()
		{
			return !myMatchBuild &&
				!myMatchRevision &&
				myMinBuild == 0 &&
				myMaxBuild == 0 &&
				myMinRevision == 0 &&
				myMaxRevision == 0;
		}
		#endregion // Base Overrides
		#region Accessor Properties
		/// <summary>
		/// This extension will fail to load if the build number
		/// does not match the NORMA build number. Ignored if <see cref="MinBuild"/>
		/// or <see cref="MaxBuild"/> are set.
		/// </summary>
		public bool MatchBuild
		{
			get
			{
				return myMatchBuild;
			}
			set
			{
				myMatchBuild = value;
			}
		}
		/// <summary>
		/// This extension will fail to load if the revision number
		/// does not match the NORMA revision number. Ignored if <see cref="MinRevision"/>
		/// or <see cref="MaxRevision"/> are set.
		/// </summary>
		public bool MatchRevision
		{
			get
			{
				return myMatchRevision;
			}
			set
			{
				myMatchRevision = value;
			}
		}
		/// <summary>
		/// This extension will fail to load if the NORMA
		/// build number is not at or after the specified version.
		/// </summary>
		public int MinBuild
		{
			get
			{
				return myMinBuild;
			}
			set
			{
				myMinBuild = value;
			}
		}
		/// <summary>
		/// This extension will fail to load if the NORMA
		/// build number is not before or at the specified version.
		/// </summary>
		public int MaxBuild
		{
			get
			{
				return myMaxBuild;
			}
			set
			{
				myMaxBuild = value;
			}
		}
		/// <summary>
		/// This extension will fail to load if the NORMA
		/// build number is not at or after the specified version.
		/// </summary>
		public int MinRevision
		{
			get
			{
				return myMinRevision;
			}
			set
			{
				myMinRevision = value;
			}
		}
		/// <summary>
		/// This extension will fail to load if the NORMA
		/// build number is not before or at the specified version.
		/// </summary>
		public int MaxRevision
		{
			get
			{
				return myMaxRevision;
			}
			set
			{
				myMaxRevision = value;
			}
		}
		#endregion // Accessor Properties
	}
	#endregion // NORMAExtensionCompatibilityAttribute class
	#region NORMAExtensionLoadKeyAttribute class
	/// <summary>
	/// Create an attribute to place on a NORMA-loaded extension domain model class.
	/// The value of the attribute is a digital signature of the assembly, which is
	/// verified against the currently assembly information.
	/// Extensions should call the <see cref="NORMAExtensionCompatibilityAttribute.VerifyCompatibility"/>
	/// method from the class construct of each extension <see cref="DomainModel"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class NORMAExtensionLoadKeyAttribute : Attribute
	{
		#region Fields and Constructor
		private static RSACryptoServiceProvider myCryptoProvider = null;
		private static SHA1CryptoServiceProvider myHashProvider = null;
		private static Regex myStripNumbersRegex = null;
		private readonly string mySignature;
		/// <summary>
		/// Create a new extension attribute. The type of check
		/// to make is set through the named properties.
		/// </summary>
		public NORMAExtensionLoadKeyAttribute(string signatureString)
		{
			mySignature = signatureString;
		}
		#endregion // Fields and Constructor
		#region Accessor properties
		/// <summary>
		/// Return the encoded signature for this attribute.
		/// </summary>
		public string Signature
		{
			get
			{
				return mySignature;
			}
		}
		#endregion // Accessor properties
		#region Helper Methods
		/// <summary>
		/// Get the crypto provider with the public key for the extension loader hash.
		/// </summary>
		private static RSACryptoServiceProvider CryptoProvider
		{
			get
			{
				RSACryptoServiceProvider provider = myCryptoProvider;
				if (null == provider)
				{
					provider = new RSACryptoServiceProvider(512);
					provider.FromXmlString("<RSAKeyValue><Modulus>nFEPOUeH/FM8nb2LJCxI7w3mrfagnW6hwVD0nBnWIS47n/ZiMk3Rd+SXE0qreQPF3PsIAdd/w5yM+t7XfW1Wjw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					System.Threading.Interlocked.CompareExchange<RSACryptoServiceProvider>(ref myCryptoProvider, provider, null);
					provider = myCryptoProvider;
				}
				return provider;
			}
		}
		/// <summary>
		/// Get the has provider for validating the load signature
		/// </summary>
		private static SHA1CryptoServiceProvider HashProvider
		{
			get
			{
				SHA1CryptoServiceProvider provider = myHashProvider;
				if (null == provider)
				{
					System.Threading.Interlocked.CompareExchange<SHA1CryptoServiceProvider>(ref myHashProvider, new SHA1CryptoServiceProvider(), null);
					provider = myHashProvider;
				}
				return provider;
			}
		}
		/// <summary>
		/// Get the crypto provider with the public key for the extension loader hash.
		/// </summary>
		private static Regex StripNumbersRegex
		{
			get
			{
				Regex regex = myStripNumbersRegex;
				if (null == regex)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(ref myStripNumbersRegex, new Regex(@"\p{Nd}+", RegexOptions.Compiled), null);
					regex = myStripNumbersRegex;
				}
				return regex;
			}
		}
		/// <summary>
		/// Look for a <see cref="NORMAExtensionLoadKeyAttribute"/> on the
		/// domain model type
		/// </summary>
		/// <param name="domainModelType"></param>
		public static void VerifyAssembly(Type domainModelType)
		{
			object[] attributes = domainModelType.GetCustomAttributes(typeof(NORMAExtensionLoadKeyAttribute), false);
			if (attributes != null &&
				attributes.Length == 1)
			{
				AssemblyName assemblyName = new AssemblyName(domainModelType.Assembly.FullName);
				assemblyName.Name = StripNumbersRegex.Replace(assemblyName.Name, "");
				if (CryptoProvider.VerifyData(Encoding.UTF8.GetBytes(assemblyName.FullName), HashProvider, Convert.FromBase64String(((NORMAExtensionLoadKeyAttribute)attributes[0]).Signature)))
				{
					return;
				}
			}
			throw new NORMAExtensionLoadException(string.Format(CultureInfo.CurrentCulture, ResourceStrings.LoadExceptionInvalidLoadKey, domainModelType.FullName));
		}
		#endregion // Helper Methods
		#region Base Overrides
		/// <summary>
		/// The default attribute has no signature.
		/// </summary>
		public override bool IsDefaultAttribute()
		{
			return !string.IsNullOrEmpty(mySignature);
		}
		#endregion // Base Overrides
	}
	#endregion // NORMAExtensionLoadKeyAttribute class
}

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

// Turning this on will result in the ability to debug the extension stripper
// transform and related callback code. A message showing with debugging when
// the transform is first loading, giving you the opportunity to format the
// transform, insert breakpoints, etc. Obviously, this is for debug purposes
// only and should never be turned on.
//#define DEBUG_EXTENSIONSTRIPPER_TRANSFORM
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Win32;
using ORMSolutions.ORMArchitect.Framework.Shell;

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
		None,
		/// <summary>
		/// The extension model loads automatically as part
		/// of another model, should not be displayed to the user.
		/// </summary>
		Secondary,
		/// <summary>
		/// The extension model should always be loaded.
		/// </summary>
		AutoLoad,
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
				object[] namespaceAttributes = type.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				if (namespaceAttributes != null && namespaceAttributes.Length != 0)
				{
					bool foundMatch = false;
					foreach (string testNamespace in (CustomSerializedXmlNamespacesAttribute)namespaceAttributes[0])
					{
						if (testNamespace == namespaceUri)
						{
							foundMatch = true;
							break;
						}
					}
					if (!foundMatch)
					{
						// Bogus request, return and leave IsValidExtension false
						this = default(ExtensionModelBinding);
						return;
					}
				}
			}
			this.myNamespaceUri = namespaceUri;
			this.myType = type;
			object[] extendsAttributes = type.GetCustomAttributes(typeof(ExtendsDomainModelAttribute), false);
			Guid[] extendsIds = new Guid[extendsAttributes.Length];
			for (int i = 0; i < extendsAttributes.Length; ++i)
			{
				extendsIds[i] = ((ExtendsDomainModelAttribute)extendsAttributes[i]).ExtendedModelId;
			}
			myExtendsIds = Array.AsReadOnly(extendsIds);
			object[] domainObjectIdAttributes = type.GetCustomAttributes(typeof(DomainObjectIdAttribute), false);
			myDomainModelId = (domainObjectIdAttributes.Length != 0) ? ((DomainObjectIdAttribute)domainObjectIdAttributes[0]).Id : Guid.Empty;
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
		/// The identifiers for the <see cref="DomainClassInfo"/> models
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
			string[] autoLoadExtensions = null;
			IDictionary<Guid, string> extensionIdToExtensionNameMap = null;
			if (extensionModels != null)
			{
				// Determine all supported assemblies
				int autoLoadCount = 0;
				foreach (ExtensionModelData extensionData in extensionModels)
				{
					Type domainModelType;
					ExtensionModelBinding extensionType;
					string extensionNamespace;
					ExtensionModelOptions options;
					if (null != (domainModelType = extensionData.ResolveExtensionDomainModel()) &&
						(extensionType = new ExtensionModelBinding(
							extensionNamespace = extensionData.NamespaceUri,
							domainModelType,
							options = extensionData.Options)).IsValidExtension)
					{
						availableExtensions[extensionNamespace] = extensionType;
						if (0 != (options & ExtensionModelOptions.AutoLoad))
						{
							++autoLoadCount;
						}
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
			Dictionary<string, Assembly> knownAssemblies = new Dictionary<string, Assembly>(1 + availableExtensions.Count, StringComparer.Ordinal);
			Assembly knownAssembly = typeof(ExtensionLoader).Assembly;
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
			CustomSerializedXmlNamespacesAttribute[] namespaceAttributes = new CustomSerializedXmlNamespacesAttribute[extensionTypes.Count + standardTypes.Count];
			int totalNamespaceCount = 0;
			int serializedAttributeCount = 0;
			foreach (Type standardType in standardTypes)
			{
				object[] attributes = standardType.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				CustomSerializedXmlNamespacesAttribute currentAttribute;
				int currentNamespaceCount;
				if (attributes != null &&
					attributes.Length != 0 &&
					0 != (currentNamespaceCount = (currentAttribute = (CustomSerializedXmlNamespacesAttribute)attributes[0]).Count))
				{
					totalNamespaceCount += currentNamespaceCount;
					namespaceAttributes[serializedAttributeCount] = currentAttribute;
					++serializedAttributeCount;
				}
			}
			foreach (ExtensionModelBinding extensionType in extensionTypes)
			{
				object[] attributes = extensionType.Type.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				CustomSerializedXmlNamespacesAttribute currentAttribute;
				int currentNamespaceCount;
				if (attributes != null &&
					attributes.Length != 0 &&
					0 != (currentNamespaceCount = (currentAttribute = (CustomSerializedXmlNamespacesAttribute)attributes[0]).Count))
				{
					totalNamespaceCount += currentNamespaceCount;
					namespaceAttributes[serializedAttributeCount] = currentAttribute;
					++serializedAttributeCount;
				}
			}
			string[] namespaces = new string[totalNamespaceCount];
			int namespaceIndex = -1;
			for (int i = 0; i < serializedAttributeCount; ++i)
			{
				CustomSerializedXmlNamespacesAttribute currentAttribute = namespaceAttributes[i];
				int attributeCount = currentAttribute.Count;
				for (int j = 0; j < attributeCount; ++j)
				{
					namespaces[++namespaceIndex] = currentAttribute[j];
				}
			}
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
}

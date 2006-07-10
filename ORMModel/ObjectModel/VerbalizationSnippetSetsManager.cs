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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.ORM.Design;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMVerbalizationSnippetsData struct
	/// <summary>
	/// Data returned for each set of verbalization snippets supported.
	/// </summary>
	public struct VerbalizationSnippetsData : IEquatable<VerbalizationSnippetsData>
	{
		#region Member Variables
		private readonly Type myEnumType;
		private readonly IVerbalizationSets myDefaultVerbalizationSets;
		private readonly string myAlternateSnippetsDirectory;
		private readonly string myTypeDescription;
		private readonly string myDefaultSetsDescription;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a new VerbalizationSnippetsData struct.
		/// </summary>
		/// <param name="enumType">
		/// The type of the enumeration used to identify and extra
		/// elements from this collection.
		/// </param>
		/// <param name="defaultVerbalizationSets">
		/// The default verbalization sets. Compiled-in verbalization
		/// sets are required and are always used as a backup if a specified
		/// base implementation does not bind correctly. The default
		/// verbalization sets are assumed to have a language of en-US.
		/// The defaultVerbalizationSets implementation must also support
		/// IVerbalizationSets&lt;enumType&gt;
		/// </param>
		/// <param name="alternateSnippetsDirectory">
		/// The name of a directory where alternate verbalization sets
		/// for this type can be loaded. This directory should be placed
		/// directly under the VerbalizationSnippets directory in the
		/// install location. All .xml files in the directory are loaded.
		/// </param>
		/// <param name="typeDescription">
		/// A description for this group of verbalization snippets
		/// </param>
		/// <param name="defaultSetsDescription">
		/// A description of the default verbalization set
		/// </param>
		public VerbalizationSnippetsData(
			Type enumType,
			IVerbalizationSets defaultVerbalizationSets,
			string alternateSnippetsDirectory,
			string typeDescription,
			string defaultSetsDescription)
		{
			string nullParamName = null;
			if (enumType == null)
			{
				nullParamName = "enumType";
			}
			else if (!typeof(Enum).IsAssignableFrom(enumType))
			{
				throw new ArgumentException("enumType");
			}
			else if (defaultVerbalizationSets == null)
			{
				nullParamName = "defaultVerbalizationSets";
			}
			else
			{
				Type[] supportedInterfaces = defaultVerbalizationSets.GetType().GetInterfaces();
				int interfacesCount = supportedInterfaces.Length;
				int i = 0;
				for (; i < interfacesCount; ++i)
				{
					Type currentInterface = supportedInterfaces[i];
					if (currentInterface.IsGenericType &&
						currentInterface.GetGenericTypeDefinition() == typeof(IVerbalizationSets<>))
					{
						Type[] typeArgs = currentInterface.GetGenericArguments();
						if (typeArgs != null &&
							typeArgs.Length == 1 &&
							typeArgs[0] == enumType)
						{
							break;
						}
					}
				}
				if (i == interfacesCount)
				{
					throw new ArgumentException("defaultVerbalizationSets");
				}
				if (alternateSnippetsDirectory == null)
				{
					nullParamName = "alternateSnippetsDirectory";
				}
				else if (typeDescription == null)
				{
					nullParamName = "typeDescription";
				}
				else if (defaultSetsDescription == null)
				{
					nullParamName = "defaultSetsDescription";
				}
			}
			if (nullParamName != null)
			{
				throw new ArgumentNullException(nullParamName);
			}
			myEnumType = enumType;
			myDefaultVerbalizationSets = defaultVerbalizationSets;
			myAlternateSnippetsDirectory = alternateSnippetsDirectory;
			myTypeDescription = typeDescription;
			myDefaultSetsDescription = defaultSetsDescription;
		}
		#endregion // Constructors
		#region Accessor Properties
		/// <summary>
		/// Return true if the structure is not initialized
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myEnumType == null;
			}
		}
		/// <summary>
		/// The type of the enumeration used to identify and extra
		/// elements from this collection.
		/// </summary>
		public Type EnumType
		{
			get
			{
				return myEnumType;
			}
		}
		/// <summary>
		/// The default verbalization sets. Compiled-in verbalization
		/// sets are required and are always used as a backup if a specified
		/// base implementation does not bind correctly. The default
		/// verbalization sets are assumed to have a language of en-US.
		/// </summary>
		public IVerbalizationSets DefaultVerbalizationSets
		{
			get
			{
				return myDefaultVerbalizationSets;
			}
		}
		/// <summary>
		/// The name of a directory where alternate verbalization sets
		/// for this type can be loaded. This directory should be placed
		/// directly under the VerbalizationSnippets directory in the
		/// install location. All .xml files in the directory are loaded.
		/// </summary>
		public string AlternateSnippetsDirectory
		{
			get
			{
				return myAlternateSnippetsDirectory;
			}
		}
		/// <summary>
		/// A description for this group of verbalization snippets
		/// </summary>
		public string TypeDescription
		{
			get
			{
				return myTypeDescription;
			}
		}
		/// <summary>
		/// A description of the default verbalization set
		/// </summary>
		public string DefaultSetsDescription
		{
			get
			{
				return myDefaultSetsDescription;
			}
		}
		#endregion // Accessor Properties
		#region Equality overrides
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(VerbalizationSnippetsData data1, VerbalizationSnippetsData data2)
		{
			return data1.Equals(data2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(VerbalizationSnippetsData data1, VerbalizationSnippetsData data2)
		{
			return !(data1.Equals(data2));
		}
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is VerbalizationSnippetsData) ? Equals((VerbalizationSnippetsData)obj) : false;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(VerbalizationSnippetsData obj)
		{
			bool leftEmpty = myDefaultVerbalizationSets == null;
			bool rightEmpty = obj.myDefaultVerbalizationSets == null;
			if (leftEmpty && rightEmpty)
			{
				return true;
			}
			else if (!leftEmpty && !rightEmpty)
			{
				// Note that myDescription is intentionally ignored
				return myEnumType == obj.myEnumType &&
					myDefaultVerbalizationSets == obj.myDefaultVerbalizationSets &&
					myAlternateSnippetsDirectory == obj.myAlternateSnippetsDirectory &&
					myTypeDescription == obj.myTypeDescription &&
					myDefaultSetsDescription == obj.myDefaultSetsDescription;
			}
			return false;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			// Note that myDescription is intentionally ignored
			return (myEnumType == null) ?
				0 :
				myEnumType.GetHashCode() ^ RotateRight(myDefaultVerbalizationSets.GetHashCode(), 1) ^ RotateRight(myAlternateSnippetsDirectory.GetHashCode(), 2) ^ RotateRight(myDefaultSetsDescription.GetHashCode(), 3) ^ RotateRight(myTypeDescription.GetHashCode(), 4);
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
		#endregion // Equality overrides
	}
	#endregion // VerbalizationSnippetsData struct
	#region IVerbalizationSnippetsProvider interface
	/// <summary>
	/// The IVerbalizationSnippetsProvider interfaces enables
	/// a model to provide verbalization snippets to the ORM
	/// verbalization engine. Snippets are identified by an
	/// enum type used to retrieve snippets.
	/// </summary>
	public interface IVerbalizationSnippetsProvider
	{
		/// <summary>
		/// Return an array of supported verbalization snippets sets
		/// </summary>
		/// <returns>VerbalizationSnippetsData array</returns>
		VerbalizationSnippetsData[] ProvideVerbalizationSnippets();
	}
	#endregion // IVerbalizationSnippetsProvider interface
	#region VerbalizationSnippetsIdentifier struct
	/// <summary>
	/// A unique identifier for verbalization snippets
	/// </summary>
	public struct VerbalizationSnippetsIdentifier
	{
		#region Member Variables
		private string myEnumTypeName;
		private string myLangId;
		private string myId;
		private string myDescription;
		private const string DefaultLanguageId = "en-US";
		private const string DefaultId = "_default";
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a snippets identifier
		/// </summary>
		/// <param name="enumType">An enum describing the snippet types</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		public VerbalizationSnippetsIdentifier(Type enumType, string languageId, string id)
			: this(enumType, languageId, id, null)
		{
		}
		/// <summary>
		/// Create a snippets identifier with a description
		/// </summary>
		/// <param name="enumType">An enum describing the snippet types</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		/// <param name="description">A displayable description for this identifier</param>
		public VerbalizationSnippetsIdentifier(Type enumType, string languageId, string id, string description)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (languageId == null)
			{
				throw new ArgumentNullException("languageId");
			}
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			// description is optional
			myEnumTypeName = enumType.FullName;
			myLangId = languageId;
			myId = id;
			myDescription = description;
		}
		/// <summary>
		/// Create a snippets identifier from a type name
		/// </summary>
		/// <param name="enumTypeName">The namespace-qualified name of an enum representing the snippets</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		public VerbalizationSnippetsIdentifier(string enumTypeName, string languageId, string id)
			: this(enumTypeName, languageId, id, null)
		{
		}
		/// <summary>
		/// Create a snippets identifier with a description from a type name
		/// </summary>
		/// <param name="enumTypeName">The namespace-qualified name of an enum representing the snippets</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		/// <param name="description">A displayable description for this identifier</param>
		public VerbalizationSnippetsIdentifier(string enumTypeName, string languageId, string id, string description)
		{
			if (enumTypeName == null)
			{
				throw new ArgumentNullException("enumTypeName");
			}
			if (languageId == null)
			{
				throw new ArgumentNullException("languageId");
			}
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			myEnumTypeName = enumTypeName;
			myLangId = languageId;
			myId = id;
			myDescription = null;
		}
		#endregion // Constructors
		#region Accessor Properties
		/// <summary>
		/// Returns true if the id structure has not been initialized
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myEnumTypeName == null;
			}
		}
		/// <summary>
		/// Return true if this is a default identifier for the enum type.
		/// Default identifiers are created with the static CreateDefaultIdentifier method.
		/// </summary>
		public bool IsDefaultIdentifier
		{
			get
			{
				return !IsEmpty && myLangId == DefaultLanguageId && myId == DefaultId;
			}
		}
		/// <summary>
		/// Create a default identifier for the specified enum type
		/// </summary>
		/// <param name="enumType">A type representing an enum</param>
		/// <param name="description">An optional description for the identifier. Can be null</param>
		/// <returns>VerbalizationSnippetsIdentifier with default language and id</returns>
		public static VerbalizationSnippetsIdentifier CreateDefaultIdentifier(Type enumType, string description)
		{
			return new VerbalizationSnippetsIdentifier(enumType, DefaultLanguageId, DefaultId, description);
		}
		/// <summary>
		/// Create a default identifier for the specified enum type name
		/// </summary>
		/// <param name="enumTypeName">The full name of an enum type</param>
		/// <param name="description">An optional description for the identifier. Can be null</param>
		/// <returns>VerbalizationSnippetsIdentifier with default language and id</returns>
		public static VerbalizationSnippetsIdentifier CreateDefaultIdentifier(string enumTypeName, string description)
		{
			return new VerbalizationSnippetsIdentifier(enumTypeName, DefaultLanguageId, DefaultId, description);
		}
		/// <summary>
		/// The qualified name of the enum type representing the verbalization snippets
		/// </summary>
		public string EnumTypeName
		{
			get
			{
				return myEnumTypeName;
			}
		}
		/// <summary>
		/// The IETF language identifier for this set of snippets
		/// </summary>
		public string LanguageId
		{
			get
			{
				return myLangId;
			}
		}
		/// <summary>
		/// A unique name identifying these snippets in this language
		/// </summary>
		public string Id
		{
			get
			{
				return myId;
			}
		}
		/// <summary>
		/// A displayable description for this set of snippets
		/// </summary>
		public string Description
		{
			get
			{
				return myDescription;
			}
		}
		#endregion // Accessor Properties
		#region Utility Functions
		/// <summary>
		/// Parses a string returned by SaveIdentifiers back into
		/// a set of identifiers. Only non-default identifiers are
		/// returned.
		/// </summary>
		/// <param name="identifiers">A string returned by SaveIdentifiers</param>
		/// <returns>VerbalizationSnippetsIdentifier array, or null if no custom identifiers specified</returns>
		public static VerbalizationSnippetsIdentifier[] ParseIdentifiers(string identifiers)
		{
			VerbalizationSnippetsIdentifier[] retVal = null;
			if (identifiers != null && identifiers.Length != 0)
			{
				string[] ids = identifiers.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
				int idsCount;
				if (ids != null &&
					0 != (idsCount = ids.Length))
				{
					retVal = new VerbalizationSnippetsIdentifier[idsCount];
					char[] separators = new char[] { ' ', ',' };
					for (int i = 0; i < idsCount; ++i)
					{
						string[] fields = ids[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
						Debug.Assert(fields.Length == 3, "The string passed to ParseIdentifiers must be saved with SaveIdentifiers");
						retVal[i] = new VerbalizationSnippetsIdentifier(fields[0], fields[1], fields[2]);
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Save identifiers as a string
		/// </summary>
		/// <param name="identifiers">Type IEnumerable of identifiers</param>
		/// <returns>Identifiers referenced as a string. Suitable for use with ParseIdentifiers.</returns>
		public static string SaveIdentifiers(IEnumerable<VerbalizationSnippetsIdentifier> identifiers)
		{
			StringBuilder sb = null;
			foreach (VerbalizationSnippetsIdentifier identifier in identifiers)
			{
				if (!identifier.IsEmpty)
				{
					if (!identifier.IsDefaultIdentifier)
					{
						if (sb == null)
						{
							sb = new StringBuilder();
						}
						else
						{
							sb.Append(';');
						}
						sb.Append(identifier.EnumTypeName);
						sb.Append(',');
						sb.Append(identifier.LanguageId);
						sb.Append(',');
						sb.Append(identifier.Id);
					}
				}
			}
			return (sb != null) ? sb.ToString() : "";
		}
		#endregion // Utility Functions
		#region Equality overrides
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(VerbalizationSnippetsIdentifier snippets1, VerbalizationSnippetsIdentifier snippets2)
		{
			return snippets1.Equals(snippets2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(VerbalizationSnippetsIdentifier snippets1, VerbalizationSnippetsIdentifier snippets2)
		{
			return !(snippets1.Equals(snippets2));
		}
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is VerbalizationSnippetsIdentifier) ? Equals((VerbalizationSnippetsIdentifier)obj) : false;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(VerbalizationSnippetsIdentifier obj)
		{
			bool leftEmpty = myEnumTypeName == null;
			bool rightEmpty = obj.myEnumTypeName == null;
			if (leftEmpty && rightEmpty)
			{
				return true;
			}
			else if (!leftEmpty && !rightEmpty)
			{
				// Note that myDescription is intentionally ignored
				return myEnumTypeName == obj.EnumTypeName &&
					myLangId == obj.myLangId &&
					myId == obj.myId;
			}
			return false;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			// Note that myDescription is intentionally ignored
			return (myEnumTypeName == null) ?
				0 :
				myEnumTypeName.GetHashCode() ^ RotateRight(myLangId.GetHashCode(), 1) ^ RotateRight(myId.GetHashCode(), 2);
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
		#endregion // Equality overrides
	}
	#endregion // VerbalizationSnippetsIdentifier struct
	#region VerbalizationSnippetSetsManager class
	/// <summary>
	/// Class for managing and loading snippet sets
	/// </summary>
	public class VerbalizationSnippetSetsManager
	{
		#region Member Variables
		private static readonly object LockObject = new object();
		#endregion // Member Variables
		#region Snippet Sets Schema definition classes
		#region VerbalizationSnippetSets class
		private static class VerbalizationSnippetSets
		{
			#region String Constants
			public const string SchemaNamespace = "http://schemas.neumont.edu/ORM/SDK/Verbalization";
			public const string LanguagesElement = "Languages";
			public const string LanguageElement = "Language";
			public const string SnippetsElement = "Snippets";
			public const string SnippetElement = "Snippet";

			public const string TypeAttribute = "type";
			public const string ModalityAttribute = "modality";
			public const string SignAttribute = "sign";
			public const string LangAttribute = "xml:lang";
			public const string NameAttribute = "name";
			public const string DescriptionAttribute = "description";
			public const string BaseSnippetsNameAttribute = "baseSnippetsName";
			public const string BaseSnippetsLanguageAttribute = "baseSnippetsLanguage";
			#endregion // String Constants
			#region Static properties
			private static VerbalizationSnippetSetsNameTable myNames;
			public static VerbalizationSnippetSetsNameTable Names
			{
				get
				{
					VerbalizationSnippetSetsNameTable retVal = myNames;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myNames;
							if (retVal == null)
							{
								retVal = myNames = new VerbalizationSnippetSetsNameTable();
							}
						}
					}
					return retVal;
				}
			}
			private static XmlReaderSettings myReaderSettings;
			public static XmlReaderSettings ReaderSettings
			{
				get
				{
					XmlReaderSettings retVal = myReaderSettings;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myReaderSettings;
							if (retVal == null)
							{
								retVal = myReaderSettings = new XmlReaderSettings();
								retVal.ValidationType = ValidationType.Schema;
								retVal.Schemas.Add(SchemaNamespace, new XmlTextReader(typeof(VerbalizationSnippetSetsManager).Assembly.GetManifestResourceStream(typeof(VerbalizationSnippetSetsManager), "VerbalizationUntypedSnippets.xsd")));
								retVal.NameTable = Names;
							}
						}
					}
					return retVal;
				}
			}
			#endregion // Static properties
		}
		#endregion // VerbalizationSnippetSets class
		#region VerbalizationSnippetSetsNameTable class
		private sealed class VerbalizationSnippetSetsNameTable : NameTable
		{
			public readonly string SchemaNamespace;
			public readonly string LanguagesElement;
			public readonly string LanguageElement;
			public readonly string SnippetsElement;
			public readonly string SnippetElement;

			public readonly string TypeAttribute;
			public readonly string ModalityAttribute;
			public readonly string SignAttribute;
			public readonly string LangAttribute;
			public readonly string NameAttribute;
			public readonly string DescriptionAttribute;
			public readonly string BaseSnippetsNameAttribute;
			public readonly string BaseSnippetsLanguageAttribute;

			public VerbalizationSnippetSetsNameTable()
				: base()
			{
				SchemaNamespace = Add(VerbalizationSnippetSets.SchemaNamespace);
				LanguagesElement = Add(VerbalizationSnippetSets.LanguagesElement);
				LanguageElement = Add(VerbalizationSnippetSets.LanguageElement);
				SnippetsElement = Add(VerbalizationSnippetSets.SnippetsElement);
				SnippetElement = Add(VerbalizationSnippetSets.SnippetElement);

				TypeAttribute = Add(VerbalizationSnippetSets.TypeAttribute);
				ModalityAttribute = Add(VerbalizationSnippetSets.ModalityAttribute);
				SignAttribute = Add(VerbalizationSnippetSets.SignAttribute);
				LangAttribute = Add(VerbalizationSnippetSets.LangAttribute);
				NameAttribute = Add(VerbalizationSnippetSets.NameAttribute);
				DescriptionAttribute = Add(VerbalizationSnippetSets.DescriptionAttribute);
				BaseSnippetsNameAttribute = Add(VerbalizationSnippetSets.BaseSnippetsNameAttribute);
				BaseSnippetsLanguageAttribute = Add(VerbalizationSnippetSets.BaseSnippetsLanguageAttribute);
			}
		}
		#endregion // VerbalizationSnippetSetsNameTable class
		#endregion // Snippet Sets Schema definition classes
		#region Directory and file loading methods
		/// <summary>
		/// Load all verbalization snippets provided by substores in the provided store
		/// </summary>
		/// <param name="store">The store to load</param>
		/// <param name="customSnippetsDirectory">The base directory to search for additional snippets</param>
		/// <param name="customIdentifiers">An array of preferred custom identifiers
		/// for the preferred verbalization sets. Can be null if no customizations are in place.</param>
		/// <returns>Snippets dictionary</returns>
		public static IDictionary<Type, IVerbalizationSets> LoadSnippetsDictionary(Store store, string customSnippetsDirectory, VerbalizationSnippetsIdentifier[] customIdentifiers)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			ICollection<DomainModel> domainModels = store.DomainModels;
			List<IVerbalizationSnippetsProvider> snippetProviders = new List<IVerbalizationSnippetsProvider>(domainModels.Count);
			foreach (DomainModel domainModel in domainModels)
			{
				IVerbalizationSnippetsProvider provider = domainModel as IVerbalizationSnippetsProvider;
				if (provider != null)
				{
					snippetProviders.Add(provider);
				}
			}
			return LoadSnippetsDictionary(snippetProviders, customSnippetsDirectory, customIdentifiers);
		}
		/// <summary>
		/// Load the descriptions and names of all verbalization snippets provided
		/// </summary>
		/// <param name="providers">The snippet providers</param>
		/// <param name="customSnippetsDirectory">The base directory to search for additional snippets</param>
		/// <returns>An array of available snippets identifiers</returns>
		public static VerbalizationSnippetsIdentifier[] LoadAvailableSnippets(IEnumerable<IVerbalizationSnippetsProvider> providers, string customSnippetsDirectory)
		{
			Type[] typeArgs = new Type[1];
			Dictionary<VerbalizationSnippetsIdentifier, IVerbalizationSets> allSets = null;
			foreach (IVerbalizationSnippetsProvider provideSnippets in providers)
			{
				VerbalizationSnippetsData[] snippetsData;
				int snippetsDataCount;
				if (null != (snippetsData = provideSnippets.ProvideVerbalizationSnippets()) &&
					0 != (snippetsDataCount = snippetsData.Length))
				{
					for (int i = 0; i < snippetsDataCount; ++i)
					{
						VerbalizationSnippetsData data = snippetsData[i];
						if (!data.IsEmpty)
						{
							Type enumType = data.EnumType;
							string enumTypeName = enumType.FullName;
							typeArgs[0] = enumType;
							if (allSets == null)
							{
								allSets = new Dictionary<VerbalizationSnippetsIdentifier, IVerbalizationSets>();
							}
							VerbalizationSnippetsIdentifier defaultSnippetsIdentifier = VerbalizationSnippetsIdentifier.CreateDefaultIdentifier(enumType, data.DefaultSetsDescription);
							allSets.Add(defaultSnippetsIdentifier, data.DefaultVerbalizationSets);
							// Call type-bound method through reflection
							typeof(VerbalizationSnippetSetsManager).GetMethod("LoadVerbalizationFiles", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(typeArgs).Invoke(
								null,
								BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static,
								null,
								new object[] { Path.Combine(customSnippetsDirectory ?? string.Empty, data.AlternateSnippetsDirectory), allSets, defaultSnippetsIdentifier, true },
								null);
						}
					}
				}
			}
			VerbalizationSnippetsIdentifier[] retVal;
			if (allSets == null)
			{
				retVal = new VerbalizationSnippetsIdentifier[0];
			}
			else
			{
				ICollection<VerbalizationSnippetsIdentifier> keys = allSets.Keys;
				retVal = new VerbalizationSnippetsIdentifier[keys.Count];
				keys.CopyTo(retVal, 0);
			}
			return retVal;
		}
		
		/// <summary>
		/// Load all verbalization snippets provided by substores in the provided store
		/// </summary>
		/// <param name="providers">The snippet providers</param>
		/// <param name="customSnippetsDirectory">The base directory to search for additional snippets</param>
		/// <param name="customIdentifiers">An array of preferred custom identifiers
		/// for the preferred verbalization sets. Can be null if no customizations are in place.</param>
		/// <returns>Snippets dictionary</returns>
		public static IDictionary<Type, IVerbalizationSets> LoadSnippetsDictionary(IEnumerable<IVerbalizationSnippetsProvider> providers, string customSnippetsDirectory, VerbalizationSnippetsIdentifier[] customIdentifiers)
		{
			if (providers == null)
			{
				throw new ArgumentNullException("providers");
			}
			Dictionary<Type, IVerbalizationSets> retVal = new Dictionary<Type, IVerbalizationSets>();
			Type[] typeArgs = new Type[1];
			int customIdentifiersCount = (customIdentifiers != null) ? customIdentifiers.Length : 0;
			Dictionary<VerbalizationSnippetsIdentifier, IVerbalizationSets> allSets = null;
			foreach (IVerbalizationSnippetsProvider provideSnippets in providers)
			{
				VerbalizationSnippetsData[] snippetsData;
				int snippetsDataCount;
				if (null != (snippetsData = provideSnippets.ProvideVerbalizationSnippets()) &&
					0 != (snippetsDataCount = snippetsData.Length))
				{
					for (int i = 0; i < snippetsDataCount; ++i)
					{
						VerbalizationSnippetsData data = snippetsData[i];
						if (!data.IsEmpty)
						{
							Type enumType = data.EnumType;
							string enumTypeName = enumType.FullName;
							int customIdentifierIndex = -1;
							for (int j = 0; j < customIdentifiersCount; ++j)
							{
								VerbalizationSnippetsIdentifier testIdentifier = customIdentifiers[j];
								if (testIdentifier.EnumTypeName == enumTypeName && !testIdentifier.IsDefaultIdentifier)
								{
									customIdentifierIndex = j;
									break;
								}
							}
							if (customIdentifierIndex != -1)
							{
								typeArgs[0] = enumType;
								if (allSets == null)
								{
									allSets = new Dictionary<VerbalizationSnippetsIdentifier, IVerbalizationSets>();
								}
								VerbalizationSnippetsIdentifier defaultSnippetsIdentifier = VerbalizationSnippetsIdentifier.CreateDefaultIdentifier(enumType, data.DefaultSetsDescription);
								allSets.Add(defaultSnippetsIdentifier, data.DefaultVerbalizationSets);
								// Call type-bound method through reflection
								typeof(VerbalizationSnippetSetsManager).GetMethod("LoadVerbalizationFiles", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(typeArgs).Invoke(
									null,
									BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static,
									null,
									new object[] { Path.Combine(customSnippetsDirectory, data.AlternateSnippetsDirectory), allSets, defaultSnippetsIdentifier, false },
									null);
								IVerbalizationSets useVerbalization;
								if (!allSets.TryGetValue(customIdentifiers[customIdentifierIndex], out useVerbalization))
								{
									useVerbalization = data.DefaultVerbalizationSets;
								}
								retVal.Add(enumType, useVerbalization);
							}
							else
							{
								retVal.Add(enumType, data.DefaultVerbalizationSets);
							}
						}
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Load all xml files in the provided directory
		/// </summary>
		/// <typeparam name="TEnum">The enum type of the verbalization snippets</typeparam>
		/// <param name="directoryPath">The directory to load</param>
		/// <param name="verbalizationSets">The dictionary to store new snippets and retrieve existing snippets. The
		/// dictionary will contain the default verbalization set for the given TEnum, which is always provided
		/// by the snippets provider.</param>
		/// <param name="defaultSnippetsIdentifier">The default snippets identifier for items of this type</param>
		/// <param name="identifiersOnly">The populated dictionary will have keys only, skip verbalizationset population.</param>
		private static void LoadVerbalizationFiles<TEnum>(string directoryPath, IDictionary<VerbalizationSnippetsIdentifier, IVerbalizationSets> verbalizationSets, VerbalizationSnippetsIdentifier defaultSnippetsIdentifier, bool identifiersOnly) where TEnum : struct
		{
			if (Directory.Exists(directoryPath))
			{
				string[] files = Directory.GetFiles(directoryPath, "*.xml");
				int fileCount = files.Length;
				List<RawSnippets<TEnum>> rawSnippetsList = null;
				for (int i = 0; i < fileCount; ++i)
				{
					string currentFile = files[i];
					if (!currentFile.EndsWith(@"\_default.xml", StringComparison.OrdinalIgnoreCase))
					{
						using (FileStream stream = new FileStream(currentFile, FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							LoadSnippets<TEnum>(stream, identifiersOnly, ref rawSnippetsList);
						}
					}
				}
				if (rawSnippetsList != null)
				{
					int snippetsCount = rawSnippetsList.Count;
					for (int i = 0; i < snippetsCount; ++i)
					{
						rawSnippetsList[i].Process(verbalizationSets, rawSnippetsList, defaultSnippetsIdentifier, identifiersOnly);
					}
				}
			}
		}
		#endregion // Directory and file loading methods
		#region XML processing structs
		private struct RawSnippet<TEnum> where TEnum : struct
		{
			public RawSnippet(TEnum id, string value, ConstraintModality? modality, bool? sign)
			{
				Id = id;
				Value = value;
				Modality = modality;
				Sign = sign;
			}
			public TEnum Id;
			public string Value;
			public bool? Sign;
			public ConstraintModality? Modality;
		}
		private struct RawSnippets<TEnum> where TEnum : struct
		{
			#region Member Variables
			public VerbalizationSnippetsIdentifier Id;
			public VerbalizationSnippetsIdentifier BaseId;
			public List<RawSnippet<TEnum>> Snippets;
			#endregion // Member Variables
			#region Helper Functions
			public bool IsEmpty
			{
				get
				{
					return Id.IsEmpty || Snippets == null;
				}
			}
			#endregion // Helper Functions
			#region VerbalizationSets implementation class
			private sealed class PassthroughVerbalizationSets : VerbalizationSets<TEnum>, IVerbalizationSets<TEnum>
			{
				private IVerbalizationSets<TEnum> myDeferTo;
				public PassthroughVerbalizationSets(IVerbalizationSets<TEnum> deferTo)
				{
					myDeferTo = deferTo;
				}
				string IVerbalizationSets<TEnum>.GetSnippet(TEnum snippetType, bool isDeontic, bool isNegative)
				{
					string retVal = base.GetSnippet(snippetType, isDeontic, isNegative);
					return (retVal == null) ? myDeferTo.GetSnippet(snippetType, isDeontic, isNegative) : retVal;
				}
				string IVerbalizationSets<TEnum>.GetSnippet(TEnum snippetType)
				{
					string retVal = base.GetSnippet(snippetType);
					return (retVal == null) ? myDeferTo.GetSnippet(snippetType) : retVal;
				}
				protected override void PopulateVerbalizationSets(VerbalizationSet[] sets, object userData)
				{
					List<RawSnippet<TEnum>> snippetsList = (List<RawSnippet<TEnum>>)userData;
					IVerbalizationSets<TEnum> baseSnippets = myDeferTo;

					int snippetsCount = snippetsList.Count;
					Debug.Assert(snippetsCount > 0); // Loader won't create the set otherwise

					// Sort the snippets by enum value, number of specified properties (modality, sign),
					// then by modality, sign so they're nicely grouped for the loop below
					#region Sort Snippets
					snippetsList.Sort(
						delegate(RawSnippet<TEnum> snippet1, RawSnippet<TEnum> snippet2)
						{
							int compareResult = 0;

							// Sort first by type
							int type1 = ValueToIndex(snippet1.Id);
							int type2 = ValueToIndex(snippet1.Id);
							if (type1 < type2)
							{
								compareResult = -1;
							}
							else if (type1 > type2)
							{
								compareResult = 1;
							}
							else
							{
								ConstraintModality? modality1 = snippet1.Modality;
								ConstraintModality? modality2 = snippet2.Modality;
								bool? sign1 = snippet1.Sign;
								bool? sign2 = snippet2.Sign;

								int totalExplicit1 = (modality1.HasValue ? 1 : 0) + (sign1.HasValue ? 1 : 0);
								int totalExplicit2 = (modality2.HasValue ? 1 : 0) + (sign2.HasValue ? 1 : 0);

								if (totalExplicit1 > totalExplicit2)
								{
									compareResult = -1;
								}
								else if (totalExplicit1 < totalExplicit2)
								{
									compareResult = 1;
								}
								else if (totalExplicit1 < 2)
								{
									// Then sort by modality (alethic/deontic/null)
									if (modality1.HasValue)
									{
										if (modality2.HasValue)
										{
											if (modality1.Value != modality2.Value)
											{
												return (modality1.Value == ConstraintModality.Alethic) ? -1 : 1;
											}
										}
										else
										{
											compareResult = -1;
										}
									}
									else if (modality2.HasValue)
									{
										compareResult = 1;
									}
									else
									{
										// Then sort by sign (positive/negative/null)
										if (sign1.HasValue)
										{
											if (sign2.HasValue)
											{
												if (sign1.Value != sign2.Value)
												{
													return sign1.Value ? -1 : 1;
												}
											}
											else
											{
												compareResult = -1;
											}
										}
										else if (sign2.HasValue)
										{
											compareResult = 1;
										}
									}
								}
							}
							return compareResult;
						});
					#endregion //Sort Snippets

					for (int i = 0; i < snippetsCount; ++i)
					{
						RawSnippet<TEnum> snippet = snippetsList[i];
						TEnum type = snippet.Id;
						ConstraintModality? modality = snippet.Modality;
						bool? sign = snippet.Sign;
						string value = snippet.Value;
						if (modality.HasValue)
						{
							bool isDeontic = modality == ConstraintModality.Deontic;
							if (sign.HasValue)
							{
								SetValue(sets, type, isDeontic, !sign.Value, value);
							}
							else
							{
								SetValue(sets, type, isDeontic, true, value);
								SetValue(sets, type, isDeontic, false, value);
							}
						}
						else if (sign.HasValue)
						{
							bool isNegative = !sign.Value;
							SetValue(sets, type, true, isNegative, value);
							SetValue(sets, type, false, isNegative, value);
						}
						else
						{
							SetValue(sets, type, true, true, value);
							SetValue(sets, type, true, false, value);
							SetValue(sets, type, false, true, value);
							SetValue(sets, type, false, false, value);
						}
					}
				}
				private static void SetValue(VerbalizationSet[] sets, TEnum key, bool isDeontic, bool isNegative, string value)
				{
					int setIndex = GetSetIndex(isDeontic, isNegative);
					DictionaryVerbalizationSet set = sets[setIndex] as DictionaryVerbalizationSet;
					if (set == null)
					{
						set = new DictionaryVerbalizationSet();
						sets[setIndex] = set;
					}
					IDictionary<TEnum, string> dictionary = set.Dictionary;
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, value);
					}
				}
				/// <summary>
				/// Convert the enum value to an index
				/// </summary>
				protected override int ValueToIndex(TEnum enumValue)
				{
					return (int)(object)enumValue;
				}
			}
			#endregion // VerbalizationSets implementation class
			#region Snippet processing
			/// <summary>
			/// A placeholder empty implementaiton of IVerbalizationSets&lt;TEnum&gt;
			/// to enable cycle detection when we're loading identifiers only
			/// </summary>
			private sealed class EmptyVerbalizationSets : IVerbalizationSets<TEnum>
			{
				#region IVerbalizationSets<TEnum> Implementation
				string IVerbalizationSets<TEnum>.GetSnippet(TEnum snippetType, bool isDeontic, bool isNegative)
				{
					return null;
				}
				string IVerbalizationSets<TEnum>.GetSnippet(TEnum snippetType)
				{
					return null;
				}
				#endregion // IVerbalizationSets<TEnum> Implementation
			}
			public IVerbalizationSets<TEnum> Process(IDictionary<VerbalizationSnippetsIdentifier, IVerbalizationSets> processedSets, List<RawSnippets<TEnum>> rawSnippetsList, VerbalizationSnippetsIdentifier defaultSnippetsIdentifier, bool identifiersOnly)
			{
				VerbalizationSnippetsIdentifier currentId = this.Id;
				if (processedSets.ContainsKey(currentId))
				{
					return (IVerbalizationSets<TEnum>)processedSets[currentId];
				}
				IVerbalizationSets<TEnum> retVal = null;

				// Add the set by this identifier with a null value
				// to stop any potential cycle. Any request to use
				// this set as a base before completion will revert
				// back to the null base.
				bool processingComplete = false;
				processedSets.Add(currentId, null);
				try
				{
					IVerbalizationSets<TEnum> baseSnippets = null;
					VerbalizationSnippetsIdentifier currentBaseId = this.BaseId;
					if (processedSets.ContainsKey(currentBaseId))
					{
						baseSnippets = (IVerbalizationSets<TEnum>)processedSets[currentBaseId];
						if (baseSnippets == null)
						{
							// We've encountered a cycle, fallback on the default set
							Debug.Fail("Cycle encountered in snippet inheritance");
							baseSnippets = (IVerbalizationSets<TEnum>)processedSets[defaultSnippetsIdentifier];
						}
					}
					if (baseSnippets == null)
					{
						RawSnippets<TEnum> requiredSnippets = rawSnippetsList.Find(
							delegate(RawSnippets<TEnum> match)
							{
								return match.Id.Equals(currentBaseId);
							});
						if (!requiredSnippets.IsEmpty)
						{
							baseSnippets = requiredSnippets.Process(processedSets, rawSnippetsList, defaultSnippetsIdentifier, identifiersOnly);
						}
						if (baseSnippets == null)
						{
							// Fallback case, base is not defined
							baseSnippets = (IVerbalizationSets<TEnum>)processedSets[defaultSnippetsIdentifier];
						}
					}
					Debug.Assert(baseSnippets != null); // Should always have some base at this point
					if (identifiersOnly)
					{
						retVal = new EmptyVerbalizationSets();
					}
					else
					{
						VerbalizationSets<TEnum> retValImpl = new PassthroughVerbalizationSets(baseSnippets);
						VerbalizationSets<TEnum>.Initialize(retValImpl, Snippets);
						retVal = retValImpl;
					}
					processingComplete = true;
				}
				finally
				{
					if (!processingComplete || retVal == null)
					{
						processedSets.Remove(currentId);
					}
					else if (retVal != null)
					{
						processedSets[currentId] = retVal;
					}
				}
				return retVal;
			}
			#endregion // Snippet processing
		}
		#endregion // XML processing structs
		#region XML element processing methods
		/// <summary>
		/// Load the snippets for a given enum type defining those snippets from a stream
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum for the values of the Snippet.@type property</typeparam>
		/// <param name="stream">The stream to load</param>
		/// <param name="identifiersOnly">Load identifiers only.</param>
		/// <param name="rawSnippetsList">The list to add results to. May be null.</param>
		private static void LoadSnippets<TEnum>(Stream stream, bool identifiersOnly, ref List<RawSnippets<TEnum>> rawSnippetsList) where TEnum : struct
		{
			VerbalizationSnippetSetsNameTable names = VerbalizationSnippetSets.Names;
			using (XmlTextReader settingsReader = new XmlTextReader(new StreamReader(stream), names))
			{
				using (XmlReader reader = XmlReader.Create(settingsReader, VerbalizationSnippetSets.ReaderSettings))
				{
					if (XmlNodeType.Element == reader.MoveToContent())
					{
						if (TestElementName(reader.NamespaceURI, names.SchemaNamespace) && TestElementName(reader.LocalName, names.LanguagesElement))
						{
							while (reader.Read())
							{
								XmlNodeType nodeType = reader.NodeType;
								if (nodeType == XmlNodeType.Element)
								{
									if (TestElementName(reader.LocalName, names.LanguageElement))
									{
										ProcessLanguage<TEnum>(reader, names, identifiersOnly, ref rawSnippetsList);
									}
									else
									{
										Debug.Fail("Validating reader should have failed");
										PassEndElement(reader);
									}
								}
								else if (nodeType == XmlNodeType.EndElement)
								{
									break;
								}
							}
						}
					}
				}
			}
		}
		private static void ProcessLanguage<TEnum>(XmlReader reader, VerbalizationSnippetSetsNameTable names, bool identifiersOnly, ref List<RawSnippets<TEnum>> rawSnippetsList) where TEnum : struct
		{
			if (reader.IsEmptyElement)
			{
				return;
			}
			string languageId = reader.GetAttribute(names.LangAttribute);
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (TestElementName(reader.LocalName, names.SnippetsElement))
					{
						RawSnippets<TEnum> rawSnippets = ProcessSnippets<TEnum>(reader, names, identifiersOnly, languageId);
						if (identifiersOnly ? !rawSnippets.Id.IsEmpty : !rawSnippets.IsEmpty)
						{
							if (rawSnippetsList == null)
							{
								rawSnippetsList = new List<RawSnippets<TEnum>>();
							}
							rawSnippetsList.Add(rawSnippets);
						}
					}
					else
					{
						Debug.Fail("Validating reader should have failed");
						PassEndElement(reader);
					}
				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
			}
		}
		private static RawSnippets<TEnum> ProcessSnippets<TEnum>(XmlReader reader, VerbalizationSnippetSetsNameTable names, bool identifiersOnly, string languageId) where TEnum : struct
		{
			RawSnippets<TEnum> retVal = default(RawSnippets<TEnum>);
			if (reader.IsEmptyElement)
			{
				return retVal;
			}
			string name = reader.GetAttribute(names.NameAttribute);
			string description = reader.GetAttribute(names.DescriptionAttribute);
			string baseName = reader.GetAttribute(names.BaseSnippetsNameAttribute);
			string baseLang = reader.GetAttribute(names.BaseSnippetsLanguageAttribute);
			if (identifiersOnly)
			{
				PassEndElement(reader);
			}
			else
			{
				while (reader.Read())
				{
					XmlNodeType nodeType = reader.NodeType;
					if (nodeType == XmlNodeType.Element)
					{
						if (TestElementName(reader.LocalName, names.SnippetElement))
						{
							ProcessSnippet<TEnum>(reader, names, ref retVal.Snippets);
						}
						else
						{
							Debug.Fail("Validating reader should have failed");
							PassEndElement(reader);
						}
					}
					else if (nodeType == XmlNodeType.EndElement)
					{
						break;
					}
				}
			}
			if (identifiersOnly || retVal.Snippets != null)
			{
				retVal.Id = new VerbalizationSnippetsIdentifier(typeof(TEnum), languageId, name, description);
				retVal.BaseId = new VerbalizationSnippetsIdentifier(typeof(TEnum), baseLang, baseName);
			}
			return retVal;
		}
		private static void ProcessSnippet<TEnum>(XmlReader reader, VerbalizationSnippetSetsNameTable names, ref List<RawSnippet<TEnum>> snippetList) where TEnum : struct
		{
			bool finishElement = true;
			try
			{
				TEnum type = (TEnum)Enum.Parse(typeof(TEnum), reader.GetAttribute(names.TypeAttribute));
				string modalityString = reader.GetAttribute(names.ModalityAttribute);
				ConstraintModality? modality = null;
				if (modalityString != null)
				{
					if (modalityString == "alethic")
					{
						modality = ConstraintModality.Alethic;
					}
					else if (modalityString == "deontic")
					{
						modality = ConstraintModality.Deontic;
					}
				}
				string signString = reader.GetAttribute(names.SignAttribute);
				bool? sign = null;
				if (signString != null)
				{
					if (signString == "positive")
					{
						sign = true;
					}
					else if (signString == "negative")
					{
						sign = false;
					}
				}
				string value = null;
				finishElement = false;
				if (!reader.IsEmptyElement)
				{
					value = reader.ReadString();
				}
				(snippetList ?? (snippetList = new List<RawSnippet<TEnum>>())).Add(
					new RawSnippet<TEnum>(type, value ?? string.Empty, modality, sign));
			}
			catch (ArgumentException)
			{
				// Invalid type, just swallow for now
			}
			finally
			{
				if (finishElement)
				{
					if (!reader.IsEmptyElement)
					{
						PassEndElement(reader);
					}
				}
			}
		}
		#endregion // XML element processing methods
		#region XML Helper Functions
		private static bool TestElementName(string localName, string elementName)
		{
			return (object)localName == (object)elementName;
		}
		/// <summary>
		/// Move the reader to the node immediately after the end element corresponding to the current open element
		/// </summary>
		/// <param name="reader">The XmlReader to advance</param>
		private static void PassEndElement(XmlReader reader)
		{
			if (!reader.IsEmptyElement)
			{
				bool finished = false;
				while (!finished && reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
							PassEndElement(reader);
							break;

						case XmlNodeType.EndElement:
							finished = true;
							break;
					}
				}
			}
		}
		#endregion // XML Helper Functions
	}
	#endregion // VerbalizationSnippetSetsManager class
	#region AvailableVerbalizationSnippetsEditor class
	/// <summary>
	/// An editor class to choose the current verbalization options
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class AvailableVerbalizationSnippetsEditor : TreePicker
	{
		#region Abstract properties
		/// <summary>
		/// The directory containing snippet XML files
		/// </summary>
		protected abstract string VerbalizationDirectory { get;}
		/// <summary>
		/// An enumerator of snippets providers
		/// </summary>
		protected abstract IEnumerable<IVerbalizationSnippetsProvider> SnippetsProviders { get;}
		/// <summary>
		/// A format string indicating how language information should be combined
		/// with the description of a given snippet. The {0} replacement field gets
		/// the description, the {1} gets the field value. Return null or empty to
		/// not display any language information.
		/// </summary>
		protected abstract string LanguageFormatString { get;}
		#endregion // Abstract properties
		#region TreePicker overrides
		/// <summary>
		/// Get the ITree representing the current verbalization settings
		/// </summary>
		protected override ITree GetTree(ITypeDescriptorContext context, object value)
		{
			ITree tree = new VirtualTree();
			IBranch rootBranch = new ProviderBranch((string)value, SnippetsProviders, VerbalizationDirectory, LanguageFormatString);
			tree.Root = rootBranch;
			if (rootBranch.VisibleItemCount == 1)
			{
				tree.ToggleExpansion(0, 0);
			}
			return tree;
		}
		/// <summary>
		/// Translate the tree settings to a useable value
		/// </summary>
		protected override object TranslateToValue(ITypeDescriptorContext context, object oldValue, ITree tree, int selectedRow, int selectedColumn)
		{
			return VerbalizationSnippetsIdentifier.SaveIdentifiers(((ProviderBranch)tree.Root).CurrentIdentifiers);
		}
		private static Size myLastControlSize = new Size(272, 128);
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
		#endregion // TreePicker overrides
		#region ProviderBranch class
		/// <summary>
		/// A branch class to display snippet types
		/// </summary>
		private sealed class ProviderBranch : BaseBranch, IBranch
		{
			#region SnippetsType structure
			private struct SnippetsType
			{
				public string TypeDescription;
				public string EnumTypeName;
				public int FirstIdentifier;
				public int LastIdentifier;
				public int CurrentIdentifier;
			}
			#endregion // SnippetsType structure
			#region Member Variables
			private List<SnippetsType> myTypes;
			private VerbalizationSnippetsIdentifier[] myIdentifiers;
			private string[] myItemStrings;
			private string myLanguageFormatString;
			#endregion // Member Variables
			#region Constructors
			public ProviderBranch(string currentSettings, IEnumerable<IVerbalizationSnippetsProvider> providers, string verbalizationDirectory, string languageFormatString)
			{
				VerbalizationSnippetsIdentifier[] allIdentifiers = VerbalizationSnippetSetsManager.LoadAvailableSnippets(
					providers,
					verbalizationDirectory);
				VerbalizationSnippetsIdentifier[] currentIdentifiers = VerbalizationSnippetsIdentifier.ParseIdentifiers(currentSettings);

				if (languageFormatString != null)
				{
					if (languageFormatString.Length != 0)
					{
						myLanguageFormatString = languageFormatString;
					}
					else
					{
						languageFormatString = null;
					}
				}

				// Gather all types
				List<SnippetsType> types = new List<SnippetsType>();
				foreach (IVerbalizationSnippetsProvider provider in providers)
				{
					VerbalizationSnippetsData[] currentData = provider.ProvideVerbalizationSnippets();
					int count = (currentData != null) ? currentData.Length : 0;
					for (int i = 0; i < count; ++i)
					{
						SnippetsType currentType = default(SnippetsType);
						currentType.TypeDescription = currentData[i].TypeDescription;
						currentType.EnumTypeName = currentData[i].EnumType.FullName;
						types.Add(currentType);
					}
				}

				// Sort first by type description
				types.Sort(
					delegate(SnippetsType type1, SnippetsType type2)
					{
						return string.Compare(type1.TypeDescription, type2.TypeDescription, StringComparison.CurrentCultureIgnoreCase);
					});

				// Sort all identifiers. First by type description on previous sort, then
				// putting the default identifier first, then by the identifier description
				int typesCount = types.Count;
				Array.Sort<VerbalizationSnippetsIdentifier>(
					allIdentifiers,
					delegate(VerbalizationSnippetsIdentifier identifier1, VerbalizationSnippetsIdentifier identifier2)
					{
						int retVal = 0;
						string typeName1 = identifier1.EnumTypeName;
						string typeName2 = identifier2.EnumTypeName;
						if (typeName1 != typeName2)
						{
							int location1 = -1;
							for (int i = 0; i < typesCount; ++i)
							{
								if (0 == string.CompareOrdinal(types[i].EnumTypeName, typeName1))
								{
									location1 = i;
									break;
								}
							}
							int location2 = -1;
							for (int i = 0; i < typesCount; ++i)
							{
								if (0 == string.CompareOrdinal(types[i].EnumTypeName, typeName2))
								{
									location2 = i;
									break;
								}
							}
							retVal = location1.CompareTo(location2);
						}
						if (retVal == 0)
						{
							bool isDefault1 = identifier1.IsDefaultIdentifier;
							bool isDefault2 = identifier2.IsDefaultIdentifier;
							if (isDefault1)
							{
								if (!isDefault2)
								{
									retVal = -1;
								}
							}
							else if (isDefault2)
							{
								retVal = 1;
							}
						}
						if (retVal == 0)
						{
							retVal = string.Compare(identifier1.Description, identifier2.Description, StringComparison.CurrentCultureIgnoreCase);
						}
						return retVal;
					});

				// Now, associate indices in the sorted allIdentifiersList with each type
				int allIdentifiersCount = allIdentifiers.Length;
				int nextIdentifier = 0;
				for (int i = 0; i < typesCount; ++i)
				{
					SnippetsType currentType = types[i];
					string matchTypeName = currentType.EnumTypeName;
					currentType.FirstIdentifier = nextIdentifier;
					currentType.LastIdentifier = nextIdentifier;
					currentType.CurrentIdentifier = nextIdentifier;
					Debug.Assert(allIdentifiers[nextIdentifier].IsDefaultIdentifier && allIdentifiers[nextIdentifier].EnumTypeName == matchTypeName, "No default snippets identifier for " + matchTypeName);
					++nextIdentifier;
					bool matchedCurrent = currentIdentifiers == null;
					for (; nextIdentifier < allIdentifiersCount; ++nextIdentifier)
					{
						if (0 != string.CompareOrdinal(allIdentifiers[nextIdentifier].EnumTypeName, matchTypeName))
						{
							break;
						}
						currentType.LastIdentifier = nextIdentifier;
						if (!matchedCurrent)
						{
							if (((ICollection<VerbalizationSnippetsIdentifier>)currentIdentifiers).Contains(allIdentifiers[nextIdentifier]))
							{
								currentType.CurrentIdentifier = nextIdentifier;
								matchedCurrent = true;
							}
						}
					}
					types[i] = currentType;
				}
				myTypes = types;
				myIdentifiers = allIdentifiers;
				if (languageFormatString != null)
				{
					myItemStrings = new string[allIdentifiersCount];
				}
			}
			#endregion // Constructors
			#region ProviderBranch specific
			/// <summary>
			/// An enumerable of current identifiers
			/// </summary>
			public IEnumerable<VerbalizationSnippetsIdentifier> CurrentIdentifiers
			{
				get
				{
					List<SnippetsType> types = myTypes;
					if (types != null)
					{
						VerbalizationSnippetsIdentifier[] allIdentifiers = myIdentifiers;
						int typesCount = types.Count;
						for (int i = 0; i < typesCount; ++i)
						{
							yield return allIdentifiers[types[i].CurrentIdentifier];
						}
					}
				}
			}
			#endregion // ProviderBranch specific
			#region IBranch implementation
			int IBranch.VisibleItemCount
			{
				get
				{
					return myTypes.Count;
				}
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.Expansions;
				}
			}
			string IBranch.GetText(int row, int column)
			{
				return myTypes[row].TypeDescription;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return true;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				Debug.Assert(style == ObjectStyle.ExpandedBranch);
				return new Subbranch(this, row);
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				retVal.Bold = true;
				return retVal;
			}
			#endregion // IBranch implementation
			#region Subbranch class
			private sealed class Subbranch : BaseBranch, IBranch
			{
				#region Member Variables
				private ProviderBranch myParentBranch;
				private int myTypeIndex;
				#endregion // Member Variables
				#region Constructors
				public Subbranch(ProviderBranch parentBranch, int typeIndex)
				{
					myParentBranch = parentBranch;
					myTypeIndex = typeIndex;
				}
				#endregion // Constructors
				#region IBranch implementation
				int IBranch.VisibleItemCount
				{
					get
					{
						SnippetsType type = myParentBranch.myTypes[myTypeIndex];
						return type.LastIdentifier - type.FirstIdentifier + 1;
					}
				}
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.StateChanges;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					ProviderBranch parentBranch = myParentBranch;
					string[] itemStrings = parentBranch.myItemStrings;
					string retVal;
					if (itemStrings != null)
					{
						retVal = itemStrings[row];
						if (retVal == null)
						{
							SnippetsType type = parentBranch.myTypes[myTypeIndex];
							VerbalizationSnippetsIdentifier id = parentBranch.myIdentifiers[type.FirstIdentifier + row];
							if (id.IsDefaultIdentifier)
							{
								retVal = id.Description;
							}
							else
							{
								string languageName = id.LanguageId;
								try
								{
									languageName = CultureInfo.GetCultureInfoByIetfLanguageTag(id.LanguageId).DisplayName;
								}
								catch (ArgumentException)
								{
								}
								retVal = string.Format(CultureInfo.CurrentCulture, parentBranch.myLanguageFormatString, id.Description, languageName);
							}
							itemStrings[row] = retVal;
						}
					}
					else
					{
						retVal = parentBranch.myIdentifiers[parentBranch.myTypes[myTypeIndex].FirstIdentifier + row].Description;
					}
					return retVal;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					SnippetsType type = myParentBranch.myTypes[myTypeIndex];
					if (type.CurrentIdentifier == (type.FirstIdentifier + row))
					{
						retVal.Bold = true;
						retVal.StateImageIndex = (short)StandardCheckBoxImage.CheckedFlat;
					}
					else
					{
						retVal.StateImageIndex = (short)StandardCheckBoxImage.UncheckedFlat;
					}
					return retVal;
				}
				StateRefreshChanges IBranch.ToggleState(int row, int column)
				{
					SnippetsType type = myParentBranch.myTypes[myTypeIndex];
					int identifierIndex = type.FirstIdentifier + row;
					if (type.CurrentIdentifier != identifierIndex)
					{
						type.CurrentIdentifier = identifierIndex;
						myParentBranch.myTypes[myTypeIndex] = type;
						return StateRefreshChanges.ParentsChildren;
					}
					return StateRefreshChanges.None;
				}
				#endregion // IBranch implementation
			}
			#endregion // Subbranch class
		}
		#endregion // ProviderBranch class
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
	}
	#endregion // AvailableVerbalizationSnippetsEditor class
}

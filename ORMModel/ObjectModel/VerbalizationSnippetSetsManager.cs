#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region GenerateVerbalizationReport delegate
	/// <summary>
	/// Callback delegate provided with <see cref="VerbalizationTargetData"/> for generating report information
	/// </summary>
	/// <param name="store">The context <see cref="Store"/>. The store will implement the <see cref="IORMToolServices"/> interface,
	/// which can be used to retrieve an <see cref="IServiceProvider"/> and other functionality.</param>
	public delegate void GenerateVerbalizationReport(Store store);
	#endregion // GenerateVerbalizationReport delegate
	#region VerbalizationTargetData structure
	/// <summary>
	/// Information representing the target type for an element
	/// </summary>
	public struct VerbalizationTargetData
	{
		#region Member variables
		private string myKeyName;
		private string myDisplayName;
		private string myReportCommandName;
		private GenerateVerbalizationReport myReportCallback;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create a new <see cref="VerbalizationTargetData"/> without report generation support
		/// </summary>
		/// <param name="keyName">The key name for this verbalization target.</param>
		/// <param name="displayName">The publicly display name for the target. This string should be localized.</param>
		public VerbalizationTargetData(string keyName, string displayName)
			: this(keyName, displayName, null, null)
		{
		}
		/// <summary>
		/// Create a new <see cref="VerbalizationTargetData"/> with report generation support
		/// </summary>
		/// <param name="keyName">The key name for this verbalization target.</param>
		/// <param name="displayName">The publicly display name for the target. This string should be localized.</param>
		/// <param name="reportCommandName">The name of the report command displayed for this target in the report menu.</param>
		/// <param name="reportCallback">The <see cref="GenerateVerbalizationReport"/> callback delegate.</param>
		public VerbalizationTargetData(string keyName, string displayName, string reportCommandName, GenerateVerbalizationReport reportCallback)
		{
			myKeyName = keyName;
			myDisplayName = displayName;
			myReportCommandName = reportCommandName;
			myReportCallback = reportCallback;
		}
		#endregion // Constructors
		#region Accessor properties
		/// <summary>
		/// The unique name for this verbalization target. This name will
		/// be used in XML and by <see cref="VerbalizationSnippetsData"/>
		/// instances to refer to this target.
		/// </summary>
		public string KeyName
		{
			get
			{
				return myKeyName;
			}
		}
		/// <summary>
		/// The friendly display name for this target. Shown in the options dialog.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return myDisplayName;
			}
		}
		/// <summary>
		/// If a ReportCommandName is not null then it will automatically display as one of the default
		/// report targets. The <see cref="ReportCallback"/> delegate will be used to execute the command.
		/// </summary>
		public string ReportCommandName
		{
			get
			{
				return myReportCommandName;
			}
		}
		/// <summary>
		/// Generate a report for this verbalization target. May be <see langword="null"/>.
		/// </summary>
		public GenerateVerbalizationReport ReportCallback
		{
			get
			{
				return myReportCallback;
			}
		}
		/// <summary>
		/// Return true if this verbalization target supports report generation
		/// </summary>
		public bool CanReport
		{
			get
			{
				return myReportCallback != null && myReportCommandName != null;
			}
		}
		#endregion // Accessor properties
		#region Equality overrides
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(VerbalizationTargetData data1, VerbalizationTargetData data2)
		{
			return data1.Equals(data2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(VerbalizationTargetData data1, VerbalizationTargetData data2)
		{
			return !(data1.Equals(data2));
		}
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is VerbalizationTargetData) ? Equals((VerbalizationTargetData)obj) : false;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(VerbalizationTargetData obj)
		{
			bool leftEmpty = myKeyName == null;
			bool rightEmpty = obj.myKeyName == null;
			if (leftEmpty && rightEmpty)
			{
				return true;
			}
			else if (!leftEmpty && !rightEmpty)
			{
				// Note that everything other than the key name is ignored
				return myKeyName == obj.myKeyName;
			}
			return false;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			// Note that everything other than the key name is ignored
			return (myKeyName == null) ? 0 : myKeyName.GetHashCode();
		}
		#endregion // Equality overrides
	}
	#endregion // VerbalizationTargetData structure
	#region IVerbalizationTargetProvider interface
	/// <summary>
	/// The IVerbalizationTargetsProvider
	/// </summary>
	public interface IVerbalizationTargetProvider
	{
		/// <summary>
		/// Return an array of supported verbalization targets
		/// </summary>
		/// <returns><see cref="VerbalizationTargetData"/> array</returns>
		VerbalizationTargetData[] ProvideVerbalizationTargets();
	}
	#endregion // IVerbalizationTargetProvider interface
	#region VerbalizationTargetProviderAttribute class
	/// <summary>
	/// Provide an IVerbalizationTargetProvider implementation for a DomainModel
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class VerbalizationTargetProviderAttribute : Attribute
	{
		private Type myProviderType;
		private string myNestedProviderName;
		/// <summary>
		/// Associate an IVerbalizationTargetProvider implementation with a DomainModel-derived class
		/// </summary>
		/// <param name="providerType">A type that implements IVerbalizationTargetProvider
		/// and has a parameterless constructor</param>
		public VerbalizationTargetProviderAttribute(Type providerType)
		{
			myProviderType = providerType;
		}
		/// <summary>
		/// Associate an IVerbalizationtargetProvider implementation with a DomainModel-derived class
		/// </summary>
		/// <param name="nestedTypeName">The name of a nested class in the DomainModel that implements
		/// the IVerbalizationTargetProvider interface.</param>
		public VerbalizationTargetProviderAttribute(string nestedTypeName)
		{
			myNestedProviderName = nestedTypeName;
		}
		/// <summary>
		/// Create an instance of the associated snippets provider
		/// </summary>
		/// <param name="domainModelType">The type of the associated domain model</param>
		/// <returns>IVerbalizationTargetProvider implementation</returns>
		public IVerbalizationTargetProvider CreateTargetProvider(Type domainModelType)
		{
			Type createType = myProviderType;
			if (createType == null)
			{
				string[] nestedTypeNames = myNestedProviderName.Split(new char[] { '.', '+' }, StringSplitOptions.RemoveEmptyEntries);
				createType = domainModelType;
				for (int i = 0; i < nestedTypeNames.Length; ++i)
				{
					createType = createType.GetNestedType(nestedTypeNames[i], BindingFlags.NonPublic | BindingFlags.Public);
				}
			}
			return (IVerbalizationTargetProvider)Activator.CreateInstance(createType, true);
		}
	}
	#endregion // VerbalizationTargetProviderAttribute class
	#region VerbalizationSnippetsData struct
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
			return (myEnumType == null) ? 0 :
				ORMSolutions.ORMArchitect.Framework.Utility.GetCombinedHashCode(
					myEnumType.GetHashCode(),
					myDefaultVerbalizationSets.GetHashCode(),
					myAlternateSnippetsDirectory.GetHashCode(),
					myDefaultSetsDescription.GetHashCode(),
					myTypeDescription.GetHashCode());
		}
		#endregion // Equality overrides
	}
	#endregion // VerbalizationSnippetsData struct
	#region IVerbalizationSnippetsProvider interface
	/// <summary>
	/// The IVerbalizationSnippetsProvider interfaces enables
	/// a model to provide verbalization snippets to the ORM
	/// verbalization engine. Snippets are identified by an
	/// enum type used to retrieve snippets. A DomainModel
	/// uses the VerbalizationSnippetsProviderAttribute to
	/// indicate the class that provides verbalization snippets.
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
	#region VerbalizationSnippetsProviderAttribute class
	/// <summary>
	/// Provide an IVerbalizationSnippetsProvider implementation for a DomainModel
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class VerbalizationSnippetsProviderAttribute : Attribute
	{
		private Type myProviderType;
		private string myNestedProviderName;
		/// <summary>
		/// Associate an IVerbalizationSnippetsProvider implementation with a DomainModel-derived class
		/// </summary>
		/// <param name="providerType">A type that implements IVerbalizationSnippetsProvider
		/// and has a parameterless constructor</param>
		public VerbalizationSnippetsProviderAttribute(Type providerType)
		{
			myProviderType = providerType;
		}
		/// <summary>
		/// Associate an IVerbalizationSnippetsProvider implementation with a DomainModel-derived class
		/// </summary>
		/// <param name="nestedTypeName">The name of a nested class in the DomainModel that implements
		/// the IVerbalizationSnippetsProvider interface.</param>
		public VerbalizationSnippetsProviderAttribute(string nestedTypeName)
		{
			myNestedProviderName = nestedTypeName;
		}
		/// <summary>
		/// Create an instance of the associated snippets provider
		/// </summary>
		/// <param name="domainModelType">The type of the associated domain model</param>
		/// <returns>IVerbalizationSnippetsProvider implementation</returns>
		public IVerbalizationSnippetsProvider CreateSnippetsProvider(Type domainModelType)
		{
			Type createType = myProviderType;
			if (createType == null)
			{
				string[] nestedTypeNames = myNestedProviderName.Split(new char[] { '.', '+' }, StringSplitOptions.RemoveEmptyEntries);
				createType = domainModelType;
				for (int i = 0; i < nestedTypeNames.Length; ++i)
				{
					createType = createType.GetNestedType(nestedTypeNames[i], BindingFlags.NonPublic | BindingFlags.Public);
				}
			}
			return (IVerbalizationSnippetsProvider)Activator.CreateInstance(createType, true);
		}
	}
	#endregion // VerbalizationSnippetsProviderAttribute class
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
		private string myTarget;
		private const string DefaultLanguageId = "en-US";
		private const string DefaultId = "_default";
		/// <summary>
		/// The default target name if no explicit target is specified
		/// </summary>
		public const string DefaultTarget = "";
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a snippets identifier
		/// </summary>
		/// <param name="enumType">An enum describing the snippet types</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		public VerbalizationSnippetsIdentifier(Type enumType, string languageId, string id)
			: this(enumType, DefaultTarget, languageId, id, null)
		{
		}
		/// <summary>
		/// Create an explicitly targeted snippets identifier
		/// </summary>
		/// <param name="enumType">An enum describing the snippet types</param>
		/// <param name="target">The target output of the Verbalization Snippets set</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		public VerbalizationSnippetsIdentifier(Type enumType, string target, string languageId, string id)
			: this(enumType, target, languageId, id, null)
		{
		}
		/// <summary>
		/// Create a snippets identifier with a description
		/// </summary>
		/// <param name="enumType">An enum describing the snippet types</param>
		/// <param name="target">The target output of the Verbalization Snippets set</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		/// <param name="description">A displayable description for this identifier</param>
		public VerbalizationSnippetsIdentifier(Type enumType, string target, string languageId, string id, string description)
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
			myTarget = target ?? DefaultTarget;
		}
		/// <summary>
		/// Create a snippets identifier from a type name
		/// </summary>
		/// <param name="enumTypeName">The namespace-qualified name of an enum representing the snippets</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		public VerbalizationSnippetsIdentifier(string enumTypeName, string languageId, string id)
			: this(enumTypeName, DefaultTarget, languageId, id, null)
		{
		}
		/// <summary>
		/// Create a snippets identifier from a type name
		/// </summary>
		/// <param name="enumTypeName">The namespace-qualified name of an enum representing the snippets</param>
		/// <param name="target">The target output of the Verbalization Snippets set</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		public VerbalizationSnippetsIdentifier(string enumTypeName, string target, string languageId, string id)
			: this(enumTypeName, target, languageId, id, null)
		{
		}
		/// <summary>
		/// Create a snippets identifier with a description from a type name
		/// </summary>
		/// <param name="enumTypeName">The namespace-qualified name of an enum representing the snippets</param>
		/// <param name="target">The target output of the Verbalization Snippets set</param>
		/// <param name="languageId">The name of the language identifying the snippets</param>
		/// <param name="id">The identifier for the snippets</param>
		/// <param name="description">A displayable description for this identifier</param>
		public VerbalizationSnippetsIdentifier(string enumTypeName, string target, string languageId, string id, string description)
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
			myDescription = description;
			myTarget = target ?? DefaultTarget;
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
		/// Return true if this is a default identifier for the enum type, ignoring the target type.
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
		/// Return true if this is a default identifier for the enum type and target.
		/// Default identifiers are created with the static CreateDefaultIdentifier method.
		/// </summary>
		/// <param name="target">The verbalization target</param>
		public bool IsTargetedDefaultIdentifier(string target)
		{
			return !IsEmpty && myLangId == DefaultLanguageId && myId == DefaultId && myTarget == target;
		}
		/// <summary>
		/// Create a default identifier for the specified enum type
		/// </summary>
		/// <param name="enumType">A type representing an enum</param>
		/// <param name="description">An optional description for the identifier. Can be null</param>
		/// <returns>VerbalizationSnippetsIdentifier with default language and id</returns>
		public static VerbalizationSnippetsIdentifier CreateDefaultIdentifier(Type enumType, string description)
		{
			return new VerbalizationSnippetsIdentifier(enumType, DefaultTarget, DefaultLanguageId, DefaultId, description);
		}
		/// <summary>
		/// Create a default identifier for the specified enum type
		/// </summary>
		/// <param name="enumType">A type representing an enum</param>
		/// <param name="target">The verbalization target</param>
		/// <param name="description">An optional description for the identifier. Can be null</param>
		/// <returns>VerbalizationSnippetsIdentifier with default language and id</returns>
		public static VerbalizationSnippetsIdentifier CreateDefaultIdentifier(Type enumType, string target, string description)
		{
			return new VerbalizationSnippetsIdentifier(enumType, target, DefaultLanguageId, DefaultId, description);
		}
		/// <summary>
		/// Create a default identifier for the specified enum type name
		/// </summary>
		/// <param name="enumTypeName">The full name of an enum type</param>
		/// <param name="description">An optional description for the identifier. Can be null</param>
		/// <returns>VerbalizationSnippetsIdentifier with default language and id</returns>
		public static VerbalizationSnippetsIdentifier CreateDefaultIdentifier(string enumTypeName, string description)
		{
			return new VerbalizationSnippetsIdentifier(enumTypeName, DefaultTarget, DefaultLanguageId, DefaultId, description);
		}
		/// <summary>
		/// Create a default identifier for the specified enum type name
		/// </summary>
		/// <param name="enumTypeName">The full name of an enum type</param>
		/// <param name="target">The verbalization target</param>
		/// <param name="description">An optional description for the identifier. Can be null</param>
		/// <returns>VerbalizationSnippetsIdentifier with default language and id</returns>
		public static VerbalizationSnippetsIdentifier CreateDefaultIdentifier(string enumTypeName, string target, string description)
		{
			return new VerbalizationSnippetsIdentifier(enumTypeName, target, DefaultLanguageId, DefaultId, description);
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
		/// <summary>
		/// The target output of the Verbalization Snippets set
		/// </summary>
		public string Target
		{
			get
			{
				return myTarget;
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
						int fieldsLength = fields.Length;
						Debug.Assert(fieldsLength == 3 || fieldsLength == 4, "The string passed to ParseIdentifiers must be saved with SaveIdentifiers");
						if (fieldsLength == 4)
						{
							retVal[i] = new VerbalizationSnippetsIdentifier(fields[0], fields[3], fields[1], fields[2]);
						}
						else
						{
							retVal[i] = new VerbalizationSnippetsIdentifier(fields[0], fields[1], fields[2]);
						}
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
						if (identifier.Target != DefaultTarget)
						{
							sb.Append(',');
							sb.Append(identifier.Target.ToString());
						}
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
					myId == obj.myId &&
					myTarget == obj.myTarget;
			}
			return false;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			// Note that myDescription is intentionally ignored
			return (myEnumTypeName == null) ? 0 :
				ORMSolutions.ORMArchitect.Framework.Utility.GetCombinedHashCode(myEnumTypeName.GetHashCode(), myLangId.GetHashCode(), myId.GetHashCode());
		}
		#endregion // Equality overrides
	}
	#endregion // VerbalizationSnippetsIdentifier struct
	#region VerbalizationSnippetSetsManager class
	/// <summary>
	/// Class for managing and loading snippet sets
	/// </summary>
	public static class VerbalizationSnippetSetsManager
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
			public const string TargetAttribute = "target";
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
			public readonly string TargetAttribute;
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
				TargetAttribute = Add(VerbalizationSnippetSets.TargetAttribute);
				DescriptionAttribute = Add(VerbalizationSnippetSets.DescriptionAttribute);
				BaseSnippetsNameAttribute = Add(VerbalizationSnippetSets.BaseSnippetsNameAttribute);
				BaseSnippetsLanguageAttribute = Add(VerbalizationSnippetSets.BaseSnippetsLanguageAttribute);
			}
		}
		#endregion // VerbalizationSnippetSetsNameTable class
		#endregion // Snippet Sets Schema definition classes
		#region Directory and file loading methods
		/// <summary>
		/// Load all verbalization snippets provided by <see cref="DomainModel"/>s in the provided <see cref="Store"/>.
		/// </summary>
		/// <param name="store">The store to load</param>
		/// <param name="target">The verbalization target to load.</param>
		/// <param name="customSnippetsDirectory">The base directory to search for additional snippets</param>
		/// <param name="customIdentifiers">An array of preferred custom identifiers
		/// for the preferred verbalization sets. Can be null if no customizations are in place.</param>
		/// <returns>Snippets dictionary</returns>
		public static IDictionary<Type, IVerbalizationSets> LoadSnippetsDictionary(Store store, string target, string customSnippetsDirectory, VerbalizationSnippetsIdentifier[] customIdentifiers)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			ICollection<DomainModel> domainModels = store.DomainModels;
			List<IVerbalizationSnippetsProvider> snippetProviders = new List<IVerbalizationSnippetsProvider>(domainModels.Count);
			foreach (DomainModel domainModel in domainModels)
			{
				Type domainModelType = domainModel.GetType();
				object[] providers = domainModelType.GetCustomAttributes(typeof(VerbalizationSnippetsProviderAttribute), false);
				if (providers.Length != 0) // Single use non-inheritable attribute, there will only be one
				{
					IVerbalizationSnippetsProvider provider = ((VerbalizationSnippetsProviderAttribute)providers[0]).CreateSnippetsProvider(domainModelType);
					if (provider != null)
					{
						snippetProviders.Add(provider);
					}
				}
			}
			return LoadSnippetsDictionary(snippetProviders, target, customSnippetsDirectory, customIdentifiers);
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
		/// Load all verbalization snippets provided by <see cref="DomainModel"/>s in the provided <see cref="Store"/>.
		/// </summary>
		/// <param name="providers">The snippet providers</param>
		/// <param name="target">The verbalization target to load.</param>
		/// <param name="customSnippetsDirectory">The base directory to search for additional snippets</param>
		/// <param name="customIdentifiers">An array of preferred custom identifiers
		/// for the preferred verbalization sets. Can be null if no customizations are in place.</param>
		/// <returns>Snippets dictionary</returns>
		public static IDictionary<Type, IVerbalizationSets> LoadSnippetsDictionary(IEnumerable<IVerbalizationSnippetsProvider> providers, string target, string customSnippetsDirectory, VerbalizationSnippetsIdentifier[] customIdentifiers)
		{
			// UNDONE: The API here should change to load the full dictionary regardless of target and
			// return a dictionary keyed off a targeted identifier. A wrapper can then be placed on
			// this set to provide a dictionary with this API. As it stands, this walks the dictionary
			// once per target, although it loads the full dictionary each time.
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
								if (testIdentifier.EnumTypeName == enumTypeName)
								{
									if (target == VerbalizationSnippetsIdentifier.DefaultTarget)
									{
										if (testIdentifier.Target == VerbalizationSnippetsIdentifier.DefaultTarget &&
											!testIdentifier.IsDefaultIdentifier)
										{
											customIdentifierIndex = j;
											break;
										}
									}
									else if (testIdentifier.Target == target)
									{
										customIdentifierIndex = j;
										break;
									}
									else if (testIdentifier.Target == VerbalizationSnippetsIdentifier.DefaultTarget && !testIdentifier.IsDefaultIdentifier)
									{
										// Fall back on default if a custom target is not available.
										// Don't break for cases where the custom target is listed later.
										customIdentifierIndex = j;
									}
								}
							}
							if (customIdentifierIndex != -1 || target != VerbalizationSnippetsIdentifier.DefaultTarget)
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
								if (!allSets.TryGetValue(
									(customIdentifierIndex != -1) ? customIdentifiers[customIdentifierIndex] : VerbalizationSnippetsIdentifier.CreateDefaultIdentifier(enumType, target, data.DefaultSetsDescription),
									out useVerbalization))
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
							if (currentBaseId.Target != VerbalizationSnippetsIdentifier.DefaultTarget &&
								currentBaseId.IsTargetedDefaultIdentifier(currentBaseId.Target))
							{
								if (string.IsNullOrEmpty(currentBaseId.Description))
								{
									currentBaseId = new VerbalizationSnippetsIdentifier(currentBaseId.EnumTypeName, currentBaseId.Target, currentBaseId.LanguageId, currentBaseId.Id, defaultSnippetsIdentifier.Description);
								}
								processedSets.Add(currentBaseId, baseSnippets);
							}
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
			string target = reader.GetAttribute(names.TargetAttribute);
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
				VerbalizationSnippetsIdentifier retValId;
				retVal.Id = retValId = new VerbalizationSnippetsIdentifier(
					typeof(TEnum),
					target,
					languageId,
					name,
					description);
				retVal.BaseId = new VerbalizationSnippetsIdentifier(typeof(TEnum), retValId.IsDefaultIdentifier ? VerbalizationSnippetsIdentifier.DefaultTarget : target, baseLang, baseName);
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
}

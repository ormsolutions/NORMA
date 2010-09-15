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
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region NameAlias class
	public partial class NameAlias
	{
		#region CustomStorage handlers
		private DomainClassInfo myConsumerDomainClass;
		private DomainClassInfo myUsageDomainClass;

		private string GetNameConsumerValue()
		{
			return ObjectModel.NameConsumer.TranslateToConsumerIdentifier(myConsumerDomainClass);
		}

		private void SetNameConsumerValue(string value)
		{
			myConsumerDomainClass = ObjectModel.NameConsumer.TranslateFromConsumerIdentifier(Store, value);
		}

		private string GetNameUsageValue()
		{
			return ObjectModel.NameUsage.TranslateToNameUsageIdentifier(myUsageDomainClass);
		}

		private void SetNameUsageValue(string value)
		{
			myUsageDomainClass = ObjectModel.NameUsage.TranslateFromNameUsageIdentifier(Store, value);
		}
		/// <summary>
		/// Return the <see cref="Type"/> assocatiated with the <see cref="NameUsage"/> property.
		/// Returns <see langword="null"/> if NameUsage is not set.
		/// </summary>
		public Type NameUsageType
		{
			get
			{
				DomainClassInfo classInfo = myUsageDomainClass;
				return (classInfo != null) ? classInfo.ImplementationClass : null;
			}
		}
		/// <summary>
		/// Return the <see cref="DomainClassInfo"/> assocatiated with the <see cref="NameConsumer"/> property.
		/// Will not return <see langword="null"/>
		/// </summary>
		public DomainClassInfo NameConsumerDomainClass
		{
			get
			{
				return myConsumerDomainClass;
			}
		}
		#endregion // CustomStorage handlers
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		///	validates NameConsumer binding for all <see cref="NameAlias"/>es
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new NameAliasFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit FactConstraint relationships
		/// </summary>
		private sealed class NameAliasFixupListener : DeserializationFixupListener<NameAlias>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public NameAliasFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Makes sure that any <see cref="NameAlias"/> that fails to resolve is deleted.
			/// </summary>
			/// <param name="element">NameAlias element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(NameAlias element, Store store, INotifyElementAdded notifyAdded)
			{
				element.DeleteIfTypeBindingFailed();
			}
		}
		/// <summary>
		/// AddRule: typeof(NameAlias), FireTime=LocalCommit, Priority=FrameworkDomainModel.CopyClosureExpansionCompletedRulePriority;
		/// </summary>
		private static void AliasAddedClosureRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted &&
				CopyMergeUtility.GetIntegrationPhase(element.Store) == CopyClosureIntegrationPhase.IntegrationComplete)
			{
				((NameAlias)element).DeleteIfTypeBindingFailed();
			}
		}
		/// <summary>
		/// Shared helper for merge integration and deserialization. Deletes
		/// the element if the types referenced by the consumer and usage
		/// are not loaded in the store.
		/// </summary>
		private void DeleteIfTypeBindingFailed()
		{
			Type consumer;
			Type usage;
			object[] attributes = null;
			DomainClassInfo consumerDomainClass;
			if (null == (consumerDomainClass = myConsumerDomainClass) ||
				null == (consumer = consumerDomainClass.ImplementationClass) ||
				(null != (usage = NameUsageType) &&
				null == (attributes = consumer.GetCustomAttributes(typeof(NameUsageAttribute), true))))
			{
				Delete();
			}
			else if (usage != null)
			{
				int i = 0;
				for (; i < attributes.Length; ++i)
				{
					if (((NameUsageAttribute)attributes[i]).Type == usage)
					{
						break;
					}
				}
				if (i == attributes.Length)
				{
					Delete();
				}
			}
		}
		#endregion // Deserialization Fixup
	}
	#endregion // NameAlias class
	#region NameConsumerIdentifierAttribute class
	/// <summary>
	/// NameConsumerIdentifierAttribute allows a <see cref="NameConsumer"/> to provide an identifier other than the derived name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class NameConsumerIdentifierAttribute : Attribute
	{
		private string myName;
		/// <summary>
		/// Create a new <see cref="NameConsumerIdentifierAttribute"/> to provide an identifier other than
		/// the <see cref="NameConsumer"/>-derived class name.
		/// </summary>
		/// <param name="name">Name used to identify a <see cref="NameConsumer"/> in the <see cref="NameAlias.NameConsumer"/> property.</param>
		public NameConsumerIdentifierAttribute(string name)
		{
			myName = name;
		}

		/// <summary>
		/// Name used to identify a <see cref="NameConsumer"/> in the <see cref="NameAlias.NameConsumer"/> property.
		/// </summary>
		public string Name
		{
			get
			{
				return myName;
			}
		}
	}
	#endregion // NameConsumeIdentifierAttribute class
	#region NameUsageIdentifierAttribute class
	/// <summary>
	/// NameUsageIdentifierAttribute allows a <see cref="NameUsage"/> to provide an identifier other than the derived name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class NameUsageIdentifierAttribute : Attribute
	{
		private string myName;
		/// <summary>
		/// Create a new <see cref="NameConsumerIdentifierAttribute"/> to provide an identifier other than
		/// the <see cref="NameUsage"/>-derived class name.
		/// </summary>
		/// <param name="name">Name used to identify a <see cref="NameUsage"/> in the <see cref="NameAlias.NameUsage"/> property.</param>
		public NameUsageIdentifierAttribute(string name)
		{
			myName = name;
		}

		/// <summary>
		/// Name used to identify a <see cref="NameUsage"/> in the <see cref="NameAlias.NameUsage"/> property.
		/// </summary>
		public string Name
		{
			get
			{
				return myName;
			}
		}
	}
	#endregion // NameUsageIdentifierAttribute class
	#region NameConsumer Class
	partial class NameConsumer
	{

		/// <summary>
		/// Iterates through the domain and finds the specific name consumer using it's <see cref="NameConsumerIdentifierAttribute"/>
		/// </summary>
		/// <param name="store"></param>
		/// <param name="identifier">String representing the NameConsumerIdentifierAttribute of the desired NameConsumer</param>
		/// <returns></returns>
		public static DomainClassInfo TranslateFromConsumerIdentifier(Store store, string identifier)
		{
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDir.FindDomainClass(NameConsumer.DomainClassId);
			DomainClassInfo retVal = null;
			string[] identifierPieces = identifier.Split(new char[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < identifierPieces.Length; ++i)
			{
				IList<DomainClassInfo> childInfos = classInfo.LocalDescendants;
				int count = childInfos.Count;
				int j = 0;

				string identifierPiece = identifierPieces[i];


				for (; j < count; ++j)
				{
					bool match = false;

					DomainClassInfo childInfo = childInfos[j];
					if (childInfo.Name == identifierPiece)
					{
						match = true;
					}
					else
					{
						object[] customAttributes = childInfo.ImplementationClass.GetCustomAttributes(typeof(NameConsumerIdentifierAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							match = ((NameConsumerIdentifierAttribute)customAttributes[0]).Name == identifierPiece;
						}
					}

					if (match)
					{
						classInfo = childInfo;
						retVal = classInfo;
						break;
					}
				}
				if (j == count)
				{
					return null;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Iterates through the <see cref="NameConsumerIdentifierAttribute"/> related to a specific <see cref="DomainClassInfo"/> 
		/// and generates a fully qualified <see cref="NameConsumer"/>
		/// Does the opposite of <see cref="TranslateFromConsumerIdentifier"/>.
		/// </summary>
		/// <param name="classInfo"><see cref="DomainClassInfo"/></param>
		/// <returns></returns>
		public static string TranslateToConsumerIdentifier(DomainClassInfo classInfo)
		{
			string retVal = "";

			bool seenNameConsumer = false;
			while (classInfo != null && classInfo.Id != ModelElement.DomainClassId && !(seenNameConsumer = classInfo.Id == NameConsumer.DomainClassId))
			{
				object[] baseCustomAttributes = classInfo.ImplementationClass.GetCustomAttributes(typeof(NameConsumerIdentifierAttribute), false);
				string useName = classInfo.Name;
				if (baseCustomAttributes != null && baseCustomAttributes.Length != 0)
				{
					useName =  ((NameConsumerIdentifierAttribute)baseCustomAttributes[0]).Name;
				}
				retVal = (retVal.Length == 0) ? useName : (useName + "." + retVal);
				classInfo = classInfo.BaseDomainClass;
			}
			return seenNameConsumer ? retVal : "";
		}
	}

	#endregion // NameConsumer Class
	#region NameUsage Class
	partial class NameUsage
	{

		/// <summary>
		/// Iterates through the domain and finds the specific name usage using it's <see cref="NameUsageIdentifierAttribute"/>
		/// </summary>
		/// <param name="store"></param>
		/// <param name="identifier">String representing the <see cref="NameUsageIdentifierAttribute"/> of the desired NameUsage</param>
		/// <returns></returns>
		public static DomainClassInfo TranslateFromNameUsageIdentifier(Store store, string identifier)
		{
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDir.FindDomainClass(NameUsage.DomainClassId);
			DomainClassInfo retVal = null;
			string[] identifierPieces = identifier.Split(new char[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < identifierPieces.Length; ++i)
			{
				IList<DomainClassInfo> childInfos = classInfo.LocalDescendants;
				int count = childInfos.Count;
				int j = 0;

				string identifierPiece = identifierPieces[i];


				for (; j < count; ++j)
				{
					bool match = false;

					DomainClassInfo childInfo = childInfos[j];
					if (childInfo.Name == identifierPiece)
					{
						match = true;
					}
					else
					{
						object[] customAttributes = childInfo.ImplementationClass.GetCustomAttributes(typeof(NameUsageIdentifierAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							match = ((NameUsageIdentifierAttribute)customAttributes[0]).Name == identifierPiece;
						}
					}

					if (match)
					{
						classInfo = childInfo;
						retVal = classInfo;
						break;
					}
				}
				if (j == count)
				{
					return null;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Iterates through the <see cref="NameUsageIdentifierAttribute"/> related to a specific <see cref="DomainClassInfo"/> 
		/// and generates a fully qualified <see cref="NameUsage"/>
		/// Does the opposite of <see cref="TranslateFromNameUsageIdentifier"/>.
		/// </summary>
		/// <param name="classInfo"><see cref="DomainClassInfo"/></param>
		/// <returns></returns>
		public static string TranslateToNameUsageIdentifier(DomainClassInfo classInfo)
		{
			string retVal = "";

			bool seenNameConsumer = false;
			while (classInfo != null && classInfo.Id != ModelElement.DomainClassId && !(seenNameConsumer = classInfo.Id == NameUsage.DomainClassId))
			{
				object[] baseCustomAttributes = classInfo.ImplementationClass.GetCustomAttributes(typeof(NameUsageIdentifierAttribute), false);
				string useName = classInfo.Name;
				if (baseCustomAttributes != null && baseCustomAttributes.Length != 0)
				{
					useName = ((NameUsageIdentifierAttribute)baseCustomAttributes[0]).Name;
				}
				retVal = (retVal.Length == 0) ? useName : (useName + "." + retVal);
				classInfo = classInfo.BaseDomainClass;
			}
			return seenNameConsumer ? retVal : "";
		}
	}

	#endregion // NameUsage Class
	#region NameAliasOwnerCreationInfoAttribute class
	/// <summary>
	/// Delegate used with the <see cref="NameAliasOwnerCreationInfoAttribute"/> to find
	/// existing alias owners in an existing container element.
	/// </summary>
	/// <param name="container">The singleton container element</param>
	/// <param name="ownerName">The name of a potentially existing alias owner element</param>
	/// <returns>The existing model element</returns>
	public delegate ModelElement GetExistingAliasOwner(ModelElement container, string ownerName);
	/// <summary>
	/// Provide custom creation information for <see cref="NameAlias"/> owner classes.
	/// The primary function of this attribute is to allow UI components to automatically
	/// create a new instance of an element that exists solely to represent a name that
	/// can in turn have aliases associated with it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class NameAliasOwnerCreationInfoAttribute : Attribute
	{
		private Guid myAutoCreateRelationshipRole;
		private bool myAllowEmptyAlias;
		private string myExistingAliasOwnerCallbackName;
		private GetExistingAliasOwner myExistingAliasCallback;

		/// <summary>
		/// Create a new <see cref="NameAliasOwnerCreationInfoAttribute"/>
		/// </summary>
		/// <param name="allowEmptyAlias">Set to <see langword="true"/> if an alias
		/// with empty content should be allowed for this owner relationship. If this
		/// is not set, then clearing the alias name will automatically delete the alias.</param>
		public NameAliasOwnerCreationInfoAttribute(bool allowEmptyAlias)
		{
			myAllowEmptyAlias = allowEmptyAlias;
		}

		/// <summary>
		/// Create a new <see cref="NameAliasOwnerCreationInfoAttribute"/>
		/// </summary>
		/// <param name="allowEmptyAlias">Set to <see langword="true"/> if an alias
		/// with empty content should be allowed for this owner relationship. If this
		/// is not set, then clearing the alias name will automatically delete the alias.</param>
		/// <param name="autoCreateRelationshipRole">The string form of a <see cref="Guid"/> representing
		/// the role played by the alias owner in an aggregating relationship with a singleton container. If this
		/// is set, then the assumption is made that the element has a formal name.</param>
		/// <param name="getExistingAliasOwnerCallbackName">The name of a static method matching the <see cref="GetExistingAliasOwner"/>
		/// signature that is implemented on the singleton container type.</param>
		public NameAliasOwnerCreationInfoAttribute(bool allowEmptyAlias, string autoCreateRelationshipRole, string getExistingAliasOwnerCallbackName)
		{
			myAllowEmptyAlias = allowEmptyAlias;
			if (!string.IsNullOrEmpty(autoCreateRelationshipRole))
			{
				myAutoCreateRelationshipRole = new Guid(autoCreateRelationshipRole);
			}
			myExistingAliasOwnerCallbackName = getExistingAliasOwnerCallbackName;
		}

		/// <summary>
		/// A <see cref="Guid"/> representing the role played by the alias owner in an aggregating relationship
		/// with a singleton container. If this is set, then the assumption is made that the element has a formal
		/// name.
		/// </summary>
		public Guid AutoCreateRelationshipRole
		{
			get { return myAutoCreateRelationshipRole; }
		}
		/// <summary>
		/// Does the <see cref="AutoCreateRelationshipRole"/> have any data?
		/// </summary>
		public bool HasAutoCreateRelationshipRole
		{
			get { return myAutoCreateRelationshipRole != Guid.Empty; }
		}
		/// <summary>
		/// Set to <see langword="true"/> if an alias
		/// with empty content should be allowed for this owner relationship. If this
		/// is not set, then clearing the alias name will automatically delete the alias.
		/// </summary>
		public bool AllowEmptyAlias
		{
			get { return myAllowEmptyAlias; }
		}
		/// <summary>
		/// Return an existing alias owner of the provided name. The container element
		/// is identified based on information in the <see cref="AutoCreateRelationshipRole"/>
		/// </summary>
		/// <param name="containerElement">A container instance.</param>
		/// <param name="aliasOwnerName">The name of an existing element to locate</param>
		/// <returns>An existing alias. Can return <see langword="null"/></returns>
		public ModelElement GetExistingAliasOwner(ModelElement containerElement, string aliasOwnerName)
		{
			GetExistingAliasOwner callback = myExistingAliasCallback;
			if (callback == null && !string.IsNullOrEmpty(myExistingAliasOwnerCallbackName))
			{
				MethodInfo methodInfo = containerElement.GetType().GetMethod(myExistingAliasOwnerCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]{typeof(ModelElement), typeof(string)}, null);
				if (methodInfo != null)
				{
					System.Threading.Interlocked.CompareExchange(ref myExistingAliasCallback, (GetExistingAliasOwner)Delegate.CreateDelegate(typeof(GetExistingAliasOwner), methodInfo), null);
					callback = myExistingAliasCallback;
				}
			}
			if (callback == null)
			{
				return null;
			}
			return callback(containerElement, aliasOwnerName);
		}
	}
	#endregion // NameAliasOwnerCreationInfoAttribute class
	#region NameUsageAttribute class
	/// <summary>
	/// <see cref=" NameUsageAttribute"/> to define the <see cref="NameConsumer"/> usage.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class NameUsageAttribute : Attribute
	{
		private Type myType;

		/// <summary>
		/// Create a new <see cref=" NameUsageAttribute"/> to define the <see cref="NameConsumer"/> usage.
		/// </summary>
		/// <param name="type">type used to identify the type of the <see cref="NameUsageAttribute"/>. The type must
		/// derive directly from <see cref="NameUsage"/></param>
		/// <exception cref="ArgumentException"/>
		public NameUsageAttribute(Type type)
		{
			if (type.BaseType != typeof(NameUsage))
			{
				throw new ArgumentException("", "type");
			}
			myType = type;
		}

		/// <summary>
		/// Type that the <see cref="NameUsageAttribute"/> identifies
		/// </summary>
		public Type Type
		{
			get
			{
				return myType;
			}
		}
	}
	#endregion // NameUsageAttribute class
	#region RecognizedPhrase class
	partial class RecognizedPhrase
	{
		/// <summary>
		/// DeleteRule: typeof(RecognizedPhraseHasAbbreviation), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Delete a <see cref="RecognizedPhrase"/> when the last abbreviation for it is deleted
		/// </summary>
		private static void RecognizedPhraseHasAbbreviationDeletedRule(ElementDeletedEventArgs e)
		{
			RecognizedPhrase phrase = ((RecognizedPhraseHasAbbreviation)e.ModelElement).RecognizedPhrase;
			if (!phrase.IsDeleted)
			{
				if (phrase.AbbreviationCollection.Count == 0)
				{
					phrase.Delete();
				}
			}
		}
	}
	#endregion // RecognizedPhrase class
}

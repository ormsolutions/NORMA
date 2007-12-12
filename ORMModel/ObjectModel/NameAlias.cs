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
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Neumont.Tools.Modeling;
using System.Text;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel
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
				Type consumer;
				Type usage;
				object[] attributes = null;
				DomainClassInfo consumerDomainClass;
				if (null == (consumerDomainClass = element.myConsumerDomainClass) ||
					null == (consumer = consumerDomainClass.ImplementationClass) ||
					(null != (usage = element.NameUsageType) &&
					null == (attributes = consumer.GetCustomAttributes(typeof(NameUsageAttribute), true))))
				{
					element.Delete();
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
						element.Delete();
					}
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
}

using System;
using Microsoft.VisualStudio.Modeling;
using System.Collections.Generic;
using Neumont.Tools.Modeling.Shell;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Contains information about an ORM extension.
	/// </summary>
	public struct ORMExtensionType : IEquatable<ORMExtensionType>, IComparable<ORMExtensionType>
	{
		private readonly string myNamespaceUri;
		private readonly Type myType;
		private readonly ICollection<Guid> myExtendsIds;
		private readonly Guid myDomainModelId;
		private readonly bool myIsSecondary;
		private readonly bool myIsAutoLoad;
		/// <summary>
		/// Initializes a new instance of <see cref="ORMExtensionType"/>.
		/// </summary>
		/// <param name="namespaceUri">The XML namespace URI of the <see cref="ORMExtensionType"/>.</param>
		/// <param name="type">The <see cref="Type"/> of the <see cref="ORMExtensionType"/>.</param>
		/// <param name="isSecondary">The extension is secondary, meaning that it is not visible
		/// in the Extension Manager dialog and automatically turned off by the Extension Manager when all
		/// non-secondary extensions that use it are turned off.</param>
		/// <param name="isAutoLoad">True if the extension is always loaded. Generally used for extensions
		/// that contribute services but not elements.</param>
		public ORMExtensionType(string namespaceUri, Type type, bool isSecondary, bool isAutoLoad)
		{
			// Verify that if the domain serializes elements, then it serializes this one
			if (!isAutoLoad)
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
						this = default(ORMExtensionType);
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
			myIsSecondary = isSecondary;
			myIsAutoLoad = isAutoLoad;
		}
		/// <summary>
		/// The XML namespace URI of this <see cref="ORMExtensionType"/>.
		/// </summary>
		public string NamespaceUri
		{
			get
			{
				return this.myNamespaceUri;
			}
		}
		/// <summary>
		/// The <see cref="Type"/> of this <see cref="ORMExtensionType"/>.
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
				return myIsSecondary;
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
				return myIsAutoLoad;
			}
		}
		/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
		public override bool Equals(object obj)
		{
			return (obj is ORMExtensionType) && this.Equals((ORMExtensionType)obj);
		}
		/// <summary>See <see cref="Object.GetHashCode"/>.</summary>
		public override int GetHashCode()
		{
			return this.myNamespaceUri.GetHashCode();
		}
		/// <summary>
		/// Checks if two ORMExtensionType's are equals.
		/// </summary>
		/// <param name="other">the namespace to compare.</param>
		/// <returns>true if they equal false if they don't.</returns>
		public bool Equals(ORMExtensionType other)
		{
			return this.myNamespaceUri.Equals(other.myNamespaceUri);
		}
		/// <summary>
		/// Compares the two namespaceURI's.
		/// </summary>
		/// <param name="other">The <see cref="ORMExtensionType"/> you would like to compare.</param>
		/// <returns>standard compare logic.</returns>
		public int CompareTo(ORMExtensionType other)
		{
			return this.myNamespaceUri.CompareTo(other.myNamespaceUri);
		}
	}
}

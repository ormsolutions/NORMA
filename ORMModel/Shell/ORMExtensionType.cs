using System;
using System.Collections.Generic;
using System.Text;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Contains information about an ORM extension.
	/// </summary>
	public struct ORMExtensionType : IEquatable<ORMExtensionType>, IComparable<ORMExtensionType>
	{
		private readonly string myNamespaceUri;
		private readonly Type myType;
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMExtensionType"/>.
		/// </summary>
		/// <param name="namespaceUri">The XML namespace URI of the <see cref="ORMExtensionType"/>.</param>
		/// <param name="type">The <see cref="Type"/> of the <see cref="ORMExtensionType"/>.</param>
		public ORMExtensionType(string namespaceUri, Type type)
		{
			this.myNamespaceUri = namespaceUri;
			this.myType = type;
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
		/// Checks if two ORMExtensionType's are equals.
		/// </summary>
		/// <param name="other">the namespace to compare.</param>
		/// <returns>true if they equal false if they don't.</returns>
		public bool Equals(ORMExtensionType other)
		{
			return this.NamespaceUri.Equals(other.NamespaceUri);
		}
		/// <summary>
		/// Compares the two namespaceURI's.
		/// </summary>
		/// <param name="other">The <see cref="ORMExtensionType"/> you would like to compare.</param>
		/// <returns>standard compare logic.</returns>
		public int CompareTo(ORMExtensionType other)
		{
			return this.NamespaceUri.CompareTo(other.NamespaceUri);
		}
	}
}

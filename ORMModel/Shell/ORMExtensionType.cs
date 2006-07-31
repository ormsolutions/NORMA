using System;

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
		/// Initializes a new instance of <see cref="ORMExtensionType"/>.
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

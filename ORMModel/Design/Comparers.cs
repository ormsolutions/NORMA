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
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

using IEqualityComparer = System.Collections.IEqualityComparer;

namespace Neumont.Tools.ORM.Design
{
	#region HashCodeComparer class
	/// <summary>
	/// <see cref="Object.GetHashCode">HashCode</see>-based <see cref="Comparer{T}"/> implementation.
	/// </summary>
	/// <remarks>
	/// Since equality tests in this class are based on the <see cref="Object.GetHashCode">HashCode</see>
	/// alone, two instances of type <typeparamref name="T"/> will be considered equal if they have the same
	/// <see cref="Object.GetHashCode">HashCode</see>.
	/// </remarks>
	public sealed class HashCodeComparer<T> : Comparer<T>, IEqualityComparer<T>, IEqualityComparer, IEquatable<HashCodeComparer<T>>
	{
		private HashCodeComparer() : base()
		{
		}
		/// <summary>
		/// The singleton instance of <see cref="HashCodeComparer{T}"/>.
		/// </summary>
		public static readonly HashCodeComparer<T> Instance = new HashCodeComparer<T>();
		/// <summary>See <see cref="Comparer{T}.Compare"/>.</summary>
		public sealed override int Compare(T x, T y)
		{
			if (x == null)
			{
				return (y == null) ? 0 : -1;
			}
			else if (y == null)
			{
				return 1;
			}
			else
			{
				return x.GetHashCode().CompareTo(y.GetHashCode());
			}
		}
		/// <summary>See <see cref="IEqualityComparer{T}.Equals"/>.</summary>
		public bool Equals(T x, T y)
		{
			if (x == null)
			{
				return (y == null);
			}
			else if (y == null)
			{
				return false;
			}
			else
			{
				return x.GetHashCode() == y.GetHashCode();
			}
		}
		/// <summary>See <see cref="IEqualityComparer.Equals"/>.</summary>
		bool IEqualityComparer.Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			else if (x == null || y == null)
			{
				return false;
			}
			else
			{
				return ((T)x).GetHashCode() == ((T)y).GetHashCode();
			}
		}
		/// <summary>See <see cref="IEqualityComparer{T}.GetHashCode"/>.</summary>
		public int GetHashCode(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return obj.GetHashCode();
		}
		/// <summary>See <see cref="IEqualityComparer.GetHashCode"/>.</summary>
		int IEqualityComparer.GetHashCode(Object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return ((T)obj).GetHashCode();
		}
		/// <summary>See <see cref="Object.GetHashCode"/>.</summary>
		public sealed override int GetHashCode()
		{
			return 0;
		}
		/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
		public sealed override bool Equals(object obj)
		{
			return this == obj;
		}
		/// <summary>See <see cref="IEquatable{T}.Equals"/>.</summary>
		public bool Equals(HashCodeComparer<T> other)
		{
			return this == other;
		}
	}
	#endregion // HashCodeComparer class

	#region NamedElementComparer class
	/// <summary>
	/// Name-based <see cref="Comparer{TModelElement}"/> implementation for <see cref="ModelElement"/> instances.
	/// </summary>
	/// <remarks>
	/// Since equality tests in this class are based on the Name alone, two <see cref="ModelElement"/>
	/// instances will be considered equal if they have the same Name.
	/// </remarks>
	public sealed class NamedElementComparer<TModelElement> : Comparer<TModelElement>, IEqualityComparer<TModelElement>, IEqualityComparer, IEquatable<NamedElementComparer<TModelElement>>
		where TModelElement : ModelElement
	{
		private readonly StringComparer myStringComparer;
		/// <summary>
		/// Instantiates a new instance of <see cref="NamedElementComparer{TModelElement}"/> that uses the
		/// <see cref="StringComparer"/> specified by <paramref name="stringComparer"/> to
		/// compare Names.
		/// </summary>
		public NamedElementComparer(StringComparer stringComparer) : base()
		{
			if (stringComparer == null)
			{
				throw new ArgumentNullException("stringComparer");
			}
			myStringComparer = stringComparer;
		}
		/// <summary>See <see cref="Comparer{TModelElement}.Compare"/>.</summary>
		public sealed override int Compare(TModelElement x, TModelElement y)
		{
			if (x == y)
			{
				return 0;
			}
			else if (x == null)
			{
				return -1;
			}
			else if (y == null)
			{
				return 1;
			}
			else
			{
				return myStringComparer.Compare(DomainClassInfo.GetName(x), DomainClassInfo.GetName(y));
			}
		}
		/// <summary>See <see cref="IEqualityComparer{TModelElement}.Equals"/>.</summary>
		public bool Equals(TModelElement x, TModelElement y)
		{
			if (x == y)
			{
				return true;
			}
			else if (x == null || y == null)
			{
				return false;
			}
			else
			{
				return myStringComparer.Equals(DomainClassInfo.GetName(x), DomainClassInfo.GetName(y));
			}
		}
		/// <summary>See <see cref="IEqualityComparer.Equals"/>.</summary>
		bool IEqualityComparer.Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			else if (x == null || y == null)
			{
				return false;
			}
			else
			{
				return myStringComparer.Equals(DomainClassInfo.GetName((TModelElement)x), DomainClassInfo.GetName((TModelElement)y));
			}
		}
		/// <summary>See <see cref="IEqualityComparer{TModelElement}.GetHashCode"/>.</summary>
		public int GetHashCode(TModelElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return myStringComparer.GetHashCode(DomainClassInfo.GetName(obj));
		}
		/// <summary>See <see cref="IEqualityComparer.GetHashCode"/>.</summary>
		int IEqualityComparer.GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return myStringComparer.GetHashCode(DomainClassInfo.GetName((TModelElement)obj));
		}
		/// <summary>See <see cref="Object.GetHashCode"/>.</summary>
		public sealed override int GetHashCode()
		{
			return myStringComparer.GetHashCode();
		}
		/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
		public sealed override bool Equals(object obj)
		{
			return this.Equals(obj as NamedElementComparer<TModelElement>);
		}
		/// <summary>See <see cref="IEquatable{TModelElement}.Equals"/>.</summary>
		public bool Equals(NamedElementComparer<TModelElement> other)
		{
			return other != null && myStringComparer.Equals(other.myStringComparer);
		}

		/// <summary>
		/// Returns an instance of <see cref="NamedElementComparer{TModelElement}"/> initialized with <see cref="StringComparer.CurrentCulture"/>.
		/// </summary>
		public static NamedElementComparer<TModelElement> CurrentCulture
		{
			get
			{
				return new NamedElementComparer<TModelElement>(StringComparer.CurrentCulture);
			}
		}
	}
	#endregion // NamedElementComparer class
}

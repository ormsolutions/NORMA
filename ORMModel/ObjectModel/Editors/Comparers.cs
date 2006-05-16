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
namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	#region HashCodeComparer class
	/// <summary>
	/// <see cref="Object.GetHashCode">HashCode</see>-based <see cref="Comparer{T}"/> implementation.
	/// </summary>
	public sealed class HashCodeComparer<T> : Comparer<T>, IEqualityComparer<T>, IEquatable<HashCodeComparer<T>>
	{
		private HashCodeComparer()
		{
		}
		/// <summary>
		/// The singleton instance of <see cref="HashCodeComparer{T}"/>.
		/// </summary>
		public static readonly HashCodeComparer<T> Instance = new HashCodeComparer<T>();
		/// <summary><see cref="Comparer{T}.Compare"/></summary>
		public override int Compare(T x, T y)
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
		/// <summary><see cref="IEqualityComparer{T}.Equals"/></summary>
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
		/// <summary><see cref="IEqualityComparer{T}.GetHashCode"/></summary>
		public int GetHashCode(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return obj.GetHashCode();
		}
		/// <summary><see cref="Object.GetHashCode"/></summary>
		public override int GetHashCode()
		{
			return 0;
		}
		/// <summary><see cref="Object.Equals(Object)"/></summary>
		public override bool Equals(object obj)
		{
			return this == obj;
		}
		/// <summary><see cref="IEquatable{T}.Equals"/></summary>
		public bool Equals(HashCodeComparer<T> other)
		{
			return this == other;
		}
	}
	#endregion // HashCodeComparer class
	#region NamedElementComparer class
	/// <summary>
	/// <see cref="NamedElement.Name">Name</see>-based <see cref="Comparer{T}"/> implementation
	/// for <see cref="NamedElement"/> instances.
	/// </summary>
	/// <remarks>
	/// Since equality tests in this class are based on the <see cref="NamedElement.Name">Name</see> alone,
	/// two <see cref="NamedElement"/> instances will be considered equal if they have the same
	/// <see cref="NamedElement.Name">Name</see>.
	/// </remarks>
	public sealed class NamedElementComparer<T> : Comparer<T>, IEqualityComparer<T>, IEquatable<NamedElementComparer<T>>
		where T : NamedElement
	{
		private readonly StringComparer myStringComparer;
		/// <summary>
		/// Instantiates a new instance of <see cref="NamedElementComparer{T}"/> that uses the
		/// specified <see cref="StringComparer"/> to compare <see cref="NamedElement.Name"/>s.
		/// </summary>
		public NamedElementComparer(StringComparer stringComparer)
		{
			if ((object)stringComparer == null)
			{
				throw new ArgumentNullException("stringComparer");
			}
			myStringComparer = stringComparer;
		}
		/// <summary><see cref="Comparer{T}.Compare"/></summary>
		public override int Compare(T x, T y)
		{
			if ((object)x == y)
			{
				return 0;
			}
			else if ((object)x == null)
			{
				return -1;
			}
			else if ((object)y == null)
			{
				return 1;
			}
			else
			{
				return myStringComparer.Compare(x.Name, y.Name);
			}
		}
		/// <summary><see cref="IEqualityComparer{T}.Equals"/></summary>
		public bool Equals(T x, T y)
		{
			if ((object)x == y)
			{
				return true;
			}
			else if ((object)x == null || (object)y == null)
			{
				return false;
			}
			else
			{
				return myStringComparer.Equals(x, y);
			}
		}
		/// <summary><see cref="IEqualityComparer{T}.GetHashCode"/></summary>
		public int GetHashCode(T obj)
		{
			if ((object)obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return myStringComparer.GetHashCode(obj.Name);
		}
		/// <summary><see cref="Object.GetHashCode"/></summary>
		public override int GetHashCode()
		{
			return myStringComparer.GetHashCode();
		}
		/// <summary><see cref="Object.Equals(Object)"/></summary>
		public override bool Equals(object obj)
		{
			return Equals(obj as NamedElementComparer<T>);
		}
		/// <summary><see cref="IEquatable{T}.Equals"/></summary>
		public bool Equals(NamedElementComparer<T> other)
		{
			return (object)other != null && myStringComparer.Equals(other.myStringComparer);
		}

		/// <summary>
		/// Returns an instance of <see cref="NamedElementComparer{T}"/> initialized with <see cref="StringComparer.CurrentCulture"/>.
		/// </summary>
		public static NamedElementComparer<T> CurrentCulture
		{
			get
			{
				return new NamedElementComparer<T>(StringComparer.CurrentCulture);
			}
		}
	}
	#endregion // NamedElementComparer class
}

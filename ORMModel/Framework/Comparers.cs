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
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;

using IComparer = System.Collections.IComparer;
using IEqualityComparer = System.Collections.IEqualityComparer;

namespace Neumont.Tools.Modeling
{
	#region RuntimeTypeHandleComparer class
	/// <summary>
	/// <see cref="RuntimeTypeHandle.Value">Value</see>-based <see cref="IComparer{T}"/> and
	/// <see cref="IEqualityComparer{T}"/> implementation for <see cref="RuntimeTypeHandle"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="RuntimeTypeHandle"/> has a strongly-typed <c>Equals</c> method (<see cref="RuntimeTypeHandle.Equals(RuntimeTypeHandle)"/>),
	/// but does not implement <see cref="IEquatable{RuntimeTypeHandle}"/>. Therefore, this <see cref="IEqualityComparer{RuntimeTypeHandle}"/>
	/// implementation (which is based on <see cref="RuntimeTypeHandle.Value"/>) should be used in order to avoid boxing.
	/// </remarks>
	[Serializable]
	public sealed class RuntimeTypeHandleComparer : EqualityComparer<RuntimeTypeHandle>, IComparer<RuntimeTypeHandle>, IComparer, IEquatable<RuntimeTypeHandleComparer>, ISerializable
	{
		private RuntimeTypeHandleComparer()
			: base()
		{
		}
		/// <summary>
		/// The singleton instance of <see cref="RuntimeTypeHandleComparer"/>.
		/// </summary>
		public static readonly RuntimeTypeHandleComparer Instance = new RuntimeTypeHandleComparer();

		#region Serialization support
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.SetType(typeof(SerializationHelper));
		}
		[Serializable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private sealed class SerializationHelper : IObjectReference
		{
			private SerializationHelper()
				: base()
			{
			}
			public object GetRealObject(StreamingContext context)
			{
				return Instance;
			}
		}
		#endregion // Serialization support

		/// <summary>
		/// Use the <see cref="Instance"/> field to retrieve the singleton instance of
		/// <see cref="RuntimeTypeHandleComparer"/>.
		/// </summary>
		/// <remarks>
		/// This is here mainly to hide the <see cref="EqualityComparer{T}.Default"/> property
		/// so that it is not used by mistake.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static new EqualityComparer<RuntimeTypeHandle> Default
		{
			get
			{
				return Instance;
			}
		}

		[Serializable]
		private sealed class InnerComparer : Comparer<RuntimeTypeHandle>
		{
			private InnerComparer()
				: base()
			{
			}
			public static readonly InnerComparer Instance = new InnerComparer();
			public sealed override int Compare(RuntimeTypeHandle x, RuntimeTypeHandle y)
			{
				return ((long)x.Value).CompareTo((long)y.Value);
			}
		}

		/// <summary>See <see cref="Comparer{T}.Compare"/>.</summary>
		public int Compare(RuntimeTypeHandle x, RuntimeTypeHandle y)
		{
			return ((long)x.Value).CompareTo((long)y.Value);
		}
		int IComparer.Compare(object x, object y)
		{
			return ((IComparer)InnerComparer.Instance).Compare(x, y);
		}
		/// <summary>See <see cref="IEqualityComparer{T}.Equals"/>.</summary>
		public sealed override bool Equals(RuntimeTypeHandle x, RuntimeTypeHandle y)
		{
			return x.Value == y.Value;
		}
		/// <summary>See <see cref="IEqualityComparer{T}.GetHashCode"/>.</summary>
		public sealed override int GetHashCode(RuntimeTypeHandle obj)
		{
			return obj.GetHashCode();
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
		public bool Equals(RuntimeTypeHandleComparer other)
		{
			return this == other;
		}
	}
	#endregion // RuntimeTypeHandleComparer class

	#region RuntimeMethodHandleComparer class
	/// <summary>
	/// <see cref="RuntimeMethodHandle.Value">Value</see>-based <see cref="IComparer{T}"/> and
	/// <see cref="IEqualityComparer{T}"/> implementation for <see cref="RuntimeMethodHandle"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="RuntimeMethodHandle"/> has a strongly-typed <c>Equals</c> method (<see cref="RuntimeMethodHandle.Equals(RuntimeMethodHandle)"/>),
	/// but does not implement <see cref="IEquatable{RuntimeMethodHandle}"/>. Therefore, this <see cref="IEqualityComparer{RuntimeMethodHandle}"/>
	/// implementation (which is based on <see cref="RuntimeMethodHandle.Value"/>) should be used in order to avoid boxing.
	/// </remarks>
	[Serializable]
	public sealed class RuntimeMethodHandleComparer : EqualityComparer<RuntimeMethodHandle>, IComparer<RuntimeMethodHandle>, IComparer, IEquatable<RuntimeMethodHandleComparer>, ISerializable
	{
		private RuntimeMethodHandleComparer()
			: base()
		{
		}
		/// <summary>
		/// The singleton instance of <see cref="RuntimeMethodHandleComparer"/>.
		/// </summary>
		public static readonly RuntimeMethodHandleComparer Instance = new RuntimeMethodHandleComparer();

		#region Serialization support
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.SetType(typeof(SerializationHelper));
		}
		[Serializable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private sealed class SerializationHelper : IObjectReference
		{
			private SerializationHelper()
				: base()
			{
			}
			public object GetRealObject(StreamingContext context)
			{
				return Instance;
			}
		}
		#endregion // Serialization support

		/// <summary>
		/// Use the <see cref="Instance"/> field to retrieve the singleton instance of
		/// <see cref="RuntimeMethodHandleComparer"/>.
		/// </summary>
		/// <remarks>
		/// This is here mainly to hide the <see cref="EqualityComparer{T}.Default"/> property
		/// so that it is not used by mistake.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static new EqualityComparer<RuntimeMethodHandle> Default
		{
			get
			{
				return Instance;
			}
		}

		[Serializable]
		private sealed class InnerComparer : Comparer<RuntimeMethodHandle>
		{
			private InnerComparer()
				: base()
			{
			}
			public static readonly InnerComparer Instance = new InnerComparer();
			public sealed override int Compare(RuntimeMethodHandle x, RuntimeMethodHandle y)
			{
				return ((long)x.Value).CompareTo((long)y.Value);
			}
		}

		/// <summary>See <see cref="Comparer{T}.Compare"/>.</summary>
		public int Compare(RuntimeMethodHandle x, RuntimeMethodHandle y)
		{
			return ((long)x.Value).CompareTo((long)y.Value);
		}
		int IComparer.Compare(object x, object y)
		{
			return ((IComparer)InnerComparer.Instance).Compare(x, y);
		}
		/// <summary>See <see cref="IEqualityComparer{T}.Equals"/>.</summary>
		public sealed override bool Equals(RuntimeMethodHandle x, RuntimeMethodHandle y)
		{
			return x.Value == y.Value;
		}
		/// <summary>See <see cref="IEqualityComparer{T}.GetHashCode"/>.</summary>
		public sealed override int GetHashCode(RuntimeMethodHandle obj)
		{
			return obj.GetHashCode();
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
		public bool Equals(RuntimeMethodHandleComparer other)
		{
			return this == other;
		}
	}
	#endregion // RuntimeMethodHandleComparer class

	#region RuntimeFieldHandleComparer class
	/// <summary>
	/// <see cref="RuntimeFieldHandle.Value">Value</see>-based <see cref="IComparer{T}"/> and
	/// <see cref="IEqualityComparer{T}"/> implementation for <see cref="RuntimeFieldHandle"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="RuntimeFieldHandle"/> has a strongly-typed <c>Equals</c> method (<see cref="RuntimeFieldHandle.Equals(RuntimeFieldHandle)"/>),
	/// but does not implement <see cref="IEquatable{RuntimeFieldHandle}"/>. Therefore, this <see cref="IEqualityComparer{RuntimeFieldHandle}"/>
	/// implementation (which is based on <see cref="RuntimeFieldHandle.Value"/>) should be used in order to avoid boxing.
	/// </remarks>
	[Serializable]
	public sealed class RuntimeFieldHandleComparer : EqualityComparer<RuntimeFieldHandle>, IComparer<RuntimeFieldHandle>, IComparer, IEquatable<RuntimeFieldHandleComparer>, ISerializable
	{
		private RuntimeFieldHandleComparer()
			: base()
		{
		}
		/// <summary>
		/// The singleton instance of <see cref="RuntimeFieldHandleComparer"/>.
		/// </summary>
		public static readonly RuntimeFieldHandleComparer Instance = new RuntimeFieldHandleComparer();

		#region Serialization support
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.SetType(typeof(SerializationHelper));
		}
		[Serializable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private sealed class SerializationHelper : IObjectReference
		{
			private SerializationHelper()
				: base()
			{
			}
			public object GetRealObject(StreamingContext context)
			{
				return Instance;
			}
		}
		#endregion // Serialization support

		/// <summary>
		/// Use the <see cref="Instance"/> field to retrieve the singleton instance of
		/// <see cref="RuntimeFieldHandleComparer"/>.
		/// </summary>
		/// <remarks>
		/// This is here mainly to hide the <see cref="EqualityComparer{T}.Default"/> property
		/// so that it is not used by mistake.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static new EqualityComparer<RuntimeFieldHandle> Default
		{
			get
			{
				return Instance;
			}
		}

		[Serializable]
		private sealed class InnerComparer : Comparer<RuntimeFieldHandle>
		{
			private InnerComparer()
				: base()
			{
			}
			public static readonly InnerComparer Instance = new InnerComparer();
			public sealed override int Compare(RuntimeFieldHandle x, RuntimeFieldHandle y)
			{
				return ((long)x.Value).CompareTo((long)y.Value);
			}
		}

		/// <summary>See <see cref="Comparer{T}.Compare"/>.</summary>
		public int Compare(RuntimeFieldHandle x, RuntimeFieldHandle y)
		{
			return ((long)x.Value).CompareTo((long)y.Value);
		}
		int IComparer.Compare(object x, object y)
		{
			return ((IComparer)InnerComparer.Instance).Compare(x, y);
		}
		/// <summary>See <see cref="IEqualityComparer{T}.Equals"/>.</summary>
		public sealed override bool Equals(RuntimeFieldHandle x, RuntimeFieldHandle y)
		{
			return x.Value == y.Value;
		}
		/// <summary>See <see cref="IEqualityComparer{T}.GetHashCode"/>.</summary>
		public sealed override int GetHashCode(RuntimeFieldHandle obj)
		{
			return obj.GetHashCode();
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
		public bool Equals(RuntimeFieldHandleComparer other)
		{
			return this == other;
		}
	}
	#endregion // RuntimeFieldHandleComparer class

	#region HashCodeComparer class
	/// <summary>
	/// <see cref="Object.GetHashCode">HashCode</see>-based <see cref="Comparer{T}"/> implementation.
	/// </summary>
	/// <remarks>
	/// Since equality tests in this class are based on the <see cref="Object.GetHashCode">HashCode</see>
	/// alone, two instances of type <typeparamref name="T"/> will be considered equal if they have the same
	/// <see cref="Object.GetHashCode">HashCode</see>.
	/// </remarks>
	[Serializable]
	public sealed class HashCodeComparer<T> : Comparer<T>, IEqualityComparer<T>, IEqualityComparer, IEquatable<HashCodeComparer<T>>, ISerializable
	{
		private HashCodeComparer() : base()
		{
		}
		/// <summary>
		/// The singleton instance of <see cref="HashCodeComparer{T}"/>.
		/// </summary>
		public static readonly HashCodeComparer<T> Instance = new HashCodeComparer<T>();

		#region Serialization support
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.SetType(typeof(SerializationHelper));
		}
		[Serializable]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private sealed class SerializationHelper : IObjectReference
		{
			private SerializationHelper()
				: base()
			{
			}
			public object GetRealObject(StreamingContext context)
			{
				return Instance;
			}
		}
		#endregion // Serialization support

		/// <summary>
		/// Use the <see cref="Instance"/> field to retrieve the singleton instance of
		/// <see cref="HashCodeComparer{T}"/>.
		/// </summary>
		/// <remarks>
		/// This is here mainly to hide the <see cref="Comparer{T}.Default"/> property
		/// so that it is not used by mistake.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static new Comparer<T> Default
		{
			get
			{
				return Instance;
			}
		}

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
	/// <c>Name</c>-based <see cref="Comparer{TModelElement}"/> implementation for <see cref="ModelElement"/> instances.
	/// </summary>
	/// <remarks>
	/// Since equality tests in this class are based on the <c>Name</c> alone, two <see cref="ModelElement"/>
	/// instances will be considered equal if they have the same <c>Name</c>.
	/// </remarks>
	[Serializable]
	public sealed class NamedElementComparer<TModelElement> : Comparer<TModelElement>, IEqualityComparer<TModelElement>, IEqualityComparer, IEquatable<NamedElementComparer<TModelElement>>
		where TModelElement : ModelElement
	{
		private readonly StringComparer myStringComparer;
		/// <summary>
		/// Initializes a new instance of <see cref="NamedElementComparer{TModelElement}"/> that uses the
		/// <see cref="StringComparer"/> specified by <paramref name="stringComparer"/> to
		/// compare <c>Name</c>s.
		/// </summary>
		public NamedElementComparer(StringComparer stringComparer) : base()
		{
			if (stringComparer == null)
			{
				throw new ArgumentNullException("stringComparer");
			}
			myStringComparer = stringComparer;
		}

		/// <summary>
		/// Use the <see cref="CurrentCulture"/> property to retrieve an instance of <see cref="NamedElementComparer{TModelElement}"/>
		/// for <see cref="StringComparer.CurrentCulture"/>, or the <see cref="NamedElementComparer{TModelElement}(StringComparer)"/>
		/// constructor to create an instance of <see cref="NamedElementComparer{TModelElement}"/> for a specified <see cref="StringComparer"/>.
		/// </summary>
		/// <remarks>
		/// This is here mainly to hide the <see cref="Comparer{T}.Default"/> property so that it is not used by mistake.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static new Comparer<TModelElement> Default
		{
			get
			{
				return CurrentCulture;
			}
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
		/// Returns an instance of <see cref="NamedElementComparer{TModelElement}"/> for <see cref="StringComparer.CurrentCulture"/>.
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

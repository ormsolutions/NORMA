using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
using AccessedThroughPropertyAttribute = System.Runtime.CompilerServices.AccessedThroughPropertyAttribute;
using GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute;
using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
using LayoutKind = System.Runtime.InteropServices.LayoutKind;
using CharSet = System.Runtime.InteropServices.CharSet;
#region Global Support Classes
namespace System
{
	#region Tuple Support
	[System.Serializable()]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Tuple
	{
		protected Tuple()
		{
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow")]
		protected static int RotateRight(int value, int places)
		{
			places = places & 0x1F;
			if (places == 0)
			{
				return value;
			}
			int mask = ~0x7FFFFFF >> (places - 1);
			return ((value >> places) & ~mask) | ((value << (32 - places)) & mask);
		}
		public abstract override string ToString();
		public abstract string ToString(System.IFormatProvider provider);
	}
	#endregion // Tuple Support
	#region Binary (2-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2> CreateTuple<T1, T2>(T1 item1, T2 item2)
		{
			if ((item1 == null) || (item2 == null))
			{
				return null;
			}
			return new Tuple<T1, T2>(item1, item2);
		}
	}
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2> : Tuple, System.IEquatable<Tuple<T1, T2>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		public Tuple(T1 item1, T2 item2)
		{
			if ((item1 == null) || (item2 == null))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2>);
		}
		public bool Equals(Tuple<T1, T2> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || !(this._item2.Equals(other._item2))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ Tuple.RotateRight(this._item2.GetHashCode(), 1);
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2})", this._item1, this._item2);
		}
		public static bool operator ==(Tuple<T1, T2> tuple1, Tuple<T1, T2> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2> tuple1, Tuple<T1, T2> tuple2)
		{
			return !(tuple1 == tuple2);
		}
		public static implicit operator System.Collections.Generic.KeyValuePair<T1, T2>(Tuple<T1, T2> tuple)
		{
			if ((object)tuple == null)
			{
				return default(System.Collections.Generic.KeyValuePair<T1, T2>);
			}
			else
			{
				return new System.Collections.Generic.KeyValuePair<T1, T2>(tuple._item1, tuple._item2);
			}
		}
		public static explicit operator Tuple<T1, T2>(System.Collections.Generic.KeyValuePair<T1, T2> keyValuePair)
		{
			if ((keyValuePair.Key == null) || (keyValuePair.Value == null))
			{
				throw new System.InvalidCastException();
			}
			else
			{
				return new Tuple<T1, T2>(keyValuePair.Key, keyValuePair.Value);
			}
		}
		public static implicit operator System.Collections.DictionaryEntry(Tuple<T1, T2> tuple)
		{
			if ((object)tuple == null)
			{
				return default(System.Collections.DictionaryEntry);
			}
			else
			{
				return new System.Collections.DictionaryEntry(tuple._item1, tuple._item2);
			}
		}
		public static explicit operator Tuple<T1, T2>(System.Collections.DictionaryEntry dictionaryEntry)
		{
			object key = dictionaryEntry.Key;
			object value = dictionaryEntry.Value;
			if (((key == null) || (value == null)) || !((key is T1) && (value is T2)))
			{
				throw new System.InvalidCastException();
			}
			else
			{
				return new Tuple<T1, T2>((T1)key, (T2)value);
			}
		}
	}
	#endregion // Binary (2-ary) Tuple
	#region Ternary (3-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3> CreateTuple<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			if ((item1 == null) || ((item2 == null) || (item3 == null)))
			{
				return null;
			}
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3> : Tuple, System.IEquatable<Tuple<T1, T2, T3>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			if ((item1 == null) || ((item2 == null) || (item3 == null)))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3>);
		}
		public bool Equals(Tuple<T1, T2, T3> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || !(this._item3.Equals(other._item3)))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ Tuple.RotateRight(this._item3.GetHashCode(), 2));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3})", this._item1, this._item2, this._item3);
		}
		public static bool operator ==(Tuple<T1, T2, T3> tuple1, Tuple<T1, T2, T3> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3> tuple1, Tuple<T1, T2, T3> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Ternary (3-ary) Tuple
	#region Quaternary (4-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4> CreateTuple<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || (item4 == null))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || (item4 == null))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || !(this._item4.Equals(other._item4))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ Tuple.RotateRight(this._item4.GetHashCode(), 3)));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4})", this._item1, this._item2, this._item3, this._item4);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4> tuple1, Tuple<T1, T2, T3, T4> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4> tuple1, Tuple<T1, T2, T3, T4> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Quaternary (4-ary) Tuple
	#region Quinary (5-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5> CreateTuple<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || (item5 == null)))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4, T5> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		private readonly T5 _item5;
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || (item5 == null)))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || (!(this._item4.Equals(other._item4)) || !(this._item5.Equals(other._item5)))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this._item4.GetHashCode(), 3) ^ Tuple.RotateRight(this._item5.GetHashCode(), 4))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5})", this._item1, this._item2, this._item3, this._item4, this._item5);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5> tuple1, Tuple<T1, T2, T3, T4, T5> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5> tuple1, Tuple<T1, T2, T3, T4, T5> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Quinary (5-ary) Tuple
	#region Senary (6-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5, T6> CreateTuple<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || (item6 == null))))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		private readonly T5 _item5;
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
		private readonly T6 _item6;
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || (item6 == null))))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || (!(this._item4.Equals(other._item4)) || (!(this._item5.Equals(other._item5)) || !(this._item6.Equals(other._item6))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this._item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this._item5.GetHashCode(), 4) ^ Tuple.RotateRight(this._item6.GetHashCode(), 5)))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6})", this._item1, this._item2, this._item3, this._item4, this._item5, this._item6);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6> tuple1, Tuple<T1, T2, T3, T4, T5, T6> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6> tuple1, Tuple<T1, T2, T3, T4, T5, T6> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Senary (6-ary) Tuple
	#region Septenary (7-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5, T6, T7> CreateTuple<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || (item7 == null)))))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		private readonly T5 _item5;
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
		private readonly T6 _item6;
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
		private readonly T7 _item7;
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || (item7 == null)))))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || (!(this._item4.Equals(other._item4)) || (!(this._item5.Equals(other._item5)) || (!(this._item6.Equals(other._item6)) || !(this._item7.Equals(other._item7)))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this._item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this._item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this._item6.GetHashCode(), 5) ^ Tuple.RotateRight(this._item7.GetHashCode(), 6))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7})", this._item1, this._item2, this._item3, this._item4, this._item5, this._item6, this._item7);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Septenary (7-ary) Tuple
	#region Octonary (8-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> CreateTuple<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || (item8 == null))))))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		private readonly T5 _item5;
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
		private readonly T6 _item6;
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
		private readonly T7 _item7;
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
		private readonly T8 _item8;
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || (item8 == null))))))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || (!(this._item4.Equals(other._item4)) || (!(this._item5.Equals(other._item5)) || (!(this._item6.Equals(other._item6)) || (!(this._item7.Equals(other._item7)) || !(this._item8.Equals(other._item8))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this._item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this._item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this._item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this._item7.GetHashCode(), 6) ^ Tuple.RotateRight(this._item8.GetHashCode(), 7)))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", this._item1, this._item2, this._item3, this._item4, this._item5, this._item6, this._item7, this._item8);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Octonary (8-ary) Tuple
	#region Nonary (9-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || (item9 == null)))))))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(item1, item2, item3, item4, item5, item6, item7, item8, item9);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		private readonly T5 _item5;
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
		private readonly T6 _item6;
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
		private readonly T7 _item7;
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
		private readonly T8 _item8;
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
		private readonly T9 _item9;
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || (item9 == null)))))))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || (!(this._item4.Equals(other._item4)) || (!(this._item5.Equals(other._item5)) || (!(this._item6.Equals(other._item6)) || (!(this._item7.Equals(other._item7)) || (!(this._item8.Equals(other._item8)) || !(this._item9.Equals(other._item9)))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this._item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this._item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this._item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this._item7.GetHashCode(), 6) ^ (Tuple.RotateRight(this._item8.GetHashCode(), 7) ^ Tuple.RotateRight(this._item9.GetHashCode(), 8))))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", this._item1, this._item2, this._item3, this._item4, this._item5, this._item6, this._item7, this._item8, this._item9);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Nonary (9-ary) Tuple
	#region Denary (10-ary) Tuple
	public abstract partial class Tuple
	{
		public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || ((item9 == null) || (item10 == null))))))))))
			{
				return null;
			}
			return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10);
		}
	}
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	[System.Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
	{
		private readonly T1 _item1;
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
		private readonly T2 _item2;
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
		private readonly T3 _item3;
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
		private readonly T4 _item4;
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
		private readonly T5 _item5;
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
		private readonly T6 _item6;
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
		private readonly T7 _item7;
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
		private readonly T8 _item8;
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
		private readonly T9 _item9;
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
		private readonly T10 _item10;
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || ((item9 == null) || (item10 == null))))))))))
			{
				throw new System.ArgumentNullException();
			}
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
		{
			if (((object)other == null) || (!(this._item1.Equals(other._item1)) || (!(this._item2.Equals(other._item2)) || (!(this._item3.Equals(other._item3)) || (!(this._item4.Equals(other._item4)) || (!(this._item5.Equals(other._item5)) || (!(this._item6.Equals(other._item6)) || (!(this._item7.Equals(other._item7)) || (!(this._item8.Equals(other._item8)) || (!(this._item9.Equals(other._item9)) || !(this._item10.Equals(other._item10))))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this._item1.GetHashCode() ^ (Tuple.RotateRight(this._item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this._item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this._item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this._item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this._item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this._item7.GetHashCode(), 6) ^ (Tuple.RotateRight(this._item8.GetHashCode(), 7) ^ (Tuple.RotateRight(this._item9.GetHashCode(), 8) ^ Tuple.RotateRight(this._item10.GetHashCode(), 9)))))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public override string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", this._item1, this._item2, this._item3, this._item4, this._item5, this._item6, this._item7, this._item8, this._item9, this._item10);
		}
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple2)
		{
			if ((object)tuple1 == null)
			{
				return (object)tuple2 == null;
			}
			else
			{
				return tuple1.Equals(tuple2);
			}
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // Denary (10-ary) Tuple
	#region Property Change Event Support
	[SuppressMessageAttribute("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public interface IPropertyChangeEventArgs<TClass, TProperty>
	{
		TClass Instance
		{
			get;
		}
		TProperty OldValue
		{
			get;
		}
		TProperty NewValue
		{
			get;
		}
	}
	[Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class PropertyChangingEventArgs<TClass, TProperty> : CancelEventArgs, IPropertyChangeEventArgs<TClass, TProperty>
	{
		private readonly TClass _instance;
		private readonly TProperty _oldValue;
		private readonly TProperty _newValue;
		public PropertyChangingEventArgs(TClass instance, TProperty oldValue, TProperty newValue)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			this._instance = instance;
			this._oldValue = oldValue;
			this._newValue = newValue;
		}
		public TClass Instance
		{
			get
			{
				return this._instance;
			}
		}
		public TProperty OldValue
		{
			get
			{
				return this._oldValue;
			}
		}
		public TProperty NewValue
		{
			get
			{
				return this._newValue;
			}
		}
	}
	[Serializable()]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class PropertyChangedEventArgs<TClass, TProperty> : EventArgs, IPropertyChangeEventArgs<TClass, TProperty>
	{
		private readonly TClass _instance;
		private readonly TProperty _oldValue;
		private readonly TProperty _newValue;
		public PropertyChangedEventArgs(TClass instance, TProperty oldValue, TProperty newValue)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			this._instance = instance;
			this._oldValue = oldValue;
			this._newValue = newValue;
		}
		public TClass Instance
		{
			get
			{
				return this._instance;
			}
		}
		public TProperty OldValue
		{
			get
			{
				return this._oldValue;
			}
		}
		public TProperty NewValue
		{
			get
			{
				return this._newValue;
			}
		}
	}
	#endregion // Property Change Event Support
}
#endregion // Global Support Classes
namespace PersonCountryDemo
{
	#region Person
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Person : INotifyPropertyChanged, IHasPersonCountryDemoContext
	{
		protected Person()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[4];
				}
				return this._events;
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void RaisePropertyChangedEvent(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
			}
		}
		public abstract PersonCountryDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseLastNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.LastName, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseLastNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.LastName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseFirstNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.FirstName, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseFirstNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.FirstName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> TitleChanging
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseTitleChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.Title, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> TitleChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseTitleChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.Title), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Title");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Country>> CountryChanging
		{
			add
			{
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseCountryChangingEvent(Country newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Country>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person, Country>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Country> eventArgs = new PropertyChangingEventArgs<Person, Country>(this, this.Country, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Country>> CountryChanged
		{
			add
			{
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseCountryChangedEvent(Country oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Country>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person, Country>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Country>(this, oldValue, this.Country), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Country");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string LastName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string FirstName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract string Title
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Country Country
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Person{0}{{{0}{1}LastName = ""{2}"",{0}{1}FirstName = ""{3}"",{0}{1}Title = ""{4}"",{0}{1}Country = {5}{0}}}", Environment.NewLine, "", this.LastName, this.FirstName, this.Title, "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // Person
	#region Country
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Country : INotifyPropertyChanged, IHasPersonCountryDemoContext
	{
		protected Country()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[2];
				}
				return this._events;
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void RaisePropertyChangedEvent(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
			}
		}
		public abstract PersonCountryDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Country, string>> Country_nameChanging
		{
			add
			{
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseCountry_nameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Country, string> eventArgs = new PropertyChangingEventArgs<Country, string>(this, this.Country_name, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Country, string>> Country_nameChanged
		{
			add
			{
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseCountry_nameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Country_name), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Country_name");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Country, string>> Region_Region_codeChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseRegion_Region_codeChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Country, string> eventArgs = new PropertyChangingEventArgs<Country, string>(this, this.Region_Region_code, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Country, string>> Region_Region_codeChanged
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseRegion_Region_codeChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Region_Region_code), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Region_Region_code");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string Country_name
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract string Region_Region_code
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<Person> Person
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Country{0}{{{0}{1}Country_name = ""{2}"",{0}{1}Region_Region_code = ""{3}""{0}}}", Environment.NewLine, "", this.Country_name, this.Region_Region_code);
		}
	}
	#endregion // Country
	#region IHasPersonCountryDemoContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface IHasPersonCountryDemoContext
	{
		PersonCountryDemoContext Context
		{
			get;
		}
	}
	#endregion // IHasPersonCountryDemoContext
	#region IPersonCountryDemoContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface IPersonCountryDemoContext
	{
		Country GetCountryByCountry_name(string Country_name);
		bool TryGetCountryByCountry_name(string Country_name, out Country Country);
		Person CreatePerson(string LastName, string FirstName);
		ReadOnlyCollection<Person> PersonCollection
		{
			get;
		}
		Country CreateCountry(string Country_name);
		ReadOnlyCollection<Country> CountryCollection
		{
			get;
		}
	}
	#endregion // IPersonCountryDemoContext
	#region PersonCountryDemoContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class PersonCountryDemoContext : IPersonCountryDemoContext
	{
		public PersonCountryDemoContext()
		{
			Dictionary<Type, object> constraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<Type, object>(1);
			Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> constraintEnforcementCollectionCallbacksByTypeAndNameDictionary = new Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object>(0);
			this._ContraintEnforcementCollectionCallbacksByTypeDictionary = constraintEnforcementCollectionCallbacksByTypeDictionary;
			this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = constraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Country, Person>), new ConstraintEnforcementCollectionCallbacks<Country, Person>(new PotentialCollectionModificationCallback<Country, Person>(this.OnCountryPersonAdding), new CommittedCollectionModificationCallback<Country, Person>(this.OnCountryPersonAdded), null, new CommittedCollectionModificationCallback<Country, Person>(this.OnCountryPersonRemoved)));
			List<Person> PersonList = new List<Person>();
			this._PersonList = PersonList;
			this._PersonReadOnlyCollection = new ReadOnlyCollection<Person>(PersonList);
			List<Country> CountryList = new List<Country>();
			this._CountryList = CountryList;
			this._CountryReadOnlyCollection = new ReadOnlyCollection<Country>(CountryList);
		}
		#region Exception Helpers
		[SuppressMessageAttribute("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		private static ArgumentException GetDifferentContextsException()
		{
			return new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
		}
		private static ArgumentException GetConstraintEnforcementFailedException(string paramName)
		{
			return new ArgumentException("Argument failed constraint enforcement.", paramName);
		}
		#endregion // Exception Helpers
		#region Lookup and External Constraint Enforcement
		private readonly Dictionary<string, Country> _CountryCountry_nameDictionary = new Dictionary<string, Country>();
		public Country GetCountryByCountry_name(string Country_name)
		{
			return this._CountryCountry_nameDictionary[Country_name];
		}
		public bool TryGetCountryByCountry_name(string Country_name, out Country Country)
		{
			return this._CountryCountry_nameDictionary.TryGetValue(Country_name, out Country);
		}
		#endregion // Lookup and External Constraint Enforcement
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext;
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionCallbacks<TClass, TProperty>
			where TClass : class, IHasPersonCountryDemoContext
		{
			public ConstraintEnforcementCollectionCallbacks(PotentialCollectionModificationCallback<TClass, TProperty> adding, CommittedCollectionModificationCallback<TClass, TProperty> added, PotentialCollectionModificationCallback<TClass, TProperty> removing, CommittedCollectionModificationCallback<TClass, TProperty> removed)
			{
				this.Adding = adding;
				this.Added = added;
				this.Removing = removing;
				this.Removed = removed;
			}
			public readonly PotentialCollectionModificationCallback<TClass, TProperty> Adding;
			public readonly CommittedCollectionModificationCallback<TClass, TProperty> Added;
			public readonly PotentialCollectionModificationCallback<TClass, TProperty> Removing;
			public readonly CommittedCollectionModificationCallback<TClass, TProperty> Removed;
		}
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private struct ConstraintEnforcementCollectionTypeAndPropertyNameKey : IEquatable<ConstraintEnforcementCollectionTypeAndPropertyNameKey>
		{
			public ConstraintEnforcementCollectionTypeAndPropertyNameKey(Type type, string name)
			{
				this.Type = type;
				this.Name = name;
			}
			public readonly Type Type;
			public readonly string Name;
			public override int GetHashCode()
			{
				return this.Type.GetHashCode() ^ this.Name.GetHashCode();
			}
			public override bool Equals(object obj)
			{
				return (obj is ConstraintEnforcementCollectionTypeAndPropertyNameKey) && this.Equals((ConstraintEnforcementCollectionTypeAndPropertyNameKey)obj);
			}
			public bool Equals(ConstraintEnforcementCollectionTypeAndPropertyNameKey other)
			{
				return this.Type.Equals(other.Type) && this.Name.Equals(other.Name);
			}
			public static bool operator ==(ConstraintEnforcementCollectionTypeAndPropertyNameKey left, ConstraintEnforcementCollectionTypeAndPropertyNameKey right)
			{
				return left.Equals(right);
			}
			public static bool operator !=(ConstraintEnforcementCollectionTypeAndPropertyNameKey left, ConstraintEnforcementCollectionTypeAndPropertyNameKey right)
			{
				return !(left.Equals(right));
			}
		}
		private readonly Dictionary<Type, object> _ContraintEnforcementCollectionCallbacksByTypeDictionary;
		private readonly Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> _ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
		private bool OnAdding<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private bool OnAdding<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private void OnAdded<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private void OnAdded<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private bool OnRemoving<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private bool OnRemoving<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private void OnRemoved<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		private void OnRemoved<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ConstraintEnforcementCollection<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasPersonCountryDemoContext
		{
			private readonly TClass _instance;
			private readonly List<TProperty> _list = new List<TProperty>();
			public ConstraintEnforcementCollection(TClass instance)
			{
				this._instance = instance;
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			public IEnumerator<TProperty> GetEnumerator()
			{
				return this._list.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnAdding(this._instance, item))
				{
					this._list.Add(item);
					this._instance.Context.OnAdded(this._instance, item);
				}
			}
			public bool Remove(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnRemoving(this._instance, item))
				{
					if (this._list.Remove(item))
					{
						this._instance.Context.OnRemoved(this._instance, item);
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				for (int i = 0; i < this._list.Count; ++i)
				{
					this.Remove(this._list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return this._list.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this._list.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
		}
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasPersonCountryDemoContext
		{
			private readonly TClass _instance;
			private readonly string _PropertyName;
			private readonly List<TProperty> _list = new List<TProperty>();
			public ConstraintEnforcementCollectionWithPropertyName(TClass instance, string propertyName)
			{
				this._instance = instance;
				this._PropertyName = propertyName;
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			public IEnumerator<TProperty> GetEnumerator()
			{
				return this._list.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnAdding(this._PropertyName, this._instance, item))
				{
					this._list.Add(item);
					this._instance.Context.OnAdded(this._PropertyName, this._instance, item);
				}
			}
			public bool Remove(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnRemoving(this._PropertyName, this._instance, item))
				{
					if (this._list.Remove(item))
					{
						this._instance.Context.OnRemoved(this._PropertyName, this._instance, item);
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				for (int i = 0; i < this._list.Count; ++i)
				{
					this.Remove(this._list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return this._list.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this._list.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
		}
		#endregion // ConstraintEnforcementCollection
		#region Person
		public Person CreatePerson(string LastName, string FirstName)
		{
			if ((object)LastName == null)
			{
				throw new ArgumentNullException("LastName");
			}
			if ((object)FirstName == null)
			{
				throw new ArgumentNullException("FirstName");
			}
			if (!(this.OnPersonLastNameChanging(null, LastName)))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("LastName");
			}
			if (!(this.OnPersonFirstNameChanging(null, FirstName)))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("FirstName");
			}
			return new PersonCore(this, LastName, FirstName);
		}
		private bool OnPersonLastNameChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonFirstNameChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonTitleChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonCountryChanging(Person instance, Country newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw PersonCountryDemoContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonCountryChanged(Person instance, Country oldValue)
		{
			if ((object)instance.Country != null)
			{
				instance.Country.Person.Add(instance);
			}
			if ((object)oldValue != null)
			{
				oldValue.Person.Remove(instance);
			}
		}
		private readonly List<Person> _PersonList;
		private readonly ReadOnlyCollection<Person> _PersonReadOnlyCollection;
		public ReadOnlyCollection<Person> PersonCollection
		{
			get
			{
				return this._PersonReadOnlyCollection;
			}
		}
		#region PersonCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class PersonCore : Person
		{
			public PersonCore(PersonCountryDemoContext context, string LastName, string FirstName)
			{
				this._Context = context;
				this._LastName = LastName;
				this._FirstName = FirstName;
				context._PersonList.Add(this);
			}
			private readonly PersonCountryDemoContext _Context;
			public override PersonCountryDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("LastName")]
			private string _LastName;
			public override string LastName
			{
				get
				{
					return this._LastName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._LastName;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonLastNameChanging(this, value) && base.RaiseLastNameChangingEvent(value))
						{
							this._LastName = value;
							base.RaiseLastNameChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("FirstName")]
			private string _FirstName;
			public override string FirstName
			{
				get
				{
					return this._FirstName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._FirstName;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonFirstNameChanging(this, value) && base.RaiseFirstNameChangingEvent(value))
						{
							this._FirstName = value;
							base.RaiseFirstNameChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Title")]
			private string _Title;
			public override string Title
			{
				get
				{
					return this._Title;
				}
				set
				{
					string oldValue = this._Title;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonTitleChanging(this, value) && base.RaiseTitleChangingEvent(value))
						{
							this._Title = value;
							base.RaiseTitleChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Country")]
			private Country _Country;
			public override Country Country
			{
				get
				{
					return this._Country;
				}
				set
				{
					Country oldValue = this._Country;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonCountryChanging(this, value) && base.RaiseCountryChangingEvent(value))
						{
							this._Country = value;
							this._Context.OnPersonCountryChanged(this, oldValue);
							base.RaiseCountryChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonCore
		#endregion // Person
		#region Country
		public Country CreateCountry(string Country_name)
		{
			if ((object)Country_name == null)
			{
				throw new ArgumentNullException("Country_name");
			}
			if (!(this.OnCountryCountry_nameChanging(null, Country_name)))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("Country_name");
			}
			return new CountryCore(this, Country_name);
		}
		private bool OnCountryCountry_nameChanging(Country instance, string newValue)
		{
			Country currentInstance;
			if (this._CountryCountry_nameDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnCountryCountry_nameChanged(Country instance, string oldValue)
		{
			this._CountryCountry_nameDictionary.Add(instance.Country_name, instance);
			if ((object)oldValue != null)
			{
				this._CountryCountry_nameDictionary.Remove(oldValue);
			}
		}
		private bool OnCountryRegion_Region_codeChanging(Country instance, string newValue)
		{
			return true;
		}
		private bool OnCountryPersonAdding(Country instance, Person value)
		{
			if ((object)this != value.Context)
			{
				throw PersonCountryDemoContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnCountryPersonAdded(Country instance, Person value)
		{
			value.Country = instance;
		}
		private void OnCountryPersonRemoved(Country instance, Person value)
		{
			value.Country = null;
		}
		private readonly List<Country> _CountryList;
		private readonly ReadOnlyCollection<Country> _CountryReadOnlyCollection;
		public ReadOnlyCollection<Country> CountryCollection
		{
			get
			{
				return this._CountryReadOnlyCollection;
			}
		}
		#region CountryCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class CountryCore : Country
		{
			public CountryCore(PersonCountryDemoContext context, string Country_name)
			{
				this._Context = context;
				this._Person = new ConstraintEnforcementCollection<Country, Person>(this);
				this._Country_name = Country_name;
				context.OnCountryCountry_nameChanged(this, null);
				context._CountryList.Add(this);
			}
			private readonly PersonCountryDemoContext _Context;
			public override PersonCountryDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("Country_name")]
			private string _Country_name;
			public override string Country_name
			{
				get
				{
					return this._Country_name;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Country_name;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnCountryCountry_nameChanging(this, value) && base.RaiseCountry_nameChangingEvent(value))
						{
							this._Country_name = value;
							this._Context.OnCountryCountry_nameChanged(this, oldValue);
							base.RaiseCountry_nameChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Region_Region_code")]
			private string _Region_Region_code;
			public override string Region_Region_code
			{
				get
				{
					return this._Region_Region_code;
				}
				set
				{
					string oldValue = this._Region_Region_code;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnCountryRegion_Region_codeChanging(this, value) && base.RaiseRegion_Region_codeChangingEvent(value))
						{
							this._Region_Region_code = value;
							base.RaiseRegion_Region_codeChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private readonly ICollection<Person> _Person;
			public override ICollection<Person> Person
			{
				get
				{
					return this._Person;
				}
			}
		}
		#endregion // CountryCore
		#endregion // Country
	}
	#endregion // PersonCountryDemoContext
}

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
namespace SampleModel
{
	#region PersonDrivesCar
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class PersonDrivesCar : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonDrivesCar()
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>> DrivesCar_vinChanging
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
		protected bool RaiseDrivesCar_vinChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonDrivesCar, int> eventArgs = new PropertyChangingEventArgs<PersonDrivesCar, int>(this, this.DrivesCar_vin, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonDrivesCar, int>> DrivesCar_vinChanged
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
		protected void RaiseDrivesCar_vinChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonDrivesCar, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<PersonDrivesCar, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonDrivesCar, int>(this, oldValue, this.DrivesCar_vin), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("DrivesCar_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>> DrivenByPersonChanging
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
		protected bool RaiseDrivenByPersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonDrivesCar, Person> eventArgs = new PropertyChangingEventArgs<PersonDrivesCar, Person>(this, this.DrivenByPerson, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonDrivesCar, Person>> DrivenByPersonChanged
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
		protected void RaiseDrivenByPersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonDrivesCar, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<PersonDrivesCar, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonDrivesCar, Person>(this, oldValue, this.DrivenByPerson), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("DrivenByPerson");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int DrivesCar_vin
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person DrivenByPerson
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
			return string.Format(provider, @"PersonDrivesCar{0}{{{0}{1}DrivesCar_vin = ""{2}"",{0}{1}DrivenByPerson = {3}{0}}}", Environment.NewLine, "	", this.DrivesCar_vin, "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // PersonDrivesCar
	#region PersonBoughtCarFromPersonOnDate
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class PersonBoughtCarFromPersonOnDate : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonBoughtCarFromPersonOnDate()
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> CarSold_vinChanging
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
		protected bool RaiseCarSold_vinChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int> eventArgs = new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, this.CarSold_vin, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> CarSold_vinChanged
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
		protected void RaiseCarSold_vinChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, oldValue, this.CarSold_vin), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("CarSold_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> SaleDate_YMDChanging
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
		protected bool RaiseSaleDate_YMDChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int> eventArgs = new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, this.SaleDate_YMD, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> SaleDate_YMDChanged
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
		protected void RaiseSaleDate_YMDChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, oldValue, this.SaleDate_YMD), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("SaleDate_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> BuyerChanging
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
		protected bool RaiseBuyerChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person> eventArgs = new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, this.Buyer, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> BuyerChanged
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
		protected void RaiseBuyerChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, oldValue, this.Buyer), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Buyer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> SellerChanging
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
		protected bool RaiseSellerChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person> eventArgs = new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, this.Seller, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> SellerChanged
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
		protected void RaiseSellerChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, oldValue, this.Seller), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Seller");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int CarSold_vin
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int SaleDate_YMD
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Buyer
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Seller
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
			return string.Format(provider, @"PersonBoughtCarFromPersonOnDate{0}{{{0}{1}CarSold_vin = ""{2}"",{0}{1}SaleDate_YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, "	", this.CarSold_vin, this.SaleDate_YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // PersonBoughtCarFromPersonOnDate
	#region Review
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Review : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Review()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[3];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Review, int>> Car_vinChanging
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
		protected bool RaiseCar_vinChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<Review, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Review, int> eventArgs = new PropertyChangingEventArgs<Review, int>(this, this.Car_vin, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, int>> Car_vinChanged
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
		protected void RaiseCar_vinChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Review, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Review, int>(this, oldValue, this.Car_vin), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Car_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, int>> Rating_Nr_IntegerChanging
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
		protected bool RaiseRating_Nr_IntegerChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<Review, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Review, int> eventArgs = new PropertyChangingEventArgs<Review, int>(this, this.Rating_Nr_Integer, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, int>> Rating_Nr_IntegerChanged
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
		protected void RaiseRating_Nr_IntegerChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Review, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Review, int>(this, oldValue, this.Rating_Nr_Integer), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Rating_Nr_Integer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, string>> Criterion_NameChanging
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
		protected bool RaiseCriterion_NameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Review, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Review, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Review, string> eventArgs = new PropertyChangingEventArgs<Review, string>(this, this.Criterion_Name, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, string>> Criterion_NameChanged
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
		protected void RaiseCriterion_NameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Review, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Review, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Review, string>(this, oldValue, this.Criterion_Name), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Criterion_Name");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int Car_vin
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int Rating_Nr_Integer
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string Criterion_Name
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
			return string.Format(provider, @"Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criterion_Name = ""{4}""{0}}}", Environment.NewLine, "	", this.Car_vin, this.Rating_Nr_Integer, this.Criterion_Name);
		}
	}
	#endregion // Review
	#region PersonHasNickName
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class PersonHasNickName : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonHasNickName()
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<PersonHasNickName, string>> NickNameChanging
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
		protected bool RaiseNickNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonHasNickName, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<PersonHasNickName, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonHasNickName, string> eventArgs = new PropertyChangingEventArgs<PersonHasNickName, string>(this, this.NickName, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonHasNickName, string>> NickNameChanged
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
		protected void RaiseNickNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonHasNickName, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<PersonHasNickName, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonHasNickName, string>(this, oldValue, this.NickName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("NickName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>> PersonChanging
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
		protected bool RaisePersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<PersonHasNickName, Person> eventArgs = new PropertyChangingEventArgs<PersonHasNickName, Person>(this, this.Person, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonHasNickName, Person>> PersonChanged
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
		protected void RaisePersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonHasNickName, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<PersonHasNickName, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<PersonHasNickName, Person>(this, oldValue, this.Person), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Person");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string NickName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Person
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
			return string.Format(provider, @"PersonHasNickName{0}{{{0}{1}NickName = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.NickName, "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // PersonHasNickName
	#region Person
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Person : INotifyPropertyChanged, IHasSampleModelContext
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
					this._events = new System.Delegate[17];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
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
		protected bool RaiseFirstNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Person, string>>;
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
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseFirstNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.FirstName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
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
		protected bool RaiseDate_YMDChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, int> eventArgs = new PropertyChangingEventArgs<Person, int>(this, this.Date_YMD, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
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
		protected void RaiseDate_YMDChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, int>(this, oldValue, this.Date_YMD), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Date_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
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
		protected bool RaiseLastNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person, string>>;
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
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseLastNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.LastName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
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
		protected bool RaiseOptionalUniqueStringChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.OptionalUniqueString, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
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
		protected void RaiseOptionalUniqueStringChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.OptionalUniqueString), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("OptionalUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseHatType_ColorARGBChangingEvent(Nullable<int> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Nullable<int>> eventArgs = new PropertyChangingEventArgs<Person, Nullable<int>>(this, this.HatType_ColorARGB, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseHatType_ColorARGBChangedEvent(Nullable<int> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, oldValue, this.HatType_ColorARGB), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("HatType_ColorARGB");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Events[5] = System.Delegate.Combine(this.Events[5], value);
			}
			remove
			{
				this.Events[5] = System.Delegate.Remove(this.Events[5], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.HatType_HatTypeStyle_HatTypeStyle_Description, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Events[5] = System.Delegate.Combine(this.Events[5], value);
			}
			remove
			{
				this.Events[5] = System.Delegate.Remove(this.Events[5], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.HatType_HatTypeStyle_HatTypeStyle_Description), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("HatType_HatTypeStyle_HatTypeStyle_Description");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Events[6] = System.Delegate.Combine(this.Events[6], value);
			}
			remove
			{
				this.Events[6] = System.Delegate.Remove(this.Events[6], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseOwnsCar_vinChangingEvent(Nullable<int> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler = this.Events[6] as EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Nullable<int>> eventArgs = new PropertyChangingEventArgs<Person, Nullable<int>>(this, this.OwnsCar_vin, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Events[6] = System.Delegate.Combine(this.Events[6], value);
			}
			remove
			{
				this.Events[6] = System.Delegate.Remove(this.Events[6], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseOwnsCar_vinChangedEvent(Nullable<int> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, oldValue, this.OwnsCar_vin), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("OwnsCar_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Events[7] = System.Delegate.Combine(this.Events[7], value);
			}
			remove
			{
				this.Events[7] = System.Delegate.Remove(this.Events[7], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseGender_Gender_CodeChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.Gender_Gender_Code, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Events[7] = System.Delegate.Combine(this.Events[7], value);
			}
			remove
			{
				this.Events[7] = System.Delegate.Remove(this.Events[7], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseGender_Gender_CodeChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[7] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.Gender_Gender_Code), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Gender_Gender_Code");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Events[8] = System.Delegate.Combine(this.Events[8], value);
			}
			remove
			{
				this.Events[8] = System.Delegate.Remove(this.Events[8], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaisePersonHasParentsChangingEvent(Nullable<bool> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> eventHandler = this.Events[8] as EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Nullable<bool>> eventArgs = new PropertyChangingEventArgs<Person, Nullable<bool>>(this, this.PersonHasParents, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Events[8] = System.Delegate.Combine(this.Events[8], value);
			}
			remove
			{
				this.Events[8] = System.Delegate.Remove(this.Events[8], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaisePersonHasParentsChangedEvent(Nullable<bool> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Nullable<bool>>(this, oldValue, this.PersonHasParents), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("PersonHasParents");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Events[9] = System.Delegate.Combine(this.Events[9], value);
			}
			remove
			{
				this.Events[9] = System.Delegate.Remove(this.Events[9], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseOptionalUniqueDecimalChangingEvent(Nullable<decimal> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Nullable<decimal>> eventArgs = new PropertyChangingEventArgs<Person, Nullable<decimal>>(this, this.OptionalUniqueDecimal, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Events[9] = System.Delegate.Combine(this.Events[9], value);
			}
			remove
			{
				this.Events[9] = System.Delegate.Remove(this.Events[9], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseOptionalUniqueDecimalChangedEvent(Nullable<decimal> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> eventHandler = this.Events[9] as EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Nullable<decimal>>(this, oldValue, this.OptionalUniqueDecimal), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("OptionalUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Events[10] = System.Delegate.Combine(this.Events[10], value);
			}
			remove
			{
				this.Events[10] = System.Delegate.Remove(this.Events[10], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseMandatoryUniqueDecimalChangingEvent(decimal newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, decimal>> eventHandler = this.Events[10] as EventHandler<PropertyChangingEventArgs<Person, decimal>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, decimal> eventArgs = new PropertyChangingEventArgs<Person, decimal>(this, this.MandatoryUniqueDecimal, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Events[10] = System.Delegate.Combine(this.Events[10], value);
			}
			remove
			{
				this.Events[10] = System.Delegate.Remove(this.Events[10], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseMandatoryUniqueDecimalChangedEvent(decimal oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, decimal>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<Person, decimal>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, decimal>(this, oldValue, this.MandatoryUniqueDecimal), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("MandatoryUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Events[11] = System.Delegate.Combine(this.Events[11], value);
			}
			remove
			{
				this.Events[11] = System.Delegate.Remove(this.Events[11], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseMandatoryUniqueStringChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[11] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.MandatoryUniqueString, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Events[11] = System.Delegate.Combine(this.Events[11], value);
			}
			remove
			{
				this.Events[11] = System.Delegate.Remove(this.Events[11], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseMandatoryUniqueStringChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[11] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.MandatoryUniqueString), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("MandatoryUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Events[12] = System.Delegate.Combine(this.Events[12], value);
			}
			remove
			{
				this.Events[12] = System.Delegate.Remove(this.Events[12], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseValueType1DoesSomethingElseWithChangingEvent(ValueType1 newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, ValueType1>> eventHandler = this.Events[12] as EventHandler<PropertyChangingEventArgs<Person, ValueType1>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, ValueType1> eventArgs = new PropertyChangingEventArgs<Person, ValueType1>(this, this.ValueType1DoesSomethingElseWith, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Events[12] = System.Delegate.Combine(this.Events[12], value);
			}
			remove
			{
				this.Events[12] = System.Delegate.Remove(this.Events[12], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseValueType1DoesSomethingElseWithChangedEvent(ValueType1 oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, ValueType1>> eventHandler = this.Events[12] as EventHandler<PropertyChangedEventArgs<Person, ValueType1>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, ValueType1>(this, oldValue, this.ValueType1DoesSomethingElseWith), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("ValueType1DoesSomethingElseWith");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Events[13] = System.Delegate.Combine(this.Events[13], value);
			}
			remove
			{
				this.Events[13] = System.Delegate.Remove(this.Events[13], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseMalePersonChangingEvent(MalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, MalePerson>> eventHandler = this.Events[13] as EventHandler<PropertyChangingEventArgs<Person, MalePerson>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, MalePerson> eventArgs = new PropertyChangingEventArgs<Person, MalePerson>(this, this.MalePerson, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Events[13] = System.Delegate.Combine(this.Events[13], value);
			}
			remove
			{
				this.Events[13] = System.Delegate.Remove(this.Events[13], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseMalePersonChangedEvent(MalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, MalePerson>> eventHandler = this.Events[13] as EventHandler<PropertyChangedEventArgs<Person, MalePerson>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, MalePerson>(this, oldValue, this.MalePerson), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("MalePerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Events[14] = System.Delegate.Combine(this.Events[14], value);
			}
			remove
			{
				this.Events[14] = System.Delegate.Remove(this.Events[14], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseFemalePersonChangingEvent(FemalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> eventHandler = this.Events[14] as EventHandler<PropertyChangingEventArgs<Person, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, FemalePerson> eventArgs = new PropertyChangingEventArgs<Person, FemalePerson>(this, this.FemalePerson, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Events[14] = System.Delegate.Combine(this.Events[14], value);
			}
			remove
			{
				this.Events[14] = System.Delegate.Remove(this.Events[14], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseFemalePersonChangedEvent(FemalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> eventHandler = this.Events[14] as EventHandler<PropertyChangedEventArgs<Person, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, FemalePerson>(this, oldValue, this.FemalePerson), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("FemalePerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Events[15] = System.Delegate.Combine(this.Events[15], value);
			}
			remove
			{
				this.Events[15] = System.Delegate.Remove(this.Events[15], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseChildPersonChangingEvent(ChildPerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> eventHandler = this.Events[15] as EventHandler<PropertyChangingEventArgs<Person, ChildPerson>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, ChildPerson> eventArgs = new PropertyChangingEventArgs<Person, ChildPerson>(this, this.ChildPerson, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Events[15] = System.Delegate.Combine(this.Events[15], value);
			}
			remove
			{
				this.Events[15] = System.Delegate.Remove(this.Events[15], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseChildPersonChangedEvent(ChildPerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> eventHandler = this.Events[15] as EventHandler<PropertyChangedEventArgs<Person, ChildPerson>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, ChildPerson>(this, oldValue, this.ChildPerson), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("ChildPerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Events[16] = System.Delegate.Combine(this.Events[16], value);
			}
			remove
			{
				this.Events[16] = System.Delegate.Remove(this.Events[16], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaiseDeathChangingEvent(Death newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Death>> eventHandler = this.Events[16] as EventHandler<PropertyChangingEventArgs<Person, Death>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Death> eventArgs = new PropertyChangingEventArgs<Person, Death>(this, this.Death, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Events[16] = System.Delegate.Combine(this.Events[16], value);
			}
			remove
			{
				this.Events[16] = System.Delegate.Remove(this.Events[16], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaiseDeathChangedEvent(Death oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Death>> eventHandler = this.Events[16] as EventHandler<PropertyChangedEventArgs<Person, Death>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Death>(this, oldValue, this.Death), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Death");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string FirstName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int Date_YMD
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string LastName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract string OptionalUniqueString
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<int> HatType_ColorARGB
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<int> OwnsCar_vin
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string Gender_Gender_Code
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<bool> PersonHasParents
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<decimal> OptionalUniqueDecimal
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract decimal MandatoryUniqueDecimal
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string MandatoryUniqueString
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ValueType1 ValueType1DoesSomethingElseWith
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract MalePerson MalePerson
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract FemalePerson FemalePerson
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ChildPerson ChildPerson
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Death Death
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<Task> Task
		{
			get;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}OptionalUniqueString = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}PersonHasParents = ""{10}"",{0}{1}OptionalUniqueDecimal = ""{11}"",{0}{1}MandatoryUniqueDecimal = ""{12}"",{0}{1}MandatoryUniqueString = ""{13}"",{0}{1}ValueType1DoesSomethingElseWith = {14},{0}{1}MalePerson = {15},{0}{1}FemalePerson = {16},{0}{1}ChildPerson = {17},{0}{1}Death = {18}{0}}}", Environment.NewLine, "	", this.FirstName, this.Date_YMD, this.LastName, this.OptionalUniqueString, this.HatType_ColorARGB, this.HatType_HatTypeStyle_HatTypeStyle_Description, this.OwnsCar_vin, this.Gender_Gender_Code, this.PersonHasParents, this.OptionalUniqueDecimal, this.MandatoryUniqueDecimal, this.MandatoryUniqueString, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		public static explicit operator MalePerson(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else if (Person.MalePerson == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return Person.MalePerson;
			}
		}
		public static explicit operator FemalePerson(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else if (Person.FemalePerson == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return Person.FemalePerson;
			}
		}
		public static explicit operator ChildPerson(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else if (Person.ChildPerson == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return Person.ChildPerson;
			}
		}
		public static explicit operator Death(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else if (Person.Death == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return Person.Death;
			}
		}
		public static explicit operator NaturalDeath(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else
			{
				return (NaturalDeath)(Death)Person;
			}
		}
		public static explicit operator UnnaturalDeath(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else
			{
				return (UnnaturalDeath)(Death)Person;
			}
		}
	}
	#endregion // Person
	#region MalePerson
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class MalePerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected MalePerson()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[1];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<MalePerson, Person>> PersonChanging
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
		protected bool RaisePersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<MalePerson, Person>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<MalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<MalePerson, Person> eventArgs = new PropertyChangingEventArgs<MalePerson, Person>(this, this.Person, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<MalePerson, Person>> PersonChanged
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
		protected void RaisePersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<MalePerson, Person>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<MalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<MalePerson, Person>(this, oldValue, this.Person), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Person");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<ChildPerson> ChildPerson
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "MalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator Person(MalePerson MalePerson)
		{
			if (MalePerson == null)
			{
				return null;
			}
			else
			{
				return MalePerson.Person;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual int Date_YMD
		{
			get
			{
				return this.Person.Date_YMD;
			}
			set
			{
				this.Person.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
		{
			add
			{
				this.Person.Date_YMDChanging += value;
			}
			remove
			{
				this.Person.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
		{
			add
			{
				this.Person.Date_YMDChanged += value;
			}
			remove
			{
				this.Person.Date_YMDChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual Nullable<bool> PersonHasParents
		{
			get
			{
				return this.Person.PersonHasParents;
			}
			set
			{
				this.Person.PersonHasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Person.PersonHasParentsChanging += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Person.PersonHasParentsChanged += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Person.FemalePerson;
			}
			set
			{
				this.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Person.FemalePersonChanged -= value;
			}
		}
		public virtual Death Death
		{
			get
			{
				return this.Person.Death;
			}
			set
			{
				this.Person.Death = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Person.DeathChanging += value;
			}
			remove
			{
				this.Person.DeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Person.DeathChanged += value;
			}
			remove
			{
				this.Person.DeathChanged -= value;
			}
		}
		public virtual ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get
			{
				return this.Person.PersonDrivesCarAsDrivenByPerson;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsBuyer;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsSeller;
			}
		}
		public virtual ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get
			{
				return this.Person.PersonHasNickNameAsPerson;
			}
		}
		public virtual ICollection<Task> Task
		{
			get
			{
				return this.Person.Task;
			}
		}
		public virtual ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWith;
			}
		}
	}
	#endregion // MalePerson
	#region FemalePerson
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class FemalePerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected FemalePerson()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[1];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<FemalePerson, Person>> PersonChanging
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
		protected bool RaisePersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<FemalePerson, Person>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<FemalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<FemalePerson, Person> eventArgs = new PropertyChangingEventArgs<FemalePerson, Person>(this, this.Person, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<FemalePerson, Person>> PersonChanged
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
		protected void RaisePersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<FemalePerson, Person>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<FemalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<FemalePerson, Person>(this, oldValue, this.Person), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Person");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<ChildPerson> ChildPerson
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "FemalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator Person(FemalePerson FemalePerson)
		{
			if (FemalePerson == null)
			{
				return null;
			}
			else
			{
				return FemalePerson.Person;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual int Date_YMD
		{
			get
			{
				return this.Person.Date_YMD;
			}
			set
			{
				this.Person.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
		{
			add
			{
				this.Person.Date_YMDChanging += value;
			}
			remove
			{
				this.Person.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
		{
			add
			{
				this.Person.Date_YMDChanged += value;
			}
			remove
			{
				this.Person.Date_YMDChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual Nullable<bool> PersonHasParents
		{
			get
			{
				return this.Person.PersonHasParents;
			}
			set
			{
				this.Person.PersonHasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Person.PersonHasParentsChanging += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Person.PersonHasParentsChanged += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Person.MalePerson;
			}
			set
			{
				this.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Person.MalePersonChanged -= value;
			}
		}
		public virtual Death Death
		{
			get
			{
				return this.Person.Death;
			}
			set
			{
				this.Person.Death = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Person.DeathChanging += value;
			}
			remove
			{
				this.Person.DeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Person.DeathChanged += value;
			}
			remove
			{
				this.Person.DeathChanged -= value;
			}
		}
		public virtual ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get
			{
				return this.Person.PersonDrivesCarAsDrivenByPerson;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsBuyer;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsSeller;
			}
		}
		public virtual ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get
			{
				return this.Person.PersonHasNickNameAsPerson;
			}
		}
		public virtual ICollection<Task> Task
		{
			get
			{
				return this.Person.Task;
			}
		}
		public virtual ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWith;
			}
		}
	}
	#endregion // FemalePerson
	#region ChildPerson
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class ChildPerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected ChildPerson()
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, int>> BirthOrder_BirthOrder_NrChanging
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
		protected bool RaiseBirthOrder_BirthOrder_NrChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<ChildPerson, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<ChildPerson, int> eventArgs = new PropertyChangingEventArgs<ChildPerson, int>(this, this.BirthOrder_BirthOrder_Nr, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, int>> BirthOrder_BirthOrder_NrChanged
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
		protected void RaiseBirthOrder_BirthOrder_NrChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<ChildPerson, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ChildPerson, int>(this, oldValue, this.BirthOrder_BirthOrder_Nr), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("BirthOrder_BirthOrder_Nr");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>> FatherChanging
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
		protected bool RaiseFatherChangingEvent(MalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<ChildPerson, MalePerson> eventArgs = new PropertyChangingEventArgs<ChildPerson, MalePerson>(this, this.Father, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>> FatherChanged
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
		protected void RaiseFatherChangedEvent(MalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ChildPerson, MalePerson>(this, oldValue, this.Father), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Father");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>> MotherChanging
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
		protected bool RaiseMotherChangingEvent(FemalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<ChildPerson, FemalePerson> eventArgs = new PropertyChangingEventArgs<ChildPerson, FemalePerson>(this, this.Mother, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>> MotherChanged
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
		protected void RaiseMotherChangedEvent(FemalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ChildPerson, FemalePerson>(this, oldValue, this.Mother), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Mother");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, Person>> PersonChanging
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
		protected bool RaisePersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<ChildPerson, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<ChildPerson, Person> eventArgs = new PropertyChangingEventArgs<ChildPerson, Person>(this, this.Person, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, Person>> PersonChanged
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
		protected void RaisePersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<ChildPerson, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ChildPerson, Person>(this, oldValue, this.Person), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Person");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int BirthOrder_BirthOrder_Nr
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract MalePerson Father
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract FemalePerson Mother
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Person
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
			return string.Format(provider, @"ChildPerson{0}{{{0}{1}BirthOrder_BirthOrder_Nr = ""{2}"",{0}{1}Father = {3},{0}{1}Mother = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, "	", this.BirthOrder_BirthOrder_Nr, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator Person(ChildPerson ChildPerson)
		{
			if (ChildPerson == null)
			{
				return null;
			}
			else
			{
				return ChildPerson.Person;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual int Date_YMD
		{
			get
			{
				return this.Person.Date_YMD;
			}
			set
			{
				this.Person.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
		{
			add
			{
				this.Person.Date_YMDChanging += value;
			}
			remove
			{
				this.Person.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
		{
			add
			{
				this.Person.Date_YMDChanged += value;
			}
			remove
			{
				this.Person.Date_YMDChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual Nullable<bool> PersonHasParents
		{
			get
			{
				return this.Person.PersonHasParents;
			}
			set
			{
				this.Person.PersonHasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Person.PersonHasParentsChanging += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Person.PersonHasParentsChanged += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Person.MalePerson;
			}
			set
			{
				this.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Person.FemalePerson;
			}
			set
			{
				this.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Person.FemalePersonChanged -= value;
			}
		}
		public virtual Death Death
		{
			get
			{
				return this.Person.Death;
			}
			set
			{
				this.Person.Death = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Person.DeathChanging += value;
			}
			remove
			{
				this.Person.DeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Person.DeathChanged += value;
			}
			remove
			{
				this.Person.DeathChanged -= value;
			}
		}
		public virtual ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get
			{
				return this.Person.PersonDrivesCarAsDrivenByPerson;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsBuyer;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsSeller;
			}
		}
		public virtual ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get
			{
				return this.Person.PersonHasNickNameAsPerson;
			}
		}
		public virtual ICollection<Task> Task
		{
			get
			{
				return this.Person.Task;
			}
		}
		public virtual ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWith;
			}
		}
	}
	#endregion // ChildPerson
	#region Death
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Death : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Death()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[5];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
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
		protected bool RaiseDate_YMDChangingEvent(Nullable<int> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Death, Nullable<int>> eventArgs = new PropertyChangingEventArgs<Death, Nullable<int>>(this, this.Date_YMD, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
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
		protected void RaiseDate_YMDChangedEvent(Nullable<int> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death, Nullable<int>>(this, oldValue, this.Date_YMD), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Date_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
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
		protected bool RaiseDeathCause_DeathCause_TypeChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Death, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Death, string> eventArgs = new PropertyChangingEventArgs<Death, string>(this, this.DeathCause_DeathCause_Type, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
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
		protected void RaiseDeathCause_DeathCause_TypeChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Death, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death, string>(this, oldValue, this.DeathCause_DeathCause_Type), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("DeathCause_DeathCause_Type");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> NaturalDeathChanging
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
		protected bool RaiseNaturalDeathChangingEvent(NaturalDeath newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Death, NaturalDeath> eventArgs = new PropertyChangingEventArgs<Death, NaturalDeath>(this, this.NaturalDeath, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> NaturalDeathChanged
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
		protected void RaiseNaturalDeathChangedEvent(NaturalDeath oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death, NaturalDeath>(this, oldValue, this.NaturalDeath), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("NaturalDeath");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanging
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
		protected bool RaiseUnnaturalDeathChangingEvent(UnnaturalDeath newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Death, UnnaturalDeath> eventArgs = new PropertyChangingEventArgs<Death, UnnaturalDeath>(this, this.UnnaturalDeath, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanged
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
		protected void RaiseUnnaturalDeathChangedEvent(UnnaturalDeath oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death, UnnaturalDeath>(this, oldValue, this.UnnaturalDeath), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("UnnaturalDeath");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected bool RaisePersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Death, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Death, Person> eventArgs = new PropertyChangingEventArgs<Death, Person>(this, this.Person, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected void RaisePersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Death, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death, Person>(this, oldValue, this.Person), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Person");
			}
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<int> Date_YMD
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string DeathCause_DeathCause_Type
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract NaturalDeath NaturalDeath
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract UnnaturalDeath UnnaturalDeath
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Person Person
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
			return string.Format(provider, @"Death{0}{{{0}{1}Date_YMD = ""{2}"",{0}{1}DeathCause_DeathCause_Type = ""{3}"",{0}{1}NaturalDeath = {4},{0}{1}UnnaturalDeath = {5},{0}{1}Person = {6}{0}}}", Environment.NewLine, "	", this.Date_YMD, this.DeathCause_DeathCause_Type, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator Person(Death Death)
		{
			if (Death == null)
			{
				return null;
			}
			else
			{
				return Death.Person;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual Nullable<bool> PersonHasParents
		{
			get
			{
				return this.Person.PersonHasParents;
			}
			set
			{
				this.Person.PersonHasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Person.PersonHasParentsChanging += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Person.PersonHasParentsChanged += value;
			}
			remove
			{
				this.Person.PersonHasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Person.MalePerson;
			}
			set
			{
				this.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Person.FemalePerson;
			}
			set
			{
				this.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Person.ChildPerson;
			}
			set
			{
				this.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Person.ChildPersonChanged -= value;
			}
		}
		public virtual ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get
			{
				return this.Person.PersonDrivesCarAsDrivenByPerson;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsBuyer;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateAsSeller;
			}
		}
		public virtual ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get
			{
				return this.Person.PersonHasNickNameAsPerson;
			}
		}
		public virtual ICollection<Task> Task
		{
			get
			{
				return this.Person.Task;
			}
		}
		public virtual ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWith;
			}
		}
		public static explicit operator NaturalDeath(Death Death)
		{
			if (Death == null)
			{
				return null;
			}
			else if (Death.NaturalDeath == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return Death.NaturalDeath;
			}
		}
		public static explicit operator UnnaturalDeath(Death Death)
		{
			if (Death == null)
			{
				return null;
			}
			else if (Death.UnnaturalDeath == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return Death.UnnaturalDeath;
			}
		}
	}
	#endregion // Death
	#region NaturalDeath
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class NaturalDeath : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected NaturalDeath()
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>> NaturalDeathIsFromProstateCancerChanging
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
		protected bool RaiseNaturalDeathIsFromProstateCancerChangingEvent(Nullable<bool> newValue)
		{
			EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<NaturalDeath, Nullable<bool>> eventArgs = new PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>(this, this.NaturalDeathIsFromProstateCancer, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>> NaturalDeathIsFromProstateCancerChanged
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
		protected void RaiseNaturalDeathIsFromProstateCancerChangedEvent(Nullable<bool> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>(this, oldValue, this.NaturalDeathIsFromProstateCancer), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("NaturalDeathIsFromProstateCancer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>> DeathChanging
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
		protected bool RaiseDeathChangingEvent(Death newValue)
		{
			EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<NaturalDeath, Death> eventArgs = new PropertyChangingEventArgs<NaturalDeath, Death>(this, this.Death, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>> DeathChanged
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
		protected void RaiseDeathChangedEvent(Death oldValue)
		{
			EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<NaturalDeath, Death>(this, oldValue, this.Death), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Death");
			}
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<bool> NaturalDeathIsFromProstateCancer
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Death Death
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
			return string.Format(provider, @"NaturalDeath{0}{{{0}{1}NaturalDeathIsFromProstateCancer = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, "	", this.NaturalDeathIsFromProstateCancer, "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator Death(NaturalDeath NaturalDeath)
		{
			if (NaturalDeath == null)
			{
				return null;
			}
			else
			{
				return NaturalDeath.Death;
			}
		}
		public static implicit operator Person(NaturalDeath NaturalDeath)
		{
			if (NaturalDeath == null)
			{
				return null;
			}
			else
			{
				return NaturalDeath.Death.Person;
			}
		}
		public virtual Nullable<int> Date_YMD
		{
			get
			{
				return this.Death.Date_YMD;
			}
			set
			{
				this.Death.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
		{
			add
			{
				this.Death.Date_YMDChanging += value;
			}
			remove
			{
				this.Death.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
		{
			add
			{
				this.Death.Date_YMDChanged += value;
			}
			remove
			{
				this.Death.Date_YMDChanged -= value;
			}
		}
		public virtual string DeathCause_DeathCause_Type
		{
			get
			{
				return this.Death.DeathCause_DeathCause_Type;
			}
			set
			{
				this.Death.DeathCause_DeathCause_Type = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanging += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanged += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanged -= value;
			}
		}
		public virtual UnnaturalDeath UnnaturalDeath
		{
			get
			{
				return this.Death.UnnaturalDeath;
			}
			set
			{
				this.Death.UnnaturalDeath = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanging
		{
			add
			{
				this.Death.UnnaturalDeathChanging += value;
			}
			remove
			{
				this.Death.UnnaturalDeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanged
		{
			add
			{
				this.Death.UnnaturalDeathChanged += value;
			}
			remove
			{
				this.Death.UnnaturalDeathChanged -= value;
			}
		}
		public virtual Person Person
		{
			get
			{
				return this.Death.Person;
			}
			set
			{
				this.Death.Person = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
		{
			add
			{
				this.Death.PersonChanging += value;
			}
			remove
			{
				this.Death.PersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
		{
			add
			{
				this.Death.PersonChanged += value;
			}
			remove
			{
				this.Death.PersonChanged -= value;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Death.Person.FirstName;
			}
			set
			{
				this.Death.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Death.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Death.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Death.Person.LastName;
			}
			set
			{
				this.Death.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Death.Person.LastNameChanging += value;
			}
			remove
			{
				this.Death.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Death.Person.LastNameChanged += value;
			}
			remove
			{
				this.Death.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Death.Person.OptionalUniqueString;
			}
			set
			{
				this.Death.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Death.Person.HatType_ColorARGB;
			}
			set
			{
				this.Death.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Death.Person.OwnsCar_vin;
			}
			set
			{
				this.Death.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Death.Person.Gender_Gender_Code;
			}
			set
			{
				this.Death.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual Nullable<bool> PersonHasParents
		{
			get
			{
				return this.Death.Person.PersonHasParents;
			}
			set
			{
				this.Death.Person.PersonHasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Death.Person.PersonHasParentsChanging += value;
			}
			remove
			{
				this.Death.Person.PersonHasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Death.Person.PersonHasParentsChanged += value;
			}
			remove
			{
				this.Death.Person.PersonHasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Death.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Death.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Death.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Death.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Death.Person.MandatoryUniqueString;
			}
			set
			{
				this.Death.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Death.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Death.Person.MalePerson;
			}
			set
			{
				this.Death.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Death.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Death.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Death.Person.FemalePerson;
			}
			set
			{
				this.Death.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Death.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Death.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Death.Person.ChildPerson;
			}
			set
			{
				this.Death.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Death.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Death.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanged -= value;
			}
		}
		public virtual ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get
			{
				return this.Death.Person.PersonDrivesCarAsDrivenByPerson;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateAsBuyer;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateAsSeller;
			}
		}
		public virtual ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get
			{
				return this.Death.Person.PersonHasNickNameAsPerson;
			}
		}
		public virtual ICollection<Task> Task
		{
			get
			{
				return this.Death.Person.Task;
			}
		}
		public virtual ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingWith;
			}
		}
	}
	#endregion // NaturalDeath
	#region UnnaturalDeath
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class UnnaturalDeath : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected UnnaturalDeath()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[3];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> UnnaturalDeathIsViolentChanging
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
		protected bool RaiseUnnaturalDeathIsViolentChangingEvent(Nullable<bool> newValue)
		{
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>> eventArgs = new PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>(this, this.UnnaturalDeathIsViolent, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> UnnaturalDeathIsViolentChanged
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
		protected void RaiseUnnaturalDeathIsViolentChangedEvent(Nullable<bool> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>(this, oldValue, this.UnnaturalDeathIsViolent), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("UnnaturalDeathIsViolent");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> UnnaturalDeathIsBloodyChanging
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
		protected bool RaiseUnnaturalDeathIsBloodyChangingEvent(Nullable<bool> newValue)
		{
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>> eventArgs = new PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>(this, this.UnnaturalDeathIsBloody, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> UnnaturalDeathIsBloodyChanged
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
		protected void RaiseUnnaturalDeathIsBloodyChangedEvent(Nullable<bool> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>(this, oldValue, this.UnnaturalDeathIsBloody), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("UnnaturalDeathIsBloody");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>> DeathChanging
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
		protected bool RaiseDeathChangingEvent(Death newValue)
		{
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<UnnaturalDeath, Death> eventArgs = new PropertyChangingEventArgs<UnnaturalDeath, Death>(this, this.Death, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>> DeathChanged
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
		protected void RaiseDeathChangedEvent(Death oldValue)
		{
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<UnnaturalDeath, Death>(this, oldValue, this.Death), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Death");
			}
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<bool> UnnaturalDeathIsViolent
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Nullable<bool> UnnaturalDeathIsBloody
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract Death Death
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
			return string.Format(provider, @"UnnaturalDeath{0}{{{0}{1}UnnaturalDeathIsViolent = ""{2}"",{0}{1}UnnaturalDeathIsBloody = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, "	", this.UnnaturalDeathIsViolent, this.UnnaturalDeathIsBloody, "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator Death(UnnaturalDeath UnnaturalDeath)
		{
			if (UnnaturalDeath == null)
			{
				return null;
			}
			else
			{
				return UnnaturalDeath.Death;
			}
		}
		public static implicit operator Person(UnnaturalDeath UnnaturalDeath)
		{
			if (UnnaturalDeath == null)
			{
				return null;
			}
			else
			{
				return UnnaturalDeath.Death.Person;
			}
		}
		public virtual Nullable<int> Date_YMD
		{
			get
			{
				return this.Death.Date_YMD;
			}
			set
			{
				this.Death.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
		{
			add
			{
				this.Death.Date_YMDChanging += value;
			}
			remove
			{
				this.Death.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
		{
			add
			{
				this.Death.Date_YMDChanged += value;
			}
			remove
			{
				this.Death.Date_YMDChanged -= value;
			}
		}
		public virtual string DeathCause_DeathCause_Type
		{
			get
			{
				return this.Death.DeathCause_DeathCause_Type;
			}
			set
			{
				this.Death.DeathCause_DeathCause_Type = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanging += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanged += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanged -= value;
			}
		}
		public virtual NaturalDeath NaturalDeath
		{
			get
			{
				return this.Death.NaturalDeath;
			}
			set
			{
				this.Death.NaturalDeath = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> NaturalDeathChanging
		{
			add
			{
				this.Death.NaturalDeathChanging += value;
			}
			remove
			{
				this.Death.NaturalDeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> NaturalDeathChanged
		{
			add
			{
				this.Death.NaturalDeathChanged += value;
			}
			remove
			{
				this.Death.NaturalDeathChanged -= value;
			}
		}
		public virtual Person Person
		{
			get
			{
				return this.Death.Person;
			}
			set
			{
				this.Death.Person = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
		{
			add
			{
				this.Death.PersonChanging += value;
			}
			remove
			{
				this.Death.PersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
		{
			add
			{
				this.Death.PersonChanged += value;
			}
			remove
			{
				this.Death.PersonChanged -= value;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Death.Person.FirstName;
			}
			set
			{
				this.Death.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Death.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Death.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Death.Person.LastName;
			}
			set
			{
				this.Death.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Death.Person.LastNameChanging += value;
			}
			remove
			{
				this.Death.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Death.Person.LastNameChanged += value;
			}
			remove
			{
				this.Death.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Death.Person.OptionalUniqueString;
			}
			set
			{
				this.Death.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Death.Person.HatType_ColorARGB;
			}
			set
			{
				this.Death.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Death.Person.OwnsCar_vin;
			}
			set
			{
				this.Death.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Death.Person.Gender_Gender_Code;
			}
			set
			{
				this.Death.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual Nullable<bool> PersonHasParents
		{
			get
			{
				return this.Death.Person.PersonHasParents;
			}
			set
			{
				this.Death.Person.PersonHasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> PersonHasParentsChanging
		{
			add
			{
				this.Death.Person.PersonHasParentsChanging += value;
			}
			remove
			{
				this.Death.Person.PersonHasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> PersonHasParentsChanged
		{
			add
			{
				this.Death.Person.PersonHasParentsChanged += value;
			}
			remove
			{
				this.Death.Person.PersonHasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Death.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Death.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Death.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Death.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Death.Person.MandatoryUniqueString;
			}
			set
			{
				this.Death.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Death.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Death.Person.MalePerson;
			}
			set
			{
				this.Death.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Death.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Death.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Death.Person.FemalePerson;
			}
			set
			{
				this.Death.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Death.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Death.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Death.Person.ChildPerson;
			}
			set
			{
				this.Death.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Death.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Death.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanged -= value;
			}
		}
		public virtual ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
		{
			get
			{
				return this.Death.Person.PersonDrivesCarAsDrivenByPerson;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateAsBuyer;
			}
		}
		public virtual ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateAsSeller;
			}
		}
		public virtual ICollection<PersonHasNickName> PersonHasNickNameAsPerson
		{
			get
			{
				return this.Death.Person.PersonHasNickNameAsPerson;
			}
		}
		public virtual ICollection<Task> Task
		{
			get
			{
				return this.Death.Person.Task;
			}
		}
		public virtual ICollection<ValueType1> ValueType1DoesSomethingWith
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingWith;
			}
		}
	}
	#endregion // UnnaturalDeath
	#region Task
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class Task : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Task()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				if ((object)this._events == null)
				{
					this._events = new System.Delegate[1];
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Task, Person>> PersonChanging
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
		protected bool RaisePersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<Task, Person>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Task, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Task, Person> eventArgs = new PropertyChangingEventArgs<Task, Person>(this, this.Person, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Task, Person>> PersonChanged
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
		protected void RaisePersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Task, Person>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Task, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Task, Person>(this, oldValue, this.Person), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Person");
			}
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Person Person
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
			return string.Format(provider, "Task{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // Task
	#region ValueType1
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public abstract partial class ValueType1 : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected ValueType1()
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
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<ValueType1, int>> ValueType1ValueChanging
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
		protected bool RaiseValueType1ValueChangingEvent(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<ValueType1, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<ValueType1, int>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<ValueType1, int> eventArgs = new PropertyChangingEventArgs<ValueType1, int>(this, this.ValueType1Value, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ValueType1, int>> ValueType1ValueChanged
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
		protected void RaiseValueType1ValueChangedEvent(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ValueType1, int>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<ValueType1, int>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ValueType1, int>(this, oldValue, this.ValueType1Value), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("ValueType1Value");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ValueType1, Person>> DoesSomethingWithPersonChanging
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
		protected bool RaiseDoesSomethingWithPersonChangingEvent(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<ValueType1, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<ValueType1, Person>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<ValueType1, Person> eventArgs = new PropertyChangingEventArgs<ValueType1, Person>(this, this.DoesSomethingWithPerson, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ValueType1, Person>> DoesSomethingWithPersonChanged
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
		protected void RaiseDoesSomethingWithPersonChangedEvent(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ValueType1, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<ValueType1, Person>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ValueType1, Person>(this, oldValue, this.DoesSomethingWithPerson), new AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("DoesSomethingWithPerson");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract int ValueType1Value
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Person DoesSomethingWithPerson
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract ICollection<Person> DoesSomethingElseWithPerson
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}DoesSomethingWithPerson = {3}{0}}}", Environment.NewLine, "	", this.ValueType1Value, "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // ValueType1
	#region IHasSampleModelContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface IHasSampleModelContext
	{
		SampleModelContext Context
		{
			get;
		}
	}
	#endregion // IHasSampleModelContext
	#region ISampleModelContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface ISampleModelContext
	{
		PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson);
		bool TryGetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson, out PersonDrivesCar PersonDrivesCar);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller);
		bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin);
		bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer);
		bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate);
		Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name);
		bool TryGetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name, out Review Review);
		PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person);
		bool TryGetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person, out PersonHasNickName PersonHasNickName);
		ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother);
		bool TryGetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother, out ChildPerson ChildPerson);
		Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD);
		bool TryGetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD, out Person Person);
		Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD);
		bool TryGetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD, out Person Person);
		Person GetPersonByOptionalUniqueString(string OptionalUniqueString);
		bool TryGetPersonByOptionalUniqueString(string OptionalUniqueString, out Person Person);
		Person GetPersonByOwnsCar_vin(int OwnsCar_vin);
		bool TryGetPersonByOwnsCar_vin(int OwnsCar_vin, out Person Person);
		Person GetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal);
		bool TryGetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal, out Person Person);
		Person GetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal);
		bool TryGetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal, out Person Person);
		Person GetPersonByMandatoryUniqueString(string MandatoryUniqueString);
		bool TryGetPersonByMandatoryUniqueString(string MandatoryUniqueString, out Person Person);
		ValueType1 GetValueType1ByValueType1Value(int ValueType1Value);
		bool TryGetValueType1ByValueType1Value(int ValueType1Value, out ValueType1 ValueType1);
		PersonDrivesCar CreatePersonDrivesCar(int DrivesCar_vin, Person DrivenByPerson);
		ReadOnlyCollection<PersonDrivesCar> PersonDrivesCarCollection
		{
			get;
		}
		PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller);
		ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
		{
			get;
		}
		Review CreateReview(int Car_vin, int Rating_Nr_Integer, string Criterion_Name);
		ReadOnlyCollection<Review> ReviewCollection
		{
			get;
		}
		PersonHasNickName CreatePersonHasNickName(string NickName, Person Person);
		ReadOnlyCollection<PersonHasNickName> PersonHasNickNameCollection
		{
			get;
		}
		Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, decimal MandatoryUniqueDecimal, string MandatoryUniqueString);
		ReadOnlyCollection<Person> PersonCollection
		{
			get;
		}
		MalePerson CreateMalePerson(Person Person);
		ReadOnlyCollection<MalePerson> MalePersonCollection
		{
			get;
		}
		FemalePerson CreateFemalePerson(Person Person);
		ReadOnlyCollection<FemalePerson> FemalePersonCollection
		{
			get;
		}
		ChildPerson CreateChildPerson(int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person);
		ReadOnlyCollection<ChildPerson> ChildPersonCollection
		{
			get;
		}
		Death CreateDeath(string DeathCause_DeathCause_Type, Person Person);
		ReadOnlyCollection<Death> DeathCollection
		{
			get;
		}
		NaturalDeath CreateNaturalDeath(Death Death);
		ReadOnlyCollection<NaturalDeath> NaturalDeathCollection
		{
			get;
		}
		UnnaturalDeath CreateUnnaturalDeath(Death Death);
		ReadOnlyCollection<UnnaturalDeath> UnnaturalDeathCollection
		{
			get;
		}
		Task CreateTask();
		ReadOnlyCollection<Task> TaskCollection
		{
			get;
		}
		ValueType1 CreateValueType1(int ValueType1Value);
		ReadOnlyCollection<ValueType1> ValueType1Collection
		{
			get;
		}
	}
	#endregion // ISampleModelContext
	#region SampleModelContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
	public sealed class SampleModelContext : ISampleModelContext
	{
		public SampleModelContext()
		{
			Dictionary<Type, object> constraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<Type, object>(7);
			Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> constraintEnforcementCollectionCallbacksByTypeAndNameDictionary = new Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object>(2);
			this._ContraintEnforcementCollectionCallbacksByTypeDictionary = constraintEnforcementCollectionCallbacksByTypeDictionary;
			this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = constraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, PersonDrivesCar>), new ConstraintEnforcementCollectionCallbacks<Person, PersonDrivesCar>(new PotentialCollectionModificationCallback<Person, PersonDrivesCar>(this.OnPersonPersonDrivesCarAsDrivenByPersonAdding), new CommittedCollectionModificationCallback<Person, PersonDrivesCar>(this.OnPersonPersonDrivesCarAsDrivenByPersonAdded), null, new CommittedCollectionModificationCallback<Person, PersonDrivesCar>(this.OnPersonPersonDrivesCarAsDrivenByPersonRemoved)));
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>), "PersonBoughtCarFromPersonOnDateAsBuyer"), new ConstraintEnforcementCollectionCallbacks<Person, PersonBoughtCarFromPersonOnDate>(new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded), null, new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved)));
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>), "PersonBoughtCarFromPersonOnDateAsSeller"), new ConstraintEnforcementCollectionCallbacks<Person, PersonBoughtCarFromPersonOnDate>(new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded), null, new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, PersonHasNickName>), new ConstraintEnforcementCollectionCallbacks<Person, PersonHasNickName>(new PotentialCollectionModificationCallback<Person, PersonHasNickName>(this.OnPersonPersonHasNickNameAsPersonAdding), new CommittedCollectionModificationCallback<Person, PersonHasNickName>(this.OnPersonPersonHasNickNameAsPersonAdded), null, new CommittedCollectionModificationCallback<Person, PersonHasNickName>(this.OnPersonPersonHasNickNameAsPersonRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, Task>), new ConstraintEnforcementCollectionCallbacks<Person, Task>(new PotentialCollectionModificationCallback<Person, Task>(this.OnPersonTaskAdding), new CommittedCollectionModificationCallback<Person, Task>(this.OnPersonTaskAdded), null, new CommittedCollectionModificationCallback<Person, Task>(this.OnPersonTaskRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, ValueType1>), new ConstraintEnforcementCollectionCallbacks<Person, ValueType1>(new PotentialCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithAdding), new CommittedCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithAdded), null, new CommittedCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<MalePerson, ChildPerson>), new ConstraintEnforcementCollectionCallbacks<MalePerson, ChildPerson>(new PotentialCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonAdding), new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonAdded), null, new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<FemalePerson, ChildPerson>), new ConstraintEnforcementCollectionCallbacks<FemalePerson, ChildPerson>(new PotentialCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonAdding), new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonAdded), null, new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<ValueType1, Person>), new ConstraintEnforcementCollectionCallbacks<ValueType1, Person>(new PotentialCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonAdding), new CommittedCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonAdded), null, new CommittedCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonRemoved)));
			List<PersonDrivesCar> PersonDrivesCarList = new List<PersonDrivesCar>();
			this._PersonDrivesCarList = PersonDrivesCarList;
			this._PersonDrivesCarReadOnlyCollection = new ReadOnlyCollection<PersonDrivesCar>(PersonDrivesCarList);
			List<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateList = new List<PersonBoughtCarFromPersonOnDate>();
			this._PersonBoughtCarFromPersonOnDateList = PersonBoughtCarFromPersonOnDateList;
			this._PersonBoughtCarFromPersonOnDateReadOnlyCollection = new ReadOnlyCollection<PersonBoughtCarFromPersonOnDate>(PersonBoughtCarFromPersonOnDateList);
			List<Review> ReviewList = new List<Review>();
			this._ReviewList = ReviewList;
			this._ReviewReadOnlyCollection = new ReadOnlyCollection<Review>(ReviewList);
			List<PersonHasNickName> PersonHasNickNameList = new List<PersonHasNickName>();
			this._PersonHasNickNameList = PersonHasNickNameList;
			this._PersonHasNickNameReadOnlyCollection = new ReadOnlyCollection<PersonHasNickName>(PersonHasNickNameList);
			List<Person> PersonList = new List<Person>();
			this._PersonList = PersonList;
			this._PersonReadOnlyCollection = new ReadOnlyCollection<Person>(PersonList);
			List<MalePerson> MalePersonList = new List<MalePerson>();
			this._MalePersonList = MalePersonList;
			this._MalePersonReadOnlyCollection = new ReadOnlyCollection<MalePerson>(MalePersonList);
			List<FemalePerson> FemalePersonList = new List<FemalePerson>();
			this._FemalePersonList = FemalePersonList;
			this._FemalePersonReadOnlyCollection = new ReadOnlyCollection<FemalePerson>(FemalePersonList);
			List<ChildPerson> ChildPersonList = new List<ChildPerson>();
			this._ChildPersonList = ChildPersonList;
			this._ChildPersonReadOnlyCollection = new ReadOnlyCollection<ChildPerson>(ChildPersonList);
			List<Death> DeathList = new List<Death>();
			this._DeathList = DeathList;
			this._DeathReadOnlyCollection = new ReadOnlyCollection<Death>(DeathList);
			List<NaturalDeath> NaturalDeathList = new List<NaturalDeath>();
			this._NaturalDeathList = NaturalDeathList;
			this._NaturalDeathReadOnlyCollection = new ReadOnlyCollection<NaturalDeath>(NaturalDeathList);
			List<UnnaturalDeath> UnnaturalDeathList = new List<UnnaturalDeath>();
			this._UnnaturalDeathList = UnnaturalDeathList;
			this._UnnaturalDeathReadOnlyCollection = new ReadOnlyCollection<UnnaturalDeath>(UnnaturalDeathList);
			List<Task> TaskList = new List<Task>();
			this._TaskList = TaskList;
			this._TaskReadOnlyCollection = new ReadOnlyCollection<Task>(TaskList);
			List<ValueType1> ValueType1List = new List<ValueType1>();
			this._ValueType1List = ValueType1List;
			this._ValueType1ReadOnlyCollection = new ReadOnlyCollection<ValueType1>(ValueType1List);
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
		private readonly Dictionary<Tuple<int, Person>, PersonDrivesCar> _InternalUniquenessConstraint18Dictionary = new Dictionary<Tuple<int, Person>, PersonDrivesCar>();
		public PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson)
		{
			return this._InternalUniquenessConstraint18Dictionary[Tuple.CreateTuple<int, Person>(DrivesCar_vin, DrivenByPerson)];
		}
		public bool TryGetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson, out PersonDrivesCar PersonDrivesCar)
		{
			return this._InternalUniquenessConstraint18Dictionary.TryGetValue(Tuple.CreateTuple<int, Person>(DrivesCar_vin, DrivenByPerson), out PersonDrivesCar);
		}
		private bool OnInternalUniquenessConstraint18Changing(PersonDrivesCar instance, Tuple<int, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonDrivesCar currentInstance;
				if (this._InternalUniquenessConstraint18Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint18Changed(PersonDrivesCar instance, Tuple<int, Person> oldValue, Tuple<int, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint18Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint18Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<Person, int, Person>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint23Dictionary = new Dictionary<Tuple<Person, int, Person>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller)
		{
			return this._InternalUniquenessConstraint23Dictionary[Tuple.CreateTuple<Person, int, Person>(Buyer, CarSold_vin, Seller)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate)
		{
			return this._InternalUniquenessConstraint23Dictionary.TryGetValue(Tuple.CreateTuple<Person, int, Person>(Buyer, CarSold_vin, Seller), out PersonBoughtCarFromPersonOnDate);
		}
		private bool OnInternalUniquenessConstraint23Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, int, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._InternalUniquenessConstraint23Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint23Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, int, Person> oldValue, Tuple<Person, int, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint23Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint23Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<int, Person, int>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint24Dictionary = new Dictionary<Tuple<int, Person, int>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin)
		{
			return this._InternalUniquenessConstraint24Dictionary[Tuple.CreateTuple<int, Person, int>(SaleDate_YMD, Seller, CarSold_vin)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate)
		{
			return this._InternalUniquenessConstraint24Dictionary.TryGetValue(Tuple.CreateTuple<int, Person, int>(SaleDate_YMD, Seller, CarSold_vin), out PersonBoughtCarFromPersonOnDate);
		}
		private bool OnInternalUniquenessConstraint24Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<int, Person, int> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._InternalUniquenessConstraint24Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint24Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<int, Person, int> oldValue, Tuple<int, Person, int> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint24Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint24Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<int, int, Person>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint25Dictionary = new Dictionary<Tuple<int, int, Person>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer)
		{
			return this._InternalUniquenessConstraint25Dictionary[Tuple.CreateTuple<int, int, Person>(CarSold_vin, SaleDate_YMD, Buyer)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate)
		{
			return this._InternalUniquenessConstraint25Dictionary.TryGetValue(Tuple.CreateTuple<int, int, Person>(CarSold_vin, SaleDate_YMD, Buyer), out PersonBoughtCarFromPersonOnDate);
		}
		private bool OnInternalUniquenessConstraint25Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<int, int, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._InternalUniquenessConstraint25Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint25Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<int, int, Person> oldValue, Tuple<int, int, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint25Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint25Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<int, string>, Review> _InternalUniquenessConstraint26Dictionary = new Dictionary<Tuple<int, string>, Review>();
		public Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name)
		{
			return this._InternalUniquenessConstraint26Dictionary[Tuple.CreateTuple<int, string>(Car_vin, Criterion_Name)];
		}
		public bool TryGetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name, out Review Review)
		{
			return this._InternalUniquenessConstraint26Dictionary.TryGetValue(Tuple.CreateTuple<int, string>(Car_vin, Criterion_Name), out Review);
		}
		private bool OnInternalUniquenessConstraint26Changing(Review instance, Tuple<int, string> newValue)
		{
			if ((object)newValue != null)
			{
				Review currentInstance;
				if (this._InternalUniquenessConstraint26Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint26Changed(Review instance, Tuple<int, string> oldValue, Tuple<int, string> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint26Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint26Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, Person>, PersonHasNickName> _InternalUniquenessConstraint33Dictionary = new Dictionary<Tuple<string, Person>, PersonHasNickName>();
		public PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person)
		{
			return this._InternalUniquenessConstraint33Dictionary[Tuple.CreateTuple<string, Person>(NickName, Person)];
		}
		public bool TryGetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person, out PersonHasNickName PersonHasNickName)
		{
			return this._InternalUniquenessConstraint33Dictionary.TryGetValue(Tuple.CreateTuple<string, Person>(NickName, Person), out PersonHasNickName);
		}
		private bool OnInternalUniquenessConstraint33Changing(PersonHasNickName instance, Tuple<string, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonHasNickName currentInstance;
				if (this._InternalUniquenessConstraint33Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint33Changed(PersonHasNickName instance, Tuple<string, Person> oldValue, Tuple<string, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint33Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint33Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<MalePerson, int, FemalePerson>, ChildPerson> _ExternalUniquenessConstraint3Dictionary = new Dictionary<Tuple<MalePerson, int, FemalePerson>, ChildPerson>();
		public ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother)
		{
			return this._ExternalUniquenessConstraint3Dictionary[Tuple.CreateTuple<MalePerson, int, FemalePerson>(Father, BirthOrder_BirthOrder_Nr, Mother)];
		}
		public bool TryGetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother, out ChildPerson ChildPerson)
		{
			return this._ExternalUniquenessConstraint3Dictionary.TryGetValue(Tuple.CreateTuple<MalePerson, int, FemalePerson>(Father, BirthOrder_BirthOrder_Nr, Mother), out ChildPerson);
		}
		private bool OnExternalUniquenessConstraint3Changing(ChildPerson instance, Tuple<MalePerson, int, FemalePerson> newValue)
		{
			if ((object)newValue != null)
			{
				ChildPerson currentInstance;
				if (this._ExternalUniquenessConstraint3Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnExternalUniquenessConstraint3Changed(ChildPerson instance, Tuple<MalePerson, int, FemalePerson> oldValue, Tuple<MalePerson, int, FemalePerson> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ExternalUniquenessConstraint3Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ExternalUniquenessConstraint3Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, int>, Person> _ExternalUniquenessConstraint1Dictionary = new Dictionary<Tuple<string, int>, Person>();
		public Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD)
		{
			return this._ExternalUniquenessConstraint1Dictionary[Tuple.CreateTuple<string, int>(FirstName, Date_YMD)];
		}
		public bool TryGetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD, out Person Person)
		{
			return this._ExternalUniquenessConstraint1Dictionary.TryGetValue(Tuple.CreateTuple<string, int>(FirstName, Date_YMD), out Person);
		}
		private bool OnExternalUniquenessConstraint1Changing(Person instance, Tuple<string, int> newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._ExternalUniquenessConstraint1Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnExternalUniquenessConstraint1Changed(Person instance, Tuple<string, int> oldValue, Tuple<string, int> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ExternalUniquenessConstraint1Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ExternalUniquenessConstraint1Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, int>, Person> _ExternalUniquenessConstraint2Dictionary = new Dictionary<Tuple<string, int>, Person>();
		public Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD)
		{
			return this._ExternalUniquenessConstraint2Dictionary[Tuple.CreateTuple<string, int>(LastName, Date_YMD)];
		}
		public bool TryGetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD, out Person Person)
		{
			return this._ExternalUniquenessConstraint2Dictionary.TryGetValue(Tuple.CreateTuple<string, int>(LastName, Date_YMD), out Person);
		}
		private bool OnExternalUniquenessConstraint2Changing(Person instance, Tuple<string, int> newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._ExternalUniquenessConstraint2Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnExternalUniquenessConstraint2Changed(Person instance, Tuple<string, int> oldValue, Tuple<string, int> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ExternalUniquenessConstraint2Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ExternalUniquenessConstraint2Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<string, Person> _PersonOptionalUniqueStringDictionary = new Dictionary<string, Person>();
		public Person GetPersonByOptionalUniqueString(string OptionalUniqueString)
		{
			return this._PersonOptionalUniqueStringDictionary[OptionalUniqueString];
		}
		public bool TryGetPersonByOptionalUniqueString(string OptionalUniqueString, out Person Person)
		{
			return this._PersonOptionalUniqueStringDictionary.TryGetValue(OptionalUniqueString, out Person);
		}
		private readonly Dictionary<int, Person> _PersonOwnsCar_vinDictionary = new Dictionary<int, Person>();
		public Person GetPersonByOwnsCar_vin(int OwnsCar_vin)
		{
			return this._PersonOwnsCar_vinDictionary[OwnsCar_vin];
		}
		public bool TryGetPersonByOwnsCar_vin(int OwnsCar_vin, out Person Person)
		{
			return this._PersonOwnsCar_vinDictionary.TryGetValue(OwnsCar_vin, out Person);
		}
		private readonly Dictionary<decimal, Person> _PersonOptionalUniqueDecimalDictionary = new Dictionary<decimal, Person>();
		public Person GetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal)
		{
			return this._PersonOptionalUniqueDecimalDictionary[OptionalUniqueDecimal];
		}
		public bool TryGetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal, out Person Person)
		{
			return this._PersonOptionalUniqueDecimalDictionary.TryGetValue(OptionalUniqueDecimal, out Person);
		}
		private readonly Dictionary<decimal, Person> _PersonMandatoryUniqueDecimalDictionary = new Dictionary<decimal, Person>();
		public Person GetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal)
		{
			return this._PersonMandatoryUniqueDecimalDictionary[MandatoryUniqueDecimal];
		}
		public bool TryGetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal, out Person Person)
		{
			return this._PersonMandatoryUniqueDecimalDictionary.TryGetValue(MandatoryUniqueDecimal, out Person);
		}
		private readonly Dictionary<string, Person> _PersonMandatoryUniqueStringDictionary = new Dictionary<string, Person>();
		public Person GetPersonByMandatoryUniqueString(string MandatoryUniqueString)
		{
			return this._PersonMandatoryUniqueStringDictionary[MandatoryUniqueString];
		}
		public bool TryGetPersonByMandatoryUniqueString(string MandatoryUniqueString, out Person Person)
		{
			return this._PersonMandatoryUniqueStringDictionary.TryGetValue(MandatoryUniqueString, out Person);
		}
		private readonly Dictionary<int, ValueType1> _ValueType1ValueType1ValueDictionary = new Dictionary<int, ValueType1>();
		public ValueType1 GetValueType1ByValueType1Value(int ValueType1Value)
		{
			return this._ValueType1ValueType1ValueDictionary[ValueType1Value];
		}
		public bool TryGetValueType1ByValueType1Value(int ValueType1Value, out ValueType1 ValueType1)
		{
			return this._ValueType1ValueType1ValueDictionary.TryGetValue(ValueType1Value, out ValueType1);
		}
		#endregion // Lookup and External Constraint Enforcement
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext;
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionCallbacks<TClass, TProperty>
			where TClass : class, IHasSampleModelContext
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
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private bool OnAdding<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private void OnAdded<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private void OnAdded<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private bool OnRemoving<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private bool OnRemoving<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private void OnRemoved<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		private void OnRemoved<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ConstraintEnforcementCollection<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasSampleModelContext
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
			where TClass : class, IHasSampleModelContext
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
		#region PersonDrivesCar
		public PersonDrivesCar CreatePersonDrivesCar(int DrivesCar_vin, Person DrivenByPerson)
		{
			if ((object)DrivenByPerson == null)
			{
				throw new ArgumentNullException("DrivenByPerson");
			}
			if (!(this.OnPersonDrivesCarDrivesCar_vinChanging(null, DrivesCar_vin)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("DrivesCar_vin");
			}
			if (!(this.OnPersonDrivesCarDrivenByPersonChanging(null, DrivenByPerson)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("DrivenByPerson");
			}
			return new PersonDrivesCarCore(this, DrivesCar_vin, DrivenByPerson);
		}
		private bool OnPersonDrivesCarDrivesCar_vinChanging(PersonDrivesCar instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple<int, Person>(newValue, instance.DrivenByPerson))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonDrivesCarDrivesCar_vinChanged(PersonDrivesCar instance, Nullable<int> oldValue)
		{
			Tuple<int, Person> InternalUniquenessConstraint18OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple<int, Person>(oldValue.Value, instance.DrivenByPerson);
			}
			else
			{
				InternalUniquenessConstraint18OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple<int, Person>(instance.DrivesCar_vin, instance.DrivenByPerson));
		}
		private bool OnPersonDrivesCarDrivenByPersonChanging(PersonDrivesCar instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple<int, Person>(instance.DrivesCar_vin, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonDrivesCarDrivenByPersonChanged(PersonDrivesCar instance, Person oldValue)
		{
			instance.DrivenByPerson.PersonDrivesCarAsDrivenByPerson.Add(instance);
			Tuple<int, Person> InternalUniquenessConstraint18OldValueTuple;
			if ((object)oldValue != null)
			{
				oldValue.PersonDrivesCarAsDrivenByPerson.Remove(instance);
				InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple<int, Person>(instance.DrivesCar_vin, oldValue);
			}
			else
			{
				InternalUniquenessConstraint18OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple<int, Person>(instance.DrivesCar_vin, instance.DrivenByPerson));
		}
		private readonly List<PersonDrivesCar> _PersonDrivesCarList;
		private readonly ReadOnlyCollection<PersonDrivesCar> _PersonDrivesCarReadOnlyCollection;
		public ReadOnlyCollection<PersonDrivesCar> PersonDrivesCarCollection
		{
			get
			{
				return this._PersonDrivesCarReadOnlyCollection;
			}
		}
		#region PersonDrivesCarCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class PersonDrivesCarCore : PersonDrivesCar
		{
			public PersonDrivesCarCore(SampleModelContext context, int DrivesCar_vin, Person DrivenByPerson)
			{
				this._Context = context;
				this._DrivesCar_vin = DrivesCar_vin;
				context.OnPersonDrivesCarDrivesCar_vinChanged(this, null);
				this._DrivenByPerson = DrivenByPerson;
				context.OnPersonDrivesCarDrivenByPersonChanged(this, null);
				context._PersonDrivesCarList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("DrivesCar_vin")]
			private int _DrivesCar_vin;
			public override int DrivesCar_vin
			{
				get
				{
					return this._DrivesCar_vin;
				}
				set
				{
					int oldValue = this._DrivesCar_vin;
					if (oldValue != value)
					{
						if (this._Context.OnPersonDrivesCarDrivesCar_vinChanging(this, value) && base.RaiseDrivesCar_vinChangingEvent(value))
						{
							this._DrivesCar_vin = value;
							this._Context.OnPersonDrivesCarDrivesCar_vinChanged(this, oldValue);
							base.RaiseDrivesCar_vinChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("DrivenByPerson")]
			private Person _DrivenByPerson;
			public override Person DrivenByPerson
			{
				get
				{
					return this._DrivenByPerson;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._DrivenByPerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonDrivesCarDrivenByPersonChanging(this, value) && base.RaiseDrivenByPersonChangingEvent(value))
						{
							this._DrivenByPerson = value;
							this._Context.OnPersonDrivesCarDrivenByPersonChanged(this, oldValue);
							base.RaiseDrivenByPersonChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonDrivesCarCore
		#endregion // PersonDrivesCar
		#region PersonBoughtCarFromPersonOnDate
		public PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller)
		{
			if ((object)Buyer == null)
			{
				throw new ArgumentNullException("Buyer");
			}
			if ((object)Seller == null)
			{
				throw new ArgumentNullException("Seller");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(null, CarSold_vin)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("CarSold_vin");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(null, SaleDate_YMD)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("SaleDate_YMD");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateBuyerChanging(null, Buyer)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Buyer");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateSellerChanging(null, Seller)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Seller");
			}
			return new PersonBoughtCarFromPersonOnDateCore(this, CarSold_vin, SaleDate_YMD, Buyer, Seller);
		}
		private bool OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(PersonBoughtCarFromPersonOnDate instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, newValue, instance.Seller))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, newValue))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple<int, int, Person>(newValue, instance.SaleDate_YMD, instance.Buyer))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(PersonBoughtCarFromPersonOnDate instance, Nullable<int> oldValue)
		{
			Tuple<Person, int, Person> InternalUniquenessConstraint23OldValueTuple;
			Tuple<int, Person, int> InternalUniquenessConstraint24OldValueTuple;
			Tuple<int, int, Person> InternalUniquenessConstraint25OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple<Person, int, Person>(instance.Buyer, oldValue.Value, instance.Seller);
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, oldValue.Value);
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple<int, int, Person>(oldValue.Value, instance.SaleDate_YMD, instance.Buyer);
			}
			else
			{
				InternalUniquenessConstraint23OldValueTuple = null;
				InternalUniquenessConstraint24OldValueTuple = null;
				InternalUniquenessConstraint25OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, instance.Seller));
			this.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
			this.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(PersonBoughtCarFromPersonOnDate instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple<int, Person, int>(newValue, instance.Seller, instance.CarSold_vin))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, newValue, instance.Buyer))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(PersonBoughtCarFromPersonOnDate instance, Nullable<int> oldValue)
		{
			Tuple<int, Person, int> InternalUniquenessConstraint24OldValueTuple;
			Tuple<int, int, Person> InternalUniquenessConstraint25OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple<int, Person, int>(oldValue.Value, instance.Seller, instance.CarSold_vin);
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, oldValue.Value, instance.Buyer);
			}
			else
			{
				InternalUniquenessConstraint24OldValueTuple = null;
				InternalUniquenessConstraint25OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
			this.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateBuyerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple<Person, int, Person>(newValue, instance.CarSold_vin, instance.Seller))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateBuyerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
		{
			instance.Buyer.PersonBoughtCarFromPersonOnDateAsBuyer.Add(instance);
			Tuple<Person, int, Person> InternalUniquenessConstraint23OldValueTuple;
			Tuple<int, int, Person> InternalUniquenessConstraint25OldValueTuple;
			if ((object)oldValue != null)
			{
				oldValue.PersonBoughtCarFromPersonOnDateAsBuyer.Remove(instance);
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple<Person, int, Person>(oldValue, instance.CarSold_vin, instance.Seller);
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, oldValue);
			}
			else
			{
				InternalUniquenessConstraint23OldValueTuple = null;
				InternalUniquenessConstraint25OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, instance.Seller));
			this.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateSellerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, newValue))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, newValue, instance.CarSold_vin))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateSellerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
		{
			instance.Seller.PersonBoughtCarFromPersonOnDateAsSeller.Add(instance);
			Tuple<Person, int, Person> InternalUniquenessConstraint23OldValueTuple;
			Tuple<int, Person, int> InternalUniquenessConstraint24OldValueTuple;
			if ((object)oldValue != null)
			{
				oldValue.PersonBoughtCarFromPersonOnDateAsSeller.Remove(instance);
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, oldValue);
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, oldValue, instance.CarSold_vin);
			}
			else
			{
				InternalUniquenessConstraint23OldValueTuple = null;
				InternalUniquenessConstraint24OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, instance.Seller));
			this.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
		}
		private readonly List<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateList;
		private readonly ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateReadOnlyCollection;
		public ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
		{
			get
			{
				return this._PersonBoughtCarFromPersonOnDateReadOnlyCollection;
			}
		}
		#region PersonBoughtCarFromPersonOnDateCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class PersonBoughtCarFromPersonOnDateCore : PersonBoughtCarFromPersonOnDate
		{
			public PersonBoughtCarFromPersonOnDateCore(SampleModelContext context, int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller)
			{
				this._Context = context;
				this._CarSold_vin = CarSold_vin;
				context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(this, null);
				this._SaleDate_YMD = SaleDate_YMD;
				context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(this, null);
				this._Buyer = Buyer;
				context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, null);
				this._Seller = Seller;
				context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, null);
				context._PersonBoughtCarFromPersonOnDateList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("CarSold_vin")]
			private int _CarSold_vin;
			public override int CarSold_vin
			{
				get
				{
					return this._CarSold_vin;
				}
				set
				{
					int oldValue = this._CarSold_vin;
					if (oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(this, value) && base.RaiseCarSold_vinChangingEvent(value))
						{
							this._CarSold_vin = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(this, oldValue);
							base.RaiseCarSold_vinChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("SaleDate_YMD")]
			private int _SaleDate_YMD;
			public override int SaleDate_YMD
			{
				get
				{
					return this._SaleDate_YMD;
				}
				set
				{
					int oldValue = this._SaleDate_YMD;
					if (oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(this, value) && base.RaiseSaleDate_YMDChangingEvent(value))
						{
							this._SaleDate_YMD = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(this, oldValue);
							base.RaiseSaleDate_YMDChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Buyer")]
			private Person _Buyer;
			public override Person Buyer
			{
				get
				{
					return this._Buyer;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Buyer;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateBuyerChanging(this, value) && base.RaiseBuyerChangingEvent(value))
						{
							this._Buyer = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, oldValue);
							base.RaiseBuyerChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Seller")]
			private Person _Seller;
			public override Person Seller
			{
				get
				{
					return this._Seller;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Seller;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateSellerChanging(this, value) && base.RaiseSellerChangingEvent(value))
						{
							this._Seller = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, oldValue);
							base.RaiseSellerChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDateCore
		#endregion // PersonBoughtCarFromPersonOnDate
		#region Review
		public Review CreateReview(int Car_vin, int Rating_Nr_Integer, string Criterion_Name)
		{
			if ((object)Criterion_Name == null)
			{
				throw new ArgumentNullException("Criterion_Name");
			}
			if (!(this.OnReviewCar_vinChanging(null, Car_vin)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Car_vin");
			}
			if (!(this.OnReviewRating_Nr_IntegerChanging(null, Rating_Nr_Integer)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Rating_Nr_Integer");
			}
			if (!(this.OnReviewCriterion_NameChanging(null, Criterion_Name)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Criterion_Name");
			}
			return new ReviewCore(this, Car_vin, Rating_Nr_Integer, Criterion_Name);
		}
		private bool OnReviewCar_vinChanging(Review instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple<int, string>(newValue, instance.Criterion_Name))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnReviewCar_vinChanged(Review instance, Nullable<int> oldValue)
		{
			Tuple<int, string> InternalUniquenessConstraint26OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple<int, string>(oldValue.Value, instance.Criterion_Name);
			}
			else
			{
				InternalUniquenessConstraint26OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple<int, string>(instance.Car_vin, instance.Criterion_Name));
		}
		private bool OnReviewRating_Nr_IntegerChanging(Review instance, int newValue)
		{
			return true;
		}
		private bool OnReviewCriterion_NameChanging(Review instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple<int, string>(instance.Car_vin, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnReviewCriterion_NameChanged(Review instance, string oldValue)
		{
			Tuple<int, string> InternalUniquenessConstraint26OldValueTuple;
			if ((object)oldValue != null)
			{
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple<int, string>(instance.Car_vin, oldValue);
			}
			else
			{
				InternalUniquenessConstraint26OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple<int, string>(instance.Car_vin, instance.Criterion_Name));
		}
		private readonly List<Review> _ReviewList;
		private readonly ReadOnlyCollection<Review> _ReviewReadOnlyCollection;
		public ReadOnlyCollection<Review> ReviewCollection
		{
			get
			{
				return this._ReviewReadOnlyCollection;
			}
		}
		#region ReviewCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ReviewCore : Review
		{
			public ReviewCore(SampleModelContext context, int Car_vin, int Rating_Nr_Integer, string Criterion_Name)
			{
				this._Context = context;
				this._Car_vin = Car_vin;
				context.OnReviewCar_vinChanged(this, null);
				this._Rating_Nr_Integer = Rating_Nr_Integer;
				this._Criterion_Name = Criterion_Name;
				context.OnReviewCriterion_NameChanged(this, null);
				context._ReviewList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("Car_vin")]
			private int _Car_vin;
			public override int Car_vin
			{
				get
				{
					return this._Car_vin;
				}
				set
				{
					int oldValue = this._Car_vin;
					if (oldValue != value)
					{
						if (this._Context.OnReviewCar_vinChanging(this, value) && base.RaiseCar_vinChangingEvent(value))
						{
							this._Car_vin = value;
							this._Context.OnReviewCar_vinChanged(this, oldValue);
							base.RaiseCar_vinChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Rating_Nr_Integer")]
			private int _Rating_Nr_Integer;
			public override int Rating_Nr_Integer
			{
				get
				{
					return this._Rating_Nr_Integer;
				}
				set
				{
					int oldValue = this._Rating_Nr_Integer;
					if (oldValue != value)
					{
						if (this._Context.OnReviewRating_Nr_IntegerChanging(this, value) && base.RaiseRating_Nr_IntegerChangingEvent(value))
						{
							this._Rating_Nr_Integer = value;
							base.RaiseRating_Nr_IntegerChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Criterion_Name")]
			private string _Criterion_Name;
			public override string Criterion_Name
			{
				get
				{
					return this._Criterion_Name;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Criterion_Name;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnReviewCriterion_NameChanging(this, value) && base.RaiseCriterion_NameChangingEvent(value))
						{
							this._Criterion_Name = value;
							this._Context.OnReviewCriterion_NameChanged(this, oldValue);
							base.RaiseCriterion_NameChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // ReviewCore
		#endregion // Review
		#region PersonHasNickName
		public PersonHasNickName CreatePersonHasNickName(string NickName, Person Person)
		{
			if ((object)NickName == null)
			{
				throw new ArgumentNullException("NickName");
			}
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnPersonHasNickNameNickNameChanging(null, NickName)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("NickName");
			}
			if (!(this.OnPersonHasNickNamePersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new PersonHasNickNameCore(this, NickName, Person);
		}
		private bool OnPersonHasNickNameNickNameChanging(PersonHasNickName instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple<string, Person>(newValue, instance.Person))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonHasNickNameNickNameChanged(PersonHasNickName instance, string oldValue)
		{
			Tuple<string, Person> InternalUniquenessConstraint33OldValueTuple;
			if ((object)oldValue != null)
			{
				InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple<string, Person>(oldValue, instance.Person);
			}
			else
			{
				InternalUniquenessConstraint33OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple<string, Person>(instance.NickName, instance.Person));
		}
		private bool OnPersonHasNickNamePersonChanging(PersonHasNickName instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple<string, Person>(instance.NickName, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonHasNickNamePersonChanged(PersonHasNickName instance, Person oldValue)
		{
			instance.Person.PersonHasNickNameAsPerson.Add(instance);
			Tuple<string, Person> InternalUniquenessConstraint33OldValueTuple;
			if ((object)oldValue != null)
			{
				oldValue.PersonHasNickNameAsPerson.Remove(instance);
				InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple<string, Person>(instance.NickName, oldValue);
			}
			else
			{
				InternalUniquenessConstraint33OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple<string, Person>(instance.NickName, instance.Person));
		}
		private readonly List<PersonHasNickName> _PersonHasNickNameList;
		private readonly ReadOnlyCollection<PersonHasNickName> _PersonHasNickNameReadOnlyCollection;
		public ReadOnlyCollection<PersonHasNickName> PersonHasNickNameCollection
		{
			get
			{
				return this._PersonHasNickNameReadOnlyCollection;
			}
		}
		#region PersonHasNickNameCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class PersonHasNickNameCore : PersonHasNickName
		{
			public PersonHasNickNameCore(SampleModelContext context, string NickName, Person Person)
			{
				this._Context = context;
				this._NickName = NickName;
				context.OnPersonHasNickNameNickNameChanged(this, null);
				this._Person = Person;
				context.OnPersonHasNickNamePersonChanged(this, null);
				context._PersonHasNickNameList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("NickName")]
			private string _NickName;
			public override string NickName
			{
				get
				{
					return this._NickName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._NickName;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonHasNickNameNickNameChanging(this, value) && base.RaiseNickNameChangingEvent(value))
						{
							this._NickName = value;
							this._Context.OnPersonHasNickNameNickNameChanged(this, oldValue);
							base.RaiseNickNameChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonHasNickNamePersonChanging(this, value) && base.RaisePersonChangingEvent(value))
						{
							this._Person = value;
							this._Context.OnPersonHasNickNamePersonChanged(this, oldValue);
							base.RaisePersonChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonHasNickNameCore
		#endregion // PersonHasNickName
		#region Person
		public Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, decimal MandatoryUniqueDecimal, string MandatoryUniqueString)
		{
			if ((object)FirstName == null)
			{
				throw new ArgumentNullException("FirstName");
			}
			if ((object)LastName == null)
			{
				throw new ArgumentNullException("LastName");
			}
			if ((object)Gender_Gender_Code == null)
			{
				throw new ArgumentNullException("Gender_Gender_Code");
			}
			if ((object)MandatoryUniqueString == null)
			{
				throw new ArgumentNullException("MandatoryUniqueString");
			}
			if (!(this.OnPersonFirstNameChanging(null, FirstName)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("FirstName");
			}
			if (!(this.OnPersonDate_YMDChanging(null, Date_YMD)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Date_YMD");
			}
			if (!(this.OnPersonLastNameChanging(null, LastName)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("LastName");
			}
			if (!(this.OnPersonGender_Gender_CodeChanging(null, Gender_Gender_Code)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Gender_Gender_Code");
			}
			if (!(this.OnPersonMandatoryUniqueDecimalChanging(null, MandatoryUniqueDecimal)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("MandatoryUniqueDecimal");
			}
			if (!(this.OnPersonMandatoryUniqueStringChanging(null, MandatoryUniqueString)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("MandatoryUniqueString");
			}
			return new PersonCore(this, FirstName, Date_YMD, LastName, Gender_Gender_Code, MandatoryUniqueDecimal, MandatoryUniqueString);
		}
		private bool OnPersonFirstNameChanging(Person instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple<string, int>(newValue, instance.Date_YMD))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonFirstNameChanged(Person instance, string oldValue)
		{
			Tuple<string, int> ExternalUniquenessConstraint1OldValueTuple;
			if ((object)oldValue != null)
			{
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple<string, int>(oldValue, instance.Date_YMD);
			}
			else
			{
				ExternalUniquenessConstraint1OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple<string, int>(instance.FirstName, instance.Date_YMD));
		}
		private bool OnPersonDate_YMDChanging(Person instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple<string, int>(instance.FirstName, newValue))))
				{
					return false;
				}
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple<string, int>(instance.LastName, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonDate_YMDChanged(Person instance, Nullable<int> oldValue)
		{
			Tuple<string, int> ExternalUniquenessConstraint1OldValueTuple;
			Tuple<string, int> ExternalUniquenessConstraint2OldValueTuple;
			if (oldValue.HasValue)
			{
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple<string, int>(instance.FirstName, oldValue.Value);
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple<string, int>(instance.LastName, oldValue.Value);
			}
			else
			{
				ExternalUniquenessConstraint1OldValueTuple = null;
				ExternalUniquenessConstraint2OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple<string, int>(instance.FirstName, instance.Date_YMD));
			this.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple<string, int>(instance.LastName, instance.Date_YMD));
		}
		private bool OnPersonLastNameChanging(Person instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple<string, int>(newValue, instance.Date_YMD))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonLastNameChanged(Person instance, string oldValue)
		{
			Tuple<string, int> ExternalUniquenessConstraint2OldValueTuple;
			if ((object)oldValue != null)
			{
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple<string, int>(oldValue, instance.Date_YMD);
			}
			else
			{
				ExternalUniquenessConstraint2OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple<string, int>(instance.LastName, instance.Date_YMD));
		}
		private bool OnPersonOptionalUniqueStringChanging(Person instance, string newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueStringDictionary.TryGetValue(newValue, out currentInstance))
				{
					if ((object)currentInstance != instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOptionalUniqueStringChanged(Person instance, string oldValue)
		{
			if ((object)instance.OptionalUniqueString != null)
			{
				this._PersonOptionalUniqueStringDictionary.Add(instance.OptionalUniqueString, instance);
			}
			if ((object)oldValue != null)
			{
				this._PersonOptionalUniqueStringDictionary.Remove(oldValue);
			}
		}
		private bool OnPersonHatType_ColorARGBChanging(Person instance, Nullable<int> newValue)
		{
			return true;
		}
		private bool OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonOwnsCar_vinChanging(Person instance, Nullable<int> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonOwnsCar_vinDictionary.TryGetValue(newValue.Value, out currentInstance))
				{
					if ((object)currentInstance != instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOwnsCar_vinChanged(Person instance, Nullable<int> oldValue)
		{
			if (instance.OwnsCar_vin.HasValue)
			{
				this._PersonOwnsCar_vinDictionary.Add(instance.OwnsCar_vin.Value, instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonOwnsCar_vinDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnPersonGender_Gender_CodeChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonPersonHasParentsChanging(Person instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnPersonOptionalUniqueDecimalChanging(Person instance, Nullable<decimal> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueDecimalDictionary.TryGetValue(newValue.Value, out currentInstance))
				{
					if ((object)currentInstance != instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOptionalUniqueDecimalChanged(Person instance, Nullable<decimal> oldValue)
		{
			if (instance.OptionalUniqueDecimal.HasValue)
			{
				this._PersonOptionalUniqueDecimalDictionary.Add(instance.OptionalUniqueDecimal.Value, instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonOptionalUniqueDecimalDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnPersonMandatoryUniqueDecimalChanging(Person instance, decimal newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueDecimalDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonMandatoryUniqueDecimalChanged(Person instance, Nullable<decimal> oldValue)
		{
			this._PersonMandatoryUniqueDecimalDictionary.Add(instance.MandatoryUniqueDecimal, instance);
			if (oldValue.HasValue)
			{
				this._PersonMandatoryUniqueDecimalDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnPersonMandatoryUniqueStringChanging(Person instance, string newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueStringDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonMandatoryUniqueStringChanged(Person instance, string oldValue)
		{
			this._PersonMandatoryUniqueStringDictionary.Add(instance.MandatoryUniqueString, instance);
			if ((object)oldValue != null)
			{
				this._PersonMandatoryUniqueStringDictionary.Remove(oldValue);
			}
		}
		private bool OnPersonValueType1DoesSomethingElseWithChanging(Person instance, ValueType1 newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonValueType1DoesSomethingElseWithChanged(Person instance, ValueType1 oldValue)
		{
			if ((object)instance.ValueType1DoesSomethingElseWith != null)
			{
				instance.ValueType1DoesSomethingElseWith.DoesSomethingElseWithPerson.Add(instance);
			}
			if ((object)oldValue != null)
			{
				oldValue.DoesSomethingElseWithPerson.Remove(instance);
			}
		}
		private bool OnPersonMalePersonChanging(Person instance, MalePerson newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonMalePersonChanged(Person instance, MalePerson oldValue)
		{
			if ((object)instance.MalePerson != null)
			{
				instance.MalePerson.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonFemalePersonChanging(Person instance, FemalePerson newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonFemalePersonChanged(Person instance, FemalePerson oldValue)
		{
			if ((object)instance.FemalePerson != null)
			{
				instance.FemalePerson.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonChildPersonChanging(Person instance, ChildPerson newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonChildPersonChanged(Person instance, ChildPerson oldValue)
		{
			if ((object)instance.ChildPerson != null)
			{
				instance.ChildPerson.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonDeathChanging(Person instance, Death newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonDeathChanged(Person instance, Death oldValue)
		{
			if ((object)instance.Death != null)
			{
				instance.Death.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonPersonDrivesCarAsDrivenByPersonAdding(Person instance, PersonDrivesCar value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonPersonDrivesCarAsDrivenByPersonAdded(Person instance, PersonDrivesCar value)
		{
			value.DrivenByPerson = instance;
		}
		private void OnPersonPersonDrivesCarAsDrivenByPersonRemoved(Person instance, PersonDrivesCar value)
		{
			value.DrivenByPerson = null;
		}
		private bool OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			value.Buyer = instance;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			value.Buyer = null;
		}
		private bool OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			value.Seller = instance;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			value.Seller = null;
		}
		private bool OnPersonPersonHasNickNameAsPersonAdding(Person instance, PersonHasNickName value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonPersonHasNickNameAsPersonAdded(Person instance, PersonHasNickName value)
		{
			value.Person = instance;
		}
		private void OnPersonPersonHasNickNameAsPersonRemoved(Person instance, PersonHasNickName value)
		{
			value.Person = null;
		}
		private bool OnPersonTaskAdding(Person instance, Task value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonTaskAdded(Person instance, Task value)
		{
			value.Person = instance;
		}
		private void OnPersonTaskRemoved(Person instance, Task value)
		{
			value.Person = null;
		}
		private bool OnPersonValueType1DoesSomethingWithAdding(Person instance, ValueType1 value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonValueType1DoesSomethingWithAdded(Person instance, ValueType1 value)
		{
			value.DoesSomethingWithPerson = instance;
		}
		private void OnPersonValueType1DoesSomethingWithRemoved(Person instance, ValueType1 value)
		{
			value.DoesSomethingWithPerson = null;
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
			public PersonCore(SampleModelContext context, string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, decimal MandatoryUniqueDecimal, string MandatoryUniqueString)
			{
				this._Context = context;
				this._PersonDrivesCarAsDrivenByPerson = new ConstraintEnforcementCollection<Person, PersonDrivesCar>(this);
				this._PersonBoughtCarFromPersonOnDateAsBuyer = new ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>(this, "PersonBoughtCarFromPersonOnDateAsBuyer");
				this._PersonBoughtCarFromPersonOnDateAsSeller = new ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>(this, "PersonBoughtCarFromPersonOnDateAsSeller");
				this._PersonHasNickNameAsPerson = new ConstraintEnforcementCollection<Person, PersonHasNickName>(this);
				this._Task = new ConstraintEnforcementCollection<Person, Task>(this);
				this._ValueType1DoesSomethingWith = new ConstraintEnforcementCollection<Person, ValueType1>(this);
				this._FirstName = FirstName;
				context.OnPersonFirstNameChanged(this, null);
				this._Date_YMD = Date_YMD;
				context.OnPersonDate_YMDChanged(this, null);
				this._LastName = LastName;
				context.OnPersonLastNameChanged(this, null);
				this._Gender_Gender_Code = Gender_Gender_Code;
				this._MandatoryUniqueDecimal = MandatoryUniqueDecimal;
				context.OnPersonMandatoryUniqueDecimalChanged(this, null);
				this._MandatoryUniqueString = MandatoryUniqueString;
				context.OnPersonMandatoryUniqueStringChanged(this, null);
				context._PersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
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
							this._Context.OnPersonFirstNameChanged(this, oldValue);
							base.RaiseFirstNameChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Date_YMD")]
			private int _Date_YMD;
			public override int Date_YMD
			{
				get
				{
					return this._Date_YMD;
				}
				set
				{
					int oldValue = this._Date_YMD;
					if (oldValue != value)
					{
						if (this._Context.OnPersonDate_YMDChanging(this, value) && base.RaiseDate_YMDChangingEvent(value))
						{
							this._Date_YMD = value;
							this._Context.OnPersonDate_YMDChanged(this, oldValue);
							base.RaiseDate_YMDChangedEvent(oldValue);
						}
					}
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
							this._Context.OnPersonLastNameChanged(this, oldValue);
							base.RaiseLastNameChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("OptionalUniqueString")]
			private string _OptionalUniqueString;
			public override string OptionalUniqueString
			{
				get
				{
					return this._OptionalUniqueString;
				}
				set
				{
					string oldValue = this._OptionalUniqueString;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonOptionalUniqueStringChanging(this, value) && base.RaiseOptionalUniqueStringChangingEvent(value))
						{
							this._OptionalUniqueString = value;
							this._Context.OnPersonOptionalUniqueStringChanged(this, oldValue);
							base.RaiseOptionalUniqueStringChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("HatType_ColorARGB")]
			private Nullable<int> _HatType_ColorARGB;
			public override Nullable<int> HatType_ColorARGB
			{
				get
				{
					return this._HatType_ColorARGB;
				}
				set
				{
					Nullable<int> oldValue = this._HatType_ColorARGB;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonHatType_ColorARGBChanging(this, value) && base.RaiseHatType_ColorARGBChangingEvent(value))
						{
							this._HatType_ColorARGB = value;
							base.RaiseHatType_ColorARGBChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("HatType_HatTypeStyle_HatTypeStyle_Description")]
			private string _HatType_HatTypeStyle_HatTypeStyle_Description;
			public override string HatType_HatTypeStyle_HatTypeStyle_Description
			{
				get
				{
					return this._HatType_HatTypeStyle_HatTypeStyle_Description;
				}
				set
				{
					string oldValue = this._HatType_HatTypeStyle_HatTypeStyle_Description;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(this, value) && base.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(value))
						{
							this._HatType_HatTypeStyle_HatTypeStyle_Description = value;
							base.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("OwnsCar_vin")]
			private Nullable<int> _OwnsCar_vin;
			public override Nullable<int> OwnsCar_vin
			{
				get
				{
					return this._OwnsCar_vin;
				}
				set
				{
					Nullable<int> oldValue = this._OwnsCar_vin;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonOwnsCar_vinChanging(this, value) && base.RaiseOwnsCar_vinChangingEvent(value))
						{
							this._OwnsCar_vin = value;
							this._Context.OnPersonOwnsCar_vinChanged(this, oldValue);
							base.RaiseOwnsCar_vinChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Gender_Gender_Code")]
			private string _Gender_Gender_Code;
			public override string Gender_Gender_Code
			{
				get
				{
					return this._Gender_Gender_Code;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Gender_Gender_Code;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonGender_Gender_CodeChanging(this, value) && base.RaiseGender_Gender_CodeChangingEvent(value))
						{
							this._Gender_Gender_Code = value;
							base.RaiseGender_Gender_CodeChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("PersonHasParents")]
			private Nullable<bool> _PersonHasParents;
			public override Nullable<bool> PersonHasParents
			{
				get
				{
					return this._PersonHasParents;
				}
				set
				{
					Nullable<bool> oldValue = this._PersonHasParents;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonPersonHasParentsChanging(this, value) && base.RaisePersonHasParentsChangingEvent(value))
						{
							this._PersonHasParents = value;
							base.RaisePersonHasParentsChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("OptionalUniqueDecimal")]
			private Nullable<decimal> _OptionalUniqueDecimal;
			public override Nullable<decimal> OptionalUniqueDecimal
			{
				get
				{
					return this._OptionalUniqueDecimal;
				}
				set
				{
					Nullable<decimal> oldValue = this._OptionalUniqueDecimal;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonOptionalUniqueDecimalChanging(this, value) && base.RaiseOptionalUniqueDecimalChangingEvent(value))
						{
							this._OptionalUniqueDecimal = value;
							this._Context.OnPersonOptionalUniqueDecimalChanged(this, oldValue);
							base.RaiseOptionalUniqueDecimalChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("MandatoryUniqueDecimal")]
			private decimal _MandatoryUniqueDecimal;
			public override decimal MandatoryUniqueDecimal
			{
				get
				{
					return this._MandatoryUniqueDecimal;
				}
				set
				{
					decimal oldValue = this._MandatoryUniqueDecimal;
					if (oldValue != value)
					{
						if (this._Context.OnPersonMandatoryUniqueDecimalChanging(this, value) && base.RaiseMandatoryUniqueDecimalChangingEvent(value))
						{
							this._MandatoryUniqueDecimal = value;
							this._Context.OnPersonMandatoryUniqueDecimalChanged(this, oldValue);
							base.RaiseMandatoryUniqueDecimalChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("MandatoryUniqueString")]
			private string _MandatoryUniqueString;
			public override string MandatoryUniqueString
			{
				get
				{
					return this._MandatoryUniqueString;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._MandatoryUniqueString;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonMandatoryUniqueStringChanging(this, value) && base.RaiseMandatoryUniqueStringChangingEvent(value))
						{
							this._MandatoryUniqueString = value;
							this._Context.OnPersonMandatoryUniqueStringChanged(this, oldValue);
							base.RaiseMandatoryUniqueStringChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("ValueType1DoesSomethingElseWith")]
			private ValueType1 _ValueType1DoesSomethingElseWith;
			public override ValueType1 ValueType1DoesSomethingElseWith
			{
				get
				{
					return this._ValueType1DoesSomethingElseWith;
				}
				set
				{
					ValueType1 oldValue = this._ValueType1DoesSomethingElseWith;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonValueType1DoesSomethingElseWithChanging(this, value) && base.RaiseValueType1DoesSomethingElseWithChangingEvent(value))
						{
							this._ValueType1DoesSomethingElseWith = value;
							this._Context.OnPersonValueType1DoesSomethingElseWithChanged(this, oldValue);
							base.RaiseValueType1DoesSomethingElseWithChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("MalePerson")]
			private MalePerson _MalePerson;
			public override MalePerson MalePerson
			{
				get
				{
					return this._MalePerson;
				}
				set
				{
					MalePerson oldValue = this._MalePerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonMalePersonChanging(this, value) && base.RaiseMalePersonChangingEvent(value))
						{
							this._MalePerson = value;
							this._Context.OnPersonMalePersonChanged(this, oldValue);
							base.RaiseMalePersonChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("FemalePerson")]
			private FemalePerson _FemalePerson;
			public override FemalePerson FemalePerson
			{
				get
				{
					return this._FemalePerson;
				}
				set
				{
					FemalePerson oldValue = this._FemalePerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonFemalePersonChanging(this, value) && base.RaiseFemalePersonChangingEvent(value))
						{
							this._FemalePerson = value;
							this._Context.OnPersonFemalePersonChanged(this, oldValue);
							base.RaiseFemalePersonChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("ChildPerson")]
			private ChildPerson _ChildPerson;
			public override ChildPerson ChildPerson
			{
				get
				{
					return this._ChildPerson;
				}
				set
				{
					ChildPerson oldValue = this._ChildPerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonChildPersonChanging(this, value) && base.RaiseChildPersonChangingEvent(value))
						{
							this._ChildPerson = value;
							this._Context.OnPersonChildPersonChanged(this, oldValue);
							base.RaiseChildPersonChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Death")]
			private Death _Death;
			public override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					Death oldValue = this._Death;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonDeathChanging(this, value) && base.RaiseDeathChangingEvent(value))
						{
							this._Death = value;
							this._Context.OnPersonDeathChanged(this, oldValue);
							base.RaiseDeathChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("PersonDrivesCarAsDrivenByPerson")]
			private readonly ICollection<PersonDrivesCar> _PersonDrivesCarAsDrivenByPerson;
			public override ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
			{
				get
				{
					return this._PersonDrivesCarAsDrivenByPerson;
				}
			}
			[AccessedThroughPropertyAttribute("PersonBoughtCarFromPersonOnDateAsBuyer")]
			private readonly ICollection<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateAsBuyer;
			public override ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
			{
				get
				{
					return this._PersonBoughtCarFromPersonOnDateAsBuyer;
				}
			}
			[AccessedThroughPropertyAttribute("PersonBoughtCarFromPersonOnDateAsSeller")]
			private readonly ICollection<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateAsSeller;
			public override ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
			{
				get
				{
					return this._PersonBoughtCarFromPersonOnDateAsSeller;
				}
			}
			[AccessedThroughPropertyAttribute("PersonHasNickNameAsPerson")]
			private readonly ICollection<PersonHasNickName> _PersonHasNickNameAsPerson;
			public override ICollection<PersonHasNickName> PersonHasNickNameAsPerson
			{
				get
				{
					return this._PersonHasNickNameAsPerson;
				}
			}
			[AccessedThroughPropertyAttribute("Task")]
			private readonly ICollection<Task> _Task;
			public override ICollection<Task> Task
			{
				get
				{
					return this._Task;
				}
			}
			[AccessedThroughPropertyAttribute("ValueType1DoesSomethingWith")]
			private readonly ICollection<ValueType1> _ValueType1DoesSomethingWith;
			public override ICollection<ValueType1> ValueType1DoesSomethingWith
			{
				get
				{
					return this._ValueType1DoesSomethingWith;
				}
			}
		}
		#endregion // PersonCore
		#endregion // Person
		#region MalePerson
		public MalePerson CreateMalePerson(Person Person)
		{
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnMalePersonPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new MalePersonCore(this, Person);
		}
		private bool OnMalePersonPersonChanging(MalePerson instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnMalePersonPersonChanged(MalePerson instance, Person oldValue)
		{
			instance.Person.MalePerson = instance;
			if ((object)oldValue != null)
			{
				oldValue.MalePerson = null;
			}
		}
		private bool OnMalePersonChildPersonAdding(MalePerson instance, ChildPerson value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnMalePersonChildPersonAdded(MalePerson instance, ChildPerson value)
		{
			value.Father = instance;
		}
		private void OnMalePersonChildPersonRemoved(MalePerson instance, ChildPerson value)
		{
			value.Father = null;
		}
		private readonly List<MalePerson> _MalePersonList;
		private readonly ReadOnlyCollection<MalePerson> _MalePersonReadOnlyCollection;
		public ReadOnlyCollection<MalePerson> MalePersonCollection
		{
			get
			{
				return this._MalePersonReadOnlyCollection;
			}
		}
		#region MalePersonCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class MalePersonCore : MalePerson
		{
			public MalePersonCore(SampleModelContext context, Person Person)
			{
				this._Context = context;
				this._ChildPerson = new ConstraintEnforcementCollection<MalePerson, ChildPerson>(this);
				this._Person = Person;
				context.OnMalePersonPersonChanged(this, null);
				context._MalePersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnMalePersonPersonChanging(this, value) && base.RaisePersonChangingEvent(value))
						{
							this._Person = value;
							this._Context.OnMalePersonPersonChanged(this, oldValue);
							base.RaisePersonChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("ChildPerson")]
			private readonly ICollection<ChildPerson> _ChildPerson;
			public override ICollection<ChildPerson> ChildPerson
			{
				get
				{
					return this._ChildPerson;
				}
			}
		}
		#endregion // MalePersonCore
		#endregion // MalePerson
		#region FemalePerson
		public FemalePerson CreateFemalePerson(Person Person)
		{
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnFemalePersonPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new FemalePersonCore(this, Person);
		}
		private bool OnFemalePersonPersonChanging(FemalePerson instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnFemalePersonPersonChanged(FemalePerson instance, Person oldValue)
		{
			instance.Person.FemalePerson = instance;
			if ((object)oldValue != null)
			{
				oldValue.FemalePerson = null;
			}
		}
		private bool OnFemalePersonChildPersonAdding(FemalePerson instance, ChildPerson value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnFemalePersonChildPersonAdded(FemalePerson instance, ChildPerson value)
		{
			value.Mother = instance;
		}
		private void OnFemalePersonChildPersonRemoved(FemalePerson instance, ChildPerson value)
		{
			value.Mother = null;
		}
		private readonly List<FemalePerson> _FemalePersonList;
		private readonly ReadOnlyCollection<FemalePerson> _FemalePersonReadOnlyCollection;
		public ReadOnlyCollection<FemalePerson> FemalePersonCollection
		{
			get
			{
				return this._FemalePersonReadOnlyCollection;
			}
		}
		#region FemalePersonCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class FemalePersonCore : FemalePerson
		{
			public FemalePersonCore(SampleModelContext context, Person Person)
			{
				this._Context = context;
				this._ChildPerson = new ConstraintEnforcementCollection<FemalePerson, ChildPerson>(this);
				this._Person = Person;
				context.OnFemalePersonPersonChanged(this, null);
				context._FemalePersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnFemalePersonPersonChanging(this, value) && base.RaisePersonChangingEvent(value))
						{
							this._Person = value;
							this._Context.OnFemalePersonPersonChanged(this, oldValue);
							base.RaisePersonChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("ChildPerson")]
			private readonly ICollection<ChildPerson> _ChildPerson;
			public override ICollection<ChildPerson> ChildPerson
			{
				get
				{
					return this._ChildPerson;
				}
			}
		}
		#endregion // FemalePersonCore
		#endregion // FemalePerson
		#region ChildPerson
		public ChildPerson CreateChildPerson(int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person)
		{
			if ((object)Father == null)
			{
				throw new ArgumentNullException("Father");
			}
			if ((object)Mother == null)
			{
				throw new ArgumentNullException("Mother");
			}
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnChildPersonBirthOrder_BirthOrder_NrChanging(null, BirthOrder_BirthOrder_Nr)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("BirthOrder_BirthOrder_Nr");
			}
			if (!(this.OnChildPersonFatherChanging(null, Father)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Father");
			}
			if (!(this.OnChildPersonMotherChanging(null, Mother)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Mother");
			}
			if (!(this.OnChildPersonPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new ChildPersonCore(this, BirthOrder_BirthOrder_Nr, Father, Mother, Person);
		}
		private bool OnChildPersonBirthOrder_BirthOrder_NrChanging(ChildPerson instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, newValue, instance.Mother))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonBirthOrder_BirthOrder_NrChanged(ChildPerson instance, Nullable<int> oldValue)
		{
			Tuple<MalePerson, int, FemalePerson> ExternalUniquenessConstraint3OldValueTuple;
			if (oldValue.HasValue)
			{
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, oldValue.Value, instance.Mother);
			}
			else
			{
				ExternalUniquenessConstraint3OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
		}
		private bool OnChildPersonFatherChanging(ChildPerson instance, MalePerson newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple<MalePerson, int, FemalePerson>(newValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonFatherChanged(ChildPerson instance, MalePerson oldValue)
		{
			instance.Father.ChildPerson.Add(instance);
			Tuple<MalePerson, int, FemalePerson> ExternalUniquenessConstraint3OldValueTuple;
			if ((object)oldValue != null)
			{
				oldValue.ChildPerson.Remove(instance);
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(oldValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother);
			}
			else
			{
				ExternalUniquenessConstraint3OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
		}
		private bool OnChildPersonMotherChanging(ChildPerson instance, FemalePerson newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonMotherChanged(ChildPerson instance, FemalePerson oldValue)
		{
			instance.Mother.ChildPerson.Add(instance);
			Tuple<MalePerson, int, FemalePerson> ExternalUniquenessConstraint3OldValueTuple;
			if ((object)oldValue != null)
			{
				oldValue.ChildPerson.Remove(instance);
				ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, oldValue);
			}
			else
			{
				ExternalUniquenessConstraint3OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
		}
		private bool OnChildPersonPersonChanging(ChildPerson instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnChildPersonPersonChanged(ChildPerson instance, Person oldValue)
		{
			instance.Person.ChildPerson = instance;
			if ((object)oldValue != null)
			{
				oldValue.ChildPerson = null;
			}
		}
		private readonly List<ChildPerson> _ChildPersonList;
		private readonly ReadOnlyCollection<ChildPerson> _ChildPersonReadOnlyCollection;
		public ReadOnlyCollection<ChildPerson> ChildPersonCollection
		{
			get
			{
				return this._ChildPersonReadOnlyCollection;
			}
		}
		#region ChildPersonCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ChildPersonCore : ChildPerson
		{
			public ChildPersonCore(SampleModelContext context, int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person)
			{
				this._Context = context;
				this._BirthOrder_BirthOrder_Nr = BirthOrder_BirthOrder_Nr;
				context.OnChildPersonBirthOrder_BirthOrder_NrChanged(this, null);
				this._Father = Father;
				context.OnChildPersonFatherChanged(this, null);
				this._Mother = Mother;
				context.OnChildPersonMotherChanged(this, null);
				this._Person = Person;
				context.OnChildPersonPersonChanged(this, null);
				context._ChildPersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("BirthOrder_BirthOrder_Nr")]
			private int _BirthOrder_BirthOrder_Nr;
			public override int BirthOrder_BirthOrder_Nr
			{
				get
				{
					return this._BirthOrder_BirthOrder_Nr;
				}
				set
				{
					int oldValue = this._BirthOrder_BirthOrder_Nr;
					if (oldValue != value)
					{
						if (this._Context.OnChildPersonBirthOrder_BirthOrder_NrChanging(this, value) && base.RaiseBirthOrder_BirthOrder_NrChangingEvent(value))
						{
							this._BirthOrder_BirthOrder_Nr = value;
							this._Context.OnChildPersonBirthOrder_BirthOrder_NrChanged(this, oldValue);
							base.RaiseBirthOrder_BirthOrder_NrChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Father")]
			private MalePerson _Father;
			public override MalePerson Father
			{
				get
				{
					return this._Father;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					MalePerson oldValue = this._Father;
					if ((object)oldValue != value)
					{
						if (this._Context.OnChildPersonFatherChanging(this, value) && base.RaiseFatherChangingEvent(value))
						{
							this._Father = value;
							this._Context.OnChildPersonFatherChanged(this, oldValue);
							base.RaiseFatherChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Mother")]
			private FemalePerson _Mother;
			public override FemalePerson Mother
			{
				get
				{
					return this._Mother;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					FemalePerson oldValue = this._Mother;
					if ((object)oldValue != value)
					{
						if (this._Context.OnChildPersonMotherChanging(this, value) && base.RaiseMotherChangingEvent(value))
						{
							this._Mother = value;
							this._Context.OnChildPersonMotherChanged(this, oldValue);
							base.RaiseMotherChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnChildPersonPersonChanging(this, value) && base.RaisePersonChangingEvent(value))
						{
							this._Person = value;
							this._Context.OnChildPersonPersonChanged(this, oldValue);
							base.RaisePersonChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // ChildPersonCore
		#endregion // ChildPerson
		#region Death
		public Death CreateDeath(string DeathCause_DeathCause_Type, Person Person)
		{
			if ((object)DeathCause_DeathCause_Type == null)
			{
				throw new ArgumentNullException("DeathCause_DeathCause_Type");
			}
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnDeathDeathCause_DeathCause_TypeChanging(null, DeathCause_DeathCause_Type)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("DeathCause_DeathCause_Type");
			}
			if (!(this.OnDeathPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new DeathCore(this, DeathCause_DeathCause_Type, Person);
		}
		private bool OnDeathDate_YMDChanging(Death instance, Nullable<int> newValue)
		{
			return true;
		}
		private bool OnDeathDeathCause_DeathCause_TypeChanging(Death instance, string newValue)
		{
			return true;
		}
		private bool OnDeathNaturalDeathChanging(Death instance, NaturalDeath newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnDeathNaturalDeathChanged(Death instance, NaturalDeath oldValue)
		{
			if ((object)instance.NaturalDeath != null)
			{
				instance.NaturalDeath.Death = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Death = null;
			}
		}
		private bool OnDeathUnnaturalDeathChanging(Death instance, UnnaturalDeath newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnDeathUnnaturalDeathChanged(Death instance, UnnaturalDeath oldValue)
		{
			if ((object)instance.UnnaturalDeath != null)
			{
				instance.UnnaturalDeath.Death = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Death = null;
			}
		}
		private bool OnDeathPersonChanging(Death instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnDeathPersonChanged(Death instance, Person oldValue)
		{
			instance.Person.Death = instance;
			if ((object)oldValue != null)
			{
				oldValue.Death = null;
			}
		}
		private readonly List<Death> _DeathList;
		private readonly ReadOnlyCollection<Death> _DeathReadOnlyCollection;
		public ReadOnlyCollection<Death> DeathCollection
		{
			get
			{
				return this._DeathReadOnlyCollection;
			}
		}
		#region DeathCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class DeathCore : Death
		{
			public DeathCore(SampleModelContext context, string DeathCause_DeathCause_Type, Person Person)
			{
				this._Context = context;
				this._DeathCause_DeathCause_Type = DeathCause_DeathCause_Type;
				this._Person = Person;
				context.OnDeathPersonChanged(this, null);
				context._DeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("Date_YMD")]
			private Nullable<int> _Date_YMD;
			public override Nullable<int> Date_YMD
			{
				get
				{
					return this._Date_YMD;
				}
				set
				{
					Nullable<int> oldValue = this._Date_YMD;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnDeathDate_YMDChanging(this, value) && base.RaiseDate_YMDChangingEvent(value))
						{
							this._Date_YMD = value;
							base.RaiseDate_YMDChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("DeathCause_DeathCause_Type")]
			private string _DeathCause_DeathCause_Type;
			public override string DeathCause_DeathCause_Type
			{
				get
				{
					return this._DeathCause_DeathCause_Type;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._DeathCause_DeathCause_Type;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnDeathDeathCause_DeathCause_TypeChanging(this, value) && base.RaiseDeathCause_DeathCause_TypeChangingEvent(value))
						{
							this._DeathCause_DeathCause_Type = value;
							base.RaiseDeathCause_DeathCause_TypeChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("NaturalDeath")]
			private NaturalDeath _NaturalDeath;
			public override NaturalDeath NaturalDeath
			{
				get
				{
					return this._NaturalDeath;
				}
				set
				{
					NaturalDeath oldValue = this._NaturalDeath;
					if ((object)oldValue != value)
					{
						if (this._Context.OnDeathNaturalDeathChanging(this, value) && base.RaiseNaturalDeathChangingEvent(value))
						{
							this._NaturalDeath = value;
							this._Context.OnDeathNaturalDeathChanged(this, oldValue);
							base.RaiseNaturalDeathChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("UnnaturalDeath")]
			private UnnaturalDeath _UnnaturalDeath;
			public override UnnaturalDeath UnnaturalDeath
			{
				get
				{
					return this._UnnaturalDeath;
				}
				set
				{
					UnnaturalDeath oldValue = this._UnnaturalDeath;
					if ((object)oldValue != value)
					{
						if (this._Context.OnDeathUnnaturalDeathChanging(this, value) && base.RaiseUnnaturalDeathChangingEvent(value))
						{
							this._UnnaturalDeath = value;
							this._Context.OnDeathUnnaturalDeathChanged(this, oldValue);
							base.RaiseUnnaturalDeathChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnDeathPersonChanging(this, value) && base.RaisePersonChangingEvent(value))
						{
							this._Person = value;
							this._Context.OnDeathPersonChanged(this, oldValue);
							base.RaisePersonChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // DeathCore
		#endregion // Death
		#region NaturalDeath
		public NaturalDeath CreateNaturalDeath(Death Death)
		{
			if ((object)Death == null)
			{
				throw new ArgumentNullException("Death");
			}
			if (!(this.OnNaturalDeathDeathChanging(null, Death)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Death");
			}
			return new NaturalDeathCore(this, Death);
		}
		private bool OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(NaturalDeath instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnNaturalDeathDeathChanging(NaturalDeath instance, Death newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnNaturalDeathDeathChanged(NaturalDeath instance, Death oldValue)
		{
			instance.Death.NaturalDeath = instance;
			if ((object)oldValue != null)
			{
				oldValue.NaturalDeath = null;
			}
		}
		private readonly List<NaturalDeath> _NaturalDeathList;
		private readonly ReadOnlyCollection<NaturalDeath> _NaturalDeathReadOnlyCollection;
		public ReadOnlyCollection<NaturalDeath> NaturalDeathCollection
		{
			get
			{
				return this._NaturalDeathReadOnlyCollection;
			}
		}
		#region NaturalDeathCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class NaturalDeathCore : NaturalDeath
		{
			public NaturalDeathCore(SampleModelContext context, Death Death)
			{
				this._Context = context;
				this._Death = Death;
				context.OnNaturalDeathDeathChanged(this, null);
				context._NaturalDeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("NaturalDeathIsFromProstateCancer")]
			private Nullable<bool> _NaturalDeathIsFromProstateCancer;
			public override Nullable<bool> NaturalDeathIsFromProstateCancer
			{
				get
				{
					return this._NaturalDeathIsFromProstateCancer;
				}
				set
				{
					Nullable<bool> oldValue = this._NaturalDeathIsFromProstateCancer;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(this, value) && base.RaiseNaturalDeathIsFromProstateCancerChangingEvent(value))
						{
							this._NaturalDeathIsFromProstateCancer = value;
							base.RaiseNaturalDeathIsFromProstateCancerChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Death")]
			private Death _Death;
			public override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Death oldValue = this._Death;
					if ((object)oldValue != value)
					{
						if (this._Context.OnNaturalDeathDeathChanging(this, value) && base.RaiseDeathChangingEvent(value))
						{
							this._Death = value;
							this._Context.OnNaturalDeathDeathChanged(this, oldValue);
							base.RaiseDeathChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // NaturalDeathCore
		#endregion // NaturalDeath
		#region UnnaturalDeath
		public UnnaturalDeath CreateUnnaturalDeath(Death Death)
		{
			if ((object)Death == null)
			{
				throw new ArgumentNullException("Death");
			}
			if (!(this.OnUnnaturalDeathDeathChanging(null, Death)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Death");
			}
			return new UnnaturalDeathCore(this, Death);
		}
		private bool OnUnnaturalDeathUnnaturalDeathIsViolentChanging(UnnaturalDeath instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(UnnaturalDeath instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnUnnaturalDeathDeathChanging(UnnaturalDeath instance, Death newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnUnnaturalDeathDeathChanged(UnnaturalDeath instance, Death oldValue)
		{
			instance.Death.UnnaturalDeath = instance;
			if ((object)oldValue != null)
			{
				oldValue.UnnaturalDeath = null;
			}
		}
		private readonly List<UnnaturalDeath> _UnnaturalDeathList;
		private readonly ReadOnlyCollection<UnnaturalDeath> _UnnaturalDeathReadOnlyCollection;
		public ReadOnlyCollection<UnnaturalDeath> UnnaturalDeathCollection
		{
			get
			{
				return this._UnnaturalDeathReadOnlyCollection;
			}
		}
		#region UnnaturalDeathCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class UnnaturalDeathCore : UnnaturalDeath
		{
			public UnnaturalDeathCore(SampleModelContext context, Death Death)
			{
				this._Context = context;
				this._Death = Death;
				context.OnUnnaturalDeathDeathChanged(this, null);
				context._UnnaturalDeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("UnnaturalDeathIsViolent")]
			private Nullable<bool> _UnnaturalDeathIsViolent;
			public override Nullable<bool> UnnaturalDeathIsViolent
			{
				get
				{
					return this._UnnaturalDeathIsViolent;
				}
				set
				{
					Nullable<bool> oldValue = this._UnnaturalDeathIsViolent;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnUnnaturalDeathUnnaturalDeathIsViolentChanging(this, value) && base.RaiseUnnaturalDeathIsViolentChangingEvent(value))
						{
							this._UnnaturalDeathIsViolent = value;
							base.RaiseUnnaturalDeathIsViolentChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("UnnaturalDeathIsBloody")]
			private Nullable<bool> _UnnaturalDeathIsBloody;
			public override Nullable<bool> UnnaturalDeathIsBloody
			{
				get
				{
					return this._UnnaturalDeathIsBloody;
				}
				set
				{
					Nullable<bool> oldValue = this._UnnaturalDeathIsBloody;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(this, value) && base.RaiseUnnaturalDeathIsBloodyChangingEvent(value))
						{
							this._UnnaturalDeathIsBloody = value;
							base.RaiseUnnaturalDeathIsBloodyChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("Death")]
			private Death _Death;
			public override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Death oldValue = this._Death;
					if ((object)oldValue != value)
					{
						if (this._Context.OnUnnaturalDeathDeathChanging(this, value) && base.RaiseDeathChangingEvent(value))
						{
							this._Death = value;
							this._Context.OnUnnaturalDeathDeathChanged(this, oldValue);
							base.RaiseDeathChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // UnnaturalDeathCore
		#endregion // UnnaturalDeath
		#region Task
		public Task CreateTask()
		{
			return new TaskCore(this);
		}
		private bool OnTaskPersonChanging(Task instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnTaskPersonChanged(Task instance, Person oldValue)
		{
			if ((object)instance.Person != null)
			{
				instance.Person.Task.Add(instance);
			}
			if ((object)oldValue != null)
			{
				oldValue.Task.Remove(instance);
			}
		}
		private readonly List<Task> _TaskList;
		private readonly ReadOnlyCollection<Task> _TaskReadOnlyCollection;
		public ReadOnlyCollection<Task> TaskCollection
		{
			get
			{
				return this._TaskReadOnlyCollection;
			}
		}
		#region TaskCore
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class TaskCore : Task
		{
			public TaskCore(SampleModelContext context)
			{
				this._Context = context;
				context._TaskList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnTaskPersonChanging(this, value) && base.RaisePersonChangingEvent(value))
						{
							this._Person = value;
							this._Context.OnTaskPersonChanged(this, oldValue);
							base.RaisePersonChangedEvent(oldValue);
						}
					}
				}
			}
		}
		#endregion // TaskCore
		#endregion // Task
		#region ValueType1
		public ValueType1 CreateValueType1(int ValueType1Value)
		{
			if (!(this.OnValueType1ValueType1ValueChanging(null, ValueType1Value)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("ValueType1Value");
			}
			return new ValueType1Core(this, ValueType1Value);
		}
		private bool OnValueType1ValueType1ValueChanging(ValueType1 instance, int newValue)
		{
			ValueType1 currentInstance;
			if (this._ValueType1ValueType1ValueDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnValueType1ValueType1ValueChanged(ValueType1 instance, Nullable<int> oldValue)
		{
			this._ValueType1ValueType1ValueDictionary.Add(instance.ValueType1Value, instance);
			if (oldValue.HasValue)
			{
				this._ValueType1ValueType1ValueDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnValueType1DoesSomethingWithPersonChanging(ValueType1 instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnValueType1DoesSomethingWithPersonChanged(ValueType1 instance, Person oldValue)
		{
			if ((object)instance.DoesSomethingWithPerson != null)
			{
				instance.DoesSomethingWithPerson.ValueType1DoesSomethingWith.Add(instance);
			}
			if ((object)oldValue != null)
			{
				oldValue.ValueType1DoesSomethingWith.Remove(instance);
			}
		}
		private bool OnValueType1DoesSomethingElseWithPersonAdding(ValueType1 instance, Person value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnValueType1DoesSomethingElseWithPersonAdded(ValueType1 instance, Person value)
		{
			value.ValueType1DoesSomethingElseWith = instance;
		}
		private void OnValueType1DoesSomethingElseWithPersonRemoved(ValueType1 instance, Person value)
		{
			value.ValueType1DoesSomethingElseWith = null;
		}
		private readonly List<ValueType1> _ValueType1List;
		private readonly ReadOnlyCollection<ValueType1> _ValueType1ReadOnlyCollection;
		public ReadOnlyCollection<ValueType1> ValueType1Collection
		{
			get
			{
				return this._ValueType1ReadOnlyCollection;
			}
		}
		#region ValueType1Core
		[StructLayoutAttribute(LayoutKind.Auto, CharSet=CharSet.Auto)]
		private sealed class ValueType1Core : ValueType1
		{
			public ValueType1Core(SampleModelContext context, int ValueType1Value)
			{
				this._Context = context;
				this._DoesSomethingElseWithPerson = new ConstraintEnforcementCollection<ValueType1, Person>(this);
				this._ValueType1Value = ValueType1Value;
				context.OnValueType1ValueType1ValueChanged(this, null);
				context._ValueType1List.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughPropertyAttribute("ValueType1Value")]
			private int _ValueType1Value;
			public override int ValueType1Value
			{
				get
				{
					return this._ValueType1Value;
				}
				set
				{
					int oldValue = this._ValueType1Value;
					if (oldValue != value)
					{
						if (this._Context.OnValueType1ValueType1ValueChanging(this, value) && base.RaiseValueType1ValueChangingEvent(value))
						{
							this._ValueType1Value = value;
							this._Context.OnValueType1ValueType1ValueChanged(this, oldValue);
							base.RaiseValueType1ValueChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("DoesSomethingWithPerson")]
			private Person _DoesSomethingWithPerson;
			public override Person DoesSomethingWithPerson
			{
				get
				{
					return this._DoesSomethingWithPerson;
				}
				set
				{
					Person oldValue = this._DoesSomethingWithPerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnValueType1DoesSomethingWithPersonChanging(this, value) && base.RaiseDoesSomethingWithPersonChangingEvent(value))
						{
							this._DoesSomethingWithPerson = value;
							this._Context.OnValueType1DoesSomethingWithPersonChanged(this, oldValue);
							base.RaiseDoesSomethingWithPersonChangedEvent(oldValue);
						}
					}
				}
			}
			[AccessedThroughPropertyAttribute("DoesSomethingElseWithPerson")]
			private readonly ICollection<Person> _DoesSomethingElseWithPerson;
			public override ICollection<Person> DoesSomethingElseWithPerson
			{
				get
				{
					return this._DoesSomethingElseWithPerson;
				}
			}
		}
		#endregion // ValueType1Core
		#endregion // ValueType1
	}
	#endregion // SampleModelContext
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute;
using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
#region Global Support Classes
namespace System
{
	#region Tuple Support
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	[System.Serializable()]
	public abstract partial class Tuple
	{
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
	public sealed class Tuple<T1, T2> : Tuple, System.IEquatable<Tuple<T1, T2>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		public Tuple(T1 item1, T2 item2)
		{
			if ((item1 == null) || (item2 == null))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2>);
		}
		public bool Equals(Tuple<T1, T2> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || !(this.item2.Equals(other.item2))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ Tuple.RotateRight(this.item2.GetHashCode(), 1);
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2})", this.item1, this.item2);
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
				return new System.Collections.Generic.KeyValuePair<T1, T2>(tuple.item1, tuple.item2);
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
				return new System.Collections.DictionaryEntry(tuple.item1, tuple.item2);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3> : Tuple, System.IEquatable<Tuple<T1, T2, T3>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			if ((item1 == null) || ((item2 == null) || (item3 == null)))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3>);
		}
		public bool Equals(Tuple<T1, T2, T3> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || !(this.item3.Equals(other.item3)))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ Tuple.RotateRight(this.item3.GetHashCode(), 2));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3})", this.item1, this.item2, this.item3);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || (item4 == null))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || !(this.item4.Equals(other.item4))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ Tuple.RotateRight(this.item4.GetHashCode(), 3)));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4})", this.item1, this.item2, this.item3, this.item4);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4, T5> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		private readonly T5 item5;
		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || (item5 == null)))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || (!(this.item4.Equals(other.item4)) || !(this.item5.Equals(other.item5)))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.item4.GetHashCode(), 3) ^ Tuple.RotateRight(this.item5.GetHashCode(), 4))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5})", this.item1, this.item2, this.item3, this.item4, this.item5);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		private readonly T5 item5;
		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}
		private readonly T6 item6;
		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || (item6 == null))))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || (!(this.item4.Equals(other.item4)) || (!(this.item5.Equals(other.item5)) || !(this.item6.Equals(other.item6))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.item5.GetHashCode(), 4) ^ Tuple.RotateRight(this.item6.GetHashCode(), 5)))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6})", this.item1, this.item2, this.item3, this.item4, this.item5, this.item6);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		private readonly T5 item5;
		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}
		private readonly T6 item6;
		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}
		private readonly T7 item7;
		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || (item7 == null)))))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || (!(this.item4.Equals(other.item4)) || (!(this.item5.Equals(other.item5)) || (!(this.item6.Equals(other.item6)) || !(this.item7.Equals(other.item7)))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.item6.GetHashCode(), 5) ^ Tuple.RotateRight(this.item7.GetHashCode(), 6))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7})", this.item1, this.item2, this.item3, this.item4, this.item5, this.item6, this.item7);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		private readonly T5 item5;
		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}
		private readonly T6 item6;
		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}
		private readonly T7 item7;
		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}
		private readonly T8 item8;
		public T8 Item8
		{
			get
			{
				return this.item8;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || (item8 == null))))))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
			this.item8 = item8;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || (!(this.item4.Equals(other.item4)) || (!(this.item5.Equals(other.item5)) || (!(this.item6.Equals(other.item6)) || (!(this.item7.Equals(other.item7)) || !(this.item8.Equals(other.item8))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this.item7.GetHashCode(), 6) ^ Tuple.RotateRight(this.item8.GetHashCode(), 7)))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", this.item1, this.item2, this.item3, this.item4, this.item5, this.item6, this.item7, this.item8);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		private readonly T5 item5;
		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}
		private readonly T6 item6;
		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}
		private readonly T7 item7;
		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}
		private readonly T8 item8;
		public T8 Item8
		{
			get
			{
				return this.item8;
			}
		}
		private readonly T9 item9;
		public T9 Item9
		{
			get
			{
				return this.item9;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || (item9 == null)))))))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
			this.item8 = item8;
			this.item9 = item9;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || (!(this.item4.Equals(other.item4)) || (!(this.item5.Equals(other.item5)) || (!(this.item6.Equals(other.item6)) || (!(this.item7.Equals(other.item7)) || (!(this.item8.Equals(other.item8)) || !(this.item9.Equals(other.item9)))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this.item7.GetHashCode(), 6) ^ (Tuple.RotateRight(this.item8.GetHashCode(), 7) ^ Tuple.RotateRight(this.item9.GetHashCode(), 8))))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", this.item1, this.item2, this.item3, this.item4, this.item5, this.item6, this.item7, this.item8, this.item9);
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA10005")]
	[System.Serializable()]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Tuple, System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
	{
		private readonly T1 item1;
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}
		private readonly T2 item2;
		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}
		private readonly T3 item3;
		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}
		private readonly T4 item4;
		public T4 Item4
		{
			get
			{
				return this.item4;
			}
		}
		private readonly T5 item5;
		public T5 Item5
		{
			get
			{
				return this.item5;
			}
		}
		private readonly T6 item6;
		public T6 Item6
		{
			get
			{
				return this.item6;
			}
		}
		private readonly T7 item7;
		public T7 Item7
		{
			get
			{
				return this.item7;
			}
		}
		private readonly T8 item8;
		public T8 Item8
		{
			get
			{
				return this.item8;
			}
		}
		private readonly T9 item9;
		public T9 Item9
		{
			get
			{
				return this.item9;
			}
		}
		private readonly T10 item10;
		public T10 Item10
		{
			get
			{
				return this.item10;
			}
		}
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || ((item9 == null) || (item10 == null))))))))))
			{
				throw new System.ArgumentNullException();
			}
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
			this.item4 = item4;
			this.item5 = item5;
			this.item6 = item6;
			this.item7 = item7;
			this.item8 = item8;
			this.item9 = item9;
			this.item10 = item10;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
		{
			if (((object)other == null) || (!(this.item1.Equals(other.item1)) || (!(this.item2.Equals(other.item2)) || (!(this.item3.Equals(other.item3)) || (!(this.item4.Equals(other.item4)) || (!(this.item5.Equals(other.item5)) || (!(this.item6.Equals(other.item6)) || (!(this.item7.Equals(other.item7)) || (!(this.item8.Equals(other.item8)) || (!(this.item9.Equals(other.item9)) || !(this.item10.Equals(other.item10))))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.item1.GetHashCode() ^ (Tuple.RotateRight(this.item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this.item7.GetHashCode(), 6) ^ (Tuple.RotateRight(this.item8.GetHashCode(), 7) ^ (Tuple.RotateRight(this.item9.GetHashCode(), 8) ^ Tuple.RotateRight(this.item10.GetHashCode(), 9)))))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", this.item1, this.item2, this.item3, this.item4, this.item5, this.item6, this.item7, this.item8, this.item9, this.item10);
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
	public sealed class PropertyChangingEventArgs<TClass, TProperty> : CancelEventArgs, IPropertyChangeEventArgs<TClass, TProperty>
	{
		private readonly TClass instance;
		private readonly TProperty oldValue;
		private readonly TProperty newValue;
		public PropertyChangingEventArgs(TClass instance, TProperty oldValue, TProperty newValue)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			this.instance = instance;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}
		public TClass Instance
		{
			get
			{
				return this.instance;
			}
		}
		public TProperty OldValue
		{
			get
			{
				return this.oldValue;
			}
		}
		public TProperty NewValue
		{
			get
			{
				return this.newValue;
			}
		}
	}
	[Serializable()]
	public sealed class PropertyChangedEventArgs<TClass, TProperty> : EventArgs, IPropertyChangeEventArgs<TClass, TProperty>
	{
		private readonly TClass instance;
		private readonly TProperty oldValue;
		private readonly TProperty newValue;
		public PropertyChangedEventArgs(TClass instance, TProperty oldValue, TProperty newValue)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			this.instance = instance;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}
		public TClass Instance
		{
			get
			{
				return this.instance;
			}
		}
		public TProperty OldValue
		{
			get
			{
				return this.oldValue;
			}
		}
		public TProperty NewValue
		{
			get
			{
				return this.newValue;
			}
		}
	}
	#endregion // Property Change Event Support
}
#endregion // Global Support Classes
namespace PersonCountryDemo
{
	#region Person
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public abstract partial class Person : INotifyPropertyChanged
	{
		protected Person()
		{
		}
		private readonly System.Delegate[] Events = new System.Delegate[5];
		[SuppressMessageAttribute("Microsoft.Design", "CA1033")]
		private event PropertyChangedEventHandler PropertyChanged
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
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}
		private void RaisePropertyChangedEvent(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
			if (eventHandler != null)
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
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected bool RaiseLastNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if (eventHandler != null)
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
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected void RaiseLastNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if (eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.LastName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
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
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected bool RaiseFirstNameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if (eventHandler != null)
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
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected void RaiseFirstNameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if (eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.FirstName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> TitleChanging
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
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected bool RaiseTitleChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if (eventHandler != null)
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
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected void RaiseTitleChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if (eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.Title), new System.AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Title");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Country>> CountryChanging
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
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected bool RaiseCountryChangingEvent(Country newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Country>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Person, Country>>;
			if (eventHandler != null)
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
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected void RaiseCountryChangedEvent(Country oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Country>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person, Country>>;
			if (eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Country>(this, oldValue, this.Country), new System.AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Country");
			}
		}
		public abstract string LastName
		{
			get;
			set;
		}
		public abstract string FirstName
		{
			get;
			set;
		}
		public abstract string Title
		{
			get;
			set;
		}
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
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public abstract partial class Country : INotifyPropertyChanged
	{
		protected Country()
		{
		}
		private readonly System.Delegate[] Events = new System.Delegate[3];
		[SuppressMessageAttribute("Microsoft.Design", "CA1033")]
		private event PropertyChangedEventHandler PropertyChanged
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
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}
		private void RaisePropertyChangedEvent(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
			if (eventHandler != null)
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
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected bool RaiseCountry_nameChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if (eventHandler != null)
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
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected void RaiseCountry_nameChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if (eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Country_name), new System.AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Country_name");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Country, string>> Region_Region_codeChanging
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
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected bool RaiseRegion_Region_codeChangingEvent(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if (eventHandler != null)
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
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		[SuppressMessageAttribute("Microsoft.Design", "CA1030")]
		protected void RaiseRegion_Region_codeChangedEvent(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if (eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Region_Region_code), new System.AsyncCallback(eventHandler.EndInvoke), null);
				this.RaisePropertyChangedEvent("Region_Region_code");
			}
		}
		public abstract string Country_name
		{
			get;
			set;
		}
		public abstract string Region_Region_code
		{
			get;
			set;
		}
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
	#region IPersonCountryDemoContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface IPersonCountryDemoContext
	{
		bool IsDeserializing
		{
			get;
		}
		Country GetCountryByCountry_name(string Country_name);
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
	public sealed class PersonCountryDemoContext : IPersonCountryDemoContext
	{
		public PersonCountryDemoContext()
		{
			List<Person> PersonList = new List<Person>();
			this.myPersonList = PersonList;
			this.myPersonReadOnlyCollection = new ReadOnlyCollection<Person>(PersonList);
			List<Country> CountryList = new List<Country>();
			this.myCountryList = CountryList;
			this.myCountryReadOnlyCollection = new ReadOnlyCollection<Country>(CountryList);
		}
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class;
		private sealed class ConstraintEnforcementCollection<TClass, TProperty> : ICollection<TProperty>
			where TClass : class
		{
			private readonly TClass myInstance;
			private readonly List<TProperty> myList = new List<TProperty>();
			public ConstraintEnforcementCollection(TClass instance, PotentialCollectionModificationCallback<TClass, TProperty> adding, CommittedCollectionModificationCallback<TClass, TProperty> added, PotentialCollectionModificationCallback<TClass, TProperty> removing, CommittedCollectionModificationCallback<TClass, TProperty> removed)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				this.myInstance = instance;
				this.myAdding = adding;
				this.myAdded = added;
				this.myRemoving = removing;
				this.myRemoved = removed;
			}
			private readonly PotentialCollectionModificationCallback<TClass, TProperty> myAdding;
			private readonly CommittedCollectionModificationCallback<TClass, TProperty> myAdded;
			private readonly PotentialCollectionModificationCallback<TClass, TProperty> myRemoving;
			private readonly CommittedCollectionModificationCallback<TClass, TProperty> myRemoved;
			private bool OnAdding(TProperty value)
			{
				if (this.myAdding != null)
				{
					return this.myAdding(this.myInstance, value);
				}
				return true;
			}
			private void OnAdded(TProperty value)
			{
				if (this.myAdded != null)
				{
					this.myAdded(this.myInstance, value);
				}
			}
			private bool OnRemoving(TProperty value)
			{
				if (this.myRemoving != null)
				{
					return this.myRemoving(this.myInstance, value);
				}
				return true;
			}
			private void OnRemoved(TProperty value)
			{
				if (this.myRemoved != null)
				{
					this.myRemoved(this.myInstance, value);
				}
			}
			private System.Collections.IEnumerator GetEnumerator()
			{
				return this.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			public IEnumerator<TProperty> GetEnumerator()
			{
				return this.myList.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (this.OnAdding(item))
				{
					this.myList.Add(item);
					this.OnAdded(item);
				}
			}
			public bool Remove(TProperty item)
			{
				if (this.OnRemoving(item))
				{
					if (this.myList.Remove(item))
					{
						this.OnRemoved(item);
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				for (int i = 0; i < this.myList.Count; ++i)
				{
					this.Remove(this.myList[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return this.myList.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this.myList.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this.myList.Count;
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
		private bool myIsDeserializing;
		public bool IsDeserializing
		{
			get
			{
				return this.myIsDeserializing;
			}
		}
		private readonly Dictionary<string, Country> myCountryCountry_nameDictionary = new Dictionary<string, Country>();
		public Country GetCountryByCountry_name(string Country_name)
		{
			return this.myCountryCountry_nameDictionary[Country_name];
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
		[SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
		[SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
		private bool OnPersonCountryChanging(Person instance, Country newValue)
		{
			if (newValue != null)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
			}
			return true;
		}
		private void OnPersonCountryChanged(Person instance, Country oldValue)
		{
			if (instance.Country != null)
			{
				instance.Country.Person.Add(instance);
			}
			if (oldValue != null)
			{
				oldValue.Person.Remove(instance);
			}
			else
			{
			}
		}
		public Person CreatePerson(string LastName, string FirstName)
		{
			if (!(this.IsDeserializing))
			{
				if (!(this.OnPersonLastNameChanging(null, LastName)))
				{
					throw new ArgumentException("Argument failed constraint enforcement.", "LastName");
				}
				if (!(this.OnPersonFirstNameChanging(null, FirstName)))
				{
					throw new ArgumentException("Argument failed constraint enforcement.", "FirstName");
				}
			}
			return new PersonCore(this, LastName, FirstName);
		}
		private readonly List<Person> myPersonList;
		private readonly ReadOnlyCollection<Person> myPersonReadOnlyCollection;
		public ReadOnlyCollection<Person> PersonCollection
		{
			get
			{
				return this.myPersonReadOnlyCollection;
			}
		}
		#region PersonCore
		private sealed class PersonCore : Person
		{
			public PersonCore(PersonCountryDemoContext context, string LastName, string FirstName)
			{
				this.myContext = context;
				this.myLastName = LastName;
				this.myFirstName = FirstName;
				context.myPersonList.Add(this);
			}
			private readonly PersonCountryDemoContext myContext;
			public override PersonCountryDemoContext Context
			{
				get
				{
					return this.myContext;
				}
			}
			private string myLastName;
			public override string LastName
			{
				get
				{
					return this.myLastName;
				}
				set
				{
					if (value == null)
					{
						return;
					}
					if (!(object.Equals(this.LastName, value)))
					{
						if (this.Context.OnPersonLastNameChanging(this, value))
						{
							if (base.RaiseLastNameChangingEvent(value))
							{
								string oldValue = this.LastName;
								this.myLastName = value;
								base.RaiseLastNameChangedEvent(oldValue);
							}
						}
					}
				}
			}
			private string myFirstName;
			public override string FirstName
			{
				get
				{
					return this.myFirstName;
				}
				set
				{
					if (value == null)
					{
						return;
					}
					if (!(object.Equals(this.FirstName, value)))
					{
						if (this.Context.OnPersonFirstNameChanging(this, value))
						{
							if (base.RaiseFirstNameChangingEvent(value))
							{
								string oldValue = this.FirstName;
								this.myFirstName = value;
								base.RaiseFirstNameChangedEvent(oldValue);
							}
						}
					}
				}
			}
			private string myTitle;
			public override string Title
			{
				get
				{
					return this.myTitle;
				}
				set
				{
					if (!(object.Equals(this.Title, value)))
					{
						if (this.Context.OnPersonTitleChanging(this, value))
						{
							if (base.RaiseTitleChangingEvent(value))
							{
								string oldValue = this.Title;
								this.myTitle = value;
								base.RaiseTitleChangedEvent(oldValue);
							}
						}
					}
				}
			}
			private Country myCountry;
			public override Country Country
			{
				get
				{
					return this.myCountry;
				}
				set
				{
					if (!(object.Equals(this.Country, value)))
					{
						if (this.Context.OnPersonCountryChanging(this, value))
						{
							if (base.RaiseCountryChangingEvent(value))
							{
								Country oldValue = this.Country;
								this.myCountry = value;
								this.Context.OnPersonCountryChanged(this, oldValue);
								base.RaiseCountryChangedEvent(oldValue);
							}
						}
					}
				}
			}
		}
		#endregion // PersonCore
		private bool OnCountryCountry_nameChanging(Country instance, string newValue)
		{
			Country currentInstance = instance;
			if (this.myCountryCountry_nameDictionary.TryGetValue(newValue, out currentInstance))
			{
				if (!(object.Equals(currentInstance, instance)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnCountryCountry_nameChanged(Country instance, string oldValue)
		{
			this.myCountryCountry_nameDictionary.Add(instance.Country_name, instance);
			if (oldValue != null)
			{
				this.myCountryCountry_nameDictionary.Remove(oldValue);
			}
			else
			{
			}
		}
		private bool OnCountryRegion_Region_codeChanging(Country instance, string newValue)
		{
			return true;
		}
		private bool OnCountryPersonAdding(Country instance, Person value)
		{
			return true;
		}
		private void OnCountryPersonAdded(Country instance, Person value)
		{
			if (value != null)
			{
				value.Country = instance;
			}
		}
		private bool OnCountryPersonRemoving(Country instance, Person value)
		{
			return true;
		}
		private void OnCountryPersonRemoved(Country instance, Person value)
		{
			if (value != null)
			{
				value.Country = null;
			}
		}
		public Country CreateCountry(string Country_name)
		{
			if (!(this.IsDeserializing))
			{
				if (!(this.OnCountryCountry_nameChanging(null, Country_name)))
				{
					throw new ArgumentException("Argument failed constraint enforcement.", "Country_name");
				}
			}
			return new CountryCore(this, Country_name);
		}
		private readonly List<Country> myCountryList;
		private readonly ReadOnlyCollection<Country> myCountryReadOnlyCollection;
		public ReadOnlyCollection<Country> CountryCollection
		{
			get
			{
				return this.myCountryReadOnlyCollection;
			}
		}
		#region CountryCore
		private sealed class CountryCore : Country
		{
			public CountryCore(PersonCountryDemoContext context, string Country_name)
			{
				this.myContext = context;
				this.myPerson = new ConstraintEnforcementCollection<Country, Person>(this, new PotentialCollectionModificationCallback<Country, Person>(context.OnCountryPersonAdding), new CommittedCollectionModificationCallback<Country, Person>(context.OnCountryPersonAdded), new PotentialCollectionModificationCallback<Country, Person>(context.OnCountryPersonRemoving), new CommittedCollectionModificationCallback<Country, Person>(context.OnCountryPersonRemoved));
				this.myCountry_name = Country_name;
				context.OnCountryCountry_nameChanged(this, null);
				context.myCountryList.Add(this);
			}
			private readonly PersonCountryDemoContext myContext;
			public override PersonCountryDemoContext Context
			{
				get
				{
					return this.myContext;
				}
			}
			private string myCountry_name;
			public override string Country_name
			{
				get
				{
					return this.myCountry_name;
				}
				set
				{
					if (value == null)
					{
						return;
					}
					if (!(object.Equals(this.Country_name, value)))
					{
						if (this.Context.OnCountryCountry_nameChanging(this, value))
						{
							if (base.RaiseCountry_nameChangingEvent(value))
							{
								string oldValue = this.Country_name;
								this.myCountry_name = value;
								this.Context.OnCountryCountry_nameChanged(this, oldValue);
								base.RaiseCountry_nameChangedEvent(oldValue);
							}
						}
					}
				}
			}
			private string myRegion_Region_code;
			public override string Region_Region_code
			{
				get
				{
					return this.myRegion_Region_code;
				}
				set
				{
					if (!(object.Equals(this.Region_Region_code, value)))
					{
						if (this.Context.OnCountryRegion_Region_codeChanging(this, value))
						{
							if (base.RaiseRegion_Region_codeChangingEvent(value))
							{
								string oldValue = this.Region_Region_code;
								this.myRegion_Region_code = value;
								base.RaiseRegion_Region_codeChangedEvent(oldValue);
							}
						}
					}
				}
			}
			private readonly ICollection<Person> myPerson;
			public override ICollection<Person> Person
			{
				get
				{
					return this.myPerson;
				}
			}
		}
		#endregion // CountryCore
	}
	#endregion // PersonCountryDemoContext
}

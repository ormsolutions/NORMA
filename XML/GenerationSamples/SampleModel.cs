using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
#region Global Support Classes
namespace System
{
	#region Tuple Support
	public static partial class Tuple
	{
		internal static int RotateRight(int value, int places)
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
	#region 2-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2> : System.IEquatable<Tuple<T1, T2>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		public Tuple(T1 item1, T2 item2)
		{
			if ((item1 == null) || (item2 == null))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2>);
		}
		public bool Equals(Tuple<T1, T2> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || !(this.Item2.Equals(other.Item2))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ Tuple.RotateRight(this.Item2.GetHashCode(), 1);
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2})", this.Item1, this.Item2);
		}
		public static bool operator==(Tuple<T1, T2> tuple1, Tuple<T1, T2> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2> tuple1, Tuple<T1, T2> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 2-ary Tuple
	#region 3-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3> : System.IEquatable<Tuple<T1, T2, T3>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			if ((item1 == null) || ((item2 == null) || (item3 == null)))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3>);
		}
		public bool Equals(Tuple<T1, T2, T3> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || !(this.Item3.Equals(other.Item3)))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ Tuple.RotateRight(this.Item3.GetHashCode(), 2));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3})", this.Item1, this.Item2, this.Item3);
		}
		public static bool operator==(Tuple<T1, T2, T3> tuple1, Tuple<T1, T2, T3> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3> tuple1, Tuple<T1, T2, T3> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 3-ary Tuple
	#region 4-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4> : System.IEquatable<Tuple<T1, T2, T3, T4>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || (item4 == null))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || !(this.Item4.Equals(other.Item4))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ Tuple.RotateRight(this.Item4.GetHashCode(), 3)));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4})", this.Item1, this.Item2, this.Item3, this.Item4);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4> tuple1, Tuple<T1, T2, T3, T4> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4> tuple1, Tuple<T1, T2, T3, T4> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 4-ary Tuple
	#region 5-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4, T5> : System.IEquatable<Tuple<T1, T2, T3, T4, T5>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T5 Item5;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || (item5 == null)))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || (!(this.Item4.Equals(other.Item4)) || !(this.Item5.Equals(other.Item5)))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.Item4.GetHashCode(), 3) ^ Tuple.RotateRight(this.Item5.GetHashCode(), 4))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4, T5> tuple1, Tuple<T1, T2, T3, T4, T5> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4, T5> tuple1, Tuple<T1, T2, T3, T4, T5> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 5-ary Tuple
	#region 6-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6> : System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T5 Item5;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T6 Item6;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || (item6 == null))))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || (!(this.Item4.Equals(other.Item4)) || (!(this.Item5.Equals(other.Item5)) || !(this.Item6.Equals(other.Item6))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.Item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.Item5.GetHashCode(), 4) ^ Tuple.RotateRight(this.Item6.GetHashCode(), 5)))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6> tuple1, Tuple<T1, T2, T3, T4, T5, T6> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6> tuple1, Tuple<T1, T2, T3, T4, T5, T6> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 6-ary Tuple
	#region 7-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7> : System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T5 Item5;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T6 Item6;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T7 Item7;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || (item7 == null)))))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || (!(this.Item4.Equals(other.Item4)) || (!(this.Item5.Equals(other.Item5)) || (!(this.Item6.Equals(other.Item6)) || !(this.Item7.Equals(other.Item7)))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.Item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.Item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.Item6.GetHashCode(), 5) ^ Tuple.RotateRight(this.Item7.GetHashCode(), 6))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 7-ary Tuple
	#region 8-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T5 Item5;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T6 Item6;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T7 Item7;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T8 Item8;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || (item8 == null))))))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			this.Item8 = item8;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || (!(this.Item4.Equals(other.Item4)) || (!(this.Item5.Equals(other.Item5)) || (!(this.Item6.Equals(other.Item6)) || (!(this.Item7.Equals(other.Item7)) || !(this.Item8.Equals(other.Item8))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.Item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.Item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.Item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this.Item7.GetHashCode(), 6) ^ Tuple.RotateRight(this.Item8.GetHashCode(), 7)))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 8-ary Tuple
	#region 9-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T5 Item5;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T6 Item6;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T7 Item7;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T8 Item8;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T9 Item9;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || (item9 == null)))))))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			this.Item8 = item8;
			this.Item9 = item9;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || (!(this.Item4.Equals(other.Item4)) || (!(this.Item5.Equals(other.Item5)) || (!(this.Item6.Equals(other.Item6)) || (!(this.Item7.Equals(other.Item7)) || (!(this.Item8.Equals(other.Item8)) || !(this.Item9.Equals(other.Item9)))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.Item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.Item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.Item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this.Item7.GetHashCode(), 6) ^ (Tuple.RotateRight(this.Item8.GetHashCode(), 7) ^ Tuple.RotateRight(this.Item9.GetHashCode(), 8))))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 9-ary Tuple
	#region 10-ary Tuple
	public static partial class Tuple
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
	[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1005")]
	[System.ComponentModel.ImmutableObjectAttribute(true)]
	public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : System.IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T1 Item1;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T2 Item2;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T3 Item3;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T4 Item4;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T5 Item5;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T6 Item6;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T7 Item7;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T8 Item8;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T9 Item9;
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1051")]
		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Security", "CA2104")]
		public readonly T10 Item10;
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
		{
			if ((item1 == null) || ((item2 == null) || ((item3 == null) || ((item4 == null) || ((item5 == null) || ((item6 == null) || ((item7 == null) || ((item8 == null) || ((item9 == null) || (item10 == null))))))))))
			{
				throw new System.ArgumentNullException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			this.Item8 = item8;
			this.Item9 = item9;
			this.Item10 = item10;
		}
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>);
		}
		public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
		{
			if ((other == null) || (!(this.Item1.Equals(other.Item1)) || (!(this.Item2.Equals(other.Item2)) || (!(this.Item3.Equals(other.Item3)) || (!(this.Item4.Equals(other.Item4)) || (!(this.Item5.Equals(other.Item5)) || (!(this.Item6.Equals(other.Item6)) || (!(this.Item7.Equals(other.Item7)) || (!(this.Item8.Equals(other.Item8)) || (!(this.Item9.Equals(other.Item9)) || !(this.Item10.Equals(other.Item10))))))))))))
			{
				return false;
			}
			return true;
		}
		public override int GetHashCode()
		{
			return this.Item1.GetHashCode() ^ (Tuple.RotateRight(this.Item2.GetHashCode(), 1) ^ (Tuple.RotateRight(this.Item3.GetHashCode(), 2) ^ (Tuple.RotateRight(this.Item4.GetHashCode(), 3) ^ (Tuple.RotateRight(this.Item5.GetHashCode(), 4) ^ (Tuple.RotateRight(this.Item6.GetHashCode(), 5) ^ (Tuple.RotateRight(this.Item7.GetHashCode(), 6) ^ (Tuple.RotateRight(this.Item8.GetHashCode(), 7) ^ (Tuple.RotateRight(this.Item9.GetHashCode(), 8) ^ Tuple.RotateRight(this.Item10.GetHashCode(), 9)))))))));
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public string ToString(System.IFormatProvider provider)
		{
			return string.Format(provider, "({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10);
		}
		public static bool operator==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator!=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple2)
		{
			return !(tuple1 == tuple2);
		}
	}
	#endregion // 10-ary Tuple
	#region Property Change Event Support
	public interface IPropertyChangeEventArgs<TProperty>
	{
		TProperty OldValue
		{
			get;
		}
		TProperty NewValue
		{
			get;
		}
	}
	public sealed class PropertyChangingEventArgs<TProperty> : CancelEventArgs, IPropertyChangeEventArgs<TProperty>
	{
		private readonly TProperty myOldValue;
		private readonly TProperty myNewValue;
		public PropertyChangingEventArgs(TProperty oldValue, TProperty newValue)
		{
			this.myOldValue = oldValue;
			this.myNewValue = newValue;
		}
		public TProperty OldValue
		{
			get
			{
				return this.myOldValue;
			}
		}
		public TProperty NewValue
		{
			get
			{
				return this.myNewValue;
			}
		}
	}
	public sealed class PropertyChangedEventArgs<TProperty> : EventArgs, IPropertyChangeEventArgs<TProperty>
	{
		private readonly TProperty myOldValue;
		private readonly TProperty myNewValue;
		public PropertyChangedEventArgs(TProperty oldValue, TProperty newValue)
		{
			this.myOldValue = oldValue;
			this.myNewValue = newValue;
		}
		public TProperty OldValue
		{
			get
			{
				return this.myOldValue;
			}
		}
		public TProperty NewValue
		{
			get
			{
				return this.myNewValue;
			}
		}
	}
	#endregion // Property Change Event Support
}
#endregion // Global Support Classes

// <ao:Object type="EntityType" id="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" name="Person"><ao:AbsorbedObject type="ValueType" ref="_73D777FE-CDC8-40DB-A544-442931809885" name="Person_id" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_DC92FEF0-36B3-4145-BDEA-2362CCC79718" oppositeRoleRef="_FFF695AD-A1B7-4970-A717-D8D40BE4249C" oppositeRoleName="Person_id" mandatory="" /><ao:AbsorbedObject type="ValueType" ref="_1AA2E229-66B0-42EA-8EB6-FAC6C3A2A2FB" name="FirstName" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_70A330F0-1AC2-486B-8469-EC115A515AC1" oppositeRoleRef="_0D9E47FC-891C-45C5-95E6-CEC245F61265" oppositeRoleName="FirstName" mandatory="" /><ao:AbsorbedObject type="ValueType" ref="_AEF8A0BD-CED0-42E2-8859-E06613051DEA" name="LastName" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_AFA5D9D8-8EC4-460E-A2E0-66E8F2FF598D" oppositeRoleRef="_089350B7-DAB7-4338-A554-275973B70EA6" oppositeRoleName="LastName" mandatory="" /><ao:AbsorbedObject type="ValueType" ref="_91E37AB6-231B-4463-BF39-8BA50E5183A6" name="SocialSecurityNumber" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_AAE038F3-BF91-4319-854E-792C7041AE36" oppositeRoleRef="_512DF478-3B6D-4944-8B9A-1A95C427BBDF" oppositeRoleName="SocialSecurityNumber" mandatory="" /><ao:AbsorbedObject type="ValueType" ref="_D4808010-4C18-442E-A5E4-B69A33A5FA88" name="ValueType1" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_D2285B0E-026A-4CE5-A4EF-A9914213ED03" oppositeRoleRef="_92066292-C1C5-4063-9652-E071F922267B" oppositeRoleName="ValueType1" mandatory="" /><ao:AbsorbedObject type="ValueType" ref="_551FCEDA-66D0-49D3-8D87-B1C551D70A8F" name="NickName" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_6FA75148-1A54-4675-BD51-E8C95E37DD51" oppositeRoleRef="_2A32CF98-5EC6-470F-999D-BF8B65CB9B64" oppositeRoleName="NickName" mandatory="" /><ao:AbsorbedObject type="ValueType" ref="_D4808010-4C18-442E-A5E4-B69A33A5FA88" name="ValueType1" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_37C4F90B-50F3-4655-9946-C6D796CA4FDB" oppositeRoleRef="_CD154F6A-69C3-4209-8EDF-D6DEF62954A0" oppositeRoleName="ValueType1" mandatory="" /><ao:AbsorbedObject type="EntityType" ref="_FA76F159-1799-4D93-BA27-0FB3A1853415" name="Gender" unique="false" multiplicity="" thisRoleName="Person" thisRoleRef="_94C9DD40-B34A-4BF7-A0B8-CAEA067FF7A0" oppositeRoleRef="_49F9204D-9900-4662-80D3-CC05BCBAAE8C" oppositeRoleName="Gender" mandatory=""><ao:AbsorbedObject type="ValueType" ref="_C0134671-0E5C-440F-9811-2FC41A94E464" name="Gender_Code" unique="false" multiplicity="" thisRoleName="Gender" thisRoleRef="_64B9B775-0F9D-463A-AB84-20E751118B53" oppositeRoleRef="_7C1ACF87-30DE-4F21-BF5D-8977B63D6078" oppositeRoleName="Gender_Code" mandatory="" /></ao:AbsorbedObject><ao:RelatedObject factRef="_8608B920-7E81-47B0-87A7-2A373F5AA2B4" roleRef="_AFD8D46E-1189-4B92-A7D3-3EAAC57AD629" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_9D83D21B-1F76-4368-A70B-EC1932BB4C42" oppositeObjectRef="_CFFA4225-A7B4-457F-818B-4D899AA157AB" oppositeObjectName="HatType" oppositeRoleName="HatType" /><ao:RelatedObject factRef="_8B053DEF-BA79-4A8F-804F-5561308F69B3" roleRef="_9F1F7C52-3E4E-4161-ACB9-E417F8D293A6" arity="2" roleName="Wife" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_089D0A43-FD48-4580-A6A6-45A6FA189DEA" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Husband" /><ao:RelatedObject factRef="_8B053DEF-BA79-4A8F-804F-5561308F69B3" roleRef="_089D0A43-FD48-4580-A6A6-45A6FA189DEA" arity="2" roleName="Husband" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_9F1F7C52-3E4E-4161-ACB9-E417F8D293A6" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Wife" /><ao:RelatedObject factRef="_F88AF1BA-35E2-4235-9C8B-0C0A4647707D" roleRef="_F6390EAB-A049-4C16-8FB4-50BE44A44364" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_FB7333FB-CE11-4A31-B7DE-6F20958A525A" oppositeObjectRef="_779E9788-0C2F-44B6-B958-AEEC22593966" oppositeObjectName="Task" oppositeRoleName="Task" /><ao:RelatedObject factRef="_C0918686-ECCA-4775-BE23-1304918E4EF5" roleRef="_0ABD30A1-BF17-4110-9340-3765A1C76BCA" arity="2" roleName="DrivenByPerson" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_18AE032D-6C6B-46C4-9383-D3CB9387FA60" oppositeObjectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" oppositeObjectName="Car" oppositeRoleName="DrivesCar" /><ao:RelatedObject factRef="_78B61C80-E4BE-460B-9714-AFB5F6E88B1E" roleRef="_ED495E3B-6C47-4D22-B01C-0E0C805C6664" arity="2" roleName="OwnedByPerson" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_94205691-F9E1-47BD-A2D4-B2698FB19A25" oppositeObjectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" oppositeObjectName="Car" oppositeRoleName="OwnsCar" /><ao:RelatedObject factRef="_5A20BCB8-D3B9-4BD4-9A33-ADE0D7B8062B" roleRef="_310FF4C1-DA3B-4C8D-8892-C43F6452245C" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_D5C1A5F1-D376-4606-A991-4A3F55206DB4" oppositeObjectRef="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" oppositeObjectName="Date" oppositeRoleName="Date" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_9496C251-F06F-4383-93FB-F9BF04A523CB" arity="4" roleName="Buyer" mandatory="" unique="true" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_7C8166A7-F037-493E-BFB7-2A1634AB2238" arity="4" roleName="Seller" mandatory="" unique="true" /><ao:RelatedObject factRef="_707264EE-47B8-40E6-B57A-F54720500160" roleRef="_CF6F1E78-7D74-4ED9-B3FA-FD7141238AB4" arity="1" roleName="Person" mandatory="" unique="false" /><ao:RelatedObject factRef="_7EC274AC-89F4-4329-89DB-78437FDD0EAB" roleRef="_D9FAD5DD-0F8F-4685-B7B9-9539CD3E2E86" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_4CB1D9D1-0A61-42B7-8FD6-E1073A40C1C4" oppositeObjectRef="_BFB2A8BF-78DB-4EDE-B2ED-24DFF73EAA63" oppositeObjectName="Death" oppositeRoleName="Death" /><ao:RelatedObject factRef="_AFBFC2BF-A410-41CD-8EAB-AA0432201D08" roleRef="_2DC337A4-AE48-4EFD-8A48-D212D8F2E1FB" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_A9D03484-569E-4839-A9EE-B59CD75E8CAE" oppositeObjectRef="_E3BF7F91-1182-4A4F-8C4D-A0331B34E9A4" oppositeObjectName="Male" oppositeRoleName="Male" /><ao:RelatedObject factRef="_0CA2A077-8AC2-4BAB-89BE-A353ADDE6875" roleRef="_8E6158FD-6C2F-4220-9294-8F358936935C" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_C987E3E9-A976-4064-ABCE-A4EA16BC2535" oppositeObjectRef="_B6D73AD6-31DD-4147-AB22-BF6E9D994CBE" oppositeObjectName="Female" oppositeRoleName="Female" /><ao:RelatedObject factRef="_2CFDEFEB-8841-4563-9E9A-C89EA8AD5208" roleRef="_D1871C44-65D4-41A5-86A6-6AEFB0F9A365" arity="2" roleName="Person" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_F6461991-31A0-4734-88C3-C94C7AA8E785" oppositeObjectRef="_D15A716E-037F-4263-B4FF-09CAC6B69738" oppositeObjectName="Child" oppositeRoleName="Child" /><ao:RelatedObject factRef="_B3958780-E433-4992-963B-E34C33E62DB7" roleRef="_65D25484-9FF7-407D-B870-B628A66A5A0F" arity="1" roleName="Person" mandatory="" unique="false" /></ao:Object><ao:Object type="EntityType" id="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" name="Date"><ao:AbsorbedObject type="ValueType" ref="_F72BE752-7E12-40F3-9B26-BF303B5E2A50" name="ymd" unique="false" multiplicity="" thisRoleName="Date" thisRoleRef="_0F3F2BC8-D785-4E4C-B953-A165DAAAFE83" oppositeRoleRef="_D7E26580-4FE3-4D4F-B088-A31B40EB44C8" oppositeRoleName="ymd" mandatory="" /><ao:RelatedObject factRef="_5A20BCB8-D3B9-4BD4-9A33-ADE0D7B8062B" roleRef="_D5C1A5F1-D376-4606-A991-4A3F55206DB4" arity="2" roleName="Date" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_310FF4C1-DA3B-4C8D-8892-C43F6452245C" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_1657F414-1F65-41C5-935F-FACC31E02630" arity="4" roleName="SaleDate" mandatory="" unique="true" /><ao:RelatedObject factRef="_AF28BA29-F7E4-485E-9683-442A9B596015" roleRef="_9CCEBE01-8F6E-4909-8CC4-B3145795169F" arity="2" roleName="Date" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_0AE5870D-43CB-45E4-B252-5B06DA9A5443" oppositeObjectRef="_BFB2A8BF-78DB-4EDE-B2ED-24DFF73EAA63" oppositeObjectName="Death" oppositeRoleName="Death" /></ao:Object><ao:Object type="EntityType" id="_CFFA4225-A7B4-457F-818B-4D899AA157AB" name="HatType"><ao:AbsorbedObject type="ValueType" ref="_CEF8D63C-7F55-4227-8C84-84B1BABF76E7" name="ColorARGB" unique="false" multiplicity="" thisRoleName="HatType" thisRoleRef="_B392D051-FF00-401F-BC87-BE9E59768999" oppositeRoleRef="_607260C5-A9AF-4D26-81BE-C06C241E2AA4" oppositeRoleName="ColorARGB" mandatory="" /><ao:RelatedObject factRef="_8608B920-7E81-47B0-87A7-2A373F5AA2B4" roleRef="_9D83D21B-1F76-4368-A70B-EC1932BB4C42" arity="2" roleName="HatType" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_AFD8D46E-1189-4B92-A7D3-3EAAC57AD629" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /><ao:AbsorbedObject type="EntityType" ref="_43FB5FA3-F8F2-4600-9C71-00AD765B3BE5" name="HatTypeStyle" unique="false" multiplicity="" thisRoleName="HatType" thisRoleRef="_A61AEFEE-70CE-466F-A3F2-0FBD558D5F4E" oppositeRoleRef="_863655CF-5EF6-4E6A-9006-B5923E146D2F" oppositeRoleName="HatTypeStyle" mandatory=""><ao:AbsorbedObject type="ValueType" ref="_990F031F-0D4A-42FD-A66E-F7405758BAD7" name="HatTypeStyle_Description" unique="false" multiplicity="" thisRoleName="HatTypeStyle" thisRoleRef="_D9F45E74-98C4-4122-9313-118DD1FFDF83" oppositeRoleRef="_0DA0D8C0-E9D3-49EF-93BC-4915BFE434B1" oppositeRoleName="HatTypeStyle_Description" mandatory="" /></ao:AbsorbedObject></ao:Object><ao:Object type="EntityType" id="_779E9788-0C2F-44B6-B958-AEEC22593966" name="Task"><ao:AbsorbedObject type="ValueType" ref="_388FC80E-A3B9-4AF0-A282-107C93DF5988" name="Task_id" unique="false" multiplicity="" thisRoleName="Task" thisRoleRef="_D38BF87C-33BA-4BFC-9D23-32F52F333AB6" oppositeRoleRef="_66E039F6-38D1-445D-A3E0-02277CA0907E" oppositeRoleName="Task_id" mandatory="" /><ao:RelatedObject factRef="_F88AF1BA-35E2-4235-9C8B-0C0A4647707D" roleRef="_FB7333FB-CE11-4A31-B7DE-6F20958A525A" arity="2" roleName="Task" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_F6390EAB-A049-4C16-8FB4-50BE44A44364" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /></ao:Object><ao:Object type="EntityType" id="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" name="Car"><ao:AbsorbedObject type="ValueType" ref="_AA4AFE18-F3AF-40FB-848F-FD6A3A3F3F1A" name="vin" unique="false" multiplicity="" thisRoleName="Car" thisRoleRef="_24FBA18D-9625-4116-86FE-F397E04E9BD2" oppositeRoleRef="_BBBCEBD0-2688-4A0B-860D-2F8BD5096DDE" oppositeRoleName="vin" mandatory="" /><ao:RelatedObject factRef="_C0918686-ECCA-4775-BE23-1304918E4EF5" roleRef="_18AE032D-6C6B-46C4-9383-D3CB9387FA60" arity="2" roleName="DrivesCar" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_0ABD30A1-BF17-4110-9340-3765A1C76BCA" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="DrivenByPerson" /><ao:RelatedObject factRef="_78B61C80-E4BE-460B-9714-AFB5F6E88B1E" roleRef="_94205691-F9E1-47BD-A2D4-B2698FB19A25" arity="2" roleName="OwnsCar" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_ED495E3B-6C47-4D22-B01C-0E0C805C6664" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="OwnedByPerson" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_C0203916-6D57-4C48-9151-B36F507D9D18" arity="4" roleName="CarSold" mandatory="" unique="true" /><ao:RelatedObject factRef="_780A6DD3-CD65-4533-8870-7DE873EBAEF5" roleRef="_0BC3478C-9D88-4C4B-9754-BB62DCC7F189" arity="3" roleName="Car" mandatory="" unique="true" /></ao:Object><ao:Object type="ObjectifiedType" id="_BFB2A8BF-78DB-4EDE-B2ED-24DFF73EAA63" name="Death"><ao:AbsorbedObject type="EntityType" ref="_CA3BAF09-F895-4A2D-A840-E911D8FE157A" name="DeathCause" unique="false" multiplicity="" thisRoleName="Death" thisRoleRef="_90A1343B-5AFD-4EDF-ABF3-868915B25477" oppositeRoleRef="_62D448CE-F03C-4DB9-81A0-0BDDCE1AEFAA" oppositeRoleName="DeathCause" mandatory=""><ao:AbsorbedObject type="ValueType" ref="_CFE7878B-52BD-4BAC-BDD9-8A847135D647" name="Type" unique="false" multiplicity="" thisRoleName="DeathCause" thisRoleRef="_065B8F23-0359-4675-8D1B-9E03E97E2529" oppositeRoleRef="_786836A9-1AB2-4F1C-88B0-83290DA1DCC0" oppositeRoleName="Type" mandatory="" /></ao:AbsorbedObject><ao:RelatedObject factRef="_7EC274AC-89F4-4329-89DB-78437FDD0EAB" roleRef="_4CB1D9D1-0A61-42B7-8FD6-E1073A40C1C4" arity="2" roleName="Death" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_D9FAD5DD-0F8F-4685-B7B9-9539CD3E2E86" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /><ao:RelatedObject factRef="_AF28BA29-F7E4-485E-9683-442A9B596015" roleRef="_0AE5870D-43CB-45E4-B252-5B06DA9A5443" arity="2" roleName="Death" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_9CCEBE01-8F6E-4909-8CC4-B3145795169F" oppositeObjectRef="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" oppositeObjectName="Date" oppositeRoleName="Date" /><ao:RelatedObject factRef="_5EE8B00B-4024-40BA-87E2-F85CB5546507" roleRef="_03B4EB5F-3A90-45D2-95EB-EE0AEA9844B8" arity="2" roleName="Death" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_1021A8DD-8005-4AB6-940F-022A4523F518" oppositeObjectRef="_DDE8D226-F41E-4524-8BD8-051DDBC37E21" oppositeObjectName="NaturalDeath" oppositeRoleName="NaturalDeath" /><ao:RelatedObject factRef="_371E6F7A-A9E9-46EB-9247-8E2A7D7111A8" roleRef="_C0B623D1-7CD0-49CC-B419-4F2EFAEA77B8" arity="2" roleName="Death" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_AE8E2980-8D1B-4CEA-8709-61CE188384C8" oppositeObjectRef="_ACEFBB61-F5A3-4B7C-9C33-1AF6FF101637" oppositeObjectName="UnnaturalDeath" oppositeRoleName="UnnaturalDeath" /></ao:Object><ao:Object type="EntityType" id="_DDE8D226-F41E-4524-8BD8-051DDBC37E21" name="NaturalDeath"><ao:RelatedObject factRef="_B1406FD9-CAAE-48BD-9AFB-C432232A32C4" roleRef="_965293C9-8AD7-4C73-B54C-818DC450C11B" arity="1" roleName="NaturalDeath" mandatory="" unique="false" /><ao:RelatedObject factRef="_5EE8B00B-4024-40BA-87E2-F85CB5546507" roleRef="_1021A8DD-8005-4AB6-940F-022A4523F518" arity="2" roleName="NaturalDeath" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_03B4EB5F-3A90-45D2-95EB-EE0AEA9844B8" oppositeObjectRef="_BFB2A8BF-78DB-4EDE-B2ED-24DFF73EAA63" oppositeObjectName="Death" oppositeRoleName="Death" /></ao:Object><ao:Object type="EntityType" id="_ACEFBB61-F5A3-4B7C-9C33-1AF6FF101637" name="UnnaturalDeath"><ao:RelatedObject factRef="_0FD5DEF7-B3C3-49A6-837D-DE91FCE968DF" roleRef="_750CCACC-4358-47DB-99BD-D0C33BBEA1D8" arity="1" roleName="UnnaturalDeath" mandatory="" unique="false" /><ao:RelatedObject factRef="_7D989A2C-9CFF-464F-822E-FE41B6758ACE" roleRef="_AACEB3BA-6D92-4927-B1AF-30CCEEC980D7" arity="1" roleName="UnnaturalDeath" mandatory="" unique="false" /><ao:RelatedObject factRef="_371E6F7A-A9E9-46EB-9247-8E2A7D7111A8" roleRef="_AE8E2980-8D1B-4CEA-8709-61CE188384C8" arity="2" roleName="UnnaturalDeath" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_C0B623D1-7CD0-49CC-B419-4F2EFAEA77B8" oppositeObjectRef="_BFB2A8BF-78DB-4EDE-B2ED-24DFF73EAA63" oppositeObjectName="Death" oppositeRoleName="Death" /></ao:Object><ao:Object type="EntityType" id="_E3BF7F91-1182-4A4F-8C4D-A0331B34E9A4" name="Male"><ao:RelatedObject factRef="_AFBFC2BF-A410-41CD-8EAB-AA0432201D08" roleRef="_A9D03484-569E-4839-A9EE-B59CD75E8CAE" arity="2" roleName="Male" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_2DC337A4-AE48-4EFD-8A48-D212D8F2E1FB" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /><ao:RelatedObject factRef="_6FAEAA29-BE6F-4B58-8DB8-54F4B39E9143" roleRef="_F3C345B1-6B6C-4C45-A4B0-DC62EFA4BCEC" arity="3" roleName="Male" mandatory="" unique="true" /><ao:RelatedObject factRef="_DB6A6290-1E22-4203-88CE-ED2E03DA19FB" roleRef="_4D72437D-3FDF-414D-BE79-ECA53C3B337D" arity="2" roleName="Male" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_EC45C05E-8DCA-4B2E-BF98-D90AAEF12B40" oppositeObjectRef="_D15A716E-037F-4263-B4FF-09CAC6B69738" oppositeObjectName="Child" oppositeRoleName="Child" /></ao:Object><ao:Object type="EntityType" id="_B6D73AD6-31DD-4147-AB22-BF6E9D994CBE" name="Female"><ao:RelatedObject factRef="_0CA2A077-8AC2-4BAB-89BE-A353ADDE6875" roleRef="_C987E3E9-A976-4064-ABCE-A4EA16BC2535" arity="2" roleName="Female" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_8E6158FD-6C2F-4220-9294-8F358936935C" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /><ao:RelatedObject factRef="_6FAEAA29-BE6F-4B58-8DB8-54F4B39E9143" roleRef="_3BB87141-9223-4C3A-881E-F8B77C4316E6" arity="3" roleName="Female" mandatory="" unique="true" /><ao:RelatedObject factRef="_599A1626-86BA-4E97-8BA0-18F6584D6F5C" roleRef="_9EAA3D4E-118F-48E1-8F95-21E223545BC7" arity="2" roleName="Female" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_FD76E0F8-4497-47C7-A66F-3D428A8B6009" oppositeObjectRef="_D15A716E-037F-4263-B4FF-09CAC6B69738" oppositeObjectName="Child" oppositeRoleName="Child" /></ao:Object><ao:Object type="EntityType" id="_B920FB79-BCAF-472D-9D1D-5625E804AEAC" name="BirthOrder"><ao:AbsorbedObject type="ValueType" ref="_6DC673C6-4A81-4503-901E-76E953A1CF51" name="BirthOrderNr" unique="false" multiplicity="" thisRoleName="BirthOrder" thisRoleRef="_1992B705-0659-4ED1-BB2D-491D502848C7" oppositeRoleRef="_3C92E223-EE65-48D2-8B3B-7620EC3B7B4C" oppositeRoleName="BirthOrderNr" mandatory="" /><ao:RelatedObject factRef="_6FAEAA29-BE6F-4B58-8DB8-54F4B39E9143" roleRef="_AFC60756-6AA3-4CD0-90B1-3717BC6359C5" arity="3" roleName="BirthOrder" mandatory="" unique="true" /><ao:RelatedObject factRef="_C495452A-5B60-4660-9B0B-CEE4CE0F465E" roleRef="_D2F06E01-2013-41F7-AF5F-8C018E8431E9" arity="2" roleName="BirthOrder" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_8084EC0C-69B5-4BB2-B306-1CE593984C7D" oppositeObjectRef="_D15A716E-037F-4263-B4FF-09CAC6B69738" oppositeObjectName="Child" oppositeRoleName="Child" /></ao:Object><ao:Object type="ObjectifiedType" id="_D15A716E-037F-4263-B4FF-09CAC6B69738" name="Child"><ao:RelatedObject factRef="_DB6A6290-1E22-4203-88CE-ED2E03DA19FB" roleRef="_EC45C05E-8DCA-4B2E-BF98-D90AAEF12B40" arity="2" roleName="Child" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_4D72437D-3FDF-414D-BE79-ECA53C3B337D" oppositeObjectRef="_E3BF7F91-1182-4A4F-8C4D-A0331B34E9A4" oppositeObjectName="Male" oppositeRoleName="Male" /><ao:RelatedObject factRef="_C495452A-5B60-4660-9B0B-CEE4CE0F465E" roleRef="_8084EC0C-69B5-4BB2-B306-1CE593984C7D" arity="2" roleName="Child" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_D2F06E01-2013-41F7-AF5F-8C018E8431E9" oppositeObjectRef="_B920FB79-BCAF-472D-9D1D-5625E804AEAC" oppositeObjectName="BirthOrder" oppositeRoleName="BirthOrder" /><ao:RelatedObject factRef="_599A1626-86BA-4E97-8BA0-18F6584D6F5C" roleRef="_FD76E0F8-4497-47C7-A66F-3D428A8B6009" arity="2" roleName="Child" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_9EAA3D4E-118F-48E1-8F95-21E223545BC7" oppositeObjectRef="_B6D73AD6-31DD-4147-AB22-BF6E9D994CBE" oppositeObjectName="Female" oppositeRoleName="Female" /><ao:RelatedObject factRef="_2CFDEFEB-8841-4563-9E9A-C89EA8AD5208" roleRef="_F6461991-31A0-4734-88C3-C94C7AA8E785" arity="2" roleName="Child" mandatory="" unique="false" multiplicity="" oppositeRoleRef="_D1871C44-65D4-41A5-86A6-6AEFB0F9A365" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /></ao:Object><ao:Association name="PersonBoughtCarFromPersonOnDate" id="_06C709AC-304C-491C-8846-D2A1630F51F2"><ao:RelatedObject type="EntityType" objectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" roleRef="_9496C251-F06F-4383-93FB-F9BF04A523CB" className="Person" unique="false" roleName="Buyer" /><ao:RelatedObject type="EntityType" objectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" roleRef="_C0203916-6D57-4C48-9151-B36F507D9D18" className="Car" unique="false" roleName="CarSold" /><ao:RelatedObject type="EntityType" objectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" roleRef="_7C8166A7-F037-493E-BFB7-2A1634AB2238" className="Person" unique="false" roleName="Seller" /><ao:RelatedObject type="EntityType" objectRef="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" roleRef="_1657F414-1F65-41C5-935F-FACC31E02630" className="Date" unique="false" roleName="SaleDate" /></ao:Association><ao:Association name="Child" id="_6FAEAA29-BE6F-4B58-8DB8-54F4B39E9143"><ao:RelatedObject type="EntityType" objectRef="_E3BF7F91-1182-4A4F-8C4D-A0331B34E9A4" roleRef="_F3C345B1-6B6C-4C45-A4B0-DC62EFA4BCEC" className="Male" unique="false" roleName="Male" /><ao:RelatedObject type="EntityType" objectRef="_B920FB79-BCAF-472D-9D1D-5625E804AEAC" roleRef="_AFC60756-6AA3-4CD0-90B1-3717BC6359C5" className="BirthOrder" unique="false" roleName="BirthOrder" /><ao:RelatedObject type="EntityType" objectRef="_B6D73AD6-31DD-4147-AB22-BF6E9D994CBE" roleRef="_3BB87141-9223-4C3A-881E-F8B77C4316E6" className="Female" unique="false" roleName="Female" /></ao:Association><ao:Association name="Review" id="_780A6DD3-CD65-4533-8870-7DE873EBAEF5"><ao:RelatedObject type="EntityType" objectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" roleRef="_0BC3478C-9D88-4C4B-9754-BB62DCC7F189" className="Car" unique="false" roleName="Car" /><ao:AbsorbedObject type="EntityType" ref="_65DBE376-74EA-40A8-8753-8A8CA24259C8" name="Rating" roleName="" thisRoleRef="_15617B90-1712-4B85-9506-2007DE9EEEC0"><ao:AbsorbedObject type="EntityType" ref="_437E0803-738F-4314-AD87-C2492305DF10" name="Nr" unique="false" multiplicity="" thisRoleName="Rating" thisRoleRef="_DEE46F2B-9ED6-477C-A6C2-9E37A44E6263" oppositeRoleRef="_EB80D766-ED3A-4511-B5C5-F8E7CAB99B8A" oppositeRoleName="Nr" mandatory=""><ao:AbsorbedObject type="ValueType" ref="_2080B74C-CA56-4349-B83B-786794656BA7" name="Integer" unique="false" multiplicity="" thisRoleName="Nr" thisRoleRef="_603F4EE0-C3B9-4CB9-918F-1D7AC2224BD9" oppositeRoleRef="_AB846AA8-64FD-4EB2-920F-6F8C1D4F16FB" oppositeRoleName="Integer" mandatory="" /></ao:AbsorbedObject></ao:AbsorbedObject><ao:AbsorbedObject type="EntityType" ref="_C93BB2C9-7F70-49DE-8295-07997960D5F3" name="Criteria" roleName="" thisRoleRef="_B8FCF342-1134-4FE2-8B10-5DF944662B07"><ao:AbsorbedObject type="ValueType" ref="_1232809E-4674-40C8-8E55-BDA203ED9FCE" name="Name" unique="false" multiplicity="" thisRoleName="Criteria" thisRoleRef="_1AE48A9B-5F7B-4AFF-81B6-595607EBB940" oppositeRoleRef="_F323AB9E-5B2E-4434-A3D0-321B838DE939" oppositeRoleName="Name" mandatory="" /></ao:AbsorbedObject></ao:Association>

namespace ORMClassGenerator
{
	namespace SampleModel
	{
		#region Person
		public abstract partial class Person : INotifyPropertyChanged
		{
			protected Person()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[20];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract int Person_id
			{
				get;
				set;
			}
			public abstract string FirstName
			{
				get;
				set;
			}
			public abstract string LastName
			{
				get;
				set;
			}
			public abstract string SocialSecurityNumber
			{
				get;
				set;
			}
			public abstract int ValueType1
			{
				get;
				set;
			}
			public abstract string NickName
			{
				get;
				set;
			}
			public abstract int ValueType1
			{
				get;
				set;
			}
			public abstract string Gender
			{
				get;
				set;
			}
			public abstract HatType HatType
			{
				get;
				set;
			}
			public abstract Person Husband
			{
				get;
				set;
			}
			public abstract Person Wife
			{
				get;
				set;
			}
			public abstract Task Task
			{
				get;
				set;
			}
			public abstract Car DrivesCar
			{
				get;
				set;
			}
			public abstract Car OwnsCar
			{
				get;
				set;
			}
			public abstract Date Date
			{
				get;
				set;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsBuyer
			{
				get;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsSeller
			{
				get;
			}
			public abstract ICollection<DeathAssociation> DeathAsPerson
			{
				get;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public abstract Male Male
			{
				get;
				set;
			}
			public abstract Female Female
			{
				get;
				set;
			}
			public abstract Child Child
			{
				get;
				set;
			}
			public abstract ICollection<PersonHasParentsAssociation> PersonHasParentsAsPerson
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> Person_idChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePerson_idChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.Person_id, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> Person_idChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePerson_idChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.Person_id), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person_id");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> FirstNameChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFirstNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.FirstName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> FirstNameChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFirstNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.FirstName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("FirstName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> LastNameChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseLastNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.LastName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> LastNameChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseLastNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.LastName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("LastName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> SocialSecurityNumberChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseSocialSecurityNumberChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.SocialSecurityNumber, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> SocialSecurityNumberChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseSocialSecurityNumberChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.SocialSecurityNumber), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("SocialSecurityNumber");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> ValueType1Changing
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseValueType1ChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.ValueType1, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> ValueType1Changed
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseValueType1ChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.ValueType1), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> NickNameChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseNickNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[6] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.NickName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> NickNameChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseNickNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.NickName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NickName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> ValueType1Changing
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseValueType1ChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.ValueType1, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> ValueType1Changed
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseValueType1ChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[7] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.ValueType1), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> GenderChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseGenderChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[8] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.Gender, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> GenderChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseGenderChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.Gender), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Gender");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<HatType>> HatTypeChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseHatTypeChangingEvent(HatType newValue)
			{
				EventHandler<PropertyChangingEventArgs<HatType>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<HatType>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<HatType> eventArgs = new PropertyChangingEventArgs<HatType>(this.HatType, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<HatType>> HatTypeChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseHatTypeChangedEvent(HatType oldValue)
			{
				EventHandler<PropertyChangedEventArgs<HatType>> eventHandler = this.Events[9] as EventHandler<PropertyChangedEventArgs<HatType>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<HatType>(oldValue, this.HatType), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatType");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> HusbandChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseHusbandChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[10] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Husband, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> HusbandChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseHusbandChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Husband), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Husband");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> WifeChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseWifeChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[11] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Wife, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> WifeChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseWifeChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[11] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Wife), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Wife");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Task>> TaskChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseTaskChangingEvent(Task newValue)
			{
				EventHandler<PropertyChangingEventArgs<Task>> eventHandler = this.Events[12] as EventHandler<PropertyChangingEventArgs<Task>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Task> eventArgs = new PropertyChangingEventArgs<Task>(this.Task, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Task>> TaskChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseTaskChangedEvent(Task oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Task>> eventHandler = this.Events[12] as EventHandler<PropertyChangedEventArgs<Task>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Task>(oldValue, this.Task), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Task");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Car>> DrivesCarChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDrivesCarChangingEvent(Car newValue)
			{
				EventHandler<PropertyChangingEventArgs<Car>> eventHandler = this.Events[13] as EventHandler<PropertyChangingEventArgs<Car>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Car> eventArgs = new PropertyChangingEventArgs<Car>(this.DrivesCar, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Car>> DrivesCarChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDrivesCarChangedEvent(Car oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Car>> eventHandler = this.Events[13] as EventHandler<PropertyChangedEventArgs<Car>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Car>(oldValue, this.DrivesCar), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DrivesCar");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Car>> OwnsCarChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseOwnsCarChangingEvent(Car newValue)
			{
				EventHandler<PropertyChangingEventArgs<Car>> eventHandler = this.Events[14] as EventHandler<PropertyChangingEventArgs<Car>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Car> eventArgs = new PropertyChangingEventArgs<Car>(this.OwnsCar, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Car>> OwnsCarChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseOwnsCarChangedEvent(Car oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Car>> eventHandler = this.Events[14] as EventHandler<PropertyChangedEventArgs<Car>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Car>(oldValue, this.OwnsCar), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("OwnsCar");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Date>> DateChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDateChangingEvent(Date newValue)
			{
				EventHandler<PropertyChangingEventArgs<Date>> eventHandler = this.Events[15] as EventHandler<PropertyChangingEventArgs<Date>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Date> eventArgs = new PropertyChangingEventArgs<Date>(this.Date, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Date>> DateChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDateChangedEvent(Date oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Date>> eventHandler = this.Events[15] as EventHandler<PropertyChangedEventArgs<Date>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Date>(oldValue, this.Date), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[16] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<Death>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[16] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Male>> MaleChanging
			{
				add
				{
					this.Events[17] = System.Delegate.Combine(this.Events[17], value);
				}
				remove
				{
					this.Events[17] = System.Delegate.Remove(this.Events[17], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseMaleChangingEvent(Male newValue)
			{
				EventHandler<PropertyChangingEventArgs<Male>> eventHandler = this.Events[17] as EventHandler<PropertyChangingEventArgs<Male>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Male> eventArgs = new PropertyChangingEventArgs<Male>(this.Male, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Male>> MaleChanged
			{
				add
				{
					this.Events[17] = System.Delegate.Combine(this.Events[17], value);
				}
				remove
				{
					this.Events[17] = System.Delegate.Remove(this.Events[17], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseMaleChangedEvent(Male oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Male>> eventHandler = this.Events[17] as EventHandler<PropertyChangedEventArgs<Male>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Male>(oldValue, this.Male), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Male");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Female>> FemaleChanging
			{
				add
				{
					this.Events[18] = System.Delegate.Combine(this.Events[18], value);
				}
				remove
				{
					this.Events[18] = System.Delegate.Remove(this.Events[18], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFemaleChangingEvent(Female newValue)
			{
				EventHandler<PropertyChangingEventArgs<Female>> eventHandler = this.Events[18] as EventHandler<PropertyChangingEventArgs<Female>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Female> eventArgs = new PropertyChangingEventArgs<Female>(this.Female, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Female>> FemaleChanged
			{
				add
				{
					this.Events[18] = System.Delegate.Combine(this.Events[18], value);
				}
				remove
				{
					this.Events[18] = System.Delegate.Remove(this.Events[18], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFemaleChangedEvent(Female oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Female>> eventHandler = this.Events[18] as EventHandler<PropertyChangedEventArgs<Female>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Female>(oldValue, this.Female), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Female");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Child>> ChildChanging
			{
				add
				{
					this.Events[19] = System.Delegate.Combine(this.Events[19], value);
				}
				remove
				{
					this.Events[19] = System.Delegate.Remove(this.Events[19], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseChildChangingEvent(Child newValue)
			{
				EventHandler<PropertyChangingEventArgs<Child>> eventHandler = this.Events[19] as EventHandler<PropertyChangingEventArgs<Child>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Child> eventArgs = new PropertyChangingEventArgs<Child>(this.Child, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Child>> ChildChanged
			{
				add
				{
					this.Events[19] = System.Delegate.Combine(this.Events[19], value);
				}
				remove
				{
					this.Events[19] = System.Delegate.Remove(this.Events[19], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseChildChangedEvent(Child oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Child>> eventHandler = this.Events[19] as EventHandler<PropertyChangedEventArgs<Child>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Child>(oldValue, this.Child), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Child");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Person{0}{{{0}{1}Person_id = ""{2}"",{0}{1}FirstName = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}SocialSecurityNumber = ""{5}"",{0}{1}ValueType1 = ""{6}"",{0}{1}NickName = ""{7}"",{0}{1}ValueType1 = ""{8}"",{0}{1}Gender = ""{9}"",{0}{1}HatType = {10},{0}{1}Husband = {11},{0}{1}Wife = {12},{0}{1}Task = {13},{0}{1}DrivesCar = {14},{0}{1}OwnsCar = {15},{0}{1}Date = {16},{0}{1}Death = {17},{0}{1}Male = {18},{0}{1}Female = {19},{0}{1}Child = {20}{0}}}", Environment.NewLine, "	", this.Person_id, this.FirstName, this.LastName, this.SocialSecurityNumber, this.ValueType1, this.NickName, this.ValueType1, this.Gender, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Person
		#region Date
		public abstract partial class Date : INotifyPropertyChanged
		{
			protected Date()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract System.DateTime ymd
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsSaleDate
			{
				get;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<System.DateTime>> ymdChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseymdChangingEvent(System.DateTime newValue)
			{
				EventHandler<PropertyChangingEventArgs<System.DateTime>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<System.DateTime>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<System.DateTime> eventArgs = new PropertyChangingEventArgs<System.DateTime>(this.ymd, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<System.DateTime>> ymdChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseymdChangedEvent(System.DateTime oldValue)
			{
				EventHandler<PropertyChangedEventArgs<System.DateTime>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<System.DateTime>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<System.DateTime>(oldValue, this.ymd), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ymd");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<Death>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Date{0}{{{0}{1}ymd = ""{2}"",{0}{1}Person = {3},{0}{1}Death = {4}{0}}}", Environment.NewLine, "	", this.ymd, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Date
		#region HatType
		public abstract partial class HatType : INotifyPropertyChanged
		{
			protected HatType()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract int ColorARGB
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract string HatTypeStyle
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> ColorARGBChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseColorARGBChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.ColorARGB, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> ColorARGBChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseColorARGBChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.ColorARGB), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ColorARGB");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> HatTypeStyleChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseHatTypeStyleChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.HatTypeStyle, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> HatTypeStyleChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseHatTypeStyleChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.HatTypeStyle), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatTypeStyle");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"HatType{0}{{{0}{1}ColorARGB = ""{2}"",{0}{1}Person = {3},{0}{1}HatTypeStyle = ""{4}""{0}}}", Environment.NewLine, "	", this.ColorARGB, "TODO: Recursively call ToString for customTypes...", this.HatTypeStyle);
			}
		}
		#endregion // HatType
		#region Task
		public abstract partial class Task : INotifyPropertyChanged
		{
			protected Task()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract int Task_id
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> Task_idChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseTask_idChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.Task_id, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> Task_idChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseTask_idChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.Task_id), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Task_id");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Task{0}{{{0}{1}Task_id = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.Task_id, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Task
		#region Car
		public abstract partial class Car : INotifyPropertyChanged
		{
			protected Car()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			[System.CLSCompliantAttribute(false)]
			public abstract uint vin
			{
				get;
				set;
			}
			public abstract Person DrivenByPerson
			{
				get;
				set;
			}
			public abstract Person OwnedByPerson
			{
				get;
				set;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsCarSold
			{
				get;
			}
			public abstract ICollection<ReviewAssociation> ReviewAsCar
			{
				get;
			}
			[System.CLSCompliantAttribute(false)]
			public event EventHandler<PropertyChangingEventArgs<uint>> vinChanging
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
			[System.CLSCompliantAttribute(false)]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisevinChangingEvent(uint newValue)
			{
				EventHandler<PropertyChangingEventArgs<uint>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<uint>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<uint> eventArgs = new PropertyChangingEventArgs<uint>(this.vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			[System.CLSCompliantAttribute(false)]
			public event EventHandler<PropertyChangedEventArgs<uint>> vinChanged
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
			[System.CLSCompliantAttribute(false)]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisevinChangedEvent(uint oldValue)
			{
				EventHandler<PropertyChangedEventArgs<uint>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<uint>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<uint>(oldValue, this.vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> DrivenByPersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDrivenByPersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.DrivenByPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> DrivenByPersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDrivenByPersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.DrivenByPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DrivenByPerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> OwnedByPersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseOwnedByPersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.OwnedByPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> OwnedByPersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseOwnedByPersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.OwnedByPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("OwnedByPerson");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Car{0}{{{0}{1}vin = ""{2}"",{0}{1}DrivenByPerson = {3},{0}{1}OwnedByPerson = {4}{0}}}", Environment.NewLine, "	", this.vin, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Car
		#region Death
		public abstract partial class Death : INotifyPropertyChanged
		{
			protected Death()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[6];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract string DeathCause
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract Date Date
			{
				get;
				set;
			}
			public abstract NaturalDeath NaturalDeath
			{
				get;
				set;
			}
			public abstract UnnaturalDeath UnnaturalDeath
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> DeathCauseChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathCauseChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.DeathCause, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> DeathCauseChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathCauseChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.DeathCause), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DeathCause");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Date>> DateChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDateChangingEvent(Date newValue)
			{
				EventHandler<PropertyChangingEventArgs<Date>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Date>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Date> eventArgs = new PropertyChangingEventArgs<Date>(this.Date, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Date>> DateChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDateChangedEvent(Date oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Date>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Date>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Date>(oldValue, this.Date), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<NaturalDeath>> NaturalDeathChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseNaturalDeathChangingEvent(NaturalDeath newValue)
			{
				EventHandler<PropertyChangingEventArgs<NaturalDeath>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<NaturalDeath>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<NaturalDeath> eventArgs = new PropertyChangingEventArgs<NaturalDeath>(this.NaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<NaturalDeath>> NaturalDeathChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseNaturalDeathChangedEvent(NaturalDeath oldValue)
			{
				EventHandler<PropertyChangedEventArgs<NaturalDeath>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<NaturalDeath>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<NaturalDeath>(oldValue, this.NaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath>> UnnaturalDeathChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseUnnaturalDeathChangingEvent(UnnaturalDeath newValue)
			{
				EventHandler<PropertyChangingEventArgs<UnnaturalDeath>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<UnnaturalDeath> eventArgs = new PropertyChangingEventArgs<UnnaturalDeath>(this.UnnaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath>> UnnaturalDeathChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseUnnaturalDeathChangedEvent(UnnaturalDeath oldValue)
			{
				EventHandler<PropertyChangedEventArgs<UnnaturalDeath>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<UnnaturalDeath>(oldValue, this.UnnaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeath");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Death{0}{{{0}{1}DeathCause = ""{2}"",{0}{1}Person = {3},{0}{1}Date = {4},{0}{1}NaturalDeath = {5},{0}{1}UnnaturalDeath = {6}{0}}}", Environment.NewLine, "	", this.DeathCause, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Death
		#region NaturalDeath
		public abstract partial class NaturalDeath : INotifyPropertyChanged
		{
			protected NaturalDeath()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[2];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract ICollection<NaturalDeathIsFromProstateCancerAssociation> NaturalDeathIsFromProstateCancerAsNaturalDeath
			{
				get;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<Death>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "NaturalDeath{0}{{{0}{1}Death = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // NaturalDeath
		#region UnnaturalDeath
		public abstract partial class UnnaturalDeath : INotifyPropertyChanged
		{
			protected UnnaturalDeath()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[2];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract ICollection<UnnaturalDeathIsViolentAssociation> UnnaturalDeathIsViolentAsUnnaturalDeath
			{
				get;
			}
			public abstract ICollection<UnnaturalDeathIsBloodyAssociation> UnnaturalDeathIsBloodyAsUnnaturalDeath
			{
				get;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<Death>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "UnnaturalDeath{0}{{{0}{1}Death = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // UnnaturalDeath
		#region Male
		public abstract partial class Male : INotifyPropertyChanged
		{
			protected Male()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ICollection<ChildAssociation> ChildAsMale
			{
				get;
			}
			public abstract Child Child
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Child>> ChildChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseChildChangingEvent(Child newValue)
			{
				EventHandler<PropertyChangingEventArgs<Child>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Child>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Child> eventArgs = new PropertyChangingEventArgs<Child>(this.Child, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Child>> ChildChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseChildChangedEvent(Child oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Child>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Child>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Child>(oldValue, this.Child), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Child");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "Male{0}{{{0}{1}Person = {2},{0}{1}Child = {3}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Male
		#region Female
		public abstract partial class Female : INotifyPropertyChanged
		{
			protected Female()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ICollection<ChildAssociation> ChildAsFemale
			{
				get;
			}
			public abstract Child Child
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Child>> ChildChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseChildChangingEvent(Child newValue)
			{
				EventHandler<PropertyChangingEventArgs<Child>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Child>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Child> eventArgs = new PropertyChangingEventArgs<Child>(this.Child, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Child>> ChildChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseChildChangedEvent(Child oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Child>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Child>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Child>(oldValue, this.Child), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Child");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "Female{0}{{{0}{1}Person = {2},{0}{1}Child = {3}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Female
		#region BirthOrder
		public abstract partial class BirthOrder : INotifyPropertyChanged
		{
			protected BirthOrder()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			[System.CLSCompliantAttribute(false)]
			public abstract uint BirthOrderNr
			{
				get;
				set;
			}
			public abstract ICollection<ChildAssociation> ChildAsBirthOrder
			{
				get;
			}
			public abstract Child Child
			{
				get;
				set;
			}
			[System.CLSCompliantAttribute(false)]
			public event EventHandler<PropertyChangingEventArgs<uint>> BirthOrderNrChanging
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
			[System.CLSCompliantAttribute(false)]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseBirthOrderNrChangingEvent(uint newValue)
			{
				EventHandler<PropertyChangingEventArgs<uint>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<uint>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<uint> eventArgs = new PropertyChangingEventArgs<uint>(this.BirthOrderNr, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			[System.CLSCompliantAttribute(false)]
			public event EventHandler<PropertyChangedEventArgs<uint>> BirthOrderNrChanged
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
			[System.CLSCompliantAttribute(false)]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseBirthOrderNrChangedEvent(uint oldValue)
			{
				EventHandler<PropertyChangedEventArgs<uint>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<uint>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<uint>(oldValue, this.BirthOrderNr), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("BirthOrderNr");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Child>> ChildChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseChildChangingEvent(Child newValue)
			{
				EventHandler<PropertyChangingEventArgs<Child>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Child>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Child> eventArgs = new PropertyChangingEventArgs<Child>(this.Child, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Child>> ChildChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseChildChangedEvent(Child oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Child>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Child>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Child>(oldValue, this.Child), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Child");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"BirthOrder{0}{{{0}{1}BirthOrderNr = ""{2}"",{0}{1}Child = {3}{0}}}", Environment.NewLine, "	", this.BirthOrderNr, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // BirthOrder
		#region Child
		public abstract partial class Child : INotifyPropertyChanged
		{
			protected Child()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[5];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract Male Male
			{
				get;
				set;
			}
			public abstract BirthOrder BirthOrder
			{
				get;
				set;
			}
			public abstract Female Female
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Male>> MaleChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseMaleChangingEvent(Male newValue)
			{
				EventHandler<PropertyChangingEventArgs<Male>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Male>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Male> eventArgs = new PropertyChangingEventArgs<Male>(this.Male, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Male>> MaleChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseMaleChangedEvent(Male oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Male>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Male>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Male>(oldValue, this.Male), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Male");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<BirthOrder>> BirthOrderChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseBirthOrderChangingEvent(BirthOrder newValue)
			{
				EventHandler<PropertyChangingEventArgs<BirthOrder>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<BirthOrder>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<BirthOrder> eventArgs = new PropertyChangingEventArgs<BirthOrder>(this.BirthOrder, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<BirthOrder>> BirthOrderChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseBirthOrderChangedEvent(BirthOrder oldValue)
			{
				EventHandler<PropertyChangedEventArgs<BirthOrder>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<BirthOrder>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<BirthOrder>(oldValue, this.BirthOrder), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("BirthOrder");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Female>> FemaleChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFemaleChangingEvent(Female newValue)
			{
				EventHandler<PropertyChangingEventArgs<Female>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Female>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Female> eventArgs = new PropertyChangingEventArgs<Female>(this.Female, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Female>> FemaleChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFemaleChangedEvent(Female oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Female>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Female>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Female>(oldValue, this.Female), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Female");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "Child{0}{{{0}{1}Male = {2},{0}{1}BirthOrder = {3},{0}{1}Female = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Child
		#region PersonBoughtCarFromPersonOnDateAssociation
		public abstract partial class PersonBoughtCarFromPersonOnDateAssociation : INotifyPropertyChanged
		{
			protected PersonBoughtCarFromPersonOnDateAssociation()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[5];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract Person Buyer
			{
				get;
				set;
			}
			public abstract Car CarSold
			{
				get;
				set;
			}
			public abstract Person Seller
			{
				get;
				set;
			}
			public abstract Date SaleDate
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> BuyerChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseBuyerChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Buyer, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> BuyerChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseBuyerChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Buyer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Buyer");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Car>> CarSoldChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseCarSoldChangingEvent(Car newValue)
			{
				EventHandler<PropertyChangingEventArgs<Car>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Car>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Car> eventArgs = new PropertyChangingEventArgs<Car>(this.CarSold, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Car>> CarSoldChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseCarSoldChangedEvent(Car oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Car>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Car>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Car>(oldValue, this.CarSold), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("CarSold");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> SellerChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseSellerChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.Seller, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> SellerChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseSellerChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Seller), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Seller");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Date>> SaleDateChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseSaleDateChangingEvent(Date newValue)
			{
				EventHandler<PropertyChangingEventArgs<Date>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Date>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Date> eventArgs = new PropertyChangingEventArgs<Date>(this.SaleDate, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Date>> SaleDateChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseSaleDateChangedEvent(Date oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Date>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Date>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Date>(oldValue, this.SaleDate), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("SaleDate");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "PersonBoughtCarFromPersonOnDateAssociation{0}{{{0}{1}Buyer = {2},{0}{1}CarSold = {3},{0}{1}Seller = {4},{0}{1}SaleDate = {5}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDateAssociation
		#region ChildAssociation
		public abstract partial class ChildAssociation : INotifyPropertyChanged
		{
			protected ChildAssociation()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract Male Male
			{
				get;
				set;
			}
			public abstract BirthOrder BirthOrder
			{
				get;
				set;
			}
			public abstract Female Female
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Male>> MaleChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseMaleChangingEvent(Male newValue)
			{
				EventHandler<PropertyChangingEventArgs<Male>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Male>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Male> eventArgs = new PropertyChangingEventArgs<Male>(this.Male, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Male>> MaleChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseMaleChangedEvent(Male oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Male>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Male>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Male>(oldValue, this.Male), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Male");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<BirthOrder>> BirthOrderChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseBirthOrderChangingEvent(BirthOrder newValue)
			{
				EventHandler<PropertyChangingEventArgs<BirthOrder>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<BirthOrder>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<BirthOrder> eventArgs = new PropertyChangingEventArgs<BirthOrder>(this.BirthOrder, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<BirthOrder>> BirthOrderChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseBirthOrderChangedEvent(BirthOrder oldValue)
			{
				EventHandler<PropertyChangedEventArgs<BirthOrder>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<BirthOrder>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<BirthOrder>(oldValue, this.BirthOrder), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("BirthOrder");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Female>> FemaleChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFemaleChangingEvent(Female newValue)
			{
				EventHandler<PropertyChangingEventArgs<Female>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Female>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Female> eventArgs = new PropertyChangingEventArgs<Female>(this.Female, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Female>> FemaleChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFemaleChangedEvent(Female oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Female>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Female>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Female>(oldValue, this.Female), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Female");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "ChildAssociation{0}{{{0}{1}Male = {2},{0}{1}BirthOrder = {3},{0}{1}Female = {4}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // ChildAssociation
		#region ReviewAssociation
		public abstract partial class ReviewAssociation : INotifyPropertyChanged
		{
			protected ReviewAssociation()
			{
			}
			private readonly System.Delegate[] Events = new System.Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new System.AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public abstract Car Car
			{
				get;
				set;
			}
			public abstract int RatingNr
			{
				get;
				set;
			}
			public abstract string CriteriaName
			{
				get;
				set;
			}
			public event EventHandler<PropertyChangingEventArgs<Car>> CarChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseCarChangingEvent(Car newValue)
			{
				EventHandler<PropertyChangingEventArgs<Car>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Car>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Car> eventArgs = new PropertyChangingEventArgs<Car>(this.Car, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Car>> CarChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseCarChangedEvent(Car oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Car>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Car>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Car>(oldValue, this.Car), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Car");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> RatingNrChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseRatingNrChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.RatingNr, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> RatingNrChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseRatingNrChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.RatingNr), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("RatingNr");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> CriteriaNameChanging
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseCriteriaNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.CriteriaName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> CriteriaNameChanged
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseCriteriaNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.CriteriaName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("CriteriaName");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"ReviewAssociation{0}{{{0}{1}Car = {2},{0}{1}RatingNr = ""{3}"",{0}{1}CriteriaName = ""{4}""{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...", this.RatingNr, this.CriteriaName);
			}
		}
		#endregion // ReviewAssociation
		#region ISampleModelContext
		public interface ISampleModelContext
		{
			IDeserializationSampleModelContext BeginDeserialization();
			bool IsDeserializing
			{
				get;
			}
			Person GetPersonByExternalUniquenessConstraint1(string FirstName, Date Date);
			Person GetPersonByExternalUniquenessConstraint2(string LastName, Date Date);
			HatType GetHatTypeByExternalUniquenessConstraint4(int ColorARGB, string HatTypeStyle);
			PersonBoughtCarFromPersonOnDateAssociation GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint23(Person Buyer, Car CarSold, Person Seller);
			PersonBoughtCarFromPersonOnDateAssociation GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint24(Date SaleDate, Person Seller, Car CarSold);
			PersonBoughtCarFromPersonOnDateAssociation GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint25(Car CarSold, Date SaleDate, Person Buyer);
			ChildAssociation GetChildAssociationByInternalUniquenessConstraint49(Male Male, BirthOrder BirthOrder, Female Female);
			ReviewAssociation GetReviewAssociationByInternalUniquenessConstraint26(Car Car, string CriteriaName);
			Person CreatePerson();
			ReadOnlyCollection<Person> PersonCollection
			{
				get;
			}
			Date CreateDate();
			ReadOnlyCollection<Date> DateCollection
			{
				get;
			}
			HatType CreateHatType();
			ReadOnlyCollection<HatType> HatTypeCollection
			{
				get;
			}
			Task CreateTask();
			ReadOnlyCollection<Task> TaskCollection
			{
				get;
			}
			Car CreateCar();
			ReadOnlyCollection<Car> CarCollection
			{
				get;
			}
			Death CreateDeath();
			ReadOnlyCollection<Death> DeathCollection
			{
				get;
			}
			NaturalDeath CreateNaturalDeath();
			ReadOnlyCollection<NaturalDeath> NaturalDeathCollection
			{
				get;
			}
			UnnaturalDeath CreateUnnaturalDeath();
			ReadOnlyCollection<UnnaturalDeath> UnnaturalDeathCollection
			{
				get;
			}
			Male CreateMale();
			ReadOnlyCollection<Male> MaleCollection
			{
				get;
			}
			Female CreateFemale();
			ReadOnlyCollection<Female> FemaleCollection
			{
				get;
			}
			BirthOrder CreateBirthOrder();
			ReadOnlyCollection<BirthOrder> BirthOrderCollection
			{
				get;
			}
			Child CreateChild();
			ReadOnlyCollection<Child> ChildCollection
			{
				get;
			}
			PersonBoughtCarFromPersonOnDateAssociation CreatePersonBoughtCarFromPersonOnDateAssociation(Person Buyer, Car CarSold, Person Seller, Date SaleDate);
			ReadOnlyCollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAssociationCollection
			{
				get;
			}
			ChildAssociation CreateChildAssociation(Male Male, BirthOrder BirthOrder, Female Female);
			ReadOnlyCollection<ChildAssociation> ChildAssociationCollection
			{
				get;
			}
			ReviewAssociation CreateReviewAssociation(Car Car, int RatingNr, string CriteriaName);
			ReadOnlyCollection<ReviewAssociation> ReviewAssociationCollection
			{
				get;
			}
		}
		#endregion // ISampleModelContext
		#region IDeserializationSampleModelContext
		public interface IDeserializationSampleModelContext : IDisposable
		{
			Person CreatePerson();
			Date CreateDate();
			HatType CreateHatType();
			Task CreateTask();
			Car CreateCar();
			Death CreateDeath();
			NaturalDeath CreateNaturalDeath();
			UnnaturalDeath CreateUnnaturalDeath();
			Male CreateMale();
			Female CreateFemale();
			BirthOrder CreateBirthOrder();
			Child CreateChild();
			PersonBoughtCarFromPersonOnDateAssociation CreatePersonBoughtCarFromPersonOnDateAssociation();
			ChildAssociation CreateChildAssociation();
			ReviewAssociation CreateReviewAssociation(int RatingNr, string CriteriaName);
		}
		#endregion // IDeserializationSampleModelContext
		#region SampleModelContext
		public sealed class SampleModelContext : ISampleModelContext
		{
			public SampleModelContext()
			{
			}
			IDeserializationSampleModelContext ISampleModelContext.BeginDeserialization()
			{
				return new DeserializationSampleModelContext(this);
			}
			private bool myIsDeserializing;
			bool ISampleModelContext.IsDeserializing
			{
				get
				{
					return this.myIsDeserializing;
				}
			}
			private Dictionary<Tuple<string, Date>, Person> myExternalUniquenessConstraint1Dictionary = new Dictionary<Tuple<string, Date>, Person>();
			private bool OnExternalUniquenessConstraint1Changing(Person instance, Tuple<string, Date> newValue)
			{
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this.myExternalUniquenessConstraint1Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint1Changed(Person instance, Tuple<string, Date> oldValue, Tuple<string, Date> newValue)
			{
				if (oldValue != null)
				{
					this.myExternalUniquenessConstraint1Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myExternalUniquenessConstraint1Dictionary.Add(newValue, instance);
				}
			}
			Person ISampleModelContext.GetPersonByExternalUniquenessConstraint1(string FirstName, Date Date)
			{
				return this.myExternalUniquenessConstraint1Dictionary[Tuple.CreateTuple(FirstName, Date)];
			}
			private Dictionary<Tuple<string, Date>, Person> myExternalUniquenessConstraint2Dictionary = new Dictionary<Tuple<string, Date>, Person>();
			private bool OnExternalUniquenessConstraint2Changing(Person instance, Tuple<string, Date> newValue)
			{
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this.myExternalUniquenessConstraint2Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint2Changed(Person instance, Tuple<string, Date> oldValue, Tuple<string, Date> newValue)
			{
				if (oldValue != null)
				{
					this.myExternalUniquenessConstraint2Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myExternalUniquenessConstraint2Dictionary.Add(newValue, instance);
				}
			}
			Person ISampleModelContext.GetPersonByExternalUniquenessConstraint2(string LastName, Date Date)
			{
				return this.myExternalUniquenessConstraint2Dictionary[Tuple.CreateTuple(LastName, Date)];
			}
			private Dictionary<Tuple<int, string>, HatType> myExternalUniquenessConstraint4Dictionary = new Dictionary<Tuple<int, string>, HatType>();
			private bool OnExternalUniquenessConstraint4Changing(HatType instance, Tuple<int, string> newValue)
			{
				if (newValue != null)
				{
					HatType currentInstance = instance;
					if (this.myExternalUniquenessConstraint4Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint4Changed(HatType instance, Tuple<int, string> oldValue, Tuple<int, string> newValue)
			{
				if (oldValue != null)
				{
					this.myExternalUniquenessConstraint4Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myExternalUniquenessConstraint4Dictionary.Add(newValue, instance);
				}
			}
			HatType ISampleModelContext.GetHatTypeByExternalUniquenessConstraint4(int ColorARGB, string HatTypeStyle)
			{
				return this.myExternalUniquenessConstraint4Dictionary[Tuple.CreateTuple(ColorARGB, HatTypeStyle)];
			}
			private Dictionary<Tuple<Person, Car, Person>, PersonBoughtCarFromPersonOnDateAssociation> myInternalUniquenessConstraint23Dictionary = new Dictionary<Tuple<Person, Car, Person>, PersonBoughtCarFromPersonOnDateAssociation>();
			private bool OnInternalUniquenessConstraint23Changing(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Person, Car, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDateAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint23Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint23Changed(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Person, Car, Person> oldValue, Tuple<Person, Car, Person> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint23Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint23Dictionary.Add(newValue, instance);
				}
			}
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint23(Person Buyer, Car CarSold, Person Seller)
			{
				return this.myInternalUniquenessConstraint23Dictionary[Tuple.CreateTuple(Buyer, CarSold, Seller)];
			}
			private Dictionary<Tuple<Date, Person, Car>, PersonBoughtCarFromPersonOnDateAssociation> myInternalUniquenessConstraint24Dictionary = new Dictionary<Tuple<Date, Person, Car>, PersonBoughtCarFromPersonOnDateAssociation>();
			private bool OnInternalUniquenessConstraint24Changing(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Date, Person, Car> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDateAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint24Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint24Changed(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Date, Person, Car> oldValue, Tuple<Date, Person, Car> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint24Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint24Dictionary.Add(newValue, instance);
				}
			}
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint24(Date SaleDate, Person Seller, Car CarSold)
			{
				return this.myInternalUniquenessConstraint24Dictionary[Tuple.CreateTuple(SaleDate, Seller, CarSold)];
			}
			private Dictionary<Tuple<Car, Date, Person>, PersonBoughtCarFromPersonOnDateAssociation> myInternalUniquenessConstraint25Dictionary = new Dictionary<Tuple<Car, Date, Person>, PersonBoughtCarFromPersonOnDateAssociation>();
			private bool OnInternalUniquenessConstraint25Changing(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Car, Date, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDateAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint25Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint25Changed(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Car, Date, Person> oldValue, Tuple<Car, Date, Person> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint25Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint25Dictionary.Add(newValue, instance);
				}
			}
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint25(Car CarSold, Date SaleDate, Person Buyer)
			{
				return this.myInternalUniquenessConstraint25Dictionary[Tuple.CreateTuple(CarSold, SaleDate, Buyer)];
			}
			private Dictionary<Tuple<Male, BirthOrder, Female>, ChildAssociation> myInternalUniquenessConstraint49Dictionary = new Dictionary<Tuple<Male, BirthOrder, Female>, ChildAssociation>();
			private bool OnInternalUniquenessConstraint49Changing(ChildAssociation instance, Tuple<Male, BirthOrder, Female> newValue)
			{
				if (newValue != null)
				{
					ChildAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint49Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint49Changed(ChildAssociation instance, Tuple<Male, BirthOrder, Female> oldValue, Tuple<Male, BirthOrder, Female> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint49Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint49Dictionary.Add(newValue, instance);
				}
			}
			ChildAssociation ISampleModelContext.GetChildAssociationByInternalUniquenessConstraint49(Male Male, BirthOrder BirthOrder, Female Female)
			{
				return this.myInternalUniquenessConstraint49Dictionary[Tuple.CreateTuple(Male, BirthOrder, Female)];
			}
			private Dictionary<Tuple<Car, string>, ReviewAssociation> myInternalUniquenessConstraint26Dictionary = new Dictionary<Tuple<Car, string>, ReviewAssociation>();
			private bool OnInternalUniquenessConstraint26Changing(ReviewAssociation instance, Tuple<Car, string> newValue)
			{
				if (newValue != null)
				{
					ReviewAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint26Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint26Changed(ReviewAssociation instance, Tuple<Car, string> oldValue, Tuple<Car, string> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint26Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint26Dictionary.Add(newValue, instance);
				}
			}
			ReviewAssociation ISampleModelContext.GetReviewAssociationByInternalUniquenessConstraint26(Car Car, string CriteriaName)
			{
				return this.myInternalUniquenessConstraint26Dictionary[Tuple.CreateTuple(Car, CriteriaName)];
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			private static bool RoleValueConstraintForFactType17Role2(string value, bool throwOnFailure)
			{
				if (!((value == "natural") || (value == "not so natural")))
				{
					if (throwOnFailure)
					{
						throw new System.ArgumentOutOfRangeException();
					}
					return false;
				}
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			private static bool ValueConstraintForInteger(int value, bool throwOnFailure)
			{
				if (!((1 <= value) && (value <= 7)))
				{
					if (throwOnFailure)
					{
						throw new System.ArgumentOutOfRangeException();
					}
					return false;
				}
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			private static bool ValueConstraintForGender_Code(string value, bool throwOnFailure)
			{
				if (!((value == "M") || (value == "F")))
				{
					if (throwOnFailure)
					{
						throw new System.ArgumentOutOfRangeException();
					}
					return false;
				}
				return true;
			}
			private bool OnPersonPerson_idChanging(Person instance, int newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonPerson_idChanged(Person instance, int oldValue)
			{
			}
			private bool OnPersonFirstNameChanging(Person instance, string newValue, bool throwOnFailure)
			{
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(newValue, instance.Date))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonFirstNameChanged(Person instance, string oldValue)
			{
				this.OnExternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(oldValue, instance.Date), Tuple.CreateTuple(instance.FirstName, instance.Date));
			}
			private bool OnPersonLastNameChanging(Person instance, string newValue, bool throwOnFailure)
			{
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(newValue, instance.Date))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonLastNameChanged(Person instance, string oldValue)
			{
				this.OnExternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(oldValue, instance.Date), Tuple.CreateTuple(instance.LastName, instance.Date));
			}
			private bool OnPersonSocialSecurityNumberChanging(Person instance, string newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonSocialSecurityNumberChanged(Person instance, string oldValue)
			{
			}
			private bool OnPersonValueType1Changing(Person instance, int newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonValueType1Changed(Person instance, int oldValue)
			{
			}
			private bool OnPersonNickNameChanging(Person instance, string newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonNickNameChanged(Person instance, string oldValue)
			{
			}
			private bool OnPersonValueType1Changing(Person instance, int newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonValueType1Changed(Person instance, int oldValue)
			{
			}
			private bool OnPersonGenderChanging(Person instance, string newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonGenderChanged(Person instance, string oldValue)
			{
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonHatTypeChanging(Person instance, HatType newValue, bool throwOnFailure)
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
			private void OnPersonHatTypeChanged(Person instance, HatType oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.HatType != null)
				{
					instance.HatType.Person.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonHusbandChanging(Person instance, Person newValue, bool throwOnFailure)
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
			private void OnPersonHusbandChanged(Person instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Wife.Remove(instance);
				}
				if (instance.Husband != null)
				{
					instance.Husband.Wife.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonWifeChanging(Person instance, Person newValue, bool throwOnFailure)
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
			private void OnPersonWifeChanged(Person instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Husband.Remove(instance);
				}
				if (instance.Wife != null)
				{
					instance.Wife.Husband.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonTaskChanging(Person instance, Task newValue, bool throwOnFailure)
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
			private void OnPersonTaskChanged(Person instance, Task oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.Task != null)
				{
					instance.Task.Person.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonDrivesCarChanging(Person instance, Car newValue, bool throwOnFailure)
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
			private void OnPersonDrivesCarChanged(Person instance, Car oldValue)
			{
				if (oldValue != null)
				{
					oldValue.DrivenByPerson.Remove(instance);
				}
				if (instance.DrivesCar != null)
				{
					instance.DrivesCar.DrivenByPerson.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonOwnsCarChanging(Person instance, Car newValue, bool throwOnFailure)
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
			private void OnPersonOwnsCarChanged(Person instance, Car oldValue)
			{
				if (oldValue != null)
				{
					oldValue.OwnedByPerson.Remove(instance);
				}
				if (instance.OwnsCar != null)
				{
					instance.OwnsCar.OwnedByPerson.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonDateChanging(Person instance, Date newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(instance.FirstName, newValue))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(instance.LastName, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonDateChanged(Person instance, Date oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.Date != null)
				{
					instance.Date.Person.Add(instance);
				}
				this.OnExternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(instance.FirstName, oldValue), Tuple.CreateTuple(instance.FirstName, instance.Date));
				this.OnExternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(instance.LastName, oldValue), Tuple.CreateTuple(instance.LastName, instance.Date));
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.Buyer = instance;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoving(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.Buyer = null;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.Seller = instance;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoving(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved(Person instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.Seller = null;
				}
			}
			private bool OnPersonDeathAsPersonAdding(Person instance, DeathAssociation value)
			{
				return true;
			}
			private void OnPersonDeathAsPersonAdded(Person instance, DeathAssociation value)
			{
				if (value != null)
				{
					value.Person.Add(instance);
				}
			}
			private bool OnPersonDeathAsPersonRemoving(Person instance, DeathAssociation value)
			{
				return true;
			}
			private void OnPersonDeathAsPersonRemoved(Person instance, DeathAssociation value)
			{
				if (value != null)
				{
					value.Person.Remove(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonDeathChanging(Person instance, Death newValue, bool throwOnFailure)
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
			private void OnPersonDeathChanged(Person instance, Death oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.Death != null)
				{
					instance.Death.Person.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonMaleChanging(Person instance, Male newValue, bool throwOnFailure)
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
			private void OnPersonMaleChanged(Person instance, Male oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.Male != null)
				{
					instance.Male.Person.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonFemaleChanging(Person instance, Female newValue, bool throwOnFailure)
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
			private void OnPersonFemaleChanged(Person instance, Female oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.Female != null)
				{
					instance.Female.Person.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonChildChanging(Person instance, Child newValue, bool throwOnFailure)
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
			private void OnPersonChildChanged(Person instance, Child oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.Child != null)
				{
					instance.Child.Person.Add(instance);
				}
			}
			private bool OnPersonPersonHasParentsAsPersonAdding(Person instance, PersonHasParentsAssociation value)
			{
				return true;
			}
			private void OnPersonPersonHasParentsAsPersonAdded(Person instance, PersonHasParentsAssociation value)
			{
				if (value != null)
				{
					value.Person.Add(instance);
				}
			}
			private bool OnPersonPersonHasParentsAsPersonRemoving(Person instance, PersonHasParentsAssociation value)
			{
				return true;
			}
			private void OnPersonPersonHasParentsAsPersonRemoved(Person instance, PersonHasParentsAssociation value)
			{
				if (value != null)
				{
					value.Person.Remove(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Person ISampleModelContext.CreatePerson()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new PersonCore(this);
			}
			#region Person
			private readonly List<Person> myPersonCollection = new List<Person>();
			ReadOnlyCollection<Person> ISampleModelContext.PersonCollection
			{
				get
				{
					return this.myPersonCollection.AsReadOnly();
				}
			}
			private sealed class PersonCore : Person
			{
				public PersonCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myPersonCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myPerson_id;
				public override int Person_id
				{
					get
					{
						return this.myPerson_id;
					}
					set
					{
						if (!(object.Equals(this.Person_id, value)))
						{
							if (this.Context.OnPersonPerson_idChanging(this, value, true))
							{
								if (base.RaisePerson_idChangingEvent(value))
								{
									int oldValue = this.Person_id;
									this.myPerson_id = value;
									this.Context.OnPersonPerson_idChanged(this, oldValue);
									base.RaisePerson_idChangedEvent(oldValue);
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
						if (!(object.Equals(this.FirstName, value)))
						{
							if (this.Context.OnPersonFirstNameChanging(this, value, true))
							{
								if (base.RaiseFirstNameChangingEvent(value))
								{
									string oldValue = this.FirstName;
									this.myFirstName = value;
									this.Context.OnPersonFirstNameChanged(this, oldValue);
									base.RaiseFirstNameChangedEvent(oldValue);
								}
							}
						}
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
						if (!(object.Equals(this.LastName, value)))
						{
							if (this.Context.OnPersonLastNameChanging(this, value, true))
							{
								if (base.RaiseLastNameChangingEvent(value))
								{
									string oldValue = this.LastName;
									this.myLastName = value;
									this.Context.OnPersonLastNameChanged(this, oldValue);
									base.RaiseLastNameChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string mySocialSecurityNumber;
				public override string SocialSecurityNumber
				{
					get
					{
						return this.mySocialSecurityNumber;
					}
					set
					{
						if (!(object.Equals(this.SocialSecurityNumber, value)))
						{
							if (this.Context.OnPersonSocialSecurityNumberChanging(this, value, true))
							{
								if (base.RaiseSocialSecurityNumberChangingEvent(value))
								{
									string oldValue = this.SocialSecurityNumber;
									this.mySocialSecurityNumber = value;
									this.Context.OnPersonSocialSecurityNumberChanged(this, oldValue);
									base.RaiseSocialSecurityNumberChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private int myValueType1;
				public override int ValueType1
				{
					get
					{
						return this.myValueType1;
					}
					set
					{
						if (!(object.Equals(this.ValueType1, value)))
						{
							if (this.Context.OnPersonValueType1Changing(this, value, true))
							{
								if (base.RaiseValueType1ChangingEvent(value))
								{
									int oldValue = this.ValueType1;
									this.myValueType1 = value;
									this.Context.OnPersonValueType1Changed(this, oldValue);
									base.RaiseValueType1ChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myNickName;
				public override string NickName
				{
					get
					{
						return this.myNickName;
					}
					set
					{
						if (!(object.Equals(this.NickName, value)))
						{
							if (this.Context.OnPersonNickNameChanging(this, value, true))
							{
								if (base.RaiseNickNameChangingEvent(value))
								{
									string oldValue = this.NickName;
									this.myNickName = value;
									this.Context.OnPersonNickNameChanged(this, oldValue);
									base.RaiseNickNameChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private int myValueType1;
				public override int ValueType1
				{
					get
					{
						return this.myValueType1;
					}
					set
					{
						if (!(object.Equals(this.ValueType1, value)))
						{
							if (this.Context.OnPersonValueType1Changing(this, value, true))
							{
								if (base.RaiseValueType1ChangingEvent(value))
								{
									int oldValue = this.ValueType1;
									this.myValueType1 = value;
									this.Context.OnPersonValueType1Changed(this, oldValue);
									base.RaiseValueType1ChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myGender;
				public override string Gender
				{
					get
					{
						return this.myGender;
					}
					set
					{
						if (!(object.Equals(this.Gender, value)))
						{
							if (this.Context.OnPersonGenderChanging(this, value, true))
							{
								if (base.RaiseGenderChangingEvent(value))
								{
									string oldValue = this.Gender;
									this.myGender = value;
									this.Context.OnPersonGenderChanged(this, oldValue);
									base.RaiseGenderChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private HatType myHatType;
				public override HatType HatType
				{
					get
					{
						return this.myHatType;
					}
					set
					{
						if (!(object.Equals(this.HatType, value)))
						{
							if (this.Context.OnPersonHatTypeChanging(this, value, true))
							{
								if (base.RaiseHatTypeChangingEvent(value))
								{
									HatType oldValue = this.HatType;
									this.myHatType = value;
									this.Context.OnPersonHatTypeChanged(this, oldValue);
									base.RaiseHatTypeChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myHusband;
				public override Person Husband
				{
					get
					{
						return this.myHusband;
					}
					set
					{
						if (!(object.Equals(this.Husband, value)))
						{
							if (this.Context.OnPersonHusbandChanging(this, value, true))
							{
								if (base.RaiseHusbandChangingEvent(value))
								{
									Person oldValue = this.Husband;
									this.myHusband = value;
									this.Context.OnPersonHusbandChanged(this, oldValue);
									base.RaiseHusbandChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myWife;
				public override Person Wife
				{
					get
					{
						return this.myWife;
					}
					set
					{
						if (!(object.Equals(this.Wife, value)))
						{
							if (this.Context.OnPersonWifeChanging(this, value, true))
							{
								if (base.RaiseWifeChangingEvent(value))
								{
									Person oldValue = this.Wife;
									this.myWife = value;
									this.Context.OnPersonWifeChanged(this, oldValue);
									base.RaiseWifeChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Task myTask;
				public override Task Task
				{
					get
					{
						return this.myTask;
					}
					set
					{
						if (!(object.Equals(this.Task, value)))
						{
							if (this.Context.OnPersonTaskChanging(this, value, true))
							{
								if (base.RaiseTaskChangingEvent(value))
								{
									Task oldValue = this.Task;
									this.myTask = value;
									this.Context.OnPersonTaskChanged(this, oldValue);
									base.RaiseTaskChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Car myDrivesCar;
				public override Car DrivesCar
				{
					get
					{
						return this.myDrivesCar;
					}
					set
					{
						if (!(object.Equals(this.DrivesCar, value)))
						{
							if (this.Context.OnPersonDrivesCarChanging(this, value, true))
							{
								if (base.RaiseDrivesCarChangingEvent(value))
								{
									Car oldValue = this.DrivesCar;
									this.myDrivesCar = value;
									this.Context.OnPersonDrivesCarChanged(this, oldValue);
									base.RaiseDrivesCarChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Car myOwnsCar;
				public override Car OwnsCar
				{
					get
					{
						return this.myOwnsCar;
					}
					set
					{
						if (!(object.Equals(this.OwnsCar, value)))
						{
							if (this.Context.OnPersonOwnsCarChanging(this, value, true))
							{
								if (base.RaiseOwnsCarChangingEvent(value))
								{
									Car oldValue = this.OwnsCar;
									this.myOwnsCar = value;
									this.Context.OnPersonOwnsCarChanged(this, oldValue);
									base.RaiseOwnsCarChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Date myDate;
				public override Date Date
				{
					get
					{
						return this.myDate;
					}
					set
					{
						if (!(object.Equals(this.Date, value)))
						{
							if (this.Context.OnPersonDateChanging(this, value, true))
							{
								if (base.RaiseDateChangingEvent(value))
								{
									Date oldValue = this.Date;
									this.myDate = value;
									this.Context.OnPersonDateChanged(this, oldValue);
									base.RaiseDateChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class PersonBoughtCarFromPersonOnDateAsBuyerCollection : ICollection<PersonBoughtCarFromPersonOnDateAssociation>
				{
					private Person myPerson;
					private List<PersonBoughtCarFromPersonOnDateAssociation> myList = new List<PersonBoughtCarFromPersonOnDateAssociation>();
					public PersonBoughtCarFromPersonOnDateAsBuyerCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<PersonBoughtCarFromPersonOnDateAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded(this.myPerson, item);
						}
					}
					public bool Remove(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved(this.myPerson, item);
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
					public bool Contains(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(PersonBoughtCarFromPersonOnDateAssociation[] array, int arrayIndex)
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
				private ICollection<PersonBoughtCarFromPersonOnDateAssociation> myPersonBoughtCarFromPersonOnDateAsBuyer;
				public override ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsBuyer
				{
					get
					{
						if (this.myPersonBoughtCarFromPersonOnDateAsBuyer == null)
						{
							this.myPersonBoughtCarFromPersonOnDateAsBuyer = new PersonBoughtCarFromPersonOnDateAsBuyerCollection(this);
						}
						return this.myPersonBoughtCarFromPersonOnDateAsBuyer;
					}
				}
				private sealed class PersonBoughtCarFromPersonOnDateAsSellerCollection : ICollection<PersonBoughtCarFromPersonOnDateAssociation>
				{
					private Person myPerson;
					private List<PersonBoughtCarFromPersonOnDateAssociation> myList = new List<PersonBoughtCarFromPersonOnDateAssociation>();
					public PersonBoughtCarFromPersonOnDateAsSellerCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<PersonBoughtCarFromPersonOnDateAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded(this.myPerson, item);
						}
					}
					public bool Remove(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved(this.myPerson, item);
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
					public bool Contains(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(PersonBoughtCarFromPersonOnDateAssociation[] array, int arrayIndex)
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
				private ICollection<PersonBoughtCarFromPersonOnDateAssociation> myPersonBoughtCarFromPersonOnDateAsSeller;
				public override ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsSeller
				{
					get
					{
						if (this.myPersonBoughtCarFromPersonOnDateAsSeller == null)
						{
							this.myPersonBoughtCarFromPersonOnDateAsSeller = new PersonBoughtCarFromPersonOnDateAsSellerCollection(this);
						}
						return this.myPersonBoughtCarFromPersonOnDateAsSeller;
					}
				}
				private sealed class DeathAsPersonCollection : ICollection<DeathAssociation>
				{
					private Person myPerson;
					private List<DeathAssociation> myList = new List<DeathAssociation>();
					public DeathAsPersonCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<DeathAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(DeathAssociation item)
					{
						if (this.myPerson.Context.OnPersonDeathAsPersonAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonDeathAsPersonAdded(this.myPerson, item);
						}
					}
					public bool Remove(DeathAssociation item)
					{
						if (this.myPerson.Context.OnPersonDeathAsPersonRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonDeathAsPersonRemoved(this.myPerson, item);
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
					public bool Contains(DeathAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(DeathAssociation[] array, int arrayIndex)
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
				private ICollection<DeathAssociation> myDeathAsPerson;
				public override ICollection<DeathAssociation> DeathAsPerson
				{
					get
					{
						if (this.myDeathAsPerson == null)
						{
							this.myDeathAsPerson = new DeathAsPersonCollection(this);
						}
						return this.myDeathAsPerson;
					}
				}
				private Death myDeath;
				public override Death Death
				{
					get
					{
						return this.myDeath;
					}
					set
					{
						if (!(object.Equals(this.Death, value)))
						{
							if (this.Context.OnPersonDeathChanging(this, value, true))
							{
								if (base.RaiseDeathChangingEvent(value))
								{
									Death oldValue = this.Death;
									this.myDeath = value;
									this.Context.OnPersonDeathChanged(this, oldValue);
									base.RaiseDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Male myMale;
				public override Male Male
				{
					get
					{
						return this.myMale;
					}
					set
					{
						if (!(object.Equals(this.Male, value)))
						{
							if (this.Context.OnPersonMaleChanging(this, value, true))
							{
								if (base.RaiseMaleChangingEvent(value))
								{
									Male oldValue = this.Male;
									this.myMale = value;
									this.Context.OnPersonMaleChanged(this, oldValue);
									base.RaiseMaleChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Female myFemale;
				public override Female Female
				{
					get
					{
						return this.myFemale;
					}
					set
					{
						if (!(object.Equals(this.Female, value)))
						{
							if (this.Context.OnPersonFemaleChanging(this, value, true))
							{
								if (base.RaiseFemaleChangingEvent(value))
								{
									Female oldValue = this.Female;
									this.myFemale = value;
									this.Context.OnPersonFemaleChanged(this, oldValue);
									base.RaiseFemaleChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Child myChild;
				public override Child Child
				{
					get
					{
						return this.myChild;
					}
					set
					{
						if (!(object.Equals(this.Child, value)))
						{
							if (this.Context.OnPersonChildChanging(this, value, true))
							{
								if (base.RaiseChildChangingEvent(value))
								{
									Child oldValue = this.Child;
									this.myChild = value;
									this.Context.OnPersonChildChanged(this, oldValue);
									base.RaiseChildChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class PersonHasParentsAsPersonCollection : ICollection<PersonHasParentsAssociation>
				{
					private Person myPerson;
					private List<PersonHasParentsAssociation> myList = new List<PersonHasParentsAssociation>();
					public PersonHasParentsAsPersonCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<PersonHasParentsAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(PersonHasParentsAssociation item)
					{
						if (this.myPerson.Context.OnPersonPersonHasParentsAsPersonAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonPersonHasParentsAsPersonAdded(this.myPerson, item);
						}
					}
					public bool Remove(PersonHasParentsAssociation item)
					{
						if (this.myPerson.Context.OnPersonPersonHasParentsAsPersonRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonPersonHasParentsAsPersonRemoved(this.myPerson, item);
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
					public bool Contains(PersonHasParentsAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(PersonHasParentsAssociation[] array, int arrayIndex)
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
				private ICollection<PersonHasParentsAssociation> myPersonHasParentsAsPerson;
				public override ICollection<PersonHasParentsAssociation> PersonHasParentsAsPerson
				{
					get
					{
						if (this.myPersonHasParentsAsPerson == null)
						{
							this.myPersonHasParentsAsPerson = new PersonHasParentsAsPersonCollection(this);
						}
						return this.myPersonHasParentsAsPerson;
					}
				}
			}
			#endregion // Person
			private bool OnDateymdChanging(Date instance, System.DateTime newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnDateymdChanged(Date instance, System.DateTime oldValue)
			{
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDatePersonChanging(Date instance, Person newValue, bool throwOnFailure)
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
			private void OnDatePersonChanged(Date instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Date.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.Date.Add(instance);
				}
			}
			private bool OnDatePersonBoughtCarFromPersonOnDateAsSaleDateAdding(Date instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnDatePersonBoughtCarFromPersonOnDateAsSaleDateAdded(Date instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.SaleDate = instance;
				}
			}
			private bool OnDatePersonBoughtCarFromPersonOnDateAsSaleDateRemoving(Date instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnDatePersonBoughtCarFromPersonOnDateAsSaleDateRemoved(Date instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.SaleDate = null;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDateDeathChanging(Date instance, Death newValue, bool throwOnFailure)
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
			private void OnDateDeathChanged(Date instance, Death oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Date.Remove(instance);
				}
				if (instance.Death != null)
				{
					instance.Death.Date.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Date ISampleModelContext.CreateDate()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new DateCore(this);
			}
			#region Date
			private readonly List<Date> myDateCollection = new List<Date>();
			ReadOnlyCollection<Date> ISampleModelContext.DateCollection
			{
				get
				{
					return this.myDateCollection.AsReadOnly();
				}
			}
			private sealed class DateCore : Date
			{
				public DateCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myDateCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private System.DateTime myymd;
				public override System.DateTime ymd
				{
					get
					{
						return this.myymd;
					}
					set
					{
						if (!(object.Equals(this.ymd, value)))
						{
							if (this.Context.OnDateymdChanging(this, value, true))
							{
								if (base.RaiseymdChangingEvent(value))
								{
									System.DateTime oldValue = this.ymd;
									this.myymd = value;
									this.Context.OnDateymdChanged(this, oldValue);
									base.RaiseymdChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnDatePersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnDatePersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class PersonBoughtCarFromPersonOnDateAsSaleDateCollection : ICollection<PersonBoughtCarFromPersonOnDateAssociation>
				{
					private Date myDate;
					private List<PersonBoughtCarFromPersonOnDateAssociation> myList = new List<PersonBoughtCarFromPersonOnDateAssociation>();
					public PersonBoughtCarFromPersonOnDateAsSaleDateCollection(Date instance)
					{
						this.myDate = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<PersonBoughtCarFromPersonOnDateAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myDate.Context.OnDatePersonBoughtCarFromPersonOnDateAsSaleDateAdding(this.myDate, item))
						{
							this.myList.Add(item);
							this.myDate.Context.OnDatePersonBoughtCarFromPersonOnDateAsSaleDateAdded(this.myDate, item);
						}
					}
					public bool Remove(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myDate.Context.OnDatePersonBoughtCarFromPersonOnDateAsSaleDateRemoving(this.myDate, item))
						{
							if (this.myList.Remove(item))
							{
								this.myDate.Context.OnDatePersonBoughtCarFromPersonOnDateAsSaleDateRemoved(this.myDate, item);
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
					public bool Contains(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(PersonBoughtCarFromPersonOnDateAssociation[] array, int arrayIndex)
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
				private ICollection<PersonBoughtCarFromPersonOnDateAssociation> myPersonBoughtCarFromPersonOnDateAsSaleDate;
				public override ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsSaleDate
				{
					get
					{
						if (this.myPersonBoughtCarFromPersonOnDateAsSaleDate == null)
						{
							this.myPersonBoughtCarFromPersonOnDateAsSaleDate = new PersonBoughtCarFromPersonOnDateAsSaleDateCollection(this);
						}
						return this.myPersonBoughtCarFromPersonOnDateAsSaleDate;
					}
				}
				private Death myDeath;
				public override Death Death
				{
					get
					{
						return this.myDeath;
					}
					set
					{
						if (!(object.Equals(this.Death, value)))
						{
							if (this.Context.OnDateDeathChanging(this, value, true))
							{
								if (base.RaiseDeathChangingEvent(value))
								{
									Death oldValue = this.Death;
									this.myDeath = value;
									this.Context.OnDateDeathChanged(this, oldValue);
									base.RaiseDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // Date
			private bool OnHatTypeColorARGBChanging(HatType instance, int newValue, bool throwOnFailure)
			{
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnExternalUniquenessConstraint4Changing(instance, Tuple.CreateTuple(newValue, instance.HatTypeStyle))))
				{
					return false;
				}
				return true;
			}
			private void OnHatTypeColorARGBChanged(HatType instance, int oldValue)
			{
				this.OnExternalUniquenessConstraint4Changed(instance, Tuple.CreateTuple(oldValue, instance.HatTypeStyle), Tuple.CreateTuple(instance.ColorARGB, instance.HatTypeStyle));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnHatTypePersonChanging(HatType instance, Person newValue, bool throwOnFailure)
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
			private void OnHatTypePersonChanged(HatType instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.HatType.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.HatType.Add(instance);
				}
			}
			private bool OnHatTypeHatTypeStyleChanging(HatType instance, string newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnHatTypeHatTypeStyleChanged(HatType instance, string oldValue)
			{
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			HatType ISampleModelContext.CreateHatType()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new HatTypeCore(this);
			}
			#region HatType
			private readonly List<HatType> myHatTypeCollection = new List<HatType>();
			ReadOnlyCollection<HatType> ISampleModelContext.HatTypeCollection
			{
				get
				{
					return this.myHatTypeCollection.AsReadOnly();
				}
			}
			private sealed class HatTypeCore : HatType
			{
				public HatTypeCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myHatTypeCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myColorARGB;
				public override int ColorARGB
				{
					get
					{
						return this.myColorARGB;
					}
					set
					{
						if (!(object.Equals(this.ColorARGB, value)))
						{
							if (this.Context.OnHatTypeColorARGBChanging(this, value, true))
							{
								if (base.RaiseColorARGBChangingEvent(value))
								{
									int oldValue = this.ColorARGB;
									this.myColorARGB = value;
									this.Context.OnHatTypeColorARGBChanged(this, oldValue);
									base.RaiseColorARGBChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnHatTypePersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnHatTypePersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myHatTypeStyle;
				public override string HatTypeStyle
				{
					get
					{
						return this.myHatTypeStyle;
					}
					set
					{
						if (!(object.Equals(this.HatTypeStyle, value)))
						{
							if (this.Context.OnHatTypeHatTypeStyleChanging(this, value, true))
							{
								if (base.RaiseHatTypeStyleChangingEvent(value))
								{
									string oldValue = this.HatTypeStyle;
									this.myHatTypeStyle = value;
									this.Context.OnHatTypeHatTypeStyleChanged(this, oldValue);
									base.RaiseHatTypeStyleChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // HatType
			private bool OnTaskTask_idChanging(Task instance, int newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnTaskTask_idChanged(Task instance, int oldValue)
			{
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnTaskPersonChanging(Task instance, Person newValue, bool throwOnFailure)
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
			private void OnTaskPersonChanged(Task instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Task.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.Task.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Task ISampleModelContext.CreateTask()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new TaskCore(this);
			}
			#region Task
			private readonly List<Task> myTaskCollection = new List<Task>();
			ReadOnlyCollection<Task> ISampleModelContext.TaskCollection
			{
				get
				{
					return this.myTaskCollection.AsReadOnly();
				}
			}
			private sealed class TaskCore : Task
			{
				public TaskCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myTaskCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myTask_id;
				public override int Task_id
				{
					get
					{
						return this.myTask_id;
					}
					set
					{
						if (!(object.Equals(this.Task_id, value)))
						{
							if (this.Context.OnTaskTask_idChanging(this, value, true))
							{
								if (base.RaiseTask_idChangingEvent(value))
								{
									int oldValue = this.Task_id;
									this.myTask_id = value;
									this.Context.OnTaskTask_idChanged(this, oldValue);
									base.RaiseTask_idChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnTaskPersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnTaskPersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // Task
			private bool OnCarvinChanging(Car instance, uint newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnCarvinChanged(Car instance, uint oldValue)
			{
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnCarDrivenByPersonChanging(Car instance, Person newValue, bool throwOnFailure)
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
			private void OnCarDrivenByPersonChanged(Car instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.DrivesCar.Remove(instance);
				}
				if (instance.DrivenByPerson != null)
				{
					instance.DrivenByPerson.DrivesCar.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnCarOwnedByPersonChanging(Car instance, Person newValue, bool throwOnFailure)
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
			private void OnCarOwnedByPersonChanged(Car instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.OwnsCar.Remove(instance);
				}
				if (instance.OwnedByPerson != null)
				{
					instance.OwnedByPerson.OwnsCar.Add(instance);
				}
			}
			private bool OnCarPersonBoughtCarFromPersonOnDateAsCarSoldAdding(Car instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnCarPersonBoughtCarFromPersonOnDateAsCarSoldAdded(Car instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.CarSold = instance;
				}
			}
			private bool OnCarPersonBoughtCarFromPersonOnDateAsCarSoldRemoving(Car instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				return true;
			}
			private void OnCarPersonBoughtCarFromPersonOnDateAsCarSoldRemoved(Car instance, PersonBoughtCarFromPersonOnDateAssociation value)
			{
				if (value != null)
				{
					value.CarSold = null;
				}
			}
			private bool OnCarReviewAsCarAdding(Car instance, ReviewAssociation value)
			{
				return true;
			}
			private void OnCarReviewAsCarAdded(Car instance, ReviewAssociation value)
			{
				if (value != null)
				{
					value.Car = instance;
				}
			}
			private bool OnCarReviewAsCarRemoving(Car instance, ReviewAssociation value)
			{
				return true;
			}
			private void OnCarReviewAsCarRemoved(Car instance, ReviewAssociation value)
			{
				if (value != null)
				{
					value.Car = null;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Car ISampleModelContext.CreateCar()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new CarCore(this);
			}
			#region Car
			private readonly List<Car> myCarCollection = new List<Car>();
			ReadOnlyCollection<Car> ISampleModelContext.CarCollection
			{
				get
				{
					return this.myCarCollection.AsReadOnly();
				}
			}
			private sealed class CarCore : Car
			{
				public CarCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myCarCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private uint myvin;
				public override uint vin
				{
					get
					{
						return this.myvin;
					}
					set
					{
						if (!(object.Equals(this.vin, value)))
						{
							if (this.Context.OnCarvinChanging(this, value, true))
							{
								if (base.RaisevinChangingEvent(value))
								{
									uint oldValue = this.vin;
									this.myvin = value;
									this.Context.OnCarvinChanged(this, oldValue);
									base.RaisevinChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myDrivenByPerson;
				public override Person DrivenByPerson
				{
					get
					{
						return this.myDrivenByPerson;
					}
					set
					{
						if (!(object.Equals(this.DrivenByPerson, value)))
						{
							if (this.Context.OnCarDrivenByPersonChanging(this, value, true))
							{
								if (base.RaiseDrivenByPersonChangingEvent(value))
								{
									Person oldValue = this.DrivenByPerson;
									this.myDrivenByPerson = value;
									this.Context.OnCarDrivenByPersonChanged(this, oldValue);
									base.RaiseDrivenByPersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myOwnedByPerson;
				public override Person OwnedByPerson
				{
					get
					{
						return this.myOwnedByPerson;
					}
					set
					{
						if (!(object.Equals(this.OwnedByPerson, value)))
						{
							if (this.Context.OnCarOwnedByPersonChanging(this, value, true))
							{
								if (base.RaiseOwnedByPersonChangingEvent(value))
								{
									Person oldValue = this.OwnedByPerson;
									this.myOwnedByPerson = value;
									this.Context.OnCarOwnedByPersonChanged(this, oldValue);
									base.RaiseOwnedByPersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class PersonBoughtCarFromPersonOnDateAsCarSoldCollection : ICollection<PersonBoughtCarFromPersonOnDateAssociation>
				{
					private Car myCar;
					private List<PersonBoughtCarFromPersonOnDateAssociation> myList = new List<PersonBoughtCarFromPersonOnDateAssociation>();
					public PersonBoughtCarFromPersonOnDateAsCarSoldCollection(Car instance)
					{
						this.myCar = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<PersonBoughtCarFromPersonOnDateAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myCar.Context.OnCarPersonBoughtCarFromPersonOnDateAsCarSoldAdding(this.myCar, item))
						{
							this.myList.Add(item);
							this.myCar.Context.OnCarPersonBoughtCarFromPersonOnDateAsCarSoldAdded(this.myCar, item);
						}
					}
					public bool Remove(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						if (this.myCar.Context.OnCarPersonBoughtCarFromPersonOnDateAsCarSoldRemoving(this.myCar, item))
						{
							if (this.myList.Remove(item))
							{
								this.myCar.Context.OnCarPersonBoughtCarFromPersonOnDateAsCarSoldRemoved(this.myCar, item);
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
					public bool Contains(PersonBoughtCarFromPersonOnDateAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(PersonBoughtCarFromPersonOnDateAssociation[] array, int arrayIndex)
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
				private ICollection<PersonBoughtCarFromPersonOnDateAssociation> myPersonBoughtCarFromPersonOnDateAsCarSold;
				public override ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsCarSold
				{
					get
					{
						if (this.myPersonBoughtCarFromPersonOnDateAsCarSold == null)
						{
							this.myPersonBoughtCarFromPersonOnDateAsCarSold = new PersonBoughtCarFromPersonOnDateAsCarSoldCollection(this);
						}
						return this.myPersonBoughtCarFromPersonOnDateAsCarSold;
					}
				}
				private sealed class ReviewAsCarCollection : ICollection<ReviewAssociation>
				{
					private Car myCar;
					private List<ReviewAssociation> myList = new List<ReviewAssociation>();
					public ReviewAsCarCollection(Car instance)
					{
						this.myCar = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<ReviewAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(ReviewAssociation item)
					{
						if (this.myCar.Context.OnCarReviewAsCarAdding(this.myCar, item))
						{
							this.myList.Add(item);
							this.myCar.Context.OnCarReviewAsCarAdded(this.myCar, item);
						}
					}
					public bool Remove(ReviewAssociation item)
					{
						if (this.myCar.Context.OnCarReviewAsCarRemoving(this.myCar, item))
						{
							if (this.myList.Remove(item))
							{
								this.myCar.Context.OnCarReviewAsCarRemoved(this.myCar, item);
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
					public bool Contains(ReviewAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(ReviewAssociation[] array, int arrayIndex)
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
				private ICollection<ReviewAssociation> myReviewAsCar;
				public override ICollection<ReviewAssociation> ReviewAsCar
				{
					get
					{
						if (this.myReviewAsCar == null)
						{
							this.myReviewAsCar = new ReviewAsCarCollection(this);
						}
						return this.myReviewAsCar;
					}
				}
			}
			#endregion // Car
			private bool OnDeathDeathCauseChanging(Death instance, string newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnDeathDeathCauseChanged(Death instance, string oldValue)
			{
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathPersonChanging(Death instance, Person newValue, bool throwOnFailure)
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
			private void OnDeathPersonChanged(Death instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Death.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.Death.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathDateChanging(Death instance, Date newValue, bool throwOnFailure)
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
			private void OnDeathDateChanged(Death instance, Date oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Death.Remove(instance);
				}
				if (instance.Date != null)
				{
					instance.Date.Death.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathNaturalDeathChanging(Death instance, NaturalDeath newValue, bool throwOnFailure)
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
			private void OnDeathNaturalDeathChanged(Death instance, NaturalDeath oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Death.Remove(instance);
				}
				if (instance.NaturalDeath != null)
				{
					instance.NaturalDeath.Death.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathUnnaturalDeathChanging(Death instance, UnnaturalDeath newValue, bool throwOnFailure)
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
			private void OnDeathUnnaturalDeathChanged(Death instance, UnnaturalDeath oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Death.Remove(instance);
				}
				if (instance.UnnaturalDeath != null)
				{
					instance.UnnaturalDeath.Death.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Death ISampleModelContext.CreateDeath()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new DeathCore(this);
			}
			#region Death
			private readonly List<Death> myDeathCollection = new List<Death>();
			ReadOnlyCollection<Death> ISampleModelContext.DeathCollection
			{
				get
				{
					return this.myDeathCollection.AsReadOnly();
				}
			}
			private sealed class DeathCore : Death
			{
				public DeathCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myDeathCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private string myDeathCause;
				public override string DeathCause
				{
					get
					{
						return this.myDeathCause;
					}
					set
					{
						if (!(object.Equals(this.DeathCause, value)))
						{
							if (this.Context.OnDeathDeathCauseChanging(this, value, true))
							{
								if (base.RaiseDeathCauseChangingEvent(value))
								{
									string oldValue = this.DeathCause;
									this.myDeathCause = value;
									this.Context.OnDeathDeathCauseChanged(this, oldValue);
									base.RaiseDeathCauseChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnDeathPersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnDeathPersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Date myDate;
				public override Date Date
				{
					get
					{
						return this.myDate;
					}
					set
					{
						if (!(object.Equals(this.Date, value)))
						{
							if (this.Context.OnDeathDateChanging(this, value, true))
							{
								if (base.RaiseDateChangingEvent(value))
								{
									Date oldValue = this.Date;
									this.myDate = value;
									this.Context.OnDeathDateChanged(this, oldValue);
									base.RaiseDateChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private NaturalDeath myNaturalDeath;
				public override NaturalDeath NaturalDeath
				{
					get
					{
						return this.myNaturalDeath;
					}
					set
					{
						if (!(object.Equals(this.NaturalDeath, value)))
						{
							if (this.Context.OnDeathNaturalDeathChanging(this, value, true))
							{
								if (base.RaiseNaturalDeathChangingEvent(value))
								{
									NaturalDeath oldValue = this.NaturalDeath;
									this.myNaturalDeath = value;
									this.Context.OnDeathNaturalDeathChanged(this, oldValue);
									base.RaiseNaturalDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private UnnaturalDeath myUnnaturalDeath;
				public override UnnaturalDeath UnnaturalDeath
				{
					get
					{
						return this.myUnnaturalDeath;
					}
					set
					{
						if (!(object.Equals(this.UnnaturalDeath, value)))
						{
							if (this.Context.OnDeathUnnaturalDeathChanging(this, value, true))
							{
								if (base.RaiseUnnaturalDeathChangingEvent(value))
								{
									UnnaturalDeath oldValue = this.UnnaturalDeath;
									this.myUnnaturalDeath = value;
									this.Context.OnDeathUnnaturalDeathChanged(this, oldValue);
									base.RaiseUnnaturalDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // Death
			private bool OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathAdding(NaturalDeath instance, NaturalDeathIsFromProstateCancerAssociation value)
			{
				return true;
			}
			private void OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathAdded(NaturalDeath instance, NaturalDeathIsFromProstateCancerAssociation value)
			{
				if (value != null)
				{
					value.NaturalDeath.Add(instance);
				}
			}
			private bool OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathRemoving(NaturalDeath instance, NaturalDeathIsFromProstateCancerAssociation value)
			{
				return true;
			}
			private void OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathRemoved(NaturalDeath instance, NaturalDeathIsFromProstateCancerAssociation value)
			{
				if (value != null)
				{
					value.NaturalDeath.Remove(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnNaturalDeathDeathChanging(NaturalDeath instance, Death newValue, bool throwOnFailure)
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
			private void OnNaturalDeathDeathChanged(NaturalDeath instance, Death oldValue)
			{
				if (oldValue != null)
				{
					oldValue.NaturalDeath.Remove(instance);
				}
				if (instance.Death != null)
				{
					instance.Death.NaturalDeath.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			NaturalDeath ISampleModelContext.CreateNaturalDeath()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new NaturalDeathCore(this);
			}
			#region NaturalDeath
			private readonly List<NaturalDeath> myNaturalDeathCollection = new List<NaturalDeath>();
			ReadOnlyCollection<NaturalDeath> ISampleModelContext.NaturalDeathCollection
			{
				get
				{
					return this.myNaturalDeathCollection.AsReadOnly();
				}
			}
			private sealed class NaturalDeathCore : NaturalDeath
			{
				public NaturalDeathCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myNaturalDeathCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private sealed class NaturalDeathIsFromProstateCancerAsNaturalDeathCollection : ICollection<NaturalDeathIsFromProstateCancerAssociation>
				{
					private NaturalDeath myNaturalDeath;
					private List<NaturalDeathIsFromProstateCancerAssociation> myList = new List<NaturalDeathIsFromProstateCancerAssociation>();
					public NaturalDeathIsFromProstateCancerAsNaturalDeathCollection(NaturalDeath instance)
					{
						this.myNaturalDeath = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<NaturalDeathIsFromProstateCancerAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(NaturalDeathIsFromProstateCancerAssociation item)
					{
						if (this.myNaturalDeath.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathAdding(this.myNaturalDeath, item))
						{
							this.myList.Add(item);
							this.myNaturalDeath.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathAdded(this.myNaturalDeath, item);
						}
					}
					public bool Remove(NaturalDeathIsFromProstateCancerAssociation item)
					{
						if (this.myNaturalDeath.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathRemoving(this.myNaturalDeath, item))
						{
							if (this.myList.Remove(item))
							{
								this.myNaturalDeath.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerAsNaturalDeathRemoved(this.myNaturalDeath, item);
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
					public bool Contains(NaturalDeathIsFromProstateCancerAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(NaturalDeathIsFromProstateCancerAssociation[] array, int arrayIndex)
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
				private ICollection<NaturalDeathIsFromProstateCancerAssociation> myNaturalDeathIsFromProstateCancerAsNaturalDeath;
				public override ICollection<NaturalDeathIsFromProstateCancerAssociation> NaturalDeathIsFromProstateCancerAsNaturalDeath
				{
					get
					{
						if (this.myNaturalDeathIsFromProstateCancerAsNaturalDeath == null)
						{
							this.myNaturalDeathIsFromProstateCancerAsNaturalDeath = new NaturalDeathIsFromProstateCancerAsNaturalDeathCollection(this);
						}
						return this.myNaturalDeathIsFromProstateCancerAsNaturalDeath;
					}
				}
				private Death myDeath;
				public override Death Death
				{
					get
					{
						return this.myDeath;
					}
					set
					{
						if (!(object.Equals(this.Death, value)))
						{
							if (this.Context.OnNaturalDeathDeathChanging(this, value, true))
							{
								if (base.RaiseDeathChangingEvent(value))
								{
									Death oldValue = this.Death;
									this.myDeath = value;
									this.Context.OnNaturalDeathDeathChanged(this, oldValue);
									base.RaiseDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // NaturalDeath
			private bool OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathAdding(UnnaturalDeath instance, UnnaturalDeathIsViolentAssociation value)
			{
				return true;
			}
			private void OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathAdded(UnnaturalDeath instance, UnnaturalDeathIsViolentAssociation value)
			{
				if (value != null)
				{
					value.UnnaturalDeath.Add(instance);
				}
			}
			private bool OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathRemoving(UnnaturalDeath instance, UnnaturalDeathIsViolentAssociation value)
			{
				return true;
			}
			private void OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathRemoved(UnnaturalDeath instance, UnnaturalDeathIsViolentAssociation value)
			{
				if (value != null)
				{
					value.UnnaturalDeath.Remove(instance);
				}
			}
			private bool OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathAdding(UnnaturalDeath instance, UnnaturalDeathIsBloodyAssociation value)
			{
				return true;
			}
			private void OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathAdded(UnnaturalDeath instance, UnnaturalDeathIsBloodyAssociation value)
			{
				if (value != null)
				{
					value.UnnaturalDeath.Add(instance);
				}
			}
			private bool OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathRemoving(UnnaturalDeath instance, UnnaturalDeathIsBloodyAssociation value)
			{
				return true;
			}
			private void OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathRemoved(UnnaturalDeath instance, UnnaturalDeathIsBloodyAssociation value)
			{
				if (value != null)
				{
					value.UnnaturalDeath.Remove(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnUnnaturalDeathDeathChanging(UnnaturalDeath instance, Death newValue, bool throwOnFailure)
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
			private void OnUnnaturalDeathDeathChanged(UnnaturalDeath instance, Death oldValue)
			{
				if (oldValue != null)
				{
					oldValue.UnnaturalDeath.Remove(instance);
				}
				if (instance.Death != null)
				{
					instance.Death.UnnaturalDeath.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			UnnaturalDeath ISampleModelContext.CreateUnnaturalDeath()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new UnnaturalDeathCore(this);
			}
			#region UnnaturalDeath
			private readonly List<UnnaturalDeath> myUnnaturalDeathCollection = new List<UnnaturalDeath>();
			ReadOnlyCollection<UnnaturalDeath> ISampleModelContext.UnnaturalDeathCollection
			{
				get
				{
					return this.myUnnaturalDeathCollection.AsReadOnly();
				}
			}
			private sealed class UnnaturalDeathCore : UnnaturalDeath
			{
				public UnnaturalDeathCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myUnnaturalDeathCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private sealed class UnnaturalDeathIsViolentAsUnnaturalDeathCollection : ICollection<UnnaturalDeathIsViolentAssociation>
				{
					private UnnaturalDeath myUnnaturalDeath;
					private List<UnnaturalDeathIsViolentAssociation> myList = new List<UnnaturalDeathIsViolentAssociation>();
					public UnnaturalDeathIsViolentAsUnnaturalDeathCollection(UnnaturalDeath instance)
					{
						this.myUnnaturalDeath = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<UnnaturalDeathIsViolentAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(UnnaturalDeathIsViolentAssociation item)
					{
						if (this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathAdding(this.myUnnaturalDeath, item))
						{
							this.myList.Add(item);
							this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathAdded(this.myUnnaturalDeath, item);
						}
					}
					public bool Remove(UnnaturalDeathIsViolentAssociation item)
					{
						if (this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathRemoving(this.myUnnaturalDeath, item))
						{
							if (this.myList.Remove(item))
							{
								this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsViolentAsUnnaturalDeathRemoved(this.myUnnaturalDeath, item);
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
					public bool Contains(UnnaturalDeathIsViolentAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(UnnaturalDeathIsViolentAssociation[] array, int arrayIndex)
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
				private ICollection<UnnaturalDeathIsViolentAssociation> myUnnaturalDeathIsViolentAsUnnaturalDeath;
				public override ICollection<UnnaturalDeathIsViolentAssociation> UnnaturalDeathIsViolentAsUnnaturalDeath
				{
					get
					{
						if (this.myUnnaturalDeathIsViolentAsUnnaturalDeath == null)
						{
							this.myUnnaturalDeathIsViolentAsUnnaturalDeath = new UnnaturalDeathIsViolentAsUnnaturalDeathCollection(this);
						}
						return this.myUnnaturalDeathIsViolentAsUnnaturalDeath;
					}
				}
				private sealed class UnnaturalDeathIsBloodyAsUnnaturalDeathCollection : ICollection<UnnaturalDeathIsBloodyAssociation>
				{
					private UnnaturalDeath myUnnaturalDeath;
					private List<UnnaturalDeathIsBloodyAssociation> myList = new List<UnnaturalDeathIsBloodyAssociation>();
					public UnnaturalDeathIsBloodyAsUnnaturalDeathCollection(UnnaturalDeath instance)
					{
						this.myUnnaturalDeath = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<UnnaturalDeathIsBloodyAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(UnnaturalDeathIsBloodyAssociation item)
					{
						if (this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathAdding(this.myUnnaturalDeath, item))
						{
							this.myList.Add(item);
							this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathAdded(this.myUnnaturalDeath, item);
						}
					}
					public bool Remove(UnnaturalDeathIsBloodyAssociation item)
					{
						if (this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathRemoving(this.myUnnaturalDeath, item))
						{
							if (this.myList.Remove(item))
							{
								this.myUnnaturalDeath.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyAsUnnaturalDeathRemoved(this.myUnnaturalDeath, item);
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
					public bool Contains(UnnaturalDeathIsBloodyAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(UnnaturalDeathIsBloodyAssociation[] array, int arrayIndex)
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
				private ICollection<UnnaturalDeathIsBloodyAssociation> myUnnaturalDeathIsBloodyAsUnnaturalDeath;
				public override ICollection<UnnaturalDeathIsBloodyAssociation> UnnaturalDeathIsBloodyAsUnnaturalDeath
				{
					get
					{
						if (this.myUnnaturalDeathIsBloodyAsUnnaturalDeath == null)
						{
							this.myUnnaturalDeathIsBloodyAsUnnaturalDeath = new UnnaturalDeathIsBloodyAsUnnaturalDeathCollection(this);
						}
						return this.myUnnaturalDeathIsBloodyAsUnnaturalDeath;
					}
				}
				private Death myDeath;
				public override Death Death
				{
					get
					{
						return this.myDeath;
					}
					set
					{
						if (!(object.Equals(this.Death, value)))
						{
							if (this.Context.OnUnnaturalDeathDeathChanging(this, value, true))
							{
								if (base.RaiseDeathChangingEvent(value))
								{
									Death oldValue = this.Death;
									this.myDeath = value;
									this.Context.OnUnnaturalDeathDeathChanged(this, oldValue);
									base.RaiseDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // UnnaturalDeath
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnMalePersonChanging(Male instance, Person newValue, bool throwOnFailure)
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
			private void OnMalePersonChanged(Male instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Male.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.Male.Add(instance);
				}
			}
			private bool OnMaleChildAsMaleAdding(Male instance, ChildAssociation value)
			{
				return true;
			}
			private void OnMaleChildAsMaleAdded(Male instance, ChildAssociation value)
			{
				if (value != null)
				{
					value.Male = instance;
				}
			}
			private bool OnMaleChildAsMaleRemoving(Male instance, ChildAssociation value)
			{
				return true;
			}
			private void OnMaleChildAsMaleRemoved(Male instance, ChildAssociation value)
			{
				if (value != null)
				{
					value.Male = null;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnMaleChildChanging(Male instance, Child newValue, bool throwOnFailure)
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
			private void OnMaleChildChanged(Male instance, Child oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Male.Remove(instance);
				}
				if (instance.Child != null)
				{
					instance.Child.Male.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Male ISampleModelContext.CreateMale()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new MaleCore(this);
			}
			#region Male
			private readonly List<Male> myMaleCollection = new List<Male>();
			ReadOnlyCollection<Male> ISampleModelContext.MaleCollection
			{
				get
				{
					return this.myMaleCollection.AsReadOnly();
				}
			}
			private sealed class MaleCore : Male
			{
				public MaleCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myMaleCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnMalePersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnMalePersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class ChildAsMaleCollection : ICollection<ChildAssociation>
				{
					private Male myMale;
					private List<ChildAssociation> myList = new List<ChildAssociation>();
					public ChildAsMaleCollection(Male instance)
					{
						this.myMale = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<ChildAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(ChildAssociation item)
					{
						if (this.myMale.Context.OnMaleChildAsMaleAdding(this.myMale, item))
						{
							this.myList.Add(item);
							this.myMale.Context.OnMaleChildAsMaleAdded(this.myMale, item);
						}
					}
					public bool Remove(ChildAssociation item)
					{
						if (this.myMale.Context.OnMaleChildAsMaleRemoving(this.myMale, item))
						{
							if (this.myList.Remove(item))
							{
								this.myMale.Context.OnMaleChildAsMaleRemoved(this.myMale, item);
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
					public bool Contains(ChildAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(ChildAssociation[] array, int arrayIndex)
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
				private ICollection<ChildAssociation> myChildAsMale;
				public override ICollection<ChildAssociation> ChildAsMale
				{
					get
					{
						if (this.myChildAsMale == null)
						{
							this.myChildAsMale = new ChildAsMaleCollection(this);
						}
						return this.myChildAsMale;
					}
				}
				private Child myChild;
				public override Child Child
				{
					get
					{
						return this.myChild;
					}
					set
					{
						if (!(object.Equals(this.Child, value)))
						{
							if (this.Context.OnMaleChildChanging(this, value, true))
							{
								if (base.RaiseChildChangingEvent(value))
								{
									Child oldValue = this.Child;
									this.myChild = value;
									this.Context.OnMaleChildChanged(this, oldValue);
									base.RaiseChildChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // Male
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnFemalePersonChanging(Female instance, Person newValue, bool throwOnFailure)
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
			private void OnFemalePersonChanged(Female instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Female.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.Female.Add(instance);
				}
			}
			private bool OnFemaleChildAsFemaleAdding(Female instance, ChildAssociation value)
			{
				return true;
			}
			private void OnFemaleChildAsFemaleAdded(Female instance, ChildAssociation value)
			{
				if (value != null)
				{
					value.Female = instance;
				}
			}
			private bool OnFemaleChildAsFemaleRemoving(Female instance, ChildAssociation value)
			{
				return true;
			}
			private void OnFemaleChildAsFemaleRemoved(Female instance, ChildAssociation value)
			{
				if (value != null)
				{
					value.Female = null;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnFemaleChildChanging(Female instance, Child newValue, bool throwOnFailure)
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
			private void OnFemaleChildChanged(Female instance, Child oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Female.Remove(instance);
				}
				if (instance.Child != null)
				{
					instance.Child.Female.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Female ISampleModelContext.CreateFemale()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new FemaleCore(this);
			}
			#region Female
			private readonly List<Female> myFemaleCollection = new List<Female>();
			ReadOnlyCollection<Female> ISampleModelContext.FemaleCollection
			{
				get
				{
					return this.myFemaleCollection.AsReadOnly();
				}
			}
			private sealed class FemaleCore : Female
			{
				public FemaleCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myFemaleCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnFemalePersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnFemalePersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class ChildAsFemaleCollection : ICollection<ChildAssociation>
				{
					private Female myFemale;
					private List<ChildAssociation> myList = new List<ChildAssociation>();
					public ChildAsFemaleCollection(Female instance)
					{
						this.myFemale = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<ChildAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(ChildAssociation item)
					{
						if (this.myFemale.Context.OnFemaleChildAsFemaleAdding(this.myFemale, item))
						{
							this.myList.Add(item);
							this.myFemale.Context.OnFemaleChildAsFemaleAdded(this.myFemale, item);
						}
					}
					public bool Remove(ChildAssociation item)
					{
						if (this.myFemale.Context.OnFemaleChildAsFemaleRemoving(this.myFemale, item))
						{
							if (this.myList.Remove(item))
							{
								this.myFemale.Context.OnFemaleChildAsFemaleRemoved(this.myFemale, item);
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
					public bool Contains(ChildAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(ChildAssociation[] array, int arrayIndex)
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
				private ICollection<ChildAssociation> myChildAsFemale;
				public override ICollection<ChildAssociation> ChildAsFemale
				{
					get
					{
						if (this.myChildAsFemale == null)
						{
							this.myChildAsFemale = new ChildAsFemaleCollection(this);
						}
						return this.myChildAsFemale;
					}
				}
				private Child myChild;
				public override Child Child
				{
					get
					{
						return this.myChild;
					}
					set
					{
						if (!(object.Equals(this.Child, value)))
						{
							if (this.Context.OnFemaleChildChanging(this, value, true))
							{
								if (base.RaiseChildChangingEvent(value))
								{
									Child oldValue = this.Child;
									this.myChild = value;
									this.Context.OnFemaleChildChanged(this, oldValue);
									base.RaiseChildChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // Female
			private bool OnBirthOrderBirthOrderNrChanging(BirthOrder instance, uint newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnBirthOrderBirthOrderNrChanged(BirthOrder instance, uint oldValue)
			{
			}
			private bool OnBirthOrderChildAsBirthOrderAdding(BirthOrder instance, ChildAssociation value)
			{
				return true;
			}
			private void OnBirthOrderChildAsBirthOrderAdded(BirthOrder instance, ChildAssociation value)
			{
				if (value != null)
				{
					value.BirthOrder = instance;
				}
			}
			private bool OnBirthOrderChildAsBirthOrderRemoving(BirthOrder instance, ChildAssociation value)
			{
				return true;
			}
			private void OnBirthOrderChildAsBirthOrderRemoved(BirthOrder instance, ChildAssociation value)
			{
				if (value != null)
				{
					value.BirthOrder = null;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnBirthOrderChildChanging(BirthOrder instance, Child newValue, bool throwOnFailure)
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
			private void OnBirthOrderChildChanged(BirthOrder instance, Child oldValue)
			{
				if (oldValue != null)
				{
					oldValue.BirthOrder.Remove(instance);
				}
				if (instance.Child != null)
				{
					instance.Child.BirthOrder.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			BirthOrder ISampleModelContext.CreateBirthOrder()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new BirthOrderCore(this);
			}
			#region BirthOrder
			private readonly List<BirthOrder> myBirthOrderCollection = new List<BirthOrder>();
			ReadOnlyCollection<BirthOrder> ISampleModelContext.BirthOrderCollection
			{
				get
				{
					return this.myBirthOrderCollection.AsReadOnly();
				}
			}
			private sealed class BirthOrderCore : BirthOrder
			{
				public BirthOrderCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myBirthOrderCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private uint myBirthOrderNr;
				public override uint BirthOrderNr
				{
					get
					{
						return this.myBirthOrderNr;
					}
					set
					{
						if (!(object.Equals(this.BirthOrderNr, value)))
						{
							if (this.Context.OnBirthOrderBirthOrderNrChanging(this, value, true))
							{
								if (base.RaiseBirthOrderNrChangingEvent(value))
								{
									uint oldValue = this.BirthOrderNr;
									this.myBirthOrderNr = value;
									this.Context.OnBirthOrderBirthOrderNrChanged(this, oldValue);
									base.RaiseBirthOrderNrChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private sealed class ChildAsBirthOrderCollection : ICollection<ChildAssociation>
				{
					private BirthOrder myBirthOrder;
					private List<ChildAssociation> myList = new List<ChildAssociation>();
					public ChildAsBirthOrderCollection(BirthOrder instance)
					{
						this.myBirthOrder = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<ChildAssociation> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(ChildAssociation item)
					{
						if (this.myBirthOrder.Context.OnBirthOrderChildAsBirthOrderAdding(this.myBirthOrder, item))
						{
							this.myList.Add(item);
							this.myBirthOrder.Context.OnBirthOrderChildAsBirthOrderAdded(this.myBirthOrder, item);
						}
					}
					public bool Remove(ChildAssociation item)
					{
						if (this.myBirthOrder.Context.OnBirthOrderChildAsBirthOrderRemoving(this.myBirthOrder, item))
						{
							if (this.myList.Remove(item))
							{
								this.myBirthOrder.Context.OnBirthOrderChildAsBirthOrderRemoved(this.myBirthOrder, item);
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
					public bool Contains(ChildAssociation item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(ChildAssociation[] array, int arrayIndex)
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
				private ICollection<ChildAssociation> myChildAsBirthOrder;
				public override ICollection<ChildAssociation> ChildAsBirthOrder
				{
					get
					{
						if (this.myChildAsBirthOrder == null)
						{
							this.myChildAsBirthOrder = new ChildAsBirthOrderCollection(this);
						}
						return this.myChildAsBirthOrder;
					}
				}
				private Child myChild;
				public override Child Child
				{
					get
					{
						return this.myChild;
					}
					set
					{
						if (!(object.Equals(this.Child, value)))
						{
							if (this.Context.OnBirthOrderChildChanging(this, value, true))
							{
								if (base.RaiseChildChangingEvent(value))
								{
									Child oldValue = this.Child;
									this.myChild = value;
									this.Context.OnBirthOrderChildChanged(this, oldValue);
									base.RaiseChildChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // BirthOrder
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildMaleChanging(Child instance, Male newValue, bool throwOnFailure)
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
			private void OnChildMaleChanged(Child instance, Male oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Child.Remove(instance);
				}
				if (instance.Male != null)
				{
					instance.Male.Child.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildBirthOrderChanging(Child instance, BirthOrder newValue, bool throwOnFailure)
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
			private void OnChildBirthOrderChanged(Child instance, BirthOrder oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Child.Remove(instance);
				}
				if (instance.BirthOrder != null)
				{
					instance.BirthOrder.Child.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildFemaleChanging(Child instance, Female newValue, bool throwOnFailure)
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
			private void OnChildFemaleChanged(Child instance, Female oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Child.Remove(instance);
				}
				if (instance.Female != null)
				{
					instance.Female.Child.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonChanging(Child instance, Person newValue, bool throwOnFailure)
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
			private void OnChildPersonChanged(Child instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Child.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.Child.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Child ISampleModelContext.CreateChild()
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				return new ChildCore(this);
			}
			#region Child
			private readonly List<Child> myChildCollection = new List<Child>();
			ReadOnlyCollection<Child> ISampleModelContext.ChildCollection
			{
				get
				{
					return this.myChildCollection.AsReadOnly();
				}
			}
			private sealed class ChildCore : Child
			{
				public ChildCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myChildCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Male myMale;
				public override Male Male
				{
					get
					{
						return this.myMale;
					}
					set
					{
						if (!(object.Equals(this.Male, value)))
						{
							if (this.Context.OnChildMaleChanging(this, value, true))
							{
								if (base.RaiseMaleChangingEvent(value))
								{
									Male oldValue = this.Male;
									this.myMale = value;
									this.Context.OnChildMaleChanged(this, oldValue);
									base.RaiseMaleChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private BirthOrder myBirthOrder;
				public override BirthOrder BirthOrder
				{
					get
					{
						return this.myBirthOrder;
					}
					set
					{
						if (!(object.Equals(this.BirthOrder, value)))
						{
							if (this.Context.OnChildBirthOrderChanging(this, value, true))
							{
								if (base.RaiseBirthOrderChangingEvent(value))
								{
									BirthOrder oldValue = this.BirthOrder;
									this.myBirthOrder = value;
									this.Context.OnChildBirthOrderChanged(this, oldValue);
									base.RaiseBirthOrderChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Female myFemale;
				public override Female Female
				{
					get
					{
						return this.myFemale;
					}
					set
					{
						if (!(object.Equals(this.Female, value)))
						{
							if (this.Context.OnChildFemaleChanging(this, value, true))
							{
								if (base.RaiseFemaleChangingEvent(value))
								{
									Female oldValue = this.Female;
									this.myFemale = value;
									this.Context.OnChildFemaleChanged(this, oldValue);
									base.RaiseFemaleChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person myPerson;
				public override Person Person
				{
					get
					{
						return this.myPerson;
					}
					set
					{
						if (!(object.Equals(this.Person, value)))
						{
							if (this.Context.OnChildPersonChanging(this, value, true))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnChildPersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // Child
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanging(PersonBoughtCarFromPersonOnDateAssociation instance, Person newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(newValue, instance.CarSold, instance.Seller))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanged(PersonBoughtCarFromPersonOnDateAssociation instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonBoughtCarFromPersonOnDateAsBuyer.Remove(instance);
				}
				if (instance.Buyer != null)
				{
					instance.Buyer.PersonBoughtCarFromPersonOnDateAsBuyer.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, Tuple.CreateTuple(oldValue, instance.CarSold, instance.Seller), Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint25Changed(instance, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, oldValue), Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.Buyer != null)
				{
					instance.Buyer.PersonBoughtCarFromPersonOnDateAsBuyer.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, null, Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint25Changed(instance, null, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanging(PersonBoughtCarFromPersonOnDateAssociation instance, Car newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(instance.Buyer, newValue, instance.Seller))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(instance.SaleDate, instance.Seller, newValue))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(newValue, instance.SaleDate, instance.Buyer))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanged(PersonBoughtCarFromPersonOnDateAssociation instance, Car oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonBoughtCarFromPersonOnDateAsCarSold.Remove(instance);
				}
				if (instance.CarSold != null)
				{
					instance.CarSold.PersonBoughtCarFromPersonOnDateAsCarSold.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, Tuple.CreateTuple(instance.Buyer, oldValue, instance.Seller), Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint24Changed(instance, Tuple.CreateTuple(instance.SaleDate, instance.Seller, oldValue), Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint25Changed(instance, Tuple.CreateTuple(oldValue, instance.SaleDate, instance.Buyer), Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.CarSold != null)
				{
					instance.CarSold.PersonBoughtCarFromPersonOnDateAsCarSold.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, null, Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint24Changed(instance, null, Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint25Changed(instance, null, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateAssociationSellerChanging(PersonBoughtCarFromPersonOnDateAssociation instance, Person newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(instance.Buyer, instance.CarSold, newValue))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(instance.SaleDate, newValue, instance.CarSold))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationSellerChanged(PersonBoughtCarFromPersonOnDateAssociation instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonBoughtCarFromPersonOnDateAsSeller.Remove(instance);
				}
				if (instance.Seller != null)
				{
					instance.Seller.PersonBoughtCarFromPersonOnDateAsSeller.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, Tuple.CreateTuple(instance.Buyer, instance.CarSold, oldValue), Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint24Changed(instance, Tuple.CreateTuple(instance.SaleDate, oldValue, instance.CarSold), Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationSellerChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.Seller != null)
				{
					instance.Seller.PersonBoughtCarFromPersonOnDateAsSeller.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, null, Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint24Changed(instance, null, Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanging(PersonBoughtCarFromPersonOnDateAssociation instance, Date newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(newValue, instance.Seller, instance.CarSold))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(instance.CarSold, newValue, instance.Buyer))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanged(PersonBoughtCarFromPersonOnDateAssociation instance, Date oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonBoughtCarFromPersonOnDateAsSaleDate.Remove(instance);
				}
				if (instance.SaleDate != null)
				{
					instance.SaleDate.PersonBoughtCarFromPersonOnDateAsSaleDate.Add(instance);
				}
				this.OnInternalUniquenessConstraint24Changed(instance, Tuple.CreateTuple(oldValue, instance.Seller, instance.CarSold), Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint25Changed(instance, Tuple.CreateTuple(instance.CarSold, oldValue, instance.Buyer), Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.SaleDate != null)
				{
					instance.SaleDate.PersonBoughtCarFromPersonOnDateAsSaleDate.Add(instance);
				}
				this.OnInternalUniquenessConstraint24Changed(instance, null, Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint25Changed(instance, null, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.CreatePersonBoughtCarFromPersonOnDateAssociation(Person Buyer, Car CarSold, Person Seller, Date SaleDate)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanging(null, Buyer, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Buyer");
				}
				if (!(this.OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanging(null, CarSold, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "CarSold");
				}
				if (!(this.OnPersonBoughtCarFromPersonOnDateAssociationSellerChanging(null, Seller, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Seller");
				}
				if (!(this.OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanging(null, SaleDate, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "SaleDate");
				}
				return new PersonBoughtCarFromPersonOnDateAssociationCore(this, Buyer, CarSold, Seller, SaleDate);
			}
			#region PersonBoughtCarFromPersonOnDateAssociation
			private readonly List<PersonBoughtCarFromPersonOnDateAssociation> myPersonBoughtCarFromPersonOnDateAssociationCollection = new List<PersonBoughtCarFromPersonOnDateAssociation>();
			ReadOnlyCollection<PersonBoughtCarFromPersonOnDateAssociation> ISampleModelContext.PersonBoughtCarFromPersonOnDateAssociationCollection
			{
				get
				{
					return this.myPersonBoughtCarFromPersonOnDateAssociationCollection.AsReadOnly();
				}
			}
			private sealed class PersonBoughtCarFromPersonOnDateAssociationCore : PersonBoughtCarFromPersonOnDateAssociation
			{
				public PersonBoughtCarFromPersonOnDateAssociationCore(SampleModelContext context, Person Buyer, Car CarSold, Person Seller, Date SaleDate)
				{
					this.myContext = context;
					this.myBuyer = Buyer;
					this.Context.OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanged(this);
					this.myCarSold = CarSold;
					this.Context.OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanged(this);
					this.mySeller = Seller;
					this.Context.OnPersonBoughtCarFromPersonOnDateAssociationSellerChanged(this);
					this.mySaleDate = SaleDate;
					this.Context.OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanged(this);
					context.myPersonBoughtCarFromPersonOnDateAssociationCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Person myBuyer;
				public override Person Buyer
				{
					get
					{
						return this.myBuyer;
					}
					set
					{
						if (!(object.Equals(this.Buyer, value)))
						{
							if (this.Context.OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanging(this, value, true))
							{
								if (base.RaiseBuyerChangingEvent(value))
								{
									Person oldValue = this.Buyer;
									this.myBuyer = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanged(this, oldValue);
									base.RaiseBuyerChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Car myCarSold;
				public override Car CarSold
				{
					get
					{
						return this.myCarSold;
					}
					set
					{
						if (!(object.Equals(this.CarSold, value)))
						{
							if (this.Context.OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanging(this, value, true))
							{
								if (base.RaiseCarSoldChangingEvent(value))
								{
									Car oldValue = this.CarSold;
									this.myCarSold = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanged(this, oldValue);
									base.RaiseCarSoldChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Person mySeller;
				public override Person Seller
				{
					get
					{
						return this.mySeller;
					}
					set
					{
						if (!(object.Equals(this.Seller, value)))
						{
							if (this.Context.OnPersonBoughtCarFromPersonOnDateAssociationSellerChanging(this, value, true))
							{
								if (base.RaiseSellerChangingEvent(value))
								{
									Person oldValue = this.Seller;
									this.mySeller = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateAssociationSellerChanged(this, oldValue);
									base.RaiseSellerChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Date mySaleDate;
				public override Date SaleDate
				{
					get
					{
						return this.mySaleDate;
					}
					set
					{
						if (!(object.Equals(this.SaleDate, value)))
						{
							if (this.Context.OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanging(this, value, true))
							{
								if (base.RaiseSaleDateChangingEvent(value))
								{
									Date oldValue = this.SaleDate;
									this.mySaleDate = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanged(this, oldValue);
									base.RaiseSaleDateChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // PersonBoughtCarFromPersonOnDateAssociation
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildAssociationMaleChanging(ChildAssociation instance, Male newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint49Changing(instance, Tuple.CreateTuple(newValue, instance.BirthOrder, instance.Female))))
				{
					return false;
				}
				return true;
			}
			private void OnChildAssociationMaleChanged(ChildAssociation instance, Male oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ChildAsMale.Remove(instance);
				}
				if (instance.Male != null)
				{
					instance.Male.ChildAsMale.Add(instance);
				}
				this.OnInternalUniquenessConstraint49Changed(instance, Tuple.CreateTuple(oldValue, instance.BirthOrder, instance.Female), Tuple.CreateTuple(instance.Male, instance.BirthOrder, instance.Female));
			}
			private void OnChildAssociationMaleChanged(ChildAssociation instance)
			{
				if (instance.Male != null)
				{
					instance.Male.ChildAsMale.Add(instance);
				}
				this.OnInternalUniquenessConstraint49Changed(instance, null, Tuple.CreateTuple(instance.Male, instance.BirthOrder, instance.Female));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildAssociationBirthOrderChanging(ChildAssociation instance, BirthOrder newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint49Changing(instance, Tuple.CreateTuple(instance.Male, newValue, instance.Female))))
				{
					return false;
				}
				return true;
			}
			private void OnChildAssociationBirthOrderChanged(ChildAssociation instance, BirthOrder oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ChildAsBirthOrder.Remove(instance);
				}
				if (instance.BirthOrder != null)
				{
					instance.BirthOrder.ChildAsBirthOrder.Add(instance);
				}
				this.OnInternalUniquenessConstraint49Changed(instance, Tuple.CreateTuple(instance.Male, oldValue, instance.Female), Tuple.CreateTuple(instance.Male, instance.BirthOrder, instance.Female));
			}
			private void OnChildAssociationBirthOrderChanged(ChildAssociation instance)
			{
				if (instance.BirthOrder != null)
				{
					instance.BirthOrder.ChildAsBirthOrder.Add(instance);
				}
				this.OnInternalUniquenessConstraint49Changed(instance, null, Tuple.CreateTuple(instance.Male, instance.BirthOrder, instance.Female));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildAssociationFemaleChanging(ChildAssociation instance, Female newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint49Changing(instance, Tuple.CreateTuple(instance.Male, instance.BirthOrder, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnChildAssociationFemaleChanged(ChildAssociation instance, Female oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ChildAsFemale.Remove(instance);
				}
				if (instance.Female != null)
				{
					instance.Female.ChildAsFemale.Add(instance);
				}
				this.OnInternalUniquenessConstraint49Changed(instance, Tuple.CreateTuple(instance.Male, instance.BirthOrder, oldValue), Tuple.CreateTuple(instance.Male, instance.BirthOrder, instance.Female));
			}
			private void OnChildAssociationFemaleChanged(ChildAssociation instance)
			{
				if (instance.Female != null)
				{
					instance.Female.ChildAsFemale.Add(instance);
				}
				this.OnInternalUniquenessConstraint49Changed(instance, null, Tuple.CreateTuple(instance.Male, instance.BirthOrder, instance.Female));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			ChildAssociation ISampleModelContext.CreateChildAssociation(Male Male, BirthOrder BirthOrder, Female Female)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnChildAssociationMaleChanging(null, Male, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Male");
				}
				if (!(this.OnChildAssociationBirthOrderChanging(null, BirthOrder, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "BirthOrder");
				}
				if (!(this.OnChildAssociationFemaleChanging(null, Female, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Female");
				}
				return new ChildAssociationCore(this, Male, BirthOrder, Female);
			}
			#region ChildAssociation
			private readonly List<ChildAssociation> myChildAssociationCollection = new List<ChildAssociation>();
			ReadOnlyCollection<ChildAssociation> ISampleModelContext.ChildAssociationCollection
			{
				get
				{
					return this.myChildAssociationCollection.AsReadOnly();
				}
			}
			private sealed class ChildAssociationCore : ChildAssociation
			{
				public ChildAssociationCore(SampleModelContext context, Male Male, BirthOrder BirthOrder, Female Female)
				{
					this.myContext = context;
					this.myMale = Male;
					this.Context.OnChildAssociationMaleChanged(this);
					this.myBirthOrder = BirthOrder;
					this.Context.OnChildAssociationBirthOrderChanged(this);
					this.myFemale = Female;
					this.Context.OnChildAssociationFemaleChanged(this);
					context.myChildAssociationCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Male myMale;
				public override Male Male
				{
					get
					{
						return this.myMale;
					}
					set
					{
						if (!(object.Equals(this.Male, value)))
						{
							if (this.Context.OnChildAssociationMaleChanging(this, value, true))
							{
								if (base.RaiseMaleChangingEvent(value))
								{
									Male oldValue = this.Male;
									this.myMale = value;
									this.Context.OnChildAssociationMaleChanged(this, oldValue);
									base.RaiseMaleChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private BirthOrder myBirthOrder;
				public override BirthOrder BirthOrder
				{
					get
					{
						return this.myBirthOrder;
					}
					set
					{
						if (!(object.Equals(this.BirthOrder, value)))
						{
							if (this.Context.OnChildAssociationBirthOrderChanging(this, value, true))
							{
								if (base.RaiseBirthOrderChangingEvent(value))
								{
									BirthOrder oldValue = this.BirthOrder;
									this.myBirthOrder = value;
									this.Context.OnChildAssociationBirthOrderChanged(this, oldValue);
									base.RaiseBirthOrderChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Female myFemale;
				public override Female Female
				{
					get
					{
						return this.myFemale;
					}
					set
					{
						if (!(object.Equals(this.Female, value)))
						{
							if (this.Context.OnChildAssociationFemaleChanging(this, value, true))
							{
								if (base.RaiseFemaleChangingEvent(value))
								{
									Female oldValue = this.Female;
									this.myFemale = value;
									this.Context.OnChildAssociationFemaleChanged(this, oldValue);
									base.RaiseFemaleChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // ChildAssociation
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnReviewAssociationCarChanging(ReviewAssociation instance, Car newValue, bool throwOnFailure)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(newValue, instance.CriteriaName))))
				{
					return false;
				}
				return true;
			}
			private void OnReviewAssociationCarChanged(ReviewAssociation instance, Car oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ReviewAsCar.Remove(instance);
				}
				if (instance.Car != null)
				{
					instance.Car.ReviewAsCar.Add(instance);
				}
				this.OnInternalUniquenessConstraint26Changed(instance, Tuple.CreateTuple(oldValue, instance.CriteriaName), Tuple.CreateTuple(instance.Car, instance.CriteriaName));
			}
			private void OnReviewAssociationCarChanged(ReviewAssociation instance)
			{
				if (instance.Car != null)
				{
					instance.Car.ReviewAsCar.Add(instance);
				}
				this.OnInternalUniquenessConstraint26Changed(instance, null, Tuple.CreateTuple(instance.Car, instance.CriteriaName));
			}
			private bool OnReviewAssociationRatingNrChanging(ReviewAssociation instance, int newValue, bool throwOnFailure)
			{
				if (!(SampleModelContext.ValueConstraintForInteger(newValue, throwOnFailure)))
				{
					return false;
				}
				return true;
			}
			private void OnReviewAssociationRatingNrChanged(ReviewAssociation instance, int oldValue)
			{
			}
			private void OnReviewAssociationRatingNrChanged(ReviewAssociation instance)
			{
			}
			private bool OnReviewAssociationCriteriaNameChanging(ReviewAssociation instance, string newValue, bool throwOnFailure)
			{
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(instance.Car, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnReviewAssociationCriteriaNameChanged(ReviewAssociation instance, string oldValue)
			{
				this.OnInternalUniquenessConstraint26Changed(instance, Tuple.CreateTuple(instance.Car, oldValue), Tuple.CreateTuple(instance.Car, instance.CriteriaName));
			}
			private void OnReviewAssociationCriteriaNameChanged(ReviewAssociation instance)
			{
				this.OnInternalUniquenessConstraint26Changed(instance, null, Tuple.CreateTuple(instance.Car, instance.CriteriaName));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			ReviewAssociation ISampleModelContext.CreateReviewAssociation(Car Car, int RatingNr, string CriteriaName)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnReviewAssociationCarChanging(null, Car, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Car");
				}
				if (!(this.OnReviewAssociationRatingNrChanging(null, RatingNr, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "RatingNr");
				}
				if (!(this.OnReviewAssociationCriteriaNameChanging(null, CriteriaName, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "CriteriaName");
				}
				return new ReviewAssociationCore(this, Car, RatingNr, CriteriaName);
			}
			#region ReviewAssociation
			private readonly List<ReviewAssociation> myReviewAssociationCollection = new List<ReviewAssociation>();
			ReadOnlyCollection<ReviewAssociation> ISampleModelContext.ReviewAssociationCollection
			{
				get
				{
					return this.myReviewAssociationCollection.AsReadOnly();
				}
			}
			private sealed class ReviewAssociationCore : ReviewAssociation
			{
				public ReviewAssociationCore(SampleModelContext context, Car Car, int RatingNr, string CriteriaName)
				{
					this.myContext = context;
					this.myCar = Car;
					this.Context.OnReviewAssociationCarChanged(this);
					this.myRatingNr = RatingNr;
					this.Context.OnReviewAssociationRatingNrChanged(this);
					this.myCriteriaName = CriteriaName;
					this.Context.OnReviewAssociationCriteriaNameChanged(this);
					context.myReviewAssociationCollection.Add(this);
				}
				private SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Car myCar;
				public override Car Car
				{
					get
					{
						return this.myCar;
					}
					set
					{
						if (!(object.Equals(this.Car, value)))
						{
							if (this.Context.OnReviewAssociationCarChanging(this, value, true))
							{
								if (base.RaiseCarChangingEvent(value))
								{
									Car oldValue = this.Car;
									this.myCar = value;
									this.Context.OnReviewAssociationCarChanged(this, oldValue);
									base.RaiseCarChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private int myRatingNr;
				public override int RatingNr
				{
					get
					{
						return this.myRatingNr;
					}
					set
					{
						if (!(object.Equals(this.RatingNr, value)))
						{
							if (this.Context.OnReviewAssociationRatingNrChanging(this, value, true))
							{
								if (base.RaiseRatingNrChangingEvent(value))
								{
									int oldValue = this.RatingNr;
									this.myRatingNr = value;
									this.Context.OnReviewAssociationRatingNrChanged(this, oldValue);
									base.RaiseRatingNrChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myCriteriaName;
				public override string CriteriaName
				{
					get
					{
						return this.myCriteriaName;
					}
					set
					{
						if (!(object.Equals(this.CriteriaName, value)))
						{
							if (this.Context.OnReviewAssociationCriteriaNameChanging(this, value, true))
							{
								if (base.RaiseCriteriaNameChangingEvent(value))
								{
									string oldValue = this.CriteriaName;
									this.myCriteriaName = value;
									this.Context.OnReviewAssociationCriteriaNameChanged(this, oldValue);
									base.RaiseCriteriaNameChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // ReviewAssociation
			#region DeserializationSampleModelContext
			private sealed class DeserializationSampleModelContext : IDeserializationSampleModelContext
			{
				private SampleModelContext myContext;
				public DeserializationSampleModelContext(SampleModelContext context)
				{
					this.myContext = context;
					context.myIsDeserializing = true;
				}
				public void Dispose()
				{
					this.myContext.myIsDeserializing = false;
				}
				public Person CreatePerson()
				{
					return new PersonCore(this.myContext);
				}
				public Date CreateDate()
				{
					return new DateCore(this.myContext);
				}
				public HatType CreateHatType()
				{
					return new HatTypeCore(this.myContext);
				}
				public Task CreateTask()
				{
					return new TaskCore(this.myContext);
				}
				public Car CreateCar()
				{
					return new CarCore(this.myContext);
				}
				public Death CreateDeath()
				{
					return new DeathCore(this.myContext);
				}
				public NaturalDeath CreateNaturalDeath()
				{
					return new NaturalDeathCore(this.myContext);
				}
				public UnnaturalDeath CreateUnnaturalDeath()
				{
					return new UnnaturalDeathCore(this.myContext);
				}
				public Male CreateMale()
				{
					return new MaleCore(this.myContext);
				}
				public Female CreateFemale()
				{
					return new FemaleCore(this.myContext);
				}
				public BirthOrder CreateBirthOrder()
				{
					return new BirthOrderCore(this.myContext);
				}
				public Child CreateChild()
				{
					return new ChildCore(this.myContext);
				}
				public PersonBoughtCarFromPersonOnDateAssociation CreatePersonBoughtCarFromPersonOnDateAssociation()
				{
					return new PersonBoughtCarFromPersonOnDateAssociationCore(this.myContext, null, null, null, null);
				}
				public ChildAssociation CreateChildAssociation()
				{
					return new ChildAssociationCore(this.myContext, null, null, null);
				}
				public ReviewAssociation CreateReviewAssociation(int RatingNr, string CriteriaName)
				{
					return new ReviewAssociationCore(this.myContext, null, RatingNr, CriteriaName);
				}
			}
			#endregion // DeserializationSampleModelContext
		}
		#endregion // SampleModelContext
	}
}

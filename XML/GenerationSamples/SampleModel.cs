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

// <ao:Object type="EntityType" id="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" name="Person"><ao:AbsorbedObject type="ValueType" ref="_73D777FE-CDC8-40DB-A544-442931809885" name="Person_id" unique="true" multiplicity="ExactlyOne" thisRoleName="Person" thisRoleRef="_DC92FEF0-36B3-4145-BDEA-2362CCC79718" oppositeRoleRef="_FFF695AD-A1B7-4970-A717-D8D40BE4249C" oppositeRoleName="Person_id" mandatory="true" /><ao:AbsorbedObject type="ValueType" ref="_1AA2E229-66B0-42EA-8EB6-FAC6C3A2A2FB" name="FirstName" unique="false" multiplicity="ExactlyOne" thisRoleName="Person" thisRoleRef="_70A330F0-1AC2-486B-8469-EC115A515AC1" oppositeRoleRef="_0D9E47FC-891C-45C5-95E6-CEC245F61265" oppositeRoleName="FirstName" mandatory="true" /><ao:AbsorbedObject type="ValueType" ref="_AEF8A0BD-CED0-42E2-8859-E06613051DEA" name="LastName" unique="false" multiplicity="ExactlyOne" thisRoleName="Person" thisRoleRef="_AFA5D9D8-8EC4-460E-A2E0-66E8F2FF598D" oppositeRoleRef="_089350B7-DAB7-4338-A554-275973B70EA6" oppositeRoleName="LastName" mandatory="true" /><ao:AbsorbedObject type="ValueType" ref="_91E37AB6-231B-4463-BF39-8BA50E5183A6" name="SocialSecurityNumber" unique="true" multiplicity="ZeroToOne" thisRoleName="Person" thisRoleRef="_AAE038F3-BF91-4319-854E-792C7041AE36" oppositeRoleRef="_512DF478-3B6D-4944-8B9A-1A95C427BBDF" oppositeRoleName="SocialSecurityNumber" mandatory="false" /><ao:AbsorbedObject type="ValueType" ref="_551FCEDA-66D0-49D3-8D87-B1C551D70A8F" name="NickName" unique="false" multiplicity="ZeroToMany" thisRoleName="Person" thisRoleRef="_2A32CF98-5EC6-470F-999D-BF8B65CB9B64" oppositeRoleRef="_6FA75148-1A54-4675-BD51-E8C95E37DD51" oppositeRoleName="NickName" mandatory="false" /><ao:AbsorbedObject type="ValueType" ref="_D4808010-4C18-442E-A5E4-B69A33A5FA88" name="ValueType1" unique="true" multiplicity="ZeroToMany" thisRoleName="Person" thisRoleRef="_D2285B0E-026A-4CE5-A4EF-A9914213ED03" oppositeRoleRef="_92066292-C1C5-4063-9652-E071F922267B" oppositeRoleName="ValueType1" mandatory="false" /><ao:AbsorbedObject type="EntityType" ref="_CFFA4225-A7B4-457F-818B-4D899AA157AB" name="HatType" unique="false" multiplicity="ZeroToOne" thisRoleName="Person" thisRoleRef="_AFD8D46E-1189-4B92-A7D3-3EAAC57AD629" oppositeRoleRef="_9D83D21B-1F76-4368-A70B-EC1932BB4C42" oppositeRoleName="HatType" mandatory="false"><ao:AbsorbedObject type="ValueType" ref="_CEF8D63C-7F55-4227-8C84-84B1BABF76E7" name="HatType_name" unique="true" multiplicity="ExactlyOne" thisRoleName="HatType" thisRoleRef="_B392D051-FF00-401F-BC87-BE9E59768999" oppositeRoleRef="_607260C5-A9AF-4D26-81BE-C06C241E2AA4" oppositeRoleName="HatType_name" mandatory="true" /></ao:AbsorbedObject><ao:RelatedObject factRef="_8B053DEF-BA79-4A8F-804F-5561308F69B3" roleRef="_9F1F7C52-3E4E-4161-ACB9-E417F8D293A6" arity="2" roleName="Wife" mandatory="false" unique="true" multiplicity="ZeroToOne" oppositeRoleRef="_089D0A43-FD48-4580-A6A6-45A6FA189DEA" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Husband" /><ao:RelatedObject factRef="_8B053DEF-BA79-4A8F-804F-5561308F69B3" roleRef="_089D0A43-FD48-4580-A6A6-45A6FA189DEA" arity="2" roleName="Husband" mandatory="false" unique="true" multiplicity="ZeroToOne" oppositeRoleRef="_9F1F7C52-3E4E-4161-ACB9-E417F8D293A6" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Wife" /><ao:RelatedObject factRef="_F88AF1BA-35E2-4235-9C8B-0C0A4647707D" roleRef="_F6390EAB-A049-4C16-8FB4-50BE44A44364" arity="2" roleName="Person" mandatory="false" unique="true" multiplicity="ZeroToMany" oppositeRoleRef="_FB7333FB-CE11-4A31-B7DE-6F20958A525A" oppositeObjectRef="_779E9788-0C2F-44B6-B958-AEEC22593966" oppositeObjectName="Task" oppositeRoleName="Task" /><ao:RelatedObject factRef="_C0918686-ECCA-4775-BE23-1304918E4EF5" roleRef="_0ABD30A1-BF17-4110-9340-3765A1C76BCA" arity="2" roleName="DrivenByPerson" mandatory="false" unique="false" multiplicity="ZeroToMany" oppositeRoleRef="_18AE032D-6C6B-46C4-9383-D3CB9387FA60" oppositeObjectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" oppositeObjectName="Car" oppositeRoleName="DrivesCar" /><ao:RelatedObject factRef="_78B61C80-E4BE-460B-9714-AFB5F6E88B1E" roleRef="_ED495E3B-6C47-4D22-B01C-0E0C805C6664" arity="2" roleName="OwnedByPerson" mandatory="false" unique="true" multiplicity="ZeroToOne" oppositeRoleRef="_94205691-F9E1-47BD-A2D4-B2698FB19A25" oppositeObjectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" oppositeObjectName="Car" oppositeRoleName="OwnsCar" /><ao:RelatedObject factRef="_5A20BCB8-D3B9-4BD4-9A33-ADE0D7B8062B" roleRef="_310FF4C1-DA3B-4C8D-8892-C43F6452245C" arity="2" roleName="Person" mandatory="true" unique="false" multiplicity="ExactlyOne" oppositeRoleRef="_D5C1A5F1-D376-4606-A991-4A3F55206DB4" oppositeObjectRef="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" oppositeObjectName="Date" oppositeRoleName="Date" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_9496C251-F06F-4383-93FB-F9BF04A523CB" arity="4" roleName="Buyer" mandatory="false" unique="true" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_7C8166A7-F037-493E-BFB7-2A1634AB2238" arity="4" roleName="Seller" mandatory="false" unique="true" /></ao:Object><ao:Object type="EntityType" id="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" name="Date"><ao:AbsorbedObject type="ValueType" ref="_F72BE752-7E12-40F3-9B26-BF303B5E2A50" name="ymd" unique="true" multiplicity="ExactlyOne" thisRoleName="Date" thisRoleRef="_0F3F2BC8-D785-4E4C-B953-A165DAAAFE83" oppositeRoleRef="_D7E26580-4FE3-4D4F-B088-A31B40EB44C8" oppositeRoleName="ymd" mandatory="true" /><ao:RelatedObject factRef="_5A20BCB8-D3B9-4BD4-9A33-ADE0D7B8062B" roleRef="_D5C1A5F1-D376-4606-A991-4A3F55206DB4" arity="2" roleName="Date" mandatory="false" unique="true" multiplicity="ZeroToMany" oppositeRoleRef="_310FF4C1-DA3B-4C8D-8892-C43F6452245C" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_1657F414-1F65-41C5-935F-FACC31E02630" arity="4" roleName="SaleDate" mandatory="false" unique="true" /></ao:Object><ao:Object type="EntityType" id="_779E9788-0C2F-44B6-B958-AEEC22593966" name="Task"><ao:AbsorbedObject type="ValueType" ref="_388FC80E-A3B9-4AF0-A282-107C93DF5988" name="Task_id" unique="true" multiplicity="ExactlyOne" thisRoleName="Task" thisRoleRef="_D38BF87C-33BA-4BFC-9D23-32F52F333AB6" oppositeRoleRef="_66E039F6-38D1-445D-A3E0-02277CA0907E" oppositeRoleName="Task_id" mandatory="true" /><ao:RelatedObject factRef="_F88AF1BA-35E2-4235-9C8B-0C0A4647707D" roleRef="_FB7333FB-CE11-4A31-B7DE-6F20958A525A" arity="2" roleName="Task" mandatory="false" unique="false" multiplicity="ZeroToOne" oppositeRoleRef="_F6390EAB-A049-4C16-8FB4-50BE44A44364" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="Person" /></ao:Object><ao:Object type="EntityType" id="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" name="Car"><ao:AbsorbedObject type="ValueType" ref="_AA4AFE18-F3AF-40FB-848F-FD6A3A3F3F1A" name="vin" unique="true" multiplicity="ExactlyOne" thisRoleName="Car" thisRoleRef="_24FBA18D-9625-4116-86FE-F397E04E9BD2" oppositeRoleRef="_BBBCEBD0-2688-4A0B-860D-2F8BD5096DDE" oppositeRoleName="vin" mandatory="true" /><ao:RelatedObject factRef="_C0918686-ECCA-4775-BE23-1304918E4EF5" roleRef="_18AE032D-6C6B-46C4-9383-D3CB9387FA60" arity="2" roleName="DrivesCar" mandatory="false" unique="false" multiplicity="ZeroToMany" oppositeRoleRef="_0ABD30A1-BF17-4110-9340-3765A1C76BCA" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="DrivenByPerson" /><ao:RelatedObject factRef="_78B61C80-E4BE-460B-9714-AFB5F6E88B1E" roleRef="_94205691-F9E1-47BD-A2D4-B2698FB19A25" arity="2" roleName="OwnsCar" mandatory="false" unique="true" multiplicity="ZeroToOne" oppositeRoleRef="_ED495E3B-6C47-4D22-B01C-0E0C805C6664" oppositeObjectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" oppositeObjectName="Person" oppositeRoleName="OwnedByPerson" /><ao:RelatedObject factRef="_06C709AC-304C-491C-8846-D2A1630F51F2" roleRef="_C0203916-6D57-4C48-9151-B36F507D9D18" arity="4" roleName="CarSold" mandatory="false" unique="true" /><ao:RelatedObject factRef="_780A6DD3-CD65-4533-8870-7DE873EBAEF5" roleRef="_0BC3478C-9D88-4C4B-9754-BB62DCC7F189" arity="3" roleName="Car" mandatory="false" unique="true" /></ao:Object><ao:Association name="PersonBoughtCarFromPersonOnDate" id="_06C709AC-304C-491C-8846-D2A1630F51F2"><ao:RelatedObject type="EntityType" objectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" roleRef="_9496C251-F06F-4383-93FB-F9BF04A523CB" className="Person" unique="false" roleName="Buyer" /><ao:RelatedObject type="EntityType" objectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" roleRef="_C0203916-6D57-4C48-9151-B36F507D9D18" className="Car" unique="false" roleName="CarSold" /><ao:RelatedObject type="EntityType" objectRef="_97C09FEE-69B9-429A-9A7C-9C218CB295BB" roleRef="_7C8166A7-F037-493E-BFB7-2A1634AB2238" className="Person" unique="false" roleName="Seller" /><ao:RelatedObject type="EntityType" objectRef="_CB7A017C-1E94-473C-B4B1-CBD710DE4B7A" roleRef="_1657F414-1F65-41C5-935F-FACC31E02630" className="Date" unique="false" roleName="SaleDate" /></ao:Association><ao:Association name="Review" id="_780A6DD3-CD65-4533-8870-7DE873EBAEF5"><ao:RelatedObject type="EntityType" objectRef="_A6617DBB-19D8-4FCB-A14A-2F237E0D8904" roleRef="_0BC3478C-9D88-4C4B-9754-BB62DCC7F189" className="Car" unique="false" roleName="Car" /><ao:AbsorbedObject type="EntityType" ref="_65DBE376-74EA-40A8-8753-8A8CA24259C8" name="Rating" roleName="" thisRoleRef="_15617B90-1712-4B85-9506-2007DE9EEEC0"><ao:AbsorbedObject type="EntityType" ref="_437E0803-738F-4314-AD87-C2492305DF10" name="Nr" unique="true" multiplicity="ExactlyOne" thisRoleName="Rating" thisRoleRef="_DEE46F2B-9ED6-477C-A6C2-9E37A44E6263" oppositeRoleRef="_EB80D766-ED3A-4511-B5C5-F8E7CAB99B8A" oppositeRoleName="Nr" mandatory="true"><ao:AbsorbedObject type="ValueType" ref="_2080B74C-CA56-4349-B83B-786794656BA7" name="Integer" unique="true" multiplicity="ExactlyOne" thisRoleName="Nr" thisRoleRef="_603F4EE0-C3B9-4CB9-918F-1D7AC2224BD9" oppositeRoleRef="_AB846AA8-64FD-4EB2-920F-6F8C1D4F16FB" oppositeRoleName="Integer" mandatory="true" /></ao:AbsorbedObject></ao:AbsorbedObject><ao:AbsorbedObject type="EntityType" ref="_C93BB2C9-7F70-49DE-8295-07997960D5F3" name="Criteria" roleName="" thisRoleRef="_B8FCF342-1134-4FE2-8B10-5DF944662B07"><ao:AbsorbedObject type="ValueType" ref="_1232809E-4674-40C8-8E55-BDA203ED9FCE" name="Name" unique="true" multiplicity="ExactlyOne" thisRoleName="Criteria" thisRoleRef="_1AE48A9B-5F7B-4AFF-81B6-595607EBB940" oppositeRoleRef="_F323AB9E-5B2E-4434-A3D0-321B838DE939" oppositeRoleName="Name" mandatory="true" /></ao:AbsorbedObject></ao:Association>

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
			private readonly System.Delegate[] Events = new System.Delegate[10];
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
			public abstract ICollection<string> NickName
			{
				get;
			}
			public abstract ICollection<int> ValueType1
			{
				get;
			}
			public abstract string HatType
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
			public abstract ICollection<Task> Task
			{
				get;
			}
			public abstract ICollection<Car> DrivesCar
			{
				get;
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
			public event EventHandler<PropertyChangingEventArgs<string>> HatTypeChanging
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
			protected bool RaiseHatTypeChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.HatType, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> HatTypeChanged
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
			protected void RaiseHatTypeChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.HatType), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatType");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> HusbandChanging
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
			protected bool RaiseHusbandChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[6] as EventHandler<PropertyChangingEventArgs<Person>>;
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
					this.Events[6] = System.Delegate.Combine(this.Events[6], value);
				}
				remove
				{
					this.Events[6] = System.Delegate.Remove(this.Events[6], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseHusbandChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<Person>>;
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
					this.Events[7] = System.Delegate.Combine(this.Events[7], value);
				}
				remove
				{
					this.Events[7] = System.Delegate.Remove(this.Events[7], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseWifeChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<Person>>;
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
					this.Events[7] = System.Delegate.Combine(this.Events[7], value);
				}
				remove
				{
					this.Events[7] = System.Delegate.Remove(this.Events[7], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseWifeChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[7] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Wife), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Wife");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Car>> OwnsCarChanging
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
			protected bool RaiseOwnsCarChangingEvent(Car newValue)
			{
				EventHandler<PropertyChangingEventArgs<Car>> eventHandler = this.Events[8] as EventHandler<PropertyChangingEventArgs<Car>>;
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
					this.Events[8] = System.Delegate.Combine(this.Events[8], value);
				}
				remove
				{
					this.Events[8] = System.Delegate.Remove(this.Events[8], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseOwnsCarChangedEvent(Car oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Car>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<Car>>;
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
					this.Events[9] = System.Delegate.Combine(this.Events[9], value);
				}
				remove
				{
					this.Events[9] = System.Delegate.Remove(this.Events[9], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDateChangingEvent(Date newValue)
			{
				EventHandler<PropertyChangingEventArgs<Date>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<Date>>;
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
					this.Events[9] = System.Delegate.Combine(this.Events[9], value);
				}
				remove
				{
					this.Events[9] = System.Delegate.Remove(this.Events[9], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDateChangedEvent(Date oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Date>> eventHandler = this.Events[9] as EventHandler<PropertyChangedEventArgs<Date>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Date>(oldValue, this.Date), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date");
				}
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Person{0}{{{0}{1}Person_id = ""{2}"",{0}{1}FirstName = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}SocialSecurityNumber = ""{5}"",{0}{1}HatType = ""{6}"",{0}{1}Husband = {7},{0}{1}Wife = {8},{0}{1}OwnsCar = {9},{0}{1}Date = {10}{0}}}", Environment.NewLine, "	", this.Person_id, this.FirstName, this.LastName, this.SocialSecurityNumber, this.HatType, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Person
		#region Date
		public abstract partial class Date : INotifyPropertyChanged
		{
			protected Date()
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
			public abstract System.DateTime ymd
			{
				get;
				set;
			}
			public abstract ICollection<Person> Person
			{
				get;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAsSaleDate
			{
				get;
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
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Date{0}{{{0}{1}ymd = ""{2}""{0}}}", Environment.NewLine, "	", this.ymd);
			}
		}
		#endregion // Date
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
			public abstract uint vin
			{
				get;
				set;
			}
			public abstract ICollection<Person> DrivenByPerson
			{
				get;
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
			public event EventHandler<PropertyChangingEventArgs<Person>> OwnedByPersonChanging
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
			protected bool RaiseOwnedByPersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
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
					this.Events[2] = System.Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = System.Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseOwnedByPersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
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
				return string.Format(provider, @"Car{0}{{{0}{1}vin = ""{2}"",{0}{1}OwnedByPerson = {3}{0}}}", Environment.NewLine, "	", this.vin, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Car
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
			PersonBoughtCarFromPersonOnDateAssociation GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint1(Person Buyer, Car CarSold, Person Seller);
			PersonBoughtCarFromPersonOnDateAssociation GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint2(Date SaleDate, Person Seller, Car CarSold);
			PersonBoughtCarFromPersonOnDateAssociation GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint3(Car CarSold, Date SaleDate, Person Buyer);
			ReviewAssociation GetReviewAssociationByInternalUniquenessConstraint4(Car Car, string CriteriaName);
			Person CreatePerson(int Person_id, string FirstName, string LastName, Date Date);
			ReadOnlyCollection<Person> PersonCollection
			{
				get;
			}
			Person GetPersonByPerson_id(int value);
			Person GetPersonBySocialSecurityNumber(string value);
			Person GetPersonByValueType1(int value);
			Date CreateDate(System.DateTime ymd);
			ReadOnlyCollection<Date> DateCollection
			{
				get;
			}
			Date GetDateByymd(System.DateTime value);
			Task CreateTask(int Task_id);
			ReadOnlyCollection<Task> TaskCollection
			{
				get;
			}
			Task GetTaskByTask_id(int value);
			Car CreateCar(uint vin);
			ReadOnlyCollection<Car> CarCollection
			{
				get;
			}
			Car GetCarByvin(uint value);
			PersonBoughtCarFromPersonOnDateAssociation CreatePersonBoughtCarFromPersonOnDateAssociation(Person Buyer, Car CarSold, Person Seller, Date SaleDate);
			ReadOnlyCollection<PersonBoughtCarFromPersonOnDateAssociation> PersonBoughtCarFromPersonOnDateAssociationCollection
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
			Person CreatePerson(int Person_id, string FirstName, string LastName);
			Date CreateDate(System.DateTime ymd);
			Task CreateTask(int Task_id);
			Car CreateCar(uint vin);
			PersonBoughtCarFromPersonOnDateAssociation CreatePersonBoughtCarFromPersonOnDateAssociation();
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
			private Dictionary<Tuple<Person, Car, Person>, PersonBoughtCarFromPersonOnDateAssociation> myInternalUniquenessConstraint1Dictionary = new Dictionary<Tuple<Person, Car, Person>, PersonBoughtCarFromPersonOnDateAssociation>();
			private bool OnInternalUniquenessConstraint1Changing(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Person, Car, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDateAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint1Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint1Changed(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Person, Car, Person> oldValue, Tuple<Person, Car, Person> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint1Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint1Dictionary.Add(newValue, instance);
				}
			}
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint1(Person Buyer, Car CarSold, Person Seller)
			{
				return this.myInternalUniquenessConstraint1Dictionary[Tuple.CreateTuple(Buyer, CarSold, Seller)];
			}
			private Dictionary<Tuple<Date, Person, Car>, PersonBoughtCarFromPersonOnDateAssociation> myInternalUniquenessConstraint2Dictionary = new Dictionary<Tuple<Date, Person, Car>, PersonBoughtCarFromPersonOnDateAssociation>();
			private bool OnInternalUniquenessConstraint2Changing(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Date, Person, Car> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDateAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint2Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint2Changed(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Date, Person, Car> oldValue, Tuple<Date, Person, Car> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint2Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint2Dictionary.Add(newValue, instance);
				}
			}
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint2(Date SaleDate, Person Seller, Car CarSold)
			{
				return this.myInternalUniquenessConstraint2Dictionary[Tuple.CreateTuple(SaleDate, Seller, CarSold)];
			}
			private Dictionary<Tuple<Car, Date, Person>, PersonBoughtCarFromPersonOnDateAssociation> myInternalUniquenessConstraint3Dictionary = new Dictionary<Tuple<Car, Date, Person>, PersonBoughtCarFromPersonOnDateAssociation>();
			private bool OnInternalUniquenessConstraint3Changing(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Car, Date, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDateAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint3Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint3Changed(PersonBoughtCarFromPersonOnDateAssociation instance, Tuple<Car, Date, Person> oldValue, Tuple<Car, Date, Person> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint3Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint3Dictionary.Add(newValue, instance);
				}
			}
			PersonBoughtCarFromPersonOnDateAssociation ISampleModelContext.GetPersonBoughtCarFromPersonOnDateAssociationByInternalUniquenessConstraint3(Car CarSold, Date SaleDate, Person Buyer)
			{
				return this.myInternalUniquenessConstraint3Dictionary[Tuple.CreateTuple(CarSold, SaleDate, Buyer)];
			}
			private Dictionary<Tuple<Car, string>, ReviewAssociation> myInternalUniquenessConstraint4Dictionary = new Dictionary<Tuple<Car, string>, ReviewAssociation>();
			private bool OnInternalUniquenessConstraint4Changing(ReviewAssociation instance, Tuple<Car, string> newValue)
			{
				if (newValue != null)
				{
					ReviewAssociation currentInstance = instance;
					if (this.myInternalUniquenessConstraint4Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint4Changed(ReviewAssociation instance, Tuple<Car, string> oldValue, Tuple<Car, string> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint4Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint4Dictionary.Add(newValue, instance);
				}
			}
			ReviewAssociation ISampleModelContext.GetReviewAssociationByInternalUniquenessConstraint4(Car Car, string CriteriaName)
			{
				return this.myInternalUniquenessConstraint4Dictionary[Tuple.CreateTuple(Car, CriteriaName)];
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
			Person ISampleModelContext.GetPersonByPerson_id(int value)
			{
				return this.myPersonPerson_idDictionary[value];
			}
			private Dictionary<int, Person> myPersonPerson_idDictionary = new Dictionary<int, Person>();
			private bool OnPersonPerson_idChanging(Person instance, int newValue, bool throwOnFailure)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this.myPersonPerson_idDictionary.TryGetValue(newValue, out currentInstance))
					{
						if (!(object.Equals(currentInstance, instance)))
						{
							return false;
						}
					}
				}
				#pragma warning restore 0472
				return true;
			}
			private void OnPersonPerson_idChanged(Person instance, int oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myPersonPerson_idDictionary.Remove(oldValue);
				}
				if (instance.Person_id != null)
				{
					this.myPersonPerson_idDictionary.Add(instance.Person_id, instance);
				}
				#pragma warning restore 0472
			}
			private void OnPersonPerson_idChanged(Person instance)
			{
				#pragma warning disable 0472
				if (instance.Person_id != null)
				{
					this.myPersonPerson_idDictionary.Add(instance.Person_id, instance);
				}
				#pragma warning restore 0472
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
			private void OnPersonFirstNameChanged(Person instance)
			{
				this.OnExternalUniquenessConstraint1Changed(instance, null, Tuple.CreateTuple(instance.FirstName, instance.Date));
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
			private void OnPersonLastNameChanged(Person instance)
			{
				this.OnExternalUniquenessConstraint2Changed(instance, null, Tuple.CreateTuple(instance.LastName, instance.Date));
			}
			Person ISampleModelContext.GetPersonBySocialSecurityNumber(string value)
			{
				return this.myPersonSocialSecurityNumberDictionary[value];
			}
			private Dictionary<string, Person> myPersonSocialSecurityNumberDictionary = new Dictionary<string, Person>();
			private bool OnPersonSocialSecurityNumberChanging(Person instance, string newValue, bool throwOnFailure)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this.myPersonSocialSecurityNumberDictionary.TryGetValue(newValue, out currentInstance))
					{
						if (!(object.Equals(currentInstance, instance)))
						{
							return false;
						}
					}
				}
				#pragma warning restore 0472
				return true;
			}
			private void OnPersonSocialSecurityNumberChanged(Person instance, string oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myPersonSocialSecurityNumberDictionary.Remove(oldValue);
				}
				if (instance.SocialSecurityNumber != null)
				{
					this.myPersonSocialSecurityNumberDictionary.Add(instance.SocialSecurityNumber, instance);
				}
				#pragma warning restore 0472
			}
			private bool OnPersonNickNameAdding(Person instance, string value)
			{
				return true;
			}
			private void OnPersonNickNameAdded(Person instance, string value)
			{
			}
			private bool OnPersonNickNameRemoving(Person instance, string value)
			{
				return true;
			}
			private void OnPersonNickNameRemoved(Person instance, string value)
			{
			}
			Person ISampleModelContext.GetPersonByValueType1(int value)
			{
				return this.myPersonValueType1Dictionary[value];
			}
			private Dictionary<int, Person> myPersonValueType1Dictionary = new Dictionary<int, Person>();
			private bool OnPersonValueType1Adding(Person instance, int value)
			{
				return true;
			}
			private void OnPersonValueType1Added(Person instance, int value)
			{
				#pragma warning disable 0472
				if (value != null)
				{
					this.myPersonValueType1Dictionary.Add(value, instance);
				}
				#pragma warning restore 0472
			}
			private bool OnPersonValueType1Removing(Person instance, int value)
			{
				return true;
			}
			private void OnPersonValueType1Removed(Person instance, int value)
			{
				#pragma warning disable 0472
				if (value != null)
				{
					this.myPersonValueType1Dictionary.Remove(value);
				}
				#pragma warning restore 0472
			}
			private bool OnPersonHatTypeChanging(Person instance, string newValue, bool throwOnFailure)
			{
				return true;
			}
			private void OnPersonHatTypeChanged(Person instance, string oldValue)
			{
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
					oldValue.Wife = null;
				}
				if (instance.Husband != null)
				{
					instance.Husband.Wife = instance;
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
					oldValue.Husband = null;
				}
				if (instance.Wife != null)
				{
					instance.Wife.Husband = instance;
				}
			}
			private bool OnPersonTaskAdding(Person instance, Task value)
			{
				return true;
			}
			private void OnPersonTaskAdded(Person instance, Task value)
			{
				if (value != null)
				{
					value.Person = instance;
				}
			}
			private bool OnPersonTaskRemoving(Person instance, Task value)
			{
				return true;
			}
			private void OnPersonTaskRemoved(Person instance, Task value)
			{
				if (value != null)
				{
					value.Person = null;
				}
			}
			private bool OnPersonDrivesCarAdding(Person instance, Car value)
			{
				return true;
			}
			private void OnPersonDrivesCarAdded(Person instance, Car value)
			{
				if (value != null)
				{
					value.DrivenByPerson.Add(instance);
				}
			}
			private bool OnPersonDrivesCarRemoving(Person instance, Car value)
			{
				return true;
			}
			private void OnPersonDrivesCarRemoved(Person instance, Car value)
			{
				if (value != null)
				{
					value.DrivenByPerson.Remove(instance);
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
					oldValue.OwnedByPerson = null;
				}
				if (instance.OwnsCar != null)
				{
					instance.OwnsCar.OwnedByPerson = instance;
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
			private void OnPersonDateChanged(Person instance)
			{
				if (instance.Date != null)
				{
					instance.Date.Person.Add(instance);
				}
				this.OnExternalUniquenessConstraint1Changed(instance, null, Tuple.CreateTuple(instance.FirstName, instance.Date));
				this.OnExternalUniquenessConstraint2Changed(instance, null, Tuple.CreateTuple(instance.LastName, instance.Date));
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Person ISampleModelContext.CreatePerson(int Person_id, string FirstName, string LastName, Date Date)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnPersonPerson_idChanging(null, Person_id, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Person_id");
				}
				if (!(this.OnPersonFirstNameChanging(null, FirstName, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "FirstName");
				}
				if (!(this.OnPersonLastNameChanging(null, LastName, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "LastName");
				}
				if (!(this.OnPersonDateChanging(null, Date, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Date");
				}
				return new PersonCore(this, Person_id, FirstName, LastName, Date);
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
				public PersonCore(SampleModelContext context, int Person_id, string FirstName, string LastName, Date Date)
				{
					this.myContext = context;
					this.myPerson_id = Person_id;
					this.Context.OnPersonPerson_idChanged(this);
					this.myFirstName = FirstName;
					this.Context.OnPersonFirstNameChanged(this);
					this.myLastName = LastName;
					this.Context.OnPersonLastNameChanged(this);
					this.myDate = Date;
					this.Context.OnPersonDateChanged(this);
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
				private sealed class NickNameCollection : ICollection<string>
				{
					private Person myPerson;
					private List<string> myList = new List<string>();
					public NickNameCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<string> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(string item)
					{
						if (this.myPerson.Context.OnPersonNickNameAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonNickNameAdded(this.myPerson, item);
						}
					}
					public bool Remove(string item)
					{
						if (this.myPerson.Context.OnPersonNickNameRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonNickNameRemoved(this.myPerson, item);
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
					public bool Contains(string item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(string[] array, int arrayIndex)
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
				private ICollection<string> myNickName;
				public override ICollection<string> NickName
				{
					get
					{
						if (this.myNickName == null)
						{
							this.myNickName = new NickNameCollection(this);
						}
						return this.myNickName;
					}
				}
				private sealed class ValueType1Collection : ICollection<int>
				{
					private Person myPerson;
					private List<int> myList = new List<int>();
					public ValueType1Collection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<int> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(int item)
					{
						if (this.myPerson.Context.OnPersonValueType1Adding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonValueType1Added(this.myPerson, item);
						}
					}
					public bool Remove(int item)
					{
						if (this.myPerson.Context.OnPersonValueType1Removing(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonValueType1Removed(this.myPerson, item);
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
					public bool Contains(int item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(int[] array, int arrayIndex)
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
				private ICollection<int> myValueType1;
				public override ICollection<int> ValueType1
				{
					get
					{
						if (this.myValueType1 == null)
						{
							this.myValueType1 = new ValueType1Collection(this);
						}
						return this.myValueType1;
					}
				}
				private string myHatType;
				public override string HatType
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
									string oldValue = this.HatType;
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
				private sealed class TaskCollection : ICollection<Task>
				{
					private Person myPerson;
					private List<Task> myList = new List<Task>();
					public TaskCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<Task> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(Task item)
					{
						if (this.myPerson.Context.OnPersonTaskAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonTaskAdded(this.myPerson, item);
						}
					}
					public bool Remove(Task item)
					{
						if (this.myPerson.Context.OnPersonTaskRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonTaskRemoved(this.myPerson, item);
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
					public bool Contains(Task item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(Task[] array, int arrayIndex)
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
				private ICollection<Task> myTask;
				public override ICollection<Task> Task
				{
					get
					{
						if (this.myTask == null)
						{
							this.myTask = new TaskCollection(this);
						}
						return this.myTask;
					}
				}
				private sealed class DrivesCarCollection : ICollection<Car>
				{
					private Person myPerson;
					private List<Car> myList = new List<Car>();
					public DrivesCarCollection(Person instance)
					{
						this.myPerson = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<Car> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(Car item)
					{
						if (this.myPerson.Context.OnPersonDrivesCarAdding(this.myPerson, item))
						{
							this.myList.Add(item);
							this.myPerson.Context.OnPersonDrivesCarAdded(this.myPerson, item);
						}
					}
					public bool Remove(Car item)
					{
						if (this.myPerson.Context.OnPersonDrivesCarRemoving(this.myPerson, item))
						{
							if (this.myList.Remove(item))
							{
								this.myPerson.Context.OnPersonDrivesCarRemoved(this.myPerson, item);
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
					public bool Contains(Car item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(Car[] array, int arrayIndex)
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
				private ICollection<Car> myDrivesCar;
				public override ICollection<Car> DrivesCar
				{
					get
					{
						if (this.myDrivesCar == null)
						{
							this.myDrivesCar = new DrivesCarCollection(this);
						}
						return this.myDrivesCar;
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
			}
			#endregion // Person
			Date ISampleModelContext.GetDateByymd(System.DateTime value)
			{
				return this.myDateymdDictionary[value];
			}
			private Dictionary<System.DateTime, Date> myDateymdDictionary = new Dictionary<System.DateTime, Date>();
			private bool OnDateymdChanging(Date instance, System.DateTime newValue, bool throwOnFailure)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					Date currentInstance = instance;
					if (this.myDateymdDictionary.TryGetValue(newValue, out currentInstance))
					{
						if (!(object.Equals(currentInstance, instance)))
						{
							return false;
						}
					}
				}
				#pragma warning restore 0472
				return true;
			}
			private void OnDateymdChanged(Date instance, System.DateTime oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myDateymdDictionary.Remove(oldValue);
				}
				if (instance.ymd != null)
				{
					this.myDateymdDictionary.Add(instance.ymd, instance);
				}
				#pragma warning restore 0472
			}
			private void OnDateymdChanged(Date instance)
			{
				#pragma warning disable 0472
				if (instance.ymd != null)
				{
					this.myDateymdDictionary.Add(instance.ymd, instance);
				}
				#pragma warning restore 0472
			}
			private bool OnDatePersonAdding(Date instance, Person value)
			{
				return true;
			}
			private void OnDatePersonAdded(Date instance, Person value)
			{
				if (value != null)
				{
					value.Date = instance;
				}
			}
			private bool OnDatePersonRemoving(Date instance, Person value)
			{
				return true;
			}
			private void OnDatePersonRemoved(Date instance, Person value)
			{
				if (value != null)
				{
					value.Date = null;
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			Date ISampleModelContext.CreateDate(System.DateTime ymd)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnDateymdChanging(null, ymd, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "ymd");
				}
				return new DateCore(this, ymd);
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
				public DateCore(SampleModelContext context, System.DateTime ymd)
				{
					this.myContext = context;
					this.myymd = ymd;
					this.Context.OnDateymdChanged(this);
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
				private sealed class PersonCollection : ICollection<Person>
				{
					private Date myDate;
					private List<Person> myList = new List<Person>();
					public PersonCollection(Date instance)
					{
						this.myDate = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<Person> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(Person item)
					{
						if (this.myDate.Context.OnDatePersonAdding(this.myDate, item))
						{
							this.myList.Add(item);
							this.myDate.Context.OnDatePersonAdded(this.myDate, item);
						}
					}
					public bool Remove(Person item)
					{
						if (this.myDate.Context.OnDatePersonRemoving(this.myDate, item))
						{
							if (this.myList.Remove(item))
							{
								this.myDate.Context.OnDatePersonRemoved(this.myDate, item);
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
					public bool Contains(Person item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(Person[] array, int arrayIndex)
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
				private ICollection<Person> myPerson;
				public override ICollection<Person> Person
				{
					get
					{
						if (this.myPerson == null)
						{
							this.myPerson = new PersonCollection(this);
						}
						return this.myPerson;
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
			}
			#endregion // Date
			Task ISampleModelContext.GetTaskByTask_id(int value)
			{
				return this.myTaskTask_idDictionary[value];
			}
			private Dictionary<int, Task> myTaskTask_idDictionary = new Dictionary<int, Task>();
			private bool OnTaskTask_idChanging(Task instance, int newValue, bool throwOnFailure)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					Task currentInstance = instance;
					if (this.myTaskTask_idDictionary.TryGetValue(newValue, out currentInstance))
					{
						if (!(object.Equals(currentInstance, instance)))
						{
							return false;
						}
					}
				}
				#pragma warning restore 0472
				return true;
			}
			private void OnTaskTask_idChanged(Task instance, int oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myTaskTask_idDictionary.Remove(oldValue);
				}
				if (instance.Task_id != null)
				{
					this.myTaskTask_idDictionary.Add(instance.Task_id, instance);
				}
				#pragma warning restore 0472
			}
			private void OnTaskTask_idChanged(Task instance)
			{
				#pragma warning disable 0472
				if (instance.Task_id != null)
				{
					this.myTaskTask_idDictionary.Add(instance.Task_id, instance);
				}
				#pragma warning restore 0472
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
			Task ISampleModelContext.CreateTask(int Task_id)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnTaskTask_idChanging(null, Task_id, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "Task_id");
				}
				return new TaskCore(this, Task_id);
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
				public TaskCore(SampleModelContext context, int Task_id)
				{
					this.myContext = context;
					this.myTask_id = Task_id;
					this.Context.OnTaskTask_idChanged(this);
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
			Car ISampleModelContext.GetCarByvin(uint value)
			{
				return this.myCarvinDictionary[value];
			}
			private Dictionary<uint, Car> myCarvinDictionary = new Dictionary<uint, Car>();
			private bool OnCarvinChanging(Car instance, uint newValue, bool throwOnFailure)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					Car currentInstance = instance;
					if (this.myCarvinDictionary.TryGetValue(newValue, out currentInstance))
					{
						if (!(object.Equals(currentInstance, instance)))
						{
							return false;
						}
					}
				}
				#pragma warning restore 0472
				return true;
			}
			private void OnCarvinChanged(Car instance, uint oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myCarvinDictionary.Remove(oldValue);
				}
				if (instance.vin != null)
				{
					this.myCarvinDictionary.Add(instance.vin, instance);
				}
				#pragma warning restore 0472
			}
			private void OnCarvinChanged(Car instance)
			{
				#pragma warning disable 0472
				if (instance.vin != null)
				{
					this.myCarvinDictionary.Add(instance.vin, instance);
				}
				#pragma warning restore 0472
			}
			private bool OnCarDrivenByPersonAdding(Car instance, Person value)
			{
				return true;
			}
			private void OnCarDrivenByPersonAdded(Car instance, Person value)
			{
				if (value != null)
				{
					value.DrivesCar.Add(instance);
				}
			}
			private bool OnCarDrivenByPersonRemoving(Car instance, Person value)
			{
				return true;
			}
			private void OnCarDrivenByPersonRemoved(Car instance, Person value)
			{
				if (value != null)
				{
					value.DrivesCar.Remove(instance);
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
					oldValue.OwnsCar = null;
				}
				if (instance.OwnedByPerson != null)
				{
					instance.OwnedByPerson.OwnsCar = instance;
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
			Car ISampleModelContext.CreateCar(uint vin)
			{
				if ((this as ISampleModelContext).IsDeserializing)
				{
					throw new InvalidOperationException("This factory method cannot be called while IsDeserializing returns true.");
				}
				if (!(this.OnCarvinChanging(null, vin, true)))
				{
					throw new System.ArgumentException("An argument failed constraint enforcement.", "vin");
				}
				return new CarCore(this, vin);
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
				public CarCore(SampleModelContext context, uint vin)
				{
					this.myContext = context;
					this.myvin = vin;
					this.Context.OnCarvinChanged(this);
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
				private sealed class DrivenByPersonCollection : ICollection<Person>
				{
					private Car myCar;
					private List<Person> myList = new List<Person>();
					public DrivenByPersonCollection(Car instance)
					{
						this.myCar = instance;
					}
					IEnumerator IEnumerable.GetEnumerator()
					{
						return this.GetEnumerator();
					}
					public IEnumerator<Person> GetEnumerator()
					{
						return this.myList.GetEnumerator();
					}
					public void Add(Person item)
					{
						if (this.myCar.Context.OnCarDrivenByPersonAdding(this.myCar, item))
						{
							this.myList.Add(item);
							this.myCar.Context.OnCarDrivenByPersonAdded(this.myCar, item);
						}
					}
					public bool Remove(Person item)
					{
						if (this.myCar.Context.OnCarDrivenByPersonRemoving(this.myCar, item))
						{
							if (this.myList.Remove(item))
							{
								this.myCar.Context.OnCarDrivenByPersonRemoved(this.myCar, item);
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
					public bool Contains(Person item)
					{
						return this.myList.Contains(item);
					}
					public void CopyTo(Person[] array, int arrayIndex)
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
				private ICollection<Person> myDrivenByPerson;
				public override ICollection<Person> DrivenByPerson
				{
					get
					{
						if (this.myDrivenByPerson == null)
						{
							this.myDrivenByPerson = new DrivenByPersonCollection(this);
						}
						return this.myDrivenByPerson;
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
				if (!(this.OnInternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(newValue, instance.CarSold, instance.Seller))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, newValue))))
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
				this.OnInternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(oldValue, instance.CarSold, instance.Seller), Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint3Changed(instance, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, oldValue), Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationBuyerChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.Buyer != null)
				{
					instance.Buyer.PersonBoughtCarFromPersonOnDateAsBuyer.Add(instance);
				}
				this.OnInternalUniquenessConstraint1Changed(instance, null, Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint3Changed(instance, null, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
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
				if (!(this.OnInternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(instance.Buyer, newValue, instance.Seller))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(instance.SaleDate, instance.Seller, newValue))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(newValue, instance.SaleDate, instance.Buyer))))
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
				this.OnInternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(instance.Buyer, oldValue, instance.Seller), Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(instance.SaleDate, instance.Seller, oldValue), Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint3Changed(instance, Tuple.CreateTuple(oldValue, instance.SaleDate, instance.Buyer), Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationCarSoldChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.CarSold != null)
				{
					instance.CarSold.PersonBoughtCarFromPersonOnDateAsCarSold.Add(instance);
				}
				this.OnInternalUniquenessConstraint1Changed(instance, null, Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint2Changed(instance, null, Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint3Changed(instance, null, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
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
				if (!(this.OnInternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(instance.Buyer, instance.CarSold, newValue))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(instance.SaleDate, newValue, instance.CarSold))))
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
				this.OnInternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(instance.Buyer, instance.CarSold, oldValue), Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(instance.SaleDate, oldValue, instance.CarSold), Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationSellerChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.Seller != null)
				{
					instance.Seller.PersonBoughtCarFromPersonOnDateAsSeller.Add(instance);
				}
				this.OnInternalUniquenessConstraint1Changed(instance, null, Tuple.CreateTuple(instance.Buyer, instance.CarSold, instance.Seller));
				this.OnInternalUniquenessConstraint2Changed(instance, null, Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
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
				if (!(this.OnInternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(newValue, instance.Seller, instance.CarSold))))
				{
					return false;
				}
				if (instance == null)
				{
					return true;
				}
				if (!(this.OnInternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(instance.CarSold, newValue, instance.Buyer))))
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
				this.OnInternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(oldValue, instance.Seller, instance.CarSold), Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint3Changed(instance, Tuple.CreateTuple(instance.CarSold, oldValue, instance.Buyer), Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
			}
			private void OnPersonBoughtCarFromPersonOnDateAssociationSaleDateChanged(PersonBoughtCarFromPersonOnDateAssociation instance)
			{
				if (instance.SaleDate != null)
				{
					instance.SaleDate.PersonBoughtCarFromPersonOnDateAsSaleDate.Add(instance);
				}
				this.OnInternalUniquenessConstraint2Changed(instance, null, Tuple.CreateTuple(instance.SaleDate, instance.Seller, instance.CarSold));
				this.OnInternalUniquenessConstraint3Changed(instance, null, Tuple.CreateTuple(instance.CarSold, instance.SaleDate, instance.Buyer));
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
				if (!(this.OnInternalUniquenessConstraint4Changing(instance, Tuple.CreateTuple(newValue, instance.CriteriaName))))
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
				this.OnInternalUniquenessConstraint4Changed(instance, Tuple.CreateTuple(oldValue, instance.CriteriaName), Tuple.CreateTuple(instance.Car, instance.CriteriaName));
			}
			private void OnReviewAssociationCarChanged(ReviewAssociation instance)
			{
				if (instance.Car != null)
				{
					instance.Car.ReviewAsCar.Add(instance);
				}
				this.OnInternalUniquenessConstraint4Changed(instance, null, Tuple.CreateTuple(instance.Car, instance.CriteriaName));
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
				if (!(this.OnInternalUniquenessConstraint4Changing(instance, Tuple.CreateTuple(instance.Car, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnReviewAssociationCriteriaNameChanged(ReviewAssociation instance, string oldValue)
			{
				this.OnInternalUniquenessConstraint4Changed(instance, Tuple.CreateTuple(instance.Car, oldValue), Tuple.CreateTuple(instance.Car, instance.CriteriaName));
			}
			private void OnReviewAssociationCriteriaNameChanged(ReviewAssociation instance)
			{
				this.OnInternalUniquenessConstraint4Changed(instance, null, Tuple.CreateTuple(instance.Car, instance.CriteriaName));
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
				public Person CreatePerson(int Person_id, string FirstName, string LastName)
				{
					return new PersonCore(this.myContext, Person_id, FirstName, LastName, null);
				}
				public Date CreateDate(System.DateTime ymd)
				{
					return new DateCore(this.myContext, ymd);
				}
				public Task CreateTask(int Task_id)
				{
					return new TaskCore(this.myContext, Task_id);
				}
				public Car CreateCar(uint vin)
				{
					return new CarCore(this.myContext, vin);
				}
				public PersonBoughtCarFromPersonOnDateAssociation CreatePersonBoughtCarFromPersonOnDateAssociation()
				{
					return new PersonBoughtCarFromPersonOnDateAssociationCore(this.myContext, null, null, null, null);
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

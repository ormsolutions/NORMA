using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute;
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
		public static bool operator ==(Tuple<T1, T2> tuple1, Tuple<T1, T2> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2> tuple1, Tuple<T1, T2> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3> tuple1, Tuple<T1, T2, T3> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3> tuple1, Tuple<T1, T2, T3> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4> tuple1, Tuple<T1, T2, T3, T4> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4> tuple1, Tuple<T1, T2, T3, T4> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5> tuple1, Tuple<T1, T2, T3, T4, T5> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5> tuple1, Tuple<T1, T2, T3, T4, T5> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6> tuple1, Tuple<T1, T2, T3, T4, T5, T6> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6> tuple1, Tuple<T1, T2, T3, T4, T5, T6> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple2)
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
		public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple2)
		{
			if (!(object.ReferenceEquals(tuple1, null)))
			{
				return tuple1.Equals(tuple2);
			}
			return object.ReferenceEquals(tuple2, null);
		}
		public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple1, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple2)
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
namespace TestNamespace
{
	namespace SampleModel
	{
		#region PersonDrivesCar
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class PersonDrivesCar : INotifyPropertyChanged
		{
			protected PersonDrivesCar()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> DrivesCar_vinChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDrivesCar_vinChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.DrivesCar_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> DrivesCar_vinChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDrivesCar_vinChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.DrivesCar_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DrivesCar_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> DrivenByPersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
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
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
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
			public abstract int DrivesCar_vin
			{
				get;
				set;
			}
			public abstract Person DrivenByPerson
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"PersonDrivesCar{0}{{{0}{1}DrivesCar_vin = ""{2}"",{0}{1}DrivenByPerson = {3}{0}}}", Environment.NewLine, "	", this.DrivesCar_vin, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonDrivesCar
		#region PersonBoughtCarFromPersonOnDate
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class PersonBoughtCarFromPersonOnDate : INotifyPropertyChanged
		{
			protected PersonBoughtCarFromPersonOnDate()
			{
			}
			private readonly Delegate[] Events = new Delegate[5];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> CarSold_vinChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseCarSold_vinChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.CarSold_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> CarSold_vinChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseCarSold_vinChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.CarSold_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("CarSold_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> SaleDate_YMDChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseSaleDate_YMDChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.SaleDate_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> SaleDate_YMDChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseSaleDate_YMDChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.SaleDate_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("SaleDate_YMD");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> BuyerChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseBuyerChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person>>;
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
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseBuyerChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Buyer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Buyer");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> SellerChanging
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseSellerChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Person>>;
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
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseSellerChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Seller), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Seller");
				}
			}
			public abstract int CarSold_vin
			{
				get;
				set;
			}
			public abstract int SaleDate_YMD
			{
				get;
				set;
			}
			public abstract Person Buyer
			{
				get;
				set;
			}
			public abstract Person Seller
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"PersonBoughtCarFromPersonOnDate{0}{{{0}{1}CarSold_vin = ""{2}"",{0}{1}SaleDate_YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, "	", this.CarSold_vin, this.SaleDate_YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDate
		#region Review
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class Review : INotifyPropertyChanged
		{
			protected Review()
			{
			}
			private readonly Delegate[] Events = new Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> Car_vinChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseCar_vinChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.Car_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> Car_vinChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseCar_vinChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.Car_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Car_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> Rating_Nr_IntegerChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseRating_Nr_IntegerChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.Rating_Nr_Integer, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> Rating_Nr_IntegerChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseRating_Nr_IntegerChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.Rating_Nr_Integer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Rating_Nr_Integer");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Criteria_NameChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseCriteria_NameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.Criteria_Name, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Criteria_NameChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseCriteria_NameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.Criteria_Name), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Criteria_Name");
				}
			}
			public abstract int Car_vin
			{
				get;
				set;
			}
			public abstract int Rating_Nr_Integer
			{
				get;
				set;
			}
			public abstract string Criteria_Name
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criteria_Name = ""{4}""{0}}}", Environment.NewLine, "	", this.Car_vin, this.Rating_Nr_Integer, this.Criteria_Name);
			}
		}
		#endregion // Review
		#region PersonHasNickName
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class PersonHasNickName : INotifyPropertyChanged
		{
			protected PersonHasNickName()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> NickNameChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseNickNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
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
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseNickNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.NickName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NickName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
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
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
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
			public abstract string NickName
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"PersonHasNickName{0}{{{0}{1}NickName = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.NickName, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonHasNickName
		#region Person
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class Person : INotifyPropertyChanged
		{
			protected Person()
			{
			}
			private readonly Delegate[] Events = new Delegate[15];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> FirstNameChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFirstNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
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
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFirstNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.FirstName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("FirstName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> Date_YMDChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDate_YMDChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.Date_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> Date_YMDChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDate_YMDChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.Date_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date_YMD");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> LastNameChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
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
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
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
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
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
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
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
			public event EventHandler<PropertyChangingEventArgs<int>> HatType_ColorARGBChanging
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseHatType_ColorARGBChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.HatType_ColorARGB, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> HatType_ColorARGBChanged
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseHatType_ColorARGBChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.HatType_ColorARGB), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatType_ColorARGB");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
			{
				add
				{
					this.Events[6] = Delegate.Combine(this.Events[6], value);
				}
				remove
				{
					this.Events[6] = Delegate.Remove(this.Events[6], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[6] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.HatType_HatTypeStyle_HatTypeStyle_Description, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
			{
				add
				{
					this.Events[6] = Delegate.Combine(this.Events[6], value);
				}
				remove
				{
					this.Events[6] = Delegate.Remove(this.Events[6], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.HatType_HatTypeStyle_HatTypeStyle_Description), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatType_HatTypeStyle_HatTypeStyle_Description");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<int>> OwnsCar_vinChanging
			{
				add
				{
					this.Events[7] = Delegate.Combine(this.Events[7], value);
				}
				remove
				{
					this.Events[7] = Delegate.Remove(this.Events[7], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseOwnsCar_vinChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.OwnsCar_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> OwnsCar_vinChanged
			{
				add
				{
					this.Events[7] = Delegate.Combine(this.Events[7], value);
				}
				remove
				{
					this.Events[7] = Delegate.Remove(this.Events[7], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseOwnsCar_vinChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[7] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.OwnsCar_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("OwnsCar_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Gender_Gender_CodeChanging
			{
				add
				{
					this.Events[8] = Delegate.Combine(this.Events[8], value);
				}
				remove
				{
					this.Events[8] = Delegate.Remove(this.Events[8], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseGender_Gender_CodeChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[8] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.Gender_Gender_Code, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Gender_Gender_CodeChanged
			{
				add
				{
					this.Events[8] = Delegate.Combine(this.Events[8], value);
				}
				remove
				{
					this.Events[8] = Delegate.Remove(this.Events[8], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseGender_Gender_CodeChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.Gender_Gender_Code), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Gender_Gender_Code");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<bool>> PersonHasParentsASPersonChanging
			{
				add
				{
					this.Events[9] = Delegate.Combine(this.Events[9], value);
				}
				remove
				{
					this.Events[9] = Delegate.Remove(this.Events[9], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonHasParentsASPersonChangingEvent(bool newValue)
			{
				EventHandler<PropertyChangingEventArgs<bool>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<bool>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<bool> eventArgs = new PropertyChangingEventArgs<bool>(this.PersonHasParentsASPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<bool>> PersonHasParentsASPersonChanged
			{
				add
				{
					this.Events[9] = Delegate.Combine(this.Events[9], value);
				}
				remove
				{
					this.Events[9] = Delegate.Remove(this.Events[9], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonHasParentsASPersonChangedEvent(bool oldValue)
			{
				EventHandler<PropertyChangedEventArgs<bool>> eventHandler = this.Events[9] as EventHandler<PropertyChangedEventArgs<bool>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<bool>(oldValue, this.PersonHasParentsASPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("PersonHasParentsASPerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<ValueType1>> ValueType1Changing
			{
				add
				{
					this.Events[10] = Delegate.Combine(this.Events[10], value);
				}
				remove
				{
					this.Events[10] = Delegate.Remove(this.Events[10], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseValueType1ChangingEvent(ValueType1 newValue)
			{
				EventHandler<PropertyChangingEventArgs<ValueType1>> eventHandler = this.Events[10] as EventHandler<PropertyChangingEventArgs<ValueType1>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<ValueType1> eventArgs = new PropertyChangingEventArgs<ValueType1>(this.ValueType1, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<ValueType1>> ValueType1Changed
			{
				add
				{
					this.Events[10] = Delegate.Combine(this.Events[10], value);
				}
				remove
				{
					this.Events[10] = Delegate.Remove(this.Events[10], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseValueType1ChangedEvent(ValueType1 oldValue)
			{
				EventHandler<PropertyChangedEventArgs<ValueType1>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<ValueType1>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ValueType1>(oldValue, this.ValueType1), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<MalePerson>> MalePersonChanging
			{
				add
				{
					this.Events[11] = Delegate.Combine(this.Events[11], value);
				}
				remove
				{
					this.Events[11] = Delegate.Remove(this.Events[11], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseMalePersonChangingEvent(MalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<MalePerson>> eventHandler = this.Events[11] as EventHandler<PropertyChangingEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<MalePerson> eventArgs = new PropertyChangingEventArgs<MalePerson>(this.MalePerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<MalePerson>> MalePersonChanged
			{
				add
				{
					this.Events[11] = Delegate.Combine(this.Events[11], value);
				}
				remove
				{
					this.Events[11] = Delegate.Remove(this.Events[11], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseMalePersonChangedEvent(MalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<MalePerson>> eventHandler = this.Events[11] as EventHandler<PropertyChangedEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<MalePerson>(oldValue, this.MalePerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("MalePerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<FemalePerson>> FemalePersonChanging
			{
				add
				{
					this.Events[12] = Delegate.Combine(this.Events[12], value);
				}
				remove
				{
					this.Events[12] = Delegate.Remove(this.Events[12], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFemalePersonChangingEvent(FemalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<FemalePerson>> eventHandler = this.Events[12] as EventHandler<PropertyChangingEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<FemalePerson> eventArgs = new PropertyChangingEventArgs<FemalePerson>(this.FemalePerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<FemalePerson>> FemalePersonChanged
			{
				add
				{
					this.Events[12] = Delegate.Combine(this.Events[12], value);
				}
				remove
				{
					this.Events[12] = Delegate.Remove(this.Events[12], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFemalePersonChangedEvent(FemalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<FemalePerson>> eventHandler = this.Events[12] as EventHandler<PropertyChangedEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<FemalePerson>(oldValue, this.FemalePerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("FemalePerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<ChildPerson>> ChildPersonChanging
			{
				add
				{
					this.Events[13] = Delegate.Combine(this.Events[13], value);
				}
				remove
				{
					this.Events[13] = Delegate.Remove(this.Events[13], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseChildPersonChangingEvent(ChildPerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<ChildPerson>> eventHandler = this.Events[13] as EventHandler<PropertyChangingEventArgs<ChildPerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<ChildPerson> eventArgs = new PropertyChangingEventArgs<ChildPerson>(this.ChildPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<ChildPerson>> ChildPersonChanged
			{
				add
				{
					this.Events[13] = Delegate.Combine(this.Events[13], value);
				}
				remove
				{
					this.Events[13] = Delegate.Remove(this.Events[13], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseChildPersonChangedEvent(ChildPerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<ChildPerson>> eventHandler = this.Events[13] as EventHandler<PropertyChangedEventArgs<ChildPerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ChildPerson>(oldValue, this.ChildPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ChildPerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
			{
				add
				{
					this.Events[14] = Delegate.Combine(this.Events[14], value);
				}
				remove
				{
					this.Events[14] = Delegate.Remove(this.Events[14], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[14] as EventHandler<PropertyChangingEventArgs<Death>>;
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
					this.Events[14] = Delegate.Combine(this.Events[14], value);
				}
				remove
				{
					this.Events[14] = Delegate.Remove(this.Events[14], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[14] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public abstract string FirstName
			{
				get;
				set;
			}
			public abstract int Date_YMD
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
			public abstract int HatType_ColorARGB
			{
				get;
				set;
			}
			public abstract string HatType_HatTypeStyle_HatTypeStyle_Description
			{
				get;
				set;
			}
			public abstract int OwnsCar_vin
			{
				get;
				set;
			}
			public abstract string Gender_Gender_Code
			{
				get;
				set;
			}
			public abstract bool PersonHasParentsASPerson
			{
				get;
				set;
			}
			public abstract ValueType1 ValueType1
			{
				get;
				set;
			}
			public abstract MalePerson MalePerson
			{
				get;
				set;
			}
			public abstract FemalePerson FemalePerson
			{
				get;
				set;
			}
			public abstract ChildPerson ChildPerson
			{
				get;
				set;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public abstract ICollection<PersonDrivesCar> PersonDrivesCarASDrivenByPerson
			{
				get;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateASBuyer
			{
				get;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateASSeller
			{
				get;
			}
			public abstract ICollection<PersonHasNickName> PersonHasNickNameASPerson
			{
				get;
			}
			public abstract ICollection<Task> Task
			{
				get;
			}
			public abstract ICollection<ValueType1> ValueType1
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}SocialSecurityNumber = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}PersonHasParentsASPerson = ""{10}"",{0}{1}ValueType1 = {11},{0}{1}MalePerson = {12},{0}{1}FemalePerson = {13},{0}{1}ChildPerson = {14},{0}{1}Death = {15}{0}}}", Environment.NewLine, "	", this.FirstName, this.Date_YMD, this.LastName, this.SocialSecurityNumber, this.HatType_ColorARGB, this.HatType_HatTypeStyle_HatTypeStyle_Description, this.OwnsCar_vin, this.Gender_Gender_Code, this.PersonHasParentsASPerson, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
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
		}
		#endregion // Person
		#region MalePerson
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class MalePerson : INotifyPropertyChanged
		{
			protected MalePerson()
			{
			}
			private readonly Delegate[] Events = new Delegate[2];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
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
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
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
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ICollection<ChildPerson> ChildPerson
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
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
		}
		#endregion // MalePerson
		#region FemalePerson
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class FemalePerson : INotifyPropertyChanged
		{
			protected FemalePerson()
			{
			}
			private readonly Delegate[] Events = new Delegate[2];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
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
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
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
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ICollection<ChildPerson> ChildPerson
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
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
		}
		#endregion // FemalePerson
		#region ChildPerson
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class ChildPerson : INotifyPropertyChanged
		{
			protected ChildPerson()
			{
			}
			private readonly Delegate[] Events = new Delegate[5];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> BirthOrder_BirthOrder_NrChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseBirthOrder_BirthOrder_NrChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.BirthOrder_BirthOrder_Nr, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> BirthOrder_BirthOrder_NrChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseBirthOrder_BirthOrder_NrChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.BirthOrder_BirthOrder_Nr), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("BirthOrder_BirthOrder_Nr");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<MalePerson>> FatherChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseFatherChangingEvent(MalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<MalePerson>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<MalePerson> eventArgs = new PropertyChangingEventArgs<MalePerson>(this.Father, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<MalePerson>> FatherChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseFatherChangedEvent(MalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<MalePerson>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<MalePerson>(oldValue, this.Father), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Father");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<FemalePerson>> MotherChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseMotherChangingEvent(FemalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<FemalePerson>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<FemalePerson> eventArgs = new PropertyChangingEventArgs<FemalePerson>(this.Mother, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<FemalePerson>> MotherChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseMotherChangedEvent(FemalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<FemalePerson>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<FemalePerson>(oldValue, this.Mother), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Mother");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
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
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
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
			public abstract int BirthOrder_BirthOrder_Nr
			{
				get;
				set;
			}
			public abstract MalePerson Father
			{
				get;
				set;
			}
			public abstract FemalePerson Mother
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
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
		}
		#endregion // ChildPerson
		#region Death
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class Death : INotifyPropertyChanged
		{
			protected Death()
			{
			}
			private readonly Delegate[] Events = new Delegate[6];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> Date_YMDChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDate_YMDChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.Date_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> Date_YMDChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDate_YMDChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.Date_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date_YMD");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> DeathCause_DeathCause_TypeChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathCause_DeathCause_TypeChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<string>(this.DeathCause_DeathCause_Type, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> DeathCause_DeathCause_TypeChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathCause_DeathCause_TypeChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<string>(oldValue, this.DeathCause_DeathCause_Type), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DeathCause_DeathCause_Type");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<NaturalDeath>> NaturalDeathChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseNaturalDeathChangingEvent(NaturalDeath newValue)
			{
				EventHandler<PropertyChangingEventArgs<NaturalDeath>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<NaturalDeath>>;
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
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseNaturalDeathChangedEvent(NaturalDeath oldValue)
			{
				EventHandler<PropertyChangedEventArgs<NaturalDeath>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<NaturalDeath>>;
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
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseUnnaturalDeathChangingEvent(UnnaturalDeath newValue)
			{
				EventHandler<PropertyChangingEventArgs<UnnaturalDeath>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath>>;
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
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseUnnaturalDeathChangedEvent(UnnaturalDeath oldValue)
			{
				EventHandler<PropertyChangedEventArgs<UnnaturalDeath>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<UnnaturalDeath>(oldValue, this.UnnaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Person>>;
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
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract int Date_YMD
			{
				get;
				set;
			}
			public abstract string DeathCause_DeathCause_Type
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
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
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
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class NaturalDeath : INotifyPropertyChanged
		{
			protected NaturalDeath()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<bool>> NaturalDeathIsFromProstateCancerASNaturalDeathChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseNaturalDeathIsFromProstateCancerASNaturalDeathChangingEvent(bool newValue)
			{
				EventHandler<PropertyChangingEventArgs<bool>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<bool>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<bool> eventArgs = new PropertyChangingEventArgs<bool>(this.NaturalDeathIsFromProstateCancerASNaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<bool>> NaturalDeathIsFromProstateCancerASNaturalDeathChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseNaturalDeathIsFromProstateCancerASNaturalDeathChangedEvent(bool oldValue)
			{
				EventHandler<PropertyChangedEventArgs<bool>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<bool>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<bool>(oldValue, this.NaturalDeathIsFromProstateCancerASNaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NaturalDeathIsFromProstateCancerASNaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Death>>;
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
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Death>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public abstract bool NaturalDeathIsFromProstateCancerASNaturalDeath
			{
				get;
				set;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"NaturalDeath{0}{{{0}{1}NaturalDeathIsFromProstateCancerASNaturalDeath = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, "	", this.NaturalDeathIsFromProstateCancerASNaturalDeath, "TODO: Recursively call ToString for customTypes...");
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
		}
		#endregion // NaturalDeath
		#region UnnaturalDeath
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class UnnaturalDeath : INotifyPropertyChanged
		{
			protected UnnaturalDeath()
			{
			}
			private readonly Delegate[] Events = new Delegate[4];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<bool>> UnnaturalDeathIsViolentASUnnaturalDeathChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseUnnaturalDeathIsViolentASUnnaturalDeathChangingEvent(bool newValue)
			{
				EventHandler<PropertyChangingEventArgs<bool>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<bool>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<bool> eventArgs = new PropertyChangingEventArgs<bool>(this.UnnaturalDeathIsViolentASUnnaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<bool>> UnnaturalDeathIsViolentASUnnaturalDeathChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseUnnaturalDeathIsViolentASUnnaturalDeathChangedEvent(bool oldValue)
			{
				EventHandler<PropertyChangedEventArgs<bool>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<bool>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<bool>(oldValue, this.UnnaturalDeathIsViolentASUnnaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeathIsViolentASUnnaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<bool>> UnnaturalDeathIsBloodyASUnnaturalDeathChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseUnnaturalDeathIsBloodyASUnnaturalDeathChangingEvent(bool newValue)
			{
				EventHandler<PropertyChangingEventArgs<bool>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<bool>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<bool> eventArgs = new PropertyChangingEventArgs<bool>(this.UnnaturalDeathIsBloodyASUnnaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<bool>> UnnaturalDeathIsBloodyASUnnaturalDeathChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseUnnaturalDeathIsBloodyASUnnaturalDeathChangedEvent(bool oldValue)
			{
				EventHandler<PropertyChangedEventArgs<bool>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<bool>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<bool>(oldValue, this.UnnaturalDeathIsBloodyASUnnaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeathIsBloodyASUnnaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
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
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
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
			public abstract bool UnnaturalDeathIsViolentASUnnaturalDeath
			{
				get;
				set;
			}
			public abstract bool UnnaturalDeathIsBloodyASUnnaturalDeath
			{
				get;
				set;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"UnnaturalDeath{0}{{{0}{1}UnnaturalDeathIsViolentASUnnaturalDeath = ""{2}"",{0}{1}UnnaturalDeathIsBloodyASUnnaturalDeath = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, "	", this.UnnaturalDeathIsViolentASUnnaturalDeath, this.UnnaturalDeathIsBloodyASUnnaturalDeath, "TODO: Recursively call ToString for customTypes...");
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
		}
		#endregion // UnnaturalDeath
		#region Task
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class Task : INotifyPropertyChanged
		{
			protected Task()
			{
			}
			private readonly Delegate[] Events = new Delegate[2];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
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
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
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
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "Task{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Task
		#region ValueType1
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public abstract partial class ValueType1 : INotifyPropertyChanged
		{
			protected ValueType1()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033")]
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
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
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<int>> ValueType1ValueChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected bool RaiseValueType1ValueChangingEvent(int newValue)
			{
				EventHandler<PropertyChangingEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<int>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<int> eventArgs = new PropertyChangingEventArgs<int>(this.ValueType1Value, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<int>> ValueType1ValueChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1030")]
			protected void RaiseValueType1ValueChangedEvent(int oldValue)
			{
				EventHandler<PropertyChangedEventArgs<int>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<int>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<int>(oldValue, this.ValueType1Value), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1Value");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
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
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
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
			public abstract int ValueType1Value
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ICollection<Person> Person
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.ValueType1Value, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // ValueType1
		#region ISampleModelContext
		[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
		public interface ISampleModelContext
		{
			bool IsDeserializing
			{
				get;
			}
			PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson);
			PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller);
			PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin);
			PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer);
			Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criteria_Name);
			PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person);
			ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother);
			Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD);
			Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD);
			Person GetPersonBySocialSecurityNumber(string SocialSecurityNumber);
			Person GetPersonByOwnsCar_vin(int OwnsCar_vin);
			ValueType1 GetValueType1ByValueType1Value(int ValueType1Value);
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
			Review CreateReview(int Car_vin, int Rating_Nr_Integer, string Criteria_Name);
			ReadOnlyCollection<Review> ReviewCollection
			{
				get;
			}
			PersonHasNickName CreatePersonHasNickName(string NickName, Person Person);
			ReadOnlyCollection<PersonHasNickName> PersonHasNickNameCollection
			{
				get;
			}
			Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code);
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
		public sealed class SampleModelContext : ISampleModelContext
		{
			public SampleModelContext()
			{
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
					this.myadding = adding;
					this.myadded = added;
					this.myremoving = removing;
					this.myremoved = removed;
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
			private bool IsDeserializing
			{
				get
				{
					return this.myIsDeserializing;
				}
			}
			bool ISampleModelContext.IsDeserializing
			{
				get
				{
					return this.IsDeserializing;
				}
			}
			private readonly Dictionary<Tuple<int, Person>, PersonDrivesCar> myInternalUniquenessConstraint18Dictionary = new Dictionary<Tuple<int, Person>, PersonDrivesCar>();
			private PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson)
			{
				return this.myInternalUniquenessConstraint18Dictionary[Tuple.CreateTuple(DrivesCar_vin, DrivenByPerson)];
			}
			PersonDrivesCar ISampleModelContext.GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson)
			{
				return this.GetPersonDrivesCarByInternalUniquenessConstraint18(DrivesCar_vin, DrivenByPerson);
			}
			private bool OnInternalUniquenessConstraint18Changing(PersonDrivesCar instance, Tuple<int, Person> newValue)
			{
				if (newValue != null)
				{
					PersonDrivesCar currentInstance = instance;
					if (this.myInternalUniquenessConstraint18Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint18Changed(PersonDrivesCar instance, Tuple<int, Person> oldValue, Tuple<int, Person> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint18Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint18Dictionary.Add(newValue, instance);
				}
			}
			private readonly Dictionary<Tuple<Person, int, Person>, PersonBoughtCarFromPersonOnDate> myInternalUniquenessConstraint23Dictionary = new Dictionary<Tuple<Person, int, Person>, PersonBoughtCarFromPersonOnDate>();
			private PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller)
			{
				return this.myInternalUniquenessConstraint23Dictionary[Tuple.CreateTuple(Buyer, CarSold_vin, Seller)];
			}
			PersonBoughtCarFromPersonOnDate ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller)
			{
				return this.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Buyer, CarSold_vin, Seller);
			}
			private bool OnInternalUniquenessConstraint23Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, int, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDate currentInstance = instance;
					if (this.myInternalUniquenessConstraint23Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint23Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, int, Person> oldValue, Tuple<Person, int, Person> newValue)
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
			private readonly Dictionary<Tuple<int, Person, int>, PersonBoughtCarFromPersonOnDate> myInternalUniquenessConstraint24Dictionary = new Dictionary<Tuple<int, Person, int>, PersonBoughtCarFromPersonOnDate>();
			private PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin)
			{
				return this.myInternalUniquenessConstraint24Dictionary[Tuple.CreateTuple(SaleDate_YMD, Seller, CarSold_vin)];
			}
			PersonBoughtCarFromPersonOnDate ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin)
			{
				return this.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(SaleDate_YMD, Seller, CarSold_vin);
			}
			private bool OnInternalUniquenessConstraint24Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<int, Person, int> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDate currentInstance = instance;
					if (this.myInternalUniquenessConstraint24Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint24Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<int, Person, int> oldValue, Tuple<int, Person, int> newValue)
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
			private readonly Dictionary<Tuple<int, int, Person>, PersonBoughtCarFromPersonOnDate> myInternalUniquenessConstraint25Dictionary = new Dictionary<Tuple<int, int, Person>, PersonBoughtCarFromPersonOnDate>();
			private PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer)
			{
				return this.myInternalUniquenessConstraint25Dictionary[Tuple.CreateTuple(CarSold_vin, SaleDate_YMD, Buyer)];
			}
			PersonBoughtCarFromPersonOnDate ISampleModelContext.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer)
			{
				return this.GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(CarSold_vin, SaleDate_YMD, Buyer);
			}
			private bool OnInternalUniquenessConstraint25Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<int, int, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDate currentInstance = instance;
					if (this.myInternalUniquenessConstraint25Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint25Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<int, int, Person> oldValue, Tuple<int, int, Person> newValue)
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
			private readonly Dictionary<Tuple<int, string>, Review> myInternalUniquenessConstraint26Dictionary = new Dictionary<Tuple<int, string>, Review>();
			private Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criteria_Name)
			{
				return this.myInternalUniquenessConstraint26Dictionary[Tuple.CreateTuple(Car_vin, Criteria_Name)];
			}
			Review ISampleModelContext.GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criteria_Name)
			{
				return this.GetReviewByInternalUniquenessConstraint26(Car_vin, Criteria_Name);
			}
			private bool OnInternalUniquenessConstraint26Changing(Review instance, Tuple<int, string> newValue)
			{
				if (newValue != null)
				{
					Review currentInstance = instance;
					if (this.myInternalUniquenessConstraint26Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint26Changed(Review instance, Tuple<int, string> oldValue, Tuple<int, string> newValue)
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
			private readonly Dictionary<Tuple<string, Person>, PersonHasNickName> myInternalUniquenessConstraint33Dictionary = new Dictionary<Tuple<string, Person>, PersonHasNickName>();
			private PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person)
			{
				return this.myInternalUniquenessConstraint33Dictionary[Tuple.CreateTuple(NickName, Person)];
			}
			PersonHasNickName ISampleModelContext.GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person)
			{
				return this.GetPersonHasNickNameByInternalUniquenessConstraint33(NickName, Person);
			}
			private bool OnInternalUniquenessConstraint33Changing(PersonHasNickName instance, Tuple<string, Person> newValue)
			{
				if (newValue != null)
				{
					PersonHasNickName currentInstance = instance;
					if (this.myInternalUniquenessConstraint33Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint33Changed(PersonHasNickName instance, Tuple<string, Person> oldValue, Tuple<string, Person> newValue)
			{
				if (oldValue != null)
				{
					this.myInternalUniquenessConstraint33Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myInternalUniquenessConstraint33Dictionary.Add(newValue, instance);
				}
			}
			private readonly Dictionary<Tuple<MalePerson, int, FemalePerson>, ChildPerson> myExternalUniquenessConstraint3Dictionary = new Dictionary<Tuple<MalePerson, int, FemalePerson>, ChildPerson>();
			private ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother)
			{
				return this.myExternalUniquenessConstraint3Dictionary[Tuple.CreateTuple(Father, BirthOrder_BirthOrder_Nr, Mother)];
			}
			ChildPerson ISampleModelContext.GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother)
			{
				return this.GetChildPersonByExternalUniquenessConstraint3(Father, BirthOrder_BirthOrder_Nr, Mother);
			}
			private bool OnExternalUniquenessConstraint3Changing(ChildPerson instance, Tuple<MalePerson, int, FemalePerson> newValue)
			{
				if (newValue != null)
				{
					ChildPerson currentInstance = instance;
					if (this.myExternalUniquenessConstraint3Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint3Changed(ChildPerson instance, Tuple<MalePerson, int, FemalePerson> oldValue, Tuple<MalePerson, int, FemalePerson> newValue)
			{
				if (oldValue != null)
				{
					this.myExternalUniquenessConstraint3Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this.myExternalUniquenessConstraint3Dictionary.Add(newValue, instance);
				}
			}
			private readonly Dictionary<Tuple<string, int>, Person> myExternalUniquenessConstraint1Dictionary = new Dictionary<Tuple<string, int>, Person>();
			private Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD)
			{
				return this.myExternalUniquenessConstraint1Dictionary[Tuple.CreateTuple(FirstName, Date_YMD)];
			}
			Person ISampleModelContext.GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD)
			{
				return this.GetPersonByExternalUniquenessConstraint1(FirstName, Date_YMD);
			}
			private bool OnExternalUniquenessConstraint1Changing(Person instance, Tuple<string, int> newValue)
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
			private void OnExternalUniquenessConstraint1Changed(Person instance, Tuple<string, int> oldValue, Tuple<string, int> newValue)
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
			private readonly Dictionary<Tuple<string, int>, Person> myExternalUniquenessConstraint2Dictionary = new Dictionary<Tuple<string, int>, Person>();
			private Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD)
			{
				return this.myExternalUniquenessConstraint2Dictionary[Tuple.CreateTuple(LastName, Date_YMD)];
			}
			Person ISampleModelContext.GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD)
			{
				return this.GetPersonByExternalUniquenessConstraint2(LastName, Date_YMD);
			}
			private bool OnExternalUniquenessConstraint2Changing(Person instance, Tuple<string, int> newValue)
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
			private void OnExternalUniquenessConstraint2Changed(Person instance, Tuple<string, int> oldValue, Tuple<string, int> newValue)
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
			private readonly Dictionary<string, Person> myPersonSocialSecurityNumberDictionary = new Dictionary<string, Person>();
			private Person GetPersonBySocialSecurityNumber(string SocialSecurityNumber)
			{
				return this.myPersonSocialSecurityNumberDictionary[SocialSecurityNumber];
			}
			Person ISampleModelContext.GetPersonBySocialSecurityNumber(string SocialSecurityNumber)
			{
				return this.GetPersonBySocialSecurityNumber(SocialSecurityNumber);
			}
			private readonly Dictionary<int, Person> myPersonOwnsCar_vinDictionary = new Dictionary<int, Person>();
			private Person GetPersonByOwnsCar_vin(int OwnsCar_vin)
			{
				return this.myPersonOwnsCar_vinDictionary[OwnsCar_vin];
			}
			Person ISampleModelContext.GetPersonByOwnsCar_vin(int OwnsCar_vin)
			{
				return this.GetPersonByOwnsCar_vin(OwnsCar_vin);
			}
			private readonly Dictionary<int, ValueType1> myValueType1ValueType1ValueDictionary = new Dictionary<int, ValueType1>();
			private ValueType1 GetValueType1ByValueType1Value(int ValueType1Value)
			{
				return this.myValueType1ValueType1ValueDictionary[ValueType1Value];
			}
			ValueType1 ISampleModelContext.GetValueType1ByValueType1Value(int ValueType1Value)
			{
				return this.GetValueType1ByValueType1Value(ValueType1Value);
			}
			private bool OnPersonDrivesCarDrivesCar_vinChanging(PersonDrivesCar instance, int newValue)
			{
				if (!(this.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple(newValue, instance.DrivenByPerson))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonDrivesCarDrivesCar_vinChanged(PersonDrivesCar instance, int oldValue)
			{
				this.OnInternalUniquenessConstraint18Changed(instance, Tuple.CreateTuple(oldValue, instance.DrivenByPerson), Tuple.CreateTuple(instance.DrivesCar_vin, instance.DrivenByPerson));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonDrivesCarDrivenByPersonChanging(PersonDrivesCar instance, Person newValue)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (!(this.OnInternalUniquenessConstraint18Changing(instance, Tuple.CreateTuple(instance.DrivesCar_vin, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonDrivesCarDrivenByPersonChanged(PersonDrivesCar instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonDrivesCarASDrivenByPerson.Remove(instance);
				}
				if (instance.DrivenByPerson != null)
				{
					instance.DrivenByPerson.PersonDrivesCarASDrivenByPerson.Add(instance);
				}
				this.OnInternalUniquenessConstraint18Changed(instance, Tuple.CreateTuple(instance.DrivesCar_vin, oldValue), Tuple.CreateTuple(instance.DrivesCar_vin, instance.DrivenByPerson));
			}
			#region PersonDrivesCarCore
			private sealed class PersonDrivesCarCore : PersonDrivesCar
			{
				public PersonDrivesCarCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myDrivesCar_vin;
				public override int DrivesCar_vin
				{
					get
					{
						return this.myDrivesCar_vin;
					}
					set
					{
						if (!(object.Equals(this.DrivesCar_vin, value)))
						{
							if (this.Context.OnPersonDrivesCarDrivesCar_vinChanging(this, value))
							{
								if (base.RaiseDrivesCar_vinChangingEvent(value))
								{
									int oldValue = this.DrivesCar_vin;
									this.myDrivesCar_vin = value;
									this.Context.OnPersonDrivesCarDrivesCar_vinChanged(this, oldValue);
									base.RaiseDrivesCar_vinChangedEvent(oldValue);
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
							if (this.Context.OnPersonDrivesCarDrivenByPersonChanging(this, value))
							{
								if (base.RaiseDrivenByPersonChangingEvent(value))
								{
									Person oldValue = this.DrivenByPerson;
									this.myDrivenByPerson = value;
									this.Context.OnPersonDrivesCarDrivenByPersonChanged(this, oldValue);
									base.RaiseDrivenByPersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // PersonDrivesCarCore
			private bool OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(PersonBoughtCarFromPersonOnDate instance, int newValue)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(instance.Buyer, newValue, instance.Seller))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(instance.SaleDate_YMD, instance.Seller, newValue))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(newValue, instance.SaleDate_YMD, instance.Buyer))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(PersonBoughtCarFromPersonOnDate instance, int oldValue)
			{
				this.OnInternalUniquenessConstraint23Changed(instance, Tuple.CreateTuple(instance.Buyer, oldValue, instance.Seller), Tuple.CreateTuple(instance.Buyer, instance.CarSold_vin, instance.Seller));
				this.OnInternalUniquenessConstraint24Changed(instance, Tuple.CreateTuple(instance.SaleDate_YMD, instance.Seller, oldValue), Tuple.CreateTuple(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
				this.OnInternalUniquenessConstraint25Changed(instance, Tuple.CreateTuple(oldValue, instance.SaleDate_YMD, instance.Buyer), Tuple.CreateTuple(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
			}
			private bool OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(PersonBoughtCarFromPersonOnDate instance, int newValue)
			{
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(newValue, instance.Seller, instance.CarSold_vin))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(instance.CarSold_vin, newValue, instance.Buyer))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(PersonBoughtCarFromPersonOnDate instance, int oldValue)
			{
				this.OnInternalUniquenessConstraint24Changed(instance, Tuple.CreateTuple(oldValue, instance.Seller, instance.CarSold_vin), Tuple.CreateTuple(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
				this.OnInternalUniquenessConstraint25Changed(instance, Tuple.CreateTuple(instance.CarSold_vin, oldValue, instance.Buyer), Tuple.CreateTuple(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateBuyerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(newValue, instance.CarSold_vin, instance.Seller))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple(instance.CarSold_vin, instance.SaleDate_YMD, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateBuyerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonBoughtCarFromPersonOnDateASBuyer.Remove(instance);
				}
				if (instance.Buyer != null)
				{
					instance.Buyer.PersonBoughtCarFromPersonOnDateASBuyer.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, Tuple.CreateTuple(oldValue, instance.CarSold_vin, instance.Seller), Tuple.CreateTuple(instance.Buyer, instance.CarSold_vin, instance.Seller));
				this.OnInternalUniquenessConstraint25Changed(instance, Tuple.CreateTuple(instance.CarSold_vin, instance.SaleDate_YMD, oldValue), Tuple.CreateTuple(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateSellerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple(instance.Buyer, instance.CarSold_vin, newValue))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple(instance.SaleDate_YMD, newValue, instance.CarSold_vin))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonBoughtCarFromPersonOnDateSellerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonBoughtCarFromPersonOnDateASSeller.Remove(instance);
				}
				if (instance.Seller != null)
				{
					instance.Seller.PersonBoughtCarFromPersonOnDateASSeller.Add(instance);
				}
				this.OnInternalUniquenessConstraint23Changed(instance, Tuple.CreateTuple(instance.Buyer, instance.CarSold_vin, oldValue), Tuple.CreateTuple(instance.Buyer, instance.CarSold_vin, instance.Seller));
				this.OnInternalUniquenessConstraint24Changed(instance, Tuple.CreateTuple(instance.SaleDate_YMD, oldValue, instance.CarSold_vin), Tuple.CreateTuple(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
			}
			#region PersonBoughtCarFromPersonOnDateCore
			private sealed class PersonBoughtCarFromPersonOnDateCore : PersonBoughtCarFromPersonOnDate
			{
				public PersonBoughtCarFromPersonOnDateCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myCarSold_vin;
				public override int CarSold_vin
				{
					get
					{
						return this.myCarSold_vin;
					}
					set
					{
						if (!(object.Equals(this.CarSold_vin, value)))
						{
							if (this.Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(this, value))
							{
								if (base.RaiseCarSold_vinChangingEvent(value))
								{
									int oldValue = this.CarSold_vin;
									this.myCarSold_vin = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(this, oldValue);
									base.RaiseCarSold_vinChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private int mySaleDate_YMD;
				public override int SaleDate_YMD
				{
					get
					{
						return this.mySaleDate_YMD;
					}
					set
					{
						if (!(object.Equals(this.SaleDate_YMD, value)))
						{
							if (this.Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(this, value))
							{
								if (base.RaiseSaleDate_YMDChangingEvent(value))
								{
									int oldValue = this.SaleDate_YMD;
									this.mySaleDate_YMD = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(this, oldValue);
									base.RaiseSaleDate_YMDChangedEvent(oldValue);
								}
							}
						}
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
							if (this.Context.OnPersonBoughtCarFromPersonOnDateBuyerChanging(this, value))
							{
								if (base.RaiseBuyerChangingEvent(value))
								{
									Person oldValue = this.Buyer;
									this.myBuyer = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, oldValue);
									base.RaiseBuyerChangedEvent(oldValue);
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
							if (this.Context.OnPersonBoughtCarFromPersonOnDateSellerChanging(this, value))
							{
								if (base.RaiseSellerChangingEvent(value))
								{
									Person oldValue = this.Seller;
									this.mySeller = value;
									this.Context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, oldValue);
									base.RaiseSellerChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // PersonBoughtCarFromPersonOnDateCore
			private bool OnReviewCar_vinChanging(Review instance, int newValue)
			{
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(newValue, instance.Criteria_Name))))
				{
					return false;
				}
				return true;
			}
			private void OnReviewCar_vinChanged(Review instance, int oldValue)
			{
				this.OnInternalUniquenessConstraint26Changed(instance, Tuple.CreateTuple(oldValue, instance.Criteria_Name), Tuple.CreateTuple(instance.Car_vin, instance.Criteria_Name));
			}
			private bool OnReviewRating_Nr_IntegerChanging(Review instance, int newValue)
			{
				return true;
			}
			private bool OnReviewCriteria_NameChanging(Review instance, string newValue)
			{
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple(instance.Car_vin, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnReviewCriteria_NameChanged(Review instance, string oldValue)
			{
				this.OnInternalUniquenessConstraint26Changed(instance, Tuple.CreateTuple(instance.Car_vin, oldValue), Tuple.CreateTuple(instance.Car_vin, instance.Criteria_Name));
			}
			#region ReviewCore
			private sealed class ReviewCore : Review
			{
				public ReviewCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myCar_vin;
				public override int Car_vin
				{
					get
					{
						return this.myCar_vin;
					}
					set
					{
						if (!(object.Equals(this.Car_vin, value)))
						{
							if (this.Context.OnReviewCar_vinChanging(this, value))
							{
								if (base.RaiseCar_vinChangingEvent(value))
								{
									int oldValue = this.Car_vin;
									this.myCar_vin = value;
									this.Context.OnReviewCar_vinChanged(this, oldValue);
									base.RaiseCar_vinChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private int myRating_Nr_Integer;
				public override int Rating_Nr_Integer
				{
					get
					{
						return this.myRating_Nr_Integer;
					}
					set
					{
						if (!(object.Equals(this.Rating_Nr_Integer, value)))
						{
							if (this.Context.OnReviewRating_Nr_IntegerChanging(this, value))
							{
								if (base.RaiseRating_Nr_IntegerChangingEvent(value))
								{
									int oldValue = this.Rating_Nr_Integer;
									this.myRating_Nr_Integer = value;
									base.RaiseRating_Nr_IntegerChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myCriteria_Name;
				public override string Criteria_Name
				{
					get
					{
						return this.myCriteria_Name;
					}
					set
					{
						if (!(object.Equals(this.Criteria_Name, value)))
						{
							if (this.Context.OnReviewCriteria_NameChanging(this, value))
							{
								if (base.RaiseCriteria_NameChangingEvent(value))
								{
									string oldValue = this.Criteria_Name;
									this.myCriteria_Name = value;
									this.Context.OnReviewCriteria_NameChanged(this, oldValue);
									base.RaiseCriteria_NameChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // ReviewCore
			private bool OnPersonHasNickNameNickNameChanging(PersonHasNickName instance, string newValue)
			{
				if (!(this.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple(newValue, instance.Person))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonHasNickNameNickNameChanged(PersonHasNickName instance, string oldValue)
			{
				this.OnInternalUniquenessConstraint33Changed(instance, Tuple.CreateTuple(oldValue, instance.Person), Tuple.CreateTuple(instance.NickName, instance.Person));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonHasNickNamePersonChanging(PersonHasNickName instance, Person newValue)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (!(this.OnInternalUniquenessConstraint33Changing(instance, Tuple.CreateTuple(instance.NickName, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonHasNickNamePersonChanged(PersonHasNickName instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.PersonHasNickNameASPerson.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.PersonHasNickNameASPerson.Add(instance);
				}
				this.OnInternalUniquenessConstraint33Changed(instance, Tuple.CreateTuple(instance.NickName, oldValue), Tuple.CreateTuple(instance.NickName, instance.Person));
			}
			#region PersonHasNickNameCore
			private sealed class PersonHasNickNameCore : PersonHasNickName
			{
				public PersonHasNickNameCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
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
							if (this.Context.OnPersonHasNickNameNickNameChanging(this, value))
							{
								if (base.RaiseNickNameChangingEvent(value))
								{
									string oldValue = this.NickName;
									this.myNickName = value;
									this.Context.OnPersonHasNickNameNickNameChanged(this, oldValue);
									base.RaiseNickNameChangedEvent(oldValue);
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
							if (this.Context.OnPersonHasNickNamePersonChanging(this, value))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnPersonHasNickNamePersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // PersonHasNickNameCore
			private bool OnPersonFirstNameChanging(Person instance, string newValue)
			{
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(newValue, instance.Date_YMD))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonFirstNameChanged(Person instance, string oldValue)
			{
				this.OnExternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(oldValue, instance.Date_YMD), Tuple.CreateTuple(instance.FirstName, instance.Date_YMD));
			}
			private bool OnPersonDate_YMDChanging(Person instance, int newValue)
			{
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple(instance.FirstName, newValue))))
				{
					return false;
				}
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(instance.LastName, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonDate_YMDChanged(Person instance, int oldValue)
			{
				this.OnExternalUniquenessConstraint1Changed(instance, Tuple.CreateTuple(instance.FirstName, oldValue), Tuple.CreateTuple(instance.FirstName, instance.Date_YMD));
				this.OnExternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(instance.LastName, oldValue), Tuple.CreateTuple(instance.LastName, instance.Date_YMD));
			}
			private bool OnPersonLastNameChanging(Person instance, string newValue)
			{
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple(newValue, instance.Date_YMD))))
				{
					return false;
				}
				return true;
			}
			private void OnPersonLastNameChanged(Person instance, string oldValue)
			{
				this.OnExternalUniquenessConstraint2Changed(instance, Tuple.CreateTuple(oldValue, instance.Date_YMD), Tuple.CreateTuple(instance.LastName, instance.Date_YMD));
			}
			private bool OnPersonSocialSecurityNumberChanging(Person instance, string newValue)
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
				#pragma warning restore 0472
				#pragma warning disable 0472
				if (instance.SocialSecurityNumber != null)
				{
					this.myPersonSocialSecurityNumberDictionary.Add(instance.SocialSecurityNumber, instance);
				}
				#pragma warning restore 0472
			}
			private bool OnPersonHatType_ColorARGBChanging(Person instance, int newValue)
			{
				return true;
			}
			private bool OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(Person instance, string newValue)
			{
				return true;
			}
			private bool OnPersonOwnsCar_vinChanging(Person instance, int newValue)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this.myPersonOwnsCar_vinDictionary.TryGetValue(newValue, out currentInstance))
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
			private void OnPersonOwnsCar_vinChanged(Person instance, int oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myPersonOwnsCar_vinDictionary.Remove(oldValue);
				}
				#pragma warning restore 0472
				#pragma warning disable 0472
				if (instance.OwnsCar_vin != null)
				{
					this.myPersonOwnsCar_vinDictionary.Add(instance.OwnsCar_vin, instance);
				}
				#pragma warning restore 0472
			}
			private bool OnPersonGender_Gender_CodeChanging(Person instance, string newValue)
			{
				return true;
			}
			private bool OnPersonPersonHasParentsASPersonChanging(Person instance, bool newValue)
			{
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonValueType1Changing(Person instance, ValueType1 newValue)
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
			private void OnPersonValueType1Changed(Person instance, ValueType1 oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person.Remove(instance);
				}
				if (instance.ValueType1 != null)
				{
					instance.ValueType1.Person.Add(instance);
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonMalePersonChanging(Person instance, MalePerson newValue)
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
			private void OnPersonMalePersonChanged(Person instance, MalePerson oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				if (instance.MalePerson != null)
				{
					instance.MalePerson.Person = instance;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonFemalePersonChanging(Person instance, FemalePerson newValue)
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
			private void OnPersonFemalePersonChanged(Person instance, FemalePerson oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				if (instance.FemalePerson != null)
				{
					instance.FemalePerson.Person = instance;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonChildPersonChanging(Person instance, ChildPerson newValue)
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
			private void OnPersonChildPersonChanged(Person instance, ChildPerson oldValue)
			{
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				if (instance.ChildPerson != null)
				{
					instance.ChildPerson.Person = instance;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonDeathChanging(Person instance, Death newValue)
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
					oldValue.Person = null;
				}
				if (instance.Death != null)
				{
					instance.Death.Person = instance;
				}
			}
			private bool OnPersonPersonDrivesCarASDrivenByPersonAdding(Person instance, PersonDrivesCar value)
			{
				return true;
			}
			private void OnPersonPersonDrivesCarASDrivenByPersonAdded(Person instance, PersonDrivesCar value)
			{
				if (value != null)
				{
					value.DrivenByPerson = instance;
				}
			}
			private bool OnPersonPersonDrivesCarASDrivenByPersonRemoving(Person instance, PersonDrivesCar value)
			{
				return true;
			}
			private void OnPersonPersonDrivesCarASDrivenByPersonRemoved(Person instance, PersonDrivesCar value)
			{
				if (value != null)
				{
					value.DrivenByPerson = null;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateASBuyerAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateASBuyerAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Buyer = instance;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateASBuyerRemoving(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateASBuyerRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Buyer = null;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateASSellerAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateASSellerAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Seller = instance;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateASSellerRemoving(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateASSellerRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Seller = null;
				}
			}
			private bool OnPersonPersonHasNickNameASPersonAdding(Person instance, PersonHasNickName value)
			{
				return true;
			}
			private void OnPersonPersonHasNickNameASPersonAdded(Person instance, PersonHasNickName value)
			{
				if (value != null)
				{
					value.Person = instance;
				}
			}
			private bool OnPersonPersonHasNickNameASPersonRemoving(Person instance, PersonHasNickName value)
			{
				return true;
			}
			private void OnPersonPersonHasNickNameASPersonRemoved(Person instance, PersonHasNickName value)
			{
				if (value != null)
				{
					value.Person = null;
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
			private bool OnPersonValueType1Adding(Person instance, ValueType1 value)
			{
				return true;
			}
			private void OnPersonValueType1Added(Person instance, ValueType1 value)
			{
				if (value != null)
				{
					value.Person = instance;
				}
			}
			private bool OnPersonValueType1Removing(Person instance, ValueType1 value)
			{
				return true;
			}
			private void OnPersonValueType1Removed(Person instance, ValueType1 value)
			{
				if (value != null)
				{
					value.Person = null;
				}
			}
			#region PersonCore
			private sealed class PersonCore : Person
			{
				public PersonCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
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
							if (this.Context.OnPersonFirstNameChanging(this, value))
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
				private int myDate_YMD;
				public override int Date_YMD
				{
					get
					{
						return this.myDate_YMD;
					}
					set
					{
						if (!(object.Equals(this.Date_YMD, value)))
						{
							if (this.Context.OnPersonDate_YMDChanging(this, value))
							{
								if (base.RaiseDate_YMDChangingEvent(value))
								{
									int oldValue = this.Date_YMD;
									this.myDate_YMD = value;
									this.Context.OnPersonDate_YMDChanged(this, oldValue);
									base.RaiseDate_YMDChangedEvent(oldValue);
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
							if (this.Context.OnPersonLastNameChanging(this, value))
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
							if (this.Context.OnPersonSocialSecurityNumberChanging(this, value))
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
				private int myHatType_ColorARGB;
				public override int HatType_ColorARGB
				{
					get
					{
						return this.myHatType_ColorARGB;
					}
					set
					{
						if (!(object.Equals(this.HatType_ColorARGB, value)))
						{
							if (this.Context.OnPersonHatType_ColorARGBChanging(this, value))
							{
								if (base.RaiseHatType_ColorARGBChangingEvent(value))
								{
									int oldValue = this.HatType_ColorARGB;
									this.myHatType_ColorARGB = value;
									base.RaiseHatType_ColorARGBChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myHatType_HatTypeStyle_HatTypeStyle_Description;
				public override string HatType_HatTypeStyle_HatTypeStyle_Description
				{
					get
					{
						return this.myHatType_HatTypeStyle_HatTypeStyle_Description;
					}
					set
					{
						if (!(object.Equals(this.HatType_HatTypeStyle_HatTypeStyle_Description, value)))
						{
							if (this.Context.OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(this, value))
							{
								if (base.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(value))
								{
									string oldValue = this.HatType_HatTypeStyle_HatTypeStyle_Description;
									this.myHatType_HatTypeStyle_HatTypeStyle_Description = value;
									base.RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private int myOwnsCar_vin;
				public override int OwnsCar_vin
				{
					get
					{
						return this.myOwnsCar_vin;
					}
					set
					{
						if (!(object.Equals(this.OwnsCar_vin, value)))
						{
							if (this.Context.OnPersonOwnsCar_vinChanging(this, value))
							{
								if (base.RaiseOwnsCar_vinChangingEvent(value))
								{
									int oldValue = this.OwnsCar_vin;
									this.myOwnsCar_vin = value;
									this.Context.OnPersonOwnsCar_vinChanged(this, oldValue);
									base.RaiseOwnsCar_vinChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myGender_Gender_Code;
				public override string Gender_Gender_Code
				{
					get
					{
						return this.myGender_Gender_Code;
					}
					set
					{
						if (!(object.Equals(this.Gender_Gender_Code, value)))
						{
							if (this.Context.OnPersonGender_Gender_CodeChanging(this, value))
							{
								if (base.RaiseGender_Gender_CodeChangingEvent(value))
								{
									string oldValue = this.Gender_Gender_Code;
									this.myGender_Gender_Code = value;
									base.RaiseGender_Gender_CodeChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private bool myPersonHasParentsASPerson;
				public override bool PersonHasParentsASPerson
				{
					get
					{
						return this.myPersonHasParentsASPerson;
					}
					set
					{
						if (!(object.Equals(this.PersonHasParentsASPerson, value)))
						{
							if (this.Context.OnPersonPersonHasParentsASPersonChanging(this, value))
							{
								if (base.RaisePersonHasParentsASPersonChangingEvent(value))
								{
									bool oldValue = this.PersonHasParentsASPerson;
									this.myPersonHasParentsASPerson = value;
									base.RaisePersonHasParentsASPersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private ValueType1 myValueType1;
				public override ValueType1 ValueType1
				{
					get
					{
						return this.myValueType1;
					}
					set
					{
						if (!(object.Equals(this.ValueType1, value)))
						{
							if (this.Context.OnPersonValueType1Changing(this, value))
							{
								if (base.RaiseValueType1ChangingEvent(value))
								{
									ValueType1 oldValue = this.ValueType1;
									this.myValueType1 = value;
									this.Context.OnPersonValueType1Changed(this, oldValue);
									base.RaiseValueType1ChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private MalePerson myMalePerson;
				public override MalePerson MalePerson
				{
					get
					{
						return this.myMalePerson;
					}
					set
					{
						if (!(object.Equals(this.MalePerson, value)))
						{
							if (this.Context.OnPersonMalePersonChanging(this, value))
							{
								if (base.RaiseMalePersonChangingEvent(value))
								{
									MalePerson oldValue = this.MalePerson;
									this.myMalePerson = value;
									this.Context.OnPersonMalePersonChanged(this, oldValue);
									base.RaiseMalePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private FemalePerson myFemalePerson;
				public override FemalePerson FemalePerson
				{
					get
					{
						return this.myFemalePerson;
					}
					set
					{
						if (!(object.Equals(this.FemalePerson, value)))
						{
							if (this.Context.OnPersonFemalePersonChanging(this, value))
							{
								if (base.RaiseFemalePersonChangingEvent(value))
								{
									FemalePerson oldValue = this.FemalePerson;
									this.myFemalePerson = value;
									this.Context.OnPersonFemalePersonChanged(this, oldValue);
									base.RaiseFemalePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private ChildPerson myChildPerson;
				public override ChildPerson ChildPerson
				{
					get
					{
						return this.myChildPerson;
					}
					set
					{
						if (!(object.Equals(this.ChildPerson, value)))
						{
							if (this.Context.OnPersonChildPersonChanging(this, value))
							{
								if (base.RaiseChildPersonChangingEvent(value))
								{
									ChildPerson oldValue = this.ChildPerson;
									this.myChildPerson = value;
									this.Context.OnPersonChildPersonChanged(this, oldValue);
									base.RaiseChildPersonChangedEvent(oldValue);
								}
							}
						}
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
							if (this.Context.OnPersonDeathChanging(this, value))
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
				private readonly ICollection<PersonDrivesCar> myPersonDrivesCarASDrivenByPerson = new ConstraintEnforcementCollection<PersonDrivesCar>(this, new PotentialCollectionModificationCallback(this.Context.OnPersonPersonDrivesCarASDrivenByPersonAdding), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonDrivesCarASDrivenByPersonAdded), new PotentialCollectionModificationCallback(this.Context.OnPersonPersonDrivesCarASDrivenByPersonRemoving), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonDrivesCarASDrivenByPersonRemoved));
				public override ICollection<PersonDrivesCar> PersonDrivesCarASDrivenByPerson
				{
					get
					{
						return this.myPersonDrivesCarASDrivenByPerson;
					}
				}
				private readonly ICollection<PersonBoughtCarFromPersonOnDate> myPersonBoughtCarFromPersonOnDateASBuyer = new ConstraintEnforcementCollection<PersonBoughtCarFromPersonOnDate>(this, new PotentialCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASBuyerAdding), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASBuyerAdded), new PotentialCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASBuyerRemoving), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASBuyerRemoved));
				public override ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateASBuyer
				{
					get
					{
						return this.myPersonBoughtCarFromPersonOnDateASBuyer;
					}
				}
				private readonly ICollection<PersonBoughtCarFromPersonOnDate> myPersonBoughtCarFromPersonOnDateASSeller = new ConstraintEnforcementCollection<PersonBoughtCarFromPersonOnDate>(this, new PotentialCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASSellerAdding), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASSellerAdded), new PotentialCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASSellerRemoving), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonBoughtCarFromPersonOnDateASSellerRemoved));
				public override ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateASSeller
				{
					get
					{
						return this.myPersonBoughtCarFromPersonOnDateASSeller;
					}
				}
				private readonly ICollection<PersonHasNickName> myPersonHasNickNameASPerson = new ConstraintEnforcementCollection<PersonHasNickName>(this, new PotentialCollectionModificationCallback(this.Context.OnPersonPersonHasNickNameASPersonAdding), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonHasNickNameASPersonAdded), new PotentialCollectionModificationCallback(this.Context.OnPersonPersonHasNickNameASPersonRemoving), new CommittedCollectionModificationCallback(this.Context.OnPersonPersonHasNickNameASPersonRemoved));
				public override ICollection<PersonHasNickName> PersonHasNickNameASPerson
				{
					get
					{
						return this.myPersonHasNickNameASPerson;
					}
				}
				private readonly ICollection<Task> myTask = new ConstraintEnforcementCollection<Task>(this, new PotentialCollectionModificationCallback(this.Context.OnPersonTaskAdding), new CommittedCollectionModificationCallback(this.Context.OnPersonTaskAdded), new PotentialCollectionModificationCallback(this.Context.OnPersonTaskRemoving), new CommittedCollectionModificationCallback(this.Context.OnPersonTaskRemoved));
				public override ICollection<Task> Task
				{
					get
					{
						return this.myTask;
					}
				}
				private readonly ICollection<ValueType1> myValueType1 = new ConstraintEnforcementCollection<ValueType1>(this, new PotentialCollectionModificationCallback(this.Context.OnPersonValueType1Adding), new CommittedCollectionModificationCallback(this.Context.OnPersonValueType1Added), new PotentialCollectionModificationCallback(this.Context.OnPersonValueType1Removing), new CommittedCollectionModificationCallback(this.Context.OnPersonValueType1Removed));
				public override ICollection<ValueType1> ValueType1
				{
					get
					{
						return this.myValueType1;
					}
				}
			}
			#endregion // PersonCore
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnMalePersonPersonChanging(MalePerson instance, Person newValue)
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
			private void OnMalePersonPersonChanged(MalePerson instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.MalePerson = null;
				}
				if (instance.Person != null)
				{
					instance.Person.MalePerson = instance;
				}
			}
			private bool OnMalePersonChildPersonAdding(MalePerson instance, ChildPerson value)
			{
				return true;
			}
			private void OnMalePersonChildPersonAdded(MalePerson instance, ChildPerson value)
			{
				if (value != null)
				{
					value.Father = instance;
				}
			}
			private bool OnMalePersonChildPersonRemoving(MalePerson instance, ChildPerson value)
			{
				return true;
			}
			private void OnMalePersonChildPersonRemoved(MalePerson instance, ChildPerson value)
			{
				if (value != null)
				{
					value.Father = null;
				}
			}
			#region MalePersonCore
			private sealed class MalePersonCore : MalePerson
			{
				public MalePersonCore()
				{
				}
				private readonly SampleModelContext myContext;
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
							if (this.Context.OnMalePersonPersonChanging(this, value))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnMalePersonPersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private readonly ICollection<ChildPerson> myChildPerson = new ConstraintEnforcementCollection<ChildPerson>(this, new PotentialCollectionModificationCallback(this.Context.OnMalePersonChildPersonAdding), new CommittedCollectionModificationCallback(this.Context.OnMalePersonChildPersonAdded), new PotentialCollectionModificationCallback(this.Context.OnMalePersonChildPersonRemoving), new CommittedCollectionModificationCallback(this.Context.OnMalePersonChildPersonRemoved));
				public override ICollection<ChildPerson> ChildPerson
				{
					get
					{
						return this.myChildPerson;
					}
				}
			}
			#endregion // MalePersonCore
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnFemalePersonPersonChanging(FemalePerson instance, Person newValue)
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
			private void OnFemalePersonPersonChanged(FemalePerson instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.FemalePerson = null;
				}
				if (instance.Person != null)
				{
					instance.Person.FemalePerson = instance;
				}
			}
			private bool OnFemalePersonChildPersonAdding(FemalePerson instance, ChildPerson value)
			{
				return true;
			}
			private void OnFemalePersonChildPersonAdded(FemalePerson instance, ChildPerson value)
			{
				if (value != null)
				{
					value.Mother = instance;
				}
			}
			private bool OnFemalePersonChildPersonRemoving(FemalePerson instance, ChildPerson value)
			{
				return true;
			}
			private void OnFemalePersonChildPersonRemoved(FemalePerson instance, ChildPerson value)
			{
				if (value != null)
				{
					value.Mother = null;
				}
			}
			#region FemalePersonCore
			private sealed class FemalePersonCore : FemalePerson
			{
				public FemalePersonCore()
				{
				}
				private readonly SampleModelContext myContext;
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
							if (this.Context.OnFemalePersonPersonChanging(this, value))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnFemalePersonPersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private readonly ICollection<ChildPerson> myChildPerson = new ConstraintEnforcementCollection<ChildPerson>(this, new PotentialCollectionModificationCallback(this.Context.OnFemalePersonChildPersonAdding), new CommittedCollectionModificationCallback(this.Context.OnFemalePersonChildPersonAdded), new PotentialCollectionModificationCallback(this.Context.OnFemalePersonChildPersonRemoving), new CommittedCollectionModificationCallback(this.Context.OnFemalePersonChildPersonRemoved));
				public override ICollection<ChildPerson> ChildPerson
				{
					get
					{
						return this.myChildPerson;
					}
				}
			}
			#endregion // FemalePersonCore
			private bool OnChildPersonBirthOrder_BirthOrder_NrChanging(ChildPerson instance, int newValue)
			{
				if (!(this.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(instance.Father, newValue, instance.Mother))))
				{
					return false;
				}
				return true;
			}
			private void OnChildPersonBirthOrder_BirthOrder_NrChanged(ChildPerson instance, int oldValue)
			{
				this.OnExternalUniquenessConstraint3Changed(instance, Tuple.CreateTuple(instance.Father, oldValue, instance.Mother), Tuple.CreateTuple(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonFatherChanging(ChildPerson instance, MalePerson newValue)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (!(this.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(newValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother))))
				{
					return false;
				}
				return true;
			}
			private void OnChildPersonFatherChanged(ChildPerson instance, MalePerson oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ChildPerson.Remove(instance);
				}
				if (instance.Father != null)
				{
					instance.Father.ChildPerson.Add(instance);
				}
				this.OnExternalUniquenessConstraint3Changed(instance, Tuple.CreateTuple(oldValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother), Tuple.CreateTuple(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonMotherChanging(ChildPerson instance, FemalePerson newValue)
			{
				if (newValue != null)
				{
					if (this != newValue.Context)
					{
						throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
					}
				}
				if (!(this.OnExternalUniquenessConstraint3Changing(instance, Tuple.CreateTuple(instance.Father, instance.BirthOrder_BirthOrder_Nr, newValue))))
				{
					return false;
				}
				return true;
			}
			private void OnChildPersonMotherChanged(ChildPerson instance, FemalePerson oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ChildPerson.Remove(instance);
				}
				if (instance.Mother != null)
				{
					instance.Mother.ChildPerson.Add(instance);
				}
				this.OnExternalUniquenessConstraint3Changed(instance, Tuple.CreateTuple(instance.Father, instance.BirthOrder_BirthOrder_Nr, oldValue), Tuple.CreateTuple(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonPersonChanging(ChildPerson instance, Person newValue)
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
			private void OnChildPersonPersonChanged(ChildPerson instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ChildPerson = null;
				}
				if (instance.Person != null)
				{
					instance.Person.ChildPerson = instance;
				}
			}
			#region ChildPersonCore
			private sealed class ChildPersonCore : ChildPerson
			{
				public ChildPersonCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myBirthOrder_BirthOrder_Nr;
				public override int BirthOrder_BirthOrder_Nr
				{
					get
					{
						return this.myBirthOrder_BirthOrder_Nr;
					}
					set
					{
						if (!(object.Equals(this.BirthOrder_BirthOrder_Nr, value)))
						{
							if (this.Context.OnChildPersonBirthOrder_BirthOrder_NrChanging(this, value))
							{
								if (base.RaiseBirthOrder_BirthOrder_NrChangingEvent(value))
								{
									int oldValue = this.BirthOrder_BirthOrder_Nr;
									this.myBirthOrder_BirthOrder_Nr = value;
									this.Context.OnChildPersonBirthOrder_BirthOrder_NrChanged(this, oldValue);
									base.RaiseBirthOrder_BirthOrder_NrChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private MalePerson myFather;
				public override MalePerson Father
				{
					get
					{
						return this.myFather;
					}
					set
					{
						if (!(object.Equals(this.Father, value)))
						{
							if (this.Context.OnChildPersonFatherChanging(this, value))
							{
								if (base.RaiseFatherChangingEvent(value))
								{
									MalePerson oldValue = this.Father;
									this.myFather = value;
									this.Context.OnChildPersonFatherChanged(this, oldValue);
									base.RaiseFatherChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private FemalePerson myMother;
				public override FemalePerson Mother
				{
					get
					{
						return this.myMother;
					}
					set
					{
						if (!(object.Equals(this.Mother, value)))
						{
							if (this.Context.OnChildPersonMotherChanging(this, value))
							{
								if (base.RaiseMotherChangingEvent(value))
								{
									FemalePerson oldValue = this.Mother;
									this.myMother = value;
									this.Context.OnChildPersonMotherChanged(this, oldValue);
									base.RaiseMotherChangedEvent(oldValue);
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
							if (this.Context.OnChildPersonPersonChanging(this, value))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnChildPersonPersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
			}
			#endregion // ChildPersonCore
			private bool OnDeathDate_YMDChanging(Death instance, int newValue)
			{
				return true;
			}
			private bool OnDeathDeathCause_DeathCause_TypeChanging(Death instance, string newValue)
			{
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathNaturalDeathChanging(Death instance, NaturalDeath newValue)
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
					oldValue.Death = null;
				}
				if (instance.NaturalDeath != null)
				{
					instance.NaturalDeath.Death = instance;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathUnnaturalDeathChanging(Death instance, UnnaturalDeath newValue)
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
					oldValue.Death = null;
				}
				if (instance.UnnaturalDeath != null)
				{
					instance.UnnaturalDeath.Death = instance;
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathPersonChanging(Death instance, Person newValue)
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
					oldValue.Death = null;
				}
				if (instance.Person != null)
				{
					instance.Person.Death = instance;
				}
			}
			#region DeathCore
			private sealed class DeathCore : Death
			{
				public DeathCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myDate_YMD;
				public override int Date_YMD
				{
					get
					{
						return this.myDate_YMD;
					}
					set
					{
						if (!(object.Equals(this.Date_YMD, value)))
						{
							if (this.Context.OnDeathDate_YMDChanging(this, value))
							{
								if (base.RaiseDate_YMDChangingEvent(value))
								{
									int oldValue = this.Date_YMD;
									this.myDate_YMD = value;
									base.RaiseDate_YMDChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private string myDeathCause_DeathCause_Type;
				public override string DeathCause_DeathCause_Type
				{
					get
					{
						return this.myDeathCause_DeathCause_Type;
					}
					set
					{
						if (!(object.Equals(this.DeathCause_DeathCause_Type, value)))
						{
							if (this.Context.OnDeathDeathCause_DeathCause_TypeChanging(this, value))
							{
								if (base.RaiseDeathCause_DeathCause_TypeChangingEvent(value))
								{
									string oldValue = this.DeathCause_DeathCause_Type;
									this.myDeathCause_DeathCause_Type = value;
									base.RaiseDeathCause_DeathCause_TypeChangedEvent(oldValue);
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
							if (this.Context.OnDeathNaturalDeathChanging(this, value))
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
							if (this.Context.OnDeathUnnaturalDeathChanging(this, value))
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
							if (this.Context.OnDeathPersonChanging(this, value))
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
			}
			#endregion // DeathCore
			private bool OnNaturalDeathNaturalDeathIsFromProstateCancerASNaturalDeathChanging(NaturalDeath instance, bool newValue)
			{
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnNaturalDeathDeathChanging(NaturalDeath instance, Death newValue)
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
					oldValue.NaturalDeath = null;
				}
				if (instance.Death != null)
				{
					instance.Death.NaturalDeath = instance;
				}
			}
			#region NaturalDeathCore
			private sealed class NaturalDeathCore : NaturalDeath
			{
				public NaturalDeathCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private bool myNaturalDeathIsFromProstateCancerASNaturalDeath;
				public override bool NaturalDeathIsFromProstateCancerASNaturalDeath
				{
					get
					{
						return this.myNaturalDeathIsFromProstateCancerASNaturalDeath;
					}
					set
					{
						if (!(object.Equals(this.NaturalDeathIsFromProstateCancerASNaturalDeath, value)))
						{
							if (this.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerASNaturalDeathChanging(this, value))
							{
								if (base.RaiseNaturalDeathIsFromProstateCancerASNaturalDeathChangingEvent(value))
								{
									bool oldValue = this.NaturalDeathIsFromProstateCancerASNaturalDeath;
									this.myNaturalDeathIsFromProstateCancerASNaturalDeath = value;
									base.RaiseNaturalDeathIsFromProstateCancerASNaturalDeathChangedEvent(oldValue);
								}
							}
						}
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
							if (this.Context.OnNaturalDeathDeathChanging(this, value))
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
			#endregion // NaturalDeathCore
			private bool OnUnnaturalDeathUnnaturalDeathIsViolentASUnnaturalDeathChanging(UnnaturalDeath instance, bool newValue)
			{
				return true;
			}
			private bool OnUnnaturalDeathUnnaturalDeathIsBloodyASUnnaturalDeathChanging(UnnaturalDeath instance, bool newValue)
			{
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnUnnaturalDeathDeathChanging(UnnaturalDeath instance, Death newValue)
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
					oldValue.UnnaturalDeath = null;
				}
				if (instance.Death != null)
				{
					instance.Death.UnnaturalDeath = instance;
				}
			}
			#region UnnaturalDeathCore
			private sealed class UnnaturalDeathCore : UnnaturalDeath
			{
				public UnnaturalDeathCore()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private bool myUnnaturalDeathIsViolentASUnnaturalDeath;
				public override bool UnnaturalDeathIsViolentASUnnaturalDeath
				{
					get
					{
						return this.myUnnaturalDeathIsViolentASUnnaturalDeath;
					}
					set
					{
						if (!(object.Equals(this.UnnaturalDeathIsViolentASUnnaturalDeath, value)))
						{
							if (this.Context.OnUnnaturalDeathUnnaturalDeathIsViolentASUnnaturalDeathChanging(this, value))
							{
								if (base.RaiseUnnaturalDeathIsViolentASUnnaturalDeathChangingEvent(value))
								{
									bool oldValue = this.UnnaturalDeathIsViolentASUnnaturalDeath;
									this.myUnnaturalDeathIsViolentASUnnaturalDeath = value;
									base.RaiseUnnaturalDeathIsViolentASUnnaturalDeathChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private bool myUnnaturalDeathIsBloodyASUnnaturalDeath;
				public override bool UnnaturalDeathIsBloodyASUnnaturalDeath
				{
					get
					{
						return this.myUnnaturalDeathIsBloodyASUnnaturalDeath;
					}
					set
					{
						if (!(object.Equals(this.UnnaturalDeathIsBloodyASUnnaturalDeath, value)))
						{
							if (this.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyASUnnaturalDeathChanging(this, value))
							{
								if (base.RaiseUnnaturalDeathIsBloodyASUnnaturalDeathChangingEvent(value))
								{
									bool oldValue = this.UnnaturalDeathIsBloodyASUnnaturalDeath;
									this.myUnnaturalDeathIsBloodyASUnnaturalDeath = value;
									base.RaiseUnnaturalDeathIsBloodyASUnnaturalDeathChangedEvent(oldValue);
								}
							}
						}
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
							if (this.Context.OnUnnaturalDeathDeathChanging(this, value))
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
			#endregion // UnnaturalDeathCore
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnTaskPersonChanging(Task instance, Person newValue)
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
			#region TaskCore
			private sealed class TaskCore : Task
			{
				public TaskCore()
				{
				}
				private readonly SampleModelContext myContext;
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
							if (this.Context.OnTaskPersonChanging(this, value))
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
			#endregion // TaskCore
			private bool OnValueType1ValueType1ValueChanging(ValueType1 instance, int newValue)
			{
				#pragma warning disable 0472
				if (newValue != null)
				{
					ValueType1 currentInstance = instance;
					if (this.myValueType1ValueType1ValueDictionary.TryGetValue(newValue, out currentInstance))
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
			private void OnValueType1ValueType1ValueChanged(ValueType1 instance, int oldValue)
			{
				#pragma warning disable 0472
				if (oldValue != null)
				{
					this.myValueType1ValueType1ValueDictionary.Remove(oldValue);
				}
				#pragma warning restore 0472
				#pragma warning disable 0472
				if (instance.ValueType1Value != null)
				{
					this.myValueType1ValueType1ValueDictionary.Add(instance.ValueType1Value, instance);
				}
				#pragma warning restore 0472
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnValueType1PersonChanging(ValueType1 instance, Person newValue)
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
			private void OnValueType1PersonChanged(ValueType1 instance, Person oldValue)
			{
				if (oldValue != null)
				{
					oldValue.ValueType1.Remove(instance);
				}
				if (instance.Person != null)
				{
					instance.Person.ValueType1.Add(instance);
				}
			}
			private bool OnValueType1PersonAdding(ValueType1 instance, Person value)
			{
				return true;
			}
			private void OnValueType1PersonAdded(ValueType1 instance, Person value)
			{
				if (value != null)
				{
					value.ValueType1 = instance;
				}
			}
			private bool OnValueType1PersonRemoving(ValueType1 instance, Person value)
			{
				return true;
			}
			private void OnValueType1PersonRemoved(ValueType1 instance, Person value)
			{
				if (value != null)
				{
					value.ValueType1 = null;
				}
			}
			#region ValueType1Core
			private sealed class ValueType1Core : ValueType1
			{
				public ValueType1Core()
				{
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private int myValueType1Value;
				public override int ValueType1Value
				{
					get
					{
						return this.myValueType1Value;
					}
					set
					{
						if (!(object.Equals(this.ValueType1Value, value)))
						{
							if (this.Context.OnValueType1ValueType1ValueChanging(this, value))
							{
								if (base.RaiseValueType1ValueChangingEvent(value))
								{
									int oldValue = this.ValueType1Value;
									this.myValueType1Value = value;
									this.Context.OnValueType1ValueType1ValueChanged(this, oldValue);
									base.RaiseValueType1ValueChangedEvent(oldValue);
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
							if (this.Context.OnValueType1PersonChanging(this, value))
							{
								if (base.RaisePersonChangingEvent(value))
								{
									Person oldValue = this.Person;
									this.myPerson = value;
									this.Context.OnValueType1PersonChanged(this, oldValue);
									base.RaisePersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private readonly ICollection<Person> myPerson = new ConstraintEnforcementCollection<Person>(this, new PotentialCollectionModificationCallback(this.Context.OnValueType1PersonAdding), new CommittedCollectionModificationCallback(this.Context.OnValueType1PersonAdded), new PotentialCollectionModificationCallback(this.Context.OnValueType1PersonRemoving), new CommittedCollectionModificationCallback(this.Context.OnValueType1PersonRemoved));
				public override ICollection<Person> Person
				{
					get
					{
						return this.myPerson;
					}
				}
			}
			#endregion // ValueType1Core
		}
		#endregion // SampleModelContext
	}
}

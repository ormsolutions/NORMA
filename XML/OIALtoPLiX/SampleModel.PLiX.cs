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
			public event EventHandler<PropertyChangingEventArgs<Nullable<int>>> HatType_ColorARGBChanging
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
			protected bool RaiseHatType_ColorARGBChangingEvent(Nullable<int> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<int>>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Nullable<int>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<int>> eventArgs = new PropertyChangingEventArgs<Nullable<int>>(this.HatType_ColorARGB, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<int>>> HatType_ColorARGBChanged
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
			protected void RaiseHatType_ColorARGBChangedEvent(Nullable<int> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<int>>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<Nullable<int>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<int>>(oldValue, this.HatType_ColorARGB), new System.AsyncCallback(eventHandler.EndInvoke), null);
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
			public event EventHandler<PropertyChangingEventArgs<Nullable<int>>> OwnsCar_vinChanging
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
			protected bool RaiseOwnsCar_vinChangingEvent(Nullable<int> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<int>>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<Nullable<int>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<int>> eventArgs = new PropertyChangingEventArgs<Nullable<int>>(this.OwnsCar_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<int>>> OwnsCar_vinChanged
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
			protected void RaiseOwnsCar_vinChangedEvent(Nullable<int> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<int>>> eventHandler = this.Events[7] as EventHandler<PropertyChangedEventArgs<Nullable<int>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<int>>(oldValue, this.OwnsCar_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
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
			public event EventHandler<PropertyChangingEventArgs<Nullable<bool>>> PersonHasParentsChanging
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
			protected bool RaisePersonHasParentsChangingEvent(Nullable<bool> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<bool>>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<bool>> eventArgs = new PropertyChangingEventArgs<Nullable<bool>>(this.PersonHasParents, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<bool>>> PersonHasParentsChanged
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
			protected void RaisePersonHasParentsChangedEvent(Nullable<bool> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<bool>>> eventHandler = this.Events[9] as EventHandler<PropertyChangedEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<bool>>(oldValue, this.PersonHasParents), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("PersonHasParents");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<ValueType1>> ValueType1DoesSomethingElseWithChanging
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
			protected bool RaiseValueType1DoesSomethingElseWithChangingEvent(ValueType1 newValue)
			{
				EventHandler<PropertyChangingEventArgs<ValueType1>> eventHandler = this.Events[10] as EventHandler<PropertyChangingEventArgs<ValueType1>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<ValueType1> eventArgs = new PropertyChangingEventArgs<ValueType1>(this.ValueType1DoesSomethingElseWith, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<ValueType1>> ValueType1DoesSomethingElseWithChanged
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
			protected void RaiseValueType1DoesSomethingElseWithChangedEvent(ValueType1 oldValue)
			{
				EventHandler<PropertyChangedEventArgs<ValueType1>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<ValueType1>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<ValueType1>(oldValue, this.ValueType1DoesSomethingElseWith), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1DoesSomethingElseWith");
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
			public abstract Nullable<int> HatType_ColorARGB
			{
				get;
				set;
			}
			public abstract string HatType_HatTypeStyle_HatTypeStyle_Description
			{
				get;
				set;
			}
			public abstract Nullable<int> OwnsCar_vin
			{
				get;
				set;
			}
			public abstract string Gender_Gender_Code
			{
				get;
				set;
			}
			public abstract Nullable<bool> PersonHasParents
			{
				get;
				set;
			}
			public abstract ValueType1 ValueType1DoesSomethingElseWith
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
			public abstract ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
			{
				get;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
			{
				get;
			}
			public abstract ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
			{
				get;
			}
			public abstract ICollection<PersonHasNickName> PersonHasNickNameAsPerson
			{
				get;
			}
			public abstract ICollection<Task> Task
			{
				get;
			}
			public abstract ICollection<ValueType1> ValueType1DoesSomethingWith
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}SocialSecurityNumber = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}PersonHasParents = ""{10}"",{0}{1}ValueType1DoesSomethingElseWith = {11},{0}{1}MalePerson = {12},{0}{1}FemalePerson = {13},{0}{1}ChildPerson = {14},{0}{1}Death = {15}{0}}}", Environment.NewLine, "	", this.FirstName, this.Date_YMD, this.LastName, this.SocialSecurityNumber, this.HatType_ColorARGB, this.HatType_HatTypeStyle_HatTypeStyle_Description, this.OwnsCar_vin, this.Gender_Gender_Code, this.PersonHasParents, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
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
			public event EventHandler<PropertyChangingEventArgs<Nullable<int>>> Date_YMDChanging
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
			protected bool RaiseDate_YMDChangingEvent(Nullable<int> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<int>>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Nullable<int>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<int>> eventArgs = new PropertyChangingEventArgs<Nullable<int>>(this.Date_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<int>>> Date_YMDChanged
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
			protected void RaiseDate_YMDChangedEvent(Nullable<int> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<int>>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Nullable<int>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<int>>(oldValue, this.Date_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
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
			public abstract Nullable<int> Date_YMD
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
			public event EventHandler<PropertyChangingEventArgs<Nullable<bool>>> NaturalDeathIsFromProstateCancerChanging
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
			protected bool RaiseNaturalDeathIsFromProstateCancerChangingEvent(Nullable<bool> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<bool>>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<bool>> eventArgs = new PropertyChangingEventArgs<Nullable<bool>>(this.NaturalDeathIsFromProstateCancer, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<bool>>> NaturalDeathIsFromProstateCancerChanged
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
			protected void RaiseNaturalDeathIsFromProstateCancerChangedEvent(Nullable<bool> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<bool>>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<bool>>(oldValue, this.NaturalDeathIsFromProstateCancer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NaturalDeathIsFromProstateCancer");
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
			public abstract Nullable<bool> NaturalDeathIsFromProstateCancer
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
			public event EventHandler<PropertyChangingEventArgs<Nullable<bool>>> UnnaturalDeathIsViolentChanging
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
			protected bool RaiseUnnaturalDeathIsViolentChangingEvent(Nullable<bool> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<bool>>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<bool>> eventArgs = new PropertyChangingEventArgs<Nullable<bool>>(this.UnnaturalDeathIsViolent, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<bool>>> UnnaturalDeathIsViolentChanged
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
			protected void RaiseUnnaturalDeathIsViolentChangedEvent(Nullable<bool> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<bool>>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<bool>>(oldValue, this.UnnaturalDeathIsViolent), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeathIsViolent");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Nullable<bool>>> UnnaturalDeathIsBloodyChanging
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
			protected bool RaiseUnnaturalDeathIsBloodyChangingEvent(Nullable<bool> newValue)
			{
				EventHandler<PropertyChangingEventArgs<Nullable<bool>>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Nullable<bool>> eventArgs = new PropertyChangingEventArgs<Nullable<bool>>(this.UnnaturalDeathIsBloody, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Nullable<bool>>> UnnaturalDeathIsBloodyChanged
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
			protected void RaiseUnnaturalDeathIsBloodyChangedEvent(Nullable<bool> oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Nullable<bool>>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Nullable<bool>>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Nullable<bool>>(oldValue, this.UnnaturalDeathIsBloody), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeathIsBloody");
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
			public abstract Nullable<bool> UnnaturalDeathIsViolent
			{
				get;
				set;
			}
			public abstract Nullable<bool> UnnaturalDeathIsBloody
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
			public event EventHandler<PropertyChangingEventArgs<Person>> DoesSomethingWithPersonChanging
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
			protected bool RaiseDoesSomethingWithPersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<Person>(this.DoesSomethingWithPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> DoesSomethingWithPersonChanged
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
			protected void RaiseDoesSomethingWithPersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person>(oldValue, this.DoesSomethingWithPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DoesSomethingWithPerson");
				}
			}
			public abstract int ValueType1Value
			{
				get;
				set;
			}
			public abstract Person DoesSomethingWithPerson
			{
				get;
				set;
			}
			public abstract ICollection<Person> DoesSomethingElseWithPerson
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}DoesSomethingWithPerson = {3}{0}}}", Environment.NewLine, "	", this.ValueType1Value, "TODO: Recursively call ToString for customTypes...");
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
			Person GetPersonByOwnsCar_vin(Nullable<int> OwnsCar_vin);
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
				List<PersonDrivesCar> PersonDrivesCarList = new List<PersonDrivesCar>();
				this.myPersonDrivesCarList = PersonDrivesCarList;
				this.myPersonDrivesCarReadOnlyCollection = new ReadOnlyCollection<PersonDrivesCar>(PersonDrivesCarList);
				List<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateList = new List<PersonBoughtCarFromPersonOnDate>();
				this.myPersonBoughtCarFromPersonOnDateList = PersonBoughtCarFromPersonOnDateList;
				this.myPersonBoughtCarFromPersonOnDateReadOnlyCollection = new ReadOnlyCollection<PersonBoughtCarFromPersonOnDate>(PersonBoughtCarFromPersonOnDateList);
				List<Review> ReviewList = new List<Review>();
				this.myReviewList = ReviewList;
				this.myReviewReadOnlyCollection = new ReadOnlyCollection<Review>(ReviewList);
				List<PersonHasNickName> PersonHasNickNameList = new List<PersonHasNickName>();
				this.myPersonHasNickNameList = PersonHasNickNameList;
				this.myPersonHasNickNameReadOnlyCollection = new ReadOnlyCollection<PersonHasNickName>(PersonHasNickNameList);
				List<Person> PersonList = new List<Person>();
				this.myPersonList = PersonList;
				this.myPersonReadOnlyCollection = new ReadOnlyCollection<Person>(PersonList);
				List<MalePerson> MalePersonList = new List<MalePerson>();
				this.myMalePersonList = MalePersonList;
				this.myMalePersonReadOnlyCollection = new ReadOnlyCollection<MalePerson>(MalePersonList);
				List<FemalePerson> FemalePersonList = new List<FemalePerson>();
				this.myFemalePersonList = FemalePersonList;
				this.myFemalePersonReadOnlyCollection = new ReadOnlyCollection<FemalePerson>(FemalePersonList);
				List<ChildPerson> ChildPersonList = new List<ChildPerson>();
				this.myChildPersonList = ChildPersonList;
				this.myChildPersonReadOnlyCollection = new ReadOnlyCollection<ChildPerson>(ChildPersonList);
				List<Death> DeathList = new List<Death>();
				this.myDeathList = DeathList;
				this.myDeathReadOnlyCollection = new ReadOnlyCollection<Death>(DeathList);
				List<NaturalDeath> NaturalDeathList = new List<NaturalDeath>();
				this.myNaturalDeathList = NaturalDeathList;
				this.myNaturalDeathReadOnlyCollection = new ReadOnlyCollection<NaturalDeath>(NaturalDeathList);
				List<UnnaturalDeath> UnnaturalDeathList = new List<UnnaturalDeath>();
				this.myUnnaturalDeathList = UnnaturalDeathList;
				this.myUnnaturalDeathReadOnlyCollection = new ReadOnlyCollection<UnnaturalDeath>(UnnaturalDeathList);
				List<Task> TaskList = new List<Task>();
				this.myTaskList = TaskList;
				this.myTaskReadOnlyCollection = new ReadOnlyCollection<Task>(TaskList);
				List<ValueType1> ValueType1List = new List<ValueType1>();
				this.myValueType1List = ValueType1List;
				this.myValueType1ReadOnlyCollection = new ReadOnlyCollection<ValueType1>(ValueType1List);
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
			private readonly Dictionary<Tuple<int, Person>, PersonDrivesCar> myInternalUniquenessConstraint18Dictionary = new Dictionary<Tuple<int, Person>, PersonDrivesCar>();
			public PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson)
			{
				return this.myInternalUniquenessConstraint18Dictionary[Tuple.CreateTuple<int, Person>(DrivesCar_vin, DrivenByPerson)];
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
			public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller)
			{
				return this.myInternalUniquenessConstraint23Dictionary[Tuple.CreateTuple<Person, int, Person>(Buyer, CarSold_vin, Seller)];
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
			public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin)
			{
				return this.myInternalUniquenessConstraint24Dictionary[Tuple.CreateTuple<int, Person, int>(SaleDate_YMD, Seller, CarSold_vin)];
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
			public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer)
			{
				return this.myInternalUniquenessConstraint25Dictionary[Tuple.CreateTuple<int, int, Person>(CarSold_vin, SaleDate_YMD, Buyer)];
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
			public Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criteria_Name)
			{
				return this.myInternalUniquenessConstraint26Dictionary[Tuple.CreateTuple<int, string>(Car_vin, Criteria_Name)];
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
			public PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person)
			{
				return this.myInternalUniquenessConstraint33Dictionary[Tuple.CreateTuple<string, Person>(NickName, Person)];
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
			public ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother)
			{
				return this.myExternalUniquenessConstraint3Dictionary[Tuple.CreateTuple<MalePerson, int, FemalePerson>(Father, BirthOrder_BirthOrder_Nr, Mother)];
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
			public Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD)
			{
				return this.myExternalUniquenessConstraint1Dictionary[Tuple.CreateTuple<string, int>(FirstName, Date_YMD)];
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
			public Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD)
			{
				return this.myExternalUniquenessConstraint2Dictionary[Tuple.CreateTuple<string, int>(LastName, Date_YMD)];
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
			public Person GetPersonBySocialSecurityNumber(string SocialSecurityNumber)
			{
				return this.myPersonSocialSecurityNumberDictionary[SocialSecurityNumber];
			}
			private readonly Dictionary<Nullable<int>, Person> myPersonOwnsCar_vinDictionary = new Dictionary<Nullable<int>, Person>();
			public Person GetPersonByOwnsCar_vin(Nullable<int> OwnsCar_vin)
			{
				return this.myPersonOwnsCar_vinDictionary[OwnsCar_vin];
			}
			private readonly Dictionary<int, ValueType1> myValueType1ValueType1ValueDictionary = new Dictionary<int, ValueType1>();
			public ValueType1 GetValueType1ByValueType1Value(int ValueType1Value)
			{
				return this.myValueType1ValueType1ValueDictionary[ValueType1Value];
			}
			private bool OnPersonDrivesCarDrivesCar_vinChanging(PersonDrivesCar instance, int newValue)
			{
				if (instance != null)
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
				if (oldValue != null)
				{
					InternalUniquenessConstraint18OldValueTuple = Tuple.CreateTuple<int, Person>(oldValue.Value, instance.DrivenByPerson);
				}
				else
				{
					InternalUniquenessConstraint18OldValueTuple = null;
				}
				this.OnInternalUniquenessConstraint18Changed(instance, InternalUniquenessConstraint18OldValueTuple, Tuple.CreateTuple<int, Person>(instance.DrivesCar_vin, instance.DrivenByPerson));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonDrivesCarDrivenByPersonChanging(PersonDrivesCar instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				if (instance != null)
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
				if (oldValue != null)
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
			public PersonDrivesCar CreatePersonDrivesCar(int DrivesCar_vin, Person DrivenByPerson)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnPersonDrivesCarDrivesCar_vinChanging(null, DrivesCar_vin)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "DrivesCar_vin");
					}
					if (!(this.OnPersonDrivesCarDrivenByPersonChanging(null, DrivenByPerson)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "DrivenByPerson");
					}
				}
				return new PersonDrivesCarCore(this, DrivesCar_vin, DrivenByPerson);
			}
			private readonly List<PersonDrivesCar> myPersonDrivesCarList;
			private readonly ReadOnlyCollection<PersonDrivesCar> myPersonDrivesCarReadOnlyCollection;
			public ReadOnlyCollection<PersonDrivesCar> PersonDrivesCarCollection
			{
				get
				{
					return this.myPersonDrivesCarReadOnlyCollection;
				}
			}
			#region PersonDrivesCarCore
			private sealed class PersonDrivesCarCore : PersonDrivesCar
			{
				public PersonDrivesCarCore(SampleModelContext context, int DrivesCar_vin, Person DrivenByPerson)
				{
					this.myContext = context;
					this.myDrivesCar_vin = DrivesCar_vin;
					context.OnPersonDrivesCarDrivesCar_vinChanged(this, null);
					this.myDrivenByPerson = DrivenByPerson;
					context.OnPersonDrivesCarDrivenByPersonChanged(this, null);
					context.myPersonDrivesCarList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
				if (instance != null)
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
				if (oldValue != null)
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
				if (instance != null)
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
				if (oldValue != null)
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateBuyerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				if (instance != null)
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
				if (oldValue != null)
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonBoughtCarFromPersonOnDateSellerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				if (instance != null)
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
				if (oldValue != null)
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
			public PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(null, CarSold_vin)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "CarSold_vin");
					}
					if (!(this.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(null, SaleDate_YMD)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "SaleDate_YMD");
					}
					if (!(this.OnPersonBoughtCarFromPersonOnDateBuyerChanging(null, Buyer)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Buyer");
					}
					if (!(this.OnPersonBoughtCarFromPersonOnDateSellerChanging(null, Seller)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Seller");
					}
				}
				return new PersonBoughtCarFromPersonOnDateCore(this, CarSold_vin, SaleDate_YMD, Buyer, Seller);
			}
			private readonly List<PersonBoughtCarFromPersonOnDate> myPersonBoughtCarFromPersonOnDateList;
			private readonly ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> myPersonBoughtCarFromPersonOnDateReadOnlyCollection;
			public ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
			{
				get
				{
					return this.myPersonBoughtCarFromPersonOnDateReadOnlyCollection;
				}
			}
			#region PersonBoughtCarFromPersonOnDateCore
			private sealed class PersonBoughtCarFromPersonOnDateCore : PersonBoughtCarFromPersonOnDate
			{
				public PersonBoughtCarFromPersonOnDateCore(SampleModelContext context, int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller)
				{
					this.myContext = context;
					this.myCarSold_vin = CarSold_vin;
					context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(this, null);
					this.mySaleDate_YMD = SaleDate_YMD;
					context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(this, null);
					this.myBuyer = Buyer;
					context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, null);
					this.mySeller = Seller;
					context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, null);
					context.myPersonBoughtCarFromPersonOnDateList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
						if (value == null)
						{
							return;
						}
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
				if (instance != null)
				{
					if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple<int, string>(newValue, instance.Criteria_Name))))
					{
						return false;
					}
				}
				return true;
			}
			private void OnReviewCar_vinChanged(Review instance, Nullable<int> oldValue)
			{
				Tuple<int, string> InternalUniquenessConstraint26OldValueTuple;
				if (oldValue != null)
				{
					InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple<int, string>(oldValue.Value, instance.Criteria_Name);
				}
				else
				{
					InternalUniquenessConstraint26OldValueTuple = null;
				}
				this.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple<int, string>(instance.Car_vin, instance.Criteria_Name));
			}
			private bool OnReviewRating_Nr_IntegerChanging(Review instance, int newValue)
			{
				return true;
			}
			private bool OnReviewCriteria_NameChanging(Review instance, string newValue)
			{
				if (instance != null)
				{
					if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple<int, string>(instance.Car_vin, newValue))))
					{
						return false;
					}
				}
				return true;
			}
			private void OnReviewCriteria_NameChanged(Review instance, string oldValue)
			{
				Tuple<int, string> InternalUniquenessConstraint26OldValueTuple;
				if (oldValue != null)
				{
					InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple<int, string>(instance.Car_vin, oldValue);
				}
				else
				{
					InternalUniquenessConstraint26OldValueTuple = null;
				}
				this.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple<int, string>(instance.Car_vin, instance.Criteria_Name));
			}
			public Review CreateReview(int Car_vin, int Rating_Nr_Integer, string Criteria_Name)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnReviewCar_vinChanging(null, Car_vin)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Car_vin");
					}
					if (!(this.OnReviewRating_Nr_IntegerChanging(null, Rating_Nr_Integer)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Rating_Nr_Integer");
					}
					if (!(this.OnReviewCriteria_NameChanging(null, Criteria_Name)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Criteria_Name");
					}
				}
				return new ReviewCore(this, Car_vin, Rating_Nr_Integer, Criteria_Name);
			}
			private readonly List<Review> myReviewList;
			private readonly ReadOnlyCollection<Review> myReviewReadOnlyCollection;
			public ReadOnlyCollection<Review> ReviewCollection
			{
				get
				{
					return this.myReviewReadOnlyCollection;
				}
			}
			#region ReviewCore
			private sealed class ReviewCore : Review
			{
				public ReviewCore(SampleModelContext context, int Car_vin, int Rating_Nr_Integer, string Criteria_Name)
				{
					this.myContext = context;
					this.myCar_vin = Car_vin;
					context.OnReviewCar_vinChanged(this, null);
					this.myRating_Nr_Integer = Rating_Nr_Integer;
					this.myCriteria_Name = Criteria_Name;
					context.OnReviewCriteria_NameChanged(this, null);
					context.myReviewList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
				if (instance != null)
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
				if (oldValue != null)
				{
					InternalUniquenessConstraint33OldValueTuple = Tuple.CreateTuple<string, Person>(oldValue, instance.Person);
				}
				else
				{
					InternalUniquenessConstraint33OldValueTuple = null;
				}
				this.OnInternalUniquenessConstraint33Changed(instance, InternalUniquenessConstraint33OldValueTuple, Tuple.CreateTuple<string, Person>(instance.NickName, instance.Person));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonHasNickNamePersonChanging(PersonHasNickName instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				if (instance != null)
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
				if (oldValue != null)
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
			public PersonHasNickName CreatePersonHasNickName(string NickName, Person Person)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnPersonHasNickNameNickNameChanging(null, NickName)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "NickName");
					}
					if (!(this.OnPersonHasNickNamePersonChanging(null, Person)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Person");
					}
				}
				return new PersonHasNickNameCore(this, NickName, Person);
			}
			private readonly List<PersonHasNickName> myPersonHasNickNameList;
			private readonly ReadOnlyCollection<PersonHasNickName> myPersonHasNickNameReadOnlyCollection;
			public ReadOnlyCollection<PersonHasNickName> PersonHasNickNameCollection
			{
				get
				{
					return this.myPersonHasNickNameReadOnlyCollection;
				}
			}
			#region PersonHasNickNameCore
			private sealed class PersonHasNickNameCore : PersonHasNickName
			{
				public PersonHasNickNameCore(SampleModelContext context, string NickName, Person Person)
				{
					this.myContext = context;
					this.myNickName = NickName;
					context.OnPersonHasNickNameNickNameChanged(this, null);
					this.myPerson = Person;
					context.OnPersonHasNickNamePersonChanged(this, null);
					context.myPersonHasNickNameList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
						if (value == null)
						{
							return;
						}
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
				if (instance != null)
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
				if (oldValue != null)
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
				if (instance != null)
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
				if (oldValue != null)
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
				if (instance != null)
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
				if (oldValue != null)
				{
					ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple<string, int>(oldValue, instance.Date_YMD);
				}
				else
				{
					ExternalUniquenessConstraint2OldValueTuple = null;
				}
				this.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple<string, int>(instance.LastName, instance.Date_YMD));
			}
			private bool OnPersonSocialSecurityNumberChanging(Person instance, string newValue)
			{
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
				return true;
			}
			private void OnPersonSocialSecurityNumberChanged(Person instance, string oldValue)
			{
				if (instance.SocialSecurityNumber != null)
				{
					this.myPersonSocialSecurityNumberDictionary.Add(instance.SocialSecurityNumber, instance);
				}
				if (oldValue != null)
				{
					this.myPersonSocialSecurityNumberDictionary.Remove(oldValue);
				}
				else
				{
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
				return true;
			}
			private void OnPersonOwnsCar_vinChanged(Person instance, Nullable<int> oldValue)
			{
				if (instance.OwnsCar_vin != null)
				{
					this.myPersonOwnsCar_vinDictionary.Add(instance.OwnsCar_vin, instance);
				}
				if (oldValue != null)
				{
					this.myPersonOwnsCar_vinDictionary.Remove(oldValue);
				}
				else
				{
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnPersonValueType1DoesSomethingElseWithChanging(Person instance, ValueType1 newValue)
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
			private void OnPersonValueType1DoesSomethingElseWithChanged(Person instance, ValueType1 oldValue)
			{
				if (instance.ValueType1DoesSomethingElseWith != null)
				{
					instance.ValueType1DoesSomethingElseWith.DoesSomethingElseWithPerson.Add(instance);
				}
				if (oldValue != null)
				{
					oldValue.DoesSomethingElseWithPerson.Remove(instance);
				}
				else
				{
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
				if (instance.MalePerson != null)
				{
					instance.MalePerson.Person = instance;
				}
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				else
				{
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
				if (instance.FemalePerson != null)
				{
					instance.FemalePerson.Person = instance;
				}
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				else
				{
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
				if (instance.ChildPerson != null)
				{
					instance.ChildPerson.Person = instance;
				}
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				else
				{
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
				if (instance.Death != null)
				{
					instance.Death.Person = instance;
				}
				if (oldValue != null)
				{
					oldValue.Person = null;
				}
				else
				{
				}
			}
			private bool OnPersonPersonDrivesCarAsDrivenByPersonAdding(Person instance, PersonDrivesCar value)
			{
				return true;
			}
			private void OnPersonPersonDrivesCarAsDrivenByPersonAdded(Person instance, PersonDrivesCar value)
			{
				if (value != null)
				{
					value.DrivenByPerson = instance;
				}
			}
			private bool OnPersonPersonDrivesCarAsDrivenByPersonRemoving(Person instance, PersonDrivesCar value)
			{
				return true;
			}
			private void OnPersonPersonDrivesCarAsDrivenByPersonRemoved(Person instance, PersonDrivesCar value)
			{
				if (value != null)
				{
					value.DrivenByPerson = null;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Buyer = instance;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoving(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Buyer = null;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Seller = instance;
				}
			}
			private bool OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoving(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				return true;
			}
			private void OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
			{
				if (value != null)
				{
					value.Seller = null;
				}
			}
			private bool OnPersonPersonHasNickNameAsPersonAdding(Person instance, PersonHasNickName value)
			{
				return true;
			}
			private void OnPersonPersonHasNickNameAsPersonAdded(Person instance, PersonHasNickName value)
			{
				if (value != null)
				{
					value.Person = instance;
				}
			}
			private bool OnPersonPersonHasNickNameAsPersonRemoving(Person instance, PersonHasNickName value)
			{
				return true;
			}
			private void OnPersonPersonHasNickNameAsPersonRemoved(Person instance, PersonHasNickName value)
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
			private bool OnPersonValueType1DoesSomethingWithAdding(Person instance, ValueType1 value)
			{
				return true;
			}
			private void OnPersonValueType1DoesSomethingWithAdded(Person instance, ValueType1 value)
			{
				if (value != null)
				{
					value.DoesSomethingWithPerson = instance;
				}
			}
			private bool OnPersonValueType1DoesSomethingWithRemoving(Person instance, ValueType1 value)
			{
				return true;
			}
			private void OnPersonValueType1DoesSomethingWithRemoved(Person instance, ValueType1 value)
			{
				if (value != null)
				{
					value.DoesSomethingWithPerson = null;
				}
			}
			public Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnPersonFirstNameChanging(null, FirstName)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "FirstName");
					}
					if (!(this.OnPersonDate_YMDChanging(null, Date_YMD)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Date_YMD");
					}
					if (!(this.OnPersonLastNameChanging(null, LastName)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "LastName");
					}
					if (!(this.OnPersonGender_Gender_CodeChanging(null, Gender_Gender_Code)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Gender_Gender_Code");
					}
				}
				return new PersonCore(this, FirstName, Date_YMD, LastName, Gender_Gender_Code);
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
				public PersonCore(SampleModelContext context, string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code)
				{
					this.myContext = context;
					this.myPersonDrivesCarAsDrivenByPerson = new ConstraintEnforcementCollection<Person, PersonDrivesCar>(this, new PotentialCollectionModificationCallback<Person, PersonDrivesCar>(context.OnPersonPersonDrivesCarAsDrivenByPersonAdding), new CommittedCollectionModificationCallback<Person, PersonDrivesCar>(context.OnPersonPersonDrivesCarAsDrivenByPersonAdded), new PotentialCollectionModificationCallback<Person, PersonDrivesCar>(context.OnPersonPersonDrivesCarAsDrivenByPersonRemoving), new CommittedCollectionModificationCallback<Person, PersonDrivesCar>(context.OnPersonPersonDrivesCarAsDrivenByPersonRemoved));
					this.myPersonBoughtCarFromPersonOnDateAsBuyer = new ConstraintEnforcementCollection<Person, PersonBoughtCarFromPersonOnDate>(this, new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerAdded), new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoving), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsBuyerRemoved));
					this.myPersonBoughtCarFromPersonOnDateAsSeller = new ConstraintEnforcementCollection<Person, PersonBoughtCarFromPersonOnDate>(this, new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerAdded), new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoving), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(context.OnPersonPersonBoughtCarFromPersonOnDateAsSellerRemoved));
					this.myPersonHasNickNameAsPerson = new ConstraintEnforcementCollection<Person, PersonHasNickName>(this, new PotentialCollectionModificationCallback<Person, PersonHasNickName>(context.OnPersonPersonHasNickNameAsPersonAdding), new CommittedCollectionModificationCallback<Person, PersonHasNickName>(context.OnPersonPersonHasNickNameAsPersonAdded), new PotentialCollectionModificationCallback<Person, PersonHasNickName>(context.OnPersonPersonHasNickNameAsPersonRemoving), new CommittedCollectionModificationCallback<Person, PersonHasNickName>(context.OnPersonPersonHasNickNameAsPersonRemoved));
					this.myTask = new ConstraintEnforcementCollection<Person, Task>(this, new PotentialCollectionModificationCallback<Person, Task>(context.OnPersonTaskAdding), new CommittedCollectionModificationCallback<Person, Task>(context.OnPersonTaskAdded), new PotentialCollectionModificationCallback<Person, Task>(context.OnPersonTaskRemoving), new CommittedCollectionModificationCallback<Person, Task>(context.OnPersonTaskRemoved));
					this.myValueType1DoesSomethingWith = new ConstraintEnforcementCollection<Person, ValueType1>(this, new PotentialCollectionModificationCallback<Person, ValueType1>(context.OnPersonValueType1DoesSomethingWithAdding), new CommittedCollectionModificationCallback<Person, ValueType1>(context.OnPersonValueType1DoesSomethingWithAdded), new PotentialCollectionModificationCallback<Person, ValueType1>(context.OnPersonValueType1DoesSomethingWithRemoving), new CommittedCollectionModificationCallback<Person, ValueType1>(context.OnPersonValueType1DoesSomethingWithRemoved));
					this.myFirstName = FirstName;
					context.OnPersonFirstNameChanged(this, null);
					this.myDate_YMD = Date_YMD;
					context.OnPersonDate_YMDChanged(this, null);
					this.myLastName = LastName;
					context.OnPersonLastNameChanged(this, null);
					this.myGender_Gender_Code = Gender_Gender_Code;
					context.myPersonList.Add(this);
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
				private Nullable<int> myHatType_ColorARGB;
				public override Nullable<int> HatType_ColorARGB
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
									Nullable<int> oldValue = this.HatType_ColorARGB;
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
				private Nullable<int> myOwnsCar_vin;
				public override Nullable<int> OwnsCar_vin
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
									Nullable<int> oldValue = this.OwnsCar_vin;
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
						if (value == null)
						{
							return;
						}
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
				private Nullable<bool> myPersonHasParents;
				public override Nullable<bool> PersonHasParents
				{
					get
					{
						return this.myPersonHasParents;
					}
					set
					{
						if (!(object.Equals(this.PersonHasParents, value)))
						{
							if (this.Context.OnPersonPersonHasParentsChanging(this, value))
							{
								if (base.RaisePersonHasParentsChangingEvent(value))
								{
									Nullable<bool> oldValue = this.PersonHasParents;
									this.myPersonHasParents = value;
									base.RaisePersonHasParentsChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private ValueType1 myValueType1DoesSomethingElseWith;
				public override ValueType1 ValueType1DoesSomethingElseWith
				{
					get
					{
						return this.myValueType1DoesSomethingElseWith;
					}
					set
					{
						if (!(object.Equals(this.ValueType1DoesSomethingElseWith, value)))
						{
							if (this.Context.OnPersonValueType1DoesSomethingElseWithChanging(this, value))
							{
								if (base.RaiseValueType1DoesSomethingElseWithChangingEvent(value))
								{
									ValueType1 oldValue = this.ValueType1DoesSomethingElseWith;
									this.myValueType1DoesSomethingElseWith = value;
									this.Context.OnPersonValueType1DoesSomethingElseWithChanged(this, oldValue);
									base.RaiseValueType1DoesSomethingElseWithChangedEvent(oldValue);
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
				private readonly ICollection<PersonDrivesCar> myPersonDrivesCarAsDrivenByPerson;
				public override ICollection<PersonDrivesCar> PersonDrivesCarAsDrivenByPerson
				{
					get
					{
						return this.myPersonDrivesCarAsDrivenByPerson;
					}
				}
				private readonly ICollection<PersonBoughtCarFromPersonOnDate> myPersonBoughtCarFromPersonOnDateAsBuyer;
				public override ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsBuyer
				{
					get
					{
						return this.myPersonBoughtCarFromPersonOnDateAsBuyer;
					}
				}
				private readonly ICollection<PersonBoughtCarFromPersonOnDate> myPersonBoughtCarFromPersonOnDateAsSeller;
				public override ICollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateAsSeller
				{
					get
					{
						return this.myPersonBoughtCarFromPersonOnDateAsSeller;
					}
				}
				private readonly ICollection<PersonHasNickName> myPersonHasNickNameAsPerson;
				public override ICollection<PersonHasNickName> PersonHasNickNameAsPerson
				{
					get
					{
						return this.myPersonHasNickNameAsPerson;
					}
				}
				private readonly ICollection<Task> myTask;
				public override ICollection<Task> Task
				{
					get
					{
						return this.myTask;
					}
				}
				private readonly ICollection<ValueType1> myValueType1DoesSomethingWith;
				public override ICollection<ValueType1> ValueType1DoesSomethingWith
				{
					get
					{
						return this.myValueType1DoesSomethingWith;
					}
				}
			}
			#endregion // PersonCore
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnMalePersonPersonChanging(MalePerson instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				return true;
			}
			private void OnMalePersonPersonChanged(MalePerson instance, Person oldValue)
			{
				instance.Person.MalePerson = instance;
				if (oldValue != null)
				{
					oldValue.MalePerson = null;
				}
				else
				{
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
			public MalePerson CreateMalePerson(Person Person)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnMalePersonPersonChanging(null, Person)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Person");
					}
				}
				return new MalePersonCore(this, Person);
			}
			private readonly List<MalePerson> myMalePersonList;
			private readonly ReadOnlyCollection<MalePerson> myMalePersonReadOnlyCollection;
			public ReadOnlyCollection<MalePerson> MalePersonCollection
			{
				get
				{
					return this.myMalePersonReadOnlyCollection;
				}
			}
			#region MalePersonCore
			private sealed class MalePersonCore : MalePerson
			{
				public MalePersonCore(SampleModelContext context, Person Person)
				{
					this.myContext = context;
					this.myChildPerson = new ConstraintEnforcementCollection<MalePerson, ChildPerson>(this, new PotentialCollectionModificationCallback<MalePerson, ChildPerson>(context.OnMalePersonChildPersonAdding), new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(context.OnMalePersonChildPersonAdded), new PotentialCollectionModificationCallback<MalePerson, ChildPerson>(context.OnMalePersonChildPersonRemoving), new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(context.OnMalePersonChildPersonRemoved));
					this.myPerson = Person;
					context.OnMalePersonPersonChanged(this, null);
					context.myMalePersonList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
				private readonly ICollection<ChildPerson> myChildPerson;
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
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				return true;
			}
			private void OnFemalePersonPersonChanged(FemalePerson instance, Person oldValue)
			{
				instance.Person.FemalePerson = instance;
				if (oldValue != null)
				{
					oldValue.FemalePerson = null;
				}
				else
				{
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
			public FemalePerson CreateFemalePerson(Person Person)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnFemalePersonPersonChanging(null, Person)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Person");
					}
				}
				return new FemalePersonCore(this, Person);
			}
			private readonly List<FemalePerson> myFemalePersonList;
			private readonly ReadOnlyCollection<FemalePerson> myFemalePersonReadOnlyCollection;
			public ReadOnlyCollection<FemalePerson> FemalePersonCollection
			{
				get
				{
					return this.myFemalePersonReadOnlyCollection;
				}
			}
			#region FemalePersonCore
			private sealed class FemalePersonCore : FemalePerson
			{
				public FemalePersonCore(SampleModelContext context, Person Person)
				{
					this.myContext = context;
					this.myChildPerson = new ConstraintEnforcementCollection<FemalePerson, ChildPerson>(this, new PotentialCollectionModificationCallback<FemalePerson, ChildPerson>(context.OnFemalePersonChildPersonAdding), new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(context.OnFemalePersonChildPersonAdded), new PotentialCollectionModificationCallback<FemalePerson, ChildPerson>(context.OnFemalePersonChildPersonRemoving), new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(context.OnFemalePersonChildPersonRemoved));
					this.myPerson = Person;
					context.OnFemalePersonPersonChanged(this, null);
					context.myFemalePersonList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
				private readonly ICollection<ChildPerson> myChildPerson;
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
				if (instance != null)
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
				if (oldValue != null)
				{
					ExternalUniquenessConstraint3OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, oldValue.Value, instance.Mother);
				}
				else
				{
					ExternalUniquenessConstraint3OldValueTuple = null;
				}
				this.OnExternalUniquenessConstraint3Changed(instance, ExternalUniquenessConstraint3OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonFatherChanging(ChildPerson instance, MalePerson newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				if (instance != null)
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
				if (oldValue != null)
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonMotherChanging(ChildPerson instance, FemalePerson newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				if (instance != null)
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
				if (oldValue != null)
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
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnChildPersonPersonChanging(ChildPerson instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				return true;
			}
			private void OnChildPersonPersonChanged(ChildPerson instance, Person oldValue)
			{
				instance.Person.ChildPerson = instance;
				if (oldValue != null)
				{
					oldValue.ChildPerson = null;
				}
				else
				{
				}
			}
			public ChildPerson CreateChildPerson(int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnChildPersonBirthOrder_BirthOrder_NrChanging(null, BirthOrder_BirthOrder_Nr)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "BirthOrder_BirthOrder_Nr");
					}
					if (!(this.OnChildPersonFatherChanging(null, Father)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Father");
					}
					if (!(this.OnChildPersonMotherChanging(null, Mother)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Mother");
					}
					if (!(this.OnChildPersonPersonChanging(null, Person)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Person");
					}
				}
				return new ChildPersonCore(this, BirthOrder_BirthOrder_Nr, Father, Mother, Person);
			}
			private readonly List<ChildPerson> myChildPersonList;
			private readonly ReadOnlyCollection<ChildPerson> myChildPersonReadOnlyCollection;
			public ReadOnlyCollection<ChildPerson> ChildPersonCollection
			{
				get
				{
					return this.myChildPersonReadOnlyCollection;
				}
			}
			#region ChildPersonCore
			private sealed class ChildPersonCore : ChildPerson
			{
				public ChildPersonCore(SampleModelContext context, int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person)
				{
					this.myContext = context;
					this.myBirthOrder_BirthOrder_Nr = BirthOrder_BirthOrder_Nr;
					context.OnChildPersonBirthOrder_BirthOrder_NrChanged(this, null);
					this.myFather = Father;
					context.OnChildPersonFatherChanged(this, null);
					this.myMother = Mother;
					context.OnChildPersonMotherChanged(this, null);
					this.myPerson = Person;
					context.OnChildPersonPersonChanged(this, null);
					context.myChildPersonList.Add(this);
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
						if (value == null)
						{
							return;
						}
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
						if (value == null)
						{
							return;
						}
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
						if (value == null)
						{
							return;
						}
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
			private bool OnDeathDate_YMDChanging(Death instance, Nullable<int> newValue)
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
				if (instance.NaturalDeath != null)
				{
					instance.NaturalDeath.Death = instance;
				}
				if (oldValue != null)
				{
					oldValue.Death = null;
				}
				else
				{
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
				if (instance.UnnaturalDeath != null)
				{
					instance.UnnaturalDeath.Death = instance;
				}
				if (oldValue != null)
				{
					oldValue.Death = null;
				}
				else
				{
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnDeathPersonChanging(Death instance, Person newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				return true;
			}
			private void OnDeathPersonChanged(Death instance, Person oldValue)
			{
				instance.Person.Death = instance;
				if (oldValue != null)
				{
					oldValue.Death = null;
				}
				else
				{
				}
			}
			public Death CreateDeath(string DeathCause_DeathCause_Type, Person Person)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnDeathDeathCause_DeathCause_TypeChanging(null, DeathCause_DeathCause_Type)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "DeathCause_DeathCause_Type");
					}
					if (!(this.OnDeathPersonChanging(null, Person)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Person");
					}
				}
				return new DeathCore(this, DeathCause_DeathCause_Type, Person);
			}
			private readonly List<Death> myDeathList;
			private readonly ReadOnlyCollection<Death> myDeathReadOnlyCollection;
			public ReadOnlyCollection<Death> DeathCollection
			{
				get
				{
					return this.myDeathReadOnlyCollection;
				}
			}
			#region DeathCore
			private sealed class DeathCore : Death
			{
				public DeathCore(SampleModelContext context, string DeathCause_DeathCause_Type, Person Person)
				{
					this.myContext = context;
					this.myDeathCause_DeathCause_Type = DeathCause_DeathCause_Type;
					this.myPerson = Person;
					context.OnDeathPersonChanged(this, null);
					context.myDeathList.Add(this);
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Nullable<int> myDate_YMD;
				public override Nullable<int> Date_YMD
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
									Nullable<int> oldValue = this.Date_YMD;
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
						if (value == null)
						{
							return;
						}
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
						if (value == null)
						{
							return;
						}
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
			private bool OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(NaturalDeath instance, Nullable<bool> newValue)
			{
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnNaturalDeathDeathChanging(NaturalDeath instance, Death newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				return true;
			}
			private void OnNaturalDeathDeathChanged(NaturalDeath instance, Death oldValue)
			{
				instance.Death.NaturalDeath = instance;
				if (oldValue != null)
				{
					oldValue.NaturalDeath = null;
				}
				else
				{
				}
			}
			public NaturalDeath CreateNaturalDeath(Death Death)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnNaturalDeathDeathChanging(null, Death)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Death");
					}
				}
				return new NaturalDeathCore(this, Death);
			}
			private readonly List<NaturalDeath> myNaturalDeathList;
			private readonly ReadOnlyCollection<NaturalDeath> myNaturalDeathReadOnlyCollection;
			public ReadOnlyCollection<NaturalDeath> NaturalDeathCollection
			{
				get
				{
					return this.myNaturalDeathReadOnlyCollection;
				}
			}
			#region NaturalDeathCore
			private sealed class NaturalDeathCore : NaturalDeath
			{
				public NaturalDeathCore(SampleModelContext context, Death Death)
				{
					this.myContext = context;
					this.myDeath = Death;
					context.OnNaturalDeathDeathChanged(this, null);
					context.myNaturalDeathList.Add(this);
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Nullable<bool> myNaturalDeathIsFromProstateCancer;
				public override Nullable<bool> NaturalDeathIsFromProstateCancer
				{
					get
					{
						return this.myNaturalDeathIsFromProstateCancer;
					}
					set
					{
						if (!(object.Equals(this.NaturalDeathIsFromProstateCancer, value)))
						{
							if (this.Context.OnNaturalDeathNaturalDeathIsFromProstateCancerChanging(this, value))
							{
								if (base.RaiseNaturalDeathIsFromProstateCancerChangingEvent(value))
								{
									Nullable<bool> oldValue = this.NaturalDeathIsFromProstateCancer;
									this.myNaturalDeathIsFromProstateCancer = value;
									base.RaiseNaturalDeathIsFromProstateCancerChangedEvent(oldValue);
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
						if (value == null)
						{
							return;
						}
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
			private bool OnUnnaturalDeathUnnaturalDeathIsViolentChanging(UnnaturalDeath instance, Nullable<bool> newValue)
			{
				return true;
			}
			private bool OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(UnnaturalDeath instance, Nullable<bool> newValue)
			{
				return true;
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnUnnaturalDeathDeathChanging(UnnaturalDeath instance, Death newValue)
			{
				if (this != newValue.Context)
				{
					throw new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
				}
				return true;
			}
			private void OnUnnaturalDeathDeathChanged(UnnaturalDeath instance, Death oldValue)
			{
				instance.Death.UnnaturalDeath = instance;
				if (oldValue != null)
				{
					oldValue.UnnaturalDeath = null;
				}
				else
				{
				}
			}
			public UnnaturalDeath CreateUnnaturalDeath(Death Death)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnUnnaturalDeathDeathChanging(null, Death)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "Death");
					}
				}
				return new UnnaturalDeathCore(this, Death);
			}
			private readonly List<UnnaturalDeath> myUnnaturalDeathList;
			private readonly ReadOnlyCollection<UnnaturalDeath> myUnnaturalDeathReadOnlyCollection;
			public ReadOnlyCollection<UnnaturalDeath> UnnaturalDeathCollection
			{
				get
				{
					return this.myUnnaturalDeathReadOnlyCollection;
				}
			}
			#region UnnaturalDeathCore
			private sealed class UnnaturalDeathCore : UnnaturalDeath
			{
				public UnnaturalDeathCore(SampleModelContext context, Death Death)
				{
					this.myContext = context;
					this.myDeath = Death;
					context.OnUnnaturalDeathDeathChanged(this, null);
					context.myUnnaturalDeathList.Add(this);
				}
				private readonly SampleModelContext myContext;
				public override SampleModelContext Context
				{
					get
					{
						return this.myContext;
					}
				}
				private Nullable<bool> myUnnaturalDeathIsViolent;
				public override Nullable<bool> UnnaturalDeathIsViolent
				{
					get
					{
						return this.myUnnaturalDeathIsViolent;
					}
					set
					{
						if (!(object.Equals(this.UnnaturalDeathIsViolent, value)))
						{
							if (this.Context.OnUnnaturalDeathUnnaturalDeathIsViolentChanging(this, value))
							{
								if (base.RaiseUnnaturalDeathIsViolentChangingEvent(value))
								{
									Nullable<bool> oldValue = this.UnnaturalDeathIsViolent;
									this.myUnnaturalDeathIsViolent = value;
									base.RaiseUnnaturalDeathIsViolentChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private Nullable<bool> myUnnaturalDeathIsBloody;
				public override Nullable<bool> UnnaturalDeathIsBloody
				{
					get
					{
						return this.myUnnaturalDeathIsBloody;
					}
					set
					{
						if (!(object.Equals(this.UnnaturalDeathIsBloody, value)))
						{
							if (this.Context.OnUnnaturalDeathUnnaturalDeathIsBloodyChanging(this, value))
							{
								if (base.RaiseUnnaturalDeathIsBloodyChangingEvent(value))
								{
									Nullable<bool> oldValue = this.UnnaturalDeathIsBloody;
									this.myUnnaturalDeathIsBloody = value;
									base.RaiseUnnaturalDeathIsBloodyChangedEvent(oldValue);
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
						if (value == null)
						{
							return;
						}
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
				if (instance.Person != null)
				{
					instance.Person.Task.Add(instance);
				}
				if (oldValue != null)
				{
					oldValue.Task.Remove(instance);
				}
				else
				{
				}
			}
			public Task CreateTask()
			{
				if (!(this.IsDeserializing))
				{
				}
				return new TaskCore(this);
			}
			private readonly List<Task> myTaskList;
			private readonly ReadOnlyCollection<Task> myTaskReadOnlyCollection;
			public ReadOnlyCollection<Task> TaskCollection
			{
				get
				{
					return this.myTaskReadOnlyCollection;
				}
			}
			#region TaskCore
			private sealed class TaskCore : Task
			{
				public TaskCore(SampleModelContext context)
				{
					this.myContext = context;
					context.myTaskList.Add(this);
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
				ValueType1 currentInstance = instance;
				if (this.myValueType1ValueType1ValueDictionary.TryGetValue(newValue, out currentInstance))
				{
					if (!(object.Equals(currentInstance, instance)))
					{
						return false;
					}
				}
				return true;
			}
			private void OnValueType1ValueType1ValueChanged(ValueType1 instance, Nullable<int> oldValue)
			{
				this.myValueType1ValueType1ValueDictionary.Add(instance.ValueType1Value, instance);
				if (oldValue != null)
				{
					this.myValueType1ValueType1ValueDictionary.Remove(oldValue.Value);
				}
				else
				{
				}
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Usage", "CA2208")]
			[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
			private bool OnValueType1DoesSomethingWithPersonChanging(ValueType1 instance, Person newValue)
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
			private void OnValueType1DoesSomethingWithPersonChanged(ValueType1 instance, Person oldValue)
			{
				if (instance.DoesSomethingWithPerson != null)
				{
					instance.DoesSomethingWithPerson.ValueType1DoesSomethingWith.Add(instance);
				}
				if (oldValue != null)
				{
					oldValue.ValueType1DoesSomethingWith.Remove(instance);
				}
				else
				{
				}
			}
			private bool OnValueType1DoesSomethingElseWithPersonAdding(ValueType1 instance, Person value)
			{
				return true;
			}
			private void OnValueType1DoesSomethingElseWithPersonAdded(ValueType1 instance, Person value)
			{
				if (value != null)
				{
					value.ValueType1DoesSomethingElseWith = instance;
				}
			}
			private bool OnValueType1DoesSomethingElseWithPersonRemoving(ValueType1 instance, Person value)
			{
				return true;
			}
			private void OnValueType1DoesSomethingElseWithPersonRemoved(ValueType1 instance, Person value)
			{
				if (value != null)
				{
					value.ValueType1DoesSomethingElseWith = null;
				}
			}
			public ValueType1 CreateValueType1(int ValueType1Value)
			{
				if (!(this.IsDeserializing))
				{
					if (!(this.OnValueType1ValueType1ValueChanging(null, ValueType1Value)))
					{
						throw new ArgumentException("Argument failed constraint enforcement.", "ValueType1Value");
					}
				}
				return new ValueType1Core(this, ValueType1Value);
			}
			private readonly List<ValueType1> myValueType1List;
			private readonly ReadOnlyCollection<ValueType1> myValueType1ReadOnlyCollection;
			public ReadOnlyCollection<ValueType1> ValueType1Collection
			{
				get
				{
					return this.myValueType1ReadOnlyCollection;
				}
			}
			#region ValueType1Core
			private sealed class ValueType1Core : ValueType1
			{
				public ValueType1Core(SampleModelContext context, int ValueType1Value)
				{
					this.myContext = context;
					this.myDoesSomethingElseWithPerson = new ConstraintEnforcementCollection<ValueType1, Person>(this, new PotentialCollectionModificationCallback<ValueType1, Person>(context.OnValueType1DoesSomethingElseWithPersonAdding), new CommittedCollectionModificationCallback<ValueType1, Person>(context.OnValueType1DoesSomethingElseWithPersonAdded), new PotentialCollectionModificationCallback<ValueType1, Person>(context.OnValueType1DoesSomethingElseWithPersonRemoving), new CommittedCollectionModificationCallback<ValueType1, Person>(context.OnValueType1DoesSomethingElseWithPersonRemoved));
					this.myValueType1Value = ValueType1Value;
					context.OnValueType1ValueType1ValueChanged(this, null);
					context.myValueType1List.Add(this);
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
				private Person myDoesSomethingWithPerson;
				public override Person DoesSomethingWithPerson
				{
					get
					{
						return this.myDoesSomethingWithPerson;
					}
					set
					{
						if (!(object.Equals(this.DoesSomethingWithPerson, value)))
						{
							if (this.Context.OnValueType1DoesSomethingWithPersonChanging(this, value))
							{
								if (base.RaiseDoesSomethingWithPersonChangingEvent(value))
								{
									Person oldValue = this.DoesSomethingWithPerson;
									this.myDoesSomethingWithPerson = value;
									this.Context.OnValueType1DoesSomethingWithPersonChanged(this, oldValue);
									base.RaiseDoesSomethingWithPersonChangedEvent(oldValue);
								}
							}
						}
					}
				}
				private readonly ICollection<Person> myDoesSomethingElseWithPerson;
				public override ICollection<Person> DoesSomethingElseWithPerson
				{
					get
					{
						return this.myDoesSomethingElseWithPerson;
					}
				}
			}
			#endregion // ValueType1Core
		}
		#endregion // SampleModelContext
	}
}

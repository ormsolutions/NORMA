using System;
using System.Diagnostics;
// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/
namespace Neumont.Tools.ORM.ObjectModel
{
	#region PortableDataType Enum
	/// <summary>
	/// A list of predefined data types. One DataType-derived class is defined for each value.
	/// </summary>
	[CLSCompliant(true)]
	public enum PortableDataType
	{
		/// <summary>
		/// A default type indicating 'no type set'
		/// </summary>
		Unspecified,
		/// <summary>
		/// A fixed length text data type
		/// </summary>
		TextFixedLength,
		/// <summary>
		/// A variable length text data type
		/// </summary>
		TextVariableLength,
		/// <summary>
		/// A large length text data type
		/// </summary>
		TextLargeLength,
		/// <summary>
		/// A signed integer numeric data type
		/// </summary>
		NumericSignedInteger,
		/// <summary>
		/// An unsigned integer numeric data type
		/// </summary>
		NumericUnsignedInteger,
		/// <summary>
		/// An auto counter numeric data type
		/// </summary>
		NumericAutoCounter,
		/// <summary>
		/// A floating point numeric data type
		/// </summary>
		NumericFloatingPoint,
		/// <summary>
		/// A decimal numeric data type
		/// </summary>
		NumericDecimal,
		/// <summary>
		/// A money numeric data type
		/// </summary>
		NumericMoney,
		/// <summary>
		/// A fixed length raw data data type
		/// </summary>
		RawDataFixedLength,
		/// <summary>
		/// A variable length raw data data type
		/// </summary>
		RawDataVariableLength,
		/// <summary>
		/// A large length raw data data type
		/// </summary>
		RawDataLargeLength,
		/// <summary>
		/// A picture raw data data type
		/// </summary>
		RawDataPicture,
		/// <summary>
		/// An OLE object raw data data type
		/// </summary>
		RawDataOleObject,
		/// <summary>
		/// An auto timestamp temporal data type
		/// </summary>
		TemporalAutoTimestamp,
		/// <summary>
		/// A time temporal data type
		/// </summary>
		TemporalTime,
		/// <summary>
		/// A date temporal data type
		/// </summary>
		TemporalDate,
		/// <summary>
		/// A date and time temporal data type
		/// </summary>
		TemporalDateAndTime,
		/// <summary>
		/// A true or false logical data type
		/// </summary>
		LogicalTrueOrFalse,
		/// <summary>
		/// A yes or no logical data type
		/// </summary>
		LogicalYesOrNo,
		/// <summary>
		/// A row id data type (can not be classified in any of the groups above)
		/// </summary>
		OtherRowId,
		/// <summary>
		/// An object id data type (can not be classified in any of the groups above)
		/// </summary>
		OtherObjectId,
		/// <summary>
		/// Used for the upper bounds of the enum values.
		/// </summary>
		UserDefined,
	}
	#endregion // PortableDataType Enum
	#region Load-time fixup listeners
	public partial class ORMModel
	{
		private sealed partial class AddIntrinsicDataTypesFixupListener
		{
			private static Type[] typeArray = new Type[]{
				typeof(UnspecifiedDataType),
				typeof(FixedLengthTextDataType),
				typeof(VariableLengthTextDataType),
				typeof(LargeLengthTextDataType),
				typeof(SignedIntegerNumericDataType),
				typeof(UnsignedIntegerNumericDataType),
				typeof(AutoCounterNumericDataType),
				typeof(FloatingPointNumericDataType),
				typeof(DecimalNumericDataType),
				typeof(MoneyNumericDataType),
				typeof(FixedLengthRawDataDataType),
				typeof(VariableLengthRawDataDataType),
				typeof(LargeLengthRawDataDataType),
				typeof(PictureRawDataDataType),
				typeof(OleObjectRawDataDataType),
				typeof(AutoTimestampTemporalDataType),
				typeof(TimeTemporalDataType),
				typeof(DateTemporalDataType),
				typeof(DateAndTimeTemporalDataType),
				typeof(TrueOrFalseLogicalDataType),
				typeof(YesOrNoLogicalDataType),
				typeof(RowIdOtherDataType),
				typeof(ObjectIdOtherDataType)};
		}
	}
	#endregion // Load-time fixup listeners
}
#region Bind data types to enums and localized names
namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A default type indicating 'no type set'
	/// </summary>
	public partial class UnspecifiedDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.Unspecified;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeUnspecified;
		}
		/// <summary>
		/// The data type does not support comparison
		/// </summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// CanCompare is false. Compare asserts if called.
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanCompare returns false");
			return 0;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
	}
	/// <summary>
	/// A fixed length text data type
	/// </summary>
	public partial class FixedLengthTextDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TextFixedLength;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTextFixedLength;
		}
		/// <summary>
		/// The data type supports 'Closed' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Closed;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			return true;
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			return value1.CompareTo(value2);
		}
	}
	/// <summary>
	/// A variable length text data type
	/// </summary>
	public partial class VariableLengthTextDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TextVariableLength;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTextVariableLength;
		}
		/// <summary>
		/// The data type supports 'Closed' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Closed;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			return true;
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			return value1.CompareTo(value2);
		}
	}
	/// <summary>
	/// A large length text data type
	/// </summary>
	public partial class LargeLengthTextDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TextLargeLength;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTextLargeLength;
		}
		/// <summary>
		/// The data type supports 'Closed' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Closed;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			return true;
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			return value1.CompareTo(value2);
		}
	}
	/// <summary>
	/// A signed integer numeric data type
	/// </summary>
	public partial class SignedIntegerNumericDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericSignedInteger;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericSignedInteger;
		}
		/// <summary>
		/// The data type supports 'Closed' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Closed;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			int result;
			return int.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			int typedValue1;
			int.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			int typedValue2;
			int.TryParse(value2, out typedValue2);
			return ((IComparable<int>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// An unsigned integer numeric data type
	/// </summary>
	public partial class UnsignedIntegerNumericDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericUnsignedInteger;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericUnsignedInteger;
		}
		/// <summary>
		/// The data type supports 'Closed' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Closed;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			uint result;
			return uint.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			uint typedValue1;
			uint.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			uint typedValue2;
			uint.TryParse(value2, out typedValue2);
			return ((IComparable<uint>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// An auto counter numeric data type
	/// </summary>
	public partial class AutoCounterNumericDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericAutoCounter;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericAutoCounter;
		}
		/// <summary>
		/// The data type supports 'Closed' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Closed;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			ulong typedValue1;
			ulong.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			ulong typedValue2;
			ulong.TryParse(value2, out typedValue2);
			return ((IComparable<ulong>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A floating point numeric data type
	/// </summary>
	public partial class FloatingPointNumericDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericFloatingPoint;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericFloatingPoint;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			double result;
			return double.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			double typedValue1;
			double.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			double typedValue2;
			double.TryParse(value2, out typedValue2);
			return ((IComparable<double>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A decimal numeric data type
	/// </summary>
	public partial class DecimalNumericDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericDecimal;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericDecimal;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			decimal result;
			return decimal.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			decimal typedValue1;
			decimal.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			decimal typedValue2;
			decimal.TryParse(value2, out typedValue2);
			return ((IComparable<decimal>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A money numeric data type
	/// </summary>
	public partial class MoneyNumericDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericMoney;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericMoney;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			decimal result;
			return decimal.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			decimal typedValue1;
			decimal.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			decimal typedValue2;
			decimal.TryParse(value2, out typedValue2);
			return ((IComparable<decimal>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A fixed length raw data data type
	/// </summary>
	public partial class FixedLengthRawDataDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataFixedLength;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataFixedLength;
		}
		/// <summary>
		/// The data type does not support comparison
		/// </summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// CanCompare is false. Compare asserts if called.
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanCompare returns false");
			return 0;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>
	/// A variable length raw data data type
	/// </summary>
	public partial class VariableLengthRawDataDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataVariableLength;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataVariableLength;
		}
		/// <summary>
		/// The data type does not support comparison
		/// </summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// CanCompare is false. Compare asserts if called.
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanCompare returns false");
			return 0;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>
	/// A large length raw data data type
	/// </summary>
	public partial class LargeLengthRawDataDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataLargeLength;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataLargeLength;
		}
		/// <summary>
		/// The data type does not support comparison
		/// </summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// CanCompare is false. Compare asserts if called.
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanCompare returns false");
			return 0;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>
	/// A picture raw data data type
	/// </summary>
	public partial class PictureRawDataDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataPicture;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataPicture;
		}
		/// <summary>
		/// The data type does not support comparison
		/// </summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// CanCompare is false. Compare asserts if called.
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanCompare returns false");
			return 0;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>
	/// An OLE object raw data data type
	/// </summary>
	public partial class OleObjectRawDataDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataOleObject;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataOleObject;
		}
		/// <summary>
		/// The data type does not support comparison
		/// </summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// CanCompare is false. Compare asserts if called.
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanCompare returns false");
			return 0;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>
	/// An auto timestamp temporal data type
	/// </summary>
	public partial class AutoTimestampTemporalDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalAutoTimestamp;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalAutoTimestamp;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(value2, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A time temporal data type
	/// </summary>
	public partial class TimeTemporalDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalTime;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalTime;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(value2, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A date temporal data type
	/// </summary>
	public partial class DateTemporalDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalDate;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalDate;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(value2, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A date and time temporal data type
	/// </summary>
	public partial class DateAndTimeTemporalDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalDateAndTime;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalDateAndTime;
		}
		/// <summary>
		/// The data type supports 'Open' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.Open;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(value2, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>
	/// A true or false logical data type
	/// </summary>
	public partial class TrueOrFalseLogicalDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.LogicalTrueOrFalse;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeLogicalTrueOrFalse;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			bool result;
			return bool.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			bool typedValue1;
			bool.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			bool typedValue2;
			bool.TryParse(value2, out typedValue2);
			if (((IEquatable<bool>)typedValue1).Equals(typedValue2))
			{
				return 0;
			}
			return 1;
		}
	}
	/// <summary>
	/// A yes or no logical data type
	/// </summary>
	public partial class YesOrNoLogicalDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.LogicalYesOrNo;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeLogicalYesOrNo;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>
	/// A row id data type (can not be classified in any of the groups above)
	/// </summary>
	public partial class RowIdOtherDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.OtherRowId;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeOtherRowId;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			ulong typedValue1;
			ulong.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			ulong typedValue2;
			ulong.TryParse(value2, out typedValue2);
			if (((IEquatable<ulong>)typedValue1).Equals(typedValue2))
			{
				return 0;
			}
			return 1;
		}
	}
	/// <summary>
	/// An object id data type (can not be classified in any of the groups above)
	/// </summary>
	public partial class ObjectIdOtherDataType
	{
		/// <summary>
		/// PortableDataType enum value for this type
		/// </summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.OtherObjectId;
			}
		}
		/// <summary>
		/// Localized data type name
		/// </summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeOtherObjectId;
		}
		/// <summary>
		/// The data type supports 'None' ranges
		/// </summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>
		/// Returns true if the string value can be interpreted as this data type
		/// </summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, out result);
		}
		/// <summary>
		/// Compare two values. Each value should be checked previously with CanParse
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Assert(this.CanParse(value1), "Don't call Compare if CanCompare(value1) returns false");
			ulong typedValue1;
			ulong.TryParse(value1, out typedValue1);
			Debug.Assert(this.CanParse(value2), "Don't call Compare if CanCompare(value2) returns false");
			ulong typedValue2;
			ulong.TryParse(value2, out typedValue2);
			if (((IEquatable<ulong>)typedValue1).Equals(typedValue2))
			{
				return 0;
			}
			return 1;
		}
	}
}
#endregion // Bind data types to enums and localized names

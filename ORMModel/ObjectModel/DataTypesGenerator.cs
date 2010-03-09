using System;
using System.Diagnostics;
using System.Globalization;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// * Copyright © ORM Solutions, LLC. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region PortableDataType Enum
	/// <summary>A list of predefined data types. One DataType-derived class is defined for each value.</summary>
	public enum PortableDataType
	{
		/// <summary>A default type indicating 'no type set'</summary>
		Unspecified,
		/// <summary>A fixed length text data type</summary>
		TextFixedLength,
		/// <summary>A variable length text data type</summary>
		TextVariableLength,
		/// <summary>A large length text data type</summary>
		TextLargeLength,
		/// <summary>A signed integer numeric data type</summary>
		NumericSignedInteger,
		/// <summary>A small signed integer numeric data type</summary>
		NumericSignedSmallInteger,
		/// <summary>A small signed integer numeric data type</summary>
		NumericSignedLargeInteger,
		/// <summary>An unsigned integer numeric data type</summary>
		NumericUnsignedInteger,
		/// <summary>A tiny unsigned integer numeric data type</summary>
		NumericUnsignedTinyInteger,
		/// <summary>A small unsigned integer numeric data type</summary>
		NumericUnsignedSmallInteger,
		/// <summary>A large unsigned integer numeric data type</summary>
		NumericUnsignedLargeInteger,
		/// <summary>An auto counter numeric data type</summary>
		NumericAutoCounter,
		/// <summary>A custom precision floating point numeric data type</summary>
		NumericFloatingPoint,
		/// <summary>A 32-bit floating point numeric data type</summary>
		NumericSinglePrecisionFloatingPoint,
		/// <summary>A 64-bit floating point numeric data type</summary>
		NumericDoublePrecisionFloatingPoint,
		/// <summary>A decimal numeric data type</summary>
		NumericDecimal,
		/// <summary>A money numeric data type</summary>
		NumericMoney,
		/// <summary>A fixed length raw data data type</summary>
		RawDataFixedLength,
		/// <summary>A variable length raw data data type</summary>
		RawDataVariableLength,
		/// <summary>A large length raw data data type</summary>
		RawDataLargeLength,
		/// <summary>A picture raw data data type</summary>
		RawDataPicture,
		/// <summary>An OLE object raw data data type</summary>
		RawDataOleObject,
		/// <summary>An auto timestamp temporal data type</summary>
		TemporalAutoTimestamp,
		/// <summary>A time temporal data type</summary>
		TemporalTime,
		/// <summary>A date temporal data type</summary>
		TemporalDate,
		/// <summary>A date and time temporal data type</summary>
		TemporalDateAndTime,
		/// <summary>A true or false logical data type</summary>
		LogicalTrueOrFalse,
		/// <summary>A yes or no logical data type</summary>
		LogicalYesOrNo,
		/// <summary>A row id data type (can not be classified in any of the groups above)</summary>
		OtherRowId,
		/// <summary>An object id data type (can not be classified in any of the groups above)</summary>
		OtherObjectId,
		/// <summary>Used for the upper bounds of the enum values.</summary>
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
				typeof(SignedSmallIntegerNumericDataType),
				typeof(SignedLargeIntegerNumericDataType),
				typeof(UnsignedIntegerNumericDataType),
				typeof(UnsignedTinyIntegerNumericDataType),
				typeof(UnsignedSmallIntegerNumericDataType),
				typeof(UnsignedLargeIntegerNumericDataType),
				typeof(AutoCounterNumericDataType),
				typeof(FloatingPointNumericDataType),
				typeof(SinglePrecisionFloatingPointNumericDataType),
				typeof(DoublePrecisionFloatingPointNumericDataType),
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
namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	/// <summary>A default type indicating 'no type set'</summary>
	public partial class UnspecifiedDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.Unspecified;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeUnspecified;
		}
		/// <summary>The data type does not support comparison</summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>CanCompare is false. Compare asserts if called.</summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanParse returns false");
			return 0;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
	}
	/// <summary>A fixed length text data type</summary>
	public partial class FixedLengthTextDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TextFixedLength;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTextFixedLength;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			return invariantValue1.CompareTo(invariantValue2);
		}
		/// <summary>Show the Length property with this DataType</summary>
		public override string LengthName
		{
			get
			{
				return "";
			}
		}
		/// <summary>Show the description for the Length property for this DataType based on the 'DataTypeExactLengthDescription' resource string.</summary>
		public override string LengthDescription
		{
			get
			{
				return ResourceStrings.DataTypeExactLengthDescription;
			}
		}
	}
	/// <summary>A variable length text data type</summary>
	public partial class VariableLengthTextDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TextVariableLength;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTextVariableLength;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			return invariantValue1.CompareTo(invariantValue2);
		}
		/// <summary>Show the Length property with this DataType</summary>
		public override string LengthName
		{
			get
			{
				return "";
			}
		}
	}
	/// <summary>A large length text data type</summary>
	public partial class LargeLengthTextDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TextLargeLength;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTextLargeLength;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			return invariantValue1.CompareTo(invariantValue2);
		}
		/// <summary>Show the Length property with this DataType</summary>
		public override string LengthName
		{
			get
			{
				return "";
			}
		}
	}
	/// <summary>A signed integer numeric data type</summary>
	public partial class SignedIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericSignedInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericSignedInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			int result;
			return int.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			int result;
			return int.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			int typedValue;
			if (int.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			int typedValue;
			if (int.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			int typedValue1;
			int.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			int typedValue2;
			int.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<int>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				int bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!int.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == int.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				int bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!int.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == int.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>A small signed integer numeric data type</summary>
	public partial class SignedSmallIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericSignedSmallInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericSignedSmallInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			short result;
			return short.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			short result;
			return short.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			short typedValue;
			if (short.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			short typedValue;
			if (short.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			short typedValue1;
			short.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			short typedValue2;
			short.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<short>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				short bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!short.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == short.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				short bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!short.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == short.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>A small signed integer numeric data type</summary>
	public partial class SignedLargeIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericSignedLargeInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericSignedLargeInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			long result;
			return long.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			long result;
			return long.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			long typedValue;
			if (long.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			long typedValue;
			if (long.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			long typedValue1;
			long.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			long typedValue2;
			long.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<long>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				long bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!long.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == long.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				long bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!long.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == long.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>An unsigned integer numeric data type</summary>
	public partial class UnsignedIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericUnsignedInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericUnsignedInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			uint result;
			return uint.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			uint result;
			return uint.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			uint typedValue;
			if (uint.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			uint typedValue;
			if (uint.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			uint typedValue1;
			uint.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			uint typedValue2;
			uint.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<uint>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				uint bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!uint.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == uint.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				uint bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!uint.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == uint.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>A tiny unsigned integer numeric data type</summary>
	public partial class UnsignedTinyIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericUnsignedTinyInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericUnsignedTinyInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			byte result;
			return byte.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			byte result;
			return byte.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			byte typedValue;
			if (byte.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			byte typedValue;
			if (byte.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			byte typedValue1;
			byte.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			byte typedValue2;
			byte.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<byte>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				byte bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!byte.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == byte.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				byte bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!byte.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == byte.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>A small unsigned integer numeric data type</summary>
	public partial class UnsignedSmallIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericUnsignedSmallInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericUnsignedSmallInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			ushort result;
			return ushort.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			ushort result;
			return ushort.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			ushort typedValue;
			if (ushort.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			ushort typedValue;
			if (ushort.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			ushort typedValue1;
			ushort.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			ushort typedValue2;
			ushort.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<ushort>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				ushort bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!ushort.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == ushort.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				ushort bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!ushort.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == ushort.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>A large unsigned integer numeric data type</summary>
	public partial class UnsignedLargeIntegerNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericUnsignedLargeInteger;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericUnsignedLargeInteger;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			ulong result;
			return ulong.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			ulong typedValue;
			if (ulong.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			ulong typedValue;
			if (ulong.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			ulong typedValue1;
			ulong.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			ulong typedValue2;
			ulong.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<ulong>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				ulong bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!ulong.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == ulong.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				ulong bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!ulong.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == ulong.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>An auto counter numeric data type</summary>
	public partial class AutoCounterNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericAutoCounter;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericAutoCounter;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'DiscontinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.DiscontinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			ulong result;
			return ulong.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			ulong typedValue;
			if (ulong.TryParse(value, NumberStyles.Integer, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			ulong typedValue;
			if (ulong.TryParse(invariantValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			ulong typedValue1;
			ulong.TryParse(invariantValue1, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			ulong typedValue2;
			ulong.TryParse(invariantValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<ulong>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Adjust the lower bound for open ranges.</summary>
		public override bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				ulong bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!ulong.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == ulong.MaxValue)
				{
					return false;
				}
				invariantBound = (bound + 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
		/// <summary>Adjust the upper bound for open ranges.</summary>
		public override bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			if (isOpen)
			{
				ulong bound;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				if (!ulong.TryParse(invariantBound, NumberStyles.Integer, formatProvider, out bound) || bound == ulong.MinValue)
				{
					return false;
				}
				invariantBound = (bound - 1).ToString(formatProvider);
				isOpen = false;
			}
			return true;
		}
	}
	/// <summary>A custom precision floating point numeric data type</summary>
	public partial class FloatingPointNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericFloatingPoint;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericFloatingPoint;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			double result;
			return double.TryParse(value, NumberStyles.Float, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			double result;
			return double.TryParse(invariantValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			double typedValue;
			if (double.TryParse(value, NumberStyles.Float, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			double typedValue;
			if (double.TryParse(invariantValue, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			double typedValue1;
			double.TryParse(invariantValue1, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			double typedValue2;
			double.TryParse(invariantValue2, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<double>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Show the Length property for this DataType based on the 'DataTypePrecision' resource string.</summary>
		public override string LengthName
		{
			get
			{
				return ResourceStrings.DataTypePrecision;
			}
		}
		/// <summary>Show the description for the Length property for this DataType based on the 'DataTypePrecisionDescription' resource string.</summary>
		public override string LengthDescription
		{
			get
			{
				return ResourceStrings.DataTypePrecisionDescription;
			}
		}
	}
	/// <summary>A 32-bit floating point numeric data type</summary>
	public partial class SinglePrecisionFloatingPointNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericSinglePrecisionFloatingPoint;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericSinglePrecisionFloatingPoint;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			float result;
			return float.TryParse(value, NumberStyles.Float, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			float result;
			return float.TryParse(invariantValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			float typedValue;
			if (float.TryParse(value, NumberStyles.Float, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			float typedValue;
			if (float.TryParse(invariantValue, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			float typedValue1;
			float.TryParse(invariantValue1, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			float typedValue2;
			float.TryParse(invariantValue2, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<float>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>A 64-bit floating point numeric data type</summary>
	public partial class DoublePrecisionFloatingPointNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericDoublePrecisionFloatingPoint;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericDoublePrecisionFloatingPoint;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			double result;
			return double.TryParse(value, NumberStyles.Float, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			double result;
			return double.TryParse(invariantValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			double typedValue;
			if (double.TryParse(value, NumberStyles.Float, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			double typedValue;
			if (double.TryParse(invariantValue, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			double typedValue1;
			double.TryParse(invariantValue1, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			double typedValue2;
			double.TryParse(invariantValue2, NumberStyles.Float, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<double>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>A decimal numeric data type</summary>
	public partial class DecimalNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericDecimal;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericDecimal;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			decimal result;
			return decimal.TryParse(value, NumberStyles.Number, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			decimal result;
			return decimal.TryParse(invariantValue, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			decimal typedValue;
			if (decimal.TryParse(value, NumberStyles.Number, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			decimal typedValue;
			if (decimal.TryParse(invariantValue, NumberStyles.Number, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			decimal typedValue1;
			decimal.TryParse(invariantValue1, NumberStyles.Number, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			decimal typedValue2;
			decimal.TryParse(invariantValue2, NumberStyles.Number, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<decimal>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Show the Length property for this DataType based on the 'DataTypePrecision' resource string.</summary>
		public override string LengthName
		{
			get
			{
				return ResourceStrings.DataTypePrecision;
			}
		}
		/// <summary>Show the description for the Length property for this DataType based on the 'DataTypePrecisionDescription' resource string.</summary>
		public override string LengthDescription
		{
			get
			{
				return ResourceStrings.DataTypePrecisionDescription;
			}
		}
		/// <summary>Show the Scale property with this DataType</summary>
		public override string ScaleName
		{
			get
			{
				return "";
			}
		}
	}
	/// <summary>A money numeric data type</summary>
	public partial class MoneyNumericDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.NumericMoney;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeNumericMoney;
		}
		/// <summary>The string form of data for this data type is culture-dependent.</summary>
		public override bool IsCultureSensitive
		{
			get
			{
				return true;
			}
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			decimal result;
			return decimal.TryParse(value, NumberStyles.Number, this.CurrentCulture, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Returns true if the invariant string value can be interpreted as this data type</summary>
		public override bool CanParseInvariant(string invariantValue)
		{
			decimal result;
			return decimal.TryParse(invariantValue, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
		}
		/// <summary>Convert a culture-dependent string to an invariant string.</summary>
		public override bool TryConvertToInvariant(string value, out string invariantValue)
		{
			decimal typedValue;
			if (decimal.TryParse(value, NumberStyles.Number, this.CurrentCulture, out typedValue))
			{
				invariantValue = typedValue.ToString(CultureInfo.InvariantCulture);
				return true;
			}
			invariantValue = null;
			return false;
		}
		/// <summary>Convert an invariant string to a culture-dependent string.</summary>
		public override bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			decimal typedValue;
			if (decimal.TryParse(invariantValue, NumberStyles.Number, CultureInfo.InvariantCulture, out typedValue))
			{
				value = typedValue.ToString(this.CurrentCulture);
				return true;
			}
			value = null;
			return false;
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			decimal typedValue1;
			decimal.TryParse(invariantValue1, NumberStyles.Number, CultureInfo.InvariantCulture, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			decimal typedValue2;
			decimal.TryParse(invariantValue2, NumberStyles.Number, CultureInfo.InvariantCulture, out typedValue2);
			return ((IComparable<decimal>)typedValue1).CompareTo(typedValue2);
		}
		/// <summary>Show the Length property for this DataType based on the 'DataTypePrecision' resource string.</summary>
		public override string LengthName
		{
			get
			{
				return ResourceStrings.DataTypePrecision;
			}
		}
		/// <summary>Show the description for the Length property for this DataType based on the 'DataTypePrecisionDescription' resource string.</summary>
		public override string LengthDescription
		{
			get
			{
				return ResourceStrings.DataTypePrecisionDescription;
			}
		}
		/// <summary>Show the Scale property with this DataType</summary>
		public override string ScaleName
		{
			get
			{
				return "";
			}
		}
	}
	/// <summary>A fixed length raw data data type</summary>
	public partial class FixedLengthRawDataDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataFixedLength;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataFixedLength;
		}
		/// <summary>The data type does not support comparison</summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>CanCompare is false. Compare asserts if called.</summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanParse returns false");
			return 0;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>Show the Length property with this DataType</summary>
		public override string LengthName
		{
			get
			{
				return "";
			}
		}
		/// <summary>Show the description for the Length property for this DataType based on the 'DataTypeExactLengthDescription' resource string.</summary>
		public override string LengthDescription
		{
			get
			{
				return ResourceStrings.DataTypeExactLengthDescription;
			}
		}
	}
	/// <summary>A variable length raw data data type</summary>
	public partial class VariableLengthRawDataDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataVariableLength;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataVariableLength;
		}
		/// <summary>The data type does not support comparison</summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>CanCompare is false. Compare asserts if called.</summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanParse returns false");
			return 0;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>Show the Length property with this DataType</summary>
		public override string LengthName
		{
			get
			{
				return "";
			}
		}
	}
	/// <summary>A large length raw data data type</summary>
	public partial class LargeLengthRawDataDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataLargeLength;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataLargeLength;
		}
		/// <summary>The data type does not support comparison</summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>CanCompare is false. Compare asserts if called.</summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanParse returns false");
			return 0;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>Show the Length property with this DataType</summary>
		public override string LengthName
		{
			get
			{
				return "";
			}
		}
	}
	/// <summary>A picture raw data data type</summary>
	public partial class PictureRawDataDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataPicture;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataPicture;
		}
		/// <summary>The data type does not support comparison</summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>CanCompare is false. Compare asserts if called.</summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanParse returns false");
			return 0;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>An OLE object raw data data type</summary>
	public partial class OleObjectRawDataDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.RawDataOleObject;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeRawDataOleObject;
		}
		/// <summary>The data type does not support comparison</summary>
		public override bool CanCompare
		{
			get
			{
				return false;
			}
		}
		/// <summary>CanCompare is false. Compare asserts if called.</summary>
		public override int Compare(string value1, string value2)
		{
			Debug.Fail("Don't call Compare if CanParse returns false");
			return 0;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>An auto timestamp temporal data type</summary>
	public partial class AutoTimestampTemporalDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalAutoTimestamp;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalAutoTimestamp;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, this.CurrentCulture, DateTimeStyles.None, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(invariantValue1, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(invariantValue2, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>A time temporal data type</summary>
	public partial class TimeTemporalDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalTime;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalTime;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, this.CurrentCulture, DateTimeStyles.None, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(invariantValue1, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(invariantValue2, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>A date temporal data type</summary>
	public partial class DateTemporalDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalDate;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalDate;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, this.CurrentCulture, DateTimeStyles.None, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(invariantValue1, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(invariantValue2, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>A date and time temporal data type</summary>
	public partial class DateAndTimeTemporalDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.TemporalDateAndTime;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeTemporalDateAndTime;
		}
		/// <summary>The data type supports 'ContinuousEndPoints' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.ContinuousEndPoints;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			System.DateTime result;
			return System.DateTime.TryParse(value, this.CurrentCulture, DateTimeStyles.None, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			System.DateTime typedValue1;
			System.DateTime.TryParse(invariantValue1, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			System.DateTime typedValue2;
			System.DateTime.TryParse(invariantValue2, CultureInfo.InvariantCulture, DateTimeStyles.None, out typedValue2);
			return ((IComparable<System.DateTime>)typedValue1).CompareTo(typedValue2);
		}
	}
	/// <summary>A true or false logical data type</summary>
	public partial class TrueOrFalseLogicalDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.LogicalTrueOrFalse;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeLogicalTrueOrFalse;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			bool result;
			return bool.TryParse(value, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			bool typedValue1;
			bool.TryParse(invariantValue1, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			bool typedValue2;
			bool.TryParse(invariantValue2, out typedValue2);
			if (((IEquatable<bool>)typedValue1).Equals(typedValue2))
			{
				return 0;
			}
			return 1;
		}
	}
	/// <summary>A yes or no logical data type</summary>
	public partial class YesOrNoLogicalDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.LogicalYesOrNo;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeLogicalYesOrNo;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
	}
	/// <summary>A row id data type (can not be classified in any of the groups above)</summary>
	public partial class RowIdOtherDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.OtherRowId;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeOtherRowId;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			ulong typedValue1;
			ulong.TryParse(invariantValue1, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			ulong typedValue2;
			ulong.TryParse(invariantValue2, out typedValue2);
			if (((IEquatable<ulong>)typedValue1).Equals(typedValue2))
			{
				return 0;
			}
			return 1;
		}
	}
	/// <summary>An object id data type (can not be classified in any of the groups above)</summary>
	public partial class ObjectIdOtherDataType
	{
		/// <summary>PortableDataType enum value for this type</summary>
		public override PortableDataType PortableDataType
		{
			get
			{
				return PortableDataType.OtherObjectId;
			}
		}
		/// <summary>Localized data type name</summary>
		public override string ToString()
		{
			return ResourceStrings.PortableDataTypeOtherObjectId;
		}
		/// <summary>The data type supports 'None' ranges</summary>
		public override DataTypeRangeSupport RangeSupport
		{
			get
			{
				return DataTypeRangeSupport.None;
			}
		}
		/// <summary>Returns true if the string value can be interpreted as this data type</summary>
		public override bool CanParse(string value)
		{
			ulong result;
			return ulong.TryParse(value, out result);
		}
		/// <summary>Returns false, meaning that CanParse can fail for some values</summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>Compare two values. Each value should be checked previously with CanParse</summary>
		public override int Compare(string invariantValue1, string invariantValue2)
		{
			Debug.Assert(this.CanParseInvariant(invariantValue1), "Don't call Compare if CanParseInvariant(invariantValue1) returns false");
			ulong typedValue1;
			ulong.TryParse(invariantValue1, out typedValue1);
			Debug.Assert(this.CanParseInvariant(invariantValue2), "Don't call Compare if CanParseInvariant(invariantValue2) returns false");
			ulong typedValue2;
			ulong.TryParse(invariantValue2, out typedValue2);
			if (((IEquatable<ulong>)typedValue1).Equals(typedValue2))
			{
				return 0;
			}
			return 1;
		}
	}
}
#endregion // Bind data types to enums and localized names

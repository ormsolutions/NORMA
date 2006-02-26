using System;
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
	}
}
#endregion // Bind data types to enums and localized names

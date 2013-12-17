#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ORMModel class elements relating to DataType
	public partial class ORMModel
	{
		private DataType[] myPortableTypes;
		/// <summary>
		/// Get one of the known data types. Data types
		/// are created automatically during model deserialization.
		/// </summary>
		/// <param name="portableType">PortableDataType value</param>
		/// <returns>Known type, or throws if out of range</returns>
		public DataType GetPortableDataType(PortableDataType portableType)
		{
			return myPortableTypes[(int)portableType];
		}
		/// <summary>
		/// Return the current portable data type specified in the
		/// ORM Designer options page
		/// </summary>
		public DataType DefaultDataType
		{
			get
			{
				return myPortableTypes[(int)Shell.OptionsPage.CurrentDefaultDataType];
			}
		}
		#region Deserialization Fixup
		/// <summary>
		/// Return a fixup listener for data type deserialization.
		/// The listener adds the implicit DataType elements if they
		/// are not already in the model.
		/// </summary>
		public static IDeserializationFixupListener DataTypesFixupListener
		{
			get
			{
				return new AddIntrinsicDataTypesFixupListener((int)ORMDeserializationFixupPhase.AddIntrinsicElements);
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds intrinsic data type implementations.
		/// </summary>
		private sealed partial class AddIntrinsicDataTypesFixupListener : IDeserializationFixupListener
		{
			private readonly int myPhase;
			/// <summary>
			/// AddIntrinsicsFixupListener constructor
			/// </summary>
			/// <param name="intrinsicPhase">A phase constant to watch</param>
			public AddIntrinsicDataTypesFixupListener(int intrinsicPhase)
			{
				myPhase = intrinsicPhase;
			}
			#region IDeserializationFixupListener Implementation
			bool IDeserializationFixupListener.HasElements(int phase, Store store)
			{
				return false;
			}
			void IDeserializationFixupListener.ProcessElements(int phase, Store store, INotifyElementAdded notifyAdded)
			{
				Debug.Fail("Shouldn't be called if HasElements returns false");
			}
			void IDeserializationFixupListener.PhaseCompleted(int phase, Store store)
			{
				if (phase == myPhase)
				{
					foreach (ORMModel model in store.ElementDirectory.FindElements<ORMModel>())
					{
						int knownTypesCount = (int)PortableDataType.UserDefined;
						DataType[] knownTypes = new DataType[knownTypesCount];
						LinkedElementCollection<DataType> currentDataTypes = model.DataTypeCollection;
						int startingCount = currentDataTypes.Count;
						for (int i = 0; i < startingCount; ++i)
						{
							DataType existingType = currentDataTypes[i];
							int currentIndex = (int)existingType.PortableDataType;
							if (currentIndex >= 0 && currentIndex < knownTypesCount)
							{
								Debug.Assert(knownTypes[currentIndex] == null);
								knownTypes[currentIndex] = existingType;
							}
						}
						ElementFactory factory = store.ElementFactory;
						for (int i = 0; i < knownTypesCount; ++i)
						{
							if (null == knownTypes[i])
							{
								Type newType = null;
								if (i < typeArray.Length)
								{
									newType = typeArray[i];
								}
								Debug.Assert(newType != null);
								DomainObjectIdAttribute newTypeDomainObjectIdAttribute = (DomainObjectIdAttribute)newType.GetCustomAttributes(typeof(DomainObjectIdAttribute), false)[0];
								DataType newDataType = (DataType)factory.CreateElement(newTypeDomainObjectIdAttribute.Id);
								newDataType.Model = model;
								knownTypes[i] = newDataType;
							}
						}
						// Cache these for later use
						model.myPortableTypes = knownTypes;
					}
				}
			}
			#endregion // IDeserializationFixupListener Implementation
			#region INotifyElementAdded Implementation
			void INotifyElementAdded.ElementAdded(ModelElement element)
			{
				// Nothing to do
			}
			void INotifyElementAdded.ElementAdded(ModelElement element, bool addLinks)
			{
				Debug.Fail("Not used on the listeners");
			}
			#endregion // INotifyElementAdded Implementation
		}
		#endregion // Deserialization Fixup
	}
	#endregion // ORMModel class elements relating to DataType
	#region DataTypeRangeSupport enum
	/// <summary>
	/// Values indicating the range support for a data type
	/// </summary>
	public enum DataTypeRangeSupport
	{
		/// <summary>
		/// The data type does not support ranges
		/// </summary>
		None,
		/// <summary>
		/// The data type naturally supports open ranges
		/// with continuous values.
		/// </summary>
		ContinuousEndPoints,
		/// <summary>
		/// The data type does not naturally support open
		/// ranges. Open intervals endpoints are interpreted
		/// by adjusting the endpoint to the nearest value.
		/// For example, for an integer, an open lower
		/// bound from 1 is treated as a closed lower bound
		/// from 2.
		/// </summary>
		DiscontinuousEndPoints,
	}
	#endregion // DataTypeRangeSupport enum
	#region DataType class
	public abstract partial class DataType
	{
		#region Helper Methods and Properties
		/// <summary>
		/// Get the current format provider
		/// </summary>
		public CultureInfo CurrentCulture
		{
			get
			{
				// UNDONE: Consider storing a culture with the model, with a
				// user option to display and parse with either the native culture
				// or the model's stored culture.
				return CultureInfo.CurrentCulture;
			}
		}
		/// <summary>
		/// Determine the best text display value based on the
		/// current culture settings. If the <paramref name="value"/>
		/// is not the same as the <paramref name="invariantValue"/> according to
		/// the current culture data, then format and return the invariant text.
		/// </summary>
		/// <param name="value">Text formatted by some culture.</param>
		/// <param name="invariantValue">Text formatted to the invariant culture.</param>
		/// <returns>Best matching text.</returns>
		public string NormalizeDisplayText(string value, string invariantValue)
		{
			if (IsCultureSensitive)
			{
				string alternateForm;
				if (value.Length != 0)
				{
					// Can't fix data with no data
					if (invariantValue.Length != 0 &&
						(!TryConvertToInvariant(value, out alternateForm) ||
						alternateForm != invariantValue) &&
						TryConvertFromInvariant(invariantValue, out alternateForm))
					{
						return alternateForm;
					}
				}
				else if (invariantValue.Length != 0 &&
					TryConvertFromInvariant(invariantValue, out alternateForm))
				{
					return alternateForm;
				}
			}
			return value;
		}
		/// <summary>
		/// Determine if a string value or a cached invariant form of
		/// that value can be recognized by this data type.
		/// </summary>
		/// <param name="value">Text formatted by some culture.</param>
		/// <param name="invariantValue">Text formatted to the invariant culture.</param>
		/// <param name="normalizedValue">The normalized value. Use this value with other
		/// functions such as <see cref="Compare"/>, <see cref="AdjustDiscontinuousLowerBound"/>, and
		/// <see cref="AdjustDiscontinuousUpperBound"/></param>
		/// <returns>Best matching text.</returns>
		public bool ParseNormalizeValue(string value, string invariantValue, out string normalizedValue)
		{
			normalizedValue = value;
			if (CanParseAnyValue)
			{
				return true;
			}
			if (IsCultureSensitive)
			{
				if (value == invariantValue)
				{
					string alternateForm;
					if (TryConvertToInvariant(value, out alternateForm))
					{
						normalizedValue = alternateForm;
						return true;
					}
					else if (CanParseInvariant(value))
					{
						return true;
					}
					return false;
				}
				else if (invariantValue.Length != 0)
				{
					string alternateForm;
					if ((TryConvertToInvariant(value, out alternateForm) && alternateForm == invariantValue) ||
						CanParseInvariant(invariantValue))
					{
						normalizedValue = invariantValue;
						return true;
					}
					return false;
				}
			}
			return CanParse(value);
		}
		/// <summary>
		/// Test if an instance of one value type can be always be assigned to
		/// an instance of another value type.
		/// </summary>
		/// <param name="targetValueType">The type of the value being assigned to.</param>
		/// <param name="sourceValueType">The type of the value being assigned from.</param>
		/// <returns><see langword="true"/> if the source value can be assigned to the target value.</returns>
		/// <remarks>If either data type is unspecified, then the assumption is made that there is an error
		/// for this condition elsewhere and that the user will fix it, so this method returns true in these
		/// cases. Autogenerated values are assumed to be incompatible if they come from different value types
		/// because assigning autogenerated values to each other is also meaningingless.</remarks>
		public static bool IsAssignableValueType(ObjectType targetValueType, ObjectType sourceValueType)
		{
			if (targetValueType == sourceValueType)
			{
				return true;
			}
			ValueTypeHasDataType targetUse;
			ValueTypeHasDataType sourceUse;
			if (null == (targetUse = ValueTypeHasDataType.GetLinkToDataType(targetValueType)) ||
				null == (sourceUse = ValueTypeHasDataType.GetLinkToDataType(sourceValueType)))
			{
				return false;
			}
			PortableDataType targetType = targetUse.DataType.PortableDataType;
			PortableDataType sourceType = sourceUse.DataType.PortableDataType;
			if (sourceType == PortableDataType.Unspecified)
			{
				// Give user the benefit of the doubt that the types will be made compatible
				// if this choice is made.
				return true;
			}
			int targetLength;
			int sourceLength;
			int targetScale;
			int sourceScale;
			switch (targetType)
			{
				case PortableDataType.Unspecified:
					// Let user fix
					return true;
				case PortableDataType.TextFixedLength:
				case PortableDataType.TextVariableLength:
				case PortableDataType.TextLargeLength:
					switch (sourceType)
					{
						case PortableDataType.TextFixedLength:
						case PortableDataType.TextVariableLength:
						case PortableDataType.TextLargeLength:
							targetLength = targetUse.Length;
							sourceLength = sourceUse.Length;
							return (targetLength == 0 ? int.MaxValue : targetLength) >= (sourceLength == 0 ? int.MaxValue : sourceLength);
					}
					break;
				case PortableDataType.NumericSignedInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericSignedInteger:
						case PortableDataType.NumericSignedSmallInteger:
							return true;
						case PortableDataType.NumericDecimal:
							if (sourceUse.Scale == 0)
							{
								sourceLength = sourceUse.Length;
								if (sourceLength != 0 && sourceLength <= 9) // 10 might be too big
								{
									return true;
								}
							}
							break;
					}
					break;
				case PortableDataType.NumericSignedSmallInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericSignedSmallInteger:
							return true;
						case PortableDataType.NumericDecimal:
							if (sourceUse.Scale == 0)
							{
								sourceLength = sourceUse.Length;
								if (sourceLength != 0 && sourceLength <= 4) // 5 might be too big
								{
									return true;
								}
							}
							break;
					}
					break;
				case PortableDataType.NumericSignedLargeInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericSignedInteger:
						case PortableDataType.NumericSignedSmallInteger:
						case PortableDataType.NumericSignedLargeInteger:
							return true;
						case PortableDataType.NumericDecimal:
							if (sourceUse.Scale == 0)
							{
								sourceLength = sourceUse.Length;
								if (sourceLength != 0 && sourceLength <= 18) // 19 might be too big
								{
									return true;
								}
							}
							break;
					}
					break;
				case PortableDataType.NumericUnsignedInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericUnsignedInteger:
						case PortableDataType.NumericUnsignedTinyInteger:
						case PortableDataType.NumericUnsignedSmallInteger:
							return true;
					}
					break;
				case PortableDataType.NumericUnsignedTinyInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericUnsignedTinyInteger:
							return true;

					}
					break;
				case PortableDataType.NumericUnsignedSmallInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericUnsignedTinyInteger:
						case PortableDataType.NumericUnsignedSmallInteger:
							return true;
					}
					break;
				case PortableDataType.NumericUnsignedLargeInteger:
					switch (sourceType)
					{
						case PortableDataType.NumericUnsignedInteger:
						case PortableDataType.NumericUnsignedTinyInteger:
						case PortableDataType.NumericUnsignedSmallInteger:
						case PortableDataType.NumericUnsignedLargeInteger:
							return true;
					}
					break;
				//case PortableDataType.NumericAutoCounter:
				// Only support this if the value types are the same. Otherwise, we're mapping different
				// counters to each other, which is meaningless.
				case PortableDataType.NumericFloatingPoint:
				case PortableDataType.NumericSinglePrecisionFloatingPoint:
				case PortableDataType.NumericDoublePrecisionFloatingPoint:
					switch (sourceType)
					{
						case PortableDataType.NumericFloatingPoint:
							sourceLength = sourceUse.Length;
							if (sourceLength == 0)
							{
								sourceLength = 53;
							}
							break;
						case PortableDataType.NumericSinglePrecisionFloatingPoint:
							sourceLength = 24;
							break;
						case PortableDataType.NumericDoublePrecisionFloatingPoint:
							sourceLength = 53;
							break;
						default:
							sourceLength = -1;
							break;
					}
					if (sourceLength != -1)
					{
						switch (targetType)
						{
							case PortableDataType.NumericSinglePrecisionFloatingPoint:
								targetLength = 24;
								break;
							case PortableDataType.NumericDoublePrecisionFloatingPoint:
								targetLength = 53;
								break;
							//case PortableDataType.NumericFloatingPoint:
							default:
								targetLength = targetUse.Length;
								if (targetLength == 0)
								{
									targetLength = 53;
								}
								break;
						}
						if (targetLength >= sourceLength)
						{
							return true;
						}
					}
					break;
				case PortableDataType.NumericDecimal:
				case PortableDataType.NumericMoney:
					sourceScale = 0;
					switch (sourceType)
					{
						case PortableDataType.NumericDecimal:
							sourceLength = sourceUse.Length;
							if (sourceLength == 0)
							{
								sourceLength = 18;
							}
							else
							{
								sourceScale = sourceUse.Scale;
							}
							break;
						case PortableDataType.NumericMoney:
							sourceLength = 19;
							sourceScale = 4;
							break;
						case PortableDataType.NumericSignedInteger:
						case PortableDataType.NumericUnsignedInteger:
							sourceLength = 10;
							break;
						case PortableDataType.NumericSignedSmallInteger:
						case PortableDataType.NumericUnsignedSmallInteger:
							sourceLength = 5;
							break;
						case PortableDataType.NumericSignedLargeInteger:
							sourceLength = 19;
							break;
						case PortableDataType.NumericUnsignedLargeInteger:
							sourceLength = 20;
							break;
						case PortableDataType.NumericUnsignedTinyInteger:
							sourceLength = 3;
							break;
						default:
							sourceLength = -1;
							break;
					}
					if (sourceLength != -1)
					{
						if (targetType == PortableDataType.NumericMoney)
						{
							targetLength = 19;
							targetScale = 4;
						}
						else
						{
							targetLength = targetUse.Length; // Length maps to precision, verify precision first, then scale
							if (targetLength == 0)
							{
								targetLength = 18;
								targetScale = 0;
							}
							else
							{
								targetScale = targetUse.Scale;
							}
						}
						if (targetLength >= sourceLength && targetScale >= sourceScale)
						{
							return true;
						}
					}
					break;

				case PortableDataType.RawDataFixedLength:
				case PortableDataType.RawDataVariableLength:
				case PortableDataType.RawDataLargeLength:
					switch (sourceType)
					{
						case PortableDataType.RawDataFixedLength:
						case PortableDataType.RawDataVariableLength:
						case PortableDataType.RawDataLargeLength:
							targetLength = targetUse.Length;
							sourceLength = sourceUse.Length;
							return (targetLength == 0 ? int.MaxValue : targetLength) >= (sourceLength == 0 ? int.MaxValue : sourceLength);
					}
					break;
				case PortableDataType.RawDataPicture:
				case PortableDataType.RawDataOleObject:
				case PortableDataType.TemporalTime:
				case PortableDataType.TemporalDate:
				case PortableDataType.OtherRowId:
				case PortableDataType.OtherObjectId:
					if (sourceType == targetType)
					{
						return true;
					}
					break;
				case PortableDataType.TemporalAutoTimestamp:
				case PortableDataType.TemporalDateAndTime:
					switch (sourceType)
					{
						case PortableDataType.TemporalAutoTimestamp:
						case PortableDataType.TemporalTime:
						case PortableDataType.TemporalDate:
						case PortableDataType.TemporalDateAndTime:
							return true;
					}
					break;
				case PortableDataType.LogicalTrueOrFalse:
				case PortableDataType.LogicalYesOrNo:
					switch (sourceType)
					{
						case PortableDataType.LogicalTrueOrFalse:
						case PortableDataType.LogicalYesOrNo:
							return true;
					}
					break;
			}
			return false;
		}
		/// <summary>
		/// Check if two data types can be compared. This is a weaker check
		/// than <see cref="IsAssignableValueType"/> in that it does not
		/// do any facet checking to see if one value can store the only,
		/// but only checks if the two can be compared.
		/// </summary>
		/// <param name="valueType1">The first value type to compare.</param>
		/// <param name="valueType2">The second value type to compare.</param>
		/// <param name="equalityOnly">Set to true if the equality or inequality
		/// of the two values is required, but an ordered check is not.</param>
		/// <returns><see langword="true"/> if the two types can be compared.</returns>
		public static bool IsComparableValueType(ObjectType valueType1, ObjectType valueType2, bool equalityOnly)
		{
			DataType dataType1 = valueType1.DataType;
			DataType dataType2 = valueType2.DataType;
			if (dataType1 == dataType2)
			{
				switch (dataType1.PortableDataType)
				{
					case PortableDataType.Unspecified:
						// Give the user the benefit of the doubt that fixing the
						// unspecified data type will result in comparable values.
						return true;
					case PortableDataType.NumericAutoCounter:
						// Only support this if the value types are the same. Otherwise, we're mapping different
						// counters to each other, which is meaningless.
						return valueType1 == valueType2;
					default:
						return dataType1.CanCompare && (equalityOnly || (dataType1.RangeSupport != DataTypeRangeSupport.None));
				}
			}
			else if (dataType1.CanCompare &&
				dataType2.CanCompare &&
				(equalityOnly || (dataType1.RangeSupport != DataTypeRangeSupport.None && dataType2.RangeSupport != DataTypeRangeSupport.None)))
			{
				// Look at specific types. 
				PortableDataType targetType = dataType1.PortableDataType;
				PortableDataType sourceType = dataType2.PortableDataType;
				if (sourceType == PortableDataType.Unspecified)
				{
					// Give user the benefit of the doubt that the types will be made comparable
					// if this choice is made.
					return true;
				}
				switch (targetType)
				{
					case PortableDataType.Unspecified:
						// Let user fix
						return true;
					case PortableDataType.TextFixedLength:
					case PortableDataType.TextVariableLength:
					case PortableDataType.TextLargeLength:
						switch (sourceType)
						{
							case PortableDataType.TextFixedLength:
							case PortableDataType.TextVariableLength:
							case PortableDataType.TextLargeLength:
								return true;
						}
						break;
					case PortableDataType.NumericSignedInteger:
					case PortableDataType.NumericSignedSmallInteger:
					case PortableDataType.NumericSignedLargeInteger:
					case PortableDataType.NumericUnsignedInteger:
					case PortableDataType.NumericUnsignedTinyInteger:
					case PortableDataType.NumericUnsignedSmallInteger:
					case PortableDataType.NumericUnsignedLargeInteger:
					case PortableDataType.NumericFloatingPoint:
					case PortableDataType.NumericSinglePrecisionFloatingPoint:
					case PortableDataType.NumericDoublePrecisionFloatingPoint:
					case PortableDataType.NumericDecimal:
					case PortableDataType.NumericMoney:
						switch (sourceType)
						{
							case PortableDataType.NumericSignedInteger:
							case PortableDataType.NumericSignedSmallInteger:
							case PortableDataType.NumericSignedLargeInteger:
							case PortableDataType.NumericUnsignedInteger:
							case PortableDataType.NumericUnsignedTinyInteger:
							case PortableDataType.NumericUnsignedSmallInteger:
							case PortableDataType.NumericUnsignedLargeInteger:
							case PortableDataType.NumericFloatingPoint:
							case PortableDataType.NumericSinglePrecisionFloatingPoint:
							case PortableDataType.NumericDoublePrecisionFloatingPoint:
							case PortableDataType.NumericDecimal:
							case PortableDataType.NumericMoney:
								return true;
						}
						break;
					//case PortableDataType.RawDataFixedLength:
					//case PortableDataType.RawDataVariableLength:
					//case PortableDataType.RawDataLargeLength:
					//case PortableDataType.RawDataPicture:
					//case PortableDataType.RawDataOleObject:
					// Raw data types are not comparable, these cases would form a dead code path.

					//case PortableDataType.NumericAutoCounter:
					//case PortableDataType.TemporalTime:
					//case PortableDataType.TemporalDate:
					//case PortableDataType.OtherRowId:
					//case PortableDataType.OtherObjectId:
					// These compare only to themselves, will be caught in first if block

					case PortableDataType.TemporalAutoTimestamp:
					case PortableDataType.TemporalDateAndTime:
						switch (sourceType)
						{
							case PortableDataType.TemporalAutoTimestamp:
							case PortableDataType.TemporalDateAndTime:
								return true;
						}
						break;
					case PortableDataType.LogicalTrueOrFalse:
					case PortableDataType.LogicalYesOrNo:
						switch (sourceType)
						{
							case PortableDataType.LogicalTrueOrFalse:
							case PortableDataType.LogicalYesOrNo:
								return true;
						}
						break;
				}
			}
			return false;
		}
		#endregion // Helper Methods and Properties
		#region Abstract and Virtual Methods and Properties
		/// <summary>
		/// Gets the PortableDataType of this DataType.
		/// </summary>
		public abstract PortableDataType PortableDataType { get;}
		/// <summary>
		/// Override ToString() to localize the property descriptor.
		/// </summary>
		public abstract override string ToString();
		/// <summary>
		/// Virtual function to determine if string data can be interpreted
		/// as a value in this data type.
		/// </summary>
		/// <param name="value">Value to parse</param>
		/// <returns>default to true</returns>
		public virtual bool CanParse(string value)
		{
			return true;
		}
		/// <summary>
		/// Virtual function to determine if a string value in invariant form
		/// can be interpreted as a value in this data type
		/// </summary>
		/// <param name="invariantValue">Invariant value to parse.</param>
		/// <returns>default to true</returns>
		public virtual bool CanParseInvariant(string invariantValue)
		{
			return true;
		}
		/// <summary>
		/// Convert stringized data in the current culture to an invariant form.
		/// </summary>
		/// <param name="value">A string in the current culture.</param>
		/// <param name="invariantValue">Invariant form of the string.</param>
		/// <returns><see langword="true"/> if the value was recognized and converted.</returns>
		public virtual bool TryConvertToInvariant(string value, out string invariantValue)
		{
			invariantValue = value;
			return true;
		}
		/// <summary>
		/// Convert a culture-invariant form of stringized data to a culture-specific form.
		/// </summary>
		/// <param name="invariantValue">A string in the invariant form.</param>
		/// <param name="value">Culture-specific form of the string.</param>
		/// <returns><see langword="true"/> if the value was recognized and converted.</returns>
		public virtual bool TryConvertFromInvariant(string invariantValue, out string value)
		{
			value = invariantValue;
			return true;
		}
		/// <summary>
		/// Return true if CanParse always returns true
		/// </summary>
		public virtual bool CanParseAnyValue
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Return true if the string form of the data is
		/// sensitive to changes in the culture. If this is
		/// set, then data sent to the <see cref="Compare"/>
		/// function should be in an invariant form returned
		/// by the <see cref="TryConvertToInvariant"/> method.
		/// </summary>
		public virtual bool IsCultureSensitive
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Specify if a data type supports single values only, closed ranges, or open ranges
		/// </summary>
		public abstract DataTypeRangeSupport RangeSupport { get; }
		/// <summary>
		/// Adjust the lower bound of an interval with discontinuous range support.
		/// </summary>
		/// <param name="invariantBound">The original bound in invariant form. Value may be modified on return.</param>
		/// <param name="isOpen"><see langword="true"/> if the bound is for an open range. Value may be modified on return.</param>
		/// <returns><see langword="true"/> if the value could be adjusted.</returns>
		public virtual bool AdjustDiscontinuousLowerBound(ref string invariantBound, ref bool isOpen)
		{
			return true;
		}
		/// <summary>
		/// Adjust the upper bound of an interval with discontinuous range support.
		/// </summary>
		/// <param name="invariantBound">The original bound in invariant form. Value may be modified on return.</param>
		/// <param name="isOpen"><see langword="true"/> if the bound is for an open range. Value may be modified on return.</param>
		/// <returns><see langword="true"/> if the value could be adjusted.</returns>
		public virtual bool AdjustDiscontinuousUpperBound(ref string invariantBound, ref bool isOpen)
		{
			return true;
		}
		/// <summary>
		/// Return true if the compare function should be called. Should
		/// be true (the default) for all but the UnspecifiedDataType
		/// </summary>
		public virtual bool CanCompare
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Override to return a name to display for the Scale property.
		/// A <see langword="null"/> return does not show the property,
		/// and an empty string uses the default name.
		/// </summary>
		public virtual string ScaleName
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Override to return a customized description to display for the Scale property.
		/// </summary>
		public virtual string ScaleDescription
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Override to return a name to display for the Length property.
		/// A <see langword="null"/> return does not show the property,
		/// and an empty string uses the default name.
		/// </summary>
		public virtual string LengthName
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Override to return a customized description to display for the Length property.
		/// </summary>
		public virtual string LengthDescription
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Compare two values according to the semantics of this data type
		/// </summary>
		/// <param name="invariantValue1">Left string value</param>
		/// <param name="invariantValue2">Right string value</param>
		/// <returns>Standard compare functions values (-1, 0, 1)</returns>
		public abstract int Compare(string invariantValue1, string invariantValue2);
		#endregion // Abstract and Virtual Methods and Properties
	}
	#endregion // DataType class
	#region Custom CanParse implementations
	public partial class YesOrNoLogicalDataType
	{
		/// <summary>
		/// return true if the string represents yes or no
		/// </summary>
		public override bool CanParse(string value)
		{
			// UNDONE: How are we going to localize this properly? We may need to
			// get the y/n/yes/no values from the resource file.
			return (!string.IsNullOrEmpty(value) &&
				(string.Equals(value, "Y", StringComparison.OrdinalIgnoreCase) ||
				string.Equals(value, "N", StringComparison.OrdinalIgnoreCase) ||
				string.Equals(value, "YES", StringComparison.OrdinalIgnoreCase) ||
				string.Equals(value, "NO", StringComparison.OrdinalIgnoreCase)));
		}
		/// <summary>
		/// There are values we can't parse
		/// </summary>
		public override bool CanParseAnyValue
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Returns true if the values are equivalent
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			// UNDONE: How are we going to localize this properly? We may need to
			// get the y/n/yes/no values from the resource file.
			bool v1 = string.Equals(value1, "Y", StringComparison.OrdinalIgnoreCase) || string.Equals(value1, "YES", StringComparison.OrdinalIgnoreCase);
			bool v2 = string.Equals(value2, "Y", StringComparison.OrdinalIgnoreCase) || string.Equals(value2, "YES", StringComparison.OrdinalIgnoreCase);
			return v1.CompareTo(v2);
		}
	}
	#endregion // Custom CanParse implementations
	#region DataTypeNotSpecified Error
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	public partial class DataTypeNotSpecifiedError : IRepresentModelElements
	{
		#region Accessor Properties
		/// <summary>
		/// The value type associated with this error
		/// </summary>
		public ObjectType AssociatedValueType
		{
			get
			{
				ValueTypeHasDataType link = ValueTypeHasDataType;
				return (link != null) ? link.ValueType : null;
			}
		}
		#endregion // Accessor Properties
		#region Required overrides
		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectType valueType = AssociatedValueType;
			if (valueType != null)
			{
				string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorValueTypeDataTypeNotSpecifiedMessage, valueType.Name, Model.Name);
				if (ErrorText != newText)
				{
					ErrorText = newText;
				}
			}
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorValueTypeDataTypeNotSpecifiedMessageCompact;
			}
		}
		/// <summary>
		/// Sets regenerate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}

		#endregion
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			// Reimplement to go to the ValueType instead of the default link
			ObjectType valueType = AssociatedValueType;
			return (valueType != null) ? new ModelElement[] { valueType } : null;
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // DataTypeNotSpecified Error
}

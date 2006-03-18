#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
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
		[CLSCompliant(true)]
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
				Debug.Assert(false); // Shouldn't be called if HasElements returns false
			}
			void IDeserializationFixupListener.PhaseCompleted(int phase, Store store)
			{
				if (phase == myPhase)
				{
					foreach (ORMModel model in store.ElementDirectory.GetElements(ORMModel.MetaClassGuid))
					{
						int knownTypesCount = (int)PortableDataType.UserDefined;
						DataType[] knownTypes = new DataType[knownTypesCount];
						DataTypeMoveableCollection currentDataTypes = model.DataTypeCollection;
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
								DataType newDataType = (DataType)factory.CreateElement(newType);
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
				Debug.Assert(false); // Not used on the listeners
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
		/// The data type supports closed ranges only
		/// </summary>
		Closed,
		/// <summary>
		/// The data type supports open and closed ranges
		/// </summary>
		Open,
	}
	#endregion // DataTypeRangeSupport enum
	#region DataType class
	public abstract partial class DataType
	{
		/// <summary>
		/// Gets the PortableDataType of this DataType.
		/// </summary>
		public abstract PortableDataType PortableDataType { get;}
		/// <summary>
		/// Override ToString() to localize the property descriptor.
		/// </summary>
		public abstract override string ToString();
		/// <summary>
		/// Defines property descriptor read only status.
		/// </summary>
		/// <returns>True</returns>
		public override bool IsPropertyDescriptorReadOnly(System.ComponentModel.PropertyDescriptor propertyDescriptor)
		{
			return true;
		}
		/// <summary>
		/// Virtual function to determine if string data can be interpreted
		/// as a value in this data type.
		/// </summary>
		/// <param name="value">value to parse</param>
		/// <returns>default to true</returns>
		public virtual bool CanParse(string value)
		{
			return true;
		}
		/// <summary>
		/// Specify if a data type supports single values only, closed ranges, or open ranges
		/// </summary>
		public abstract DataTypeRangeSupport RangeSupport { get; }
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
		/// Compare two values according to the semantics of this data type
		/// </summary>
		/// <param name="value1">Left string value</param>
		/// <param name="value2">Right string value</param>
		/// <returns>Standard compare functions values (-1, 0, 1)</returns>
		public abstract int Compare(string value1, string value2);
	}
	#endregion // DataType class
	#region Custom CanParse implementations
	public partial class YesOrNoLogicalDataType
	{
		/// <summary>
		/// return true if the string represents yes or no
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool CanParse(string value)
		{
			// UNDONE: How are we going to localize this properly? We may need to
			// get the y/n/yes/no values from the resource file.
			return (value != null && value.Length != 0 &&
				(0 == string.Compare(value, "y", true, CultureInfo.InvariantCulture) ||
				0 == string.Compare(value, "n", true, CultureInfo.InvariantCulture) ||
				0 == string.Compare(value, "yes", true, CultureInfo.InvariantCulture) ||
				0 == string.Compare(value, "no", true, CultureInfo.InvariantCulture)));
		}
		/// <summary>
		/// Returns true if the values are equivalent
		/// </summary>
		public override int Compare(string value1, string value2)
		{
			// UNDONE: How are we going to localize this properly? We may need to
			// get the y/n/yes/no values from the resource file.
			bool v1 = 0 == string.Compare(value1, "y", true, CultureInfo.InvariantCulture) || 0 == string.Compare(value1, "yes", true, CultureInfo.InvariantCulture);
			bool v2 = 0 == string.Compare(value2, "y", true, CultureInfo.InvariantCulture) || 0 == string.Compare(value2, "yes", true, CultureInfo.InvariantCulture);
			if (v1 == v2)
			{
				return 0;
			}
			return 1;
		}
	}
	public partial class VariableLengthRawDataDataType
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool CanParse(string value)
		{
			return true;
		}
	}
	public partial class PictureRawDataDataType
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool CanParse(string value)
		{
			return true;
		}
	}
	public partial class OleObjectRawDataDataType
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool CanParse(string value)
		{
			return true;
		}
	}
	public partial class LargeLengthRawDataDataType
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool CanParse(string value)
		{
			return true;
		}
	}
	public partial class FixedLengthRawDataDataType
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool CanParse(string value)
		{
			return true;
		}
	}
	#endregion // Custom CanParse implementations
	#region DataTypeNotSpecified Error
	public partial class DataTypeNotSpecifiedError : IRepresentModelElements
	{
		/// <summary>
		/// A class to add unspecified data type errors
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class UnspecifiedTypeAddedRule : AddRule
		{
			/// <summary>
			/// Test if an added data type relationship points to
			/// an unspecified type
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				UnspecifiedDataType unspecifiedType = link.DataType as UnspecifiedDataType;
				if (unspecifiedType != null)
				{
					DataTypeNotSpecifiedError error = DataTypeNotSpecifiedError.CreateDataTypeNotSpecifiedError(link.Store);
					link.DataTypeNotSpecifiedError = error;
					error.Model = unspecifiedType.Model;
					error.GenerateErrorText();
				}
			}
		}
		
		#region Accessor Properties
		/// <summary>
		/// The value type associated with this error
		/// </summary>
		public ObjectType AssociatedValueType
		{
			get
			{
				ValueTypeHasDataType link = ValueTypeHasDataType;
				return (link != null) ? link.ValueTypeCollection : null;
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
				if (Name != newText)
				{
					Name = newText;
				}
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
		/// Implements IRepresentModelElements.GetRepresentedElements
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
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

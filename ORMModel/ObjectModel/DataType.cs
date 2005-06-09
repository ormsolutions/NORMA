using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;

namespace Northface.Tools.ORM.ObjectModel
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
		private static IDeserializationFixupListener DataTypesFixupListener
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
	}
	#endregion // DataType class
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
					error.Model = unspecifiedType.Model;
					link.DataTypeNotSpecifiedError = error;
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

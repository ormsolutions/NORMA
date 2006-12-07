using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
using AccessedThroughPropertyAttribute = System.Runtime.CompilerServices.AccessedThroughPropertyAttribute;
using GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute;
using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
using LayoutKind = System.Runtime.InteropServices.LayoutKind;
using CharSet = System.Runtime.InteropServices.CharSet;
namespace SampleModel
{
	#region Person
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Person : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Person()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[38]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
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
		protected bool OnFirstNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.FirstName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
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
		protected void OnFirstNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.FirstName));
				this.OnPropertyChanged("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
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
		protected bool OnDate_YMDChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, int>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, int>>(eventHandler, this, new PropertyChangingEventArgs<Person, int>(this, this.Date_YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
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
		protected void OnDate_YMDChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, int>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, int>>(eventHandler, this, new PropertyChangedEventArgs<Person, int>(this, oldValue, this.Date_YMD));
				this.OnPropertyChanged("Date_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
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
		protected bool OnLastNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.LastName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
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
		protected void OnLastNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.LastName));
				this.OnPropertyChanged("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
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
		protected bool OnOptionalUniqueStringChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.OptionalUniqueString, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
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
		protected void OnOptionalUniqueStringChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.OptionalUniqueString));
				this.OnPropertyChanged("OptionalUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
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
		protected bool OnHatType_ColorARGBChanging(Nullable<int> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<int>>(this, this.HatType_ColorARGB, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
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
		protected void OnHatType_ColorARGBChanged(Nullable<int> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, oldValue, this.HatType_ColorARGB));
				this.OnPropertyChanged("HatType_ColorARGB");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
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
		protected bool OnHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[11] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.HatType_HatTypeStyle_HatTypeStyle_Description, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
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
		protected void OnHatType_HatTypeStyle_HatTypeStyle_DescriptionChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[12] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.HatType_HatTypeStyle_HatTypeStyle_Description));
				this.OnPropertyChanged("HatType_HatTypeStyle_HatTypeStyle_Description");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
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
		protected bool OnOwnsCar_vinChanging(Nullable<int> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler = this.Events[13] as EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<int>>(this, this.OwnsCar_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
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
		protected void OnOwnsCar_vinChanged(Nullable<int> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler = this.Events[14] as EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, oldValue, this.OwnsCar_vin));
				this.OnPropertyChanged("OwnsCar_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
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
		protected bool OnGender_Gender_CodeChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[15] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.Gender_Gender_Code, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
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
		protected void OnGender_Gender_CodeChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[16] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.Gender_Gender_Code));
				this.OnPropertyChanged("Gender_Gender_Code");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
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
		protected bool OnhasParentsChanging(bool newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, bool>> eventHandler = this.Events[17] as EventHandler<PropertyChangingEventArgs<Person, bool>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, bool>>(eventHandler, this, new PropertyChangingEventArgs<Person, bool>(this, this.hasParents, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
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
		protected void OnhasParentsChanged(bool oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, bool>> eventHandler = this.Events[18] as EventHandler<PropertyChangedEventArgs<Person, bool>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, bool>>(eventHandler, this, new PropertyChangedEventArgs<Person, bool>(this, oldValue, this.hasParents));
				this.OnPropertyChanged("hasParents");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
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
		protected bool OnOptionalUniqueDecimalChanging(Nullable<decimal> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> eventHandler = this.Events[19] as EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<decimal>>(this, this.OptionalUniqueDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Events[20] = System.Delegate.Combine(this.Events[20], value);
			}
			remove
			{
				this.Events[20] = System.Delegate.Remove(this.Events[20], value);
			}
		}
		protected void OnOptionalUniqueDecimalChanged(Nullable<decimal> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> eventHandler = this.Events[20] as EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<decimal>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<decimal>>(this, oldValue, this.OptionalUniqueDecimal));
				this.OnPropertyChanged("OptionalUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Events[21] = System.Delegate.Combine(this.Events[21], value);
			}
			remove
			{
				this.Events[21] = System.Delegate.Remove(this.Events[21], value);
			}
		}
		protected bool OnMandatoryUniqueDecimalChanging(decimal newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, decimal>> eventHandler = this.Events[21] as EventHandler<PropertyChangingEventArgs<Person, decimal>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, decimal>>(eventHandler, this, new PropertyChangingEventArgs<Person, decimal>(this, this.MandatoryUniqueDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Events[22] = System.Delegate.Combine(this.Events[22], value);
			}
			remove
			{
				this.Events[22] = System.Delegate.Remove(this.Events[22], value);
			}
		}
		protected void OnMandatoryUniqueDecimalChanged(decimal oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, decimal>> eventHandler = this.Events[22] as EventHandler<PropertyChangedEventArgs<Person, decimal>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, decimal>>(eventHandler, this, new PropertyChangedEventArgs<Person, decimal>(this, oldValue, this.MandatoryUniqueDecimal));
				this.OnPropertyChanged("MandatoryUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Events[23] = System.Delegate.Combine(this.Events[23], value);
			}
			remove
			{
				this.Events[23] = System.Delegate.Remove(this.Events[23], value);
			}
		}
		protected bool OnMandatoryUniqueStringChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[23] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.MandatoryUniqueString, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Events[24] = System.Delegate.Combine(this.Events[24], value);
			}
			remove
			{
				this.Events[24] = System.Delegate.Remove(this.Events[24], value);
			}
		}
		protected void OnMandatoryUniqueStringChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[24] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.MandatoryUniqueString));
				this.OnPropertyChanged("MandatoryUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Events[25] = System.Delegate.Combine(this.Events[25], value);
			}
			remove
			{
				this.Events[25] = System.Delegate.Remove(this.Events[25], value);
			}
		}
		protected bool OnHusbandChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Person>> eventHandler = this.Events[25] as EventHandler<PropertyChangingEventArgs<Person, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Person>>(eventHandler, this, new PropertyChangingEventArgs<Person, Person>(this, this.Husband, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Events[26] = System.Delegate.Combine(this.Events[26], value);
			}
			remove
			{
				this.Events[26] = System.Delegate.Remove(this.Events[26], value);
			}
		}
		protected void OnHusbandChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Person>> eventHandler = this.Events[26] as EventHandler<PropertyChangedEventArgs<Person, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Person>>(eventHandler, this, new PropertyChangedEventArgs<Person, Person>(this, oldValue, this.Husband));
				this.OnPropertyChanged("Husband");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Events[27] = System.Delegate.Combine(this.Events[27], value);
			}
			remove
			{
				this.Events[27] = System.Delegate.Remove(this.Events[27], value);
			}
		}
		protected bool OnValueType1DoesSomethingElseWithChanging(ValueType1 newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, ValueType1>> eventHandler = this.Events[27] as EventHandler<PropertyChangingEventArgs<Person, ValueType1>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, ValueType1>>(eventHandler, this, new PropertyChangingEventArgs<Person, ValueType1>(this, this.ValueType1DoesSomethingElseWith, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Events[28] = System.Delegate.Combine(this.Events[28], value);
			}
			remove
			{
				this.Events[28] = System.Delegate.Remove(this.Events[28], value);
			}
		}
		protected void OnValueType1DoesSomethingElseWithChanged(ValueType1 oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, ValueType1>> eventHandler = this.Events[28] as EventHandler<PropertyChangedEventArgs<Person, ValueType1>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, ValueType1>>(eventHandler, this, new PropertyChangedEventArgs<Person, ValueType1>(this, oldValue, this.ValueType1DoesSomethingElseWith));
				this.OnPropertyChanged("ValueType1DoesSomethingElseWith");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Events[29] = System.Delegate.Combine(this.Events[29], value);
			}
			remove
			{
				this.Events[29] = System.Delegate.Remove(this.Events[29], value);
			}
		}
		protected bool OnMalePersonChanging(MalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, MalePerson>> eventHandler = this.Events[29] as EventHandler<PropertyChangingEventArgs<Person, MalePerson>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, MalePerson>>(eventHandler, this, new PropertyChangingEventArgs<Person, MalePerson>(this, this.MalePerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Events[30] = System.Delegate.Combine(this.Events[30], value);
			}
			remove
			{
				this.Events[30] = System.Delegate.Remove(this.Events[30], value);
			}
		}
		protected void OnMalePersonChanged(MalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, MalePerson>> eventHandler = this.Events[30] as EventHandler<PropertyChangedEventArgs<Person, MalePerson>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, MalePerson>>(eventHandler, this, new PropertyChangedEventArgs<Person, MalePerson>(this, oldValue, this.MalePerson));
				this.OnPropertyChanged("MalePerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Events[31] = System.Delegate.Combine(this.Events[31], value);
			}
			remove
			{
				this.Events[31] = System.Delegate.Remove(this.Events[31], value);
			}
		}
		protected bool OnFemalePersonChanging(FemalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> eventHandler = this.Events[31] as EventHandler<PropertyChangingEventArgs<Person, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, FemalePerson>>(eventHandler, this, new PropertyChangingEventArgs<Person, FemalePerson>(this, this.FemalePerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Events[32] = System.Delegate.Combine(this.Events[32], value);
			}
			remove
			{
				this.Events[32] = System.Delegate.Remove(this.Events[32], value);
			}
		}
		protected void OnFemalePersonChanged(FemalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> eventHandler = this.Events[32] as EventHandler<PropertyChangedEventArgs<Person, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, FemalePerson>>(eventHandler, this, new PropertyChangedEventArgs<Person, FemalePerson>(this, oldValue, this.FemalePerson));
				this.OnPropertyChanged("FemalePerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Events[33] = System.Delegate.Combine(this.Events[33], value);
			}
			remove
			{
				this.Events[33] = System.Delegate.Remove(this.Events[33], value);
			}
		}
		protected bool OnChildPersonChanging(ChildPerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> eventHandler = this.Events[33] as EventHandler<PropertyChangingEventArgs<Person, ChildPerson>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, ChildPerson>>(eventHandler, this, new PropertyChangingEventArgs<Person, ChildPerson>(this, this.ChildPerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Events[34] = System.Delegate.Combine(this.Events[34], value);
			}
			remove
			{
				this.Events[34] = System.Delegate.Remove(this.Events[34], value);
			}
		}
		protected void OnChildPersonChanged(ChildPerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> eventHandler = this.Events[34] as EventHandler<PropertyChangedEventArgs<Person, ChildPerson>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, ChildPerson>>(eventHandler, this, new PropertyChangedEventArgs<Person, ChildPerson>(this, oldValue, this.ChildPerson));
				this.OnPropertyChanged("ChildPerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Events[35] = System.Delegate.Combine(this.Events[35], value);
			}
			remove
			{
				this.Events[35] = System.Delegate.Remove(this.Events[35], value);
			}
		}
		protected bool OnDeathChanging(Death newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Death>> eventHandler = this.Events[35] as EventHandler<PropertyChangingEventArgs<Person, Death>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Death>>(eventHandler, this, new PropertyChangingEventArgs<Person, Death>(this, this.Death, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Events[36] = System.Delegate.Combine(this.Events[36], value);
			}
			remove
			{
				this.Events[36] = System.Delegate.Remove(this.Events[36], value);
			}
		}
		protected void OnDeathChanged(Death oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Death>> eventHandler = this.Events[36] as EventHandler<PropertyChangedEventArgs<Person, Death>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Death>>(eventHandler, this, new PropertyChangedEventArgs<Person, Death>(this, oldValue, this.Death));
				this.OnPropertyChanged("Death");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Events[37] = System.Delegate.Combine(this.Events[37], value);
			}
			remove
			{
				this.Events[37] = System.Delegate.Remove(this.Events[37], value);
			}
		}
		protected bool OnWifeChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Person>> eventHandler = this.Events[37] as EventHandler<PropertyChangingEventArgs<Person, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Person>>(eventHandler, this, new PropertyChangingEventArgs<Person, Person>(this, this.Wife, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Events[38] = System.Delegate.Combine(this.Events[38], value);
			}
			remove
			{
				this.Events[38] = System.Delegate.Remove(this.Events[38], value);
			}
		}
		protected void OnWifeChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Person>> eventHandler = this.Events[38] as EventHandler<PropertyChangedEventArgs<Person, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Person>>(eventHandler, this, new PropertyChangedEventArgs<Person, Person>(this, oldValue, this.Wife));
				this.OnPropertyChanged("Wife");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract string FirstName
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract int Date_YMD
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string LastName
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract string OptionalUniqueString
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<int> HatType_ColorARGB
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<int> OwnsCar_vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string Gender_Gender_Code
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract bool hasParents
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<decimal> OptionalUniqueDecimal
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract decimal MandatoryUniqueDecimal
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string MandatoryUniqueString
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Person Husband
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract ValueType1 ValueType1DoesSomethingElseWith
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract MalePerson MalePerson
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract FemalePerson FemalePerson
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract ChildPerson ChildPerson
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Death Death
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Person Wife
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<Task> TaskViaPersonCollection
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}OptionalUniqueString = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}hasParents = ""{10}"",{0}{1}OptionalUniqueDecimal = ""{11}"",{0}{1}MandatoryUniqueDecimal = ""{12}"",{0}{1}MandatoryUniqueString = ""{13}"",{0}{1}Husband = {14},{0}{1}ValueType1DoesSomethingElseWith = {15},{0}{1}MalePerson = {16},{0}{1}FemalePerson = {17},{0}{1}ChildPerson = {18},{0}{1}Death = {19},{0}{1}Wife = {20}{0}}}", Environment.NewLine, @"	", this.FirstName, this.Date_YMD, this.LastName, this.OptionalUniqueString, this.HatType_ColorARGB, this.HatType_HatTypeStyle_HatTypeStyle_Description, this.OwnsCar_vin, this.Gender_Gender_Code, this.hasParents, this.OptionalUniqueDecimal, this.MandatoryUniqueDecimal, this.MandatoryUniqueString, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
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
		public static explicit operator NaturalDeath(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else
			{
				return (NaturalDeath)(Death)Person;
			}
		}
		public static explicit operator UnnaturalDeath(Person Person)
		{
			if (Person == null)
			{
				return null;
			}
			else
			{
				return (UnnaturalDeath)(Death)Person;
			}
		}
	}
	#endregion // Person
	#region MalePerson
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class MalePerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected MalePerson()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[2]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<MalePerson, Person>> PersonChanging
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
		protected bool OnPersonChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<MalePerson, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<MalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<MalePerson, Person>>(eventHandler, this, new PropertyChangingEventArgs<MalePerson, Person>(this, this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<MalePerson, Person>> PersonChanged
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
		protected void OnPersonChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<MalePerson, Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<MalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<MalePerson, Person>>(eventHandler, this, new PropertyChangedEventArgs<MalePerson, Person>(this, oldValue, this.Person));
				this.OnPropertyChanged("Person");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<ChildPerson> ChildPersonViaFatherCollection
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "MalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
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
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual int Date_YMD
		{
			get
			{
				return this.Person.Date_YMD;
			}
			set
			{
				this.Person.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
		{
			add
			{
				this.Person.Date_YMDChanging += value;
			}
			remove
			{
				this.Person.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
		{
			add
			{
				this.Person.Date_YMDChanged += value;
			}
			remove
			{
				this.Person.Date_YMDChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual bool hasParents
		{
			get
			{
				return this.Person.hasParents;
			}
			set
			{
				this.Person.hasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
		{
			add
			{
				this.Person.hasParentsChanging += value;
			}
			remove
			{
				this.Person.hasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
		{
			add
			{
				this.Person.hasParentsChanged += value;
			}
			remove
			{
				this.Person.hasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual Person Husband
		{
			get
			{
				return this.Person.Husband;
			}
			set
			{
				this.Person.Husband = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Person.HusbandChanging += value;
			}
			remove
			{
				this.Person.HusbandChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Person.HusbandChanged += value;
			}
			remove
			{
				this.Person.HusbandChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Person.FemalePerson;
			}
			set
			{
				this.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Person.ChildPerson;
			}
			set
			{
				this.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Person.ChildPersonChanged -= value;
			}
		}
		public virtual Death Death
		{
			get
			{
				return this.Person.Death;
			}
			set
			{
				this.Person.Death = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Person.DeathChanging += value;
			}
			remove
			{
				this.Person.DeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Person.DeathChanged += value;
			}
			remove
			{
				this.Person.DeathChanged -= value;
			}
		}
		public virtual Person Wife
		{
			get
			{
				return this.Person.Wife;
			}
			set
			{
				this.Person.Wife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Person.WifeChanging += value;
			}
			remove
			{
				this.Person.WifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Person.WifeChanged += value;
			}
			remove
			{
				this.Person.WifeChanged -= value;
			}
		}
		public virtual IEnumerable<Task> TaskViaPersonCollection
		{
			get
			{
				return this.Person.TaskViaPersonCollection;
			}
		}
		public virtual IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaSellerCollection;
			}
		}
	}
	#endregion // MalePerson
	#region FemalePerson
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class FemalePerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected FemalePerson()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[2]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<FemalePerson, Person>> PersonChanging
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
		protected bool OnPersonChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<FemalePerson, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<FemalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<FemalePerson, Person>>(eventHandler, this, new PropertyChangingEventArgs<FemalePerson, Person>(this, this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<FemalePerson, Person>> PersonChanged
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
		protected void OnPersonChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<FemalePerson, Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<FemalePerson, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<FemalePerson, Person>>(eventHandler, this, new PropertyChangedEventArgs<FemalePerson, Person>(this, oldValue, this.Person));
				this.OnPropertyChanged("Person");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<ChildPerson> ChildPersonViaMotherCollection
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "FemalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
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
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual int Date_YMD
		{
			get
			{
				return this.Person.Date_YMD;
			}
			set
			{
				this.Person.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
		{
			add
			{
				this.Person.Date_YMDChanging += value;
			}
			remove
			{
				this.Person.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
		{
			add
			{
				this.Person.Date_YMDChanged += value;
			}
			remove
			{
				this.Person.Date_YMDChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual bool hasParents
		{
			get
			{
				return this.Person.hasParents;
			}
			set
			{
				this.Person.hasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
		{
			add
			{
				this.Person.hasParentsChanging += value;
			}
			remove
			{
				this.Person.hasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
		{
			add
			{
				this.Person.hasParentsChanged += value;
			}
			remove
			{
				this.Person.hasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual Person Husband
		{
			get
			{
				return this.Person.Husband;
			}
			set
			{
				this.Person.Husband = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Person.HusbandChanging += value;
			}
			remove
			{
				this.Person.HusbandChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Person.HusbandChanged += value;
			}
			remove
			{
				this.Person.HusbandChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Person.MalePerson;
			}
			set
			{
				this.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Person.MalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Person.ChildPerson;
			}
			set
			{
				this.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Person.ChildPersonChanged -= value;
			}
		}
		public virtual Death Death
		{
			get
			{
				return this.Person.Death;
			}
			set
			{
				this.Person.Death = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Person.DeathChanging += value;
			}
			remove
			{
				this.Person.DeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Person.DeathChanged += value;
			}
			remove
			{
				this.Person.DeathChanged -= value;
			}
		}
		public virtual Person Wife
		{
			get
			{
				return this.Person.Wife;
			}
			set
			{
				this.Person.Wife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Person.WifeChanging += value;
			}
			remove
			{
				this.Person.WifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Person.WifeChanged += value;
			}
			remove
			{
				this.Person.WifeChanged -= value;
			}
		}
		public virtual IEnumerable<Task> TaskViaPersonCollection
		{
			get
			{
				return this.Person.TaskViaPersonCollection;
			}
		}
		public virtual IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaSellerCollection;
			}
		}
	}
	#endregion // FemalePerson
	#region ChildPerson
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class ChildPerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected ChildPerson()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[8]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, int>> BirthOrder_BirthOrder_NrChanging
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
		protected bool OnBirthOrder_BirthOrder_NrChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<ChildPerson, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, int>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, int>(this, this.BirthOrder_BirthOrder_Nr, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, int>> BirthOrder_BirthOrder_NrChanged
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
		protected void OnBirthOrder_BirthOrder_NrChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<ChildPerson, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, int>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, int>(this, oldValue, this.BirthOrder_BirthOrder_Nr));
				this.OnPropertyChanged("BirthOrder_BirthOrder_Nr");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>> FatherChanging
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
		protected bool OnFatherChanging(MalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, MalePerson>(this, this.Father, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>> FatherChanged
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
		protected void OnFatherChanged(MalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, MalePerson>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, MalePerson>(this, oldValue, this.Father));
				this.OnPropertyChanged("Father");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>> MotherChanging
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
		protected bool OnMotherChanging(FemalePerson newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, FemalePerson>(this, this.Mother, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>> MotherChanged
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
		protected void OnMotherChanged(FemalePerson oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, FemalePerson>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, FemalePerson>(this, oldValue, this.Mother));
				this.OnPropertyChanged("Mother");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, Person>> PersonChanging
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
		protected bool OnPersonChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<ChildPerson, Person>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<ChildPerson, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, Person>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, Person>(this, this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, Person>> PersonChanged
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
		protected void OnPersonChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ChildPerson, Person>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<ChildPerson, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, Person>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, Person>(this, oldValue, this.Person));
				this.OnPropertyChanged("Person");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract int BirthOrder_BirthOrder_Nr
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract MalePerson Father
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract FemalePerson Mother
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"ChildPerson{0}{{{0}{1}BirthOrder_BirthOrder_Nr = ""{2}"",{0}{1}Father = {3},{0}{1}Mother = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, @"	", this.BirthOrder_BirthOrder_Nr, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
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
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual int Date_YMD
		{
			get
			{
				return this.Person.Date_YMD;
			}
			set
			{
				this.Person.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
		{
			add
			{
				this.Person.Date_YMDChanging += value;
			}
			remove
			{
				this.Person.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
		{
			add
			{
				this.Person.Date_YMDChanged += value;
			}
			remove
			{
				this.Person.Date_YMDChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual bool hasParents
		{
			get
			{
				return this.Person.hasParents;
			}
			set
			{
				this.Person.hasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
		{
			add
			{
				this.Person.hasParentsChanging += value;
			}
			remove
			{
				this.Person.hasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
		{
			add
			{
				this.Person.hasParentsChanged += value;
			}
			remove
			{
				this.Person.hasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual Person Husband
		{
			get
			{
				return this.Person.Husband;
			}
			set
			{
				this.Person.Husband = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Person.HusbandChanging += value;
			}
			remove
			{
				this.Person.HusbandChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Person.HusbandChanged += value;
			}
			remove
			{
				this.Person.HusbandChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Person.MalePerson;
			}
			set
			{
				this.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Person.FemalePerson;
			}
			set
			{
				this.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Person.FemalePersonChanged -= value;
			}
		}
		public virtual Death Death
		{
			get
			{
				return this.Person.Death;
			}
			set
			{
				this.Person.Death = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				this.Person.DeathChanging += value;
			}
			remove
			{
				this.Person.DeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				this.Person.DeathChanged += value;
			}
			remove
			{
				this.Person.DeathChanged -= value;
			}
		}
		public virtual Person Wife
		{
			get
			{
				return this.Person.Wife;
			}
			set
			{
				this.Person.Wife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Person.WifeChanging += value;
			}
			remove
			{
				this.Person.WifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Person.WifeChanged += value;
			}
			remove
			{
				this.Person.WifeChanged -= value;
			}
		}
		public virtual IEnumerable<Task> TaskViaPersonCollection
		{
			get
			{
				return this.Person.TaskViaPersonCollection;
			}
		}
		public virtual IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaSellerCollection;
			}
		}
	}
	#endregion // ChildPerson
	#region Death
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Death : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Death()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[10]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
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
		protected bool OnDate_YMDChanging(Nullable<int> newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Death, Nullable<int>>(this, this.Date_YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
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
		protected void OnDate_YMDChanged(Nullable<int> oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Death, Nullable<int>>(this, oldValue, this.Date_YMD));
				this.OnPropertyChanged("Date_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
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
		protected bool OnDeathCause_DeathCause_TypeChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Death, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, string>>(eventHandler, this, new PropertyChangingEventArgs<Death, string>(this, this.DeathCause_DeathCause_Type, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
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
		protected void OnDeathCause_DeathCause_TypeChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Death, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, string>>(eventHandler, this, new PropertyChangedEventArgs<Death, string>(this, oldValue, this.DeathCause_DeathCause_Type));
				this.OnPropertyChanged("DeathCause_DeathCause_Type");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> NaturalDeathChanging
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
		protected bool OnNaturalDeathChanging(NaturalDeath newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, NaturalDeath>>(eventHandler, this, new PropertyChangingEventArgs<Death, NaturalDeath>(this, this.NaturalDeath, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> NaturalDeathChanged
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
		protected void OnNaturalDeathChanged(NaturalDeath oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, NaturalDeath>>(eventHandler, this, new PropertyChangedEventArgs<Death, NaturalDeath>(this, oldValue, this.NaturalDeath));
				this.OnPropertyChanged("NaturalDeath");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanging
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
		protected bool OnUnnaturalDeathChanging(UnnaturalDeath newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>>(eventHandler, this, new PropertyChangingEventArgs<Death, UnnaturalDeath>(this, this.UnnaturalDeath, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanged
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
		protected void OnUnnaturalDeathChanged(UnnaturalDeath oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, UnnaturalDeath>>(eventHandler, this, new PropertyChangedEventArgs<Death, UnnaturalDeath>(this, oldValue, this.UnnaturalDeath));
				this.OnPropertyChanged("UnnaturalDeath");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
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
		protected bool OnPersonChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<Death, Person>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<Death, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, Person>>(eventHandler, this, new PropertyChangingEventArgs<Death, Person>(this, this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
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
		protected void OnPersonChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Death, Person>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<Death, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, Person>>(eventHandler, this, new PropertyChangedEventArgs<Death, Person>(this, oldValue, this.Person));
				this.OnPropertyChanged("Person");
			}
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<int> Date_YMD
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string DeathCause_DeathCause_Type
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract NaturalDeath NaturalDeath
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract UnnaturalDeath UnnaturalDeath
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Death{0}{{{0}{1}Date_YMD = ""{2}"",{0}{1}DeathCause_DeathCause_Type = ""{3}"",{0}{1}NaturalDeath = {4},{0}{1}UnnaturalDeath = {5},{0}{1}Person = {6}{0}}}", Environment.NewLine, @"	", this.Date_YMD, this.DeathCause_DeathCause_Type, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
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
		public virtual string FirstName
		{
			get
			{
				return this.Person.FirstName;
			}
			set
			{
				this.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Person.FirstNameChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Person.LastName;
			}
			set
			{
				this.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Person.LastNameChanging += value;
			}
			remove
			{
				this.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Person.LastNameChanged += value;
			}
			remove
			{
				this.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Person.OptionalUniqueString;
			}
			set
			{
				this.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Person.HatType_ColorARGB;
			}
			set
			{
				this.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Person.OwnsCar_vin;
			}
			set
			{
				this.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Person.Gender_Gender_Code;
			}
			set
			{
				this.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual bool hasParents
		{
			get
			{
				return this.Person.hasParents;
			}
			set
			{
				this.Person.hasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
		{
			add
			{
				this.Person.hasParentsChanging += value;
			}
			remove
			{
				this.Person.hasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
		{
			add
			{
				this.Person.hasParentsChanged += value;
			}
			remove
			{
				this.Person.hasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Person.MandatoryUniqueString;
			}
			set
			{
				this.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual Person Husband
		{
			get
			{
				return this.Person.Husband;
			}
			set
			{
				this.Person.Husband = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Person.HusbandChanging += value;
			}
			remove
			{
				this.Person.HusbandChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Person.HusbandChanged += value;
			}
			remove
			{
				this.Person.HusbandChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Person.MalePerson;
			}
			set
			{
				this.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Person.FemalePerson;
			}
			set
			{
				this.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Person.ChildPerson;
			}
			set
			{
				this.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Person.ChildPersonChanged -= value;
			}
		}
		public virtual Person Wife
		{
			get
			{
				return this.Person.Wife;
			}
			set
			{
				this.Person.Wife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Person.WifeChanging += value;
			}
			remove
			{
				this.Person.WifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Person.WifeChanged += value;
			}
			remove
			{
				this.Person.WifeChanged -= value;
			}
		}
		public virtual IEnumerable<Task> TaskViaPersonCollection
		{
			get
			{
				return this.Person.TaskViaPersonCollection;
			}
		}
		public virtual IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get
			{
				return this.Person.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get
			{
				return this.Person.PersonBoughtCarFromPersonOnDateViaSellerCollection;
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
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class NaturalDeath : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected NaturalDeath()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[4]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, bool>> isFromProstateCancerChanging
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
		protected bool OnisFromProstateCancerChanging(bool newValue)
		{
			EventHandler<PropertyChangingEventArgs<NaturalDeath, bool>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<NaturalDeath, bool>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NaturalDeath, bool>>(eventHandler, this, new PropertyChangingEventArgs<NaturalDeath, bool>(this, this.isFromProstateCancer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, bool>> isFromProstateCancerChanged
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
		protected void OnisFromProstateCancerChanged(bool oldValue)
		{
			EventHandler<PropertyChangedEventArgs<NaturalDeath, bool>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<NaturalDeath, bool>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NaturalDeath, bool>>(eventHandler, this, new PropertyChangedEventArgs<NaturalDeath, bool>(this, oldValue, this.isFromProstateCancer));
				this.OnPropertyChanged("isFromProstateCancer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>> DeathChanging
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
		protected bool OnDeathChanging(Death newValue)
		{
			EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NaturalDeath, Death>>(eventHandler, this, new PropertyChangingEventArgs<NaturalDeath, Death>(this, this.Death, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>> DeathChanged
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
		protected void OnDeathChanged(Death oldValue)
		{
			EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NaturalDeath, Death>>(eventHandler, this, new PropertyChangedEventArgs<NaturalDeath, Death>(this, oldValue, this.Death));
				this.OnPropertyChanged("Death");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract bool isFromProstateCancer
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Death Death
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"NaturalDeath{0}{{{0}{1}isFromProstateCancer = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, @"	", this.isFromProstateCancer, "TODO: Recursively call ToString for customTypes...");
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
		public static implicit operator Person(NaturalDeath NaturalDeath)
		{
			if (NaturalDeath == null)
			{
				return null;
			}
			else
			{
				return NaturalDeath.Death.Person;
			}
		}
		public virtual Nullable<int> Date_YMD
		{
			get
			{
				return this.Death.Date_YMD;
			}
			set
			{
				this.Death.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
		{
			add
			{
				this.Death.Date_YMDChanging += value;
			}
			remove
			{
				this.Death.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
		{
			add
			{
				this.Death.Date_YMDChanged += value;
			}
			remove
			{
				this.Death.Date_YMDChanged -= value;
			}
		}
		public virtual string DeathCause_DeathCause_Type
		{
			get
			{
				return this.Death.DeathCause_DeathCause_Type;
			}
			set
			{
				this.Death.DeathCause_DeathCause_Type = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanging += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanged += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanged -= value;
			}
		}
		public virtual UnnaturalDeath UnnaturalDeath
		{
			get
			{
				return this.Death.UnnaturalDeath;
			}
			set
			{
				this.Death.UnnaturalDeath = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanging
		{
			add
			{
				this.Death.UnnaturalDeathChanging += value;
			}
			remove
			{
				this.Death.UnnaturalDeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanged
		{
			add
			{
				this.Death.UnnaturalDeathChanged += value;
			}
			remove
			{
				this.Death.UnnaturalDeathChanged -= value;
			}
		}
		public virtual Person Person
		{
			get
			{
				return this.Death.Person;
			}
			set
			{
				this.Death.Person = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
		{
			add
			{
				this.Death.PersonChanging += value;
			}
			remove
			{
				this.Death.PersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
		{
			add
			{
				this.Death.PersonChanged += value;
			}
			remove
			{
				this.Death.PersonChanged -= value;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Death.Person.FirstName;
			}
			set
			{
				this.Death.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Death.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Death.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Death.Person.LastName;
			}
			set
			{
				this.Death.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Death.Person.LastNameChanging += value;
			}
			remove
			{
				this.Death.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Death.Person.LastNameChanged += value;
			}
			remove
			{
				this.Death.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Death.Person.OptionalUniqueString;
			}
			set
			{
				this.Death.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Death.Person.HatType_ColorARGB;
			}
			set
			{
				this.Death.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Death.Person.OwnsCar_vin;
			}
			set
			{
				this.Death.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Death.Person.Gender_Gender_Code;
			}
			set
			{
				this.Death.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual bool hasParents
		{
			get
			{
				return this.Death.Person.hasParents;
			}
			set
			{
				this.Death.Person.hasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
		{
			add
			{
				this.Death.Person.hasParentsChanging += value;
			}
			remove
			{
				this.Death.Person.hasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
		{
			add
			{
				this.Death.Person.hasParentsChanged += value;
			}
			remove
			{
				this.Death.Person.hasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Death.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Death.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Death.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Death.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Death.Person.MandatoryUniqueString;
			}
			set
			{
				this.Death.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual Person Husband
		{
			get
			{
				return this.Death.Person.Husband;
			}
			set
			{
				this.Death.Person.Husband = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Death.Person.HusbandChanging += value;
			}
			remove
			{
				this.Death.Person.HusbandChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Death.Person.HusbandChanged += value;
			}
			remove
			{
				this.Death.Person.HusbandChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Death.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Death.Person.MalePerson;
			}
			set
			{
				this.Death.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Death.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Death.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Death.Person.FemalePerson;
			}
			set
			{
				this.Death.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Death.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Death.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Death.Person.ChildPerson;
			}
			set
			{
				this.Death.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Death.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Death.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanged -= value;
			}
		}
		public virtual Person Wife
		{
			get
			{
				return this.Death.Person.Wife;
			}
			set
			{
				this.Death.Person.Wife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Death.Person.WifeChanging += value;
			}
			remove
			{
				this.Death.Person.WifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Death.Person.WifeChanged += value;
			}
			remove
			{
				this.Death.Person.WifeChanged -= value;
			}
		}
		public virtual IEnumerable<Task> TaskViaPersonCollection
		{
			get
			{
				return this.Death.Person.TaskViaPersonCollection;
			}
		}
		public virtual IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateViaSellerCollection;
			}
		}
	}
	#endregion // NaturalDeath
	#region UnnaturalDeath
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class UnnaturalDeath : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected UnnaturalDeath()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[6]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>> isViolentChanging
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
		protected bool OnisViolentChanging(bool newValue)
		{
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, bool>(this, this.isViolent, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, bool>> isViolentChanged
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
		protected void OnisViolentChanged(bool oldValue)
		{
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, bool>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath, bool>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, bool>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, bool>(this, oldValue, this.isViolent));
				this.OnPropertyChanged("isViolent");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>> isBloodyChanging
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
		protected bool OnisBloodyChanging(bool newValue)
		{
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, bool>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, bool>(this, this.isBloody, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, bool>> isBloodyChanged
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
		protected void OnisBloodyChanged(bool oldValue)
		{
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, bool>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath, bool>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, bool>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, bool>(this, oldValue, this.isBloody));
				this.OnPropertyChanged("isBloody");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>> DeathChanging
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
		protected bool OnDeathChanging(Death newValue)
		{
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, Death>(this, this.Death, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>> DeathChanged
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
		protected void OnDeathChanged(Death oldValue)
		{
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, Death>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, Death>(this, oldValue, this.Death));
				this.OnPropertyChanged("Death");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract bool isViolent
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract bool isBloody
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Death Death
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"UnnaturalDeath{0}{{{0}{1}isViolent = ""{2}"",{0}{1}isBloody = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, @"	", this.isViolent, this.isBloody, "TODO: Recursively call ToString for customTypes...");
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
		public static implicit operator Person(UnnaturalDeath UnnaturalDeath)
		{
			if (UnnaturalDeath == null)
			{
				return null;
			}
			else
			{
				return UnnaturalDeath.Death.Person;
			}
		}
		public virtual Nullable<int> Date_YMD
		{
			get
			{
				return this.Death.Date_YMD;
			}
			set
			{
				this.Death.Date_YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
		{
			add
			{
				this.Death.Date_YMDChanging += value;
			}
			remove
			{
				this.Death.Date_YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
		{
			add
			{
				this.Death.Date_YMDChanged += value;
			}
			remove
			{
				this.Death.Date_YMDChanged -= value;
			}
		}
		public virtual string DeathCause_DeathCause_Type
		{
			get
			{
				return this.Death.DeathCause_DeathCause_Type;
			}
			set
			{
				this.Death.DeathCause_DeathCause_Type = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanging += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
		{
			add
			{
				this.Death.DeathCause_DeathCause_TypeChanged += value;
			}
			remove
			{
				this.Death.DeathCause_DeathCause_TypeChanged -= value;
			}
		}
		public virtual NaturalDeath NaturalDeath
		{
			get
			{
				return this.Death.NaturalDeath;
			}
			set
			{
				this.Death.NaturalDeath = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> NaturalDeathChanging
		{
			add
			{
				this.Death.NaturalDeathChanging += value;
			}
			remove
			{
				this.Death.NaturalDeathChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> NaturalDeathChanged
		{
			add
			{
				this.Death.NaturalDeathChanged += value;
			}
			remove
			{
				this.Death.NaturalDeathChanged -= value;
			}
		}
		public virtual Person Person
		{
			get
			{
				return this.Death.Person;
			}
			set
			{
				this.Death.Person = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
		{
			add
			{
				this.Death.PersonChanging += value;
			}
			remove
			{
				this.Death.PersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
		{
			add
			{
				this.Death.PersonChanged += value;
			}
			remove
			{
				this.Death.PersonChanged -= value;
			}
		}
		public virtual string FirstName
		{
			get
			{
				return this.Death.Person.FirstName;
			}
			set
			{
				this.Death.Person.FirstName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				this.Death.Person.FirstNameChanging += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				this.Death.Person.FirstNameChanged += value;
			}
			remove
			{
				this.Death.Person.FirstNameChanged -= value;
			}
		}
		public virtual string LastName
		{
			get
			{
				return this.Death.Person.LastName;
			}
			set
			{
				this.Death.Person.LastName = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				this.Death.Person.LastNameChanging += value;
			}
			remove
			{
				this.Death.Person.LastNameChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Death.Person.LastNameChanged += value;
			}
			remove
			{
				this.Death.Person.LastNameChanged -= value;
			}
		}
		public virtual string OptionalUniqueString
		{
			get
			{
				return this.Death.Person.OptionalUniqueString;
			}
			set
			{
				this.Death.Person.OptionalUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueStringChanged -= value;
			}
		}
		public virtual Nullable<int> HatType_ColorARGB
		{
			get
			{
				return this.Death.Person.HatType_ColorARGB;
			}
			set
			{
				this.Death.Person.HatType_ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
		{
			add
			{
				this.Death.Person.HatType_ColorARGBChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_ColorARGBChanged -= value;
			}
		}
		public virtual string HatType_HatTypeStyle_HatTypeStyle_Description
		{
			get
			{
				return this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description;
			}
			set
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_Description = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
		{
			add
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged += value;
			}
			remove
			{
				this.Death.Person.HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged -= value;
			}
		}
		public virtual Nullable<int> OwnsCar_vin
		{
			get
			{
				return this.Death.Person.OwnsCar_vin;
			}
			set
			{
				this.Death.Person.OwnsCar_vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanging += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
		{
			add
			{
				this.Death.Person.OwnsCar_vinChanged += value;
			}
			remove
			{
				this.Death.Person.OwnsCar_vinChanged -= value;
			}
		}
		public virtual string Gender_Gender_Code
		{
			get
			{
				return this.Death.Person.Gender_Gender_Code;
			}
			set
			{
				this.Death.Person.Gender_Gender_Code = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanging += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
		{
			add
			{
				this.Death.Person.Gender_Gender_CodeChanged += value;
			}
			remove
			{
				this.Death.Person.Gender_Gender_CodeChanged -= value;
			}
		}
		public virtual bool hasParents
		{
			get
			{
				return this.Death.Person.hasParents;
			}
			set
			{
				this.Death.Person.hasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, bool>> hasParentsChanging
		{
			add
			{
				this.Death.Person.hasParentsChanging += value;
			}
			remove
			{
				this.Death.Person.hasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, bool>> hasParentsChanged
		{
			add
			{
				this.Death.Person.hasParentsChanged += value;
			}
			remove
			{
				this.Death.Person.hasParentsChanged -= value;
			}
		}
		public virtual Nullable<decimal> OptionalUniqueDecimal
		{
			get
			{
				return this.Death.Person.OptionalUniqueDecimal;
			}
			set
			{
				this.Death.Person.OptionalUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueDecimalChanged -= value;
			}
		}
		public virtual decimal MandatoryUniqueDecimal
		{
			get
			{
				return this.Death.Person.MandatoryUniqueDecimal;
			}
			set
			{
				this.Death.Person.MandatoryUniqueDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueDecimalChanged -= value;
			}
		}
		public virtual string MandatoryUniqueString
		{
			get
			{
				return this.Death.Person.MandatoryUniqueString;
			}
			set
			{
				this.Death.Person.MandatoryUniqueString = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueStringChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueStringChanged -= value;
			}
		}
		public virtual Person Husband
		{
			get
			{
				return this.Death.Person.Husband;
			}
			set
			{
				this.Death.Person.Husband = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
		{
			add
			{
				this.Death.Person.HusbandChanging += value;
			}
			remove
			{
				this.Death.Person.HusbandChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
		{
			add
			{
				this.Death.Person.HusbandChanged += value;
			}
			remove
			{
				this.Death.Person.HusbandChanged -= value;
			}
		}
		public virtual ValueType1 ValueType1DoesSomethingElseWith
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingElseWith;
			}
			set
			{
				this.Death.Person.ValueType1DoesSomethingElseWith = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged += value;
			}
			remove
			{
				this.Death.Person.ValueType1DoesSomethingElseWithChanged -= value;
			}
		}
		public virtual MalePerson MalePerson
		{
			get
			{
				return this.Death.Person.MalePerson;
			}
			set
			{
				this.Death.Person.MalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				this.Death.Person.MalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				this.Death.Person.MalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.MalePersonChanged -= value;
			}
		}
		public virtual FemalePerson FemalePerson
		{
			get
			{
				return this.Death.Person.FemalePerson;
			}
			set
			{
				this.Death.Person.FemalePerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				this.Death.Person.FemalePersonChanging += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				this.Death.Person.FemalePersonChanged += value;
			}
			remove
			{
				this.Death.Person.FemalePersonChanged -= value;
			}
		}
		public virtual ChildPerson ChildPerson
		{
			get
			{
				return this.Death.Person.ChildPerson;
			}
			set
			{
				this.Death.Person.ChildPerson = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				this.Death.Person.ChildPersonChanging += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				this.Death.Person.ChildPersonChanged += value;
			}
			remove
			{
				this.Death.Person.ChildPersonChanged -= value;
			}
		}
		public virtual Person Wife
		{
			get
			{
				return this.Death.Person.Wife;
			}
			set
			{
				this.Death.Person.Wife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				this.Death.Person.WifeChanging += value;
			}
			remove
			{
				this.Death.Person.WifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				this.Death.Person.WifeChanged += value;
			}
			remove
			{
				this.Death.Person.WifeChanged -= value;
			}
		}
		public virtual IEnumerable<Task> TaskViaPersonCollection
		{
			get
			{
				return this.Death.Person.TaskViaPersonCollection;
			}
		}
		public virtual IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
		{
			get
			{
				return this.Death.Person.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			}
		}
		public virtual IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
		{
			get
			{
				return this.Death.Person.PersonBoughtCarFromPersonOnDateViaSellerCollection;
			}
		}
	}
	#endregion // UnnaturalDeath
	#region Task
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Task : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Task()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[2]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Task, Person>> PersonChanging
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
		protected bool OnPersonChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<Task, Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Task, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Task, Person>>(eventHandler, this, new PropertyChangingEventArgs<Task, Person>(this, this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Task, Person>> PersonChanged
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
		protected void OnPersonChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Task, Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Task, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Task, Person>>(eventHandler, this, new PropertyChangedEventArgs<Task, Person>(this, oldValue, this.Person));
				this.OnPropertyChanged("Person");
			}
		}
		[DataObjectField(false, false, true)]
		public abstract Person Person
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "Task{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // Task
	#region ValueType1
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class ValueType1 : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected ValueType1()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[4]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<ValueType1, int>> ValueType1ValueChanging
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
		protected bool OnValueType1ValueChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<ValueType1, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<ValueType1, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ValueType1, int>>(eventHandler, this, new PropertyChangingEventArgs<ValueType1, int>(this, this.ValueType1Value, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ValueType1, int>> ValueType1ValueChanged
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
		protected void OnValueType1ValueChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ValueType1, int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<ValueType1, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ValueType1, int>>(eventHandler, this, new PropertyChangedEventArgs<ValueType1, int>(this, oldValue, this.ValueType1Value));
				this.OnPropertyChanged("ValueType1Value");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ValueType1, Person>> DoesSomethingWithPersonChanging
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
		protected bool OnDoesSomethingWithPersonChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<ValueType1, Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<ValueType1, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ValueType1, Person>>(eventHandler, this, new PropertyChangingEventArgs<ValueType1, Person>(this, this.DoesSomethingWithPerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ValueType1, Person>> DoesSomethingWithPersonChanged
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
		protected void OnDoesSomethingWithPersonChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<ValueType1, Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<ValueType1, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ValueType1, Person>>(eventHandler, this, new PropertyChangedEventArgs<ValueType1, Person>(this, oldValue, this.DoesSomethingWithPerson));
				this.OnPropertyChanged("DoesSomethingWithPerson");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract int ValueType1Value
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Person DoesSomethingWithPerson
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<Person> DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}DoesSomethingWithPerson = {3}{0}}}", Environment.NewLine, @"	", this.ValueType1Value, "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // ValueType1
	#region PersonBoughtCarFromPersonOnDate
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class PersonBoughtCarFromPersonOnDate : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonBoughtCarFromPersonOnDate()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[8]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> CarSold_vinChanging
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
		protected bool OnCarSold_vinChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, this.CarSold_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> CarSold_vinChanged
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
		protected void OnCarSold_vinChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, oldValue, this.CarSold_vin));
				this.OnPropertyChanged("CarSold_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> SaleDate_YMDChanging
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
		protected bool OnSaleDate_YMDChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, this.SaleDate_YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> SaleDate_YMDChanged
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
		protected void OnSaleDate_YMDChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, oldValue, this.SaleDate_YMD));
				this.OnPropertyChanged("SaleDate_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> BuyerChanging
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
		protected bool OnBuyerChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, this.Buyer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> BuyerChanged
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
		protected void OnBuyerChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, oldValue, this.Buyer));
				this.OnPropertyChanged("Buyer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> SellerChanging
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
		protected bool OnSellerChanging(Person newValue)
		{
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, this.Seller, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> SellerChanged
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
		protected void OnSellerChanged(Person oldValue)
		{
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, oldValue, this.Seller));
				this.OnPropertyChanged("Seller");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract int CarSold_vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract int SaleDate_YMD
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Person Buyer
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Person Seller
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"PersonBoughtCarFromPersonOnDate{0}{{{0}{1}CarSold_vin = ""{2}"",{0}{1}SaleDate_YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, @"	", this.CarSold_vin, this.SaleDate_YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // PersonBoughtCarFromPersonOnDate
	#region Review
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Review : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Review()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[6]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract SampleModelContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Review, int>> Car_vinChanging
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
		protected bool OnCar_vinChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<Review, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, int>>(eventHandler, this, new PropertyChangingEventArgs<Review, int>(this, this.Car_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, int>> Car_vinChanged
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
		protected void OnCar_vinChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Review, int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, int>>(eventHandler, this, new PropertyChangedEventArgs<Review, int>(this, oldValue, this.Car_vin));
				this.OnPropertyChanged("Car_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, int>> Rating_Nr_IntegerChanging
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
		protected bool OnRating_Nr_IntegerChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<Review, int>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, int>>(eventHandler, this, new PropertyChangingEventArgs<Review, int>(this, this.Rating_Nr_Integer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, int>> Rating_Nr_IntegerChanged
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
		protected void OnRating_Nr_IntegerChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Review, int>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Review, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, int>>(eventHandler, this, new PropertyChangedEventArgs<Review, int>(this, oldValue, this.Rating_Nr_Integer));
				this.OnPropertyChanged("Rating_Nr_Integer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, string>> Criterion_NameChanging
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
		protected bool OnCriterion_NameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Review, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Review, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, string>>(eventHandler, this, new PropertyChangingEventArgs<Review, string>(this, this.Criterion_Name, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, string>> Criterion_NameChanged
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
		protected void OnCriterion_NameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Review, string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<Review, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, string>>(eventHandler, this, new PropertyChangedEventArgs<Review, string>(this, oldValue, this.Criterion_Name));
				this.OnPropertyChanged("Criterion_Name");
			}
		}
		[DataObjectField(false, false, false)]
		public abstract int Car_vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract int Rating_Nr_Integer
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string Criterion_Name
		{
			get;
			set;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criterion_Name = ""{4}""{0}}}", Environment.NewLine, @"	", this.Car_vin, this.Rating_Nr_Integer, this.Criterion_Name);
		}
	}
	#endregion // Review
	#region IHasSampleModelContext
	[GeneratedCode("OIALtoPLiX", "1.0")]
	public interface IHasSampleModelContext
	{
		SampleModelContext Context
		{
			get;
		}
	}
	#endregion // IHasSampleModelContext
	#region ISampleModelContext
	[GeneratedCode("OIALtoPLiX", "1.0")]
	public interface ISampleModelContext
	{
		ChildPerson GetChildPersonByInternalUniquenessConstraint49(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother);
		bool TryGetChildPersonByInternalUniquenessConstraint49(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother, out ChildPerson ChildPerson);
		Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD);
		bool TryGetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD, out Person Person);
		Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD);
		bool TryGetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD, out Person Person);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller);
		bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin);
		bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer);
		bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate);
		Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name);
		bool TryGetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name, out Review Review);
		Person GetPersonByOptionalUniqueString(string OptionalUniqueString);
		bool TryGetPersonByOptionalUniqueString(string OptionalUniqueString, out Person Person);
		Person GetPersonByOwnsCar_vin(int OwnsCar_vin);
		bool TryGetPersonByOwnsCar_vin(int OwnsCar_vin, out Person Person);
		Person GetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal);
		bool TryGetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal, out Person Person);
		Person GetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal);
		bool TryGetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal, out Person Person);
		Person GetPersonByMandatoryUniqueString(string MandatoryUniqueString);
		bool TryGetPersonByMandatoryUniqueString(string MandatoryUniqueString, out Person Person);
		ValueType1 GetValueType1ByValueType1Value(int ValueType1Value);
		bool TryGetValueType1ByValueType1Value(int ValueType1Value, out ValueType1 ValueType1);
		Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, bool hasParents, decimal MandatoryUniqueDecimal, string MandatoryUniqueString);
		IEnumerable<Person> PersonCollection
		{
			get;
		}
		MalePerson CreateMalePerson(Person Person);
		IEnumerable<MalePerson> MalePersonCollection
		{
			get;
		}
		FemalePerson CreateFemalePerson(Person Person);
		IEnumerable<FemalePerson> FemalePersonCollection
		{
			get;
		}
		ChildPerson CreateChildPerson(int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person);
		IEnumerable<ChildPerson> ChildPersonCollection
		{
			get;
		}
		Death CreateDeath(string DeathCause_DeathCause_Type, Person Person);
		IEnumerable<Death> DeathCollection
		{
			get;
		}
		NaturalDeath CreateNaturalDeath(bool isFromProstateCancer, Death Death);
		IEnumerable<NaturalDeath> NaturalDeathCollection
		{
			get;
		}
		UnnaturalDeath CreateUnnaturalDeath(bool isViolent, bool isBloody, Death Death);
		IEnumerable<UnnaturalDeath> UnnaturalDeathCollection
		{
			get;
		}
		Task CreateTask();
		IEnumerable<Task> TaskCollection
		{
			get;
		}
		ValueType1 CreateValueType1(int ValueType1Value);
		IEnumerable<ValueType1> ValueType1Collection
		{
			get;
		}
		PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller);
		IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
		{
			get;
		}
		Review CreateReview(int Car_vin, int Rating_Nr_Integer, string Criterion_Name);
		IEnumerable<Review> ReviewCollection
		{
			get;
		}
	}
	#endregion // ISampleModelContext
}

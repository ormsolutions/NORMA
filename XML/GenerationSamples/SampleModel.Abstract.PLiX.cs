using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace SampleModel
{
	#region Person
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Person : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Person()
		{
		}
		#region Person INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // Person INotifyPropertyChanged Implementation
		#region Person Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[52], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnFirstNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "FirstName", this.FirstName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnFirstNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "FirstName", oldValue, this.FirstName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnLastNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "LastName", this.LastName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnLastNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "LastName", oldValue, this.LastName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> OptionalUniqueStringChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnOptionalUniqueStringChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "OptionalUniqueString", this.OptionalUniqueString, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> OptionalUniqueStringChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnOptionalUniqueStringChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "OptionalUniqueString", oldValue, this.OptionalUniqueString), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OptionalUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[6], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[6], value);
				}
			}
		}
		protected bool OnColorARGBChanging(Nullable<int> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<int>>(this, "ColorARGB", this.ColorARGB, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[7], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[7], value);
				}
			}
		}
		protected void OnColorARGBChanged(Nullable<int> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, "ColorARGB", oldValue, this.ColorARGB), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("ColorARGB");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[8], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[8], value);
				}
			}
		}
		protected bool OnHatTypeStyleDescriptionChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[8]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "HatTypeStyleDescription", this.HatTypeStyleDescription, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[9], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[9], value);
				}
			}
		}
		protected void OnHatTypeStyleDescriptionChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[9]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "HatTypeStyleDescription", oldValue, this.HatTypeStyleDescription), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("HatTypeStyleDescription");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[10], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[10], value);
				}
			}
		}
		protected bool OnVinChanging(Nullable<uint> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>>)events[10]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<uint>>(this, "Vin", this.Vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[11], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[11], value);
				}
			}
		}
		protected void OnVinChanged(Nullable<uint> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>>)events[11]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<uint>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<uint>>(this, "Vin", oldValue, this.Vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[12], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[12], value);
				}
			}
		}
		protected bool OnYMDChanging(System.DateTime newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, System.DateTime>>)events[12]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, System.DateTime>>(eventHandler, this, new PropertyChangingEventArgs<Person, System.DateTime>(this, "YMD", this.YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[13], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[13], value);
				}
			}
		}
		protected void OnYMDChanged(System.DateTime oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, System.DateTime>>)events[13]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, System.DateTime>>(eventHandler, this, new PropertyChangedEventArgs<Person, System.DateTime>(this, "YMD", oldValue, this.YMD), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[14], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[14], value);
				}
			}
		}
		protected bool OnIsDeadChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>)events[14]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<bool>>(this, "IsDead", this.IsDead, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[15], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[15], value);
				}
			}
		}
		protected void OnIsDeadChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>>)events[15]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<bool>>(this, "IsDead", oldValue, this.IsDead), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("IsDead");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[16], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[16], value);
				}
			}
		}
		protected bool OnGenderCodeChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[16]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "GenderCode", this.GenderCode, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[17], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[17], value);
				}
			}
		}
		protected void OnGenderCodeChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[17]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "GenderCode", oldValue, this.GenderCode), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("GenderCode");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[18], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[18], value);
				}
			}
		}
		protected bool OnHasParentsChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>)events[18]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<bool>>(this, "HasParents", this.HasParents, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[19], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[19], value);
				}
			}
		}
		protected void OnHasParentsChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>>)events[19]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<bool>>(this, "HasParents", oldValue, this.HasParents), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("HasParents");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[20], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[20], value);
				}
			}
		}
		protected bool OnOptionalUniqueDecimalChanging(Nullable<int> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>)events[20]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<int>>(this, "OptionalUniqueDecimal", this.OptionalUniqueDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[21], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[21], value);
				}
			}
		}
		protected void OnOptionalUniqueDecimalChanged(Nullable<int> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>)events[21]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, "OptionalUniqueDecimal", oldValue, this.OptionalUniqueDecimal), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OptionalUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[22], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[22], value);
				}
			}
		}
		protected bool OnMandatoryUniqueDecimalChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, int>>)events[22]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, int>>(eventHandler, this, new PropertyChangingEventArgs<Person, int>(this, "MandatoryUniqueDecimal", this.MandatoryUniqueDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[23], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[23], value);
				}
			}
		}
		protected void OnMandatoryUniqueDecimalChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, int>>)events[23]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, int>>(eventHandler, this, new PropertyChangedEventArgs<Person, int>(this, "MandatoryUniqueDecimal", oldValue, this.MandatoryUniqueDecimal), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> MandatoryUniqueStringChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[24], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[24], value);
				}
			}
		}
		protected bool OnMandatoryUniqueStringChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[24]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "MandatoryUniqueString", this.MandatoryUniqueString, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> MandatoryUniqueStringChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[25], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[25], value);
				}
			}
		}
		protected void OnMandatoryUniqueStringChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[25]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "MandatoryUniqueString", oldValue, this.MandatoryUniqueString), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[26], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[26], value);
				}
			}
		}
		protected bool OnOptionalUniqueTinyIntChanging(Nullable<byte> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>>)events[26]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<byte>>(this, "OptionalUniqueTinyInt", this.OptionalUniqueTinyInt, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[27], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[27], value);
				}
			}
		}
		protected void OnOptionalUniqueTinyIntChanged(Nullable<byte> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>>)events[27]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<byte>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<byte>>(this, "OptionalUniqueTinyInt", oldValue, this.OptionalUniqueTinyInt), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OptionalUniqueTinyInt");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[28], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[28], value);
				}
			}
		}
		protected bool OnMandatoryUniqueTinyIntChanging(byte newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, byte>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, byte>>)events[28]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, byte>>(eventHandler, this, new PropertyChangingEventArgs<Person, byte>(this, "MandatoryUniqueTinyInt", this.MandatoryUniqueTinyInt, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[29], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[29], value);
				}
			}
		}
		protected void OnMandatoryUniqueTinyIntChanged(byte oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, byte>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, byte>>)events[29]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, byte>>(eventHandler, this, new PropertyChangedEventArgs<Person, byte>(this, "MandatoryUniqueTinyInt", oldValue, this.MandatoryUniqueTinyInt), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryUniqueTinyInt");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[30], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[30], value);
				}
			}
		}
		protected bool OnOptionalNonUniqueTinyIntChanging(Nullable<byte> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>>)events[30]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<byte>>(this, "OptionalNonUniqueTinyInt", this.OptionalNonUniqueTinyInt, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[31], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[31], value);
				}
			}
		}
		protected void OnOptionalNonUniqueTinyIntChanged(Nullable<byte> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>>)events[31]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<byte>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<byte>>(this, "OptionalNonUniqueTinyInt", oldValue, this.OptionalNonUniqueTinyInt), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OptionalNonUniqueTinyInt");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[32], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[32], value);
				}
			}
		}
		protected bool OnMandatoryNonUniqueTinyIntChanging(byte newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, byte>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, byte>>)events[32]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, byte>>(eventHandler, this, new PropertyChangingEventArgs<Person, byte>(this, "MandatoryNonUniqueTinyInt", this.MandatoryNonUniqueTinyInt, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[33], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[33], value);
				}
			}
		}
		protected void OnMandatoryNonUniqueTinyIntChanged(byte oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, byte>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, byte>>)events[33]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, byte>>(eventHandler, this, new PropertyChangedEventArgs<Person, byte>(this, "MandatoryNonUniqueTinyInt", oldValue, this.MandatoryNonUniqueTinyInt), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryNonUniqueTinyInt");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[34], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[34], value);
				}
			}
		}
		protected bool OnMandatoryNonUniqueUnconstrainedDecimalChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, int>>)events[34]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, int>>(eventHandler, this, new PropertyChangingEventArgs<Person, int>(this, "MandatoryNonUniqueUnconstrainedDecimal", this.MandatoryNonUniqueUnconstrainedDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[35], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[35], value);
				}
			}
		}
		protected void OnMandatoryNonUniqueUnconstrainedDecimalChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, int>>)events[35]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, int>>(eventHandler, this, new PropertyChangedEventArgs<Person, int>(this, "MandatoryNonUniqueUnconstrainedDecimal", oldValue, this.MandatoryNonUniqueUnconstrainedDecimal), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryNonUniqueUnconstrainedDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[36], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[36], value);
				}
			}
		}
		protected bool OnMandatoryNonUniqueUnconstrainedFloatChanging(float newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, float>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, float>>)events[36]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, float>>(eventHandler, this, new PropertyChangingEventArgs<Person, float>(this, "MandatoryNonUniqueUnconstrainedFloat", this.MandatoryNonUniqueUnconstrainedFloat, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[37], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[37], value);
				}
			}
		}
		protected void OnMandatoryNonUniqueUnconstrainedFloatChanged(float oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, float>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, float>>)events[37]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, float>>(eventHandler, this, new PropertyChangedEventArgs<Person, float>(this, "MandatoryNonUniqueUnconstrainedFloat", oldValue, this.MandatoryNonUniqueUnconstrainedFloat), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryNonUniqueUnconstrainedFloat");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[38], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[38], value);
				}
			}
		}
		protected bool OnWifeChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Person>>)events[38]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Person>>(eventHandler, this, new PropertyChangingEventArgs<Person, Person>(this, "Wife", this.Wife, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> WifeChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[39], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[39], value);
				}
			}
		}
		protected void OnWifeChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Person>>)events[39]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Person>>(eventHandler, this, new PropertyChangedEventArgs<Person, Person>(this, "Wife", oldValue, this.Wife), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Wife");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[40], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[40], value);
				}
			}
		}
		protected bool OnValueType1DoesSomethingElseWithChanging(ValueType1 newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, ValueType1>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, ValueType1>>)events[40]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, ValueType1>>(eventHandler, this, new PropertyChangingEventArgs<Person, ValueType1>(this, "ValueType1DoesSomethingElseWith", this.ValueType1DoesSomethingElseWith, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[41], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[41], value);
				}
			}
		}
		protected void OnValueType1DoesSomethingElseWithChanged(ValueType1 oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, ValueType1>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, ValueType1>>)events[41]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, ValueType1>>(eventHandler, this, new PropertyChangedEventArgs<Person, ValueType1>(this, "ValueType1DoesSomethingElseWith", oldValue, this.ValueType1DoesSomethingElseWith), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("ValueType1DoesSomethingElseWith");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, MalePerson>> MalePersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[42], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[42], value);
				}
			}
		}
		protected bool OnMalePersonChanging(MalePerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, MalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, MalePerson>>)events[42]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, MalePerson>>(eventHandler, this, new PropertyChangingEventArgs<Person, MalePerson>(this, "MalePerson", this.MalePerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, MalePerson>> MalePersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[43], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[43], value);
				}
			}
		}
		protected void OnMalePersonChanged(MalePerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, MalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, MalePerson>>)events[43]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, MalePerson>>(eventHandler, this, new PropertyChangedEventArgs<Person, MalePerson>(this, "MalePerson", oldValue, this.MalePerson), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MalePerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> FemalePersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[44], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[44], value);
				}
			}
		}
		protected bool OnFemalePersonChanging(FemalePerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, FemalePerson>>)events[44]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, FemalePerson>>(eventHandler, this, new PropertyChangingEventArgs<Person, FemalePerson>(this, "FemalePerson", this.FemalePerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> FemalePersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[45], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[45], value);
				}
			}
		}
		protected void OnFemalePersonChanged(FemalePerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, FemalePerson>>)events[45]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, FemalePerson>>(eventHandler, this, new PropertyChangedEventArgs<Person, FemalePerson>(this, "FemalePerson", oldValue, this.FemalePerson), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("FemalePerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> ChildPersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[46], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[46], value);
				}
			}
		}
		protected bool OnChildPersonChanging(ChildPerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, ChildPerson>>)events[46]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, ChildPerson>>(eventHandler, this, new PropertyChangingEventArgs<Person, ChildPerson>(this, "ChildPerson", this.ChildPerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> ChildPersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[47], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[47], value);
				}
			}
		}
		protected void OnChildPersonChanged(ChildPerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, ChildPerson>>)events[47]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, ChildPerson>>(eventHandler, this, new PropertyChangedEventArgs<Person, ChildPerson>(this, "ChildPerson", oldValue, this.ChildPerson), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("ChildPerson");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Death>> DeathChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[48], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[48], value);
				}
			}
		}
		protected bool OnDeathChanging(Death newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Death>>)events[48]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Death>>(eventHandler, this, new PropertyChangingEventArgs<Person, Death>(this, "Death", this.Death, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Death>> DeathChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[49], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[49], value);
				}
			}
		}
		protected void OnDeathChanged(Death oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Death>>)events[49]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Death>>(eventHandler, this, new PropertyChangedEventArgs<Person, Death>(this, "Death", oldValue, this.Death), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Death");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[50], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[50], value);
				}
			}
		}
		protected bool OnHusbandViaWifeChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Person>>)events[50]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Person>>(eventHandler, this, new PropertyChangingEventArgs<Person, Person>(this, "HusbandViaWife", this.HusbandViaWife, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				if ((object)value != null)
				{
					Person.InterlockedDelegateCombine(ref this.Events[51], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Person.InterlockedDelegateRemove(ref events[51], value);
				}
			}
		}
		protected void OnHusbandViaWifeChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Person>>)events[51]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Person>>(eventHandler, this, new PropertyChangedEventArgs<Person, Person>(this, "HusbandViaWife", oldValue, this.HusbandViaWife), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("HusbandViaWife");
			}
		}
		#endregion // Person Property Change Events
		#region Person Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract string FirstName
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
		public abstract Nullable<int> ColorARGB
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract string HatTypeStyleDescription
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<uint> Vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract System.DateTime YMD
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> IsDead
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string GenderCode
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> HasParents
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<int> OptionalUniqueDecimal
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract int MandatoryUniqueDecimal
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
		public abstract Nullable<byte> OptionalUniqueTinyInt
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract byte MandatoryUniqueTinyInt
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<byte> OptionalNonUniqueTinyInt
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract byte MandatoryNonUniqueTinyInt
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract int MandatoryNonUniqueUnconstrainedDecimal
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract float MandatoryNonUniqueUnconstrainedFloat
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
		public abstract Person HusbandViaWife
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
		public abstract IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
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
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get;
		}
		#endregion // Person Abstract Properties
		#region Person ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}LastName = ""{3}"",{0}{1}OptionalUniqueString = ""{4}"",{0}{1}ColorARGB = ""{5}"",{0}{1}HatTypeStyleDescription = ""{6}"",{0}{1}Vin = ""{7}"",{0}{1}YMD = ""{8}"",{0}{1}IsDead = ""{9}"",{0}{1}GenderCode = ""{10}"",{0}{1}HasParents = ""{11}"",{0}{1}OptionalUniqueDecimal = ""{12}"",{0}{1}MandatoryUniqueDecimal = ""{13}"",{0}{1}MandatoryUniqueString = ""{14}"",{0}{1}OptionalUniqueTinyInt = ""{15}"",{0}{1}MandatoryUniqueTinyInt = ""{16}"",{0}{1}OptionalNonUniqueTinyInt = ""{17}"",{0}{1}MandatoryNonUniqueTinyInt = ""{18}"",{0}{1}MandatoryNonUniqueUnconstrainedDecimal = ""{19}"",{0}{1}MandatoryNonUniqueUnconstrainedFloat = ""{20}"",{0}{1}Wife = {21},{0}{1}ValueType1DoesSomethingElseWith = {22},{0}{1}MalePerson = {23},{0}{1}FemalePerson = {24},{0}{1}ChildPerson = {25},{0}{1}Death = {26},{0}{1}HusbandViaWife = {27}{0}}}", Environment.NewLine, @"	", this.FirstName, this.LastName, this.OptionalUniqueString, this.ColorARGB, this.HatTypeStyleDescription, this.Vin, this.YMD, this.IsDead, this.GenderCode, this.HasParents, this.OptionalUniqueDecimal, this.MandatoryUniqueDecimal, this.MandatoryUniqueString, this.OptionalUniqueTinyInt, this.MandatoryUniqueTinyInt, this.OptionalNonUniqueTinyInt, this.MandatoryNonUniqueTinyInt, this.MandatoryNonUniqueUnconstrainedDecimal, this.MandatoryNonUniqueUnconstrainedFloat, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // Person ToString Methods
		#region Person Children Support
		#region Person Child Support (Death)
		public static explicit operator Death(Person person)
		{
			if ((object)person == null)
			{
				return null;
			}
			Death death;
			if ((object)(death = person.Death) == null)
			{
				throw new InvalidCastException();
			}
			return death;
		}
		#endregion // Person Child Support (Death)
		#region Person Child Support (MalePerson)
		public static explicit operator MalePerson(Person person)
		{
			if ((object)person == null)
			{
				return null;
			}
			MalePerson malePerson;
			if ((object)(malePerson = person.MalePerson) == null)
			{
				throw new InvalidCastException();
			}
			return malePerson;
		}
		#endregion // Person Child Support (MalePerson)
		#region Person Child Support (FemalePerson)
		public static explicit operator FemalePerson(Person person)
		{
			if ((object)person == null)
			{
				return null;
			}
			FemalePerson femalePerson;
			if ((object)(femalePerson = person.FemalePerson) == null)
			{
				throw new InvalidCastException();
			}
			return femalePerson;
		}
		#endregion // Person Child Support (FemalePerson)
		#region Person Child Support (ChildPerson)
		public static explicit operator ChildPerson(Person person)
		{
			if ((object)person == null)
			{
				return null;
			}
			ChildPerson childPerson;
			if ((object)(childPerson = person.ChildPerson) == null)
			{
				throw new InvalidCastException();
			}
			return childPerson;
		}
		#endregion // Person Child Support (ChildPerson)
		#endregion // Person Children Support
	}
	#endregion // Person
	#region Task
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Task : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Task()
		{
		}
		#region Task INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // Task INotifyPropertyChanged Implementation
		#region Task Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[2], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Task, Person>> PersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					Task.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Task.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Task, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Task, Person>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Task, Person>>(eventHandler, this, new PropertyChangingEventArgs<Task, Person>(this, "Person", this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Task, Person>> PersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					Task.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Task.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Task, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Task, Person>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Task, Person>>(eventHandler, this, new PropertyChangedEventArgs<Task, Person>(this, "Person", oldValue, this.Person), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Person");
			}
		}
		#endregion // Task Property Change Events
		#region Task Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract Person Person
		{
			get;
			set;
		}
		#endregion // Task Abstract Properties
		#region Task ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "Task{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // Task ToString Methods
	}
	#endregion // Task
	#region ValueType1
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class ValueType1 : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected ValueType1()
		{
		}
		#region ValueType1 INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // ValueType1 INotifyPropertyChanged Implementation
		#region ValueType1 Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[4], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ValueType1, int>> ValueType1ValueChanging
		{
			add
			{
				if ((object)value != null)
				{
					ValueType1.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ValueType1.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnValueType1ValueChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ValueType1, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ValueType1, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ValueType1, int>>(eventHandler, this, new PropertyChangingEventArgs<ValueType1, int>(this, "ValueType1Value", this.ValueType1Value, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ValueType1, int>> ValueType1ValueChanged
		{
			add
			{
				if ((object)value != null)
				{
					ValueType1.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ValueType1.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnValueType1ValueChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ValueType1, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ValueType1, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ValueType1, int>>(eventHandler, this, new PropertyChangedEventArgs<ValueType1, int>(this, "ValueType1Value", oldValue, this.ValueType1Value), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("ValueType1Value");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ValueType1, Person>> DoesSomethingWithPersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					ValueType1.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ValueType1.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnDoesSomethingWithPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ValueType1, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ValueType1, Person>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ValueType1, Person>>(eventHandler, this, new PropertyChangingEventArgs<ValueType1, Person>(this, "DoesSomethingWithPerson", this.DoesSomethingWithPerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ValueType1, Person>> DoesSomethingWithPersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					ValueType1.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ValueType1.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnDoesSomethingWithPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ValueType1, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ValueType1, Person>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ValueType1, Person>>(eventHandler, this, new PropertyChangedEventArgs<ValueType1, Person>(this, "DoesSomethingWithPerson", oldValue, this.DoesSomethingWithPerson), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("DoesSomethingWithPerson");
			}
		}
		#endregion // ValueType1 Property Change Events
		#region ValueType1 Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
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
		#endregion // ValueType1 Abstract Properties
		#region ValueType1 ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}DoesSomethingWithPerson = {3}{0}}}", Environment.NewLine, @"	", this.ValueType1Value, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // ValueType1 ToString Methods
	}
	#endregion // ValueType1
	#region Death
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Death : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Death()
		{
		}
		#region Death INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // Death INotifyPropertyChanged Implementation
		#region Death Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[10], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<System.DateTime>>> YMDChanging
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnYMDChanging(Nullable<System.DateTime> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, Nullable<System.DateTime>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, Nullable<System.DateTime>>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, Nullable<System.DateTime>>>(eventHandler, this, new PropertyChangingEventArgs<Death, Nullable<System.DateTime>>(this, "YMD", this.YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<System.DateTime>>> YMDChanged
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnYMDChanged(Nullable<System.DateTime> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, Nullable<System.DateTime>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, Nullable<System.DateTime>>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, Nullable<System.DateTime>>>(eventHandler, this, new PropertyChangedEventArgs<Death, Nullable<System.DateTime>>(this, "YMD", oldValue, this.YMD), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCauseTypeChanging
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnDeathCauseTypeChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, string>>(eventHandler, this, new PropertyChangingEventArgs<Death, string>(this, "DeathCauseType", this.DeathCauseType, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCauseTypeChanged
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnDeathCauseTypeChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, string>>(eventHandler, this, new PropertyChangedEventArgs<Death, string>(this, "DeathCauseType", oldValue, this.DeathCauseType), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("DeathCauseType");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> NaturalDeathChanging
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnNaturalDeathChanging(NaturalDeath newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, NaturalDeath>>(eventHandler, this, new PropertyChangingEventArgs<Death, NaturalDeath>(this, "NaturalDeath", this.NaturalDeath, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> NaturalDeathChanged
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnNaturalDeathChanged(NaturalDeath oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, NaturalDeath>>(eventHandler, this, new PropertyChangedEventArgs<Death, NaturalDeath>(this, "NaturalDeath", oldValue, this.NaturalDeath), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("NaturalDeath");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanging
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[6], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[6], value);
				}
			}
		}
		protected bool OnUnnaturalDeathChanging(UnnaturalDeath newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>>(eventHandler, this, new PropertyChangingEventArgs<Death, UnnaturalDeath>(this, "UnnaturalDeath", this.UnnaturalDeath, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> UnnaturalDeathChanged
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[7], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[7], value);
				}
			}
		}
		protected void OnUnnaturalDeathChanged(UnnaturalDeath oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, UnnaturalDeath>>(eventHandler, this, new PropertyChangedEventArgs<Death, UnnaturalDeath>(this, "UnnaturalDeath", oldValue, this.UnnaturalDeath), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("UnnaturalDeath");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Person>> PersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[8], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[8], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, Person>>)events[8]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, Person>>(eventHandler, this, new PropertyChangingEventArgs<Death, Person>(this, "Person", this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Person>> PersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					Death.InterlockedDelegateCombine(ref this.Events[9], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[9], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, Person>>)events[9]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, Person>>(eventHandler, this, new PropertyChangedEventArgs<Death, Person>(this, "Person", oldValue, this.Person), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Person");
			}
		}
		#endregion // Death Property Change Events
		#region Death Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<System.DateTime> YMD
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string DeathCauseType
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
		#endregion // Death Abstract Properties
		#region Death ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Death{0}{{{0}{1}YMD = ""{2}"",{0}{1}DeathCauseType = ""{3}"",{0}{1}NaturalDeath = {4},{0}{1}UnnaturalDeath = {5},{0}{1}Person = {6}{0}}}", Environment.NewLine, @"	", this.YMD, this.DeathCauseType, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // Death ToString Methods
		#region Death Parent Support (Person)
		public static implicit operator Person(Death death)
		{
			if ((object)death == null)
			{
				return null;
			}
			return death.Person;
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
		public virtual Nullable<int> ColorARGB
		{
			get
			{
				return this.Person.ColorARGB;
			}
			set
			{
				this.Person.ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				this.Person.ColorARGBChanging += value;
			}
			remove
			{
				this.Person.ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				this.Person.ColorARGBChanged += value;
			}
			remove
			{
				this.Person.ColorARGBChanged -= value;
			}
		}
		public virtual string HatTypeStyleDescription
		{
			get
			{
				return this.Person.HatTypeStyleDescription;
			}
			set
			{
				this.Person.HatTypeStyleDescription = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanging += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanged += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanged -= value;
			}
		}
		public virtual Nullable<uint> Vin
		{
			get
			{
				return this.Person.Vin;
			}
			set
			{
				this.Person.Vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				this.Person.VinChanging += value;
			}
			remove
			{
				this.Person.VinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				this.Person.VinChanged += value;
			}
			remove
			{
				this.Person.VinChanged -= value;
			}
		}
		public virtual System.DateTime YMD
		{
			get
			{
				return this.Person.YMD;
			}
			set
			{
				this.Person.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				this.Person.YMDChanging += value;
			}
			remove
			{
				this.Person.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				this.Person.YMDChanged += value;
			}
			remove
			{
				this.Person.YMDChanged -= value;
			}
		}
		public virtual Nullable<bool> IsDead
		{
			get
			{
				return this.Person.IsDead;
			}
			set
			{
				this.Person.IsDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				this.Person.IsDeadChanging += value;
			}
			remove
			{
				this.Person.IsDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				this.Person.IsDeadChanged += value;
			}
			remove
			{
				this.Person.IsDeadChanged -= value;
			}
		}
		public virtual string GenderCode
		{
			get
			{
				return this.Person.GenderCode;
			}
			set
			{
				this.Person.GenderCode = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				this.Person.GenderCodeChanging += value;
			}
			remove
			{
				this.Person.GenderCodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				this.Person.GenderCodeChanged += value;
			}
			remove
			{
				this.Person.GenderCodeChanged -= value;
			}
		}
		public virtual Nullable<bool> HasParents
		{
			get
			{
				return this.Person.HasParents;
			}
			set
			{
				this.Person.HasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				this.Person.HasParentsChanging += value;
			}
			remove
			{
				this.Person.HasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				this.Person.HasParentsChanged += value;
			}
			remove
			{
				this.Person.HasParentsChanged -= value;
			}
		}
		public virtual Nullable<int> OptionalUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
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
		public virtual int MandatoryUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
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
		public virtual Nullable<byte> OptionalUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanged -= value;
			}
		}
		public virtual Nullable<byte> OptionalNonUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalNonUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryNonUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryNonUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual int MandatoryNonUniqueUnconstrainedDecimal
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedDecimal;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged -= value;
			}
		}
		public virtual float MandatoryNonUniqueUnconstrainedFloat
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedFloat;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloat = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged -= value;
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
		public virtual Person HusbandViaWife
		{
			get
			{
				return this.Person.HusbandViaWife;
			}
			set
			{
				this.Person.HusbandViaWife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				this.Person.HusbandViaWifeChanging += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				this.Person.HusbandViaWifeChanged += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanged -= value;
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
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
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
			}
		}
		#endregion // Death Parent Support (Person)
		#region Death Children Support
		#region Death Child Support (NaturalDeath)
		public static explicit operator NaturalDeath(Death death)
		{
			if ((object)death == null)
			{
				return null;
			}
			NaturalDeath naturalDeath;
			if ((object)(naturalDeath = death.NaturalDeath) == null)
			{
				throw new InvalidCastException();
			}
			return naturalDeath;
		}
		#endregion // Death Child Support (NaturalDeath)
		#region Death Child Support (UnnaturalDeath)
		public static explicit operator UnnaturalDeath(Death death)
		{
			if ((object)death == null)
			{
				return null;
			}
			UnnaturalDeath unnaturalDeath;
			if ((object)(unnaturalDeath = death.UnnaturalDeath) == null)
			{
				throw new InvalidCastException();
			}
			return unnaturalDeath;
		}
		#endregion // Death Child Support (UnnaturalDeath)
		#endregion // Death Children Support
	}
	#endregion // Death
	#region NaturalDeath
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class NaturalDeath : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected NaturalDeath()
		{
		}
		#region NaturalDeath INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // NaturalDeath INotifyPropertyChanged Implementation
		#region NaturalDeath Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[4], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>> IsFromProstateCancerChanging
		{
			add
			{
				if ((object)value != null)
				{
					NaturalDeath.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					NaturalDeath.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnIsFromProstateCancerChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>(this, "IsFromProstateCancer", this.IsFromProstateCancer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>> IsFromProstateCancerChanged
		{
			add
			{
				if ((object)value != null)
				{
					NaturalDeath.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					NaturalDeath.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnIsFromProstateCancerChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>(this, "IsFromProstateCancer", oldValue, this.IsFromProstateCancer), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("IsFromProstateCancer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>> DeathChanging
		{
			add
			{
				if ((object)value != null)
				{
					NaturalDeath.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					NaturalDeath.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnDeathChanging(Death newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<NaturalDeath, Death>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NaturalDeath, Death>>(eventHandler, this, new PropertyChangingEventArgs<NaturalDeath, Death>(this, "Death", this.Death, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>> DeathChanged
		{
			add
			{
				if ((object)value != null)
				{
					NaturalDeath.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					NaturalDeath.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnDeathChanged(Death oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<NaturalDeath, Death>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NaturalDeath, Death>>(eventHandler, this, new PropertyChangedEventArgs<NaturalDeath, Death>(this, "Death", oldValue, this.Death), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Death");
			}
		}
		#endregion // NaturalDeath Property Change Events
		#region NaturalDeath Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> IsFromProstateCancer
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
		#endregion // NaturalDeath Abstract Properties
		#region NaturalDeath ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"NaturalDeath{0}{{{0}{1}IsFromProstateCancer = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, @"	", this.IsFromProstateCancer, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // NaturalDeath ToString Methods
		#region NaturalDeath Parent Support (Death)
		public static implicit operator Death(NaturalDeath naturalDeath)
		{
			if ((object)naturalDeath == null)
			{
				return null;
			}
			return naturalDeath.Death;
		}
		public static implicit operator Person(NaturalDeath naturalDeath)
		{
			if ((object)naturalDeath == null)
			{
				return null;
			}
			return naturalDeath.Death.Person;
		}
		public virtual Nullable<System.DateTime> YMD
		{
			get
			{
				return this.Death.YMD;
			}
			set
			{
				this.Death.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<System.DateTime>>> YMDChanging
		{
			add
			{
				this.Death.YMDChanging += value;
			}
			remove
			{
				this.Death.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<System.DateTime>>> YMDChanged
		{
			add
			{
				this.Death.YMDChanged += value;
			}
			remove
			{
				this.Death.YMDChanged -= value;
			}
		}
		public virtual string DeathCauseType
		{
			get
			{
				return this.Death.DeathCauseType;
			}
			set
			{
				this.Death.DeathCauseType = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCauseTypeChanging
		{
			add
			{
				this.Death.DeathCauseTypeChanging += value;
			}
			remove
			{
				this.Death.DeathCauseTypeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCauseTypeChanged
		{
			add
			{
				this.Death.DeathCauseTypeChanged += value;
			}
			remove
			{
				this.Death.DeathCauseTypeChanged -= value;
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
		public virtual Nullable<int> ColorARGB
		{
			get
			{
				return this.Death.Person.ColorARGB;
			}
			set
			{
				this.Death.Person.ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				this.Death.Person.ColorARGBChanging += value;
			}
			remove
			{
				this.Death.Person.ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				this.Death.Person.ColorARGBChanged += value;
			}
			remove
			{
				this.Death.Person.ColorARGBChanged -= value;
			}
		}
		public virtual string HatTypeStyleDescription
		{
			get
			{
				return this.Death.Person.HatTypeStyleDescription;
			}
			set
			{
				this.Death.Person.HatTypeStyleDescription = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				this.Death.Person.HatTypeStyleDescriptionChanging += value;
			}
			remove
			{
				this.Death.Person.HatTypeStyleDescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				this.Death.Person.HatTypeStyleDescriptionChanged += value;
			}
			remove
			{
				this.Death.Person.HatTypeStyleDescriptionChanged -= value;
			}
		}
		public virtual Nullable<uint> Vin
		{
			get
			{
				return this.Death.Person.Vin;
			}
			set
			{
				this.Death.Person.Vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				this.Death.Person.VinChanging += value;
			}
			remove
			{
				this.Death.Person.VinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				this.Death.Person.VinChanged += value;
			}
			remove
			{
				this.Death.Person.VinChanged -= value;
			}
		}
		public virtual System.DateTime YMD
		{
			get
			{
				return this.Death.Person.YMD;
			}
			set
			{
				this.Death.Person.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				this.Death.Person.YMDChanging += value;
			}
			remove
			{
				this.Death.Person.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				this.Death.Person.YMDChanged += value;
			}
			remove
			{
				this.Death.Person.YMDChanged -= value;
			}
		}
		public virtual Nullable<bool> IsDead
		{
			get
			{
				return this.Death.Person.IsDead;
			}
			set
			{
				this.Death.Person.IsDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				this.Death.Person.IsDeadChanging += value;
			}
			remove
			{
				this.Death.Person.IsDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				this.Death.Person.IsDeadChanged += value;
			}
			remove
			{
				this.Death.Person.IsDeadChanged -= value;
			}
		}
		public virtual string GenderCode
		{
			get
			{
				return this.Death.Person.GenderCode;
			}
			set
			{
				this.Death.Person.GenderCode = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				this.Death.Person.GenderCodeChanging += value;
			}
			remove
			{
				this.Death.Person.GenderCodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				this.Death.Person.GenderCodeChanged += value;
			}
			remove
			{
				this.Death.Person.GenderCodeChanged -= value;
			}
		}
		public virtual Nullable<bool> HasParents
		{
			get
			{
				return this.Death.Person.HasParents;
			}
			set
			{
				this.Death.Person.HasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				this.Death.Person.HasParentsChanging += value;
			}
			remove
			{
				this.Death.Person.HasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				this.Death.Person.HasParentsChanged += value;
			}
			remove
			{
				this.Death.Person.HasParentsChanged -= value;
			}
		}
		public virtual Nullable<int> OptionalUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
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
		public virtual int MandatoryUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
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
		public virtual Nullable<byte> OptionalUniqueTinyInt
		{
			get
			{
				return this.Death.Person.OptionalUniqueTinyInt;
			}
			set
			{
				this.Death.Person.OptionalUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryUniqueTinyInt
		{
			get
			{
				return this.Death.Person.MandatoryUniqueTinyInt;
			}
			set
			{
				this.Death.Person.MandatoryUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueTinyIntChanged -= value;
			}
		}
		public virtual Nullable<byte> OptionalNonUniqueTinyInt
		{
			get
			{
				return this.Death.Person.OptionalNonUniqueTinyInt;
			}
			set
			{
				this.Death.Person.OptionalNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryNonUniqueTinyInt
		{
			get
			{
				return this.Death.Person.MandatoryNonUniqueTinyInt;
			}
			set
			{
				this.Death.Person.MandatoryNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual int MandatoryNonUniqueUnconstrainedDecimal
		{
			get
			{
				return this.Death.Person.MandatoryNonUniqueUnconstrainedDecimal;
			}
			set
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanged -= value;
			}
		}
		public virtual float MandatoryNonUniqueUnconstrainedFloat
		{
			get
			{
				return this.Death.Person.MandatoryNonUniqueUnconstrainedFloat;
			}
			set
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloat = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanged -= value;
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
		public virtual Person HusbandViaWife
		{
			get
			{
				return this.Death.Person.HusbandViaWife;
			}
			set
			{
				this.Death.Person.HusbandViaWife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				this.Death.Person.HusbandViaWifeChanging += value;
			}
			remove
			{
				this.Death.Person.HusbandViaWifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				this.Death.Person.HusbandViaWifeChanged += value;
			}
			remove
			{
				this.Death.Person.HusbandViaWifeChanged -= value;
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Death.Person.PersonDrivesCarViaDrivenByPersonCollection;
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
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Death.Person.PersonHasNickNameViaPersonCollection;
			}
		}
		#endregion // NaturalDeath Parent Support (Death)
	}
	#endregion // NaturalDeath
	#region UnnaturalDeath
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class UnnaturalDeath : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected UnnaturalDeath()
		{
		}
		#region UnnaturalDeath INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // UnnaturalDeath INotifyPropertyChanged Implementation
		#region UnnaturalDeath Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[6], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> IsViolentChanging
		{
			add
			{
				if ((object)value != null)
				{
					UnnaturalDeath.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					UnnaturalDeath.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnIsViolentChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>(this, "IsViolent", this.IsViolent, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> IsViolentChanged
		{
			add
			{
				if ((object)value != null)
				{
					UnnaturalDeath.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					UnnaturalDeath.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnIsViolentChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>(this, "IsViolent", oldValue, this.IsViolent), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("IsViolent");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> IsBloodyChanging
		{
			add
			{
				if ((object)value != null)
				{
					UnnaturalDeath.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					UnnaturalDeath.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnIsBloodyChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>(this, "IsBloody", this.IsBloody, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> IsBloodyChanged
		{
			add
			{
				if ((object)value != null)
				{
					UnnaturalDeath.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					UnnaturalDeath.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnIsBloodyChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>(this, "IsBloody", oldValue, this.IsBloody), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("IsBloody");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>> DeathChanging
		{
			add
			{
				if ((object)value != null)
				{
					UnnaturalDeath.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					UnnaturalDeath.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnDeathChanging(Death newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, Death>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, Death>(this, "Death", this.Death, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>> DeathChanged
		{
			add
			{
				if ((object)value != null)
				{
					UnnaturalDeath.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					UnnaturalDeath.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnDeathChanged(Death oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Death>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, Death>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, Death>(this, "Death", oldValue, this.Death), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Death");
			}
		}
		#endregion // UnnaturalDeath Property Change Events
		#region UnnaturalDeath Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> IsViolent
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> IsBloody
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
		#endregion // UnnaturalDeath Abstract Properties
		#region UnnaturalDeath ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"UnnaturalDeath{0}{{{0}{1}IsViolent = ""{2}"",{0}{1}IsBloody = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, @"	", this.IsViolent, this.IsBloody, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // UnnaturalDeath ToString Methods
		#region UnnaturalDeath Parent Support (Death)
		public static implicit operator Death(UnnaturalDeath unnaturalDeath)
		{
			if ((object)unnaturalDeath == null)
			{
				return null;
			}
			return unnaturalDeath.Death;
		}
		public static implicit operator Person(UnnaturalDeath unnaturalDeath)
		{
			if ((object)unnaturalDeath == null)
			{
				return null;
			}
			return unnaturalDeath.Death.Person;
		}
		public virtual Nullable<System.DateTime> YMD
		{
			get
			{
				return this.Death.YMD;
			}
			set
			{
				this.Death.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<System.DateTime>>> YMDChanging
		{
			add
			{
				this.Death.YMDChanging += value;
			}
			remove
			{
				this.Death.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<System.DateTime>>> YMDChanged
		{
			add
			{
				this.Death.YMDChanged += value;
			}
			remove
			{
				this.Death.YMDChanged -= value;
			}
		}
		public virtual string DeathCauseType
		{
			get
			{
				return this.Death.DeathCauseType;
			}
			set
			{
				this.Death.DeathCauseType = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCauseTypeChanging
		{
			add
			{
				this.Death.DeathCauseTypeChanging += value;
			}
			remove
			{
				this.Death.DeathCauseTypeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCauseTypeChanged
		{
			add
			{
				this.Death.DeathCauseTypeChanged += value;
			}
			remove
			{
				this.Death.DeathCauseTypeChanged -= value;
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
		public virtual Nullable<int> ColorARGB
		{
			get
			{
				return this.Death.Person.ColorARGB;
			}
			set
			{
				this.Death.Person.ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				this.Death.Person.ColorARGBChanging += value;
			}
			remove
			{
				this.Death.Person.ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				this.Death.Person.ColorARGBChanged += value;
			}
			remove
			{
				this.Death.Person.ColorARGBChanged -= value;
			}
		}
		public virtual string HatTypeStyleDescription
		{
			get
			{
				return this.Death.Person.HatTypeStyleDescription;
			}
			set
			{
				this.Death.Person.HatTypeStyleDescription = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				this.Death.Person.HatTypeStyleDescriptionChanging += value;
			}
			remove
			{
				this.Death.Person.HatTypeStyleDescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				this.Death.Person.HatTypeStyleDescriptionChanged += value;
			}
			remove
			{
				this.Death.Person.HatTypeStyleDescriptionChanged -= value;
			}
		}
		public virtual Nullable<uint> Vin
		{
			get
			{
				return this.Death.Person.Vin;
			}
			set
			{
				this.Death.Person.Vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				this.Death.Person.VinChanging += value;
			}
			remove
			{
				this.Death.Person.VinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				this.Death.Person.VinChanged += value;
			}
			remove
			{
				this.Death.Person.VinChanged -= value;
			}
		}
		public virtual System.DateTime YMD
		{
			get
			{
				return this.Death.Person.YMD;
			}
			set
			{
				this.Death.Person.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				this.Death.Person.YMDChanging += value;
			}
			remove
			{
				this.Death.Person.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				this.Death.Person.YMDChanged += value;
			}
			remove
			{
				this.Death.Person.YMDChanged -= value;
			}
		}
		public virtual Nullable<bool> IsDead
		{
			get
			{
				return this.Death.Person.IsDead;
			}
			set
			{
				this.Death.Person.IsDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				this.Death.Person.IsDeadChanging += value;
			}
			remove
			{
				this.Death.Person.IsDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				this.Death.Person.IsDeadChanged += value;
			}
			remove
			{
				this.Death.Person.IsDeadChanged -= value;
			}
		}
		public virtual string GenderCode
		{
			get
			{
				return this.Death.Person.GenderCode;
			}
			set
			{
				this.Death.Person.GenderCode = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				this.Death.Person.GenderCodeChanging += value;
			}
			remove
			{
				this.Death.Person.GenderCodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				this.Death.Person.GenderCodeChanged += value;
			}
			remove
			{
				this.Death.Person.GenderCodeChanged -= value;
			}
		}
		public virtual Nullable<bool> HasParents
		{
			get
			{
				return this.Death.Person.HasParents;
			}
			set
			{
				this.Death.Person.HasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				this.Death.Person.HasParentsChanging += value;
			}
			remove
			{
				this.Death.Person.HasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				this.Death.Person.HasParentsChanged += value;
			}
			remove
			{
				this.Death.Person.HasParentsChanged -= value;
			}
		}
		public virtual Nullable<int> OptionalUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
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
		public virtual int MandatoryUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
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
		public virtual Nullable<byte> OptionalUniqueTinyInt
		{
			get
			{
				return this.Death.Person.OptionalUniqueTinyInt;
			}
			set
			{
				this.Death.Person.OptionalUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.OptionalUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.OptionalUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryUniqueTinyInt
		{
			get
			{
				return this.Death.Person.MandatoryUniqueTinyInt;
			}
			set
			{
				this.Death.Person.MandatoryUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.MandatoryUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.MandatoryUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryUniqueTinyIntChanged -= value;
			}
		}
		public virtual Nullable<byte> OptionalNonUniqueTinyInt
		{
			get
			{
				return this.Death.Person.OptionalNonUniqueTinyInt;
			}
			set
			{
				this.Death.Person.OptionalNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.OptionalNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryNonUniqueTinyInt
		{
			get
			{
				return this.Death.Person.MandatoryNonUniqueTinyInt;
			}
			set
			{
				this.Death.Person.MandatoryNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual int MandatoryNonUniqueUnconstrainedDecimal
		{
			get
			{
				return this.Death.Person.MandatoryNonUniqueUnconstrainedDecimal;
			}
			set
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedDecimalChanged -= value;
			}
		}
		public virtual float MandatoryNonUniqueUnconstrainedFloat
		{
			get
			{
				return this.Death.Person.MandatoryNonUniqueUnconstrainedFloat;
			}
			set
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloat = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanging += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanged += value;
			}
			remove
			{
				this.Death.Person.MandatoryNonUniqueUnconstrainedFloatChanged -= value;
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
		public virtual Person HusbandViaWife
		{
			get
			{
				return this.Death.Person.HusbandViaWife;
			}
			set
			{
				this.Death.Person.HusbandViaWife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				this.Death.Person.HusbandViaWifeChanging += value;
			}
			remove
			{
				this.Death.Person.HusbandViaWifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				this.Death.Person.HusbandViaWifeChanged += value;
			}
			remove
			{
				this.Death.Person.HusbandViaWifeChanged -= value;
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Death.Person.PersonDrivesCarViaDrivenByPersonCollection;
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
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Death.Person.PersonHasNickNameViaPersonCollection;
			}
		}
		#endregion // UnnaturalDeath Parent Support (Death)
	}
	#endregion // UnnaturalDeath
	#region MalePerson
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class MalePerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected MalePerson()
		{
		}
		#region MalePerson INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // MalePerson INotifyPropertyChanged Implementation
		#region MalePerson Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[2], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<MalePerson, Person>> PersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					MalePerson.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					MalePerson.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<MalePerson, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<MalePerson, Person>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<MalePerson, Person>>(eventHandler, this, new PropertyChangingEventArgs<MalePerson, Person>(this, "Person", this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<MalePerson, Person>> PersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					MalePerson.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					MalePerson.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<MalePerson, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<MalePerson, Person>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<MalePerson, Person>>(eventHandler, this, new PropertyChangedEventArgs<MalePerson, Person>(this, "Person", oldValue, this.Person), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Person");
			}
		}
		#endregion // MalePerson Property Change Events
		#region MalePerson Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
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
		#endregion // MalePerson Abstract Properties
		#region MalePerson ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "MalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // MalePerson ToString Methods
		#region MalePerson Parent Support (Person)
		public static implicit operator Person(MalePerson malePerson)
		{
			if ((object)malePerson == null)
			{
				return null;
			}
			return malePerson.Person;
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
		public virtual Nullable<int> ColorARGB
		{
			get
			{
				return this.Person.ColorARGB;
			}
			set
			{
				this.Person.ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				this.Person.ColorARGBChanging += value;
			}
			remove
			{
				this.Person.ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				this.Person.ColorARGBChanged += value;
			}
			remove
			{
				this.Person.ColorARGBChanged -= value;
			}
		}
		public virtual string HatTypeStyleDescription
		{
			get
			{
				return this.Person.HatTypeStyleDescription;
			}
			set
			{
				this.Person.HatTypeStyleDescription = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanging += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanged += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanged -= value;
			}
		}
		public virtual Nullable<uint> Vin
		{
			get
			{
				return this.Person.Vin;
			}
			set
			{
				this.Person.Vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				this.Person.VinChanging += value;
			}
			remove
			{
				this.Person.VinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				this.Person.VinChanged += value;
			}
			remove
			{
				this.Person.VinChanged -= value;
			}
		}
		public virtual System.DateTime YMD
		{
			get
			{
				return this.Person.YMD;
			}
			set
			{
				this.Person.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				this.Person.YMDChanging += value;
			}
			remove
			{
				this.Person.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				this.Person.YMDChanged += value;
			}
			remove
			{
				this.Person.YMDChanged -= value;
			}
		}
		public virtual Nullable<bool> IsDead
		{
			get
			{
				return this.Person.IsDead;
			}
			set
			{
				this.Person.IsDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				this.Person.IsDeadChanging += value;
			}
			remove
			{
				this.Person.IsDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				this.Person.IsDeadChanged += value;
			}
			remove
			{
				this.Person.IsDeadChanged -= value;
			}
		}
		public virtual string GenderCode
		{
			get
			{
				return this.Person.GenderCode;
			}
			set
			{
				this.Person.GenderCode = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				this.Person.GenderCodeChanging += value;
			}
			remove
			{
				this.Person.GenderCodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				this.Person.GenderCodeChanged += value;
			}
			remove
			{
				this.Person.GenderCodeChanged -= value;
			}
		}
		public virtual Nullable<bool> HasParents
		{
			get
			{
				return this.Person.HasParents;
			}
			set
			{
				this.Person.HasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				this.Person.HasParentsChanging += value;
			}
			remove
			{
				this.Person.HasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				this.Person.HasParentsChanged += value;
			}
			remove
			{
				this.Person.HasParentsChanged -= value;
			}
		}
		public virtual Nullable<int> OptionalUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
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
		public virtual int MandatoryUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
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
		public virtual Nullable<byte> OptionalUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanged -= value;
			}
		}
		public virtual Nullable<byte> OptionalNonUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalNonUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryNonUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryNonUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual int MandatoryNonUniqueUnconstrainedDecimal
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedDecimal;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged -= value;
			}
		}
		public virtual float MandatoryNonUniqueUnconstrainedFloat
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedFloat;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloat = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged -= value;
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
		public virtual Person HusbandViaWife
		{
			get
			{
				return this.Person.HusbandViaWife;
			}
			set
			{
				this.Person.HusbandViaWife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				this.Person.HusbandViaWifeChanging += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				this.Person.HusbandViaWifeChanged += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanged -= value;
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
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
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
			}
		}
		#endregion // MalePerson Parent Support (Person)
	}
	#endregion // MalePerson
	#region FemalePerson
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class FemalePerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected FemalePerson()
		{
		}
		#region FemalePerson INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // FemalePerson INotifyPropertyChanged Implementation
		#region FemalePerson Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[2], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<FemalePerson, Person>> PersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					FemalePerson.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					FemalePerson.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<FemalePerson, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<FemalePerson, Person>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<FemalePerson, Person>>(eventHandler, this, new PropertyChangingEventArgs<FemalePerson, Person>(this, "Person", this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<FemalePerson, Person>> PersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					FemalePerson.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					FemalePerson.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<FemalePerson, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<FemalePerson, Person>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<FemalePerson, Person>>(eventHandler, this, new PropertyChangedEventArgs<FemalePerson, Person>(this, "Person", oldValue, this.Person), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Person");
			}
		}
		#endregion // FemalePerson Property Change Events
		#region FemalePerson Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
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
		#endregion // FemalePerson Abstract Properties
		#region FemalePerson ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "FemalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // FemalePerson ToString Methods
		#region FemalePerson Parent Support (Person)
		public static implicit operator Person(FemalePerson femalePerson)
		{
			if ((object)femalePerson == null)
			{
				return null;
			}
			return femalePerson.Person;
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
		public virtual Nullable<int> ColorARGB
		{
			get
			{
				return this.Person.ColorARGB;
			}
			set
			{
				this.Person.ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				this.Person.ColorARGBChanging += value;
			}
			remove
			{
				this.Person.ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				this.Person.ColorARGBChanged += value;
			}
			remove
			{
				this.Person.ColorARGBChanged -= value;
			}
		}
		public virtual string HatTypeStyleDescription
		{
			get
			{
				return this.Person.HatTypeStyleDescription;
			}
			set
			{
				this.Person.HatTypeStyleDescription = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanging += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanged += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanged -= value;
			}
		}
		public virtual Nullable<uint> Vin
		{
			get
			{
				return this.Person.Vin;
			}
			set
			{
				this.Person.Vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				this.Person.VinChanging += value;
			}
			remove
			{
				this.Person.VinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				this.Person.VinChanged += value;
			}
			remove
			{
				this.Person.VinChanged -= value;
			}
		}
		public virtual System.DateTime YMD
		{
			get
			{
				return this.Person.YMD;
			}
			set
			{
				this.Person.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				this.Person.YMDChanging += value;
			}
			remove
			{
				this.Person.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				this.Person.YMDChanged += value;
			}
			remove
			{
				this.Person.YMDChanged -= value;
			}
		}
		public virtual Nullable<bool> IsDead
		{
			get
			{
				return this.Person.IsDead;
			}
			set
			{
				this.Person.IsDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				this.Person.IsDeadChanging += value;
			}
			remove
			{
				this.Person.IsDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				this.Person.IsDeadChanged += value;
			}
			remove
			{
				this.Person.IsDeadChanged -= value;
			}
		}
		public virtual string GenderCode
		{
			get
			{
				return this.Person.GenderCode;
			}
			set
			{
				this.Person.GenderCode = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				this.Person.GenderCodeChanging += value;
			}
			remove
			{
				this.Person.GenderCodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				this.Person.GenderCodeChanged += value;
			}
			remove
			{
				this.Person.GenderCodeChanged -= value;
			}
		}
		public virtual Nullable<bool> HasParents
		{
			get
			{
				return this.Person.HasParents;
			}
			set
			{
				this.Person.HasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				this.Person.HasParentsChanging += value;
			}
			remove
			{
				this.Person.HasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				this.Person.HasParentsChanged += value;
			}
			remove
			{
				this.Person.HasParentsChanged -= value;
			}
		}
		public virtual Nullable<int> OptionalUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
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
		public virtual int MandatoryUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
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
		public virtual Nullable<byte> OptionalUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanged -= value;
			}
		}
		public virtual Nullable<byte> OptionalNonUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalNonUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryNonUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryNonUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual int MandatoryNonUniqueUnconstrainedDecimal
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedDecimal;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged -= value;
			}
		}
		public virtual float MandatoryNonUniqueUnconstrainedFloat
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedFloat;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloat = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged -= value;
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
		public virtual Person HusbandViaWife
		{
			get
			{
				return this.Person.HusbandViaWife;
			}
			set
			{
				this.Person.HusbandViaWife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				this.Person.HusbandViaWifeChanging += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				this.Person.HusbandViaWifeChanged += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanged -= value;
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
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
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
			}
		}
		#endregion // FemalePerson Parent Support (Person)
	}
	#endregion // FemalePerson
	#region ChildPerson
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class ChildPerson : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected ChildPerson()
		{
		}
		#region ChildPerson INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // ChildPerson INotifyPropertyChanged Implementation
		#region ChildPerson Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[8], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, uint>> BirthOrderNrChanging
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnBirthOrderNrChanging(uint newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ChildPerson, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ChildPerson, uint>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, uint>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, uint>(this, "BirthOrderNr", this.BirthOrderNr, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, uint>> BirthOrderNrChanged
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnBirthOrderNrChanged(uint oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ChildPerson, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ChildPerson, uint>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, uint>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, uint>(this, "BirthOrderNr", oldValue, this.BirthOrderNr), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BirthOrderNr");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>> FatherChanging
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnFatherChanging(MalePerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, MalePerson>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, MalePerson>(this, "Father", this.Father, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>> FatherChanged
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnFatherChanged(MalePerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ChildPerson, MalePerson>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, MalePerson>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, MalePerson>(this, "Father", oldValue, this.Father), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Father");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>> MotherChanging
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnMotherChanging(FemalePerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, FemalePerson>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, FemalePerson>(this, "Mother", this.Mother, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>> MotherChanged
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnMotherChanged(FemalePerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ChildPerson, FemalePerson>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, FemalePerson>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, FemalePerson>(this, "Mother", oldValue, this.Mother), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Mother");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, Person>> PersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[6], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[6], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ChildPerson, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ChildPerson, Person>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, Person>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, Person>(this, "Person", this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, Person>> PersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					ChildPerson.InterlockedDelegateCombine(ref this.Events[7], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					ChildPerson.InterlockedDelegateRemove(ref events[7], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ChildPerson, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ChildPerson, Person>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, Person>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, Person>(this, "Person", oldValue, this.Person), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Person");
			}
		}
		#endregion // ChildPerson Property Change Events
		#region ChildPerson Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract uint BirthOrderNr
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
		#endregion // ChildPerson Abstract Properties
		#region ChildPerson ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"ChildPerson{0}{{{0}{1}BirthOrderNr = ""{2}"",{0}{1}Father = {3},{0}{1}Mother = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, @"	", this.BirthOrderNr, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // ChildPerson ToString Methods
		#region ChildPerson Parent Support (Person)
		public static implicit operator Person(ChildPerson childPerson)
		{
			if ((object)childPerson == null)
			{
				return null;
			}
			return childPerson.Person;
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
		public virtual Nullable<int> ColorARGB
		{
			get
			{
				return this.Person.ColorARGB;
			}
			set
			{
				this.Person.ColorARGB = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> ColorARGBChanging
		{
			add
			{
				this.Person.ColorARGBChanging += value;
			}
			remove
			{
				this.Person.ColorARGBChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> ColorARGBChanged
		{
			add
			{
				this.Person.ColorARGBChanged += value;
			}
			remove
			{
				this.Person.ColorARGBChanged -= value;
			}
		}
		public virtual string HatTypeStyleDescription
		{
			get
			{
				return this.Person.HatTypeStyleDescription;
			}
			set
			{
				this.Person.HatTypeStyleDescription = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatTypeStyleDescriptionChanging
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanging += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatTypeStyleDescriptionChanged
		{
			add
			{
				this.Person.HatTypeStyleDescriptionChanged += value;
			}
			remove
			{
				this.Person.HatTypeStyleDescriptionChanged -= value;
			}
		}
		public virtual Nullable<uint> Vin
		{
			get
			{
				return this.Person.Vin;
			}
			set
			{
				this.Person.Vin = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<uint>>> VinChanging
		{
			add
			{
				this.Person.VinChanging += value;
			}
			remove
			{
				this.Person.VinChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<uint>>> VinChanged
		{
			add
			{
				this.Person.VinChanged += value;
			}
			remove
			{
				this.Person.VinChanged -= value;
			}
		}
		public virtual System.DateTime YMD
		{
			get
			{
				return this.Person.YMD;
			}
			set
			{
				this.Person.YMD = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, System.DateTime>> YMDChanging
		{
			add
			{
				this.Person.YMDChanging += value;
			}
			remove
			{
				this.Person.YMDChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, System.DateTime>> YMDChanged
		{
			add
			{
				this.Person.YMDChanged += value;
			}
			remove
			{
				this.Person.YMDChanged -= value;
			}
		}
		public virtual Nullable<bool> IsDead
		{
			get
			{
				return this.Person.IsDead;
			}
			set
			{
				this.Person.IsDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> IsDeadChanging
		{
			add
			{
				this.Person.IsDeadChanging += value;
			}
			remove
			{
				this.Person.IsDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> IsDeadChanged
		{
			add
			{
				this.Person.IsDeadChanged += value;
			}
			remove
			{
				this.Person.IsDeadChanged -= value;
			}
		}
		public virtual string GenderCode
		{
			get
			{
				return this.Person.GenderCode;
			}
			set
			{
				this.Person.GenderCode = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> GenderCodeChanging
		{
			add
			{
				this.Person.GenderCodeChanging += value;
			}
			remove
			{
				this.Person.GenderCodeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> GenderCodeChanged
		{
			add
			{
				this.Person.GenderCodeChanged += value;
			}
			remove
			{
				this.Person.GenderCodeChanged -= value;
			}
		}
		public virtual Nullable<bool> HasParents
		{
			get
			{
				return this.Person.HasParents;
			}
			set
			{
				this.Person.HasParents = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> HasParentsChanging
		{
			add
			{
				this.Person.HasParentsChanging += value;
			}
			remove
			{
				this.Person.HasParentsChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> HasParentsChanged
		{
			add
			{
				this.Person.HasParentsChanged += value;
			}
			remove
			{
				this.Person.HasParentsChanged -= value;
			}
		}
		public virtual Nullable<int> OptionalUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OptionalUniqueDecimalChanged
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
		public virtual int MandatoryUniqueDecimal
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryUniqueDecimalChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryUniqueDecimalChanged
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
		public virtual Nullable<byte> OptionalUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryUniqueTinyIntChanged -= value;
			}
		}
		public virtual Nullable<byte> OptionalNonUniqueTinyInt
		{
			get
			{
				return this.Person.OptionalNonUniqueTinyInt;
			}
			set
			{
				this.Person.OptionalNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<byte>>> OptionalNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.OptionalNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.OptionalNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual byte MandatoryNonUniqueTinyInt
		{
			get
			{
				return this.Person.MandatoryNonUniqueTinyInt;
			}
			set
			{
				this.Person.MandatoryNonUniqueTinyInt = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, byte>> MandatoryNonUniqueTinyIntChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueTinyIntChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueTinyIntChanged -= value;
			}
		}
		public virtual int MandatoryNonUniqueUnconstrainedDecimal
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedDecimal;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimal = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> MandatoryNonUniqueUnconstrainedDecimalChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedDecimalChanged -= value;
			}
		}
		public virtual float MandatoryNonUniqueUnconstrainedFloat
		{
			get
			{
				return this.Person.MandatoryNonUniqueUnconstrainedFloat;
			}
			set
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloat = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanging
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, float>> MandatoryNonUniqueUnconstrainedFloatChanged
		{
			add
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged += value;
			}
			remove
			{
				this.Person.MandatoryNonUniqueUnconstrainedFloatChanged -= value;
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
		public virtual Person HusbandViaWife
		{
			get
			{
				return this.Person.HusbandViaWife;
			}
			set
			{
				this.Person.HusbandViaWife = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandViaWifeChanging
		{
			add
			{
				this.Person.HusbandViaWifeChanging += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandViaWifeChanged
		{
			add
			{
				this.Person.HusbandViaWifeChanged += value;
			}
			remove
			{
				this.Person.HusbandViaWifeChanged -= value;
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
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
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
			}
		}
		#endregion // ChildPerson Parent Support (Person)
	}
	#endregion // ChildPerson
	#region PersonDrivesCar
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class PersonDrivesCar : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonDrivesCar()
		{
		}
		#region PersonDrivesCar INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // PersonDrivesCar INotifyPropertyChanged Implementation
		#region PersonDrivesCar Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[4], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonDrivesCar, uint>> VinChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonDrivesCar.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonDrivesCar.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnVinChanging(uint newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonDrivesCar, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonDrivesCar, uint>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonDrivesCar, uint>>(eventHandler, this, new PropertyChangingEventArgs<PersonDrivesCar, uint>(this, "Vin", this.Vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonDrivesCar, uint>> VinChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonDrivesCar.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonDrivesCar.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnVinChanged(uint oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonDrivesCar, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonDrivesCar, uint>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonDrivesCar, uint>>(eventHandler, this, new PropertyChangedEventArgs<PersonDrivesCar, uint>(this, "Vin", oldValue, this.Vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>> DrivenByPersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonDrivesCar.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonDrivesCar.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnDrivenByPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonDrivesCar, Person>>(eventHandler, this, new PropertyChangingEventArgs<PersonDrivesCar, Person>(this, "DrivenByPerson", this.DrivenByPerson, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonDrivesCar, Person>> DrivenByPersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonDrivesCar.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonDrivesCar.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnDrivenByPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonDrivesCar, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonDrivesCar, Person>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonDrivesCar, Person>>(eventHandler, this, new PropertyChangedEventArgs<PersonDrivesCar, Person>(this, "DrivenByPerson", oldValue, this.DrivenByPerson), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("DrivenByPerson");
			}
		}
		#endregion // PersonDrivesCar Property Change Events
		#region PersonDrivesCar Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract uint Vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract Person DrivenByPerson
		{
			get;
			set;
		}
		#endregion // PersonDrivesCar Abstract Properties
		#region PersonDrivesCar ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"PersonDrivesCar{0}{{{0}{1}Vin = ""{2}"",{0}{1}DrivenByPerson = {3}{0}}}", Environment.NewLine, @"	", this.Vin, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // PersonDrivesCar ToString Methods
	}
	#endregion // PersonDrivesCar
	#region PersonBoughtCarFromPersonOnDate
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class PersonBoughtCarFromPersonOnDate : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonBoughtCarFromPersonOnDate()
		{
		}
		#region PersonBoughtCarFromPersonOnDate INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDate INotifyPropertyChanged Implementation
		#region PersonBoughtCarFromPersonOnDate Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[8], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, uint>> VinChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnVinChanging(uint newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, uint>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, uint>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, uint>(this, "Vin", this.Vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, uint>> VinChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnVinChanged(uint oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, uint>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, uint>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, uint>(this, "Vin", oldValue, this.Vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>> YMDChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnYMDChanging(System.DateTime newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>(this, "YMD", this.YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>> YMDChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnYMDChanged(System.DateTime oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, System.DateTime>(this, "YMD", oldValue, this.YMD), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> BuyerChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnBuyerChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, "Buyer", this.Buyer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> BuyerChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnBuyerChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, "Buyer", oldValue, this.Buyer), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Buyer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> SellerChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[6], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[6], value);
				}
			}
		}
		protected bool OnSellerChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, "Seller", this.Seller, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> SellerChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateCombine(ref this.Events[7], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonBoughtCarFromPersonOnDate.InterlockedDelegateRemove(ref events[7], value);
				}
			}
		}
		protected void OnSellerChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, Person>(this, "Seller", oldValue, this.Seller), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Seller");
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDate Property Change Events
		#region PersonBoughtCarFromPersonOnDate Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract uint Vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract System.DateTime YMD
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
		#endregion // PersonBoughtCarFromPersonOnDate Abstract Properties
		#region PersonBoughtCarFromPersonOnDate ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"PersonBoughtCarFromPersonOnDate{0}{{{0}{1}Vin = ""{2}"",{0}{1}YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, @"	", this.Vin, this.YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // PersonBoughtCarFromPersonOnDate ToString Methods
	}
	#endregion // PersonBoughtCarFromPersonOnDate
	#region Review
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Review : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected Review()
		{
		}
		#region Review INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // Review INotifyPropertyChanged Implementation
		#region Review Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[6], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, uint>> VinChanging
		{
			add
			{
				if ((object)value != null)
				{
					Review.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Review.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnVinChanging(uint newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Review, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Review, uint>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, uint>>(eventHandler, this, new PropertyChangingEventArgs<Review, uint>(this, "Vin", this.Vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, uint>> VinChanged
		{
			add
			{
				if ((object)value != null)
				{
					Review.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Review.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnVinChanged(uint oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Review, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Review, uint>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, uint>>(eventHandler, this, new PropertyChangedEventArgs<Review, uint>(this, "Vin", oldValue, this.Vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, uint>> IntegerChanging
		{
			add
			{
				if ((object)value != null)
				{
					Review.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Review.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnIntegerChanging(uint newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Review, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Review, uint>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, uint>>(eventHandler, this, new PropertyChangingEventArgs<Review, uint>(this, "Integer", this.Integer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, uint>> IntegerChanged
		{
			add
			{
				if ((object)value != null)
				{
					Review.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Review.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnIntegerChanged(uint oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Review, uint>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Review, uint>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, uint>>(eventHandler, this, new PropertyChangedEventArgs<Review, uint>(this, "Integer", oldValue, this.Integer), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Integer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, string>> NameChanging
		{
			add
			{
				if ((object)value != null)
				{
					Review.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Review.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Review, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Review, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, string>>(eventHandler, this, new PropertyChangingEventArgs<Review, string>(this, "Name", this.Name, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, string>> NameChanged
		{
			add
			{
				if ((object)value != null)
				{
					Review.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Review.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Review, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Review, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, string>>(eventHandler, this, new PropertyChangedEventArgs<Review, string>(this, "Name", oldValue, this.Name), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Name");
			}
		}
		#endregion // Review Property Change Events
		#region Review Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract uint Vin
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract uint Integer
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string Name
		{
			get;
			set;
		}
		#endregion // Review Abstract Properties
		#region Review ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Review{0}{{{0}{1}Vin = ""{2}"",{0}{1}Integer = ""{3}"",{0}{1}Name = ""{4}""{0}}}", Environment.NewLine, @"	", this.Vin, this.Integer, this.Name);
		}
		#endregion // Review ToString Methods
	}
	#endregion // Review
	#region PersonHasNickName
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class PersonHasNickName : INotifyPropertyChanged, IHasSampleModelContext
	{
		protected PersonHasNickName()
		{
		}
		#region PersonHasNickName INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // PersonHasNickName INotifyPropertyChanged Implementation
		#region PersonHasNickName Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[4], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonHasNickName, string>> NickNameChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonHasNickName.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonHasNickName.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnNickNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonHasNickName, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonHasNickName, string>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonHasNickName, string>>(eventHandler, this, new PropertyChangingEventArgs<PersonHasNickName, string>(this, "NickName", this.NickName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonHasNickName, string>> NickNameChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonHasNickName.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonHasNickName.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnNickNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonHasNickName, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonHasNickName, string>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonHasNickName, string>>(eventHandler, this, new PropertyChangedEventArgs<PersonHasNickName, string>(this, "NickName", oldValue, this.NickName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("NickName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>> PersonChanging
		{
			add
			{
				if ((object)value != null)
				{
					PersonHasNickName.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonHasNickName.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonHasNickName, Person>>(eventHandler, this, new PropertyChangingEventArgs<PersonHasNickName, Person>(this, "Person", this.Person, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonHasNickName, Person>> PersonChanged
		{
			add
			{
				if ((object)value != null)
				{
					PersonHasNickName.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					PersonHasNickName.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonHasNickName, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonHasNickName, Person>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonHasNickName, Person>>(eventHandler, this, new PropertyChangedEventArgs<PersonHasNickName, Person>(this, "Person", oldValue, this.Person), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Person");
			}
		}
		#endregion // PersonHasNickName Property Change Events
		#region PersonHasNickName Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract string NickName
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
		#endregion // PersonHasNickName Abstract Properties
		#region PersonHasNickName ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"PersonHasNickName{0}{{{0}{1}NickName = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, @"	", this.NickName, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // PersonHasNickName ToString Methods
	}
	#endregion // PersonHasNickName
	#region IHasSampleModelContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	public interface IHasSampleModelContext
	{
		SampleModelContext Context
		{
			get;
		}
	}
	#endregion // IHasSampleModelContext
	#region ISampleModelContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	public interface ISampleModelContext
	{
		Person GetPersonByPersonId(int personId);
		bool TryGetPersonByPersonId(int personId, out Person person);
		Person GetPersonByFirstNameAndYMD(string firstName, System.DateTime YMD);
		bool TryGetPersonByFirstNameAndYMD(string firstName, System.DateTime YMD, out Person person);
		Person GetPersonByLastNameAndYMD(string lastName, System.DateTime YMD);
		bool TryGetPersonByLastNameAndYMD(string lastName, System.DateTime YMD, out Person person);
		Person GetPersonByOptionalUniqueString(string optionalUniqueString);
		bool TryGetPersonByOptionalUniqueString(string optionalUniqueString, out Person person);
		Person GetPersonByWife(Person wife);
		bool TryGetPersonByWife(Person wife, out Person person);
		Person GetPersonByVin(uint vin);
		bool TryGetPersonByVin(uint vin, out Person person);
		Person GetPersonByOptionalUniqueDecimal(int optionalUniqueDecimal);
		bool TryGetPersonByOptionalUniqueDecimal(int optionalUniqueDecimal, out Person person);
		Person GetPersonByMandatoryUniqueDecimal(int mandatoryUniqueDecimal);
		bool TryGetPersonByMandatoryUniqueDecimal(int mandatoryUniqueDecimal, out Person person);
		Person GetPersonByMandatoryUniqueString(string mandatoryUniqueString);
		bool TryGetPersonByMandatoryUniqueString(string mandatoryUniqueString, out Person person);
		Person GetPersonByOptionalUniqueTinyInt(byte optionalUniqueTinyInt);
		bool TryGetPersonByOptionalUniqueTinyInt(byte optionalUniqueTinyInt, out Person person);
		Person GetPersonByMandatoryUniqueTinyInt(byte mandatoryUniqueTinyInt);
		bool TryGetPersonByMandatoryUniqueTinyInt(byte mandatoryUniqueTinyInt, out Person person);
		Task GetTaskByTaskId(int taskId);
		bool TryGetTaskByTaskId(int taskId, out Task task);
		ValueType1 GetValueType1ByValueType1Value(int valueType1Value);
		bool TryGetValueType1ByValueType1Value(int valueType1Value, out ValueType1 valueType1);
		ChildPerson GetChildPersonByFatherAndBirthOrderNrAndMother(MalePerson father, uint birthOrderNr, FemalePerson mother);
		bool TryGetChildPersonByFatherAndBirthOrderNrAndMother(MalePerson father, uint birthOrderNr, FemalePerson mother, out ChildPerson childPerson);
		PersonDrivesCar GetPersonDrivesCarByVinAndDrivenByPerson(uint vin, Person drivenByPerson);
		bool TryGetPersonDrivesCarByVinAndDrivenByPerson(uint vin, Person drivenByPerson, out PersonDrivesCar personDrivesCar);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByBuyerAndVinAndSeller(Person buyer, uint vin, Person seller);
		bool TryGetPersonBoughtCarFromPersonOnDateByBuyerAndVinAndSeller(Person buyer, uint vin, Person seller, out PersonBoughtCarFromPersonOnDate personBoughtCarFromPersonOnDate);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByVinAndYMDAndBuyer(uint vin, System.DateTime YMD, Person buyer);
		bool TryGetPersonBoughtCarFromPersonOnDateByVinAndYMDAndBuyer(uint vin, System.DateTime YMD, Person buyer, out PersonBoughtCarFromPersonOnDate personBoughtCarFromPersonOnDate);
		PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByYMDAndSellerAndVin(System.DateTime YMD, Person seller, uint vin);
		bool TryGetPersonBoughtCarFromPersonOnDateByYMDAndSellerAndVin(System.DateTime YMD, Person seller, uint vin, out PersonBoughtCarFromPersonOnDate personBoughtCarFromPersonOnDate);
		Review GetReviewByVinAndName(uint vin, string name);
		bool TryGetReviewByVinAndName(uint vin, string name, out Review review);
		PersonHasNickName GetPersonHasNickNameByNickNameAndPerson(string nickName, Person person);
		bool TryGetPersonHasNickNameByNickNameAndPerson(string nickName, Person person, out PersonHasNickName personHasNickName);
		Person CreatePerson(string firstName, string lastName, System.DateTime YMD, string genderCode, int mandatoryUniqueDecimal, string mandatoryUniqueString, byte mandatoryUniqueTinyInt, byte mandatoryNonUniqueTinyInt, int mandatoryNonUniqueUnconstrainedDecimal, float mandatoryNonUniqueUnconstrainedFloat);
		IEnumerable<Person> PersonCollection
		{
			get;
		}
		Task CreateTask(Person person);
		IEnumerable<Task> TaskCollection
		{
			get;
		}
		ValueType1 CreateValueType1(int valueType1Value);
		IEnumerable<ValueType1> ValueType1Collection
		{
			get;
		}
		Death CreateDeath(string deathCauseType, Person person);
		IEnumerable<Death> DeathCollection
		{
			get;
		}
		NaturalDeath CreateNaturalDeath(Death death);
		IEnumerable<NaturalDeath> NaturalDeathCollection
		{
			get;
		}
		UnnaturalDeath CreateUnnaturalDeath(Death death);
		IEnumerable<UnnaturalDeath> UnnaturalDeathCollection
		{
			get;
		}
		MalePerson CreateMalePerson(Person person);
		IEnumerable<MalePerson> MalePersonCollection
		{
			get;
		}
		FemalePerson CreateFemalePerson(Person person);
		IEnumerable<FemalePerson> FemalePersonCollection
		{
			get;
		}
		ChildPerson CreateChildPerson(uint birthOrderNr, MalePerson father, FemalePerson mother, Person person);
		IEnumerable<ChildPerson> ChildPersonCollection
		{
			get;
		}
		PersonDrivesCar CreatePersonDrivesCar(uint vin, Person drivenByPerson);
		IEnumerable<PersonDrivesCar> PersonDrivesCarCollection
		{
			get;
		}
		PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(uint vin, System.DateTime YMD, Person buyer, Person seller);
		IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
		{
			get;
		}
		Review CreateReview(uint vin, uint integer, string name);
		IEnumerable<Review> ReviewCollection
		{
			get;
		}
		PersonHasNickName CreatePersonHasNickName(string nickName, Person person);
		IEnumerable<PersonHasNickName> PersonHasNickNameCollection
		{
			get;
		}
	}
	#endregion // ISampleModelContext
}

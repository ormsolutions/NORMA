using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace SampleModel
{
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
		public event EventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>> DrivesCar_vinChanging
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
		protected bool OnDrivesCar_vinChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonDrivesCar, int>>(eventHandler, this, new PropertyChangingEventArgs<PersonDrivesCar, int>(this, "DrivesCar_vin", this.DrivesCar_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonDrivesCar, int>> DrivesCar_vinChanged
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
		protected void OnDrivesCar_vinChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonDrivesCar, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonDrivesCar, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonDrivesCar, int>>(eventHandler, this, new PropertyChangedEventArgs<PersonDrivesCar, int>(this, "DrivesCar_vin", oldValue, this.DrivesCar_vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("DrivesCar_vin");
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
		public abstract int DrivesCar_vin
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
			return string.Format(provider, @"PersonDrivesCar{0}{{{0}{1}DrivesCar_vin = ""{2}"",{0}{1}DrivenByPerson = {3}{0}}}", Environment.NewLine, @"	", this.DrivesCar_vin, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // PersonDrivesCar ToString Methods
	}
	#endregion // PersonDrivesCar
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
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[38], null) ?? localEvents;
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
		public event EventHandler<PropertyChangingEventArgs<Person, int>> Date_YMDChanging
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
		protected bool OnDate_YMDChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, int>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, int>>(eventHandler, this, new PropertyChangingEventArgs<Person, int>(this, "Date_YMD", this.Date_YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, int>> Date_YMDChanged
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
		protected void OnDate_YMDChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, int>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, int>>(eventHandler, this, new PropertyChangedEventArgs<Person, int>(this, "Date_YMD", oldValue, this.Date_YMD), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Date_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
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
		protected bool OnLastNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[4]) != null)
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
		protected void OnLastNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[5]) != null)
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
		protected bool OnOptionalUniqueStringChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[6]) != null)
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
		protected void OnOptionalUniqueStringChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "OptionalUniqueString", oldValue, this.OptionalUniqueString), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OptionalUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanging
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
		protected bool OnHatType_ColorARGBChanging(Nullable<int> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>)events[8]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<int>>(this, "HatType_ColorARGB", this.HatType_ColorARGB, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> HatType_ColorARGBChanged
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
		protected void OnHatType_ColorARGBChanged(Nullable<int> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>)events[9]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, "HatType_ColorARGB", oldValue, this.HatType_ColorARGB), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("HatType_ColorARGB");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
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
		protected bool OnHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[10]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "HatType_HatTypeStyle_HatTypeStyle_Description", this.HatType_HatTypeStyle_HatTypeStyle_Description, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
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
		protected void OnHatType_HatTypeStyle_HatTypeStyle_DescriptionChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[11]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "HatType_HatTypeStyle_HatTypeStyle_Description", oldValue, this.HatType_HatTypeStyle_HatTypeStyle_Description), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("HatType_HatTypeStyle_HatTypeStyle_Description");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> OwnsCar_vinChanging
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
		protected bool OnOwnsCar_vinChanging(Nullable<int> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>)events[12]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<int>>(this, "OwnsCar_vin", this.OwnsCar_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> OwnsCar_vinChanged
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
		protected void OnOwnsCar_vinChanged(Nullable<int> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<int>>>)events[13]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<int>>(this, "OwnsCar_vin", oldValue, this.OwnsCar_vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OwnsCar_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> Gender_Gender_CodeChanging
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
		protected bool OnGender_Gender_CodeChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[14]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, "Gender_Gender_Code", this.Gender_Gender_Code, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> Gender_Gender_CodeChanged
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
		protected void OnGender_Gender_CodeChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[15]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "Gender_Gender_Code", oldValue, this.Gender_Gender_Code), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Gender_Gender_Code");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		protected bool OnhasParentsChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>)events[16]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<bool>>(this, "hasParents", this.hasParents, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		protected void OnhasParentsChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>>)events[17]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<bool>>(this, "hasParents", oldValue, this.hasParents), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("hasParents");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanging
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
		protected bool OnOptionalUniqueDecimalChanging(Nullable<decimal> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>>)events[18]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Nullable<decimal>>>(eventHandler, this, new PropertyChangingEventArgs<Person, Nullable<decimal>>(this, "OptionalUniqueDecimal", this.OptionalUniqueDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> OptionalUniqueDecimalChanged
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
		protected void OnOptionalUniqueDecimalChanged(Nullable<decimal> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Nullable<decimal>>>)events[19]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Nullable<decimal>>>(eventHandler, this, new PropertyChangedEventArgs<Person, Nullable<decimal>>(this, "OptionalUniqueDecimal", oldValue, this.OptionalUniqueDecimal), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("OptionalUniqueDecimal");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, decimal>> MandatoryUniqueDecimalChanging
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
		protected bool OnMandatoryUniqueDecimalChanging(decimal newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, decimal>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, decimal>>)events[20]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, decimal>>(eventHandler, this, new PropertyChangingEventArgs<Person, decimal>(this, "MandatoryUniqueDecimal", this.MandatoryUniqueDecimal, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, decimal>> MandatoryUniqueDecimalChanged
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
		protected void OnMandatoryUniqueDecimalChanged(decimal oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, decimal>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, decimal>>)events[21]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, decimal>>(eventHandler, this, new PropertyChangedEventArgs<Person, decimal>(this, "MandatoryUniqueDecimal", oldValue, this.MandatoryUniqueDecimal), this._propertyChangedEventHandler);
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
		protected bool OnMandatoryUniqueStringChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, string>>)events[22]) != null)
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
		protected void OnMandatoryUniqueStringChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, string>>)events[23]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, "MandatoryUniqueString", oldValue, this.MandatoryUniqueString), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MandatoryUniqueString");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> HusbandChanging
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
		protected bool OnHusbandChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Person>>)events[24]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Person>>(eventHandler, this, new PropertyChangingEventArgs<Person, Person>(this, "Husband", this.Husband, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Person>> HusbandChanged
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
		protected void OnHusbandChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Person>>)events[25]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Person>>(eventHandler, this, new PropertyChangedEventArgs<Person, Person>(this, "Husband", oldValue, this.Husband), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Husband");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, ValueType1>> ValueType1DoesSomethingElseWithChanging
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
		protected bool OnValueType1DoesSomethingElseWithChanging(ValueType1 newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, ValueType1>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, ValueType1>>)events[26]) != null)
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
		protected void OnValueType1DoesSomethingElseWithChanged(ValueType1 oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, ValueType1>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, ValueType1>>)events[27]) != null)
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
		protected bool OnMalePersonChanging(MalePerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, MalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, MalePerson>>)events[28]) != null)
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
		protected void OnMalePersonChanged(MalePerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, MalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, MalePerson>>)events[29]) != null)
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
		protected bool OnFemalePersonChanging(FemalePerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, FemalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, FemalePerson>>)events[30]) != null)
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
		protected void OnFemalePersonChanged(FemalePerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, FemalePerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, FemalePerson>>)events[31]) != null)
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
		protected bool OnChildPersonChanging(ChildPerson newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, ChildPerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, ChildPerson>>)events[32]) != null)
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
		protected void OnChildPersonChanged(ChildPerson oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, ChildPerson>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, ChildPerson>>)events[33]) != null)
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
		protected bool OnDeathChanging(Death newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Death>>)events[34]) != null)
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
		protected void OnDeathChanged(Death oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Death>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Death>>)events[35]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Death>>(eventHandler, this, new PropertyChangedEventArgs<Person, Death>(this, "Death", oldValue, this.Death), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Death");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Person>> WifeChanging
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
		protected bool OnWifeChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Person, Person>>)events[36]) != null)
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
		protected void OnWifeChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Person, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Person, Person>>)events[37]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Person>>(eventHandler, this, new PropertyChangedEventArgs<Person, Person>(this, "Wife", oldValue, this.Wife), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Wife");
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
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> hasParents
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
		public abstract IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get;
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
		#endregion // Person Abstract Properties
		#region Person ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Date_YMD = ""{3}"",{0}{1}LastName = ""{4}"",{0}{1}OptionalUniqueString = ""{5}"",{0}{1}HatType_ColorARGB = ""{6}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{7}"",{0}{1}OwnsCar_vin = ""{8}"",{0}{1}Gender_Gender_Code = ""{9}"",{0}{1}hasParents = ""{10}"",{0}{1}OptionalUniqueDecimal = ""{11}"",{0}{1}MandatoryUniqueDecimal = ""{12}"",{0}{1}MandatoryUniqueString = ""{13}"",{0}{1}Husband = {14},{0}{1}ValueType1DoesSomethingElseWith = {15},{0}{1}MalePerson = {16},{0}{1}FemalePerson = {17},{0}{1}ChildPerson = {18},{0}{1}Death = {19},{0}{1}Wife = {20}{0}}}", Environment.NewLine, @"	", this.FirstName, this.Date_YMD, this.LastName, this.OptionalUniqueString, this.HatType_ColorARGB, this.HatType_HatTypeStyle_HatTypeStyle_Description, this.OwnsCar_vin, this.Gender_Gender_Code, this.hasParents, this.OptionalUniqueDecimal, this.MandatoryUniqueDecimal, this.MandatoryUniqueString, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // Person ToString Methods
		#region Person Children Support
		#region Person Child Support (MalePerson)
		public static explicit operator MalePerson(Person Person)
		{
			if ((object)Person == null)
			{
				return null;
			}
			MalePerson MalePerson;
			if ((object)(MalePerson = Person.MalePerson) == null)
			{
				throw new InvalidCastException();
			}
			return MalePerson;
		}
		#endregion // Person Child Support (MalePerson)
		#region Person Child Support (FemalePerson)
		public static explicit operator FemalePerson(Person Person)
		{
			if ((object)Person == null)
			{
				return null;
			}
			FemalePerson FemalePerson;
			if ((object)(FemalePerson = Person.FemalePerson) == null)
			{
				throw new InvalidCastException();
			}
			return FemalePerson;
		}
		#endregion // Person Child Support (FemalePerson)
		#region Person Child Support (ChildPerson)
		public static explicit operator ChildPerson(Person Person)
		{
			if ((object)Person == null)
			{
				return null;
			}
			ChildPerson ChildPerson;
			if ((object)(ChildPerson = Person.ChildPerson) == null)
			{
				throw new InvalidCastException();
			}
			return ChildPerson;
		}
		#endregion // Person Child Support (ChildPerson)
		#region Person Child Support (Death)
		public static explicit operator Death(Person Person)
		{
			if ((object)Person == null)
			{
				return null;
			}
			Death Death;
			if ((object)(Death = Person.Death) == null)
			{
				throw new InvalidCastException();
			}
			return Death;
		}
		public static explicit operator NaturalDeath(Person Person)
		{
			if ((object)Person == null)
			{
				return null;
			}
			return (NaturalDeath)(Death)Person;
		}
		public static explicit operator UnnaturalDeath(Person Person)
		{
			if ((object)Person == null)
			{
				return null;
			}
			return (UnnaturalDeath)(Death)Person;
		}
		#endregion // Person Child Support (Death)
		#endregion // Person Children Support
	}
	#endregion // Person
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
		public static implicit operator Person(MalePerson MalePerson)
		{
			if ((object)MalePerson == null)
			{
				return null;
			}
			return MalePerson.Person;
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
		public virtual Nullable<bool> hasParents
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
			}
		}
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
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
		public static implicit operator Person(FemalePerson FemalePerson)
		{
			if ((object)FemalePerson == null)
			{
				return null;
			}
			return FemalePerson.Person;
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
		public virtual Nullable<bool> hasParents
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
			}
		}
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
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
		public event EventHandler<PropertyChangingEventArgs<ChildPerson, int>> BirthOrder_BirthOrder_NrChanging
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
		protected bool OnBirthOrder_BirthOrder_NrChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<ChildPerson, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<ChildPerson, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<ChildPerson, int>>(eventHandler, this, new PropertyChangingEventArgs<ChildPerson, int>(this, "BirthOrder_BirthOrder_Nr", this.BirthOrder_BirthOrder_Nr, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<ChildPerson, int>> BirthOrder_BirthOrder_NrChanged
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
		protected void OnBirthOrder_BirthOrder_NrChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<ChildPerson, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<ChildPerson, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<ChildPerson, int>>(eventHandler, this, new PropertyChangedEventArgs<ChildPerson, int>(this, "BirthOrder_BirthOrder_Nr", oldValue, this.BirthOrder_BirthOrder_Nr), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BirthOrder_BirthOrder_Nr");
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
		#endregion // ChildPerson Abstract Properties
		#region ChildPerson ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"ChildPerson{0}{{{0}{1}BirthOrder_BirthOrder_Nr = ""{2}"",{0}{1}Father = {3},{0}{1}Mother = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, @"	", this.BirthOrder_BirthOrder_Nr, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // ChildPerson ToString Methods
		#region ChildPerson Parent Support (Person)
		public static implicit operator Person(ChildPerson ChildPerson)
		{
			if ((object)ChildPerson == null)
			{
				return null;
			}
			return ChildPerson.Person;
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
		public virtual Nullable<bool> hasParents
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
			}
		}
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
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
		#endregion // ChildPerson Parent Support (Person)
	}
	#endregion // ChildPerson
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
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[12], null) ?? localEvents;
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
		public event EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> Date_YMDChanging
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
		protected bool OnDate_YMDChanging(Nullable<int> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, Nullable<int>>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, Nullable<int>>>(eventHandler, this, new PropertyChangingEventArgs<Death, Nullable<int>>(this, "Date_YMD", this.Date_YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> Date_YMDChanged
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
		protected void OnDate_YMDChanged(Nullable<int> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, Nullable<int>>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, Nullable<int>>>(eventHandler, this, new PropertyChangedEventArgs<Death, Nullable<int>>(this, "Date_YMD", oldValue, this.Date_YMD), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Date_YMD");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, string>> DeathCause_DeathCause_TypeChanging
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
		protected bool OnDeathCause_DeathCause_TypeChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, string>>(eventHandler, this, new PropertyChangingEventArgs<Death, string>(this, "DeathCause_DeathCause_Type", this.DeathCause_DeathCause_Type, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, string>> DeathCause_DeathCause_TypeChanged
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
		protected void OnDeathCause_DeathCause_TypeChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, string>>(eventHandler, this, new PropertyChangedEventArgs<Death, string>(this, "DeathCause_DeathCause_Type", oldValue, this.DeathCause_DeathCause_Type), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("DeathCause_DeathCause_Type");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, bool>> isDeadChanging
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
		protected bool OnisDeadChanging(bool newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, bool>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, bool>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Death, bool>>(eventHandler, this, new PropertyChangingEventArgs<Death, bool>(this, "isDead", this.isDead, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Death, bool>> isDeadChanged
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
		protected void OnisDeadChanged(bool oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, bool>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, bool>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Death, bool>>(eventHandler, this, new PropertyChangedEventArgs<Death, bool>(this, "isDead", oldValue, this.isDead), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("isDead");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> NaturalDeathChanging
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
		protected bool OnNaturalDeathChanging(NaturalDeath newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, NaturalDeath>>)events[6]) != null)
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
		protected void OnNaturalDeathChanged(NaturalDeath oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, NaturalDeath>>)events[7]) != null)
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
		protected bool OnUnnaturalDeathChanging(UnnaturalDeath newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, UnnaturalDeath>>)events[8]) != null)
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
		protected void OnUnnaturalDeathChanged(UnnaturalDeath oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, UnnaturalDeath>>)events[9]) != null)
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
					Death.InterlockedDelegateCombine(ref this.Events[10], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[10], value);
				}
			}
		}
		protected bool OnPersonChanging(Person newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Death, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Death, Person>>)events[10]) != null)
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
					Death.InterlockedDelegateCombine(ref this.Events[11], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					Death.InterlockedDelegateRemove(ref events[11], value);
				}
			}
		}
		protected void OnPersonChanged(Person oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Death, Person>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Death, Person>>)events[11]) != null)
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
		[DataObjectField(false, false, false)]
		public abstract bool isDead
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
			return string.Format(provider, @"Death{0}{{{0}{1}Date_YMD = ""{2}"",{0}{1}DeathCause_DeathCause_Type = ""{3}"",{0}{1}isDead = ""{4}"",{0}{1}NaturalDeath = {5},{0}{1}UnnaturalDeath = {6},{0}{1}Person = {7}{0}}}", Environment.NewLine, @"	", this.Date_YMD, this.DeathCause_DeathCause_Type, this.isDead, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // Death ToString Methods
		#region Death Parent Support (Person)
		public static implicit operator Person(Death Death)
		{
			if ((object)Death == null)
			{
				return null;
			}
			return Death.Person;
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
		public virtual Nullable<bool> hasParents
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Person.PersonDrivesCarViaDrivenByPersonCollection;
			}
		}
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Person.PersonHasNickNameViaPersonCollection;
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
		#endregion // Death Parent Support (Person)
		#region Death Children Support
		#region Death Child Support (NaturalDeath)
		public static explicit operator NaturalDeath(Death Death)
		{
			if ((object)Death == null)
			{
				return null;
			}
			NaturalDeath NaturalDeath;
			if ((object)(NaturalDeath = Death.NaturalDeath) == null)
			{
				throw new InvalidCastException();
			}
			return NaturalDeath;
		}
		#endregion // Death Child Support (NaturalDeath)
		#region Death Child Support (UnnaturalDeath)
		public static explicit operator UnnaturalDeath(Death Death)
		{
			if ((object)Death == null)
			{
				return null;
			}
			UnnaturalDeath UnnaturalDeath;
			if ((object)(UnnaturalDeath = Death.UnnaturalDeath) == null)
			{
				throw new InvalidCastException();
			}
			return UnnaturalDeath;
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
		public event EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>> isFromProstateCancerChanging
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
		protected bool OnisFromProstateCancerChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<NaturalDeath, Nullable<bool>>(this, "isFromProstateCancer", this.isFromProstateCancer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>> isFromProstateCancerChanged
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
		protected void OnisFromProstateCancerChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<NaturalDeath, Nullable<bool>>(this, "isFromProstateCancer", oldValue, this.isFromProstateCancer), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("isFromProstateCancer");
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
		public abstract Nullable<bool> isFromProstateCancer
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
			return string.Format(provider, @"NaturalDeath{0}{{{0}{1}isFromProstateCancer = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, @"	", this.isFromProstateCancer, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // NaturalDeath ToString Methods
		#region NaturalDeath Parent Support (Death)
		public static implicit operator Death(NaturalDeath NaturalDeath)
		{
			if ((object)NaturalDeath == null)
			{
				return null;
			}
			return NaturalDeath.Death;
		}
		public static implicit operator Person(NaturalDeath NaturalDeath)
		{
			if ((object)NaturalDeath == null)
			{
				return null;
			}
			return NaturalDeath.Death.Person;
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
		public virtual bool isDead
		{
			get
			{
				return this.Death.isDead;
			}
			set
			{
				this.Death.isDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, bool>> isDeadChanging
		{
			add
			{
				this.Death.isDeadChanging += value;
			}
			remove
			{
				this.Death.isDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, bool>> isDeadChanged
		{
			add
			{
				this.Death.isDeadChanged += value;
			}
			remove
			{
				this.Death.isDeadChanged -= value;
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
		public virtual Nullable<bool> hasParents
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Death.Person.PersonDrivesCarViaDrivenByPersonCollection;
			}
		}
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Death.Person.PersonHasNickNameViaPersonCollection;
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
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> isViolentChanging
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
		protected bool OnisViolentChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>(this, "isViolent", this.isViolent, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> isViolentChanged
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
		protected void OnisViolentChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>(this, "isViolent", oldValue, this.isViolent), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("isViolent");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> isBloodyChanging
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
		protected bool OnisBloodyChanging(Nullable<bool> newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangingEventArgs<UnnaturalDeath, Nullable<bool>>(this, "isBloody", this.isBloody, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> isBloodyChanged
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
		protected void OnisBloodyChanged(Nullable<bool> oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>>(eventHandler, this, new PropertyChangedEventArgs<UnnaturalDeath, Nullable<bool>>(this, "isBloody", oldValue, this.isBloody), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("isBloody");
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
		public abstract Nullable<bool> isViolent
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract Nullable<bool> isBloody
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
			return string.Format(provider, @"UnnaturalDeath{0}{{{0}{1}isViolent = ""{2}"",{0}{1}isBloody = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, @"	", this.isViolent, this.isBloody, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // UnnaturalDeath ToString Methods
		#region UnnaturalDeath Parent Support (Death)
		public static implicit operator Death(UnnaturalDeath UnnaturalDeath)
		{
			if ((object)UnnaturalDeath == null)
			{
				return null;
			}
			return UnnaturalDeath.Death;
		}
		public static implicit operator Person(UnnaturalDeath UnnaturalDeath)
		{
			if ((object)UnnaturalDeath == null)
			{
				return null;
			}
			return UnnaturalDeath.Death.Person;
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
		public virtual bool isDead
		{
			get
			{
				return this.Death.isDead;
			}
			set
			{
				this.Death.isDead = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Death, bool>> isDeadChanging
		{
			add
			{
				this.Death.isDeadChanging += value;
			}
			remove
			{
				this.Death.isDeadChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<Death, bool>> isDeadChanged
		{
			add
			{
				this.Death.isDeadChanged += value;
			}
			remove
			{
				this.Death.isDeadChanged -= value;
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
		public virtual Nullable<bool> hasParents
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
		public event EventHandler<PropertyChangingEventArgs<Person, Nullable<bool>>> hasParentsChanging
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
		public event EventHandler<PropertyChangedEventArgs<Person, Nullable<bool>>> hasParentsChanged
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
		public virtual IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
		{
			get
			{
				return this.Death.Person.PersonDrivesCarViaDrivenByPersonCollection;
			}
		}
		public virtual IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
		{
			get
			{
				return this.Death.Person.PersonHasNickNameViaPersonCollection;
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
		#endregion // UnnaturalDeath Parent Support (Death)
	}
	#endregion // UnnaturalDeath
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
		[DataObjectField(false, false, true)]
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
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> CarSold_vinChanging
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
		protected bool OnCarSold_vinChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, "CarSold_vin", this.CarSold_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> CarSold_vinChanged
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
		protected void OnCarSold_vinChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, "CarSold_vin", oldValue, this.CarSold_vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("CarSold_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> SaleDate_YMDChanging
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
		protected bool OnSaleDate_YMDChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangingEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, "SaleDate_YMD", this.SaleDate_YMD, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> SaleDate_YMDChanged
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
		protected void OnSaleDate_YMDChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>>(eventHandler, this, new PropertyChangedEventArgs<PersonBoughtCarFromPersonOnDate, int>(this, "SaleDate_YMD", oldValue, this.SaleDate_YMD), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("SaleDate_YMD");
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
		#endregion // PersonBoughtCarFromPersonOnDate Abstract Properties
		#region PersonBoughtCarFromPersonOnDate ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"PersonBoughtCarFromPersonOnDate{0}{{{0}{1}CarSold_vin = ""{2}"",{0}{1}SaleDate_YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, @"	", this.CarSold_vin, this.SaleDate_YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
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
		public event EventHandler<PropertyChangingEventArgs<Review, int>> Car_vinChanging
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
		protected bool OnCar_vinChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Review, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Review, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, int>>(eventHandler, this, new PropertyChangingEventArgs<Review, int>(this, "Car_vin", this.Car_vin, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, int>> Car_vinChanged
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
		protected void OnCar_vinChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Review, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Review, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, int>>(eventHandler, this, new PropertyChangedEventArgs<Review, int>(this, "Car_vin", oldValue, this.Car_vin), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Car_vin");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, int>> Rating_Nr_IntegerChanging
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
		protected bool OnRating_Nr_IntegerChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Review, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Review, int>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, int>>(eventHandler, this, new PropertyChangingEventArgs<Review, int>(this, "Rating_Nr_Integer", this.Rating_Nr_Integer, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, int>> Rating_Nr_IntegerChanged
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
		protected void OnRating_Nr_IntegerChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Review, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Review, int>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, int>>(eventHandler, this, new PropertyChangedEventArgs<Review, int>(this, "Rating_Nr_Integer", oldValue, this.Rating_Nr_Integer), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Rating_Nr_Integer");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Review, string>> Criterion_NameChanging
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
		protected bool OnCriterion_NameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<Review, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<Review, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Review, string>>(eventHandler, this, new PropertyChangingEventArgs<Review, string>(this, "Criterion_Name", this.Criterion_Name, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Review, string>> Criterion_NameChanged
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
		protected void OnCriterion_NameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<Review, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<Review, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Review, string>>(eventHandler, this, new PropertyChangedEventArgs<Review, string>(this, "Criterion_Name", oldValue, this.Criterion_Name), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Criterion_Name");
			}
		}
		#endregion // Review Property Change Events
		#region Review Abstract Properties
		public abstract SampleModelContext Context
		{
			get;
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
		#endregion // Review Abstract Properties
		#region Review ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criterion_Name = ""{4}""{0}}}", Environment.NewLine, @"	", this.Car_vin, this.Rating_Nr_Integer, this.Criterion_Name);
		}
		#endregion // Review ToString Methods
	}
	#endregion // Review
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
		PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson);
		bool TryGetPersonDrivesCarByInternalUniquenessConstraint18(int DrivesCar_vin, Person DrivenByPerson, out PersonDrivesCar PersonDrivesCar);
		PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person);
		bool TryGetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person, out PersonHasNickName PersonHasNickName);
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
		PersonDrivesCar CreatePersonDrivesCar(int DrivesCar_vin, Person DrivenByPerson);
		IEnumerable<PersonDrivesCar> PersonDrivesCarCollection
		{
			get;
		}
		PersonHasNickName CreatePersonHasNickName(string NickName, Person Person);
		IEnumerable<PersonHasNickName> PersonHasNickNameCollection
		{
			get;
		}
		Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, decimal MandatoryUniqueDecimal, string MandatoryUniqueString);
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
		Death CreateDeath(string DeathCause_DeathCause_Type, bool isDead, Person Person);
		IEnumerable<Death> DeathCollection
		{
			get;
		}
		NaturalDeath CreateNaturalDeath(Death Death);
		IEnumerable<NaturalDeath> NaturalDeathCollection
		{
			get;
		}
		UnnaturalDeath CreateUnnaturalDeath(Death Death);
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

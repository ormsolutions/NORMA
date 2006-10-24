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
namespace PersonCountryDemo
{
	#region Person
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Person : INotifyPropertyChanged, IHasPersonCountryDemoContext
	{
		protected Person()
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
		[SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
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
		public abstract PersonCountryDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> LastNameChanging
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
		protected bool OnLastNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person, string>>;
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
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OnLastNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.LastName));
				this.OnPropertyChanged("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> FirstNameChanging
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
		protected bool OnFirstNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person, string>>;
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
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		protected void OnFirstNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.FirstName));
				this.OnPropertyChanged("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> TitleChanging
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
		protected bool OnTitleChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, string>>(eventHandler, this, new PropertyChangingEventArgs<Person, string>(this, this.Title, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> TitleChanged
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
		protected void OnTitleChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, string>>(eventHandler, this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.Title));
				this.OnPropertyChanged("Title");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Country>> CountryChanging
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
		protected bool OnCountryChanging(Country newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Country>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<Person, Country>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Person, Country>>(eventHandler, this, new PropertyChangingEventArgs<Person, Country>(this, this.Country, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Country>> CountryChanged
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
		protected void OnCountryChanged(Country oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Country>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<Person, Country>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Person, Country>>(eventHandler, this, new PropertyChangedEventArgs<Person, Country>(this, oldValue, this.Country));
				this.OnPropertyChanged("Country");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string LastName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string FirstName
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract string Title
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract Country Country
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
			return string.Format(provider, @"Person{0}{{{0}{1}LastName = ""{2}"",{0}{1}FirstName = ""{3}"",{0}{1}Title = ""{4}"",{0}{1}Country = {5}{0}}}", Environment.NewLine, "	", this.LastName, this.FirstName, this.Title, "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // Person
	#region Country
	[DataObjectAttribute()]
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class Country : INotifyPropertyChanged, IHasPersonCountryDemoContext
	{
		protected Country()
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
		[SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
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
		public abstract PersonCountryDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<Country, string>> Country_nameChanging
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
		protected bool OnCountry_nameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Country, string>>(eventHandler, this, new PropertyChangingEventArgs<Country, string>(this, this.Country_name, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Country, string>> Country_nameChanged
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
		protected void OnCountry_nameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Country, string>>(eventHandler, this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Country_name));
				this.OnPropertyChanged("Country_name");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Country, string>> Region_Region_codeChanging
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
		protected bool OnRegion_Region_codeChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<Country, string>>(eventHandler, this, new PropertyChangingEventArgs<Country, string>(this, this.Region_Region_code, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Country, string>> Region_Region_codeChanged
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
		protected void OnRegion_Region_codeChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<Country, string>>(eventHandler, this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Region_Region_code));
				this.OnPropertyChanged("Region_Region_code");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract string Country_name
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract string Region_Region_code
		{
			get;
			set;
		}
		[DataObjectFieldAttribute(false, false, true)]
		public abstract IEnumerable<Person> PersonViaCountryCollection
		{
			get;
		}
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"Country{0}{{{0}{1}Country_name = ""{2}"",{0}{1}Region_Region_code = ""{3}""{0}}}", Environment.NewLine, "	", this.Country_name, this.Region_Region_code);
		}
	}
	#endregion // Country
	#region IHasPersonCountryDemoContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface IHasPersonCountryDemoContext
	{
		PersonCountryDemoContext Context
		{
			get;
		}
	}
	#endregion // IHasPersonCountryDemoContext
	#region IPersonCountryDemoContext
	[GeneratedCodeAttribute("OIALtoPLiX", "1.0")]
	public interface IPersonCountryDemoContext
	{
		Country GetCountryByCountry_name(string Country_name);
		bool TryGetCountryByCountry_name(string Country_name, out Country Country);
		Person CreatePerson(string LastName, string FirstName);
		IEnumerable<Person> PersonCollection
		{
			get;
		}
		Country CreateCountry(string Country_name);
		IEnumerable<Country> CountryCollection
		{
			get;
		}
	}
	#endregion // IPersonCountryDemoContext
}

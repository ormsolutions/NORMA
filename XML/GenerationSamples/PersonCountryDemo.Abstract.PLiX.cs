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
				return this._events ?? (this._events = new System.Delegate[5]);
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
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
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
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		protected bool OnLastNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.LastName, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> LastNameChanged
		{
			add
			{
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		protected void OnLastNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.LastName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.OnPropertyChanged("LastName");
			}
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
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.FirstName, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> FirstNameChanged
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
		protected void OnFirstNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.FirstName), new AsyncCallback(eventHandler.EndInvoke), null);
				this.OnPropertyChanged("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, string>> TitleChanging
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
		protected bool OnTitleChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, string> eventArgs = new PropertyChangingEventArgs<Person, string>(this, this.Title, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, string>> TitleChanged
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
		protected void OnTitleChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, string>(this, oldValue, this.Title), new AsyncCallback(eventHandler.EndInvoke), null);
				this.OnPropertyChanged("Title");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Person, Country>> CountryChanging
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
		protected bool OnCountryChanging(Country newValue)
		{
			EventHandler<PropertyChangingEventArgs<Person, Country>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person, Country>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Person, Country> eventArgs = new PropertyChangingEventArgs<Person, Country>(this, this.Country, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Person, Country>> CountryChanged
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
		protected void OnCountryChanged(Country oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Person, Country>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person, Country>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Person, Country>(this, oldValue, this.Country), new AsyncCallback(eventHandler.EndInvoke), null);
				this.OnPropertyChanged("Country");
			}
		}
		[DataObjectFieldAttribute(false, false, false)]
		public abstract object Person_id
		{
			get;
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
			return string.Format(provider, @"Person{0}{{{0}{1}Person_id = ""{2}"",{0}{1}LastName = ""{3}"",{0}{1}FirstName = ""{4}"",{0}{1}Title = ""{5}"",{0}{1}Country = {6}{0}}}", Environment.NewLine, "	", this.Person_id, this.LastName, this.FirstName, this.Title, "TODO: Recursively call ToString for customTypes...");
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
				return this._events ?? (this._events = new System.Delegate[2]);
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
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
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
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		protected bool OnCountry_nameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Country, string> eventArgs = new PropertyChangingEventArgs<Country, string>(this, this.Country_name, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Country, string>> Country_nameChanged
		{
			add
			{
				this.Events[0] = System.Delegate.Combine(this.Events[0], value);
			}
			remove
			{
				this.Events[0] = System.Delegate.Remove(this.Events[0], value);
			}
		}
		protected void OnCountry_nameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[0] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Country_name), new AsyncCallback(eventHandler.EndInvoke), null);
				this.OnPropertyChanged("Country_name");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<Country, string>> Region_Region_codeChanging
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
		protected bool OnRegion_Region_codeChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				PropertyChangingEventArgs<Country, string> eventArgs = new PropertyChangingEventArgs<Country, string>(this, this.Region_Region_code, newValue);
				eventHandler(this, eventArgs);
				return !(eventArgs.Cancel);
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<Country, string>> Region_Region_codeChanged
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
		protected void OnRegion_Region_codeChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<Country, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Country, string>>;
			if ((object)eventHandler != null)
			{
				eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<Country, string>(this, oldValue, this.Region_Region_code), new AsyncCallback(eventHandler.EndInvoke), null);
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
		Person GetPersonByPerson_id(object Person_id);
		bool TryGetPersonByPerson_id(object Person_id, out Person Person);
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

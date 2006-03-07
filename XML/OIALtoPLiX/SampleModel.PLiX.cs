using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace TestNamespace
{
	namespace SampleModel
	{
		#region PersonDrivesCar
		public abstract partial class PersonDrivesCar : INotifyPropertyChanged
		{
			protected PersonDrivesCar()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> DrivesCar_vinChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseDrivesCar_vinChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.DrivesCar_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> DrivesCar_vinChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseDrivesCar_vinChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.DrivesCar_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DrivesCar_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> DrivenByPersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseDrivenByPersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.DrivenByPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> DrivenByPersonChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseDrivenByPersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.DrivenByPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DrivenByPerson");
				}
			}
			public abstract string DrivesCar_vin
			{
				get;
				set;
			}
			public abstract Person DrivenByPerson
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"PersonDrivesCar{0}{{{0}{1}DrivesCar_vin = ""{2}"",{0}{1}DrivenByPerson = {3}{0}}}", Environment.NewLine, "	", this.DrivesCar_vin, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonDrivesCar
		#region PersonBoughtCarFromPersonOnDate
		public abstract partial class PersonBoughtCarFromPersonOnDate : INotifyPropertyChanged
		{
			protected PersonBoughtCarFromPersonOnDate()
			{
			}
			private readonly Delegate[] Events = new Delegate[5];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> CarSold_vinChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseCarSold_vinChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.CarSold_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> CarSold_vinChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseCarSold_vinChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.CarSold_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("CarSold_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> SaleDate_YMDChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseSaleDate_YMDChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.SaleDate_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> SaleDate_YMDChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseSaleDate_YMDChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.SaleDate_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("SaleDate_YMD");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> BuyerChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected bool RaiseBuyerChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Buyer, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> BuyerChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected void RaiseBuyerChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Buyer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Buyer");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> SellerChanging
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected bool RaiseSellerChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Seller, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> SellerChanged
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected void RaiseSellerChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Seller), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Seller");
				}
			}
			public abstract string CarSold_vin
			{
				get;
				set;
			}
			public abstract string SaleDate_YMD
			{
				get;
				set;
			}
			public abstract Person Buyer
			{
				get;
				set;
			}
			public abstract Person Seller
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"PersonBoughtCarFromPersonOnDate{0}{{{0}{1}CarSold_vin = ""{2}"",{0}{1}SaleDate_YMD = ""{3}"",{0}{1}Buyer = {4},{0}{1}Seller = {5}{0}}}", Environment.NewLine, "	", this.CarSold_vin, this.SaleDate_YMD, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDate
		#region Review
		public abstract partial class Review : INotifyPropertyChanged
		{
			protected Review()
			{
			}
			private readonly Delegate[] Events = new Delegate[4];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Car_vinChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseCar_vinChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Car_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Car_vinChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseCar_vinChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Car_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Car_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Rating_Nr_IntegerChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseRating_Nr_IntegerChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Rating_Nr_Integer, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Rating_Nr_IntegerChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseRating_Nr_IntegerChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Rating_Nr_Integer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Rating_Nr_Integer");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Criteria_NameChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected bool RaiseCriteria_NameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Criteria_Name, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Criteria_NameChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected void RaiseCriteria_NameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Criteria_Name), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Criteria_Name");
				}
			}
			public abstract string Car_vin
			{
				get;
				set;
			}
			public abstract string Rating_Nr_Integer
			{
				get;
				set;
			}
			public abstract string Criteria_Name
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Review{0}{{{0}{1}Car_vin = ""{2}"",{0}{1}Rating_Nr_Integer = ""{3}"",{0}{1}Criteria_Name = ""{4}""{0}}}", Environment.NewLine, "	", this.Car_vin, this.Rating_Nr_Integer, this.Criteria_Name);
			}
		}
		#endregion // Review
		#region PersonHasNickName
		public abstract partial class PersonHasNickName : INotifyPropertyChanged
		{
			protected PersonHasNickName()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> NickNameChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseNickNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.NickName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> NickNameChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseNickNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.NickName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NickName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract string NickName
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"PersonHasNickName{0}{{{0}{1}NickName = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.NickName, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // PersonHasNickName
		#region Person
		public abstract partial class Person : INotifyPropertyChanged
		{
			protected Person()
			{
			}
			private readonly Delegate[] Events = new Delegate[16];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> FirstNameChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseFirstNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.FirstName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> FirstNameChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseFirstNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.FirstName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("FirstName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Person_idChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaisePerson_idChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Person_id, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Person_idChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaisePerson_idChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person_id), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person_id");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Date_YMDChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected bool RaiseDate_YMDChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Date_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Date_YMDChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected void RaiseDate_YMDChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Date_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date_YMD");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> LastNameChanging
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected bool RaiseLastNameChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.LastName, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> LastNameChanged
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected void RaiseLastNameChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.LastName), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("LastName");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> SocialSecurityNumberChanging
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			protected bool RaiseSocialSecurityNumberChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.SocialSecurityNumber, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> SocialSecurityNumberChanged
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			protected void RaiseSocialSecurityNumberChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.SocialSecurityNumber), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("SocialSecurityNumber");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> HatType_ColorARGBChanging
			{
				add
				{
					this.Events[6] = Delegate.Combine(this.Events[6], value);
				}
				remove
				{
					this.Events[6] = Delegate.Remove(this.Events[6], value);
				}
			}
			protected bool RaiseHatType_ColorARGBChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[6] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.HatType_ColorARGB, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> HatType_ColorARGBChanged
			{
				add
				{
					this.Events[6] = Delegate.Combine(this.Events[6], value);
				}
				remove
				{
					this.Events[6] = Delegate.Remove(this.Events[6], value);
				}
			}
			protected void RaiseHatType_ColorARGBChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.HatType_ColorARGB), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatType_ColorARGB");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanging
			{
				add
				{
					this.Events[7] = Delegate.Combine(this.Events[7], value);
				}
				remove
				{
					this.Events[7] = Delegate.Remove(this.Events[7], value);
				}
			}
			protected bool RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.HatType_HatTypeStyle_HatTypeStyle_Description, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> HatType_HatTypeStyle_HatTypeStyle_DescriptionChanged
			{
				add
				{
					this.Events[7] = Delegate.Combine(this.Events[7], value);
				}
				remove
				{
					this.Events[7] = Delegate.Remove(this.Events[7], value);
				}
			}
			protected void RaiseHatType_HatTypeStyle_HatTypeStyle_DescriptionChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[7] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.HatType_HatTypeStyle_HatTypeStyle_Description), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("HatType_HatTypeStyle_HatTypeStyle_Description");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> OwnsCar_vinChanging
			{
				add
				{
					this.Events[8] = Delegate.Combine(this.Events[8], value);
				}
				remove
				{
					this.Events[8] = Delegate.Remove(this.Events[8], value);
				}
			}
			protected bool RaiseOwnsCar_vinChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[8] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.OwnsCar_vin, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> OwnsCar_vinChanged
			{
				add
				{
					this.Events[8] = Delegate.Combine(this.Events[8], value);
				}
				remove
				{
					this.Events[8] = Delegate.Remove(this.Events[8], value);
				}
			}
			protected void RaiseOwnsCar_vinChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.OwnsCar_vin), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("OwnsCar_vin");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Gender_Gender_CodeChanging
			{
				add
				{
					this.Events[9] = Delegate.Combine(this.Events[9], value);
				}
				remove
				{
					this.Events[9] = Delegate.Remove(this.Events[9], value);
				}
			}
			protected bool RaiseGender_Gender_CodeChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Gender_Gender_Code, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Gender_Gender_CodeChanged
			{
				add
				{
					this.Events[9] = Delegate.Combine(this.Events[9], value);
				}
				remove
				{
					this.Events[9] = Delegate.Remove(this.Events[9], value);
				}
			}
			protected void RaiseGender_Gender_CodeChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[9] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Gender_Gender_Code), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Gender_Gender_Code");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> PersonHasParentsChanging
			{
				add
				{
					this.Events[10] = Delegate.Combine(this.Events[10], value);
				}
				remove
				{
					this.Events[10] = Delegate.Remove(this.Events[10], value);
				}
			}
			protected bool RaisePersonHasParentsChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[10] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.PersonHasParents, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> PersonHasParentsChanged
			{
				add
				{
					this.Events[10] = Delegate.Combine(this.Events[10], value);
				}
				remove
				{
					this.Events[10] = Delegate.Remove(this.Events[10], value);
				}
			}
			protected void RaisePersonHasParentsChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.PersonHasParents), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("PersonHasParents");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<ValueType1>> ValueType1Changing
			{
				add
				{
					this.Events[11] = Delegate.Combine(this.Events[11], value);
				}
				remove
				{
					this.Events[11] = Delegate.Remove(this.Events[11], value);
				}
			}
			protected bool RaiseValueType1ChangingEvent(ValueType1 newValue)
			{
				EventHandler<PropertyChangingEventArgs<ValueType1>> eventHandler = this.Events[11] as EventHandler<PropertyChangingEventArgs<ValueType1>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<ValueType1> eventArgs = new PropertyChangingEventArgs<void>(this.ValueType1, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<ValueType1>> ValueType1Changed
			{
				add
				{
					this.Events[11] = Delegate.Combine(this.Events[11], value);
				}
				remove
				{
					this.Events[11] = Delegate.Remove(this.Events[11], value);
				}
			}
			protected void RaiseValueType1ChangedEvent(ValueType1 oldValue)
			{
				EventHandler<PropertyChangedEventArgs<ValueType1>> eventHandler = this.Events[11] as EventHandler<PropertyChangedEventArgs<ValueType1>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.ValueType1), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<MalePerson>> MalePersonChanging
			{
				add
				{
					this.Events[12] = Delegate.Combine(this.Events[12], value);
				}
				remove
				{
					this.Events[12] = Delegate.Remove(this.Events[12], value);
				}
			}
			protected bool RaiseMalePersonChangingEvent(MalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<MalePerson>> eventHandler = this.Events[12] as EventHandler<PropertyChangingEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<MalePerson> eventArgs = new PropertyChangingEventArgs<void>(this.MalePerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<MalePerson>> MalePersonChanged
			{
				add
				{
					this.Events[12] = Delegate.Combine(this.Events[12], value);
				}
				remove
				{
					this.Events[12] = Delegate.Remove(this.Events[12], value);
				}
			}
			protected void RaiseMalePersonChangedEvent(MalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<MalePerson>> eventHandler = this.Events[12] as EventHandler<PropertyChangedEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.MalePerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("MalePerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<FemalePerson>> FemalePersonChanging
			{
				add
				{
					this.Events[13] = Delegate.Combine(this.Events[13], value);
				}
				remove
				{
					this.Events[13] = Delegate.Remove(this.Events[13], value);
				}
			}
			protected bool RaiseFemalePersonChangingEvent(FemalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<FemalePerson>> eventHandler = this.Events[13] as EventHandler<PropertyChangingEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<FemalePerson> eventArgs = new PropertyChangingEventArgs<void>(this.FemalePerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<FemalePerson>> FemalePersonChanged
			{
				add
				{
					this.Events[13] = Delegate.Combine(this.Events[13], value);
				}
				remove
				{
					this.Events[13] = Delegate.Remove(this.Events[13], value);
				}
			}
			protected void RaiseFemalePersonChangedEvent(FemalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<FemalePerson>> eventHandler = this.Events[13] as EventHandler<PropertyChangedEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.FemalePerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("FemalePerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<ChildPerson>> ChildPersonChanging
			{
				add
				{
					this.Events[14] = Delegate.Combine(this.Events[14], value);
				}
				remove
				{
					this.Events[14] = Delegate.Remove(this.Events[14], value);
				}
			}
			protected bool RaiseChildPersonChangingEvent(ChildPerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<ChildPerson>> eventHandler = this.Events[14] as EventHandler<PropertyChangingEventArgs<ChildPerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<ChildPerson> eventArgs = new PropertyChangingEventArgs<void>(this.ChildPerson, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<ChildPerson>> ChildPersonChanged
			{
				add
				{
					this.Events[14] = Delegate.Combine(this.Events[14], value);
				}
				remove
				{
					this.Events[14] = Delegate.Remove(this.Events[14], value);
				}
			}
			protected void RaiseChildPersonChangedEvent(ChildPerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<ChildPerson>> eventHandler = this.Events[14] as EventHandler<PropertyChangedEventArgs<ChildPerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.ChildPerson), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ChildPerson");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
			{
				add
				{
					this.Events[15] = Delegate.Combine(this.Events[15], value);
				}
				remove
				{
					this.Events[15] = Delegate.Remove(this.Events[15], value);
				}
			}
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[15] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<void>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
			{
				add
				{
					this.Events[15] = Delegate.Combine(this.Events[15], value);
				}
				remove
				{
					this.Events[15] = Delegate.Remove(this.Events[15], value);
				}
			}
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[15] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public abstract string FirstName
			{
				get;
				set;
			}
			public abstract string Person_id
			{
				get;
				set;
			}
			public abstract string Date_YMD
			{
				get;
				set;
			}
			public abstract string LastName
			{
				get;
				set;
			}
			public abstract string SocialSecurityNumber
			{
				get;
				set;
			}
			public abstract string HatType_ColorARGB
			{
				get;
				set;
			}
			public abstract string HatType_HatTypeStyle_HatTypeStyle_Description
			{
				get;
				set;
			}
			public abstract string OwnsCar_vin
			{
				get;
				set;
			}
			public abstract string Gender_Gender_Code
			{
				get;
				set;
			}
			public abstract string PersonHasParents
			{
				get;
				set;
			}
			public abstract ValueType1 ValueType1
			{
				get;
				set;
			}
			public abstract MalePerson MalePerson
			{
				get;
				set;
			}
			public abstract FemalePerson FemalePerson
			{
				get;
				set;
			}
			public abstract ChildPerson ChildPerson
			{
				get;
				set;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public abstract PersonDrivesCar PersonDrivesCar
			{
				get;
			}
			public abstract PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate
			{
				get;
			}
			public abstract PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate
			{
				get;
			}
			public abstract PersonHasNickName PersonHasNickName
			{
				get;
			}
			public abstract Task Task
			{
				get;
			}
			public abstract ValueType1 ValueType1
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Person{0}{{{0}{1}FirstName = ""{2}"",{0}{1}Person_id = ""{3}"",{0}{1}Date_YMD = ""{4}"",{0}{1}LastName = ""{5}"",{0}{1}SocialSecurityNumber = ""{6}"",{0}{1}HatType_ColorARGB = ""{7}"",{0}{1}HatType_HatTypeStyle_HatTypeStyle_Description = ""{8}"",{0}{1}OwnsCar_vin = ""{9}"",{0}{1}Gender_Gender_Code = ""{10}"",{0}{1}PersonHasParents = ""{11}"",{0}{1}ValueType1 = {12},{0}{1}MalePerson = {13},{0}{1}FemalePerson = {14},{0}{1}ChildPerson = {15},{0}{1}Death = {16}{0}}}", Environment.NewLine, "	", this.FirstName, this.Person_id, this.Date_YMD, this.LastName, this.SocialSecurityNumber, this.HatType_ColorARGB, this.HatType_HatTypeStyle_HatTypeStyle_Description, this.OwnsCar_vin, this.Gender_Gender_Code, this.PersonHasParents, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Person
		#region MalePerson
		public abstract partial class MalePerson : INotifyPropertyChanged
		{
			protected MalePerson()
			{
			}
			private readonly Delegate[] Events = new Delegate[2];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ChildPerson ChildPerson
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "MalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // MalePerson
		#region FemalePerson
		public abstract partial class FemalePerson : INotifyPropertyChanged
		{
			protected FemalePerson()
			{
			}
			private readonly Delegate[] Events = new Delegate[2];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract ChildPerson ChildPerson
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, "FemalePerson{0}{{{0}{1}Person = {2}{0}}}", Environment.NewLine, "	", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // FemalePerson
		#region ChildPerson
		public abstract partial class ChildPerson : INotifyPropertyChanged
		{
			protected ChildPerson()
			{
			}
			private readonly Delegate[] Events = new Delegate[5];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> BirthOrder_BirthOrder_NrChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseBirthOrder_BirthOrder_NrChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.BirthOrder_BirthOrder_Nr, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> BirthOrder_BirthOrder_NrChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseBirthOrder_BirthOrder_NrChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.BirthOrder_BirthOrder_Nr), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("BirthOrder_BirthOrder_Nr");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<MalePerson>> FatherChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseFatherChangingEvent(MalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<MalePerson>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<MalePerson> eventArgs = new PropertyChangingEventArgs<void>(this.Father, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<MalePerson>> FatherChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseFatherChangedEvent(MalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<MalePerson>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<MalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Father), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Father");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<FemalePerson>> MotherChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected bool RaiseMotherChangingEvent(FemalePerson newValue)
			{
				EventHandler<PropertyChangingEventArgs<FemalePerson>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<FemalePerson> eventArgs = new PropertyChangingEventArgs<void>(this.Mother, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<FemalePerson>> MotherChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected void RaiseMotherChangedEvent(FemalePerson oldValue)
			{
				EventHandler<PropertyChangedEventArgs<FemalePerson>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<FemalePerson>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Mother), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Mother");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract string BirthOrder_BirthOrder_Nr
			{
				get;
				set;
			}
			public abstract MalePerson Father
			{
				get;
				set;
			}
			public abstract FemalePerson Mother
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"ChildPerson{0}{{{0}{1}BirthOrder_BirthOrder_Nr = ""{2}"",{0}{1}Father = {3},{0}{1}Mother = {4},{0}{1}Person = {5}{0}}}", Environment.NewLine, "	", this.BirthOrder_BirthOrder_Nr, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // ChildPerson
		#region Death
		public abstract partial class Death : INotifyPropertyChanged
		{
			protected Death()
			{
			}
			private readonly Delegate[] Events = new Delegate[6];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Date_YMDChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseDate_YMDChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Date_YMD, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Date_YMDChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseDate_YMDChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Date_YMD), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Date_YMD");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> DeathCause_DeathCause_TypeChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseDeathCause_DeathCause_TypeChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.DeathCause_DeathCause_Type, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> DeathCause_DeathCause_TypeChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseDeathCause_DeathCause_TypeChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.DeathCause_DeathCause_Type), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("DeathCause_DeathCause_Type");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<NaturalDeath>> NaturalDeathChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected bool RaiseNaturalDeathChangingEvent(NaturalDeath newValue)
			{
				EventHandler<PropertyChangingEventArgs<NaturalDeath>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<NaturalDeath>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<NaturalDeath> eventArgs = new PropertyChangingEventArgs<void>(this.NaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<NaturalDeath>> NaturalDeathChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected void RaiseNaturalDeathChangedEvent(NaturalDeath oldValue)
			{
				EventHandler<PropertyChangedEventArgs<NaturalDeath>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<NaturalDeath>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.NaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<UnnaturalDeath>> UnnaturalDeathChanging
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected bool RaiseUnnaturalDeathChangingEvent(UnnaturalDeath newValue)
			{
				EventHandler<PropertyChangingEventArgs<UnnaturalDeath>> eventHandler = this.Events[4] as EventHandler<PropertyChangingEventArgs<UnnaturalDeath>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<UnnaturalDeath> eventArgs = new PropertyChangingEventArgs<void>(this.UnnaturalDeath, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<UnnaturalDeath>> UnnaturalDeathChanged
			{
				add
				{
					this.Events[4] = Delegate.Combine(this.Events[4], value);
				}
				remove
				{
					this.Events[4] = Delegate.Remove(this.Events[4], value);
				}
			}
			protected void RaiseUnnaturalDeathChangedEvent(UnnaturalDeath oldValue)
			{
				EventHandler<PropertyChangedEventArgs<UnnaturalDeath>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<UnnaturalDeath>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.UnnaturalDeath), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeath");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[5] = Delegate.Combine(this.Events[5], value);
				}
				remove
				{
					this.Events[5] = Delegate.Remove(this.Events[5], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[5] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract string Date_YMD
			{
				get;
				set;
			}
			public abstract string DeathCause_DeathCause_Type
			{
				get;
				set;
			}
			public abstract NaturalDeath NaturalDeath
			{
				get;
				set;
			}
			public abstract UnnaturalDeath UnnaturalDeath
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Death{0}{{{0}{1}Date_YMD = ""{2}"",{0}{1}DeathCause_DeathCause_Type = ""{3}"",{0}{1}NaturalDeath = {4},{0}{1}UnnaturalDeath = {5},{0}{1}Person = {6}{0}}}", Environment.NewLine, "	", this.Date_YMD, this.DeathCause_DeathCause_Type, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Death
		#region NaturalDeath
		public abstract partial class NaturalDeath : INotifyPropertyChanged
		{
			protected NaturalDeath()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> NaturalDeathIsFromProstateCancerChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseNaturalDeathIsFromProstateCancerChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.NaturalDeathIsFromProstateCancer, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> NaturalDeathIsFromProstateCancerChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseNaturalDeathIsFromProstateCancerChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.NaturalDeathIsFromProstateCancer), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("NaturalDeathIsFromProstateCancer");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<void>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public abstract string NaturalDeathIsFromProstateCancer
			{
				get;
				set;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"NaturalDeath{0}{{{0}{1}NaturalDeathIsFromProstateCancer = ""{2}"",{0}{1}Death = {3}{0}}}", Environment.NewLine, "	", this.NaturalDeathIsFromProstateCancer, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // NaturalDeath
		#region UnnaturalDeath
		public abstract partial class UnnaturalDeath : INotifyPropertyChanged
		{
			protected UnnaturalDeath()
			{
			}
			private readonly Delegate[] Events = new Delegate[4];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> UnnaturalDeathIsViolentChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseUnnaturalDeathIsViolentChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.UnnaturalDeathIsViolent, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> UnnaturalDeathIsViolentChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseUnnaturalDeathIsViolentChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.UnnaturalDeathIsViolent), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeathIsViolent");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<string>> UnnaturalDeathIsBloodyChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaiseUnnaturalDeathIsBloodyChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.UnnaturalDeathIsBloody, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> UnnaturalDeathIsBloodyChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaiseUnnaturalDeathIsBloodyChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.UnnaturalDeathIsBloody), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("UnnaturalDeathIsBloody");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Death>> DeathChanging
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected bool RaiseDeathChangingEvent(Death newValue)
			{
				EventHandler<PropertyChangingEventArgs<Death>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<Death>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Death> eventArgs = new PropertyChangingEventArgs<void>(this.Death, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Death>> DeathChanged
			{
				add
				{
					this.Events[3] = Delegate.Combine(this.Events[3], value);
				}
				remove
				{
					this.Events[3] = Delegate.Remove(this.Events[3], value);
				}
			}
			protected void RaiseDeathChangedEvent(Death oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Death>> eventHandler = this.Events[3] as EventHandler<PropertyChangedEventArgs<Death>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Death), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Death");
				}
			}
			public abstract string UnnaturalDeathIsViolent
			{
				get;
				set;
			}
			public abstract string UnnaturalDeathIsBloody
			{
				get;
				set;
			}
			public abstract Death Death
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"UnnaturalDeath{0}{{{0}{1}UnnaturalDeathIsViolent = ""{2}"",{0}{1}UnnaturalDeathIsBloody = ""{3}"",{0}{1}Death = {4}{0}}}", Environment.NewLine, "	", this.UnnaturalDeathIsViolent, this.UnnaturalDeathIsBloody, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // UnnaturalDeath
		#region Task
		public abstract partial class Task : INotifyPropertyChanged
		{
			protected Task()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> Task_idChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseTask_idChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.Task_id, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> Task_idChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseTask_idChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Task_id), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Task_id");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract string Task_id
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"Task{0}{{{0}{1}Task_id = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.Task_id, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // Task
		#region ValueType1
		public abstract partial class ValueType1 : INotifyPropertyChanged
		{
			protected ValueType1()
			{
			}
			private readonly Delegate[] Events = new Delegate[3];
			private event PropertyChangedEventHandler PropertyChanged
			{
				add
				{
					this.Events[0] = Delegate.Combine(this.Events[0], value);
				}
				remove
				{
					this.Events[0] = Delegate.Remove(this.Events[0], value);
				}
			}
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add
				{
					this.PropertyChanged += value;
				}
				remove
				{
					this.PropertyChanged -= value;
				}
			}
			private void RaisePropertyChangedEvent(string propertyName)
			{
				PropertyChangedEventHandler eventHandler = this.Events[0] as PropertyChangedEventHandler;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs(propertyName), new AsyncCallback(eventHandler.EndInvoke), null);
				}
			}
			public abstract SampleModelContext Context
			{
				get;
			}
			public event EventHandler<PropertyChangingEventArgs<string>> ValueType1ValueChanging
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected bool RaiseValueType1ValueChangingEvent(string newValue)
			{
				EventHandler<PropertyChangingEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<string>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<string> eventArgs = new PropertyChangingEventArgs<void>(this.ValueType1Value, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<string>> ValueType1ValueChanged
			{
				add
				{
					this.Events[1] = Delegate.Combine(this.Events[1], value);
				}
				remove
				{
					this.Events[1] = Delegate.Remove(this.Events[1], value);
				}
			}
			protected void RaiseValueType1ValueChangedEvent(string oldValue)
			{
				EventHandler<PropertyChangedEventArgs<string>> eventHandler = this.Events[1] as EventHandler<PropertyChangedEventArgs<string>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.ValueType1Value), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("ValueType1Value");
				}
			}
			public event EventHandler<PropertyChangingEventArgs<Person>> PersonChanging
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected bool RaisePersonChangingEvent(Person newValue)
			{
				EventHandler<PropertyChangingEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangingEventArgs<Person>>;
				if (eventHandler != null)
				{
					PropertyChangingEventArgs<Person> eventArgs = new PropertyChangingEventArgs<void>(this.Person, newValue);
					eventHandler(this, eventArgs);
					return !(eventArgs.Cancel);
				}
				return true;
			}
			public event EventHandler<PropertyChangedEventArgs<Person>> PersonChanged
			{
				add
				{
					this.Events[2] = Delegate.Combine(this.Events[2], value);
				}
				remove
				{
					this.Events[2] = Delegate.Remove(this.Events[2], value);
				}
			}
			protected void RaisePersonChangedEvent(Person oldValue)
			{
				EventHandler<PropertyChangedEventArgs<Person>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<Person>>;
				if (eventHandler != null)
				{
					eventHandler.BeginInvoke(this, new PropertyChangedEventArgs<void>(oldValue, this.Person), new System.AsyncCallback(eventHandler.EndInvoke), null);
					this.RaisePropertyChangedEvent("Person");
				}
			}
			public abstract string ValueType1Value
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
				set;
			}
			public abstract Person Person
			{
				get;
			}
			public override sealed string ToString()
			{
				return this.ToString(null);
			}
			public string ToString(IFormatProvider provider)
			{
				return string.Format(provider, @"ValueType1{0}{{{0}{1}ValueType1Value = ""{2}"",{0}{1}Person = {3}{0}}}", Environment.NewLine, "	", this.ValueType1Value, "TODO: Recursively call ToString for customTypes...");
			}
		}
		#endregion // ValueType1
		#region ISampleModelContext
		public interface ISampleModelContext
		{
			bool IsDeserializing
			{
				get;
			}
			PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(string DrivesCar_vin, Person DrivenByPerson);
			PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, string CarSold_vin, Person Seller);
			PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(string SaleDate_YMD, Person Seller, string CarSold_vin);
			PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(string CarSold_vin, string SaleDate_YMD, Person Buyer);
			Review GetReviewByInternalUniquenessConstraint26(string Car_vin, string Criteria_Name);
			PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person);
			ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, string BirthOrder_BirthOrder_Nr, FemalePerson Mother);
			Person GetPersonByExternalUniquenessConstraint1(string FirstName, string Date_YMD);
			Person GetPersonByExternalUniquenessConstraint2(string LastName, string Date_YMD);
			Person GetPersonByPerson_id(string Person_id);
			Person GetPersonBySocialSecurityNumber(string SocialSecurityNumber);
			Person GetPersonByOwnsCar_vin(string OwnsCar_vin);
			Task GetTaskByTask_id(string Task_id);
			ValueType1 GetValueType1ByValueType1Value(string ValueType1Value);
			PersonDrivesCar CreatePersonDrivesCar(string DrivesCar_vin, Person DrivenByPerson);
			ReadOnlyCollection<PersonDrivesCar> PersonDrivesCarCollection
			{
				get;
			}
			PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(string CarSold_vin, string SaleDate_YMD, Person Buyer, Person Seller);
			ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
			{
				get;
			}
			Review CreateReview(string Car_vin, string Rating_Nr_Integer, string Criteria_Name);
			ReadOnlyCollection<Review> ReviewCollection
			{
				get;
			}
			PersonHasNickName CreatePersonHasNickName(string NickName, Person Person);
			ReadOnlyCollection<PersonHasNickName> PersonHasNickNameCollection
			{
				get;
			}
			Person CreatePerson(string FirstName, string Person_id, string Date_YMD, string LastName, string Gender_Gender_Code);
			ReadOnlyCollection<Person> PersonCollection
			{
				get;
			}
			MalePerson CreateMalePerson(Person Person);
			ReadOnlyCollection<MalePerson> MalePersonCollection
			{
				get;
			}
			FemalePerson CreateFemalePerson(Person Person);
			ReadOnlyCollection<FemalePerson> FemalePersonCollection
			{
				get;
			}
			ChildPerson CreateChildPerson(string BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person);
			ReadOnlyCollection<ChildPerson> ChildPersonCollection
			{
				get;
			}
			Death CreateDeath(string DeathCause_DeathCause_Type, Person Person);
			ReadOnlyCollection<Death> DeathCollection
			{
				get;
			}
			NaturalDeath CreateNaturalDeath(Death Death);
			ReadOnlyCollection<NaturalDeath> NaturalDeathCollection
			{
				get;
			}
			UnnaturalDeath CreateUnnaturalDeath(Death Death);
			ReadOnlyCollection<UnnaturalDeath> UnnaturalDeathCollection
			{
				get;
			}
			Task CreateTask(string Task_id);
			ReadOnlyCollection<Task> TaskCollection
			{
				get;
			}
			ValueType1 CreateValueType1(string ValueType1Value);
			ReadOnlyCollection<ValueType1> ValueType1Collection
			{
				get;
			}
		}
		#endregion // ISampleModelContext
		#region SampleModelContext
		public sealed class SampleModelContext : ISampleModelContext
		{
			public SampleModelContext()
			{
			}
			private bool _IsDeserializing;
			public bool IsDeserializing
			{
				get
				{
					return this._IsDeserializing;
				}
			}
			private Dictionary<Tuple<string, Person>, PersonDrivesCar> _InternalUniquenessConstraint18Dictionary = new Dictionary<Tuple<string, Person>, PersonDrivesCar>();
			public PersonDrivesCar GetPersonDrivesCarByInternalUniquenessConstraint18(string DrivesCar_vin, Person DrivenByPerson)
			{
				return this._InternalUniquenessConstraint18Dictionary[Tuple.CreateTuple(DrivesCar_vin, DrivenByPerson)];
			}
			private bool OnInternalUniquenessConstraint18Changing(PersonDrivesCar instance, Tuple<string, Person> newValue)
			{
				if (newValue != null)
				{
					PersonDrivesCar currentInstance = instance;
					if (this._InternalUniquenessConstraint18Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint18Changed(PersonDrivesCar instance, Tuple<string, Person> oldValue, Tuple<string, Person> newValue)
			{
				if (oldValue != null)
				{
					this._InternalUniquenessConstraint18Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._InternalUniquenessConstraint18Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<Person, string, Person>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint23Dictionary = new Dictionary<Tuple<Person, string, Person>, PersonBoughtCarFromPersonOnDate>();
			public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, string CarSold_vin, Person Seller)
			{
				return this._InternalUniquenessConstraint23Dictionary[Tuple.CreateTuple(Buyer, CarSold_vin, Seller)];
			}
			private bool OnInternalUniquenessConstraint23Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, string, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDate currentInstance = instance;
					if (this._InternalUniquenessConstraint23Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint23Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, string, Person> oldValue, Tuple<Person, string, Person> newValue)
			{
				if (oldValue != null)
				{
					this._InternalUniquenessConstraint23Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._InternalUniquenessConstraint23Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<string, Person, string>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint24Dictionary = new Dictionary<Tuple<string, Person, string>, PersonBoughtCarFromPersonOnDate>();
			public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(string SaleDate_YMD, Person Seller, string CarSold_vin)
			{
				return this._InternalUniquenessConstraint24Dictionary[Tuple.CreateTuple(SaleDate_YMD, Seller, CarSold_vin)];
			}
			private bool OnInternalUniquenessConstraint24Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<string, Person, string> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDate currentInstance = instance;
					if (this._InternalUniquenessConstraint24Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint24Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<string, Person, string> oldValue, Tuple<string, Person, string> newValue)
			{
				if (oldValue != null)
				{
					this._InternalUniquenessConstraint24Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._InternalUniquenessConstraint24Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<string, string, Person>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint25Dictionary = new Dictionary<Tuple<string, string, Person>, PersonBoughtCarFromPersonOnDate>();
			public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(string CarSold_vin, string SaleDate_YMD, Person Buyer)
			{
				return this._InternalUniquenessConstraint25Dictionary[Tuple.CreateTuple(CarSold_vin, SaleDate_YMD, Buyer)];
			}
			private bool OnInternalUniquenessConstraint25Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<string, string, Person> newValue)
			{
				if (newValue != null)
				{
					PersonBoughtCarFromPersonOnDate currentInstance = instance;
					if (this._InternalUniquenessConstraint25Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint25Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<string, string, Person> oldValue, Tuple<string, string, Person> newValue)
			{
				if (oldValue != null)
				{
					this._InternalUniquenessConstraint25Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._InternalUniquenessConstraint25Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<string, string>, Review> _InternalUniquenessConstraint26Dictionary = new Dictionary<Tuple<string, string>, Review>();
			public Review GetReviewByInternalUniquenessConstraint26(string Car_vin, string Criteria_Name)
			{
				return this._InternalUniquenessConstraint26Dictionary[Tuple.CreateTuple(Car_vin, Criteria_Name)];
			}
			private bool OnInternalUniquenessConstraint26Changing(Review instance, Tuple<string, string> newValue)
			{
				if (newValue != null)
				{
					Review currentInstance = instance;
					if (this._InternalUniquenessConstraint26Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint26Changed(Review instance, Tuple<string, string> oldValue, Tuple<string, string> newValue)
			{
				if (oldValue != null)
				{
					this._InternalUniquenessConstraint26Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._InternalUniquenessConstraint26Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<string, Person>, PersonHasNickName> _InternalUniquenessConstraint33Dictionary = new Dictionary<Tuple<string, Person>, PersonHasNickName>();
			public PersonHasNickName GetPersonHasNickNameByInternalUniquenessConstraint33(string NickName, Person Person)
			{
				return this._InternalUniquenessConstraint33Dictionary[Tuple.CreateTuple(NickName, Person)];
			}
			private bool OnInternalUniquenessConstraint33Changing(PersonHasNickName instance, Tuple<string, Person> newValue)
			{
				if (newValue != null)
				{
					PersonHasNickName currentInstance = instance;
					if (this._InternalUniquenessConstraint33Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnInternalUniquenessConstraint33Changed(PersonHasNickName instance, Tuple<string, Person> oldValue, Tuple<string, Person> newValue)
			{
				if (oldValue != null)
				{
					this._InternalUniquenessConstraint33Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._InternalUniquenessConstraint33Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<MalePerson, string, FemalePerson>, ChildPerson> _ExternalUniquenessConstraint3Dictionary = new Dictionary<Tuple<MalePerson, string, FemalePerson>, ChildPerson>();
			public ChildPerson GetChildPersonByExternalUniquenessConstraint3(MalePerson Father, string BirthOrder_BirthOrder_Nr, FemalePerson Mother)
			{
				return this._ExternalUniquenessConstraint3Dictionary[Tuple.CreateTuple(Father, BirthOrder_BirthOrder_Nr, Mother)];
			}
			private bool OnExternalUniquenessConstraint3Changing(ChildPerson instance, Tuple<MalePerson, string, FemalePerson> newValue)
			{
				if (newValue != null)
				{
					ChildPerson currentInstance = instance;
					if (this._ExternalUniquenessConstraint3Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint3Changed(ChildPerson instance, Tuple<MalePerson, string, FemalePerson> oldValue, Tuple<MalePerson, string, FemalePerson> newValue)
			{
				if (oldValue != null)
				{
					this._ExternalUniquenessConstraint3Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._ExternalUniquenessConstraint3Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<string, string>, Person> _ExternalUniquenessConstraint1Dictionary = new Dictionary<Tuple<string, string>, Person>();
			public Person GetPersonByExternalUniquenessConstraint1(string FirstName, string Date_YMD)
			{
				return this._ExternalUniquenessConstraint1Dictionary[Tuple.CreateTuple(FirstName, Date_YMD)];
			}
			private bool OnExternalUniquenessConstraint1Changing(Person instance, Tuple<string, string> newValue)
			{
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this._ExternalUniquenessConstraint1Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint1Changed(Person instance, Tuple<string, string> oldValue, Tuple<string, string> newValue)
			{
				if (oldValue != null)
				{
					this._ExternalUniquenessConstraint1Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._ExternalUniquenessConstraint1Dictionary.Add(newValue, instance);
				}
			}
			private Dictionary<Tuple<string, string>, Person> _ExternalUniquenessConstraint2Dictionary = new Dictionary<Tuple<string, string>, Person>();
			public Person GetPersonByExternalUniquenessConstraint2(string LastName, string Date_YMD)
			{
				return this._ExternalUniquenessConstraint2Dictionary[Tuple.CreateTuple(LastName, Date_YMD)];
			}
			private bool OnExternalUniquenessConstraint2Changing(Person instance, Tuple<string, string> newValue)
			{
				if (newValue != null)
				{
					Person currentInstance = instance;
					if (this._ExternalUniquenessConstraint2Dictionary.TryGetValue(newValue, out currentInstance))
					{
						return currentInstance == instance;
					}
				}
				return true;
			}
			private void OnExternalUniquenessConstraint2Changed(Person instance, Tuple<string, string> oldValue, Tuple<string, string> newValue)
			{
				if (oldValue != null)
				{
					this._ExternalUniquenessConstraint2Dictionary.Remove(oldValue);
				}
				if (newValue != null)
				{
					this._ExternalUniquenessConstraint2Dictionary.Add(newValue, instance);
				}
			}
			public Person GetPersonByPerson_id(string Person_id)
			{
			}
			public Person GetPersonBySocialSecurityNumber(string SocialSecurityNumber)
			{
			}
			public Person GetPersonByOwnsCar_vin(string OwnsCar_vin)
			{
			}
			public Task GetTaskByTask_id(string Task_id)
			{
			}
			public ValueType1 GetValueType1ByValueType1Value(string ValueType1Value)
			{
			}
		}
		#endregion // SampleModelContext
	}
}

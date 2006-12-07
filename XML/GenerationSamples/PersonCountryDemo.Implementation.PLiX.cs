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
	#region PersonCountryDemoContext
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public sealed class PersonCountryDemoContext : IPersonCountryDemoContext
	{
		public PersonCountryDemoContext()
		{
			Dictionary<Type, object> constraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<Type, object>(1);
			Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> constraintEnforcementCollectionCallbacksByTypeAndNameDictionary = new Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object>(0);
			this._ContraintEnforcementCollectionCallbacksByTypeDictionary = constraintEnforcementCollectionCallbacksByTypeDictionary;
			this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = constraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Country, Person>), new ConstraintEnforcementCollectionCallbacks<Country, Person>(new PotentialCollectionModificationCallback<Country, Person>(this.OnCountryPersonViaCountryCollectionAdding), new CommittedCollectionModificationCallback<Country, Person>(this.OnCountryPersonViaCountryCollectionAdded), null, new CommittedCollectionModificationCallback<Country, Person>(this.OnCountryPersonViaCountryCollectionRemoved)));
			List<Person> PersonList = new List<Person>();
			this._PersonList = PersonList;
			this._PersonReadOnlyCollection = new ReadOnlyCollection<Person>(PersonList);
			List<Country> CountryList = new List<Country>();
			this._CountryList = CountryList;
			this._CountryReadOnlyCollection = new ReadOnlyCollection<Country>(CountryList);
		}
		#region Exception Helpers
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		private static ArgumentException GetDifferentContextsException()
		{
			return new ArgumentException("All objects in a relationship must be part of the same Context.", "value");
		}
		private static ArgumentException GetConstraintEnforcementFailedException(string paramName)
		{
			return new ArgumentException("Argument failed constraint enforcement.", paramName);
		}
		#endregion // Exception Helpers
		#region Lookup and External Constraint Enforcement
		private readonly Dictionary<string, Country> _CountryCountry_nameDictionary = new Dictionary<string, Country>();
		public Country GetCountryByCountry_name(string Country_name)
		{
			return this._CountryCountry_nameDictionary[Country_name];
		}
		public bool TryGetCountryByCountry_name(string Country_name, out Country Country)
		{
			return this._CountryCountry_nameDictionary.TryGetValue(Country_name, out Country);
		}
		#endregion // Lookup and External Constraint Enforcement
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext;
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionCallbacks<TClass, TProperty>
			where TClass : class, IHasPersonCountryDemoContext
		{
			public ConstraintEnforcementCollectionCallbacks(PotentialCollectionModificationCallback<TClass, TProperty> adding, CommittedCollectionModificationCallback<TClass, TProperty> added, PotentialCollectionModificationCallback<TClass, TProperty> removing, CommittedCollectionModificationCallback<TClass, TProperty> removed)
			{
				this.Adding = adding;
				this.Added = added;
				this.Removing = removing;
				this.Removed = removed;
			}
			public readonly PotentialCollectionModificationCallback<TClass, TProperty> Adding;
			public readonly CommittedCollectionModificationCallback<TClass, TProperty> Added;
			public readonly PotentialCollectionModificationCallback<TClass, TProperty> Removing;
			public readonly CommittedCollectionModificationCallback<TClass, TProperty> Removed;
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private struct ConstraintEnforcementCollectionTypeAndPropertyNameKey : IEquatable<ConstraintEnforcementCollectionTypeAndPropertyNameKey>
		{
			public ConstraintEnforcementCollectionTypeAndPropertyNameKey(Type type, string name)
			{
				this.Type = type;
				this.Name = name;
			}
			public readonly Type Type;
			public readonly string Name;
			public override int GetHashCode()
			{
				return this.Type.GetHashCode() ^ this.Name.GetHashCode();
			}
			public override bool Equals(object obj)
			{
				return (obj is ConstraintEnforcementCollectionTypeAndPropertyNameKey) && this.Equals((ConstraintEnforcementCollectionTypeAndPropertyNameKey)obj);
			}
			public bool Equals(ConstraintEnforcementCollectionTypeAndPropertyNameKey other)
			{
				return this.Type.Equals(other.Type) && this.Name.Equals(other.Name);
			}
			public static bool operator ==(ConstraintEnforcementCollectionTypeAndPropertyNameKey left, ConstraintEnforcementCollectionTypeAndPropertyNameKey right)
			{
				return left.Equals(right);
			}
			public static bool operator !=(ConstraintEnforcementCollectionTypeAndPropertyNameKey left, ConstraintEnforcementCollectionTypeAndPropertyNameKey right)
			{
				return !(left.Equals(right));
			}
		}
		private readonly Dictionary<Type, object> _ContraintEnforcementCollectionCallbacksByTypeDictionary;
		private readonly Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> _ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
		private bool OnAdding<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private bool OnAdding<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private void OnAdded<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private void OnAdded<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private bool OnRemoving<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private bool OnRemoving<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private void OnRemoved<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		private void OnRemoved<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasPersonCountryDemoContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollection<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasPersonCountryDemoContext
		{
			private readonly TClass _instance;
			private readonly List<TProperty> _list = new List<TProperty>();
			public ConstraintEnforcementCollection(TClass instance)
			{
				this._instance = instance;
			}
			private System.Collections.IEnumerator GetNonGenericEnumerator()
			{
				return this.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetNonGenericEnumerator();
			}
			public IEnumerator<TProperty> GetEnumerator()
			{
				return this._list.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnAdding(this._instance, item))
				{
					this._list.Add(item);
					this._instance.Context.OnAdded(this._instance, item);
				}
			}
			public bool Remove(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnRemoving(this._instance, item))
				{
					if (this._list.Remove(item))
					{
						this._instance.Context.OnRemoved(this._instance, item);
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				for (int i = 0; i < this._list.Count; ++i)
				{
					this.Remove(this._list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return this._list.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this._list.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasPersonCountryDemoContext
		{
			private readonly TClass _instance;
			private readonly string _PropertyName;
			private readonly List<TProperty> _list = new List<TProperty>();
			public ConstraintEnforcementCollectionWithPropertyName(TClass instance, string propertyName)
			{
				this._instance = instance;
				this._PropertyName = propertyName;
			}
			private System.Collections.IEnumerator GetNonGenericEnumerator()
			{
				return this.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetNonGenericEnumerator();
			}
			public IEnumerator<TProperty> GetEnumerator()
			{
				return this._list.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnAdding(this._PropertyName, this._instance, item))
				{
					this._list.Add(item);
					this._instance.Context.OnAdded(this._PropertyName, this._instance, item);
				}
			}
			public bool Remove(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this._instance.Context.OnRemoving(this._PropertyName, this._instance, item))
				{
					if (this._list.Remove(item))
					{
						this._instance.Context.OnRemoved(this._PropertyName, this._instance, item);
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				for (int i = 0; i < this._list.Count; ++i)
				{
					this.Remove(this._list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return this._list.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this._list.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
		}
		#endregion // ConstraintEnforcementCollection
		#region Person
		public Person CreatePerson(string LastName, string FirstName)
		{
			if ((object)LastName == null)
			{
				throw new ArgumentNullException("LastName");
			}
			if ((object)FirstName == null)
			{
				throw new ArgumentNullException("FirstName");
			}
			if (!(this.OnPersonLastNameChanging(null, LastName)))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("LastName");
			}
			if (!(this.OnPersonFirstNameChanging(null, FirstName)))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("FirstName");
			}
			return new PersonCore(this, LastName, FirstName);
		}
		private bool OnPersonLastNameChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonFirstNameChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonTitleChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonCountryChanging(Person instance, Country newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw PersonCountryDemoContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonCountryChanged(Person instance, Country oldValue)
		{
			if ((object)instance.Country != null)
			{
				((ICollection<Person>)instance.Country.PersonViaCountryCollection).Add(instance);
			}
			if ((object)oldValue != null)
			{
				((ICollection<Person>)oldValue.PersonViaCountryCollection).Remove(instance);
			}
		}
		private readonly List<Person> _PersonList;
		private readonly ReadOnlyCollection<Person> _PersonReadOnlyCollection;
		public IEnumerable<Person> PersonCollection
		{
			get
			{
				return this._PersonReadOnlyCollection;
			}
		}
		#region PersonCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class PersonCore : Person
		{
			public PersonCore(PersonCountryDemoContext context, string LastName, string FirstName)
			{
				this._Context = context;
				this._LastName = LastName;
				this._FirstName = FirstName;
				context._PersonList.Add(this);
			}
			private readonly PersonCountryDemoContext _Context;
			public override PersonCountryDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("LastName")]
			private string _LastName;
			public override string LastName
			{
				get
				{
					return this._LastName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._LastName;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonLastNameChanging(this, value) && base.OnLastNameChanging(value))
						{
							this._LastName = value;
							base.OnLastNameChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("FirstName")]
			private string _FirstName;
			public override string FirstName
			{
				get
				{
					return this._FirstName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._FirstName;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonFirstNameChanging(this, value) && base.OnFirstNameChanging(value))
						{
							this._FirstName = value;
							base.OnFirstNameChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Title")]
			private string _Title;
			public override string Title
			{
				get
				{
					return this._Title;
				}
				set
				{
					string oldValue = this._Title;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonTitleChanging(this, value) && base.OnTitleChanging(value))
						{
							this._Title = value;
							base.OnTitleChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Country")]
			private Country _Country;
			public override Country Country
			{
				get
				{
					return this._Country;
				}
				set
				{
					Country oldValue = this._Country;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonCountryChanging(this, value) && base.OnCountryChanging(value))
						{
							this._Country = value;
							this._Context.OnPersonCountryChanged(this, oldValue);
							base.OnCountryChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonCore
		#endregion // Person
		#region Country
		public Country CreateCountry(string Country_name)
		{
			if ((object)Country_name == null)
			{
				throw new ArgumentNullException("Country_name");
			}
			if (!(this.OnCountryCountry_nameChanging(null, Country_name)))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("Country_name");
			}
			return new CountryCore(this, Country_name);
		}
		private bool OnCountryCountry_nameChanging(Country instance, string newValue)
		{
			Country currentInstance;
			if (this._CountryCountry_nameDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnCountryCountry_nameChanged(Country instance, string oldValue)
		{
			this._CountryCountry_nameDictionary.Add(instance.Country_name, instance);
			if ((object)oldValue != null)
			{
				this._CountryCountry_nameDictionary.Remove(oldValue);
			}
		}
		private bool OnCountryRegion_Region_codeChanging(Country instance, string newValue)
		{
			return true;
		}
		private bool OnCountryPersonViaCountryCollectionAdding(Country instance, Person value)
		{
			if ((object)this != value.Context)
			{
				throw PersonCountryDemoContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnCountryPersonViaCountryCollectionAdded(Country instance, Person value)
		{
			value.Country = instance;
		}
		private void OnCountryPersonViaCountryCollectionRemoved(Country instance, Person value)
		{
			if ((object)value.Country == instance)
			{
				value.Country = null;
			}
		}
		private readonly List<Country> _CountryList;
		private readonly ReadOnlyCollection<Country> _CountryReadOnlyCollection;
		public IEnumerable<Country> CountryCollection
		{
			get
			{
				return this._CountryReadOnlyCollection;
			}
		}
		#region CountryCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class CountryCore : Country
		{
			public CountryCore(PersonCountryDemoContext context, string Country_name)
			{
				this._Context = context;
				this._PersonViaCountryCollection = new ConstraintEnforcementCollection<Country, Person>(this);
				this._Country_name = Country_name;
				context.OnCountryCountry_nameChanged(this, null);
				context._CountryList.Add(this);
			}
			private readonly PersonCountryDemoContext _Context;
			public override PersonCountryDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("Country_name")]
			private string _Country_name;
			public override string Country_name
			{
				get
				{
					return this._Country_name;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Country_name;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnCountryCountry_nameChanging(this, value) && base.OnCountry_nameChanging(value))
						{
							this._Country_name = value;
							this._Context.OnCountryCountry_nameChanged(this, oldValue);
							base.OnCountry_nameChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Region_Region_code")]
			private string _Region_Region_code;
			public override string Region_Region_code
			{
				get
				{
					return this._Region_Region_code;
				}
				set
				{
					string oldValue = this._Region_Region_code;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnCountryRegion_Region_codeChanging(this, value) && base.OnRegion_Region_codeChanging(value))
						{
							this._Region_Region_code = value;
							base.OnRegion_Region_codeChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("PersonViaCountryCollection")]
			private readonly IEnumerable<Person> _PersonViaCountryCollection;
			public override IEnumerable<Person> PersonViaCountryCollection
			{
				get
				{
					return this._PersonViaCountryCollection;
				}
			}
		}
		#endregion // CountryCore
		#endregion // Country
	}
	#endregion // PersonCountryDemoContext
}

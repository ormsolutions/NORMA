using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace PersonCountryDemo
{
	#region PersonCountryDemoContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public sealed class PersonCountryDemoContext : IPersonCountryDemoContext
	{
		public PersonCountryDemoContext()
		{
			Dictionary<RuntimeTypeHandle, object> constraintEnforcementCollectionCallbacksByTypeDictionary = this._ContraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<RuntimeTypeHandle, object>(1, RuntimeTypeHandleEqualityComparer.Instance);
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Country, Person>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<Country, Person>(new PotentialCollectionModificationCallback<Country, Person>(this.OnCountryPersonViaCountryCollectionAdding), new CommittedCollectionModificationCallback<Country, Person>(this.OnCountryPersonViaCountryCollectionAdded), null, new CommittedCollectionModificationCallback<Country, Person>(this.OnCountryPersonViaCountryCollectionRemoved)));
			this._PersonReadOnlyCollection = new ReadOnlyCollection<Person>(this._PersonList = new List<Person>());
			this._CountryReadOnlyCollection = new ReadOnlyCollection<Country>(this._CountryList = new List<Country>());
		}
		#region Exception Helpers
		private static ArgumentException GetDifferentContextsException()
		{
			return PersonCountryDemoContext.GetDifferentContextsException("value");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		private static ArgumentException GetDifferentContextsException(string paramName)
		{
			return new ArgumentException("All objects in a relationship must be part of the same Context.", paramName);
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
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty item)
			where TClass : class, IHasPersonCountryDemoContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty item)
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
		private ConstraintEnforcementCollectionCallbacks<TClass, TProperty> GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>()
			where TClass : class, IHasPersonCountryDemoContext
		{
			return (ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>).TypeHandle];
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class RuntimeTypeHandleEqualityComparer : IEqualityComparer<RuntimeTypeHandle>
		{
			public static readonly RuntimeTypeHandleEqualityComparer Instance = new RuntimeTypeHandleEqualityComparer();
			private RuntimeTypeHandleEqualityComparer()
			{
			}
			public bool Equals(RuntimeTypeHandle x, RuntimeTypeHandle y)
			{
				return x.Equals(y);
			}
			public int GetHashCode(RuntimeTypeHandle obj)
			{
				return obj.GetHashCode();
			}
		}
		private readonly Dictionary<RuntimeTypeHandle, object> _ContraintEnforcementCollectionCallbacksByTypeDictionary;
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
				TClass instance = this._instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>();
				PotentialCollectionModificationCallback<TClass, TProperty> adding = callbacks.Adding;
				if (((object)adding == null) || adding(instance, item))
				{
					this._list.Add(item);
					CommittedCollectionModificationCallback<TClass, TProperty> added = callbacks.Added;
					if ((object)added != null)
					{
						added(instance, item);
					}
				}
			}
			public bool Remove(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				TClass instance = this._instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>();
				PotentialCollectionModificationCallback<TClass, TProperty> removing = callbacks.Removing;
				if (((object)removing == null) || removing(instance, item))
				{
					if (this._list.Remove(item))
					{
						CommittedCollectionModificationCallback<TClass, TProperty> removed = callbacks.Removed;
						if ((object)removed != null)
						{
							removed(instance, item);
						}
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				List<TProperty> list = this._list;
				for (int i = list.Count - 1; i > 0; --i)
				{
					this.Remove(list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return (item != null) && this._list.Contains(item);
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
				if ((object)this != (object)newValue.Context)
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
			public sealed override PersonCountryDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private string _LastName;
			public sealed override string LastName
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
					if (((object)oldValue != (object)value) && !(value.Equals(oldValue)))
					{
						if (this._Context.OnPersonLastNameChanging(this, value) && base.OnLastNameChanging(value))
						{
							this._LastName = value;
							base.OnLastNameChanged(oldValue);
						}
					}
				}
			}
			private string _FirstName;
			public sealed override string FirstName
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
					if (((object)oldValue != (object)value) && !(value.Equals(oldValue)))
					{
						if (this._Context.OnPersonFirstNameChanging(this, value) && base.OnFirstNameChanging(value))
						{
							this._FirstName = value;
							base.OnFirstNameChanged(oldValue);
						}
					}
				}
			}
			private string _Title;
			public sealed override string Title
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
			private Country _Country;
			public sealed override Country Country
			{
				get
				{
					return this._Country;
				}
				set
				{
					Country oldValue = this._Country;
					if ((object)oldValue != (object)value)
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
				if ((object)currentInstance != (object)instance)
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
		private bool OnCountryPersonViaCountryCollectionAdding(Country instance, Person item)
		{
			if ((object)this != (object)item.Context)
			{
				throw PersonCountryDemoContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnCountryPersonViaCountryCollectionAdded(Country instance, Person item)
		{
			item.Country = instance;
		}
		private void OnCountryPersonViaCountryCollectionRemoved(Country instance, Person item)
		{
			if ((object)item.Country == (object)instance)
			{
				item.Country = null;
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
			public sealed override PersonCountryDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private string _Country_name;
			public sealed override string Country_name
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
					if (((object)oldValue != (object)value) && !(value.Equals(oldValue)))
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
			private string _Region_Region_code;
			public sealed override string Region_Region_code
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
			private readonly IEnumerable<Person> _PersonViaCountryCollection;
			public sealed override IEnumerable<Person> PersonViaCountryCollection
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

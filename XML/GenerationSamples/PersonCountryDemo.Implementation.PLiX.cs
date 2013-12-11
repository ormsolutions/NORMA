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
		private readonly Dictionary<int, Person> _PersonPersonIdDictionary = new Dictionary<int, Person>();
		public Person GetPersonByPersonId(int personId)
		{
			return this._PersonPersonIdDictionary[personId];
		}
		public bool TryGetPersonByPersonId(int personId, out Person person)
		{
			return this._PersonPersonIdDictionary.TryGetValue(personId, out person);
		}
		private readonly Dictionary<string, Country> _CountryCountryNameDictionary = new Dictionary<string, Country>();
		public Country GetCountryByCountryName(string countryName)
		{
			return this._CountryCountryNameDictionary[countryName];
		}
		public bool TryGetCountryByCountryName(string countryName, out Country country)
		{
			return this._CountryCountryNameDictionary.TryGetValue(countryName, out country);
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
			private readonly TClass _Instance;
			private readonly List<TProperty> _List = new List<TProperty>();
			public ConstraintEnforcementCollection(TClass instance)
			{
				this._Instance = instance;
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
				return this._List.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				TClass instance = this._Instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>();
				PotentialCollectionModificationCallback<TClass, TProperty> adding = callbacks.Adding;
				if ((object)adding == null || adding(instance, item))
				{
					this._List.Add(item);
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
				TClass instance = this._Instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>();
				PotentialCollectionModificationCallback<TClass, TProperty> removing = callbacks.Removing;
				if ((object)removing == null || removing(instance, item))
				{
					if (this._List.Remove(item))
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
				List<TProperty> list = this._List;
				for (int i = list.Count - 1; i > 0; --i)
				{
					this.Remove(list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return item != null && this._List.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this._List.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this._List.Count;
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
		public Person CreatePerson(string lastName, string firstName)
		{
			if ((object)lastName == null)
			{
				throw new ArgumentNullException("lastName");
			}
			if ((object)firstName == null)
			{
				throw new ArgumentNullException("firstName");
			}
			if (!this.OnPersonLastNameChanging(null, lastName))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("lastName");
			}
			if (!this.OnPersonFirstNameChanging(null, firstName))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("firstName");
			}
			return new PersonImpl(this, lastName, firstName);
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
		#region PersonImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class PersonImpl : Person
		{
			public PersonImpl(PersonCountryDemoContext context, string lastName, string firstName)
			{
				this._Context = context;
				this._LastName = lastName;
				this._FirstName = firstName;
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
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
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
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
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
					if (!object.Equals(oldValue, value))
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
		#endregion // PersonImpl
		#endregion // Person
		#region Country
		public Country CreateCountry(string countryName)
		{
			if ((object)countryName == null)
			{
				throw new ArgumentNullException("countryName");
			}
			if (!this.OnCountryCountryNameChanging(null, countryName))
			{
				throw PersonCountryDemoContext.GetConstraintEnforcementFailedException("countryName");
			}
			return new CountryImpl(this, countryName);
		}
		private bool OnCountryCountryNameChanging(Country instance, string newValue)
		{
			Country currentInstance;
			if (this._CountryCountryNameDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != (object)instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnCountryCountryNameChanged(Country instance, string oldValue)
		{
			this._CountryCountryNameDictionary.Add(instance.CountryName, instance);
			if ((object)oldValue != null)
			{
				this._CountryCountryNameDictionary.Remove(oldValue);
			}
		}
		private bool OnCountryRegionCodeChanging(Country instance, string newValue)
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
		#region CountryImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class CountryImpl : Country
		{
			public CountryImpl(PersonCountryDemoContext context, string countryName)
			{
				this._Context = context;
				this._PersonViaCountryCollection = new ConstraintEnforcementCollection<Country, Person>(this);
				this._CountryName = countryName;
				context.OnCountryCountryNameChanged(this, null);
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
			private string _CountryName;
			public sealed override string CountryName
			{
				get
				{
					return this._CountryName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._CountryName;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnCountryCountryNameChanging(this, value) && base.OnCountryNameChanging(value))
						{
							this._CountryName = value;
							this._Context.OnCountryCountryNameChanged(this, oldValue);
							base.OnCountryNameChanged(oldValue);
						}
					}
				}
			}
			private string _RegionCode;
			public sealed override string RegionCode
			{
				get
				{
					return this._RegionCode;
				}
				set
				{
					string oldValue = this._RegionCode;
					if (!object.Equals(oldValue, value))
					{
						if (this._Context.OnCountryRegionCodeChanging(this, value) && base.OnRegionCodeChanging(value))
						{
							this._RegionCode = value;
							base.OnRegionCodeChanged(oldValue);
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
		#endregion // CountryImpl
		#endregion // Country
	}
	#endregion // PersonCountryDemoContext
}

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
	#region SampleModelContext
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public sealed class SampleModelContext : ISampleModelContext
	{
		public SampleModelContext()
		{
			Dictionary<Type, object> constraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<Type, object>(5);
			Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> constraintEnforcementCollectionCallbacksByTypeAndNameDictionary = new Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object>(2);
			this._ContraintEnforcementCollectionCallbacksByTypeDictionary = constraintEnforcementCollectionCallbacksByTypeDictionary;
			this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = constraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, Task>), new ConstraintEnforcementCollectionCallbacks<Person, Task>(new PotentialCollectionModificationCallback<Person, Task>(this.OnPersonTaskViaPersonCollectionAdding), new CommittedCollectionModificationCallback<Person, Task>(this.OnPersonTaskViaPersonCollectionAdded), null, new CommittedCollectionModificationCallback<Person, Task>(this.OnPersonTaskViaPersonCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, ValueType1>), new ConstraintEnforcementCollectionCallbacks<Person, ValueType1>(new PotentialCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdding), new CommittedCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdded), null, new CommittedCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>), "PersonBoughtCarFromPersonOnDateViaBuyerCollection"), new ConstraintEnforcementCollectionCallbacks<Person, PersonBoughtCarFromPersonOnDate>(new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdded), null, new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>), "PersonBoughtCarFromPersonOnDateViaSellerCollection"), new ConstraintEnforcementCollectionCallbacks<Person, PersonBoughtCarFromPersonOnDate>(new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdded), null, new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<MalePerson, ChildPerson>), new ConstraintEnforcementCollectionCallbacks<MalePerson, ChildPerson>(new PotentialCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonViaFatherCollectionAdding), new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonViaFatherCollectionAdded), null, new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonViaFatherCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<FemalePerson, ChildPerson>), new ConstraintEnforcementCollectionCallbacks<FemalePerson, ChildPerson>(new PotentialCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonViaMotherCollectionAdding), new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonViaMotherCollectionAdded), null, new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonViaMotherCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<ValueType1, Person>), new ConstraintEnforcementCollectionCallbacks<ValueType1, Person>(new PotentialCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdding), new CommittedCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdded), null, new CommittedCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionRemoved)));
			List<Person> PersonList = new List<Person>();
			this._PersonList = PersonList;
			this._PersonReadOnlyCollection = new ReadOnlyCollection<Person>(PersonList);
			List<MalePerson> MalePersonList = new List<MalePerson>();
			this._MalePersonList = MalePersonList;
			this._MalePersonReadOnlyCollection = new ReadOnlyCollection<MalePerson>(MalePersonList);
			List<FemalePerson> FemalePersonList = new List<FemalePerson>();
			this._FemalePersonList = FemalePersonList;
			this._FemalePersonReadOnlyCollection = new ReadOnlyCollection<FemalePerson>(FemalePersonList);
			List<ChildPerson> ChildPersonList = new List<ChildPerson>();
			this._ChildPersonList = ChildPersonList;
			this._ChildPersonReadOnlyCollection = new ReadOnlyCollection<ChildPerson>(ChildPersonList);
			List<Death> DeathList = new List<Death>();
			this._DeathList = DeathList;
			this._DeathReadOnlyCollection = new ReadOnlyCollection<Death>(DeathList);
			List<NaturalDeath> NaturalDeathList = new List<NaturalDeath>();
			this._NaturalDeathList = NaturalDeathList;
			this._NaturalDeathReadOnlyCollection = new ReadOnlyCollection<NaturalDeath>(NaturalDeathList);
			List<UnnaturalDeath> UnnaturalDeathList = new List<UnnaturalDeath>();
			this._UnnaturalDeathList = UnnaturalDeathList;
			this._UnnaturalDeathReadOnlyCollection = new ReadOnlyCollection<UnnaturalDeath>(UnnaturalDeathList);
			List<Task> TaskList = new List<Task>();
			this._TaskList = TaskList;
			this._TaskReadOnlyCollection = new ReadOnlyCollection<Task>(TaskList);
			List<ValueType1> ValueType1List = new List<ValueType1>();
			this._ValueType1List = ValueType1List;
			this._ValueType1ReadOnlyCollection = new ReadOnlyCollection<ValueType1>(ValueType1List);
			List<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateList = new List<PersonBoughtCarFromPersonOnDate>();
			this._PersonBoughtCarFromPersonOnDateList = PersonBoughtCarFromPersonOnDateList;
			this._PersonBoughtCarFromPersonOnDateReadOnlyCollection = new ReadOnlyCollection<PersonBoughtCarFromPersonOnDate>(PersonBoughtCarFromPersonOnDateList);
			List<Review> ReviewList = new List<Review>();
			this._ReviewList = ReviewList;
			this._ReviewReadOnlyCollection = new ReadOnlyCollection<Review>(ReviewList);
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
		private readonly Dictionary<Tuple<MalePerson, int, FemalePerson>, ChildPerson> _InternalUniquenessConstraint49Dictionary = new Dictionary<Tuple<MalePerson, int, FemalePerson>, ChildPerson>();
		public ChildPerson GetChildPersonByInternalUniquenessConstraint49(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother)
		{
			return this._InternalUniquenessConstraint49Dictionary[Tuple.CreateTuple<MalePerson, int, FemalePerson>(Father, BirthOrder_BirthOrder_Nr, Mother)];
		}
		public bool TryGetChildPersonByInternalUniquenessConstraint49(MalePerson Father, int BirthOrder_BirthOrder_Nr, FemalePerson Mother, out ChildPerson ChildPerson)
		{
			return this._InternalUniquenessConstraint49Dictionary.TryGetValue(Tuple.CreateTuple<MalePerson, int, FemalePerson>(Father, BirthOrder_BirthOrder_Nr, Mother), out ChildPerson);
		}
		private bool OnInternalUniquenessConstraint49Changing(ChildPerson instance, Tuple<MalePerson, int, FemalePerson> newValue)
		{
			if ((object)newValue != null)
			{
				ChildPerson currentInstance;
				if (this._InternalUniquenessConstraint49Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint49Changed(ChildPerson instance, Tuple<MalePerson, int, FemalePerson> oldValue, Tuple<MalePerson, int, FemalePerson> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint49Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint49Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, int>, Person> _ExternalUniquenessConstraint1Dictionary = new Dictionary<Tuple<string, int>, Person>();
		public Person GetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD)
		{
			return this._ExternalUniquenessConstraint1Dictionary[Tuple.CreateTuple<string, int>(FirstName, Date_YMD)];
		}
		public bool TryGetPersonByExternalUniquenessConstraint1(string FirstName, int Date_YMD, out Person Person)
		{
			return this._ExternalUniquenessConstraint1Dictionary.TryGetValue(Tuple.CreateTuple<string, int>(FirstName, Date_YMD), out Person);
		}
		private bool OnExternalUniquenessConstraint1Changing(Person instance, Tuple<string, int> newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._ExternalUniquenessConstraint1Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnExternalUniquenessConstraint1Changed(Person instance, Tuple<string, int> oldValue, Tuple<string, int> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ExternalUniquenessConstraint1Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ExternalUniquenessConstraint1Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, int>, Person> _ExternalUniquenessConstraint2Dictionary = new Dictionary<Tuple<string, int>, Person>();
		public Person GetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD)
		{
			return this._ExternalUniquenessConstraint2Dictionary[Tuple.CreateTuple<string, int>(LastName, Date_YMD)];
		}
		public bool TryGetPersonByExternalUniquenessConstraint2(string LastName, int Date_YMD, out Person Person)
		{
			return this._ExternalUniquenessConstraint2Dictionary.TryGetValue(Tuple.CreateTuple<string, int>(LastName, Date_YMD), out Person);
		}
		private bool OnExternalUniquenessConstraint2Changing(Person instance, Tuple<string, int> newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._ExternalUniquenessConstraint2Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnExternalUniquenessConstraint2Changed(Person instance, Tuple<string, int> oldValue, Tuple<string, int> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ExternalUniquenessConstraint2Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ExternalUniquenessConstraint2Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<Person, int, Person>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint23Dictionary = new Dictionary<Tuple<Person, int, Person>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller)
		{
			return this._InternalUniquenessConstraint23Dictionary[Tuple.CreateTuple<Person, int, Person>(Buyer, CarSold_vin, Seller)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint23(Person Buyer, int CarSold_vin, Person Seller, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate)
		{
			return this._InternalUniquenessConstraint23Dictionary.TryGetValue(Tuple.CreateTuple<Person, int, Person>(Buyer, CarSold_vin, Seller), out PersonBoughtCarFromPersonOnDate);
		}
		private bool OnInternalUniquenessConstraint23Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, int, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._InternalUniquenessConstraint23Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint23Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, int, Person> oldValue, Tuple<Person, int, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint23Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint23Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<int, Person, int>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint24Dictionary = new Dictionary<Tuple<int, Person, int>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin)
		{
			return this._InternalUniquenessConstraint24Dictionary[Tuple.CreateTuple<int, Person, int>(SaleDate_YMD, Seller, CarSold_vin)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint24(int SaleDate_YMD, Person Seller, int CarSold_vin, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate)
		{
			return this._InternalUniquenessConstraint24Dictionary.TryGetValue(Tuple.CreateTuple<int, Person, int>(SaleDate_YMD, Seller, CarSold_vin), out PersonBoughtCarFromPersonOnDate);
		}
		private bool OnInternalUniquenessConstraint24Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<int, Person, int> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._InternalUniquenessConstraint24Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint24Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<int, Person, int> oldValue, Tuple<int, Person, int> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint24Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint24Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<int, int, Person>, PersonBoughtCarFromPersonOnDate> _InternalUniquenessConstraint25Dictionary = new Dictionary<Tuple<int, int, Person>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer)
		{
			return this._InternalUniquenessConstraint25Dictionary[Tuple.CreateTuple<int, int, Person>(CarSold_vin, SaleDate_YMD, Buyer)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByInternalUniquenessConstraint25(int CarSold_vin, int SaleDate_YMD, Person Buyer, out PersonBoughtCarFromPersonOnDate PersonBoughtCarFromPersonOnDate)
		{
			return this._InternalUniquenessConstraint25Dictionary.TryGetValue(Tuple.CreateTuple<int, int, Person>(CarSold_vin, SaleDate_YMD, Buyer), out PersonBoughtCarFromPersonOnDate);
		}
		private bool OnInternalUniquenessConstraint25Changing(PersonBoughtCarFromPersonOnDate instance, Tuple<int, int, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._InternalUniquenessConstraint25Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint25Changed(PersonBoughtCarFromPersonOnDate instance, Tuple<int, int, Person> oldValue, Tuple<int, int, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint25Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint25Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<int, string>, Review> _InternalUniquenessConstraint26Dictionary = new Dictionary<Tuple<int, string>, Review>();
		public Review GetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name)
		{
			return this._InternalUniquenessConstraint26Dictionary[Tuple.CreateTuple<int, string>(Car_vin, Criterion_Name)];
		}
		public bool TryGetReviewByInternalUniquenessConstraint26(int Car_vin, string Criterion_Name, out Review Review)
		{
			return this._InternalUniquenessConstraint26Dictionary.TryGetValue(Tuple.CreateTuple<int, string>(Car_vin, Criterion_Name), out Review);
		}
		private bool OnInternalUniquenessConstraint26Changing(Review instance, Tuple<int, string> newValue)
		{
			if ((object)newValue != null)
			{
				Review currentInstance;
				if (this._InternalUniquenessConstraint26Dictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == instance;
				}
			}
			return true;
		}
		private void OnInternalUniquenessConstraint26Changed(Review instance, Tuple<int, string> oldValue, Tuple<int, string> newValue)
		{
			if ((object)oldValue != null)
			{
				this._InternalUniquenessConstraint26Dictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._InternalUniquenessConstraint26Dictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<string, Person> _PersonOptionalUniqueStringDictionary = new Dictionary<string, Person>();
		public Person GetPersonByOptionalUniqueString(string OptionalUniqueString)
		{
			return this._PersonOptionalUniqueStringDictionary[OptionalUniqueString];
		}
		public bool TryGetPersonByOptionalUniqueString(string OptionalUniqueString, out Person Person)
		{
			return this._PersonOptionalUniqueStringDictionary.TryGetValue(OptionalUniqueString, out Person);
		}
		private readonly Dictionary<int, Person> _PersonOwnsCar_vinDictionary = new Dictionary<int, Person>();
		public Person GetPersonByOwnsCar_vin(int OwnsCar_vin)
		{
			return this._PersonOwnsCar_vinDictionary[OwnsCar_vin];
		}
		public bool TryGetPersonByOwnsCar_vin(int OwnsCar_vin, out Person Person)
		{
			return this._PersonOwnsCar_vinDictionary.TryGetValue(OwnsCar_vin, out Person);
		}
		private readonly Dictionary<decimal, Person> _PersonOptionalUniqueDecimalDictionary = new Dictionary<decimal, Person>();
		public Person GetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal)
		{
			return this._PersonOptionalUniqueDecimalDictionary[OptionalUniqueDecimal];
		}
		public bool TryGetPersonByOptionalUniqueDecimal(decimal OptionalUniqueDecimal, out Person Person)
		{
			return this._PersonOptionalUniqueDecimalDictionary.TryGetValue(OptionalUniqueDecimal, out Person);
		}
		private readonly Dictionary<decimal, Person> _PersonMandatoryUniqueDecimalDictionary = new Dictionary<decimal, Person>();
		public Person GetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal)
		{
			return this._PersonMandatoryUniqueDecimalDictionary[MandatoryUniqueDecimal];
		}
		public bool TryGetPersonByMandatoryUniqueDecimal(decimal MandatoryUniqueDecimal, out Person Person)
		{
			return this._PersonMandatoryUniqueDecimalDictionary.TryGetValue(MandatoryUniqueDecimal, out Person);
		}
		private readonly Dictionary<string, Person> _PersonMandatoryUniqueStringDictionary = new Dictionary<string, Person>();
		public Person GetPersonByMandatoryUniqueString(string MandatoryUniqueString)
		{
			return this._PersonMandatoryUniqueStringDictionary[MandatoryUniqueString];
		}
		public bool TryGetPersonByMandatoryUniqueString(string MandatoryUniqueString, out Person Person)
		{
			return this._PersonMandatoryUniqueStringDictionary.TryGetValue(MandatoryUniqueString, out Person);
		}
		private readonly Dictionary<int, ValueType1> _ValueType1ValueType1ValueDictionary = new Dictionary<int, ValueType1>();
		public ValueType1 GetValueType1ByValueType1Value(int ValueType1Value)
		{
			return this._ValueType1ValueType1ValueDictionary[ValueType1Value];
		}
		public bool TryGetValueType1ByValueType1Value(int ValueType1Value, out ValueType1 ValueType1)
		{
			return this._ValueType1ValueType1ValueDictionary.TryGetValue(ValueType1Value, out ValueType1);
		}
		#endregion // Lookup and External Constraint Enforcement
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext;
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionCallbacks<TClass, TProperty>
			where TClass : class, IHasSampleModelContext
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
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private bool OnAdding<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> adding = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Adding;
			if (adding != null)
			{
				return adding(instance, value);
			}
			return true;
		}
		private void OnAdded<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private void OnAdded<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> added = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Added;
			if (added != null)
			{
				added(instance, value);
			}
		}
		private bool OnRemoving<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private bool OnRemoving<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			PotentialCollectionModificationCallback<TClass, TProperty> removing = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removing;
			if (removing != null)
			{
				return removing(instance, value);
			}
			return true;
		}
		private void OnRemoved<TClass, TProperty>(TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		private void OnRemoved<TClass, TProperty>(string propertyName, TClass instance, TProperty value)
			where TClass : class, IHasSampleModelContext
		{
			CommittedCollectionModificationCallback<TClass, TProperty> removed = ((ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>), propertyName)]).Removed;
			if (removed != null)
			{
				removed(instance, value);
			}
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollection<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasSampleModelContext
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
			where TClass : class, IHasSampleModelContext
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
		public Person CreatePerson(string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, bool hasParents, decimal MandatoryUniqueDecimal, string MandatoryUniqueString)
		{
			if ((object)FirstName == null)
			{
				throw new ArgumentNullException("FirstName");
			}
			if ((object)LastName == null)
			{
				throw new ArgumentNullException("LastName");
			}
			if ((object)Gender_Gender_Code == null)
			{
				throw new ArgumentNullException("Gender_Gender_Code");
			}
			if ((object)MandatoryUniqueString == null)
			{
				throw new ArgumentNullException("MandatoryUniqueString");
			}
			if (!(this.OnPersonFirstNameChanging(null, FirstName)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("FirstName");
			}
			if (!(this.OnPersonDate_YMDChanging(null, Date_YMD)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Date_YMD");
			}
			if (!(this.OnPersonLastNameChanging(null, LastName)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("LastName");
			}
			if (!(this.OnPersonGender_Gender_CodeChanging(null, Gender_Gender_Code)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Gender_Gender_Code");
			}
			if (!(this.OnPersonhasParentsChanging(null, hasParents)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("hasParents");
			}
			if (!(this.OnPersonMandatoryUniqueDecimalChanging(null, MandatoryUniqueDecimal)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("MandatoryUniqueDecimal");
			}
			if (!(this.OnPersonMandatoryUniqueStringChanging(null, MandatoryUniqueString)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("MandatoryUniqueString");
			}
			return new PersonCore(this, FirstName, Date_YMD, LastName, Gender_Gender_Code, hasParents, MandatoryUniqueDecimal, MandatoryUniqueString);
		}
		private bool OnPersonFirstNameChanging(Person instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple<string, int>(newValue, instance.Date_YMD))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonFirstNameChanged(Person instance, string oldValue)
		{
			Tuple<string, int> ExternalUniquenessConstraint1OldValueTuple;
			if ((object)oldValue != null)
			{
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple<string, int>(oldValue, instance.Date_YMD);
			}
			else
			{
				ExternalUniquenessConstraint1OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple<string, int>(instance.FirstName, instance.Date_YMD));
		}
		private bool OnPersonDate_YMDChanging(Person instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint1Changing(instance, Tuple.CreateTuple<string, int>(instance.FirstName, newValue))))
				{
					return false;
				}
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple<string, int>(instance.LastName, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonDate_YMDChanged(Person instance, Nullable<int> oldValue)
		{
			Tuple<string, int> ExternalUniquenessConstraint1OldValueTuple;
			Tuple<string, int> ExternalUniquenessConstraint2OldValueTuple;
			if (oldValue.HasValue)
			{
				ExternalUniquenessConstraint1OldValueTuple = Tuple.CreateTuple<string, int>(instance.FirstName, oldValue.Value);
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple<string, int>(instance.LastName, oldValue.Value);
			}
			else
			{
				ExternalUniquenessConstraint1OldValueTuple = null;
				ExternalUniquenessConstraint2OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint1Changed(instance, ExternalUniquenessConstraint1OldValueTuple, Tuple.CreateTuple<string, int>(instance.FirstName, instance.Date_YMD));
			this.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple<string, int>(instance.LastName, instance.Date_YMD));
		}
		private bool OnPersonLastNameChanging(Person instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnExternalUniquenessConstraint2Changing(instance, Tuple.CreateTuple<string, int>(newValue, instance.Date_YMD))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonLastNameChanged(Person instance, string oldValue)
		{
			Tuple<string, int> ExternalUniquenessConstraint2OldValueTuple;
			if ((object)oldValue != null)
			{
				ExternalUniquenessConstraint2OldValueTuple = Tuple.CreateTuple<string, int>(oldValue, instance.Date_YMD);
			}
			else
			{
				ExternalUniquenessConstraint2OldValueTuple = null;
			}
			this.OnExternalUniquenessConstraint2Changed(instance, ExternalUniquenessConstraint2OldValueTuple, Tuple.CreateTuple<string, int>(instance.LastName, instance.Date_YMD));
		}
		private bool OnPersonOptionalUniqueStringChanging(Person instance, string newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueStringDictionary.TryGetValue(newValue, out currentInstance))
				{
					if ((object)currentInstance != instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOptionalUniqueStringChanged(Person instance, string oldValue)
		{
			if ((object)instance.OptionalUniqueString != null)
			{
				this._PersonOptionalUniqueStringDictionary.Add(instance.OptionalUniqueString, instance);
			}
			if ((object)oldValue != null)
			{
				this._PersonOptionalUniqueStringDictionary.Remove(oldValue);
			}
		}
		private bool OnPersonHatType_ColorARGBChanging(Person instance, Nullable<int> newValue)
		{
			return true;
		}
		private bool OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonOwnsCar_vinChanging(Person instance, Nullable<int> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonOwnsCar_vinDictionary.TryGetValue(newValue.Value, out currentInstance))
				{
					if ((object)currentInstance != instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOwnsCar_vinChanged(Person instance, Nullable<int> oldValue)
		{
			if (instance.OwnsCar_vin.HasValue)
			{
				this._PersonOwnsCar_vinDictionary.Add(instance.OwnsCar_vin.Value, instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonOwnsCar_vinDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnPersonGender_Gender_CodeChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonhasParentsChanging(Person instance, bool newValue)
		{
			return true;
		}
		private bool OnPersonOptionalUniqueDecimalChanging(Person instance, Nullable<decimal> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueDecimalDictionary.TryGetValue(newValue.Value, out currentInstance))
				{
					if ((object)currentInstance != instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOptionalUniqueDecimalChanged(Person instance, Nullable<decimal> oldValue)
		{
			if (instance.OptionalUniqueDecimal.HasValue)
			{
				this._PersonOptionalUniqueDecimalDictionary.Add(instance.OptionalUniqueDecimal.Value, instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonOptionalUniqueDecimalDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnPersonMandatoryUniqueDecimalChanging(Person instance, decimal newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueDecimalDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonMandatoryUniqueDecimalChanged(Person instance, Nullable<decimal> oldValue)
		{
			this._PersonMandatoryUniqueDecimalDictionary.Add(instance.MandatoryUniqueDecimal, instance);
			if (oldValue.HasValue)
			{
				this._PersonMandatoryUniqueDecimalDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnPersonMandatoryUniqueStringChanging(Person instance, string newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueStringDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonMandatoryUniqueStringChanged(Person instance, string oldValue)
		{
			this._PersonMandatoryUniqueStringDictionary.Add(instance.MandatoryUniqueString, instance);
			if ((object)oldValue != null)
			{
				this._PersonMandatoryUniqueStringDictionary.Remove(oldValue);
			}
		}
		private bool OnPersonHusbandChanging(Person instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonHusbandChanged(Person instance, Person oldValue)
		{
			if ((object)instance.Husband != null)
			{
				instance.Husband.Wife = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Wife = null;
			}
		}
		private bool OnPersonValueType1DoesSomethingElseWithChanging(Person instance, ValueType1 newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonValueType1DoesSomethingElseWithChanged(Person instance, ValueType1 oldValue)
		{
			if ((object)instance.ValueType1DoesSomethingElseWith != null)
			{
				((ICollection<Person>)instance.ValueType1DoesSomethingElseWith.DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection).Add(instance);
			}
			if ((object)oldValue != null)
			{
				((ICollection<Person>)oldValue.DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection).Remove(instance);
			}
		}
		private bool OnPersonMalePersonChanging(Person instance, MalePerson newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonMalePersonChanged(Person instance, MalePerson oldValue)
		{
			if ((object)instance.MalePerson != null)
			{
				instance.MalePerson.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonFemalePersonChanging(Person instance, FemalePerson newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonFemalePersonChanged(Person instance, FemalePerson oldValue)
		{
			if ((object)instance.FemalePerson != null)
			{
				instance.FemalePerson.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonChildPersonChanging(Person instance, ChildPerson newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonChildPersonChanged(Person instance, ChildPerson oldValue)
		{
			if ((object)instance.ChildPerson != null)
			{
				instance.ChildPerson.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonDeathChanging(Person instance, Death newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonDeathChanged(Person instance, Death oldValue)
		{
			if ((object)instance.Death != null)
			{
				instance.Death.Person = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Person = null;
			}
		}
		private bool OnPersonWifeChanging(Person instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonWifeChanged(Person instance, Person oldValue)
		{
			if ((object)instance.Wife != null)
			{
				instance.Wife.Husband = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Husband = null;
			}
		}
		private bool OnPersonTaskViaPersonCollectionAdding(Person instance, Task value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonTaskViaPersonCollectionAdded(Person instance, Task value)
		{
			value.Person = instance;
		}
		private void OnPersonTaskViaPersonCollectionRemoved(Person instance, Task value)
		{
			if ((object)value.Person == instance)
			{
				value.Person = null;
			}
		}
		private bool OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdding(Person instance, ValueType1 value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdded(Person instance, ValueType1 value)
		{
			value.DoesSomethingWithPerson = instance;
		}
		private void OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionRemoved(Person instance, ValueType1 value)
		{
			if ((object)value.DoesSomethingWithPerson == instance)
			{
				value.DoesSomethingWithPerson = null;
			}
		}
		private bool OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			value.Buyer = instance;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			if ((object)value.Buyer == instance)
			{
				value.Buyer = null;
			}
		}
		private bool OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdding(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdded(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			value.Seller = instance;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionRemoved(Person instance, PersonBoughtCarFromPersonOnDate value)
		{
			if ((object)value.Seller == instance)
			{
				value.Seller = null;
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
			public PersonCore(SampleModelContext context, string FirstName, int Date_YMD, string LastName, string Gender_Gender_Code, bool hasParents, decimal MandatoryUniqueDecimal, string MandatoryUniqueString)
			{
				this._Context = context;
				this._TaskViaPersonCollection = new ConstraintEnforcementCollection<Person, Task>(this);
				this._ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection = new ConstraintEnforcementCollection<Person, ValueType1>(this);
				this._PersonBoughtCarFromPersonOnDateViaBuyerCollection = new ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>(this, "PersonBoughtCarFromPersonOnDateViaBuyerCollection");
				this._PersonBoughtCarFromPersonOnDateViaSellerCollection = new ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>(this, "PersonBoughtCarFromPersonOnDateViaSellerCollection");
				this._FirstName = FirstName;
				context.OnPersonFirstNameChanged(this, null);
				this._Date_YMD = Date_YMD;
				context.OnPersonDate_YMDChanged(this, null);
				this._LastName = LastName;
				context.OnPersonLastNameChanged(this, null);
				this._Gender_Gender_Code = Gender_Gender_Code;
				this._hasParents = hasParents;
				this._MandatoryUniqueDecimal = MandatoryUniqueDecimal;
				context.OnPersonMandatoryUniqueDecimalChanged(this, null);
				this._MandatoryUniqueString = MandatoryUniqueString;
				context.OnPersonMandatoryUniqueStringChanged(this, null);
				context._PersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
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
							this._Context.OnPersonFirstNameChanged(this, oldValue);
							base.OnFirstNameChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Date_YMD")]
			private int _Date_YMD;
			public override int Date_YMD
			{
				get
				{
					return this._Date_YMD;
				}
				set
				{
					int oldValue = this._Date_YMD;
					if (oldValue != value)
					{
						if (this._Context.OnPersonDate_YMDChanging(this, value) && base.OnDate_YMDChanging(value))
						{
							this._Date_YMD = value;
							this._Context.OnPersonDate_YMDChanged(this, oldValue);
							base.OnDate_YMDChanged(oldValue);
						}
					}
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
							this._Context.OnPersonLastNameChanged(this, oldValue);
							base.OnLastNameChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("OptionalUniqueString")]
			private string _OptionalUniqueString;
			public override string OptionalUniqueString
			{
				get
				{
					return this._OptionalUniqueString;
				}
				set
				{
					string oldValue = this._OptionalUniqueString;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonOptionalUniqueStringChanging(this, value) && base.OnOptionalUniqueStringChanging(value))
						{
							this._OptionalUniqueString = value;
							this._Context.OnPersonOptionalUniqueStringChanged(this, oldValue);
							base.OnOptionalUniqueStringChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("HatType_ColorARGB")]
			private Nullable<int> _HatType_ColorARGB;
			public override Nullable<int> HatType_ColorARGB
			{
				get
				{
					return this._HatType_ColorARGB;
				}
				set
				{
					Nullable<int> oldValue = this._HatType_ColorARGB;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonHatType_ColorARGBChanging(this, value) && base.OnHatType_ColorARGBChanging(value))
						{
							this._HatType_ColorARGB = value;
							base.OnHatType_ColorARGBChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("HatType_HatTypeStyle_HatTypeStyle_Description")]
			private string _HatType_HatTypeStyle_HatTypeStyle_Description;
			public override string HatType_HatTypeStyle_HatTypeStyle_Description
			{
				get
				{
					return this._HatType_HatTypeStyle_HatTypeStyle_Description;
				}
				set
				{
					string oldValue = this._HatType_HatTypeStyle_HatTypeStyle_Description;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(this, value) && base.OnHatType_HatTypeStyle_HatTypeStyle_DescriptionChanging(value))
						{
							this._HatType_HatTypeStyle_HatTypeStyle_Description = value;
							base.OnHatType_HatTypeStyle_HatTypeStyle_DescriptionChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("OwnsCar_vin")]
			private Nullable<int> _OwnsCar_vin;
			public override Nullable<int> OwnsCar_vin
			{
				get
				{
					return this._OwnsCar_vin;
				}
				set
				{
					Nullable<int> oldValue = this._OwnsCar_vin;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonOwnsCar_vinChanging(this, value) && base.OnOwnsCar_vinChanging(value))
						{
							this._OwnsCar_vin = value;
							this._Context.OnPersonOwnsCar_vinChanged(this, oldValue);
							base.OnOwnsCar_vinChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Gender_Gender_Code")]
			private string _Gender_Gender_Code;
			public override string Gender_Gender_Code
			{
				get
				{
					return this._Gender_Gender_Code;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Gender_Gender_Code;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonGender_Gender_CodeChanging(this, value) && base.OnGender_Gender_CodeChanging(value))
						{
							this._Gender_Gender_Code = value;
							base.OnGender_Gender_CodeChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("hasParents")]
			private bool _hasParents;
			public override bool hasParents
			{
				get
				{
					return this._hasParents;
				}
				set
				{
					bool oldValue = this._hasParents;
					if (oldValue != value)
					{
						if (this._Context.OnPersonhasParentsChanging(this, value) && base.OnhasParentsChanging(value))
						{
							this._hasParents = value;
							base.OnhasParentsChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("OptionalUniqueDecimal")]
			private Nullable<decimal> _OptionalUniqueDecimal;
			public override Nullable<decimal> OptionalUniqueDecimal
			{
				get
				{
					return this._OptionalUniqueDecimal;
				}
				set
				{
					Nullable<decimal> oldValue = this._OptionalUniqueDecimal;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnPersonOptionalUniqueDecimalChanging(this, value) && base.OnOptionalUniqueDecimalChanging(value))
						{
							this._OptionalUniqueDecimal = value;
							this._Context.OnPersonOptionalUniqueDecimalChanged(this, oldValue);
							base.OnOptionalUniqueDecimalChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("MandatoryUniqueDecimal")]
			private decimal _MandatoryUniqueDecimal;
			public override decimal MandatoryUniqueDecimal
			{
				get
				{
					return this._MandatoryUniqueDecimal;
				}
				set
				{
					decimal oldValue = this._MandatoryUniqueDecimal;
					if (oldValue != value)
					{
						if (this._Context.OnPersonMandatoryUniqueDecimalChanging(this, value) && base.OnMandatoryUniqueDecimalChanging(value))
						{
							this._MandatoryUniqueDecimal = value;
							this._Context.OnPersonMandatoryUniqueDecimalChanged(this, oldValue);
							base.OnMandatoryUniqueDecimalChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("MandatoryUniqueString")]
			private string _MandatoryUniqueString;
			public override string MandatoryUniqueString
			{
				get
				{
					return this._MandatoryUniqueString;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._MandatoryUniqueString;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnPersonMandatoryUniqueStringChanging(this, value) && base.OnMandatoryUniqueStringChanging(value))
						{
							this._MandatoryUniqueString = value;
							this._Context.OnPersonMandatoryUniqueStringChanged(this, oldValue);
							base.OnMandatoryUniqueStringChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Husband")]
			private Person _Husband;
			public override Person Husband
			{
				get
				{
					return this._Husband;
				}
				set
				{
					Person oldValue = this._Husband;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonHusbandChanging(this, value) && base.OnHusbandChanging(value))
						{
							this._Husband = value;
							this._Context.OnPersonHusbandChanged(this, oldValue);
							base.OnHusbandChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("ValueType1DoesSomethingElseWith")]
			private ValueType1 _ValueType1DoesSomethingElseWith;
			public override ValueType1 ValueType1DoesSomethingElseWith
			{
				get
				{
					return this._ValueType1DoesSomethingElseWith;
				}
				set
				{
					ValueType1 oldValue = this._ValueType1DoesSomethingElseWith;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonValueType1DoesSomethingElseWithChanging(this, value) && base.OnValueType1DoesSomethingElseWithChanging(value))
						{
							this._ValueType1DoesSomethingElseWith = value;
							this._Context.OnPersonValueType1DoesSomethingElseWithChanged(this, oldValue);
							base.OnValueType1DoesSomethingElseWithChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("MalePerson")]
			private MalePerson _MalePerson;
			public override MalePerson MalePerson
			{
				get
				{
					return this._MalePerson;
				}
				set
				{
					MalePerson oldValue = this._MalePerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonMalePersonChanging(this, value) && base.OnMalePersonChanging(value))
						{
							this._MalePerson = value;
							this._Context.OnPersonMalePersonChanged(this, oldValue);
							base.OnMalePersonChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("FemalePerson")]
			private FemalePerson _FemalePerson;
			public override FemalePerson FemalePerson
			{
				get
				{
					return this._FemalePerson;
				}
				set
				{
					FemalePerson oldValue = this._FemalePerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonFemalePersonChanging(this, value) && base.OnFemalePersonChanging(value))
						{
							this._FemalePerson = value;
							this._Context.OnPersonFemalePersonChanged(this, oldValue);
							base.OnFemalePersonChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("ChildPerson")]
			private ChildPerson _ChildPerson;
			public override ChildPerson ChildPerson
			{
				get
				{
					return this._ChildPerson;
				}
				set
				{
					ChildPerson oldValue = this._ChildPerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonChildPersonChanging(this, value) && base.OnChildPersonChanging(value))
						{
							this._ChildPerson = value;
							this._Context.OnPersonChildPersonChanged(this, oldValue);
							base.OnChildPersonChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Death")]
			private Death _Death;
			public override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					Death oldValue = this._Death;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonDeathChanging(this, value) && base.OnDeathChanging(value))
						{
							this._Death = value;
							this._Context.OnPersonDeathChanged(this, oldValue);
							base.OnDeathChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Wife")]
			private Person _Wife;
			public override Person Wife
			{
				get
				{
					return this._Wife;
				}
				set
				{
					Person oldValue = this._Wife;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonWifeChanging(this, value) && base.OnWifeChanging(value))
						{
							this._Wife = value;
							this._Context.OnPersonWifeChanged(this, oldValue);
							base.OnWifeChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("TaskViaPersonCollection")]
			private readonly IEnumerable<Task> _TaskViaPersonCollection;
			public override IEnumerable<Task> TaskViaPersonCollection
			{
				get
				{
					return this._TaskViaPersonCollection;
				}
			}
			[AccessedThroughProperty("ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection")]
			private readonly IEnumerable<ValueType1> _ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			public override IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
			{
				get
				{
					return this._ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
				}
			}
			[AccessedThroughProperty("PersonBoughtCarFromPersonOnDateViaBuyerCollection")]
			private readonly IEnumerable<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			public override IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
			{
				get
				{
					return this._PersonBoughtCarFromPersonOnDateViaBuyerCollection;
				}
			}
			[AccessedThroughProperty("PersonBoughtCarFromPersonOnDateViaSellerCollection")]
			private readonly IEnumerable<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateViaSellerCollection;
			public override IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
			{
				get
				{
					return this._PersonBoughtCarFromPersonOnDateViaSellerCollection;
				}
			}
		}
		#endregion // PersonCore
		#endregion // Person
		#region MalePerson
		public MalePerson CreateMalePerson(Person Person)
		{
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnMalePersonPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new MalePersonCore(this, Person);
		}
		private bool OnMalePersonPersonChanging(MalePerson instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnMalePersonPersonChanged(MalePerson instance, Person oldValue)
		{
			instance.Person.MalePerson = instance;
			if ((object)oldValue != null)
			{
				oldValue.MalePerson = null;
			}
		}
		private bool OnMalePersonChildPersonViaFatherCollectionAdding(MalePerson instance, ChildPerson value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnMalePersonChildPersonViaFatherCollectionAdded(MalePerson instance, ChildPerson value)
		{
			value.Father = instance;
		}
		private void OnMalePersonChildPersonViaFatherCollectionRemoved(MalePerson instance, ChildPerson value)
		{
			if ((object)value.Father == instance)
			{
				value.Father = null;
			}
		}
		private readonly List<MalePerson> _MalePersonList;
		private readonly ReadOnlyCollection<MalePerson> _MalePersonReadOnlyCollection;
		public IEnumerable<MalePerson> MalePersonCollection
		{
			get
			{
				return this._MalePersonReadOnlyCollection;
			}
		}
		#region MalePersonCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class MalePersonCore : MalePerson
		{
			public MalePersonCore(SampleModelContext context, Person Person)
			{
				this._Context = context;
				this._ChildPersonViaFatherCollection = new ConstraintEnforcementCollection<MalePerson, ChildPerson>(this);
				this._Person = Person;
				context.OnMalePersonPersonChanged(this, null);
				context._MalePersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnMalePersonPersonChanging(this, value) && base.OnPersonChanging(value))
						{
							this._Person = value;
							this._Context.OnMalePersonPersonChanged(this, oldValue);
							base.OnPersonChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("ChildPersonViaFatherCollection")]
			private readonly IEnumerable<ChildPerson> _ChildPersonViaFatherCollection;
			public override IEnumerable<ChildPerson> ChildPersonViaFatherCollection
			{
				get
				{
					return this._ChildPersonViaFatherCollection;
				}
			}
		}
		#endregion // MalePersonCore
		#endregion // MalePerson
		#region FemalePerson
		public FemalePerson CreateFemalePerson(Person Person)
		{
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnFemalePersonPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new FemalePersonCore(this, Person);
		}
		private bool OnFemalePersonPersonChanging(FemalePerson instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnFemalePersonPersonChanged(FemalePerson instance, Person oldValue)
		{
			instance.Person.FemalePerson = instance;
			if ((object)oldValue != null)
			{
				oldValue.FemalePerson = null;
			}
		}
		private bool OnFemalePersonChildPersonViaMotherCollectionAdding(FemalePerson instance, ChildPerson value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnFemalePersonChildPersonViaMotherCollectionAdded(FemalePerson instance, ChildPerson value)
		{
			value.Mother = instance;
		}
		private void OnFemalePersonChildPersonViaMotherCollectionRemoved(FemalePerson instance, ChildPerson value)
		{
			if ((object)value.Mother == instance)
			{
				value.Mother = null;
			}
		}
		private readonly List<FemalePerson> _FemalePersonList;
		private readonly ReadOnlyCollection<FemalePerson> _FemalePersonReadOnlyCollection;
		public IEnumerable<FemalePerson> FemalePersonCollection
		{
			get
			{
				return this._FemalePersonReadOnlyCollection;
			}
		}
		#region FemalePersonCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class FemalePersonCore : FemalePerson
		{
			public FemalePersonCore(SampleModelContext context, Person Person)
			{
				this._Context = context;
				this._ChildPersonViaMotherCollection = new ConstraintEnforcementCollection<FemalePerson, ChildPerson>(this);
				this._Person = Person;
				context.OnFemalePersonPersonChanged(this, null);
				context._FemalePersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnFemalePersonPersonChanging(this, value) && base.OnPersonChanging(value))
						{
							this._Person = value;
							this._Context.OnFemalePersonPersonChanged(this, oldValue);
							base.OnPersonChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("ChildPersonViaMotherCollection")]
			private readonly IEnumerable<ChildPerson> _ChildPersonViaMotherCollection;
			public override IEnumerable<ChildPerson> ChildPersonViaMotherCollection
			{
				get
				{
					return this._ChildPersonViaMotherCollection;
				}
			}
		}
		#endregion // FemalePersonCore
		#endregion // FemalePerson
		#region ChildPerson
		public ChildPerson CreateChildPerson(int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person)
		{
			if ((object)Father == null)
			{
				throw new ArgumentNullException("Father");
			}
			if ((object)Mother == null)
			{
				throw new ArgumentNullException("Mother");
			}
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnChildPersonBirthOrder_BirthOrder_NrChanging(null, BirthOrder_BirthOrder_Nr)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("BirthOrder_BirthOrder_Nr");
			}
			if (!(this.OnChildPersonFatherChanging(null, Father)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Father");
			}
			if (!(this.OnChildPersonMotherChanging(null, Mother)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Mother");
			}
			if (!(this.OnChildPersonPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new ChildPersonCore(this, BirthOrder_BirthOrder_Nr, Father, Mother, Person);
		}
		private bool OnChildPersonBirthOrder_BirthOrder_NrChanging(ChildPerson instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint49Changing(instance, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, newValue, instance.Mother))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonBirthOrder_BirthOrder_NrChanged(ChildPerson instance, Nullable<int> oldValue)
		{
			Tuple<MalePerson, int, FemalePerson> InternalUniquenessConstraint49OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint49OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, oldValue.Value, instance.Mother);
			}
			else
			{
				InternalUniquenessConstraint49OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint49Changed(instance, InternalUniquenessConstraint49OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
		}
		private bool OnChildPersonFatherChanging(ChildPerson instance, MalePerson newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint49Changing(instance, Tuple.CreateTuple<MalePerson, int, FemalePerson>(newValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonFatherChanged(ChildPerson instance, MalePerson oldValue)
		{
			((ICollection<ChildPerson>)instance.Father.ChildPersonViaFatherCollection).Add(instance);
			Tuple<MalePerson, int, FemalePerson> InternalUniquenessConstraint49OldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<ChildPerson>)oldValue.ChildPersonViaFatherCollection).Remove(instance);
				InternalUniquenessConstraint49OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(oldValue, instance.BirthOrder_BirthOrder_Nr, instance.Mother);
			}
			else
			{
				InternalUniquenessConstraint49OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint49Changed(instance, InternalUniquenessConstraint49OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
		}
		private bool OnChildPersonMotherChanging(ChildPerson instance, FemalePerson newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint49Changing(instance, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonMotherChanged(ChildPerson instance, FemalePerson oldValue)
		{
			((ICollection<ChildPerson>)instance.Mother.ChildPersonViaMotherCollection).Add(instance);
			Tuple<MalePerson, int, FemalePerson> InternalUniquenessConstraint49OldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<ChildPerson>)oldValue.ChildPersonViaMotherCollection).Remove(instance);
				InternalUniquenessConstraint49OldValueTuple = Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, oldValue);
			}
			else
			{
				InternalUniquenessConstraint49OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint49Changed(instance, InternalUniquenessConstraint49OldValueTuple, Tuple.CreateTuple<MalePerson, int, FemalePerson>(instance.Father, instance.BirthOrder_BirthOrder_Nr, instance.Mother));
		}
		private bool OnChildPersonPersonChanging(ChildPerson instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnChildPersonPersonChanged(ChildPerson instance, Person oldValue)
		{
			instance.Person.ChildPerson = instance;
			if ((object)oldValue != null)
			{
				oldValue.ChildPerson = null;
			}
		}
		private readonly List<ChildPerson> _ChildPersonList;
		private readonly ReadOnlyCollection<ChildPerson> _ChildPersonReadOnlyCollection;
		public IEnumerable<ChildPerson> ChildPersonCollection
		{
			get
			{
				return this._ChildPersonReadOnlyCollection;
			}
		}
		#region ChildPersonCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ChildPersonCore : ChildPerson
		{
			public ChildPersonCore(SampleModelContext context, int BirthOrder_BirthOrder_Nr, MalePerson Father, FemalePerson Mother, Person Person)
			{
				this._Context = context;
				this._BirthOrder_BirthOrder_Nr = BirthOrder_BirthOrder_Nr;
				context.OnChildPersonBirthOrder_BirthOrder_NrChanged(this, null);
				this._Father = Father;
				context.OnChildPersonFatherChanged(this, null);
				this._Mother = Mother;
				context.OnChildPersonMotherChanged(this, null);
				this._Person = Person;
				context.OnChildPersonPersonChanged(this, null);
				context._ChildPersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("BirthOrder_BirthOrder_Nr")]
			private int _BirthOrder_BirthOrder_Nr;
			public override int BirthOrder_BirthOrder_Nr
			{
				get
				{
					return this._BirthOrder_BirthOrder_Nr;
				}
				set
				{
					int oldValue = this._BirthOrder_BirthOrder_Nr;
					if (oldValue != value)
					{
						if (this._Context.OnChildPersonBirthOrder_BirthOrder_NrChanging(this, value) && base.OnBirthOrder_BirthOrder_NrChanging(value))
						{
							this._BirthOrder_BirthOrder_Nr = value;
							this._Context.OnChildPersonBirthOrder_BirthOrder_NrChanged(this, oldValue);
							base.OnBirthOrder_BirthOrder_NrChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Father")]
			private MalePerson _Father;
			public override MalePerson Father
			{
				get
				{
					return this._Father;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					MalePerson oldValue = this._Father;
					if ((object)oldValue != value)
					{
						if (this._Context.OnChildPersonFatherChanging(this, value) && base.OnFatherChanging(value))
						{
							this._Father = value;
							this._Context.OnChildPersonFatherChanged(this, oldValue);
							base.OnFatherChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Mother")]
			private FemalePerson _Mother;
			public override FemalePerson Mother
			{
				get
				{
					return this._Mother;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					FemalePerson oldValue = this._Mother;
					if ((object)oldValue != value)
					{
						if (this._Context.OnChildPersonMotherChanging(this, value) && base.OnMotherChanging(value))
						{
							this._Mother = value;
							this._Context.OnChildPersonMotherChanged(this, oldValue);
							base.OnMotherChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnChildPersonPersonChanging(this, value) && base.OnPersonChanging(value))
						{
							this._Person = value;
							this._Context.OnChildPersonPersonChanged(this, oldValue);
							base.OnPersonChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // ChildPersonCore
		#endregion // ChildPerson
		#region Death
		public Death CreateDeath(string DeathCause_DeathCause_Type, Person Person)
		{
			if ((object)DeathCause_DeathCause_Type == null)
			{
				throw new ArgumentNullException("DeathCause_DeathCause_Type");
			}
			if ((object)Person == null)
			{
				throw new ArgumentNullException("Person");
			}
			if (!(this.OnDeathDeathCause_DeathCause_TypeChanging(null, DeathCause_DeathCause_Type)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("DeathCause_DeathCause_Type");
			}
			if (!(this.OnDeathPersonChanging(null, Person)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Person");
			}
			return new DeathCore(this, DeathCause_DeathCause_Type, Person);
		}
		private bool OnDeathDate_YMDChanging(Death instance, Nullable<int> newValue)
		{
			return true;
		}
		private bool OnDeathDeathCause_DeathCause_TypeChanging(Death instance, string newValue)
		{
			return true;
		}
		private bool OnDeathNaturalDeathChanging(Death instance, NaturalDeath newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnDeathNaturalDeathChanged(Death instance, NaturalDeath oldValue)
		{
			if ((object)instance.NaturalDeath != null)
			{
				instance.NaturalDeath.Death = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Death = null;
			}
		}
		private bool OnDeathUnnaturalDeathChanging(Death instance, UnnaturalDeath newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnDeathUnnaturalDeathChanged(Death instance, UnnaturalDeath oldValue)
		{
			if ((object)instance.UnnaturalDeath != null)
			{
				instance.UnnaturalDeath.Death = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Death = null;
			}
		}
		private bool OnDeathPersonChanging(Death instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnDeathPersonChanged(Death instance, Person oldValue)
		{
			instance.Person.Death = instance;
			if ((object)oldValue != null)
			{
				oldValue.Death = null;
			}
		}
		private readonly List<Death> _DeathList;
		private readonly ReadOnlyCollection<Death> _DeathReadOnlyCollection;
		public IEnumerable<Death> DeathCollection
		{
			get
			{
				return this._DeathReadOnlyCollection;
			}
		}
		#region DeathCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class DeathCore : Death
		{
			public DeathCore(SampleModelContext context, string DeathCause_DeathCause_Type, Person Person)
			{
				this._Context = context;
				this._DeathCause_DeathCause_Type = DeathCause_DeathCause_Type;
				this._Person = Person;
				context.OnDeathPersonChanged(this, null);
				context._DeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("Date_YMD")]
			private Nullable<int> _Date_YMD;
			public override Nullable<int> Date_YMD
			{
				get
				{
					return this._Date_YMD;
				}
				set
				{
					Nullable<int> oldValue = this._Date_YMD;
					if ((oldValue.GetValueOrDefault() != value.GetValueOrDefault()) || (oldValue.HasValue != value.HasValue))
					{
						if (this._Context.OnDeathDate_YMDChanging(this, value) && base.OnDate_YMDChanging(value))
						{
							this._Date_YMD = value;
							base.OnDate_YMDChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("DeathCause_DeathCause_Type")]
			private string _DeathCause_DeathCause_Type;
			public override string DeathCause_DeathCause_Type
			{
				get
				{
					return this._DeathCause_DeathCause_Type;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._DeathCause_DeathCause_Type;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnDeathDeathCause_DeathCause_TypeChanging(this, value) && base.OnDeathCause_DeathCause_TypeChanging(value))
						{
							this._DeathCause_DeathCause_Type = value;
							base.OnDeathCause_DeathCause_TypeChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("NaturalDeath")]
			private NaturalDeath _NaturalDeath;
			public override NaturalDeath NaturalDeath
			{
				get
				{
					return this._NaturalDeath;
				}
				set
				{
					NaturalDeath oldValue = this._NaturalDeath;
					if ((object)oldValue != value)
					{
						if (this._Context.OnDeathNaturalDeathChanging(this, value) && base.OnNaturalDeathChanging(value))
						{
							this._NaturalDeath = value;
							this._Context.OnDeathNaturalDeathChanged(this, oldValue);
							base.OnNaturalDeathChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("UnnaturalDeath")]
			private UnnaturalDeath _UnnaturalDeath;
			public override UnnaturalDeath UnnaturalDeath
			{
				get
				{
					return this._UnnaturalDeath;
				}
				set
				{
					UnnaturalDeath oldValue = this._UnnaturalDeath;
					if ((object)oldValue != value)
					{
						if (this._Context.OnDeathUnnaturalDeathChanging(this, value) && base.OnUnnaturalDeathChanging(value))
						{
							this._UnnaturalDeath = value;
							this._Context.OnDeathUnnaturalDeathChanged(this, oldValue);
							base.OnUnnaturalDeathChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnDeathPersonChanging(this, value) && base.OnPersonChanging(value))
						{
							this._Person = value;
							this._Context.OnDeathPersonChanged(this, oldValue);
							base.OnPersonChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // DeathCore
		#endregion // Death
		#region NaturalDeath
		public NaturalDeath CreateNaturalDeath(bool isFromProstateCancer, Death Death)
		{
			if ((object)Death == null)
			{
				throw new ArgumentNullException("Death");
			}
			if (!(this.OnNaturalDeathisFromProstateCancerChanging(null, isFromProstateCancer)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("isFromProstateCancer");
			}
			if (!(this.OnNaturalDeathDeathChanging(null, Death)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Death");
			}
			return new NaturalDeathCore(this, isFromProstateCancer, Death);
		}
		private bool OnNaturalDeathisFromProstateCancerChanging(NaturalDeath instance, bool newValue)
		{
			return true;
		}
		private bool OnNaturalDeathDeathChanging(NaturalDeath instance, Death newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnNaturalDeathDeathChanged(NaturalDeath instance, Death oldValue)
		{
			instance.Death.NaturalDeath = instance;
			if ((object)oldValue != null)
			{
				oldValue.NaturalDeath = null;
			}
		}
		private readonly List<NaturalDeath> _NaturalDeathList;
		private readonly ReadOnlyCollection<NaturalDeath> _NaturalDeathReadOnlyCollection;
		public IEnumerable<NaturalDeath> NaturalDeathCollection
		{
			get
			{
				return this._NaturalDeathReadOnlyCollection;
			}
		}
		#region NaturalDeathCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class NaturalDeathCore : NaturalDeath
		{
			public NaturalDeathCore(SampleModelContext context, bool isFromProstateCancer, Death Death)
			{
				this._Context = context;
				this._isFromProstateCancer = isFromProstateCancer;
				this._Death = Death;
				context.OnNaturalDeathDeathChanged(this, null);
				context._NaturalDeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("isFromProstateCancer")]
			private bool _isFromProstateCancer;
			public override bool isFromProstateCancer
			{
				get
				{
					return this._isFromProstateCancer;
				}
				set
				{
					bool oldValue = this._isFromProstateCancer;
					if (oldValue != value)
					{
						if (this._Context.OnNaturalDeathisFromProstateCancerChanging(this, value) && base.OnisFromProstateCancerChanging(value))
						{
							this._isFromProstateCancer = value;
							base.OnisFromProstateCancerChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Death")]
			private Death _Death;
			public override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Death oldValue = this._Death;
					if ((object)oldValue != value)
					{
						if (this._Context.OnNaturalDeathDeathChanging(this, value) && base.OnDeathChanging(value))
						{
							this._Death = value;
							this._Context.OnNaturalDeathDeathChanged(this, oldValue);
							base.OnDeathChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // NaturalDeathCore
		#endregion // NaturalDeath
		#region UnnaturalDeath
		public UnnaturalDeath CreateUnnaturalDeath(bool isViolent, bool isBloody, Death Death)
		{
			if ((object)Death == null)
			{
				throw new ArgumentNullException("Death");
			}
			if (!(this.OnUnnaturalDeathisViolentChanging(null, isViolent)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("isViolent");
			}
			if (!(this.OnUnnaturalDeathisBloodyChanging(null, isBloody)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("isBloody");
			}
			if (!(this.OnUnnaturalDeathDeathChanging(null, Death)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Death");
			}
			return new UnnaturalDeathCore(this, isViolent, isBloody, Death);
		}
		private bool OnUnnaturalDeathisViolentChanging(UnnaturalDeath instance, bool newValue)
		{
			return true;
		}
		private bool OnUnnaturalDeathisBloodyChanging(UnnaturalDeath instance, bool newValue)
		{
			return true;
		}
		private bool OnUnnaturalDeathDeathChanging(UnnaturalDeath instance, Death newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnUnnaturalDeathDeathChanged(UnnaturalDeath instance, Death oldValue)
		{
			instance.Death.UnnaturalDeath = instance;
			if ((object)oldValue != null)
			{
				oldValue.UnnaturalDeath = null;
			}
		}
		private readonly List<UnnaturalDeath> _UnnaturalDeathList;
		private readonly ReadOnlyCollection<UnnaturalDeath> _UnnaturalDeathReadOnlyCollection;
		public IEnumerable<UnnaturalDeath> UnnaturalDeathCollection
		{
			get
			{
				return this._UnnaturalDeathReadOnlyCollection;
			}
		}
		#region UnnaturalDeathCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class UnnaturalDeathCore : UnnaturalDeath
		{
			public UnnaturalDeathCore(SampleModelContext context, bool isViolent, bool isBloody, Death Death)
			{
				this._Context = context;
				this._isViolent = isViolent;
				this._isBloody = isBloody;
				this._Death = Death;
				context.OnUnnaturalDeathDeathChanged(this, null);
				context._UnnaturalDeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("isViolent")]
			private bool _isViolent;
			public override bool isViolent
			{
				get
				{
					return this._isViolent;
				}
				set
				{
					bool oldValue = this._isViolent;
					if (oldValue != value)
					{
						if (this._Context.OnUnnaturalDeathisViolentChanging(this, value) && base.OnisViolentChanging(value))
						{
							this._isViolent = value;
							base.OnisViolentChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("isBloody")]
			private bool _isBloody;
			public override bool isBloody
			{
				get
				{
					return this._isBloody;
				}
				set
				{
					bool oldValue = this._isBloody;
					if (oldValue != value)
					{
						if (this._Context.OnUnnaturalDeathisBloodyChanging(this, value) && base.OnisBloodyChanging(value))
						{
							this._isBloody = value;
							base.OnisBloodyChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Death")]
			private Death _Death;
			public override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Death oldValue = this._Death;
					if ((object)oldValue != value)
					{
						if (this._Context.OnUnnaturalDeathDeathChanging(this, value) && base.OnDeathChanging(value))
						{
							this._Death = value;
							this._Context.OnUnnaturalDeathDeathChanged(this, oldValue);
							base.OnDeathChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // UnnaturalDeathCore
		#endregion // UnnaturalDeath
		#region Task
		public Task CreateTask()
		{
			return new TaskCore(this);
		}
		private bool OnTaskPersonChanging(Task instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnTaskPersonChanged(Task instance, Person oldValue)
		{
			if ((object)instance.Person != null)
			{
				((ICollection<Task>)instance.Person.TaskViaPersonCollection).Add(instance);
			}
			if ((object)oldValue != null)
			{
				((ICollection<Task>)oldValue.TaskViaPersonCollection).Remove(instance);
			}
		}
		private readonly List<Task> _TaskList;
		private readonly ReadOnlyCollection<Task> _TaskReadOnlyCollection;
		public IEnumerable<Task> TaskCollection
		{
			get
			{
				return this._TaskReadOnlyCollection;
			}
		}
		#region TaskCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class TaskCore : Task
		{
			public TaskCore(SampleModelContext context)
			{
				this._Context = context;
				context._TaskList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("Person")]
			private Person _Person;
			public override Person Person
			{
				get
				{
					return this._Person;
				}
				set
				{
					Person oldValue = this._Person;
					if ((object)oldValue != value)
					{
						if (this._Context.OnTaskPersonChanging(this, value) && base.OnPersonChanging(value))
						{
							this._Person = value;
							this._Context.OnTaskPersonChanged(this, oldValue);
							base.OnPersonChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // TaskCore
		#endregion // Task
		#region ValueType1
		public ValueType1 CreateValueType1(int ValueType1Value)
		{
			if (!(this.OnValueType1ValueType1ValueChanging(null, ValueType1Value)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("ValueType1Value");
			}
			return new ValueType1Core(this, ValueType1Value);
		}
		private bool OnValueType1ValueType1ValueChanging(ValueType1 instance, int newValue)
		{
			ValueType1 currentInstance;
			if (this._ValueType1ValueType1ValueDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnValueType1ValueType1ValueChanged(ValueType1 instance, Nullable<int> oldValue)
		{
			this._ValueType1ValueType1ValueDictionary.Add(instance.ValueType1Value, instance);
			if (oldValue.HasValue)
			{
				this._ValueType1ValueType1ValueDictionary.Remove(oldValue.Value);
			}
		}
		private bool OnValueType1DoesSomethingWithPersonChanging(ValueType1 instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnValueType1DoesSomethingWithPersonChanged(ValueType1 instance, Person oldValue)
		{
			if ((object)instance.DoesSomethingWithPerson != null)
			{
				((ICollection<ValueType1>)instance.DoesSomethingWithPerson.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection).Add(instance);
			}
			if ((object)oldValue != null)
			{
				((ICollection<ValueType1>)oldValue.ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection).Remove(instance);
			}
		}
		private bool OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdding(ValueType1 instance, Person value)
		{
			if ((object)this != value.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdded(ValueType1 instance, Person value)
		{
			value.ValueType1DoesSomethingElseWith = instance;
		}
		private void OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionRemoved(ValueType1 instance, Person value)
		{
			if ((object)value.ValueType1DoesSomethingElseWith == instance)
			{
				value.ValueType1DoesSomethingElseWith = null;
			}
		}
		private readonly List<ValueType1> _ValueType1List;
		private readonly ReadOnlyCollection<ValueType1> _ValueType1ReadOnlyCollection;
		public IEnumerable<ValueType1> ValueType1Collection
		{
			get
			{
				return this._ValueType1ReadOnlyCollection;
			}
		}
		#region ValueType1Core
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ValueType1Core : ValueType1
		{
			public ValueType1Core(SampleModelContext context, int ValueType1Value)
			{
				this._Context = context;
				this._DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection = new ConstraintEnforcementCollection<ValueType1, Person>(this);
				this._ValueType1Value = ValueType1Value;
				context.OnValueType1ValueType1ValueChanged(this, null);
				context._ValueType1List.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("ValueType1Value")]
			private int _ValueType1Value;
			public override int ValueType1Value
			{
				get
				{
					return this._ValueType1Value;
				}
				set
				{
					int oldValue = this._ValueType1Value;
					if (oldValue != value)
					{
						if (this._Context.OnValueType1ValueType1ValueChanging(this, value) && base.OnValueType1ValueChanging(value))
						{
							this._ValueType1Value = value;
							this._Context.OnValueType1ValueType1ValueChanged(this, oldValue);
							base.OnValueType1ValueChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("DoesSomethingWithPerson")]
			private Person _DoesSomethingWithPerson;
			public override Person DoesSomethingWithPerson
			{
				get
				{
					return this._DoesSomethingWithPerson;
				}
				set
				{
					Person oldValue = this._DoesSomethingWithPerson;
					if ((object)oldValue != value)
					{
						if (this._Context.OnValueType1DoesSomethingWithPersonChanging(this, value) && base.OnDoesSomethingWithPersonChanging(value))
						{
							this._DoesSomethingWithPerson = value;
							this._Context.OnValueType1DoesSomethingWithPersonChanged(this, oldValue);
							base.OnDoesSomethingWithPersonChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection")]
			private readonly IEnumerable<Person> _DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection;
			public override IEnumerable<Person> DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection
			{
				get
				{
					return this._DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection;
				}
			}
		}
		#endregion // ValueType1Core
		#endregion // ValueType1
		#region PersonBoughtCarFromPersonOnDate
		public PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller)
		{
			if ((object)Buyer == null)
			{
				throw new ArgumentNullException("Buyer");
			}
			if ((object)Seller == null)
			{
				throw new ArgumentNullException("Seller");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(null, CarSold_vin)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("CarSold_vin");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(null, SaleDate_YMD)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("SaleDate_YMD");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateBuyerChanging(null, Buyer)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Buyer");
			}
			if (!(this.OnPersonBoughtCarFromPersonOnDateSellerChanging(null, Seller)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Seller");
			}
			return new PersonBoughtCarFromPersonOnDateCore(this, CarSold_vin, SaleDate_YMD, Buyer, Seller);
		}
		private bool OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(PersonBoughtCarFromPersonOnDate instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, newValue, instance.Seller))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, newValue))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple<int, int, Person>(newValue, instance.SaleDate_YMD, instance.Buyer))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(PersonBoughtCarFromPersonOnDate instance, Nullable<int> oldValue)
		{
			Tuple<Person, int, Person> InternalUniquenessConstraint23OldValueTuple;
			Tuple<int, Person, int> InternalUniquenessConstraint24OldValueTuple;
			Tuple<int, int, Person> InternalUniquenessConstraint25OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple<Person, int, Person>(instance.Buyer, oldValue.Value, instance.Seller);
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, oldValue.Value);
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple<int, int, Person>(oldValue.Value, instance.SaleDate_YMD, instance.Buyer);
			}
			else
			{
				InternalUniquenessConstraint23OldValueTuple = null;
				InternalUniquenessConstraint24OldValueTuple = null;
				InternalUniquenessConstraint25OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, instance.Seller));
			this.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
			this.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(PersonBoughtCarFromPersonOnDate instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple<int, Person, int>(newValue, instance.Seller, instance.CarSold_vin))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, newValue, instance.Buyer))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(PersonBoughtCarFromPersonOnDate instance, Nullable<int> oldValue)
		{
			Tuple<int, Person, int> InternalUniquenessConstraint24OldValueTuple;
			Tuple<int, int, Person> InternalUniquenessConstraint25OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple<int, Person, int>(oldValue.Value, instance.Seller, instance.CarSold_vin);
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, oldValue.Value, instance.Buyer);
			}
			else
			{
				InternalUniquenessConstraint24OldValueTuple = null;
				InternalUniquenessConstraint25OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
			this.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateBuyerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple<Person, int, Person>(newValue, instance.CarSold_vin, instance.Seller))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint25Changing(instance, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateBuyerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
		{
			((ICollection<PersonBoughtCarFromPersonOnDate>)instance.Buyer.PersonBoughtCarFromPersonOnDateViaBuyerCollection).Add(instance);
			Tuple<Person, int, Person> InternalUniquenessConstraint23OldValueTuple;
			Tuple<int, int, Person> InternalUniquenessConstraint25OldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<PersonBoughtCarFromPersonOnDate>)oldValue.PersonBoughtCarFromPersonOnDateViaBuyerCollection).Remove(instance);
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple<Person, int, Person>(oldValue, instance.CarSold_vin, instance.Seller);
				InternalUniquenessConstraint25OldValueTuple = Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, oldValue);
			}
			else
			{
				InternalUniquenessConstraint23OldValueTuple = null;
				InternalUniquenessConstraint25OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, instance.Seller));
			this.OnInternalUniquenessConstraint25Changed(instance, InternalUniquenessConstraint25OldValueTuple, Tuple.CreateTuple<int, int, Person>(instance.CarSold_vin, instance.SaleDate_YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateSellerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
		{
			if ((object)this != newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint23Changing(instance, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, newValue))))
				{
					return false;
				}
				if (!(this.OnInternalUniquenessConstraint24Changing(instance, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, newValue, instance.CarSold_vin))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateSellerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
		{
			((ICollection<PersonBoughtCarFromPersonOnDate>)instance.Seller.PersonBoughtCarFromPersonOnDateViaSellerCollection).Add(instance);
			Tuple<Person, int, Person> InternalUniquenessConstraint23OldValueTuple;
			Tuple<int, Person, int> InternalUniquenessConstraint24OldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<PersonBoughtCarFromPersonOnDate>)oldValue.PersonBoughtCarFromPersonOnDateViaSellerCollection).Remove(instance);
				InternalUniquenessConstraint23OldValueTuple = Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, oldValue);
				InternalUniquenessConstraint24OldValueTuple = Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, oldValue, instance.CarSold_vin);
			}
			else
			{
				InternalUniquenessConstraint23OldValueTuple = null;
				InternalUniquenessConstraint24OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint23Changed(instance, InternalUniquenessConstraint23OldValueTuple, Tuple.CreateTuple<Person, int, Person>(instance.Buyer, instance.CarSold_vin, instance.Seller));
			this.OnInternalUniquenessConstraint24Changed(instance, InternalUniquenessConstraint24OldValueTuple, Tuple.CreateTuple<int, Person, int>(instance.SaleDate_YMD, instance.Seller, instance.CarSold_vin));
		}
		private readonly List<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateList;
		private readonly ReadOnlyCollection<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateReadOnlyCollection;
		public IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateCollection
		{
			get
			{
				return this._PersonBoughtCarFromPersonOnDateReadOnlyCollection;
			}
		}
		#region PersonBoughtCarFromPersonOnDateCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class PersonBoughtCarFromPersonOnDateCore : PersonBoughtCarFromPersonOnDate
		{
			public PersonBoughtCarFromPersonOnDateCore(SampleModelContext context, int CarSold_vin, int SaleDate_YMD, Person Buyer, Person Seller)
			{
				this._Context = context;
				this._CarSold_vin = CarSold_vin;
				context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(this, null);
				this._SaleDate_YMD = SaleDate_YMD;
				context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(this, null);
				this._Buyer = Buyer;
				context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, null);
				this._Seller = Seller;
				context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, null);
				context._PersonBoughtCarFromPersonOnDateList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("CarSold_vin")]
			private int _CarSold_vin;
			public override int CarSold_vin
			{
				get
				{
					return this._CarSold_vin;
				}
				set
				{
					int oldValue = this._CarSold_vin;
					if (oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanging(this, value) && base.OnCarSold_vinChanging(value))
						{
							this._CarSold_vin = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateCarSold_vinChanged(this, oldValue);
							base.OnCarSold_vinChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("SaleDate_YMD")]
			private int _SaleDate_YMD;
			public override int SaleDate_YMD
			{
				get
				{
					return this._SaleDate_YMD;
				}
				set
				{
					int oldValue = this._SaleDate_YMD;
					if (oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanging(this, value) && base.OnSaleDate_YMDChanging(value))
						{
							this._SaleDate_YMD = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateSaleDate_YMDChanged(this, oldValue);
							base.OnSaleDate_YMDChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Buyer")]
			private Person _Buyer;
			public override Person Buyer
			{
				get
				{
					return this._Buyer;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Buyer;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateBuyerChanging(this, value) && base.OnBuyerChanging(value))
						{
							this._Buyer = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, oldValue);
							base.OnBuyerChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Seller")]
			private Person _Seller;
			public override Person Seller
			{
				get
				{
					return this._Seller;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._Seller;
					if ((object)oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateSellerChanging(this, value) && base.OnSellerChanging(value))
						{
							this._Seller = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, oldValue);
							base.OnSellerChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonBoughtCarFromPersonOnDateCore
		#endregion // PersonBoughtCarFromPersonOnDate
		#region Review
		public Review CreateReview(int Car_vin, int Rating_Nr_Integer, string Criterion_Name)
		{
			if ((object)Criterion_Name == null)
			{
				throw new ArgumentNullException("Criterion_Name");
			}
			if (!(this.OnReviewCar_vinChanging(null, Car_vin)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Car_vin");
			}
			if (!(this.OnReviewRating_Nr_IntegerChanging(null, Rating_Nr_Integer)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Rating_Nr_Integer");
			}
			if (!(this.OnReviewCriterion_NameChanging(null, Criterion_Name)))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("Criterion_Name");
			}
			return new ReviewCore(this, Car_vin, Rating_Nr_Integer, Criterion_Name);
		}
		private bool OnReviewCar_vinChanging(Review instance, int newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple<int, string>(newValue, instance.Criterion_Name))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnReviewCar_vinChanged(Review instance, Nullable<int> oldValue)
		{
			Tuple<int, string> InternalUniquenessConstraint26OldValueTuple;
			if (oldValue.HasValue)
			{
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple<int, string>(oldValue.Value, instance.Criterion_Name);
			}
			else
			{
				InternalUniquenessConstraint26OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple<int, string>(instance.Car_vin, instance.Criterion_Name));
		}
		private bool OnReviewRating_Nr_IntegerChanging(Review instance, int newValue)
		{
			return true;
		}
		private bool OnReviewCriterion_NameChanging(Review instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!(this.OnInternalUniquenessConstraint26Changing(instance, Tuple.CreateTuple<int, string>(instance.Car_vin, newValue))))
				{
					return false;
				}
			}
			return true;
		}
		private void OnReviewCriterion_NameChanged(Review instance, string oldValue)
		{
			Tuple<int, string> InternalUniquenessConstraint26OldValueTuple;
			if ((object)oldValue != null)
			{
				InternalUniquenessConstraint26OldValueTuple = Tuple.CreateTuple<int, string>(instance.Car_vin, oldValue);
			}
			else
			{
				InternalUniquenessConstraint26OldValueTuple = null;
			}
			this.OnInternalUniquenessConstraint26Changed(instance, InternalUniquenessConstraint26OldValueTuple, Tuple.CreateTuple<int, string>(instance.Car_vin, instance.Criterion_Name));
		}
		private readonly List<Review> _ReviewList;
		private readonly ReadOnlyCollection<Review> _ReviewReadOnlyCollection;
		public IEnumerable<Review> ReviewCollection
		{
			get
			{
				return this._ReviewReadOnlyCollection;
			}
		}
		#region ReviewCore
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ReviewCore : Review
		{
			public ReviewCore(SampleModelContext context, int Car_vin, int Rating_Nr_Integer, string Criterion_Name)
			{
				this._Context = context;
				this._Car_vin = Car_vin;
				context.OnReviewCar_vinChanged(this, null);
				this._Rating_Nr_Integer = Rating_Nr_Integer;
				this._Criterion_Name = Criterion_Name;
				context.OnReviewCriterion_NameChanged(this, null);
				context._ReviewList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			[AccessedThroughProperty("Car_vin")]
			private int _Car_vin;
			public override int Car_vin
			{
				get
				{
					return this._Car_vin;
				}
				set
				{
					int oldValue = this._Car_vin;
					if (oldValue != value)
					{
						if (this._Context.OnReviewCar_vinChanging(this, value) && base.OnCar_vinChanging(value))
						{
							this._Car_vin = value;
							this._Context.OnReviewCar_vinChanged(this, oldValue);
							base.OnCar_vinChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Rating_Nr_Integer")]
			private int _Rating_Nr_Integer;
			public override int Rating_Nr_Integer
			{
				get
				{
					return this._Rating_Nr_Integer;
				}
				set
				{
					int oldValue = this._Rating_Nr_Integer;
					if (oldValue != value)
					{
						if (this._Context.OnReviewRating_Nr_IntegerChanging(this, value) && base.OnRating_Nr_IntegerChanging(value))
						{
							this._Rating_Nr_Integer = value;
							base.OnRating_Nr_IntegerChanged(oldValue);
						}
					}
				}
			}
			[AccessedThroughProperty("Criterion_Name")]
			private string _Criterion_Name;
			public override string Criterion_Name
			{
				get
				{
					return this._Criterion_Name;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Criterion_Name;
					if (!(object.Equals(oldValue, value)))
					{
						if (this._Context.OnReviewCriterion_NameChanging(this, value) && base.OnCriterion_NameChanging(value))
						{
							this._Criterion_Name = value;
							this._Context.OnReviewCriterion_NameChanged(this, oldValue);
							base.OnCriterion_NameChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // ReviewCore
		#endregion // Review
	}
	#endregion // SampleModelContext
}

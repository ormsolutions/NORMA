using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace SampleModel
{
	#region SampleModelContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public sealed class SampleModelContext : ISampleModelContext
	{
		public SampleModelContext()
		{
			Dictionary<RuntimeTypeHandle, object> constraintEnforcementCollectionCallbacksByTypeDictionary = this._ContraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<RuntimeTypeHandle, object>(7, RuntimeTypeHandleEqualityComparer.Instance);
			Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> constraintEnforcementCollectionCallbacksByTypeAndNameDictionary = this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary = new Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object>(2);
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, Task>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<Person, Task>(new PotentialCollectionModificationCallback<Person, Task>(this.OnPersonTaskViaPersonCollectionAdding), new CommittedCollectionModificationCallback<Person, Task>(this.OnPersonTaskViaPersonCollectionAdded), null, new CommittedCollectionModificationCallback<Person, Task>(this.OnPersonTaskViaPersonCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, ValueType1>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<Person, ValueType1>(new PotentialCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdding), new CommittedCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdded), null, new CommittedCollectionModificationCallback<Person, ValueType1>(this.OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, PersonDrivesCar>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<Person, PersonDrivesCar>(new PotentialCollectionModificationCallback<Person, PersonDrivesCar>(this.OnPersonPersonDrivesCarViaDrivenByPersonCollectionAdding), new CommittedCollectionModificationCallback<Person, PersonDrivesCar>(this.OnPersonPersonDrivesCarViaDrivenByPersonCollectionAdded), null, new CommittedCollectionModificationCallback<Person, PersonDrivesCar>(this.OnPersonPersonDrivesCarViaDrivenByPersonCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>).TypeHandle, "PersonBoughtCarFromPersonOnDateViaBuyerCollection"), new ConstraintEnforcementCollectionCallbacks<Person, PersonBoughtCarFromPersonOnDate>(new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdded), null, new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeAndNameDictionary.Add(new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>).TypeHandle, "PersonBoughtCarFromPersonOnDateViaSellerCollection"), new ConstraintEnforcementCollectionCallbacks<Person, PersonBoughtCarFromPersonOnDate>(new PotentialCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdding), new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdded), null, new CommittedCollectionModificationCallback<Person, PersonBoughtCarFromPersonOnDate>(this.OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<Person, PersonHasNickName>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<Person, PersonHasNickName>(new PotentialCollectionModificationCallback<Person, PersonHasNickName>(this.OnPersonPersonHasNickNameViaPersonCollectionAdding), new CommittedCollectionModificationCallback<Person, PersonHasNickName>(this.OnPersonPersonHasNickNameViaPersonCollectionAdded), null, new CommittedCollectionModificationCallback<Person, PersonHasNickName>(this.OnPersonPersonHasNickNameViaPersonCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<ValueType1, Person>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<ValueType1, Person>(new PotentialCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdding), new CommittedCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdded), null, new CommittedCollectionModificationCallback<ValueType1, Person>(this.OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<MalePerson, ChildPerson>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<MalePerson, ChildPerson>(new PotentialCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonViaFatherCollectionAdding), new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonViaFatherCollectionAdded), null, new CommittedCollectionModificationCallback<MalePerson, ChildPerson>(this.OnMalePersonChildPersonViaFatherCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<FemalePerson, ChildPerson>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<FemalePerson, ChildPerson>(new PotentialCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonViaMotherCollectionAdding), new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonViaMotherCollectionAdded), null, new CommittedCollectionModificationCallback<FemalePerson, ChildPerson>(this.OnFemalePersonChildPersonViaMotherCollectionRemoved)));
			this._PersonReadOnlyCollection = new ReadOnlyCollection<Person>(this._PersonList = new List<Person>());
			this._TaskReadOnlyCollection = new ReadOnlyCollection<Task>(this._TaskList = new List<Task>());
			this._ValueType1ReadOnlyCollection = new ReadOnlyCollection<ValueType1>(this._ValueType1List = new List<ValueType1>());
			this._DeathReadOnlyCollection = new ReadOnlyCollection<Death>(this._DeathList = new List<Death>());
			this._NaturalDeathReadOnlyCollection = new ReadOnlyCollection<NaturalDeath>(this._NaturalDeathList = new List<NaturalDeath>());
			this._UnnaturalDeathReadOnlyCollection = new ReadOnlyCollection<UnnaturalDeath>(this._UnnaturalDeathList = new List<UnnaturalDeath>());
			this._MalePersonReadOnlyCollection = new ReadOnlyCollection<MalePerson>(this._MalePersonList = new List<MalePerson>());
			this._FemalePersonReadOnlyCollection = new ReadOnlyCollection<FemalePerson>(this._FemalePersonList = new List<FemalePerson>());
			this._ChildPersonReadOnlyCollection = new ReadOnlyCollection<ChildPerson>(this._ChildPersonList = new List<ChildPerson>());
			this._PersonDrivesCarReadOnlyCollection = new ReadOnlyCollection<PersonDrivesCar>(this._PersonDrivesCarList = new List<PersonDrivesCar>());
			this._PersonBoughtCarFromPersonOnDateReadOnlyCollection = new ReadOnlyCollection<PersonBoughtCarFromPersonOnDate>(this._PersonBoughtCarFromPersonOnDateList = new List<PersonBoughtCarFromPersonOnDate>());
			this._ReviewReadOnlyCollection = new ReadOnlyCollection<Review>(this._ReviewList = new List<Review>());
			this._PersonHasNickNameReadOnlyCollection = new ReadOnlyCollection<PersonHasNickName>(this._PersonHasNickNameList = new List<PersonHasNickName>());
		}
		#region Exception Helpers
		private static ArgumentException GetDifferentContextsException()
		{
			return SampleModelContext.GetDifferentContextsException("value");
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
		private readonly Dictionary<Tuple<string, System.DateTime>, Person> _PersonFirstNameAndYMDDictionary = new Dictionary<Tuple<string, System.DateTime>, Person>();
		public Person GetPersonByFirstNameAndYMD(string firstName, System.DateTime YMD)
		{
			return this._PersonFirstNameAndYMDDictionary[Tuple.CreateTuple<string, System.DateTime>(firstName, YMD)];
		}
		public bool TryGetPersonByFirstNameAndYMD(string firstName, System.DateTime YMD, out Person person)
		{
			return this._PersonFirstNameAndYMDDictionary.TryGetValue(Tuple.CreateTuple<string, System.DateTime>(firstName, YMD), out person);
		}
		private bool OnFirstNameAndYMDChanging(Person instance, Tuple<string, System.DateTime> newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._PersonFirstNameAndYMDDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnFirstNameAndYMDChanged(Person instance, Tuple<string, System.DateTime> oldValue, Tuple<string, System.DateTime> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonFirstNameAndYMDDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonFirstNameAndYMDDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, System.DateTime>, Person> _PersonLastNameAndYMDDictionary = new Dictionary<Tuple<string, System.DateTime>, Person>();
		public Person GetPersonByLastNameAndYMD(string lastName, System.DateTime YMD)
		{
			return this._PersonLastNameAndYMDDictionary[Tuple.CreateTuple<string, System.DateTime>(lastName, YMD)];
		}
		public bool TryGetPersonByLastNameAndYMD(string lastName, System.DateTime YMD, out Person person)
		{
			return this._PersonLastNameAndYMDDictionary.TryGetValue(Tuple.CreateTuple<string, System.DateTime>(lastName, YMD), out person);
		}
		private bool OnLastNameAndYMDChanging(Person instance, Tuple<string, System.DateTime> newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._PersonLastNameAndYMDDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnLastNameAndYMDChanged(Person instance, Tuple<string, System.DateTime> oldValue, Tuple<string, System.DateTime> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonLastNameAndYMDDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonLastNameAndYMDDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<string, Person> _PersonOptionalUniqueStringDictionary = new Dictionary<string, Person>();
		public Person GetPersonByOptionalUniqueString(string optionalUniqueString)
		{
			return this._PersonOptionalUniqueStringDictionary[optionalUniqueString];
		}
		public bool TryGetPersonByOptionalUniqueString(string optionalUniqueString, out Person person)
		{
			return this._PersonOptionalUniqueStringDictionary.TryGetValue(optionalUniqueString, out person);
		}
		private readonly Dictionary<Person, Person> _PersonWifeDictionary = new Dictionary<Person, Person>();
		public Person GetPersonByWife(Person wife)
		{
			return this._PersonWifeDictionary[wife];
		}
		public bool TryGetPersonByWife(Person wife, out Person person)
		{
			return this._PersonWifeDictionary.TryGetValue(wife, out person);
		}
		private readonly Dictionary<uint, Person> _PersonVinDictionary = new Dictionary<uint, Person>();
		public Person GetPersonByVin(uint vin)
		{
			return this._PersonVinDictionary[vin];
		}
		public bool TryGetPersonByVin(uint vin, out Person person)
		{
			return this._PersonVinDictionary.TryGetValue(vin, out person);
		}
		private readonly Dictionary<int, Person> _PersonOptionalUniqueDecimalDictionary = new Dictionary<int, Person>();
		public Person GetPersonByOptionalUniqueDecimal(int optionalUniqueDecimal)
		{
			return this._PersonOptionalUniqueDecimalDictionary[optionalUniqueDecimal];
		}
		public bool TryGetPersonByOptionalUniqueDecimal(int optionalUniqueDecimal, out Person person)
		{
			return this._PersonOptionalUniqueDecimalDictionary.TryGetValue(optionalUniqueDecimal, out person);
		}
		private readonly Dictionary<int, Person> _PersonMandatoryUniqueDecimalDictionary = new Dictionary<int, Person>();
		public Person GetPersonByMandatoryUniqueDecimal(int mandatoryUniqueDecimal)
		{
			return this._PersonMandatoryUniqueDecimalDictionary[mandatoryUniqueDecimal];
		}
		public bool TryGetPersonByMandatoryUniqueDecimal(int mandatoryUniqueDecimal, out Person person)
		{
			return this._PersonMandatoryUniqueDecimalDictionary.TryGetValue(mandatoryUniqueDecimal, out person);
		}
		private readonly Dictionary<string, Person> _PersonMandatoryUniqueStringDictionary = new Dictionary<string, Person>();
		public Person GetPersonByMandatoryUniqueString(string mandatoryUniqueString)
		{
			return this._PersonMandatoryUniqueStringDictionary[mandatoryUniqueString];
		}
		public bool TryGetPersonByMandatoryUniqueString(string mandatoryUniqueString, out Person person)
		{
			return this._PersonMandatoryUniqueStringDictionary.TryGetValue(mandatoryUniqueString, out person);
		}
		private readonly Dictionary<byte, Person> _PersonOptionalUniqueTinyIntDictionary = new Dictionary<byte, Person>();
		public Person GetPersonByOptionalUniqueTinyInt(byte optionalUniqueTinyInt)
		{
			return this._PersonOptionalUniqueTinyIntDictionary[optionalUniqueTinyInt];
		}
		public bool TryGetPersonByOptionalUniqueTinyInt(byte optionalUniqueTinyInt, out Person person)
		{
			return this._PersonOptionalUniqueTinyIntDictionary.TryGetValue(optionalUniqueTinyInt, out person);
		}
		private readonly Dictionary<byte, Person> _PersonMandatoryUniqueTinyIntDictionary = new Dictionary<byte, Person>();
		public Person GetPersonByMandatoryUniqueTinyInt(byte mandatoryUniqueTinyInt)
		{
			return this._PersonMandatoryUniqueTinyIntDictionary[mandatoryUniqueTinyInt];
		}
		public bool TryGetPersonByMandatoryUniqueTinyInt(byte mandatoryUniqueTinyInt, out Person person)
		{
			return this._PersonMandatoryUniqueTinyIntDictionary.TryGetValue(mandatoryUniqueTinyInt, out person);
		}
		private readonly Dictionary<int, Task> _TaskTaskIdDictionary = new Dictionary<int, Task>();
		public Task GetTaskByTaskId(int taskId)
		{
			return this._TaskTaskIdDictionary[taskId];
		}
		public bool TryGetTaskByTaskId(int taskId, out Task task)
		{
			return this._TaskTaskIdDictionary.TryGetValue(taskId, out task);
		}
		private readonly Dictionary<int, ValueType1> _ValueType1ValueType1ValueDictionary = new Dictionary<int, ValueType1>();
		public ValueType1 GetValueType1ByValueType1Value(int valueType1Value)
		{
			return this._ValueType1ValueType1ValueDictionary[valueType1Value];
		}
		public bool TryGetValueType1ByValueType1Value(int valueType1Value, out ValueType1 valueType1)
		{
			return this._ValueType1ValueType1ValueDictionary.TryGetValue(valueType1Value, out valueType1);
		}
		private readonly Dictionary<Tuple<MalePerson, uint, FemalePerson>, ChildPerson> _ChildPersonFatherAndBirthOrderNrAndMotherDictionary = new Dictionary<Tuple<MalePerson, uint, FemalePerson>, ChildPerson>();
		public ChildPerson GetChildPersonByFatherAndBirthOrderNrAndMother(MalePerson father, uint birthOrderNr, FemalePerson mother)
		{
			return this._ChildPersonFatherAndBirthOrderNrAndMotherDictionary[Tuple.CreateTuple<MalePerson, uint, FemalePerson>(father, birthOrderNr, mother)];
		}
		public bool TryGetChildPersonByFatherAndBirthOrderNrAndMother(MalePerson father, uint birthOrderNr, FemalePerson mother, out ChildPerson childPerson)
		{
			return this._ChildPersonFatherAndBirthOrderNrAndMotherDictionary.TryGetValue(Tuple.CreateTuple<MalePerson, uint, FemalePerson>(father, birthOrderNr, mother), out childPerson);
		}
		private bool OnFatherAndBirthOrderNrAndMotherChanging(ChildPerson instance, Tuple<MalePerson, uint, FemalePerson> newValue)
		{
			if ((object)newValue != null)
			{
				ChildPerson currentInstance;
				if (this._ChildPersonFatherAndBirthOrderNrAndMotherDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnFatherAndBirthOrderNrAndMotherChanged(ChildPerson instance, Tuple<MalePerson, uint, FemalePerson> oldValue, Tuple<MalePerson, uint, FemalePerson> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ChildPersonFatherAndBirthOrderNrAndMotherDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ChildPersonFatherAndBirthOrderNrAndMotherDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<uint, Person>, PersonDrivesCar> _PersonDrivesCarVinAndDrivenByPersonDictionary = new Dictionary<Tuple<uint, Person>, PersonDrivesCar>();
		public PersonDrivesCar GetPersonDrivesCarByVinAndDrivenByPerson(uint vin, Person drivenByPerson)
		{
			return this._PersonDrivesCarVinAndDrivenByPersonDictionary[Tuple.CreateTuple<uint, Person>(vin, drivenByPerson)];
		}
		public bool TryGetPersonDrivesCarByVinAndDrivenByPerson(uint vin, Person drivenByPerson, out PersonDrivesCar personDrivesCar)
		{
			return this._PersonDrivesCarVinAndDrivenByPersonDictionary.TryGetValue(Tuple.CreateTuple<uint, Person>(vin, drivenByPerson), out personDrivesCar);
		}
		private bool OnVinAndDrivenByPersonChanging(PersonDrivesCar instance, Tuple<uint, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonDrivesCar currentInstance;
				if (this._PersonDrivesCarVinAndDrivenByPersonDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnVinAndDrivenByPersonChanged(PersonDrivesCar instance, Tuple<uint, Person> oldValue, Tuple<uint, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonDrivesCarVinAndDrivenByPersonDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonDrivesCarVinAndDrivenByPersonDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<Person, uint, Person>, PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateBuyerAndVinAndSellerDictionary = new Dictionary<Tuple<Person, uint, Person>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByBuyerAndVinAndSeller(Person buyer, uint vin, Person seller)
		{
			return this._PersonBoughtCarFromPersonOnDateBuyerAndVinAndSellerDictionary[Tuple.CreateTuple<Person, uint, Person>(buyer, vin, seller)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByBuyerAndVinAndSeller(Person buyer, uint vin, Person seller, out PersonBoughtCarFromPersonOnDate personBoughtCarFromPersonOnDate)
		{
			return this._PersonBoughtCarFromPersonOnDateBuyerAndVinAndSellerDictionary.TryGetValue(Tuple.CreateTuple<Person, uint, Person>(buyer, vin, seller), out personBoughtCarFromPersonOnDate);
		}
		private bool OnBuyerAndVinAndSellerChanging(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, uint, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._PersonBoughtCarFromPersonOnDateBuyerAndVinAndSellerDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnBuyerAndVinAndSellerChanged(PersonBoughtCarFromPersonOnDate instance, Tuple<Person, uint, Person> oldValue, Tuple<Person, uint, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonBoughtCarFromPersonOnDateBuyerAndVinAndSellerDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonBoughtCarFromPersonOnDateBuyerAndVinAndSellerDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<uint, System.DateTime, Person>, PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateVinAndYMDAndBuyerDictionary = new Dictionary<Tuple<uint, System.DateTime, Person>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByVinAndYMDAndBuyer(uint vin, System.DateTime YMD, Person buyer)
		{
			return this._PersonBoughtCarFromPersonOnDateVinAndYMDAndBuyerDictionary[Tuple.CreateTuple<uint, System.DateTime, Person>(vin, YMD, buyer)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByVinAndYMDAndBuyer(uint vin, System.DateTime YMD, Person buyer, out PersonBoughtCarFromPersonOnDate personBoughtCarFromPersonOnDate)
		{
			return this._PersonBoughtCarFromPersonOnDateVinAndYMDAndBuyerDictionary.TryGetValue(Tuple.CreateTuple<uint, System.DateTime, Person>(vin, YMD, buyer), out personBoughtCarFromPersonOnDate);
		}
		private bool OnVinAndYMDAndBuyerChanging(PersonBoughtCarFromPersonOnDate instance, Tuple<uint, System.DateTime, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._PersonBoughtCarFromPersonOnDateVinAndYMDAndBuyerDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnVinAndYMDAndBuyerChanged(PersonBoughtCarFromPersonOnDate instance, Tuple<uint, System.DateTime, Person> oldValue, Tuple<uint, System.DateTime, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonBoughtCarFromPersonOnDateVinAndYMDAndBuyerDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonBoughtCarFromPersonOnDateVinAndYMDAndBuyerDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<System.DateTime, Person, uint>, PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateYMDAndSellerAndVinDictionary = new Dictionary<Tuple<System.DateTime, Person, uint>, PersonBoughtCarFromPersonOnDate>();
		public PersonBoughtCarFromPersonOnDate GetPersonBoughtCarFromPersonOnDateByYMDAndSellerAndVin(System.DateTime YMD, Person seller, uint vin)
		{
			return this._PersonBoughtCarFromPersonOnDateYMDAndSellerAndVinDictionary[Tuple.CreateTuple<System.DateTime, Person, uint>(YMD, seller, vin)];
		}
		public bool TryGetPersonBoughtCarFromPersonOnDateByYMDAndSellerAndVin(System.DateTime YMD, Person seller, uint vin, out PersonBoughtCarFromPersonOnDate personBoughtCarFromPersonOnDate)
		{
			return this._PersonBoughtCarFromPersonOnDateYMDAndSellerAndVinDictionary.TryGetValue(Tuple.CreateTuple<System.DateTime, Person, uint>(YMD, seller, vin), out personBoughtCarFromPersonOnDate);
		}
		private bool OnYMDAndSellerAndVinChanging(PersonBoughtCarFromPersonOnDate instance, Tuple<System.DateTime, Person, uint> newValue)
		{
			if ((object)newValue != null)
			{
				PersonBoughtCarFromPersonOnDate currentInstance;
				if (this._PersonBoughtCarFromPersonOnDateYMDAndSellerAndVinDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnYMDAndSellerAndVinChanged(PersonBoughtCarFromPersonOnDate instance, Tuple<System.DateTime, Person, uint> oldValue, Tuple<System.DateTime, Person, uint> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonBoughtCarFromPersonOnDateYMDAndSellerAndVinDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonBoughtCarFromPersonOnDateYMDAndSellerAndVinDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<uint, string>, Review> _ReviewVinAndNameDictionary = new Dictionary<Tuple<uint, string>, Review>();
		public Review GetReviewByVinAndName(uint vin, string name)
		{
			return this._ReviewVinAndNameDictionary[Tuple.CreateTuple<uint, string>(vin, name)];
		}
		public bool TryGetReviewByVinAndName(uint vin, string name, out Review review)
		{
			return this._ReviewVinAndNameDictionary.TryGetValue(Tuple.CreateTuple<uint, string>(vin, name), out review);
		}
		private bool OnVinAndNameChanging(Review instance, Tuple<uint, string> newValue)
		{
			if ((object)newValue != null)
			{
				Review currentInstance;
				if (this._ReviewVinAndNameDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnVinAndNameChanged(Review instance, Tuple<uint, string> oldValue, Tuple<uint, string> newValue)
		{
			if ((object)oldValue != null)
			{
				this._ReviewVinAndNameDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._ReviewVinAndNameDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<Tuple<string, Person>, PersonHasNickName> _PersonHasNickNameNickNameAndPersonDictionary = new Dictionary<Tuple<string, Person>, PersonHasNickName>();
		public PersonHasNickName GetPersonHasNickNameByNickNameAndPerson(string nickName, Person person)
		{
			return this._PersonHasNickNameNickNameAndPersonDictionary[Tuple.CreateTuple<string, Person>(nickName, person)];
		}
		public bool TryGetPersonHasNickNameByNickNameAndPerson(string nickName, Person person, out PersonHasNickName personHasNickName)
		{
			return this._PersonHasNickNameNickNameAndPersonDictionary.TryGetValue(Tuple.CreateTuple<string, Person>(nickName, person), out personHasNickName);
		}
		private bool OnNickNameAndPersonChanging(PersonHasNickName instance, Tuple<string, Person> newValue)
		{
			if ((object)newValue != null)
			{
				PersonHasNickName currentInstance;
				if (this._PersonHasNickNameNickNameAndPersonDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnNickNameAndPersonChanged(PersonHasNickName instance, Tuple<string, Person> oldValue, Tuple<string, Person> newValue)
		{
			if ((object)oldValue != null)
			{
				this._PersonHasNickNameNickNameAndPersonDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._PersonHasNickNameNickNameAndPersonDictionary.Add(newValue, instance);
			}
		}
		#endregion // Lookup and External Constraint Enforcement
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty item)
			where TClass : class, IHasSampleModelContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty item)
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
		private ConstraintEnforcementCollectionCallbacks<TClass, TProperty> GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>()
			where TClass : class, IHasSampleModelContext
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
			where TClass : class, IHasSampleModelContext
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
		private ConstraintEnforcementCollectionCallbacks<TClass, TProperty> GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>(string propertyName)
			where TClass : class, IHasSampleModelContext
		{
			return (ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary[new ConstraintEnforcementCollectionTypeAndPropertyNameKey(typeof(ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty>).TypeHandle, propertyName)];
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private struct ConstraintEnforcementCollectionTypeAndPropertyNameKey : IEquatable<ConstraintEnforcementCollectionTypeAndPropertyNameKey>
		{
			public ConstraintEnforcementCollectionTypeAndPropertyNameKey(RuntimeTypeHandle typeHandle, string name)
			{
				this.TypeHandle = typeHandle;
				this.Name = name;
			}
			public readonly RuntimeTypeHandle TypeHandle;
			public readonly string Name;
			public override int GetHashCode()
			{
				return this.TypeHandle.GetHashCode() ^ this.Name.GetHashCode();
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals")]
			public override bool Equals(object obj)
			{
				return obj is ConstraintEnforcementCollectionTypeAndPropertyNameKey && this.Equals((ConstraintEnforcementCollectionTypeAndPropertyNameKey)obj);
			}
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals")]
			public bool Equals(ConstraintEnforcementCollectionTypeAndPropertyNameKey other)
			{
				return this.TypeHandle.Equals(other.TypeHandle) && this.Name.Equals(other.Name);
			}
		}
		private readonly Dictionary<ConstraintEnforcementCollectionTypeAndPropertyNameKey, object> _ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary;
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionWithPropertyName<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasSampleModelContext
		{
			private readonly TClass _Instance;
			private readonly string _PropertyName;
			private readonly List<TProperty> _List = new List<TProperty>();
			public ConstraintEnforcementCollectionWithPropertyName(TClass instance, string propertyName)
			{
				this._Instance = instance;
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
				return this._List.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				TClass instance = this._Instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>(this._PropertyName);
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
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>(this._PropertyName);
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
		public Person CreatePerson(string firstName, string lastName, System.DateTime YMD, string genderCode, int mandatoryUniqueDecimal, string mandatoryUniqueString, byte mandatoryUniqueTinyInt, byte mandatoryNonUniqueTinyInt, int mandatoryNonUniqueUnconstrainedDecimal, float mandatoryNonUniqueUnconstrainedFloat)
		{
			if ((object)firstName == null)
			{
				throw new ArgumentNullException("firstName");
			}
			if ((object)lastName == null)
			{
				throw new ArgumentNullException("lastName");
			}
			if ((object)genderCode == null)
			{
				throw new ArgumentNullException("genderCode");
			}
			if ((object)mandatoryUniqueString == null)
			{
				throw new ArgumentNullException("mandatoryUniqueString");
			}
			if (!this.OnPersonFirstNameChanging(null, firstName))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("firstName");
			}
			if (!this.OnPersonLastNameChanging(null, lastName))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("lastName");
			}
			if (!this.OnPersonYMDChanging(null, YMD))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("YMD");
			}
			if (!this.OnPersonGenderCodeChanging(null, genderCode))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("genderCode");
			}
			if (!this.OnPersonMandatoryUniqueDecimalChanging(null, mandatoryUniqueDecimal))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mandatoryUniqueDecimal");
			}
			if (!this.OnPersonMandatoryUniqueStringChanging(null, mandatoryUniqueString))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mandatoryUniqueString");
			}
			if (!this.OnPersonMandatoryUniqueTinyIntChanging(null, mandatoryUniqueTinyInt))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mandatoryUniqueTinyInt");
			}
			if (!this.OnPersonMandatoryNonUniqueTinyIntChanging(null, mandatoryNonUniqueTinyInt))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mandatoryNonUniqueTinyInt");
			}
			if (!this.OnPersonMandatoryNonUniqueUnconstrainedDecimalChanging(null, mandatoryNonUniqueUnconstrainedDecimal))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mandatoryNonUniqueUnconstrainedDecimal");
			}
			if (!this.OnPersonMandatoryNonUniqueUnconstrainedFloatChanging(null, mandatoryNonUniqueUnconstrainedFloat))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mandatoryNonUniqueUnconstrainedFloat");
			}
			return new PersonImpl(this, firstName, lastName, YMD, genderCode, mandatoryUniqueDecimal, mandatoryUniqueString, mandatoryUniqueTinyInt, mandatoryNonUniqueTinyInt, mandatoryNonUniqueUnconstrainedDecimal, mandatoryNonUniqueUnconstrainedFloat);
		}
		private bool OnPersonFirstNameChanging(Person instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnFirstNameAndYMDChanging(instance, Tuple.CreateTuple<string, System.DateTime>(newValue, instance.YMD)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonFirstNameChanged(Person instance, string oldValue)
		{
			Tuple<string, System.DateTime> FirstNameAndYMDOldValueTuple;
			if ((object)oldValue != null)
			{
				FirstNameAndYMDOldValueTuple = Tuple.CreateTuple<string, System.DateTime>(oldValue, instance.YMD);
			}
			else
			{
				FirstNameAndYMDOldValueTuple = null;
			}
			this.OnFirstNameAndYMDChanged(instance, FirstNameAndYMDOldValueTuple, Tuple.CreateTuple<string, System.DateTime>(instance.FirstName, instance.YMD));
		}
		private bool OnPersonLastNameChanging(Person instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnLastNameAndYMDChanging(instance, Tuple.CreateTuple<string, System.DateTime>(newValue, instance.YMD)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonLastNameChanged(Person instance, string oldValue)
		{
			Tuple<string, System.DateTime> LastNameAndYMDOldValueTuple;
			if ((object)oldValue != null)
			{
				LastNameAndYMDOldValueTuple = Tuple.CreateTuple<string, System.DateTime>(oldValue, instance.YMD);
			}
			else
			{
				LastNameAndYMDOldValueTuple = null;
			}
			this.OnLastNameAndYMDChanged(instance, LastNameAndYMDOldValueTuple, Tuple.CreateTuple<string, System.DateTime>(instance.LastName, instance.YMD));
		}
		private bool OnPersonOptionalUniqueStringChanging(Person instance, string newValue)
		{
			if ((object)newValue != null)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueStringDictionary.TryGetValue(newValue, out currentInstance))
				{
					if ((object)currentInstance != (object)instance)
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
		private bool OnPersonColorARGBChanging(Person instance, Nullable<int> newValue)
		{
			return true;
		}
		private bool OnPersonHatTypeStyleDescriptionChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonVinChanging(Person instance, Nullable<uint> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonVinDictionary.TryGetValue(newValue.GetValueOrDefault(), out currentInstance))
				{
					if ((object)currentInstance != (object)instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonVinChanged(Person instance, Nullable<uint> oldValue)
		{
			if (instance.Vin.HasValue)
			{
				this._PersonVinDictionary.Add(instance.Vin.GetValueOrDefault(), instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonVinDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnPersonYMDChanging(Person instance, System.DateTime newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnFirstNameAndYMDChanging(instance, Tuple.CreateTuple<string, System.DateTime>(instance.FirstName, newValue)))
				{
					return false;
				}
				if (!this.OnLastNameAndYMDChanging(instance, Tuple.CreateTuple<string, System.DateTime>(instance.LastName, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonYMDChanged(Person instance, Nullable<System.DateTime> oldValue)
		{
			Tuple<string, System.DateTime> FirstNameAndYMDOldValueTuple;
			Tuple<string, System.DateTime> LastNameAndYMDOldValueTuple;
			if (oldValue.HasValue)
			{
				FirstNameAndYMDOldValueTuple = Tuple.CreateTuple<string, System.DateTime>(instance.FirstName, oldValue.GetValueOrDefault());
				LastNameAndYMDOldValueTuple = Tuple.CreateTuple<string, System.DateTime>(instance.LastName, oldValue.GetValueOrDefault());
			}
			else
			{
				FirstNameAndYMDOldValueTuple = null;
				LastNameAndYMDOldValueTuple = null;
			}
			this.OnFirstNameAndYMDChanged(instance, FirstNameAndYMDOldValueTuple, Tuple.CreateTuple<string, System.DateTime>(instance.FirstName, instance.YMD));
			this.OnLastNameAndYMDChanged(instance, LastNameAndYMDOldValueTuple, Tuple.CreateTuple<string, System.DateTime>(instance.LastName, instance.YMD));
		}
		private bool OnPersonIsDeadChanging(Person instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnPersonGenderCodeChanging(Person instance, string newValue)
		{
			return true;
		}
		private bool OnPersonHasParentsChanging(Person instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnPersonOptionalUniqueDecimalChanging(Person instance, Nullable<int> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueDecimalDictionary.TryGetValue(newValue.GetValueOrDefault(), out currentInstance))
				{
					if ((object)currentInstance != (object)instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOptionalUniqueDecimalChanged(Person instance, Nullable<int> oldValue)
		{
			if (instance.OptionalUniqueDecimal.HasValue)
			{
				this._PersonOptionalUniqueDecimalDictionary.Add(instance.OptionalUniqueDecimal.GetValueOrDefault(), instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonOptionalUniqueDecimalDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnPersonMandatoryUniqueDecimalChanging(Person instance, int newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueDecimalDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != (object)instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonMandatoryUniqueDecimalChanged(Person instance, Nullable<int> oldValue)
		{
			this._PersonMandatoryUniqueDecimalDictionary.Add(instance.MandatoryUniqueDecimal, instance);
			if (oldValue.HasValue)
			{
				this._PersonMandatoryUniqueDecimalDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnPersonMandatoryUniqueStringChanging(Person instance, string newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueStringDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != (object)instance)
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
		private bool OnPersonOptionalUniqueTinyIntChanging(Person instance, Nullable<byte> newValue)
		{
			if (newValue.HasValue)
			{
				Person currentInstance;
				if (this._PersonOptionalUniqueTinyIntDictionary.TryGetValue(newValue.GetValueOrDefault(), out currentInstance))
				{
					if ((object)currentInstance != (object)instance)
					{
						return false;
					}
				}
			}
			return true;
		}
		private void OnPersonOptionalUniqueTinyIntChanged(Person instance, Nullable<byte> oldValue)
		{
			if (instance.OptionalUniqueTinyInt.HasValue)
			{
				this._PersonOptionalUniqueTinyIntDictionary.Add(instance.OptionalUniqueTinyInt.GetValueOrDefault(), instance);
			}
			if (oldValue.HasValue)
			{
				this._PersonOptionalUniqueTinyIntDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnPersonMandatoryUniqueTinyIntChanging(Person instance, byte newValue)
		{
			Person currentInstance;
			if (this._PersonMandatoryUniqueTinyIntDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != (object)instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonMandatoryUniqueTinyIntChanged(Person instance, Nullable<byte> oldValue)
		{
			this._PersonMandatoryUniqueTinyIntDictionary.Add(instance.MandatoryUniqueTinyInt, instance);
			if (oldValue.HasValue)
			{
				this._PersonMandatoryUniqueTinyIntDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnPersonOptionalNonUniqueTinyIntChanging(Person instance, Nullable<byte> newValue)
		{
			return true;
		}
		private bool OnPersonMandatoryNonUniqueTinyIntChanging(Person instance, byte newValue)
		{
			return true;
		}
		private bool OnPersonMandatoryNonUniqueUnconstrainedDecimalChanging(Person instance, int newValue)
		{
			return true;
		}
		private bool OnPersonMandatoryNonUniqueUnconstrainedFloatChanging(Person instance, float newValue)
		{
			return true;
		}
		private bool OnPersonWifeChanging(Person instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
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
				instance.Wife.HusbandViaWife = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.HusbandViaWife = null;
			}
		}
		private bool OnPersonValueType1DoesSomethingElseWithChanging(Person instance, ValueType1 newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
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
				if ((object)this != (object)newValue.Context)
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
				if ((object)this != (object)newValue.Context)
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
				if ((object)this != (object)newValue.Context)
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
				if ((object)this != (object)newValue.Context)
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
		private bool OnPersonHusbandViaWifeChanging(Person instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
				{
					throw SampleModelContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnPersonHusbandViaWifeChanged(Person instance, Person oldValue)
		{
			if ((object)instance.HusbandViaWife != null)
			{
				instance.HusbandViaWife.Wife = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.Wife = null;
			}
		}
		private bool OnPersonTaskViaPersonCollectionAdding(Person instance, Task item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnPersonTaskViaPersonCollectionAdded(Person instance, Task item)
		{
			item.Person = instance;
		}
		private void OnPersonTaskViaPersonCollectionRemoved(Person instance, Task item)
		{
			if ((object)item.Person == (object)instance)
			{
				item.Person = null;
			}
		}
		private bool OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdding(Person instance, ValueType1 item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionAdded(Person instance, ValueType1 item)
		{
			item.DoesSomethingWithPerson = instance;
		}
		private void OnPersonValueType1DoesSomethingWithViaDoesSomethingWithPersonCollectionRemoved(Person instance, ValueType1 item)
		{
			if ((object)item.DoesSomethingWithPerson == (object)instance)
			{
				item.DoesSomethingWithPerson = null;
			}
		}
		private bool OnPersonPersonDrivesCarViaDrivenByPersonCollectionAdding(Person instance, PersonDrivesCar item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnPersonPersonDrivesCarViaDrivenByPersonCollectionAdded(Person instance, PersonDrivesCar item)
		{
			item.DrivenByPerson = instance;
		}
		private void OnPersonPersonDrivesCarViaDrivenByPersonCollectionRemoved(Person instance, PersonDrivesCar item)
		{
			if ((object)item.DrivenByPerson == (object)instance)
			{
				item.DrivenByPerson = null;
			}
		}
		private bool OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdding(Person instance, PersonBoughtCarFromPersonOnDate item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionAdded(Person instance, PersonBoughtCarFromPersonOnDate item)
		{
			item.Buyer = instance;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaBuyerCollectionRemoved(Person instance, PersonBoughtCarFromPersonOnDate item)
		{
			if ((object)item.Buyer == (object)instance)
			{
				item.Buyer = null;
			}
		}
		private bool OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdding(Person instance, PersonBoughtCarFromPersonOnDate item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionAdded(Person instance, PersonBoughtCarFromPersonOnDate item)
		{
			item.Seller = instance;
		}
		private void OnPersonPersonBoughtCarFromPersonOnDateViaSellerCollectionRemoved(Person instance, PersonBoughtCarFromPersonOnDate item)
		{
			if ((object)item.Seller == (object)instance)
			{
				item.Seller = null;
			}
		}
		private bool OnPersonPersonHasNickNameViaPersonCollectionAdding(Person instance, PersonHasNickName item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnPersonPersonHasNickNameViaPersonCollectionAdded(Person instance, PersonHasNickName item)
		{
			item.Person = instance;
		}
		private void OnPersonPersonHasNickNameViaPersonCollectionRemoved(Person instance, PersonHasNickName item)
		{
			if ((object)item.Person == (object)instance)
			{
				item.Person = null;
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
			public PersonImpl(SampleModelContext context, string firstName, string lastName, System.DateTime YMD, string genderCode, int mandatoryUniqueDecimal, string mandatoryUniqueString, byte mandatoryUniqueTinyInt, byte mandatoryNonUniqueTinyInt, int mandatoryNonUniqueUnconstrainedDecimal, float mandatoryNonUniqueUnconstrainedFloat)
			{
				this._Context = context;
				this._TaskViaPersonCollection = new ConstraintEnforcementCollection<Person, Task>(this);
				this._ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection = new ConstraintEnforcementCollection<Person, ValueType1>(this);
				this._PersonDrivesCarViaDrivenByPersonCollection = new ConstraintEnforcementCollection<Person, PersonDrivesCar>(this);
				this._PersonBoughtCarFromPersonOnDateViaBuyerCollection = new ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>(this, "PersonBoughtCarFromPersonOnDateViaBuyerCollection");
				this._PersonBoughtCarFromPersonOnDateViaSellerCollection = new ConstraintEnforcementCollectionWithPropertyName<Person, PersonBoughtCarFromPersonOnDate>(this, "PersonBoughtCarFromPersonOnDateViaSellerCollection");
				this._PersonHasNickNameViaPersonCollection = new ConstraintEnforcementCollection<Person, PersonHasNickName>(this);
				this._FirstName = firstName;
				context.OnPersonFirstNameChanged(this, null);
				this._LastName = lastName;
				context.OnPersonLastNameChanged(this, null);
				this._YMD = YMD;
				context.OnPersonYMDChanged(this, null);
				this._GenderCode = genderCode;
				this._MandatoryUniqueDecimal = mandatoryUniqueDecimal;
				context.OnPersonMandatoryUniqueDecimalChanged(this, null);
				this._MandatoryUniqueString = mandatoryUniqueString;
				context.OnPersonMandatoryUniqueStringChanged(this, null);
				this._MandatoryUniqueTinyInt = mandatoryUniqueTinyInt;
				context.OnPersonMandatoryUniqueTinyIntChanged(this, null);
				this._MandatoryNonUniqueTinyInt = mandatoryNonUniqueTinyInt;
				this._MandatoryNonUniqueUnconstrainedDecimal = mandatoryNonUniqueUnconstrainedDecimal;
				this._MandatoryNonUniqueUnconstrainedFloat = mandatoryNonUniqueUnconstrainedFloat;
				context._PersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
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
							this._Context.OnPersonFirstNameChanged(this, oldValue);
							base.OnFirstNameChanged(oldValue);
						}
					}
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
							this._Context.OnPersonLastNameChanged(this, oldValue);
							base.OnLastNameChanged(oldValue);
						}
					}
				}
			}
			private string _OptionalUniqueString;
			public sealed override string OptionalUniqueString
			{
				get
				{
					return this._OptionalUniqueString;
				}
				set
				{
					string oldValue = this._OptionalUniqueString;
					if (!object.Equals(oldValue, value))
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
			private Nullable<int> _ColorARGB;
			public sealed override Nullable<int> ColorARGB
			{
				get
				{
					return this._ColorARGB;
				}
				set
				{
					Nullable<int> oldValue = this._ColorARGB;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnPersonColorARGBChanging(this, value) && base.OnColorARGBChanging(value))
						{
							this._ColorARGB = value;
							base.OnColorARGBChanged(oldValue);
						}
					}
				}
			}
			private string _HatTypeStyleDescription;
			public sealed override string HatTypeStyleDescription
			{
				get
				{
					return this._HatTypeStyleDescription;
				}
				set
				{
					string oldValue = this._HatTypeStyleDescription;
					if (!object.Equals(oldValue, value))
					{
						if (this._Context.OnPersonHatTypeStyleDescriptionChanging(this, value) && base.OnHatTypeStyleDescriptionChanging(value))
						{
							this._HatTypeStyleDescription = value;
							base.OnHatTypeStyleDescriptionChanged(oldValue);
						}
					}
				}
			}
			private Nullable<uint> _Vin;
			public sealed override Nullable<uint> Vin
			{
				get
				{
					return this._Vin;
				}
				set
				{
					Nullable<uint> oldValue = this._Vin;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnPersonVinChanging(this, value) && base.OnVinChanging(value))
						{
							this._Vin = value;
							this._Context.OnPersonVinChanged(this, oldValue);
							base.OnVinChanged(oldValue);
						}
					}
				}
			}
			private System.DateTime _YMD;
			public sealed override System.DateTime YMD
			{
				get
				{
					return this._YMD;
				}
				set
				{
					System.DateTime oldValue = this._YMD;
					if (oldValue != value)
					{
						if (this._Context.OnPersonYMDChanging(this, value) && base.OnYMDChanging(value))
						{
							this._YMD = value;
							this._Context.OnPersonYMDChanged(this, oldValue);
							base.OnYMDChanged(oldValue);
						}
					}
				}
			}
			private Nullable<bool> _IsDead;
			public sealed override Nullable<bool> IsDead
			{
				get
				{
					return this._IsDead;
				}
				set
				{
					Nullable<bool> oldValue = this._IsDead;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnPersonIsDeadChanging(this, value) && base.OnIsDeadChanging(value))
						{
							this._IsDead = value;
							base.OnIsDeadChanged(oldValue);
						}
					}
				}
			}
			private string _GenderCode;
			public sealed override string GenderCode
			{
				get
				{
					return this._GenderCode;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._GenderCode;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnPersonGenderCodeChanging(this, value) && base.OnGenderCodeChanging(value))
						{
							this._GenderCode = value;
							base.OnGenderCodeChanged(oldValue);
						}
					}
				}
			}
			private Nullable<bool> _HasParents;
			public sealed override Nullable<bool> HasParents
			{
				get
				{
					return this._HasParents;
				}
				set
				{
					Nullable<bool> oldValue = this._HasParents;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnPersonHasParentsChanging(this, value) && base.OnHasParentsChanging(value))
						{
							this._HasParents = value;
							base.OnHasParentsChanged(oldValue);
						}
					}
				}
			}
			private Nullable<int> _OptionalUniqueDecimal;
			public sealed override Nullable<int> OptionalUniqueDecimal
			{
				get
				{
					return this._OptionalUniqueDecimal;
				}
				set
				{
					Nullable<int> oldValue = this._OptionalUniqueDecimal;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
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
			private int _MandatoryUniqueDecimal;
			public sealed override int MandatoryUniqueDecimal
			{
				get
				{
					return this._MandatoryUniqueDecimal;
				}
				set
				{
					int oldValue = this._MandatoryUniqueDecimal;
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
			private string _MandatoryUniqueString;
			public sealed override string MandatoryUniqueString
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
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
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
			private Nullable<byte> _OptionalUniqueTinyInt;
			public sealed override Nullable<byte> OptionalUniqueTinyInt
			{
				get
				{
					return this._OptionalUniqueTinyInt;
				}
				set
				{
					Nullable<byte> oldValue = this._OptionalUniqueTinyInt;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnPersonOptionalUniqueTinyIntChanging(this, value) && base.OnOptionalUniqueTinyIntChanging(value))
						{
							this._OptionalUniqueTinyInt = value;
							this._Context.OnPersonOptionalUniqueTinyIntChanged(this, oldValue);
							base.OnOptionalUniqueTinyIntChanged(oldValue);
						}
					}
				}
			}
			private byte _MandatoryUniqueTinyInt;
			public sealed override byte MandatoryUniqueTinyInt
			{
				get
				{
					return this._MandatoryUniqueTinyInt;
				}
				set
				{
					byte oldValue = this._MandatoryUniqueTinyInt;
					if (oldValue != value)
					{
						if (this._Context.OnPersonMandatoryUniqueTinyIntChanging(this, value) && base.OnMandatoryUniqueTinyIntChanging(value))
						{
							this._MandatoryUniqueTinyInt = value;
							this._Context.OnPersonMandatoryUniqueTinyIntChanged(this, oldValue);
							base.OnMandatoryUniqueTinyIntChanged(oldValue);
						}
					}
				}
			}
			private Nullable<byte> _OptionalNonUniqueTinyInt;
			public sealed override Nullable<byte> OptionalNonUniqueTinyInt
			{
				get
				{
					return this._OptionalNonUniqueTinyInt;
				}
				set
				{
					Nullable<byte> oldValue = this._OptionalNonUniqueTinyInt;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnPersonOptionalNonUniqueTinyIntChanging(this, value) && base.OnOptionalNonUniqueTinyIntChanging(value))
						{
							this._OptionalNonUniqueTinyInt = value;
							base.OnOptionalNonUniqueTinyIntChanged(oldValue);
						}
					}
				}
			}
			private byte _MandatoryNonUniqueTinyInt;
			public sealed override byte MandatoryNonUniqueTinyInt
			{
				get
				{
					return this._MandatoryNonUniqueTinyInt;
				}
				set
				{
					byte oldValue = this._MandatoryNonUniqueTinyInt;
					if (oldValue != value)
					{
						if (this._Context.OnPersonMandatoryNonUniqueTinyIntChanging(this, value) && base.OnMandatoryNonUniqueTinyIntChanging(value))
						{
							this._MandatoryNonUniqueTinyInt = value;
							base.OnMandatoryNonUniqueTinyIntChanged(oldValue);
						}
					}
				}
			}
			private int _MandatoryNonUniqueUnconstrainedDecimal;
			public sealed override int MandatoryNonUniqueUnconstrainedDecimal
			{
				get
				{
					return this._MandatoryNonUniqueUnconstrainedDecimal;
				}
				set
				{
					int oldValue = this._MandatoryNonUniqueUnconstrainedDecimal;
					if (oldValue != value)
					{
						if (this._Context.OnPersonMandatoryNonUniqueUnconstrainedDecimalChanging(this, value) && base.OnMandatoryNonUniqueUnconstrainedDecimalChanging(value))
						{
							this._MandatoryNonUniqueUnconstrainedDecimal = value;
							base.OnMandatoryNonUniqueUnconstrainedDecimalChanged(oldValue);
						}
					}
				}
			}
			private float _MandatoryNonUniqueUnconstrainedFloat;
			public sealed override float MandatoryNonUniqueUnconstrainedFloat
			{
				get
				{
					return this._MandatoryNonUniqueUnconstrainedFloat;
				}
				set
				{
					float oldValue = this._MandatoryNonUniqueUnconstrainedFloat;
					if (oldValue != value)
					{
						if (this._Context.OnPersonMandatoryNonUniqueUnconstrainedFloatChanging(this, value) && base.OnMandatoryNonUniqueUnconstrainedFloatChanging(value))
						{
							this._MandatoryNonUniqueUnconstrainedFloat = value;
							base.OnMandatoryNonUniqueUnconstrainedFloatChanged(oldValue);
						}
					}
				}
			}
			private Person _Wife;
			public sealed override Person Wife
			{
				get
				{
					return this._Wife;
				}
				set
				{
					Person oldValue = this._Wife;
					if ((object)oldValue != (object)value)
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
			private ValueType1 _ValueType1DoesSomethingElseWith;
			public sealed override ValueType1 ValueType1DoesSomethingElseWith
			{
				get
				{
					return this._ValueType1DoesSomethingElseWith;
				}
				set
				{
					ValueType1 oldValue = this._ValueType1DoesSomethingElseWith;
					if ((object)oldValue != (object)value)
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
			private MalePerson _MalePerson;
			public sealed override MalePerson MalePerson
			{
				get
				{
					return this._MalePerson;
				}
				set
				{
					MalePerson oldValue = this._MalePerson;
					if ((object)oldValue != (object)value)
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
			private FemalePerson _FemalePerson;
			public sealed override FemalePerson FemalePerson
			{
				get
				{
					return this._FemalePerson;
				}
				set
				{
					FemalePerson oldValue = this._FemalePerson;
					if ((object)oldValue != (object)value)
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
			private ChildPerson _ChildPerson;
			public sealed override ChildPerson ChildPerson
			{
				get
				{
					return this._ChildPerson;
				}
				set
				{
					ChildPerson oldValue = this._ChildPerson;
					if ((object)oldValue != (object)value)
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
			private Death _Death;
			public sealed override Death Death
			{
				get
				{
					return this._Death;
				}
				set
				{
					Death oldValue = this._Death;
					if ((object)oldValue != (object)value)
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
			private Person _HusbandViaWife;
			public sealed override Person HusbandViaWife
			{
				get
				{
					return this._HusbandViaWife;
				}
				set
				{
					Person oldValue = this._HusbandViaWife;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnPersonHusbandViaWifeChanging(this, value) && base.OnHusbandViaWifeChanging(value))
						{
							this._HusbandViaWife = value;
							this._Context.OnPersonHusbandViaWifeChanged(this, oldValue);
							base.OnHusbandViaWifeChanged(oldValue);
						}
					}
				}
			}
			private readonly IEnumerable<Task> _TaskViaPersonCollection;
			public sealed override IEnumerable<Task> TaskViaPersonCollection
			{
				get
				{
					return this._TaskViaPersonCollection;
				}
			}
			private readonly IEnumerable<ValueType1> _ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
			public sealed override IEnumerable<ValueType1> ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection
			{
				get
				{
					return this._ValueType1DoesSomethingWithViaDoesSomethingWithPersonCollection;
				}
			}
			private readonly IEnumerable<PersonDrivesCar> _PersonDrivesCarViaDrivenByPersonCollection;
			public sealed override IEnumerable<PersonDrivesCar> PersonDrivesCarViaDrivenByPersonCollection
			{
				get
				{
					return this._PersonDrivesCarViaDrivenByPersonCollection;
				}
			}
			private readonly IEnumerable<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateViaBuyerCollection;
			public sealed override IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaBuyerCollection
			{
				get
				{
					return this._PersonBoughtCarFromPersonOnDateViaBuyerCollection;
				}
			}
			private readonly IEnumerable<PersonBoughtCarFromPersonOnDate> _PersonBoughtCarFromPersonOnDateViaSellerCollection;
			public sealed override IEnumerable<PersonBoughtCarFromPersonOnDate> PersonBoughtCarFromPersonOnDateViaSellerCollection
			{
				get
				{
					return this._PersonBoughtCarFromPersonOnDateViaSellerCollection;
				}
			}
			private readonly IEnumerable<PersonHasNickName> _PersonHasNickNameViaPersonCollection;
			public sealed override IEnumerable<PersonHasNickName> PersonHasNickNameViaPersonCollection
			{
				get
				{
					return this._PersonHasNickNameViaPersonCollection;
				}
			}
		}
		#endregion // PersonImpl
		#endregion // Person
		#region Task
		public Task CreateTask(Person person)
		{
			if ((object)person == null)
			{
				throw new ArgumentNullException("person");
			}
			if (!this.OnTaskPersonChanging(null, person))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("person");
			}
			return new TaskImpl(this, person);
		}
		private bool OnTaskPersonChanging(Task instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnTaskPersonChanged(Task instance, Person oldValue)
		{
			((ICollection<Task>)instance.Person.TaskViaPersonCollection).Add(instance);
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
		#region TaskImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class TaskImpl : Task
		{
			public TaskImpl(SampleModelContext context, Person person)
			{
				this._Context = context;
				this._Person = person;
				context.OnTaskPersonChanged(this, null);
				context._TaskList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private Person _Person;
			public sealed override Person Person
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
					if ((object)oldValue != (object)value)
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
		#endregion // TaskImpl
		#endregion // Task
		#region ValueType1
		public ValueType1 CreateValueType1(int valueType1Value)
		{
			if (!this.OnValueType1ValueType1ValueChanging(null, valueType1Value))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("valueType1Value");
			}
			return new ValueType1Impl(this, valueType1Value);
		}
		private bool OnValueType1ValueType1ValueChanging(ValueType1 instance, int newValue)
		{
			ValueType1 currentInstance;
			if (this._ValueType1ValueType1ValueDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != (object)instance)
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
				this._ValueType1ValueType1ValueDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnValueType1DoesSomethingWithPersonChanging(ValueType1 instance, Person newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
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
		private bool OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdding(ValueType1 instance, Person item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionAdded(ValueType1 instance, Person item)
		{
			item.ValueType1DoesSomethingElseWith = instance;
		}
		private void OnValueType1DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollectionRemoved(ValueType1 instance, Person item)
		{
			if ((object)item.ValueType1DoesSomethingElseWith == (object)instance)
			{
				item.ValueType1DoesSomethingElseWith = null;
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
		#region ValueType1Impl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ValueType1Impl : ValueType1
		{
			public ValueType1Impl(SampleModelContext context, int valueType1Value)
			{
				this._Context = context;
				this._DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection = new ConstraintEnforcementCollection<ValueType1, Person>(this);
				this._ValueType1Value = valueType1Value;
				context.OnValueType1ValueType1ValueChanged(this, null);
				context._ValueType1List.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private int _ValueType1Value;
			public sealed override int ValueType1Value
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
			private Person _DoesSomethingWithPerson;
			public sealed override Person DoesSomethingWithPerson
			{
				get
				{
					return this._DoesSomethingWithPerson;
				}
				set
				{
					Person oldValue = this._DoesSomethingWithPerson;
					if ((object)oldValue != (object)value)
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
			private readonly IEnumerable<Person> _DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection;
			public sealed override IEnumerable<Person> DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection
			{
				get
				{
					return this._DoesSomethingElseWithPersonViaValueType1DoesSomethingElseWithCollection;
				}
			}
		}
		#endregion // ValueType1Impl
		#endregion // ValueType1
		#region Death
		public Death CreateDeath(string deathCauseType, Person person)
		{
			if ((object)deathCauseType == null)
			{
				throw new ArgumentNullException("deathCauseType");
			}
			if ((object)person == null)
			{
				throw new ArgumentNullException("person");
			}
			if (!this.OnDeathDeathCauseTypeChanging(null, deathCauseType))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("deathCauseType");
			}
			if (!this.OnDeathPersonChanging(null, person))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("person");
			}
			return new DeathImpl(this, deathCauseType, person);
		}
		private bool OnDeathYMDChanging(Death instance, Nullable<System.DateTime> newValue)
		{
			return true;
		}
		private bool OnDeathDeathCauseTypeChanging(Death instance, string newValue)
		{
			return true;
		}
		private bool OnDeathNaturalDeathChanging(Death instance, NaturalDeath newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
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
				if ((object)this != (object)newValue.Context)
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
			if ((object)this != (object)newValue.Context)
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
		#region DeathImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class DeathImpl : Death
		{
			public DeathImpl(SampleModelContext context, string deathCauseType, Person person)
			{
				this._Context = context;
				this._DeathCauseType = deathCauseType;
				this._Person = person;
				context.OnDeathPersonChanged(this, null);
				context._DeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private Nullable<System.DateTime> _YMD;
			public sealed override Nullable<System.DateTime> YMD
			{
				get
				{
					return this._YMD;
				}
				set
				{
					Nullable<System.DateTime> oldValue = this._YMD;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnDeathYMDChanging(this, value) && base.OnYMDChanging(value))
						{
							this._YMD = value;
							base.OnYMDChanged(oldValue);
						}
					}
				}
			}
			private string _DeathCauseType;
			public sealed override string DeathCauseType
			{
				get
				{
					return this._DeathCauseType;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._DeathCauseType;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnDeathDeathCauseTypeChanging(this, value) && base.OnDeathCauseTypeChanging(value))
						{
							this._DeathCauseType = value;
							base.OnDeathCauseTypeChanged(oldValue);
						}
					}
				}
			}
			private NaturalDeath _NaturalDeath;
			public sealed override NaturalDeath NaturalDeath
			{
				get
				{
					return this._NaturalDeath;
				}
				set
				{
					NaturalDeath oldValue = this._NaturalDeath;
					if ((object)oldValue != (object)value)
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
			private UnnaturalDeath _UnnaturalDeath;
			public sealed override UnnaturalDeath UnnaturalDeath
			{
				get
				{
					return this._UnnaturalDeath;
				}
				set
				{
					UnnaturalDeath oldValue = this._UnnaturalDeath;
					if ((object)oldValue != (object)value)
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
			private Person _Person;
			public sealed override Person Person
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
					if ((object)oldValue != (object)value)
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
		#endregion // DeathImpl
		#endregion // Death
		#region NaturalDeath
		public NaturalDeath CreateNaturalDeath(Death death)
		{
			if ((object)death == null)
			{
				throw new ArgumentNullException("death");
			}
			if (!this.OnNaturalDeathDeathChanging(null, death))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("death");
			}
			return new NaturalDeathImpl(this, death);
		}
		private bool OnNaturalDeathIsFromProstateCancerChanging(NaturalDeath instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnNaturalDeathDeathChanging(NaturalDeath instance, Death newValue)
		{
			if ((object)this != (object)newValue.Context)
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
		#region NaturalDeathImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class NaturalDeathImpl : NaturalDeath
		{
			public NaturalDeathImpl(SampleModelContext context, Death death)
			{
				this._Context = context;
				this._Death = death;
				context.OnNaturalDeathDeathChanged(this, null);
				context._NaturalDeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private Nullable<bool> _IsFromProstateCancer;
			public sealed override Nullable<bool> IsFromProstateCancer
			{
				get
				{
					return this._IsFromProstateCancer;
				}
				set
				{
					Nullable<bool> oldValue = this._IsFromProstateCancer;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnNaturalDeathIsFromProstateCancerChanging(this, value) && base.OnIsFromProstateCancerChanging(value))
						{
							this._IsFromProstateCancer = value;
							base.OnIsFromProstateCancerChanged(oldValue);
						}
					}
				}
			}
			private Death _Death;
			public sealed override Death Death
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
					if ((object)oldValue != (object)value)
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
		#endregion // NaturalDeathImpl
		#endregion // NaturalDeath
		#region UnnaturalDeath
		public UnnaturalDeath CreateUnnaturalDeath(Death death)
		{
			if ((object)death == null)
			{
				throw new ArgumentNullException("death");
			}
			if (!this.OnUnnaturalDeathDeathChanging(null, death))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("death");
			}
			return new UnnaturalDeathImpl(this, death);
		}
		private bool OnUnnaturalDeathIsViolentChanging(UnnaturalDeath instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnUnnaturalDeathIsBloodyChanging(UnnaturalDeath instance, Nullable<bool> newValue)
		{
			return true;
		}
		private bool OnUnnaturalDeathDeathChanging(UnnaturalDeath instance, Death newValue)
		{
			if ((object)this != (object)newValue.Context)
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
		#region UnnaturalDeathImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class UnnaturalDeathImpl : UnnaturalDeath
		{
			public UnnaturalDeathImpl(SampleModelContext context, Death death)
			{
				this._Context = context;
				this._Death = death;
				context.OnUnnaturalDeathDeathChanged(this, null);
				context._UnnaturalDeathList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private Nullable<bool> _IsViolent;
			public sealed override Nullable<bool> IsViolent
			{
				get
				{
					return this._IsViolent;
				}
				set
				{
					Nullable<bool> oldValue = this._IsViolent;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnUnnaturalDeathIsViolentChanging(this, value) && base.OnIsViolentChanging(value))
						{
							this._IsViolent = value;
							base.OnIsViolentChanged(oldValue);
						}
					}
				}
			}
			private Nullable<bool> _IsBloody;
			public sealed override Nullable<bool> IsBloody
			{
				get
				{
					return this._IsBloody;
				}
				set
				{
					Nullable<bool> oldValue = this._IsBloody;
					if (oldValue.GetValueOrDefault() != value.GetValueOrDefault() || oldValue.HasValue != value.HasValue)
					{
						if (this._Context.OnUnnaturalDeathIsBloodyChanging(this, value) && base.OnIsBloodyChanging(value))
						{
							this._IsBloody = value;
							base.OnIsBloodyChanged(oldValue);
						}
					}
				}
			}
			private Death _Death;
			public sealed override Death Death
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
					if ((object)oldValue != (object)value)
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
		#endregion // UnnaturalDeathImpl
		#endregion // UnnaturalDeath
		#region MalePerson
		public MalePerson CreateMalePerson(Person person)
		{
			if ((object)person == null)
			{
				throw new ArgumentNullException("person");
			}
			if (!this.OnMalePersonPersonChanging(null, person))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("person");
			}
			return new MalePersonImpl(this, person);
		}
		private bool OnMalePersonPersonChanging(MalePerson instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
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
		private bool OnMalePersonChildPersonViaFatherCollectionAdding(MalePerson instance, ChildPerson item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnMalePersonChildPersonViaFatherCollectionAdded(MalePerson instance, ChildPerson item)
		{
			item.Father = instance;
		}
		private void OnMalePersonChildPersonViaFatherCollectionRemoved(MalePerson instance, ChildPerson item)
		{
			if ((object)item.Father == (object)instance)
			{
				item.Father = null;
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
		#region MalePersonImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class MalePersonImpl : MalePerson
		{
			public MalePersonImpl(SampleModelContext context, Person person)
			{
				this._Context = context;
				this._ChildPersonViaFatherCollection = new ConstraintEnforcementCollection<MalePerson, ChildPerson>(this);
				this._Person = person;
				context.OnMalePersonPersonChanged(this, null);
				context._MalePersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private Person _Person;
			public sealed override Person Person
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
					if ((object)oldValue != (object)value)
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
			private readonly IEnumerable<ChildPerson> _ChildPersonViaFatherCollection;
			public sealed override IEnumerable<ChildPerson> ChildPersonViaFatherCollection
			{
				get
				{
					return this._ChildPersonViaFatherCollection;
				}
			}
		}
		#endregion // MalePersonImpl
		#endregion // MalePerson
		#region FemalePerson
		public FemalePerson CreateFemalePerson(Person person)
		{
			if ((object)person == null)
			{
				throw new ArgumentNullException("person");
			}
			if (!this.OnFemalePersonPersonChanging(null, person))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("person");
			}
			return new FemalePersonImpl(this, person);
		}
		private bool OnFemalePersonPersonChanging(FemalePerson instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
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
		private bool OnFemalePersonChildPersonViaMotherCollectionAdding(FemalePerson instance, ChildPerson item)
		{
			if ((object)this != (object)item.Context)
			{
				throw SampleModelContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnFemalePersonChildPersonViaMotherCollectionAdded(FemalePerson instance, ChildPerson item)
		{
			item.Mother = instance;
		}
		private void OnFemalePersonChildPersonViaMotherCollectionRemoved(FemalePerson instance, ChildPerson item)
		{
			if ((object)item.Mother == (object)instance)
			{
				item.Mother = null;
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
		#region FemalePersonImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class FemalePersonImpl : FemalePerson
		{
			public FemalePersonImpl(SampleModelContext context, Person person)
			{
				this._Context = context;
				this._ChildPersonViaMotherCollection = new ConstraintEnforcementCollection<FemalePerson, ChildPerson>(this);
				this._Person = person;
				context.OnFemalePersonPersonChanged(this, null);
				context._FemalePersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private Person _Person;
			public sealed override Person Person
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
					if ((object)oldValue != (object)value)
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
			private readonly IEnumerable<ChildPerson> _ChildPersonViaMotherCollection;
			public sealed override IEnumerable<ChildPerson> ChildPersonViaMotherCollection
			{
				get
				{
					return this._ChildPersonViaMotherCollection;
				}
			}
		}
		#endregion // FemalePersonImpl
		#endregion // FemalePerson
		#region ChildPerson
		public ChildPerson CreateChildPerson(uint birthOrderNr, MalePerson father, FemalePerson mother, Person person)
		{
			if ((object)father == null)
			{
				throw new ArgumentNullException("father");
			}
			if ((object)mother == null)
			{
				throw new ArgumentNullException("mother");
			}
			if ((object)person == null)
			{
				throw new ArgumentNullException("person");
			}
			if (!this.OnChildPersonBirthOrderNrChanging(null, birthOrderNr))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("birthOrderNr");
			}
			if (!this.OnChildPersonFatherChanging(null, father))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("father");
			}
			if (!this.OnChildPersonMotherChanging(null, mother))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("mother");
			}
			if (!this.OnChildPersonPersonChanging(null, person))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("person");
			}
			return new ChildPersonImpl(this, birthOrderNr, father, mother, person);
		}
		private bool OnChildPersonBirthOrderNrChanging(ChildPerson instance, uint newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnFatherAndBirthOrderNrAndMotherChanging(instance, Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, newValue, instance.Mother)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonBirthOrderNrChanged(ChildPerson instance, Nullable<uint> oldValue)
		{
			Tuple<MalePerson, uint, FemalePerson> FatherAndBirthOrderNrAndMotherOldValueTuple;
			if (oldValue.HasValue)
			{
				FatherAndBirthOrderNrAndMotherOldValueTuple = Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, oldValue.GetValueOrDefault(), instance.Mother);
			}
			else
			{
				FatherAndBirthOrderNrAndMotherOldValueTuple = null;
			}
			this.OnFatherAndBirthOrderNrAndMotherChanged(instance, FatherAndBirthOrderNrAndMotherOldValueTuple, Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, instance.BirthOrderNr, instance.Mother));
		}
		private bool OnChildPersonFatherChanging(ChildPerson instance, MalePerson newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnFatherAndBirthOrderNrAndMotherChanging(instance, Tuple.CreateTuple<MalePerson, uint, FemalePerson>(newValue, instance.BirthOrderNr, instance.Mother)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonFatherChanged(ChildPerson instance, MalePerson oldValue)
		{
			((ICollection<ChildPerson>)instance.Father.ChildPersonViaFatherCollection).Add(instance);
			Tuple<MalePerson, uint, FemalePerson> FatherAndBirthOrderNrAndMotherOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<ChildPerson>)oldValue.ChildPersonViaFatherCollection).Remove(instance);
				FatherAndBirthOrderNrAndMotherOldValueTuple = Tuple.CreateTuple<MalePerson, uint, FemalePerson>(oldValue, instance.BirthOrderNr, instance.Mother);
			}
			else
			{
				FatherAndBirthOrderNrAndMotherOldValueTuple = null;
			}
			this.OnFatherAndBirthOrderNrAndMotherChanged(instance, FatherAndBirthOrderNrAndMotherOldValueTuple, Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, instance.BirthOrderNr, instance.Mother));
		}
		private bool OnChildPersonMotherChanging(ChildPerson instance, FemalePerson newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnFatherAndBirthOrderNrAndMotherChanging(instance, Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, instance.BirthOrderNr, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnChildPersonMotherChanged(ChildPerson instance, FemalePerson oldValue)
		{
			((ICollection<ChildPerson>)instance.Mother.ChildPersonViaMotherCollection).Add(instance);
			Tuple<MalePerson, uint, FemalePerson> FatherAndBirthOrderNrAndMotherOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<ChildPerson>)oldValue.ChildPersonViaMotherCollection).Remove(instance);
				FatherAndBirthOrderNrAndMotherOldValueTuple = Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, instance.BirthOrderNr, oldValue);
			}
			else
			{
				FatherAndBirthOrderNrAndMotherOldValueTuple = null;
			}
			this.OnFatherAndBirthOrderNrAndMotherChanged(instance, FatherAndBirthOrderNrAndMotherOldValueTuple, Tuple.CreateTuple<MalePerson, uint, FemalePerson>(instance.Father, instance.BirthOrderNr, instance.Mother));
		}
		private bool OnChildPersonPersonChanging(ChildPerson instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
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
		#region ChildPersonImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ChildPersonImpl : ChildPerson
		{
			public ChildPersonImpl(SampleModelContext context, uint birthOrderNr, MalePerson father, FemalePerson mother, Person person)
			{
				this._Context = context;
				this._BirthOrderNr = birthOrderNr;
				context.OnChildPersonBirthOrderNrChanged(this, null);
				this._Father = father;
				context.OnChildPersonFatherChanged(this, null);
				this._Mother = mother;
				context.OnChildPersonMotherChanged(this, null);
				this._Person = person;
				context.OnChildPersonPersonChanged(this, null);
				context._ChildPersonList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private uint _BirthOrderNr;
			public sealed override uint BirthOrderNr
			{
				get
				{
					return this._BirthOrderNr;
				}
				set
				{
					uint oldValue = this._BirthOrderNr;
					if (oldValue != value)
					{
						if (this._Context.OnChildPersonBirthOrderNrChanging(this, value) && base.OnBirthOrderNrChanging(value))
						{
							this._BirthOrderNr = value;
							this._Context.OnChildPersonBirthOrderNrChanged(this, oldValue);
							base.OnBirthOrderNrChanged(oldValue);
						}
					}
				}
			}
			private MalePerson _Father;
			public sealed override MalePerson Father
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
					if ((object)oldValue != (object)value)
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
			private FemalePerson _Mother;
			public sealed override FemalePerson Mother
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
					if ((object)oldValue != (object)value)
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
			private Person _Person;
			public sealed override Person Person
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
					if ((object)oldValue != (object)value)
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
		#endregion // ChildPersonImpl
		#endregion // ChildPerson
		#region PersonDrivesCar
		public PersonDrivesCar CreatePersonDrivesCar(uint vin, Person drivenByPerson)
		{
			if ((object)drivenByPerson == null)
			{
				throw new ArgumentNullException("drivenByPerson");
			}
			if (!this.OnPersonDrivesCarVinChanging(null, vin))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("vin");
			}
			if (!this.OnPersonDrivesCarDrivenByPersonChanging(null, drivenByPerson))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("drivenByPerson");
			}
			return new PersonDrivesCarImpl(this, vin, drivenByPerson);
		}
		private bool OnPersonDrivesCarVinChanging(PersonDrivesCar instance, uint newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnVinAndDrivenByPersonChanging(instance, Tuple.CreateTuple<uint, Person>(newValue, instance.DrivenByPerson)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonDrivesCarVinChanged(PersonDrivesCar instance, Nullable<uint> oldValue)
		{
			Tuple<uint, Person> VinAndDrivenByPersonOldValueTuple;
			if (oldValue.HasValue)
			{
				VinAndDrivenByPersonOldValueTuple = Tuple.CreateTuple<uint, Person>(oldValue.GetValueOrDefault(), instance.DrivenByPerson);
			}
			else
			{
				VinAndDrivenByPersonOldValueTuple = null;
			}
			this.OnVinAndDrivenByPersonChanged(instance, VinAndDrivenByPersonOldValueTuple, Tuple.CreateTuple<uint, Person>(instance.Vin, instance.DrivenByPerson));
		}
		private bool OnPersonDrivesCarDrivenByPersonChanging(PersonDrivesCar instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnVinAndDrivenByPersonChanging(instance, Tuple.CreateTuple<uint, Person>(instance.Vin, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonDrivesCarDrivenByPersonChanged(PersonDrivesCar instance, Person oldValue)
		{
			((ICollection<PersonDrivesCar>)instance.DrivenByPerson.PersonDrivesCarViaDrivenByPersonCollection).Add(instance);
			Tuple<uint, Person> VinAndDrivenByPersonOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<PersonDrivesCar>)oldValue.PersonDrivesCarViaDrivenByPersonCollection).Remove(instance);
				VinAndDrivenByPersonOldValueTuple = Tuple.CreateTuple<uint, Person>(instance.Vin, oldValue);
			}
			else
			{
				VinAndDrivenByPersonOldValueTuple = null;
			}
			this.OnVinAndDrivenByPersonChanged(instance, VinAndDrivenByPersonOldValueTuple, Tuple.CreateTuple<uint, Person>(instance.Vin, instance.DrivenByPerson));
		}
		private readonly List<PersonDrivesCar> _PersonDrivesCarList;
		private readonly ReadOnlyCollection<PersonDrivesCar> _PersonDrivesCarReadOnlyCollection;
		public IEnumerable<PersonDrivesCar> PersonDrivesCarCollection
		{
			get
			{
				return this._PersonDrivesCarReadOnlyCollection;
			}
		}
		#region PersonDrivesCarImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class PersonDrivesCarImpl : PersonDrivesCar
		{
			public PersonDrivesCarImpl(SampleModelContext context, uint vin, Person drivenByPerson)
			{
				this._Context = context;
				this._Vin = vin;
				context.OnPersonDrivesCarVinChanged(this, null);
				this._DrivenByPerson = drivenByPerson;
				context.OnPersonDrivesCarDrivenByPersonChanged(this, null);
				context._PersonDrivesCarList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private uint _Vin;
			public sealed override uint Vin
			{
				get
				{
					return this._Vin;
				}
				set
				{
					uint oldValue = this._Vin;
					if (oldValue != value)
					{
						if (this._Context.OnPersonDrivesCarVinChanging(this, value) && base.OnVinChanging(value))
						{
							this._Vin = value;
							this._Context.OnPersonDrivesCarVinChanged(this, oldValue);
							base.OnVinChanged(oldValue);
						}
					}
				}
			}
			private Person _DrivenByPerson;
			public sealed override Person DrivenByPerson
			{
				get
				{
					return this._DrivenByPerson;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					Person oldValue = this._DrivenByPerson;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnPersonDrivesCarDrivenByPersonChanging(this, value) && base.OnDrivenByPersonChanging(value))
						{
							this._DrivenByPerson = value;
							this._Context.OnPersonDrivesCarDrivenByPersonChanged(this, oldValue);
							base.OnDrivenByPersonChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonDrivesCarImpl
		#endregion // PersonDrivesCar
		#region PersonBoughtCarFromPersonOnDate
		public PersonBoughtCarFromPersonOnDate CreatePersonBoughtCarFromPersonOnDate(uint vin, System.DateTime YMD, Person buyer, Person seller)
		{
			if ((object)buyer == null)
			{
				throw new ArgumentNullException("buyer");
			}
			if ((object)seller == null)
			{
				throw new ArgumentNullException("seller");
			}
			if (!this.OnPersonBoughtCarFromPersonOnDateVinChanging(null, vin))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("vin");
			}
			if (!this.OnPersonBoughtCarFromPersonOnDateYMDChanging(null, YMD))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("YMD");
			}
			if (!this.OnPersonBoughtCarFromPersonOnDateBuyerChanging(null, buyer))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("buyer");
			}
			if (!this.OnPersonBoughtCarFromPersonOnDateSellerChanging(null, seller))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("seller");
			}
			return new PersonBoughtCarFromPersonOnDateImpl(this, vin, YMD, buyer, seller);
		}
		private bool OnPersonBoughtCarFromPersonOnDateVinChanging(PersonBoughtCarFromPersonOnDate instance, uint newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnBuyerAndVinAndSellerChanging(instance, Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, newValue, instance.Seller)))
				{
					return false;
				}
				if (!this.OnVinAndYMDAndBuyerChanging(instance, Tuple.CreateTuple<uint, System.DateTime, Person>(newValue, instance.YMD, instance.Buyer)))
				{
					return false;
				}
				if (!this.OnYMDAndSellerAndVinChanging(instance, Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, instance.Seller, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateVinChanged(PersonBoughtCarFromPersonOnDate instance, Nullable<uint> oldValue)
		{
			Tuple<Person, uint, Person> BuyerAndVinAndSellerOldValueTuple;
			Tuple<uint, System.DateTime, Person> VinAndYMDAndBuyerOldValueTuple;
			Tuple<System.DateTime, Person, uint> YMDAndSellerAndVinOldValueTuple;
			if (oldValue.HasValue)
			{
				BuyerAndVinAndSellerOldValueTuple = Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, oldValue.GetValueOrDefault(), instance.Seller);
				VinAndYMDAndBuyerOldValueTuple = Tuple.CreateTuple<uint, System.DateTime, Person>(oldValue.GetValueOrDefault(), instance.YMD, instance.Buyer);
				YMDAndSellerAndVinOldValueTuple = Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, instance.Seller, oldValue.GetValueOrDefault());
			}
			else
			{
				BuyerAndVinAndSellerOldValueTuple = null;
				VinAndYMDAndBuyerOldValueTuple = null;
				YMDAndSellerAndVinOldValueTuple = null;
			}
			this.OnBuyerAndVinAndSellerChanged(instance, BuyerAndVinAndSellerOldValueTuple, Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, instance.Vin, instance.Seller));
			this.OnVinAndYMDAndBuyerChanged(instance, VinAndYMDAndBuyerOldValueTuple, Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, instance.YMD, instance.Buyer));
			this.OnYMDAndSellerAndVinChanged(instance, YMDAndSellerAndVinOldValueTuple, Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, instance.Seller, instance.Vin));
		}
		private bool OnPersonBoughtCarFromPersonOnDateYMDChanging(PersonBoughtCarFromPersonOnDate instance, System.DateTime newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnVinAndYMDAndBuyerChanging(instance, Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, newValue, instance.Buyer)))
				{
					return false;
				}
				if (!this.OnYMDAndSellerAndVinChanging(instance, Tuple.CreateTuple<System.DateTime, Person, uint>(newValue, instance.Seller, instance.Vin)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateYMDChanged(PersonBoughtCarFromPersonOnDate instance, Nullable<System.DateTime> oldValue)
		{
			Tuple<uint, System.DateTime, Person> VinAndYMDAndBuyerOldValueTuple;
			Tuple<System.DateTime, Person, uint> YMDAndSellerAndVinOldValueTuple;
			if (oldValue.HasValue)
			{
				VinAndYMDAndBuyerOldValueTuple = Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, oldValue.GetValueOrDefault(), instance.Buyer);
				YMDAndSellerAndVinOldValueTuple = Tuple.CreateTuple<System.DateTime, Person, uint>(oldValue.GetValueOrDefault(), instance.Seller, instance.Vin);
			}
			else
			{
				VinAndYMDAndBuyerOldValueTuple = null;
				YMDAndSellerAndVinOldValueTuple = null;
			}
			this.OnVinAndYMDAndBuyerChanged(instance, VinAndYMDAndBuyerOldValueTuple, Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, instance.YMD, instance.Buyer));
			this.OnYMDAndSellerAndVinChanged(instance, YMDAndSellerAndVinOldValueTuple, Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, instance.Seller, instance.Vin));
		}
		private bool OnPersonBoughtCarFromPersonOnDateBuyerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnBuyerAndVinAndSellerChanging(instance, Tuple.CreateTuple<Person, uint, Person>(newValue, instance.Vin, instance.Seller)))
				{
					return false;
				}
				if (!this.OnVinAndYMDAndBuyerChanging(instance, Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, instance.YMD, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateBuyerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
		{
			((ICollection<PersonBoughtCarFromPersonOnDate>)instance.Buyer.PersonBoughtCarFromPersonOnDateViaBuyerCollection).Add(instance);
			Tuple<Person, uint, Person> BuyerAndVinAndSellerOldValueTuple;
			Tuple<uint, System.DateTime, Person> VinAndYMDAndBuyerOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<PersonBoughtCarFromPersonOnDate>)oldValue.PersonBoughtCarFromPersonOnDateViaBuyerCollection).Remove(instance);
				BuyerAndVinAndSellerOldValueTuple = Tuple.CreateTuple<Person, uint, Person>(oldValue, instance.Vin, instance.Seller);
				VinAndYMDAndBuyerOldValueTuple = Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, instance.YMD, oldValue);
			}
			else
			{
				BuyerAndVinAndSellerOldValueTuple = null;
				VinAndYMDAndBuyerOldValueTuple = null;
			}
			this.OnBuyerAndVinAndSellerChanged(instance, BuyerAndVinAndSellerOldValueTuple, Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, instance.Vin, instance.Seller));
			this.OnVinAndYMDAndBuyerChanged(instance, VinAndYMDAndBuyerOldValueTuple, Tuple.CreateTuple<uint, System.DateTime, Person>(instance.Vin, instance.YMD, instance.Buyer));
		}
		private bool OnPersonBoughtCarFromPersonOnDateSellerChanging(PersonBoughtCarFromPersonOnDate instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnBuyerAndVinAndSellerChanging(instance, Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, instance.Vin, newValue)))
				{
					return false;
				}
				if (!this.OnYMDAndSellerAndVinChanging(instance, Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, newValue, instance.Vin)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonBoughtCarFromPersonOnDateSellerChanged(PersonBoughtCarFromPersonOnDate instance, Person oldValue)
		{
			((ICollection<PersonBoughtCarFromPersonOnDate>)instance.Seller.PersonBoughtCarFromPersonOnDateViaSellerCollection).Add(instance);
			Tuple<Person, uint, Person> BuyerAndVinAndSellerOldValueTuple;
			Tuple<System.DateTime, Person, uint> YMDAndSellerAndVinOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<PersonBoughtCarFromPersonOnDate>)oldValue.PersonBoughtCarFromPersonOnDateViaSellerCollection).Remove(instance);
				BuyerAndVinAndSellerOldValueTuple = Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, instance.Vin, oldValue);
				YMDAndSellerAndVinOldValueTuple = Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, oldValue, instance.Vin);
			}
			else
			{
				BuyerAndVinAndSellerOldValueTuple = null;
				YMDAndSellerAndVinOldValueTuple = null;
			}
			this.OnBuyerAndVinAndSellerChanged(instance, BuyerAndVinAndSellerOldValueTuple, Tuple.CreateTuple<Person, uint, Person>(instance.Buyer, instance.Vin, instance.Seller));
			this.OnYMDAndSellerAndVinChanged(instance, YMDAndSellerAndVinOldValueTuple, Tuple.CreateTuple<System.DateTime, Person, uint>(instance.YMD, instance.Seller, instance.Vin));
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
		#region PersonBoughtCarFromPersonOnDateImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class PersonBoughtCarFromPersonOnDateImpl : PersonBoughtCarFromPersonOnDate
		{
			public PersonBoughtCarFromPersonOnDateImpl(SampleModelContext context, uint vin, System.DateTime YMD, Person buyer, Person seller)
			{
				this._Context = context;
				this._Vin = vin;
				context.OnPersonBoughtCarFromPersonOnDateVinChanged(this, null);
				this._YMD = YMD;
				context.OnPersonBoughtCarFromPersonOnDateYMDChanged(this, null);
				this._Buyer = buyer;
				context.OnPersonBoughtCarFromPersonOnDateBuyerChanged(this, null);
				this._Seller = seller;
				context.OnPersonBoughtCarFromPersonOnDateSellerChanged(this, null);
				context._PersonBoughtCarFromPersonOnDateList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private uint _Vin;
			public sealed override uint Vin
			{
				get
				{
					return this._Vin;
				}
				set
				{
					uint oldValue = this._Vin;
					if (oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateVinChanging(this, value) && base.OnVinChanging(value))
						{
							this._Vin = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateVinChanged(this, oldValue);
							base.OnVinChanged(oldValue);
						}
					}
				}
			}
			private System.DateTime _YMD;
			public sealed override System.DateTime YMD
			{
				get
				{
					return this._YMD;
				}
				set
				{
					System.DateTime oldValue = this._YMD;
					if (oldValue != value)
					{
						if (this._Context.OnPersonBoughtCarFromPersonOnDateYMDChanging(this, value) && base.OnYMDChanging(value))
						{
							this._YMD = value;
							this._Context.OnPersonBoughtCarFromPersonOnDateYMDChanged(this, oldValue);
							base.OnYMDChanged(oldValue);
						}
					}
				}
			}
			private Person _Buyer;
			public sealed override Person Buyer
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
					if ((object)oldValue != (object)value)
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
			private Person _Seller;
			public sealed override Person Seller
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
					if ((object)oldValue != (object)value)
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
		#endregion // PersonBoughtCarFromPersonOnDateImpl
		#endregion // PersonBoughtCarFromPersonOnDate
		#region Review
		public Review CreateReview(uint vin, uint integer, string name)
		{
			if ((object)name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.OnReviewVinChanging(null, vin))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("vin");
			}
			if (!this.OnReviewIntegerChanging(null, integer))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("integer");
			}
			if (!this.OnReviewNameChanging(null, name))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("name");
			}
			return new ReviewImpl(this, vin, integer, name);
		}
		private bool OnReviewVinChanging(Review instance, uint newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnVinAndNameChanging(instance, Tuple.CreateTuple<uint, string>(newValue, instance.Name)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnReviewVinChanged(Review instance, Nullable<uint> oldValue)
		{
			Tuple<uint, string> VinAndNameOldValueTuple;
			if (oldValue.HasValue)
			{
				VinAndNameOldValueTuple = Tuple.CreateTuple<uint, string>(oldValue.GetValueOrDefault(), instance.Name);
			}
			else
			{
				VinAndNameOldValueTuple = null;
			}
			this.OnVinAndNameChanged(instance, VinAndNameOldValueTuple, Tuple.CreateTuple<uint, string>(instance.Vin, instance.Name));
		}
		private bool OnReviewIntegerChanging(Review instance, uint newValue)
		{
			return true;
		}
		private bool OnReviewNameChanging(Review instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnVinAndNameChanging(instance, Tuple.CreateTuple<uint, string>(instance.Vin, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnReviewNameChanged(Review instance, string oldValue)
		{
			Tuple<uint, string> VinAndNameOldValueTuple;
			if ((object)oldValue != null)
			{
				VinAndNameOldValueTuple = Tuple.CreateTuple<uint, string>(instance.Vin, oldValue);
			}
			else
			{
				VinAndNameOldValueTuple = null;
			}
			this.OnVinAndNameChanged(instance, VinAndNameOldValueTuple, Tuple.CreateTuple<uint, string>(instance.Vin, instance.Name));
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
		#region ReviewImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ReviewImpl : Review
		{
			public ReviewImpl(SampleModelContext context, uint vin, uint integer, string name)
			{
				this._Context = context;
				this._Vin = vin;
				context.OnReviewVinChanged(this, null);
				this._Integer = integer;
				this._Name = name;
				context.OnReviewNameChanged(this, null);
				context._ReviewList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private uint _Vin;
			public sealed override uint Vin
			{
				get
				{
					return this._Vin;
				}
				set
				{
					uint oldValue = this._Vin;
					if (oldValue != value)
					{
						if (this._Context.OnReviewVinChanging(this, value) && base.OnVinChanging(value))
						{
							this._Vin = value;
							this._Context.OnReviewVinChanged(this, oldValue);
							base.OnVinChanged(oldValue);
						}
					}
				}
			}
			private uint _Integer;
			public sealed override uint Integer
			{
				get
				{
					return this._Integer;
				}
				set
				{
					uint oldValue = this._Integer;
					if (oldValue != value)
					{
						if (this._Context.OnReviewIntegerChanging(this, value) && base.OnIntegerChanging(value))
						{
							this._Integer = value;
							base.OnIntegerChanged(oldValue);
						}
					}
				}
			}
			private string _Name;
			public sealed override string Name
			{
				get
				{
					return this._Name;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Name;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnReviewNameChanging(this, value) && base.OnNameChanging(value))
						{
							this._Name = value;
							this._Context.OnReviewNameChanged(this, oldValue);
							base.OnNameChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // ReviewImpl
		#endregion // Review
		#region PersonHasNickName
		public PersonHasNickName CreatePersonHasNickName(string nickName, Person person)
		{
			if ((object)nickName == null)
			{
				throw new ArgumentNullException("nickName");
			}
			if ((object)person == null)
			{
				throw new ArgumentNullException("person");
			}
			if (!this.OnPersonHasNickNameNickNameChanging(null, nickName))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("nickName");
			}
			if (!this.OnPersonHasNickNamePersonChanging(null, person))
			{
				throw SampleModelContext.GetConstraintEnforcementFailedException("person");
			}
			return new PersonHasNickNameImpl(this, nickName, person);
		}
		private bool OnPersonHasNickNameNickNameChanging(PersonHasNickName instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnNickNameAndPersonChanging(instance, Tuple.CreateTuple<string, Person>(newValue, instance.Person)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonHasNickNameNickNameChanged(PersonHasNickName instance, string oldValue)
		{
			Tuple<string, Person> NickNameAndPersonOldValueTuple;
			if ((object)oldValue != null)
			{
				NickNameAndPersonOldValueTuple = Tuple.CreateTuple<string, Person>(oldValue, instance.Person);
			}
			else
			{
				NickNameAndPersonOldValueTuple = null;
			}
			this.OnNickNameAndPersonChanged(instance, NickNameAndPersonOldValueTuple, Tuple.CreateTuple<string, Person>(instance.NickName, instance.Person));
		}
		private bool OnPersonHasNickNamePersonChanging(PersonHasNickName instance, Person newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw SampleModelContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnNickNameAndPersonChanging(instance, Tuple.CreateTuple<string, Person>(instance.NickName, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnPersonHasNickNamePersonChanged(PersonHasNickName instance, Person oldValue)
		{
			((ICollection<PersonHasNickName>)instance.Person.PersonHasNickNameViaPersonCollection).Add(instance);
			Tuple<string, Person> NickNameAndPersonOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<PersonHasNickName>)oldValue.PersonHasNickNameViaPersonCollection).Remove(instance);
				NickNameAndPersonOldValueTuple = Tuple.CreateTuple<string, Person>(instance.NickName, oldValue);
			}
			else
			{
				NickNameAndPersonOldValueTuple = null;
			}
			this.OnNickNameAndPersonChanged(instance, NickNameAndPersonOldValueTuple, Tuple.CreateTuple<string, Person>(instance.NickName, instance.Person));
		}
		private readonly List<PersonHasNickName> _PersonHasNickNameList;
		private readonly ReadOnlyCollection<PersonHasNickName> _PersonHasNickNameReadOnlyCollection;
		public IEnumerable<PersonHasNickName> PersonHasNickNameCollection
		{
			get
			{
				return this._PersonHasNickNameReadOnlyCollection;
			}
		}
		#region PersonHasNickNameImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class PersonHasNickNameImpl : PersonHasNickName
		{
			public PersonHasNickNameImpl(SampleModelContext context, string nickName, Person person)
			{
				this._Context = context;
				this._NickName = nickName;
				context.OnPersonHasNickNameNickNameChanged(this, null);
				this._Person = person;
				context.OnPersonHasNickNamePersonChanged(this, null);
				context._PersonHasNickNameList.Add(this);
			}
			private readonly SampleModelContext _Context;
			public sealed override SampleModelContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private string _NickName;
			public sealed override string NickName
			{
				get
				{
					return this._NickName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._NickName;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnPersonHasNickNameNickNameChanging(this, value) && base.OnNickNameChanging(value))
						{
							this._NickName = value;
							this._Context.OnPersonHasNickNameNickNameChanged(this, oldValue);
							base.OnNickNameChanged(oldValue);
						}
					}
				}
			}
			private Person _Person;
			public sealed override Person Person
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
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnPersonHasNickNamePersonChanging(this, value) && base.OnPersonChanging(value))
						{
							this._Person = value;
							this._Context.OnPersonHasNickNamePersonChanged(this, oldValue);
							base.OnPersonChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // PersonHasNickNameImpl
		#endregion // PersonHasNickName
	}
	#endregion // SampleModelContext
}

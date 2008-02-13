<?php
class PersonDrivesCarBase extends Entity {
	private $PersonDrivesCar_DrivenByPerson_Person_Proxy;
	private $DrivesCar_vin;
	public function __construct() {
		parent::__construct();
		$this->PersonDrivesCar_DrivenByPerson_Person_Proxy = new PersonDrivesCar_DrivenByPerson_Person_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("DrivesCar_vin"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("vin", 0, ValueRangeValidatorClusivity::$inclusive, 4294967295, ValueRangeValidatorClusivity::$inclusive));
	}
	public function setDrivenByPerson(Person $value) {
		$this->PersonDrivesCar_DrivenByPerson_Person_Proxy->Set($value);
	}
	public function getDrivenByPerson() {
		return $this->PersonDrivesCar_DrivenByPerson_Person_Proxy->Get();
	}
	public function getDrivesCar_vin() {
		return $this->DrivesCar_vin;
	}
	public function setDrivesCar_vin(/*int*/ $value) {
		$this->DrivesCar_vin = $value;
	}
}
if (!class_exists('PersonDrivesCar')) {
	class PersonDrivesCar extends PersonDrivesCarBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonHasNickNameBase extends Entity {
	private $NickName;
	private $PersonHasNickName_Person_Person_Proxy;
	public function __construct() {
		parent::__construct();
		$this->PersonHasNickName_Person_Person_Proxy = new PersonHasNickName_Person_Person_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("NickName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("NickName", null, 64));
	}
	public function getNickName() {
		return $this->NickName;
	}
	public function setNickName(/*string*/ $value) {
		$this->NickName = $value;
	}
	public function setPerson(Person $value) {
		$this->PersonHasNickName_Person_Person_Proxy->Set($value);
	}
	public function getPerson() {
		return $this->PersonHasNickName_Person_Person_Proxy->Get();
	}
}
if (!class_exists('PersonHasNickName')) {
	class PersonHasNickName extends PersonHasNickNameBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonBase extends Entity {
	private $MalePerson;
	private $FemalePerson;
	private $ChildPerson;
	private $FirstName;
	private $Person_id;
	private $Date_YMD;
	private $LastName;
	private $OptionalUniqueString;
	private $HatType_ColorARGB;
	private $HatType_HatTypeStyle_HatTypeStyle_Description;
	private $Wife_Husband_Person_Proxy;
	private $OwnsCar_vin;
	private $Death;
	private $Gender_Gender_Code;
	private $hasParents;
	private $DoesSomethingElseWithPerson_ValueType1DoesSomethingElseWith_ValueType1_Proxy;
	private $OptionalUniqueDecimal;
	private $MandatoryUniqueDecimal;
	private $MandatoryUniqueString;
	public function __construct() {
		parent::__construct();
		$this->Wife_Husband_Person_Proxy = new Wife_Husband_Person_Proxy($this);
		$this->DoesSomethingElseWithPerson_ValueType1DoesSomethingElseWith_ValueType1_Proxy = new DoesSomethingElseWithPerson_ValueType1DoesSomethingElseWith_ValueType1_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("FirstName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("FirstName", null, 64));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Person_id"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Date_YMD"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("LastName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("LastName", null, 64));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("OptionalUniqueString"));
		$this->validationRules->addValidationRule(new StringLenthValidator("OptionalUniqueString", 11, 11));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("HatType_ColorARGB"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("ColorARGB", -2147483648, ValueRangeValidatorClusivity::$inclusive, 2147483647, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("HatType_HatTypeStyle_HatTypeStyle_Description"));
		$this->validationRules->addValidationRule(new StringLenthValidator("HatTypeStyle_Description", null, 256));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("OwnsCar_vin"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("vin", 0, ValueRangeValidatorClusivity::$inclusive, 4294967295, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Gender_Gender_Code"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Gender_Code", 1, 1));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("hasParents"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("OptionalUniqueDecimal"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("MandatoryUniqueDecimal"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("MandatoryUniqueDecimal", 4000, ValueRangeValidatorClusivity::$inclusive, 20000, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("MandatoryUniqueString"));
		$this->validationRules->addValidationRule(new StringLenthValidator("MandatoryUniqueString", 11, 11));
	}
	public function getMalePerson() {
		return $this->MalePerson;
	}
	public function setMalePerson(MalePerson $value) {
		if ($this->MalePerson !== $value) {
			$this->MalePerson = $value;
			$value->setPerson($this);
		}
	}
	public function getFemalePerson() {
		return $this->FemalePerson;
	}
	public function setFemalePerson(FemalePerson $value) {
		if ($this->FemalePerson !== $value) {
			$this->FemalePerson = $value;
			$value->setPerson($this);
		}
	}
	public function getChildPerson() {
		return $this->ChildPerson;
	}
	public function setChildPerson(ChildPerson $value) {
		if ($this->ChildPerson !== $value) {
			$this->ChildPerson = $value;
			$value->setPerson($this);
		}
	}
	public function getDeath() {
		return $this->Death;
	}
	public function setDeath(Death $value) {
		if ($this->Death !== $value) {
			$this->Death = $value;
			$value->setPerson($this);
		}
	}
	public function getFirstName() {
		return $this->FirstName;
	}
	public function setFirstName(/*string*/ $value) {
		$this->FirstName = $value;
	}
	public function getPerson_id() {
		return $this->Person_id;
	}
	public function setPerson_id(/*object*/ $value) {
		$this->Person_id = $value;
	}
	public function getDate_YMD() {
		return $this->Date_YMD;
	}
	public function setDate_YMD(/*int*/ $value) {
		$this->Date_YMD = $value;
	}
	public function getLastName() {
		return $this->LastName;
	}
	public function setLastName(/*string*/ $value) {
		$this->LastName = $value;
	}
	public function getOptionalUniqueString() {
		return $this->OptionalUniqueString;
	}
	public function setOptionalUniqueString(/*string*/ $value) {
		$this->OptionalUniqueString = $value;
	}
	public function getHatType_ColorARGB() {
		return $this->HatType_ColorARGB;
	}
	public function setHatType_ColorARGB(/*int*/ $value) {
		$this->HatType_ColorARGB = $value;
	}
	public function getHatType_HatTypeStyle_HatTypeStyle_Description() {
		return $this->HatType_HatTypeStyle_HatTypeStyle_Description;
	}
	public function setHatType_HatTypeStyle_HatTypeStyle_Description(/*string*/ $value) {
		$this->HatType_HatTypeStyle_HatTypeStyle_Description = $value;
	}
	public function setHusband(Person $value) {
		$this->Wife_Husband_Person_Proxy->Set($value);
	}
	public function getHusband() {
		return $this->Wife_Husband_Person_Proxy->Get();
	}
	public function getOwnsCar_vin() {
		return $this->OwnsCar_vin;
	}
	public function setOwnsCar_vin(/*int*/ $value) {
		$this->OwnsCar_vin = $value;
	}
	public function getGender_Gender_Code() {
		return $this->Gender_Gender_Code;
	}
	public function setGender_Gender_Code(/*string*/ $value) {
		$this->Gender_Gender_Code = $value;
	}
	public function gethasParents() {
		return $this->hasParents;
	}
	public function sethasParents(/*bool*/ $value) {
		$this->hasParents = $value;
	}
	public function setValueType1DoesSomethingElseWith(ValueType1 $value) {
		$this->DoesSomethingElseWithPerson_ValueType1DoesSomethingElseWith_ValueType1_Proxy->Set($value);
	}
	public function getValueType1DoesSomethingElseWith() {
		return $this->DoesSomethingElseWithPerson_ValueType1DoesSomethingElseWith_ValueType1_Proxy->Get();
	}
	public function getOptionalUniqueDecimal() {
		return $this->OptionalUniqueDecimal;
	}
	public function setOptionalUniqueDecimal(/*decimal*/ $value) {
		$this->OptionalUniqueDecimal = $value;
	}
	public function getMandatoryUniqueDecimal() {
		return $this->MandatoryUniqueDecimal;
	}
	public function setMandatoryUniqueDecimal(/*decimal*/ $value) {
		$this->MandatoryUniqueDecimal = $value;
	}
	public function getMandatoryUniqueString() {
		return $this->MandatoryUniqueString;
	}
	public function setMandatoryUniqueString(/*string*/ $value) {
		$this->MandatoryUniqueString = $value;
	}
}
if (!class_exists('Person')) {
	class Person extends PersonBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class MalePersonBase extends Entity {
	private $Person;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
	}
	public function getPerson() {
		return $this->Person;
	}
	public function setPerson(Person $value) {
		if ($this->Person !== $value) {
			$this->Person = $value;
			$value->setMalePerson($this);
		}
	}
}
if (!class_exists('MalePerson')) {
	class MalePerson extends MalePersonBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class FemalePersonBase extends Entity {
	private $Person;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
	}
	public function getPerson() {
		return $this->Person;
	}
	public function setPerson(Person $value) {
		if ($this->Person !== $value) {
			$this->Person = $value;
			$value->setFemalePerson($this);
		}
	}
}
if (!class_exists('FemalePerson')) {
	class FemalePerson extends FemalePersonBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ChildPersonBase extends Entity {
	private $ChildPerson_Father_MalePerson_Proxy;
	private $BirthOrder_BirthOrder_Nr;
	private $ChildPerson_Mother_FemalePerson_Proxy;
	private $Person;
	public function __construct() {
		parent::__construct();
		$this->ChildPerson_Father_MalePerson_Proxy = new ChildPerson_Father_MalePerson_Proxy($this);
		$this->ChildPerson_Mother_FemalePerson_Proxy = new ChildPerson_Mother_FemalePerson_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("BirthOrder_BirthOrder_Nr"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("BirthOrder_Nr", 0, ValueRangeValidatorClusivity::$inclusive, 4294967295, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new ValueRangeValidator("BirthOrder_Nr", 1, ValueRangeValidatorClusivity::$inclusive, null, ValueRangeValidatorClusivity::$));
	}
	public function getPerson() {
		return $this->Person;
	}
	public function setPerson(Person $value) {
		if ($this->Person !== $value) {
			$this->Person = $value;
			$value->setChildPerson($this);
		}
	}
	public function setFather(MalePerson $value) {
		$this->ChildPerson_Father_MalePerson_Proxy->Set($value);
	}
	public function getFather() {
		return $this->ChildPerson_Father_MalePerson_Proxy->Get();
	}
	public function getBirthOrder_BirthOrder_Nr() {
		return $this->BirthOrder_BirthOrder_Nr;
	}
	public function setBirthOrder_BirthOrder_Nr(/*int*/ $value) {
		$this->BirthOrder_BirthOrder_Nr = $value;
	}
	public function setMother(FemalePerson $value) {
		$this->ChildPerson_Mother_FemalePerson_Proxy->Set($value);
	}
	public function getMother() {
		return $this->ChildPerson_Mother_FemalePerson_Proxy->Get();
	}
}
if (!class_exists('ChildPerson')) {
	class ChildPerson extends ChildPersonBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class DeathBase extends Entity {
	private $NaturalDeath;
	private $UnnaturalDeath;
	private $Date_YMD;
	private $DeathCause_DeathCause_Type;
	private $isDead;
	private $Person;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Date_YMD"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("DeathCause_DeathCause_Type"));
		$this->validationRules->addValidationRule(new StringLenthValidator("DeathCause_Type", null, 14));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("isDead"));
	}
	public function getPerson() {
		return $this->Person;
	}
	public function setPerson(Person $value) {
		if ($this->Person !== $value) {
			$this->Person = $value;
			$value->setDeath($this);
		}
	}
	public function getNaturalDeath() {
		return $this->NaturalDeath;
	}
	public function setNaturalDeath(NaturalDeath $value) {
		if ($this->NaturalDeath !== $value) {
			$this->NaturalDeath = $value;
			$value->setDeath($this);
		}
	}
	public function getUnnaturalDeath() {
		return $this->UnnaturalDeath;
	}
	public function setUnnaturalDeath(UnnaturalDeath $value) {
		if ($this->UnnaturalDeath !== $value) {
			$this->UnnaturalDeath = $value;
			$value->setDeath($this);
		}
	}
	public function getDate_YMD() {
		return $this->Date_YMD;
	}
	public function setDate_YMD(/*int*/ $value) {
		$this->Date_YMD = $value;
	}
	public function getDeathCause_DeathCause_Type() {
		return $this->DeathCause_DeathCause_Type;
	}
	public function setDeathCause_DeathCause_Type(/*string*/ $value) {
		$this->DeathCause_DeathCause_Type = $value;
	}
	public function getisDead() {
		return $this->isDead;
	}
	public function setisDead(/*bool*/ $value) {
		$this->isDead = $value;
	}
}
if (!class_exists('Death')) {
	class Death extends DeathBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class NaturalDeathBase extends Entity {
	private $isFromProstateCancer;
	private $Death;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("isFromProstateCancer"));
	}
	public function getDeath() {
		return $this->Death;
	}
	public function setDeath(Death $value) {
		if ($this->Death !== $value) {
			$this->Death = $value;
			$value->setNaturalDeath($this);
		}
	}
	public function getisFromProstateCancer() {
		return $this->isFromProstateCancer;
	}
	public function setisFromProstateCancer(/*bool*/ $value) {
		$this->isFromProstateCancer = $value;
	}
}
if (!class_exists('NaturalDeath')) {
	class NaturalDeath extends NaturalDeathBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class UnnaturalDeathBase extends Entity {
	private $isViolent;
	private $isBloody;
	private $Death;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("isViolent"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("isBloody"));
	}
	public function getDeath() {
		return $this->Death;
	}
	public function setDeath(Death $value) {
		if ($this->Death !== $value) {
			$this->Death = $value;
			$value->setUnnaturalDeath($this);
		}
	}
	public function getisViolent() {
		return $this->isViolent;
	}
	public function setisViolent(/*bool*/ $value) {
		$this->isViolent = $value;
	}
	public function getisBloody() {
		return $this->isBloody;
	}
	public function setisBloody(/*bool*/ $value) {
		$this->isBloody = $value;
	}
}
if (!class_exists('UnnaturalDeath')) {
	class UnnaturalDeath extends UnnaturalDeathBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class TaskBase extends Entity {
	private $Task_Person_Person_Proxy;
	private $Task_id;
	public function __construct() {
		parent::__construct();
		$this->Task_Person_Person_Proxy = new Task_Person_Person_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Task_id"));
	}
	public function setPerson(Person $value) {
		$this->Task_Person_Person_Proxy->Set($value);
	}
	public function getPerson() {
		return $this->Task_Person_Person_Proxy->Get();
	}
	public function getTask_id() {
		return $this->Task_id;
	}
	public function setTask_id(/*object*/ $value) {
		$this->Task_id = $value;
	}
}
if (!class_exists('Task')) {
	class Task extends TaskBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ValueType1Base extends Entity {
	private $ValueType1Value;
	private $ValueType1DoesSomethingWith_DoesSomethingWithPerson_Person_Proxy;
	public function __construct() {
		parent::__construct();
		$this->ValueType1DoesSomethingWith_DoesSomethingWithPerson_Person_Proxy = new ValueType1DoesSomethingWith_DoesSomethingWithPerson_Person_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("ValueType1Value"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("ValueType1", -2147483648, ValueRangeValidatorClusivity::$inclusive, 2147483647, ValueRangeValidatorClusivity::$inclusive));
	}
	public function getValueType1Value() {
		return $this->ValueType1Value;
	}
	public function setValueType1Value(/*int*/ $value) {
		$this->ValueType1Value = $value;
	}
	public function setDoesSomethingWithPerson(Person $value) {
		$this->ValueType1DoesSomethingWith_DoesSomethingWithPerson_Person_Proxy->Set($value);
	}
	public function getDoesSomethingWithPerson() {
		return $this->ValueType1DoesSomethingWith_DoesSomethingWithPerson_Person_Proxy->Get();
	}
}
if (!class_exists('ValueType1')) {
	class ValueType1 extends ValueType1Base {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonBoughtCarFromPersonOnDateBase extends Entity {
	private $PersonBoughtCarFromPersonOnDate_Buyer_Person_Proxy;
	private $CarSold_vin;
	private $PersonBoughtCarFromPersonOnDate_Seller_Person_Proxy;
	private $SaleDate_YMD;
	public function __construct() {
		parent::__construct();
		$this->PersonBoughtCarFromPersonOnDate_Buyer_Person_Proxy = new PersonBoughtCarFromPersonOnDate_Buyer_Person_Proxy($this);
		$this->PersonBoughtCarFromPersonOnDate_Seller_Person_Proxy = new PersonBoughtCarFromPersonOnDate_Seller_Person_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("CarSold_vin"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("vin", 0, ValueRangeValidatorClusivity::$inclusive, 4294967295, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("SaleDate_YMD"));
	}
	public function setBuyer(Person $value) {
		$this->PersonBoughtCarFromPersonOnDate_Buyer_Person_Proxy->Set($value);
	}
	public function getBuyer() {
		return $this->PersonBoughtCarFromPersonOnDate_Buyer_Person_Proxy->Get();
	}
	public function getCarSold_vin() {
		return $this->CarSold_vin;
	}
	public function setCarSold_vin(/*int*/ $value) {
		$this->CarSold_vin = $value;
	}
	public function setSeller(Person $value) {
		$this->PersonBoughtCarFromPersonOnDate_Seller_Person_Proxy->Set($value);
	}
	public function getSeller() {
		return $this->PersonBoughtCarFromPersonOnDate_Seller_Person_Proxy->Get();
	}
	public function getSaleDate_YMD() {
		return $this->SaleDate_YMD;
	}
	public function setSaleDate_YMD(/*int*/ $value) {
		$this->SaleDate_YMD = $value;
	}
}
if (!class_exists('PersonBoughtCarFromPersonOnDate')) {
	class PersonBoughtCarFromPersonOnDate extends PersonBoughtCarFromPersonOnDateBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ReviewBase extends Entity {
	private $Car_vin;
	private $Rating_Nr_Integer;
	private $Criterion_Name;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Car_vin"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("vin", 0, ValueRangeValidatorClusivity::$inclusive, 4294967295, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Rating_Nr_Integer"));
		$this->validationRules->addValidationRule(new ValueRangeValidator("Integer", 0, ValueRangeValidatorClusivity::$inclusive, 4294967295, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new ValueRangeValidator("Integer", 1, ValueRangeValidatorClusivity::$inclusive, 7, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new ValueRangeValidator("Integer", 14, ValueRangeValidatorClusivity::$inclusive, 16, ValueRangeValidatorClusivity::$inclusive));
		$this->validationRules->addValidationRule(new ValueRangeValidator("Integer", 18, ValueRangeValidatorClusivity::$inclusive, null, ValueRangeValidatorClusivity::$));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Criterion_Name"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Name", null, 64));
	}
	public function getCar_vin() {
		return $this->Car_vin;
	}
	public function setCar_vin(/*int*/ $value) {
		$this->Car_vin = $value;
	}
	public function getRating_Nr_Integer() {
		return $this->Rating_Nr_Integer;
	}
	public function setRating_Nr_Integer(/*int*/ $value) {
		$this->Rating_Nr_Integer = $value;
	}
	public function getCriterion_Name() {
		return $this->Criterion_Name;
	}
	public function setCriterion_Name(/*string*/ $value) {
		$this->Criterion_Name = $value;
	}
}
if (!class_exists('Review')) {
	class Review extends ReviewBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
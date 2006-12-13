<?php
.global::require_once("Entities.php");
.global::require_once("DataLayer.php");
class PersonServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new PersonService();
		}
		return instance;
	}
	
	public function getAll() {
		return PersonDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return PersonDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(Person $Person) {
		return PersonDAO::getInstance()->insert($Person);
	}
	
	public function update(Person $Person) {
		return PersonDAO::getInstance()->update($Person);
	}
	
	public function delete(Person $Person) {
		return PersonDAO::getInstance()->delete($Person);
	}
	// <summary>Retrieves a collection of Wife objects by the given Person object</summary>
	public function get_Wife_Collection_By_Husband(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_Wife_Collection_By_Husband($Person_id);
	}
	// <summary>Retrieves a collection of Task objects by the given Person object</summary>
	public function get_Task_Collection_By_Person(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_Task_Collection_By_Person($Person_id);
	}
	// <summary>Retrieves a collection of ValueType1DoesSomethingWith objects by the given Person object</summary>
	public function get_ValueType1DoesSomethingWith_Collection_By_DoesSomethingWithPerson(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_ValueType1DoesSomethingWith_Collection_By_DoesSomethingWithPerson($Person_id);
	}
	// <summary>Retrieves a collection of PersonBoughtCarFromPersonOnDate objects by the given Person object</summary>
	public function get_PersonBoughtCarFromPersonOnDate_Collection_By_Buyer(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_PersonBoughtCarFromPersonOnDate_Collection_By_Buyer($Person_id);
	}
	// <summary>Retrieves a collection of PersonBoughtCarFromPersonOnDate objects by the given Person object</summary>
	public function get_PersonBoughtCarFromPersonOnDate_Collection_By_Seller(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_PersonBoughtCarFromPersonOnDate_Collection_By_Seller($Person_id);
	}
}
if (!class_exists('PersonService')) {
	class PersonService extends PersonServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class MalePersonServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new MalePersonService();
		}
		return instance;
	}
	
	public function getAll() {
		return MalePersonDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return MalePersonDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(MalePerson $MalePerson) {
		return MalePersonDAO::getInstance()->insert($MalePerson);
	}
	
	public function update(MalePerson $MalePerson) {
		return MalePersonDAO::getInstance()->update($MalePerson);
	}
	
	public function delete(MalePerson $MalePerson) {
		return MalePersonDAO::getInstance()->delete($MalePerson);
	}
	// <summary>Retrieves a collection of ChildPerson objects by the given MalePerson object</summary>
	public function get_ChildPerson_Collection_By_Father(/*int*/ $Person_id) {
		return MalePersonDAO::getInstance()->get_ChildPerson_Collection_By_Father($Person_id);
	}
}
if (!class_exists('MalePersonService')) {
	class MalePersonService extends MalePersonServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class FemalePersonServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new FemalePersonService();
		}
		return instance;
	}
	
	public function getAll() {
		return FemalePersonDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return FemalePersonDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(FemalePerson $FemalePerson) {
		return FemalePersonDAO::getInstance()->insert($FemalePerson);
	}
	
	public function update(FemalePerson $FemalePerson) {
		return FemalePersonDAO::getInstance()->update($FemalePerson);
	}
	
	public function delete(FemalePerson $FemalePerson) {
		return FemalePersonDAO::getInstance()->delete($FemalePerson);
	}
	// <summary>Retrieves a collection of ChildPerson objects by the given FemalePerson object</summary>
	public function get_ChildPerson_Collection_By_Mother(/*int*/ $Person_id) {
		return FemalePersonDAO::getInstance()->get_ChildPerson_Collection_By_Mother($Person_id);
	}
}
if (!class_exists('FemalePersonService')) {
	class FemalePersonService extends FemalePersonServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ChildPersonServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new ChildPersonService();
		}
		return instance;
	}
	
	public function getAll() {
		return ChildPersonDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return ChildPersonDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(ChildPerson $ChildPerson) {
		return ChildPersonDAO::getInstance()->insert($ChildPerson);
	}
	
	public function update(ChildPerson $ChildPerson) {
		return ChildPersonDAO::getInstance()->update($ChildPerson);
	}
	
	public function delete(ChildPerson $ChildPerson) {
		return ChildPersonDAO::getInstance()->delete($ChildPerson);
	}
}
if (!class_exists('ChildPersonService')) {
	class ChildPersonService extends ChildPersonServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class DeathServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new DeathService();
		}
		return instance;
	}
	
	public function getAll() {
		return DeathDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return DeathDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(Death $Death) {
		return DeathDAO::getInstance()->insert($Death);
	}
	
	public function update(Death $Death) {
		return DeathDAO::getInstance()->update($Death);
	}
	
	public function delete(Death $Death) {
		return DeathDAO::getInstance()->delete($Death);
	}
}
if (!class_exists('DeathService')) {
	class DeathService extends DeathServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class NaturalDeathServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new NaturalDeathService();
		}
		return instance;
	}
	
	public function getAll() {
		return NaturalDeathDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return NaturalDeathDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(NaturalDeath $NaturalDeath) {
		return NaturalDeathDAO::getInstance()->insert($NaturalDeath);
	}
	
	public function update(NaturalDeath $NaturalDeath) {
		return NaturalDeathDAO::getInstance()->update($NaturalDeath);
	}
	
	public function delete(NaturalDeath $NaturalDeath) {
		return NaturalDeathDAO::getInstance()->delete($NaturalDeath);
	}
}
if (!class_exists('NaturalDeathService')) {
	class NaturalDeathService extends NaturalDeathServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class UnnaturalDeathServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new UnnaturalDeathService();
		}
		return instance;
	}
	
	public function getAll() {
		return UnnaturalDeathDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id) {
		return UnnaturalDeathDAO::getInstance()->getSingle($Person_id);
	}
	
	public function insert(UnnaturalDeath $UnnaturalDeath) {
		return UnnaturalDeathDAO::getInstance()->insert($UnnaturalDeath);
	}
	
	public function update(UnnaturalDeath $UnnaturalDeath) {
		return UnnaturalDeathDAO::getInstance()->update($UnnaturalDeath);
	}
	
	public function delete(UnnaturalDeath $UnnaturalDeath) {
		return UnnaturalDeathDAO::getInstance()->delete($UnnaturalDeath);
	}
}
if (!class_exists('UnnaturalDeathService')) {
	class UnnaturalDeathService extends UnnaturalDeathServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class TaskServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new TaskService();
		}
		return instance;
	}
	
	public function getAll() {
		return TaskDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Task_id) {
		return TaskDAO::getInstance()->getSingle($Task_id);
	}
	
	public function insert(Task $Task) {
		return TaskDAO::getInstance()->insert($Task);
	}
	
	public function update(Task $Task) {
		return TaskDAO::getInstance()->update($Task);
	}
	
	public function delete(Task $Task) {
		return TaskDAO::getInstance()->delete($Task);
	}
}
if (!class_exists('TaskService')) {
	class TaskService extends TaskServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ValueType1ServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new ValueType1Service();
		}
		return instance;
	}
	
	public function getAll() {
		return ValueType1DAO::getInstance()->getAll();
	}
	
	public function getSingle( $ValueType1Value) {
		return ValueType1DAO::getInstance()->getSingle($ValueType1Value);
	}
	
	public function insert(ValueType1 $ValueType1) {
		return ValueType1DAO::getInstance()->insert($ValueType1);
	}
	
	public function update(ValueType1 $ValueType1) {
		return ValueType1DAO::getInstance()->update($ValueType1);
	}
	
	public function delete(ValueType1 $ValueType1) {
		return ValueType1DAO::getInstance()->delete($ValueType1);
	}
	// <summary>Retrieves a collection of DoesSomethingElseWithPerson objects by the given ValueType1 object</summary>
	public function get_DoesSomethingElseWithPerson_Collection_By_ValueType1DoesSomethingElseWith(/*decimal*/ $ValueType1Value) {
		return ValueType1DAO::getInstance()->get_DoesSomethingElseWithPerson_Collection_By_ValueType1DoesSomethingElseWith($ValueType1Value);
	}
}
if (!class_exists('ValueType1Service')) {
	class ValueType1Service extends ValueType1ServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonBoughtCarFromPersonOnDateServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new PersonBoughtCarFromPersonOnDateService();
		}
		return instance;
	}
	
	public function getAll() {
		return PersonBoughtCarFromPersonOnDateDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Person_id,  $CarSold_vin,  $Person_id) {
		return PersonBoughtCarFromPersonOnDateDAO::getInstance()->getSingle($Person_id, $CarSold_vin, $Person_id);
	}
	
	public function insert(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		return PersonBoughtCarFromPersonOnDateDAO::getInstance()->insert($PersonBoughtCarFromPersonOnDate);
	}
	
	public function update(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		return PersonBoughtCarFromPersonOnDateDAO::getInstance()->update($PersonBoughtCarFromPersonOnDate);
	}
	
	public function delete(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		return PersonBoughtCarFromPersonOnDateDAO::getInstance()->delete($PersonBoughtCarFromPersonOnDate);
	}
}
if (!class_exists('PersonBoughtCarFromPersonOnDateService')) {
	class PersonBoughtCarFromPersonOnDateService extends PersonBoughtCarFromPersonOnDateServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ReviewServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new ReviewService();
		}
		return instance;
	}
	
	public function getAll() {
		return ReviewDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Car_vin,  $Criterion_Name) {
		return ReviewDAO::getInstance()->getSingle($Car_vin, $Criterion_Name);
	}
	
	public function insert(Review $Review) {
		return ReviewDAO::getInstance()->insert($Review);
	}
	
	public function update(Review $Review) {
		return ReviewDAO::getInstance()->update($Review);
	}
	
	public function delete(Review $Review) {
		return ReviewDAO::getInstance()->delete($Review);
	}
}
if (!class_exists('ReviewService')) {
	class ReviewService extends ReviewServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
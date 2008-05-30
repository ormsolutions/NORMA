<?php
require_once("Entities.php");
require_once("DataLayer.php");
class PersonDrivesCarServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset()) {
			instance = new PersonDrivesCarService();
		}
		return instance;
	}
	
	public function getAll() {
		return PersonDrivesCarDAO::getInstance()->getAll();
	}
	
	public function getSingle( $DrivesCar_vin,  $Person_id) {
		return PersonDrivesCarDAO::getInstance()->getSingle($DrivesCar_vin, $Person_id);
	}
	
	public function insert(PersonDrivesCar $PersonDrivesCar) {
		return PersonDrivesCarDAO::getInstance()->insert($PersonDrivesCar);
	}
	
	public function update(PersonDrivesCar $PersonDrivesCar) {
		return PersonDrivesCarDAO::getInstance()->update($PersonDrivesCar);
	}
	
	public function delete(PersonDrivesCar $PersonDrivesCar) {
		return PersonDrivesCarDAO::getInstance()->delete($PersonDrivesCar);
	}
}
if (!class_exists('PersonDrivesCarService')) {
	class PersonDrivesCarService extends PersonDrivesCarServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonHasNickNameServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset()) {
			instance = new PersonHasNickNameService();
		}
		return instance;
	}
	
	public function getAll() {
		return PersonHasNickNameDAO::getInstance()->getAll();
	}
	
	public function getSingle( $NickName,  $Person_id) {
		return PersonHasNickNameDAO::getInstance()->getSingle($NickName, $Person_id);
	}
	
	public function insert(PersonHasNickName $PersonHasNickName) {
		return PersonHasNickNameDAO::getInstance()->insert($PersonHasNickName);
	}
	
	public function update(PersonHasNickName $PersonHasNickName) {
		return PersonHasNickNameDAO::getInstance()->update($PersonHasNickName);
	}
	
	public function delete(PersonHasNickName $PersonHasNickName) {
		return PersonHasNickNameDAO::getInstance()->delete($PersonHasNickName);
	}
}
if (!class_exists('PersonHasNickNameService')) {
	class PersonHasNickNameService extends PersonHasNickNameServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset()) {
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
	// <summary>Retrieves a collection of PersonDrivesCar objects by the given Person object</summary>
	public function get_PersonDrivesCar_Collection_By_DrivenByPerson(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_PersonDrivesCar_Collection_By_DrivenByPerson($Person_id);
	}
	// <summary>Retrieves a collection of PersonHasNickName objects by the given Person object</summary>
	public function get_PersonHasNickName_Collection_By_Person(/*int*/ $Person_id) {
		return PersonDAO::getInstance()->get_PersonHasNickName_Collection_By_Person($Person_id);
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
class TaskServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset()) {
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
		if (!isset()) {
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
class DeathServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset()) {
			instance = new DeathService();
		}
		return instance;
	}
	
	public function getAll() {
		return DeathDAO::getInstance()->getAll();
	}
	
	public function getSingle() {
		return DeathDAO::getInstance()->getSingle();
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
class PersonBoughtCarFromPersonOnDateServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset()) {
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
		if (!isset()) {
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
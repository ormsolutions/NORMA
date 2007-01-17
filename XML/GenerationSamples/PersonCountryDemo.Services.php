<?php
require_once("Entities.php");
require_once("DataLayer.php");
class PersonServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset())) {
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
}
if (!class_exists('PersonService')) {
	class PersonService extends PersonServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class CountryServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset())) {
			instance = new CountryService();
		}
		return instance;
	}
	
	public function getAll() {
		return CountryDAO::getInstance()->getAll();
	}
	
	public function getSingle( $Country_name) {
		return CountryDAO::getInstance()->getSingle($Country_name);
	}
	
	public function insert(Country $Country) {
		return CountryDAO::getInstance()->insert($Country);
	}
	
	public function update(Country $Country) {
		return CountryDAO::getInstance()->update($Country);
	}
	
	public function delete(Country $Country) {
		return CountryDAO::getInstance()->delete($Country);
	}
	// <summary>Retrieves a collection of Person objects by the given Country object</summary>
	public function get_Person_Collection_By_Country(/*string*/ $Country_name) {
		return CountryDAO::getInstance()->get_Person_Collection_By_Country($Country_name);
	}
}
if (!class_exists('CountryService')) {
	class CountryService extends CountryServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
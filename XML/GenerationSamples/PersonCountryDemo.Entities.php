<?php
class PersonBase extends Entity {
	private $Person_id;
	private $Person_Country_Country_Proxy;
	private $LastName;
	private $FirstName;
	private $Title;
	public function __construct() {
		parent::__construct();
		$this->Person_Country_Country_Proxy = new Person_Country_Country_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Person_id"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("LastName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("LastName", null, 30));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("FirstName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("FirstName", null, 30));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Title"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Title", null, 4));
	}
	public function getPerson_id() {
		return $this->Person_id;
	}
	public function setPerson_id(/*object*/ $value) {
		$this->Person_id = $value;
	}
	public function setCountry(Country $value) {
		$this->Person_Country_Country_Proxy->Set($value);
	}
	public function getCountry() {
		return $this->Person_Country_Country_Proxy->Get();
	}
	public function getLastName() {
		return $this->LastName;
	}
	public function setLastName(/*string*/ $value) {
		$this->LastName = $value;
	}
	public function getFirstName() {
		return $this->FirstName;
	}
	public function setFirstName(/*string*/ $value) {
		$this->FirstName = $value;
	}
	public function getTitle() {
		return $this->Title;
	}
	public function setTitle(/*string*/ $value) {
		$this->Title = $value;
	}
}
if (!class_exists('Person')) {
	class Person extends PersonBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class CountryBase extends Entity {
	private $Country_name;
	private $Region_Region_code;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Country_name"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Country_name", null, 20));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("Region_Region_code"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Region_code", 8, 8));
	}
	public function getCountry_name() {
		return $this->Country_name;
	}
	public function setCountry_name(/*string*/ $value) {
		$this->Country_name = $value;
	}
	public function getRegion_Region_code() {
		return $this->Region_Region_code;
	}
	public function setRegion_Region_code(/*string*/ $value) {
		$this->Region_Region_code = $value;
	}
}
if (!class_exists('Country')) {
	class Country extends CountryBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
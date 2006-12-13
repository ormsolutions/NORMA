<?php
// <summary>Class used to proxy a Country for the role Country for use inside of a Person isCollection: false</summary>
class Person_Country_Country_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!(.global::isset($this->value))) {
			$this->value = PersonDAO::getInstance()->getSingle($this->ref->getCountry_name());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Country for the role Country for use inside of a Country isCollection: true</summary>
class Country_Country_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!(.global::isset($this->value))) {
			$this->value = CountryDAO::getInstance()->get_Person_Collection_By_Country($this->ref->getCountry_name());
		}
		return $this->value;
	}
}
?>
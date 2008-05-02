<?php
// <summary>Class used to proxy a Person for the role DrivenByPerson for use inside of a PersonDrivesCar isCollection: false</summary>
class PersonDrivesCar_DrivenByPerson_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDrivesCarDAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Person for use inside of a PersonHasNickName isCollection: false</summary>
class PersonHasNickName_Person_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonHasNickNameDAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Husband for use inside of a Person isCollection: false</summary>
class Wife_Husband_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a ValueType1 for the role ValueType1DoesSomethingElseWith for use inside of a Person isCollection: false</summary>
class DoesSomethingElseWithPerson_ValueType1DoesSomethingElseWith_ValueType1_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->getSingle($this->ref->getValueType1Value());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role DrivenByPerson for use inside of a Person isCollection: true</summary>
class Person_DrivenByPerson_PersonDrivesCar_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_PersonDrivesCar_Collection_By_DrivenByPerson($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Person for use inside of a Person isCollection: true</summary>
class Person_Person_PersonHasNickName_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_PersonHasNickName_Collection_By_Person($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Husband for use inside of a Person isCollection: true</summary>
class Person_Husband_Wife_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_Wife_Collection_By_Husband($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Person for use inside of a Person isCollection: true</summary>
class Person_Person_Task_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_Task_Collection_By_Person($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role DoesSomethingWithPerson for use inside of a Person isCollection: true</summary>
class Person_DoesSomethingWithPerson_ValueType1DoesSomethingWith_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_ValueType1DoesSomethingWith_Collection_By_DoesSomethingWithPerson($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Buyer for use inside of a Person isCollection: true</summary>
class Person_Buyer_PersonBoughtCarFromPersonOnDate_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_PersonBoughtCarFromPersonOnDate_Collection_By_Buyer($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Seller for use inside of a Person isCollection: true</summary>
class Person_Seller_PersonBoughtCarFromPersonOnDate_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonDAO::getInstance()->get_PersonBoughtCarFromPersonOnDate_Collection_By_Seller($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Person for use inside of a Task isCollection: false</summary>
class Task_Person_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = TaskDAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role DoesSomethingWithPerson for use inside of a ValueType1 isCollection: false</summary>
class ValueType1DoesSomethingWith_DoesSomethingWithPerson_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = ValueType1DAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a ValueType1 for the role ValueType1DoesSomethingElseWith for use inside of a ValueType1 isCollection: true</summary>
class ValueType1_ValueType1DoesSomethingElseWith_DoesSomethingElseWithPerson_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = ValueType1DAO::getInstance()->get_DoesSomethingElseWithPerson_Collection_By_ValueType1DoesSomethingElseWith($this->ref->getValueType1Value());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Buyer for use inside of a PersonBoughtCarFromPersonOnDate isCollection: false</summary>
class PersonBoughtCarFromPersonOnDate_Buyer_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonBoughtCarFromPersonOnDateDAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a Person for the role Seller for use inside of a PersonBoughtCarFromPersonOnDate isCollection: false</summary>
class PersonBoughtCarFromPersonOnDate_Seller_Person_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = PersonBoughtCarFromPersonOnDateDAO::getInstance()->getSingle($this->ref->getPerson_id());
		}
		return $this->value;
	}
}
?>
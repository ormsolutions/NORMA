<?php
static class DataAccessBase {
	static $params = null;
	// <summary>Gets the appropriate data adapter for the current database configuration</summary>
	public static function getDataAdapter() {
		return Zend_Db::factory(getpdoType(), getparams());
	}
	private static function get() {
		if (params === null) {
			params = array();
			params["host"] = "";
			params["username"] = "";
			params["password"] = "";
			params["dbname"] = "";
		}
		return params;
	}
	private static function getpdoType() {
		if (pdoType === null) {
			pdoType = "PDO_MYSQL";
		}
		return pdoType;
	}
}
if (!class_exists('DataAccess')) {
	static class DataAccess extends DataAccessBase {
	}
}
class PersonDrivesCarDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of PersonDrivesCarDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new PersonDrivesCarDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of PersonDrivesCar objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonDrivesCar", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonDrivesCar();
				$tempEntity->setPerson_id($results["DrivenByPerson_Person_id"]);
				$tempEntity->setDrivesCar_vin($results["DrivesCar_vin"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified PersonDrivesCarobject from the database</summary>
	public function getSingle(/*decimal*/ $DrivesCar_vin, /*int*/ $Person_id) {
		try {
			$retVal = new PersonDrivesCar();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("PersonDrivesCar", "*");
			$select->where("DrivesCar_vin = ?", $DrivesCar_vin);
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->setPerson_id($row["DrivenByPerson_Person_id"]);
			$retVal->setDrivesCar_vin($row["DrivesCar_vin"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given PersonDrivesCar object into the database</summary>
	public function insert(PersonDrivesCar $PersonDrivesCar) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["DrivenByPerson"] = $PersonDrivesCar->getPerson_id();
			$dataArray["DrivesCar_vin"] = $PersonDrivesCar->getDrivesCar_vin();
			$nrRowsAffected = $db->insert("PersonDrivesCar", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given PersonDrivesCar object in the database</summary>
	public function update(PersonDrivesCar $PersonDrivesCar) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["DrivenByPerson"] = $PersonDrivesCar->getPerson_id();
			$dataArray["DrivesCar_vin"] = $PersonDrivesCar->getDrivesCar_vin();
			$whereClause = $db->quoteInto("DrivesCar_vin = ?", $PersonDrivesCar->getDrivesCar_vin()) . $db->quoteInto("Person_id = ?", $PersonDrivesCar->getPerson_id());
			$nrRowsAffected = $db->update("PersonDrivesCar", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given PersonDrivesCar object from the database</summary>
	public function delete(PersonDrivesCar $PersonDrivesCar) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("DrivesCar_vin = ?", $PersonDrivesCar->getDrivesCar_vin()) . $db->quoteInto("Person_id = ?", $PersonDrivesCar->getPerson_id());
			$nrRowsAffected = $db->delete("PersonDrivesCar", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('PersonDrivesCarDao')) {
	class PersonDrivesCarDao extends PersonDrivesCarDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonHasNickNameDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of PersonHasNickNameDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new PersonHasNickNameDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of PersonHasNickName objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonHasNickName", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonHasNickName();
				$tempEntity->setNickName($results["NickName"]);
				$tempEntity->setPerson_id($results["Person_Person_id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified PersonHasNickNameobject from the database</summary>
	public function getSingle(/*string*/ $NickName, /*int*/ $Person_id) {
		try {
			$retVal = new PersonHasNickName();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("PersonHasNickName", "*");
			$select->where("NickName = ?", $NickName);
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->setNickName($row["NickName"]);
			$retVal->setPerson_id($row["Person_Person_id"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given PersonHasNickName object into the database</summary>
	public function insert(PersonHasNickName $PersonHasNickName) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["NickName"] = $PersonHasNickName->getNickName();
			$dataArray["Person"] = $PersonHasNickName->getPerson_id();
			$nrRowsAffected = $db->insert("PersonHasNickName", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given PersonHasNickName object in the database</summary>
	public function update(PersonHasNickName $PersonHasNickName) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["NickName"] = $PersonHasNickName->getNickName();
			$dataArray["Person"] = $PersonHasNickName->getPerson_id();
			$whereClause = $db->quoteInto("NickName = ?", $PersonHasNickName->getNickName()) . $db->quoteInto("Person_id = ?", $PersonHasNickName->getPerson_id());
			$nrRowsAffected = $db->update("PersonHasNickName", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given PersonHasNickName object from the database</summary>
	public function delete(PersonHasNickName $PersonHasNickName) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("NickName = ?", $PersonHasNickName->getNickName()) . $db->quoteInto("Person_id = ?", $PersonHasNickName->getPerson_id());
			$nrRowsAffected = $db->delete("PersonHasNickName", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('PersonHasNickNameDao')) {
	class PersonHasNickNameDao extends PersonHasNickNameDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of PersonDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new PersonDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of Person objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Person", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Person();
				$tempEntity->setFirstName($results["FirstName"]);
				$tempEntity->setDate_YMD($results["Date_YMD"]);
				$tempEntity->setLastName($results["LastName"]);
				$tempEntity->setOptionalUniqueString($results["OptionalUniqueString"]);
				$tempEntity->setHatType_ColorARGB($results["HatType_ColorARGB"]);
				$tempEntity->setHatType_HatTypeStyle_HatTypeStyle_Description($results["HatType_HatTypeStyle_HatTypeStyle_Description"]);
				$tempEntity->setPerson_id($results["Husband_Person_id"]);
				$tempEntity->setOwnsCar_vin($results["OwnsCar_vin"]);
				$tempEntity->setGender_Gender_Code($results["Gender_Gender_Code"]);
				$tempEntity->sethasParents($results["hasParents"]);
				$tempEntity->setValueType1Value($results["ValueType1DoesSomethingElseWith_ValueType1Value"]);
				$tempEntity->setOptionalUniqueDecimal($results["OptionalUniqueDecimal"]);
				$tempEntity->setMandatoryUniqueDecimal($results["MandatoryUniqueDecimal"]);
				$tempEntity->setMandatoryUniqueString($results["MandatoryUniqueString"]);
				$tempEntity->setOptionalUniqueTinyInt($results["OptionalUniqueTinyInt"]);
				$tempEntity->setMandatoryUniqueTinyInt($results["MandatoryUniqueTinyInt"]);
				$tempEntity->setOptionalNonUniqueTinyInt($results["OptionalNonUniqueTinyInt"]);
				$tempEntity->setMandatoryNonUniqueTinyInt($results["MandatoryNonUniqueTinyInt"]);
				$tempEntity->setMandatoryNonUniqueUnconstrainedDecimal($results["MandatoryNonUniqueUnconstrainedDecimal"]);
				$tempEntity->setMandatoryNonUniqueUnconstrainedFloat($results["MandatoryNonUniqueUnconstrainedFloat"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified Personobject from the database</summary>
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new Person();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->setFirstName($row["FirstName"]);
			$retVal->setDate_YMD($row["Date_YMD"]);
			$retVal->setLastName($row["LastName"]);
			$retVal->setOptionalUniqueString($row["OptionalUniqueString"]);
			$retVal->setHatType_ColorARGB($row["HatType_ColorARGB"]);
			$retVal->setHatType_HatTypeStyle_HatTypeStyle_Description($row["HatType_HatTypeStyle_HatTypeStyle_Description"]);
			$retVal->setPerson_id($row["Husband_Person_id"]);
			$retVal->setOwnsCar_vin($row["OwnsCar_vin"]);
			$retVal->setGender_Gender_Code($row["Gender_Gender_Code"]);
			$retVal->sethasParents($row["hasParents"]);
			$retVal->setValueType1Value($row["ValueType1DoesSomethingElseWith_ValueType1Value"]);
			$retVal->setOptionalUniqueDecimal($row["OptionalUniqueDecimal"]);
			$retVal->setMandatoryUniqueDecimal($row["MandatoryUniqueDecimal"]);
			$retVal->setMandatoryUniqueString($row["MandatoryUniqueString"]);
			$retVal->setOptionalUniqueTinyInt($row["OptionalUniqueTinyInt"]);
			$retVal->setMandatoryUniqueTinyInt($row["MandatoryUniqueTinyInt"]);
			$retVal->setOptionalNonUniqueTinyInt($row["OptionalNonUniqueTinyInt"]);
			$retVal->setMandatoryNonUniqueTinyInt($row["MandatoryNonUniqueTinyInt"]);
			$retVal->setMandatoryNonUniqueUnconstrainedDecimal($row["MandatoryNonUniqueUnconstrainedDecimal"]);
			$retVal->setMandatoryNonUniqueUnconstrainedFloat($row["MandatoryNonUniqueUnconstrainedFloat"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given Person object into the database</summary>
	public function insert(Person $Person) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $Person->getFirstName();
			$dataArray["Person_id"] = $Person->getPerson_id();
			$dataArray["Date_YMD"] = $Person->getDate_YMD();
			$dataArray["LastName"] = $Person->getLastName();
			$dataArray["OptionalUniqueString"] = $Person->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $Person->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $Person->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $Person->getPerson_id();
			$dataArray["OwnsCar_vin"] = $Person->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $Person->getGender_Gender_Code();
			$dataArray["hasParents"] = $Person->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $Person->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $Person->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $Person->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $Person->getMandatoryUniqueString();
			$dataArray["OptionalUniqueTinyInt"] = $Person->getOptionalUniqueTinyInt();
			$dataArray["MandatoryUniqueTinyInt"] = $Person->getMandatoryUniqueTinyInt();
			$dataArray["OptionalNonUniqueTinyInt"] = $Person->getOptionalNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueTinyInt"] = $Person->getMandatoryNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueUnconstrainedDecimal"] = $Person->getMandatoryNonUniqueUnconstrainedDecimal();
			$dataArray["MandatoryNonUniqueUnconstrainedFloat"] = $Person->getMandatoryNonUniqueUnconstrainedFloat();
			$nrRowsAffected = $db->insert("Person", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given Person object in the database</summary>
	public function update(Person $Person) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $Person->getFirstName();
			$dataArray["Person_id"] = $Person->getPerson_id();
			$dataArray["Date_YMD"] = $Person->getDate_YMD();
			$dataArray["LastName"] = $Person->getLastName();
			$dataArray["OptionalUniqueString"] = $Person->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $Person->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $Person->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $Person->getPerson_id();
			$dataArray["OwnsCar_vin"] = $Person->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $Person->getGender_Gender_Code();
			$dataArray["hasParents"] = $Person->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $Person->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $Person->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $Person->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $Person->getMandatoryUniqueString();
			$dataArray["OptionalUniqueTinyInt"] = $Person->getOptionalUniqueTinyInt();
			$dataArray["MandatoryUniqueTinyInt"] = $Person->getMandatoryUniqueTinyInt();
			$dataArray["OptionalNonUniqueTinyInt"] = $Person->getOptionalNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueTinyInt"] = $Person->getMandatoryNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueUnconstrainedDecimal"] = $Person->getMandatoryNonUniqueUnconstrainedDecimal();
			$dataArray["MandatoryNonUniqueUnconstrainedFloat"] = $Person->getMandatoryNonUniqueUnconstrainedFloat();
			$whereClause = $db->quoteInto("Person_id = ?", $Person->getPerson_id());
			$nrRowsAffected = $db->update("Person", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given Person object from the database</summary>
	public function delete(Person $Person) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $Person->getPerson_id());
			$nrRowsAffected = $db->delete("Person", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of PersonDrivesCar objects by the given Person object</summary>
	public function get_PersonDrivesCar_Collection_By_DrivenByPerson(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonDrivesCar", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonDrivesCar();
				$tempEntity->setPerson_id($results["DrivenByPerson_Person_id"]);
				$tempEntity->setDrivesCar_vin($results["DrivesCar_vin"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves a collection of PersonHasNickName objects by the given Person object</summary>
	public function get_PersonHasNickName_Collection_By_Person(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonHasNickName", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonHasNickName();
				$tempEntity->setNickName($results["NickName"]);
				$tempEntity->setPerson_id($results["Person_Person_id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves a collection of Wife objects by the given Person object</summary>
	public function get_Wife_Collection_By_Husband(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new ();
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves a collection of Task objects by the given Person object</summary>
	public function get_Task_Collection_By_Person(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Task", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Task();
				$tempEntity->setPerson_id($results["Person_Person_id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves a collection of ValueType1DoesSomethingWith objects by the given Person object</summary>
	public function get_ValueType1DoesSomethingWith_Collection_By_DoesSomethingWithPerson(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new ();
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves a collection of PersonBoughtCarFromPersonOnDate objects by the given Person object</summary>
	public function get_PersonBoughtCarFromPersonOnDate_Collection_By_Buyer(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonBoughtCarFromPersonOnDate", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonBoughtCarFromPersonOnDate();
				$tempEntity->setPerson_id($results["Buyer_Person_id"]);
				$tempEntity->setCarSold_vin($results["CarSold_vin"]);
				$tempEntity->setPerson_id($results["Seller_Person_id"]);
				$tempEntity->setSaleDate_YMD($results["SaleDate_YMD"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves a collection of PersonBoughtCarFromPersonOnDate objects by the given Person object</summary>
	public function get_PersonBoughtCarFromPersonOnDate_Collection_By_Seller(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonBoughtCarFromPersonOnDate", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonBoughtCarFromPersonOnDate();
				$tempEntity->setPerson_id($results["Buyer_Person_id"]);
				$tempEntity->setCarSold_vin($results["CarSold_vin"]);
				$tempEntity->setPerson_id($results["Seller_Person_id"]);
				$tempEntity->setSaleDate_YMD($results["SaleDate_YMD"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('PersonDao')) {
	class PersonDao extends PersonDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class DeathDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of DeathDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new DeathDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of Death objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Person", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Death();
				$tempEntity->getPerson()->setFirstName($results["FirstName"]);
				$tempEntity->getPerson()->setDate_YMD($results["Date_YMD"]);
				$tempEntity->getPerson()->setLastName($results["LastName"]);
				$tempEntity->getPerson()->setOptionalUniqueString($results["OptionalUniqueString"]);
				$tempEntity->getPerson()->setHatType_ColorARGB($results["HatType_ColorARGB"]);
				$tempEntity->getPerson()->setHatType_HatTypeStyle_HatTypeStyle_Description($results["HatType_HatTypeStyle_HatTypeStyle_Description"]);
				$tempEntity->getPerson()->setPerson_id($results["Husband_Person_id"]);
				$tempEntity->getPerson()->setOwnsCar_vin($results["OwnsCar_vin"]);
				$tempEntity->getPerson()->setGender_Gender_Code($results["Gender_Gender_Code"]);
				$tempEntity->getPerson()->sethasParents($results["hasParents"]);
				$tempEntity->getPerson()->setValueType1Value($results["ValueType1DoesSomethingElseWith_ValueType1Value"]);
				$tempEntity->getPerson()->setOptionalUniqueDecimal($results["OptionalUniqueDecimal"]);
				$tempEntity->getPerson()->setMandatoryUniqueDecimal($results["MandatoryUniqueDecimal"]);
				$tempEntity->getPerson()->setMandatoryUniqueString($results["MandatoryUniqueString"]);
				$tempEntity->getPerson()->setOptionalUniqueTinyInt($results["OptionalUniqueTinyInt"]);
				$tempEntity->getPerson()->setMandatoryUniqueTinyInt($results["MandatoryUniqueTinyInt"]);
				$tempEntity->getPerson()->setOptionalNonUniqueTinyInt($results["OptionalNonUniqueTinyInt"]);
				$tempEntity->getPerson()->setMandatoryNonUniqueTinyInt($results["MandatoryNonUniqueTinyInt"]);
				$tempEntity->getPerson()->setMandatoryNonUniqueUnconstrainedDecimal($results["MandatoryNonUniqueUnconstrainedDecimal"]);
				$tempEntity->getPerson()->setMandatoryNonUniqueUnconstrainedFloat($results["MandatoryNonUniqueUnconstrainedFloat"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified Deathobject from the database</summary>
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new Death();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->getPerson()->setFirstName($row["FirstName"]);
			$retVal->getPerson()->setDate_YMD($row["Date_YMD"]);
			$retVal->getPerson()->setLastName($row["LastName"]);
			$retVal->getPerson()->setOptionalUniqueString($row["OptionalUniqueString"]);
			$retVal->getPerson()->setHatType_ColorARGB($row["HatType_ColorARGB"]);
			$retVal->getPerson()->setHatType_HatTypeStyle_HatTypeStyle_Description($row["HatType_HatTypeStyle_HatTypeStyle_Description"]);
			$retVal->getPerson()->setPerson_id($row["Husband_Person_id"]);
			$retVal->getPerson()->setOwnsCar_vin($row["OwnsCar_vin"]);
			$retVal->getPerson()->setGender_Gender_Code($row["Gender_Gender_Code"]);
			$retVal->getPerson()->sethasParents($row["hasParents"]);
			$retVal->getPerson()->setValueType1Value($row["ValueType1DoesSomethingElseWith_ValueType1Value"]);
			$retVal->getPerson()->setOptionalUniqueDecimal($row["OptionalUniqueDecimal"]);
			$retVal->getPerson()->setMandatoryUniqueDecimal($row["MandatoryUniqueDecimal"]);
			$retVal->getPerson()->setMandatoryUniqueString($row["MandatoryUniqueString"]);
			$retVal->getPerson()->setOptionalUniqueTinyInt($row["OptionalUniqueTinyInt"]);
			$retVal->getPerson()->setMandatoryUniqueTinyInt($row["MandatoryUniqueTinyInt"]);
			$retVal->getPerson()->setOptionalNonUniqueTinyInt($row["OptionalNonUniqueTinyInt"]);
			$retVal->getPerson()->setMandatoryNonUniqueTinyInt($row["MandatoryNonUniqueTinyInt"]);
			$retVal->getPerson()->setMandatoryNonUniqueUnconstrainedDecimal($row["MandatoryNonUniqueUnconstrainedDecimal"]);
			$retVal->getPerson()->setMandatoryNonUniqueUnconstrainedFloat($row["MandatoryNonUniqueUnconstrainedFloat"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given Death object into the database</summary>
	public function insert(Death $Death) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $Death->getPerson()->getFirstName();
			$dataArray["Person_id"] = $Death->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $Death->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $Death->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $Death->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $Death->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $Death->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $Death->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $Death->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $Death->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $Death->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $Death->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $Death->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $Death->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $Death->getPerson()->getMandatoryUniqueString();
			$dataArray["OptionalUniqueTinyInt"] = $Death->getPerson()->getOptionalUniqueTinyInt();
			$dataArray["MandatoryUniqueTinyInt"] = $Death->getPerson()->getMandatoryUniqueTinyInt();
			$dataArray["OptionalNonUniqueTinyInt"] = $Death->getPerson()->getOptionalNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueTinyInt"] = $Death->getPerson()->getMandatoryNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueUnconstrainedDecimal"] = $Death->getPerson()->getMandatoryNonUniqueUnconstrainedDecimal();
			$dataArray["MandatoryNonUniqueUnconstrainedFloat"] = $Death->getPerson()->getMandatoryNonUniqueUnconstrainedFloat();
			$dataArray["Date_YMD"] = $Death->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $Death->getDeathCause_DeathCause_Type();
			$dataArray["isDead"] = $Death->getisDead();
			$nrRowsAffected = $db->insert("Death", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given Death object in the database</summary>
	public function update(Death $Death) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $Death->getPerson()->getFirstName();
			$dataArray["Person_id"] = $Death->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $Death->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $Death->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $Death->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $Death->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $Death->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $Death->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $Death->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $Death->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $Death->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $Death->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $Death->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $Death->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $Death->getPerson()->getMandatoryUniqueString();
			$dataArray["OptionalUniqueTinyInt"] = $Death->getPerson()->getOptionalUniqueTinyInt();
			$dataArray["MandatoryUniqueTinyInt"] = $Death->getPerson()->getMandatoryUniqueTinyInt();
			$dataArray["OptionalNonUniqueTinyInt"] = $Death->getPerson()->getOptionalNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueTinyInt"] = $Death->getPerson()->getMandatoryNonUniqueTinyInt();
			$dataArray["MandatoryNonUniqueUnconstrainedDecimal"] = $Death->getPerson()->getMandatoryNonUniqueUnconstrainedDecimal();
			$dataArray["MandatoryNonUniqueUnconstrainedFloat"] = $Death->getPerson()->getMandatoryNonUniqueUnconstrainedFloat();
			$dataArray["Date_YMD"] = $Death->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $Death->getDeathCause_DeathCause_Type();
			$dataArray["isDead"] = $Death->getisDead();
			$whereClause = $db->quoteInto("Person_id = ?", $Death->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("Death", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given Death object from the database</summary>
	public function delete(Death $Death) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $Death->getPerson()->getPerson_id());
			$nrRowsAffected = $db->delete("Death", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('DeathDao')) {
	class DeathDao extends DeathDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class TaskDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of TaskDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new TaskDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of Task objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Task", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Task();
				$tempEntity->setPerson_id($results["Person_Person_id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified Taskobject from the database</summary>
	public function getSingle(/*int*/ $Task_id) {
		try {
			$retVal = new Task();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Task", "*");
			$select->where("Task_id = ?", $Task_id);
			$row = $db->fetchRow($select);
			$retVal->setPerson_id($row["Person_Person_id"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given Task object into the database</summary>
	public function insert(Task $Task) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Person"] = $Task->getPerson_id();
			$dataArray["Task_id"] = $Task->getTask_id();
			$nrRowsAffected = $db->insert("Task", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given Task object in the database</summary>
	public function update(Task $Task) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Person"] = $Task->getPerson_id();
			$dataArray["Task_id"] = $Task->getTask_id();
			$whereClause = $db->quoteInto("Task_id = ?", $Task->getTask_id());
			$nrRowsAffected = $db->update("Task", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given Task object from the database</summary>
	public function delete(Task $Task) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Task_id = ?", $Task->getTask_id());
			$nrRowsAffected = $db->delete("Task", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('TaskDao')) {
	class TaskDao extends TaskDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ValueType1DaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of ValueType1Dao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new ValueType1Dao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of ValueType1 objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("ValueType1", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new ValueType1();
				$tempEntity->setPerson_id($results["DoesSomethingWithPerson_Person_id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified ValueType1object from the database</summary>
	public function getSingle(/*decimal*/ $ValueType1Value) {
		try {
			$retVal = new ValueType1();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("ValueType1", "*");
			$select->where("ValueType1Value = ?", $ValueType1Value);
			$row = $db->fetchRow($select);
			$retVal->setPerson_id($row["DoesSomethingWithPerson_Person_id"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given ValueType1 object into the database</summary>
	public function insert(ValueType1 $ValueType1) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["ValueType1Value"] = $ValueType1->getValueType1Value();
			$dataArray["DoesSomethingWithPerson"] = $ValueType1->getPerson_id();
			$nrRowsAffected = $db->insert("ValueType1", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given ValueType1 object in the database</summary>
	public function update(ValueType1 $ValueType1) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["ValueType1Value"] = $ValueType1->getValueType1Value();
			$dataArray["DoesSomethingWithPerson"] = $ValueType1->getPerson_id();
			$whereClause = $db->quoteInto("ValueType1Value = ?", $ValueType1->getValueType1Value());
			$nrRowsAffected = $db->update("ValueType1", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given ValueType1 object from the database</summary>
	public function delete(ValueType1 $ValueType1) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("ValueType1Value = ?", $ValueType1->getValueType1Value());
			$nrRowsAffected = $db->delete("ValueType1", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of DoesSomethingElseWithPerson objects by the given ValueType1 object</summary>
	public function get_DoesSomethingElseWithPerson_Collection_By_ValueType1DoesSomethingElseWith(/*decimal*/ $ValueType1Value) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("", "*");
			$select->where("ValueType1Value = ?", $ValueType1Value);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new ();
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('ValueType1Dao')) {
	class ValueType1Dao extends ValueType1DaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class PersonBoughtCarFromPersonOnDateDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of PersonBoughtCarFromPersonOnDateDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new PersonBoughtCarFromPersonOnDateDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of PersonBoughtCarFromPersonOnDate objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("PersonBoughtCarFromPersonOnDate", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new PersonBoughtCarFromPersonOnDate();
				$tempEntity->setPerson_id($results["Buyer_Person_id"]);
				$tempEntity->setCarSold_vin($results["CarSold_vin"]);
				$tempEntity->setPerson_id($results["Seller_Person_id"]);
				$tempEntity->setSaleDate_YMD($results["SaleDate_YMD"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified PersonBoughtCarFromPersonOnDateobject from the database</summary>
	public function getSingle(/*int*/ $Person_id, /*decimal*/ $CarSold_vin, /*int*/ $Person_id) {
		try {
			$retVal = new PersonBoughtCarFromPersonOnDate();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("PersonBoughtCarFromPersonOnDate", "*");
			$select->where("Person_id = ?", $Person_id);
			$select->where("CarSold_vin = ?", $CarSold_vin);
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->setPerson_id($row["Buyer_Person_id"]);
			$retVal->setCarSold_vin($row["CarSold_vin"]);
			$retVal->setPerson_id($row["Seller_Person_id"]);
			$retVal->setSaleDate_YMD($row["SaleDate_YMD"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given PersonBoughtCarFromPersonOnDate object into the database</summary>
	public function insert(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Buyer"] = $PersonBoughtCarFromPersonOnDate->getPerson_id();
			$dataArray["CarSold_vin"] = $PersonBoughtCarFromPersonOnDate->getCarSold_vin();
			$dataArray["Seller"] = $PersonBoughtCarFromPersonOnDate->getPerson_id();
			$dataArray["SaleDate_YMD"] = $PersonBoughtCarFromPersonOnDate->getSaleDate_YMD();
			$nrRowsAffected = $db->insert("PersonBoughtCarFromPersonOnDate", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given PersonBoughtCarFromPersonOnDate object in the database</summary>
	public function update(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Buyer"] = $PersonBoughtCarFromPersonOnDate->getPerson_id();
			$dataArray["CarSold_vin"] = $PersonBoughtCarFromPersonOnDate->getCarSold_vin();
			$dataArray["Seller"] = $PersonBoughtCarFromPersonOnDate->getPerson_id();
			$dataArray["SaleDate_YMD"] = $PersonBoughtCarFromPersonOnDate->getSaleDate_YMD();
			$whereClause = $db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id()) . $db->quoteInto("CarSold_vin = ?", $PersonBoughtCarFromPersonOnDate->getCarSold_vin()) . $db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id());
			$nrRowsAffected = $db->update("PersonBoughtCarFromPersonOnDate", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given PersonBoughtCarFromPersonOnDate object from the database</summary>
	public function delete(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id()) . $db->quoteInto("CarSold_vin = ?", $PersonBoughtCarFromPersonOnDate->getCarSold_vin()) . $db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id());
			$nrRowsAffected = $db->delete("PersonBoughtCarFromPersonOnDate", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('PersonBoughtCarFromPersonOnDateDao')) {
	class PersonBoughtCarFromPersonOnDateDao extends PersonBoughtCarFromPersonOnDateDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ReviewDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of ReviewDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new ReviewDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of Review objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Review", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Review();
				$tempEntity->setCar_vin($results["Car_vin"]);
				$tempEntity->setRating_Nr_Integer($results["Rating_Nr_Integer"]);
				$tempEntity->setCriterion_Name($results["Criterion_Name"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified Reviewobject from the database</summary>
	public function getSingle(/*decimal*/ $Car_vin, /*string*/ $Criterion_Name) {
		try {
			$retVal = new Review();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Review", "*");
			$select->where("Car_vin = ?", $Car_vin);
			$select->where("Criterion_Name = ?", $Criterion_Name);
			$row = $db->fetchRow($select);
			$retVal->setCar_vin($row["Car_vin"]);
			$retVal->setRating_Nr_Integer($row["Rating_Nr_Integer"]);
			$retVal->setCriterion_Name($row["Criterion_Name"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given Review object into the database</summary>
	public function insert(Review $Review) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Car_vin"] = $Review->getCar_vin();
			$dataArray["Rating_Nr_Integer"] = $Review->getRating_Nr_Integer();
			$dataArray["Criterion_Name"] = $Review->getCriterion_Name();
			$nrRowsAffected = $db->insert("Review", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given Review object in the database</summary>
	public function update(Review $Review) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Car_vin"] = $Review->getCar_vin();
			$dataArray["Rating_Nr_Integer"] = $Review->getRating_Nr_Integer();
			$dataArray["Criterion_Name"] = $Review->getCriterion_Name();
			$whereClause = $db->quoteInto("Car_vin = ?", $Review->getCar_vin()) . $db->quoteInto("Criterion_Name = ?", $Review->getCriterion_Name());
			$nrRowsAffected = $db->update("Review", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given Review object from the database</summary>
	public function delete(Review $Review) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Car_vin = ?", $Review->getCar_vin()) . $db->quoteInto("Criterion_Name = ?", $Review->getCriterion_Name());
			$nrRowsAffected = $db->delete("Review", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('ReviewDao')) {
	class ReviewDao extends ReviewDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
<?php
static class DataAccessBase {
	static $params = null;
	
	public static function getDataAdapter() {
		return Zend_Db::factory(getpdoType(), getparams());
	}
	private static function get() {
		if (params == null) {
			params = array();
			params["host"] = "";
			params["username"] = "";
			params["password"] = "";
			params["dbname"] = "";
		}
		return params;
	}
	private static function getpdoType() {
		if (pdoType == null) {
			pdoType = "PDO_MYSQL";
		}
		return pdoType;
	}
}
if (!class_exists('DataAccess')) {
	static class DataAccess extends DataAccessBase {
	}
}
class PersonDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new PersonDao();
		}
		return instance;
	}
	
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
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
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
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
			$nrRowsAffected = $db->insert("Person", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
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
			$whereClause = $db->quoteInto("Person_id = ?", $Person->getPerson_id());
			$nrRowsAffected = $db->update("Person", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
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
class MalePersonDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new MalePersonDao();
		}
		return instance;
	}
	
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
				$tempEntity = new MalePerson();
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new MalePerson();
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
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function insert(MalePerson $MalePerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $MalePerson->getPerson()->getFirstName();
			$dataArray["Person_id"] = $MalePerson->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $MalePerson->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $MalePerson->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $MalePerson->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $MalePerson->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $MalePerson->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $MalePerson->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $MalePerson->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $MalePerson->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $MalePerson->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $MalePerson->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $MalePerson->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $MalePerson->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $MalePerson->getPerson()->getMandatoryUniqueString();
			$nrRowsAffected = $db->insert("MalePerson", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function update(MalePerson $MalePerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $MalePerson->getPerson()->getFirstName();
			$dataArray["Person_id"] = $MalePerson->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $MalePerson->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $MalePerson->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $MalePerson->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $MalePerson->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $MalePerson->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $MalePerson->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $MalePerson->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $MalePerson->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $MalePerson->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $MalePerson->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $MalePerson->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $MalePerson->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $MalePerson->getPerson()->getMandatoryUniqueString();
			$whereClause = $db->quoteInto("Person_id = ?", $MalePerson->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("MalePerson", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(MalePerson $MalePerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $MalePerson->getPerson()->getPerson_id());
			$nrRowsAffected = $db->delete("MalePerson", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of ChildPerson objects by the given MalePerson object</summary>
	public function get_ChildPerson_Collection_By_Father(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new ChildPerson();
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('MalePersonDao')) {
	class MalePersonDao extends MalePersonDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class FemalePersonDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new FemalePersonDao();
		}
		return instance;
	}
	
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
				$tempEntity = new FemalePerson();
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new FemalePerson();
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
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function insert(FemalePerson $FemalePerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $FemalePerson->getPerson()->getFirstName();
			$dataArray["Person_id"] = $FemalePerson->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $FemalePerson->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $FemalePerson->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $FemalePerson->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $FemalePerson->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $FemalePerson->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $FemalePerson->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $FemalePerson->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $FemalePerson->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $FemalePerson->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $FemalePerson->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $FemalePerson->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $FemalePerson->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $FemalePerson->getPerson()->getMandatoryUniqueString();
			$nrRowsAffected = $db->insert("FemalePerson", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function update(FemalePerson $FemalePerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $FemalePerson->getPerson()->getFirstName();
			$dataArray["Person_id"] = $FemalePerson->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $FemalePerson->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $FemalePerson->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $FemalePerson->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $FemalePerson->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $FemalePerson->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $FemalePerson->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $FemalePerson->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $FemalePerson->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $FemalePerson->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $FemalePerson->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $FemalePerson->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $FemalePerson->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $FemalePerson->getPerson()->getMandatoryUniqueString();
			$whereClause = $db->quoteInto("Person_id = ?", $FemalePerson->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("FemalePerson", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(FemalePerson $FemalePerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $FemalePerson->getPerson()->getPerson_id());
			$nrRowsAffected = $db->delete("FemalePerson", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of ChildPerson objects by the given FemalePerson object</summary>
	public function get_ChildPerson_Collection_By_Mother(/*int*/ $Person_id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Person_id = ?", $Person_id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new ChildPerson();
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('FemalePersonDao')) {
	class FemalePersonDao extends FemalePersonDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class ChildPersonDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new ChildPersonDao();
		}
		return instance;
	}
	
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
				$tempEntity = new ChildPerson();
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new ChildPerson();
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
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function insert(ChildPerson $ChildPerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $ChildPerson->getPerson()->getFirstName();
			$dataArray["Person_id"] = $ChildPerson->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $ChildPerson->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $ChildPerson->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $ChildPerson->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $ChildPerson->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $ChildPerson->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $ChildPerson->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $ChildPerson->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $ChildPerson->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $ChildPerson->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $ChildPerson->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $ChildPerson->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $ChildPerson->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $ChildPerson->getPerson()->getMandatoryUniqueString();
			$dataArray["BirthOrder_BirthOrder_Nr"] = $ChildPerson->getBirthOrder_BirthOrder_Nr();
			$nrRowsAffected = $db->insert("ChildPerson", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function update(ChildPerson $ChildPerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $ChildPerson->getPerson()->getFirstName();
			$dataArray["Person_id"] = $ChildPerson->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $ChildPerson->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $ChildPerson->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $ChildPerson->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $ChildPerson->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $ChildPerson->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $ChildPerson->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $ChildPerson->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $ChildPerson->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $ChildPerson->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $ChildPerson->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $ChildPerson->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $ChildPerson->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $ChildPerson->getPerson()->getMandatoryUniqueString();
			$dataArray["BirthOrder_BirthOrder_Nr"] = $ChildPerson->getBirthOrder_BirthOrder_Nr();
			$whereClause = $db->quoteInto("Person_id = ?", $ChildPerson->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("ChildPerson", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(ChildPerson $ChildPerson) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $ChildPerson->getPerson()->getPerson_id());
			$nrRowsAffected = $db->delete("ChildPerson", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('ChildPersonDao')) {
	class ChildPersonDao extends ChildPersonDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class DeathDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new DeathDao();
		}
		return instance;
	}
	
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
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
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
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
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
			$dataArray["Date_YMD"] = $Death->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $Death->getDeathCause_DeathCause_Type();
			$nrRowsAffected = $db->insert("Death", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
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
			$dataArray["Date_YMD"] = $Death->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $Death->getDeathCause_DeathCause_Type();
			$whereClause = $db->quoteInto("Person_id = ?", $Death->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("Death", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
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
class NaturalDeathDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new NaturalDeathDao();
		}
		return instance;
	}
	
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
				$tempEntity = new NaturalDeath();
				$tempEntity->getDeath()->getPerson()->setFirstName($results["FirstName"]);
				$tempEntity->getDeath()->getPerson()->setDate_YMD($results["Date_YMD"]);
				$tempEntity->getDeath()->getPerson()->setLastName($results["LastName"]);
				$tempEntity->getDeath()->getPerson()->setOptionalUniqueString($results["OptionalUniqueString"]);
				$tempEntity->getDeath()->getPerson()->setHatType_ColorARGB($results["HatType_ColorARGB"]);
				$tempEntity->getDeath()->getPerson()->setHatType_HatTypeStyle_HatTypeStyle_Description($results["HatType_HatTypeStyle_HatTypeStyle_Description"]);
				$tempEntity->getDeath()->getPerson()->setPerson_id($results["Husband_Person_id"]);
				$tempEntity->getDeath()->getPerson()->setOwnsCar_vin($results["OwnsCar_vin"]);
				$tempEntity->getDeath()->getPerson()->setGender_Gender_Code($results["Gender_Gender_Code"]);
				$tempEntity->getDeath()->getPerson()->sethasParents($results["hasParents"]);
				$tempEntity->getDeath()->getPerson()->setValueType1Value($results["ValueType1DoesSomethingElseWith_ValueType1Value"]);
				$tempEntity->getDeath()->getPerson()->setOptionalUniqueDecimal($results["OptionalUniqueDecimal"]);
				$tempEntity->getDeath()->getPerson()->setMandatoryUniqueDecimal($results["MandatoryUniqueDecimal"]);
				$tempEntity->getDeath()->getPerson()->setMandatoryUniqueString($results["MandatoryUniqueString"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new NaturalDeath();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->getDeath()->getPerson()->setFirstName($row["FirstName"]);
			$retVal->getDeath()->getPerson()->setDate_YMD($row["Date_YMD"]);
			$retVal->getDeath()->getPerson()->setLastName($row["LastName"]);
			$retVal->getDeath()->getPerson()->setOptionalUniqueString($row["OptionalUniqueString"]);
			$retVal->getDeath()->getPerson()->setHatType_ColorARGB($row["HatType_ColorARGB"]);
			$retVal->getDeath()->getPerson()->setHatType_HatTypeStyle_HatTypeStyle_Description($row["HatType_HatTypeStyle_HatTypeStyle_Description"]);
			$retVal->getDeath()->getPerson()->setPerson_id($row["Husband_Person_id"]);
			$retVal->getDeath()->getPerson()->setOwnsCar_vin($row["OwnsCar_vin"]);
			$retVal->getDeath()->getPerson()->setGender_Gender_Code($row["Gender_Gender_Code"]);
			$retVal->getDeath()->getPerson()->sethasParents($row["hasParents"]);
			$retVal->getDeath()->getPerson()->setValueType1Value($row["ValueType1DoesSomethingElseWith_ValueType1Value"]);
			$retVal->getDeath()->getPerson()->setOptionalUniqueDecimal($row["OptionalUniqueDecimal"]);
			$retVal->getDeath()->getPerson()->setMandatoryUniqueDecimal($row["MandatoryUniqueDecimal"]);
			$retVal->getDeath()->getPerson()->setMandatoryUniqueString($row["MandatoryUniqueString"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function insert(NaturalDeath $NaturalDeath) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $NaturalDeath->getDeath()->getPerson()->getFirstName();
			$dataArray["Person_id"] = $NaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $NaturalDeath->getDeath()->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $NaturalDeath->getDeath()->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $NaturalDeath->getDeath()->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $NaturalDeath->getDeath()->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $NaturalDeath->getDeath()->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $NaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $NaturalDeath->getDeath()->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $NaturalDeath->getDeath()->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $NaturalDeath->getDeath()->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $NaturalDeath->getDeath()->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $NaturalDeath->getDeath()->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $NaturalDeath->getDeath()->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $NaturalDeath->getDeath()->getPerson()->getMandatoryUniqueString();
			$dataArray["Date_YMD"] = $NaturalDeath->getDeath()->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $NaturalDeath->getDeath()->getDeathCause_DeathCause_Type();
			$dataArray["isFromProstateCancer"] = $NaturalDeath->getisFromProstateCancer();
			$nrRowsAffected = $db->insert("NaturalDeath", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function update(NaturalDeath $NaturalDeath) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $NaturalDeath->getDeath()->getPerson()->getFirstName();
			$dataArray["Person_id"] = $NaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $NaturalDeath->getDeath()->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $NaturalDeath->getDeath()->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $NaturalDeath->getDeath()->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $NaturalDeath->getDeath()->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $NaturalDeath->getDeath()->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $NaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $NaturalDeath->getDeath()->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $NaturalDeath->getDeath()->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $NaturalDeath->getDeath()->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $NaturalDeath->getDeath()->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $NaturalDeath->getDeath()->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $NaturalDeath->getDeath()->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $NaturalDeath->getDeath()->getPerson()->getMandatoryUniqueString();
			$dataArray["Date_YMD"] = $NaturalDeath->getDeath()->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $NaturalDeath->getDeath()->getDeathCause_DeathCause_Type();
			$dataArray["isFromProstateCancer"] = $NaturalDeath->getisFromProstateCancer();
			$whereClause = $db->quoteInto("Person_id = ?", $NaturalDeath->getDeath()->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("NaturalDeath", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(NaturalDeath $NaturalDeath) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $NaturalDeath->getDeath()->getPerson()->getPerson_id());
			$nrRowsAffected = $db->delete("NaturalDeath", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('NaturalDeathDao')) {
	class NaturalDeathDao extends NaturalDeathDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class UnnaturalDeathDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new UnnaturalDeathDao();
		}
		return instance;
	}
	
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
				$tempEntity = new UnnaturalDeath();
				$tempEntity->getDeath()->getPerson()->setFirstName($results["FirstName"]);
				$tempEntity->getDeath()->getPerson()->setDate_YMD($results["Date_YMD"]);
				$tempEntity->getDeath()->getPerson()->setLastName($results["LastName"]);
				$tempEntity->getDeath()->getPerson()->setOptionalUniqueString($results["OptionalUniqueString"]);
				$tempEntity->getDeath()->getPerson()->setHatType_ColorARGB($results["HatType_ColorARGB"]);
				$tempEntity->getDeath()->getPerson()->setHatType_HatTypeStyle_HatTypeStyle_Description($results["HatType_HatTypeStyle_HatTypeStyle_Description"]);
				$tempEntity->getDeath()->getPerson()->setPerson_id($results["Husband_Person_id"]);
				$tempEntity->getDeath()->getPerson()->setOwnsCar_vin($results["OwnsCar_vin"]);
				$tempEntity->getDeath()->getPerson()->setGender_Gender_Code($results["Gender_Gender_Code"]);
				$tempEntity->getDeath()->getPerson()->sethasParents($results["hasParents"]);
				$tempEntity->getDeath()->getPerson()->setValueType1Value($results["ValueType1DoesSomethingElseWith_ValueType1Value"]);
				$tempEntity->getDeath()->getPerson()->setOptionalUniqueDecimal($results["OptionalUniqueDecimal"]);
				$tempEntity->getDeath()->getPerson()->setMandatoryUniqueDecimal($results["MandatoryUniqueDecimal"]);
				$tempEntity->getDeath()->getPerson()->setMandatoryUniqueString($results["MandatoryUniqueString"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new UnnaturalDeath();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Person_id = ?", $Person_id);
			$row = $db->fetchRow($select);
			$retVal->getDeath()->getPerson()->setFirstName($row["FirstName"]);
			$retVal->getDeath()->getPerson()->setDate_YMD($row["Date_YMD"]);
			$retVal->getDeath()->getPerson()->setLastName($row["LastName"]);
			$retVal->getDeath()->getPerson()->setOptionalUniqueString($row["OptionalUniqueString"]);
			$retVal->getDeath()->getPerson()->setHatType_ColorARGB($row["HatType_ColorARGB"]);
			$retVal->getDeath()->getPerson()->setHatType_HatTypeStyle_HatTypeStyle_Description($row["HatType_HatTypeStyle_HatTypeStyle_Description"]);
			$retVal->getDeath()->getPerson()->setPerson_id($row["Husband_Person_id"]);
			$retVal->getDeath()->getPerson()->setOwnsCar_vin($row["OwnsCar_vin"]);
			$retVal->getDeath()->getPerson()->setGender_Gender_Code($row["Gender_Gender_Code"]);
			$retVal->getDeath()->getPerson()->sethasParents($row["hasParents"]);
			$retVal->getDeath()->getPerson()->setValueType1Value($row["ValueType1DoesSomethingElseWith_ValueType1Value"]);
			$retVal->getDeath()->getPerson()->setOptionalUniqueDecimal($row["OptionalUniqueDecimal"]);
			$retVal->getDeath()->getPerson()->setMandatoryUniqueDecimal($row["MandatoryUniqueDecimal"]);
			$retVal->getDeath()->getPerson()->setMandatoryUniqueString($row["MandatoryUniqueString"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	
	public function insert(UnnaturalDeath $UnnaturalDeath) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $UnnaturalDeath->getDeath()->getPerson()->getFirstName();
			$dataArray["Person_id"] = $UnnaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $UnnaturalDeath->getDeath()->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $UnnaturalDeath->getDeath()->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $UnnaturalDeath->getDeath()->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $UnnaturalDeath->getDeath()->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $UnnaturalDeath->getDeath()->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $UnnaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $UnnaturalDeath->getDeath()->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $UnnaturalDeath->getDeath()->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $UnnaturalDeath->getDeath()->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $UnnaturalDeath->getDeath()->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $UnnaturalDeath->getDeath()->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $UnnaturalDeath->getDeath()->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $UnnaturalDeath->getDeath()->getPerson()->getMandatoryUniqueString();
			$dataArray["Date_YMD"] = $UnnaturalDeath->getDeath()->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $UnnaturalDeath->getDeath()->getDeathCause_DeathCause_Type();
			$dataArray["isViolent"] = $UnnaturalDeath->getisViolent();
			$dataArray["isBloody"] = $UnnaturalDeath->getisBloody();
			$nrRowsAffected = $db->insert("UnnaturalDeath", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function update(UnnaturalDeath $UnnaturalDeath) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["FirstName"] = $UnnaturalDeath->getDeath()->getPerson()->getFirstName();
			$dataArray["Person_id"] = $UnnaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["Date_YMD"] = $UnnaturalDeath->getDeath()->getPerson()->getDate_YMD();
			$dataArray["LastName"] = $UnnaturalDeath->getDeath()->getPerson()->getLastName();
			$dataArray["OptionalUniqueString"] = $UnnaturalDeath->getDeath()->getPerson()->getOptionalUniqueString();
			$dataArray["HatType_ColorARGB"] = $UnnaturalDeath->getDeath()->getPerson()->getHatType_ColorARGB();
			$dataArray["HatType_HatTypeStyle_HatTypeStyle_Description"] = $UnnaturalDeath->getDeath()->getPerson()->getHatType_HatTypeStyle_HatTypeStyle_Description();
			$dataArray["Husband"] = $UnnaturalDeath->getDeath()->getPerson()->getPerson_id();
			$dataArray["OwnsCar_vin"] = $UnnaturalDeath->getDeath()->getPerson()->getOwnsCar_vin();
			$dataArray["Gender_Gender_Code"] = $UnnaturalDeath->getDeath()->getPerson()->getGender_Gender_Code();
			$dataArray["hasParents"] = $UnnaturalDeath->getDeath()->getPerson()->gethasParents();
			$dataArray["ValueType1DoesSomethingElseWith"] = $UnnaturalDeath->getDeath()->getPerson()->getValueType1Value();
			$dataArray["OptionalUniqueDecimal"] = $UnnaturalDeath->getDeath()->getPerson()->getOptionalUniqueDecimal();
			$dataArray["MandatoryUniqueDecimal"] = $UnnaturalDeath->getDeath()->getPerson()->getMandatoryUniqueDecimal();
			$dataArray["MandatoryUniqueString"] = $UnnaturalDeath->getDeath()->getPerson()->getMandatoryUniqueString();
			$dataArray["Date_YMD"] = $UnnaturalDeath->getDeath()->getDate_YMD();
			$dataArray["DeathCause_DeathCause_Type"] = $UnnaturalDeath->getDeath()->getDeathCause_DeathCause_Type();
			$dataArray["isViolent"] = $UnnaturalDeath->getisViolent();
			$dataArray["isBloody"] = $UnnaturalDeath->getisBloody();
			$whereClause = $db->quoteInto("Person_id = ?", $UnnaturalDeath->getDeath()->getPerson()->getPerson_id());
			$nrRowsAffected = $db->update("UnnaturalDeath", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(UnnaturalDeath $UnnaturalDeath) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $UnnaturalDeath->getDeath()->getPerson()->getPerson_id());
			$nrRowsAffected = $db->delete("UnnaturalDeath", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('UnnaturalDeathDao')) {
	class UnnaturalDeathDao extends UnnaturalDeathDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class TaskDaoBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new TaskDao();
		}
		return instance;
	}
	
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
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new ValueType1Dao();
		}
		return instance;
	}
	
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
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new PersonBoughtCarFromPersonOnDateDao();
		}
		return instance;
	}
	
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
	
	public function update(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Buyer"] = $PersonBoughtCarFromPersonOnDate->getPerson_id();
			$dataArray["CarSold_vin"] = $PersonBoughtCarFromPersonOnDate->getCarSold_vin();
			$dataArray["Seller"] = $PersonBoughtCarFromPersonOnDate->getPerson_id();
			$dataArray["SaleDate_YMD"] = $PersonBoughtCarFromPersonOnDate->getSaleDate_YMD();
			$whereClause = $db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id()).$db->quoteInto("CarSold_vin = ?", $PersonBoughtCarFromPersonOnDate->getCarSold_vin()).$db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id());
			$nrRowsAffected = $db->update("PersonBoughtCarFromPersonOnDate", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(PersonBoughtCarFromPersonOnDate $PersonBoughtCarFromPersonOnDate) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id()).$db->quoteInto("CarSold_vin = ?", $PersonBoughtCarFromPersonOnDate->getCarSold_vin()).$db->quoteInto("Person_id = ?", $PersonBoughtCarFromPersonOnDate->getPerson_id());
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
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(isset(instance))) {
			instance = new ReviewDao();
		}
		return instance;
	}
	
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
	
	public function update(Review $Review) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Car_vin"] = $Review->getCar_vin();
			$dataArray["Rating_Nr_Integer"] = $Review->getRating_Nr_Integer();
			$dataArray["Criterion_Name"] = $Review->getCriterion_Name();
			$whereClause = $db->quoteInto("Car_vin = ?", $Review->getCar_vin()).$db->quoteInto("Criterion_Name = ?", $Review->getCriterion_Name());
			$nrRowsAffected = $db->update("Review", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function delete(Review $Review) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Car_vin = ?", $Review->getCar_vin()).$db->quoteInto("Criterion_Name = ?", $Review->getCriterion_Name());
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
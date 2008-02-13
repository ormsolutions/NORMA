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
				$tempEntity->setCountry_name($results["Country_Country_name"]);
				$tempEntity->setLastName($results["LastName"]);
				$tempEntity->setFirstName($results["FirstName"]);
				$tempEntity->setTitle($results["Title"]);
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
			$retVal->setCountry_name($row["Country_Country_name"]);
			$retVal->setLastName($row["LastName"]);
			$retVal->setFirstName($row["FirstName"]);
			$retVal->setTitle($row["Title"]);
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
			$dataArray["Person_id"] = $Person->getPerson_id();
			$dataArray["Country"] = $Person->getCountry_name();
			$dataArray["LastName"] = $Person->getLastName();
			$dataArray["FirstName"] = $Person->getFirstName();
			$dataArray["Title"] = $Person->getTitle();
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
			$dataArray["Person_id"] = $Person->getPerson_id();
			$dataArray["Country"] = $Person->getCountry_name();
			$dataArray["LastName"] = $Person->getLastName();
			$dataArray["FirstName"] = $Person->getFirstName();
			$dataArray["Title"] = $Person->getTitle();
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
}
if (!class_exists('PersonDao')) {
	class PersonDao extends PersonDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class CountryDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of CountryDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new CountryDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of Country objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Country", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Country();
				$tempEntity->setRegion_Region_code($results["Region_Region_code"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified Countryobject from the database</summary>
	public function getSingle(/*string*/ $Country_name) {
		try {
			$retVal = new Country();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("Country", "*");
			$select->where("Country_name = ?", $Country_name);
			$row = $db->fetchRow($select);
			$retVal->setRegion_Region_code($row["Region_Region_code"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given Country object into the database</summary>
	public function insert(Country $Country) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Country_name"] = $Country->getCountry_name();
			$dataArray["Region_Region_code"] = $Country->getRegion_Region_code();
			$nrRowsAffected = $db->insert("Country", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given Country object in the database</summary>
	public function update(Country $Country) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["Country_name"] = $Country->getCountry_name();
			$dataArray["Region_Region_code"] = $Country->getRegion_Region_code();
			$whereClause = $db->quoteInto("Country_name = ?", $Country->getCountry_name());
			$nrRowsAffected = $db->update("Country", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given Country object from the database</summary>
	public function delete(Country $Country) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("Country_name = ?", $Country->getCountry_name());
			$nrRowsAffected = $db->delete("Country", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of Person objects by the given Country object</summary>
	public function get_Person_Collection_By_Country(/*string*/ $Country_name) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Person", "*");
			$select->where("Country_name = ?", $Country_name);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new Person();
				$tempEntity->setCountry_name($results["Country_Country_name"]);
				$tempEntity->setLastName($results["LastName"]);
				$tempEntity->setFirstName($results["FirstName"]);
				$tempEntity->setTitle($results["Title"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('CountryDao')) {
	class CountryDao extends CountryDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
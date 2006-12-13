<?php
static class DataAccessBase {
	static $params = null;
	
	public static function getDataAdapter() {
		return Zend_Db::factory(getpdoType(), getparams());
	}
	private static function get() {
		if (params == null) {
			params = .global::array();
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
		if (!(.global::isset(instance))) {
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
			$db->setFetchMode(.global::$PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = .global::count($result);
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
	
	public function getSingle(/*int*/ $Person_id) {
		try {
			$retVal = new Person();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(.global::$PDO::FETCH_ASSOC);
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
	
	public function insert(Person $Person) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = .global::array();
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
	
	public function update(Person $Person) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = .global::array();
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
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset(instance))) {
			instance = new CountryDao();
		}
		return instance;
	}
	
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("Country", "*");
			$db->setFetchMode(.global::$PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = .global::count($result);
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
	
	public function getSingle(/*string*/ $Country_name) {
		try {
			$retVal = new Country();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(.global::$PDO::FETCH_ASSOC);
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
	
	public function insert(Country $Country) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = .global::array();
			$dataArray["Country_name"] = $Country->getCountry_name();
			$dataArray["Region_Region_code"] = $Country->getRegion_Region_code();
			$nrRowsAffected = $db->insert("Country", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
	public function update(Country $Country) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = .global::array();
			$dataArray["Country_name"] = $Country->getCountry_name();
			$dataArray["Region_Region_code"] = $Country->getRegion_Region_code();
			$whereClause = $db->quoteInto("Country_name = ?", $Country->getCountry_name());
			$nrRowsAffected = $db->update("Country", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	
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
			$db->setFetchMode(.global::$PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = .global::count($result);
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
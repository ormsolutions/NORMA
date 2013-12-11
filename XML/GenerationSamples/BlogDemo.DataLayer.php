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
class BlogEntryLabelDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of BlogEntryLabelDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new BlogEntryLabelDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of BlogEntryLabel objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntryLabel", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogEntryLabel();
				$tempEntity->setBlogEntry_Id($results["blogEntryId_BlogEntry_Id"]);
				$tempEntity->setBlogLabel_Id($results["blogLabelId_BlogLabel_Id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified BlogEntryLabelobject from the database</summary>
	public function getSingle(/*decimal*/ $BlogEntry_Id, /*int*/ $BlogLabel_Id) {
		try {
			$retVal = new BlogEntryLabel();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("BlogEntryLabel", "*");
			$select->where("BlogEntry_Id = ?", $BlogEntry_Id);
			$select->where("BlogLabel_Id = ?", $BlogLabel_Id);
			$row = $db->fetchRow($select);
			$retVal->setBlogEntry_Id($row["blogEntryId_BlogEntry_Id"]);
			$retVal->setBlogLabel_Id($row["blogLabelId_BlogLabel_Id"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given BlogEntryLabel object into the database</summary>
	public function insert(BlogEntryLabel $BlogEntryLabel) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["blogEntryId"] = $BlogEntryLabel->getBlogEntry_Id();
			$dataArray["blogLabelId"] = $BlogEntryLabel->getBlogLabel_Id();
			$nrRowsAffected = $db->insert("BlogEntryLabel", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given BlogEntryLabel object in the database</summary>
	public function update(BlogEntryLabel $BlogEntryLabel) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["blogEntryId"] = $BlogEntryLabel->getBlogEntry_Id();
			$dataArray["blogLabelId"] = $BlogEntryLabel->getBlogLabel_Id();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $BlogEntryLabel->getBlogEntry_Id()) . $db->quoteInto("BlogLabel_Id = ?", $BlogEntryLabel->getBlogLabel_Id());
			$nrRowsAffected = $db->update("BlogEntryLabel", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given BlogEntryLabel object from the database</summary>
	public function delete(BlogEntryLabel $BlogEntryLabel) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $BlogEntryLabel->getBlogEntry_Id()) . $db->quoteInto("BlogLabel_Id = ?", $BlogEntryLabel->getBlogLabel_Id());
			$nrRowsAffected = $db->delete("BlogEntryLabel", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('BlogEntryLabelDao')) {
	class BlogEntryLabelDao extends BlogEntryLabelDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogEntryDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of BlogEntryDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new BlogEntryDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of BlogEntry objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogEntry();
				$tempEntity->setfirstName($results["userId_firstName"]);
				$tempEntity->setlastName($results["userId_lastName"]);
				$tempEntity->setentryTitle($results["entryTitle"]);
				$tempEntity->setentryBody($results["entryBody"]);
				$tempEntity->setpostedDate_MDYValue($results["postedDate_MDYValue"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified BlogEntryobject from the database</summary>
	public function getSingle(/*decimal*/ $BlogEntry_Id) {
		try {
			$retVal = new BlogEntry();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$select->where("BlogEntry_Id = ?", $BlogEntry_Id);
			$row = $db->fetchRow($select);
			$retVal->setfirstName($row["userId_firstName"]);
			$retVal->setlastName($row["userId_lastName"]);
			$retVal->setentryTitle($row["entryTitle"]);
			$retVal->setentryBody($row["entryBody"]);
			$retVal->setpostedDate_MDYValue($row["postedDate_MDYValue"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given BlogEntry object into the database</summary>
	public function insert(BlogEntry $BlogEntry) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogEntry_Id"] = $BlogEntry->getBlogEntry_Id();
			$dataArray["entryTitle"] = $BlogEntry->getentryTitle();
			$dataArray["entryBody"] = $BlogEntry->getentryBody();
			$dataArray["postedDate_MDYValue"] = $BlogEntry->getpostedDate_MDYValue();
			$nrRowsAffected = $db->insert("BlogEntry", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given BlogEntry object in the database</summary>
	public function update(BlogEntry $BlogEntry) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogEntry_Id"] = $BlogEntry->getBlogEntry_Id();
			$dataArray["entryTitle"] = $BlogEntry->getentryTitle();
			$dataArray["entryBody"] = $BlogEntry->getentryBody();
			$dataArray["postedDate_MDYValue"] = $BlogEntry->getpostedDate_MDYValue();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $BlogEntry->getBlogEntry_Id());
			$nrRowsAffected = $db->update("BlogEntry", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given BlogEntry object from the database</summary>
	public function delete(BlogEntry $BlogEntry) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $BlogEntry->getBlogEntry_Id());
			$nrRowsAffected = $db->delete("BlogEntry", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of BlogEntryLabel objects by the given BlogEntry object</summary>
	public function get_BlogEntryLabel_Collection_By_blogEntryId(/*decimal*/ $BlogEntry_Id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntryLabel", "*");
			$select->where("BlogEntry_Id = ?", $BlogEntry_Id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogEntryLabel();
				$tempEntity->setBlogEntry_Id($results["blogEntryId_BlogEntry_Id"]);
				$tempEntity->setBlogLabel_Id($results["blogLabelId_BlogLabel_Id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('BlogEntryDao')) {
	class BlogEntryDao extends BlogEntryDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogCommentDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of BlogCommentDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new BlogCommentDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of BlogComment objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogComment();
				$tempEntity->getBlogEntry()->setfirstName($results["userId_firstName"]);
				$tempEntity->getBlogEntry()->setlastName($results["userId_lastName"]);
				$tempEntity->getBlogEntry()->setentryTitle($results["entryTitle"]);
				$tempEntity->getBlogEntry()->setentryBody($results["entryBody"]);
				$tempEntity->getBlogEntry()->setpostedDate_MDYValue($results["postedDate_MDYValue"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified BlogCommentobject from the database</summary>
	public function getSingle(/*decimal*/ $BlogEntry_Id) {
		try {
			$retVal = new BlogComment();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$select->where("BlogEntry_Id = ?", $BlogEntry_Id);
			$row = $db->fetchRow($select);
			$retVal->getBlogEntry()->setfirstName($row["userId_firstName"]);
			$retVal->getBlogEntry()->setlastName($row["userId_lastName"]);
			$retVal->getBlogEntry()->setentryTitle($row["entryTitle"]);
			$retVal->getBlogEntry()->setentryBody($row["entryBody"]);
			$retVal->getBlogEntry()->setpostedDate_MDYValue($row["postedDate_MDYValue"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given BlogComment object into the database</summary>
	public function insert(BlogComment $BlogComment) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogEntry_Id"] = $BlogComment->getBlogEntry()->getBlogEntry_Id();
			$dataArray["entryTitle"] = $BlogComment->getBlogEntry()->getentryTitle();
			$dataArray["entryBody"] = $BlogComment->getBlogEntry()->getentryBody();
			$dataArray["postedDate_MDYValue"] = $BlogComment->getBlogEntry()->getpostedDate_MDYValue();
			$nrRowsAffected = $db->insert("BlogComment", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given BlogComment object in the database</summary>
	public function update(BlogComment $BlogComment) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogEntry_Id"] = $BlogComment->getBlogEntry()->getBlogEntry_Id();
			$dataArray["entryTitle"] = $BlogComment->getBlogEntry()->getentryTitle();
			$dataArray["entryBody"] = $BlogComment->getBlogEntry()->getentryBody();
			$dataArray["postedDate_MDYValue"] = $BlogComment->getBlogEntry()->getpostedDate_MDYValue();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $BlogComment->getBlogEntry()->getBlogEntry_Id());
			$nrRowsAffected = $db->update("BlogComment", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given BlogComment object from the database</summary>
	public function delete(BlogComment $BlogComment) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $BlogComment->getBlogEntry()->getBlogEntry_Id());
			$nrRowsAffected = $db->delete("BlogComment", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
}
if (!class_exists('BlogCommentDao')) {
	class BlogCommentDao extends BlogCommentDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class NonCommentEntryDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of NonCommentEntryDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new NonCommentEntryDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of NonCommentEntry objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new NonCommentEntry();
				$tempEntity->getBlogEntry()->setfirstName($results["userId_firstName"]);
				$tempEntity->getBlogEntry()->setlastName($results["userId_lastName"]);
				$tempEntity->getBlogEntry()->setentryTitle($results["entryTitle"]);
				$tempEntity->getBlogEntry()->setentryBody($results["entryBody"]);
				$tempEntity->getBlogEntry()->setpostedDate_MDYValue($results["postedDate_MDYValue"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified NonCommentEntryobject from the database</summary>
	public function getSingle(/*decimal*/ $BlogEntry_Id) {
		try {
			$retVal = new NonCommentEntry();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$select->where("BlogEntry_Id = ?", $BlogEntry_Id);
			$row = $db->fetchRow($select);
			$retVal->getBlogEntry()->setfirstName($row["userId_firstName"]);
			$retVal->getBlogEntry()->setlastName($row["userId_lastName"]);
			$retVal->getBlogEntry()->setentryTitle($row["entryTitle"]);
			$retVal->getBlogEntry()->setentryBody($row["entryBody"]);
			$retVal->getBlogEntry()->setpostedDate_MDYValue($row["postedDate_MDYValue"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given NonCommentEntry object into the database</summary>
	public function insert(NonCommentEntry $NonCommentEntry) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogEntry_Id"] = $NonCommentEntry->getBlogEntry()->getBlogEntry_Id();
			$dataArray["entryTitle"] = $NonCommentEntry->getBlogEntry()->getentryTitle();
			$dataArray["entryBody"] = $NonCommentEntry->getBlogEntry()->getentryBody();
			$dataArray["postedDate_MDYValue"] = $NonCommentEntry->getBlogEntry()->getpostedDate_MDYValue();
			$nrRowsAffected = $db->insert("NonCommentEntry", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given NonCommentEntry object in the database</summary>
	public function update(NonCommentEntry $NonCommentEntry) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogEntry_Id"] = $NonCommentEntry->getBlogEntry()->getBlogEntry_Id();
			$dataArray["entryTitle"] = $NonCommentEntry->getBlogEntry()->getentryTitle();
			$dataArray["entryBody"] = $NonCommentEntry->getBlogEntry()->getentryBody();
			$dataArray["postedDate_MDYValue"] = $NonCommentEntry->getBlogEntry()->getpostedDate_MDYValue();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $NonCommentEntry->getBlogEntry()->getBlogEntry_Id());
			$nrRowsAffected = $db->update("NonCommentEntry", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given NonCommentEntry object from the database</summary>
	public function delete(NonCommentEntry $NonCommentEntry) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("BlogEntry_Id = ?", $NonCommentEntry->getBlogEntry()->getBlogEntry_Id());
			$nrRowsAffected = $db->delete("NonCommentEntry", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of BlogComment objects by the given NonCommentEntry object</summary>
	public function get_BlogComment_Collection_By_parentEntryId(/*decimal*/ $BlogEntry_Id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$select->where("BlogEntry_Id = ?", $BlogEntry_Id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogComment();
				$tempEntity->getBlogEntry()->setfirstName($results["userId_firstName"]);
				$tempEntity->getBlogEntry()->setlastName($results["userId_lastName"]);
				$tempEntity->getBlogEntry()->setentryTitle($results["entryTitle"]);
				$tempEntity->getBlogEntry()->setentryBody($results["entryBody"]);
				$tempEntity->getBlogEntry()->setpostedDate_MDYValue($results["postedDate_MDYValue"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('NonCommentEntryDao')) {
	class NonCommentEntryDao extends NonCommentEntryDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class UserDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of UserDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new UserDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of User objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("User", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new User();
				$tempEntity->setfirstName($results["firstName"]);
				$tempEntity->setlastName($results["lastName"]);
				$tempEntity->setusername($results["username"]);
				$tempEntity->setpassword($results["password"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified Userobject from the database</summary>
	public function getSingle(/*string*/ $firstName, /*string*/ $lastName) {
		try {
			$retVal = new User();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("User", "*");
			$select->where("firstName = ?", $firstName);
			$select->where("lastName = ?", $lastName);
			$row = $db->fetchRow($select);
			$retVal->setfirstName($row["firstName"]);
			$retVal->setlastName($row["lastName"]);
			$retVal->setusername($row["username"]);
			$retVal->setpassword($row["password"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given User object into the database</summary>
	public function insert(User $User) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["firstName"] = $User->getfirstName();
			$dataArray["lastName"] = $User->getlastName();
			$dataArray["username"] = $User->getusername();
			$dataArray["password"] = $User->getpassword();
			$nrRowsAffected = $db->insert("User", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given User object in the database</summary>
	public function update(User $User) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["firstName"] = $User->getfirstName();
			$dataArray["lastName"] = $User->getlastName();
			$dataArray["username"] = $User->getusername();
			$dataArray["password"] = $User->getpassword();
			$whereClause = $db->quoteInto("firstName = ?", $User->getfirstName()) . $db->quoteInto("lastName = ?", $User->getlastName());
			$nrRowsAffected = $db->update("User", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given User object from the database</summary>
	public function delete(User $User) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("firstName = ?", $User->getfirstName()) . $db->quoteInto("lastName = ?", $User->getlastName());
			$nrRowsAffected = $db->delete("User", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of BlogEntry objects by the given User object</summary>
	public function get_BlogEntry_Collection_By_userId(/*string*/ $firstName, /*string*/ $lastName) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntry", "*");
			$select->where("firstName = ?", $firstName);
			$select->where("lastName = ?", $lastName);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogEntry();
				$tempEntity->setfirstName($results["userId_firstName"]);
				$tempEntity->setlastName($results["userId_lastName"]);
				$tempEntity->setentryTitle($results["entryTitle"]);
				$tempEntity->setentryBody($results["entryBody"]);
				$tempEntity->setpostedDate_MDYValue($results["postedDate_MDYValue"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('UserDao')) {
	class UserDao extends UserDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogLabelDaoBase {
	private static $instance;
	// <summary>Instantiates a new instance of BlogLabelDao</summary>
	public function __construct() {
	}
	public static function getInstance() {
		if (!isset(instance)) {
			instance = new BlogLabelDao();
		}
		return instance;
	}
	// <summary>Retrieves the entire collection of BlogLabel objects</summary>
	public function getAll() {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogLabel", "*");
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogLabel();
				$tempEntity->settitle($results["title"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Retrieves the specified BlogLabelobject from the database</summary>
	public function getSingle(/*int*/ $BlogLabel_Id) {
		try {
			$retVal = new BlogLabel();
			$db = DataAccess::getDataAdapter();
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$select = $db->select();
			$select->from("BlogLabel", "*");
			$select->where("BlogLabel_Id = ?", $BlogLabel_Id);
			$row = $db->fetchRow($select);
			$retVal->settitle($row["title"]);
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
	// <summary>Inserts the given BlogLabel object into the database</summary>
	public function insert(BlogLabel $BlogLabel) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogLabel_Id"] = $BlogLabel->getBlogLabel_Id();
			$dataArray["title"] = $BlogLabel->gettitle();
			$nrRowsAffected = $db->insert("BlogLabel", $dataArray);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Updates the given BlogLabel object in the database</summary>
	public function update(BlogLabel $BlogLabel) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$dataArray = array();
			$dataArray["BlogLabel_Id"] = $BlogLabel->getBlogLabel_Id();
			$dataArray["title"] = $BlogLabel->gettitle();
			$whereClause = $db->quoteInto("BlogLabel_Id = ?", $BlogLabel->getBlogLabel_Id());
			$nrRowsAffected = $db->update("BlogLabel", $dataArray, $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Deletes the given BlogLabel object from the database</summary>
	public function delete(BlogLabel $BlogLabel) {
		$retVal = false;
		try {
			$db = DataAccess::getDataAdapter();
			$whereClause = $db->quoteInto("BlogLabel_Id = ?", $BlogLabel->getBlogLabel_Id());
			$nrRowsAffected = $db->delete("BlogLabel", $whereClause);
		}
		catch (Exception $exc) {
		}
		return $retVal;
	}
	// <summary>Retrieves a collection of BlogEntryLabel objects by the given BlogLabel object</summary>
	public function get_BlogEntryLabel_Collection_By_blogLabelId(/*int*/ $BlogLabel_Id) {
		try {
			$retVal = null;
			$db = DataAccess::getDataAdapter();
			$select = $db->select();
			$select->from("BlogEntryLabel", "*");
			$select->where("BlogLabel_Id = ?", $BlogLabel_Id);
			$db->setFetchMode(PDO::FETCH_ASSOC);
			$result = $db->fetchPairs($select);
			$rowCount = count($result);
			for ($i = 0; $i < $rowCount; ++$i) {
				$tempEntity = new BlogEntryLabel();
				$tempEntity->setBlogEntry_Id($results["blogEntryId_BlogEntry_Id"]);
				$tempEntity->setBlogLabel_Id($results["blogLabelId_BlogLabel_Id"]);
				$retVal[] = $tempEntity;
			}
			return $retVal;
		}
		catch (Exception $exc) {
			return null;
		}
	}
}
if (!class_exists('BlogLabelDao')) {
	class BlogLabelDao extends BlogLabelDaoBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
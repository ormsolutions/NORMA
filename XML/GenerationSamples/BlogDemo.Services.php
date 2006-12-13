<?php
.global::require_once("Entities.php");
.global::require_once("DataLayer.php");
class BlogEntryLabelServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new BlogEntryLabelService();
		}
		return instance;
	}
	
	public function getAll() {
		return BlogEntryLabelDAO::getInstance()->getAll();
	}
	
	public function getSingle( $BlogEntry_Id,  $BlogLabel_Id) {
		return BlogEntryLabelDAO::getInstance()->getSingle($BlogEntry_Id, $BlogLabel_Id);
	}
	
	public function insert(BlogEntryLabel $BlogEntryLabel) {
		return BlogEntryLabelDAO::getInstance()->insert($BlogEntryLabel);
	}
	
	public function update(BlogEntryLabel $BlogEntryLabel) {
		return BlogEntryLabelDAO::getInstance()->update($BlogEntryLabel);
	}
	
	public function delete(BlogEntryLabel $BlogEntryLabel) {
		return BlogEntryLabelDAO::getInstance()->delete($BlogEntryLabel);
	}
}
if (!class_exists('BlogEntryLabelService')) {
	class BlogEntryLabelService extends BlogEntryLabelServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogEntryServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new BlogEntryService();
		}
		return instance;
	}
	
	public function getAll() {
		return BlogEntryDAO::getInstance()->getAll();
	}
	
	public function getSingle( $BlogEntry_Id) {
		return BlogEntryDAO::getInstance()->getSingle($BlogEntry_Id);
	}
	
	public function insert(BlogEntry $BlogEntry) {
		return BlogEntryDAO::getInstance()->insert($BlogEntry);
	}
	
	public function update(BlogEntry $BlogEntry) {
		return BlogEntryDAO::getInstance()->update($BlogEntry);
	}
	
	public function delete(BlogEntry $BlogEntry) {
		return BlogEntryDAO::getInstance()->delete($BlogEntry);
	}
	// <summary>Retrieves a collection of BlogEntryLabel objects by the given BlogEntry object</summary>
	public function get_BlogEntryLabel_Collection_By_blogEntryId(/*decimal*/ $BlogEntry_Id) {
		return BlogEntryDAO::getInstance()->get_BlogEntryLabel_Collection_By_blogEntryId($BlogEntry_Id);
	}
}
if (!class_exists('BlogEntryService')) {
	class BlogEntryService extends BlogEntryServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogCommentServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new BlogCommentService();
		}
		return instance;
	}
	
	public function getAll() {
		return BlogCommentDAO::getInstance()->getAll();
	}
	
	public function getSingle( $BlogEntry_Id) {
		return BlogCommentDAO::getInstance()->getSingle($BlogEntry_Id);
	}
	
	public function insert(BlogComment $BlogComment) {
		return BlogCommentDAO::getInstance()->insert($BlogComment);
	}
	
	public function update(BlogComment $BlogComment) {
		return BlogCommentDAO::getInstance()->update($BlogComment);
	}
	
	public function delete(BlogComment $BlogComment) {
		return BlogCommentDAO::getInstance()->delete($BlogComment);
	}
}
if (!class_exists('BlogCommentService')) {
	class BlogCommentService extends BlogCommentServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class NonCommentEntryServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new NonCommentEntryService();
		}
		return instance;
	}
	
	public function getAll() {
		return NonCommentEntryDAO::getInstance()->getAll();
	}
	
	public function getSingle( $BlogEntry_Id) {
		return NonCommentEntryDAO::getInstance()->getSingle($BlogEntry_Id);
	}
	
	public function insert(NonCommentEntry $NonCommentEntry) {
		return NonCommentEntryDAO::getInstance()->insert($NonCommentEntry);
	}
	
	public function update(NonCommentEntry $NonCommentEntry) {
		return NonCommentEntryDAO::getInstance()->update($NonCommentEntry);
	}
	
	public function delete(NonCommentEntry $NonCommentEntry) {
		return NonCommentEntryDAO::getInstance()->delete($NonCommentEntry);
	}
	// <summary>Retrieves a collection of BlogComment objects by the given NonCommentEntry object</summary>
	public function get_BlogComment_Collection_By_parentEntryId(/*decimal*/ $BlogEntry_Id) {
		return NonCommentEntryDAO::getInstance()->get_BlogComment_Collection_By_parentEntryId($BlogEntry_Id);
	}
}
if (!class_exists('NonCommentEntryService')) {
	class NonCommentEntryService extends NonCommentEntryServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class UserServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new UserService();
		}
		return instance;
	}
	
	public function getAll() {
		return UserDAO::getInstance()->getAll();
	}
	
	public function getSingle( $firstName,  $lastName) {
		return UserDAO::getInstance()->getSingle($firstName, $lastName);
	}
	
	public function insert(User $User) {
		return UserDAO::getInstance()->insert($User);
	}
	
	public function update(User $User) {
		return UserDAO::getInstance()->update($User);
	}
	
	public function delete(User $User) {
		return UserDAO::getInstance()->delete($User);
	}
	// <summary>Retrieves a collection of BlogEntry objects by the given User object</summary>
	public function get_BlogEntry_Collection_By_userId(/*string*/ $firstName, /*string*/ $lastName) {
		return UserDAO::getInstance()->get_BlogEntry_Collection_By_userId($firstName, $lastName);
	}
}
if (!class_exists('UserService')) {
	class UserService extends UserServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogLabelServiceBase {
	private static $instance;
	
	public function __construct() {
	}
	public static function getInstance() {
		if (!(.global::isset())) {
			instance = new BlogLabelService();
		}
		return instance;
	}
	
	public function getAll() {
		return BlogLabelDAO::getInstance()->getAll();
	}
	
	public function getSingle( $BlogLabel_Id) {
		return BlogLabelDAO::getInstance()->getSingle($BlogLabel_Id);
	}
	
	public function insert(BlogLabel $BlogLabel) {
		return BlogLabelDAO::getInstance()->insert($BlogLabel);
	}
	
	public function update(BlogLabel $BlogLabel) {
		return BlogLabelDAO::getInstance()->update($BlogLabel);
	}
	
	public function delete(BlogLabel $BlogLabel) {
		return BlogLabelDAO::getInstance()->delete($BlogLabel);
	}
	// <summary>Retrieves a collection of BlogEntryLabel objects by the given BlogLabel object</summary>
	public function get_BlogEntryLabel_Collection_By_blogLabelId(/*int*/ $BlogLabel_Id) {
		return BlogLabelDAO::getInstance()->get_BlogEntryLabel_Collection_By_blogLabelId($BlogLabel_Id);
	}
}
if (!class_exists('BlogLabelService')) {
	class BlogLabelService extends BlogLabelServiceBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
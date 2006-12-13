<?php
class BlogEntryLabelBase extends Entity {
	private $BlogEntryLabel_blogEntryId_BlogEntry_Proxy;
	private $BlogEntryLabel_blogLabelId_BlogLabel_Proxy;
	public function __construct() {
		parent::__construct();
		$this->BlogEntryLabel_blogEntryId_BlogEntry_Proxy = new BlogEntryLabel_blogEntryId_BlogEntry_Proxy($this);
		$this->BlogEntryLabel_blogLabelId_BlogLabel_Proxy = new BlogEntryLabel_blogLabelId_BlogLabel_Proxy($this);
	}
	public function addValidationRules() {
	}
	public function setblogEntryId(BlogEntry $value) {
		$this->BlogEntryLabel_blogEntryId_BlogEntry_Proxy->Set($value);
	}
	public function getblogEntryId() {
		return $this->BlogEntryLabel_blogEntryId_BlogEntry_Proxy->Get();
	}
	public function setblogLabelId(BlogLabel $value) {
		$this->BlogEntryLabel_blogLabelId_BlogLabel_Proxy->Set($value);
	}
	public function getblogLabelId() {
		return $this->BlogEntryLabel_blogLabelId_BlogLabel_Proxy->Get();
	}
}
if (!class_exists('BlogEntryLabel')) {
	class BlogEntryLabel extends BlogEntryLabelBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogEntryBase extends Entity {
	private $BlogComment;
	private $NonCommentEntry;
	private $BlogEntry_userId_User_Proxy;
	private $BlogEntry_Id;
	private $entryTitle;
	private $entryBody;
	private $postedDate_MDYValue;
	public function __construct() {
		parent::__construct();
		$this->BlogEntry_userId_User_Proxy = new BlogEntry_userId_User_Proxy($this);
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("BlogEntry_Id"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("entryTitle"));
		$this->validationRules->addValidationRule(new StringLenthValidator("EntryTitle", null, 30));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("entryBody"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("postedDate_MDYValue"));
	}
	public function getBlogComment() {
		return $this->BlogComment;
	}
	public function setBlogComment(BlogComment $value) {
		if ($this->BlogComment != $value) {
			$this->BlogComment = $value;
			$value->setBlogEntry($this);
		}
	}
	public function getNonCommentEntry() {
		return $this->NonCommentEntry;
	}
	public function setNonCommentEntry(NonCommentEntry $value) {
		if ($this->NonCommentEntry != $value) {
			$this->NonCommentEntry = $value;
			$value->setBlogEntry($this);
		}
	}
	public function setuserId(User $value) {
		$this->BlogEntry_userId_User_Proxy->Set($value);
	}
	public function getuserId() {
		return $this->BlogEntry_userId_User_Proxy->Get();
	}
	public function getBlogEntry_Id() {
		return $this->BlogEntry_Id;
	}
	public function setBlogEntry_Id(/*int*/ $value) {
		$this->BlogEntry_Id = $value;
	}
	public function getentryTitle() {
		return $this->entryTitle;
	}
	public function setentryTitle(/*string*/ $value) {
		$this->entryTitle = $value;
	}
	public function getentryBody() {
		return $this->entryBody;
	}
	public function setentryBody(/*string*/ $value) {
		$this->entryBody = $value;
	}
	public function getpostedDate_MDYValue() {
		return $this->postedDate_MDYValue;
	}
	public function setpostedDate_MDYValue(/*int*/ $value) {
		$this->postedDate_MDYValue = $value;
	}
}
if (!class_exists('BlogEntry')) {
	class BlogEntry extends BlogEntryBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogCommentBase extends Entity {
	private $BlogComment_parentEntryId_NonCommentEntry_Proxy;
	private $BlogEntry;
	public function __construct() {
		parent::__construct();
		$this->BlogComment_parentEntryId_NonCommentEntry_Proxy = new BlogComment_parentEntryId_NonCommentEntry_Proxy($this);
	}
	public function addValidationRules() {
	}
	public function getBlogEntry() {
		return $this->BlogEntry;
	}
	public function setBlogEntry(BlogEntry $value) {
		if ($this->BlogEntry != $value) {
			$this->BlogEntry = $value;
			$value->setBlogComment($this);
		}
	}
	public function setparentEntryId(NonCommentEntry $value) {
		$this->BlogComment_parentEntryId_NonCommentEntry_Proxy->Set($value);
	}
	public function getparentEntryId() {
		return $this->BlogComment_parentEntryId_NonCommentEntry_Proxy->Get();
	}
}
if (!class_exists('BlogComment')) {
	class BlogComment extends BlogCommentBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class NonCommentEntryBase extends Entity {
	private $BlogEntry;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
	}
	public function getBlogEntry() {
		return $this->BlogEntry;
	}
	public function setBlogEntry(BlogEntry $value) {
		if ($this->BlogEntry != $value) {
			$this->BlogEntry = $value;
			$value->setNonCommentEntry($this);
		}
	}
}
if (!class_exists('NonCommentEntry')) {
	class NonCommentEntry extends NonCommentEntryBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class UserBase extends Entity {
	private $firstName;
	private $lastName;
	private $username;
	private $password;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("firstName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Name", null, 30));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("lastName"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Name", null, 30));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("username"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Username", null, 30));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("password"));
		$this->validationRules->addValidationRule(new StringLenthValidator("Password", 32, 32));
	}
	public function getfirstName() {
		return $this->firstName;
	}
	public function setfirstName(/*string*/ $value) {
		$this->firstName = $value;
	}
	public function getlastName() {
		return $this->lastName;
	}
	public function setlastName(/*string*/ $value) {
		$this->lastName = $value;
	}
	public function getusername() {
		return $this->username;
	}
	public function setusername(/*string*/ $value) {
		$this->username = $value;
	}
	public function getpassword() {
		return $this->password;
	}
	public function setpassword(/*string*/ $value) {
		$this->password = $value;
	}
}
if (!class_exists('User')) {
	class User extends UserBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
class BlogLabelBase extends Entity {
	private $BlogLabel_Id;
	private $title;
	public function __construct() {
		parent::__construct();
	}
	public function addValidationRules() {
		$this->validationRules->addValidationRule(new RequiredFieldValidator("BlogLabel_Id"));
		$this->validationRules->addValidationRule(new RequiredFieldValidator("title"));
	}
	public function getBlogLabel_Id() {
		return $this->BlogLabel_Id;
	}
	public function setBlogLabel_Id(/*object*/ $value) {
		$this->BlogLabel_Id = $value;
	}
	public function gettitle() {
		return $this->title;
	}
	public function settitle(/*string*/ $value) {
		$this->title = $value;
	}
}
if (!class_exists('BlogLabel')) {
	class BlogLabel extends BlogLabelBase {
		public function __construct() {
			parent::__construct();
		}
	}
}
?>
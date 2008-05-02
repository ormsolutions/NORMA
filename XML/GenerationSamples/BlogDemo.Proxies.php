<?php
// <summary>Class used to proxy a BlogEntry for the role blogEntryId for use inside of a BlogEntryLabel isCollection: false</summary>
class BlogEntryLabel_blogEntryId_BlogEntry_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = BlogEntryLabelDAO::getInstance()->getSingle($this->ref->getBlogEntry_Id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a BlogLabel for the role blogLabelId for use inside of a BlogEntryLabel isCollection: false</summary>
class BlogEntryLabel_blogLabelId_BlogLabel_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = BlogEntryLabelDAO::getInstance()->getSingle($this->ref->getBlogLabel_Id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a User for the role userId for use inside of a BlogEntry isCollection: false</summary>
class BlogEntry_userId_User_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = BlogEntryDAO::getInstance()->getSingle($this->ref->getfirstName(), $this->ref->getlastName());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a BlogEntry for the role blogEntryId for use inside of a BlogEntry isCollection: true</summary>
class BlogEntry_blogEntryId_BlogEntryLabel_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = BlogEntryDAO::getInstance()->get_BlogEntryLabel_Collection_By_blogEntryId($this->ref->getBlogEntry_Id());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a User for the role userId for use inside of a User isCollection: true</summary>
class User_userId_BlogEntry_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = UserDAO::getInstance()->get_BlogEntry_Collection_By_userId($this->ref->getfirstName(), $this->ref->getlastName());
		}
		return $this->value;
	}
}
// <summary>Class used to proxy a BlogLabel for the role blogLabelId for use inside of a BlogLabel isCollection: true</summary>
class BlogLabel_blogLabelId_BlogEntryLabel_Proxy {
	private $ref = null;
	private $value = null;
	public function __construct(/*object*/ $ref) {
		$this->ref = $ref;
	}
	public function get() {
		if (!isset($this->value)) {
			$this->value = BlogLabelDAO::getInstance()->get_BlogEntryLabel_Collection_By_blogLabelId($this->ref->getBlogLabel_Id());
		}
		return $this->value;
	}
}
?>
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE BlogEntry
(
	BlogEntry_Id NUMBER NOT NULL,
	EntryTitle NVARCHAR2(30) NOT NULL,
	EntryBody CHARACTER LARGE OBJECT() NOT NULL,
	PostedDate TIMESTAMP NOT NULL,
	UserIdName2 NVARCHAR2(30) NOT NULL,
	UserIdName1 NVARCHAR2(30) NOT NULL,
	BlogCommentParentEntryId NUMBER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(BlogEntry_Id)
);

CREATE TABLE "User"
(
	Name1 NVARCHAR2(30) NOT NULL,
	Name2 NVARCHAR2(30) NOT NULL,
	Username NVARCHAR2(30) NOT NULL,
	Password NCHAR(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(Name1, Name2)
);

CREATE TABLE BlogLabel
(
	BlogLabel_Id NUMBER NOT NULL,
	Title CHARACTER LARGE OBJECT(),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(BlogLabel_Id)
);

CREATE TABLE BlogEntryLabel
(
	BlogEntryId NUMBER NOT NULL,
	BlogLabelId NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(BlogEntryId, BlogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_BlogEntry_FK1 FOREIGN KEY (UserIdName2, UserIdName1)  REFERENCES "User" (Name1, Name2) ;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_BlogEntry_FK2 FOREIGN KEY (BlogCommentParentEntryId)  REFERENCES BlogEntry (BlogEntry_Id) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_BlogEntryLabel_FK1 FOREIGN KEY (BlogEntryId)  REFERENCES BlogEntry (BlogEntry_Id) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_BlogEntryLabel_FK2 FOREIGN KEY (BlogLabelId)  REFERENCES BlogLabel (BlogLabel_Id) ;

COMMIT WORK;


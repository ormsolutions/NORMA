
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE BlogEntry
(
	blogEntry_Id NUMBER NOT NULL,
	entryTitle NVARCHAR2(30) NOT NULL,
	entryBody CHARACTER LARGE OBJECT() NOT NULL,
	postedDate TIMESTAMP NOT NULL,
	userIdName2 NVARCHAR2(30) NOT NULL,
	userIdName1 NVARCHAR2(30) NOT NULL,
	blogCommentParentEntryId NUMBER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntry_Id)
);

CREATE TABLE "User"
(
	name1 NVARCHAR2(30) NOT NULL,
	name2 NVARCHAR2(30) NOT NULL,
	username NVARCHAR2(30) NOT NULL,
	password NCHAR(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(name1, name2)
);

CREATE TABLE BlogLabel
(
	blogLabel_Id NUMBER NOT NULL,
	title CHARACTER LARGE OBJECT(),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabel_Id)
);

CREATE TABLE BlogEntryLabel
(
	blogEntryId NUMBER NOT NULL,
	blogLabelId NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (userIdName2, userIdName1)  REFERENCES "User" (name1, name2) ;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryId)  REFERENCES BlogEntry (blogEntry_Id) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId)  REFERENCES BlogEntry (blogEntry_Id) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId)  REFERENCES BlogLabel (blogLabel_Id) ;

COMMIT WORK;

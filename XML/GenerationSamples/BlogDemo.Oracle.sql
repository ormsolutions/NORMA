
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE BlogEntry
(
	blogEntryId NUMBER(10,0) NOT NULL,
	entryTitle NVARCHAR2(30) NOT NULL,
	entryBody NCLOB NOT NULL,
	postedDate TIMESTAMP NOT NULL,
	firstName NVARCHAR2(30) NOT NULL,
	lastName NVARCHAR2(30) NOT NULL,
	blogCommentParentEntryIdNonCommentEntryBlogEntryId NUMBER(10,0),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntryId)
);

CREATE TABLE "User"
(
	firstName NVARCHAR2(30) NOT NULL,
	lastName NVARCHAR2(30) NOT NULL,
	username NVARCHAR2(30) NOT NULL,
	password NCHAR(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
);

CREATE TABLE BlogLabel
(
	blogLabelId NUMBER(10,0) NOT NULL,
	title NCLOB,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabelId)
);

CREATE TABLE BlogEntryLabel
(
	blogEntryId NUMBER(10,0) NOT NULL,
	blogLabelId NUMBER(10,0) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (firstName, lastName)  REFERENCES "User" (firstName, lastName) ;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryIdNonCommentEntryBlogEntryId)  REFERENCES BlogEntry (blogEntryId) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId)  REFERENCES BlogEntry (blogEntryId) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId)  REFERENCES BlogLabel (blogLabelId) ;

COMMIT WORK;

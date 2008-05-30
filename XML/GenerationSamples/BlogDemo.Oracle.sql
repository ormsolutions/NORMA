
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE BlogEntry
(
	blogEntryId NUMBER(10,0) NOT NULL,
	entryTitle NVARCHAR2(30) NOT NULL,
	entryBody NCLOB NOT NULL,
	postedDate TIMESTAMP NOT NULL,
	firstName NVARCHAR2(30) NOT NULL,
	lastName NVARCHAR2(30) NOT NULL,
	blogCommentParentEntryIdBlogEntry_Id NUMBER(10,0),
	CONSTRAINT BlogEntry_PK PRIMARY KEY(blogEntryId)
);

CREATE TABLE "User"
(
	firstName NVARCHAR2(30) NOT NULL,
	lastName NVARCHAR2(30) NOT NULL,
	username NVARCHAR2(30) NOT NULL,
	password NCHAR(32) NOT NULL,
	CONSTRAINT User_PK PRIMARY KEY(firstName, lastName)
);

CREATE TABLE BlogLabel
(
	blogLabelId NUMBER(10,0) NOT NULL,
	title NCLOB,
	CONSTRAINT BlogLabel_PK PRIMARY KEY(blogLabelId)
);

CREATE TABLE BlogEntryLabel
(
	blogEntryId NUMBER(10,0) NOT NULL,
	blogLabelId NUMBER(10,0) NOT NULL,
	CONSTRAINT BlogEntryLabel_PK PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (firstName, lastName)  REFERENCES "User" (firstName, lastName) ;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryIdBlogEntry_Id)  REFERENCES BlogEntry (blogEntryId) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId)  REFERENCES BlogEntry (blogEntryId) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId)  REFERENCES BlogLabel (blogLabelId) ;

COMMIT WORK;

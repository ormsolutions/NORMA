CREATE SCHEMA BlogDemo
GO

GO


CREATE TABLE BlogDemo.BlogEntry
(
	blogEntry_Id INTEGER NOT NULL,
	entryTitle NATIONAL CHARACTER VARYING(30) NOT NULL,
	entryBody CHARACTER LARGE OBJECT() NOT NULL,
	postedDate DATETIME NOT NULL,
	userIdName2 NATIONAL CHARACTER VARYING(30) NOT NULL,
	userIdName1 NATIONAL CHARACTER VARYING(30) NOT NULL,
	blogCommentParentEntryId INTEGER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntry_Id)
)
GO


CREATE TABLE BlogDemo."User"
(
	name1 NATIONAL CHARACTER VARYING(30) NOT NULL,
	name2 NATIONAL CHARACTER VARYING(30) NOT NULL,
	username NATIONAL CHARACTER VARYING(30) NOT NULL,
	password NATIONAL CHARACTER(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(name1, name2)
)
GO


CREATE TABLE BlogDemo.BlogLabel
(
	blogLabel_Id INTEGER IDENTITY (1, 1) NOT NULL,
	title CHARACTER LARGE OBJECT(),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabel_Id)
)
GO


CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId INTEGER NOT NULL,
	blogLabelId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
)
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (userIdName2, userIdName1) REFERENCES BlogDemo."User" (name1, name2) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryId) REFERENCES BlogDemo.BlogEntry (blogEntry_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntry_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId) REFERENCES BlogDemo.BlogLabel (blogLabel_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
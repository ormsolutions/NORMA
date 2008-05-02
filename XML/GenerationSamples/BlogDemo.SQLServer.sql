CREATE SCHEMA BlogDemo
GO

GO


CREATE TABLE BlogDemo.BlogEntry
(
	blogEntryId INTEGER NOT NULL,
	entryTitle NATIONAL CHARACTER VARYING(30) NOT NULL,
	entryBody NATIONAL CHARACTER VARYING(MAX) NOT NULL,
	postedDate DATETIME NOT NULL,
	firstName NATIONAL CHARACTER VARYING(30) NOT NULL,
	lastName NATIONAL CHARACTER VARYING(30) NOT NULL,
	blogCommentParentEntryIdNonCommentEntryBlogEntryId INTEGER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntryId)
)
GO


CREATE TABLE BlogDemo."User"
(
	firstName NATIONAL CHARACTER VARYING(30) NOT NULL,
	lastName NATIONAL CHARACTER VARYING(30) NOT NULL,
	username NATIONAL CHARACTER VARYING(30) NOT NULL,
	password NATIONAL CHARACTER(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
)
GO


CREATE TABLE BlogDemo.BlogLabel
(
	blogLabelId INTEGER IDENTITY (1, 1) NOT NULL,
	title NATIONAL CHARACTER VARYING(MAX),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabelId)
)
GO


CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId INTEGER NOT NULL,
	blogLabelId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
)
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (firstName, lastName) REFERENCES BlogDemo."User" (firstName, lastName) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryIdNonCommentEntryBlogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntryId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntryId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId) REFERENCES BlogDemo.BlogLabel (blogLabelId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
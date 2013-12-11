CREATE SCHEMA BlogDemo
GO

GO


CREATE TABLE BlogDemo.BlogEntry
(
	blogEntryId int NOT NULL,
	entryBody nvarchar(max) NOT NULL,
	entryTitle nvarchar(30) NOT NULL,
	firstName nvarchar(30) NOT NULL,
	lastName nvarchar(30) NOT NULL,
	postedDate datetime NOT NULL,
	blogCommentParentEntryId int,
	CONSTRAINT BlogEntry_PK PRIMARY KEY(blogEntryId)
)
GO


CREATE TABLE BlogDemo."User"
(
	firstName nvarchar(30) NOT NULL,
	lastName nvarchar(30) NOT NULL,
	password nchar(32) NOT NULL,
	username nvarchar(30) NOT NULL,
	CONSTRAINT User_PK PRIMARY KEY(firstName, lastName)
)
GO


CREATE TABLE BlogDemo.BlogLabel
(
	blogLabelId int IDENTITY (1, 1) NOT NULL,
	title nvarchar(max),
	CONSTRAINT BlogLabel_PK PRIMARY KEY(blogLabelId)
)
GO


CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId int NOT NULL,
	blogLabelId int NOT NULL,
	CONSTRAINT BlogEntryLabel_PK PRIMARY KEY(blogEntryId, blogLabelId)
)
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (firstName, lastName) REFERENCES BlogDemo."User" (firstName, lastName) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryId) REFERENCES BlogDemo.BlogEntry (blogEntryId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntryId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId) REFERENCES BlogDemo.BlogLabel (blogLabelId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
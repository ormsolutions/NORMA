
START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA BlogDemo;

SET search_path TO BLOGDEMO,"$user",public;

CREATE TABLE BlogDemo.BlogEntry
(
	blogEntryId INTEGER NOT NULL,
	entryTitle CHARACTER VARYING(30) NOT NULL,
	entryBody CHARACTER VARYING NOT NULL,
	postedDate TIMESTAMP NOT NULL,
	firstName CHARACTER VARYING(30) NOT NULL,
	lastName CHARACTER VARYING(30) NOT NULL,
	blogCommentParentEntryIdNonCommentEntryBlogEntryId INTEGER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntryId)
);

CREATE TABLE BlogDemo."User"
(
	firstName CHARACTER VARYING(30) NOT NULL,
	lastName CHARACTER VARYING(30) NOT NULL,
	username CHARACTER VARYING(30) NOT NULL,
	password CHARACTER(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
);

CREATE TABLE BlogDemo.BlogLabel
(
	blogLabelId SERIAL NOT NULL,
	title CHARACTER VARYING,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabelId)
);

CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId INTEGER NOT NULL,
	blogLabelId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (firstName, lastName) REFERENCES BlogDemo."User" (firstName, lastName) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryIdNonCommentEntryBlogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntryId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntryId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId) REFERENCES BlogDemo.BlogLabel (blogLabelId) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;

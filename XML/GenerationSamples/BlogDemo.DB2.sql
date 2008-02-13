
CREATE SCHEMA BlogDemo;

SET SCHEMA 'BLOGDEMO';

CREATE TABLE BlogDemo.BlogEntry
(
	blogEntry_Id INTEGER NOT NULL,
	entryTitle CHARACTER VARYING(30) NOT NULL,
	entryBody CHARACTER LARGE OBJECT() NOT NULL,
	postedDate TIMESTAMP NOT NULL,
	userIdName2 CHARACTER VARYING(30) NOT NULL,
	userIdName1 CHARACTER VARYING(30) NOT NULL,
	blogCommentParentEntryId INTEGER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntry_Id)
);

CREATE TABLE BlogDemo."User"
(
	name1 CHARACTER VARYING(30) NOT NULL,
	name2 CHARACTER VARYING(30) NOT NULL,
	username CHARACTER VARYING(30) NOT NULL,
	password CHARACTER(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(name1, name2)
);

CREATE TABLE BlogDemo.BlogLabel
(
	blogLabel_Id INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	title CHARACTER LARGE OBJECT(),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabel_Id)
);

CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId INTEGER NOT NULL,
	blogLabelId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (userIdName2, userIdName1) REFERENCES BlogDemo."User" (name1, name2) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryId) REFERENCES BlogDemo.BlogEntry (blogEntry_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId) REFERENCES BlogDemo.BlogEntry (blogEntry_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId) REFERENCES BlogDemo.BlogLabel (blogLabel_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT;

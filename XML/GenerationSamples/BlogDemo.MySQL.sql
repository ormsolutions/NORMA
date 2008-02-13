CREATE TABLE BlogEntry
(
	blogEntry_Id INT NOT NULL,
	entryTitle VARCHAR(30) NOT NULL,
	entryBody TEXT() NOT NULL,
	postedDate TIMESTAMP NOT NULL,
	userIdName2 VARCHAR(30) NOT NULL,
	userIdName1 VARCHAR(30) NOT NULL,
	blogCommentParentEntryId INT,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntry_Id)
);

CREATE TABLE `User`
(
	name1 VARCHAR(30) NOT NULL,
	name2 VARCHAR(30) NOT NULL,
	username VARCHAR(30) NOT NULL,
	password CHAR(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(name1, name2)
);

CREATE TABLE BlogLabel
(
	blogLabel_Id INT AUTO_INCREMENT NOT NULL,
	title TEXT(),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabel_Id)
);

CREATE TABLE BlogEntryLabel
(
	blogEntryId INT NOT NULL,
	blogLabelId INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (userIdName2, userIdName1)  REFERENCES `User` (name1, name2)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryId)  REFERENCES BlogEntry (blogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId)  REFERENCES BlogEntry (blogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId)  REFERENCES BlogLabel (blogLabel_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


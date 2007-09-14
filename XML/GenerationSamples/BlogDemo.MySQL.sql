CREATE TABLE BlogEntry
(
	BlogEntry_Id INTEGER NOT NULL,
	EntryTitle VARCHAR(30)  NOT NULL,
	EntryBody TEXT()  NOT NULL,
	PostedDate TIMESTAMP NOT NULL,
	UserIdName2 VARCHAR(30)  NOT NULL,
	UserIdName1 VARCHAR(30)  NOT NULL,
	BlogCommentParentEntryId INTEGER,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(BlogEntry_Id)
);

CREATE TABLE `User`
(
	Name1 VARCHAR(30)  NOT NULL,
	Name2 VARCHAR(30)  NOT NULL,
	Username VARCHAR(30)  NOT NULL,
	Password CHAR(32)  NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(Name1, Name2)
);

CREATE TABLE BlogLabel
(
	BlogLabel_Id INTEGERAUTO_INCREMENT  NOT NULL, 
	Title TEXT() ,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(BlogLabel_Id)
);

CREATE TABLE BlogEntryLabel
(
	BlogEntryId INTEGER NOT NULL,
	BlogLabelId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(BlogEntryId, BlogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (UserIdName2, UserIdName1)  REFERENCES `User` (Name1, Name2)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (BlogCommentParentEntryId)  REFERENCES BlogEntry (BlogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (BlogEntryId)  REFERENCES BlogEntry (BlogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (BlogLabelId)  REFERENCES BlogLabel (BlogLabel_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


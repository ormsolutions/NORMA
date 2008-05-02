
CREATE TABLE BlogEntry
(
	blogEntryId INT NOT NULL,
	entryTitle VARCHAR(30) NOT NULL,
	entryBody LONGTEXT(1073741823) NOT NULL,
	postedDate DATETIME NOT NULL,
	firstName VARCHAR(30) NOT NULL,
	lastName VARCHAR(30) NOT NULL,
	blogCommentParentEntryIdNonCommentEntryBlogEntryId INT,
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(blogEntryId)
);

CREATE TABLE `User`
(
	firstName VARCHAR(30) NOT NULL,
	lastName VARCHAR(30) NOT NULL,
	username VARCHAR(30) NOT NULL,
	password CHAR(32) NOT NULL,
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
);

CREATE TABLE BlogLabel
(
	blogLabelId INT AUTO_INCREMENT NOT NULL,
	title LONGTEXT(1073741823),
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(blogLabelId)
);

CREATE TABLE BlogEntryLabel
(
	blogEntryId INT NOT NULL,
	blogLabelId INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId, blogLabelId)
);

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK1 FOREIGN KEY (firstName, lastName) REFERENCES `User` (firstName, lastName) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_FK2 FOREIGN KEY (blogCommentParentEntryIdNonCommentEntryBlogEntryId) REFERENCES BlogEntry (blogEntryId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK1 FOREIGN KEY (blogEntryId) REFERENCES BlogEntry (blogEntryId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_FK2 FOREIGN KEY (blogLabelId) REFERENCES BlogLabel (blogLabelId) ON DELETE RESTRICT ON UPDATE RESTRICT;

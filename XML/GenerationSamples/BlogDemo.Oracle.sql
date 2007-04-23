SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE BlogEntryLabel
(
	blogEntryId_BlogEntry_Id NUMBER(38) NOT NULL, 
	blogLabelId_BlogLabel_Id NUMBER(38) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
);

CREATE TABLE BlogEntry
(
	BlogEntry_Id NUMBER(38) NOT NULL, 
	entryTitle CHARACTER VARYING(30) NOT NULL, 
	entryBody CHARACTER VARYING() NOT NULL, 
	postedDate_MDYValue NUMBER(38) NOT NULL, 
	userId_firstName CHARACTER VARYING(30) NOT NULL, 
	userId_lastName CHARACTER VARYING(30) NOT NULL, 
	parentEntryId_BlogEntry_Id NUMBER(38) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(BlogEntry_Id)
);

CREATE TABLE "User"
(
	firstName CHARACTER VARYING(30) NOT NULL, 
	lastName CHARACTER VARYING(30) NOT NULL, 
	username CHARACTER VARYING(30) NOT NULL, 
	password CHARACTER(32) CONSTRAINT Password_Chk CHECK ((LENGTH(TRIM(BOTH FROM password))) >= 32) NOT NULL, 
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
);

CREATE TABLE BlogLabel
(
	BlogLabel_Id NUMBER(38) NOT NULL, 
	title CHARACTER VARYING() , 
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(BlogLabel_Id)
);

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_blogEntryId_FK FOREIGN KEY (blogEntryId_BlogEntry_Id)  REFERENCES BlogEntry (BlogEntry_Id) ;

ALTER TABLE BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_blogLabelId_FK FOREIGN KEY (blogLabelId_BlogLabel_Id)  REFERENCES BlogLabel (BlogLabel_Id) ;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_userId_FK FOREIGN KEY (userId_firstName, userId_lastName)  REFERENCES "User" (firstName, lastName) ;

ALTER TABLE BlogEntry ADD CONSTRAINT BlogEntry_parentEntryId_FK FOREIGN KEY (parentEntryId_BlogEntry_Id)  REFERENCES BlogEntry (BlogEntry_Id) ;

COMMIT WORK;


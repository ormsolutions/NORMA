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


CREATE PROCEDURE BlogDemo.InsertBlogEntryLabel
(
	blogEntryId_BlogEntry_Id NUMBER(38) , 
	blogLabelId_BlogLabel_Id NUMBER(38) 
)
AS
	INSERT INTO BlogDemo.BlogEntryLabel(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
	VALUES (blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id);

CREATE PROCEDURE BlogDemo.DeleteBlogEntryLabel
(
	blogEntryId_BlogEntry_Id NUMBER(38) , 
	blogLabelId_BlogLabel_Id NUMBER(38) 
)
AS
	DELETE FROM BlogDemo.BlogEntryLabel
	WHERE blogEntryId_BlogEntry_Id = blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = blogLabelId_BlogLabel_Id;

CREATE PROCEDURE BlogDemo.UBELEIBEI
(
	blogEntryId_BlogEntry_Id NUMBER(38) 
)
AS
	UPDATE BlogDemo.BlogEntryLabel
SET blogEntryId_BlogEntry_Id = blogEntryId_BlogEntry_Id
	WHERE blogEntryId_BlogEntry_Id = blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = blogLabelId_BlogLabel_Id;

CREATE PROCEDURE BlogDemo.UBELLIBLI
(
	blogLabelId_BlogLabel_Id NUMBER(38) 
)
AS
	UPDATE BlogDemo.BlogEntryLabel
SET blogLabelId_BlogLabel_Id = blogLabelId_BlogLabel_Id
	WHERE blogEntryId_BlogEntry_Id = blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = blogLabelId_BlogLabel_Id;

CREATE PROCEDURE BlogDemo.InsertBlogEntry
(
	BlogEntry_Id NUMBER(38) , 
	entryTitle CHARACTER VARYING(30) , 
	entryBody CHARACTER VARYING() , 
	postedDate_MDYValue NUMBER(38) , 
	userId_firstName CHARACTER VARYING(30) , 
	userId_lastName CHARACTER VARYING(30) , 
	parentEntryId_BlogEntry_Id NUMBER(38) 
)
AS
	INSERT INTO BlogDemo.BlogEntry(BlogEntry_Id, entryTitle, entryBody, postedDate_MDYValue, userId_firstName, userId_lastName, parentEntryId_BlogEntry_Id)
	VALUES (BlogEntry_Id, entryTitle, entryBody, postedDate_MDYValue, userId_firstName, userId_lastName, parentEntryId_BlogEntry_Id);

CREATE PROCEDURE BlogDemo.DeleteBlogEntry
(
	BlogEntry_Id NUMBER(38) 
)
AS
	DELETE FROM BlogDemo.BlogEntry
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryBlogEntry_Id
(
	BlogEntry_Id NUMBER(38) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET BlogEntry_Id = BlogEntry_Id
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryentryTitle
(
	entryTitle CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET entryTitle = entryTitle
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryentryBody
(
	entryBody CHARACTER VARYING() 
)
AS
	UPDATE BlogDemo.BlogEntry
SET entryBody = entryBody
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdtBlgEntrypstdDt_MDYVl
(
	postedDate_MDYValue NUMBER(38) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET postedDate_MDYValue = postedDate_MDYValue
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdtBlgEntrysrId_frstNm
(
	userId_firstName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET userId_firstName = userId_firstName
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryuserId_lastName
(
	userId_lastName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET userId_lastName = userId_lastName
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UBEEIBEI
(
	parentEntryId_BlogEntry_Id NUMBER(38) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET parentEntryId_BlogEntry_Id = parentEntryId_BlogEntry_Id
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.InsertUser
(
	firstName CHARACTER VARYING(30) , 
	lastName CHARACTER VARYING(30) , 
	username CHARACTER VARYING(30) , 
	password CHARACTER(32) 
)
AS
	INSERT INTO BlogDemo."User"(firstName, lastName, username, password)
	VALUES (firstName, lastName, username, password);

CREATE PROCEDURE BlogDemo.DeleteUser
(
	firstName CHARACTER VARYING(30) , 
	lastName CHARACTER VARYING(30) 
)
AS
	DELETE FROM BlogDemo."User"
	WHERE firstName = firstName AND 
lastName = lastName;

CREATE PROCEDURE BlogDemo."Update""User""firstName"
(
	firstName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET firstName = firstName
	WHERE firstName = firstName AND 
lastName = lastName;

CREATE PROCEDURE BlogDemo."Update""User""lastName"
(
	lastName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET lastName = lastName
	WHERE firstName = firstName AND 
lastName = lastName;

CREATE PROCEDURE BlogDemo."Update""User""username"
(
	username CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET username = username
	WHERE firstName = firstName AND 
lastName = lastName;

CREATE PROCEDURE BlogDemo."Update""User""password"
(
	password CHARACTER(32) 
)
AS
	UPDATE BlogDemo."User"
SET password = password
	WHERE firstName = firstName AND 
lastName = lastName;

CREATE PROCEDURE BlogDemo.InsertBlogLabel
(
	BlogLabel_Id NUMBER(38) , 
	title CHARACTER VARYING() 
)
AS
	INSERT INTO BlogDemo.BlogLabel(BlogLabel_Id, title)
	VALUES (BlogLabel_Id, title);

CREATE PROCEDURE BlogDemo.DeleteBlogLabel
(
	BlogLabel_Id NUMBER(38) 
)
AS
	DELETE FROM BlogDemo.BlogLabel
	WHERE BlogLabel_Id = BlogLabel_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogLabeltitle
(
	title CHARACTER VARYING() 
)
AS
	UPDATE BlogDemo.BlogLabel
SET title = title
	WHERE BlogLabel_Id = BlogLabel_Id;
COMMIT WORK;


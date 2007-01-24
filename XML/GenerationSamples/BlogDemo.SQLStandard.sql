START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA BlogDemo DEFAULT CHARACTER SET UTF8;

SET SCHEMA 'BLOGDEMO';

CREATE DOMAIN BlogDemo.Password AS CHARACTER(32) CONSTRAINT Password_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 32) ;

CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId_BlogEntry_Id BIGINT NOT NULL, 
	blogLabelId_BlogLabel_Id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
);

CREATE TABLE BlogDemo.BlogEntry
(
	BlogEntry_Id BIGINT NOT NULL, 
	entryTitle CHARACTER VARYING(30) NOT NULL, 
	entryBody CHARACTER VARYING() NOT NULL, 
	postedDate_MDYValue BIGINT NOT NULL, 
	userId_firstName CHARACTER VARYING(30) NOT NULL, 
	userId_lastName CHARACTER VARYING(30) NOT NULL, 
	parentEntryId_BlogEntry_Id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(BlogEntry_Id)
);

CREATE TABLE BlogDemo."User"
(
	firstName CHARACTER VARYING(30) NOT NULL, 
	lastName CHARACTER VARYING(30) NOT NULL, 
	username CHARACTER VARYING(30) NOT NULL, 
	password BlogDemo.Password NOT NULL, 
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
);

CREATE TABLE BlogDemo.BlogLabel
(
	BlogLabel_Id BIGINT GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL, 
	title CHARACTER VARYING() , 
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(BlogLabel_Id)
);

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT blogEntryId_FK FOREIGN KEY (blogEntryId_BlogEntry_Id)  REFERENCES BlogDemo.BlogEntry (BlogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT blogLabelId_FK FOREIGN KEY (blogLabelId_BlogLabel_Id)  REFERENCES BlogDemo.BlogLabel (BlogLabel_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT userId_FK FOREIGN KEY (userId_firstName, userId_lastName)  REFERENCES BlogDemo."User" (firstName, lastName)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT parentEntryId_FK FOREIGN KEY (parentEntryId_BlogEntry_Id)  REFERENCES BlogDemo.BlogEntry (BlogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE PROCEDURE BlogDemo.InsertBlogEntryLabel
(
	blogEntryId_BlogEntry_Id BIGINT , 
	blogLabelId_BlogLabel_Id BIGINT 
)
AS
	INSERT INTO BlogDemo.BlogEntryLabel(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
	VALUES (blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id);

CREATE PROCEDURE BlogDemo.DeleteBlogEntryLabel
(
	blogEntryId_BlogEntry_Id BIGINT , 
	blogLabelId_BlogLabel_Id BIGINT 
)
AS
	DELETE FROM BlogDemo.BlogEntryLabel
	WHERE blogEntryId_BlogEntry_Id = blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = blogLabelId_BlogLabel_Id;

CREATE PROCEDURE BlogDemo.UBELEIBEI
(
	old_blogEntryId_BlogEntry_Id BIGINT , 
	old_blogLabelId_BlogLabel_Id BIGINT , 
	blogEntryId_BlogEntry_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntryLabel
SET blogEntryId_BlogEntry_Id = blogEntryId_BlogEntry_Id
	WHERE blogEntryId_BlogEntry_Id = old_blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = old_blogLabelId_BlogLabel_Id;

CREATE PROCEDURE BlogDemo.UBELLIBLI
(
	old_blogEntryId_BlogEntry_Id BIGINT , 
	old_blogLabelId_BlogLabel_Id BIGINT , 
	blogLabelId_BlogLabel_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntryLabel
SET blogLabelId_BlogLabel_Id = blogLabelId_BlogLabel_Id
	WHERE blogEntryId_BlogEntry_Id = old_blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = old_blogLabelId_BlogLabel_Id;

CREATE PROCEDURE BlogDemo.InsertBlogEntry
(
	BlogEntry_Id BIGINT , 
	entryTitle CHARACTER VARYING(30) , 
	entryBody CHARACTER VARYING() , 
	postedDate_MDYValue BIGINT , 
	userId_firstName CHARACTER VARYING(30) , 
	userId_lastName CHARACTER VARYING(30) , 
	parentEntryId_BlogEntry_Id BIGINT 
)
AS
	INSERT INTO BlogDemo.BlogEntry(BlogEntry_Id, entryTitle, entryBody, postedDate_MDYValue, userId_firstName, userId_lastName, parentEntryId_BlogEntry_Id)
	VALUES (BlogEntry_Id, entryTitle, entryBody, postedDate_MDYValue, userId_firstName, userId_lastName, parentEntryId_BlogEntry_Id);

CREATE PROCEDURE BlogDemo.DeleteBlogEntry
(
	BlogEntry_Id BIGINT 
)
AS
	DELETE FROM BlogDemo.BlogEntry
	WHERE BlogEntry_Id = BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryBlogEntry_Id
(
	old_BlogEntry_Id BIGINT , 
	BlogEntry_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntry
SET BlogEntry_Id = BlogEntry_Id
	WHERE BlogEntry_Id = old_BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryentryTitle
(
	old_BlogEntry_Id BIGINT , 
	entryTitle CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET entryTitle = entryTitle
	WHERE BlogEntry_Id = old_BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryentryBody
(
	old_BlogEntry_Id BIGINT , 
	entryBody CHARACTER VARYING() 
)
AS
	UPDATE BlogDemo.BlogEntry
SET entryBody = entryBody
	WHERE BlogEntry_Id = old_BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdtBlgEntrypstdDt_MDYVl
(
	old_BlogEntry_Id BIGINT , 
	postedDate_MDYValue BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntry
SET postedDate_MDYValue = postedDate_MDYValue
	WHERE BlogEntry_Id = old_BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdtBlgEntrysrId_frstNm
(
	old_BlogEntry_Id BIGINT , 
	userId_firstName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET userId_firstName = userId_firstName
	WHERE BlogEntry_Id = old_BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogEntryuserId_lastName
(
	old_BlogEntry_Id BIGINT , 
	userId_lastName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET userId_lastName = userId_lastName
	WHERE BlogEntry_Id = old_BlogEntry_Id;

CREATE PROCEDURE BlogDemo.UBEEIBEI
(
	old_BlogEntry_Id BIGINT , 
	parentEntryId_BlogEntry_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntry
SET parentEntryId_BlogEntry_Id = parentEntryId_BlogEntry_Id
	WHERE BlogEntry_Id = old_BlogEntry_Id;

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
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	firstName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET firstName = firstName
	WHERE firstName = old_firstName AND 
lastName = old_lastName;

CREATE PROCEDURE BlogDemo."Update""User""lastName"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	lastName CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET lastName = lastName
	WHERE firstName = old_firstName AND 
lastName = old_lastName;

CREATE PROCEDURE BlogDemo."Update""User""username"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	username CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET username = username
	WHERE firstName = old_firstName AND 
lastName = old_lastName;

CREATE PROCEDURE BlogDemo."Update""User""password"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	password CHARACTER(32) 
)
AS
	UPDATE BlogDemo."User"
SET password = password
	WHERE firstName = old_firstName AND 
lastName = old_lastName;

CREATE PROCEDURE BlogDemo.InsertBlogLabel
(
	BlogLabel_Id BIGINT , 
	title CHARACTER VARYING() 
)
AS
	INSERT INTO BlogDemo.BlogLabel(BlogLabel_Id, title)
	VALUES (BlogLabel_Id, title);

CREATE PROCEDURE BlogDemo.DeleteBlogLabel
(
	BlogLabel_Id BIGINT 
)
AS
	DELETE FROM BlogDemo.BlogLabel
	WHERE BlogLabel_Id = BlogLabel_Id;

CREATE PROCEDURE BlogDemo.UpdateBlogLabeltitle
(
	old_BlogLabel_Id BIGINT , 
	title CHARACTER VARYING() 
)
AS
	UPDATE BlogDemo.BlogLabel
SET title = title
	WHERE BlogLabel_Id = old_BlogLabel_Id;
COMMIT WORK;


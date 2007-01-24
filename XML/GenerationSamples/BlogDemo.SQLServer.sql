CREATE SCHEMA BlogDemo
GO

GO

CREATE TABLE BlogDemo.BlogEntryLabel
(
	blogEntryId_BlogEntry_Id BIGINT NOT NULL, 
	blogLabelId_BlogLabel_Id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint20 PRIMARY KEY(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
)
GO


CREATE TABLE BlogDemo.BlogEntry
(
	BlogEntry_Id BIGINT NOT NULL, 
	entryTitle NATIONAL CHARACTER VARYING(30) NOT NULL, 
	entryBody NATIONAL CHARACTER VARYING() NOT NULL, 
	postedDate_MDYValue BIGINT NOT NULL, 
	userId_firstName NATIONAL CHARACTER VARYING(30) NOT NULL, 
	userId_lastName NATIONAL CHARACTER VARYING(30) NOT NULL, 
	parentEntryId_BlogEntry_Id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(BlogEntry_Id)
)
GO


CREATE TABLE BlogDemo."User"
(
	firstName NATIONAL CHARACTER VARYING(30) NOT NULL, 
	lastName NATIONAL CHARACTER VARYING(30) NOT NULL, 
	username NATIONAL CHARACTER VARYING(30) NOT NULL, 
	password NATIONAL CHARACTER(32) CONSTRAINT Password_Chk CHECK ((LEN(LTRIM(RTRIM(password)))) >= 32) NOT NULL, 
	CONSTRAINT ExternalUniquenessConstraint1 PRIMARY KEY(firstName, lastName)
)
GO


CREATE TABLE BlogDemo.BlogLabel
(
	BlogLabel_Id BIGINT IDENTITY (1, 1) NOT NULL, 
	title NATIONAL CHARACTER VARYING() , 
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(BlogLabel_Id)
)
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_blogEntryId_FK FOREIGN KEY (blogEntryId_BlogEntry_Id)  REFERENCES BlogDemo.BlogEntry (BlogEntry_Id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT BlogEntryLabel_blogLabelId_FK FOREIGN KEY (blogLabelId_BlogLabel_Id)  REFERENCES BlogDemo.BlogLabel (BlogLabel_Id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_userId_FK FOREIGN KEY (userId_firstName, userId_lastName)  REFERENCES BlogDemo."User" (firstName, lastName)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT BlogEntry_parentEntryId_FK FOREIGN KEY (parentEntryId_BlogEntry_Id)  REFERENCES BlogDemo.BlogEntry (BlogEntry_Id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO



CREATE PROCEDURE BlogDemo.InsertBlogEntryLabel
(
	@blogEntryId_BlogEntry_Id BIGINT , 
	@blogLabelId_BlogLabel_Id BIGINT 
)
AS
	INSERT INTO BlogDemo.BlogEntryLabel(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
	VALUES (@blogEntryId_BlogEntry_Id, @blogLabelId_BlogLabel_Id)
GO


CREATE PROCEDURE BlogDemo.DeleteBlogEntryLabel
(
	@blogEntryId_BlogEntry_Id BIGINT , 
	@blogLabelId_BlogLabel_Id BIGINT 
)
AS
	DELETE FROM BlogDemo.BlogEntryLabel
	WHERE blogEntryId_BlogEntry_Id = @blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = @blogLabelId_BlogLabel_Id
GO


CREATE PROCEDURE BlogDemo.UBELEIBEI
(
	@old_blogEntryId_BlogEntry_Id BIGINT , 
	@old_blogLabelId_BlogLabel_Id BIGINT , 
	@blogEntryId_BlogEntry_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntryLabel
SET blogEntryId_BlogEntry_Id = @blogEntryId_BlogEntry_Id
	WHERE blogEntryId_BlogEntry_Id = @old_blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = @old_blogLabelId_BlogLabel_Id
GO


CREATE PROCEDURE BlogDemo.UBELLIBLI
(
	@old_blogEntryId_BlogEntry_Id BIGINT , 
	@old_blogLabelId_BlogLabel_Id BIGINT , 
	@blogLabelId_BlogLabel_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntryLabel
SET blogLabelId_BlogLabel_Id = @blogLabelId_BlogLabel_Id
	WHERE blogEntryId_BlogEntry_Id = @old_blogEntryId_BlogEntry_Id AND 
blogLabelId_BlogLabel_Id = @old_blogLabelId_BlogLabel_Id
GO


CREATE PROCEDURE BlogDemo.InsertBlogEntry
(
	@BlogEntry_Id BIGINT , 
	@entryTitle NATIONAL CHARACTER VARYING(30) , 
	@entryBody NATIONAL CHARACTER VARYING() , 
	@postedDate_MDYValue BIGINT , 
	@userId_firstName NATIONAL CHARACTER VARYING(30) , 
	@userId_lastName NATIONAL CHARACTER VARYING(30) , 
	@parentEntryId_BlogEntry_Id BIGINT 
)
AS
	INSERT INTO BlogDemo.BlogEntry(BlogEntry_Id, entryTitle, entryBody, postedDate_MDYValue, userId_firstName, userId_lastName, parentEntryId_BlogEntry_Id)
	VALUES (@BlogEntry_Id, @entryTitle, @entryBody, @postedDate_MDYValue, @userId_firstName, @userId_lastName, @parentEntryId_BlogEntry_Id)
GO


CREATE PROCEDURE BlogDemo.DeleteBlogEntry
(
	@BlogEntry_Id BIGINT 
)
AS
	DELETE FROM BlogDemo.BlogEntry
	WHERE BlogEntry_Id = @BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UpdateBlogEntryBlogEntry_Id
(
	@old_BlogEntry_Id BIGINT , 
	@BlogEntry_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntry
SET BlogEntry_Id = @BlogEntry_Id
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UpdateBlogEntryentryTitle
(
	@old_BlogEntry_Id BIGINT , 
	@entryTitle NATIONAL CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET entryTitle = @entryTitle
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UpdateBlogEntryentryBody
(
	@old_BlogEntry_Id BIGINT , 
	@entryBody NATIONAL CHARACTER VARYING() 
)
AS
	UPDATE BlogDemo.BlogEntry
SET entryBody = @entryBody
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UpdtBlgEntrypstdDt_MDYVl
(
	@old_BlogEntry_Id BIGINT , 
	@postedDate_MDYValue BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntry
SET postedDate_MDYValue = @postedDate_MDYValue
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UpdtBlgEntrysrId_frstNm
(
	@old_BlogEntry_Id BIGINT , 
	@userId_firstName NATIONAL CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET userId_firstName = @userId_firstName
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UpdateBlogEntryuserId_lastName
(
	@old_BlogEntry_Id BIGINT , 
	@userId_lastName NATIONAL CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo.BlogEntry
SET userId_lastName = @userId_lastName
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.UBEEIBEI
(
	@old_BlogEntry_Id BIGINT , 
	@parentEntryId_BlogEntry_Id BIGINT 
)
AS
	UPDATE BlogDemo.BlogEntry
SET parentEntryId_BlogEntry_Id = @parentEntryId_BlogEntry_Id
	WHERE BlogEntry_Id = @old_BlogEntry_Id
GO


CREATE PROCEDURE BlogDemo.InsertUser
(
	@firstName NATIONAL CHARACTER VARYING(30) , 
	@lastName NATIONAL CHARACTER VARYING(30) , 
	@username NATIONAL CHARACTER VARYING(30) , 
	@password NATIONAL CHARACTER(32) 
)
AS
	INSERT INTO BlogDemo."User"(firstName, lastName, username, password)
	VALUES (@firstName, @lastName, @username, @password)
GO


CREATE PROCEDURE BlogDemo.DeleteUser
(
	@firstName NATIONAL CHARACTER VARYING(30) , 
	@lastName NATIONAL CHARACTER VARYING(30) 
)
AS
	DELETE FROM BlogDemo."User"
	WHERE firstName = @firstName AND 
lastName = @lastName
GO


CREATE PROCEDURE BlogDemo."Update""User""firstName"
(
	@old_firstName NATIONAL CHARACTER VARYING(30) , 
	@old_lastName NATIONAL CHARACTER VARYING(30) , 
	@firstName NATIONAL CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET firstName = @firstName
	WHERE firstName = @old_firstName AND 
lastName = @old_lastName
GO


CREATE PROCEDURE BlogDemo."Update""User""lastName"
(
	@old_firstName NATIONAL CHARACTER VARYING(30) , 
	@old_lastName NATIONAL CHARACTER VARYING(30) , 
	@lastName NATIONAL CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET lastName = @lastName
	WHERE firstName = @old_firstName AND 
lastName = @old_lastName
GO


CREATE PROCEDURE BlogDemo."Update""User""username"
(
	@old_firstName NATIONAL CHARACTER VARYING(30) , 
	@old_lastName NATIONAL CHARACTER VARYING(30) , 
	@username NATIONAL CHARACTER VARYING(30) 
)
AS
	UPDATE BlogDemo."User"
SET username = @username
	WHERE firstName = @old_firstName AND 
lastName = @old_lastName
GO


CREATE PROCEDURE BlogDemo."Update""User""password"
(
	@old_firstName NATIONAL CHARACTER VARYING(30) , 
	@old_lastName NATIONAL CHARACTER VARYING(30) , 
	@password NATIONAL CHARACTER(32) 
)
AS
	UPDATE BlogDemo."User"
SET password = @password
	WHERE firstName = @old_firstName AND 
lastName = @old_lastName
GO


CREATE PROCEDURE BlogDemo.InsertBlogLabel
(
	@BlogLabel_Id BIGINT , 
	@title NATIONAL CHARACTER VARYING() 
)
AS
	INSERT INTO BlogDemo.BlogLabel(BlogLabel_Id, title)
	VALUES (@BlogLabel_Id, @title)
GO


CREATE PROCEDURE BlogDemo.DeleteBlogLabel
(
	@BlogLabel_Id BIGINT 
)
AS
	DELETE FROM BlogDemo.BlogLabel
	WHERE BlogLabel_Id = @BlogLabel_Id
GO


CREATE PROCEDURE BlogDemo.UpdateBlogLabeltitle
(
	@old_BlogLabel_Id BIGINT , 
	@title NATIONAL CHARACTER VARYING() 
)
AS
	UPDATE BlogDemo.BlogLabel
SET title = @title
	WHERE BlogLabel_Id = @old_BlogLabel_Id
GO


GO
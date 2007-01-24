START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA BlogDemo;

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
	BlogLabel_Id BIGSERIAL NOT NULL, 
	title CHARACTER VARYING() , 
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(BlogLabel_Id)
);

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT blogEntryId_FK FOREIGN KEY (blogEntryId_BlogEntry_Id)  REFERENCES BlogDemo.BlogEntry (BlogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntryLabel ADD CONSTRAINT blogLabelId_FK FOREIGN KEY (blogLabelId_BlogLabel_Id)  REFERENCES BlogDemo.BlogLabel (BlogLabel_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT userId_FK FOREIGN KEY (userId_firstName, userId_lastName)  REFERENCES BlogDemo."User" (firstName, lastName)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE BlogDemo.BlogEntry ADD CONSTRAINT parentEntryId_FK FOREIGN KEY (parentEntryId_BlogEntry_Id)  REFERENCES BlogDemo.BlogEntry (BlogEntry_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE FUNCTION BlogDemo.InsertBlogEntryLabel
(
	blogEntryId_BlogEntry_Id BIGINT , 
	blogLabelId_BlogLabel_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO BlogDemo.BlogEntryLabel(blogEntryId_BlogEntry_Id, blogLabelId_BlogLabel_Id)
	VALUES ($1, $2)';

CREATE FUNCTION BlogDemo.DeleteBlogEntryLabel
(
	blogEntryId_BlogEntry_Id BIGINT , 
	blogLabelId_BlogLabel_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM BlogDemo.BlogEntryLabel
	WHERE blogEntryId_BlogEntry_Id = $1 AND 
blogLabelId_BlogLabel_Id = $2';

CREATE FUNCTION BlogDemo.UBELEIBEI
(
	old_blogEntryId_BlogEntry_Id BIGINT , 
	old_blogLabelId_BlogLabel_Id BIGINT , 
	blogEntryId_BlogEntry_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntryLabel
SET blogEntryId_BlogEntry_Id = $3
	WHERE blogEntryId_BlogEntry_Id = $1 AND 
blogLabelId_BlogLabel_Id = $2';

CREATE FUNCTION BlogDemo.UBELLIBLI
(
	old_blogEntryId_BlogEntry_Id BIGINT , 
	old_blogLabelId_BlogLabel_Id BIGINT , 
	blogLabelId_BlogLabel_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntryLabel
SET blogLabelId_BlogLabel_Id = $3
	WHERE blogEntryId_BlogEntry_Id = $1 AND 
blogLabelId_BlogLabel_Id = $2';

CREATE FUNCTION BlogDemo.InsertBlogEntry
(
	BlogEntry_Id BIGINT , 
	entryTitle CHARACTER VARYING(30) , 
	entryBody CHARACTER VARYING() , 
	postedDate_MDYValue BIGINT , 
	userId_firstName CHARACTER VARYING(30) , 
	userId_lastName CHARACTER VARYING(30) , 
	parentEntryId_BlogEntry_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO BlogDemo.BlogEntry(BlogEntry_Id, entryTitle, entryBody, postedDate_MDYValue, userId_firstName, userId_lastName, parentEntryId_BlogEntry_Id)
	VALUES ($1, $2, $3, $4, $5, $6, $7)';

CREATE FUNCTION BlogDemo.DeleteBlogEntry
(
	BlogEntry_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM BlogDemo.BlogEntry
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UpdateBlogEntryBlogEntry_Id
(
	old_BlogEntry_Id BIGINT , 
	BlogEntry_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET BlogEntry_Id = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UpdateBlogEntryentryTitle
(
	old_BlogEntry_Id BIGINT , 
	entryTitle CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET entryTitle = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UpdateBlogEntryentryBody
(
	old_BlogEntry_Id BIGINT , 
	entryBody CHARACTER VARYING() 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET entryBody = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UpdtBlgEntrypstdDt_MDYVl
(
	old_BlogEntry_Id BIGINT , 
	postedDate_MDYValue BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET postedDate_MDYValue = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UpdtBlgEntrysrId_frstNm
(
	old_BlogEntry_Id BIGINT , 
	userId_firstName CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET userId_firstName = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UpdateBlogEntryuserId_lastName
(
	old_BlogEntry_Id BIGINT , 
	userId_lastName CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET userId_lastName = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.UBEEIBEI
(
	old_BlogEntry_Id BIGINT , 
	parentEntryId_BlogEntry_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogEntry
SET parentEntryId_BlogEntry_Id = $2
	WHERE BlogEntry_Id = $1';

CREATE FUNCTION BlogDemo.InsertUser
(
	firstName CHARACTER VARYING(30) , 
	lastName CHARACTER VARYING(30) , 
	username CHARACTER VARYING(30) , 
	password CHARACTER(32) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO BlogDemo."User"(firstName, lastName, username, password)
	VALUES ($1, $2, $3, $4)';

CREATE FUNCTION BlogDemo.DeleteUser
(
	firstName CHARACTER VARYING(30) , 
	lastName CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM BlogDemo."User"
	WHERE firstName = $1 AND 
lastName = $2';

CREATE FUNCTION BlogDemo."Update""User""firstName"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	firstName CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo."User"
SET firstName = $3
	WHERE firstName = $1 AND 
lastName = $2';

CREATE FUNCTION BlogDemo."Update""User""lastName"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	lastName CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo."User"
SET lastName = $3
	WHERE firstName = $1 AND 
lastName = $2';

CREATE FUNCTION BlogDemo."Update""User""username"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	username CHARACTER VARYING(30) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo."User"
SET username = $3
	WHERE firstName = $1 AND 
lastName = $2';

CREATE FUNCTION BlogDemo."Update""User""password"
(
	old_firstName CHARACTER VARYING(30) , 
	old_lastName CHARACTER VARYING(30) , 
	password CHARACTER(32) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo."User"
SET password = $3
	WHERE firstName = $1 AND 
lastName = $2';

CREATE FUNCTION BlogDemo.InsertBlogLabel
(
	BlogLabel_Id BIGINT , 
	title CHARACTER VARYING() 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO BlogDemo.BlogLabel(BlogLabel_Id, title)
	VALUES ($1, $2)';

CREATE FUNCTION BlogDemo.DeleteBlogLabel
(
	BlogLabel_Id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM BlogDemo.BlogLabel
	WHERE BlogLabel_Id = $1';

CREATE FUNCTION BlogDemo.UpdateBlogLabeltitle
(
	old_BlogLabel_Id BIGINT , 
	title CHARACTER VARYING() 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE BlogDemo.BlogLabel
SET title = $2
	WHERE BlogLabel_Id = $1';
COMMIT WORK;


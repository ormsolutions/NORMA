CREATE SCHEMA SampleModel
GO

GO


CREATE TABLE SampleModel.Person
(
	personId int IDENTITY (1, 1) NOT NULL,
	"date" date NOT NULL,
	firstName nvarchar(64) NOT NULL,
	genderCode nchar(1) CHECK (genderCode IN (N'M', N'F')) NOT NULL,
	lastName nvarchar(64) NOT NULL,
	mandatoryNonUniqueTinyInt tinyint NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal decimal(38,38) NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat float NOT NULL,
	mandatoryUniqueDecimal decimal(9,0) CHECK (mandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	mandatoryUniqueString nchar(11) NOT NULL,
	mandatoryUniqueTinyInt tinyint NOT NULL,
	childPersonBirthOrderNr int CHECK (childPersonBirthOrderNr >= 0 AND childPersonBirthOrderNr >= 1),
	childPersonFather int,
	childPersonMother int,
	optionalUniqueDecimal decimal(9,0),
	optionalUniqueString nchar(11),
	optionalUniqueTinyInt tinyint,
	ownsCar int CHECK (ownsCar >= 0),
	wife int,
	colorARGB int,
	deathCause nvarchar(14) CHECK (deathCause IN (N'natural', N'not so natural')),
	deathDate date,
	deathNaturalDeathIsFromProstateCancer bit,
	deathUnnaturalDeathIsBloody bit,
	deathUnnaturalDeathIsViolent bit,
	hasParents bit,
	hatTypeStyle nvarchar(256),
	isDead bit,
	optionalNonUniqueTinyInt tinyint,
	valueType1DoesSomethingElseWith int,
	CONSTRAINT Person_PK PRIMARY KEY(personId),
	CONSTRAINT Person_UC1 UNIQUE(firstName, "date"),
	CONSTRAINT Person_UC2 UNIQUE(lastName, "date"),
	CONSTRAINT Person_UC7 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT Person_UC8 UNIQUE(mandatoryUniqueString),
	CONSTRAINT Person_UC10 UNIQUE(mandatoryUniqueTinyInt),
	CONSTRAINT Person_mandatoryUniqueDecimal_RoleValueConstraint2 CHECK (mandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT Person_optionalUniqueDecimal_RoleValueConstraint1 CHECK (optionalUniqueDecimal BETWEEN 100 AND 4000),
	CONSTRAINT Person_Death_MandatoryGroup CHECK (deathCause IS NOT NULL OR deathDate IS NULL AND deathNaturalDeathIsFromProstateCancer IS NULL AND deathCause IS NULL AND deathUnnaturalDeathIsViolent IS NULL AND deathUnnaturalDeathIsBloody IS NULL),
	CONSTRAINT Person_ChildPerson_MandatoryGroup CHECK (childPersonMother IS NOT NULL AND childPersonBirthOrderNr IS NOT NULL AND childPersonFather IS NOT NULL OR childPersonMother IS NULL AND childPersonBirthOrderNr IS NULL AND childPersonFather IS NULL)
)
GO


CREATE VIEW SampleModel.Person_UC3 (optionalUniqueString)
WITH SCHEMABINDING
AS
	SELECT optionalUniqueString
	FROM 
		SampleModel.Person
	WHERE optionalUniqueString IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC3Index ON SampleModel.Person_UC3(optionalUniqueString)
GO


CREATE VIEW SampleModel.Person_UC4 (wife)
WITH SCHEMABINDING
AS
	SELECT wife
	FROM 
		SampleModel.Person
	WHERE wife IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC4Index ON SampleModel.Person_UC4(wife)
GO


CREATE VIEW SampleModel.Person_UC5 (ownsCar)
WITH SCHEMABINDING
AS
	SELECT ownsCar
	FROM 
		SampleModel.Person
	WHERE ownsCar IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC5Index ON SampleModel.Person_UC5(ownsCar)
GO


CREATE VIEW SampleModel.Person_UC6 (optionalUniqueDecimal)
WITH SCHEMABINDING
AS
	SELECT optionalUniqueDecimal
	FROM 
		SampleModel.Person
	WHERE optionalUniqueDecimal IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC6Index ON SampleModel.Person_UC6(optionalUniqueDecimal)
GO


CREATE VIEW SampleModel.Person_UC9 (optionalUniqueTinyInt)
WITH SCHEMABINDING
AS
	SELECT optionalUniqueTinyInt
	FROM 
		SampleModel.Person
	WHERE optionalUniqueTinyInt IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC9Index ON SampleModel.Person_UC9(optionalUniqueTinyInt)
GO


CREATE VIEW SampleModel.Person_UC11 (childPersonFather, childPersonBirthOrderNr, childPersonMother)
WITH SCHEMABINDING
AS
	SELECT childPersonFather, childPersonBirthOrderNr, childPersonMother
	FROM 
		SampleModel.Person
	WHERE childPersonFather IS NOT NULL AND childPersonBirthOrderNr IS NOT NULL AND childPersonMother IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC11Index ON SampleModel.Person_UC11(childPersonFather, childPersonBirthOrderNr, childPersonMother)
GO


CREATE TABLE SampleModel.Task
(
	taskId int IDENTITY (1, 1) NOT NULL,
	personId int NOT NULL,
	CONSTRAINT Task_PK PRIMARY KEY(taskId)
)
GO


CREATE TABLE SampleModel.ValueType1
(
	"value" int NOT NULL,
	doesSomethingWithPerson int,
	CONSTRAINT ValueType1_PK PRIMARY KEY("value")
)
GO


CREATE TABLE SampleModel.PersonDrivesCar
(
	drivenByPerson int NOT NULL,
	drivesCar int CHECK (drivesCar >= 0) NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
)
GO


CREATE TABLE SampleModel.PersonBoughtCarFromPersonOnDate
(
	buyer int NOT NULL,
	carSold int CHECK (carSold >= 0) NOT NULL,
	seller int NOT NULL,
	saleDate date NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonOnDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonOnDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonOnDate_UC2 UNIQUE(saleDate, seller, carSold)
)
GO


CREATE TABLE SampleModel.Review
(
	car int CHECK (car >= 0) NOT NULL,
	criterion nvarchar(64) NOT NULL,
	nr int CHECK (nr >= 0 AND nr IN (9, 10, 12) OR nr BETWEEN 1 AND 7 OR nr BETWEEN 14 AND 16 OR nr >= 18) NOT NULL,
	CONSTRAINT Review_PK PRIMARY KEY(car, criterion)
)
GO


CREATE TABLE SampleModel.PersonHasNickName
(
	nickName nvarchar(64) NOT NULL,
	personId int NOT NULL,
	CONSTRAINT PersonHasNickName_PK PRIMARY KEY(nickName, personId)
)
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 ("value") ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
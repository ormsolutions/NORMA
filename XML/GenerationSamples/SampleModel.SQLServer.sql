CREATE SCHEMA SampleModel
GO

GO


CREATE TABLE SampleModel.Person
(
	personId INTEGER IDENTITY (1, 1) NOT NULL,
	firstName NATIONAL CHARACTER VARYING(64) NOT NULL,
	lastName NATIONAL CHARACTER VARYING(64) NOT NULL,
	"date" DATETIME NOT NULL,
	mandatoryUniqueDecimal DECIMAL(9,0) CHECK (mandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	mandatoryUniqueString NATIONAL CHARACTER(11) NOT NULL,
	mandatoryUniqueTinyInt TINYINT NOT NULL,
	genderCode NATIONAL CHARACTER(1) CHECK (genderCode IN (N'M', N'F')) NOT NULL,
	mandatoryNonUniqueTinyInt TINYINT NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal DECIMAL(38,38) NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT NOT NULL,
	optionalUniqueString NATIONAL CHARACTER(11),
	ownsCar INTEGER CHECK (ownsCar >= 0),
	optionalUniqueDecimal DECIMAL(9,0),
	optionalUniqueTinyInt TINYINT,
	wife INTEGER,
	childPersonBirthOrderNr INTEGER CHECK (childPersonBirthOrderNr >= 0 AND childPersonBirthOrderNr >= 1),
	childPersonFatherPerson_id INTEGER,
	childPersonMotherPerson_id INTEGER,
	ColorARGB INTEGER,
	hatTypeStyle NATIONAL CHARACTER VARYING(256),
	isDead BIT,
	hasParents BIT,
	optionalNonUniqueTinyInt TINYINT,
	valueType1DoesSomethingElseWith INTEGER,
	deathDate DATETIME,
	deathCause NATIONAL CHARACTER VARYING(14) CHECK (deathCause IN (N'natural', N'not so natural')),
	deathNaturalDeathIsFromProstateCancer BIT,
	deathUnnaturalDeathIsViolent BIT,
	deathUnnaturalDeathIsBloody BIT,
	CONSTRAINT Person_PK PRIMARY KEY(personId),
	CONSTRAINT Person_UC1 UNIQUE(firstName, "date"),
	CONSTRAINT Person_UC2 UNIQUE(lastName, "date"),
	CONSTRAINT Person_UC7 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT Person_UC8 UNIQUE(mandatoryUniqueString),
	CONSTRAINT Person_UC10 UNIQUE(mandatoryUniqueTinyInt),
	CONSTRAINT RoleValueConstraint2 CHECK (mandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT RoleValueConstraint1 CHECK (optionalUniqueDecimal BETWEEN 100 AND 4000)
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


CREATE VIEW SampleModel.Person_UC11 (childPersonFatherPerson_id, childPersonBirthOrderNr, childPersonMotherPerson_id)
WITH SCHEMABINDING
AS
	SELECT childPersonFatherPerson_id, childPersonBirthOrderNr, childPersonMotherPerson_id
	FROM 
		SampleModel.Person
	WHERE childPersonFatherPerson_id IS NOT NULL AND childPersonBirthOrderNr IS NOT NULL AND childPersonMotherPerson_id IS NOT NULL
GO


CREATE UNIQUE CLUSTERED INDEX Person_UC11Index ON SampleModel.Person_UC11(childPersonFatherPerson_id, childPersonBirthOrderNr, childPersonMotherPerson_id)
GO


CREATE TABLE SampleModel.Task
(
	taskId INTEGER IDENTITY (1, 1) NOT NULL,
	personId INTEGER NOT NULL,
	CONSTRAINT Task_PK PRIMARY KEY(taskId)
)
GO


CREATE TABLE SampleModel.ValueType1
(
	"value" INTEGER NOT NULL,
	doesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1_PK PRIMARY KEY("value")
)
GO


CREATE TABLE SampleModel.PersonDrivesCar
(
	drivesCar INTEGER CHECK (drivesCar >= 0) NOT NULL,
	drivenByPerson INTEGER NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
)
GO


CREATE TABLE SampleModel.PersonBoughtCarFromPersonDate
(
	carSold INTEGER CHECK (carSold >= 0) NOT NULL,
	buyer INTEGER NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATETIME NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC2 UNIQUE(saleDate, seller, carSold)
)
GO


CREATE TABLE SampleModel.Review
(
	car INTEGER CHECK (car >= 0) NOT NULL,
	criterion NATIONAL CHARACTER VARYING(64) NOT NULL,
	nr INTEGER CHECK (nr >= 0 AND nr IN (9, 10, 12) OR nr BETWEEN 1 AND 7 OR nr BETWEEN 14 AND 16 OR nr >= 18) NOT NULL,
	CONSTRAINT Review_PK PRIMARY KEY(car, criterion)
)
GO


CREATE TABLE SampleModel.PersonHasNickName
(
	nickName NATIONAL CHARACTER VARYING(64) NOT NULL,
	personId INTEGER NOT NULL,
	CONSTRAINT PersonHasNickName_PK PRIMARY KEY(nickName, personId)
)
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 ("value") ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFatherPerson_id) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMotherPerson_id) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
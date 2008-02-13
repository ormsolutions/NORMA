CREATE SCHEMA SampleModel
GO

GO


CREATE TABLE SampleModel.Person
(
	person_Id INTEGER IDENTITY (1, 1) NOT NULL,
	firstName NATIONAL CHARACTER VARYING(64) NOT NULL,
	"date" DATETIME NOT NULL,
	lastName NATIONAL CHARACTER VARYING(64) NOT NULL,
	mandatoryUniqueDecimal DECIMAL(9) CHECK (mandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	mandatoryUniqueString NATIONAL CHARACTER(11) NOT NULL,
	gender_Code NATIONAL CHARACTER(1) CHECK (gender_Code IN ('M', 'F')) NOT NULL,
	optionalUniqueString NATIONAL CHARACTER(11),
	ownsCar INTEGER CHECK (ownsCar >= 0),
	optionalUniqueDecimal DECIMAL(9),
	wife INTEGER,
	childPerson INTEGER CHECK (childPerson >= 0 AND childPerson >= 1),
	childPersonFather INTEGER,
	childPersonMother INTEGER,
	wearsHatTypePerson1 INTEGER,
	wearsHatTypePerson2 NATIONAL CHARACTER VARYING(256),
	hasParents BIT,
	valueType1DoesSomethingElseWith INTEGER,
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(person_Id),
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(firstName, "date"),
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE(lastName, "date"),
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(optionalUniqueString),
	CONSTRAINT InternalUniquenessConstraint13 UNIQUE(wife),
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(ownsCar),
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(optionalUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(mandatoryUniqueString),
	CONSTRAINT InternalUniquenessConstraint49 UNIQUE(childPersonFather, childPerson, childPersonMother),
	CONSTRAINT RoleValueConstraint2 CHECK (mandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT RoleValueConstraint1 CHECK (optionalUniqueDecimal BETWEEN 100 AND 4000)
)
GO


CREATE TABLE SampleModel.Task
(
	task_Id INTEGER IDENTITY (1, 1) NOT NULL,
	person_Id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(task_Id)
)
GO


CREATE TABLE SampleModel.ValueType1
(
	valueType1Value INTEGER NOT NULL,
	doesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(valueType1Value)
)
GO


CREATE TABLE SampleModel.Death
(
	isDeadPerson_Id INTEGER NOT NULL,
	deathCause NATIONAL CHARACTER VARYING(14) CHECK (deathCause IN ('natural', 'not so natural')) NOT NULL,
	isDeadPersonisDead BIT NOT NULL,
	"date" DATETIME,
	naturalDeathIsFromProstateCancer BIT,
	unnaturalDeathIsViolent BIT,
	unnaturalDeathIsBloody BIT,
	CONSTRAINT "Constraint" PRIMARY KEY(isDeadPerson_Id)
)
GO


CREATE TABLE SampleModel.PersonDrivesCar
(
	drivesCar INTEGER CHECK (drivesCar >= 0) NOT NULL,
	drivenByPerson INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
)
GO


CREATE TABLE SampleModel.PersonBoughtCarFromPersonOnDate
(
	carSold INTEGER CHECK (carSold >= 0) NOT NULL,
	buyer INTEGER NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATETIME NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
)
GO


CREATE TABLE SampleModel.Review
(
	vin INTEGER CHECK (vin >= 0) NOT NULL,
	name NATIONAL CHARACTER VARYING(64) NOT NULL,
	"integer" INTEGER CHECK ("integer" >= 0 AND "integer" IN (9, 10, 12) OR "integer" BETWEEN 1 AND 7 OR "integer" BETWEEN 14 AND 16 OR "integer" >= 18) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(vin, name)
)
GO


CREATE TABLE SampleModel.PersonHasNickName
(
	nickName NATIONAL CHARACTER VARYING(64) NOT NULL,
	person_Id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, person_Id)
)
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 (valueType1Value) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (wife) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (person_Id) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Death ADD CONSTRAINT Death_FK FOREIGN KEY (isDeadPerson_Id) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (person_Id) REFERENCES SampleModel.Person (person_Id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
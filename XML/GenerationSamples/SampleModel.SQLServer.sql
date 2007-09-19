CREATE SCHEMA SampleModel
GO

GO

CREATE TABLE SampleModel.Person
(
	Person_id INTEGER IDENTITY (1, 1) NOT NULL,
	FirstName NATIONAL CHARACTER VARYING(64) NOT NULL,
	"Date" DATETIME NOT NULL,
	LastName NATIONAL CHARACTER VARYING(64) NOT NULL,
	MandatoryUniqueDecimal DECIMAL(9) CHECK (MandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	MandatoryUniqueString NATIONAL CHARACTER(11) NOT NULL,
	Gender_Code NATIONAL CHARACTER(1) CHECK (Gender_Code IN ('M', 'F')) NOT NULL,
	OptionalUniqueString NATIONAL CHARACTER(11),
	OwnsCar INTEGER CHECK (OwnsCar >= 0),
	OptionalUniqueDecimal DECIMAL(9),
	Wife INTEGER,
	ChildPerson INTEGER CHECK (ChildPerson >= 0 AND ChildPerson >= 1),
	ChildPersonFather INTEGER,
	ChildPersonMother INTEGER,
	WearsHatTypeColorARGB INTEGER,
	WearsHatTypeStyle_Description NATIONAL CHARACTER VARYING(256),
	HasParents BIT,
	ValueType1DoesSomethingElseWith INTEGER,
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(Person_id),
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(FirstName, "Date"),
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE("Date", LastName),
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(OptionalUniqueString),
	CONSTRAINT InternalUniquenessConstraint13 UNIQUE(Wife),
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(OwnsCar),
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(OptionalUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(MandatoryUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(MandatoryUniqueString),
	CONSTRAINT InternalUniquenessConstraint49 UNIQUE(ChildPersonFather, ChildPerson, ChildPersonMother),
	CONSTRAINT RoleValueConstraint2 CHECK (MandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT RoleValueConstraint1 CHECK (OptionalUniqueDecimal BETWEEN 100 AND 4000)
)
GO


CREATE TABLE SampleModel.Task
(
	Task_id INTEGER IDENTITY (1, 1) NOT NULL,
	Person_id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
)
GO


CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value INTEGER NOT NULL,
	DoesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(ValueType1Value)
)
GO


CREATE TABLE SampleModel.Death
(
	Person_id INTEGER IDENTITY (1, 1) NOT NULL,
	DeathCause NATIONAL CHARACTER VARYING(14) CHECK (DeathCause IN ('natural', 'not so natural')) NOT NULL,
	IsDead BIT NOT NULL,
	"Date" DATETIME,
	NaturalDeathIsFromProstateCancer BIT,
	UnnaturalDeathIsViolent BIT,
	UnnaturalDeathIsBloody BIT,
	CONSTRAINT "Constraint" PRIMARY KEY(Person_id)
)
GO


CREATE TABLE SampleModel.PersonDrivesCar
(
	DrivesCar INTEGER CHECK (DrivesCar >= 0) NOT NULL,
	DrivenByPerson INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(DrivesCar, DrivenByPerson)
)
GO


CREATE TABLE SampleModel.PersonBoughtCarFromPersonOnDate
(
	CarSold INTEGER CHECK (CarSold >= 0) NOT NULL,
	Buyer INTEGER NOT NULL,
	Seller INTEGER NOT NULL,
	SaleDate DATETIME NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(CarSold, Buyer, Seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold, SaleDate, Buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(CarSold, SaleDate, Seller)
)
GO


CREATE TABLE SampleModel.Review
(
	Vin INTEGER CHECK (Vin >= 0) NOT NULL,
	Name NATIONAL CHARACTER VARYING(64) NOT NULL,
	"Integer" INTEGER CHECK ("Integer" >= 0 AND "Integer" IN (9, 10, 12) OR "Integer" BETWEEN 1 AND 7 OR "Integer" BETWEEN 14 AND 16 OR "Integer" >= 18) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Vin, Name)
)
GO


CREATE TABLE SampleModel.PersonHasNickName
(
	NickName NATIONAL CHARACTER VARYING(64) NOT NULL,
	Person_id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_id)
)
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Person_FK1 FOREIGN KEY (ValueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 (ValueType1Value) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Person_FK2 FOREIGN KEY (Wife) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Person_FK3 FOREIGN KEY (ChildPersonFather) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Person_FK4 FOREIGN KEY (ChildPersonMother) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_Task_FK FOREIGN KEY (Person_id) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_ValueType1_FK FOREIGN KEY (DoesSomethingWithPerson) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Death ADD CONSTRAINT Death_Death_FK FOREIGN KEY (Person_id) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_PersonDrivesCar_FK FOREIGN KEY (DrivenByPerson) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (Buyer) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (Seller) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_PersonHasNickName_FK FOREIGN KEY (Person_id) REFERENCES SampleModel.Person (Person_id) ON DELETE NO ACTION ON UPDATE NO ACTION
GO



GO
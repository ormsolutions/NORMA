SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	Person_id NUMBER NOT NULL,
	FirstName NVARCHAR2(64) NOT NULL,
	"Date" DATE NOT NULL,
	LastName NVARCHAR2(64) NOT NULL,
	MandatoryUniqueDecimal NUMBER(9) NOT NULL,
	MandatoryUniqueString NCHAR(11) NOT NULL,
	Gender_Code NCHAR(1) CHECK (Gender_Code IN ('M', 'F')) NOT NULL,
	OptionalUniqueString NCHAR(11),
	OwnsCar NUMBER CHECK (OwnsCar >= 0),
	OptionalUniqueDecimal NUMBER(9),
	Wife NUMBER,
	ChildPerson NUMBER CHECK (ChildPerson >= 0),
	ChildPersonFather NUMBER,
	ChildPersonMother NUMBER,
	WearsHatTypeColorARGB NUMBER,
	WearsHatTypeStyle_Description NVARCHAR2(256),
	HasParents NCHAR(1),
	ValueType1DoesSomethingElseWith NUMBER,
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(Person_id),
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(FirstName, "Date"),
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE("Date", LastName),
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(OptionalUniqueString),
	CONSTRAINT InternalUniquenessConstraint13 UNIQUE(Wife),
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(OwnsCar),
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(OptionalUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(MandatoryUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(MandatoryUniqueString),
	CONSTRAINT InternalUniquenessConstraint49 UNIQUE(ChildPersonFather, ChildPerson, ChildPersonMother)
);

CREATE TABLE Task
(
	Task_id NUMBER NOT NULL,
	Person_id NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
);

CREATE TABLE ValueType1
(
	ValueType1Value NUMBER NOT NULL,
	DoesSomethingWithPerson NUMBER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(ValueType1Value)
);

CREATE TABLE Death
(
	Person_id NUMBER NOT NULL,
	DeathCause NVARCHAR2(14) CHECK (DeathCause IN ('natural', 'not so natural')) NOT NULL,
	IsDead NCHAR(1) NOT NULL,
	"Date" DATE,
	NaturalDeathIsFromProstateCancer NCHAR(1),
	UnnaturalDeathIsViolent NCHAR(1),
	UnnaturalDeathIsBloody NCHAR(1),
	CONSTRAINT "Constraint" PRIMARY KEY(Person_id)
);

CREATE TABLE PersonDrivesCar
(
	DrivesCar NUMBER CHECK (DrivesCar >= 0) NOT NULL,
	DrivenByPerson NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(DrivesCar, DrivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonOnDate
(
	CarSold NUMBER CHECK (CarSold >= 0) NOT NULL,
	Buyer NUMBER NOT NULL,
	Seller NUMBER NOT NULL,
	SaleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(CarSold, Buyer, Seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold, SaleDate, Buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(CarSold, SaleDate, Seller)
);

CREATE TABLE Review
(
	Vin NUMBER CHECK (Vin >= 0) NOT NULL,
	Name NVARCHAR2(64) NOT NULL,
	"Integer" NUMBER CHECK ("Integer" >= 0 AND "Integer" IN (9, 10, 12) OR "Integer" BETWEEN 1 AND 7 OR "Integer" BETWEEN 14 AND 16 OR "Integer" >= 18) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Vin, Name)
);

CREATE TABLE PersonHasNickName
(
	NickName NVARCHAR2(64) NOT NULL,
	Person_id NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_id)
);

ALTER TABLE Person ADD CONSTRAINT Person_Person_FK1 FOREIGN KEY (ValueType1DoesSomethingElseWith)  REFERENCES ValueType1 (ValueType1Value) ;

ALTER TABLE Person ADD CONSTRAINT Person_Person_FK2 FOREIGN KEY (Wife)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Person_FK3 FOREIGN KEY (ChildPersonFather)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Person_FK4 FOREIGN KEY (ChildPersonMother)  REFERENCES Person (Person_id) ;

ALTER TABLE Task ADD CONSTRAINT Task_Task_FK FOREIGN KEY (Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_ValueType1_FK FOREIGN KEY (DoesSomethingWithPerson)  REFERENCES Person (Person_id) ;

ALTER TABLE Death ADD CONSTRAINT Death_Death_FK FOREIGN KEY (Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_PersonDrivesCar_FK FOREIGN KEY (DrivenByPerson)  REFERENCES Person (Person_id) ;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (Buyer)  REFERENCES Person (Person_id) ;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (Seller)  REFERENCES Person (Person_id) ;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_PersonHasNickName_FK FOREIGN KEY (Person_id)  REFERENCES Person (Person_id) ;

COMMIT WORK;


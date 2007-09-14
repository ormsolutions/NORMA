START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel;

CREATE DOMAIN SampleModel.vin AS INTEGER CONSTRAINT vin_Unsigned_Chk CHECK (VALUE >= 0);

CREATE DOMAIN SampleModel."Integer" AS INTEGER CONSTRAINT ValueTypeValueConstraint1 CHECK (VALUE >= 0 AND VALUE IN (9, 10, 12) OR VALUE BETWEEN 1 AND 7 OR VALUE BETWEEN 14 AND 16 OR VALUE >= 18);

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT ValueTypeValueConstraint2 CHECK (VALUE IN ('natural', 'not so natural'));

CREATE DOMAIN SampleModel.BirthOrder_Nr AS INTEGER CONSTRAINT BirthOrder_Nr_Unsigned_Chk CHECK (VALUE >= 0);

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT ValueTypeValueConstraint3 CHECK (VALUE IN ('M', 'F'));

CREATE TABLE SampleModel.Person
(
	Person_id SERIAL NOT NULL, 
	FirstName CHARACTER VARYING(64) NOT NULL,
	"Date" DATE NOT NULL,
	LastName CHARACTER VARYING(64) NOT NULL,
	MandatoryUniqueDecimal DECIMAL(9) NOT NULL,
	MandatoryUniqueString CHARACTER(11) NOT NULL,
	Gender_Code SampleModel.Gender_Code NOT NULL,
	OptionalUniqueString CHARACTER(11),
	OwnsCar SampleModel.vin,
	OptionalUniqueDecimal DECIMAL(9),
	Wife INTEGER,
	ChildPerson SampleModel.BirthOrder_Nr,
	ChildPersonFather INTEGER,
	ChildPersonMother INTEGER,
	WearsHatTypeColorARGB INTEGER,
	WearsHatTypeStyle_Description CHARACTER VARYING(256),
	HasParents BOOLEAN,
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
	CONSTRAINT InternalUniquenessConstraint49 UNIQUE(ChildPersonFather, ChildPerson, ChildPersonMother)
);

CREATE TABLE SampleModel.Task
(
	Task_id SERIAL NOT NULL, 
	Person_id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
);

CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value INTEGER NOT NULL,
	DoesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(ValueType1Value)
);

CREATE TABLE SampleModel.Death
(
	Person_id SERIAL NOT NULL, 
	DeathCause SampleModel.DeathCause_Type NOT NULL,
	IsDead BOOLEAN NOT NULL,
	"Date" DATE,
	NaturalDeathIsFromProstateCancer BOOLEAN,
	UnnaturalDeathIsViolent BOOLEAN,
	UnnaturalDeathIsBloody BOOLEAN,
	CONSTRAINT "Constraint" PRIMARY KEY(Person_id)
);

CREATE TABLE SampleModel.PersonDrivesCar
(
	DrivesCar SampleModel.vin NOT NULL,
	DrivenByPerson INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(DrivesCar, DrivenByPerson)
);

CREATE TABLE SampleModel.PersonBoughtCarFromPersonOnDate
(
	CarSold SampleModel.vin NOT NULL,
	Buyer INTEGER NOT NULL,
	Seller INTEGER NOT NULL,
	SaleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(CarSold, Buyer, Seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold, SaleDate, Buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(CarSold, SaleDate, Seller)
);

CREATE TABLE SampleModel.Review
(
	Vin SampleModel.vin NOT NULL,
	Name CHARACTER VARYING(64) NOT NULL,
	"Integer" SampleModel."Integer" NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Vin, Name)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	NickName CHARACTER VARYING(64) NOT NULL,
	Person_id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_id)
);

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (ValueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 (ValueType1Value) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (Wife) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (ChildPersonFather) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (ChildPersonMother) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (Person_id) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (DoesSomethingWithPerson) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Death ADD CONSTRAINT Death_FK FOREIGN KEY (Person_id) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (DrivenByPerson) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (Buyer) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (Seller) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (Person_id) REFERENCES SampleModel.Person (Person_id) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;


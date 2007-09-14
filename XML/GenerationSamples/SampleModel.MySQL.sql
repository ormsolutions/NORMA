CREATE TABLE Person
(
	Person_id INTEGERAUTO_INCREMENT  NOT NULL, 
	FirstName VARCHAR(64)  NOT NULL,
	`Date` DATE NOT NULL,
	LastName VARCHAR(64)  NOT NULL,
	MandatoryUniqueDecimal DECIMAL(9) NOT NULL,
	MandatoryUniqueString CHAR(11)  NOT NULL,
	Gender_Code CHAR(1)  NOT NULL,
	OptionalUniqueString CHAR(11) ,
	OwnsCar ,
	OptionalUniqueDecimal DECIMAL(9),
	Wife INTEGER,
	ChildPerson ,
	ChildPersonFather INTEGER,
	ChildPersonMother INTEGER,
	WearsHatTypeColorARGB INTEGER,
	WearsHatTypeStyle_Description VARCHAR(256) ,
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

CREATE TABLE Task
(
	Task_id INTEGERAUTO_INCREMENT  NOT NULL, 
	Person_id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
);

CREATE TABLE ValueType1
(
	ValueType1Value INTEGER NOT NULL,
	DoesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(ValueType1Value)
);

CREATE TABLE Death
(
	Person_id INTEGERAUTO_INCREMENT  NOT NULL, 
	DeathCause VARCHAR(14)  NOT NULL,
	IsDead BOOLEAN NOT NULL,
	`Date` DATE,
	NaturalDeathIsFromProstateCancer BOOLEAN,
	UnnaturalDeathIsViolent BOOLEAN,
	UnnaturalDeathIsBloody BOOLEAN,
	CONSTRAINT `Constraint` PRIMARY KEY(Person_id)
);

CREATE TABLE PersonDrivesCar
(
	DrivesCar  NOT NULL,
	DrivenByPerson INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(DrivesCar, DrivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonOnDate
(
	CarSold  NOT NULL,
	Buyer INTEGER NOT NULL,
	Seller INTEGER NOT NULL,
	SaleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(CarSold, Buyer, Seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold, SaleDate, Buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(CarSold, SaleDate, Seller)
);

CREATE TABLE Review
(
	Vin  NOT NULL,
	Name VARCHAR(64)  NOT NULL,
	`Integer`  NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Vin, Name)
);

CREATE TABLE PersonHasNickName
(
	NickName VARCHAR(64)  NOT NULL,
	Person_id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_id)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (ValueType1DoesSomethingElseWith)  REFERENCES ValueType1 (ValueType1Value)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (Wife)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (ChildPersonFather)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (ChildPersonMother)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Task ADD CONSTRAINT Task_FK FOREIGN KEY (Person_id)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (DoesSomethingWithPerson)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Death ADD CONSTRAINT Death_FK FOREIGN KEY (Person_id)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (DrivenByPerson)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (Buyer)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (Seller)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (Person_id)  REFERENCES Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


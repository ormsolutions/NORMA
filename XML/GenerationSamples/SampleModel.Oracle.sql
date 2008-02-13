
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	person_Id NUMBER NOT NULL,
	firstName NVARCHAR2(64) NOT NULL,
	"date" DATE NOT NULL,
	lastName NVARCHAR2(64) NOT NULL,
	mandatoryUniqueDecimal NUMBER(9) CHECK (mandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	mandatoryUniqueString NCHAR(11) NOT NULL,
	gender_Code NCHAR(1) CHECK (gender_Code IN ('M', 'F')) NOT NULL,
	optionalUniqueString NCHAR(11),
	ownsCar NUMBER CHECK (ownsCar >= 0),
	optionalUniqueDecimal NUMBER(9),
	wife NUMBER,
	childPerson NUMBER CHECK (childPerson >= 0 AND childPerson >= 1),
	childPersonFather NUMBER,
	childPersonMother NUMBER,
	wearsHatTypePerson1 NUMBER,
	wearsHatTypePerson2 NVARCHAR2(256),
	hasParents NCHAR(1),
	valueType1DoesSomethingElseWith NUMBER,
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
);

CREATE TABLE Task
(
	task_Id NUMBER NOT NULL,
	person_Id NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(task_Id)
);

CREATE TABLE ValueType1
(
	valueType1Value NUMBER NOT NULL,
	doesSomethingWithPerson NUMBER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(valueType1Value)
);

CREATE TABLE Death
(
	isDeadPerson_Id NUMBER NOT NULL,
	deathCause NVARCHAR2(14) CHECK (deathCause IN ('natural', 'not so natural')) NOT NULL,
	isDeadPersonisDead NCHAR(1) NOT NULL,
	"date" DATE,
	naturalDeathIsFromProstateCancer NCHAR(1),
	unnaturalDeathIsViolent NCHAR(1),
	unnaturalDeathIsBloody NCHAR(1),
	CONSTRAINT "Constraint" PRIMARY KEY(isDeadPerson_Id)
);

CREATE TABLE PersonDrivesCar
(
	drivesCar NUMBER CHECK (drivesCar >= 0) NOT NULL,
	drivenByPerson NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonOnDate
(
	carSold NUMBER CHECK (carSold >= 0) NOT NULL,
	buyer NUMBER NOT NULL,
	seller NUMBER NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE Review
(
	vin NUMBER CHECK (vin >= 0) NOT NULL,
	name NVARCHAR2(64) NOT NULL,
	"integer" NUMBER CHECK ("integer" >= 0 AND "integer" IN (9, 10, 12) OR "integer" BETWEEN 1 AND 7 OR "integer" BETWEEN 14 AND 16 OR "integer" >= 18) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(vin, name)
);

CREATE TABLE PersonHasNickName
(
	nickName NVARCHAR2(64) NOT NULL,
	person_Id NUMBER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, person_Id)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (valueType1DoesSomethingElseWith)  REFERENCES ValueType1 (valueType1Value) ;

ALTER TABLE Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (wife)  REFERENCES Person (person_Id) ;

ALTER TABLE Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather)  REFERENCES Person (person_Id) ;

ALTER TABLE Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother)  REFERENCES Person (person_Id) ;

ALTER TABLE Task ADD CONSTRAINT Task_FK FOREIGN KEY (person_Id)  REFERENCES Person (person_Id) ;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson)  REFERENCES Person (person_Id) ;

ALTER TABLE Death ADD CONSTRAINT Death_FK FOREIGN KEY (isDeadPerson_Id)  REFERENCES Person (person_Id) ;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson)  REFERENCES Person (person_Id) ;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer)  REFERENCES Person (person_Id) ;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller)  REFERENCES Person (person_Id) ;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (person_Id)  REFERENCES Person (person_Id) ;

COMMIT WORK;

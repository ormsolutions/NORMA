
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	personId NUMBER(10,0) NOT NULL,
	firstName NVARCHAR2(64) NOT NULL,
	lastName NVARCHAR2(64) NOT NULL,
	"date" DATE NOT NULL,
	mandatoryUniqueDecimal NUMBER(9,0) CHECK (mandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	mandatoryUniqueString NCHAR(11) NOT NULL,
	mandatoryUniqueTinyInt NUMBER(3,0) CHECK (mandatoryUniqueTinyInt BETWEEN 0 AND 255) NOT NULL,
	genderCode NCHAR(1) CHECK (genderCode IN (N'M', N'F')) NOT NULL,
	mandatoryNonUniqueTinyInt NUMBER(3,0) CHECK (mandatoryNonUniqueTinyInt BETWEEN 0 AND 255) NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal NUMBER NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT(126) NOT NULL,
	optionalUniqueString NCHAR(11),
	ownsCar NUMBER(10,0) CHECK (ownsCar >= 0),
	optionalUniqueDecimal NUMBER(9,0),
	optionalUniqueTinyInt NUMBER(3,0) CHECK (optionalUniqueTinyInt BETWEEN 0 AND 255),
	wife NUMBER(10,0),
	childPersonBirthOrderNr NUMBER(10,0) CHECK (childPersonBirthOrderNr >= 0 AND childPersonBirthOrderNr >= 1),
	childPersonFatherMalePersonId NUMBER(10,0),
	childPersonMotherFemalePersonId NUMBER(10,0),
	ColorARGB NUMBER(10,0),
	hatTypeStyle NVARCHAR2(256),
	hasParents NCHAR(1),
	optionalNonUniqueTinyInt NUMBER(3,0) CHECK (optionalNonUniqueTinyInt BETWEEN 0 AND 255),
	valueType1DoesSomethingElseWith NUMBER(10,0),
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(personId),
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(firstName, "date"),
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE(lastName, "date"),
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(optionalUniqueString),
	CONSTRAINT InternalUniquenessConstraint13 UNIQUE(wife),
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(ownsCar),
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(optionalUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(mandatoryUniqueString),
	CONSTRAINT InternalUniquenessConstraint86 UNIQUE(optionalUniqueTinyInt),
	CONSTRAINT InternalUniquenessConstraint88 UNIQUE(mandatoryUniqueTinyInt),
	CONSTRAINT InternalUniquenessConstraint49 UNIQUE(childPersonFatherMalePersonId, childPersonBirthOrderNr, childPersonMotherFemalePersonId),
	CONSTRAINT RoleValueConstraint2 CHECK (mandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT RoleValueConstraint1 CHECK (optionalUniqueDecimal BETWEEN 100 AND 4000)
);

CREATE TABLE Task
(
	taskId NUMBER(10,0) NOT NULL,
	personId NUMBER(10,0) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(taskId)
);

CREATE TABLE ValueType1
(
	"value" NUMBER(10,0) NOT NULL,
	doesSomethingWithPerson NUMBER(10,0),
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY("value")
);

CREATE TABLE Death
(
	personId NUMBER(10,0) NOT NULL,
	deathCause NVARCHAR2(14) CHECK (deathCause IN (N'natural', N'not so natural')) NOT NULL,
	isDead NCHAR(1) NOT NULL,
	"date" DATE,
	naturalDeathIsFromProstateCancer NCHAR(1),
	unnaturalDeathIsViolent NCHAR(1),
	unnaturalDeathIsBloody NCHAR(1),
	CONSTRAINT "Constraint" PRIMARY KEY(personId)
);

CREATE TABLE PersonDrivesCar
(
	drivesCar NUMBER(10,0) CHECK (drivesCar >= 0) NOT NULL,
	drivenByPerson NUMBER(10,0) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonDate
(
	carSold NUMBER(10,0) CHECK (carSold >= 0) NOT NULL,
	buyer NUMBER(10,0) NOT NULL,
	seller NUMBER(10,0) NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE Review
(
	car NUMBER(10,0) CHECK (car >= 0) NOT NULL,
	criterion NVARCHAR2(64) NOT NULL,
	nr NUMBER(10,0) CHECK (nr >= 0 AND nr IN (9, 10, 12) OR nr BETWEEN 1 AND 7 OR nr BETWEEN 14 AND 16 OR nr >= 18) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(car, criterion)
);

CREATE TABLE PersonHasNickName
(
	nickName NVARCHAR2(64) NOT NULL,
	personId NUMBER(10,0) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, personId)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife)  REFERENCES Person (personId) ;

ALTER TABLE Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith)  REFERENCES ValueType1 ("value") ;

ALTER TABLE Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFatherMalePersonId)  REFERENCES Person (personId) ;

ALTER TABLE Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMotherFemalePersonId)  REFERENCES Person (personId) ;

ALTER TABLE Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId)  REFERENCES Person (personId) ;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson)  REFERENCES Person (personId) ;

ALTER TABLE Death ADD CONSTRAINT Death_FK FOREIGN KEY (personId)  REFERENCES Person (personId) ;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson)  REFERENCES Person (personId) ;

ALTER TABLE PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer)  REFERENCES Person (personId) ;

ALTER TABLE PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller)  REFERENCES Person (personId) ;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId)  REFERENCES Person (personId) ;

COMMIT WORK;

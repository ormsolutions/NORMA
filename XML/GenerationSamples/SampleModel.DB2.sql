
CREATE SCHEMA SampleModel;

SET SCHEMA 'SAMPLEMODEL';

CREATE TABLE SampleModel.Person
(
	personId INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	firstName CHARACTER VARYING(64) NOT NULL,
	lastName CHARACTER VARYING(64) NOT NULL,
	"date" DATE NOT NULL,
	mandatoryUniqueDecimal DECIMAL(9,0) CHECK (mandatoryUniqueDecimal BETWEEN 4000 AND 20000) NOT NULL,
	mandatoryUniqueString CHARACTER(11) NOT NULL,
	mandatoryUniqueTinyInt SMALLINT CHECK (mandatoryUniqueTinyInt BETWEEN 0 AND 255) NOT NULL,
	genderCode CHARACTER(1) CHECK (genderCode IN ('M', 'F')) NOT NULL,
	mandatoryNonUniqueTinyInt SMALLINT CHECK (mandatoryNonUniqueTinyInt BETWEEN 0 AND 255) NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal DECIMAL(31,31) NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT NOT NULL,
	optionalUniqueString CHARACTER(11),
	ownsCar INTEGER CHECK (ownsCar >= 0),
	optionalUniqueDecimal DECIMAL(9,0),
	optionalUniqueTinyInt SMALLINT CHECK (optionalUniqueTinyInt BETWEEN 0 AND 255),
	wife INTEGER,
	childPersonBirthOrderNr INTEGER CHECK (childPersonBirthOrderNr >= 0 AND childPersonBirthOrderNr >= 1),
	childPersonFatherMalePersonId INTEGER,
	childPersonMotherFemalePersonId INTEGER,
	ColorARGB INTEGER,
	hatTypeStyle CHARACTER VARYING(256),
	hasParents CHARACTER(1) FOR BIT DATA,
	optionalNonUniqueTinyInt SMALLINT CHECK (optionalNonUniqueTinyInt BETWEEN 0 AND 255),
	valueType1DoesSomethingElseWith INTEGER,
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

CREATE TABLE SampleModel.Task
(
	taskId INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	personId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(taskId)
);

CREATE TABLE SampleModel.ValueType1
(
	"value" INTEGER NOT NULL,
	doesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY("value")
);

CREATE TABLE SampleModel.Death
(
	personId INTEGER NOT NULL,
	deathCause CHARACTER VARYING(14) CHECK (deathCause IN ('natural', 'not so natural')) NOT NULL,
	isDead CHARACTER(1) FOR BIT DATA NOT NULL,
	"date" DATE,
	naturalDeathIsFromProstateCancer CHARACTER(1) FOR BIT DATA,
	unnaturalDeathIsViolent CHARACTER(1) FOR BIT DATA,
	unnaturalDeathIsBloody CHARACTER(1) FOR BIT DATA,
	CONSTRAINT "Constraint" PRIMARY KEY(personId)
);

CREATE TABLE SampleModel.PersonDrivesCar
(
	drivesCar INTEGER CHECK (drivesCar >= 0) NOT NULL,
	drivenByPerson INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE SampleModel.PersonBoughtCarFromPersonDate
(
	carSold INTEGER CHECK (carSold >= 0) NOT NULL,
	buyer INTEGER NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE SampleModel.Review
(
	car INTEGER CHECK (car >= 0) NOT NULL,
	criterion CHARACTER VARYING(64) NOT NULL,
	nr INTEGER CHECK (nr >= 0 AND nr IN (9, 10, 12) OR nr BETWEEN 1 AND 7 OR nr BETWEEN 14 AND 16 OR nr >= 18) NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(car, criterion)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	nickName CHARACTER VARYING(64) NOT NULL,
	personId INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, personId)
);

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 ("value") ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFatherMalePersonId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMotherFemalePersonId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Death ADD CONSTRAINT Death_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT;

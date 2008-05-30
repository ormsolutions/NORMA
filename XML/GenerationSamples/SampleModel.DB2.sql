
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
	childPersonFatherPerson_id INTEGER,
	childPersonMotherPerson_id INTEGER,
	ColorARGB INTEGER,
	hatTypeStyle CHARACTER VARYING(256),
	isDead CHARACTER(1) FOR BIT DATA,
	hasParents CHARACTER(1) FOR BIT DATA,
	optionalNonUniqueTinyInt SMALLINT CHECK (optionalNonUniqueTinyInt BETWEEN 0 AND 255),
	valueType1DoesSomethingElseWith INTEGER,
	deathDate DATE,
	deathCause CHARACTER VARYING(14) CHECK (deathCause IN ('natural', 'not so natural')),
	deathNaturalDeathIsFromProstateCancer CHARACTER(1) FOR BIT DATA,
	deathUnnaturalDeathIsViolent CHARACTER(1) FOR BIT DATA,
	deathUnnaturalDeathIsBloody CHARACTER(1) FOR BIT DATA,
	CONSTRAINT Person_PK PRIMARY KEY(personId),
	CONSTRAINT Person_UC1 UNIQUE(firstName, "date"),
	CONSTRAINT Person_UC2 UNIQUE(lastName, "date"),
	CONSTRAINT Person_UC3 UNIQUE(optionalUniqueString),
	CONSTRAINT Person_UC4 UNIQUE(wife),
	CONSTRAINT Person_UC5 UNIQUE(ownsCar),
	CONSTRAINT Person_UC6 UNIQUE(optionalUniqueDecimal),
	CONSTRAINT Person_UC7 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT Person_UC8 UNIQUE(mandatoryUniqueString),
	CONSTRAINT Person_UC9 UNIQUE(optionalUniqueTinyInt),
	CONSTRAINT Person_UC10 UNIQUE(mandatoryUniqueTinyInt),
	CONSTRAINT Person_UC11 UNIQUE(childPersonFatherPerson_id, childPersonBirthOrderNr, childPersonMotherPerson_id),
	CONSTRAINT RoleValueConstraint2 CHECK (mandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT RoleValueConstraint1 CHECK (optionalUniqueDecimal BETWEEN 100 AND 4000)
);

CREATE TABLE SampleModel.Task
(
	taskId INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	personId INTEGER NOT NULL,
	CONSTRAINT Task_PK PRIMARY KEY(taskId)
);

CREATE TABLE SampleModel.ValueType1
(
	"value" INTEGER NOT NULL,
	doesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1_PK PRIMARY KEY("value")
);

CREATE TABLE SampleModel.PersonDrivesCar
(
	drivesCar INTEGER CHECK (drivesCar >= 0) NOT NULL,
	drivenByPerson INTEGER NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE SampleModel.PersonBoughtCarFromPersonDate
(
	carSold INTEGER CHECK (carSold >= 0) NOT NULL,
	buyer INTEGER NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC2 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE SampleModel.Review
(
	car INTEGER CHECK (car >= 0) NOT NULL,
	criterion CHARACTER VARYING(64) NOT NULL,
	nr INTEGER CHECK (nr >= 0 AND nr IN (9, 10, 12) OR nr BETWEEN 1 AND 7 OR nr BETWEEN 14 AND 16 OR nr >= 18) NOT NULL,
	CONSTRAINT Review_PK PRIMARY KEY(car, criterion)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	nickName CHARACTER VARYING(64) NOT NULL,
	personId INTEGER NOT NULL,
	CONSTRAINT PersonHasNickName_PK PRIMARY KEY(nickName, personId)
);

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 ("value") ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFatherPerson_id) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMotherPerson_id) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT;

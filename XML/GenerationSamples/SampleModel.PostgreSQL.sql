
START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel;

CREATE DOMAIN SampleModel.TINYINT AS SMALLINT CONSTRAINT TINYINT_RangeCheck CHECK (VALUE BETWEEN 0 AND 255);

SET search_path TO SAMPLEMODEL,"$user",public;

CREATE DOMAIN SampleModel.vin AS INTEGER CONSTRAINT vin_Unsigned_Chk CHECK (VALUE >= 0);

CREATE DOMAIN SampleModel."Integer" AS INTEGER CONSTRAINT ValueTypeValueConstraint1 CHECK (VALUE >= 0 AND VALUE IN (9, 10, 12) OR VALUE BETWEEN 1 AND 7 OR VALUE BETWEEN 14 AND 16 OR VALUE >= 18);

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT ValueTypeValueConstraint2 CHECK (VALUE IN ('natural', 'not so natural'));

CREATE DOMAIN SampleModel.BirthOrder_Nr AS INTEGER CONSTRAINT ValueTypeValueConstraint10 CHECK (VALUE >= 0 AND VALUE >= 1);

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT ValueTypeValueConstraint3 CHECK (VALUE IN ('M', 'F'));

CREATE DOMAIN SampleModel.MandatoryUniqueDecimal AS DECIMAL(9,0) CONSTRAINT ValueTypeValueConstraint9 CHECK (VALUE BETWEEN 4000 AND 20000);

CREATE TABLE SampleModel.Person
(
	personId SERIAL NOT NULL,
	firstName CHARACTER VARYING(64) NOT NULL,
	lastName CHARACTER VARYING(64) NOT NULL,
	"date" DATE NOT NULL,
	mandatoryUniqueDecimal SampleModel.MandatoryUniqueDecimal NOT NULL,
	mandatoryUniqueString CHARACTER(11) NOT NULL,
	mandatoryUniqueTinyInt SampleModel.TINYINT NOT NULL,
	genderCode SampleModel.Gender_Code NOT NULL,
	mandatoryNonUniqueTinyInt SampleModel.TINYINT NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal DECIMAL NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT NOT NULL,
	optionalUniqueString CHARACTER(11),
	ownsCar SampleModel.vin,
	optionalUniqueDecimal DECIMAL(9,0),
	optionalUniqueTinyInt SampleModel.TINYINT,
	wife INTEGER,
	childPersonBirthOrderNr SampleModel.BirthOrder_Nr,
	childPersonFather INTEGER,
	childPersonMother INTEGER,
	ColorARGB INTEGER,
	hatTypeStyle CHARACTER VARYING(256),
	isDead BOOLEAN,
	hasParents BOOLEAN,
	optionalNonUniqueTinyInt SampleModel.TINYINT,
	valueType1DoesSomethingElseWith INTEGER,
	deathDate DATE,
	deathCause SampleModel.DeathCause_Type,
	deathNaturalDeathIsFromProstateCancer BOOLEAN,
	deathUnnaturalDeathIsViolent BOOLEAN,
	deathUnnaturalDeathIsBloody BOOLEAN,
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
	CONSTRAINT Person_UC11 UNIQUE(childPersonFather, childPersonBirthOrderNr, childPersonMother),
	CONSTRAINT Person_mandatoryUniqueDecimal_RoleValueConstraint2 CHECK (mandatoryUniqueDecimal BETWEEN 9000 AND 10000),
	CONSTRAINT Person_optionalUniqueDecimal_RoleValueConstraint1 CHECK (optionalUniqueDecimal BETWEEN 100 AND 4000),
	CONSTRAINT Person_Death_MandatoryGroup CHECK (deathCause IS NOT NULL OR deathCause IS NULL AND deathDate IS NULL AND deathNaturalDeathIsFromProstateCancer IS NULL AND deathUnnaturalDeathIsViolent IS NULL AND deathUnnaturalDeathIsBloody IS NULL),
	CONSTRAINT Person_ChildPerson_MandatoryGroup CHECK (childPersonBirthOrderNr IS NOT NULL AND childPersonMother IS NOT NULL AND childPersonFather IS NOT NULL OR childPersonBirthOrderNr IS NULL AND childPersonMother IS NULL AND childPersonFather IS NULL)
);

CREATE TABLE SampleModel.Task
(
	taskId SERIAL NOT NULL,
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
	drivesCar SampleModel.vin NOT NULL,
	drivenByPerson INTEGER NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE SampleModel.PersonBoughtCarFromPersonDate
(
	carSold SampleModel.vin NOT NULL,
	buyer INTEGER NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC2 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE SampleModel.Review
(
	car SampleModel.vin NOT NULL,
	criterion CHARACTER VARYING(64) NOT NULL,
	nr SampleModel."Integer" NOT NULL,
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

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;

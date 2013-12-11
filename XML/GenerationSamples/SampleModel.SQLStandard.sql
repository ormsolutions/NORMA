
START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel DEFAULT CHARACTER SET UTF8;

SET SCHEMA 'SAMPLEMODEL';

CREATE DOMAIN SampleModel.vin AS INTEGER CONSTRAINT vin_Unsigned_Chk CHECK (VALUE >= 0);

CREATE DOMAIN SampleModel."Integer" AS INTEGER CONSTRAINT ValueTypeValueConstraint1 CHECK (VALUE >= 0 AND VALUE IN (9, 10, 12) OR VALUE BETWEEN 1 AND 7 OR VALUE BETWEEN 14 AND 16 OR VALUE >= 18);

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT ValueTypeValueConstraint2 CHECK (VALUE IN ('natural', 'not so natural'));

CREATE DOMAIN SampleModel.BirthOrder_Nr AS INTEGER CONSTRAINT ValueTypeValueConstraint10 CHECK (VALUE >= 0 AND VALUE >= 1);

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT ValueTypeValueConstraint3 CHECK (VALUE IN ('M', 'F'));

CREATE DOMAIN SampleModel.MandatoryUniqueDecimal AS DECIMAL(9,0) CONSTRAINT ValueTypeValueConstraint9 CHECK (VALUE BETWEEN 4000 AND 20000);

CREATE TABLE SampleModel.Person
(
	personId INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	"date" DATE NOT NULL,
	firstName CHARACTER VARYING(64) NOT NULL,
	genderCode SampleModel.Gender_Code NOT NULL,
	lastName CHARACTER VARYING(64) NOT NULL,
	mandatoryNonUniqueTinyInt TINYINT NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal DECIMAL NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT NOT NULL,
	mandatoryUniqueDecimal SampleModel.MandatoryUniqueDecimal NOT NULL,
	mandatoryUniqueString CHARACTER(11) NOT NULL,
	mandatoryUniqueTinyInt TINYINT NOT NULL,
	childPersonBirthOrderNr SampleModel.BirthOrder_Nr,
	childPersonFather INTEGER,
	childPersonMother INTEGER,
	optionalUniqueDecimal DECIMAL(9,0),
	optionalUniqueString CHARACTER(11),
	optionalUniqueTinyInt TINYINT,
	ownsCar SampleModel.vin,
	wife INTEGER,
	colorARGB INTEGER,
	deathCause SampleModel.DeathCause_Type,
	deathDate DATE,
	deathNaturalDeathIsFromProstateCancer BOOLEAN,
	deathUnnaturalDeathIsBloody BOOLEAN,
	deathUnnaturalDeathIsViolent BOOLEAN,
	hasParents BOOLEAN,
	hatTypeStyle CHARACTER VARYING(256),
	isDead BOOLEAN,
	optionalNonUniqueTinyInt TINYINT,
	valueType1DoesSomethingElseWith INTEGER,
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
	CONSTRAINT Person_Death_MandatoryGroup CHECK (deathCause IS NOT NULL OR deathDate IS NULL AND deathNaturalDeathIsFromProstateCancer IS NULL AND deathCause IS NULL AND deathUnnaturalDeathIsViolent IS NULL AND deathUnnaturalDeathIsBloody IS NULL),
	CONSTRAINT Person_ChildPerson_MandatoryGroup CHECK (childPersonMother IS NOT NULL AND childPersonBirthOrderNr IS NOT NULL AND childPersonFather IS NOT NULL OR childPersonMother IS NULL AND childPersonBirthOrderNr IS NULL AND childPersonFather IS NULL)
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
	drivenByPerson INTEGER NOT NULL,
	drivesCar SampleModel.vin NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE SampleModel.PersonBoughtCarFromPersonOnDate
(
	buyer INTEGER NOT NULL,
	carSold SampleModel.vin NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonOnDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonOnDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonOnDate_UC2 UNIQUE(saleDate, seller, carSold)
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

ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES SampleModel.Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;

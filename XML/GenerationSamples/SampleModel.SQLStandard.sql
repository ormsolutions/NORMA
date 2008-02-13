
START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel DEFAULT CHARACTER SET UTF8;

SET SCHEMA 'SAMPLEMODEL';

CREATE DOMAIN SampleModel.vin AS INTEGER CONSTRAINT vin_Unsigned_Chk CHECK (VALUE >= 0);

CREATE DOMAIN SampleModel."Integer" AS INTEGER CONSTRAINT ValueTypeValueConstraint1 CHECK (VALUE >= 0 AND VALUE IN (9, 10, 12) OR VALUE BETWEEN 1 AND 7 OR VALUE BETWEEN 14 AND 16 OR VALUE >= 18);

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT ValueTypeValueConstraint2 CHECK (VALUE IN ('natural', 'not so natural'));

CREATE DOMAIN SampleModel.BirthOrder_Nr AS INTEGER CONSTRAINT ValueTypeValueConstraint10 CHECK (VALUE >= 0 AND VALUE >= 1);

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT ValueTypeValueConstraint3 CHECK (VALUE IN ('M', 'F'));

CREATE DOMAIN SampleModel.MandatoryUniqueDecimal AS DECIMAL(9) CONSTRAINT ValueTypeValueConstraint9 CHECK (VALUE BETWEEN 4000 AND 20000);

CREATE TABLE SampleModel.Person
(
	person_Id INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	firstName CHARACTER VARYING(64) NOT NULL,
	"date" DATE NOT NULL,
	lastName CHARACTER VARYING(64) NOT NULL,
	mandatoryUniqueDecimal SampleModel.MandatoryUniqueDecimal NOT NULL,
	mandatoryUniqueString CHARACTER(11) NOT NULL,
	gender_Code SampleModel.Gender_Code NOT NULL,
	optionalUniqueString CHARACTER(11),
	ownsCar SampleModel.vin,
	optionalUniqueDecimal DECIMAL(9),
	wife INTEGER,
	childPerson SampleModel.BirthOrder_Nr,
	childPersonFather INTEGER,
	childPersonMother INTEGER,
	wearsHatTypePerson1 INTEGER,
	wearsHatTypePerson2 CHARACTER VARYING(256),
	hasParents BOOLEAN,
	valueType1DoesSomethingElseWith INTEGER,
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

CREATE TABLE SampleModel.Task
(
	task_Id INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	person_Id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(task_Id)
);

CREATE TABLE SampleModel.ValueType1
(
	valueType1Value INTEGER NOT NULL,
	doesSomethingWithPerson INTEGER,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(valueType1Value)
);

CREATE TABLE SampleModel.Death
(
	isDeadPerson_Id INTEGER NOT NULL,
	deathCause SampleModel.DeathCause_Type NOT NULL,
	isDeadPersonisDead BOOLEAN NOT NULL,
	"date" DATE,
	naturalDeathIsFromProstateCancer BOOLEAN,
	unnaturalDeathIsViolent BOOLEAN,
	unnaturalDeathIsBloody BOOLEAN,
	CONSTRAINT "Constraint" PRIMARY KEY(isDeadPerson_Id)
);

CREATE TABLE SampleModel.PersonDrivesCar
(
	drivesCar SampleModel.vin NOT NULL,
	drivenByPerson INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE SampleModel.PersonBoughtCarFromPersonOnDate
(
	carSold SampleModel.vin NOT NULL,
	buyer INTEGER NOT NULL,
	seller INTEGER NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE SampleModel.Review
(
	vin SampleModel.vin NOT NULL,
	name CHARACTER VARYING(64) NOT NULL,
	"integer" SampleModel."Integer" NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(vin, name)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	nickName CHARACTER VARYING(64) NOT NULL,
	person_Id INTEGER NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, person_Id)
);

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES SampleModel.ValueType1 (valueType1Value) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (wife) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_FK FOREIGN KEY (person_Id) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Death ADD CONSTRAINT Death_FK FOREIGN KEY (isDeadPerson_Id) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (person_Id) REFERENCES SampleModel.Person (person_Id) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;

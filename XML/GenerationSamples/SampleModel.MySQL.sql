CREATE TABLE Person
(
	person_Id INT AUTO_INCREMENT NOT NULL,
	firstName VARCHAR(64) NOT NULL,
	`date` DATE NOT NULL,
	lastName VARCHAR(64) NOT NULL,
	mandatoryUniqueDecimal  NOT NULL,
	mandatoryUniqueString CHAR(11) NOT NULL,
	gender_Code CHAR(1) NOT NULL,
	optionalUniqueString CHAR(11),
	ownsCar ,
	optionalUniqueDecimal DECIMAL(9),
	wife INT,
	childPerson ,
	childPersonFather INT,
	childPersonMother INT,
	wearsHatTypePerson1 INT,
	wearsHatTypePerson2 VARCHAR(256),
	hasParents BOOLEAN,
	valueType1DoesSomethingElseWith INT,
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
	task_Id INT AUTO_INCREMENT NOT NULL,
	person_Id INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(task_Id)
);

CREATE TABLE ValueType1
(
	valueType1Value INT NOT NULL,
	doesSomethingWithPerson INT,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(valueType1Value)
);

CREATE TABLE Death
(
	isDeadPerson_Id INT NOT NULL,
	deathCause VARCHAR(14) NOT NULL,
	isDeadPersonisDead BOOLEAN NOT NULL,
	`date` DATE,
	naturalDeathIsFromProstateCancer BOOLEAN,
	unnaturalDeathIsViolent BOOLEAN,
	unnaturalDeathIsBloody BOOLEAN,
	CONSTRAINT `Constraint` PRIMARY KEY(isDeadPerson_Id)
);

CREATE TABLE PersonDrivesCar
(
	drivesCar  NOT NULL,
	drivenByPerson INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonOnDate
(
	carSold  NOT NULL,
	buyer INT NOT NULL,
	seller INT NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE Review
(
	vin  NOT NULL,
	name VARCHAR(64) NOT NULL,
	`integer`  NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(vin, name)
);

CREATE TABLE PersonHasNickName
(
	nickName VARCHAR(64) NOT NULL,
	person_Id INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, person_Id)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (valueType1DoesSomethingElseWith)  REFERENCES ValueType1 (valueType1Value)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (wife)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Task ADD CONSTRAINT Task_FK FOREIGN KEY (person_Id)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Death ADD CONSTRAINT Death_FK FOREIGN KEY (isDeadPerson_Id)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (person_Id)  REFERENCES Person (person_Id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


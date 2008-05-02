
CREATE TABLE Person
(
	personId INT AUTO_INCREMENT NOT NULL,
	firstName VARCHAR(64) NOT NULL,
	lastName VARCHAR(64) NOT NULL,
	`date` DATE NOT NULL,
	mandatoryUniqueDecimal DECIMAL(9,0) NOT NULL,
	mandatoryUniqueString CHAR(11) NOT NULL,
	mandatoryUniqueTinyInt TINYINT UNSIGNED NOT NULL,
	genderCode CHAR(1) NOT NULL,
	mandatoryNonUniqueTinyInt TINYINT UNSIGNED NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal DECIMAL(65,65) NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT(53) NOT NULL,
	optionalUniqueString CHAR(11),
	ownsCar INT,
	optionalUniqueDecimal DECIMAL(9,0),
	optionalUniqueTinyInt TINYINT UNSIGNED,
	wife INT,
	childPersonBirthOrderNr INT,
	childPersonFatherMalePersonId INT,
	childPersonMotherFemalePersonId INT,
	ColorARGB INT,
	hatTypeStyle VARCHAR(256),
	hasParents BIT(1),
	optionalNonUniqueTinyInt TINYINT UNSIGNED,
	valueType1DoesSomethingElseWith INT,
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(personId),
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(firstName, `date`),
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE(lastName, `date`),
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(optionalUniqueString),
	CONSTRAINT InternalUniquenessConstraint13 UNIQUE(wife),
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(ownsCar),
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(optionalUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(mandatoryUniqueString),
	CONSTRAINT InternalUniquenessConstraint86 UNIQUE(optionalUniqueTinyInt),
	CONSTRAINT InternalUniquenessConstraint88 UNIQUE(mandatoryUniqueTinyInt),
	CONSTRAINT InternalUniquenessConstraint49 UNIQUE(childPersonFatherMalePersonId, childPersonBirthOrderNr, childPersonMotherFemalePersonId),
);

CREATE TABLE Task
(
	taskId INT AUTO_INCREMENT NOT NULL,
	personId INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(taskId)
);

CREATE TABLE ValueType1
(
	`value` INT NOT NULL,
	doesSomethingWithPerson INT,
	CONSTRAINT ValueType1Uniqueness PRIMARY KEY(`value`)
);

CREATE TABLE Death
(
	personId INT NOT NULL,
	deathCause VARCHAR(14) NOT NULL,
	isDead BIT(1) NOT NULL,
	`date` DATE,
	naturalDeathIsFromProstateCancer BIT(1),
	unnaturalDeathIsViolent BIT(1),
	unnaturalDeathIsBloody BIT(1),
	CONSTRAINT `Constraint` PRIMARY KEY(personId)
);

CREATE TABLE PersonDrivesCar
(
	drivesCar INT NOT NULL,
	drivenByPerson INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonDate
(
	carSold INT NOT NULL,
	buyer INT NOT NULL,
	seller INT NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE Review
(
	car INT NOT NULL,
	criterion VARCHAR(64) NOT NULL,
	nr INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(car, criterion)
);

CREATE TABLE PersonHasNickName
(
	nickName VARCHAR(64) NOT NULL,
	personId INT NOT NULL,
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(nickName, personId)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES ValueType1 (`value`) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFatherMalePersonId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMotherFemalePersonId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Death ADD CONSTRAINT Death_FK FOREIGN KEY (personId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

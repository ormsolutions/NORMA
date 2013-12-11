
CREATE TABLE Person
(
	personId INT AUTO_INCREMENT NOT NULL,
	`date` DATE NOT NULL,
	firstName VARCHAR(64) NOT NULL,
	genderCode CHAR(1) NOT NULL,
	lastName VARCHAR(64) NOT NULL,
	mandatoryNonUniqueTinyInt TINYINT UNSIGNED NOT NULL,
	mandatoryNonUniqueUnconstrainedDecimal DECIMAL(65,65) NOT NULL,
	mandatoryNonUniqueUnconstrainedFloat FLOAT(53) NOT NULL,
	mandatoryUniqueDecimal DECIMAL(9,0) NOT NULL,
	mandatoryUniqueString CHAR(11) NOT NULL,
	mandatoryUniqueTinyInt TINYINT UNSIGNED NOT NULL,
	childPersonBirthOrderNr INT,
	childPersonFather INT,
	childPersonMother INT,
	optionalUniqueDecimal DECIMAL(9,0),
	optionalUniqueString CHAR(11),
	optionalUniqueTinyInt TINYINT UNSIGNED,
	ownsCar INT,
	wife INT,
	colorARGB INT,
	deathCause VARCHAR(14),
	deathDate DATE,
	deathNaturalDeathIsFromProstateCancer BIT(1),
	deathUnnaturalDeathIsBloody BIT(1),
	deathUnnaturalDeathIsViolent BIT(1),
	hasParents BIT(1),
	hatTypeStyle VARCHAR(256),
	isDead BIT(1),
	optionalNonUniqueTinyInt TINYINT UNSIGNED,
	valueType1DoesSomethingElseWith INT,
	CONSTRAINT Person_PK PRIMARY KEY(personId),
	CONSTRAINT Person_UC1 UNIQUE(firstName, `date`),
	CONSTRAINT Person_UC2 UNIQUE(lastName, `date`),
	CONSTRAINT Person_UC3 UNIQUE(optionalUniqueString),
	CONSTRAINT Person_UC4 UNIQUE(wife),
	CONSTRAINT Person_UC5 UNIQUE(ownsCar),
	CONSTRAINT Person_UC6 UNIQUE(optionalUniqueDecimal),
	CONSTRAINT Person_UC7 UNIQUE(mandatoryUniqueDecimal),
	CONSTRAINT Person_UC8 UNIQUE(mandatoryUniqueString),
	CONSTRAINT Person_UC9 UNIQUE(optionalUniqueTinyInt),
	CONSTRAINT Person_UC10 UNIQUE(mandatoryUniqueTinyInt),
	CONSTRAINT Person_UC11 UNIQUE(childPersonFather, childPersonBirthOrderNr, childPersonMother)
);

CREATE TABLE Task
(
	taskId INT AUTO_INCREMENT NOT NULL,
	personId INT NOT NULL,
	CONSTRAINT Task_PK PRIMARY KEY(taskId)
);

CREATE TABLE ValueType1
(
	`value` INT NOT NULL,
	doesSomethingWithPerson INT,
	CONSTRAINT ValueType1_PK PRIMARY KEY(`value`)
);

CREATE TABLE PersonDrivesCar
(
	drivenByPerson INT NOT NULL,
	drivesCar INT NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonOnDate
(
	buyer INT NOT NULL,
	carSold INT NOT NULL,
	seller INT NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonOnDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonOnDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonOnDate_UC2 UNIQUE(saleDate, seller, carSold)
);

CREATE TABLE Review
(
	car INT NOT NULL,
	criterion VARCHAR(64) NOT NULL,
	nr INT NOT NULL,
	CONSTRAINT Review_PK PRIMARY KEY(car, criterion)
);

CREATE TABLE PersonHasNickName
(
	nickName VARCHAR(64) NOT NULL,
	personId INT NOT NULL,
	CONSTRAINT PersonHasNickName_PK PRIMARY KEY(nickName, personId)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK1 FOREIGN KEY (wife) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK2 FOREIGN KEY (valueType1DoesSomethingElseWith) REFERENCES ValueType1 (`value`) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK3 FOREIGN KEY (childPersonFather) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Person ADD CONSTRAINT Person_FK4 FOREIGN KEY (childPersonMother) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE Task ADD CONSTRAINT Task_FK FOREIGN KEY (personId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE ValueType1 ADD CONSTRAINT ValueType1_FK FOREIGN KEY (doesSomethingWithPerson) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_FK FOREIGN KEY (drivenByPerson) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK1 FOREIGN KEY (buyer) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonOnDate ADD CONSTRAINT PersonBoughtCarFromPersonOnDate_FK2 FOREIGN KEY (seller) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

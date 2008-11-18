
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
	childPersonFather INT,
	childPersonMother INT,
	ColorARGB INT,
	hatTypeStyle VARCHAR(256),
	isDead BIT(1),
	hasParents BIT(1),
	optionalNonUniqueTinyInt TINYINT UNSIGNED,
	valueType1DoesSomethingElseWith INT,
	deathDate DATE,
	deathCause VARCHAR(14),
	deathNaturalDeathIsFromProstateCancer BIT(1),
	deathUnnaturalDeathIsViolent BIT(1),
	deathUnnaturalDeathIsBloody BIT(1),
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
	drivesCar INT NOT NULL,
	drivenByPerson INT NOT NULL,
	CONSTRAINT PersonDrivesCar_PK PRIMARY KEY(drivesCar, drivenByPerson)
);

CREATE TABLE PersonBoughtCarFromPersonDate
(
	carSold INT NOT NULL,
	buyer INT NOT NULL,
	seller INT NOT NULL,
	saleDate DATE NOT NULL,
	CONSTRAINT PersonBoughtCarFromPersonDate_PK PRIMARY KEY(buyer, carSold, seller),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC1 UNIQUE(carSold, saleDate, buyer),
	CONSTRAINT PersonBoughtCarFromPersonDate_UC2 UNIQUE(saleDate, seller, carSold)
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

ALTER TABLE PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK1 FOREIGN KEY (buyer) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonBoughtCarFromPersonDate ADD CONSTRAINT PersonBoughtCarFromPersonDate_FK2 FOREIGN KEY (seller) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_FK FOREIGN KEY (personId) REFERENCES Person (personId) ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE TABLE Person
(
	personId INT AUTO_INCREMENT NOT NULL,
	lastName VARCHAR(30) NOT NULL,
	firstName VARCHAR(30) NOT NULL,
	title VARCHAR(4),
	countryName VARCHAR(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(personId)
);

CREATE TABLE Country
(
	countryName VARCHAR(20) NOT NULL,
	regionCode CHAR(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(countryName)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName) REFERENCES Country (countryName) ON DELETE RESTRICT ON UPDATE RESTRICT;

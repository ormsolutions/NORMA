
CREATE TABLE Person
(
	personId INT AUTO_INCREMENT NOT NULL,
	firstName VARCHAR(30) NOT NULL,
	lastName VARCHAR(30) NOT NULL,
	countryName VARCHAR(20),
	title VARCHAR(4),
	CONSTRAINT Person_PK PRIMARY KEY(personId)
);

CREATE TABLE Country
(
	countryName VARCHAR(20) NOT NULL,
	regionCode CHAR(8),
	CONSTRAINT Country_PK PRIMARY KEY(countryName)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName) REFERENCES Country (countryName) ON DELETE RESTRICT ON UPDATE RESTRICT;

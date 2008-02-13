CREATE TABLE Person
(
	person_Id INT AUTO_INCREMENT NOT NULL,
	lastName VARCHAR(30) NOT NULL,
	firstName VARCHAR(30) NOT NULL,
	title VARCHAR(4),
	country_Name VARCHAR(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(person_Id)
);

CREATE TABLE Country
(
	country_Name VARCHAR(20) NOT NULL,
	region_Code CHAR(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(country_Name)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK FOREIGN KEY (country_Name)  REFERENCES Country (country_Name)  ON DELETE RESTRICT ON UPDATE RESTRICT;


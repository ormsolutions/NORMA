CREATE TABLE Person
(
	Person_id BIGINT AUTO_INCREMENT NOT NULL, 
	LastName VARCHAR(30) NOT NULL, 
	FirstName VARCHAR(30) NOT NULL, 
	Title VARCHAR(4) , 
	Country_Country_name VARCHAR(20) , 
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(Person_id)
);

CREATE TABLE Country
(
	Country_name VARCHAR(20) NOT NULL, 
	Region_Region_code CHAR(8) , 
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(Country_name)
);

ALTER TABLE Person ADD CONSTRAINT Country_FK FOREIGN KEY (Country_Country_name)  REFERENCES Country (Country_name)  ON DELETE RESTRICT ON UPDATE RESTRICT;


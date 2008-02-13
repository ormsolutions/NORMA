
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	person_Id NUMBER NOT NULL,
	lastName NVARCHAR2(30) NOT NULL,
	firstName NVARCHAR2(30) NOT NULL,
	title NVARCHAR2(4) CHECK (title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')),
	country_Name NVARCHAR2(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(person_Id)
);

CREATE TABLE Country
(
	country_Name NVARCHAR2(20) NOT NULL,
	region_Code NCHAR(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(country_Name)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK FOREIGN KEY (country_Name)  REFERENCES Country (country_Name) ;

COMMIT WORK;

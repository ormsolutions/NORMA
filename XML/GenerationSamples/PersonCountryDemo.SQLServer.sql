CREATE SCHEMA PersonCountryDemo
GO

GO


CREATE TABLE PersonCountryDemo.Person
(
	person_Id INTEGER IDENTITY (1, 1) NOT NULL,
	lastName NATIONAL CHARACTER VARYING(30) NOT NULL,
	firstName NATIONAL CHARACTER VARYING(30) NOT NULL,
	title NATIONAL CHARACTER VARYING(4) CHECK (title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')),
	country_Name NATIONAL CHARACTER VARYING(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(person_Id)
)
GO


CREATE TABLE PersonCountryDemo.Country
(
	country_Name NATIONAL CHARACTER VARYING(20) NOT NULL,
	region_Code NATIONAL CHARACTER(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(country_Name)
)
GO


ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_FK FOREIGN KEY (country_Name) REFERENCES PersonCountryDemo.Country (country_Name) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
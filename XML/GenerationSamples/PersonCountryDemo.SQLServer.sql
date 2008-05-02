CREATE SCHEMA PersonCountryDemo
GO

GO


CREATE TABLE PersonCountryDemo.Person
(
	personId INTEGER IDENTITY (1, 1) NOT NULL,
	lastName NATIONAL CHARACTER VARYING(30) NOT NULL,
	firstName NATIONAL CHARACTER VARYING(30) NOT NULL,
	title NATIONAL CHARACTER VARYING(4) CHECK (title IN (N'Dr', N'Prof', N'Mr', N'Mrs', N'Miss', N'Ms')),
	countryName NATIONAL CHARACTER VARYING(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(personId)
)
GO


CREATE TABLE PersonCountryDemo.Country
(
	countryName NATIONAL CHARACTER VARYING(20) NOT NULL,
	regionCode NATIONAL CHARACTER(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(countryName)
)
GO


ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName) REFERENCES PersonCountryDemo.Country (countryName) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO
CREATE SCHEMA PersonCountryDemo
GO

GO


CREATE TABLE PersonCountryDemo.Person
(
	personId int IDENTITY (1, 1) NOT NULL,
	firstName nvarchar(30) NOT NULL,
	lastName nvarchar(30) NOT NULL,
	countryName nvarchar(20),
	title nvarchar(4) CHECK (title IN (N'Dr', N'Prof', N'Mr', N'Mrs', N'Miss', N'Ms')),
	CONSTRAINT Person_PK PRIMARY KEY(personId)
)
GO


CREATE TABLE PersonCountryDemo.Country
(
	countryName nvarchar(20) NOT NULL,
	regionCode nchar(8),
	CONSTRAINT Country_PK PRIMARY KEY(countryName)
)
GO


ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName) REFERENCES PersonCountryDemo.Country (countryName) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	personId NUMBER(10,0) NOT NULL,
	firstName NVARCHAR2(30) NOT NULL,
	lastName NVARCHAR2(30) NOT NULL,
	countryName NVARCHAR2(20),
	title NVARCHAR2(4) CHECK (title IN (N'Dr', N'Prof', N'Mr', N'Mrs', N'Miss', N'Ms')),
	CONSTRAINT Person_PK PRIMARY KEY(personId)
);

CREATE TABLE Country
(
	countryName NVARCHAR2(20) NOT NULL,
	regionCode NCHAR(8),
	CONSTRAINT Country_PK PRIMARY KEY(countryName)
);

ALTER TABLE Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName)  REFERENCES Country (countryName) ;

COMMIT WORK;

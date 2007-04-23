CREATE SCHEMA PersonCountryDemo
GO

GO

CREATE TABLE PersonCountryDemo.Person
(
	Person_id BIGINT IDENTITY (1, 1) NOT NULL, 
	LastName NATIONAL CHARACTER VARYING(30) NOT NULL, 
	FirstName NATIONAL CHARACTER VARYING(30) NOT NULL, 
	Title NATIONAL CHARACTER VARYING(4) CONSTRAINT Title_Chk CHECK (Title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')) , 
	Country_Country_name NATIONAL CHARACTER VARYING(20) , 
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(Person_id)
)
GO


CREATE TABLE PersonCountryDemo.Country
(
	Country_name NATIONAL CHARACTER VARYING(20) NOT NULL, 
	Region_Region_code NATIONAL CHARACTER(8) CONSTRAINT Region_code_Chk CHECK ((LEN(LTRIM(RTRIM(Region_Region_code)))) >= 8) , 
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(Country_name)
)
GO


ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_Country_FK FOREIGN KEY (Country_Country_name)  REFERENCES PersonCountryDemo.Country (Country_name)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO



GO
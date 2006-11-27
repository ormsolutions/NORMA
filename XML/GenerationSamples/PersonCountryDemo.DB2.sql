CREATE SCHEMA PersonCountryDemo;

SET SCHEMA 'PERSONCOUNTRYDEMO';

CREATE TABLE PersonCountryDemo.Person
(
	Person_id BIGINT GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL, 
	LastName CHARACTER VARYING(30) NOT NULL, 
	FirstName CHARACTER VARYING(30) NOT NULL, 
	Title CHARACTER VARYING(4) CONSTRAINT Title_Chk CHECK (Title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')) , 
	Country_Country_name CHARACTER VARYING(20) , 
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(Person_id)
);

CREATE TABLE PersonCountryDemo.Country
(
	Country_name CHARACTER VARYING(20) NOT NULL, 
	Region_Region_code CHARACTER(8) CONSTRAINT Region_code_Chk CHECK ((LENGTH(LTRIM(RTRIM(Region_Region_code)))) >= 8) , 
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(Country_name)
);

ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Country_FK FOREIGN KEY (Country_Country_name)  REFERENCES PersonCountryDemo.Country (Country_name)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE PROCEDURE PersonCountryDemo.InsertPerson
(
	Person_id BIGINT , 
	LastName CHARACTER VARYING(30) , 
	FirstName CHARACTER VARYING(30) , 
	Title CHARACTER VARYING(4) , 
	Country_Country_name CHARACTER VARYING(20) 
)
AS
	INSERT INTO PersonCountryDemo.Person(Person_id, LastName, FirstName, Title, Country_Country_name)
	VALUES (Person_id, LastName, FirstName, Title, Country_Country_name);

CREATE PROCEDURE PersonCountryDemo.DeletePerson
(
	Person_id BIGINT 
)
AS
	DELETE FROM PersonCountryDemo.Person
	WHERE Person_id = Person_id;

CREATE PROCEDURE PersonCountryDemo.InsertCountry
(
	Country_name CHARACTER VARYING(20) , 
	Region_Region_code CHARACTER(8) 
)
AS
	INSERT INTO PersonCountryDemo.Country(Country_name, Region_Region_code)
	VALUES (Country_name, Region_Region_code);

CREATE PROCEDURE PersonCountryDemo.DeleteCountry
(
	Country_name CHARACTER VARYING(20) 
)
AS
	DELETE FROM PersonCountryDemo.Country
	WHERE Country_name = Country_name;
COMMIT;


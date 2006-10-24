START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA PersonCountryDemo;

CREATE DOMAIN PersonCountryDemo.Title AS CHARACTER VARYING(4) CONSTRAINT Title_Chk CHECK (VALUE IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')) ;

CREATE DOMAIN PersonCountryDemo.Region_code AS CHARACTER(8) CONSTRAINT Region_code_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 8) ;

CREATE TABLE PersonCountryDemo.Person
(
	Person_id BIGSERIAL NOT NULL, 
	LastName CHARACTER VARYING(30) NOT NULL, 
	FirstName CHARACTER VARYING(30) NOT NULL, 
	Title PersonCountryDemo.Title , 
	Cntry_Cntry_nm CHARACTER VARYING(20) , 
	CONSTRAINT IUC1 PRIMARY KEY(Person_id)
);

CREATE TABLE PersonCountryDemo.Country
(
	Country_name CHARACTER VARYING(20) NOT NULL, 
	Region_Region_code PersonCountryDemo.Region_code , 
	CONSTRAINT IUC3 PRIMARY KEY(Country_name)
);

ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Country_FK FOREIGN KEY (Cntry_Cntry_nm)  REFERENCES PersonCountryDemo.Country (Country_name)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE PROCEDURE PersonCountryDemo.InsertPerson
(
	Person_id BIGINT , 
	LastName CHARACTER VARYING(30) , 
	FirstName CHARACTER VARYING(30) , 
	Title CHARACTER VARYING(4) , 
	Cntry_Cntry_nm CHARACTER VARYING(20) 
)
AS
	INSERT INTO PersonCountryDemo.Person(Person_id, LastName, FirstName, Title, Cntry_Cntry_nm)
	VALUES (Person_id, LastName, FirstName, Title, Cntry_Cntry_nm);

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
COMMIT WORK;


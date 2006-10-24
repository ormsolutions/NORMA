SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	Person_id NUMBER(38) NOT NULL, 
	LastName CHARACTER VARYING(30) NOT NULL, 
	FirstName CHARACTER VARYING(30) NOT NULL, 
	Title CHARACTER VARYING(4) CONSTRAINT Title_Chk CHECK (Title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')) , 
	Cntry_Cntry_nm CHARACTER VARYING(20) , 
	CONSTRAINT IUC1 PRIMARY KEY(Person_id)
);

CREATE TABLE Country
(
	Country_name CHARACTER VARYING(20) NOT NULL, 
	Region_Region_code CHARACTER(8) CONSTRAINT Region_code_Chk CHECK ((LENGTH(TRIM(BOTH FROM Region_Region_code))) >= 8) , 
	CONSTRAINT IUC3 PRIMARY KEY(Country_name)
);

ALTER TABLE Person ADD CONSTRAINT Person_Country_FK FOREIGN KEY (Cntry_Cntry_nm)  REFERENCES Country (Country_name) ;


CREATE PROCEDURE PersonCountryDemo.InsertPerson
(
	Person_id NUMBER(38) , 
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
	Person_id NUMBER(38) 
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


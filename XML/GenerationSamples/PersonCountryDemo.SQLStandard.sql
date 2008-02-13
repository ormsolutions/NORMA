
START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA PersonCountryDemo DEFAULT CHARACTER SET UTF8;

SET SCHEMA 'PERSONCOUNTRYDEMO';

CREATE DOMAIN PersonCountryDemo.Title AS CHARACTER VARYING(4) CONSTRAINT ValueTypeValueConstraint1 CHECK (VALUE IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms'));

CREATE TABLE PersonCountryDemo.Person
(
	person_Id INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	lastName CHARACTER VARYING(30) NOT NULL,
	firstName CHARACTER VARYING(30) NOT NULL,
	title PersonCountryDemo.Title,
	country_Name CHARACTER VARYING(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(person_Id)
);

CREATE TABLE PersonCountryDemo.Country
(
	country_Name CHARACTER VARYING(20) NOT NULL,
	region_Code CHARACTER(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(country_Name)
);

ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_FK FOREIGN KEY (country_Name) REFERENCES PersonCountryDemo.Country (country_Name) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;

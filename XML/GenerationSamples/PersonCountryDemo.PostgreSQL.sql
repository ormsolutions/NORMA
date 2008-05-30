
START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA PersonCountryDemo;

SET search_path TO PERSONCOUNTRYDEMO,"$user",public;

CREATE DOMAIN PersonCountryDemo.Title AS CHARACTER VARYING(4) CONSTRAINT ValueTypeValueConstraint1 CHECK (VALUE IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms'));

CREATE TABLE PersonCountryDemo.Person
(
	personId SERIAL NOT NULL,
	lastName CHARACTER VARYING(30) NOT NULL,
	firstName CHARACTER VARYING(30) NOT NULL,
	title PersonCountryDemo.Title,
	countryName CHARACTER VARYING(20),
	CONSTRAINT Person_PK PRIMARY KEY(personId)
);

CREATE TABLE PersonCountryDemo.Country
(
	countryName CHARACTER VARYING(20) NOT NULL,
	regionCode CHARACTER(8),
	CONSTRAINT Country_PK PRIMARY KEY(countryName)
);

ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName) REFERENCES PersonCountryDemo.Country (countryName) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;

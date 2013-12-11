
CREATE SCHEMA PersonCountryDemo;

SET SCHEMA 'PERSONCOUNTRYDEMO';

CREATE TABLE PersonCountryDemo.Person
(
	personId INTEGER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL,
	firstName CHARACTER VARYING(30) NOT NULL,
	lastName CHARACTER VARYING(30) NOT NULL,
	countryName CHARACTER VARYING(20),
	title CHARACTER VARYING(4) CHECK (title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')),
	CONSTRAINT Person_PK PRIMARY KEY(personId)
);

CREATE TABLE PersonCountryDemo.Country
(
	countryName CHARACTER VARYING(20) NOT NULL,
	regionCode CHARACTER(8),
	CONSTRAINT Country_PK PRIMARY KEY(countryName)
);

ALTER TABLE PersonCountryDemo.Person ADD CONSTRAINT Person_FK FOREIGN KEY (countryName) REFERENCES PersonCountryDemo.Country (countryName) ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE Person
(
	Person_id NUMBER NOT NULL,
	LastName NVARCHAR2(30) NOT NULL,
	FirstName NVARCHAR2(30) NOT NULL,
	Title NVARCHAR2(4) CHECK (Title IN ('Dr', 'Prof', 'Mr', 'Mrs', 'Miss', 'Ms')),
	Country_name NVARCHAR2(20),
	CONSTRAINT InternalUniquenessConstraint1 PRIMARY KEY(Person_id)
);

CREATE TABLE Country
(
	Country_name NVARCHAR2(20) NOT NULL,
	Region_code NCHAR(8),
	CONSTRAINT InternalUniquenessConstraint3 PRIMARY KEY(Country_name)
);

ALTER TABLE Person ADD CONSTRAINT Person_Person_FK FOREIGN KEY (Country_name)  REFERENCES Country (Country_name) ;

COMMIT WORK;


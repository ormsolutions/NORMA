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

COMMIT WORK;


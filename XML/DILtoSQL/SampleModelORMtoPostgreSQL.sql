START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel;

CREATE DOMAIN SampleModel.SclScrtyNmbr AS CHARACTER(11) CONSTRAINT SclScrtyNmbr_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 11) ;

CREATE DOMAIN SampleModel."Integer" AS BIGINT CONSTRAINT Integer_Chk CHECK (VALUE BETWEEN 1 AND 7) ;

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT DthCs_Typ_Chk CHECK (VALUE IN ('natural', 'not so natural')) ;

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 1 AND VALUE IN ('M', 'F')) ;

CREATE TABLE SampleModel.PersonDrivesCar
(
	DrivesCar_vin BIGINT NOT NULL, 
	DrvnByPrsn_Prsn_d BIGINT NOT NULL, 
	CONSTRAINT IUC18 PRIMARY KEY(DrivesCar_vin, DrvnByPrsn_Prsn_d)
);

CREATE TABLE SampleModel.PBCFPOD
(
	CarSold_vin BIGINT NOT NULL, 
	SaleDate_YMD BIGINT NOT NULL, 
	Buyer_Person_id BIGINT NOT NULL, 
	Seller_Person_id BIGINT NOT NULL, 
	CONSTRAINT IUC23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id), 
	CONSTRAINT IUC24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin), 
	CONSTRAINT IUC25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
);

CREATE TABLE SampleModel.Review
(
	Car_vin BIGINT NOT NULL, 
	Rating_Nr_Integer SampleModel."Integer" NOT NULL, 
	Criteria_Name CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT IUC26 PRIMARY KEY(Car_vin, Criteria_Name)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	NickName CHARACTER VARYING(64) NOT NULL, 
	Person_Person_id BIGINT NOT NULL, 
	CONSTRAINT IUC33 PRIMARY KEY(NickName, Person_Person_id)
);

CREATE TABLE SampleModel.Person
(
	FirstName CHARACTER VARYING(64) NOT NULL, 
	Person_id BIGSERIAL NOT NULL, 
	Date_YMD BIGINT NOT NULL, 
	LastName CHARACTER VARYING(64) NOT NULL, 
	SclScrtyNmbr SampleModel.SclScrtyNmbr , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code SampleModel.Gender_Code NOT NULL, 
	PersonHasParents BOOLEAN, 
	VlTyp1_VlTyp1Vl BIGINT , 
	CPBOBON BIGINT , 
	Father_Person_id BIGINT NOT NULL, 
	Mother_Person_id BIGINT NOT NULL, 
	Death_Date_YMD BIGINT , 
	DDCDCT SampleModel.DeathCause_Type , 
	DNDNDIFPC BOOLEAN, 
	DUDUDIV BOOLEAN, 
	DUDUDIB BOOLEAN, 
	CONSTRAINT IUC2 PRIMARY KEY(Person_id), 
	CONSTRAINT IUC9 UNIQUE(SclScrtyNmbr), 
	CONSTRAINT IUC22 UNIQUE(OwnsCar_vin), 
	CONSTRAINT CPEUC3 PRIMARY KEY(Father_Person_id, CPBOBON, Mother_Person_id), 
	CONSTRAINT EUC1 UNIQUE(FirstName, Date_YMD), 
	CONSTRAINT EUC2 UNIQUE(LastName, Date_YMD)
);

CREATE TABLE SampleModel.Task
(
	Task_id BIGSERIAL NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT IUC16 PRIMARY KEY(Task_id)
);

CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value BIGINT NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT VlTyp1Vl_Unq PRIMARY KEY(ValueType1Value)
);

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT DrivenByPerson_FK FOREIGN KEY (DrvnByPrsn_Prsn_d)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PBCFPOD ADD CONSTRAINT Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PBCFPOD ADD CONSTRAINT Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT ValueType1_FK FOREIGN KEY (VlTyp1_VlTyp1Vl)  REFERENCES SampleModel.ValueType1 (ValueType1Value)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

COMMIT WORK;


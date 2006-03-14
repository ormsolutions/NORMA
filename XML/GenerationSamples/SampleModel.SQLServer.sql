BEGIN TRANSACTION;
GO

CREATE SCHEMA SampleModel;
GO

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
	Rating_Nr_Integer BIGINT CONSTRAINT Integer_Chk CHECK (Rating_Nr_Integer BETWEEN 1 AND 7) NOT NULL, 
	Criterion_Name NATIONAL CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT IUC26 PRIMARY KEY(Car_vin, Criterion_Name)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	NickName NATIONAL CHARACTER VARYING(64) NOT NULL, 
	Person_Person_id BIGINT NOT NULL, 
	CONSTRAINT IUC33 PRIMARY KEY(NickName, Person_Person_id)
);

CREATE TABLE SampleModel.Person
(
	FirstName NATIONAL CHARACTER VARYING(64) NOT NULL, 
	Person_id BIGINT IDENTITY (1, 1) NOT NULL, 
	Date_YMD BIGINT NOT NULL, 
	LastName NATIONAL CHARACTER VARYING(64) NOT NULL, 
	SclScrtyNmbr NATIONAL CHARACTER(11) CONSTRAINT SclScrtyNmbr_Chk CHECK ((LEN(LTRIM(RTRIM(SclScrtyNmbr)))) >= 11) , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD NATIONAL CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code NATIONAL CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LEN(LTRIM(RTRIM(Gender_Gender_Code)))) >= 1 AND Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	PersonHasParents BOOLEAN, 
	VT1DSEWVT1V BIGINT , 
	CPBOBON BIGINT , 
	Father_Person_id BIGINT NOT NULL, 
	Mother_Person_id BIGINT NOT NULL, 
	Death_Date_YMD BIGINT , 
	DDCDCT NATIONAL CHARACTER VARYING(14) CONSTRAINT DthCs_Typ_Chk CHECK (DDCDCT IN ('natural', 'not so natural')) , 
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
	Task_id BIGINT IDENTITY (1, 1) NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT IUC16 PRIMARY KEY(Task_id)
);

CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value BIGINT NOT NULL, 
	DSWPP BIGINT , 
	CONSTRAINT VlTyp1Vl_Unq PRIMARY KEY(ValueType1Value)
);

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT PersonDrivesCar_DrivenByPerson_FK FOREIGN KEY (DrvnByPrsn_Prsn_d)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.PBCFPOD ADD CONSTRAINT PBCFPOD_Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.PBCFPOD ADD CONSTRAINT PBCFPOD_Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT PersonHasNickName_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_VT1DSEWFK FOREIGN KEY (VT1DSEWVT1V)  REFERENCES SampleModel.ValueType1 (ValueType1Value)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_DsSmthngWthPrsn_FK FOREIGN KEY (DSWPP)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION;

COMMIT WORK;


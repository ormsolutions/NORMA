SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE PersonDrivesCar
(
	DrivesCar_vin NUMBER(38) NOT NULL, 
	DrvnByPrsn_Prsn_d NUMBER(38) NOT NULL, 
	CONSTRAINT IUC18 PRIMARY KEY(DrivesCar_vin, DrvnByPrsn_Prsn_d)
);

CREATE TABLE PBCFPOD
(
	CarSold_vin NUMBER(38) NOT NULL, 
	SaleDate_YMD NUMBER(38) NOT NULL, 
	Buyer_Person_id NUMBER(38) NOT NULL, 
	Seller_Person_id NUMBER(38) NOT NULL, 
	CONSTRAINT IUC23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id), 
	CONSTRAINT IUC24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin), 
	CONSTRAINT IUC25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
);

CREATE TABLE Review
(
	Car_vin NUMBER(38) NOT NULL, 
	Rating_Nr_Integer NUMBER(38) CONSTRAINT Integer_Chk CHECK (Rating_Nr_Integer BETWEEN 1 AND 7) NOT NULL, 
	Criterion_Name CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT IUC26 PRIMARY KEY(Car_vin, Criterion_Name)
);

CREATE TABLE PersonHasNickName
(
	NickName CHARACTER VARYING(64) NOT NULL, 
	Person_Person_id NUMBER(38) NOT NULL, 
	CONSTRAINT IUC33 PRIMARY KEY(NickName, Person_Person_id)
);

CREATE TABLE Person
(
	FirstName CHARACTER VARYING(64) NOT NULL, 
	Person_id NUMBER(38) NOT NULL, 
	Date_YMD NUMBER(38) NOT NULL, 
	LastName CHARACTER VARYING(64) NOT NULL, 
	OptnlUnqStrng CHARACTER(11) CONSTRAINT OptnlUnqStrng_Chk CHECK ((LENGTH(TRIM(BOTH FROM OptnlUnqStrng))) >= 11) , 
	HatType_ColorARGB NUMBER(38) , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin NUMBER(38) , 
	Gender_Gender_Code CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LENGTH(TRIM(BOTH FROM Gender_Gender_Code))) >= 1 AND Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	hasParents NCHARNOT NULL, 
	OptnlUnqDcml NUMBER(38) , 
	MndtryUnqDcml NUMBER(38) NOT NULL, 
	MndtryUnqStrng CHARACTER(11) CONSTRAINT MndtryUnqStrng_Chk CHECK ((LENGTH(TRIM(BOTH FROM MndtryUnqStrng))) >= 11) NOT NULL, 
	VT1DSEWVT1V NUMBER(38) , 
	CPBOBON NUMBER(38) , 
	Father_Person_id NUMBER(38) NOT NULL, 
	Mother_Person_id NUMBER(38) NOT NULL, 
	Death_Date_YMD NUMBER(38) , 
	DDCDCT CHARACTER VARYING(14) CONSTRAINT DthCs_Typ_Chk CHECK (DDCDCT IN ('natural', 'not so natural')) , 
	DNDFPC NCHAR, 
	DUDV NCHAR, 
	DUDB NCHAR, 
	CONSTRAINT IUC2 PRIMARY KEY(Person_id), 
	CONSTRAINT IUC9 UNIQUE(OptnlUnqStrng), 
	CONSTRAINT IUC22 UNIQUE(OwnsCar_vin), 
	CONSTRAINT IUC65 UNIQUE(OptnlUnqDcml), 
	CONSTRAINT IUC69 UNIQUE(MndtryUnqDcml), 
	CONSTRAINT IUC67 UNIQUE(MndtryUnqStrng), 
	CONSTRAINT CPIUC49 UNIQUE(Father_Person_id, CPBOBON, Mother_Person_id), 
	CONSTRAINT EUC1 UNIQUE(FirstName, Date_YMD), 
	CONSTRAINT EUC2 UNIQUE(LastName, Date_YMD)
);

CREATE TABLE Task
(
	Task_id NUMBER(38) NOT NULL, 
	Person_Person_id NUMBER(38) , 
	CONSTRAINT IUC16 PRIMARY KEY(Task_id)
);

CREATE TABLE ValueType1
(
	ValueType1Value NUMBER(38) NOT NULL, 
	DSWPP NUMBER(38) , 
	CONSTRAINT VlTyp1Vl_Unq PRIMARY KEY(ValueType1Value)
);

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PDCDBPFK FOREIGN KEY (DrvnByPrsn_Prsn_d)  REFERENCES Person (Person_id) ;

ALTER TABLE PBCFPOD ADD CONSTRAINT PBCFPOD_Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PBCFPOD ADD CONSTRAINT PBCFPOD_Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PHNNPFK FOREIGN KEY (Person_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_VT1DSEWFK FOREIGN KEY (VT1DSEWVT1V)  REFERENCES ValueType1 (ValueType1Value) ;

ALTER TABLE Person ADD CONSTRAINT Person_Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Task ADD CONSTRAINT Task_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE ValueType1 ADD CONSTRAINT VT1DSWPFK FOREIGN KEY (DSWPP)  REFERENCES Person (Person_id) ;

COMMIT WORK;


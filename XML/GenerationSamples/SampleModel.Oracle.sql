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
	Gender_Gender_Code CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LENGTH(TRIM(BOTH FROM Gender_Gender_Code))) >= 1 AND 
Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	hasParents NCHAR NOT NULL, 
	OptnlUnqDcml NUMBER(38) , 
	MndtryUnqDcml NUMBER(38) NOT NULL, 
	MndtryUnqStrng CHARACTER(11) CONSTRAINT MndtryUnqStrng_Chk CHECK ((LENGTH(TRIM(BOTH FROM MndtryUnqStrng))) >= 11) NOT NULL, 
	Husband_Person_id NUMBER(38) , 
	VT1DSEWVT1V NUMBER(38) , 
	CPBOBON NUMBER(38) , 
	Father_Person_id NUMBER(38) NOT NULL, 
	Mother_Person_id NUMBER(38) NOT NULL, 
	Death_Date_YMD NUMBER(38) , 
	DDCDCT CHARACTER VARYING(14) CONSTRAINT DthCs_Typ_Chk CHECK (DDCDCT IN ('natural', 'not so natural')) , 
	DNDFPC NCHAR , 
	DUDV NCHAR , 
	DUDB NCHAR , 
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

ALTER TABLE Person ADD CONSTRAINT Person_Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_VT1DSEWFK FOREIGN KEY (VT1DSEWVT1V)  REFERENCES ValueType1 (ValueType1Value) ;

ALTER TABLE Person ADD CONSTRAINT Person_Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Task ADD CONSTRAINT Task_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE ValueType1 ADD CONSTRAINT VT1DSWPFK FOREIGN KEY (DSWPP)  REFERENCES Person (Person_id) ;


CREATE PROCEDURE SampleModel.InsrtPrsnDrvsCr
(
	DrivesCar_vin NUMBER(38) , 
	DrvnByPrsn_Prsn_d NUMBER(38) 
)
AS
	INSERT INTO SampleModel.PersonDrivesCar(DrivesCar_vin, DrvnByPrsn_Prsn_d)
	VALUES (DrivesCar_vin, DrvnByPrsn_Prsn_d);

CREATE PROCEDURE SampleModel.DltPrsnDrvsCr
(
	DrivesCar_vin NUMBER(38) , 
	DrvnByPrsn_Prsn_d NUMBER(38) 
)
AS
	DELETE FROM SampleModel.PersonDrivesCar
	WHERE DrivesCar_vin = DrivesCar_vin AND 
DrvnByPrsn_Prsn_d = DrvnByPrsn_Prsn_d;

CREATE PROCEDURE SampleModel.IPBCFPOD
(
	CarSold_vin NUMBER(38) , 
	SaleDate_YMD NUMBER(38) , 
	Buyer_Person_id NUMBER(38) , 
	Seller_Person_id NUMBER(38) 
)
AS
	INSERT INTO SampleModel.PBCFPOD(CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id)
	VALUES (CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id);

CREATE PROCEDURE SampleModel.DPBCFPOD
(
	Buyer_Person_id NUMBER(38) , 
	CarSold_vin NUMBER(38) , 
	Seller_Person_id NUMBER(38) 
)
AS
	DELETE FROM SampleModel.PBCFPOD
	WHERE Buyer_Person_id = Buyer_Person_id AND 
CarSold_vin = CarSold_vin AND 
Seller_Person_id = Seller_Person_id;

CREATE PROCEDURE SampleModel.InsertReview
(
	Car_vin NUMBER(38) , 
	Rating_Nr_Integer NUMBER(38) , 
	Criterion_Name CHARACTER VARYING(64) 
)
AS
	INSERT INTO SampleModel.Review(Car_vin, Rating_Nr_Integer, Criterion_Name)
	VALUES (Car_vin, Rating_Nr_Integer, Criterion_Name);

CREATE PROCEDURE SampleModel.DeleteReview
(
	Car_vin NUMBER(38) , 
	Criterion_Name CHARACTER VARYING(64) 
)
AS
	DELETE FROM SampleModel.Review
	WHERE Car_vin = Car_vin AND 
Criterion_Name = Criterion_Name;

CREATE PROCEDURE SampleModel.InsrtPrsnHsNckNm
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id NUMBER(38) 
)
AS
	INSERT INTO SampleModel.PersonHasNickName(NickName, Person_Person_id)
	VALUES (NickName, Person_Person_id);

CREATE PROCEDURE SampleModel.DltPrsnHsNckNm
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id NUMBER(38) 
)
AS
	DELETE FROM SampleModel.PersonHasNickName
	WHERE NickName = NickName AND 
Person_Person_id = Person_Person_id;

CREATE PROCEDURE SampleModel.InsertPerson
(
	FirstName CHARACTER VARYING(64) , 
	Person_id NUMBER(38) , 
	Date_YMD NUMBER(38) , 
	LastName CHARACTER VARYING(64) , 
	OptnlUnqStrng CHARACTER(11) , 
	HatType_ColorARGB NUMBER(38) , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin NUMBER(38) , 
	Gender_Gender_Code CHARACTER(1) , 
	hasParents NCHAR , 
	OptnlUnqDcml NUMBER(38) , 
	MndtryUnqDcml NUMBER(38) , 
	MndtryUnqStrng CHARACTER(11) , 
	Husband_Person_id NUMBER(38) , 
	VT1DSEWVT1V NUMBER(38) , 
	CPBOBON NUMBER(38) , 
	Father_Person_id NUMBER(38) , 
	Mother_Person_id NUMBER(38) , 
	Death_Date_YMD NUMBER(38) , 
	DDCDCT CHARACTER VARYING(14) , 
	DNDFPC NCHAR , 
	DUDV NCHAR , 
	DUDB NCHAR 
)
AS
	INSERT INTO SampleModel.Person(FirstName, Person_id, Date_YMD, LastName, OptnlUnqStrng, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptnlUnqDcml, MndtryUnqDcml, MndtryUnqStrng, Husband_Person_id, VT1DSEWVT1V, CPBOBON, Father_Person_id, Mother_Person_id, Death_Date_YMD, DDCDCT, DNDFPC, DUDV, DUDB)
	VALUES (FirstName, Person_id, Date_YMD, LastName, OptnlUnqStrng, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptnlUnqDcml, MndtryUnqDcml, MndtryUnqStrng, Husband_Person_id, VT1DSEWVT1V, CPBOBON, Father_Person_id, Mother_Person_id, Death_Date_YMD, DDCDCT, DNDFPC, DUDV, DUDB);

CREATE PROCEDURE SampleModel.DeletePerson
(
	Person_id NUMBER(38) 
)
AS
	DELETE FROM SampleModel.Person
	WHERE Person_id = Person_id;

CREATE PROCEDURE SampleModel.InsertTask
(
	Task_id NUMBER(38) , 
	Person_Person_id NUMBER(38) 
)
AS
	INSERT INTO SampleModel.Task(Task_id, Person_Person_id)
	VALUES (Task_id, Person_Person_id);

CREATE PROCEDURE SampleModel.DeleteTask
(
	Task_id NUMBER(38) 
)
AS
	DELETE FROM SampleModel.Task
	WHERE Task_id = Task_id;

CREATE PROCEDURE SampleModel.InsertValueType1
(
	ValueType1Value NUMBER(38) , 
	DSWPP NUMBER(38) 
)
AS
	INSERT INTO SampleModel.ValueType1(ValueType1Value, DSWPP)
	VALUES (ValueType1Value, DSWPP);

CREATE PROCEDURE SampleModel.DeleteValueType1
(
	ValueType1Value NUMBER(38) 
)
AS
	DELETE FROM SampleModel.ValueType1
	WHERE ValueType1Value = ValueType1Value;
COMMIT WORK;


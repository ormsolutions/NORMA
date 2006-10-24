CREATE SCHEMA SampleModel;

SET SCHEMA 'SAMPLEMODEL';

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
	CONSTRAINT IUC23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id)
	CONSTRAINT IUC24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin)
	CONSTRAINT IUC25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
);

CREATE TABLE SampleModel.Review
(
	Car_vin BIGINT NOT NULL, 
	Rating_Nr_Integer BIGINT CONSTRAINT Integer_Chk CHECK (Rating_Nr_Integer BETWEEN 1 AND 7) NOT NULL, 
	Criterion_Name CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT IUC26 PRIMARY KEY(Car_vin, Criterion_Name)
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
	Person_id BIGINT GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL, 
	Date_YMD BIGINT NOT NULL, 
	LastName CHARACTER VARYING(64) NOT NULL, 
	OptnlUnqStrng CHARACTER(11) CONSTRAINT OptnlUnqStrng_Chk CHECK ((LENGTH(LTRIM(RTRIM(OptnlUnqStrng)))) >= 11) , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LENGTH(LTRIM(RTRIM(Gender_Gender_Code)))) >= 1 AND 
Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	hasParents CHARACTER(1) FOR BIT DATA NOT NULL, 
	OptnlUnqDcml DECIMAL(9) , 
	MndtryUnqDcml DECIMAL(9) NOT NULL, 
	MndtryUnqStrng CHARACTER(11) CONSTRAINT MndtryUnqStrng_Chk CHECK ((LENGTH(LTRIM(RTRIM(MndtryUnqStrng)))) >= 11) NOT NULL, 
	Husband_Person_id BIGINT , 
	VT1DSEWVT1V BIGINT , 
	CPBOBON BIGINT , 
	Father_Person_id BIGINT NOT NULL, 
	Mother_Person_id BIGINT NOT NULL, 
	Death_Date_YMD BIGINT , 
	DDCDCT CHARACTER VARYING(14) CONSTRAINT DthCs_Typ_Chk CHECK (DDCDCT IN ('natural', 'not so natural')) , 
	DNDFPC CHARACTER(1) FOR BIT DATA , 
	DUDV CHARACTER(1) FOR BIT DATA , 
	DUDB CHARACTER(1) FOR BIT DATA , 
	CONSTRAINT IUC2 PRIMARY KEY(Person_id)
	
	
	
	CONSTRAINT IUC69 UNIQUE(MndtryUnqDcml)
	CONSTRAINT IUC67 UNIQUE(MndtryUnqStrng)
	
	CONSTRAINT EUC1 UNIQUE(FirstName, Date_YMD)
	CONSTRAINT EUC2 UNIQUE(LastName, Date_YMD)
);

CREATE TABLE SampleModel.Task
(
	Task_id BIGINT GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT IUC16 PRIMARY KEY(Task_id)
);

CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value BIGINT NOT NULL, 
	DSWPP BIGINT , 
	CONSTRAINT VlTyp1Vl_Unq PRIMARY KEY(ValueType1Value)
);

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT DrivenByPerson_FK FOREIGN KEY (DrvnByPrsn_Prsn_d)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PBCFPOD ADD CONSTRAINT Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PBCFPOD ADD CONSTRAINT Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT VT1DSEWFK FOREIGN KEY (VT1DSEWVT1V)  REFERENCES SampleModel.ValueType1 (ValueType1Value)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT DsSmthngWthPrsn_FK FOREIGN KEY (DSWPP)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE PROCEDURE SampleModel.InsrtPrsnDrvsCr
(
	DrivesCar_vin BIGINT , 
	DrvnByPrsn_Prsn_d BIGINT 
)
AS
	INSERT INTO SampleModel.PersonDrivesCar(DrivesCar_vin, DrvnByPrsn_Prsn_d)
	VALUES (DrivesCar_vin, DrvnByPrsn_Prsn_d);

CREATE PROCEDURE SampleModel.DltPrsnDrvsCr
(
	DrivesCar_vin BIGINT , 
	DrvnByPrsn_Prsn_d BIGINT 
)
AS
	DELETE FROM SampleModel.PersonDrivesCar
	WHERE DrivesCar_vin = DrivesCar_vin AND 
DrvnByPrsn_Prsn_d = DrvnByPrsn_Prsn_d;

CREATE PROCEDURE SampleModel.IPBCFPOD
(
	CarSold_vin BIGINT , 
	SaleDate_YMD BIGINT , 
	Buyer_Person_id BIGINT , 
	Seller_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.PBCFPOD(CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id)
	VALUES (CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id);

CREATE PROCEDURE SampleModel.DPBCFPOD
(
	Buyer_Person_id BIGINT , 
	CarSold_vin BIGINT , 
	Seller_Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.PBCFPOD
	WHERE Buyer_Person_id = Buyer_Person_id AND 
CarSold_vin = CarSold_vin AND 
Seller_Person_id = Seller_Person_id;

CREATE PROCEDURE SampleModel.InsertReview
(
	Car_vin BIGINT , 
	Rating_Nr_Integer BIGINT , 
	Criterion_Name CHARACTER VARYING(64) 
)
AS
	INSERT INTO SampleModel.Review(Car_vin, Rating_Nr_Integer, Criterion_Name)
	VALUES (Car_vin, Rating_Nr_Integer, Criterion_Name);

CREATE PROCEDURE SampleModel.DeleteReview
(
	Car_vin BIGINT , 
	Criterion_Name CHARACTER VARYING(64) 
)
AS
	DELETE FROM SampleModel.Review
	WHERE Car_vin = Car_vin AND 
Criterion_Name = Criterion_Name;

CREATE PROCEDURE SampleModel.InsrtPrsnHsNckNm
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.PersonHasNickName(NickName, Person_Person_id)
	VALUES (NickName, Person_Person_id);

CREATE PROCEDURE SampleModel.DltPrsnHsNckNm
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.PersonHasNickName
	WHERE NickName = NickName AND 
Person_Person_id = Person_Person_id;

CREATE PROCEDURE SampleModel.InsertPerson
(
	FirstName CHARACTER VARYING(64) , 
	Person_id BIGINT , 
	Date_YMD BIGINT , 
	LastName CHARACTER VARYING(64) , 
	OptnlUnqStrng CHARACTER(11) , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code CHARACTER(1) , 
	hasParents CHARACTER(1) FOR BIT DATA , 
	OptnlUnqDcml DECIMAL(9) , 
	MndtryUnqDcml DECIMAL(9) , 
	MndtryUnqStrng CHARACTER(11) , 
	Husband_Person_id BIGINT , 
	VT1DSEWVT1V BIGINT , 
	CPBOBON BIGINT , 
	Father_Person_id BIGINT , 
	Mother_Person_id BIGINT , 
	Death_Date_YMD BIGINT , 
	DDCDCT CHARACTER VARYING(14) , 
	DNDFPC CHARACTER(1) FOR BIT DATA , 
	DUDV CHARACTER(1) FOR BIT DATA , 
	DUDB CHARACTER(1) FOR BIT DATA 
)
AS
	INSERT INTO SampleModel.Person(FirstName, Person_id, Date_YMD, LastName, OptnlUnqStrng, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptnlUnqDcml, MndtryUnqDcml, MndtryUnqStrng, Husband_Person_id, VT1DSEWVT1V, CPBOBON, Father_Person_id, Mother_Person_id, Death_Date_YMD, DDCDCT, DNDFPC, DUDV, DUDB)
	VALUES (FirstName, Person_id, Date_YMD, LastName, OptnlUnqStrng, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptnlUnqDcml, MndtryUnqDcml, MndtryUnqStrng, Husband_Person_id, VT1DSEWVT1V, CPBOBON, Father_Person_id, Mother_Person_id, Death_Date_YMD, DDCDCT, DNDFPC, DUDV, DUDB);

CREATE PROCEDURE SampleModel.DeletePerson
(
	Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.Person
	WHERE Person_id = Person_id;

CREATE PROCEDURE SampleModel.InsertTask
(
	Task_id BIGINT , 
	Person_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.Task(Task_id, Person_Person_id)
	VALUES (Task_id, Person_Person_id);

CREATE PROCEDURE SampleModel.DeleteTask
(
	Task_id BIGINT 
)
AS
	DELETE FROM SampleModel.Task
	WHERE Task_id = Task_id;

CREATE PROCEDURE SampleModel.InsertValueType1
(
	ValueType1Value BIGINT , 
	DSWPP BIGINT 
)
AS
	INSERT INTO SampleModel.ValueType1(ValueType1Value, DSWPP)
	VALUES (ValueType1Value, DSWPP);

CREATE PROCEDURE SampleModel.DeleteValueType1
(
	ValueType1Value BIGINT 
)
AS
	DELETE FROM SampleModel.ValueType1
	WHERE ValueType1Value = ValueType1Value;
COMMIT;


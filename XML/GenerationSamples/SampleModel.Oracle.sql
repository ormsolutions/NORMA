SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

CREATE TABLE PersonDrivesCar
(
	DrivesCar_vin NUMBER(38) NOT NULL, 
	DrivenByPerson_Person_id NUMBER(38) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(DrivesCar_vin, DrivenByPerson_Person_id)
);

CREATE TABLE PrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin NUMBER(38) NOT NULL, 
	SaleDate_YMD NUMBER(38) NOT NULL, 
	Buyer_Person_id NUMBER(38) NOT NULL, 
	Seller_Person_id NUMBER(38) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id), 
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin), 
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
);

CREATE TABLE Review
(
	Car_vin NUMBER(38) NOT NULL, 
	Rating_Nr_Integer NUMBER(38) CONSTRAINT Integer_Chk CHECK (Rating_Nr_Integer BETWEEN 1 AND 7) NOT NULL, 
	Criterion_Name CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Car_vin, Criterion_Name)
);

CREATE TABLE PersonHasNickName
(
	NickName CHARACTER VARYING(64) NOT NULL, 
	Person_Person_id NUMBER(38) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_Person_id)
);

CREATE TABLE Person
(
	FirstName CHARACTER VARYING(64) NOT NULL, 
	Person_id NUMBER(38) NOT NULL, 
	Date_YMD NUMBER(38) NOT NULL, 
	LastName CHARACTER VARYING(64) NOT NULL, 
	OptionalUniqueString CHARACTER(11) CONSTRAINT OptionalUniqueString_Chk CHECK ((LENGTH(TRIM(BOTH FROM OptionalUniqueString))) >= 11) , 
	HatType_ColorARGB NUMBER(38) , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin NUMBER(38) , 
	Gender_Gender_Code CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LENGTH(TRIM(BOTH FROM Gender_Gender_Code))) >= 1 AND 
Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	hasParents NCHAR NOT NULL, 
	OptionalUniqueDecimal NUMBER(38) , 
	MandatoryUniqueDecimal NUMBER(38) NOT NULL, 
	MandatoryUniqueString CHARACTER(11) CONSTRAINT MandatoryUniqueString_Chk CHECK ((LENGTH(TRIM(BOTH FROM MandatoryUniqueString))) >= 11) NOT NULL, 
	Husband_Person_id NUMBER(38) , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl NUMBER(38) , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr NUMBER(38) , 
	Father_Person_id NUMBER(38) NOT NULL, 
	Mother_Person_id NUMBER(38) NOT NULL, 
	Death_Date_YMD NUMBER(38) , 
	Dth_DthCs_DthCs_Typ CHARACTER VARYING(14) CONSTRAINT DeathCause_Type_Chk CHECK (Dth_DthCs_DthCs_Typ IN ('natural', 'not so natural')) , 
	Dth_NtrlDth_sFrmPrsttCncr NCHAR , 
	Death_UnnaturalDeath_isViolent NCHAR , 
	Death_UnnaturalDeath_isBloody NCHAR , 
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(Person_id), 
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(OptionalUniqueString), 
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(OwnsCar_vin), 
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(OptionalUniqueDecimal), 
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(MandatoryUniqueDecimal), 
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(MandatoryUniqueString), 
	CONSTRAINT ChldPrsn_IntrnlUnqnssCnstrnt49 UNIQUE(Father_Person_id, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Mother_Person_id), 
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(FirstName, Date_YMD), 
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE(LastName, Date_YMD)
);

CREATE TABLE Task
(
	Task_id NUMBER(38) NOT NULL, 
	Person_Person_id NUMBER(38) , 
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
);

CREATE TABLE ValueType1
(
	ValueType1Value NUMBER(38) NOT NULL, 
	DsSmthngWthPrsn_Prsn_d NUMBER(38) , 
	CONSTRAINT ValueType1Value_Unique PRIMARY KEY(ValueType1Value)
);

ALTER TABLE PersonDrivesCar ADD CONSTRAINT PrsnDrvsCr_DrvnByPrsn_FK FOREIGN KEY (DrivenByPerson_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT PrsnBghtCrFrmPrsnOnDt_Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT PrsnBghtCrFrmPrsnOnDt_Sllr_FK FOREIGN KEY (Seller_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PersonHasNickName ADD CONSTRAINT PersonHasNickName_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_VlTyp1DsSmthngElsWth_FK FOREIGN KEY (VlTyp1DsSmthngElsWth_VlTyp1Vl)  REFERENCES ValueType1 (ValueType1Value) ;

ALTER TABLE Person ADD CONSTRAINT Person_Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Task ADD CONSTRAINT Task_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE ValueType1 ADD CONSTRAINT VlTyp1_DsSmthngWthPrsn_FK FOREIGN KEY (DsSmthngWthPrsn_Prsn_d)  REFERENCES Person (Person_id) ;


CREATE PROCEDURE SampleModel.InsertPersonDrivesCar
(
	DrivesCar_vin NUMBER(38) , 
	DrivenByPerson_Person_id NUMBER(38) 
)
AS
	INSERT INTO SampleModel.PersonDrivesCar(DrivesCar_vin, DrivenByPerson_Person_id)
	VALUES (DrivesCar_vin, DrivenByPerson_Person_id);

CREATE PROCEDURE SampleModel.DeletePersonDrivesCar
(
	DrivesCar_vin NUMBER(38) , 
	DrivenByPerson_Person_id NUMBER(38) 
)
AS
	DELETE FROM SampleModel.PersonDrivesCar
	WHERE DrivesCar_vin = DrivesCar_vin AND 
DrivenByPerson_Person_id = DrivenByPerson_Person_id;

CREATE PROCEDURE SampleModel.InsrtPrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin NUMBER(38) , 
	SaleDate_YMD NUMBER(38) , 
	Buyer_Person_id NUMBER(38) , 
	Seller_Person_id NUMBER(38) 
)
AS
	INSERT INTO SampleModel.PrsnBghtCrFrmPrsnOnDt(CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id)
	VALUES (CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id);

CREATE PROCEDURE SampleModel.DltPrsnBghtCrFrmPrsnOnDt
(
	Buyer_Person_id NUMBER(38) , 
	CarSold_vin NUMBER(38) , 
	Seller_Person_id NUMBER(38) 
)
AS
	DELETE FROM SampleModel.PrsnBghtCrFrmPrsnOnDt
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

CREATE PROCEDURE SampleModel.InsertPersonHasNickName
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id NUMBER(38) 
)
AS
	INSERT INTO SampleModel.PersonHasNickName(NickName, Person_Person_id)
	VALUES (NickName, Person_Person_id);

CREATE PROCEDURE SampleModel.DeletePersonHasNickName
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
	OptionalUniqueString CHARACTER(11) , 
	HatType_ColorARGB NUMBER(38) , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin NUMBER(38) , 
	Gender_Gender_Code CHARACTER(1) , 
	hasParents NCHAR , 
	OptionalUniqueDecimal NUMBER(38) , 
	MandatoryUniqueDecimal NUMBER(38) , 
	MandatoryUniqueString CHARACTER(11) , 
	Husband_Person_id NUMBER(38) , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl NUMBER(38) , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr NUMBER(38) , 
	Father_Person_id NUMBER(38) , 
	Mother_Person_id NUMBER(38) , 
	Death_Date_YMD NUMBER(38) , 
	Dth_DthCs_DthCs_Typ CHARACTER VARYING(14) , 
	Dth_NtrlDth_sFrmPrsttCncr NCHAR , 
	Death_UnnaturalDeath_isViolent NCHAR , 
	Death_UnnaturalDeath_isBloody NCHAR 
)
AS
	INSERT INTO SampleModel.Person(FirstName, Person_id, Date_YMD, LastName, OptionalUniqueString, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptionalUniqueDecimal, MandatoryUniqueDecimal, MandatoryUniqueString, Husband_Person_id, VlTyp1DsSmthngElsWth_VlTyp1Vl, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Father_Person_id, Mother_Person_id, Death_Date_YMD, Dth_DthCs_DthCs_Typ, Dth_NtrlDth_sFrmPrsttCncr, Death_UnnaturalDeath_isViolent, Death_UnnaturalDeath_isBloody)
	VALUES (FirstName, Person_id, Date_YMD, LastName, OptionalUniqueString, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptionalUniqueDecimal, MandatoryUniqueDecimal, MandatoryUniqueString, Husband_Person_id, VlTyp1DsSmthngElsWth_VlTyp1Vl, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Father_Person_id, Mother_Person_id, Death_Date_YMD, Dth_DthCs_DthCs_Typ, Dth_NtrlDth_sFrmPrsttCncr, Death_UnnaturalDeath_isViolent, Death_UnnaturalDeath_isBloody);

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
	DsSmthngWthPrsn_Prsn_d NUMBER(38) 
)
AS
	INSERT INTO SampleModel.ValueType1(ValueType1Value, DsSmthngWthPrsn_Prsn_d)
	VALUES (ValueType1Value, DsSmthngWthPrsn_Prsn_d);

CREATE PROCEDURE SampleModel.DeleteValueType1
(
	ValueType1Value NUMBER(38) 
)
AS
	DELETE FROM SampleModel.ValueType1
	WHERE ValueType1Value = ValueType1Value;
COMMIT WORK;


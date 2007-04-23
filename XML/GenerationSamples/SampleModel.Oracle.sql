SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

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

ALTER TABLE Person ADD CONSTRAINT Person_Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_VlTyp1DsSmthngElsWth_FK FOREIGN KEY (VlTyp1DsSmthngElsWth_VlTyp1Vl)  REFERENCES ValueType1 (ValueType1Value) ;

ALTER TABLE Person ADD CONSTRAINT Person_Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Person ADD CONSTRAINT Person_Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE Task ADD CONSTRAINT Task_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE ValueType1 ADD CONSTRAINT VlTyp1_DsSmthngWthPrsn_FK FOREIGN KEY (DsSmthngWthPrsn_Prsn_d)  REFERENCES Person (Person_id) ;

ALTER TABLE PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT PrsnBghtCrFrmPrsnOnDt_Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES Person (Person_id) ;

ALTER TABLE PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT PrsnBghtCrFrmPrsnOnDt_Sllr_FK FOREIGN KEY (Seller_Person_id)  REFERENCES Person (Person_id) ;

COMMIT WORK;


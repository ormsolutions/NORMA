CREATE SCHEMA SampleModel;

SET SCHEMA 'SAMPLEMODEL';

CREATE TABLE SampleModel.PersonDrivesCar
(
	DrivesCar_vin BIGINT NOT NULL, 
	DrivenByPerson_Person_id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint18 PRIMARY KEY(DrivesCar_vin, DrivenByPerson_Person_id)
);

CREATE TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin BIGINT NOT NULL, 
	SaleDate_YMD BIGINT NOT NULL, 
	Buyer_Person_id BIGINT NOT NULL, 
	Seller_Person_id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id)
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin)
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
);

CREATE TABLE SampleModel.Review
(
	Car_vin BIGINT NOT NULL, 
	Rating_Nr_Integer BIGINT CONSTRAINT Integer_Chk CHECK (Rating_Nr_Integer BETWEEN 1 AND 7) NOT NULL, 
	Criterion_Name CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Car_vin, Criterion_Name)
);

CREATE TABLE SampleModel.PersonHasNickName
(
	NickName CHARACTER VARYING(64) NOT NULL, 
	Person_Person_id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_Person_id)
);

CREATE TABLE SampleModel.Person
(
	FirstName CHARACTER VARYING(64) NOT NULL, 
	Person_id BIGINT GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL, 
	Date_YMD BIGINT NOT NULL, 
	LastName CHARACTER VARYING(64) NOT NULL, 
	OptionalUniqueString CHARACTER(11) CONSTRAINT OptionalUniqueString_Chk CHECK ((LENGTH(LTRIM(RTRIM(OptionalUniqueString)))) >= 11) , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LENGTH(LTRIM(RTRIM(Gender_Gender_Code)))) >= 1 AND 
Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	hasParents CHARACTER(1) FOR BIT DATA NOT NULL, 
	OptionalUniqueDecimal DECIMAL(9) , 
	MandatoryUniqueDecimal DECIMAL(9) NOT NULL, 
	MandatoryUniqueString CHARACTER(11) CONSTRAINT MandatoryUniqueString_Chk CHECK ((LENGTH(LTRIM(RTRIM(MandatoryUniqueString)))) >= 11) NOT NULL, 
	Husband_Person_id BIGINT , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT , 
	Father_Person_id BIGINT NOT NULL, 
	Mother_Person_id BIGINT NOT NULL, 
	Death_Date_YMD BIGINT , 
	Dth_DthCs_DthCs_Typ CHARACTER VARYING(14) CONSTRAINT DeathCause_Type_Chk CHECK (Dth_DthCs_DthCs_Typ IN ('natural', 'not so natural')) , 
	Dth_NtrlDth_sFrmPrsttCncr CHARACTER(1) FOR BIT DATA , 
	Death_UnnaturalDeath_isViolent CHARACTER(1) FOR BIT DATA , 
	Death_UnnaturalDeath_isBloody CHARACTER(1) FOR BIT DATA , 
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(Person_id)
	
	
	
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(MandatoryUniqueDecimal)
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(MandatoryUniqueString)
	
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(FirstName, Date_YMD)
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE(LastName, Date_YMD)
);

CREATE TABLE SampleModel.Task
(
	Task_id BIGINT GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1) NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
);

CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value BIGINT NOT NULL, 
	DsSmthngWthPrsn_Prsn_d BIGINT , 
	CONSTRAINT ValueType1Value_Unique PRIMARY KEY(ValueType1Value)
);

ALTER TABLE SampleModel.PersonDrivesCar ADD CONSTRAINT DrivenByPerson_FK FOREIGN KEY (DrivenByPerson_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PersonHasNickName ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT VlTyp1DsSmthngElsWth_FK FOREIGN KEY (VlTyp1DsSmthngElsWth_VlTyp1Vl)  REFERENCES SampleModel.ValueType1 (ValueType1Value)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT DoesSomethingWithPerson_FK FOREIGN KEY (DsSmthngWthPrsn_Prsn_d)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE PROCEDURE SampleModel.InsertPersonDrivesCar
(
	DrivesCar_vin BIGINT , 
	DrivenByPerson_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.PersonDrivesCar(DrivesCar_vin, DrivenByPerson_Person_id)
	VALUES (DrivesCar_vin, DrivenByPerson_Person_id);

CREATE PROCEDURE SampleModel.DeletePersonDrivesCar
(
	DrivesCar_vin BIGINT , 
	DrivenByPerson_Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.PersonDrivesCar
	WHERE DrivesCar_vin = DrivesCar_vin AND 
DrivenByPerson_Person_id = DrivenByPerson_Person_id;

CREATE PROCEDURE SampleModel.InsrtPrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin BIGINT , 
	SaleDate_YMD BIGINT , 
	Buyer_Person_id BIGINT , 
	Seller_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.PrsnBghtCrFrmPrsnOnDt(CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id)
	VALUES (CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id);

CREATE PROCEDURE SampleModel.DltPrsnBghtCrFrmPrsnOnDt
(
	Buyer_Person_id BIGINT , 
	CarSold_vin BIGINT , 
	Seller_Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.PrsnBghtCrFrmPrsnOnDt
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

CREATE PROCEDURE SampleModel.InsertPersonHasNickName
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.PersonHasNickName(NickName, Person_Person_id)
	VALUES (NickName, Person_Person_id);

CREATE PROCEDURE SampleModel.DeletePersonHasNickName
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
	OptionalUniqueString CHARACTER(11) , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code CHARACTER(1) , 
	hasParents CHARACTER(1) FOR BIT DATA , 
	OptionalUniqueDecimal DECIMAL(9) , 
	MandatoryUniqueDecimal DECIMAL(9) , 
	MandatoryUniqueString CHARACTER(11) , 
	Husband_Person_id BIGINT , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT , 
	Father_Person_id BIGINT , 
	Mother_Person_id BIGINT , 
	Death_Date_YMD BIGINT , 
	Dth_DthCs_DthCs_Typ CHARACTER VARYING(14) , 
	Dth_NtrlDth_sFrmPrsttCncr CHARACTER(1) FOR BIT DATA , 
	Death_UnnaturalDeath_isViolent CHARACTER(1) FOR BIT DATA , 
	Death_UnnaturalDeath_isBloody CHARACTER(1) FOR BIT DATA 
)
AS
	INSERT INTO SampleModel.Person(FirstName, Person_id, Date_YMD, LastName, OptionalUniqueString, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptionalUniqueDecimal, MandatoryUniqueDecimal, MandatoryUniqueString, Husband_Person_id, VlTyp1DsSmthngElsWth_VlTyp1Vl, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Father_Person_id, Mother_Person_id, Death_Date_YMD, Dth_DthCs_DthCs_Typ, Dth_NtrlDth_sFrmPrsttCncr, Death_UnnaturalDeath_isViolent, Death_UnnaturalDeath_isBloody)
	VALUES (FirstName, Person_id, Date_YMD, LastName, OptionalUniqueString, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptionalUniqueDecimal, MandatoryUniqueDecimal, MandatoryUniqueString, Husband_Person_id, VlTyp1DsSmthngElsWth_VlTyp1Vl, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Father_Person_id, Mother_Person_id, Death_Date_YMD, Dth_DthCs_DthCs_Typ, Dth_NtrlDth_sFrmPrsttCncr, Death_UnnaturalDeath_isViolent, Death_UnnaturalDeath_isBloody);

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
	DsSmthngWthPrsn_Prsn_d BIGINT 
)
AS
	INSERT INTO SampleModel.ValueType1(ValueType1Value, DsSmthngWthPrsn_Prsn_d)
	VALUES (ValueType1Value, DsSmthngWthPrsn_Prsn_d);

CREATE PROCEDURE SampleModel.DeleteValueType1
(
	ValueType1Value BIGINT 
)
AS
	DELETE FROM SampleModel.ValueType1
	WHERE ValueType1Value = ValueType1Value;
COMMIT;


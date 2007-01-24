START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel;

CREATE DOMAIN SampleModel.OptionalUniqueString AS CHARACTER(11) CONSTRAINT OptionalUniqueString_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 11) ;

CREATE DOMAIN SampleModel."Integer" AS BIGINT CONSTRAINT Integer_Chk CHECK (VALUE BETWEEN 1 AND 7) ;

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT DeathCause_Type_Chk CHECK (VALUE IN ('natural', 'not so natural')) ;

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 1 AND 
VALUE IN ('M', 'F')) ;

CREATE DOMAIN SampleModel.MandatoryUniqueString AS CHARACTER(11) CONSTRAINT MandatoryUniqueString_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 11) ;

CREATE TABLE SampleModel.Person
(
	FirstName CHARACTER VARYING(64) NOT NULL, 
	Person_id BIGSERIAL NOT NULL, 
	Date_YMD BIGINT NOT NULL, 
	LastName CHARACTER VARYING(64) NOT NULL, 
	OptionalUniqueString SampleModel.OptionalUniqueString , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code SampleModel.Gender_Code NOT NULL, 
	hasParents BOOLEAN NOT NULL, 
	OptionalUniqueDecimal DECIMAL(9) , 
	MandatoryUniqueDecimal DECIMAL(9) NOT NULL, 
	MandatoryUniqueString SampleModel.MandatoryUniqueString NOT NULL, 
	Husband_Person_id BIGINT , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT , 
	Father_Person_id BIGINT NOT NULL, 
	Mother_Person_id BIGINT NOT NULL, 
	Death_Date_YMD BIGINT , 
	Dth_DthCs_DthCs_Typ SampleModel.DeathCause_Type , 
	Dth_NtrlDth_sFrmPrsttCncr BOOLEAN , 
	Death_UnnaturalDeath_isViolent BOOLEAN , 
	Death_UnnaturalDeath_isBloody BOOLEAN , 
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

CREATE TABLE SampleModel.Task
(
	Task_id BIGSERIAL NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
);

CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value BIGINT NOT NULL, 
	DsSmthngWthPrsn_Prsn_d BIGINT , 
	CONSTRAINT ValueType1Value_Unique PRIMARY KEY(ValueType1Value)
);

CREATE TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin BIGINT NOT NULL, 
	SaleDate_YMD BIGINT NOT NULL, 
	Buyer_Person_id BIGINT NOT NULL, 
	Seller_Person_id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id), 
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin), 
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
);

CREATE TABLE SampleModel.Review
(
	Car_vin BIGINT NOT NULL, 
	Rating_Nr_Integer SampleModel."Integer" NOT NULL, 
	Criterion_Name CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Car_vin, Criterion_Name)
);

ALTER TABLE SampleModel.Person ADD CONSTRAINT Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT VlTyp1DsSmthngElsWth_FK FOREIGN KEY (VlTyp1DsSmthngElsWth_VlTyp1Vl)  REFERENCES SampleModel.ValueType1 (ValueType1Value)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Person ADD CONSTRAINT Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.Task ADD CONSTRAINT Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT DoesSomethingWithPerson_FK FOREIGN KEY (DsSmthngWthPrsn_Prsn_d)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE RESTRICT ON UPDATE RESTRICT;


CREATE FUNCTION SampleModel.InsertPerson
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
	hasParents BOOLEAN , 
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
	Dth_NtrlDth_sFrmPrsttCncr BOOLEAN , 
	Death_UnnaturalDeath_isViolent BOOLEAN , 
	Death_UnnaturalDeath_isBloody BOOLEAN 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.Person(FirstName, Person_id, Date_YMD, LastName, OptionalUniqueString, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptionalUniqueDecimal, MandatoryUniqueDecimal, MandatoryUniqueString, Husband_Person_id, VlTyp1DsSmthngElsWth_VlTyp1Vl, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Father_Person_id, Mother_Person_id, Death_Date_YMD, Dth_DthCs_DthCs_Typ, Dth_NtrlDth_sFrmPrsttCncr, Death_UnnaturalDeath_isViolent, Death_UnnaturalDeath_isBloody)
	VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17, $18, $19, $20, $21, $22, $23)';

CREATE FUNCTION SampleModel.DeletePerson
(
	Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.Person
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonFirstName
(
	old_Person_id BIGINT , 
	FirstName CHARACTER VARYING(64) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET FirstName = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonDate_YMD
(
	old_Person_id BIGINT , 
	Date_YMD BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Date_YMD = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonLastName
(
	old_Person_id BIGINT , 
	LastName CHARACTER VARYING(64) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET LastName = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnOptnlUnqStrng
(
	old_Person_id BIGINT , 
	OptionalUniqueString CHARACTER(11) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET OptionalUniqueString = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonHatType_ColorARGB
(
	old_Person_id BIGINT , 
	HatType_ColorARGB BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET HatType_ColorARGB = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonHTHTSHTSD
(
	old_Person_id BIGINT , 
	HTHTSHTSD CHARACTER VARYING(256) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET HTHTSHTSD = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonOwnsCar_vin
(
	old_Person_id BIGINT , 
	OwnsCar_vin BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET OwnsCar_vin = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonGender_Gender_Code
(
	old_Person_id BIGINT , 
	Gender_Gender_Code CHARACTER(1) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Gender_Gender_Code = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonhasParents
(
	old_Person_id BIGINT , 
	hasParents BOOLEAN 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET hasParents = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnOptnlUnqDcml
(
	old_Person_id BIGINT , 
	OptionalUniqueDecimal DECIMAL(9) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET OptionalUniqueDecimal = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnMndtryUnqDcml
(
	old_Person_id BIGINT , 
	MandatoryUniqueDecimal DECIMAL(9) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET MandatoryUniqueDecimal = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnMndtryUnqStrng
(
	old_Person_id BIGINT , 
	MandatoryUniqueString CHARACTER(11) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET MandatoryUniqueString = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonHusband_Person_id
(
	old_Person_id BIGINT , 
	Husband_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Husband_Person_id = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UPVT1DSEWVT1V
(
	old_Person_id BIGINT , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET VlTyp1DsSmthngElsWth_VlTyp1Vl = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UPCPBOBON
(
	old_Person_id BIGINT , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET ChldPrsn_BrthOrdr_BrthOrdr_Nr = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonFather_Person_id
(
	old_Person_id BIGINT , 
	Father_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Father_Person_id = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonMother_Person_id
(
	old_Person_id BIGINT , 
	Mother_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Mother_Person_id = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdatePersonDeath_Date_YMD
(
	old_Person_id BIGINT , 
	Death_Date_YMD BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Death_Date_YMD = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnDth_DthCs_DthCs_Typ
(
	old_Person_id BIGINT , 
	Dth_DthCs_DthCs_Typ CHARACTER VARYING(14) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Dth_DthCs_DthCs_Typ = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UPDNDFPC
(
	old_Person_id BIGINT , 
	Dth_NtrlDth_sFrmPrsttCncr BOOLEAN 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Dth_NtrlDth_sFrmPrsttCncr = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnDth_UnntrlDth_sVlnt
(
	old_Person_id BIGINT , 
	Death_UnnaturalDeath_isViolent BOOLEAN 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Death_UnnaturalDeath_isViolent = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.UpdtPrsnDth_UnntrlDth_sBldy
(
	old_Person_id BIGINT , 
	Death_UnnaturalDeath_isBloody BOOLEAN 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Person
SET Death_UnnaturalDeath_isBloody = $2
	WHERE Person_id = $1';

CREATE FUNCTION SampleModel.InsertTask
(
	Task_id BIGINT , 
	Person_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.Task(Task_id, Person_Person_id)
	VALUES ($1, $2)';

CREATE FUNCTION SampleModel.DeleteTask
(
	Task_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.Task
	WHERE Task_id = $1';

CREATE FUNCTION SampleModel.UpdateTaskPerson_Person_id
(
	old_Task_id BIGINT , 
	Person_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Task
SET Person_Person_id = $2
	WHERE Task_id = $1';

CREATE FUNCTION SampleModel.InsertValueType1
(
	ValueType1Value BIGINT , 
	DsSmthngWthPrsn_Prsn_d BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.ValueType1(ValueType1Value, DsSmthngWthPrsn_Prsn_d)
	VALUES ($1, $2)';

CREATE FUNCTION SampleModel.DeleteValueType1
(
	ValueType1Value BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.ValueType1
	WHERE ValueType1Value = $1';

CREATE FUNCTION SampleModel.UpdtVlTyp1VlTyp1Vl
(
	old_ValueType1Value BIGINT , 
	ValueType1Value BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.ValueType1
SET ValueType1Value = $2
	WHERE ValueType1Value = $1';

CREATE FUNCTION SampleModel.UVT1DSWPP
(
	old_ValueType1Value BIGINT , 
	DsSmthngWthPrsn_Prsn_d BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.ValueType1
SET DsSmthngWthPrsn_Prsn_d = $2
	WHERE ValueType1Value = $1';

CREATE FUNCTION SampleModel.InsrtPrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin BIGINT , 
	SaleDate_YMD BIGINT , 
	Buyer_Person_id BIGINT , 
	Seller_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.PrsnBghtCrFrmPrsnOnDt(CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id)
	VALUES ($1, $2, $3, $4)';

CREATE FUNCTION SampleModel.DltPrsnBghtCrFrmPrsnOnDt
(
	Buyer_Person_id BIGINT , 
	CarSold_vin BIGINT , 
	Seller_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.PrsnBghtCrFrmPrsnOnDt
	WHERE Buyer_Person_id = $1 AND 
CarSold_vin = $2 AND 
Seller_Person_id = $3';

CREATE FUNCTION SampleModel.UPBCFPODCS
(
	old_Buyer_Person_id BIGINT , 
	old_CarSold_vin BIGINT , 
	old_Seller_Person_id BIGINT , 
	CarSold_vin BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET CarSold_vin = $4
	WHERE Buyer_Person_id = $1 AND 
CarSold_vin = $2 AND 
Seller_Person_id = $3';

CREATE FUNCTION SampleModel.UPBCFPODSDYMD
(
	old_Buyer_Person_id BIGINT , 
	old_CarSold_vin BIGINT , 
	old_Seller_Person_id BIGINT , 
	SaleDate_YMD BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET SaleDate_YMD = $4
	WHERE Buyer_Person_id = $1 AND 
CarSold_vin = $2 AND 
Seller_Person_id = $3';

CREATE FUNCTION SampleModel.UPBCFPODBP
(
	old_Buyer_Person_id BIGINT , 
	old_CarSold_vin BIGINT , 
	old_Seller_Person_id BIGINT , 
	Buyer_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET Buyer_Person_id = $4
	WHERE Buyer_Person_id = $1 AND 
CarSold_vin = $2 AND 
Seller_Person_id = $3';

CREATE FUNCTION SampleModel.UPBCFPODSP
(
	old_Buyer_Person_id BIGINT , 
	old_CarSold_vin BIGINT , 
	old_Seller_Person_id BIGINT , 
	Seller_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET Seller_Person_id = $4
	WHERE Buyer_Person_id = $1 AND 
CarSold_vin = $2 AND 
Seller_Person_id = $3';

CREATE FUNCTION SampleModel.InsertReview
(
	Car_vin BIGINT , 
	Rating_Nr_Integer BIGINT , 
	Criterion_Name CHARACTER VARYING(64) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.Review(Car_vin, Rating_Nr_Integer, Criterion_Name)
	VALUES ($1, $2, $3)';

CREATE FUNCTION SampleModel.DeleteReview
(
	Car_vin BIGINT , 
	Criterion_Name CHARACTER VARYING(64) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.Review
	WHERE Car_vin = $1 AND 
Criterion_Name = $2';

CREATE FUNCTION SampleModel.UpdateReviewCar_vin
(
	old_Car_vin BIGINT , 
	old_Criterion_Name CHARACTER VARYING(64) , 
	Car_vin BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Review
SET Car_vin = $3
	WHERE Car_vin = $1 AND 
Criterion_Name = $2';

CREATE FUNCTION SampleModel.UpdateReviewRating_Nr_Integer
(
	old_Car_vin BIGINT , 
	old_Criterion_Name CHARACTER VARYING(64) , 
	Rating_Nr_Integer BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Review
SET Rating_Nr_Integer = $3
	WHERE Car_vin = $1 AND 
Criterion_Name = $2';

CREATE FUNCTION SampleModel.UpdateReviewCriterion_Name
(
	old_Car_vin BIGINT , 
	old_Criterion_Name CHARACTER VARYING(64) , 
	Criterion_Name CHARACTER VARYING(64) 
)
RETURNS VOID
LANGUAGE SQL
AS
	'UPDATE SampleModel.Review
SET Criterion_Name = $3
	WHERE Car_vin = $1 AND 
Criterion_Name = $2';
COMMIT WORK;


START TRANSACTION ISOLATION LEVEL SERIALIZABLE, READ WRITE;

CREATE SCHEMA SampleModel;

CREATE DOMAIN SampleModel.OptionalUniqueString AS CHARACTER(11) CONSTRAINT OptionalUniqueString_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 11) ;

CREATE DOMAIN SampleModel."Integer" AS BIGINT CONSTRAINT Integer_Chk CHECK (VALUE BETWEEN 1 AND 7) ;

CREATE DOMAIN SampleModel.DeathCause_Type AS CHARACTER VARYING(14) CONSTRAINT DeathCause_Type_Chk CHECK (VALUE IN ('natural', 'not so natural')) ;

CREATE DOMAIN SampleModel.Gender_Code AS CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 1 AND 
VALUE IN ('M', 'F')) ;

CREATE DOMAIN SampleModel.MandatoryUniqueString AS CHARACTER(11) CONSTRAINT MandatoryUniqueString_Chk CHECK ((CHARACTER_LENGTH(TRIM(BOTH FROM VALUE))) >= 11) ;

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

CREATE TABLE SampleModel.PersonHasNickName
(
	NickName CHARACTER VARYING(64) NOT NULL, 
	Person_Person_id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint33 PRIMARY KEY(NickName, Person_Person_id)
);

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


CREATE FUNCTION SampleModel.InsertPersonDrivesCar
(
	DrivesCar_vin BIGINT , 
	DrivenByPerson_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.PersonDrivesCar(DrivesCar_vin, DrivenByPerson_Person_id)
	VALUES ($1, $2)';

CREATE FUNCTION SampleModel.DeletePersonDrivesCar
(
	DrivesCar_vin BIGINT , 
	DrivenByPerson_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.PersonDrivesCar
	WHERE DrivesCar_vin = $1 AND 
DrivenByPerson_Person_id = $2';

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

CREATE FUNCTION SampleModel.InsertPersonHasNickName
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'INSERT INTO SampleModel.PersonHasNickName(NickName, Person_Person_id)
	VALUES ($1, $2)';

CREATE FUNCTION SampleModel.DeletePersonHasNickName
(
	NickName CHARACTER VARYING(64) , 
	Person_Person_id BIGINT 
)
RETURNS VOID
LANGUAGE SQL
AS
	'DELETE FROM SampleModel.PersonHasNickName
	WHERE NickName = $1 AND 
Person_Person_id = $2';

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
COMMIT WORK;


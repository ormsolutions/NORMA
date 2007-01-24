CREATE SCHEMA SampleModel
GO

GO

CREATE TABLE SampleModel.Person
(
	FirstName NATIONAL CHARACTER VARYING(64) NOT NULL, 
	Person_id BIGINT IDENTITY (1, 1) NOT NULL, 
	Date_YMD BIGINT NOT NULL, 
	LastName NATIONAL CHARACTER VARYING(64) NOT NULL, 
	OptionalUniqueString NATIONAL CHARACTER(11) CONSTRAINT OptionalUniqueString_Chk CHECK ((LEN(LTRIM(RTRIM(OptionalUniqueString)))) >= 11) , 
	HatType_ColorARGB BIGINT , 
	HTHTSHTSD NATIONAL CHARACTER VARYING(256) , 
	OwnsCar_vin BIGINT , 
	Gender_Gender_Code NATIONAL CHARACTER(1) CONSTRAINT Gender_Code_Chk CHECK ((LEN(LTRIM(RTRIM(Gender_Gender_Code)))) >= 1 AND 
Gender_Gender_Code IN ('M', 'F')) NOT NULL, 
	hasParents BIT NOT NULL, 
	OptionalUniqueDecimal DECIMAL(9) , 
	MandatoryUniqueDecimal DECIMAL(9) NOT NULL, 
	MandatoryUniqueString NATIONAL CHARACTER(11) CONSTRAINT MandatoryUniqueString_Chk CHECK ((LEN(LTRIM(RTRIM(MandatoryUniqueString)))) >= 11) NOT NULL, 
	Husband_Person_id BIGINT , 
	VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT , 
	ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT , 
	Father_Person_id BIGINT NOT NULL, 
	Mother_Person_id BIGINT NOT NULL, 
	Death_Date_YMD BIGINT , 
	Dth_DthCs_DthCs_Typ NATIONAL CHARACTER VARYING(14) CONSTRAINT DeathCause_Type_Chk CHECK (Dth_DthCs_DthCs_Typ IN ('natural', 'not so natural')) , 
	Dth_NtrlDth_sFrmPrsttCncr BIT , 
	Death_UnnaturalDeath_isViolent BIT , 
	Death_UnnaturalDeath_isBloody BIT , 
	CONSTRAINT InternalUniquenessConstraint2 PRIMARY KEY(Person_id), 
	CONSTRAINT InternalUniquenessConstraint9 UNIQUE(OptionalUniqueString), 
	CONSTRAINT InternalUniquenessConstraint22 UNIQUE(OwnsCar_vin), 
	CONSTRAINT InternalUniquenessConstraint65 UNIQUE(OptionalUniqueDecimal), 
	CONSTRAINT InternalUniquenessConstraint69 UNIQUE(MandatoryUniqueDecimal), 
	CONSTRAINT InternalUniquenessConstraint67 UNIQUE(MandatoryUniqueString), 
	CONSTRAINT ChldPrsn_IntrnlUnqnssCnstrnt49 UNIQUE(Father_Person_id, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Mother_Person_id), 
	CONSTRAINT ExternalUniquenessConstraint1 UNIQUE(FirstName, Date_YMD), 
	CONSTRAINT ExternalUniquenessConstraint2 UNIQUE(LastName, Date_YMD)
)
GO


CREATE TABLE SampleModel.Task
(
	Task_id BIGINT IDENTITY (1, 1) NOT NULL, 
	Person_Person_id BIGINT , 
	CONSTRAINT InternalUniquenessConstraint16 PRIMARY KEY(Task_id)
)
GO


CREATE TABLE SampleModel.ValueType1
(
	ValueType1Value BIGINT NOT NULL, 
	DsSmthngWthPrsn_Prsn_d BIGINT , 
	CONSTRAINT ValueType1Value_Unique PRIMARY KEY(ValueType1Value)
)
GO


CREATE TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt
(
	CarSold_vin BIGINT NOT NULL, 
	SaleDate_YMD BIGINT NOT NULL, 
	Buyer_Person_id BIGINT NOT NULL, 
	Seller_Person_id BIGINT NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint23 PRIMARY KEY(Buyer_Person_id, CarSold_vin, Seller_Person_id), 
	CONSTRAINT InternalUniquenessConstraint24 UNIQUE(SaleDate_YMD, Seller_Person_id, CarSold_vin), 
	CONSTRAINT InternalUniquenessConstraint25 UNIQUE(CarSold_vin, SaleDate_YMD, Buyer_Person_id)
)
GO


CREATE TABLE SampleModel.Review
(
	Car_vin BIGINT NOT NULL, 
	Rating_Nr_Integer BIGINT CONSTRAINT Integer_Chk CHECK (Rating_Nr_Integer BETWEEN 1 AND 7) NOT NULL, 
	Criterion_Name NATIONAL CHARACTER VARYING(64) NOT NULL, 
	CONSTRAINT InternalUniquenessConstraint26 PRIMARY KEY(Car_vin, Criterion_Name)
)
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Husband_FK FOREIGN KEY (Husband_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_VlTyp1DsSmthngElsWth_FK FOREIGN KEY (VlTyp1DsSmthngElsWth_VlTyp1Vl)  REFERENCES SampleModel.ValueType1 (ValueType1Value)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Father_FK FOREIGN KEY (Father_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Person ADD CONSTRAINT Person_Mother_FK FOREIGN KEY (Mother_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.Task ADD CONSTRAINT Task_Person_FK FOREIGN KEY (Person_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.ValueType1 ADD CONSTRAINT ValueType1_DoesSomethingWithPerson_FK FOREIGN KEY (DsSmthngWthPrsn_Prsn_d)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT PrsnBghtCrFrmPrsnOnDt_Buyer_FK FOREIGN KEY (Buyer_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO


ALTER TABLE SampleModel.PrsnBghtCrFrmPrsnOnDt ADD CONSTRAINT PrsnBghtCrFrmPrsnOnDt_Seller_FK FOREIGN KEY (Seller_Person_id)  REFERENCES SampleModel.Person (Person_id)  ON DELETE NO ACTION ON UPDATE NO ACTION
GO



CREATE PROCEDURE SampleModel.InsertPerson
(
	@FirstName NATIONAL CHARACTER VARYING(64) , 
	@Person_id BIGINT , 
	@Date_YMD BIGINT , 
	@LastName NATIONAL CHARACTER VARYING(64) , 
	@OptionalUniqueString NATIONAL CHARACTER(11) , 
	@HatType_ColorARGB BIGINT , 
	@HTHTSHTSD NATIONAL CHARACTER VARYING(256) , 
	@OwnsCar_vin BIGINT , 
	@Gender_Gender_Code NATIONAL CHARACTER(1) , 
	@hasParents BIT , 
	@OptionalUniqueDecimal DECIMAL(9) , 
	@MandatoryUniqueDecimal DECIMAL(9) , 
	@MandatoryUniqueString NATIONAL CHARACTER(11) , 
	@Husband_Person_id BIGINT , 
	@VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT , 
	@ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT , 
	@Father_Person_id BIGINT , 
	@Mother_Person_id BIGINT , 
	@Death_Date_YMD BIGINT , 
	@Dth_DthCs_DthCs_Typ NATIONAL CHARACTER VARYING(14) , 
	@Dth_NtrlDth_sFrmPrsttCncr BIT , 
	@Death_UnnaturalDeath_isViolent BIT , 
	@Death_UnnaturalDeath_isBloody BIT 
)
AS
	INSERT INTO SampleModel.Person(FirstName, Person_id, Date_YMD, LastName, OptionalUniqueString, HatType_ColorARGB, HTHTSHTSD, OwnsCar_vin, Gender_Gender_Code, hasParents, OptionalUniqueDecimal, MandatoryUniqueDecimal, MandatoryUniqueString, Husband_Person_id, VlTyp1DsSmthngElsWth_VlTyp1Vl, ChldPrsn_BrthOrdr_BrthOrdr_Nr, Father_Person_id, Mother_Person_id, Death_Date_YMD, Dth_DthCs_DthCs_Typ, Dth_NtrlDth_sFrmPrsttCncr, Death_UnnaturalDeath_isViolent, Death_UnnaturalDeath_isBloody)
	VALUES (@FirstName, @Person_id, @Date_YMD, @LastName, @OptionalUniqueString, @HatType_ColorARGB, @HTHTSHTSD, @OwnsCar_vin, @Gender_Gender_Code, @hasParents, @OptionalUniqueDecimal, @MandatoryUniqueDecimal, @MandatoryUniqueString, @Husband_Person_id, @VlTyp1DsSmthngElsWth_VlTyp1Vl, @ChldPrsn_BrthOrdr_BrthOrdr_Nr, @Father_Person_id, @Mother_Person_id, @Death_Date_YMD, @Dth_DthCs_DthCs_Typ, @Dth_NtrlDth_sFrmPrsttCncr, @Death_UnnaturalDeath_isViolent, @Death_UnnaturalDeath_isBloody)
GO


CREATE PROCEDURE SampleModel.DeletePerson
(
	@Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.Person
	WHERE Person_id = @Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonFirstName
(
	@old_Person_id BIGINT , 
	@FirstName NATIONAL CHARACTER VARYING(64) 
)
AS
	UPDATE SampleModel.Person
SET FirstName = @FirstName
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonDate_YMD
(
	@old_Person_id BIGINT , 
	@Date_YMD BIGINT 
)
AS
	UPDATE SampleModel.Person
SET Date_YMD = @Date_YMD
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonLastName
(
	@old_Person_id BIGINT , 
	@LastName NATIONAL CHARACTER VARYING(64) 
)
AS
	UPDATE SampleModel.Person
SET LastName = @LastName
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnOptnlUnqStrng
(
	@old_Person_id BIGINT , 
	@OptionalUniqueString NATIONAL CHARACTER(11) 
)
AS
	UPDATE SampleModel.Person
SET OptionalUniqueString = @OptionalUniqueString
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonHatType_ColorARGB
(
	@old_Person_id BIGINT , 
	@HatType_ColorARGB BIGINT 
)
AS
	UPDATE SampleModel.Person
SET HatType_ColorARGB = @HatType_ColorARGB
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonHTHTSHTSD
(
	@old_Person_id BIGINT , 
	@HTHTSHTSD NATIONAL CHARACTER VARYING(256) 
)
AS
	UPDATE SampleModel.Person
SET HTHTSHTSD = @HTHTSHTSD
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonOwnsCar_vin
(
	@old_Person_id BIGINT , 
	@OwnsCar_vin BIGINT 
)
AS
	UPDATE SampleModel.Person
SET OwnsCar_vin = @OwnsCar_vin
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonGender_Gender_Code
(
	@old_Person_id BIGINT , 
	@Gender_Gender_Code NATIONAL CHARACTER(1) 
)
AS
	UPDATE SampleModel.Person
SET Gender_Gender_Code = @Gender_Gender_Code
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonhasParents
(
	@old_Person_id BIGINT , 
	@hasParents BIT 
)
AS
	UPDATE SampleModel.Person
SET hasParents = @hasParents
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnOptnlUnqDcml
(
	@old_Person_id BIGINT , 
	@OptionalUniqueDecimal DECIMAL(9) 
)
AS
	UPDATE SampleModel.Person
SET OptionalUniqueDecimal = @OptionalUniqueDecimal
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnMndtryUnqDcml
(
	@old_Person_id BIGINT , 
	@MandatoryUniqueDecimal DECIMAL(9) 
)
AS
	UPDATE SampleModel.Person
SET MandatoryUniqueDecimal = @MandatoryUniqueDecimal
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnMndtryUnqStrng
(
	@old_Person_id BIGINT , 
	@MandatoryUniqueString NATIONAL CHARACTER(11) 
)
AS
	UPDATE SampleModel.Person
SET MandatoryUniqueString = @MandatoryUniqueString
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonHusband_Person_id
(
	@old_Person_id BIGINT , 
	@Husband_Person_id BIGINT 
)
AS
	UPDATE SampleModel.Person
SET Husband_Person_id = @Husband_Person_id
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UPVT1DSEWVT1V
(
	@old_Person_id BIGINT , 
	@VlTyp1DsSmthngElsWth_VlTyp1Vl BIGINT 
)
AS
	UPDATE SampleModel.Person
SET VlTyp1DsSmthngElsWth_VlTyp1Vl = @VlTyp1DsSmthngElsWth_VlTyp1Vl
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UPCPBOBON
(
	@old_Person_id BIGINT , 
	@ChldPrsn_BrthOrdr_BrthOrdr_Nr BIGINT 
)
AS
	UPDATE SampleModel.Person
SET ChldPrsn_BrthOrdr_BrthOrdr_Nr = @ChldPrsn_BrthOrdr_BrthOrdr_Nr
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonFather_Person_id
(
	@old_Person_id BIGINT , 
	@Father_Person_id BIGINT 
)
AS
	UPDATE SampleModel.Person
SET Father_Person_id = @Father_Person_id
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonMother_Person_id
(
	@old_Person_id BIGINT , 
	@Mother_Person_id BIGINT 
)
AS
	UPDATE SampleModel.Person
SET Mother_Person_id = @Mother_Person_id
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdatePersonDeath_Date_YMD
(
	@old_Person_id BIGINT , 
	@Death_Date_YMD BIGINT 
)
AS
	UPDATE SampleModel.Person
SET Death_Date_YMD = @Death_Date_YMD
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnDth_DthCs_DthCs_Typ
(
	@old_Person_id BIGINT , 
	@Dth_DthCs_DthCs_Typ NATIONAL CHARACTER VARYING(14) 
)
AS
	UPDATE SampleModel.Person
SET Dth_DthCs_DthCs_Typ = @Dth_DthCs_DthCs_Typ
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UPDNDFPC
(
	@old_Person_id BIGINT , 
	@Dth_NtrlDth_sFrmPrsttCncr BIT 
)
AS
	UPDATE SampleModel.Person
SET Dth_NtrlDth_sFrmPrsttCncr = @Dth_NtrlDth_sFrmPrsttCncr
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnDth_UnntrlDth_sVlnt
(
	@old_Person_id BIGINT , 
	@Death_UnnaturalDeath_isViolent BIT 
)
AS
	UPDATE SampleModel.Person
SET Death_UnnaturalDeath_isViolent = @Death_UnnaturalDeath_isViolent
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.UpdtPrsnDth_UnntrlDth_sBldy
(
	@old_Person_id BIGINT , 
	@Death_UnnaturalDeath_isBloody BIT 
)
AS
	UPDATE SampleModel.Person
SET Death_UnnaturalDeath_isBloody = @Death_UnnaturalDeath_isBloody
	WHERE Person_id = @old_Person_id
GO


CREATE PROCEDURE SampleModel.InsertTask
(
	@Task_id BIGINT , 
	@Person_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.Task(Task_id, Person_Person_id)
	VALUES (@Task_id, @Person_Person_id)
GO


CREATE PROCEDURE SampleModel.DeleteTask
(
	@Task_id BIGINT 
)
AS
	DELETE FROM SampleModel.Task
	WHERE Task_id = @Task_id
GO


CREATE PROCEDURE SampleModel.UpdateTaskPerson_Person_id
(
	@old_Task_id BIGINT , 
	@Person_Person_id BIGINT 
)
AS
	UPDATE SampleModel.Task
SET Person_Person_id = @Person_Person_id
	WHERE Task_id = @old_Task_id
GO


CREATE PROCEDURE SampleModel.InsertValueType1
(
	@ValueType1Value BIGINT , 
	@DsSmthngWthPrsn_Prsn_d BIGINT 
)
AS
	INSERT INTO SampleModel.ValueType1(ValueType1Value, DsSmthngWthPrsn_Prsn_d)
	VALUES (@ValueType1Value, @DsSmthngWthPrsn_Prsn_d)
GO


CREATE PROCEDURE SampleModel.DeleteValueType1
(
	@ValueType1Value BIGINT 
)
AS
	DELETE FROM SampleModel.ValueType1
	WHERE ValueType1Value = @ValueType1Value
GO


CREATE PROCEDURE SampleModel.UpdtVlTyp1VlTyp1Vl
(
	@old_ValueType1Value BIGINT , 
	@ValueType1Value BIGINT 
)
AS
	UPDATE SampleModel.ValueType1
SET ValueType1Value = @ValueType1Value
	WHERE ValueType1Value = @old_ValueType1Value
GO


CREATE PROCEDURE SampleModel.UVT1DSWPP
(
	@old_ValueType1Value BIGINT , 
	@DsSmthngWthPrsn_Prsn_d BIGINT 
)
AS
	UPDATE SampleModel.ValueType1
SET DsSmthngWthPrsn_Prsn_d = @DsSmthngWthPrsn_Prsn_d
	WHERE ValueType1Value = @old_ValueType1Value
GO


CREATE PROCEDURE SampleModel.InsrtPrsnBghtCrFrmPrsnOnDt
(
	@CarSold_vin BIGINT , 
	@SaleDate_YMD BIGINT , 
	@Buyer_Person_id BIGINT , 
	@Seller_Person_id BIGINT 
)
AS
	INSERT INTO SampleModel.PrsnBghtCrFrmPrsnOnDt(CarSold_vin, SaleDate_YMD, Buyer_Person_id, Seller_Person_id)
	VALUES (@CarSold_vin, @SaleDate_YMD, @Buyer_Person_id, @Seller_Person_id)
GO


CREATE PROCEDURE SampleModel.DltPrsnBghtCrFrmPrsnOnDt
(
	@Buyer_Person_id BIGINT , 
	@CarSold_vin BIGINT , 
	@Seller_Person_id BIGINT 
)
AS
	DELETE FROM SampleModel.PrsnBghtCrFrmPrsnOnDt
	WHERE Buyer_Person_id = @Buyer_Person_id AND 
CarSold_vin = @CarSold_vin AND 
Seller_Person_id = @Seller_Person_id
GO


CREATE PROCEDURE SampleModel.UPBCFPODCS
(
	@old_Buyer_Person_id BIGINT , 
	@old_CarSold_vin BIGINT , 
	@old_Seller_Person_id BIGINT , 
	@CarSold_vin BIGINT 
)
AS
	UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET CarSold_vin = @CarSold_vin
	WHERE Buyer_Person_id = @old_Buyer_Person_id AND 
CarSold_vin = @old_CarSold_vin AND 
Seller_Person_id = @old_Seller_Person_id
GO


CREATE PROCEDURE SampleModel.UPBCFPODSDYMD
(
	@old_Buyer_Person_id BIGINT , 
	@old_CarSold_vin BIGINT , 
	@old_Seller_Person_id BIGINT , 
	@SaleDate_YMD BIGINT 
)
AS
	UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET SaleDate_YMD = @SaleDate_YMD
	WHERE Buyer_Person_id = @old_Buyer_Person_id AND 
CarSold_vin = @old_CarSold_vin AND 
Seller_Person_id = @old_Seller_Person_id
GO


CREATE PROCEDURE SampleModel.UPBCFPODBP
(
	@old_Buyer_Person_id BIGINT , 
	@old_CarSold_vin BIGINT , 
	@old_Seller_Person_id BIGINT , 
	@Buyer_Person_id BIGINT 
)
AS
	UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET Buyer_Person_id = @Buyer_Person_id
	WHERE Buyer_Person_id = @old_Buyer_Person_id AND 
CarSold_vin = @old_CarSold_vin AND 
Seller_Person_id = @old_Seller_Person_id
GO


CREATE PROCEDURE SampleModel.UPBCFPODSP
(
	@old_Buyer_Person_id BIGINT , 
	@old_CarSold_vin BIGINT , 
	@old_Seller_Person_id BIGINT , 
	@Seller_Person_id BIGINT 
)
AS
	UPDATE SampleModel.PrsnBghtCrFrmPrsnOnDt
SET Seller_Person_id = @Seller_Person_id
	WHERE Buyer_Person_id = @old_Buyer_Person_id AND 
CarSold_vin = @old_CarSold_vin AND 
Seller_Person_id = @old_Seller_Person_id
GO


CREATE PROCEDURE SampleModel.InsertReview
(
	@Car_vin BIGINT , 
	@Rating_Nr_Integer BIGINT , 
	@Criterion_Name NATIONAL CHARACTER VARYING(64) 
)
AS
	INSERT INTO SampleModel.Review(Car_vin, Rating_Nr_Integer, Criterion_Name)
	VALUES (@Car_vin, @Rating_Nr_Integer, @Criterion_Name)
GO


CREATE PROCEDURE SampleModel.DeleteReview
(
	@Car_vin BIGINT , 
	@Criterion_Name NATIONAL CHARACTER VARYING(64) 
)
AS
	DELETE FROM SampleModel.Review
	WHERE Car_vin = @Car_vin AND 
Criterion_Name = @Criterion_Name
GO


CREATE PROCEDURE SampleModel.UpdateReviewCar_vin
(
	@old_Car_vin BIGINT , 
	@old_Criterion_Name NATIONAL CHARACTER VARYING(64) , 
	@Car_vin BIGINT 
)
AS
	UPDATE SampleModel.Review
SET Car_vin = @Car_vin
	WHERE Car_vin = @old_Car_vin AND 
Criterion_Name = @old_Criterion_Name
GO


CREATE PROCEDURE SampleModel.UpdateReviewRating_Nr_Integer
(
	@old_Car_vin BIGINT , 
	@old_Criterion_Name NATIONAL CHARACTER VARYING(64) , 
	@Rating_Nr_Integer BIGINT 
)
AS
	UPDATE SampleModel.Review
SET Rating_Nr_Integer = @Rating_Nr_Integer
	WHERE Car_vin = @old_Car_vin AND 
Criterion_Name = @old_Criterion_Name
GO


CREATE PROCEDURE SampleModel.UpdateReviewCriterion_Name
(
	@old_Car_vin BIGINT , 
	@old_Criterion_Name NATIONAL CHARACTER VARYING(64) , 
	@Criterion_Name NATIONAL CHARACTER VARYING(64) 
)
AS
	UPDATE SampleModel.Review
SET Criterion_Name = @Criterion_Name
	WHERE Car_vin = @old_Car_vin AND 
Criterion_Name = @old_Criterion_Name
GO


GO
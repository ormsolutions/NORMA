@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*
SET XMLDir=%TrunkDir%\XML
SET NetTiersDir=%TrunkDir%\CodeSmith\NetTiersPort
SET NORMAGenerators=HKLM\SOFTWARE\ORM Solutions\Natural ORM Architect for %TargetVisualStudioLongProductName%\Generators

:: Generate a native image for System.Data.SqlXml.dll if one does not already exist (this greatly improves the XSLT compilation speed).
:: Note that this method of determining whether a native image already exists is an undocumented hack that is subject to change. It should not be used for anything where reliability matters.
REG QUERY "HKLM\SOFTWARE\Microsoft\.NETFramework\%FrameworkVersion%\NGENService\Roots\System.Data.SqlXml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /v "Status" 1>NUL 2>&1
IF ERRORLEVEL 1 (ngen.exe install "System.Data.SqlXml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /nologo /verbose)

:: Delete old dlls
DEL /F /Q "%NORMADir%\bin\ORMSolutions.ORMArchitect.ORMCustomTool.dll.delete.*" 1>NUL 2>&1
CALL:_CleanupFile "%NORMADir%\bin\ORMSolutions.ORMArchitect.ORMCustomTool.dll"
CALL:_CleanupFile "%NORMADir%\bin\ORMSolutions.ORMArchitect.ORMCustomTool.pdb"
CALL:_CleanupFile "%NORMADir%\bin\ORMSolutions.ORMArchitect.ORMCustomTool.xml"
IF EXIST "%NORMADir%\bin\ORMSolutions.ORMArchitect.ORMCustomTool.dll" (REN "%NORMADir%\bin\ORMSolutions.ORMArchitect.ORMCustomTool.dll" "ORMSolutions.ORMArchitect.ORMCustomTool.dll.delete.%RANDOM%")

:: Install Custom Tool DLL
SET TargetBaseName=ORMSolutions.ORMArchitect.ORMCustomTool.%TargetVisualStudioShortProductName%
DEL /F /Q "%NORMADir%\bin\%TargetBaseName%.dll.delete.*" 1>NUL 2>&1
IF EXIST "%NORMADir%\bin\%TargetBaseName%.dll" (REN "%NORMADir%\bin\%TargetBaseName%.dll" "%TargetBaseName%.dll.delete.%RANDOM%")
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMADir%\bin\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMADir%\bin\"
:: For some reason, the next copy is randomly giving errors about half the time. They can be safely ignored, so they've been redirected to NUL.
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.xml" "%NORMADir%\bin\" 2>NUL
CALL:_InstallCustomToolReg "%VSRegistryRootVersion%"
CALL:_InstallExtenderReg "%VSRegistryRootVersion%"
IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_InstallCustomToolReg "%VSRegistryRootVersion%%VSRegistryRootSuffix%")
IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_InstallExtenderReg "%VSRegistryRootVersion%%VSRegistryRootSuffix%")

:: Get rid of old transform registrations
REG DELETE "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio" /f 1>NUL 2>&1
REG DELETE "HKLM\SOFTWARE\Neumont\ORM Architect for %TargetVisualStudioLongProductName%" /f 1>NUL 2>&1
REG DELETE "%NORMAGenerators%" /f 1>NUL 2>&1

:: Install and register ORM Transforms
XCOPY /Y /D /V /Q "%XMLDir%\OIAL\CoRefORM.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIAL\ORMtoOIAL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoXSD\OIALtoXSD.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoOWL\OIALtoOWL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoDCIL\OIALtoDCIL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\ConceptualDBtoDCIL\ConceptualDBtoDCIL.xslt" "%ORMTransformsDir%\"

XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoCLIProperties.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_GenerateTuple.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_GlobalSupportFunctions.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_GlobalSupportParameters.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_GenerateGlobalSupportClasses.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_Abstract.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_DataLayer_Implementation.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_DataLayer_SprocFree_Implementation.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\OIALtoPLiX_InMemory_Implementation.xslt" "%ORMTransformsDir%\"

XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\DataLayerTestForm\OIALtoPLiX_DataLayerTestForm.xslt" "%ORMTransformsDir%\DataLayerTestForm\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\DataLayerTestForm\OIALtoPLiX_InputControl.xslt" "%ORMTransformsDir%\DataLayerTestForm\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\DataLayerTestForm\OIALtoPLiX_InputControl_Designer.xslt" "%ORMTransformsDir%\DataLayerTestForm\"


CALL:_AddXslORMGenerator "CoRefORM" "ORM Co-Referencer" "Co-references (binarizes) an ORM file." ".CoRef.orm" "ORM" "CoRefORM" "%ORMTransformsDir%\CoRefORM.xslt" "" "1"
CALL:_AddXslORMGenerator "ORMtoOIAL" "ORM to OIAL" "Transforms a coreferenced ORM file to OIAL." ".OIAL.xml" "CoRefORM" "OIAL" "%ORMTransformsDir%\ORMtoOIAL.xslt" "" "1"
CALL:_AddXslORMGenerator "OIALtoXSD" "OIAL to XSD" "Transforms an OIAL file to XML Schema." ".xsd" "OIAL" "XSD" "%ORMTransformsDir%\OIALtoXSD.xslt"
CALL:_AddXslORMGenerator "OIALtoOWL" "OIAL to OWL" "Transforms an OIAL file to OWL." ".owl" "OIAL" "OWL" "%ORMTransformsDir%\OIALtoOWL.xslt"
CALL:_AddXslORMGenerator "OIALtoDCIL" "OIAL to DCIL" "Transforms an OIAL file to DCIL." ".DCIL.xml" "OIAL" "DCIL" "%ORMTransformsDir%\OIALtoDCIL.xslt" "" "1"
CALL:_AddXslORMGenerator "ConceptualDBtoDCL" "ConceptualDB to DCIL" "Transforms an ORM file with the ConceptualDB extension to DCIL." ".DCIL.xml" "ORM http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" "DCIL" "%ORMTransformsDir%\ConceptualDBtoDCIL.xslt" "" "1"

CALL:_AddXslORMGenerator "OIALtoCLIProperties" "OIAL to CLI Properties" "Transforms an OIAL file to CLI (Common Language Infrastructure) Properties" ".CLIProperties.xml" "OIAL" "CLIProperties" "%ORMTransformsDir%\OIALtoCLIProperties.xslt" "" "1"
CALL:_AddXslORMGenerator "PLiXSupport" "PLiX Support" "Transforms nothing to SupportClasses PLiX." ".Support.PLiX.xml" "ORM" "PLiX_Support" "%ORMTransformsDir%\OIALtoPLiX_GenerateGlobalSupportClasses.xslt" "NUPlixLoader"
CALL:_AddXslORMGenerator "CLIPropertiesToPLiXAbstract" "CLIProperties to PLiX Abstract" "Transforms a CLI Properties file to Abstract PLiX" ".Abstract.PLiX.xml" "CLIProperties" "PLiX_Abstract" "%ORMTransformsDir%\OIALtoPLiX_Abstract.xslt" "NUPlixLoader" "1" "" "" "OIAL\0"
CALL:_AddXslORMGenerator "CLIPropertiesToPLiXDataLayerWithSproc" "CLIProperties to PLiX Data Layer with Sprocs" "Transforms a CLI Properties file to DataLayer PLiX" ".Implementation.PLiX.xml" "CLIProperties" "PLiX_Implementation" "%ORMTransformsDir%\OIALtoPLiX_DataLayer_Implementation.xslt" "NUPlixLoader" "" "" "" "OIAL\0" "PLiX_Abstract\0"
CALL:_AddXslORMGenerator "CLIPropertiesToPliXDataLayerSprocFree" "CLIProperties to PLiX Sproc Free Data Layer" "Transforms a CLI Properties file to Sproc Free Data Layer PLiX" ".Implementation.PLiX.xml" "CLIProperties" "PLiX_Implementation" "%ORMTransformsDir%\OIALtoPLiX_DataLayer_SprocFree_Implementation.xslt" "NUPlixLoader" "" "" "" "OIAL\0" "PLiX_Abstract\0"
CALL:_AddXslORMGenerator "CLIPropertiesToPLiXInMemory" "CLIProperties to PLiX In Memory" "Transforms a CLI Properties file to InMemory PLiX" ".Implementation.PLiX.xml" "CLIProperties" "PLiX_Implementation" "%ORMTransformsDir%\OIALtoPLiX_InMemory_Implementation.xslt" "NUPlixLoader" "" "" "" "OIAL\0" "PLiX_Abstract\0"

CALL:_AddXslORMGenerator "DataLayerTestForm" "Data Layer Test Form" "Generates a Windows Form with custom controls for testing and manipulating data using the generated data access layer and database." ".DataLayerTestForm.PLiX.xml" "PLiX_Implementation" "DataLayerTestForm" "%ORMTransformsDir%\DataLayerTestForm\OIALtoPLiX_DataLayerTestForm.xslt" "NUPlixLoader" "" "" "" "" "DataLayerTestFormInputControl\0"
CALL:_AddXslORMGenerator "DataLayerTestFormInputControl" "Data Layer Test Form Input Control" "Generates a custom controls to be used on the generated form for testing and manipulating data using the generated data access layer and database." ".DataLayerTestFormInputControl.PLiX.xml" "PLiX_Implementation" "DataLayerTestFormInputControl" "%ORMTransformsDir%\DataLayerTestForm\OIALtoPLiX_InputControl.xslt" "NUPlixLoader" "" "" "" "OIAL\0" "DataLayerTestFormInputControlDesigner\0"
CALL:_AddXslORMGenerator "DataLayerTestFormInputControlDesigner" "Data Layer Test Form Input Control Designer" "Generates a custom controls to be used on the generated form for testing and manipulating data using the generated data access layer and database." ".DataLayerTestFormInputControl.Designer.PLiX.xml" "PLiX_Implementation" "DataLayerTestFormInputControlDesigner" "%ORMTransformsDir%\DataLayerTestForm\OIALtoPLiX_InputControl_Designer.xslt" "NUPlixLoader" "1" "" "" "OIAL\0"

:: Install and register DIL Transforms
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DCILtoDDIL.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DDILtoSQLStandard.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DDILtoPostgreSQL.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DDILtoDB2.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DDILtoSQLServer.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DDILtoOracle.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DDILtoMySQL.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\DomainInliner.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\TruthValueTestRemover.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\UniqueNullableOutliner.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DILtoSQL\TinyIntRemover.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DIL\DILSupportFunctions.xslt" "%DILTransformsDir%\"
CALL:_AddXslORMGenerator "DCILtoDDIL" "DCIL to DDIL" "Transforms DCIL to DDIL." ".DDIL.xml" "DCIL" "DDIL" "%DILTransformsDir%\DCILtoDDIL.xslt" "" "1"
CALL:_AddXslORMGenerator "DDILtoSQLStandard" "DDIL to SQL Standard" "Transforms DDIL to Standard-dialect SQL." ".SQLStandard.sql" "DDIL" "SQL_SQLStandard" "%DILTransformsDir%\DDILtoSQLStandard.xslt"
CALL:_AddXslORMGenerator "DDILtoPostgreSQL" "DDIL to PostgreSQL" "Transforms DDIL to PostgreSQL-dialect SQL." ".PostgreSQL.sql" "DDIL" "SQL_PostgreSQL" "%DILTransformsDir%\DDILtoPostgreSQL.xslt"
CALL:_AddXslORMGenerator "DDILtoDB2" "DDIL to DB2" "Transforms DDIL to DB2-dialect SQL." ".DB2.sql" "DDIL" "SQL_DB2" "%DILTransformsDir%\DDILtoDB2.xslt"
CALL:_AddXslORMGenerator "DDILtoSQLServer" "DDIL to SQL Server" "Transforms DDIL to SQL Server-dialect SQL." ".SQLServer.sql" "DDIL" "SQL_SQLServer" "%DILTransformsDir%\DDILtoSQLServer.xslt"
CALL:_AddXslORMGenerator "DDILtoOracle" "DDIL to Oracle" "Transforms DDIL to Oracle-dialect SQL." ".Oracle.sql" "DDIL" "SQL_Oracle" "%DILTransformsDir%\DDILtoOracle.xslt"
CALL:_AddXslORMGenerator "DDILtoMySQL" "DDIL to MySQL" "Transforms DDIL to MySQL-dialect SQL." ".MySQL.sql" "DDIL" "SQL_MySQL" "%DILTransformsDir%\DDILtoMySQL.xslt"
XCOPY /Y /D /V /Q "%XMLDir%\DCILtoHTML\DCILtoTV.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%XMLDir%\DCILtoHTML\TVtoHTML.xslt" "%DILTransformsDir%\"
CALL:_AddXslORMGenerator "DCILtoTV" "DCIL to TableView" "Transforms DCIL to TableView." ".TableView.xml" "DCIL" "TV" "%DILTransformsDir%\DCILtoTV.xslt" "" "1"
CALL:_AddXslORMGenerator "TVtoHTML" "TableView to HTML" "Transforms TableView to HTML." ".TableView.html" "TV" "TableViewHTML" "%DILTransformsDir%\TVtoHTML.xslt"

:: Install and register NetTiers Transforms
XCOPY /Y /D /V /Q "%NetTiersDir%\SchemaExplorer.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%NetTiersDir%\DCILToSchemaExplorer.xslt" "%ORMTransformsDir%\NetTiers\"
XCOPY /Y /D /V /Q "%NetTiersDir%\NetTiersSettings.xslt" "%ORMTransformsDir%\NetTiers\"
XCOPY /Y /D /V /Q "%NetTiersDir%\EntityProvider.xslt" "%ORMTransformsDir%\NetTiers\"
XCOPY /Y /D /V /Q "%NetTiersDir%\EntityScript.xslt" "%ORMTransformsDir%\NetTiers\"
XCOPY /Y /D /V /Q "%NetTiersDir%\Entities.xslt" "%ORMTransformsDir%\NetTiers\"
XCOPY /Y /D /V /Q "%NetTiersDir%\SqlProvider.xslt" "%ORMTransformsDir%\NetTiers\"
CALL:_AddXslORMGenerator "DCILtoSchemaExplorer" "DCIL to SchemaExplorer" "Transforms DCIL to SchemaExplorer." ".SchemaExplorer.xml" "DCIL" "SchemaExplorer" "%ORMTransformsDir%\NetTiers\DCILToSchemaExplorer.xslt" "" "1"
CALL:_AddXslORMGenerator "NetTiersSettings" "NetTiers Settings" "Default settings file for NetTiers generators" ".NetTiersSettings.xml" "ORM" "NetTiersSettings" "%ORMTransformsDir%\NetTiers\NetTiersSettings.xslt" "" "1" "1"
CALL:_AddXslORMGenerator "SchemaExplorertoNetTiersEntities" "SchemaExplorer to NetTiers Entities" "Transforms SchemaExplorer to NetTiers Entity layer." ".NetTiersEntities.xml" "SchemaExplorer" "NetTiersEntities" "%ORMTransformsDir%\NetTiers\Entities.xslt" "NUPlixLoader" "1" "" "" "NetTiersSettings\0"
CALL:_AddXslORMGenerator "SchemaExplorertoNetTiersDataAccessLayer" "SchemaExplorer to NetTiers DataAccessLayer" "Transforms SchemaExplorer to NetTiers DataAccessLayer." ".NetTiersDataAccessLayer.xml" "SchemaExplorer" "NetTiersDataAccessLayer" "%ORMTransformsDir%\NetTiers\EntityProvider.xslt" "NUPlixLoader" "" "" "" "NetTiersSettings\0" "NetTiersEntities\0"

:: Install and register LinqToSql Transforms
IF NOT "%TargetVisualStudioVersion%"=="v8.0" (
XCOPY /Y /D /V /Q "%XMLDir%\DCILtoLINQ\LinqToSqlSettings.xslt" "%ORMTransformsDir%\LinqToSql\"
XCOPY /Y /D /V /Q "%XMLDir%\DCILtoLINQ\DCILtoLinqAttributeMapping.xslt" "%ORMTransformsDir%\LinqToSql\"
XCOPY /Y /D /V /Q "%XMLDir%\DCILtoLINQ\DCILtoDBML.xslt" "%ORMTransformsDir%\LinqtoSql\"
CALL:_AddXslORMGenerator "DCILtoDBML" "DCIL to DBML" "Transforms DCIL to DBML." ".DBML" "DCIL" "DBML" "%ORMTransformsDir%\LinqToSql\DCILtoDBML.xslt" "MSLinqToSqlGenerator" "" "" "" "LinqToSqlSettings\0"
CALL:_AddXslORMGenerator "DCILtoLinqAttributeMapping" "DCIL to LinqToSql" "Transforms DCIL to a LinqToSql-targeted object model" ".LinqToSqlAttributeMapping.PLiX.xml" "DCIL" "LinqToSqlAttributeMapping" "%ORMTransformsDir%\LinqToSql\DCILtoLinqAttributeMapping.xslt" "NUPlixLoader" "" "" "" "LinqToSqlSettings\0"
CALL:_AddXslORMGenerator "LinqToSqlSettings" "LinqToSql Settings" "Default settings file for LinqToSql generators" ".LinqToSqlSettings.xml" "ORM" "LinqToSqlSettings" "%ORMTransformsDir%\LinqToSql\LinqToSqlSettings.xslt" "" "1" "1"
)

:: Install and register PHP Transforms
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\PHP\PHPDataLayer.xslt" "%ORMTransformsDir%\PHP\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\PHP\PHPServices.xslt" "%ORMTransformsDir%\PHP\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\PHP\PHPProxies.xslt" "%ORMTransformsDir%\PHP\"
XCOPY /Y /D /V /Q "%XMLDir%\OIALtoPLiX\PHP\PHPEntities.xslt" "%ORMTransformsDir%\PHP\"

CALL:_AddXslORMGenerator "OIALtoPHPEntitiesPLiX" "OIAL to PHP Entities PLiX" "Transforms OIAL to PHP Entities PLiX." ".Entities.php.xml" "OIAL" "PHPEntitiesPLiX" "%ORMTransformsDir%\PHP\PHPEntities.xslt" "" "1" "" "" "" "PHPProxiesPLiX\0PHPServicesPLiX\0PHPDataLayerPLiX\0"
CALL:_AddXslORMGenerator "PHPEntitiesPLiXtoPHP" "PHP Entities PLiX to PHP" "Transforms PLiX Entities TO PHP." ".Entities.php" "PHPEntitiesPLiX" "PHPEntitiesImplementation" "%PLiXDir%\Formatters\PLiXPHP.xslt" "" "" "" "" "" "PHPProxiesImplementation\0PHPServicesImplementation\0PHPDataLayerImplementation\0"

CALL:_AddXslORMGenerator "OIALtoPHPProxiesPLiX" "OIAL to PHP Proxies PLiX" "Transforms OIAL to PHP Proxies PLiX." ".Proxies.php.xml" "OIAL" "PHPProxiesPLiX" "%ORMTransformsDir%\PHP\PHPProxies.xslt" "" "1"
CALL:_AddXslORMGenerator "PHPProxiesPLiXtoPHP" "PHP Proxies PLiX to PHP" "Transforms PLiX Proxies TO PHP." ".Proxies.php" "PHPProxiesPLiX" "PHPProxiesImplementation" "%PLiXDir%\Formatters\PLiXPHP.xslt" "" "1"

CALL:_AddXslORMGenerator "OIALtoPHPServicesPLiX" "OIAL to PHP Services PLiX" "Transforms OIAL to PHP Services PLiX." ".Services.php.xml" "OIAL" "PHPServicesPLiX" "%ORMTransformsDir%\PHP\PHPServices.xslt" "" "1"
CALL:_AddXslORMGenerator "PHPServicesPLiXtoPHP" "PHP Services PLiX to PHP" "Transforms PLiX Services TO PHP." ".Services.php" "PHPServicesPLiX" "PHPServicesImplementation" "%PLiXDir%\Formatters\PLiXPHP.xslt" "" "1"

CALL:_AddXslORMGenerator "OIALtoPHPDataLayerPLiX" "OIAL to PHP Data Layer PLiX" "Transforms OIAL to PHP Data Layer PLiX." ".DataLayer.php.xml" "OIAL" "PHPDataLayerPLiX" "%ORMTransformsDir%\PHP\PHPDataLayer.xslt" "" "1"
CALL:_AddXslORMGenerator "PHPDataLayerPLiXtoPHP" "PHP DataLayer PLiX to PHP" "Transforms PLiX Data Layer TO PHP." ".DataLayer.php" "PHPDataLayerPLiX" "PHPDataLayerImplementation" "%PLiXDir%\Formatters\PLiXPHP.xslt" "" "1"
GOTO:EOF

:_InstallCustomToolReg
CALL:_AddCustomToolReg "%~1"
CALL:_AddRegGenerator "%~1" "{164b10b9-b200-11d0-8c61-00a0c91e29d5}"
CALL:_AddRegGenerator "%~1" "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}"
CALL:_AddRegGenerator "%~1" "{e6fdf8b0-f3d1-11d4-8576-0002a516ece8}"
GOTO:EOF

:_InstallExtenderReg
CALL:_AddExtenderReg "%~1"
CALL:_AddRegExtender "%~1" "{8D58E6AF-ED4E-48B0-8C7B-C74EF0735451}"
CALL:_AddRegExtender "%~1" "{EA5BD05D-3C72-40A5-95A0-28A2773311CA}"
CALL:_AddRegExtender "%~1" "{E6FDF869-F3D1-11D4-8576-0002A516ECE8}"
GOTO:EOF

:_AddCustomToolReg
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}" /f /ve /d "ORMSolutions.ORMArchitect.ORMCustomTool.ORMCustomTool" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32" /f /ve /d "%SystemRoot%\System32\mscoree.dll" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32" /f /v "ThreadingModel" /d "Both" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32" /f /v "Class" /d "ORMSolutions.ORMArchitect.ORMCustomTool.ORMCustomTool" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32" /f /v "CodeBase" /d "%NORMADir%\bin\%TargetBaseName%.dll" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32" /f /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" 1>NUL 2>&1
GOTO:EOF

:_AddExtenderReg
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}" /f /ve /d "ORMSolutions.ORMArchitect.ORMCustomTool.ExtenderProvider" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32" /f /ve /d "%SystemRoot%\System32\mscoree.dll" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32" /f /v "ThreadingModel" /d "Both" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32" /f /v "Class" /d "ORMSolutions.ORMArchitect.ORMCustomTool.ExtenderProvider" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32" /f /v "CodeBase" /d "%NORMADir%\bin\%TargetBaseName%.dll" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32" /f /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32\1.0.0.0" /f /v "Class" /d "ORMSolutions.ORMArchitect.ORMCustomTool.ExtenderProvider" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32\1.0.0.0" /f /v "CodeBase" /d "%TargetBaseName%.dll" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32\1.0.0.0" /f /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" 1>NUL 2>&1
GOTO:EOF

:_AddRegGenerator
REG ADD "HKLM\%VSRegistryRootBase%\%~1\Generators\%~2\ORMCustomTool" /f /ve /d "ORM Custom Tool" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\Generators\%~2\ORMCustomTool" /f /v "CLSID" /d "{977BD01E-F2B4-4341-9C47-459420624A20}" 1>NUL 2>&1
REG ADD "HKLM\%VSRegistryRootBase%\%~1\Generators\%~2\.orm" /f /ve /d "ORMCustomTool" 1>NUL 2>&1
GOTO:EOF

:_AddRegExtender
REG ADD "HKLM\%VSRegistryRootBase%\%~1\Extenders\%~2\ORMCustomTool" /f /ve /d "{6FDCC073-20C2-4435-9B2E-9E70451C81D8}" 1>NUL 2>&1
GOTO:EOF

:_AddXslORMGenerator
REG DELETE "%NORMAGenerators%\%~1" /va /f 1>NUL 2>&1
REG ADD "%NORMAGenerators%\%~1" /f /v "Type" /d "XSLT" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "OfficialName" /d "%~1" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "DisplayName" /d "%~2" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "DisplayDescription" /d "%~3" 1>NUL
IF NOT "%~4"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "FileExtension" /d "%~4") 1>NUL 2>&1
REG ADD "%NORMAGenerators%\%~1" /f /v "SourceInputFormat" /d "%~5" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "ProvidesOutputFormat" /d "%~6" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "TransformUri" /d "%~7" 1>NUL
IF NOT "%~8"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "CustomTool" /d "%~8") 1>NUL
IF NOT "%~9"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "GeneratesSupportFile" /t REG_DWORD /d "%~9") 1>NUL
SHIFT /8
IF NOT "%~9"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "GeneratesOnce" /t REG_DWORD /d "%~9") 1>NUL
SHIFT /8
IF NOT "%~9"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "Compilable" /t REG_DWORD /d "%~9") 1>NUL
SHIFT /8
IF NOT "%~9"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "ReferenceInputFormats" /t REG_MULTI_SZ /d "%~9") 1>NUL
SHIFT /8
IF NOT "%~9"=="" (REG ADD "%NORMAGenerators%\%~1" /f /v "CompanionOutputFormats" /t REG_MULTI_SZ /d "%~9") 1>NUL
GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

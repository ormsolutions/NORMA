@ECHO OFF
SETLOCAL

SET VSDir=%ProgramFiles%\Microsoft Visual Studio 8
SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio
SET ORMTransformsDir=%CommonProgramFiles%\Neumont\ORM\Transforms
SET DILTransformsDir=%CommonProgramFiles%\Neumont\DIL\Transforms
SET PLiXDir=%CommonProgramFiles%\Neumont\PLiX

:: Install Custom Tool DLL
DEL /F /Q "%VSDir%\Common7\IDE\PrivateAssemblies\Neumont.Tools.ORM.ORMCustomTool.*" 1>NUL 2>&1
XCOPY /Y /D /V /Q "%~dp0\bin\Neumont.Tools.ORM.ORMCustomTool.dll" "%NORMADir%\bin\"
XCOPY /Y /D /V /Q "%~dp0\bin\Neumont.Tools.ORM.ORMCustomTool.pdb" "%NORMADir%\bin\"
:: For some reason, the next copy is randomly giving errors about half the time. They can be safely ignored, so they've been redirected to NUL.
XCOPY /Y /D /V /Q "%~dp0\bin\Neumont.Tools.ORM.ORMCustomTool.xml" "%NORMADir%\bin\" 2>NUL
CALL:_InstallCustomToolReg "8.0"
CALL:_InstallExtenderReg "8.0"
CALL:_InstallCustomToolReg "8.0Exp"
CALL:_InstallExtenderReg "8.0Exp"

:: Install and register ORM Transforms
XCOPY /Y /D /V /Q "%~dp0\..\OIAL\CoRefORM.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIAL\ORMtoOIAL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoXSD\OIALtoXSD.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoOWL\OIALtoOWL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoDCIL\OIALtoDCIL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_GenerateGlobalSupportClasses.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_GenerateTuple.xslt" "%ORMTransformsDir%\"

XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_Abstract.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_DataLayer_Implementation.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_GlobalSupportFunctions.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_GlobalSupportParameters.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoPLiX_InMemory_Implementation.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoPLiX\OIALtoCLIProperties.xslt" "%ORMTransformsDir%\"

CALL:_AddXslORMGenerator "CoRefORM" "ORM Co-Referencer" "Co-references (binarizes) an ORM file." ".CoRef.orm" "ORM" "CoRefORM" "%ORMTransformsDir%\CoRefORM.xslt"
CALL:_AddXslORMGenerator "ORMtoOIAL" "ORM to OIAL" "Transforms a coreferenced ORM file to OIAL." ".OIAL.xml" "CoRefORM" "OIAL" "%ORMTransformsDir%\ORMtoOIAL.xslt"
CALL:_AddXslORMGenerator "OIALtoXSD" "OIAL to XSD" "Transforms an OIAL file to XML Schema." ".xsd" "OIAL" "XSD" "%ORMTransformsDir%\OIALtoXSD.xslt"
CALL:_AddXslORMGenerator "OIALtoOWL" "OIAL to OWL" "Transforms an OIAL file to OWL." ".owl" "OIAL" "OWL" "%ORMTransformsDir%\OIALtoOWL.xslt"
CALL:_AddXslORMGenerator "OIALtoDCIL" "OIAL to DCIL" "Transforms an OIAL file to DCIL." ".DCIL.xml" "OIAL" "DCIL" "%ORMTransformsDir%\OIALtoDCIL.xslt"

CALL:_AddXslORMGenerator "OIALtoCLIProperties" "OIAL to CLI Properties" "Transforms an OIAL file to CLI (Common Language Infrastructure) Properties" ".CLIProperties.xml" "OIAL" "CLIProperties" "%ORMTransformsDir%\OIALtoCLIProperties.xslt"
CALL:_AddXslORMGenerator "PLiXSupport" "PLiX Support" "Transforms nothing to SupportClasses PLiX." ".Support.PLiX.xml" "OIAL" "PLiX_Support" "%ORMTransformsDir%\OIALtoPLiX_GenerateGlobalSupportClasses.xslt"
CALL:_AddXslORMGenerator "CLIPropertiesToPLiXAbstract" "CLIProperties to PLiX Abstract" "Transforms a CLI Properties file to Abstract PLiX" ".Abstract.PLiX.xml" "CLIProperties" "PLiX_Abstract" "%ORMTransformsDir%\OIALtoPLiX_Abstract.xslt" "" "OIAL\0" "PLiX_Support\0"
CALL:_AddXslORMGenerator "CLIPropertiesToPLiXDataLayer" "CLIProperties to PLiX Data Layer" "Transforms a CLI Properties file to DataLayer PLiX" ".Implementation.PLiX.xml" "CLIProperties" "PLiX_Implementation" "%ORMTransformsDir%\OIALtoPLiX_DataLayer_Implementation.xslt" "" "OIAL\0" "PLiX_Abstract\0"
CALL:_AddXslORMGenerator "CLIPropertiesToPLiXInMemory" "CLIProperties to PLiX In Memory" "Transforms a CLI Properties file to InMemory PLiX" ".Implementation.PLiX.xml" "CLIProperties" "PLiX_Implementation" "%ORMTransformsDir%\OIALtoPLiX_InMemory_Implementation.xslt" "" "OIAL\0" "PLiX_Abstract\0"


:: Install and register DIL Transforms
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DCILtoDDIL.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DDILtoSQLStandard.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DDILtoPostgreSQL.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DDILtoDB2.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DDILtoSQLServer.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DDILtoOracle.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DILtoSQL\DomainInliner.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DIL\DILSupportFunctions.xslt" "%DILTransformsDir%\"
CALL:_AddXslORMGenerator "DCILtoDDIL" "DCIL to DDIL" "Transforms DCIL to DDIL." ".DDIL.xml" "DCIL" "DDIL" "%DILTransformsDir%\DCILtoDDIL.xslt"
CALL:_AddXslORMGenerator "DDILtoSQLStandard" "DDIL to SQL Standard" "Transforms DDIL to Standard-dialect SQL." ".SQLStandard.sql" "DDIL" "SQL_SQLStandard" "%DILTransformsDir%\DDILtoSQLStandard.xslt"
CALL:_AddXslORMGenerator "DDILtoPostgreSQL" "DDIL to PostgreSQL" "Transforms DDIL to PostgreSQL-dialect SQL." ".PostgreSQL.sql" "DDIL" "SQL_PostgreSQL" "%DILTransformsDir%\DDILtoPostgreSQL.xslt"
CALL:_AddXslORMGenerator "DDILtoDB2" "DDIL to DB2" "Transforms DDIL to DB2-dialect SQL." ".DB2.sql" "DDIL" "SQL_DB2" "%DILTransformsDir%\DDILtoDB2.xslt"
CALL:_AddXslORMGenerator "DDILtoSQLServer" "DDIL to SQL Server" "Transforms DDIL to SQL Server-dialect SQL." ".SQLServer.sql" "DDIL" "SQL_SQLServer" "%DILTransformsDir%\DDILtoSQLServer.xslt"
CALL:_AddXslORMGenerator "DDILtoOracle" "DDIL to Oracle" "Transforms DDIL to Oracle-dialect SQL." ".Oracle.sql" "DDIL" "SQL_Oracle" "%DILTransformsDir%\DDILtoOracle.xslt"
XCOPY /Y /D /V /Q "%~dp0\..\DCILtoHTML\DCILtoTV.xslt" "%DILTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\DCILtoHTML\TVtoHTML.xslt" "%DILTransformsDir%\"
CALL:_AddXslORMGenerator "DCILtoTV" "DCIL to TableView" "Transforms DCIL to TableView." ".TableView.xml" "DCIL" "TV" "%DILTransformsDir%\DCILtoTV.xslt"
CALL:_AddXslORMGenerator "TVtoHTML" "TableView to HTML" "Transforms TableView to HTML." ".TableView.html" "TV" "TableViewHTML" "%DILTransformsDir%\TVtoHTML.xslt"

:: Register PLiX Transforms

CALL:_AddXslORMGenerator "PLiXtoCSharpSupport" "PLiX to C# Support" "Transforms PLiX to C#." ".Support.cs" "PLiX_Support" "CSharp_Support" "%PLiXDir%\Formatters\PLiXCS.xslt" "1"
CALL:_AddXslORMGenerator "PLiXtoVisualBasicSupport" "PLiX to Visual Basic Support" "Transforms PLiX to Visual Basic." ".Support.vb" "PLiX_Support" "VisualBasic_Support" "%PLiXDir%\Formatters\PLiXVB.xslt" "1"
CALL:_AddXslORMGenerator "PLiXtoCSharpAbstract" "PLiX to C# Abstract" "Transforms PLiX to C#." ".Abstract.cs" "PLiX_Abstract" "CSharp_Abstract" "%PLiXDir%\Formatters\PLiXCS.xslt" "1" "" "CSharp_Support\0"
CALL:_AddXslORMGenerator "PLiXtoVisualBasicAbstract" "PLiX to Visual Basic Abstract" "Transforms PLiX to Visual Basic." ".Abstract.vb" "PLiX_Abstract" "VisualBasic_Abstract" "%PLiXDir%\Formatters\PLiXVB.xslt" "1" "" "VisualBasic_Support\0"
CALL:_AddXslORMGenerator "PLiXtoCSharpImplementation" "PLiX to C# Implementation" "Transforms PLiX to C#." ".Implementation.cs" "PLiX_Implementation" "CSharp_Implementation" "%PLiXDir%\Formatters\PLiXCS.xslt" "1" "" "CSharp_Abstract\0"
CALL:_AddXslORMGenerator "PLiXtoVisualBasicImplementation" "PLiX to Visual Basic Implementation" "Transforms PLiX to Visual Basic." ".Implementation.vb" "PLiX_Implementation" "VisualBasic_Implementation" "%PLiXDir%\Formatters\PLiXVB.xslt" "1" "" "VisualBasic_Abstract\0"

GOTO:EOF

:_InstallCustomToolReg
REG QUERY HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /v "CodeBase" 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (GOTO:EOF)
CALL:_AddCustomToolReg "%~1"
CALL:_AddRegGenerator "%~1" "{164b10b9-b200-11d0-8c61-00a0c91e29d5}"
CALL:_AddRegGenerator "%~1" "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}"
CALL:_AddRegGenerator "%~1" "{e6fdf8b0-f3d1-11d4-8576-0002a516ece8}"
GOTO:EOF

:_InstallExtenderReg
REG QUERY HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32 /v "CodeBase" 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (GOTO:EOF)
CALL:_AddExtenderReg "%~1"
CALL:_AddRegExtender "%~1" "{8D58E6AF-ED4E-48B0-8C7B-C74EF0735451}"
CALL:_AddRegExtender "%~1" "{EA5BD05D-3C72-40A5-95A0-28A2773311CA}"
CALL:_AddRegExtender "%~1" "{E6FDF869-F3D1-11D4-8576-0002A516ECE8}"
GOTO:EOF

:_AddCustomToolReg
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20} /f /ve /d "Neumont.Tools.ORM.ORMCustomTool.ORMCustomTool"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /ve /d "%SystemRoot%\System32\mscoree.dll"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "ThreadingModel" /d "Both"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "Class" /d "Neumont.Tools.ORM.ORMCustomTool.ORMCustomTool"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "CodeBase" /d "%NORMADir%\bin\Neumont.Tools.ORM.ORMCustomTool.dll"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "Assembly" /d "Neumont.Tools.ORM.ORMCustomTool, Version=1.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

:_AddExtenderReg
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8} /f /ve /d "Neumont.Tools.ORM.ORMCustomTool.ExtenderProvider"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32 /f /ve /d "%SystemRoot%\System32\mscoree.dll"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32 /f /v "ThreadingModel" /d "Both"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32 /f /v "Class" /d "Neumont.Tools.ORM.ORMCustomTool.ExtenderProvider"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32 /f /v "CodeBase" /d "%NORMADir%\bin\Neumont.Tools.ORM.ORMCustomTool.dll"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32 /f /v "Assembly" /d "Neumont.Tools.ORM.ORMCustomTool, Version=1.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32\1.0.0.0 /f /v "Class" /d "Neumont.Tools.ORM.ORMCustomTool.ExtenderProvider"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32\1.0.0.0 /f /v "CodeBase" /d "%NORMADir%\bin\Neumont.Tools.ORM.ORMCustomTool.dll"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\CLSID\{6FDCC073-20C2-4435-9B2E-9E70451C81D8}\InprocServer32\1.0.0.0 /f /v "Assembly" /d "Neumont.Tools.ORM.ORMCustomTool, Version=1.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

:_AddRegGenerator
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\Generators\%~2\ORMCustomTool /f /ve /d "ORM Custom Tool"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\Generators\%~2\ORMCustomTool /f /v "CLSID" /d "{977BD01E-F2B4-4341-9C47-459420624A20}"
GOTO:EOF

:_AddRegExtender
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\Extenders\%~2\ORMCustomTool /f /ve /d "{6FDCC073-20C2-4435-9B2E-9E70451C81D8}"
GOTO:EOF

:_AddXslORMGenerator
REG DELETE "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /va /f 1>NUL 2>&1
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "Type" /d "XSLT" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "OfficialName" /d "%~1" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "DisplayName" /d "%~2" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "DisplayDescription" /d "%~3" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "FileExtension" /d "%~4" 1>NUL 2>&1
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "SourceInputFormat" /d "%~5" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "ProvidesOutputFormat" /d "%~6" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "TransformUri" /d "%~7" 1>NUL
IF NOT "%~8"=="" (REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "Compilable" /t REG_DWORD /d "%~8") 1>NUL
IF NOT "%~9"=="" (REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "ReferenceInputFormats" /t REG_MULTI_SZ /d "%~9") 1>NUL
SHIFT /8
IF NOT "%~9"=="" (REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "PrequisiteInputFormats" /t REG_MULTI_SZ /d "%~9") 1>NUL
GOTO:EOF

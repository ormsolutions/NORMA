@ECHO OFF
SETLOCAL

SET VSDir=%ProgramFiles%\Microsoft Visual Studio 8
SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio
SET ORMTransformsDir=%CommonProgramFiles%\ORM\Transforms
SET DILTransformsDir=%CommonProgramFiles%\DIL\Transforms

:: Install Custom Tool DLL
DEL /F /Q "%VSDir%\Common7\IDE\PrivateAssemblies\Neumont.Tools.ORM.ORMCustomTool.*" 1>NUL 2>&1
XCOPY /Y /D /V /Q "%~dp0\bin\Neumont.Tools.ORM.ORMCustomTool.dll" "%NORMADir%\bin\"
XCOPY /Y /D /V /Q "%~dp0\bin\Neumont.Tools.ORM.ORMCustomTool.pdb" "%NORMADir%\bin\"
CALL:_InstallCustomToolReg

:: Install ORM Transforms
XCOPY /Y /D /V /Q "%~dp0\..\OIAL\CoRefORM.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIAL\ORMtoOIAL.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoXSD\OIALtoXSD.xslt" "%ORMTransformsDir%\"
XCOPY /Y /D /V /Q "%~dp0\..\OIALtoDCIL\OIALtoDCIL.xslt" "%ORMTransformsDir%\"
CALL:_AddXslORMGenerator "CoRefORM" "ORM Co-Referencer" "Co-references (binarizes) an ORM file." ".orm" "ORM" "ORM" "%ORMTransformsDir%\CoRefORM.xslt"
CALL:_AddXslORMGenerator "ORMtoOIAL" "ORM to OIAL" "Transforms an ORM file to OIAL." ".OIAL.xml" "ORM" "OIAL" "%ORMTransformsDir%\ORMtoOIAL.xslt"
CALL:_AddXslORMGenerator "OIALtoXSD" "OIAL to XSD" "Transforms an OIAL file to XML Schema." ".xsd" "OIAL" "XSD" "%ORMTransformsDir%\OIALtoXSD.xslt"
CALL:_AddXslORMGenerator "OIALtoDCIL" "OIAL to DCIL" "Transforms an OIAL file to DCIL." ".DCIL.xml" "OIAL" "DCIL" "%ORMTransformsDir%\OIALtoDCIL.xslt"

:: Install DIL Transforms
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

GOTO:EOF

:_InstallCustomToolReg
REG QUERY HKCR\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /v "CodeBase" 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (GOTO:EOF)
CALL:_AddCustomToolRegCR
CALL:_AddRegGenerator "8.0" "{164b10b9-b200-11d0-8c61-00a0c91e29d5}"
CALL:_AddRegGenerator "8.0" "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}"
CALL:_AddRegGenerator "8.0" "{e6fdf8b0-f3d1-11d4-8576-0002a516ece8}"
CALL:_AddRegGenerator "8.0Exp" "{164b10b9-b200-11d0-8c61-00a0c91e29d5}"
CALL:_AddRegGenerator "8.0Exp" "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}"
CALL:_AddRegGenerator "8.0Exp" "{e6fdf8b0-f3d1-11d4-8576-0002a516ece8}"
GOTO:EOF

:_AddCustomToolRegCR
REG ADD HKLM\SOFTWARE\Classes\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20} /f /ve /d "Neumont.Tools.ORM.ORMCustomTool.ORMCustomTool, Neumont.Tools.ORM.ORMCustomTool, Version=1.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
REG ADD HKLM\SOFTWARE\Classes\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /ve /d "%SystemRoot%\System32\mscoree.dll"
REG ADD HKLM\SOFTWARE\Classes\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "ThreadingModel" /d "Both"
REG ADD HKLM\SOFTWARE\Classes\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "Class" /d "Neumont.Tools.ORM.ORMCustomTool.ORMCustomTool"
REG ADD HKLM\SOFTWARE\Classes\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "CodeBase" /d "%NORMADir%\bin\Neumont.Tools.ORM.ORMCustomTool.dll"
REG ADD HKLM\SOFTWARE\Classes\CLSID\{977BD01E-F2B4-4341-9C47-459420624A20}\InprocServer32 /f /v "Assembly" /d "Neumont.Tools.ORM.ORMCustomTool, Version=1.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

:_AddRegGenerator
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\Generators\%~2\ORMCustomTool /f /ve /d "ORM Custom Tool"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\Generators\%~2\ORMCustomTool /f /v "CLSID" /d "{977BD01E-F2B4-4341-9C47-459420624A20}"
GOTO:EOF

:_AddXslORMGenerator
REG QUERY "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /v "OfficialName" 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (GOTO:EOF)
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "Type" /d "XSLT"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "OfficialName" /d "%~1"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "DisplayName" /d "%~2"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "DisplayDescription" /d "%~3"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "FileExtension" /d "%~4"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "SourceInputFormat" /d "%~5"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "ProvidesOutputFormat" /d "%~6"
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "TransformUri" /d "%~7"
GOTO:EOF

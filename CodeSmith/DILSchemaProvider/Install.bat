@ECHO OFF
SETLOCAL
IF "%~1"=="" (SET OutDir=bin\Debug) ELSE (SET OutDir=%~1.)
SET RootDir=%~dp0.

FOR /F "usebackq skip=3 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\CodeSmith\v3.2" /v "ApplicationDirectory"`) DO SET CodeSmithDir=%%~fB

XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\SchemaExplorer.DILSchemaProvider.dll" "%CodeSmithDir%\SchemaProviders\"
XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\SchemaExplorer.DILSchemaProvider.pdb" "%CodeSmithDir%\SchemaProviders\"
@ECHO OFF
SETLOCAL
IF "%~1"=="" (SET OutDir=bin\Debug) ELSE (SET OutDir=%~1.)
SET RootDir=%~dp0.

FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS" /v "ProductDir"`) DO SET VSDir=%%~fB

XCOPY /Y /D /V /Q "%RootDir%\OrmDatabaseImportTemplate.zip" "%VSDir%\Common7\IDE\ItemTemplates\CSharp\"

@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

XCOPY /Y /D /V /Q "%RootDir%\OrmDatabaseImportTemplate.zip" "%VSItemTemplatesDir%\CSharp\"

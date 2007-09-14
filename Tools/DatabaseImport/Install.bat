@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

FOR %%A IN ("%RootDir%\ProjectItems\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%VSItemTemplatesDir%\%%~nA\ORMModelFromDatabase.zip"
FOR %%A IN ("%RootDir%\ProjectItems\Web\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%VSItemTemplatesDir%\Web\%%~nA\ORMModelFromDatabase.zip"

@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF EXIST "%VSDir%" (
	FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%VSItemTemplatesDir%\%%~nA\ORMModelFromDatabase.zip"
	FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%\Web\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%VSItemTemplatesDir%\Web\%%~nA\ORMModelFromDatabase.zip"
)

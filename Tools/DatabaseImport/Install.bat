@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT "%ItemTemplatesInstallDir%"=="" (
	CALL:_MakeDir "%ItemTemplatesInstallDir%"
	FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%ItemTemplatesInstallDir%\%%~nA\ORMModelFromDatabase.zip"
	FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%\Web\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%ItemTemplatesInstallDir%\Web\%%~nA\ORMModelFromDatabase.zip"
)

GOTO:EOF

:_MakeDir
IF NOT EXIST "%~1" (MKDIR "%~1")
GOTO:EOF

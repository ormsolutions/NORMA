@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF "%VSSideBySide%"=="true" (
	GOTO:SxSInstall
	GOTO:EOF
)

IF NOT "%ItemTemplatesInstallDir%"=="" (
	CALL:_MakeDir "%ItemTemplatesInstallDir%"
	IF "%NORMAOfficial%"=="1" (
		FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%_Official\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%ItemTemplatesInstallDir%\%%~nA\ORMModelFromDatabase.zip"
		FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%_Official\Web\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%ItemTemplatesInstallDir%\Web\%%~nA\ORMModelFromDatabase.zip"
	) ELSE (
		FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%ItemTemplatesInstallDir%\%%~nA\ORMModelFromDatabase.zip"
		FOR %%A IN ("%RootDir%\ProjectItems\%TargetVisualStudioShortProductName%\Web\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%ItemTemplatesInstallDir%\Web\%%~nA\ORMModelFromDatabase.zip"
	)
)

GOTO:EOF

:SxSInstall
IF NOT EXIST "%VSIXInstallDir%" (MKDIR "%VSIXInstallDir%")

SET TargetBaseName=ORMSolutions.ORMArchitect.DatabaseImport.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%VSIXInstallDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%VSIXInstallDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.xml" "%VSIXInstallDir%\"

IF NOT EXIST "%VSIXInstallDir%\Xml\Transforms\Converters" (MKDIR "%VSIXInstallDir%\Xml\Transforms\Converters")
XCOPY /Y /D /V /Q "%RootDir%\..\..\ORMModel\Shell\Converters\DCILtoOIAL.xslt" "%VSIXInstallDir%\Xml\Transforms\Converters\"
XCOPY /Y /D /V /Q "%RootDir%\..\..\ORMModel\Shell\Converters\OIALtoORM.xslt" "%VSIXInstallDir%\Xml\Transforms\Converters\"
XCOPY /Y /D /V /Q "%RootDir%\..\..\ORMModel\Shell\Converters\CoreModelImport.xslt" "%VSIXInstallDir%\Xml\Transforms\Converters\"

::ItemTemplates are installed with the VSIXInstall\VSIXOnly project.
GOTO:EOF


:_MakeDir
IF NOT EXIST "%~1" (MKDIR "%~1")
GOTO:EOF

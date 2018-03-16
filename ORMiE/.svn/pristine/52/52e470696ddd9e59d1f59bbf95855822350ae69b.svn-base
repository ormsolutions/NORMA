@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat"
CALL "%RootDir%\..\..\Version.bat" %*

SET TargetBaseName=ORMInferenceEnginePackage.%TargetVisualStudioShortProductName%

IF NOT "%VSIXInstallDir%"=="" (
	CALL:_SETVAR "VSIXInstallDir" "%VSIXInstallDir%\..\..\..\unibz\ORM Inference\1.5"
)

if EXIST "%VSDir%" (
	IF "%VSIXInstallDir%"=="" (
		IF NOT EXIST "%NORMADir%\bin\Extensions\%TargetBaseName%.dll" (SET RunDevEnvSetup=True)
		IF EXIST "%NORMADir%\bin\Extensions\%TargetBaseName%.dll" (%RegPkg% /unregister "%NORMADir%\bin\Extensions\%TargetBaseName%.dll")
	)
)

CALL:_MakeDir "%NORMADir%\bin\Extensions"
CALL:_MakeDir "%NORMADir%\Xml\Schemas"
CALL:_MakeDir "%NORMADir%\Xml\Transforms\Converters"
CALL:_MakeDir "%ORMDir%\Schemas"
CALL:_MakeDir "%ORMDir%\Transforms"


IF "%VSIXInstallDir%"=="" (
	CALL:_SETVAR "FullTargetPath" "%NORMADir%\bin\Extensions\"
) ELSE (
	CALL:_SETVAR "FullTargetPath" "%VSIXInstallDir%\"
)

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%FullTargetPath%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%FullTargetPath%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\FaCT-1.6.2-OWLAPI-customv4.dll" "%FullTargetPath%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\IKVM*.dll" "%FullTargetPath%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\ikvm-native-win32-x64.dll" "%FullTargetPath%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\ikvm-native-win32-x32.dll" "%FullTargetPath%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\FaCT-1.6.2-OWLAPI-customv4.dll" "%VSEnvironmentDir%"

if %PROCESSOR_ARCHITECTURE%==x86 (
	echo f | XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\FaCTPlusPlusJNI-win32-x86.dll" "%VSEnvironmentDir%\FaCTPlusPlusJNI.dll"
    echo f | XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\FaCTPlusPlusJNI-win32-x86.dll" "%FullTargetPath%\FaCTPlusPlusJNI.dll"
 ) ELSE (
	echo f | XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\FaCTPlusPlusJNI-win32-x64.dll" "%VSEnvironmentDir%\FaCTPlusPlusJNI.dll"
    echo f | XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\FaCTPlusPlusJNI-win32-x64.dll" "%FullTargetPath%\FaCTPlusPlusJNI.dll"
 )

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\IKVM*.dll" "%VSEnvironmentDir%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\ikvm-native-win32-x64.dll" "%VSEnvironmentDir%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\ikvm-native-win32-x86.dll" "%VSEnvironmentDir%"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\owlapi-3.4.3.dll" "%VSEnvironmentDir%"

::XCOPY /Y /D /V /Q "%RootDir%\ORMInferenceEngine\%BuildOutDir%\%TargetBaseName%.xml" "%FullTargetPath%"
::XCOPY /Y /D /V /Q "%RootDir%\ORMInferenceEngine\ObjectModel\ORMInference.xsd" "%ORMDir%\Schemas\"
::XCOPY /Y /D /V /Q "%RootDir%\catalog.xml" "%ORMDir%\Schemas\"

::XCOPY /Y /D /V /Q "%RootDir%\ORMModel\Shell\catalog.xml" "%NORMADir%\Xml\Schemas\"


:: These 3 lines are just after SETLOCAL ENABLEDDELAYEDEXPANSIONS
::	ECHO F | XCOPY /Y /D /V /Q "%RootDir%\Setup\NORMASchemaCatalog.%TargetVisualStudioShortProductName%.xml" "%VSDir%\Xml\Schemas\NORMASchemaCatalog.xml"
::	XCOPY /Y /D /V /Q "%RootDir%\Setup\ORMSchemaCatalog.xml" "%VSDir%\Xml\Schemas\"
::	XCOPY /Y /D /V /Q "%RootDir%\Setup\DILSchemaCatalog.xml" "%VSDir%\Xml\Schemas\"

:: This line is at the end of the first block below
::		REG DELETE "HKLM\%VSRegistryRoot%\InstalledProducts\Natural ORM Architect" /v "UseRegNameAsSplashName" /f 1>NUL

:: These two lines are before the REG ADD in the second block below
::		XCOPY /Y /D /V /Q "%RootDir%\VSIXInstall\ORMDesignerIcon.png" "%VSIXInstallDir%\"
::		XCOPY /Y /D /V /Q "%RootDir%\VSIXInstall\ORMDesignerPreview.png" "%VSIXInstallDir%\"


if EXIST "%VSDir%" (
SETLOCAL ENABLEDELAYEDEXPANSION

	IF "%VSIXInstallDir%"=="" (
		%RegPkg% "%NORMADir%\bin\Extensions\%TargetBaseName%.dll"
	) ELSE (
		CALL:_MakeDir "%VSIXInstallDir%"
		XCOPY /Y /D /V /Q "%RootDir%\VSIXInstall\%TargetVisualStudioShortProductName%\extension.vsixmanifest" "%VSIXInstallDir%\"
		XCOPY /Y /D /V /Q "%RootDir%\VSIXInstall\%TargetVisualStudioShortProductName%\ORMInferenceEnginePackage.pkgdef" "%VSIXInstallDir%\"
		REG ADD "%VSRegistryConfigHive%\%VSRegistryConfigRootBase%\%VSRegistryRootVersion%%VSRegistryRootSuffix%\ExtensionManager\EnabledExtensions" /v "76d8e8ce-b44d-46b9-a37c-4e3094288830,%ProductMajorVersion%.%ProductMinorVersion%" /d "%VSIXInstallDir%\\" /f 1>NUL
	)


	IF /I "%RunDevEnvSetup%"=="True" (ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes... && "%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup)
)

REG ADD "%DesignerRegistryRoot%\Extensions\http://unibz.edu/NORMA/2013-05/ORMInference" /v "Class" /d "unibz.ORMInferenceEngine.ORMInferenceEngineDomainModel" /f 1>NUL
REG ADD "%DesignerRegistryRoot%\Extensions\http://unibz.edu/NORMA/2013-05/ORMInference" /v "CodeBase" /d "%FullTargetPath%%TargetBaseName%.dll" /f 1>NUL
REG ADD "%DesignerRegistryRoot%\Extensions\http://unibz.edu/NORMA/2013-05/ORMInference" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8eca31422181c479" /f 1>NUL


GOTO:EOF

:_MakeDir
IF NOT EXIST "%~1" (MKDIR "%~1")
GOTO:EOF

:_MakeDirCopy
IF NOT EXIST "%~1" (
	MKDIR "%~1"
	IF EXIST "%~2" (
		XCOPY /S /Q "%~2" "%~1"
	)
)
GOTO:EOF

:_RemoveDir
IF EXIST "%~1" (RMDIR /S /Q "%~1")
GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

:_SETVAR
SET %~1=%~2
GOTO:EOF

:SetFromRegistry
:: Parameters:
::   %~1 = Environment variable name
::   %~2 = Registry key name
::   %~3 = Registry value name
::   %~4 = FOR variable modifier
IF NOT DEFINED %~1 FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "%~2" /v "%~3"`) DO SET %~1=%%~%~4B
GOTO:EOF
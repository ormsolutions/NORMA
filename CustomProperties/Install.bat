@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")
IF NOT EXIST "%NORMADir%\Xml\Verbalization\CustomProperties" (MKDIR "%NORMADir%\Xml\Verbalization\CustomProperties")

CALL:_CleanupFile "%NORMAExtensionsDir%\Neumont.Tools.ORM.CustomProperties.dll"
CALL:_CleanupFile "%NORMAExtensionsDir%\Neumont.Tools.ORM.CustomProperties.pdb"

SET TargetBaseName=Neumont.Tools.ORM.CustomProperties.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\VerbalizationSnippets\*.x??" "%NORMADir%\Xml\Verbalization\CustomProperties\"
IF EXIST "%NORMADir%\Xml\Verbalization\CustomProperties\CustomPropertyVerbalizationSnippets.xml" (
	IF EXIST "%NORMADir%\Xml\Verbalization\CustomProperties\_default.xml" (DEL /F /Q "%NORMADir%\Xml\Verbalization\CustomProperties\_default.xml")
	REN "%NORMADir%\Xml\Verbalization\CustomProperties\CustomPropertyVerbalizationSnippets.xml" "_default.xml"
)

REG DELETE "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Preview/CustomProperties" /f 1>NUL 2>&1

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "Class" /d "Neumont.Tools.ORM.CustomProperties.CustomPropertiesDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
:: REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "AutoLoadNamespace" /t REG_DWORD /d "1" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF
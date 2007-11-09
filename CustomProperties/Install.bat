@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")
IF NOT EXIST "%NORMADir%\Xml\Verbalization\CustomProperties" (MKDIR "%NORMADir%\Xml\Verbalization\CustomProperties")

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORM.CustomProperties.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORM.CustomProperties.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\VerbalizationSnippets\*.x??" "%NORMADir%\Xml\Verbalization\CustomProperties\"
IF EXIST "%NORMADir%\Xml\Verbalization\CustomProperties\CustomPropertyVerbalizationSnippets.xml" (
	IF EXIST "%NORMADir%\Xml\Verbalization\CustomProperties\_default.xml" (DEL /F /Q "%NORMADir%\Xml\Verbalization\CustomProperties\_default.xml")
	REN "%NORMADir%\Xml\Verbalization\CustomProperties\CustomPropertyVerbalizationSnippets.xml" "_default.xml"
)

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "Class" /d "Neumont.Tools.ORM.CustomProperties.CustomPropertiesDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "CodeBase" /d "%NORMAExtensionsDir%\Neumont.Tools.ORM.CustomProperties.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "Assembly" /d "Neumont.Tools.ORM.CustomProperties, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
:: REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions" /v "AutoLoadNamespaces" /t REG_MULTI_SZ /d "http://schemas.neumont.edu/ORM/2007-11/CustomProperties\0" /f 1>NUL
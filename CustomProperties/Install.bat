@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")
IF NOT EXIST "%NORMADir%\Xml\Verbalization\CustomProperties" (MKDIR "%NORMADir%\Xml\Verbalization\CustomProperties")

SET TargetBaseName=ORMSolutions.ORMArchitect.CustomProperties.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\VerbalizationSnippets\*.x??" "%NORMADir%\Xml\Verbalization\CustomProperties\"
IF EXIST "%NORMADir%\Xml\Verbalization\CustomProperties\CustomPropertyVerbalizationSnippets.xml" (
	IF EXIST "%NORMADir%\Xml\Verbalization\CustomProperties\_default.xml" (DEL /F /Q "%NORMADir%\Xml\Verbalization\CustomProperties\_default.xml")
	REN "%NORMADir%\Xml\Verbalization\CustomProperties\CustomPropertyVerbalizationSnippets.xml" "_default.xml"
)

REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "Class" /d "ORMSolutions.ORMArchitect.CustomProperties.CustomPropertiesDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
:: REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-11/CustomProperties" /v "AutoLoadNamespace" /t REG_DWORD /d "1" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF
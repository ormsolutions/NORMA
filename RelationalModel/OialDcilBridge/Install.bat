@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.OialDcilBridge.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.OialDcilBridge.pdb" "%NORMAExtensionsDir%\"

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase" /v "Class" /d "Neumont.Tools.OialDcilBridge.OialDcilBridgeDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase" /v "CodeBase" /d "%NORMAExtensionsDir%\Neumont.Tools.OialDcilBridge.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase" /v "Assembly" /d "Neumont.Tools.OialDcilBridge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
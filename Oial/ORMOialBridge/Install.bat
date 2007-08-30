@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMToORMAbstractionBridge.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMToORMAbstractionBridge.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMToORMAbstractionBridge.xml" "%NORMAExtensionsDir%\"

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" /v "Class" /d "Neumont.Tools.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" /v "CodeBase" /d "%NORMAExtensionsDir%\Neumont.Tools.ORMToORMAbstractionBridge.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" /v "Assembly" /d "Neumont.Tools.ORMToORMAbstractionBridge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMOialBridge.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMOialBridge.pdb" "%NORMAExtensionsDir%\"

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-06/ORMOialBridge" /v "Class" /d "Neumont.Tools.ORMOialBridge.ORMOialBridgeDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-06/ORMOialBridge" /v "CodeBase" /d "%NORMAExtensionsDir%\Neumont.Tools.ORMOialBridge.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-06/ORMOialBridge" /v "Assembly" /d "Neumont.Tools.ORMOialBridge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

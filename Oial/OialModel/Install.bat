@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMAbstraction.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.ORMAbstraction.pdb" "%NORMAExtensionsDir%\"

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core" /v "Class" /d "Neumont.Tools.ORMAbstraction.AbstractionDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core" /v "CodeBase" /d "%NORMAExtensionsDir%\Neumont.Tools.ORMAbstraction.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core" /v "Assembly" /d "Neumont.Tools.ORMAbstraction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
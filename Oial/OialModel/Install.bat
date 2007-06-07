@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.Oial.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.Oial.pdb" "%NORMAExtensionsDir%\"

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-01/OialModel" /v "Class" /d "Neumont.Tools.Oial.OialDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-01/OialModel" /v "CodeBase" /d "%NORMAExtensionsDir%\Neumont.Tools.Oial.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-01/OialModel" /v "Assembly" /d "Neumont.Tools.Oial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
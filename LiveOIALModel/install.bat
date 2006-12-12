@ECHO OFF
SETLOCAL
IF "%~1"=="" (SET OutDir=bin\Debug) ELSE (SET OutDir=%~1.)
SET RootDir=%~dp0.
SET ExtensionsDir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio\bin\Extensions

IF NOT EXIST "%ExtensionsDir%" (MKDIR "%ExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\Neumont.Tools.ORM.TestOIALModel.dll" "%ExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\Neumont.Tools.ORM.TestOIALModel.pdb" "%ExtensionsDir%\"

REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2006-01/TestOIALModel" /v "Class" /d "Neumont.Tools.ORM.TestOIALModel.OIALDomainModel" /f 1>NUL
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2006-01/TestOIALModel" /v "CodeBase" /d "%ExtensionsDir%\Neumont.Tools.ORM.TestOIALModel.dll" /f 1>NUL
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2006-01/TestOIALModel" /v "Assembly" /d "Neumont.Tools.ORM.TestOIALModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

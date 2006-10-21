@ECHO OFF
SETLOCAL
IF "%~1"=="" (SET OutDir=bin\Debug) ELSE (SET OutDir=%~1.)
SET RootDir=%~dp0.
SET ExtensionsDir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio\bin\Extensions

IF NOT EXIST "%ExtensionsDir%" (MKDIR "%ExtensionsDir%")

XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\Neumont.Tools.ORM.CustomProperties.dll" "%ExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\Neumont.Tools.ORM.CustomProperties.pdb" "%ExtensionsDir%\"

REG DELETE "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.orm.net/ORM/CustomProperties" /f 1>NUL 2>&1
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Preview/CustomProperties" /v "Class" /d "Neumont.Tools.ORM.CustomProperties.CustomPropertiesDomainModel" /f 1>NUL
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Preview/CustomProperties" /v "CodeBase" /d "%ExtensionsDir%\Neumont.Tools.ORM.CustomProperties.dll" /f 1>NUL
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Preview/CustomProperties" /v "Assembly" /d "Neumont.Tools.ORM.CustomProperties, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL
:: REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions" /v "AutoLoadNamespaces" /t REG_MULTI_SZ /d "http://schemas.neumont.edu/ORM/Preview/CustomProperties\0" /f 1>NUL

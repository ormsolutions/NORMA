@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

SET TargetBaseName=Neumont.Tools.ORM.OIALModel.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"

REM REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/2006-01/OIALModel" /v "Class" /d "Neumont.Tools.ORM.OIALModel.OIALDomainModel" /f 1>NUL
REM REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/2006-01/OIALModel" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REM REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/2006-01/OIALModel" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

SET TargetBaseName=ORMSolutions.ORMArchitect.Views.RelationalView.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"

IF "%VSSideBySide%"=="true" GOTO:EOF

REG DELETE "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/Views/RelationalView" /f 1>NUL 2>&1

REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/2007-11/RelationalView" /v "Class" /d "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalShapeDomainModel" /f 1>NUL
REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/2007-11/RelationalView" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/2007-11/RelationalView" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=%NORMAPublicKeyToken%" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

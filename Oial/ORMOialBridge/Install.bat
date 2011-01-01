@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

SET TargetBaseName=ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.xml" "%NORMAExtensionsDir%\"

REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" /v "Class" /d "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel" /f 1>NUL
REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REG ADD "%DesignerRegistryRoot%\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

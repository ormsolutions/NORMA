@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

SET TargetBaseName=ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.xml" "%NORMAExtensionsDir%\"

REG DELETE "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2008-04/ORMAbstractionToBarkerERBridge" /f 1>NUL 2>&1

REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER" /v "Class" /d "ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\ORM Solutions\Natural ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF
@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%NORMAExtensionsDir%" (MKDIR "%NORMAExtensionsDir%")

CALL:_CleanupFile "%NORMAExtensionsDir%\Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.dll"
CALL:_CleanupFile "%NORMAExtensionsDir%\Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.pdb"
CALL:_CleanupFile "%NORMAExtensionsDir%\Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.xml"

SET TargetBaseName=Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.%TargetVisualStudioShortProductName%

XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.dll" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.pdb" "%NORMAExtensionsDir%\"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\%TargetBaseName%.xml" "%NORMAExtensionsDir%\"

REG DELETE "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/2007-01/AbstractionToRelationalBridge" /f 1>NUL 2>&1

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase" /v "Class" /d "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase" /v "CodeBase" /d "%NORMAExtensionsDir%\%TargetBaseName%.dll" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect\Extensions\http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase" /v "Assembly" /d "%TargetBaseName%, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /f 1>NUL

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF
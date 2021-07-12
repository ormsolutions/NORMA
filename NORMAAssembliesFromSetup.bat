@ECHO OFF
SETLOCAL EnableDelayedExpansion

if '%~1'=='?' GOTO:_Instructions
if '%~1'=='-?' GOTO:_Instructions
if '%~1'=='/?' GOTO:_Instructions
if '%~1'=='-help' GOTO:_Instructions
if '%~1'=='/help' GOTO:_Instructions
if '%~1'=='-Help' GOTO:_Instructions
if '%~1'=='/Help' GOTO:_Instructions
if '%~1'=='-HELP' GOTO:_Instructions
if '%~1'=='/HELP' GOTO:_Instructions
if '%TargetVisualStudioVersion%'=='' (
	ECHO Please designate your Visual Studio version by running one of the VS20xx.bat files to set the TargetVisualStudioVersion environment variable in this command prompt.
	GOTO:EOF
)
SET SourceRoot=%~dp0
SET BuildOutputDir=\bin\Debug
CALL "%SourceRoot%SetupEnvironment.bat"

IF NOT DEFINED NORMADir (
	GOTO:EOF
)

IF "%VSSideBySide%"=="true" (
	:: Side by side installation, find and load private registry file. This reads the files from the Exp installation
	REG LOAD HKLM\VSHive%TargetVisualStudioInstallSuffix%Exp "%LocalAppData%\Microsoft\VisualStudio\%TargetVisualStudioMajorMinorVersion%_%TargetVisualStudioInstallSuffix%Exp\privateregistry.bin" 1>NUL 2>&1

	FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\VSHive%TargetVisualStudioInstallSuffix%Exp\Software\Microsoft\VisualStudio\%TargetVisualStudioMajorMinorVersion%_%TargetVisualStudioInstallSuffix%Exp_Config\Packages\{efddc549-1646-4451-8a51-e5a5e94d647c}" /v "CodeBase"`) DO CALL:_GetDirectory "%%B" NORMAInstallRoot
	REG UNLOAD HKLM\VSHive%TargetVisualStudioInstallSuffix%Exp 1>NUL 2>&1

	IF "!NORMAInstallRoot!"=="" (
		ECHO NORMA is not installed in the Experimental Visual Studio instance.
		GOTO:EOF
	)
)

CALL:_MakeDir "%SourceRoot%AlternateViews\BarkerERView%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%AlternateViews\RelationalView%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%CustomProperties%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%EntityRelationship\BarkerErModel%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%EntityRelationship\OialBerBridge%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%Oial\OialModel%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%Oial\ORMOialBridge%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%ORMModel%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%RelationalModel\DcilModel%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%RelationalModel\OialDcilBridge%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%Tools\DatabaseImport%BuildOutputDir%"
CALL:_MakeDir "%SourceRoot%Tools\ORMCustomTool%BuildOutputDir%"


IF NOT "%VSSideBySide%"=="true" (
	GOTO:_NotSideBySide
)

::Copy files. It is likely you have a dev build of these files already so this does not do a timestamp check with /D.
XCOPY /Y /V /Q "%NORMAInstallRoot%ORMSolutions.ORMArchitect.Core.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%ORMModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%ORMSolutions.ORMArchitect.DatabaseImport.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Tools\DatabaseImport%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%ORMSolutions.ORMArchitect.ORMCustomTool.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Tools\ORMCustomTool%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.CustomProperties.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%CustomProperties%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.EntityRelationshipModels.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%EntityRelationship\BarkerErModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.ORMAbstraction.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Oial\OialModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%EntityRelationship\OialBerBridge%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%RelationalModel\OialDcilBridge%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Oial\ORMOialBridge%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.RelationalModels.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%RelationalModel\DcilModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.Views.BarkerERView.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%AlternateViews\BarkerERView%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAInstallRoot%Extensions\ORMSolutions.ORMArchitect.Views.RelationalView.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%AlternateViews\RelationalView%BuildOutputDir%\"
GOTO:EOF

:_NotSideBySide
XCOPY /Y /V /Q "%NORMABinDir%\ORMSolutions.ORMArchitect.Core.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%ORMModel%BuildOutputDir%\"
::Ignore database import. It is GAC installed and won't generally be referenced regardless
::XCOPY /Y /V /Q "%NORMABinDir%ORMSolutions.ORMArchitect.DatabaseImport.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%%Tools\DatabaseImport%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMABinDir%\ORMSolutions.ORMArchitect.ORMCustomTool.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Tools\ORMCustomTool%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.CustomProperties.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%CustomProperties%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.EntityRelationshipModels.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%EntityRelationship\BarkerErModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.ORMAbstraction.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Oial\OialModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%EntityRelationship\OialBerBridge%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%RelationalModel\OialDcilBridge%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%Oial\ORMOialBridge%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.RelationalModels.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%RelationalModel\DcilModel%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.Views.BarkerERView.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%AlternateViews\BarkerERView%BuildOutputDir%\"
XCOPY /Y /V /Q "%NORMAExtensionDir%\ORMSolutions.ORMArchitect.Views.RelationalView.%TargetVisualStudioShortProductName%.dll" "%SourceRoot%AlternateViews\RelationalView%BuildOutputDir%\"
GOTO:EOF

:_GetDirectory
SET %2=%~dp1
GOTO:EOF

:_MakeDir
IF NOT EXIST "%~1" (MKDIR "%~1")
GOTO:EOF

:_Instructions
ECHO This file is a tool for extension developers who need to test their
ECHO extensions against the official version of NORMA. The official version
ECHO is built with a private signing key, which means that the regular
ECHO instructions to build NORMA then build your extension projects will no
ECHO longer work. With the introduction of side-by-side Visual Studio versions
ECHO in VS2017, extension projects should all reference NORMA files in the
ECHO built locations in the tree. This batch file will locate the
ECHO setup-installed NORMA assemblies and copy them into their build locations.
@ECHO(
ECHO The expectation is that the official NORMA product has already been installed
ECHO into the experimental Visual Studio installation, which can be accessed by
ECHO launching devenv with a rootsuffix argument of Exp. For side-by-side
ECHO installations the Exp registry hive will be read to locate the NORMA files and
ECHO back-copy them into the designated build locations. For earlier versions the
ECHO standard install location is used.
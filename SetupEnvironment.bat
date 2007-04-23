@ECHO OFF

IF "%~1"=="" (SET BuildOutDir=bin\Debug) ELSE (SET BuildOutDir=%~1.)

IF NOT DEFINED FrameworkSDKDir (CALL:_SetupVsVars)

:: Normally, the next line would have parentheses around the command portion. However, it is possible
:: for there to be parentheses in the %ProgramFiles% path (and there are by default on x64), which
:: causes a syntax error. Therefore, we leave the parentheses off here.
IF NOT DEFINED MSBuildExtensionsPath SET MSBuildExtensionsPath=%ProgramFiles%\MSBuild

SET TrunkDir=%~dp0.
SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio
SET NORMAExtensionsDir=%NORMADir%\bin\Extensions
SET ORMDir=%CommonProgramFiles%\Neumont\ORM
SET DILDir=%CommonProgramFiles%\Neumont\DIL
SET PLiXDir=%CommonProgramFiles%\Neumont\PLiX
SET ORMTransformsDir=%ORMDir%\Transforms
SET DILTransformsDir=%DILDir%\Transforms

SET VSRegistryRootBase=SOFTWARE\Microsoft\VisualStudio
:: VSRegistryRootVersion settings:
::   8.0 = Visual Studio 2005 (Code Name "Whidbey")
::   9.0 = Visual Studio Code Name "Orcas"
SET VSRegistryRootVersion=8.0
SET VSRegistryRootSuffix=Exp
SET VSRegistryRoot=%VSRegistryRootBase%\%VSRegistryRootVersion%%VSRegistryRootSuffix%

FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRoot%\Setup\VS" /v "EnvironmentPath"`) DO SET VSEnvironmentPath=%%~fB
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRoot%\Setup\VS" /v "ProductDir"`) DO SET VSDir=%%~fB
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRoot%\VSTemplate\Item" /v "UserFolder"`) DO SET VSItemTemplatesDir=%%~fB
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%\VSIP\8.0" /v "InstallDir"`) DO SET VSIPDir=%%~fB
SET RegPkg="%VSIPDir%\VisualStudioIntegration\Tools\Bin\regpkg.exe" /root:"%VSRegistryRoot%"

GOTO:EOF


:_SetupVsVars
:: Set up the build environment to use Orcas if available, falling back to 2005 otherwise.
IF DEFINED VS90COMNTOOLS (CALL "%VS90COMNTOOLS%\vsvars32.bat") ELSE (CALL "%VS80COMNTOOLS%\vsvars32.bat")
GOTO:EOF

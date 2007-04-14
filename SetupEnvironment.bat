@ECHO OFF

IF "%~1"=="" (SET BuildOutDir=bin\Debug) ELSE (SET BuildOutDir=%~1.)

IF NOT DEFINED FrameworkSDKDir (CALL "%VS80COMNTOOLS%\vsvars32.bat")
IF NOT DEFINED MSBuildExtensionsPath (SET MSBuildExtensionsPath=%ProgramFiles%\MSBuild)

SET TrunkDir=%~dp0.
SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio
SET NORMAExtensionsDir=%NORMADir%\bin\Extensions
SET ORMDir=%CommonProgramFiles%\Neumont\ORM
SET DILDir=%CommonProgramFiles%\Neumont\DIL
SET PLiXDir=%CommonProgramFiles%\Neumont\PLiX
SET ORMTransformsDir=%ORMDir%\Transforms
SET DILTransformsDir=%DILDir%\Transforms

SET VSRegistryRootBase=SOFTWARE\Microsoft\VisualStudio
SET VSRegistryRootSuffix=Exp
SET VSRegistryRoot=%VSRegistryRootBase%\8.0%VSRegistryRootSuffix%

FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRoot%\Setup\VS" /v "EnvironmentPath"`) DO SET VSEnvironmentPath=%%~fB
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRoot%\Setup\VS" /v "ProductDir"`) DO SET VSDir=%%~fB
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRoot%\VSTemplate\Item" /v "UserFolder"`) DO SET VSItemTemplatesDir=%%~fB
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%\VSIP\8.0" /v "InstallDir"`) DO SET VSIPDir=%%~fB
SET RegPkg="%VSIPDir%\VisualStudioIntegration\Tools\Bin\regpkg.exe" /root:"%VSRegistryRoot%"

GOTO:EOF


@ECHO OFF

IF "%~1"=="" (SET BuildOutDir=bin\Debug) ELSE (SET BuildOutDir=%~1.)

:: If we aren't running under a 32-bit command prompt, bail out.
IF /I NOT "%PROCESSOR_ARCHITECTURE%"=="x86" (ECHO These build scripts must be run under a 32-bit command prompt. && PAUSE && EXIT)

:: TargetVisualStudioVersion settings:
::   v8.0 = Visual Studio 2005 (Code Name "Whidbey")
::   v9.0 = Visual Studio 2008 (Code Name "Orcas")
IF NOT DEFINED TargetVisualStudioVersion (SET TargetVisualStudioVersion=v8.0)

CALL:_SetupVersionVars_%TargetVisualStudioVersion%

IF NOT DEFINED FrameworkDir (CALL:_SetupVsVars)

:: Normally, the next few lines would have parentheses around the command portion. However, it is possible
:: for there to be parentheses in the %ProgramFiles% path (and there are by default on x64), which
:: causes a syntax error. Therefore, we leave the parentheses off here.
IF NOT DEFINED MSBuildExtensionsPath SET MSBuildExtensionsPath=%ProgramFiles%\MSBuild
IF NOT DEFINED TrunkDir SET TrunkDir=%~dp0.
IF NOT DEFINED NORMADir SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio
IF NOT DEFINED NORMAExtensionsDir SET NORMAExtensionsDir=%NORMADir%\bin\Extensions
IF NOT DEFINED ORMDir SET ORMDir=%CommonProgramFiles%\Neumont\ORM
IF NOT DEFINED DILDir SET DILDir=%CommonProgramFiles%\Neumont\DIL
IF NOT DEFINED PLiXDir SET PLiXDir=%CommonProgramFiles%\Neumont\PLiX
IF NOT DEFINED ORMTransformsDir SET ORMTransformsDir=%ORMDir%\Transforms
IF NOT DEFINED DILTransformsDir SET DILTransformsDir=%DILDir%\Transforms

IF NOT DEFINED VSRegistryRootBase (SET VSRegistryRootBase=SOFTWARE\Microsoft\VisualStudio)
IF NOT DEFINED VSRegistryRootVersion (SET VSRegistryRootVersion=%TargetVisualStudioMajorMinorVersion%)
:: Remove the value "Exp" on the next line if you want installations to be performed
:: against the regular (non-Experimental) Visual Studio registry hive.
IF NOT DEFINED VSRegistryRootSuffix (SET VSRegistryRootSuffix=Exp)
IF NOT DEFINED VSRegistryRoot (SET VSRegistryRoot=%VSRegistryRootBase%\%VSRegistryRootVersion%%VSRegistryRootSuffix%)

CALL "%TrunkDir%\SetFromRegistry.bat" "VSEnvironmentPath" "HKLM\%VSRegistryRoot%\Setup\VS" "EnvironmentPath" "f"
CALL "%TrunkDir%\SetFromRegistry.bat" "VSDir" "HKLM\%VSRegistryRoot%\Setup\VS" "ProductDir" "f"
CALL "%TrunkDir%\SetFromRegistry.bat" "VSItemTemplatesDir" "HKLM\%VSRegistryRoot%\VSTemplate\Item" "UserFolder" "f"
CALL "%TrunkDir%\SetFromRegistry.bat" "VSIPDir" "HKLM\%VSRegistryRootBase%\VSIP\%VSRegistryRootVersion%" "InstallDir" "f"
IF NOT DEFINED RegPkg (SET RegPkg="%VSIPDir%\VisualStudioIntegration\Tools\Bin\regpkg.exe" /root:"%VSRegistryRoot%")

GOTO:EOF


:_SetupVsVars
:: Set up the build environment.
CALL "%%VS%TargetVisualStudioMajorMinorVersion:.=%COMNTOOLS%%\vsvars32.bat"
GOTO:EOF


:_SetupVersionVars_v8.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v2.0)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=8.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=8.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=2.0.0.0)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=8.2.0.0)
GOTO:EOF

:_SetupVersionVars_v9.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v3.5)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=9.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=9.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=3.5.0.0)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=9.0.0.0)
GOTO:EOF

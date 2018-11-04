@ECHO OFF

IF "%~1"=="" (SET BuildOutDir=bin\Debug) ELSE (SET BuildOutDir=%~1.)

IF "%ProgramFiles(X86)%"=="" (
	SET ResolvedProgramFiles=%ProgramFiles%
	SET ResolvedCommonProgramFiles=%CommonProgramFiles%
	SET WOWRegistryAdjust=
) ELSE (
	CALL:SET6432
)

:: TargetVisualStudioVersion settings:
::   v8.0 = Visual Studio 2005 (Code Name "Whidbey")
::   v9.0 = Visual Studio 2008 (Code Name "Orcas")
::  v10.0 = Visual Studio 2010 (Code Name "Rosario")
::  v11.0 = Visual Studio 2012
::  v12.0 = Visual Studio 2013
::  v14.0 = Visual Studio 2015
::  v15.0 = Visual Studio 2017
IF NOT DEFINED TargetVisualStudioVersion (SET TargetVisualStudioVersion=v8.0)

:: Remove the value "Exp" on the next line if you want installations to be performed
:: against the regular (non-Experimental) Visual Studio registry hive.
IF NOT DEFINED VSRegistryRootSuffix (SET VSRegistryRootSuffix=Exp)

CALL:_SetupVersionVars_%TargetVisualStudioVersion%

IF NOT DEFINED FrameworkDir (CALL:_SetupVsVars)

:: Normally, the next few lines would have parentheses around the command portion. However, it is possible
:: for there to be parentheses in the %ProgramFiles% path (and there are by default on x64), which
:: causes a syntax error. Therefore, we leave the parentheses off here.
IF NOT DEFINED TrunkDir SET TrunkDir=%~dp0.
IF NOT DEFINED NORMADir SET NORMADir=%ResolvedProgramFiles%\ORM Solutions\ORM Architect for %TargetVisualStudioLongProductName%
IF NOT DEFINED OldNORMADir SET OldNORMADir=%ResolvedProgramFiles%\Neumont\ORM Architect for %TargetVisualStudioLongProductName%
IF NOT DEFINED NORMAExtensionsDir SET NORMAExtensionsDir=%NORMADir%\bin\Extensions
IF NOT DEFINED ORMDir SET ORMDir=%ResolvedCommonProgramFiles%\Neumont\ORM
IF NOT DEFINED DILDir SET DILDir=%ResolvedCommonProgramFiles%\Neumont\DIL
IF NOT DEFINED PLiXDir SET PLiXDir=%ResolvedCommonProgramFiles%\Neumont\PLiX
IF NOT DEFINED ORMTransformsDir SET ORMTransformsDir=%ORMDir%\Transforms
IF NOT DEFINED DILTransformsDir SET DILTransformsDir=%DILDir%\Transforms

IF NOT DEFINED VSRegistryRootBase (SET VSRegistryRootBase=SOFTWARE%WOWRegistryAdjust%\Microsoft\VisualStudio)
IF NOT DEFINED VSRegistryRootVersion (SET VSRegistryRootVersion=%TargetVisualStudioMajorMinorVersion%)
IF NOT DEFINED VSRegistryRoot (SET VSRegistryRoot=%VSRegistryRootBase%\%VSRegistryRootVersion%%VSRegistryRootSuffix%)
IF NOT DEFINED VSRegistryConfigRootBase (
	IF "%VSRegistryConfigHive%"=="HKCU" (
		SET VSRegistryConfigRootBase=SOFTWARE\Microsoft\VisualStudio
	) ELSE (
		SET VSRegistryConfigRootBase=%VSRegistryRootBase%
	)
)
IF NOT DEFINED VSRegistryConfigRoot (SET VSRegistryConfigRoot=%VSRegistryConfigRootBase%\%VSRegistryRootVersion%%VSRegistryRootSuffix%%VSRegistryConfigDecorator%)

IF NOT "%VSSideBySide%"=="1" (
	CALL "%TrunkDir%\SetFromRegistry.bat" "VSEnvironmentPath" "%VSRegistryConfigHive%\%VSRegistryConfigRoot%\Setup\VS" "EnvironmentPath" "f"
	CALL "%TrunkDir%\SetFromRegistry.bat" "VSEnvironmentDir" "%VSRegistryConfigHive%\%VSRegistryConfigRoot%\Setup\VS" "EnvironmentDirectory" "f"
	CALL "%TrunkDir%\SetFromRegistry.bat" "VSDir" "%VSRegistryConfigHive%\%VSRegistryConfigRoot%\Setup\VS" "ProductDir" "f"
	CALL "%TrunkDir%\SetFromRegistry.bat" "VSItemTemplatesDir" "%VSRegistryConfigHive%\%VSRegistryConfigRoot%\VSTemplate\Item" "UserFolder" "f"
) ELSE (
	if NOT EXIST "%~dp0%TargetVisualStudioShortProductName%Installation.bat" (
		ECHO %TargetVisualStudioLongProductName% supports side-by-side installations of the
		ECHO Visual Studio product. The NORMA build systems needs to know which of these
		ECHO installations to target.
		ECHO(
		ECHO Please create %TargetVisualStudioShortProductName%Installation.bat to set the TargetVisualStudioEdition
		ECHO and TargetVisualStudioInstallSuffix environment variables.
		ECHO(
		ECHO The installed editions are:
		dir /b "%ResolvedProgramFiles%\Microsoft Visual Studio\%TargetVisualStudioLongProductYear%"
		ECHO(
		ECHO The installed suffixes are the 8 characters after '%TargetVisualStudioMajorMinorVersion%_' and
		ECHO before any recognizable suffix like 'Exp' in:
		dir /b /ad "%LocalAppData%\Microsoft\VisualStudio\%TargetVisualStudioMajorMinorVersion%_*"
		GOTO:EOF
	)
	CALL "%TrunkDir%\%TargetVisualStudioShortProductName%Installation"

	IF NOT DEFINED TargetVisualStudioEdition (
		ECHO The 'TargetVisualStudioEdition' environment variable must be defined.
		GOTO:EOF
	)

	IF NOT DEFINED TargetVisualStudioInstallSuffix (
		ECHO The 'TargetVisualStudioInstallSuffix' environment variable must be defined.
		GOTO:EOF
	)
)
IF "%VSSideBySide%"=="1" (
	CALL:SETVAR "VSDir" "%ResolvedProgramFiles%\Microsoft Visual Studio\%TargetVisualStudioLongProductYear%\%TargetVisualStudioEdition%\"
)
IF "%VSSideBySide%"=="1" (
	CALL:SETVAR "VSEnvironmentDir" "%VSDir%Common7\IDE\"
	IF NOT DEFINED MSBuildExtensionsPath CALL:SETVAR "MSBuildExtensionsPath" "%VSDir%MSBuild"
) ELSE (
	IF NOT DEFINED MSBuildExtensionsPath CALL:SETVAR "MSBuildExtensionsPath" "%ResolvedProgramFiles%\MSBuild"
)
IF "%VSSideBySide%"=="1" (
	CALL:SETVAR "VSEnvironmentPath" "%VSEnvironmentDir%devenv.exe"
	CALL:SETVAR "VSItemTemplatesDir" "%VSEnvironmentDir%ItemTemplates"
	CALL:SETVAR "VSIPDir" "%VSDir%VSSDK\"
)
IF NOT DEFINED LocalAppData SET LocalAppData=%UserProfile%\Local Settings\Application Data
IF NOT "%VSIXExtensionDir%"=="" (
	IF "%VSSideBySide%"=="1" (
		CALL:SETVAR "VSIXInstallDir" "%LocalAppData%\Microsoft\VisualStudio\%TargetVisualStudioMajorMinorVersion%_%TargetVisualStudioInstallSuffix%%VSRegistryRootSuffix%\%VSIXExtensionDir%"
	) ELSE (
		IF "%VSRegistryRootSuffix%"=="" (
			CALL:SETVAR "VSIXInstallDir" "%VSEnvironmentDir%\%VSIXExtensionDir%"
		) ELSE (
			CALL:SETVAR "VSIXInstallDir" "%LocalAppData%\Microsoft\VisualStudio\%VSRegistryRootVersion%%VSRegistryRootSuffix%\%VSIXExtensionDir%"
		)
	)
) ELSE (
	SET VSIXInstallDir=
)
IF "%VSIXInstallDir%"=="" (
	CALL:SETVAR "ItemTemplatesInstallDir" "%VSItemTemplatesDir%"
	CALL:SETVAR "DesignerRegistryRoot" "%VSRegistryConfigHive%\%VSRegistryConfigRoot%\ORM Solutions\Natural ORM Architect"
) ELSE (
	CALL:SETVAR "ItemTemplatesInstallDir" "%VSIXInstallDir%\ItemTemplates"
	CALL:SETVAR "DesignerRegistryRoot" "HKLM\Software%WOWRegistryAdjust%\ORM Solutions\Natural ORM Architect for %TargetVisualStudioLongProductName%\Designer"
)
IF ERRORLEVEL 1 %COMSPEC% /c

IF NOT "%VSSideBySide%"=="1" (
	CALL "%TrunkDir%\SetFromRegistry.bat" "VSIPDir" "HKLM\%VSRegistryRootBase%\VSIP\%VSRegistryRootVersion%" "InstallDir" "f"
	:: Fallback, enable building 8.0 without an 8.0 installation
	CALL "%TrunkDir%\SetFromRegistry.bat" "VSIPDir" "HKLM\%VSRegistryRootBase%\VSIP\9.0" "InstallDir" "f"
)

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
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2005)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=05)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2005)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2005)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=8.2.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=2.0)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=2.0.0.0)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKLM)
GOTO:EOF

:_SetupVersionVars_v9.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v3.5)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=9.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=9.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=3.5.0.0)
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2008)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=08)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2008)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2008)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=9.0.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=3.5)
IF NOT DEFINED ProjectToolsAssemblySuffix (SET ProjectToolsAssemblySuffix=.v3.5)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=3.5.0.0)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKLM)
GOTO:EOF

:_SetupVersionVars_v10.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v4.0)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=10.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=10.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=4.0.0.0)
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2010)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=10)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2010)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2010)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=10.0.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=4.0)
IF NOT DEFINED ProjectToolsAssemblySuffix (SET ProjectToolsAssemblySuffix=.v4.0)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=4.0.0.0)
IF NOT DEFINED VSRegistryConfigDecorator (SET VSRegistryConfigDecorator=_Config)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKCU)
IF NOT DEFINED VSIXExtensionDir (SET VSIXExtensionDir=Extensions\ORM Solutions\Natural ORM Architect\1.0)
GOTO:EOF

:_SetupVersionVars_v11.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v4.5)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=11.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=11.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=4.5.0.0)
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2012)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=12)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2012)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2012)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=11.0.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=4.0)
IF NOT DEFINED ProjectToolsAssemblySuffix (SET ProjectToolsAssemblySuffix=.v4.0)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=4.0.0.0)
IF NOT DEFINED VSRegistryConfigDecorator (SET VSRegistryConfigDecorator=_Config)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKCU)
IF NOT DEFINED VSIXExtensionDir (SET VSIXExtensionDir=Extensions\ORM Solutions\Natural ORM Architect\1.0)
GOTO:EOF

:_SetupVersionVars_v12.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v4.5)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=12.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=12.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=4.5.0.0)
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2013)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=13)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2013)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2013)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=12.0.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=12.0)
IF NOT DEFINED ProjectToolsAssemblySuffix (SET ProjectToolsAssemblySuffix=.v12.0)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=12.0.0.0)
IF NOT DEFINED VSRegistryConfigDecorator (SET VSRegistryConfigDecorator=_Config)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKCU)
IF NOT DEFINED VSIXExtensionDir (SET VSIXExtensionDir=Extensions\ORM Solutions\Natural ORM Architect\1.0)
GOTO:EOF

:_SetupVersionVars_v14.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v4.6)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=14.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=14.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=4.6.0.0)
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2015)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=15)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2015)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2015)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=14.0.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=14.0)
IF NOT DEFINED ProjectToolsAssemblySuffix (SET ProjectToolsAssemblySuffix=.Core)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=14.0.0.0)
IF NOT DEFINED VSRegistryConfigDecorator (SET VSRegistryConfigDecorator=_Config)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKCU)
IF NOT DEFINED VSIXExtensionDir (SET VSIXExtensionDir=Extensions\ORM Solutions\Natural ORM Architect\1.0)
GOTO:EOF

:_SetupVersionVars_v15.0
IF NOT DEFINED TargetFrameworkVersion (SET TargetFrameworkVersion=v4.6.1)
IF NOT DEFINED TargetVisualStudioMajorMinorVersion (SET TargetVisualStudioMajorMinorVersion=15.0)
IF NOT DEFINED TargetVisualStudioAssemblyVersion (SET TargetVisualStudioAssemblyVersion=15.0.0.0)
IF NOT DEFINED TargetVisualStudioFrameworkAssemblyVersion (SET TargetVisualStudioFrameworkAssemblyVersion=4.6.2.0)
IF NOT DEFINED TargetVisualStudioLongProductYear (SET TargetVisualStudioLongProductYear=2017)
IF NOT DEFINED TargetVisualStudioShortProductYear (SET TargetVisualStudioShortProductYear=17)
IF NOT DEFINED TargetVisualStudioShortProductName (SET TargetVisualStudioShortProductName=VS2017)
IF NOT DEFINED TargetVisualStudioLongProductName (SET TargetVisualStudioLongProductName=Visual Studio 2017)
IF NOT DEFINED TargetDslToolsAssemblyVersion (SET TargetDslToolsAssemblyVersion=15.0.0.0)
IF NOT DEFINED ProjectToolsVersion (SET ProjectToolsVersion=15.0)
IF NOT DEFINED ProjectToolsAssemblySuffix (SET ProjectToolsAssemblySuffix=.Core)
IF NOT DEFINED ProjectToolsAssemblyVersion (SET ProjectToolsAssemblyVersion=15.1.0.0)
IF NOT DEFINED VSRegistryConfigDecorator (SET VSRegistryConfigDecorator=_Config)
IF NOT DEFINED VSRegistryConfigHive (SET VSRegistryConfigHive=HKCU)
IF NOT DEFINED VSIXExtensionDir (SET VSIXExtensionDir=Extensions\ORM Solutions\Natural ORM Architect\1.0)
IF NOT DEFINED VSSideBySide (SET VSSideBySide=1)
GOTO:EOF

:SET6432
::Do this somewhere the resolved parens will not cause problems.
SET ResolvedProgramFiles=%ProgramFiles(x86)%
SET ResolvedCommonProgramFiles=%CommonProgramFiles(x86)%
::If this batch file is already running under a 32 bit process, then the
::reg utility will choose the appropriate registry keys without our help.
::This also means that this file should not be called to pre-set environment
::variables before invoking 32-bit processes that use these variables.
IF DEFINED PROCESSOR_ARCHITEW6432 (
	SET WOWRegistryAdjust=
) ELSE (
	SET WOWRegistryAdjust=\Wow6432Node
)
GOTO:EOF

:SETVAR
SET %~1=%~2
GOTO:EOF

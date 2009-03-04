@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.

IF "%TargetVisualStudioVersion%"=="v8.0" (
	SET DegradeToolsVersion=/toolsversion:2.0
) ELSE (
	SET TargetVisualStudioVersion=v9.0
)

REG QUERY "HKLM\Software\Microsoft\VisualStudio\9.0Exp" /v ApplicationID 1>NUL 2>&1
IF ERRORLEVEL 1 (CALL:_SetupExpHive)
REG QUERY "HKLM\Software\Microsoft\VisualStudio\9.0Exp" /v ApplicationID 1>NUL 2>&1
IF ERRORLEVEL 1 (ECHO Could not find experimental registry hive for Visual Studio 2008. Aborting... && GOTO:EOF)

CALL "%RootDir%\SetupEnvironment.bat" %*

CALL "%RootDir%\BuildDevTools.bat" %* /consoleloggerparameters:DisableMPLogging %DegradeToolsVersion%
CALL "%RootDir%\Build.bat" %* /consoleloggerparameters:DisableMPLogging %DegradeToolsVersion%

ECHO.
ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup

GOTO:EOF

:_SetupExpHive
CALL "%RootDir%\SetFromRegistry.bat" "VSIPDir" "HKLM\Software\Microsoft\VisualStudio\VSIP\9.0" "InstallDir" "f"
IF "%VSIPDir%"=="" (ECHO Please install the Microsoft Visual Studio 2008 SDK. See README.txt. && PAUSE && EXIT)
ECHO Setting up machine-level experimental registry hive for Visual Studio 2008... This may take a few minutes...
"%VSIPDir%\VisualStudioIntegration\Tools\Bin\VSRegEx.exe" GetOrig 9.0 Exp
REG QUERY "HKCU\Software\Microsoft\VisualStudio\9.0Exp\Configuration" /v ApplicationID 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (CALL:_BackupUserConfiguration)
GOTO:EOF

:_BackupUserConfiguration
ECHO Backing up user-level experimental registry hive for Visual Studio 2008... This may take a minute...
REG DELETE "HCKU\Software\Microsoft\VisualStudio\9.0Exp\Configuration_Backup" /f 1>NUL 2>&1
REG ADD "HKCU\Software\Microsoft\VisualStudio\9.0Exp\Configuration_Backup" 1>NUL 2>&1
REG COPY "HKCU\Software\Microsoft\VisualStudio\9.0Exp\Configuration" "HKCU\Software\Microsoft\VisualStudio\9.0Exp\Configuration_Backup" /s /f
REG DELETE "HKCU\Software\Microsoft\VisualStudio\9.0Exp\Configuration" /f 1>NUL 2>&1
GOTO:EOF

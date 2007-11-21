@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
SET TargetVisualStudioVersion=v9.0

REG QUERY "HKLM\Software\Microsoft\VisualStudio\9.0Exp" /ve 1>NUL 2>&1
IF ERRORLEVEL 1 (CALL:_SetupExpHive)
REG QUERY "HKLM\Software\Microsoft\VisualStudio\9.0Exp" /ve 1>NUL 2>&1
IF ERRORLEVEL 1 (ECHO Could not find experimental registry hive for Visual Studio 2008. Aborting... && GOTO:EOF)

CALL "%RootDir%\SetupEnvironment.bat" %* /toolsversion:3.5

CALL "%RootDir%\BuildDevTools.bat" %* /consoleloggerparameters:DisableMPLogging /toolsversion:3.5
CALL "%RootDir%\Build.bat" %* /consoleloggerparameters:DisableMPLogging /toolsversion:3.5

ECHO.
ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup

GOTO:EOF

:_SetupExpHive
CALL "%RootDir%\SetFromRegistry.bat" "VSIPDir" "HKLM\Software\Microsoft\VisualStudio\VSIP\9.0" "InstallDir" "f"
IF ERRORLEVEL 1 (ECHO Please install the Microsoft Visual Studio 2008 SDK. See README.txt. && PAUSE && EXIT)
ECHO Setting up machine-level experimental registry hive for Visual Studio 2008... This may take a few minutes...
"%VSIPDir%\VisualStudioIntegration\Tools\Bin\VSRegEx.exe" GetOrig 9.0 Exp
GOTO:EOF

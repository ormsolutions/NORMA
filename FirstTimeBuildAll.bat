@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

CALL "%RootDir%\BuildDevTools.bat" %*
CALL "%RootDir%\BuildSetup.bat" %*
CALL "%RootDir%\BuildTests.bat" %*

ECHO.
ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup

GOTO:EOF

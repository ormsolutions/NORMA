@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.

CALL "%RootDir%\BuildDevTools.bat" %*
CALL "%RootDir%\BuildAll.bat" %*

CALL "%RootDir%\SetupEnvironment.bat" %*
ECHO.
ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup

GOTO:EOF

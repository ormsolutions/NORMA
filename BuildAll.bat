@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

CALL "%RootDir%\BuildSetup.bat" %*
CALL "%RootDir%\BuildTests.bat" %*

GOTO:EOF

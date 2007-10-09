@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

MSBuild.exe /nologo "%RootDir%\Tests.proj" %*

GOTO:EOF

@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.

MSBuild.exe /nologo "%RootDir%\Tests.proj" %*

GOTO:EOF

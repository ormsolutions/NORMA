@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.

MSBuild.exe /nologo "%RootDir%\TestsWithAutomation.proj" %*

GOTO:EOF

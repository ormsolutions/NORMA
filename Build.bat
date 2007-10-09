@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

MSBuild.exe /nologo "%RootDir%\Main.proj" %*

GOTO:EOF

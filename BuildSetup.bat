@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

CALL "%RootDir%\BuildHelp.bat" %*
MSBuild.exe /nologo "%RootDir%\Setup.proj" %*

GOTO:EOF

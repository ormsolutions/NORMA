@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

:: NUBuild and NUBuildVS need to be built separately, before any other projects, since all of the later projects depend on them.
MSBuild.exe /nologo "%RootDir%\Tools\NUBuild\NUBuild.sln" %*
MSBuild.exe /nologo "%RootDir%\Tools\NUBuildVS\NUBuildVS.sln" %*
MSBuild.exe /nologo "%RootDir%\DevTools.proj" %*

GOTO:EOF

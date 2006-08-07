@ECHO OFF
SETLOCAL
IF NOT DEFINED FrameworkSDKDir (CALL "%VS80COMNTOOLS%\vsvars32.bat")
SET RootDir=%~dp0.
MSBuild.exe /nologo "%RootDir%\ORMPackage.sln"
MSBuild.exe /nologo "%RootDir%\ORMTestPackage.sln"
MSBuild.exe /nologo "%RootDir%\Tools\ORMCustomTool\ORMCustomTool.sln"
MSBuild.exe /nologo "%RootDir%\Setup\Setup.sln"

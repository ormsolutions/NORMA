@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*
MSBuild.exe /nologo "%RootDir%\ORMPackage.sln" %*
MSBuild.exe /nologo "%RootDir%\ORMTestPackage.sln" %*
MSBuild.exe /nologo "%RootDir%\AlternateViews\RelationalView\RelationalView.sln" %*
MSBuild.exe /nologo "%RootDir%\CustomProperties\CustomProperties.sln" %*
MSBuild.exe /nologo "%RootDir%\Tools\ORMCustomTool\ORMCustomTool.sln" %*
MSBuild.exe /nologo "%RootDir%\TestSuites\TestSuites.sln" %*
MSBuild.exe /nologo "%RootDir%\Setup\Setup.sln" %*

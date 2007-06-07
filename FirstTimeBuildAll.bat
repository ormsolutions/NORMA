@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

:: GAC the VSCT compiler so that we can use it.
SET VsSDKVsctDir=%VSIPDir%\Prerelease\VSCT
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTCompress.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTLibrary.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCT.exe"

ngen.exe install "VSCTCompress, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCTLibrary, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCT, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo

MSBuild.exe /nologo "%RootDir%\Tools\NUBuild\NUBuild.sln" %*
MSBuild.exe /nologo "%RootDir%\Tools\DisableRuleDirectiveProcessor\DisableRuleDirectiveProcessor.sln" %*

CALL "%RootDir%\BuildAll.bat" %*
ECHO.
ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup
GOTO:EOF

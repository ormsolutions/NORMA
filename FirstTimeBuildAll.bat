@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

:: Normally, the next two lines would have parentheses around the command portions. However, it is possible
:: for there to be parentheses in the %VSIPDir% path (and there are by default on x64), which
:: causes a syntax error. Therefore, we leave the parentheses off here.
IF "%TargetVisualStudioNumericVersion%"=="8.0" SET VsSDKVsctDir=%VSIPDir%\Prerelease\VSCT
IF NOT DEFINED VsSDKVsctDir SET VsSDKVsctDir=%VSIPDir%\VisualStudioIntegration\Tools\Bin

:: GAC the VSCT compiler so that we can use it.
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTCompress.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTLibrary.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCT.exe"

:: As of the August 2007 release of the VsSDK, the VSCTCompress assembly is still versioned as 8.0.0.0.
ngen.exe install "VSCTCompress, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCTLibrary, Version=%TargetVisualStudioNumericVersion%.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCT, Version=%TargetFrameworkNumericVersion%.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo

MSBuild.exe /nologo "%RootDir%\Tools\NUBuild\NUBuild.sln" %*
MSBuild.exe /nologo "%RootDir%\Tools\DisableRuleDirectiveProcessor\DisableRuleDirectiveProcessor.sln" %*

CALL "%RootDir%\BuildAll.bat" %*
ECHO.
ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup
GOTO:EOF

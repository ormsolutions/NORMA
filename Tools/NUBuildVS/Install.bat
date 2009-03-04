@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

:: Normally, the next two lines would have parentheses around the command portions. However, it is possible
:: for there to be parentheses in the %VSIPDir% path (and there are by default on x64), which
:: causes a syntax error. Therefore, we leave the parentheses off here.
IF /I "%TargetVisualStudioVersion%"=="v8.0" (
	IF EXIST "%VSIPDir%\Prerelease\VSCT" SET VsSDKVsctDir=%VSIPDir%\Prerelease\VSCT
)
IF NOT DEFINED VsSDKVsctDir SET VsSDKVsctDir=%VSIPDir%\VisualStudioIntegration\Tools\Bin

:: GAC the VSCT compiler so that we can use it.
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTCompress.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTLibrary.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCT.exe"

:: As of the August 2007 release of the VsSDK, the VSCTCompress assembly is still versioned as 8.0.0.0.
ngen.exe install "VSCTCompress, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCTLibrary, Version=%TargetVisualStudioAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCT, Version=%TargetVisualStudioFrameworkAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo


IF NOT EXIST "%MSBuildExtensionsPath%\Neumont\VisualStudio" (MKDIR "%MSBuildExtensionsPath%\Neumont\VisualStudio")

XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.VisualStudio.targets" "%MSBuildExtensionsPath%\Neumont\VisualStudio\"
XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.VisualStudio.Multitargeting.targets" "%MSBuildExtensionsPath%\Neumont\VisualStudio\"

FOR /F "usebackq skip=3 tokens=*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%"`) DO (REG QUERY "%%~A\MSBuild\SafeImports" 1>NUL 2>&1 && IF NOT ERRORLEVEL 1 (REG ADD "%%~A\MSBuild\SafeImports" /v "NUBuildVS1" /d "%MSBuildExtensionsPath%\Neumont\VisualStudio\Neumont.Build.VisualStudio.targets" /f))
FOR /F "usebackq skip=3 tokens=*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%"`) DO (REG QUERY "%%~A\MSBuild\SafeImports" 1>NUL 2>&1 && IF NOT ERRORLEVEL 1 (REG ADD "%%~A\MSBuild\SafeImports" /v "NUBuildVSMultitargeting1" /d "%MSBuildExtensionsPath%\Neumont\VisualStudio\Neumont.Build.VisualStudio.Multitargeting.targets" /f))
IF ERRORLEVEL 1 %COMSPEC% /c

GOTO:EOF

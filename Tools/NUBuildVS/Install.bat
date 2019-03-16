@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*
@ECHO ON

CALL:SETVAR "ResolvedMSBuildExtensionsPath" "%MSBuildExtensionsPath%\Neumont\VisualStudio"

IF NOT EXIST "%ResolvedMSBuildExtensionsPath%" (MKDIR "%ResolvedMSBuildExtensionsPath%")

XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.VisualStudio.targets" "%ResolvedMSBuildExtensionsPath%\"
XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.VisualStudio.Multitargeting.targets" "%ResolvedMSBuildExtensionsPath%\"

IF "%VSSideBySide%"=="true" (
	:: Not GAC Installed
	copy /y "%RootDir%\bin\Neumont.Build.VisualStudio%ProjectToolsAssemblySuffix%.dll" "%ResolvedMSBuildExtensionsPath%"
	GOTO:EOF
)


:: Normally, the next two lines would have parentheses around the command portions. However, it is possible
:: for there to be parentheses in the %VSIPDir% path (and there are by default on x64), which
:: causes a syntax error. Therefore, we leave the parentheses off here.
IF /I "%TargetVisualStudioVersion%"=="v8.0" (
	IF EXIST "%VSIPDir%\Prerelease\VSCT" CALL:SETVAR "VsSDKVsctDir" "%VSIPDir%\Prerelease\VSCT"
)
IF NOT DEFINED VsSDKVsctDir CALL:SETVAR "VsSDKVsctDir" "%VSIPDir%\VisualStudioIntegration\Tools\Bin"

:: GAC the VSCT compiler so that we can use it.
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTCompress.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCTLibrary.dll"
gacutil.exe /nologo /f /i "%VsSDKVsctDir%\VSCT.exe"

IF /I "%TargetVisualStudioVersion%"=="v8.0" (
	CALL:VSCT_OLD
) ELSE IF /I "%TargetVisualStudioVersion%"=="v9.0" (
	CALL:VSCT_OLD
) ELSE (
	ngen.exe install "VSCTCompress, Version=%TargetVisualStudioAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
	ngen.exe install "VSCTLibrary, Version=%TargetVisualStudioAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
	ngen.exe install "VSCT, Version=%TargetVisualStudioAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
)

FOR /F "usebackq skip=3 tokens=*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%"`) DO (REG QUERY "%%~A\MSBuild\SafeImports" 1>NUL 2>&1 && IF NOT ERRORLEVEL 1 (REG ADD "%%~A\MSBuild\SafeImports" /v "NUBuildVS1" /d "%ResolvedMSBuildExtensionsPath%\Neumont.Build.VisualStudio.targets" /f))
FOR /F "usebackq skip=3 tokens=*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%"`) DO (REG QUERY "%%~A\MSBuild\SafeImports" 1>NUL 2>&1 && IF NOT ERRORLEVEL 1 (REG ADD "%%~A\MSBuild\SafeImports" /v "NUBuildVSMultitargeting1" /d "%ResolvedMSBuildExtensionsPath%\Neumont.Build.VisualStudio.Multitargeting.targets" /f))
IF ERRORLEVEL 1 %COMSPEC% /c

GOTO:EOF

:SETVAR
SET %~1=%~2
GOTO:EOF

:VSCT_OLD
:: As of the August 2007 release of the VsSDK, the VSCTCompress assembly is still versioned as 8.0.0.0.
ngen.exe install "VSCTCompress, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCTLibrary, Version=%TargetVisualStudioAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
ngen.exe install "VSCT, Version=%TargetVisualStudioFrameworkAssemblyVersion%, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" /NoDependencies /nologo
GOTO:EOF
@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%MSBuildExtensionsPath%\Neumont" (MKDIR "%MSBuildExtensionsPath%\Neumont")

:: Clean up any files with the old name (i.e. ".Tools.")
IF EXIST "%MSBuildExtensionsPath%\Neumont\Neumont.Tools.Build.targets" (CALL:_DoCleanup)

XCOPY /Y /D /V /Q "%RootDir%\Tasks\RegexCompilationInfo.xsd" "%VSDir%\Xml\Schemas\"

XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.targets" "%MSBuildExtensionsPath%\Neumont\"
REG ADD "HKLM\%VSRegistryRootBase%\8.0\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Build.targets" /f
IF NOT "%VSRegistryRootSuffix%"=="" (REG ADD "HKLM\%VSRegistryRootBase%\8.0%VSRegistryRootSuffix%\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Build.targets" /f)

gacutil.exe /nologo /f /i "%RootDir%\bin\Neumont.Build.dll"
ngen.exe install "Neumont.Build, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /NoDependencies /nologo /verbose

GOTO:EOF


:_DoCleanup
ECHO Cleaning up old files...
DEL /F /Q "%RootDir%\bin\Neumont.Tools.Build.*"
DEL /F /Q "%MSBuildExtensionsPath%\Neumont\Neumont.Tools.Build.targets"
ngen.exe uninstall "Neumont.Tools.Build, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /NoDependencies /nologo /verbose
gacutil.exe /nologo /u "Neumont.Tools.Build, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

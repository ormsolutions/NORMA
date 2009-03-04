@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT EXIST "%MSBuildExtensionsPath%\Neumont" (MKDIR "%MSBuildExtensionsPath%\Neumont")

if EXIST "%VSDIR%" (
	XCOPY /Y /D /V /Q "%RootDir%\Tasks\RegexCompilationInfo.xsd" "%VSDir%\Xml\Schemas\"
)

XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.targets" "%MSBuildExtensionsPath%\Neumont\"

FOR /F "usebackq skip=3 tokens=*" %%A IN (`REG QUERY "HKLM\%VSRegistryRootBase%"`) DO (REG QUERY "%%~A\MSBuild\SafeImports" 1>NUL 2>&1 && IF NOT ERRORLEVEL 1 (REG ADD "%%~A\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Build.targets" /f))

gacutil.exe /nologo /f /i "%RootDir%\bin\Neumont.Build.dll"
ngen.exe install "Neumont.Build, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /NoDependencies /nologo /verbose

GOTO:EOF

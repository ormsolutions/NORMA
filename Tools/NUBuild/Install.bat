@ECHO OFF
SETLOCAL
IF NOT DEFINED FrameworkSDKDir (CALL "%VS80COMNTOOLS%\vsvars32.bat")
IF NOT DEFINED MSBuildExtensionsPath (SET MSBuildExtensionsPath=%ProgramFiles%\MSBuild)
IF NOT EXIST "%MSBuildExtensionsPath%\Neumont" (MKDIR "%MSBuildExtensionsPath%\Neumont")
SET RootDir=%~dp0.

IF EXIST "%MSBuildExtensionsPath%\Neumont\Neumont.Tools.Build.targets" (CALL:_DoCleanup)

FOR /F "usebackq skip=3 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS" /v "ProductDir"`) DO SET VSDir=%%~fB
XCOPY /Y /D /V /Q "%RootDir%\Tasks\RegexCompilationInfo.xsd" "%VSDir%\Xml\Schemas\"

XCOPY /Y /D /V /Q "%RootDir%\Neumont.Build.targets" "%MSBuildExtensionsPath%\Neumont\"
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Build.targets" /f
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Build.targets" /f

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

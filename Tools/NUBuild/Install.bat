@ECHO OFF
SETLOCAL
IF NOT DEFINED FrameworkSDKDir (CALL "%VS80COMNTOOLS%\vsvars32.bat")
IF NOT DEFINED MSBuildExtensionsPath (SET MSBuildExtensionsPath=%ProgramFiles%\MSBuild)
IF NOT EXIST "%MSBuildExtensionsPath%\Neumont" (MKDIR "%MSBuildExtensionsPath%\Neumont")
SET RootDir=%~dp0.

XCOPY /Y /D /V /Q "%RootDir%\Neumont.Tools.Build.targets" "%MSBuildExtensionsPath%\Neumont\"
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Tools.Build.targets" /f
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\MSBuild\SafeImports" /v "NUBuild1" /d "%MSBuildExtensionsPath%\Neumont\Neumont.Tools.Build.targets" /f

gacutil.exe /nologo /f /i "%RootDir%\bin\Neumont.Tools.Build.dll"
ngen.exe install "Neumont.Tools.Build, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" /NoDependencies /nologo /verbose

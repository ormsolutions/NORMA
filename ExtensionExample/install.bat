@echo off
setlocal
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont\ORM Architect\Extensions\ExtensionExample" /v "AssemblyFilePath" /t REG_EXPAND_SZ /d "%%ProgramFiles%%\Neumont\ORM Architect for Visual Studio\bin\Extensions\ExtensionExample.dll" /f
set rootPath=%1
if '%2'=='' (
set outDir="bin\Debug\"
) else (
set outDir=%2
)
set envPath="%ProgramFiles%\Neumont\ORM Architect for Visual Studio\bin\Extensions\"
if not exist %envPath% (
mkdir %envPath%
)
xcopy /Y /D /Q %rootPath%%outDir%"ExtensionExample.dll" %envPath%
if exist %rootPath%%outDir%"ExtensionExample.pdb" (
xcopy /Y /D /Q %rootPath%%outDir%"ExtensionExample.pdb" %envPath%
) else (
if exist %envPath%"ExtensionExample.pdb" (
del %envPath%"ExtensionExample.pdb"
)
)

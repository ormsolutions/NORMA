echo off
REG ADD "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0Exp\Neumont University\ORM Designer\Extensions\ExtensionExample" /v "AssemblyFilePath" /t REG_EXPAND_SZ /d "%%ProgramFiles%%\Neumont\NORMA\Extensions\ExtensionExample.dll" /f
set rootPath=%1
if '%2'=='' (
set outDir="bin\Debug\"
) else (
set outDir=%2
)
set envPath="%ProgramFiles%\Neumont\NORMA\Extensions\"
mkdir %envPath%
xcopy /Y /D /Q %rootPath%%outDir%"ExtensionExample.dll" %envPath%
if exist %rootPath%%outDir%"ExtensionExample.pdb" (
xcopy /Y /D /Q %rootPath%%outDir%"ExtensionExample.pdb" %envPath%
) else (
if exist %envPath%"ExtensionExample.pdb" (
del %envPath%"ExtensionExample.pdb"
)
)

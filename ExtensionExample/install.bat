echo off
set rootPath=%1
if '%2'=='' (
set outDir="bin\Debug\"
) else (
set outDir=%2
)
if '%3'=='' (
set envPath="C:\Program Files\Microsoft Visual Studio 8\"
) else (
set envPath=%3
)
xcopy /Y /D /Q %rootPath%%outDir%"ExtensionExample.dll" %envPath%"Common7\IDE\PrivateAssemblies\"
if exist %rootPath%%outDir%"ExtensionExample.pdb" (
xcopy /Y /D /Q %rootPath%%outDir%"ExtensionExample.pdb" %envPath%"Common7\IDE\PrivateAssemblies\"
) else (
if exist %envPath%"Common7\IDE\PrivateAssemblies\ExtensionExample.pdb" (
del %envPath%"Common7\IDE\PrivateAssemblies\ExtensionExample.pdb"
)
)

@echo off
setlocal
if '%1'=='' (
set rootPath=%~dp0
) else (
set rootPath=%~dp1
)
if '%2'=='' (
set envPath=C:\Program Files\Microsoft Visual Studio 8\
) else (
set envPath=%~dp2
)
xcopy /Y /D /Q "%rootPath%bin\Debug\Neumont.Tools.Converters.dll" "%envPath%Common7\IDE\PrivateAssemblies\"
xcopy /Y /D /Q "%rootPath%bin\Debug\Neumont.Tools.Converters.pdb" "%envPath%Common7\IDE\PrivateAssemblies\"
regedit /s "%rootPath%ImsToDmdCustomTool.reg"

@echo off
setlocal
if '%1'=='' (
set rootPath=%~dp0
) else (
set rootPath=%~dp1
)
if '%2'=='' (
set outDir=bin\Debug\
) else (
set outDir=%~2
)
if '%3'=='' (
set envPath=C:\Program Files\Microsoft Visual Studio 8\
) else (
set envPath=%~dp3
)
SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for Visual Studio
xcopy /Y /D /Q "%rootPath%%outDir%Neumont.Tools.ORM.SDK.TestEngine.dll" "%NORMADir%\bin\"
xcopy /Y /D /Q "%rootPath%%outDir%Neumont.Tools.ORM.SDK.TestEngine.XML" "%NORMADir%\bin\"
if exist "%rootPath%%outDir%Neumont.Tools.ORM.SDK.TestEngine.pdb" (
xcopy /Y /D /Q "%rootPath%%outDir%Neumont.Tools.ORM.SDK.TestEngine.pdb" "%NORMADir%\bin\"
) else (
if exist "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.pdb" (
del "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.pdb"
)
)
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestDriver.exe" "%NORMADir%\bin\"
if exist "%rootPath%%outDir%ORMTestDriver.pdb" (
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestDriver.pdb" "%NORMADir%\bin\"
) else (
if exist "%NORMADir%\bin\ORMTestDriver.pdb" (
del "%NORMADir%\bin\ORMTestDriver.pdb"
)
)
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestReport.xsd" "%envPath%Xml\Schemas\"
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestSuite.xsd" "%envPath%Xml\Schemas\"
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestSuiteReport.xsd" "%envPath%Xml\Schemas\"

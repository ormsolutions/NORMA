@echo off
setlocal
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
xcopy /Y /D /Q %rootPath%%outDir%"Neumont.Tools.ORM.SDK.TestEngine.dll" %envPath%"Common7\IDE\PrivateAssemblies\"
xcopy /Y /D /Q %rootPath%%outDir%"Neumont.Tools.ORM.SDK.TestEngine.XML" %envPath%"Common7\IDE\PrivateAssemblies\"
if exist %rootPath%%outDir%"Neumont.Tools.ORM.SDK.TestEngine.pdb" (
xcopy /Y /D /Q %rootPath%%outDir%"Neumont.Tools.ORM.SDK.TestEngine.pdb" %envPath%"Common7\IDE\PrivateAssemblies\"
) else (
if exist %envPath%"Common7\IDE\PrivateAssemblies\Neumont.Tools.ORM.SDK.TestEngine.pdb" (
del %envPath%"Common7\IDE\PrivateAssemblies\Neumont.Tools.ORM.SDK.TestEngine.pdb"
)
)
xcopy /Y /D /Q %rootPath%%outDir%"ORMTestDriver.exe" %envPath%"Common7\IDE\"
if exist %rootPath%%outDir%"ORMTestDriver.pdb" (
xcopy /Y /D /Q %rootPath%%outDir%"ORMTestDriver.pdb" %envPath%"Common7\IDE\"
) else (
if exist %envPath%"Common7\IDE\ORMTestDriver.pdb" (
del %envPath%"Common7\IDE\ORMTestDriver.pdb"
)
)
xcopy /Y /D /Q %rootPath%%outDir%"ORMTestDriver.exe.config" %envPath%"Common7\IDE\"
xcopy /Y /D /Q %rootPath%%outDir%"ORMTestReport.xsd" %envPath%"Xml\Schemas\"
xcopy /Y /D /Q %rootPath%%outDir%"ORMTestSuite.xsd" %envPath%"Xml\Schemas\"
xcopy /Y /D /Q %rootPath%%outDir%"ORMTestSuiteReport.xsd" %envPath%"Xml\Schemas\"

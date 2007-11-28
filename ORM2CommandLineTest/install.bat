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
if '%4'=='' (
set VSProduct=%~4
) else (
set VSProduct=VS2005
)
if '%5'=='' (
set VSLongProduct=%~5
) else (
set VSLongProduct=Visual Studio 2005
)

SET NORMADir=%ProgramFiles%\Neumont\ORM Architect for %VSLongProduct%

CALL:_CleanupFile "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.dll"
CALL:_CleanupFile "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.pdb"
CALL:_CleanupFile "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.xml"
CALL:_CleanupFile "%NORMADir%\bin\ORMTestDriver.exe"
CALL:_CleanupFile "%NORMADir%\bin\ORMTestDriver.pdb"
CALL:_CleanupFile "%NORMADir%\bin\ORMTestReportViewer.exe"
CALL:_CleanupFile "%NORMADir%\bin\ORMTestReportViewer.pdb"

xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\Neumont.Tools.ORM.SDK.TestEngine.%VSProduct%.dll" "%NORMADir%\bin\"
xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\Neumont.Tools.ORM.SDK.TestEngine.%VSProduct%.XML" "%NORMADir%\bin\"
if exist "%rootPath%%outDir%TestEngine\Neumont.Tools.ORM.SDK.TestEngine.%VSProduct%.pdb" (
xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\Neumont.Tools.ORM.SDK.TestEngine.%VSProduct%.pdb" "%NORMADir%\bin\"
) else (
if exist "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.%VSProduct%.pdb" (
del "%NORMADir%\bin\Neumont.Tools.ORM.SDK.TestEngine.%VSProduct%.pdb"
)
)

if exist "%rootPath%%outDir%ORMTestDriver.%VSProduct%.exe" (
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestDriver.%VSProduct%.exe" "%NORMADir%\bin\"
if exist "%rootPath%%outDir%ORMTestDriver.%VSProduct%.pdb" (
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestDriver.%VSProduct%.pdb" "%NORMADir%\bin\"
) else (
if exist "%NORMADir%\bin\ORMTestDriver.%VSProduct%.pdb" (
del "%NORMADir%\bin\ORMTestDriver.%VSProduct%.pdb"
)
)
)

if exist "%rootPath%%outDir%ORMTestReportViewer.%VSProduct%.exe" (
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestReportViewer.%VSProduct%.exe" "%NORMADir%\bin\"
if exist "%rootPath%%outDir%ORMTestReportViewer.%VSProduct%.pdb" (
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestReportViewer.%VSProduct%.pdb" "%NORMADir%\bin\"
) else (
if exist "%NORMADir%\bin\ORMTestReportViewer.%VSProduct%.pdb" (
del "%NORMADir%\bin\ORMTestReportViewer.%VSProduct%.pdb"
)
)
)

xcopy /Y /D /Q "%rootPath%%outDir%ORMTestReport.xsd" "%envPath%Xml\Schemas\"
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestSuite.xsd" "%envPath%Xml\Schemas\"
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestSuiteReport.xsd" "%envPath%Xml\Schemas\"
xcopy /Y /D /Q "%rootPath%%outDir%ORMTestSuiteReport.xsd" "%envPath%Xml\Schemas\"

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF
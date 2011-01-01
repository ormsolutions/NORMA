@echo off
setlocal

IF "%ProgramFiles(X86)%"=="" (
	SET ResolvedProgramFiles=%ProgramFiles%
	SET WOWRegistryAdjust=
) ELSE (
	CALL:SET6432
)

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
set envPath=%ResolvedProgramFiles%\Microsoft Visual Studio 8\
) else (
set envPath=%~dp3
)
if '%4'=='' (
set VSProduct=VS2005
) else (
set VSProduct=%~4
)
if '%5'=='' (
set VSLongProduct=Visual Studio 2005
) else (
set VSLongProduct=%~5
)

SET NORMADir=%ResolvedProgramFiles%\ORM Solutions\ORM Architect for %VSLongProduct%

xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%VSProduct%.dll" "%NORMADir%\bin\"
xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%VSProduct%.XML" "%NORMADir%\bin\"
xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\nunit.framework.dll" "%NORMADir%\bin\"
if exist "%rootPath%%outDir%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%VSProduct%.pdb" (
xcopy /Y /D /Q "%rootPath%%outDir%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%VSProduct%.pdb" "%NORMADir%\bin\"
) else (
if exist "%NORMADir%\bin\ORMSolutions.ORMArchitectSDK.TestEngine.%VSProduct%.pdb" (
del "%NORMADir%\bin\ORMSolutions.ORMArchitectSDK.TestEngine.%VSProduct%.pdb"
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

:SET6432
::Do this somewhere the resolved parens will not cause problems.
SET ResolvedProgramFiles=%ProgramFiles(X86)%
SET WOWRegistryAdjust=\Wow6432Node
GOTO:EOF

@ECHO OFF
SETLOCAL
SET RootDir=%~dp0
CALL "%RootDir%..\..\SetupEnvironment.bat" %*
@ECHO ON

SET TargetFile=%VSDIR%\XML\Schemas\NORMASDKSchemaCatalog.xml

type "%RootDir%RegisterSchemaFiles.VSCatalog.start.txt" > "%TargetFile%"
echo "%RootDir%catalog.xml" >> "%TargetFile%"
type "%RootDir%RegisterSchemaFiles.VSCatalog.end.txt" >> "%TargetFile%"

GOTO:EOF
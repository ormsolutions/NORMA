@ECHO OFF
SETLOCAL
CALL "%~dp0..\SetupEnvironment.bat"
CALL:SETVAR "NORMADomainModelDirectories" "%NORMABinDir%;%NORMAExtensionsDir%"

"%DevEnvDir%\ORMTestDriver.%TargetVisualStudioShortProductName%" %1 %2 %3 %4 %5 %6 %7 %8 %9
GOTO:EOF

:SETVAR
SET %~1=%~2
GOTO:EOF
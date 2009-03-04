@ECHO OFF
SETLOCAL

IF "%TargetVisualStudioVersion%"=="v8.0" (
	SET DegradeToolsVersion=/toolsversion:2.0
) ELSE (
	SET TargetVisualStudioVersion=v9.0
)

CALL "%~dp0BuildTests.bat" %* /consoleloggerparameters:DisableMPLogging %DegradeToolsVersion%
@ECHO OFF
SETLOCAL

IF "%TargetVisualStudioVersion%"=="v8.0" (
	SET DegradeToolsVersion=/toolsversion:2.0
) ELSE IF "%TargetVisualStudioVersion%"=="v9.0" (
	SET DegradeToolsVersion=/toolsversion:3.5
) ELSE (
	SET TargetVisualStudioVersion=v10.0
)

CALL "%~dp0BuildAll.bat" %* /consoleloggerparameters:DisableMPLogging %DegradeToolsVersion%

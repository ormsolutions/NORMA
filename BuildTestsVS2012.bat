@ECHO OFF
SETLOCAL

IF "%TargetVisualStudioVersion%"=="v8.0" (
	SET DegradeToolsVersion=/toolsversion:2.0
) ELSE IF "%TargetVisualStudioVersion%"=="v9.0" (
	SET DegradeToolsVersion=/toolsversion:3.5
) ELSE IF "%TargetVisualStudioVersion%"=="v10.0" (
	SET DegradeToolsVersion=/toolsversion:4.0
) ELSE (
	SET TargetVisualStudioVersion=v11.0
	SET DegradeToolsVersion=/toolsversion:4.0
)

CALL "%~dp0BuildTests.bat" %* /consoleloggerparameters:DisableMPLogging %DegradeToolsVersion%
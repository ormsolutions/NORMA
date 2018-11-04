@ECHO OFF
SETLOCAL

IF "%TargetVisualStudioVersion%"=="v8.0" (
	SET DegradeToolsVersion=/toolsversion:2.0
) ELSE IF "%TargetVisualStudioVersion%"=="v9.0" (
	SET DegradeToolsVersion=/toolsversion:3.5
) ELSE IF "%TargetVisualStudioVersion%"=="v10.0" (
	SET DegradeToolsVersion=/toolsversion:4.0
) ELSE IF "%TargetVisualStudioVersion%"=="v11.0" (
	SET DegradeToolsVersion=/toolsversion:4.0
) ELSE IF "%TargetVisualStudioVersion%"=="v12.0" (
	SET DegradeToolsVersion=/toolsversion:12.0
) ELSE IF "%TargetVisualStudioVersion%"=="v14.0" (
	SET DegradeToolsVersion=/toolsversion:14.0
) ELSE (
	SET TargetVisualStudioVersion=v15.0
	SET DegradeToolsVersion=/toolsversion:15.0
)

CALL "%~dp0BuildAll.bat" %* /consoleloggerparameters:DisableMPLogging %DegradeToolsVersion%
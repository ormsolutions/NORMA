@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

IF "%TargetVisualStudioVersion%"=="v8.0" (
	SET DegradeToolsVersion=/toolsversion:2.0
) ELSE IF "%TargetVisualStudioVersion%"=="v9.0" (
	SET DegradeToolsVersion=/toolsversion:3.5
) ELSE (
	SET TargetVisualStudioVersion=v10.0
)

CALL "%RootDir%\BuildDevTools.bat" %*
CALL "%RootDir%\Build.bat" %*

GOTO:EOF

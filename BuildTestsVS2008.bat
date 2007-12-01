@ECHO OFF
SETLOCAL
SET TargetVisualStudioVersion=v9.0

CALL "%~dp0BuildTests.bat" %* /consoleloggerparameters:DisableMPLogging /toolsversion:3.5

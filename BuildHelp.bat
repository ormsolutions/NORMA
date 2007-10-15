@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

CALL "%TrunkDir%\SetFromRegistry.bat" "HSLiteInstallDir" "HKLM\SOFTWARE\Innovasys\HelpStudio2" "LiteInstallDir" "f"

ECHO Building help project. This may take several minutes...
PUSHD "%TrunkDir%\Documentation\Help Project"
"%HSLiteInstallDir%\bin\HelpStudioLite2.exe" /b /s /p "Neumont.ORM.General.hsp"
SET HSLiteErrorLevel=%ERRORLEVEL%
POPD
IF NOT "%HSLiteErrorLevel%"=="0" (ECHO WARNING: Problems were encountered while building help project. Error level: %HSLiteErrorLevel% && PAUSE)
ECHO Finished building help project.

GOTO:EOF

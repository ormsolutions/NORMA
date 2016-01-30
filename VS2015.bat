@CALL "%~dp0VSVer.bat" 2015

@ECHO OFF
::Add SDK tools to path if vsct cannot be found, fails to compile in VS2015 otherwise
set SDKVSCT=
for %%i in (vsct.exe) do (set EXISTINGVSCT=%%~s$PATH:i)
if '%EXISTINGVSCT%'=='' (
	CALL:GETSHORTPATH "SDKVSCT" "%VSSDK140INSTALL%\VisualStudioIntegration\Tools\Bin"
)
if NOT '%SDKVSCT%'=='' (
	CALL:EXTENDPATH "%SDKVSCT%"
)
SET FOUNDVSCT=
SET SDKVSCT=
goto:EOF

:GETSHORTPATH
SET %~1=%~s2
GOTO:EOF

:EXTENDPATH
SET PATH=%PATH%;%~1
GOTO:EOF

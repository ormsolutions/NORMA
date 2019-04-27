@CALL "%~dp0VSVer.bat" 2019

::Add SDK tools to path if vsct cannot be found, fails to compile in VS2019 otherwise
set SDKVSCT=
for %%i in (vsct.exe) do (set EXISTINGVSCT=%%~s$PATH:i)
if '%EXISTINGVSCT%'=='' (
	CALL:GETSHORTPATH "SDKVSCT" "%VSSDK150INSTALL%\VisualStudioIntegration\Tools\Bin"
)
if NOT '%SDKVSCT%'=='' (
	CALL:EXTENDPATH "%SDKVSCT%"
)
CALL:SETVAR "VSSdkInstall" "%VSSDK150INSTALL%"
SET FOUNDVSCT=
SET SDKVSCT=
goto:EOF

:GETSHORTPATH
SET %~1=%~s2
GOTO:EOF

:EXTENDPATH
SET PATH=%PATH%;%~1
GOTO:EOF

:SETVAR
SET %~1=%~2
GOTO:EOF

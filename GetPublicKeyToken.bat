@ECHO OFF
SETLOCAL
SET SNKOutFile=%~dpn1.snkOut
CALL:_CleanupFile "%SNKOutFile%"
sn -q -p "%~1" "%SNKOutFile%">NUL
IF "%ERRORLEVEL%"=="0" (
	CALL:_GetToken "%SNKOutFile%"
	CALL:_CleanupFile "%SNKOutFile%"
) ELSE (
	CALL:_GetToken "%~1"
)
@ECHO %TempPKToken:~-16%
set SNKOutFile=
set TempPKToken=
GOTO:EOF

:_GetToken
FOR /F "usebackq tokens=1 delims=" %%A IN (`sn -q -t "%~1"`) DO SET TempPKToken=%%A
GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF


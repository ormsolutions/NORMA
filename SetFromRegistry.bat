@ECHO OFF
:: Parameters:
::   %~1 = Environment variable name
::   %~2 = Registry key name
::   %~3 = Registry value name
::   %~4 = FOR variable modifier
IF NOT DEFINED %~1 FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "%~2" /v "%~3"`) DO SET %~1=%%~%~4B
GOTO:EOF

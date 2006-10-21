@ECHO OFF
SETLOCAL
FOR /F "usebackq skip=3 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS" /v "EnvironmentPath"`) DO SET VSEnvironmentPath=%%~fB
IF NOT DEFINED FrameworkSDKDir (CALL "%VS80COMNTOOLS%\vsvars32.bat")
SET RootDir=%~dp0.
MSBuild.exe /nologo "%RootDir%\Tools\NUBuild\NUBuild.sln" %*
CALL "%RootDir%\BuildAll.bat" %*
ECHO.
ECHO Running "devenv.exe /RootSuffix Exp /Setup"... This may take a few minutes...
"%VSEnvironmentPath%" /RootSuffix Exp /Setup

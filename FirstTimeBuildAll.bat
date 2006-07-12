@setlocal
@FOR /F "usebackq skip=3 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0" /v "InstallDir"`) DO call set LaunchDevenv=%%~dpsBdevenv
call "%~dp0BuildAll.bat"
%launchdevenv% "%~dp0Tools\DslImportDirectiveProcessor\DslImportDirectiveProcessor.sln" /Rebuild DslImportDirectiveProcessor
call "%~dp0install.bat"
call "%~dp0ORM2CommandLineTest\install.bat"
call "%~dp0Tools\ORMCustomTool\install.bat"
call "%~dp0Tools\DslImportDirectiveProcessor\install.bat"
%launchdevenv% /RootSuffix Exp /setup
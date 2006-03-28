@setlocal
@set BuildType=Debug
@FOR /F "usebackq skip=3 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0" /v "InstallDir"`) DO call set LaunchDevenv=%%~dpsBdevenv
%launchdevenv% "%~dp0ORMPackage.sln" /Rebuild %BuildType%
%launchdevenv% "%~dp0ORMTestPackage.sln" /Rebuild %BuildType%
%launchdevenv% "%~dp0XML/ORMCustomTool/ORMCustomTool.sln" /Rebuild %BuildType%
%launchdevenv% "%~dp0Setup/Setup.sln" /Rebuild Setup

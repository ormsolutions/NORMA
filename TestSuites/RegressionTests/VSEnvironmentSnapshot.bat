@echo off
setlocal
if '%~1'=='' goto:usage
set BaseName=%~1
set RootSuffix=%~2
if exist "%~dp0%BaseName%.hiv" del /f "%~dp0%BaseName%.hiv"
if exist "%~dp0%BaseName%.vssettings" del /f "%~dp0%BaseName%.vssettings"
if exist "%~dp0%BaseName%" rd /s /q "%~dp0%BaseName%"

FOR /F "usebackq eol=! tokens=2*" %%A IN (`REG QUERY "HKCU\SOFTWARE\Microsoft\VisualStudio\8.0%RootSuffix%" /v "VisualStudioLocation"`) DO call set VSFileLocation=%%B
reg save HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% "%~dp0%BaseName%.hiv" 1>NUL
FOR /F "usebackq eol=! tokens=2* delims=%%" %%A IN (`REG QUERY "HKCU\SOFTWARE\Microsoft\VisualStudio\8.0%RootSuffix%\Profile" /v "AutoSaveFile"`) DO (
        copy /y "%VSFileLocation%%%B" "%~dp0%BaseName%.vssettings" 1>NUL
        )
        xcopy /y /q /e /d "%AppData%\Microsoft\VisualStudio\8.0%RootSuffix%" "%~dp0%BaseName%\" 1>NUL
        goto:eof

        :usage
        echo Snapshot the current environment settings into files usable with TestHost.bat
        echo Sample Usage:
        echo VSEnvironmentSnapshot ENVIRONMENTSETTINGS [ROOTSUFFIX]
        echo The first argument is the name of the environment settings to establish.
        echo    The ENVIRONMENTSETTINGS directory will be updated to contain the
        echo    contents of  %%AppData%%\Microsoft\VisualStudio\8.0%%ROOTSUFFIX%%, the
        echo    ENVIRONMENTSETTINGS.hiv file will be updated to contain the contents
        echo    for the HKCU\Software\Microsoft\VisualStudio\8.0%ROOTSUFFIX% registry key,
        echo    and the ENVIRONMENTSETTINGS.vssettings file will be updated to contain
        echo    the contents of the
        echo    %%VSFileLocation%%\%%ROOTSUFFIX%%\CurrentSettings.vssettings file.
        echo The optional second argument specifies the setting to send to Visual
        echo    studio for the /RootSuffix argument. Use "" for the standard environment.
        echo    Visual Studio supports multiple sets of user and installation settings via
        echo    this argument. The root suffix is appended to all application data directory
        echo    names and root registry keys. For example, a root suffix of Exp means that
        echo    the Software\Microsoft\VisualStudio\8.0Exp registry settings should be
        echo    used for both the HKLM (machine install) and HKCU (user settings)
        echo    registry settings.


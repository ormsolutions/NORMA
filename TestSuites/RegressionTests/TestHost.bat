@echo off
setlocal
REM This batch file is responsible for setting up and restoring the test VS environment.
REM The LanchDevenv, TestLog, and RootSuffix environment variables will be set and the
REM specifiec test addin will be installed in the correct add-ins directory.
REM The actual tests are run by a call to RunTests.bat.
REM You can find explanations for the various commands in the ORMAutomationInVSFAQ.htm file

REM Check usage and do some sanity checking on the passed in values
if '%~1'=='' goto:usage
set TestAddin=%~1
if not exist "%~dp0%TestAddin%.sln" goto:usage
if not exist "%~dp0%TestAddin%" goto:usage
if '%~2'=='' goto:usage
set CachedSettings=%~2
if not exist "%~dp0%CachedSettings%.hiv" goto:usage
if not exist "%~dp0%CachedSettings%.vssettings" goto:usage
if not exist "%~dp0%CachedSettings%" goto:usage
if '%~3'=='' goto:usage
set LogFile=%~3
set RootSuffix=%~4
if '%~5'=='' goto:usage
if not exist "%~dp0%~5" (
	if not exist "%~dp0%~5.bat" goto:usage
)

REM Get the Visual Studio environment install location
FOR /F "usebackq eol=! tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\8.0%RootSuffix%" /v "InstallDir"`) DO call set LaunchDevenv=%%~dpsBdevenv

REM Clear the current add-in file, it may not load
FOR /F "usebackq eol=! tokens=2*" %%A IN (`REG QUERY "HKCU\SOFTWARE\Microsoft\VisualStudio\8.0%RootSuffix%" /v "VisualStudioLocation"`) DO call set VSFileLocation=%%B
if not exist "%VSFileLocation%\Addins" (md "%VSFileLocation%\Addins")
del /F "%VSFileLocation%\Addins\%TestAddin%.Addin" 1>NUL 2>&1

REM Backup the current user Application Data and registry settings
set UserSettingsDir=%APPDATA%\Microsoft\VisualStudio\8.0%RootSuffix%
call:BackupDir "%UserSettingsDir%" "Backup"
if ERRORLEVEL 1 goto:eof
REM Remove the file before running the command
if exist "%~dp0CurrentUserSettings.hiv" del "%~dp0CurrentUserSettings.hiv"
if ERRORLEVEL 1 (
	call:RestoreDir %UserSettingsDir%" "Backup"
	goto:eof
)
reg save HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% "%~dp0CurrentUserSettings.hiv" 1>NUL

REM Establish the test environment
xcopy /y /q /e /d "%~dp0%CachedSettings%" "%UserSettingsDir%\" 1>NUL
reg delete HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% /f 1>NUL 2>&1
reg add HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% 1>NUL
reg restore HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% "%~dp0%CachedSettings%.hiv" 1>NUL
del /f "%~dp0%CachedSettings%.copy.vssettings" 1>NUL 2>&1
copy /y "%~dp0%CachedSettings%.vssettings" "%~dp0%CachedSettings%.copy.vssettings" 1>NUL
reg add HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix%\Profile /v AutoSaveFile /t REG_SZ /f /d "%~dp0%CachedSettings%.copy.vssettings" 1>NUL

REM Run VS once to make sure all directories are in line
start /wait /min %LaunchDevenv%.exe /Command File.Exit /RootSuffix %RootSuffix%

REM Install the add-in file so VS knows its there
FOR /F "usebackq eol=! tokens=2*" %%A IN (`REG QUERY "HKCU\SOFTWARE\Microsoft\VisualStudio\8.0%RootSuffix%" /v "VisualStudioLocation"`) DO call set VSFileLocation=%%B
SET InstallAddin="%VSFileLocation%\Addins\%TestAddin%.AddIn"
type "%~dp0%TestAddin%\%TestAddin%.AddIn.start" > %InstallAddin%
echo %~dp0%TestAddin%\bin\%TestAddin%.dll >> %InstallAddin%
type "%TestAddin%\%TestAddin%.AddIn.end" >> %InstallAddin%

set TestLog=%~dp0%LogFile%
echo Running tests on %DATE% %TIME% >> "%TestLog%"
:RunTest
if not '%~5'=='' (
	echo Running tests in '%~5'
	echo Running tests in '%~5' >> "%TestLog%"
	call "%~dp0%~5"
	shift /5
	goto:RunTest
)
REM type "%TestLog%"

REM Restore registry settings, application data, and .vssettings files
reg delete HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% /f 1>NUL 2>&1
reg add HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% 1>NUL
reg restore HKCU\Software\Microsoft\VisualStudio\8.0%RootSuffix% "%~dp0CurrentUserSettings.hiv" 1>NUL
del /f "%~dp0CurrentUserSettings.hiv"
del /f "%~dp0%CachedSettings%.copy.vssettings" 1>NUL 2>&1
call:RestoreDir "%UserSettingsDir%" "Backup"
goto:eof

:BackupDir
REM The first argument is the directory name to backup, the second argument is the extension to add to it for later retrieval
REM echo Back up "%~1" to "%~1.%~2"
if exist "%~1.%~2" (
	rd /s /q "%~1.%~2"
	if ERRORLEVEL 1 goto:eof
)
ren "%~1" "%~nx1.%~2"
if ERRORLEVEL 1 goto:eof
REM echo Backup successful
goto:eof

:RestoreDir
REM The first argument is the directory name backup up with BackupDir, the second argument is the extension added to it
REM echo Restore "%~1" from "%~1.%~2"
if exist "%~1" rd /s /q "%~1"
if ERRORLEVEL 1 goto:eof
ren "%~1.%~2" "%~nx1"
if ERRORLEVEL 1 goto:eof
REM echo Restoration successful

goto:eof

:usage
echo Sample Usage:
echo TestHost BASEADDINNAME ENVIRONMENTSETTINGS LOGFILE ROOTSUFFIX TESTSBATCHFILE [TESTSBATCHFILE...]
echo The first argument is the base name of the add-in project to build and install.
echo    The assumption is made that the BASEADDINNAME.sln file exists in the same
echo    directory as this batch file and that there is a subdirectory of the same
echo    name. The subdirectory should contain the BASEADDINNAME.Addin.start and
echo    BASEADDINNAME.Addin.end files, which are used to install the addin.
echo The second argument is the name of the environment settings to establish and
echo    restore before the tests are run. The ENVIRONMENTSETTINGS directory
echo    should contain the settings for the test environment in
echo    %%AppData%%\Microsoft\VisualStudio\8.0%%ROOTSUFFIX%%, the
echo    ENVIRONMENTSETTINGS.hiv file should contain the registry contents
echo    for the HKCU\Software\Microsoft\VisualStudio\8.0%%ROOTSUFFIX%% key,
echo    and the ENVIRONMENTSETTINGS.vssettings file should contain a snapshot
echo    of the %%VSFileLocation%%\%%RootSuffix%%\CurrentSettings.vssettings file
echo The third argument is the file name of the log file to create. The file name
echo    is relative to the location of the batch file.
echo The fourth argument specifies the setting to send to Visual
echo    studio for the /RootSuffix argument. Use "" for the standard environment.
echo    Visual Studio supports multiple sets of user and installation settings via
echo    this argument. The root suffix is appended to all application data directory
echo    names and root registry keys. For example, a root suffix of Exp means that
echo    the Software\Microsoft\VisualStudio\8.0Exp registry settings should be
echo    used for both the HKLM (machine install) and HKCU (user settings)
echo    registry settings.
echo The fifth and any following arguments contain the name of test batch
echo    files to run after the environment has been established. Each test batch
echo    file should set the TESTFILE environment variable to a full path as well as
echo    any additional environment variables required by the test code. The file
echo    specified by the TESTFILE environment variable should be a .cs file
echo    containing a Test class in the Test namespace with a
echo    public static void RunTest(DTE2 DTE) method in it that runs the
echo    test. The TESTLOG environment variable will be available to the
echo    test, as well as any additional environment variables set by the
echo    TESTSBATCHFILE for that test. After the environment variables
echo    are set, the TESTSBATCHFILE should do a 'call RunTest'
echo    to execute the test.

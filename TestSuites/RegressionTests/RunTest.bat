REM This batch file uses the LAUNCHDEVENV, ROOTSUFFIX, TESTADDIN, TESTLOG and TESTFILE environment
REM variables to rebuild the addin solution and run the test in visual studio
REM If an argument is specified, it is appended to the TESTLOG file before the test is run.
echo Begin test "%~1"
echo Begin test "%~1" >>"%TestLog%"
REM If the current Test.cs has changed since it was copied, then back it up to save data loss
call:CompareBackupFile "%~dp0LastCodeFile.txt" "%~dp0%TestAddin%\Test.cs"
copy /y "%TestFile%" "%~dp0%TestAddin%\Test.cs" 1>NUL
echo %TestFile% > "%~dp0LastCodeFile.txt"
%LaunchDevenv% "%TestAddin%.sln" /Rebuild Debug /RootSuffix %RootSuffix% 1>NUL
if ERRORLEVEL 1 (
	echo "%~1" failed to build
	echo "%~1" failed to build >>"%TestLog%"
) ELSE (
	start /w %LaunchDevenv%.exe /resetaddin %TESTADDIN%.Connect /command %TESTADDIN%.Connect.%TESTADDIN% /RootSuffix %RootSuffix% 
	echo Completed test "%~1"
	echo Completed test "%~1" >>"%TestLog%"
)
goto:eof

:CompareBackupFile
if exist "%~2" (
	if exist "%~1" (
		FOR /F "delims= tokens=1" %%A in (%~s1) do call:RandomBackupFile "%~2" "%%~A"
	)
)
goto:eof

:RandomBackupFile
if exist %~2 (
	if not '%~z1'=='%~z2' (
		copy "%~1" "%~1.EmergencyBackup.%RANDOM%" 1>NUL
	)
)
goto:eof

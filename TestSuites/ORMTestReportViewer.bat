@ECHO OFF
SETLOCAL
CALL "%~dp0..\SetupEnvironment.bat"

CALL:run "%DevEnvDir%\ORMTestReportViewer.%TargetVisualStudioShortProductName%.exe" %1 %2 %3 %4 %5 %6 %7 %8 %9
GOTO:EOF

:run
start %~s1 %~s2 %~s3 %~s4 %~s5 %~s6 %~s7 %~s8

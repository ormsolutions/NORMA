@ECHO OFF
SETLOCAL

IF "%ProgramFiles(X86)%"=="" (
	SET ResolvedProgramFiles=%ProgramFiles%
) ELSE (
	CALL:SET6432
)

@call :run "%ResolvedProgramFiles%\ORM Solutions\ORM Architect For Visual Studio 2005\Bin\ORMTestReportViewer.VS2005.exe" %1 %2 %3 %4 %5 %6 %7 %8 
goto:eof

:SET6432
::Do this somewhere the resolved parens will not cause problems.
SET ResolvedProgramFiles=%ProgramFiles(x86)%
GOTO:EOF

:run
start %~s1 %~s2 %~s3 %~s4 %~s5 %~s6 %~s7 %~s8

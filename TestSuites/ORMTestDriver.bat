@ECHO OFF
SETLOCAL

IF "%ProgramFiles(X86)%"=="" (
	SET ResolvedProgramFiles=%ProgramFiles%
) ELSE (
	CALL:SET6432
)

"%ResolvedProgramFiles%\ORM Solutions\ORM Architect For Visual Studio 2005\Bin\ORMTestDriver.VS2005" %1 %2 %3 %4 %5 %6 %7 %8 %9

GOTO:EOF

:SET6432
::Do this somewhere the resolved parens will not cause problems.
SET ResolvedProgramFiles=%ProgramFiles(x86)%
GOTO:EOF


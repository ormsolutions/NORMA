@ECHO OFF
SETLOCAL
FOR /F "usebackq delims=v.-g tokens=1,2,3,4,5" %%i in (`git -C "%2" describe --first-parent --long %3`) DO (
SET GitMajor=%%i
SET GitMinor=%%j
SET GitSub=%%k
SET GitUpdate=%%l
SET GitHash=%%m
)
SET PadSub=00%GitSub%
REM Put a 3 in front of the tagged update number. This distinguishes the git tag
REM based versioning system (and makes the numbers larger than) previous versioning
REM schemes that were year/month based.
IF "%1"=="all" (
@ECHO %GitMajor%.%GitMinor%.3%PadSub:~-3%.%GitUpdate%
)
IF "%1"=="" (
@ECHO %GitMajor%.%GitMinor%.3%PadSub:~-3%.%GitUpdate%
)
IF "%1"=="full" (
@ECHO %GitMajor%.%GitMinor%.3%PadSub:~-3%.%GitUpdate%-g%GitHash%
)
IF "%1"=="major" (
@ECHO %GitMajor%
)
IF "%1"=="minor" (
@ECHO %GitMinor%
)
IF "%1"=="build" (
@ECHO 3%PadSub:~-3%
)
IF "%1"=="revision" (
@ECHO %GitUpdate%
)
IF "%1"=="hash" (
@ECHO %GitHash%
)
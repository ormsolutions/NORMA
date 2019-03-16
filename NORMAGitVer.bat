@ECHO OFF
SETLOCAL
FOR /F "usebackq delims=v.-g tokens=1,2,3,4" %%i in (`git -C "%1" describe --first-parent --long %2`) DO (
SET GitMajor=%%i
SET GitMinor=%%j
SET GitSub=%%k
SET GitUpdate=%%l
)
SET PadSub=00%GitSub%
REM Put a 3 in front of the tagged update number. This distinguished the git tag
REM based versioning system (and makes the numbers larger than) previous versioning
REM schemes were year/month based.
@ECHO %GitMajor%.%GitMinor%.3%PadSub:~-3%.%GitUpdate%

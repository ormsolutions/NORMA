@ECHO OFF
SETLOCAL
FOR /F "usebackq delims=v.-g tokens=1,2,3,4" %%i in (`git -C "%1" describe --first-parent --long %2`) DO (
SET GitMajor=%%i
SET GitMinor=%%j
SET GitSub=%%k
SET GitUpdate=%%l
)
SET PadSub=00%GitSub%
REM Put a 3 in front of the tagged update number. This distinguishes the git tag
REM based versioning system (and makes the numbers larger than) previous versioning
REM schemes that were year/month based.
REM UNDONE: Changed 3 to 4, do not check in. This is so the preview build has a higher
REM version than the installed build.
@ECHO %GitMajor%.%GitMinor%.4%PadSub:~-3%.%GitUpdate%

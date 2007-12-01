@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
IF NOT "%~2"=="" (SET TargetVisualStudioVersion=%~2)
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

SET NORMAGenerators=HKLM\SOFTWARE\Neumont\ORM Architect for %TargetVisualStudioLongProductName%\Generators

:: Install Custom Tool DLL
CALL:_AddCustomToolReg "%TargetVisualStudioMajorMinorVersion%"
CALL:_AddCustomToolReg "%TargetVisualStudioMajorMinorVersion%Exp"


CALL:_AddXslORMGenerator "UndeadOIAL" "UndeadOIAL" "UndeadOIAL" ".UndeadOIAL.xml" "ORM" "UndeadOIAL" "%~dp0UndeadOIAL.xslt"
CALL:_AddXslORMGenerator "LiveOIAL" "LiveOIAL" "LiveOIAL" ".LiveOIAL.xml" "ORM" "LiveOIAL" "%~dp0LiveOIAL.xslt"

GOTO:EOF

:_AddCustomToolReg
REG ADD "%NORMAGenerators%\OIALDiff" /f /v "Type" /d "Class" 1>NUL
REG ADD "%NORMAGenerators%\OIALDiff" /f /v "Class" /d "OIALDiffCustomTool" 1>NUL
REG ADD "%NORMAGenerators%\OIALDiff" /f /v "CodeBase" /d "%~dp0bin\OIALDiffCustomTool.dll" 1>NUL
REG ADD "%NORMAGenerators%\OIALDiff" /f /v "Assembly" /d "OIALDiffCustomTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" 1>NUL
GOTO:EOF


:_AddXslORMGenerator
REG DELETE "%NORMAGenerators%\%~1" /va /f 1>NUL 2>&1
REG ADD "%NORMAGenerators%\%~1" /f /v "Type" /d "XSLT" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "OfficialName" /d "%~1" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "DisplayName" /d "%~2" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "DisplayDescription" /d "%~3" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "FileExtension" /d "%~4" 1>NUL 2>&1
REG ADD "%NORMAGenerators%\%~1" /f /v "SourceInputFormat" /d "%~5" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "ProvidesOutputFormat" /d "%~6" 1>NUL
REG ADD "%NORMAGenerators%\%~1" /f /v "TransformUri" /d "%~7" 1>NUL
GOTO:EOF

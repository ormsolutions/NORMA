@ECHO OFF
SETLOCAL

:: Install Custom Tool DLL
CALL:_AddCustomToolReg "8.0"
CALL:_AddCustomToolReg "8.0Exp"


CALL:_AddXslORMGenerator "UndeadOIAL" "UndeadOIAL" "UndeadOIAL" ".UndeadOIAL.xml" "ORM" "UndeadOIAL" "%~dp0UndeadOIAL.xslt"
CALL:_AddXslORMGenerator "LiveOIAL" "LiveOIAL" "LiveOIAL" ".LiveOIAL.xml" "ORM" "LiveOIAL" "%~dp0LiveOIAL.xslt"

GOTO:EOF

:_AddCustomToolReg
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\OIALDiff" /f /v "Type" /d "Class" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\OIALDiff" /f /v "Class" /d "OIALDiffCustomTool" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\OIALDiff" /f /v "CodeBase" /d "%~dp0bin\OIALDiffCustomTool.dll" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\OIALDiff" /f /v "Assembly" /d "OIALDiffCustomTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f" 1>NUL
GOTO:EOF


:_AddXslORMGenerator
REG DELETE "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /va /f 1>NUL 2>&1
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "Type" /d "XSLT" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "OfficialName" /d "%~1" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "DisplayName" /d "%~2" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "DisplayDescription" /d "%~3" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "FileExtension" /d "%~4" 1>NUL 2>&1
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "SourceInputFormat" /d "%~5" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "ProvidesOutputFormat" /d "%~6" 1>NUL
REG ADD "HKLM\SOFTWARE\Neumont\ORM Architect for Visual Studio\Generators\%~1" /f /v "TransformUri" /d "%~7" 1>NUL
GOTO:EOF

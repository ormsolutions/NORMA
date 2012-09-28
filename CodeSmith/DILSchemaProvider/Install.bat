@ECHO OFF
SETLOCAL
IF "%~1"=="" (SET OutDir=bin\Debug) ELSE (SET OutDir=%~1.)
SET RootDir=%~dp0.

IF "%ProgramFiles(X86)%"=="" (
	SET WOWRegistryAdjust=
) ELSE (
	IF DEFINED PROCESSOR_ARCHITEW6432 (
		SET WOWRegistryAdjust=
	) ELSE (
		SET WOWRegistryAdjust=\Wow6432Node
	)
)

FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKLM\SOFTWARE%WOWRegistryAdjust%\CodeSmith\v3.2" /v "ApplicationDirectory"`) DO SET CodeSmithDir=%%~fB

XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\SchemaExplorer.DILSchemaProvider.dll" "%CodeSmithDir%\SchemaProviders\"
XCOPY /Y /D /V /Q "%RootDir%\%OutDir%\SchemaExplorer.DILSchemaProvider.pdb" "%CodeSmithDir%\SchemaProviders\"
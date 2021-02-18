@ECHO off
SETLOCAL
IF '%1'=='' (
	CALL:SETVAR "RootDir" "%~dp0"
) ELSE (
	CALL:SETVAR "RootDir" "%~dp1"
)
CALL "%~dp0..\SetupEnvironment.bat"

IF '%2'=='' (
	SET OutPath=%RootDir%bin\Debug\
) ELSE (
	SET OutPath=%RootDir%%~2
)

XCOPY /Y /D /V /Q "%OutPath%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%TargetVisualStudioShortProductName%.dll" "%DevEnvDir%PrivateAssemblies\"
XCOPY /Y /D /V /Q "%OutPath%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%TargetVisualStudioShortProductName%.XML" "%DevEnvDir%PrivateAssemblies\"
XCOPY /Y /D /V /Q "%OutPath%TestEngine\nunit.framework.dll" "%DevEnvDir%PrivateAssemblies\"
XCOPY /Y /D /V /Q "%OutPath%TestEngine\xmldiffpatch.dll" "%DevEnvDir%PrivateAssemblies\"

IF EXIST "%OutPath%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%TargetVisualStudioShortProductName%.pdb" (
	XCOPY /Y /D /V /Q "%OutPath%TestEngine\ORMSolutions.ORMArchitectSDK.TestEngine.%TargetVisualStudioShortProductName%.pdb" "%DevEnvDir%PrivateAssemblies\"
) ELSE (
	CALL:_CleanupFile "%DevEnvDir%PrivateAssemblies\ORMSolutions.ORMArchitectSDK.TestEngine.%TargetVisualStudioShortProductName%.pdb"
)

IF EXIST "%OutPath%ORMTestDriver.%TargetVisualStudioShortProductName%.exe" (
	XCOPY /Y /D /V /Q "%OutPath%ORMTestDriver.%TargetVisualStudioShortProductName%.exe" "%DevEnvDir%"
	ECHO F | XCOPY /Y /D /V /Q "%DevEnvDir%devenv.exe.config" "%DeveEnvDir%ORMTestDriver.%TargetVisualStudioShortProductName%.exe.config"
	IF EXIST "%OutPath%ORMTestDriver.%TargetVisualStudioShortProductName%.pdb" (
		XCOPY /Y /D /V /Q "%OutPath%ORMTestDriver.%TargetVisualStudioShortProductName%.pdb" "%DevEnvDir%"
	) ELSE (
		CALL:_CleanupFile "%DevEnvDir%ORMTestDriver.%TargetVisualStudioShortProductName%.pdb"
	)
)

IF EXIST "%OutPath%ORMTestReportViewer.%TargetVisualStudioShortProductName%.exe" (
	XCOPY /Y /D /V /Q "%OutPath%ORMTestReportViewer.%TargetVisualStudioShortProductName%.exe" "%DevEnvDir%"
	ECHO F | XCOPY /Y /D /V /Q "%DevEnvDir%devenv.exe.config" "%DevEnvDir%ORMTestReportViewer.%TargetVisualStudioShortProductName%.exe.config"
	if EXIST "%OutPath%ORMTestReportViewer.%TargetVisualStudioShortProductName%.pdb" (
		XCOPY /Y /D /V /Q "%OutPath%ORMTestReportViewer.%TargetVisualStudioShortProductName%.pdb" "%DevEnvDir%"
	) else (
		CALL:_CleanupFile "%DevEnvDir%ORMTestReportViewer.%TargetVisualStudioShortProductName%.pdb"
	)
)

XCOPY /Y /D /V /Q "%RootDir%ORMTestReport.xsd" "%DevEnvDir%..\..\Xml\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%ORMTestSuite.xsd" "%DevEnvDir%..\..\Xml\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%ORMTestSuiteReport.xsd" "%DevEnvDir%..\..\Xml\Schemas\"

GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

:SETVAR
SET %~1=%~2
GOTO:EOF

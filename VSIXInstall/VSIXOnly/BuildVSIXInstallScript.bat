@ECHO OFF
SETLOCAL
SET RootDir=%~dp0
CALL "%RootDir%..\..\SetupEnvironment.bat"

REM Create a batch file to install the vsix file. Most of the data here comes from the build script,
REM but we also need environment information like the VS Extensions directory and identity of the target
REM package that are only in the batch file SetupEnvironment variables.
REM 1) Full path to vsix installer
REM 2) Path from the project directory to the output directory, with trailing \. This is where the .vsix file is and where to write the installVSIX.bat file.
REM 3) Name of the vsix file (no directory name)
REM 4) VSIX sku name (Community, Pro, etc.)
REM 5) Major.Minor sku version (18.6, for example)

SET scriptFile=%RootDir%%~2installVSIX.bat
IF EXIST "%scriptFile%" DEL /F /Q "%scriptFile%"

REM For a non-per user install, the build process writes out schema files that are also
REM written by the installer. This can cause the installer to fail.
IF "%VSIXPerUser%"=="1" (
@echo "%~1" /rootSuffix:Exp /skuName:%~4 /skuVersion:%~5 /quiet /uninstall:%NORMAVsixIdentity% > "%scriptFile%"
@echo "%~1" /rootSuffix:Exp /skuName:%~4 /skuVersion:%~5 "%RootDir%%~2%~3" >> "%scriptFile%"
@echo DEL /F /Q "%VSIXExtensionsRootDir%\extensions.configurationchanged" >> "%scriptFile%"
) ELSE (
@echo "%~1" /rootSuffix:Exp /skuName:%~4 /skuVersion:%~5 /quiet /uninstall:%NORMAVsixIdentity% > "%scriptFile%"
@echo IF EXIST "%VSDir%Xml\Schemas\NORMACatalog.xml" DEL /F /Q "%VSDir%Xml\Schemas\NORMACatalog.xml" >> "%scriptFile%"
@echo IF EXIST "%VSDir%Xml\Schemas\ORM Solutions\NORMA" RMDIR /S /Q "%VSDir%Xml\Schemas\ORM Solutions\NORMA" >> "%scriptFile%"
@echo "%~1" /rootSuffix:Exp /skuName:%~4 /skuVersion:%~5 "%RootDir%%~2%~3" >> "%scriptFile%"
@echo DEL /F /Q "%VSIXExtensionsRootDir%\extensions.configurationchanged" >> "%scriptFile%"
)

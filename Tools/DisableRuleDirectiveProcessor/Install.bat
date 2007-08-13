@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

SET BinDir=%RootDir%\bin

CALL:_AddTextTemplateReg "%VSRegistryRootVersion%" "DisableRuleDirectiveProcessor"
IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_AddTextTemplateReg "%VSRegistryRootVersion%%VSRegistryRootSuffix%" "DisableRuleDirectiveProcessor")

GOTO:EOF

:_AddTextTemplateReg
REG ADD "HKLM\%VSRegistryRootBase%\%~1\TextTemplating\DirectiveProcessors\%~2" /f /ve /d "A directive processor that disables Rules."
REG ADD "HKLM\%VSRegistryRootBase%\%~1\TextTemplating\DirectiveProcessors\%~2" /f /v "Class" /d "Neumont.Tools.Modeling.%~2"
REG ADD "HKLM\%VSRegistryRootBase%\%~1\TextTemplating\DirectiveProcessors\%~2" /f /v "CodeBase" /d "%BinDir%\Neumont.Tools.Modeling.DisableRuleDirectiveProcessor.dll"
GOTO:EOF

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
REG ADD "HKLM\%VSRegistryRootBase%\%~1\TextTemplating\DirectiveProcessors\%~2" /f /v "Assembly" /d "Neumont.Tools.Modeling.DisableRuleDirectiveProcessor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

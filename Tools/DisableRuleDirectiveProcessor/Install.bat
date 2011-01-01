@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

SET BinDir=%RootDir%\bin

CALL:_AddTextTemplateReg HKLM "%VSRegistryRootVersion%" "DisableRuleDirectiveProcessor"
IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_AddTextTemplateReg HKLM "%VSRegistryRootVersion%%VSRegistryRootSuffix%" "DisableRuleDirectiveProcessor")
IF NOT "%VSRegistryConfigDecorator%"=="" (
	CALL:_AddTextTemplateReg "%VSRegistryConfigHive%" "%VSRegistryRootVersion%%VSRegistryConfigDecorator%" "DisableRuleDirectiveProcessor"
	IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_AddTextTemplateReg "%VSRegistryConfigHive%" "%VSRegistryRootVersion%%VSRegistryRootSuffix%%VSRegistryConfigDecorator%" "DisableRuleDirectiveProcessor")
)

GOTO:EOF

:_AddTextTemplateReg
REG ADD "%~1\%VSRegistryRootBase%\%~2\TextTemplating\DirectiveProcessors\%~3" /f /ve /d "A directive processor that disables Rules."
REG ADD "%~1\%VSRegistryRootBase%\%~2\TextTemplating\DirectiveProcessors\%~3" /f /v "Class" /d "Neumont.Tools.Modeling.%~3"
REG ADD "%~1\%VSRegistryRootBase%\%~2\TextTemplating\DirectiveProcessors\%~3" /f /v "Assembly" /d "Neumont.Tools.Modeling.DisableRuleDirectiveProcessor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

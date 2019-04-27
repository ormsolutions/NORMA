@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\..\..\SetupEnvironment.bat" %*

IF NOT "%VSSideBySide%"=="true" (
	GOTO:_NotSideBySide
)

:: Side by side installation, find and load private registry file
REG LOAD HKLM\VSHive%TargetVisualStudioInstallSuffix% "%LocalAppData%\Microsoft\VisualStudio\%TargetVisualStudioMajorMinorVersion%_%TargetVisualStudioInstallSuffix%\privateregistry.bin"
SET DirectiveProcessorsKey=HKLM\VSHive%TargetVisualStudioInstallSuffix%\Software\Microsoft\VisualStudio\%TargetVisualStudioMajorMinorVersion%_%TargetVisualStudioInstallSuffix%_Config\TextTemplating\DirectiveProcessors
CALL "%RootDir%\..\..\SetFromRegistry.bat" "DSLDirectiveProcessorLocation" "%DirectiveProcessorsKey%\DslDirectiveProcessor" "CodeBase" "dp"
XCOPY /Y /D /V /Q "%RootDir%\%BuildOutDir%\Neumont.Tools.Modeling.DisableRuleDirectiveProcessor.dll" "%DSLDirectiveProcessorLocation%"
REG ADD "%DirectiveProcessorsKey%\DisableRuleDirectiveProcessor" /f /ve /d "A directive processor that disables Rules."
REG ADD "%DirectiveProcessorsKey%\DisableRuleDirectiveProcessor" /f /v "Class" /d "Neumont.Tools.Modeling.DisableRuleDirectiveProcessor"
REG ADD "%DirectiveProcessorsKey%\DisableRuleDirectiveProcessor" /f /v "CodeBase" /d "%DSLDirectiveProcessorLocation%Neumont.Tools.Modeling.DisableRuleDirectiveProcessor.dll"
REG UNLOAD HKLM\VSHive%TargetVisualStudioInstallSuffix%
GOTO:EOF

:_NotSideBySide

CALL:_AddTextTemplateReg HKLM "%VSRegistryRootVersion%" "DisableRuleDirectiveProcessor"
IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_AddTextTemplateReg HKLM "%VSRegistryRootVersion%%VSRegistryRootSuffix%" "DisableRuleDirectiveProcessor")
IF NOT "%VSRegistryConfigDecorator%"=="" (
	CALL:_AddTextTemplateReg "%VSRegistryConfigHive%" "%VSRegistryRootVersion%%VSRegistryConfigDecorator%" "DisableRuleDirectiveProcessor"
	IF NOT "%VSRegistryRootSuffix%"=="" (CALL:_AddTextTemplateReg "%VSRegistryConfigHive%" "%VSRegistryRootVersion%%VSRegistryRootSuffix%%VSRegistryConfigDecorator%" "DisableRuleDirectiveProcessor")
)

GOTO:EOF

:_AddTextTemplateReg
REG ADD "%~1\%VSRegistryConfigRootBase%\%~2\TextTemplating\DirectiveProcessors\%~3" /f /ve /d "A directive processor that disables Rules."
REG ADD "%~1\%VSRegistryConfigRootBase%\%~2\TextTemplating\DirectiveProcessors\%~3" /f /v "Class" /d "Neumont.Tools.Modeling.%~3"
REG ADD "%~1\%VSRegistryConfigRootBase%\%~2\TextTemplating\DirectiveProcessors\%~3" /f /v "Assembly" /d "Neumont.Tools.Modeling.DisableRuleDirectiveProcessor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=957d5b7d5e79e25f"
GOTO:EOF

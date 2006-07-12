@ECHO OFF
SETLOCAL

SET BinDir=%~dp0bin

CALL:_AddTextTemplateReg "8.0" "DslImportDirectiveProcessor"
CALL:_AddTextTemplateReg "8.0Exp" "DslImportDirectiveProcessor"
CALL:_AddTextTemplateReg "8.0" "DslImportEndDirectiveProcessor"
CALL:_AddTextTemplateReg "8.0Exp" "DslImportEndDirectiveProcessor"

GOTO:EOF

:_AddTextTemplateReg
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\TextTemplating\DirectiveProcessors\%~2 /f /ve /d "A directive processor that helps import Dsl files."
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\TextTemplating\DirectiveProcessors\%~2 /f /v "Class" /d "Neumont.Tools.ORM.Framework.%~2"
REG ADD HKLM\SOFTWARE\Microsoft\VisualStudio\%~1\TextTemplating\DirectiveProcessors\%~2 /f /v "CodeBase" /d "%BinDir%\Neumont.Tools.ORM.Framework.DslImportDirectiveProcessor.dll"
GOTO:EOF


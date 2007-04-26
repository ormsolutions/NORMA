@ECHO OFF
SETLOCAL
SET RootDir=%~dp0.
CALL "%RootDir%\SetupEnvironment.bat" %*

IF EXIST "%NORMADir%\bin\Neumont.Tools.ORM.dll" (%RegPkg% /unregister "%NORMADir%\bin\Neumont.Tools.ORM.dll")

IF NOT EXIST "%NORMADir%" (SET RunDevEnvSetup=True)

CALL:_MakeDir "%NORMADir%\bin"
CALL:_RemoveDir "%NORMADir%\bin\1033"
CALL:_MakeDir "%NORMADir%\Help"
CALL:_MakeDir "%NORMADir%\Xml\Schemas"
CALL:_MakeDir "%NORMADir%\Xml\Transforms\Converters"
CALL:_MakeDir "%NORMADir%\Xml\Verbalization\Core"
CALL:_MakeDir "%NORMADir%\Xml\Verbalization\Report"
CALL:_RemoveDir "%NORMADir%\ORMProjectItems"
CALL:_RemoveDir "%ORMDir%\..\..\ORM"
CALL:_MakeDir "%ORMDir%\Schemas"
CALL:_MakeDir "%ORMDir%\Transforms"
CALL:_RemoveDir "%DILDir%\..\..\DIL"
CALL:_MakeDir "%DILDir%\Schemas"
CALL:_MakeDir "%DILDir%\Transforms"

XCOPY /Y /D /V /Q "%RootDir%\ORMModel\%BuildOutDir%\Neumont.Tools.ORM.dll" "%NORMADir%\bin\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\%BuildOutDir%\Neumont.Tools.ORM.pdb" "%NORMADir%\bin\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\%BuildOutDir%\Neumont.Tools.ORM.xml" "%NORMADir%\bin\"
CALL:_CleanupFile "%NORMADir%\bin\1033\Neumont.Tools.ORMUI.dll"

FOR %%A IN ("%RootDir%\ORMModel\Shell\ProjectItems\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%VSItemTemplatesDir%\%%~nA\ORMModel.zip"
FOR %%A IN ("%RootDir%\ORMModel\Shell\ProjectItems\Web\*.zip") DO ECHO F | XCOPY /Y /D /V /Q "%%~fA" "%VSItemTemplatesDir%\Web\%%~nA\ORMModel.zip"

XCOPY /Y /D /V /Q "%RootDir%\ORMModel\ObjectModel\ORM2Core.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\ShapeModel\ORM2Diagram.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\Shell\ORM2Root.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\OIAL\OIAL.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\OIAL\ORMDataTypes.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\OIAL\ORMDataTypes-Temporal.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\OIALtoPLiX\OIALtoPLiX_Properties.xsd" "%ORMDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\catalog.xml" "%ORMDir%\Schemas\"

XCOPY /Y /D /V /Q "%RootDir%\ORMModel\Shell\ORMDesignerSettings.xsd" "%NORMADir%\Xml\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\Shell\catalog.xml" "%NORMADir%\Xml\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\Shell\ORMDesignerSettings.xml" "%NORMADir%\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\Shell\Converters\*.xslt" "%NORMADir%\Xml\Transforms\Converters\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\ObjectModel\VerbalizationUntypedSnippets.xsd" "%NORMADir%\Xml\Verbalization\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\ObjectModel\VerbalizationCoreSnippets\*.x??" "%NORMADir%\Xml\Verbalization\Core\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\ObjectModel\VerbalizationReportSnippets\*.x??" "%NORMADir%\Xml\Verbalization\Report\"
XCOPY /Y /D /V /Q "%RootDir%\ORMModel\ObjectModel\VerbalizationCoreSnippetsDocumentation.html" "%NORMADir%\Xml\Verbalization\Core\"
IF EXIST "%NORMADir%\Xml\Verbalization\Core\VerbalizationCoreSnippets.xml" (
	CALL:_CleanupFile "%NORMADir%\Xml\Verbalization\Core\_default.xml"
	REN "%NORMADir%\Xml\Verbalization\Core\VerbalizationCoreSnippets.xml" "_default.xml"
)
IF EXIST "%NORMADir%\Xml\Verbalization\Core\VerbalizationCoreReportSnippets.xml" (
	CALL:_CleanupFile "%NORMADir%\Xml\Verbalization\Core\DefaultReportSnippets.xml"
	REN "%NORMADir%\Xml\Verbalization\Core\VerbalizationCoreReportSnippets.xml" "DefaultReportSnippets.xml"
)
IF EXIST "%NORMADir%\Xml\Verbalization\Report\VerbalizationReportSnippets.xml" (
	CALL:_CleanupFile "%NORMADir%\Xml\Verbalization\Report\_default.xml"
	REN "%NORMADir%\Xml\Verbalization\Report\VerbalizationReportSnippets.xml" "_default.xml"
)

XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DIL.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DILDT.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DILEP.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DILMS.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DMIL.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DCIL.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\DDIL.xsd" "%DILDir%\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\XML\DIL\catalog.xml" "%DILDir%\Schemas\"

XCOPY /Y /D /V /Q "%RootDir%\Setup\NORMASchemaCatalog.xml" "%VSDir%\Xml\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\Setup\ORMSchemaCatalog.xml" "%VSDir%\Xml\Schemas\"
XCOPY /Y /D /V /Q "%RootDir%\Setup\DILSchemaCatalog.xml" "%VSDir%\Xml\Schemas\"

%RegPkg% "%NORMADir%\bin\Neumont.Tools.ORM.dll"

REG DELETE "HKLM\%VSRegistryRoot%\InstalledProducts\Neumont ORM Architect" /v "UseRegNameAsSplashName" /f 1>NUL

REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect" /v "SettingsPath" /d "%NORMADir%\ORMDesignerSettings.xml" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect" /v "ConvertersDir" /d "%NORMADir%\Xml\Transforms\Converters\\" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\Neumont\ORM Architect" /v "VerbalizationDir" /d "%NORMADir%\Xml\Verbalization\\" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\FontAndColors\Orm Designer" /v "Category" /d "{663DE24F-8E3A-4C0F-A307-53053ED6C59B}" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\FontAndColors\Orm Designer" /v "Package" /d "{C5AA80F8-F730-4809-AAB1-8D925E36F9F5}" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\FontAndColors\Orm Verbalizer" /v "Category" /d "{663DE24F-5A08-4490-80E7-EA332DFFE7F0}" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\FontAndColors\Orm Verbalizer" /v "Package" /d "{C5AA80F8-F730-4809-AAB1-8D925E36F9F5}" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\ShellFileAssociations\.orm" /ve /d "ormfile" /f 1>NUL

REG ADD "HKLM\%VSRegistryRoot%\XmlDesigners\{EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B}" /ve /d "ORM Designer" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\XmlDesigners\{EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B}" /v "Editor" /d "EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\XmlDesigners\{EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B}" /v "Extension" /d "orm" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\XmlDesigners\{EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B}" /v "LogicalView" /d "7651A702-06E5-11D1-8EBD-00A0C90F26EA" /f 1>NUL
REG ADD "HKLM\%VSRegistryRoot%\XmlDesigners\{EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B}" /v "Namespace" /d "http://schemas.neumont.edu/ORM/2006-04/ORMRoot" /f 1>NUL

REG ADD "HKCR\MIME\Database\Content Type\application/orm+xml" /v "Extension" /d ".orm" /f 1>NUL
REG ADD "HKCR\.orm" /ve /d "ormfile" /f 1>NUL
REG ADD "HKCR\.orm" /v "Content Type" /d "application/orm+xml" /f 1>NUL
REG ADD "HKCR\ormfile" /ve /d "Object-Role Modeling File" /f 1>NUL
REG ADD "HKCR\ormfile\DefaultIcon" /ve /d "%NORMADir%\bin\Neumont.Tools.ORM.dll,0" /f 1>NUL
REG ADD "HKCR\ormfile\shell\open" /ve /d "&Open" /f 1>NUL
REG ADD "HKCR\ormfile\shell\open\command" /ve /d "\"%VSEnvironmentPath%\" /RootSuffix \"%VSRegistryRootSuffix%\" /dde \"%%1\"" /f 1>NUL
REG ADD "HKCR\ormfile\shell\open\ddeexec" /ve /d "Open(\"%%1\")" /f 1>NUL
REG ADD "HKCR\ormfile\shell\open\ddeexec\application" /ve /d "VisualStudio.8.0" /f 1>NUL
REG ADD "HKCR\ormfile\shell\open\ddeexec\topic" /ve /d "system" /f 1>NUL


IF /I "%RunDevEnvSetup%"=="True" (ECHO Running 'devenv.exe /RootSuffix "%VSRegistryRootSuffix%" /Setup'... This may take a few minutes... && "%VSEnvironmentPath%" /RootSuffix "%VSRegistryRootSuffix%" /Setup)

GOTO:EOF


:_MakeDir
IF NOT EXIST "%~1" (MKDIR "%~1")
GOTO:EOF

:_RemoveDir
IF EXIST "%~1" (RMDIR /S /Q "%~1")
GOTO:EOF

:_CleanupFile
IF EXIST "%~1" (DEL /F /Q "%~1")
GOTO:EOF

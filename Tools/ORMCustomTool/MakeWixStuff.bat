@ECHO OFF
:: This batch script will generate the appropriate WiX for registering ORM Generators.
:: To use:
:: 1) Copy in all "CALL:_AddXslORMGenerator" lines from Install.bat
:: 2) Search-and-replace "%ORMTransformsDir%\" with ""
:: 3) Search-and-replace "%DILTransformsDir%\" with ""
:: 4) Search-and-replace "%PLiXDir%\" with "[NEUMONTCOMMONFILESDIR]\PLiX\"
:: 5) Save this batch file and run it
:: 6) DO NOT CHECK IN THE CHANGES YOU MAKE TO THIS FILE!


:: The "CALL:_AddXslORMGenerator" lines go here


GOTO:EOF


:_AddXslORMGenerator
ECHO ^<Registry Action="createKeyAndRemoveKeyOnUninstall" Root="HKLM" Key="$(var.NORMARegRoot)\Generators\%~1"^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="Type" Value="XSLT"/^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="OfficialName" Value="%~1"/^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="DisplayName" Value="%~2"/^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="DisplayDescription" Value="%~3"/^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="FileExtension" Value="%~4"/^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="SourceInputFormat" Value="%~5"/^> >> Output.WiX.xml
ECHO 	^<Registry Type="string" Name="ProvidesOutputFormat" Value="%~6"/^> >> Output.WiX.xml
SET TmpTransformUri=%~7
IF "%TmpTransformUri:[NEUMONTCOMMONFILESDIR]=%"=="%~7" (SET TmpTransformURI=[#%TmpTransformUri:\=_%]) ELSE (SET TmpTransformUri=%~7)
ECHO 	^<Registry Type="string" Name="TransformUri" Value="%TmpTransformUri%"/^> >> Output.WiX.xml
SET TmpTransformUri=
IF NOT "%~8"=="" (ECHO 	^<Registry Type="string" Name="CustomTool" Value="%~8"/^> >> Output.WiX.xml)
IF NOT "%~9"=="" (ECHO 	^<Registry Type="integer" Name="GeneratesSupportFile" Value="%~9"/^> >> Output.WiX.xml)
SHIFT /8
IF NOT "%~9"=="" (ECHO 	^<Registry Type="integer" Name="Compilable" Value="%~9"/^> >> Output.WiX.xml)
SHIFT /8
IF NOT "%~9"=="" (CALL:_GenerateMultiString "ReferenceInputFormats" "%~9")
SHIFT /8
IF NOT "%~9"=="" (CALL:_GenerateMultiString "PrerequisiteInputFormats" "%~9")
ECHO ^</Registry^> >> Output.WiX.xml
ECHO. >> Output.WiX.xml
ECHO. >> Output.WiX.xml
GOTO:EOF

:_GenerateMultiString
ECHO 	^<Registry Type="multiString" Name="%~1"^> >> Output.WiX.xml
FOR /F "usebackq tokens=1-26 delims=\0" %%A IN ('%~2') DO (CALL:_GenerateMultiStringBody "%%A" "%%B" "%%C" "%%D" "%%E" "%%F" "%%G" "%%H" "%%I" "%%J" "%%K" "%%L" "%%M" "%%N" "%%O" "%%P" "%%Q" "%%R" "%%S" "%%T" "%%U" "%%V" "%%W" "%%X" "%%Y" "%%Z")
ECHO 	^</Registry^> >> Output.WiX.xml
GOTO:EOF

:_GenerateMultiStringBody
IF "%~1"=="" (GOTO:EOF)
ECHO 		^<RegistryValue^>%~1^</RegistryValue^> >> Output.WiX.xml
SHIFT
GOTO:_GenerateMultiStringBody


@echo off
set rootPath=%1
if '%2'=='' (
set envPath="C:\Program Files\Microsoft Visual Studio 8\"
) else (
set envPath=%2
)
copy /y %rootPath%"ORMModel\bin\Debug\Northface.Tools.ORM.dll" %envPath%"Common7\IDE\PrivateAssemblies\"
copy /y %rootPath%"ORMModel\bin\Debug\Northface.Tools.ORM.pdb" %envPath%"Common7\IDE\PrivateAssemblies\"
copy /y %rootPath%"ORMModelSatDll\Debug\Northface.Tools.ORMUI.dll" %envPath%"Common7\IDE\PrivateAssemblies\1033"
copy /y %rootPath%"ORMModelSatDll\Debug\Northface.Tools.ORMUI.pdb" %envPath%"Common7\IDE\PrivateAssemblies\1033"
copy /y %rootPath%"ORMModel\Shell\ProjectItems\ORMProjectItems.vsdir" %envPath%"VC#\CSharpProjectItems"
copy /y %rootPath%"ORMModel\Shell\ProjectItems\ORMModel.orm" %envPath%"VC#\CSharpProjectItems"
copy /y %rootPath%"ORMModel\Shell\ProjectItems\FactEditor.fct" %envPath%"VC#\CSharpProjectItems"
copy /y %rootPath%"ORMModel\Shell\ProjectItems\ORMProjectItems.vsdir" %envPath%"Common7\IDE\NewFileItems"
copy /y %rootPath%"ORMModel\Shell\ProjectItems\ORMModel.orm" %envPath%"Common7\IDE\NewFileItems"
copy /y %rootPath%"ORMModel\Shell\ProjectItems\FactEditor.fct" %envPath%"Common7\IDE\NewFileItems"
regedit /s %rootPath%ORMDesigner.vrg
regedit /s %rootPath%FactEditor.vrg

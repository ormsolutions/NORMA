Neumont Object-Role Modeling Architect
Copyright © Neumont University. All rights reserved.
Copyright © ORM Solutions, LLC. All rights reserved.
Homepage: http://orm.sourceforge.net

For licensing terms, see the adjacent LICENSE.txt file.


Several third-party components are required to build or compile the main
portion of this software. These components, and the locations from which
they can be obtained, are as follows:

PLiX - Programming Language in XML
	Homepage: https://sourceforge.net/projects/plix

Microsoft Visual Studio SDK
	Homepage: http://www.microsoft.com/extendvs
	Download (Visual Studio 2005): http://go.microsoft.com/fwlink/?LinkId=73702
	Download (Visual Studio 2008): http://www.microsoft.com/en-us/download/details.aspx?id=508
	Download (Visual Studio 2010): http://www.microsoft.com/en-us/download/details.aspx?id=2680
	Download (Visual Studio 2012): http://www.microsoft.com/en-us/download/details.aspx?id=30668
	Download (Visual Studio 2013): http://www.microsoft.com/en-us/download/details.aspx?id=40758
	Visual Studio 2015 installs the SDK with the normal VS setup. Choose a custom setup and select 'Visual Studio Extensibility'
IMPORTANT: You will need to establish the Visual Studio experimental hive before building NORMA by running Visual Studio once in this environment. Use the link provided by the VS SDK, or run 'devenv.exe /RootSuffix Exp' from a Visual Studio command prompt. For Visual Studio 2008, make sure you use either the provided shortcut or add /RANU (run as normal user) to the command line. After running FirstTimeBuildVS2008.bat you should use 'devenv /rootsuffix Exp' instead of 'devenv /rootsuffix Exp /RANU'.


Microsoft Visual Studio Modeling and Visualization Tools (DSL Tools SDK), installs after the primary SDK
	Download (Visual Studio 2010): http://www.microsoft.com/en-us/download/details.aspx?id=23025
	Download (Visual Studio 2012): http://www.microsoft.com/en-us/download/details.aspx?id=30680
	Download (Visual Studio 2013): http://www.microsoft.com/en-us/download/details.aspx?id=40754
	Download (Visual Studio 2015): http://www.microsoft.com/en-us/download/details.aspx?id=48148


Additional components used by other portions of this software include:

Windows Installer XML (WiX) toolset (v3.10, tested successfully through v3.10.0.2026)
	Homepage: http://wix.sourceforge.net

NUnit (v2.4.7 or later)
	Homepage: http://www.nunit.org
	Download: http://www.nunit.org/index.php?p=download

Microsoft XML Diff and Patch 1.0
	Homepage: http://apps.gotdotnet.com/xmltools/xmldiff
	Download: http://download.microsoft.com/download/xml/patch/1.0/wxp/en-us/xmldiffpatch.exe

Microsoft FxCop
	Homepage: http://gotdotnet.com/team/fxcop


The build is divided into multiple components:
	Main - The core NORMA tool and various extensions. Everything that is installed on an end-user's machine, except Help.
	DevTools - Various MSBuild targets, DSL Tools directive processors, and other developer-only projects. These are updated infrequently, and do not often need to be rebuilt.
	Help - The help and documentation for NORMA. Requires Help Studio Lite (part of the Visual Studio SDK).
	Tests - The test engine and various test suites. Note that this only builds the tests, it does not actually run them. Requires NUnit and Microsoft XML Diff and Patch.
	Setup - The end-user installer for NORMA. Requires WiX.

Several build scripts are available for building common combinations of the NORMA components:
	Build.bat - Main
	BuildDevTools.bat - DevTools
	FirstTimeBuild.bat - DevTools, Main
	BuildHelp.bat - Help
	BuildSetup.bat - Main, Help, Setup
	BuildTests.bat - Tests
	BuildAll.bat - Main, Help, Setup, Tests
	FirstTimeBuildAll.bat - DevTools, Main, Help, Setup, Tests

To set the version of Visual Studio that NORMA is being built for, use the TargetVisualStudioVersion environment variable. The supported values are:
SET TargetVisualStudioVersion=v8.0
SET TargetVisualStudioVersion=v9.0
SET TargetVisualStudioVersion=v10.0
SET TargetVisualStudioVersion=v11.0
SET TargetVisualStudioVersion=v12.0
SET TargetVisualStudioVersion=v14.0

These values correspond to Visual Studio 20xx where v8.0=2005, v9.0=2008, v10.0=2010, v11.0=2012, v12.0=2013, v14.0=2015.

See the comments in SetupEnvironment.bat for additional details on how the options are used.

Notes on building and debugging with VS2008 through VS2015:
The project files (.csproj, etc) are multitargeted to work correctly in Visual Studio 2005 and higher. However, the solution files (.sln) have slightly different formats in each VS version. If you open a VS2005 solution file in VS2008 then you will be prompted to upgrade. *.VS2008.sln files are provided as companions to all *.sln files for use in VS2008, and you will find corresponding .sln files for each successive version of VisualStudio. However, the *.VS2008.sln files are not sufficient for successfully building in VS2008 (etc).

You must set the TargetVisualStudioVersion to the correct version (listed above) before opening a NORMA project file from any of the Visual Studio IDE environments. There is also registry information that needs to be set so that the ToolsVersion setting the project files match the target environment. The easiest way to do this (after the initial batch files mentioned above have completed successfully) is with the following steps. You may want to put these steps into an easily accessible batch file. The example given is for Visual Studio 2008, but 2010, 2012, 2013, and 2015 work similarly.

1) Open a Visual Studio 2008 Command Prompt (as an Administrator)
2) Navigate to your NORMA root code directory
3) Execute the VS2008.bat batch file in the command prompt. Each of these batch files (matching the VS20xx environment you're opening) do two things. First, it sets the TargetVisualStudioVersion variable to the appropriate value. Second, it updates the registry to copy the contents of the tools version matching your system to the tools version 12.34. All of the NORMA .csproj files use ToolsVersion="12.34" (a fake number), and modifying the contents of the corresponding registry key allows the same .csproj file to be used for all of the visual studio versions.
4) devenv ORMPackage.VS2008.sln

VS2015 builds are exhibiting an additional issue where the VSCT.exe file used during the build cannot load VSCTCompress.dll. The only current solution for this is to include the directory containing this file on your path. With the Visual Studio SDK installed you should have an environment variable call VSSDK140Install. The VSCT directory is %VSSDK140Install%\VisualStudioIntegration\Tools\Bin. The VS2015.bat file has been extended to automatically extend the path in your local command prompt as needed (be sure to run as an admin or the batch file will not complete).

To build from the command line for VS2008:
1) Open a 'Visual Studio 2008 Command Prompt'
2) Navigate to your NORMA root code directory
3) Choose one of the batch files mentioned above, using the batch file ending in VS2008.bat (BuildVS2008.bat, etc)

After getting a new drop, we recommend you use 'Build /t:Rebuild' (or BuildVS2008 /t:Rebuild) to update your files.

You can build the VS2005 pieces using a VS2008 SDK installation (plus a handful of files) by defining the TargetVisualStudioVersion=v8.0 environment variable before running a *VS2008 batch file. The exact details of this approach are still being worked out, but you should only need to do this if you are building installation packages.

Notes on building and debugging with VS2010:
The general directions are the same as for the VS2008 approach (open a Visual Studio 2010 Command Prompt, navigate to the NORMA directory, set TargetVisualStudioVersion=v10.0, and use the *VS2010* solutions and batch files).

Notes on building and debugging with VS2012:
The general directions are the same as for the VS2008 approach (open a Visual Studio 2012 Command Prompt, navigate to the NORMA directory, set TargetVisualStudioVersion=v11.0, and use the *VS2012* solutions and batch files).

The installation process on VS2012 is not as easy as on VS2010, which simply required extension files to be copied into place. You will likely to need run devenv /setup for VS2012 regardless of what you do with the Visual Studio installations. See additional readme information in the VSIXInstall/VS2012 directory.

Notes on building and debugging with VS2013:
The general directions are the same as for the VS2008 approach (open a Visual Studio 2013 Command Prompt, navigate to the NORMA directory, set TargetVisualStudioVersion=v12.0, and use the *VS2013* solutions and batch files).

The installation process on VS2013 is not as easy as on VS2010, which simply required extension files to be copied into place. You will likely to need run devenv /setup for VS2013 regardless of what you do with the Visual Studio installations. See additional readme information in the VSIXInstall/VS2013 directory.

If NORMA is not functioning after the first dev build (the first clue will be in the file new dialog, which will simply say 'ORMModel' instead of 'Object-Role Modeling File') then you will need to reset the VSIX installation following the directions from the readme in the VSIXInstall/VS2013 directory. This should be required only after the first build, or in situations indicated in the readme.

Notes on building and debugging with VS2015:
The general directions are the same as for the VS2008 approach (open a Visual Studio 2015 Command Prompt, navigate to the NORMA directory, set TargetVisualStudioVersion=v14.0, and use the *VS2015* solutions and batch files).

The installation process on VS2015 is not as easy as on VS2010, which simply required extension files to be copied into place. You will likely to need run devenv /setup for VS2015 regardless of what you do with the Visual Studio installations. See additional readme information in the VSIXInstall/VS2015 directory.

If NORMA is not functioning after the first dev build (the first clue will be in the file new dialog, which will simply say 'ORMModel' instead of 'Object-Role Modeling File') then you will need to reset the VSIX installation following the directions from the readme in the VSIXInstall/VS2015 directory. This should be required only after the first build, or in situations indicated in the readme.


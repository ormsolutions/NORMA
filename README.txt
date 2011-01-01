Neumont Object-Role Modeling Architect
Copyright © Neumont University. All rights reserved.
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
	Download (Visual Studio 2008): http://www.microsoft.com/downloads/details.aspx?familyid=30402623-93ca-479a-867c-04dc45164f5b&displaylang=en


Additional components used by other portions of this software include:

Windows Installer XML (WiX) toolset (v3.5, tested successfully through 2415)
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

To set the version of Visual Studio that NORMA is being built for, use the TargetVisualStudioVersion environment variable. The two supported values are:
SET TargetVisualStudioVersion=v8.0
SET TargetVisualStudioVersion=v9.0

See the comments in SetupEnvironment.bat for additional details on how the options are used.

Notes on building and debugging with VS2008:
The project files (.csproj, etc) are multitargeted to work correctly in either Visual Studio 2005 or 2008. However, the solution files (.sln) have slightly different formats in VS2005 and VS2008. If you open a VS2005 solution file in VS2008 then you will be prompted to upgrade. *.VS2008.sln files are provided as companions to all *.sln files for use in VS2008. However, the *.VS2008.sln files are not sufficient for successfully building in VS2008.

You must set the TargetVisualStudioVersion to v9.0 to successfully target VS2008 from the VS2008 IDE. The easiest way to do this is to (after the initial batch files mentioned above have completed successfully) is with the following steps. You may want to put these steps into an easily accessible batch file.
1) Open a Visual Studio 2008 Command Prompt
2) SET TargetVisualStudioVersion=v9.0
3) Navigate to your NORMA root code directory
4) devenv ORMPackage.VS2008.sln

To build from the command line for VS2008:
1) Open a 'Visual Studio 2008 Command Prompt'
2) Navigate to your NORMA root code directory
3) Choose one of the batch files mentioned above, using the batch file ending in VS2008.bat (BuildVS2008.bat, etc)

After getting a new drop, we recommend you use 'Build /t:Rebuild' (or BuildVS2008 /t:Rebuild) to update your files.

You can build the VS2005 pieces using a VS2008 SDK installation (plus a handful of files) by defining the TargetVisualStudioVersion=v8.0 environment variable before running a *VS2008 batch file. The exact details of this approach are still being worked out, but you should only need to do this if you are building installation packages.

Notes on building and debugging with VS2010:
The general directions are the same as for the VS2008 approach (open a Visual Studio 2010 Command Prompt, navigate to the NORMA directory, set TargetVisualStudioVersion=v10.0, and use the *VS2010* solutions and batch files).


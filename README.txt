Neumont Object-Role Modeling Architect
Copyright © Neumont University. All rights reserved.
Homepage: http://orm.sourceforge.net

For licensing terms, see the adjacent LICENSE.txt file.


Several third-party components are required to build or compile the main
portion of this software. These components, and the locations from which
they can be obtained, are as follows:

PLiX - Programming Language in XML
	Homepage: https://sourceforge.net/projects/plix

Microsoft Visual Studio 2005 SDK (v4.0 or later)
	Homepage: http://www.microsoft.com/extendvs
	Download: http://go.microsoft.com/fwlink/?LinkId=73702


Additional components used by other portions of this software include:

Windows Installer XML (WiX) toolset (v3.0 or later)
	Homepage: http://wix.sourceforge.net

NUnit (v2.2.9 or later)
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

To set the version of Visual Studio that NORMA is being built for, use the TargetVisualStudioVersion environment variable.
See the comments in SetupEnvironment.bat for the options that are available.

# Natural Object-Role Modeling Architect

Copyright &copy; Neumont University. All rights reserved.
Copyright &copy; ORM Solutions, LLC. All rights reserved.
[Homepage](https://github.com/ormsolutions/NORMA)
[Original Project](https://orm.sourceforge.net)

NORMA is a plugin to Visual Studio for designing relational databases
using Object-Role Modeling version 2 standard, a fact-based conceptual
modeling approach. Object-Role Models are easier for people to
understand, validate adapt than the corresponding Entity-Relationship
Models.

## Licensing

For licensing terms, see the adjacent [LICENSE.txt](LICENSE.txt) file.

## Building

### Third-party components

Several third-party components are required to build or compile the main portion of this software prior to Visual Studio 2017 (instructions for the newer environments are at the end of this file). These components can be obtained as follows:

* PLiX - Programming Language in XML

  * [Original Project](https://sourceforge.net/projects/plix)
  * [Current Source](https://github.com/ormsolutions/PLiX)
  * Through VS2015 this is included as a separate .msi file in the NORMA download installers. The NORMA project itself uses PLiX for many of its code generation needs, so this .msi should be installed to make code modifications.
  * See VS2017/VS2019 notes for how to get this installed from the Visual Studio Marketplace.

* [Microsoft Visual Studio SDK](http://www.microsoft.com/extendvs)

  * [Download for Visual Studio 2005](http://go.microsoft.com/fwlink/?LinkId=73702)
  * [Download for Visual Studio 2008](http://www.microsoft.com/en-us/download/details.aspx?id=508)
  * [Download for Visual Studio 2010](http://www.microsoft.com/en-us/download/details.aspx?id=2680)
  * [Download (Visual Studio 2012)](http://www.microsoft.com/en-us/download/details.aspx?id=30668)
  * [Download (Visual Studio 2013)](http://www.microsoft.com/en-us/download/details.aspx?id=40758)
  * Visual Studio 2015 installs the SDK with the normal VS setup. Choose a custom setup and select 'Visual Studio Extensibility'

  IMPORTANT: You will need to establish the Visual Studio experimental
  hive before building NORMA by running Visual Studio once in this
  environment. Use the link provided by the VS SDK, or run `devenv.exe
  /RootSuffix Exp` from a Visual Studio command prompt. For Visual
  Studio 2008, make sure you use either the provided shortcut or add
  `/RANU` (run as normal user) to the command line. After running
  FirstTimeBuildVS2008.bat you should use `devenv /rootsuffix Exp`
  instead of `devenv /rootsuffix Exp /RANU`.

* Microsoft Visual Studio Modeling and Visualization Tools (DSL Tools
  SDK), installs after the primary SDK

  * [Download for Visual Studio 2010](http://www.microsoft.com/en-us/download/details.aspx?id=23025)
  * [Download for Visual Studio 2012](http://www.microsoft.com/en-us/download/details.aspx?id=30680)
  * [Download for Visual Studio 2013](http://www.microsoft.com/en-us/download/details.aspx?id=40754)
  * [Download for Visual Studio 2015](http://www.microsoft.com/en-us/download/details.aspx?id=48148)
  * For Visual Studio 2017 and 2019, the Visual Studio SDK and
    modeling tools are installed with the 'Visual Studio extension
    development' area near the bottom of the setup page. You will need
    to add the optional 'Modeling SDK' component under this area.

* Additional components used by other portions of this software
  include:

  * [Windows Installer XML (WiX) toolset](http://wix.sourceforge.net) (v3.10, tested successfully through v3.10.0.2026)
  * [NUnit (v2.4.7 or later)](http://www.nunit.org)
  * [Microsoft XML Diff and Patch
	1.0](http://apps.gotdotnet.com/xmltools/xmldiff):
	[download](http://download.microsoft.com/download/xml/patch/1.0/wxp/en-us/xmldiffpatch.exe)
  * [Microsoft FxCop](http://gotdotnet.com/team/fxcop)

### Components of NORMA

* The build is divided into multiple components:
  * **Main** - The core NORMA tool and various extensions. Everything that
    is installed on an end-user's machine, except Help.
  * **DevTools** - Various MSBuild targets, DSL Tools directive
    processors, and other developer-only projects. These are updated
    infrequently, and do not often need to be rebuilt.
  * **Help** - The help and documentation for NORMA. Requires Help Studio
    Lite (part of the Visual Studio SDK).
  * **Tests** - The test engine and various test suites. Note that this
    only builds the tests, it does not actually run them. Requires
    NUnit and Microsoft XML Diff and Patch.
  * **Setup** - The end-user installer for NORMA. Requires WiX.

Several build scripts are available for building common combinations
of the NORMA components:

  * [Build.bat](Build.bat) - Main
  * [BuildDevTools.bat](BuildDevTools.bat) - DevTools
  * [FirstTimeBuild.bat](FirstTimeBuild.bat) - DevTools, Main
  * [BuildHelp.bat](BuildHelp.bat) - Help
  * [BuildSetup.bat](BuildSetup.bat) - Main, Help, Setup
  * [BuildTests.bat](BuildTests.bat) - Tests
  * [BuildAll.bat](BuildAll.bat) - Main, Help, Setup, Tests
  * [FirstTimeBuildAll.bat](FirstTimeBuildAll.bat) - DevTools, Main, Help, Setup, Tests

To set the version of Visual Studio that NORMA is being built for, use
the `TargetVisualStudioVersion` environment variable. The supported
values are:

``` batchfile
SET TargetVisualStudioVersion=v8.0
SET TargetVisualStudioVersion=v9.0
SET TargetVisualStudioVersion=v10.0
SET TargetVisualStudioVersion=v11.0
SET TargetVisualStudioVersion=v12.0
SET TargetVisualStudioVersion=v14.0
SET TargetVisualStudioVersion=v15.0
SET TargetVisualStudioVersion=v16.0
```

Note that these are set for you with the `VS20xx.bat` files discussed
below, which we strongly recommend you run before launching Visual
Studio from a command prompt.

These values correspond to Visual Studio 20xx:

| TargetVisualStudioVersion | VS version name |
| ------------------------- | --------------- |
| v8.0                      | 2005            |
| v9.0                      | 2008            |
| v10.0                     | 2010            |
| v11.0                     | 2012            |
| v12.0                     | 2013            |
| v14.0                     | 2015            |
| v15.0                     | 2017            |
| v16.0                     | 2019            |

See the comments in [SetupEnvironment.bat](SetupEnvironment.bat) for
additional details on how the options are used.

## Notes on building and debugging specific to VS versions

### VS2008 through VS2015:

The project files (`.csproj`, etc) are multitargeted to work correctly
in Visual Studio 2005 and higher. However, the solution files (`.sln`)
have slightly different formats in each VS version. If you open a
VS2005 solution file in VS2008 then you will be prompted to
upgrade. `*.VS2008.sln` files are provided as companions to all
`*.sln` files for use in VS2008, and you will find corresponding
`.sln` files for each successive version of VisualStudio. However, the
`*.VS2008.sln` files are not sufficient for successfully building in
VS2008 (etc).

You must set the `TargetVisualStudioVersion` to the correct version
(listed above) before opening a NORMA project file from any of the
Visual Studio IDE environments. There is also registry information
that needs to be set so that the `ToolsVersion` setting the project
files match the target environment. The easiest way to do this (after
the initial batch files mentioned above have completed successfully)
is with the following steps. You may want to put these steps into an
easily accessible batch file. The example given is for Visual Studio
2008, but 2010, 2012, 2013, and 2015 work similarly.

1. Open a Visual Studio 2008 Command Prompt (as an Administrator)
2. Navigate to your NORMA root code directory
3. Execute the `VS2008.bat` batch file in the command prompt. Each of
   these batch files (matching the `VS20xx` environment you're
   opening) do two things. First, it sets the
   TargetVisualStudioVersion variable to the appropriate
   value. Second, it updates the registry to copy the contents of the
   tools version matching your system to the tools version
   `12.34`. All of the NORMA `.csproj` files use
   `ToolsVersion="12.34"` (a fake number), and modifying the contents
   of the corresponding registry key allows the same `.csproj` file to
   be used for all of the visual studio versions.
4. `devenv ORMPackage.VS2008.sln`

VS2015 builds are exhibiting an additional issue where the `VSCT.exe`
file used during the build cannot load `VSCTCompress.dll`. The only
current solution for this is to include the directory containing this
file on your path. With the Visual Studio SDK installed you should
have an environment variable call `VSSDK140Install`. The VSCT
directory is `%VSSDK140Install%\VisualStudioIntegration\Tools\Bin`. The
`VS2015.bat` file has been extended to automatically extend the path
in your local command prompt as needed (be sure to run as an admin or
the batch file will not complete).

To build from the command line for VS2008:

1. Open a 'Visual Studio 2008 Command Prompt'
2. Navigate to your NORMA root code directory
3. Choose one of the batch files mentioned above, using the batch file
   ending in `VS2008.bat` (`BuildVS2008.bat`, etc)

After getting a new drop, we recommend you use `Build /t:Rebuild` (or
`BuildVS2008 /t:Rebuild`) to update your files.

You can build the VS2005 pieces using a VS2008 SDK installation (plus
a handful of files) by defining the `TargetVisualStudioVersion=v8.0`
environment variable before running a `*VS2008` batch file. The exact
details of this approach are still being worked out, but you should
only need to do this if you are building installation packages.

### Notes on building and debugging with VS2010:

The general directions are the same as for the VS2008 approach (open a
Visual Studio 2010 Command Prompt, navigate to the NORMA directory,
`set TargetVisualStudioVersion=v10.0`, and use the *VS2010* solutions
and batch files).

### Notes on building and debugging with VS2012:

The general directions are the same as for the VS2008 approach (open a
Visual Studio 2012 Command Prompt, navigate to the NORMA directory,
`set TargetVisualStudioVersion=v11.0`, and use the *VS2012* solutions
and batch files).

The installation process on VS2012 is not as easy as on VS2010, which
simply required extension files to be copied into place. You will
likely to need run `devenv /setup` for VS2012 regardless of what you
do with the Visual Studio installations. See additional readme
information in the (VSIXInstall/VS2012)[VSIXInstall/VS2012] directory.

### Notes on building and debugging with VS2013:

The general directions are the same as for the VS2008 approach (open a
Visual Studio 2013 Command Prompt, navigate to the NORMA directory,
`set TargetVisualStudioVersion=v12.0`, and use the *VS2013* solutions
and batch files).

The installation process on VS2013 is not as easy as on VS2010, which
simply required extension files to be copied into place. You will
likely to need run `devenv /setup` for VS2013 regardless of what you
do with the Visual Studio installations. See additional readme
information in the (VSIXInstall/VS2013](VSIXInstall/VS2013) directory.

If NORMA is not functioning after the first dev build (the first clue
will be in the file new dialog, which will simply say 'ORMModel'
instead of 'Object-Role Modeling File') then you will need to reset
the VSIX installation following the directions from the readme in the
[VSIXInstall/VS2013](VSIXInstall/VS2013) directory. This should be
required only after the first build, or in situations indicated in the
readme.

### Notes on building and debugging with VS2015:

The general directions are the same as for the VS2008 approach (open a
Visual Studio 2015 Command Prompt, navigate to the NORMA directory,
`set TargetVisualStudioVersion=v14.0`, and use the *VS2015* solutions
and batch files).

The installation process on VS2015 is not as easy as on VS2010, which
simply required extension files to be copied into place. You will
likely to need run `devenv /setup` for VS2015 regardless of what you
do with the Visual Studio installations. See additional readme
information in the (VSIXInstall/VS2015](VSIXInstall/VS2015) directory.

If NORMA is not functioning after the first dev build (the first clue
will be in the file new dialog, which will simply say 'ORMModel'
instead of 'Object-Role Modeling File') then you will need to reset
the VSIX installation following the directions from the readme in the
[VSIXInstall/VS2015](VSIXInstall/VS2015) directory. This should be
required only after the first build, or in situations indicated in the
readme.

### Notes on building and debugging with VS2017 and VS2019:

The build and installation process for VS2015 (and earlier) and VS2017 (and later) is radically different because of the side-by-side installation support introduced with VS2017. When NORMA is installed through the Visual Studio Marketplace installer it will be installed for all users, which means it will appear in all Visual Studio instances. However, as a developer you will be developing NORMA in a Visual Studio instance, so you don't can't have NORMA running in that instance while you develop it. If you already have NORMA installed as an extension please uninstall it.

1. In your Visual Studio installation, make sure you have the _Visual Studio extension development_
workload selected. You will also need to add the option _Modeling SDK_ selection.
2. The VSSDK options adds an second Visual Studio installation, kwown as the _Experimental Instance_.
You need to run this once before trying to build NORMA. There are two ways to do this. If you search
for _experimental_ in your start menu, you'll see an option to _Start Experimental Instance of Visual Studio 201x_.
Alternately, open the _Developer Command Prompt for Visual Studio 201x_ command prompt and run _devenv /rootsuffix Exp_.
Once the Experimental instance is open shut it down again.
3. Make sure the PLiX extension is installed and available for the NORMA build. To get PLiX.vsix, you need to download
the NORMA installer _but not actually install it_.
  * Open Visual Studio and go to the extension dialog (Tools/Updates and Extension in VS2017, Extensions/Manage Extension in VS2019)
  * Click _Online_ and search for NORMA
  * Select the item to install.
  * Close Visual Studio and let the installer launch and download the .vsix file. _DO NOT CLICK MODIFY._
  * The extension will be the latest item in your %temp% directory. (From a command prompt, cd %temp% then dir /od will list it last. The file name will be random, but it will have a .vsix extension). Copy this file, but rename it to have a .zip extension.
  * Close the installer.
  * Open the Windows Explorer and find the renamed .vsix. Open the context menu to view the file properties, then locate the Security warning at the bottom of the dialog and click the Unblock button (if you can't find that you're fine).
  * Open the renamed .vsix file and extract the PLiX.vsix file. You're doing two things with this file.
  * First, install the PLiX.vsix. Just double click on it an let it install. This will add it to all of the Visual Studio instances.
  * Second, copy the file into your NORMA git repository in the VSIXInstall/VSIXOnly directory.
4. Open the _Developer Command Prompt for Visual Studio 201x_ as an administrator and navigate to the NORMA git root. (As in other Visual Studio version, this will be your launch environment when working with NORMA, so you might want to add a shortcut to the start menu.)
5. Unlike earlier versions, the VS installation is no longer in a fixed location discoverable from the registry, so the NORMA build needs you to provide some data. Depending on your version, run either `VS2017.bat` or `VS2019.bat`, which will fail with instructions on how to create a VS20xxInstallation.bat file. Following the instructions, then run `VS20xx.bat` again. In earlier VS versions built NORMA files will be under the `C:\Program Files (x86)\ORM Solutions` directory. With the side-by-side installs, the location will be based on the VS version and (15.0 for VS2017, 16.0 for VS2019) and the VisualStudioInstallSuffix you just set. For example, my install suffix for VS2019 is f5148b42, so the build NORMA files install under the `%localappdata%\Microsoft\VisualStudio\16.0_f5148b42Exp\ORM Solutions\Natural ORM Architect` directory. The exact install directory is based on the git version--you can run `NORMAGitVer.bat` to see it. Note that the schema files will be copied to the Visual Studio installation, which is why you need to be running as an administrator.
6. With your build environment set (Developer command prompt in admin mode, run the matching VS20xx.bat file) you are ready to build.
7. Make sure there are no VS instances running (be patient after shutting them, they can take a while to shut down), then run `FirstTimeBuildVS20xx /t:Rebuild`. Note that this step usually does not have to be rerun. However, if you have problems after updating your Visual Studio installation try rerunning this.
8. You can now run `BuildVS20xx /t:Rebuild` to finish the NORMA build. This will build everything, including creating the .vsix project, which automatically writes to your Exp registry to install the product. If you are building individual components you do not need to rerun this step _unless_ you do a git pull or commit that changes the result of NORMAGitVer. If this happens and you do not rebuild, then the other projects will writer to the new version while the registry is still pointing to the version when you last ran this.
9. `devenv ORMPackage.VS20xx.sln` from your command prompt will now let you build the core package, with similar packages for other extensions. There is no central solution that builds them all together. Note that the ORMModel\ORMModel.csproj.user file is the only .user file checked into the project. If you try to F5 launch another project it will tell you that it can't start a class library. In this case, close the project, copy this .user file to the project directory (renamed to match the corresponding .csproj project file, so DcilModel.csproj would need DcilModel.csproj.user). There is no need to check these .user files in.

Note that the Setup directory is no longer used for VS2017 and above, so you will not see BuildSetup* or BuildAll* batch files for the newer versions.

At this point, the basic protocol is much simpler:
1. Open the dev prompt in admin mode, navigate to the root of the NORMA git repository, and run `VS20xx` (xx matching your VS version).
2. Find the .sln file you want to work on in the command prompt.
3. `devenv WHATEVER.VS20xx.sln` where xx again matches your version. If you get the wrong solution file you'll get a solution upgrade warning message. Just cancel and try with the solution file that matches your Visual Studio version. Just remember that you need to launch the VS version to open these projects from inside an environment where you've run the correspond `VS20xx` batch file--you can't just double click them in your file explorer. 
4. F5 to launch VS and you'll get an instance where you can open or create a .orm file and hit your breakpoints.
5. If you pull or commit, make sure you run `Buildvs20xx /t:Rebuild` to reset the registry with the right version, and rerun `BuildDevToolsVS20xx /t:Rebuild` if you see any build issues.

## DSL-generated Files
Note that if you need to regenerate .dsl files you will need to make some modifications to the Microsoft-installed .tt generators for the DSL library. Please contact mcurland@live.com for instructions on how to do this. You will not need this unless you modify a .dsl file.
The build is broken into several pieces to support multiple Visual Studio targeted versions and parts of the project.

The batch files perform as follows:
1) Build.bat builds all of the NORMA tools and extension projects. The test build helper, libary, and setup pieces are not built.

2) BuildDevTools.bat builds the (relatively static) NUBuild MSBuild extensions and DSLTools directive processors. Called by FirstTimeBuild.bat

3) FirstTimeBuild.bat should be run the first time the tool is used in a development environment. Does not do test or setup.

4) BuildTests.bat builds testing tools and samples. Requires XML Diff Patch and nunit2.2.9 installation as discussed in README.txt in this directory.

5) BuildSetup.bat builds everything included in setup. Requires install of WIX 3 as discussed in the README.txt file.

6) BuildAll.bat builds everything except the development tools

7) FirstTimeBuildAll.bat builds everything including the development tools

To build for an Visual Studio 2008 (Orcas) version:
Before running, set the following environment variable:
set TargetVisualStudioVersion=v9.0

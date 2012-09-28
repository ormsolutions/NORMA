@ECHO OFF
SETLOCAL
::Do not call SetupEnvironment.bat. For 64-bit systems, the environment
::variables differ depending on whether the installation batch files are
::launched directly from the command line or indirectly by the 32-bit
::build process. Therefore, any environment variables designed for registry
::use that include Wow6432Node will be correct here, but incorrect when they
::are used by the batch scripts invoked during build.

MSBuild.exe /nologo "%~dp0Main.proj" %*

GOTO:EOF

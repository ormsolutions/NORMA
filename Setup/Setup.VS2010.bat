@echo off
setlocal

echo Natural Object-Role Modeling Architect for Visual Studio 2010 Installation

::See if a current NORMA version is already installed
REG QUERY "HKCR\Installer\Products\D04466BB000031440123100000000000" 1>NUL 2>&1
IF ERRORLEVEL 1 goto :INSTALLNORMA

::If it is the right version, then we're done. If it is the wrong version, then uninstall it and reinstall
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKCR\Installer\Products\D04466BB000031440123100000000000" /v "PackageCode"`) DO SET InstalledNORMAPackageCode=%%B
IF '%InstalledNORMAPackageCode%'=='D04466BB000031440123100001210613' (
echo Current NORMA Installation Found
pause
goto :EOF
)

::Uninstall NORMA, including verification that the registry key is gone after MSIExec is completed
echo Removing previous NORMA version
start /w msiexec.exe /x {BB66440D-0000-4413-1032-010000000000} /qb
REG QUERY "HKCR\Installer\Products\D04466BB000031440123100000000000" 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (
echo Existing NORMA version failed to uninstall
pause
goto :EOF
)

:INSTALLNORMA

::See if a current PLiX version is already installed
REG QUERY "HKCR\Installer\Products\E1CF1F21000068240123100000000000" 1>NUL 2>&1
IF ERRORLEVEL 1 goto :INSTALLPLIX

::If it is the right version, then we're done. If it is the wrong version, then uninstall it and reinstall
FOR /F "usebackq skip=2 tokens=2*" %%A IN (`REG QUERY "HKCR\Installer\Products\E1CF1F21000068240123100000000000" /v "PackageCode"`) DO SET InstalledPlixPackageCode=%%B
IF '%InstalledPlixPackageCode%'=='E1CF1F21000068240123100001210613' (
echo Current PLiX Installation Found
GOTO :PLIXINSTALLED
)

::Uninstall plix, including verification that the registry key is gone after MSIExec is completed
echo Removing previous PLiX version
start /w msiexec.exe /x {12F1FC1E-0000-4286-1032-010000000000} /q
REG QUERY "HKCR\Installer\Products\E1CF1F21000068240123100000000000" 1>NUL 2>&1
IF NOT ERRORLEVEL 1 (
echo Existing PLiX version failed to uninstall
pause
goto :EOF
)

:INSTALLPLIX
echo Installing current PLiX version
start /w msiexec.exe /i "%~dp0PLiX for Visual Studio.msi" /q
REG QUERY "HKCR\Installer\Products\E1CF1F21000068240123100000000000" 1>NUL 2>&1
IF ERRORLEVEL 1 (
echo New PLiX version failed to install
pause
goto :EOF
)
:PLIXINSTALLED


echo Installing current NORMA version
start msiexec.exe /i "%~dp0Natural ORM Architect for Visual Studio 2010.msi"
goto :EOF

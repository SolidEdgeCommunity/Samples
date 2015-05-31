@echo off

set ADDIN_X86_PATH="%~dp0\..\Debug\DemoAddIn.dll"
set ADDIN_X64_PATH="%~dp0\..\x64\Debug\DemoAddIn.dll"

CLS

echo This batch file must be executed with administrator privileges!
echo. 

:menu
echo [Options]
echo 1 Register (x86 and x64)
echo 2 Unregister (x86 and x6)
echo 3 Quit

:choice
set /P C=Enter selection:
if "%C%"=="1" goto register
if "%C%"=="2" goto unregister
if "%C%"=="3" goto end
goto choice

:register
echo.
IF EXIST %ADDIN_X86_PATH% (
	regsvr32.exe %ADDIN_X86_PATH%
) ELSE (
	echo %ADDIN_X86_PATH% does not exist.
)

IF EXIST %ADDIN_X64_PATH% (
	regsvr32.exe %ADDIN_X64_PATH%
) ELSE (
	echo %ADDIN_X64_PATH% does not exist.
)
goto end

:unregister
echo.
IF EXIST %ADDIN_X86_PATH% (
	regsvr32.exe /u %ADDIN_X86_PATH%
) ELSE (
	echo %ADDIN_X86_PATH% does not exist.
)

IF EXIST %ADDIN_X64_PATH% (
	regsvr32.exe /u %ADDIN_X64_PATH%
) ELSE (
	echo %ADDIN_X64_PATH% does not exist.
)
goto end

:end
pause
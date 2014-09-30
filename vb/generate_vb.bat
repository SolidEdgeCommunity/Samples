@echo off

set CS_PATH="%~dp0."
set VB_PATH=%CS_PATH:cs=vb%
set CONVERTER_PATH="C:\Program Files\Tangible Software Solutions\Instant VB\Instant VB.exe"

ECHO %CONVERTER_PATH%
ECHO %CS_PATH%
ECHO %VB_PATH%

%CONVERTER_PATH% %CS_PATH% %VB_PATH%

pause
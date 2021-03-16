@echo off

echo *** print working dir
cd

set _DEST_DIR=C:\s

echo.
if exist .\bin\Release (
    echo *** copy .\bin\Release\*      %_DEST_DIR%\*
    copy .\bin\Release\OpPDir32.exe    %_DEST_DIR%\OpPDir32.exe
) else (
    color 0C
    echo *** ERROR *** No Directory .\bin\Release !!!
)

echo.
dir                                    %_DEST_DIR%\OpPDir32.exe

pause

@echo off

echo *** print working dir
cd
echo.

set _SOURCE_DIR=.\bin\Release
set _DEST_DIR=C:\s

rem Check
if not exist %_SOURCE_DIR% (
	color 0C
	echo *** ERROR *** No Directory %_SOURCE_DIR% !!!
	goto error
)
if not exist %_DEST_DIR% (
	color 0C
	echo *** ERROR *** No Directory %_DEST_DIR% !!!
	goto error
)

echo.
echo *** copy %_SOURCE_DIR%\*               %_DEST_DIR%\*
copy          %_SOURCE_DIR%\OpPDir32.exe    %_DEST_DIR%\OpPDir32.exe

echo.
dir                                         %_DEST_DIR%\OpPDir32.exe
goto ende

:error
goto ende

:ende
echo.
echo *** fertig ***
pause

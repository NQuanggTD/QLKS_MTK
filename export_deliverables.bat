@echo off
setlocal ENABLEDELAYEDEXPANSION

REM ==============================================
REM Export Deliverables Wrapper (Windows .bat)
REM - Purpose: Copy Database/ and GITHUB_INFO.txt
REM   next to the project folder (or a custom path)
REM   by invoking export_deliverables.ps1.
REM - Location: run this from the repo root (QLKS)
REM ==============================================

title Export Deliverables (Database + GitHub info)
echo ==============================================
echo   HOTEL MANAGEMENT SYSTEM - QLKS
echo   Export Deliverables (Database + GitHub info)
echo ==============================================
echo.

REM Resolve script directory
set "SCRIPT_DIR=%~dp0"
set "PS1=%SCRIPT_DIR%export_deliverables.ps1"

REM Check PowerShell
where powershell >nul 2>&1
if errorlevel 1 (
	echo [ERROR] PowerShell not found. Please run on Windows with PowerShell available.
	pause
	exit /b 1
)

REM Help
if /I "%~1"=="/h"  goto :help
if /I "%~1"=="-h"  goto :help
if /I "%~1"=="/help" goto :help
if /I "%~1"=="--help" goto :help

REM Default args: if user passes nothing, export to parent folder
if "%~1"=="" (
	set "ARGS=-TargetRoot .."
) else (
	REM Pass all arguments through to the PowerShell script
	set "ARGS=%*"
)

echo [INFO] Calling PowerShell script:
echo        %PS1% %ARGS%
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS1%" %ARGS%
set "RC=%ERRORLEVEL%"
echo.
if not "%RC%"=="0" (
	echo [ERROR] Export failed with exit code %RC%.
	pause
	exit /b %RC%
)

echo [DONE] Deliverables exported successfully.
echo You can find them in the target folder you specified (default: parent folder).
echo.
pause
exit /b 0

:help
echo Usage: export_deliverables.bat [options]
echo.
echo Options are forwarded to export_deliverables.ps1. Common examples:
echo   (no args)            Export to parent folder next to QLKS and your .docx
echo   -TargetRoot D:\Handin  Export to custom folder
echo   -Zip                   Create Deliverables_yyyyMMdd_HHmmss.zip in target
echo   -Overwrite            Overwrite existing files in target
echo.
echo Examples:
echo   export_deliverables.bat
echo   export_deliverables.bat -TargetRoot D:\Handin -Zip -Overwrite
echo.
pause
exit /b 0


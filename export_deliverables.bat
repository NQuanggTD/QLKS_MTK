@echo off
REM Export Database and GitHub info to parent folder next to the project
powershell -ExecutionPolicy Bypass -File "%~dp0export_deliverables.ps1"
pause

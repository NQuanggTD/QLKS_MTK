@echo off
REM ===================================
REM Script tự động chạy QLKS
REM Hotel Management System Auto-Run
REM ===================================

echo ========================================
echo   HOTEL MANAGEMENT SYSTEM - QLKS
echo ========================================
echo.

REM Kiểm tra .NET SDK
echo [1/3] Checking .NET SDK...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] .NET SDK not found!
    echo Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)
echo [OK] .NET SDK found
echo.

REM Restore dependencies
echo [2/3] Restoring dependencies...
dotnet restore
if errorlevel 1 (
    echo [ERROR] Failed to restore dependencies
    pause
    exit /b 1
)
echo [OK] Dependencies restored
echo.

REM Build và chạy ứng dụng
echo [3/3] Building and running application...
echo.
echo ========================================
echo   Application starting...
echo   URL: http://localhost:5000
echo ========================================
echo.
echo Default login credentials:
echo   Username: admin
echo   Password: admin123
echo.
echo Opening browser in 5 seconds...
echo Press Ctrl+C to stop the application
echo.

REM Chạy ứng dụng trong background và mở trình duyệt
start /B dotnet run

REM Đợi 5 giây để ứng dụng khởi động
timeout /t 5 /nobreak >nul

REM Mở trình duyệt mặc định
start http://localhost:5000

REM Giữ cửa sổ mở
echo.
echo ========================================
echo   Browser opened successfully!
echo   Application is running...
echo ========================================
echo.
pause

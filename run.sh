#!/bin/bash
# ===================================
# Script tự động chạy QLKS
# Hotel Management System Auto-Run
# ===================================

echo "========================================"
echo "  HOTEL MANAGEMENT SYSTEM - QLKS"
echo "========================================"
echo ""

# Kiểm tra .NET SDK
echo "[1/3] Checking .NET SDK..."
if ! command -v dotnet &> /dev/null; then
    echo "[ERROR] .NET SDK not found!"
    echo "Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/download"
    exit 1
fi
echo "[OK] .NET SDK found"
echo ""

# Restore dependencies
echo "[2/3] Restoring dependencies..."
if ! dotnet restore; then
    echo "[ERROR] Failed to restore dependencies"
    exit 1
fi
echo "[OK] Dependencies restored"
echo ""

# Build và chạy ứng dụng
echo "[3/3] Building and running application..."
echo ""
echo "========================================"
echo "  Application starting..."
echo "  URL: http://localhost:5000"
echo "========================================"
echo ""
echo "Default login credentials:"
echo "  Username: admin"
echo "  Password: admin123"
echo ""
echo "Press Ctrl+C to stop the application"
echo ""

dotnet run

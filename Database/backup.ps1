param(
    [string]$Source = "$(Resolve-Path ./Data)",
    [string]$DestRoot = "$(Resolve-Path ./Database/backups)"
)

# Create destination folder with timestamp
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$dest = Join-Path $DestRoot $timestamp

New-Item -ItemType Directory -Path $dest -Force | Out-Null

Write-Host "[Backup] Copying data from: $Source" -ForegroundColor Cyan
Write-Host "[Backup]           to backup: $dest" -ForegroundColor Cyan

Copy-Item -Path (Join-Path $Source '*') -Destination $dest -Recurse -Force

Write-Host "[Backup] Done. Snapshot created at $dest" -ForegroundColor Green

param(
    [string]$Source,
    [string]$DestRoot
)

# Determine project root (works even if Database is moved)
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$candidateBases = @(
    $ScriptDir,
    (Split-Path $ScriptDir -Parent),
    (Get-Location).Path
) | Select-Object -Unique

$ProjectRoot = $null
foreach ($base in $candidateBases) {
    if (Test-Path (Join-Path $base 'QLKS.csproj')) { $ProjectRoot = $base; break }
    if (Test-Path (Join-Path $base 'QLKS/QLKS.csproj')) { $ProjectRoot = (Join-Path $base 'QLKS'); break }
}
if (-not $ProjectRoot) { $ProjectRoot = (Split-Path $ScriptDir -Parent) }

if (-not $Source)   { $Source   = (Resolve-Path (Join-Path $ProjectRoot 'Data')) }
if (-not $DestRoot) { $DestRoot = (Resolve-Path (Join-Path $ScriptDir 'backups')) }

# Create destination folder with timestamp
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$dest = Join-Path $DestRoot $timestamp

New-Item -ItemType Directory -Path $dest -Force | Out-Null

Write-Host "[Backup] Copying data from: $Source" -ForegroundColor Cyan
Write-Host "[Backup]           to backup: $dest" -ForegroundColor Cyan

Copy-Item -Path (Join-Path $Source '*') -Destination $dest -Recurse -Force

Write-Host "[Backup] Done. Snapshot created at $dest" -ForegroundColor Green

param(
    [Parameter(Mandatory=$false)][string]$Timestamp,
    [switch]$Latest,
    [string]$Target,
    [string]$Backups
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

if (-not $Target)  { $Target  = (Resolve-Path (Join-Path $ProjectRoot 'Data')) }
if (-not $Backups) { $Backups = (Resolve-Path (Join-Path $ScriptDir 'backups')) }

if (-not (Test-Path $Backups)) {
    Write-Error "Backups folder not found: $Backups"; exit 1
}

# Pick source backup
if ($Latest) {
    $folder = Get-ChildItem -Path $Backups | Sort-Object Name -Descending | Select-Object -First 1
} elseif ($Timestamp) {
    $folder = Get-Item -Path (Join-Path $Backups $Timestamp) -ErrorAction Stop
} else {
    Write-Host "Available backups:" -ForegroundColor Cyan
    Get-ChildItem -Path $Backups | ForEach-Object { $_.Name }
    Write-Error "Please specify -Latest or -Timestamp yyyyMMdd_HHmmss"; exit 1
}

$src = $folder.FullName
Write-Host "[Restore] Restoring from: $src" -ForegroundColor Yellow
Write-Host "[Restore]              to: $Target" -ForegroundColor Yellow

Copy-Item -Path (Join-Path $src '*') -Destination $Target -Recurse -Force
Write-Host "[Restore] Done." -ForegroundColor Green

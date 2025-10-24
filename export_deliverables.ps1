# Export Database and GitHub info to parent folder next to the project (e.g., alongside MTK.docx)

$ProjectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$ParentDir   = Split-Path -Parent $ProjectRoot

$destDb = Join-Path $ParentDir 'Database'
$destInfo = Join-Path $ParentDir 'GITHUB_INFO.txt'

Write-Host "[Export] Project root:  $ProjectRoot" -ForegroundColor Cyan
Write-Host "[Export] Parent folder: $ParentDir" -ForegroundColor Cyan

# Ensure destination Database exists
New-Item -ItemType Directory -Path $destDb -Force | Out-Null

# Source Database folder inside repo (if exists)
$srcDb = Join-Path $ProjectRoot 'Database'

if (Test-Path $srcDb) {
    Write-Host "[Export] Copying Database → parent..." -ForegroundColor Yellow
    Copy-Item -Path (Join-Path $srcDb '*') -Destination $destDb -Recurse -Force
} else {
    # If Database folder doesn't exist in repo, create structure from Data
    Write-Host "[Export] No repo Database folder found. Creating from Data/..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path (Join-Path $destDb 'data') -Force | Out-Null
    Copy-Item -Path (Join-Path $ProjectRoot 'Data/*') -Destination (Join-Path $destDb 'data') -Recurse -Force
}

# Copy GitHub info file if exists
$srcInfo = Join-Path $ProjectRoot 'GITHUB_INFO.txt'
if (Test-Path $srcInfo) {
    Copy-Item $srcInfo $destInfo -Force
    Write-Host "[Export] Copied GITHUB_INFO.txt → $destInfo" -ForegroundColor Green
} else {
    Write-Host "[Export] GITHUB_INFO.txt not found in project root." -ForegroundColor DarkYellow
}

Write-Host "[Export] Done. Check: $destDb and $destInfo" -ForegroundColor Green

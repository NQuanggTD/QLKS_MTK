<#!
.SYNOPSIS
  Export deliverables (Database + GitHub info + optional runner files) to a target folder

.DESCRIPTION
  This script prepares your handoff package so that "Database" and GitHub info sit
  next to the source folder (QLKS) and your Word document (e.g., MTK.docx).

.PARAMETER TargetRoot
  Destination directory. Defaults to parent folder (..), which is typically the place
  that contains both the QLKS folder and your .docx file.

.PARAMETER Zip
  If specified, also creates a Deliverables_yyyyMMdd_HHmmss.zip in TargetRoot.

.PARAMETER Overwrite
  Overwrite existing files/folders at destination.

.EXAMPLE
  # Copy Database + GITHUB_INFO.txt to parent alongside MTK.docx
  powershell -ExecutionPolicy Bypass -File .\export_deliverables.ps1

.EXAMPLE
  # Export to a custom folder and also create a zip
  powershell -ExecutionPolicy Bypass -File .\export_deliverables.ps1 -TargetRoot "D:\Handin" -Zip -Overwrite
#>

param(
	[string]$TargetRoot = (Resolve-Path ..),
	[switch]$Zip,
	[switch]$Overwrite
)

function Write-Info($msg)  { Write-Host $msg -ForegroundColor Cyan }
function Write-Ok($msg)    { Write-Host $msg -ForegroundColor Green }
function Write-Warn($msg)  { Write-Warning $msg }
function Write-Err($msg)   { Write-Error $msg }

# Resolve important paths
$ProjectRoot   = (Resolve-Path .)
$SrcDatabase   = Join-Path $ProjectRoot "Database"
$SrcData       = Join-Path $ProjectRoot "Data"
$SrcGitInfo    = Join-Path $ProjectRoot "GITHUB_INFO.txt"
$SrcReadme     = Join-Path $ProjectRoot "README.md"
$SrcRunBat     = Join-Path $ProjectRoot "run.bat"
$SrcRunSh      = Join-Path $ProjectRoot "run.sh"

# Destination paths
$DestDatabase  = Join-Path $TargetRoot "Database"
$DestGitInfo   = Join-Path $TargetRoot "GITHUB_INFO.txt"

Write-Info "[Export] Project root: $ProjectRoot"
Write-Info "[Export] Target root : $TargetRoot"

if (-not (Test-Path $TargetRoot)) {
	Write-Info "[Export] Creating target directory..."
	New-Item -ItemType Directory -Path $TargetRoot -Force | Out-Null
}

# Helper: safe copy directory
function Copy-Dir($source, $destination, [switch]$force) {
	if (-not (Test-Path $source)) { return $false }
	if (Test-Path $destination) {
		if ($force) {
			Remove-Item $destination -Recurse -Force -ErrorAction SilentlyContinue
		} else {
			Write-Warn "[Export] Destination exists: $destination (use -Overwrite to replace)"
			return $true
		}
	}
	New-Item -ItemType Directory -Path $destination -Force | Out-Null
	Copy-Item -Path (Join-Path $source '*') -Destination $destination -Recurse -Force
	return $true
}

# 1) Export Database
if (Test-Path $SrcDatabase) {
	Write-Info "[Export] Copying Database folder..."
	$ok = Copy-Dir -source $SrcDatabase -destination $DestDatabase -force:$Overwrite
	if ($ok) { Write-Ok "[Export] Database -> $DestDatabase" }
} elseif (Test-Path $SrcData) {
	Write-Info "[Export] Database folder not found. Building from Data/* as fallback..."
	$DestData = Join-Path $DestDatabase "data"
	if (Test-Path $DestDatabase -and $Overwrite) {
		Remove-Item $DestDatabase -Recurse -Force -ErrorAction SilentlyContinue
	}
	New-Item -ItemType Directory -Path $DestData -Force | Out-Null
	Copy-Item -Path (Join-Path $SrcData '*.json') -Destination $DestData -Recurse -Force -ErrorAction SilentlyContinue
	# Write a minimal README for the Database folder if it doesn't exist
	$dbReadme = @(
		"# Database",
		"This folder is an export snapshot for handoff.",
		"The app reads/writes JSON from 'Data/' at runtime; this copy is for submission/backup."
	) -join [Environment]::NewLine
	Set-Content -Path (Join-Path $DestDatabase 'README.md') -Value $dbReadme -Encoding UTF8
	Write-Ok "[Export] Fallback export done -> $DestDatabase"
} else {
	Write-Warn "[Export] Neither 'Database' nor 'Data' folder exists. Skipping Database export."
}

# 2) Export GitHub info
if (Test-Path $SrcGitInfo) {
	if ((Test-Path $DestGitInfo) -and -not $Overwrite) {
		Write-Warn "[Export] $DestGitInfo exists (use -Overwrite to replace)"
	} else {
		Copy-Item $SrcGitInfo -Destination $DestGitInfo -Force
		Write-Ok "[Export] GITHUB_INFO.txt -> $DestGitInfo"
	}
} else {
	Write-Warn "[Export] GITHUB_INFO.txt not found at project root; skipping"
}

# 3) Optionally export handy runner/readme files next to the docx
foreach ($file in @($SrcReadme, $SrcRunBat, $SrcRunSh)) {
	if (-not (Test-Path $file)) { continue }
	$dest = Join-Path $TargetRoot ([System.IO.Path]::GetFileName($file))
	if ((Test-Path $dest) -and -not $Overwrite) {
		Write-Warn "[Export] $(Split-Path $dest -Leaf) exists (use -Overwrite to replace)"
		continue
	}
	Copy-Item $file -Destination $dest -Force
	Write-Ok "[Export] Copied $(Split-Path $dest -Leaf)"
}

# 4) Optional ZIP
if ($Zip) {
	try {
		Add-Type -AssemblyName System.IO.Compression.FileSystem -ErrorAction Stop
		$stamp    = Get-Date -Format "yyyyMMdd_HHmmss"
		$stageDir = Join-Path $TargetRoot ("Deliverables_" + $stamp)
		$zipPath  = $stageDir + ".zip"

		# Stage content to a temp folder to keep zip clean
		New-Item -ItemType Directory -Path $stageDir -Force | Out-Null
		if (Test-Path $DestDatabase) { Copy-Item $DestDatabase -Destination (Join-Path $stageDir 'Database') -Recurse }
		if (Test-Path $DestGitInfo)  { Copy-Item $DestGitInfo -Destination (Join-Path $stageDir 'GITHUB_INFO.txt') }
		foreach ($leaf in @('README.md','run.bat','run.sh')) {
			$p = Join-Path $TargetRoot $leaf
			if (Test-Path $p) { Copy-Item $p -Destination (Join-Path $stageDir $leaf) }
		}

		[System.IO.Compression.ZipFile]::CreateFromDirectory($stageDir, $zipPath)
		Remove-Item $stageDir -Recurse -Force -ErrorAction SilentlyContinue
		Write-Ok "[Export] ZIP created: $zipPath"
	} catch {
		Write-Err "[Export] Failed to zip deliverables: $($_.Exception.Message)"
	}
}

Write-Ok "[Export] Completed. Review deliverables in: $TargetRoot"


# Database Folder

This folder stores reference data snapshots and utility scripts. The application itself reads/writes JSON in `Data/`. Use these scripts for backup/restore without breaking runtime paths.

## Structure

- `data/` — reference copy of JSON datasets (manual snapshot)
- `backups/` — automatically created by backup script with timestamp
- `backup.ps1` — backup `Data/` into `Database/backups/<timestamp>`
- `restore.ps1` — restore a backup back into the `Data/` folder

## Usage (PowerShell on Windows)

Backup current data:

```powershell
# From project root
./Database/backup.ps1
```

Restore the latest backup:

```powershell
./Database/restore.ps1 -Latest
```

Restore a specific timestamp:

```powershell
./Database/restore.ps1 -Timestamp 20251024_180000
```

Note: Do not move or rename the runtime `Data/` folder. The app expects it to exist.

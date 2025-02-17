param (
    [string]$Configuration = "Debug",
    [string]$ProjectDir
)

Write-Host "Starting PostBuild script..."

# Remove any extra quotes from the ProjectDir
$ProjectDir = $ProjectDir.Trim('"')

# Get the directory of the current script
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Define the bin directory path dynamically using Join-Path
$binDir = Join-Path -Path $ProjectDir -ChildPath "bin\$Configuration\net8.0-windows"

Write-Host "Bin directory: $binDir"  # Debugging the binDir

# Check if the RevitSyncFramework.dll exists in the bin directory
$binDllPath = Join-Path -Path $binDir -ChildPath "RevitSyncFramework.dll"

if (-not (Test-Path $binDllPath)) {
    Write-Host "ERROR: RevitSyncFramework.dll not found in $binDir"
    exit 1  # Exit the script if DLL is not found
}

# Find Revit versions dynamically in the registry
$revitKeyPath = "HKLM:\SOFTWARE\Autodesk\Revit"
$revitVersionKeys = Get-ChildItem -Path $revitKeyPath | Where-Object { $_.PSChildName -match '^\d{4}$' }

if ($revitVersionKeys.Count -gt 0) {
    foreach ($revitKey in $revitVersionKeys) {
        # Get version from the registry for each installed version
        $revitVersion = $revitKey.PSChildName
        Write-Host "Found Revit Version: $revitVersion"

        # Set the destination path for the Addins folder for the detected version
        $destinationPath = Join-Path -Path $env:APPDATA -ChildPath "Autodesk\Revit\Addins\$revitVersion"
        Write-Host "Copying plugin to: $destinationPath"  # Debugging the destination path
        
        # Ensure the destination folder exists
        if (-not (Test-Path $destinationPath)) {
            New-Item -ItemType Directory -Path $destinationPath | Out-Null
        }

        # Modify the .addin file to set the correct path for the Assembly
        $addinFilePath = Join-Path -Path $ProjectDir "RevitSyncFramework.addin"
        Write-Host "addinfile to: $addinFilePath"  # Debugging the destination path

        $addinContent = Get-Content $addinFilePath -Raw

        # Replace the <Assembly> tag with the dynamically set bin DLL path
        $addinContent = $addinContent -replace '<Assembly>.*</Assembly>', "<Assembly>$binDllPath</Assembly>"

        # Save the modified .addin file
        $modifiedAddinFilePath = Join-Path -Path $scriptDir -ChildPath "RevitSyncFramework.addin"
        Set-Content -Path $modifiedAddinFilePath -Value $addinContent

        # Copy the modified .addin file to the destination directory
        Copy-Item $modifiedAddinFilePath -Destination $destinationPath -Force
        Write-Host "Modified ADDIN file copied successfully."

        # Copy the DLL file from the bin directory to the correct directory
        Copy-Item $binDllPath -Destination $destinationPath -Force
        Write-Host "DLL copied successfully."
    }
}
else {
    Write-Host "Revit is not installed or cannot find the version."
}

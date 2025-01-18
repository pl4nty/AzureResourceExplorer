[CmdletBinding()]
param(
  [Parameter()]
  [string]$Filter
)
# Define source and destination roots
$sourceRoot = "../azure-rest-api-specs/specification"
$destinationRoot = "App_Data/SwaggerSpecs"

Copy-Item $sourceRoot/common-types/resource-management $destinationRoot/common-types -Recurse -Force

# Get all shortname directories
$shortnames = Get-ChildItem -Path $sourceRoot -Directory -Filter $Filter

foreach ($shortname in $shortnames) {
  # Access the resource-manager folder
  $resourceManagerPath = "$($shortname.FullName)/resource-manager"
  if (-not (Test-Path -Path $resourceManagerPath)) {
    if ((Get-ChildItem $shortname -Filter *.tsp -Recurse).Count -gt 0) {
      Write-Verbose "Skipping $($shortname.FullName): TypeSpec (.tsp) files detected, which are not supported"
      continue
    }
    elseif ($shortname.Name -in ("graphrbac", "imds")) {
      $resourceManagerPath = "$($shortname.FullName)/data-plane"
    }
    else {
      Write-Warning "Skipping $($shortname.FullName): No resource-manager directory found"
      continue
    }
  }
    
  # Get all provider directories under resource-manager
  # TODO compute??? -Exclude quickstart-templates
  $providers = Get-ChildItem -Path $resourceManagerPath -Directory -Exclude common

  if ($providers.Count -eq 0) {
    Write-Warning "Skipping $($shortname.FullName): No provider directories detected"
    continue
  }
  elseif ($providers.Count -gt 1) {
    Write-Verbose "$($providers.Count) provider directories found for $($shortname.FullName)"
    $providers = $providers | Get-ChildItem -Directory
  }
    
  foreach ($provider in $providers) {
    $versions = @()
    # Get all version directories for the provider
    if ($stable = Test-Path -Path "$($provider.FullName)/stable") {
      $versions += Get-ChildItem -Path "$($provider.FullName)/stable" -Directory
    }
    if ($preview = Test-Path -Path "$($provider.FullName)/preview") {
      $versions += Get-ChildItem -Path "$($provider.FullName)/preview" -Directory
    }
    if (!$stable -and !$preview) {
      if ($stable = Test-Path -Path "$($provider.Parent)/stable") {
        $versions += Get-ChildItem -Path "$($provider.Parent)/stable" -Directory
      }
      if ($preview = Test-Path -Path "$($provider.Parent)/preview") {
        $versions += Get-ChildItem -Path "$($provider.Parent)/preview" -Directory
      }
      if (!$stable -and !$preview) {
        if (($secondLevelProvider = Get-ChildItem -Path $provider -Directory -Exclude common-types, preview, stable).Count -gt 0) {
          $providers = @($providers; $secondLevelProvider)
          Write-Verbose "$($secondLevelProvider.Count) second level provider directories found for $($provider.FullName)"
          continue
        }
        else {
          Write-Warning "Skipping $($provider.FullName): Neither stable nor preview versions found"
          continue
        }
      }
    }

    # Determine the latest version based on the directory name date
    $latestVersion = $versions | Sort-Object { 
      # Extract the date part from the directory name
      if ($_ -match '(\d{4}-\d{2}-\d{2})') {
        [datetime]::Parse($matches[1])
      }
      else {
        [datetime]::MinValue
      }
    } -Descending | Select-Object -First 1

    if ($latestVersion) {
      # Define source and destination file paths
      $sourceFiles = Get-ChildItem -Path $latestVersion.FullName -Filter *.json
      $destinationPath = "$destinationRoot/$($provider.Name)"

      # Create destination directory if it doesn't exist
      if (-not (Test-Path -Path $destinationPath)) {
        New-Item -ItemType Directory -Path $destinationPath | Out-Null
      }

      # Copy all JSON files to the destination
      foreach ($file in $sourceFiles) {
        Copy-Item -Path $file.FullName -Destination "$destinationPath/$($file.Name)" -Force
      }
    }
  }
}

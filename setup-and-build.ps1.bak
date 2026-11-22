#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Auto-setup git, create GitHub repo, push, and trigger APK build via GitHub Actions
#>

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Little Kitten - Auto Setup & Build to GitHub" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Check for git
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Error "Git not found. Install from https://git-scm.com/"
    exit 1
}

# Check for gh
$hasGh = $null -ne (Get-Command gh -ErrorAction SilentlyContinue)
if (-not $hasGh) {
    Write-Warning "GitHub CLI (gh) not found."
    Write-Warning "Install from https://cli.github.com/ for auto-repo creation."
    Write-Warning "Or create repo manually at github.com"
    Write-Host ""
}

Set-Location $PSScriptRoot

Write-Host "[1/6] Initializing git repository..." -ForegroundColor Yellow
if (-not (Test-Path .git)) {
    git init
    git lfs install
    @"
*.psd
*.png
*.jpg
*.mp3
*.wav
*.fbx
*.prefab
*.unity
*.mat
*.anim
*.controller
"@ | Set-Content .gitattributes -Encoding utf8
    git add .gitattributes
}

Write-Host "[2/6] Adding all files..." -ForegroundColor Yellow
git add -A

Write-Host "[3/6] Committing..." -ForegroundColor Yellow
git commit -m "Initial commit: Unity Android port skeleton" 2>$null | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Host "Nothing to commit" }

Write-Host "[4/6] Creating GitHub repository..." -ForegroundColor Yellow
if ($hasGh -and (Test-Path "$env:USERPROFILE\.gh\hosts.yml")) {
    $result = gh repo create LittleKittenUnity --public --source=. --push --description "Little Kitten Virtual Pet - Unity Android Port" 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "[SUCCESS] Repository created and pushed!" -ForegroundColor Green
        Write-Host ""
        Write-Host "[5/6] Triggering GitHub Actions build..." -ForegroundColor Yellow
        gh workflow run unity-android.yml
        Write-Host ""
        Write-Host "[6/6] DONE! Check build status at:" -ForegroundColor Green
        gh repo view --web
        Write-Host ""
        Write-Host "APK will be available in Actions artifacts when build completes (~10-15 min)." -ForegroundColor Cyan
    } else {
        Write-Error "Failed to create repo. Try manually:"
        Write-Host "  gh auth login"
        Write-Host "  gh repo create LittleKittenUnity --public --source=. --push"
    }
} else {
    Write-Host ""
    Write-Host "[INFO] GitHub CLI not authenticated. Run this once:" -ForegroundColor Yellow
    Write-Host "  gh auth login"
    Write-Host ""
    Write-Host "Then re-run this script, OR manually:" -ForegroundColor Yellow
    Write-Host "  1. Create repo at https://github.com/new (name: LittleKittenUnity)"
    Write-Host "  2. git remote add origin https://github.com/YOUR_USER/LittleKittenUnity.git"
    Write-Host "  3. git branch -M main"
    Write-Host "  4. git push -u origin main"
    Write-Host "  5. Go to repo Actions tab -> Enable workflows"
    Write-Host "  6. Add UNITY_LICENSE secret (get from https://id.unity.com)"
}

Write-Host ""
Read-Host "Press Enter to exit"
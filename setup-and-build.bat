@echo off
chcp 65001 >nul
echo ==========================================
echo Little Kitten - Auto Setup & Build to GitHub
echo ==========================================
echo.

REM Check for git
where git >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] Git not found. Install from https://git-scm.com/
    pause
    exit /b 1
)

REM Check for gh (GitHub CLI)
where gh >nul 2>&1
if %errorlevel% neq 0 (
    echo [WARN] GitHub CLI (gh) not found.
    echo         Install from https://cli.github.com/ for auto-repo creation.
    echo         Or create repo manually at github.com and run:
    echo         git remote add origin https://github.com/YOUR_USER/LittleKittenUnity.git
    echo.
)

cd /d "%~dp0"

echo [1/6] Initializing git repository...
if not exist .git (
    git init
    git lfs install
    echo "*.psd" > .gitattributes
    echo "*.png" >> .gitattributes
    echo "*.jpg" >> .gitattributes
    echo "*.mp3" >> .gitattributes
    echo "*.wav" >> .gitattributes
    echo "*.fbx" >> .gitattributes
    echo "*.prefab" >> .gitattributes
    echo "*.unity" >> .gitattributes
    echo "*.mat" >> .gitattributes
    echo "*.anim" >> .gitattributes
    echo "*.controller" >> .gitattributes
    git add .gitattributes
)

echo [2/6] Adding all files...
git add -A

echo [3/6] Committing...
git commit -m "Initial commit: Unity Android port skeleton" >nul 2>&1 || echo "Nothing to commit"

echo [4/6] Creating GitHub repository...
if exist "%USERPROFILE%\.gh\hosts.yml" (
    gh repo create LittleKittenUnity --public --source=. --push --description "Little Kitten Virtual Pet - Unity Android Port"
    if %errorlevel% equ 0 (
        echo.
        echo [SUCCESS] Repository created and pushed!
        echo.
        echo [5/6] Triggering GitHub Actions build...
        gh workflow run unity-android.yml
        echo.
        echo [6/6] DONE! Check build status at:
        gh repo view --web
        echo.
        echo APK will be available in Actions artifacts when build completes (~10-15 min).
    ) else (
        echo [ERROR] Failed to create repo. Try manually:
        echo   gh auth login
        echo   gh repo create LittleKittenUnity --public --source=. --push
    )
) else (
    echo.
    echo [INFO] GitHub CLI not authenticated. Run this once:"
    echo   gh auth login
    echo.
    echo Then re-run this script, OR manually:"
    echo   1. Create repo at https://github.com/new (name: LittleKittenUnity)"
    echo   2. git remote add origin https://github.com/YOUR_USER/LittleKittenUnity.git"
    echo   3. git branch -M main"
    echo   4. git push -u origin main"
    echo   5. Go to repo Actions tab -> Enable workflows"
    echo   6. Add UNITY_LICENSE secret (get from https://id.unity.com)"
)

echo.
pause
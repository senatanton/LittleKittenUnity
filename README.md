# Little Kitten Virtual Pet - Unity Android Port

## Quick Start

1. **Open in Unity 2022.3 LTS+**
 - Open `LittleKittenUnity` folder as project

2. **Install TextMeshPro** (if not auto-imported)
 - Window → TextMeshPro → Import TMP Essentials

3. **Create Scene**
 - Menu: `LittleKitten` → `Setup Main Scene`
 - This creates `Assets/Scenes/Main.unity` with full UI

4. **Add Sprites** (replace placeholders)
 - Put cat sprites in `Assets/Art/Sprites/`
 - Assign to `PetVisual` component in Inspector:
 - Idle, Happy, Sad, Sleep, Eat, Play sprites

5. **Build for Android**
 - File → Build Settings → Android
 - Switch Platform
 - Player Settings: Package Name, Min API 24+
 - Build → `LittleKitten.apk`

## Project Structure

```
Assets/
├── Scripts/
│ ├── Core/
│ │ ├── PetStats.cs # ScriptableObject config (5 stats)
│ │ ├── Pet.cs # Main pet logic (singleton, 5 stats + save/load)
│ │ ├── PetVisual.cs # Sprite/animation handling
│ │ ├── GameManager.cs # UI panels, pause/restart
│ │ └── AudioManager.cs # Music/SFX
│ ├── UI/
│ │ ├── StatBar.cs # 5 stat bars (Hunger/Sleep/Happiness/Cleanliness/Health)
│ │ └── ActionButtons.cs # 5 buttons (Feed/Play/Clean/Sleep/Medicine)
│ └── Editor/
│ ├── SceneSetup.cs # One-click scene generator
│ └── AutoSetupUI.cs # UI reference auto-binding
├── Resources/
│ └── PetStats.asset # Balancing config
└── Scenes/
 └── Main.unity # Generated scene
```

## Pet Stats Balancing (5 Stats)

Edit `Assets/Resources/PetStats.asset` in Inspector:

| Stat | Default | Meaning |
|------|---------|---------|
| maxHunger | 100 | Full hunger value |
| maxSleep | 100 | Full sleep value |
| maxHappiness | 100 | Full happiness value |
| maxCleanliness | 100 | Full cleanliness value |
| maxHealth | 100 | Full health value |
| hungerDecay | 0.02/s | How fast hunger drops |
| sleepDecay | 0.015/s | How fast sleep drops |
| happinessDecay | 0.01/s | How fast happiness drops |
| cleanlinessDecay | 0.008/s | How fast cleanliness drops |
| healthDecay | 0.005/s | How fast health drops |
| feedAmount | 25 | Hunger restored per feed |
| sleepAmount | 40 | Sleep restored per sleep action |
| playAmount | 15 | Happiness per play |
| petAmount | 5 | Happiness per pet |
| cleanAmount | 30 | Cleanliness restored per clean |
| medicineAmount | 35 | Health restored per medicine |
| dirtAmount | 5 | Dirt added per action |
| lowThreshold | 0.3 | Warning threshold |
| criticalThreshold | 0.1 | Critical threshold |

## Actions & Effects

| Action | Primary Effect | Side Effects |
|--------|---------------|--------------|
| Feed | +25 Hunger | +5 Dirt |
| Sleep | +40 Sleep | (only if not sleeping) |
| Play | +15 Happiness | -5 Sleep, +5 Dirt |
| Clean | +30 Cleanliness | -5 Sleep |
| Medicine | +35 Health | -5 Sleep |
| Pet | +5 Happiness | (no decay) |

Death occurs when any stat hits 0.

## Extending

**New Actions:** Add methods to `Pet.cs`, buttons in `ActionButtons.cs`, sprites in `PetVisual.cs`

**Mini-games:** Create new scenes, load via `GameManager`, reward stats on completion

**Persistence:** JSON save/load in `Pet.cs` via `OnApplicationPause`

## Asset Extraction from IPA

The original IPA uses `.car` (Asset Catalog) - requires macOS:
```bash
# On macOS:
assetutil --extract Assets.car ./extracted
# Or use: https://github.com/insidegui/AssetCatalogTinkerer
```

Place extracted PNGs in `Assets/Art/Sprites/` and assign in Inspector.

## License

Original game: Little Kitten Virtual Pet Cat (v4.8.3)
This is a fan remake for learning purposes.
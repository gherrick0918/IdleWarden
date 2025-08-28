# Contributing to IdleWarden

Thanks for helping build IdleWarden! This project targets **Unity 6 (6000.2.x LTS)** with the **2D Mobile** template.

## Prerequisites
- Unity 6 LTS (install Android Build Support, Android SDK/NDK, OpenJDK)
- Git
- (Optional) Visual Studio 2022 or Rider

## Project Structure
- `Assets/` – game code, art, prefabs, scenes
- `Packages/` – Unity package manifest
- `ProjectSettings/` – project configuration (Android, URP, input)
- **Ignored:** `Library/`, `Temp/`, `Builds/`, etc. (auto‑generated)

## Branching
- `main`: stable branch
- feature branches: `feat/<short-topic>`
- fixes: `fix/<short-topic>`

## Commit Style
Use concise, present‑tense messages:
```
feat(clock): add tick loop and offline catch-up
fix(ui): clamp gold text to integer
chore: update .gitignore for Android builds
```

## Scene & Play
- Default scene: `Assets/Scenes/Main.unity`
- Run in portrait; target device: Google Pixel 8 Pro (Android 14+)

## Build (Android)
1. **File → Build Settings → Android → Switch Platform**
2. Player Settings → Resolution & Presentation → Default Orientation **Portrait**
3. Other Settings:
   - Scripting Backend: **IL2CPP**
   - Target Architectures: **ARM64**
   - Minimum API Level: **26 (Android 8.0)** or higher
4. **Build & Run** to a USB‑connected device (Developer options + USB debugging)

## Code Style
- C#: default Unity conventions; `PascalCase` for types/methods, `camelCase` for fields
- Keep MonoBehaviours minimal; push logic to plain C# classes where possible
- Prefer ScriptableObjects for content definitions (items, enemies, zones)

## Testing
- Keep simple play‑mode validations in `Assets/Tests/` (TBD)
- Manual acceptance checks are OK for now:
  - Gold increases during play
  - Offline catch‑up grants capped rewards
  - Gear visuals swap via `LoadoutPresenter`

## PR Checklist
- [ ] Compiles in Unity 6 LTS
- [ ] Runs in Editor (portrait) without console errors
- [ ] Builds to Android device
- [ ] No large binaries committed unintentionally
- [ ] Updated README/docs if behavior changed

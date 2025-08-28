# IdleRPG Scaffold (Unity)

Drop the **Assets** folder into a fresh Unity 2D project (URP optional).

## Scene Setup
1. Create a scene `Scenes/Main.unity` (or open your own).
2. Add an empty `Game` object with `GameClock` and `CombatSimulator`.
3. Add a `CharacterRig` object with four children SpriteRenderers: `Head`, `Body`, `Weapon`, `Offhand`.
   - Add `LoadoutPresenter` on `CharacterRig` and wire the four children.
   - Add `LoadoutDemo` on `CharacterRig` and assign the four ItemDef assets from `Assets/Art/Sprites/Items`.
4. Add a Canvas → TextMeshPro Text for gold display and attach `GoldHud` (assign the `CombatSimulator` and TMP text).
5. Player Settings → Android → Orientation: Portrait.

## Placeholder Items
ScriptableObjects you can create via right-click: **Create → IdleRPG → ItemDef**.
Use the included placeholder sprites:
- `head_hood.png`
- `body_leather.png`
- `wpn_sword.png`
- `off_shield.png`

Assign them to the appropriate slots in your ItemDefs.

## Project Quickstart (Unity 6 LTS)

**Unity Version:** 6000.2.x LTS · Template: **2D Mobile**

### Install
- Unity Hub → Add **Android Build Support** (+ SDK/NDK, OpenJDK)
- Clone repo, open the project folder in Unity

### Run (Editor)
- Open `Assets/Scenes/Main.unity`
- Press Play — you should see a gold counter increasing in portrait layout

### Build (Android)
- File → Build Settings → **Android** → Switch Platform
- Player Settings → **Portrait**
- Scripting Backend: **IL2CPP**, Architecture: **ARM64**
- Build & Run to device (Pixel 8 Pro recommended)

### Folder Highlights
- `Assets/Scripts/Core` – `GameClock`, `SaveService`
- `Assets/Scripts/Combat` – `CombatSimulator`
- `Assets/Scripts/Items` – `ItemDef`, `LoadoutPresenter`
- `Assets/Scripts/UI` – `GoldHud`
- `Assets/Art/Sprites` – placeholder sprites

### Roadmap (near term)
1. Loadout UI panel (tap to equip)
2. StatBlock + DPS calculation (tie income to combat)
3. Enemy waves & zone scaling
4. Offline simulation parity with live tick


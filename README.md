# Simple-UI (v0.1.0)

**Simple-UI** is a lightweight, uGUI-based UI foundation for **portrait-only mobile games**, designed for iOS and Android phones & tablets.

Its main goals are:
- Deterministic **Safe Area–aware UI**
- Reliable behavior in **Device Simulator & Editor**
- Centralized UI orchestration
- Minimal runtime overhead
- Easy reuse across projects

This version (`v0.1.0`) is a **proof of concept** distributed as a `.unitypackage`.

---

## Requirements

- **Unity**: 2022.3.x (tested on 2022.3.62f1 / f3)
- **UI System**: uGUI
- **Platform**: Mobile (iOS / Android)
- **Orientation**: Portrait only
- **Optional**: Unity Device Simulator (recommended for development)

---

## Installation (Proof of Concept)

1. Download `SimpleUI-v0.1.0.unitypackage`
2. Import it into your Unity project
3. Open or create a scene
4. Run: `Tools → Simple-UI → Install or Ensure Setup`

The installer will:
- Create `UIRoot` and `EventSystem` if missing
- Add and configure `SimpleUIManager`
- Create a default `SimpleUIConfig` asset if none exists
- Build a Safe Area–aware UI hierarchy
- Enable unsafe-area debug overlay (Dev only, if configured)

---

## Core Concepts

### Central Orchestration
`SimpleUIManager` is the **single authority** that:
1. Detects screen & safe-area changes
2. Applies safe-area anchors
3. Forces layout rebuilds (Editor-only)
4. Updates unsafe-area overlays (Dev-only)

This avoids timing/race issues common with multiple `[ExecuteAlways]` components.

---

### Passive Components
These components **do not run Update loops**:

- `SafeAreaFitter`  
  Applies `Screen.safeArea` to a RectTransform via normalized anchors.

- `UnsafeAreaOverlay`  
  Visual debug overlay showing unsafe top/bottom areas.

They are driven exclusively by `SimpleUIManager`.

---

## Expected Hierarchy

```

UIRoot (Canvas)
└── FullScreenRoot
├── SafeAreaRoot (SafeAreaFitter + VerticalLayoutGroup)
│   ├── TopBar    (LayoutElement, preferredHeight)
│   ├── Content   (LayoutElement, flexibleHeight = 1)
│   └── BottomBar (LayoutElement, preferredHeight)
└── UnsafeAreaOverlay
    ├── UnsafeTop    (Image, raycast disabled)
    └── UnsafeBottom (Image, raycast disabled)

```

---

## Configuration

### SimpleUIConfig
A `ScriptableObject` controlling:
- Dev vs Prod stage
- CanvasScaler reference resolution
- Match Width / Height value
- TopBar & BottomBar preferred heights
- Unsafe-area overlay visibility per stage

### Apply Changes
Select the config asset and press: `Apply To Scene (UIRoot)`

Changes are applied immediately in Editor and Device Simulator.

---

## Dev vs Prod Behavior

### Dev
- Live safe-area updates
- Device Simulator friendly
- Optional unsafe-area overlay visible

### Prod
- Unsafe-area overlay disabled
- Safe-area fitting remains active (cheap early-out)
- Deterministic and robust for rare runtime changes

---

## Design Notes

- Only `SimpleUIManager` may use `[ExecuteAlways]`
- Layout is rebuilt **once per detected change**, Editor-only
- Safe Area math uses **normalized anchors**, not pixel offsets
- Overlay is visual-only and never blocks input

---

## Known Limitations (v0.1.0)

- Distributed as `.unitypackage` (UPM planned)
- Portrait orientation only
- No animated transitions (by design)
- No automatic prefab-based install yet

---

## Versioning

This project follows **Semantic Versioning**.

- `0.x` → Experimental / evolving API
- `1.0.0` → Stable API guarantee (planned)

---

## Changelog

See [CHANGELOG.md](./CHANGELOG.md)

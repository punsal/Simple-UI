# Simple-UI

Simple-UI is a small, uGUI-based UI foundation for **portrait mobile games** (iOS & Android).

It focuses on:
- Correct **Safe Area** handling
- Deterministic behavior in **Editor & Device Simulator**
- Centralized UI setup with minimal runtime cost

This is an **early proof-of-concept** release.

---

## Requirements
- Unity 2022.3.x
- uGUI
- Portrait orientation
- iOS / Android

---

## Installation
1. Import `SimpleUI-v0.1.0.unitypackage`
2. Open or create a scene
3. Run: `Tools → Simple-UI → Install or Ensure Setup`

This creates:
- `UIRoot` + `EventSystem`
- `SimpleUIManager`
- A Safe Area–aware UI hierarchy
- Default config asset (if missing)

---

## Usage
- Edit `SimpleUIConfig` to adjust layout and scaling
- Use **Dev** stage for live updates and unsafe-area debug overlay
- Use **Prod** stage for release builds

Apply changes via: `Apply To Scene (UIRoot)`

---

## Notes
- Only `SimpleUIManager` runs update logic
- Safe Area math uses normalized anchors
- Overlay is debug-only and never blocks input

---

## Version
**v0.1.0** (proof of concept)

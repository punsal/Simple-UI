# Simple-UI

**Simple-UI** is a lightweight, centralized uGUI setup for **mobile games (iOS & Android)** that prioritizes:
- Safe Area–first interaction
- Portrait-only layouts
- Modern phone aspect ratios (20:9)
- Fast iteration using Unity’s Device Simulator

This repository contains a **Unity project** used to develop and validate the system.  
All Simple-UI code lives under `Assets/SimpleUI/`.

---

## Goals

- Ensure **all interactive UI remains inside the safe area**
- Support phones and tablets with notches, cutouts, and home indicators
- Keep UI architecture **predictable, centralized, and reusable**
- Avoid editor-heavy workflows; rely on layout groups and runtime fitting

---

## Folder Structure

```
Assets/
  SimpleUI/
    Runtime/
      SafeArea/
        SafeAreaFitter.cs
        SafeAreaInsets.cs
        SafeAreaDebugOverlay.cs
      Layout/
        DeviceLayout.cs
      SimpleUI.Runtime.asmdef
    Editor/
      CreateUIRootMenu.cs
      SimpleUI.Editor.asmdef
    Prefabs/
      UIRoot.prefab
    ~Samples/
      DemoScene.unity
    README.md
```

---

## Canvas & Scaling Setup

Simple-UI assumes **portrait orientation only**.

**CanvasScaler settings**
- UI Scale Mode: `Scale With Screen Size`
- Reference Resolution: **1080 × 2400** (20:9)
- Screen Match Mode: `Match Width Or Height`
- Match: **0.5** (neutral baseline)

This reference works well for:
- Modern phones (19.5:9 – 20:9)
- Tablets (scaled via CanvasScaler)

---

## Core UI Hierarchy

Every scene should use **one** `UIRoot` prefab.

```

UIRoot (Canvas)
└── FullScreenRoot        ← stretches to full screen (0..1 anchors)
├── SafeAreaRoot      ← dynamically resized to Screen.safeArea
│   ├── TopBar
│   ├── Content
│   └── BottomBar
└── (Optional) Visual-only elements

```

### Rules
- ✅ **All interactive UI MUST be under `SafeAreaRoot`**
- ❌ Do NOT place buttons or inputs outside the safe area
- Backgrounds / decorative visuals may live outside the safe area

---

## Safe Area Handling

### SafeAreaFitter
`SafeAreaFitter` dynamically resizes `SafeAreaRoot` to match `Screen.safeArea`.

- Works at runtime and in **Device Simulator**
- Automatically updates when:
  - Screen size changes
  - Device profile changes
  - Notch / cutout configuration changes

Attach it to **SafeAreaRoot**.

---

## Debugging Unsafe Areas (Editor)

To visually inspect unsafe areas:

1. Add `SafeAreaDebugOverlay` under `UIRoot`
2. Assign:
   - `RootCanvas`
   - `FullScreenRoot`
3. Enable **Device Simulator**
4. Toggle `show` in Inspector

This renders four translucent panels:
- Top / Bottom / Left / Right unsafe regions

Use this to verify:
- Buttons never overlap notches
- Bottom UI avoids home indicator areas

---

## Layout Guidelines

- Use **VerticalLayoutGroup** for stacked screens
- Use **HorizontalLayoutGroup** for top/bottom bars
- Use **LayoutElement** for min / preferred sizing
- Avoid deep nesting of layout groups (performance)

Simple-UI favors **structure over pixel positioning**.

---

## Device Helpers (Optional)

`DeviceLayout.IsTabletLike` can be used to:
- Increase side paddings
- Widen panels
- Adjust spacing for tablet-like portrait ratios

This is a helper only; Simple-UI itself is device-agnostic.

---

## Editor Utility

Use:
```

GameObject → Simple-UI → Create UIRoot (Safe Area)

```

This creates:
- Canvas with correct scaling
- FullScreenRoot
- SafeAreaRoot with SafeAreaFitter
- TopBar / Content / BottomBar placeholders

---

## Recommended Workflow

1. Create `UIRoot` once
2. Build all screens under `SafeAreaRoot`
3. Test using Device Simulator profiles:
   - iPhone with notch
   - Android with cutout
   - Tablet portrait
4. Never hardcode notch values — rely on `Screen.safeArea`

---

## Non-Goals

- World-space UI
- Landscape layouts
- UI Toolkit
- Theme / color token system (intentionally out of scope)

---

## Summary

Simple-UI gives you:
- One safe way to build mobile UI
- One hierarchy
- One scaling strategy
- Zero guesswork around notches

Build once. Reuse everywhere.
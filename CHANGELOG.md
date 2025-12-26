# Changelog

All notable changes to this project will be documented in this file.

This project follows **Semantic Versioning**:
- `MAJOR` version for incompatible API changes
- `MINOR` version for new functionality in a backward-compatible manner
- `PATCH` version for backward-compatible bug fixes

---

## [0.1.0] - 2025-12-26

### Added
- `SimpleUIManager` as the single orchestrator for Safe Area and layout updates.
- Deterministic update order:
    1. Safe Area anchor application
    2. Layout rebuild (Editor-only)
    3. Unsafe Area overlay update
- `SimpleUIConfig` ScriptableObject for centralized configuration:
    - Dev / Prod stage control
    - CanvasScaler reference resolution and match settings
    - TopBar and BottomBar preferred heights
    - Unsafe-area overlay visibility per stage
- `SafeAreaFitter` passive component for applying `Screen.safeArea` via normalized anchors.
- `UnsafeAreaOverlay` passive debug component for visualizing unsafe top/bottom regions.
- Editor tools:
    - `Tools → Simple-UI → Install or Ensure Setup`
    - `GameObject → Simple-UI → Create UIRoot (Safe Area)`
    - Config inspector button: `Apply To Scene (UIRoot)`
- Optional `[ExecuteAlways]` support on `SimpleUIManager` for instant Editor / Device Simulator feedback.

### Fixed
- Non-deterministic layout behavior caused by multiple `[ExecuteAlways]` update loops.
- Safe Area / overlay desynchronization in Device Simulator.
- Visual artifacts (“orange gap”) caused by update-order and layout timing issues.

### Notes
- This release is a **proof of concept** distributed as a `.unitypackage`.
- UPM packaging and prefab-based installer planned for future versions.
- Designed for portrait-only mobile games (phones and tablets).

---

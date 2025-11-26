# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Asteroid Foundry is a Unity 6 game project targeting WebGL deployment. The project uses the Universal Render Pipeline (URP) configured for 2D rendering and the new Input System.

## Unity Configuration

- **Unity Version**: 6000.1.3f1 (Unity 6)
- **Target Platform**: WebGL (primary)
- **Render Pipeline**: Universal Render Pipeline (URP) 2D
- **Input System**: New Input System (com.unity.inputsystem)
- **C# Language Version**: C# 9.0
- **Target Framework**: .NET Standard 2.1

## Project Structure

```
Assets/
├── Scenes/          - Unity scene files (SampleScene.unity)
├── Scripts/         - C# game scripts (currently empty, awaiting implementation)
├── Settings/        - Project configuration assets
│   ├── InputSystem_Actions.inputactions - Input mappings
│   ├── UniversalRP.asset               - URP renderer settings
│   └── Renderer2D.asset                - 2D renderer configuration
└── WebGLTemplates/
    └── PWACustom/   - Progressive Web App template for WebGL builds
```

## Build and Development Commands

### Building the Project
The project uses GitHub Actions for automated builds. Local builds should be done through Unity Editor:
- **Build WebGL**: File → Build Settings → WebGL → Build
- **Build and Run**: File → Build Settings → WebGL → Build And Run

### CI/CD Pipeline
The project uses GitHub Actions (`.github/workflows/main.yml`) to:
1. Build WebGL version on pushes to main branch
2. Deploy to GitHub Pages automatically
3. Cache Library folder for faster builds

### Opening the Project
- Open Unity Hub
- Add project from disk: Select the `Asteroid-Foundry` folder
- Unity will automatically detect version 6000.1.3f1

## Key Packages and Dependencies

**Core Unity Packages:**
- `com.unity.render-pipelines.universal` (17.1.0) - URP rendering
- `com.unity.inputsystem` (1.14.0) - New Input System
- `com.unity.2d.animation` (10.2.0) - 2D animation tools
- `com.unity.2d.sprite` (1.0.0) - 2D sprite support
- `com.unity.2d.spriteshape` (10.1.0) - Spline-based 2D shapes
- `com.unity.ugui` (2.0.0) - UI system
- `com.unity.visualscripting` (1.9.6) - Visual scripting
- `com.unity.splines` (2.8.1) - Spline tools

**Editor Packages:**
- `com.unity.ide.visualstudio` (2.0.23)
- `com.unity.ide.rider` (3.0.36)

## Architecture Notes

### Input System
Input actions are defined in `Assets/Settings/InputSystem_Actions.inputactions`. When modifying input:
1. Open the Input Actions asset in Unity
2. Edit actions through the Input Actions editor
3. Generate C# class if using code-based input handling
4. Reference actions via the Input System's Action Map API

### Rendering Pipeline
The project uses URP 2D renderer:
- Pipeline asset: `Assets/Settings/UniversalRP.asset`
- 2D renderer: `Assets/Settings/Renderer2D.asset`
- Global settings: `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`

When creating materials or shaders, ensure they're compatible with URP 2D.

### WebGL Deployment
The project includes a custom PWA template for WebGL builds:
- Template location: `Assets/WebGLTemplates/PWACustom/`
- Includes Service Worker for offline capability
- Configured as Progressive Web App with manifest

When modifying the web template:
- Edit `index.html` for structure
- Modify `ServiceWorker.js` for caching behavior
- Update `manifest.webmanifest` for PWA metadata

## Assembly Definitions

The project is configured to use assembly definitions for better compile times:
- **AsteroidFoundry.Common** - Common utilities and shared code
  - Currently contains or will contain: Debugger utilities
  - Located in: `Assets/Scripts/Common/`

When adding new scripts, consider which assembly they belong to for optimal compilation.

## Code Style

The repository includes an `.editorconfig` at the root level that:
- Sets C# analyzer diagnostic severity to none
- Disables IDE0079 warnings
- Maintains consistent code style across editors

## Development Notes

### WebGL Platform Considerations
When developing for WebGL:
- Avoid threading APIs (WebGL is single-threaded)
- Test memory usage carefully (browser memory constraints)
- Use web-compatible compression formats
- Consider build size optimization with code stripping settings

### Code Stripping
The project uses Unity's web stripping tool (`com.unity.web.stripping-tool`) for build size optimization. Settings are in `Assets/DefaultSubmoduleStrippingSettings.asset`.

### Git Workflow
- Main branch: `main`
- Current working branch: `draft/relaxed-vibes`
- GitHub Actions automatically build and deploy from main branch

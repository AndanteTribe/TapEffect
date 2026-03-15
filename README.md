# TapEffect
[![unity-meta-check](https://github.com/AndanteTribe/TapEffect/actions/workflows/unity-meta-file-check.yml/badge.svg)](https://github.com/AndanteTribe/TapEffect/actions/workflows/unity-meta-file-check.yml)
[![Releases](https://img.shields.io/github/release/AndanteTribe/TapEffect.svg)](https://github.com/AndanteTribe/TapEffect/releases)
[![GitHub license](https://img.shields.io/github/license/AndanteTribe/TapEffect.svg)](./LICENSE)
[![openupm](https://img.shields.io/npm/v/jp.andantetribe.tapeffect?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/jp.andantetribe.tapeffect/)

English | [日本語](README_JA.md)

## Overview
**TapEffect** is a Unity package that displays a ripple tap interaction effect on Unity UI (uGUI).

When the user taps or clicks on the screen, a ripple animation is rendered using a custom shader. Multiple simultaneous tap effects are supported, with configurable maximum count and lifetime.

## Requirements
- Unity 2021.3 or later
- [UniTask](https://github.com/Cysharp/UniTask) 2.5.10 or later
- [ObjectReference](https://github.com/AndanteTribe/ObjectReference) 1.0.0 or later

## Installation
Open `Window > Package Manager`, select `[+] > Add package from git URL`, and enter the following URL:

```
https://github.com/AndanteTribe/TapEffect.git?path=src/TapEffect.Unity/Packages/jp.andantetribe.tapeffect
```

## Quick Start

1. Add the `TapEffect` component to a UI GameObject that is a child of a Screen Space - Overlay Canvas (e.g., a full-screen panel).
2. Assign a material using the `_material` (`IObjectReference<Material>`) field in the Inspector.
3. Adjust `Max Count` and `Lifetime` as needed.

> **Note:** It is recommended to place the `TapEffect` component on a higher-layer UI element so that ripples render on top of other UI elements.

## API

| Property | Description |
|----------|-------------|
| `MaxCount` | The maximum number of simultaneous tap effects. Changing this at runtime resets the internal graphics buffer counter. |
| `Lifetime` | The duration in seconds of each tap ripple animation. |
| `raycastTarget` | Always `false`; the component never blocks raycasts. |

## License
This library is released under the MIT license.

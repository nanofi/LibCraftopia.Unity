# LibCraftopia.Unity
A unity package to support creating a Craftopia's mod based on LibCraftopia

![GitHub package.json version](https://img.shields.io/github/package-json/v/nanofi/LibCraftopia.Unity)
[![GitHub](https://img.shields.io/github/license/nanofi/LibCraftopia.Unity)](LICENSE.md)

## Requirements

Craftopia is constructed on Unity 2020.1.3f1. Therefore, I suppose to work this package in that version.

## Installation

You can install LibCraftopia.Unity from the Unity Package Manager. Follow the [instruction](https://docs.unity3d.com/Manual/upm-ui-giturl.html) to add this package from git url, where the url is
```
https://github.com/nanofi/LibCraftopia.Unity.git
```

## Usage

After installing this package, you can see the `LibCraftopia` in the menu bar. Click `LibCraftopia/Initiate` first. Then, you can see the `LibCraftopia` asset in `Assets` folder. You can configure this package via the inspector of the `LibCraftopia` asset. List this package's configurations:

## Main Game Path Information

Set the location where Craftopia is installed. This settings can be filled automatically by clicking the `Browse` button and then choosing `Craftopia.exe` file.

## Mod Information

Settings about your mod. 
- Mod's assembly name: The name of your mod's `*.dll` file. MUST BE alphabetic and numeric characters without space.
- GUID: Unique id for your mod. 
- Mod Name: Your mod's name.
- Author: Your name.
- Description: The description of your mod.
- Version: Your mod's version. MUST consist of four numbers separated by dots `.`; such as `1.0.0.0`.

After filling in this information, click `Configure` button. This will generate `Source` folder with some necessary files in it. You must place your mod's scripts in here.

## Build Information

Setting used for building your mod.
- Target: Build target. MUST BE `Standalone Windows 64`.
- Target Group: Build target group. MUST BE `Standalone`.
- Output Path: The directory path to output the built contents. Either relative path from your project root or Absolute path.

## Dependencies

- Harmony (https://github.com/pardeike/Harmony): the binary assembly is included

## Changelog

See [Changelog](CHANGELOG.md)
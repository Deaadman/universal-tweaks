# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/2.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!--
A version's section is its GitHub release description, so write it for
players: one plain sentence each, about what changed for them.

- Add entries under "## [Unreleased]" as you go, grouped under Added,
  Changed, Deprecated, Removed, Fixed or Security.
- To release, rename it to "## [1.5.0] - YYYY-MM-DD" (no v), add a fresh
  "## [Unreleased]" above it, and update the links at the bottom.

See docs/development.md, "Releasing". CI checks this file's format.
-->

## [Unreleased]

### Added

- **Breaking:** The mod logs through [Complex Logger](https://github.com/Arkhorse/Complex-Logger), which it now needs. Turn on its debug levels when reporting a bug.
- A capacity setting for the Rustic Storage Trunk.
- **Skip Intro** starts the game straight into the main menu, skipping the disclaimers, the press any key prompt and the intro movie.
- The container capacities have their own page in Mod Settings, and are hidden while **Infinite Container Weight** is on.
- Settings for the brightness and range of the flashlight's low and high beams ([#28](https://github.com/Deaadman/universal-tweaks/issues/28)).
- A **Vanilla** choice for **Toilet Water Potability**, which leaves it to the game. It's the default.
- **Ignore Map Visibility**, so spray paint markings always reach the map, even at night or in fog ([#14](https://github.com/Deaadman/universal-tweaks/issues/14)).
- **Drop Single Rounds**, so dropping one rifle or revolver round from a box drops a single round instead of a box.
- **Safer Torch Lighting**, so lighting a torch always lets you pick a fire tool first, even if you only have one, and letting go before it lights no longer wastes a match or wears your flint and steel or fire striker.
- **Permanent Stamina Bar** and **Permanent Time of Day Dial** keep the sprint stamina bar and the sun and moon dial on screen at all times, like **Permanent Crosshair**.

### Changed

- The mod works with The Long Dark 2.58.
- The insulated flask settings describe how long the heat lasts in hours instead of minutes.
- **MRE Texture Variation** and **Consistent Old Man's Dressing Weight** apply and undo as soon as you confirm the settings.
- **Remove Headache from Foods** works straight away, on every food that gives a headache.
- The insulated flask settings work straight away, on every insulated flask.
- The travois settings work straight away, including on a travois that's already placed.
- **Snow Shelter Decay Rate** works straight away, on every snow shelter.
- **Reduce Stew Fatigue Loss** works straight away.
- The noisemaker settings work straight away.
- **Respirator Canister Duration** works straight away, including on a canister that's already attached.
- The container capacities apply as soon as you confirm the settings.

### Fixed

- Spray paint decals no longer stay glowing after **Glowing Decals** is turned off ([#49](https://github.com/Deaadman/universal-tweaks/issues/49)).
- The insulated flask settings work on the DLC flask too ([#41](https://github.com/Deaadman/universal-tweaks/issues/41)).
- The flashlight beam colour changes the whole beam, instead of nothing.
- The **Consistent Old Man's Dressing Weight** setting's name is spelled right.
- The high-beam restriction message shows once, instead of over and over, and only with **Extended Functionality** on.
- The 'Cheat' Tweaks settings stop working when 'Cheat' Tweaks is turned off.
- **Respirator Canister Duration** works right after loading or attaching a canister.
- Calorie burn counts the weight you carry again, and is left to the game with **Additional Carry Weight** off.
- You can reset a snare from the inspect menu after a rabbit is caught again.
- Flashlights turn off when their battery runs out, instead of staying lit.

## [1.4.8] - 2026-01-06

### Changed

- **Breaking:** The mod needs [ModComponent](https://github.com/dommrogers/ModComponent/releases) for its translations, instead of [Localization Utilities](https://github.com/dommrogers/LocalizationUtilities/releases).

## [1.4.7] - 2026-01-05

### Changed

- Updated for The Long Dark 2.51.

## [1.4.6] - 2025-11-25

### Deprecated

- Resetting snares from the inspect menu is turned off for now, as newer versions of The Long Dark broke it. It will come back in a future release.

### Fixed

- The spacebar actions show in inspect mode again ([#36](https://github.com/Deaadman/universal-tweaks/issues/36)).

## [1.4.5] - 2025-11-21

### Added

- Lily pancakes no longer give a headache with the headache setting on ([#34](https://github.com/Deaadman/universal-tweaks/issues/34)).

### Changed

- Updated for The Long Dark 2.44 ([#31](https://github.com/Deaadman/universal-tweaks/issues/31)).

### Fixed

- **Extended Functionality** for flashlights scares off aurora wolves again ([#30](https://github.com/Deaadman/universal-tweaks/issues/30)).

## [1.4.4] - 2024-12-17

### Changed

- Updated for The Long Dark 2.36.

### Fixed

- Placing rock caches no longer causes errors. To place them indoors, you also need the PlaceAnywhere mod.

## [1.4.3] - 2024-08-11

### Changed

- Updated for The Long Dark 2.32.

### Removed

- The campfire lives setting, since the game no longer shows them.

### Fixed

- Turning on 'Cheat' Tweaks no longer hides the container settings.
- Some typos in the settings.
- The stew fatigue setting works on both stews.

## [1.4.2] - 2024-06-29

### Added

- A setting to hide the campfire lives at the bottom of the screen.
- [Carry Weight](https://github.com/Xpazeman/tld-carry-weight-mod), by [Xpazeman](https://github.com/Xpazeman), is part of Universal Tweaks, with his permission.
- [Container Tweaks](https://github.com/GruffCassquatch/ContainerTweaks), by [Cass](https://github.com/GruffCassquatch), is part of Universal Tweaks, with her permission.
- [Limitless Container Space](https://github.com/Atlas-Lumi/LimitlessContainerSpace), by [Emma](https://github.com/Atlas-Lumi), is part of Universal Tweaks, with her permission.
- Settings show their units, like KG.

### Changed

- Updated for The Long Dark 2.31.

### Fixed

- The old man's beard dressing weight setting works again.
- An error when changing the flashlight beam colour.

## [1.4.1] - 2024-06-18

### Removed

- Hiding research books from the fuel list when feeding a fire.

## [1.4.0] - 2024-06-17

### Added

- A setting to drag the travois anywhere, like near stairs and other hard-to-reach places.
- A setting to interact with things while carrying the travois, so you can take it indoors.
- A setting for how long respirator canisters last.
- Settings for how long the noisemaker's fuse burns and how far you can throw it.
- Research books only show up as fuel once you've finished reading them.
- Settings for how long every insulated flask keeps its heat.
- A setting for how much fatigue the meat and vegetable stews take away.
- A setting to drop items facing a random direction, instead of all the same way.
- A setting to make the old man's beard dressing weigh the same as lichen.
- [Enable Feat Progress In Custom Mode](https://github.com/RomainDeschampsFR/EnableFeatProgressInCustomModeLegacy), by [Romain](https://github.com/RomainDeschampsFR), is part of Universal Tweaks, with his permission.

### Changed

- Camber Flight porridge no longer gives a headache with the headache setting on.

## [1.3.0] - 2023-12-10

### Added

- Flashlights stay lit when dropped outside an aurora.
- A setting to turn off the flashlight's high beam outside auroras.
- A setting for flashlight batteries that never run out.
- Settings for the colour of the flashlight and miner's flashlight beams.
- Settings for how long the flashlight's low and high beams last, and how long it takes to recharge.
- A setting so flashlights only flicker during auroras.
- Rock caches can be built indoors.
- Settings for the most rock caches in a region and the least distance between them.
- A setting to make spray paint decals glow in the dark.
- Settings for how fast the travois decays.
- A setting for how fast the travois turns.
- A setting for the steepest slope the travois can go up or down.

### Changed

- Updated for The Long Dark 2.25.
- The breath setting is simpler and lighter on performance.
- Clearer names and descriptions for some settings.

### Removed

- The left-handed animations setting.

## [1.2.1] - 2023-11-19

### Added

- Turkish translations, thanks to [Elderly-Emre](https://github.com/Elderly-Emre).
- A workaround setting for players who also use Relentless Night.

### Changed

- The settings are grouped so they're easier to find.

### Fixed

- Every toilet becomes non-potable when the setting says so.
- Flashlights no longer drain twice as fast during an aurora.
- The decal overlap setting works as expected.

## [1.2.0] - 2023-11-11

### Added

- **Breaking:** The mod needs [ModSettings](https://github.com/DigitalzombieTLD/ModSettings), for its many settings.
- A setting for a brown MRE instead of the blue one, with textures by [Jods](https://github.com/Jods-Its) and Kaiusername.
- [Disable Breath Effects](https://github.com/Thekillergreece/DisableBreathEffect), by [Thekillergreece](https://github.com/Thekillergreece) and [zeobviouslyfakeacc](https://github.com/zeobviouslyfakeacc), is part of Universal Tweaks.
- [TLD_NonPotableToiletWater](https://github.com/Ezinw/TLD_NonPotableToiletWater), by [Ezinw](https://github.com/Ezinw), is part of Universal Tweaks.
- A setting to hide the WINTERMUTE and expansion menu items when you don't own them.
- A setting for left-handed animations.
- A setting so flashlights don't flicker outside auroras.
- A setting for flashlights to start with a random battery charge, instead of always full.
- A setting to always show the crosshair.

### Changed

- Spray paint is greyed out in the radial menu when you have no spray paint can.
- The headache setting works differently behind the scenes.

## [1.1.1] - 2023-10-25

### Fixed

- The snare setting no longer hides every other equip prompt.

## [1.1.0] - 2023-10-24

### Added

- **Breaking:** The mod needs [Localization Utilities](https://github.com/dommrogers/LocalizationUtilities).
- You can reset a snare from the inspect menu after a rabbit is caught.
- You can move while aiming the revolver.
- Spray paint decals can be placed as close to each other as you like.
- Peach and rose hip pies no longer give a headache with the headache setting on.

### Changed

- Snow shelters decay half as fast, losing 50 hit points a day instead of 100.

### Fixed

- The fire sprite on the feed fire panel is lined up properly.

## [1.0.0] - 2023-10-14

### Added

- The first release.

[Unreleased]: https://github.com/Deaadman/universal-tweaks/compare/1.4.8...HEAD
[1.4.8]: https://github.com/Deaadman/universal-tweaks/compare/1.4.7...1.4.8
[1.4.7]: https://github.com/Deaadman/universal-tweaks/compare/1.4.6...1.4.7
[1.4.6]: https://github.com/Deaadman/universal-tweaks/compare/1.4.5...1.4.6
[1.4.5]: https://github.com/Deaadman/universal-tweaks/compare/1.4.4...1.4.5
[1.4.4]: https://github.com/Deaadman/universal-tweaks/compare/1.4.3...1.4.4
[1.4.3]: https://github.com/Deaadman/universal-tweaks/compare/1.4.2...1.4.3
[1.4.2]: https://github.com/Deaadman/universal-tweaks/compare/1.4.1...1.4.2
[1.4.1]: https://github.com/Deaadman/universal-tweaks/compare/1.4.0...1.4.1
[1.4.0]: https://github.com/Deaadman/universal-tweaks/compare/1.3.0...1.4.0
[1.3.0]: https://github.com/Deaadman/universal-tweaks/compare/1.2.1...1.3.0
[1.2.1]: https://github.com/Deaadman/universal-tweaks/compare/1.2.0...1.2.1
[1.2.0]: https://github.com/Deaadman/universal-tweaks/compare/1.1.1...1.2.0
[1.1.1]: https://github.com/Deaadman/universal-tweaks/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/Deaadman/universal-tweaks/compare/1.0.0...1.1.0
[1.0.0]: https://github.com/Deaadman/universal-tweaks/releases/tag/1.0.0

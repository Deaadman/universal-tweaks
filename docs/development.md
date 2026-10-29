# Building from source

You need the [.NET SDK](https://dotnet.microsoft.com/download) 10 or newer, which CI uses too. The game's assemblies come from NuGet, so you don't need the game to build.

```powershell
dotnet build -c Release
dotnet test --project tests/UniversalTweaks.Tests
```

The mod is `src/bin/Release/net6.0/UniversalTweaks.dll`.

To copy it into the game after every Debug build, set `TheLongDarkPath` to the game's folder, as an environment variable or in `Directory.Build.props`:

```powershell
$env:TheLongDarkPath = 'C:\Program Files (x86)\Steam\steamapps\common\TheLongDark'
dotnet build
```

## How it's laid out

- `src/Core`: the parts with numbers and rules but nothing from Unity, the game or MelonLoader: the carry weight thresholds, the game's own values that 'Cheat' Tweaks puts back, the flashlight tuning, which items get the mod's icons, and which single round each ammo box drops. It can be [tested](#testing) without the game.
- `src/Patches`: the [Harmony](https://harmony.pardeike.net) patches on the game, one file for each game type, like `FlashlightItemPatches.cs` for `FlashlightItem`. Each patch is a private nested class named for what it does.
- `src/Utilities`: the tweaks with more to them than a patch, which the patches call: `Flashlights`, `Encumbrance`, `ContainerCapacities`, `SprayPaint`, `Travois`, `MreTexture`, `DressingWeight`, `StewFatigue`, `SingleRoundDrops`, `TorchLightMenu`, `TorchToolUse` and `TimeOfDayDial`. Also the helpers they share: `ObjectStateCache` keeps the game's own values per object so they can be put back, `GearPrefabs` loads the game's gear prefabs by name, `OncePerFrame` stops work repeating within a frame, and `TextureSwapper` and `AssetBundleLoader` load the mod's textures.
- `src/Settings.cs` and `src/ContainerSettings.cs`: the mod's two pages in the game's Mod Settings menu, from [ModSettings](https://github.com/DigitalzombieTLD/ModSettings) (its DLL is committed in `src/Dependencies`). Most settings are read where they're used, so they take effect straight away. `OnConfirm` pushes the rest into the objects that already exist.
- `src/Resources`: the asset bundle with the brown MRE's texture and icon, and `Localization.json` with the mod's translations. Both are embedded in the DLL.
- `src/Dependencies`: DLLs that aren't on NuGet, or not in the version the mod needs. [Complex Logger](https://github.com/Arkhorse/Complex-Logger), which the mod logs through with `Mod.Logger`, [ModComponent](https://github.com/dommrogers/ModComponent), for its localization, and [ModSettings](https://github.com/DigitalzombieTLD/ModSettings), for its settings menu.
- `src/Properties/AssemblyInfo.cs`: what MelonLoader reads about the mod, including the MelonLoader version it needs and the mods it won't load with.
- `tests/UniversalTweaks.Tests`: unit tests for `src/Core`.
- `docs`: the pages linked from the README. `assets` has the banner and thumbnail as layered SVGs, each with a layer for each strip colour (Stone, Black and Ice); hide the ones above the colour you want. Their exported WebPs, which the README and mod pages use, are in `assets/images`.

Every type and member has an XML comment. Keep it that way: the build checks them (see [CI](#ci)).

The settings' field names are what the settings files save, so renaming one resets it for every player. Leave them as they are.

## Versions

The version is `VersionPrefix` in [`src/UniversalTweaks.csproj`](../src/UniversalTweaks.csproj). It's the next release's version, so a release changes nothing but the changelog.

Nightly builds add a suffix with the date and commit, like `1.5.0-nightly.20261022+g1a2b3c4`. The game's console shows the version when the mod loads, so a bug report says exactly which build it came from.

## Packages

[`src/packages.lock.json`](../src/packages.lock.json) and the test project's lock file pin every NuGet package, including the ones they depend on. After changing a package version, run `dotnet restore UniversalTweaks.slnx` and commit the lock file with it.

The first two numbers of the `MonsieurMeh.Modding.TLD.Il2CppAssemblies.Windows` version are the game version its assemblies come from. So a new version of it usually means a game update: build against it, then check the mod in the game.
## Branches

- `develop` is where work happens. Every push to it publishes a [nightly build](#nightly-builds).
- `main` has the newest release. Merge `develop` into it to release.

## CI

[CI](../.github/workflows/ci.yml) runs on Windows for every push to `main` and every pull request. It checks:

- that `CHANGELOG.md` follows the format below
- formatting, with `dotnet format whitespace --verify-no-changes`. Run `dotnet format whitespace UniversalTweaks.slnx` to fix it.
- that each `packages.lock.json` matches its project
- that no package has a security advisory: restoring checks them all
- that a Release build works with warnings as errors, and shows the DLL's version and size. That includes the XML comments: public types and members need one, and every `<see cref="..."/>` must point at something that exists.
- that the [unit tests](#testing) pass

## Testing

Most of the mod can only be tested in the game. Everything it touches, down to Unity's `Color` and `Mathf`, calls into the running game through Il2CppInterop, so it can't run in a test.

The exception is `src/Core`, with nothing from Unity, the game or MelonLoader. [`tests/UniversalTweaks.Tests`](../tests/UniversalTweaks.Tests) tests it with [xUnit](https://xunit.net):

```powershell
dotnet test --project tests/UniversalTweaks.Tests
```

The test project compiles the files in `src/Core` itself rather than referencing the mod. So if something in `src/Core` uses a game type, the tests stop building, and it has to move out. When you add a rule with numbers in it, put it in `src/Core` and test it there, and keep the game-facing part in `src/Utilities` or `src/Patches`.

### In the game

Before a release, and after a game update, load a save and check, with each setting on and then off again:

- **Additional Carry Weight** raises the weight you can carry, shown in the inventory, without lowering the calorie burn from what you carry. It and **Infinite Carry Weight** only show and work with 'Cheat' Tweaks on.
- Container capacities change in the next container you open, without reloading, and **Infinite Container Weight** hides them.
- With 'Cheat' Tweaks and **Extended Functionality** on, a flashlight works outside an aurora, stays lit on the ground, drains and turns off when empty. **High-Beam Restrictions** blocks the high beam outside auroras with one message. Beam colour, brightness and range change in your hands and on the ground.
- **Remove Headache from Foods**, the insulated flask and stew settings, **Toilet Water Potability**, the noisemaker, respirator and snow shelter settings work on items that already exist.
- Rock caches can be placed indoors with **Allowed Indoors**.
- Spray paint decals glow after confirming **Glowing Decals** and stop glowing when it's turned off. With **Ignore Map Visibility**, a marking sprayed at night shows on the map.
- The travois settings change a travois that's already placed.
- **MRE Texture Variation** turns MREs and their icon brown after confirming, and blue again when it's off.
- **Drop Single Rounds** drops one rifle or revolver round from a box as a single round, and dropping more still drops a box.
- With **Safer Torch Lighting**, lighting a torch with only matches still asks which tool to use, and letting go before it lights keeps the match.
- **Permanent Crosshair**, **Permanent Stamina Bar** and **Permanent Time of Day Dial** keep them on screen, including straight after loading a save. The dial goes back to the HUD after the radial menu or Rest closes.
- **Skip Intro** starts the next launch straight in the main menu.
- A snare with a rabbit shows **Set** in the inspect menu, and pressing it resets the snare.
- Settings changed in Mod Settings are still there after restarting the game.
- The console shows the mod's version when the game starts, with no errors in `MelonLoader\Latest.log`.

Once a week, [Dependabot](../.github/dependabot.yml) opens a pull request against `develop` for NuGet packages and actions with a newer version, with the minor and patch updates together. Merge it when CI passes.

## Nightly builds

The [nightly workflow](../.github/workflows/nightly.yml) runs on every push to `develop`. It runs CI, builds the mod with the nightly version, and replaces the [`nightly`](https://github.com/Deaadman/universal-tweaks/releases/tag/nightly) pre-release with it. Its notes are the `## [Unreleased]` section of `CHANGELOG.md`, so testers see what's new, then a link to every commit since the latest release. To preview them, run `.\scripts\release-notes.ps1 -Version Unreleased -OutDir notes`.

## Releasing

The [release workflow](../.github/workflows/release.yml) runs when a version tag is pushed:

1. On `develop`, in [`CHANGELOG.md`](../CHANGELOG.md), rename `## [Unreleased]` to the new version and date, for example `## [1.5.0] - 2026-11-02`. Add a new empty `## [Unreleased]` above it, and update the links at the bottom.
2. Check `VersionPrefix` in `src/UniversalTweaks.csproj` is the same version. Commit, and merge `develop` into `main`.
3. Tag the commit on `main` and push the tag: `git tag 1.5.0`, then `git push origin 1.5.0`. Tags don't start with `v`.
4. Set `VersionPrefix` on `develop` to the next version, so nightlies after the release are numbered after it.

The workflow then runs CI, checks the tag matches `VersionPrefix`, builds the mod and publishes the release with `UniversalTweaks.dll`. It stops if `CHANGELOG.md` has no section for that version.

### Writing the changelog

`CHANGELOG.md` follows [Keep a Changelog 2.0.0](https://keepachangelog.com/en/2.0.0/), but without a `v` in front of versions. CI checks the format and says what to fix.

Each version's section is the release's description on GitHub, so write for players:

- Add each change under `## [Unreleased]` as you make it.
- One plain sentence per change, about what's different in the game. "Flashlights turn off when their battery runs out", not "Call EnableLights(Off) in the Update postfix".
- Leave out changes players can't see, like refactors and CI.
- Use full `https://` links. Links like `docs/features.md` break on the release page.

The format rules:

- Use only these headings, in this order, and skip empty ones: `### Added`, `### Changed`, `### Deprecated`, `### Removed`, `### Fixed`, `### Security`.
- Every line under a heading starts with `- `. A long entry can continue on the next line if it's indented.
- A version can start with a short summary before its first heading.
- Start a breaking change with `**Breaking:**`.
- Add ` [YANKED]` after the date of a pulled release. The workflow won't publish it.

The links at the bottom make each heading clickable. `[Unreleased]` compares the newest release with `HEAD`, each version compares with the one before, and the first links to its tag.

To preview a release's notes, run `.\scripts\release-notes.ps1 -Version 1.5.0 -OutDir notes`. It writes the GitHub description to `notes/release-notes.md`. Add `-Check` instead to only check the format, like CI does. The script is shared with [Abalone](https://github.com/Deaadman/abalone): change it there, not here.

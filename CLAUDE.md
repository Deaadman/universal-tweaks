# CLAUDE.md

Universal Tweaks is a MelonLoader mod for The Long Dark that adjusts lots of small things in the game, each behind a setting. It patches the game with Harmony and ships as one `UniversalTweaks.dll`.

## Build and check

```powershell
dotnet build -c Release
dotnet test --project tests/UniversalTweaks.Tests
dotnet format whitespace UniversalTweaks.slnx
.\scripts\release-notes.ps1 -Check
```

- MelonLoader is pinned to **0.7.2** (`MelonLoaderVersion` in the csproj's `GenerateBuildInfo` target), which runs mods on .NET 6. Keep `TargetFramework` at `net6.0` and don't bump MelonLoader incidentally. Any newer SDK (10, 11) is fine to build with.
- Warnings are errors, including missing XML comments and broken `cref`s. Every type and member has an XML comment.
- `docs/development.md` has the full layout, CI, nightly and release process.

## Game code

- `game-refs/` (git-ignored, never commit it or paste large parts of it: it's Hinterland's code) is the game decompiled with Cpp2IL and call analysis. Method bodies are mostly stubs listing the calls they make, not real logic. Use it for type, field and method names, signatures, and what calls what; infer behaviour carefully and say when you're guessing.
- The compiled interop assemblies (`MonsieurMeh.Modding.TLD.Il2CppAssemblies.Windows`, in the NuGet cache) are what the mod builds against. If they and `game-refs` disagree, they win.
- Game types are in the `Il2Cpp` namespace. Unity objects need `== null`, not `?.` or `is null`, to detect destroyed objects.

## Conventions

- Patches live in `src/Patches/<GameType>Patches.cs`, as private nested classes named for what they do. The instance parameter must be `__instance`. Prefer `Postfix` unless the original must be blocked or replaced. A tweak with more to it than a patch goes in `src/Utilities`, and its patches call into it.
- Rules with numbers go in `src/Core`: plain C#, no Unity, game or MelonLoader types, so `tests/UniversalTweaks.Tests` can test it (it compiles those files directly). Game-facing code calls into it.
- Every tweak is a setting in `src/Settings.cs` (container capacities in `src/ContainerSettings.cs`), and defaults to the game's own behaviour. Read `Settings.Instance` where it's used, so changes apply straight away; push values into objects that already exist from `OnConfirm`. Settings that could count as cheating go in `CheatingTweakFields` and fall back to `Core/VanillaDefaults.cs` when 'Cheat' Tweaks is off.
- To put the game's own values back when a setting is turned off, capture them per object in an `ObjectStateCache` before the first change.
- Translated text goes in `src/Resources/Localization.json` and is read with `Localization.Get`.
- Log with `Mod.Logger.Log(message, FlaggedLoggingLevel.X)` (Complex Logger, from `src/Dependencies`). Don't use `MelonLogger` directly.
- Fields are `m_Name` (instance) and `s_Name` (static), constants and static readonly fields `PascalCase`, matching the game. The exception is the settings fields: their names are what the settings files save, so never rename them. Global usings are in the csproj.
- Change gameplay only when asked; refactors keep behaviour identical. Check changes in game where possible: tests can't cover patches.

## Changes and releases

- Work happens on `develop`; every push publishes a nightly. `main` has the latest release, published by pushing a tag like `1.5.0` (no `v`) that matches `VersionPrefix`.
- Add each player-visible change to `## [Unreleased]` in `CHANGELOG.md` as one plain sentence for players. CI checks the format.
- Never add Claude or any AI as a co-author: no `Co-Authored-By` trailer or attribution footer in commits or pull requests.

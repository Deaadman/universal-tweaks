using System.Runtime.CompilerServices;

using BuildInfo = UniversalTweaks.Properties.BuildInfo;

[assembly: MelonInfo(typeof(UniversalTweaks.Mod), BuildInfo.Name, BuildInfo.Version, BuildInfo.Author, BuildInfo.DownloadLink)]
[assembly: MelonGame("Hinterland", "TheLongDark")]
[assembly: MelonPriority(BuildInfo.Priority)]
[assembly: MelonIncompatibleAssemblies("DisableBreathEffect", "NonPotableToiletWater", "UnlimitedRockCaches", "ContainerTweaks")]
[assembly: VerifyLoaderVersion(BuildInfo.MelonLoaderVersion, true)]
[assembly: InternalsVisibleTo("UniversalTweaks.Tests")]
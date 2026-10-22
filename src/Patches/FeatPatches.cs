namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Feat"/>, the feats players unlock by playing.</summary>
internal static class FeatPatches
{
    /// <summary>Lets feats progress in custom games when <see cref="Settings.FeatProgressInCustomMode"/> is on.</summary>
    [HarmonyPatch(typeof(Feat), nameof(Feat.ShouldBlockIncrement))]
    private static class FeatProgressInCustomMode
    {
        private static void Postfix(ref bool __result)
        {
            if (!Settings.Instance.FeatProgressInCustomMode)
                return;

            __result = false;
            Mod.Logger.Log("Allowing feat progress in custom mode.", FlaggedLoggingLevel.Trace);
        }
    }
}

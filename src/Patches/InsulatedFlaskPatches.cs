using Il2CppTLD.Gear;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="InsulatedFlask"/>, every kind of insulated flask.</summary>
internal static class InsulatedFlaskPatches
{
    /// <summary>
    /// Sets how fast a flask loses heat from the settings, before the game works it out. It runs every tick and when
    /// catching up on time passed after loading, so changes apply straight away.
    /// </summary>
    [HarmonyPatch(typeof(InsulatedFlask), nameof(InsulatedFlask.CalculateHeatLoss))]
    private static class InsulatedFlaskHeatLoss
    {
        private static void Prefix(InsulatedFlask __instance)
        {
            __instance.m_PercentHeatLossPerMinuteIndoors = Settings.Instance.InsulatedFlaskHeatLossPerMinuteIndoors;
            __instance.m_PercentHeatLossPerMinuteOutdoors = Settings.Instance.InsulatedFlaskHeatLossPerMinuteOutdoors;
        }
    }
}

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Breath"/>, the player's visible breath in the cold.</summary>
internal static class BreathPatches
{
    /// <summary>Hides the breath effect when <see cref="Settings.BreathVisibility"/> is off.</summary>
    [HarmonyPatch(typeof(Breath), nameof(Breath.PlayBreathEffect))]
    private static class BreathVisibility
    {
        private static void Postfix(Breath __instance) =>
            __instance.m_SuppressEffects = !Settings.Instance.BreathVisibility;
    }
}

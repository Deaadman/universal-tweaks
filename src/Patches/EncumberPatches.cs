using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Encumber"/>, how much the player can carry.</summary>
internal static class EncumberPatches
{
    /// <summary>Adds the extra carry weight from the settings when the game starts: see <see cref="Encumbrance"/>.</summary>
    [HarmonyPatch(typeof(Encumber), nameof(Encumber.Start))]
    private static class StartEncumberTweaks
    {
        private static void Postfix(Encumber __instance) => Encumbrance.Apply(__instance);
    }
}

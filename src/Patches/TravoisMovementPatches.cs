using Il2CppTLD.BigCarry;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="TravoisMovement"/>, where the travois can be dragged.</summary>
internal static class TravoisMovementPatches
{
    /// <summary>
    /// Lets the travois go anywhere, like indoors and between regions, when 'Cheat' Tweaks and
    /// <see cref="Settings.OverrideTravoisMovementRestrictions"/> are on.
    /// </summary>
    [HarmonyPatch(typeof(TravoisMovement), nameof(TravoisMovement.CheckMovementRestriction))]
    private static class OverrideMovementRestrictions
    {
        private static void Postfix(ref CarryDisplayError __result)
        {
            if (Settings.Instance.CheatingTweaks && Settings.Instance.OverrideTravoisMovementRestrictions)
                __result = CarryDisplayError.None;
        }
    }
}

using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="CharcoalItem"/>, whose visibility check decides what gets drawn on the map.</summary>
internal static class CharcoalItemPatches
{
    /// <summary>
    /// Counts the visibility as good enough while placing a spray paint decal, when
    /// <see cref="Settings.IgnoreDecalMapVisibility"/> is on, so the marking always reaches the map.
    /// </summary>
    [HarmonyPatch(typeof(CharcoalItem), nameof(CharcoalItem.HasSurveyVisibility))]
    private static class IgnoreDecalMapVisibility
    {
        private static void Postfix(ref bool __result)
        {
            if (SprayPaint.PlacingDecal && Settings.Instance.IgnoreDecalMapVisibility)
                __result = true;
        }
    }
}

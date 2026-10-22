using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="DynamicDecalsManager"/>, which draws spray paint decals.</summary>
internal static class DynamicDecalsManagerPatches
{
    /// <summary>Makes spray paint decals glow when glowing decals are on: see <see cref="SprayPaint"/>.</summary>
    [HarmonyPatch(typeof(DynamicDecalsManager), nameof(DynamicDecalsManager.RenderDynamicDecal))]
    private static class GlowingDecals
    {
        private static void Prefix(DynamicDecalsManager __instance) => SprayPaint.ApplyGlowingMaterial(__instance);
    }
}

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="RockCacheManager"/>, which decides where rock caches can be built.</summary>
internal static class RockCacheManagerPatches
{
    /// <summary>
    /// Lets rock caches be built anywhere, including indoors, when <see cref="Settings.AllowedIndoorsRockCaches"/> is on,
    /// and applies the per-region limit and minimum distance from the settings.
    /// </summary>
    [HarmonyPatch(typeof(RockCacheManager), nameof(RockCacheManager.CanAttemptToPlaceRockCache))]
    private static class RockCacheRadialMenuIndoors
    {
        private static bool Prefix(ref bool __result)
        {
            if (!Settings.Instance.AllowedIndoorsRockCaches)
                return true;

            __result = true;
            return false;
        }

        private static void Postfix(RockCacheManager __instance)
        {
            __instance.m_MaxRockCachesPerRegion = Settings.Instance.MaximumPerRegionRockCaches;
            __instance.m_MinDistanceBetweenRockCaches = Settings.Instance.MinimumDistanceBetweenRockCaches;
        }
    }
}

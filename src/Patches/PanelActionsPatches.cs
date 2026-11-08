namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_Actions"/>, the actions the player picks from the radial menu.</summary>
internal static class PanelActionsPatches
{
    /// <summary>
    /// Starts placing a rock cache without the game's indoor checks when <see cref="Settings.AllowedIndoorsRockCaches"/>
    /// is on. It still checks the player has the materials and hasn't reached the limit.
    /// </summary>
    [HarmonyPatch(typeof(Panel_Actions), nameof(Panel_Actions.OnPlaceRockCache))]
    private static class RockCachePlacementIndoors
    {
        private static bool Prefix()
        {
            if (!Settings.Instance.AllowedIndoorsRockCaches)
                return true;

            var rockCacheManager = GameManager.GetRockCacheManager();
            if (!rockCacheManager.CanAttemptToPlaceRockCache())
            {
                GameAudioManager.PlayGUIError();
                return false;
            }

            var missingMaterialsString = rockCacheManager.GetMissingMaterialsString();
            if (missingMaterialsString != null)
            {
                HUDMessage.AddMessage(missingMaterialsString, false, false);
                return false;
            }

            GameAudioManager.PlayGUIButtonClick();
            var gameObject = UnityEngine.Object.Instantiate(rockCacheManager.m_RockCachePrefab.gameObject);
            if (gameObject == null)
            {
                Mod.Logger.Log("Failed to instantiate rock cache prefab for indoor placement.", FlaggedLoggingLevel.Error);
                return false;
            }

            gameObject.name = rockCacheManager.m_RockCachePrefab.name;
            gameObject.SetActive(false);
            GameManager.GetPlayerManagerComponent().StartPlaceMesh(gameObject,
                rockCacheManager.m_BuildRangeMax, PlaceMeshFlags.UseMeshVariant);

            Mod.Logger.Log("Started indoor rock cache placement.", FlaggedLoggingLevel.Debug);

            return false;
        }
    }
}

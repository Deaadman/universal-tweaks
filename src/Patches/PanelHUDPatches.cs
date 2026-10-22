namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_HUD"/>, the in-game HUD.</summary>
internal static class PanelHUDPatches
{
    /// <summary>
    /// Hides the "limited mobility while aiming" icon while aiming a revolver, since <see cref="vp_FPSPlayerPatches"/>
    /// lets the player walk then.
    /// </summary>
    [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.Update))]
    private static class RevolverLimitedMobilityUiDisable
    {
        private static void Postfix(Panel_HUD __instance)
        {
            if (Settings.Instance.RevolverImprovements &&
                GameManager.GetPlayerManagerComponent().GetControlMode() == PlayerControlMode.AimRevolver)
                __instance.m_AimingLimitedMobility.gameObject.SetActive(false);
        }
    }
}

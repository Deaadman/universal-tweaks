using UniversalTweaks.Utilities;

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

    /// <summary>
    /// Keeps the stamina bar showing when <see cref="Settings.PermanentStaminaBar"/> is on, including before the player
    /// first sprints, when the game hasn't shown it yet.
    /// </summary>
    [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.Update))]
    private static class PermanentStaminaBar
    {
        private static void Postfix(Panel_HUD __instance)
        {
            UIWidget sprintBar = __instance.m_SprintBar;
            if (!Settings.Instance.PermanentStaminaBar || sprintBar == null)
                return;

            if (!sprintBar.gameObject.activeSelf)
                sprintBar.gameObject.SetActive(true);
            sprintBar.alpha = 1f;
        }
    }

    /// <summary>Keeps the sun and moon dial on the HUD: see <see cref="TimeOfDayDial"/>.</summary>
    [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.Update))]
    private static class PermanentTimeOfDayDial
    {
        private static void Postfix(Panel_HUD __instance) => TimeOfDayDial.Update(__instance);
    }
}

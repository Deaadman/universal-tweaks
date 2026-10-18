using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class GunsTweaks
{
    [HarmonyPatch(typeof(vp_FPSPlayer), nameof(vp_FPSPlayer.Update))]
    private static class RevolverMovementUnblocked
    {
        private static void Postfix(vp_FPSPlayer __instance)
        {
            if (!Settings.Instance.RevolverImprovements)
            {
                return;
            }

            var controlMode = GameManager.GetPlayerManagerComponent().GetControlMode();
            if (controlMode == PlayerControlMode.AimRevolver && GameManager.IsMoveInputUnblocked())
            {
                __instance.InputWalk();
            }
        }
    }

    [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.Update))]
    private static class RevolverLimitedMobilityUiDisable
    {
        private static void Postfix(Panel_HUD __instance)
        {
            if (Settings.Instance.RevolverImprovements && GameManager.GetPlayerManagerComponent().GetControlMode() ==
                PlayerControlMode.AimRevolver)
            {
                __instance.m_AimingLimitedMobility.gameObject.SetActive(false);
            }
        }
    }
}
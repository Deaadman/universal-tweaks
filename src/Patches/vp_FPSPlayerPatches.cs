namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="vp_FPSPlayer"/>, the player's first-person movement.</summary>
internal static class vp_FPSPlayerPatches
{
    /// <summary>
    /// Lets the player walk while aiming a revolver when <see cref="Settings.RevolverImprovements"/> is on, which the
    /// game doesn't otherwise allow.
    /// </summary>
    [HarmonyPatch(typeof(vp_FPSPlayer), nameof(vp_FPSPlayer.Update))]
    private static class RevolverMovementUnblocked
    {
        private static void Postfix(vp_FPSPlayer __instance)
        {
            if (!Settings.Instance.RevolverImprovements)
                return;

            var controlMode = GameManager.GetPlayerManagerComponent().GetControlMode();
            if (controlMode == PlayerControlMode.AimRevolver && GameManager.IsMoveInputUnblocked())
                __instance.InputWalk();
        }
    }
}

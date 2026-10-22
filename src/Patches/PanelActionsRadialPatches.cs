namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_ActionsRadial"/>, the radial menu.</summary>
internal static class PanelActionsRadialPatches
{
    /// <summary>Greys out spray paint in the radial menu when the player has no spray paint can.</summary>
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.GetShouldGreyOut))]
    private static class GreyOutSprayPaintRadial
    {
        private static bool Prefix(Panel_ActionsRadial.RadialType radialType, ref bool __result)
        {
            if (radialType != Panel_ActionsRadial.RadialType.SprayPaint)
                return true;

            __result = GameManager.GetInventoryComponent().GetBestGearItemWithName("GEAR_SprayPaintCan") == null;
            return false;
        }
    }
}

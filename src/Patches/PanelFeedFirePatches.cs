namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_FeedFire"/>, the screen for adding fuel to a fire.</summary>
internal static class PanelFeedFirePatches
{
    /// <summary>Moves the fire sprite back into line with the rest of the panel; the game leaves it out of place.</summary>
    [HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Initialize))]
    private static class FireSpriteFix
    {
        private static void Postfix(Panel_FeedFire __instance)
        {
            if (__instance.m_Sprite_FireFill == null)
                return;

            __instance.m_Sprite_FireFill.gameObject.transform.localPosition = new Vector3(159.1f, -31.6f, 0);
            __instance.m_Sprite_FireFill.gameObject.transform.localScale = new Vector3(1.7f, 1.7f, 1);

            Mod.Logger.Log("Applied feed fire sprite fix.", FlaggedLoggingLevel.Debug);
        }
    }
}

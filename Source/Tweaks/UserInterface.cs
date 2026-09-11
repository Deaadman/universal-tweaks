using Il2CppTLD.OptionalContent;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class UserInterface
{
    [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.UpdateCrosshair))]
    private static class PermanentCrosshair
    {
        private static bool Prefix(HUDManager __instance)
        {
            if (Settings.Instance.PermanentCrosshair)
            {
                __instance.m_CrosshairAlpha = 1f;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.GetShouldGreyOut))]
    private static class GreyOutSprayPaintRadial
    {
        private static bool Prefix(Panel_ActionsRadial.RadialType radialType, ref bool __result)
        {
            if (radialType != Panel_ActionsRadial.RadialType.SprayPaint)
            {
                return true;
            }

            __result = GameManager.GetInventoryComponent().GetBestGearItemWithName("GEAR_SprayPaintCan") == null;
            Mod.Logger.Log($"Spray paint radial greyed out: {__result}.", FlaggedLoggingLevel.Trace);
            return false;
        }
    }

    [HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Initialize))]
    private static class FireSpriteFix
    {
        private static void Postfix(Panel_FeedFire __instance)
        {
            if (__instance.m_Sprite_FireFill == null)
            {
                return;
            }

            __instance.m_Sprite_FireFill.gameObject.transform.localPosition = new Vector3(159.1f, -31.6f, 0);
            __instance.m_Sprite_FireFill.gameObject.transform.localScale = new Vector3(1.7f, 1.7f, 1);

            Mod.Logger.Log("Applied feed fire sprite fix.", FlaggedLoggingLevel.Debug);
        }
    }

    [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.Initialize))]
    private class RemoveOptionalContentMenus
    {
        private static void Postfix(Panel_MainMenu __instance)
        {
            if (!Settings.Instance.RemoveMainMenuItems)
            {
                return;
            }

            var contentManager = OptionalContentManager.Instance;
            var hasWintermute = contentManager.IsContentOwned(__instance.m_WintermuteConfig);
            RemoveMainMenuItem(Panel_MainMenu.MainMenuItem.MainMenuItemType.TFTFTUpsell, __instance);
            if (!hasWintermute)
            {
                RemoveMainMenuItem(Panel_MainMenu.MainMenuItem.MainMenuItemType.Story, __instance);
            }
        }

        private static void RemoveMainMenuItem(Panel_MainMenu.MainMenuItem.MainMenuItemType removeType,
            Panel_MainMenu __instance)
        {
            for (var i = __instance.m_MenuItems.Count - 1; i >= 0; i--)
            {
                if (__instance.m_MenuItems[i].m_Type == removeType)
                {
                    __instance.m_MenuItems.RemoveAt(i);
                    Mod.Logger.Log($"Removed promotional main menu item '{removeType}'.", FlaggedLoggingLevel.Debug);
                }
            }
        }
    }
}
using Il2CppTLD.OptionalContent;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_MainMenu"/>, the main menu.</summary>
internal static class PanelMainMenuPatches
{
    /// <summary>
    /// Removes the expansion's advert, and WINTERMUTE's if the player doesn't own it, when
    /// <see cref="Settings.RemoveMainMenuItems"/> is on.
    /// </summary>
    [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.Initialize))]
    private static class RemoveOptionalContentMenus
    {
        private static void Postfix(Panel_MainMenu __instance)
        {
            if (!Settings.Instance.RemoveMainMenuItems)
                return;

            var contentManager = OptionalContentManager.Instance;
            var hasWintermute = contentManager.IsContentOwned(__instance.m_WintermuteConfig);
            RemoveMainMenuItem(Panel_MainMenu.MainMenuItem.MainMenuItemType.TFTFTUpsell, __instance);
            if (!hasWintermute)
                RemoveMainMenuItem(Panel_MainMenu.MainMenuItem.MainMenuItemType.Story, __instance);
        }

        /// <summary>Removes every main menu item of a type.</summary>
        /// <param name="removeType">The type of item.</param>
        /// <param name="mainMenu">The main menu.</param>
        private static void RemoveMainMenuItem(Panel_MainMenu.MainMenuItem.MainMenuItemType removeType,
            Panel_MainMenu mainMenu)
        {
            for (var i = mainMenu.m_MenuItems.Count - 1; i >= 0; i--)
            {
                if (mainMenu.m_MenuItems[i].m_Type != removeType)
                    continue;

                mainMenu.m_MenuItems.RemoveAt(i);
                Mod.Logger.Log($"Removed promotional main menu item '{removeType}'.", FlaggedLoggingLevel.Debug);
            }
        }
    }
}

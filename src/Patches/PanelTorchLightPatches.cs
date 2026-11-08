using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_TorchLight"/>, the menu for picking what to light a torch with.</summary>
internal static class PanelTorchLightPatches
{
    /// <summary>Opens the menu even with only one fire tool: see <see cref="TorchLightMenu"/>.</summary>
    [HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.Enable))]
    private static class AlwaysShowMenu
    {
        private static void Prefix(bool enable) => TorchLightMenu.OnEnabling(enable);

        private static void Postfix(Panel_TorchLight __instance) => TorchLightMenu.OnEnabled(__instance);

        private static void Finalizer(Exception? __exception)
        {
            if (__exception != null)
                TorchLightMenu.Reset();
        }
    }

    /// <summary>Holds off using the matches until the torch lights: see <see cref="TorchToolUse"/>.</summary>
    [HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.OnUseMatches))]
    private static class DeferMatches
    {
        private static void Prefix(GearItem matches, ref bool consumeResource) =>
            TorchToolUse.Defer(matches, ref consumeResource);
    }

    /// <summary>Holds off wearing the flint and steel until the torch lights: see <see cref="TorchToolUse"/>.</summary>
    [HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.OnUseFlintAndSteel))]
    private static class DeferFlintAndSteel
    {
        private static void Prefix(Panel_TorchLight __instance, ref bool consumeResource) =>
            TorchToolUse.Defer(__instance.m_Flint, ref consumeResource);
    }

    /// <summary>Holds off wearing the fire striker until the torch lights: see <see cref="TorchToolUse"/>.</summary>
    [HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.OnUseFireStriker))]
    private static class DeferFireStriker
    {
        private static void Prefix(Panel_TorchLight __instance, ref bool consumeResource) =>
            TorchToolUse.Defer(__instance.m_Striker, ref consumeResource);
    }

    /// <summary>Stops the game using the only fire tool as the menu opens: see <see cref="TorchLightMenu"/>.</summary>
    [HarmonyPatch(typeof(Panel_TorchLight), nameof(Panel_TorchLight.OnUseSelectedItem))]
    private static class WaitForChoice
    {
        private static bool Prefix() => TorchLightMenu.AllowUse();
    }
}

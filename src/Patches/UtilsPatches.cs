using UniversalTweaks.Core;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Utils"/>, the game's shared helpers, including inventory icons.</summary>
internal static class UtilsPatches
{
    /// <summary>Shows the mod's own icon for items that have one in <see cref="InventoryIcons"/>, like the brown MRE.</summary>
    [HarmonyPatch(typeof(Utils), nameof(Utils.GetInventoryIconTexture), typeof(GearItem))]
    private static class GenericIconTextureSwap
    {
        private static bool Prefix(GearItem gi, ref Texture2D __result)
        {
            if (gi == null)
                return true;

            var textureName = InventoryIcons.ResolveTextureName(gi.name, Settings.Instance.MRETextureVariant);
            if (string.IsNullOrEmpty(textureName) || !TextureSwapper.TryGetTexture(textureName, out var newTexture))
                return true;

            __result = newTexture;
            return false;
        }
    }

    /// <summary>
    /// Starts the game straight into the main menu when <see cref="Settings.SkipIntro"/> is on, by answering yes when
    /// the boot screen asks whether the game was launched with <c>-skipintro</c>. The game then skips the disclaimers,
    /// the prompt to press a key once loaded, and the intro movie itself.
    /// </summary>
    [HarmonyPatch(typeof(Utils), nameof(Utils.IsCommandLineArgumentPresent))]
    private static class SkipIntro
    {
        private static void Postfix(string arg, ref bool __result)
        {
            if (__result || arg != EmptyScene.SKIP_INTRO_FLAG || !Settings.Instance.SkipIntro)
                return;

            __result = true;
            Mod.Logger.Log("Skipping the intro.", FlaggedLoggingLevel.Debug);
        }
    }
}

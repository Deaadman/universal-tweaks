using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class TextureSwapTweaks
{
    private static readonly Dictionary<string, string> IconTextures = new()
    {
        { "GEAR_MRE", "ico_GearItem__BrownMRE" }
    };

    private static readonly OncePerFrame PrefabTextureUpdate = new();

    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class SwapGearItemTextures
    {
        private static void Postfix()
        {
            if (Settings.Instance.MRETextureVariant && PrefabTextureUpdate.ShouldRun())
            {
                TextureSwapper.SwapGearItemTexture("GEAR_MRE", "Obj_FoodMRE_LOD0",
                    "GEAR_FoodBrownMRE_Dif");
                Mod.Logger.Log("Applied brown MRE texture variant.", FlaggedLoggingLevel.Debug);
            }
        }
    }

    [HarmonyPatch(typeof(Utils), nameof(Utils.GetInventoryIconTexture), typeof(GearItem))]
    private static class GenericIconTextureSwap
    {
        private static bool Prefix(GearItem gi, ref Texture2D __result)
        {
            if (gi == null)
            {
                return true;
            }

            var textureName = ResolveTextureName(gi.name, Settings.Instance.MRETextureVariant);
            if (string.IsNullOrEmpty(textureName) || !TextureSwapper.TryGetTexture(textureName, out var newTexture))
            {
                return true;
            }

            Mod.Logger.Log($"Swapped inventory icon texture for '{gi.name}' to '{textureName}'.", FlaggedLoggingLevel.Trace);

            __result = newTexture;
            return false;
        }
    }

    internal static string ResolveTextureName(string gearItemName, bool mreTextureVariantEnabled)
    {
        if (gearItemName == "GEAR_MRE" && !mreTextureVariantEnabled)
        {
            return string.Empty;
        }

        return IconTextures.TryGetValue(gearItemName, out var textureName) ? textureName : string.Empty;
    }
}
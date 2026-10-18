using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class TextureSwap
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

    internal static string GetTextureNameForGearItem(GearItem gi) =>
        ResolveTextureName(gi.name, Settings.Instance.MRETextureVariant);

    internal static string ResolveTextureName(string gearItemName, bool mreTextureVariantEnabled)
    {
        if (gearItemName == "GEAR_MRE" && !mreTextureVariantEnabled)
        {
            return string.Empty;
        }

        return IconTextures.TryGetValue(gearItemName, out var textureName) ? textureName : string.Empty;
    }
}
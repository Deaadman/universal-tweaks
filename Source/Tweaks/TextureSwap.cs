using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class TextureSwap
{
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class SwapGearItemTextures
    {
        private static void Postfix()
        {
            if (Settings.Instance.MRETextureVariant)
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
        var textureMapping = new Dictionary<string, string>
        {
            { "GEAR_MRE", "ico_GearItem__BrownMRE" }
        };

        if (gearItemName == "GEAR_MRE" && !mreTextureVariantEnabled)
        {
            return string.Empty;
        }

        return textureMapping.TryGetValue(gearItemName, out var textureName) ? textureName : string.Empty;
    }
}
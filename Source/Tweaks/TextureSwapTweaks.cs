using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class TextureSwapTweaks
{
    private const string MreGearName = "GEAR_MRE";
    private const string MreRendererName = "Obj_FoodMRE_LOD0";
    private const string BrownMreTextureName = "GEAR_FoodBrownMRE_Dif";

    private static readonly Dictionary<string, string> IconTextures = new()
    {
        { MreGearName, "ico_GearItem__BrownMRE" }
    };

    private static readonly OncePerFrame PrefabTextureUpdate = new();

    // Captured before the first swap so turning the setting off can put the vanilla texture back.
    private static Texture? _originalMreTexture;

    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class SwapGearItemTextures
    {
        private static void Postfix(GearItem __instance)
        {
            if (!Settings.Instance.MRETextureVariant && _originalMreTexture == null)
            {
                return;
            }

            if (PrefabTextureUpdate.ShouldRun())
            {
                var prefab = GearItem.LoadGearItemPrefab(MreGearName);
                if (prefab != null)
                {
                    ApplyMreTexture(prefab.gameObject);
                }
            }

            if (__instance.name == MreGearName)
            {
                ApplyMreTexture(__instance.gameObject);
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

    internal static void RefreshMreTextures()
    {
        if (!Settings.Instance.MRETextureVariant && _originalMreTexture == null)
        {
            return;
        }

        // Includes inactive items (e.g. ones sitting in the inventory) and the loaded prefab itself.
        var refreshed = 0;
        foreach (var gearItem in Resources.FindObjectsOfTypeAll<GearItem>())
        {
            if (gearItem.name == MreGearName)
            {
                ApplyMreTexture(gearItem.gameObject);
                refreshed++;
            }
        }

        Mod.Logger.Log($"Refreshed the MRE texture on {refreshed} object(s).", FlaggedLoggingLevel.Debug);
    }

    private static void ApplyMreTexture(GameObject mre)
    {
        if (_originalMreTexture == null)
        {
            var currentTexture = TextureSwapper.GetMainTexture(mre, MreRendererName);
            if (currentTexture != null && currentTexture.name != BrownMreTextureName)
            {
                _originalMreTexture = currentTexture;
            }
        }

        Texture? texture = _originalMreTexture;
        if (Settings.Instance.MRETextureVariant && TextureSwapper.TryGetTexture(BrownMreTextureName, out var brownTexture))
        {
            texture = brownTexture;
        }

        if (texture != null)
        {
            TextureSwapper.SetMainTexture(mre, MreRendererName, texture);
        }
    }

    internal static string ResolveTextureName(string gearItemName, bool mreTextureVariantEnabled)
    {
        if (gearItemName == MreGearName && !mreTextureVariantEnabled)
        {
            return string.Empty;
        }

        return IconTextures.TryGetValue(gearItemName, out var textureName) ? textureName : string.Empty;
    }
}
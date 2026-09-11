using UniversalTweaks.Tweaks;

namespace UniversalTweaks.Utilities;

internal static class TextureSwapper
{
    private static readonly AssetBundle? UniversalTweaksAssetBundle =
        AssetBundleLoader.LoadBundle("UniversalTweaks.Resources.UniversalTweaksAssetBundle");

    private static readonly Dictionary<string, Texture2D> Textures = LoadTexturesFromAssetBundle();

    private static Dictionary<string, Texture2D> LoadTexturesFromAssetBundle()
    {
        var loadedTextures = new Dictionary<string, Texture2D>();
        if (UniversalTweaksAssetBundle == null)
        {
            Mod.Logger.Log("Asset bundle unavailable; no textures were loaded.", FlaggedLoggingLevel.Warning);
            return loadedTextures;
        }

        foreach (var texture in UniversalTweaksAssetBundle.LoadAllAssets<Texture2D>())
        {
            loadedTextures[texture.name] = texture;
        }

        Mod.Logger.Log($"Loaded {loadedTextures.Count} texture(s) from the asset bundle.", FlaggedLoggingLevel.Debug);
        return loadedTextures;
    }

    internal static void SwapGearItemTexture(string gearItemName, string gameObjectName, string newTextureName)
    {
        if (!Textures.TryGetValue(newTextureName, out var newTexture))
        {
            Mod.Logger.Log($"Texture '{newTextureName}' was not found; skipping swap for '{gearItemName}'.", FlaggedLoggingLevel.Warning);
            return;
        }

        var gearItemPrefab = GearItem.LoadGearItemPrefab(gearItemName);
        if (gearItemPrefab == null)
        {
            Mod.Logger.Log($"Gear item prefab '{gearItemName}' was not found; skipping texture swap.", FlaggedLoggingLevel.Warning);
            return;
        }

        foreach (var renderer in gearItemPrefab.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name != gameObjectName)
            {
                continue;
            }

            foreach (var material in renderer.materials)
            {
                material.mainTexture = newTexture;
            }
        }

        Mod.Logger.Log($"Swapped texture for '{gearItemName}' ('{gameObjectName}') to '{newTextureName}'.", FlaggedLoggingLevel.Debug);
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

            var textureName = TextureSwap.GetTextureNameForGearItem(gi);
            if (string.IsNullOrEmpty(textureName))
            {
                return true;
            }

            var textures = LoadTexturesFromAssetBundle();
            if (textures.Count == 0)
            {
                return true;
            }

            if (!textures.TryGetValue(textureName, out var newTexture))
            {
                return true;
            }

            Mod.Logger.Log($"Swapped inventory icon texture for '{gi.name}' to '{textureName}'.", FlaggedLoggingLevel.Trace);

            __result = newTexture;
            return false;
        }
    }
}
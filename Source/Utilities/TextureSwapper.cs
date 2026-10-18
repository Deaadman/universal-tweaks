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

    internal static bool TryGetTexture(string textureName, [MaybeNullWhen(false)] out Texture2D texture) =>
        Textures.TryGetValue(textureName, out texture);
}
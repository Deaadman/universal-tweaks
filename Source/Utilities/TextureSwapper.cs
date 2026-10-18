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
            // Unreferenced textures (e.g. icons) are otherwise unloaded on scene change, leaving this cache dangling.
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            loadedTextures[texture.name] = texture;
        }

        Mod.Logger.Log($"Loaded {loadedTextures.Count} texture(s) from the asset bundle.", FlaggedLoggingLevel.Debug);
        return loadedTextures;
    }

    internal static Texture? GetMainTexture(GameObject root, string rendererName)
    {
        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name == rendererName)
            {
                return renderer.sharedMaterial != null ? renderer.sharedMaterial.mainTexture : null;
            }
        }

        return null;
    }

    internal static void SetMainTexture(GameObject root, string rendererName, Texture texture)
    {
        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name != rendererName)
            {
                continue;
            }

            foreach (var material in renderer.materials)
            {
                material.mainTexture = texture;
            }
        }
    }

    internal static bool TryGetTexture(string textureName, [MaybeNullWhen(false)] out Texture2D texture) =>
        Textures.TryGetValue(textureName, out texture) && texture != null;
}
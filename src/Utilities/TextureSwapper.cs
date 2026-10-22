namespace UniversalTweaks.Utilities;

/// <summary>
/// The textures in the mod's asset bundle, and helpers to swap them onto a model's renderers.
/// </summary>
internal static class TextureSwapper
{
    /// <summary>The mod's asset bundle, embedded in the DLL, or <see langword="null"/> if it couldn't be loaded.</summary>
    private static readonly AssetBundle? UniversalTweaksAssetBundle =
        AssetBundleLoader.LoadBundle("UniversalTweaks.Resources.UniversalTweaksAssetBundle");

    /// <summary>Every texture in the asset bundle, by name.</summary>
    private static readonly Dictionary<string, Texture2D> Textures = LoadTexturesFromAssetBundle();

    /// <summary>Loads every texture from the asset bundle and keeps it loaded across scenes.</summary>
    /// <returns>The textures, by name, or none if the bundle couldn't be loaded.</returns>
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

    /// <summary>Gets the main texture of the first renderer with a name, among a model's renderers.</summary>
    /// <param name="root">The model.</param>
    /// <param name="rendererName">The renderer's game object name.</param>
    /// <returns>The texture, or <see langword="null"/> if there's no such renderer or it has no material.</returns>
    internal static Texture? GetMainTexture(GameObject root, string rendererName)
    {
        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name == rendererName)
                return renderer.sharedMaterial != null ? renderer.sharedMaterial.mainTexture : null;
        }

        return null;
    }

    /// <summary>Sets the main texture of every material on every renderer with a name, among a model's renderers.</summary>
    /// <param name="root">The model.</param>
    /// <param name="rendererName">The renderers' game object name.</param>
    /// <param name="texture">The texture.</param>
    internal static void SetMainTexture(GameObject root, string rendererName, Texture texture)
    {
        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name != rendererName)
                continue;

            foreach (var material in renderer.materials)
                material.mainTexture = texture;
        }
    }

    /// <summary>Gets a texture from the asset bundle.</summary>
    /// <param name="textureName">The texture's name.</param>
    /// <param name="texture">The texture, if the bundle has it.</param>
    /// <returns><see langword="true"/> if the bundle has it.</returns>
    internal static bool TryGetTexture(string textureName, [MaybeNullWhen(false)] out Texture2D texture) =>
        Textures.TryGetValue(textureName, out texture) && texture != null;
}

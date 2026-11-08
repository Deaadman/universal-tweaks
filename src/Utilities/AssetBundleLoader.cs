namespace UniversalTweaks.Utilities;

/// <summary>Loads Unity asset bundles embedded in the mod's DLL.</summary>
internal static class AssetBundleLoader
{
    /// <summary>Loads an asset bundle from an embedded resource.</summary>
    /// <param name="path">The resource's manifest name, like <c>UniversalTweaks.Resources.UniversalTweaksAssetBundle</c>.</param>
    /// <returns>The bundle, or <see langword="null"/> if the resource is missing or can't be loaded, which is logged.</returns>
    internal static AssetBundle? LoadBundle(string path)
    {
        try
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            if (stream == null)
            {
                Mod.Logger.Log($"Embedded resource '{path}' was not found.", FlaggedLoggingLevel.Error);
                return null;
            }

            MemoryStream memory = new((int)stream.Length);
            stream.CopyTo(memory);

            // Goes through an Il2Cpp stream: span un-stripping doesn't work as of game 2.51 and MelonLoader 0.7.2-ci.2385.
            Il2CppSystem.IO.MemoryStream memoryStream = new(memory.ToArray());
            var bundle = AssetBundle.LoadFromStream(memoryStream);

            Mod.Logger.Log($"Loaded asset bundle from embedded resource '{path}'.", FlaggedLoggingLevel.Debug);
            return bundle;
        }
        catch (Exception ex)
        {
            Mod.Logger.Log($"Failed to load asset bundle from embedded resource '{path}'.", FlaggedLoggingLevel.Exception, ex);
            return null;
        }
    }
}

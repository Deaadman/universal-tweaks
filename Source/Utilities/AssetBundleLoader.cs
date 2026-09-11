namespace UniversalTweaks.Utilities;

internal static class AssetBundleLoader
{
    internal static AssetBundle? LoadBundle(string path)
    {
        try
        {
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            if (stream == null)
            {
                Mod.Logger.Log($"Embedded resource '{path}' was not found.", FlaggedLoggingLevel.Error);
                return null;
            }

            MemoryStream memory = new((int)stream.Length);
            stream.CopyTo(memory);

            Il2CppSystem.IO.MemoryStream memoryStream = new(memory.ToArray()); // workaround to deal with span un-stripping not working properly as of 2.51 && v0.7.2-ci.2385
            AssetBundle bundle = AssetBundle.LoadFromStream(memoryStream);

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
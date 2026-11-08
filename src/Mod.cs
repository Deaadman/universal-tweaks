using LocalizationUtilities;

using UniversalTweaks.Utilities;

namespace UniversalTweaks;

/// <summary>
/// The mod's entry point for MelonLoader. Most of what it does is in the Harmony patches in <see cref="Patches"/>,
/// which MelonLoader applies when it loads the mod. It loads the translations and sets up the settings.
/// </summary>
internal sealed class Mod : MelonMod
{
    /// <summary>The manifest name of the embedded translations, read by ModComponent's localization.</summary>
    private const string LocalizationResource = "UniversalTweaks.Resources.Localization.json";

    /// <summary>
    /// The mod's logger. It writes to the MelonLoader console and <c>MelonLoader\Latest.log</c>, at the levels the
    /// player turns on in Complex Logger's settings.
    /// </summary>
    internal static readonly ComplexLogger.ComplexLogger<Mod> Logger = new();

    /// <inheritdoc />
    public override void OnInitializeMelon()
    {
        Logger.Log("Initializing Universal Tweaks.", FlaggedLoggingLevel.Debug);

        LoadLocalizations();
        Settings.Initialize();
        ContainerSettings.Initialize();

        Logger.Log($"{Properties.BuildInfo.Name} {Properties.BuildInfo.Version} initialized.", FlaggedLoggingLevel.Debug);
    }

    /// <inheritdoc />
    /// <remarks>Prunes destroyed objects from every <see cref="ObjectStateCache{TObject, TState}"/>.</remarks>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName) => ObjectStateCaches.PruneAll();

    /// <summary>Loads the mod's translations, like the snare's "Set" prompt, from the embedded JSON file.</summary>
    private static void LoadLocalizations()
    {
        try
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(LocalizationResource);
            if (stream == null)
            {
                Logger.Log($"Embedded resource '{LocalizationResource}' was not found.", FlaggedLoggingLevel.Error);
                return;
            }

            using StreamReader reader = new(stream);
            LocalizationManager.LoadJsonLocalization(reader.ReadToEnd());

            Logger.Log("Loaded localizations.", FlaggedLoggingLevel.Debug);
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to load localizations.", FlaggedLoggingLevel.Exception, ex);
        }
    }
}

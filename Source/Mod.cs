using LocalizationUtilities;

using UniversalTweaks.Properties;

namespace UniversalTweaks;

internal sealed class Mod : MelonMod
{
    internal static ComplexLogger.ComplexLogger<Mod> Logger = new();

    public override void OnInitializeMelon()
    {
        Logger.Log("Initializing Universal Tweaks.", FlaggedLoggingLevel.Debug);

        LoadLocalizations();
        Settings.OnLoad();
        ContainerSettings.OnLoad();

        Logger.Log($"{Properties.BuildInfo.Name} v{Properties.BuildInfo.Version} initialized.", FlaggedLoggingLevel.Debug);
    }

    private static void LoadLocalizations()
    {
        const string jsonFile = "UniversalTweaks.Resources.Localization.json";

        try
        {
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(jsonFile);
            if (stream == null)
            {
                Logger.Log($"Embedded resource '{jsonFile}' was not found.", FlaggedLoggingLevel.Error);
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
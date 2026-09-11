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

        Logger.Log($"{Properties.BuildInfo.Name} v{Properties.BuildInfo.Version} initialized.", FlaggedLoggingLevel.Always);
    }

    private static void LoadLocalizations()
    {
        const string jsonFile = "UniversalTweaks.Resources.Localization.json";

        try
        {
#pragma warning disable CS8600, CS8604
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(jsonFile))
            {
                using StreamReader reader = new(stream);
                string results = reader.ReadToEnd();
                LocalizationManager.LoadJsonLocalization(results);
            }
#pragma warning restore CS8600, CS8604

            Logger.Log("Loaded localizations.", FlaggedLoggingLevel.Debug);
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to load localizations.", FlaggedLoggingLevel.Exception, ex);
        }
    }
}
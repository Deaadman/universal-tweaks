using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;

/// <summary>
/// The Universal Tweaks repository, which the editor scripts read the mod's container list from. Asked for the first
/// time it's needed, then remembered in the editor's preferences.
/// </summary>
public static class UniversalTweaksRepository
{
    private const string PathPreference = "UniversalTweaks.RepositoryPath";

    /// <summary>The file with the mod's container list, which every copy of the repository has.</summary>
    public static readonly string ContainerCapacitiesFile = Path.Combine("src", "Utilities", "ContainerCapacities.cs");

    /// <summary>An entry of the container list, like <c>("CONTAINER_Safe", nameof(ContainerSettings.ContainerSafeCapacity))</c>.</summary>
    private static readonly Regex CapacityEntry = new Regex(@"\(""([^""]+)"",\s*nameof\(ContainerSettings\.(\w+)\)\)");

    /// <summary>Gets the repository's folder, asking for it if it isn't known or has moved.</summary>
    /// <returns>The folder, or null if none was chosen.</returns>
    public static string Find()
    {
        var path = EditorPrefs.GetString(PathPreference, "");
        if (IsRepository(path))
            return path;

        path = EditorUtility.OpenFolderPanel("Choose the Universal Tweaks repository", "", "");
        if (!IsRepository(path))
        {
            Debug.LogWarning($"'{path}' isn't the Universal Tweaks repository: it has no {ContainerCapacitiesFile}.");
            return null;
        }

        EditorPrefs.SetString(PathPreference, path);
        return path;
    }

    [MenuItem("Tools/Universal Tweaks/Choose Repository Folder")]
    private static void Choose()
    {
        EditorPrefs.DeleteKey(PathPreference);
        var path = Find();
        if (path != null)
            Debug.Log($"Using the Universal Tweaks repository at {path}");
    }

    /// <summary>
    /// Reads the mod's container list from ContainerCapacities.cs: each name fragment with the setting it uses, in the
    /// order the mod matches them.
    /// </summary>
    /// <returns>The fragments and settings, or none if the file isn't there.</returns>
    public static List<KeyValuePair<string, string>> ReadContainerCapacities(string repository)
    {
        var capacities = new List<KeyValuePair<string, string>>();
        var path = Path.Combine(repository, ContainerCapacitiesFile);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Couldn't find {path}, so every container is logged.");
            return capacities;
        }

        foreach (Match match in CapacityEntry.Matches(File.ReadAllText(path)))
            capacities.Add(new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value));

        return capacities;
    }

    /// <summary>Quotes a CSV field if it needs it.</summary>
    public static string Csv(string value) =>
        value.IndexOfAny(new[] { ',', '"', '\n' }) < 0 ? value : $"\"{value.Replace("\"", "\"\"")}\"";

    private static bool IsRepository(string path) =>
        !string.IsNullOrEmpty(path) && File.Exists(Path.Combine(path, ContainerCapacitiesFile));
}

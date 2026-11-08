using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;

/// <summary>
/// Finds every Container in the project's prefabs and writes each kind once to Containers.csv in the project's folder,
/// with the capacity setting the mod gives it, matched by name like the mod's ContainerCapacities. Logs the ones the mod
/// has no setting for.
/// </summary>
/// <remarks>
/// The export flattened nested prefabs, so a container placed in a building is a plain copy inside that building's
/// prefab, named like "CONTAINER_MetalBox_Prefab (3)", and the exporter saved some prefabs twice, like
/// "CONTAINER_Safe_0". Each container is named for its own object without the tags in brackets after the name, so the
/// copies merge into one row. Drawers and doors are containers on a child of the furniture, so children are checked too.
/// </remarks>
public static class FindContainers
{
    private const string OutputFile = "Containers.csv";

    /// <summary>The tags in brackets after a name, like " (2)", "(Clone)" and " (PLACED)".</summary>
    private static readonly Regex Tags = new Regex(@"(\s*\([^()]+\))+$");

    /// <summary>The "_0" the exporter adds to a prefab saved twice.</summary>
    private static readonly Regex ExportSuffix = new Regex(@"_(0|[1-9]\d*)$");

    /// <summary>The units an ItemWeight saves for a kilogram: a 15 kg backpack saves 15000000000.</summary>
    private const double UnitsPerKilogram = 1e9;

    private sealed class ContainerKind
    {
        public int Copies;
        public string Path;
        public string DisplayName = "";
        public readonly SortedSet<string> Capacities = new SortedSet<string>(System.StringComparer.Ordinal);
    }

    [MenuItem("Tools/Universal Tweaks/Find Containers")]
    private static void Find()
    {
        var repository = UniversalTweaksRepository.Find();
        if (repository == null)
            return;

        var capacities = UniversalTweaksRepository.ReadContainerCapacities(repository);

        var guids = AssetDatabase.FindAssets("t:Prefab");
        var containers = new Dictionary<string, ContainerKind>(System.StringComparer.Ordinal);
        var containerCount = 0;

        try
        {
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (i % 50 == 0 && EditorUtility.DisplayCancelableProgressBar(
                        "Finding containers", $"{i} of {guids.Length}: {path}", (float)i / guids.Length))
                {
                    Debug.LogWarning("Stopped finding containers: cancelled.");
                    return;
                }

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                    continue;

                foreach (var container in prefab.GetComponentsInChildren<Container>(true))
                {
                    containerCount++;
                    var name = Tags.Replace(container.name, "").Trim();

                    if (!containers.TryGetValue(name, out var found))
                        containers[name] = found = new ContainerKind { Path = path };

                    // The decompiled members are stubs, so read the saved fields.
                    var serialized = new SerializedObject(container);
                    found.Capacities.Add(ToKilograms(ReadFirstValue(serialized.FindProperty("m_Capacity"))));
                    if (found.DisplayName.Length == 0)
                        found.DisplayName = ReadFirstValue(serialized.FindProperty("m_LocalizedDisplayName"));

                    found.Copies++;
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        MergeExportCopies(containers);

        var names = new List<string>(containers.Keys);
        names.Sort(System.StringComparer.Ordinal);

        var csv = new StringBuilder("Container,Setting,Matched By,Capacity (kg),Display Name,Copies,Path\n");
        var missing = 0;
        foreach (var name in names)
        {
            var setting = "";
            var matchedBy = "";
            foreach (var capacity in capacities)
            {
                // The same match as the mod: the first fragment the name contains wins.
                if (name.Contains(capacity.Key))
                {
                    matchedBy = capacity.Key;
                    setting = capacity.Value;
                    break;
                }
            }

            var kind = containers[name];
            if (setting.Length == 0)
            {
                missing++;
                Debug.Log($"Container without a setting: {name} ({kind.DisplayName})");
            }

            csv.Append($"{UniversalTweaksRepository.Csv(name)},{UniversalTweaksRepository.Csv(setting)}," +
                       $"{UniversalTweaksRepository.Csv(matchedBy)},{UniversalTweaksRepository.Csv(string.Join(" / ", kind.Capacities))}," +
                       $"{UniversalTweaksRepository.Csv(kind.DisplayName)},{kind.Copies},{UniversalTweaksRepository.Csv(kind.Path)}\n");
        }

        var outputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", OutputFile));
        File.WriteAllText(outputPath, csv.ToString());

        Debug.Log($"Checked {guids.Length} prefabs: {containerCount} containers, {names.Count} unique, " +
                  $"{missing} without a setting. Wrote {outputPath}");
        EditorUtility.RevealInFinder(outputPath);
    }

    /// <summary>
    /// Reads the first number or text saved in a field, like an ItemWeight's units or a LocalizedString's key, whose own
    /// field names can't be relied on in the export.
    /// </summary>
    /// <returns>The value, or "" if the field isn't there or holds neither.</returns>
    private static string ReadFirstValue(SerializedProperty property)
    {
        if (property == null)
            return "";

        var end = property.GetEndProperty();
        var child = property.Copy();
        while (child.Next(true) && !SerializedProperty.EqualContents(child, end))
        {
            switch (child.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return child.longValue.ToString();
                case SerializedPropertyType.Float:
                    return child.floatValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                case SerializedPropertyType.String:
                    return child.stringValue;
            }
        }

        return "";
    }

    /// <summary>Turns a saved ItemWeight's units into kilograms, leaving anything else as it is.</summary>
    private static string ToKilograms(string units) =>
        long.TryParse(units, out var value)
            ? (value / UnitsPerKilogram).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)
            : units;

    /// <summary>
    /// Merges each "Name_0" into "Name" when both were found. Only then, so a name that really ends in a number stays.
    /// </summary>
    private static void MergeExportCopies(Dictionary<string, ContainerKind> containers)
    {
        foreach (var name in new List<string>(containers.Keys))
        {
            var original = ExportSuffix.Replace(name, "");
            if (original == name || !containers.TryGetValue(original, out var kept))
                continue;

            var copy = containers[name];
            kept.Copies += copy.Copies;
            kept.Capacities.UnionWith(copy.Capacities);
            containers.Remove(name);
        }
    }
}

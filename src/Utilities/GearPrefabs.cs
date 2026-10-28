using Il2CppTLD.AddressableAssets;

namespace UniversalTweaks.Utilities;

/// <summary>Loads gear item prefabs by name, which the game stopped offering itself in 2.58.</summary>
/// <remarks>
/// Each prefab is loaded once and kept, so it stays in memory and changes made to it reach every item made from it
/// later. A prefab that's been unloaded anyway is loaded again.
/// </remarks>
internal static class GearPrefabs
{
    /// <summary>The prefabs loaded so far, by gear item name.</summary>
    private static readonly Dictionary<string, GearItem> s_Prefabs = new();

    /// <summary>Gets a gear item's prefab, loading it if needed.</summary>
    /// <param name="gearName">The gear item's name, like <c>GEAR_FoodMRE</c>.</param>
    /// <returns>The prefab's gear item, or <see langword="null"/> if there's no gear item by that name.</returns>
    internal static GearItem? Load(string gearName)
    {
        if (s_Prefabs.TryGetValue(gearName, out var cached) && cached != null)
            return cached;

        var prefab = AssetHelper.SafeLoadAssetAsync<GameObject>(gearName).WaitForCompletion();
        var gearItem = prefab == null ? null : prefab.GetComponent<GearItem>();
        if (gearItem == null)
        {
            Mod.Logger.Log($"Couldn't load the {gearName} prefab.", FlaggedLoggingLevel.Warning);
            return null;
        }

        s_Prefabs[gearName] = gearItem;
        return gearItem;
    }
}

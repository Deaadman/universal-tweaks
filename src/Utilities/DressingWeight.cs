using Il2CppTLD.Gear;
using Il2CppTLD.IntBackedUnit;

namespace UniversalTweaks.Utilities;

/// <summary>Makes the old man's beard dressing weigh the same as lichen when the setting is on.</summary>
/// <remarks>
/// The weight lives on the gear item data asset all dressings share, so changing it once changes them all.
/// <see cref="Patches.GearItemPatches"/> applies it again as dressings load, in case the asset was reloaded.
/// </remarks>
internal static class DressingWeight
{
    /// <summary>The old man's beard dressing's gear item name.</summary>
    internal const string GearName = "GEAR_OldMansBeardDressing";

    /// <summary>Lichen's weight, in kilograms.</summary>
    private const float ConsistentWeightKG = 0.03f;

    /// <summary>The dressing's own weight, captured before the first change so turning the setting off can put it back.</summary>
    private static ItemWeight? s_OriginalWeight;

    /// <summary>Applies the setting to the dressing's prefab, and so to every dressing.</summary>
    internal static void Refresh()
    {
        var prefab = GearItem.LoadGearItemPrefab(GearName);
        if (prefab != null)
            Apply(prefab.m_GearItemData);
    }

    /// <summary>Sets the dressing's weight to lichen's, or back to its own.</summary>
    /// <param name="data">The dressing's gear item data, if it has any.</param>
    internal static void Apply(GearItemData? data)
    {
        if (data == null)
            return;

        s_OriginalWeight ??= data.m_BaseWeight;

        data.m_BaseWeight = Settings.Instance.ConsistantDressingWeight
            ? ItemWeight.FromKilograms(ConsistentWeightKG)
            : s_OriginalWeight.Value;

        Mod.Logger.Log($"Set Old Man's Dressing base weight to {data.m_BaseWeight}.", FlaggedLoggingLevel.Trace);
    }
}

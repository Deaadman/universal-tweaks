using Il2CppTLD.Gear;

namespace UniversalTweaks.Utilities;

/// <summary>Sets how much fatigue the meat and vegetable stews take away.</summary>
/// <remarks>
/// The value is set on the stews themselves rather than by patching <see cref="FoodStatEffect"/>: its getters are too
/// small to hook safely, and hooking them crashed the game in the cooking and inventory menus.
/// </remarks>
internal static class StewFatigue
{
    /// <summary>The stews' gear item names.</summary>
    private static readonly string[] StewGearNames = ["GEAR_CookedStewMeat", "GEAR_CookedStewVegetables"];

    /// <summary>Limits updating the stew prefabs to once a frame, however many items load in it.</summary>
    private static readonly OncePerFrame PrefabUpdate = new();

    /// <summary>Applies the setting to the stew prefabs, and to a gear item that's just loaded, if it's a stew.</summary>
    /// <param name="gearItem">The gear item.</param>
    internal static void OnGearItemLoaded(GearItem gearItem)
    {
        if (PrefabUpdate.ShouldRun())
            ApplyToPrefabs();

        if (IsStew(gearItem.name))
            Apply(gearItem);
    }

    /// <summary>Applies the setting to every stew, including ones in the inventory and the loaded prefabs.</summary>
    internal static void Refresh()
    {
        foreach (var gearItem in Resources.FindObjectsOfTypeAll<GearItem>())
        {
            if (IsStew(gearItem.name))
                Apply(gearItem);
        }

        ApplyToPrefabs();
    }

    /// <summary>Applies the setting to the stew prefabs, which newly cooked stews are made from.</summary>
    private static void ApplyToPrefabs()
    {
        foreach (var gearName in StewGearNames)
        {
            var prefab = GearPrefabs.Load(gearName);
            if (prefab != null)
                Apply(prefab);
        }
    }

    /// <summary>Sets a stew's fatigue effect to <see cref="Settings.ReduceStewFatigueLossAmount"/>.</summary>
    /// <param name="stew">The stew.</param>
    private static void Apply(GearItem stew)
    {
        foreach (var effect in stew.GetComponents<FoodStatEffect>())
        {
            if (effect.m_Stat == FoodStatEffect.StatType.Fatigue)
                effect.m_Effect = Settings.Instance.ReduceStewFatigueLossAmount;
        }
    }

    /// <summary>Whether a gear item is one of the stews.</summary>
    /// <param name="gearItemName">The gear item's name.</param>
    /// <returns><see langword="true"/> for the meat or vegetable stew.</returns>
    private static bool IsStew(string gearItemName) => Array.IndexOf(StewGearNames, gearItemName) >= 0;
}

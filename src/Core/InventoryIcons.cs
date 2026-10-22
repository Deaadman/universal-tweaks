namespace UniversalTweaks.Core;

/// <summary>The inventory icons the mod swaps in from its asset bundle, by the gear item they belong to.</summary>
internal static class InventoryIcons
{
    /// <summary>The MRE's gear item name.</summary>
    internal const string MreGearName = "GEAR_MRE";

    /// <summary>The icon texture to use for each gear item, by gear item name.</summary>
    private static readonly Dictionary<string, string> IconTextures = new()
    {
        { MreGearName, "ico_GearItem__BrownMRE" }
    };

    /// <summary>Gets the name of the icon texture to show for a gear item instead of the game's own.</summary>
    /// <param name="gearItemName">The gear item's name, like <c>GEAR_MRE</c>.</param>
    /// <param name="mreTextureVariantEnabled">Whether the brown MRE is on.</param>
    /// <returns>The texture's name, or an empty string to keep the game's icon.</returns>
    internal static string ResolveTextureName(string gearItemName, bool mreTextureVariantEnabled)
    {
        if (gearItemName == MreGearName && !mreTextureVariantEnabled)
            return string.Empty;

        return IconTextures.TryGetValue(gearItemName, out var textureName) ? textureName : string.Empty;
    }
}

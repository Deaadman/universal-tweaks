using UniversalTweaks.Core;

namespace UniversalTweaks.Utilities;

/// <summary>Swaps the MRE's blue texture for the brown one in the mod's asset bundle, and back.</summary>
internal static class MreTexture
{
    /// <summary>The MRE model's renderer that has the texture.</summary>
    private const string RendererName = "Obj_FoodMRE_LOD0";

    /// <summary>The brown texture's name in the asset bundle.</summary>
    private const string BrownTextureName = "GEAR_FoodBrownMRE_Dif";

    /// <summary>Limits updating the MRE prefab to once a frame, however many items load in it.</summary>
    private static readonly OncePerFrame PrefabTextureUpdate = new();

    /// <summary>The game's own MRE texture, captured before the first swap so turning the setting off can put it back.</summary>
    private static Texture? s_OriginalTexture;

    /// <summary>Whether there's nothing to do: the brown MRE is off and was never on.</summary>
    private static bool NothingToDo => !Settings.Instance.MRETextureVariant && s_OriginalTexture == null;

    /// <summary>Applies the setting to the MRE prefab and to a gear item that's just loaded, if it's an MRE.</summary>
    /// <param name="gearItem">The gear item.</param>
    internal static void OnGearItemLoaded(GearItem gearItem)
    {
        if (NothingToDo)
            return;

        if (PrefabTextureUpdate.ShouldRun())
        {
            var prefab = GearItem.LoadGearItemPrefab(InventoryIcons.MreGearName);
            if (prefab != null)
                Apply(prefab.gameObject);
        }

        if (gearItem.name == InventoryIcons.MreGearName)
            Apply(gearItem.gameObject);
    }

    /// <summary>Applies the setting to every MRE, including ones in the inventory and the loaded prefab.</summary>
    internal static void Refresh()
    {
        if (NothingToDo)
            return;

        var refreshed = 0;
        foreach (var gearItem in Resources.FindObjectsOfTypeAll<GearItem>())
        {
            if (gearItem.name == InventoryIcons.MreGearName)
            {
                Apply(gearItem.gameObject);
                refreshed++;
            }
        }

        Mod.Logger.Log($"Refreshed the MRE texture on {refreshed} object(s).", FlaggedLoggingLevel.Debug);
    }

    /// <summary>Puts the brown texture on an MRE when the setting is on, or the game's own when it's off.</summary>
    /// <param name="mre">The MRE's game object.</param>
    private static void Apply(GameObject mre)
    {
        if (s_OriginalTexture == null)
        {
            var currentTexture = TextureSwapper.GetMainTexture(mre, RendererName);
            if (currentTexture != null && currentTexture.name != BrownTextureName)
                s_OriginalTexture = currentTexture;
        }

        var texture = s_OriginalTexture;
        if (Settings.Instance.MRETextureVariant && TextureSwapper.TryGetTexture(BrownTextureName, out var brownTexture))
            texture = brownTexture;

        if (texture != null)
            TextureSwapper.SetMainTexture(mre, RendererName, texture);
    }
}

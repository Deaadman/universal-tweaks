namespace UniversalTweaks.Utilities;

/// <summary>The spray paint tweaks: glowing decals, and marking the map whatever the visibility.</summary>
internal static class SprayPaint
{
    /// <summary>
    /// The mod's own copy of the game's glow material. The original is never changed: other decals use it too, like the
    /// glowing footprints of invisible creatures.
    /// </summary>
    private static Material? s_GlowMaterialInstance;

    /// <summary>
    /// The decal manager's own spray paint material, captured before it's ever swapped, so turning glowing decals off
    /// puts back exactly what was there.
    /// </summary>
    private static Material? s_OriginalRevealMaterial;

    /// <summary>
    /// Whether the player is placing a decal right now. The map visibility check is shared with charcoal surveys and rock
    /// caches, so <see cref="Patches.CharcoalItemPatches"/> only skips it while this is set.
    /// </summary>
    internal static bool PlacingDecal { get; set; }

    /// <summary>
    /// Swaps the decal manager's spray paint material for the glowing one when glowing decals are on, and back when
    /// they're off.
    /// </summary>
    /// <param name="manager">The decal manager, about to draw a decal.</param>
    internal static void ApplyGlowingMaterial(DynamicDecalsManager manager)
    {
        // Unity's == null also catches materials destroyed with the scene, which a plain ??= would miss.
        if (s_OriginalRevealMaterial == null)
            s_OriginalRevealMaterial = manager.m_AnimatedRevealMaterial;

        if (!Settings.Instance.ConfirmedGlowingDecals || manager.m_GlowMaterial == null)
        {
            if (manager.m_AnimatedRevealMaterial == s_GlowMaterialInstance)
            {
                manager.m_AnimatedRevealMaterial = s_OriginalRevealMaterial;

                Mod.Logger.Log("Reverted to standard decal material.", FlaggedLoggingLevel.Trace);
            }

            return;
        }

        if (s_GlowMaterialInstance == null)
            s_GlowMaterialInstance = new Material(manager.m_GlowMaterial);

        s_GlowMaterialInstance.SetColor("_GlowColor", new Color(1f, 0.4489248f, 0f, 0f));
        s_GlowMaterialInstance.SetFloat("_GlowMult", Settings.Instance.ConfirmedGlowingDecalMultiplier);

        manager.m_AnimatedRevealMaterial = s_GlowMaterialInstance;

        Mod.Logger.Log("Applied glowing decal material.", FlaggedLoggingLevel.Trace);
    }
}

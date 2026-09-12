using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class Decals
{
    [HarmonyPatch(typeof(DynamicDecalsManager), nameof(DynamicDecalsManager.RenderDynamicDecal))]
    private static class GlowingDecals
    {
        // A private copy so we never mutate the vanilla m_GlowMaterial asset in place - it's also used by
        // unrelated GlowDecal-type decals elsewhere (e.g. invisible-creature glow footprints).
        private static Material? _glowMaterialInstance;

        // The manager's own default spray paint reveal material, captured before we ever swap it out, so
        // disabling the setting restores exactly what was there instead of guessing at a substitute.
        private static Material? _originalRevealMaterial;

        private static bool Prefix(DynamicDecalsManager __instance)
        {
            // Unity's Object equality (== / != null) detects natively-destroyed objects even when the C#
            // reference itself isn't null - which a plain ??= would miss, leaving a dangling Material behind
            // after a scene/region transition destroys our dynamically-created copy.
            if (_originalRevealMaterial == null)
            {
                _originalRevealMaterial = __instance.m_AnimatedRevealMaterial;
            }

            if (!Settings.Instance.ConfirmedGlowingDecals || __instance.m_GlowMaterial == null)
            {
                if (__instance.m_AnimatedRevealMaterial == _glowMaterialInstance)
                {
                    __instance.m_AnimatedRevealMaterial = _originalRevealMaterial;

                    Mod.Logger.Log("Reverted to standard decal material.", FlaggedLoggingLevel.Trace);
                }

                return true;
            }

            if (_glowMaterialInstance == null)
            {
                _glowMaterialInstance = new Material(__instance.m_GlowMaterial);
            }

            _glowMaterialInstance.SetColor("_GlowColor", new Color(1f, 0.4489248f, 0f, 0f));
            _glowMaterialInstance.SetFloat("_GlowMult", Settings.Instance.ConfirmedGlowingDecalMultiplier);

            __instance.m_AnimatedRevealMaterial = _glowMaterialInstance;

            Mod.Logger.Log("Applied glowing decal material.", FlaggedLoggingLevel.Trace);

            return true;
        }
    }

    [HarmonyPatch(typeof(Panel_SprayPaint), nameof(Panel_SprayPaint.Enable), [typeof(bool)])]
    private static class DecalRestrictions
    {
        private static void Postfix()
        {
            var dynamicDecalsManager = GameManager.GetDynamicDecalsManager();
            dynamicDecalsManager.m_DecalOverlapLeniencyPercent = Settings.Instance.DecalOverlapLeniency;

            Mod.Logger.Log($"Decal overlap leniency set to {Settings.Instance.DecalOverlapLeniency}.", FlaggedLoggingLevel.Debug);
        }
    }
}
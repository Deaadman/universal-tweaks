using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class Decals
{
    [HarmonyPatch(typeof(DynamicDecalsManager), nameof(DynamicDecalsManager.RenderDynamicDecal))]
    private static class GlowingDecals
    {
        private static bool Prefix(DynamicDecalsManager __instance)
        {
            if (!Settings.Instance.GlowingDecals && __instance.m_GlowMaterial == null)
            {
                return true;
            }

            __instance.m_GlowMaterial.SetColor("_GlowColor", new Color(1f, 0.4489248f, 0f, 0f));
            __instance.m_GlowMaterial.SetFloat("_GlowMult", Settings.Instance.GlowingDecalMultiplier);

            __instance.m_AnimatedRevealMaterial = __instance.m_GlowMaterial;

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
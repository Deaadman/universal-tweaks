using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class EncumberTweaks
{
    [HarmonyPatch(typeof(Encumber), nameof(Encumber.Start))]
    private static class StartEncumberTweaks
    {
        private static void Postfix(Encumber __instance)
        {
            EncumberUpdate(__instance);
        }
    }

    // Scales the weight burn back to vanilla's 30 kg capacity; without extra carry weight the game's result stands.
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.CalculateModifiedCalorieBurnRate), [typeof(float)])]
    private static class CalorieBurnRateFix
    {
        private static void Postfix(PlayerManager __instance, float baseBurnRate, ref float __result)
        {
            if (ResolveAdditionalWeight() <= 0f)
            {
                return;
            }

            var encumber = GameManager.GetEncumberComponent();
            var freezing = GameManager.GetFreezingComponent();
            var isMoving = __instance.PlayerIsSprinting() || __instance.PlayerIsWalking();
            var rate = baseBurnRate;

            if (isMoving || __instance.PlayerIsClimbing())
            {
                var maxCarryCapacityKG = encumber.m_MaxCarryCapacity.m_Units / (float)OneKilogram.m_Units;
                rate += encumber.GetHourlyCalorieBurnFromWeight() * (30f / maxCarryCapacityKG);
            }

            if (freezing.IsFreezing())
            {
                rate *= freezing.m_CalorieBurnMultiplier;
            }

            if (isMoving)
            {
                var playerVelocity = GameManager.GetVpFPSPlayer().Controller.Velocity.normalized.y;
                if (playerVelocity > 0.1f)
                {
                    var speed = (playerVelocity - 0.1f) / 0.5f;
                    speed = Mathf.Clamp(speed, 0f, 1f);
                    var speedMod = Mathf.Lerp(1f, 1.5f, speed);
                    rate *= speedMod;
                }
            }

            rate *= GameManager.GetExperienceModeManagerComponent().GetCalorieBurnScale();
            __result = rate * GameManager.GetFeatEfficientMachine().ReduceCaloriesScale();
        }
    }

    // ItemWeight stores whole internal units rather than kilograms, so conversions go through a 1 kg reference.
    private static ItemWeight OneKilogram => ItemWeight.FromKilograms(1f);

    private static float ResolveAdditionalWeight() =>
        EncumberCalculator.ResolveAdditionalWeight(Settings.Instance.AdditionalEncumbermentWeight,
            Settings.Instance.CheatingTweaks && Settings.Instance.InfiniteEncumberWeight);

    internal static void EncumberUpdate(Encumber encumber)
    {
        var additionalWeight = ResolveAdditionalWeight();
        var thresholds = EncumberCalculator.CalculateThresholds(additionalWeight);

        encumber.m_MaxCarryCapacity = ItemWeight.FromKilograms(thresholds.MaxCarryCapacity);
        encumber.m_MaxCarryCapacityWhenExhausted = ItemWeight.FromKilograms(thresholds.MaxCarryCapacityWhenExhausted);
        encumber.m_NoSprintCarryCapacity = ItemWeight.FromKilograms(thresholds.NoSprintCarryCapacity);
        encumber.m_NoWalkCarryCapacity = ItemWeight.FromKilograms(thresholds.NoWalkCarryCapacity);
        encumber.m_EncumberLowThreshold = ItemWeight.FromKilograms(thresholds.EncumberLowThreshold);
        encumber.m_EncumberMedThreshold = ItemWeight.FromKilograms(thresholds.EncumberMedThreshold);
        encumber.m_EncumberHighThreshold = ItemWeight.FromKilograms(thresholds.EncumberHighThreshold);

        Mod.Logger.Log($"Applied encumbrance tweaks with {additionalWeight}KG additional weight.", FlaggedLoggingLevel.Debug);
    }
}
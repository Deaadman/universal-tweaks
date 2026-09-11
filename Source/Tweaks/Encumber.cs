using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class Encumber
{
    [HarmonyPatch(typeof(Il2Cpp.Encumber), nameof(Il2Cpp.Encumber.Start))]
    private class StartEncumberTweaks
    {
        private static void Postfix(Il2Cpp.Encumber __instance)
        {
            EncumberUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.CalculateModifiedCalorieBurnRate), [typeof(float)])]
    private class CalorieBurnRateFix
    {
        private static void Postfix(PlayerManager __instance, float baseBurnRate, ref float __result)
        {
            var rate = baseBurnRate;

            if (__instance.PlayerIsSprinting() || __instance.PlayerIsWalking() || __instance.PlayerIsClimbing())
            {
                rate += GameManager.GetEncumberComponent().GetHourlyCalorieBurnFromWeight() *
                        (30f / GameManager.GetEncumberComponent().m_MaxCarryCapacity.m_Units);
            }

            if (GameManager.GetFreezingComponent().IsFreezing())
            {
                rate *= GameManager.GetFreezingComponent().m_CalorieBurnMultiplier;
            }

            if (__instance.PlayerIsSprinting() || __instance.PlayerIsWalking())
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
            var moddedBurn = rate * GameManager.GetFeatEfficientMachine().ReduceCaloriesScale();

            __result = moddedBurn;
        }
    }

    internal static void EncumberUpdate(Il2Cpp.Encumber encumber)
    {
        var additionalWeight = EncumberCalculator.ResolveAdditionalWeight(
            Settings.Instance.AdditionalEncumbermentWeight, Settings.Instance.InfiniteEncumberWeight);
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
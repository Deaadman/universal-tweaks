using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;

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
        if (Settings.Instance.AdditionalEncumbermentWeight > 0 || Settings.Instance.InfiniteEncumberWeight)
        {
            var additionalWeight = Settings.Instance.AdditionalEncumbermentWeight;
            if (Settings.Instance.InfiniteEncumberWeight) additionalWeight = 9970;

            encumber.m_MaxCarryCapacity = ItemWeight.FromKilograms(30f + additionalWeight);
            encumber.m_MaxCarryCapacityWhenExhausted = ItemWeight.FromKilograms(15f + additionalWeight);
            encumber.m_NoSprintCarryCapacity = ItemWeight.FromKilograms(40f + additionalWeight);
            encumber.m_NoWalkCarryCapacity = ItemWeight.FromKilograms(60f + additionalWeight);
            encumber.m_EncumberLowThreshold = ItemWeight.FromKilograms(31f + additionalWeight);
            encumber.m_EncumberMedThreshold = ItemWeight.FromKilograms(40f + additionalWeight);
            encumber.m_EncumberHighThreshold = ItemWeight.FromKilograms(60f + additionalWeight);

            Mod.Logger.Log($"Applied encumbrance tweaks with {additionalWeight}KG additional weight.", FlaggedLoggingLevel.Debug);
        }
        else
        {
            encumber.m_MaxCarryCapacity = ItemWeight.FromKilograms(30f);
            encumber.m_MaxCarryCapacityWhenExhausted = ItemWeight.FromKilograms(15f);
            encumber.m_NoSprintCarryCapacity = ItemWeight.FromKilograms(40f);
            encumber.m_NoWalkCarryCapacity = ItemWeight.FromKilograms(60f);
            encumber.m_EncumberLowThreshold = ItemWeight.FromKilograms(31f);
            encumber.m_EncumberMedThreshold = ItemWeight.FromKilograms(40f);
            encumber.m_EncumberHighThreshold = ItemWeight.FromKilograms(60f);
        }
    }
}
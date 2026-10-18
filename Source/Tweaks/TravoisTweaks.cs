using Il2CppTLD.BigCarry;
using Il2CppTLD.Interactions;

using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class TravoisTweaks
{
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.CanPerformInteractionWhileCarrying))]
    private static class OverrideInteractionRestrictionsWhileCarrying
    {
        private static void Postfix(ref bool __result, IInteraction interaction)
        {
            if (!Settings.Instance.OverrideTravoisInteractionRestrictions)
            {
                return;
            }

            if (GameManager.GetPlayerInVehicle().IsEntering() ||
                GameManager.GetSnowShelterManager().PlayerEnteringShelter())
            {
                __result = false;
            }
            else
            {
                __result = true;
                Mod.Logger.Log($"Overrode travois interaction restriction for '{interaction}'.", FlaggedLoggingLevel.Trace);
            }
        }
    }

    [HarmonyPatch(typeof(TravoisMovement), nameof(TravoisMovement.CheckMovementRestriction))]
    private static class OverrideMovementRestrictions
    {
        private static void Postfix(ref CarryDisplayError __result)
        {
            if (Settings.Instance.OverrideTravoisMovementRestrictions)
            {
                __result = CarryDisplayError.None;
            }
        }
    }

    // Applied each tick rather than on pickup so setting changes reach a travois that is already placed or carried.
    [HarmonyPatch(typeof(TravoisMovement), nameof(TravoisMovement.UpdateTravoisMovement))]
    private static class ApplyMovementTweaks
    {
        private static void Prefix(TravoisMovement __instance)
        {
            __instance.m_TurnSpeed = Settings.Instance.TurnSpeedTravois;
            __instance.m_MaxSlopeClimbAngle = Settings.Instance.MaximumSlopeAngleTravois;
            __instance.m_MaxSlopeDownhillAngle = Settings.Instance.MaximumSlopeAngleTravois;
        }
    }

    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.UpdateTimeDecay))]
    private static class ApplyTimeDecayTweaks
    {
        private static void Prefix(TravoisBigCarryItem __instance) => ApplyDecayTweaks(__instance);
    }

    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.UpdateBlizzardDecay))]
    private static class ApplyBlizzardDecayTweaks
    {
        private static void Prefix(TravoisBigCarryItem __instance) => ApplyDecayTweaks(__instance);
    }

    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.UpdateTravelDecay))]
    private static class ApplyTravelDecayTweaks
    {
        private static void Prefix(TravoisBigCarryItem __instance) => ApplyDecayTweaks(__instance);
    }

    private static void ApplyDecayTweaks(TravoisBigCarryItem travois)
    {
        var settings = Settings.Instance;

        travois.m_BlizzardDecayPerHour = settings.CheatingTweaks
            ? settings.DecayBlizzardTravois
            : VanillaDefaults.TravoisBlizzardDecayPerHour;
        travois.m_DecayHPPerHour = settings.CheatingTweaks
            ? settings.DecayHPPerHourTravois / 1000f
            : VanillaDefaults.TravoisDecayHPPerHour;
        travois.m_MovementDecayPerUnit = settings.CheatingTweaks
            ? settings.DecayMovementPerUnitTravois / 100f
            : VanillaDefaults.TravoisMovementDecayPerUnit;
    }
}
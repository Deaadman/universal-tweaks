using Il2CppTLD.BigCarry;
using Il2CppTLD.Interactions;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class Travois
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

    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.OnCarried))]
    private static class AllTravoisTweaks
    {
        private static void Postfix(TravoisBigCarryItem __instance)
        {
            __instance.m_TravoisMovement.m_TurnSpeed = Settings.Instance.TurnSpeedTravois;
            __instance.m_TravoisMovement.m_MaxSlopeClimbAngle = Settings.Instance.MaximumSlopeAngleTravois;
            __instance.m_TravoisMovement.m_MaxSlopeDownhillAngle = Settings.Instance.MaximumSlopeAngleTravois;

            __instance.m_BlizzardDecayPerHour =
                Settings.Instance.CheatingTweaks ? Settings.Instance.DecayBlizzardTravois : 3;
            __instance.m_DecayHPPerHour = Settings.Instance.CheatingTweaks
                ? Settings.Instance.DecayHPPerHourTravois / 1000f
                : 0.01f;
            __instance.m_MovementDecayPerUnit = Settings.Instance.CheatingTweaks
                ? Settings.Instance.DecayMovementPerUnitTravois / 100f
                : 0.05f;

            Mod.Logger.Log("Applied travois tweaks on pickup.", FlaggedLoggingLevel.Debug);
        }
    }
}
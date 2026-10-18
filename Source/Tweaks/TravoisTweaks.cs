using Il2CppTLD.BigCarry;

using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class TravoisTweaks
{
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.CanPerformInteractionWhileCarrying))]
    private static class OverrideInteractionRestrictionsWhileCarrying
    {
        private static void Postfix(ref bool __result)
        {
            if (!Settings.Instance.CheatingTweaks || !Settings.Instance.OverrideTravoisInteractionRestrictions)
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
            }
        }
    }

    [HarmonyPatch(typeof(TravoisMovement), nameof(TravoisMovement.CheckMovementRestriction))]
    private static class OverrideMovementRestrictions
    {
        private static void Postfix(ref CarryDisplayError __result)
        {
            if (Settings.Instance.CheatingTweaks && Settings.Instance.OverrideTravoisMovementRestrictions)
            {
                __result = CarryDisplayError.None;
            }
        }
    }

    // The game's own movement and decay helpers look inlined (no callers in IL2CPP), so these hook their entry points.
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.UpdateItem))]
    private static class ApplyTweaksOnUpdate
    {
        private static void Prefix(TravoisBigCarryItem __instance) => ApplyTravoisTweaks(__instance);
    }

    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.DeserializePostSceneLoad))]
    private static class ApplyTweaksOnLoad
    {
        private static void Prefix(TravoisBigCarryItem __instance) => ApplyTravoisTweaks(__instance);
    }

    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.OnCarried))]
    private static class ApplyTweaksOnCarried
    {
        private static void Postfix(TravoisBigCarryItem __instance) => ApplyTravoisTweaks(__instance);
    }

    internal static void RefreshTravois()
    {
        foreach (var travois in UnityEngine.Object.FindObjectsOfType<TravoisBigCarryItem>())
        {
            ApplyTravoisTweaks(travois);
        }
    }

    private static void ApplyTravoisTweaks(TravoisBigCarryItem travois)
    {
        var settings = Settings.Instance;

        var movement = travois.m_TravoisMovement;
        if (movement != null)
        {
            movement.m_TurnSpeed = settings.TurnSpeedTravois;
            movement.m_MaxSlopeClimbAngle = settings.MaximumSlopeAngleTravois;
            movement.m_MaxSlopeDownhillAngle = settings.MaximumSlopeAngleTravois;
        }

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
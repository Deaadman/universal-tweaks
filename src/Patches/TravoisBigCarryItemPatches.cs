using Il2CppTLD.BigCarry;

using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="TravoisBigCarryItem"/>, the travois.</summary>
/// <remarks>
/// The game's own movement and decay helpers look inlined (nothing calls them in IL2CPP), so the settings are applied
/// at the travois's entry points instead.
/// </remarks>
internal static class TravoisBigCarryItemPatches
{
    /// <summary>
    /// Lets the player interact with anything while carrying the travois, except entering a vehicle or snow shelter,
    /// when 'Cheat' Tweaks and <see cref="Settings.OverrideTravoisInteractionRestrictions"/> are on.
    /// </summary>
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.CanPerformInteractionWhileCarrying))]
    private static class OverrideInteractionRestrictionsWhileCarrying
    {
        private static void Postfix(ref bool __result)
        {
            if (!Settings.Instance.CheatingTweaks || !Settings.Instance.OverrideTravoisInteractionRestrictions)
                return;

            __result = !GameManager.GetPlayerInVehicle().IsEntering() &&
                       !GameManager.GetSnowShelterManager().PlayerEnteringShelter();
        }
    }

    /// <summary>Applies the travois settings every frame: see <see cref="Travois"/>.</summary>
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.UpdateItem))]
    private static class ApplyTweaksOnUpdate
    {
        private static void Prefix(TravoisBigCarryItem __instance) => Travois.Apply(__instance);
    }

    /// <summary>Applies the travois settings as a travois loads.</summary>
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.DeserializePostSceneLoad))]
    private static class ApplyTweaksOnLoad
    {
        private static void Prefix(TravoisBigCarryItem __instance) => Travois.Apply(__instance);
    }

    /// <summary>Applies the travois settings when the player picks a travois up.</summary>
    [HarmonyPatch(typeof(TravoisBigCarryItem), nameof(TravoisBigCarryItem.OnCarried))]
    private static class ApplyTweaksOnCarried
    {
        private static void Postfix(TravoisBigCarryItem __instance) => Travois.Apply(__instance);
    }
}

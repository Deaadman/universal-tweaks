using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="PlayerManager"/>, which handles the player's gear, inspecting and placing.</summary>
internal static class PlayerManagerPatches
{
    /// <summary>
    /// Works out the calorie burn with the burn from carried weight scaled back to the game's own carry capacity, so
    /// <see cref="Settings.AdditionalEncumbermentWeight"/> doesn't make each kilogram burn less. Without extra carry
    /// weight the game's result stands.
    /// </summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.CalculateModifiedCalorieBurnRate), [typeof(float)])]
    private static class CalorieBurnRateFix
    {
        private static void Postfix(PlayerManager __instance, float baseBurnRate, ref float __result)
        {
            if (Encumbrance.AdditionalWeight <= 0f)
                return;

            __result = Encumbrance.CalorieBurnRate(__instance, baseBurnRate);
        }
    }

    /// <summary>Notes when the player is placing a spray paint decal, for <see cref="SprayPaint.PlacingDecal"/>.</summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.OnCompletedDecalPlaceDown))]
    private static class TrackDecalPlacement
    {
        private static void Prefix() => SprayPaint.PlacingDecal = true;

        private static void Finalizer() => SprayPaint.PlacingDecal = false;
    }

    /// <summary>Shows a "Set" prompt when inspecting a snare with a rabbit in it.</summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
    private static class ShowSetSnarePrompt
    {
        private static void Postfix(PlayerManager __instance, Panel_HUD hud)
        {
            if (!HasCaughtRabbit(__instance.m_Gear))
                return;

            hud.m_InspectMode_Equip.gameObject.SetActive(true);
            hud.m_InspectMode_Equip.text = Localization.Get("GAMEPLAY_SetSnare");

            // The prompt grid was already laid out without the equip button, so it would overlap "Take It".
            hud.m_InspectMode_ButtonLayout.Reposition();
        }
    }

    /// <summary>Resets a snare with a rabbit in it when the player presses equip while inspecting it.</summary>
    /// <remarks>Runs before the game's own handling, so the press resets the snare instead of reaching it.</remarks>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
    private static class SetSnareFromInspect
    {
        private static bool Prefix(PlayerManager __instance)
        {
            var gear = __instance.m_Gear;
            if (!HasCaughtRabbit(gear) || !InputManager.GetEquipPressed(__instance))
                return true;

            var snare = gear.m_SnareItem;

            // Leaving inspection restores the snare's pre-inspect state, which would undo a plain SetState.
            snare.m_RestoreStateWhenLeaveInspection = SnareState.Set;
            __instance.ExitInspectGearMode(false);
            snare.SetState(SnareState.Set);

            Mod.Logger.Log($"Reset snare '{gear.name}' from the inspect menu.", FlaggedLoggingLevel.Debug);
            return false;
        }
    }

    /// <summary>Whether an item is a snare with a rabbit in it.</summary>
    /// <remarks>The game may swap the live state for a display state while inspecting, so the restore state is checked too.</remarks>
    /// <param name="gear">The item being inspected, if any.</param>
    /// <returns><see langword="true"/> for a snare with a rabbit.</returns>
    private static bool HasCaughtRabbit([NotNullWhen(true)] GearItem? gear) =>
        gear != null && gear.m_SnareItem != null &&
        (gear.m_SnareItem.m_State == SnareState.WithRabbit ||
         gear.m_SnareItem.m_RestoreStateWhenLeaveInspection == SnareState.WithRabbit);
}

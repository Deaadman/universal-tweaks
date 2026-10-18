namespace UniversalTweaks.Tweaks;

internal static class SnareTweaks
{
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
    private static class ShowSetSnarePrompt
    {
        private static void Postfix(PlayerManager __instance, Panel_HUD hud)
        {
            if (!HasCaughtRabbit(__instance.m_Gear))
            {
                return;
            }

            hud.m_InspectMode_Equip.gameObject.SetActive(true);
            hud.m_InspectMode_Equip.text = Localization.Get("GAMEPLAY_SetSnare");

            // The prompt grid was already laid out without the equip button, so it would overlap "Take It".
            hud.m_InspectMode_ButtonLayout.Reposition();
        }
    }

    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
    private static class SetSnareFromInspect
    {
        // Runs before the game's own handling so the equip press resets the snare instead of reaching vanilla.
        private static bool Prefix(PlayerManager __instance)
        {
            var gear = __instance.m_Gear;
            if (!HasCaughtRabbit(gear) || !InputManager.GetEquipPressed(__instance))
            {
                return true;
            }

            var snare = gear.m_SnareItem;

            // Leaving inspection restores the snare's pre-inspect state, which would undo a plain SetState.
            snare.m_RestoreStateWhenLeaveInspection = SnareState.Set;
            __instance.ExitInspectGearMode(false);
            snare.SetState(SnareState.Set);

            Mod.Logger.Log($"Reset snare '{gear.name}' from the inspect menu.", FlaggedLoggingLevel.Debug);
            return false;
        }
    }

    // The game may swap the live state for a display state while inspecting, so the restore state is checked too.
    private static bool HasCaughtRabbit([NotNullWhen(true)] GearItem? gear) =>
        gear != null && gear.m_SnareItem != null &&
        (gear.m_SnareItem.m_State == SnareState.WithRabbit ||
         gear.m_SnareItem.m_RestoreStateWhenLeaveInspection == SnareState.WithRabbit);
}
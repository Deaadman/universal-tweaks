//namespace UniversalTweaks.Tweaks;
//
//internal static class Snare
//{
//    private static SnareItem? _snareItem;
//
//    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
//    private static class SnareItemInspectEquipPrompt
//    {
//        private static void Postfix(PlayerManager __instance)
//        {
//            _snareItem = __instance.m_Gear.m_SnareItem;
//
//            if (_snareItem != null && _snareItem.m_State == SnareState.WithRabbit)
//            {
//                InterfaceManager.TryGetPanel<Panel_HUD>(out var hudPanel);
//                var inspectModeUILabel = hudPanel.m_InspectMode_Equip;
//                inspectModeUILabel.gameObject.SetActive(true);
//                inspectModeUILabel.text = Localization.Get("GAMEPLAY_SetSnare");
//            }
//        }
//    }
//
//    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UpdateInspectGear))]
//    private static class SnareItemInspectEquipFunctionality
//    {
//        // TODO: Seems that this is no longer setting the snare.
//        // Add logging to see what is happening.
//        private static void Postfix(PlayerManager __instance)
//        {
//            if (_snareItem == null || _snareItem.m_State != SnareState.WithRabbit)
//            {
//                return;
//            }
//
//            if (!InputManager.GetEquipPressed(__instance))
//            {
//                return;
//            }
//
//            __instance.ExitInspectGearMode(false);
//            _snareItem.SetState(SnareState.Set);
//        }
//    }
//}
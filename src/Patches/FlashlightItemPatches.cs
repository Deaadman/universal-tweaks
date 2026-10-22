using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="FlashlightItem"/>, the flashlight and miner's flashlight.</summary>
internal static class FlashlightItemPatches
{
    /// <summary>Gives each flashlight a random battery charge when <see cref="Settings.BatteryRandomization"/> is on.</summary>
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Awake))]
    private static class FlashlightCustomization
    {
        private static void Prefix(FlashlightItem __instance)
        {
            if (!Settings.Instance.BatteryRandomization)
                return;

            __instance.m_CurrentBatteryCharge = UnityEngine.Random.Range(0f, 1f);
            Mod.Logger.Log($"Randomized flashlight battery charge to {__instance.m_CurrentBatteryCharge:0.##}.", FlaggedLoggingLevel.Debug);
        }
    }

    /// <summary>
    /// Turns the lights on and off outside auroras too, and keeps a dropped flashlight lit, when
    /// <see cref="Settings.ExtendedFunctionality"/> is on: see <see cref="Flashlights.EnableLights"/>.
    /// </summary>
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.EnableLights))]
    private static class FlashlightBeamDropped
    {
        private static bool Prefix(FlashlightItem __instance, FlashlightItem.State state)
        {
            if (!Settings.Instance.ExtendedFunctionality)
                return true;

            Flashlights.EnableLights(__instance, state);
            return false;
        }
    }

    /// <summary>
    /// Reports the battery's real charge outside auroras when <see cref="Settings.ExtendedFunctionality"/> is on, instead
    /// of the game's empty one.
    /// </summary>
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.GetNormalizedCharge))]
    private static class FlashlightKeepBatteryCharge
    {
        private static bool Prefix(FlashlightItem __instance, ref float __result)
        {
            if (!Settings.Instance.ExtendedFunctionality)
                return true;

            __result = __instance.m_CurrentBatteryCharge;
            return false;
        }
    }

    /// <summary>
    /// Lets the flashlight light up outside auroras when <see cref="Settings.ExtendedFunctionality"/> is on, except the
    /// high beam when <see cref="Settings.HighBeamRestrictions"/> is on too, which fails with the game's message.
    /// </summary>
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.IsLit))]
    private static class FlashlightFunctionality
    {
        /// <summary>
        /// The flashlights whose high beam failure has been announced. <c>IsLit</c> is called several times a frame, so
        /// the failure is only announced once each time the high beam is switched on.
        /// </summary>
        private static readonly ObjectStateCache<FlashlightItem, bool> HighBeamFailureAnnounced = new();

        private static bool Prefix(FlashlightItem __instance, ref bool __result)
        {
            if (Settings.Instance.ExtendedFunctionality && Settings.Instance.HighBeamRestrictions &&
                __instance.m_State == FlashlightItem.State.High && !GameManager.GetAuroraManager().AuroraIsActive())
            {
                if (HighBeamFailureAnnounced.TryAdd(__instance, true))
                {
                    GameAudioManager.PlayGUIError();
                    HUDMessage.AddMessage(Localization.Get("GAMEPLAY_StateHighFail"));
                }

                __result = __instance.IsOn();
                return true;
            }

            HighBeamFailureAnnounced.Remove(__instance, out _);

            __result = __instance.IsOn();
            return false;
        }
    }

    /// <summary>Drains the battery outside auroras and applies the flashlight settings every frame: see <see cref="Flashlights.Update"/>.</summary>
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Update))]
    private static class FlashlightBatteryDrain
    {
        private static void Postfix(FlashlightItem __instance) => Flashlights.Update(__instance);
    }
}

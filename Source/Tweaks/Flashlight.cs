using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class Flashlight
{
    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Awake))]
    private static class FlashlightCustomization
    {
        private static void Prefix(FlashlightItem __instance)
        {
            if (Settings.Instance.BatteryRandomization)
            {
                __instance.m_CurrentBatteryCharge = UnityEngine.Random.Range(0f, 1f);
                Mod.Logger.Log($"Randomized flashlight battery charge to {__instance.m_CurrentBatteryCharge:0.##}.", FlaggedLoggingLevel.Debug);
            }
        }
    }

    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.EnableLights))]
    private static class FlashlightBeamDropped
    {
        private static bool Prefix(FlashlightItem __instance, FlashlightItem.State state)
        {
            if (!Settings.Instance.ExtendedFunctionality)
            {
                return true;
            }

            __instance.m_AuroraField.Enable(state == FlashlightItem.State.High);
            if (__instance.m_GearItem == GameManager.GetPlayerManagerComponent().m_ItemInHands)
            {
                state = FlashlightItem.State.Off;
            }

            __instance.m_FxObjectLow.SetActive(state == FlashlightItem.State.Low);
            __instance.m_FxObjectHigh.SetActive(state == FlashlightItem.State.High);

            var useOutdoorLighting = GameManager.GetWeatherComponent().UseOutdoorLightingForLightSources();

            __instance.m_LightIndoor.enabled =
                !useOutdoorLighting && state is FlashlightItem.State.Low or FlashlightItem.State.High;
            __instance.m_LightIndoorHigh.enabled = !useOutdoorLighting && state == FlashlightItem.State.High;

            __instance.m_LightOutdoor.enabled =
                useOutdoorLighting && state is FlashlightItem.State.Low or FlashlightItem.State.High;
            __instance.m_LightOutdoorHigh.enabled = useOutdoorLighting && state == FlashlightItem.State.High;

            Mod.Logger.Log($"Flashlight beam state changed to {state}.", FlaggedLoggingLevel.Trace);

            return false;
        }
    }

    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.GetNormalizedCharge))]
    private static class FlashlightKeepBatteryCharge
    {
        private static bool Prefix(FlashlightItem __instance, ref float __result)
        {
            if (!Settings.Instance.ExtendedFunctionality)
            {
                return true;
            }

            __result = __instance.m_CurrentBatteryCharge;
            return false;
        }
    }

    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.IsLit))]
    private static class FlashlightFunctionality
    {
        private static bool Prefix(FlashlightItem __instance, ref bool __result)
        {
            if (Settings.Instance.HighBeamRestrictions && __instance.m_State == FlashlightItem.State.High &&
                !GameManager.GetAuroraManager().AuroraIsActive())
            {
                GameAudioManager.PlayGUIError();
                HUDMessage.AddMessage(Localization.Get("GAMEPLAY_StateHighFail"));
                __result = __instance.IsOn();
                return true;
            }

            __result = __instance.IsOn();
            return false;
        }
    }

    [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Update))]
    private static class FlashlightBatteryDrain
    {
        private static void Postfix(FlashlightItem __instance)
        {
            var todHours = GameManager.GetTimeOfDayComponent().GetTODHours(Time.deltaTime);

            if (!GameManager.GetAuroraManager().AuroraIsActive())
            {
                if (__instance.m_State == FlashlightItem.State.Low)
                {
                    __instance.m_CurrentBatteryCharge -= todHours / __instance.m_LowBeamDuration;
                }
                else if (!Settings.Instance.HighBeamRestrictions && __instance.m_State == FlashlightItem.State.High)
                {
                    __instance.m_CurrentBatteryCharge -= todHours / __instance.m_HighBeamDuration;
                }

                if (__instance.m_CurrentBatteryCharge <= 0f)
                {
                    __instance.m_CurrentBatteryCharge = 0f;
                    __instance.m_State = FlashlightItem.State.Off;
                }
            }

            if (Settings.Instance.InfiniteBattery)
            {
                __instance.m_CurrentBatteryCharge = 1f;
            }

            var isMinersFlashlight = __instance.m_GearItem != null &&
                                     __instance.m_GearItem.name == "GEAR_Flashlight_LongLasting";
            __instance.m_LowBeamDuration = Settings.Instance.CheatingTweaks
                ?
                isMinersFlashlight
                    ? Settings.Instance.MinersFlashlightLowBeamDuration
                    : Settings.Instance.FlashlightLowBeamDuration
                : isMinersFlashlight
                    ? 1.5f
                    : 1f;
            __instance.m_HighBeamDuration = Settings.Instance.CheatingTweaks
                ? isMinersFlashlight
                    ? Settings.Instance.MinersFlashlightHighBeamDuration
                    : Settings.Instance.FlashlightHighBeamDuration
                : 0.08333334f;
            __instance.m_RechargeTime = Settings.Instance.CheatingTweaks
                ?
                isMinersFlashlight
                    ? Settings.Instance.MinersFlashlightRechargeTime
                    : Settings.Instance.FlashlightRechargeTime
                : isMinersFlashlight
                    ? 1.75f
                    : 2f;

            UpdateFlashlightBeamColor(__instance, __instance.m_FxObjectLow, __instance.m_FxObjectHigh);
        }
    }

    [HarmonyPatch(typeof(FirstPersonFlashlight), nameof(FirstPersonFlashlight.Update))]
    private static class ModifyFlashFlashlightBeamColor
    {
        private static void Postfix(FirstPersonFlashlight __instance)
        {
            var itemInHands = GameManager.GetPlayerManagerComponent().m_ItemInHands;
            if (itemInHands == null) return;
            var flashlightItem = itemInHands.m_FlashlightItem;

            UpdateFlashlightBeamColor(flashlightItem, __instance.m_LowFxGameObject, __instance.m_HighFxGameObject);
        }
    }

    [HarmonyPatch(typeof(LightRandomIntensity), nameof(LightRandomIntensity.Update))]
    private static class FlashlightFlicker
    {
        private static bool Prefix(LightRandomIntensity __instance)
        {
            if (!Settings.Instance.AuroraFlickering)
            {
                return true;
            }

            if (__instance.gameObject.name is "LightIndoors" or "LightOutdoors" or "LightExtend" &&
                !GameManager.GetAuroraManager().AuroraIsActive() && Settings.Instance.ExtendedFunctionality)
            {
                return false;
            }

            return true;
        }
    }

    private static Color GetColorFromFlashlight(FlashlightItem flashlightItem)
    {
        var isMinersFlashlight = flashlightItem.m_GearItem?.name is "GEAR_Flashlight_LongLasting";
        return GetColorFromSettings(
            isMinersFlashlight ? Settings.Instance.MinersFlashlightBeamColor : Settings.Instance.FlashlightBeamColor,
            isMinersFlashlight);
    }

    private static Color GetColorFromSettings(FlashlightBeamColor beamColor, bool isMinersFlashlight)
    {
        return beamColor switch
        {
            FlashlightBeamColor.Custom => new Color(
                (isMinersFlashlight
                    ? Settings.Instance.MinersFlashlightRedValue
                    : Settings.Instance.FlashlightRedValue) / 255f,
                (isMinersFlashlight
                    ? Settings.Instance.MinersFlashlightGreenValue
                    : Settings.Instance.FlashlightGreenValue) / 255f,
                (isMinersFlashlight
                    ? Settings.Instance.MinersFlashlightBlueValue
                    : Settings.Instance.FlashlightBlueValue) / 255f),
            FlashlightBeamColor.Default => new Color(0.7215686f, 0.8117647f, 0.9176471f),
            FlashlightBeamColor.Red => Color.red,
            FlashlightBeamColor.Green => Color.green,
            FlashlightBeamColor.Blue => Color.blue,
            FlashlightBeamColor.Yellow => Color.yellow,
            FlashlightBeamColor.Purple => new Color(0.5f, 0f, 0.5f),
            FlashlightBeamColor.Orange => new Color(1f, 0.6470588f, 0f),
            FlashlightBeamColor.White => Color.white,
            _ => new Color(0.7215686f, 0.8117647f, 0.9176471f),
        };
    }

    private static void UpdateFlashlightBeamColor(FlashlightItem flashlightItem, GameObject lowFxGameObject,
        GameObject highFxGameObject)
    {
        if (flashlightItem == null)
        {
            return;
        }

        var newColor = GetColorFromFlashlight(flashlightItem);

        if (lowFxGameObject != null)
        {
            UpdateLightColorRecursive(lowFxGameObject.transform, newColor);
        }

        if (highFxGameObject != null)
        {
            UpdateLightColorRecursive(highFxGameObject.transform, newColor);
        }
    }

    private static void UpdateLightColorRecursive(Transform transform, Color color)
    {
        if (transform == null)
            return;

        Light childLight = transform.GetComponent<Light>();

        if (childLight == null)
            return;

        childLight.color = color;
        for (var i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != null) UpdateLightColorRecursive(transform.GetChild(i), color);
        }
    }
}
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

            Mod.Logger.Log($"Flashlight world light state set to {state} (actual beam state: {__instance.m_State}).", FlaggedLoggingLevel.Trace);

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

            var (lowBeamIntensity, lowBeamRange, highBeamIntensity, highBeamRange) = GetBeamLightSettings(isMinersFlashlight);

            __instance.m_LightIndoor.intensity = lowBeamIntensity;
            __instance.m_LightOutdoor.intensity = lowBeamIntensity;
            __instance.m_LightIndoor.range = lowBeamRange;
            __instance.m_LightOutdoor.range = lowBeamRange;

            __instance.m_LightIndoorHigh.intensity = highBeamIntensity;
            __instance.m_LightOutdoorHigh.intensity = highBeamIntensity;
            __instance.m_LightIndoorHigh.range = highBeamRange;
            __instance.m_LightOutdoorHigh.range = highBeamRange;

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
            if (flashlightItem == null) return;

            UpdateFlashlightBeamColor(flashlightItem, __instance.m_LowFxGameObject, __instance.m_HighFxGameObject);

            // These are the lights the player actually sees in first person - FlashlightItem's own
            // m_LightIndoor/m_LightOutdoor pair only drives the world-space representation of the item.
            var isMinersFlashlight = flashlightItem.m_GearItem?.name is "GEAR_Flashlight_LongLasting";
            var (lowBeamIntensity, lowBeamRange, highBeamIntensity, highBeamRange) = GetBeamLightSettings(isMinersFlashlight);

            __instance.m_LightIndoorsLow.intensity = lowBeamIntensity;
            __instance.m_LightIndoorsLow.range = lowBeamRange;
            __instance.m_LightOutdoorsLow.intensity = lowBeamIntensity;
            __instance.m_LightOutdoorsLow.range = lowBeamRange;

            if (__instance.m_LightIndoorsHigh != null)
            {
                __instance.m_LightIndoorsHigh.intensity = highBeamIntensity;
                __instance.m_LightIndoorsHigh.range = highBeamRange;
            }

            if (__instance.m_LightOutdoorsHigh != null)
            {
                __instance.m_LightOutdoorsHigh.intensity = highBeamIntensity;
                __instance.m_LightOutdoorsHigh.range = highBeamRange;
            }
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

    private static (float lowIntensity, float lowRange, float highIntensity, float highRange) GetBeamLightSettings(
        bool isMinersFlashlight)
    {
        var lowIntensity = Settings.Instance.CheatingTweaks
            ? isMinersFlashlight
                ? Settings.Instance.MinersFlashlightLowBeamIntensity
                : Settings.Instance.FlashlightLowBeamIntensity
            : 1f;
        var lowRange = Settings.Instance.CheatingTweaks
            ? isMinersFlashlight
                ? Settings.Instance.MinersFlashlightLowBeamRange
                : Settings.Instance.FlashlightLowBeamRange
            : 40f;
        var highIntensity = Settings.Instance.CheatingTweaks
            ? isMinersFlashlight
                ? Settings.Instance.MinersFlashlightHighBeamIntensity
                : Settings.Instance.FlashlightHighBeamIntensity
            : 7.86f;
        var highRange = Settings.Instance.CheatingTweaks
            ? isMinersFlashlight
                ? Settings.Instance.MinersFlashlightHighBeamRange
                : Settings.Instance.FlashlightHighBeamRange
            : 80f;

        return (lowIntensity, lowRange, highIntensity, highRange);
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

        // The FX root itself usually holds no Light (it's a container for the light + particle/flare
        // children), so this must keep recursing regardless - returning early here silently no-ops every color change.
        var childLight = transform.GetComponent<Light>();
        if (childLight != null)
        {
            childLight.color = color;
        }

        for (var i = 0; i < transform.childCount; i++)
        {
            UpdateLightColorRecursive(transform.GetChild(i), color);
        }
    }
}
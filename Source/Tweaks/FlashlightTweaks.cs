using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class FlashlightTweaks
{
    private static readonly Color DefaultBeamColor = new(0.7215686f, 0.8117647f, 0.9176471f);

    // Last colour applied per FX root, so the light hierarchy is only walked when the colour actually changes.
    private static readonly Dictionary<int, Color> AppliedBeamColors = new();

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
        // IsLit is polled several times a frame, so the failure is only announced once per switch to high beam.
        private static readonly HashSet<int> HighBeamFailureAnnounced = new();

        private static bool Prefix(FlashlightItem __instance, ref bool __result)
        {
            var id = __instance.GetInstanceID();

            if (Settings.Instance.ExtendedFunctionality && Settings.Instance.HighBeamRestrictions &&
                __instance.m_State == FlashlightItem.State.High && !GameManager.GetAuroraManager().AuroraIsActive())
            {
                if (HighBeamFailureAnnounced.Add(id))
                {
                    GameAudioManager.PlayGUIError();
                    HUDMessage.AddMessage(Localization.Get("GAMEPLAY_StateHighFail"));
                }

                __result = __instance.IsOn();
                return true;
            }

            HighBeamFailureAnnounced.Remove(id);

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
                else if (!(Settings.Instance.ExtendedFunctionality && Settings.Instance.HighBeamRestrictions) &&
                         __instance.m_State == FlashlightItem.State.High)
                {
                    __instance.m_CurrentBatteryCharge -= todHours / __instance.m_HighBeamDuration;
                }

                if (__instance.m_CurrentBatteryCharge <= 0f)
                {
                    __instance.m_CurrentBatteryCharge = 0f;
                    __instance.m_State = FlashlightItem.State.Off;
                }
            }

            if (Settings.Instance.CheatingTweaks && Settings.Instance.InfiniteBattery)
            {
                __instance.m_CurrentBatteryCharge = 1f;
            }

            var isMinersFlashlight = IsMinersFlashlight(__instance);
            var tuning = GetTuning(isMinersFlashlight);

            __instance.m_LowBeamDuration = tuning.LowBeamDuration;
            __instance.m_HighBeamDuration = tuning.HighBeamDuration;
            __instance.m_RechargeTime = tuning.RechargeTime;

            ApplyBeamLight(__instance.m_LightIndoor, tuning.LowBeamIntensity, tuning.LowBeamRange);
            ApplyBeamLight(__instance.m_LightOutdoor, tuning.LowBeamIntensity, tuning.LowBeamRange);
            ApplyBeamLight(__instance.m_LightIndoorHigh, tuning.HighBeamIntensity, tuning.HighBeamRange);
            ApplyBeamLight(__instance.m_LightOutdoorHigh, tuning.HighBeamIntensity, tuning.HighBeamRange);

            ApplyBeamColor(GetBeamColor(isMinersFlashlight), __instance.m_FxObjectLow, __instance.m_FxObjectHigh);
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

            var isMinersFlashlight = IsMinersFlashlight(flashlightItem);
            ApplyBeamColor(GetBeamColor(isMinersFlashlight), __instance.m_LowFxGameObject, __instance.m_HighFxGameObject);

            // These are the lights the player actually sees in first person - FlashlightItem's own
            // m_LightIndoor/m_LightOutdoor pair only drives the world-space representation of the item.
            var tuning = GetTuning(isMinersFlashlight);

            ApplyBeamLight(__instance.m_LightIndoorsLow, tuning.LowBeamIntensity, tuning.LowBeamRange);
            ApplyBeamLight(__instance.m_LightOutdoorsLow, tuning.LowBeamIntensity, tuning.LowBeamRange);
            ApplyBeamLight(__instance.m_LightIndoorsHigh, tuning.HighBeamIntensity, tuning.HighBeamRange);
            ApplyBeamLight(__instance.m_LightOutdoorsHigh, tuning.HighBeamIntensity, tuning.HighBeamRange);
        }
    }

    [HarmonyPatch(typeof(LightRandomIntensity), nameof(LightRandomIntensity.Update))]
    private static class FlashlightFlicker
    {
        private static bool Prefix(LightRandomIntensity __instance)
        {
            // Runs for every flickering light in the scene, so the name (a marshalled string) is read last.
            if (!Settings.Instance.AuroraFlickering || !Settings.Instance.ExtendedFunctionality ||
                GameManager.GetAuroraManager().AuroraIsActive())
            {
                return true;
            }

            return __instance.gameObject.name is not ("LightIndoors" or "LightOutdoors" or "LightExtend");
        }
    }

    private static bool IsMinersFlashlight(FlashlightItem flashlightItem) =>
        flashlightItem.m_GearItem != null && flashlightItem.m_GearItem.name == "GEAR_Flashlight_LongLasting";

    private static FlashlightTuning GetTuning(bool isMinersFlashlight)
    {
        var settings = Settings.Instance;

        if (!settings.CheatingTweaks)
        {
            return isMinersFlashlight ? VanillaDefaults.MinersFlashlight : VanillaDefaults.Flashlight;
        }

        return isMinersFlashlight
            ? new FlashlightTuning(
                settings.MinersFlashlightLowBeamDuration,
                settings.MinersFlashlightHighBeamDuration,
                settings.MinersFlashlightRechargeTime,
                settings.MinersFlashlightLowBeamIntensity,
                settings.MinersFlashlightLowBeamRange,
                settings.MinersFlashlightHighBeamIntensity,
                settings.MinersFlashlightHighBeamRange)
            : new FlashlightTuning(
                settings.FlashlightLowBeamDuration,
                settings.FlashlightHighBeamDuration,
                settings.FlashlightRechargeTime,
                settings.FlashlightLowBeamIntensity,
                settings.FlashlightLowBeamRange,
                settings.FlashlightHighBeamIntensity,
                settings.FlashlightHighBeamRange);
    }

    private static Color GetBeamColor(bool isMinersFlashlight)
    {
        var settings = Settings.Instance;

        return isMinersFlashlight
            ? ToColor(settings.MinersFlashlightBeamColor, settings.MinersFlashlightRedValue,
                settings.MinersFlashlightGreenValue, settings.MinersFlashlightBlueValue)
            : ToColor(settings.FlashlightBeamColor, settings.FlashlightRedValue,
                settings.FlashlightGreenValue, settings.FlashlightBlueValue);
    }

    private static Color ToColor(FlashlightBeamColor beamColor, int red, int green, int blue)
    {
        return beamColor switch
        {
            FlashlightBeamColor.Custom => new Color(red / 255f, green / 255f, blue / 255f),
            FlashlightBeamColor.Red => Color.red,
            FlashlightBeamColor.Green => Color.green,
            FlashlightBeamColor.Blue => Color.blue,
            FlashlightBeamColor.Yellow => Color.yellow,
            FlashlightBeamColor.Purple => new Color(0.5f, 0f, 0.5f),
            FlashlightBeamColor.Orange => new Color(1f, 0.6470588f, 0f),
            FlashlightBeamColor.White => Color.white,
            _ => DefaultBeamColor,
        };
    }

    private static void ApplyBeamLight(Light? light, float intensity, float range)
    {
        if (light == null)
        {
            return;
        }

        light.intensity = intensity;
        light.range = range;
    }

    private static void ApplyBeamColor(Color color, GameObject? lowFxGameObject, GameObject? highFxGameObject)
    {
        ApplyBeamColor(color, lowFxGameObject);
        ApplyBeamColor(color, highFxGameObject);
    }

    private static void ApplyBeamColor(Color color, GameObject? fxGameObject)
    {
        if (fxGameObject == null)
        {
            return;
        }

        var id = fxGameObject.GetInstanceID();
        if (AppliedBeamColors.TryGetValue(id, out var appliedColor) && appliedColor == color)
        {
            return;
        }

        UpdateLightColorRecursive(fxGameObject.transform, color);
        AppliedBeamColors[id] = color;
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
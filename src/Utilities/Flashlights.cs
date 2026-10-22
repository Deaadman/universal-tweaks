using UniversalTweaks.Core;

namespace UniversalTweaks.Utilities;

/// <summary>
/// The flashlight tweaks: lights that work outside auroras, battery drain, beam durations, brightness, range and colour.
/// </summary>
/// <remarks>
/// The settings are applied every frame from <see cref="Patches.FlashlightItemPatches"/> and
/// <see cref="Patches.FirstPersonFlashlightPatches"/>, so changes reach flashlights that already exist.
/// </remarks>
internal static class Flashlights
{
    /// <summary>The miner's flashlight's gear item name.</summary>
    private const string MinersFlashlightGearName = "GEAR_Flashlight_LongLasting";

    /// <summary>The game's own pale blue beam colour.</summary>
    private static readonly Color DefaultBeamColor = new(0.7215686f, 0.8117647f, 0.9176471f);

    /// <summary>The colour last applied to each beam's FX root, so its lights are only walked when the colour changes.</summary>
    private static readonly ObjectStateCache<GameObject, Color> AppliedBeamColors = new();

    /// <summary>The game's own values for each beam light, captured before the first change so they can be put back.</summary>
    private static readonly ObjectStateCache<Light, BeamLightState> OriginalBeamLights = new();

    /// <summary>A beam light's own brightness and range, and the flicker script driving it, if any.</summary>
    /// <param name="Intensity">The light's base intensity.</param>
    /// <param name="Range">The light's range, in metres.</param>
    /// <param name="Flicker">The script that flickers the light, which rewrites its intensity every frame.</param>
    private sealed record BeamLightState(float Intensity, float Range, LightRandomIntensity? Flicker);

    /// <summary>Whether a flashlight is the miner's flashlight, which has its own settings.</summary>
    /// <param name="flashlightItem">The flashlight.</param>
    /// <returns><see langword="true"/> for the miner's flashlight.</returns>
    internal static bool IsMinersFlashlight(FlashlightItem flashlightItem) =>
        flashlightItem.m_GearItem != null && flashlightItem.m_GearItem.name == MinersFlashlightGearName;

    /// <summary>
    /// Turns a flashlight's lights on or off for a beam state, as the game does, but outside auroras too and with the
    /// beam lit when the flashlight isn't in the player's hands.
    /// </summary>
    /// <param name="flashlight">The flashlight.</param>
    /// <param name="state">The beam state to show.</param>
    internal static void EnableLights(FlashlightItem flashlight, FlashlightItem.State state)
    {
        flashlight.m_AuroraField.Enable(state == FlashlightItem.State.High);
        if (flashlight.m_GearItem == GameManager.GetPlayerManagerComponent().m_ItemInHands)
            state = FlashlightItem.State.Off;

        flashlight.m_FxObjectLow.SetActive(state == FlashlightItem.State.Low);
        flashlight.m_FxObjectHigh.SetActive(state == FlashlightItem.State.High);

        var useOutdoorLighting = GameManager.GetWeatherComponent().UseOutdoorLightingForLightSources();

        flashlight.m_LightIndoor.enabled =
            !useOutdoorLighting && state is FlashlightItem.State.Low or FlashlightItem.State.High;
        flashlight.m_LightIndoorHigh.enabled = !useOutdoorLighting && state == FlashlightItem.State.High;

        flashlight.m_LightOutdoor.enabled =
            useOutdoorLighting && state is FlashlightItem.State.Low or FlashlightItem.State.High;
        flashlight.m_LightOutdoorHigh.enabled = useOutdoorLighting && state == FlashlightItem.State.High;
    }

    /// <summary>
    /// Drains a flashlight's battery outside auroras, turns it off when it runs out, and applies the battery, beam and
    /// colour settings to it.
    /// </summary>
    /// <param name="flashlight">The flashlight, after the game's own update.</param>
    internal static void Update(FlashlightItem flashlight)
    {
        var todHours = GameManager.GetTimeOfDayComponent().GetTODHours(Time.deltaTime);

        if (!GameManager.GetAuroraManager().AuroraIsActive())
        {
            if (flashlight.m_State == FlashlightItem.State.Low)
            {
                flashlight.m_CurrentBatteryCharge -= todHours / flashlight.m_LowBeamDuration;
            }
            else if (!(Settings.Instance.ExtendedFunctionality && Settings.Instance.HighBeamRestrictions) &&
                     flashlight.m_State == FlashlightItem.State.High)
            {
                flashlight.m_CurrentBatteryCharge -= todHours / flashlight.m_HighBeamDuration;
            }

            if (flashlight.m_CurrentBatteryCharge <= 0f)
            {
                flashlight.m_CurrentBatteryCharge = 0f;
                if (flashlight.m_State != FlashlightItem.State.Off)
                {
                    flashlight.m_State = FlashlightItem.State.Off;
                    // Flipping the state alone leaves the beam lights enabled, lit wherever the item now is.
                    flashlight.EnableLights(FlashlightItem.State.Off);
                }
            }
        }

        if (Settings.Instance.CheatingTweaks && Settings.Instance.InfiniteBattery)
            flashlight.m_CurrentBatteryCharge = 1f;

        var isMinersFlashlight = IsMinersFlashlight(flashlight);
        var tuning = GetTuning(isMinersFlashlight);

        flashlight.m_LowBeamDuration = tuning.LowBeamDuration;
        flashlight.m_HighBeamDuration = tuning.HighBeamDuration;
        flashlight.m_RechargeTime = tuning.RechargeTime;

        var beamLights = GetBeamLights(isMinersFlashlight);
        ApplyBeamLight(flashlight.m_LightIndoor, beamLights.LowIntensity, beamLights.LowRange);
        ApplyBeamLight(flashlight.m_LightOutdoor, beamLights.LowIntensity, beamLights.LowRange);
        ApplyBeamLight(flashlight.m_LightIndoorHigh, beamLights.HighIntensity, beamLights.HighRange);
        ApplyBeamLight(flashlight.m_LightOutdoorHigh, beamLights.HighIntensity, beamLights.HighRange);

        ApplyBeamColor(GetBeamColor(isMinersFlashlight), flashlight.m_FxObjectLow, flashlight.m_FxObjectHigh);
    }

    /// <summary>Applies the beam colour, brightness and range settings to the flashlight in the player's hands.</summary>
    /// <param name="firstPerson">The first-person flashlight model.</param>
    internal static void UpdateFirstPerson(FirstPersonFlashlight firstPerson)
    {
        var itemInHands = GameManager.GetPlayerManagerComponent().m_ItemInHands;
        if (itemInHands == null)
            return;

        var flashlightItem = itemInHands.m_FlashlightItem;
        if (flashlightItem == null)
            return;

        var isMinersFlashlight = IsMinersFlashlight(flashlightItem);
        ApplyBeamColor(GetBeamColor(isMinersFlashlight), firstPerson.m_LowFxGameObject, firstPerson.m_HighFxGameObject);

        // These are the lights the player sees in first person: FlashlightItem's own lights only light the world model.
        var beamLights = GetBeamLights(isMinersFlashlight);

        ApplyBeamLight(firstPerson.m_LightIndoorsLow, beamLights.LowIntensity, beamLights.LowRange);
        ApplyBeamLight(firstPerson.m_LightOutdoorsLow, beamLights.LowIntensity, beamLights.LowRange);
        ApplyBeamLight(firstPerson.m_LightIndoorsHigh, beamLights.HighIntensity, beamLights.HighRange);
        ApplyBeamLight(firstPerson.m_LightOutdoorsHigh, beamLights.HighIntensity, beamLights.HighRange);
    }

    /// <summary>Gets a flashlight's battery durations and recharge time: the settings with 'Cheat' Tweaks on, else the game's.</summary>
    /// <param name="isMinersFlashlight">Whether it's the miner's flashlight.</param>
    /// <returns>The durations and recharge time.</returns>
    private static FlashlightTuning GetTuning(bool isMinersFlashlight)
    {
        var settings = Settings.Instance;

        if (!settings.CheatingTweaks)
            return isMinersFlashlight ? VanillaDefaults.MinersFlashlight : VanillaDefaults.Flashlight;

        return isMinersFlashlight
            ? new FlashlightTuning(
                settings.MinersFlashlightLowBeamDuration,
                settings.MinersFlashlightHighBeamDuration,
                settings.MinersFlashlightRechargeTime)
            : new FlashlightTuning(
                settings.FlashlightLowBeamDuration,
                settings.FlashlightHighBeamDuration,
                settings.FlashlightRechargeTime);
    }

    /// <summary>Gets a flashlight's beam brightness and range from the settings.</summary>
    /// <param name="isMinersFlashlight">Whether it's the miner's flashlight.</param>
    /// <returns>The brightness and range of each beam.</returns>
    private static BeamLightTuning GetBeamLights(bool isMinersFlashlight)
    {
        var settings = Settings.Instance;

        return isMinersFlashlight
            ? new BeamLightTuning(
                settings.MinersFlashlightLowBeamIntensity,
                settings.MinersFlashlightLowBeamRange,
                settings.MinersFlashlightHighBeamIntensity,
                settings.MinersFlashlightHighBeamRange)
            : new BeamLightTuning(
                settings.FlashlightLowBeamIntensity,
                settings.FlashlightLowBeamRange,
                settings.FlashlightHighBeamIntensity,
                settings.FlashlightHighBeamRange);
    }

    /// <summary>Gets a flashlight's beam colour from the settings.</summary>
    /// <param name="isMinersFlashlight">Whether it's the miner's flashlight.</param>
    /// <returns>The colour.</returns>
    private static Color GetBeamColor(bool isMinersFlashlight)
    {
        var settings = Settings.Instance;

        return isMinersFlashlight
            ? ToColor(settings.MinersFlashlightBeamColor, settings.MinersFlashlightRedValue,
                settings.MinersFlashlightGreenValue, settings.MinersFlashlightBlueValue)
            : ToColor(settings.FlashlightBeamColor, settings.FlashlightRedValue,
                settings.FlashlightGreenValue, settings.FlashlightBlueValue);
    }

    /// <summary>Turns a beam colour setting into a colour.</summary>
    /// <param name="beamColor">The colour setting.</param>
    /// <param name="red">The red value, 0 to 255, for <see cref="FlashlightBeamColor.Custom"/>.</param>
    /// <param name="green">The green value, 0 to 255, for <see cref="FlashlightBeamColor.Custom"/>.</param>
    /// <param name="blue">The blue value, 0 to 255, for <see cref="FlashlightBeamColor.Custom"/>.</param>
    /// <returns>The colour.</returns>
    private static Color ToColor(FlashlightBeamColor beamColor, int red, int green, int blue) =>
        beamColor switch
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

    /// <summary>
    /// Sets a beam light's brightness and range with 'Cheat' Tweaks on, or puts back the game's own when it's off.
    /// </summary>
    /// <param name="light">The beam light, if the flashlight has it.</param>
    /// <param name="intensity">The brightness.</param>
    /// <param name="range">The range, in metres.</param>
    private static void ApplyBeamLight(Light? light, float intensity, float range)
    {
        if (light == null)
            return;

        if (!Settings.Instance.CheatingTweaks)
        {
            if (OriginalBeamLights.Remove(light, out var original))
            {
                SetBeamIntensity(light, original.Flicker, original.Intensity);
                light.range = original.Range;
            }

            return;
        }

        if (!OriginalBeamLights.TryGet(light, out var state))
        {
            var flicker = light.GetComponent<LightRandomIntensity>();
            var baseIntensity = flicker != null && flicker.m_LightIntensityStart > 0f
                ? flicker.m_LightIntensityStart
                : light.intensity;

            state = new BeamLightState(baseIntensity, light.range, flicker);
            OriginalBeamLights.Set(light, state);
        }

        SetBeamIntensity(light, state.Flicker, intensity);
        light.range = range;
    }

    /// <summary>Sets a beam light's brightness, through its flicker script if it has an active one.</summary>
    /// <remarks>The flicker script rewrites the intensity every frame from its base value, so that base is what has to change.</remarks>
    /// <param name="light">The beam light.</param>
    /// <param name="flicker">The light's flicker script, if any.</param>
    /// <param name="intensity">The brightness.</param>
    private static void SetBeamIntensity(Light light, LightRandomIntensity? flicker, float intensity)
    {
        if (flicker != null)
        {
            flicker.m_LightIntensityStart = intensity;
            if (flicker.enabled)
                return;
        }

        light.intensity = intensity;
    }

    /// <summary>Colours the lights under a flashlight's low and high beam effects.</summary>
    /// <param name="color">The colour.</param>
    /// <param name="lowFxGameObject">The low beam's effect, if any.</param>
    /// <param name="highFxGameObject">The high beam's effect, if any.</param>
    private static void ApplyBeamColor(Color color, GameObject? lowFxGameObject, GameObject? highFxGameObject)
    {
        ApplyBeamColor(color, lowFxGameObject);
        ApplyBeamColor(color, highFxGameObject);
    }

    /// <summary>Colours the lights under a beam effect, unless they already have that colour.</summary>
    /// <param name="color">The colour.</param>
    /// <param name="fxGameObject">The beam's effect, if any.</param>
    private static void ApplyBeamColor(Color color, GameObject? fxGameObject)
    {
        if (fxGameObject == null)
            return;

        if (AppliedBeamColors.TryGet(fxGameObject, out var appliedColor) && appliedColor == color)
            return;

        UpdateLightColorRecursive(fxGameObject.transform, color);
        AppliedBeamColors.Set(fxGameObject, color);
    }

    /// <summary>Colours every light on a transform and all its children.</summary>
    /// <param name="transform">The transform.</param>
    /// <param name="color">The colour.</param>
    private static void UpdateLightColorRecursive(Transform transform, Color color)
    {
        if (transform == null)
            return;

        // The FX root usually has no Light of its own, only children that do, so this keeps going either way.
        var childLight = transform.GetComponent<Light>();
        if (childLight != null)
            childLight.color = color;

        for (var i = 0; i < transform.childCount; i++)
            UpdateLightColorRecursive(transform.GetChild(i), color);
    }
}

using ModSettings;

using UniversalTweaks.Utilities;

namespace UniversalTweaks.Properties;

internal class Settings : JsonModSettings
{
    internal static Settings Instance { get; } = new();

    #region General Tweaks

    [Section("General Tweaks")]
    [Name("Additional Carry Weight")]
    [Description("Adds an additional amount weight on top of the players existing weight.")]
    [Slider(0, 120, NumberFormat = "{0:0.##} KG")]
    public int AdditionalEncumbermentWeight = 0;

    [Name("Breath Visibility")]
    [Description("Toggle the visual breath effect on or off.")]
    public bool BreathVisibility = true;

    [Name("Consistant Old Man's Dressing Weight")]
    [Description("Changes the old man's dressing weight to be the same as lichen.")]
    public bool ConsistantDressingWeight = false;

    [Name("Feat Progress in Custom Mode")]
    [Description("Determine whether feat progress counts in custom difficulties.")]
    public bool FeatProgressInCustomMode = false;

    [Name("Infinite Carry Weight")]
    [Description("Give the player a semi-infinite carrying weight.")]
    public bool InfiniteEncumberWeight = false;

    [Name("MRE Texture Variation")]
    [Description(
        "Switches the MRE texture to a brown variant. (Requires scene reload to change to the variant, and a game restart back to the original).")]
    public bool MRETextureVariant = false;

    [Name("Noisemaker Burn Length")]
    [Description("Adjust how quickly the fuse of the noisemaker burns in minutes. Default is 0.7 minutes.")]
    [Slider(0.7f, 2.7f, NumberFormat = "{0:0.##} MIN")]
    public float NoisemakerBurnLength = 0.7f;

    [Name("Noisemaker Throw Force")]
    [Description("Allow the player to have more force when throwing a noisemaker. Default is 9.")]
    [Slider(1, 20)]
    public int NoisemakerThrowForce = 9;

    [Name("Randomized Item Rotation Drops")]
    [Description("Randomizes the rotation of items when they are dropped.")]
    public bool RandomizedItemRotationDrops = false;

    [Name("Respirator Canister Duration")]
    [Description("Determines the time in real life seconds which the canister lasts. Default is 45 seconds.")]
    [Slider(45, 90, 4, NumberFormat = "{0:0.##} SEC")]
    public int RespiratorCanisterDuration = 45;

    [Name("Revolver Handling Improvements")]
    [Description("Allows movement while aiming with the revolver.")]
    public bool RevolverImprovements = false;

    [Name("Snow Shelter Decay Rate")]
    [Description("Adjust the daily decay rate of snow shelters. Default is 100.")]
    [Slider(50, 100)]
    public int SnowShelterDailyDecayRate = 100;

    [Name("Toilet Water Potability")]
    [Description("Sets whether water obtained from toilets is potable or non-potable.")]
    [Choice("Potable", "Non-Potable")]
    public int ToiletWaterQuality = 0;

    #endregion

    #region Flashlight Tweaks

    [Section("Flashlight Tweaks")]
    [Name("Aurora Flickering")]
    [Description("Alters the flashlight's flickering behavior by only flickering during an aurora if enabled.")]
    public bool AuroraFlickering = false;

    [Name("Battery Randomization")]
    [Description("Sets the flashlight to start with a random battery charge level.")]
    public bool BatteryRandomization = false;

    [Name("Extended Functionality")]
    [Description("Allows the flashlight to function anytime, not just during auroras.")]
    public bool ExtendedFunctionality = false;

    [Name("High-Beam Restrictions")]
    [Description("Restricts high beam functionality to aurora events only.")]
    public bool HighBeamRestrictions = false;

    [Name("Flashlight Beam Color")]
    [Description("Changes the color of the flashlight's beam.")]
    public FlashlightBeamColor FlashlightBeamColor = FlashlightBeamColor.Default;

    [Name("Red Value")][Slider(0, 255)] public int FlashlightRedValue = 0;

    [Name("Green Value")][Slider(0, 255)] public int FlashlightGreenValue = 0;

    [Name("Blue Value")][Slider(0, 255)] public int FlashlightBlueValue = 0;

    [Name("Flashlight Low Beam Duration")]
    [Description("Adjusts the flashlight's low beam duration. Default is 1.")]
    [Slider(0.08333334f, 2f, 20)]
    public float FlashlightLowBeamDuration = 1f;

    //[Name("Flashlight Low Beam Intensity")]
    //[Slider(0, 25)] public int FlashlightLowBeamIntensity = 1;

    //[Name("Flashlight Low Beam Range")]
    //[Slider(0, 100)] public int FlashlightLowBeamRange = 40;

    [Name("Flashlight High Beam Duration")]
    [Description("Adjusts the flashlight's high beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float FlashlightHighBeamDuration = 0.08333334f;

    //[Name("Flashlight High Beam Intensity")]
    //[Slider(0, 25, 26)] public float FlashlightHighBeamIntensity = 7.86f;

    //[Name("Flashlight High Beam Range")]
    //[Slider(0, 200)] public int FlashlightHighBeamRange = 80;

    [Name("Flashlight Recharge Time")]
    [Description("Sets the recharge time for the flashlight's battery. Range: 0 (fast) to 2 (slow).")]
    [Slider(0f, 2f, 20)]
    public float FlashlightRechargeTime = 2f;

    [Name("Infinite Battery")]
    [Description("Ensures the flashlight's battery never depletes, allowing unlimited use without recharging.")]
    public bool InfiniteBattery = false;

    [Name("Miner's Flashlight Beam Color")]
    [Description("Changes the color of the miner's flashlight's beam.")]
    public FlashlightBeamColor MinersFlashlightBeamColor = FlashlightBeamColor.Default;

    [Name("Red Value")][Slider(0, 255)] public int MinersFlashlightRedValue = 0;

    [Name("Green Value")][Slider(0, 255)] public int MinersFlashlightGreenValue = 0;

    [Name("Blue Value")][Slider(0, 255)] public int MinersFlashlightBlueValue = 0;

    [Name("Miner's Flashlight Low Beam Duration")]
    [Description("Adjusts the miner's flashlight's low beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float MinersFlashlightLowBeamDuration = 1.5f;

    //[Name("Miner's Flashlight Low Beam Intensity")]
    //[Slider(0, 25)] public int MinersFlashlightLowBeamIntensity = 1;

    //[Name("Miner's Flashlight Low Beam Range")]
    //[Slider(0, 100)] public int MinersFlashlightLowBeamRange = 40;

    [Name("Miner's Flashlight High Beam Duration")]
    [Description("Adjusts the miner's flashlight's high beam duration. Range: 0.1 (short) to 2 (long).")]
    [Slider(0.08333334f, 2f, 20)]
    public float MinersFlashlightHighBeamDuration = 0.08333334f;

    //[Name("Miner's Flashlight High Beam Intensity")]
    //[Slider(0, 25, 26)] public float MinersFlashlightHighBeamIntensity = 7.86f;

    //[Name("Miner's Flashlight High Beam Range")]
    //[Slider(0, 200)] public int MinersFlashlightHighBeamRange = 80;

    [Name("Miner's Flashlight Recharge Time")]
    [Description("Sets the recharge time for the miner's flashlight's battery. Range: 0 (fast) to 2 (slow).")]
    [Slider(0f, 2f, 20)]
    public float MinersFlashlightRechargeTime = 1.75f;

    #endregion

    #region Food Tweaks

    [Section("Food Tweaks")]
    [Name("Insulated Flask Duration Indoors")]
    [Description(
        "Customise the duration of how long heat is keep inside the flask indoors. Range: 0.1 (16.7 in-game hours) to 0.25 (6.7 in-game hours / default). (Requires scene reload).")]
    [Slider(0.1f, 1, 18)]
    public float InsulatedFlaskHeatLossPerMinuteIndoors = 0.25f;

    [Name("Insulated Flask Duration Outdoors")]
    [Description(
        "Customise the duration of how long heat is keep inside the flask outdoors. Range: 0.1 (16.7 in-game hours) to 0.5 (3.3 in-game hours / default). (Requires scene reload).")]
    [Slider(0.1f, 1, 18)]
    public float InsulatedFlaskHeatLossPerMinuteOutdoors = 0.5f;

    [Name("Remove Headache from Foods")]
    [Description("Removes the headache debuff from certain foods. (Requires scene reload).")]
    public bool RemoveHeadacheDebuffFromFoods = false;

    [Name("Reduce Stew Fatigue Loss")]
    [Description("Reduces the amount of fatigue lost after consuming. Default is 15. (Requires scene reload).")]
    [Slider(0, 15, 15)]
    public int ReduceStewFatigueLossAmount = 15;

    #endregion

    #region Rock Cache Tweaks

    [Section("Rock Cache Tweaks")]
    [Name("Allowed Indoors")]
    [Description("Enables the placement of rock caches indoors.")]
    public bool AllowedIndoorsRockCaches = false;

    [Name("Maximum Per Region")]
    [Description("Sets the maximum number of rock caches allowed per region. Default is 5.")]
    [Slider(1, 100)]
    public int MaximumPerRegionRockCaches = 5;

    [Name("Minimum Distance Between Caches")]
    [Description("Determines the minimum distance required between rock caches. Default is 10.")]
    [Slider(0.3f, 10, 97)]
    public float MinimumDistanceBetweenRockCaches = 10;

    #endregion

    #region Spray Paint Tweaks

    [Section("Spray Paint Tweaks")]
    [Name("Decal Overlap Leniency")]
    [Description("Set the leniency for placing decals. Default is 0.2.")]
    [Slider(0f, 1f, 11)]
    public float DecalOverlapLeniency = 0.2f;

    [Name("Glowing Decals")]
    [Description("Toggle the glow effect for spray paint decals. (Applies once settings are confirmed).")]
    public bool GlowingDecals = false;

    [Name("Glow Brightness Multiplier")]
    [Description("Adjust the brightness of the glow effect. Default is 1.")]
    [Slider(0.5f, 1.5f, 11)]
    public float GlowingDecalMultiplier = 1f;

    #endregion

    #region Travois Tweaks

    [Section("Travois Tweaks")]
    [Name("Decay Blizzard Per Hour")]
    [Description("Determines how quickly the travois deteriorates during a blizzard. Default is 3.")]
    [Slider(1, 10)]
    public int DecayBlizzardTravois = 3;

    [Name("Decay HP Per Hour")]
    [Description("Sets the rate at which the travois loses hit points per hour. Default is 10.")]
    [Slider(1, 10, 10)]
    public int DecayHPPerHourTravois = 10;

    [Name("Decay Movement Per Unit")]
    [Description("Configures how much the travois's movement capability degrades per unit carried. Default is 5.")]
    [Slider(1, 5, 5)]
    public int DecayMovementPerUnitTravois = 5;

    [Name("Maximum Slope Angle")]
    [Description("Sets the maximum slope angle the travois can handle. Default is 35.")]
    [Slider(35, 75)]
    public int MaximumSlopeAngleTravois = 35;

    [Name("Turning Speed")]
    [Description("Adjusts the turning speed of the travois. Default is 0.5.")]
    [Slider(0.5f, 5f, 45)]
    public float TurnSpeedTravois = 0.5f;

    [Name("Override Movement Restrictions")]
    [Description(
        "Overrides the movement restrictions in place by Hinterland, allowing to easy travel indoors and between regions.")]
    public bool OverrideTravoisMovementRestrictions = false;

    [Name("Override Interaction Restrictions")]
    [Description("Overrides not being able to interact with certain things such as door while holding the Travois.")]
    public bool OverrideTravoisInteractionRestrictions = false;

    #endregion

    #region UI Tweaks

    [Section("UI Tweaks")]
    /*[Name("Campfire HUD Elements")] [Description("Disables all the elements on the UI that are related to the campfire lives remaining.")]
    public bool DisableCampfireHUDElements = false;*/

    // [Name("Cougar HUD Elements")] [Description("Disables all the elements on the UI that are related to the Cougar.")]
    // public bool DisableCougarHUDElements = false;
    [Name("Promotional Main Menu Items")]
    [Description("Removes the promotional WINTERMUTE and Expansion menu items. (Requires main menu reload).")]
    public bool RemoveMainMenuItems = false;

    [Name("Permanent Crosshair")]
    [Description("Keeps the crosshair visible at all times.")]
    public bool PermanentCrosshair = false;

    #endregion

    #region Miscellaneous

    [Section("Miscellaneous")]
    [Name("'Cheat' Tweaks")]
    [Description("Enables tweaks which alters items in a way that can be seen as 'cheating'.")]
    public bool CheatingTweaks = false;

    #endregion

    internal bool ConfirmedGlowingDecals { get; private set; }
    internal float ConfirmedGlowingDecalMultiplier { get; private set; }

    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    protected override void OnConfirm()
    {
        base.OnConfirm();

        ConfirmedGlowingDecals = GlowingDecals;
        ConfirmedGlowingDecalMultiplier = GlowingDecalMultiplier;

        Mod.Logger.Log("Settings confirmed.", FlaggedLoggingLevel.Debug);

        // Will need to change this return back into a != check if anything else here is added.
        var encumber = GameManager.GetEncumberComponent();
        if (encumber == null) return;
        Tweaks.Encumber.EncumberUpdate(encumber);
    }

    // Update the following code below, move it to another class underneath Utilities called SettingsOptionsVisibility or something
    // Split these up into different bool methods to hide or enable all fields like CheatTweaks, ContainerTweaks etc.
    // After doing that, when certain options are selected - hide certain ones. Example being infinite container weight, hide the rest of the options.
    private void RefreshFields()
    {
        if (ExtendedFunctionality)
        {
            SetFieldVisible(nameof(HighBeamRestrictions), true);
        }
        else
        {
            SetFieldVisible(nameof(HighBeamRestrictions), false);
        }

        if (FlashlightBeamColor == FlashlightBeamColor.Custom)
        {
            SetFieldVisible(nameof(FlashlightRedValue), true);
            SetFieldVisible(nameof(FlashlightGreenValue), true);
            SetFieldVisible(nameof(FlashlightBlueValue), true);
        }
        else
        {
            SetFieldVisible(nameof(FlashlightRedValue), false);
            SetFieldVisible(nameof(FlashlightGreenValue), false);
            SetFieldVisible(nameof(FlashlightBlueValue), false);
        }

        if (MinersFlashlightBeamColor == FlashlightBeamColor.Custom)
        {
            SetFieldVisible(nameof(MinersFlashlightRedValue), true);
            SetFieldVisible(nameof(MinersFlashlightGreenValue), true);
            SetFieldVisible(nameof(MinersFlashlightBlueValue), true);
        }
        else
        {
            SetFieldVisible(nameof(MinersFlashlightRedValue), false);
            SetFieldVisible(nameof(MinersFlashlightGreenValue), false);
            SetFieldVisible(nameof(MinersFlashlightBlueValue), false);
        }

        if (CheatingTweaks)
        {
            SetFieldVisible(nameof(SnowShelterDailyDecayRate), true);

            SetFieldVisible(nameof(DecayBlizzardTravois), true);
            SetFieldVisible(nameof(DecayHPPerHourTravois), true);
            SetFieldVisible(nameof(DecayMovementPerUnitTravois), true);

            SetFieldVisible(nameof(InfiniteBattery), true);
            SetFieldVisible(nameof(FlashlightLowBeamDuration), true);
            SetFieldVisible(nameof(FlashlightHighBeamDuration), true);
            SetFieldVisible(nameof(FlashlightRechargeTime), true);
            //SetFieldVisible(nameof(FlashlightLowBeamIntensity), true);
            //SetFieldVisible(nameof(FlashlightLowBeamRange), true);
            //SetFieldVisible(nameof(FlashlightHighBeamIntensity), true);
            //SetFieldVisible(nameof(FlashlightHighBeamRange), true);

            SetFieldVisible(nameof(MinersFlashlightLowBeamDuration), true);
            SetFieldVisible(nameof(MinersFlashlightHighBeamDuration), true);
            SetFieldVisible(nameof(MinersFlashlightRechargeTime), true);
            //SetFieldVisible(nameof(MinersFlashlightLowBeamIntensity), true);
            //SetFieldVisible(nameof(MinersFlashlightLowBeamRange), true);
            //SetFieldVisible(nameof(MinersFlashlightHighBeamIntensity), true);
            //SetFieldVisible(nameof(MinersFlashlightHighBeamRange), true);

            SetFieldVisible(nameof(OverrideTravoisMovementRestrictions), true);
            SetFieldVisible(nameof(OverrideTravoisInteractionRestrictions), true);

            SetFieldVisible(nameof(InfiniteEncumberWeight), true);
        }
        else
        {
            SetFieldVisible(nameof(SnowShelterDailyDecayRate), false);

            SetFieldVisible(nameof(DecayBlizzardTravois), false);
            SetFieldVisible(nameof(DecayHPPerHourTravois), false);
            SetFieldVisible(nameof(DecayMovementPerUnitTravois), false);

            SetFieldVisible(nameof(InfiniteBattery), false);
            SetFieldVisible(nameof(FlashlightLowBeamDuration), false);
            SetFieldVisible(nameof(FlashlightHighBeamDuration), false);
            SetFieldVisible(nameof(FlashlightRechargeTime), false);
            //SetFieldVisible(nameof(FlashlightLowBeamIntensity), false);
            //SetFieldVisible(nameof(FlashlightLowBeamRange), false);
            //SetFieldVisible(nameof(FlashlightHighBeamIntensity), false);
            //SetFieldVisible(nameof(FlashlightHighBeamRange), false);

            SetFieldVisible(nameof(MinersFlashlightLowBeamDuration), false);
            SetFieldVisible(nameof(MinersFlashlightHighBeamDuration), false);
            SetFieldVisible(nameof(MinersFlashlightRechargeTime), false);
            //SetFieldVisible(nameof(MinersFlashlightLowBeamIntensity), false);
            //SetFieldVisible(nameof(MinersFlashlightLowBeamRange), false);
            //SetFieldVisible(nameof(MinersFlashlightHighBeamIntensity), false);
            //SetFieldVisible(nameof(MinersFlashlightHighBeamRange), false);

            SetFieldVisible(nameof(OverrideTravoisMovementRestrictions), false);
            SetFieldVisible(nameof(OverrideTravoisInteractionRestrictions), false);

            SetFieldVisible(nameof(InfiniteEncumberWeight), false);
        }

        ContainerSettings.Instance.RefreshFields();
    }

    internal static void OnLoad()
    {
        Instance.AddToModSettings(BuildInfo.Name);
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Instance.ConfirmedGlowingDecals = Instance.GlowingDecals;
        Instance.ConfirmedGlowingDecalMultiplier = Instance.GlowingDecalMultiplier;

        Mod.Logger.Log("Settings loaded.", FlaggedLoggingLevel.Debug);
    }
}
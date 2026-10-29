using ModSettings;

using UniversalTweaks.Core;
using UniversalTweaks.Utilities;

namespace UniversalTweaks;

/// <summary>
/// The mod's page in the game's Mod Settings menu, saved to <c>Mods\UniversalTweaks.json</c>. The container capacities
/// have their own page: <see cref="ContainerSettings"/>.
/// </summary>
/// <remarks>
/// ModSettings shows each public field as a setting, so the fields here are public. Their names are what the settings
/// file saves, so they keep their names rather than the <c>m_</c> prefix, or players would lose their settings. Most are
/// read where they're used; <see cref="OnConfirm"/> pushes the rest into objects that already exist.
/// </remarks>
internal sealed class Settings : JsonModSettings
{
    /// <summary>The settings only shown with <see cref="CheatingTweaks"/> on, which have no effect while it's off.</summary>
    /// <remarks>Declared before <see cref="Instance"/>, so it's set before the constructor could reach <see cref="RefreshFields"/>.</remarks>
    private static readonly string[] CheatingTweakFields =
    [
        nameof(AdditionalEncumbermentWeight),
        nameof(ExtendedFunctionality),
        nameof(SnowShelterDailyDecayRate),
        nameof(DecayBlizzardTravois),
        nameof(DecayHPPerHourTravois),
        nameof(DecayMovementPerUnitTravois),
        nameof(InfiniteBattery),
        nameof(FlashlightLowBeamDuration),
        nameof(FlashlightHighBeamDuration),
        nameof(FlashlightRechargeTime),
        nameof(FlashlightLowBeamIntensity),
        nameof(FlashlightLowBeamRange),
        nameof(FlashlightHighBeamIntensity),
        nameof(FlashlightHighBeamRange),
        nameof(MinersFlashlightLowBeamDuration),
        nameof(MinersFlashlightHighBeamDuration),
        nameof(MinersFlashlightRechargeTime),
        nameof(MinersFlashlightLowBeamIntensity),
        nameof(MinersFlashlightLowBeamRange),
        nameof(MinersFlashlightHighBeamIntensity),
        nameof(MinersFlashlightHighBeamRange),
        nameof(OverrideTravoisMovementRestrictions),
        nameof(OverrideTravoisInteractionRestrictions),
        nameof(InfiniteEncumberWeight),
    ];

    /// <summary>The settings, loaded from the file when the mod starts.</summary>
    internal static readonly Settings Instance = new();

    #region General Tweaks

    /// <summary>
    /// Whether the tweaks that could count as cheating are on. Off, their settings are hidden and the game's own values
    /// are used.
    /// </summary>
    [Section("General Tweaks")]
    [Name("'Cheat' Tweaks")]
    [Description("Enables tweaks which alters items in a way that can be seen as 'cheating'.")]
    public bool CheatingTweaks = false;

    /// <summary>Extra weight, in kilograms, the player can carry: see <see cref="Encumbrance"/>. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Additional Carry Weight")]
    [Description("Adds an additional amount weight on top of the players existing weight.")]
    [Slider(0, 120, NumberFormat = "{0:0.##} KG")]
    public int AdditionalEncumbermentWeight = 0;

    /// <summary>Whether the player's breath shows in the cold.</summary>
    [Name("Breath Visibility")]
    [Description("Toggle the visual breath effect on or off.")]
    public bool BreathVisibility = true;

    /// <summary>Whether the old man's beard dressing weighs the same as lichen: see <see cref="DressingWeight"/>.</summary>
    [Name("Consistent Old Man's Dressing Weight")]
    [Description("Changes the old man's dressing weight to be the same as lichen.")]
    public bool ConsistantDressingWeight = false;

    /// <summary>Whether one round dropped from a box drops as a single round: see <see cref="SingleRoundDrops"/>.</summary>
    [Name("Drop Single Rounds")]
    [Description("Dropping one rifle or revolver round from a box drops a single round instead of a box.")]
    public bool DropSingleRounds = false;

    /// <summary>Whether feats progress in custom games.</summary>
    [Name("Feat Progress in Custom Mode")]
    [Description("Determine whether feat progress counts in custom difficulties.")]
    public bool FeatProgressInCustomMode = false;

    /// <summary>Whether the player can carry a practically unlimited weight. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Infinite Carry Weight")]
    [Description("Give the player a semi-infinite carrying weight.")]
    public bool InfiniteEncumberWeight = false;

    /// <summary>Whether MREs are brown instead of blue: see <see cref="MreTexture"/>.</summary>
    [Name("MRE Texture Variation")]
    [Description("Switches the MRE texture to a brown variant. (Applies once settings are confirmed).")]
    public bool MRETextureVariant = false;

    /// <summary>How long the noisemaker's fuse burns, in in-game minutes.</summary>
    [Name("Noisemaker Burn Length")]
    [Description("Adjust how quickly the fuse of the noisemaker burns in minutes. Default is 0.7 minutes.")]
    [Slider(0.7f, 2.7f, NumberFormat = "{0:0.##} MIN")]
    public float NoisemakerBurnLength = 0.7f;

    /// <summary>How hard the player throws the noisemaker.</summary>
    [Name("Noisemaker Throw Force")]
    [Description("Allow the player to have more force when throwing a noisemaker. Default is 9.")]
    [Slider(1, 20)]
    public int NoisemakerThrowForce = 9;

    /// <summary>Whether dropped items face a random direction.</summary>
    [Name("Randomized Item Rotation Drops")]
    [Description("Randomizes the rotation of items when they are dropped.")]
    public bool RandomizedItemRotationDrops = false;

    /// <summary>How long a respirator canister lasts, in real seconds.</summary>
    [Name("Respirator Canister Duration")]
    [Description("Determines the time in real life seconds which the canister lasts. Default is 45 seconds.")]
    [Slider(45, 90, 4, NumberFormat = "{0:0.##} SEC")]
    public int RespiratorCanisterDuration = 45;

    /// <summary>
    /// Whether lighting a torch always opens the menu to pick a fire tool, even with only one, and uses up the match, or
    /// wears the flint and steel or fire striker, only once the torch lights: see <see cref="TorchLightMenu"/> and
    /// <see cref="TorchToolUse"/>.
    /// </summary>
    [Name("Safer Torch Lighting")]
    [Description("Lighting a torch always lets you pick a fire tool, even if you only have one, and only uses it up once the torch lights, so letting go early wastes nothing.")]
    public bool SaferTorchLighting = false;

    /// <summary>How many hit points a snow shelter loses a day. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Snow Shelter Decay Rate")]
    [Description("Adjust the daily decay rate of snow shelters. Default is 100.")]
    [Slider(50, 100)]
    public int SnowShelterDailyDecayRate = 100;

    #endregion

    #region Flashlight Tweaks

    /// <summary>Whether the flashlight only flickers during auroras. Needs <see cref="ExtendedFunctionalityActive"/>.</summary>
    [Section("Flashlight Tweaks")]
    [Name("Aurora Flickering")]
    [Description("Alters the flashlight's flickering behavior by only flickering during an aurora if enabled.")]
    public bool AuroraFlickering = false;

    /// <summary>Whether flashlights start with a random battery charge.</summary>
    [Name("Battery Randomization")]
    [Description("Sets the flashlight to start with a random battery charge level.")]
    public bool BatteryRandomization = false;

    /// <summary>
    /// Whether the flashlight works outside auroras: see <see cref="Flashlights"/>. Needs <see cref="CheatingTweaks"/>,
    /// so read <see cref="ExtendedFunctionalityActive"/>.
    /// </summary>
    [Name("Extended Functionality")]
    [Description("Allows the flashlight to function anytime, not just during auroras.")]
    public bool ExtendedFunctionality = false;

    /// <summary>Whether the high beam only works during auroras. Shown with <see cref="ExtendedFunctionalityActive"/>.</summary>
    [Name("High-Beam Restrictions")]
    [Description("Restricts high beam functionality to aurora events only.")]
    public bool HighBeamRestrictions = false;

    /// <summary>The flashlight's beam colour.</summary>
    [Name("Flashlight Beam Color")]
    [Description("Changes the color of the flashlight's beam.")]
    public FlashlightBeamColor FlashlightBeamColor = FlashlightBeamColor.Default;

    /// <summary>The red in the flashlight's custom beam colour, 0 to 255.</summary>
    [Name("Red Value")][Slider(0, 255)] public int FlashlightRedValue = 0;

    /// <summary>The green in the flashlight's custom beam colour, 0 to 255.</summary>
    [Name("Green Value")][Slider(0, 255)] public int FlashlightGreenValue = 0;

    /// <summary>The blue in the flashlight's custom beam colour, 0 to 255.</summary>
    [Name("Blue Value")][Slider(0, 255)] public int FlashlightBlueValue = 0;

    /// <summary>How long the flashlight's battery lasts on the low beam, in in-game hours. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight Low Beam Duration")]
    [Description("Adjusts the flashlight's low beam duration. Default is 1.")]
    [Slider(1f, 2f, 11)]
    public float FlashlightLowBeamDuration = 1f;

    /// <summary>The flashlight's low beam brightness. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight Low Beam Intensity")]
    [Description("Adjusts the brightness of the flashlight's low beam. Default is 1.")]
    [Slider(1, 25)]
    public int FlashlightLowBeamIntensity = 1;

    /// <summary>The flashlight's low beam range, in metres. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight Low Beam Range")]
    [Description("Adjusts the reach of the flashlight's low beam. Default is 40.")]
    [Slider(40, 100)]
    public int FlashlightLowBeamRange = 40;

    /// <summary>How long the flashlight's battery lasts on the high beam, in in-game hours. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight High Beam Duration")]
    [Description("Adjusts the flashlight's high beam duration. Default is 0.08.")]
    [Slider(0.08333334f, 2f, 20)]
    public float FlashlightHighBeamDuration = 0.08333334f;

    /// <summary>The flashlight's high beam brightness. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight High Beam Intensity")]
    [Description("Adjusts the brightness of the flashlight's high beam. Default is 7.86.")]
    [Slider(7.86f, 25f, 1715, NumberFormat = "{0:0.##}")]
    public float FlashlightHighBeamIntensity = 7.86f;

    /// <summary>The flashlight's high beam range, in metres. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight High Beam Range")]
    [Description("Adjusts the reach of the flashlight's high beam. Default is 80.")]
    [Slider(80, 200)]
    public int FlashlightHighBeamRange = 80;

    /// <summary>How long the flashlight's battery takes to recharge, in in-game hours. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Flashlight Recharge Time")]
    [Description("Sets the recharge time for the flashlight's battery. Default is 2.")]
    [Slider(0f, 2f, 20)]
    public float FlashlightRechargeTime = 2f;

    /// <summary>Whether flashlight batteries never run out. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Infinite Battery")]
    [Description("Ensures the flashlight's battery never depletes, allowing unlimited use without recharging.")]
    public bool InfiniteBattery = false;

    /// <summary>The miner's flashlight's beam colour.</summary>
    [Name("Miner's Flashlight Beam Color")]
    [Description("Changes the color of the miner's flashlight's beam.")]
    public FlashlightBeamColor MinersFlashlightBeamColor = FlashlightBeamColor.Default;

    /// <summary>The red in the miner's flashlight's custom beam colour, 0 to 255.</summary>
    [Name("Red Value")][Slider(0, 255)] public int MinersFlashlightRedValue = 0;

    /// <summary>The green in the miner's flashlight's custom beam colour, 0 to 255.</summary>
    [Name("Green Value")][Slider(0, 255)] public int MinersFlashlightGreenValue = 0;

    /// <summary>The blue in the miner's flashlight's custom beam colour, 0 to 255.</summary>
    [Name("Blue Value")][Slider(0, 255)] public int MinersFlashlightBlueValue = 0;

    /// <summary>How long the miner's flashlight's battery lasts on the low beam, in in-game hours. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight Low Beam Duration")]
    [Description("Adjusts the miner's flashlight's low beam duration. Default is 1.5.")]
    [Slider(1.5f, 2f, 6)]
    public float MinersFlashlightLowBeamDuration = 1.5f;

    /// <summary>The miner's flashlight's low beam brightness. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight Low Beam Intensity")]
    [Description("Adjusts the brightness of the miner's flashlight's low beam. Default is 1.")]
    [Slider(1, 25)]
    public int MinersFlashlightLowBeamIntensity = 1;

    /// <summary>The miner's flashlight's low beam range, in metres. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight Low Beam Range")]
    [Description("Adjusts the reach of the miner's flashlight's low beam. Default is 40.")]
    [Slider(40, 100)]
    public int MinersFlashlightLowBeamRange = 40;

    /// <summary>How long the miner's flashlight's battery lasts on the high beam, in in-game hours. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight High Beam Duration")]
    [Description("Adjusts the miner's flashlight's high beam duration. Default is 0.08.")]
    [Slider(0.08333334f, 2f, 20)]
    public float MinersFlashlightHighBeamDuration = 0.08333334f;

    /// <summary>The miner's flashlight's high beam brightness. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight High Beam Intensity")]
    [Description("Adjusts the brightness of the miner's flashlight's high beam. Default is 7.86.")]
    [Slider(7.86f, 25f, 1715, NumberFormat = "{0:0.##}")]
    public float MinersFlashlightHighBeamIntensity = 7.86f;

    /// <summary>The miner's flashlight's high beam range, in metres. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight High Beam Range")]
    [Description("Adjusts the reach of the miner's flashlight's high beam. Default is 80.")]
    [Slider(80, 200)]
    public int MinersFlashlightHighBeamRange = 80;

    /// <summary>How long the miner's flashlight's battery takes to recharge, in in-game hours. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Miner's Flashlight Recharge Time")]
    [Description("Sets the recharge time for the miner's flashlight's battery. Default is 1.75.")]
    [Slider(0f, 1.75f, 8)]
    public float MinersFlashlightRechargeTime = 1.75f;

    #endregion

    #region Food Tweaks

    /// <summary>The share of its heat, in percent, an insulated flask loses each in-game minute indoors.</summary>
    [Section("Food Tweaks")]
    [Name("Insulated Flask Duration Indoors")]
    [Description(
        "Customise the duration of how long heat is keep inside the flask indoors. Range: 0.1 (16.7 in-game hours) to 0.25 (6.7 in-game hours / default).")]
    [Slider(0.1f, 1, 18)]
    public float InsulatedFlaskHeatLossPerMinuteIndoors = 0.25f;

    /// <summary>The share of its heat, in percent, an insulated flask loses each in-game minute outdoors.</summary>
    [Name("Insulated Flask Duration Outdoors")]
    [Description(
        "Customise the duration of how long heat is keep inside the flask outdoors. Range: 0.1 (16.7 in-game hours) to 0.5 (3.3 in-game hours / default).")]
    [Slider(0.1f, 1, 18)]
    public float InsulatedFlaskHeatLossPerMinuteOutdoors = 0.5f;

    /// <summary>Whether foods that give a headache, like pies and porridge, no longer do.</summary>
    [Name("Remove Headache from Foods")]
    [Description("Removes the headache debuff from any food that causes one.")]
    public bool RemoveHeadacheDebuffFromFoods = false;

    /// <summary>How much fatigue the meat and vegetable stews take away.</summary>
    [Name("Reduce Stew Fatigue Loss")]
    [Description("Reduces the amount of fatigue lost after consuming. Default is 15.")]
    [Slider(0, 15, 15)]
    public int ReduceStewFatigueLossAmount = 15;

    /// <summary>Whether toilet water is potable (0), non-potable (1), or left to the game (2).</summary>
    [Name("Toilet Water Potability")]
    [Description("Sets whether water obtained from toilets is potable or non-potable. Vanilla leaves it to the game.")]
    [Choice("Potable", "Non-Potable", "Vanilla")]
    public int ToiletWaterQuality = 2;

    #endregion

    #region Rock Cache Tweaks

    /// <summary>Whether rock caches can be built indoors.</summary>
    [Section("Rock Cache Tweaks")]
    [Name("Allowed Indoors")]
    [Description("Enables the placement of rock caches indoors.")]
    public bool AllowedIndoorsRockCaches = false;

    /// <summary>The most rock caches the player can build in a region.</summary>
    [Name("Maximum Per Region")]
    [Description("Sets the maximum number of rock caches allowed per region. Default is 5.")]
    [Slider(1, 100)]
    public int MaximumPerRegionRockCaches = 5;

    /// <summary>The least distance, in metres, between two rock caches.</summary>
    [Name("Minimum Distance Between Caches")]
    [Description("Determines the minimum distance required between rock caches. Default is 10.")]
    [Slider(0.3f, 10, 97)]
    public float MinimumDistanceBetweenRockCaches = 10;

    #endregion

    #region Spray Paint Tweaks

    /// <summary>How much spray paint decals can overlap, from 0 to 1.</summary>
    [Section("Spray Paint Tweaks")]
    [Name("Decal Overlap Leniency")]
    [Description("Set the leniency for placing decals. Default is 0.2.")]
    [Slider(0f, 1f, 11)]
    public float DecalOverlapLeniency = 0.2f;

    /// <summary>Whether spray paint decals glow. Only takes effect once confirmed: see <see cref="ConfirmedGlowingDecals"/>.</summary>
    [Name("Glowing Decals")]
    [Description("Toggle the glow effect for spray paint decals. (Applies once settings are confirmed).")]
    public bool GlowingDecals = false;

    /// <summary>How brightly glowing decals glow.</summary>
    [Name("Glow Brightness Multiplier")]
    [Description("Adjust the brightness of the glow effect. Default is 1.")]
    [Slider(0.5f, 1.5f, 11)]
    public float GlowingDecalMultiplier = 1f;

    /// <summary>Whether spray paint markings reach the map whatever the visibility.</summary>
    [Name("Ignore Map Visibility")]
    [Description("Spray paint markings are added to the map even in low visibility, such as at night or in fog.")]
    public bool IgnoreDecalMapVisibility = false;

    #endregion

    #region Travois Tweaks

    /// <summary>How many hit points the travois loses an hour in a blizzard. Needs <see cref="CheatingTweaks"/>.</summary>
    [Section("Travois Tweaks")]
    [Name("Decay Blizzard Per Hour")]
    [Description("Determines how quickly the travois deteriorates during a blizzard. Default is 3.")]
    [Slider(1, 10)]
    public int DecayBlizzardTravois = 3;

    /// <summary>How fast the travois decays over time, in thousandths of a hit point an hour. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Decay HP Per Hour")]
    [Description("Sets the rate at which the travois loses hit points per hour. Default is 10.")]
    [Slider(1, 10, 10)]
    public int DecayHPPerHourTravois = 10;

    /// <summary>How fast the travois decays as it's dragged, in hundredths of a hit point a unit. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Decay Movement Per Unit")]
    [Description("Configures how much the travois's movement capability degrades per unit carried. Default is 5.")]
    [Slider(1, 5, 5)]
    public int DecayMovementPerUnitTravois = 5;

    /// <summary>The steepest slope, in degrees, the travois can go up or down.</summary>
    [Name("Maximum Slope Angle")]
    [Description("Sets the maximum slope angle the travois can handle. Default is 35.")]
    [Slider(35, 75)]
    public int MaximumSlopeAngleTravois = 35;

    /// <summary>How fast the travois turns.</summary>
    [Name("Turning Speed")]
    [Description("Adjusts the turning speed of the travois. Default is 0.5.")]
    [Slider(0.5f, 5f, 45)]
    public float TurnSpeedTravois = 0.5f;

    /// <summary>Whether the travois can go anywhere, like indoors and between regions. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Override Movement Restrictions")]
    [Description(
        "Overrides the movement restrictions in place by Hinterland, allowing to easy travel indoors and between regions.")]
    public bool OverrideTravoisMovementRestrictions = false;

    /// <summary>Whether the player can interact with things, like doors, while carrying the travois. Needs <see cref="CheatingTweaks"/>.</summary>
    [Name("Override Interaction Restrictions")]
    [Description("Overrides not being able to interact with certain things such as door while holding the Travois.")]
    public bool OverrideTravoisInteractionRestrictions = false;

    #endregion

    #region UI Tweaks

    /// <summary>Whether the crosshair always shows.</summary>
    [Section("UI Tweaks")]
    [Name("Permanent Crosshair")]
    [Description("Keeps the crosshair visible at all times.")]
    public bool PermanentCrosshair = false;

    /// <summary>Whether the sprint stamina bar always shows.</summary>
    [Name("Permanent Stamina Bar")]
    [Description("Keeps the sprint stamina bar in the bottom right visible at all times.")]
    public bool PermanentStaminaBar = false;

    /// <summary>Whether the sun and moon dial always shows.</summary>
    [Name("Permanent Time of Day Dial")]
    [Description("Keeps the sun and moon dial in the top right visible at all times.")]
    public bool PermanentTimeOfDayDial = false;

    #endregion

    #region Miscellaneous

    /// <summary>Whether the game starts straight into the main menu: see <see cref="Patches.UtilsPatches"/>.</summary>
    [Section("Miscellaneous")]
    [Name("Skip Intro")]
    [Description("Skips the disclaimers, the press any key prompt and the intro movie when the game starts. (Applies from the next launch).")]
    public bool SkipIntro = false;

    #endregion

    /// <summary>Whether the flashlight works outside auroras: <see cref="ExtendedFunctionality"/> with <see cref="CheatingTweaks"/> on.</summary>
    internal bool ExtendedFunctionalityActive => CheatingTweaks && ExtendedFunctionality;

    /// <summary><see cref="GlowingDecals"/> as last confirmed, which is what decals use.</summary>
    internal bool ConfirmedGlowingDecals { get; private set; }

    /// <summary><see cref="GlowingDecalMultiplier"/> as last confirmed, which is what decals use.</summary>
    internal float ConfirmedGlowingDecalMultiplier { get; private set; }

    /// <summary>Adds the settings to the Mod Settings menu and puts the saved values into effect.</summary>
    internal static void Initialize()
    {
        Instance.AddToModSettings(Properties.BuildInfo.Name);
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Instance.ConfirmedGlowingDecals = Instance.GlowingDecals;
        Instance.ConfirmedGlowingDecalMultiplier = Instance.GlowingDecalMultiplier;

        Mod.Logger.Log("Settings loaded.", FlaggedLoggingLevel.Debug);
    }

    /// <inheritdoc />
    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    /// <inheritdoc />
    /// <remarks>Pushes the settings into the objects that already exist, so they take effect without reloading.</remarks>
    protected override void OnConfirm()
    {
        base.OnConfirm();

        ConfirmedGlowingDecals = GlowingDecals;
        ConfirmedGlowingDecalMultiplier = GlowingDecalMultiplier;

        Mod.Logger.Log("Settings confirmed.", FlaggedLoggingLevel.Debug);

        var encumber = GameManager.GetEncumberComponent();
        if (encumber != null)
            Encumbrance.Apply(encumber);

        MreTexture.Refresh();
        DressingWeight.Refresh();
        StewFatigue.Refresh();
        Travois.Refresh();
        ContainerCapacities.Refresh();
    }

    /// <summary>
    /// Shows only the settings that apply: the high-beam restriction with extended functionality, the custom colour
    /// values with a custom colour, and the cheat settings with 'Cheat' Tweaks. Also refreshes
    /// <see cref="ContainerSettings"/>, whose infinite weight setting depends on 'Cheat' Tweaks.
    /// </summary>
    private void RefreshFields()
    {
        // TODO: Move this into its own class in Utilities, and split the fields into groups that can be shown or hidden.
        SetFieldVisible(nameof(HighBeamRestrictions), ExtendedFunctionalityActive);

        var showFlashlightCustomColor = FlashlightBeamColor == FlashlightBeamColor.Custom;
        SetFieldVisible(nameof(FlashlightRedValue), showFlashlightCustomColor);
        SetFieldVisible(nameof(FlashlightGreenValue), showFlashlightCustomColor);
        SetFieldVisible(nameof(FlashlightBlueValue), showFlashlightCustomColor);

        var showMinersFlashlightCustomColor = MinersFlashlightBeamColor == FlashlightBeamColor.Custom;
        SetFieldVisible(nameof(MinersFlashlightRedValue), showMinersFlashlightCustomColor);
        SetFieldVisible(nameof(MinersFlashlightGreenValue), showMinersFlashlightCustomColor);
        SetFieldVisible(nameof(MinersFlashlightBlueValue), showMinersFlashlightCustomColor);

        foreach (var field in CheatingTweakFields)
            SetFieldVisible(field, CheatingTweaks);

        ContainerSettings.Instance.RefreshFields();
    }
}

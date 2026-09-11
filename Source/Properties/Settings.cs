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
        "Customise the duration of how long heat is keep inside the flask indoors. Range: 0.1 (1000 in-game minutes) to 0.25 (400 in-game minutes / default). (Requires scene reload).")]
    [Slider(0.1f, 1, 18)]
    public float InsulatedFlaskHeatLossPerMinuteIndoors = 0.25f;

    [Name("Insulated Flask Duration Outdoors")]
    [Description(
        "Customise the duration of how long heat is keep inside the flask outdoors. Range: 0.1 (1000 in-game minutes) to 0.5 (200 in-game minutes / default). (Requires scene reload).")]
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
    [Description("Toggle the glow effect for spray paint decals. (Requires scene reload to turn back off).")]
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

    #region Weight Container Tweaks

    [Section("Container Weight Tweaks")]
    [Name("Infinite Container Weight")]
    [Description("Gives all the containers a semi-infinite weight value.")]
    public bool InfiniteContainerWeight = false;

    [Name("Backpack")]
    [Description("Adjust how much weight Backpack can hold. Default is 15 KG.")]
    [Slider(0f, 30f, 31, NumberFormat = "{0:0.##} KG")]
    public float ContainerBackpackCapacity = 15;

    [Name("Briefcase")]
    [Description("Adjust how much weight Briefcase can hold. Default is 10 KG.")]
    [Slider(0f, 30f, 31, NumberFormat = "{0:0.##} KG")]
    public float ContainerBriefcaseCapacity = 10;

    [Name("Cabinet (Small)")]
    [Description("Adjust how much weight Cabinet (Small) can hold. Default is 20 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerCabinetSmlCapacity = 20;

    [Name("Cabinet (Large)")]
    [Description("Adjust how much weight Cabinet (Large) can hold. Default is 40 KG.")]
    [Slider(0f, 200f, 201, NumberFormat = "{0:0.##} KG")]
    public float ContainerCabinetLgeCapacity = 40;

    [Name("Cargo Container")]
    [Description("Adjust how much weight Cargo Container can hold. Default is 30 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerCargoContainerCapacity = 30;

    [Name("Cash Register")]
    [Description("Adjust how much weight Cash Register can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerCashRegisterCapacity = 5;

    [Name("Coal Bin")]
    [Description("Adjust how much weight Coal Bin can hold. Default is 60 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerCoalBinCapacity = 60;

    [Name("Cooler")]
    [Description("Adjust how much weight Cooler can hold. Default is 20 KG.")]
    [Slider(0f, 150f, 151, NumberFormat = "{0:0.##} KG")]
    public float ContainerCoolerCapacity = 20;

    [Name("Cupboard")]
    [Description("Adjust how much weight Cupboard can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerCupboardCapacity = 15;

    [Name("Dresser Drawer")]
    [Description("Adjust how much weight Dresser Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerDresserDrawerCapacity = 5;

    [Name("Dryer")]
    [Description("Adjust how much weight Dryer can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerDryerCapacity = 30;

    [Name("File Cabinet")]
    [Description("Adjust how much weight File Cabinet can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerFileCabinetCapacity = 10;

    [Name("First Aid Kit")]
    [Description("Adjust how much weight First Aid Kit can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerFirstAidCapacity = 5;

    [Name("Firewood Bin")]
    [Description("Adjust how much weight Firewood Bin can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerFirewoodBinCapacity = 30;

    [Name("Fishing Hut Drawer")]
    [Description("Adjust how much weight Fishing Hut Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerFishingHutDrawerCapacity = 10;

    [Name("Freezer")]
    [Description("Adjust how much weight Freezer can hold. Default is 20 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerFreezerCapacity = 20;

    [Name("Fridge")]
    [Description("Adjust how much weight Fridge can hold. Default is 40 KG.")]
    [Slider(0f, 200f, 201, NumberFormat = "{0:0.##} KG")]
    public float ContainerFridgeCapacity = 40;

    [Name("Glove Box")]
    [Description("Adjust how much weight Glove Box can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerGloveBoxCapacity = 5;

    [Name("Gun Locker")]
    [Description("Adjust how much weight Gun Locker can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerGunLockerCapacity = 30;

    [Name("Hatch")]
    [Description("Adjust how much weight Hatch can hold. Default is 40 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerHatchCapacity = 40;

    [Name("Hidden Cache")]
    [Description("Adjust how much weight Hidden Cache can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerHiddenCacheCapacity = 15;

    [Name("Infirmary Drawer")]
    [Description("Adjust how much weight Infirmary Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerInfirmaryDrawerCapacity = 10;

    [Name("Kitchen Cabinet")]
    [Description("Adjust how much weight Kitchen Cabinet can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerKitchenCabinetCapacity = 15;

    [Name("Kitchen Drawer")]
    [Description("Adjust how much weight Kitchen Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerKitchenDrawerCapacity = 10;

    [Name("Lock Box")]
    [Description("Adjust how much weight Lock Box can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerLockBoxCapacity = 10;

    [Name("Locker")]
    [Description("Adjust how much weight Locker can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerLockerCapacity = 30;

    [Name("Medicine Shelf")]
    [Description("Adjust how much weight Medicine Shelf can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerMedicineShelfCapacity = 5;

    [Name("Metal Desk Drawer (Small)")]
    [Description("Adjust how much weight Metal Desk Drawer (Small) can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerDeskDrawerSmlCapacity = 5;

    [Name("Metal Desk Drawer (Large)")]
    [Description("Adjust how much weight Metal Desk Drawer (Large) can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerDeskDrawerLgeCapacity = 10;

    [Name("Metal Container")]
    [Description("Adjust how much weight Metal Container can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerMetalContainerCapacity = 15;

    [Name("Oven")]
    [Description("Adjust how much weight Oven can hold. Default is 40 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerOvenCapacity = 40;

    [Name("Plastic Container")]
    [Description("Adjust how much weight Plastic Container can hold. Default is 15 KG.")]
    [Slider(0f, 30f, 31, NumberFormat = "{0:0.##} KG")]
    public float ContainerPlasticContainerCapacity = 15;

    [Name("Rock Cache")]
    [Description("Adjust how much weight Rock Cache can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerRockCacheCapacity = 30;

    [Name("Safe")]
    [Description("Adjust how much weight Safe can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerSafeCapacity = 10;

    [Name("Safety Deposit Box")]
    [Description("Adjust how much weight Safety Deposit Box can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerSafetyDepositBoxCapacity = 5;

    [Name("Side Table Drawer")]
    [Description("Adjust how much weight Side Table Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerEndTableDrawerCapacity = 5;

    [Name("Supply Bin")]
    [Description("Adjust how much weight Supply Bin can hold. Default is 30 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerSupplyBinCapacity = 30;

    [Name("Suitcase")]
    [Description("Adjust how much weight Suitcase can hold. Default is 20 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerSuitcaseCapacity = 20;

    [Name("Tool Cabinet Drawer (Small)")]
    [Description("Adjust how much weight Tool Cabinet Drawer (Small) can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerToolCabinetDrawerSmlCapacity = 5;

    [Name("Tool Cabinet Drawer (Large)")]
    [Description("Adjust how much weight Tool Cabinet Drawer (Large) can hold. Default is 10 KG.")]
    [Slider(0f, 100f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerToolCabinetDrawerLgeCapacity = 10;

    [Name("Trash Can")]
    [Description("Adjust how much weight Trash Can can hold. Default is 15 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerTrashCanCapacity = 15;

    [Name("Trunk (Car)")]
    [Description("Adjust how much weight Trunk (Car) can hold. Default is 40 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerCarTrunkCapacity = 40;

    [Name("Trunk (Rustic Storage)")]
    [Description("Adjust how much weight the Rustic Storage Trunk can hold. Default is 35 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerRusticStorageTrunkCapacity = 35;

    [Name("Trunk (Wooden Box)")]
    [Description("Adjust how much weight Trunk (Wooden Box) can hold. Default is 40 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerTrunkCapacity = 40;

    [Name("Warden Desk Drawer")]
    [Description("Adjust how much weight Warden Desk Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerWardenDeskDrawerCapacity = 10;

    [Name("Washer")]
    [Description("Adjust how much weight Washer can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerWasherCapacity = 30;

    [Name("Wood Desk Drawer")]
    [Description("Adjust how much weight Wood Desk Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerWoodDeskDrawerCapacity = 5;

    [Name("Workbench Drawer")]
    [Description("Adjust how much weight Workbench Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerWorkbenchDrawerCapacity = 5;

    #endregion

    #region Miscellaneous

    [Section("Miscellaneous")]
    [Name("'Cheat' Tweaks")]
    [Description("Enables tweaks which alters items in a way that can be seen as 'cheating'.")]
    public bool CheatingTweaks = false;

    [Name("Container Weight Tweaks")]
    [Description("Enables all tweaks related to modifying the containers weight.")]
    public bool ContainerWeightTweaks = false;

    #endregion

    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    protected override void OnConfirm()
    {
        base.OnConfirm();

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

        if (ContainerWeightTweaks && CheatingTweaks)
            SetFieldVisible(nameof(InfiniteContainerWeight), true);

        if (ContainerWeightTweaks)
        {
            SetFieldVisible(nameof(ContainerBackpackCapacity), true);
            SetFieldVisible(nameof(ContainerBriefcaseCapacity), true);
            SetFieldVisible(nameof(ContainerCabinetSmlCapacity), true);
            SetFieldVisible(nameof(ContainerCabinetLgeCapacity), true);
            SetFieldVisible(nameof(ContainerCargoContainerCapacity), true);
            SetFieldVisible(nameof(ContainerCashRegisterCapacity), true);
            SetFieldVisible(nameof(ContainerCoalBinCapacity), true);
            SetFieldVisible(nameof(ContainerCoolerCapacity), true);
            SetFieldVisible(nameof(ContainerCupboardCapacity), true);
            SetFieldVisible(nameof(ContainerDresserDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerDryerCapacity), true);
            SetFieldVisible(nameof(ContainerFileCabinetCapacity), true);
            SetFieldVisible(nameof(ContainerFirstAidCapacity), true);
            SetFieldVisible(nameof(ContainerFirewoodBinCapacity), true);
            SetFieldVisible(nameof(ContainerFishingHutDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerFreezerCapacity), true);
            SetFieldVisible(nameof(ContainerFridgeCapacity), true);
            SetFieldVisible(nameof(ContainerGloveBoxCapacity), true);
            SetFieldVisible(nameof(ContainerGunLockerCapacity), true);
            SetFieldVisible(nameof(ContainerHatchCapacity), true);
            SetFieldVisible(nameof(ContainerHiddenCacheCapacity), true);
            SetFieldVisible(nameof(ContainerInfirmaryDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerKitchenCabinetCapacity), true);
            SetFieldVisible(nameof(ContainerKitchenDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerLockBoxCapacity), true);
            SetFieldVisible(nameof(ContainerLockerCapacity), true);
            SetFieldVisible(nameof(ContainerMedicineShelfCapacity), true);
            SetFieldVisible(nameof(ContainerDeskDrawerSmlCapacity), true);
            SetFieldVisible(nameof(ContainerDeskDrawerLgeCapacity), true);
            SetFieldVisible(nameof(ContainerMetalContainerCapacity), true);
            SetFieldVisible(nameof(ContainerOvenCapacity), true);
            SetFieldVisible(nameof(ContainerPlasticContainerCapacity), true);
            SetFieldVisible(nameof(ContainerRockCacheCapacity), true);
            SetFieldVisible(nameof(ContainerSafeCapacity), true);
            SetFieldVisible(nameof(ContainerSafetyDepositBoxCapacity), true);
            SetFieldVisible(nameof(ContainerEndTableDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerSupplyBinCapacity), true);
            SetFieldVisible(nameof(ContainerSuitcaseCapacity), true);
            SetFieldVisible(nameof(ContainerToolCabinetDrawerSmlCapacity), true);
            SetFieldVisible(nameof(ContainerToolCabinetDrawerLgeCapacity), true);
            SetFieldVisible(nameof(ContainerTrashCanCapacity), true);
            SetFieldVisible(nameof(ContainerCarTrunkCapacity), true);
            SetFieldVisible(nameof(ContainerTrunkCapacity), true);
            SetFieldVisible(nameof(ContainerWardenDeskDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerWasherCapacity), true);
            SetFieldVisible(nameof(ContainerWoodDeskDrawerCapacity), true);
            SetFieldVisible(nameof(ContainerWorkbenchDrawerCapacity), true);
        }
        else
        {
            SetFieldVisible(nameof(ContainerBackpackCapacity), false);
            SetFieldVisible(nameof(ContainerBriefcaseCapacity), false);
            SetFieldVisible(nameof(ContainerCabinetSmlCapacity), false);
            SetFieldVisible(nameof(ContainerCabinetLgeCapacity), false);
            SetFieldVisible(nameof(ContainerCargoContainerCapacity), false);
            SetFieldVisible(nameof(ContainerCashRegisterCapacity), false);
            SetFieldVisible(nameof(ContainerCoalBinCapacity), false);
            SetFieldVisible(nameof(ContainerCoolerCapacity), false);
            SetFieldVisible(nameof(ContainerCupboardCapacity), false);
            SetFieldVisible(nameof(ContainerDresserDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerDryerCapacity), false);
            SetFieldVisible(nameof(ContainerFileCabinetCapacity), false);
            SetFieldVisible(nameof(ContainerFirstAidCapacity), false);
            SetFieldVisible(nameof(ContainerFirewoodBinCapacity), false);
            SetFieldVisible(nameof(ContainerFishingHutDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerFreezerCapacity), false);
            SetFieldVisible(nameof(ContainerFridgeCapacity), false);
            SetFieldVisible(nameof(ContainerGloveBoxCapacity), false);
            SetFieldVisible(nameof(ContainerGunLockerCapacity), false);
            SetFieldVisible(nameof(ContainerHatchCapacity), false);
            SetFieldVisible(nameof(ContainerHiddenCacheCapacity), false);
            SetFieldVisible(nameof(ContainerInfirmaryDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerKitchenCabinetCapacity), false);
            SetFieldVisible(nameof(ContainerKitchenDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerLockBoxCapacity), false);
            SetFieldVisible(nameof(ContainerLockerCapacity), false);
            SetFieldVisible(nameof(ContainerMedicineShelfCapacity), false);
            SetFieldVisible(nameof(ContainerDeskDrawerSmlCapacity), false);
            SetFieldVisible(nameof(ContainerDeskDrawerLgeCapacity), false);
            SetFieldVisible(nameof(ContainerMetalContainerCapacity), false);
            SetFieldVisible(nameof(ContainerOvenCapacity), false);
            SetFieldVisible(nameof(ContainerPlasticContainerCapacity), false);
            SetFieldVisible(nameof(ContainerRockCacheCapacity), false);
            SetFieldVisible(nameof(ContainerSafeCapacity), false);
            SetFieldVisible(nameof(ContainerSafetyDepositBoxCapacity), false);
            SetFieldVisible(nameof(ContainerEndTableDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerSupplyBinCapacity), false);
            SetFieldVisible(nameof(ContainerSuitcaseCapacity), false);
            SetFieldVisible(nameof(ContainerToolCabinetDrawerSmlCapacity), false);
            SetFieldVisible(nameof(ContainerToolCabinetDrawerLgeCapacity), false);
            SetFieldVisible(nameof(ContainerTrashCanCapacity), false);
            SetFieldVisible(nameof(ContainerCarTrunkCapacity), false);
            SetFieldVisible(nameof(ContainerTrunkCapacity), false);
            SetFieldVisible(nameof(ContainerWardenDeskDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerWasherCapacity), false);
            SetFieldVisible(nameof(ContainerWoodDeskDrawerCapacity), false);
            SetFieldVisible(nameof(ContainerWorkbenchDrawerCapacity), false);

            SetFieldVisible(nameof(InfiniteContainerWeight), false);
        }
    }

    internal static void OnLoad()
    {
        Instance.AddToModSettings(BuildInfo.Name);
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Mod.Logger.Log("Settings loaded.", FlaggedLoggingLevel.Debug);
    }
}
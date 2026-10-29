using ModSettings;

using UniversalTweaks.Utilities;

namespace UniversalTweaks;

/// <summary>
/// The mod's second page in the game's Mod Settings menu, with how much each kind of container holds, saved to
/// <c>Mods\UniversalTweaksContainers.json</c>. <see cref="ContainerCapacities"/> applies them.
/// </summary>
/// <remarks>
/// ModSettings shows each public field as a setting, so the fields here are public. Their names are what the settings
/// file saves, so they keep their names rather than the <c>m_</c> prefix. Each capacity's default goes through
/// <see cref="Default"/>, so a capacity left at its default can keep each container's own: see <see cref="TryGetChangedCapacity"/>.
/// </remarks>
internal sealed class ContainerSettings : JsonModSettings
{
    /// <summary>Each capacity's default, in kilograms, by field name, recorded by <see cref="Default"/>.</summary>
    /// <remarks>Declared before <see cref="Instance"/>, so it exists when the instance's field initializers run.</remarks>
    private static readonly Dictionary<string, float> Defaults = [];

    /// <summary>The capacity fields, by name, read by <see cref="TryGetChangedCapacity"/>.</summary>
    private static readonly Dictionary<string, FieldInfo> CapacityFields = [];

    /// <summary>The settings, loaded from the file when the mod starts.</summary>
    internal static readonly ContainerSettings Instance = new();

    /// <summary>Makes the settings, saved under their own file name so they don't overwrite <see cref="Settings"/>.</summary>
    private ContainerSettings() : base("UniversalTweaksContainers")
    {
    }

    #region Container Weight Tweaks

    /// <summary>Whether every container holds a practically unlimited weight. Needs <see cref="Settings.CheatingTweaks"/>.</summary>
    [Section("Container Weight Tweaks")]
    [Name("Infinite Container Weight")]
    [Description("Gives all the containers a semi-infinite weight value.")]
    public bool InfiniteContainerWeight = false;

    /// <summary>How much the backpack holds, in kilograms.</summary>
    [Name("Backpack")]
    [Description("Adjust how much weight Backpack can hold. Default is 15 KG.")]
    [Slider(0f, 30f, 31, NumberFormat = "{0:0.##} KG")]
    public float ContainerBackpackCapacity = Default(nameof(ContainerBackpackCapacity), 15);

    /// <summary>How much the briefcase holds, in kilograms.</summary>
    [Name("Briefcase")]
    [Description("Adjust how much weight Briefcase can hold. Default is 10 KG.")]
    [Slider(0f, 30f, 31, NumberFormat = "{0:0.##} KG")]
    public float ContainerBriefcaseCapacity = Default(nameof(ContainerBriefcaseCapacity), 10);

    /// <summary>How much the cabinet (small) holds, in kilograms.</summary>
    [Name("Cabinet (Small)")]
    [Description("Adjust how much weight Cabinet (Small) can hold. Default is 20 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerCabinetSmlCapacity = Default(nameof(ContainerCabinetSmlCapacity), 20);

    /// <summary>How much the cabinet (large) holds, in kilograms.</summary>
    [Name("Cabinet (Large)")]
    [Description("Adjust how much weight Cabinet (Large) can hold. Default is 40 KG.")]
    [Slider(0f, 200f, 201, NumberFormat = "{0:0.##} KG")]
    public float ContainerCabinetLgeCapacity = Default(nameof(ContainerCabinetLgeCapacity), 40);

    /// <summary>How much the cargo container holds, in kilograms.</summary>
    [Name("Cargo Container")]
    [Description("Adjust how much weight Cargo Container can hold. Default is 30 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerCargoContainerCapacity = Default(nameof(ContainerCargoContainerCapacity), 30);

    /// <summary>How much the cash register holds, in kilograms.</summary>
    [Name("Cash Register")]
    [Description("Adjust how much weight Cash Register can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerCashRegisterCapacity = Default(nameof(ContainerCashRegisterCapacity), 5);

    /// <summary>How much the coal bin holds, in kilograms.</summary>
    [Name("Coal Bin")]
    [Description("Adjust how much weight Coal Bin can hold. Default is 60 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerCoalBinCapacity = Default(nameof(ContainerCoalBinCapacity), 60);

    /// <summary>How much the cooler holds, in kilograms.</summary>
    [Name("Cooler")]
    [Description("Adjust how much weight Cooler can hold. Default is 20 KG.")]
    [Slider(0f, 150f, 151, NumberFormat = "{0:0.##} KG")]
    public float ContainerCoolerCapacity = Default(nameof(ContainerCoolerCapacity), 20);

    /// <summary>How much the cupboard holds, in kilograms.</summary>
    [Name("Cupboard")]
    [Description("Adjust how much weight Cupboard can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerCupboardCapacity = Default(nameof(ContainerCupboardCapacity), 15);

    /// <summary>How much the dresser drawer holds, in kilograms.</summary>
    [Name("Dresser Drawer")]
    [Description("Adjust how much weight Dresser Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerDresserDrawerCapacity = Default(nameof(ContainerDresserDrawerCapacity), 5);

    /// <summary>How much the dryer holds, in kilograms.</summary>
    [Name("Dryer")]
    [Description("Adjust how much weight Dryer can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerDryerCapacity = Default(nameof(ContainerDryerCapacity), 30);

    /// <summary>How much the file cabinet holds, in kilograms.</summary>
    [Name("File Cabinet")]
    [Description("Adjust how much weight File Cabinet can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerFileCabinetCapacity = Default(nameof(ContainerFileCabinetCapacity), 10);

    /// <summary>How much the first aid kit holds, in kilograms.</summary>
    [Name("First Aid Kit")]
    [Description("Adjust how much weight First Aid Kit can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerFirstAidCapacity = Default(nameof(ContainerFirstAidCapacity), 5);

    /// <summary>How much the firewood bin holds, in kilograms.</summary>
    [Name("Firewood Bin")]
    [Description("Adjust how much weight Firewood Bin can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerFirewoodBinCapacity = Default(nameof(ContainerFirewoodBinCapacity), 30);

    /// <summary>How much the fishing boat's covers and cabinet hold, in kilograms.</summary>
    [Name("Fishing Boat Storage")]
    [Description("Adjust how much weight Fishing Boat Storage can hold. Default is 20 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerFishingBoatStorageCapacity = Default(nameof(ContainerFishingBoatStorageCapacity), 20);

    /// <summary>How much the fishing hut drawer holds, in kilograms.</summary>
    [Name("Fishing Hut Drawer")]
    [Description("Adjust how much weight Fishing Hut Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerFishingHutDrawerCapacity = Default(nameof(ContainerFishingHutDrawerCapacity), 10);

    /// <summary>How much the freezer holds, in kilograms.</summary>
    [Name("Freezer")]
    [Description("Adjust how much weight Freezer can hold. Default is 20 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerFreezerCapacity = Default(nameof(ContainerFreezerCapacity), 20);

    /// <summary>How much the fridge holds, in kilograms.</summary>
    [Name("Fridge")]
    [Description("Adjust how much weight Fridge can hold. Default is 40 KG.")]
    [Slider(0f, 200f, 201, NumberFormat = "{0:0.##} KG")]
    public float ContainerFridgeCapacity = Default(nameof(ContainerFridgeCapacity), 40);

    /// <summary>How much the glove box holds, in kilograms.</summary>
    [Name("Glove Box")]
    [Description("Adjust how much weight Glove Box can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerGloveBoxCapacity = Default(nameof(ContainerGloveBoxCapacity), 5);

    /// <summary>How much the gun locker holds, in kilograms.</summary>
    [Name("Gun Locker")]
    [Description("Adjust how much weight Gun Locker can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerGunLockerCapacity = Default(nameof(ContainerGunLockerCapacity), 30);

    /// <summary>How much the hatch holds, in kilograms.</summary>
    [Name("Hatch")]
    [Description("Adjust how much weight Hatch can hold. Default is 40 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerHatchCapacity = Default(nameof(ContainerHatchCapacity), 40);

    /// <summary>How much the helicopter's storage holds, in kilograms.</summary>
    [Name("Helicopter Storage")]
    [Description("Adjust how much weight Helicopter Storage can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerHelicopterStorageCapacity = Default(nameof(ContainerHelicopterStorageCapacity), 30);

    /// <summary>How much the hidden cache holds, in kilograms.</summary>
    [Name("Hidden Cache")]
    [Description("Adjust how much weight Hidden Cache can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerHiddenCacheCapacity = Default(nameof(ContainerHiddenCacheCapacity), 15);

    /// <summary>How much the infirmary drawer holds, in kilograms.</summary>
    [Name("Infirmary Drawer")]
    [Description("Adjust how much weight Infirmary Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerInfirmaryDrawerCapacity = Default(nameof(ContainerInfirmaryDrawerCapacity), 10);

    /// <summary>How much the kitchen cabinet holds, in kilograms.</summary>
    [Name("Kitchen Cabinet")]
    [Description("Adjust how much weight Kitchen Cabinet can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerKitchenCabinetCapacity = Default(nameof(ContainerKitchenCabinetCapacity), 15);

    /// <summary>How much the kitchen drawer holds, in kilograms.</summary>
    [Name("Kitchen Drawer")]
    [Description("Adjust how much weight Kitchen Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerKitchenDrawerCapacity = Default(nameof(ContainerKitchenDrawerCapacity), 10);

    /// <summary>How much the lock box holds, in kilograms.</summary>
    [Name("Lock Box")]
    [Description("Adjust how much weight Lock Box can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerLockBoxCapacity = Default(nameof(ContainerLockBoxCapacity), 10);

    /// <summary>How much the locker holds, in kilograms.</summary>
    [Name("Locker")]
    [Description("Adjust how much weight Locker can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerLockerCapacity = Default(nameof(ContainerLockerCapacity), 30);

    /// <summary>How much the medicine shelf holds, in kilograms.</summary>
    [Name("Medicine Shelf")]
    [Description("Adjust how much weight Medicine Shelf can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerMedicineShelfCapacity = Default(nameof(ContainerMedicineShelfCapacity), 5);

    /// <summary>How much the metal desk drawer (small) holds, in kilograms.</summary>
    [Name("Metal Desk Drawer (Small)")]
    [Description("Adjust how much weight Metal Desk Drawer (Small) can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerDeskDrawerSmlCapacity = Default(nameof(ContainerDeskDrawerSmlCapacity), 5);

    /// <summary>How much the metal desk drawer (large) holds, in kilograms.</summary>
    [Name("Metal Desk Drawer (Large)")]
    [Description("Adjust how much weight Metal Desk Drawer (Large) can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerDeskDrawerLgeCapacity = Default(nameof(ContainerDeskDrawerLgeCapacity), 10);

    /// <summary>How much the metal container holds, in kilograms.</summary>
    [Name("Metal Container")]
    [Description("Adjust how much weight Metal Container can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerMetalContainerCapacity = Default(nameof(ContainerMetalContainerCapacity), 15);

    /// <summary>How much the oven holds, in kilograms.</summary>
    [Name("Oven")]
    [Description("Adjust how much weight Oven can hold. Default is 40 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerOvenCapacity = Default(nameof(ContainerOvenCapacity), 40);

    /// <summary>How much the plastic container holds, in kilograms.</summary>
    [Name("Plastic Container")]
    [Description("Adjust how much weight Plastic Container can hold. Default is 15 KG.")]
    [Slider(0f, 30f, 31, NumberFormat = "{0:0.##} KG")]
    public float ContainerPlasticContainerCapacity = Default(nameof(ContainerPlasticContainerCapacity), 15);

    /// <summary>How much the potato sack holds, in kilograms.</summary>
    [Name("Potato Sack")]
    [Description("Adjust how much weight Potato Sack can hold. Default is 2 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerPotatoSackCapacity = Default(nameof(ContainerPotatoSackCapacity), 2);

    /// <summary>How much the rock cache holds, in kilograms.</summary>
    [Name("Rock Cache")]
    [Description("Adjust how much weight Rock Cache can hold. Default is 30 KG.")]
    [Slider(0f, 100f, 101, NumberFormat = "{0:0.##} KG")]
    public float ContainerRockCacheCapacity = Default(nameof(ContainerRockCacheCapacity), 30);

    /// <summary>How much the rustic storage box holds, in kilograms.</summary>
    [Name("Rustic Storage Box")]
    [Description("Adjust how much weight Rustic Storage Box can hold. Default is 15 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerRusticStorageBoxCapacity = Default(nameof(ContainerRusticStorageBoxCapacity), 15);

    /// <summary>How much the safe holds, in kilograms.</summary>
    [Name("Safe")]
    [Description("Adjust how much weight Safe can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerSafeCapacity = Default(nameof(ContainerSafeCapacity), 10);

    /// <summary>How much the safety deposit box holds, in kilograms.</summary>
    [Name("Safety Deposit Box")]
    [Description("Adjust how much weight Safety Deposit Box can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerSafetyDepositBoxCapacity = Default(nameof(ContainerSafetyDepositBoxCapacity), 5);

    /// <summary>How much the side table drawer holds, in kilograms.</summary>
    [Name("Side Table Drawer")]
    [Description("Adjust how much weight Side Table Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerEndTableDrawerCapacity = Default(nameof(ContainerEndTableDrawerCapacity), 5);

    /// <summary>How much the snowmobile's compartment holds, in kilograms.</summary>
    [Name("Snowmobile Compartment")]
    [Description("Adjust how much weight Snowmobile Compartment can hold. Default is 40 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerSnowmobileCompartmentCapacity = Default(nameof(ContainerSnowmobileCompartmentCapacity), 40);

    /// <summary>How much the supply bin holds, in kilograms.</summary>
    [Name("Supply Bin")]
    [Description("Adjust how much weight Supply Bin can hold. Default is 30 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerSupplyBinCapacity = Default(nameof(ContainerSupplyBinCapacity), 30);

    /// <summary>How much the suitcase holds, in kilograms.</summary>
    [Name("Suitcase")]
    [Description("Adjust how much weight Suitcase can hold. Default is 20 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerSuitcaseCapacity = Default(nameof(ContainerSuitcaseCapacity), 20);

    /// <summary>How much the tool cabinet drawer (small) holds, in kilograms.</summary>
    [Name("Tool Cabinet Drawer (Small)")]
    [Description("Adjust how much weight Tool Cabinet Drawer (Small) can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerToolCabinetDrawerSmlCapacity = Default(nameof(ContainerToolCabinetDrawerSmlCapacity), 5);

    /// <summary>How much the tool cabinet drawer (large) holds, in kilograms.</summary>
    [Name("Tool Cabinet Drawer (Large)")]
    [Description("Adjust how much weight Tool Cabinet Drawer (Large) can hold. Default is 10 KG.")]
    [Slider(0f, 100f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerToolCabinetDrawerLgeCapacity = Default(nameof(ContainerToolCabinetDrawerLgeCapacity), 10);

    /// <summary>How much the train cabin's storage holds, in kilograms.</summary>
    [Name("Train Cabin Storage")]
    [Description("Adjust how much weight Train Cabin Storage can hold. Default is 45 KG.")]
    [Slider(0f, 250f, 251, NumberFormat = "{0:0.##} KG")]
    public float ContainerTrainCabinStorageCapacity = Default(nameof(ContainerTrainCabinStorageCapacity), 45);

    /// <summary>How much the trash can holds, in kilograms.</summary>
    [Name("Trash Can")]
    [Description("Adjust how much weight Trash Can can hold. Default is 15 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerTrashCanCapacity = Default(nameof(ContainerTrashCanCapacity), 15);

    /// <summary>How much the trunk (car) holds, in kilograms.</summary>
    [Name("Trunk (Car)")]
    [Description("Adjust how much weight Trunk (Car) can hold. Default is 40 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerCarTrunkCapacity = Default(nameof(ContainerCarTrunkCapacity), 40);

    /// <summary>How much the trunk (rustic storage) holds, in kilograms.</summary>
    [Name("Trunk (Rustic Storage)")]
    [Description("Adjust how much weight the Rustic Storage Trunk can hold. Default is 35 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerRusticStorageTrunkCapacity = Default(nameof(ContainerRusticStorageTrunkCapacity), 35);

    /// <summary>How much the trunk (wooden box) holds, in kilograms.</summary>
    [Name("Trunk (Wooden Box)")]
    [Description("Adjust how much weight Trunk (Wooden Box) can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerTrunkCapacity = Default(nameof(ContainerTrunkCapacity), 30);

    /// <summary>How much the warden desk drawer holds, in kilograms.</summary>
    [Name("Warden Desk Drawer")]
    [Description("Adjust how much weight Warden Desk Drawer can hold. Default is 10 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerWardenDeskDrawerCapacity = Default(nameof(ContainerWardenDeskDrawerCapacity), 10);

    /// <summary>How much the washer holds, in kilograms.</summary>
    [Name("Washer")]
    [Description("Adjust how much weight Washer can hold. Default is 30 KG.")]
    [Slider(0f, 500f, 501, NumberFormat = "{0:0.##} KG")]
    public float ContainerWasherCapacity = Default(nameof(ContainerWasherCapacity), 30);

    /// <summary>How much the wood desk drawer holds, in kilograms.</summary>
    [Name("Wood Desk Drawer")]
    [Description("Adjust how much weight Wood Desk Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 50f, 51, NumberFormat = "{0:0.##} KG")]
    public float ContainerWoodDeskDrawerCapacity = Default(nameof(ContainerWoodDeskDrawerCapacity), 5);

    /// <summary>How much the workbench drawer holds, in kilograms.</summary>
    [Name("Workbench Drawer")]
    [Description("Adjust how much weight Workbench Drawer can hold. Default is 5 KG.")]
    [Slider(0f, 25f, 26, NumberFormat = "{0:0.##} KG")]
    public float ContainerWorkbenchDrawerCapacity = Default(nameof(ContainerWorkbenchDrawerCapacity), 5);

    #endregion

    /// <summary>
    /// Gets a capacity setting's value, if the player has changed it from its default. Left at its default, each
    /// container keeps its own capacity, since containers that share a setting don't all hold the same in the game.
    /// </summary>
    /// <param name="fieldName">The capacity field's name.</param>
    /// <param name="capacity">The capacity, in kilograms, if it has been changed.</param>
    /// <returns><see langword="true"/> if the setting is a capacity and isn't at its default.</returns>
    internal bool TryGetChangedCapacity(string fieldName, out float capacity)
    {
        capacity = 0f;
        if (!Defaults.TryGetValue(fieldName, out var defaultCapacity))
            return false;

        if (!CapacityFields.TryGetValue(fieldName, out var field))
            CapacityFields[fieldName] = field = typeof(ContainerSettings).GetField(fieldName)!;

        capacity = (float)field.GetValue(this)!;
        return Math.Abs(capacity - defaultCapacity) > 0.001f;
    }

    /// <summary>Records a capacity's default for <see cref="TryGetChangedCapacity"/>, as its field is made.</summary>
    /// <param name="fieldName">The capacity field's name.</param>
    /// <param name="capacity">The default, in kilograms.</param>
    /// <returns><paramref name="capacity"/>.</returns>
    private static float Default(string fieldName, float capacity)
    {
        Defaults[fieldName] = capacity;
        return capacity;
    }

    /// <summary>Adds the settings to the Mod Settings menu, on a page of their own.</summary>
    internal static void Initialize()
    {
        Instance.AddToModSettings($"{Properties.BuildInfo.Name} - Containers");
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Mod.Logger.Log("Container settings loaded.", FlaggedLoggingLevel.Debug);
    }

    /// <inheritdoc />
    /// <remarks>Applies the capacities to the containers that already exist.</remarks>
    protected override void OnConfirm()
    {
        base.OnConfirm();
        ContainerCapacities.Refresh();
    }

    /// <inheritdoc />
    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    /// <summary>
    /// Shows infinite container weight only with 'Cheat' Tweaks on, and the capacities only while infinite container
    /// weight is off.
    /// </summary>
    internal void RefreshFields()
    {
        SetFieldVisible(nameof(InfiniteContainerWeight), Settings.Instance.CheatingTweaks);

        var showCapacities = !InfiniteContainerWeight;

        foreach (var capacity in Defaults.Keys)
            SetFieldVisible(capacity, showCapacities);
    }
}

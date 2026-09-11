using ModSettings;

namespace UniversalTweaks.Properties;

internal class ContainerSettings : JsonModSettings
{
    internal static ContainerSettings Instance { get; } = new();

    private ContainerSettings() : base("UniversalTweaksContainers")
    {
    }

    #region Container Weight Tweaks

    [Section("Container Weight Tweaks")]
    [Name("Container Weight Tweaks")]
    [Description("Enables all tweaks related to modifying the containers weight.")]
    public bool ContainerWeightTweaks = false;

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

    protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();

    internal void RefreshFields()
    {
        if (ContainerWeightTweaks && Settings.Instance.CheatingTweaks)
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
        Instance.AddToModSettings($"{BuildInfo.Name} - Containers");
        Instance.RefreshFields();
        Instance.RefreshGUI();

        Mod.Logger.Log("Container settings loaded.", FlaggedLoggingLevel.Debug);
    }
}
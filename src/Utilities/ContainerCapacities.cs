using Il2CppTLD.IntBackedUnit;

namespace UniversalTweaks.Utilities;

/// <summary>
/// Sets how much weight each container can hold, from <see cref="ContainerSettings"/>.
/// </summary>
/// <remarks>
/// <see cref="Patches.ContainerPatches"/> registers each container as it wakes, so <see cref="Refresh"/> can reach the
/// ones already in the scene when the player confirms a change.
/// </remarks>
internal static class ContainerCapacities
{
    /// <summary>The capacity setting of each kind of container, by part of its name.</summary>
    /// <remarks>
    /// Matched in order against the container's name; the first fragment it contains wins. The editor script
    /// tools/UnityEditor/FindContainers.cs reads this list as text, so keep each entry a quoted name fragment followed by the setting's <c>nameof</c>.
    /// </remarks>
    private static readonly (string NameFragment, string Setting)[] Capacities =
    [
        ("CarSedanGloveBox_Prefab", nameof(ContainerSettings.ContainerGloveBoxCapacity)),
        ("CarSedanTrunkDoor_Prefab", nameof(ContainerSettings.ContainerCarTrunkCapacity)),
        ("CarTruckGloveBox_Prefab", nameof(ContainerSettings.ContainerGloveBoxCapacity)),
        ("CONTAINER_BackPack", nameof(ContainerSettings.ContainerBackpackCapacity)),
        ("CONTAINER_BathroomCabinet", nameof(ContainerSettings.ContainerCabinetLgeCapacity)),
        ("CONTAINER_Briefcase", nameof(ContainerSettings.ContainerBriefcaseCapacity)),
        ("CONTAINER_CacheStoreCommon", nameof(ContainerSettings.ContainerHiddenCacheCapacity)),
        ("CONTAINER_CacheStoreRare", nameof(ContainerSettings.ContainerPlasticContainerCapacity)),
        ("CONTAINER_CoalBin", nameof(ContainerSettings.ContainerCoalBinCapacity)),
        ("CONTAINER_Cooler", nameof(ContainerSettings.ContainerCoolerCapacity)),
        ("CONTAINER_Dryer", nameof(ContainerSettings.ContainerDryerCapacity)),
        ("CONTAINER_FirewoodBin", nameof(ContainerSettings.ContainerFirewoodBinCapacity)),
        ("CONTAINER_FirstAidKit", nameof(ContainerSettings.ContainerFirstAidCapacity)),
        ("CONTAINER_StorageGunLocker", nameof(ContainerSettings.ContainerGunLockerCapacity)),
        ("CONTAINER_ForestryCrate", nameof(ContainerSettings.ContainerSupplyBinCapacity)),
        ("CONTAINER_LargeCabinet", nameof(ContainerSettings.ContainerCabinetLgeCapacity)),
        ("CONTAINER_LilysChest", nameof(ContainerSettings.ContainerTrunkCapacity)),
        ("CONTAINER_Rustic_Storage_Trunk", nameof(ContainerSettings.ContainerRusticStorageTrunkCapacity)),
        ("CONTAINER_LockBoxB", nameof(ContainerSettings.ContainerLockBoxCapacity)),
        ("CONTAINER_LockerA", nameof(ContainerSettings.ContainerLockerCapacity)),
        ("CONTAINER_MedicineShelf", nameof(ContainerSettings.ContainerMedicineShelfCapacity)),
        ("CONTAINER_MetalBox", nameof(ContainerSettings.ContainerMetalContainerCapacity)),
        ("CONTAINER_MetalLocker", nameof(ContainerSettings.ContainerLockerCapacity)),
        ("CONTAINER_PlasticBox", nameof(ContainerSettings.ContainerPlasticContainerCapacity)),
        ("CONTAINER_Safe", nameof(ContainerSettings.ContainerSafeCapacity)),
        ("CONTAINER_SmallCabinet", nameof(ContainerSettings.ContainerCabinetSmlCapacity)),
        ("CONTAINER_SteamerTrunk", nameof(ContainerSettings.ContainerTrunkCapacity)),
        ("CONTAINER_StoneCabinATrapDoor", nameof(ContainerSettings.ContainerHatchCapacity)),
        ("CONTAINER_TrashCanister", nameof(ContainerSettings.ContainerTrashCanCapacity)),
        ("CONTAINER_Washer", nameof(ContainerSettings.ContainerWasherCapacity)),
        ("GEAR_RockCache_Prefab", nameof(ContainerSettings.ContainerRockCacheCapacity)),
        ("OBJ_CargoCrateBottomDoor", nameof(ContainerSettings.ContainerCargoContainerCapacity)),
        ("OBJ_CargoCrateTopDoor", nameof(ContainerSettings.ContainerCargoContainerCapacity)),
        ("OBJ_CashRegisterDrawer", nameof(ContainerSettings.ContainerCashRegisterCapacity)),
        ("OBJ_DresserDrawer", nameof(ContainerSettings.ContainerDresserDrawerCapacity)),
        ("OBJ_DresserTallDrawer", nameof(ContainerSettings.ContainerDresserDrawerCapacity)),
        ("OBJ_EndTableDrawer", nameof(ContainerSettings.ContainerEndTableDrawerCapacity)),
        ("OBJ_CupboardDoor", nameof(ContainerSettings.ContainerCupboardCapacity)),
        ("OBJ_FishingCabinCupboardDoor", nameof(ContainerSettings.ContainerCupboardCapacity)),
        ("OBJ_FishingCabinDresserDrawer", nameof(ContainerSettings.ContainerFishingHutDrawerCapacity)),
        ("OBJ_FridgeBottomDoor", nameof(ContainerSettings.ContainerFridgeCapacity)),
        ("OBJ_FridgeTopDoor", nameof(ContainerSettings.ContainerFreezerCapacity)),
        ("OBJ_InfirmaryDrawer", nameof(ContainerSettings.ContainerInfirmaryDrawerCapacity)),
        ("OBJ_GasOvenDoor", nameof(ContainerSettings.ContainerOvenCapacity)),
        ("OBJ_KitchenCabinetDoor", nameof(ContainerSettings.ContainerKitchenCabinetCapacity)),
        ("OBJ_KitchenDrawer", nameof(ContainerSettings.ContainerKitchenDrawerCapacity)),
        ("OBJ_MetalDeskDrawer1", nameof(ContainerSettings.ContainerDeskDrawerLgeCapacity)),
        ("OBJ_MetalDeskDrawer4", nameof(ContainerSettings.ContainerDeskDrawerLgeCapacity)),
        ("OBJ_MetalDeskDrawer2", nameof(ContainerSettings.ContainerDeskDrawerSmlCapacity)),
        ("OBJ_MetalDeskDrawer3", nameof(ContainerSettings.ContainerDeskDrawerSmlCapacity)),
        ("OBJ_MetalFileCabinetDrawer", nameof(ContainerSettings.ContainerFileCabinetCapacity)),
        ("OBJ_WorkBenchDrawer", nameof(ContainerSettings.ContainerWorkbenchDrawerCapacity)),
        ("OBJ_MetalLockerDoor", nameof(ContainerSettings.ContainerLockerCapacity)),
        ("OBJ_SmallCabinetDoor", nameof(ContainerSettings.ContainerCabinetSmlCapacity)),
        ("OBJ_Suitcase", nameof(ContainerSettings.ContainerSuitcaseCapacity)),
        ("OBJ_ToolCabinetDrawerE", nameof(ContainerSettings.ContainerToolCabinetDrawerLgeCapacity)),
        ("OBJ_ToolCabinetDrawer", nameof(ContainerSettings.ContainerToolCabinetDrawerSmlCapacity)),
        ("OBJ_WardenDesk", nameof(ContainerSettings.ContainerWardenDeskDrawerCapacity)),
        ("OBJ_TrailerInteriorDeskDrawerLg_Prefab", nameof(ContainerSettings.ContainerWoodDeskDrawerCapacity)),
        ("STR_BankAVaultDepositBox", nameof(ContainerSettings.ContainerSafetyDepositBoxCapacity)),
        ("CONTAINER_LargeKitchenCabinet", nameof(ContainerSettings.ContainerCabinetLgeCapacity)),
        ("CONTAINER_MedicalLocker", nameof(ContainerSettings.ContainerLockerCapacity)),
        ("CONTAINER_PotatoSack", nameof(ContainerSettings.ContainerPotatoSackCapacity)),
        ("CONTAINER_StageStorage", nameof(ContainerSettings.ContainerCabinetLgeCapacity)),
        ("CONTAINER_TrainCabinStorage", nameof(ContainerSettings.ContainerTrainCabinStorageCapacity)),
        ("OBJ_AmmoCraftingBench_Drawer", nameof(ContainerSettings.ContainerWorkbenchDrawerCapacity)),
        ("OBJ_BunkerToolStorage", nameof(ContainerSettings.ContainerToolCabinetDrawerSmlCapacity)),
        ("OBJ_Desk_Home_Drawer", nameof(ContainerSettings.ContainerWoodDeskDrawerCapacity)),
        ("OBJ_Helicopter_A_", nameof(ContainerSettings.ContainerHelicopterStorageCapacity)),
        ("OBJ_InfirmaryCabinetDoor", nameof(ContainerSettings.ContainerCabinetSmlCapacity)),
        ("OBJ_InfirmaryRefrigeratorDoor", nameof(ContainerSettings.ContainerFridgeCapacity)),
        ("OBJ_KitchenCabinet_BleachedWood_Door", nameof(ContainerSettings.ContainerKitchenCabinetCapacity)),
        ("OBJ_Metal_Medical_FileCabinet_Drawer", nameof(ContainerSettings.ContainerFileCabinetCapacity)),
        ("OBJ_MiniFridgeDoor", nameof(ContainerSettings.ContainerFridgeCapacity)),
        ("OBJ_Rustic_Closet_Door", nameof(ContainerSettings.ContainerCabinetSmlCapacity)),
        ("OBJ_Rustic_Closet_Drawer", nameof(ContainerSettings.ContainerDresserDrawerCapacity)),
        ("OBJ_Rustic_Display_Shelf_Drawer", nameof(ContainerSettings.ContainerDresserDrawerCapacity)),
        ("OBJ_Rustic_Dresser_Drawer", nameof(ContainerSettings.ContainerDresserDrawerCapacity)),
        ("OBJ_Rustic_End_Table_Door", nameof(ContainerSettings.ContainerCabinetSmlCapacity)),
        ("OBJ_Rustic_End_Table_Drawer", nameof(ContainerSettings.ContainerEndTableDrawerCapacity)),
        ("OBJ_Rustic_Storage_Box_Lid", nameof(ContainerSettings.ContainerRusticStorageBoxCapacity)),
        ("OBJ_SmallFishingBoat", nameof(ContainerSettings.ContainerFishingBoatStorageCapacity)),
        ("OBJ_Snowmobile_A_Compartment", nameof(ContainerSettings.ContainerSnowmobileCompartmentCapacity)),
        ("OBJ_Storage_Wooden_A_Drawer", nameof(ContainerSettings.ContainerKitchenDrawerCapacity)),
        ("OBJ_WoodDeskDrawer", nameof(ContainerSettings.ContainerWoodDeskDrawerCapacity)),
        ("OBJ_WoodPullout", nameof(ContainerSettings.ContainerDresserDrawerCapacity)),
    ];

    /// <summary>
    /// The game's own capacity of each container, kept while its setting is at its default or it has none, and put back
    /// when infinite container weight is turned off.
    /// </summary>
    private static readonly ObjectStateCache<Container, ItemWeight> OriginalCapacities = new();

    /// <summary>Remembers a container's own capacity and sets it from the settings.</summary>
    /// <param name="container">A container that has just woken.</param>
    internal static void Register(Container container)
    {
        OriginalCapacities.Set(container, container.m_Capacity);
        Apply(container);

        Mod.Logger.Log($"Container '{container.name}' capacity set to {container.m_Capacity}.", FlaggedLoggingLevel.Trace);
    }

    /// <summary>Sets the capacity of every registered container from the settings again.</summary>
    /// <remarks>Only containers that went through <c>Awake</c> are touched, which leaves prefabs as the game made them.</remarks>
    internal static void Refresh()
    {
        var refreshed = 0;
        foreach (var container in OriginalCapacities.Owners)
        {
            if (container != null)
            {
                Apply(container);
                refreshed++;
            }
        }

        Mod.Logger.Log($"Refreshed the capacity of {refreshed} container(s).", FlaggedLoggingLevel.Debug);
    }

    /// <summary>
    /// Sets a container's capacity: infinite, from its setting if the player changed it, or its own if the setting is
    /// at its default or it has none.
    /// </summary>
    /// <param name="container">The container.</param>
    private static void Apply(Container container)
    {
        var settings = ContainerSettings.Instance;

        if (Settings.Instance.CheatingTweaks && settings.InfiniteContainerWeight)
        {
            container.m_Capacity = ItemWeight.FromKilograms(10000);
            return;
        }

        var hasSetting = TryGetSetting(container.name, out var setting);
        if (hasSetting && settings.TryGetChangedCapacity(setting, out var capacity))
        {
            container.m_Capacity = ItemWeight.FromKilograms(capacity);
        }
        else if (OriginalCapacities.TryGet(container, out var originalCapacity))
        {
            container.m_Capacity = originalCapacity;
            if (!hasSetting)
                Mod.Logger.Log($"Found container '{container.name}' that is missing from the known containers list.", FlaggedLoggingLevel.Debug);
        }
    }

    /// <summary>Gets the capacity setting for a container, by its name.</summary>
    /// <param name="containerName">The container's name.</param>
    /// <param name="setting">The name of the container's <see cref="ContainerSettings"/> field, if it has one.</param>
    /// <returns><see langword="true"/> if it has a setting.</returns>
    private static bool TryGetSetting(string containerName, out string setting)
    {
        foreach (var (nameFragment, settingName) in Capacities)
        {
            if (containerName.Contains(nameFragment))
            {
                setting = settingName;
                return true;
            }
        }

        setting = "";
        return false;
    }
}

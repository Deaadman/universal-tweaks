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
    // TODO: Make sure that all of these containers are up-to-date, it has been awhile since it was last touched.
    /// <summary>The capacity of each kind of container, by part of its name.</summary>
    /// <remarks>Matched in order against the container's name; the first fragment it contains wins.</remarks>
    private static readonly (string NameFragment, Func<ContainerSettings, float> Capacity)[] Capacities =
    [
        ("CarSedanGloveBox_Prefab", s => s.ContainerGloveBoxCapacity),
        ("CarSedanTrunkDoor_Prefab", s => s.ContainerCarTrunkCapacity),
        ("CarTruckGloveBox_Prefab", s => s.ContainerGloveBoxCapacity),
        ("CONTAINER_BackPack", s => s.ContainerBackpackCapacity),
        ("CONTAINER_BathroomCabinet", s => s.ContainerCabinetLgeCapacity),
        ("CONTAINER_Briefcase", s => s.ContainerBriefcaseCapacity),
        ("CONTAINER_CacheStoreCommon", s => s.ContainerHiddenCacheCapacity),
        ("CONTAINER_CacheStoreRare", s => s.ContainerPlasticContainerCapacity),
        ("CONTAINER_CoalBin", s => s.ContainerCoalBinCapacity),
        ("CONTAINER_Cooler", s => s.ContainerCoolerCapacity),
        ("CONTAINER_Dryer", s => s.ContainerDryerCapacity),
        ("CONTAINER_FirewoodBin", s => s.ContainerFirewoodBinCapacity),
        ("CONTAINER_FirstAidKit", s => s.ContainerFirstAidCapacity),
        ("CONTAINER_StorageGunLocker", s => s.ContainerGunLockerCapacity),
        ("CONTAINER_ForestryCrate", s => s.ContainerSupplyBinCapacity),
        ("CONTAINER_LargeCabinet", s => s.ContainerCabinetLgeCapacity),
        ("CONTAINER_LilysChest", s => s.ContainerTrunkCapacity),
        ("CONTAINER_Rustic_Storage_Trunk", s => s.ContainerRusticStorageTrunkCapacity),
        ("CONTAINER_LockBoxB", s => s.ContainerLockBoxCapacity),
        ("CONTAINER_LockerA", s => s.ContainerLockerCapacity),
        ("CONTAINER_MedicineShelf", s => s.ContainerMedicineShelfCapacity),
        ("CONTAINER_MetalBox", s => s.ContainerMetalContainerCapacity),
        ("CONTAINER_MetalLocker", s => s.ContainerLockerCapacity),
        ("CONTAINER_PlasticBox", s => s.ContainerPlasticContainerCapacity),
        ("CONTAINER_Safe", s => s.ContainerSafeCapacity),
        ("CONTAINER_SmallCabinet", s => s.ContainerCabinetSmlCapacity),
        ("CONTAINER_SteamerTrunk", s => s.ContainerTrunkCapacity),
        ("CONTAINER_StoneCabinATrapDoor", s => s.ContainerHatchCapacity),
        ("CONTAINER_TrashCanister", s => s.ContainerTrashCanCapacity),
        ("CONTAINER_Washer", s => s.ContainerWasherCapacity),
        ("GEAR_RockCache_Prefab", s => s.ContainerRockCacheCapacity),
        ("OBJ_CargoCrateBottomDoor", s => s.ContainerCargoContainerCapacity),
        ("OBJ_CargoCrateTopDoor", s => s.ContainerCargoContainerCapacity),
        ("OBJ_CashRegisterDrawer", s => s.ContainerCashRegisterCapacity),
        ("OBJ_DresserDrawer", s => s.ContainerDresserDrawerCapacity),
        ("OBJ_DresserTallDrawer", s => s.ContainerDresserDrawerCapacity),
        ("OBJ_EndTableDrawer", s => s.ContainerEndTableDrawerCapacity),
        ("OBJ_CupboardDoor", s => s.ContainerCupboardCapacity),
        ("OBJ_FishingCabinCupboardDoor", s => s.ContainerCupboardCapacity),
        ("OBJ_FishingCabinDresserDrawer", s => s.ContainerFishingHutDrawerCapacity),
        ("OBJ_FridgeBottomDoor", s => s.ContainerFridgeCapacity),
        ("OBJ_FridgeTopDoor", s => s.ContainerFreezerCapacity),
        ("OBJ_InfirmaryDrawer", s => s.ContainerInfirmaryDrawerCapacity),
        ("OBJ_GasOvenDoor", s => s.ContainerOvenCapacity),
        ("OBJ_KitchenCabinetDoor", s => s.ContainerKitchenCabinetCapacity),
        ("OBJ_KitchenDrawer", s => s.ContainerKitchenDrawerCapacity),
        ("OBJ_MetalDeskDrawer1", s => s.ContainerDeskDrawerLgeCapacity),
        ("OBJ_MetalDeskDrawer4", s => s.ContainerDeskDrawerLgeCapacity),
        ("OBJ_MetalDeskDrawer2", s => s.ContainerDeskDrawerSmlCapacity),
        ("OBJ_MetalDeskDrawer3", s => s.ContainerDeskDrawerSmlCapacity),
        ("OBJ_MetalFileCabinetDrawer", s => s.ContainerFileCabinetCapacity),
        ("OBJ_WorkBenchDrawer", s => s.ContainerWorkbenchDrawerCapacity),
        ("OBJ_MetalLockerDoor", s => s.ContainerLockerCapacity),
        ("OBJ_SmallCabinetDoor", s => s.ContainerCabinetSmlCapacity),
        ("OBJ_Suitcase", s => s.ContainerSuitcaseCapacity),
        ("OBJ_ToolCabinetDrawerE", s => s.ContainerToolCabinetDrawerLgeCapacity),
        ("OBJ_ToolCabinetDrawer", s => s.ContainerToolCabinetDrawerSmlCapacity),
        ("OBJ_WardenDesk", s => s.ContainerWardenDeskDrawerCapacity),
        ("OBJ_TrailerInteriorDeskDrawerLg_Prefab", s => s.ContainerWoodDeskDrawerCapacity),
        ("STR_BankAVaultDepositBox", s => s.ContainerSafetyDepositBoxCapacity),
    ];

    /// <summary>
    /// The game's own capacity of each container, so ones missing from <see cref="Capacities"/> can be put back when
    /// infinite container weight is turned off.
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

    /// <summary>Sets a container's capacity: infinite, from its setting, or its own if it has no setting.</summary>
    /// <param name="container">The container.</param>
    private static void Apply(Container container)
    {
        var settings = ContainerSettings.Instance;

        if (Settings.Instance.CheatingTweaks && settings.InfiniteContainerWeight)
        {
            container.m_Capacity = ItemWeight.FromKilograms(10000);
        }
        else if (TryGetCapacity(container.name, settings, out var capacity))
        {
            container.m_Capacity = ItemWeight.FromKilograms(capacity);
        }
        else if (OriginalCapacities.TryGet(container, out var originalCapacity))
        {
            container.m_Capacity = originalCapacity;
            Mod.Logger.Log($"Found container '{container.name}' that is missing from the known containers list.", FlaggedLoggingLevel.Debug);
        }
    }

    /// <summary>Gets the capacity setting for a container, by its name.</summary>
    /// <param name="containerName">The container's name.</param>
    /// <param name="settings">The container settings.</param>
    /// <param name="capacity">The capacity, in kilograms, if the container has a setting.</param>
    /// <returns><see langword="true"/> if it has a setting.</returns>
    private static bool TryGetCapacity(string containerName, ContainerSettings settings, out float capacity)
    {
        foreach (var (nameFragment, getCapacity) in Capacities)
        {
            if (containerName.Contains(nameFragment))
            {
                capacity = getCapacity(settings);
                return true;
            }
        }

        capacity = 0f;
        return false;
    }
}

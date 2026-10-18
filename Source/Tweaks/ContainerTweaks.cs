using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

// TODO: Make sure that all of these containers are up-to-date, it has been awhile since it was last touched.
internal static class ContainerTweaks
{
    // Matched in order against the container's name; the first fragment it contains wins.
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

    // Vanilla capacity per container instance, so unlisted containers can be restored when infinite weight is turned off.
    private static readonly Dictionary<int, ItemWeight> OriginalCapacities = new();

    [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
    private static class AdjustContainerCapacityAmount
    {
        private static void Postfix(Container __instance)
        {
            OriginalCapacities[__instance.GetInstanceID()] = __instance.m_Capacity;
            ApplyCapacity(__instance);

            Mod.Logger.Log($"Container '{__instance.name}' capacity set to {__instance.m_Capacity}.", FlaggedLoggingLevel.Trace);
        }
    }

    internal static void RefreshContainerCapacities()
    {
        // Only containers that went through Awake are touched, which skips prefabs and leaves their vanilla values intact.
        var refreshed = 0;
        foreach (var container in Resources.FindObjectsOfTypeAll<Container>())
        {
            if (OriginalCapacities.ContainsKey(container.GetInstanceID()))
            {
                ApplyCapacity(container);
                refreshed++;
            }
        }

        Mod.Logger.Log($"Refreshed the capacity of {refreshed} container(s).", FlaggedLoggingLevel.Debug);
    }

    private static void ApplyCapacity(Container container)
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
        else if (OriginalCapacities.TryGetValue(container.GetInstanceID(), out var originalCapacity))
        {
            container.m_Capacity = originalCapacity;
            Mod.Logger.Log($"Found container '{container.name}' that is missing from the known containers list.", FlaggedLoggingLevel.Debug);
        }
    }

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
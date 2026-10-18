using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

// This entire class needs to be revisited with the following changes...
// Create a dedicated method for updating these prefabs, instantly update them like we are with the encumbering tweaks.
// Make sure that all of these containers are up-to-date, it has been awhile since it was last touched.
internal static class Container
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

    [HarmonyPatch(typeof(Il2Cpp.Container), nameof(Il2Cpp.Container.Awake))]
    private static class AdjustContainerCapacityAmount
    {
        private static void Postfix(Il2Cpp.Container __instance)
        {
            var settings = ContainerSettings.Instance;

            if (settings.InfiniteContainerWeight)
            {
                __instance.m_Capacity = ItemWeight.FromKilograms(10000);
            }
            else if (TryGetCapacity(__instance.name, settings, out var capacity))
            {
                __instance.m_Capacity = ItemWeight.FromKilograms(capacity);
            }
            else
            {
                Mod.Logger.Log($"Found container '{__instance.name}' that is missing from the known containers list.", FlaggedLoggingLevel.Debug);
            }

            Mod.Logger.Log($"Container '{__instance.name}' capacity set to {__instance.m_Capacity}.", FlaggedLoggingLevel.Trace);
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
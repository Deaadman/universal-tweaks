using Il2CppTLD.IntBackedUnit;
using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

// This entire class needs to be revisited with the following changes...
// Make sure that all of these containers are up-to-date, it has been awhile since it was last touched.
internal static class Container
{
    [HarmonyPatch(typeof(Il2Cpp.Container), nameof(Il2Cpp.Container.BeginContainerOpen))]
    private class AdjustCapacityOnOpen
    {
        public static void Postfix(Il2Cpp.Container __instance)
        {
            float? capacity = GetCapacity(__instance.name);
            if (capacity.HasValue)
            {
                __instance.m_Capacity = ItemWeight.FromKilograms(capacity.Value);
            }
        }
    }

    [HarmonyPatch(typeof(Il2Cpp.Container), nameof(Il2Cpp.Container.Awake))]
    private class AdjustCapacityOnAwake
    {
        private static void Postfix(Il2Cpp.Container __instance)
        {
            float? capacity = GetCapacity(__instance.name);
            if (capacity.HasValue)
            {
                __instance.m_Capacity = ItemWeight.FromKilograms(capacity.Value);
            }
        }
    }

    /// <summary>Get the capacity in kilograms to set for a container.</summary>
    /// <param name="name">The container's internal name.</param>
    public static float? GetCapacity(string name)
    {
        if (Settings.Instance.InfiniteContainerWeight)
        {
            return 10000;
        }

        // Glove Box 1
        if (name.Contains("CarSedanGloveBox_Prefab"))
        {
            return Settings.Instance.ContainerGloveBoxCapacity;
        }

        // Trunk (Car)
        if (name.Contains("CarSedanTrunkDoor_Prefab"))
        {
            return Settings.Instance.ContainerCarTrunkCapacity;
        }

        // Glove Box 2
        if (name.Contains("CarTruckGloveBox_Prefab"))
        {
            return Settings.Instance.ContainerGloveBoxCapacity;
        }

        // Backpack
        if (name.Contains("CONTAINER_BackPack"))
        {
            return Settings.Instance.ContainerBackpackCapacity;
        }

        // Large Cabinet 1
        if (name.Contains("CONTAINER_BathroomCabinet"))
        {
            return Settings.Instance.ContainerCabinetLgeCapacity;
        }

        // Briefcase
        if (name.Contains("CONTAINER_Briefcase"))
        {
            return Settings.Instance.ContainerBriefcaseCapacity;
        }

        // Hidden Cache
        if (name.Contains("CONTAINER_CacheStoreCommon"))
        {
            return Settings.Instance.ContainerHiddenCacheCapacity;
        }

        // Plastic Container 1
        if (name.Contains("CONTAINER_CacheStoreRare"))
        {
            return Settings.Instance.ContainerPlasticContainerCapacity;
        }

        // Coal Bin
        if (name.Contains("CONTAINER_CoalBin"))
        {
            return Settings.Instance.ContainerCoalBinCapacity;
        }

        // Cooler
        if (name.Contains("CONTAINER_Cooler"))
        {
            return Settings.Instance.ContainerCoolerCapacity;
        }

        // Dryer
        if (name.Contains("CONTAINER_Dryer"))
        {
            return Settings.Instance.ContainerDryerCapacity;
        }

        // Firewood Bin
        if (name.Contains("CONTAINER_FirewoodBin"))
        {
            return Settings.Instance.ContainerFirewoodBinCapacity;
        }

        // First Aid Kit
        if (name.Contains("CONTAINER_FirstAidKit"))
        {
            return Settings.Instance.ContainerFirstAidCapacity;
        }

        // Gun Locker
        if (name.Contains("CONTAINER_StorageGunLocker"))
        {
            return Settings.Instance.ContainerGunLockerCapacity;
        }

        // Supply Bin
        if (name.Contains("CONTAINER_ForestryCrate"))
        {
            return Settings.Instance.ContainerSupplyBinCapacity;
        }

        // Large Cabinet
        if (name.Contains("CONTAINER_LargeCabinet"))
        {
            return Settings.Instance.ContainerCabinetLgeCapacity;
        }

        // Trunk (Box) 1
        if (name.Contains("CONTAINER_LilysChest"))
        {
            return Settings.Instance.ContainerTrunkCapacity;
        }

        // Lock Box
        if (name.Contains("CONTAINER_LockBoxB"))
        {
            return Settings.Instance.ContainerLockBoxCapacity;
        }

        // Locker 1
        if (name.Contains("CONTAINER_LockerA"))
        {
            return Settings.Instance.ContainerLockerCapacity;
        }

        //Medicine Shelf
        if (name.Contains("CONTAINER_MedicineShelf"))
        {
            return Settings.Instance.ContainerMedicineShelfCapacity;
        }

        // Metal Box
        if (name.Contains("CONTAINER_MetalBox"))
        {
            return Settings.Instance.ContainerMetalContainerCapacity;
        }

        // Locker 2
        if (name.Contains("CONTAINER_MetalLocker"))
        {
            return Settings.Instance.ContainerLockerCapacity;
        }

        //Plastic Container
        if (name.Contains("CONTAINER_PlasticBox"))
        {
            return Settings.Instance.ContainerPlasticContainerCapacity;
        }

        // Safe
        if (name.Contains("CONTAINER_Safe"))
        {
            return Settings.Instance.ContainerSafeCapacity;
        }

        // Small Cabinet 1
        if (name.Contains("CONTAINER_SmallCabinet"))
        {
            return Settings.Instance.ContainerCabinetSmlCapacity;
        }

        // Trunk (Box)
        if (name.Contains("CONTAINER_SteamerTrunk"))
        {
            return Settings.Instance.ContainerTrunkCapacity;
        }

        // Hatch
        if (name.Contains("CONTAINER_StoneCabinATrapDoor"))
        {
            return Settings.Instance.ContainerHatchCapacity;
        }

        // Trash Can
        if (name.Contains("CONTAINER_TrashCanister"))
        {
            return Settings.Instance.ContainerTrashCanCapacity;
        }

        // Washer
        if (name.Contains("CONTAINER_Washer"))
        {
            return Settings.Instance.ContainerWasherCapacity;
        }

        // Rock Cache
        if (name.Contains("GEAR_RockCache_Prefab"))
        {
            return Settings.Instance.ContainerRockCacheCapacity;
        }

        // Cargo Container
        if (name.Contains("OBJ_CargoCrateBottomDoor") || name.Contains("OBJ_CargoCrateTopDoor"))
        {
            return Settings.Instance.ContainerCargoContainerCapacity;
        }

        // Cash Register
        if (name.Contains("OBJ_CashRegisterDrawer"))
        {
            return Settings.Instance.ContainerCashRegisterCapacity;
        }

        // Dresser Drawer
        if (name.Contains("OBJ_DresserDrawer") || name.Contains("OBJ_DresserTallDrawer"))
        {
            return Settings.Instance.ContainerDresserDrawerCapacity;
        }

        // Side Table Drawer
        if (name.Contains("OBJ_EndTableDrawer"))
        {
            return Settings.Instance.ContainerEndTableDrawerCapacity;
        }

        // Cupboard
        if (name.Contains("OBJ_CupboardDoor"))
        {
            return Settings.Instance.ContainerCupboardCapacity;
        }

        // Fishing Hut Cupboard
        if (name.Contains("OBJ_FishingCabinCupboardDoor"))
        {
            return Settings.Instance.ContainerCupboardCapacity;
        }

        // Fishing Hut Drawers
        if (name.Contains("OBJ_FishingCabinDresserDrawer"))
        {
            return Settings.Instance.ContainerFishingHutDrawerCapacity;
        }

        // Fridge
        if (name.Contains("OBJ_FridgeBottomDoor"))
        {
            return Settings.Instance.ContainerFridgeCapacity;
        }

        // Freezer
        if (name.Contains("OBJ_FridgeTopDoor"))
        {
            return Settings.Instance.ContainerFreezerCapacity;
        }

        // Infirmary Drawers
        if (name.Contains("OBJ_InfirmaryDrawer"))
        {
            return Settings.Instance.ContainerInfirmaryDrawerCapacity;
        }

        // Oven
        if (name.Contains("OBJ_GasOvenDoor"))
        {
            return Settings.Instance.ContainerOvenCapacity;
        }

        // Kitchen Cabinet
        if (name.Contains("OBJ_KitchenCabinetDoor"))
        {
            return Settings.Instance.ContainerKitchenCabinetCapacity;
        }

        // Kitchen Drawer
        if (name.Contains("OBJ_KitchenDrawer"))
        {
            return Settings.Instance.ContainerKitchenDrawerCapacity;
        }

        // Metal Desk Drawer Large
        if (name.Contains("OBJ_MetalDeskDrawer1") || name.Contains("OBJ_MetalDeskDrawer4"))
        {
            return Settings.Instance.ContainerDeskDrawerLgeCapacity;
        }

        // Metal Desk Drawer Small
        if (name.Contains("OBJ_MetalDeskDrawer2") || name.Contains("OBJ_MetalDeskDrawer3"))
        {
            return Settings.Instance.ContainerDeskDrawerSmlCapacity;
        }

        // File Cabinet
        if (name.Contains("OBJ_MetalFileCabinetDrawer"))
        {
            return Settings.Instance.ContainerFileCabinetCapacity;
        }

        // Workbench Drawer
        if (name.Contains("OBJ_WorkBenchDrawer"))
        {
            return Settings.Instance.ContainerWorkbenchDrawerCapacity;
        }

        // Locker 3
        if (name.Contains("OBJ_MetalLockerDoor"))
        {
            return Settings.Instance.ContainerLockerCapacity;
        }

        // Small Cabinet 2
        if (name.Contains("OBJ_SmallCabinetDoor"))
        {
            return Settings.Instance.ContainerCabinetSmlCapacity;
        }

        // Suitcase
        if (name.Contains("OBJ_Suitcase"))
        {
            return Settings.Instance.ContainerSuitcaseCapacity;
        }

        // Tool Cabinet Drawers
        if (name.Contains("OBJ_ToolCabinetDrawer"))
        {
            return name.Contains("OBJ_ToolCabinetDrawerE")
                ? Settings.Instance.ContainerToolCabinetDrawerLgeCapacity
                : Settings.Instance.ContainerToolCabinetDrawerSmlCapacity;
        }

        // Warden Desk
        if (name.Contains("OBJ_WardenDesk"))
        {
            return Settings.Instance.ContainerWardenDeskDrawerCapacity;
        }

        // Wood Desk Drawer
        if (name.Contains("OBJ_TrailerInteriorDeskDrawerLg_Prefab"))
        {
            return Settings.Instance.ContainerWoodDeskDrawerCapacity;
        }

        // Safety Deposit Box
        if (name.Contains("STR_BankAVaultDepositBox"))
        {
            return Settings.Instance.ContainerSafetyDepositBoxCapacity;
        }

        return null;
    }
}

using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

// This entire class needs to be revisited with the following changes...
// Create a dedicated method for updating these prefabs, instantly update them like we are with the encumbering tweaks.
// Make sure that all of these containers are up-to-date, it has been awhile since it was last touched.
internal static class Container
{
    [HarmonyPatch(typeof(Il2Cpp.Container), nameof(Il2Cpp.Container.Awake))]
    private class AdjustContainerCapacityAmount
    {
        private static void Postfix(Il2Cpp.Container __instance)
        {
            if (ContainerSettings.Instance.InfiniteContainerWeight)
            {
                __instance.m_Capacity = ItemWeight.FromKilograms(10000);
            }
            else
            {
                // Glove Box 1
                if (__instance.name.Contains("CarSedanGloveBox_Prefab"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerGloveBoxCapacity);
                }

                // Trunk (Car)
                else if (__instance.name.Contains("CarSedanTrunkDoor_Prefab"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCarTrunkCapacity);
                }

                // Glove Box 2
                else if (__instance.name.Contains("CarTruckGloveBox_Prefab"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerGloveBoxCapacity);
                }

                // Backpack
                if (__instance.name.Contains("CONTAINER_BackPack"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerBackpackCapacity);
                }

                // Large Cabinet 1
                else if (__instance.name.Contains("CONTAINER_BathroomCabinet"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCabinetLgeCapacity);
                }

                // Briefcase
                else if (__instance.name.Contains("CONTAINER_Briefcase"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerBriefcaseCapacity);
                }

                // Hidden Cache
                else if (__instance.name.Contains("CONTAINER_CacheStoreCommon"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerHiddenCacheCapacity);
                }

                // Plastic Container 1
                else if (__instance.name.Contains("CONTAINER_CacheStoreRare"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerPlasticContainerCapacity);
                }

                // Coal Bin
                else if (__instance.name.Contains("CONTAINER_CoalBin"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCoalBinCapacity);
                }

                // Cooler
                else if (__instance.name.Contains("CONTAINER_Cooler"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCoolerCapacity);
                }

                // Dryer
                else if (__instance.name.Contains("CONTAINER_Dryer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerDryerCapacity);
                }

                // Firewood Bin
                else if (__instance.name.Contains("CONTAINER_FirewoodBin"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerFirewoodBinCapacity);
                }

                // First Aid Kit
                else if (__instance.name.Contains("CONTAINER_FirstAidKit"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerFirstAidCapacity);
                }

                // Gun Locker
                else if (__instance.name.Contains("CONTAINER_StorageGunLocker"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerGunLockerCapacity);
                }

                // Supply Bin
                else if (__instance.name.Contains("CONTAINER_ForestryCrate"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerSupplyBinCapacity);
                }

                // Large Cabinet
                else if (__instance.name.Contains("CONTAINER_LargeCabinet"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCabinetLgeCapacity);
                }

                // Trunk (Box) 1
                else if (__instance.name.Contains("CONTAINER_LilysChest"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerTrunkCapacity);
                }

                // Trunk (Rustic Storage)
                else if (__instance.name.Contains("CONTAINER_Rustic_Storage_Trunk"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerRusticStorageTrunkCapacity);
                }

                // Lock Box
                else if (__instance.name.Contains("CONTAINER_LockBoxB"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerLockBoxCapacity);
                }

                // Locker 1
                else if (__instance.name.Contains("CONTAINER_LockerA"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerLockerCapacity);
                }

                //Medicine Shelf
                else if (__instance.name.Contains("CONTAINER_MedicineShelf"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerMedicineShelfCapacity);
                }

                // Metal Box
                else if (__instance.name.Contains("CONTAINER_MetalBox"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerMetalContainerCapacity);
                }

                // Locker 2
                else if (__instance.name.Contains("CONTAINER_MetalLocker"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerLockerCapacity);
                }

                //Plastic Container
                else if (__instance.name.Contains("CONTAINER_PlasticBox"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerPlasticContainerCapacity);
                }

                // Safe
                else if (__instance.name.Contains("CONTAINER_Safe"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerSafeCapacity);
                }

                // Small Cabinet 1
                else if (__instance.name.Contains("CONTAINER_SmallCabinet"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCabinetSmlCapacity);
                }

                // Trunk (Box)
                else if (__instance.name.Contains("CONTAINER_SteamerTrunk"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerTrunkCapacity);
                }

                // Hatch
                else if (__instance.name.Contains("CONTAINER_StoneCabinATrapDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerHatchCapacity);
                }

                // Trash Can
                else if (__instance.name.Contains("CONTAINER_TrashCanister"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerTrashCanCapacity);
                }

                // Washer
                else if (__instance.name.Contains("CONTAINER_Washer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerWasherCapacity);
                }

                // Rock Cache
                else if (__instance.name.Contains("GEAR_RockCache_Prefab"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerRockCacheCapacity);
                }

                // Cargo Container
                else if (__instance.name.Contains("OBJ_CargoCrateBottomDoor") || __instance.name.Contains("OBJ_CargoCrateTopDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCargoContainerCapacity);
                }

                // Cash Register
                else if (__instance.name.Contains("OBJ_CashRegisterDrawer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCashRegisterCapacity);
                }

                // Dresser Drawer
                else if (__instance.name.Contains("OBJ_DresserDrawer") || __instance.name.Contains("OBJ_DresserTallDrawer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerDresserDrawerCapacity);
                }

                // Side Table Drawer
                else if (__instance.name.Contains("OBJ_EndTableDrawer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerEndTableDrawerCapacity);
                }

                // Cupboard
                else if (__instance.name.Contains("OBJ_CupboardDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCupboardCapacity);
                }

                // Fishing Hut Cupboard
                else if (__instance.name.Contains("OBJ_FishingCabinCupboardDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCupboardCapacity);
                }

                // Fishing Hut Drawers
                else if (__instance.name.Contains("OBJ_FishingCabinDresserDrawer"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerFishingHutDrawerCapacity);
                }

                // Fridge
                else if (__instance.name.Contains("OBJ_FridgeBottomDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerFridgeCapacity);
                }

                // Freezer
                else if (__instance.name.Contains("OBJ_FridgeTopDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerFreezerCapacity);
                }

                // Infirmary Drawers
                else if (__instance.name.Contains("OBJ_InfirmaryDrawer"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerInfirmaryDrawerCapacity);
                }

                // Oven
                else if (__instance.name.Contains("OBJ_GasOvenDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerOvenCapacity);
                }

                // Kitchen Cabinet
                else if (__instance.name.Contains("OBJ_KitchenCabinetDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerKitchenCabinetCapacity);
                }

                // Kitchen Drawer
                else if (__instance.name.Contains("OBJ_KitchenDrawer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerKitchenDrawerCapacity);
                }

                // Metal Desk Drawer Large
                else if (__instance.name.Contains("OBJ_MetalDeskDrawer1") || __instance.name.Contains("OBJ_MetalDeskDrawer4"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerDeskDrawerLgeCapacity);
                }

                // Metal Desk Drawer Small
                else if (__instance.name.Contains("OBJ_MetalDeskDrawer2") || __instance.name.Contains("OBJ_MetalDeskDrawer3"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerDeskDrawerSmlCapacity);
                }

                // File Cabinet
                else if (__instance.name.Contains("OBJ_MetalFileCabinetDrawer"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerFileCabinetCapacity);
                }

                // Workbench Drawer
                else if (__instance.name.Contains("OBJ_WorkBenchDrawer"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerWorkbenchDrawerCapacity);
                }

                // Locker 3
                else if (__instance.name.Contains("OBJ_MetalLockerDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerLockerCapacity);
                }

                // Small Cabinet 2
                else if (__instance.name.Contains("OBJ_SmallCabinetDoor"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerCabinetSmlCapacity);
                }

                // Suitcase
                else if (__instance.name.Contains("OBJ_Suitcase"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerSuitcaseCapacity);
                }

                // Tool Cabinet Drawers
                else if (__instance.name.Contains("OBJ_ToolCabinetDrawer"))
                {
                    if (__instance.name.Contains("OBJ_ToolCabinetDrawerE"))
                    {
                        __instance.m_Capacity =
                            ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerToolCabinetDrawerLgeCapacity);
                    }
                    else
                    {
                        __instance.m_Capacity =
                            ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerToolCabinetDrawerSmlCapacity);
                    }
                }

                // Warden Desk
                else if (__instance.name.Contains("OBJ_WardenDesk"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerWardenDeskDrawerCapacity);
                }

                // Wood Desk Drawer
                else if (__instance.name.Contains("OBJ_TrailerInteriorDeskDrawerLg_Prefab"))
                {
                    __instance.m_Capacity = ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerWoodDeskDrawerCapacity);
                }

                // Safety Deposit Box
                else if (__instance.name.Contains("STR_BankAVaultDepositBox"))
                {
                    __instance.m_Capacity =
                        ItemWeight.FromKilograms(ContainerSettings.Instance.ContainerSafetyDepositBoxCapacity);
                }

                else
                {
                    Mod.Logger.Log($"Found container '{__instance.name}' that is missing from the known containers list.", FlaggedLoggingLevel.Debug);
                }
            }

            Mod.Logger.Log($"Container '{__instance.name}' capacity set to {__instance.m_Capacity}.", FlaggedLoggingLevel.Trace);
        }
    }
}
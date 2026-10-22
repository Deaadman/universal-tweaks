using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Container"/>, anything the player can store items in.</summary>
internal static class ContainerPatches
{
    /// <summary>Sets each container's capacity from the settings as it wakes: see <see cref="ContainerCapacities"/>.</summary>
    [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
    private static class AdjustContainerCapacityAmount
    {
        private static void Postfix(Container __instance) => ContainerCapacities.Register(__instance);
    }
}

using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="FirstPersonFlashlight"/>, the flashlight in the player's hands.</summary>
internal static class FirstPersonFlashlightPatches
{
    /// <summary>Applies the beam colour, brightness and range settings every frame: see <see cref="Flashlights"/>.</summary>
    [HarmonyPatch(typeof(FirstPersonFlashlight), nameof(FirstPersonFlashlight.Update))]
    private static class ModifyFlashFlashlightBeamColor
    {
        private static void Postfix(FirstPersonFlashlight __instance) => Flashlights.UpdateFirstPerson(__instance);
    }
}

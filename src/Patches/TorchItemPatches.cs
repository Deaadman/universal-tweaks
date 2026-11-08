using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="TorchItem"/>, the torch.</summary>
internal static class TorchItemPatches
{
    /// <summary>Uses the held-off fire tool once the torch lights: see <see cref="TorchToolUse"/>.</summary>
    [HarmonyPatch(typeof(TorchItem), nameof(TorchItem.Ignite), [typeof(string)])]
    private static class UseToolOnLit
    {
        private static void Postfix(TorchItem __instance) => TorchToolUse.OnLit(__instance);
    }

    /// <summary>Forgets the held-off fire tool when the player stops lighting the torch: see <see cref="TorchToolUse"/>.</summary>
    [HarmonyPatch(typeof(TorchItem), nameof(TorchItem.CancelIgnite))]
    private static class KeepToolOnCancel
    {
        private static void Postfix(TorchItem __instance) => TorchToolUse.OnCancelled(__instance);
    }
}

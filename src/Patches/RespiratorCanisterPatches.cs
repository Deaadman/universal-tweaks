using Il2CppTLD.Gear;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="RespiratorCanister"/>, the respirator's canister.</summary>
/// <remarks>
/// The duration is applied every frame and whenever the canister's condition is read, so loading or attaching a
/// canister never sees the game's own value.
/// </remarks>
internal static class RespiratorCanisterPatches
{
    /// <summary>Applies the canister duration every frame.</summary>
    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.ManualUpdate))]
    private static class ApplyDurationOnUpdate
    {
        private static void Prefix(RespiratorCanister __instance) => ApplyDuration(__instance);
    }

    /// <summary>Applies the canister duration before its condition is read.</summary>
    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.NormalizedCondition), MethodType.Getter)]
    private static class ApplyDurationOnConditionRead
    {
        private static void Prefix(RespiratorCanister __instance) => ApplyDuration(__instance);
    }

    /// <summary>Applies the canister duration before checking whether it still protects.</summary>
    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.IsValid), MethodType.Getter)]
    private static class ApplyDurationOnValidityRead
    {
        private static void Prefix(RespiratorCanister __instance) => ApplyDuration(__instance);
    }

    /// <summary>Sets how long a canister protects for, in real seconds, from the settings.</summary>
    /// <param name="canister">The canister.</param>
    private static void ApplyDuration(RespiratorCanister canister) =>
        canister.m_ProtectionDurationRTSeconds = Settings.Instance.RespiratorCanisterDuration;
}

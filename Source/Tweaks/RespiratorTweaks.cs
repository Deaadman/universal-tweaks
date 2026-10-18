using Il2CppTLD.Gear;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class RespiratorTweaks
{
    // Applied on every tick and on every condition read, so loading or attaching a canister never sees the prefab value.
    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.ManualUpdate))]
    private static class ApplyDurationOnUpdate
    {
        private static void Prefix(RespiratorCanister __instance) => ApplyDuration(__instance);
    }

    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.NormalizedCondition), MethodType.Getter)]
    private static class ApplyDurationOnConditionRead
    {
        private static void Prefix(RespiratorCanister __instance) => ApplyDuration(__instance);
    }

    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.IsValid), MethodType.Getter)]
    private static class ApplyDurationOnValidityRead
    {
        private static void Prefix(RespiratorCanister __instance) => ApplyDuration(__instance);
    }

    private static void ApplyDuration(RespiratorCanister canister)
    {
        canister.m_ProtectionDurationRTSeconds = Settings.Instance.RespiratorCanisterDuration;
    }
}
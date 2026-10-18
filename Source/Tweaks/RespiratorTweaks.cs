using Il2CppTLD.Gear;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class RespiratorTweaks
{
    // Applied each tick while attached rather than on attach, so setting changes reach a canister already in use.
    [HarmonyPatch(typeof(RespiratorCanister), nameof(RespiratorCanister.ManualUpdate))]
    private static class ModifyCanisterDuration
    {
        private static void Prefix(RespiratorCanister __instance)
        {
            __instance.m_ProtectionDurationRTSeconds = Settings.Instance.RespiratorCanisterDuration;
        }
    }
}
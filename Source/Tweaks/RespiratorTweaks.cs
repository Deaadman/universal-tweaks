using Il2CppTLD.Gear;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class RespiratorTweaks
{
    [HarmonyPatch(typeof(Respirator), nameof(Respirator.AttachCanister))]
    private static class ModifyCanisterDuration
    {
        private static void Postfix(Respirator __instance, RespiratorCanister canister)
        {
            canister.m_ProtectionDurationRTSeconds = Settings.Instance.RespiratorCanisterDuration;

            Mod.Logger.Log($"Set respirator canister duration to {Settings.Instance.RespiratorCanisterDuration} seconds.", FlaggedLoggingLevel.Debug);
        }
    }
}
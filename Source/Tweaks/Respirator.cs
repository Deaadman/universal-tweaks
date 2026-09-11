using Il2CppTLD.Gear;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class Respirator
{
    [HarmonyPatch(typeof(Il2CppTLD.Gear.Respirator), nameof(Il2CppTLD.Gear.Respirator.AttachCanister))]
    private static class ModifyCanisterDuration
    {
        private static void Postfix(Il2CppTLD.Gear.Respirator __instance, RespiratorCanister canister)
        {
            canister.m_ProtectionDurationRTSeconds = Settings.Instance.RespiratorCanisterDuration;

            Mod.Logger.Log($"Set respirator canister duration to {Settings.Instance.RespiratorCanisterDuration} seconds.", FlaggedLoggingLevel.Debug);
        }
    }
}
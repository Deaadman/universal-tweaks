using Il2CppTLD.Gear;

namespace UniversalTweaks.Patches;

/// <summary>
/// Patches on <see cref="CausesHeadacheDebuff"/>, the headache some foods give. Patching the debuff itself covers every
/// food that has it, including any the game adds later.
/// </summary>
internal static class CausesHeadacheDebuffPatches
{
    /// <summary>Stops the headache when <see cref="Settings.RemoveHeadacheDebuffFromFoods"/> is on.</summary>
    [HarmonyPatch(typeof(CausesHeadacheDebuff), nameof(CausesHeadacheDebuff.Apply))]
    private static class BlockHeadacheDebuff
    {
        private static bool Prefix(CausesHeadacheDebuff __instance)
        {
            if (!Settings.Instance.RemoveHeadacheDebuffFromFoods)
                return true;

            Mod.Logger.Log($"Blocked headache debuff from '{__instance.name}'.", FlaggedLoggingLevel.Trace);
            return false;
        }
    }

    /// <summary>Shows the food's effect as neutral when <see cref="Settings.RemoveHeadacheDebuffFromFoods"/> is on.</summary>
    [HarmonyPatch(typeof(CausesHeadacheDebuff), nameof(CausesHeadacheDebuff.GetEffect))]
    private static class HideHeadacheDebuffEffect
    {
        private static void Postfix(ref IFoodEffect.Effect __result)
        {
            if (Settings.Instance.RemoveHeadacheDebuffFromFoods)
                __result = IFoodEffect.Effect.Neutral;
        }
    }
}

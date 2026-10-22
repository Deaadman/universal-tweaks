using Il2CppTLD.Gear;

namespace UniversalTweaks.Patches;

/// <summary>
/// Patches on <see cref="FoodStatEffect"/>, a food's effect on one of the player's stats, like the stews' fatigue.
/// </summary>
/// <remarks>
/// The stew fatigue is set whenever the effect is applied or read, so changes reach stews that already exist.
/// </remarks>
internal static class FoodStatEffectPatches
{
    /// <summary>Sets the stew fatigue before the food is eaten.</summary>
    [HarmonyPatch(typeof(FoodStatEffect), nameof(FoodStatEffect.Apply))]
    private static class ApplyStewFatigueOnConsume
    {
        private static void Prefix(FoodStatEffect __instance) => ApplyStewFatigue(__instance);
    }

    /// <summary>Sets the stew fatigue before the effect's amount is read.</summary>
    [HarmonyPatch(typeof(FoodStatEffect), nameof(FoodStatEffect.GetStatEffect))]
    private static class ApplyStewFatigueOnStatQuery
    {
        private static void Prefix(FoodStatEffect __instance) => ApplyStewFatigue(__instance);
    }

    /// <summary>Sets the stew fatigue before the effect is shown.</summary>
    [HarmonyPatch(typeof(FoodStatEffect), nameof(FoodStatEffect.GetEffect))]
    private static class ApplyStewFatigueOnEffectQuery
    {
        private static void Prefix(FoodStatEffect __instance) => ApplyStewFatigue(__instance);
    }

    /// <summary>
    /// Sets the fatigue the meat and vegetable stews take away to <see cref="Settings.ReduceStewFatigueLossAmount"/>.
    /// </summary>
    /// <param name="effect">The food's stat effect; anything but the stews' fatigue is left alone.</param>
    private static void ApplyStewFatigue(FoodStatEffect effect)
    {
        if (effect.m_Stat == FoodStatEffect.StatType.Fatigue &&
            effect.gameObject.name is "GEAR_CookedStewMeat" or "GEAR_CookedStewVegetables")
            effect.m_Effect = Settings.Instance.ReduceStewFatigueLossAmount;
    }
}

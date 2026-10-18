using Il2CppTLD.Gear;
using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class FoodTweaks
{
    // Patching the debuff itself covers every food carrying the component, including any Hinterland add later.
    [HarmonyPatch(typeof(CausesHeadacheDebuff), nameof(CausesHeadacheDebuff.Apply))]
    private static class BlockHeadacheDebuff
    {
        private static bool Prefix(CausesHeadacheDebuff __instance)
        {
            if (!Settings.Instance.RemoveHeadacheDebuffFromFoods)
            {
                return true;
            }

            Mod.Logger.Log($"Blocked headache debuff from '{__instance.name}'.", FlaggedLoggingLevel.Trace);
            return false;
        }
    }

    [HarmonyPatch(typeof(CausesHeadacheDebuff), nameof(CausesHeadacheDebuff.GetEffect))]
    private static class HideHeadacheDebuffEffect
    {
        private static void Postfix(ref IFoodEffect.Effect __result)
        {
            if (Settings.Instance.RemoveHeadacheDebuffFromFoods)
            {
                __result = IFoodEffect.Effect.Neutral;
            }
        }
    }

    // Runs every tick and for the elapsed-time catch-up on load, so setting changes apply immediately.
    [HarmonyPatch(typeof(InsulatedFlask), nameof(InsulatedFlask.CalculateHeatLoss))]
    private static class InsulatedFlaskHeatLoss
    {
        private static void Prefix(InsulatedFlask __instance)
        {
            __instance.m_PercentHeatLossPerMinuteIndoors = Settings.Instance.InsulatedFlaskHeatLossPerMinuteIndoors;
            __instance.m_PercentHeatLossPerMinuteOutdoors = Settings.Instance.InsulatedFlaskHeatLossPerMinuteOutdoors;
        }
    }

    // Applied whenever the effect is read or consumed, so setting changes reach stews that already exist.
    [HarmonyPatch(typeof(FoodStatEffect), nameof(FoodStatEffect.Apply))]
    private static class ApplyStewFatigueOnConsume
    {
        private static void Prefix(FoodStatEffect __instance) => ApplyStewFatigue(__instance);
    }

    [HarmonyPatch(typeof(FoodStatEffect), nameof(FoodStatEffect.GetStatEffect))]
    private static class ApplyStewFatigueOnStatQuery
    {
        private static void Prefix(FoodStatEffect __instance) => ApplyStewFatigue(__instance);
    }

    [HarmonyPatch(typeof(FoodStatEffect), nameof(FoodStatEffect.GetEffect))]
    private static class ApplyStewFatigueOnEffectQuery
    {
        private static void Prefix(FoodStatEffect __instance) => ApplyStewFatigue(__instance);
    }

    private static void ApplyStewFatigue(FoodStatEffect effect)
    {
        if (effect.m_Stat == FoodStatEffect.StatType.Fatigue &&
            effect.gameObject.name is "GEAR_CookedStewMeat" or "GEAR_CookedStewVegetables")
        {
            effect.m_Effect = Settings.Instance.ReduceStewFatigueLossAmount;
        }
    }

    // TODO: Possibly move this to the Awake() method and attach a GearItemExtension.cs MonoBehaviour to each GearItem prefab.
    // From there we can call a method that will set the values of the GearItem based on the settings without relying on Harmony Patches.
    // This will allow us to change the settings and have them apply immediately without needing to reload the game.
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class ApplyFoodItemTweaks
    {
        private static void Postfix(GearItem __instance)
        {
            if (Settings.Instance.ConsistantDressingWeight && __instance.gameObject.name is "GEAR_OldMansBeardDressing")
            {
                __instance.m_GearItemData.m_BaseWeight = ItemWeight.FromKilograms(0.03f);
                Mod.Logger.Log("Applied consistent dressing weight to Old Man's Dressing.", FlaggedLoggingLevel.Trace);
            }
        }
    }
}
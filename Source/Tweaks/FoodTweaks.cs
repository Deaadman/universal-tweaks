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

    private const string DressingGearName = "GEAR_OldMansBeardDressing";
    private const float ConsistentDressingWeightKG = 0.03f;

    // Captured before the first change so turning the setting off can put the vanilla weight back.
    private static ItemWeight? _originalDressingWeight;

    // The weight lives on the shared GearItemData asset, so this also covers a reloaded asset after a scene change.
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class ApplyFoodItemTweaks
    {
        private static void Postfix(GearItem __instance)
        {
            if (__instance.gameObject.name is DressingGearName)
            {
                ApplyDressingWeight(__instance.m_GearItemData);
            }
        }
    }

    internal static void RefreshDressingWeight()
    {
        var prefab = GearItem.LoadGearItemPrefab(DressingGearName);
        if (prefab != null)
        {
            ApplyDressingWeight(prefab.m_GearItemData);
        }
    }

    private static void ApplyDressingWeight(GearItemData? data)
    {
        if (data == null)
        {
            return;
        }

        _originalDressingWeight ??= data.m_BaseWeight;

        if (Settings.Instance.ConsistantDressingWeight)
        {
            data.m_BaseWeight = ItemWeight.FromKilograms(ConsistentDressingWeightKG);
        }
        else
        {
            data.m_BaseWeight = _originalDressingWeight.Value;
        }

        Mod.Logger.Log($"Set Old Man's Dressing base weight to {data.m_BaseWeight}.", FlaggedLoggingLevel.Trace);
    }
}
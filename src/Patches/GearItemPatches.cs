using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="GearItem"/>, any item in the world or the player's inventory.</summary>
internal static class GearItemPatches
{
    /// <summary>
    /// Drops items facing a random direction when <see cref="Settings.RandomizedItemRotationDrops"/> is on, instead of
    /// all the same way. Rifles stay lying on their side.
    /// </summary>
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Drop))]
    private static class RandomizedItemRotation
    {
        private static void Postfix(GearItem __instance)
        {
            if (__instance == null || !Settings.Instance.RandomizedItemRotationDrops)
                return;

            var itemTransform = __instance.transform;
            var randomRotationY = UnityEngine.Random.Range(0f, 360f);

            itemTransform.eulerAngles = __instance.name.Contains("GEAR_Rifle")
                ? new Vector3(itemTransform.eulerAngles.x, randomRotationY, 90)
                : new Vector3(0, randomRotationY, 0);

            Mod.Logger.Log($"Randomized drop rotation for '{__instance.name}' to {randomRotationY:0.##} degrees.", FlaggedLoggingLevel.Trace);
        }
    }

    /// <summary>Notes when the player is dropping an item, for <see cref="SingleRoundDrops.Dropping"/>.</summary>
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Drop))]
    private static class TrackDrop
    {
        private static void Prefix() => SingleRoundDrops.Dropping = true;

        private static void Finalizer() => SingleRoundDrops.Dropping = false;
    }

    /// <summary>Drops one round split off a box as a single round: see <see cref="SingleRoundDrops"/>.</summary>
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.InstantiateStackable))]
    private static class DropSingleRound
    {
        private static void Postfix(int numUnits, ref GearItem? __result) =>
            __result = SingleRoundDrops.SwapForSingleRound(__result, numUnits);
    }

    /// <summary>
    /// Applies <see cref="DressingWeight"/> as an old man's beard dressing loads, which also covers its data asset
    /// being reloaded after a scene change.
    /// </summary>
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class ApplyDressingWeight
    {
        private static void Postfix(GearItem __instance)
        {
            if (__instance.gameObject.name is DressingWeight.GearName)
                DressingWeight.Apply(__instance.m_GearItemData);
        }
    }

    /// <summary>Applies <see cref="MreTexture"/> as items load.</summary>
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class SwapMreTexture
    {
        private static void Postfix(GearItem __instance) => MreTexture.OnGearItemLoaded(__instance);
    }

    /// <summary>Applies <see cref="StewFatigue"/> as items load.</summary>
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class ApplyStewFatigue
    {
        private static void Postfix(GearItem __instance) => StewFatigue.OnGearItemLoaded(__instance);
    }
}

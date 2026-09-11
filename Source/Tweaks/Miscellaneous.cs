using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class Miscellaneous
{
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Drop))]
    public class RandomizedItemRotation
    {
        private static void Postfix(GearItem __instance)
        {
            if (__instance == null || !Settings.Instance.RandomizedItemRotationDrops)
            {
                return;
            }

            var itemTransform = __instance.transform;
            var randomRotationY = UnityEngine.Random.Range(0f, 360f);

            itemTransform.eulerAngles = __instance.name.Contains("GEAR_Rifle")
                ? new Vector3(itemTransform.eulerAngles.x, randomRotationY, 90)
                : new Vector3(0, randomRotationY, 0);

            Mod.Logger.Log($"Randomized drop rotation for '{__instance.name}' to {randomRotationY:0.##} degrees.", FlaggedLoggingLevel.Trace);
        }
    }
}
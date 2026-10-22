namespace UniversalTweaks.Core;

/// <summary>How long a flashlight's battery lasts and recharges, in in-game hours, as the game's <c>FlashlightItem</c> keeps them.</summary>
/// <param name="LowBeamDuration">How long a full battery lasts on the low beam.</param>
/// <param name="HighBeamDuration">How long a full battery lasts on the high beam.</param>
/// <param name="RechargeTime">How long an empty battery takes to recharge during an aurora.</param>
internal readonly record struct FlashlightTuning(float LowBeamDuration, float HighBeamDuration, float RechargeTime);

/// <summary>How bright a flashlight's beams are and how far they reach.</summary>
/// <param name="LowIntensity">The low beam's light intensity.</param>
/// <param name="LowRange">The low beam's range, in metres.</param>
/// <param name="HighIntensity">The high beam's light intensity.</param>
/// <param name="HighRange">The high beam's range, in metres.</param>
internal readonly record struct BeamLightTuning(float LowIntensity, float LowRange, float HighIntensity, float HighRange);

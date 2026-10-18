namespace UniversalTweaks.Utilities;

internal readonly record struct FlashlightTuning(float LowBeamDuration, float HighBeamDuration, float RechargeTime);

internal readonly record struct BeamLightTuning(float LowIntensity, float LowRange, float HighIntensity, float HighRange);
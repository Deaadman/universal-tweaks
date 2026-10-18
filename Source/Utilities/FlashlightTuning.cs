namespace UniversalTweaks.Utilities;

internal readonly record struct FlashlightTuning(
    float LowBeamDuration,
    float HighBeamDuration,
    float RechargeTime,
    float LowBeamIntensity,
    float LowBeamRange,
    float HighBeamIntensity,
    float HighBeamRange);
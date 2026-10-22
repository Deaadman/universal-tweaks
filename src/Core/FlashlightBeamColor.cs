namespace UniversalTweaks.Core;

/// <summary>The colours a flashlight's beam can be set to in the settings.</summary>
/// <remarks>The settings file saves the choice, so keep the existing colours where they are.</remarks>
internal enum FlashlightBeamColor
{
    /// <summary>The game's own pale blue.</summary>
    Default,

    /// <summary>Blue.</summary>
    Blue,

    /// <summary>Green.</summary>
    Green,

    /// <summary>Orange.</summary>
    Orange,

    /// <summary>Purple.</summary>
    Purple,

    /// <summary>Red.</summary>
    Red,

    /// <summary>White.</summary>
    White,

    /// <summary>Yellow.</summary>
    Yellow,

    /// <summary>A colour mixed from the red, green and blue settings.</summary>
    Custom,
}

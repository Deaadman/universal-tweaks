namespace UniversalTweaks.Utilities;

/// <summary>Lets something called many times a frame run only the first time.</summary>
internal sealed class OncePerFrame
{
    /// <summary>The frame it last ran in, or -1 if it never has.</summary>
    private int m_LastFrame = -1;

    /// <summary>Whether to run this time: only the first call in each frame gets <see langword="true"/>.</summary>
    /// <returns><see langword="true"/> on the frame's first call.</returns>
    internal bool ShouldRun()
    {
        if (Time.frameCount == m_LastFrame)
            return false;

        m_LastFrame = Time.frameCount;
        return true;
    }
}

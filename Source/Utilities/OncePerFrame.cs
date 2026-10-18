namespace UniversalTweaks.Utilities;

internal sealed class OncePerFrame
{
    private int _lastFrame = -1;

    internal bool ShouldRun()
    {
        if (Time.frameCount == _lastFrame)
        {
            return false;
        }

        _lastFrame = Time.frameCount;
        return true;
    }
}
namespace UniversalTweaks.Utilities;

/// <summary>
/// Opens the menu for lighting a torch even when the player has only one fire tool, when
/// <see cref="Settings.SaferTorchLighting"/> is on.
/// </summary>
/// <remarks>
/// With one tool, the game's <see cref="Panel_TorchLight.Enable"/> uses it straight away instead of opening the menu, so
/// a match is spent even if the player lets go before the torch lights. The menu is opened the way the game opens it for
/// several tools instead, and the player confirms or cancels as usual.
/// </remarks>
internal static class TorchLightMenu
{
    /// <summary>Whether the menu is opening, so a tool used now is the game picking the only one.</summary>
    private static bool s_Opening;

    /// <summary>Whether the game's pick of the only tool was stopped while the menu was opening.</summary>
    private static bool s_StoppedAutoUse;

    /// <summary>Notes that the menu is opening or closing.</summary>
    /// <param name="enable">Whether it's opening.</param>
    internal static void OnEnabling(bool enable)
    {
        s_Opening = enable;
        s_StoppedAutoUse = false;
    }

    /// <summary>Whether to let a tool be used: not when the game picks the only one as the menu opens.</summary>
    /// <returns><see langword="true"/> to let the game use the selected tool.</returns>
    internal static bool AllowUse()
    {
        if (!s_Opening || !Settings.Instance.SaferTorchLighting)
            return true;

        s_StoppedAutoUse = true;
        return false;
    }

    /// <summary>Shows the menu, the way the game does for several tools, if the only tool wasn't used.</summary>
    /// <param name="panel">The menu.</param>
    internal static void OnEnabled(Panel_TorchLight panel)
    {
        s_Opening = false;
        if (!s_StoppedAutoUse)
            return;

        s_StoppedAutoUse = false;

        panel.RefreshVisuals();
        Input.ResetInputAxes();

        var itemInHands = GameManager.GetPlayerManagerComponent().m_ItemInHands;
        if (itemInHands != null && itemInHands.m_FirstPersonItem != null)
            itemInHands.m_FirstPersonItem.AnimateFPSMeshToExitPosition();

        Mod.Logger.Log("Opened the torch lighting menu for the only fire tool.", FlaggedLoggingLevel.Debug);
    }

    /// <summary>Forgets the menu was opening, if opening it failed.</summary>
    internal static void Reset()
    {
        s_Opening = false;
        s_StoppedAutoUse = false;
    }
}

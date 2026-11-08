namespace UniversalTweaks.Utilities;

/// <summary>
/// Uses up the match, or wears the flint and steel or fire striker, only once the torch lights, when
/// <see cref="Settings.SaferTorchLighting"/> is on.
/// </summary>
/// <remarks>
/// The game uses the tool as soon as the player picks it, before the hold to light the torch, so letting go early wastes
/// it. The tool is picked the game's own way without using it, and used when <see cref="TorchItem.Ignite(string)"/>
/// lights the same torch. Letting go (<see cref="TorchItem.CancelIgnite"/>) forgets it. A flint and steel or fire
/// striker that breaks from the use still lights the torch, since the torch is already lit by then.
/// </remarks>
internal static class TorchToolUse
{
    /// <summary>The torch being lit and the tool to use once it lights, or <see langword="null"/> for none.</summary>
    private static (TorchItem Torch, GearItem Tool)? s_Pending;

    /// <summary>
    /// Holds off using a tool the player just picked, until the torch lights. Plays the game's click, which it only
    /// plays when it uses the tool.
    /// </summary>
    /// <param name="tool">The tool, if there is one.</param>
    /// <param name="consumeResource">Whether the game would use the tool; set to <see langword="false"/> when held off.</param>
    internal static void Defer(GearItem? tool, ref bool consumeResource)
    {
        if (!Settings.Instance.SaferTorchLighting || !consumeResource || tool == null)
            return;

        var itemInHands = GameManager.GetPlayerManagerComponent().m_ItemInHands;
        if (itemInHands == null || itemInHands.m_TorchItem == null)
            return;

        var torch = itemInHands.m_TorchItem;

        GameAudioManager.PlayGUIButtonClick();
        consumeResource = false;
        s_Pending = (torch, tool);

        Mod.Logger.Log($"Holding off using '{tool.name}' until the torch lights.", FlaggedLoggingLevel.Debug);
    }

    /// <summary>Uses the held-off tool, if this is the torch it was picked for.</summary>
    /// <param name="torch">The torch that lit.</param>
    internal static void OnLit(TorchItem torch)
    {
        if (s_Pending is not var (pendingTorch, tool) || pendingTorch == null || pendingTorch != torch)
            return;

        s_Pending = null;
        if (tool == null || tool.m_FireStarterItem == null)
            return;

        var fireStarter = tool.m_FireStarterItem;
        if (fireStarter.m_ConsumeOnUse)
        {
            GameManager.GetPlayerManagerComponent().ConsumeUnitFromInventory(tool.gameObject);
        }
        else
        {
            tool.Degrade(fireStarter.m_ConditionDegradeOnUse);
            if (tool.CheckForBreakOnUse())
                tool.BreakOnUse();
        }

        Mod.Logger.Log($"Used '{tool.name}' now the torch is lit.", FlaggedLoggingLevel.Debug);
    }

    /// <summary>Forgets the held-off tool when the player stops lighting the torch it was picked for.</summary>
    /// <param name="torch">The torch.</param>
    internal static void OnCancelled(TorchItem torch)
    {
        if (s_Pending is not var (pendingTorch, _) || (pendingTorch != null && pendingTorch != torch))
            return;

        s_Pending = null;
        Mod.Logger.Log("Kept the fire tool: the torch wasn't lit.", FlaggedLoggingLevel.Debug);
    }
}

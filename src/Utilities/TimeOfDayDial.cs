namespace UniversalTweaks.Utilities;

/// <summary>
/// <see cref="Settings.PermanentTimeOfDayDial"/>: docks the sun and moon dial on the HUD whenever no other panel is
/// showing it.
/// </summary>
/// <remarks>
/// The game has one dial, <see cref="InterfaceManager.m_TimeWidget"/>, which panels like the radial menu and Rest move
/// under their own anchor while they're open. The HUD has no anchor of its own, so this adds one in the top right corner,
/// level with the sprint icon in the bottom right, and only takes the dial back once the panel that had it is closed. Called every frame from
/// <see cref="Patches.PanelHUDPatches"/>.
/// </remarks>
internal static class TimeOfDayDial
{
    /// <summary>The anchor added to the HUD for the dial, or <see langword="null"/> before it's needed.</summary>
    private static GameObject? s_Anchor;

    /// <summary>Docks the dial on the HUD, or takes it off again, to match the setting.</summary>
    /// <param name="hud">The HUD.</param>
    internal static void Update(Panel_HUD hud)
    {
        InterfaceManager interfaceManager = InterfaceManager.GetInstance();
        if (interfaceManager == null)
            return;

        GameObject dial = InterfaceManager.m_TimeWidget;
        bool docked = dial != null && s_Anchor != null && dial.transform.parent == s_Anchor.transform;

        if (!Settings.Instance.PermanentTimeOfDayDial)
        {
            if (docked && dial!.activeSelf)
                interfaceManager.SetTimeWidgetActive(false);
            return;
        }

        // Another panel is showing the dial.
        if (dial != null && !docked && dial.activeInHierarchy)
            return;

        UIWidget sprintIcon = hud.m_Sprite_SprintCenter != null ? hud.m_Sprite_SprintCenter : hud.m_SprintBar;
        Camera camera = NGUITools.FindCameraForLayer(hud.gameObject.layer);
        if (sprintIcon == null || camera == null)
            return;

        if (s_Anchor == null)
        {
            s_Anchor = new GameObject("UniversalTweaks_TimeWidgetPos") { layer = hud.gameObject.layer };
            s_Anchor.transform.SetParent(hud.transform, false);
        }

        // Mirror the sprint icon in the bottom right corner up to the top right, so the dial follows the HUD's own
        // margins at any resolution.
        Vector3 sprintIconPoint = camera.WorldToViewportPoint(sprintIcon.worldCenter);
        s_Anchor.transform.position =
            camera.ViewportToWorldPoint(new Vector3(sprintIconPoint.x, 1f - sprintIconPoint.y, sprintIconPoint.z));

        if (!docked || !dial!.activeSelf)
            interfaceManager.InitializeAndActivateTimeWidget(s_Anchor.transform);
    }
}

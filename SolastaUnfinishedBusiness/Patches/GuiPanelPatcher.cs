﻿using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using SolastaUnfinishedBusiness.CustomBehaviors;
using SolastaUnfinishedBusiness.Models;

namespace SolastaUnfinishedBusiness.Patches;

public static class GuiPanelPatcher
{
    [HarmonyPatch(typeof(GuiPanel), "Show")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class Show_Patch
    {
        public static void Postfix(GuiPanel __instance)
        {
            //PATCH: Keeps last level up hero selected
            if (__instance is not MainMenuScreen mainMenuScreen || Global.LastLevelUpHeroName == null)
            {
                return;
            }

            mainMenuScreen.charactersPanel.Show();
        }
    }

    [HarmonyPatch(typeof(GuiPanel), "OnBeginHide")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class OnEndHide_Patch
    {
        public static void Prefix(GuiPanel __instance, bool instant)
        {
            //PATCH: Power Bundle: hide sub-power selector when some panels start closing
            if (__instance is PowerSelectionPanel or InvocationSelectionPanel)
            {
                PowerBundle.CloseSubPowerSelectionModal(instant);
            }
        }
    }
}

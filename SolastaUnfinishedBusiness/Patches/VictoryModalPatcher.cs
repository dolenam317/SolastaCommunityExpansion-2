﻿using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace SolastaUnfinishedBusiness.Patches;

public static class VictoryModalPatcher
{
    [HarmonyPatch(typeof(VictoryModal), "OnBeginShow")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class OnBeginShow_Patch
    {
        public static void Prefix([NotNull] VictoryModal __instance)
        {
            //PATCH: scales down the rest sub panel whenever the party size is bigger than 4 (PARTYSIZE)
            if (Gui.GameCampaign.Party.CharactersList.Count > 4)
            {
                __instance.heroStatsGroup.anchoredPosition = new Vector2(-145f, -248f);
                __instance.heroStatsGroup.localScale = new Vector3(0.7225f, 1f, 0.7225f);
            }
            else
            {
                __instance.heroStatsGroup.anchoredPosition = new Vector2(-0, -248f);
                __instance.heroStatsGroup.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}

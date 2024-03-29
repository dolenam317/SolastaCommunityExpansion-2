﻿using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using SolastaUnfinishedBusiness.Models;
using UnityEngine;

namespace SolastaUnfinishedBusiness.Patches;

public static class GraphicsCharacterPatcher
{
    [HarmonyPatch(typeof(GraphicsCharacter), "ResetScale")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public class ResetScale_Patch
    {
        public static void Postfix(GraphicsCharacter __instance, ref float __result)
        {
            //PATCH: Allows custom races with different scales
            if (__instance.RulesetCharacter is not RulesetCharacterHero rulesetCharacterHero ||
                !RacesContext.RaceScaleMap.TryGetValue(rulesetCharacterHero.RaceDefinition, out var scale))
            {
                return;
            }

            __result *= scale;
            __instance.transform.localScale = new Vector3(__result, __result, __result);
        }
    }
}

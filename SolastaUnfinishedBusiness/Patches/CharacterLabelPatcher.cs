﻿using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using SolastaUnfinishedBusiness.Models;

namespace SolastaUnfinishedBusiness.Patches;

public static class CharacterLabelPatcher
{
    [HarmonyPatch(typeof(CharacterLabel), "ConditionAdded")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class ConditionAdded_Patch
    {
        public static void Prefix(CharacterLabel __instance, RulesetActor character, RulesetCondition condition)
        {
            //PATCH: allows certain conditions to report on hero label
            if (Global.CharacterLabelEnabledConditions.Contains(condition.ConditionDefinition))
            {
                __instance.DisplayConditionLabel(character, condition, false);
            }
        }
    }

    [HarmonyPatch(typeof(CharacterLabel), "ConditionRemoved")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class ConditionRemoved_Patch
    {
        public static void Prefix(CharacterLabel __instance, RulesetActor character, RulesetCondition condition)
        {
            //PATCH: allows certain conditions to report on hero label
            if (Global.CharacterLabelEnabledConditions.Contains(condition.ConditionDefinition))
            {
                __instance.DisplayConditionLabel(character, condition, true);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Api.Extensions;
using SolastaUnfinishedBusiness.Api.Helpers;
using SolastaUnfinishedBusiness.CustomBehaviors;
using SolastaUnfinishedBusiness.CustomInterfaces;
using SolastaUnfinishedBusiness.Models;

namespace SolastaUnfinishedBusiness.Patches;

public static class RulesetEffectSpellPatcher
{
    //PATCH: support for `ICustomMagicEffectBasedOnCaster` and `IModifySpellEffect` 
    [HarmonyPatch(typeof(RulesetEffectSpell), "EffectDescription", MethodType.Getter)]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class EffectDescription_Getter_Patch
    {
        public static void Postfix(RulesetEffectSpell __instance, ref EffectDescription __result)
        {
            // allowing to pick and/or tweak spell effect depending on some caster properties
            __result = PowerBundle.ModifySpellEffect(__result, __instance);
        }
    }

    [HarmonyPatch(typeof(RulesetEffectSpell), "SaveDC", MethodType.Getter)]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class SaveDC_Getter_Patch
    {
        public static void Postfix(RulesetEffectSpell __instance, ref int __result)
        {
            //PATCH: allow devices have DC based on user or item summoner stats, instead of static value
            var originItem = __instance.OriginItem;

            if (originItem == null || __result >= 0)
            {
                return;
            }

            var caster = __instance.Caster;
            string className = null;

            if (__result == EffectHelpers.BasedOnItemSummoner)
            {
                caster = EffectHelpers.GetCharacterByEffectGuid(originItem.SourceSummoningEffectGuid) ?? caster;
            }

            var classHolder = originItem.ItemDefinition.GetFirstSubFeatureOfType<IClassHoldingFeature>();

            if (classHolder != null)
            {
                className = classHolder.Class.Name;
            }

            __result = EffectHelpers.CalculateSaveDc(caster, __instance.spellDefinition.effectDescription, className);
        }
    }

    [HarmonyPatch(typeof(RulesetEffectSpell), "MagicAttackBonus", MethodType.Getter)]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class MagicAttackBonus_Getter_Patch
    {
        public static void Postfix(RulesetEffectSpell __instance, ref int __result)
        {
            //PATCH: allow devices have magic attack bonus based on user or item summoner stats, instead of static value
            var originItem = __instance.OriginItem;

            if (originItem == null || originItem.UsableDeviceDescription.magicAttackBonus >= 0)
            {
                return;
            }

            var caster = __instance.Caster;
            string className = null;

            if (__result == EffectHelpers.BasedOnItemSummoner)
            {
                caster = EffectHelpers.GetCharacterByEffectGuid(originItem.SourceSummoningEffectGuid) ?? caster;
            }

            var classHolder = originItem.ItemDefinition.GetFirstSubFeatureOfType<IClassHoldingFeature>();

            if (classHolder != null)
            {
                className = classHolder.Class.Name;
            }

            var repertoire = caster.GetClassSpellRepertoire(className);

            if (repertoire != null)
            {
                __result = repertoire.SpellAttackBonus;
            }
        }
    }

    [HarmonyPatch(typeof(RulesetEffectSpell), "MagicAttackTrends", MethodType.Getter)]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class MagicAttackTrends_Getter_Patch
    {
        public static void Postfix(RulesetEffectSpell __instance, ref List<RuleDefinitions.TrendInfo> __result)
        {
            //PATCH: allow devices have magic attack trends based on user or item summoner stats, instead of static value
            var originItem = __instance.OriginItem;

            if (originItem == null || originItem.UsableDeviceDescription.magicAttackBonus >= 0)
            {
                return;
            }

            var caster = __instance.Caster;
            string className = null;

            if (originItem.UsableDeviceDescription.magicAttackBonus == EffectHelpers.BasedOnItemSummoner)
            {
                caster = EffectHelpers.GetCharacterByEffectGuid(originItem.SourceSummoningEffectGuid) ?? caster;
            }

            var classHolder = originItem.ItemDefinition.GetFirstSubFeatureOfType<IClassHoldingFeature>();

            if (classHolder != null)
            {
                className = classHolder.Class.Name;
            }

            var repertoire = caster.GetClassSpellRepertoire(className);

            if (repertoire != null)
            {
                __result = repertoire.MagicAttackTrends;
            }
        }
    }

    //PATCH: Multiclass: enforces cantrips to be cast at character level 
    [HarmonyPatch(typeof(RulesetEffectSpell), "GetClassLevel")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class GetClassLevel_Patch
    {
        public static void Postfix(RulesetEffectSpell __instance, ref int __result, RulesetCharacter character)
        {
            if (character is not RulesetCharacterHero hero)
            {
                return;
            }

            if (__instance.SpellDefinition.SpellLevel == 0)
            {
                __result = hero.GetAttribute(AttributeDefinitions.CharacterLevel).CurrentValue;
                return;
            }

            //PATCH: support for `IClassHoldingFeature`
            var holder = __instance.SpellDefinition.GetFirstSubFeatureOfType<IClassHoldingFeature>();

            if (holder == null)
            {
                return;
            }

            if (hero.ClassesAndLevels.TryGetValue(holder.Class, out var level))
            {
                __result = level;
            }
        }
    }

    //PATCH: enforces cantrips to be cast at character level
    [HarmonyPatch(typeof(RulesetEffectSpell), "ComputeTargetParameter")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class ComputeTargetParameter_Patch
    {
        [NotNull]
        public static IEnumerable<CodeInstruction> Transpiler([NotNull] IEnumerable<CodeInstruction> instructions)
        {
            //replaces repertoire's SpellCastingLevel with character level for cantrips
            var spellCastingLevelMethod = typeof(RulesetSpellRepertoire).GetMethod("get_SpellCastingLevel");
            var spellCastingLevel =
                new Func<RulesetSpellRepertoire, RulesetEffectSpell, int>(MulticlassContext.SpellCastingLevel).Method;

            return instructions.ReplaceCalls(spellCastingLevelMethod, "RulesetEffectSpell.ComputeTargetParameter",
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, spellCastingLevel));
        }
    }
}

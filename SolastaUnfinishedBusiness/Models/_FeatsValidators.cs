﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Api;
using SolastaUnfinishedBusiness.CustomDefinitions;

namespace SolastaUnfinishedBusiness.Models;

internal static class FeatsValidators
{
    //
    // validation routines for FeatDefinitionWithPrerequisites
    //

    internal static (bool, string) IsElfOrHalfElf(FeatDefinitionWithPrerequisites _,
        [NotNull] RulesetCharacterHero hero)
    {
        var isElf = hero.RaceDefinition.Name.Contains(DatabaseHelper.CharacterRaceDefinitions.Elf.Name);
        var elfTitle = DatabaseHelper.CharacterRaceDefinitions.Elf.FormatTitle();
        var halfElfTitle = DatabaseHelper.CharacterRaceDefinitions.HalfElf.FormatTitle();
        var param = $"{elfTitle}, {halfElfTitle}";

        return isElf
            ? (true, Gui.Format("Tooltip/&FeatPrerequisiteIs", param))
            : (false, Gui.Colorize(Gui.Format("Tooltip/&FeatPrerequisiteIs", param), "EA7171"));
    }

    [NotNull]
    internal static Func<FeatDefinition, RulesetCharacterHero, (bool result, string output)> ValidateMinCharLevel(
        int minCharLevel)
    {
        return (_, hero) =>
        {
            var isLevelValid = hero.ClassesHistory.Count >= minCharLevel;

            if (isLevelValid)
            {
                return (true, Gui.Format("Tooltip/&FeatPrerequisiteLevelFormat", minCharLevel.ToString()));
            }

            return (false,
                Gui.Colorize(Gui.Format("Tooltip/&FeatPrerequisiteLevelFormat", minCharLevel.ToString()),
                    "EA7171"));
        };
    }

    // [NotNull]
    // internal static Func<FeatDefinition, RulesetCharacterHero, (bool result, string output)> ValidateRace(
    //     CharacterRaceDefinition characterRaceDefinition)
    // {
    //     var raceName = characterRaceDefinition.FormatTitle();
    //
    //     return (_, hero) =>
    //     {
    //         var isRace = characterRaceDefinition == hero.RaceDefinition;
    //
    //         return isRace
    //             ? (true, Gui.Format("Tooltip/&FeatPrerequisiteIs", raceName))
    //             : (false, Gui.Colorize(Gui.Format("Tooltip/&FeatPrerequisiteIs", raceName), "EA7171"));
    //     };
    // }

    [NotNull]
    internal static Func<FeatDefinition, RulesetCharacterHero, (bool result, string output)> ValidateNotClass(
        [NotNull] CharacterClassDefinition characterClassDefinition)
    {
        var className = characterClassDefinition.FormatTitle();

        return (_, hero) =>
        {
            var isNotClass = !hero.ClassesAndLevels.ContainsKey(characterClassDefinition);

            return isNotClass
                ? (true, Gui.Format("Tooltip/&FeatPrerequisiteIsNot", className))
                : (false, Gui.Colorize(Gui.Format("Tooltip/&FeatPrerequisiteIsNot", className), "EA7171"));
        };
    }

    internal static (bool, string) ValidateHasStealthAttack(FeatDefinition _, [NotNull] RulesetCharacterHero hero)
    {
        var features = new List<FeatureDefinition>();

        hero.EnumerateFeaturesToBrowse<FeatureDefinitionAdditionalDamage>(features);

        var hasStealthAttack = features.Any(x => x.Name.Contains(TagsDefinitions.AdditionalDamageSneakAttackTag));

        return hasStealthAttack
            ? (true, Gui.Localize("Tooltip/&FeatPrerequisiteHasStealthAttack"))
            : (false, Gui.Colorize(Gui.Localize("Tooltip/&FeatPrerequisiteHasStealthAttack"), "EA7171"));
    }
}

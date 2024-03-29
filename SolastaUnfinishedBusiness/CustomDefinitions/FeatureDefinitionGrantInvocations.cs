﻿using System.Collections.Generic;
using System.Linq;

namespace SolastaUnfinishedBusiness.CustomDefinitions;

internal class FeatureDefinitionGrantInvocations : FeatureDefinition
{
    // ReSharper disable once CollectionNeverUpdated.Local
    internal List<InvocationDefinition> Invocations { get; } = new();

    internal static void GrantInvocations(
        RulesetCharacterHero hero,
        string tag,
        IEnumerable<FeatureDefinition> grantedFeatures)
    {
        var features = grantedFeatures
            .OfType<FeatureDefinitionGrantInvocations>()
            .ToList();

        if (features.Empty())
        {
            return;
        }

        var command = ServiceRepository.GetService<IHeroBuildingCommandService>();

        foreach (var invocation in features.SelectMany(f => f.Invocations))
        {
            command.TrainCharacterFeature(hero, tag, invocation.Name, HeroDefinitions.PointsPoolType.Invocation);
        }
    }

    internal static void RemoveInvocations(
        RulesetCharacterHero hero,
        string tag,
        IEnumerable<FeatureDefinition> removedFeatures)
    {
        var features = removedFeatures
            .OfType<FeatureDefinitionGrantInvocations>()
            .ToList();

        if (features.Empty())
        {
            return;
        }

        var command = ServiceRepository.GetService<IHeroBuildingCommandService>();

        foreach (var invocation in features.SelectMany(f => f.Invocations))
        {
            command.UntrainCharacterFeature(hero, tag, invocation.Name, HeroDefinitions.PointsPoolType.Invocation);
        }
    }
}

﻿using SolastaUnfinishedBusiness.CustomBehaviors;
using SolastaUnfinishedBusiness.CustomInterfaces;

namespace SolastaUnfinishedBusiness.CustomDefinitions;

internal sealed class FeatureDefinitionPowerUseModifier : FeatureDefinition, IPowerUseModifier
{
    internal PowerUseModifier Modifier { get; } = new();

    public FeatureDefinitionPower PowerPool => Modifier.PowerPool;

    public int PoolChangeAmount(RulesetCharacter character)
    {
        return Modifier.PoolChangeAmount(character);
    }
}

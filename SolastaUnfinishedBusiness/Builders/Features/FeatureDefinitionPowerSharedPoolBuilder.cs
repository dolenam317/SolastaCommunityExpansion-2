﻿using System;
using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Api.Infrastructure;
using SolastaUnfinishedBusiness.CustomDefinitions;

namespace SolastaUnfinishedBusiness.Builders.Features;

[UsedImplicitly]
internal class FeatureDefinitionPowerSharedPoolBuilder
    : FeatureDefinitionPowerBuilder<FeatureDefinitionPowerSharedPool, FeatureDefinitionPowerSharedPoolBuilder>
{
    internal override void Validate()
    {
        base.Validate();

        PreConditions.ArgumentIsNotNull(Definition.SharedPool,
            $"FeatureDefinitionPowerSharedPoolBuilder[{Definition.Name}].SharedPool is null.");
        PreConditions.AreEqual(Definition.UsesDetermination, RuleDefinitions.UsesDetermination.Fixed,
            $"FeatureDefinitionPowerSharedPoolBuilder[{Definition.Name}].UsesDetermination must be set to Fixed.");
    }

    internal FeatureDefinitionPowerSharedPoolBuilder SetSharedPool(
        RuleDefinitions.ActivationTime activationTime,
        FeatureDefinitionPower poolPower,
        int costPerUse = 1)
    {
        Definition.activationTime = activationTime;
        Definition.SharedPool = poolPower;
        Definition.rechargeRate = poolPower.RechargeRate; // recharge rate should match pool for tooltips to make sense
        Definition.costPerUse = costPerUse;
        Definition.fixedUsesPerRecharge = 1;
        return this;
    }

    #region Constructors

    protected FeatureDefinitionPowerSharedPoolBuilder(string name, Guid namespaceGuid) : base(name, namespaceGuid)
    {
    }

    protected FeatureDefinitionPowerSharedPoolBuilder(FeatureDefinitionPowerSharedPool original, string name,
        Guid namespaceGuid) : base(original, name, namespaceGuid)
    {
    }

    #endregion
}

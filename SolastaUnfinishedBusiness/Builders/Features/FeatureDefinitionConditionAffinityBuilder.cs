﻿using System;
using JetBrains.Annotations;

namespace SolastaUnfinishedBusiness.Builders.Features;

[UsedImplicitly]
internal class FeatureDefinitionConditionAffinityBuilder
    : FeatureDefinitionBuilder<FeatureDefinitionConditionAffinity,
        FeatureDefinitionConditionAffinityBuilder>
{
    internal FeatureDefinitionConditionAffinityBuilder SetConditionAffinityType(RuleDefinitions.ConditionAffinityType value)
    {
        Definition.conditionAffinityType = value;
        return this;
    }

    internal FeatureDefinitionConditionAffinityBuilder SetConditionType(ConditionDefinition value)
    {
        Definition.conditionType = value.Name;
        return this;
    }
    
    #region Constructors

    protected FeatureDefinitionConditionAffinityBuilder(string name, Guid namespaceGuid) : base(name, namespaceGuid)
    {
    }

    protected FeatureDefinitionConditionAffinityBuilder(string name, string definitionGuid) : base(name,
        definitionGuid)
    {
    }

    protected FeatureDefinitionConditionAffinityBuilder(FeatureDefinitionConditionAffinity original, string name,
        Guid namespaceGuid) : base(original, name, namespaceGuid)
    {
    }

    protected FeatureDefinitionConditionAffinityBuilder(FeatureDefinitionConditionAffinity original, string name,
        string definitionGuid) : base(original, name, definitionGuid)
    {
    }

    #endregion
}

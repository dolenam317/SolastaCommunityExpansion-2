﻿using System;
using SolastaModApi;
using SolastaModApi.Extensions;

namespace SolastaCommunityExpansion.Builders.Features
{
    public sealed class FeatureDefinitionDieRollModifierBuilder : BaseDefinitionBuilder<FeatureDefinitionDieRollModifier>
    {
        private FeatureDefinitionDieRollModifierBuilder(string name, Guid namespaceGuid)
            : base(name, namespaceGuid, Category.None)
        {
        }

        // Add other standard Create methods and constructors as required.

        public static FeatureDefinitionDieRollModifierBuilder Create(string name, Guid namespaceGuid)
        {
            return new FeatureDefinitionDieRollModifierBuilder(name, namespaceGuid);
        }

        public FeatureDefinitionDieRollModifierBuilder SetModifiers(
            RuleDefinitions.RollContext context, int rerollCount, int minRerollValue, string consoleLocalizationKey)
        {
            Definition.SetValidityContext(context);
            Definition.SetRerollLocalizationKey(consoleLocalizationKey);
            Definition.SetRerollCount(rerollCount);
            Definition.SetMinRerollValue(minRerollValue);
            Definition.SetMinRollValue(minRerollValue);
            return this;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using SolastaUnfinishedBusiness.Builders;
using SolastaUnfinishedBusiness.CustomInterfaces;
using static SolastaUnfinishedBusiness.Api.DatabaseHelper;

namespace SolastaUnfinishedBusiness.Feats;

internal static class GroupFeats
{
    private static readonly List<FeatDefinition> Groups = new();

    internal static void Load(Action<FeatDefinition> loader)
    {
        Groups.Add(MakeGroup("FeatGroupCreed", null,
            FeatDefinitions.Creed_Of_Arun,
            FeatDefinitions.Creed_Of_Einar,
            FeatDefinitions.Creed_Of_Maraike,
            FeatDefinitions.Creed_Of_Misaye,
            FeatDefinitions.Creed_Of_Pakri,
            FeatDefinitions.Creed_Of_Solasta));

        Groups.Add(MakeGroup("FeatGroupElementalTouch", "Touch",
            FeatDefinitions.BurningTouch,
            FeatDefinitions.ToxicTouch,
            FeatDefinitions.ElectrifyingTouch,
            FeatDefinitions.IcyTouch,
            FeatDefinitions.MeltingTouch));

        Groups.Add(MakeGroup("FeatGroupTwoHandedCombat", null,
            FeatDefinitions.MightyBlow,
            FeatDefinitions.ForestallingStrength,
            FeatDefinitions.FollowUpStrike));

        Groups.ForEach(loader);
    }

    internal static FeatDefinition MakeGroup(string name, string family, params FeatDefinition[] feats)
    {
        return MakeGroup(name, family, feats.AsEnumerable());
    }

    internal static FeatDefinition MakeGroup(string name, string family, IEnumerable<FeatDefinition> feats)
    {
        var group = FeatDefinitionBuilder
            .Create(name)
            .SetGuiPresentation(Category.Feat)
            .SetCustomSubFeatures(new GroupedFeat(feats))
            .SetFeatFamily(family)
            .SetFeatures()
            .AddToDB();

        Groups.Add(group);

        return group;
    }
}

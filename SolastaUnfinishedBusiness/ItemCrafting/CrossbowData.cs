﻿using System.Collections.Generic;
using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Api;
using static SolastaUnfinishedBusiness.Models.CraftingContext;

namespace SolastaUnfinishedBusiness.ItemCrafting;

internal static class CrossbowData
{
    private static ItemCollection _items;

    [NotNull]
    internal static ItemCollection Items =>
        _items ??= new ItemCollection
        {
            BaseWeapons =
                new List<ItemDefinition>
                {
                    DatabaseHelper.ItemDefinitions.LightCrossbow, DatabaseHelper.ItemDefinitions.HeavyCrossbow
                },
            PossiblePrimedItemsToReplace = new List<ItemDefinition>
            {
                DatabaseHelper.ItemDefinitions.Primed_Longbow, DatabaseHelper.ItemDefinitions.Primed_Shortbow
            },
            MagicToCopy = new List<ItemCollection.MagicItemDataHolder>
            {
                // Same as +1
                new("Accuracy", DatabaseHelper.ItemDefinitions.Enchanted_Longbow_Of_Accurary,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongbowOfAcurracy),
                // Same as +2
                new("Sharpshooting", DatabaseHelper.ItemDefinitions.Enchanted_Shortbow_Of_Sharpshooting,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_ShortbowOfSharpshooting),
                new("Lightbringer", DatabaseHelper.ItemDefinitions.Enchanted_Longbow_Lightbringer,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongbowLightbringer),
                new("Stormbow", DatabaseHelper.ItemDefinitions.Enchanted_Longbow_Stormbow,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_LongsbowStormbow),
                new("Medusa", DatabaseHelper.ItemDefinitions.Enchanted_Shortbow_Medusa,
                    DatabaseHelper.RecipeDefinitions.Recipe_Enchantment_ShortbowMedusa)
            }
        };
}

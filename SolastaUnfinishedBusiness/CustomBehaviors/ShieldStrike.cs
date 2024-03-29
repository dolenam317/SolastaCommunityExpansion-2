﻿using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Builders;
using static SolastaUnfinishedBusiness.Api.DatabaseHelper;

namespace SolastaUnfinishedBusiness.CustomBehaviors;

internal static class ShieldStrike
{
    private static WeaponDescription _shieldWeaponDescription;
    private static WeaponTypeDefinition _shieldWeaponType;

    [NotNull]
    internal static WeaponDescription ShieldWeaponDescription =>
        _shieldWeaponDescription ??= BuildShieldWeaponDescription();

    [NotNull] internal static WeaponTypeDefinition ShieldWeaponType => _shieldWeaponType ??= BuildShieldWeaponType();

    internal static bool IsShield([CanBeNull] RulesetItem item)
    {
        return item != null && IsShield(item.ItemDefinition);
    }

    internal static bool IsShield([CanBeNull] ItemDefinition item)
    {
        if (item == null || !item.IsArmor)
        {
            return false;
        }

        var armorDescription = item.ArmorDescription;

        return armorDescription.ArmorType == "ShieldType";
    }

    [NotNull]
    private static WeaponTypeDefinition BuildShieldWeaponType()
    {
        var shieldType = WeaponTypeDefinitionBuilder
            .Create(WeaponTypeDefinitions.UnarmedStrikeType, "CEShieldStrikeType")
            .AddToDB();

        shieldType.soundEffectOnHitDescription = WeaponTypeDefinitions.ClubType.SoundEffectOnHitDescription;

        return shieldType;
    }

    [NotNull]
    private static WeaponDescription BuildShieldWeaponDescription()
    {
        var description =
            new WeaponDescription(ItemDefinitions.UnarmedStrikeBase.WeaponDescription)
            {
                weaponType = ShieldWeaponType.Name
            };

        var damage = description.EffectDescription.FindFirstDamageForm();

        damage.DieType = RuleDefinitions.DieType.D4;

        return description;
    }
}

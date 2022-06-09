﻿using System.Collections.Generic;
using System.Linq;
using SolastaModApi.Extensions;
using SolastaModApi.Infrastructure;
using TA.AI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RuleDefinitions;

namespace SolastaCommunityExpansion.Builders;

public class EffectFormBuilder
{
    private readonly EffectForm effectForm;

    public EffectFormBuilder()
    {
        effectForm = new EffectForm();
    }

    public EffectFormBuilder(EffectForm reference)
    {
        effectForm = new EffectForm();
        effectForm.Copy(reference);
    }

    public EffectFormBuilder HasSavingThrow(EffectSavingThrowType savingThrowAffinity)
    {
        effectForm.HasSavingThrow = true;
        effectForm.SavingThrowAffinity = savingThrowAffinity;
        return this;
    }

    public EffectFormBuilder CanSaveToCancel(TurnOccurenceType saveOccurence)
    {
        effectForm.CanSaveToCancel = true;
        effectForm.SaveOccurence = saveOccurence;
        return this;
    }

    public EffectFormBuilder CreatedByCharacter()
    {
        effectForm.SetCreatedByCharacter(true);
        return this;
    }

    public EffectFormBuilder CreatedByCondition()
    {
        effectForm.SetCreatedByCondition(true);
        return this;
    }

    public EffectFormBuilder SetBonusMode(AddBonusMode bonusMode)
    {
        effectForm.AddBonusMode = bonusMode;
        return this;
    }

    public EffectFormBuilder SetLevelAdvancement(EffectForm.LevelApplianceType applyLevel,
        LevelSourceType levelType, int levelMultiplier = 1)
    {
        effectForm.SetApplyLevel(applyLevel);
        effectForm.SetLevelType(levelType);
        effectForm.SetLevelMultiplier(levelMultiplier);
        return this;
    }

    public EffectFormBuilder SetAlterationForm(AlterationForm.Type alterationType)
    {
        effectForm.FormType = EffectForm.EffectFormType.Alteration;
        var alterationForm = new AlterationForm();
        alterationForm.alterationType = alterationType;
        effectForm.SetAlterationForm(alterationForm);
        return this;
    }

    public EffectFormBuilder SetAlterationAbilityScore(string abilityScore, int valueIncrease, int maximumIncrease)
    {
        effectForm.FormType = EffectForm.EffectFormType.Alteration;
        var alterationForm = new AlterationForm();
        alterationForm.alterationType = AlterationForm.Type.AbilityScoreIncrease;
        alterationForm.abilityScore = abilityScore;
        alterationForm.valueIncrease = valueIncrease;
        alterationForm.maximumIncrease = maximumIncrease;
        effectForm.SetAlterationForm(alterationForm);
        return this;
    }

    public EffectFormBuilder SetConditionForm(ConditionDefinition condition,
        ConditionForm.ConditionOperation operation)
    {
        return SetConditionForm(condition, operation, false, false, condition);
    }

    public EffectFormBuilder SetConditionForm(ConditionDefinition condition,
        ConditionForm.ConditionOperation operation, bool applyToSelf, bool forceOnSelf,
        params ConditionDefinition[] detrimentalConditions)
    {
        return SetConditionForm(condition, operation, applyToSelf, forceOnSelf,
            detrimentalConditions.AsEnumerable());
    }

    public EffectFormBuilder SetConditionForm(ConditionDefinition condition,
        ConditionForm.ConditionOperation operation, bool applyToSelf, bool forceOnSelf,
        IEnumerable<ConditionDefinition> detrimentalConditions)
    {
        effectForm.FormType = EffectForm.EffectFormType.Condition;
        var conditionForm = new ConditionForm {Operation = operation, ConditionDefinition = condition};
        if (condition != null)
        {
            conditionForm.SetConditionDefinitionName(condition.Name);
        }

        conditionForm.SetApplyToSelf(applyToSelf);
        conditionForm.SetForceOnSelf(forceOnSelf);
        conditionForm.ConditionsList.SetRange(detrimentalConditions.ToList());
        effectForm.ConditionForm = conditionForm;
        return this;
    }

    public EffectFormBuilder SetCounterForm(CounterForm.CounterType type, int automaticSpellLevel, int checkBaseDC,
        bool addSpellCastingAbility, bool addProficiencyBonus)
    {
        effectForm.FormType = EffectForm.EffectFormType.Counter;
        var counterForm = new CounterForm();
        counterForm.type = type;
        counterForm.automaticSpellLevel = automaticSpellLevel;
        counterForm.checkBaseDC = checkBaseDC;
        counterForm.addSpellCastingAbility = addSpellCastingAbility;
        counterForm.addProficiencyBonus = addProficiencyBonus;
        effectForm.SetCounterForm(counterForm);
        return this;
    }

    public EffectFormBuilder SetDamageForm(bool versatile = false, DieType versatileDieType = DieType.D1,
        string damageType = DamageTypeBludgeoning, int bonusDamage = 0,
        DieType dieType = DieType.D1, int diceNumber = 0,
        HealFromInflictedDamage healFromInflictedDamage = HealFromInflictedDamage.Never,
        params TrendInfo[] damageBonusTrends)
    {
        return SetDamageForm(versatile, versatileDieType, damageType, bonusDamage, dieType,
            diceNumber, healFromInflictedDamage, damageBonusTrends.AsEnumerable());
    }

    public EffectFormBuilder SetDamageForm(bool versatile, DieType versatileDieType, string damageType,
        int bonusDamage,
        DieType dieType, int diceNumber, HealFromInflictedDamage healFromInflictedDamage,
        IEnumerable<TrendInfo> damageBonusTrends)
    {
        effectForm.FormType = EffectForm.EffectFormType.Damage;
        var damageForm = new DamageForm();
        damageForm.SetVersatile(versatile);
        damageForm.VersatileDieType = versatileDieType;
        damageForm.BonusDamage = bonusDamage;
        damageForm.DamageType = damageType;
        damageForm.DiceNumber = diceNumber;
        damageForm.DieType = dieType;
        damageForm.SetHealFromInflictedDamage(healFromInflictedDamage);
        damageForm.DamageBonusTrends.SetRange(damageBonusTrends);
        effectForm.DamageForm = damageForm;
        return this;
    }

    public EffectFormBuilder SetKillForm(KillCondition condition, float challengeRating = 0, int hitPoints = 0)
    {
        effectForm.FormType = EffectForm.EffectFormType.Kill;
        var killForm = new KillForm();
        killForm.killCondition = condition;
        killForm.challengeRating = challengeRating;
        killForm.hitPoints = hitPoints;
        effectForm.SetKillForm(killForm);
        return this;
    }

    public EffectFormBuilder SetDivinationForm(DivinationForm.Type divinationType,
        IEnumerable<CharacterFamilyDefinition> creatureFamilies,
        IEnumerable<string> revealedTags, int rangeCells)
    {
        effectForm.FormType = EffectForm.EffectFormType.Divination;
        var divinationForm = new DivinationForm();
        divinationForm.divinationType = divinationType;
        divinationForm.CreatureFamilies.SetRange(creatureFamilies);
        divinationForm.RevealedTags.SetRange(revealedTags);
        divinationForm.rangeCells = rangeCells;
        effectForm.SetDivinationForm(divinationForm);
        return this;
    }

    public EffectFormBuilder SetHealingForm(HealingComputation healingComputation,
        int bonusHitPoints, DieType dieType, int diceNumber, bool variablePool,
        HealingCap healingCap)
    {
        effectForm.FormType = EffectForm.EffectFormType.Healing;

        var healingForm = new HealingForm
        {
            HealingComputation = healingComputation,
            BonusHealing = bonusHitPoints,
            DieType = dieType,
            DiceNumber = diceNumber,
            VariablePool = variablePool,
            HealingCap = healingCap
        };
        effectForm.SetHealingForm(healingForm);
        return this;
    }

    public EffectFormBuilder SetItemPropertyForm(ItemPropertyUsage usageLimitation, int useAmount,
        params FeatureUnlockByLevel[] featureBySlotLevel)
    {
        return SetItemPropertyForm(featureBySlotLevel.AsEnumerable(), usageLimitation, useAmount);
    }

    public EffectFormBuilder SetItemPropertyForm(IEnumerable<FeatureUnlockByLevel> featureBySlotLevel,
        ItemPropertyUsage usageLimitation, int useAmount)
    {
        effectForm.FormType = EffectForm.EffectFormType.ItemProperty;
        var itemForm = new ItemPropertyForm();
        itemForm.SetUsageLimitation(usageLimitation);
        itemForm.SetUseAmount(useAmount);
        itemForm.FeatureBySlotLevel.SetRange(featureBySlotLevel);
        effectForm.SetItemPropertyForm(itemForm);
        return this;
    }

    public EffectFormBuilder SetLightSourceForm(LightSourceType lightSourceType, int brightRange,
        int dimAdditionalRange,
        Color color, AssetReference graphicsPrefabReference)
    {
        effectForm.FormType = EffectForm.EffectFormType.LightSource;
        var lightSourceForm = new LightSourceForm();
        lightSourceForm.SetLightSourceType(lightSourceType);
        lightSourceForm.SetBrightRange(brightRange);
        lightSourceForm.SetDimAdditionalRange(dimAdditionalRange);
        lightSourceForm.SetColor(color);
        lightSourceForm.SetGraphicsPrefabReference(graphicsPrefabReference);
        effectForm.SetLightSourceForm(lightSourceForm);
        return this;
    }

    public EffectFormBuilder SetMotionForm(MotionForm.MotionType motionType, int motionDistance)
    {
        effectForm.FormType = EffectForm.EffectFormType.Motion;
        var motionForm = new MotionForm();
        motionForm.SetType(motionType);
        motionForm.SetDistance(motionDistance);
        effectForm.SetMotionForm(motionForm);
        return this;
    }

    public EffectFormBuilder SetReviveForm(int secondsSinceDeath, ReviveHitPoints reviveHitPoints,
        params ConditionDefinition[] removedConditions)
    {
        return SetReviveForm(secondsSinceDeath, reviveHitPoints, removedConditions.AsEnumerable());
    }

    public EffectFormBuilder SetReviveForm(int secondsSinceDeath, ReviveHitPoints reviveHitPoints,
        IEnumerable<ConditionDefinition> removedConditions)
    {
        effectForm.FormType = EffectForm.EffectFormType.Revive;
        var reviveForm = new ReviveForm();
        reviveForm.maxSecondsSinceDeath = secondsSinceDeath;
        reviveForm.reviveHitPoints = reviveHitPoints;
        reviveForm.RemovedConditions.SetRange(removedConditions);

        effectForm.SetReviveForm(reviveForm);
        return this;
    }

    public EffectFormBuilder SetSpellForm(int maxSlotLevel)
    {
        effectForm.FormType = EffectForm.EffectFormType.SpellSlots;
        var spellSlotsForm = new SpellSlotsForm();
        spellSlotsForm.type = SpellSlotsForm.EffectType.RecoverHalfLevelUp;
        spellSlotsForm.maxSlotLevel = maxSlotLevel;
        effectForm.SetSpellSlotsForm(spellSlotsForm);
        return this;
    }

    public EffectFormBuilder SetSummonForm(SummonForm.Type summonType, ItemDefinition item, int number,
        string monsterDefinitionName, ConditionDefinition conditionDefinition,
        bool persistOnConcentrationLoss, DecisionPackageDefinition decisionPackage,
        EffectProxyDefinition effectProxyDefinition)
    {
        effectForm.FormType = EffectForm.EffectFormType.Summon;
        var summonForm = new SummonForm();
        summonForm.SetSummonType(summonType);
        summonForm.SetItemDefinition(item);
        summonForm.SetNumber(number);
        summonForm.SetMonsterDefinitionName(monsterDefinitionName);
        summonForm.SetConditionDefinition(conditionDefinition);
        summonForm.SetPersistOnConcentrationLoss(persistOnConcentrationLoss);
        summonForm.SetDecisionPackage(decisionPackage);
        summonForm.SetEffectProxyDefinitionName(effectProxyDefinition.Name);
        effectForm.SetSummonForm(summonForm);
        return this;
    }

    public EffectFormBuilder SetSummonCreatureForm(int number, string monsterDefinitionName,
        bool persistOnConcentrationLoss = false, ConditionDefinition condition = null,
        DecisionPackageDefinition decisionPackage = null)
    {
        effectForm.FormType = EffectForm.EffectFormType.Summon;
        var summonForm = new SummonForm();
        summonForm.SetSummonType(SummonForm.Type.Creature);
        summonForm.SetItemDefinition(null);
        summonForm.SetNumber(number);
        summonForm.SetMonsterDefinitionName(monsterDefinitionName);
        summonForm.SetConditionDefinition(condition);
        summonForm.SetPersistOnConcentrationLoss(persistOnConcentrationLoss);
        summonForm.SetDecisionPackage(decisionPackage);
        summonForm.SetEffectProxyDefinitionName(null);
        effectForm.SetSummonForm(summonForm);
        return this;
    }

    public EffectFormBuilder SetSummonItemForm(ItemDefinition item, int number)
    {
        effectForm.FormType = EffectForm.EffectFormType.Summon;
        var summonForm = new SummonForm();
        summonForm.SetSummonType(SummonForm.Type.InventoryItem);
        summonForm.SetItemDefinition(item);
        summonForm.SetNumber(number);
        summonForm.SetMonsterDefinitionName(""); //do we even need this?
        summonForm.SetConditionDefinition(null);
        summonForm.SetPersistOnConcentrationLoss(true);
        summonForm.SetDecisionPackage(null);
        summonForm.SetEffectProxyDefinitionName(null);
        effectForm.SetSummonForm(summonForm);
        return this;
    }

    public EffectFormBuilder SetTempHPForm(int bonusHitPoints, DieType dieType, int diceNumber)
    {
        effectForm.FormType = EffectForm.EffectFormType.TemporaryHitPoints;
        var tempHPForm = new TemporaryHitPointsForm
        {
            BonusHitPoints = bonusHitPoints, DieType = dieType, DiceNumber = diceNumber
        };
        effectForm.SetTemporaryHitPointsForm(tempHPForm);
        return this;
    }

    public EffectFormBuilder SetTopologyForm(TopologyForm.Type changeType, bool impactsFlyingCharacters)
    {
        effectForm.FormType = EffectForm.EffectFormType.Topology;
        var topologyForm = new TopologyForm();
        topologyForm.changeType = changeType;
        topologyForm.impactsFlyingCharacters = impactsFlyingCharacters;
        effectForm.SetTopologyForm(topologyForm);
        return this;
    }

    public EffectForm Build()
    {
        return effectForm;
    }

    public static EffectFormBuilder Create() { return new EffectFormBuilder(); }
}

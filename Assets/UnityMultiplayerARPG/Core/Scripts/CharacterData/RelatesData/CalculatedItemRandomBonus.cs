﻿using System.Collections.Generic;

namespace MultiplayerARPG
{
    public class CalculatedItemRandomBonus
    {
        private static readonly List<System.Action> s_randomActions = new List<System.Action>(32);
        private static readonly List<int> s_randomIndexes = new List<int>();
        private IEquipmentItem _item;
        private int _level;
        private int _randomSeed;
        private byte _version;
        private CharacterStats _cacheIncreaseStats = CharacterStats.Empty;
        private CharacterStats _cacheIncreaseStatsRate = CharacterStats.Empty;
        private Dictionary<Attribute, float> _cacheIncreaseAttributes = new Dictionary<Attribute, float>();
        private Dictionary<Attribute, float> _cacheIncreaseAttributesRate = new Dictionary<Attribute, float>();
        private Dictionary<DamageElement, float> _cacheIncreaseResistances = new Dictionary<DamageElement, float>();
        private Dictionary<DamageElement, float> _cacheIncreaseArmors = new Dictionary<DamageElement, float>();
        private Dictionary<DamageElement, float> _cacheIncreaseArmorsRate = new Dictionary<DamageElement, float>();
        private Dictionary<DamageElement, MinMaxFloat> _cacheIncreaseDamages = new Dictionary<DamageElement, MinMaxFloat>();
        private Dictionary<DamageElement, MinMaxFloat> _cacheIncreaseDamagesRate = new Dictionary<DamageElement, MinMaxFloat>();
        private Dictionary<BaseSkill, int> _cacheIncreaseSkills = new Dictionary<BaseSkill, int>();

        private ItemRandomBonus _randomBonus;
        private int _appliedAmount = 0;

        public CalculatedItemRandomBonus()
        {

        }

        public CalculatedItemRandomBonus(IEquipmentItem item, int level, int randomSeed, byte version)
        {
            Build(item, level, randomSeed, version);
        }

        ~CalculatedItemRandomBonus()
        {
            _cacheIncreaseAttributes.Clear();
            _cacheIncreaseAttributes = null;
            _cacheIncreaseAttributesRate.Clear();
            _cacheIncreaseAttributesRate = null;
            _cacheIncreaseResistances.Clear();
            _cacheIncreaseResistances = null;
            _cacheIncreaseArmors.Clear();
            _cacheIncreaseArmors = null;
            _cacheIncreaseArmorsRate.Clear();
            _cacheIncreaseArmorsRate = null;
            _cacheIncreaseDamages.Clear();
            _cacheIncreaseDamages = null;
            _cacheIncreaseDamagesRate.Clear();
            _cacheIncreaseDamagesRate = null;
            _cacheIncreaseSkills.Clear();
            _cacheIncreaseSkills = null;
        }

        public void Clear()
        {
            _cacheIncreaseStats = CharacterStats.Empty;
            _cacheIncreaseStatsRate = CharacterStats.Empty;
            _cacheIncreaseAttributes.Clear();
            _cacheIncreaseAttributesRate.Clear();
            _cacheIncreaseResistances.Clear();
            _cacheIncreaseArmors.Clear();
            _cacheIncreaseArmorsRate.Clear();
            _cacheIncreaseDamages.Clear();
            _cacheIncreaseDamagesRate.Clear();
            _cacheIncreaseSkills.Clear();
        }

        public void Build(IEquipmentItem item, int level, int randomSeed, byte version)
        {
            _item = item;
            _level = level;
            _randomSeed = randomSeed;
            _version = version;

            Clear();

            if (item == null || !item.IsEquipment())
                return;

            _randomBonus = item.RandomBonus;
            System.Random random = new System.Random(_randomSeed);
            s_randomActions.Clear();
            s_randomActions.Add(() => RandomAttributeAmounts(random));
            s_randomActions.Add(() => RandomAttributeAmountRates(random));
            s_randomActions.Add(() => RandomResistanceAmounts(random));
            s_randomActions.Add(() => RandomArmorAmounts(random));
            s_randomActions.Add(() => RandomDamageAmounts(random));
            s_randomActions.Add(() => RandomSkillLevels(random));
            s_randomActions.Add(() => RandomCharacterStats(random, false));
            s_randomActions.Add(() => RandomCharacterStats(random, true));
            if (version > 0)
            {
                s_randomActions.Add(() => RandomArmorAmountRates(random));
                s_randomActions.Add(() => RandomDamageAmountRates(random));
            }
            s_randomActions.Shuffle(random);
            for (int i = 0; i < s_randomActions.Count; ++i)
            {
                if (IsReachedMaxRandomStatsAmount())
                    break;
                s_randomActions[i].Invoke();
            }
            s_randomActions.Clear();
        }

        public bool IsReachedMaxRandomStatsAmount()
        {
            return _randomBonus.maxRandomStatsAmount > 0 && _appliedAmount >= _randomBonus.maxRandomStatsAmount;
        }

        private void PrepareRandomingIndexes(int length, System.Random random)
        {
            s_randomIndexes.Clear();
            for (int i = 0; i < length; ++i)
            {
                s_randomIndexes.Add(i);
            }
            s_randomIndexes.Shuffle(random);
        }

        public void RandomAttributeAmounts(System.Random random)
        {
            if (_randomBonus.randomAttributeAmounts != null && _randomBonus.randomAttributeAmounts.Length > 0)
            {
                int length = _randomBonus.randomAttributeAmounts.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomAttributeAmounts[index].Apply(random)) continue;
                    _cacheIncreaseAttributes = GameDataHelpers.CombineAttributes(_cacheIncreaseAttributes, _randomBonus.randomAttributeAmounts[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomAttributeAmountRates(System.Random random)
        {
            if (_randomBonus.randomAttributeAmountRates != null && _randomBonus.randomAttributeAmountRates.Length > 0)
            {
                int length = _randomBonus.randomAttributeAmountRates.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomAttributeAmountRates[index].Apply(random)) continue;
                    _cacheIncreaseAttributesRate = GameDataHelpers.CombineAttributes(_cacheIncreaseAttributesRate, _randomBonus.randomAttributeAmountRates[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomResistanceAmounts(System.Random random)
        {
            if (_randomBonus.randomResistanceAmounts != null && _randomBonus.randomResistanceAmounts.Length > 0)
            {
                int length = _randomBonus.randomResistanceAmounts.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomResistanceAmounts[index].Apply(random)) continue;
                    _cacheIncreaseResistances = GameDataHelpers.CombineResistances(_cacheIncreaseResistances, _randomBonus.randomResistanceAmounts[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomArmorAmounts(System.Random random)
        {
            if (_randomBonus.randomArmorAmounts != null && _randomBonus.randomArmorAmounts.Length > 0)
            {
                int length = _randomBonus.randomArmorAmounts.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomArmorAmounts[index].Apply(random)) continue;
                    _cacheIncreaseArmors = GameDataHelpers.CombineArmors(_cacheIncreaseArmors, _randomBonus.randomArmorAmounts[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomArmorAmountRates(System.Random random)
        {
            if (_randomBonus.randomArmorAmountRates != null && _randomBonus.randomArmorAmountRates.Length > 0)
            {
                int length = _randomBonus.randomArmorAmountRates.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomArmorAmountRates[index].Apply(random)) continue;
                    _cacheIncreaseArmorsRate = GameDataHelpers.CombineArmors(_cacheIncreaseArmorsRate, _randomBonus.randomArmorAmountRates[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomDamageAmounts(System.Random random)
        {
            if (_randomBonus.randomDamageAmounts != null && _randomBonus.randomDamageAmounts.Length > 0)
            {
                int length = _randomBonus.randomDamageAmounts.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomDamageAmounts[index].Apply(random)) continue;
                    _cacheIncreaseDamages = GameDataHelpers.CombineDamages(_cacheIncreaseDamages, _randomBonus.randomDamageAmounts[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomDamageAmountRates(System.Random random)
        {
            if (_randomBonus.randomDamageAmountRates != null && _randomBonus.randomDamageAmountRates.Length > 0)
            {
                int length = _randomBonus.randomDamageAmountRates.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomDamageAmountRates[index].Apply(random)) continue;
                    _cacheIncreaseDamagesRate = GameDataHelpers.CombineDamages(_cacheIncreaseDamagesRate, _randomBonus.randomDamageAmountRates[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomSkillLevels(System.Random random)
        {
            if (_randomBonus.randomSkillLevels != null && _randomBonus.randomSkillLevels.Length > 0)
            {
                int length = _randomBonus.randomSkillLevels.Length;
                if (_version > 1)
                    PrepareRandomingIndexes(length, random);
                for (int i = 0; i < length; ++i)
                {
                    int index = i;
                    if (_version > 1)
                        index = s_randomIndexes[i];
                    if (!_randomBonus.randomSkillLevels[index].Apply(random)) continue;
                    _cacheIncreaseSkills = GameDataHelpers.CombineSkills(_cacheIncreaseSkills, _randomBonus.randomSkillLevels[index].GetRandomedAmount(random).ToKeyValuePair(1f));
                    _appliedAmount++;
                    if (IsReachedMaxRandomStatsAmount())
                        return;
                }
            }
        }

        public void RandomCharacterStatsHp(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyHp(random))
            {
                stats.hp = randomStats.GetRandomedHp(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsHpRecovery(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyHpRecovery(random))
            {
                stats.hpRecovery = randomStats.GetRandomedHpRecovery(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsHpLeechRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyHpLeechRate(random))
            {
                stats.hpLeechRate = randomStats.GetRandomedHpLeechRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsMp(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyMp(random))
            {
                stats.mp = randomStats.GetRandomedMp(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsMpRecovery(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyMpRecovery(random))
            {
                stats.mpRecovery = randomStats.GetRandomedMpRecovery(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsMpLeechRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyMpLeechRate(random))
            {
                stats.mpLeechRate = randomStats.GetRandomedMpLeechRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsStamina(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyStamina(random))
            {
                stats.stamina = randomStats.GetRandomedStamina(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsStaminaRecovery(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyStaminaRecovery(random))
            {
                stats.staminaRecovery = randomStats.GetRandomedStaminaRecovery(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsStaminaLeechRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyStaminaLeechRate(random))
            {
                stats.staminaLeechRate = randomStats.GetRandomedStaminaLeechRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsFood(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyFood(random))
            {
                stats.food = randomStats.GetRandomedFood(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsWater(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyWater(random))
            {
                stats.water = randomStats.GetRandomedWater(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsAccuracy(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyAccuracy(random))
            {
                stats.accuracy = randomStats.GetRandomedAccuracy(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsEvasion(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyEvasion(random))
            {
                stats.evasion = randomStats.GetRandomedEvasion(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsCriRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyCriRate(random))
            {
                stats.criRate = randomStats.GetRandomedCriRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsCriDmgRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyCriDmgRate(random))
            {
                stats.criDmgRate = randomStats.GetRandomedCriDmgRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsBlockRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyBlockRate(random))
            {
                stats.blockRate = randomStats.GetRandomedBlockRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsBlockDmgRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyBlockDmgRate(random))
            {
                stats.blockDmgRate = randomStats.GetRandomedBlockDmgRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsMoveSpeed(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyMoveSpeed(random))
            {
                stats.moveSpeed = randomStats.GetRandomedMoveSpeed(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsAtkSpeed(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyAtkSpeed(random))
            {
                stats.atkSpeed = randomStats.GetRandomedAtkSpeed(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsWeightLimit(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyWeightLimit(random))
            {
                stats.weightLimit = randomStats.GetRandomedWeightLimit(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsSlotLimit(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplySlotLimit(random))
            {
                stats.slotLimit = randomStats.GetRandomedSlotLimit(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsGoldRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyGoldRate(random))
            {
                stats.goldRate = randomStats.GetRandomedGoldRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsExpRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyExpRate(random))
            {
                stats.expRate = randomStats.GetRandomedExpRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsItemDropRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyItemDropRate(random))
            {
                stats.itemDropRate = randomStats.GetRandomedItemDropRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsJumpHeight(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyJumpHeight(random))
            {
                stats.jumpHeight = randomStats.GetRandomedJumpHeight(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsHeadDamageAbsorbs(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyHeadDamageAbsorbs(random))
            {
                stats.headDamageAbsorbs = randomStats.GetRandomedHeadDamageAbsorbs(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsBodyDamageAbsorbs(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyBodyDamageAbsorbs(random))
            {
                stats.bodyDamageAbsorbs = randomStats.GetRandomedBodyDamageAbsorbs(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsFallDamageAbsorbs(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyFallDamageAbsorbs(random))
            {
                stats.fallDamageAbsorbs = randomStats.GetRandomedFallDamageAbsorbs(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStatsGravityRate(System.Random random, RandomCharacterStats randomStats, ref CharacterStats stats)
        {
            if (randomStats.ApplyGravityRate(random))
            {
                stats.gravityRate = randomStats.GetRandomedGravityRate(random);
                _appliedAmount++;
            }
        }

        public void RandomCharacterStats(System.Random random, bool isRate)
        {
            CharacterStats tempStats = isRate ? _cacheIncreaseStatsRate : _cacheIncreaseStats;
            RandomCharacterStats randomStats = isRate ? _randomBonus.randomCharacterStatsRate : _randomBonus.randomCharacterStats;
            s_randomActions.Clear();
            s_randomActions.Add(() => RandomCharacterStatsHp(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsHpRecovery(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsHpLeechRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsMp(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsMpRecovery(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsMpLeechRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsStamina(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsStaminaRecovery(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsStaminaLeechRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsFood(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsWater(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsAccuracy(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsEvasion(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsCriRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsCriDmgRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsBlockRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsBlockDmgRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsMoveSpeed(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsAtkSpeed(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsWeightLimit(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsSlotLimit(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsGoldRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsExpRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsItemDropRate(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsJumpHeight(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsHeadDamageAbsorbs(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsBodyDamageAbsorbs(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsFallDamageAbsorbs(random, randomStats, ref tempStats));
            s_randomActions.Add(() => RandomCharacterStatsGravityRate(random, randomStats, ref tempStats));
            if (_version > 0)
                s_randomActions.Shuffle(random);
            for (int i = 0; i < s_randomActions.Count; ++i)
            {
                if (IsReachedMaxRandomStatsAmount())
                    break;
                s_randomActions[i].Invoke();
            }
            s_randomActions.Clear();
            if (GameExtensionInstance.onRandomCharacterStats != null)
                GameExtensionInstance.onRandomCharacterStats(random, _randomBonus, isRate, ref tempStats, ref _appliedAmount);
            if (isRate)
                _cacheIncreaseStatsRate = tempStats;
            else
                _cacheIncreaseStats = tempStats;
        }

        public IEquipmentItem GetItem()
        {
            return _item;
        }

        public int GetRandomSeed()
        {
            return _randomSeed;
        }

        public CharacterStats GetIncreaseStats()
        {
            return _cacheIncreaseStats;
        }

        public CharacterStats GetIncreaseStatsRate()
        {
            return _cacheIncreaseStatsRate;
        }

        public Dictionary<Attribute, float> GetIncreaseAttributes()
        {
            return _cacheIncreaseAttributes;
        }

        public Dictionary<Attribute, float> GetIncreaseAttributesRate()
        {
            return _cacheIncreaseAttributesRate;
        }

        public Dictionary<DamageElement, float> GetIncreaseResistances()
        {
            return _cacheIncreaseResistances;
        }

        public Dictionary<DamageElement, float> GetIncreaseArmors()
        {
            return _cacheIncreaseArmors;
        }

        public Dictionary<DamageElement, float> GetIncreaseArmorsRate()
        {
            return _cacheIncreaseArmorsRate;
        }

        public Dictionary<DamageElement, MinMaxFloat> GetIncreaseDamages()
        {
            return _cacheIncreaseDamages;
        }

        public Dictionary<DamageElement, MinMaxFloat> GetIncreaseDamagesRate()
        {
            return _cacheIncreaseDamagesRate;
        }

        public Dictionary<BaseSkill, int> GetIncreaseSkills()
        {
            return _cacheIncreaseSkills;
        }
    }
}

﻿using UnityEngine;

[System.Serializable]
public struct IncrementalFloat
{
    public float baseAmount;
    public float amountIncreaseEachLevel;
    [Tooltip("It won't automatically sort by `minLevel`, you have to sort it from low to high to make it calculate properly")]
    public IncrementalFloatByLevel[] amountIncreaseEachLevelByLevels;

    public float GetAmount(int level)
    {
        if (amountIncreaseEachLevelByLevels == null || amountIncreaseEachLevelByLevels.Length == 0)
            return baseAmount + (amountIncreaseEachLevel * (level - 1));
        float result = baseAmount;
        int countLevel = 2;
        int indexOfIncremental = 0;
        int firstMinLevel = amountIncreaseEachLevelByLevels[indexOfIncremental].minLevel;
        while (countLevel <= level)
        {
            if (countLevel < firstMinLevel)
                result += amountIncreaseEachLevel;
            else
                result += amountIncreaseEachLevelByLevels[indexOfIncremental].amountIncreaseEachLevel;
            countLevel++;
            if (indexOfIncremental + 1 < amountIncreaseEachLevelByLevels.Length && countLevel >= amountIncreaseEachLevelByLevels[indexOfIncremental + 1].minLevel)
                indexOfIncremental++;
        }
        return result;
    }
}

[System.Serializable]
public struct IncrementalFloatByLevel
{
    public int minLevel;
    public float amountIncreaseEachLevel;
}

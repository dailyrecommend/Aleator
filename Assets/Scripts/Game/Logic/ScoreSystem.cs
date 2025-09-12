using Data;
using UnityEngine;

namespace GameLayer.Logic
{
    public static class ScoreSystem
    {
        // 스텝1: 기본만. 각 주사위 눈 수를 칩에 더하고, 카테고리 기본칩/배수는 CSV에서.
        public static void PreviewCategory(CategoryRecord cat, int diceSum, out int chips, out int mult)
        {
            chips = cat.baseChips + diceSum;
            mult  = cat.baseMultiplier;
        }
    }
}
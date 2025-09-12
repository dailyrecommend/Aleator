namespace Data
{
    public sealed class CategoryRecord
    {
        public string name;
        public int level;
        public int baseChips;
        public int baseMultiplier;
        public int chipsPerLevel;
        public int multiplierPerLevel;

        // 파생 값 계산 헬퍼
        public int GetChipsAtLevel(int targetLevel)
        {
            var delta = targetLevel - level;
            if (delta < 0) delta = 0;

            return baseChips + chipsPerLevel * delta;
        }

        public int GetMultiplierAtLevel(int targetLevel)
        {
            var delta = targetLevel - level;
            if (delta < 0) delta = 0;

            return baseMultiplier + multiplierPerLevel * delta;
        }
    }
}
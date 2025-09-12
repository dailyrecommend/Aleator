using System;
using System.Collections.Generic;

namespace Data
{
    public static class Validators
    {
        public const string DUP_KEY = "duplicate key";
        public const string NEGATIVE_VALUE = "negative value";
        public const string COLUMN_MISMATCH = "column mismatch";

        public static void ValidateCategories(List<CategoryRecord> list)
        {
            var keySet = new HashSet<string>();

            foreach (var r in list)
            {
                var key = $"{r.name}#{r.level}";
                if (!keySet.Add(key))
                    throw new Exception($"{DUP_KEY}: {key}");

                if (r.level < 1 ||
                    r.baseChips < 0 || r.baseMultiplier < 0 ||
                    r.chipsPerLevel < 0 || r.multiplierPerLevel < 0)
                    throw new Exception($"{NEGATIVE_VALUE}: {key}");
            }
        }
    }
}
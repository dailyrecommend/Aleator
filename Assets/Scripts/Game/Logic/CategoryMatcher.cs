using System.Collections.Generic;
using System.Linq;
using Data;

namespace GameLayer.Logic
{
    public static class CategoryMatcher
    {
        public static bool IsMatch(CategoryRecord cat, IReadOnlyList<int> dice)
        {
            var vals = dice.Where(v => v > 0).ToList();
            if (vals.Count == 0) return false;

            switch (cat.name)
            {
                case "Ace":   return vals.Contains(1);
                case "Two":   return vals.Contains(2);
                case "Three": return vals.Contains(3);
                case "Four":  return vals.Contains(4);
                case "Five":  return vals.Contains(5);
                case "Six":   return vals.Contains(6);

                case "Four_Kind":
                    return vals.GroupBy(v => v).Any(g => g.Count() >= 4);

                case "Full_House":
                {
                    var gs = vals.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(x=>x).ToArray();
                    return gs.Length >= 2 && gs[0] == 3 && gs[1] == 2;
                }

                case "Small_Straight": return HasStraight(vals, 4);
                case "Large_Straight": return HasStraight(vals, 5);
                case "Yahtzee":        return vals.GroupBy(v => v).Any(g => g.Count() == 5);
                case "Chance":         return true;

                default: return false; // 이름 기반. 추후 id 도입 권장.
            }
        }

        static bool HasStraight(List<int> vals, int need)
        {
            var u = vals.Distinct().OrderBy(v => v).ToArray();
            int streak = 1;
            for (int i = 1; i < u.Length; i++)
            {
                if (u[i] == u[i-1] + 1) { streak++; if (streak >= need) return true; }
                else streak = 1;
            }
            return false;
        }
    }
}
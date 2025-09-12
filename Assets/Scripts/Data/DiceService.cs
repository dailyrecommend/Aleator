using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Data
{
    public sealed class DiceService : MonoBehaviour
    {
        private const string FILE_NAME = "dice.csv";
        private readonly Dictionary<string, DiceRecord> map = new();
        private List<DiceRecord> list = new();

        public IReadOnlyList<DiceRecord> All => list;
        public bool TryGet(string id, out DiceRecord r) => map.TryGetValue(id, out r!);

        public void LoadFromStreamingAssets()
        {
            map.Clear(); list = new List<DiceRecord>();
            var path = Path.Combine(Application.streamingAssetsPath, "Data", FILE_NAME);
            if (!File.Exists(path)) { Debug.LogError($"dice.csv not found: {path}"); return; }

            var rows = Data.CsvReader.ReadAll(path);
            if (rows.Count == 0) return;
            var h = rows[0]; if (h.Length != 6) throw new Exception("dice.csv header mismatch");

            for (int i = 1; i < rows.Count; i++)
            {
                var c = rows[i]; if (c.Length == 0) continue; if (c.Length != 6) throw new Exception($"dice.csv row {i+1} mismatch");
                var r = new DiceRecord { id=c[0].Trim(), name=c[1].Trim(), rarity=c[2].Trim(), desc=c[3].Trim(), effectId=c[4].Trim(), @params=c[5] };
                if (!map.TryAdd(r.id, r)) throw new Exception($"duplicate dice id: {r.id}");
                list.Add(r);
            }
            Debug.Log($"DiceService loaded: {list.Count}");
        }
    }
}
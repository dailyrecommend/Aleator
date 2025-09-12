using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Data
{
    public sealed class TesseraService : MonoBehaviour
    {
        private const string FILE_NAME = "tessera.csv";

        private readonly Dictionary<string, TesseraRecord> _map = new();
        private List<TesseraRecord> _list = new();

        public IReadOnlyList<TesseraRecord> All => _list;

        public bool TryGet(string id, out TesseraRecord record) => _map.TryGetValue(id, out record!);

        public void LoadFromStreamingAssets()
        {
            _map.Clear();
            _list = new List<TesseraRecord>();

            var path = Path.Combine(Application.streamingAssetsPath, "Data", FILE_NAME);
            if (!File.Exists(path))
            {
                Debug.LogError($"tessera.csv not found: {path}");
                return;
            }

            var rows = CsvReader.ReadAll(path);
            if (rows.Count == 0) return;

            var header = rows[0];
            if (header.Length != 6) throw new Exception($"{Validators.COLUMN_MISMATCH}: expected 6, got {header.Length}");

            for (int i = 1; i < rows.Count; i++)
            {
                var cols = rows[i];
                if (cols.Length == 0) continue;
                if (cols.Length != 6) throw new Exception($"{Validators.COLUMN_MISMATCH} at row {i + 1}");

                var r = new TesseraRecord
                {
                    id = cols[0].Trim(),
                    name = cols[1].Trim(),
                    rarity = cols[2].Trim(),
                    desc = cols[3].Trim(),
                    effectId = cols[4].Trim(),
                    @params = ParseJsonDict(cols[5]),
                };

                if (!_map.TryAdd(r.id, r)) throw new Exception($"{Validators.DUP_KEY}: {r.id}");
                _list.Add(r);
            }

            Debug.Log($"TesseraService loaded: {_list.Count}");
        }

        private static Dictionary<string, string> ParseJsonDict(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new();
            try
            {
                // 매우 가벼운 파서: JsonUtility는 Dictionary 미지원 → MiniJson
                return MiniJson.Deserialize(json) as Dictionary<string, object> is { } obj
                    ? ToStringDict(obj)
                    : new();
            }
            catch { return new(); }
        }

        private static Dictionary<string, string> ToStringDict(Dictionary<string, object> src)
        {
            var dst = new Dictionary<string, string>(src.Count, StringComparer.Ordinal);
            foreach (var kv in src) dst[kv.Key] = kv.Value?.ToString() ?? "";
            return dst;
        }
    }
}
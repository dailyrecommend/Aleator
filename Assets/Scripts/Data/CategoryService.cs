using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
    public sealed class CategoryService : MonoBehaviour
    {
        private const string FILE_NAME = "categories.csv";
        private readonly Dictionary<string, CategoryRecord> _map = new();
        private List<CategoryRecord> _list = new();

        public IReadOnlyList<CategoryRecord> All => _list;

        public void LoadFromStreamingAssets()
        {
            _map.Clear();
            _list = new List<CategoryRecord>();

            var path = Path.Combine(Application.streamingAssetsPath, "Data", FILE_NAME);

            string csvText = LoadTextSync(path);
            if (string.IsNullOrEmpty(csvText))
            {
                Debug.LogError($"categories.csv not found or empty: {path}");
                return;
            }

            var rows = CsvReader.ReadAllFromString(csvText);
            if (rows.Count <= 1) return;

            for (int i = 1; i < rows.Count; i++)
            {
                var cols = rows[i];
                if (cols.Length < 6) continue;

                var r = new CategoryRecord
                {
                    name = cols[0].Trim(),
                    level = int.Parse(cols[1], CultureInfo.InvariantCulture),
                    baseChips = int.Parse(cols[2], CultureInfo.InvariantCulture),
                    baseMultiplier = int.Parse(cols[3], CultureInfo.InvariantCulture),
                    chipsPerLevel = int.Parse(cols[4], CultureInfo.InvariantCulture),
                    multiplierPerLevel = int.Parse(cols[5], CultureInfo.InvariantCulture),
                };

                _list.Add(r);
                _map[$"{r.name}#{r.level}"] = r;
            }

            Debug.Log($"CategoryService loaded: {_list.Count}");
        }

        string LoadTextSync(string path)
        {
            if (path.Contains("://") || path.Contains(":///"))
            {
                // Android/iOS/WebGL → UnityWebRequest 필요
                using var req = UnityWebRequest.Get(path);
                req.SendWebRequest();
                while (!req.isDone) { } // 동기 대기 (작은 CSV라면 허용)
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load CSV: {path} \n{req.error}");
                    return null;
                }
                return req.downloadHandler.text;
            }
            else
            {
                // PC/Editor → File IO 가능
                return File.Exists(path) ? File.ReadAllText(path) : null;
            }
        }
    }
}
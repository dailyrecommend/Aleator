using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLayer.UI
{
    public sealed class CategoryListUI : MonoBehaviour
    {
        [SerializeField] Transform root;           // VerticalLayoutGroup 컨테이너
        [SerializeField] Button itemPrefab;        // 텍스트 포함 프리팹
        [SerializeField] TMP_Text counterLabel;    // "현재/최대" 표기(옵션)

        readonly HashSet<string> used = new();
        List<CategoryRecord> cached = new();
        System.Action<CategoryRecord> onPick;

        public void ClearAll()
        {
            foreach (Transform c in root) Destroy(c.gameObject);
            if (counterLabel) counterLabel.text = "가능: 0";
        }

// Init에서 자동 생성 제거 (onPick만 세팅)
        public void Init(CategoryService svc, System.Action<CategoryRecord> onPickCallback)
        {
            onPick = onPickCallback;
            cached = new List<CategoryRecord>(svc.All);
            ClearAll(); // ← 시작 시 비움
        }

        public void MarkUsed(string name) { used.Add(name); Rebuild(); }

        void Rebuild()
        {
            foreach (Transform c in root) Destroy(c.gameObject);

            foreach (var r in cached)
            {
                if (used.Contains(r.name)) continue;
                var b = Instantiate(itemPrefab, root);
                var label = b.GetComponentInChildren<TMP_Text>();
                if (label) label.text = r.name;
                var rec = r;
                b.onClick.AddListener(() => onPick?.Invoke(rec));
            }

            if (counterLabel) counterLabel.text = $"{root.childCount}";
        }
        public void ShowAll()
        {
            foreach (Transform c in root) Destroy(c.gameObject);
            foreach (var r in cached)
            {
                var b = Instantiate(itemPrefab, root);
                var label = b.GetComponentInChildren<TMP_Text>();
                if (label) label.text = r.name;
            }
            if (counterLabel) counterLabel.text = $"전체: {root.childCount}";
        }
        
        public void ClearUsed()
        {
            used.Clear();
            foreach (Transform c in root) Destroy(c.gameObject);
            if (counterLabel) counterLabel.text = "가능: 0";
        }
        
        public void RefreshAvailable(IReadOnlyList<int> diceValues)
        {
            foreach (Transform c in root) Destroy(c.gameObject);

            foreach (var r in cached)
            {
                if (used.Contains(r.name)) continue;
                if (!GameLayer.Logic.CategoryMatcher.IsMatch(r, diceValues)) continue;

                var b = Instantiate(itemPrefab, root);
                var label = b.GetComponentInChildren<TMP_Text>();
                if (label) label.text = r.name;
                var rec = r;
                b.onClick.AddListener(() => onPick?.Invoke(rec));
            }

            if (counterLabel) counterLabel.text = $"가능: {root.childCount}";
        }
    }
}
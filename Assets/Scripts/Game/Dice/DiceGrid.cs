
using System.Collections.Generic;
using UnityEngine;

namespace GameLayer.Dice
{
    public enum DiceSortMode { None, ValueAsc, ValueDesc, HeldLast, HeldFirst }
    
    public sealed class DiceGrid : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] Transform root;           // HorizontalLayoutGroup 컨테이너
        [SerializeField] DiceSlot slotPrefab;
        [SerializeField] int initialCount = 5;

        [Header("Sort")]
        [SerializeField] DiceSortMode sortMode = DiceSortMode.None;

        readonly List<DiceSlot> _slots = new();
        System.Random _rng = new();

        public IReadOnlyList<DiceSlot> Slots => _slots;

        void Awake()
        {
            if (!root) root = transform;
            Rebuild(initialCount);
        }

        // 개수 가변 설정
        public void Rebuild(int count, int sides = 6)
        {
            count = Mathf.Max(0, count);

            // 추가 생성
            while (_slots.Count < count)
            {
                var s = Instantiate(slotPrefab, root);
                s.Clear();
                s.SetSides(sides);
                _slots.Add(s);
            }

            // 초과 제거
            while (_slots.Count > count)
            {
                var last = _slots[_slots.Count - 1];
                _slots.RemoveAt(_slots.Count - 1);
                if (last) Destroy(last.gameObject);
            }

            ApplySort();
        }

        public void RollAll()
        {
            foreach (var s in _slots) s.Roll(_rng);
            ApplySort();
        }

        public void ClearAll()
        {
            foreach (var s in _slots) s.Clear();
            ApplySort();
        }

        public void SetSortMode(DiceSortMode mode)
        {
            sortMode = mode;
            ApplySort();
        }

        public void ApplySort()
        {
            if (sortMode == DiceSortMode.None) return;

            _slots.Sort((a, b) =>
            {
                switch (sortMode)
                {
                    case DiceSortMode.ValueAsc:  return a.Value.CompareTo(b.Value);
                    case DiceSortMode.ValueDesc: return b.Value.CompareTo(a.Value);
                    case DiceSortMode.HeldLast:  return a.Held == b.Held ? 0 : (a.Held ? 1 : -1);
                    case DiceSortMode.HeldFirst: return a.Held == b.Held ? 0 : (a.Held ? -1 : 1);
                    default: return 0;
                }
            });

            for (int i = 0; i < _slots.Count; i++)
                _slots[i].transform.SetSiblingIndex(i);
        }
    }
}
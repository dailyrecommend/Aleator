using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLayer.Round
{
    public sealed class RoundSelectUI : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] RectTransform panel;      // 전체 카드 패널
        [SerializeField] Transform cardRoot;       // HorizontalLayoutGroup가 붙은 컨테이너
        [SerializeField] RoundCard cardPrefab;

        [Header("Anim")]
        [SerializeField] float slideDistance = 600f;   // 아래로 이동 거리
        [SerializeField] float slideDuration = 0.35f;

        readonly List<RoundInfo> rounds = new();
        int currentIndex;

        System.Action<RoundInfo> onSelected;

        public void Init(System.Action<RoundInfo> onSelectedCallback)
        {
            onSelected = onSelectedCallback;

            rounds.Clear();
            rounds.Add(new RoundInfo { id="r1", name="Small Round", requiredScore=100 });
            rounds.Add(new RoundInfo { id="r2", name="Big Round", requiredScore=150 });
            rounds.Add(new RoundInfo { id="r3", name="Boss Round", requiredScore=200 });

            Build();
        }

        
        float initY;

        void Awake()
        {
            if (!panel) panel = transform as RectTransform;
            initY = panel.anchoredPosition.y;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            // 선택 영역을 원위치로
            if (panel) panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, initY);
        }
        
        public void SetCurrentIndex(int index)
        {
            currentIndex = Mathf.Clamp(index, 0, rounds.Count - 1);
            Build();
        }

        void Build()
        {
            foreach (Transform c in cardRoot) Destroy(c.gameObject);

            for (int i = 0; i < rounds.Count; i++)
            {
                var card = Instantiate(cardPrefab, cardRoot);
                var iCopy = i;
                card.Bind(rounds[i], i == currentIndex, () => Select(iCopy));
            }
        }

        void Select(int index)
        {
            if (index != currentIndex) return;
            StartCoroutine(SlideDownAndNotify(rounds[index]));
        }

        IEnumerator SlideDownAndNotify(RoundInfo info)
        {
            var rt = panel;
            var startY = rt.anchoredPosition.y;
            var endY = startY - slideDistance;
            float t = 0f;

            while (t < slideDuration)
            {
                t += Time.deltaTime;
                var k = Mathf.Clamp01(t / slideDuration);
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, Mathf.Lerp(startY, endY, k));
                yield return null;
            }

            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, endY);

            onSelected?.Invoke(info);
        }
    }
}
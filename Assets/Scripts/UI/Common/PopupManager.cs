using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public sealed class PopupManager : MonoBehaviour
    {
        [SerializeField] GameObject blocker;      // Image+Button, full-screen, inactive at start
        [SerializeField] float dimAlpha = 0.5f;   // 어둡기

        readonly Stack<GameObject> stack = new();

        void Awake()
        {
            if (blocker)
            {
                var img = blocker.GetComponent<Image>();
                if (img) img.color = new Color(0,0,0, dimAlpha);

                var btn = blocker.GetComponent<Button>();
                if (btn) btn.onClick.RemoveAllListeners(); // 배경 클릭 무시(닫지 않음)
                blocker.SetActive(false);
            }
        }

        public void Show(GameObject popup)
        {
            if (!popup) return;
            if (stack.Count == 0 && blocker) blocker.SetActive(true);

            popup.SetActive(true);
            stack.Push(popup);
        }

        public void Hide(GameObject popup)
        {
            if (!popup || stack.Count == 0) { popup?.SetActive(false); TryDisableBlocker(); return; }

            // 최상위만 닫도록 보장
            if (stack.Peek() == popup) stack.Pop();
            popup.SetActive(false);
            TryDisableBlocker();
        }

        public void HideAll()
        {
            while (stack.Count > 0) stack.Pop().SetActive(false);
            TryDisableBlocker();
        }

        void TryDisableBlocker()
        {
            if (stack.Count == 0 && blocker) blocker.SetActive(false);
        }
    }
}
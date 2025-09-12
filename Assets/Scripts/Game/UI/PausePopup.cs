using UnityEngine;
using UnityEngine.UI;

namespace GameLayer.UI
{
    public sealed class PausePopup : MonoBehaviour
    {
        [SerializeField] GameObject root;          // 최상위 GO (비활성 시작)
        [SerializeField] Button resumeButton;
        [SerializeField] CategoryListUI allCategories; // 전체 표시용

        bool paused;

        void Awake()
        {
            if (resumeButton) resumeButton.onClick.AddListener(Toggle);
            Hide();
        }

        public void Setup(Data.CategoryService svc) { allCategories.Init(svc, null); allCategories.ShowAll(); }

        public void Toggle()
        {
            if (paused) Hide(); else Show();
        }

        void Show()
        {
            paused = true;
            Time.timeScale = 0f;
            root.SetActive(true);
        }

        void Hide()
        {
            paused = false;
            Time.timeScale = 1f;
            root.SetActive(false);
        }
    }
}
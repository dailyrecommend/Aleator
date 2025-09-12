using SystemLayer;
using TMPro;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public sealed class StartPopupUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] PopupManager popupManager;   // Blocker 관리용

        [Header("Root")]
        [SerializeField] GameObject root;             // 스타트 팝업 최상위 GO

        [Header("Sub Views (한 번에 하나만 on)")]
        [SerializeField] GameObject dicePanel;
        [SerializeField] GameObject continuePanel;

        [Header("Tabs / Actions")]
        [SerializeField] Button tabDice;              // “New Run”
        [SerializeField] Button tabContinue;          // “Continue”
        [SerializeField] Button backButton;           // 루트 팝업 닫기

        [Header("Dice Panel")]
        [SerializeField] DiceSelectUI diceSelect;
        [SerializeField] Button startButton;          // 선택 확정 후 인게임

        //[Header("Continue Panel")] // Continue→Dice 전환
        // 여기에 세이브 슬롯 리스트 UI 바인딩(필요 시)

        void Awake()
        {
            // 탭/버튼 바인딩
            tabDice.onClick.AddListener(ShowDice);
            tabContinue.onClick.AddListener(ShowContinue);
            backButton.onClick.AddListener(CloseRoot);
            startButton.onClick.AddListener(StartRun);

            tabContinue.interactable = SaveService.HasSave();

            // ★ 여기서 비활성화하지 말 것. (씬에서 비활성로 배치)
            // if (root) root.SetActive(false);

            // ★ 서브뷰 바인딩 누락 시 바로 경고
            if (!dicePanel) Debug.LogWarning("[StartPopupUI] dicePanel not assigned");
            if (!continuePanel) Debug.LogWarning("[StartPopupUI] continuePanel not assigned");

            SetView(dice: true);
        }


        public void Open()
        {
            SetView(dice: true);
            var target = root ? root : gameObject;     // ★ fallback
            if (!popupManager) {                       // 팝업매니저 없으면 직접 활성 (디버그용)
                target.SetActive(true);
                return;
            }
            popupManager.Show(target);
        }

        void CloseRoot()
        {
            var target = root ? root : gameObject;
            if (!popupManager) { target.SetActive(false); return; }
            popupManager.Hide(target);
        }

        void ShowDice()    => SetView(dice: true);
        void ShowContinue()=> SetView(dice: false);

        void SetView(bool dice)
        {
            if (dicePanel)     dicePanel.SetActive(dice);
            if (continuePanel) continuePanel.SetActive(!dice);
        }

        void StartRun()
        {
            if (diceSelect) diceSelect.SetSelectedToParams();
            popupManager.Hide(root);
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIds.INGAME);
        }
    }
}
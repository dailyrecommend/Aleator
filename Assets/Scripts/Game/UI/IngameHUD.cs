using System;
using System.Linq;
using Data;
using GameLayer.Dice;
using GameLayer.Logic;
using GameLayer.Round;
using GameLayer.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLayer.UI
{
    public sealed class IngameHUD : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] CategoryService categoryService;
        public CategoryListUI categoryList;

        [Header("Dice Area")]
        [SerializeField] DiceSlot[] diceSlots;
        [SerializeField] TMP_Text rollCountLabel;
        [SerializeField] DiceGrid diceGrid;
        
        [Header("Left Preview")]
        [SerializeField] TMP_Text roundNameLabel;
        [SerializeField] TMP_Text requiredLabel;
        [SerializeField] TMP_Text collectedLabel;
        [SerializeField] TMP_Text chipsLabel;
        [SerializeField] TMP_Text multLabel;
        [SerializeField] TMP_Text roundScoreLabel;     // 상단 "Round Score"
        [SerializeField] TMP_Text previewLabel;        // 중간 미리보기(= chips*mult)
        [SerializeField] TMP_Text categoryNameLabel;
        [SerializeField] TMP_Text categoryLevelLabel;// 선택된 족보 이름
        [SerializeField] TMP_Text handsLeftLabel;
        
        
        [Header("Controls")]
        [SerializeField] Button rollButton;
        [SerializeField] Button confirmButton;

        [Header("Panels")]
        [SerializeField] GameObject dicePanel;
        [SerializeField] GameObject categoryPanel;
        [SerializeField] Button pauseButton;
        
        

        System.Random rng = new();
        GameState state = GameState.HandStart;
        int rollsLeft = 3;
        int handsLeft = 3;
        int roundScore = 0;
        int requiredScore = 0;
        int currChips, currMult;
        
        public System.Action onRoundWin;
        public System.Action onHandsExhausted;
        public System.Action onPauseToggled;
        
        CategoryRecord heldCategory; // 선택한 족보(홀드)

        void Awake()
        {
            if (!categoryService) categoryService = FindObjectOfType<CategoryService>();
            if (categoryService) categoryService.LoadFromStreamingAssets();

            categoryList.Init(categoryService, OnPickCategory);

            rollButton.onClick.AddListener(RollOnce);
            confirmButton.onClick.AddListener(OnConfirm);
            if (pauseButton) pauseButton.onClick.AddListener(() => onPauseToggled?.Invoke());
            confirmButton.interactable = false;

            ResetHandUI();
        }

        
        
        void ResetHandUI()
        {
            foreach (var d in diceSlots) d.Clear();
            heldCategory = null;
            rollsLeft = 3;
            state = GameState.HandStart;

            rollCountLabel.text = $"{rollsLeft}";
            chipsLabel.text = "0";
            multLabel.text = "0";
            previewLabel.text = "0";
            categoryNameLabel.text  = "category";
            categoryLevelLabel.text = "lv.0";
            
            categoryList.ClearAll();          // 핸드 리셋 시도 비우기
            confirmButton.interactable = false;
        }

        void RollOnce()
        {
            if (rollsLeft <= 0) return;
            diceGrid.RollAll();          // held는 유지되고 미보유만 굴림
            rollsLeft--;
            rollCountLabel.text = $"{rollsLeft}";
            state = GameState.Rolled;
            UpdatePreview();
            confirmButton.interactable = (heldCategory != null);
            var diceValues = diceGrid.Slots.Select(s => s.Value).ToList();
            categoryList.RefreshAvailable(diceValues); 
        }

        void OnPickCategory(CategoryRecord rec)
        {
            heldCategory = rec;

            categoryNameLabel.text  = rec.name;
            categoryLevelLabel.text = $"lv.{rec.level}";

            UpdatePreview();
            confirmButton.interactable = (state == GameState.Rolled);
        }

        void UpdatePreview()
        {
            if (heldCategory == null)
            {
                chipsLabel.text = "0";
                multLabel.text = "0";
                previewLabel.text = "0";
                return;
            }

            var sum = diceSlots.Sum(d => d.Value); // 또는 diceGrid.Slots.Sum(...)
            ScoreSystem.PreviewCategory(heldCategory, sum, out currChips, out currMult);

            chipsLabel.text   = $"{currChips}";
            multLabel.text    = $"{currMult}";
            previewLabel.text = $"{currChips * currMult}";
        }

        void OnConfirm()
        {
            if (heldCategory == null || state != GameState.Rolled) return;

            var gain = currChips * currMult;
            roundScore += gain;
            roundScoreLabel.text = $"{roundScore}";
            collectedLabel.text  = $"{roundScore}";

            categoryList.MarkUsed(heldCategory.name);

            handsLeft = Mathf.Max(0, handsLeft - 1);
            handsLeftLabel.text = $"{handsLeft}";

            // ★ 승리 판정
            if (roundScore >= requiredScore)
            {
                onRoundWin?.Invoke(); // GameDriver가 받음
                return;
            }
            if (handsLeft == 0)
            {
                onHandsExhausted?.Invoke();
                return;
            }

            // 다음 핸드 준비
            ResetHandUI();
        }
        
        public void HidePanels()
        {
            if (dicePanel) dicePanel.SetActive(false);
            if (categoryPanel) categoryPanel.SetActive(false);
        }
        
        public void BeginRound(GameLayer.Round.RoundInfo info)
        {
            ResetHandUI();

            roundNameLabel.text  = info.name;
            requiredLabel.text   = $"{info.requiredScore}";
            requiredScore        = info.requiredScore;

            roundScore           = 0;
            roundScoreLabel.text = "0";
            collectedLabel.text  = "0";

            handsLeft            = 3;
            rollsLeft            = 3;
            handsLeftLabel.text  = $"{handsLeft}";
            rollCountLabel.text  = $"{rollsLeft}";

            categoryNameLabel.text  = "category";
            categoryLevelLabel.text = "lv.0";
            previewLabel.text       = "0";

            if (dicePanel) dicePanel.SetActive(true);
            if (categoryPanel) categoryPanel.SetActive(true);
            
            categoryList.ClearAll();          // 라운드 시작 시 비우기
            confirmButton.interactable = false;
        }

        public void ResetHandOnly()
        {
            foreach (var d in diceSlots) d.Clear();
            heldCategory = null;
            rollsLeft = 3;
            rollCountLabel.text = $"{rollsLeft}";
            chipsLabel.text = "0";
            multLabel.text = "0";
            previewLabel.text = "0";
            categoryNameLabel.text = "category";
            categoryLevelLabel.text = "lv.0";
        }
        
        public void ResetRoundUI()
        {
            roundNameLabel.text = "Round";
            requiredLabel.text = "0";
            collectedLabel.text = "0";
            chipsLabel.text = "0";
            multLabel.text = "0";
        }
    }
}

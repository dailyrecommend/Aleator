using GameLayer.Round;
using GameLayer.UI;
using SystemLayer;
using UnityEngine;

namespace GameLayer
{
    public sealed class GameDriver : MonoBehaviour
    {
        [SerializeField] RoundSelectUI roundSelect;
        [SerializeField] IngameHUD hud;
        [SerializeField] PausePopup pausePopup;

        [SerializeField] int totalRounds = 3;

        int roundIndex;

        void Start()
        {
            hud.HidePanels();
            roundSelect.Init(OnRoundSelected);

            hud.onRoundWin        = OnRoundWin;
            hud.onHandsExhausted  = OnHandsExhausted;
            hud.onPauseToggled    = OnPause;

            if (pausePopup)
            {
                var svc = FindObjectOfType<Data.CategoryService>();
                if (svc) svc.LoadFromStreamingAssets();
                pausePopup.Setup(svc);
            }
        }

        void OnRoundSelected(RoundInfo info)
        {
            roundSelect.gameObject.SetActive(false);
            hud.BeginRound(info);
        }

        void OnRoundWin()
        {
            roundIndex++;
            if (roundIndex >= totalRounds)
            {
                Time.timeScale = 1f;
                SceneLoader.Load(SceneIds.VICTORY);
                return;
            }

            hud.ResetRoundUI();
            hud.HidePanels();
            hud.categoryList.ClearUsed();
            roundSelect.SetCurrentIndex(roundIndex);
            roundSelect.Show();
        }

        void OnHandsExhausted()
        {
            Time.timeScale = 1f;
            SceneLoader.Load(SceneIds.GAMEOVER);
        }

        void OnPause()
        {
            if (pausePopup) pausePopup.Toggle();
        }
    }
}
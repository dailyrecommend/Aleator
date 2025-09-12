using UI.Common;
using UnityEngine;
using UnityEngine.UI;
using SystemLayer;

namespace UI.Lobby
{
    public sealed class LobbyUI : MonoBehaviour
    {
        [SerializeField] Button playButton;
        [SerializeField] Button quitButton;

        [SerializeField] StartPopupUI startPopup;   // 위 스크립트

        void Awake()
        {
            playButton.onClick.AddListener(() => startPopup.Open());
            quitButton.onClick.AddListener(SceneLoader.Quit);
        }
    }
}
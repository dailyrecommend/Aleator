using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLayer.Round
{
    public sealed class RoundCard : MonoBehaviour
    {
        [SerializeField] TMP_Text titleLabel;
        [SerializeField] TMP_Text reqLabel;
        [SerializeField] Button playButton;

        public void Bind(RoundInfo info, bool interactable, System.Action onPlay)
        {
            titleLabel.text = info.name;
            reqLabel.text = $"{info.requiredScore}";
            playButton.interactable = interactable;
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => onPlay?.Invoke());
        }
    }
}
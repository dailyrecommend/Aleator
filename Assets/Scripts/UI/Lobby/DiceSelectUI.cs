// FILE: Assets/Scripts/UI/Lobby/DiceSelectUI.cs
using System.Linq;
using Data;
using SystemLayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public sealed class DiceSelectUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private DiceService diceService;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descLabel;

        private DiceRecord[] options = System.Array.Empty<DiceRecord>();
        private int index;

        private const string DEFAULT_ID = "default_die";
        private static readonly DiceRecord DEFAULT = new()
        {
            id = DEFAULT_ID,
            name = "Default",
            rarity = "Common",
            desc = "No special effect",
            effectId = "dice.none",
            @params = "{}"
        };

        private void Awake()
        {
            if (!diceService) diceService = FindAnyObjectByType<DiceService>();
            if (diceService) diceService.LoadFromStreamingAssets();

            options = (diceService && diceService.All.Count > 0)
                ? diceService.All.ToArray()
                : new[] { DEFAULT };

            leftButton.onClick.AddListener(Prev);
            rightButton.onClick.AddListener(Next);

            // 초기 선택: GameStartParams.diceId 있으면 매칭
            var currentId = GameStartParams.diceId;
            index = Mathf.Max(0, System.Array.FindIndex(options, r => r.id == currentId));

            Refresh();
        }

        public string GetSelectedId() => options.Length == 0 ? DEFAULT_ID : options[index].id;

        public void SetSelectedToParams()
        {
            GameStartParams.SetDice(GetSelectedId());
        }

        private void Prev()
        {
            if (options.Length == 0) return;
            index = (index + options.Length - 1) % options.Length;
            Refresh();
        }

        private void Next()
        {
            if (options.Length == 0) return;
            index = (index + 1) % options.Length;
            Refresh();
        }

        private void Refresh()
        {
            var r = options[index];
            if (nameLabel) nameLabel.text = r.name;
            if (descLabel) descLabel.text = r.desc;
        }
    }
}

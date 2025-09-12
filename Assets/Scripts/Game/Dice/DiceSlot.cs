using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameLayer.Dice
{
    public sealed class DiceSlot : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI")]
        [SerializeField] Image faceImage;
        [SerializeField] Sprite[] faceSprites;   // 1..sides

        [Header("Data")]
        [SerializeField] int sides = 6;

        int _value;
        bool _held;

        public int Value => _value;
        public bool Held => _held;

        public void SetSides(int newSides) { sides = Mathf.Max(2, newSides); }

        public void Clear()
        {
            _value = 0;
            SetHold(false);
            if (faceImage) faceImage.sprite = null;
        }

        public void Roll(System.Random rng)
        {
            if (_held) return;
            _value = rng.Next(1, sides + 1);
            if (faceImage && _value >= 1 && _value <= faceSprites.Length)
                faceImage.sprite = faceSprites[_value - 1];
        }

        public void SetHold(bool on)
        {
            _held = on;
            // 필요 시 홀드 마크 토글 추가
        }

        public void ToggleHold() { SetHold(!_held); }

        public void OnPointerClick(PointerEventData e) { ToggleHold(); }
    }
}
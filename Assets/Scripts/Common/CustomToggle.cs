using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Scripts/General/General.CustomToggle")]
    internal class CustomToggle : MonoBehaviour
    {
        [SerializeField]
        private RectTransform handle;

        [SerializeField]
        private Vector2 leftPosition;

        [SerializeField]
        private Vector2 rightPosition;

        [SerializeField]
        private float switchTime;

        [SerializeField]
        private bool colorSwitch;

        [SerializeField]
        private Graphic targetGraphic;

        [SerializeField]
        private Color activeColor;

        [SerializeField]
        private Color disableColor;

        [SerializeField]
        private bool spriteSwitch;

        [SerializeField]
        private Image targetImage;

        [SerializeField]
        private Sprite activeSprite;

        [SerializeField]
        private Sprite disableSprite;

        private Button control;
        private bool value;
        private bool inAction = false;

        public delegate void Switched(bool value);
        public event Switched OnValueChanged;

        private void Awake()
        {
            control = GetComponent<Button>();
            control.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (!inAction)
            {
                SetValue(!value);
            }
        }

        public void SetValue(bool value, bool immediate = false)
        {
            this.value = value;
            OnValueChanged?.Invoke(value);
            Color backgroundColor = value ? activeColor : disableColor;
            Sprite spriteToSwitch = value ? activeSprite : disableSprite;
            Vector2 handlePosition = value ? rightPosition : leftPosition;

            if (immediate)
            {
                if (colorSwitch)
                {
                    targetGraphic.color = backgroundColor;
                }
                if (spriteSwitch)
                {
                    targetImage.sprite = spriteToSwitch;
                }
                handle.localPosition = handlePosition;
                return;
            }

            inAction = true;
            if (colorSwitch)
            {
                LerpBackground(backgroundColor);
            }
            if (spriteSwitch)
            {
                targetImage.sprite = spriteToSwitch;
            }
            MoveHandle(handlePosition);
            _ = DOVirtual.DelayedCall(switchTime, () => inAction = false, false);
        }

        private void LerpBackground(Color color)
        {
            _ = targetGraphic.DOColor(color, switchTime);
        }

        private void MoveHandle(Vector2 position)
        {
            _ = handle.DOLocalMove(position, switchTime);
        }
    }
}

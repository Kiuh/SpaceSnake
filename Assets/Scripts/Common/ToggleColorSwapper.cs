using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    [RequireComponent(typeof(Toggle))]
    [AddComponentMenu("Scripts/Common/Common.ToggleColorSwapper")]
    internal class ToggleColorSwapper : MonoBehaviour
    {
        [SerializeField]
        private Graphic graphic;

        [SerializeField]
        private Color onColor;

        [SerializeField]
        private Color offColor;

        [SerializeField]
        private bool initialCheck = true;

        private Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleSwitched);
        }

        private void Start()
        {
            if (initialCheck)
            {
                SetColor(toggle.isOn);
            }
        }

        private void OnToggleSwitched(bool isOn)
        {
            SetColor(isOn);
        }

        private void SetColor(bool isOn)
        {
            graphic.color = isOn ? onColor : offColor;
        }
    }
}

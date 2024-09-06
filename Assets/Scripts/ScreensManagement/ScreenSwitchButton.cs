using System;
using System.Collections.Generic;
using TNRD;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScreensManagement
{
    [Serializable]
    internal class ScreenSwitchButton
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private List<SerializableInterface<IScreen>> nextScreens;

        public UnityEvent OnPreButtonClick;
        public event Action OnPreButtonClickAction;
        private Func<bool> condition;

        public void Initialize()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        public void SetCondition(Func<bool> condition)
        {
            this.condition = condition;
        }

        private void OnButtonClick()
        {
            if (condition != null)
            {
                if (!condition())
                {
                    return;
                }
            }

            OnPreButtonClick?.Invoke();
            OnPreButtonClickAction?.Invoke();
            Manager.SwitchScreens(nextScreens);
        }
    }
}

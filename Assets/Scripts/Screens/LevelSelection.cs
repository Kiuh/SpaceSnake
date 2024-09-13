using System.Collections.Generic;
using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.LevelSelection")]
    internal class LevelSelection : CanvasGroupScreen
    {
        [SerializeField]
        private ScrollRect scrollView;

        [SerializeField]
        private List<Button> buttons;

        [SerializeField]
        private TransitionCall call;

        public override void AwakeInitialization()
        {
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => call.DoTransition());
            }
        }

        public override void StartInitialization()
        {
            scrollView.normalizedPosition = Vector3.one;
        }
    }
}

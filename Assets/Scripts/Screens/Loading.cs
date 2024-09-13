using DG.Tweening;
using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.Loading")]
    internal class Loading : CanvasGroupScreen
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private Image handle;

        [SerializeField]
        private TransitionCall nextScreen;

        private void Awake()
        {
            OnShowEnds += Loading_OnShowEnds;
        }

        private void Loading_OnShowEnds()
        {
            _ = DOVirtual
                .Float(
                    0,
                    1,
                    duration,
                    (x) =>
                    {
                        handle.fillAmount = x;
                    }
                )
                .OnComplete(nextScreen.DoTransition);
        }
    }
}

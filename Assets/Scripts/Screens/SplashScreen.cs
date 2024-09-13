using DG.Tweening;
using ScreensManagement;
using UnityEngine;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.SplashScreen")]
    internal class SplashScreen : CanvasGroupScreen
    {
        [SerializeField]
        private TransitionCall call;

        private void Start()
        {
            _ = DOVirtual.DelayedCall(1, call.DoTransition);
        }
    }
}

using DG.Tweening;
using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.Spin")]
    internal class Spin : CanvasGroupScreen
    {
        [SerializeField]
        private Button spin;

        [SerializeField]
        private TransitionCall call;

        [SerializeField]
        private RectTransform handle;

        private bool spinning = false;

        public override void AwakeInitialization()
        {
            spin.onClick.AddListener(SpinW);
        }

        private void SpinW()
        {
            if (spinning)
            {
                return;
            }

            spinning = true;
            Sequence sequence = DOTween.Sequence();

            Tween spinTween = handle
                .DOLocalRotate(Vector3.forward * (360 * 15), 6, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuad);

            _ = sequence.Append(spinTween);
            _ = sequence.AppendCallback(() =>
            {
                spinning = false;
                call.DoTransition();
            });
        }
    }
}

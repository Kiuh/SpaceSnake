using DG.Tweening;
using General;
using ScreensManagement;
using TMPro;
using UnityEngine;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.CoinsUpdater")]
    internal class CoinsUpdater : CanvasGroupScreen
    {
        [SerializeField]
        private TextMeshProUGUI coinsLabel;

        [SerializeField]
        private float lerpTime;
        private int currentCoins;

        private Tween lerpTween;

        public override void AwakeInitialization()
        {
            GameConfig.DynamicData.OnMutableDataChanged += MutableData_OnMutableDataChanged;
            MutableData_OnMutableDataChanged();
        }

        private void MutableData_OnMutableDataChanged()
        {
            lerpTween?.Kill();
            lerpTween = DOVirtual.Int(
                currentCoins,
                GameConfig.DynamicData.Coins,
                lerpTime,
                (x) =>
                {
                    currentCoins = x;
                    coinsLabel.text = "Coins: " + currentCoins.ToString();
                }
            );
        }
    }
}

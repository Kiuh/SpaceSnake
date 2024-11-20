using System.Collections.Generic;
using DG.Tweening;
using General;
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

        [SerializeField]
        private int spinCost;

        [SerializeField]
        private SpinWin spinWin;

        [SerializeField]
        private List<string> rewards;

        private float OneSectionRotation => 360f / rewards.Count;

        public override void AwakeInitialization()
        {
            spin.onClick.AddListener(SpinW);
        }

        private void SpinW()
        {
            if (GameConfig.DynamicData.Coins >= spinCost)
            {
                GameConfig.DynamicData.Coins -= spinCost;
            }
            else
            {
                return;
            }

            UILocker.Lock();

            Sequence sequence = DOTween.Sequence();

            int index = Random.Range(0, rewards.Count);
            float endRotation = (OneSectionRotation / 2) + (OneSectionRotation * index);

            Tween spinTween = handle
                .DOLocalRotate(
                    Vector3.forward * ((360 * 15) + endRotation),
                    6,
                    RotateMode.LocalAxisAdd
                )
                .SetEase(Ease.InOutQuad);

            Tween spinBackTween = handle
                .DOLocalRotate(Vector3.zero, 0.7f, RotateMode.Fast)
                .SetEase(Ease.Linear);

            _ = sequence.Append(spinTween);
            _ = sequence.AppendCallback(() =>
            {
                GiveReward(rewards[index]);
                spinWin.SetRewardImage(rewards[index]);
                call.DoTransition();
            });
            _ = sequence.Append(spinBackTween);
        }

        private void GiveReward(string rew)
        {
            if (int.TryParse(rew, out int amount))
            {
                GameConfig.DynamicData.Coins += amount;
                return;
            }

            if (rew == "Thunder")
            {
                GameConfig.DynamicData.Thunders++;
                return;
            }
            if (rew == "Battery")
            {
                GameConfig.DynamicData.Batteries++;
                return;
            }
            if (rew == "Rocket")
            {
                GameConfig.DynamicData.Rockets++;
                return;
            }

            Debug.Log("No reward given");
        }
    }
}

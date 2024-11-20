using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.SpinWin")]
    internal class SpinWin : MultipleButtonScreen
    {
        [SerializeField]
        private Sprite coins;

        [SerializeField]
        private Sprite thunder;

        [SerializeField]
        private Sprite battery;

        [SerializeField]
        private Sprite rocket;

        [SerializeField]
        private Image image;

        public void SetRewardImage(string rew)
        {
            if (int.TryParse(rew, out _))
            {
                image.sprite = coins;
                return;
            }

            if (rew == "Thunder")
            {
                image.sprite = thunder;
                return;
            }
            if (rew == "Battery")
            {
                image.sprite = battery;
                return;
            }
            if (rew == "Rocket")
            {
                image.sprite = rocket;
                return;
            }
        }
    }
}

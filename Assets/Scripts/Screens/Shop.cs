using System.Collections.Generic;
using General;
using ScreensManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.Shop")]
    internal class Shop : CanvasGroupScreen
    {
        [SerializeField]
        private Button right;

        [SerializeField]
        private Button left;

        [SerializeField]
        private List<GameObject> icons;

        [SerializeField]
        private Button buy;

        [SerializeField]
        private TextMeshProUGUI costLebel;

        [SerializeField]
        private List<int> cost;

        private int current = 0;

        public override void AwakeInitialization()
        {
            right.onClick.AddListener(() => Set(current + 1));
            left.onClick.AddListener(() => Set(current - 1));
            buy.onClick.AddListener(Buy);
        }

        public override void StartInitialization()
        {
            Set(current);
        }

        private void Buy()
        {
            if (GameConfig.DynamicData.Coins >= cost[current])
            {
                GameConfig.DynamicData.Coins -= cost[current];
                if (current == 0)
                {
                    GameConfig.DynamicData.Thunders++;
                }
                if (current == 1)
                {
                    GameConfig.DynamicData.Batteries++;
                }
                if (current == 2)
                {
                    GameConfig.DynamicData.Rockets++;
                }
            }
        }

        public void Set(int value)
        {
            current =
                value < 0
                    ? icons.Count - 1
                    : value >= icons.Count
                        ? 0
                        : value;

            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].SetActive(current == i);
            }

            costLebel.text = cost[current].ToString();
        }
    }
}

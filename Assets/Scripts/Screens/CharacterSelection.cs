using System.Collections.Generic;
using General;
using ScreensManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.CharacterSelection")]
    internal class CharacterSelection : CanvasGroupScreen
    {
        [SerializeField]
        private ScreenSwitchButton button;

        [SerializeField]
        private Button right;

        [SerializeField]
        private Button left;

        [SerializeField]
        private List<GameObject> snakes;

        [SerializeField]
        private Button buy;

        [SerializeField]
        private GameObject lockerObject;

        [SerializeField]
        private TextMeshProUGUI costLebel;
        private GameObject costLabelGameObject => costLebel.transform.parent.gameObject;

        [SerializeField]
        private List<int> cost;

        private int current = 0;

        public override void AwakeInitialization()
        {
            right.onClick.AddListener(() => Set(current + 1));
            left.onClick.AddListener(() => Set(current - 1));
            buy.onClick.AddListener(Buy);
            button.Initialize();
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
                GameConfig.DynamicData.unlockedSkins[current] = true;
                GameConfig.DynamicData.SaveData();
                Set(current);
            }
        }

        public void Set(int value)
        {
            current =
                value < 0
                    ? snakes.Count - 1
                    : value >= snakes.Count
                        ? 0
                        : value;

            GameConfig.StaticData.currentSnakeSkin = current;

            for (int i = 0; i < snakes.Count; i++)
            {
                snakes[i].SetActive(false);
            }
            lockerObject.SetActive(false);
            buy.gameObject.SetActive(false);
            button.Button.gameObject.SetActive(false);
            costLabelGameObject.SetActive(false);

            snakes[current].SetActive(true);
            if (GameConfig.DynamicData.unlockedSkins[current])
            {
                button.Button.gameObject.SetActive(true);
            }
            else
            {
                costLabelGameObject.SetActive(true);
                lockerObject.SetActive(true);
                buy.gameObject.SetActive(true);
                costLebel.text = cost[current].ToString();
            }
        }
    }
}

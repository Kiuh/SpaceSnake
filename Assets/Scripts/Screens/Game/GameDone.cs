using System.Collections.Generic;
using General;
using ScreensManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.GameDone")]
    internal class GameDone : MultipleButtonScreen
    {
        [SerializeField]
        private List<Image> stars;

        [SerializeField]
        private TextMeshProUGUI coinsReward;

        public void SetGameDone(int collected, LevelConfig config)
        {
            stars[0].gameObject.SetActive(false);
            stars[1].gameObject.SetActive(false);
            stars[2].gameObject.SetActive(false);

            int star = 1;
            stars[0].gameObject.SetActive(true);

            if (config.starsRequirements.ForTwoStars <= collected)
            {
                stars[1].gameObject.SetActive(true);
                star++;
            }

            if (config.starsRequirements.ForThreeStars <= collected)
            {
                stars[2].gameObject.SetActive(true);
                star++;
            }

            if (GameConfig.DynamicData.levelStars[GameConfig.StaticData.currentLevelIndex] < star)
            {
                GameConfig.DynamicData.levelStars[GameConfig.StaticData.currentLevelIndex] = star;
            }

            GameConfig.DynamicData.Coins += config.coinsReward;
            GameConfig.DynamicData.SaveData();

            coinsReward.text = config.coinsReward.ToString();
        }
    }
}

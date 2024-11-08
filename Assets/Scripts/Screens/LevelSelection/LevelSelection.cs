using System.Collections.Generic;
using General;
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
        private List<PlanetButton> buttons;

        [SerializeField]
        private TransitionCall call;

        public override void AwakeInitialization()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                int buff = i;
                buttons[i].Button.onClick.AddListener(() => TryClick(buff));
            }

            OnShowStarts += LevelSelection_OnShowStarts;
        }

        private void TryClick(int levelInd)
        {
            if (levelInd == 0 || GameConfig.DynamicData.levelStars[levelInd - 1] > 0)
            {
                GameConfig.StaticData.currentLevelIndex = levelInd;
                call.DoTransition();
            }
        }

        private void LevelSelection_OnShowStarts()
        {
            scrollView.normalizedPosition = Vector3.zero;
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetStars(GameConfig.DynamicData.levelStars[i]);
            }
        }
    }
}

using System;
using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [Serializable]
    public class SmartSlider
    {
        public Button plus;
        public Button minus;
        public Image fill;
        public int sections;
        public int current;

        public void Init()
        {
            plus.onClick.AddListener(() => Set(current + 1));
            minus.onClick.AddListener(() => Set(current - 1));
            Set(current);
        }

        private void Set(int value)
        {
            current = Mathf.Clamp(value, 0, sections);
            fill.fillAmount = (float)current / sections;
        }
    }

    [AddComponentMenu("Scripts/Screens/Screens.Settings")]
    internal class Settings : MultipleButtonScreen
    {
        [SerializeField]
        private SmartSlider musicSlider;

        [SerializeField]
        private SmartSlider soundsSlider;

        public override void AwakeInitialization()
        {
            musicSlider.Init();
            soundsSlider.Init();
        }
    }
}

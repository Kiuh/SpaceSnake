using System;
using General;
using ScreensManagement;
using UnityEngine;
using UnityEngine.Audio;
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

        public event Action<float> OnValueChanged;

        public void Init()
        {
            plus.onClick.AddListener(() => Set(current + 1));
            minus.onClick.AddListener(() => Set(current - 1));
            Set(current);
        }

        public void SetValue(float value)
        {
            Set((int)(sections * value));
        }

        private void Set(int value)
        {
            current = Mathf.Clamp(value, 0, sections);
            float newValue = current / (float)sections;
            fill.fillAmount = newValue;
            OnValueChanged?.Invoke(newValue);
        }
    }

    [AddComponentMenu("Scripts/Screens/Screens.Settings")]
    internal class Settings : MultipleButtonScreen
    {
        [SerializeField]
        private SmartSlider musicSlider;

        [SerializeField]
        private SmartSlider soundsSlider;

        [SerializeField]
        private AudioMixer mixer;

        public override void AwakeInitialization()
        {
            musicSlider.Init();
            musicSlider.OnValueChanged += MusicSlider_OnValueChanged;
            MusicSlider_OnValueChanged(GameConfig.DynamicData.Music);

            soundsSlider.Init();
            soundsSlider.OnValueChanged += SoundsSlider_OnValueChanged;
            SoundsSlider_OnValueChanged(GameConfig.DynamicData.Sound);
        }

        public override void StartInitialization()
        {
            musicSlider.SetValue(GameConfig.DynamicData.Music);
            soundsSlider.SetValue(GameConfig.DynamicData.Sound);
        }

        private void SoundsSlider_OnValueChanged(float val)
        {
            _ = mixer.SetFloat("Sounds", GetValue(val));
            GameConfig.DynamicData.Sound = val;
        }

        private void MusicSlider_OnValueChanged(float val)
        {
            _ = mixer.SetFloat("Music", GetValue(val));
            GameConfig.DynamicData.Music = val;
        }

        private float GetValue(float value)
        {
            return Mathf.Lerp(-80, 20, value);
        }
    }
}

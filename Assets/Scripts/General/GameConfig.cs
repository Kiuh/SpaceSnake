using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace General
{
    [Serializable]
    public class SavableData
    {
        private int coins = 500;
        private int rockets;
        private int batteries;
        private int thunders;

        private float sound;
        private float music;

        public int currentSkin = 0;
        public List<int> levelStars = new() { 0, 0, 0, 0, 0, 0 };
        public List<bool> unlockedSkins = new() { true, false, false, false };

        public int Rockets
        {
            get => rockets;
            set
            {
                rockets = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public int Batteries
        {
            get => batteries;
            set
            {
                batteries = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public int Thunders
        {
            get => thunders;
            set
            {
                thunders = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public int Coins
        {
            get => coins;
            set
            {
                coins = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public float Sound
        {
            get => sound;
            set
            {
                sound = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public float Music
        {
            get => music;
            set
            {
                music = value;
                OnMutableDataChanged?.Invoke();
            }
        }

        public event Action OnMutableDataChanged;

        public void SaveData()
        {
            OnMutableDataChanged?.Invoke();
        }
    }

    [Serializable]
    public class StarsRequirements
    {
        public int ForOneStar;
        public int ForTwoStars;
        public int ForThreeStars;
    }

    [Serializable]
    public class LevelConfig
    {
        public int levelSeconds;
        public int coinsReward;
        public StarsRequirements starsRequirements;
    }

    [Serializable]
    public class StaticData
    {
        public int currentLevelIndex;
        public int currentSnakeSkin;
        public List<LevelConfig> levels;
        public LevelConfig CurrentLevel => levels[currentLevelIndex];
    }

    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    internal class GameConfig : ScriptableObject
    {
        private static GameConfig InternalInstance;
        private static GameConfig Instance
        {
            get
            {
                if (InternalInstance == null)
                {
                    InternalInstance = Resources.Load<GameConfig>("GameConfig");
                    InternalInstance.Initialize();
                }
                return InternalInstance;
            }
        }
        public static SavableData DynamicData => Instance.internalMutableData;
        public static StaticData StaticData => Instance.internalStaticData;

        [SerializeField]
        private StaticData internalStaticData;
        private SavableData internalMutableData;

        private static readonly string GAME_DATA_LABEL = "GAME_DATA";

        private void Initialize()
        {
            LoadData();
            internalMutableData.OnMutableDataChanged += SaveData;
            //internalMutableData.OnMutableDataChanged += () =>
            //    Debug.Log(PlayerPrefs.GetString(GAME_DATA_LABEL, string.Empty));
        }

        private void LoadData()
        {
            string rawData = PlayerPrefs.GetString(GAME_DATA_LABEL, string.Empty);
            internalMutableData = JsonConvert.DeserializeObject<SavableData>(rawData);
            internalMutableData ??= new();
        }

        private void SaveData()
        {
            string rawData = JsonConvert.SerializeObject(internalMutableData);
            PlayerPrefs.SetString(GAME_DATA_LABEL, rawData);
        }
    }
}

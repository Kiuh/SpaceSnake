using System;
using Newtonsoft.Json;
using UnityEngine;

namespace General
{
    [Serializable]
    public class SavableData
    {
        private int rockets;
        private int batteries;
        private int thunders;

        public int Rockets
        {
            get => rockets;
            set
            {
                if (value < 0)
                {
                    return;
                }

                rockets = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public int Batteries
        {
            get => batteries;
            set
            {
                if (value < 0)
                {
                    return;
                }

                batteries = value;
                OnMutableDataChanged?.Invoke();
            }
        }
        public int Thunders
        {
            get => thunders;
            set
            {
                if (value < 0)
                {
                    return;
                }

                thunders = value;
                OnMutableDataChanged?.Invoke();
            }
        }

        public event Action OnMutableDataChanged;
    }

    [Serializable]
    public class StaticData { }

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

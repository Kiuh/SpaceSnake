using System;
using General;
using ScreensManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Game
{
    [Serializable]
    public class Consumable
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI counter;

        private Func<int> getAmount;
        public event Action Used;

        public void Init(Func<int> getAmount)
        {
            this.getAmount = getAmount;
            button.onClick.AddListener(() =>
            {
                if (getAmount() < 1)
                {
                    return;
                }
                Used?.Invoke();
                UpdateView();
            });
        }

        public void UpdateView()
        {
            counter.text = getAmount().ToString();
        }
    }

    [Serializable]
    public class Bar
    {
        public Image fill;

        public void Set(float value)
        {
            fill.fillAmount = value;
        }
    }

    public class Timer
    {
        public float Duration;
        public event Action OnEnd;
        public float currentTime;

        public bool IsTicking { get; private set; } = false;

        public void Start()
        {
            currentTime = 0;
            IsTicking = true;
        }

        public void Stop()
        {
            IsTicking = false;
        }

        public void AddTime(float time)
        {
            currentTime -= time;
        }

        public void Update(float deltaTime)
        {
            if (!IsTicking)
            {
                return;
            }

            currentTime += deltaTime;
            if (Duration <= currentTime)
            {
                OnEnd?.Invoke();
                Stop();
            }
        }

        public int GetSeconds()
        {
            return (int)(Duration - currentTime);
        }
    }

    [AddComponentMenu("Scripts/Screens/Game/Screens.Game")]
    internal class GameImpl : CanvasGroupScreen
    {
        [SerializeField]
        private Consumable rocket;

        [SerializeField]
        private Consumable thunder;

        [SerializeField]
        private Consumable batteries;

        [SerializeField]
        private Bar planets;

        [SerializeField]
        private TransitionCall win;

        [SerializeField]
        private TransitionCall lose;

        [SerializeField]
        private SnakeMover snakeMover;

        [SerializeField]
        private GameDone gameDone;

        [SerializeField]
        private TextMeshProUGUI timerLabel;

        [SerializeField]
        private TextMeshProUGUI levelNumber;

        [SerializeField]
        private ObstacleGenerator obstacleGenerator;

        [SerializeField]
        private PickupGenerator pickupGenerator;

        [SerializeField]
        private int batteryTimeAdd;

        [SerializeField]
        private int planetPointsAmount;
        public RectTransform LablesParent;
        public int PlanetPointsAmount => planetPointsAmount;

        private int planetCollected;
        private int shields;
        private Timer timer = new();

        public override void AwakeInitialization()
        {
            rocket.Init(() => GameConfig.DynamicData.Rockets);
            rocket.Used += () => GameConfig.DynamicData.Rockets--;
            rocket.Used += () => shields++;
            thunder.Init(() => GameConfig.DynamicData.Thunders);
            thunder.Used += UseThunder;
            batteries.Init(() => GameConfig.DynamicData.Batteries);
            batteries.Used += UseBattery;

            snakeMover.OnPlanetCollected += SnakeMover_OnPlanetCollected;
            snakeMover.OnObstacleHinted += SnakeMover_OnObstacleHinted;

            timer.OnEnd += FinishGame;
        }

        private void UseThunder()
        {
            GameConfig.DynamicData.Thunders--;
            snakeMover.AddSpeedUp();
        }

        private void UseBattery()
        {
            GameConfig.DynamicData.Batteries--;
            timer.AddTime(batteryTimeAdd);
        }

        private void SnakeMover_OnObstacleHinted()
        {
            if (shields > 0)
            {
                obstacleGenerator.SkipCurrentObstacle();
                shields--;
            }
            else
            {
                KillGame();
                lose.DoTransition();
            }
        }

        private void SnakeMover_OnPlanetCollected()
        {
            planetCollected += planetPointsAmount;
        }

        public void StartGame()
        {
            snakeMover.ResetSnake();
            snakeMover.EnableMovement();
            obstacleGenerator.StartGenerate();
            pickupGenerator.StartGenerate();
            planetCollected = 0;
            shields = 0;
            levelNumber.text = "Level " + (GameConfig.StaticData.currentLevelIndex + 1).ToString();

            int duration = GameConfig.StaticData.CurrentLevel.levelSeconds;
            timer.Duration = duration;
            timer.Start();
        }

        private void Update()
        {
            planets.Set(
                planetCollected
                    / (float)GameConfig.StaticData.CurrentLevel.starsRequirements.ForThreeStars
            );
            UpdateTimer(timer.GetSeconds());
            timer.Update(Time.deltaTime);
            rocket.UpdateView();
            thunder.UpdateView();
            batteries.UpdateView();
        }

        public void UpdateTimer(int time)
        {
            timerLabel.text = "Time left: " + time.ToString() + "s";
        }

        public void FinishGame()
        {
            KillGame();

            if (planetCollected < GameConfig.StaticData.CurrentLevel.starsRequirements.ForOneStar)
            {
                lose.DoTransition();
            }
            else
            {
                gameDone.SetGameDone(planetCollected, GameConfig.StaticData.CurrentLevel);
                win.DoTransition();
            }
        }

        public void KillGame()
        {
            obstacleGenerator.StopGenerate();
            pickupGenerator.StopGenerate();
            timer.Stop();
            snakeMover.DisableMovement();
        }
    }
}

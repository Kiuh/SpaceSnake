using System;
using DG.Tweening;
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
        private Bar money;

        [SerializeField]
        private TransitionCall win;

        [SerializeField]
        private TransitionCall lose;

        [SerializeField]
        private SnakeMover snakeMover;

        public override void AwakeInitialization()
        {
            rocket.Init(() => GameConfig.DynamicData.Rockets);
            rocket.Used += () => GameConfig.DynamicData.Rockets--;
            thunder.Init(() => GameConfig.DynamicData.Thunders);
            thunder.Used += () => GameConfig.DynamicData.Thunders--;
            batteries.Init(() => GameConfig.DynamicData.Batteries);
            batteries.Used += () => GameConfig.DynamicData.Batteries--;
        }

        private Tween plLerp;
        private Tween monLerp;

        public void StartGame()
        {
            snakeMover.StartMove();

            float plDur = UnityEngine.Random.Range(10f, 20f);
            plLerp = DOVirtual.Float(0, 1, plDur, planets.Set).OnComplete(() => FinishGame(true));

            float monDur = UnityEngine.Random.Range(10f, 20f);
            monLerp = DOVirtual.Float(0, 1, monDur, money.Set).OnComplete(() => FinishGame(false));
        }

        public void FinishGame(bool isWin)
        {
            KillGame();

            if (isWin)
            {
                win.DoTransition();
            }
            else
            {
                lose.DoTransition();
            }
        }

        public void KillGame()
        {
            plLerp?.Kill();
            monLerp?.Kill();
            snakeMover.StopMove();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TNRD;
using UnityEngine;

namespace ScreensManagement
{
    [AddComponentMenu("Scripts/ScreensManagement/ScreensManagement.Manager")]
    internal class Manager : MonoBehaviour
    {
        [SerializeField]
        private float transitionDuration;
        public float TransitionDuration => transitionDuration;

        public static Manager Instance;

        private List<IScreen> screens = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            Application.targetFrameRate = 60;
            MonoBehaviour[] behaviors = FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );
            screens = behaviors.Where(x => x is IScreen).Select(x => x as IScreen).ToList();
            screens.ForEach(x => x.AwakeInitialization());
        }

        private void Start()
        {
            screens.ForEach(x => x.StartInitialization());
        }

        public static void SwitchScreens(SerializableInterface<IScreen> inScreen)
        {
            Instance.SwitchScreensInternal(new List<IScreen>() { inScreen.Value });
        }

        public static void SwitchScreens(IEnumerable<SerializableInterface<IScreen>> inScreens)
        {
            Instance.SwitchScreensInternal(inScreens.Select(x => x.Value));
        }

        private void SwitchScreensInternal(IEnumerable<IScreen> inScreens)
        {
            UILocker.Lock();

            List<IScreen> inScreensStack = inScreens.ToList();
            List<IScreen> inScreensHeap = new();

            while (inScreensStack.Count > 0)
            {
                IScreen current = inScreensStack.First();
                inScreensHeap.Add(current);
                inScreensStack.AddRange(
                    current.LinkedScreens.Where(x =>
                        !inScreensStack.Contains(x) && !inScreensHeap.Contains(x)
                    )
                );
                inScreensStack.RemoveAt(0);
            }

            List<IScreen> outScreens = screens.Except(inScreensHeap).ToList();

            inScreensHeap.ToList().ForEach(x => x.Show().Play());
            outScreens.ForEach(x => x.Hide().Play());

            _ = DOVirtual.DelayedCall(Manager.Instance.TransitionDuration, UILocker.UnLock);
        }
    }
}

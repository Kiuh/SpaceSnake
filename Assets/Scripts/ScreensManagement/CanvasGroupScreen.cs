using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TNRD;
using UnityEngine;

namespace ScreensManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("Scripts/ScreensManagement/ScreensManagement.CanvasGroupScreen")]
    internal class CanvasGroupScreen : MonoBehaviour, IScreen
    {
        public virtual void AwakeInitialization() { }

        public virtual void StartInitialization() { }

        private CanvasGroup canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null)
                {
                    canvasGroup = GetComponent<CanvasGroup>();
                }
                return canvasGroup;
            }
        }

        [SerializeField]
        private List<SerializableInterface<IScreen>> linkedScreens;
        public IEnumerable<IScreen> LinkedScreens => linkedScreens.Select(x => x.Value);

        public event Action OnHideStarts;
        public event Action OnHideEnds;
        public event Action OnShowStarts;
        public event Action OnShowEnds;

        public Sequence Hide()
        {
            if (CanvasGroup.gameObject.activeSelf == false)
            {
                return DOTween.Sequence();
            }

            Sequence sequence = DOTween.Sequence();
            sequence.onPlay += () => OnHideStarts?.Invoke();
            _ = sequence.Append(CanvasGroup.DOFade(0, Manager.Instance.TransitionDuration));
            sequence.onComplete += () => OnHideEnds?.Invoke();
            sequence.onComplete += () => CanvasGroup.gameObject.SetActive(false);
            return sequence;
        }

        public Sequence Show()
        {
            Sequence sequence = DOTween.Sequence();

            if (CanvasGroup.gameObject.activeSelf == true)
            {
                sequence.onPlay += () => OnShowStarts?.Invoke();
                _ = sequence.AppendInterval(Manager.Instance.TransitionDuration);
                sequence.onComplete += () => OnShowEnds?.Invoke();
                return sequence;
            }

            sequence.onPlay += () => CanvasGroup.gameObject.SetActive(true);
            sequence.onPlay += () => CanvasGroup.DOFade(0, 0).Play().Complete();
            sequence.onPlay += () => OnShowStarts?.Invoke();
            _ = sequence.Append(CanvasGroup.DOFade(1, Manager.Instance.TransitionDuration));
            sequence.onComplete += () => OnShowEnds?.Invoke();
            return sequence;
        }
    }
}

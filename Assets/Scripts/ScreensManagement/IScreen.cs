using System;
using System.Collections.Generic;
using DG.Tweening;

namespace ScreensManagement
{
    internal interface IScreen
    {
        public virtual void AwakeInitialization() { }
        public virtual void StartInitialization() { }
        public IEnumerable<IScreen> LinkedScreens { get; }

        public event Action OnHideStarts;
        public event Action OnHideEnds;

        public event Action OnShowStarts;
        public event Action OnShowEnds;

        public Sequence Hide();
        public Sequence Show();
    }
}

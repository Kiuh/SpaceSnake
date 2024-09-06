using System;
using System.Collections.Generic;
using TNRD;
using UnityEngine;

namespace ScreensManagement
{
    [Serializable]
    internal class TransitionCall
    {
        [SerializeField]
        private List<SerializableInterface<IScreen>> screens;

        public void DoTransition()
        {
            Manager.SwitchScreens(screens);
        }
    }
}

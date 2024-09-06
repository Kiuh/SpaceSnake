using System.Collections.Generic;
using UnityEngine;

namespace ScreensManagement
{
    [AddComponentMenu("Scripts/ScreensManagement/ScreensManagement.MultipleButtonScreen")]
    internal class MultipleButtonScreen : CanvasGroupScreen
    {
        [SerializeField]
        private List<ScreenSwitchButton> switchButtons;

        protected void Awake()
        {
            switchButtons.ForEach(x => x.Initialize());
        }
    }
}

using General;
using ScreensManagement;
using UnityEngine;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.SpinWin")]
    internal class SpinWin : MultipleButtonScreen
    {
        public override void AwakeInitialization()
        {
            OnHideEnds += SpinWin_OnHideEnds;
        }

        private void SpinWin_OnHideEnds()
        {
            GameConfig.DynamicData.Rockets++;
        }
    }
}

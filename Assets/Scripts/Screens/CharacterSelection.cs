using System.Collections.Generic;
using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.CharacterSelection")]
    internal class CharacterSelection : CanvasGroupScreen
    {
        [SerializeField]
        private ScreenSwitchButton button;

        [SerializeField]
        private Button right;

        [SerializeField]
        private Button left;

        [SerializeField]
        private List<GameObject> go;

        private int current = 0;

        public override void AwakeInitialization()
        {
            right.onClick.AddListener(() => Set(current + 1));
            left.onClick.AddListener(() => Set(current - 1));
            button.Initialize();
        }

        public void Set(int value)
        {
            current = value < 0 ? go.Count - 1 : (value >= go.Count - 1 ? 0 : value);
            for (int i = 0; i < go.Count; i++)
            {
                go[i].SetActive(current == i);
            }
        }
    }
}

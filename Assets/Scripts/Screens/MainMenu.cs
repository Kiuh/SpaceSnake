using ScreensManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.MainMenu")]
    internal class MainMenu : MultipleButtonScreen
    {
        [SerializeField]
        private Button exit;

        private void Start()
        {
            exit.onClick.AddListener(() => Application.Quit());
        }
    }
}

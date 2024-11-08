using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    [AddComponentMenu("Scripts/Screens/Screens.PlanetButton")]
    internal class PlanetButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        public Button Button => button;

        [SerializeField]
        private Image starsOn;

        public void SetStars(int stars)
        {
            starsOn.fillAmount = stars / 3.0f;
        }
    }
}

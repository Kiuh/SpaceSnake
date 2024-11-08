using UnityEngine;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.Obstacle")]
    internal class Obstacle : MonoBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
    }
}

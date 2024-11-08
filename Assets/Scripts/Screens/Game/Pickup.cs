using UnityEngine;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.Pickup")]
    internal class Pickup : MonoBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
    }
}

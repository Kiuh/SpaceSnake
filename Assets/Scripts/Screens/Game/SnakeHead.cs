using System;
using UnityEngine;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.SnakeHead")]
    internal class SnakeHead : MonoBehaviour
    {
        public event Action TouchedObstacle;
        public event Action<Pickup> TouchedPickup;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Obstacle obstacle))
            {
                TouchedObstacle?.Invoke();
            }

            if (collision.TryGetComponent(out Pickup pickup))
            {
                TouchedPickup?.Invoke(pickup);
            }
        }
    }
}

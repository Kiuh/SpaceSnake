using UnityEngine;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.SnakeTail")]
    internal class SnakeTail : MonoBehaviour
    {
        public SnakeHead SnakeHead;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SnakeHead.OnTriggerEnter2D(collision);
        }
    }
}

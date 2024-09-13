using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.SnakeMover")]
    internal class SnakeMover : MonoBehaviour
    {
        [SerializeField]
        private float moveTime;

        [SerializeField]
        private RectTransform head;

        [SerializeField]
        private List<RectTransform> pickups;

        [SerializeField]
        private RectTransform leftUp;

        [SerializeField]
        private RectTransform rightDown;

        private RectTransform targetPickup;
        private Tween moveTween;

        public void StartMove()
        {
            StopMove();

            float x = Random.Range(leftUp.anchoredPosition3D.x, rightDown.anchoredPosition3D.x);
            float y = Random.Range(rightDown.anchoredPosition3D.y, leftUp.anchoredPosition3D.y);
            Vector2 pos = new(x, y);

            targetPickup = Instantiate(pickups[Random.Range(0, pickups.Count)], transform);
            targetPickup.anchoredPosition3D = pos;
            moveTween = head.DOAnchorPos3D(pos, moveTime).OnComplete(StartMove);
        }

        public void StopMove()
        {
            if (targetPickup != null)
            {
                Destroy(targetPickup.gameObject);
            }
            moveTween?.Kill();
        }
    }
}

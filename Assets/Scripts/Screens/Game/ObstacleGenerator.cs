using System.Collections.Generic;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.ObstacleGenerator")]
    internal class ObstacleGenerator : MonoBehaviour
    {
        [SerializeField]
        private Obstacle obstaclePrefab;
        private Obstacle obstacle;

        [SerializeField]
        private List<Sprite> sprites;

        [SerializeField]
        private float travelTime;

        [SerializeField]
        private RectTransform canvas;

        private Tween moveTween;
        public RectTransform RectTransform => (RectTransform)transform;
        private Vector2 StartPos => new(Random.Range(0, canvas.GetWidth()), 0);
        private Vector2 EndPos => new(Random.Range(0, canvas.GetWidth()), -canvas.GetHeight());

        public void StartGenerate()
        {
            if (obstacle != null)
            {
                Destroy(obstacle.gameObject);
            }

            obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Count)];
            obstacle.RectTransform.anchoredPosition3D = (Vector3)StartPos;
            obstacle.gameObject.SetActive(true);
            moveTween = obstacle
                .RectTransform.DOAnchorPos3D(EndPos, travelTime)
                .SetEase(Ease.InOutCubic);
            _ = moveTween.OnComplete(StartGenerate);
        }

        public void StopGenerate()
        {
            if (obstacle != null)
            {
                Destroy(obstacle.gameObject);
            }
            moveTween?.Kill();
        }
    }
}

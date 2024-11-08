using System.Collections.Generic;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.PickupGenerator")]
    internal class PickupGenerator : MonoBehaviour
    {
        [SerializeField]
        private Pickup pickupPrefab;

        [SerializeField]
        private List<Sprite> sprites;

        [SerializeField]
        private RectTransform canvas;

        [SerializeField]
        private float generationInterval;

        private List<Pickup> created = new();
        private Tween generationTween;

        private Vector2 RandomPos =>
            new(Random.Range(0, canvas.GetWidth()), Random.Range(0, -canvas.GetHeight()));

        public void StartGenerate()
        {
            Pickup pickup = Instantiate(pickupPrefab, transform);
            pickup.RectTransform.anchoredPosition = RandomPos;
            pickup.GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Count)];
            created.Add(pickup);
            generationTween = DOVirtual.DelayedCall(generationInterval, StartGenerate);
        }

        public void StopGenerate()
        {
            for (int i = 0; i < created.Count; i++)
            {
                if (created[i] != null && created[i].gameObject != null)
                {
                    Destroy(created[i].gameObject);
                }
            }
            created.Clear();
            generationTween?.Kill();
        }
    }
}

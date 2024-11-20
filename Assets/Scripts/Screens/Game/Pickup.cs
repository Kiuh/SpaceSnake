using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.Pickup")]
    internal class Pickup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI labelPrefab;
        public GameImpl game;
        public RectTransform RectTransform => (RectTransform)transform;

        private void OnDestroy()
        {
            TextMeshProUGUI go = Instantiate(labelPrefab, game.LablesParent);
            go.text = "+" + game.PlanetPointsAmount.ToString();
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = RectTransform.anchoredPosition;
            _ = go.DOFade(0, 2);
            _ = rt.DOAnchorPosY(rt.anchoredPosition.y + 20, 2)
                .OnComplete(() => GameObject.Destroy(go.gameObject));
        }
    }
}

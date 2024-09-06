using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    [AddComponentMenu("Scripts/Common/Common.EffectsSetter")]
    internal class EffectsSetter : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        private void Awake()
        {
            foreach (
                Button buttons in FindObjectsByType<Button>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                )
            )
            {
                buttons.onClick.AddListener(() => audioSource.Play());
            }
        }
    }
}

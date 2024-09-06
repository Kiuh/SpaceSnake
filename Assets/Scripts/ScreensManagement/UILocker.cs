using UnityEngine;

namespace ScreensManagement
{
    [AddComponentMenu("Scripts/ScreensManagement/ScreensManagement.UILocker")]
    internal class UILocker : MonoBehaviour
    {
        private static UILocker cachedInstance;
        public static UILocker Instance
        {
            get
            {
                if (cachedInstance == null)
                {
                    cachedInstance = FindAnyObjectByType<UILocker>(FindObjectsInactive.Include);
                }
                return cachedInstance;
            }
        }

        public static void Lock()
        {
            Instance.gameObject.SetActive(true);
        }

        public static void UnLock()
        {
            Instance.gameObject.SetActive(false);
        }
    }
}

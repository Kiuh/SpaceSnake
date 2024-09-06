using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    internal class CornerRounder : MonoBehaviour
    {
        private static readonly int prop_halfSize = Shader.PropertyToID("_halfSize");
        private static readonly int prop_radiuses = Shader.PropertyToID("_r");
        private static readonly int prop_rect2props = Shader.PropertyToID("_rect2props");

        private static readonly Vector2 wNorm = new(0.7071068f, -0.7071068f);
        private static readonly Vector2 hNorm = new(0.7071068f, 0.7071068f);

        [SerializeField]
        private bool independent = false;

        [SerializeField]
        private float dependedCorner = 40f;

        [SerializeField]
        private Vector4 independentCorners = new(40f, 40f, 40f, 40f);
        private Vector4 Corners => independent ? independentCorners : Vector4.one * dependedCorner;
        private Vector4 NormalizedCorners
        {
            get
            {
                float width = GetComponent<RectTransform>().GetWidth();
                float height = GetComponent<RectTransform>().GetHeight();

                Vector4 r =
                    new()
                    {
                        x = Corners.x.NormalizeAngleToSize(width, height),
                        y = Corners.y.NormalizeAngleToSize(width, height),
                        z = Corners.z.NormalizeAngleToSize(width, height),
                        w = Corners.w.NormalizeAngleToSize(width, height)
                    };
                return r;
            }
        }

        private Material material;

        [HideInInspector, SerializeField]
        private Vector4 rect2props;

        [HideInInspector, SerializeField]
        private MaskableGraphic image;

        private void OnValidate()
        {
            Validate();
            Refresh();
        }

        private void OnEnable()
        {
            Validate();
            Refresh();
        }

        private void OnDrawGizmos()
        {
            Validate();
            Refresh();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (enabled && material != null)
            {
                Refresh();
            }
        }

        private void OnDestroy()
        {
            image.material = null;
            _ = material.Destroy();
            image = null;
            material = null;
        }

        public void Validate()
        {
            if (material == null)
            {
                material = new Material(Shader.Find("UI/RoundedCorners/CornerRounder"));
            }

            if (image == null)
            {
                _ = TryGetComponent(out image);
            }

            if (image != null)
            {
                image.material = material;
            }
        }

        public void Refresh()
        {
            Rect rect = ((RectTransform)transform).rect;
            RecalculateProps(rect.size);
            material.SetVector(prop_rect2props, rect2props);
            material.SetVector(prop_halfSize, rect.size * 0.5f);
            material.SetVector(prop_radiuses, NormalizedCorners);
        }

        private void RecalculateProps(Vector2 size)
        {
            Vector2 aVec = new(size.x, -size.y + NormalizedCorners.x + NormalizedCorners.z);
            float halfWidth = Vector2.Dot(aVec, wNorm) * .5f;
            rect2props.z = halfWidth;
            Vector2 bVec = new(size.x, size.y - NormalizedCorners.w - NormalizedCorners.y);
            float halfHeight = Vector2.Dot(bVec, hNorm) * .5f;
            rect2props.w = halfHeight;
            Vector2 efVec = new(size.x - NormalizedCorners.x - NormalizedCorners.y, 0);
            Vector2 egVec = hNorm * Vector2.Dot(efVec, hNorm);
            Vector2 ePoint = new(NormalizedCorners.x - (size.x / 2), size.y / 2);
            Vector2 origin = ePoint + egVec + (wNorm * halfWidth) + (hNorm * -halfHeight);
            rect2props.x = origin.x;
            rect2props.y = origin.y;
        }
    }

    public static class Extensions
    {
        public static float GetWidth(this RectTransform rt)
        {
            float w = ((rt.anchorMax.x - rt.anchorMin.x) * Screen.width) + rt.sizeDelta.x;
            return w;
        }

        public static float GetHeight(this RectTransform rt)
        {
            float h = ((rt.anchorMax.y - rt.anchorMin.y) * Screen.height) + rt.sizeDelta.y;
            return h;
        }

        public static int NormalizeAngleToSize(this float val, float width, float height)
        {
            if (val <= 0)
            {
                val = 0;
                return (int)val;
            }

            float resW = width / val;
            float resH = height / val;

            if (resW < 2 || resH < 2)
            {
                int min = Mathf.Min((int)(width / 2), (int)(height / 2));
                val = min;
            }

            if (val <= 0)
            {
                val = 0;
            }

            return (int)val;
        }

        public static bool Destroy(this UnityEngine.Object unityObject)
        {
            try
            {
#if UNITY_EDITOR
                UnityEngine.Object.DestroyImmediate(unityObject);
#else
                UnityEngine.Object.Destroy(unityObject);
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

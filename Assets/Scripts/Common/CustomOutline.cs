using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Common
{
    [AddComponentMenu("Scripts/Common/Common.CustomOutline")]
    internal class CustomOutline : Shadow
    {
        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (!IsActive())
            {
                return;
            }

            List<UIVertex> uiVertices = ListPool<UIVertex>.Get();
            vertexHelper.GetUIVertexStream(uiVertices);

            int neededCapacity = uiVertices.Count * 5;
            if (uiVertices.Capacity < neededCapacity)
            {
                uiVertices.Capacity = neededCapacity;
            }

            int end = 0;
            void Apply(Vector2 effectDistance)
            {
                int start = end;
                end = uiVertices.Count;
                ApplyShadowZeroAlloc(
                    uiVertices,
                    effectColor,
                    start,
                    uiVertices.Count,
                    effectDistance.x,
                    effectDistance.y
                );
            }

            Apply(effectDistance);
            Apply(effectDistance * new Vector2(1, -1));
            Apply(effectDistance * new Vector2(-1, 1));
            Apply(effectDistance * new Vector2(-1, -1));
            Apply(effectDistance * new Vector2(-1, 0));
            Apply(effectDistance * new Vector2(1, 0));
            Apply(effectDistance * new Vector2(0, 1));
            Apply(effectDistance * new Vector2(0, -1));

            vertexHelper.Clear();
            vertexHelper.AddUIVertexTriangleStream(uiVertices);
            ListPool<UIVertex>.Release(uiVertices);
        }
    }
}

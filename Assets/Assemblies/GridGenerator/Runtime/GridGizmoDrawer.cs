using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ArmyClash.Grid
{
    public static class GridGizmoDrawer
    {
        public static void Draw(GridConfig config, Vector3 origin, Vector3 rightAxis, Vector3 forwardAxis,
            Quaternion rotation, Color gizmoColor)
        {
            if (config == null)
            {
                return;
            }

            GridGlobalSettings settings = GridGlobalSettings.Instance;
            if (settings != null && !settings.ShowGizmos)
            {
                return;
            }

            IReadOnlyList<GridSlot> slots = GridLayoutBuilder.Build(config, origin, rightAxis, forwardAxis, rotation);
            Color color = gizmoColor;
            float height = 0.02f;
            float sizeScale = 1f;

            if (settings != null)
            {
                if (settings.OverrideGizmoColor)
                {
                    color = settings.GizmoColor;
                }

                height = Mathf.Max(0.001f, settings.GizmoHeight);
                sizeScale = Mathf.Max(0.01f, settings.GizmoSizeScale);
            }

            float size = Mathf.Max(0.05f, config.CellSize) * sizeScale;
            Vector3 cubeSize = new(size, height, size);
            float lineWidth = settings != null ? settings.LinePixelWidth : 2f;

#if UNITY_EDITOR
            Handles.color = color;
#else
            Gizmos.color = color;
#endif

            for (int i = 0; i < slots.Count; i++)
            {
                DrawCell(slots[i].Position, cubeSize, color, lineWidth);
            }
        }

        private static void DrawCell(Vector3 center, Vector3 size, Color color, float lineWidth)
        {
            float halfX = size.x * 0.5f;
            float halfZ = size.z * 0.5f;
            Vector3 p0 = new(center.x - halfX, center.y, center.z - halfZ);
            Vector3 p1 = new(center.x + halfX, center.y, center.z - halfZ);
            Vector3 p2 = new(center.x + halfX, center.y, center.z + halfZ);
            Vector3 p3 = new(center.x - halfX, center.y, center.z + halfZ);

#if UNITY_EDITOR
            Handles.DrawAAPolyLine(Mathf.Max(0.5f, lineWidth), p0, p1, p2, p3, p0);
#else
            Gizmos.color = color;
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p0);
#endif
        }
    }
}

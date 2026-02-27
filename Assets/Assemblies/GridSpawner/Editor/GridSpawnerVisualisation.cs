#if UNITY_EDITOR
using ArmyClash.Grid;
using UnityEditor;
using UnityEngine;

namespace VladislavTsurikov.MegaWorld.Editor.GridSpawner
{
    public static class GridSpawnerVisualisation
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawGizmoForArea(Runtime.GridSpawner.GridSpawner spawner, GizmoType gizmoType)
        {
            GridConfig config = spawner.Config;
            if (config == null)
            {
                return;
            }

            GridGizmoDrawer.Draw(config, spawner.transform.position, spawner.transform.right,
                spawner.transform.forward, spawner.transform.rotation);
        }
    }
}
#endif

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ArmyClash.Grid
{
    public static class GridGlobalSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider SettingsGUI()
        {
            SettingsProvider provider = new("Preferences/GridGenerator", SettingsScope.User)
            {
                label = "GridGenerator",
                guiHandler = _ =>
                {
                    GridGlobalSettings settings = GridGlobalSettings.Instance;
                    if (settings == null)
                    {
                        EditorGUILayout.HelpBox("GridGlobalSettings asset is missing.", MessageType.Warning);
                        return;
                    }

                    settings.ShowGizmos = EditorGUILayout.Toggle("Show Grid Gizmos", settings.ShowGizmos);
                    settings.GizmoColor = EditorGUILayout.ColorField("Gizmo Color", settings.GizmoColor);
                    settings.GizmoHeight = EditorGUILayout.FloatField("Gizmo Height", settings.GizmoHeight);
                    settings.GizmoSizeScale = EditorGUILayout.FloatField("Gizmo Size Scale", settings.GizmoSizeScale);
                    settings.LinePixelWidth = EditorGUILayout.FloatField("Line Pixel Width", settings.LinePixelWidth);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(settings);
                    }
                },
                keywords = new HashSet<string>(new[] { "Grid", "Gizmo" })
            };

            return provider;
        }
    }
}
#endif

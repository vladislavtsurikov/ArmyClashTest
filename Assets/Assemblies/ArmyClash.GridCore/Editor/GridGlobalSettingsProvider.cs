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
            var provider = new SettingsProvider("Preferences/Army Clash/Grid", SettingsScope.User)
            {
                label = "Army Clash Grid",
                guiHandler = _ =>
                {
                    GridGlobalSettings settings = GridGlobalSettings.Instance;
                    if (settings == null)
                    {
                        EditorGUILayout.HelpBox("GridGlobalSettings asset is missing.", MessageType.Warning);
                        return;
                    }

                    settings.ShowGizmos = EditorGUILayout.Toggle("Show Grid Gizmos", settings.ShowGizmos);
                    settings.OverrideGizmoColor = EditorGUILayout.Toggle("Override Gizmo Color", settings.OverrideGizmoColor);

                    using (new EditorGUI.DisabledScope(!settings.OverrideGizmoColor))
                    {
                        settings.GizmoColor = EditorGUILayout.ColorField("Gizmo Color", settings.GizmoColor);
                    }

                    settings.GizmoHeight = EditorGUILayout.FloatField("Gizmo Height", settings.GizmoHeight);
                    settings.GizmoSizeScale = EditorGUILayout.FloatField("Gizmo Size Scale", settings.GizmoSizeScale);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(settings);
                    }
                },
                keywords = new HashSet<string>(new[] { "Army", "Clash", "Grid", "Gizmo" })
            };

            return provider;
        }
    }
}
#endif

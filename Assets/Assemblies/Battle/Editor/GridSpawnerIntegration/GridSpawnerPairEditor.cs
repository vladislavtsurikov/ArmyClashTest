#if UNITY_EDITOR
using ArmyClash.MegaWorldGrid;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;

namespace ArmyClash.Battle.Editor.GridSpawnerIntegration
{
    [CustomEditor(typeof(GridSpawnerPair))]
    public sealed class GridSpawnerPairEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new();
        private GridSpawnerPair _pair;

        public override void OnInspectorGUI()
        {
            _pair ??= target as GridSpawnerPair;
            if (_pair == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_pair);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_pair, fieldsRect);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_pair);
            }
        }
    }
}
#endif

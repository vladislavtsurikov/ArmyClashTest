#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;

namespace ArmyClash.MegaWorldGrid.Editor
{
    [CustomEditor(typeof(GridSpawnerPairSetup))]
    public sealed class GridSpawnerPairSetupEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new IMGUIInspectorFieldsDrawer();

        public override void OnInspectorGUI()
        {
            float height = _fieldsDrawer.GetFieldsHeight(target);
            var rect = GUILayoutUtility.GetRect(0f, height, GUILayout.ExpandWidth(true));
            _fieldsDrawer.DrawFields(target, rect);

            GUILayout.Space(6f);
            if (GUILayout.Button("Create Spawners"))
            {
                var setup = (GridSpawnerPairSetup)target;
                if (setup != null)
                {
                    setup.CreateSpawners();
                    EditorUtility.SetDirty(setup);
                }
            }

            GUILayout.Space(4f);
            if (GUILayout.Button("Apply Transform"))
            {
                var setup = (GridSpawnerPairSetup)target;
                if (setup != null)
                {
                    setup.ApplyTransforms();
                    EditorUtility.SetDirty(setup);
                }
            }
        }
    }
}
#endif

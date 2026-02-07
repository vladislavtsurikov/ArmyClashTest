#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;

namespace VladislavTsurikov.ActionFlow.Editor.Stats
{
    [CustomEditor(typeof(Runtime.Stats.Entity))]
    public sealed class EntityEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new IMGUIInspectorFieldsDrawer();
        private Runtime.Stats.Entity _entity;

        public override void OnInspectorGUI()
        {
            _entity ??= target as Runtime.Stats.Entity;
            if (_entity == null)
            {
                return;
            }

            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_entity);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_entity, fieldsRect);
        }
    }
}
#endif

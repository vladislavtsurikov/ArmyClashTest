#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;

namespace VladislavTsurikov.ActionFlow.Editor.Stats
{
    [CustomEditor(typeof(Runtime.Stats.StatCollection))]
    public sealed class StatCollectionEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new IMGUIInspectorFieldsDrawer();
        private Runtime.Stats.StatCollection _collection;

        public override void OnInspectorGUI()
        {
            _collection ??= target as Runtime.Stats.StatCollection;
            if (_collection == null)
            {
                return;
            }

            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_collection);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_collection, fieldsRect);
        }
    }
}
#endif

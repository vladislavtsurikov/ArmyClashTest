#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.ActionFlow.Editor.Stats
{
    [CustomEditor(typeof(Runtime.Stats.Stat))]
    public sealed class StatEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new IMGUIInspectorFieldsDrawer();
        private ReorderableListStackEditor<Node, ReorderableListComponentEditor> _stackEditor;
        private Runtime.Stats.Stat _stat;

        public override void OnInspectorGUI()
        {
            _stat ??= target as Runtime.Stats.Stat;
            if (_stat == null)
            {
                return;
            }

            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_stat);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_stat, fieldsRect);

            EnsureStackEditor();
            _stackEditor.OnGUI();
        }

        private void EnsureStackEditor()
        {
            if (_stat.ComponentStack == null)
            {
                return;
            }

            if (_stackEditor != null && ReferenceEquals(_stackEditor.Stack, _stat.ComponentStack))
            {
                return;
            }

            _stackEditor = new ReorderableListStackEditor<Node, ReorderableListComponentEditor>(_stat.ComponentStack)
            {
                AllowedNamePrefixes = new[] { "Stats/" },
                DisplayHeaderText = true
            };
        }
    }
}
#endif

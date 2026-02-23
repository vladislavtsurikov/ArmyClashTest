#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.ActionFlow.Editor.Modifier
{
    [CustomEditor(typeof(Runtime.Modifier.Modifier), true)]
    public sealed class ModifierEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new();
        private Runtime.Modifier.Modifier _modifier;
        private ReorderableListStackEditor<ComponentData, ReorderableListComponentEditor> _stackEditor;

        public override void OnInspectorGUI()
        {
            _modifier ??= target as Runtime.Modifier.Modifier;
            if (_modifier == null)
            {
                return;
            }

            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_modifier);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_modifier, fieldsRect);

            EnsureStackEditor();
            _stackEditor.OnGUI();
        }

        private void EnsureStackEditor()
        {
            if (_modifier.ComponentStack == null)
            {
                return;
            }

            if (_stackEditor != null && ReferenceEquals(_stackEditor.Stack, _modifier.ComponentStack))
            {
                return;
            }

            _stackEditor =
                new ReorderableListStackEditor<ComponentData, ReorderableListComponentEditor>(_modifier.ComponentStack)
                {
                    AllowedGroupAttributes = new[] { "CommonUI" }, DisplayHeaderText = true
                };
        }
    }
}
#endif

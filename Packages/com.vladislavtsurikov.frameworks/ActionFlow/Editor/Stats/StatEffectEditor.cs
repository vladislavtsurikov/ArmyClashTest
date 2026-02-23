#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.ActionFlow.Editor.Stats
{
    [CustomEditor(typeof(StatEffect))]
    public sealed class StatEffectEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new();
        private ReorderableListStackEditor<ComponentData, ReorderableListComponentEditor> _stackEditor;
        private StatEffect _statEffect;

        public override void OnInspectorGUI()
        {
            _statEffect ??= target as StatEffect;
            if (_statEffect == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_statEffect);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_statEffect, fieldsRect);

            EnsureStackEditor();
            _stackEditor.OnGUI();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_statEffect);
            }
        }

        private void EnsureStackEditor()
        {
            if (_statEffect.ComponentStack == null)
            {
                return;
            }

            if (_stackEditor != null && ReferenceEquals(_stackEditor.Stack, _statEffect.ComponentStack))
            {
                return;
            }

            _stackEditor =
                new ReorderableListStackEditor<ComponentData, ReorderableListComponentEditor>(
                    _statEffect.ComponentStack)
                {
                    AllowedGroupAttributes = new[] { "Stats", "CommonUI" }, DisplayHeaderText = true
                };
        }
    }
}
#endif

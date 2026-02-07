#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Editor.IMGUI;

namespace VladislavTsurikov.ActionFlow.Editor.Stats
{
    [CustomEditor(typeof(Runtime.Stats.StatEffect))]
    public sealed class StatEffectEditor : UnityEditor.Editor
    {
        private readonly IMGUIInspectorFieldsDrawer _fieldsDrawer = new IMGUIInspectorFieldsDrawer();
        private Runtime.Stats.StatEffect _effect;

        public override void OnInspectorGUI()
        {
            _effect ??= target as Runtime.Stats.StatEffect;
            if (_effect == null)
            {
                return;
            }

            float fieldsHeight = _fieldsDrawer.GetFieldsHeight(_effect);
            Rect fieldsRect = EditorGUILayout.GetControlRect(false, fieldsHeight);
            _fieldsDrawer.DrawFields(_effect, fieldsRect);
        }
    }
}
#endif

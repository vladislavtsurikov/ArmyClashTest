#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.Core.Editor;

namespace VladislavTsurikov.MegaWorld.Editor.PhysicsEffectsTool.PhysicsEffectsSystem.GUI
{
    [ElementEditor(typeof(BlackHole))]
    public class BlackHoleEditor : PhysicsEffectEditor
    {
        private BlackHole _settings;

        public override void OnEnable() => _settings = (BlackHole)Target;

        protected override void OnPhysicsEffectGUI() =>
            _settings.Force =
                EditorGUILayout.Slider(new GUIContent("Force"), _settings.Force, 0, _settings.MaxForce);
    }
}
#endif

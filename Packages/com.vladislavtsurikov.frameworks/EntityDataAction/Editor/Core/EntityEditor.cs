#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Editor.Core
{
    [CustomEditor(typeof(EntityMonoBehaviour), true)]
    public sealed class EntityEditor : UnityEditor.Editor
    {
        private ActionReorderableListStackEditor _actionsEditor;

        private ReorderableListStackEditor<ComponentData, ReorderableListComponentEditor> _dataEditor;
        private EntityMonoBehaviour _entity;

        private void OnEnable()
        {
            _entity = (EntityMonoBehaviour)target;

            if (!_entity.IsSetup && !Application.isPlaying)
            {
                _entity.Entity.Setup();
            }

            _dataEditor = new ReorderableListStackEditor<ComponentData, ReorderableListComponentEditor>(
                new GUIContent("Data"), _entity.Data, true);
            _dataEditor.ShowActiveToggle = false;

            _actionsEditor = new ActionReorderableListStackEditor(_entity.Actions, _entity.Data);
        }

        public override void OnInspectorGUI()
        {
            DrawDirtyRunnerButton();

            EditorGUI.BeginChangeCheck();

            GUILayout.Space(3);
            bool isDerived = _entity.GetType() != typeof(EntityMonoBehaviour);
            if (isDerived)
            {
                _dataEditor.DisplayPlusButton = false;
                _dataEditor.DuplicateSupport = false;
                _dataEditor.RemoveSupport = false;
                _dataEditor.ReorderSupport = false;

                _actionsEditor.DisplayPlusButton = false;
                _actionsEditor.DuplicateSupport = false;
                _actionsEditor.RemoveSupport = false;
                _actionsEditor.ReorderSupport = false;
            }
            else
            {
                _dataEditor.DisplayPlusButton = true;
                _dataEditor.DuplicateSupport = true;
                _dataEditor.RemoveSupport = true;
                _dataEditor.ReorderSupport = true;

                _actionsEditor.DisplayPlusButton = true;
                _actionsEditor.DuplicateSupport = true;
                _actionsEditor.RemoveSupport = true;
                _actionsEditor.ReorderSupport = true;
            }

            _dataEditor.OnGUI();

            GUILayout.Space(3);
            _actionsEditor.OnGUI();

            if (EditorGUI.EndChangeCheck())
            {
                SetDirtyTarget();
            }
        }

        private void DrawDirtyRunnerButton()
        {
            DirtyActionRunner runner = _entity.Entity.DirtyRunner;
            if (runner == null)
            {
                return;
            }

            bool enabled = _entity.LocalActive;

            Color prev = GUI.color;
            GUI.color = enabled ? new Color(0.2f, 0.8f, 0.2f, 1f) : new Color(0.8f, 0.2f, 0.2f, 1f);

            string label = enabled ? "Active" : "Inactive";
            if (GUILayout.Button(label))
            {
                _entity.LocalActive = !enabled;
                SetDirtyTarget();
            }

            GUI.color = prev;

#if UNITY_EDITOR
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                EditorGUILayout.HelpBox("Inactive in Prefab Mode.", MessageType.Info);
            }
#endif
        }

        private void SetDirtyTarget()
        {
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
#endif

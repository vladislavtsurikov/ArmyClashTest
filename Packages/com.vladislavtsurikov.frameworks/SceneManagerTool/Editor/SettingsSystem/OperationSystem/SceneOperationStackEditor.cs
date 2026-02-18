#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime;
using VladislavTsurikov.AttributeUtility.Runtime;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.SceneManagerTool.Runtime.SettingsSystem;
using VladislavTsurikov.SceneManagerTool.Runtime.SettingsSystem.OperationSystem;
using Action = VladislavTsurikov.ActionFlow.Runtime.Actions.Action;

namespace VladislavTsurikov.SceneManagerTool.Editor.SettingsSystem.OperationSystem
{
    public class SceneOperationStackEditor : ReorderableListStackEditor<Action, ReorderableListComponentEditor>
    {
        private readonly ActionCollection _actionCollection;
        private readonly SettingsTypes _settingsTypes;

        public SceneOperationStackEditor(SettingsTypes settingsTypes, ActionCollection list) :
            base(new GUIContent("Actions"), list, true)
        {
            _actionCollection = list;
            _settingsTypes = settingsTypes;
            CopySettings = true;
            ShowActiveToggle = false;
        }

        protected override void ShowAddMenu()
        {
            var menu = new GenericMenu();

            foreach (Type settingsType in GetComponentTypes())
            {
                switch (_settingsTypes)
                {
                    case SettingsTypes.AfterLoadScene:
                        if (settingsType.GetAttribute<AfterLoadSceneComponentAttribute>() == null)
                        {
                            continue;
                        }

                        break;
                    case SettingsTypes.BeforeLoadScene:
                        if (settingsType.GetAttribute<BeforeLoadSceneComponentAttribute>() == null)
                        {
                            continue;
                        }

                        break;
                    case SettingsTypes.AfterUnloadScene:
                        if (settingsType.GetAttribute<AfterUnloadSceneComponentAttribute>() == null)
                        {
                            continue;
                        }

                        break;
                    case SettingsTypes.BeforeUnloadScene:
                        if (settingsType.GetAttribute<BeforeUnloadSceneComponentAttribute>() == null)
                        {
                            continue;
                        }

                        break;
                }

                var context = settingsType.GetAttribute<NameAttribute>().Name;

                menu.AddItem(new GUIContent(context), false,
                    () => _actionCollection.CreateNode(settingsType));
            }

            menu.ShowAsContext();
        }
    }
}
#endif
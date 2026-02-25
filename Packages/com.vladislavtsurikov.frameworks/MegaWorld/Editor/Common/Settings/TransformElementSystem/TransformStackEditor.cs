#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.AttributeUtility.Runtime;
using VladislavTsurikov.Nody.Editor.Core;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.TransformElementSystem;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.TransformElementSystem.Attributes;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Editor.Common.Settings.TransformElementSystem
{
    public class TransformStackEditor : ReorderableListStackEditor<TransformComponent, ReorderableListComponentEditor>
    {
        private readonly List<Type> _dontShowTransformTypes = new();
        private readonly bool _useSimpleComponent;

        public TransformStackEditor(GUIContent reorderableListName, TransformComponentStack list,
            List<Type> dontShowTransformTypes, bool useSimpleComponent) : base(reorderableListName, list, true)
        {
            DisplayHeaderText = false;
            _dontShowTransformTypes = dontShowTransformTypes;
            _useSimpleComponent = useSimpleComponent;
        }

        public TransformStackEditor(GUIContent reorderableListName, TransformComponentStack list,
            bool useSimpleComponent) : base(reorderableListName, list, true)
        {
            DisplayHeaderText = false;
            _useSimpleComponent = useSimpleComponent;
        }

        public TransformStackEditor(GUIContent reorderableListName, TransformComponentStack list) : base(
            reorderableListName, list, true)
        {
            DisplayHeaderText = false;
            _useSimpleComponent = false;
        }

        private NodeStackOnlyDifferentTypes<TransformComponent> ComponentStackOnlyDifferentTypes =>
            (NodeStackOnlyDifferentTypes<TransformComponent>)Stack;

        protected override bool PopulateMenu(string context, GenericMenu menu, Type settingsType)
        {
            if (_dontShowTransformTypes.Contains(settingsType))
            {
                return false;
            }

            if (_useSimpleComponent)
            {
                if (settingsType.GetAttribute<SimpleAttribute>() == null)
                {
                    return false;
                }
            }
            else
            {
                if (settingsType.GetAttribute<SimpleAttribute>() != null)
                {
                    context = "Simple/" + context;
                }
                else
                {
                    context = "Advanced/" + context;
                }
            }

            var exists = ComponentStackOnlyDifferentTypes.HasType(settingsType);

            if (!exists)
            {
                menu.AddItem(new GUIContent(context), false,
                    () => ComponentStackOnlyDifferentTypes.CreateIfMissingType(settingsType));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(context));
            }

            return true;
        }
    }
}
#endif

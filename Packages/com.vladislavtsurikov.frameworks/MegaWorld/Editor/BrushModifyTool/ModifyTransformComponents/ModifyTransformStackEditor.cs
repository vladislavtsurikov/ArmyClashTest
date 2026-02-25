#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.AttributeUtility.Runtime;
using VladislavTsurikov.Nody.Editor.Core;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Editor.BrushModifyTool.ModifyTransformComponents
{
    public class
        ModifyTransformStackEditor : ReorderableListStackEditor<ModifyTransformComponent,
        ReorderableListComponentEditor>
    {
        public ModifyTransformStackEditor(GUIContent label,
            NodeStackOnlyDifferentTypes<ModifyTransformComponent> stack) : base(label, stack, true) =>
            DisplayHeaderText = false;

        private NodeStackOnlyDifferentTypes<ModifyTransformComponent> ComponentStackOnlyDifferentTypes =>
            (NodeStackOnlyDifferentTypes<ModifyTransformComponent>)Stack;

        protected override bool PopulateMenu(string context, GenericMenu menu, Type settingsType)
        {
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

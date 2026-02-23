using System;
using System.Collections.Generic;
using System.Reflection;
using VladislavTsurikov.ReflectionUtility.Runtime;

namespace VladislavTsurikov.Nody.Runtime.Core
{
    public sealed class ContextHierarchy
    {
        public List<object> ContextHierarchyData { get; private set; }

        private readonly MethodInfo _buildMethod = typeof(ContextHierarchy).GetMethod(nameof(Build),
            BindingFlags.Instance | BindingFlags.NonPublic);

        internal void Build<T>(NodeStack<T> stack, List<object> contextHierarchy = null) where T : Node
        {
            if (stack == null)
            {
                return;
            }

            ContextHierarchyData = contextHierarchy ?? new List<object>();

            foreach (T element in stack.ElementList)
            {
                if (element == null)
                {
                    continue;
                }

                element.SetContextHierarchy(ContextHierarchyData);
                BuildForChildStacks(element, ContextHierarchyData);
            }
        }

        private void BuildForChildStacks(Node element, List<object> parentContext)
        {
            var fields = element.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fields == null || fields.Length == 0)
            {
                return;
            }

            var childContext = parentContext == null ? new List<object>() : new List<object>(parentContext);
            childContext.Add(element);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                Type fieldType = field.FieldType;
                if (!IsNodeStackType(fieldType))
                {
                    continue;
                }

                object stackValue = field.GetValue(element);
                if (stackValue == null)
                {
                    continue;
                }

                Type elementType = fieldType.TryGetGenericArgument(typeof(NodeStack<>));
                if (elementType == null)
                {
                    continue;
                }

                MethodInfo genericBuild = _buildMethod.MakeGenericMethod(elementType);
                var contextHierarchyProperty = fieldType.GetProperty("ContextHierarchy",
                    BindingFlags.Instance | BindingFlags.Public);
                if (contextHierarchyProperty == null)
                {
                    continue;
                }

                var childHierarchy = contextHierarchyProperty.GetValue(stackValue) as ContextHierarchy;
                if (childHierarchy == null)
                {
                    continue;
                }

                genericBuild.Invoke(childHierarchy, new object[] { stackValue, childContext });
            }
        }

        private static bool IsNodeStackType(Type type) =>
            type.TryGetGenericArgument(typeof(NodeStack<>)) != null;

    }
}

using UnityEngine;
using VladislavTsurikov.CustomInspector.Runtime;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [Persistent]
    [Name("Stats/Value")]
    public sealed class StatValueComponent : Node
    {
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public bool ClampEnabled { get; private set; }
        [field: SerializeField, ShowIf(nameof(ClampEnabled), true)] public bool UseMin { get; private set; }
        [field: SerializeField, ShowIf(nameof(UseMin), true)] public float MinValue { get; private set; }
        [field: SerializeField, ShowIf(nameof(ClampEnabled), true)] public bool UseMax { get; private set; }
        [field: SerializeField, ShowIf(nameof(UseMax), true)] public float MaxValue { get; private set; }
    }
}

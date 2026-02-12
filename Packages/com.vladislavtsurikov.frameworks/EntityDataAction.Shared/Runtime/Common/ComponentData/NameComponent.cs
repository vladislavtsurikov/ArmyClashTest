using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.EntityDataAction.Shared.Runtime.Common
{
    [Name("Common/Name")]
    [Group("CommonUI")]
    public sealed class NameComponent : ComponentData
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}

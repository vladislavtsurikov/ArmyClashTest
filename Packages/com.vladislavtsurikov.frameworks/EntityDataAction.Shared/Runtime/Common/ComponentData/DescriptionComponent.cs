using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.EntityDataAction.Shared.Runtime.Common
{
    [Name("Common/Description")]
    [Group("CommonUI")]
    public sealed class DescriptionComponent : ComponentData
    {
        [field: SerializeField] public string Description { get; private set; }
    }
}

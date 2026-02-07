using UnityEngine;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [Name("Stats/UI/Name")]
    public sealed class StatNameComponent : Node
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}

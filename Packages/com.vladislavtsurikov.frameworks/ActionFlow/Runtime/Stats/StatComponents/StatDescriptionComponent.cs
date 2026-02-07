using UnityEngine;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [Name("Stats/UI/Description")]
    public sealed class StatDescriptionComponent : Node
    {
        [field: SerializeField] public string Description { get; private set; }
    }
}

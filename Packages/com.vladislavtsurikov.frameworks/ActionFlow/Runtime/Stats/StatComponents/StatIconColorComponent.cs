using UnityEngine;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [Name("Stats/UI/IconColor")]
    public sealed class StatIconColorComponent : Node
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Color Color { get; private set; } = Color.white;
    }
}

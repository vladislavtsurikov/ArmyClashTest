using System.Collections.Generic;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Modifier;

namespace ArmyClash.Battle.Modifiers
{
    public sealed class ModifierProvider : MonoBehaviour
    {
        [SerializeField]
        private List<Modifier> _modifiers = new();

        public IReadOnlyList<Modifier> Modifiers => _modifiers;
    }
}

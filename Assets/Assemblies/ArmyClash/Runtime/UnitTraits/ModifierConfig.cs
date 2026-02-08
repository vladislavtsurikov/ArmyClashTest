using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;

namespace ArmyClash.UnitTraits
{
    public abstract class ModifierConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private StatEffect _statEffect;

        public string Id => _id;
        public string DisplayName => _displayName;
        public StatEffect StatEffect => _statEffect;
    }
}

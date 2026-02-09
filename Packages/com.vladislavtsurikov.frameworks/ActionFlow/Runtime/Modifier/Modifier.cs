using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;

namespace VladislavTsurikov.ActionFlow.Runtime.Modifier
{
    public abstract class Modifier : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private StatEffect _statEffect;

        public string Id => _id;
        public string DisplayName => _displayName;
        public StatEffect StatEffect => _statEffect;
    }
}

using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.ActionFlow.Runtime.Modifier
{
    public abstract class Modifier : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private StatEffect _statEffect;
        [SerializeField]
        [HideInInspector]
        private ModifierComponentStack _componentStack = new();

        public string Id => _id;
        public string DisplayName => _displayName;
        public StatEffect StatEffect => _statEffect;
        public ModifierComponentStack ComponentStack => _componentStack;

        private void OnEnable()
        {
            _componentStack ??= new ModifierComponentStack();
            _componentStack.Setup(true, new object[] { this });
        }

        private void OnDisable()
        {
            _componentStack?.OnDisable();
        }
    }
}

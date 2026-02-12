using UnityEngine;
using VladislavTsurikov.Nody.Runtime.AdvancedNodeStack;
using VladislavTsurikov.Nody.Runtime.Core;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [CreateAssetMenu(menuName = "ActionFlow/Stats/Stat", fileName = "Stat")]
    public sealed class Stat : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        [HideInInspector]
        private StatsComponentStack _componentStack = new();

        public string Id => _id;
        public StatsComponentStack ComponentStack => _componentStack;

        private void OnEnable()
        {
            _componentStack ??= new StatsComponentStack();
            _componentStack.Setup(true, new object[] { this });
        }

        private void OnDisable()
        {
            _componentStack?.OnDisable();
        }
    }
}

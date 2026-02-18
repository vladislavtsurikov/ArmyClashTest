using System.Collections.Generic;
using UnityEngine;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [CreateAssetMenu(menuName = "Stats/Effect", fileName = "StatEffect")]
    public class StatEffect : ScriptableObject
    {
        [System.Serializable]
        public struct Entry
        {
            public Stat Stat;
            public float Delta;
        }

        [SerializeField] private List<Entry> _entries = new();
        [SerializeField]
        [HideInInspector]
        private StatsComponentStack _componentStack = new();

        public IReadOnlyList<Entry> Entries => _entries;
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

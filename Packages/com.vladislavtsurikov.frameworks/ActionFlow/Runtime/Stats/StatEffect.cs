using System.Collections.Generic;
using UnityEngine;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [CreateAssetMenu(menuName = "Stats/Effect", fileName = "StatEffect")]
    public sealed class StatEffect : ScriptableObject
    {
        [System.Serializable]
        public struct Entry
        {
            public Stat Stat;
            public float Delta;
        }

        [SerializeField] private List<Entry> _entries = new();

        public IReadOnlyList<Entry> Entries => _entries;
    }
}

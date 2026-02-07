using System;
using System.Collections.Generic;
using UnityEngine;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [CreateAssetMenu(menuName = "ActionFlow/Stats/Stat Modifier", fileName = "StatModifier")]
    public sealed class StatModifier : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public Stat Definition;
            public float Delta;
        }

        [SerializeField] private List<Entry> _entries = new List<Entry>();

        public IReadOnlyList<Entry> Entries => _entries;
    }
}

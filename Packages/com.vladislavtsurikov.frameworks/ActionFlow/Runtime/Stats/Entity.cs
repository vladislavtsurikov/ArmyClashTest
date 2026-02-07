using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [CreateAssetMenu(menuName = "ActionFlow/Stats/Entity", fileName = "Entity")]
    public sealed class Entity : ScriptableObject
    {
        [SerializeField] private StatCollection _stats = new();

        public IReadOnlyList<Stat> Stats => _stats.Stats;

        public bool AddStat(Stat stat)
        {
            if (stat == null || _stats.Stats.Contains(stat))
            {
                return false;
            }

            _stats.AddStat(stat);
            return true;
        }

        public bool RemoveStat(Stat stat) => _stats.RemoveStat(stat);

        public void ClearStats() => _stats.Clear();
    }
}

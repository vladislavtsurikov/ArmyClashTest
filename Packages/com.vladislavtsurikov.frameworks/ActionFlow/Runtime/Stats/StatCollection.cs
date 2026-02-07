using System.Collections.Generic;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [System.Serializable]
    public sealed class StatCollection
    {
        [UnityEngine.SerializeField] private List<Stat> _stats = new();

        public IReadOnlyList<Stat> Stats => _stats;

        public bool AddStat(Stat stat)
        {
            if (stat == null || _stats.Contains(stat))
            {
                return false;
            }

            _stats.Add(stat);
            return true;
        }

        public bool RemoveStat(Stat stat) => _stats.Remove(stat);

        public void Clear() => _stats.Clear();
    }
}

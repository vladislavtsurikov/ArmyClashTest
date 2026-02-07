using System.Collections.Generic;

namespace VladislavTsurikov.ActionFlow.Runtime.Stats
{
    [System.Serializable]
    public sealed class StatCollection
    {
        [UnityEngine.SerializeField] private List<StatValue> _stats = new();

        public IReadOnlyList<StatValue> Stats => _stats;

        public bool AddStat(StatValue stat)
        {
            if (stat == null || _stats.Contains(stat))
            {
                return false;
            }

            _stats.Add(stat);
            return true;
        }

        public bool RemoveStat(StatValue stat) => _stats.Remove(stat);

        public void Clear() => _stats.Clear();
    }
}

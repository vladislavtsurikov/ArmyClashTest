using OdinSerializer;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats
{
    [Name("Stats/StatsEntity")]
    public sealed class StatsEntityData : ComponentData
    {
        [OdinSerialize] private StatCollection _collection;
        [OdinSerialize] private System.Collections.Generic.List<RuntimeStat> _stats = new();

        public StatCollection Collection
        {
            get => _collection;
            set
            {
                if (_collection == value)
                {
                    return;
                }

                _collection = value;
                RebuildFromCollection();
                MarkDirty();
            }
        }

        public System.Collections.Generic.IReadOnlyList<RuntimeStat> Stats => _stats;

        public void RebuildFromCollection()
        {
            _stats.Clear();

            if (_collection == null)
            {
                return;
            }

            var sourceStats = _collection.Stats;
            if (sourceStats == null)
            {
                return;
            }

            for (int i = 0; i < sourceStats.Count; i++)
            {
                Stat stat = sourceStats[i];
                if (stat == null)
                {
                    continue;
                }

                float value = GetDefaultValue(stat);
                _stats.Add(new RuntimeStat(stat, value));
            }
        }

        public bool AddStat(Stat stat)
        {
            if (stat == null || TryGetRuntimeStat(stat, out _))
            {
                return false;
            }

            _stats.Add(new RuntimeStat(stat, GetDefaultValue(stat)));
            MarkDirty();
            return true;
        }

        public bool RemoveStat(Stat stat)
        {
            if (stat == null)
            {
                return false;
            }

            for (int i = 0; i < _stats.Count; i++)
            {
                if (_stats[i].Stat == stat)
                {
                    _stats.RemoveAt(i);
                    MarkDirty();
                    return true;
                }
            }

            return false;
        }

        public bool SetStatValue(Stat stat, float value)
        {
            if (!TryGetRuntimeStat(stat, out RuntimeStat runtimeStat))
            {
                return false;
            }

            runtimeStat.Value = ApplyClamp(stat, value);
            MarkDirty();
            return true;
        }

        public bool AddStatValue(Stat stat, float delta)
        {
            if (!TryGetRuntimeStat(stat, out RuntimeStat runtimeStat))
            {
                return false;
            }

            runtimeStat.Value = ApplyClamp(stat, runtimeStat.Value + delta);
            MarkDirty();
            return true;
        }

        public bool TryGetStatValue(Stat stat, out float value)
        {
            value = 0f;
            if (!TryGetRuntimeStat(stat, out RuntimeStat runtimeStat))
            {
                return false;
            }

            value = runtimeStat.Value;
            return true;
        }

        private bool TryGetRuntimeStat(Stat stat, out RuntimeStat runtimeStat)
        {
            runtimeStat = null;

            if (stat == null)
            {
                return false;
            }

            for (int i = 0; i < _stats.Count; i++)
            {
                if (_stats[i].Stat == stat)
                {
                    runtimeStat = _stats[i];
                    return true;
                }
            }

            return false;
        }

        private float GetDefaultValue(Stat stat)
        {
            StatValueComponent valueComponent = stat.ComponentStack.GetElement<StatValueComponent>();
            return valueComponent != null ? valueComponent.BaseValue : 0f;
        }

        private float ApplyClamp(Stat stat, float value)
        {
            StatValueComponent valueComponent = stat.ComponentStack.GetElement<StatValueComponent>();
            return valueComponent != null ? valueComponent.ApplyClamp(value) : value;
        }
    }
}

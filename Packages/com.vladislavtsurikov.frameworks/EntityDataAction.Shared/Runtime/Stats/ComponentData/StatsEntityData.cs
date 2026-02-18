using System;
using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
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

        public bool SetStatValue(Stat stat, float value)
        {
            if (!TryGetRuntimeStat(stat, out RuntimeStat runtimeStat))
            {
                return false;
            }

            float previous = runtimeStat.CurrentValue;
            runtimeStat.CurrentValue = ApplyClamp(stat, value);
            if (previous.Equals(runtimeStat.CurrentValue))
            {
                return false;
            }

            MarkDirty();
            return true;
        }

        public bool AddStatValue(Stat stat, float delta)
        {
            if (!TryGetRuntimeStat(stat, out RuntimeStat runtimeStat))
            {
                return false;
            }

            float previous = runtimeStat.CurrentValue;
            runtimeStat.CurrentValue = ApplyClamp(stat, runtimeStat.CurrentValue + delta);
            if (previous.Equals(runtimeStat.CurrentValue))
            {
                return false;
            }

            MarkDirty();
            return true;
        }

        public bool AddStatValueById(string id, float delta)
        {
            if (!TryGetRuntimeStatById(id, out RuntimeStat runtimeStat))
            {
                return false;
            }

            float previous = runtimeStat.CurrentValue;
            runtimeStat.CurrentValue = ApplyClamp(runtimeStat.Stat, runtimeStat.CurrentValue + delta);
            if (previous.Equals(runtimeStat.CurrentValue))
            {
                return false;
            }

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

            value = runtimeStat.CurrentValue;
            return true;
        }

        public bool TryGetStatValueById(string id, out float value)
        {
            value = 0f;
            if (!TryGetRuntimeStatByIdInternal(id, out RuntimeStat runtimeStat))
            {
                return false;
            }

            value = runtimeStat.CurrentValue;
            return true;
        }

        public bool TryGetRuntimeStatById(string id, out RuntimeStat runtimeStat)
        {
            return TryGetRuntimeStatByIdInternal(id, out runtimeStat);
        }

        public RuntimeStat GetRuntimeStatById(string id)
        {
            TryGetRuntimeStatByIdInternal(id, out RuntimeStat runtimeStat);
            return runtimeStat;
        }

        public IDisposable SubscribeToStatValue(string id, Action<float> handler)
        {
            if (handler == null)
            {
                return null;
            }

            if (!TryGetRuntimeStatByIdInternal(id, out RuntimeStat runtimeStat))
            {
                return null;
            }

            return runtimeStat.Value.Subscribe(handler);
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

        private bool TryGetRuntimeStatByIdInternal(string id, out RuntimeStat runtimeStat)
        {
            runtimeStat = null;

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            for (int i = 0; i < _stats.Count; i++)
            {
                Stat stat = _stats[i].Stat;
                if (stat != null && string.Equals(stat.Id, id, System.StringComparison.Ordinal))
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

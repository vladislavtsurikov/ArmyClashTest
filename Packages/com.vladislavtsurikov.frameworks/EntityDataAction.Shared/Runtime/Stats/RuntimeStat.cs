using System;
using OdinSerializer;
using VladislavTsurikov.ActionFlow.Runtime.Stats;

namespace VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats
{
    [Serializable]
    public sealed class RuntimeStat
    {
        [OdinSerialize] private Stat _stat;
        [OdinSerialize] private float _value;

        public Stat Stat => _stat;

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public RuntimeStat()
        {
        }

        public RuntimeStat(Stat stat, float value)
        {
            _stat = stat;
            _value = value;
        }
    }
}

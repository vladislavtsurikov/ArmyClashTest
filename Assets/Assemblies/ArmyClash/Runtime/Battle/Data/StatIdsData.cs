using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/StatIds")]
    public sealed class StatIdsData : ComponentData
    {
        [OdinSerialize] private string _healthId = "HP";
        [OdinSerialize] private string _attackId = "ATK";
        [OdinSerialize] private string _speedId = "SPEED";
        [OdinSerialize] private string _attackSpeedId = "ATKSPD";
        [OdinSerialize] private string _regenId = "REGEN";

        public string HealthId
        {
            get => _healthId;
            set
            {
                if (_healthId == value)
                {
                    return;
                }

                _healthId = value;
                MarkDirty();
            }
        }

        public string AttackId
        {
            get => _attackId;
            set
            {
                if (_attackId == value)
                {
                    return;
                }

                _attackId = value;
                MarkDirty();
            }
        }

        public string SpeedId
        {
            get => _speedId;
            set
            {
                if (_speedId == value)
                {
                    return;
                }

                _speedId = value;
                MarkDirty();
            }
        }

        public string AttackSpeedId
        {
            get => _attackSpeedId;
            set
            {
                if (_attackSpeedId == value)
                {
                    return;
                }

                _attackSpeedId = value;
                MarkDirty();
            }
        }

        public string RegenId
        {
            get => _regenId;
            set
            {
                if (_regenId == value)
                {
                    return;
                }

                _regenId = value;
                MarkDirty();
            }
        }
    }
}

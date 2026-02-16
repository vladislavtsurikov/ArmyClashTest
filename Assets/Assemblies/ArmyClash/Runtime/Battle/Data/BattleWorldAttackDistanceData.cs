using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/WorldAttackDistance")]
    public sealed class BattleWorldAttackDistanceData : ComponentData
    {
        [OdinSerialize] private float _attackRange = 1.2f;
        [OdinSerialize] private float _stopDistance = 0.1f;

        public float AttackRange
        {
            get => _attackRange;
            set
            {
                if (_attackRange.Equals(value))
                {
                    return;
                }

                _attackRange = value;
                MarkDirty();
            }
        }

        public float StopDistance
        {
            get => _stopDistance;
            set
            {
                if (_stopDistance.Equals(value))
                {
                    return;
                }

                _stopDistance = value;
                MarkDirty();
            }
        }
    }
}

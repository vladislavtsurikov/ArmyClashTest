using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/WorldAttackDistance")]
    public sealed class AttackDistanceData : ComponentData
    {
        private readonly ReactiveProperty<float> _attackRangeReactive = new();
        private readonly ReactiveProperty<float> _stopDistanceReactive = new();

        [OdinSerialize]
        private float _attackRange = 1.2f;

        [OdinSerialize]
        private float _stopDistance = 0.1f;

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
                _attackRangeReactive.Value = _attackRange;
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
                _stopDistanceReactive.Value = _stopDistance;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<float> AttackRangeReactive => _attackRangeReactive;
        public IReadOnlyReactiveProperty<float> StopDistanceReactive => _stopDistanceReactive;

        protected override void OnFirstSetupComponent(object[] setupData = null)
        {
            _attackRangeReactive.Value = _attackRange;
            _stopDistanceReactive.Value = _stopDistance;
        }
    }
}

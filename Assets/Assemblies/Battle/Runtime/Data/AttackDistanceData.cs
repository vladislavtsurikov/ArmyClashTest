using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/AttackDistanceData")]
    public sealed class AttackDistanceData : ComponentData
    {
        [OdinSerialize]
        private ReactiveProperty<float> _attackRange = new(1.2f);

        [OdinSerialize]
        private ReactiveProperty<float> _stopDistance = new(0.1f);

        public IReadOnlyReactiveProperty<float> AttackRange
        {
            get
            {
                _attackRange ??= new ReactiveProperty<float>();
                return _attackRange;
            }
        }

        public IReadOnlyReactiveProperty<float> StopDistance
        {
            get
            {
                _stopDistance ??= new ReactiveProperty<float>();
                return _stopDistance;
            }
        }
    }
}

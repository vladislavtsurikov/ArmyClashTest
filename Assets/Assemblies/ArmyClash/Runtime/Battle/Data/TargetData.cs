using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/Data/Target")]
    public sealed class TargetData : ComponentData
    {
        [OdinSerialize]
        private ReactiveProperty<BattleEntity> _target;

        public BattleEntity Target
        {
            get => EnsureTarget().Value;
            set
            {
                if (EnsureTarget().Value == value)
                {
                    return;
                }

                _target.Value = value;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<BattleEntity> TargetReactive => EnsureTarget();

        private ReactiveProperty<BattleEntity> EnsureTarget()
        {
            if (_target == null)
            {
                _target = new ReactiveProperty<BattleEntity>();
            }

            return _target;
        }
    }
}

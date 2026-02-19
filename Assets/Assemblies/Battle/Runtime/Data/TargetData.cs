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
        private ReactiveProperty<BattleEntity> _target = new();

        public BattleEntity Target
        {
            get => _target.Value;
            set
            {
                if (_target.Value == value)
                {
                    return;
                }

                _target.Value = value;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<BattleEntity> TargetReactive => _target;
    }
}

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

        public ReactiveProperty<BattleEntity> Target
        {
            get
            {
                _target ??= new ReactiveProperty<BattleEntity>();
                return _target;
            }
            set => _target = value;
        }
    }
}

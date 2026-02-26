using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/LifeData")]
    public sealed class LifeData : ComponentData
    {
        [OdinSerialize]
        private ReactiveProperty<bool> _isDead = new();

        public ReactiveProperty<bool> IsDead
        {
            get
            {
                _isDead ??= new ReactiveProperty<bool>();
                return _isDead;
            }
            set => _isDead = value;
        }
    }
}

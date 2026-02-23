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

        public bool IsDead
        {
            get => _isDead.Value;
            set
            {
                if (_isDead.Value == value)
                {
                    return;
                }

                _isDead.Value = value;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<bool> IsDeadReactive => _isDead;
    }
}

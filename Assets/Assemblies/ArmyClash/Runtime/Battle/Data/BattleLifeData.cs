using OdinSerializer;
using UniRx;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/LifeData")]
    public sealed class BattleLifeData : ComponentData
    {
        [OdinSerialize]
        private ReactiveProperty<bool> _isDead;

        public bool IsDead
        {
            get => EnsureIsDead().Value;
            set
            {
                if (EnsureIsDead().Value == value)
                {
                    return;
                }

                _isDead.Value = value;
                MarkDirty();
            }
        }

        public IReadOnlyReactiveProperty<bool> IsDeadReactive => EnsureIsDead();

        private ReactiveProperty<bool> EnsureIsDead()
        {
            if (_isDead == null)
            {
                _isDead = new ReactiveProperty<bool>();
            }

            return _isDead;
        }
    }
}

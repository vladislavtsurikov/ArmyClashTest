using OdinSerializer;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/LifeData")]
    public sealed class BattleLifeData : ComponentData
    {
        [OdinSerialize]
        private bool _isDead;

        public bool IsDead
        {
            get => _isDead;
            set
            {
                if (_isDead == value)
                {
                    return;
                }

                _isDead = value;
                MarkDirty();
            }
        }
    }
}

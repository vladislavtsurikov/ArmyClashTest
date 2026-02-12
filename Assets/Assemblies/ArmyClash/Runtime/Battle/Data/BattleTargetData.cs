using OdinSerializer;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/TargetData")]
    public sealed class BattleTargetData : ComponentData
    {
        [OdinSerialize]
        private BattleEntity _target;

        public BattleEntity Target
        {
            get => _target;
            set
            {
                if (_target == value)
                {
                    return;
                }

                _target = value;
                MarkDirty();
            }
        }
    }
}

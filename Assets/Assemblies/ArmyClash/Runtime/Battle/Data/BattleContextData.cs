using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/ContextData")]
    public sealed class BattleContextData : ComponentData
    {
        [OdinSerialize]
        private BattleWorldEntity _world;

        public BattleWorldEntity World
        {
            get => _world;
            set
            {
                if (_world == value)
                {
                    return;
                }

                _world = value;
                MarkDirty();
            }
        }
    }
}

using OdinSerializer;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/ContextData")]
    public sealed class BattleContextData : ComponentData
    {
        [OdinSerialize]
        private BattleController _controller;

        public BattleController Controller
        {
            get => _controller;
            set
            {
                if (_controller == value)
                {
                    return;
                }

                _controller = value;
                MarkDirty();
            }
        }
    }
}

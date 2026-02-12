using OdinSerializer;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Data
{
    public enum BattleSpeed
    {
        Normal,
        Fast
    }

    [Name("UI/ArmyClash/BattleSpeedData")]
    public sealed class BattleSpeedData : ComponentData
    {
        [OdinSerialize]
        private BattleSpeed _speed;

        public BattleSpeed Speed
        {
            get => _speed;
            set
            {
                if (_speed == value)
                {
                    return;
                }

                _speed = value;
                MarkDirty();
            }
        }
    }
}

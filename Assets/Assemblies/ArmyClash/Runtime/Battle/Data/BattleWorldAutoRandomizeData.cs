using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/WorldAutoRandomize")]
    public sealed class BattleWorldAutoRandomizeData : ComponentData
    {
        [OdinSerialize] private bool _autoRandomizeOnAwake = true;

        public bool AutoRandomizeOnAwake
        {
            get => _autoRandomizeOnAwake;
            set
            {
                if (_autoRandomizeOnAwake == value)
                {
                    return;
                }

                _autoRandomizeOnAwake = value;
                MarkDirty();
            }
        }
    }
}

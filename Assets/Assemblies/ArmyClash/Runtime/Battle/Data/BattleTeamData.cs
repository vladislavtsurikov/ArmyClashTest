using OdinSerializer;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Data
{
    [Name("Battle/TeamData")]
    public sealed class BattleTeamData : ComponentData
    {
        [OdinSerialize]
        private int _teamId;

        public int TeamId
        {
            get => _teamId;
            set
            {
                if (_teamId == value)
                {
                    return;
                }

                _teamId = value;
                MarkDirty();
            }
        }
    }
}

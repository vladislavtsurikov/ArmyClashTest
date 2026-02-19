using ArmyClash.Battle.Data;

namespace ArmyClash.Battle
{
    public sealed class BattleAllyEntity : BattleEntity
    {
        protected override void OnAfterCreateDataAndActions()
        {
            TeamData team = GetData<TeamData>();
            if (team != null)
            {
                team.TeamId = 0;
            }
        }
    }
}

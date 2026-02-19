using ArmyClash.Battle.Data;

namespace ArmyClash.Battle
{
    public sealed class BattleEnemyEntity : BattleEntity
    {
        protected override void OnAfterCreateDataAndActions()
        {
            TeamData team = GetData<TeamData>();
            if (team != null)
            {
                team.TeamId = 1;
            }
        }
    }
}

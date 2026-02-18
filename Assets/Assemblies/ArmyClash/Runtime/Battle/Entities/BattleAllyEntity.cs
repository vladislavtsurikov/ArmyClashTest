namespace ArmyClash.Battle
{
    public sealed class BattleAllyEntity : BattleEntity
    {
        protected override void OnAfterCreateDataAndActions()
        {
            var team = GetData<Data.TeamData>();
            if (team != null)
            {
                team.TeamId = 0;
            }
        }
    }
}

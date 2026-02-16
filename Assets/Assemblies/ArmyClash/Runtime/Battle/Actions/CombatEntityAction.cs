using ArmyClash.Battle.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;

namespace ArmyClash.Battle.Actions
{
    public abstract class CombatEntityAction : EntityMonoBehaviourAction
    {
        protected BattleEntity Self => Host as BattleEntity;

        protected BattleWorldEntity GetWorld()
        {
            var data = Get<BattleContextData>();
            return data != null ? data.World : null;
        }

        protected BattleWorldRosterAction GetRosterAction()
        {
            var world = GetWorld();
            return world != null ? world.GetAction<BattleWorldRosterAction>() : null;
        }

        protected BattleWorldStateAction GetStateAction()
        {
            var world = GetWorld();
            return world != null ? world.GetAction<BattleWorldStateAction>() : null;
        }

        protected bool IsDead()
        {
            var life = Get<BattleLifeData>();
            return life != null && life.IsDead;
        }

        protected StatsEntityData GetStats() => Get<StatsEntityData>();

        protected BattleStatIdsData GetStatIds() => Get<BattleStatIdsData>();
    }
}

using ArmyClash.Battle.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;

namespace ArmyClash.Battle.Actions
{
    public abstract class BattleEntityAction : EntityMonoBehaviourAction
    {
        protected BattleEntity Self => Host as BattleEntity;

        protected BattleController GetController()
        {
            var data = Get<BattleContextData>();
            return data != null ? data.Controller : null;
        }

        protected bool IsDead()
        {
            var life = Get<BattleLifeData>();
            return life != null && life.IsDead;
        }

        protected StatsEntityData GetStats() => Get<StatsEntityData>();
    }
}

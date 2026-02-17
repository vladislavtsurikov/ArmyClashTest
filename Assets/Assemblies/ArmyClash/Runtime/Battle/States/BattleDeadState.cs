using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/Dead")]
    public sealed class BattleDeadState : State
    {
        public BattleDeadState()
        {
            Priority = int.MaxValue;
        }

        protected override void Conditional()
        {
            var life = GetData<BattleLifeData>();
            var ids = GetData<BattleStatIdsData>();
            var stats = GetData<StatsEntityData>();
            var health = stats.GetRuntimeStatById(ids.HealthId);
            BindEligibility(life.IsDeadReactive
                .CombineLatest(health.Value, (isDead, hp) => isDead || hp <= 0f));
        }

        public override void Enter(Entity entity)
        {
            var life = entity.GetData<BattleLifeData>();
            if (!life.IsDead)
            {
                life.IsDead = true;
            }

            var battleEntity = GetBattleEntity(entity);
            entity.GetAction<BattleWorldStateAction>().HandleEntityDeath(battleEntity);
        }

        private static BattleEntity GetBattleEntity(Entity entity)
        {
            return (BattleEntity)entity.SetupData[0];
        }
    }
}

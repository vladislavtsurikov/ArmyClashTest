using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;
using Zenject;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/Dead")]
    public sealed class DeadState : State
    {
        [Inject]
        private BattleStateService _state;

        public DeadState()
        {
            Priority = int.MaxValue;
        }

        protected override void Conditional()
        {
            var life = GetData<LifeData>();
            var ids = GetData<StatIdsData>();
            var stats = GetData<StatsEntityData>();
            var health = stats.GetRuntimeStatById(ids.HealthId);

            BindEligibility(life.IsDeadReactive
                .CombineLatest(health.Value, (isDead, hp) => isDead || hp <= 0f));
        }

        public override void Enter(Entity entity)
        {
            var life = entity.GetData<LifeData>();
            if (!life.IsDead)
            {
                life.IsDead = true;
            }
        }
    }
}

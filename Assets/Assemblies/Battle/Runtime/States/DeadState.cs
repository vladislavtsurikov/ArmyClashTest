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
        private const string HealthId = "HP";

        [Inject]
        private BattleStateService _state;

        public DeadState() => Priority = int.MaxValue;

        protected override void Conditional()
        {
            LifeData life = GetData<LifeData>();
            StatsEntityData stats = GetData<StatsEntityData>();
            RuntimeStat health = stats.GetRuntimeStatById(HealthId);

            BindEligibility(life.IsDeadReactive
                .CombineLatest(health.Value, (isDead, hp) => isDead || hp <= 0f));
        }

        public override void Enter(Entity entity)
        {
            LifeData life = entity.GetData<LifeData>();
            if (!life.IsDead)
            {
                life.IsDead = true;
            }
        }
    }
}

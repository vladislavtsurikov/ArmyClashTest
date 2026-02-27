using ArmyClash.Battle.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Definitions;

namespace ArmyClash.Battle.States
{
    [Name("Battle/StateMachine/Dead")]
    public sealed class DeadState : State
    {
        private const string HealthId = "HP";

        public DeadState() => Priority = int.MaxValue;

        protected override bool Conditional()
        {
            LifeData life = GetData<LifeData>();
            StatsEntityData stats = GetData<StatsEntityData>();
            RuntimeStat health = stats.GetRuntimeStatById(HealthId);

            return life.IsDead.Value || health.Value.Value <= 0f;
        }

        protected override void Tick(float delta)
        {
            LifeData life = EntityMonoBehaviour.GetData<LifeData>();
            if (!life.IsDead.Value)
            {
                life.IsDead.Value = true;
            }
        }
    }
}

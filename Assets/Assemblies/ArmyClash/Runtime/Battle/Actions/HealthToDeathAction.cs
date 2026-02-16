using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(StatsEntityData), typeof(BattleStatIdsData), typeof(BattleLifeData))]
    [Name("Action/HealthToDeath")]
    public sealed class HealthToDeathAction : CombatEntityAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected override void OnEnable()
        {
            _subscriptions.Clear();
            var stats = GetStats();
            var ids = GetStatIds();
            var life = Get<BattleLifeData>();
            var runtimeStat = stats.GetRuntimeStatById(ids.HealthId);
            runtimeStat.Value
                .Subscribe(value => HandleHealthChanged(value, life))
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }

        private void HandleHealthChanged(float value, BattleLifeData life)
        {
            var stateAction = GetStateAction();
            if (!stateAction.IsRunning)
            {
                return;
            }

            if (life.IsDead)
            {
                return;
            }

            if (value <= 0f)
            {
                life.IsDead = true;
            }
        }
    }
}

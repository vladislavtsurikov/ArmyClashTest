using ArmyClash.Battle.Data;
using ArmyClash.Battle.States;
using UnityEngine;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Data;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(StatsEntityData), typeof(BattleStatIdsData), typeof(BattleLifeData), typeof(StateMachineData))]
    [Name("Battle/Actions/RegenHealth")]
    public sealed class RegenHealthAction : EntityMonoBehaviourAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private bool _canRegen;

        protected override void OnEnable()
        {
            _subscriptions.Clear();
            _canRegen = false;

            var entity = Entity;
            if (entity == null)
            {
                return;
            }

            var stateMachine = entity.GetData<StateMachineData>();
            var worldState = entity.GetAction<BattleWorldStateAction>();
            var life = entity.GetData<BattleLifeData>();
            if (stateMachine == null || worldState == null || life == null)
            {
                return;
            }

            worldState.IsRunningReactive
                .CombineLatest(stateMachine.CurrentStateReactive, life.IsDeadReactive,
                    (running, state, isDead) =>
                        running && !isDead && state != null && state is not BattlePausedState && state is not BattleDeadState)
                .Subscribe(canRegen => _canRegen = canRegen)
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
            _canRegen = false;
        }

        protected override void Update()
        {
            if (!_canRegen)
            {
                return;
            }

            var entity = Entity;
            if (entity == null)
            {
                return;
            }

            var stats = entity.GetData<StatsEntityData>();
            var ids = entity.GetData<BattleStatIdsData>();
            if (stats == null || ids == null)
            {
                return;
            }

            if (!stats.TryGetStatValueById(ids.RegenId, out float regen))
            {
                return;
            }

            if (regen <= 0f)
            {
                return;
            }

            if (!string.IsNullOrEmpty(ids.HealthId))
            {
                stats.AddStatValueById(ids.HealthId, regen * Time.deltaTime);
            }
        }
    }
}

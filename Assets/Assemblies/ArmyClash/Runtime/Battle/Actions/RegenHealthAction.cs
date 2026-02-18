using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using ArmyClash.Battle.States;
using UnityEngine;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Data;
using Zenject;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(StatsEntityData), typeof(StatIdsData), typeof(LifeData), typeof(StateMachineData))]
    [Name("Battle/Actions/RegenHealth")]
    public sealed class RegenHealthAction : EntityMonoBehaviourAction
    {
        [Inject]
        private BattleStateService _state;

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
            var life = entity.GetData<LifeData>();
            if (stateMachine == null || life == null)
            {
                return;
            }

            _state.SimulationStateReactive
                .CombineLatest(stateMachine.CurrentStateReactive, life.IsDeadReactive,
                    (simState, state, isDead) =>
                        simState == SimulationState.Running &&
                        !isDead &&
                        state != null &&
                        state is not IdleState &&
                        state is not DeadState)
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
            var ids = entity.GetData<StatIdsData>();
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

            stats.AddStatValueById(ids.HealthId, regen * Time.deltaTime);
        }
    }
}

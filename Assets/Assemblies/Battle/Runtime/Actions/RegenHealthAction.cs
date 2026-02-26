using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using ArmyClash.Battle.States;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.StateMachine.Runtime.Data;
using Zenject;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(StatsEntityData), typeof(LifeData), typeof(StateMachineData))]
    [Name("Battle/Actions/RegenHealth")]
    public sealed class RegenHealthAction : EntityMonoBehaviourAction
    {
        private const string RegenId = "REGEN";
        private const string HealthId = "HP";
        private readonly CompositeDisposable _subscriptions = new();

        [Inject]
        private BattleStateService _state;

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            StateMachineData stateMachine = Entity.GetData<StateMachineData>();
            LifeData life = Entity.GetData<LifeData>();

            StatsEntityData stats = Entity.GetData<StatsEntityData>();
            _state.SimulationStateReactive
                .CombineLatest(stateMachine.CurrentState, life.IsDead,
                    (simState, state, isDead) =>
                        simState == SimulationState.Running &&
                        !isDead &&
                        state != null &&
                        state is not IdleState &&
                        state is not DeadState)
                .DistinctUntilChanged()
                .Select(canRegen =>
                    canRegen
                        ? Observable.EveryUpdate()
                        : Observable.Empty<long>())
                .Switch()
                .Subscribe(_ => ApplyRegen(stats))
                .AddTo(_subscriptions);
        }

        protected override void OnDisable() => _subscriptions?.Clear();

        private void ApplyRegen(StatsEntityData stats)
        {
            float regen = stats.GetStatValueById(RegenId);
            if (regen > 0f)
            {
                stats.AddStatValueById(HealthId, regen * Time.deltaTime);
            }
        }
    }
}

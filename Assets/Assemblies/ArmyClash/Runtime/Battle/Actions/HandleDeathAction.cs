using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(LifeData))]
    [Name("Battle/Actions/HandleDeath")]
    public sealed class HandleDeathAction : EntityMonoBehaviourAction
    {
        [Inject]
        private BattleStateService _state;

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            var life = Entity.GetData<LifeData>();
            life.IsDeadReactive
                .Where(isDead => isDead)
                .Subscribe(_ => HandleDeath())
                .AddTo(_subscriptions);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }

        private void HandleDeath()
        {
            if (_state.SimulationState != SimulationState.Running)
            {
                return;
            }

            UnityEngine.Object.Destroy(Host.gameObject);

            _state.UnregisterEntity(Host);
        }
    }
}

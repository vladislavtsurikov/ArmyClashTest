using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using UniRx;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(LifeData))]
    [Name("Battle/Actions/HandleDeath")]
    public sealed class HandleDeathAction : EntityMonoBehaviourAction
    {
        private readonly CompositeDisposable _subscriptions = new();

        [Inject]
        private BattleStateService _state;

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            LifeData life = Entity.GetData<LifeData>();
            life.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ => HandleDeath())
                .AddTo(_subscriptions);
        }

        protected override void OnDisable() => _subscriptions?.Clear();

        private void HandleDeath()
        {
            Object.Destroy(EntityMonoBehaviour.gameObject);

            _state.UnregisterEntity(EntityMonoBehaviour);
        }
    }
}

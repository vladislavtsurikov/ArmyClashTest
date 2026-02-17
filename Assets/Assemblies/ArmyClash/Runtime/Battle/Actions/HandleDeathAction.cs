using ArmyClash.Battle.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(BattleLifeData))]
    [Name("Battle/Actions/HandleDeath")]
    public sealed class HandleDeathAction : EntityMonoBehaviourAction
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            var life = Entity.GetData<BattleLifeData>();
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
            var stateAction = Entity.GetAction<BattleWorldStateAction>();
            if (!stateAction.IsRunning)
            {
                return;
            }

            var battleEntity = (BattleEntity)Entity.SetupData[0];
            stateAction.HandleEntityDeath(battleEntity);
        }
    }
}
